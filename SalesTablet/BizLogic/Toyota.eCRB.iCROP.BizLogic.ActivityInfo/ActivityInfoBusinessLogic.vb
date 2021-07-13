'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ActivityInfoBusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/02/27 TCS 安田 【SALES_2】
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2013/06/30 TCS 徐   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）
'更新： 2014/02/12 TCS 高橋、山口 受注後フォロー機能開発
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/30 TCS 市川 TMT不具合対応
'更新： 2014/05/31 TCS 外崎 TMT不具合対応
'更新： 2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応)
'更新： 2014/08/05 TCS 森   受注後活動A⇒H退避対応
'更新： 2014/09/01 TCS 松月 【A STEP2】ToDo連携店舗コード変更対応(初期活動店舗)（問連TR-V4-GTMC140807001）
'更新： 2015/03/12 TCS 藤井 セールスタブレット：0118
'更新： 2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）
'更新： 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2016/11/08 TCS 河原 TR-SLT-TMT-20161020-001
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/08/27 TCS 佐々木     TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061)
'更新： 2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

'2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
'Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
'2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.Common.VisitResult.BizLogic

'2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
'Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.iCROP.BizLogic.IC3802801

'2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

Public Class ActivityInfoBusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ' 活動結果(Success)
    Public Const CRACTRESULT_SUCCESS As String = "31"
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    ' アクションコード(SEQ)
    ' カタログ
    Public Const ACTIONCD_CATALOG As String = "A22"
    ' 試乗
    Public Const ACTIONCD_TESTDRIVE As String = "A26"
    ' 査定
    Public Const ACTIONCD_EVALUATION As String = "A30"
    ' 見積
    Public Const ACTIONCD_QUOTATION As String = "A23"

    ' 受注時
    Public Const SALESAFTER_NO As String = "0"
    ' 受注後
    Public Const SALESAFTER_YES As String = "1"

    ''' <summary>
    ''' カタログ用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_CATALOG As Integer = 9

    ''' <summary>
    ''' 試乗用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_TESTDRIVE As Integer = 16

    ''' <summary>
    ''' 査定用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_ASSESSMENT As Integer = 18

    ''' <summary>
    ''' 見積り用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_VALUATION As Integer = 10

    ''' <summary>
    ''' HotのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_HOT_ACTIONCD As String = "D06"

    ''' <summary>
    ''' ProspectのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_PROSPECT_ACTIONCD As String = "D05"

    ''' <summary>
    ''' SuccessのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SUCCESS_ACTIONCD As String = "D01"

    ''' <summary>
    ''' Give-upのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_GIVEUP_ACTIONCD As String = "D02"

    ''' <summary>
    ''' 成約時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SUCCESS_CRRSLTID As String = "SUCCESS_CRRSLTID"

    ''' <summary>
    ''' 継続時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_CONTINUE_CRRSLTID As String = "CONTINUE_CRRSLTID"

    ''' <summary>
    ''' 断念時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_GIVEUP_CRRSLTID As String = "GIVEUP_CRRSLTID"

    ''' <summary>
    ''' Hot・Procpect時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_HOTPROSPECT_CRRSLTID As String = "HOTPROSPECT_CRRSLTID"

    ''' <summary>
    ''' Walk-in時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_WALKINREQUEST_CRRSLTID As String = "WALKINREQUEST_CRRSLTID"

    ''' <summary>
    ''' 来店区分取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_WALKIN_WICID As String = "WALKIN_WICID"

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 用件ソース(1st）コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USED_FLG_SOURCE1EDIT As String = "USED_FLG_SOURCE1EDIT"
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 敬称前後取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_KEISYO_ZENGO As String = "KEISYO_ZENGO"

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ V3データ表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_ICROP_OLD_SYSTEM_DISP_FLG As String = "ICROP_OLD_SYSTEM_DISP_FLG"
    ''' <summary>
    ''' システム設定の指定パラメータ 受注後工程利用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USE_AFTER_ODR_PROC_FLG As String = "USE_AFTER_ODR_PROC_FLG"
    ''' <summary>
    ''' サフィックス使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_SUFFIX As String = "USE_FLG_SUFFIX"
    ''' <summary>
    ''' 内装色使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_INTERIORCLR As String = "USE_FLG_INTERIORCLR"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' 受注後フラグ（0:受注時）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesFlg As String = "0"
    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End


    ''' <summary>
    ''' Follow-upBoxのCR活動スタータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FLLWUP_HOT = "1"
    Private Const C_FLLWUP_PROSPECT = "2"
    Private Const C_FLLWUP_REPUCHASE = "3"
    Private Const C_FLLWUP_PERIODICAL = "4"
    Private Const C_FLLWUP_PROMOTION = "5"
    Private Const C_FLLWUP_REQUEST = "6"
    Private Const C_FLLWUP_WALKIN = "7"


    ''' <summary>
    ''' Follow-upBoxの活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CRACTRSLT_HOT As String = "1"
    Private Const C_CRACTRSLT_PROSPECT As String = "2"
    Private Const C_CRACTRSLT_SUCCESS As String = "3"
    Private Const C_CRACTRSLT_CONTINUE As String = "4"
    Private Const C_CRACTRSLT_GIVEUP As String = "5"

    ''' <summary>
    ''' 画面で選択する活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RSLT_WALKIN As String = "1"
    Private Const C_RSLT_PROSPECT As String = "2"
    Private Const C_RSLT_HOT As String = "3"
    Private Const C_RSLT_SUCCESS As String = "4"
    Private Const C_RSLT_GIVEUP As String = "5"


    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    Private Const CRACTRESULT_HOT As String = "21"
    Private Const CRACTRESULT_GIVEUP As String = "32"
    Private Const CRACTSTATUS_HOT As String = "30"
    Private Const CRACTSTATUS_PROSPECT As String = "20"
    Private Const CRACTSTATUS_WALKIN As String = "10"
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    Private Const C_CRACTRESULT_HOT As String = "1"
    Private Const C_CRACTRESULT_PROSPECT As String = "2"
    Private Const C_CRACTRESULT_NOTACT As String = "0"
    Private Const C_CRACTRESULT_CONTINUE As String = "4"


    '1：Periodical Inspection  2：Repurchase Follow-up  3：Others  4:Birthday

    Private Const C_CRACTCATEGORY_DEFFULT As String = "0"
    Private Const C_CRACTCATEGORY_PERIODICAL As String = "1"
    Private Const C_CRACTCATEGORY_REPURCHASE As String = "2"
    Private Const C_CRACTCATEGORY_OTHERS As String = "3"
    Private Const C_CRACTCATEGORY_BIRTHDAY As String = "4"

    '1：Walk-in  2：Call-in  3：RMM  4：Request
    Private Const C_REQCATEGORY_WALKIN As String = "1"
    Private Const C_REQCATEGORY_CALLIN As String = "2"
    Private Const C_REQCATEGORY_RMM As String = "3"
    Private Const C_REQCATEGORY_REQUEST As String = "4"


    Private Const C_DONECAT_HOT = "6"
    Private Const C_DONECAT_PROSPECT = "7"
    Private Const C_DONECAT_REPURCHASE = "2"
    Private Const C_DONECAT_PERIODICAL = "1"
    Private Const C_DONECAT_PROMOTION = "3"
    Private Const C_DONECAT_REQUEST = "4"
    Private Const C_DONECAT_WALKIN = "5"

    'Sales Staff権限の権限コード
    Private Const C_SALESSTAFFOPECD As String = "8"

    'CalDAV連携用URL
    Private Const C_CALDAV_WEBSERVICE_URL As String = "CALDAV_WEBSERVICE_URL"

    ''' <summary>
    ''' 在席状態：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_NEGOTIATION As String = "20"

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    ''' <summary>
    ''' 在席状態：納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_DELIVERY As String = "22"
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' SC3080216 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SC3080216_MODULEID As String = "SC3080216"

    '2015/12/21 TCS 鈴木 受注後工程蓋閉め対応 START
    ''' <summary>
    ''' SC3080203 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SC3080203_MODULEID As String = "SC3080203"
    '2015/12/21 TCS 鈴木 受注後工程蓋閉め対応 END

    ''' <summary>
    ''' 査定依頼機能使用可否フラグ(パラメータ名称)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USED_FLG_ASSESS_PRMNAME As String = "USED_FLG_ASSESS"

    ''' <summary>
    ''' 査定依頼機能使用可否フラグ(機能を使用する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USED_FLG_ASSESS_ON As String = "1"

    ''' <summary>
    ''' 査定実績フラグ(査定実績あり)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_ASMFLG_ON As String = "1"
    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ''' <summary>
    ''' 査定実績判定(査定実績なし)
    ''' </summary>
    Private Const C_ASMACTSTATUS_NASI As String = "0"

    ''' <summary>
    ''' 査定実績判定(査定実績あり＆査定回答済)
    ''' </summary>
    Private Const C_ASMACTSTATUS_ARI As String = "1"

    ''' <summary>
    ''' 査定実績判定(査定実績あり＆査定未回答)
    ''' </summary>
    Private Const C_ASMACTSTATUS_MIKAITOU As String = "2"

    ''' <summary>
    ''' 通知依頼情報．最終ステータス(キャンセル)
    ''' </summary>
    Private Const C_NOTICEREQSTATUS_CANCEL As String = "2"
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END


    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    'システム環境設定・販売店環境設置のKEY
    Private Const ENVSETTINGKEY_SA01_ENABLED As String = "USE_DMS_ACTIVITY_LINK"    '基幹連携(TMTのみ、SA01)の使用可
    Private Const ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD As String = "MOST_PREFERRED_PROSPECT_CD" '希望者の商談見込み度コード

    ''' <summary>
    ''' 受注後工程機能の使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_USE_B2D_FUNCTION As String = "USE_B2D_FUNCTION"


    '入力必須チェック用定数
    Const INPUT_CHECK_TIMING As String = "05"
    Const CHECKITEM_ID_CST_FIRASTNAME As String = "01"
    Const ERRMSG_ID_CST_FIRSTNAME As Integer = 40902
    Const CHECKITEM_ID_CST_MIDDLENAME As String = "02"
    Const ERRMSG_ID_CST_MIDDLENAME As Integer = 40949
    Const CHECKITEM_ID_CST_LASTNAME As String = "03"
    Const ERRMSG_ID_CST_LASTNAME As Integer = 40950
    Const CHECKITEM_ID_CST_GENDER As String = "04"
    Const ERRMSG_ID_CST_GENDER As Integer = 40951
    Const CHECKITEM_ID_CST_NAMETITLE As String = "05"
    Const ERRMSG_ID_CST_NAMETITLE As Integer = 40952
    Const CHECKITEM_ID_CST_FLEET As String = "06"
    Const ERRMSG_ID_CST_FLEET As Integer = 40953
    Const CHECKITEM_ID_CST_FLEETDETAIL As String = "07"
    Const ERRMSG_ID_CST_FLEETDETAIL As Integer = 40954
    Const CHECKITEM_ID_CST_FLEET_PIC_NAME As String = "08"
    Const ERRMSG_ID_CST_FLEET_PIC_NAME As Integer = 40955
    Const CHECKITEM_ID_CST_FLEET_PIC_DEPT As String = "09"
    Const ERRMSG_ID_CST_FLEET_PIC_DEPT As Integer = 40956
    Const CHECKITEM_ID_CST_FLEET_PIC_POSITION As String = "10"
    Const ERRMSG_ID_CST_FLEET_PIC_POSITION As Integer = 40957
    Const ERRMSG_ID_CST_TEL As Integer = 30934
    Const CHECKITEM_ID_CST_MOBILE As String = "11"
    Const ERRMSG_ID_CST_MOBILE As Integer = 40907
    Const CHECKITEM_ID_CST_PHONE As String = "12"
    Const ERRMSG_ID_CST_PHONE As Integer = 40907
    Const CHECKITEM_ID_CST_BIZ_PHONE As String = "13"
    Const ERRMSG_ID_CST_BIZ_PHONE As Integer = 40959
    Const CHECKITEM_ID_CST_FAX As String = "14"
    Const ERRMSG_ID_CST_FAX As Integer = 40958
    Const CHECKITEM_ID_CST_ZIPCD As String = "15"
    Const ERRMSG_ID_CST_ZIPCD As Integer = 40960
    Const CHECKITEM_ID_CST_ADDRESS_1 As String = "16"
    Const ERRMSG_ID_CST_ADDRESS_1 As Integer = 40961
    Const CHECKITEM_ID_CST_ADDRESS_2 As String = "17"
    Const ERRMSG_ID_CST_ADDRESS_2 As Integer = 40962
    Const CHECKITEM_ID_CST_ADDRESS_3 As String = "18"
    Const ERRMSG_ID_CST_ADDRESS_3 As Integer = 40963
    Const CHECKITEM_ID_CST_ADDRESS_STATE As String = "19"
    Const ERRMSG_ID_CST_ADDRESS_STATE As Integer = 40964
    Const CHECKITEM_ID_CST_ADDRESS_DISTRICT As String = "20"
    Const ERRMSG_ID_CST_ADDRESS_DISTRICT As Integer = 40965
    Const CHECKITEM_ID_CST_ADDRESS_CITY As String = "21"
    Const ERRMSG_ID_CST_ADDRESS_CITY As Integer = 40966
    Const CHECKITEM_ID_CST_ADDRESS_LOCATION As String = "22"
    Const ERRMSG_ID_CST_ADDRESS_LOCATION As Integer = 40967
    Const CHECKITEM_ID_CST_DOMICILE As String = "23"
    Const ERRMSG_ID_CST_DOMICILE As Integer = 40968
    Const CHECKITEM_ID_CST_EMAIL_1 As String = "24"
    Const ERRMSG_ID_CST_EMAIL_1 As Integer = 40969
    Const CHECKITEM_ID_CST_EMAIL_2 As String = "25"
    Const ERRMSG_ID_CST_EMAIL_2 As Integer = 40970
    Const CHECKITEM_ID_CST_COUNTRY As String = "26"
    Const ERRMSG_ID_CST_COUNTRY As Integer = 40971
    Const CHECKITEM_ID_CST_SOCIALNUM As String = "27"
    Const ERRMSG_ID_CST_SOCIALNUM As Integer = 40972
    Const CHECKITEM_ID_CST_BIRTH_DATE As String = "28"
    Const ERRMSG_ID_CST_BIRTH_DATE As Integer = 40973
    Const CHECKITEM_ID_ACT_CAT_TYPE As String = "29"
    Const ERRMSG_ID_ACT_CAT_TYPE As Integer = 40974
    Const CHECKITEM_ID_PREFER_VCL As String = "30"
    Const ERRMSG_ID_PREFER_VCL As Integer = 20901
    Const CHECKITEM_ID_PREFER_VCL_MODEL As String = "31"
    Const ERRMSG_ID_PREFER_VCL_MODEL As Integer = 20912
    Const CHECKITEM_ID_PREFER_VCL_BODYCLR As String = "32"
    Const ERRMSG_ID_PREFER_VCL_BODYCLR As Integer = 20913
    Const CHECKITEM_ID_SOURCE_1_CD As String = "33"
    Const ERRMSG_ID_SOURCE_1_CD As Integer = 20914
    Const CHECKITEM_ID_BRAND_RECOGNITION_ID As String = "34"
    Const ERRMSG_ID_BRAND_RECOGNITION_ID As Integer = 20915
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    Const CHECKITEM_ID_SALESCONDITION As String = "35"
    Const ERRMSG_ID_SALESCONDITION As Integer = 20916
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
    Const ERRMSG_ID_CST_ORGNZ_NAME As Integer = 4000901
    Const ERRMSG_ID_CST_SUBCAT2_CD As Integer = 4000902
    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
    Const ERRMSG_ID_SOURCE_2_CD As Integer = 2020918
    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END

    ''' <summary>
    ''' 顧客組織入力区分 (1：マスタから選択)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORG_INPUT_TYPE_MASTER As String = "1"
    ''' <summary>
    ''' 顧客組織入力区分 (2：手入力)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORG_INPUT_TYPE_MANUAL As String = "2"
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    'システム環境設定.受注後工程コード(その他)
    Private Const ENVSETTINGKEY_AFTER_ODR_PRCS_OTHER As String = "AFTER_ODR_PRCS_OTHER"
    '2014/02/12 TCS 山口 受注後フォロー機能開発 End

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）START
    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORGCUSTFLG As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCUSTFLG As String = "2"
    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）END
#End Region

#Region "メソット"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    '現在未使用で、CA1811のコード分析エラーが発生する為、削除
    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END


    ''' <summary>
    ''' 担当SC一覧取得
    ''' </summary>
    ''' <returns>担当SC一覧データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetUsers() As ActivityInfoDataSet.ActivityInfoUsersDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetUsers(context.DlrCD, context.BrnCD)

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' シリーズ単位の希望車種取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>シリーズ単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwSeries(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwSeriesDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        Dim context As StaffContext = StaffContext.Current
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Return (ActivityInfoTableAdapter.GetFllwSeries(context.DlrCD, fllwupboxseqno))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' グレード単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>グレード単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwModel(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwModelDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 End

        Dim context As StaffContext = StaffContext.Current
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Return ActivityInfoTableAdapter.GetFllwModel(context.DlrCD, fllwupboxseqno)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 End
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' カラー単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>カラー単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwColor(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwColorDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 End

        Dim context As StaffContext = StaffContext.Current
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Return ActivityInfoTableAdapter.GetFllwColor(context.DlrCD, fllwupboxseqno)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 End
    End Function


    ''' <summary>
    ''' 活動方法取得
    ''' </summary>
    ''' <param name="bookedafterflg">受注後フラグ (指定がなければ全件検索)</param>
    ''' <returns>活動方法データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetActContact(ByVal bookedafterflg As String) As ActivityInfoDataSet.ActivityInfoActContactDataTable

        Return ActivityInfoTableAdapter.GetActContact(bookedafterflg)

    End Function


    ''' <summary>
    ''' 文言取得
    ''' </summary>
    ''' <param name="serchdt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentWord(ByVal serchdt As ActivityInfoDataSet.ActivityInfoSeqDataTable) As ActivityInfoDataSet.ActivityInfoContentWordDataTable

        Dim Serchrw As ActivityInfoDataSet.ActivityInfoSeqRow
        Serchrw = CType(serchdt.Rows(0), ActivityInfoDataSet.ActivityInfoSeqRow)
        Return ActivityInfoTableAdapter.GetContentWord(Serchrw.SEQNO)

    End Function

    ''' <summary>
    ''' 日付フォーマット取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetDateFormat() As ActivityInfoDataSet.ActivityInfoDateFormatDataTable

        Return ActivityInfoTableAdapter.GetDateFormat()

    End Function

    ''' <summary>
    ''' アイコンのパス取得
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentIconPath(ByVal seqno As Integer) As ActivityInfoDataSet.ActivityInfoContentIconPathDataTable

        Return ActivityInfoTableAdapter.GetContentIconPath(seqno)

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 新規登録処理
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function InsertActivityData(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertActivityData Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        registRw = CType(registdt.Rows(0), ActivityInfoRegistDataRow)
        Dim sysEnv As New SystemEnvSetting

        '変数の宣言
        'ログイン情報系
        '活動ID
        Dim actid As Decimal = registRw.ACTID
        '用件ID
        Dim reqid As Decimal = registRw.REQID
        '商談ID
        Dim salesid As Decimal = registRw.SALESID
        '誘致ID
        Dim attid As Decimal = registRw.ATTID
        '自身の店舗コード
        Dim dlrcd As String = context.DlrCD
        '自身の販売店コード
        Dim brncd As String = context.BrnCD
        '画面入力内容（活動内容の活動日時(From)）
        Dim actdayfrom As String = registRw.ACTDAYFROM
        Dim actDayFromDate As Date = Date.ParseExact(actdayfrom, "yyyy/MM/dd HH:mm", Nothing)
        '画面入力内容（活動内容の活動日時(To)）
        Dim actdayto As String = registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        '活動回数
        Dim count As Long = registRw.ACTCOUNT + 1
        '予定スタッフコード
        Dim staffcdplan As String = context.Account
        '予定コンタクト方法 画面入力内容(予定のコンタクト方法のコード)
        Dim schecontactmtd As String = registRw.NEXTACTCONTACT
        '日付省略値
        Dim defultDate As Date = Date.ParseExact("1900/01/01 00:00:00", "yyyy/MM/dd HH:mm:ss", Nothing)
        '活動結果ID
        Dim actresult As String = registRw.ACTRESULT
        Dim rsltid As String
        '機能識別子
        Dim rowfunction As String = SC3080216_MODULEID
        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_GIVEUP_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
        End If
        '行更新(作成)アカウント
        Dim account As String = context.Account
        'ステータス
        If actid <> 0 Then
            Try
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                ActivityInfoTableAdapter.GetFollowupSalesLock(salesid)
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Catch ex As Exception
                Return False
            End Try

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        Else
            '新規活動の場合
            Try
                '商談一時情報ロック
                If ActivityInfoTableAdapter.LockSalesTemp(salesid) <> 1 Then Return False
            Catch ex As Exception
                InsertActivityData = False
                Throw ex
            End Try
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        End If
        '未存在の希望車種を登録
        ActivityInfoBusinessLogic.InsertSelectedSeries(registRw)

        If actid = 0 Then
            ActivityInfoBusinessLogic.InsertNewRequest(registRw)
        End If

        '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
        '活動結果登録時に【用件ソース変更可能フラグ】を固定で0に更新(処理結果は問わない)
        ActivityInfoTableAdapter.UpdateSourceChgPossibleFlg(salesid, account, rowfunction)
        '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END

        '商談活動追加
        If (Not ActivityInfoBusinessLogic.InsertSalesAct(registRw)) Then
            Return False
        End If
        reqid = registRw.REQID
        attid = registRw.ATTID

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        If Not String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) _
           AndAlso Not String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then

            actid = ActivityInfoTableAdapter.GetSqActId()
            Dim schedatetime As Date = Date.ParseExact(registRw.NEXTACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
            Dim walkinschestart As Date = Date.ParseExact(registRw.NEXTACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
            Dim walkinscheend As Date = Date.ParseExact(registRw.NEXTACTDAYFROM.Substring(0, 10) & " " & registRw.NEXTACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            '活動(予定)追加
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
            Dim orgnzidplan As Decimal
            orgnzidplan = ActivityInfoTableAdapter.GetorgnzId(staffcdplan)
            Dim orgnzid As Decimal
            orgnzid = ActivityInfoTableAdapter.GetorgnzId(account)
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End
            ActivityInfoTableAdapter.InsertActivity(actid, reqid, attid, count, schedatetime, walkinschestart, walkinscheend, _
                                                     dlrcd, brncd, staffcdplan, schecontactmtd, "0", defultDate, " ", " ", " ", _
                                                    " ", " ", rsltid, account, rowfunction, " ", orgnzid, orgnzidplan)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End If

        'Follow-up Box商談の更新
        UpdateFllwupboxSales(registRw) 'SA01連携にて必要なため、事前更新する(※後半の再更新への影響は無い)

        'SA01連携(TMT向け機能)（※次回活動予定登録後、History移動前に呼び出す、FllwupboxSales更新後に呼び出す）
        If Not SyncDmsSA01(salesid, StaffContext.Current.Account) Then
            Logger.Error("ActivityInfoBusinessLogic.InsertActivityData - Internal Error: SyncDmsSA01 (SALES_ID:" & salesid & ")")
            Return False
        End If

        If String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) _
            Or String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            If reqid <> 0 Then
                ActivityInfoBusinessLogic.MoveHistory(True, reqid, salesid)
            Else
                If ActivityInfoTableAdapter.AttractStatusCheck(attid) = 0 Then
                    ActivityInfoBusinessLogic.MoveHistory(False, attid, salesid)
                End If
            End If
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '一発Success、Give-up時に2回プロセス実績が登録されないように空にする
        registRw.SELECTACTCATALOG = ""
        registRw.SELECTACTTESTDRIVE = ""
        registRw.SELECTACTVALUATION = ""
        registRw.SELECTACTASSESMENT = ""
        '査定実績フラグ更新
        ActivityInfoBusinessLogic.UpdateActAsmFlg(dlrcd, brncd, salesid, account, rowfunction)
        '見積実績フラグ更新
        ActivityInfoBusinessLogic.UpdateActEstFlg(dlrcd, brncd, salesid, account, rowfunction)

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertActivityData End")
        '-----------------------------------------------------------------
        Return True

    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    ''' 2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動新規登録
    ''' </summary>
    ''' <param name="registRw">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function InsertNewRequest(ByVal registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNewRequest Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        '変数の宣言
        'ログイン情報系
        '顧客ID
        Dim cstid As Decimal = registRw.CSTID
        '車両ID
        Dim vclid As Decimal = registRw.VCLID
        '用件ソース(1st）コード
        sysEnvRow = sysEnv.GetSystemEnvSetting(USED_FLG_SOURCE1EDIT)
        Dim source1cd As String = sysEnvRow.PARAMVALUE
        '用件ステータス と　活動ステータス
        Dim cractrslt As String = " "
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If
        '画面入力内容（活動内容の活動日時(From)）
        Dim actdayfrom As String = registRw.ACTDAYFROM
        Dim actDayFromDate As Date = Date.ParseExact(actdayfrom, "yyyy/MM/dd HH:mm", Nothing)
        '画面入力内容（活動内容の活動日時(To)）
        Dim actdayto As String = registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        '販売店コード
        Dim dlrcd As String = context.DlrCD
        '店舗コード
        Dim brncd As String = context.BrnCD
        '受付スタッフコード
        Dim staffcd As String = context.Account
        'コンタクト方法
        Dim actmtd As String = registRw.ACTCONTACT
        '行更新(作成)アカウント
        Dim account As String = context.Account
        '機能識別子
        Dim rowfunction As String = SC3080216_MODULEID
        '活動結果ID
        Dim actresult As String = registRw.ACTRESULT
        Dim rsltid As String
        '活動回数
        Dim count As Long = registRw.ACTCOUNT + 1
        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = registRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
        End If
        '商談見込み度コード
        Dim prospectcd As String = " "
        If String.Equals(registRw.ACTRESULT, C_RSLT_HOT) Then
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_PROSPECT) Then
            prospectcd = CRACTSTATUS_PROSPECT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_WALKIN) Then
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        End If
        '商談完了フラグ
        Dim compflg As String = "0"
        If String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            compflg = "1"
        Else
            compflg = "0"
        End If
        '断念競合車種連番
        Dim giveupvclseq As String = registRw.GIVEUPVCLSEQ
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim giveupreason As String = " "
        If Not String.IsNullOrEmpty(registRw.GIVEUPREASON) Then
            giveupreason = registRw.GIVEUPREASON
        End If
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        '日付省略値
        Dim defultDate As Date = Date.ParseExact("1900/01/01 00:00:00", "yyyy/MM/dd HH:mm:ss", Nothing)
        '用件IDシーケンス取得
        Dim reqid As Decimal
        reqid = ActivityInfoTableAdapter.GetSqReqId()
        '活動IDシーケンス取得
        Dim actid As Decimal
        actid = ActivityInfoTableAdapter.GetSqActId()
        '商談ID
        Dim salesid As Decimal = registRw.SALESID
        Dim walkinschestart As Date = Date.ParseExact(registRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        Dim walkinscheend As Date = Date.ParseExact(registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
        Dim orgnzidplan As Decimal
        orgnzidplan = ActivityInfoTableAdapter.GetorgnzId(staffcd)
        Dim orgnzid As Decimal
        orgnzid = ActivityInfoTableAdapter.GetorgnzId(account)
        ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End

        '用件追加
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        ActivityInfoTableAdapter.InsertRequest(reqid, cstid, vclid, "1", source1cd, cractrslt, actDayToDate, rsltid, actid, _
                                                actDayFromDate, actDayFromDate, dlrcd, brncd, staffcd, actmtd, actid, account, rowfunction, salesid, orgnzid)
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Elnd

        '活動追加
        ActivityInfoTableAdapter.InsertActivity(actid, reqid, 0, count, actDayFromDate, walkinschestart, walkinscheend, _
                                                 dlrcd, brncd, staffcd, actmtd, "1", walkinschestart, dlrcd, brncd, staffcd, _
                                                 actmtd, cractrslt, rsltid, account, rowfunction, prospectcd, orgnzid, orgnzidplan)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '商談追加
        ActivityInfoTableAdapter.InsertSales(salesid, dlrcd, brncd, cstid, prospectcd, reqid, compflg, giveupvclseq, giveupreason, _
                                             account, rowfunction, actid)

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '商談一時情報削除
        If ActivityInfoTableAdapter.MoveSalesTemp(salesid) Then
            If Not ActivityInfoTableAdapter.DeleteSalesTemp(salesid) Then Return False
        Else
            Return False
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        registRw.ACTID = actid
        registRw.REQID = reqid
        registRw.ATTID = 0

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNewRequest End")
        '-----------------------------------------------------------------

        Return True
    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    ''' 2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談活動追加
    ''' </summary>
    ''' <param name="registRw">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function InsertSalesAct(ByVal registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesAct Start")
        '-----------------------------------------------------------------

        Dim context As StaffContext = StaffContext.Current
        '販売店コード
        Dim dlrcd As String = context.DlrCD
        '店舗コード
        Dim strcd As String = context.BrnCD
        '活動ID
        Dim actid As Decimal = registRw.ACTID
        '商談ID
        Dim salesid As Decimal = registRw.SALESID
        '商談見込み度コード
        Dim prospectcd As String
        If String.Equals(registRw.ACTRESULT, C_RSLT_HOT) Then
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_PROSPECT) Then
            prospectcd = CRACTSTATUS_PROSPECT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_WALKIN) Then
            prospectcd = CRACTSTATUS_WALKIN
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            prospectcd = CRACTRESULT_SUCCESS
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            prospectcd = CRACTRESULT_GIVEUP
        End If
        '行更新(作成)アカウント
        Dim account As String = registRw.ACTACCOUNT & "@" & context.DlrCD
        '保持情報（スタッフコード）
        Dim staffcd As String = context.Account
        '画面入力内容(活動内容のコンタクト方法のコード)
        Dim actmtd As String = registRw.ACTCONTACT
        '車両ID
        Dim vclid As Decimal = registRw.VCLID
        '機能識別子
        Dim rowfunction As String = SC3080216_MODULEID
        '活動先顧客コード
        Dim crcstid As Decimal = registRw.CRCUSTID
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim catalog As String = ""
        Dim testdrive As String = ""
        Dim assessment As String = ""
        Dim valuation As String = ""
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim lockversion As String = ""
        Dim valuationPrice As String = ""
        '配列作成
        Dim wkary As String()
        Dim tempary As String()
        Dim seqdt As ActivityInfoSeqDataTable
        Dim seqrw As ActivityInfoSeqRow
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim selockversion As String = ""
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        seqdt = ActivityInfoTableAdapter.GetActHisCarSeq(salesid)
        For j As Integer = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
            selcar = selcar & seqrw.SEQNO & ","
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            selockversion = selockversion & seqrw.LOCKVERSION & ","
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        Next

        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        Dim orgnzid As Decimal
        orgnzid = ActivityInfoTableAdapter.GetorgnzId(staffcd)
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End


        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        wkary = registRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(salesid, tempary(0), "1")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                    catalog = catalog & seqrw.SEQNO & ","
                Next
            End If
        Next

        '試乗実績がある希望車種のSEQのリストを作成
        testdrive = ""
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        wkary = registRw.SELECTACTTESTDRIVE.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(salesid, tempary(0), "2")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                    testdrive = testdrive & seqrw.SEQNO & ","
                Next
            End If
        Next
        '見積り実績(見積りは全部に対して有り、無しの2択)
        assessment = registRw.SELECTACTASSESMENT
        Dim assessmentNo As Long = 0    '査定No
        'Toは時分しか持っていないためFrom側から日付をセット
        Dim actdayto As String = registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        Dim actdate As Date                    '活動日(画面で入力した値)
        actdate = actDayToDate
        Dim actdateDt As Date = actDayToDate
        '査定実績(査定は全希望車種に対して同じ区分を適用)
        assessment = GetRegActAsmStatus(registRw.SELECTACTASSESMENT, dlrcd, strcd, salesid, assessmentNo)

        '見積り実績がある希望車種のSEQのリストを作成
        valuation = ""
        lockversion = ""
        valuationPrice = ""
        wkary = registRw.SELECTACTVALUATION.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(salesid, tempary(0), "4")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                    lockversion = lockversion & seqrw.LOCKVERSION & ","
                Next
            End If
            valuationPrice = valuationPrice & tempary(2) & ","
        Next
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        'カタログのマスタ情報取得
        Dim ActHisCatalogdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisCatalogrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisCatalogdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_CATALOG)
        ActHisCatalogrw = CType(ActHisCatalogdt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisCatalogCategorydvsid As Nullable(Of Long)
        If ActHisCatalogrw.IsCATEGORYDVSIDNull Then
            ActHisCatalogCategorydvsid = Nothing
        Else
            ActHisCatalogCategorydvsid = ActHisCatalogrw.CATEGORYDVSID
        End If

        '試乗のマスタ情報取得()
        Dim ActHisTestDrivedt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisTestDriverw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisTestDrivedt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_TESTDRIVE)
        ActHisTestDriverw = CType(ActHisTestDrivedt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisTestDriveCategorydvsid As Nullable(Of Long)
        If ActHisTestDriverw.IsCATEGORYDVSIDNull Then
            ActHisTestDriveCategorydvsid = Nothing
        Else
            ActHisTestDriveCategorydvsid = ActHisTestDriverw.CATEGORYDVSID
        End If

        '査定のマスタ情報取得
        Dim ActHisAssessmentdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisAssessmentrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisAssessmentdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_ASSESSMENT)
        ActHisAssessmentrw = CType(ActHisAssessmentdt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisAssessmentCategorydvsid As Nullable(Of Long)
        If ActHisAssessmentrw.IsCATEGORYDVSIDNull Then
            ActHisAssessmentCategorydvsid = Nothing
        Else
            ActHisAssessmentCategorydvsid = ActHisAssessmentrw.CATEGORYDVSID
        End If

        '見積りのマスタ情報取得
        Dim ActHisValuationdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisValuationrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisValuationdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_VALUATION)
        ActHisValuationrw = CType(ActHisValuationdt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisValuationCategorydvsid As Nullable(Of Long)
        If ActHisValuationrw.IsCATEGORYDVSIDNull Then
            ActHisValuationCategorydvsid = Nothing
        Else
            ActHisValuationCategorydvsid = ActHisValuationrw.CATEGORYDVSID
        End If


        Dim selcarary As String() = selcar.Split(","c)
        Dim catalogary As String() = catalog.Split(","c)
        Dim testdriveary As String() = testdrive.Split(","c)
        Dim valuationary As String() = valuation.Split(","c)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim valuationaryPrice As String() = valuationPrice.Split(","c)
        Dim valVersion As String() = lockversion.Split(","c)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim seVersion As String() = selockversion.Split(","c)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        Dim actHisSelCardt As ActivityInfoDataSet.ActivityInfoActHisSelCarDataTable
        Dim actHisSelCarrw As ActivityInfoDataSet.ActivityInfoActHisSelCarRow
        Dim actHisFllwdt As ActivityInfoDataSet.ActivityInfoActHisFllwDataTable
        Dim actHisFllwrw As ActivityInfoDataSet.ActivityInfoActHisFllwRow = Nothing
        actHisFllwdt = ActivityInfoTableAdapter.GetActHisFllw(salesid, dlrcd)
        Dim actHisFllwCrplan_id As Nullable(Of Long)
        Dim actHisFllwPromotion_id As Nullable(Of Long)
        If actHisFllwdt.Rows.Count > 0 Then
            actHisFllwrw = CType(actHisFllwdt.Rows(0), ActivityInfoActHisFllwRow)

            If actHisFllwrw.IsCRPLAN_IDNull Then
                actHisFllwCrplan_id = Nothing
            Else
                actHisFllwCrplan_id = actHisFllwrw.CRPLAN_ID
            End If

            If actHisFllwrw.IsPROMOTION_IDNull Then
                actHisFllwPromotion_id = Nothing
            Else
                actHisFllwPromotion_id = actHisFllwrw.PROMOTION_ID
            End If

        End If

        If Not String.IsNullOrEmpty(catalogary(0)) Then
            ActivityInfoTableAdapter.DeleteBrochure(salesid)
        End If

        '希望車種に対する活動実績を入力する
        Dim rsltsalescat As String = " "
        Dim createctactresult As String = " "
        If String.Equals(registRw.ACTRESULT, C_RSLT_WALKIN) Then
            createctactresult = CRACTSTATUS_WALKIN
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_PROSPECT) Then
            createctactresult = CRACTSTATUS_PROSPECT
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_HOT) Then
            createctactresult = CRACTSTATUS_HOT
        End If

        '査定処理フラグ
        Dim sateflg As Boolean = True
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        '希望車種の台数分ループ
        For i = 0 To selcarary.Length - 2
            '希望車種の情報取得
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            actHisSelCardt = ActivityInfoTableAdapter.GetActHisCarSeq(dlrcd, salesid, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()))
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            Dim cractrslt As String = " "
            Dim lock_ver As Long = CLng(seVersion(i))
            Dim req As Integer
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            If actHisSelCardt.Rows.Count > 0 Then
                actHisSelCarrw = CType(actHisSelCardt.Rows(0), ActivityInfoActHisSelCarRow)
                Dim actHisSelCardisp_Bdy_Color As String = " "
                If actHisSelCarrw.IsDISP_BDY_COLORNull Then
                    actHisSelCardisp_Bdy_Color = Nothing
                Else
                    actHisSelCardisp_Bdy_Color = actHisSelCarrw.DISP_BDY_COLOR
                End If

                '査定モデル名
                Dim modelname As String = " "
                If Not actHisSelCarrw.IsVCLMODEL_NAMENull Then
                    modelname = actHisSelCarrw.VCLMODEL_NAME
                End If
                'モデルコード
                Dim modelcd As String = " "
                If Not String.IsNullOrEmpty(actHisSelCarrw.SERIESNM) Then
                    modelcd = actHisSelCarrw.SERIESNM
                End If
                'カタログ実績確認
                For j = 0 To catalogary.Length - 2
                    If selcarary(i) = catalogary(j) Then
                        rsltsalescat = "2"
                        Dim salesactid As Decimal = ActivityInfoTableAdapter.GetSqSalesActId()
                        ActivityInfoTableAdapter.InsertSalesActivity(salesactid, salesid, actid, rsltsalescat, createctactresult, _
                                                                         modelcd, " ", account, rowfunction)
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                        ActivityInfoTableAdapter.InsertBrochure(salesid, modelcd, actdate, staffcd, actmtd, _
                                                               account, rowfunction, orgnzid)
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                    End If
                Next
                '試乗実績確認
                For j = 0 To testdriveary.Length - 2
                    If selcarary(i) = testdriveary(j) Then
                        rsltsalescat = "4"
                        Dim salesactid As Decimal = ActivityInfoTableAdapter.GetSqSalesActId()
                        ActivityInfoTableAdapter.InsertSalesActivity(salesactid, salesid, actid, rsltsalescat, createctactresult, _
                                                                         modelcd, modelname, account, rowfunction)
                        '試乗予約ID取得
                        Dim testdriveid As Decimal = ActivityInfoTableAdapter.GetReqTestDriveId()
                        Dim actDayfromDate As Date = Date.ParseExact(registRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                        '試乗予約追加
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                        ActivityInfoTableAdapter.InsertTestDrive(testdriveid, dlrcd, strcd,
                                                              modelcd, modelname, registRw.CRCUSTID, salesid,
                                                              actdate, actDayfromDate, actdate,
                                                              staffcd, account, rowfunction, orgnzid)
                        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                    End If
                Next
                '査定実績確認
                If String.Equals(assessment, "1") Then
                    rsltsalescat = "7"
                    '査定情報取得
                    Dim actasminfo As ActivityInfoDataSet.ActAsmInfoDataTable
                    Dim actasmrow As ActivityInfoDataSet.ActAsmInfoRow
                    Dim vclname As String = " "
                    Dim iCount As Integer
                    Dim asmseq As Long

                    '査定処理フラグはTRUEの場合のみ実施
                    If sateflg Then
                        actasminfo = ActivityInfoTableAdapter.GetActAsmInfo(salesid)
                        If actasminfo.Rows.Count > 0 Then
                            actasmrow = CType(actasminfo.Rows(0), ActivityInfoDataSet.ActAsmInfoRow)
                            asmseq = actasmrow.ASSMNTSEQ
                            For iCount = 0 To actasminfo.Rows.Count - 1
                                actasmrow = CType(actasminfo.Rows(iCount), ActivityInfoDataSet.ActAsmInfoRow)

                                '査定実績登録
                                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                                ActivityInfoTableAdapter.SetAssessmentAct(salesid, asmseq + iCount,
                                                                               actasmrow.VEHICLENAME, staffcd,
                                                                               actasmrow.APPRISAL_PRICE,
                                                                               account, rowfunction, orgnzid)
                                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                                vclname = actasmrow.VEHICLENAME

                                '商談活動IDシーケンス取得
                                Dim salesactid As Decimal = ActivityInfoTableAdapter.GetSqSalesActId()
                                '商談活動追加
                                ActivityInfoTableAdapter.InsertSalesActivity(salesactid, salesid, actid, rsltsalescat, createctactresult, _
                                                                                 modelcd, vclname, account, rowfunction)
                            Next
                        Else
                            '商談活動IDシーケンス取得
                            Dim salesactid As Decimal = ActivityInfoTableAdapter.GetSqSalesActId()
                            '商談活動追加
                            ActivityInfoTableAdapter.InsertSalesActivity(salesactid, salesid, actid, rsltsalescat, createctactresult, _
                                                                             modelcd, vclname, account, rowfunction)
                        End If
                    End If
                End If

                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
                If Not UpdateEstimateAmount(salesid, registRw.SALESVERSION, selcarary(i), actmtd, staffcd, account, rowfunction, lock_ver, orgnzid, actid, createctactresult, modelcd, modelname) Then
                    Return False
                End If

                '査定依頼機能を使用している場合、且つ査定実績がある場合、且つ査定依頼未回答の場合
                If String.Equals(assessment, C_ASMACTSTATUS_MIKAITOU) Then
                    Dim bfafdvs As String = " "
                    If Not actHisFllwrw Is Nothing Then
                        bfafdvs = actHisFllwrw.BFAFDVS
                    End If
                    ActivityInfoTableAdapter.InsertFllwupBoxCrHisAsm(assessmentNo, dlrcd, strcd, salesid, actid, actHisFllwCrplan_id, bfafdvs, _
                                                                        actHisFllwrw.CRDVSID, actHisFllwrw.INSDID, actHisFllwrw.SERIESCODE, actHisFllwrw.SERIESNAME, _
                                                                        account, actHisFllwrw.VCLREGNO, actHisFllwrw.SUBCTGCODE, actHisFllwrw.SERVICECD, _
                                                                        actHisFllwrw.SUBCTGORGNAME, actHisFllwrw.SUBCTGORGNAME_EX, actHisFllwPromotion_id, _
                                                                        actHisFllwrw.CRACTRESULT, actHisFllwrw.PLANDVS, actdateDt, ActHisAssessmentrw.METHOD, _
                                                                        ActHisAssessmentrw.ACTION, ActHisAssessmentrw.ACTIONTYPE, actHisFllwrw.ACCOUNT_PLAN, _
                                                                        ActHisAssessmentrw.ACTIONCD, CONTENT_SEQ_ASSESSMENT, _
                                                                        Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), actHisSelCarrw.SERIESNM, _
                                                                        modelname, actHisSelCardisp_Bdy_Color, actHisSelCarrw.QUANTITY, salesid, _
                                                                        ActHisAssessmentrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()), _
                                                                        ActHisAssessmentCategorydvsid, actHisFllwrw.VIN, actHisFllwrw.CUSTCHRGSTAFFNM, _
                                                                        actHisFllwrw.CRCUSTID, actHisFllwrw.CUSTOMERCLASS)
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                If String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
                    Dim ContractFlag As Integer = ActivityInfoTableAdapter.GetEstimateContractFlg(salesid, selcarary(i))
                    '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
                    Dim salesbkgnum As String
                    salesbkgnum = " "
                    '見積状況フラグが立っている場合
                    If (ContractFlag = 1) Then
                        cractrslt = CRACTRESULT_SUCCESS
                        '見積の成約フラグを更新
                        ActivityInfoTableAdapter.GetEstimateLock(salesid, selcarary(i))
                        req = ActivityInfoTableAdapter.UpdateSuccessFlag(salesid, selcarary(i), account, rowfunction)
                        salesbkgnum = ActivityInfoTableAdapter.GetSalesbkgNum(salesid, selcarary(i))
                        If String.IsNullOrEmpty(salesbkgnum) Then
                            salesbkgnum = " "
                        End If
                    Else
                        cractrslt = CRACTRESULT_GIVEUP
                    End If
                    '希望車のステータスを更新
                    '2014/05/29 TCS 市川 TR-V4-GTMC140524001対応 START
                    req = ActivityInfoTableAdapter.UpdateSalesstatus(salesid, selcarary(i), cractrslt, account, rowfunction, lock_ver, actid, salesbkgnum)
                    '2014/05/29 TCS 市川 TR-V4-GTMC140524001対応 END
                    '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
                    If req = 0 Then
                        Return False
                    End If
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END
            End If
        Next

        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END

        'Follow-up Box商談メモ追加
        ActivityInfoTableAdapter.InsertFllwupboxSalesmemo(salesid, crcstid, vclid, actid, account)

        'Follow-up Box商談メモWK削除
        ActivityInfoTableAdapter.DeleteFllwupboxSalesmemowk(salesid)

        '一発Success、Give-up時に2回プロセス実績が登録されないように空にする
        registRw.SELECTACTCATALOG = ""
        registRw.SELECTACTTESTDRIVE = ""
        registRw.SELECTACTVALUATION = ""
        registRw.SELECTACTASSESMENT = ""

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesAct End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
    ''' <summary>
    ''' 見積結果更新
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="salesversion">商談行ロックバージョン</param>
    ''' <param name="pref_vcl_seq">希望車連番</param>
    ''' <param name="actmtd">活動コンタクトID</param>
    ''' <param name="staffcd">スタッフコード</param>
    ''' <param name="account">アカウントコード</param>
    ''' <param name="rowfunction">更新機能ID</param>
    ''' <param name="lock">希望車行ロックバージョン</param>
    ''' <param name="orgnzid">組織ID</param>
    ''' <param name="actid">活動ID</param>
    ''' <param name="createctactresult">実施後商談見込み度コード</param>
    ''' <param name="modelcd">モデルコード</param>
    ''' <param name="modelname">モデル名称</param>
    ''' <returns>True：正常終了/Flase：異常終了</returns>
    ''' <remarks></remarks>
    Shared Function UpdateEstimateAmount(ByVal salesid As Decimal, ByVal salesversion As Long, ByVal pref_vcl_seq As Integer, ByVal actmtd As String, ByVal staffcd As String,
                                         ByVal account As String, ByVal rowfunction As String, ByRef lock As Long, ByVal orgnzid As Decimal, ByVal actid As Decimal,
                                         ByVal createctactresult As String, ByVal modelcd As String, ByVal modelname As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateEstimateAmount Start")
        '-----------------------------------------------------------------

        '更新件数
        Dim cnt As Integer = 0

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim useFlgSuffix As String
        Dim useFlgInteriorClr As String

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            useFlgSuffix = "0"
        Else
            useFlgSuffix = dataRow.SETTING_VAL
        End If

        '内装色使用可否フラグ(設定値が無ければ0)
        Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRowclr = systemBiz.GetSystemSetting(USE_FLG_INTERIORCLR)

        If IsNothing(dataRowclr) Then
            useFlgInteriorClr = "0"
        Else
            useFlgInteriorClr = dataRowclr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '希望車種に紐付く見積情報が存在しかつ印刷済みか確認
        Dim estimateid As ULong
        estimateid = ActivityInfoTableAdapter.IsPrintedEstimate(salesid, pref_vcl_seq, useFlgSuffix, useFlgInteriorClr)

        If estimateid <> 0 Then
            '見積金額
            Dim estimateBiz As New IC3070201BusinessLogic()
            Dim est_amount As Double
            est_amount = estimateBiz.GetTotalPrice(estimateid, 0)
            est_amount = est_amount.ToString("0.00", Globalization.CultureInfo.InvariantCulture)

            'インスタンス解放
            estimateBiz = Nothing

            Try
                ActivityInfoTableAdapter.GetSalesLock(salesid, salesversion)
            Catch ex As Exception
                Return False
            End Try

            '見積実績を登録
            cnt = ActivityInfoTableAdapter.UpdateEstimateAmount(salesid, pref_vcl_seq, actmtd, est_amount, staffcd, account, rowfunction, lock, orgnzid)

            '更新件数が1件以外の場合、異常扱い
            If cnt <> 1 Then
                Return False
            End If

            lock = lock + 1

            Dim rsltsalesact As String = "6"
            Dim salesactid As Decimal = ActivityInfoTableAdapter.GetSqSalesActId()
            ActivityInfoTableAdapter.InsertSalesActivity(salesactid, salesid, actid, rsltsalesact, createctactresult, modelcd, modelname, account, rowfunction)

        End If

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateEstimateAmount End")
        '-----------------------------------------------------------------

        Return True

    End Function
    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END

    ''' 2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' History移動
    ''' </summary>
    ''' <param name="isrequest">用件判断</param>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function MoveHistory(ByVal isrequest As Boolean, ByVal reqid As Decimal, ByVal salesid As Decimal) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("MoveHistory Start")
        '-----------------------------------------------------------------
        Dim req As ActivityInfoDataSet.ActionidDataTable
        '用件の場合
        If isrequest Then
            ActivityInfoTableAdapter.MoveRequest(reqid)
            ActivityInfoTableAdapter.MoveActivity(reqid, 0)
            req = ActivityInfoTableAdapter.SelectActionID(reqid, 0)
        Else
            ActivityInfoTableAdapter.MoveAttract(reqid)
            ActivityInfoTableAdapter.MoveAttractCall(reqid)
            ActivityInfoTableAdapter.MoveAttractDM(reqid)
            ActivityInfoTableAdapter.MoveAttractRMM(reqid)
            ActivityInfoTableAdapter.MoveActivity(0, reqid)
            req = ActivityInfoTableAdapter.SelectActionID(0, reqid)
        End If

        Dim seqrw As ActionidRow
        For i = 0 To req.Rows.Count - 1
            seqrw = CType(req.Rows(i), ActionidRow)
            ActivityInfoTableAdapter.MoveActivityMemo(seqrw.ACT_ID)
        Next
        ActivityInfoTableAdapter.MoveSales(salesid)
        ActivityInfoTableAdapter.MoveSalesAct(salesid)
        ActivityInfoTableAdapter.MovePreferVcl(salesid)
        ActivityInfoTableAdapter.MoveCompetitorVcl(salesid)
        ActivityInfoTableAdapter.MoveBrochure(salesid)
        ActivityInfoTableAdapter.MoveTestDrive(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ActivityInfoTableAdapter.MoveAssessmentAct(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '用件の場合
        If isrequest Then
            ActivityInfoTableAdapter.DeleteRequest(reqid)
            ActivityInfoTableAdapter.DeleteActivity(reqid, 0)
        Else
            ActivityInfoTableAdapter.DeleteAttract(reqid)
            ActivityInfoTableAdapter.DeleteAttractCall(reqid)
            ActivityInfoTableAdapter.DeleteAttractDM(reqid)
            ActivityInfoTableAdapter.DeleteAttractRMM(reqid)
            ActivityInfoTableAdapter.DeleteActivity(0, reqid)
        End If

        For i = 0 To req.Rows.Count - 1
            seqrw = CType(req.Rows(i), ActionidRow)
            ActivityInfoTableAdapter.DeleteActivityMemo(seqrw.ACT_ID)
        Next
        ActivityInfoTableAdapter.DeleteSales(salesid)
        ActivityInfoTableAdapter.DeleteSalesAct(salesid)
        ActivityInfoTableAdapter.DeletePreferVcl(salesid)
        ActivityInfoTableAdapter.DeleteCompetitorVcl(salesid)
        ActivityInfoTableAdapter.DeleteBrochure(salesid)
        ActivityInfoTableAdapter.DeleteTestDrive(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ActivityInfoTableAdapter.DeleteAssessmentAct(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("MoveHistory End")
        '-----------------------------------------------------------------

        Return True
    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
    'Shared Function InsertHistory(ByVal dlrcd As String, ByVal fllwstrcd As String, ByVal fllwupboxseqno As Long,
    '                                 ByVal selcar As String, ByVal catalog As String, ByVal testdrive As String,
    '                                 ByVal assessment As String, ByVal valuation As String, ByVal account As String, ByVal actdate As String) As Boolean

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動結果更新処理
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdateActivityData(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable) As Boolean

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityData Start")
        '-----------------------------------------------------------------
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        registRw = CType(registdt.Rows(0), ActivityInfoRegistDataRow)
        Dim sysEnv As New SystemEnvSetting

        '変数の宣言
        'ログイン情報系
        '活動ID
        Dim actid As Decimal = registRw.ACTID
        '用件ID
        Dim reqid As Decimal = registRw.REQID
        '商談ID
        Dim salesid As Decimal = registRw.SALESID
        '誘致ID
        Dim attid As Decimal = registRw.ATTID
        '自身の店舗コード
        Dim dlrcd As String = context.DlrCD
        '自身の販売店コード
        Dim brncd As String = context.BrnCD
        '用件行ロックバージョン
        Dim rowlockversion As Long = registRw.REQUESTLOCKVERSION
        '商談行ロックバージョン
        Dim salesversion As Long = registRw.SALESVERSION
        '画面入力内容（活動内容の活動日時(From)）
        Dim actdayfrom As String = registRw.ACTDAYFROM
        Dim actDayFromDate As Date = Date.ParseExact(actdayfrom, "yyyy/MM/dd HH:mm", Nothing)
        '画面入力内容（活動内容の活動日時(From)）
        Dim actdayto As String = registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        '活動回数
        Dim count As Long = registRw.ACTCOUNT + 1
        '予定スタッフコード
        Dim staffcdplan As String = context.Account
        '予定コンタクト方法 画面入力内容(予定のコンタクト方法のコード)
        Dim schecontactmtd As String = registRw.NEXTACTCONTACT
        '日付省略値
        Dim defultDate As Date = Date.ParseExact("1900/01/01 00:00:00", "yyyy/MM/dd HH:mm:ss", Nothing)
        '活動結果ID
        Dim actresult As String = registRw.ACTRESULT
        Dim rsltid As String
        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = registRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
        Else
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
        End If
        '機能識別子
        Dim rowfunction As String = SC3080216_MODULEID
        '行更新(作成)アカウント
        Dim account As String = registRw.ACTACCOUNT & "@" & context.DlrCD
        'ステータス
        If actid <> 0 Then
            Try
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                If (reqid <> 0) Then
                    ActivityInfoTableAdapter.GetRequestLock(reqid, rowlockversion)
                Else
                    ActivityInfoTableAdapter.GetAttractLock(attid, rowlockversion)
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                ActivityInfoTableAdapter.GetSalesLock(salesid, salesversion)
                ActivityInfoTableAdapter.GetFollowupSalesLock(salesid)
            Catch ex As Exception
                Return False
            End Try
        End If
        '未存在の希望車種を登録
        ActivityInfoBusinessLogic.InsertSelectedSeries(registRw)

        If actid <> 0 Then
            If (reqid <> 0) Then
                If (Not ActivityInfoBusinessLogic.UpdateRequest(registRw)) Then
                    Return False
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            Else
                '誘致の場合
                If (Not ActivityInfoBusinessLogic.UpdateAttract(registRw)) Then
                    Return False
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            End If
        End If

        '商談活動追加
        ActivityInfoBusinessLogic.InsertSalesAct(registRw)

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        If Not String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) _
           AndAlso Not String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            actid = ActivityInfoTableAdapter.GetSqActId()
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            '活動(予定)追加
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
            Dim orgnzidplan As Decimal
            orgnzidplan = ActivityInfoTableAdapter.GetorgnzId(staffcdplan)
            Dim orgnzid As Decimal
            orgnzid = ActivityInfoTableAdapter.GetorgnzId(account)
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End
            ActivityInfoTableAdapter.InsertActivity(actid, reqid, attid, count, actDayFromDate, actDayFromDate, actDayToDate, _
                                                     dlrcd, brncd, staffcdplan, schecontactmtd, "0", defultDate, " ", " ", " ", _
                                                    " ", " ", rsltid, account, rowfunction, " ", orgnzid, orgnzidplan)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End If

        'Follow-up Box商談の更新
        UpdateFllwupboxSales(registRw) 'SA01連携にて必要なため、事前更新する(※後半の再更新への影響は無い)

        'SA01連携(TMT向け機能)（※次回活動予定登録後、History移動前に呼び出す、FllwupboxSales更新後に呼び出す）
        If Not SyncDmsSA01(salesid, StaffContext.Current.Account) Then
            Logger.Error("ActivityInfoBusinessLogic.UpdateActivityData - Internal Error: SyncDmsSA01 (SALES_ID:" & salesid & ")")
            Return False
        End If

        If String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Or String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            If reqid <> 0 Then
                ActivityInfoBusinessLogic.MoveHistory(True, reqid, salesid)
            Else
                If ActivityInfoTableAdapter.AttractStatusCheck(attid) = 0 Then
                    ActivityInfoBusinessLogic.MoveHistory(False, attid, salesid)
                End If
            End If
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '査定実績フラグ更新
        ActivityInfoBusinessLogic.UpdateActAsmFlg(dlrcd, brncd, salesid, account, rowfunction)
        '見積実績フラグ更新
        ActivityInfoBusinessLogic.UpdateActEstFlg(dlrcd, brncd, salesid, account, rowfunction)

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityData End")
        '-----------------------------------------------------------------

        Return True
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
    End Function

    ''' 2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 用件情報更新
    ''' </summary>
    ''' <param name="registRw">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdateRequest(ByVal registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateRequest Start")
        '-----------------------------------------------------------------

        Dim cractrslt As String = " "
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If

        Dim context As StaffContext = StaffContext.Current

        Dim fllwupboxseqno As Decimal = registRw.FLLWSEQ
        Dim completeflg As String
        If String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            completeflg = "1"
        Else
            completeflg = "0"
        End If

        Dim giveupvclseq As Long = registRw.GIVEUPVCLSEQ

        Dim account As String = context.Account
        Dim rowuodatefunction As String = SC3080216_MODULEID
        Dim requestlockversion As Long = registRw.REQUESTLOCKVERSION
        Dim saleslockversion As Long = registRw.SALESVERSION

        Dim rsltdate As Date
        If Not String.IsNullOrEmpty(registRw.ACTDAYFROM) Then
            rsltdate = Convert.ToDateTime(registRw.ACTDAYFROM)
        Else
            rsltdate = Convert.ToDateTime("1900/01/01 00:00")
        End If
        Dim rsltdatetime As Date = rsltdate
        Dim dlrcd As String = context.DlrCD
        Dim brncd As String = context.BrnCD
        Dim staffcd As String = context.Account
        Dim rsltcontactmthd As String = registRw.ACTCONTACT

        Dim sysEnv As New SystemEnvSetting
        Dim actresult As String = registRw.ACTRESULT
        Dim rsltid As String
        Dim prospectcd As String = " "
        'CR活動結果IDを取得
        '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = registRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_WALKIN
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_PROSPECT
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        End If

        Dim reqid As Decimal = registRw.REQID
        Dim thistimecractrslt As String = registRw.ACTRESULT
        Dim lastactdatetime As Date = Date.ParseExact(registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
        Dim count As Long = registRw.ACTCOUNT + 1
        Dim giveupreason As String = " "
        If Not String.IsNullOrEmpty(registRw.GIVEUPREASON) Then
            giveupreason = registRw.GIVEUPREASON
        End If

        '予定活動ID取得
        Dim dt As ActivityInfoDataSet.ActivityInfoGetScheDataDataTable = ActivityInfoTableAdapter.GetScheSqActId(reqid, 0)
        Dim GetScheDataRw As ActivityInfoDataSet.ActivityInfoGetScheDataRow = CType(dt.Rows(0), ActivityInfoGetScheDataRow)
        Dim actid As Decimal = GetScheDataRw.ACT_ID
        Dim actrockverrion As Long = GetScheDataRw.ROW_LOCK_VERSION
        registRw.ACTID = actid

        '要件更新
        If (0 = ActivityInfoTableAdapter.UpdateRequest(reqid, cractrslt, lastactdatetime, count,
                                                       rsltid, actid,
                                                       account, rowuodatefunction, requestlockversion)) Then
            Return False
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim orgnzid As Decimal
        orgnzid = ActivityInfoTableAdapter.GetorgnzId(staffcd)
        '活動(結果)更新
        If (0 = ActivityInfoTableAdapter.UpdateActivity(actid, rsltdate, rsltdatetime, dlrcd,
                                                       brncd, staffcd, rsltcontactmthd,
                                                       cractrslt, rsltid,
                                                       account, rowuodatefunction, actrockverrion, prospectcd, orgnzid)) Then
            Return False
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '商談更新
        If (0 = ActivityInfoTableAdapter.UpdateSales(fllwupboxseqno, prospectcd, completeflg, giveupvclseq, giveupreason,
                                                       account, rowuodatefunction, saleslockversion)) Then
            Return False
        End If
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateRequest End")
        '-----------------------------------------------------------------
        Return True
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
    End Function

    ''' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致情報更新
    ''' </summary>
    ''' <param name="registRw">データテーブル (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdateAttract(ByVal registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateAttract Start")
        '-----------------------------------------------------------------

        Dim cractrslt As String = " "
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If

        Dim context As StaffContext = StaffContext.Current

        Dim fllwupboxseqno As Decimal = registRw.FLLWSEQ
        Dim completeflg As String
        If String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
            completeflg = "1"
        Else
            completeflg = "0"
        End If

        Dim giveupvclseq As Long = registRw.GIVEUPVCLSEQ

        Dim account As String = context.Account
        Dim rowuodatefunction As String = SC3080216_MODULEID
        Dim requestlockversion As Long = registRw.ATTRACTLOCKVERSION
        Dim saleslockversion As Long = registRw.SALESVERSION

        Dim rsltdate As Date
        If Not String.IsNullOrEmpty(registRw.ACTDAYFROM) Then
            rsltdate = Convert.ToDateTime(registRw.ACTDAYFROM)
        Else
            rsltdate = Convert.ToDateTime("1900/01/01 00:00")
        End If
        Dim rsltdatetime As Date = rsltdate
        Dim dlrcd As String = context.DlrCD
        Dim brncd As String = context.BrnCD
        Dim staffcd As String = context.Account
        Dim rsltcontactmthd As String = registRw.ACTCONTACT

        Dim sysEnv As New SystemEnvSetting
        Dim actresult As String = registRw.ACTRESULT
        Dim rsltid As String
        Dim prospectcd As String = " "
        'CR活動結果IDを取得
        '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_GIVEUP_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_WALKIN
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_PROSPECT
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        End If

        Dim attid As Decimal = registRw.ATTID
        Dim thistimecractrslt As String = registRw.ACTRESULT
        Dim lastactdatetime As Date = Date.ParseExact(registRw.ACTDAYFROM.Substring(0, 10) & " " & registRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
        Dim count As Long = registRw.ACTCOUNT + 1
        Dim giveupreason As String = " "
        If Not String.IsNullOrEmpty(registRw.GIVEUPREASON) Then
            giveupreason = registRw.GIVEUPREASON
        End If

        '予定活動ID取得
        Dim dt As ActivityInfoDataSet.ActivityInfoGetScheDataDataTable = ActivityInfoTableAdapter.GetScheSqActId(0, attid)
        Dim GetScheDataRw As ActivityInfoDataSet.ActivityInfoGetScheDataRow = CType(dt.Rows(0), ActivityInfoGetScheDataRow)
        Dim actid As Decimal = GetScheDataRw.ACT_ID
        Dim actrockverrion As Long = GetScheDataRw.ROW_LOCK_VERSION
        registRw.ACTID = actid

        '誘致更新
        If (0 = ActivityInfoTableAdapter.UpdateAttract(attid, cractrslt, lastactdatetime, count,
                                                       rsltid, actid,
                                                       account, rowuodatefunction, requestlockversion)) Then
            Return False
        End If
        '活動(結果)更新
        Dim orgnzid As Decimal
        orgnzid = ActivityInfoTableAdapter.GetorgnzId(staffcd)
        If (0 = ActivityInfoTableAdapter.UpdateActivity(actid, rsltdate, rsltdatetime, dlrcd,
                                                       brncd, staffcd, rsltcontactmthd,
                                                       cractrslt, rsltid,
                                                       account, rowuodatefunction, actrockverrion, prospectcd, orgnzid)) Then
            Return False
        End If
        '商談更新
        If (0 = ActivityInfoTableAdapter.UpdateSales(fllwupboxseqno, prospectcd, completeflg, giveupvclseq, giveupreason,
                                                       account, rowuodatefunction, saleslockversion)) Then
            Return False
        End If
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateAttract End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END


    ''' <summary>
    ''' tbl_FLLWUPBOXRSLT_DONEで使用するカテゴリの設定
    ''' </summary>
    ''' <param name="fllwuptyp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupDoneCategory(ByVal fllwuptyp As String) As String
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupDoneCategory Start")
        '-----------------------------------------------------------------
        Dim doneCategory As String = ""
        'カテゴリ設定
        Select Case fllwuptyp
            Case C_FLLWUP_HOT
                doneCategory = C_DONECAT_HOT
            Case C_FLLWUP_PROSPECT
                doneCategory = C_DONECAT_PROSPECT
            Case C_FLLWUP_REPUCHASE
                doneCategory = C_DONECAT_REPURCHASE
            Case C_FLLWUP_PERIODICAL
                doneCategory = C_DONECAT_PERIODICAL
            Case C_FLLWUP_PROMOTION
                doneCategory = C_DONECAT_PROMOTION
            Case C_FLLWUP_REQUEST
                doneCategory = C_DONECAT_REQUEST
            Case C_FLLWUP_WALKIN
                doneCategory = C_DONECAT_WALKIN
        End Select
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupDoneCategory End")
        '-----------------------------------------------------------------
        Return doneCategory
    End Function

    ''' <summary>
    ''' 活動名を生成する
    ''' </summary>
    ''' <param name="servicename"></param>
    ''' <param name="promoname"></param>
    ''' <param name="condition"></param>
    ''' <param name="planid"></param>
    ''' <param name="actcategory"></param>
    ''' <param name="promoid"></param>
    ''' <param name="reqcategory"></param>
    ''' <param name="reqname"></param>
    ''' <param name="actresult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetActName(ByVal servicename As String, ByVal promoname As String, ByVal condition As String, ByVal planid As Nullable(Of Long),
                               ByVal actcategory As String, ByVal promoid As Nullable(Of Long), ByVal reqcategory As String,
                               ByVal reqname As String, ByVal actresult As String) As String

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetActName Start")
        '-----------------------------------------------------------------
        Const C_ACTCTG_PERIODICAL = "1"         ' CR活動カテゴリ／Periodical Inspection
        Const C_ACTCTG_REPURCHASE = "2"         ' CR活動カテゴリ／Repurchase Follow-up
        Const C_ACTCTG_BIRTHDAY = "4"           ' CR活動カテゴリ／Birthday
        Const C_CONDITION_ONE = "1"             ' 計画外管理-作成状態／One Time
        Const C_CONDITION_EVERY = "2"           ' 計画外管理-作成状態／Every Month
        Const C_REQCTG_WALKIN = "1"             ' リクエストカテゴリ／Walk-in
        Const C_REQCTG_CALLIN = "2"             ' リクエストカテゴリ／Call-in
        Const C_REQCTG_RMM = "3"                ' リクエストカテゴリ／RMM
        Const C_REQCTG_REQUEST = "4"            ' リクエストカテゴリ／Request
        Const C_CRRESULT_HOT = "1"              ' CR活動結果／Hot
        Const C_CRRESULT_PROSPECT = "2"         ' CR活動結果／Prospect

        Dim recactname As String                       ' 編集済活動名
        Dim prmonth As String '付属年月

        recactname = ""
        prmonth = ""

        ' Follow-up Box
        Select Case actcategory
            Case C_ACTCTG_PERIODICAL, C_ACTCTG_REPURCHASE, C_ACTCTG_BIRTHDAY
                ' 1:Periodical Inspection 2:Repurchase Follow-up 4:Birthday
                recactname = servicename
            Case Else
                If Not promoid Is Nothing Then
                    ' プロモーションIDがNULLでない
                    recactname = promoname
                    If String.IsNullOrEmpty(condition) = False Then
                        Select Case condition
                            Case C_CONDITION_ONE    ' One Time
                                prmonth = ""
                            Case C_CONDITION_EVERY  ' Every Month
                                prmonth = Mid(planid & "", 1, 8)
                                Dim prmonthwk As Date
                                prmonthwk = CDate(prmonth)
                                prmonth = DateTimeFunc.FormatDate(12, prmonthwk)
                        End Select
                    End If

                    recactname = recactname & prmonth
                Else
                    ' プロモーションIDがNULL
                    Select Case reqcategory
                        Case C_REQCTG_CALLIN, C_REQCTG_RMM, C_REQCTG_REQUEST
                            ' 2:Call-in 3:RMM 4:Request
                            If String.IsNullOrEmpty(reqname) = False Then
                                recactname = WebWordUtility.GetWord(30351) & " (" & reqname & ")"      'Request Follow-up
                            Else
                                recactname = WebWordUtility.GetWord(30351)                             'Request Follow-up
                            End If
                        Case C_REQCTG_WALKIN
                            ' Walk-in
                            recactname = WebWordUtility.GetWord(30352)                                'Walk-in Follow-up
                        Case Else
                            recactname = ""
                    End Select
                End If
        End Select

        ' CR活動結果

        If String.IsNullOrEmpty(actresult) = False Then
            Select Case actresult
                Case C_CRRESULT_HOT         ' Hot
                    If String.IsNullOrEmpty(Trim(recactname)) = False Then
                        recactname = WebWordUtility.GetWord(30353) & " (" & recactname & ")"           'Hot
                    Else
                        recactname = WebWordUtility.GetWord(30353)                                  'Hot
                    End If
                Case C_CRRESULT_PROSPECT    ' Prospect
                    If String.IsNullOrEmpty(Trim(recactname)) = False Then
                        recactname = WebWordUtility.GetWord(30354) & " (" & recactname & ")"           'Prospect
                    Else
                        recactname = WebWordUtility.GetWord(30354)                                   'Prospect
                    End If
                Case Else
                    ' そのまま出力
            End Select
        Else
            ' そのまま出力
        End If
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetActName End")
        '-----------------------------------------------------------------
        Return recactname
    End Function

    ''' <summary>
    ''' Follow-up Box種別取得
    ''' </summary>
    ''' <param name="cractresult"></param>
    ''' <param name="promotionid"></param>
    ''' <param name="cractcategory"></param>
    ''' <param name="reqcategory"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupBoxType(ByVal cractresult As String, ByVal promotionid As Nullable(Of Long), ByVal cractcategory As String,
                                     ByVal reqcategory As String) As String
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupBoxType Start")
        '-----------------------------------------------------------------
        Dim fllwupBoxType As String = ""
        Select Case cractresult
            Case C_CRACTRESULT_HOT    'Hot
                fllwupBoxType = C_FLLWUP_HOT
            Case C_CRACTRESULT_PROSPECT    'Prospect
                fllwupBoxType = C_FLLWUP_PROSPECT
            Case C_CRACTRESULT_NOTACT, C_CRACTRESULT_CONTINUE
                If Not promotionid Is Nothing Then  'Promotion
                    fllwupBoxType = C_FLLWUP_PROMOTION
                Else
                    Select Case cractcategory
                        Case C_CRACTCATEGORY_REPURCHASE    'Repurchase
                            fllwupBoxType = C_FLLWUP_REPUCHASE
                        Case C_CRACTCATEGORY_PERIODICAL, C_CRACTCATEGORY_OTHERS, C_CRACTCATEGORY_BIRTHDAY 'Periodical
                            fllwupBoxType = C_FLLWUP_PERIODICAL
                        Case C_CRACTCATEGORY_DEFFULT
                            Select Case reqcategory
                                Case C_REQCATEGORY_WALKIN    'Walk-in
                                    fllwupBoxType = C_FLLWUP_WALKIN
                                Case C_REQCATEGORY_CALLIN, C_REQCATEGORY_RMM, C_REQCATEGORY_REQUEST    'Request
                                    fllwupBoxType = C_FLLWUP_REQUEST
                            End Select
                    End Select
                End If
        End Select
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupBoxType End")
        '-----------------------------------------------------------------
        Return fllwupBoxType
    End Function

    ''' <summary>
    ''' ToDoリスト登録
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <param name="fllwstatus">CR活動ステータス</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function SetToDo(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, ByVal fllwstatus As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("SetToDo Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_KEISYO_ZENGO)
        Dim nmtitledt As ActivityInfoNameTitleDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        registRw = CType(registdt.Rows(0), ActivityInfoRegistDataRow)
        nmtitledt = ActivityInfoTableAdapter.GetOrgNameTitle(registRw.INSDID)
        Dim nmtitlerw As ActivityInfoNameTitleRow
        nmtitlerw = CType(nmtitledt.Rows(0), ActivityInfoNameTitleRow)
        Using sendObj As New IC3040401.IC3040401BusinessLogic
            '共通設定項目作成
            sendObj.CreateCommon()
            '親レコード設定
            sendObj.ActionType = "0"
            sendObj.DealerCode = context.DlrCD
            '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 START
            sendObj.BranchCode = ActivityInfoTableAdapter.GetPreBrnCd(registRw.FLLWSEQ)
            '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 END
            sendObj.ScheduleDivision = "0"
            sendObj.ScheduleId = registRw.FLLWSEQ.ToString(CultureInfo.CurrentCulture())
            sendObj.ActivityCreateStaffCode = context.Account

            If String.Equals(registRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(registRw.ACTRESULT, C_RSLT_GIVEUP) Then
                'SuccessかGive-upの場合
                sendObj.CompleteFlg = "3"
                sendObj.CompletionDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
            Else
                If String.IsNullOrEmpty(fllwstatus) Then
                    sendObj.CompleteFlg = "1"
                Else
                    sendObj.CompleteFlg = "2"
                End If
                If String.Equals(registRw.CSTKIND, "1") Then
                    sendObj.CustomerDivision = "0"
                Else
                    sendObj.CustomerDivision = "2"
                End If
                sendObj.CustomerId = registRw.INSDID
                sendObj.CustomerName = nmtitlerw.NAME
                If nmtitlerw.IsNAMETITLENull Then
                    sendObj.NameTitle = ""
                Else
                    sendObj.NameTitle = nmtitlerw.NAMETITLE
                End If
                sendObj.NameTitlePosition = sysEnvRow.PARAMVALUE
            End If

            If String.Equals(registRw.ACTRESULT, C_RSLT_WALKIN) Or String.Equals(registRw.ACTRESULT, C_RSLT_PROSPECT) Or String.Equals(registRw.ACTRESULT, C_RSLT_HOT) Then
                '子レコード作成
                sendObj.CreateScheduleInfo()
                '子レコードプロパティ設定
                sendObj.ActivityStaffBranchCode(0) = context.BrnCD
                sendObj.ActivityStaffCode(0) = context.Account
                Dim cntnmdt As ActivityInfoGetContactNmDataTable
                Dim cntnmrw As ActivityInfoGetContactNmRow
                Dim clrdt As ActivityInfoTodoColorDataTable
                Dim clrrw As ActivityInfoTodoColorRow

                If String.Equals(registRw.FOLLOWFLG, "1") Then
                    'フォロー有(2レコード作るパターン)
                    If String.Equals(registRw.FOLLOWDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(0) = registRw.FOLLOWDAYFROM
                        sendObj.EndTime(0) = registRw.FOLLOWDAYFROM.Substring(0, 10) & " " & registRw.FOLLOWDAYTO
                    Else
                        '納期のみ
                        If registRw.FOLLOWTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(0) = registRw.FOLLOWDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(0) = registRw.FOLLOWDAYFROM.Substring(0, 10)
                        End If
                    End If

                    sendObj.AlarmNo(0) = registRw.FOLLOWALERT
                    sendObj.ContactNo(0) = registRw.FOLLOWCONTACT
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(registRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    sendObj.ComingFollowName(0) = WebWordUtility.GetWord("SCHEDULE", 1)
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "1", Long.Parse(registRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
                    sendObj.BackgroundColor(0) = clrrw.BACKGROUNDCOLOR
                    '子レコード作成
                    sendObj.CreateScheduleInfo()
                    sendObj.ActivityStaffBranchCode(1) = context.BrnCD
                    sendObj.ActivityStaffCode(1) = context.Account

                    '次回活動のチップ
                    If String.Equals(registRw.NEXTACTDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(1) = registRw.NEXTACTDAYFROM
                        sendObj.EndTime(1) = registRw.NEXTACTDAYFROM.Substring(0, 10) & " " & registRw.NEXTACTDAYTO
                    Else
                        '納期のみ
                        If registRw.NEXTACTTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(1) = registRw.NEXTACTDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(1) = registRw.NEXTACTDAYFROM.Substring(0, 10)
                        End If
                    End If

                    sendObj.AlarmNo(1) = registRw.NEXTACTALERT
                    sendObj.ContactNo(1) = registRw.NEXTACTCONTACT
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(registRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(1) = cntnmrw.CONTACT
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(registRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
                    sendObj.BackgroundColor(1) = clrrw.BACKGROUNDCOLOR
                Else
                    'フォロー無
                    If String.Equals(registRw.NEXTACTDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(0) = registRw.NEXTACTDAYFROM
                        sendObj.EndTime(0) = registRw.NEXTACTDAYFROM.Substring(0, 10) & " " & registRw.NEXTACTDAYTO
                    Else
                        '納期のみ
                        If registRw.NEXTACTTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(0) = registRw.NEXTACTDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(0) = registRw.NEXTACTDAYFROM.Substring(0, 10)
                        End If

                    End If
                    sendObj.AlarmNo(0) = registRw.NEXTACTALERT
                    sendObj.ContactNo(0) = registRw.NEXTACTCONTACT
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(registRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(registRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
                    sendObj.BackgroundColor(0) = clrrw.BACKGROUNDCOLOR
                End If
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            'Webサービス連携を実施:引数は対象URL
            Dim errCd As String
            Dim dlrenvdt As New DealerEnvSetting
            Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
            dlrenvrw = dlrenvdt.GetEnvSetting("XXXXX", C_CALDAV_WEBSERVICE_URL)
            '対象URLはDLRENVSETTINGより取得する
            errCd = sendObj.SendScheduleInfo(dlrenvrw.PARAMVALUE)
            'errCd = 1
            If String.Equals(errCd, "0") = False Then
                'エラー処理
                '--デバッグログ---------------------------------------------------
                Logger.Info("Webサービス連携 失敗")
                '-----------------------------------------------------------------
                Return False
            End If
        End Using
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetToDo End")
        '-----------------------------------------------------------------
        Return True


    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談 を更新
    ''' </summary>
    ''' <param name="fllwupbox_seqno"></param>
    ''' <param name="actualaccount"></param>
    ''' <param name="salesstarttime"></param>
    ''' <param name="salesendtime"></param>
    ''' <param name="account"></param>
    ''' <param name="updateid"></param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal fllwupbox_seqno As Decimal, _
                            ByVal actualaccount As String, _
                            ByVal salesstarttime As Date, _
                            ByVal salesendtime As Date, _
                            ByVal account As String, _
                            ByVal updateid As String) As Integer

        'FLLWUPBOX 商談を更新
        Return ActivityInfoTableAdapter.UpdateFllwupboxSales(fllwupbox_seqno, _
                                                            actualaccount, _
                                                            salesstarttime, _
                                                            salesendtime, _
                                                            account, _
                                                            updateid)

    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' Follow-up Box商談を更新
    ''' </summary>
    ''' <param name="registRow">登録用データ</param>
    ''' <returns>処理件数</returns>
    ''' <remarks>(SC3080216の活動結果登録時にのみ利用可)</remarks>
    Private Shared Function UpdateFllwupboxSales(registRow As ActivityInfoDataSet.ActivityInfoRegistDataRow) As Integer

        Dim actaccount As String = registRow.ACTACCOUNT & "@" & StaffContext.Current.DlrCD       '活動実施者(画面で入力した値)
        Dim actDayFromDate As Date = Date.ParseExact(registRow.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        'Toは時分しか持っていないためFrom側から日付をセット
        Dim actdayto As String = registRow.ACTDAYFROM.Substring(0, 10) & " " & registRow.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        Const CONTENT_MODULEID As String = "SC3080216"

        Return UpdateFllwupboxSales(registRow.FLLWSEQ, actaccount, actDayFromDate, actDayToDate, StaffContext.Current.Account, CONTENT_MODULEID)
    End Function
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 最大の活動終了時間を取得
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="strCD"></param>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetLatestActTimeEnd(ByVal dlrCD As String, ByVal strCD As String, ByVal fllwupboxSeqNo As Decimal) As ActivityInfoDataSet.ActivityInfoLatestActTimeDataTable

        Return ActivityInfoTableAdapter.GetLatestActTimeEnd(fllwupboxSeqNo)

    End Function
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 希望車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' CNTCD:国コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SERIESCD:シリーズコード
    ''' SERIESNM:シリーズ名
    ''' MODELCD:モデルコード
    ''' VCLMODEL_NAME:モデル名
    ''' COLORCD:カラーコード
    ''' DISP_BDY_COLOR:カラー名
    ''' PICIMAGE:モデル写真
    ''' LOGOIMAGE:モデルロゴ
    ''' QUANTITY:台数
    ''' SEQNO:希望車種シーケンスNo
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetSelectedSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable

        Logger.Info("GetSelectedSeriesList Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim cntcd As String                         '国コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow

        '商談見込み度コード既定値設定
        Dim mostPerfCd As String
        mostPerfCd = GetSysEnvSettingValue(ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD)

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        cntcd = datarowFrom.CNTCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 希望車種取得
        datatableSelectedSeries =
            ActivityInfoTableAdapter.GetSelectedSeries(dlrcd, strcd, cntcd, fllwupbox_seqno)

        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
            For Each dt As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In datatableSelectedSeries
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow
                datarowTo = datatableTo.NewActivityInfoGetSelectedSeriesListToRow

                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.SERIESNM = dt.SERIESNM

                'グレードコード
                If dt.IsMODELCDNull Then
                    datarowTo.MODELCD = String.Empty
                Else
                    datarowTo.MODELCD = dt.MODELCD
                End If

                'グレード名称
                If dt.IsVCLMODEL_NAMENull Then
                    datarowTo.VCLMODEL_NAME = String.Empty
                Else
                    datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME
                End If

                'サフィックスコード
                If dt.IsSUFFIX_CDNull Then
                    datarowTo.SUFFIX_CD = String.Empty
                Else
                    datarowTo.SUFFIX_CD = dt.SUFFIX_CD
                End If

                'サフィックス名称
                If dt.IsSUFFIX_NAMENull Then
                    datarowTo.SUFFIX_NAME = String.Empty
                Else
                    datarowTo.SUFFIX_NAME = dt.SUFFIX_NAME
                End If

                '外装色コード
                If dt.IsCOLORCDNull Then
                    datarowTo.COLORCD = String.Empty
                Else
                    datarowTo.COLORCD = dt.COLORCD
                End If

                '外装色名称
                If dt.IsDISP_BDY_COLORNull Then
                    datarowTo.DISP_BDY_COLOR = String.Empty
                Else
                    datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR
                End If

                '内装色コード
                If dt.IsINTERIORCLR_CDNull Then
                    datarowTo.INTERIORCLR_CD = String.Empty
                Else
                    datarowTo.INTERIORCLR_CD = dt.INTERIORCLR_CD
                End If

                '内装色名称
                If dt.IsINTERIORCLR_NAMENull Then
                    datarowTo.INTERIORCLR_NAME = String.Empty
                Else
                    datarowTo.INTERIORCLR_NAME = dt.INTERIORCLR_NAME
                End If

                '画像パス
                If dt.IsPICIMAGENull Then
                    datarowTo.PICIMAGE = String.Empty
                Else
                    datarowTo.PICIMAGE = dt.PICIMAGE
                End If

                If dt.IsLOGOIMAGENull Then
                    datarowTo.LOGOIMAGE = String.Empty
                Else
                    datarowTo.LOGOIMAGE = dt.LOGOIMAGE
                End If

                datarowTo.QUANTITY = dt.QUANTITY
                datarowTo.SEQNO = dt.SEQNO
                datarowTo.ROWLOCKVERSION = dt.ROWLOCKVERSION

                If dt.IsSALES_PROSPECT_CDNull OrElse dt.SALES_PROSPECT_CD.Trim().Length = 0 Then
                    datarowTo.MOST_PREF_VCL_FLG = "0"
                Else
                    If (dt.SALES_PROSPECT_CD.Equals(mostPerfCd)) Then
                        datarowTo.MOST_PREF_VCL_FLG = "1"
                    Else
                        datarowTo.MOST_PREF_VCL_FLG = "0"
                    End If
                End If

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedSeriesList End")

    End Function

    ''' <summary>
    ''' 成約車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SERIESCD:シリーズコード
    ''' SERIESNM:シリーズ名
    ''' MODELCD:モデルコード
    ''' VCLMODEL_NAME:モデル名
    ''' COLORCD:カラーコード
    ''' DISP_BDY_COLOR:カラー名
    ''' PICIMAGE:モデル写真
    ''' LOGOIMAGE:モデルロゴ
    ''' QUANTITY:台数
    ''' SEQNO:希望車種シーケンスNo
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetSuccessSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
        Logger.Info("GetSuccessSeriesList Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal                 'FollowupBox連番
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        datatableSelectedSeries = ActivityInfoTableAdapter.GetSuccessSeries(fllwupbox_seqno)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
            For Each dt As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In datatableSelectedSeries
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow
                datarowTo = datatableTo.NewActivityInfoGetSelectedSeriesListToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.SERIESNM = dt.SERIESNM

                'グレードコード
                If dt.IsMODELCDNull Then
                    datarowTo.MODELCD = String.Empty
                Else
                    datarowTo.MODELCD = dt.MODELCD
                End If

                'グレード名称
                If dt.IsVCLMODEL_NAMENull Then
                    datarowTo.VCLMODEL_NAME = String.Empty
                Else
                    datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME
                End If

                'サフィックスコード
                If dt.IsSUFFIX_CDNull Then
                    datarowTo.SUFFIX_CD = String.Empty
                Else
                    datarowTo.SUFFIX_CD = dt.SUFFIX_CD
                End If

                'サフィックス名称
                If dt.IsSUFFIX_NAMENull Then
                    datarowTo.SUFFIX_NAME = String.Empty
                Else
                    datarowTo.SUFFIX_NAME = dt.SUFFIX_NAME
                End If

                '外装色コード
                If dt.IsCOLORCDNull Then
                    datarowTo.COLORCD = String.Empty
                Else
                    datarowTo.COLORCD = dt.COLORCD
                End If

                '外装色名称
                If dt.IsDISP_BDY_COLORNull Then
                    datarowTo.DISP_BDY_COLOR = String.Empty
                Else
                    datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR
                End If

                '内装色コード
                If dt.IsINTERIORCLR_CDNull Then
                    datarowTo.INTERIORCLR_CD = String.Empty
                Else
                    datarowTo.INTERIORCLR_CD = dt.INTERIORCLR_CD
                End If

                '内装色名称
                If dt.IsINTERIORCLR_NAMENull Then
                    datarowTo.INTERIORCLR_NAME = String.Empty
                Else
                    datarowTo.INTERIORCLR_NAME = dt.INTERIORCLR_NAME
                End If

                '画像パス
                If dt.IsPICIMAGENull Then
                    datarowTo.PICIMAGE = String.Empty
                Else
                    datarowTo.PICIMAGE = dt.PICIMAGE
                End If

                If dt.IsLOGOIMAGENull Then
                    datarowTo.LOGOIMAGE = String.Empty
                Else
                    datarowTo.LOGOIMAGE = dt.LOGOIMAGE
                End If

                datarowTo.QUANTITY = dt.QUANTITY
                datarowTo.SEQNO = dt.SEQNO
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSuccessSeriesList End")
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' プロセス取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SEQNO:希望車種シーケンスNo
    ''' CATALOGDATE:カタログ実施日
    ''' TESTDRIVEDATE:試乗実施日
    ''' EVALUATIONDATE:査定実施日
    ''' QUOTATIONDATE:見積実施日
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetProcess(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable) As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
        Logger.Info("GetProcess Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim datatableProcess As ActivityInfoDataSet.ActivityInfoGetProcessDataTable
        Dim datarowProcess As ActivityInfoDataSet.ActivityInfoGetProcessRow
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetProcessFromRow
        Dim tempSeqno As Long

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        ' プロセス取得
        datatableProcess = ActivityInfoTableAdapter.GetProcess(fllwupbox_seqno)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        ' DataTableに格納
        If datatableProcess.Rows.Count > 0 Then
            datarowProcess = CType(datatableProcess.Rows(0), ActivityInfoDataSet.ActivityInfoGetProcessRow)
            tempSeqno = datarowProcess.SEQNO
        End If

        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
            Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetProcessToRow
            datarowTo = datatableTo.NewActivityInfoGetProcessToRow
            Dim scDlrCd As String = StaffContext.Current.DlrCD
            For Each dt As ActivityInfoDataSet.ActivityInfoGetProcessRow In datatableProcess
                If tempSeqno <> dt.SEQNO Then
                    datatableTo.Rows.Add(datarowTo)
                    datarowTo = datatableTo.NewActivityInfoGetProcessToRow
                    tempSeqno = dt.SEQNO
                End If

                Select Case dt.ACTIONCD
                    Case ACTIONCD_CATALOG
                        datarowTo.CATALOGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_TESTDRIVE
                        datarowTo.TESTDRIVEDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_EVALUATION
                        datarowTo.EVALUATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_QUOTATION
                        datarowTo.QUOTATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                End Select

                datarowTo.SEQNO = dt.SEQNO
            Next

            If datatableProcess.Rows.Count > 0 Then
                datatableTo.Rows.Add(datarowTo)
            End If

            Return datatableTo
        End Using

        Logger.Info("GetProcess End")
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    ''' <summary>
    ''' ステータス取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' CRACTRESULT:活動結果(1:Hot,2:Warm,3:Success,4:Cold,5:Give-up)
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetStatus(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable) As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
        Logger.Info("GetStatus Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal                 'FollowupBox連番
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableStatus As ActivityInfoDataSet.ActivityInfoGetStatusDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetStatusFromRow

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        ' ステータス取得
        datatableStatus = ActivityInfoTableAdapter.GetStatus(fllwupbox_seqno)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
            If datatableStatus.Rows.Count > 0 Then
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetStatusToRow
                datarowTo = datatableTo.NewActivityInfoGetStatusToRow
                Dim dt As ActivityInfoDataSet.ActivityInfoGetStatusRow =
                    CType(datatableStatus.Rows(0), ActivityInfoDataSet.ActivityInfoGetStatusRow)
                datarowTo.CRACTRESULT = dt.CRACTRESULT
                datatableTo.Rows.Add(datarowTo)
            End If

            Return datatableTo
        End Using
        Logger.Info("GetStatus End")
    End Function

    ''' <summary>
    ''' CR活動成功のデータ存在判定
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' 判定結果:(0:受注時,1:受注後)
    ''' </returns>
    ''' <remarks>CR活動成功のデータが存在するか判定</remarks>
    Public Shared Function CountFllwupboxRslt(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoCountFromDataTable) As String

        Logger.Info("CountFllwupboxRslt Start")

        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim strcd As String = datatableFrom(0).STRCD
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupboxseqno As Decimal = datatableFrom(0).FLLWUPBOX_SEQNO
        Dim rslt As String
        Dim cnt As Integer = ActivityInfoTableAdapter.CountFllwupboxRslt(fllwupboxseqno, CRACTRESULT_SUCCESS)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        If cnt > 0 Then
            '活動が1件以上存在する
            rslt = SALESAFTER_YES
        Else
            '存在しない
            rslt = SALESAFTER_NO
        End If

        Logger.Info("CountFllwupboxRslt End")
        Return rslt
    End Function

    ''' <summary>
    ''' 契約書No取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>契約書No</returns>
    ''' <remarks>契約書No取得</remarks>
    Public Shared Function GetContractNo(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String

        Logger.Info("GetContractInfo Start")

        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim strcd As String = datatableFrom(0).STRCD
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupboxseqno As Decimal = datatableFrom(0).FLLWUPBOX_SEQNO
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim rslt As String

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        '契約書Noを取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoContractNoDataTable =
            ActivityInfoTableAdapter.GetContractNo(fllwupboxseqno)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        If dataSet.Rows.Count > 0 Then
            If (dataSet(0).IsCONTRACTNONull) Then
                rslt = String.Empty
            Else
                rslt = CStr(dataSet(0).CONTRACTNO)
            End If
        Else
            rslt = String.Empty
        End If

        Logger.Info("GetContractInfo End")
        Return rslt
    End Function

    ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' 未実施受注後活動存在確認
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>確認結果 True:存在する False:存在しない</returns>
    ''' <remarks></remarks>
    Public Shared Function IsExistsUnexecutedAfterOdrAct(salesid As Decimal) As Boolean

        Logger.Info("IsExistsUnexecutedAfterOdrAct Start")

        Dim result As Boolean = False

        ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 ADD START
        Dim parmAfterOdrFlg As String = String.Empty
        '受注後工程利用フラグ取得
        parmAfterOdrFlg = ActivityInfoBusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)

        If String.Equals(parmAfterOdrFlg, "0") Then
            '受注後工程利用フラグを利用しない場合
            Return False
        End If
        ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 ADD END 

        '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
        If Not ActivityInfoTableAdapter.IsExistsAfterOdr(salesid) Then
            '受注後活動が存在しない場合(受注後フォロー対応以前に終わった活動で、そのまま放置されている)
            result = True
        Else
            Dim cnt As Integer = 0
            '受注後活動未実施件数を取得
            cnt = ActivityInfoTableAdapter.CountUnexecutedAfterOdrAct(salesid)
            result = (cnt > 0)
        End If
        '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END

        Logger.Info("IsExistsUnexecutedAfterOdrAct End")
        Return result
    End Function
    ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 END

    ' 2012/02/29 TCS 安田 【SALES_2】 START
    ''' <summary>
    ''' キャンセル区分取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesbkgno">受注No</param>
    ''' <returns>True:注文キャンセル False:それ以外</returns>
    ''' <remarks>注文キャンセルされているか判定する</remarks>
    Public Shared Function GetSalesCancel(ByVal dlrcd As String,
                                            ByVal salesbkgno As String) As Boolean

        Logger.Info("GetSalesCancel Start")

        Dim rslt As Boolean = False

        'キャンセル区分取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoGetCancelStatusDataTable =
            ActivityInfoTableAdapter.GetSalesCancel(dlrcd, salesbkgno)
        If dataSet.Rows.Count > 0 Then
            'キャンセル区分が1:注文キャンセル時
            If (Not dataSet(0).IsCANCELFLGNull AndAlso dataSet(0).CANCELFLG.Equals("1")) Then
                rslt = True
            End If
        End If

        Logger.Info("GetSalesCancel End")

        Return rslt

    End Function

    ''' <summary>
    ''' 活動中リスト取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="insdid">未取引客ID／自社客連番</param>
    ''' <param name="cstkind">未取引客:2／自社客種別:1</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesActiveList(ByVal dlrcd As String,
                                       ByVal strcd As String,
                                       ByVal insdid As String,
                                       ByVal cstkind As String,
                                       ByVal newcustid As String) As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable

        Logger.Info("GetSalesActiveList Start")

        Dim dataSet As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable =
            ActivityInfoTableAdapter.GetSalesActiveList(dlrcd, strcd, insdid, cstkind, newcustid)

        Return dataSet

        Logger.Info("GetSalesActiveList End")

    End Function
    ' 2012/02/29 TCS 安田 【SALES_2】 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 契約日取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>契約日</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateDate(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetContractDateFromDataTable) As ActivityInfoDataSet.ActivityInfoGetContractDateDataTable

        Logger.Info("GetEstimateDate Start")

        '契約日を取得
        Dim estimateTbl As New ActivityInfoDataSet.ActivityInfoGetContractDateDataTable
        estimateTbl = ActivityInfoTableAdapter.GetContractDate(datatableFrom(0).FLLWUPBOX_SEQNO)

        Logger.Info("GetEstimateDate End")
        Return estimateTbl
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 活動方法初期値取得
    ''' </summary>
    ''' <param name="bookedafterflg">受注後フラグ (指定がなければ全件検索)</param>
    ''' <returns>活動方法データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetInitActContact(ByVal bookedafterflg As String) As ActivityInfoDataSet.ActivityInfoActContactRow

        '2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        '現在未使用で、CA1804のコード分析エラーが発生する為、削除
        'Dim contactNo As Integer = Nothing
        '2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        Dim dt As ActivityInfoDataSet.ActivityInfoActContactDataTable
        dt = ActivityInfoTableAdapter.GetActContact(bookedafterflg)
        Dim rw As ActivityInfoDataSet.ActivityInfoActContactRow = Nothing

        '現在のステータス
        Dim staffStatus As String = StaffContext.Current.PresenceCategory & StaffContext.Current.PresenceDetail

        '初期選択値を探す
        For Each dr As ActivityInfoDataSet.ActivityInfoActContactRow In dt.Rows
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
            If (String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Or String.Equals(staffStatus, STAFF_STATUS_DELIVERY)) AndAlso String.Equals(dr.FIRSTSELECT_WALKIN, "1") Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

                '商談中,納車作業中の場合、初期選択(商談)のレコードを探す
                rw = dr
                Exit For

                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
            ElseIf Not String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) And Not String.Equals(staffStatus, STAFF_STATUS_DELIVERY) AndAlso String.Equals(dr.FIRSTSELECT_NOTWALKIN, "1") Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

                '商談中以外の場合、初期選択(営業活動)のレコードを探す
                rw = dr
                Exit For
            End If
        Next

        'Return CStr(rw.CONTACTNO) & "," & rw.PROCESS
        Return rw
    End Function

    ''' <summary>
    ''' 担当スタッフ取得
    ''' </summary>
    ''' <param name="account">アカウント</param>
    ''' <returns>担当スタッフデータテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetStaff(ByVal account As String) As ActivityInfoDataSet.ActivityInfoUsersDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetStaff(context.DlrCD, context.BrnCD, account)

    End Function

    ''' <summary>
    ''' 今回活動分類タイトル取得
    ''' </summary>
    ''' <param name="contactNo">分類コード</param>
    ''' <returns>活動分類タイトル</returns>
    ''' <remarks></remarks>
    Shared Function GetInitActContactTitle(ByVal contactNo As String) As String

        'Dim contactNo As Integer = Nothing
        Dim dt As ActivityInfoDataSet.ActivityInfoGetContactNmDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        dt = ActivityInfoTableAdapter.GetContactNM(CType(contactNo, Long))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim rw As ActivityInfoDataSet.ActivityInfoGetContactNmRow = Nothing

        rw = dt.Rows(0)

        ''現在のステータス
        'Dim staffStatus As String = StaffContext.Current.PresenceCategory & StaffContext.Current.PresenceDetail
        '
        ''初期選択値を探す
        'For Each dr As ActivityInfoDataSet.ActivityInfoActContactRow In dt.Rows
        '    If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_WALKIN, "1") Then
        '        '商談中の場合、初期選択(商談)のレコードを探す
        '        rw = dr
        '        Exit For
        '    ElseIf Not String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_NOTWALKIN, "1") Then
        '        '商談中以外の場合、初期選択(営業活動)のレコードを探す
        '        rw = dr
        '        Exit For
        '    End If
        'Next

        'Return CStr(rw.CONTACTNO) & "," & rw.PROCESS
        Return rw.CONTACT

    End Function

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' 未取引客個人情報取得
    ''' </summary>
    ''' <param name="custId">未取引客ユーザーID</param>
    ''' <returns>GetNewCustomerDataTable</returns>
    ''' <remarks></remarks>
    Shared Function GetNewcustomer(ByVal custId As String) As ActivityInfoDataSet.GetNewCustomerDataTable
        Dim dt As ActivityInfoDataSet.GetNewCustomerDataTable = ActivityInfoTableAdapter.GetNewCustomer(custId)

        Return dt
    End Function
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2013/03/06 TCS 河原 GL0874 START
    ''' <summary>
    ''' 契約状況フラグの取得
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns>契約状況フラグ</returns>
    ''' <remarks>契約状況フラグの取得</remarks>
    Public Shared Function GetContractFlg(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String
        Logger.Info("GetContractFlg Start")
        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim strcd As String = datatableFrom(0).STRCD
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupboxseqno As Decimal = datatableFrom(0).FLLWUPBOX_SEQNO
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim rslt As String
        '契約フラグを取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoContractFlgDataTable =
            ActivityInfoTableAdapter.GetContractFlg(dlrcd, strcd, fllwupboxseqno)
        If dataSet.Rows.Count > 0 Then
            If (dataSet(0).IsCONTRACTFLGNull) Then
                rslt = String.Empty
            Else
                rslt = CStr(dataSet(0).CONTRACTFLG)
            End If
        Else
            rslt = String.Empty
        End If
        Logger.Info("GetContractFlg End")
        Return rslt
    End Function
    '2013/03/06 TCS 河原 GL0874 END

    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' 査定依頼機能使用可否判定
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <returns>査定依頼機能使用可否判定結果</returns>
    ''' <remarks></remarks>
    Public Shared Function IsRegAsm(ByVal dlrcd As String) As Boolean

        Logger.Info("IsRegAsm Start")

        'ログ出力
        Dim paramvalue As String = String.Empty
        paramvalue = "Param dlrcd=" & dlrcd
        Logger.Info(paramvalue)

        '販売店環境設定からフラグを取得
        Dim dlrenvdt As New DealerEnvSetting
        Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
        dlrenvrw = dlrenvdt.GetEnvSetting(dlrcd, C_USED_FLG_ASSESS_PRMNAME)
        Dim AssessFlg As String = dlrenvrw.PARAMVALUE

        'インスタンス解放
        dlrenvrw = Nothing
        dlrenvdt = Nothing

        Dim retUsedFlg As Boolean = False
        If String.Equals(AssessFlg, C_USED_FLG_ASSESS_ON) Then
            '査定依頼機能を使用する
            retUsedFlg = True
        Else
            '査定依頼機能を使用しない
            retUsedFlg = False
        End If

        Logger.Info("IsRegAsm End")

        Return retUsedFlg

    End Function

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START

    ''' <summary>
    ''' 査定実績判定
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <param name="strcd">セッション値の店舗コード</param>
    ''' <param name="fllwupboxseqno">セッション値のFollow-up Box内連番</param>
    ''' <param name="assessmentNo">査定No</param>
    ''' <param name="isDbRowLock">レコードロックの有無</param>
    ''' <returns>査定実績判定</returns>
    ''' <remarks></remarks>
    Public Shared Function IsActAsm(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal,
                                    Optional ByRef assessmentNo As Long = 0, Optional ByVal isDbRowLock As Boolean = False) As String
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

        Logger.Info("IsActAsm Start")

        'ログ出力
        Dim paramvalue As String = String.Empty
        paramvalue = "Param dlrcd=" & dlrcd & _
                     ", strcd=" & strcd & _
                     ", fllwupboxseqno=" & fllwupboxseqno
        Logger.Info(paramvalue)

        '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START

        '戻り値を実績なしで初期化
        Dim retAsmActStatus As String = C_ASMACTSTATUS_NASI

        Dim dt As ActivityInfoDataSet.ActivityInfoActAsmInfoDataTable

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        '査定実績を取得
        dt = ActivityInfoTableAdapter.GetActAsmInfo(fllwupboxseqno, isDbRowLock)
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START

        '査定実績がなくない場合
        If Not (dt Is Nothing) Then

            '査定実績は複数存在する場合がある
            For Each rw As ActivityInfoDataSet.ActivityInfoActAsmInfoRow In dt
                Dim asmAnsweredFlg As String = rw.ASM_ANSWERED_FLG
                Dim noticeReqStatus As String = rw.STATUS

                '査定依頼回答
                If String.Equals(asmAnsweredFlg, C_ASMFLG_ON) Then
                    '回答有りは、キャンセルでも実績登録で確定
                    retAsmActStatus = C_ASMACTSTATUS_ARI
                    Exit For
                Else
                    '査定依頼ステータス
                    If Not String.Equals(noticeReqStatus, C_NOTICEREQSTATUS_CANCEL) Then
                        'キャンセル以外は実績退避
                        retAsmActStatus = C_ASMACTSTATUS_MIKAITOU
                        '退避する査定Noを取得
                        assessmentNo = rw.ASSESSMENTNO
                    End If
                End If
            Next

        End If
        '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

        Logger.Info("IsActAsm End")

        '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
        Return retAsmActStatus
        '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START

    ''' <summary>
    ''' 見積車種取得
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <param name="strcd">セッション値の店舗コード</param>
    ''' <param name="fllwupboxseqno">セッション値のFollow-up Box内連番</param>
    ''' <param name="cntcd">国コード</param>
    ''' <returns>見積車種のデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateCar(ByVal dlrcd As String, ByVal strcd As String, _
                                                ByVal fllwupboxseqno As Decimal, ByVal cntcd As String) As ActivityInfoDataSet.ActivityInfoEstimateCarDataTable

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("GetEstimateCar Start")

        'ログ出力
        Dim paramvalue As String = String.Empty
        paramvalue = "Param dlrcd=" & dlrcd & _
                     ", strcd=" & strcd & _
                     ", fllwupboxseqno=" & fllwupboxseqno & _
                     ", cntcd=" & cntcd
        Logger.Info(paramvalue)

        Dim dtEstimateCar As ActivityInfoDataSet.ActivityInfoEstimateCarDataTable = Nothing

        '見積車種取得
        dtEstimateCar = ActivityInfoTableAdapter.GetEstimateCar(fllwupboxseqno)

        '支払い総額を取得する
        Dim estimateBiz As New IC3070201BusinessLogic()
        Dim totalPrice As Double
        For Each row In dtEstimateCar
            totalPrice = 0

            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            totalPrice = estimateBiz.GetTotalPrice(row.ESTIMATEID, 0)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            '表示用支払い総額設定
            row.DISPLAY_PRICE = totalPrice.ToString("0.00", Globalization.CultureInfo.InvariantCulture)
        Next

        'インスタンス解放
        estimateBiz = Nothing

        Logger.Info("GetEstimateCar End")

        Return dtEstimateCar

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未存在希望車種の登録
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>Follow-up Box選択車種情報シーケンスNo</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertNotRegSelectedSeries(ByVal estimateId As Long, ByVal updateAccount As String, ByVal salesid As Decimal) As Long
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        'ログ出力
        Logger.Info("InsertNotRegSelectedSeries Start")

        Dim paramvalue As String = String.Empty
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        paramvalue = "Param estimateId=" & estimateId & _
                     ", updateAccount=" & updateAccount
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info(paramvalue)

        '商談がHistoryテーブルに移行されているかチェックする
        Dim salesHisFlg As Boolean
        salesHisFlg = ActivityInfoTableAdapter.CheckSalesHistory(salesid)

        Dim dtExistSeq As ActivityInfoDataSet.ActivityInfoExistSeqSelectedSeriesDataTable
        Dim rwExistSeq As ActivityInfoDataSet.ActivityInfoExistSeqSelectedSeriesRow

        'Follow-up Box選択車種情報シーケンスNo
        Dim retSeqno As Long = -1

        '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim useFlgSuffix As String
        Dim useFlgInteriorClr As String

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            useFlgSuffix = "0"
        Else
            useFlgSuffix = dataRow.SETTING_VAL
        End If

        '内装色使用可否フラグ(設定値が無ければ0)
        Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRowclr = systemBiz.GetSystemSetting(USE_FLG_INTERIORCLR)

        If IsNothing(dataRowclr) Then
            useFlgInteriorClr = "0"
        Else
            useFlgInteriorClr = dataRowclr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '希望車種の既存シーケンス取得
        dtExistSeq = ActivityInfoTableAdapter.GetExistSeqSelectedSeries(estimateId, salesHisFlg, useFlgSuffix, useFlgInteriorClr)
        '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        '既存の希望車種にシーケンスが存在したか判別
        'note:既存の見積情報に紐づく希望車種が存在しているかを
        '     シーケンスのレコード数で判別
        If (dtExistSeq Is Nothing) OrElse _
            (0 = dtExistSeq.Rows.Count) Then
            '=== 希望車種の既存シーケンスが未存在 ===

            '希望車種に追加する見積車両情報を取得
            Dim dtEstVclInfo As ActivityInfoDataSet.ActivityInfoEstVclInfoDataTable
            Dim rwEstVclInfo As ActivityInfoDataSet.ActivityInfoEstVclInfoRow

            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            dtEstVclInfo = ActivityInfoTableAdapter.GetEstVclInfo(estimateId)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            '見積車両情報が0件時は、SeqNoを-1で返却
            If (dtEstVclInfo Is Nothing) OrElse _
                (0 = dtEstVclInfo.Rows.Count) Then
                Return -1
            End If

            rwEstVclInfo = dtEstVclInfo.Rows(0)

            '希望車種の新規シーケンス取得
            Dim dtNewSeq As ActivityInfoDataSet.ActivityInfoNewSeqSelectedSeriesDataTable
            Dim rwNewSeq As ActivityInfoDataSet.ActivityInfoNewSeqSelectedSeriesRow

            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            dtNewSeq = ActivityInfoTableAdapter.GetNewSeqSelectedSeries(salesid, salesHisFlg)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            rwNewSeq = dtNewSeq.Rows(0)

            '2016/11/08 TCS 河原 TR-SLT-TMT-20161020-001 START
            If rwNewSeq.IsSEQNONull Then
                'NULLの場合、希望車が存在しないため初期値の1を設定
                retSeqno = 1
            Else
                retSeqno = rwNewSeq.SEQNO
            End If

            '一押し希望車フラグ(商談見込み度コードを利用)
            Dim mostPerfCd As String = " " '一押しで無い場合は半角スペースとする。
            mostPerfCd = GetSysEnvSettingValue(ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD)

            '一押し希望車フラグの解除
            Dim insertCnt As Integer = 0

            insertCnt = ActivityInfoTableAdapter.ClearSalesProspectCd(rwEstVclInfo.FLLWUPBOX_SEQNO, updateAccount, salesHisFlg, mostPerfCd)

            '更新失敗時は、SeqNoを-1で返却
            If 0 > insertCnt Then
                Return -1
            End If

            '未存在希望車種の登録
            insertCnt = ActivityInfoTableAdapter.InsertNotRegSelectedSeries(rwEstVclInfo, _
                                                                            retSeqno, _
                                                                            updateAccount, _
                                                                            salesHisFlg,
                                                                            mostPerfCd)
            '登録失敗時は、SeqNoを-1で返却
            If 0 = insertCnt Then
                Return -1
            End If

            '2016/11/08 TCS 河原 TR-SLT-TMT-20161020-001 END

            'インスタンス解放
            rwEstVclInfo = Nothing
            dtEstVclInfo = Nothing
            rwNewSeq = Nothing
            dtNewSeq = Nothing
        Else
            '=== 希望車種の既存シーケンスが存在 ===

            'シーケンス取得
            rwExistSeq = dtExistSeq.Rows(0)
            If Not rwExistSeq.IsSEQNONull Then
                retSeqno = rwExistSeq.SEQNO
            End If
        End If

        'インスタンス解放
        rwExistSeq = Nothing
        dtExistSeq = Nothing

        Logger.Info("InsertNotRegSelectedSeries End")

        Return retSeqno

    End Function


    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定実績フラグの更新
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <param name="strcd">セッション値の店舗コード</param>
    ''' <param name="fllwupboxseqno">セッション値のFollow-up Box内連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateActAsmFlg(ByVal dlrcd As String, ByVal strcd As String, _
                                                ByVal fllwupboxseqno As Decimal, ByVal updateAccount As String, _
                                                    ByVal updateId As String) As Boolean

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("UpdateActAsmFlg Start")

        'ログ出力
        Dim paramvalue As String = String.Empty
        paramvalue = "Param dlrcd=" & dlrcd & _
                     ", strcd=" & strcd & _
                     ", fllwupboxseqno=" & fllwupboxseqno & _
                     ", updateAccount=" & updateAccount & _
                     ", updateId=" & updateId
        Logger.Info(paramvalue)

        '更新件数
        Dim updateCnt As Integer = 0

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        '査定実績フラグの更新
        updateCnt = ActivityInfoTableAdapter.UpdateActAsmFlg(fllwupboxseqno, _
                                                             updateAccount, _
                                                             updateId)
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
        '更新失敗時は、Falseで返却
        If 0 = updateCnt Then
            Return False
        End If

        Logger.Info("UpdateActAsmFlg End")

        Return True

    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積実績フラグの更新
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <param name="strcd">セッション値の店舗コード</param>
    ''' <param name="fllwupboxseqno">セッション値のFollow-up Box内連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateActEstFlg(ByVal dlrcd As String, ByVal strcd As String, _
                                                ByVal fllwupboxseqno As Decimal, ByVal updateAccount As String, _
                                                    ByVal updateId As String) As Boolean

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("UpdateActEstFlg Start")

        'ログ出力
        Dim paramvalue As String = String.Empty
        paramvalue = "Param dlrcd=" & dlrcd & _
                     ", strcd=" & strcd & _
                     ", fllwupboxseqno=" & fllwupboxseqno & _
                     ", updateAccount=" & updateAccount & _
                     ", updateId=" & updateId
        Logger.Info(paramvalue)

        '更新件数
        Dim updateCnt As Integer = 0

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        '見積実績フラグの更新
        updateCnt = ActivityInfoTableAdapter.UpdateActEstFlg(fllwupboxseqno, _
                                                             updateAccount, _
                                                             updateId)
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
        '更新失敗時は、Falseで返却
        If 0 = updateCnt Then
            Return False
        End If

        Logger.Info("UpdateActEstFlg End")

        Return True

    End Function

    ''' 2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未存在選択車種登録
    ''' </summary>
    ''' <param name="registRw">登録する入力データ</param>
    ''' <remarks></remarks>
    Private Shared Sub InsertSelectedSeries(ByRef registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow)

        '配列作成
        Dim wkary As String()
        Dim tempary As String()
        Dim estimateId As Long
        Dim seqno As Long
        Dim carList As String
        Dim context As StaffContext = StaffContext.Current
        Dim account As String = context.Account     '自身のアカウント

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        '見積作成実績
        carList = ""
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        wkary = registRw.SELECTACTVALUATION.Split(";"c)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            'プロセス(見積)で選択された車種のうち、選択車種レコードが存在しない場合
            If String.Equals(tempary(1), "1") And String.Equals(tempary(2), "0") Then
                '見積管理IDを取得
                estimateId = CType(tempary(4), Long)

                '選択車種登録
                'note:選択車種が存在する場合は、登録はしないがシーケンスNoだけ取得
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                seqno = ActivityInfoBusinessLogic.InsertNotRegSelectedSeries(estimateId, account, registRw.SALESID)
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

                tempary(0) = CType(seqno, String)
            End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            carList = carList & tempary(0) & "," & tempary(1) & "," & tempary(5) & ";"
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Next

        '結果を反映
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        registRw.SELECTACTVALUATION = carList
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

    End Sub
    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定実績登録区分判定
    ''' </summary>
    ''' <param name="selectActAsmBottom">査定実績ボタンの押下状態</param>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <param name="fllwUpBoxSeqNo">Follow-up Box内連番</param>
    ''' <param name="assessmentNo">依頼No</param>
    ''' <returns>0:活動履歴登録無し/1:活動履歴登録有り/2:活動履歴退避登録</returns>
    ''' <remarks>査定依頼機能使用可否判定と査定実績判定から、査定の活動履歴を登録する区分を設定します。</remarks>
    Private Shared Function GetRegActAsmStatus(ByVal selectActAsmBottom As String,
                                               ByVal dlrCd As String, ByVal strCd As String, ByVal fllwUpBoxSeqNo As Decimal,
                                               ByRef assessmentNo As Long
                                               ) As String
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim retCd As String = C_ASMACTSTATUS_NASI

        '査定依頼機能使用可否判定
        If IsRegAsm(dlrCd) Then
            '査定実績判定
            retCd = IsActAsm(dlrCd, strCd, fllwUpBoxSeqNo, assessmentNo, True)
        Else
            '査定実績ボタンの押下状態を反映
            retCd = selectActAsmBottom
        End If

        Return retCd
    End Function
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END


#Region "Aカード情報相互連携開発"
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) START
    ''' <summary>
    ''' 必須チェック
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="msgId">メッセージID（返却用）</param>
    ''' <param name="msgItem0">メッセージ置換項目（返却用）</param>
    ''' <param name="bookingFlg">受注状態フラグ(True：受注済み(受注時・受注後 SC3080216呼び出し)/False：受注前 SC3080203呼び出し)</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Public Shared Function MandatoryCheck(ByVal dlrCD As String, ByVal brnCD As String, ByVal cstId As Decimal, ByVal salesId As Decimal, ByRef msgId As Integer, ByRef msgItem0 As String, bookingFlg As Boolean) As Boolean
        '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) END
        '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

        Dim cstInfo As ActivityInfoDataSet.CustomerInfoForCheckDataTable = Nothing
        Dim salesInfo As ActivityInfoDataSet.SalesInfoForCheckDataTable = Nothing
        Dim preferVclInfo As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable = Nothing
        Dim inputCheckSettings As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable = Nothing

        Try
            '顧客情報取得
            cstInfo = ActivityInfoTableAdapter.GetCustomerInfoForCheck(dlrCD, cstId)
            '希望車取得
            preferVclInfo = ActivityInfoTableAdapter.GetSelectedSeries(dlrCD, brnCD, String.Empty, salesId)
            '商談情報取得
            salesInfo = ActivityInfoTableAdapter.GetSalesInfoForCheck(salesId)

            '2014/02/12 TCS 高橋、山口 受注後フォロー機能開発 START
            If salesInfo.Count = 0 Then
                '受注後工程の場合、Historyから商談情報を取得する
                salesInfo = ActivityInfoTableAdapter.GetSalesHistInfoForCheck(salesId)
            End If
            '2014/02/12 TCS 高橋、山口 受注後フォロー機能開発 END

            '入力チェック設定取得
            inputCheckSettings = ActivityInfoTableAdapter.GetSettingsInputCheck(INPUT_CHECK_TIMING)

            If Not salesInfo Is Nothing AndAlso salesInfo.Count = 0 Then Return True '0件 = HISTORYへ移動済み のため入力チェック不要

            '顧客情報
            With cstInfo(0)
                'FirstName(固定チェック)
                If (.IsFIRST_NAMENull OrElse .FIRST_NAME.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_FIRSTNAME
                    Return False
                End If
                'MiddleName
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_MIDDLENAME) AndAlso (.IsMIDDLE_NAMENull OrElse .MIDDLE_NAME.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_MIDDLENAME
                    Return False
                End If
                'LastName
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_LASTNAME) AndAlso (.IsLAST_NAMENull OrElse .LAST_NAME.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_LASTNAME
                    Return False
                End If
                '性別
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_GENDER) AndAlso (.IsCST_GENDERNull OrElse .CST_GENDER.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_GENDER
                    Return False
                End If
                '敬称
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_NAMETITLE) AndAlso (.IsNAMETITLE_CDNull OrElse .NAMETITLE_CD.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_NAMETITLE
                    Return False
                End If
                '個人／法人
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FLEET) AndAlso (.IsFLEET_FLGNull OrElse .FLEET_FLG.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_FLEET
                    Return False
                End If
                '個人法人詳細項目
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FLEETDETAIL) AndAlso (.IsPRIVATE_FLEET_ITEM_CDNull OrElse .PRIVATE_FLEET_ITEM_CD.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_FLEETDETAIL
                    Return False
                End If

                '法人のみの入力欄
                If .FLEET_FLG = "1" Then
                    '担当者氏名(法人)
                    If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FLEET_PIC_NAME) AndAlso (.IsFLEET_PIC_NAMENull OrElse .FLEET_PIC_NAME.Trim().Length = 0) Then
                        msgId = ERRMSG_ID_CST_FLEET_PIC_NAME
                        Return False
                    End If
                    '担当者部署名(法人)
                    If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FLEET_PIC_DEPT) AndAlso (.IsFLEET_PIC_DEPTNull OrElse .FLEET_PIC_DEPT.Trim().Length = 0) Then
                        msgId = ERRMSG_ID_CST_FLEET_PIC_DEPT
                        Return False
                    End If
                    '役職(法人)
                    If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FLEET_PIC_POSITION) AndAlso (.IsFLEET_PIC_POSITIONNull OrElse .FLEET_PIC_POSITION.Trim().Length = 0) Then
                        msgId = ERRMSG_ID_CST_FLEET_PIC_POSITION
                        Return False
                    End If
                End If

                '自宅電話番号または携帯番号(固定チェック)
                If (.IsCST_MOBILENull OrElse .CST_MOBILE.Trim().Length = 0) AndAlso (.IsCST_PHONENull OrElse .CST_PHONE.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_TEL
                    Return False
                End If

                '勤務先電話番号
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_BIZ_PHONE) AndAlso (.IsCST_BIZ_PHONENull OrElse .CST_BIZ_PHONE.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_BIZ_PHONE
                    Return False
                End If
                '自宅FAX番号
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_FAX) AndAlso (.IsCST_FAXNull OrElse .CST_FAX.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_FAX
                    Return False
                End If
                '郵便番号
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ZIPCD) AndAlso (.IsCST_ZIPCDNull OrElse .CST_ZIPCD.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ZIPCD
                    Return False
                End If
                '住所1
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_1) AndAlso (.IsCST_ADDRESS_1Null OrElse .CST_ADDRESS_1.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_1
                    Return False
                End If
                '住所2
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_2) AndAlso (.IsCST_ADDRESS_2Null OrElse .CST_ADDRESS_2.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_2
                    Return False
                End If
                '住所3
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_3) AndAlso (.IsCST_ADDRESS_3Null OrElse .CST_ADDRESS_3.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_3
                    Return False
                End If
                '住所(州)
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_STATE) AndAlso (.IsCST_ADDRESS_STATENull OrElse .CST_ADDRESS_STATE.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_STATE
                    Return False
                End If
                '住所(地域)
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_DISTRICT) AndAlso (.IsCST_ADDRESS_DISTRICTNull OrElse .CST_ADDRESS_DISTRICT.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_DISTRICT
                    Return False
                End If
                '住所(市)
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_CITY) AndAlso (.IsCST_ADDRESS_CITYNull OrElse .CST_ADDRESS_CITY.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_CITY
                    Return False
                End If
                '住所(地区)
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_ADDRESS_LOCATION) AndAlso (.IsCST_ADDRESS_LOCATIONNull OrElse .CST_ADDRESS_LOCATION.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_ADDRESS_LOCATION
                    Return False
                End If
                '本籍
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_DOMICILE) AndAlso (.IsCST_DOMICILENull OrElse .CST_DOMICILE.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_DOMICILE
                    Return False
                End If
                'e-Mail1
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_EMAIL_1) AndAlso (.IsCST_EMAIL_1Null OrElse .CST_EMAIL_1.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_EMAIL_1
                    Return False
                End If
                'e-Mail2
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_EMAIL_2) AndAlso (.IsCST_EMAIL_2Null OrElse .CST_EMAIL_2.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_EMAIL_2
                    Return False
                End If
                '国籍
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_COUNTRY) AndAlso (.IsCST_COUNTRYNull OrElse .CST_COUNTRY.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_COUNTRY
                    Return False
                End If
                '国民ID
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_SOCIALNUM) AndAlso (.IsCST_SOCIALNUMNull OrElse .CST_SOCIALNUM.Trim().Length = 0) Then
                    msgId = ERRMSG_ID_CST_SOCIALNUM
                    Return False
                End If
                '誕生日
                If IsMandatory(inputCheckSettings, CHECKITEM_ID_CST_BIRTH_DATE) AndAlso (.IsCST_BIRTH_DATENull OrElse .CST_BIRTH_DATE.Equals(Date.Parse("1900/01/01"))) Then
                    msgId = ERRMSG_ID_CST_BIRTH_DATE
                    Return False
                End If

                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                '以下、未取引客の場合必須
                If String.Equals(.CST_TYPE, NEWCUSTFLG) Then
                    '顧客タイプ
                    If String.IsNullOrWhiteSpace(.FLEET_FLG) Then
                        msgId = ERRMSG_ID_CST_FLEET
                        Return False
                    End If

                    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) DELETE

                End If
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            End With

            '活動区分(全所有車のチェックを実施)
            If IsMandatory(inputCheckSettings, CHECKITEM_ID_ACT_CAT_TYPE) Then
                For Each dr As ActivityInfoDataSet.CustomerInfoForCheckRow In cstInfo.Rows
                    If (dr.IsACT_CAT_TYPENull OrElse dr.ACT_CAT_TYPE.Trim().Length = 0) Then
                        msgId = ERRMSG_ID_ACT_CAT_TYPE
                        Return False
                    End If
                Next
            End If

            '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) START
            '希望車チェックは受注前のみ
            If Not bookingFlg Then
                '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) END

                '希望車1件以上選択必須(固定チェック)
                If Not preferVclInfo Is Nothing And preferVclInfo.Count = 0 Then
                    msgId = ERRMSG_ID_PREFER_VCL
                    Return False
                Else
                    '全希望車のチェックを実施
                    For Each dr As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In preferVclInfo.Rows
                        '希望車種モデル
                        If IsMandatory(inputCheckSettings, CHECKITEM_ID_PREFER_VCL_MODEL) AndAlso (dr.IsMODELCDNull OrElse dr.MODELCD.Trim().Length = 0) Then
                            msgId = ERRMSG_ID_PREFER_VCL_MODEL
                            Return False
                        End If
                        '希望車種カラー
                        If IsMandatory(inputCheckSettings, CHECKITEM_ID_PREFER_VCL_BODYCLR) AndAlso (dr.IsCOLORCDNull OrElse dr.COLORCD.Trim().Length = 0) Then
                            msgId = ERRMSG_ID_PREFER_VCL_BODYCLR
                            Return False
                        End If
                    Next
                End If

                '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) START
            End If
            '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) END

            '商談情報
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            With salesInfo(0)
                'TKMローカル対応でソース1、2は入力項目マスタの設定によらず必ず必須入力チェックを実施
                'ソース1
                If Not .IsSOURCE_1_CDNull Then
                    If .SOURCE_1_CD = 0 Then
                        msgId = ERRMSG_ID_SOURCE_1_CD
                        Return False
                    End If
                End If

                'ソース2
                If Not .IsSOURCE_2_CDNull Then
                    If .SOURCE_2_CD = 0 Then
                        msgId = ERRMSG_ID_SOURCE_2_CD
                        Return False
                    End If
                End If
            End With
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END

            '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD START
            '商談条件チェックは受注前、受注時に実施
            Using dt As New ActivityInfoDataSet.ActivityInfoCountFromDataTable()
                dt.AddActivityInfoCountFromRow(dlrCD, brnCD, salesId)
                Dim salesafterflg As String = ActivityInfoBusinessLogic.CountFllwupboxRslt(dt)

                If Not salesafterflg.Equals(ActivityInfoBusinessLogic.SALESAFTER_YES) Then
                    '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD END
                    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
                    '商談条件
                    Return MandatoryCheckSalesConditions(INPUT_CHECK_TIMING, salesId, msgId, msgItem0)
                    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
                    '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD START
                End If
            End Using
            '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD END

        Finally
            If Not inputCheckSettings Is Nothing Then inputCheckSettings.Dispose()
            If Not cstInfo Is Nothing Then cstInfo.Dispose()
            If Not preferVclInfo Is Nothing Then preferVclInfo.Dispose()
            If Not salesInfo Is Nothing Then salesInfo.Dispose()
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 必須チェック設定取得
    ''' </summary>
    ''' <param name="checkSettings">チェック設定テーブル</param>
    ''' <param name="itemId">チェック設定項目ID</param>
    ''' <returns>True：必須対象/False：必須対象外</returns>
    ''' <remarks></remarks>
    Public Shared Function IsMandatory(ByVal checkSettings As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable, ByVal itemId As String) As Boolean
        Dim ret As Boolean = False
        checkSettings.DefaultView.RowFilter = String.Format("TGT_ITEM_ID='{0}'", itemId)
        ret = (checkSettings.DefaultView.Count > 0 AndAlso CType(checkSettings.DefaultView(0).Row, ActivityInfoDataSet.ActivityInfoSettingsInputCheckRow).IS_CHECKTARGET)
        Return ret
    End Function

    ''' <summary>
    ''' DMS連携（SA01）実施
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="account">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>システム環境設定にて蓋閉め可能</remarks>
    Public Shared Function SyncDmsSA01(ByVal salesId As Decimal, ByVal account As String) As Boolean

        'SA01連携が有効の場合
        If String.Equals(GetSysEnvSettingValue(ENVSETTINGKEY_SA01_ENABLED), "1") Then
            Dim aCardNum As String = String.Empty

            '活動情報連携(SA01)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "SA01_Start (SalesID:{0})", salesId), True)
            Dim sa01 As New IC3802801BusinessLogic
            Dim result As Boolean = sa01.Main(salesId)
            If (result) Then
                aCardNum = sa01.Main_ACardNo()
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "SA01_End (A-CardNum:{0})", aCardNum), True)

            'Aカード番号更新
            If String.IsNullOrEmpty(aCardNum) OrElse aCardNum.Trim().Length = 0 Then
                Return False
            End If
            If Not ActivityInfoTableAdapter.UpdateSalesACardNum(salesId, aCardNum, account) Then
                Return False
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' チェック対象セールススタッフのチームコード
    ''' </summary>
    ''' <param name="targetStfCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsMyTeamMember(targetStfCd As String) As Boolean

        If Not StaffContext.Current.TeamLeader Then Return False
        Dim dt As New ActivityInfoDataSet.MyBranchOrganizationsDataTable

        Try
            With StaffContext.Current
                dt = ActivityInfoTableAdapter.GetMyBranchOrganizations(.DlrCD, .BrnCD, targetStfCd, .TeamCD)
            End With
            dt.DefaultView.RowFilter = "IsSalesStaffOrgnz = 'True'"

            If dt.DefaultView.Count = 0 Then Return False
            If CType(dt.DefaultView(0).Row, ActivityInfoDataSet.MyBranchOrganizationsRow).IsTeamLeaderOrgnz Then Return True

            '再起処理にてセールス担当の上位組織にTLの所属組織が含まれるか判定する。
            Return IsMyTeamMember(dt, CType(dt.DefaultView(0).Row, ActivityInfoDataSet.MyBranchOrganizationsRow).PARENT_ORGNZ_ID)
        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try

    End Function

    Private Shared Function IsMyTeamMember(ByRef dt As ActivityInfoDataSet.MyBranchOrganizationsDataTable, ByVal parnetId As Decimal) As Boolean
        dt.DefaultView.RowFilter = "ORGNZ_ID = " & parnetId.ToString()
        If dt.DefaultView.Count = 0 Then Return False
        If CType(dt.DefaultView(0).Row, ActivityInfoDataSet.MyBranchOrganizationsRow).IsTeamLeaderOrgnz Then Return True
        '再起処理
        Return IsMyTeamMember(dt, CType(dt.DefaultView(0).Row, ActivityInfoDataSet.MyBranchOrganizationsRow).PARENT_ORGNZ_ID)
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing
        Try
            env = New SystemEnvSetting()
            dr = env.GetSystemEnvSetting(sysEnvName)
            If Not dr Is Nothing Then
                Return dr.PARAMVALUE.Trim()
            End If
        Catch ex As Exception
            Logger.Error("GetSystemEnvSetting", ex)
            Err.Clear()
        Finally
            env = Nothing
        End Try

        Return String.Empty
    End Function

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    ''' <summary>
    ''' 必須チェック（商談条件）
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>商談条件の必須チェックを実行</remarks>
    Public Shared Function MandatoryCheckSalesConditions(ByVal checkTimingType As String, ByVal salesId As Decimal, ByRef msgId As Integer, ByRef msgItem0 As String) As Boolean
        Dim ret As Boolean = True
        Dim dt As ActivityInfoDataSet.SalesConditionsForCheckDataTable = Nothing

        Try
            dt = ActivityInfoTableAdapter.GetSalesConditionsForCheck(salesId, checkTimingType, CHECKITEM_ID_SALESCONDITION)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                dt.DefaultView.RowFilter = "[IsMandatory] = 'True' AND [SelectedValues] = 0 "
                If dt.DefaultView.Count > 0 Then
                    msgId = ERRMSG_ID_SALESCONDITION
                    msgItem0 = CType(dt.DefaultView(0).Row, ActivityInfoDataSet.SalesConditionsForCheckRow).TITLE
                    Return False
                End If
            End If
        Catch ex As Exception
            Logger.Error("MandatoryCheckSalesConditions_Err", ex)
            Return False
        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try

        Return ret
    End Function

    ''' <summary>
    ''' 受注後工程機能の使用可否フラグ判定
    ''' </summary>
    ''' <returns>true:受注後工程機能の使用可／false:受注後工程機能の使用可</returns>
    ''' <remarks>受注後工程機能の使用可否チェックを実行</remarks>
    Public Shared Function CheckUsedB2D() As Boolean

        Dim dt As ActivityInfoDataSet.SalesConditionsForCheckDataTable = Nothing

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        '受注後工程機能の使用可否フラグ("1"の場合に使用可)
        sysEnvRow = sysEnv.GetSystemEnvSetting(ENVSETTINGKEY_USE_B2D_FUNCTION)
        If ("1".Equals(sysEnvRow.PARAMVALUE)) Then
            Return True
        Else
            Return False
        End If

    End Function

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
#End Region

#Region "受注後フォロー機能開発"
    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注後プロセスマスタ取得
    ''' </summary>
    ''' <returns>受注後プロセスマスタ情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterProcessMaster() As ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable
        Return ActivityInfoTableAdapter.GetBookedAfterProcessMaster(GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_PRCS_OTHER))
    End Function

    ''' <summary>
    ''' 受注後プロセス実績取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>受注後プロセス実績情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterProcessResult(ByVal salesId As Decimal) As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable
        Return ActivityInfoTableAdapter.GetBookedAfterProcessResult(salesId, GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_PRCS_OTHER))
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2014/08/05 TCS 森   受注後活動A⇒H退避対応 START
    ''' <summary>
    ''' 受注後工程情報移行処理
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="moduleId">更新機能ID</param>
    ''' <returns>処理結果 True(固定)</returns>
    ''' <remarks></remarks>
    Shared Function MoveAfterOrderProcInfo(ByVal salesId As Decimal, ByVal account As String, ByVal moduleId As String) As Boolean

        '受注後工程情報移行処理
        Logger.Info("MoveAfterOrderProcInfo Strat")

        '受注後ロック取得
        Dim AfterActTable As ActivityInfoDataSet.ActivityInfoGetAfterActDataTable = ActivityInfoTableAdapter.GetLockAfterOdr(salesId)

        If AfterActTable.Count > 0 Then

            '受注後活動が1件以上存在する場合
            '受注後活動ロック取得
            ActivityInfoTableAdapter.GetLockAfterOdrAct(AfterActTable.Item(0).AFTER_ODR_ID)

            '受注後History移行
            ActivityInfoTableAdapter.MoveHistoryAfterOdr(salesId, account, moduleId)

            '受注後活動History移行
            ActivityInfoTableAdapter.MoveHistoryAfterOdrAct(AfterActTable.Item(0).AFTER_ODR_ID, account, moduleId)

            '受注後活動削除
            ActivityInfoTableAdapter.DeleteAfterOdrAct(AfterActTable.Item(0).AFTER_ODR_ID)

            '契約条件移行対象ロック取得
            ActivityInfoTableAdapter.GetLockAfterOrdContract(AfterActTable.Item(0).AFTER_ODR_ID)

            '契約条件History移行
            ActivityInfoTableAdapter.MoveHistoryAfterOrdContract(AfterActTable.Item(0).AFTER_ODR_ID, account, moduleId)

            '契約条件移行元削除
            ActivityInfoTableAdapter.DeleteAfterOrdContract(AfterActTable.Item(0).AFTER_ODR_ID)

            '予定変更履歴移行対象ロック取得
            ActivityInfoTableAdapter.GetLockAfterOrdHis(AfterActTable.Item(0).AFTER_ODR_ID)

            '予定変更履歴History移行
            ActivityInfoTableAdapter.MoveHistoryAfterOrdHis(AfterActTable.Item(0).AFTER_ODR_ID, account, moduleId)

            '予定変更履歴移行元削除
            ActivityInfoTableAdapter.DeleteAfterOrdHis(AfterActTable.Item(0).AFTER_ODR_ID)

            '受注後必要書類移行対象ロック取得
            ActivityInfoTableAdapter.GetLockAfterOrdDoc(AfterActTable.Item(0).AFTER_ODR_ID)

            '受注後必要書類History移行
            ActivityInfoTableAdapter.MoveHistoryAfterOrdDoc(AfterActTable.Item(0).AFTER_ODR_ID, account, moduleId)

            '受注後必要書類移行元削除
            ActivityInfoTableAdapter.DeleteAfterOrdDoc(AfterActTable.Item(0).AFTER_ODR_ID)

            '受注後削除
            ActivityInfoTableAdapter.DeleteAfterOdr(salesId)

        End If

        Logger.Info("MoveAfterOrderProcInfo End")

        Return True

    End Function
    '2014/08/05 TCS 森   受注後活動A⇒H退避対応 END
#End Region


    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）START

    ''' <summary>
    ''' コンタクト履歴取得
    ''' </summary>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="tabIndex">検索対象のタブ</param>
    ''' <param name="vin">VIN</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>コンタクト履歴を取得する処理</remarks>
    Public Shared Function GetContactHistoryData(ByVal customerClass As String, _
                                             ByVal crcustId As String, _
                                             ByVal dlrCD As String, _
                                             ByVal cstKind As String, _
                                             ByVal newCustId As String, _
                                             ByVal tabIndex As String, _
                                             ByVal vin As String) As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable
        Logger.Info("GetContactHistoryData Start")

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Dim GetContactHistoryDt As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable
        GetContactHistoryDt = ActivityInfoTableAdapter.GetContactHistory(customerClass, crcustId, dlrCD, cstKind, newCustId, tabIndex, vin)

        '2014/11/20 TCS 河原  TMT B案 START

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim Setting_Val As String
        Dim systemBiz As New SystemSettingDlr

        '①販売店≠'XXXXX'、店舗＝'XXX'（販売店コードのみ該当）
        '②販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし)  
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrCD, ConstantBranchCD.AllBranchCD, C_ICROP_OLD_SYSTEM_DISP_FLG)
        If drSettingDlr Is Nothing Then
            'DataRowそのものがNULLの時のみ0にする
            Setting_Val = "0"
        Else
            Setting_Val = drSettingDlr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 START
        'サービスのV3履歴は参照しない
        If String.Equals(Setting_Val, "1") And Not String.Equals(tabIndex, ActivityInfoTableAdapter.CONTACTHISTORY_TAB_SERVICE) Then
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 END
            'V3の顧客IDを取得
            Dim V3cst_IDDt As New ActivityInfoDataSet.ActivityInfoCst_CDDataTable
            V3cst_IDDt = ActivityInfoTableAdapter.GetV3CustomerCD(CDec(crcustId), dlrCD)
            Dim V3cst_id As String = Nothing
            Dim V3NewCstCD As String = Nothing
            Dim V3VIN As String = vin
            Dim V3cst_idRw As ActivityInfoDataSet.ActivityInfoCst_CDRow
            V3cst_idRw = CType(V3cst_IDDt.Rows(0), ActivityInfoDataSet.ActivityInfoCst_CDRow)
            V3cst_id = V3cst_idRw.CST_CD

            If Not String.IsNullOrEmpty(Trim(V3cst_id)) Then
                Dim V3NewCstCDRW As ActivityInfoDataSet.ActivityInfoCst_CDRow = Nothing
                If String.Equals(V3cst_idRw.CST_TYPE, ORGCUSTFLG) Then
                    '自社客の場合、未取引客CDを取得
                    Dim V3NewCstCDDt As ActivityInfoDataSet.ActivityInfoCst_CDDataTable
                    V3NewCstCDDt = ActivityInfoTableAdapter.GetV3NewCustomerCD(V3cst_id)
                    If V3NewCstCDDt.Count() > 0 Then
                        V3NewCstCDRW = CType(V3NewCstCDDt.Rows(0), ActivityInfoDataSet.ActivityInfoCst_CDRow)
                        V3NewCstCD = V3NewCstCDRW.CST_CD
                    End If
                End If
                Dim GetV3ContactHistoryDt As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable
                GetV3ContactHistoryDt = ActivityInfoTableAdapter.GetV3ContactHistory(customerClass, V3cst_id, dlrCD, cstKind, V3NewCstCD, tabIndex, V3VIN)

                GetContactHistoryDt.Merge(GetV3ContactHistoryDt)
            End If

        End If

        '並び替えを実施
        Dim GetContactHistoryDv As DataView = New DataView(GetContactHistoryDt)
        GetContactHistoryDv.Sort = "ACTUALDATE DESC"

        Dim GetContactHistoryDt2 As New ActivityInfoDataSet.ActivityInfoContactHistoryDataTable
        For Each drv As DataRowView In GetContactHistoryDv
            GetContactHistoryDt2.ImportRow(drv.Row)
        Next

        'コンタクト履歴取得
        Return GetContactHistoryDt2

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        '2014/11/20 TCS 河原  TMT B案 END
        Logger.Info("GetContactHistoryData End")
    End Function

    ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注後活動内容取得
    ''' </summary>
    ''' <param name="actidList">活動IDのリスト</param>
    ''' <param name="afterOdrFllwSeqList">受注後工程フォロー結果連番のリスト</param>
    ''' <returns>受注後活動内容</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactAfterOdrAct(actidList As List(Of Decimal), _
                                                 afterOdrFllwSeqList As List(Of Decimal) _
                                                 ) As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable

        Logger.Info("GetContactAfterOdrAct Start")

        '受注後活動内容を取得
        Dim dt As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable _
            = ActivityInfoTableAdapter.GetContactAfterOdrAct(actidList, afterOdrFllwSeqList)

        Logger.Info("GetContactAfterOdrAct End")

        Return dt
    End Function
    ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 END

    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' 対象の入庫履歴の詳細情報を取得
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <param name="vin"></param>
    ''' <param name="dlrcd"></param>
    ''' <returns>取得結果</returns>
    ''' <remarks>対象の入庫履歴の詳細情報を取得</remarks>
    Public Shared Function GetServiceInInfo(ByVal originalid As String, _
                                            ByVal vin As String, _
                                            ByVal dlrcd As String) As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        Logger.Info("GetServiceInInfo Start")
        'コンタクト履歴取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (ActivityInfoTableAdapter.GetServiceInInfo(originalid, vin, dlrcd))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Logger.Info("GetServiceInInfo End")
    End Function

    ''' <summary>
    ''' 基幹システム名を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBasesystemNM() As ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable
        Logger.Info("GetBasesystemNM Start")
        'コンタクト履歴取得
        Return (ActivityInfoTableAdapter.GetBasesystemNM())
        Logger.Info("GetBasesystemNM End")
    End Function
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）END

    ''' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
    ''' <summary>
    ''' 商談活動追加
    ''' </summary>
    ''' <param name="actid">活動ID</param>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="account">活動実施者</param>
    ''' <param name="actmtd">コンタクト方法のコード</param>
    ''' <param name="salesversion">商談ロック</param>
    ''' <param name="assessment">査定実績</param>
    ''' <param name="actdayto"></param>
    ''' <param name="selectactvaluation">希望車種のSEQのリスト</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function InsertSalesActContracted(ByVal actid As Decimal, ByVal salesid As Decimal, ByVal account As String, _
                                             ByVal actmtd As String, ByVal salesversion As Long, _
                                             ByVal assessment As String, ByVal actdayto As String, _
                                             ByVal selectactvaluation As String()) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesActContracted Start")
        '-----------------------------------------------------------------

        Dim context As StaffContext = StaffContext.Current
        '販売店コード
        Dim dlrcd As String = context.DlrCD
        '店舗コード
        Dim strcd As String = context.BrnCD

        '保持情報（スタッフコード）
        Dim staffcd As String = context.Account

        '機能識別子
        Dim rowfunction As String = SC3080203_MODULEID

        '配列作成
        Dim tempary As String()
        Dim wkary As String()
        Dim seqdt As ActivityInfoSeqDataTable
        Dim seqrw As ActivityInfoSeqRow

        Dim selockversion As String = ""
        Dim valuation As String = ""
        Dim lockversion As String = ""
        Dim valuationPrice As String = ""

        Dim orgnzid As Decimal
        orgnzid = ActivityInfoTableAdapter.GetorgnzId(staffcd)

        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        seqdt = ActivityInfoTableAdapter.GetActHisCarSeq(salesid)
        For j As Integer = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
            selcar = selcar & seqrw.SEQNO & ","
            selockversion = selockversion & seqrw.LOCKVERSION & ","
        Next

        '活動実績登録用の希望車種のリストを作成
        valuation = ""
        lockversion = ""
        valuationPrice = ""
        '希望車種のSEQのリスト
        wkary = selectactvaluation
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(salesid, tempary(0), "4")
            For j = 0 To seqdt.Count - 1
                seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                valuation = valuation & seqrw.SEQNO & ","
                lockversion = lockversion & seqrw.LOCKVERSION & ","
            Next
            valuationPrice = valuationPrice & tempary(2) & ","
        Next

        Dim selcarary As String() = selcar.Split(","c)
        Dim seVersion As String() = selockversion.Split(","c)
        Dim valuationary As String() = valuation.Split(","c)
        Dim valuationaryPrice As String() = valuationPrice.Split(","c)
        Dim valVersion As String() = lockversion.Split(","c)

        Dim actHisSelCardt As ActivityInfoDataSet.ActivityInfoActHisSelCarDataTable

        '希望車種の台数分ループ
        For i = 0 To selcarary.Length - 2

            '希望車種の情報取得
            actHisSelCardt = ActivityInfoTableAdapter.GetActHisCarSeq(dlrcd, salesid, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()))

            Dim cractrslt As String = " "
            Dim lock_ver As Long = CLng(seVersion(i))
            Dim req As Integer

            If actHisSelCardt.Rows.Count > 0 Then

                Dim estPrice As Long = 0
                '希望車の見積金額合計
                estPrice = CLng(valuationaryPrice(0))

                Dim ContractFlag As Integer = ActivityInfoTableAdapter.GetEstimateContractFlg(salesid, selcarary(i))
                Dim salesbkgnum As String
                salesbkgnum = " "
                '見積状況フラグが立っている場合
                If (ContractFlag = 1) Then
                    cractrslt = CRACTRESULT_SUCCESS
                    '見積の成約フラグを更新
                    ActivityInfoTableAdapter.GetEstimateLock(salesid, selcarary(i))
                    req = ActivityInfoTableAdapter.UpdateSuccessFlag(salesid, selcarary(i), account, rowfunction)
                    salesbkgnum = ActivityInfoTableAdapter.GetSalesbkgNum(salesid, selcarary(i))
                    If String.IsNullOrEmpty(salesbkgnum) Then
                        salesbkgnum = " "
                    End If
                Else
                    cractrslt = CRACTRESULT_GIVEUP
                End If

                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
                '希望車のステータスを更新
                req = ActivityInfoTableAdapter.UpdateSalesstatus(salesid, selcarary(i), cractrslt, account, rowfunction, lock_ver, actid, salesbkgnum)
                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END

                If req = 0 Then
                    Return False
                End If

            End If
        Next

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesActContracted End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' 受注後工程利用フラグ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns>afterOdrProcFlg（0:受注後工程を利用しない 1:受注後工程を利用する）</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOdrProcFlg(ByVal dlrcd As String, ByVal brncd As String) As String

        Logger.Info("GetAfterOdrProcFlg Start")

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '①販売店≠'XXXXX'、店舗≠'XXX'（販売店コード・店舗コード該当）
        '②①実行でデータがなければ販売店≠'XXXXX'、店舗＝'XXX'（販売店コードのみ該当）
        '③①②実行でデータがなければ販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim systemBiz As New SystemSettingDlr
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrcd, brncd, C_USE_AFTER_ODR_PROC_FLG)

        'データそのものが取れなかった場合、取得した列に値が設定されていない場合はException
        If drSettingDlr Is Nothing Then
            Throw New ArgumentException("受注後工程利用フラグ取得処理失敗")
        End If

        Return drSettingDlr.SETTING_VAL
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        Logger.Info("GetAfterOdrProcFlg End")

    End Function
    '2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' V4側のシステム設定の取得
    ''' </summary>
    ''' <param name="setting_name">項目名</param>
    ''' <returns>設定値</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSystemSetting(ByVal setting_name As String) As String
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim systemBiz As New SystemSetting
        Dim setVal As String
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(setting_name)

        If (dataRow Is Nothing) Then
            '改修前の仕様に合わせて取得できない場合は0を返す
            setVal = "0"
        Else
            setVal = dataRow.SETTING_VAL
        End If

        Return setVal
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

#End Region

End Class

