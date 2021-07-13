'-------------------------------------------------------------------------
'IC3802701BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：JobDispatch実績送信(ビジネスロジック)
'補足：JobDispatch実績送信(ビジネスロジック)の定数宣言用部分クラス
'作成：2013/12/13 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない
'更新：
'─────────────────────────────────────

Partial Class IC3802701BusinessLogic

#Region "JobDispatch実績送信関連"

    ''' <summary>
    ''' メッセージID 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageId As String = "IC45202"

    ''' <summary>
    ''' シーケンス番号採番に用いる日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeqNoNumberingFormat As String = "yyyyMMddHHmmss"

    ''' <summary>
    ''' 日付のフォーマット:yyyy/MM/dd HH:mm:ss
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateFormatYYYYMMddHHmmss As String = "yyyy/MM/dd HH:mm:ss"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

#Region "JobDispatch実績送信処理結果コード"

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

#Region "システム設定名"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' 国コード
    ''' </summary>
    Private Const SysCountryCode = "DIST_CD"

    ''' <summary>
    ''' 関連チップ送信フラグ
    ''' </summary>
    Private Const SysSendRelationStatus = "SEND_RELATION_STATUS"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const SysDateFormat = "DATE_FORMAT"

    ''' <summary>
    ''' 基幹連携URL（作業実績情報）
    ''' </summary>
    Private Const DlrSysLinkUrlJobRsltInfo = "LINK_URL_JOB_RSLT_INFO"

    ''' <summary>
    ''' ステータスコード変換フラグ
    ''' </summary>
    Private Const StatusCodeConvFlg = "STATUS_CD_CONV_FLG"

    ''' <summary>
    ''' スタッフ表示フラグ
    ''' </summary>
    Private Const StaffShowFlg = "SMB_STF_DISP_FLG"

    ''' <summary>
    ''' SOAPバージョン判定値
    ''' </summary>
    Private Const SysSoapVersion = "LINK_SOAP_VERSION"
#End Region

#Region "タグ名"

#Region "JobClockOnタグ"

    ''' <summary>
    ''' タグ名：JobClockOn
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobClockOn As String = "JobClockOn"

#End Region

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

#Region "DispatchInformationタグ"

    ''' <summary>
    ''' タグ名：DispatchInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDispatchInformation As String = "DispatchInformation"

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
    ''' タグ名：R_O
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRo As String = "R_O"

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

#Region "JobDetailタグ"

    ''' <summary>
    ''' タグ名：JobDetail
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobDetail As String = "JobDetail"

    ''' <summary>
    ''' タグ名：DispatchNo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDispatchNo As String = "DispatchNo"

    ''' <summary>
    ''' タグ名：JobSequenceNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobSequenceNumber As String = "JobSequenceNumber"

    ''' <summary>
    ''' タグ名：JobID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobId As String = "JobID"

    ''' <summary>
    ''' タグ名：GroupID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagGroupId As String = "GroupID"

    ''' <summary>
    ''' タグ名：Status
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStatus As String = "Status"

    ''' <summary>
    ''' タグ名：StartTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRsltStartTime As String = "StartTime"

    ''' <summary>
    ''' タグ名：EndTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRsltEndTime As String = "EndTime"

    ''' <summary>
    ''' タグ名：WorkTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWorkTime As String = "WorkTime"

    ''' <summary>
    ''' タグ名：FMスタッフコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FmAccount As String = "FM_ACCOUNT"

    ''' <summary>
    ''' タグ名：検査フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InspectionFlg As String = "INSPECTION_FLAG"

#Region "StopInformationタグ"

    ''' <summary>
    ''' タグ名：StopInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStopInformation As String = "StopInformation"

    ''' <summary>
    ''' タグ名：StopSequenceNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStopSequenceNumber As String = "StopSequenceNumber"

    ''' <summary>
    ''' タグ名：StopStart
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStopStart As String = "StopStart"

    ''' <summary>
    ''' タグ名：StopEnd
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStopEnd As String = "StopEnd"

    ''' <summary>
    ''' タグ名：StopReason
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStopReason As String = "StopReason"

#End Region

#End Region

#Region "StallInformationタグ"

    ''' <summary>
    ''' タグ名：StallInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallInfomation As String = "StallInformation"

    ''' <summary>
    ''' タグ名：StallID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallId As String = "StallID"

    ''' <summary>
    ''' タグ名：StartTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWorkStartTime As String = "StartTime"

    ''' <summary>
    ''' タグ名：EndTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWorkEndTime As String = "EndTime"

#Region "TechnicianInformationタグ"

    ''' <summary>
    ''' タグ名：TechnicianInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTechnicianInformation As String = "TechnicianInformation"

    ''' <summary>
    ''' タグ名：TechnicianID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTechnicianId As String = "TechnicianID"

#End Region

#Region "RestInformationタグ"

    ''' <summary>
    ''' タグ名：RestInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRestInfomation As String = "RestInformation"

    ''' <summary>
    ''' タグ名：StartTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagIdleStartTime As String = "StartTime"

    ''' <summary>
    ''' タグ名：EndTime
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagIdleEndTime As String = "EndTime"

#End Region

#End Region

#End Region

#End Region

#End Region

#Region "Responseタグ"

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
    ''' 関連チップ送信フラグ：送信しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendRelationChipFlg_NotSend As String = "1"

    ''' <summary>
    ''' 関連チップ送信フラグ：送信する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendRelationChipFlg_Send As String = "0"

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
    ''' 非稼働タイプ：休憩
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdleTypeRest As String = "1"

    ''' <summary>
    ''' ステータス送信フラグ：送信する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendStatus As String = "1"

    ''' <summary>
    ''' ストールに割り当てられたテクニシャンを表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffCodeShow As String = "0"

    ''' <summary>
    ''' DBのディフォルト日時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultDate As String = "1900/01/01 00:00:00"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' インターフェース区分：作業実績送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InterfaceTypeSendJobDispatch As String = "3"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

#Region "作業ステータス"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusWorking As String = "0"

    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusFinish As String = "1"

    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusStop As String = "2"

    ''' <summary>
    ''' 作業ステータス：作業前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusBeforeStart As String = "3"

#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStop As String = "05"
#End Region

#Region "中断理由区分"

    ''' <summary>
    ''' 99：その他、01：部品欠品、02：お客様連絡待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StopTypeOther As String = "99"

    ''' <summary>
    ''' 1：部品欠品
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StopTypeNoParts As String = "01"

    ''' <summary>
    ''' 2：お客様連絡待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StopTypeWaitApprovel As String = "02"

#End Region

    '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
#Region "制御用作業ステータス"
    ''' <summary>
    ''' 制御用作業ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusFinish As String = "103"
#End Region

#Region "基幹作業ステータス"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DmsJobStatusWorking As String = "1"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DmsJobStatusStop As String = "2"
#End Region
    '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

#End Region

#End Region
End Class
