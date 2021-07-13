'------------------------------------------------------------------------------
'SC3080103.aspx.vb
'------------------------------------------------------------------------------
'機能：顧客検索結果一覧
'補足：
'作成： 2013/12/20 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新： 2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
'更新： 2017/02/20 NSK 竹中 TR-SVT-TMT-20161111-001 Welcome Board で顧客氏名が2行にわたって表示される
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.BizLogic.IC3811501BusinessLogic
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.DataAccess.IC3811501DataSet
Imports Toyota.eCRB.SMBLinkage.Customer.BizLogic.IC3810203BusinessLogic
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSet
Imports Toyota.eCRB.CustomerInfo.Search.BizLogic
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.Common.OtherLinkage.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports System.Web.Script.Serialization


Partial Class Pages_SC3080103
    Inherits BasePage


#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3080103"
    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Result_Success As Long = 0
    ''' <summary>
    ''' 失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Result_Fail As Long = -1
    ''' <summary>
    ''' DateTimeFuncにて、"yyyyMMdd"形式にコンバートするための定数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERTDATE_YMD As Integer = 9
    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonclick({0});"
    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_SA As String = "SC3140103"
    ''' <summary>
    ''' メインメニュー(SM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_SM As String = "SC3220201"
    ''' <summary>
    ''' メインメニュー(CT)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_CT As String = "SC3200101"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_FM As String = "SC3230101"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"
    ''' <summary>
    ''' 新規顧客登録画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEW_CUSTOMER_PAGE As String = "SC3080207"
    ''' <summary>
    ''' 顧客詳細画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_DETAIL_PAGE As String = "SC3080225"
    ''' <summary>
    ''' R/O一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_LIST_PAGE As String = "SC3160101"
    ''' <summary>
    ''' 追加作業一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_LIST_PAGE As String = "SC3170101"
    ''' <summary>
    ''' 完成検査一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERVER_CHECK_LIST_PAGE As String = "SC3180101"
    ''' <summary>
    ''' 予約管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_VSTMANAGER As String = "SC3100303"
    ''' <summary>
    ''' R/O参照画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_DETAIL_PAGE As String = "SPAD001"
    ''' <summary>
    ''' スケジューラボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_SCHEDULER As String = "return schedule.appExecute.executeCaleNew();"
    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

    ''' <summary>
    ''' VISITSEQ既定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISITSEQ_DEFAULT_VALUE As Long = 0
    ''' <summary>
    ''' 事前準備チップフラグ（0：事前準備チップ以外に対する顧客登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PREPEARENCE_CHIP_FLG As String = "0"

    ''' <summary>
    ''' ソートフラグ（0：ソートなし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SORT_TYPE_NONE As String = "0"
    ''' <summary>
    ''' ソートフラグ（1：昇順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SORT_TYPE_ASC As String = "1"
    ''' <summary>
    ''' ソートフラグ（2：降順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SORT_TYPE_DESC As String = "2"

    ''' <summary>
    ''' ヘッダー検索タイプのSESSIONKEY (1：車両登録No、2：顧客名称、3：VIN、4：電話番号/携帯番号、5：RO番号 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHTYPE As String = "searchType"
    ''' <summary>
    ''' ヘッダー検索文字列のSESSIONKEY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHSTRING As String = "searchString"

    ''' <summary>
    ''' VIP区分（1：VIP）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIP_FLG As String = "1"

    ''' <summary>
    ''' 顧客区分（1：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_PERSONAL As String = "1"
    ''' <summary>
    ''' 顧客区分（2：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_MY_COMPANY As String = "2"
    ''' <summary>
    ''' 顧客区分（3：Other）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_OTHER As String = "3"
    ''' <summary>
    ''' 法人フラグ（0：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CORPORATION_TYPE_MINE As String = "0"
    ''' <summary>
    ''' 予約ありフラグ（1：予約あり）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPOITMENT_FLG As String = "1"

    ''' <summary>
    ''' 本販売店客（0：本販売店客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PURCHASE_DLR_CUSTOMER As String = "0"
    ''' <summary>
    ''' 他販売店客（1：他販売店客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_DLR_CUSTOMER As String = "1"
    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
    ''' <summary>
    ''' SSC対象顧客（1：SSC対象）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SSC_CUSTOMER As String = "1"
    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
    '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' アイコンフラグ1（1：M/E/T/P対象）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_1 As String = "1"
    ''' <summary>
    ''' アイコンフラグ2（2：B/L対象）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_2 As String = "2"
    '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT As String = "SC3080103_DEFAULT_READ_COUNT"
    ''' <summary>
    ''' 検索最大表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_DISPLAY_COUNT As String = "SC3080103_MAX_DISPLAY_COUNT"

    ''' <summary>
    ''' Sessionキー（ストール利用ID）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_STALL_USE_ID As String = "Session.STALL_USE_ID"
    ''' <summary>
    ''' Sessionキー（開始日時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DATE As String = "Session.DATE"
    ''' <summary>
    ''' Sessionキー（ストール利用ステータス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_STALL_USE_STATUS As String = "Session.STALL_USE_STATUS"
    ''' <summary>
    ''' Sessionキー（サブチップフラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SUB_CHIP_TYPE As String = "Session.SUB_CHIP_TYPE"
    ''' <summary>
    ''' Sessionキー（サービスステータス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SVC_STATUS As String = "Session.SVC_STATUS"
    ''' <summary>
    ''' Sessionキー（完成検査ステータス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_INSPECTION_STATUS As String = "Session.INSPECTION_STATUS"

    ''' <summary>
    ''' Sessionキー（販売店コード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CRDEALERCODE As String = "Redirect.CRDEALERCODE"
    ''' <summary>
    ''' Sessionキー（来店管理連番）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VISITSEQ As String = "Redirect.VISITSEQ"
    ''' <summary>
    ''' Sessionキー（予約ID）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_REZID As String = "Redirect.REZID"
    ''' <summary>
    ''' Sessionキー（顧客名）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_NAME As String = "Redirect.NAME"
    ''' <summary>
    ''' Sessionキー（車両登録No）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_REGISTERNO As String = "Redirect.REGISTERNO"
    ''' <summary>
    ''' Sessionキー（VIN）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VINNO As String = "SessionKey.VIN"
    ''' <summary>
    ''' Sessionキー（DMS_CST_ID）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DMS_CST_ID As String = "SessionKey.DMS_CST_ID"
    ''' <summary>
    ''' Sessionキー（モデルコード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_MODELCODE As String = "Redirect.MODELCODE"
    ''' <summary>
    ''' Sessionキー（電話番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TEL1 As String = "Redirect.TEL1"
    ''' <summary>
    ''' Sessionキー（携帯番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TEL2 As String = "Redirect.TEL2"
    ''' <summary>
    ''' Sessionキー（担当SAコード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SACODE As String = "Redirect.SACODE"
    ''' <summary>
    ''' Sessionキー（事前準備フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_PREPARECHIPFLAG As String = "Redirect.PREPARECHIPFLAG"
    ''' <summary>
    ''' Sessionキー（受付フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_RECEPTIONFLAG As String = "Redirect.RECEPTIONFLAG"
    ''' <summary>
    ''' Sessionキー（固定フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_FLAG As String = "Redirect.FLAG"

    ''' <summary>
    ''' 写真URL取得のParameterName
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IMAGE_FILE_PATH As String = "FACEPIC_UPLOADURL"
    ''' <summary>
    ''' 写真無画像のファイルパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IMAGE_FILE_NO_PHOTO As String = "../Styles/Images/SC3080103/no_photo.png"

    ''' <summary>
    ''' 行追加ステータス（0：追加していない行）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOff As String = "0"
    ''' <summary>
    ''' 行追加ステータス（1：追加した行）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOn As String = "1"

    ''' <summary>
    ''' サービスステータス（07：洗車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitWash As String = "07"
    ''' <summary>
    ''' サービスステータス（08：洗車中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWashing As String = "08"
    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    ''' <summary>
    ''' ストール利用テータス（05：中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStop As String = "05"
    ''' <summary>
    ''' ストール利用テータス（07：未来店客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusNoVisitor As String = "07"

    ''' <summary>
    ''' 完成検査ステータス（1：完成検査承認待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApprovalStatusWaitApproval As String = "1"

    ''' <summary>
    ''' 予約ステータス（0：仮予約）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReserveStatusDummy As String = "0"
    ''' <summary>
    ''' 予約ステータス（1：本予約）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReserveStatusMaster As String = "1"

    ''' <summary>
    ''' 事前準備フラグ(0：事前準備チップ以外)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AdvancePreparationTypeOff As String = "0"
    ''' <summary>
    ''' 事前準備フラグ(1：事前準備チップ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AdvancePreparationTypeOn As String = "1"

    ''' <summary>
    ''' 固定フラグ（1固定）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FixationTypeOne As String = "1"

    ''' <summary>
    ''' 敬称タイプ：名前後方
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBack As String = "1"

    ''' <summary>
    ''' 敬称タイプ：名前前方
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeFoward As String = "2"

    ''' <summary>
    ''' 受付区分（0：受付エリアのROがあり、受付エリア以外）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitTypeOff As String = "0"
    ''' <summary>
    ''' 受付区分（1：受付エリアのROがなし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitTypeOn As String = "1"

    ''' <summary>
    ''' 新規顧客登録確認フラグ（0：何もしない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewCustomerCheckNone As String = "0"
    ''' <summary>
    ''' 新規顧客登録確認フラグ（1：確認メッセージ表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewCustomerCheckConfirm As String = "1"
    ''' <summary>
    ''' 新規顧客登録確認フラグ（2：新規顧客登録をする）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewCustomerCheckRedirectNewCustomer As String = "2"

    ''' <summary>
    ''' RO一覧ポップアップ表示フラグ（0：何もしない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderListPopupTypeNone As String = "0"
    ''' <summary>
    ''' RO一覧ポップアップ表示フラグ（1：表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderListPopupTypeDisplay As String = "1"

    '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START

    ''' <summary>
    ''' RO一覧顧客氏名表示フラグ (0:非表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderListCstNameTypeHidden As String = "0"

    ''' <summary>
    ''' RO一覧顧客氏名表示フラグ (1:表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderListCstNameTypeDisplay As String = "1"

    '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

    ''' <summary>
    ''' １つスベース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ONE_SPACE As String = " "

    ''' <summary>
    ''' 現地にシステム連携用画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOCAL_TACT_PAGE As String = "SC3010501"

    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"

    ''' <summary>
    ''' プログラムID：来店管理画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISIT_MANAGER_PAGE As String = "SC3100303"

    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_FM As String = "SC3230101"

    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_ADD_LIST As Long = 22

#Region "SESSION KEY"

    ''' <summary>
    ''' SessionKey(DealerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_DEALERCODE As String = "DealerCode"
    ''' <summary>
    ''' SessionKey(BranchCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BRANCHCODE As String = "BranchCode"
    ''' <summary>
    ''' SessionKey(LoginUserID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_LOGINUSERID As String = "LoginUserID"
    ''' <summary>
    ''' SessionKey(SAChipID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SACHIPID As String = "SAChipID"
    ''' <summary>
    ''' SessionKey(BASREZID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BASREZID As String = "BASREZID"
    ''' <summary>
    ''' SessionKey(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_RO As String = "R_O"
    ''' <summary>
    ''' SessionKey(SEQ_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SEQ_NO As String = "SEQ_NO"
    ''' <summary>
    ''' SessionKey(VIN_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIN_NO As String = "VIN_NO"
    ''' <summary>
    ''' SessionKey(CustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_CUSTOMERID As String = "CustomerID"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIEWMODE As String = "ViewMode"

    ''' <summary>
    ''' Sessionキー（整備受注No）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_ORDERNO As String = "OrderNo"
    ''' <summary>
    ''' SessionKey(DearlerCode):ログインユーザーのDMS販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DEARLER_CODE As String = "Session.Param1"
    ''' <summary>
    ''' SessionKey(BranchCode):ログインユーザーのDMS店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BRANCH_CODE As String = "Session.Param2"
    ''' <summary>
    ''' SessionKey(LoginUserID):ログインユーザーのアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_LOGIN_USER_ID As String = "Session.Param3"
    ''' <summary>
    ''' SessionKey(SAChipID):来店管理番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SA_CHIP_ID As String = "Session.Param4"
    ''' <summary>
    ''' SessionKey(BASREZID):DMS予約ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BASREZID As String = "Session.Param5"
    ''' <summary>
    ''' SessionKey(R_O):RO番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_R_O As String = "Session.Param6"
    ''' <summary>
    ''' SessionKey(SEQ_NO):RO作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SEQ_NO As String = "Session.Param7"
    ''' <summary>
    ''' SessionKey(VIN_NO):車両登録No.のVIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIN_NO As String = "Session.Param8"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIEW_MODE As String = "Session.Param9"
    ''' <summary>
    ''' SessionKey(Format)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_FORMAT As String = "Session.Param10"
    ''' <summary>
    ''' SessionKey(CustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CUSTOMER_ID As String = "Session.Param10"

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

    ''' <summary>
    ''' SessionKey(ContactParson)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CONTACT_PARSON As String = "Session.Param11"

    ''' <summary>
    ''' SessionKey(ContactTEL)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CONTACT_TELNO As String = "Session.Param12"

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

    ''' <summary>
    ''' SessionKey(DISP_NUM)：画面番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"

    ''' <summary>
    ''' SessionValue(ViewMode)：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_EDIT As String = "0"

    ''' <summary>
    ''' SessionValue(ViewMode)：参照
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_READ As String = "1"

    ''' <summary>
    ''' SessionValue(DISP_NUM)：「1：R/O作成」固定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISP_NUM_ROCREATE As String = "1"

    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_RO_LIST As String = "14"

    ''' <summary>
    ''' SessionValue(画面番号)：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_CAMPAIGN As String = "15"

#End Region

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>顧客検索</summary>
        id001 = 1
        ''' <summary>合計{0}件</summary>
        id002 = 2
        ''' <summary>保有車両</summary>
        id003 = 3
        ''' <summary>お客様</summary>
        id004 = 4
        ''' <summary>携帯</summary>
        id005 = 5
        ''' <summary>/</summary>
        id006 = 6
        ''' <summary>自宅</summary>
        id007 = 7
        ''' <summary>SA</summary>
        id008 = 8
        ''' <summary>SC</summary>
        id009 = 9
        ''' <summary>V</summary>
        id010 = 10
        ''' <summary>個</summary>
        id011 = 11
        ''' <summary>法</summary>
        id012 = 12
        ''' <summary>自</summary>
        id013 = 13
        ''' <summary>未</summary>
        id014 = 14
        ''' <summary>A</summary>
        id015 = 15
        ''' <summary>前の{0}件を読み込む…</summary>
        id016 = 16
        ''' <summary>前の{0}件を読み込み中…</summary>
        id017 = 17
        ''' <summary>次の{0}件を読み込む…</summary>
        id018 = 18
        ''' <summary>次の{0}件を読み込み中…</summary>
        id019 = 19
        ''' <summary>-</summary>
        id020 = 20
        ''' <summary>R/Oまたは予約が既に存在します</summary>
        id021 = 21
        ''' <summary>キャンセル</summary>
        id022 = 22
        ''' <summary>R/O作成</summary>
        id023 = 23
        ''' <summary>検索結果が0件です。</summary>
        id026 = 26
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        ''' <summary>S</summary>
        id027 = 27
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>M</summary>
        id10001 = 10001
        ''' <summary>B</summary>
        id10002 = 10002
        ''' <summary>E</summary>
        id10003 = 10003
        ''' <summary>T</summary>
        id10004 = 10004
        ''' <summary>P</summary>
        id10005 = 10005
        ''' <summary>L</summary>
        id10006 = 10006
        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ''' <summary>データベースへのアクセスにてタイムアウトが発生しました。再度実行して下さい。</summary>
        id901 = 901
        ''' <summary>予約キャンセルされたため、選択できません。</summary>
        id902 = 902
        ''' <summary>作業が開始されたため、ROの編集はできません。</summary>
        id903 = 903
        ''' <summary>予期せぬエラーが発生したため、顧客検索できませんでした。</summary>
        id904 = 904
    End Enum

#End Region

#Region "初期処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim staffInfo As StaffContext = StaffContext.Current

        '初回読み込み時
        If Not IsPostBack Then
            'session情報を取得し格納する
            Dim sessionsearchtype As String = _
                CType(GetValue(ScreenPos.Current, SESSION_KEY_SERCHTYPE, False), String)
            Dim sessionsearchvalue As String = _
                CType(GetValue(ScreenPos.Current, SESSION_KEY_SERCHSTRING, False), String)
            Me.HiddenSearchType.Value = sessionsearchtype
            Me.HiddenSearchValue.Value = sessionsearchvalue

            ''権限情報保持
            Me.HiddenOperationCode.Value = CType(staffInfo.OpeCD, String)

            'ヘッダーのテキストエリアの値設定
            CType(FindControl("ctl00$MstPG_CustomerSearchTextBox"), TextBox).Text = sessionsearchvalue

            '顧客一覧：初期ソート値を設定
            Me.HiddenRegisterSortType.Value = SORT_TYPE_DESC
            Me.HiddenCustomerSortType.Value = SORT_TYPE_NONE
            Me.HiddenSASortType.Value = SORT_TYPE_NONE
            Me.HiddenSCSortType.Value = SORT_TYPE_NONE

            '顧客一覧：初期表示件数を設定
            Dim systemEnv As New SystemEnvSetting
            Dim loadCount As String = systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT).PARAMVALUE
            Dim maxDisplayCount As String = systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT).PARAMVALUE
            Me.HiddenStartIndex.Value = "1"
            Me.HiddenEndIndex.Value = loadCount
            Me.HiddenLoadCount.Value = loadCount
            Me.HiddenMaxDisplayCount.Value = maxDisplayCount

            '顧客一覧：ヘッダー
            Me.RegisterHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id003)
            Me.CustomerHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id004)
            Me.TelMobileHeader.Text = String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id005), _
                                                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id006), _
                                                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id007))
            Me.SAHeader.Text = String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id008))
            Me.SCHeader.Text = String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id009))

            '顧客一覧：ページング文言
            Me.BackPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id016).Replace("{0}", loadCount)
            Me.BackPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id017).Replace("{0}", loadCount)
            Me.NextPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id018).Replace("{0}", loadCount)
            Me.NextPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id019).Replace("{0}", loadCount)

            '顧客一覧：取得件数0件文言
            Me.NoSearchWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id026)

            'RO一覧：ヘッダーフッター文言
            Me.PopUpOrderListHeaderLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id021)
            Me.PopUpOrderListFooterButton.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id022)

            '制御用
            Me.HiddenOrderListDisplayType.Value = OrderListPopupTypeNone
            Me.HiddenNewCustomerConfirmType.Value = NewCustomerCheckNone

            'RO作成
            Me.ROCreate.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id023)
        End If

        'フッター設定
        Me.InitFooterButton()

    End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' ハイライトフッター設定
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '権限チェック
        If StaffContext.Current.OpeCD = Operation.SA OrElse StaffContext.Current.OpeCD = Operation.SM Then
            '顧客詳細フッターボタンハイライト
            category = FooterMenuCategory.CustomerDetail
        Else
            '顧客詳細フッターボタンハイライト
            category = FooterMenuCategory.MainMenu
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub InitFooterButton()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        If Not IsNothing(mainMenuButton) Then
            AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
            mainMenuButton.OnClientClick = "return FooterButtonControl();"
        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        If Not IsNothing(telDirectoryButton) Then
            telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL
        End If

        Dim inStaffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SA OrElse inStaffInfo.OpeCD = Operation.SM Then

            '顧客詳細ボタンの設定
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            If Not IsNothing(customerButton) Then
                customerButton.OnClientClick = "return false ;"
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = "return FooterButtonControl();"
            End If

            '商品訴求ボタン
            Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            If Not IsNothing(footerGoodsSolicitationContentsButton) Then
                AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
                footerGoodsSolicitationContentsButton.OnClientClick = "return FooterButtonControl();"
            End If

            'キャンペーンボタン
            Dim footerCampaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            If Not IsNothing(footerCampaignButton) Then
                AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
                footerCampaignButton.OnClientClick = "return FooterButtonControl();"
            End If

            '予約管理ボタンの設定
            Dim addReserveManagement As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
            If Not IsNothing(addReserveManagement) Then
                AddHandler addReserveManagement.Click, AddressOf ReserveManagement_Click
                addReserveManagement.OnClientClick = "return FooterButtonControl();"
            End If

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            If Not IsNothing(smbButton) Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = "return FooterButtonControl();"
            End If

        End If

        '権限チェック
        If inStaffInfo.OpeCD = Operation.CHT Then

            'TCメインボタンの設定
            Dim tcMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TechnicianMain)
            If Not IsNothing(tcMainButton) Then
                tcMainButton.OnClientClick = "return FooterButtonClick(" + FooterMenuCategory.TechnicianMain.ToString() + ");"
            End If
            '2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            tcMainButton.Enabled = False
            '2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

            'FMメインボタンの設定
            Dim fmMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ForemanMain)
            If Not IsNothing(fmMainButton) Then
                AddHandler fmMainButton.Click, AddressOf FormanMainButton_Click
                fmMainButton.OnClientClick = "return FooterButtonControl();"
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = "return FooterButtonControl();"
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = "return FooterButtonControl();"
            End If

        End If

        '権限チェック
        If inStaffInfo.OpeCD = Operation.FM Then

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            If Not IsNothing(smbButton) Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = "return FooterButtonControl();"
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = "return FooterButtonControl();"
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = "return FooterButtonControl();"
            End If

        End If

        '権限チェック
        If inStaffInfo.OpeCD = Operation.CT Then

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = "return FooterButtonControl();"
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = "return FooterButtonControl();"
            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.SA Then
            'メインメニュー(SA)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_SA)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            'メインメニュー(SM)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_SM)

        ElseIf staffInfo.OpeCD = Operation.CT Then
            'メインメニュー(CT)に遷移する
            Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_FM)
        ElseIf staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/Oボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080103BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If


            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_RO_LIST)

        End Using

        '決定した遷移先に遷移
        Me.RedirectNextScreen(LOCAL_TACT_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 追加作業ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3010501BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", SESSION_DATA_DISP_NUM_ADD_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    'Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")))
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dtDmsCodeMapDataTable(0).ACCOUNT)
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(LOCAL_TACT_PAGE)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' FMメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub FormanMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニュー(FM)画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_FM)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 予約管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub ReserveManagement_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '予約管理画面に遷移する
        Me.RedirectNextScreen(APPLICATIONID_VSTMANAGER)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "イベント"

    ''' <summary>
    ''' 初期表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub MainAreaReload_Click(sender As Object, e As System.EventArgs) Handles MainAreaReload.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '初期表示の検索を行う
        Me.SetCustomerList(Me.HiddenRegisterSortType.Value, _
                           Me.HiddenCustomerSortType.Value, _
                           Me.HiddenSASortType.Value, _
                           Me.HiddenSCSortType.Value, _
                           1, _
                           CType(Me.HiddenEndIndex.Value, Long))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 車両ソート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub RegisterSortButton_Click(sender As Object, e As System.EventArgs) Handles RegisterSortButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '車両ソートをして検索を行う
        If SORT_TYPE_ASC.Equals(Me.HiddenRegisterSortType.Value) Then
            '車両降順、顧客・SA・SCソートなし
            Me.SetCustomerList(SORT_TYPE_DESC, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            '車両昇順、顧客・SA・SCソートなし
            Me.SetCustomerList(SORT_TYPE_ASC, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客ソート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub CustomerSortButton_Click(sender As Object, e As System.EventArgs) Handles CustomerSortButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '顧客ソートをして検索を行う
        If SORT_TYPE_ASC.Equals(Me.HiddenCustomerSortType.Value) Then
            '車両・SA・SCソートなし、顧客昇順
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_DESC, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            '車両・SA・SCソートなし、顧客降順
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_ASC, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SAソート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub SASortButtonButton_Click(sender As Object, e As System.EventArgs) Handles SASortButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SAソートをして検索を行う
        If SORT_TYPE_ASC.Equals(Me.HiddenSASortType.Value) Then
            'SA降順、顧客・車両・SCソートなし
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_DESC, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            'SA昇順、顧客・車両・SCソートなし
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_ASC, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SCソート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub SCSortButtonButton_Click(sender As Object, e As System.EventArgs) Handles SCSortButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SCソートをして検索を行う
        If SORT_TYPE_ASC.Equals(Me.HiddenSCSortType.Value) Then
            'SC降順、顧客・車両・SAソートなし
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_DESC, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            'SC昇順、顧客・車両・SAソートなし
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_NONE, _
                               SORT_TYPE_ASC, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 前の50件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub BackPageButton_Click(sender As Object, e As System.EventArgs) Handles BackPageButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim searcdStartIndex As Long
        Dim searcdEndIndex As Long

        '現在のページ情報を取得する
        Dim startIndex As Long = CType(Me.HiddenStartIndex.Value, Long)
        Dim endIndex As Long = CType(Me.HiddenEndIndex.Value, Long)
        Dim loadCount As Long = CType(Me.HiddenLoadCount.Value, Long)
        Dim maxDisplayCount As Long = CType(Me.HiddenMaxDisplayCount.Value, Long)

        ' 開始行の設定
        Dim setStartMin As Long = startIndex - loadCount
        If setStartMin <= 0 Then
            searcdStartIndex = 1
        Else
            searcdStartIndex = setStartMin
        End If
        ' 終了行の設定
        Dim setEndMin As Long = endIndex - searcdStartIndex + 1
        If setEndMin < maxDisplayCount Then
            searcdEndIndex = endIndex
        Else
            searcdEndIndex = searcdStartIndex + maxDisplayCount - 1
        End If

        '前の50件を表示させる
        Me.SetCustomerList(Me.HiddenRegisterSortType.Value, _
                           Me.HiddenCustomerSortType.Value, _
                           Me.HiddenSASortType.Value, _
                           Me.HiddenSCSortType.Value, _
                           searcdStartIndex, _
                           searcdEndIndex)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 次の50件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub NextPageButton_Click(sender As Object, e As System.EventArgs) Handles NextPageButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim searcdStartIndex As Long
        Dim searcdEndIndex As Long

        '現在のページ情報を取得する
        Dim startIndex As Long = CType(Me.HiddenStartIndex.Value, Long)
        Dim endIndex As Long = CType(Me.HiddenEndIndex.Value, Long)
        Dim loadCount As Long = CType(Me.HiddenLoadCount.Value, Long)
        Dim maxDisplayCount As Long = CType(Me.HiddenMaxDisplayCount.Value, Long)

        ' 終了行の設定
        searcdEndIndex = endIndex + loadCount

        ' 開始行の設定
        Dim setStartMax As Long = searcdEndIndex - startIndex + 1
        If setStartMax <= maxDisplayCount Then
            searcdStartIndex = startIndex
        Else
            searcdStartIndex = searcdEndIndex - maxDisplayCount + 1

            If searcdStartIndex <= 0 Then
                searcdStartIndex = 1
            End If
        End If

        '次の50件を表示させる
        Me.SetCustomerList(Me.HiddenRegisterSortType.Value, _
                           Me.HiddenCustomerSortType.Value, _
                           Me.HiddenSASortType.Value, _
                           Me.HiddenSCSortType.Value, _
                           searcdStartIndex, _
                           searcdEndIndex)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客エリアタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub CustomerAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles CustomerAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.RedirectCustomerDetailPage(CType(Me.HiddenSelectDMSCSTID.Value, String), _
                                      CType(Me.HiddenSelectVIN.Value, String))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 予約ポップアップの新規ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub ROCreateButton_Click(sender As Object, e As System.EventArgs) Handles ROCreateButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'RO作成画面へ遷移処理
        Dim returnCode As Long = Me.RedirectROCreate()

        If returnCode <> Result_Success Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Me.ShowMessageBox(WordId.id904)

            Return
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 車両エリアタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Protected Sub VehicleAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles VehicleAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SA権限以外の場合、何も処理しない
        If Not staffInfo.OpeCD = Operation.SA Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return

        End If

        'RO情報確認
        Using biz As New SC3080103BusinessLogic
            Try
                'RO情報の件数確認
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)                 ' 現在の日付を取得
                'Dim strBaseDate = String.Format("{0:yyyyMMdd}", nowDate)                ' 予約情報の取得基準日を「翌日」に設定

                ' 現在の日付を取得
                Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
                ' 予約情報の取得基準日を「翌日」に設定
                Dim strBaseDate = String.Format(CultureInfo.CurrentCulture, "{0:yyyyMMdd}", nowDate)
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                '自社客の予約のみ取得
                Dim dt As SC3080103DataSet.SC3080103ReserveInfoDataTable = _
                    biz.GetReservationList(staffInfo.DlrCD, _
                                     staffInfo.BrnCD, _
                                     CType(Me.HiddenSelectDMSCSTID.Value, String), _
                                     CType(Me.HiddenSelectVehRegNo.Value, String), _
                                     CType(Me.HiddenSelectVIN.Value, String), _
                                     strBaseDate, _
                                     nowDate, _
                                     True)
                If IsNothing(dt) Then
                    '見込み客の予約を取得
                    dt = biz.GetReservationList(staffInfo.DlrCD, _
                                     staffInfo.BrnCD, _
                                     CType(Me.HiddenSelectDMSCSTID.Value, String), _
                                     CType(Me.HiddenSelectVehRegNo.Value, String), _
                                     CType(Me.HiddenSelectVIN.Value, String), _
                                     strBaseDate, _
                                     nowDate, _
                                     False)
                    Me.HiddenOrderListCstNameType.Value = OrderListCstNameTypeDisplay
                Else
                    Me.HiddenOrderListCstNameType.Value = OrderListCstNameTypeHidden
                End If
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END
                If IsNothing(dt) Then
                    'RO作成画面へ遷移処理
                    Dim returnCode As Long = Me.RedirectROCreate()

                    If returnCode <> Result_Success Then
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        Return
                    End If
                Else
                    '取得できた場合
                    Dim serviceInCount = dt.Count

                    If 0 < serviceInCount Then
                        '1件以上ある場合はポップアップを表示する
                        Me.HiddenOrderListDisplayType.Value = OrderListPopupTypeDisplay
                        Me.SetOrderListData(dt)

                    Else
                        'RO作成画面へ遷移処理
                        Dim returnCode As Long = Me.RedirectROCreate()

                        If returnCode <> Result_Success Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} END" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
                            Return
                        End If

                    End If
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try
        End Using

        'ボタンエリアの情報を更新
        Me.ContentUpdateButtonPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' RO一覧エリアタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </history>
    Protected Sub OrderAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles OrderAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '予約情報確認
        Using biz As New SC3080103BusinessLogic
            Try
                '予約情報を取得
                If Not String.IsNullOrEmpty(Me.HiddenSelectSvcIn.Value) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    'If Not Me.HiddenSelectSvcIn.Value = String.Empty Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                    Dim dt As SC3080103DataSet.SC3080103StallUseInfoDataTable = _
                        biz.GetStallUseInfo(CType(Me.HiddenSelectSvcIn.Value, Decimal))

                    '件数の確認
                    If dt.Count = 0 Then
                        '取得できなかった場合はエラーメッセージを表示する
                        Me.ShowMessageBox(WordId.id902)

                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CloseOrderList", "CloseOrderList();", True)

                    Else

                        'R/O作成画面に遷移する処理
                        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                        'Me.RedirectROCreate(CType(Me.HiddenSelectSvcIn.Value, String), _
                        '                    CType(Me.HiddenSelectOrderNumber.Value, String), _
                        '                    CType(Me.HiddenSelectRoJobSeq.Value, String), _
                        '                    CType(Me.HiddenSelectDmsJobDtlId.Value, String), _
                        '                    True)

                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                        '来店情報の来店者氏名と来店者電話番号の登録・更新の際、
                        '予約IDが存在し、サービス入庫情報が取得できなかった場合の処理
                        'Before:黒い画面が表示 → After:エラーメッセージを表示

                        'Me.RedirectROCreate(Me.HiddenSelectSvcIn.Value, _
                        '                    Me.HiddenSelectOrderNumber.Value, _
                        '                    Me.HiddenSelectRoJobSeq.Value, _
                        '                    Me.HiddenSelectDmsJobDtlId.Value, _
                        '                    True)

                        Dim resultCode As Long = Me.RedirectROCreate(Me.HiddenSelectSvcIn.Value, _
                                                                     Me.HiddenSelectOrderNumber.Value, _
                                                                     Me.HiddenSelectRoJobSeq.Value, _
                                                                     Me.HiddenSelectDmsJobDtlId.Value, _
                                                                     True)

                        If resultCode <> Result_Success Then

                            '取得できなかった場合はエラーメッセージを表示する
                            Me.ShowMessageBox(WordId.id902)

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "CloseOrderList", "CloseOrderList();", True)

                        End If

                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    End If

                Else
                    Me.ShowMessageBox(WordId.id902)

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "CloseOrderList", "CloseOrderList();", True)
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "顧客一覧表示処理"

    ''' <summary>
    ''' 顧客一覧表示処理
    ''' </summary>
    ''' <param name="inRegisterSortType">車両ソートフラグ</param>
    ''' <param name="inCustomerSortType">顧客ソートフラグ</param>
    ''' <param name="inSASortType">SAソートフラグ</param>
    ''' <param name="inSCSortType">SCソートフラグ</param>
    ''' <param name="inStartRow">開始行番号</param>
    ''' <param name="inEndRow">終了行番号</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SetCustomerList(ByVal inRegisterSortType As String, _
                                ByVal inCustomerSortType As String, _
                                ByVal inSASortType As String, _
                                ByVal inSCSortType As String, _
                                ByVal inStartRow As Long, _
                                ByVal inEndRow As Long)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '写真情報取得用
        Dim daSystemEnvSetting As New SystemEnvSetting

        '顧客情報と予約情報を取得
        Using biz As New SC3080103BusinessLogic
            Try
                '現在日時取得
                Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
                Dim dtCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoRow() = _
                    biz.GetCustomerList(staffInfo.DlrCD, _
                                        staffInfo.BrnCD, _
                                        staffInfo.Account, _
                                        Me.HiddenSearchType.Value, _
                                        Me.HiddenSearchValue.Value, _
                                        nowDate, _
                                        inStartRow, _
                                        inEndRow, _
                                        inRegisterSortType, _
                                        inCustomerSortType, _
                                        inSASortType, _
                                        inSCSortType)

                Dim resultFlg As Boolean = False
                Dim customerInfoCount As Long = 0

                If IsNothing(dtCustomerInfo) OrElse IsNothing(dtCustomerInfo(0)) Then

                    Me.ShowMessageBox(WordId.id026)

                    resultFlg = False
                Else
                    If 0 < dtCustomerInfo.Count Then
                        resultFlg = True
                    End If
                End If

                '件数がある場合のみ情報取得し表示処理を行う
                If resultFlg = True Then

                    Dim drOne As SC3080103DataSet.SC3080103CustomerInfoRow = dtCustomerInfo(0)
                    customerInfoCount = drOne.ALLCOUNT

                    '写真URL取得
                    Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
                        daSystemEnvSetting.GetSystemEnvSetting(IMAGE_FILE_PATH)

                    '画面情報設定
                    Me.SetCustomerListData(dtCustomerInfo, drSystemEnvSetting)

                    '現在のソートを保持
                    Me.HiddenRegisterSortType.Value = inRegisterSortType
                    Me.HiddenCustomerSortType.Value = inCustomerSortType
                    Me.HiddenSASortType.Value = inSASortType
                    Me.HiddenSCSortType.Value = inSCSortType

                    '次の50件の表示設定
                    If 1 < inStartRow Then
                        Me.BackPage.Attributes("style") = ""
                    Else
                        Me.BackPage.Attributes("style") = "display:none;"
                    End If

                    '前の50件表示設定
                    If inEndRow < customerInfoCount Then
                        Me.NextPage.Attributes("style") = ""
                    Else
                        Me.NextPage.Attributes("style") = "display:none;"
                    End If

                    '読み込み中を非表示設定
                    Me.BackPageLoad.Attributes("style") = "display:none;"
                    Me.NextPageLoad.Attributes("style") = "display:none;"

                    '表示件数を保持
                    Me.HiddenStartIndex.Value = CType(inStartRow, String)
                    Me.HiddenEndIndex.Value = CType(inEndRow, String)

                    '件数保持
                    Me.HiddenSearchListCount.Value = CType(customerInfoCount, String)

                Else
                    '取得できなかった場合は文言を表示する
                    Me.CustomerSearchArea.Attributes("style") = "display:none;"
                    Me.NoSearchImage.Attributes("style") = "display:block;"

                End If

                '合計件数表示設定
                Me.SearchCount.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id002).Replace("{0}", CType(customerInfoCount, String))

                'エリア更新
                Me.ContentUpdateMainPanel.Update()

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            Finally
                '初期化
                daSystemEnvSetting = Nothing

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客一覧画面出力処理
    ''' </summary>
    ''' <param name="dtCustomerInfo">顧客情報</param>
    ''' <param name="drSystemEnvSetting">写真URL情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub SetCustomerListData(ByVal dtCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoRow(), _
                                    ByVal drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '顧客情報をバインドする
        CustomerReserveAreaRepeater.DataSource = Nothing
        Me.CustomerReserveAreaRepeater.DataSource = dtCustomerInfo
        Me.CustomerReserveAreaRepeater.DataBind()

        For i = 0 To Me.CustomerReserveAreaRepeater.Items.Count - 1
            '画面定義取得
            Dim customerReserveArea As Control = Me.CustomerReserveAreaRepeater.Items(i)

            'ROW取得
            Dim drCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoRow = _
                CType(dtCustomerInfo(i), SC3080103DataSet.SC3080103CustomerInfoRow)

            '/*****************
            ' 車両情報エリア
            ' *****************/
            '車両登録番号
            CType(customerReserveArea.FindControl("RegisterNo"), CustomLabel).Text = drCustomerInfo.REG_NUM

            'Province
            If Not (drCustomerInfo.IsREG_AREA_NAMENull) Then
                CType(customerReserveArea.FindControl("Province"), CustomLabel).Text = drCustomerInfo.REG_AREA_NAME
            End If

            '車名
            If Not (drCustomerInfo.IsMODEL_NAMENull) Then
                CType(customerReserveArea.FindControl("VehicleName"), CustomLabel).Text = drCustomerInfo.MODEL_NAME
            End If

            'VIN
            If Not (drCustomerInfo.IsVCL_VINNull) Then
                CType(customerReserveArea.FindControl("Vin"), CustomLabel).Text = drCustomerInfo.VCL_VIN
            End If

            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            'SSCアイコン
            If Not (drCustomerInfo.IsSSC_MARKNull) AndAlso _
               SSC_CUSTOMER.Equals(drCustomerInfo.SSC_MARK) Then
                'SSC対象フラグがONの場合
                CType(customerReserveArea.FindControl("SSCWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id027)
                CType(customerReserveArea.FindControl("SSCIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            End If
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            If ICON_FLAG_1.Equals(drCustomerInfo.PL_MARK) Then
                'P対象フラグがONの場合
                CType(customerReserveArea.FindControl("PWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10005)
                CType(customerReserveArea.FindControl("PIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            End If

            If ICON_FLAG_1.Equals(drCustomerInfo.TLM_MBR_FLG) Then
                'T対象フラグがONの場合
                CType(customerReserveArea.FindControl("TWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10004)
                CType(customerReserveArea.FindControl("TIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            End If
            If ICON_FLAG_1.Equals(drCustomerInfo.E_MARK) Then
                'E対象フラグがONの場合
                CType(customerReserveArea.FindControl("EWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10003)
                CType(customerReserveArea.FindControl("EIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            End If

            If ICON_FLAG_1.Equals(drCustomerInfo.MB_MARK) Then
                'M対象フラグがONの場合
                CType(customerReserveArea.FindControl("MWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10001)
                CType(customerReserveArea.FindControl("MIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            ElseIf ICON_FLAG_2.Equals(drCustomerInfo.MB_MARK) Then
                'B対象フラグがONの場合
                CType(customerReserveArea.FindControl("BWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10002)
                CType(customerReserveArea.FindControl("BIcon"),  _
                    HtmlContainerControl).Attributes("style") = "display:block;"
            End If
            '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            'Dim staffInfo As StaffContext = StaffContext.Current
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            '2017/02/20 NSK 竹中 TR-SVT-TMT-20161111-001 Welcome Board で顧客氏名が2行にわたって表示される START

            Dim attrList As ArrayList = New ArrayList()

            attrList.Add(drCustomerInfo.CST_ID.ToString(CultureInfo.CurrentCulture))
            attrList.Add(drCustomerInfo.VCL_ID.ToString(CultureInfo.CurrentCulture))
            attrList.Add(drCustomerInfo.VCL_VIN)
            attrList.Add(drCustomerInfo.DLR_CD)
            attrList.Add(drCustomerInfo.STR_CD)
            attrList.Add(drCustomerInfo.DMS_CST_CD)
            attrList.Add(drCustomerInfo.REG_NUM)
            attrList.Add(drCustomerInfo.MODEL_CD)
            attrList.Add(drCustomerInfo.CST_NAME)
            attrList.Add(drCustomerInfo.CST_PHONE)
            attrList.Add(drCustomerInfo.CST_MOBILE)
            attrList.Add(drCustomerInfo.SACODE)
            attrList.Add(drCustomerInfo.MODEL_NAME)
            attrList.Add(drCustomerInfo.CST_EMAIL_1)

            Dim serializer As New JavaScriptSerializer

            CType(customerReserveArea.FindControl("vehicleRecord"), HtmlControl).Attributes("name") = serializer.Serialize(attrList)

            ''車両情報エリアタップ時用のデータ格納
            'CType(customerReserveArea.FindControl("vehicleRecord"), HtmlControl).Attributes("name") = _
            '                            drCustomerInfo.CST_ID.ToString(CultureInfo.CurrentCulture) + _
            '                      "," + drCustomerInfo.VCL_ID.ToString(CultureInfo.CurrentCulture) + _
            '                      "," + drCustomerInfo.VCL_VIN + _
            '                      "," + drCustomerInfo.DLR_CD + _
            '                      "," + drCustomerInfo.STR_CD + _
            '                      "," + drCustomerInfo.DMS_CST_CD + _
            '                      "," + drCustomerInfo.REG_NUM + _
            '                      "," + drCustomerInfo.MODEL_CD + _
            '                      "," + drCustomerInfo.CST_NAME + _
            '                      "," + drCustomerInfo.CST_PHONE + _
            '                      "," + drCustomerInfo.CST_MOBILE + _
            '                      "," + drCustomerInfo.SACODE + _
            '                      "," + drCustomerInfo.MODEL_NAME + _
            '                      "," + drCustomerInfo.CST_EMAIL_1

            '2017/02/20 NSK 竹中 TR-SVT-TMT-20161111-001 Welcome Board で顧客氏名が2行にわたって表示される END

            '/*****************
            ' 顧客情報エリア
            ' *****************/
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            ''顧客アイコン
            'If Not (IsNothing(drSystemEnvSetting)) AndAlso Not (drCustomerInfo.IsIMG_FILENull) AndAlso Not (String.IsNullOrEmpty(drCustomerInfo.IMG_FILE)) Then
            '    '写真URL情報が取得できる場合
            '    Dim imageurl = String.Concat(drSystemEnvSetting.PARAMVALUE, drCustomerInfo.IMG_FILE)

            '    If System.IO.File.Exists(Server.MapPath(imageurl)) Then
            '        CType(customerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = imageurl
            '    Else
            '        '写真URL情報が取得できない場合
            '        CType(customerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = IMAGE_FILE_NO_PHOTO
            '    End If
            'Else
            '    '写真URL情報が取得できない場合
            '    CType(customerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = IMAGE_FILE_NO_PHOTO
            'End If

            ''顧客名＋敬称
            'If drCustomerInfo.IsNAMETITLE_NAMENull Then
            '    '敬称がない場合
            '    CType(customerReserveArea.FindControl("CustomerName"), CustomLabel).Text = drCustomerInfo.CST_NAME
            'Else
            '    '敬称がある場合
            '    If Not drCustomerInfo.IsPOSITION_TYPENull Then
            '        If drCustomerInfo.POSITION_TYPE = PositionTypeBack Then
            '            CType(customerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
            '                String.Concat(drCustomerInfo.CST_NAME, Space(1), drCustomerInfo.NAMETITLE_NAME)
            '        Else
            '            CType(customerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
            '                String.Concat(drCustomerInfo.NAMETITLE_NAME, Space(1), drCustomerInfo.CST_NAME)
            '        End If
            '    Else
            '        CType(customerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
            '            drCustomerInfo.CST_NAME
            '    End If
            'End If

            ''VIPアイコン
            'If Not (drCustomerInfo.IsVIPFLGNull) AndAlso _
            '   VIP_FLG.Equals(drCustomerInfo.VIPFLG) Then
            '    CType(customerReserveArea.FindControl("VipWord"), CustomLabel).Text = _
            '            WebWordUtility.GetWord(APPLICATION_ID, WordId.id010)
            '    CType(customerReserveArea.FindControl("VipIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            ''本販売店客アイコン
            'If Not (drCustomerInfo.IsCUSTOMER_FLAGNull) AndAlso _
            '   PURCHASE_DLR_CUSTOMER.Equals(drCustomerInfo.CUSTOMER_FLAG) Then
            '    '本販売店客存在する場合
            '    CType(customerReserveArea.FindControl("MyCompanyWord"), CustomLabel).Text = _
            '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id013)
            '    CType(customerReserveArea.FindControl("MyCompanyIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            ''他販売店客アイコン
            'If Not (drCustomerInfo.IsCUSTOMER_FLAGNull) AndAlso _
            '   OTHER_DLR_CUSTOMER.Equals(drCustomerInfo.CUSTOMER_FLAG) Then
            '    '他販売店客存在する場合
            '    CType(customerReserveArea.FindControl("MyCompanyWord"), CustomLabel).Text = _
            '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id014)
            '    CType(customerReserveArea.FindControl("MyCompanyIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            ''顧客タイプアイコン
            'If Not (drCustomerInfo.IsCST_TYPENull) Then
            '    If CUSTOMER_TYPE_PERSONAL.Equals(drCustomerInfo.CST_TYPE) Then
            '        '個人
            '        CType(customerReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
            '            WebWordUtility.GetWord(APPLICATION_ID, WordId.id011)
            '        CType(customerReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
            '    End If
            'End If

            ''法人アイコン
            'If Not (drCustomerInfo.IsFLEET_FLGNull) AndAlso _
            '   CORPORATION_TYPE_MINE.Equals(drCustomerInfo.FLEET_FLG) Then
            '    '法人フラグが存在する場合
            '    CType(customerReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
            '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id012)
            '    CType(customerReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            ''予約ありアイコン
            'If Not (drCustomerInfo.IsAPPOITMENT_FLGNull) AndAlso _
            '   APPOITMENT_FLG.Equals(drCustomerInfo.APPOITMENT_FLG) Then
            '    '予約ありフラグが存在する場合
            '    CType(customerReserveArea.FindControl("MyAppointmentWord"), CustomLabel).Text = _
            '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id015)
            '    CType(customerReserveArea.FindControl("MyAppointmentIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            ''顧客エリアタップ時用のデータ格納（顧客ID、車両ID）
            'CType(customerReserveArea.FindControl("customerRecord"), HtmlControl).Attributes("name") = _
            '    String.Concat(drCustomerInfo.DMS_CST_CD, "," & drCustomerInfo.VCL_ID.ToString(CultureInfo.CurrentCulture), "," & drCustomerInfo.VCL_VIN)

            '顧客エリアの設定
            Me.SetCustomerArea(drCustomerInfo, drSystemEnvSetting, customerReserveArea)

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            '/*****************
            ' 電話番号エリア
            ' *****************/
            '電話番号
            If Not (drCustomerInfo.IsCST_PHONENull) Then
                CType(customerReserveArea.FindControl("TelNo"), CustomLabel).Text = drCustomerInfo.CST_PHONE
            End If

            '携帯電話番号
            If Not (drCustomerInfo.IsCST_MOBILENull) Then
                CType(customerReserveArea.FindControl("MobileNo"), CustomLabel).Text = drCustomerInfo.CST_MOBILE
            End If

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            ''電話番号エリアタップ時用のデータ格納（顧客ID、車両ID）
            'CType(customerReserveArea.FindControl("telRecord"), HtmlControl).Attributes("name") = _
            '    String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            '電話番号エリアタップ時用のデータ格納（顧客ID、車両ID）
            CType(customerReserveArea.FindControl("telRecord"), HtmlControl).Attributes("name") = _
                String.Concat(drCustomerInfo.DMS_CST_CD, "," & drCustomerInfo.VCL_ID.ToString(CultureInfo.CurrentCulture), "," & drCustomerInfo.VCL_VIN)

            '/*****************
            ' SAエリア
            ' *****************/
            'SA
            If Not (drCustomerInfo.IsSANull) Then
                CType(customerReserveArea.FindControl("SA_NAME"), CustomLabel).Text = drCustomerInfo.SA
            End If

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            ''SCエリアタップ時用のデータ格納（顧客ID、車両ID）
            'CType(customerReserveArea.FindControl("SCRecord"), HtmlControl).Attributes("name") = _
            '    String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            '/*****************
            ' SCエリア
            ' *****************/
            'SC
            If Not (drCustomerInfo.IsSCNull) Then
                CType(customerReserveArea.FindControl("SC_NAME"), CustomLabel).Text = drCustomerInfo.SC
            End If

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            ''SCエリアタップ時用のデータ格納（顧客ID、車両ID）
            'CType(customerReserveArea.FindControl("SCRecord"), HtmlControl).Attributes("name") = _
            '    String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

    ''' <summary>
    ''' 顧客エリア設定処理
    ''' </summary>
    ''' <param name="drCustomerInfo">顧客情報</param>
    ''' <param name="drSystemEnvSetting">システム設定情報</param>
    ''' <param name="inCustomerReserveArea">顧客エリア情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SetCustomerArea(ByVal drCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoRow, _
                                ByVal drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow, _
                                ByVal inCustomerReserveArea As Control)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '顧客アイコン
        If Not (IsNothing(drSystemEnvSetting)) AndAlso Not (drCustomerInfo.IsIMG_FILENull) AndAlso Not (String.IsNullOrWhiteSpace(drCustomerInfo.IMG_FILE)) Then
            '写真URL情報が取得できる場合
            Dim imageurl = String.Concat(drSystemEnvSetting.PARAMVALUE, drCustomerInfo.IMG_FILE)

            If System.IO.File.Exists(Server.MapPath(imageurl)) Then
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                'CType(inCustomerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = imageurl
                CType(inCustomerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = _
                    String.Concat(imageurl, "?", Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
            Else
                '写真URL情報が取得できない場合
                CType(inCustomerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = IMAGE_FILE_NO_PHOTO
            End If
        Else
            '写真URL情報が取得できない場合
            CType(inCustomerReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = IMAGE_FILE_NO_PHOTO
        End If

        '顧客名＋敬称
        If drCustomerInfo.IsNAMETITLE_NAMENull Then
            '敬称がない場合
            CType(inCustomerReserveArea.FindControl("CustomerName"), CustomLabel).Text = drCustomerInfo.CST_NAME
        Else
            '敬称がある場合
            If Not drCustomerInfo.IsPOSITION_TYPENull Then
                If PositionTypeBack.Equals(drCustomerInfo.POSITION_TYPE) Then
                    CType(inCustomerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                        String.Concat(drCustomerInfo.CST_NAME, Space(1), drCustomerInfo.NAMETITLE_NAME)
                Else
                    CType(inCustomerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                        String.Concat(drCustomerInfo.NAMETITLE_NAME, Space(1), drCustomerInfo.CST_NAME)
                End If
            Else
                CType(inCustomerReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                    drCustomerInfo.CST_NAME
            End If
        End If

        'VIPアイコン
        If Not (drCustomerInfo.IsVIPFLGNull) AndAlso _
           VIP_FLG.Equals(drCustomerInfo.VIPFLG) Then
            CType(inCustomerReserveArea.FindControl("VipWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id010)
            CType(inCustomerReserveArea.FindControl("VipIcon"), HtmlContainerControl).Attributes("style") = ""
        End If

        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        If ICON_FLAG_2.Equals(drCustomerInfo.PL_MARK) Then
            'L対象フラグがONの場合
            CType(inCustomerReserveArea.FindControl("LWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10006)
            CType(inCustomerReserveArea.FindControl("LIcon"), HtmlContainerControl).Attributes("style") = ""
        End If
        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        '本販売店客アイコン
        If Not (drCustomerInfo.IsCUSTOMER_FLAGNull) AndAlso _
           PURCHASE_DLR_CUSTOMER.Equals(drCustomerInfo.CUSTOMER_FLAG) Then
            '本販売店客存在する場合
            CType(inCustomerReserveArea.FindControl("MyCompanyWord"), CustomLabel).Text = _
                WebWordUtility.GetWord(APPLICATION_ID, WordId.id013)
            CType(inCustomerReserveArea.FindControl("MyCompanyIcon"), HtmlContainerControl).Attributes("style") = ""
        End If

        '他販売店客アイコン
        If Not (drCustomerInfo.IsCUSTOMER_FLAGNull) AndAlso _
           OTHER_DLR_CUSTOMER.Equals(drCustomerInfo.CUSTOMER_FLAG) Then
            '他販売店客存在する場合
            CType(inCustomerReserveArea.FindControl("MyCompanyWord"), CustomLabel).Text = _
                WebWordUtility.GetWord(APPLICATION_ID, WordId.id014)
            CType(inCustomerReserveArea.FindControl("MyCompanyIcon"), HtmlContainerControl).Attributes("style") = ""
        End If

        '顧客タイプアイコン
        If Not (drCustomerInfo.IsCST_TYPENull) AndAlso _
            CUSTOMER_TYPE_PERSONAL.Equals(drCustomerInfo.CST_TYPE) Then
            '個人
            CType(inCustomerReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
                WebWordUtility.GetWord(APPLICATION_ID, WordId.id011)
            CType(inCustomerReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""

        End If

        '法人アイコン
        If Not (drCustomerInfo.IsFLEET_FLGNull) AndAlso _
           CORPORATION_TYPE_MINE.Equals(drCustomerInfo.FLEET_FLG) Then
            '法人フラグが存在する場合
            CType(inCustomerReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
                WebWordUtility.GetWord(APPLICATION_ID, WordId.id012)
            CType(inCustomerReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
        End If

        '予約ありアイコン
        If Not (drCustomerInfo.IsAPPOITMENT_FLGNull) AndAlso _
           APPOITMENT_FLG.Equals(drCustomerInfo.APPOITMENT_FLG) Then
            '予約ありフラグが存在する場合
            CType(inCustomerReserveArea.FindControl("MyAppointmentWord"), CustomLabel).Text = _
                WebWordUtility.GetWord(APPLICATION_ID, WordId.id015)
            CType(inCustomerReserveArea.FindControl("MyAppointmentIcon"), HtmlContainerControl).Attributes("style") = ""
        End If

        '顧客エリアタップ時用のデータ格納（顧客ID、車両ID）
        CType(inCustomerReserveArea.FindControl("customerRecord"), HtmlControl).Attributes("name") = _
            String.Concat(drCustomerInfo.DMS_CST_CD, "," & drCustomerInfo.VCL_ID.ToString(CultureInfo.CurrentCulture), "," & drCustomerInfo.VCL_VIN)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

#End Region

#Region "RO一覧表示処理"

    ''' <summary>
    ''' RO一覧画面出力処理
    ''' </summary>
    ''' <param name="dtReserveInfo">RO情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub SetOrderListData(ByVal dtReserveInfo As SC3080103DataSet.SC3080103ReserveInfoDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim drSearchReserveInfo As SC3080103DataSet.SC3080103ReserveInfoRow() = _
            (From drReserveInfo In dtReserveInfo _
             Order By drReserveInfo.ROSTATUS Ascending
             Select drReserveInfo).ToArray

        'RO情報をバインドする
        Me.OrderListRepeater.DataSource = drSearchReserveInfo
        Me.OrderListRepeater.DataBind()

        For i = 0 To Me.OrderListRepeater.Items.Count - 1
            '画面定義取得
            Dim orderListRepeater As Control = Me.OrderListRepeater.Items(i)

            'ROW取得
            Dim drOrderInfo As SC3080103DataSet.SC3080103ReserveInfoRow = drSearchReserveInfo(i)

            '開始日時 - 終了日時
            'レコード追加ステータスが「0：追加していない行」の場合、
            'ストール利用IDが「05：中断、07：未来店客」でない場合は表示する
            If String.Equals(DateTimeFunc.FormatDate(11, drOrderInfo.START_DATETIME), _
                             DateTimeFunc.FormatDate(11, drOrderInfo.END_DATETIME)) Then
                '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
                DateTimeFunc.FormatDate(11, drOrderInfo.START_DATETIME)
                DateTimeFunc.FormatDate(14, drOrderInfo.END_DATETIME)
                CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                    String.Concat(DateTimeFunc.FormatDate(11, drOrderInfo.START_DATETIME), _
                                  Space(1), _
                                  DateTimeFunc.FormatDate(14, drOrderInfo.START_DATETIME), _
                                  Space(1), _
                                  WebWordUtility.GetWord(APPLICATION_ID, WordId.id020), _
                                  Space(1), _
                                  DateTimeFunc.FormatDate(14, drOrderInfo.END_DATETIME))

            Else
                '日跨ぎ出ない場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
                Dim reserveFromStart As String
                Dim reserveFromEnd As String
                Dim fromMD As String
                Dim fromHM As String

                fromMD = DateTimeFunc.FormatDate(11, drOrderInfo.START_DATETIME)      'MM/dd
                fromHM = DateTimeFunc.FormatDate(14, drOrderInfo.START_DATETIME)      'hh:mm
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'reserveFromStart = fromMD & " " & fromHM                              'MM/dd hh:mm
                reserveFromStart = fromMD & Space(1) & fromHM                         'MM/dd hh:mm
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                fromMD = DateTimeFunc.FormatDate(11, drOrderInfo.END_DATETIME)      'MM/dd
                fromHM = DateTimeFunc.FormatDate(14, drOrderInfo.END_DATETIME)      'hh:mm
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'reserveFromEnd = fromMD & " " & fromHM                              'MM/dd hh:mm
                reserveFromEnd = fromMD & Space(1) & fromHM                         'MM/dd hh:mm
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                '    String.Concat(reserveFromStart, _
                '                  Space(1), _
                '                  WebWordUtility.GetWord(APPLICATION_ID, WordId.id020), _
                '                  Space(1), _
                '                  reserveFromEnd)

                '開始終了日時を文字列結合
                Dim strStartEndDate As String = String.Concat(reserveFromStart, _
                                  Space(1), _
                                  WebWordUtility.GetWord(APPLICATION_ID, WordId.id020), _
                                  Space(1), _
                                  reserveFromEnd)

                CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = strStartEndDate
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END


            End If

            '整備名称
            If Not (drOrderInfo.IsSERVICE_NAMENull) Then
                CType(orderListRepeater.FindControl("OrderServiceName"), CustomLabel).Text = drOrderInfo.SERVICE_NAME
            End If

            'RO状態
            If Not (drOrderInfo.IsROSTATUSNull) Then
                CType(orderListRepeater.FindControl("ROIssuingDisp"), CustomLabel).Text = drOrderInfo.ROSTATUS
            End If

            'RO_NUM
            If Not (drOrderInfo.IsRO_NUMNull) Then
                CType(orderListRepeater.FindControl("OrderNumber"), CustomLabel).Text = drOrderInfo.RO_NUM
            End If

            'RO作業番号
            If Not (drOrderInfo.IsRO_JOB_SEQNull) Then
                CType(orderListRepeater.FindControl("RoJobSeq"), CustomLabel).Text = drOrderInfo.RO_JOB_SEQ
            End If

            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
            '顧客氏名
            If Not (drOrderInfo.IsCST_NAMENull) Then
                CType(orderListRepeater.FindControl("NewCustomerName"), CustomLabel).Text = drOrderInfo.CST_NAME
            End If
            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            ''RO_NUM、RO作業番号、入庫ID、DMS予約ID
            'Dim roNum As String = String.Empty
            'If Not drOrderInfo.IsRO_NUMNull AndAlso _
            '   Not IsNothing(drOrderInfo.RO_NUM) Then
            '    roNum = drOrderInfo.RO_NUM
            'End If
            'Dim roJobSeq As String = String.Empty
            'If Not drOrderInfo.IsRO_JOB_SEQNull AndAlso _
            '   Not IsNothing(drOrderInfo.RO_JOB_SEQ) Then
            '    roJobSeq = drOrderInfo.RO_JOB_SEQ
            'End If
            'Dim svcIN_ID As String = String.Empty
            'If Not drOrderInfo.IsSVCIN_IDNull AndAlso _
            '   Not IsNothing(drOrderInfo.SVCIN_ID) Then
            '    svcIN_ID = drOrderInfo.SVCIN_ID.ToString()
            'End If
            'Dim dmsJobDtlId As String = String.Empty
            'If Not drOrderInfo.IsDMS_JOB_DTL_IDNull AndAlso _
            '   Not IsNothing(drOrderInfo.DMS_JOB_DTL_ID) Then
            '    dmsJobDtlId = drOrderInfo.DMS_JOB_DTL_ID.ToString()
            'End If

            'RO_NUM
            Dim roNum As String = String.Empty
            If Not drOrderInfo.IsRO_NUMNull AndAlso _
               Not String.IsNullOrEmpty(drOrderInfo.RO_NUM) Then
                roNum = drOrderInfo.RO_NUM
            End If

            'RO作業番号
            Dim roJobSeq As String = String.Empty
            If Not drOrderInfo.IsRO_JOB_SEQNull AndAlso _
               Not String.IsNullOrEmpty(drOrderInfo.RO_JOB_SEQ) Then
                roJobSeq = drOrderInfo.RO_JOB_SEQ
            End If

            '入庫ID
            Dim svcIN_ID As String = String.Empty
            If Not drOrderInfo.IsSVCIN_IDNull AndAlso _
               0 < drOrderInfo.SVCIN_ID Then
                svcIN_ID = drOrderInfo.SVCIN_ID.ToString(CultureInfo.CurrentCulture)
            End If

            'DMS予約ID
            Dim dmsJobDtlId As String = String.Empty
            If Not drOrderInfo.IsDMS_JOB_DTL_IDNull AndAlso _
               Not String.IsNullOrEmpty(drOrderInfo.DMS_JOB_DTL_ID.Trim) Then
                dmsJobDtlId = drOrderInfo.DMS_JOB_DTL_ID.ToString()
            End If

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            CType(orderListRepeater.FindControl("OrderListItem"), HtmlControl).Attributes("name") = _
                String.Concat(roNum, "," & roJobSeq, "," & svcIN_ID, "," & dmsJobDtlId)

        Next

        'ROポップアップ一覧エリア更新
        Me.ContentUpdatePopuupPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "RO作成画面へ遷移処理"

    ''' <summary>
    ''' RO作成画面へ遷移処理
    ''' </summary>
    ''' <param name="InSvcINId">入庫ID</param>
    ''' <param name="InRoNum">RO情報</param>
    ''' <param name="InRoJobSeq">RO作業番号</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Function RedirectROCreate(Optional ByVal InSvcINId As String = "", _
                                      Optional ByVal InRoNum As String = "", _
                                      Optional ByVal InRoJobSeq As String = "", _
                                      Optional ByVal InDmsJobDtlId As String = "", _
                                      Optional ByVal InIsFromPopUpRedirect As Boolean = False) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dtCondition As New IC3810203InCustomerSaveDataTable
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            'Dim dtCondition As New IC3810203InCustomerSaveDataTable
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            Dim rowCondition As IC3810203InCustomerSaveRow = dtCondition.NewIC3810203InCustomerSaveRow
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

            If Not String.IsNullOrEmpty(InSvcINId) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'Try
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                'サービス入庫ID
                rowCondition.SVCIN_ID = CType(InSvcINId, Decimal)
                rowCondition.REZID = CType(InSvcINId, Decimal)

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'Catch ex As Exception
                '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                    , "{0}.{1} END ExceptionMessage:{2}" _
                '                    , Me.GetType.ToString _
                '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                    , ex.Message))
                '        Me.ShowMessageBox(WordId.id904)
                '    End Try
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            End If

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            'Dim DMSCstID As String = CType(Me.HiddenSelectDMSCSTID.Value, String)
            Dim DMSCstID As String = Me.HiddenSelectDMSCSTID.Value
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            Using commonClass As New SMBCommonClassBusinessLogic
                DMSCstID = commonClass.ReplaceBaseCustomerCode(staffInfo.DlrCD, DMSCstID)
            End Using

            '来店実績番号（固定「0」）
            rowCondition.VISITSEQ = VISITSEQ_DEFAULT_VALUE
            '機能ID（画面ID）
            rowCondition.SYSTEM = APPLICATION_ID
            'アカウント（ログインユーザアカウント）
            rowCondition.ACCOUNT = staffInfo.Account
            '事前準備チップフラグ（固定「0」）
            rowCondition.PREPARECHIPFLAG = PREPEARENCE_CHIP_FLG

            '販売店コード
            rowCondition.DLRCD = staffInfo.DlrCD
            '店舗コード
            rowCondition.STRCD = staffInfo.BrnCD
            '顧客コード
            rowCondition.CUSTOMERCODE = Me.HiddenSelectDMSCSTID.Value
            '基幹顧客ID
            rowCondition.DMSID = DMSCstID
            '車両登録No
            rowCondition.VCLREGNO = Me.HiddenSelectVehRegNo.Value
            'VIN
            rowCondition.VIN = Me.HiddenSelectVIN.Value
            'モデルコード
            rowCondition.MODELCODE = Me.HiddenSelectModelCode.Value
            '顧客名
            rowCondition.CUSTOMERNAME = Me.HiddenSelectCustomerName.Value
            '電話番号
            rowCondition.TELNO = Me.HiddenSelectTelNumber.Value
            '携帯番号
            rowCondition.MOBILE = Me.HiddenSelectMobileNumber.Value
            '振当SA
            rowCondition.SACODE = staffInfo.Account
            '車名
            rowCondition.VEHICLENAME = Me.HiddenSelectModelName.Value
            'E-MAILアドレス1
            rowCondition.EMAIL1 = Me.HiddenSelectEMail.Value

            Dim retIC3810203ReservationInfoRow As IC3810203ReservationInfoRow = Nothing

            Using biz As New SC3080103BusinessLogic
                retIC3810203ReservationInfoRow = biz.VisitRegistProccess(rowCondition, nowDate)
                If IsNothing(retIC3810203ReservationInfoRow) Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:ReturnCode = Nothing" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return Result_Fail
                End If

                If InIsFromPopUpRedirect Then
                    'WelcomeBoardリフレッシュPush送信
                    biz.SendPushForRefreshWelcomeBoard(staffInfo)
                End If

            End Using

            'R/O作成画面へ遷移
            Me.RedirectOrderCreatePage(retIC3810203ReservationInfoRow, InRoNum, InRoJobSeq, InDmsJobDtlId)

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
        End Using
        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return Result_Success

    End Function

#End Region

#Region "画面遷移処理"

    ''' <summary>
    ''' 顧客詳細画面遷移
    ''' </summary>
    ''' <param name="inCstID">顧客ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub RedirectCustomerDetailPage(ByVal inCstID As String, _
                                           ByVal inVin As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '次画面遷移パラメータ設定

        'DMS顧客ID
        Dim DmsID As String = String.Empty
        If Not (String.IsNullOrEmpty(inCstID)) Then DmsID = inCstID
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DMS_CST_ID, DmsID)

        'VIN
        Dim vin As String = String.Empty
        If Not (String.IsNullOrEmpty(inVin)) Then vin = inVin
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VINNO, vin)

        '顧客詳細画面に遷移
        Me.RedirectNextScreen(CUSTOMER_DETAIL_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/O作成画面遷移
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub RedirectOrderCreatePage(Optional ByVal rowIn As IC3810203ReservationInfoRow = Nothing, _
                                        Optional ByVal RoNum As String = "", _
                                        Optional ByVal RoJobSeq As String = "", _
                                        Optional ByVal DmsJobDtlId As String = "")
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If StaffContext.Current.OpeCD = Operation.SA Then

            'ログインスタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            Using biz As New SC3080103BusinessLogic

                '基幹コードへ変換処理
                Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

                '基幹販売店コードチェック
                If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    'If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordId.id006)

                    '処理終了
                    Exit Sub

                End If

                '基幹店舗コードチェック
                If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    'If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordId.id006)

                    '処理終了
                    Exit Sub

                End If

                '基幹アカウントチェック
                If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    'If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordId.id006)

                    '処理終了
                    Exit Sub

                End If

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

                '来店情報取得
                Dim dtVisitiManagmentInfo As SC3080103DataSet.SC3080103VisitManagmentInfoDataTable = _
                    biz.GetVisitManagmentInfo(rowIn.VISITSEQ)

                '来店情報チェック
                If dtVisitiManagmentInfo.Count = 0 Then
                    '0件の場合

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:GetVisitManagmentInfo IS NODATA" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordId.id006)

                    '処理終了
                    Exit Sub

                End If

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                'セション値の設定
                'DMS用販売店コード
                Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

                'DMS用店舗コード
                Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

                'ログインユーザアカウント
                Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

                If IsNothing(rowIn) Then
                    '来店実績連番 
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")
                Else
                    '来店実績連番 
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, rowIn.VISITSEQ)
                End If

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, DmsJobDtlId)

                'RO番号
                Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, RoNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, RoJobSeq)

                '車両登録NOのVIN
                Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, CType(Me.HiddenSelectVIN.Value, String))

                '「0：編集」固定
                Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

                'ViewMode顧客のDMSID
                Me.SetValue(ScreenPos.Next, SESSIONKEY_CUSTOMER_ID, CType(Me.HiddenSelectDMSCSTID.Value, String))

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

                'コンタクトパーソン
                '値チェック
                If Not (dtVisitiManagmentInfo(0).IsVISITNAMENull) Then
                    '値有り
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_PARSON, HttpUtility.UrlEncode(dtVisitiManagmentInfo(0).VISITNAME))

                Else
                    '値無し
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_PARSON, String.Empty)

                End If

                'コンタクト電話番号
                '値チェック
                If Not (dtVisitiManagmentInfo(0).IsVISITTELNONull) Then
                    '値有り
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_TELNO, HttpUtility.UrlEncode(dtVisitiManagmentInfo(0).VISITTELNO))

                Else
                    '値無し
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_TELNO, String.Empty)

                End If

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                'RO作成フラグ
                Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISP_NUM_ROCREATE)

            End Using

            'R/O作成画面に遷移
            Me.RedirectNextScreen(LOCAL_TACT_PAGE)

        End If


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "フッターボタン押す処理"

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
    ' ''' <summary>
    ' ''' 商品訴求コンテンツボタンタップイベント初期化
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub InitGoodsSolicitationContentsButtonEvent()

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} START" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'SA、SMの場合、商品訴求ボタンのイベントを登録する
    '    If StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SA _
    '        OrElse StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SM Then

    '        '商品訴求ボタン
    '        Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
    '        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
    '        'イベントをbindする
    '        AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
    '        footerGoodsSolicitationContentsButton.OnClientClick = "return FooterButtonControl();"

    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} END" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    'End Sub

    ' ''' <summary>
    ' ''' キャンペーンボタンタップイベント初期化
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub InitCampaignButtonEvent()

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} START" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'SA、SMの場合、キャンペーンボタンタップすると、現地のキャンペーン画面に遷移
    '    If StaffContext.Current.OpeCD = Operation.SA _
    '        OrElse StaffContext.Current.OpeCD = Operation.SM Then

    '        'キャンペーンボタン
    '        Dim footerCampaignButton As CommonMasterFooterButton = _
    '        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)

    '        'メインボタンのClientとサーバ両方のクリックイベントをbind
    '        AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
    '        footerCampaignButton.OnClientClick = "return FooterButtonControl();"

    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} END" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    'End Sub

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerGoodsSolicitationContentsMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_DEALERCODE, ONE_SPACE)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BRANCHCODE, ONE_SPACE)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_LOGINUSERID, ONE_SPACE)

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SACHIPID, "")

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BASREZID, "")

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_RO, "")

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SEQ_NO, "")

        '車両登録NOのVIN
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIN_NO, "")

        'RO作成フラグ
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIEWMODE, SESSIONVALUE_READ)

        '商品訴求コンテンツ画面に遷移
        Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub footerCampaignMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080103BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordId.id006)

                '処理終了
                Exit Sub

            End If


            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
            'Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_READ)
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_CAMPAIGN)

        End Using

        '決定した遷移先に遷移
        Me.RedirectNextScreen(LOCAL_TACT_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "DMS販売店コード、店舗コードの取得する"

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

    ' ''' <summary>
    ' ''' 基幹販売店、基幹店舗コードを取得する
    ' ''' </summary>
    ' ''' <param name="dealerCode">i-CROP販売店コード</param>
    ' ''' <param name="branchCode">i-CROP店舗コード</param>
    ' ''' <returns>中断情報テーブル</returns>
    ' ''' <remarks></remarks>
    'Private Function GetDmsBlnCd(ByVal dealerCode As String, _
    '                             ByVal branchCode As String) As ServiceCommonClassDataSet.DmsCodeMapRow

    '    Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

    '    Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
    '        '基幹販売店コード、店舗コードを取得
    '        dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dealerCode, _
    '                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
    '                                                            dealerCode, _
    '                                                            branchCode, _
    '                                                            String.Empty)
    '        If dmsDlrBrnTable.Count <= 0 Then
    '            'データが取得できない場合はエラー
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
    '                                       "{0}.Error ErrCode: Failed to convert key dealer code.(No data found)", _
    '                                       MethodBase.GetCurrentMethod.Name))
    '            Return Nothing
    '        ElseIf 1 < dmsDlrBrnTable.Count Then
    '            'データが2件以上取得できた場合は一意に決定できないためエラー
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
    '                                       "{0}.Error ErrCode:Failed to convert key dealer code.(Non-unique)", _
    '                                       MethodBase.GetCurrentMethod.Name))
    '            Return Nothing
    '        End If
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.E ", _
    '                              MethodBase.GetCurrentMethod.Name))

    '    Return dmsDlrBrnTable.Item(0)

    'End Function

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

#End Region

End Class


