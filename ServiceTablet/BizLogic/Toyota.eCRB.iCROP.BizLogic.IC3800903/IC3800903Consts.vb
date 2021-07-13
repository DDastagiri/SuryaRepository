'-------------------------------------------------------------------------
'IC3800903BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：予約情報送信(ビジネスロジック)
'補足：予約情報送信(ビジネスロジック)の定数宣言用部分クラス
'作成：2013/11/21 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：
'─────────────────────────────────────

Partial Class IC3800903BusinessLogic

#Region "予約送信関連"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendReservationId As String = "IC45201"

    ''' <summary>
    ''' シーケンス番号採番に用いる日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeqNoNumberingFormat As String = "yyyyMMddHHmmss"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

    ''' <summary>
    ''' ハイフン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Hyphen As String = "-"

#Region "予約送信処理結果コード"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Success As Integer = 0

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSysEnv As Integer = 1101

    ''' <summary>
    ''' 販売店システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDlrEnv As Integer = 1102

    ''' <summary>
    ''' 基幹コードマップ設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDmsCodeMap As Integer = 1103

    ''' <summary>
    ''' 顧客マスタエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCust As Integer = 1105

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
    ''' 作業内容テーブル更新エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorUpdateJobDtl As Integer = 9004

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

#Region "UpdateReserveタグ"

    ''' <summary>
    ''' タグ名：UpdateReserve
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagUpdateReserve As String = "UpdateReserve"

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
    Private Const TagVin1 As String = "Vin"

#End Region

#Region "UpdateReserveInformationタグ"

    ''' <summary>
    ''' タグ名：UpdateReserveInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagUpdateReserveInformation As String = "UpdateReserveInformation"

#Region "Reserve_CustomerInformationタグ"

    ''' <summary>
    ''' タグ名：Reserve_CustomerInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagReserveCustomerInformation As String = "Reserve_CustomerInformation"

    ''' <summary>
    ''' タグ名：CUSTCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustCode As String = "CUSTCD"

    ''' <summary>
    ''' タグ名：NewcustomerID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagNewCustomerId As String = "NewcustomerID"

    ''' <summary>
    ''' タグ名：CUSTOMERCLASS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerClass As String = "CUSTOMERCLASS"

    ''' <summary>
    ''' タグ名：CUSTOMERNAME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerName As String = "CUSTOMERNAME"

    ''' <summary>
    ''' タグ名：TELNO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTelNo As String = "TELNO"

    ''' <summary>
    ''' タグ名：MOBILE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMobile As String = "MOBILE"

    ''' <summary>
    ''' タグ名：EMAIL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEmail As String = "EMAIL"

    ''' <summary>
    ''' タグ名：ZIPCODE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagZipCode As String = "ZIPCODE"

    ''' <summary>
    ''' タグ名：ADDRESS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAddress As String = "ADDRESS"

#End Region

#Region "Reserve_VehicleInformationタグ"

    ''' <summary>
    ''' タグ名：Reserve_VehicleInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagReserveVehicleInformation As String = "Reserve_VehicleInformation"

    ''' <summary>
    ''' タグ名：VCLREGNO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVclRegNo As String = "VCLREGNO"

    ''' <summary>
    ''' タグ名：VIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVin2 As String = "VIN"

    ''' <summary>
    ''' タグ名：MAKERCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMakerCode As String = "MAKERCD"

    ''' <summary>
    ''' タグ名：SERIESCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSeriesCode As String = "SERIESCD"

    ''' <summary>
    ''' タグ名：SERIESNM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSeriesName As String = "SERIESNM"

    ''' <summary>
    ''' タグ名：BASETYPE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBaseType As String = "BASETYPE"

    ''' <summary>
    ''' タグ名：MILEAGE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMileage As String = "MILEAGE"

#End Region

#Region "Reserve_ServiceInformationタグ"

    ''' <summary>
    ''' タグ名：Reserve_ServiceInformation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagReserveServiceInformation As String = "Reserve_ServiceInformation"

    ''' <summary>
    ''' タグ名：STALLID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallId As String = "STALLID"

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
    ''' タグ名：WORKTIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWorkTime As String = "WORKTIME"

    ''' <summary>
    ''' タグ名：WASHFLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWashFlg As String = "WASHFLG"

    ''' <summary>
    ''' タグ名：INSPECTIONFLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagInspectionFlg As String = "INSPECTIONFLG"

    ''' <summary>
    ''' タグ名：MERCHANDISECD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMerchandiseCode As String = "MERCHANDISECD"

    ''' <summary>
    ''' タグ名：MNTNCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMntnCode As String = "MNTNCD"

    ''' <summary>
    ''' タグ名：SERVICECODE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagServiceCode As String = "SERVICECODE"

    ''' <summary>
    ''' タグ名：SERVICENAME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagServiceName As String = "SERVICENAME"

    ''' <summary>
    ''' タグ名：REZ_RECEPTION
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezReception As String = "REZ_RECEPTION"

    ''' <summary>
    ''' タグ名：REZ_PICK_DATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezPickDate As String = "REZ_PICK_DATE"

    ''' <summary>
    ''' タグ名：REZ_PICK_LOC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezPickLoc As String = "REZ_PICK_LOC"

    ''' <summary>
    ''' タグ名：REZ_PICK_TIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezPickTime As String = "REZ_PICK_TIME"

    ''' <summary>
    ''' タグ名：REZ_DELI_DATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezDeliDate As String = "REZ_DELI_DATE"

    ''' <summary>
    ''' タグ名：REZ_DELI_LOC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezDeliLoc As String = "REZ_DELI_LOC"

    ''' <summary>
    ''' タグ名：REZ_DELI_TIME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezDeliTime As String = "REZ_DELI_TIME"

#End Region

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
    ''' タグ名：PREZID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPrezId As String = "PREZID"

    ''' <summary>
    ''' タグ名：REZCHILDNO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezChildNo As String = "REZCHILDNO"

    ''' <summary>
    ''' タグ名：MEMO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMemo As String = "MEMO"

    ''' <summary>
    ''' タグ名：SACODE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSaCode As String = "SACODE"

    ''' <summary>
    ''' タグ名：ACCOUNT_PLAN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAccountPlan As String = "ACCOUNT_PLAN"

    ''' <summary>
    ''' タグ名：INPUTACCOUNT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagInputAccount As String = "INPUTACCOUNT"

    ''' <summary>
    ''' タグ名：UPDATEACCOUNT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>
    ''' タグ名：STATUS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStatus As String = "STATUS"

    ''' <summary>
    ''' タグ名：WALKIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagWalkIn As String = "WALKIN"

    ''' <summary>
    ''' タグ名：SMSFLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSmsFlg As String = "SMSFLG"

    ''' <summary>
    ''' タグ名：REZTYPE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRezType As String = "REZTYPE"

    ''' <summary>
    ''' タグ名：CANCELFLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCancelFlg As String = "CANCELFLG"

    ''' <summary>
    ''' タグ名：CREATEDATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCreateDate As String = "CREATEDATE"

    ''' <summary>
    ''' タグ名：UPDATEDATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagUpdateDate As String = "UPDATEDATE"

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

#Region "システム設定名"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' 管理作業内容ID送信フラグ
    ''' </summary>
    Private Const SysManageJobDtlIdSendFlg = "MNG_JOB_DTL_ID_SEND_FLG"

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
    ''' サブエリアストールID送信フラグ
    ''' </summary>
    Private Const SysSubAreaStallIdSendFlg = "SUBAREA_STALL_ID_SEND_FLG"

    ''' <summary>
    ''' 仮予約更新キャンセルフラグ
    ''' </summary>
    Private Const SysTentativeUpdateCancelFlg = "TENTATIVE_UPDATE_CANCEL_FLG"

    ''' <summary>
    ''' SOAPバージョン判定値
    ''' </summary>
    Private Const SysSoapVersion = "LINK_SOAP_VERSION"

    ''' <summary>
    ''' CDATA付与フラグ
    ''' </summary>
    Private Const SysCDataApdFlg = "CDATA_APD_FLG"

#End Region

#Region "販売店システム設定名"

    ''' <summary>
    ''' 基幹連携URL（予約情報）
    ''' </summary>
    Private Const DlrSysLinkUrlResvInfo = "LINK_URL_RESV_INFO"

#End Region

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
    ''' 仮置き(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeTemp As String = "23"

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

#Region "受付区分"

    ''' <summary>
    ''' WalkIn
    ''' </summary>
    Private Const AcceptanceTypeWalkin As String = "1"

#End Region

#Region "顧客種別"

    ''' <summary>
    ''' 未取引客
    ''' </summary>
    Private Const CustomerTypeNew As String = "2"

#End Region

#Region "顧客車両区分"

    ''' <summary>
    ''' 保険(基幹連携予約送信時、未取引客の区分）
    ''' </summary>
    Private Const CustomerVehicleTypeNew As String = "4"

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

#Region "サービス分類区分"

    ''' <summary>
    ''' PDS
    ''' </summary>
    Private Const ServiceClassTypePds As String = "4"

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
    ''' 予約送信フラグ：送信する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendReserve As String = "1"

    ''' <summary>
    ''' 予約送信フラグ：送信しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotSendReserve As String = "0"

    ''' <summary>
    ''' 来店送信フラグ：送信しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotSendWalkIn As String = "0"

    ''' <summary>
    ''' PDS送信フラグ：送信しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotSendPds As String = "0"

    ''' <summary>
    ''' CDATA付与フラグ：付与する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppendCData As String = "1"

    ''' <summary>
    ''' 日付フォーマット(yyyy/MM/dd HH:mm:ss)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const yyyyMMddHHmmssDateFormat As String = "yyyy/MM/dd HH:mm:ss"

    ''' <summary>
    ''' 最小日時(文字列型)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const yyyyMMddHHmmssMinDate As String = "1900/01/01 00:00:00"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' インターフェース区分：予約送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InterfaceTypeSendReserve As String = "1"

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start

    ''' <summary>
    ''' 文言紐付けマスタ区分種別コード（予約情報送信（G07）のResultId）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TypeCodeRsltIdSendResvTablet As String = "RSLT_ID_SEND_RESV_TABLET"

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

#End Region

#Region "国コード"

    ''' <summary>
    ''' 国コード：TMT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DistCodeTH As String = "TH"

#End Region

End Class
