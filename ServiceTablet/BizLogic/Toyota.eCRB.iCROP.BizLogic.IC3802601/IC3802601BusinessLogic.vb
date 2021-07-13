'-------------------------------------------------------------------------
'IC3802601BizLogic.vb
'-------------------------------------------------------------------------
'機能：ステータス送信
'補足：
'作成：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2019/08/14 NSK 小牟禮 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
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
Imports System.Text
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic.IC46203CN
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.DataAccess.IC3802601DataSetTableAdapters
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.DataAccess.IC3802601DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class IC3802601BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

#Region "システム設定名"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal As String = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' 国コード
    ''' </summary>
    Private Const SysCountryCode As String = "DIST_CD"

    ''' <summary>
    ''' 関連チップ送信フラグ
    ''' </summary>
    Private Const SysSendRelationStatus As String = "SEND_RELATION_STATUS"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const SysDateFormat As String = "DATE_FORMAT"

    ''' <summary>
    ''' 基幹連携URL（予約情報）
    ''' </summary>
    Private Const DlrSysLinkUrlStatusInfo As String = "LINK_URL_STATUS_INFO"

    ''' <summary>
    ''' ステータスコード変換フラグ
    ''' </summary>
    Private Const StatusCodeConvFlg As String = "STATUS_CD_CONV_FLG"

    ''' <summary>
    ''' SOAPバージョン判定値
    ''' </summary>
    Private Const SysSoapVersion As String = "LINK_SOAP_VERSION"

    '''<summary>
    ''' CDATA付与フラグ
    ''' </summary>
    Private Const SysCDataApdFlg As String = "CDATA_APD_FLG"

#End Region

#Region "ステータス送信関連"
    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageId As String = "IC45203"

    ''' <summary>
    ''' シーケンス番号採番に用いる日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeqNoNumberingFormat As String = "yyyyMMddHHmmss"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateFormat As String = "yyyy/MM/dd HH:mm:ss"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

#Region "ステータス送信処理結果コード"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Success As Integer = 0

    ''' <summary>
    ''' 基幹コードマップ設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDmsCodeMap As Integer = 1119

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSysEnv As Integer = 1121

    ''' <summary>
    ''' 販売店システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDlrEnv As Integer = 1150

    ''' <summary>
    ''' 入力チェックエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorInputCheck As Integer = 6101

    ''' <summary>
    ''' 通信エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorNetwork As Integer = 6102

    ''' <summary>
    ''' 基幹側での処理エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDms As Integer = 6103

    ''' <summary>
    ''' ログ登録エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorRegistrationLog As Integer = 6104

    ''' <summary>
    ''' DB接続エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDBConnect As Integer = 6105

    ''' <summary>
    ''' タイムアウト発生エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorTimeOut As Integer = 6010

    ''' <summary>
    ''' システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystem As Integer = 9999

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' DMS除外エラーの警告
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WarningOmitDmsError As Integer = -9000

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

#End Region


#Region "タグ名"

#Region "UpdateStatusタグ"

    ''' <summary>
    ''' タグ名：UpdateStatus
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagUpdateStatus As String = "UpdateStatus"

#Region "headタグ"

    ''' <summary>
    ''' タグ名：head
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagHead As String = "head"

    ''' <summary>
    ''' タグ名：MessageID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMessageID As String = "MessageID"

    ''' <summary>
    ''' タグ名：CountryCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCountryCode As String = "CountryCode"

    ''' <summary>
    ''' タグ名：LinkSystemCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagLinkSystemCode As String = "LinkSystemCode"

    ''' <summary>
    ''' タグ名：TransmissionDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTransmissionDate As String = "TransmissionDate"

#End Region

#Region "Detailタグ"

    ''' <summary>
    ''' タグ名：Detail
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDetail As String = "Detail"

#Region "Commonタグ"

    ''' <summary>
    ''' タグ名：Common
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCommon As String = "Common"

    ''' <summary>
    ''' タグ名：DealerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDealerCode As String = "DealerCode"

    ''' <summary>
    ''' タグ名：BranchCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBranchCode As String = "BranchCode"

    ''' <summary>
    ''' タグ名：StaffCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStaffCode As String = "StaffCode"

    ''' <summary>
    ''' タグ名：CustomerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerCode As String = "CustomerCode"

    ''' <summary>
    ''' タグ名：SalesBookingNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSalesBookingNumber As String = "SalesBookingNumber"

    ''' <summary>
    ''' タグ名：Vin
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVin As String = "Vin"

#End Region

#Region "StatusInformationタグ"

    ''' <summary>
    ''' タグ名：StatusInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStatusInformation As String = "StatusInformation"

    ''' <summary>
    ''' タグ名：SeqNo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSeqNo As String = "SeqNo"

    ''' <summary>
    ''' タグ名：REZID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezId As String = "REZID"

    ''' <summary>
    ''' タグ名：BASREZID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBasRezId As String = "BASREZID"

    ''' <summary>
    ''' タグ名：STATUS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStatus As String = "STATUS"

    ''' <summary>
    ''' タグ名：STARTTIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStartTime As String = "STARTTIME"

    ''' <summary>
    ''' タグ名：ENDTIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEndTime As String = "ENDTIME"

    ''' <summary>
    ''' タグ名：REZ_TIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezTime As String = "REZ_TIME"

    ''' <summary>
    ''' タグ名：WORKTIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWorkTime As String = "WORKTIME"

    ''' <summary>
    ''' タグ名：STALLID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallid As String = "STALLID"

    ''' <summary>
    ''' タグ名：BREAK
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBreak As String = "BREAK"

    ''' <summary>
    ''' タグ名：WAITTIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWaitTime As String = "WAITTIME"

    ''' <summary>
    ''' タグ名：INSPECTIONMEMO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TaginspectionMemo As String = "INSPECTIONMEMO"

#End Region

#End Region

#End Region

    ''' <summary>
    ''' タグ名：ResultId
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultId As String = "ResultId"

    ''' <summary>
    ''' タグ名：Message
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMessage As String = "Message"
#End Region

#End Region

#Region "その他"

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

    ''' <summary>
    ''' 関連チップ送信フラグ：送信する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendRelationChipFlg_Send As String = "1"

    ''' <summary>
    ''' ステータスコード変換フラグ：変換しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusCodeConvFlg_NotUse As String = "0"

    ''' <summary>
    ''' ステータスコード変換フラグ：変換する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusCodeConvFlg_Use As String = "1"

    ''' <summary>
    ''' ステータス送信フラグ：送信する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendStatus As String = "1"

    ''' <summary>
    ''' 定数：ゼロ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Zero As String = "0"

    ''' <summary>
    ''' 付けない
    ''' </summary>
    Private Const CData_None As String = "0"

    ''' <summary>
    ''' 付ける
    ''' </summary>
    Private Const CData_Append As String = "1"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' インターフェース区分：ステータス送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InterfaceTypeSendStatus As String = "2"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

#End Region

#Region "基幹コード区分"

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    Private Const DmsCodeTypeBranchCode As String = "2"

    ''' <summary>
    ''' ストールID
    ''' </summary>
    Private Const DmsCodeTypeStallId As String = "3"

    ''' <summary>
    ''' チップステータス
    ''' </summary>
    Private Const DmsCodeTypeStatus As String = "7"

#End Region

#Region "チップステータス"

    ''' <summary>
    ''' 未入庫(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNotCarIn As String = "1"

    ''' <summary>
    ''' 未入庫(本予約)
    ''' </summary>ChipStatusConfirmedNotCarIn
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedNotCarIn As String = "2"

    ''' <summary>
    ''' 作業開始待ち(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeWaitStart As String = "3"

    ''' <summary>
    ''' 作業開始待ち(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedWaitStart As String = "4"

    ''' <summary>
    ''' 仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTemp As String = "5"

    ''' <summary>
    ''' 未来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusNoShow As String = "6"

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
    ''' <remarks></remarks>
    Private Const ChipStatusStopForPartsStockout As String = "9"

    ''' <summary>
    ''' 作業中断：顧客連絡待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusStopForWaitCustomer As String = "10"

    ''' <summary>
    ''' 作業中断：ストール待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusStopForWaitStall As String = "11"

    ''' <summary>
    ''' 作業中断：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusStopForOtherReason As String = "12"

    ''' <summary>
    ''' 作業中断：検査中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusStopForInspection As String = "13"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWaitWash As String = "14"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWashing As String = "15"

    ''' <summary>
    '''  検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWaitInspection As String = "16"

    ''' <summary>
    '''  検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusInspecting As String = "17"

    ''' <summary>
    '''  預かり中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusKeeping As String = "18"

    ''' <summary>
    '''  納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWaitDelivery As String = "19"

    ''' <summary>
    '''  作業完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusJobFinish As String = "20"

    ''' <summary>
    '''  日跨ぎ終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusDateCrossEnd As String = "21"

    ''' <summary>
    '''  納車済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusDeliveryEnd As String = "22"

    ''' <summary>
    '''  仮置き(仮予約)
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Private Const ChipStatusTentativeTemp As String = "23"

    ''' <summary>
    '''  未来店客(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNoShow As String = "24"


#End Region

#Region "基幹チップステータス"
    ''' <summary>
    ''' 未入庫（仮予約）
    ''' </summary>
    Private Const DmsTentativeNotCarIn As String = "00"

    ''' <summary>
    ''' 未入庫（本予約）
    ''' </summary>
    Private Const DmsConfirmedNotCarIn As String = "01"

    ''' <summary>
    ''' 作業開始待ち（仮予約）
    ''' </summary>
    Private Const DmsTentativeWaitStart As String = "10"

    ''' <summary>
    ''' 作業開始待ち（本予約）
    ''' </summary>
    Private Const DmsConfirmedWaitStart As String = "11"

    ''' <summary>
    ''' 仮置き
    ''' </summary>
    Private Const DmsTemp As String = "32"

    ''' <summary>
    ''' 未来店客
    ''' </summary>
    Private Const DmsNoShow As String = "33"

    ''' <summary>
    ''' 飛び込み客
    ''' </summary>
    Private Const DmsWalkin As String = "7"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    Private Const DmsWorking As String = "20"

    ''' <summary>
    ''' 中断・部品欠品
    ''' </summary>
    Private Const DmsStopForPartsStockout As String = "30"

    ''' <summary>
    ''' 中断・お客様連絡待ち
    ''' </summary>
    Private Const DmsStopForWaitCustomer As String = "31"

    ''' <summary>
    ''' 中断・ストール待機
    ''' </summary>
    Private Const DmsStopForWaitStall As String = "38"

    ''' <summary>
    ''' 中断・その他
    ''' </summary>
    Private Const DmsStopForOtherReason As String = "39"

    ''' <summary>
    ''' 中断・検査中断
    ''' </summary>
    Private Const DmsStopForInspection As String = "44"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    Private Const DmsWaitWash As String = "40"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    Private Const DmsWashing As String = "41"

    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    Private Const DmsWaitInspection As String = "42"

    ''' <summary>
    ''' 検査中
    ''' </summary>
    Private Const DmsInspecting As String = "43"

    ''' <summary>
    ''' 預かり中
    ''' </summary>
    Private Const DmsKeeping As String = "50"

    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    Private Const DmsWaitDelivery As String = "60"

    ''' <summary>
    ''' 作業完了
    ''' </summary>
    Private Const DmsJobFinish As String = "97"

    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    Private Const DmsDateCrossEnd As String = "98"

    ''' <summary>
    ''' 納車済み
    ''' </summary>
    Private Const DmsDeliveryEnd As String = "99"
#End Region

#Region "ストール利用ステータス"

    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusWorkOrderWait As String = "00"

    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStartWait As String = "01"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusWorking As String = "02"

    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusFinish As String = "03"

    ''' <summary>
    ''' 作業計画の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStartIncludeStopJob As String = "04"

    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusJobStop As String = "05"

    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusNoShow As String = "07"

#End Region

#Region "サービスステータス"

    ''' <summary>
    ''' 未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNotCarIn As String = "00"

    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNoShow As String = "01"

    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorkOrderWait As String = "03"

    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStartWait As String = "04"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorking As String = "05"

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
    Private Const SvcStatusDelivered As String = "13"

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

#Region "受付区分"

    ''' <summary>
    ''' WalkIn
    ''' </summary>
    Private Const AcceptanceTypeWalkin As String = "1"

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

#End Region

#Region "公開メッソド"

#Region "基幹連携(ステータス送信処理)を行う(メイン)"
    ''' <summary>
    ''' 基幹連携(ステータス送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPrevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inCrntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inWaitTime">ストール待機時間</param>
    ''' <param name="inPrevResvStatus">更新前予約ステータス</param>
    ''' <param name="inCrntResvStatus">更新後予約ステータス</param>
    ''' <returns>ステータス送信処理結果コード(0:正常終了/-1:異常終了)</returns>
    ''' <remarks></remarks>
    Public Function SendStatusInfo(ByVal inSvcInId As Decimal, _
                                    ByVal inJobDtlId As Decimal, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inPrevStatus As String, _
                                    ByVal inCrntStatus As String, _
                                    ByVal inWaitTime As Long, _
                                    Optional ByVal inPrevResvStatus As String = "", _
                                    Optional ByVal inCrntResvStatus As String = "") As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:inSvcInId={1}, inJobDtlId={2}, inStallUseId={3}, inPrevStatus={4}, inCrntStatus={5}, inWaitTime={6}, inPrevResvStatus={7},  inCrntResvStatus={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId, _
                                  inStallUseId, _
                                  inPrevStatus, _
                                  inCrntStatus, _
                                  inWaitTime, _
                                  inPrevResvStatus, _
                                  inCrntResvStatus))
        '戻り値
        Dim retValue As Integer = Success

        Try

            'ステータス送信するかしないかのフラグを取得
            Dim sendFlg As String = Me.GetSendStatusFlg(Me.ConvertStatus(inPrevStatus, inPrevResvStatus), _
                                                        Me.ConvertStatus(inCrntStatus, inCrntResvStatus))

            If SendStatus.Equals(sendFlg) Then
                '送信する
                retValue = Me.SendStatusInfoDetails(inSvcInId, _
                                                     inJobDtlId, _
                                                     inStallUseId, _
                                                     inPrevStatus, _
                                                     inCrntStatus, _
                                                     inWaitTime)
            Else
                '送信しない(正常終了)
                retValue = Success
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:retValue={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      retValue))

        Catch oex As OracleExceptionEx

            'DBエラー
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDBConnect), oex)
            retValue = ErrorDBConnect

        Catch ex As Exception

            '異常終了
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorSystem), ex)
            retValue = ErrorSystem

        End Try

        Return retValue

    End Function

#End Region

#End Region

#Region "Privateメッソド"

#Region "ステータス送信実行"
    ''' <summary>
    ''' ステータス送信を実行する
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPrevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inCrntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inWaitTime">ストール待機時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendStatusInfoDetails(ByVal inSvcInId As Decimal, _
                                            ByVal inJobDtlId As Decimal, _
                                            ByVal inStallUseId As Decimal, _
                                            ByVal inPrevStatus As String, _
                                            ByVal inCrntStatus As String, _
                                            ByVal inWaitTime As Long) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:inSvcInId={1}, inJobDtlId={2}, inStallUseId={3}, inPrevStatus={4}, inCrntStatus={5}, inWaitTime={6}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId, _
                                  inStallUseId, _
                                  inPrevStatus, _
                                  inCrntStatus, _
                                  inWaitTime))

        '戻り値を初期化(正常終了)
        Dim retValue As Integer = Success

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

        'DMS除外エラーの警告フラグ(True：除外対象あり / False：除外対象のエラーなし、またはエラー自体なし)
        Dim warningOmitDmsErrorFlg As Boolean = False

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

        Try
            'ログインスタッフ情報取得
            Dim userContext As StaffContext = StaffContext.Current

            '現在日時取得
            Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)

            '**************************************************
            '* システム設定値を取得
            '**************************************************
            Dim systemSettingsValueRow As IC3802601SystemSettingValueRow _
                = Me.GetSystemSettingValues()

            '必要なシステム設定値が一つでも取得できない場合はエラー
            If IsNothing(systemSettingsValueRow) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Not setting value is present in the system settings.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

            'SMBCommonClassBusinessLogicインスタンスを生成
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic

                '**************************************************
                '* 基幹販売店コード、店舗コードを取得
                '**************************************************
                dmsDlrBrnTable = smbCommonBiz.GetIcropToDmsCode(userContext.DlrCD, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                                userContext.DlrCD, _
                                                                userContext.BrnCD, _
                                                                String.Empty)

                If dmsDlrBrnTable.Count <= 0 Then

                    'データが取得できない場合はエラー
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(No data found)", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDmsCodeMap))
                    retValue = ErrorSystem
                    Exit Try

                ElseIf 1 < dmsDlrBrnTable.Count Then

                    'データが2件以上取得できた場合は一意に決定できないためエラー
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(Non-unique)", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDmsCodeMap))
                    retValue = ErrorSystem
                    Exit Try

                End If


            End Using

            '上記処理で取得したデータテーブルからデータ行抜き出し
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = dmsDlrBrnTable.Item(0)

            '**************************************************
            '* 送信対象のチップ情報を全て取得
            '**************************************************
            Dim sendTargetChipInfoRow As IC3802601SendChipInfoDataTable _
                = Me.CreateXMLData(inSvcInId, _
                                inJobDtlId, _
                                inStallUseId, _
                                inWaitTime, _
                                inPrevStatus, _
                                inCrntStatus, _
                                DateFormat, _
                                systemSettingsValueRow)

            If IsNothing(sendTargetChipInfoRow) Then
                '送信対象のチップ情報の取得に失敗
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Err:Failed to get a chip info.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try
            End If

            '送信対象のチップ件数分のステータス送信を行う
            '送信項目フラグリストを取得する
            Dim sendItemList As List(Of Boolean) = JudgeSenddecision(inPrevStatus, inCrntStatus)
            If sendItemList.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.Error, Failed to get a senddecisionMap", _
                                MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try
            End If
            For Each sendTargetChip In sendTargetChipInfoRow


                '**************************************************
                '* 送信XMLの作成
                '**************************************************
                Dim sendXml As XmlDocument = Me.StructSendStatusXml(systemSettingsValueRow, _
                                                                    dmsDlrBrnRow, _
                                                                    sendTargetChip, _
                                                                    nowDateTime, _
                                                                    sendItemList)
                If IsNothing(sendXml) Then
                    '送信XMLの構築に失敗
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error Err:Failed to build XML for transmission.", _
                                               MethodBase.GetCurrentMethod.Name))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '**************************************************
                '* XMLの送受信処理
                '**************************************************
                Dim resultString As String = Me.ExecuteSendStatusInfoXml(sendXml.InnerXml, _
                                                                      systemSettingsValueRow.LINK_URL_STATUS_INFO, _
                                                                      systemSettingsValueRow.LINK_SEND_TIMEOUT_VAL, _
                                                                      systemSettingsValueRow.LINK_SOAP_VERSION)

                '結果文字列が空文字の場合、エラーとする(送受信処理でエラー発生)
                If String.IsNullOrEmpty(resultString) Then
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error Err:Received XML is empty.", _
                                               MethodBase.GetCurrentMethod.Name))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '受信XMLから必要な値を取得する
                Dim resultValueList As Dictionary(Of String, String) = Me.GetSendStatusXmlResultData(resultString)

                '受信XMLから必要な値を取得できない場合、エラーとする
                If IsNothing(resultValueList) Then
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error Err:Received XML is empty.", _
                                               MethodBase.GetCurrentMethod.Name))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '結果コードが0以外の場合、エラーとする
                If Not resultValueList.Item(TagResultId).Equals("0") Then
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error ErrorCode:{2}, ReceivedXmlContents:ResultId={3}, Message={4}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDms, _
                                               resultValueList.Item(TagResultId), _
                                               resultValueList.Item(TagMessage)))

                    '送信XMLのログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error SentXML = {2}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               sendXml.InnerXml))

                    '受信XMLのログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error ReceivedXML = {2}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               resultString))

                    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

                    'retValue = ErrorSystem
                    'Exit Try

                    Using svcCommonBiz As New ServiceCommonClassBusinessLogic

                        'DMSから返却されたエラーコードが除外対象かどうかを確認
                        If svcCommonBiz.IsOmitDmsErrorCode(InterfaceTypeSendStatus, _
                                                           resultValueList.Item(TagResultId)) Then

                            '除外対象であった場合
                            'DMS除外エラーの警告フラグをTrueに設定しておく
                            warningOmitDmsErrorFlg = True

                        Else
                            '除外対象でなかった場合
                            '9999(異常終了)を返却値に設定し、以降の処理中止
                            retValue = ErrorSystem
                            Exit Try

                        End If

                    End Using

                    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

                End If
            Next

            '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

            If retValue <> ErrorSystem Then
                '全ての処理を終えた時点で、返却値が9999(システムエラー)でない

                If warningOmitDmsErrorFlg Then
                    'DMS除外エラーの警告フラグがTrueである

                    '-9000(DMS除外エラーの警告)を返却値に設定
                    retValue = WarningOmitDmsError

                End If

            End If

            '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

        Finally
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:retValue={1}", _
                                      MethodBase.GetCurrentMethod.Name, retValue))
        End Try

        Return retValue

    End Function
#End Region

#Region "ステータス送信用XMLの送受信"

    ''' <summary>
    ''' WebServiceにXMLを送信し、結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <param name="timeOutValue">タイムアウト値</param>
    ''' <param name="soapVersion">SOAPバージョン判定値</param>
    ''' <returns>受信XML文字列</returns>
    ''' <remarks></remarks>
    Private Function ExecuteSendStatusInfoXml(ByVal sendXml As String, _
                                           ByVal webServiceUrl As String, _
                                           ByVal timeOutValue As String, _
                                           ByVal soapVersion As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:sendXml={1}, webServiceUrl={2}, timeOutValue={3}", _
                                  MethodBase.GetCurrentMethod.Name, sendXml, webServiceUrl, timeOutValue))

        'SoapClientインスタンスを生成
        Using service As New IC46203CN.IC46203CN

            service.Url = webServiceUrl
            service.Timeout = CType(timeOutValue, Integer)

            If soapVersion.Equals("1") Then
                'SOAPバージョンを1.1に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap11

            ElseIf soapVersion.Equals("2") Then
                'SOAPバージョンを1.2に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap12

            End If

            Dim resultString As String = String.Empty

            Try
                'XML送信
                resultString = service.IC45203(sendXml)

            Catch webEx As WebException

                If webEx.Status = WebExceptionStatus.Timeout Then
                    'タイムアウトが発生した場合
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}, Timeout error occurred.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorTimeOut), webEx)
                Else
                    'それ以外のネットワークエラー
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorNetwork), webEx)

                End If

                resultString = String.Empty

            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E OUT:resultString={1}", _
                          MethodBase.GetCurrentMethod.Name, resultString))

            Return resultString

        End Using

    End Function

#End Region

#Region "WebServiceの戻りXMLを解析し値を取得"
    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">送信XML文字列</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetSendStatusXmlResultData(ByVal resultString As String) As Dictionary(Of String, String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:resultString:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  resultString))

        Dim retXmlValueDic As New Dictionary(Of String, String)

        Try
            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultString)

            'XmlElementを取得
            Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

            'XmlElementの確認
            If IsNothing(resultXmlElement) Then
                '取得失敗
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.Error Err: XmlDocument.DocumentElement is nothing." _
                            , MethodBase.GetCurrentMethod.Name))

                retXmlValueDic = Nothing
                Exit Try
            End If

            'ステータス送信の返却XML内の必要なタグがない場合、エラー(戻り値Nothing)とする
            If Not Me.CheckResultXmlElementTag(resultXmlElement) Then
                retXmlValueDic = Nothing
                Exit Try
            End If

            '返却XMLの中から必要な値を取得
            Dim resultId As String = resultXmlElement.GetElementsByTagName(TagResultId).Item(0).InnerText
            Dim message As String = resultXmlElement.GetElementsByTagName(TagMessage).Item(0).InnerText

            '戻り値のDictionaryに設定
            retXmlValueDic.Add(TagResultId, resultId)
            retXmlValueDic.Add(TagMessage, message)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.E OUT:ResultId:{1}, Message:{2}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      resultId, _
                                      message))

        Catch ex As XmlException

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.Error" _
                         , MethodBase.GetCurrentMethod.Name), ex)
            retXmlValueDic = Nothing

        End Try

        Return retXmlValueDic

    End Function
#End Region

#Region "ステータス送信の返却XML内の必要なタグ存在チェックを行う"
    ''' <summary>
    ''' ステータスの返却XML内の必要なタグ存在チェックを行う
    ''' </summary>
    ''' <param name="xmlElement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckResultXmlElementTag(ByVal xmlElement As XmlElement) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:xmlElement:{1}", _
                                  MethodBase.GetCurrentMethod.Name, xmlElement))

        Dim retCheckOkFlg As Boolean = True

        'ResultId
        If IsNothing(xmlElement.GetElementsByTagName(TagResultId).Item(0)) Then
            'ResultIdタグが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Failed to get the ResultId.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'Message
        If IsNothing(xmlElement.GetElementsByTagName(TagMessage).Item(0)) Then
            'Messageタグが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Failed to get the Message.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.E OUT:retCheckOkFlg:{1}", _
                                  MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

#End Region

#Region "システム設定値を取得"
    ''' <summary>
    ''' システム設定、販売店設定からステータス送信に必要な設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As IC3802601SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As IC3802601SystemSettingValueRow = Nothing

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        Try

            '******************************
            '* システム設定から取得
            '******************************
            '基幹連携送信時タイムアウト値
            Dim linkSendTimeoutVal As String _
                = Me.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

            If String.IsNullOrEmpty(linkSendTimeoutVal) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, LINK_SEND_TIMEOUT_VAL does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If

            '国コード
            Dim countryCode As String _
                = Me.GetSystemSettingValueBySettingName(SysCountryCode)

            If String.IsNullOrEmpty(countryCode) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, DIST_CD does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If

            '日付フォーマット
            Dim dateFormat As String _
                = Me.GetSystemSettingValueBySettingName(SysDateFormat)

            If String.IsNullOrEmpty(dateFormat) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, DATE_FORMAT does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If

            'ステータスコード変換フラグ
            Dim StatusCdConv As String _
                = Me.GetSystemSettingValueBySettingName(StatusCodeConvFlg)

            If String.IsNullOrEmpty(StatusCdConv) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, STATUS_CD_CONV_FLG does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If

            '関連チップ送信フラグ
            Dim sendRelationStatus As String _
                = Me.GetSystemSettingValueBySettingName(SysSendRelationStatus)

            If String.IsNullOrEmpty(sendRelationStatus) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, SEND_RELATION_STATUS does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If
            'SOAPバージョン判定値
            Dim soapVersion As String _
                = Me.GetSystemSettingValueBySettingName(SysSoapVersion)

            If String.IsNullOrEmpty(soapVersion) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, LINK_SOAP_VERSION does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorSysEnv))
                errorFlg = True
                Exit Try
            End If

            'CDATA付与フラグ
            Dim cDataFlg As String _
                = Me.GetSystemSettingValueBySettingName(SysCDataApdFlg)

            If String.IsNullOrEmpty(cDataFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, CDATA_APD_FLG does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDlrEnv))
                errorFlg = True
                Exit Try
            End If

            '******************************
            '* 販売店システム設定から取得
            '******************************
            '送信先アドレス
            Dim linkUrlStatusInfo As String _
                = Me.GetDlrSystemSettingValueBySettingName(DlrSysLinkUrlStatusInfo)

            If String.IsNullOrEmpty(linkUrlStatusInfo) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, LINK_URL_STATUS_INFO does not exist.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDlrEnv))
                errorFlg = True
                Exit Try
            End If


            Using table As New IC3802601SystemSettingValueDataTable

                retRow = table.NewIC3802601SystemSettingValueRow

                With retRow
                    '取得した値を戻り値のデータ行に設定
                    .LINK_SEND_TIMEOUT_VAL = linkSendTimeoutVal
                    .DIST_CD = countryCode
                    .DATE_FORMAT = dateFormat
                    .STATUS_CD_CONV_FLG = StatusCdConv
                    .SEND_RELATION_STATUS = sendRelationStatus
                    .LINK_URL_STATUS_INFO = linkUrlStatusInfo
                    .LINK_SOAP_VERSION = soapVersion
                    .CDATA_APD_FLG = cDataFlg
                End With

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retRow

    End Function

    ''' <summary>
    ''' システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        '自分のテーブルアダプタークラスインスタンスを生成
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic

            'システム設定から取得
            retValue = smbCommonBiz.GetSystemSettingValueBySettingName(settingName)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:{1}={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty


        '自分のテーブルアダプタークラスインスタンスを生成
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic

            '販売店システム設定から取得
            retValue = smbCommonBiz.GetDlrSystemSettingValueBySettingName(settingName)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:{1}={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function
#End Region

#Region "送信フラグを取得する"
    ''' <summary>
    ''' ステータス送信を行うかどうかを決定するフラグを取得する
    ''' </summary>
    ''' <param name="prevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="crntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSendStatusFlg(ByVal prevStatus As String, _
                                       ByVal crntStatus As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S prevStatus={1}, crntStatus={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  prevStatus, _
                                  crntStatus))


        'ログインユーザー情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '送信フラグ(戻り値)
        Dim sendFlg As String = String.Empty

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ic3802601da As New IC3802601DataTableAdapter
            'ステータス送信するかしないかのフラグを取得
            Dim getTable As IC3802601LinkSendSettingsDataTable = _
                ic3802601da.GetLinkSettings(userContext.DlrCD, _
                                               userContext.BrnCD, _
                                               AllDealerCode, _
                                               AllBranchCode, _
                                               "2", _
                                               prevStatus, _
                                               crntStatus)

            'ログ出力用販売店コード、店舗コード
            Dim dealerCode As String = String.Empty
            Dim branchCode As String = String.Empty

            Dim getFirstRow As IC3802601LinkSendSettingsRow

            If 0 < getTable.Count Then

                '取得データの1行目（最優先レコード）のみを取得
                getFirstRow = getTable.Item(0)
                '最優先レコードの送信フラグ、販売店コード、店舗コードを取得
                sendFlg = getFirstRow.SEND_FLG
                dealerCode = getFirstRow.DLR_CD
                branchCode = getFirstRow.BRN_CD

            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E SEND_FLG={1}, DLR_CD={2}, BRN_CD={3}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      sendFlg, _
                                      dealerCode, _
                                      branchCode))

        End Using

        Return sendFlg

    End Function
#End Region

#Region "ステータス送信用XMLの構築"

    ''' <summary>
    ''' ステータス送信用XMLを構築する(メイン)
    ''' </summary>
    ''' <param name="sysValueRow">システム設定値データ行</param>
    ''' <param name="dmsDlrbrnRow">基幹販売店・店舗コードデータ行</param>
    ''' <param name="sendChipInfoRow">チップ情報データ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="itemSendFlg">送信項目フラグリスト</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendStatusXml(ByVal sysValueRow As IC3802601SystemSettingValueRow, _
                                          ByVal dmsDlrbrnRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                          ByVal sendChipInfoRow As IC3802601SendChipInfoRow, _
                                          ByVal nowDateTime As Date, _
                                          ByVal itemSendFlg As List(Of Boolean)) As XmlDocument

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, sysValueRow)
        Me.AddLogData(args, dmsDlrbrnRow)
        Me.AddLogData(args, sendChipInfoRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))


        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(UpdateStatusタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagUpdateStatus)

        'headタグの構築
        Dim headTag As XmlElement = Me.StructSendStatusXmlHeadTag(xmlDocument, sysValueRow.DIST_CD, DateFormat, nowDateTime, MessageId, "0")

        'Detailタグの構築
        Dim detailTag As XmlElement = Me.StructSendStatusXmlDetailTag(xmlDocument, _
                                                                      sendChipInfoRow, _
                                                                      dmsDlrbrnRow, _
                                                                      nowDateTime, _
                                                                      itemSendFlg, _
                                                                      sysValueRow)

        If String.IsNullOrEmpty(detailTag.InnerXml) Then
            '必須チェックエラー

            xmlDocument = Nothing

        Else
            'UpdateStatusタグを構築
            xmlRoot.AppendChild(headTag)
            xmlRoot.AppendChild(detailTag)

            '送信用XMLを構築
            xmlDocument.AppendChild(xmlDeclaration)
            xmlDocument.AppendChild(xmlRoot)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                      MethodBase.GetCurrentMethod.Name, _
                      Me.FormatXml(xmlDocument)))

        End If

        Return xmlDocument

    End Function

    ''' <summary>
    ''' ステータス送信用XMLのheadタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="countryCode">国コード</param>
    ''' <param name="dateFormat">日付フォーマット</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>headタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendStatusXmlHeadTag(ByVal xmlDocument As XmlDocument, ByVal countryCode As String, _
                                                 ByVal dateFormat As String, ByVal nowDateTime As Date, _
                                                 ByVal inmessageId As String, _
                                                 ByVal inlinkSystemCode As String) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:countryCode={1}, dateFormat={2}, dateFormat={3}", _
                                  MethodBase.GetCurrentMethod.Name, countryCode, dateFormat, nowDateTime))

        'headタグを作成
        Dim headTag As XmlElement = xmlDocument.CreateElement(TagHead)

        'headタグの子要素を作成
        Dim messageIdTag As XmlElement = xmlDocument.CreateElement(TagMessageID)
        Dim countryCodeTag As XmlElement = xmlDocument.CreateElement(TagCountryCode)
        Dim linkSystemCodeTag As XmlElement = xmlDocument.CreateElement(TagLinkSystemCode)
        Dim TransmissionDateTag As XmlElement = xmlDocument.CreateElement(TagTransmissionDate)

        '子要素に値を設定
        messageIdTag.AppendChild(xmlDocument.CreateTextNode(inmessageId))
        countryCodeTag.AppendChild(xmlDocument.CreateTextNode(countryCode))
        linkSystemCodeTag.AppendChild(xmlDocument.CreateTextNode(inlinkSystemCode))
        TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime.ToString(dateFormat, CultureInfo.CurrentCulture)))

        'headタグを構築
        With headTag
            .AppendChild(messageIdTag)
            .AppendChild(countryCodeTag)
            .AppendChild(linkSystemCodeTag)
            .AppendChild(TransmissionDateTag)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:statusInfoTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, headTag.InnerXml))

        Return headTag

    End Function

    ''' <summary>
    ''' ステータス送信用XMLのDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="sendChipInfoRow">チップ情報データ行</param>
    ''' <param name="dmsDlrBrnRow">基幹販売店・店舗コードデータ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="itemSendFlgList">送信項目フラグリスト</param>
    ''' <returns>ステータス送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendStatusXmlDetailTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal sendChipInfoRow As IC3802601SendChipInfoRow, _
                                                   ByVal dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                                   ByVal nowDateTime As Date, _
                                                   ByVal itemSendFlgList As List(Of Boolean), _
                                                   ByVal sysValueRow As IC3802601SystemSettingValueRow) As XmlElement

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, sendChipInfoRow)
        Me.AddLogData(args, dmsDlrBrnRow)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'Detailタグを作成
        Dim detailTag As XmlElement = xmlDocument.CreateElement(TagDetail)

        Try
            'Commonタグを構築
            Dim commonTag As XmlElement = Me.StructSendStatusXmlCommonTag(xmlDocument, dmsDlrBrnRow)

            If String.IsNullOrEmpty(commonTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'StatusInformationタグを構築
            Dim statusInformationTag As XmlElement _
                = Me.StructSendStatusXmlStatusInformationTag(xmlDocument, sendChipInfoRow, _
                                                                     nowDateTime, _
                                                                     itemSendFlgList, _
                                                                      sysValueRow)

            If String.IsNullOrEmpty(statusInformationTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'Detailタグを構築
            With detailTag
                .AppendChild(commonTag)
                .AppendChild(statusInformationTag)
            End With

        Finally

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                      "{0}_E OUT:detailTag={1}", _
                      MethodBase.GetCurrentMethod.Name, detailTag.InnerXml))

        End Try

        Return detailTag

    End Function

    ''' <summary>
    ''' ステータス送信用XMLのCommonタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="dmsDlrBrnInfoRow">基幹販売店・店舗コードデータ行</param>
    ''' <returns>ステータス送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendStatusXmlCommonTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal dmsDlrBrnInfoRow As ServiceCommonClassDataSet.DmsCodeMapRow) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'Commonタグを作成
        Dim commonTag As XmlElement = xmlDocument.CreateElement(TagCommon)

        If Me.CheckNecessaryCommonTag(dmsDlrBrnInfoRow.CODE1, dmsDlrBrnInfoRow.CODE2) Then
            '必須チェックOK

            'Commonタグの子要素を作成
            Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
            Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)
            Dim staffCodeTag As XmlElement = xmlDocument.CreateElement(TagStaffCode)
            Dim customerCodeTag As XmlElement = xmlDocument.CreateElement(TagCustomerCode)
            Dim salesBookingNumberTag As XmlElement = xmlDocument.CreateElement(TagSalesBookingNumber)
            Dim vinTag As XmlElement = xmlDocument.CreateElement(TagVin)

            '子要素に値を設定
            dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsDlrBrnInfoRow.CODE1))
            branchCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsDlrBrnInfoRow.CODE2))
            staffCodeTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            customerCodeTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            salesBookingNumberTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            vinTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))

            'Commonタグの子要素を追加
            With commonTag
                .AppendChild(dealerCodeTag)
                .AppendChild(branchCodeTag)
                .AppendChild(staffCodeTag)
                .AppendChild(customerCodeTag)
                .AppendChild(salesBookingNumberTag)
                .AppendChild(vinTag)
            End With

        Else
            '必須チェックNG

            commonTag.InnerXml = String.Empty

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:commonTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, commonTag.InnerXml))

        Return commonTag

    End Function

    ''' <summary>
    ''' ステータス送信用XMLのStatusInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="sendChipInfoRow">チップ情報データ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="sendFlgList">送信項目フラグリスト</param>
    ''' <returns>StatusInformationタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendStatusXmlStatusInformationTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal sendChipInfoRow As IC3802601SendChipInfoRow, _
                                                   ByVal nowDateTime As Date, _
                                                   ByVal sendFlgList As List(Of Boolean), _
                                                   ByVal sysValueRow As IC3802601SystemSettingValueRow) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'StatusInformationタグを作成
        Dim statusInformationTag As XmlElement = xmlDocument.CreateElement(TagStatusInformation)
        '予約IDと基幹予約ID両方も値ない時エラーにする
        If sendChipInfoRow.IsJOB_DTL_IDNull AndAlso sendChipInfoRow.IsDMS_JOB_DTL_IDNull Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E JOB_DTL_ID and DMS_JOB_DTL_ID is null", _
                          MethodBase.GetCurrentMethod.Name))
            Return statusInformationTag
        End If

        If String.IsNullOrEmpty(sendChipInfoRow.JOB_DTL_ID.ToString(CultureInfo.CurrentCulture)) _
            AndAlso String.IsNullOrEmpty(sendChipInfoRow.DMS_JOB_DTL_ID) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E JOB_DTL_ID and DMS_JOB_DTL_ID is empty", _
                          MethodBase.GetCurrentMethod.Name))
            Return statusInformationTag
        End If

        Try
            'タグに設定する値を全てローカル変数に格納
            Dim sequenceNo As String = nowDateTime.ToString(SeqNoNumberingFormat, CultureInfo.CurrentCulture)
            Dim reserveId As String = sendChipInfoRow.JOB_DTL_ID.ToString(CultureInfo.CurrentCulture)
            Dim basReserveId As String = sendChipInfoRow.DMS_JOB_DTL_ID.Trim()

            Dim status As String = sendChipInfoRow.DMS_CHIP_STATUS.Trim()

            Dim startTime As String = String.Empty
            If sendFlgList.Item(0) Then
                startTime = sendChipInfoRow.STARTTIME.Trim()
            End If

            Dim endTime As String = String.Empty
            If sendFlgList.Item(1) Then
                endTime = sendChipInfoRow.ENDTIME.Trim()
            End If

            Dim rezTime As String = String.Empty
            If sendFlgList.Item(2) Then
                rezTime = sendChipInfoRow.REZ_TIME.Trim()
            End If

            Dim workTime As String = String.Empty
            If sendFlgList.Item(3) Then
                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'workTime = sendChipInfoRow.WORKTIME.Trim()
                If Not sendChipInfoRow.IsWORKTIMENull Then
                    workTime = sendChipInfoRow.WORKTIME.Trim()
                End If
                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            End If

            Dim stallId As String = String.Empty
            If sendFlgList.Item(4) Then
                stallId = sendChipInfoRow.STALLID.Trim()
            End If

            Dim break As String = String.Empty
            If sendFlgList.Item(5) Then
                break = sendChipInfoRow.BREAK.Trim()
            End If

            Dim waitTime As String = String.Empty
            If sendFlgList.Item(6) Then
                waitTime = sendChipInfoRow.WAITTIME.Trim()
            End If

            Dim inspectionMemo As String = String.Empty
            If Not sendChipInfoRow.IsINSPECTION_MEMONull Then
                inspectionMemo = sendChipInfoRow.INSPECTION_MEMO.Trim()
            End If


            'StatusInfomationタグの子要素を作成
            Dim seqNoTag As XmlElement = xmlDocument.CreateElement(TagSeqNo)
            Dim rezIdTag As XmlElement = xmlDocument.CreateElement(TagRezId)
            Dim basRezIdTag As XmlElement = xmlDocument.CreateElement(TagBasRezId)
            Dim statusTag As XmlElement = xmlDocument.CreateElement(TagStatus)
            Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagStartTime)
            Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagEndTime)
            Dim rezTimeTag As XmlElement = xmlDocument.CreateElement(TagRezTime)
            Dim workTimeTag As XmlElement = xmlDocument.CreateElement(TagWorkTime)
            Dim stallIdTag As XmlElement = xmlDocument.CreateElement(TagStallid)
            Dim breakTag As XmlElement = xmlDocument.CreateElement(TagBreak)
            Dim waitTimeTag As XmlElement = xmlDocument.CreateElement(TagWaitTime)
            Dim inspectionMemoTag As XmlElement = xmlDocument.CreateElement(TaginspectionMemo)


            '子要素に値を設定
            seqNoTag.AppendChild(xmlDocument.CreateTextNode(sequenceNo))
            rezIdTag.AppendChild(xmlDocument.CreateTextNode(reserveId))
            basRezIdTag.AppendChild(xmlDocument.CreateTextNode(basReserveId))
            statusTag.AppendChild(xmlDocument.CreateTextNode(status))
            startTimeTag.AppendChild(xmlDocument.CreateTextNode(startTime))
            endTimeTag.AppendChild(xmlDocument.CreateTextNode(endTime))
            rezTimeTag.AppendChild(xmlDocument.CreateTextNode(rezTime))
            workTimeTag.AppendChild(xmlDocument.CreateTextNode(workTime))
            stallIdTag.AppendChild(xmlDocument.CreateTextNode(stallId))
            breakTag.AppendChild(xmlDocument.CreateTextNode(break))
            waitTimeTag.AppendChild(xmlDocument.CreateTextNode(waitTime))
            If CData_Append.Equals(sysValueRow.CDATA_APD_FLG) Then
                inspectionMemoTag.AppendChild(xmlDocument.CreateCDataSection(inspectionMemo))
            Else
                inspectionMemoTag.AppendChild(xmlDocument.CreateTextNode(inspectionMemo))
            End If


            'UpdateStatusInfomationタグを構築
            With statusInformationTag
                .AppendChild(seqNoTag)
                .AppendChild(rezIdTag)
                .AppendChild(basRezIdTag)
                .AppendChild(statusTag)
                .AppendChild(startTimeTag)
                .AppendChild(endTimeTag)
                .AppendChild(rezTimeTag)
                .AppendChild(workTimeTag)
                .AppendChild(stallIdTag)
                .AppendChild(breakTag)
                .AppendChild(waitTimeTag)
                .AppendChild(inspectionMemoTag)
            End With

        Finally
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E OUT:statusInformationTag={1}", _
                                      MethodBase.GetCurrentMethod.Name, statusInformationTag.InnerXml))
        End Try

        Return statusInformationTag

    End Function

#End Region

#Region "送信XMLデータの作成"
    ''' <summary>
    ''' 送信XMLデータの作成
    ''' </summary>
    ''' <param name="serviceinId">サービス入庫ID</param>
    ''' <param name="jobdtlId">作業内容ID</param>
    ''' <param name="stalluseId">ストールUSEID</param>
    ''' <param name="waitTime">ストール待機時間</param>
    ''' <param name="prevStatus">icrop更新前ステータス</param>
    ''' <param name="currentStatus">icrop更新後ステータス</param>
    ''' <param name="dateFormat">日時フォーマット</param>
    ''' <param name="systemSettingsValueRow">システム設定値行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateXMLData(ByVal serviceinId As Decimal, _
                                   ByVal jobdtlId As Decimal, _
                                   ByVal stalluseId As Decimal, _
                                   ByVal waitTime As Long, _
                                   ByVal prevStatus As String, _
                                   ByVal currentStatus As String, _
                                   ByVal dateFormat As String, _
                                   ByVal systemSettingsValueRow As IC3802601SystemSettingValueRow) As IC3802601SendChipInfoDataTable
        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, systemSettingsValueRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:systemSettingsValueRow={1}, serviceinId={2}, jobdtlId={3}, stalluseId={4}, waitTime={5}, currentStatus={6}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray()), _
                                  serviceinId, _
                                  jobdtlId, _
                                  stalluseId, _
                                  waitTime, _
                                  currentStatus))
        'ログインスタッフ情報取得
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        '返却用データテーブル
        Using dtSendChipInfo As New IC3802601SendChipInfoDataTable
            Using ic3802601Ta As New IC3802601DataTableAdapter
                Dim dtChipinfo As IC3802601ChipInfoDataTable = ic3802601Ta.GetChipInfo(serviceinId, stalluseId, jobdtlId)
                '正しく取得できなかった場合はエラにする
                If dtChipinfo.Count <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error, Failed to get a chip information.", _
                           MethodBase.GetCurrentMethod.Name))
                    Return Nothing
                End If

                '返却用データテーブルを作成するためのデータ行
                Dim drsendChipinfo As IC3802601SendChipInfoRow = dtSendChipInfo.NewIC3802601SendChipInfoRow

                '送信項目を取得
                '関連チップ送信フラグが「1」送信する場合は操作チップの作業内容ID、基幹作業内容IDを設定する
                If SendRelationChipFlg_Send.Equals(systemSettingsValueRow.SEND_RELATION_STATUS) Then
                    '操作チップの作業内容IDと基幹作業内容IDを設定する
                    drsendChipinfo.JOB_DTL_ID = jobdtlId.ToString(CultureInfo.CurrentCulture)
                    drsendChipinfo.DMS_JOB_DTL_ID = dtChipinfo(0).DMS_JOB_DTL_ID
                Else
                    '親チップの作業内容ID、基幹作業内容IDを設定する
                    drsendChipinfo.JOB_DTL_ID = dtChipinfo(0).MANA_JOB_DTL_ID.ToString(CultureInfo.CurrentCulture)
                    drsendChipinfo.DMS_JOB_DTL_ID = dtChipinfo(0).MANA_DMS_JOB_DTL_ID
                End If

                '更新後ステータスの置換
                Dim dmsStatus As String = Me.ConvertStatusToDMS(currentStatus, systemSettingsValueRow.STATUS_CD_CONV_FLG, waitTime)
                If String.IsNullOrEmpty(dmsStatus) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.Error, Failed to ConvertStatusToDMS.", _
                                    MethodBase.GetCurrentMethod.Name))
                    Return Nothing
                End If
                drsendChipinfo.DMS_CHIP_STATUS = dmsStatus

                drsendChipinfo.STARTTIME = GetStartTime(dtChipinfo(0), currentStatus, dateFormat)
                drsendChipinfo.ENDTIME = GetEndTime(dtChipinfo(0), prevStatus, dateFormat)
                drsendChipinfo.REZ_TIME = GetScheStartDate(dtChipinfo(0), dateFormat)
                drsendChipinfo.WORKTIME = GetWorkTime(dtChipinfo(0))
                '基幹ストールID変換
                Dim dmsStallId As String = GetDmsStallId(dlrCd, brnCd, dtChipinfo(0).STALL_ID.ToString(CultureInfo.InvariantCulture))
                If String.IsNullOrEmpty(dmsStallId) Then
                    '変換失敗
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                            "{0}.Error, Failed to ConvertDmsStallId.", _
                                                MethodBase.GetCurrentMethod.Name))
                    Return Nothing
                Else
                    drsendChipinfo.STALLID = dmsStallId
                End If

                drsendChipinfo.BREAK = GetRestFlg(dtChipinfo(0))
                drsendChipinfo.WAITTIME = GetWaitTime(waitTime)
                drsendChipinfo.INSPECTION_MEMO = dtChipinfo(0).INSPECTION_MEMO
                dtSendChipInfo.AddIC3802601SendChipInfoRow(drsendChipinfo)

                '関連チップ送信判定
                If SendRelationChipFlg_Send.Equals(systemSettingsValueRow.SEND_RELATION_STATUS) AndAlso _
                    ShouldSendRelationChips(dmsStatus, currentStatus, prevStatus) Then
                    '該当チップ以外のチップの情報を取得
                    Dim dtRelationChipJobInfo As IC3802601RelationChipInfoDataTable = _
                                        ic3802601Ta.GetRelationChipInfo(serviceinId, jobdtlId)

                    '全リレーションチップ分データ作成
                    For Each drRelationChipJobInfo As IC3802601RelationChipInfoRow In dtRelationChipJobInfo
                        Dim drsendOtherChipsinfo As IC3802601SendChipInfoRow = dtSendChipInfo.NewIC3802601SendChipInfoRow

                        drsendOtherChipsinfo = CollectOtherRelationChips(drRelationChipJobInfo.JOB_DTL_ID, _
                                                                         drRelationChipJobInfo.DMS_JOB_DTL_ID, _
                                                                         dmsStatus, _
                                                                         drsendOtherChipsinfo)
                        dtSendChipInfo.AddIC3802601SendChipInfoRow(drsendOtherChipsinfo)
                    Next

                End If
            End Using


            '引数をログに出力
            Dim outArgs As New List(Of String)
            For Each drSendChipInfo As IC3802601SendChipInfoRow In dtSendChipInfo
                'DataRow内の項目を列挙
                Me.AddLogData(outArgs, drSendChipInfo)
            Next
            'ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.E OUT:{1}" _
                        , MethodBase.GetCurrentMethod.Name _
                        , String.Join(", ", outArgs.ToArray())))
            Return dtSendChipInfo
        End Using
    End Function

#Region "関連チップデータ作成"
    ''' <summary>
    ''' 関連チップデータ作成
    ''' </summary>
    ''' <param name="jobdtlId">作業内容ID</param>
    ''' <param name="dmsjobdtlId">基幹作業内容ID</param>
    ''' <param name="dmsStatus">基幹ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CollectOtherRelationChips(ByVal jobdtlId As Decimal, _
                                               ByVal dmsjobdtlId As String, _
                                               ByVal dmsStatus As String, _
                                               ByVal drSendChipInfo As IC3802601SendChipInfoRow) As IC3802601SendChipInfoRow
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
          "{0}_S, jobdtlId={1}, dmsjobdtlId={2} dmsStatus={3}", _
          MethodBase.GetCurrentMethod.Name, _
          jobdtlId, _
          dmsjobdtlId, _
          dmsStatus))

        Dim drRelationChipinfo As IC3802601SendChipInfoRow = drSendChipInfo
        drRelationChipinfo.JOB_DTL_ID = jobdtlId.ToString(CultureInfo.CurrentCulture)
        drRelationChipinfo.DMS_JOB_DTL_ID = dmsjobdtlId
        drRelationChipinfo.DMS_CHIP_STATUS = dmsStatus
        drRelationChipinfo.STARTTIME = String.Empty
        drRelationChipinfo.ENDTIME = String.Empty
        drRelationChipinfo.REZ_TIME = String.Empty
        drRelationChipinfo.WAITTIME = String.Empty
        drRelationChipinfo.STALLID = String.Empty
        drRelationChipinfo.BREAK = String.Empty
        drRelationChipinfo.INSPECTION_MEMO = String.Empty

        '引数をログに出力
        Dim outArgs As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(outArgs, drRelationChipinfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.E OUT:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", outArgs.ToArray())))
        Return drRelationChipinfo


    End Function
#End Region

#Region "基幹チップステータス変換"
    ''' <summary>
    ''' 基幹チップステータス変換
    ''' </summary>
    ''' <param name="icropChipStatus">icropステータス</param>
    ''' <param name="statusCdCnvFlg">ステータス変換フラグ</param>
    ''' <param name="stallWaitTime">ストール待機時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertStatusToDMS(ByVal icropChipStatus As String, _
                                        ByVal statusCdCnvFlg As String, _
                                        ByVal stallWaitTime As Long) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}_S, icropChipStatus={1}, statusCdCnvFlg={2}, stallWaitTime={3}", _
                  MethodBase.GetCurrentMethod.Name, _
                  icropChipStatus, _
                  statusCdCnvFlg, _
                  stallWaitTime))

        If String.IsNullOrEmpty(icropChipStatus.Trim()) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error ConvertStatusToDMS failure, icropChipStatus is empty", _
                           MethodBase.GetCurrentMethod.Name))
            Return String.Empty
        End If

        '返却用変数
        Dim dmsStatus As String

        '11:中断・ストール待機に変換するチップステータスリスト
        Dim convertToStopForStallWaitStatusList As New List(Of String)
        With convertToStopForStallWaitStatusList
            .Add(ChipStatusStopForPartsStockout)
            .Add(ChipStatusStopForWaitCustomer)
            .Add(ChipStatusStopForOtherReason)
        End With
        'ストール待機時間設定時の検査中断以外の中断ステータスは、中断・ストール待機に変換する
        Dim icropCurrentStatus As String = icropChipStatus
        If Not stallWaitTime = 0 AndAlso _
            convertToStopForStallWaitStatusList.Contains(icropChipStatus) Then
            icropCurrentStatus = ChipStatusStopForWaitStall
        End If

        Using smbcommonblz As New ServiceCommonClassBusinessLogic
            'ログインスタッフ情報取得
            Dim userContext As StaffContext = StaffContext.Current
            Dim dtDmsStatus As ServiceCommonClassDataSet.DmsCodeMapDataTable = smbcommonblz.GetIcropToDmsCode(userContext.DlrCD, _
                                                                                            ServiceCommonClassBusinessLogic.DmsCodeType.ChipStatus, _
                                                                                            icropCurrentStatus, _
                                                                                            String.Empty, _
                                                                                            String.Empty)
            If dtDmsStatus.Count <> 1 OrElse dtDmsStatus(0).IsCODE1Null Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
               "{0}.Error ConvertStatusToDMS failure, DBvalue is incorrect ", _
               MethodBase.GetCurrentMethod.Name))
                Return String.Empty
            Else
                'ステータスコード変換フラグが「1:更新する」でかつ、
                '更新後ステータスが「作業完了」または「日跨ぎ終了」
                'の場合は更新後ステータスを「納車済み」に変換する
                If StatusCodeConvFlg_Use.Equals(statusCdCnvFlg) AndAlso _
                    (DmsJobFinish.Equals(dtDmsStatus(0).CODE1) OrElse _
                      DmsDateCrossEnd.Equals(dtDmsStatus(0).CODE1)) Then
                    dmsStatus = DmsDeliveryEnd
                Else
                    dmsStatus = dtDmsStatus(0).CODE1
                End If
            End If
        End Using


        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.E OUT:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , dmsStatus))

        Return dmsStatus
    End Function

#End Region

#Region "現在のステータス遷移に応じて、実績開始日時を取得する"
    ''' <summary>
    ''' 実績開始日時を取得する
    ''' </summary>
    ''' <param name="drChipInfo">チップ情報行</param>
    ''' <param name="currentStatus">更新後icropステータス</param>
    ''' <param name="DateFormat">日時フォーマット</param>
    ''' <returns>実績開始日時</returns>
    ''' <remarks></remarks>
    Private Function GetStartTime(ByVal drChipInfo As IC3802601ChipInfoRow, _
                                  ByVal currentStatus As String, _
                                  ByVal DateFormat As String) As String
        '引数をログに出力
        Dim inArgs As New List(Of String)
        'DataRow内の項目を列挙
        Me.AddLogData(inArgs, drChipInfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}, currentStatus={2}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", inArgs.ToArray()) _
                    , currentStatus))
        '返却変数
        Dim retStartTime As String = String.Empty

        Select Case currentStatus
            Case ChipStatusWorking
                If Not drChipInfo.IsRSLT_START_DATETIMENull Then
                    'ストール利用.実績開始日時を実績開始日時として使用
                    retStartTime = drChipInfo.RSLT_START_DATETIME.ToString(DateFormat, CultureInfo.CurrentCulture)
                End If
            Case ChipStatusWashing
                If Not drChipInfo.IsCW_RSLT_START_DATETIMENull Then
                    '洗車実績.実績開始日時を実績開始日時として使用
                    retStartTime = drChipInfo.CW_RSLT_START_DATETIME.ToString(DateFormat, CultureInfo.CurrentCulture)
                End If
            Case ChipStatusInspecting
                If Not drChipInfo.IsIS_RSLT_START_DATETIMENull Then
                    '検査実績.実績開始日時を実績開始日時として使用
                    retStartTime = drChipInfo.IS_RSLT_START_DATETIME.ToString(DateFormat, CultureInfo.CurrentCulture)
                End If
        End Select
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retStartTime={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retStartTime.ToString(CultureInfo.InvariantCulture)))
        Return retStartTime
    End Function
#End Region

#Region "現在のステータス遷移に応じて、実績終了日時を取得する"
    ''' <summary>
    ''' 実績終了日時を取得する
    ''' </summary>
    ''' <param name="drChipInfo">チップ情報行</param>
    ''' <param name="prevStatus">更新前icropステータス</param>
    ''' <param name="dateFormat">日時フォーマット</param>
    ''' <returns>実績終了日時</returns>
    ''' <remarks></remarks>
    Private Function GetEndTime(ByVal drChipInfo As IC3802601ChipInfoRow, _
                                  ByVal prevStatus As String, _
                                  ByVal dateFormat As String) As String
        '引数をログに出力
        Dim inArgs As New List(Of String)
        'DataRow内の項目を列挙
        Me.AddLogData(inArgs, drChipInfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}, prevStatus={2}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", inArgs.ToArray()) _
                    , prevStatus))
        '返却変数
        Dim retEndTime As String = String.Empty

        Select Case prevStatus
            Case ChipStatusWorking, ChipStatusJobFinish
                If Not drChipInfo.IsRSLT_END_DATETIMENull Then
                    'ストール利用.実績開始日時を実績開始日時として使用
                    retEndTime = drChipInfo.RSLT_END_DATETIME.ToString(dateFormat, CultureInfo.CurrentCulture)
                End If
            Case ChipStatusWashing
                If Not drChipInfo.IsCW_RSLT_END_DATETIMENull Then
                    '洗車実績.実績開始日時を実績開始日時として使用
                    retEndTime = drChipInfo.CW_RSLT_END_DATETIME.ToString(dateFormat, CultureInfo.CurrentCulture)
                End If
            Case ChipStatusInspecting
                If Not drChipInfo.IsIS_RSLT_END_DATETIMENull Then
                    '検査実績.実績開始日時を実績開始日時として使用
                    retEndTime = drChipInfo.IS_RSLT_END_DATETIME.ToString(dateFormat, CultureInfo.CurrentCulture)
                End If
        End Select
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retEndTime={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retEndTime.ToString(CultureInfo.InvariantCulture)))
        Return retEndTime
    End Function
#End Region

#Region "予定開始日時を取得する"
    ''' <summary>
    ''' 予定開始日時を取得する
    ''' </summary>
    ''' <param name="drChipInfo">チップ情報行</param>
    ''' <param name="dateFormat">日時フォーマット</param>
    ''' <returns>予定開始日時</returns>
    ''' <remarks></remarks>
    Private Function GetScheStartDate(ByVal drChipInfo As IC3802601ChipInfoRow, _
                                      ByVal dateFormat As String) As String
        '引数をログに出力
        Dim inArgs As New List(Of String)
        'DataRow内の項目を列挙
        Me.AddLogData(inArgs, drChipInfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", inArgs.ToArray())))
        '返却変数
        Dim retRezTime As String = String.Empty

        If Not drChipInfo.IsREZ_TIMENull Then
            retRezTime = drChipInfo.REZ_TIME.ToString(dateFormat, CultureInfo.CurrentCulture)
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retRezTime={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retRezTime.ToString(CultureInfo.InvariantCulture)))
        Return retRezTime
    End Function
#End Region

#Region "予定作業時間を取得する"
    ''' <summary>
    ''' 予定作業時間を取得する
    ''' </summary>
    ''' <param name="drChipInfo">チップ情報行</param>
    ''' <returns>予定作業時間</returns>
    ''' <remarks></remarks>
    Private Function GetWorkTime(ByVal drChipInfo As IC3802601ChipInfoRow) As String

        '引数をログに出力
        Dim inArgs As New List(Of String)
        'DataRow内の項目を列挙
        Me.AddLogData(inArgs, drChipInfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", inArgs.ToArray())))

        '返却変数
        Dim retWorkTime As String = String.Empty

        If Not drChipInfo.IsWORKTIMENull Then
            retWorkTime = drChipInfo.WORKTIME.ToString(CultureInfo.CurrentCulture)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retWorkTime={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retWorkTime))
        Return retWorkTime
    End Function
#End Region

#Region "休憩フラグを取得する"
    ''' <summary>
    ''' 休憩フラグを取得する
    ''' </summary>
    ''' <param name="drChipInfo">チップ情報行</param>
    ''' <returns>予定作業時間</returns>
    ''' <remarks></remarks>
    Private Function GetRestFlg(ByVal drChipInfo As IC3802601ChipInfoRow) As String
        '引数をログに出力
        Dim inArgs As New List(Of String)
        'DataRow内の項目を列挙
        Me.AddLogData(inArgs, drChipInfo)
        'ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", inArgs.ToArray())))
        '返却変数
        Dim retRestFlg As String = String.Empty

        If Not drChipInfo.IsBREAKNull Then
            retRestFlg = drChipInfo.BREAK
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retRestFlg={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retRestFlg))
        Return retRestFlg
    End Function
#End Region

#Region "待機時間を取得する"
    ''' <summary>
    ''' 待機時間を取得する
    ''' </summary>
    ''' <param name="waitTime">ストール待機時間</param>
    ''' <returns>予定作業時間</returns>
    ''' <remarks></remarks>
    Private Function GetWaitTime(ByVal waitTime As Long) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.S, waitTime={1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          waitTime))
        '返却変数
        Dim retWaitTime As String = String.Empty

        retWaitTime = waitTime.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0}_E, retWaitTime={1} ", _
              MethodBase.GetCurrentMethod.Name, _
              retWaitTime))
        Return retWaitTime
    End Function
#End Region

#Region "基幹ストールIDを取得する"
    ''' <summary>
    ''' 基幹ストールIDを取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>基幹ストールID</returns>
    ''' <remarks>変換に失敗した場合は、空文字を返却する</remarks>
    Private Function GetDmsStallId(ByVal dealerCode As String, _
                                   ByVal branchCode As String, _
                                   ByVal stallId As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:dealerCode={1}, branchCode={2}, stallId={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode, _
                                  stallId))

        Dim dmsStallIdTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using smbCommnBiz As New ServiceCommonClassBusinessLogic

            'ストールIDを基幹ストールIDに変換
            dmsStallIdTable = smbCommnBiz.GetIcropToDmsCode(dealerCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.StallId, _
                                                            dealerCode, _
                                                            branchCode, _
                                                            stallId)

        End Using

        'ストールID(xml設定用)
        Dim dmsStallId As String

        If dmsStallIdTable.Count <= 0 Then
            'データが取得できない場合は変換無し
            dmsStallId = stallId

        ElseIf dmsStallIdTable.Count = 1 Then
            'データが1件取得できた場合はDB値を設定(DMS_CD_3)
            dmsStallId = dmsStallIdTable.Item(0).CODE3

        Else
            'データが2件以上取得できた場合は一意に決定できないためエラー
            dmsStallId = String.Empty

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert key stallId.(Non-unique)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:dmsStallId:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dmsStallId))
        Return dmsStallId

    End Function
#End Region

#End Region

#Region "ステータス遷移毎の送信項目判定"
    ''' <summary>
    ''' ステータス遷移毎の送信項目判定
    ''' </summary>
    ''' <param name="PrevStatus">更新前ステータス</param>
    ''' <param name="CurrentStatus">更新後ステータス</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 実績開始日時を送信するかどうか
    ''' 実績終了日時を送信するかどうか
    ''' 予定開始日時を送信するかどうか
    ''' 予定作業時間を送信するかどうか
    ''' 基幹ストールIDを送信するかどうか
    ''' 休憩フラグを送信するかどうか
    ''' 待機時間を送信するかどうか
    ''' </remarks>
    Public Function JudgeSenddecision(ByVal prevStatus As String, ByVal currentStatus As String) As List(Of Boolean)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S", _
                          MethodBase.GetCurrentMethod.Name))
        '返却用リスト
        Dim sendDecisionList As New List(Of Boolean)
        Select Case prevStatus
            Case ChipStatusTentativeNotCarIn, ChipStatusConfirmedNotCarIn
                sendDecisionList = StatusChangeFromNotCarIn()
            Case ChipStatusTemp
                sendDecisionList = StatusChangeFromTemp()
            Case ChipStatusNoShow
                sendDecisionList = StatusChangeFromNoShow()
            Case ChipStatusTentativeWaitStart, ChipStatusConfirmedWaitStart
                sendDecisionList = StatusChangeFromWaitStart(currentStatus)
            Case ChipStatusWorking
                sendDecisionList = StatusChangeFromWorking(currentStatus)
            Case ChipStatusJobFinish
                sendDecisionList = StatusChangeFromJobFinish(currentStatus)
            Case ChipStatusStopForPartsStockout, ChipStatusStopForInspection, ChipStatusStopForOtherReason, _
                ChipStatusStopForWaitCustomer, ChipStatusStopForWaitStall
                sendDecisionList = StatusChangeFromStop(currentStatus)
            Case ChipStatusWaitWash
                sendDecisionList = StatusChangeFromWaitWash(currentStatus)
            Case ChipStatusWashing
                sendDecisionList = StatusChangeFromWashing(currentStatus)
            Case ChipStatusWaitInspection
                sendDecisionList = StatusChangeFromWaitInspection(currentStatus)
            Case ChipStatusInspecting
                sendDecisionList = StatusChangeFromInspecting(currentStatus)
            Case ChipStatusKeeping
                sendDecisionList = StatusChangeFromKeeping(currentStatus)
            Case ChipStatusWaitDelivery
                sendDecisionList = StatusChangeFromWaitDelivery(currentStatus)
            Case ChipStatusWalkin
                sendDecisionList = StatusChangeFromWalkin()
        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E, sendDecisionList={1} ", _
                          MethodBase.GetCurrentMethod.Name, _
                          String.Join(", ", sendDecisionList.ToArray())))
        Return sendDecisionList
    End Function

    ''' <summary>
    ''' ステータス未入庫から遷移
    ''' </summary>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromNotCarIn() As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        retList.Add(False)
        retList.Add(False)
        retList.Add(False)
        retList.Add(False)
        retList.Add(False)
        retList.Add(False)
        retList.Add(False)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
          "{0}.E", _
          MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス仮置きからの遷移
    ''' </summary>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromTemp() As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        retList.Add(False)
        retList.Add(False)
        retList.Add(True)
        retList.Add(False)
        retList.Add(True)
        retList.Add(True)
        retList.Add(False)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス未来店客からの遷移
    ''' </summary>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromNoShow() As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        retList.Add(False)
        retList.Add(False)
        retList.Add(True)
        retList.Add(False)
        retList.Add(True)
        retList.Add(True)
        retList.Add(False)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス作業開始待ちからの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWaitStart(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusWorking.Equals(CurrentStatus) Then
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        ElseIf ChipStatusJobFinish.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))

        Return retList
    End Function

    ''' <summary>
    ''' ステータス作業中からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWorking(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) OrElse ChipStatusTentativeWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusStopForWaitStall.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(True)
        ElseIf ChipStatusDateCrossEnd.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(True)
            retList.Add(True)
            retList.Add(True)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス作業終了からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromJobFinish(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス中断からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromStop(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(True)
            retList.Add(True)
            retList.Add(False)
        ElseIf ChipStatusWaitWash.Equals(CurrentStatus) OrElse _
            ChipStatusWaitInspection.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス洗車待ちからの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWaitWash(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusWashing.Equals(CurrentStatus) OrElse _
            ChipStatusWaitInspection.Equals(CurrentStatus) Then
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス洗車中からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWashing(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusWaitWash.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス検査待ちからの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWaitInspection(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusInspecting.Equals(CurrentStatus) Then
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' ステータス検査中からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromInspecting(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusWaitInspection.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusStopForInspection.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
        Else
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' 預かり中からの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromKeeping(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusWaitDelivery.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' 納車待ちからの遷移
    ''' </summary>
    ''' <param name="CurrentStatus">変更後のステータス</param>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWaitDelivery(ByVal CurrentStatus As String) As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)
        If ChipStatusConfirmedWaitStart.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
        ElseIf ChipStatusKeeping.Equals(CurrentStatus) Then
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        Else
            retList.Add(True)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
            retList.Add(False)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

    ''' <summary>
    ''' 飛び込みからの遷移
    ''' </summary>
    ''' <returns>送信項目判定リスト</returns>
    ''' <remarks></remarks>
    Private Function StatusChangeFromWalkin() As List(Of Boolean)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S", _
                  MethodBase.GetCurrentMethod.Name))
        '返却リスト作成
        Dim retList As New List(Of Boolean)

        retList.Add(False)
        retList.Add(False)
        retList.Add(True)
        retList.Add(False)
        retList.Add(True)
        retList.Add(True)
        retList.Add(False)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E", _
                                MethodBase.GetCurrentMethod.Name))
        Return retList
    End Function

#End Region

#Region " 予約ステータスでチップステータスを変換する"
    ''' <summary>
    ''' 予約ステータスでチップステータスを変換する
    ''' </summary>
    ''' <param name="chipStatus">チップステータス</param>
    ''' <param name="resStatus">予約ステータス</param>
    '''<returns>
    ''' 予約ステータスが「1：本予約」の場合、
    ''' またはチップステータスが「5：仮置き」「6：未来店客」以外の場合はチップステータスを変換せずに返却する。
    ''' 予約ステータスが「0：仮予約」、チップステータスが「5：仮置き」「6：未来店客」の場合、以下の２通りに分岐する。
    '''チップステータスが「5：仮置き」の場合、「23：仮置き(仮予約)」を返却する。
    ''' チップステータスが「6：未来店客」の場合、「24：未来店客(仮予約)」を返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function ConvertStatus(ByVal chipStatus As String, ByVal resStatus As String) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.S IN:chipStatus={1} resStatus={2}" _
                , MethodBase.GetCurrentMethod.Name, chipStatus, resStatus))
        Dim reStatus As String = chipStatus
        '仮予約の場合
        If ResvStatusTentative.Equals(resStatus) Then
            If ChipStatusNoShow.Equals(chipStatus) Then
                reStatus = ChipStatusTentativeNoShow
            ElseIf ChipStatusTemp.Equals(chipStatus) Then
                reStatus = ChipStatusTentativeTemp
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.E OUT: STATUS={1} ", _
                  MethodBase.GetCurrentMethod.Name, reStatus))

        Return reStatus
    End Function
#End Region

#Region "関連チップ送信判定"
    ''' <summary>
    ''' 関連チップを送信する必要があるかを判定する
    ''' </summary>
    ''' <param name="currentDmsStatus">更新後DMSチップステータス</param>
    ''' <param name="currentIcropStatus">更新後iCROPチップステータス</param>
    ''' <param name="prevIcropStatus">更新前iCROPチップステータス</param>
    ''' <returns>
    ''' 更新後DMSチップステータスが納車済みの場合、
    ''' または更新前iCROPチップステータスが未入庫(本・仮)、作業開始待ち(本・仮)でかつ、
    ''' 更新後iCROPステータスが未入庫(本・仮)、作業開始待ち(本・仮)、未来店客の場合に関連チップを送信する必要ありと判定し、
    '''  trueを返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function ShouldSendRelationChips(ByVal currentDmsStatus As String, _
                                             ByVal currentIcropStatus As String, _
                                             ByVal prevIcropStatus As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S IN:currentDmsStatus={1}, currentIcropStatus={2}, prevIcropStatus={3}", _
                          MethodBase.GetCurrentMethod.Name, _
                          currentDmsStatus, _
                          currentIcropStatus, _
                          prevIcropStatus))
        '返却用変数
        Dim sendRelationFlg As Boolean = False
        '関連チップ送信の更新前iCROPチップステータスリスト
        Dim prevAllSendStatusList As New List(Of String)
        With prevAllSendStatusList
            .Add(ChipStatusTentativeNotCarIn)
            .Add(ChipStatusTentativeWaitStart)
            .Add(ChipStatusConfirmedNotCarIn)
            .Add(ChipStatusConfirmedWaitStart)
        End With

        '関連チップ送信の更新後iCROPチップステータスリスト
        Dim currentAllSendStatusList As New List(Of String)
        With currentAllSendStatusList
            .Add(ChipStatusTentativeNotCarIn)
            .Add(ChipStatusTentativeWaitStart)
            .Add(ChipStatusConfirmedNotCarIn)
            .Add(ChipStatusConfirmedWaitStart)
            .Add(ChipStatusNoShow)
        End With
        '関連チップ送信判定する
        If DmsDeliveryEnd.Equals(currentDmsStatus) OrElse _
           Not prevIcropStatus.Equals(currentIcropStatus) AndAlso _
           prevAllSendStatusList.Contains(prevIcropStatus) AndAlso _
            currentAllSendStatusList.Contains(currentIcropStatus) Then
            sendRelationFlg = True
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E, sendRelationFlg={1}", _
                                MethodBase.GetCurrentMethod.Name, _
                                sendRelationFlg))
        Return sendRelationFlg
    End Function
#End Region

#Region "チェック用メッソド"
    ''' <summary>
    ''' Commonタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryCommonTag(ByVal dmsDealerCode As String, ByVal dmsBranchCode As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.S IN:dmsDealerCode:{1}, dmsBranchCode:{2}", _
                      MethodBase.GetCurrentMethod.Name, dmsDealerCode, dmsBranchCode))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(dmsDealerCode) Then
            '基幹販売店コードが存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:DMSDealerCode is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(dmsBranchCode) Then
            '基幹店舗コードが存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:DMSBranchCode is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.E OUT:retCheckOkFlg:{1}", _
                      MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

#End Region

#Region "ログ出力用"
    ''' <summary>
    ''' DataRow内の項目を列挙(ログ出力用)
    ''' </summary>
    ''' <param name="args">ログ項目のコレクション</param>
    ''' <param name="row">対象となるDataRow</param>
    ''' <remarks></remarks>
    Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
        For Each column As DataColumn In row.Table.Columns
            If row.IsNull(column.ColumnName) Then
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
            End If
        Next
    End Sub

    ''' <summary>
    ''' XMLをインデントを付加して整形する(ログ出力用)
    ''' </summary>
    ''' <param name="xmlDoc">XMLドキュメント</param>
    ''' <returns>整形後XML文字列</returns>
    ''' <remarks></remarks>
    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        Using textWriter As New StringWriter(CultureInfo.InvariantCulture)

            Dim xmlWriter As XmlTextWriter

            Try
                xmlWriter = New XmlTextWriter(textWriter)

                'インデントを2でフォーマット
                xmlWriter.Formatting = Formatting.Indented
                xmlWriter.Indentation = 2

                'XmlTextWriterにXMLを出力
                xmlDoc.WriteTo(xmlWriter)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E", _
                                          MethodBase.GetCurrentMethod.Name))

                Return textWriter.ToString()

            Finally
                xmlWriter = Nothing
            End Try

        End Using

    End Function

#End Region

#End Region


#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
