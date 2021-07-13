'------------------------------------------------------------------------------
'SC3240401.aspx.vb
'------------------------------------------------------------------------------
'機能：チップ検索
'補足：
'作成： 2013/02/28 TMEJ 小澤 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新： 2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
'更新： 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SMB.ChipSearch.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SMB.ChipSearch.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

'2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports System.Web.Script.Serialization
'2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

Public Class SC3240401
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3240401"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

    ' ''' <summary>
    ' ''' フッターコード：メインメニュー
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_MAINMENU As Integer = 100
    ' ''' <summary>
    ' ''' フッターコード：顧客詳細
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_CUSTOMER As Integer = 200
    ' ''' <summary>
    ' ''' フッターコード：R/Oボタン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_RO As Integer = 600
    ' ''' <summary>
    ' ''' フッターコード：追加作業ボタン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_ADD_LIST As Integer = 1100
    ' ''' <summary>
    ' ''' フッターコード：完成検査ボタン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_SERVER_CHECK_LIST As Integer = 1000
    ' ''' <summary>
    ' ''' フッターコード：スケジューラ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_SCHEDULE As Integer = 400
    ' ''' <summary>
    ' ''' フッターコード：電話帳
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_TEL_DIRECTORY As Integer = 500
    ' ''' <summary>
    ' ''' フッターコード：SMB
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_SMB As Integer = 800
    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = FooterMenuCategory.MainMenu
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CUSTOMER As Integer = FooterMenuCategory.CustomerDetail
    ''' <summary>
    ''' フッターコード：R/O一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RO As Integer = FooterMenuCategory.RepairOrderList
    ''' <summary>
    ''' フッターコード：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CAMPAIGN As Integer = FooterMenuCategory.Campaign
    ''' <summary>
    ''' フッターコード：予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RESERVE As Integer = FooterMenuCategory.ReserveManagement
    ''' <summary>
    ''' フッターコード：追加作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADD_LIST As Integer = FooterMenuCategory.AddWorkList
    ''' <summary>
    ''' フッターコード：電話帳
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = FooterMenuCategory.Contact
    ''' <summary>
    ''' フッターコード：FM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_FM As Integer = FooterMenuCategory.ForemanMain
    ''' <summary>
    ''' フッターコード：TC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TC As Integer = FooterMenuCategory.TechnicianMain
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = FooterMenuCategory.SMB
    ''' <summary>
    ''' フッターコード：商品訴求
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_GOOD As Integer = FooterMenuCategory.GoodsSolicitationContents
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_SA As String = "SC3140103"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' メインメニュー(SM)画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const MAINMENU_ID_SM As String = "SC3220101"
    ''' <summary>
    ''' メインメニュー(SM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_SM As String = "SC3220201"
    ' ''' <summary>
    ' ''' メインメニュー(CT)画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const MAINMENU_ID_CT As String = "SC3200101"
    ''' <summary>
    ''' メインメニュー(CT)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_CT As String = "SC3240101"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
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
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 顧客詳細画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CUSTOMER_DETAIL_PAGE As String = "SC3080208"
    ' ''' <summary>
    ' ''' R/O一覧画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const REPAIR_ORDERE_LIST_PAGE As String = "SC3160101"
    ''' <summary>
    ''' 顧客詳細画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_DETAIL_PAGE As String = "SC3080225"
    ''' <summary>
    ''' 現地にシステム連携用画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_LINKAGE_PAGE As String = "SC3010501"
    ''' <summary>
    ''' プログラムID：来店管理画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISIT_MANAGER_PAGE As String = "SC3100303"

    ' ''' <summary>
    ' ''' 追加作業一覧画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ADDITION_WORK_LIST_PAGE As String = "SC3170101"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 完成検査一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERVER_CHECK_LIST_PAGE As String = "SC3180101"
    ''' <summary>
    ''' R/O参照画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_DETAIL_PAGE As String = "SC3160208"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"

    ' ''' <summary>
    ' ''' スケジューラボタンのイベント
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_EVENT_SCHEDULER As String = "return schedule.appExecute.executeCaleNew();"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

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
    ''' 顧客区分（1：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_MY_COMPANY As String = "1"

    '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
    ''' <summary>
    ''' 個人法人フラグ（1：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CORPORATION_TYPE_MINE As String = "1"

    ''' <summary>
    ''' 個人法人フラグ（2：法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CORPORATION_TYPE_COMPANY As String = "2"
    '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END

    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT As String = "SC3240401_DEFAULT_READ_COUNT"
    ''' <summary>
    ''' 検索最大表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_DISPLAY_COUNT As String = "SC3240401_MAX_DISPLAY_COUNT"

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

    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' Sessionキー（仮置きフラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TEMP_FLG As String = "Session.TEMP_FLG"

    ''' <summary>
    ''' Sessionキー（RO番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_RO_NUM As String = "Session.RO_NUM"
    ''' <summary>
    ''' Sessionキー（RO番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_RO_SEQ As String = "Session.RO_SEQ"
    ''' <summary>
    ''' Sessionキー（ROステータス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_RO_STATUS As String = "Session.RO_STATUS"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' SessionKey(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TOTMEJ_DMSCSTID As String = "SessionKey.DMS_CST_ID"
    ''' <summary>
    ''' SessionKey(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TOTMEJ_VIN As String = "SessionKey.VIN"
    ' ''' <summary>
    ' ''' Sessionキー（販売店コード）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_CRDEALERCODE As String = "Redirect.CRDEALERCODE"
    ' ''' <summary>
    ' ''' Sessionキー（来店管理連番）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_VISITSEQ As String = "Redirect.VISITSEQ"
    ' ''' <summary>
    ' ''' Sessionキー（予約ID）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_REZID As String = "Redirect.REZID"
    ' ''' <summary>
    ' ''' Sessionキー（顧客名）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_NAME As String = "Redirect.NAME"
    ' ''' <summary>
    ' ''' Sessionキー（車両登録No）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_REGISTERNO As String = "Redirect.REGISTERNO"
    ' ''' <summary>
    ' ''' Sessionキー（VIN）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_VINNO As String = "Redirect.VINNO"
    ' ''' <summary>
    ' ''' Sessionキー（モデルコード）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_MODELCODE As String = "Redirect.MODELCODE"
    ' ''' <summary>
    ' ''' Sessionキー（電話番号）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_TEL1 As String = "Redirect.TEL1"
    ' ''' <summary>
    ' ''' Sessionキー（携帯番号）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_TEL2 As String = "Redirect.TEL2"
    ' ''' <summary>
    ' ''' Sessionキー（担当SAコード）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_SACODE As String = "Redirect.SACODE"
    ' ''' <summary>
    ' ''' Sessionキー（事前準備フラグ）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_PREPARECHIPFLAG As String = "Redirect.PREPARECHIPFLAG"
    ' ''' <summary>
    ' ''' Sessionキー（受付フラグ）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_RECEPTIONFLAG As String = "Redirect.RECEPTIONFLAG"
    ' ''' <summary>
    ' ''' Sessionキー（固定フラグ）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_FLAG As String = "Redirect.FLAG"


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
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIEWMODE As String = "ViewMode"

    ' ''' <summary>
    ' ''' Sessionキー（整備受注No）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_ORDERNO As String = "OrderNo"

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
    ''' SessionKey(入庫管理番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SVCIN_NUM As String = "Session.Param11"
    ''' <summary>
    ''' SessionKey(入庫販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SVCIN_DLRCD As String = "Session.Param12"
    ''' <summary>
    ''' SessionKey(DISP_NUM)：画面番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"

    ''' <summary>
    ''' SessionValue(Format)：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_FORMAT_PREVIEW As String = "0"

    ''' <summary>
    ''' SessionValue(ViewMode)：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_VIEWMODE_EDIT As String = "0"

    ''' <summary>
    ''' SessionValue(ViewMode)：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_VIEWMODE_PREVIEW As String = "1"

    ''' <summary>
    ''' SessionValue(画面番号)：RO参照
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ROPREVIEW As String = "13"

    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ROLIST As String = "14"

    ''' <summary>
    ''' SessionValue(画面番号)：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_CAMPAIGN As String = "15"

    ''' <summary>
    ''' SessionValue(画面番号)：追加作業一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ADDWORKLIST As String = "22"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 写真URL取得のParameterName
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IMAGE_FILE_PATH As String = "FACEPIC_UPLOADURL"
    ''' <summary>
    ''' 写真無画像のファイルパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IMAGE_FILE_NO_PHOTO As String = "../Styles/Images/SC3240401/no_photo.png"

    ''' <summary>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ' ''' 行追加ステータス（0：追加していない行）
    ''' 行追加ステータス（0：ストール上）
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOff As String = "0"
    ''' <summary>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ' ''' 行追加ステータス（1：追加した行）
    ''' 行追加ステータス（1：サブエリア）
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

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
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' サービスステータス（11：預かり中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusDropOffCustomer As String = "11"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' ROステータス（20：FM承認待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWaitingFmApproval As String = "20"

    ''' <summary>
    ''' ROステータス（50：着工指示待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWorkOrderWait As String = "50"

    ''' <summary>
    ''' ROステータス（60：作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWorking As String = "60"

    ''' <summary>
    ''' 文字列省略値（空白文字）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const defaultValueString As String = " "

    ''' <summary>
    ''' シーケンス省略値（初期値：-1）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const defaultValueSequence As Decimal = -1
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

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
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' １つスベース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ONE_SPACE As String = " "

    ''' <summary>
    ''' 敬称が名称の後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBehindCustName As String = "1"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' 仮置きフラグ（1：仮置き）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TempFlagOn As String = "1"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    '''  Mマーク/Eマーク/Tマーク/Pマーク表示フラグ（1：Mマーク/Eマーク/Tマーク/Pマーク表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkFlgOn1 As String = "1"
    ''' <summary>
    ''' Bマーク/Lマーク表示フラグ（2：Bマーク・Lマーク表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkFlgOn2 As String = "2"
    ''' <summary>
    ''' フラグ非表示（0：マーク非表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkFlgOff As String = "0"
    '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>チップ検索</summary>
        id001 = 1
        ''' <summary>合計{0}件</summary>
        id002 = 2
        ''' <summary>車両</summary>
        id003 = 3
        ''' <summary>お客様</summary>
        id004 = 4
        ''' <summary>Mobile</summary>
        id005 = 5
        ''' <summary>/</summary>
        id006 = 6
        ''' <summary>Home</summary>
        id007 = 7
        ''' <summary>予約日時</summary>
        id008 = 8
        ''' <summary>整備内容</summary>
        id009 = 9
        ''' <summary>V</summary>
        id010 = 10
        ''' <summary>自</summary>
        id011 = 11
        ''' <summary>個</summary>
        id012 = 12
        ''' <summary>-</summary>
        id013 = 13
        ''' <summary>仮</summary>
        id014 = 14
        ''' <summary>本</summary>
        id015 = 15
        ''' <summary>前の{0}件を読み込む…</summary>
        id016 = 16
        ''' <summary>前の{0}件を読み込み中…</summary>
        id017 = 17
        ''' <summary>次の{0}件を読み込む…</summary>
        id018 = 18
        ''' <summary>次の{0}件を読み込み中…</summary>
        id019 = 19
        ''' <summary>完成検査</summary>
        id020 = 20
        ''' <summary>洗車</summary>
        id021 = 21
        ''' <summary>納車待ち</summary>
        id022 = 22
        ''' <summary>中断</summary>
        id023 = 23
        ''' <summary>No Show</summary>
        id024 = 24
        ''' <summary>R/O一覧</summary>
        id025 = 25
        ''' <summary>キャンセル</summary>
        id026 = 26
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ''' <summary>受付</summary>
        id027 = 27
        ''' <summary>追加作業</summary>
        id028 = 28
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
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
        '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
        ''' <summary>C</summary>
        id10007 = 10007
        '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
        ''' <summary>データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。</summary>
        id901 = 901
        ''' <summary>他のユーザーによって予約情報が削除されているため、選択できません。</summary>
        id902 = 902
        ''' <summary>結果が見つかりません。</summary>
        id903 = 903
        ''' <summary>R/Oがありません。</summary>
        id904 = 904
        ''' <summary>新規顧客作成しますか？</summary>
        id905 = 905
        ''' <summary>新規顧客登録をしてください。</summary>
        id906 = 906
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

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim staffInfo As StaffContext = StaffContext.Current
        objStaffContext = StaffContext.Current
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '初回読み込み時
        If Not IsPostBack Then
            'SESSION情報を取得し格納する
            Dim sessionSearchType As String = _
                CType(GetValue(ScreenPos.Current, SESSION_KEY_SERCHTYPE, False), String)
            Dim sessionSearchValue As String = _
                CType(GetValue(ScreenPos.Current, SESSION_KEY_SERCHSTRING, False), String)
            Me.HiddenSearchType.Value = sessionSearchType
            Me.HiddenSearchValue.Value = sessionSearchValue

            '権限情報保持
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Me.HiddenOperationCode.Value = CType(staffInfo.OpeCD, String)
            Me.HiddenOperationCode.Value = CType(objStaffContext.OpeCD, String)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            'ヘッダーのテキストエリアの値設定
            CType(FindControl("ctl00$MstPG_CustomerSearchTextBox"), TextBox).Text = sessionSearchValue

            '顧客一覧：初期ソート値を設定
            Me.HiddenRegisterSortType.Value = SORT_TYPE_DESC
            Me.HiddenCustomerSortType.Value = SORT_TYPE_NONE

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
            Me.ReserveHeader.Text = String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id008), _
                                                  WebWordUtility.GetWord(APPLICATION_ID, WordId.id006), _
                                                  WebWordUtility.GetWord(APPLICATION_ID, WordId.id009))

            '顧客一覧：ページング文言
            Me.BackPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id016).Replace("{0}", loadCount)
            Me.BackPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id017).Replace("{0}", loadCount)
            Me.NextPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id018).Replace("{0}", loadCount)
            Me.NextPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id019).Replace("{0}", loadCount)

            '顧客一覧：取得件数0件文言
            Me.NoSearchWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id903)

            'RO一覧：ヘッダーフッター文言
            Me.PopUpOrderListHeaderLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id025)
            Me.PopUpOrderListFooterButton.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id026)

            '制御用
            Me.HiddenOrderListDisplayType.Value = OrderListPopupTypeNone
            Me.HiddenNewCustomerConfirmType.Value = NewCustomerCheckNone
            Me.HiddenNewCustomerConfirmWord.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id905)

        End If

        'フッター設定
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Me.InitFooterButton(staffInfo)
        Me.InitFooterButton()
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

        ''顧客ボタンを活性にする
        'category = FooterMenuCategory.Customer

        Dim staffInfo As StaffContext = StaffContext.Current

        '権限によって遷移先を活性ボタンを設定する
        If staffInfo.OpeCD = Operation.SA _
            Or staffInfo.OpeCD = Operation.SM Then
            'SA、SM権限の場合、顧客詳細ボタンを活性にする
            category = FooterMenuCategory.CustomerDetail
        Else
            '他の場合、メインボタンを活性にする
            category = FooterMenuCategory.MainMenu
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

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
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Private Sub InitFooterButton()
        'Private Sub InitFooterButton(ByVal inStaffInfo As StaffContext)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(mainMenuButton) Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
            mainMenuButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_MAINMENU.ToString(CultureInfo.CurrentCulture))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END


        'SMBボタンの設定
        Dim smbButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(smbButton) Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_SMB.ToString(CultureInfo.CurrentCulture))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '顧客詳細ボタンの設定
        Dim customerButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CUSTOMER)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(customerButton) Then
            'AddHandler customerButton.Click, AddressOf CustomerButton_Click
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            customerButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_CUSTOMER.ToString(CultureInfo.CurrentCulture))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        'R/Oボタンの設定
        Dim roButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(roButton) Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ''追加作業ボタンの設定
        Dim addListButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(addListButton) Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_ADD_LIST.ToString(CultureInfo.CurrentCulture))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If

        ''CT、FM権限の場合のみ完成検査ボタンを設定する
        'If inStaffInfo.OpeCD = Operation.CT OrElse inStaffInfo.OpeCD = Operation.FM Then
        '    Dim approvalButton As CommonMasterFooterButton = _
        '        CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SERVER_CHECK_LIST)
        '    AddHandler approvalButton.Click, AddressOf ServerCheckListButton_Click
        '    approvalButton.OnClientClick = _
        '        String.Format(CultureInfo.CurrentCulture, _
        '                      FOOTER_REPLACE_EVENT, _
        '                      FOOTER_SERVER_CHECK_LIST.ToString(CultureInfo.CurrentCulture))
        'End If

        ''スケジュールボタンの設定
        'Dim scheduleButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SCHEDULE)
        'scheduleButton.OnClientClick = FOOTER_EVENT_SCHEDULER
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        If Not IsNothing(telDirectoryButton) Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        End If


        '商品訴求コンテンツボタン 
        Me.InitGoodsSolicitationContentsButtonEvent()

        'キャンペーンボタン
        Me.InitCampaignButtonEvent()

        '予約管理ボタン
        Me.InitVisitManagerButtonEvent()

        'TCメインボタン
        Me.InitTCMainButtonEvent()

        'FMメイン
        Me.InitFMMainButtonEvent()

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

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
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'ElseIf staffInfo.OpeCD = Operation.CT Then
        ElseIf staffInfo.OpeCD = Operation.CT _
            OrElse staffInfo.OpeCD = Operation.CHT Then
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            'メインメニュー(CT)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_CT)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_FM)

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

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 顧客ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    ' ''' <hitory></hitory>
    'Private Sub CustomerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '新規顧客登録画面に遷移する
    '    Me.RedirectNextScreen(NEW_CUSTOMER_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGoodsSolicitationContentsButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMの場合、商品訴求ボタンのイベントを登録する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then

            '商品訴求ボタン
            Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            'イベントをbindする
            AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
            footerGoodsSolicitationContentsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                                FOOTER_REPLACE_EVENT, _
                                                                                FOOTER_GOOD.ToString(CultureInfo.CurrentCulture))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitCampaignButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMの場合、キャンペーンボタンタップすると、現地のキャンペーン画面に遷移
        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

            'キャンペーンボタン
            Dim footerCampaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
            footerCampaignButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                              FOOTER_REPLACE_EVENT, _
                                                              FOOTER_CAMPAIGN.ToString(CultureInfo.CurrentCulture))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 予約ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitVisitManagerButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMで予約ボタンを押すと、来店管理画面に遷移する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
           OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then

            'メインメニュー
            Dim footerVisitManagerMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerVisitManagerMenuButton.Click, AddressOf footerVisitManagerMenuButton_Click
            footerVisitManagerMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                      FOOTER_REPLACE_EVENT, _
                                                                      FOOTER_RESERVE.ToString(CultureInfo.CurrentCulture))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' TCメインボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitTCMainButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CHTの場合、TCメインメニュータップすると、処理なし
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CHT Then

            'メインメニュー
            Dim footerTCMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TechnicianMain)
            footerTCMainMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                FOOTER_REPLACE_EVENT, _
                                                                FOOTER_TC.ToString(CultureInfo.CurrentCulture))
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            footerTCMainMenuButton.Enabled = False
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' FMメインボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFMMainButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CHTの場合、FMメインメニュータップすると、FMメイン画面に遷移する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CHT Then

            'メインメニュー
            Dim footerFMMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ForemanMain)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerFMMainMenuButton.Click, AddressOf footerFMMainMenuButton_Click
            footerFMMainMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                FOOTER_REPLACE_EVENT, _
                                                                FOOTER_FM.ToString(CultureInfo.CurrentCulture))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' R/O一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '基幹販売店コード、店舗コードを取得
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                     objStaffContext.BrnCD, _
                                                                                     objStaffContext.Account)
        If IsNothing(dmsDlrBrnRow) _
            OrElse dmsDlrBrnRow.IsCODE1Null _
            OrElse dmsDlrBrnRow.IsCODE2Null Then
            Throw New ArgumentException("Error: Failed to convert key dealer code.")
            Return
        End If

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

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
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

        '画面番号(RO一覧)
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ROLIST)

        ''R/O一覧画面に遷移する
        'Me.RedirectNextScreen(REPAIR_ORDERE_LIST_PAGE)
        Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 追加作業ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    ' ''' <hitory></hitory>
    'Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '追加作業一覧画面に遷移する
    '    Me.RedirectNextScreen(ADDITION_WORK_LIST_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    ' ''' <summary>
    ' ''' 完成検査ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    ' ''' <hitory></hitory>
    'Private Sub ServerCheckListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '完成検査一覧画面に遷移する
    '    Me.RedirectNextScreen(SERVER_CHECK_LIST_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    ''' <summary>
    ''' 追加作業ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '基幹販売店コード、店舗コードを取得
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                     objStaffContext.BrnCD, _
                                                                                     objStaffContext.Account)
        If IsNothing(dmsDlrBrnRow) _
            OrElse dmsDlrBrnRow.IsCODE1Null _
            OrElse dmsDlrBrnRow.IsCODE2Null Then
            Throw New ArgumentException("Error: Failed to convert key dealer code.")
            Return
        End If

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

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
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

        '画面番号(追加作業一覧)
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ADDWORKLIST)

        '決定した遷移先に遷移
        Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

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

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

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
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIEWMODE, SESSIONVALUE_VIEWMODE_PREVIEW)

            '商品訴求コンテンツ画面に遷移
            Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)
        End If

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
    Private Sub footerCampaignMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                         objStaffContext.BrnCD, _
                                                                                         objStaffContext.Account)
            If IsNothing(dmsDlrBrnRow) _
                OrElse dmsDlrBrnRow.IsCODE1Null _
                OrElse dmsDlrBrnRow.IsCODE2Null Then
                Throw New ArgumentException("Error: Failed to convert key dealer code.")
                Return
            End If

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

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
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

            '画面番号(キャンペーン)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_CAMPAIGN)

            '決定した遷移先に遷移
            Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 来店管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerVisitManagerMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then
            '決定した遷移先に遷移
            Me.RedirectNextScreen(VISIT_MANAGER_PAGE)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' FMメインボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerFMMainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.CHT Then
            '決定した遷移先に遷移
            Me.RedirectNextScreen(MAINMENU_ID_FM)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

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
            '車両降順、顧客ソートなし
            Me.SetCustomerList(SORT_TYPE_DESC, _
                               SORT_TYPE_NONE, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            '車両昇順、顧客ソートなし
            Me.SetCustomerList(SORT_TYPE_ASC, _
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
            '車両ソートなし、顧客降順
            Me.SetCustomerList(SORT_TYPE_NONE, _
                               SORT_TYPE_DESC, _
                               1, _
                               CType(Me.HiddenLoadCount.Value, Long))
        Else
            '車両ソートなし、顧客降順
            Me.SetCustomerList(SORT_TYPE_NONE, _
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

        Using biz As New SC3240401BusinessLogic
            Try
                'スタッフ情報取得
                Dim staffInfo As StaffContext = StaffContext.Current

                '選択した顧客IDと車両ID取得
                Dim customerId As Decimal = CType(Me.HiddenSelectCustomerId.Value, Decimal)
                Dim vehicleId As Decimal = CType(Me.HiddenSelectVehicleId.Value, Decimal)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                Dim svcinId As Decimal = CType(Me.HiddenSelectSvcinId.Value, Decimal)


                '顧客情報取得
                'Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                '    biz.GetCustomerInfo(staffInfo.DlrCD, _
                '                        customerId, _
                '                        vehicleId)
                Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                        biz.GetCustomerInfo(staffInfo.DlrCD, _
                                            customerId, _
                                            vehicleId, _
                                            svcinId)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                If dtCustomerInfo.Count = 0 Then
                    '顧客情報を取得できなかった場合はエラー
                    Me.ShowMessageBox(WordId.id906)

                Else
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    If dtCustomerInfo(0).IsDMS_CST_CDNull _
                        OrElse String.IsNullOrWhiteSpace(dtCustomerInfo(0).DMS_CST_CD.Trim()) Then
                        '未取引客の場合、
                        Me.ShowMessageBox(WordId.id906)

                        '取得できた場合
                        'Tactからの顧客情報取得
                        'Dim bizIC3800703 As New IC3800703BusinessLogic
                        'Dim dtServiceCustomer As IC3800703SrvCustomerDataTable = _
                        '    bizIC3800703.GetCustomerInfo(dtCustomerInfo(0).REG_NUM.Trim(), _
                        '                                 dtCustomerInfo(0).VCL_VIN.Trim(), _
                        '                                 staffInfo.DlrCD)

                        'If dtServiceCustomer.Count = 0 Then
                        '    'Tactにデータが存在しない場合
                        '    If staffInfo.OpeCD = Operation.SA Then
                        '        'SA権限の場合は新規顧客登録画面に遷移
                        '        If NewCustomerCheckRedirectNewCustomer.Equals(Me.HiddenNewCustomerConfirmType.Value) Then
                        '            Me.RedirectNewCustomerPage(dtCustomerInfo(0).DLR_CD, _
                        '                                       0, _
                        '                                       0, _
                        '                                       dtCustomerInfo(0).CST_NAME.Trim(), _
                        '                                       dtCustomerInfo(0).REG_NUM.Trim(), _
                        '                                       dtCustomerInfo(0).VCL_VIN.Trim(), _
                        '                                       dtCustomerInfo(0).VCL_KATASHIKI.Trim(), _
                        '                                       dtCustomerInfo(0).CST_PHONE.Trim(), _
                        '                                       dtCustomerInfo(0).CST_MOBILE.Trim(), _
                        '                                       String.Empty, _
                        '                                       AdvancePreparationTypeOn, _
                        '                                       VisitTypeOn)

                        '        Else
                        '            Me.HiddenNewCustomerConfirmType.Value = NewCustomerCheckConfirm

                        '        End If

                        '    Else
                        '        'SA権限以外の場合はエラー
                        '        Me.ShowMessageBox(WordId.id906)

                        '    End If
                        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    Else
                        '自社客の場合
                        '顧客詳細画面に遷移
                        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                        'Me.RedirectCustomerDetailPage(dtCustomerInfo(0).DLR_CD, _
                        '                              0, _
                        '                              0, _
                        '                              dtCustomerInfo(0).CST_NAME.Trim(), _
                        '                              dtCustomerInfo(0).REG_NUM.Trim(), _
                        '                              dtCustomerInfo(0).VCL_VIN.Trim(), _
                        '                              dtCustomerInfo(0).VCL_KATASHIKI.Trim(), _
                        '                              dtCustomerInfo(0).CST_PHONE.Trim(), _
                        '                              dtCustomerInfo(0).CST_MOBILE.Trim(), _
                        '                              String.Empty, _
                        '                              AdvancePreparationTypeOn, _
                        '                              VisitTypeOn)

                        Dim vin As String = ""
                        If Not dtCustomerInfo(0).IsVCL_VINNull Then
                            vin = dtCustomerInfo(0).VCL_VIN.Trim()
                        End If

                        Dim dmsCstCd As String = ""
                        If Not dtCustomerInfo(0).IsDMS_CST_CDNull Then
                            dmsCstCd = dtCustomerInfo(0).DMS_CST_CD.Trim()
                        End If

                        Me.RedirectCustomerDetailPage(dmsCstCd, vin)
                        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    End If
                End If
                'ボタンエリアの情報を更新
                Me.ContentUpdateButtonPanel.Update()

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
    ''' 予約エリアタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub ReserveAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles ReserveAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '予約情報確認
        Using biz As New SC3240401BusinessLogic
            Try
                'ストール利用情報を取得
                Dim dt As SC3240401DataSet.SC3240401StallUseInfoDataTable = _
                    biz.GetStallUseInfo(CType(Me.HiddenSelectStallUseId.Value, Decimal))

                '件数の確認
                If dt.Count = 0 Then
                    '取得できなかった場合はエラーメッセージを表示する
                    Me.ShowMessageBox(WordId.id902)

                Else
                    '取得できた場合はSessionにデータを格納し工程管理画面に遷移する
                    Dim dr As SC3240401DataSet.SC3240401StallUseInfoRow = _
                        DirectCast(dt.Rows(0), SC3240401DataSet.SC3240401StallUseInfoRow)

                    'Session値の設定
                    'ストール利用ID
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_STALL_USE_ID, dr.STALL_USE_ID)

                    '開始日時
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_DATE, dr.START_DATE)

                    'ストール利用ステータス
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_STALL_USE_STATUS, dr.STALL_USE_STATUS)

                    '追加行ステータス
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SUB_CHIP_TYPE, Me.HiddenSelectAddType.Value)

                    'サービス入庫ステータス
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SVC_STATUS, dr.SVC_STATUS)

                    '完成検査フラグ
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_INSPECTION_STATUS, dr.INSPECTION_STATUS)


                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    '仮置きフラグ
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_TEMP_FLG, Me.HiddenSelectTempFlag.Value)

                End If

                If Not defaultValueString.Equals(Me.HiddenSelectRoNum.Value) Then
                    'RO番号が設定されている場合

                    'ログインスタッフの情報の取得
                    Dim staffContext As StaffContext = staffContext.Current

                    'RO情報を取得
                    Dim dtRoInfo As SC3240401DataSet.SC3240401RoInfoDataTable = _
                        biz.GetRoInfo(staffContext.DlrCD, staffContext.BrnCD, Me.HiddenSelectRoNum.Value, CType(Me.HiddenSelectRoSeq.Value, Decimal))

                    '件数の確認
                    If dtRoInfo.Count = 0 Then
                        '取得できなかった場合は初期値を代入する
                        'RO番号
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_NUM, defaultValueString)

                        'RO連番
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_SEQ, defaultValueSequence)

                        'ROステータス
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_STATUS, defaultValueString)

                    Else
                        '取得できた場合はSessionにRO情報を格納する
                        Dim drRoInfo As SC3240401DataSet.SC3240401RoInfoRow = _
                            DirectCast(dtRoInfo.Rows(0), SC3240401DataSet.SC3240401RoInfoRow)

                        'RO番号
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_NUM, drRoInfo.RO_NUM)

                        'RO連番
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_SEQ, drRoInfo.RO_SEQ)

                        'ROステータス
                        Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_STATUS, drRoInfo.RO_STATUS)

                    End If
                Else
                    'RO番号が設定されていない場合、初期値を代入する
                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_NUM, defaultValueString)

                    'RO連番
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_SEQ, defaultValueSequence)

                    'ROステータス
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_RO_STATUS, defaultValueString)
                End If

                '工程管理画面に遷移
                Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END


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
    ''' 車両エリアタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub VehicleAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles VehicleAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'RO情報確認
        Using biz As New SC3240401BusinessLogic
            Try
                'RO情報の件数確認
                'Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                '    biz.GetOrderList(staffInfo.DlrCD, _
                '                     staffInfo.BrnCD, _
                '                     CType(Me.HiddenSelectCustomerId.Value, Decimal), _
                '                     CType(Me.HiddenSelectVehicleId.Value, Decimal), _
                '                     DateTimeFunc.Now(staffInfo.DlrCD))
                Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                    biz.GetOrderList(staffInfo.DlrCD, _
                                     staffInfo.BrnCD, _
                                     CType(Me.HiddenSelectCustomerId.Value, Decimal), _
                                     CType(Me.HiddenSelectVehicleId.Value, Decimal), _
                                     DateTimeFunc.Now(staffInfo.DlrCD), _
                                     CType(Me.HiddenBranchOperationDateTime.Value, Date))
                '件数の確認
                If dt.Count = 0 Then
                    '取得できなかった場合はエラーメッセージを表示する
                    Me.ShowMessageBox(WordId.id904)

                Else
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    '来店実績番号
                    Me.HiddenVisitId.Value = ""
                    'DMS予約ID
                    Me.HiddenDmsJobDtlId.Value = ""
                    'Vin(車単位でVinが一緒)
                    Me.HiddenVin.Value = ""
                    If Not dt(0).IsVCL_VINNull Then
                        Me.HiddenVin.Value = dt(0).VCL_VIN.Trim()
                    End If
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    '取得できた場合
                    'サービス入庫IDレコード数を取得
                    Dim serviceInCount As Long = (From dr As SC3240401DataSet.SC3240401ReserveInfoRow In dt _
                                                  Group By dr.SVCIN_ID, dr.RO_NUM Into Group).Count

                    If 1 < serviceInCount Then
                        '2件以上ある場合はポップアップを表示する
                        Me.HiddenOrderListDisplayType.Value = OrderListPopupTypeDisplay
                        Me.SetOrderListData(dt)

                    Else
                        '1件の場合はRO参照画面に遷移する
                        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                        'Me.RedirectOrderDetailPage(dt(0).RO_NUM)

                        '来店番号がNull可能性がある
                        Dim visitSeq As String = ""
                        If Not dt(0).IsVISITSEQNull Then
                            visitSeq = dt(0).VISITSEQ.ToString(CultureInfo.CurrentCulture)
                        End If

                        Me.RedirectOrderDetailPage(visitSeq, _
                                                   dt(0).DMS_JOB_DTL_ID, _
                                                   dt(0).RO_NUM, _
                                                   Me.HiddenVin.Value)
                        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
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
                Me.ShowMessageBox(WordId.id902)

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
    ''' <hitory></hitory>
    Protected Sub OrderAreaEventButton_Click(sender As Object, e As System.EventArgs) Handles OrderAreaEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O参照細画面に遷移する
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Me.RedirectOrderDetailPage(Me.HiddenSelectOrderNumber.Value)
        Me.RedirectOrderDetailPage(Me.HiddenVisitId.Value, _
                                   Me.HiddenDmsJobDtlId.Value.Trim(), _
                                   Me.HiddenSelectOrderNumber.Value, _
                                   Me.HiddenVin.Value)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
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
    ''' <param name="inStartRow">開始行番号</param>
    ''' <param name="inEndRow">終了行番号</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SetCustomerList(ByVal inRegisterSortType As String, _
                                ByVal inCustomerSortType As String, _
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
        Using biz As New SC3240401BusinessLogic
            Try
                '現在日時取得
                Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '当日の営業時刻の取得
                Dim branchOperatingHourDataTable As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable
                Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
                    branchOperatingHourDataTable = clsTabletSMBCommonClass.GetBranchOperatingHours(staffInfo.DlrCD, staffInfo.BrnCD)
                End Using
                '営業開始時刻
                Dim svcJobStartTime As DateTime = CType(branchOperatingHourDataTable(0)(0), DateTime)
                '当日の日付を追加する
                Dim branchOperatingDateTime As New DateTime(nowDate.Year, nowDate.Month, nowDate.Day, _
                                                            svcJobStartTime.Hour, svcJobStartTime.Minute, svcJobStartTime.Second)
                '営業開始時刻をHiddenFieldに格納
                Me.HiddenBranchOperationDateTime.Value = branchOperatingDateTime.ToString
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '検索件数取得
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'Dim customerInfoCount As Long = biz.GetCustomerListCount(staffInfo.DlrCD, _
                '                                                         staffInfo.BrnCD, _
                '                                                         Me.HiddenSearchType.Value, _
                '                                                         Me.HiddenSearchValue.Value, _
                '                                                         nowDate)
                Dim customerInfoCount As Long = biz.GetCustomerListCount(staffInfo.DlrCD, _
                                                         staffInfo.BrnCD, _
                                                         Me.HiddenSearchType.Value, _
                                                         Me.HiddenSearchValue.Value, _
                                                         nowDate, _
                                                         branchOperatingDateTime)
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '件数がある場合のみ情報取得し表示処理を行う
                If 0 < customerInfoCount Then
                    '顧客情報取得
                    ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                    '    biz.GetCustomerList(staffInfo.DlrCD, _
                    '                        staffInfo.BrnCD, _
                    '                        Me.HiddenSearchType.Value, _
                    '                        Me.HiddenSearchValue.Value, _
                    '                        nowDate, _
                    '                        inStartRow, _
                    '                        inEndRow, _
                    '                        inRegisterSortType, _
                    '                        inCustomerSortType)
                    Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                        biz.GetCustomerList(staffInfo.DlrCD, _
                                            staffInfo.BrnCD, _
                                            Me.HiddenSearchType.Value, _
                                            Me.HiddenSearchValue.Value, _
                                            nowDate, _
                                            inStartRow, _
                                            inEndRow, _
                                            inRegisterSortType, _
                                            inCustomerSortType, _
                                            branchOperatingDateTime)
                    ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    '予約情報取得
                    ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'Dim dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                    '    biz.GetReserveList(staffInfo.DlrCD, _
                    '                       staffInfo.BrnCD, _
                    '                       Me.HiddenSearchType.Value, _
                    '                       Me.HiddenSearchValue.Value, _
                    '                       nowDate, _
                    '                       dtCustomerInfo)
                    Dim dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                        biz.GetReserveList(staffInfo.DlrCD, _
                                           staffInfo.BrnCD, _
                                           Me.HiddenSearchType.Value, _
                                           Me.HiddenSearchValue.Value, _
                                           nowDate, _
                                           branchOperatingDateTime, _
                                           dtCustomerInfo)
                    ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    '写真URL取得
                    Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
                        daSystemEnvSetting.GetSystemEnvSetting(IMAGE_FILE_PATH)

                    '画面情報設定
                    Me.SetCustomerListData(dtCustomerInfo, dtReserveInfo, drSystemEnvSetting)

                    '現在のソートを保持
                    Me.HiddenRegisterSortType.Value = inRegisterSortType
                    Me.HiddenCustomerSortType.Value = inCustomerSortType

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
    ''' <param name="dtReserveInfo">予約情報</param>
    ''' <param name="drSystemEnvSetting">写真URL情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SetCustomerListData(ByVal dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable, _
                                    ByVal dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable, _
                                    ByVal drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '顧客情報をバインドする
        Me.ChipReserveAreaRepeater.DataSource = dtCustomerInfo
        Me.ChipReserveAreaRepeater.DataBind()

        For i = 0 To Me.ChipReserveAreaRepeater.Items.Count - 1
            '画面定義取得
            Dim chipReserveArea As Control = Me.ChipReserveAreaRepeater.Items(i)

            'ROW取得
            Dim drCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoRow = _
                CType(dtCustomerInfo.Rows(i), SC3240401DataSet.SC3240401CustomerInfoRow)

            '/*****************
            ' 車両情報エリア
            ' *****************/
            '車両登録番号
            CType(chipReserveArea.FindControl("RegisterNo"), CustomLabel).Text = drCustomerInfo.REG_NUM

            '車名
            If Not (drCustomerInfo.IsMODEL_NAMENull) Then
                CType(chipReserveArea.FindControl("VehicleName"), CustomLabel).Text = drCustomerInfo.MODEL_NAME
            End If

            'VIN
            If Not (drCustomerInfo.IsVCL_VINNull) Then
                CType(chipReserveArea.FindControl("Vin"), CustomLabel).Text = drCustomerInfo.VCL_VIN
            End If

            '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'Pアイコン
            If MarkFlgOn1.Equals(drCustomerInfo.IMP_VCL_FLG) Then
                CType(chipReserveArea.FindControl("PIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("PWord"), CustomLabel).Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id10005)
            End If
            'Tアイコン
            If MarkFlgOn1.Equals(drCustomerInfo.TLM_MBR_FLG) Then
                CType(chipReserveArea.FindControl("TIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("TWord"), CustomLabel).Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id10004)
            End If
            'Eアイコン
            If MarkFlgOn1.Equals(drCustomerInfo.EW_FLG) Then
                CType(chipReserveArea.FindControl("EIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("EWord"), CustomLabel).Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id10003)
            End If
            'M/Bアイコン
            If MarkFlgOn1.Equals(drCustomerInfo.SML_AMC_FLG) Then
                CType(chipReserveArea.FindControl("MIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("MWord"), CustomLabel).Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id10001)
            ElseIf MarkFlgOn2.Equals(drCustomerInfo.SML_AMC_FLG) Then
                CType(chipReserveArea.FindControl("BIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("BWord"), CustomLabel).Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id10002)
            End If
            '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            '車両情報エリアタップ時用のデータ格納
            CType(chipReserveArea.FindControl("vehicleRecord"), HtmlControl).Attributes("name") = _
                String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)

            '/*****************
            ' 顧客情報エリア
            ' *****************/
            '顧客アイコン
            If Not (IsNothing(drSystemEnvSetting)) AndAlso Not (drCustomerInfo.IsIMG_FILENull) Then
                '写真URL情報が取得できる場合
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                'CType(chipReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = _
                '    String.Concat(drSystemEnvSetting.PARAMVALUE, drCustomerInfo.IMG_FILE)
                CType(chipReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = _
                    String.Concat(drSystemEnvSetting.PARAMVALUE, drCustomerInfo.IMG_FILE, _
                                  "?", Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
            Else
                '写真URL情報が取得できない場合
                CType(chipReserveArea.FindControl("CustomerImageIcon"), Image).ImageUrl = IMAGE_FILE_NO_PHOTO
            End If

            '顧客名＋敬称
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'If drCustomerInfo.IsNAMETITLE_NAMENull Then
            If drCustomerInfo.IsNAMETITLE_NAMENull _
                Or drCustomerInfo.IsPOSITION_TYPENull Then
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '敬称がない場合
                CType(chipReserveArea.FindControl("CustomerName"), CustomLabel).Text = drCustomerInfo.CST_NAME
            Else
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''敬称がある場合
                'CType(chipReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                '    String.Concat(drCustomerInfo.CST_NAME, Space(1), drCustomerInfo.NAMETITLE_NAME)
                '名前+敬称の場合
                If PositionTypeBehindCustName.Equals(drCustomerInfo.POSITION_TYPE) Then
                    CType(chipReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                        String.Concat(drCustomerInfo.CST_NAME, Space(1), drCustomerInfo.NAMETITLE_NAME)
                Else
                    CType(chipReserveArea.FindControl("CustomerName"), CustomLabel).Text = _
                        String.Concat(drCustomerInfo.NAMETITLE_NAME, Space(1), drCustomerInfo.CST_NAME)
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            End If

            'VIPアイコン
            'CType(chipReserveArea.FindControl("VipWord"), CustomLabel).Text = "V"
            'CType(chipReserveArea.FindControl("VipIcon"), HtmlContainerControl).Attributes("style") = ""

            '自社客アイコン
            If Not (drCustomerInfo.IsCST_TYPENull) AndAlso _
               CUSTOMER_TYPE_MY_COMPANY.Equals(drCustomerInfo.CST_TYPE) Then
                '顧客区分が存在する場合
                CType(chipReserveArea.FindControl("MyCompanyWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id011)
                CType(chipReserveArea.FindControl("MyCompanyIcon"), HtmlContainerControl).Attributes("style") = ""
            End If

            '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
            ''個人アイコン
            'If Not (drCustomerInfo.IsFLEET_FLGNull) AndAlso _
            '   CORPORATION_TYPE_MINE.Equals(drCustomerInfo.FLEET_FLG) Then
            '    '法人フラグが存在する場合
            '    CType(chipReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
            '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id012)
            '    CType(chipReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
            'End If

            '個人アイコン
            If (CORPORATION_TYPE_MINE.Equals(drCustomerInfo.CST_JOIN_TYPE)) Then
                CType(chipReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id012)
                CType(chipReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
            End If
            '法人アイコン
            If (CORPORATION_TYPE_COMPANY.Equals(drCustomerInfo.CST_JOIN_TYPE)) Then
                CType(chipReserveArea.FindControl("MyVehicleWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10007)
                CType(chipReserveArea.FindControl("MyVehicleIcon"), HtmlContainerControl).Attributes("style") = ""
            End If
            '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類

            '顧客エリアタップ時用のデータ格納（顧客ID、車両ID）
            CType(chipReserveArea.FindControl("customerRecord"), HtmlControl).Attributes("name") = _
                String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)

            '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'Lアイコン
            If MarkFlgOn2.Equals(drCustomerInfo.IMP_VCL_FLG) Then
                CType(chipReserveArea.FindControl("LIcon"), HtmlContainerControl).Attributes("style") = ""
                CType(chipReserveArea.FindControl("LWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id10006)
            End If
            '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            '/*****************
            ' 電話番号エリア
            ' *****************/
            '電話番号
            If Not (drCustomerInfo.IsCST_PHONENull) Then
                CType(chipReserveArea.FindControl("TelNo"), CustomLabel).Text = drCustomerInfo.CST_PHONE
            End If

            '携帯電話番号
            If Not (drCustomerInfo.IsCST_MOBILENull) Then
                CType(chipReserveArea.FindControl("MobileNo"), CustomLabel).Text = drCustomerInfo.CST_MOBILE
            End If

            '電話番号エリアタップ時用のデータ格納（顧客ID、車両ID）
            CType(chipReserveArea.FindControl("telRecord"), HtmlControl).Attributes("name") = _
                String.Concat(drCustomerInfo.CST_ID, ",", drCustomerInfo.VCL_ID)

            '/*****************
            ' 予約エリア
            ' *****************/
            Me.SetReserveArea(dtReserveInfo, _
                              drCustomerInfo, _
                              chipReserveArea)
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 予約エリアの表示処理
    ''' </summary>
    ''' <param name="dtReserveInfo">予約情報</param>
    ''' <param name="drCustomerInfo">顧客情報</param>
    ''' <param name="inChipReserveArea">予約エリア情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SetReserveArea(ByVal dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable, _
                               ByVal drCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoRow, _
                               ByVal inChipReserveArea As Control)

        '顧客IDと車両IDで予約情報を抽出する
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        'Dim drSearchReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow() = _
        '    (From drReserveInfo In dtReserveInfo _
        '     Where drReserveInfo.CST_ID = drCustomerInfo.CST_ID _
        '     And drReserveInfo.VCL_ID = drCustomerInfo.VCL_ID _
        '     Order By drReserveInfo.SORTKEY1_RSLT_SVCIN_TYPE Ascending, _
        '          drReserveInfo.SORTKEY2_START_DATETIME Ascending, _
        '          drReserveInfo.ADDTYPE Ascending _
        '     Select drReserveInfo).ToArray
        Dim drSearchReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow() = _
            (From drReserveInfo In dtReserveInfo _
             Where drReserveInfo.CST_ID = drCustomerInfo.CST_ID _
             And drReserveInfo.VCL_ID = drCustomerInfo.VCL_ID _
             Order By drReserveInfo.SORTKEY1_RSLT_SVCIN_TYPE Ascending, _
                      drReserveInfo.SORTKEY2_START_DATETIME Ascending, _
                      drReserveInfo.ADDTYPE Ascending, _
                      drReserveInfo.SORTKEY3 Ascending, _
                      drReserveInfo.CUSTOMER_APPROVAL_DATETIME Ascending, _
                      drReserveInfo.RO_SEQ Ascending, _
                      drReserveInfo.START_DATETIME Ascending, _
                      drReserveInfo.JOB_DTL_ID Ascending _
             Select drReserveInfo).ToArray
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        '顧客で絞り込んだ予約情報をバインド
        CType(inChipReserveArea.FindControl("ReserveListAreaRepeater"), Repeater).DataSource = drSearchReserveInfo
        CType(inChipReserveArea.FindControl("ReserveListAreaRepeater"), Repeater).DataBind()

        Dim reserveListCount As Integer = _
            CType(inChipReserveArea.FindControl("ReserveListAreaRepeater"), Repeater).Items.Count
        For j = 0 To reserveListCount - 1
            '画面定義取得
            Dim reserveListArea As Control = _
                CType(inChipReserveArea.FindControl("ReserveListAreaRepeater"), Repeater).Items(j)

            'ROW取得
            Dim drReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = drSearchReserveInfo(j)

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            '開始日時 - 終了日時
            'If AddRecordTypeOff.Equals(drReserveInfo.ADDTYPE) OrElse _
            '   Not ((New String() {StallUseStatusStop, _
            '                       StallUseStatusNoVisitor}.Contains(drReserveInfo.STALL_USE_STATUS))) Then
            '    'レコード追加ステータスが「0：追加していない行」の場合、
            '    'ストール利用IDが「05：中断、07：未来店客」でない場合は表示する
            '    If String.Equals(drReserveInfo.START_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture), _
            '                     drReserveInfo.END_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture)) Then
            '        '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
            '        CType(reserveListArea.FindControl("StartDate"), CustomLabel).Text = _
            '            drReserveInfo.START_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture)

            '        CType(reserveListArea.FindControl("EndDate"), CustomLabel).Text = _
            '            String.Concat(drReserveInfo.START_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture), _
            '                          Space(1), _
            '                          WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
            '                          Space(1), _
            '                          drReserveInfo.END_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

            '    Else
            '        '日跨ぎの場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
            '        CType(reserveListArea.FindControl("StartDate"), CustomLabel).Text = _
            '            drReserveInfo.START_DATETIME.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture)

            '        CType(reserveListArea.FindControl("EndDate"), CustomLabel).Text = _
            '            String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
            '                          Space(1), _
            '                          drReserveInfo.END_DATETIME.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture))
            '    End If
            'End If

            Dim dateFormatMMdd As String = DateTimeFunc.GetDateFormat(11)
            Dim dateFormatHHmm As String = DateTimeFunc.GetDateFormat(14)
            Dim dateFormatMMddHHmm As String = String.Concat(dateFormatMMdd, " ", dateFormatHHmm)

            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'If AddRecordTypeOff.Equals(drReserveInfo.ADDTYPE) OrElse _
            '   Not ((New String() {StallUseStatusStop, _
            '   StallUseStatusNoVisitor}.Contains(drReserveInfo.STALL_USE_STATUS))) Then
            '    'レコード追加ステータスが「0：追加していない行」の場合、
            '    'ストール利用IDが「05：中断、07：未来店客」でない場合は表示する
            If AddRecordTypeOff.Equals(drReserveInfo.ADDTYPE) OrElse _
               Not ((New String() {StallUseStatusStop, _
                                   StallUseStatusNoVisitor}.Contains(drReserveInfo.STALL_USE_STATUS)) OrElse _
                    (New String() {RepairOrderStatusWaitingFmApproval, _
                                  RepairOrderStatusWorkOrderWait, _
                                  RepairOrderStatusWorking}.Contains(drReserveInfo.RO_STATUS)) OrElse _
                     TempFlagOn.Equals(drReserveInfo.TEMP_FLG)) Then
                'レコード追加ステータスが「0：ストール上」の場合、
                'ストール利用IDが「05：中断、07：未来店客」でない場合、
                'または、ROステータスが「20：FM承認待ち、50：着工指示待ち、60：作業中」でない場合、
                'または、仮置きフラグが「1：仮置き」でない場合は表示する
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                If String.Equals(drReserveInfo.START_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture), _
                                 drReserveInfo.END_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture)) Then
                    '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
                    CType(reserveListArea.FindControl("StartDate"), CustomLabel).Text = _
                        drReserveInfo.START_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture)

                    CType(reserveListArea.FindControl("EndDate"), CustomLabel).Text = _
                        String.Concat(drReserveInfo.START_DATETIME.ToString(dateFormatHHmm, CultureInfo.CurrentCulture), _
                                      Space(1), _
                                      WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                                      Space(1), _
                                      drReserveInfo.END_DATETIME.ToString(dateFormatHHmm, CultureInfo.CurrentCulture))

                Else
                    '日跨ぎの場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
                    CType(reserveListArea.FindControl("StartDate"), CustomLabel).Text = _
                        drReserveInfo.START_DATETIME.ToString(dateFormatMMddHHmm, CultureInfo.CurrentCulture)

                    CType(reserveListArea.FindControl("EndDate"), CustomLabel).Text = _
                        String.Concat(WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                                      Space(1), _
                                      drReserveInfo.END_DATETIME.ToString(dateFormatMMddHHmm, CultureInfo.CurrentCulture))
                End If
            End If
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '仮本予約アイコン
            If ReserveStatusDummy.Equals(drReserveInfo.RESV_STATUS) Then
                '仮予約アイコン
                CType(reserveListArea.FindControl("ReserveWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id014)
                CType(reserveListArea.FindControl("ReserveIcon"), HtmlContainerControl).Attributes("class") = "Icon02"

            ElseIf ReserveStatusMaster.Equals(drReserveInfo.RESV_STATUS) Then
                '本予約アイコン
                CType(reserveListArea.FindControl("ReserveWord"), CustomLabel).Text = _
                    WebWordUtility.GetWord(APPLICATION_ID, WordId.id015)
                CType(reserveListArea.FindControl("ReserveIcon"), HtmlContainerControl).Attributes("class") = "Icon01"

            End If

            'ストール名
            If AddRecordTypeOff.Equals(drReserveInfo.ADDTYPE) Then
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''レコード追加ステータスが「0:追加していない行」の場合はストール名を表示する
                'レコード追加ステータスが「0:ストール上」の場合はストール名を表示する
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                If Not (drReserveInfo.IsSTALL_NAMENull) Then
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = drReserveInfo.STALL_NAME

                End If
            Else
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''レコード追加ステータスが「1:追加した行」の場合はそれぞれに適応した文言を表示する
                'レコード追加ステータスが「1:サブエリア」の場合はそれぞれに適応した文言を表示する
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                If New String() {RepairOrderStatusWorkOrderWait, _
                                     RepairOrderStatusWorking}.Contains(drReserveInfo.RO_STATUS) OrElse _
                                 TempFlagOn.Equals(drReserveInfo.TEMP_FLG) Then
                    'ROステータスが「50：着工指示待ち、60：作業中」の場合
                    'もしくは、仮置きフラグが「1：仮置き」の場合
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id027)
                ElseIf RepairOrderStatusWaitingFmApproval.Equals(drReserveInfo.RO_STATUS) Then
                    'ROステータスが「20：FM承認待ち」の場合
                    '「追加作業」を表示する
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id028)
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                ElseIf (New String() {ServiceStatusWaitWash, _
                                  ServiceStatusWashing}.Contains(drReserveInfo.SVC_STATUS)) Then
                    'サービスステータスが「07：洗車待ち、08：洗車中」の場合
                    '「洗車」を表示する
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id021)

                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'ElseIf ServiceStatusWaitDelivery.Equals(drReserveInfo.SVC_STATUS) Then
                    '        'サービスステータスが「12：納車待ち」の場合
                    '        '「納車待ち」を表示する
                ElseIf New String() {ServiceStatusDropOffCustomer, _
                                     ServiceStatusWaitDelivery}.Contains(drReserveInfo.SVC_STATUS) Then
                    'サービスステータスが「11：預かり中、12：納車待ち」の場合
                    '「納車待ち」を表示する
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id022)

                ElseIf StallUseStatusStop.Equals(drReserveInfo.STALL_USE_STATUS) Then
                    'ストール利用ステータスが「05：中断」の場合
                    '「中断」を表示する
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id023)

                ElseIf StallUseStatusNoVisitor.Equals(drReserveInfo.STALL_USE_STATUS) Then
                    'ストール利用ステータスが「07：未来店客」の場合
                    '「No Show」を表示する
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id024)

                ElseIf ApprovalStatusWaitApproval.Equals(drReserveInfo.INSPECTION_STATUS) Then
                    '完成検査ステータスが「1：完成検査承認待ち」の場合
                    '「完成検査」を表示する
                    CType(reserveListArea.FindControl("StallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id020)

                End If
            End If

            '整備名称
            If Not (drReserveInfo.IsSERVICE_NAMENull) Then
                CType(reserveListArea.FindControl("ServiceName"), CustomLabel).Text = drReserveInfo.SERVICE_NAME
            End If

            '入庫アイコン
            If drReserveInfo.IsRSLT_SVCIN_DATETIMENull OrElse drReserveInfo.RSLT_SVCIN_DATETIME = Date.MinValue Then
                '未入庫の場合は表示しない
                CType(reserveListArea.FindControl("ServiceInIcon"), HtmlContainerControl).Attributes("style") = "visibility:hidden;"
            End If

            '担当名
            If Not (drReserveInfo.IsSTF_NAMENull) Then
                CType(reserveListArea.FindControl("StaffName"), CustomLabel).Text = drReserveInfo.STF_NAME
            End If


            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            ''タップ時用のデータ格納（ストール利用ID、追加行ステータス）
            ''CType(reserveListArea.FindControl("reserveInfoRecord"), HtmlControl).Attributes("name") = _
            ''    String.Concat(CType(drReserveInfo.STALL_USE_ID, String), ",", drReserveInfo.ADDTYPE)
            'CType(reserveListArea.FindControl("reserveInfoRecord"), HtmlControl).Attributes("name") = _
            '     String.Concat(CType(drReserveInfo.STALL_USE_ID, String), ",", drReserveInfo.ADDTYPE, ",", drReserveInfo.SVCIN_ID)
            ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            'タップ時用のデータ格納（ストール利用ID、追加行ステータス、サービス入庫ID、RO番号、RO連番、仮置きフラグ）
            Dim attributeList As ArrayList = New ArrayList()

            attributeList.Add(drReserveInfo.STALL_USE_ID.ToString)
            attributeList.Add(drReserveInfo.ADDTYPE)
            attributeList.Add(drReserveInfo.SVCIN_ID.ToString)
            attributeList.Add(drReserveInfo.RO_NUM)
            attributeList.Add(drReserveInfo.RO_SEQ.ToString)
            attributeList.Add(drReserveInfo.TEMP_FLG)

            Dim serializer As New JavaScriptSerializer

            CType(reserveListArea.FindControl("reserveInfoRecord"), HtmlControl).Attributes("name") = _
                serializer.Serialize(attributeList)
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        Next

        '高さ調整
        If 1 < CType(inChipReserveArea.FindControl("ReserveListAreaRepeater"), Repeater).Items.Count Then
            '行の高さを設定
            Dim recordHeight As String = CType((reserveListCount * 80) + 3, String)
            CType(inChipReserveArea.FindControl("chipReserveRow"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inChipReserveArea.FindControl("vehicleRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inChipReserveArea.FindControl("customerRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inChipReserveArea.FindControl("telRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inChipReserveArea.FindControl("reserveRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"

        End If
    End Sub

#End Region

#Region "RO一覧表示処理"

    ''' <summary>
    ''' RO一覧画面出力処理
    ''' </summary>
    ''' <param name="dtReserveInfo">RO情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SetOrderListData(ByVal dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        'ソートしたものを取得する
        'Dim drSearchReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow() = _
        '    (From drReserveInfo In dtReserveInfo _
        '     Order By drReserveInfo.SORTKEY1_RSLT_SVCIN_TYPE Ascending, _
        '              drReserveInfo.SORTKEY2_START_DATETIME Ascending, _
        '              drReserveInfo.ADDTYPE Ascending _
        '     Select drReserveInfo).ToArray
        Dim drSearchReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow() = _
            (From drReserveInfo In dtReserveInfo _
             Order By drReserveInfo.SORTKEY1_RSLT_SVCIN_TYPE Ascending, _
                      drReserveInfo.SORTKEY2_START_DATETIME Ascending, _
                      drReserveInfo.ADDTYPE Ascending, _
                      drReserveInfo.SORTKEY3 Ascending, _
                      drReserveInfo.CUSTOMER_APPROVAL_DATETIME Ascending, _
                      drReserveInfo.RO_SEQ Ascending, _
                      drReserveInfo.START_DATETIME Ascending, _
                      drReserveInfo.JOB_DTL_ID Ascending _
             Select drReserveInfo).ToArray
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        'RO情報をバインドする
        Me.OrderListRepeater.DataSource = drSearchReserveInfo
        Me.OrderListRepeater.DataBind()

        For i = 0 To Me.OrderListRepeater.Items.Count - 1
            '画面定義取得
            Dim orderListRepeater As Control = Me.OrderListRepeater.Items(i)

            'ROW取得
            Dim drOrderInfo As SC3240401DataSet.SC3240401ReserveInfoRow = drSearchReserveInfo(i)

            'RO番号
            CType(orderListRepeater.FindControl("OrderNumber"), CustomLabel).Text = drOrderInfo.RO_NUM

            '開始日時 - 終了日時
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'If AddRecordTypeOff.Equals(drOrderInfo.ADDTYPE) OrElse _
            '    Not ((New String() {StallUseStatusStop, _
            '                        StallUseStatusNoVisitor}.Contains(drOrderInfo.STALL_USE_STATUS))) Then
            If AddRecordTypeOff.Equals(drOrderInfo.ADDTYPE) OrElse _
                Not ((New String() {StallUseStatusStop, _
                                    StallUseStatusNoVisitor}.Contains(drOrderInfo.STALL_USE_STATUS)) OrElse _
                     (New String() {RepairOrderStatusWaitingFmApproval, _
                                    RepairOrderStatusWorkOrderWait, _
                                    RepairOrderStatusWorking}.Contains(drOrderInfo.RO_STATUS)) OrElse _
                      TempFlagOn.Equals(drOrderInfo.TEMP_FLG)) Then

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'レコード追加ステータスが「0：追加していない行」の場合、
                'ストール利用IDが「05：中断、07：未来店客」でない場合は表示する
                'If String.Equals(drOrderInfo.START_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture), _
                '                 drOrderInfo.END_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture)) Then
                '    '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
                '    CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                '        String.Concat(drOrderInfo.START_DATETIME.ToString("MM/dd", CultureInfo.CurrentCulture), _
                '                      Space(1), _
                '                      drOrderInfo.START_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture), _
                '                      Space(1), _
                '                      WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                '                      Space(1), _
                '                      drOrderInfo.END_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

                'Else
                '    '日跨ぎ出ない場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
                '    CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                '        String.Concat(drOrderInfo.START_DATETIME.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture), _
                '                      Space(1), _
                '                      WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                '                      Space(1), _
                '                      drOrderInfo.END_DATETIME.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture))

                'End If

                Dim dateFormatMMdd As String = DateTimeFunc.GetDateFormat(11)
                Dim dateFormatHHmm As String = DateTimeFunc.GetDateFormat(14)
                Dim dateFormatMMddHHmm As String = String.Concat(dateFormatMMdd, " ", dateFormatHHmm)

                If String.Equals(drOrderInfo.START_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture), _
                 drOrderInfo.END_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture)) Then
                    '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
                    CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                        String.Concat(drOrderInfo.START_DATETIME.ToString(dateFormatMMdd, CultureInfo.CurrentCulture), _
                                      Space(1), _
                                      drOrderInfo.START_DATETIME.ToString(dateFormatHHmm, CultureInfo.CurrentCulture), _
                                      Space(1), _
                                      WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                                      Space(1), _
                                      drOrderInfo.END_DATETIME.ToString(dateFormatHHmm, CultureInfo.CurrentCulture))

                Else
                    '日跨ぎ出ない場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
                    CType(orderListRepeater.FindControl("OrderStartEndDate"), CustomLabel).Text = _
                        String.Concat(drOrderInfo.START_DATETIME.ToString(dateFormatMMddHHmm, CultureInfo.CurrentCulture), _
                                      Space(1), _
                                      WebWordUtility.GetWord(APPLICATION_ID, WordId.id013), _
                                      Space(1), _
                                      drOrderInfo.END_DATETIME.ToString(dateFormatMMddHHmm, CultureInfo.CurrentCulture))

                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            End If

            'ストール名
            If AddRecordTypeOff.Equals(drOrderInfo.ADDTYPE) Then
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''追加行ステータス「0：追加していない行」の場合はストール名を表示する
                '追加行ステータス「0:ストール上」の場合はストール名を表示する
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                If Not (drOrderInfo.IsSTALL_NAMENull) Then
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = drOrderInfo.STALL_NAME

                End If

            Else
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''追加行ステータス「0：追加していない行」の場合はそれぞれに適応した文言を表示する
                '追加行ステータス「1:サブエリア」の場合はそれぞれに適応した文言を表示する

                'If (New String() {ServiceStatusWaitWash, _
                '                  ServiceStatusWashing}.Contains(drOrderInfo.SVC_STATUS)) Then
                '    'サービスステータスが「07：洗車待ち、08：洗車中」の場合
                '    '「洗車」を表示する
                '    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                '        WebWordUtility.GetWord(APPLICATION_ID, WordId.id021)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                If New String() {RepairOrderStatusWorkOrderWait, _
                                     RepairOrderStatusWorking}.Contains(drOrderInfo.RO_STATUS) OrElse _
                                 TempFlagOn.Equals(drOrderInfo.TEMP_FLG) Then
                    'ROステータスが「50：着工指示待ち、60：作業中」の場合
                    'もしくは、仮置きフラグが「1：仮置き」の場合
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id027)
                ElseIf RepairOrderStatusWaitingFmApproval.Equals(drOrderInfo.RO_STATUS) Then
                    'ROステータスが「20：FM承認待ち」の場合
                    '「追加作業」を表示する
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id028)

                ElseIf (New String() {ServiceStatusWaitWash, _
                                  ServiceStatusWashing}.Contains(drOrderInfo.SVC_STATUS)) Then
                    'サービスステータスが「07：洗車待ち、08：洗車中」の場合
                    '「洗車」を表示する
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id021)
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'ElseIf ServiceStatusWaitDelivery.Equals(drOrderInfo.SVC_STATUS) Then
                    '    'サービスステータスが「12：納車待ち」の場合
                    '    '「納車待ち」を表示する
                ElseIf New String() {ServiceStatusDropOffCustomer, _
                                     ServiceStatusWaitDelivery}.Contains(drOrderInfo.SVC_STATUS) Then
                    'サービスステータスが「11：預かり中、12：納車待ち」の場合
                    '「納車待ち」を表示する
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id022)

                ElseIf StallUseStatusStop.Equals(drOrderInfo.STALL_USE_STATUS) Then
                    'ストール利用ステータスが「05：中断」の場合
                    '「中断」を表示する
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id023)

                ElseIf StallUseStatusNoVisitor.Equals(drOrderInfo.STALL_USE_STATUS) Then
                    'ストール利用ステータスが「07：未来店客」の場合
                    '「No Show」を表示する
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id024)

                ElseIf ApprovalStatusWaitApproval.Equals(drOrderInfo.INSPECTION_STATUS) Then
                    '完成検査ステータスが「1：完成検査承認待ち」の場合
                    '「完成検査」を表示する
                    CType(orderListRepeater.FindControl("OrderStallName"), CustomLabel).Text = _
                        WebWordUtility.GetWord(APPLICATION_ID, WordId.id020)

                End If

            End If

            '整備名称
            If Not (drOrderInfo.IsSERVICE_NAMENull) Then
                CType(orderListRepeater.FindControl("OrderServiceName"), CustomLabel).Text = drOrderInfo.SERVICE_NAME
            End If

            'RO一覧エリアタップ時用のデータ格納
            CType(orderListRepeater.FindControl("OrderListItem"), HtmlControl).Attributes("name") = drOrderInfo.RO_NUM
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            CType(orderListRepeater.FindControl("OrderListItem"), HtmlControl).Attributes("dmsJobDtlId") = drOrderInfo.DMS_JOB_DTL_ID
            CType(orderListRepeater.FindControl("OrderListItem"), HtmlControl).Attributes("visitSeq") = CType(drOrderInfo.VISITSEQ, String)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Next

        'ROポップアップ一覧エリア更新
        Me.ContentUpdatePopuupPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "画面遷移処理"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 新規顧客登録画面（SC3080207）遷移
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inVisitSequence">来店管理連番</param>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inCustomerName">顧客名</param>
    ' ''' <param name="inRegisterNo">車両登録番号</param>
    ' ''' <param name="inVin">VIN</param>
    ' ''' <param name="inModelCode">車名</param>
    ' ''' <param name="inTelNo">電話番号</param>
    ' ''' <param name="inMobileNo">携帯番号</param>
    ' ''' <param name="inSaCode">SAコード</param>
    ' ''' <param name="inAdvancePreparation">事前準備フラグ</param>
    ' ''' <param name="inVisitType">受付フラグ</param>
    ' ''' <remarks></remarks>
    ' ''' <hitory></hitory>
    'Private Sub RedirectNewCustomerPage(ByVal inDealerCode As String, _
    '                                    ByVal inVisitSequence As Long, _
    '                                    ByVal inServiceInId As Decimal, _
    '                                    ByVal inCustomerName As String, _
    '                                    ByVal inRegisterNo As String, _
    '                                    ByVal inVin As String, _
    '                                    ByVal inModelCode As String, _
    '                                    ByVal inTelNo As String, _
    '                                    ByVal inMobileNo As String, _
    '                                    ByVal inSaCode As String, _
    '                                    ByVal inAdvancePreparation As String, _
    '                                    ByVal inVisitType As String)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '次画面遷移パラメータ設定

    '    '販売店コード
    '    Dim dealerCode As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inDealerCode)) Then dealerCode = inDealerCode
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_CRDEALERCODE, dealerCode)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.CRDEALERCODE:{0}", dealerCode))

    '    '来店者管理連番
    '    Dim visitSequence As String = String.Empty
    '    If 0 < inVisitSequence Then visitSequence = CType(inVisitSequence, String)
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_VISITSEQ, visitSequence)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.VISITSEQ:{0}", visitSequence))

    '    'サービス入庫ID
    '    Dim serviceInId As String = String.Empty
    '    If 0 < inServiceInId Then serviceInId = CType(inServiceInId, String)
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_REZID, serviceInId)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.REZID:{0}", serviceInId))

    '    '顧客名
    '    Dim customerName As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inCustomerName)) Then customerName = inCustomerName
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_NAME, customerName)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.NAME:{0}", customerName))

    '    '車両登録No
    '    Dim registerNo As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inRegisterNo)) Then registerNo = inRegisterNo
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_REGISTERNO, registerNo)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.REGISTERNO:{0}", registerNo))

    '    'VIN
    '    Dim vin As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inVin)) Then vin = inVin
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_VINNO, vin)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.VINNO:{0}", vin))

    '    'モデルコード
    '    Dim modelCode As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inModelCode)) Then modelCode = inModelCode
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_MODELCODE, modelCode)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.MODELCODE:{0}", modelCode))

    '    '電話番号
    '    Dim telNo As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inTelNo)) Then telNo = inTelNo
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_TEL1, telNo)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.TEL1:{0}", telNo))

    '    '携帯番号
    '    Dim mobileNo As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inMobileNo)) Then mobileNo = inMobileNo
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_TEL2, mobileNo)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.TEL2:{0}", mobileNo))

    '    'SAコード
    '    Dim saCode As String = String.Empty
    '    If Not (String.IsNullOrEmpty(inSaCode)) Then saCode = inSaCode
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_SACODE, saCode)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.SACODE:{0}", saCode))

    '    '事前準備フラグ
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_PREPARECHIPFLAG, inAdvancePreparation)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.PREPARECHIPFLAG:{0}", inAdvancePreparation))

    '    '受付フラグ
    '    Me.SetValue(ScreenPos.Next, SESSIONKEY_RECEPTIONFLAG, inVisitType)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.RECEPTIONFLAG:{0}", inVisitType))

    '    ' 新規顧客登録画面に遷移
    '    Me.RedirectNextScreen(NEW_CUSTOMER_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 顧客詳細画面遷移
    ''' </summary>
    ''' <param name="inDmsCstId">基幹顧客ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Private Sub RedirectCustomerDetailPage(ByVal inDmsCstId As String, _
                                           ByVal inVin As String)
        'Private Sub RedirectCustomerDetailPage(ByVal inDealerCode As String, _
        '                                       ByVal inVisitSequence As Long, _
        '                                       ByVal inServiceInId As Decimal, _
        '                                       ByVal inCustomerName As String, _
        '                                       ByVal inRegisterNo As String, _
        '                                       ByVal inVin As String, _
        '                                       ByVal inModelCode As String, _
        '                                       ByVal inTelNo As String, _
        '                                       ByVal inMobileNo As String, _
        '                                       ByVal inSaCode As String, _
        '                                       ByVal inAdvancePreparation As String, _
        '                                       ByVal inVisitType As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '次画面遷移パラメータ設定
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''販売店コード
        'Dim dealerCode As String = String.Empty
        'If Not (String.IsNullOrEmpty(inDealerCode)) Then dealerCode = inDealerCode
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_CRDEALERCODE, dealerCode)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.CRDEALERCODE:{0}", dealerCode))

        ''来店者管理連番
        'Dim visitSequence As String = String.Empty
        'If 0 < inVisitSequence Then visitSequence = CType(inVisitSequence, String)
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_VISITSEQ, visitSequence)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.VISITSEQ:{0}", visitSequence))

        ''サービス入庫ID
        'Dim serviceInId As String = String.Empty
        'If 0 < inServiceInId Then serviceInId = CType(inServiceInId, String)
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_REZID, serviceInId)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.REZID:{0}", serviceInId))

        ''顧客名
        'Dim customerName As String = String.Empty
        'If Not (String.IsNullOrEmpty(inCustomerName)) Then customerName = inCustomerName
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_NAME, customerName)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.NAME:{0}", customerName))

        ''車両登録No
        'Dim registerNo As String = String.Empty
        'If Not (String.IsNullOrEmpty(inRegisterNo)) Then registerNo = inRegisterNo
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_REGISTERNO, registerNo)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.REGISTERNO:{0}", registerNo))

        ''VIN
        'Dim vin As String = String.Empty
        'If Not (String.IsNullOrEmpty(inVin)) Then vin = inVin
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_VINNO, vin)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.VINNO:{0}", vin))

        ''モデルコード
        'Dim modelCode As String = String.Empty
        'If Not (String.IsNullOrEmpty(inModelCode)) Then modelCode = inModelCode
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_MODELCODE, modelCode)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.MODELCODE:{0}", modelCode))

        ''電話番号
        'Dim telNo As String = String.Empty
        'If Not (String.IsNullOrEmpty(inTelNo)) Then telNo = inTelNo
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_TEL1, telNo)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.TEL1:{0}", telNo))

        ''携帯番号
        'Dim mobileNo As String = String.Empty
        'If Not (String.IsNullOrEmpty(inMobileNo)) Then mobileNo = inMobileNo
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_TEL2, mobileNo)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.TEL2:{0}", mobileNo))

        ''SAコード
        'Dim saCode As String = String.Empty
        'If Not (String.IsNullOrEmpty(inSaCode)) Then saCode = inSaCode
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_SACODE, saCode)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.SACODE:{0}", saCode))

        ''事前準備フラグ
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_PREPARECHIPFLAG, inAdvancePreparation)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.PREPARECHIPFLAG:{0}", inAdvancePreparation))

        ''受付フラグ
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_RECEPTIONFLAG, inVisitType)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.RECEPTIONFLAG:{0}", inVisitType))

        ''固定フラグ
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_FLAG, FixationTypeOne)
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "Redirect.FLAG:{0}", FixationTypeOne))

        '基幹顧客ID
        Dim dmsCstId As String = String.Empty
        If Not (String.IsNullOrEmpty(inDmsCstId)) Then dmsCstId = inDmsCstId.Trim()
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TOTMEJ_DMSCSTID, dmsCstId)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}:{1}", SESSIONKEY_TOTMEJ_DMSCSTID, dmsCstId))

        'VIN
        Dim vin As String = String.Empty
        If Not (String.IsNullOrEmpty(inVin)) Then vin = inVin.Trim()
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TOTMEJ_VIN, vin)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}:{1}", SESSIONKEY_TOTMEJ_VIN, vin))

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '顧客詳細画面に遷移
        Me.RedirectNextScreen(CUSTOMER_DETAIL_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' ROプレビュー画面遷移
    ''' </summary>
    ''' <param name="inVisitId">来店番号</param>
    ''' <param name="inOrdeNumber">RO番号</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Private Sub RedirectOrderDetailPage(ByVal inVisitId As String, _
                                        ByVal inDmsJobDtlId As String, _
                                        ByVal inOrdeNumber As String, _
                                        ByVal inVin As String)
        'Private Sub RedirectOrderDetailPage(ByVal inOrdeNumber As String)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''次画面遷移パラメータ設定
        ''ストール利用ID
        'Me.SetValue(ScreenPos.Next, SESSIONKEY_ORDERNO, inOrdeNumber)

        ''R/O参照細画面に遷移
        'Me.RedirectNextScreen(ORDER_DETAIL_PAGE)

        '基幹販売店コード、店舗コードを取得
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                     objStaffContext.BrnCD, _
                                                                                     objStaffContext.Account)
        If IsNothing(dmsDlrBrnRow) _
            OrElse dmsDlrBrnRow.IsCODE1Null _
            OrElse dmsDlrBrnRow.IsCODE2Null Then
            Throw New ArgumentException("Error: Failed to convert key dealer code.")
            Return
        End If

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, inVisitId.Trim())

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, inDmsJobDtlId.Trim())

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, inOrdeNumber.Trim())

        'RO作業連番:作業連番は複数がある可能なので、0を渡す
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "0")

        '車両登録NOのVIN
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, inVin.Trim())

        'RO作成フラグ
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

        'Format:「0：プレビュー」固定
        Me.SetValue(ScreenPos.Next, SESSIONKEY_FORMAT, SESSIONVALUE_FORMAT_PREVIEW)

        '入庫管理番号を空文字に設定する
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SVCIN_NUM, "")

        '入庫販売店コードを空文字に設定する
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SVCIN_DLRCD, "")

        '画面番号(ROプレビュー)
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ROPREVIEW)

        'R/O参照画面に遷移する
        Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "DMS販売店コード、店舗コードの取得する"

    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>中断情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetDmsBlnCd(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal account As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            '基幹販売店コード、店舗コードを取得
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                                dealerCode, _
                                                                branchCode, _
                                                                String.Empty, _
                                                                account)
            If dmsDlrBrnTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode: Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable.Item(0)

    End Function

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

End Class
