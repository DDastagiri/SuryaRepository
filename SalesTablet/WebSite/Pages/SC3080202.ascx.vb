'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080202.ascx.vb
'─────────────────────────────────────
'機能： 顧客詳細(商談情報)
'補足： 
'作成： 2011/11/24 TCS 小野
'更新： 2012/01/26 TCS 山口 【SALES_1B】
'更新： 2012/03/16 TCS 相田 【SALES_2】TCS_0315ao_03
'更新： 2012/04/24 TCS 河原 【SALES_2】営業キャンセルでシステムエラー (号口課題 No.111) 本対応
'更新： 2012/04/26 TCS 河原 HTMLエンコード対応
'更新： 2012/07/05 TCS 河原 入力エラー後プロセス欄が正しく表示されない
'更新： 2012/07/05 TCS 河原 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2012/11/22 TCS 坪根 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2013/06/30 TCS 徐   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/09 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 山口 受注後フォロー機能開発
'更新： 2014/05/07 TCS 高橋 受注後フォロー機能開発
'更新： 2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移)
'更新： 2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) 
'更新： 2014/08/29 TCS 武田 Next追加要件
'更新： 2015/03/12 TCS 藤井 セールスタブレット：0118
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2016/09/14 TCS 河原 TMTタブレット性能改善
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証
'更新： 2019/03/15 TS  高木  (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究
'更新： 2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証
'更新： 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)
'─────────────────────────────────────

Imports System.Data
' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start
Imports System.Globalization
' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
' 2012/02/29 TCS 小野 【SALES_2】 START
Imports Toyota.eCRB.CommonUtility.DataAccess
' 2012/02/29 TCS 小野 【SALES_2】 END
'2013/12/09 TCS 市川 Aカード情報相互連携開発 START
Imports Toyota.eCRB.CommonUtility.BizLogic
'2013/12/09 TCS 市川 Aカード情報相互連携開発 END

Partial Class Pages_SC3080202
    Inherits UserControl
    Implements ISC3080202Control

#Region " イベント "
    Public Event ChangeSelectedSeries(ByVal sender As Object, ByVal e As System.EventArgs) Implements ISC3080202Control.ChangeSelectedSeries
    Public Event CreateFollow(ByVal sender As Object, ByVal e As System.EventArgs) Implements ISC3080202Control.CreateFollow
    Public Event ChangeFollow(ByVal sender As Object, ByVal e As System.EventArgs) Implements ISC3080202Control.ChangeFollow
#End Region

#Region " 定数 "
    ' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
    '' 台数（規定値）
    'Public Const QUANTITY_DEFAULT As Integer = 1
    '' エラーメッセージ（希望車種重複エラー）
    'Public Const ERRMSGID_20901 As Integer = 20901
    'Public Const ERRMSGID_20902 As Integer = 20902
    'Public Const ERRMSGID_20903 As Integer = 20903
    'Public Const ERRMSGID_20904 As Integer = 20904
    'Public Const ERRMSGID_20905 As Integer = 20905
    'Public Const ERRMSGID_20906 As Integer = 20906
    'Public Const ERRMSGID_20907 As Integer = 20907
    'Public Const ERRMSGID_20908 As Integer = 20908
    'Public Const ERRMSGID_20909 As Integer = 20909
    'Public Const ERRMSGID_20910 As Integer = 20910
    'Public Const ERRMSGID_20911 As Integer = 20911
    '' メモ欄の文字制限
    'Public Const MEMOLENGTH As Integer = 256
    ''活動ステータス	$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
    'Public Const PRESENCECATEGORY_STANDBY As String = "1" '大分類 スタンバイ
    'Public Const PRESENCECATEGORY_SALES As String = "2" '大分類 商談中
    'Public Const PRESENCEDETAIL_0 As String = "0" '小分類 0
    'Public Const PRESENCEDETAIL_1 As String = "1" '小分類 1

    ' 台数（規定値）
    Private Const QUANTITY_DEFAULT As Integer = 1
    ' エラーメッセージ（希望車種重複エラー）
    Private Const ERRMSGID_20901 As Integer = 20901
    Private Const ERRMSGID_20902 As Integer = 20902
    Private Const ERRMSGID_20903 As Integer = 20903
    Private Const ERRMSGID_20904 As Integer = 20904
    Private Const ERRMSGID_20905 As Integer = 20905
    Private Const ERRMSGID_20906 As Integer = 20906
    Private Const ERRMSGID_20907 As Integer = 20907
    Private Const ERRMSGID_20908 As Integer = 20908
    Private Const ERRMSGID_20909 As Integer = 20909
    Private Const ERRMSGID_20910 As Integer = 20910
    Private Const ERRMSGID_20911 As Integer = 20911
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    Private Const ERRMSGID_20912 As Integer = 20912 '必須：グレード
    Private Const ERRMSGID_20913 As Integer = 20913 '必須：外鈑色
    Private Const ERRMSGID_20914 As Integer = 20914 '必須：用件ソース1
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除 
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
    Private Const ERRMSGID_2020918 As Integer = 2020918 '必須：用件ソース2
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    Private Const ERRMSGID_20916 As Integer = 20916 '必須：商談条件
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    Private Const ERRMSGID_20917 As Integer = 20917 '受注時説明の登録完了チェック
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END
    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    Private Const ERRMSGID_901 As Integer = 901
    '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const LC_ERRMSGID_2020901 As Integer = 2020901 '商談条件項目名：項目長
    Private Const LC_ERRMSGID_2020902 As Integer = 2020902 '商談条件項目名：未入力
    Private Const LC_ERRMSGID_2020903 As Integer = 2020903 '商談条件項目名：禁則文字

    Private Const LC_ERRMSGID_2020914 As Integer = 2020914 '購入分類：未選択
    Private Const LC_ERRMSGID_2020915 As Integer = 2020915 '下取り車両 メーカー：未選択
    Private Const LC_ERRMSGID_2020916 As Integer = 2020916 '下取り車両 モデル　：未選択
    Private Const LC_ERRMSGID_2020906 As Integer = 2020906 '下取り車両 走行距離：未入力
    Private Const LC_ERRMSGID_2020907 As Integer = 2020907 '下取り車両 走行距離：項目長
    Private Const LC_ERRMSGID_2020908 As Integer = 2020908 '下取り車両 走行距離：禁則文字
    Private Const LC_ERRMSGID_2020912 As Integer = 2020912 '下取り車両 走行距離：精度
    Private Const LC_ERRMSGID_2020917 As Integer = 2020917 '下取り車両 年式　　：未選択

    Private Const LC_WORDNO_REPLACE_TXT_ITEMTITLE As Integer = 2020001 '画面表示用の置換文字列（項目名変更アイテム）
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

    ' メモ欄の文字制限
    Private Const MEMOLENGTH As Integer = 256
    '活動ステータス	$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
    Private Const PRESENCECATEGORY_STANDBY As String = "1" '大分類 スタンバイ
    Private Const PRESENCECATEGORY_SALES As String = "2" '大分類 商談中
    Private Const PRESENCEDETAIL_0 As String = "0" '小分類 0
    Private Const PRESENCEDETAIL_1 As String = "1" '小分類 1
    ' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify End

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    Private Const PRESENCEDETAIL_2 As String = "2" '小分類 2
    Private Const BOOKEDAFTER_SALESBKG As String = "000" '受注
    Private Const BOOKEDAFTER_ALLOCATION As String = "001" '振当
    Private Const BOOKEDAFTER_PAYMENT As String = "002" '入金
    Private Const BOOKEDAFTER_DELIVERY As String = "005" '納車
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    Private Const PRESENCEDETAIL_3 As String = "3" '小分類 3
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <summary>
    ''' CR活動結果(SUCCESS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_SUCCESS As String = "3"

    ''' <summary>
    ''' 契約状況フラグ(契約済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACT As String = "1"

    ''' <summary>
    ''' サクセスフラグ(サクセス済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUCCESS_FLG_COMPLETED As String = "1"
    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    '入力チェック設定：項目コード
    Const CHECKITEM_ID_PREFER_VCL_MODEL As String = "31"        'グレード
    Const CHECKITEM_ID_PREFER_VCL_BODYCLR As String = "32"      '外鈑色
    Const CHECKITEM_ID_SOURCE_1_CD As String = "33"             '用件ソース
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除

    'セッションキー：顧客ID
    Private Const CONST_INSDID As String = "SearchKey.CRCUSTID"

    'ダミー入力フラグ
    Public Const DummyNameFlgDummy As String = "1"

    Const WORD_NUM_ACARDNUMTITLE As Decimal = 20048
    Const WORD_NUM_CONTORACTNUMTITLE As Decimal = 20052
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END    

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    '受注後プロセス表示可能数
    Private Const BOOKED_AFTER_PROCESS_COUNT As Decimal = 6D
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2014/05/07 TCS 高橋 受注後フォロー機能開発
    Private Const SUBMENU_BOOKING_EXPLAIN = 205
    '2014/05/07 TCS 高橋 受注後フォロー機能開発

    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    Private Const ACT_STATUS_DISP_FLG_ON As String = "1"
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
    Private Const FLG_FALSE = "0"
    Private Const FLG_TRUE = "1"
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
#End Region

#Region " セッションキーの定数 "
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"
    ''' <summary>FOLLOW_UP_BOX</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"
    ''' <summary>FOLLOW_UP_BOX_NEW</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_NEW As String = "SearchKey.FOLLOW_UP_BOX_NEW"
    '$01 2012/01/25 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' 商談中Follow-upBox内連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_SALES As String = "SearchKey.FOLLOW_UP_BOX_SALES"
    '$01 2012/01/25 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End
    ' 2012/02/29 TCS 小野 【SALES_2】 START
    Private Const SESSION_KEY_SALESBKGNO As String = "SearchKey.ORDER_NO"
    Private Const SESSION_KEY_NEWCUSTID As String = "SearchKey.NEW_CUST_ID"
    Private Const SESSION_KEY_SALESAFTER As String = "SearchKey.SALESAFTER"
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2013/03/06 TCS 河原 GL0874 START
    ''' <summary>商談中Follow-upBox店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD_SALES As String = "SearchKey.FLLWUPBOX_STRCD_SALES"
    '2013/03/06 TCS 河原 GL0874 END

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' 活動ID
    Private Const SESSION_KEY_ACT_ID As String = "SearchKey.ACT_ID"

    ' 用件ID
    Private Const SESSION_KEY_REQ_ID As String = "SearchKey.REQ_ID"

    ' 誘致ID
    Private Const SESSION_KEY_ATT_ID As String = "SearchKey.ATT_ID"

    ' 活動回数
    Private Const SESSION_KEY_COUNT As String = "SearchKey.COUNT"

    ' 用件ロックバージョン
    Private Const SESSION_KEY_REQUEST_LOCK_VERSION As String = "SearchKey.REQUEST_LOCK_VERSION"

    ' 誘致ロックバージョン
    Private Const SESSION_KEY_ATTRACT_LOCK_VERSION As String = "SearchKey.ATTRACT_LOCK_VERSION"

    ' 商談ロックバージョン
    Private Const SESSION_KEY_SALES_LOCK_VERSION As String = "SearchKey.SALES_LOCK_VERSION"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    'メニューロック状態
    Private Const SESSION_KEY_READONLYFLG As String = "ReadOnlyFlg"
    '見積ID
    Private Const SESSION_KEY_ESTIMATEID As String = "EstimateId"
    '選択している見積IDのindex
    Private Const SESSION_KEY_SELECTEDESTIMATEINDEX As String = "SelectedEstimateIndex"
    '未保存フラグ
    Private Const SESSION_KEY_BUSINESSFLG As String = "BusinessFlg"
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END
#End Region

#Region "変数定義"
    ' Bizクラス
    Protected bizClass As New SC3080202BusinessLogic
    ' DataTable
    ' 活動リスト取得(From)
    Protected getActivityListFromDataTable As SC3080202DataSet.SC3080202GetActivityListFromDataTable
    ' 活動リスト取得(To)
    Protected getActivityListToDataTable As SC3080202DataSet.SC3080202GetActivityListToDataTable
    ' 活動詳細取得(From)
    Protected getActivityDetailFromDataTable As SC3080202DataSet.SC3080202GetActivityDetailFromDataTable
    ' 活動詳細取得(To)
    Protected getActivityDetailToDataTable As SC3080202DataSet.SC3080202GetActivityDetailToDataTable
    ' 2012/02/29 TCS 小野 【SALES_2】 START
    '' 希望車種取得(From)
    'Protected getSelectedSeriesFromDataTable As SC3080202DataSet.SC3080202GetSelectedSeriesListFromDataTable
    '' 希望車種取得(To)
    'Protected getSelectedSeriesToDataTable As SC3080202DataSet.SC3080202GetSelectedSeriesListToDataTable
    ' 希望車種取得(From)
    Protected getSelectedSeriesFromDataTable As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable
    ' 希望車種取得(To)
    Protected getSelectedSeriesToDataTable As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
    ' 2012/02/29 TCS 小野 【SALES_2】 END
    ' 競合車種取得(From)
    Protected getSelectedCompeFromDataTable As SC3080202DataSet.SC3080202GetSelectedCompeListFromDataTable
    ' 競合車種取得(To)
    Protected getSelectedCompeToDataTable As SC3080202DataSet.SC3080202GetSelectedCompeListToDataTable
    ' 商談条件取得(From)
    Protected getSalesConditionFromDataTable As SC3080202DataSet.SC3080202GetSalesConditionFromDataTable
    ' 商談条件取得(To)
    Protected getSalesConditionToDataTable As SC3080202DataSet.SC3080202GetSalesConditionToDataTable
    ' 2012/02/29 TCS 小野 【SALES_2】 START
    '' 希望車種プロセス取得(From)
    'Protected getProcessFromDataTable As SC3080202DataSet.SC3080202GetProcessFromDataTable
    '' 希望車種プロセス取得(To)
    'Protected getProcessToDataTable As SC3080202DataSet.SC3080202GetProcessToDataTable
    '' 活動ステータス取得(From)
    'Protected getStatusFromDataTable As SC3080202DataSet.SC3080202GetStatusFromDataTable
    '' 活動ステータス取得(To)
    'Protected getStatusToDataTable As SC3080202DataSet.SC3080202GetStatusToDataTable
    ' 希望車種プロセス取得(From)
    Protected getProcessFromDataTable As ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable
    ' 希望車種プロセス取得(To)
    Protected getProcessToDataTable As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
    ' 活動ステータス取得(From)
    Protected getStatusFromDataTable As ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable
    ' 活動ステータス取得(To)
    Protected getStatusToDataTable As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
    ' 2012/02/29 TCS 小野 【SALES_2】 END
    ' 活動メモリスト取得(From)
    Protected getSalesMemoHisFromDataTable As SC3080202DataSet.SC3080202GetSalesMemoHisFromDataTable
    ' 活動メモリスト取得(To)
    Protected getSalesMemoHisToDataTable As SC3080202DataSet.SC3080202GetSalesMemoHisToDataTable
    ' 活動メモ取得(From)
    Protected getSalesMemoTodayFromDataTable As SC3080202DataSet.SC3080202GetSalesMemoTodayFromDataTable
    ' 活動メモ取得(To)
    Protected getSalesMemoTodayToDataTable As SC3080202DataSet.SC3080202GetSalesMemoTodayToDataTable
    ' シリーズマスタ取得(From)
    Protected getSeriesMasterFromDataTable As SC3080202DataSet.SC3080202GetSeriesMasterFromDataTable
    ' シリーズマスタ取得(To)
    Protected getSeriesMasterToDataTable As SC3080202DataSet.SC3080202GetSeriesMasterToDataTable
    ' モデルマスタ取得(From)
    Protected getModelMasterFromDataTable As SC3080202DataSet.SC3080202GetModelMasterFromDataTable
    ' モデルマスタ取得(To)
    Protected getModelMasterToDataTable As SC3080202DataSet.SC3080202GetModelMasterToDataTable

    '2017/11/20 TCS 河原 TKM独自機能開発 START

    ' サフィックスマスタ取得(From)
    Protected getsuffixMasterFromDataTable As SC3080202DataSet.SC3080202GetSuffixMasterFromDataTable
    ' サフィックスマスタ取得(To)
    Protected getsuffixMasterToDataTable As SC3080202DataSet.SC3080202GetSuffixMasterToDataTable


    ' 外装色マスタ取得(From)
    Protected getExteriorColorMasterFromDataTable As SC3080202DataSet.SC3080202GetExteriorColorMasterFromDataTable
    ' 外装色マスタ取得(To)
    Protected getExteriorColorMasterToDataTable As SC3080202DataSet.SC3080202GetExteriorColorMasterToDataTable


    ' 内装色マスタ取得(From)
    Protected getInteriorColorMasterFromDataTable As SC3080202DataSet.SC3080202GetInteriorColorMasterFromDataTable
    ' 内装色マスタ取得(To)
    Protected getInteriorColorMasterToDataTable As SC3080202DataSet.SC3080202GetInteriorColorMasterToDataTable

    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ' メーカマスタ取得(To)
    Protected getCompeMakerMasterToDataTable As SC3080202DataSet.SC3080202GetCompeMakerMasterToDataTable
    ' モデルマスタ取得(To)
    Protected getCompeModelMasterToDataTable As SC3080202DataSet.SC3080202GetCompeModelMasterToDataTable
    ' 商談条件更新(From)
    Protected updateSalesConditionFromDataTable As SC3080202DataSet.SC3080202UpdateSalesConditionFromDataTable
    ' 商談メモ更新(From)
    Protected updateSalesMemoFromDataTable As SC3080202DataSet.SC3080202UpdateSalesMemoFromDataTable
    ' 希望車種台数更新(From)
    Protected UpdateSelectedVclCountFromDataTable As SC3080202DataSet.SC3080202UpdateSelectedVclCountFromDataTable
    ' 希望車種情報更新(From)
    Protected UpdateSelectedSeriesFromDataTable As SC3080202DataSet.SC3080202UpdateSelectedSeriesFromDataTable
    ' 競合車種情報更新(From)
    Protected UpdateSelectedCompeFromDataTable As SC3080202DataSet.SC3080202UpdateSelectedCompeFromDataTable
    ' SEQNO(To)
    Protected GetSeqnoToDataTable As SC3080202DataSet.SC3080202GetSeqnoToDataTable
    ' メッセージID
    Dim msgid As Integer = 0

#End Region

#Region " ページロード "

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load Start")

        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        useFlgSuffix.Value = SC3080202BusinessLogic.GetUseFlgSuffix()
        useFlgInteriorColor.Value = SC3080202BusinessLogic.GetUseFlgInteriorColor()
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        '2016/09/14 TCS 河原 TMTタブレット性能改善 START
        If Me.Page.IsCallback OrElse (ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack) Then
            Exit Sub
        End If
        '2016/09/14 TCS 河原 TMTタブレット性能改善 END

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        UseAfterOdrProcFlgHidden.Value = SC3080202BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
        If ContainsKey(ScreenPos.Current, "StartPageId") Then
            If DirectCast(GetValue(ScreenPos.Current, "StartPageId", False), String).Equals("SC3070201") Then
                Exit Sub
            End If
        End If
        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

        If Not Me.Visible Then
            '顧客が登録されている場合のみ活動を取得する
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) And Not Page.IsPostBack Then
                getActivityList()
            End If
            Return
        End If

        If Not Page.IsPostBack Then
            '2014/02/12 TCS 山口 受注後フォロー機能開発 START
            '活動情報取得
            getActivityList()

            '受注前、受注時の場合

            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
            If (Not "1".Equals(selFllwupboxSalesAfterFlg.Value)) Or ("0".Equals(UseAfterOdrProcFlgHidden.Value)) Then
                '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

                '希望車種ポップアップ初期化  
                RefreshSeriesSelectPopup()
                '競合車種マスタ取得
                getCompeMakerMaster()
                getCompeModelMaster()
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
                '用件ソース1stリスト初期設定
                initSource1List()
                '必須入力タイトル設定
                setMandatotyItemTitle()
                '商談条件取得
                getSalesCondition()
                '競合車種取得
                getSelectedCompe()
            End If

            'メモ取得
            getSalesMemo()
            '活動詳細取得
            getActivityDetail()
            '希望車種取得
            getSelectedSeries()
            '更新： 2012/07/05 TCS 河原 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START

            '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
            '商談情報取得
            getSalesInfoDetail()
            '2013/12/09 TCS 市川 Aカード情報相互連携開発 END
            '2014/02/12 TCS 山口 受注後フォロー機能開発 END

            '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
            '購入分類マスタ取得
            getDemandStructure()
            '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
        End If
        '2012/07/05 TCS 河原 入力エラー後プロセス欄が正しく表示されない
        '課題対応のため、以下の処理が必ず実行されるように修正。
        'ただし、処理順序を入れ替えることによる影響度を考慮してこのような形で修正
        'プロセス取得
        getProcess()
        '2012/07/05 TCS 河原 入力エラー後プロセス欄が正しく表示されない
        '更新： 2012/07/05 TCS 河原 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
        If Not Page.IsPostBack Then
            'ステータス取得
            getStatus()
            '活性・非活性の制御
            setEnabled()

            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
            ' 活動登録画面に通知
            RaiseEvent ChangeSelectedSeries(Me, EventArgs.Empty)

            ' 電卓の設定
            setNumericHidden()

            '2014/02/12 TCS 山口 受注後フォロー機能開発 START
            '受注時、受注後の設定
            setBookedAfter()
            '2014/02/12 TCS 山口 受注後フォロー機能開発 END
        End If

        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        ScNsc51OtherConditionInputText.Attributes("placeholder") = WebWordUtility.GetWord(20016)
        '2012/04/26 TCS 河原 HTMLエンコード対応 END

        CleansingErrorMessage.Value = WebWordUtility.GetWord(41018)

        Logger.Info("Page_Load End")
    End Sub

    '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 画面描画直前イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        'TL表示制御
        SetTLMode()
    End Sub
    '2013/12/09 TCS 市川 Aカード情報相互連携開発 END
#End Region

#Region "プライベートメソッド"
    ''' <summary>
    ''' 電卓の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setNumericHidden()
        Logger.Info("setNumericHidden Start")

        CancelNumericMessage.Value = WebWordUtility.GetWord(20013)
        CompletionNumericMessage.Value = WebWordUtility.GetWord(20014)

        Logger.Info("setNumericHidden End")
    End Sub

    ''' <summary>
    ''' 活性・非活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setEnabled()
        Logger.Info("setEnabled Start")

        Dim tempFllwupboxseqno As String = String.Empty
        PageMoveFlgHidden.Value = "True"
        PageEnabledFlgHidden.Value = "True"
        Dim sessionStaffcd As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD) Then
            sessionStaffcd = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False), String)
        End If

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        '商談中または営業活動中または納車作業中のFollowUpBoxキー
        Dim sessionFollowUpBoxEdit As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            sessionFollowUpBoxEdit = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
        End If

        '表示活動状態
        Dim activityEnableFlg As Boolean
        '表示活動活動担当
        Dim accountPlan As String = String.Empty

        Dim account As String = StaffContext.Current.Account
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' 対象のDataTableを指定
        For Each dt In getActivityListToDataTable
            If dt.IsFLLWUPBOX_SEQNONull Then
                tempFllwupboxseqno = String.Empty
            Else
                tempFllwupboxseqno = dt.FLLWUPBOX_SEQNO
            End If
            ' 現在、表示対象の計画か？
            If selFllwupboxSeqnoHidden.Value.Equals(tempFllwupboxseqno) Then
                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start

                'フッタTCVボタン制御用
                activityEnableFlg = dt.ENABLEFLG
                accountPlan = dt.ACCOUNT_PLAN

                If selFllwupboxSeqnoHidden.Value.Equals(sessionFollowUpBoxEdit) And getStatusSales() = True Then
                    '現在、表示対象の活動のステータスが商談中または営業活動中または納車作業中

                    ' 生きている活動か？
                    If dt.ENABLEFLG = False Then
                        ' 入力不可制御
                        PageEnabledFlgHidden.Value = "False"
                        ' 活動登録不可
                        PageMoveFlgHidden.Value = "False"
                    End If
                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                    ' 受注後活動か？
                    If dt.ENABLEFLG = True And "1".Equals(selFllwupboxSalesAfterFlg.Value) Then
                        ' 入力不可制御
                        PageEnabledFlgHidden.Value = "False"
                        MemoOnlyFlgHidden.Value = "True"
                    End If
                    ' 2012/02/29 TCS 小野 【SALES_2】 END
                    ' 担当スタッフか、活動担当スタッフならば、活動実施可能
                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 
                    'If StaffContext.Current.Account.Equals(sessionStaffcd) Or StaffContext.Current.Account.Equals(dt.ACCOUNT_PLAN) Then
                    If account.Equals(sessionStaffcd) Or account.Equals(dt.ACCOUNT_PLAN) Then
                    Else
                        Dim presenceCategory As String = StaffContext.Current.PresenceCategory
                        Dim presenceDetail As String = StaffContext.Current.PresenceDetail
                        ' 2012/03/07 TCS 山口 【SALES_2】 START
                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                        If presenceCategory.Equals(PRESENCECATEGORY_SALES) And ((presenceDetail.Equals(PRESENCEDETAIL_1)) Or (presenceDetail.Equals(PRESENCEDETAIL_3))) Then
                            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

                            '一時対応中または納車作業中(一時対応)の場合
                            ' 活動登録不可
                            PageMoveFlgHidden.Value = "False"
                        Else
                            '商談中、営業活動中、納車作業中の場合
                            ' 入力不可制御
                            PageEnabledFlgHidden.Value = "False"
                            ' 活動登録不可
                            PageMoveFlgHidden.Value = "False"
                        End If
                        ' エラーメッセージ設定（権限なし）
                        PageMoveErrorMessage.Value = WebWordUtility.GetWord(ERRMSGID_20909)
                        ' 2012/03/07 TCS 山口 【SALES_2】 END
                    End If
                Else
                    '上記以外
                    ' 入力不可制御
                    PageEnabledFlgHidden.Value = "False"
                    ' 活動登録不可
                    PageMoveFlgHidden.Value = "False"

                    '2012/04/06 TCS 河原 受注後で商談開始→キャンセル時に商談メモが入力可能になるバグ対応 START
                    MemoOnlyFlgHidden.Value = "False"
                    '2012/04/06 TCS 河原 受注後で商談開始→キャンセル時に商談メモが入力可能になるバグ対応 END

                End If
                Exit For
                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End
            End If
        Next

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        'フッタTCVボタン制御
        '2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
        'SetTCVButton(activityEnableFlg, accountPlan)
        SetTCVButton()
        '2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start

        '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
        'フッタ 受注時説明ボタン制御
        SetBookingExplainButton()
        '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

        If getActivityListToDataTable.Rows.Count = 0 Then
            '対象の活動リストが0件の場合
            If "True".Equals(PageEnabledFlgHidden.Value) Then
                ' 入力不可制御
                PageEnabledFlgHidden.Value = "False"
            End If
            If "True".Equals(PageMoveFlgHidden.Value) Then
                ' 活動登録不可
                PageMoveFlgHidden.Value = "False"
            End If
        End If
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' SS以外、編集不可
        If Not StaffContext.Current.OpeCD = Operation.SSF Then
            If "True".Equals(PageEnabledFlgHidden.Value) Then
                ' 入力不可制御
                PageEnabledFlgHidden.Value = "False"
            End If
            If "True".Equals(PageMoveFlgHidden.Value) Then
                ' 活動登録不可
                PageMoveFlgHidden.Value = "False"
            End If
        End If

        '2013/12/12 TCS 市川 Aカード情報相互連携開発 START
        '入力チェック(商談画面)
        If "True".Equals(PageMoveFlgHidden.Value) Then
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
            Dim inputErrMsg As String = String.Empty
            If Not InputCheck(inputErrMsg) Then
                ' 活動登録不可
                PageMoveFlgHidden.Value = "False"
            End If
            ' エラーメッセージ設定
            PageMoveErrorMessage.Value = inputErrMsg
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
        End If
        '2013/12/12 TCS 市川 Aカード情報相互連携開発 END

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        '受注前、受注後時の画面表示制御
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If "1".Equals(selFllwupboxSalesAfterFlg.Value) And "1".Equals(UseAfterOdrProcFlgHidden.Value) Then
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
            '受注後
            AfterOdrPrcsCompeCarPanel.Visible = False
            AfterOdrPrcsSalesParametersPanel.Visible = False
            AfterOdrPrcsCVDIPanel.Visible = True
            BeforeAfterOdrPrcsSwitchButtonPanel.Visible = True
        Else
            '受注前
            AfterOdrPrcsCompeCarPanel.Visible = True
            AfterOdrPrcsSalesParametersPanel.Visible = True
            AfterOdrPrcsCVDIPanel.Visible = False
            BeforeAfterOdrPrcsSwitchButtonPanel.Visible = False
        End If

        '受注時、受注後の場合
        If "0".Equals(selFllwupboxSalesAfterFlg.Value) Or "1".Equals(selFllwupboxSalesAfterFlg.Value) Then
            If Not String.IsNullOrWhiteSpace(selFllwupboxSeqnoHidden.Value) Then
                '受注時説明登録確認
                Dim ret As Integer = SC3080202BusinessLogic.IsOrderExplanation(CDec(selFllwupboxSeqnoHidden.Value))

                If ret = 0 Then

                    'エラーメッセージ設定（受注時説明の登録完了チェック）
                    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
                    If "1".Equals(UseAfterOdrProcFlgHidden.Value) Then
                        PageMoveErrorMessage.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(ERRMSGID_20917))
                        PageMoveFlgHidden.Value = "False"
                    End If
                    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

                End If
            End If
        End If
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        ' エラーメッセージを設定する
        QuantityErrorMessageReqiored.Value = WebWordUtility.GetWord(ERRMSGID_20904)
        QuantityErrorMessageNumric.Value = WebWordUtility.GetWord(ERRMSGID_20903)
        OtherConditionErrorMessage.Value = WebWordUtility.GetWord(ERRMSGID_20906)
        DestructionMessage.Value = WebWordUtility.GetWord(ERRMSGID_20908)

        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        CondItemLabelErrorMessage.Value = WebWordUtility.GetWord(LC_ERRMSGID_2020902)
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        Logger.Info("setEnabled End")
    End Sub

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <param name="errMsg">メッセージ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function InputCheck(ByRef errMsg As String) As Boolean
        '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

        Dim inputCheckSettings As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable = Nothing
        Dim customerInfo As ActivityInfoDataSet.GetNewCustomerDataTable = Nothing
        Dim preferVcl As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable = Nothing
        Dim isDummyInput As Boolean = False
        Dim msgId As Integer = 0
        Dim msgitem0 As String = String.Empty

        Try
            '入力チェック設定取得
            inputCheckSettings = SC3080202BusinessLogic.GetSettingsInputCheckForSalesInfo()
            'ダミー入力判定
            customerInfo = ActivityInfoBusinessLogic.GetNewcustomer(DirectCast(GetValue(ScreenPos.Current, CONST_INSDID, False), String))
            If Not customerInfo Is Nothing AndAlso customerInfo.Rows.Count > 0 AndAlso Not customerInfo(0).IsDUMMYNAMEFLGNull Then
                isDummyInput = DummyNameFlgDummy.Equals(customerInfo(0).DUMMYNAMEFLG)
            End If

            Using dt As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable()
                dt.AddActivityInfoGetSelectedSeriesListFromRow(StaffContext.Current.DlrCD, selFllwupboxStrcdHidden.Value, EnvironmentSetting.CountryCode, selFllwupboxSeqnoHidden.Value)
                preferVcl = ActivityInfoBusinessLogic.GetSelectedSeriesList(dt)
                '希望車種0件のときは成約車種を取得する。
                If Not preferVcl Is Nothing AndAlso preferVcl.Rows.Count = 0 Then
                    preferVcl = ActivityInfoBusinessLogic.GetSuccessSeriesList(dt)
                End If
            End Using
            '希望車種(成約車種)が1件以上選択(固定チェック)
            If preferVcl Is Nothing OrElse preferVcl.Rows.Count = 0 Then
                msgId = ERRMSGID_20901
                Return False
            End If

            'ダミー入力(新規顧客)場合、チェックしない。
            If isDummyInput Then Return True

            '全希望車のチェックを実施
            For Each dr As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow In preferVcl.Rows
                '希望車種モデル
                If ActivityInfoBusinessLogic.IsMandatory(inputCheckSettings, CHECKITEM_ID_PREFER_VCL_MODEL) AndAlso (dr.IsMODELCDNull OrElse dr.MODELCD.Trim().Length = 0) Then
                    msgId = ERRMSGID_20912
                    Return False
                End If
                '希望車種カラー
                If ActivityInfoBusinessLogic.IsMandatory(inputCheckSettings, CHECKITEM_ID_PREFER_VCL_BODYCLR) AndAlso (dr.IsCOLORCDNull OrElse dr.COLORCD.Trim().Length = 0) Then
                    msgId = ERRMSGID_20913
                    Return False
                End If
            Next

            '商談情報errMsgId
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            'ソース選択
            If ActivityInfoBusinessLogic.IsMandatory(inputCheckSettings, CHECKITEM_ID_SOURCE_1_CD) _
                AndAlso (Me.Source1SelectedCodeHidden.Value.Trim().Length = 0 OrElse "0".Equals(Me.Source1SelectedCodeHidden.Value.Trim())) Then
                msgId = ERRMSGID_20914
                Return False
            End If

            '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD START
            '受注後は商談条件の必須入力チェック除外
            If Not (selFllwupboxSalesAfterFlg.Value.Equals("1")) Then
                '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD END
                '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
                '商談条件
                Return ActivityInfoBusinessLogic.MandatoryCheckSalesConditions(SC3080202BusinessLogic.INPUT_CHECK_TIMING, Decimal.Parse(selFllwupboxSeqnoHidden.Value), msgId, msgitem0)
                '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
                '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD START
            End If
            '2015/03/12 TCS 藤井 セールスタブレット：0118 ADD END

        Finally
            If Not inputCheckSettings Is Nothing Then inputCheckSettings.Dispose()
            If msgId > 0 Then
                errMsg = WebWordUtility.GetWord(msgId)
                If msgId = ERRMSGID_20916 Then errMsg = String.Format(errMsg, msgitem0)
            End If
        End Try

        Return True
    End Function
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' フッタTCVボタン制御
    ''' (＋納車時説明ボタン制御)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetTCVButton()

        ' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
        'Private Sub SetTCVButton(ByVal activityEnableFlg As Boolean, ByVal accountPlan As String)
        '【補足】activityEnableFlg:現在表示中の活動状態 継続中:True/完了済み:False
        '        accountPlan:現在表示中の活動の活動担当
        ' 2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start

        Logger.Info("SetTCVButton Start")

        '活動有無確認用
        Dim activityListCount As Integer = getActivityListToDataTable.Rows.Count
        Dim dlrCd As String = StaffContext.Current.DlrCD

        If StaffContext.Current.OpeCD = Operation.SSF Then
            'セールススタッフの場合

            If activityListCount > 0 Then
                '活動がある

                '商談中または営業活動中または納車作業中のFollowUpBoxキー
                Dim sessionFollowUpBoxEdit As String = String.Empty
                If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
                    sessionFollowUpBoxEdit = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
                End If

                If selFllwupboxSeqnoHidden.Value.Equals(sessionFollowUpBoxEdit) And getStatusSales() = True Then
                    '①ステータスが商談中または営業活動中または納車作業中＋表示対象の活動と一致
                    InitTCVButtonEvent(True) '活性
                ElseIf Not selFllwupboxSeqnoHidden.Value.Equals(sessionFollowUpBoxEdit) And getStatusSales() Then
                    '②ステータスが商談中または営業活動中または納車作業中＋表示対象の活動と一致しない
                    InitTCVButtonEvent(False) '非活性
                Else
                    '2012/02/27 TCS 平野 【SALES_1B】 START
                    'If activityEnableFlg = True Then
                    '    '現在、表示対象の活動が継続中
                    '    Dim sessionStaffcd As String = String.Empty
                    '    If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD) Then
                    '        sessionStaffcd = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False), String)
                    '    End If

                    '    Dim account As String = StaffContext.Current.Account
                    '    'If StaffContext.Current.Account.Equals(sessionStaffcd) Or StaffContext.Current.Account.Equals(accountPlan) Then
                    '    If account.Equals(sessionStaffcd) Or account.Equals(accountPlan) Then
                    '        '担当スタッフか、活動担当スタッフ
                    '        InitTCVButtonEvent(True) '活性

                    '    Else
                    '        '担当スタッフ、活動担当スタッフ以外

                    '        '見積もりＩＤ有無
                    '        SetTCVButtonEstimatedId(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                    '    End If

                    'Else
                    '    '現在、表示対象の活動が完了済み

                    '    '見積もりＩＤ有無
                    '    SetTCVButtonEstimatedId(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                    'End If

                    '③現在、表示対象の活動のステータスが商談中、営業活動中以外
                    '見積もりＩＤ有無
                    SetTCVButtonEstimatedId(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)

                    '2012/02/27 TCS 平野 【SALES_1B】 END
                End If

            Else
                '活動がない
                InitTCVButtonEvent(False) '非活性
            End If
        Else
            'セールススタッフ以外の場合
            If activityListCount > 0 Then
                '活動がある
                '見積もりＩＤ有無
                SetTCVButtonEstimatedId(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
            Else
                '活動がない
                InitTCVButtonEvent(False) '非活性
            End If
        End If

        Logger.Info("SetTCVButton End")
    End Sub

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' 見積もりＩＤ有無によりフッタTCVボタンを制御
    ''' (＋納車時説明ボタン制御)
    ''' </summary>
    ''' <param name="fboxDlrCd">販売店コード</param>
    ''' <param name="fboxStrCd">店舗コード</param>
    ''' <param name="fboxSeqNo">Follow-up box 連番</param>
    ''' <remarks></remarks>
    Private Sub SetTCVButtonEstimatedId(ByVal fboxDlrCd As String, ByVal fboxStrCd As String, ByVal fboxSeqNo As Long)
        Logger.Info("SetTCVButtonEstimatedId Start")

        '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
        Using param As New SC3080202DataSet.SC3080202GetEstimateidFromDataTable

            '検索条件となるレコードを作製
            param.AddSC3080202GetEstimateidFromRow(fboxDlrCd, fboxStrCd, fboxSeqNo)

            '検索処理
            Dim result As SC3080202DataSet.SC3080202GetEstimateidToDataTable = SC3080202BusinessLogic.GetEstimatedId(param)

            '活動結果取得
            Dim strCrResult As String = Me.ActivityResult(fboxSeqNo)

            InitTCVButtonEvent(True) '活性

            'Successの時、成約見積(削除フラグ=0)が0件の場合、TCVを表示させない
            '(※Success結果登録時に成約見積以外、注文承認依頼時に対象見積以外を論理削除する）
            If strCrResult.Equals(CRACTRESULT_SUCCESS) Then
                If Not result Is Nothing AndAlso result.Rows.Count = 0 Then
                    InitTCVButtonEvent(False)
                End If
            End If

        End Using
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 END

        Logger.Info("SetTCVButtonEstimatedId End")
    End Sub
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

    ''' <summary>
    ''' TCVボタン制御
    ''' (＋納車時説明ボタン制御)
    ''' </summary>
    ''' <param name="enabled">True:活性/False:非活性</param>
    ''' <remarks></remarks>
    Private Sub InitTCVButtonEvent(ByVal enabled As Boolean)
        Logger.Info("InitTCVButtonEvent Start")

        Dim tcvButton As CommonMasterFooterButton = CType(Me.Page.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        'TCVボタン制御
        tcvButton.Enabled = enabled

        '納車時説明ボタン制御も合わせて行う
        InitNewCarExplainButtonEvent(enabled)

        If enabled Then
            'TCV活性→注文番号タップ時の遷移可
            TcvRedirectFlgHidden.Value = "1"
        Else
            'TCV非活性→注文番号タップ時の遷移不可
            TcvRedirectFlgHidden.Value = "0"
        End If

        Logger.Info("InitTCVButtonEvent End")
    End Sub
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

    ''' <summary>
    ''' 納車時説明ボタン制御
    ''' </summary>
    ''' <param name="enabled">ボタン活性状態</param>
    ''' <remarks></remarks>
    Private Sub InitNewCarExplainButtonEvent(ByVal enabled As Boolean)
        Logger.Info("InitNewCarExplainButtonEvent Start")

        Dim button As CommonMasterFooterButton = CType(Me.Page.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain)
        If button IsNot Nothing Then
            'セールススタッフ、チームリーダーにのみ開放される
            button.Enabled = enabled

            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
            If "0".Equals(UseAfterOdrProcFlgHidden.Value) Then
                button.Visible = False
            End If
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        End If

        Logger.Info("InitNewCarExplainButtonEvent End")
    End Sub

    ''' <summary>
    ''' 受注時説明ボタン制御
    ''' </summary>
    ''' <param name="enabled">ボタン活性状態</param>
    ''' <param name="visible">ボタン表示状態</param>
    ''' <remarks></remarks>
    Private Sub InitBookingExplainButtonEvent(ByVal enabled As Boolean, ByVal visible As Boolean)
        Logger.Info("InitBookingExplainButtonEvent Start")

        Dim button As CommonMasterFooterButton = CType(Me.Page.Master, CommonMasterPage).GetFooterButton(SUBMENU_BOOKING_EXPLAIN)
        button.Enabled = enabled
        button.Visible = visible

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If "0".Equals(UseAfterOdrProcFlgHidden.Value) Then
            button.Visible = False
        End If
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        Logger.Info("InitBookingExplainButtonEvent End")
    End Sub

    ''' <summary>
    ''' フッタ 受注時説明ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBookingExplainButton()

        Logger.Info("SetBookingExplainButton Start")

        '活動有無確認用
        Dim activityListCount As Integer = getActivityListToDataTable.Rows.Count

        '商談中または営業活動中または納車作業中のFollowUpBoxキー
        Dim sessionFollowUpBoxEdit As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            sessionFollowUpBoxEdit = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
        End If

        '契約番号
        Dim contractno As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
            contractno = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False), String)
        End If

        If activityListCount = 0 Then
            '活動なしの場合、③非表示
            InitBookingExplainButtonEvent(False, False)
        ElseIf Not String.IsNullOrEmpty(contractno) Then
            '表示中の活動が、受注時、受注後の場合
            If selFllwupboxSeqnoHidden.Value.Equals(sessionFollowUpBoxEdit) And getStatusSales() = True Then
                'ステータスが商談中または営業活動中または納車作業中＋表示対象の活動と一致
                '①活性表示
                InitBookingExplainButtonEvent(True, True)

                '2014/08/29 TCS 武田 Next追加要件 START
                '配送希望日ボタン制御
                Dim TdisParam As New TDISFooterButton
                TdisParam.DlrCD = StaffContext.Current.DlrCD
                TdisParam.BrnCD = StaffContext.Current.BrnCD
                CType(Me.Page.Master, CommonMasterPage).DisplayTDISFooterButton(TdisParam)
                '2014/08/29 TCS 武田 Next追加要件 END
            ElseIf Not selFllwupboxSeqnoHidden.Value.Equals(sessionFollowUpBoxEdit) And getStatusSales() Then
                'ステータスが商談中または営業活動中または納車作業中＋表示対象の活動と一致しない
                '②非活性表示
                InitBookingExplainButtonEvent(False, True)
            Else
                '現在、表示対象の活動のステータスが商談中、営業活動中以外 (受注後)
                '①活性表示
                InitBookingExplainButtonEvent(True, True)

                '2014/08/29 TCS 武田 Next追加要件 START
                '配送希望日ボタン制御
                Dim TdisParam As New TDISFooterButton
                TdisParam.DlrCD = StaffContext.Current.DlrCD
                TdisParam.BrnCD = StaffContext.Current.BrnCD
                CType(Me.Page.Master, CommonMasterPage).DisplayTDISFooterButton(TdisParam)
                '2014/08/29 TCS 武田 Next追加要件 END
            End If
        Else

            '受注前、Give-up、契約なしSuccessの場合、③非表示
            InitBookingExplainButtonEvent(False, False)
        End If

        Logger.Info("SetBookingExplainButton End")
    End Sub

    '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

    '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' ショウルームボタン制御
    ''' </summary>
    ''' <param name="enabled">True:活性/False:非活性</param>
    ''' <remarks></remarks>
    Private Sub InitShowRoomButtonEvent(ByVal enabled As Boolean)
        Logger.Info("InitShowRoomButtonEvent Start")
        'TLログインの時、ショウルームボタンを制御する。
        If StaffContext.Current.TeamLeader Then
            Dim showRoomButton As CommonMasterFooterButton = CType(Me.Page.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus)
            'ショウルームボタンを制御
            If Not showRoomButton Is Nothing Then showRoomButton.Enabled = enabled
        End If
        Logger.Info("InitShowRoomButtonEvent End")
    End Sub
    '2013/12/09 TCS 市川 Aカード情報相互連携開発 END

    ''' <summary>
    ''' 活動リスト取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getActivityList()
        Logger.Info("getActivityList Start")

        ' 引数用DataRow宣言
        Dim getActivityListFromRow As SC3080202DataSet.SC3080202GetActivityListFromRow
        Dim fllwupboxSeqno As Nullable(Of Long)
        Dim fllwupboxStrcd As String = String.Empty
        Dim newfllwupboxSeqno As Nullable(Of Long)
        Dim sessionStaffcd As String = String.Empty
        Dim dlrCd As String = StaffContext.Current.DlrCD
        ' 最新活動有無フラグ
        Dim newActivityFlg As Boolean = False
        ' 新規活動行を生成し、DataTableに格納。選択する。
        Dim getNewActivityRow As SC3080202DataSet.SC3080202GetActivityListToRow

        ' 引数の設定
        getActivityListFromDataTable = New SC3080202DataSet.SC3080202GetActivityListFromDataTable
        getActivityListFromRow = getActivityListFromDataTable.NewSC3080202GetActivityListFromRow
        getActivityListFromRow.CUSTFLG = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        getActivityListFromRow.INSDID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        getActivityListFromRow.DLRCD = dlrCd
        getActivityListFromRow.STRCD = StaffContext.Current.BrnCD
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        '2013/03/06 TCS 河原 GL0874 START
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES) Then
            getActivityListFromRow.SALESFLLWSTRCD = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, False), String)
        Else
            getActivityListFromRow.SALESFLLWSTRCD = Nothing
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            getActivityListFromRow.SALESFLLWSEQNO = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
        Else
            getActivityListFromRow.SALESFLLWSEQNO = Nothing
        End If
        '2013/03/06 TCS 河原 GL0874 END
        '未取引客ID設定
        If ContainsKey(ScreenPos.Current, SESSION_KEY_NEWCUSTID) Then
            getActivityListFromRow.NEWCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_NEWCUSTID, False), String)
        End If
        ' 2012/02/29 TCS 小野 【SALES_2】 END
        getActivityListFromDataTable.Rows.Add(getActivityListFromRow)
        getActivityListToDataTable = SC3080202BusinessLogic.GetActivityList(getActivityListFromDataTable)

        If ContainsKey(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD) Then
            fllwupboxStrcd = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False), String)
        End If

        'String以外で渡ってきたらStringで詰め直す
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
            '型の判定
            Dim type As Integer
            type = VarType(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False))
            If type <> 8 Then
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            End If
        End If

        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
            If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False), String)) Then
            Else
                fllwupboxSeqno = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False), String)
            End If
        End If

        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW) Then
            If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, False), String)) Then
            Else
                newfllwupboxSeqno = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, False), String)
            End If
        End If

        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD) Then
            sessionStaffcd = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False), String)
        End If

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        '新規活動存在フラグ OFF(初期値)
        NewActivityFlgHidden.Value = "False"
        Dim account As String = StaffContext.Current.Account
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' 最新活動を取得
        If IsNothing(fllwupboxSeqno) Then
            ' 画面引数でFLLWUPBOXSEQNOが存在しない場合
            ' 最新の活動を選択する
            For Each resentRow In getActivityListToDataTable
                If resentRow.ENABLEFLG = True Then
                    newActivityFlg = True
                    selFllwupboxSeqnoHidden.Value = resentRow.FLLWUPBOX_SEQNO
                    selFllwupboxStrcdHidden.Value = resentRow.STRCD
                    Exit For
                End If
            Next

            ' 最新活動が無かった場合
            If newActivityFlg = False Then
                '$01-STEP-1B-ここから-----
                'If sessionStaffcd.Equals(StaffContext.Current.Account) And _
                If sessionStaffcd.Equals(account) And _
                   getStatusSales() = True Then '$01-STEP-1B-ここまで-----
                    '商談中または営業活動中または納車作業中の場合

                    getNewActivityRow = getActivityListToDataTable.NewSC3080202GetActivityListToRow
                    getNewActivityRow.DLRCD = dlrCd
                    'getNewActivityRow.ACCOUNT_PLAN = StaffContext.Current.Account
                    getNewActivityRow.ACCOUNT_PLAN = account
                    getNewActivityRow.STRCD = StaffContext.Current.BrnCD
                    getNewActivityRow.CRACTNAME = WebWordUtility.GetWord(20038)
                    getNewActivityRow.ENABLEFLG = True
                    If IsNothing(newfllwupboxSeqno) Then
                    Else
                        getNewActivityRow.FLLWUPBOX_SEQNO = newfllwupboxSeqno
                    End If
                    If IsNothing(fllwupboxStrcd) Then
                    Else
                        getNewActivityRow.STRCD = fllwupboxStrcd
                    End If
                    getActivityListToDataTable.Rows.InsertAt(getNewActivityRow, 0)

                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
                    '新規活動存在フラグ ON
                    NewActivityFlgHidden.Value = "True"
                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

                Else
                    If getActivityListToDataTable IsNot Nothing AndAlso 0 < getActivityListToDataTable.Count Then
                        '取得した活動の先頭のキーを設定
                        selFllwupboxSeqnoHidden.Value = getActivityListToDataTable.Rows(0).Item(getActivityListToDataTable.FLLWUPBOX_SEQNOColumn)
                        selFllwupboxStrcdHidden.Value = getActivityListToDataTable.Rows(0).Item(getActivityListToDataTable.STRCDColumn)
                    Else '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
                        '活動が存在しない場合、Hidden値を初期化
                        selFllwupboxSeqnoHidden.Value = String.Empty
                        selFllwupboxStrcdHidden.Value = String.Empty
                        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End
                    End If
                End If
            End If
        Else
            ' 画面引数でFLLWUPBOXSEQNOが存在する場合
            '2019/03/15 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究 Start
            '店舗コードが空文字の場合
            If String.IsNullOrEmpty(fllwupboxStrcd) Then
                '店舗コードにログインスタッフの店舗コードを設定
                selFllwupboxStrcdHidden.Value = StaffContext.Current.BrnCD

                For Each resentrowStrcd In getActivityListToDataTable
                    '活動情報のSEQNOとパラメータ値：SEQNOが一致する場合
                    If resentrowStrcd.FLLWUPBOX_SEQNO = fllwupboxSeqno Then
                        '活動情報の店舗コードを設定
                        selFllwupboxStrcdHidden.Value = resentrowStrcd.STRCD
                        Exit For
                    End If
                Next
            Else
                selFllwupboxStrcdHidden.Value = fllwupboxStrcd
            End If
            '2019/03/15 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究 End

            selFllwupboxSeqnoHidden.Value = fllwupboxSeqno
            For Each resentrow In getActivityListToDataTable
                ' 活動対象か
                If resentrow.ENABLEFLG = True Then
                    newActivityFlg = True
                    Exit For
                End If
            Next

            '生きている活動がない場合 新規活動用の行を追加
            If newActivityFlg = False Then
                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
                If sessionStaffcd.Equals(account) And _
                   getStatusSales() = True Then '$01-STEP-1B-ここまで-----
                    '商談中または営業活動中または納車作業中の場合

                    getNewActivityRow = getActivityListToDataTable.NewSC3080202GetActivityListToRow
                    getNewActivityRow.DLRCD = dlrCd
                    getNewActivityRow.ACCOUNT_PLAN = account
                    getNewActivityRow.STRCD = StaffContext.Current.BrnCD
                    getNewActivityRow.CRACTNAME = WebWordUtility.GetWord(20038)
                    getNewActivityRow.ENABLEFLG = True
                    If IsNothing(newfllwupboxSeqno) Then

                        'ＴＣＶからの戻り対応
                        If Not Me.IsPostBack Then

                            '活動リストにセッションFollow-up Box SEQが存在するかチェック
                            Dim keyFoundFlag As Boolean = False
                            For Each resentrow In getActivityListToDataTable
                                If resentrow.FLLWUPBOX_SEQNO = fllwupboxSeqno.Value Then
                                    'あり
                                    keyFoundFlag = True
                                End If
                            Next

                            If Not keyFoundFlag Then
                                'ＴＣＶからの戻りで、新規活動で活動登録が未の場合
                                '新規活動のレコードに、ＴＣＶに行く前にセットしたFollow-up Box SEQを設定
                                getNewActivityRow.FLLWUPBOX_SEQNO = fllwupboxSeqno.Value
                                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, selFllwupboxSeqnoHidden.Value)
                            End If
                        End If

                    Else
                        getNewActivityRow.FLLWUPBOX_SEQNO = newfllwupboxSeqno
                    End If
                    If IsNothing(fllwupboxStrcd) Then
                    Else
                        getNewActivityRow.STRCD = fllwupboxStrcd
                    End If
                    getActivityListToDataTable.Rows.InsertAt(getNewActivityRow, 0)

                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
                    '新規活動存在フラグ ON
                    NewActivityFlgHidden.Value = "True"
                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

                End If
                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End
            End If
        End If

        SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
        SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)

        ' 2012/02/29 TCS 小野 【SALES_2】 START
        '受注No、受注後フラグの取得
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            Dim salesbkgno As String = String.Empty
            Using datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable =
                New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
                datatableFrom.Rows.Add(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                salesbkgno = SC3080202BusinessLogic.GetSalesbkgno(datatableFrom)
            End Using
            SetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, salesbkgno)
            selFllwupboxSalesBkgno.Value = salesbkgno

            '2013/03/06 TCS 河原 GL0874 START
            '今表示している活動の契約状態を確認
            Dim contractflg As String = Nothing
            Using datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable = New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
                datatableFrom.Rows.Add(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                contractflg = SC3080202BusinessLogic.GetContractFlg(datatableFrom)
            End Using

            '契約キャンセルの場合
            Dim ContractCancelFlg As Boolean = False
            Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
            Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

            If (String.Equals(contractflg, "2")) AndAlso
                (PresenceCategory = "2" Or (PresenceCategory = "1" And PresenceDetail = "1")) AndAlso
                (selFllwupboxStrcdHidden.Value = getActivityListFromRow.SALESFLLWSTRCD And selFllwupboxSeqnoHidden.Value = getActivityListFromRow.SALESFLLWSEQNO) Then
                '現在既に商談中(営業活動中)で、商談中の活動と参照している活動が同じ場合
                ContractCancelFlg = True
                selFllwupboxSalesBkgno.Value = 0
                Me.SC3080202ContractCancelFlg.Value = "1"
            Else
                Me.SC3080202ContractCancelFlg.Value = "0"
            End If
            '2013/03/06 TCS 河原 GL0874 END

            If Not String.IsNullOrEmpty(salesbkgno) Or ContractCancelFlg Then
                Dim salesafterflg As String = String.Empty
                Using datatableFrom2 As ActivityInfoDataSet.ActivityInfoCountFromDataTable =
                    New ActivityInfoDataSet.ActivityInfoCountFromDataTable
                    datatableFrom2.Rows.Add(dlrCd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                    salesafterflg = SC3080202BusinessLogic.CountFllwupboxRslt(datatableFrom2)
                End Using
                SetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, salesafterflg)
                selFllwupboxSalesAfterFlg.Value = salesafterflg
            Else
                SetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, String.Empty)
                selFllwupboxSalesAfterFlg.Value = String.Empty
            End If
        End If
        ' 2012/02/29 TCS 小野 【SALES_2】 END

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        If getActivityListToDataTable.Rows.Count = 0 Then
            '活動選択用ポップアップを選択不可に
            PageActivityPopEnabledFlgHidden.Value = "False"
        Else
            '活動選択用ポップアップを選択可に
            PageActivityPopEnabledFlgHidden.Value = "True"
        End If

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' Repeaterにデータソースを設定
        ActivityRepeater.DataSource = getActivityListToDataTable
        ActivityRepeater.DataBind()

        Logger.Info("getActivityList End")
    End Sub

    ''' <summary>
    ''' 活動詳細取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getActivityDetail()
        Logger.Info("getActivityDetail Start")

        Dim getActivityDetailToRow As SC3080202DataSet.SC3080202GetActivityDetailToRow

        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            getActivityDetailToDataTable = New SC3080202DataSet.SC3080202GetActivityDetailToDataTable
        Else
            ' 引数の設定
            getActivityDetailFromDataTable = New SC3080202DataSet.SC3080202GetActivityDetailFromDataTable
            ' 引数用DataRow宣言
            Dim getActivityDetailFromRow As SC3080202DataSet.SC3080202GetActivityDetailFromRow
            getActivityDetailFromRow = getActivityDetailFromDataTable.NewSC3080202GetActivityDetailFromRow
            getActivityDetailFromRow.DLRCD = StaffContext.Current.DlrCD
            getActivityDetailFromRow.STRCD = selFllwupboxStrcdHidden.Value
            getActivityDetailFromRow.CNTCD = EnvironmentSetting.CountryCode
            getActivityDetailFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            getActivityDetailFromDataTable.Rows.Add(getActivityDetailFromRow)
            getActivityDetailToDataTable = SC3080202BusinessLogic.GetActivityDetail(getActivityDetailFromDataTable)
        End If
        ' 活動詳細の画面変数設定
        If getActivityDetailToDataTable.Rows.Count > 0 Then
            getActivityDetailToRow = getActivityDetailToDataTable.Rows(0)
            '2012/04/26 TCS 河原 HTMLエンコード対応 START
            dispContactname.Text = HttpUtility.HtmlEncode(getActivityDetailToRow.CONTACTNAME)
            dispSalesStartTime.Text = HttpUtility.HtmlEncode(getActivityDetailToRow.SALESTIME)
            dispWalkinnum.Text = HttpUtility.HtmlEncode(getActivityDetailToRow.WALKINNUM)
            dispAccount.Text = HttpUtility.HtmlEncode(getActivityDetailToRow.USERNAME)
            '2012/04/26 TCS 河原 HTMLエンコード対応 END
            accountOperationHidden.Value = getActivityDetailToRow.OPERATIONCODE

            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            ' 活動ID
            SetValue(ScreenPos.Current, SESSION_KEY_ACT_ID, getActivityDetailToRow.ACTID)

            ' 用件ID
            SetValue(ScreenPos.Current, SESSION_KEY_REQ_ID, getActivityDetailToRow.REQID)

            ' 誘致ID
            SetValue(ScreenPos.Current, SESSION_KEY_ATT_ID, getActivityDetailToRow.ATTID)

            ' 活動回数
            SetValue(ScreenPos.Current, SESSION_KEY_COUNT, getActivityDetailToRow.COUNT)

            ' 用件ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION, getActivityDetailToRow.REQUESTLOCKVERSION)

            ' 誘致ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION, getActivityDetailToRow.ATTRACTLOCKVERSION)

            ' 商談ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION, getActivityDetailToRow.SALESLOCKVERSION)
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Else
            dispContactname.Text = String.Empty
            dispSalesStartTime.Text = String.Empty
            dispWalkinnum.Text = String.Empty
            dispAccount.Text = String.Empty
            accountOperationHidden.Value = String.Empty

            '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
            AcardNumOrContractNumValue.Text = String.Empty
            '2014/02/12 TCS 山口 受注後フォロー機能開発 START
            Me.ContractNoFlgHidden.Value = String.Empty
            Me.EstimateIdHidden.Value = String.Empty
            '2014/02/12 TCS 山口 受注後フォロー機能開発 END
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            Me.Source1SelectedNameLabel.Text = String.Empty
            Me.Source1SelectedCodeHidden.Value = String.Empty
            '2013/12/09 TCS 市川 Aカード情報相互連携開発 END
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
            Me.Source2SelectedNameLabel.Text = String.Empty
            Me.Source2SelectedCodeHidden.Value = String.Empty
            Me.hdnLastSource1.Value = String.Empty
            Me.hdnLastSource2.Value = String.Empty
            Me.hdnLTRowLockVersion.Value = String.Empty
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end

            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            Dim zero As Decimal = 0
            ' 活動ID
            SetValue(ScreenPos.Current, SESSION_KEY_ACT_ID, zero)
            ' 用件ID
            SetValue(ScreenPos.Current, SESSION_KEY_REQ_ID, zero)
            ' 誘致ID
            SetValue(ScreenPos.Current, SESSION_KEY_ATT_ID, zero)
            ' 活動回数
            SetValue(ScreenPos.Current, SESSION_KEY_COUNT, CType(zero, Long))
            ' 用件ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION, CType(zero, Long))
            ' 誘致ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION, CType(zero, Long))
            ' 商談ロックバージョン
            SetValue(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION, CType(zero, Long))
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End If

        Logger.Info("getActivityDetail End")
    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 希望車種取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getSelectedSeries()
        Logger.Info("getSelectedSeries Start")
        ' 引数用DataRow宣言
        Dim getSelectedSeriesFromRow As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow
        Dim getSelectedSeriesToRow As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow

        '成約前・希望車在りの場合以外一押し車種ボタン非表示
        Me.dispSelectedMostPreferred.Visible = False

        '成約車種・希望車種情報を取得する。
        If String.IsNullOrEmpty(selFllwupboxSalesBkgno.Value) Then
            ' 希望車種
            CustomLabel1.TextWordNo = 20006
            ' 引数の設定
            If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            Else
                getSelectedSeriesFromDataTable = New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable
                getSelectedSeriesFromRow = getSelectedSeriesFromDataTable.NewActivityInfoGetSelectedSeriesListFromRow
                getSelectedSeriesFromRow.DLRCD = StaffContext.Current.DlrCD
                getSelectedSeriesFromRow.STRCD = selFllwupboxStrcdHidden.Value
                getSelectedSeriesFromRow.CNTCD = EnvironmentSetting.CountryCode
                getSelectedSeriesFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
                getSelectedSeriesFromDataTable.Rows.Add(getSelectedSeriesFromRow)
                getSelectedSeriesToDataTable = SC3080202BusinessLogic.GetSelectedSeriesList(getSelectedSeriesFromDataTable)
                If getSelectedSeriesToDataTable.Rows.Count > 0 Then
                    getSelectedSeriesToRow = getSelectedSeriesToDataTable.Rows(0)
                    selSeqnoHidden.Value = getSelectedSeriesToRow.SEQNO
                    selModelcdHidden.Value = getSelectedSeriesToRow.SERIESCD
                    selGradecdHidden.Value = getSelectedSeriesToRow.MODELCD
                    selSuffixcdHidden.Value = getSelectedSeriesToRow.SUFFIX_CD
                    selExteriorColorcdHidden.Value = getSelectedSeriesToRow.COLORCD
                    selInteriorColorcdHidden.Value = getSelectedSeriesToRow.INTERIORCLR_CD
                    selLockvrHidden.Value = getSelectedSeriesToRow.ROWLOCKVERSION
                    selMostPreferredHidden.Value = getSelectedSeriesToRow.MOST_PREF_VCL_FLG
                End If

                '車両画像、ロゴのUrlを変換
                For Each dr In getSelectedSeriesToDataTable
                    dr.PICIMAGE = Me.ResolveClientUrl(dr.PICIMAGE)
                    dr.LOGOIMAGE = Me.ResolveClientUrl(dr.LOGOIMAGE)
                Next

                '成約前は一押し車種ボタン表示
                Me.dispSelectedMostPreferred.Visible = True
            End If
        Else
            ' 成約車種
            CustomLabel1.TextWordNo = 20039
            If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            Else
                getSelectedSeriesFromDataTable = New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable
                getSelectedSeriesFromRow = getSelectedSeriesFromDataTable.NewActivityInfoGetSelectedSeriesListFromRow
                getSelectedSeriesFromRow.DLRCD = StaffContext.Current.DlrCD
                getSelectedSeriesFromRow.STRCD = selFllwupboxStrcdHidden.Value
                getSelectedSeriesFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
                getSelectedSeriesFromDataTable.Rows.Add(getSelectedSeriesFromRow)
                getSelectedSeriesToDataTable = SC3080202BusinessLogic.GetSuccessSeriesList(getSelectedSeriesFromDataTable)
                If getSelectedSeriesToDataTable.Rows.Count > 0 Then
                    getSelectedSeriesToRow = getSelectedSeriesToDataTable.Rows(0)
                    selModelcdHidden.Value = getSelectedSeriesToRow.SERIESCD
                    selGradecdHidden.Value = getSelectedSeriesToRow.MODELCD
                    selSuffixcdHidden.Value = getSelectedSeriesToRow.SUFFIX_CD
                    selExteriorColorcdHidden.Value = getSelectedSeriesToRow.COLORCD
                    selInteriorColorcdHidden.Value = getSelectedSeriesToRow.INTERIORCLR_CD
                End If

                '車両画像、ロゴのUrlを変換
                For Each dr In getSelectedSeriesToDataTable
                    dr.PICIMAGE = Me.ResolveClientUrl(dr.PICIMAGE)
                    dr.LOGOIMAGE = Me.ResolveClientUrl(dr.LOGOIMAGE)
                Next
            End If

        End If

        SelectedCarRepeater.DataSource = getSelectedSeriesToDataTable
        SelectedCarRepeater.DataBind()

        Logger.Info("getSelectedSeries End")
    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 競合車種取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getSelectedCompe()
        Logger.Info("getSelectedCompe Start")

        ' 引数の設定
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            otherCountHidden.Value = 0
        Else
            getSelectedCompeFromDataTable = New SC3080202DataSet.SC3080202GetSelectedCompeListFromDataTable
            ' 引数用DataRow宣言
            Dim getSelectedCompeFromRow As SC3080202DataSet.SC3080202GetSelectedCompeListFromRow
            getSelectedCompeFromRow = getSelectedCompeFromDataTable.NewSC3080202GetSelectedCompeListFromRow
            getSelectedCompeFromRow.DLRCD = StaffContext.Current.DlrCD
            getSelectedCompeFromRow.STRCD = selFllwupboxStrcdHidden.Value
            getSelectedCompeFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            getSelectedCompeFromDataTable.Rows.Add(getSelectedCompeFromRow)
            getSelectedCompeToDataTable = SC3080202BusinessLogic.GetSelectedCompeList(getSelectedCompeFromDataTable)
            ' 競合車種の画面変数設定
            If getSelectedCompeToDataTable.Rows.Count > 0 Then
                getSelectedCompeToDataTable.Rows(0).Item(getSelectedCompeToDataTable.TITLEColumn.ColumnName) = WebWordUtility.GetWord(20008)
            End If

            '2012/04/26 TCS 河原 HTMLエンコード対応 START
            competingOtherCount.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(20010).Replace("{0}", getSelectedCompeToDataTable.Rows.Count - 1))
            '2012/04/26 TCS 河原 HTMLエンコード対応 END
            otherCountHidden.Value = getSelectedCompeToDataTable.Rows.Count - 1
        End If

        CompeRepeater.DataSource = getSelectedCompeToDataTable
        CompeRepeater.DataBind()

        Logger.Info("getSelectedCompe End")
    End Sub

    ''' <summary>
    ''' 商談条件取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getSalesCondition()
        Logger.Info("getSalesCondition Start")

        ' 引数の設定
        getSalesConditionFromDataTable = New SC3080202DataSet.SC3080202GetSalesConditionFromDataTable
        ' 引数用DataRow宣言
        Dim getSalesConditionFromRow As SC3080202DataSet.SC3080202GetSalesConditionFromRow
        getSalesConditionFromRow = getSalesConditionFromDataTable.NewSC3080202GetSalesConditionFromRow
        getSalesConditionFromRow.DLRCD = StaffContext.Current.DlrCD
        getSalesConditionFromRow.STRCD = selFllwupboxStrcdHidden.Value
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            getSalesConditionFromRow.FLLWUPBOX_SEQNO = CLng(selFllwupboxSeqnoHidden.Value)
        End If
        getSalesConditionFromDataTable.Rows.Add(getSalesConditionFromRow)
        getSalesConditionToDataTable = SC3080202BusinessLogic.GetSalesCondition(getSalesConditionFromDataTable)
        Dim temp As Integer = -1
        Dim getSalesConditionTitleRow As SC3080202DataSet.SC3080202GetSalesConditionTitleRow
        Dim getSalesConditionItemRow As SC3080202DataSet.SC3080202GetSalesConditionItemRow
        Dim tempSalesConditionToRow As SC3080202DataSet.SC3080202GetSalesConditionTitleRow
        ' 商談条件(Title)
        Dim getSalesConditionTitle As SC3080202DataSet.SC3080202GetSalesConditionTitleDataTable
        Dim getSalesConditionItem As SC3080202DataSet.SC3080202GetSalesConditionItemDataTable

        getSalesConditionTitle = New SC3080202DataSet.SC3080202GetSalesConditionTitleDataTable

        ' 商談条件タイトルデータセット作成
        For Each dt In getSalesConditionToDataTable
            If temp = dt.SALESCONDITIONNO Then
            Else
                getSalesConditionTitleRow = getSalesConditionTitle.NewSC3080202GetSalesConditionTitleRow
                getSalesConditionTitleRow.SALESCONDITIONNO = dt.SALESCONDITIONNO
                getSalesConditionTitleRow.TITLE = dt.TITLE
                getSalesConditionTitleRow.AND_OR = dt.AND_OR
                '2013/12/05 TCS 市川 Aカード情報相互連携開発 START
                If dt.IS_MANDATORY Then getSalesConditionTitleRow.TITLE_CSSCLASS = "mandatory"
                '2013/12/05 TCS 市川 Aカード情報相互連携開発 END
                getSalesConditionTitle.Rows.Add(getSalesConditionTitleRow)
                temp = dt.SALESCONDITIONNO
            End If
        Next

        ConditionRepeater.DataSource = getSalesConditionTitle
        ConditionRepeater.DataBind()

        ' 商談条件項目データセット作成
        For i As Integer = 0 To ConditionRepeater.Items.Count - 1
            getSalesConditionItem = New SC3080202DataSet.SC3080202GetSalesConditionItemDataTable
            Dim repeater As New Repeater
            repeater = ConditionRepeater.Items(i).FindControl("ConditionItemRepeater")
            tempSalesConditionToRow = getSalesConditionTitle.Rows(i)
            Dim tempSalesConditionNo As Integer = tempSalesConditionToRow.SALESCONDITIONNO

            For Each dt In getSalesConditionToDataTable
                If tempSalesConditionNo = dt.SALESCONDITIONNO Then
                    getSalesConditionItemRow = getSalesConditionItem.NewSC3080202GetSalesConditionItemRow
                    getSalesConditionItemRow.SALESCONDITIONNO = dt.SALESCONDITIONNO
                    getSalesConditionItemRow.ITEMNO = dt.ITEMNO
                    getSalesConditionItemRow.ITEMTITLE = dt.ITEMTITLE
                    getSalesConditionItemRow.OTHER = dt.OTHER
                    getSalesConditionItemRow.CHECKFLG = dt.CHECKFLG
                    getSalesConditionItemRow.OTHERSALESCONDITION = dt.OTHERSALESCONDITION
                    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                    getSalesConditionItemRow.DEFAULT_ITEMTITLE = dt.DEFAULT_ITEMTITLE
                    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
                    getSalesConditionItem.Rows.Add(getSalesConditionItemRow)
                End If
            Next

            repeater.DataSource = getSalesConditionItem
            repeater.DataBind()
        Next

        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        ' 画面表示用の置換文字列（項目名変更アイテムの項目名）
        Me.ReplaceTxtItemTitle.Value = WebWordUtility.GetWord(LC_WORDNO_REPLACE_TXT_ITEMTITLE)
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        Logger.Info("getSalesCondition End")
    End Sub

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' プロセス取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getProcess()
        Logger.Info("getProcess Start")

        'アイコンパス設定
        SetProcessIconAttr()

        '初期値文言設定
        ProcessCatalogHiddenDefalutName.Value = WebWordUtility.GetWord(20040)
        ProcessTestdriveHiddenDefalutName.Value = WebWordUtility.GetWord(20041)
        ProcessEvaluationHiddenDefalutName.Value = WebWordUtility.GetWord(20042)
        ProcessQuotationHiddenDefalutName.Value = WebWordUtility.GetWord(20043)
        ''ProcessSuccessHiddenDefalutName.Value = WebWordUtility.GetWord(20044)
        ''ProcessAllocationHiddenDefalutName.Value = WebWordUtility.GetWord(20045)
        ''ProcessPaymentHiddenDefalutName.Value = WebWordUtility.GetWord(20046)
        ''ProcessDeliveryHiddenDefalutName.Value = WebWordUtility.GetWord(20047)

        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            ' 2012/02/29 TCS 小野 【SALES_2】 START
            ' 引数の設定
            getProcessFromDataTable = New ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable
            ' 引数用DataRow宣言
            Dim getProcessFromRow As ActivityInfoDataSet.ActivityInfoGetProcessFromRow
            getProcessFromRow = getProcessFromDataTable.NewActivityInfoGetProcessFromRow
            ''getProcessFromRow.SALESBKGNO = selFllwupboxSalesBkgno.Value
            ' 2012/02/29 TCS 小野 【SALES_2】 END
            getProcessFromRow.DLRCD = StaffContext.Current.DlrCD
            getProcessFromRow.STRCD = selFllwupboxStrcdHidden.Value
            getProcessFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            getProcessFromDataTable.Rows.Add(getProcessFromRow)
            getProcessToDataTable = SC3080202BusinessLogic.GetProcess(getProcessFromDataTable)
            ' 2012/02/29 TCS 小野 【SALES_2】 START
            If getProcessToDataTable.Rows.Count > 0 Then
                If getProcessToDataTable(0).IsSEQNONull Then
                    '更新： 2012/07/05 TCS 河原 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
                    If Not IsNothing(getSelectedSeriesToDataTable) Then
                        If getSelectedSeriesToDataTable.Rows.Count > 0 Then
                            getProcessToDataTable(0).SEQNO = getSelectedSeriesToDataTable(0).SEQNO
                        End If
                    End If
                    '更新： 2012/07/05 TCS 河原 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
                End If
            End If
            ' 2012/02/29 TCS 小野 【SALES_2】 END
        End If

        ProcessRepeater.DataSource = getProcessToDataTable
        ProcessRepeater.DataBind()

        Logger.Info("getProcess End")
    End Sub

    ''' <summary>
    ''' プロセスアイコンの属性設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetProcessIconAttr()
        Logger.Info("SetProcessIconAttr Start")

        'アイコンリスト取得
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        'Using dt As SC3080202DataSet.SC3080202GetFllwupboxContentDataTable = SC3080202BusinessLogic.GetProcessIcons()
        Using dt As SC3080202DataSet.SC3080202GetFllwupboxContentDataTable = SC3080202BusinessLogic.GetProcessIcons()
            ' 2012/02/29 TCS 小野 【SALES_2】 END
            'ループ
            For Each dr As SC3080202DataSet.SC3080202GetFllwupboxContentRow In dt.Rows

                Dim targetdom As HtmlGenericControl

                If dr.CTNTSEQNO = SC3080202BusinessLogic.ActionCatalog Then
                    'カタログ
                    targetdom = dispProcessCatalog
                ElseIf dr.CTNTSEQNO = SC3080202BusinessLogic.ActionTestdrive Then
                    '試乗
                    targetdom = dispProcessTestdrive
                ElseIf dr.CTNTSEQNO = SC3080202BusinessLogic.ActionEvaluation Then
                    '査定
                    targetdom = dispProcessEvaluation
                ElseIf dr.CTNTSEQNO = SC3080202BusinessLogic.ActionQuotation Then
                    '見積
                    targetdom = dispProcessQuotation
                Else
                    Continue For
                End If

                '属性設定
                targetdom.Attributes("onIconPath") = dr.ICONPATH_SALES_SELECTED
                targetdom.Attributes("offIconPath") = dr.ICONPATH_SALES_NOTSELECTED
            Next

        End Using

        Logger.Info("SetProcessIconAttr End")
    End Sub
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' ステータス取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getStatus()
        Logger.Info("getStatus Start")

        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            CrActResult.Src = String.Empty
        Else
            If String.IsNullOrEmpty(selFllwupboxSalesBkgno.Value) Then
                Dim crResult As String = Me.ActivityResult(CDec(selFllwupboxSeqnoHidden.Value))
                Select Case crResult
                    Case SC3080202BusinessLogic.CractresultHot
                        CrActResult.Src = SC3080202BusinessLogic.StatuspicHot
                    Case SC3080202BusinessLogic.CractresultProspect
                        CrActResult.Src = SC3080202BusinessLogic.StatuspicWarm
                    Case SC3080202BusinessLogic.CractresultSuccess
                        CrActResult.Src = SC3080202BusinessLogic.StatuspicSuccess
                    Case SC3080202BusinessLogic.CractresultGiveup
                        CrActResult.Src = SC3080202BusinessLogic.StatuspicGiveup
                    Case SC3080202BusinessLogic.CractresultCold
                        CrActResult.Src = SC3080202BusinessLogic.StatuspicCold
                    Case Else
                        CrActResult.Src = String.Empty
                End Select
            Else
                CrActResult.Src = SC3080202BusinessLogic.StatuspicSuccess
            End If
        End If

        Logger.Info("getStatus End")
    End Sub
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

    Private Sub getSalesMemo()
        Logger.Info("getSalesMemo Start")

        ' メモ取得
        ' 引数の設定
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
            todayMemoTextBox.Text = String.Empty
            todayMemoTextBoxBefore.Value = todayMemoTextBox.Text
            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End
        Else
            getSalesMemoHisFromDataTable = New SC3080202DataSet.SC3080202GetSalesMemoHisFromDataTable
            getSalesMemoTodayFromDataTable = New SC3080202DataSet.SC3080202GetSalesMemoTodayFromDataTable
            ' 引数用DataRow宣言
            Dim getSalesMemoHisFromRow As SC3080202DataSet.SC3080202GetSalesMemoHisFromRow
            getSalesMemoHisFromRow = getSalesMemoHisFromDataTable.NewSC3080202GetSalesMemoHisFromRow
            getSalesMemoHisFromRow.DLRCD = StaffContext.Current.DlrCD
            getSalesMemoHisFromRow.STRCD = selFllwupboxStrcdHidden.Value
            getSalesMemoHisFromRow.FLLWUPBOX_SEQNO = CLng(selFllwupboxSeqnoHidden.Value)
            getSalesMemoHisFromDataTable.Rows.Add(getSalesMemoHisFromRow)
            getSalesMemoHisToDataTable = SC3080202BusinessLogic.GetSalesMemoHis(getSalesMemoHisFromDataTable)

            Dim getSalesMemoTodayFromRow As SC3080202DataSet.SC3080202GetSalesMemoTodayFromRow
            getSalesMemoTodayFromRow = getSalesMemoTodayFromDataTable.NewSC3080202GetSalesMemoTodayFromRow
            getSalesMemoTodayFromRow.DLRCD = StaffContext.Current.DlrCD
            getSalesMemoTodayFromRow.STRCD = selFllwupboxStrcdHidden.Value
            getSalesMemoTodayFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            getSalesMemoTodayFromDataTable.Rows.Add(getSalesMemoTodayFromRow)
            getSalesMemoTodayToDataTable = SC3080202BusinessLogic.GetSalesMemoToday(getSalesMemoTodayFromDataTable)

            Dim getSalesMemoTodayToRow As SC3080202DataSet.SC3080202GetSalesMemoTodayToRow
            ' メモ欄の画面変数設定
            If getSalesMemoTodayToDataTable.Rows.Count > 0 Then
                getSalesMemoTodayToRow = getSalesMemoTodayToDataTable.Rows(0)
                todayMemoTextBox.Text = getSalesMemoTodayToRow.MEMO
            Else
                todayMemoTextBox.Text = String.Empty
            End If
            '変更前の値を格納
            todayMemoTextBoxBefore.Value = todayMemoTextBox.Text
        End If

        MemoRepeater.DataSource = getSalesMemoHisToDataTable
        MemoRepeater.DataBind()

        Logger.Info("getSalesMemo End")
    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 車種選択のリフレッシュ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshSeriesSelectPopup()
        Logger.Info("RefreshSeriesSelectPopup Start")

        'モデル
        getModelMaster()

        'グレード
        getGradeMaster()

        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If "1".Equals(useFlgSuffix.Value) Then
            'サフィックス
            getsuffixMaster()
        End If
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        '外装色
        getExteriorColorMaster()

        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If "1".Equals(useFlgInteriorColor.Value) Then
            '内装色
            getInteriorColorMaster()
        End If
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        Logger.Info("RefreshSeriesSelectPopup End")
    End Sub

    ''' <summary>
    ''' モデルマスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getModelMaster()
        Logger.Info("getSeriesMaster Start")

        ' 引数の設定
        getSeriesMasterFromDataTable = New SC3080202DataSet.SC3080202GetSeriesMasterFromDataTable
        getSeriesMasterToDataTable = New SC3080202DataSet.SC3080202GetSeriesMasterToDataTable
        ' 引数用dataRow宣言
        Dim getSeriesMasterFromRow As SC3080202DataSet.SC3080202GetSeriesMasterFromRow
        getSeriesMasterFromRow = getSeriesMasterFromDataTable.NewSC3080202GetSeriesMasterFromRow
        getSeriesMasterFromRow.DLRCD = StaffContext.Current.DlrCD
        getSeriesMasterFromRow.CNTCD = EnvironmentSetting.CountryCode
        getSeriesMasterFromDataTable.Rows.Add(getSeriesMasterFromRow)
        getSeriesMasterToDataTable = SC3080202BusinessLogic.GetSelectedSeriesMaster(getSeriesMasterFromDataTable)

        SeriesMasterRepeater.DataSource = getSeriesMasterToDataTable
        SeriesMasterRepeater.DataBind()

        Logger.Info("getSeriesMaster End")
    End Sub

    ''' <summary>
    ''' グレードマスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getGradeMaster()
        Logger.Info("getModelMaster Start")

        ' 引数の設定
        getModelMasterFromDataTable = New SC3080202DataSet.SC3080202GetModelMasterFromDataTable
        getModelMasterToDataTable = New SC3080202DataSet.SC3080202GetModelMasterToDataTable
        ' 引数用dataRow宣言
        Dim getModelMasterFromRow As SC3080202DataSet.SC3080202GetModelMasterFromRow
        getModelMasterFromRow = getModelMasterFromDataTable.NewSC3080202GetModelMasterFromRow
        getModelMasterFromRow.DLRCD = StaffContext.Current.DlrCD
        getModelMasterFromRow.CNTCD = EnvironmentSetting.CountryCode
        getModelMasterFromDataTable.Rows.Add(getModelMasterFromRow)
        getModelMasterToDataTable = SC3080202BusinessLogic.GetSelectedGradeMaster(getModelMasterFromDataTable)

        ModelMasterRepeater.DataSource = getModelMasterToDataTable
        ModelMasterRepeater.DataBind()

        Logger.Info("getModelMaster End")
    End Sub

    ''' <summary>
    ''' サフィックスマスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getsuffixMaster()
        Logger.Info("getsuffixMaster Start")

        ' 引数の設定
        getsuffixMasterFromDataTable = New SC3080202DataSet.SC3080202GetSuffixMasterFromDataTable
        getsuffixMasterToDataTable = New SC3080202DataSet.SC3080202GetSuffixMasterToDataTable
        ' 引数用dataRow宣言
        Dim getsuffixMasterFromRow As SC3080202DataSet.SC3080202GetSuffixMasterFromRow
        getsuffixMasterFromRow = getsuffixMasterFromDataTable.NewSC3080202GetSuffixMasterFromRow
        getsuffixMasterFromRow.DLRCD = StaffContext.Current.DlrCD
        getsuffixMasterFromRow.CNTCD = EnvironmentSetting.CountryCode
        getsuffixMasterFromDataTable.Rows.Add(getsuffixMasterFromRow)
        getsuffixMasterToDataTable = SC3080202BusinessLogic.GetSelectedSuffixMaster(getsuffixMasterFromDataTable)

        SuffixMasterRepeater.DataSource = getsuffixMasterToDataTable
        SuffixMasterRepeater.DataBind()

        Logger.Info("getsuffixMaster End")
    End Sub

    ''' <summary>
    ''' 外装色マスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getExteriorColorMaster()
        Logger.Info("getExteriorColorMaster Start")

        ' 引数の設定
        getExteriorColorMasterFromDataTable = New SC3080202DataSet.SC3080202GetExteriorColorMasterFromDataTable
        getExteriorColorMasterToDataTable = New SC3080202DataSet.SC3080202GetExteriorColorMasterToDataTable
        ' 引数用dataRow宣言
        Dim getExteriorColorMasterFromRow As SC3080202DataSet.SC3080202GetExteriorColorMasterFromRow
        getExteriorColorMasterFromRow = getExteriorColorMasterFromDataTable.NewSC3080202GetExteriorColorMasterFromRow
        getExteriorColorMasterFromRow.DLRCD = StaffContext.Current.DlrCD
        getExteriorColorMasterFromRow.CNTCD = EnvironmentSetting.CountryCode
        getExteriorColorMasterFromDataTable.Rows.Add(getExteriorColorMasterFromRow)
        getExteriorColorMasterToDataTable = SC3080202BusinessLogic.GetSelectedExteriorColorMaster(getExteriorColorMasterFromDataTable)

        ExteriorColorMasterRepeater.DataSource = getExteriorColorMasterToDataTable
        ExteriorColorMasterRepeater.DataBind()

        Logger.Info("getExteriorColorMaster End")
    End Sub

    ''' <summary>
    ''' 内装色マスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getInteriorColorMaster()
        Logger.Info("getInteriorColorMaster Start")

        ' 引数の設定
        getInteriorColorMasterFromDataTable = New SC3080202DataSet.SC3080202GetInteriorColorMasterFromDataTable
        getInteriorColorMasterToDataTable = New SC3080202DataSet.SC3080202GetInteriorColorMasterToDataTable
        ' 引数用dataRow宣言
        Dim getInteriorColorMasterFromRow As SC3080202DataSet.SC3080202GetInteriorColorMasterFromRow
        getInteriorColorMasterFromRow = getInteriorColorMasterFromDataTable.NewSC3080202GetInteriorColorMasterFromRow
        getInteriorColorMasterFromRow.DLRCD = StaffContext.Current.DlrCD
        getInteriorColorMasterFromRow.CNTCD = EnvironmentSetting.CountryCode
        getInteriorColorMasterFromDataTable.Rows.Add(getInteriorColorMasterFromRow)
        getInteriorColorMasterToDataTable = SC3080202BusinessLogic.GetSelectedInteriorColorMaster(getInteriorColorMasterFromDataTable)

        InteriorColorMasterRepeater.DataSource = getInteriorColorMasterToDataTable
        InteriorColorMasterRepeater.DataBind()

        Logger.Info("getInteriorColorMaster End")
    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 競合車種（メーカ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getCompeMakerMaster()
        Logger.Info("getCompeMakerMaster Start")
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim dlrcd As String = StaffContext.Current.DlrCD
        ' メーカマスタ取得
        ' 引数の設定
        getCompeMakerMasterToDataTable = New SC3080202DataSet.SC3080202GetCompeMakerMasterToDataTable

        getCompeMakerMasterToDataTable = SC3080202BusinessLogic.GetSelectedCompeMakerMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        CompCarMakerMasterRepeater.DataSource = getCompeMakerMasterToDataTable
        CompCarMakerMasterRepeater.DataBind()

        Logger.Info("getCompeMakerMaster End")
    End Sub

    ''' <summary>
    ''' 競合車種（モデル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getCompeModelMaster()
        Logger.Info("getCompeModelMaster Start")
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim dlrcd As String = StaffContext.Current.DlrCD

        ' モデルマスタ取得
        ' 引数の設定
        getCompeModelMasterToDataTable = New SC3080202DataSet.SC3080202GetCompeModelMasterToDataTable
        getCompeModelMasterToDataTable = SC3080202BusinessLogic.GetSelectedCompeModelMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        CompCarModelMasterRepeater.DataSource = getCompeModelMasterToDataTable
        CompCarModelMasterRepeater.DataBind()

        Logger.Info("getCompeModelMaster End")
    End Sub

    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' ステータスが商談中または営業活動中または納車作業中かを判定
    ''' </summary>
    ''' <returns>True:商談中または営業活動中または納車作業中、False:商談中または営業活動中または納車作業中以外</returns>
    ''' <remarks></remarks>
    Private Function getStatusSales() As Boolean
        Logger.Info("RefreshSalesCondition Start")

        ' 共通部品を使用しステータスを取得
        Dim presenceCategory As String = StaffContext.Current.PresenceCategory
        Dim presenceDetail As String = StaffContext.Current.PresenceDetail

        Dim flg As Boolean = False
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        'If ((presenceCategory.Equals(PRESENCECATEGORY_SALES) And presenceDetail.Equals(PRESENCEDETAIL_0)) Or
        '    (presenceCategory.Equals(PRESENCECATEGORY_STANDBY) And presenceDetail.Equals(PRESENCEDETAIL_1))) Then
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        If ((presenceCategory.Equals(PRESENCECATEGORY_SALES) And (presenceDetail.Equals(PRESENCEDETAIL_0) Or presenceDetail.Equals(PRESENCEDETAIL_1) Or
                                                                  presenceDetail.Equals(PRESENCEDETAIL_2) Or presenceDetail.Equals(PRESENCEDETAIL_3))) Or
        (presenceCategory.Equals(PRESENCECATEGORY_STANDBY) And presenceDetail.Equals(PRESENCEDETAIL_1))) Then
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
            ' 2012/02/29 TCS 小野 【SALES_2】 END
            '商談中または営業活動中 または一時対応中 または納車作業中または納車作業中(一時対応)
            flg = True
        Else
            '上記以外
            flg = False
        End If

        Logger.Info("RefreshSalesCondition End")

        Return flg
    End Function
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <summary>
    ''' 活動結果取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <param name="followupBox">Follow-up Box内連番</param>
    ''' <returns>活動結果</returns>
    ''' <remarks>活動結果が存在しない場合は、空白で返却</remarks>
    Private Function GetCrResult(ByVal dlrCd As String, _
                                 ByVal strCd As String, _
                                 ByVal followupBox As Long) As String

        Dim returnCrActresult As String = String.Empty

        '検索条件となるレコードを生成
        Using setCrResultTbl As New ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable
            Dim setCrResultRow As ActivityInfoDataSet.ActivityInfoGetStatusFromRow = Nothing
            setCrResultRow = setCrResultTbl.NewActivityInfoGetStatusFromRow

            '検索条件を設定
            setCrResultRow.DLRCD = dlrCd
            setCrResultRow.STRCD = strCd
            setCrResultRow.FLLWUPBOX_SEQNO = followupBox
            setCrResultTbl.Rows.Add(setCrResultRow)

            '活動結果取得
            Dim getCrResultTbl As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable = Nothing
            Dim getCrResultRow As ActivityInfoDataSet.ActivityInfoGetStatusToRow = Nothing

            getCrResultTbl = SC3080202BusinessLogic.GetStatus(setCrResultTbl)
            If 0 < getCrResultTbl.Count Then
                '活動結果取得
                getCrResultRow = CType(getCrResultTbl.Rows(0), ActivityInfoDataSet.ActivityInfoGetStatusToRow)
                returnCrActresult = getCrResultRow.CRACTRESULT
            End If

            setCrResultRow = Nothing
            getCrResultTbl = Nothing
            getCrResultRow = Nothing
        End Using

        Return returnCrActresult
    End Function
    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 商談情報表示更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getSalesInfoDetail()
        Dim dt As SC3080202DataSet.SC3080202SalesInfoDetailDataTable = Nothing
        Dim salesId As Decimal = 0

        '初期化
        Me.AcardNumOrContractNumValue.Text = String.Empty
        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        Me.ContractNoFlgHidden.Value = String.Empty
        Me.EstimateIdHidden.Value = String.Empty
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
        Me.Source1SelectedNameLabel.Text = String.Empty
        Me.Source1SelectedCodeHidden.Value = String.Empty
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
        Me.Source2SelectedNameLabel.Text = String.Empty
        Me.Source2SelectedCodeHidden.Value = String.Empty
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end

        If Decimal.TryParse(selFllwupboxSeqnoHidden.Value, salesId) Then
            Try
                dt = SC3080202BusinessLogic.GetSalesInfoDetail(salesId)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                    'Aカード番号/契約番号
                    If Not dt(0).IsDISPLAY_NUMNull Then AcardNumOrContractNumValue.Text = dt(0).DISPLAY_NUM
                    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
                    'Aカード番号or表示無し：0、注文番号：1
                    If Not dt(0).IsCONTRACTNOFLGNull Then ContractNoFlgHidden.Value = dt(0).CONTRACTNOFLG
                    '見積管理ID(遷移用)
                    If Not dt(0).IsESTIMATEIDNull Then EstimateIdHidden.Value = dt(0).ESTIMATEID
                    '2014/02/12 TCS 山口 受注後フォロー機能開発 END
                    AcardNumOrContractNumTitle.TextWordNo = WORD_NUM_ACARDNUMTITLE
                    If dt(0).IS_CONTRACTED Then AcardNumOrContractNumTitle.TextWordNo = WORD_NUM_CONTORACTNUMTITLE
                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
                    '用件ソース1st
                    If Not dt(0).IsSOURCE_1_NAMENull Then Me.Source1SelectedNameLabel.Text = dt(0).SOURCE_1_NAME
                    If Not dt(0).IsSOURCE_1_CDNull Then Me.Source1SelectedCodeHidden.Value = dt(0).SOURCE_1_CD
                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                    '用件ソース2nd
                    If Not dt(0).IsREQ_SECOND_CAT_NAMENull Then Me.Source2SelectedNameLabel.Text = dt(0).REQ_SECOND_CAT_NAME
                    If Not dt(0).IsSOURCE_2_CDNull Then Me.Source2SelectedCodeHidden.Value = dt(0).SOURCE_2_CD
                    If Not dt(0).IsSOURCE_1_CHG_POSSIBLE_FLGNull Then Me.hdnSource1PossibleFlg.Value = dt(0).SOURCE_1_CHG_POSSIBLE_FLG
                    If Not dt(0).IsSOURCE_2_CHG_POSSIBLE_FLGNull Then Me.hdnSource2PossibleFlg.Value = dt(0).SOURCE_2_CHG_POSSIBLE_FLG
                    If Not dt(0).IsLTLOCKVERSIONNull Then Me.hdnLTRowLockVersion.Value = dt(0).LTLOCKVERSION
                    If Not dt(0).IsGET_TABLE_NONull Then Me.hdnGetTableNO.Value = dt(0).GET_TABLE_NO
                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                    '行ロック番号
                    If Not dt(0).IsSALESLOCKVERSIONNull Then SetValue(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION, dt(0).SALESLOCKVERSION)
                    If Not dt(0).IsREQUESTLOCKVERSIONNull Then SetValue(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION, dt(0).REQUESTLOCKVERSION)
                    If Not dt(0).IsATTRACTLOCKVERSIONNull Then SetValue(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION, dt(0).ATTRACTLOCKVERSION)

                    '直販フラグの制御
                    If String.Equals(dt(0).DIRECT_SALES_FLG, "1") Then
                        Me.SelectedDirectBilling.Checked = True
                    Else
                        Me.SelectedDirectBilling.Checked = False
                    End If

                    If Not String.Equals(selFllwupboxSalesAfterFlg.Value, "1") Or String.Equals(UseAfterOdrProcFlgHidden.Value, "0") Then
                        If getStatusSales() And Not String.Equals(dt(0).DIRECT_SALES_FLG_UPDATE_FLG, "1") Then
                            Me.SelectedDirectBilling.Disabled = False
                        Else
                            Me.SelectedDirectBilling.Disabled = True
                        End If
                    Else
                        Me.SelectedDirectBilling.Disabled = True
                    End If

                End If
                '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                GetSalesLocal(salesId)
                '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
            Finally
                If Not dt Is Nothing Then dt.Dispose()
            End Try
        End If
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 start
        'Source１と２の活性・非活性を設定する

        If (Me.hdnGetTableNO.Value.Equals("1")) Then
            'ソース1の取得元テーブルNOが１の場合は、無条件で活性
            Me.hdnSource1PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource1.Value = Me.Source1SelectedCodeHidden.Value
        ElseIf (Me.hdnSource1PossibleFlg.Value.Equals(FLG_TRUE)) Then
            'ソース1の編集可能フラグが1の時も無条件で活性
            Me.hdnSource1PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource1.Value = Me.Source1SelectedCodeHidden.Value
        ElseIf Trim(Me.Source1SelectedNameLabel.Text).Length = 0 Then
            'ソース1が未入力の場合も活性
            Me.hdnSource1PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource1.Value = Me.Source1SelectedCodeHidden.Value
        Else
            Me.hdnSource1PossibleFlg.Value = FLG_FALSE
        End If

        'ソース１が設定されていれば用件ソース2ndリスト初期設定
        If Trim(Me.Source1SelectedNameLabel.Text).Length > 0 Then
            initSource2List(Me.Source1SelectedCodeHidden.Value, False)
        End If

        If Trim(Me.Source1SelectedNameLabel.Text).Length = 0 Then
            'ソース１が設定されていなければ用件ソース2ndリストは非活性
            Me.hdnSource2PossibleFlg.Value = FLG_FALSE
        ElseIf (Me.hdnGetTableNO.Value.Equals("1")) Then
            'ソース1の取得元テーブルNOが１の場合は、無条件で活性
            Me.hdnSource2PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource2.Value = Me.Source2SelectedCodeHidden.Value
        ElseIf (Me.hdnSource2PossibleFlg.Value.Equals(FLG_TRUE)) Then
            'ソース2の編集可能フラグが1の時も無条件で活性
            Me.hdnSource2PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource2.Value = Me.Source2SelectedCodeHidden.Value
        ElseIf Trim(Me.Source2SelectedNameLabel.Text).Length = 0 Then
            'ソース2が未入力の場合も活性
            Me.hdnSource2PossibleFlg.Value = FLG_TRUE
            Me.hdnLastSource2.Value = Me.Source2SelectedCodeHidden.Value
        Else
            Me.hdnSource2PossibleFlg.Value = FLG_FALSE
        End If

        Me.Source1ListUpdatePanel.Update()
        Me.Source2ListUpdatePanel.Update()
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 end

    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 削除

    ''' <summary>
    ''' 用件ソース1st選択リスト初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initSource1List()

        Dim dt As SC3080202DataSet.SC3080202SourcesOfACardMasterDataTable = Nothing
        dt = SC3080202BusinessLogic.GetSourcesOfACardMaster()
        Me.Source1ListRepeater.DataSource = dt
        Me.Source1ListRepeater.DataBind()

    End Sub
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 start
    ''' <summary>
    ''' 用件ソース2nd選択リスト初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initSource2List(ByVal source1CD As Long, ByVal initflg As Boolean)
        Dim dt As SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable = Nothing
        dt = SC3080202BusinessLogic.GetSources2Master(source1CD)
        Me.Source2ListRepeater.DataSource = dt
        Me.Source2ListRepeater.DataBind()
        If (initflg) Then
            Me.Source2SelectedCodeHidden.Value = 0
            Me.Source2SelectedNameLabel.Text = ""
            Me.hdnLastSource2.Value = ""
        End If
        Me.UpdSource2Selector.Update()
    End Sub
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 end

    ''' <summary>
    ''' 必須入力項目タイトル色設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setMandatotyItemTitle()
        Dim dt As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable = Nothing
        Try
            dt = SC3080202BusinessLogic.GetSettingsInputCheckForSalesInfo()

            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            If (ActivityInfoBusinessLogic.IsMandatory(dt, CHECKITEM_ID_SOURCE_1_CD)) Then
                Me.Source1TitleLabel.CssClass = "mandatory"
            End If

        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' TeamLeaderの表示制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetTLMode()
        If StaffContext.Current.TeamLeader Then

            'TLが商談中(納車作業中・営業活動中・商談(一時)中・納車作業(一時)中)の場合
            'SCOとして振る舞う(他の顧客へ移動しない)
            Dim InActive As Boolean = getStatusSales()

            'ショウルームボタン
            InitShowRoomButtonEvent(Not InActive)
            'MG用通知一覧非表示
            If InActive Then DisappredMGInfoList()

        End If
    End Sub

    ''' <summary>
    ''' MG用通知一覧を非表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisappredMGInfoList()

        'MG用通知一覧を非表示とする。(毎回PostBack時にマスターページにて復元されるため、復元処理は不要)
        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "noticeListFrameHide", "$(function(){ $('#noticeListFrame').css('display','none');});", True)

    End Sub

    '2013/12/09 TCS 市川 Aカード情報相互連携開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注時、受注後の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setBookedAfter()
        Logger.Info("setBookedAfter Start")

        '初期化
        'アイコン数
        AfterOdrPrcsIconCountHidden.Value = "0"
        '最大ページ数
        AfterOdrPrcsIconMaxPageHidden.Value = "0"
        '表示中ページ（デフォルト値）
        AfterOdrPrcsIconPageHidden.Value = "1"

        '受注後プロセス設定
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If "0".Equals(UseAfterOdrProcFlgHidden.Value) Then
            ProcessBookedAfterRepeater.DataSource = Nothing
            ProcessBookedAfterRepeater.DataBind()
        End If
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        '受注後の場合
        If "1".Equals(selFllwupboxSalesAfterFlg.Value) Then
            '契約車両情報取得
            Dim dtContractCar As SC3080202DataSet.SC3080202GetContractCarDataTable = _
                            SC3080202BusinessLogic.GetContractCarData(CDec(selFllwupboxSeqnoHidden.Value))

            '受注後工程詳細情報取得
            Dim dtBookedAfterDetailInfo As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDataTable = _
                            SC3080202BusinessLogic.GetBookedAfterDetailInfo(CDec(selFllwupboxSeqnoHidden.Value))

            '受注後プロセスマスタ取得
            Dim dtBookedAfterProcessMaster As ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable = _
                                            ActivityInfoBusinessLogic.GetBookedAfterProcessMaster()

            '受注後プロセス実績取得
            Dim dtBookedAfterProcessResult As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable = _
                                            ActivityInfoBusinessLogic.GetBookedAfterProcessResult(CDec(selFllwupboxSeqnoHidden.Value))

            '受注後プロセスマスタ、受注後プロセス実績を一つのDataTableにマージ
            Dim dtBookedAfterProcess As New SC3080202DataSet.SC3080202BookedAfterProcessDataTable
            dtBookedAfterProcess.Merge(dtBookedAfterProcessMaster)

            For Each drBookedAfterProcess In dtBookedAfterProcess
                '紐付く受注後プロセス実績取得
                Dim drBookedAfterProcessResult As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow() = _
                    CType(dtBookedAfterProcessResult.Select(dtBookedAfterProcessResult.AFTER_ODR_PRCS_CDColumn.ColumnName & " = '" & drBookedAfterProcess.AFTER_ODR_PRCS_CD & "'"),  _
                        ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow())

                If drBookedAfterProcessResult.Length > 0 Then
                    If Not drBookedAfterProcessResult(0).IsRSLT_DATENull AndAlso Not String.IsNullOrWhiteSpace(drBookedAfterProcessResult(0).RSLT_DATE) Then
                        Dim timeSpecify As Boolean

                        If String.Equals(drBookedAfterProcessResult(0).RSLT_DATEORTIME_FLG, "1") Then
                            timeSpecify = True
                        Else
                            timeSpecify = False
                        End If

                        drBookedAfterProcess.RSLT_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                                        CDate(drBookedAfterProcessResult(0).RSLT_DATE), _
                                                                                        DateTimeFunc.Now, _
                                                                                        StaffContext.Current.DlrCD, _
                                                                                        timeSpecify)
                    End If
                    If Not drBookedAfterProcessResult(0).IsCHECKFLGNull AndAlso Not String.IsNullOrWhiteSpace(drBookedAfterProcessResult(0).CHECKFLG) Then
                        drBookedAfterProcess.CHECKFLG = drBookedAfterProcessResult(0).CHECKFLG
                    End If
                Else
                    drBookedAfterProcess.RSLT_DATE = String.Empty
                    drBookedAfterProcess.CHECKFLG = "0"
                End If
            Next

            '受注後時の画面設定
            setBookedAfterDisplaySetting(dtContractCar, dtBookedAfterDetailInfo, dtBookedAfterProcess)
        End If

        Logger.Info("setBookedAfter End")
    End Sub

    ''' <summary>
    ''' 受注後時の画面設定
    ''' </summary>
    ''' <param name="dtContractCar">契約車両情報</param>
    ''' <param name="dtBookedAfterDetailInfo">受注後工程詳細情報取得</param>
    ''' <param name="dtBookedAfterProcess">受注後プロセス情報</param>
    ''' <remarks></remarks>
    Private Sub setBookedAfterDisplaySetting(ByVal dtContractCar As SC3080202DataSet.SC3080202GetContractCarDataTable, _
                                             ByVal dtBookedAfterDetailInfo As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDataTable, _
                                             ByVal dtBookedAfterProcess As SC3080202DataSet.SC3080202BookedAfterProcessDataTable)

        Logger.Info("setBookedAfterDisplaySetting Start")
        '契約車両情報エリア
        '受注日
        CVDIBookingDate.Text = getColumn(dtContractCar, dtContractCar.CONTRACTDATEColumn.ColumnName)
        'SFX
        CVDISuffix.Text = getColumn(dtContractCar, dtContractCar.SUFFIXCDColumn.ColumnName)
        'VIN No.
        CVDIVIN.Text = getColumn(dtContractCar, dtContractCar.ASSIGN_TEMP_VCL_VINColumn.ColumnName)
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
        DispFlgActStatus.Value = SC3080202BusinessLogic.GetDispFlgActStatus()
        If DispFlgActStatus.Value.Equals(ACT_STATUS_DISP_FLG_ON) Then
            '車両ステイタス
            CVDIVehicleStatus.Text = getColumn(dtContractCar, dtContractCar.AFTER_ODR_ACT_STATUS_NAMEColumn.ColumnName)
            '到着予定日
            CVDIProductionDate.Text = getColumn(dtContractCar, dtContractCar.SCHE_START_DATEORTIMEColumn.ColumnName)
            'ファイナンスステイタス
            CVDIFinanceStatus.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_FINANCEColumn.ColumnName)
            'ファイナンス申請日
            CVDIFinanceApplicationDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_FINANCE_APPLICATIONColumn.ColumnName)
            'ファイナンス承認日
            CVDIFinanceApprovalDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_FINANCE_APPROVALColumn.ColumnName)
            'マッチングステイタス
            CVDIMatchingStatus.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_MATCHINGColumn.ColumnName)
        End If
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
        '振当て日
        CVDIMatchingDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_ASSIGNColumn.ColumnName)
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
        If DispFlgActStatus.Value.Equals(ACT_STATUS_DISP_FLG_ON) Then
            'VDQIステイタス
            CVDIVDQIStatus.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_VDQIColumn.ColumnName)
            'VDQIリクエスト日
            CVDIVDQIOrderDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_VDQI_REQUESTColumn.ColumnName)
            'VDQI開始日
            CVDIVDQIStartDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_VDQI_STARTColumn.ColumnName)
            'VDQI完了日
            CVDIVDQIFinishDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_VDQI_COMPLETEColumn.ColumnName)
            'PDS実施日
            CVDIPDSFinishDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_PDS_IMPLEMENTColumn.ColumnName)
            '保険登録日
            CVDIInsuranceIssueDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_INSURANCE_REGISTRATIONColumn.ColumnName)
        End If
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
        '納車日時
        CVDIDeliveryDateTime.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_DELIVERYColumn.ColumnName)
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
        If DispFlgActStatus.Value.Equals(ACT_STATUS_DISP_FLG_ON) Then
            '登録ステイタス
            CVDIRegistrationStatus.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_REGISTRATIONColumn.ColumnName)
            '登録申請日
            CVDIRegDocCollectionDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_REGISTRATION_APPLICATIONColumn.ColumnName)
            'ナンバー取得日
            CVDIRegPlateDocComplDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_NUMBER_ACQUIREColumn.ColumnName)
            'ナンバー引き渡し日
            CVDIRegistrationHandoverDate.Text = getColumn(dtBookedAfterDetailInfo, dtBookedAfterDetailInfo.AFTER_ODR_NUMBER_HANDINGColumn.ColumnName)
        End If
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
        '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

        '受注後プロセス設定
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If "1".Equals(UseAfterOdrProcFlgHidden.Value) Then
            ProcessBookedAfterRepeater.DataSource = dtBookedAfterProcess
            ProcessBookedAfterRepeater.DataBind()
        End If
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        'アイコン数
        AfterOdrPrcsIconCountHidden.Value = CStr(dtBookedAfterProcess.Rows.Count)
        '最大ページ数
        If dtBookedAfterProcess.Rows.Count > 0 Then
            Dim ciling As Decimal = CDec(dtBookedAfterProcess.Rows.Count) / BOOKED_AFTER_PROCESS_COUNT
            AfterOdrPrcsIconMaxPageHidden.Value = CStr(Math.Ceiling(ciling))
        End If

        Logger.Info("setBookedAfterDisplaySetting End")

    End Sub

    ''' <summary>
    ''' 対象テーブル、項目名より値(HTMLエンコード済み)を取得する
    ''' </summary>
    ''' <param name="dt">対象テーブル</param>
    ''' <param name="columnName">項目名</param>
    ''' <returns>値(HTMLエンコード済み)</returns>
    ''' <remarks></remarks>
    Private Function getColumn(ByVal dt As DataTable, ByVal columnName As String) As String
        Logger.Info("getColumn Start")
        Dim ret As String = String.Empty
        If dt.Rows.Count > 0 Then
            If Not dt.Rows(0).IsNull(columnName) Then
                ret = HttpUtility.HtmlEncode(CStr(dt.Rows(0).Item(columnName)).Trim())
            End If
        End If
        Logger.Info("getColumn End")
        Return ret
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END
#End Region

#Region " 編集完了時に呼び出されるダミーボタンクリックイベント "

    ''' <summary>
    ''' メモ入力確定時に呼ばれるイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CommitTodayMemoButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles commitTodayMemoButtonDummy.Click
        Logger.Info("CommitTodayMemoButtonDummy_Click Start")

        ' 入力チェック(メモが256文字以内ではない場合)
        If todayMemoTextBox.Text.Length > MEMOLENGTH Then
            ShowMessageBox(ERRMSGID_20907)
            Return
        End If

        '禁則文字チェック
        If Not Validation.IsValidString(todayMemoTextBox.Text) Then
            ShowMessageBox(ERRMSGID_20911)
            Return
        End If

        updateSalesMemoFromDataTable = New SC3080202DataSet.SC3080202UpdateSalesMemoFromDataTable
        GetSeqnoToDataTable = New SC3080202DataSet.SC3080202GetSeqnoToDataTable

        Dim updateSalesMemoFromRow As SC3080202DataSet.SC3080202UpdateSalesMemoFromRow
        Dim GetSeqnoToRow As SC3080202DataSet.SC3080202GetSeqnoToRow

        updateSalesMemoFromRow = updateSalesMemoFromDataTable.NewSC3080202UpdateSalesMemoFromRow

        If selFllwupboxSeqnoHidden.Value.Trim.Length > 0 Then
            updateSalesMemoFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
        End If

        updateSalesMemoFromRow.STRCD = selFllwupboxStrcdHidden.Value
        If String.IsNullOrEmpty(Trim(todayMemoTextBox.Text)) Then
            updateSalesMemoFromRow.MEMO = " "
        Else
            updateSalesMemoFromRow.MEMO = todayMemoTextBox.Text
        End If
        updateSalesMemoFromDataTable.Rows.Add(updateSalesMemoFromRow)

        Dim bizLogic As New SC3080202BusinessLogic
        GetSeqnoToDataTable = bizLogic.UpdateSalesMemo(updateSalesMemoFromDataTable)
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
        If GetSeqnoToDataTable Is Nothing Then
            ShowMessageBox(ERRMSGID_901)
            Exit Sub
        End If
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 END
        If GetSeqnoToDataTable.Rows.Count > 0 Then
            GetSeqnoToRow = GetSeqnoToDataTable.Rows(0)
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newActivityChange", "key", GetSeqnoToRow.FLLWUPBOX_SEQNO)
            If GetSeqnoToRow.IsFLLWUPBOX_SEQNONull Then
            Else
                selFllwupboxStrcdHidden.Value = StaffContext.Current.BrnCD
                selFllwupboxSeqnoHidden.Value = GetSeqnoToRow.FLLWUPBOX_SEQNO
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, selFllwupboxSeqnoHidden.Value)
            End If
        End If

        ' ボタン押下でSEQNOを引き渡す
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
        End If
        ' 活動情報取得
        getActivityList()
        ' 活性・非活性の制御
        setEnabled()

        ' 活動登録画面に通知
        If GetSeqnoToDataTable.Rows.Count > 0 Then
            RaiseEvent CreateFollow(Me, EventArgs.Empty)
        End If

        Logger.Info("CommitTodayMemoButtonDummy_Click End")
    End Sub

    Private Sub commitCompleteSeriesQuantiryButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles commitCompleteSeriesQuantiryButtonDummy.Click
        Logger.Info("commitCompleteSeriesQuantiryButtonDummy_Click Start")

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        Dim editflg As String
        Dim checkflg As String
        editflg = SelectSeriesEidtMode.Value
        checkflg = SelectSeriesDelMode.Value
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        ' 入力チェック
        If String.IsNullOrEmpty(inputSelectQuantiryHidden.Value) Then
            ShowMessageBox(ERRMSGID_20904)
            Exit Sub
        ElseIf ((CInt(inputSelectQuantiryHidden.Value) < 1) Or (CInt(inputSelectQuantiryHidden.Value) > 99)) Then
            ShowMessageBox(ERRMSGID_20903)
            Exit Sub
        End If

        UpdateSelectedVclCountFromDataTable = New SC3080202DataSet.SC3080202UpdateSelectedVclCountFromDataTable
        GetSeqnoToDataTable = New SC3080202DataSet.SC3080202GetSeqnoToDataTable

        Dim updateSelectedVclCountFromRow As SC3080202DataSet.SC3080202UpdateSelectedVclCountFromRow

        updateSelectedVclCountFromRow = UpdateSelectedVclCountFromDataTable.NewSC3080202UpdateSelectedVclCountFromRow

        updateSelectedVclCountFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
        updateSelectedVclCountFromRow.STRCD = selFllwupboxStrcdHidden.Value
        updateSelectedVclCountFromRow.QUANTITY = inputSelectQuantiryHidden.Value
        updateSelectedVclCountFromRow.SEQNO = selSeqnoHidden.Value
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        updateSelectedVclCountFromRow.LOCKVERSION = selLockvrHidden.Value
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        UpdateSelectedVclCountFromDataTable.Rows.Add(updateSelectedVclCountFromRow)

        Dim bizLogic As New SC3080202BusinessLogic
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
        If Not bizLogic.UpdateSelectedVclCount(UpdateSelectedVclCountFromDataTable) Then
            ShowMessageBox(ERRMSGID_901)
            Exit Sub
        End If
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        ' 希望車種取得
        getSelectedSeries()

        ScNscSelectCarAreaUpdatePanel.Update()

        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commitCompleteSelectedSeriesButtonDummyAfter", "after", editflg, checkflg, selSeqnoHidden.Value)
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        Logger.Info("commitCompleteSeriesQuantiryButtonDummy_Click End")
    End Sub


    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 競合車種選択完了イベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CommitCompleteCompButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles commitCompleteSelectedCompButtonDummy.Click
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Logger.Info("CommitCompleteCompButtonDummy_Click Start")

        UpdateSelectedCompeFromDataTable = New SC3080202DataSet.SC3080202UpdateSelectedCompeFromDataTable
        GetSeqnoToDataTable = New SC3080202DataSet.SC3080202GetSeqnoToDataTable

        Dim UpdateSelectedCompeFromRow As SC3080202DataSet.SC3080202UpdateSelectedCompeFromRow
        Dim GetSeqnoToRow As SC3080202DataSet.SC3080202GetSeqnoToRow
        ' seqnoの初期化(MAX+1)
        Dim maxseqno As Integer = 1

        ' 1行目はFollowup-box seqnoを格納しておく
        UpdateSelectedCompeFromRow = UpdateSelectedCompeFromDataTable.NewSC3080202UpdateSelectedCompeFromRow
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            UpdateSelectedCompeFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            UpdateSelectedCompeFromRow.STRCD = selFllwupboxStrcdHidden.Value
        End If
        UpdateSelectedCompeFromDataTable.Rows.Add(UpdateSelectedCompeFromRow)

        ' 項目を取得()
        Dim checkFlg As String
        For Each i As RepeaterItem In CompCarModelMasterRepeater.Items
            checkFlg = CType(i.FindControl("CompCheckState"), HiddenField).Value
            If "True".Equals(checkFlg) Then
                UpdateSelectedCompeFromRow = UpdateSelectedCompeFromDataTable.NewSC3080202UpdateSelectedCompeFromRow
                If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
                Else
                    UpdateSelectedCompeFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
                End If
                UpdateSelectedCompeFromRow.SEQNO = maxseqno
                UpdateSelectedCompeFromRow.SERIESCD = CType(i.FindControl("CompModelCd"), HiddenField).Value
                UpdateSelectedCompeFromDataTable.Rows.Add(UpdateSelectedCompeFromRow)
                maxseqno += 1
            End If
        Next


        '１件以上競合車種を選択している場合又は、Follow-up Box連番がある場合に更新
        If Not String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Or maxseqno > 1 Then


            Dim bizLogic As New SC3080202BusinessLogic
            GetSeqnoToDataTable = bizLogic.UpdateSelectedCompe(UpdateSelectedCompeFromDataTable)
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
            If GetSeqnoToDataTable Is Nothing Then
                ShowMessageBox(ERRMSGID_901)
                Exit Sub
            End If
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END
            If GetSeqnoToDataTable.Rows.Count > 0 Then
                GetSeqnoToRow = GetSeqnoToDataTable.Rows(0)
                JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newActivityChange", "key", GetSeqnoToRow.FLLWUPBOX_SEQNO)
                If GetSeqnoToRow.IsFLLWUPBOX_SEQNONull Then
                Else
                    selFllwupboxStrcdHidden.Value = StaffContext.Current.BrnCD
                    selFllwupboxSeqnoHidden.Value = GetSeqnoToRow.FLLWUPBOX_SEQNO
                    SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
                    SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, selFllwupboxSeqnoHidden.Value)
                End If
            End If

            ' ボタン押下でSEQNOを引き渡す
            If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            Else
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
            End If

        End If



        ' 活動情報取得
        getActivityList()
        ' 競合車種取得
        getSelectedCompe()
        ' 活性・非活性の制御
        setEnabled()

        ' 活動登録画面に通知
        If GetSeqnoToDataTable.Rows.Count > 0 Then
            RaiseEvent CreateFollow(Me, EventArgs.Empty)
        End If

        ScNscCompeCarAreaUpdatePanel.Update()


        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commitCompleteSelectedCompButtonDummyAfter", "after")

        Logger.Info("CommitCompleteCompButtonDummy_Click End")
    End Sub

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談条件完了イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SalesConditionCompleteButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles salesConditionCompleteButtonDummy.Click
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Logger.Info("SalesConditionCompleteButtonDummy_Click Start")

        Dim salesConditionNo As String
        Dim itemNo As String
        Dim checkFlg As String
        Dim other As String
        Dim othersalescondition As String
        Dim updateSalesConditionFromRow As SC3080202DataSet.SC3080202UpdateSalesConditionFromRow
        Dim GetSeqnoToRow As SC3080202DataSet.SC3080202GetSeqnoToRow
        updateSalesConditionFromDataTable = New SC3080202DataSet.SC3080202UpdateSalesConditionFromDataTable

        ' 1行目はFollowup-box seqnoを格納しておく
        updateSalesConditionFromRow = updateSalesConditionFromDataTable.NewSC3080202UpdateSalesConditionFromRow
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            updateSalesConditionFromRow.STRCD = selFllwupboxStrcdHidden.Value
            updateSalesConditionFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
        End If
        updateSalesConditionFromDataTable.Rows.Add(updateSalesConditionFromRow)

        For Each i As RepeaterItem In ConditionRepeater.Items
            For Each j As RepeaterItem In CType(i.FindControl("ConditionItemRepeater"), Repeater).Items
                checkFlg = CType(j.FindControl("CheckFlgHidden"), HiddenField).Value
                If "True".Equals(checkFlg) Then
                    salesConditionNo = CType(j.FindControl("SalesConditionNoHidden"), HiddenField).Value
                    itemNo = CType(j.FindControl("ItemNoHidden"), HiddenField).Value
                    other = CType(j.FindControl("OtherHidden"), HiddenField).Value
                    othersalescondition = CType(j.FindControl("OtherSalesConditionHidden"), HiddenField).Value
                    updateSalesConditionFromRow = updateSalesConditionFromDataTable.NewSC3080202UpdateSalesConditionFromRow
                    updateSalesConditionFromRow.SALESCONDITIONNO = salesConditionNo
                    updateSalesConditionFromRow.ITEMNO = itemNo
                    updateSalesConditionFromRow.OTHER = other
                    ' 入力チェック
                    If "1".Equals(other) Then
                        If String.IsNullOrEmpty(othersalescondition) Then
                            '必須
                            ShowMessageBox(ERRMSGID_20906)
                            Return
                        ElseIf othersalescondition.Length > 30 Then
                            '桁数
                            ShowMessageBox(ERRMSGID_20905)
                            Return
                        ElseIf Not Validation.IsValidString(othersalescondition) Then
                            '禁則文字
                            ShowMessageBox(ERRMSGID_20910)
                            Return
                        End If
                    End If

                    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                    If "2".Equals(other) Then
                        If String.IsNullOrEmpty(othersalescondition) Then
                            '必須
                            ShowMessageBox(LC_ERRMSGID_2020902)
                            Return
                        ElseIf othersalescondition.Length > 30 Then
                            '桁数
                            ShowMessageBox(LC_ERRMSGID_2020901)
                            Return
                        ElseIf Not Validation.IsValidString(othersalescondition) Then
                            '禁則文字
                            ShowMessageBox(LC_ERRMSGID_2020903)
                            Return
                        End If
                    End If
                    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
                    updateSalesConditionFromRow.OTHERSALESCONDITION = othersalescondition
                    updateSalesConditionFromRow.CSTCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                    updateSalesConditionFromRow.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                    updateSalesConditionFromRow.CUSTCD = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                    If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
                    Else
                        updateSalesConditionFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
                    End If
                    updateSalesConditionFromDataTable.Rows.Add(updateSalesConditionFromRow)
                End If
            Next
        Next

        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        Dim checkRslt As Integer
        checkRslt = checkDemandStructure()

        If checkRslt > 0 Then
            ShowMessageBox(checkRslt)
            Return
        End If

        Dim datatableFromRow As SC3080202DataSet.SC3080202GetSalesLocalRow = Nothing
        Dim salesLocalDataTable = New SC3080202DataSet.SC3080202GetSalesLocalDataTable

        datatableFromRow = salesLocalDataTable.NewSC3080202GetSalesLocalRow

        datatableFromRow.SALES_ID = Me.selFllwupboxSeqnoHidden.Value
        datatableFromRow.DEMAND_STRUCTURE_CD = Me.DemandStructureCd.Value

        If String.IsNullOrWhiteSpace(Me.Trade_in_MakerValue.Value) Then
            datatableFromRow.TRADEINCAR_MAKER_CD = " "
        Else
            datatableFromRow.TRADEINCAR_MAKER_CD = Me.Trade_in_MakerValue.Value
        End If

        If String.IsNullOrWhiteSpace(Me.Trade_in_ModelValue.Value) Then
            datatableFromRow.TRADEINCAR_MODEL_CD = " "
        Else
            datatableFromRow.TRADEINCAR_MODEL_CD = Me.Trade_in_ModelValue.Value
        End If

        If String.IsNullOrWhiteSpace(Me.Trade_in_MileageValue.Value) Then
            datatableFromRow.TRADEINCAR_MILE = 0
        Else
            datatableFromRow.TRADEINCAR_MILE = Me.Trade_in_MileageValue.Value
        End If

        If String.IsNullOrWhiteSpace(Me.Trade_in_ModelYearValue.Value) Then
            datatableFromRow.TRADEINCAR_MODEL_YEAR = " "
        Else
            datatableFromRow.TRADEINCAR_MODEL_YEAR = Me.Trade_in_ModelYearValue.Value
        End If

        If String.IsNullOrWhiteSpace(Me.SalesLocalLockvr.Value) Then
            datatableFromRow.ROW_LOCK_VERSION = 0
        Else
            datatableFromRow.ROW_LOCK_VERSION = Me.SalesLocalLockvr.Value
        End If

        salesLocalDataTable.Rows.Add(datatableFromRow)
        Dim bizLogic As New SC3080202BusinessLogic

        Dim msgId As Integer = 0
        GetSeqnoToDataTable = bizLogic.UpdateSalesCondition(updateSalesConditionFromDataTable, salesLocalDataTable, msgId)

        If msgId = ERRMSGID_901 Then
            'エラーメッセージを表示
            ShowMessageBox(msgId)
            Exit Sub
        Else
            Dim lockVerVal As Long
            If String.IsNullOrWhiteSpace(Me.SalesLocalLockvr.Value) Then
                lockVerVal = 0
            Else
                lockVerVal = Long.Parse(Me.SalesLocalLockvr.Value) + 1
            End If
            Me.SalesLocalLockvr.Value = lockVerVal.ToString()
            DemandStructureUpdatePanel.Update()
        End If

        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        If GetSeqnoToDataTable.Rows.Count > 0 Then
            GetSeqnoToRow = GetSeqnoToDataTable.Rows(0)
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newActivityChange", "key", GetSeqnoToRow.FLLWUPBOX_SEQNO)
            If GetSeqnoToRow.IsFLLWUPBOX_SEQNONull Then
            Else
                selFllwupboxStrcdHidden.Value = StaffContext.Current.BrnCD
                selFllwupboxSeqnoHidden.Value = GetSeqnoToRow.FLLWUPBOX_SEQNO
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, selFllwupboxSeqnoHidden.Value)
            End If
        End If

        ' ボタン押下でSEQNOを引き渡す
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
        End If
        ' 活動情報取得
        getActivityList()

        ' 活性・非活性の制御
        setEnabled()

        ' 活動登録画面に通知
        If GetSeqnoToDataTable.Rows.Count > 0 Then
            RaiseEvent CreateFollow(Me, EventArgs.Empty)
        End If

        Logger.Info("SalesConditionCompleteButtonDummy_Click End")
    End Sub

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動選択時に押下されるダミーボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CommitActivityButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles commitActivityButtonDummy.Click
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Logger.Info("CommitActivityButtonDummy_Click Start")
        Dim strDlrcd As String = StaffContext.Current.DlrCD

        ' ボタン押下でSEQNOを引き渡す
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, String.Empty)
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, StaffContext.Current.BrnCD)
        Else
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
            ' 2012/02/29 TCS 小野 【SALES_2】 START
            Dim salesbkgno As String = String.Empty
            Using datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable =
                New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
                datatableFrom.Rows.Add(strDlrcd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                salesbkgno = SC3080202BusinessLogic.GetSalesbkgno(datatableFrom)
            End Using
            SetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, salesbkgno)
            selFllwupboxSalesBkgno.Value = salesbkgno

            If Not String.IsNullOrEmpty(salesbkgno) Then
                Dim salesafterflg As String = String.Empty
                Using datatableFrom2 As ActivityInfoDataSet.ActivityInfoCountFromDataTable =
                    New ActivityInfoDataSet.ActivityInfoCountFromDataTable
                    datatableFrom2.Rows.Add(strDlrcd, selFllwupboxStrcdHidden.Value, selFllwupboxSeqnoHidden.Value)
                    salesafterflg = SC3080202BusinessLogic.CountFllwupboxRslt(datatableFrom2)
                End Using
                SetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, salesafterflg)
                selFllwupboxSalesAfterFlg.Value = salesafterflg
            Else
                SetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, String.Empty)
                selFllwupboxSalesAfterFlg.Value = String.Empty
            End If
            ' 2012/02/29 TCS 小野 【SALES_2】 END
        End If

        '活動情報取得
        getActivityList()

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        '受注前、受注時の場合

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If (Not "1".Equals(selFllwupboxSalesAfterFlg.Value)) Or ("0".Equals(UseAfterOdrProcFlgHidden.Value)) Then
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

            '希望車種ポップアップ初期化
            RefreshSeriesSelectPopup()
            '競合車種マスタ取得
            getCompeMakerMaster()
            getCompeModelMaster()
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            '用件ソース1stリスト初期設定
            initSource1List()
            '必須入力タイトル設定
            setMandatotyItemTitle()
            '商談条件取得
            getSalesCondition()
            '競合車種取得
            getSelectedCompe()
        End If
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END
        '希望車種取得
        getSelectedSeries()
        'メモ取得
        getSalesMemo()
        '活動詳細取得
        getActivityDetail()
        'プロセス取得
        getProcess()
        'ステータス取得
        getStatus()
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
        '商談情報取得
        getSalesInfoDetail()
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 END
        '活性・非活性の制御
        setEnabled()

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        '受注時、受注後の設定
        setBookedAfter()
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        '選択活動変更イベントを発生させる
        RaiseEvent ChangeFollow(Me, EventArgs.Empty)

        Logger.Info("CommitActivityButtonDummy_Click End")
    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    Protected Sub CommitCompleteSelectedSeriesButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles commitCompleteSelectedSeriesButtonDummy.Click
        Logger.Info("CommitCompleteSelectedSeriesButtonDummy_Click Start")

        ' 引数定義
        Dim modelcd As String
        Dim gradecd As String
        Dim suffixcd As String
        Dim interiorcolorcd As String
        Dim exteriorcolorcd As String
        Dim editflg As String
        Dim checkflg As String
        Dim lockvr As Long

        ' 引数取得
        modelcd = SelectModelcdHidden.Value
        gradecd = SelectGradecdHidden.Value
        suffixcd = SelectSuffixcdHidden.Value
        exteriorcolorcd = SelectExteriorColorcdHidden.Value
        interiorcolorcd = SelectInteriorColorcdHidden.Value
        editflg = SelectSeriesEidtMode.Value
        checkflg = SelectSeriesDelMode.Value
        lockvr = CLng(SelectLockvrHidden.Value)

        UpdateSelectedSeriesFromDataTable = New SC3080202DataSet.SC3080202UpdateSelectedSeriesFromDataTable
        GetSeqnoToDataTable = New SC3080202DataSet.SC3080202GetSeqnoToDataTable

        Dim UpdateSelectedSeriesFromRow As SC3080202DataSet.SC3080202UpdateSelectedSeriesFromRow
        Dim GetSeqnoToRow As SC3080202DataSet.SC3080202GetSeqnoToRow

        UpdateSelectedSeriesFromRow = UpdateSelectedSeriesFromDataTable.NewSC3080202UpdateSelectedSeriesFromRow
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            UpdateSelectedSeriesFromRow.FLLWUPBOX_SEQNO = selFllwupboxSeqnoHidden.Value
            UpdateSelectedSeriesFromRow.STRCD = selFllwupboxStrcdHidden.Value
        End If
        If editflg = 1 Then
            UpdateSelectedSeriesFromRow.SEQNO = selSeqnoHidden.Value
        End If
        UpdateSelectedSeriesFromRow.QUANTITY = QUANTITY_DEFAULT
        UpdateSelectedSeriesFromRow.SERIESCD = modelcd
        UpdateSelectedSeriesFromRow.MODELCD = gradecd


        If String.IsNullOrEmpty(suffixcd) Then
            UpdateSelectedSeriesFromRow.SUFFIX_CD = " "
        Else
            UpdateSelectedSeriesFromRow.SUFFIX_CD = suffixcd
        End If

        If String.IsNullOrEmpty(exteriorcolorcd) Then
            UpdateSelectedSeriesFromRow.COLORCD = " "
        Else
            UpdateSelectedSeriesFromRow.COLORCD = exteriorcolorcd
        End If

        If String.IsNullOrEmpty(interiorcolorcd) Then
            UpdateSelectedSeriesFromRow.INTERIORCLR_CD = " "
        Else
            UpdateSelectedSeriesFromRow.INTERIORCLR_CD = interiorcolorcd
        End If

        UpdateSelectedSeriesFromRow.EDITFLG = editflg
        UpdateSelectedSeriesFromRow.CHECKFLG = checkflg
        UpdateSelectedSeriesFromRow.LOCKVERSION = lockvr

        If Me.SelectMostPreferredHidden.Value.Trim().Length = 0 Then
            UpdateSelectedSeriesFromRow.MOST_PREF_VCL_FLG = "0"
        Else
            UpdateSelectedSeriesFromRow.MOST_PREF_VCL_FLG = Me.SelectMostPreferredHidden.Value
        End If

        UpdateSelectedSeriesFromDataTable.Rows.Add(UpdateSelectedSeriesFromRow)

        Dim bizLogic As New SC3080202BusinessLogic
        GetSeqnoToDataTable = bizLogic.UpdateSelectedSeries(UpdateSelectedSeriesFromDataTable, msgid)

        If msgid = ERRMSGID_901 Then
            'エラーメッセージを表示
            ShowMessageBox(msgid)
            Exit Sub
        End If

        If ERRMSGID_20902 = msgid Then
            'エラーメッセージを表示
            Dim tempArray As String() = {modelcd, gradecd, suffixcd, exteriorcolorcd, interiorcolorcd}
            ShowMessageBox(msgid, tempArray)
            Exit Sub
        End If

        If GetSeqnoToDataTable.Rows.Count > 0 Then
            GetSeqnoToRow = GetSeqnoToDataTable.Rows(0)
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newActivityChange", "key", GetSeqnoToRow.FLLWUPBOX_SEQNO)
            If GetSeqnoToRow.IsFLLWUPBOX_SEQNONull Then
            Else
                selFllwupboxStrcdHidden.Value = StaffContext.Current.BrnCD
                selFllwupboxSeqnoHidden.Value = GetSeqnoToRow.FLLWUPBOX_SEQNO
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, selFllwupboxSeqnoHidden.Value)
            End If
            If GetSeqnoToRow.IsSEQNull Then
            Else
                selSeqnoHidden.Value = GetSeqnoToRow.SEQ
                selModelcdHidden.Value = modelcd
                selGradecdHidden.Value = gradecd
                selSuffixcdHidden.Value = suffixcd
                selInteriorColorcdHidden.Value = interiorcolorcd
                selExteriorColorcdHidden.Value = exteriorcolorcd
                selLockvrHidden.Value = lockvr
                Me.selMostPreferredHidden.Value = UpdateSelectedSeriesFromRow.MOST_PREF_VCL_FLG
            End If
        End If

        ' ボタン押下でSEQNOを引き渡す
        If String.IsNullOrEmpty(selFllwupboxSeqnoHidden.Value) Then
        Else
            SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)
            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
        End If

        ' 活動情報取得
        getActivityList()

        ' 希望車種取得
        getSelectedSeries()

        ' 活性・非活性の制御
        setEnabled()

        ' 活動登録画面に通知
        RaiseEvent ChangeSelectedSeries(Me, EventArgs.Empty)
        If GetSeqnoToDataTable.Rows.Count > 0 Then
            RaiseEvent CreateFollow(Me, EventArgs.Empty)
        End If

        ScNscSelectCarAreaUpdatePanel.Update()

        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commitCompleteSelectedSeriesButtonDummyAfter", "after", editflg, checkflg, selSeqnoHidden.Value)

        Logger.Info("CommitCompleteSelectedSeriesButtonDummy_Click End")

    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END



    '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
    ''' <summary>
    ''' 活動きっかけ編集
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>活動きっかけ編集完了時に発生するイベント</remarks>
    Protected Sub CommitSource1ButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
        If Not (hdnLastSource1.Value().Equals(Me.Source1SelectedCodeHidden.Value)) Then
            '入力前の値から変更なしでボタンを押下した場合はそのまま何もせず処理を抜ける
            Dim rowlockversion As Decimal = 0
            Dim ret As Boolean = False
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
            Dim salesId As Decimal = 0
            Dim source1Code As Long = 0
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            Dim biz As SC3080202BusinessLogic = Nothing

            Try
                If Decimal.TryParse(selFllwupboxSeqnoHidden.Value, salesId) _
                    AndAlso Long.TryParse(Me.Source1SelectedCodeHidden.Value, source1Code) Then
                    biz = New SC3080202BusinessLogic()

                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)更新 start
                    ret = biz.UpdateSourceOfACard(salesId, source1Code)
                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)更新 end

                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                    If (ret) Then
                        Decimal.TryParse(hdnLTRowLockVersion.Value, rowlockversion)

                        '商談ローカルテーブルのソース1編集可能フラグのみを設定する
                        Decimal.TryParse(hdnLTRowLockVersion.Value, rowlockversion)
                        ret = biz.UpdateSourceFlg_Local(salesId, True, False, rowlockversion)

                        '商談ローカルのソース2を0にする（リセット）
                        ret = biz.UpdateSource2(salesId, 0, rowlockversion)
                        initSource2List(Me.Source1SelectedCodeHidden.Value, True)
                    End If
                    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                End If
            Finally
                If Not biz Is Nothing Then biz = Nothing

                '最新状態に更新(更新エラーの場合でも現在のDB値を表示する。)
                getSalesInfoDetail()

                '活動情報取得
                getActivityList()
                '活性制御(入力チェック)呼び出し
                Me.setEnabled()

            End Try
            Return
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
        Else
            Return
        End If
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end

    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 一押し希望車種変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub MostPreferredUpdateDummyButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '編集モード設定
        Me.SelectSeriesEidtMode.Value = "1"
        Me.SelectSeriesDelMode.Value = "0"

        '選択中の希望車両の一押しフラグを立てる
        Me.selMostPreferredHidden.Value = "1"

        '現在選択中の希望車種情報を更新情報に設定
        Me.SelectModelcdHidden.Value = Me.selModelcdHidden.Value
        Me.SelectGradecdHidden.Value = Me.selGradecdHidden.Value
        Me.SelectSuffixcdHidden.Value = Me.selSuffixcdHidden.Value
        Me.SelectExteriorColorcdHidden.Value = Me.selExteriorColorcdHidden.Value
        Me.SelectInteriorColorcdHidden.Value = Me.selInteriorColorcdHidden.Value
        Me.SelectSeqnoHidden.Value = Me.selSeqnoHidden.Value
        Me.SelectLockvrHidden.Value = Me.selLockvrHidden.Value
        Me.SelectMostPreferredHidden.Value = Me.selMostPreferredHidden.Value

        '希望車種更新処理呼び出し
        Me.CommitCompleteSelectedSeriesButtonDummy_Click(sender, e)

    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END
    '2013/12/09 TCS 市川 Aカード情報相互連携開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 注文番号タップ時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AcardNumOrContractNumDummyButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AcardNumOrContractNumDummyButton.Click
        Logger.Info("AcardNumOrContractNumDummyButton_Click Start")

        'セッション情報をセット
        'メニューロック状態
        Me.SetValue(ScreenPos.Next, SESSION_KEY_READONLYFLG, False)
        '見積管理ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_ESTIMATEID, CLng(HttpUtility.HtmlDecode(EstimateIdHidden.Value)))
        '選択している見積管理IDのindex
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SELECTEDESTIMATEINDEX, "0")
        '未保存フラグ
        If getStatusSales() Then
            '商談中または営業活動中 または一時対応中 または納車作業中または納車作業中(一時対応)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_BUSINESSFLG, True)
        Else
            '上記以外
            Me.SetValue(ScreenPos.Next, SESSION_KEY_BUSINESSFLG, False)
        End If


        '見積作成画面へ遷移するためのフラグを立てる
        AcardNumOrContractNumRedirectFlg.Value = "1"

        Logger.Info("AcardNumOrContractNumDummyButton_Click End")
    End Sub

    ''' <summary>
    ''' 受注後プロセス欄作成
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ProcessBookedAfterRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ProcessBookedAfterRepeater.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim row As SC3080202DataSet.SC3080202BookedAfterProcessRow = DirectCast(e.Item.DataItem.row, SC3080202DataSet.SC3080202BookedAfterProcessRow)
            Dim li As HtmlGenericControl = DirectCast(e.Item.FindControl("ProcessBookedAfterLi"), HtmlGenericControl)
            Dim label As Label = DirectCast(e.Item.FindControl("ProcessBookedAfterTitleLabel"), Label)

            'タイトル、アイコンパス設定
            If Not row.IsRSLT_DATENull AndAlso Not String.IsNullOrWhiteSpace(row.RSLT_DATE) Then
                label.Text = HttpUtility.HtmlEncode(row.RSLT_DATE)
                li.Attributes("IconPath") = row.ICON_PATH_ON
            Else
                label.Text = HttpUtility.HtmlEncode(row.AFTER_ODR_PRCS_NAME)
                li.Attributes("IconPath") = row.ICON_PATH_OFF
                li.Attributes("class") = li.Attributes("class") & " On"
            End If

        End If
    End Sub
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END
#End Region

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub RemoveValue(ByVal pos As ScreenPos, ByVal key As String)
        GetPageInterface().RemoveValueBypass(pos, key)
    End Sub

    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function

    ''' <summary>
    ''' 顧客詳細の活動登録後に呼ばれるリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshSalesCondition() Implements ISC3080202Control.RefreshSalesCondition
        Logger.Info("RefreshSalesCondition Start")

        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW) Then
            '新規Follow-up boxに対して活動登録した場合、セッションから削除
            RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW)
        End If

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        ' 活動情報取得
        getActivityList()

        '受注前、受注時の場合

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If (Not "1".Equals(selFllwupboxSalesAfterFlg.Value)) Or ("0".Equals(UseAfterOdrProcFlgHidden.Value)) Then
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

            '希望車種ポップアップ初期化
            RefreshSeriesSelectPopup()
            ' 競合車種マスタ取得
            getCompeMakerMaster()
            getCompeModelMaster()
            ' 商談条件取得
            getSalesCondition()
            ' 競合車種取得
            getSelectedCompe()
        End If

        ' 希望車種取得
        getSelectedSeries()
        ' メモ取得
        getSalesMemo()
        ' 活動詳細取得
        getActivityDetail()
        ' プロセス取得
        getProcess()
        ' ステータス取得
        getStatus()
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
        '商談情報取得
        getSalesInfoDetail()
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 END
        ' 活性・非活性の制御
        setEnabled()

        '受注時、受注後の設定
        setBookedAfter()
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, selFllwupboxSeqnoHidden.Value)
        SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, selFllwupboxStrcdHidden.Value)

        Logger.Info("RefreshSalesCondition End")
    End Sub

    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' 商談ステータスが変更された場合に呼ばれる関数 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReflectionActivityStatus() Implements ISC3080202Control.ReflectionActivityStatus
        Logger.Info("ReflectionActivityStatus Start")

        '活動情報取得
        getActivityList()

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        '受注前、受注時の場合

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If (Not "1".Equals(selFllwupboxSalesAfterFlg.Value)) Or ("0".Equals(UseAfterOdrProcFlgHidden.Value)) Then
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

            '希望車種ポップアップ初期化
            RefreshSeriesSelectPopup()
            ' 競合メーカマスタ取得
            getCompeMakerMaster()
            ' 競合車種マスタ取得
            getCompeModelMaster()
            '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除
            '用件ソース1stリスト初期設定
            initSource1List()
            '必須入力タイトル設定
            setMandatotyItemTitle()
            ' 商談条件取得
            getSalesCondition()
        End If
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        If NewActivityFlgHidden.Value.Equals("True") Then
            '新規活動が追加されている

            ' メモエリア初期化
            If Not getSalesMemoHisToDataTable Is Nothing Then
                getSalesMemoHisToDataTable.Dispose()
            End If
            MemoRepeater.DataSource = getSalesMemoHisToDataTable
            MemoRepeater.DataBind()

            ' 希望車種エリア初期化
            If Not getSelectedSeriesToDataTable Is Nothing Then
                getSelectedSeriesToDataTable.Dispose()
            End If
            SelectedCarRepeater.DataSource = getSelectedSeriesToDataTable
            SelectedCarRepeater.DataBind()

            ' 競合車種エリア初期化
            otherCountHidden.Value = 0
            If Not getSelectedCompeToDataTable Is Nothing Then
                getSelectedCompeToDataTable.Dispose()
            End If
            CompeRepeater.DataSource = getSelectedCompeToDataTable
            CompeRepeater.DataBind()

            ' 活動詳細初期化
            dispContactname.Text = String.Empty
            dispSalesStartTime.Text = String.Empty
            dispWalkinnum.Text = String.Empty
            dispAccount.Text = String.Empty
            accountOperationHidden.Value = String.Empty

            ' プロセスアイコン設定
            SetProcessIconAttr()

            ' プロセスエリア初期化
            If Not getProcessToDataTable Is Nothing Then
                getProcessToDataTable.Dispose()
            End If

            ProcessRepeater.DataSource = getProcessToDataTable
            ProcessRepeater.DataBind()

            ' ステータスエリア初期化
            CrActResult.Src = String.Empty
        Else
            '新規活動が追加されていない
            '受注前、受注時の場合

            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
            If (Not "1".Equals(selFllwupboxSalesAfterFlg.Value)) Or ("0".Equals(UseAfterOdrProcFlgHidden.Value)) Then
                '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

                ' 競合車種取得
                getSelectedCompe()

            End If
            ' 希望車種取得
            getSelectedSeries()
            ' メモ取得
            getSalesMemo()
            ' 活動詳細取得
            getActivityDetail()
            ' プロセス取得
            getProcess()
            ' ステータス取得
            getStatus()
            '2014/02/12 TCS 山口 受注後フォロー機能開発 END
        End If

        '2013/12/09 TCS 市川 Aカード情報相互連携開発 START
        '商談情報取得
        getSalesInfoDetail()
        '2013/12/09 TCS 市川 Aカード情報相互連携開発 END

        ' 活性・非活性の制御
        setEnabled()

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        '受注時、受注後の設定
        setBookedAfter()
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        '2012/04/24 TCS 河原 【SALES_2】営業キャンセルでシステムエラー (号口課題 No.111) 本対応 START
        '選択活動変更イベントを発生させる
        'RaiseEvent ChangeFollow(Me, EventArgs.Empty)
        '2012/04/24 TCS 河原 【SALES_2】営業キャンセルでシステムエラー (号口課題 No.111) 本対応 END

        Logger.Info("ReflectionActivityStatus End")
    End Sub
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

#End Region

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start
#Region "ポップアップ処理"
    ''' <summary>
    ''' 希望車種選択ポップアップ起動処理(新規)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SeriesSelectPopupButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SeriesSelectPopupButtonDummy.Click
        Logger.Info("SeriesSelectPopupButtonDummy_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.PopUpList01PanelArea.Visible = True
        Me.PopUpList02PanelArea.Visible = True
        Me.PopUpList03PanelArea.Visible = True
        Me.PopUpList04PanelArea.Visible = True
        Me.PopUpList05PanelArea.Visible = True

        '希望車種ポップアップ初期化
        RefreshSeriesSelectPopup()

        '希望車種取得
        getSelectedSeries()

        '活動情報取得
        getActivityList()

        '活性・非活性の制御
        setEnabled()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showSeriesSelect", "startup")

        Logger.Info("SeriesSelectPopupButtonDummy_Click End")
    End Sub

    ''' <summary>
    ''' 希望車種選択ポップアップ起動処理(更新)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SeriesSelectPopupUpdateButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SeriesSelectPopupUpdateButtonDummy.Click
        Logger.Info("SeriesSelectPopupUpdateButtonDummy_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.PopUpList01PanelArea.Visible = True
        Me.PopUpList02PanelArea.Visible = True
        Me.PopUpList03PanelArea.Visible = True
        Me.PopUpList04PanelArea.Visible = True
        Me.PopUpList05PanelArea.Visible = True

        '希望車種ポップアップ初期化
        RefreshSeriesSelectPopup()

        '希望車種取得
        getSelectedSeries()

        '活動情報取得
        getActivityList()

        '活性・非活性の制御
        setEnabled()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showSeriesSelectUpdate", "startup")

        Logger.Info("SeriesSelectPopupUpdateButtonDummy_Click End")
    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 競合車種選択ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CompCarSelectPopupButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CompCarSelectPopupButtonDummy.Click
        Logger.Info("CompCarSelectPopupButtonDummy_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.CompPopUpList01PanelArea.Visible = True
        Me.CompPopUpList02PanelArea.Visible = True

        '競合車種マスタ取得
        getCompeMakerMaster()
        getCompeModelMaster()

        '競合車種取得
        getSelectedCompe()

        '活動情報取得
        getActivityList()

        '活性・非活性の制御
        setEnabled()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showCompCarSelect", "startup")

        Logger.Info("CompCarSelectPopupButtonDummy_Click End")
    End Sub

    '2012/03/27 TCS 松野 【SALES_2】 START
    ''' <summary>
    ''' 活動結果ダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ActivityPopUpdateDummyButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles activityPopUpdateDummyButton.Click
        Logger.Info("ActivityPopUpdateDummyButton_Click Start")

        '活動結果一覧表示
        Me.activityPopPanel.Visible = True

        '活動情報取得
        getActivityList()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showPopUpActivityEnd", "startup")

        Logger.Info("ActivityPopUpdateDummyButton_Click End")
    End Sub
    '2012/03/27 TCS 松野 【SALES_2】 END

#End Region
    '2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' 活動結果取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>１回のページライフサイクル内でのクエリ重複呼び出し回避対策</remarks>
    Private ReadOnly Property ActivityResult(ByVal salesId As Decimal) As String
        Get
            Dim dtRet As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable = Nothing
            If String.IsNullOrEmpty(Me._activityresult) Then
                Using dt As New ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable()
                    'DataAccess層にてDLR_CD,STR_CD未使用のため不要
                    dt.AddActivityInfoGetStatusFromRow(" ", " ", salesId)
                    dtRet = SC3080202BusinessLogic.GetStatus(dt)
                    If Not dtRet Is Nothing AndAlso dtRet.Count > 0 AndAlso Not dtRet(0).IsCRACTRESULTNull Then
                        Me._activityresult = dtRet(0).CRACTRESULT
                    Else
                        Me._activityresult = String.Empty
                    End If
                    dtRet.Dispose()
                End Using
            End If
            Return Me._activityresult
        End Get
    End Property
    Private _activityresult As String
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 直販フラグ更新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CommitdDirectBillingDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim salesId As Decimal = 0
        Dim directBillingFlg As String

        If Me.SelectedDirectBilling.Checked Then
            directBillingFlg = "1"
        Else
            directBillingFlg = "0"
        End If

        Dim biz As SC3080202BusinessLogic = Nothing

        Try
            If Decimal.TryParse(selFllwupboxSeqnoHidden.Value, salesId) Then
                biz = New SC3080202BusinessLogic()
                biz.UpdateDirectBilling(salesId, directBillingFlg)
            End If
        Finally
            If Not biz Is Nothing Then biz = Nothing
        End Try

    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 END

#Region "TKMローカル"

    Private Const TRADE_IN_CAR_ENABLED_ON As String = "1"
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
    ''' <summary>
    ''' 活動きっかけ２編集
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>活動きっかけ２編集完了時に発生するイベント</remarks>
    Protected Sub CommitSource2ButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommitSource2ButtonDummy.Click
        Logger.Debug("CommitSource2ButtonDummy_Click Start")
        If Not (Me.hdnLastSource2.Value().Equals(Me.Source2SelectedCodeHidden.Value)) Then
            '入力前の値から変更なしでボタンを押下した場合は処理を抜ける

            Dim salesId As Decimal = 0
            Dim rowlockversion As Decimal = 0
            Dim source2Code As Long = 0
            Dim biz As SC3080202BusinessLogic = Nothing
            Dim ret As Boolean = False
            If Not biz Is Nothing Then biz = Nothing

            Try
                If Decimal.TryParse(Me.selFllwupboxSeqnoHidden.Value, salesId) _
                    AndAlso Long.TryParse(Me.Source2SelectedCodeHidden.Value, source2Code) Then
                    biz = New SC3080202BusinessLogic()
                    Decimal.TryParse(Me.hdnLTRowLockVersion.Value, rowlockversion)
                    ret = biz.UpdateSource2(salesId, source2Code, rowlockversion)
                    If (ret) Then
                        '更新後の値に変更
                        Me.hdnLastSource2.Value() = Me.Source2SelectedCodeHidden.Value
                    End If
                End If
            Finally
                If Not biz Is Nothing Then biz = Nothing

                '最新状態に更新(更新エラーの場合でも現在のDB値を表示する。)
                getSalesInfoDetail()

                '活動情報取得
                getActivityList()
                '活性制御(入力チェック)呼び出し
                Me.setEnabled()

            End Try
        Else
            RefreshSource2Popup(Me.Source1SelectedCodeHidden.Value)
            Me.UpdSource2Selector.Update()
        End If

        Logger.Debug("CommitSource2ButtonDummy_Click End")
    End Sub

    ''' <summary>
    ''' 活動きっかけ２選択ポップアップ初期化
    ''' </summary>
    ''' <remarks>活動きっかけ２選択ポップアップ初期化 privateメソッド</remarks>
    Private Sub RefreshSource2Popup(ByVal source1Cd As Long)

        Dim dt As SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable = Nothing
        dt = SC3080202BusinessLogic.GetSources2Master(source1Cd)
        Me.Source2ListRepeater.DataSource = dt
        Me.Source2ListRepeater.DataBind()

    End Sub
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end

    ''' <summary>
    ''' 商談ローカル取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSalesLocal(ByVal salesId As Decimal)

        Dim SalesLocalDataTable As SC3080202DataSet.SC3080202GetSalesLocalDataTable
        SalesLocalDataTable = SC3080202BusinessLogic.GetSalesLocal(salesId)

        If SalesLocalDataTable.Count > 0 Then
            Dim SalesLocalDatarow As SC3080202DataSet.SC3080202GetSalesLocalRow
            SalesLocalDatarow = SalesLocalDataTable.Rows(0)

            Me.DemandStructureCd.Value = Trim(SalesLocalDatarow.DEMAND_STRUCTURE_CD)


            Me.Trade_in_MakerValue.Value = Trim(SalesLocalDatarow.TRADEINCAR_MAKER_CD)

            If SalesLocalDatarow.IsMAKER_NAMENull Then
                Me.Trade_in_MakerName.Value = ""
            Else
                Me.Trade_in_MakerName.Value = SalesLocalDatarow.MAKER_NAME
            End If

            Me.Trade_in_ModelValue.Value = Trim(SalesLocalDatarow.TRADEINCAR_MODEL_CD)

            If SalesLocalDatarow.IsMODEL_NAMENull Then
                Me.Trade_in_ModelName.Value = ""
            Else
                Me.Trade_in_ModelName.Value = SalesLocalDatarow.MODEL_NAME
            End If

            '0の場合、未入力扱い
            If SalesLocalDatarow.TRADEINCAR_MILE = 0 Then
                Me.Trade_in_MileageValue.Value = ""
            Else
                Me.Trade_in_MileageValue.Value = SalesLocalDatarow.TRADEINCAR_MILE
            End If

            Me.Trade_in_ModelYearValue.Value = Trim(SalesLocalDatarow.TRADEINCAR_MODEL_YEAR)

            Me.SalesLocalLockvr.Value = SalesLocalDatarow.ROW_LOCK_VERSION

            DemandStructureUpdatePanel.Update()

        End If

    End Sub

    ''' <summary>
    ''' 購入分類マスタ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getDemandStructure()

        Dim getDemandStructureDataTable = New SC3080202DataSet.SC3080202GetDemandStructureLocalDataTable

        getDemandStructureDataTable = SC3080202BusinessLogic.GetDemandStructure()

        DemandStructureItemRepeater.DataSource = getDemandStructureDataTable
        DemandStructureItemRepeater.DataBind()

    End Sub

    ''' <summary>
    ''' 下取車両メーカーマスタ選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Trade_in_MakerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Trade_in_MakerButton.Click
        'パネルを表示
        Me.Trade_in_MakerPanel.Visible = True

        'データ取得
        Trade_in_MakerRepeater.DataBind()

        '反映させる為に更新
        Me.Trade_in_MakerUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "Trade_in_MakerPageOpenEnd", "startup")
    End Sub

    ''' <summary>
    ''' 下取車両モデルマスタ選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Trade_in_ModelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Trade_in_ModelButton.Click
        'パネルを表示
        Me.Trade_in_ModelPanel.Visible = True

        'データ取得
        Trade_in_ModelRepeater.DataBind()

        '反映させる為に更新
        Me.Trade_in_ModelUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "Trade_in_ModelPageOpenEnd", "startup")
    End Sub

    ''' <summary>
    ''' 下取車両年式マスタ選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Trade_in_ModelYearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Trade_in_ModelYearButton.Click
        'パネルを表示
        Me.Trade_in_ModelYearPanel.Visible = True

        'データ取得
        Trade_in_ModelYearRepeater.DataBind()

        '反映させる為に更新
        Me.Trade_in_ModelYearUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "Trade_in_ModelYearPageOpenEnd", "startup")
    End Sub

    ''' <summary>
    ''' 購入分類関連情報チェック
    ''' </summary>
    ''' <returns>ReturnId:入力チェック結果</returns>
    ''' <remarks></remarks>
    Private Function checkDemandStructure() As Integer

        Dim ReturnId As Integer = 0

        '購入分類が選択されているか
        If String.IsNullOrWhiteSpace(Me.DemandStructureCd.Value) Then
            ReturnId = LC_ERRMSGID_2020914

            Return ReturnId
        End If

        '下取車両が入力可能な場合、下取車両情報が入力されているか
        If String.Equals(Me.TradeinEnabledFlg.Value, TRADE_IN_CAR_ENABLED_ON) Then

            '下取車両メーカー選択されているか
            If String.IsNullOrWhiteSpace(Me.Trade_in_MakerValue.Value) Then
                ReturnId = LC_ERRMSGID_2020915
                Return ReturnId
            End If

            '下取車両モデルが選択されているか
            If String.IsNullOrWhiteSpace(Me.Trade_in_ModelValue.Value) Then
                ReturnId = LC_ERRMSGID_2020916
                Return ReturnId
            End If

            '下取車両走行距離が入力されているか
            If String.IsNullOrWhiteSpace(Me.Trade_in_MileageValue.Value) Then
                ReturnId = LC_ERRMSGID_2020906
                Return ReturnId
            End If

            '下取車両走行距離が20文字を超えている
            If (Validation.IsCorrectDigit(Me.Trade_in_MileageValue.Value, 20) = False) Then
                ReturnId = LC_ERRMSGID_2020907
                Return ReturnId
            End If

            '下取車両走行距離が数値以外
            Dim parseRslt As Double
            If (Not Double.TryParse(Me.Trade_in_MileageValue.Value, parseRslt)) Then
                ReturnId = LC_ERRMSGID_2020908
                Return ReturnId
            End If

            '下取車両走行距離を小数点で分割
            Dim splittedMile As String() = Me.Trade_in_MileageValue.Value.Split("."c)
            '分割後の要素数が１より大きい＝小数点が入力されている場合
            If splittedMile.Length > 1 Then
                '小数点以下の桁数が４桁より大きい場合はエラー
                If splittedMile(1).Length > 4 Then
                    ReturnId = LC_ERRMSGID_2020912
                    Return ReturnId
                End If
            End If

            '下取車両年式が選択されているか
            If String.IsNullOrWhiteSpace(Me.Trade_in_ModelYearValue.Value) Then
                ReturnId = LC_ERRMSGID_2020917
                Return ReturnId
            End If

        End If

        Return ReturnId
    End Function

#End Region

End Class
