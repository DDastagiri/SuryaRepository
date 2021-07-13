'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' SC3140103.aspx.vb
'─────────────────────────────────────
'機能: メインメニュー(SA) コードビハインド
'補足: 
'作成: 2012/01/16 KN   森下
'更新: 2012/03/13 KN   森下  【SERVICE_1】課題管理番号-KN_0312_TK_1の対応修正 追加作業承認画面の遷移先ID変更
'更新: 2012/03/21 KN   上田  【SERVICE_1】仕様変更対応(追加作業関連の画面遷移先変更)
'更新: 2012/04/04 KN   森下  【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No77の対応 引数でスペースは渡さない
'更新：2012/04/05 KN   西田  【SERVICE_1】プレユーザーテスト課題No.29 R/O作成画面遷移時の予約IDの引数変更対応
'更新：2012/04/09 KN   森下  【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応
'更新：2012/06/18 KN   西岡  【SERVICE_2】事前準備対応
'更新：2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加)
'更新：2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
'更新：2012/08/03 TMEJ 彭  サービス緊急課題対応（受付登録機能）
'更新：2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66）
'更新：2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）
'更新：2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
'更新：2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発
'更新：2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応
'更新：2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1
'更新：2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
'更新：2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34）
'更新：2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
'更新：2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」
'更新：2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
'更新：2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応
'更新：2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新：2013/04/25 TMEJ 小澤 【開発】ITxxxx_TSL自主研緊急対応（サービス）
'更新：2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新：2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
'更新：2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発
'更新：2014/07/01 TMEJ 丁 　TMT_UAT対応
'更新：2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正
'更新：2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定
'更新：2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
'更新：2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新：2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新：2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証
'更新：2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成
'更新：2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新：
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Data
Imports System.Reflection
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.UsersDataSet

Imports Toyota.eCRB.SystemFrameworks.Web

Imports Toyota.eCRB.iCROP.BizLogic.SC3140103
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103

' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END

'2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
'2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）　END

'2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSet
'2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END


'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STAT

'2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

''Imports Toyota.eCRB.iCROP.DataAccess.IC3810101
''Imports Toyota.eCRB.iCROP.BizLogic.IC3810101

''2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


''2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.DataAccess.IC3801901
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.BizLogic.IC3801901


''2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） END


''2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 START
'Imports Toyota.eCRB.iCROP.BizLogic.IC3802102
''2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
'通知用
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.BizLogic

'2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

'2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 START

Imports Toyota.eCRB.SystemFrameworks.Web.Controls

'2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 END

Partial Class Pages_SC3140103
    Inherits BasePage

#Region "定数定義"

    ' 画面ID
    Private Const APPLICATIONID As String = "SC3140103"
    Private Const MAINMENUID As String = "SC3140103"
    'Private Const APPLICATIONID_CUSTOMERNEW As String = "SC3080207"   ' 新規顧客登録画面
    'Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080208"   ' 顧客詳細画面
    'Private Const APPLICATIONID_ORDERNEW As String = "SC3160213"      ' R/O作成画面
    'Private Const APPLICATIONID_ORDEROUT As String = "SC3160208"      ' R/O参照画面
    'Private Const APPLICATIONID_ORDERLIST As String = "SC3160101"     ' R/O一覧画面
    'Private Const APPLICATIONID_WORK As String = "SC3170201"          ' 追加作業登録画面
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
    Private Const VISIT_MANAGEMENT_LIST_PAGE As String = "SC3100303"  ' 来店管理画面ID
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
    '2012/03/13 KN 森下【SERVICE_1】課題管理番号-KN_0312_TK_1の対応修正 START
    'Private Const C_APPLICATIONID_APPROVAL As String = "SC3170301"      ' 追加作業承認画面
    Private Const APPLICATIONID_APPROVAL As String = "SC3170201"      ' 追加作業承認画面(画面IDは同じだが遷移パラメータで遷移先変更される)
    '2012/03/13 KN 森下【SERVICE_1】課題管理番号-KN_0312_TK_1の対応修正 END
    'Private Const APPLICATIONID_CHECKSHEET As String = "SC3180202"    ' チェックシート印刷画面
    Private Const APPLICATIONID_SETTLEMENT As String = "SC3160207"    ' 清算入力画面
    Private Const APPLICATIONID_ADD_LIST As String = "SC3170101"      ' 追加作業一覧画面

    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) START
    Private Const APPLICATIONID_WORK_PREVIEW As String = "SC3170302"  ' 追加作業プレビュー画面
    Private Const APPLICATIONID_WORK_OUT As String = "SC3170203"      ' 追加作業入力(参照)画面
    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) END

    ' 最大表示時間(秒・分)
    Private Const MAX_TIME As Long = (99 * 60 + 59)
    Private Const MAX_TIME_DISP As String = "99'59"
    Private Const MIN_TIME_DISP As String = ""

    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) START
    Private Const SC3170201_EDIT_FLAG_NEW_EDIT As String = "0"                  '編集フラグ(0: 新規/編集)
    Private Const SC3170203_EDIT_FLAG_EDIT As String = "0"                      '編集フラグ(0: 編集)
    Private Const SC3170203_EDIT_FLAG_PREVIEW As String = "1"                   '編集フラグ(1: 参照)
    Private Const SC3170302_EDIT_FLAG_EDIT As String = "0"                      '編集フラグ(0: 編集)
    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) END

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RET_SUCCESS As Long = 0
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RET_DBTIMEOUT As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RET_NOMATCH As Long = 902
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    ''' <summary>
    ''' エラー:排他エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RET_EXCLUSION As Long = 903
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RET_DIFFSACODE As Long = 1

    ' 2012/02/27 KN 西田【SERVICE_1】START
    Private Const C_DEFAULT_CHIP_SPACE As String = "&nbsp;"
    ' 2012/02/27 KN 西田【SERVICE_1】END

    ' 2012/03/30 KN 森下【SERVICE_1】R/O作成画面遷移時の予約IDの引数変更対応 START
    Private Const REZID_NONE_FIRST_VALUE As Long = -1
    ' 2012/03/30 KN 森下【SERVICE_1】R/O作成画面遷移時の予約IDの引数変更対応 END
    ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 START
    Private Const VISITSEQ_NONE_FIRST_VALUE As Long = -1
    ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 END
    ''' <summary>ストール予約</summary>
    Private Const TBL_STALLREZINFO As String = "TBL_STALLREZINFO"
    ''' <summary>サービス来店者管理</summary>
    Private Const TBL_SERVICE_VISIT_MANAGEMENT As String = "TBL_SERVICE_VISIT_MANAGEMENT"
    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    ''' <summary>顧客情報リストの高さ</summary>
    Private Const SEARCH_LIST_HEIGHT As Long = 84
    ''' <summary>来店連番、予約IDのデフォルト値</summary>
    Private Const DEFAULT_LONG_VALUE As Long = -1
    ''' <summary>
    ''' 詳細ポップアップのマーク：非表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_MARK_INACTIVE As String = "0"
    ''' <summary>
    ''' 詳細ポップアップのマーク：表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_MARK_ACTIVE As String = "1"
    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' 詳細ポップアップのマーク2：表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_MARK_ACTIVE_2 As String = "2"
    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
    ''' <summary>
    ''' 予約_受付納車区分:Waiting
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_RECEPTION_WAITING As String = "0"
    ''' <summary>
    ''' 予約_受付納車区分:Drop off
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_RECEPTION_DROP_OFF As String = "4"

    ''' <summary>
    ''' ステータスコード：101：新規お客様登録待ち
    ''' </summary>
    Private Const StatusCodeLeft101 As String = "101"
    ''' <summary>
    ''' ステータスコード：102：R/O作成待ち
    ''' </summary>
    Private Const StatusCodeLeft102 As String = "102"
    ''' <summary>
    ''' ステータスコード：103：R/O作成中
    ''' </summary>
    Private Const StatusCodeLeft103 As String = "103"
    ''' <summary>
    ''' ステータスコード：104：新規お客様登録待ち
    ''' </summary>
    Private Const StatusCodeLeft104 As String = "104"
    ''' <summary>
    ''' ステータスコード：105：R/O作成待ち
    ''' </summary>
    Private Const StatusCodeLeft105 As String = "105"
    ''' <summary>
    ''' ステータスコード：106：R/O作成中
    ''' </summary>
    Private Const StatusCodeLeft106 As String = "106"
    ''' <summary>
    ''' ステータスコード：108：着工指示待ち/部品準備待ち
    ''' </summary>
    Private Const StatusCodeLeft108 As String = "108"
    ''' <summary>
    ''' ステータスコード：109：着工指示待ち/部品準備中
    ''' </summary>
    Private Const StatusCodeLeft109 As String = "109"
    ''' <summary>
    ''' ステータスコード：111：着工指示済み/部品準備待ち
    ''' </summary>
    Private Const StatusCodeLeft111 As String = "111"
    ''' <summary>
    ''' ステータスコード：112：着工指示済み/部品準備中
    ''' </summary>
    Private Const StatusCodeLeft112 As String = "112"
    ''' <summary>
    ''' ステータスコード：110：着工指示待ち/部品準備済み
    ''' </summary>
    Private Const StatusCodeLeft110 As String = "110"
    ''' <summary>
    ''' ステータスコード：107：着工指示待ち
    ''' </summary>
    Private Const StatusCodeLeft107 As String = "107"
    ''' <summary>
    ''' ステータスコード：113：作業開始待ち
    ''' </summary>
    Private Const StatusCodeLeft113 As String = "113"
    ''' <summary>
    ''' ステータスコード：115：中断中
    ''' </summary>
    Private Const StatusCodeLeft115 As String = "115"
    ''' <summary>
    ''' ステータスコード：114：作業中
    ''' </summary>
    Private Const StatusCodeLeft114 As String = "114"
    ''' <summary>
    ''' ステータスコード：116：完成検査待ち
    ''' </summary>
    Private Const StatusCodeLeft116 As String = "116"
    ''' <summary>
    ''' ステータスコード：117：納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft117 As String = "117"
    ''' <summary>
    ''' ステータスコード：118：洗車待ち/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft118 As String = "118"
    ''' <summary>
    ''' ステータスコード：119：洗車中/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft119 As String = "119"
    ''' <summary>
    ''' ステータスコード：120：洗車完了/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft120 As String = "120"
    ''' <summary>
    ''' ステータスコード：121：納車待ち
    ''' </summary>
    Private Const StatusCodeLeft121 As String = "121"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    ''' <summary>
    ''' ステータスコード：122：中断中
    ''' </summary>
    Private Const StatusCodeLeft122 As String = "122"
    ''' <summary>
    ''' ステータスコード：123：作業開始待ち
    ''' </summary>
    Private Const StatusCodeLeft123 As String = "123"
    ''' <summary>
    ''' ステータスコード：124：作業中
    ''' </summary>
    Private Const StatusCodeLeft124 As String = "124"
    ''' <summary>
    ''' ステータスコード：125：作業中
    ''' </summary>
    Private Const StatusCodeLeft125 As String = "125"
    ''' <summary>
    ''' ステータスコード：126：完成検査承認待ち
    ''' </summary>
    Private Const StatusCodeLeft126 As String = "126"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    ''' <summary>
    ''' ステータスコード：132：洗車待ち/CloseJob済み
    ''' </summary>
    Private Const StatusCodeLeft132 As String = "132"
    ''' <summary>
    ''' ステータスコード：133：洗車中/CloseJob済み
    ''' </summary>
    Private Const StatusCodeLeft133 As String = "133"
    ''' <summary>
    ''' ステータスコード：135：振当て待ち(新規お客様登録)
    ''' </summary>
    Private Const StatusCodeLeft135 As String = "135"
    ''' <summary>
    ''' ステータスコード：136：振当て待ち(RO作成)
    ''' </summary>
    Private Const StatusCodeLeft136 As String = "136"
    ''' <summary>
    ''' ステータスコード：137：振当て待ち(RO編集)
    ''' </summary>
    Private Const StatusCodeLeft137 As String = "137"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    ''' <summary>
    ''' ステータスコード：199：非表示
    ''' </summary>
    Private Const StatusCodeLeft199 As String = "199"
    ''' <summary>
    ''' ステータスコード：201：TC追加作業起票中
    ''' </summary>
    Private Const StatusCodeRight201 As String = "201"
    ''' <summary>
    ''' ステータスコード：202：CT承認待ち
    ''' </summary>
    Private Const StatusCodeRight202 As String = "202"
    ''' <summary>
    ''' ステータスコード：203：部品見積り待ち
    ''' </summary>
    Private Const StatusCodeRight203 As String = "203"
    ''' <summary>
    ''' ステータスコード：205：SA追加作業起票中
    ''' </summary>
    Private Const StatusCodeRight205 As String = "205"
    ''' <summary>
    ''' ステータスコード：206：SA見積り確定待ち
    ''' </summary>
    Private Const StatusCodeRight206 As String = "206"
    ''' <summary>
    ''' ステータスコード：207：お客様承認待ち
    ''' </summary>
    Private Const StatusCodeRight207 As String = "207"
    ''' <summary>
    ''' ステータスコード：208：非表示
    ''' </summary>
    Private Const StatusCodeRight208 As String = "208"
    ''' <summary>
    ''' ステータスコード：209：非表示
    ''' </summary>
    Private Const StatusCodeRight209 As String = "209"
    ''' <summary>
    ''' ステータスコード：210：非表示
    ''' </summary>
    Private Const StatusCodeRight210 As String = "210"
    ''' <summary>
    ''' ステータスコード：211：非表示
    ''' </summary>
    Private Const StatusCodeRight211 As String = "211"
    ''' <summary>
    ''' ステータスコード：299：非表示
    ''' </summary>
    Private Const StatusCodeRight299 As String = "299"
    ''' <summary>R/Oステータス</summary>
    Private Const ROFinSales As String = "3"
    Private Const ROFinMaintenance As String = "6"
    Private Const ROFinInspection As String = "7"
    ''' <summary>実績ステータス</summary>
    Private Const SMWaitWash As String = "40"
    Private Const SMWash As String = "41"
    Private Const SMCustody As String = "50"
    Private Const SMDelivery As String = "60"

    '2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） START
    ''' <summary>追加作業ステータス</summary>
    Private Const AddOrderStatusTC As String = "1"
    Private Const AddOrderStatusCT As String = "2"
    Private Const AddOrderStatusPS As String = "3"
    ''' <summary>起票者</summary>
    Private Const ReissueVouchersTC As String = "1"
    '2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） END

    Private Const PreparationMiddle As String = "1"
    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    Private Const WashFlagFalse As String = "1"
    Private Const WashFlagTrue As String = "0"
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '呼出ステータス
    '0：未呼出
    Private Const CALLSTATUS_NOTCALL As String = "0"
    '1：呼び出し中
    Private Const CALLSTATUS_CALLING As String = "1"
    '2：呼出完了
    Private Const CALLSTATUS_CALLED As String = "2"
    '3：受付完了
    Private Const CALLSTATUS_RECEIPTED As String = "3"
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    'Attributesプロパティ名
    Private Const AttributesPropertyName As String = "Name"
    Private Const AttributesPropertyLimittime As String = "limittime"
    Private Const AttributesPropertyOvertime1 As String = "overtime1"
    Private Const AttributesPropertyOvertime2 As String = "overtime2"
    Private Const AttributesPropertyProcgroup As String = "procgroup"

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

    Private Const DateFormateYYYYMMDDHHMM As String = "yyyyMMddHHmm"
    '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    Private Const CONVERTDATE_YMD As Integer = 9         'DateTimeFuncにて、"yyyyMMdd"形式にコンバートするための定数
    '2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

    ''' <summary>
    ''' カウンターエリアの表示レベル
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum CounterAreaLevel
        ''' <summary>通常</summary>
        NORMAL
        ''' <summary>警告</summary>
        WARN
        ''' <summary>異常</summary>
        ERR
    End Enum

    'アイコン表示スタイル
    Private Enum IconShowType
        RightIcnD
        RightIcnI
        RightIcnS
        WorkRightIcnD
        WorkRightIcnI
        WorkRightIcnS
        PopupRightIcnD
        PopupRightIcnI
        PopupRightIcnS
    End Enum

    ''' <summary>
    ''' 工程管理エリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ChipArea
        ''' <summary>未選択</summary>
        None = 0
        ''' <summary>受付</summary>
        Reception
        ''' <summary>追加承認</summary>
        Approval
        ''' <summary>納車準備</summary>
        Preparation
        ''' <summary>納車作業</summary>
        Delivery
        ''' <summary>作業中</summary>
        Work
        ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START
        ''' <summary>事前準備</summary>
        AdvancePreparations
        ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
        ''' <summary>振当待ち</summary>
        Assignment
        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    End Enum
    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
#Region "文言ID"
    ''' <summary>
    ''' アイコンの文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
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
    End Enum
#End Region
    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>タイムアウト</summary>
        id901 = 901
        ''' <summary>その他</summary>
        id902 = 902
        ''' <summary>担当SA外</summary>
        id903 = 903
        ''' <summary>未取引客</summary>
        id904 = 904
        ''' <summary>自社客登録済み</summary>
        id905 = 905
        ''' <summary>自社客でない</summary>
        id906 = 906
        ''' <summary>SA振当て済み</summary>
        id907 = 907
        ''' <summary>受付登録失敗</summary>
        id908 = 908

        '2012/10/22 TMEJ 河原  問連「GTMC121015030」対応 START
        ''' <summary>R/O作成失敗</summary>
        id909 = 909
        ''' <summary>登録No.</summary>
        id84 = 84
        ''' <summary>VIN</summary>
        id85 = 85
        ''' <summary>車両型式</summary>
        id86 = 86
        ''' <summary>顧客名</summary>
        id87 = 87
        ''' <summary>電話番号</summary>
        id88 = 88
        ''' <summary>入庫日時</summary>
        id90 = 90

        '2012/10/22 TMEJ 河原  問連「GTMC121015030」対応 END

        '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 STRAT
        ''' <summary>完成検査承認ボタンエラー</summary>
        id910 = 910
        '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END
        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        ''' <summary>呼出場所未入力エラー</summary>
        id911 = 911
        ''' <summary>呼出場所禁止文字エラー</summary>
        id912 = 912
        ''' <summary>呼び出し処理失敗</summary>
        id913 = 913
        ''' <summary>呼び出しキャンセル処理失敗</summary>
        id914 = 914
        ''' <summary>発券番号未入力エラー</summary>
        id915 = 915
        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''' <summary>顧客検索予期せぬエラー</summary>
        id916 = 916

        ''' <summary>予期せぬエラー</summary>
        id917 = 917

        ''' <summary>すでに清算処理が行われているため・・・</summary>
        id918 = 918

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    End Enum

    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    ''' <summary>
    ''' 受付フラグ：受付エリア以外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitTypeOff As String = "0"
    ''' <summary>
    ''' 受付フラグ：受付エリア白チップ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitTypeOn As String = "1"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    ' 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 STRAT
    ''' <summary>
    '''完成検査ボタン表示フラグ(OFF)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InspectionButtonTypeOff As String = "0"

    ''' <summary>
    ''' 完成検査ボタン表示フラグ(ON)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InspectionButtonTypeOn As String = "1"
    ' 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"

    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 検索エリアフラグ("0"：車両登録番号) 
    ''' </summary>
    Private Const SearchAreaRegNo As String = "0"

    ''' <summary>
    ''' 検索エリアフラグ("1"：VIN) 
    ''' </summary>
    Private Const SearchAreaVin As String = "1"

    ''' <summary>
    ''' 検索エリアフラグ("2"：顧客名) 
    ''' </summary>
    Private Const SearchAreaName As String = "2"

    ''' <summary>
    ''' 検索エリアフラグ("3"：基幹予約ID) 
    ''' </summary>
    Private Const SearchAreaAppoint As String = "3"

    ''' <summary>
    ''' 表示アイコンフラグ("1"：ON 表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconFlagOn As String = "1"

    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' 表示アイコンフラグ2(""：ON 表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconFlagOn2 As String = "2"
    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
    ''' <summary>
    ''' チップの色を白(仕掛中ではない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentsBoder As String = "ColumnContentsBoder"

    ''' <summary>
    ''' チップの色を青(仕掛中)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentsBoderAqua As String = "ColumnContentsBoder ColumnBoxAqua"

    ''' <summary>
    ''' サービスステータス(11：預かり中)
    ''' </summary>
    Private Const ServiceStatusDropOff As String = "11"

    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    ''' <summary>
    ''' 追加作業承認待ちフラグ("1"：有り)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_APPROVAL_STATUS_ON As String = "1"

    ''' <summary>
    ''' ROステータス（"50"：着工指示待ち）
    ''' </summary>
    Private Const StatusInstructionsWait As String = "50"

    ''' <summary>
    ''' ROステータス（"60"：作業中）
    ''' </summary>
    Private Const StatusWork As String = "60"

    ''' <summary>
    ''' ROステータス（"80"：納車準備）
    ''' </summary>
    Private Const StatusDeliveryWait As String = "80"

    ''' <summary>
    ''' ROステータス（"85"：納車作業）
    ''' </summary>
    Private Const StatusDeliveryWork As String = "85"

    ''' <summary>
    '''洗車必要フラグ("1"：洗車有り)
    ''' </summary>
    Private Const WashNeedFlagTrue As String = "1"

    ''' <summary>
    ''' 振当待ちエリアリフレッシュフラグ
    ''' GKよりPush時に振当待ちエリアのみリフレッシュするためのフラグ
    ''' </summary>
    Private Const AssignmentRefreshFlag As String = "1"

    ''' <summary>
    ''' 受付モニター使用フラグ取得用検索PARAMNAME("WAIT_RECEPTION_TYPE")
    ''' </summary>
    Private Const WaitReceptionType As String = "WAIT_RECEPTION_TYPE"

    ''' <summary>
    ''' 受付モニター使用フラグ("1"：使用する)
    ''' </summary>
    Private Const UseReceptionFlag As String = "1"

    '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 START
    ''' <summary>
    ''' 追加作業起票蓋締めフラグ取得用検索PARAMNAME("ADD_WORK_CLOSE_TYPE")
    ''' </summary>
    Private Const AddWorkCloseTypeParamName As String = "ADD_WORK_CLOSE_TYPE"

    ''' <summary>
    ''' 追加作業起票蓋締めフラグ("1"：蓋締めする)
    ''' </summary>
    Private Const AddWorkCloseTypeOn As String = "1"
    '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 END

    ''' <summary>
    ''' 新規顧客登録使用フラグ("0"：使用する)
    ''' </summary>
    Private Const UseNewCustomerFlagOff As String = "0"

    ''' <summary>
    ''' 新規顧客登録使用フラグ("1"：使用する)
    ''' </summary>
    Private Const UseNewCustomerFlagOn As String = "1"

    ''' <summary>
    ''' 追加作業起票者("1"：TC)
    ''' </summary>
    Private Const TechnicianIssuance As String = "1"

    ''' <summary>
    ''' 標準ボタン・サブボタン表示フラグ("0"：非表示)
    ''' </summary>
    Private Const SubMenuButtonOff As String = "0"

    ''' <summary>
    ''' 標準ボタン・サブボタン表示フラグ("1"：表示)
    ''' </summary>
    Private Const SubMenuButtonOn As String = "1"

    ''' <summary>
    ''' 標準ボタンステータスAttributes名
    ''' </summary>
    Private Const SubMenuButtonStatusCalss As String = "SubMenuButtonStatus"

    ''' <summary>
    ''' 標準ボタン左側フラグ
    ''' </summary>
    Private Const DetailbottomLeft As String = "1"

    ''' <summary>
    ''' 標準ボタン右側フラグ
    ''' </summary>
    Private Const DetailbottomRight As String = "2"

    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
    ''' <summary>
    ''' 標準ボタン3フラグ
    ''' </summary>
    Private Const Detailbottom3 As String = "3"
    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

    ''' <summary>
    ''' 編集モードフラグ("0"；編集) 
    ''' </summary>
    Private Const EditMode As String = "0"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    ''' <summary>
    ''' プレビューフラグ("0"；プレビュー) 
    ''' </summary>
    Private Const PreviewFlag As String = "0"

    ''' <summary>親のRO作業連番編集(0) 
    ''' </summary>
    Private Const ParentJobSeq As String = "0"

    ''' <summary>
    ''' 基幹画面連携用フレームID("SC3010501")
    ''' </summary>
    Private Const APPLICATIONID_FRAMEID As String = "SC3010501"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param1")
    ''' </summary>
    Private Const SessionParam01 As String = "Session.Param1"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param2")
    ''' </summary>
    Private Const SessionParam02 As String = "Session.Param2"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param3")
    ''' </summary>
    Private Const SessionParam03 As String = "Session.Param3"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param4")
    ''' </summary>
    Private Const SessionParam04 As String = "Session.Param4"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param5")
    ''' </summary>
    Private Const SessionParam05 As String = "Session.Param5"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param6")
    ''' </summary>
    Private Const SessionParam06 As String = "Session.Param6"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param7")
    ''' </summary>
    Private Const SessionParam07 As String = "Session.Param7"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param8")
    ''' </summary>
    Private Const SessionParam08 As String = "Session.Param8"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param9")
    ''' </summary>
    Private Const SessionParam09 As String = "Session.Param9"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param10")
    ''' </summary>
    Private Const SessionParam10 As String = "Session.Param10"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param11")
    ''' </summary>
    Private Const SessionParam11 As String = "Session.Param11"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param12")
    ''' </summary>
    Private Const SessionParam12 As String = "Session.Param12"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.DISP_NUM")
    ''' </summary>
    Private Const SessionDispNum As String = "Session.DISP_NUM"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.DMS_CST_ID")
    ''' </summary>
    Private Const SessionDMSID As String = "SessionKey.DMS_CST_ID"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.VIN")
    ''' </summary>
    Private Const SessionVIN As String = "SessionKey.VIN"

    ''' <summary>
    ''' セッション名("DealerCode")
    ''' </summary>
    Private Const SessionDealerCode As String = "DealerCode"

    ''' <summary>
    ''' セッション名("BranchCode")
    ''' </summary>
    Private Const SessionBranchCode As String = "BranchCode"

    ''' <summary>
    ''' セッション名("LoginUserID")
    ''' </summary>
    Private Const SessionLoginUserID As String = "LoginUserID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"

    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"

    ''' <summary>
    ''' セッション名("R_O")
    ''' </summary>
    Private Const SessionRO As String = "R_O"

    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSEQNO As String = "SEQ_NO"

    ''' <summary>
    ''' セッション名("VIN_NO")
    ''' </summary>
    Private Const SessionVINNO As String = "VIN_NO"

    ''' <summary>
    ''' セッション名("ViewMode")
    ''' </summary>
    Private Const SessionViewMode As String = "ViewMode"

    ''' <summary>
    ''' R/O作成・R/O編集画面(DISP_NUM:"1")
    ''' </summary>
    Private Const APPLICATIONID_ORDERNEW As String = "1"

    ''' <summary>
    ''' チェックシート印刷画面("SC3180202")
    ''' </summary>
    Private Const APPLICATIONID_CHECKSHEET As String = "SC3180202"

    ''' <summary>
    ''' R/O参照画面(DISP_NUM:"13")
    ''' </summary>
    Private Const APPLICATIONID_ORDEROUT As String = "13"

    ''' <summary>
    ''' 顧客詳細画面("SC3080225")
    ''' </summary>
    Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080225"

    ''' <summary>
    ''' R/O一覧画面(DISP_NUM:"14")
    ''' </summary>
    Private Const APPLICATIONID_ORDERLIST As String = "14"

    ''' <summary>
    ''' 商品訴求コンテンツ画面("SC3250101")
    ''' </summary>
    Private Const APPLICATIONID_PRODUCTSAPPEALCONTENT As String = "SC3250101"

    ''' <summary>
    ''' キャンペーン画面(DISP_NUM:"15")
    ''' </summary>
    Private Const APPLICATIONID_CAMPAIGN As String = "15"

    ''' <summary>
    ''' Otherjob画面(DISP_NUM:"9001")
    ''' </summary>
    Private Const APPLICATIONID_OTHERJOB As String = "9001"

    ''' <summary>
    ''' PushFlag(SVR:"0":来店管理にPush無し  SA:"0":振当エリアのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushFlag0 As String = "0"

    ''' <summary>
    ''' PushFlag(SVR:"1":来店管理にPush有り  SA:"1":全体のPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushFlag1 As String = "1"

    ''' <summary>
    ''' イベントキーID
    ''' </summary>
    Private Enum EventKeyId

        ''' <summary>
        ''' SA振当
        ''' </summary>
        SAAssig = 100

        ''' <summary>
        ''' SA解除
        ''' </summary>
        SAUndo = 200

        ''' <summary>
        ''' 退店
        ''' </summary>
        StoreOut = 300

    End Enum

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定 START

    ''' <summary>
    ''' セッション名("SEQ_NO")の追加作業起票パラメーター
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionSEQNONew As String = "New"

    '2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' 実績フラグ(実績チップを含む全てのチップ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultsFlgOn As Long = 1

    ''' <summary>
    ''' キャンセルフラグ(キャンセルチップを含まない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CancelFlgOff As Long = 0
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
    ''' <summary>
    ''' 削除フラグ(未削除)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNone As String = "0"
    '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

#End Region

#Region "変数定義"

    ' 現在時刻
    'カウンター対応
    Protected nowDateTime As DateTime

    '' 受付_予約無し_警告表示標準時間（分）
    'Private mlngReceptNoresWarningLt As Long = 0    ' 予約なし
    '' 受付_予約無し_異常表示標準時間（分）
    'Private mlngReceptNoresAbnormalLt As Long = 0   ' 予約なし

    '' 受付_予約有り_警告表示標準時間（分）
    'Private mlngReceptResWarningLt As Long = 0      ' 予約あり
    '' 受付_予約有り_異常表示標準時間（分）
    'Private mlngReceptResAbnormalLt As Long = 0     ' 予約あり

    '' 追加承認_予約無し_警告表示標準時間（分）
    'Private mlngAddworkNoresWarningLt As Long = 0   ' 予約なし
    '' 追加承認_予約無し_異常表示標準時間（分）
    'Private mlngAddworkNoresAbnormalLt As Long = 0  ' 予約なし

    '' 追加承認_予約有り_警告表示標準時間（分）
    'Private mlngAddworkResWarningLt As Long = 0     ' 予約あり
    '' 追加承認_予約有り_異常表示標準時間（分）
    'Private mlngAddworkResAbnormallt As Long = 0    ' 予約あり

    ' 納車準備_異常表示標準時間（分）
    Private deliverypreAbnormalLt As Long = 0
    ' 納車作業_異常表示時標準間（分）
    'Private deliverywrAbnormalLt As Long = 0

    '固定文字列
    Private wordFixedString As String = ""

    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    'アイコン文字
    Private strRightIcnM As String = ""
    Private strRightIcnB As String = ""
    Private strRightIcnE As String = ""
    Private strRightIcnT As String = ""
    Private strRightIcnP As String = ""
    Private strRightIcnL As String = ""
    ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
#End Region

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STAT
    '(GL版となりモジュール一新のため一度全てコメントアウト)


    '#Region " イベント処理 "
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' ページロード時の処理です。
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    '    ''' </history>
    '    '''-----------------------------------------------------------------------
    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        '2012/04/09 KN 森下 【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応 START
    '        'Try
    '        '    If Not Me.IsPostBack Then
    '        '        'カウンターエリアで使用する経過時間設定
    '        '        Me.SetCounterTime()
    '        '        ' チップ情報取得
    '        '        Me.InitVisitChip()
    '        '    End If

    '        '    'フッターの制御
    '        '    InitFooterEvent()

    '        'Catch ex As OracleExceptionEx When ex.Number = 1013
    '        '    'タイムアウトエラーの場合は、メッセージを表示する
    '        '    ShowMessageBox(MsgID.id901)
    '        'Finally
    '        '    ' チップタイマー用に現在時刻取得
    '        '    Dim staffInfo As StaffContext = StaffContext.Current
    '        '    Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
    '        'End Try

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        'SYSTEMENVから事前準備仕様フラグを取得し、表示設定をする

    '        Me.AdvancePreparationsButton.Visible = False


    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        'SYSTEMENVから受付モニター使用フラグを取得する

    '        If Me.IsPostBack Then

    '            Me.UseReception.Value = "1"

    '        End If

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    '        ' チップタイマー用に現在時刻取得
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

    '        'フッターの制御
    '        InitFooterEvent()

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '        'ヘッダーボタン表示
    '        'Me.ChipChanges.Value = WebWordUtility.GetWord(APPLICATIONID, 61)
    '        'Me.SearchCancel.Value = WebWordUtility.GetWord(APPLICATIONID, 60)
    '        Me.ChipChanges.Text = WebWordUtility.GetWord(APPLICATIONID, 61)
    '        Me.SearchCancel.Text = WebWordUtility.GetWord(APPLICATIONID, 60)

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        'Me.SearchBottomButton.Value = WebWordUtility.GetWord(APPLICATIONID, 70)

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '        ''2012/04/09 KN 森下 【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応 END

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
    '        '担当SAリスト設定
    '        'Me.SearchBottomButton.Value = WebWordUtility.GetWord(APPLICATIONID, 70)
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '        '担当SA選択欄設定
    '        If Not Me.IsPostBack Then
    '            Using user As New IC3810601BusinessLogic
    '                'Dim user As New IC3810601BusinessLogic
    '                Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable
    '                Dim useropcode As New List(Of Long)
    '                useropcode.Add(9)
    '                userdt = user.GetAcknowledgeStaffList(staffInfo.DlrCD, staffInfo.BrnCD, useropcode)
    '                Me.SASelector.Items.Clear()
    '                Me.SASelector.Items.Add(New ListItem("", ""))
    '                For i = 0 To userdt.Rows.Count Step 1
    '                    If userdt.Rows.Count > i AndAlso userdt(i).ACCOUNT IsNot Nothing Then
    '                        Me.SASelector.Items.Add(New ListItem(userdt(i).USERNAME, userdt(i).ACCOUNT))
    '                        If userdt(i).ACCOUNT = staffInfo.Account Then
    '                            Me.SASelector.SelectedIndex = i + 1
    '                        End If
    '                    End If
    '                Next
    '            End Using
    '        End If
    '        SASelector.Attributes.Add("onchange", "SAchange()")
    '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' チップ詳細フッタ部の標準ボタンの押下
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' チップ詳細のフッタ部の標準ボタンを押下したときの処理を実施する。
    '    ''' </remarks>
    '    ''' <history>
    '    ''' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
    '    ''' 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Protected Sub DetailDefaultButton_Click(sender As Object,
    '                                            e As System.EventArgs) Handles DetailNextScreenCommonButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim actionFlg As Boolean = True
    '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '        'Dim bis As New SC3140103BusinessLogic
    '        Using bis As New SC3140103BusinessLogic
    '            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
    '            Try
    '                Dim rowReserve As SC3140103AdvancePreparationsReserveInfoRow = Nothing
    '                Dim rowVisitData As SC3140103VisitRow = Nothing
    '                Dim rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow = Nothing
    '                Dim wkVisitSeq As Nullable(Of Long) = Nothing

    '                ' 選択チップ情報の取得
    '                Dim rezId As Long = Me.SetNullToLong(Me.DetailsRezId.Value, -1)
    '                Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '                Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
    '                '受付エリアチップの場合、呼出ステータスを更新
    '                If detailArea = CType(ChipArea.Reception, Long) Then
    '                    Dim resultCode As Long = Me.CallCompleteOperation()
    '                    If resultCode <> 0 Then
    '                        '失敗の場合、処理を中断する
    '                        actionFlg = False
    '                    End If
    '                End If
    '                If actionFlg Then
    '                    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
    '                    If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
    '                        '事前準備エリア
    '                        Dim dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable

    '                        ' 事前準備チップ予約情報の取得
    '                        dtReserve = bis.GetAdvancePreparationsReserveInfo(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

    '                        ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 START
    '                        Dim dtRow As SC3140103AdvancePreparationsReserveInfoRow() = _
    '                        CType(dtReserve.Select("", " WORKTIME ASC"), SC3140103AdvancePreparationsReserveInfoRow())

    '                        If dtReserve IsNot Nothing AndAlso dtReserve.Rows.Count > 0 Then
    '                            rowReserve = CType(dtRow(0), SC3140103AdvancePreparationsReserveInfoRow)

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                            'TACT側の顧客情報をチェックして予約情報に設定する
    '                            rowReserve = Me.SetCustomerInfo(rowReserve)

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                            ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 END
    '                        Else
    '                            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '                            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2} REZID={3}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , "DetailDefaultButton_Click  detailArea::dtReserve=nothing or Count=0" _
    '                            , rezId))
    '                            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '                            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                            'タイマークリア
    '                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                            Me.ShowMessageBox(MsgID.id902)
    '                            Exit Try
    '                        End If

    '                        ' 事前準備チップサービス来店管理情報の取得
    '                        Dim dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable

    '                        dtVisit = bis.GetAdvancePreparationsVisitManager(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

    '                        If dtVisit IsNot Nothing AndAlso dtVisit.Rows.Count > 0 Then
    '                            rowAdvanceVisit = _
    '                                CType(dtVisit.Rows(0), SC3140103AdvancePreparationsServiceVisitManagementRow)

    '                            wkVisitSeq = rowAdvanceVisit.VISITSEQ
    '                        End If


    '                    Else
    '                        ' 事前準備エリア以外
    '                        Dim dt As SC3140103DataSet.SC3140103VisitDataTable = _
    '                            bis.GetVisitChipDetailForNextScreen(visitSeq, Me.DetailsArea.Value)
    '                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '                            rowVisitData = CType(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)
    '                        Else
    '                            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '                            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                        , "{0}.{1} {2} detailArea={3} VISITSEQ={4} displayDiv={5}" _
    '                                        , Me.GetType.ToString _
    '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                        , "DetailDefaultButton_Click  ELSE NOT detailArea::dt=nothing or dt=0" _
    '                                        , detailArea, visitSeq, Me.DetailsArea.Value))
    '                            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '                            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                            'タイマークリア
    '                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                            Me.ShowMessageBox(MsgID.id902)
    '                            Exit Try
    '                        End If
    '                    End If

    '                    '遷移処理
    '                    Me.NextScreenVisitChipDetailButton(Me.DetailClickButtonName.Value,
    '                                                       rowReserve,
    '                                                       rowVisitData,
    '                                                       rowAdvanceVisit,
    '                                                       wkVisitSeq)
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                'タイムアウトエラーの場合は、メッセージを表示する
    '                ShowMessageBox(MsgID.id901)
    '            Finally
    '                ' チップタイマー用に現在時刻取得
    '                Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '                'If bis IsNot Nothing Then
    '                'bis.Dispose()
    '                ' bis = Nothing
    '                ' End If
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
    '            End Try
    '            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '        End Using
    '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        If actionFlg Then
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        Else
    '            'タイマークリア(呼出ステータス更新失敗用)
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '        End If
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)
    '    End Sub


    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' チップ詳細ボタン(共通画面遷移時処理)
    '    ' ''' 2012/02/29 KN 森下【SERVICE_1】START
    '    ' ''' </summary>
    '    ' ''' <param name="sender">イベント発生元</param>
    '    ' ''' <param name="e">イベント引数</param>
    '    ' ''' <remarks>
    '    ' ''' チップ詳細画面フッタ部の右ボタン押下時に当イベントが発生します。
    '    ' ''' </remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Protected Sub DetailNextScreenCommonButton_Click(sender As Object, e As System.EventArgs) Handles DetailNextScreenCommonButton.Click
    '    '    '開始ログ出力
    '    '    Dim logStart As New StringBuilder
    '    '    With logStart
    '    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '    '        .Append(" Start")
    '    '    End With
    '    '    Logger.Info(logStart.ToString)

    '    '    Try
    '    '        '遷移処理
    '    '        Me.NextScreenVisitChipDetailButton(Me.DetailClickButtonName.Value)

    '    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '    '        'タイムアウトエラーの場合は、メッセージを表示する
    '    '        ShowMessageBox(MsgID.id901)
    '    '    Finally
    '    '        ' チップタイマー用に現在時刻取得
    '    '        Dim staffInfo As StaffContext = StaffContext.Current
    '    '        Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
    '    '    End Try

    '    '    '終了ログ出力
    '    '    Dim logEnd As New StringBuilder
    '    '    With logEnd
    '    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '    '        .Append(" End")
    '    '    End With
    '    '    Logger.Info(logEnd.ToString)
    '    'End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' チップの詳細ポップアップウィンドウに表示する情報を取得する為のダミーボタンクリック
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' チップ詳細画面ポップアップ表示のために隠しボタンである当ボタンを
    '    ''' クライアント側にてクリックすることでイベントが発生します。
    '    ''' </remarks>
    '    ''' <histiry>
    '    ''' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
    '    ''' 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    '    ''' </histiry>
    '    '''-----------------------------------------------------------------------
    '    Protected Sub DetailPopupButton_Click(sender As Object,
    '                                          e As System.EventArgs) Handles DetailPopupButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        Dim visitNo As String = Me.DetailsVisitNo.Value
    '        Dim reserveId As String = Me.DetailsRezId.Value
    '        Dim selectChipArea As Long = SetNullToLong(Me.DetailsArea.Value)
    '        Dim chipDetail As ChipDetail = Nothing
    '        'Try
    '        ' 現在時刻取得
    '        'Dim staffInfo As StaffContext = StaffContext.Current
    '        'Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)

    '        ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START
    '        ' 選択したチップのエリアを取得
    '        'Dim selectChipArea As Long = SetNullToLong(Me.DetailsArea.Value)
    '        ' 事前準備エリアかそれ以外かでチップ詳細情報の取得方法分岐する
    '        'If selectChipArea = ChipArea.AdvancePreparations Then
    '        ' チップ詳細情報表示
    '        'Me.InitAdvancePreparationsChipDetail()
    '        'Me.ContentUpdatePanelDetail.Update()
    '        'Else
    '        ' チップ詳細情報表示
    '        'Me.InitVisitChipDetail()
    '        'Me.ContentUpdatePanelDetail.Update()
    '        'End If
    '        ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END
    '        'Catch ex As OracleExceptionEx When ex.Number = 1013
    '        'タイムアウトエラーの場合は、メッセージを表示する
    '        'ShowMessageBox(MsgID.id901)
    '        '2012/04/09 KN 森下 【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応 START
    '        '    ' 2012/02/22 KN 森下【SERVICE_1】START
    '        'Finally
    '        '    ' 詳細ポップアップウィンドウの読み込み中アイコン非表示
    '        '    Dim ctrlDiv As HtmlContainerControl _
    '        '        = CType(Me.ContentUpdatePanelDetail.FindControl("IconLoadingPopup"), HtmlContainerControl)
    '        '    ctrlDiv.Attributes("style") = "visibility: hidden"
    '        '    ' 2012/02/22 KN 森下【SERVICE_1】END
    '        '2012/04/09 KN 森下 【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応 END
    '        'End Try

    '        Using SMBCommon As New SMBCommonClassBusinessLogic
    '            Try
    '                If ChipArea.AdvancePreparations = selectChipArea Then
    '                    chipDetail = SMBCommon.GetChipDetailReserve(staffInfo.DlrCD, _
    '                                                                staffInfo.BrnCD, _
    '                                                                CType(reserveId, Long))
    '                Else
    '                    '来店チップ詳細情報の取得
    '                    chipDetail = SMBCommon.GetChipDetailVisit(staffInfo.DlrCD, _
    '                                                              staffInfo.BrnCD, _
    '                                                              CType(visitNo, Long))
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                'ORACLEのタイムアウトのみ処理
    '                Me.ShowMessageBox(MsgID.id901)
    '                Logger.Info("DetailPopupButton_Click END")
    '                Exit Sub
    '            End Try
    '        End Using

    '        If chipDetail Is Nothing Then
    '            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} {2} selectChipArea={3} REZID={4} visitNo={5}" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                        , "DetailPopupButton_Click  chipDetail=nothing" _
    '                        , selectChipArea, reserveId, visitNo))
    '            ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            Me.ShowMessageBox(MsgID.id902)
    '            Logger.Info("DetailPopupButton_Click END")
    '            Exit Sub
    '        End If

    '        Dim wordNullDateTime As String = WebWordUtility.GetWord(APPLICATIONID, 42) '「--:--」

    '        'ステータス
    '        Me.IconStatsLabel.Text = chipDetail.Status
    '        '納車予定時刻
    '        Me.DeliveryTimeLabel.Text = Me.SetNullToString(chipDetail.DeliveryPlanDate, wordNullDateTime)
    '        '納車予定時刻変更回数
    '        Me.ChangeCountLabel.Text = _
    '        WebWordUtility.GetWord(APPLICATIONID, 39) _
    '            .Replace("%1", CType(chipDetail.DeliveryPlanDateUpdateCount, String))
    '        Me.HiddenDeliveryPlanUpdateCount.Value = CType(chipDetail.DeliveryPlanDateUpdateCount, String)

    '        '納車見込時刻
    '        Me.DeliveryEstimateLabel.Text = _
    '        Me.SetNullToString(chipDetail.DeliveryHopeDate, wordNullDateTime)

    '        '車両登録No.
    '        Me.DetailsRegistrationNumber.Text = chipDetail.VehicleRegNo

    '        '予約マーク
    '        If DETAILS_MARK_ACTIVE.Equals(chipDetail.WalkIn) Then
    '            Me.DetailsRightIconD.Visible = True
    '        Else
    '            Me.DetailsRightIconD.Visible = False
    '        End If

    '        'JDP調査対象客マーク
    '        If DETAILS_MARK_ACTIVE.Equals(chipDetail.JdpType) Then
    '            Me.DetailsRightIconI.Visible = True
    '        Else
    '            Me.DetailsRightIconI.Visible = False
    '        End If

    '        'SSCマーク
    '        If DETAILS_MARK_ACTIVE.Equals(chipDetail.SscType) Then
    '            Me.DetailsRightIconS.Visible = True
    '        Else
    '            Me.DetailsRightIconS.Visible = False
    '        End If
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '        '来店者呼び出しエリア設定
    '        Me.VisitAreaSet(selectChipArea, chipDetail)
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '        '車種名
    '        Me.DetailsCarModel.Text = chipDetail.VehicleName
    '        'グレード
    '        Me.DetailsModel.Text = chipDetail.Grade
    '        '顧客名
    '        Me.DetailsCustomerName.Text = chipDetail.CustomerName
    '        '電話番号
    '        Me.DetailsPhoneNumber.Text = chipDetail.TelNo
    '        '携帯電話番号
    '        Me.DetailsMobileNumber.Text = chipDetail.Mobile
    '        '整備内容
    '        Me.DetailsServiceContents.Text = chipDetail.MerchandiseName
    '        '待ち方
    '        If REZ_RECEPTION_WAITING.Equals(chipDetail.ReserveReception) Then
    '            Me.DetailsWaitPlan.Text = WebWordUtility.GetWord(APPLICATIONID, 44) '店内
    '        ElseIf REZ_RECEPTION_DROP_OFF.Equals(chipDetail.ReserveReception) Then
    '            Me.DetailsWaitPlan.Text = WebWordUtility.GetWord(APPLICATIONID, 45) '店外
    '        Else
    '            Me.DetailsWaitPlan.Text = ""
    '        End If


    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'TcatのモデルコードをI-crop側が未取引客の場合に必要のため格納
    '        Me.HiddenVehicleModel.Value = chipDetail.Model

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） START
    '        '追加作業エリアのチップ詳細で起票者がTCの場合は起票者ストールを表示する
    '        If ChipArea.Approval = selectChipArea AndAlso "1".Equals(chipDetail.ReissueVouchers) Then
    '            Me.DrawerTable.Style("display") = ""
    '            Me.DetailsDrawer.Text = chipDetail.AddAccountName
    '        Else
    '            Me.DrawerTable.Style("display") = "none"
    '            Me.DetailsDrawer.Text = ""
    '        End If
    '        '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） END

    '        '中断理由
    '        If chipDetail.StopReasonList IsNot Nothing AndAlso 0 < chipDetail.StopReasonList.Count Then
    '            Using dtInterruptionInfo As New SC3140103InterruptionInfoDataTable
    '                For Each item As StopReason In chipDetail.StopReasonList

    '                    Dim rowInterruptionInfo As SC3140103InterruptionInfoRow = _
    '                        DirectCast(dtInterruptionInfo.NewRow(), SC3140103InterruptionInfoRow)

    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                    ' 中断理由
    '                    'rowInterruptionInfo.InterruptionCause = Me.GetResultStatusWord(item.ResultStatus)
    '                    rowInterruptionInfo.InterruptionCause = item.ResultStatus

    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                    ' 中断注釈
    '                    rowInterruptionInfo.InterruptionDetails = item.StopMemo

    '                    dtInterruptionInfo.AddSC3140103InterruptionInfoRow(rowInterruptionInfo)
    '                Next

    '                Me.InterruptionCauseRepeater.DataSource = dtInterruptionInfo
    '                Me.InterruptionCauseRepeater.DataBind()
    '            End Using
    '        Else
    '            Me.InterruptionCauseRepeater.DataSource = Nothing
    '            Me.InterruptionCauseRepeater.DataBind()
    '        End If

    '        '納車時刻変更履歴
    '        If chipDetail.DeliveryChgList IsNot Nothing AndAlso 0 < chipDetail.DeliveryChgList.Count Then

    '            Using dtDeliChange As New SC3140103DeliveryTimeChangeLogInfoDataTable
    '                Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

    '                For Each item As DeliveryChg In chipDetail.DeliveryChgList

    '                    Dim rowDeliChange As SC3140103DeliveryTimeChangeLogInfoRow = _
    '                        DirectCast(dtDeliChange.NewRow(), SC3140103DeliveryTimeChangeLogInfoRow)

    '                    '変更前納車予定時刻
    '                    rowDeliChange.ChangeFromTime = Me.SetDateTimeToStringDetail(item.OldDeliveryHopeDate, nowDate)
    '                    '変更後納車予定時刻
    '                    rowDeliChange.ChangeToTime = Me.SetDateTimeToStringDetail(item.NewDeliveryHopeDate, nowDate)
    '                    '変更日時
    '                    rowDeliChange.UpdateTime = Me.SetDateTimeToStringDetail(item.ChangeDate, nowDate)
    '                    '変更理由
    '                    rowDeliChange.UpdatePretext = item.ChangeReason

    '                    dtDeliChange.AddSC3140103DeliveryTimeChangeLogInfoRow(rowDeliChange)
    '                Next

    '                Me.ChangeTimeRepeater.DataSource = dtDeliChange
    '                Me.ChangeTimeRepeater.DataBind()
    '            End Using
    '        Else
    '            Me.ChangeTimeRepeater.DataSource = Nothing
    '            Me.ChangeTimeRepeater.DataBind()
    '        End If

    '        'サブボタンの非活性の設定
    '        If ChipArea.Approval = selectChipArea Then
    '            Me.SetSubMenuButtonApproval(chipDetail.StatusRight)
    '        Else
    '            Me.SetSubMenuButton(chipDetail.StatusLeft)
    '        End If

    '        '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 STRAT
    '        '作業中エリアの場合
    '        If ChipArea.Work = selectChipArea Then
    '            '完成検査承認ボタン活性設定
    '            Me.SetInspectionButton(staffInfo)
    '        End If
    '        '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END


    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        '' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
    '        ''受付の削除ボタンの文言取得(すでにある場合はとってこない)
    '        'If ChipArea.Reception = selectChipArea AndAlso String.IsNullOrEmpty(Me.DetailButtonDelete.Text) Then
    '        '    Me.DetailButtonDelete.Text = WebWordUtility.GetWord(APPLICATIONID, 82)
    '        'End If
    '        '' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

    '        '受付エリアと受付待ちエリアの場合、Deleteボタン表示
    '        If ChipArea.Reception = selectChipArea Then
    '            '受付エリアの場合

    '            'ボタン文言の設定
    '            Me.ButtonDeleteWord01.Visible = True
    '            Me.ButtonDeleteWord02.Visible = False


    '        ElseIf ChipArea.Assignment = selectChipArea Then
    '            '受付待ちエリアの場合

    '            'ボタン文言の設定
    '            Me.ButtonDeleteWord01.Visible = False
    '            Me.ButtonDeleteWord02.Visible = True

    '        End If

    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    '        Me.ContentUpdatePanelDetail.Update()
    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 通知リフレッシュ処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 通知イベントが発生した際に、ダミーボタンである当ボタンを
    '    ''' クライアントにてクリックすることで当イベントが発生します。
    '    ''' </remarks>
    '    '''-----------------------------------------------------------------------
    '    Protected Sub MainPolling_Click(sender As Object, _
    '                                    e As System.EventArgs) Handles MainPolling.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Try
    '            'カウンターエリアで使用する経過時間設定
    '            'Me.SetCounterTime()
    '            ' チップ情報表示
    '            Me.InitVisitChip()
    '            Me.ContentUpdatePanel.Update()

    '            ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START
    '            Me.FotterUpdatePanel.Update()
    '            ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END

    '        Catch oracleEx As OracleExceptionEx When oracleEx.Number = 1013
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            Me.ShowMessageBox(MsgID.id901)
    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '        Catch timeOutEx As TimeoutException When TypeOf timeOutEx.InnerException Is OracleExceptionEx
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            Me.ShowMessageBox(MsgID.id901)
    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Finally
    '            ' チップタイマー用に現在時刻取得
    '            Dim staffInfo As StaffContext = StaffContext.Current
    '            Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
    '        End Try

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START
    '    ''' -----------------------------------------------------------------
    '    ''' <summary>
    '    ''' 事前準備ポップアップに表示するチップの取得用ダミーボタンクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' フッターの事前予約ボタンを押下した際に隠しボタンである当ボタンを
    '    ''' クライアント側でクリックすることでイベントが発生します。
    '    ''' </remarks>
    '    ''' <history>
    '    ''' 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    Protected Sub AdvancePreparations_Click(sender As Object,
    '                                            e As System.EventArgs) Handles AdvancePreparationsClick.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable = Nothing
    '        Dim bl As SC3140103BusinessLogic = _
    '                New SC3140103BusinessLogic(Me.deliverypreAbnormalLt, Me.nowDateTime)

    '        Try
    '            ' 現在時刻取得
    '            Dim staffInfo As StaffContext = StaffContext.Current
    '            Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

    '            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '            'dt = bl.GetReserveChipInfo()
    '            dt = bl.GetReserveChipInfo(SASelector.SelectedValue.ToString)
    '            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

    '            Dim todayCount As Long = 0
    '            Dim nextCount As Long = 0
    '            ' 事前準備チップ情報を取得

    '            '当日、および翌日の件数を取得
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                'SA事前準備フラグが未完了のもの（整備受注Noがないものも含まれる）を件数にカウントする
    '                If PreparationMiddle.Equals(row.SASTATUSFLG) Then
    '                    If (row.TODAYFLG.Equals("1")) Then
    '                        '当日件数加算
    '                        todayCount += 1
    '                    Else
    '                        '翌日件数加算
    '                        nextCount += 1
    '                    End If
    '                End If
    '            Next

    '            'アイコンの固定文字列取得
    '            Dim strRightIcnD As String = WebWordUtility.GetWord(APPLICATIONID, 7)
    '            Dim strRightIcnI As String = WebWordUtility.GetWord(APPLICATIONID, 8)
    '            Dim strRightIcnS As String = WebWordUtility.GetWord(APPLICATIONID, 9)

    '            '固定文字列「～」取得
    '            wordFixedString = WebWordUtility.GetWord(APPLICATIONID, 32)
    '            '事前準備エリアチップ初期設定
    '            Me.InitAdvancePreparations(dt, strRightIcnD, strRightIcnI, strRightIcnS)

    '            ' 取得した事前準備件数を事前準備ボタンに反映
    '            Dim buttonStatus As String
    '            Dim countResult As String
    '            If todayCount = 0 Then
    '                If nextCount = 0 Then
    '                    ' 当日・翌日とも0件なら件数表示なし
    '                    buttonStatus = "0"
    '                    countResult = " "
    '                Else
    '                    buttonStatus = "1"
    '                    countResult = nextCount.ToString(CultureInfo.CurrentCulture)
    '                End If
    '            Else
    '                buttonStatus = "2"
    '                countResult = (todayCount + nextCount).ToString(CultureInfo.CurrentCulture)
    '            End If
    '            Me.AdvancePreparationsCntHidden.Value = countResult
    '            Me.AdvancePreparationsColorHidden.Value = buttonStatus

    '            Me.UpdatePanel1.Update()
    '            Me.FotterUpdatePanel.Update()

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            ShowMessageBox(MsgID.id901)
    '        Finally
    '            If dt IsNot Nothing Then
    '                dt.Dispose()
    '                dt = Nothing
    '            End If
    '            If bl IsNot Nothing Then
    '                bl.Dispose()
    '                bl = Nothing
    '            End If
    '        End Try

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END

    '    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '    ''' <summary>
    '    ''' 顧客検索開始のダミーボタンクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 顧客検索の検索開始ボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    Protected Sub SearchCustomerButton_Click(sender As Object,
    '                                             e As System.EventArgs) Handles SearchCustomerDummyButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)


    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        ' 自社客検索処理
    '        Me.GetCustomerInfomation(staffInfo.DlrCD, staffInfo.BrnCD)

    '        Me.SearchDataUpdate.Update()

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' 顧客付替え前確認ボタンのダミーボタンクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 顧客付替えボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    Protected Sub BeforeChipChanges_Click(sender As Object,
    '                                          e As System.EventArgs) Handles BeforeChipChangesDummyButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        ' 付替え元情報取得
    '        Dim visitNumber As Long = SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)
    '        Dim reserveNumber As Long = SetNullToLong(Me.DetailsRezId.Value, DEFAULT_LONG_VALUE)
    '        Dim orderNumber As String = SetNullToString(Me.DetailsOrderNo.Value)
    '        ' 顧客情報取得
    '        Dim registrationNumber As String = SetNullToString(Me.SearchRegistrationNumberChange.Value)
    '        Dim customerCode As String = SetNullToString(Me.SearchCustomerCodeChange.Value)
    '        Dim dmsId As String = SetNullToString(Me.SearchDMSIdChange.Value)
    '        Dim vinNumber As String = SetNullToString(Me.SearchVinChange.Value)
    '        Dim model As String = SetNullToString(Me.SearchModelChange.Value)
    '        Dim customerName As String = SetNullToString(Me.SearchCustomerNameChange.Value)
    '        Dim phone As String = SetNullToString(Me.SearchPhoneChange.Value)
    '        Dim mobile As String = SetNullToString(Me.SearchMobileChange.Value)
    '        Dim saCode As String = SetNullToString(Me.SearchSACodeChange.Value)


    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        'Dim dt As SC3140103BeforeChangeCheckResultDataTable
    '        Dim row As SC3140103BeforeChangeCheckResultRow

    '        Using bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()
    '            row = bl.GetCustomerChangeCheck(staffInfo.DlrCD, _
    '                                          staffInfo.BrnCD, _
    '                                          visitNumber, _
    '                                          registrationNumber, _
    '                                          vinNumber)
    '        End Using


    '        'Dim row As SC3140103BeforeChangeCheckResultRow = _
    '        '    DirectCast(dt.Rows(0), SC3140103BeforeChangeCheckResultRow)

    '        If SC3140103BusinessLogic.ChangeResultTrue.Equals(row.CHANGECHECKRESULT) Then
    '            Me.SetHiddenStatus(row)
    '        ElseIf SC3140103BusinessLogic.ChangeReusltCheck.Equals(row.CHANGECHECKRESULT) Then
    '            Me.SetHiddenStatus(row)
    '            Me.ChipConfirmChange.Value = WebWordUtility.GetWord(APPLICATIONID, MsgID.id907)
    '        Else
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            Me.ShowMessageBox(MsgID.id903)
    '        End If

    '        Me.ChipResultChange.Value = row.CHANGECHECKRESULT.ToString(CultureInfo.CurrentCulture)

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' 顧客付替えボタンのダミーボタンクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 顧客付替えボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Protected Sub ChipChanges_Click(sender As Object,
    '                                    e As System.EventArgs) Handles ChipChangesDummyButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)
    '        ' 付替え元情報取得
    '        Dim beforeVisitNumber As Long = SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)
    '        Dim beforeReserveNumber As Long = SetNullToLong(Me.ChipReserveNumberBefore.Value, DEFAULT_LONG_VALUE)
    '        Dim beforeOrderNumber As String = SetNullToString(Me.ChipOrderNumberBefore.Value)
    '        ' 付替え先情報取得
    '        Dim afterVisitNumber As Long = SetNullToLong(Me.ChipVisitNumberChange.Value, DEFAULT_LONG_VALUE)
    '        Dim afterReserveNumber As Long = SetNullToLong(Me.ChipReserveNumberChange.Value, DEFAULT_LONG_VALUE)
    '        Dim afterSaCode As String = SetNullToString(Me.ChipSACodeChange.Value)
    '        Dim afterOrderNumber As String = SetNullToString(Me.ChipOrderNumberChange.Value)
    '        ' 顧客情報取得
    '        Dim registrationNumber As String = SetNullToString(Me.SearchRegistrationNumberChange.Value)
    '        Dim customerCode As String = SetNullToString(Me.SearchCustomerCodeChange.Value)
    '        Dim dmsId As String = SetNullToString(Me.SearchDMSIdChange.Value)
    '        Dim vinNumber As String = SetNullToString(Me.SearchVinChange.Value)
    '        Dim model As String = SetNullToString(Me.SearchModelChange.Value)
    '        Dim customerName As String = SetNullToString(Me.SearchCustomerNameChange.Value)
    '        Dim phone As String = SetNullToString(Me.SearchPhoneChange.Value)
    '        Dim mobile As String = SetNullToString(Me.SearchMobileChange.Value)
    '        Dim saCode As String = SetNullToString(Me.SearchSACodeChange.Value)

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'Hiddenより車両IDを取得
    '        Dim afterVehicleId As Long = SetNullToLong(Me.ChipVehicleIdAfter.Value, DEFAULT_LONG_VALUE)

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim customerChange As Long

    '        Using bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            'customerChange = bl.SetCustomerChange(staffInfo.DlrCD, _
    '            '                                      staffInfo.BrnCD, _
    '            '                                      beforeVisitNumber, _
    '            '                                      afterVisitNumber, _
    '            '                                      afterReserveNumber, _
    '            '                                      afterOrderNumber, _
    '            '                                      registrationNumber, _
    '            '                                      customerCode, _
    '            '                                      dmsId, _
    '            '                                      vinNumber, _
    '            '                                      model, _
    '            '                                      customerName, _
    '            '                                      phone, _
    '            '                                      mobile)

    '            customerChange = bl.SetCustomerChange(staffInfo.DlrCD, _
    '                                                  staffInfo.BrnCD, _
    '                                                  beforeVisitNumber, _
    '                                                  afterVisitNumber, _
    '                                                  afterReserveNumber, _
    '                                                  afterOrderNumber, _
    '                                                  registrationNumber, _
    '                                                  customerCode, _
    '                                                  dmsId, _
    '                                                  vinNumber, _
    '                                                  model, _
    '                                                  customerName, _
    '                                                  phone, _
    '                                                  mobile, _
    '                                                  afterVehicleId)

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        End Using



    '        If Not SC3140103BusinessLogic.ResultSuccess = customerChange Then
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            Me.ShowMessageBox(MsgID.id903)
    '        End If

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' 顧客解除ボタンのクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 顧客解除ボタンがクライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    Protected Sub SearchCustomerClearButton_Click(sender As Object,
    '                                                  e As System.EventArgs) Handles ChipClearDummyButton.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        '解除対象の情報を取得
    '        Dim removeVisitNumber As Long = SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)
    '        Dim removeReserveNumber As Long = SetNullToLong(Me.DetailsRezId.Value, DEFAULT_LONG_VALUE)
    '        Dim customerChange As Long = 0

    '        Using bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()
    '            customerChange = bl.SetCustomerClear(removeVisitNumber)
    '        End Using


    '        If SC3140103BusinessLogic.ResultSuccess <> customerChange Then
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            Me.ShowMessageBox(MsgID.id903)
    '        End If

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '    ''' <summary>
    '    ''' 顧客詳細ボタンのクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 顧客詳細ボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    ''' <history>
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' </history>
    '    Protected Sub DetailCustomerButton_Click(sender As Object,
    '                                             e As System.EventArgs) Handles DetailButtonLeftDummy.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim bis As New SC3140103BusinessLogic

    '        Try
    '            ' 選択チップ情報の取得
    '            Dim rezId As Long = Me.SetNullToLong(Me.DetailsRezId.Value, -1)
    '            Dim visitNo As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 ADD START
    '            '受付エリアチップの場合、呼出ステータスを更新
    '            If detailArea = CType(ChipArea.Reception, Long) Then
    '                Dim resultCode As Long = Me.CallCompleteOperation()
    '                If resultCode <> 0 Then
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    Exit Try
    '                End If
    '            End If
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 ADD END

    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　MODIFY START
    '            If CType(ChipArea.AdvancePreparations, Long).Equals(detailArea) Then
    '                Dim dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable = _
    '                     bis.GetAdvancePreparationsReserveInfo(staffInfo.DlrCD, staffInfo.BrnCD, rezId)
    '                Dim dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable = _
    '                    bis.GetAdvancePreparationsVisitManager(staffInfo.DlrCD, staffInfo.BrnCD, rezId)
    '                Dim resultFlg As Boolean = Me.AdvancePrepDetailCustomer(dtReserve, dtVisit, rezId, staffInfo)
    '                If Not resultFlg Then
    '                    Exit Try
    '                End If
    '                'If CType(ChipArea.AdvancePreparations, Long).Equals(detailArea) Then
    '                'Dim rowReserve As SC3140103AdvancePreparationsReserveInfoRow = Nothing
    '                'Dim rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow = Nothing
    '                'Dim tempVisitSeq As Nullable(Of Long) = Nothing
    '                '' 事前準備チップ予約情報の取得
    '                'Dim dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable = _
    '                '    bis.GetAdvancePreparationsReserveInfo(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

    '                '' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 START
    '                'If dtReserve IsNot Nothing AndAlso 0 < dtReserve.Rows.Count Then
    '                '    Dim dtRow As SC3140103AdvancePreparationsReserveInfoRow() = _
    '                '        DirectCast(dtReserve.Select("", "WORKTIME ASC"), SC3140103AdvancePreparationsReserveInfoRow())
    '                '    rowReserve = CType(dtRow(0), SC3140103AdvancePreparationsReserveInfoRow)
    '                '    ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 END
    '                'Else
    '                '    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                '    'タイマークリア
    '                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                '    Me.ShowMessageBox(MsgID.id903)
    '                '    Exit Try
    '                'End If

    '                '' 事前準備チップサービス来店管理情報の取得
    '                'Dim dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable = _
    '                '    bis.GetAdvancePreparationsVisitManager(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

    '                'If dtVisit IsNot Nothing AndAlso 0 < dtVisit.Rows.Count Then
    '                '    rowAdvanceVisit = DirectCast(dtVisit.Rows(0), SC3140103AdvancePreparationsServiceVisitManagementRow)

    '                '    tempVisitSeq = rowAdvanceVisit.VISITSEQ
    '                'End If

    '                ''2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                ''If Not (rowReserve.IsACCOUNT_PLANNull) AndAlso _
    '                ''    Not (staffInfo.Account.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                ''    'SA未振当て
    '                ''    Me.ShowMessageBox(MsgID.id903)
    '                ''    Exit Try
    '                ''End If
    '                ''If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
    '                ''Not (SC3140103BusinessLogic.CustomerSegmentON.Equals(rowReserve.CUSTOMERFLAG)) Then
    '                ''    Me.ShowMessageBox(MsgID.id903)
    '                ''    Exit Try
    '                ''End If
    '                ''空の場合は自分のアカウントを入れる
    '                'If String.IsNullOrEmpty(SASelector.SelectedValue) Then
    '                '    rowReserve.ACCOUNT_PLAN = staffInfo.Account
    '                'Else
    '                '    'SA振当てが一致しない場合はエラー
    '                '    If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                '        'SA未振当て
    '                '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                '        'タイマークリア
    '                '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                '        Me.ShowMessageBox(MsgID.id903)
    '                '        Exit Try
    '                '    End If
    '                'End If
    '                'If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
    '                '   Not (Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, TBL_STALLREZINFO)) Then
    '                '    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                '    'タイマークリア
    '                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                '    '自社客でない/SA振当て済み
    '                '    Me.ShowMessageBox(MsgID.id903)
    '                '    Exit Try
    '                'End If
    '                ''2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '                'If Not IsNothing(rowAdvanceVisit) Then
    '                '    '事前準備チップの来店管理情報がある場合
    '                '    If (Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
    '                '                                 TBL_SERVICE_VISIT_MANAGEMENT)) OrElse
    '                '        (Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then

    '                '        '自社客でない/SA振当て済み
    '                '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                '        'タイマークリア
    '                '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                '        Me.ShowMessageBox(MsgID.id903)
    '                '        Exit Try
    '                '    End If
    '                'End If
    '                '' 顧客詳細画面へ遷移
    '                '' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '                ''Me.RedirectCustomer(tempVisitSeq,
    '                ''                    rezId,
    '                ''                    rowReserve.VCLREGNO,
    '                ''                    rowReserve.VIN,
    '                ''                    rowReserve.MODELCODE,
    '                ''                    rowReserve.TELNO,
    '                ''                    rowReserve.MOBILE,
    '                ''                    rowReserve.CUSTOMERNAME,
    '                ''                    staffInfo.DlrCD,
    '                ''                    "1",
    '                ''                    rowReserve.ACCOUNT_PLAN,
    '                ''                    Me.ChengeCustomerType(rowReserve.CUSTOMERFLAG))
    '                ''2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                ''Me.RedirectCustomer(tempVisitSeq,
    '                ''                    rezId,
    '                ''                    rowReserve.VCLREGNO,
    '                ''                    rowReserve.VIN,
    '                ''                    rowReserve.MODELCODE,
    '                ''                    rowReserve.TELNO,
    '                ''                    rowReserve.MOBILE,
    '                ''                    rowReserve.CUSTOMERNAME,
    '                ''                    staffInfo.DlrCD,
    '                ''                    "1",
    '                ''                    rowReserve.ACCOUNT_PLAN,
    '                ''                    Me.ChengeCustomerType(rowReserve.CUSTOMERFLAG), _
    '                ''                    VisitTypeOff)
    '                'Me.RedirectCustomer(tempVisitSeq,
    '                '                    rezId,
    '                '                    rowReserve.VCLREGNO,
    '                '                    rowReserve.VIN,
    '                '                    rowReserve.MODELCODE,
    '                '                    rowReserve.TELNO,
    '                '                    rowReserve.MOBILE,
    '                '                    rowReserve.CUSTOMERNAME,
    '                '                    staffInfo.DlrCD,
    '                '                    "1",
    '                '                    rowReserve.ACCOUNT_PLAN,
    '                '                    VisitTypeOff)
    '                ''2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '                '' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　MODIFY END
    '            Else
    '                ' 事前準備エリア以外
    '                Dim dt As SC3140103DataSet.SC3140103VisitDataTable = _
    '                    bis.GetVisitChipDetailForNextScreen(visitNo, Me.DetailsArea.Value)

    '                Dim rowVisitData As SC3140103DataSet.SC3140103VisitRow = Nothing
    '                If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
    '                    rowVisitData = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)
    '                Else
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Exit Try
    '                End If

    '                If Not (rowVisitData.IsSACODENull) AndAlso _
    '                    Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                    '担当SAが自分ではない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Exit Try
    '                End If
    '                If Not (rowVisitData.IsCUSTSEGMENTNull) AndAlso _
    '                    Not (SC3140103BusinessLogic.CustomerSegmentON.Equals(rowVisitData.CUSTSEGMENT)) Then
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Exit Try
    '                End If
    '                ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '                'Me.RedirectCustomer(rowVisitData.VISITSEQ, _
    '                '                    rowVisitData.FREZID, _
    '                '                    rowVisitData.VCLREGNO, _
    '                '                    rowVisitData.VIN, _
    '                '                    rowVisitData.MODELCODE, _
    '                '                    rowVisitData.TELNO, _
    '                '                    rowVisitData.MOBILE, _
    '                '                    rowVisitData.CUSTOMERNAME, _
    '                '                    rowVisitData.DLRCD, _
    '                '                    "1",
    '                '                    rowVisitData.SACODE,
    '                '                    rowVisitData.CUSTSEGMENT)
    '                Dim visitType As String = VisitTypeOff
    '                '受付エリアでRO番号がない場合
    '                If CType(ChipArea.Reception, Long).Equals(detailArea) AndAlso _
    '                   (rowVisitData.IsORDERNONull OrElse String.IsNullOrEmpty(rowVisitData.ORDERNO)) Then
    '                    visitType = VisitTypeOn
    '                End If
    '                '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                'Me.RedirectCustomer(rowVisitData.VISITSEQ, _
    '                '                    rowVisitData.FREZID, _
    '                '                    rowVisitData.VCLREGNO, _
    '                '                    rowVisitData.VIN, _
    '                '                    rowVisitData.MODELCODE, _
    '                '                    rowVisitData.TELNO, _
    '                '                    rowVisitData.MOBILE, _
    '                '                    rowVisitData.CUSTOMERNAME, _
    '                '                    rowVisitData.DLRCD, _
    '                '                    "1",
    '                '                    rowVisitData.SACODE,
    '                '                    rowVisitData.CUSTSEGMENT, _
    '                '                    visitType)
    '                Me.RedirectCustomer(rowVisitData.VISITSEQ, _
    '                                    rowVisitData.FREZID, _
    '                                    rowVisitData.VCLREGNO, _
    '                                    rowVisitData.VIN, _
    '                                    rowVisitData.MODELCODE, _
    '                                    rowVisitData.TELNO, _
    '                                    rowVisitData.MOBILE, _
    '                                    rowVisitData.CUSTOMERNAME, _
    '                                    rowVisitData.DLRCD, _
    '                                    "1",
    '                                    rowVisitData.SACODE,
    '                                    visitType)
    '                '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '                ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '            End If
    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            Me.ShowMessageBox(MsgID.id901)
    '        Finally
    '            If bis IsNot Nothing Then
    '                bis.Dispose()
    '                bis = Nothing
    '            End If
    '        End Try

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' R/O参照ボタンのクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' R/O参照ボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    Protected Sub DetailOrderButton_Click(sender As Object,
    '                                          e As System.EventArgs) Handles DetailButtonCenterDummy.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim bis As New SC3140103BusinessLogic

    '        Try
    '            Dim visitNo As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

    '            Dim dt As SC3140103DataSet.SC3140103VisitDataTable = _
    '                bis.GetVisitChipDetailForNextScreen(visitNo, Me.DetailsArea.Value)

    '            Dim rowVisitData As SC3140103DataSet.SC3140103VisitRow = Nothing
    '            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
    '                rowVisitData = CType(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)
    '            Else
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If Not (rowVisitData.IsSACODENull) AndAlso _
    '                Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                '担当SAが自分ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If Not (rowVisitData.IsCUSTSEGMENTNull) AndAlso _
    '                Not (SC3140103BusinessLogic.CustomerSegmentON.Equals(rowVisitData.CUSTSEGMENT)) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If rowVisitData.IsORDERNONull Then
    '                '整備受注Noが未採番
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If
    '            ' R/O参照画面に遷移
    '            Me.RedirectOrderDisp(rowVisitData.ORDERNO)

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            Me.ShowMessageBox(MsgID.id901)
    '        Finally
    '            If bis IsNot Nothing Then
    '                bis.Dispose()
    '                bis = Nothing
    '            End If
    '        End Try

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    ''' <summary>
    '    ''' 追加作業ボタンのクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 追加作業ボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    Protected Sub DetailApprovalButton_Click(sender As Object,
    '                                             e As System.EventArgs) Handles DetailButtonRightDummy.Click
    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim bis As New SC3140103BusinessLogic

    '        Try
    '            Dim visitNo As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '            Dim dt As SC3140103DataSet.SC3140103VisitDataTable = _
    '                bis.GetVisitChipDetailForNextScreen(visitNo, Me.DetailsArea.Value)

    '            Dim rowVisitData As SC3140103DataSet.SC3140103VisitRow = Nothing
    '            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
    '                rowVisitData = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)
    '            Else
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If Not (rowVisitData.IsSACODENull) AndAlso _
    '                Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                '担当SAが自分ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If Not (rowVisitData.IsCUSTSEGMENTNull) AndAlso _
    '            Not (SC3140103BusinessLogic.CustomerSegmentON.Equals(rowVisitData.CUSTSEGMENT)) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If

    '            If rowVisitData.IsORDERNONull Then
    '                '整備受注Noが未採番
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Exit Try
    '            End If
    '            Me.RedirectAddWork(rowVisitData.ORDERNO)

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            Me.ShowMessageBox(MsgID.id901)
    '        Finally
    '            If bis IsNot Nothing Then
    '                bis.Dispose()
    '                bis = Nothing
    '            End If
    '        End Try

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 STRAT
    '    ''' <summary>
    '    ''' 完成検査承認ボタンのクリック処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks>
    '    ''' 完成検査承認ボタンを押下した際に隠しボタンである当ボタンが
    '    ''' クライアント側でクリックされることでイベントが発生します。
    '    ''' </remarks>
    '    ''' <history>
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' </history>
    '    Protected Sub DetailButtonInspectionDummy_Click(sender As Object, e As System.EventArgs) _
    '        Handles DetailButtonInspectionDummy.Click

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '        'Try
    '        'インスタンス
    '        'Using sc3140103Biz As New SC3140103BusinessLogic
    '        Using sc3140103Biz As New SC3140103BusinessLogic
    '            Try
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '                '来店実績連番を取得する
    '                Dim visitNo As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '                '来店情報を取得
    '                Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable = _
    '                    sc3140103Biz.GetVisitManager(visitNo)

    '                '来店情報取得結果を判定
    '                If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
    '                    'DateRowに変換
    '                    Dim rowVisitData As SC3140103DataSet.SC3140103ServiceVisitManagementRow = _
    '                        DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103ServiceVisitManagementRow)

    '                    '整備受注NOの確認
    '                    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '                    'If rowVisitData.IsORDERNONull OrElse _
    '                    'String.Empty.Equals(rowVisitData.ORDERNO.Trim) Then
    '                    If rowVisitData.IsORDERNONull OrElse _
    '                    String.IsNullOrEmpty(rowVisitData.ORDERNO.Trim) Then
    '                        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '                        '整備受注Noが取得できなっかた場合
    '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} ORDERNO = NUll or Empty" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '                        Me.ShowMessageBox(MsgID.id903)
    '                        Exit Try
    '                    End If
    '                    'ログインユーザー情報取得
    '                    Dim staffInfo As StaffContext = StaffContext.Current
    '                    'ログインユーザーアカウント                
    '                    Dim renameSACode As String = staffInfo.Account
    '                    Dim splitString() As String
    '                    ' IF用にSAコードの調整-「@」より前の文字列取得
    '                    splitString = renameSACode.Split(CType("@", Char))
    '                    renameSACode = splitString(0)

    '                    '戻り値
    '                    Dim returnCode As Integer = 0
    '                    'BMTSAPI(IC3802102)
    '                    Dim ic3802102Biz As New IC3802102BusinessLogic
    '                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                          , "{0}.{1} CALL IC3802102 SubmitComfirm ORDERNO = {2} DLRCD = {3} ACCOUNT = {4}" _
    '                          , Me.GetType.ToString _
    '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                          , rowVisitData.ORDERNO.Trim _
    '                          , staffInfo.DlrCD _
    '                          , renameSACode))
    '                    '完成検査承認API
    '                    returnCode = ic3802102Biz.SubmitComfirm(rowVisitData.ORDERNO.Trim, _
    '                                                            staffInfo.DlrCD, _
    '                                                            renameSACode)

    '                    '戻り値判定
    '                    If returnCode <> 0 Then
    '                        '戻り値エラー
    '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} CALL IC3802102 RETURNCODE = {2} " _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                               , returnCode))

    '                        Me.ShowMessageBox(MsgID.id910)
    '                    End If

    '                Else '来店情報が取得できなかった場合  
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} TBL_SERVICE_VISIT_MANAGEMENT = NOTHING  VISITSEQ = {2} " _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                               , visitNo))

    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Exit Try
    '                End If
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START    
    '                'End Using
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END	
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} OracleExceptionEx ex.Number = 1013(DBTimeOut)" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '                'タイムアウトエラーの場合は、メッセージを表示する
    '                Me.ShowMessageBox(MsgID.id901)
    '            End Try
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START    
    '        End Using
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub
    '    '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    '    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
    '    ''' <summary>
    '    ''' 削除ボタンタップ処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
    '    ''' </history>
    '    Protected Sub DetailButtonDeleteDummy_Click(sender As Object, e As System.EventArgs) Handles DetailButtonDeleteDummy.Click
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        'ログインユーザー情報取得
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        '現在日時取得
    '        Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)
    '        '選択チップの来店実績連番取得
    '        Dim visitSequence As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '        '削除処理実行
    '        Using bis As New SC3140103BusinessLogic
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '            'Dim returnCOde As Integer = bis.SetReceptDelete(visitSequence, nowDate, staffInfo)
    '            Dim returnCode As Long = bis.SetReceptDelete(visitSequence, nowDate, staffInfo)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
    '        End Using

    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '        'タイマークリア
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} END" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    End Sub

    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START

    '#Region "お客様呼び出し処理"

    '    ''' <summary>
    '    ''' お客様呼び出し処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub CallButton_Click(sender As Object, e As System.EventArgs) Handles CallButton.Click
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Dim blnupdateFlg As Boolean = True
    '        Dim callPlace As String = Me.DetailsCallPlace.Text
    '        Dim callNo As String = Me.DetailsCallNo.Text
    '        '呼出前に入力チェック
    '        blnupdateFlg = CheckCallOperation(callNo, callPlace)
    '        If blnupdateFlg Then
    '            '戻り値
    '            Dim returnCode As Long = 0
    '            '選択チップの来店実績連番
    '            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)
    '            If detailArea <> CType(ChipArea.Reception, Long) Then
    '                Return
    '            End If
    '            'ログインユーザー情報取得
    '            Dim staffInfo As StaffContext = StaffContext.Current
    '            Dim staffAccount As String = staffInfo.Account
    '            Dim dealerCode = staffInfo.DlrCD
    '            Dim storeCode = staffInfo.BrnCD
    '            '現在日時取得
    '            Dim nowDate As DateTime = DateTimeFunc.Now(dealerCode, storeCode)
    '            '更新日時取得
    '            Dim updateDate As Date = Date.MinValue
    '            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Text, updateDate) Then
    '                updateDate = Date.MinValue
    '            End If

    '            '呼び出し処理実行
    '            Using bis As New SC3140103BusinessLogic
    '                returnCode = bis.CallVisit(visitSeq, staffAccount, nowDate, updateDate)
    '            End Using
    '            '戻り値判定
    '            If returnCode <> 0 Then
    '                '戻り値エラー
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} RETURNCODE = {2} " _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , returnCode))
    '                Select Case returnCode
    '                    Case RET_DBTIMEOUT
    '                        Me.ShowMessageBox(MsgID.id901)
    '                    Case RET_NOMATCH
    '                        Me.ShowMessageBox(MsgID.id902)
    '                    Case RET_EXCLUSION
    '                        Me.ShowMessageBox(MsgID.id903)
    '                End Select
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            Else
    '                Me.BtnCALL.Style("display") = "none"
    '                Me.BtnCALLCancel.Style("display") = "block"
    '                Me.DetailsCallPlace.ReadOnly = True
    '                Me.DetailsVisitUpdateDate.Text = nowDate.ToString(CultureInfo.CurrentCulture)
    '                Using bls As New SC3140103BusinessLogic
    '                    bls.SendPushForCall(dealerCode, storeCode)
    '                End Using
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            End If
    '        Else
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        End If
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} END" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    End Sub

    '#End Region

    '#Region "お客様呼び出しキャンセル処理"

    '    ''' <summary>
    '    ''' お客様呼び出しキャンセル処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub CallCancelButton_Click(sender As Object, e As System.EventArgs) Handles CallCancelButton.Click
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        '戻り値
    '        Dim returnCode As Long = 0
    '        '選択チップの来店実績連番
    '        Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '        'ログインユーザー情報取得
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim staffAccount As String = staffInfo.Account
    '        Dim dealerCode = staffInfo.DlrCD
    '        Dim storeCode = staffInfo.BrnCD
    '        '現在日時取得
    '        Dim nowDate As DateTime = DateTimeFunc.Now(dealerCode, storeCode)
    '        '更新日時取得
    '        Dim updateDate As Date = Date.MinValue
    '        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Text, updateDate) Then
    '            updateDate = Date.MinValue
    '        End If
    '        '呼び出し処理実行
    '        Using bis As New SC3140103BusinessLogic
    '            returnCode = bis.CallCancelVisit(visitSeq, staffAccount, nowDate, updateDate)
    '        End Using
    '        '戻り値判定
    '        If returnCode <> 0 Then
    '            '戻り値エラー
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} RETURNCODE = {2} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , returnCode))
    '            Select Case returnCode
    '                Case RET_DBTIMEOUT
    '                    Me.ShowMessageBox(MsgID.id901)
    '                Case RET_NOMATCH
    '                    Me.ShowMessageBox(MsgID.id902)
    '                Case RET_EXCLUSION
    '                    Me.ShowMessageBox(MsgID.id903)
    '            End Select
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '        Else
    '            Me.BtnCALLCancel.Style("display") = "none"
    '            Me.BtnCALL.Style("display") = "block"
    '            Me.DetailsCallPlace.ReadOnly = False
    '            Me.DetailsVisitUpdateDate.Text = nowDate.ToString(CultureInfo.CurrentCulture)
    '            Using bls As New SC3140103BusinessLogic
    '                bls.SendPushForCallCancel(dealerCode, storeCode)
    '            End Using
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        End If
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} END" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    End Sub

    '#End Region

    '#Region "呼び出し場所更新"

    '    ''' <summary>
    '    ''' 呼び出し場所更新
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub DetailsCallPlace_Change(sender As Object, e As System.EventArgs) Handles CallPlaceChangeButton.Click
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        'Updateフラグ
    '        Dim blnupdateFlg As Boolean = True
    '        '呼び出し場所
    '        Dim callPlace As String = Me.DetailsCallPlace.Text
    '        '入力内容Check
    '        If Not Validation.IsValidString(callPlace) Then
    '            Me.ShowMessageBox(MsgID.id912)
    '            Dim bakCallPlace = Me.SetNullToString(Me.BakCallPlace.Value)
    '            Me.DetailsCallPlace.Text = bakCallPlace
    '            blnupdateFlg = False
    '        End If
    '        If blnupdateFlg Then
    '            '戻り値
    '            Dim returnCode As Long = 0
    '            '選択チップの来店実績連番
    '            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)
    '            'ログインユーザー情報取得
    '            Dim staffInfo As StaffContext = StaffContext.Current
    '            Dim staffAccount As String = staffInfo.Account
    '            '現在日時取得
    '            Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)
    '            '更新日時取得
    '            Dim updateDate As Date = Date.MinValue
    '            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Text, updateDate) Then
    '                updateDate = Date.MinValue
    '            End If
    '            '呼び出し場所更新処理実行
    '            Using bis As New SC3140103BusinessLogic
    '                returnCode = bis.CallPlaceChange(visitSeq, callPlace.Trim, staffAccount, nowDate, updateDate)
    '            End Using
    '            '戻り値判定
    '            If returnCode <> 0 Then
    '                '戻り値エラー
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} RETURNCODE = {2} " _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , returnCode))
    '                Select Case returnCode
    '                    Case RET_DBTIMEOUT
    '                        Me.ShowMessageBox(MsgID.id901)
    '                    Case RET_NOMATCH
    '                        Me.ShowMessageBox(MsgID.id902)
    '                    Case RET_EXCLUSION
    '                        Me.ShowMessageBox(MsgID.id903)
    '                End Select
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            Else
    '                Me.DetailsVisitUpdateDate.Text = nowDate.ToString(CultureInfo.CurrentCulture)
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            End If
    '        Else
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '        End If
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} END" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    End Sub

    '#End Region

    '#Region "来店管理ボタン"

    '    ''' <summary>
    '    ''' 来店管理ボタンを押した時の処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub VisitManagementFooterButton_Click(ByVal sender As Object, _
    '                                                    ByVal e As System.EventArgs) Handles VisitManagementFooterButton.Click
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} START" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '        '来店管理画面に遷移する
    '        Me.RedirectNextScreen(VISIT_MANAGEMENT_LIST_PAGE)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    End Sub

    '#End Region
    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    '#End Region

    '#Region " 非公開メソッド"
    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' カウンターエリアで使用する経過時間設定
    '    ' ''' </summary>
    '    ' ''' <remarks></remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Sub SetCounterTime()

    '    '    Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()

    '    '    ' ストール設定情報取得(標準時間専用)
    '    '    Using dt As SC3140103DataSet.SC3140103StallCtl2DataTable = bl.GetStallControl()
    '    '        If dt.Rows.Count > 0 Then
    '    '            Dim row As SC3140103DataSet.SC3140103StallCtl2Row = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103StallCtl2Row)
    '    '            Me.mlngReceptNoresWarningLt = row.RECEPT_NORES_WARNING_LT
    '    '            Me.mlngReceptNoresAbnormalLt = row.RECEPT_NORES_ABNORMAL_LT
    '    '            Me.mlngReceptResWarningLt = row.RECEPT_RES_WARNING_LT
    '    '            Me.mlngReceptResAbnormalLt = row.RECEPT_RES_ABNORMAL_LT
    '    '            Me.mlngAddworkNoresWarningLt = row.ADDWORK_NORES_WARNING_LT
    '    '            Me.mlngAddworkNoresAbnormalLt = row.ADDWORK_NORES_ABNORMAL_LT
    '    '            Me.mlngAddworkResWarningLt = row.ADDWORK_RES_WARNING_LT
    '    '            Me.mlngAddworkResAbnormallt = row.ADDWORK_RES_ABNORMAL_LT
    '    '            Me.mlngDeliverypreAbnormalLt = row.DELIVERYPRE_ABNORMAL_LT
    '    '            Me.mlngDeliverywrAbnormalLt = row.DELIVERYWR_ABNORMAL_LT
    '    '        End If
    '    '    End Using

    '    'End Sub
    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ情報取得
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能強化に向けた要件確認
    '    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    '    ''' </History>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitVisitChip()

    '        ' 現在時刻取得
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

    '        Dim bl As SC3140103BusinessLogic = _
    '            New SC3140103BusinessLogic(Me.deliverypreAbnormalLt, Me.nowDateTime)

    '        Try

    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            'サービス標準LT取得
    '            Dim dtStanderdLt As StandardLTListDataTable = bl.GetStandardLTList(staffInfo.DlrCD,
    '                                                                               staffInfo.BrnCD)
    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            ' チップ情報取得
    '            Using dt As SC3140103DataSet.SC3140103VisitChipDataTable = bl.GetVisitChip()
    '                ' 2012/02/27 KN 西田【SERVICE_1】START
    '                'アイコンの固定文字列取得
    '                Dim strRightIcnD As String = WebWordUtility.GetWord(APPLICATIONID, 7)
    '                Dim strRightIcnI As String = WebWordUtility.GetWord(APPLICATIONID, 8)
    '                Dim strRightIcnS As String = WebWordUtility.GetWord(APPLICATIONID, 9)

    '                '固定文字列「～」取得
    '                wordFixedString = WebWordUtility.GetWord(APPLICATIONID, 32)
    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '                '受付エリアチップ初期設定
    '                Me.InitReception(dt, strRightIcnD, strRightIcnI, strRightIcnS, dtStanderdLt)
    '                '追加承認エリアチップ初期設定
    '                Me.InitApproval(dt, strRightIcnD, strRightIcnI, strRightIcnS, dtStanderdLt)
    '                '納車準備エリアチップ初期設定
    '                Me.InitPreparation(dt, strRightIcnD, strRightIcnI, strRightIcnS, dtStanderdLt)
    '                '納車作業エリアチップ初期設定
    '                Me.InitDelivery(dt, strRightIcnD, strRightIcnI, strRightIcnS, dtStanderdLt)
    '                '作業中エリアチップ初期設定
    '                Me.InitWork(dt, strRightIcnD, strRightIcnI, strRightIcnS)
    '                ' 2012/02/27 KN 西田【SERVICE_1】END
    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END


    '                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能強化に向けた要件確認 START
    '                '受付待ちエリアチップ初期設定
    '                Me.InitAssignment(bl, staffInfo.DlrCD, staffInfo.BrnCD, strRightIcnD, strRightIcnI, strRightIcnS)

    '                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能強化に向けた要件確認 END


    '            End Using

    '            '2012/06/01 西岡 事前準備追加 START
    '            ' 事前準備件数を取得
    '            Dim countList As List(Of Long) = bl.GetAdvancePreparationsCount()

    '            Dim todayCount As Long = countList(0)
    '            Dim nextCount As Long = countList(1)

    '            ' 取得した事前準備件数を事前準備ボタンに反映
    '            Dim buttonStatus As String
    '            Dim countResult As String
    '            If todayCount = 0 Then
    '                If nextCount = 0 Then
    '                    ' 当日・翌日とも0件なら件数表示なし
    '                    buttonStatus = "0"
    '                    countResult = " "
    '                Else
    '                    buttonStatus = "1"
    '                    countResult = nextCount.ToString(CultureInfo.CurrentCulture)
    '                End If
    '            Else
    '                buttonStatus = "2"
    '                countResult = (todayCount + nextCount).ToString(CultureInfo.CurrentCulture)
    '            End If
    '            Me.AdvancePreparationsCntHidden.Value = countResult
    '            Me.AdvancePreparationsColorHidden.Value = buttonStatus
    '            '2012/06/01 西岡 事前準備追加 END

    '            ' 現在時刻取得(最新)
    '            'Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)

    '        Finally
    '            If bl IsNot Nothing Then
    '                bl.Dispose()
    '                bl = Nothing
    '            End If
    '        End Try
    '    End Sub

    '    '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 受付エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' </history>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitReception(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable,
    '                              ByVal strRightIcnD As String,
    '                              ByVal strRightIcnI As String,
    '                              ByVal strRightIcnS As String,
    '                              ByVal dtStanderdLt As StandardLTListDataTable)

    '        '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' コントロールにバインドする
    '        Me.ReceptionRepeater.DataSource =
    '        dt.Select(String.Format(CultureInfo.CurrentCulture,
    '                                "DISP_DIV = '{0}'",
    '                                SC3140103BusinessLogic.DisplayDivReception), "DISP_SORT")
    '        Me.ReceptionRepeater.DataBind()

    '        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = _
    '            DirectCast(ReceptionRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())


    '        ' 2012/02/27 KN 西田【SERVICE_1】START
    '        Dim reception As Control
    '        Dim row As SC3140103DataSet.SC3140103VisitChipRow

    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        'Dim strVisitTime As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strParkingNumber As String

    '        Dim strReserveMark As String
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String
    '        Dim strStart As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl
    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To ReceptionRepeater.Items.Count - 1

    '            reception = ReceptionRepeater.Items(i)
    '            row = rowList(i)

    '            strRegistrationNumber = row.VCLREGNO
    '            strCustomerName = row.CUSTOMERNAME
    '            'strVisitTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            strParkingNumber = row.PARKINGCODE

    '            strReserveMark = row.REZ_MARK
    '            strJDPMark = row.JDP_MARK
    '            strSSCMark = row.SSC_MARK
    '            strStart = row.DISP_START

    '            CType(reception.FindControl("RegistrationNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            CType(reception.FindControl("CustomerName"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            'CType(reception.FindControl("VisitTime"), HtmlContainerControl) _
    '            '   .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strVisitTime), ChipArea.Reception)
    '            CType(reception.FindControl("RepresentativeWarehousing"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
    '            CType(reception.FindControl("ParkingNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strParkingNumber, C_DEFAULT_CHIP_SPACE)

    '            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                reception.FindControl("RightIcnD").Visible = True
    '            Else
    '                reception.FindControl("RightIcnD").Visible = False
    '            End If

    '            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                reception.FindControl("RightIcnI").Visible = True
    '            Else
    '                reception.FindControl("RightIcnI").Visible = False
    '            End If

    '            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                reception.FindControl("RightIcnS").Visible = True
    '            Else
    '                reception.FindControl("RightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            CType(reception.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            CType(reception.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            CType(reception.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            'CType(reception.FindControl("ElapsedTime"), HtmlContainerControl).InnerText = C_MIN_TIME_DISP
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            divDeskDevice = CType(reception.FindControl("ReceptionDeskDevice"), HtmlContainerControl)
    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("approvalId") = row.APPROVAL_ID
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
    '                .Attributes("callStatus") = row.CALLSTATUS
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    '                ' 仕掛中チェック
    '                If Me.SetNullToString(strStart, "0").Equals("0") Then
    '                    .Attributes("class") = "ColumnContentsBoder"
    '                Else
    '                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
    '                End If
    '            End With

    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            Dim addMinutes As Long = 0

    '            If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
    '                Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)

    '                If Not rowStanderdLt.IsRECEPT_STANDARD_LTNull Then
    '                    addMinutes = rowStanderdLt.RECEPT_STANDARD_LT
    '                End If

    '            End If

    '            divElapsedTime = DirectCast(reception.FindControl("ElapsedTime"), HtmlContainerControl)
    '            'With divElapsedTime
    '            '    .InnerText = C_MIN_TIME_DISP
    '            '    .Attributes("name") = "procgroup"
    '            '    .Attributes("procdate") = CType(row.PROC_DATE, String)
    '            '    .Attributes("overclass1") = "ColumnTimeYellow"
    '            '    .Attributes("overclass2") = "ColumnTimeRed"

    '            '    ''警告
    '            '    If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
    '            '        ''黄色：予約有りの場合、SA振当済みから5分経過かつ10分以下
    '            '        .Attributes("overseconds1") = CType(Me.mlngReceptResWarningLt * 60, String)
    '            '    Else
    '            '        ''黄色：予約無しの場合、SA振当済みから10分経過かつ15分以下
    '            '        .Attributes("overseconds1") = CType(Me.mlngReceptNoresWarningLt * 60, String)
    '            '    End If
    '            '    ''異常
    '            '    If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
    '            '        ''赤色：予約有りの場合、SA振当済みから5分経過かつ10分以下
    '            '        .Attributes("overseconds2") = CType(Me.mlngReceptResAbnormalLt * 60, String)
    '            '    Else
    '            '        ''赤色：予約無しの場合、SA振当済みから10分経過かつ15分以下
    '            '        .Attributes("overseconds2") = CType(Me.mlngReceptNoresAbnormalLt * 60, String)
    '            '    End If
    '            'End With
    '            '2013/04/25 TMEJ 小澤 【開発】ITxxxx_TSL自主研緊急対応（サービス） START
    '            'With divElapsedTime
    '            '    .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '            '    .Attributes(AttributesPropertyLimittime) = CType(row.PROC_DATE.AddMinutes(addMinutes), String)
    '            '    .Attributes(AttributesPropertyOvertime1) = ""
    '            '    .Attributes(AttributesPropertyOvertime2) = CType(row.REZ_DELI_DATE, String)
    '            'End With
    '            '2013/04/25 TMEJ 小澤 【開発】ITxxxx_TSL自主研緊急対応（サービス） END
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.ReceptionDeskTipNumber.Text = _
    '        ReceptionRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub

    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加承認エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitApproval(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, _
    '                             ByVal strRightIcnD As String, _
    '                             ByVal strRightIcnI As String, _
    '                             ByVal strRightIcnS As String, _
    '                             ByVal dtStanderdLt As StandardLTListDataTable)
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' コントロールにバインドする
    '        Me.ApprovalRepeater.DataSource = _
    '        dt.Select(String.Format(CultureInfo.CurrentCulture, _
    '        "DISP_DIV = '{0}'", _
    '        SC3140103BusinessLogic.DisplayDivApproval), "DISP_SORT")
    '        Me.ApprovalRepeater.DataBind()

    '        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = _
    '            DirectCast(ApprovalRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

    '        ' 2012/02/27 KN 西田【SERVICE_1】START
    '        Dim approval As Control
    '        Dim row As SC3140103DataSet.SC3140103VisitChipRow

    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        'Dim strDeliveryPlanTime As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strChargeTechnician As String

    '        Dim strReserveMark As String
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String
    '        Dim strStart As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl
    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To ApprovalRepeater.Items.Count - 1

    '            approval = ApprovalRepeater.Items(i)
    '            row = rowList(i)

    '            strRegistrationNumber = row.VCLREGNO
    '            strCustomerName = row.CUSTOMERNAME
    '            'strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            strChargeTechnician = row.STAFFNAME

    '            strReserveMark = row.REZ_MARK
    '            strJDPMark = row.JDP_MARK
    '            strSSCMark = row.SSC_MARK
    '            strStart = row.DISP_START

    '            CType(approval.FindControl("ApprovalRegistrationNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            CType(approval.FindControl("ApprovalCustomerName"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            'CType(approval.FindControl("ApprovalDeliveryPlanTime"), HtmlContainerControl) _
    '            '   .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Approval)
    '            CType(approval.FindControl("ApprovalRepresentativeWarehousing"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
    '            CType(approval.FindControl("ApprovalChargeTechnician"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

    '            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                approval.FindControl("RightIcnD").Visible = True
    '            Else
    '                approval.FindControl("RightIcnD").Visible = False
    '            End If

    '            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                approval.FindControl("RightIcnI").Visible = True
    '            Else
    '                approval.FindControl("RightIcnI").Visible = False
    '            End If

    '            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                approval.FindControl("RightIcnS").Visible = True
    '            Else
    '                approval.FindControl("RightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            CType(approval.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            CType(approval.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            CType(approval.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

    '            divDeskDevice = CType(approval.FindControl("ApprovalDeskDevice"), HtmlContainerControl)
    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("approvalId") = row.APPROVAL_ID
    '                ' 仕掛中チェック
    '                If Me.SetNullToString(strStart, "0").Equals("0") Then
    '                    .Attributes("class") = "ColumnContentsBoder"
    '                Else
    '                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
    '                End If
    '            End With

    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            '納車予定時刻
    '            CType(approval.FindControl("ApprovalDeliveryPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetDateTimeToString(row.REZ_DELI_DATE)

    '            Dim addMinutes As Long = 0
    '            If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
    '                Dim rowStanderdLt As StandardLTListRow = _
    '                    DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)

    '                If Not rowStanderdLt.IsADDWORK_STANDARD_LTNull Then
    '                    addMinutes = rowStanderdLt.ADDWORK_STANDARD_LT
    '                End If
    '            End If

    '            divElapsedTime = DirectCast(approval.FindControl("ApprovalElapsedTime"), HtmlContainerControl)
    '            'With divElapsedTime
    '            '    .InnerText = C_MIN_TIME_DISP
    '            '    .Attributes("name") = "procgroup"
    '            '    .Attributes("procdate") = CType(row.PROC_DATE, String)
    '            '    .Attributes("overclass1") = "ColumnTimeYellow"
    '            '    .Attributes("overclass2") = "ColumnTimeRed"

    '            '    ''警告
    '            '    If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
    '            '        ''黄色：予約有りの場合、SA振当済みから5分経過かつ10分以下
    '            '        .Attributes("overseconds1") = CType(Me.mlngAddworkResWarningLt * 60, String)
    '            '    Else
    '            '        ''黄色：予約無しの場合、SA振当済みから10分経過かつ15分以下
    '            '        .Attributes("overseconds1") = CType(Me.mlngAddworkNoresWarningLt * 60, String)
    '            '    End If
    '            '    ''異常
    '            '    If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
    '            '        ''赤色：予約有りの場合、SA振当済みから5分経過かつ10分以下
    '            '        .Attributes("overseconds2") = CType(Me.mlngAddworkResAbnormallt * 60, String)
    '            '    Else
    '            '        ''赤色：予約無しの場合、SA振当済みから10分経過かつ15分以下
    '            '        .Attributes("overseconds2") = CType(Me.mlngAddworkNoresAbnormalLt * 60, String)
    '            '    End If
    '            'End With

    '            With divElapsedTime
    '                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '                'SA承認依頼日時＋追加作業標準時間
    '                .Attributes(AttributesPropertyLimittime) = CType(row.PROC_DATE.AddMinutes(addMinutes), String)
    '                '納車見込遅れ時刻
    '                .Attributes(AttributesPropertyOvertime1) = CType(row.DELAY_DELI_TIME, String)
    '                '納車予定時刻
    '                .Attributes(AttributesPropertyOvertime2) = CType(row.REZ_DELI_DATE, String)
    '            End With
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.ApprovalNumber.Text = ApprovalRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub

    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    ''' <summary>
    '    ''' 納車準備エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    '    ''' <remarks></remarks>
    '    Private Sub InitPreparation(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, _
    '                                ByVal strRightIcnD As String, _
    '                                ByVal strRightIcnI As String, _
    '                                ByVal strRightIcnS As String, _
    '                                ByVal dtStanderdLt As StandardLTListDataTable)
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' コントロールにバインドする
    '        Me.PreparationRepeater.DataSource = _
    '        dt.Select(String.Format(CultureInfo.CurrentCulture, _
    '        "DISP_DIV = '{0}'", _
    '        SC3140103BusinessLogic.DisplayDivPreparation), "DISP_SORT")
    '        Me.PreparationRepeater.DataBind()

    '        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = _
    '            DirectCast(PreparationRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

    '        ' 2012/02/27 KN 西田【SERVICE_1】START
    '        Dim preparation As Control
    '        Dim row As SC3140103DataSet.SC3140103VisitChipRow

    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        'Dim strDeliveryPlanTime As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strChargeTechnician As String

    '        Dim strReserveMark As String
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String
    '        Dim strStart As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl
    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To PreparationRepeater.Items.Count - 1

    '            preparation = PreparationRepeater.Items(i)
    '            row = rowList(i)

    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '            '2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） START
    '            ''ステータス情報取得する
    '            'Dim drOrderStatus As IC3801901DataSet.OrderStatusDataRow
    '            'Dim bizIC3801901 As New IC3801901BusinessLogic
    '            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '               , "{0}.{1} CALL IC3801901BusinessLogic.GetOrderStatus P1:{2} P2:{3} P3:{4}" _
    '            '               , Me.GetType.ToString _
    '            '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '            '               , row.DLRCD, row.STRCD, row.ORDERNO))
    '            'drOrderStatus = bizIC3801901.GetOrderStatus(row.DLRCD, row.STRCD, row.ORDERNO)



    '            ''「起票者=1:TC」 AndAlso 「追加作業ステータス:1(TC起票中),2(CT承認待ち),3(PS部品見積待ち)」の場合、追加作業起票中アイコンを表示する
    '            'If Not (drOrderStatus.IsDRAWERNull OrElse String.IsNullOrEmpty(drOrderStatus.DRAWER)) AndAlso ReissueVouchersTC.Equals(drOrderStatus.DRAWER) AndAlso _
    '            '    (New String() {AddOrderStatusTC, AddOrderStatusCT, AddOrderStatusPS}.Contains(drOrderStatus.ADDStatus)) Then
    '            '    CType(preparation.FindControl("WorkIcon"), HtmlContainerControl).Attributes("class") = "Icn02"
    '            'End If

    '            '2012/09/18 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.66） END

    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '            strRegistrationNumber = row.VCLREGNO
    '            strCustomerName = row.CUSTOMERNAME
    '            'strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            strChargeTechnician = row.STAFFNAME

    '            strReserveMark = row.REZ_MARK
    '            strJDPMark = row.JDP_MARK
    '            strSSCMark = row.SSC_MARK
    '            strStart = row.DISP_START

    '            CType(preparation.FindControl("PreparationRegistrationNumber"), HtmlContainerControl) _
    '            .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            CType(preparation.FindControl("PreparationCustomerName"), HtmlContainerControl) _
    '            .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            'CType(preparation.FindControl("PreparationDeliveryPlanTime"), HtmlContainerControl) _
    '            '   .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Preparation)
    '            CType(preparation.FindControl("PreparationRepresentativeWarehousing"), HtmlContainerControl) _
    '            .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
    '            CType(preparation.FindControl("PreparationChargeTechnician"), HtmlContainerControl) _
    '            .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

    '            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                preparation.FindControl("RightIcnD").Visible = True
    '            Else
    '                preparation.FindControl("RightIcnD").Visible = False
    '            End If

    '            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                preparation.FindControl("RightIcnI").Visible = True
    '            Else
    '                preparation.FindControl("RightIcnI").Visible = False
    '            End If

    '            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                preparation.FindControl("RightIcnS").Visible = True
    '            Else
    '                preparation.FindControl("RightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            CType(preparation.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            CType(preparation.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            CType(preparation.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

    '            divDeskDevice = CType(preparation.FindControl("PreparationDeskDevice"), HtmlContainerControl)

    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("approvalId") = row.APPROVAL_ID
    '                ' 仕掛中チェック
    '                If Me.SetNullToString(strStart, "0").Equals("0") Then
    '                    .Attributes("class") = "ColumnContentsBoder"
    '                Else
    '                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
    '                End If
    '            End With

    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            '納車予定時刻
    '            CType(preparation.FindControl("PreparationDeliveryPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetDateTimeToString(row.REZ_DELI_DATE)

    '            Dim addMinutes As Long = 0
    '            If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
    '                Dim rowStanderdLt As StandardLTListRow = _
    '                    DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)

    '                If WashFlagFalse.Equals(row.WASHFLG) Then
    '                    '洗車ありの場合
    '                    If rowStanderdLt.WASHTIME < rowStanderdLt.DELIVERYPRE_STANDARD_LT Then
    '                        If Not rowStanderdLt.IsDELIVERYPRE_STANDARD_LTNull Then
    '                            addMinutes = rowStanderdLt.DELIVERYPRE_STANDARD_LT
    '                        End If
    '                    Else
    '                        If Not rowStanderdLt.IsWASHTIMENull Then
    '                            addMinutes = rowStanderdLt.WASHTIME
    '                        End If
    '                    End If
    '                Else
    '                    '洗車なしの場合
    '                    If Not rowStanderdLt.IsDELIVERYPRE_STANDARD_LTNull Then
    '                        addMinutes = rowStanderdLt.DELIVERYPRE_STANDARD_LT
    '                    End If
    '                End If
    '            End If

    '            divElapsedTime = _
    '            DirectCast(preparation.FindControl("PreparationElapsedTime"), HtmlContainerControl)
    '            'With divElapsedTime
    '            '    .InnerText = C_MIN_TIME_DISP
    '            '    .Attributes("name") = "procgroup"
    '            '    .Attributes("procdate") = CType(row.PROC_DATE, String)
    '            '    .Attributes("overclass1") = "ColumnTimeRed"
    '            '    .Attributes("overclass2") = String.Empty
    '            '    .Attributes("overseconds1") = CType(-Me.mlngDeliverypreAbnormalLt * 60, String)
    '            '    .Attributes("overseconds2") = String.Empty
    '            'End With
    '            With divElapsedTime
    '                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '                '完成検査完了時刻＋納車準備標準時間　または　洗車標準時間
    '                .Attributes(AttributesPropertyLimittime) = CType(row.PROC_DATE.AddMinutes(addMinutes), String)
    '                '納車見込遅れ時刻
    '                .Attributes(AttributesPropertyOvertime1) = CType(row.DELAY_DELI_TIME, String)
    '                '納車予定時刻
    '                .Attributes(AttributesPropertyOvertime2) = CType(row.REZ_DELI_DATE, String)
    '            End With
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.PreparationNumber.Text = _
    '        PreparationRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub

    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 納車作業エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitDelivery(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, _
    '                             ByVal strRightIcnD As String, _
    '                             ByVal strRightIcnI As String, _
    '                             ByVal strRightIcnS As String, _
    '                             ByVal dtStanderdLt As StandardLTListDataTable)

    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' コントロールにバインドする
    '        Me.DeliveryRepeater.DataSource = _
    '        dt.Select(String.Format(CultureInfo.CurrentCulture, _
    '        "DISP_DIV = '{0}'", _
    '        SC3140103BusinessLogic.DisplayDivDelivery), "DISP_SORT")
    '        Me.DeliveryRepeater.DataBind()

    '        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = _
    '            DirectCast(DeliveryRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

    '        ' 2012/02/27 KN 西田【SERVICE_1】START
    '        Dim delivery As Control
    '        Dim row As SC3140103DataSet.SC3140103VisitChipRow

    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        'Dim strDeliveryPlanTime As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strChargeTechnician As String

    '        Dim strReserveMark As String
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String
    '        Dim strStart As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl
    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To DeliveryRepeater.Items.Count - 1

    '            delivery = DeliveryRepeater.Items(i)
    '            row = rowList(i)

    '            strRegistrationNumber = row.VCLREGNO
    '            strCustomerName = row.CUSTOMERNAME
    '            'strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            strChargeTechnician = row.STAFFNAME

    '            strReserveMark = row.REZ_MARK
    '            strJDPMark = row.JDP_MARK
    '            strSSCMark = row.SSC_MARK
    '            strStart = row.DISP_START

    '            CType(delivery.FindControl("DeliveryRegistrationNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            CType(delivery.FindControl("DeliveryCustomerName"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            'CType(delivery.FindControl("DeliveryDeliveryPlanTime"), HtmlContainerControl) _
    '            '   .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Delivery)
    '            CType(delivery.FindControl("DeliveryRepresentativeWarehousing"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
    '            CType(delivery.FindControl("DeliveryChargeTechnician"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

    '            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                delivery.FindControl("RightIcnD").Visible = True
    '            Else
    '                delivery.FindControl("RightIcnD").Visible = False
    '            End If

    '            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                delivery.FindControl("RightIcnI").Visible = True
    '            Else
    '                delivery.FindControl("RightIcnI").Visible = False
    '            End If

    '            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                delivery.FindControl("RightIcnS").Visible = True
    '            Else
    '                delivery.FindControl("RightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            CType(delivery.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            CType(delivery.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            CType(delivery.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

    '            divDeskDevice = CType(delivery.FindControl("DeliveryDeskDevice"), HtmlContainerControl)
    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("approvalId") = row.APPROVAL_ID

    '                ' 仕掛中チェック
    '                If Me.SetNullToString(strStart, "0").Equals("0") Then
    '                    .Attributes("class") = "ColumnContentsBoder"
    '                Else
    '                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
    '                End If
    '            End With

    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            '納車予定時刻
    '            CType(delivery.FindControl("DeliveryDeliveryPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetDateTimeToString(row.REZ_DELI_DATE)

    '            Dim addMinutes As Long = 0
    '            If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
    '                Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)

    '                If Not rowStanderdLt.IsDELIVERYWR_STANDARD_LTNull Then
    '                    addMinutes = rowStanderdLt.DELIVERYWR_STANDARD_LT
    '                End If
    '            End If

    '            divElapsedTime = _
    '                DirectCast(delivery.FindControl("DeliveryElapsedTime"), HtmlContainerControl)
    '            'With divElapsedTime
    '            '    .InnerText = C_MIN_TIME_DISP
    '            '    .Attributes("name") = "procgroup"
    '            '    .Attributes("procdate") = CType(row.PROC_DATE, String)
    '            '    .Attributes("overclass1") = "ColumnTimeRed"
    '            '    .Attributes("overclass2") = String.Empty
    '            '    .Attributes("overSeconds1") = CType(-Me.mlngDeliverywrAbnormalLt * 60, String)
    '            '    .Attributes("overSeconds2") = String.Empty
    '            'End With
    '            With divElapsedTime
    '                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '                '清算書印刷時刻＋納車作業標準時間
    '                .Attributes(AttributesPropertyLimittime) = CType(row.PROC_DATE.AddMinutes(addMinutes), String)
    '                '納車見込遅れ時刻
    '                .Attributes(AttributesPropertyOvertime1) = CType(row.DELAY_DELI_TIME, String)
    '                '納車予定時刻
    '                .Attributes(AttributesPropertyOvertime2) = CType(row.REZ_DELI_DATE, String)
    '            End With
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.DeliveryNumber.Text = DeliveryRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub

    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 作業中エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitWork(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, _
    '                         ByVal strRightIcnD As String, _
    '                         ByVal strRightIcnI As String, _
    '                         ByVal strRightIcnS As String)

    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' コントロールにバインドする
    '        Me.WorkRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                 "DISP_DIV = '{0}'", _
    '                                 SC3140103BusinessLogic.DisplayDivWork), "DISP_SORT")
    '        Me.WorkRepeater.DataBind()

    '        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = _
    '            DirectCast(WorkRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

    '        ' 2012/02/27 KN 西田【SERVICE_1】START
    '        Dim work As Control
    '        Dim row As SC3140103DataSet.SC3140103VisitChipRow

    '        Dim strCompletionPlanTime As String
    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        Dim strDeliveryPlanTime As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strAdditionalWorkNumber As String

    '        Dim strReserveMark As String
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String
    '        Dim strStart As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl
    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To WorkRepeater.Items.Count - 1

    '            work = WorkRepeater.Items(i)
    '            row = rowList(i)

    '            strCompletionPlanTime = row.PROC_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRegistrationNumber = row.VCLREGNO
    '            strCustomerName = row.CUSTOMERNAME
    '            strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            strAdditionalWorkNumber = row.APPROVAL_COUNT.ToString(CultureInfo.CurrentCulture)

    '            strReserveMark = row.REZ_MARK
    '            strJDPMark = row.JDP_MARK
    '            strSSCMark = row.SSC_MARK
    '            strStart = row.DISP_START

    '            'CType(work.FindControl("WorkTimeLag"), HtmlContainerControl).InnerText = C_MIN_TIME_DISP

    '            CType(work.FindControl("WorkCompletionPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetDateStringToString(strCompletionPlanTime)
    '            CType(work.FindControl("WorkRegistrationNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            CType(work.FindControl("WorkCustomerName"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            CType(work.FindControl("WorkCompletionPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Work)
    '            CType(work.FindControl("WorkRepresentativeWarehousing"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)

    '            Dim lngAdditionalWorkNumber As Long = 0
    '            If Not Long.TryParse(strAdditionalWorkNumber, lngAdditionalWorkNumber) Then
    '                lngAdditionalWorkNumber = 0
    '            End If

    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '            'If lngAdditionalWorkNumber > 0 Then
    '            '    ' 画像表示
    '            '    CType(work.FindControl("AdditionalWorkNumber"), HtmlContainerControl) _
    '            '    .InnerHtml = strAdditionalWorkNumber
    '            '    CType(work.FindControl("WorkIcon"), HtmlContainerControl) _
    '            '    .Attributes("class") = "Icn01"
    '            'Else
    '            '    ' 画像非表示
    '            '    CType(work.FindControl("AdditionalWorkNumber"), HtmlContainerControl) _
    '            '    .InnerHtml = String.Empty
    '            'End If

    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                work.FindControl("WorkRightIcnD").Visible = True
    '            Else
    '                work.FindControl("WorkRightIcnD").Visible = False
    '            End If

    '            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                work.FindControl("WorkRightIcnI").Visible = True
    '            Else
    '                work.FindControl("WorkRightIcnI").Visible = False
    '            End If

    '            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                work.FindControl("WorkRightIcnS").Visible = True
    '            Else
    '                work.FindControl("WorkRightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            CType(work.FindControl("WorkRightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            CType(work.FindControl("WorkRightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            CType(work.FindControl("WorkRightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

    '            divDeskDevice = CType(work.FindControl("Working"), HtmlContainerControl)
    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("approvalId") = row.APPROVAL_ID
    '            End With
    '            If Me.SetNullToString(strStart, "0").Equals("0") Then

    '                CType(work.FindControl("WorkDeskDevice"), HtmlContainerControl).Attributes("class") = "ColumnContentsBoder"
    '            Else

    '                CType(work.FindControl("WorkDeskDevice"), HtmlContainerControl).Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
    '            End If


    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            '納車予定時刻
    '            CType(work.FindControl("WorkDeliveryPlanTime"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetDateTimeToString(row.REZ_DELI_DATE)

    '            divElapsedTime = DirectCast(work.FindControl("WorkElapsedTime"), HtmlContainerControl)
    '            'With divElapsedTime
    '            '    .InnerText = C_MIN_TIME_DISP
    '            '    .Attributes("name") = "procgroup"
    '            '    .Attributes("procdate") = CType(row.PROC_DATE, String)
    '            '    .Attributes("overclass1") = "ColumnTimeRed"
    '            '    .Attributes("overclass2") = String.Empty
    '            '    .Attributes("overSeconds1") = "0"
    '            '    .Attributes("overSeconds2") = String.Empty
    '            'End With
    '            With divElapsedTime
    '                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '                '作業終了予定時刻
    '                .Attributes(AttributesPropertyLimittime) = CType(row.PROC_DATE, String)
    '                '納車見込遅れ時刻
    '                .Attributes(AttributesPropertyOvertime1) = CType(row.DELAY_DELI_TIME, String)
    '                '納車予定時刻
    '                .Attributes(AttributesPropertyOvertime2) = CType(row.REZ_DELI_DATE, String)
    '            End With
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.WorkNumber.Text = WorkRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub


    '    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能強化に向けた要件確認 START

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 受付待ちエリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="inDealerCode">販売店コード</param>
    '    ''' <param name="inBranchCode">店舗コード</param>
    '    ''' <param name="strRightIcnD">予約マーク 文言</param>
    '    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    '    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitAssignment(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
    '                             , ByVal inDealerCode As String _
    '                             , ByVal inBranchCode As String _
    '                             , ByVal strRightIcnD As String, _
    '                               ByVal strRightIcnI As String, _
    '                               ByVal strRightIcnS As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} " _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        '受付待ちエリア情報を取得
    '        Dim rowList As SC3140103DataSet.SC3140103AssignmentInfoDataTable = _
    '            inSC3140103BusinessLogic.GetAssignmentInfo(inDealerCode, inBranchCode)

    '        '受付待ちエリア情報をコントロールにバインドする
    '        Me.AssignmentRepeater.DataSource = rowList

    '        'バインド処理
    '        Me.AssignmentRepeater.DataBind()

    '        Dim reception As Control
    '        Dim row As SC3140103DataSet.SC3140103AssignmentInfoRow

    '        '車両登録番号
    '        Dim strRegistrationNumber As String
    '        '顧客氏名
    '        Dim strCustomerName As String
    '        '代表整備項目
    '        Dim strRepresentativeWarehousing As String
    '        '駐車場コード
    '        Dim strParkingNumber As String

    '        '予約アイコンフラグ
    '        Dim strReserveMark As String
    '        'JDPアイコンフラグ
    '        Dim strJDPMark As String
    '        'SSCアイコンフラグ
    '        Dim strSSCMark As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim divElapsedTime As HtmlContainerControl

    '        ' -----------------------------------------------------
    '        ' 各チップにデータを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To AssignmentRepeater.Items.Count - 1

    '            reception = AssignmentRepeater.Items(i)
    '            row = rowList(i)

    '            '車両登録番号
    '            strRegistrationNumber = row.VCLREGNO
    '            '顧客氏名
    '            strCustomerName = row.NAME
    '            '代表整備項目
    '            strRepresentativeWarehousing = row.MERCHANDISENAME
    '            '駐車場コード
    '            strParkingNumber = row.PARKINGCODE
    '            '予約アイコンフラグ
    '            strReserveMark = row.REZ_MARK
    '            'JDPアイコンフラグ
    '            strJDPMark = row.JDP_MARK
    '            'SSCアイコンフラグ
    '            strSSCMark = row.SSC_MARK

    '            '車両登録番号を設定
    '            CType(reception.FindControl("RegistrationNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '            '顧客氏名を設定
    '            CType(reception.FindControl("CustomerName"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '            '代表整備項目を設定
    '            CType(reception.FindControl("RepresentativeWarehousing"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
    '            '駐車場コードを設定
    '            CType(reception.FindControl("ParkingNumber"), HtmlContainerControl) _
    '                .InnerHtml = Me.SetNullToString(strParkingNumber, C_DEFAULT_CHIP_SPACE)

    '            '予約アイコンを設定
    '            If strReserveMark.Equals("1") Then
    '                reception.FindControl("RightIcnD").Visible = True
    '            Else
    '                reception.FindControl("RightIcnD").Visible = False
    '            End If

    '            'JDPアイコンを設定
    '            If strJDPMark.Equals("1") Then
    '                reception.FindControl("RightIcnI").Visible = True
    '            Else
    '                reception.FindControl("RightIcnI").Visible = False
    '            End If

    '            'SSCアイコンを設定
    '            If strSSCMark.Equals("1") Then
    '                reception.FindControl("RightIcnS").Visible = True
    '            Else
    '                reception.FindControl("RightIcnS").Visible = False
    '            End If

    '            'アイコンの文言設定
    '            '予約アイコン文言
    '            CType(reception.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '            '予約アイコンJDPアイコン文言
    '            CType(reception.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '            'SSCアイコン文言
    '            CType(reception.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS


    '            divDeskDevice = CType(reception.FindControl("AssignmentDeskDevice"), HtmlContainerControl)

    '            With divDeskDevice
    '                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                .Attributes("orderNo") = row.ORDERNO
    '                .Attributes("class") = "ColumnContentsBoder"
    '            End With


    '            divElapsedTime = DirectCast(reception.FindControl("ElapsedTime"), HtmlContainerControl)

    '            With divElapsedTime
    '                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup
    '                .Attributes(AttributesPropertyLimittime) = CType(row.VISITTIMESTAMP, String)
    '                .Attributes("Area") = "Assignment"
    '            End With

    '        Next
    '        ' 2012/02/27 KN 西田【SERVICE_1】END

    '        'データ表示件数を表示する
    '        Me.AssignmentNumber.Text = _
    '        AssignmentRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    '    End Sub

    '    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能強化に向けた要件確認 END


    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' チップ詳細情報表示
    '    ' ''' </summary>
    '    ' ''' <remarks>
    '    ' ''' チップ詳細画面に表示する情報を取得し、設定します。
    '    ' ''' </remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Sub InitVisitChipDetail()

    '    '    Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()
    '    '    Dim dt As SC3140103DataSet.SC3140103VisitChipDetailDataTable

    '    '    ' サービス来店者情報取得(チップ詳細)
    '    '    Dim visitSeq As Long = SetNullToLong(Me.DetailsVisitNo.Value)
    '    '    dt = bl.GetVisitChipDetail(visitSeq, Me.DetailsArea.Value)

    '    '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '    '        Dim row As SC3140103DataSet.SC3140103VisitChipDetailRow = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitChipDetailRow)

    '    '        If row.REZ_MARK.Equals("1") Then
    '    '            Me.DetailsRightIconD.Visible = True
    '    '        Else
    '    '            Me.DetailsRightIconD.Visible = False
    '    '        End If

    '    '        If row.JDP_MARK.Equals("1") Then
    '    '            Me.DetailsRightIconI.Visible = True
    '    '        Else
    '    '            Me.DetailsRightIconI.Visible = False
    '    '        End If

    '    '        If row.SSC_MARK.Equals("1") Then
    '    '            Me.DetailsRightIconS.Visible = True
    '    '        Else
    '    '            Me.DetailsRightIconS.Visible = False
    '    '        End If

    '    '        ' 登録番号
    '    '        Me.DetailsRegistrationNumber.Text = Me.SetNullToString(row.VCLREGNO)
    '    '        ' 車種
    '    '        Me.DetailsCarModel.Text = Me.SetNullToString(row.VEHICLENAME)
    '    '        '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '    '        '' モデル
    '    '        'Me.DetailsModel.Text = Me.SetNullToString(row.MODELCODE)
    '    '        '' グレード
    '    '        Me.DetailsModel.Text = Me.SetNullToString(row.GRADE)
    '    '        '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '    '        ' VIN
    '    '        'Me.DetailsVin.Text = Me.SetNullToString(row.VIN)
    '    '        ' 走行距離
    '    '        'If row.MILEAGE >= 0 Then
    '    '        'Me.DetailsMileage.Text = String.Format(CultureInfo.CurrentCulture, "{0:#,0}", row.MILEAGE)
    '    '        'Else
    '    '        'Me.DetailsMileage.Text = String.Empty
    '    '        'End If
    '    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '    '        '' 納車予定日時
    '    '        'If Me.IsDateTimeNull(row.REZ_DELI_DATE) Then
    '    '        '    Me.DetailsDeliveryCarDay.Text = String.Empty
    '    '        'Else
    '    '        '    Me.DetailsDeliveryCarDay.Text = DateTimeFunc.FormatDate(3, row.REZ_DELI_DATE)
    '    '        'End If
    '    '        ' 納車日
    '    '        'If Me.IsDateTimeNull(row.DELIVERDATE) Then
    '    '        'Me.DetailsDeliveryCarDay.Text = String.Empty
    '    '        'Else
    '    '        'Me.DetailsDeliveryCarDay.Text = DateTimeFunc.FormatDate(21, row.DELIVERDATE)
    '    '        'End If

    '    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END
    '    '        ' 顧客名
    '    '        Me.DetailsCustomerName.Text = Me.SetNullToString(row.CUSTOMERNAME)
    '    '        ' 電話番号
    '    '        Me.DetailsPhoneNumber.Text = Me.SetNullToString(row.TELNO)
    '    '        ' 携帯番号
    '    '        Me.DetailsMobileNumber.Text = Me.SetNullToString(row.MOBILE)
    '    '        ' 代表入庫項目
    '    '        'Me.DetailsRepresentativeWarehousing.Text = Me.SetNullToString(row.MERCHANDISENAME)

    '    '        ' 表示区分
    '    '        Select Case Me.DetailsArea.Value
    '    '            Case SC3140103BusinessLogic.DisplayDivReception       ' 受付
    '    '                ' 来店時刻
    '    '                'Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 20)
    '    '                'Me.DetailsVisitTime.Text = Me.SetDateTimeToString(row.VISITTIMESTAMP)

    '    '            Case SC3140103BusinessLogic.DisplayDivApproval,
    '    '            SC3140103BusinessLogic.DisplayDivPreparation,
    '    '            SC3140103BusinessLogic.DisplayDivDelivery          ' 承認依頼・納車準備・納車作業
    '    '                ' 納車予定日時
    '    '                'Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 28)
    '    '                'Me.DetailsVisitTime.Text = Me.SetDateTimeToString(row.REZ_DELI_DATE)

    '    '            Case SC3140103BusinessLogic.DisplayDivWork            ' 作業中
    '    '                ' 作業開始 ～ 作業終了予定時刻
    '    '                'Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 25)
    '    '                'Me.DetailsVisitTime.Text = String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", Me.SetDateTimeToString(row.ACTUAL_STIME),
    '    '                'WebWordUtility.GetWord(APPLICATIONID, 32),
    '    '                'Me.SetDateTimeToString(row.ENDTIME))

    '    '            Case Else

    '    '        End Select

    '    '        ' ボタン表示
    '    '        Me.DetailButtonLeft.Text = Me.InitVisitChipDetailButton(row.BUTTON_LEFT)
    '    '        Me.DetailButtonLeft.Enabled = row.BUTTON_ENABLED_LEFT
    '    '        Me.DetailButtonRight.Text = Me.InitVisitChipDetailButton(row.BUTTON_RIGHT)
    '    '        Me.DetailButtonRight.Enabled = row.BUTTON_ENABLED_RIGHT

    '    '    End If

    '    'End Sub

    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' チップ詳細フッタボタン文言初期設定
    '    ' ''' </summary>
    '    ' ''' <remarks></remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Function InitVisitChipDetailButton(ByVal button As String) As String

    '    '    ' ボタンチェック
    '    '    Select Case button
    '    '        Case SC3140103BusinessLogic.ButtonCustomer          ' 顧客詳細ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 24)

    '    '        Case SC3140103BusinessLogic.ButtonNewCustomer       ' 新規顧客登録ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 22)

    '    '        Case SC3140103BusinessLogic.ButtonNewRO             ' R/O作成ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 23)

    '    '        Case SC3140103BusinessLogic.ButtonRODisplay         ' R/O参照ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 26)

    '    '        Case SC3140103BusinessLogic.ButtonWork              ' 追加作業登録ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 27)

    '    '        Case SC3140103BusinessLogic.ButtonApproval          ' 追加作業承認ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 29)

    '    '        Case SC3140103BusinessLogic.ButtonCheckSheet        ' チェックシートボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 30)

    '    '        Case SC3140103BusinessLogic.ButtonSettlement        ' 清算入力ボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 31)

    '    '            '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '        Case SC3140103BusinessLogic.ButtonWorkPreview       ' 追加作業プレビューボタン
    '    '            Return WebWordUtility.GetWord(APPLICATIONID, 33)

    '    '            '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END
    '    '        Case Else
    '    '            Return String.Empty

    '    '    End Select

    '    'End Function
    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '    ' ''' <summary>
    '    ' ''' 顧客区分を修正(TBL_STALLREZINFO → TBL_SERVICE_VISIT_MANAGEMENT)
    '    ' ''' </summary>
    '    ' ''' <param name="customerType">顧客区分(0:自社客  1:未取引客)</param>
    '    ' ''' <returns>顧客区分(1:自社客  2:未取引客)</returns>
    '    ' ''' <remarks></remarks>
    '    'Private Function ChengeCustomerType(ByVal customerType As String) As String

    '    '    If customerType.Equals("0") Then
    '    '        Return "1"
    '    '    Else
    '    '        Return "2"
    '    '    End If

    '    'End Function
    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '    ' 2012/06/11 日比野 事前準備対応 START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ詳細からの画面遷移処理 (ボタン用)
    '    ''' </summary>
    '    ''' <param name="buttonText">ボタン名称</param>
    '    '''-----------------------------------------------------------------------
    '    Private Sub NextScreenVisitChipDetailButton(ByVal buttonText As String,
    '                                                ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow,
    '                                                ByVal rowVisitData As SC3140103VisitRow,
    '                                                ByVal rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow,
    '                                                ByVal visitNo As Nullable(Of Long))
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:buttonText = {3}, rowReserve = (DataSet)," & _
    '                                   " rowVisitData = (DataSet), rowAdvanceVisit = (DataSet) " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , buttonText))

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        ' 選択チップ情報の取得
    '        Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '        Dim orderNo As String = Me.SetNullToString(Me.DetailsOrderNo.Value)
    '        Dim rezId As Long = Me.SetNullToLong(Me.DetailsRezId.Value, -1)
    '        Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)

    '        ' ボタン名称に応じて遷移先変更
    '        Select Case buttonText
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '            'Case WebWordUtility.GetWord(APPLICATIONID, 24)			' 顧客詳細
    '            'If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
    '            ' 事前準備エリア

    '            'If Not IsNothing(rowAdvanceVisit) Then
    '            '事前準備チップの来店管理情報がある場合

    '            'If (Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, TBL_SERVICE_VISIT_MANAGEMENT)) OrElse _
    '            '(Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then
    '            '自社客でない/担当SAが自分ではない/SA振当て済み

    '            'Return
    '            'End If

    '            'End If

    '            ' 顧客詳細画面に遷移
    '            'Me.RedirectCustomer(wkVisitSeq, _
    '            'rezId, _
    '            'rowReserve.VCLREGNO, _
    '            'rowReserve.VIN, _
    '            'rowReserve.MODELCODE, _
    '            'rowReserve.TELNO, _
    '            'rowReserve.MOBILE, _
    '            'rowReserve.CUSTOMERNAME, _
    '            'staffInfo.DlrCD, _
    '            '"1",
    '            'rowReserve.ACCOUNT_PLAN,
    '            ' Me.ChengeCustomerType(rowReserve.CUSTOMERFLAG))
    '            'Else
    '            ' 事前準備エリア以外

    '            ' 顧客情報画面へ遷移
    '            'Me.RedirectCustomer(rowVisitData.VISITSEQ, _
    '            'rowVisitData.FREZID, _
    '            'rowVisitData.VCLREGNO, _
    '            'rowVisitData.VIN, _
    '            'rowVisitData.MODELCODE, _
    '            'rowVisitData.TELNO, _
    '            'rowVisitData.MOBILE, _
    '            'rowVisitData.CUSTOMERNAME, _
    '            'rowVisitData.DLRCD, _
    '            '"0",
    '            'rowVisitData.SACODE,
    '            'rowVisitData.CUSTSEGMENT)
    '            'End If
    '            'Case WebWordUtility.GetWord(APPLICATIONID, 22)			' 新規顧客登録
    '            Case WebWordUtility.GetWord(APPLICATIONID, 52)          ' 新規顧客登録

    '                Me.ChipDetailNewCustormerButton(detailArea, _
    '                                                staffInfo, _
    '                                                rowReserve, _
    '                                                rowAdvanceVisit, _
    '                                                rowVisitData, _
    '                                                visitNo, _
    '                                                rezId)

    '                'Case WebWordUtility.GetWord(APPLICATIONID, 23)			' R/O作成
    '            Case (WebWordUtility.GetWord(APPLICATIONID, 53))     ' R/O作成

    '                Me.ChipDetailROCreationButton(detailArea, _
    '                                              staffInfo, _
    '                                              rowReserve, _
    '                                              rowAdvanceVisit, _
    '                                              rowVisitData, _
    '                                              visitNo, _
    '                                              rezId)

    '                'Case WebWordUtility.GetWord(APPLICATIONID, 29)			' 追加作業承認
    '            Case WebWordUtility.GetWord(APPLICATIONID, 54)          'R/O編集

    '                Me.ChipDetailROEditButton(detailArea, _
    '                                          staffInfo, _
    '                                          rowReserve, _
    '                                          rowAdvanceVisit, _
    '                                          rowVisitData, _
    '                                          visitNo, _
    '                                          rezId)

    '            Case WebWordUtility.GetWord(APPLICATIONID, 57), _
    '                 WebWordUtility.GetWord(APPLICATIONID, 58), _
    '                 WebWordUtility.GetWord(APPLICATIONID, 59)            '見積り確定、追加作業承認


    '                If Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                    '担当SAが自分ではない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If

    '                If Not (Me.CheckCustomerType(rowVisitData.CUSTSEGMENT, _
    '                                             TBL_SERVICE_VISIT_MANAGEMENT)) Then
    '                    '顧客が自社客ではない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If

    '                If String.IsNullOrEmpty(rowVisitData.ORDERNO) Then
    '                    '整備受注Noが未採番
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '                Dim approvalId As String = Me.SetNullToString(Me.DetailsApprovalId.Value)

    '                Me.RedirectApproval(orderNo, approvalId)            ' 追加作業承認画面に遷移

    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '                'Case WebWordUtility.GetWord(APPLICATIONID, 30)			' チェックシート
    '                'Me.RedirectCheckSheet(orderNo)						' チェックシート印刷画面に遷移

    '                'Case WebWordUtility.GetWord(APPLICATIONID, 31)			' 清算入力
    '            Case WebWordUtility.GetWord(APPLICATIONID, 55), _
    '                 WebWordUtility.GetWord(APPLICATIONID, 56)            ' 清算書作成、清算書編集

    '                If Not staffInfo.Account.Equals(rowVisitData.SACODE) Then
    '                    '担当SAが自分ではない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If

    '                If Not Me.CheckCustomerType(rowVisitData.CUSTSEGMENT, _
    '                                            TBL_SERVICE_VISIT_MANAGEMENT) Then
    '                    '顧客が自社客ではない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If

    '                If String.IsNullOrEmpty(rowVisitData.ORDERNO) Then
    '                    '整備受注Noが未採番
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If

    '                'R/Oステータスと実績ステータスを取得
    '                Dim orderStatus As String = rowVisitData.RO_STATUS
    '                Dim progressStatus As String = String.Empty
    '                If Not (rowVisitData.IsRESULT_STATUSNull) Then
    '                    progressStatus = rowVisitData.RESULT_STATUS
    '                End If

    '                If ROFinSales.Equals(orderStatus) Or _
    '                    ROFinMaintenance.Equals(orderStatus) Then
    '                    'R/Oステータスが売上済/整備完了
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return

    '                ElseIf ROFinInspection.Equals(orderStatus) Then

    '                    If SMWaitWash.Equals(progressStatus) Or _
    '                         SMWash.Equals(progressStatus) Or _
    '                         SMCustody.Equals(progressStatus) Or _
    '                         SMDelivery.Equals(progressStatus) Then
    '                        'R/Oステータスが検査完了、かつ実績ステータスが洗車待ち/洗車中/預かり中/納車待ちのいずれかの場合
    '                        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                        'タイマークリア
    '                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                        Me.ShowMessageBox(MsgID.id903)
    '                        Return
    '                    End If
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '                Me.RedirectSettlement(orderNo)                      ' 清算印刷画面に遷移

    '            Case Else
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                '遷移しない

    '        End Select

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub


    '    ''' <summary>
    '    ''' チップ詳細からの画面遷移処理 (新規顧客登録ボタン用)
    '    ''' </summary>
    '    ''' <param name="detailArea">表示エリア</param>
    '    ''' <param name="staffInfo">ログイン情報</param>
    '    ''' <param name="rowReserve"></param>
    '    ''' <param name="rowAdvanceVisit"></param>
    '    ''' <param name="rowVisitData"></param>
    '    ''' <param name="visitNo"></param>
    '    ''' <param name="rezId">予約ID</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    Private Sub ChipDetailNewCustormerButton(ByVal detailArea As Long, _
    '                                             ByVal staffInfo As StaffContext, _
    '                                             ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow, _
    '                                             ByVal rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow, _
    '                                             ByVal rowVisitData As SC3140103VisitRow, _
    '                                             ByVal visitNo As Nullable(Of Long), _
    '                                             ByVal rezId As Long)

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:detailArea = {3}, staffInfo = (StaffContext)," & _
    '                                   " rowReserve = (DataRow), rowAdvanceVisit = (DataRow)," & _
    '                                   " rowVisitData = (DataRow), rezId = {4} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , detailArea _
    '                                 , CType(rezId, String)))


    '        If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
    '            ' 事前準備エリア

    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'If Not (staffInfo.Account.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '            '    '担当SAが自分ではない
    '            '    Me.ShowMessageBox(MsgID.id903)
    '            '    Return
    '            'End If
    '            '空の場合は自分のアカウントを入れる
    '            If String.IsNullOrEmpty(SASelector.SelectedValue) Then
    '                rowReserve.ACCOUNT_PLAN = staffInfo.Account
    '            Else
    '                'SA振当てが一致しない場合はエラー
    '                If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                    'SA未振当て
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '            End If
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '            If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
    '                (Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, TBL_STALLREZINFO)) Then
    '                '自社客の場合
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If Not IsNothing(rowAdvanceVisit) Then
    '                '事前準備チップの来店管理情報がある場合
    '                If Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
    '                                         TBL_SERVICE_VISIT_MANAGEMENT) Then
    '                    '自社客の場合
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                If Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS) Then
    '                    'SA振当て済み
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '            End If

    '            ' 新規顧客登録画面に遷移
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '            'Me.RedirectCustomerNew(visitNo, _
    '            '                       rezId, _
    '            '                       rowReserve.CUSTOMERNAME, _
    '            '                       rowReserve.VCLREGNO, _
    '            '                       rowReserve.VIN, _
    '            '                       rowReserve.MODELCODE, _
    '            '                       rowReserve.TELNO, _
    '            '                       rowReserve.MOBILE, _
    '            '                       "1", _
    '            '                       rowReserve.ACCOUNT_PLAN)
    '            Me.RedirectCustomerNew(visitNo, _
    '                                   rezId, _
    '                                   rowReserve.CUSTOMERNAME, _
    '                                   rowReserve.VCLREGNO, _
    '                                   rowReserve.VIN, _
    '                                   rowReserve.MODELCODE, _
    '                                   rowReserve.TELNO, _
    '                                   rowReserve.MOBILE, _
    '                                   "1", _
    '                                   rowReserve.ACCOUNT_PLAN, _
    '                                   VisitTypeOff)
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '        Else
    '            ' 事前準備エリア以外(受付エリア)
    '            If Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                '担当SAが自分ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            If (Me.CheckCustomerType(rowVisitData.CUSTSEGMENT, _
    '                                  TBL_SERVICE_VISIT_MANAGEMENT)) Then
    '                '自社客の場合
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            ' 新規顧客登録画面に遷移
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '            'Me.RedirectCustomerNew(rowVisitData.VISITSEQ, _
    '            '                       rowVisitData.FREZID, _
    '            '                       rowVisitData.CUSTOMERNAME, _
    '            '                       rowVisitData.VCLREGNO, _
    '            '                       rowVisitData.VIN, _
    '            '                       rowVisitData.MODELCODE, _
    '            '                       rowVisitData.TELNO, _
    '            '                       rowVisitData.MOBILE, _
    '            '                       "0", _
    '            '                       rowVisitData.SACODE)
    '            Me.RedirectCustomerNew(rowVisitData.VISITSEQ, _
    '                                   rowVisitData.FREZID, _
    '                                   rowVisitData.CUSTOMERNAME, _
    '                                   rowVisitData.VCLREGNO, _
    '                                   rowVisitData.VIN, _
    '                                   rowVisitData.MODELCODE, _
    '                                   rowVisitData.TELNO, _
    '                                   rowVisitData.MOBILE, _
    '                                   "0", _
    '                                   rowVisitData.SACODE, _
    '                                   VisitTypeOn)
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '    End Sub

    '    ''' <summary>
    '    ''' チップ詳細からの画面遷移処理 (R/O作成ボタン用)
    '    ''' </summary>
    '    ''' <param name="detailArea"></param>
    '    ''' <param name="staffInfo"></param>
    '    ''' <param name="rowReserve"></param>
    '    ''' <param name="rowAdvanceVisit"></param>
    '    ''' <param name="rowVisitData"></param>
    '    ''' <param name="visitNo"></param>
    '    ''' <param name="reserveId"></param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    Private Sub ChipDetailROCreationButton(ByVal detailArea As Long, _
    '                                           ByVal staffInfo As StaffContext, _
    '                                           ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow, _
    '                                           ByVal rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow, _
    '                                           ByVal rowVisitData As SC3140103VisitRow, _
    '                                           ByVal visitNo As Nullable(Of Long), _
    '                                           ByVal reserveId As Long)

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:detailArea = {3}, staffInfo = (StaffContext)," & _
    '                                   " rowReserve = (DataRow), rowAdvanceVisit = (DataRow)," & _
    '                                   " rowVisitData = (DataRow), rezId = {4} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , detailArea _
    '                                 , CType(reserveId, String)))

    '        If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
    '            ' 事前準備エリア
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'If Not (staffInfo.Account.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '            '    '担当SAが自分ではない
    '            '    Me.ShowMessageBox(MsgID.id903)
    '            '    Return
    '            'End If
    '            '空の場合は自分のアカウントを入れる

    '            If String.IsNullOrEmpty(SASelector.SelectedValue) Then
    '                rowReserve.ACCOUNT_PLAN = staffInfo.Account
    '            Else
    '                'SA振当てが一致しない場合はエラー
    '                If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    'SA未振当て
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '            End If
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '            If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
    '                Not (Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, _
    '                                          TBL_STALLREZINFO)) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If Not (String.IsNullOrEmpty(rowReserve.ORDERNO)) Then
    '                '整備受注Noが採番済み
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            If Not IsNothing(rowAdvanceVisit) Then
    '                '事前準備チップの来店管理情報がある場合

    '                If Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
    '                                            TBL_SERVICE_VISIT_MANAGEMENT) Then
    '                    '自社客でない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '                If Not (rowAdvanceVisit.IsASSIGNSTATUSNull) AndAlso _
    '                    (Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then
    '                    'SA振当て済み
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                If Not (String.IsNullOrEmpty(rowAdvanceVisit.ORDERNO)) Then
    '                    '整備受注Noが採番済みの場合
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            Else
    '                visitNo = VISITSEQ_NONE_FIRST_VALUE
    '            End If

    '            ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 START
    '            Dim stockTime As New Date
    '            If Not String.IsNullOrEmpty(rowReserve.STOCKTIME) Then
    '                stockTime = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowReserve.STOCKTIME)
    '            Else
    '                stockTime = rowReserve.WORKTIME
    '            End If
    '            ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 END

    '            ' R/O作成画面へ遷移
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'Me.RedirectOrderNew(visitNo.Value, _
    '            '                    staffInfo.DlrCD, _
    '            '                    rowReserve.VCLREGNO, _
    '            '                    rowReserve.VIN, _
    '            '                    rowReserve.MODELCODE, _
    '            '                    rowReserve.CUSTOMERNAME, _
    '            '                    rowReserve.TELNO, _
    '            '                    rowReserve.MOBILE, _
    '            '                    reserveId, staffInfo.BrnCD, _
    '            '                    rowReserve.ACCOUNT_PLAN, _
    '            '                    rowReserve.WASHFLG, _
    '            '                    "1", _
    '            '                    rowReserve.ORDERNO, _
    '            '                    Me.ChengeCustomerType(rowReserve.CUSTOMERFLAG), _
    '            '                    stockTime)
    '            Me.RedirectOrderNew(visitNo.Value, _
    '                                staffInfo.DlrCD, _
    '                                rowReserve.VCLREGNO, _
    '                                rowReserve.VIN, _
    '                                rowReserve.MODELCODE, _
    '                                rowReserve.CUSTOMERNAME, _
    '                                rowReserve.TELNO, _
    '                                rowReserve.MOBILE, _
    '                                reserveId, staffInfo.BrnCD, _
    '                                rowReserve.ACCOUNT_PLAN, _
    '                                rowReserve.WASHFLG, _
    '                                "1", _
    '                                rowReserve.ORDERNO, _
    '                                stockTime)
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        Else
    '            ' 事前準備エリア以外
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '            If Not staffInfo.Account.Equals(rowVisitData.SACODE) Then
    '                '担当SAが自分ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If Not Me.CheckCustomerType(rowVisitData.CUSTSEGMENT,
    '                                        TBL_SERVICE_VISIT_MANAGEMENT) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If Not String.IsNullOrEmpty(rowVisitData.ORDERNO) Then
    '                '整備受注Noが採番済み
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    '            ' R/O作成画面へ遷移
    '            'Me.RedirectOrderNew(rowVisitData.VISITSEQ, _
    '            '                    staffInfo.DlrCD, _
    '            '                    rowVisitData.VCLREGNO, _
    '            '                    rowVisitData.VIN, _
    '            '                    rowVisitData.MODELCODE, _
    '            '                    rowVisitData.CUSTOMERNAME, _
    '            '                    rowVisitData.TELNO, _
    '            '                    rowVisitData.MOBILE, _
    '            '                    rowVisitData.FREZID, _
    '            '                    staffInfo.BrnCD, _
    '            '                    rowVisitData.SACODE, _
    '            '                    rowVisitData.WASHFLG, _
    '            '                    "0", _
    '            '                    rowVisitData.ORDERNO, _
    '            '                    rowVisitData.CUSTSEGMENT, _
    '            '                    rowVisitData.ASSIGNTIMESTAMP)
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'Me.RedirectOrderNew(rowVisitData.VISITSEQ, _
    '            '                    staffInfo.DlrCD, _
    '            '                    rowVisitData.VCLREGNO, _
    '            '                    rowVisitData.VIN, _
    '            '                    rowVisitData.MODELCODE, _
    '            '                    rowVisitData.CUSTOMERNAME, _
    '            '                    rowVisitData.TELNO, _
    '            '                    rowVisitData.MOBILE, _
    '            '                    rowVisitData.FREZID, _
    '            '                    staffInfo.BrnCD, _
    '            '                    rowVisitData.SACODE, _
    '            '                    rowVisitData.WASHFLG, _
    '            '                    "0", _
    '            '                    rowVisitData.ORDERNO, _
    '            '                    rowVisitData.CUSTSEGMENT, _
    '            '                    rowVisitData.ASSIGNTIMESTAMP, _
    '            '                    rowVisitData.CUSTCD)
    '            Me.RedirectOrderNew(rowVisitData.VISITSEQ, _
    '                                staffInfo.DlrCD, _
    '                                rowVisitData.VCLREGNO, _
    '                                rowVisitData.VIN, _
    '                                rowVisitData.MODELCODE, _
    '                                rowVisitData.CUSTOMERNAME, _
    '                                rowVisitData.TELNO, _
    '                                rowVisitData.MOBILE, _
    '                                rowVisitData.FREZID, _
    '                                staffInfo.BrnCD, _
    '                                rowVisitData.SACODE, _
    '                                rowVisitData.WASHFLG, _
    '                                "0", _
    '                                rowVisitData.ORDERNO, _
    '                                rowVisitData.ASSIGNTIMESTAMP, _
    '                                rowVisitData.CUSTID)
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '            ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END
    '        End If

    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '        'Case WebWordUtility.GetWord(APPLICATIONID, 26)			' R/O参照
    '        'Me.RedirectOrderDisp(orderNo)						' R/O参照画面に遷移

    '        '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '        'Case WebWordUtility.GetWord(APPLICATIONID, 27)          ' 追加作業登録
    '        '    Me.RedirectWork(orderNo)                            ' 追加作業登録画面に遷移
    '        'Case WebWordUtility.GetWord(APPLICATIONID, 27),
    '        'WebWordUtility.GetWord(APPLICATIONID, 33)		   ' 追加作業登録又は、追加作業プレビュー
    '        'Me.RedirectAddWork(orderNo)							' 追加作業登録関連画面に遷移
    '        '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '        ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '    End Sub

    '    ''' <summary>
    '    ''' チップ詳細からの画面遷移処理 (R/O編集ボタン用)
    '    ''' </summary>
    '    ''' <param name="detailArea"></param>
    '    ''' <param name="staffInfo"></param>
    '    ''' <param name="rowReserve"></param>
    '    ''' <param name="rowAdvanceVisit"></param>
    '    ''' <param name="rowVisitData"></param>
    '    ''' <param name="visitNo"></param>
    '    ''' <param name="reserveId"></param>
    '    ''' <remarks></remarks>
    '    Private Sub ChipDetailROEditButton(ByVal detailArea As Long, _
    '                                        ByVal staffInfo As StaffContext, _
    '                                        ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow, _
    '                                        ByVal rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow, _
    '                                        ByVal rowVisitData As SC3140103VisitRow, _
    '                                        ByVal visitNo As Nullable(Of Long), _
    '                                        ByVal reserveId As Long)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:detailArea = {3}, staffInfo = (StaffContext)," & _
    '                                   " rowReserve = (DataRow), rowAdvanceVisit = (DataRow)," & _
    '                                   " rowVisitData = (DataRow), rezId = {4} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , detailArea _
    '                                 , CType(reserveId, String)))

    '        If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
    '            ' 事前準備エリア
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'If Not staffInfo.Account.Equals(rowReserve.ACCOUNT_PLAN) Then
    '            '    '担当SAが自分ではない
    '            '    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            '    Me.ShowMessageBox(MsgID.id903)
    '            '    Return
    '            'End If
    '            If String.IsNullOrEmpty(SASelector.SelectedValue) Then
    '                rowReserve.ACCOUNT_PLAN = staffInfo.Account
    '            Else
    '                'SA振当てが一致しない場合はエラー
    '                If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    'SA未振当て
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '            End If
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '            If Not Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, _
    '                                        TBL_STALLREZINFO) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If String.IsNullOrEmpty(rowReserve.ORDERNO) Then
    '                '整備受注Noが未採番
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            If Not IsNothing(rowAdvanceVisit) Then
    '                '事前準備チップの来店管理情報がある場合

    '                If Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
    '                                            TBL_SERVICE_VISIT_MANAGEMENT) Then
    '                    '自社客でない
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '                If Not (rowAdvanceVisit.IsASSIGNSTATUSNull) AndAlso _
    '                    (Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then
    '                    'SA振当て済み
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                If String.IsNullOrEmpty(rowAdvanceVisit.ORDERNO) Then
    '                    '整備受注Noが未採番の場合
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)
    '                    Return
    '                End If
    '                ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            Else
    '                visitNo = VISITSEQ_NONE_FIRST_VALUE
    '            End If

    '            ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 START
    '            Dim stockTime As New Date
    '            If Not String.IsNullOrEmpty(rowReserve.STOCKTIME) Then
    '                stockTime = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowReserve.STOCKTIME)
    '            Else
    '                stockTime = rowReserve.WORKTIME
    '            End If
    '            ' 2012/06/26 KN 西岡 【SERVICE_2】事前準備対応 END

    '            ' R/O作成画面へ遷移
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'Me.RedirectOrderNew(visitNo.Value, _
    '            '                    staffInfo.DlrCD, _
    '            '                    rowReserve.VCLREGNO, _
    '            '                    rowReserve.VIN, _
    '            '                    rowReserve.MODELCODE, _
    '            '                    rowReserve.CUSTOMERNAME, _
    '            '                    rowReserve.TELNO, _
    '            '                    rowReserve.MOBILE, _
    '            '                    reserveId, staffInfo.BrnCD, _
    '            '                    rowReserve.ACCOUNT_PLAN, _
    '            '                    rowReserve.WASHFLG, _
    '            '                    "1", _
    '            '                    rowReserve.ORDERNO, _
    '            '                    Me.ChengeCustomerType(rowReserve.CUSTOMERFLAG), _
    '            '                    stockTime)
    '            Me.RedirectOrderNew(visitNo.Value, _
    '                                staffInfo.DlrCD, _
    '                                rowReserve.VCLREGNO, _
    '                                rowReserve.VIN, _
    '                                rowReserve.MODELCODE, _
    '                                rowReserve.CUSTOMERNAME, _
    '                                rowReserve.TELNO, _
    '                                rowReserve.MOBILE, _
    '                                reserveId, staffInfo.BrnCD, _
    '                                rowReserve.ACCOUNT_PLAN, _
    '                                rowReserve.WASHFLG, _
    '                                "1", _
    '                                rowReserve.ORDERNO, _
    '                                stockTime)
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        Else
    '            ' 事前準備エリア以外
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START

    '            If Not (staffInfo.Account.Equals(rowVisitData.SACODE)) Then
    '                '担当SAが自分ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If Not (Me.CheckCustomerType(rowVisitData.CUSTSEGMENT,
    '                                         TBL_SERVICE_VISIT_MANAGEMENT)) Then
    '                '顧客が自社客ではない
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If

    '            If String.IsNullOrEmpty(rowVisitData.ORDERNO) Then
    '                '整備受注Noが未採番
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return
    '            End If
    '            ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '            ' R/O作成画面へ遷移
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            'Me.RedirectOrderNew(rowVisitData.VISITSEQ, _
    '            '                    staffInfo.DlrCD, _
    '            '                    rowVisitData.VCLREGNO, _
    '            '                    rowVisitData.VIN, _
    '            '                    rowVisitData.MODELCODE, _
    '            '                    rowVisitData.CUSTOMERNAME, _
    '            '                    rowVisitData.TELNO, _
    '            '                    rowVisitData.MOBILE, _
    '            '                    rowVisitData.FREZID, _
    '            '                    staffInfo.BrnCD, _
    '            '                    rowVisitData.SACODE, _
    '            '                    rowVisitData.WASHFLG, _
    '            '                    "0", _
    '            '                    rowVisitData.ORDERNO, _
    '            '                    rowVisitData.CUSTSEGMENT, _
    '            '                    rowVisitData.ASSIGNTIMESTAMP)
    '            Me.RedirectOrderNew(rowVisitData.VISITSEQ, _
    '                                staffInfo.DlrCD, _
    '                                rowVisitData.VCLREGNO, _
    '                                rowVisitData.VIN, _
    '                                rowVisitData.MODELCODE, _
    '                                rowVisitData.CUSTOMERNAME, _
    '                                rowVisitData.TELNO, _
    '                                rowVisitData.MOBILE, _
    '                                rowVisitData.FREZID, _
    '                                staffInfo.BrnCD, _
    '                                rowVisitData.SACODE, _
    '                                rowVisitData.WASHFLG, _
    '                                "0", _
    '                                rowVisitData.ORDERNO, _
    '                                rowVisitData.ASSIGNTIMESTAMP)
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub


    '    ' 2012/06/11 日比野 事前準備対応 START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    '    ''' </summary>
    '    ''' <param name="time">対象時間</param>
    '    ''' <returns>変換値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetDateTimeToString(ByVal time As DateTime) As String

    '        Dim strResult As String

    '        ' 日付チェック
    '        If time.Equals(DateTime.MinValue) Then
    '            Return String.Empty
    '        End If

    '        ' 時間範囲チェック
    '        If Me.nowDateTime.ToString("yyyyMMdd", CultureInfo.CurrentCulture) _
    '            .Equals(time.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then
    '            ' 当日 (hh:mm)
    '            strResult = DateTimeFunc.FormatDate(14, time)
    '        Else
    '            ' 上記以外 (mm/dd)
    '            strResult = DateTimeFunc.FormatDate(11, time)
    '        End If

    '        Return strResult

    '    End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    '    ''' </summary>
    '    ''' <param name="time">対象時間</param>
    '    ''' <returns>変換値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetDateStringToString(ByVal time As String) As String

    '        ' 空白チェック
    '        If String.IsNullOrEmpty(time) Then
    '            Return String.Empty
    '        End If

    '        ' 日付チェック
    '        Dim result As DateTime
    '        If Not DateTime.TryParse(time, result) Then
    '            Return String.Empty
    '        End If
    '        If result.Equals(DateTime.MinValue) Then
    '            Return String.Empty
    '        End If

    '        Return SetDateTimeToString(result)

    '    End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 文字列変換
    '    ''' </summary>
    '    ''' <param name="str"></param>
    '    ''' <returns>変換値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetNullToString(ByVal str As String, Optional ByVal strNull As String = "") As String

    '        ' 空白チェック
    '        If String.IsNullOrEmpty(str) Then
    '            Return strNull
    '        End If
    '        If String.IsNullOrEmpty(str.Trim()) Then
    '            Return strNull
    '        End If

    '        Return str

    '    End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 数値変換
    '    ''' </summary>
    '    ''' <param name="num"></param>
    '    ''' <returns>変換値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetNullToLong(ByVal num As String, Optional ByVal lngNull As Long = 0) As Long

    '        Dim result As Long
    '        If Not Long.TryParse(num, result) Then
    '            result = lngNull
    '        End If
    '        If result = 0 Then
    '            result = lngNull
    '        End If

    '        Return result

    '    End Function

    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' 時間チェック
    '    ' ''' </summary>
    '    ' ''' <param name="time">対象時間</param>
    '    ' ''' <returns></returns>
    '    ' ''' <remarks>True: Null(MinValue), false: Null(MinValue)以外</remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Function IsDateTimeNull(ByVal time As DateTime) As Boolean

    '    '    ' 日付チェック
    '    '    If time.Equals(DateTime.MinValue) Then
    '    '        Return True
    '    '    End If

    '    '    Return False

    '    'End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 固定文字列付与「～」
    '    ''' </summary>
    '    ''' <param name="appendTime">付与対象文字列</param>
    '    ''' <param name="ChipArea">工程管理エリア</param>
    '    ''' <returns>固定文字列付与値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetTimeFromToAppend(ByVal appendTime As String, ByVal chipArea As ChipArea) As String

    '        ' 空白チェック
    '        If String.IsNullOrEmpty(appendTime) Then
    '            Return String.Empty
    '        End If

    '        ' 工程管理エリア確認
    '        Dim rtnVal As StringBuilder = New StringBuilder
    '        With rtnVal
    '            ' 工程管理エリア確認
    '            Select Case chipArea
    '                Case chipArea.Reception
    '                    ' 受付エリア

    '                    ' XXX～
    '                    .Append(appendTime)
    '                    .Append(wordFixedString)
    '                Case chipArea.Approval,
    '                     chipArea.Preparation,
    '                     chipArea.Delivery,
    '                     chipArea.Work
    '                    ' 追加承認エリア、納車準備エリア、納車作業エリア、作業エリア

    '                    ' ～XXX
    '                    .Append(wordFixedString)
    '                    .Append(appendTime)
    '                Case Else
    '                    .Append(appendTime)
    '            End Select
    '        End With

    '        Return rtnVal.ToString

    '    End Function

    '    ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 事前準備エリアチップ初期設定
    '    ''' </summary>
    '    ''' <param name="dt">チップ情報</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitAdvancePreparations(ByVal dt As SC3140103AdvancePreparationsDataTable,
    '                                        ByVal strRightIcnD As String,
    '                                        ByVal strRightIcnI As String,
    '                                        ByVal strRightIcnS As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim columnsList As New List(Of List(Of SC3140103AdvancePreparationsRow))
    '        Dim rowList As New List(Of SC3140103AdvancePreparationsRow)
    '        Dim j As Integer = 0
    '        ' 事前準備チップのデータセットをリストに設定、さらに4件ずつ別のリストに設定する
    '        For Each rowIFAdvancePreparations As SC3140103DataSet.SC3140103AdvancePreparationsRow In _
    '        dt.Select(String.Format(CultureInfo.CurrentCulture, "SASTATUSFLG = '{0}'", "1"), "REZ_PICK_DATE")
    '            If j Mod 4 = 0 Then
    '                rowList = New List(Of SC3140103AdvancePreparationsRow)
    '                columnsList.Add(rowList)
    '            End If
    '            rowList.Add(rowIFAdvancePreparations)
    '            j = j + 1
    '        Next

    '        ' コントロールにバインドする
    '        Me.Repeater1.DataSource = columnsList
    '        Me.Repeater1.DataBind()

    '        Dim advancePreparations As Control
    '        Dim advancePreparationsBox As Control
    '        Dim rowBox As List(Of SC3140103DataSet.SC3140103AdvancePreparationsRow)
    '        Dim row As SC3140103DataSet.SC3140103AdvancePreparationsRow

    '        Dim strRegistrationNumber As String
    '        Dim strCustomerName As String
    '        Dim strRepresentativeWarehousing As String
    '        Dim strParkingNumber As String = ""

    '        Dim strReserveMark As String = "1"
    '        Dim strJDPMark As String
    '        Dim strSSCMark As String

    '        Dim divDeskDevice As HtmlContainerControl
    '        Dim deliveryPlanTime As HtmlContainerControl

    '        Dim divApoint As HtmlContainerControl

    '        Dim strTodayFlag As String
    '        Dim strDeliveryPlanTime As String

    '        ' -----------------------------------------------------
    '        ' データを設定する
    '        ' -----------------------------------------------------
    '        For i = 0 To Repeater1.Items.Count - 1
    '            '4チップセットのリストを取得する
    '            advancePreparationsBox = Repeater1.Items(i)
    '            rowBox = columnsList(i)
    '            ' 4チップセットのリストをバインド
    '            Dim rowListRepeater As Repeater = CType(advancePreparationsBox.FindControl("AdvancePreparationsRepeater"), Repeater)
    '            rowListRepeater.DataSource = rowBox
    '            rowListRepeater.DataBind()
    '            ' 4チップセットからチップ情報を取得する
    '            For k = 0 To rowListRepeater.Items.Count - 1

    '                advancePreparations = rowListRepeater.Items(k)
    '                row = rowBox(k)

    '                strRegistrationNumber = row.VCLREGNO
    '                strCustomerName = row.CUSTOMERNAME
    '                strTodayFlag = row.TODAYFLG
    '                ' 納車予定時刻は、取得データによって表示形式を切り替える
    '                strDeliveryPlanTime = Me.SetTimeOrDateToString(row.REZ_DELI_DATE.ToString(CultureInfo.CurrentCulture),
    '                                                               row.REZ_PICK_DATE.ToString(CultureInfo.CurrentCulture))
    '                strRepresentativeWarehousing = row.MERCHANDISENAME

    '                strJDPMark = row.JDP_MARK
    '                strSSCMark = row.SSC_MARK

    '                ' チップ内の文字列表示設定
    '                CType(advancePreparations.FindControl("AdvancePreparationsRegistrationNumber"), HtmlContainerControl) _
    '                    .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
    '                CType(advancePreparations.FindControl("AdvancePreparationsCustomerName"), HtmlContainerControl) _
    '                    .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
    '                CType(advancePreparations.FindControl("AdvancePreparationsDeliveryPlanTime"), HtmlContainerControl) _
    '                    .InnerHtml = strDeliveryPlanTime
    '                CType(advancePreparations.FindControl("AdvancePreparationsRepresentativeWarehousing"), HtmlContainerControl) _
    '                    .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)

    '                ' アイコンの表示設定
    '                If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
    '                    advancePreparations.FindControl("RightIcnD").Visible = True
    '                Else
    '                    advancePreparations.FindControl("RightIcnD").Visible = False
    '                End If

    '                If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
    '                    advancePreparations.FindControl("RightIcnI").Visible = True
    '                Else
    '                    advancePreparations.FindControl("RightIcnI").Visible = False
    '                End If

    '                If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
    '                    advancePreparations.FindControl("RightIcnS").Visible = True
    '                Else
    '                    advancePreparations.FindControl("RightIcnS").Visible = False
    '                End If

    '                'アイコンの文言設定
    '                CType(advancePreparations.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
    '                CType(advancePreparations.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
    '                CType(advancePreparations.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

    '                ' チップの内部保持属性の設定
    '                divDeskDevice = CType(advancePreparations.FindControl("AdvancePreparationsDeskDevice"), HtmlContainerControl)
    '                With divDeskDevice
    '                    .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
    '                    .Attributes("orderNo") = row.ORDERNO
    '                    .Attributes("rezId") = row.REZID
    '                    .Attributes("class") = "ColumnContentsBoder"
    '                End With

    '                ' チップ内の納車予定時刻の文字色設定
    '                deliveryPlanTime = CType(advancePreparations.FindControl("AdvancePreparationsDeliveryPlanTime"), HtmlContainerControl)
    '                If strTodayFlag.Equals("1") Then
    '                    deliveryPlanTime.Style("color") = "#F00"
    '                Else
    '                    deliveryPlanTime.Style("color") = "#666666"
    '                End If

    '                divApoint = CType(advancePreparations.FindControl("AdvancePreparations"), HtmlContainerControl)
    '                With divApoint
    '                    '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '                    '.Style("top") = (114 * i).ToString(CultureInfo.CurrentCulture) & "px"
    '                    .Style("top") = (104 * i).ToString(CultureInfo.CurrentCulture) & "px"
    '                    '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
    '                    .Style("left") = (160 * k).ToString(CultureInfo.CurrentCulture) & "px"
    '                End With
    '            Next

    '        Next

    '        Dim divFlickableBox As HtmlContainerControl
    '        divFlickableBox = CType(Me.flickableBox, HtmlContainerControl)
    '        divFlickableBox.Style("height") = (114 * Repeater1.Items.Count).ToString(CultureInfo.CurrentCulture) & "px"
    '        divFlickableBox.Style("height") = (106 * Repeater1.Items.Count).ToString(CultureInfo.CurrentCulture) & "px"

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END

    '    ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 START

    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' チップ詳細情報表示
    '    ' ''' </summary>
    '    ' ''' <remarks>
    '    ' ''' チップ詳細画面に表示する情報を取得し、設定します。
    '    ' ''' </remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Sub InitAdvancePreparationsChipDetail()

    '    '    'ログイン情報管理機能（ログイン情報取得）
    '    '    Dim staffInfo As StaffContext = StaffContext.Current

    '    '    ' サービス来店者情報取得(チップ詳細)
    '    '    Dim rezId As Long = SetNullToLong(Me.DetailsRezId.Value)

    '    '    Dim ChipDetails As ChipDetail

    '    '    Using CommonClass As New SMBCommonClassBusinessLogic
    '    '        ' チップ詳細情報取得
    '    '        ChipDetails = CommonClass.GetChipDetailReserve(staffInfo.DlrCD, staffInfo.BrnCD, rezId)
    '    '    End Using

    '    '    ' チップ詳細情報が取得できた場合の画面表示設定
    '    '    If ChipDetails IsNot Nothing Then
    '    '        ' 事前準備の場合予約アイコンは常に表示
    '    '        Me.DetailsRightIconD.Visible = True
    '    '        ' その他アイコンの表示設定
    '    '        If "1".Equals(ChipDetails.JdpType) Then
    '    '            Me.DetailsRightIconI.Visible = True
    '    '        Else
    '    '            Me.DetailsRightIconI.Visible = False
    '    '        End If

    '    '        If "1".Equals(ChipDetails.SscType) Then
    '    '            Me.DetailsRightIconS.Visible = True
    '    '        Else
    '    '            Me.DetailsRightIconS.Visible = False
    '    '        End If

    '    '        ' 登録番号
    '    '        Me.DetailsRegistrationNumber.Text = ChipDetails.VehicleRegNo
    '    '        ' 車種
    '    '        Me.DetailsCarModel.Text = ChipDetails.VehicleName
    '    '        ' グレード
    '    '        Me.DetailsModel.Text = ChipDetails.Grade
    '    '        ' VIN
    '    '        'Me.DetailsVin.Text = ChipDetails.Vin
    '    '        ' 走行距離
    '    '        'If ChipDetails.Mileage >= 0 Then
    '    '        'Me.DetailsMileage.Text = String.Format(CultureInfo.CurrentCulture, "{0:#,0}", ChipDetails.Mileage)
    '    '        'Else
    '    '        'Me.DetailsMileage.Text = String.Empty
    '    '        'End If
    '    '        ' 納車日
    '    '        'If Me.IsDateTimeNull(ChipDetails.DeliveryDate) Then
    '    '        'Me.DetailsDeliveryCarDay.Text = String.Empty
    '    '        'Else
    '    '        'Me.DetailsDeliveryCarDay.Text = DateTimeFunc.FormatDate(21, ChipDetails.DeliveryDate)
    '    '        'End If

    '    '        ' 顧客名
    '    '        Me.DetailsCustomerName.Text = Me.SetNullToString(ChipDetails.CustomerName)
    '    '        ' 電話番号
    '    '        Me.DetailsPhoneNumber.Text = Me.SetNullToString(ChipDetails.TelNo)
    '    '        ' 携帯番号
    '    '        Me.DetailsMobileNumber.Text = Me.SetNullToString(ChipDetails.Mobile)
    '    '        ' 代表入庫項目
    '    '        'Me.DetailsRepresentativeWarehousing.Text = Me.SetNullToString(ChipDetails.MerchandiseName)

    '    '        ' 表示区分
    '    '        ' 来店時刻
    '    '        'Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 36)
    '    '        'Me.DetailsVisitTime.Text = Me.SetDateTimeToString(ChipDetails.VisitReserveDate)

    '    '        Dim wkCustomerType As String
    '    '        ' ストール予約テーブル(識別フラグ 0:自社客 1:未取引客)を変換
    '    '        If ChipDetails.CustomerType.Equals("0") Then
    '    '            '自社客
    '    '            wkCustomerType = "1"
    '    '        Else
    '    '            '未取引客
    '    '            wkCustomerType = "2"
    '    '        End If

    '    '        Dim bis As New SC3140103BusinessLogic

    '    '        bis.CheckChipDetailButton(ChipDetails.CustomerType, Me.DetailsArea.Value, "", "", "")

    '    '        ' ボタン表示
    '    '        Me.DetailButtonLeft.Text = Me.InitVisitChipDetailButton(bis.GetButtonLeft)
    '    '        Me.DetailButtonLeft.Enabled = bis.GetButtonEnabledLeft
    '    '        Me.DetailButtonRight.Text = Me.InitVisitChipDetailButton(bis.GetButtonRight)
    '    '        Me.DetailButtonRight.Enabled = bis.GetButtonEnabledRight

    '    '    End If

    '    'End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 固定文字列付与「～」
    '    ''' </summary>
    '    ''' <param name="appendTakeTime">付与対象引取時間文字列</param>
    '    ''' <param name="appendVisitTime">付与対象納車時間文字列</param>
    '    ''' <returns>固定文字列付与値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetTimeFromToAppendTimes(ByVal appendTakeTime As String,
    '                                              ByVal appendVisitTime As String) As String

    '        ' 空白チェック
    '        If String.IsNullOrEmpty(appendTakeTime) Or String.IsNullOrEmpty(appendVisitTime) Then
    '            Return String.Empty
    '        End If

    '        ' 表示する文字列の設定
    '        Dim rtnVal As StringBuilder = New StringBuilder
    '        With rtnVal
    '            .Append(appendTakeTime)
    '            .Append(wordFixedString)
    '            .Append(appendVisitTime)
    '        End With

    '        Return rtnVal.ToString

    '    End Function


    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 納車時間文字列の表示設定（日付と時刻）
    '    ''' </summary>
    '    ''' <param name="TakingTime">付与対象引取時刻文字列</param>
    '    ''' <param name="VisitTime">付与対象納車時刻文字列</param>
    '    ''' <returns>納車予定時刻文字列</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetTimeOrDateToString(ByVal VisitTime As String,
    '                                           ByVal TakingTime As String) As String
    '        ' 空白チェック
    '        If String.IsNullOrEmpty(VisitTime) Or String.IsNullOrEmpty(TakingTime) Then
    '            Return String.Empty
    '        End If
    '        ' Date型にフォーマット
    '        Dim TimeFromDate As Date = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, TakingTime)
    '        Dim TimeToDate As Date = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, VisitTime)
    '        Dim strResult As String
    '        ' 時刻の文字列に切り分け
    '        Dim strFromTime As String = DateTimeFunc.FormatDate(14, TimeFromDate)
    '        Dim strToTime As String
    '        ' 日付の文字列に切り分け
    '        Dim strFromDate As String = DateTimeFunc.FormatDate(11, TimeFromDate)
    '        Dim strToDate As String = DateTimeFunc.FormatDate(11, TimeToDate)
    '        ' 日付が同一の場合、時刻表示、異なる場合日付表示を設定
    '        If strFromDate.Equals(strToDate) Then
    '            strToTime = DateTimeFunc.FormatDate(14, TimeToDate)
    '            strResult = SetTimeFromToAppendTimes(strFromTime, strToTime)
    '        Else
    '            strResult = SetTimeFromToAppendTimes(strFromTime, strToDate)
    '        End If

    '        Return strResult

    '    End Function


    '    ' 2012/06/13 KN 西岡 【SERVICE_2】事前準備対応 END
    '    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 顧客情報の検索処理
    '    ''' </summary>
    '    ''' <param name="dealerCode">顧客コード</param>
    '    ''' <param name="storeCode">販売店コード</param>
    '    '''-----------------------------------------------------------------------
    '    Private Sub GetCustomerInfomation(ByVal dealerCode As String,
    '                                      ByVal storeCode As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:dealerCode = {3}, storeCode = {4}," _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , dealerCode _
    '                                 , storeCode))

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim searchRegistrationNumber As String = Me.SearchRegistrationNumberHidden.Value
    '        Dim searchVin As String = Me.SearchVinHidden.Value
    '        Dim searchCustomerName As String = Me.SearchCustomerNameHidden.Value
    '        Dim searchPhoneNumber As String = Me.SearchPhoneNumberHidden.Value
    '        Dim searchStartRow As Long = CType(Me.SearchStartRowHidden.Value, Long)
    '        Dim searchEndRow As Long = CType(Me.SearchEndRowHidden.Value, Long)
    '        Dim searchSelectType As Long = CType(Me.SearchSelectTypeHidden.Value, Long)

    '        Dim divFrontLink As HtmlContainerControl
    '        divFrontLink = CType(Me.FrontLink, HtmlContainerControl)
    '        Dim divNextLink As HtmlContainerControl
    '        divNextLink = CType(Me.NextLink, HtmlContainerControl)
    '        Dim divSearchList As HtmlContainerControl = CType(Me.NoSearchImage, HtmlContainerControl)
    '        divSearchList.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 71)

    '        If 0 < searchRegistrationNumber.Length Then
    '            If Not Validation.IsValidString(searchRegistrationNumber) Then
    '                divFrontLink.Style("display") = "none"
    '                divNextLink.Style("display") = "none"
    '                divSearchList.Style("display") = "none"
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                ' メッセージの表示
    '                Me.ShowMessageBox(MsgID.id906)
    '                Return
    '            End If
    '        ElseIf 0 < searchVin.Length Then
    '            If Not Validation.IsValidString(searchVin) Then
    '                divFrontLink.Style("display") = "none"
    '                divNextLink.Style("display") = "none"
    '                divSearchList.Style("display") = "none"
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                ' メッセージの表示
    '                Me.ShowMessageBox(MsgID.id906)
    '                Return
    '            End If
    '        ElseIf 0 < searchCustomerName.Length Then
    '            If Not Validation.IsValidString(searchCustomerName) Then
    '                divFrontLink.Style("display") = "none"
    '                divNextLink.Style("display") = "none"
    '                divSearchList.Style("display") = "none"
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                ' メッセージの表示
    '                Me.ShowMessageBox(MsgID.id906)
    '                Return
    '            End If
    '        ElseIf 0 < searchPhoneNumber.Length Then
    '            If Not Validation.IsValidString(searchPhoneNumber) Then
    '                divFrontLink.Style("display") = "none"
    '                divNextLink.Style("display") = "none"
    '                divSearchList.Style("display") = "none"
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                ' メッセージの表示
    '                Me.ShowMessageBox(MsgID.id906)
    '                Return
    '            End If
    '        Else
    '            divFrontLink.Style("display") = "none"
    '            divNextLink.Style("display") = "none"
    '            divSearchList.Style("display") = "none"
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '            ' メッセージの表示
    '            Me.ShowMessageBox(MsgID.id905)
    '            Return
    '        End If

    '        Dim result As SC3140103SearchResult

    '        Using bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()
    '            result = bl.GetCustomerList(dealerCode, _
    '                                        storeCode, _
    '                                        searchRegistrationNumber, _
    '                                        searchVin, _
    '                                        searchCustomerName, _
    '                                        searchPhoneNumber, _
    '                                        searchStartRow, _
    '                                        searchEndRow, _
    '                                        searchSelectType)
    '        End Using


    '        ' コントロールにバインドする
    '        Me.SearchRepeater.DataSource = result.DataTable.Select()
    '        Me.SearchRepeater.DataBind()

    '        ' 顧客情報取得時
    '        If result.DataTable IsNot Nothing AndAlso 0 < result.DataTable.Count Then

    '            Dim searchData As Control
    '            Dim row As SC3140103DataSet.SC3140103VisitSearchResultRow

    '            Dim rowList As SC3140103DataSet.SC3140103VisitSearchResultRow() = _
    '                DirectCast(Me.SearchRepeater.DataSource, SC3140103DataSet.SC3140103VisitSearchResultRow())

    '            Dim vehicleName As String
    '            Dim grade As String
    '            Dim CustomerChangeParameterDiv As HtmlContainerControl

    '            For i = 0 To SearchRepeater.Items.Count - 1

    '                searchData = SearchRepeater.Items(i)
    '                row = rowList(i)

    '                vehicleName = row.VEHICLENAME                           ' 車種
    '                grade = row.GRADE                                       ' グレード

    '                ' 画像
    '                CType(searchData.FindControl("SearchPhotoImage"), HtmlImage).Attributes("src") = Me.ResolveClientUrl(row.IMAGEFILE)
    '                ' 車両登録No
    '                CType(searchData.FindControl("SearchRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.VCLREGNO)
    '                ' VIN
    '                CType(searchData.FindControl("SearchVinNo"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.VIN)
    '                ' 顧客名称
    '                CType(searchData.FindControl("SearchCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.CUSTOMERNAME)
    '                ' 車種＋グレード
    '                CType(searchData.FindControl("SearchModel"), HtmlContainerControl).InnerHtml = Me.SetConnectString(vehicleName, grade)
    '                ' 電話番号
    '                CType(searchData.FindControl("SearchPhone"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.TELNO)
    '                ' 携帯電話番号
    '                CType(searchData.FindControl("SearchMobile"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.MOBILE)
    '                ' 付替え用データの設定
    '                CustomerChangeParameterDiv = DirectCast(searchData.FindControl("CustomerChangeParameter"), HtmlContainerControl)
    '                With CustomerChangeParameterDiv
    '                    .Attributes("CustomerCodeParameter") = Me.SetNullToString(row.CUSTOMERCODE) ' 顧客コード
    '                    .Attributes("DmsIdParameter") = Me.SetNullToString(row.DMSID)               ' 基幹顧客ID
    '                    .Attributes("ModelParameter") = Me.SetNullToString(row.MODEL)               ' モデル
    '                    .Attributes("SacodeParameter") = Me.SetNullToString(row.SACODE)             ' SAコード
    '                End With
    '            Next

    '            ' 次の表示件数，および前の表示件数の設定
    '            Dim customerCount As Long = result.ResultCustomerCount
    '            Dim resultStartRow As Long = result.ResultStartRow
    '            Dim resultEndRow As Long = result.ResultEndRow
    '            Dim standardCount As Long = result.StandardCount
    '            Me.SetOtherDisplay(resultStartRow, resultEndRow, customerCount, standardCount)
    '            divSearchList.Style("display") = "none"

    '            ' スクロール設定
    '            Dim differenceRow As Long = 0
    '            If 0 < searchSelectType Then
    '                ' 前回終了位置と今回開始位置の差分を求める
    '                Dim beforeEndRow As Long = searchEndRow - 4
    '                differenceRow = beforeEndRow - resultStartRow + 1
    '            ElseIf searchSelectType < 0 Then
    '                ' 前回開始位置と今回開始位置の差分を求める
    '                differenceRow = searchStartRow - resultStartRow - 1
    '            End If
    '            If 1 < resultStartRow Then
    '                ' 今回開始位置が1行目以降の場合、前のN件表示分、行が加算される
    '                Me.ScrollPositionHidden.Value = _
    '                    ((differenceRow + 1) * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)
    '            Else
    '                Me.ScrollPositionHidden.Value = _
    '                    (differenceRow * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)
    '            End If

    '        Else
    '            divSearchList.Style("display") = "block"
    '            divFrontLink.Style("display") = "none"
    '            divNextLink.Style("display") = "none"
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 次回表示件数の設定（次のN件，前のN件）
    '    ''' </summary>
    '    ''' <param name="startRow">開始行番号</param>
    '    ''' <param name="endRow">終了行番号</param>
    '    ''' <param name="customerCount">顧客検索件数</param>
    '    ''' <param name="standardCount">標準取得件数</param>
    '    '''-----------------------------------------------------------------------
    '    Private Sub SetOtherDisplay(ByVal startRow As Long, _
    '                                ByVal endRow As Long, _
    '                                ByVal customerCount As Long, _
    '                                ByVal standardCount As Long)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:startRow = {3}, endRow = {4}," & _
    '                                   " customerCount = {5}, standardCount = {6}," _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , CType(startRow, String) _
    '                                 , CType(endRow, String) _
    '                                 , CType(customerCount, String) _
    '                                 , CType(standardCount, String)))

    '        If 0 < customerCount Then
    '            ' 前の件数検索表示の設定
    '            Dim displayFront As String
    '            Dim divFrontLink As HtmlContainerControl
    '            Dim divFrontList As HtmlContainerControl
    '            Dim divFrontListSearching As HtmlContainerControl
    '            divFrontLink = CType(Me.FrontLink, HtmlContainerControl)
    '            divFrontList = CType(Me.FrontList, HtmlContainerControl)
    '            divFrontListSearching = CType(Me.FrontListSearching, HtmlContainerControl)
    '            If 1 < startRow Then
    '                If startRow <= standardCount Then
    '                    displayFront = (startRow - 1).ToString(CultureInfo.CurrentCulture)
    '                Else
    '                    displayFront = standardCount.ToString(CultureInfo.CurrentCulture)
    '                End If
    '                divFrontList.InnerHtml = _
    '                    WebWordUtility.GetWord(APPLICATIONID, 72).Replace("{0}", CType(displayFront, String))
    '                divFrontListSearching.InnerHtml = _
    '                    WebWordUtility.GetWord(APPLICATIONID, 73).Replace("{0}", CType(displayFront, String))
    '                divFrontLink.Style("display") = "block"
    '            Else
    '                divFrontLink.Style("display") = "none"
    '            End If

    '            ' 次の件数検索表示の設定
    '            Dim displayNext As String
    '            Dim divNextLink As HtmlContainerControl
    '            Dim divNextList As HtmlContainerControl
    '            Dim divNextListSearching As HtmlContainerControl
    '            divNextLink = CType(Me.NextLink, HtmlContainerControl)
    '            divNextList = CType(Me.NextList, HtmlContainerControl)
    '            divNextListSearching = CType(Me.NextListSearching, HtmlContainerControl)
    '            If endRow < customerCount Then
    '                Dim differenceEndRow As Long = customerCount - endRow
    '                If differenceEndRow < standardCount Then
    '                    displayNext = CType(differenceEndRow, String)
    '                Else
    '                    displayNext = standardCount.ToString(CultureInfo.CurrentCulture)
    '                End If
    '                divNextList.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 74) _
    '                .Replace("{0}", CType(displayNext, String))
    '                divNextListSearching.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 75) _
    '                .Replace("{0}", CType(displayNext, String))

    '                divNextLink.Style("display") = "block"
    '            Else
    '                divNextLink.Style("display") = "none"
    '            End If
    '            ' 開始行，終了行を記憶する
    '            Me.SearchStartRowHidden.Value = startRow.ToString(CultureInfo.CurrentCulture)
    '            Me.SearchEndRowHidden.Value = endRow.ToString(CultureInfo.CurrentCulture)

    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 車種，グレードの文字列の結合
    '    ''' </summary>
    '    ''' <param name="firstWord">開始行番号</param>
    '    ''' <param name="secondWord">終了行番号</param>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetConnectString(ByVal firstWord As String, _
    '                                      ByVal secondWord As String) As String

    '        Dim setFirst As String = SetNullToString(firstWord)
    '        Dim setSecond As String = SetNullToString(secondWord)

    '        Dim resultWord As String = setFirst & "  " & setSecond

    '        Return resultWord
    '    End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 付替え前確認結果のHiddenFieldへの設定
    '    ''' </summary>
    '    ''' <param name="row">付替え前確認結果</param>
    '    '''-----------------------------------------------------------------------
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Sub SetHiddenStatus(ByVal row As SC3140103DataSet.SC3140103BeforeChangeCheckResultRow)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        If Not row.IsAFTERVISITNONull Then
    '            Me.ChipVisitNumberChange.Value = CType(row.AFTERVISITNO, String)
    '        Else
    '            Me.ChipVisitNumberChange.Value = String.Empty
    '        End If
    '        If Not row.IsAFTERRESERVENONull Then
    '            Me.ChipReserveNumberChange.Value = CType(row.AFTERRESERVENO, String)
    '        Else
    '            Me.ChipReserveNumberChange.Value = String.Empty
    '        End If
    '        If Not row.IsAFTERORDERNONull Then
    '            Me.ChipOrderNumberChange.Value = row.AFTERORDERNO
    '        Else
    '            Me.ChipOrderNumberChange.Value = String.Empty
    '        End If
    '        If Not row.IsAFTERSACODENull Then
    '            Me.ChipSACodeChange.Value = row.AFTERSACODE
    '        Else
    '            Me.ChipSACodeChange.Value = String.Empty
    '        End If
    '        If Not row.IsBEFOREORDERNONull Then
    '            Me.ChipOrderNumberBefore.Value = row.BEFOREORDERNO
    '        Else
    '            Me.ChipOrderNumberBefore.Value = String.Empty
    '        End If
    '        If Not row.IsBEFORERESERVENONull Then
    '            Me.ChipReserveNumberBefore.Value = CType(row.BEFORERESERVENO, String)
    '        Else
    '            Me.ChipReserveNumberBefore.Value = String.Empty
    '        End If

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        '車両IDをHiddenFiledに設定する
    '        If Not row.IsAFTERVCLIDNull AndAlso 0 < row.AFTERVCLID Then
    '            Me.ChipVehicleIdAfter.Value = CType(row.AFTERVCLID, String)
    '        Else
    '            Me.ChipVehicleIdAfter.Value = String.Empty
    '        End If

    '        '顧客IDをHiddenFiledより取得
    '        Dim customerCode As String = SetNullToString(Me.SearchCustomerCodeChange.Value)

    '        '顧客IDをHiddenFiledに設定する
    '        '顧客IDが取得できた場合のみ
    '        If Not row.IsAFTERCSTIDNull AndAlso 0 < row.AFTERCSTID Then

    '            Me.SearchCustomerCodeChange.Value = CType(row.AFTERCSTID, String)
    '        End If

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '    ' ''' <summary>
    '    ' ''' 実績ステータスに該当する文言を取得する
    '    ' ''' </summary>
    '    ' ''' <param name="ResultStatus">実績ステータス</param>
    '    ' ''' <returns></returns>
    '    ' ''' <remarks></remarks>
    '    'Private Function GetResultStatusWord(ByVal resultStatus As String) As String

    '    '    '開始ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '                             , "{0}.{1} {2} resultStatus = {3}" _
    '    '                             , Me.GetType.ToString _
    '    '                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '                             , LOG_START _
    '    '                             , resultStatus))

    '    '    Dim returnWord As String = ""

    '    '    Select Case (resultStatus)
    '    '        Case "30"
    '    '            returnWord = WebWordUtility.GetWord(APPLICATIONID, 46)
    '    '        Case "31"
    '    '            returnWord = WebWordUtility.GetWord(APPLICATIONID, 47)
    '    '        Case "38"
    '    '            returnWord = WebWordUtility.GetWord(APPLICATIONID, 48)
    '    '        Case "39"
    '    '            returnWord = WebWordUtility.GetWord(APPLICATIONID, 49)
    '    '        Case Else
    '    '            returnWord = ""
    '    '    End Select

    '    '    '終了ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '               , "{0}.{1} {2}" _
    '    '               , Me.GetType.ToString _
    '    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '               , LOG_END))

    '    '    Return returnWord
    '    'End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    ''' <summary>
    '    ''' ステータスコードに該当するボタン活性状態を設定する
    '    ''' </summary>
    '    ''' <param name="statusCode">ステータスコード</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    Private Sub SetSubMenuButton(ByVal statusCode As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:statusCode = {3}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , statusCode))

    '        Select Case (statusCode)
    '            Case StatusCodeLeft101
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 52)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft102
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 53)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft103
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 54)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft104
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 52)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft105
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 53)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft106
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 54)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft107
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft108
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft109
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft110
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft111
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft112
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft113
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft114
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft115
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft116
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft117
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 55)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft118
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 55)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft119
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 55)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft120
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 55)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeLeft121
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 56)
    '                Me.DetailBottomBox.Style("display") = "block"
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '                'Case StatusCodeLeft122
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '                'Case StatusCodeLeft123
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '                'Case StatusCodeLeft124
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '                'Case StatusCodeLeft125
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '                'Case StatusCodeLeft126
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "1"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '                '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '            Case StatusCodeLeft199
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                'Case Else
    '                '    Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                '    Me.HiddenDetailsROButtonStatus.Value = "0"
    '                '    Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                '    Me.DetailbottomButton.Text = ""
    '                '    Me.DetailBottomBox.Style("display") = "none"
    '            Case Else
    '                SetSubMenuButtonElse(statusCode)
    '                '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        End Select

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '    ''' <summary>
    '    ''' ステータスコードに該当するボタン活性状態を設定する
    '    ''' </summary>
    '    ''' <param name="statusCode">ステータスコード</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetSubMenuButtonElse(ByVal statusCode As String)

    '        Select Case statusCode
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '            Case StatusCodeLeft122
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft123
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft124
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft125
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeLeft126
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '                ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '            Case Else
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '        End Select

    '    End Sub
    '    '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '    ''' <summary>
    '    ''' 追加作業エリアチップにおける、
    '    ''' ステータスコードに該当するボタン活性状態を設定する
    '    ''' </summary>
    '    ''' <param name="statusCode">ステータスコード</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetSubMenuButtonApproval(ByVal statusCode As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:statusCode = {3}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , statusCode))

    '        Select Case (statusCode)
    '            Case StatusCodeRight201
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight202
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight203
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight205
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 58)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeRight206
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 57)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeRight207
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "1"
    '                Me.HiddenDetailsROButtonStatus.Value = "1"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "1"
    '                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 59)
    '                Me.DetailBottomBox.Style("display") = "block"
    '            Case StatusCodeRight208
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight209
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight210
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight211
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case StatusCodeRight299
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '            Case Else
    '                Me.HiddenDetailsCustomerButtonStatus.Value = "0"
    '                Me.HiddenDetailsROButtonStatus.Value = "0"
    '                Me.HiddenDetailsApprovalButtonStatus.Value = "0"
    '                Me.DetailbottomButton.Text = ""
    '                Me.DetailBottomBox.Style("display") = "none"
    '        End Select

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 時間変換 (hh:mm)
    '    ''' </summary>
    '    ''' <param name="time">対象時間</param>
    '    ''' <returns>変換値</returns>
    '    '''-----------------------------------------------------------------------
    '    Private Function SetDateTimeToStringDetail(ByVal time As DateTime, _
    '                                               ByVal inNowDate As Date) As String

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:time={3}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , time))

    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        Dim strResult As String

    '        ' 日付チェック
    '        If time.Equals(DateTime.MinValue) Then
    '            Return String.Empty
    '        End If

    '        Try
    '            If Not inNowDate.Date = time.Date Then
    '                '(MM/dd hh:mm)
    '                strResult = time.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture)
    '            Else
    '                ' (hh:mm)
    '                strResult = DateTimeFunc.FormatDate(14, time)
    '            End If

    '        Catch ex As FormatException
    '            strResult = String.Empty
    '        End Try

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Result:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , strResult))

    '        Return strResult

    '    End Function

    '    ' 2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '    '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 STRAT

    '    ''' <summary>
    '    ''' 完成検査承認ボタン活性設定
    '    ''' </summary>
    '    ''' <param name="inSstaffInfo">リターンコード</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    '    ''' </history>
    '    Private Sub SetInspectionButton(ByVal inSstaffInfo As StaffContext)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '        'Try
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '        '来店実績連番を取得
    '        Dim visitNo As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '        '結果
    '        Dim result As Boolean = False

    '        Using sc3140103Biz As New SC3140103BusinessLogic
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '            Try
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END   
    '                '来店情報を取得
    '                Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable = _
    '                    sc3140103Biz.GetVisitManager(visitNo)

    '                '来店情報取得結果を判定
    '                If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
    '                    'DateRowに変換
    '                    Dim rowVisitData As SC3140103DataSet.SC3140103ServiceVisitManagementRow = _
    '                        DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103ServiceVisitManagementRow)
    '                    '整備受注NOの判定
    '                    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '                    'If Not rowVisitData.IsORDERNONull _
    '                    'AndAlso Not String.Empty.Equals(rowVisitData.ORDERNO.Trim) Then
    '                    If Not rowVisitData.IsORDERNONull _
    '                        AndAlso Not String.IsNullOrEmpty(rowVisitData.ORDERNO.Trim) Then
    '                        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END    
    '                        '整備受注NOが存在する場合
    '                        'BMTSAPI(IC3802102)
    '                        Dim ic3802102Biz As New IC3802102BusinessLogic
    '                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "{0}.{1} CALL IC3802102 ValidateCondition ORDERNO = {2} DLRCD = {3} " _
    '                              , Me.GetType.ToString _
    '                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                              , rowVisitData.ORDERNO.Trim _
    '                              , inSstaffInfo.DlrCD))
    '                        '完成検査承認API
    '                        result = ic3802102Biz.ValidationCondition(rowVisitData.ORDERNO.Trim, _
    '                                                                  inSstaffInfo.DlrCD)

    '                    End If
    '                End If
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START    
    '                'End Using
    '                '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
    '                '結果判定
    '                If result Then
    '                    '完成検査承認ボタン活性
    '                    Me.HiddenDetailsInspectionButtonStatus.Value = InspectionButtonTypeOn
    '                Else
    '                    '完成検査承認ボタン非活性
    '                    Me.HiddenDetailsInspectionButtonStatus.Value = InspectionButtonTypeOff
    '                End If

    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                ''終了ログの出力
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} OracleExceptionEx ex.Number = 1013(DBTimeOut)" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

    '                'タイムアウトエラーの場合は、メッセージを表示する
    '                Me.ShowMessageBox(MsgID.id901)
    '            End Try
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START     
    '        End Using
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '    End Sub
    '    '2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '    ''' <summary>
    '    ''' 来店管理エリア設定
    '    ''' </summary>
    '    ''' <param name="inSelectChipArea">選択チップエリア</param>
    '    ''' <param name="inChipDetail">チップ詳細</param>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    '    ''' </History>
    '    Private Sub VisitAreaSet(ByVal inSelectChipArea As Long, _
    '                             ByVal inChipDetail As ChipDetail)


    '        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    '        If "1".Equals(Me.UseReception.Value) Then


    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    '            'If CType(ChipArea.Reception, Long) = inSelectChipArea Then
    '            If CType(ChipArea.Reception, Long) = inSelectChipArea _
    '                OrElse CType(ChipArea.Assignment, Long) = inSelectChipArea Then

    '                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '                Me.VisitCustomer.Style("display") = "block"
    '                'DB排他制御ため、更新日時を格納する
    '                Me.DetailsVisitUpdateDate.Text = inChipDetail.UpdateDate.ToString(CultureInfo.CurrentCulture)
    '                '呼出No.
    '                Me.DetailsCallNo.Text = inChipDetail.CallNO
    '                '呼出場所
    '                Me.DetailsCallPlace.Text = inChipDetail.CallPlace
    '                '来店者氏名(空の場合、氏名をセットする)
    '                If Not String.IsNullOrEmpty(inChipDetail.VisitName) _
    '                AndAlso Not String.IsNullOrEmpty(inChipDetail.VisitName.Trim) Then
    '                    Me.DetailsVisitName.Text = inChipDetail.VisitName
    '                Else
    '                    Me.DetailsVisitName.Text = inChipDetail.CustomerName
    '                End If
    '                '来店者電話番号(空の場合、携帯番号（空の場合、電話番号）をセットする)
    '                If Not String.IsNullOrEmpty(inChipDetail.VisitTelNO) _
    '                    AndAlso Not String.IsNullOrEmpty(inChipDetail.VisitTelNO.Trim) Then
    '                    Me.DetailsVisitTelno.Text = inChipDetail.VisitTelNO
    '                ElseIf Not String.IsNullOrEmpty(inChipDetail.Mobile) _
    '                    AndAlso Not String.IsNullOrEmpty(inChipDetail.Mobile.Trim) Then
    '                    Me.DetailsVisitTelno.Text = inChipDetail.Mobile
    '                ElseIf Not String.IsNullOrEmpty(inChipDetail.TelNo) _
    '                    AndAlso Not String.IsNullOrEmpty(inChipDetail.TelNo.Trim) Then
    '                    Me.DetailsVisitTelno.Text = inChipDetail.TelNo
    '                Else
    '                    '上記以外の場合
    '                    '値無し(空)をセットする
    '                    Me.DetailsVisitTelno.Text = String.Empty
    '                End If
    '                '呼出ステータスによってボタンが変わる
    '                If inChipDetail.CallStatus = CALLSTATUS_CALLING Then
    '                    '呼出中
    '                    Me.BtnCALLCancel.Style("display") = "block"
    '                    Me.BtnCALL.Style("display") = "none"
    '                    Me.DetailsCallPlace.ReadOnly = True
    '                ElseIf inChipDetail.CallStatus = CALLSTATUS_NOTCALL Then
    '                    '未呼出
    '                    Me.BtnCALLCancel.Style("display") = "none"
    '                    Me.BtnCALL.Style("display") = "block"
    '                    Me.DetailsCallPlace.ReadOnly = False
    '                Else
    '                    '以外の場合
    '                    Me.BtnCALLCancel.Style("display") = "none"
    '                    Me.BtnCALL.Style("display") = "none"
    '                    Me.DetailsCallPlace.ReadOnly = True
    '                End If
    '            Else
    '                Me.DetailsVisitUpdateDate.Text = String.Empty
    '                Me.VisitCustomer.Style("display") = "none"
    '            End If

    '        End If

    '        Me.DetailsCallStatus.Value = inChipDetail.CallStatus
    '    End Sub

    '    ''' <summary>
    '    ''' 呼び出し完了処理
    '    ''' </summary>
    '    ''' <returns>resultCode</returns>
    '    ''' <remarks></remarks>
    '    Private Function CallCompleteOperation() As Long
    '        Dim resultCode As Long      '返却用
    '        '呼出ステータス
    '        Dim callStatus As String = Me.SetNullToString(Me.DetailsCallStatus.Value)
    '        If Not callStatus = CType(CALLSTATUS_CALLED, String) Then
    '            Dim staffInfo As StaffContext = StaffContext.Current
    '            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
    '            '現在日時取得
    '            Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)
    '            '更新日時取得
    '            Dim updateDate As Date = Date.MinValue
    '            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Text, updateDate) Then
    '                updateDate = Date.MinValue
    '            End If
    '            '担当SACODE
    '            Dim staffAccount As String = staffInfo.Account
    '            Using bl As New SC3140103BusinessLogic
    '                resultCode = bl.CallCompleted(visitSeq, staffAccount, nowDate, updateDate)
    '            End Using
    '            If resultCode <> 0 Then
    '                '戻り値エラー
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} CALL IC3802102 RETURNCODE = {2} " _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , resultCode))
    '                Select Case resultCode
    '                    Case RET_DBTIMEOUT
    '                        Me.ShowMessageBox(MsgID.id901)
    '                    Case RET_NOMATCH
    '                        Me.ShowMessageBox(MsgID.id902)
    '                    Case RET_EXCLUSION
    '                        Me.ShowMessageBox(MsgID.id903)
    '                End Select
    '            Else
    '                Me.DetailsVisitUpdateDate.Text = nowDate.ToString(CultureInfo.CurrentCulture)
    '            End If
    '        End If
    '        Return resultCode
    '    End Function

    '    ''' <summary>
    '    ''' 事前準備お客詳細
    '    ''' </summary>
    '    ''' <param name="dtReserve">受付情報</param>
    '    ''' <param name="dtVisit">来店情報</param>
    '    ''' <param name="rezID">予約ID</param>
    '    ''' <param name="staffInfo">ログイン情報</param>
    '    ''' <returns>処理結果</returns>
    '    ''' <remarks></remarks>
    '    Private Function AdvancePrepDetailCustomer(ByVal dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable, _
    '                         ByVal dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable, _
    '                         ByVal rezID As Long, ByVal staffInfo As StaffContext) As Boolean
    '        Dim rowReserve As SC3140103AdvancePreparationsReserveInfoRow = Nothing
    '        Dim rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow = Nothing
    '        Dim tempVisitSeq As Nullable(Of Long) = Nothing
    '        ' 事前準備チップ予約情報の取得
    '        If dtReserve IsNot Nothing AndAlso 0 < dtReserve.Rows.Count Then
    '            Dim dtRow As SC3140103AdvancePreparationsReserveInfoRow() = _
    '                DirectCast(dtReserve.Select("", "WORKTIME ASC"), SC3140103AdvancePreparationsReserveInfoRow())
    '            rowReserve = CType(dtRow(0), SC3140103AdvancePreparationsReserveInfoRow)
    '        Else
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            Me.ShowMessageBox(MsgID.id903)
    '            Return False
    '        End If

    '        ' 事前準備チップサービス来店管理情報の取得
    '        If dtVisit IsNot Nothing AndAlso 0 < dtVisit.Rows.Count Then
    '            rowAdvanceVisit = DirectCast(dtVisit.Rows(0), SC3140103AdvancePreparationsServiceVisitManagementRow)
    '            tempVisitSeq = rowAdvanceVisit.VISITSEQ
    '        End If
    '        '空の場合は自分のアカウントを入れる
    '        If String.IsNullOrEmpty(SASelector.SelectedValue) Then
    '            rowReserve.ACCOUNT_PLAN = staffInfo.Account
    '        Else
    '            'SA振当てが一致しない場合はエラー
    '            If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
    '                'SA未振当て
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return False
    '            End If
    '        End If
    '        If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
    '           Not (Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, TBL_STALLREZINFO)) Then
    '            'タイマークリア
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '            '自社客でない/SA振当て済み
    '            Me.ShowMessageBox(MsgID.id903)
    '            Return False
    '        End If

    '        If Not IsNothing(rowAdvanceVisit) Then
    '            '事前準備チップの来店管理情報がある場合
    '            If (Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
    '                                         TBL_SERVICE_VISIT_MANAGEMENT)) OrElse
    '                (Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then

    '                '自社客でない/SA振当て済み
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                Me.ShowMessageBox(MsgID.id903)
    '                Return False
    '            End If
    '        End If
    '        ' 顧客詳細画面へ遷移
    '        Me.RedirectCustomer(tempVisitSeq,
    '                            rezID,
    '                            rowReserve.VCLREGNO,
    '                            rowReserve.VIN,
    '                            rowReserve.MODELCODE,
    '                            rowReserve.TELNO,
    '                            rowReserve.MOBILE,
    '                            rowReserve.CUSTOMERNAME,
    '                            staffInfo.DlrCD,
    '                            "1",
    '                            rowReserve.ACCOUNT_PLAN,
    '                            VisitTypeOff)
    '        Return True
    '    End Function
    '    ''' <summary>
    '    ''' 呼出前に入力チェック
    '    ''' </summary>
    '    ''' <param name="callNo">呼出番号</param>
    '    ''' <param name="callPlace">呼出場所</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckCallOperation(ByVal callNo As String, ByVal callPlace As String) As Boolean
    '        Dim rtFlg As Boolean = True
    '        '発券番号チェック
    '        If String.IsNullOrEmpty(callNo) _
    '            OrElse String.IsNullOrEmpty(callNo.Trim) Then
    '            Me.ShowMessageBox(MsgID.id915)
    '            rtFlg = False
    '            Return rtFlg
    '        End If
    '        '呼び出し場所内容チェック
    '        If String.IsNullOrEmpty(callPlace) _
    '              OrElse String.IsNullOrEmpty(callPlace.Trim) Then
    '            Me.ShowMessageBox(MsgID.id911)
    '            rtFlg = False
    '            Return rtFlg
    '        End If
    '        If Not Validation.IsValidString(callPlace) Then
    '            Me.ShowMessageBox(MsgID.id912)
    '            Me.DetailsCallPlace.Focus()
    '            rtFlg = False
    '            Return rtFlg
    '        End If
    '        Return rtFlg
    '    End Function

    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


    '    ''' <summary>
    '    ''' 顧客車両情報(TACT側)設定
    '    ''' </summary>
    '    ''' <param name="rowReserve">事前準備予約情報</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    Private Function SetCustomerInfo(ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow) _
    '                                     As SC3140103AdvancePreparationsReserveInfoRow


    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} {2}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , LOG_START))


    '        'チップ詳細で取得しているモデルコード(車両型式)が存在するか確認
    '        If Not String.IsNullOrEmpty(Me.HiddenVehicleModel.Value.Trim) Then
    '            'モデルコード(車両型式)が存在する

    '            'チップ詳細で取得しているモデルコードを予約情報に設定
    '            rowReserve.MODELCODE = Me.HiddenVehicleModel.Value.Trim

    '        End If

    '        'チップ詳細で取得している電話番号が存在するか確認
    '        If Not String.IsNullOrEmpty(Me.DetailsPhoneNumber.Text.Trim) Then
    '            '電話番号が存在する

    '            'チップ詳細で取得している電話番号を予約情報に設定
    '            rowReserve.TELNO = Me.DetailsPhoneNumber.Text.Trim

    '        End If

    '        'チップ詳細で取得している携帯電話番号が存在するか確認
    '        If Not String.IsNullOrEmpty(Me.DetailsMobileNumber.Text.Trim) Then
    '            '携帯電話番号が存在する

    '            'チップ詳細で取得している携帯電話番号を予約情報に設定
    '            rowReserve.MOBILE = Me.DetailsMobileNumber.Text.Trim

    '        End If

    '        'チップ詳細で取得している顧客名が存在するか確認
    '        If Not String.IsNullOrEmpty(Me.DetailsCustomerName.Text.Trim) Then
    '            '顧客名が存在する

    '            'チップ詳細で取得している顧客名を予約情報に設定
    '            rowReserve.CUSTOMERNAME = Me.DetailsCustomerName.Text.Trim

    '        End If


    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))


    '        Return rowReserve


    '    End Function


    '    ''' <summary>
    '    ''' 予約IDの変換処理
    '    ''' サービス入庫IDから作業内容ID
    '    ''' </summary>
    '    ''' <param name="inReserveId">予約ID</param>
    '    ''' <remarks>予約ID</remarks>
    '    ''' <history>
    '    ''' </history>
    '    Private Function ChangeReserveId(ByVal inReserveId As Long) _
    '                                     As Long

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} {2} REZID:{3}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , LOG_START, inReserveId))


    '        '予約IDの確認
    '        If Not IsNothing(inReserveId) AndAlso 0 < inReserveId Then
    '            '予約IDの値がある場合

    '            'SMBCommonClass初期化
    '            Using smbCommonBiz As New SMBCommonClassBusinessLogic

    '                'サービス入庫IDから作業内容IDへ変換
    '                inReserveId = smbCommonBiz.GetServiceInIdToJobDetailId(inReserveId)

    '            End Using

    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} REZID:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END, inReserveId))

    '        Return inReserveId

    '    End Function


    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '#End Region

    '#Region " 画面遷移メソッド"

    '    ''' <summary>
    '    ''' 顧客詳細画面に遷移
    '    ''' </summary>
    '    ''' <param name="visitSeq">来店者実績連番</param>
    '    ''' <param name="reserveId">予約ID</param>
    '    ''' <param name="registerNo">車両登録No</param>
    '    ''' <param name="vin">ＶＩＮ</param>
    '    ''' <param name="modelCode">モデルコード</param>
    '    ''' <param name="telNo">電話番号</param>
    '    ''' <param name="mobileNo">携帯番号</param>
    '    ''' <param name="customerName">顧客名</param>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="AdvancePreparation">事前準備フラグ</param>
    '    ''' <param name="saCode">担当SAコード</param>
    '    ''' <param name="visitType">受付フラグ(0:受付エリア以外  1:受付エリア白チップ)</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '    ''' </history>
    '    Private Sub RedirectCustomer(ByVal visitSeq As Nullable(Of Long), _
    '                                 ByVal reserveId As Long, _
    '                                 ByVal registerNo As String, _
    '                                 ByVal vin As String, _
    '                                 ByVal modelCode As String, _
    '                                 ByVal telNo As String, _
    '                                 ByVal mobileNo As String, _
    '                                 ByVal customerName As String, _
    '                                 ByVal dealerCode As String, _
    '                                 ByVal AdvancePreparation As String, _
    '                                 ByVal saCode As String, _
    '                                 ByVal visitType As String)

    '        ' 2012/06/05 日比野 事前準備対応　START
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '        'If (Not Me.CheckCustomerType(customerType, TBL_SERVICE_VISIT_MANAGEMENT)) OrElse
    '        '    (Not Me.CheckCustormerCharge(saCode)) Then
    '        '    '自社客でない/担当SAが自分ではない
    '        '    Me.ShowMessageBox(MsgID.id903)
    '        '    Return
    '        'End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END


    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'BMTSへ引渡す予約IDは、作業内容IDへ統一
    '        'サービス入庫IDから作業内容IDへ変換
    '        reserveId = Me.ChangeReserveId(reserveId)

    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        Dim logOutCust As StringBuilder = New StringBuilder(String.Empty)
    '        With logOutCust
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_CUSTOMEROUT))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", NAME = {0}", customerName))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", REGISTERNO = {0}", registerNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", VINNO = {0}", vin))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", MODELCODE = {0}", modelCode))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", TEL1 = {0}", telNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", TEL2 = {0}", mobileNo))
    '            If visitSeq.HasValue Then
    '                .Append(String.Format(CultureInfo.CurrentCulture, "VISITSEQ = {0}", visitSeq.Value))
    '            End If
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", REZID = {0}", reserveId))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", AdvancePreparation = {0}", AdvancePreparation))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", CRDEALERCODE = {0}", dealerCode))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", FLAG = {0}", "1"))
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 START
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", Redirect.RECEPTIONFLAG = {0}", visitType))
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 END
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            If "1".Equals(AdvancePreparation) Then
    '                .Append(String.Format(CultureInfo.CurrentCulture, ", Redirect.SACODE = {0}", saCode))
    '            End If
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        End With
    '        Logger.Info(logOutCust.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", registerNo.Trim())           ' 車両登録No
    '        Me.SetValue(ScreenPos.Next, "Redirect.VINNO", vin.Trim())                       ' ＶＩＮ
    '        Me.SetValue(ScreenPos.Next, "Redirect.NAME", customerName.Trim())               ' 顧客名
    '        Me.SetValue(ScreenPos.Next, "Redirect.MODELCODE", modelCode.Trim())             ' モデルコード
    '        Me.SetValue(ScreenPos.Next, "Redirect.TEL1", telNo.Trim())                      ' 電話番号
    '        Me.SetValue(ScreenPos.Next, "Redirect.TEL2", mobileNo.Trim())                   ' 携帯番号
    '        If visitSeq.HasValue Then
    '            Me.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", visitSeq.Value)            ' 来店者ID
    '        End If
    '        Me.SetValue(ScreenPos.Next, "Redirect.REZID", reserveId)                        ' 予約ID
    '        Me.SetValue(ScreenPos.Next, "Redirect.PREPARECHIPFLAG", AdvancePreparation)     ' 事前準備フラグ
    '        Me.SetValue(ScreenPos.Next, "Redirect.CRDEALERCODE", dealerCode.Trim())         ' DLRコード
    '        Me.SetValue(ScreenPos.Next, "Redirect.FLAG", "1")                               ' 固定フラグ
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 START
    '        Me.SetValue(ScreenPos.Next, "Redirect.RECEPTIONFLAG", visitType)                ' 受付フラグ
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 END
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '        If "1".Equals(AdvancePreparation) Then
    '            Me.SetValue(ScreenPos.Next, "Redirect.SACODE", saCode)                ' SAコード
    '        End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        ' 顧客詳細画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)
    '        '' 2012/06/05 日比野 事前準備対応　END

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 新規顧客登録画面に遷移
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectCustomerNew()
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logNewCust As StringBuilder = New StringBuilder(String.Empty)
    '        With logNewCust
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_CUSTOMERNEW))
    '        End With
    '        Logger.Info(logNewCust.ToString())

    '        ' 新規顧客登録画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_CUSTOMERNEW)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '2012/06/11 日比野 事前準備対応 START
    '    ''' <summary>
    '    ''' 新規顧客登録画面に遷移
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Sub RedirectCustomerNew(ByVal visitSeq As Nullable(Of Long), _
    '                                    ByVal reserveId As Long, _
    '                                    ByVal name As String, _
    '                                    ByVal registerNo As String, _
    '                                    ByVal vin As String, _
    '                                    ByVal modelCode As String, _
    '                                    ByVal telNo As String, _
    '                                    ByVal mobileNo As String, _
    '                                    ByVal AdvancePreparation As String, _
    '                                    ByVal saCode As String, _
    '                                    ByVal visitType As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '        'If (Not Me.CheckCustormerCharge(saCode)) Then
    '        '    '担当SAが自分ではない
    '        '    Me.ShowMessageBox(MsgID.id903)
    '        '    Return
    '        'End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END


    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'BMTSへ引渡す予約IDは、作業内容IDへ統一
    '        'サービス入庫IDから作業内容IDへ変換
    '        reserveId = Me.ChangeReserveId(reserveId)

    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        Dim logOutCust As StringBuilder = New StringBuilder(String.Empty)
    '        With logOutCust
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_CUSTOMERNEW))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", NAME = {0}", name))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", REGISTERNO = {0}", registerNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", VINNO = {0}", vin))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", MODELCODE = {0}", modelCode))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", TEL1 = {0}", telNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", TEL2 = {0}", mobileNo))
    '            If visitSeq.HasValue Then
    '                .Append(String.Format(CultureInfo.CurrentCulture, "VISITSEQ = {0}", visitSeq.Value))
    '            End If
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", REZID = {0}", reserveId))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", AdvancePreparation = {0}", AdvancePreparation))
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 START
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", Redirect.RECEPTIONFLAG = {0}", visitType))
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 END
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '            If "1".Equals(AdvancePreparation) Then
    '                .Append(String.Format(CultureInfo.CurrentCulture, ", Redirect.SACODE = {0}", saCode))
    '            End If
    '            '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '        End With
    '        Logger.Info(logOutCust.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.NAME", name.Trim())                           ' 顧客名
    '        Me.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", registerNo.Trim())               ' 車両登録No
    '        Me.SetValue(ScreenPos.Next, "Redirect.VINNO", vin.Trim())                           ' ＶＩＮ
    '        Me.SetValue(ScreenPos.Next, "Redirect.MODELCODE", modelCode.Trim())                 ' モデルコード
    '        Me.SetValue(ScreenPos.Next, "Redirect.TEL1", telNo.Trim())                          ' 電話番号
    '        Me.SetValue(ScreenPos.Next, "Redirect.TEL2", mobileNo.Trim())                       ' 携帯番号
    '        Me.SetValue(ScreenPos.Next, "Redirect.PREPARECHIPFLAG", AdvancePreparation)         ' 事前準備フラグ
    '        Me.SetValue(ScreenPos.Next, "Redirect.REZID", reserveId)                            ' 予約ID
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 START
    '        Me.SetValue(ScreenPos.Next, "Redirect.RECEPTIONFLAG", visitType)                    ' 受付フラグ
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】問連「GTMC121022041」対応 END
    '        If visitSeq.HasValue Then
    '            Me.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", visitSeq.Value)                ' 来店者ID
    '        End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '        If "1".Equals(AdvancePreparation) Then
    '            Me.SetValue(ScreenPos.Next, "Redirect.SACODE", saCode)                    ' SAコード
    '        End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '        ' 新規顧客登録画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_CUSTOMERNEW)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub
    '    '2012/06/11 日比野 事前準備対応 END

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' R/O作成画面に遷移
    '    ''' </summary>
    '    ''' <param name="visitSeq">来店実績連番</param>
    '    ''' <remarks>
    '    ''' 来店実績連番を元に来店管理情報を取得し、
    '    ''' 当該情報の整備受注NOが未発行の場合は、新規に整備受注NOを発行し、
    '    ''' R/O作成画面に遷移します。
    '    ''' また当該情報の顧客が未取引客の場合、R/O作成画面には遷移しません。
    '    ''' </remarks>
    '    ''' <histiry>
    '    ''' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
    '    ''' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34）
    '    ''' 2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </histiry>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectOrderNew(ByVal visitSeq As Long, _
    '                                 ByVal dlrcd As String, _
    '                                 ByVal vclregno As String, _
    '                                 ByVal vin As String, _
    '                                 ByVal modelcode As String, _
    '                                 ByVal customername As String, _
    '                                 ByVal telno As String, _
    '                                 ByVal mobile As String, _
    '                                 ByVal rezid As Long, _
    '                                 ByVal strcd As String, _
    '                                 ByVal sacode As String, _
    '                                 ByVal washflg As String, _
    '                                 ByVal displayAreaAdvance As String, _
    '                                 ByVal orderNo As String, _
    '                                 ByVal stockTime As Date, _
    '                                 Optional ByVal customerCode As String = "")
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '        'If (Not Me.CheckCustomerType(customerType, TBL_SERVICE_VISIT_MANAGEMENT)) OrElse
    '        '    (Not Me.CheckCustormerCharge(sacode)) Then
    '        '    '自社客でない/担当SAが自分ではない
    '        '    Me.ShowMessageBox(MsgID.id903)
    '        '    Return
    '        'End If
    '        '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END


    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'BMTSへ引渡す予約IDは、作業内容IDへ統一
    '        'サービス入庫IDから作業内容IDへ変換
    '        rezid = Me.ChangeReserveId(rezid)

    '        '2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        ' 2012/06/05 日比野 事前準備対応　START
    '        Dim staffInfo As StaffContext = StaffContext.Current


    '        Dim wkOrderNo As String

    '        ' 整備受注NO発行チェック
    '        If String.IsNullOrEmpty(orderNo) Then
    '            ' 未発行

    '            '2012/10/22 TMEJ 河原  問連「GTMC121015030」対応 STRAT

    '            '引数チェック
    '            '登録No.
    '            If String.IsNullOrEmpty(vclregno) Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id84))
    '                Return
    '            End If
    '            'VIN
    '            If String.IsNullOrEmpty(vin) Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id85))
    '                Return
    '            End If
    '            '型式
    '            If String.IsNullOrEmpty(modelcode) Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id86))
    '                Return
    '            End If
    '            '顧客名
    '            If String.IsNullOrEmpty(customername) Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id87))
    '                Return
    '            End If
    '            '電話番号
    '            If String.IsNullOrEmpty(telno) Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id88))
    '                Return
    '            End If
    '            '入庫日時
    '            If IsNothing(stockTime) OrElse stockTime = Date.MinValue Then
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                'タイマークリア
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                Me.ShowMessageBox(MsgID.id909, WebWordUtility.GetWord(MsgID.id90))
    '                Return
    '            End If

    '            '2012/10/22 TMEJ 河原  問連「GTMC121015030」対応 END

    '            ' 整備受注NO 作成情報(0-整備受注NO、1-UpDate結果)
    '            Dim createOrderInformation(2) As String

    '            Using bl As New SC3140103BusinessLogic
    '                ' 整備受注NO発行処理
    '                createOrderInformation = bl.GetIFCreateOrderNo(dlrcd,
    '                                                               vclregno,
    '                                                               vin,
    '                                                               modelcode,
    '                                                               customername,
    '                                                               telno,
    '                                                               mobile,
    '                                                               rezid,
    '                                                               strcd,
    '                                                               sacode,
    '                                                               washflg,
    '                                                               visitSeq,
    '                                                               staffInfo,
    '                                                               stockTime,
    '                                                               displayAreaAdvance)
    '            End Using

    '            ' 反映結果格納
    '            Dim UpDateCheck As Long = Long.Parse(createOrderInformation(1), CultureInfo.CurrentCulture)
    '            ' 反映結果確認
    '            Select Case UpDateCheck
    '                Case RET_SUCCESS
    '                    wkOrderNo = createOrderInformation(0) ' 整備受注NO
    '                Case RET_DBTIMEOUT
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id901)   ' タイムアウト
    '                    Return
    '                Case RET_NOMATCH
    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} vclregno={3} vin={4} modelcode={5} customername={6} telno={7} " _
    '                                & "mobile={8} rezid={9} strcd={10} sacode={11} washflg={12} visitSeq={13} " _
    '                                & "stockTime={14} displayAreaAdvance={15}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , "RedirectOrderNew  UpDateCheck=902" _
    '                                , vclregno, vin, modelcode, customername, telno, _
    '                                mobile, rezid, strcd, sacode, washflg, visitSeq, stockTime, displayAreaAdvance))
    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id902)   ' その他
    '                    Return
    '                Case RET_DIFFSACODE
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id903)   ' ステータス異常
    '                    Return
    '                Case Else
    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} vclregno={3} vin={4} modelcode={5} customername={6} telno={7}" _
    '                                & "mobile={8} rezid={9} strcd={10} sacode={11} washflg={12} visitSeq={13} " _
    '                                & "stockTime={14} displayAreaAdvance={15} UpDateCheck={16}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , "RedirectOrderNew  UpDateCheck=ELSE" _
    '                                , vclregno, vin, modelcode, customername, telno, _
    '                                mobile, rezid, strcd, sacode, washflg, visitSeq, stockTime, displayAreaAdvance, UpDateCheck))
    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                    'タイマークリア
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                    Me.ShowMessageBox(MsgID.id902)   ' その他
    '                    Return
    '            End Select

    '        Else
    '            wkOrderNo = orderNo
    '        End If

    '        Dim sessionRezId As String = String.Empty

    '        If Not rezid = REZID_NONE_FIRST_VALUE Then
    '            sessionRezId = rezid.ToString(CultureInfo.CurrentCulture)
    '        End If

    '        ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 START
    '        Dim visitSeqString As String = String.Empty

    '        If Not visitSeq = VISITSEQ_NONE_FIRST_VALUE Then
    '            visitSeqString = visitSeq.ToString(CultureInfo.CurrentCulture)
    '        End If
    '        ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 END

    '        Dim logNewOrder As StringBuilder = New StringBuilder(String.Empty)
    '        With logNewOrder
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_ORDERNEW))
    '            .Append(String.Format(CultureInfo.CurrentCulture, " OrderNo = {0}", wkOrderNo))
    '            ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 START
    '            '.Append(String.Format(CultureInfo.CurrentCulture, ", VISITSEQ = {0}", visitSeq))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", VISITSEQ = {0}", visitSeqString))
    '            ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 END
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", REZID = {0}", sessionRezId))
    '        End With
    '        Logger.Info(logNewOrder.ToString())

    '        ' 次画面遷移パラメータ設定
    '        ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 START
    '        'Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)			  ' R/O No
    '        'Me.SetValue(ScreenPos.Next, "VISITSEQ", visitSeq)	  ' 来店者実績連番
    '        Me.SetValue(ScreenPos.Next, "OrderNo", wkOrderNo)         ' R/O No
    '        Me.SetValue(ScreenPos.Next, "VISITSEQ", visitSeqString)   ' 来店者実績連番
    '        ' 2012/07/05 TMEJ 西岡 【SERVICE_2】事前準備対応 END
    '        Me.SetValue(ScreenPos.Next, "REZID", sessionRezId)        ' 予約ID

    '        ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    '        Dim blnPopUpReserveFlg As Boolean = False                           ' 予約情報ポップアップ出力フラグ（True: 出力／False: 未出力）
    '        Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)     ' 選択チップ(表示エリア)

    '        ' 事前準備エリア以外のチップの場合（ = メインチップの場合）
    '        If Not (CType(ChipArea.AdvancePreparations, Long).Equals(detailArea)) Then
    '            ' 整備受注NOが未発行の場合（ = RO作成ボタン押下時）
    '            If String.IsNullOrEmpty(orderNo) Then
    '                Dim nowDate As Date = DateTimeFunc.Now(dlrcd)                          ' 現在の日付を取得
    '                Dim nextDate As Date = DateAdd(DateInterval.Day, 1, nowDate)           ' 翌日の日付を取得
    '                Dim strBaseDate = DateTimeFunc.FormatDate(CONVERTDATE_YMD, nextDate)   ' 予約情報の取得基準日を「翌日」に設定

    '                ' 予約情報ポップアップへデータをセット
    '                Dim dt As SC3140103ReserveListDataTable
    '                Using bl As New SC3140103BusinessLogic
    '                    dt = bl.GetPopupReservationList(dlrcd, _
    '                                                    strcd, _
    '                                                    customerCode, _
    '                                                    vclregno, _
    '                                                    vin, _
    '                                                    strBaseDate)
    '                    ' 翌日以降の該当予約が存在する場合（※翌日を含む）
    '                    If Not IsNothing(dt) Then
    '                        blnPopUpReserveFlg = True
    '                        ' 画面作成処理
    '                        Me.ReserveListRepeater.DataSource = dt
    '                        Me.ReserveListRepeater.DataBind()
    '                        Me.PopUpReserveListFooterButton.Text = WebWordUtility.GetWord(APPLICATIONID, 95)   '「OK」ボタンの文言を設定

    '                        ' 画面表示処理
    '                        Dim script As New StringBuilder
    '                        script.AppendLine("$('#DetailButtonLeft').css('display', 'none');")           '顧客詳細ボタン非表示
    '                        script.AppendLine("$('#DetailButtonCenter').css('display', 'none');")         'R/O参照ボタン非表示
    '                        script.AppendLine("$('#DetailButtonRight').css('display', 'none');")          '追加作業ボタン非表示
    '                        script.AppendLine("$('#IconLoadingPopup').css('display', 'none');")           'クルクル非表示（左）
    '                        script.AppendLine("$('#PopUpReserveListContents').fingerScroll();")
    '                        script.AppendLine("$('#PopUpReserveList').fadeIn(100);")
    '                        script.AppendLine("$('#PopUpReserveListFooterButton').bind('click', function (e) {$('#PopUpReserveList').fadeOut(100); ") 'ポップアップ画面を消す
    '                        script.AppendLine("commonRefreshTimer(RefreshDisplay); });")         'タイマーセット
    '                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PopUpReserve", script.ToString, True)
    '                        Me.UpdatePanel2.Update()
    '                    End If
    '                End Using
    '            End If
    '        End If

    '        ' 予約情報ポップアップを出力しない場合
    '        ' 　（予約情報ポップアップを出力する場合は、
    '        ' 　　ポップアップ内のOKボタン押下(detailReservePopupButton_Clickイベント)により、R/O作成画面に遷移を行う。）
    '        If Not (blnPopUpReserveFlg) Then
    '            ' R/O作成画面に遷移
    '            Me.RedirectNextScreen(APPLICATIONID_ORDERNEW)
    '        End If
    '        ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

    '        ' 2012/06/05 Hibino 事前準備対応　END

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' R/O参照画面に遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectOrderDisp(ByVal orderNo As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logOutOrder As StringBuilder = New StringBuilder(String.Empty)
    '        With logOutOrder
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_ORDEROUT))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "OrderNo = {0}", orderNo))
    '        End With
    '        Logger.Info(logOutOrder.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)   ' R/O ID

    '        ' R/O参照画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_ORDEROUT)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' R/O一覧画面に遷移
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectOrderList()
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logOrderList As StringBuilder = New StringBuilder(String.Empty)
    '        With logOrderList
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ORDERLIST))
    '        End With
    '        Logger.Info(logOrderList.ToString())

    '        ' R/O一覧画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_ORDERLIST)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業登録関連画面遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectAddWork(ByVal orderNo As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Using bizLogic As New SC3140103BusinessLogic
    '            Dim redirect As AddWorkRedirect = AddWorkRedirect.Invalid

    '            '追加作業関連遷移先情報取得
    '            redirect = bizLogic.GetAddWorkRedirect(orderNo)

    '            If redirect.Equals(AddWorkRedirect.SC3170201New) Then
    '                'SC3170201_追加作業入力(新規)画面遷移
    '                RedirectAddRepairSC3170201(orderNo, SC3170201_EDIT_FLAG_NEW_EDIT, "0")
    '            ElseIf redirect.Equals(AddWorkRedirect.SC3170101) Then
    '                '追加作業一覧へ画面遷移
    '                RedirectAddRepairList()
    '            Else
    '                '上記以外の場合

    '                Dim staffInfo As StaffContext = StaffContext.Current

    '                '最大の枝番取得
    '                Dim srvaddSeq As Integer = bizLogic.GetAddRepairMaxSeq(staffInfo.DlrCD,
    '                orderNo)

    '                '画面遷移
    '                Select Case redirect
    '                    Case AddWorkRedirect.SC3170201Edit
    '                        'SC3170201_追加作業入力(編集)画面遷移
    '                        RedirectAddRepairSC3170201(orderNo, SC3170201_EDIT_FLAG_NEW_EDIT, srvaddSeq.ToString(CultureInfo.CurrentCulture))
    '                    Case AddWorkRedirect.SC3170203Acting
    '                        'SC3170203_追加作業入力(代行)画面遷移
    '                        RedirectAddRepairSC3170203(orderNo, SC3170203_EDIT_FLAG_EDIT, srvaddSeq.ToString(CultureInfo.CurrentCulture))
    '                    Case AddWorkRedirect.SC3170203Preview
    '                        'SC3170203_追加作業入力(参照)画面遷移
    '                        RedirectAddRepairSC3170203(orderNo, SC3170203_EDIT_FLAG_PREVIEW, srvaddSeq.ToString(CultureInfo.CurrentCulture))
    '                    Case AddWorkRedirect.SC3170302
    '                        'SC3170302_追加作業プレビュー画面遷移
    '                        RedirectAddRepairPreview(orderNo, SC3170302_EDIT_FLAG_EDIT, srvaddSeq.ToString(CultureInfo.CurrentCulture))
    '                    Case Else
    '                        '2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '                        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '                        'タイマークリア
    '                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)
    '                        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '                        Me.ShowMessageBox(MsgID.id903)
    '                        '2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '                End Select

    '            End If
    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業一覧画面遷移
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectAddRepairList()
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logAddList As StringBuilder = New StringBuilder(String.Empty)

    '        With logAddList
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ADD_LIST))
    '        End With
    '        Logger.Info(logAddList.ToString())

    '        ' 追加作業一覧画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_ADD_LIST)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業入力(SC3170201)画面遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <param name="editFlg">編集フラグ(0: 新規, 1: 編集)</param>
    '    ''' <param name="srvaddSeq">追加作業ユニークID(0: 新規, それ以外: 枝番)</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectAddRepairSC3170201(ByVal orderNo As String, _
    '                                           ByVal editFlg As String, _
    '                                           ByVal srvaddSeq As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '        With logWork
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_WORK))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", editFlg))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", srvaddSeq))
    '        End With
    '        Logger.Info(logWork.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)            ' 整備受注NO
    '        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editFlg)            ' 編集フラグ
    '        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvaddSeq)        ' 追加作業ユニークID

    '        ' 追加作業入力画面(SC3170201)に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_WORK)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業入力(SC3170203)画面遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <param name="editFlg">編集フラグ(0: 新規, 1: 編集)</param>
    '    ''' <param name="srvaddSeq">追加作業ユニークID(0: 新規, それ以外: 枝番)</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectAddRepairSC3170203(ByVal orderNo As String, _
    '                                           ByVal editFlg As String, _
    '                                           ByVal srvaddSeq As String)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '        With logWork
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_WORK_OUT))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", editFlg))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", srvaddSeq))
    '        End With
    '        Logger.Info(logWork.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)            ' 整備受注NO
    '        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editFlg)            ' 編集フラグ
    '        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvaddSeq)        ' 追加作業ユニークID

    '        ' 追加作業入力画面(SC3170203)に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_WORK_OUT)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業プレビュー(SC3170302)画面遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <param name="editFlg">編集フラグ(0: 新規, 1: 編集)</param>
    '    ''' <param name="srvaddSeq">追加作業ユニークID(0: 新規, それ以外: 枝番)</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectAddRepairPreview(ByVal orderNo As String, _
    '                                         ByVal editFlg As String, _
    '                                         ByVal srvaddSeq As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '        With logWork
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_WORK_PREVIEW))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", editFlg))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", srvaddSeq))
    '        End With
    '        Logger.Info(logWork.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)            ' 整備受注NO
    '        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editFlg)            ' 編集フラグ
    '        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvaddSeq)        ' 追加作業ユニークID

    '        ' 追加作業プレビュー画面(SC3170302)に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_WORK_PREVIEW)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub
    '    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '    'Private Sub RedirectWork(ByVal orderNo As String)

    '    'Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '    'With logWork
    '    '.Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '    '.Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_WORK))
    '    '.Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '    '.Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", 0))
    '    '.Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", 0))
    '    'End With
    '    'Logger.Info(logWork.ToString())

    '    ' 次画面遷移パラメータ設定
    '    'Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)   ' R/O ID
    '    'Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", 0)         ' 編集フラグ
    '    'Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", 0)       ' 追加作業ユニークID

    '    ' 追加作業登録画面に遷移
    '    'Me.RedirectNextScreen(C_APPLICATIONID_WORK)

    '    'End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加作業承認画面に遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <param name="approvalId">追加承認待ちID</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectApproval(ByVal orderNo As String, _
    '                                 ByVal approvalId As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logApproval As StringBuilder = New StringBuilder(String.Empty)
    '        With logApproval
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_APPROVAL))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", approvalId))
    '            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", 0))
    '        End With
    '        Logger.Info(logApproval.ToString())

    '        '次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)       ' R/O ID
    '        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", approvalId)  ' 追加作業ユニークID
    '        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", 0)             ' 編集フラグ

    '        ' 追加作業承認画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_APPROVAL)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '    ' '''-----------------------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' チェックシート印刷画面に遷移
    '    ' ''' </summary>
    '    ' ''' <param name="orderNo">整備受注NO</param>
    '    ' ''' <remarks></remarks>
    '    ' '''-----------------------------------------------------------------------
    '    'Private Sub RedirectCheckSheet(ByVal orderNo As String)

    '    '    Dim logCheckSheet As StringBuilder = New StringBuilder(String.Empty)
    '    '    With logCheckSheet
    '    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_CHECKSHEET))
    '    '        .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '    '    End With
    '    '    Logger.Info(logCheckSheet.ToString())

    '    '    ' 次画面遷移パラメータ設定
    '    '    Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)   ' R/O ID

    '    '    ' チェックシート印刷画面に遷移
    '    '    Me.RedirectNextScreen(C_APPLICATIONID_CHECKSHEET)

    '    'End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 清算印刷画面に遷移
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub RedirectSettlement(ByVal orderNo As String)
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim logSettlement As StringBuilder = New StringBuilder(String.Empty)
    '        With logSettlement
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", APPLICATIONID_SETTLEMENT))
    '            .Append(String.Format(CultureInfo.CurrentCulture, "OrderNo = {0}", orderNo))
    '        End With
    '        Logger.Info(logSettlement.ToString())

    '        ' 次画面遷移パラメータ設定
    '        Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)   ' R/O ID

    '        ' 清算印刷画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_SETTLEMENT)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '    End Sub

    '#End Region

    '#Region " フッター制御 "

    '    ''' <summary>
    '    ''' メインメニュー
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const MAIN_MENU As Integer = 100
    '    ''' <summary>
    '    ''' 顧客情報
    '    ''' </summary>
    '    Private Const CUSTOMER_INFORMATION As Integer = 200
    '    ''' <summary>
    '    ''' 説明ツール
    '    ''' </summary>
    '    Private Const SUBMENU_EXPLANATION_TOOL As Integer = 700
    '    ''' <summary>
    '    ''' R/O作成
    '    ''' </summary>
    '    Private Const SUBMENU_RO_MAKE As Integer = 600
    '    ''' <summary>
    '    ''' スケジューラ
    '    ''' </summary>
    '    Private Const SUBMENU_SCHEDULER As Integer = 400
    '    ''' <summary>
    '    ''' 電話帳
    '    ''' </summary>
    '    Private Const SUBMENU_TELEPHONE_BOOK As Integer = 500
    '    ''' <summary>
    '    ''' 追加作業一覧
    '    ''' </summary>
    '    Private Const SUBMENU_ADD_LIST As Integer = 1100

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '    ''' <summary>
    '    ''' フッターコード：SMB
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Const FOOTER_SMB As Integer = 800
    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' フッター制御
    '    ''' </summary>
    '    ''' <param name="commonMaster">マスターページ</param>
    '    ''' <param name="category">カテゴリ</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, _
    '                        ByRef category As FooterMenuCategory) As Integer()
    '        '自ページの所属メニューを宣言
    '        category = FooterMenuCategory.MainMenu

    '        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
    '        Return New Integer() {}
    '    End Function

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' フッターボタンの制御
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    '''-----------------------------------------------------------------------
    '    Private Sub InitFooterEvent()

    '        'ヘッダ表示設定
    '        '戻るボタン非活性化
    '        CType(Me.Master.Master, CommonMasterPage).IsRewindButtonEnabled = False

    '        'フッタ表示設定
    '        'サブメニューボタンを設定（イベントハンドラ割り当て）
    '        '2012/06/18 KN 西岡 【SERVICE_2】事前準備対応 START
    '        '説明ツール(STEP1では非活性)
    '        'Dim explanationToolButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_EXPLANATION_TOOL)
    '        'explanationToolButton.Enabled = False
    '        'Dim explanationToolButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_EXPLANATION_TOOL)
    '        'explanationToolButton.Enabled = False
    '        '2012/06/18 KN 西岡 【SERVICE_2】事前準備対応 END

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'SMBボタンの設定
    '        Dim smbButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
    '        AddHandler smbButton.Click, AddressOf SMBButton_Click
    '        smbButton.OnClientClick = "return FooterButtonControl();"

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        'R/O作成
    '        Dim roMakeButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_RO_MAKE)
    '        AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
    '        ' 2012/02/23 KN 上田【SERVICE_1】START
    '        roMakeButton.OnClientClick = "return FooterButtonControl();"
    '        ' 2012/02/23 KN 上田【SERVICE_1】END
    '        '追加作業一覧
    '        Dim addListButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_ADD_LIST)
    '        AddHandler addListButton.Click, AddressOf addListButton_Click
    '        ' 2012/02/23 KN 上田【SERVICE_1】START
    '        addListButton.OnClientClick = "return FooterButtonControl();"
    '        ' 2012/02/23 KN 上田【SERVICE_1】END
    '        'スケジューラ
    '        Dim schedulerButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_SCHEDULER)
    '        schedulerButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"
    '        '電話帳
    '        Dim telephoneBookButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_TELEPHONE_BOOK)
    '        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

    '        'メニューボタンを設定（イベントハンドラ割り当て）
    '        'メインメニュー再表示
    '        Dim mainMenuButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
    '        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
    '        ' 2012/02/23 KN 上田【SERVICE_1】START
    '        mainMenuButton.OnClientClick = "return FooterButtonControl();"
    '        ' 2012/02/23 KN 上田【SERVICE_1】END
    '        '顧客情報画面
    '        Dim customerButton As CommonMasterFooterButton = _
    '        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
    '        AddHandler customerButton.Click, AddressOf customerButton_Click
    '        ' 2012/02/23 KN 上田【SERVICE_1】START
    '        customerButton.OnClientClick = "return FooterButtonControl();"
    '        ' 2012/02/23 KN 上田【SERVICE_1】END

    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' メインメニューへ遷移する。
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベントデータ</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub mainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        Try

    '            Dim logMainMenu As StringBuilder = New StringBuilder(String.Empty)
    '            With logMainMenu
    '                .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '                .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", MAINMENUID))
    '            End With
    '            Logger.Info(logMainMenu.ToString())

    '            ' 再表示
    '            Me.RedirectNextScreen(MAINMENUID)

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'タイムアウトエラーの場合は、メッセージを表示する
    '            ShowMessageBox(MsgID.id901)
    '        End Try

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' フッター「顧客詳細ボタン」クリック時の処理。
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベントデータ</param>
    '    ''' <remarks>
    '    ''' 新規顧客登録画面に遷移します。
    '    ''' </remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub customerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        '新規顧客登録画面に遷移
    '        Me.RedirectCustomerNew()

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' フッター「R/Oボタン」クリック時の処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベントデータ</param>
    '    ''' <remarks>
    '    ''' 　R/O一覧画面に遷移します。
    '    ''' </remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub roMakeButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        'R/O一覧画面に遷移
    '        Me.RedirectOrderList()

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    '''-----------------------------------------------------------------------
    '    ''' <summary>
    '    ''' フッター「追加作業ボタン」クリック時の処理
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベントデータ</param>
    '    ''' <remarks></remarks>
    '    '''-----------------------------------------------------------------------
    '    Private Sub addListButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        '追加作業一覧画面遷移
    '        Me.RedirectAddRepairList()

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '    ''' <summary>
    '    ''' SMBボタンを押した時の処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

    '        '開始ログ出力
    '        Dim logStart As New StringBuilder
    '        With logStart
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" Start")
    '        End With
    '        Logger.Info(logStart.ToString)

    '        '工程管理画面に遷移する
    '        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

    '        '終了ログ出力
    '        Dim logEnd As New StringBuilder
    '        With logEnd
    '            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '            .Append(" End")
    '        End With
    '        Logger.Info(logEnd.ToString)

    '    End Sub
    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '    ' ''' <summary>
    '    ' ''' 担当SAチェック
    '    ' ''' </summary>
    '    ' ''' <param name="saCode"></param>
    '    ' ''' <returns></returns>
    '    ' ''' <remarks></remarks>
    '    'Private Function CheckCustormerCharge(ByVal saCode As String) As Boolean
    '    '    Dim staffInfo As StaffContext = StaffContext.Current

    '    '    If saCode.Equals(staffInfo.Account) Then
    '    '        Return True
    '    '    End If

    '    '    '担当SAが自分では無い場合
    '    '    'Me.ShowMessageBox(MsgID.id903)
    '    '    Return False

    '    'End Function
    '    '2012/12/02 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '    ''' <summary>
    '    ''' 顧客区分をチェック
    '    ''' </summary>
    '    ''' <param name="customerType"></param>
    '    ''' <param name="tableName"></param>
    '    ''' <returns>チェック結果（true:自社客  false:未取引客）</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Function CheckCustomerType(ByVal customerType As String,
    '                                       ByVal tableName As String) As Boolean


    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} {2} P1:{3} P2:{4}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , LOG_START, customerType, tableName))

    '        'If tableName.Equals(TBL_STALLREZINFO) Then
    '        '    If customerType.Equals("0") Then
    '        '        Return True
    '        '    End If
    '        'ElseIf tableName.Equals(TBL_SERVICE_VISIT_MANAGEMENT) Then
    '        '    If customerType.Equals("1") Then
    '        '        Return True
    '        '    End If
    '        'End If

    '        If customerType.Equals("1") Then
    '            Return True
    '        End If

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' 顧客が自社客でない
    '        'Me.ShowMessageBox(MsgID.id903)
    '        Return False

    '    End Function

    '    ' ''' <summary>
    '    ' ''' 整備受注NoのNullチェック
    '    ' ''' </summary>
    '    ' ''' <param name="orderNo"></param>
    '    ' ''' <returns></returns>
    '    ' ''' <remarks></remarks>
    '    'Private Function CheckNullOrderNo(ByVal orderNo As Object) As Boolean

    '    '    If IsDBNull(orderNo) Then
    '    '        'R/OがNullの場合
    '    '        Me.ShowMessageBox(MsgID.id903)
    '    '        Return False
    '    '    End If

    '    '    If String.IsNullOrEmpty(CType(orderNo, String)) Then
    '    '        'R/OがNullの場合
    '    '        Me.ShowMessageBox(MsgID.id903)
    '    '        Return False
    '    '    End If

    '    '    Return True
    '    'End Function

    '    ''' <summary>
    '    ''' SA振当て済みﾁｪｯｸ
    '    ''' </summary>
    '    ''' <param name="status"></param>
    '    ''' <returns>true:SA未振当て  False:SA振当て済み</returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckAssignStatus(ByVal status As String) As Boolean
    '        If status.Equals("2") Then
    '            'SA振当て済み
    '            'Me.ShowMessageBox(MsgID.id903)
    '            Return False
    '        End If

    '        Return True
    '    End Function

    '#End Region

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '    '' 2012/08/03 彭 サービス緊急課題対応（受付登録機能）START
    '    ' ''' <summary>
    '    ' ''' 受付登録Footerボタンの押下
    '    ' ''' </summary>
    '    ' ''' <param name="sender">イベント発生元</param>
    '    ' ''' <param name="e">イベント引数</param>
    '    'Protected Sub ButtonRegister_Click(sender As Object, e As System.EventArgs) Handles ButtonRegister.Click
    '    '    Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_S")

    '    '    Dim RegNo As String = hfRegNo.Value.Trim()
    '    '    Logger.Info("RegNo=" & RegNo)

    '    '    If (RegNo.Length > 0) AndAlso (RegNo.Length <= 32) Then
    '    '        Dim result As Long = RET_SUCCESS

    '    '        Using bizLogic As New IC3810101BusinessLogic
    '    '            With StaffContext.Current
    '    '                Dim visitSeq As Long = -1
    '    '                result = bizLogic.InsertServiceVisitSA(.DlrCD, .BrnCD, hfRegNo.Value.Trim(), .Account, .Account, APPLICATIONID, visitSeq)
    '    '                Logger.Info("result=" & result.ToString(CultureInfo.CurrentCulture) & ", visitSeq=" & visitSeq.ToString(CultureInfo.CurrentCulture))

    '    '                If result = RET_SUCCESS AndAlso visitSeq > 0 Then
    '    '                    result = bizLogic.ChangeSACode(.DlrCD, .BrnCD, visitSeq, .Account, APPLICATIONID)
    '    '                    Logger.Info("result=" & result.ToString(CultureInfo.CurrentCulture))
    '    '                End If
    '    '            End With
    '    '        End Using
    '    '        Logger.Info("result=" & result.ToString(CultureInfo.CurrentCulture))

    '    '        Select Case result
    '    '            Case RET_SUCCESS
    '    '                'Do nothing, just refresh
    '    '            Case RET_DBTIMEOUT
    '    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '    '                'タイマークリア
    '    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '    '                Me.ShowMessageBox(MsgID.id901)   ' タイムアウト
    '    '            Case Else
    '    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
    '    '                'タイマークリア
    '    '                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END
    '    '                Me.ShowMessageBox(MsgID.id908)   ' 登録失敗（今のところに実際に出る可能性がないとInsertServiceVisitSAの担当者から聞いているが）
    '    '        End Select
    '    '    Else
    '    '        Logger.Error("wrong input. RegNo.Length=" & RegNo.Length.ToString(CultureInfo.CurrentCulture))
    '    '    End If

    '    '    '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    '    '    'タイマークリア
    '    '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
    '    '    '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '    '    Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_E")
    '    'End Sub
    '    '' 2012/08/03 彭 サービス緊急課題対応（受付登録機能）END

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '    '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START

    '    ''' <summary>
    '    ''' 再描画用ボタン
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub RefreshButton_Click(sender As Object, e As System.EventArgs) Handles RefreshButton.Click

    '        'PostBackさせてリフレッシュさせるため処理なし。

    '    End Sub

    '    '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    '    ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START

    '    ''' <summary>
    '    ''' 予約情報ポップアップのOKボタンの押下
    '    ''' </summary>
    '    ''' <param name="sender">イベント発生元</param>
    '    ''' <param name="e">イベント引数</param>
    '    ''' <remarks></remarks>
    '    Protected Sub DetailReservePopupButton_Click(sender As Object, e As System.EventArgs) Handles PopUpReserveListFooterButton.Click
    '        ' R/O作成画面に遷移
    '        Me.RedirectNextScreen(APPLICATIONID_ORDERNEW)
    '    End Sub

    '    ' 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STAT

#Region "イベント処理"

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発
    ''' </history>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '事前準備機能開発時にEnvSettingからフラグを取得し設定する
        '現在は機能開発していないので固定で非表示設定
        '事前準備仕様フラグを取得し、表示設定をする
        Me.AdvancePreparationsButton.Visible = False

        'チップタイマー用に現在時刻取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)


        If Me.IsPostBack Then


            '新規顧客登録機能開発時にEnvSettingからフラグを取得し設定する
            '現在は機能開発していないので固定で非表示設定
            'SYSTEMENVから新規顧客登録使用フラグを取得する
            Me.UseNewCustomer.Value = UseNewCustomerFlagOff


            '受付モニター使用フラグ取得
            'DealerEnvSettingのインスタンス
            Dim dealerEnvBiz As New DealerEnvSetting

            'DealerEnvSettingの取得値
            Dim dealerEnvParam As String = String.Empty

            '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 START
            ''DealerEnvSettingの取得処理(受付モニター使用フラグ)
            'Dim drSystemEnvSetting As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
            '    dealerEnvBiz.GetEnvSetting(staffInfo.DlrCD, WaitReceptionType)

            ''取得できた場合のみ設定する
            'If Not (IsNothing(drSystemEnvSetting)) Then

            '    dealerEnvParam = drSystemEnvSetting.PARAMVALUE

            'End If
            'DealerEnvSettingの取得処理(受付モニター使用フラグ)
            Dim drDealerEnvSetting As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
                dealerEnvBiz.GetEnvSetting(staffInfo.DlrCD, WaitReceptionType)

            '取得できた場合のみ設定する
            If Not (IsNothing(drDealerEnvSetting)) Then

                dealerEnvParam = drDealerEnvSetting.PARAMVALUE

            End If
            '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 END

            'SYSTEMENVから受付モニター使用フラグを取得する
            Me.UseReception.Value = dealerEnvParam

            '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 START
            '追加作業起票蓋締めフラグを取得
            Dim systemEnvBiz As New SystemEnvSetting
            Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
                systemEnvBiz.GetSystemEnvSetting(AddWorkCloseTypeParamName)

            '取得情報チェック
            If Not (IsNothing(drSystemEnvSetting)) Then
                '取得できた場合
                '取得した値を設定する
                Me.AddWorkCloseType.Value = drSystemEnvSetting.PARAMVALUE

            End If
            '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 END

        End If

        'フッターの制御
        InitFooterEvent()

        'ポストバック確認
        If Not Me.IsPostBack Then

            '事前準備機能が使用されているかチェック
            If Me.AdvancePreparationsButton.Visible Then
                '使用している

                '担当SA選択欄設定処理
                'ユーザーステータス取得
                Using user As New IC3810601BusinessLogic

                    'ユーザーステータステーブル
                    Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable
                    Dim useropcode As New List(Of Long)

                    'SA権限のオペレーションコード設定
                    useropcode.Add(Operation.SA)

                    'ユーザーステータス取得処理
                    userdt = user.GetAcknowledgeStaffList(staffInfo.DlrCD, _
                                                          staffInfo.BrnCD, _
                                                          useropcode)

                    '一度今設定されているのもを削除する
                    Me.SASelector.Items.Clear()

                    '一番上に空白のSAを設定する
                    Me.SASelector.Items.Add(New ListItem("", ""))

                    '取得したユーザー分ループしてSASelectorに設定
                    For i = 0 To userdt.Rows.Count Step 1

                        '取得してユーザー情報でアカウントがNothingのものは除外する
                        If i < userdt.Rows.Count AndAlso userdt(i).ACCOUNT IsNot Nothing Then

                            'SASelectorに追加
                            Me.SASelector.Items.Add(New ListItem(userdt(i).USERNAME, userdt(i).ACCOUNT))

                            'ログインユーザーと同じアカウントの検索
                            If userdt(i).ACCOUNT = staffInfo.Account Then

                                '同じアカウントの場合初期SAとして設定する
                                Me.SASelector.SelectedIndex = i + 1
                            End If
                        End If
                    Next
                End Using
            End If
        End If

        'SASASelectorのChangeイベントを設定
        SASelector.Attributes.Add("onchange", "SAchange()")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 初期表示ボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 初期表示または通知イベントが発生した際に、ダミーボタンである当ボタンを
    ''' クライアントにてクリックすることで当イベントが発生します。
    ''' </remarks>
    Protected Sub MainPolling_Click(sender As Object, e As System.EventArgs) Handles MainPolling.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try

            '各工程チップ情報取得
            Me.InitVisitChip()

            'UpdatePanelを更新する
            Me.ContentUpdatePanel.Update()

            'フッターボタンを更新
            Me.FotterUpdatePanel.Update()


        Catch oracleEx As OracleExceptionEx When oracleEx.Number = 1013
            'DBタイムアウト

            'アクティブインジケータ非表示(タイマークリア)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'DBタイムアウトエラーの場合は、メッセージを表示する
            Me.ShowMessageBox(MsgID.id901)

        Catch timeOutEx As TimeoutException When TypeOf timeOutEx.InnerException Is OracleExceptionEx

            'アクティブインジケータ非表示(タイマークリア)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'タイムアウトエラーの場合は、メッセージを表示する
            Me.ShowMessageBox(MsgID.id901)

        Finally

            ' チップタイマー用に現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
        End Try

        'アクティブインジケータ非表示(タイマークリア)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 振当待ちリフレッシュボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' GKサービス送信時にPush送信され、ダミーボタンである当ボタンを
    ''' クライアントにてクリックすることで当イベントが発生します。
    ''' </remarks>
    Protected Sub AssignmentRefreshButton_Click(sender As Object, e As System.EventArgs) Handles AssignmentRefreshButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Me.InitVisitChip(AssignmentRefreshFlag)


        'アクティブインジケータ非表示(タイマークリア)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Push用再描画ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RefreshButton_Click(sender As Object, e As System.EventArgs) Handles RefreshButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'PostBackさせてリフレッシュさせるため処理なし。


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' チップの詳細ポップアップウィンドウに表示する情報を取得する為のダミーボタンクリック
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' チップ詳細画面ポップアップ表示のために隠しボタンである当ボタンを
    ''' クライアント側にてクリックすることでイベントが発生します。
    ''' </remarks>
    ''' <histiry>
    ''' </histiry>
    Protected Sub DetailPopupButton_Click(sender As Object, e As System.EventArgs) Handles DetailPopupButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '選択せれているチップ情報の取得
        '来店実績連番
        Dim visitNo As String = Me.DetailsVisitNo.Value
        '予約ID
        Dim reserveId As Decimal = 0
        '選択チップの表示エリア
        Dim selectChipArea As Long = SetNullToLong(Me.DetailsArea.Value)

        '予約IDがDecimalに変換できるかチェック
        If Not Decimal.TryParse(Me.DetailsRezId.Value, reserveId) Then

            '初期値-1を設定
            reserveId = -1

        End If


        'チップ詳細の情報用クラス
        Dim chipDetail As ChipDetail = Nothing

        'SMBCommoncclassのインスタンス
        Using SMBCommon As New SMBCommonClassBusinessLogic

            Try
                '選択エリアチェック
                If ChipArea.AdvancePreparations = selectChipArea Then
                    '事前準備エリア

                    'チップ詳細情報の取得
                    chipDetail = SMBCommon.GetChipDetailReserve(staffInfo.DlrCD, _
                                                                staffInfo.BrnCD, _
                                                                reserveId)
                Else
                    '上記以外(メイン画面エリア)

                    '来店チップ詳細情報の取得
                    chipDetail = SMBCommon.GetChipDetailVisit(staffInfo.DlrCD, _
                                                              staffInfo.BrnCD, _
                                                              CType(visitNo, Long))
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトのみキャッチ

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                'ORACLEのタイムアウトのみメッセージ
                Me.ShowMessageBox(MsgID.id901)


                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} DBTIMEOUTERR MESSAGE = {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , ex.Message))

                '処理終了
                Exit Sub

            End Try

        End Using

        'チップ詳細情報の取得チェック
        If chipDetail Is Nothing Then
            '情報が取得できなかった場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} {2} SELECTCHIPAREA={3} REZID={4} VISITSEQ={5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , "DetailPopupButton_Click  chipDetail=nothing" _
                        , selectChipArea, reserveId, visitNo))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)


            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id902)

            '処理終了
            Exit Sub

        End If


        'チップ詳細に情報を設定
        Me.SetDetailPopupInfo(chipDetail, staffInfo, selectChipArea)


        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' チップ詳細フッタ部の標準ボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' チップ詳細のフッタ部の標準ボタンを押下したときの処理を実施する。
    ''' </remarks>
    ''' <history>
    ''' </history>
    Protected Sub DetailDefaultButton_Click(sender As Object, e As System.EventArgs) Handles DetailNextScreenCommonButton.Click

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在日時取得
        Dim presentTime As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)

        'アクションフラグ
        Dim actionFlg As Boolean = True

        'SC3140103BusinessLogicインスタンス
        Using bis As New SC3140103BusinessLogic

            Try
                '各オブジェクトの宣言
                '予約情報ROW
                Dim rowReserve As SC3140103AdvancePreparationsReserveInfoRow = Nothing
                '画面遷移用来店情報ROW
                Dim rowVisitInfo As SC3140103NextScreenVisitInfoRow = Nothing
                '事前準備情報ROW
                Dim rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow = Nothing
                '来店実績連番(NULLを許可するオブジェクト)
                Dim wkVisitSeq As Nullable(Of Long) = Nothing


                '選択チップ情報の取得
                '予約IDの取得
                Dim rezId As Decimal = Me.SetNullToDecimal(Me.DetailsRezId.Value, -1)

                '来店実績連番の取得
                Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

                '選択チップ(表示エリア)
                Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

                '標準ボタンステータス
                Dim buttonStatus As String = Me.DetailClickButtonStatus.Value

                '2ボタン標準ボタンステータス
                Dim eventButtonStatus As String = Me.DetailClickButtonCheck.Value

                'サービス入庫テーブルの行ロックバージョン
                Dim rowLockVersion As Long = Me.SetNullToLong(Me.DetailRowLockVersion.Value, 0)

                '来店テーブルのステータス更新処理
                '振当待ちエリア：SA振当状態にして、呼出ステータスを「2：呼出完了にする」
                '受付エリア：呼出ステータスを「2：呼出完了にする」
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'Dim resultCode As Long = Me.UpdateVisitManageStatus(bis, staffInfo, visitSeq, detailArea, presentTime, True)
                Dim resultCode As Long = Me.UpdateVisitManageStatus(bis, staffInfo, visitSeq, detailArea, presentTime, True, rezId)
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                '更新処理チェック
                If resultCode <> RET_SUCCESS Then
                    '失敗の場合、処理を中断する

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:UpdateVisitManageStatus VISITSEQ={2}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , visitSeq))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(CType(resultCode, Integer))

                    '処理終了スクリプト設定
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                    '処理中断
                    Exit Sub

                End If

                '表示エリアチェック
                If detailArea = CType(ChipArea.AdvancePreparations, Long) Then
                    '事前準備エリアの場合

                    '事前準備情報用DataTable
                    Dim dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable

                    '事前準備チップ予約情報の取得
                    dtReserve = bis.GetAdvancePreparationsReserveInfo(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

                    '作業開始順にソートする
                    Dim dtRow As SC3140103AdvancePreparationsReserveInfoRow() = _
                        CType(dtReserve.Select("", " WORKTIME ASC"), SC3140103AdvancePreparationsReserveInfoRow())

                    '事前準備チップ予約情報のチェック
                    If dtReserve IsNot Nothing AndAlso dtReserve.Rows.Count > 0 Then
                        '事前準備チップ予約情報がある場合

                        '作業の一番早い予約の情報を取得
                        rowReserve = CType(dtRow(0), SC3140103AdvancePreparationsReserveInfoRow)


                        '基幹側の顧客情報をチェックして予約情報に設定する
                        rowReserve = Me.SetCustomerInfo(rowReserve)


                    Else
                        '事前準備チップ予約情報の取得に失敗

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} {2} REZID={3}" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , "DetailDefaultButton_Click  detailArea::dtReserve=nothing or Count=0" _
                                   , rezId))


                        'タイマークリア
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                        'エラーメッセージ
                        Me.ShowMessageBox(MsgID.id902)

                        Exit Try

                    End If

                    '事前準備チップサービス来店管理情報の取得
                    Dim dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable

                    '事前準備チップサービス来店管理情報取得
                    dtVisit = bis.GetAdvancePreparationsVisitManager(staffInfo.DlrCD, staffInfo.BrnCD, rezId)

                    '事前準備チップサービス来店管理情報チェック
                    If dtVisit IsNot Nothing AndAlso dtVisit.Rows.Count > 0 Then
                        '事前準備チップサービス来店管理情報がある場合

                        '事前準備サービス来店管理情報の先頭行を取得
                        rowAdvanceVisit = _
                            CType(dtVisit.Rows(0), SC3140103AdvancePreparationsServiceVisitManagementRow)

                        '来店実績連番の取得
                        wkVisitSeq = rowAdvanceVisit.VISITSEQ

                    End If

                Else
                    '事前準備エリア以外の工程エリアの場合

                    '画面遷移用来店情報取得
                    Dim dt As SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable = _
                        bis.GetNextScreenVisitInfo(detailArea, visitSeq, staffInfo, rowLockVersion, False)

                    '画面遷移用来店情報のチェック
                    If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
                        '画面遷移用来店情報の取得成功

                        'ROWに変換
                        rowVisitInfo = CType(dt.Rows(0), SC3140103DataSet.SC3140103NextScreenVisitInfoRow)

                    Else
                        '取得失敗

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} DETAILAREA={3} VISITSEQ={4} ROWLOCKVERSION={5}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , "NextScreenVisitInfoDataTable = NOTHING " _
                                    , detailArea, visitSeq, rowLockVersion))

                        'タイマークリア
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                        '排他エラーメッセージ
                        Me.ShowMessageBox(MsgID.id903)

                        Exit Try

                    End If

                    'チップ詳細からの画面遷移処理
                    Me.NextScreenVisitChipDetailButton(bis,
                                                       detailArea,
                                                       buttonStatus,
                                                       eventButtonStatus,
                                                       rowVisitInfo,
                                                       presentTime)

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'タイムアウトエラーの場合は、メッセージを表示する
                ShowMessageBox(MsgID.id901)

                '処理終了スクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '処理中断
                Exit Sub

            Finally

                ' チップタイマー用に現在時刻取得
                Me.nowDateTime = presentTime

            End Try

        End Using

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)


        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' 予約情報ポップアップのOKボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub DetailReservePopupButton_Click(sender As Object, e As System.EventArgs) Handles PopUpReserveListFooterButton.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' チップ詳細R/O参照ボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' R/O参照ボタンを押下した際に隠しボタンである当ボタンが
    ''' クライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    Protected Sub DetailOrderButton_Click(sender As Object, e As System.EventArgs) Handles DetailButtonRightDummy.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim bis As New SC3140103BusinessLogic

        Try

            '画面遷移用来店情報ROW
            Dim rowVisitInfo As SC3140103NextScreenVisitInfoRow = Nothing

            '来店実績連番の取得
            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

            '選択チップ(表示エリア)
            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

            'サービス入庫テーブルの行ロックバージョン
            Dim rowLockVersion As Long = Me.SetNullToLong(Me.DetailRowLockVersion.Value, 0)

            '画面遷移用来店情報取得
            Dim dt As SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable = _
                bis.GetNextScreenVisitInfo(detailArea, visitSeq, staffInfo, rowLockVersion, True)

            '画面遷移用来店情報のチェック
            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
                '画面遷移用来店情報の取得成功

                'ROWに変換
                rowVisitInfo = CType(dt.Rows(0), SC3140103DataSet.SC3140103NextScreenVisitInfoRow)

                'ログインアカウントの設定
                rowVisitInfo.ACCOUNT = staffInfo.Account

            Else
                '取得失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} {2} DETAILAREA={3} VISITSEQ={4} ROWLOCKVERSION={5}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , "NextScreenVisitInfoDataTable = NOTHING " _
                            , detailArea, visitSeq, rowLockVersion))

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '排他エラーメッセージ
                Me.ShowMessageBox(MsgID.id917)

                Exit Try

            End If


            'R/O参照画面遷移処理
            Me.ChipDetailOrderDispButton(detailArea, rowVisitInfo)


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウトエラー

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , "ERR:DBTIMEOUT "))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'タイムアウトエラーの場合は、メッセージを表示する
            Me.ShowMessageBox(MsgID.id901)

        Finally

            If bis IsNot Nothing Then

                bis.Dispose()
                bis = Nothing

            End If

        End Try

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' チップ詳細顧客詳細ボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客詳細ボタンを押下した際に隠しボタンである当ボタンが
    ''' クライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    ''' <history>
    ''' </history>
    Protected Sub DetailCustomerButton_Click(sender As Object, e As System.EventArgs) Handles DetailButtonLeftDummy.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim bis As New SC3140103BusinessLogic

        Try

            '画面遷移用来店情報ROW
            Dim rowVisitInfo As SC3140103NextScreenVisitInfoRow = Nothing

            '来店実績連番の取得
            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

            '選択チップ(表示エリア)
            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

            '現在日時
            Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

            'サービス入庫テーブルの行ロックバージョン
            Dim rowLockVersion As Long = Me.SetNullToLong(Me.DetailRowLockVersion.Value, 0)

            '画面遷移用来店情報取得
            Dim dt As SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable = _
                bis.GetNextScreenVisitInfo(detailArea, visitSeq, staffInfo, rowLockVersion, True, False)

            '画面遷移用来店情報のチェック
            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
                '画面遷移用来店情報の取得成功

                'ROWに変換
                rowVisitInfo = CType(dt.Rows(0), SC3140103DataSet.SC3140103NextScreenVisitInfoRow)

                'ログインアカウントの設定
                rowVisitInfo.ACCOUNT = staffInfo.Account

            Else
                '取得失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} {2} DETAILAREA={3} VISITSEQ={4} ROWLOCKVERSION={5}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , "NextScreenVisitInfoDataTable = NOTHING " _
                            , detailArea, visitSeq, rowLockVersion))

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '排他エラーメッセージ
                Me.ShowMessageBox(MsgID.id917)

                Exit Try

            End If

            '来店テーブルのステータス更新処理
            '受付エリア：呼出ステータスを「2：呼出完了にする」
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            'Dim resultCode As Long = Me.UpdateVisitManageStatus(bis, staffInfo, visitSeq, detailArea, presentTime, False)
            Dim resultCode As Long = Me.UpdateVisitManageStatus(bis, staffInfo, visitSeq, detailArea, presentTime, False, rowVisitInfo.REZID)
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            '更新処理チェック
            If resultCode <> RET_SUCCESS Then
                '失敗の場合、処理を中断する

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} Err:UpdateVisitManageStatus VISITSEQ={2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , visitSeq))

                'エラーメッセージ表示
                Me.ShowMessageBox(CType(resultCode, Integer))

                '処理終了スクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '処理中断
                Exit Sub

            End If


            '顧客詳細画面処理
            Me.ChipDetailCustomerButton(rowVisitInfo)


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウトエラー

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , "ERR:DBTIMEOUT "))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'タイムアウトエラーの場合は、メッセージを表示する
            Me.ShowMessageBox(MsgID.id901)

        Finally

            If bis IsNot Nothing Then

                bis.Dispose()
                bis = Nothing

            End If

        End Try

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 退店・振当解除ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Protected Sub DetailButtonDeleteDummy_Click(sender As Object, e As System.EventArgs) Handles DetailButtonDeleteDummy.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインユーザー情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在日時取得
        Dim presentTime As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)

        '選択チップ(表示エリア)
        Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

        '選択チップの来店実績連番取得
        Dim visitSequence As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

        'サービス入庫テーブルの行ロックバージョン
        Dim rowLockVersion As Long = Me.SetNullToLong(Me.DetailRowLockVersion.Value, 0)

        'チップ表示時更新日時
        Dim updateDate As Date = Date.MinValue

        '排他制御用のチップ表示時更新日時の取得
        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
            '更新日時の取得に失敗

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} DetailsVisitUpdateDate IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            '処理中断
            Exit Sub

        End If

        'SC3140103BusinessLogicのインスタンス
        Using bis As New SC3140103BusinessLogic

            '退店・振当解除処理
            Dim returnCode As Long = bis.SetReceptDelete(detailArea, _
                                                         visitSequence, _
                                                         presentTime, _
                                                         staffInfo, _
                                                         updateDate, _
                                                         rowLockVersion)

            '処理結果チェック
            If returnCode <> RET_SUCCESS Then
                '処理失敗

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:SetReceptDelete" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー内容によって表示メッセージ変更
                Select Case returnCode
                    Case RET_DBTIMEOUT
                        'DBタイムアウト

                        Me.ShowMessageBox(MsgID.id901)

                    Case RET_NOMATCH
                        '該当データ無し

                        Me.ShowMessageBox(MsgID.id902)

                    Case Else
                        'DBエラー

                        Me.ShowMessageBox(MsgID.id903)

                End Select

                '処理中断
                Exit Sub

            End If

            'OperationCodeリスト
            Dim operationCodeList As New List(Of Long)

            'イベントキー情報
            Dim eventKey As String = String.Empty

            '表示されているエリアで処理変更
            '表示エリアチェック
            If CType(ChipArea.Assignment, Long) = detailArea Then
                '振当待ちエリア

                '退店処理の通知処理

                'OperationCodeリストに権限"52"：SVRを設定
                operationCodeList.Add(Operation.SVR)

                'OperationCodeリストに権限"9"：SAを設定
                operationCodeList.Add(Operation.SA)

                'イベントキーに退店を設定
                eventKey = CType(EventKeyId.StoreOut, String)

            ElseIf CType(ChipArea.Reception, Long) = detailArea Then
                '受付エリア

                '振当解除処理の通知処理

                'OperationCodeリストに権限"52"：SVRを設定
                operationCodeList.Add(Operation.SVR)

                'OperationCodeリストに権限"9"：SAを設定
                operationCodeList.Add(Operation.SA)

                'イベントキーに振当解除を設定
                eventKey = CType(EventKeyId.SAUndo, String)

            End If

            '通知処理
            bis.NoticeProcessing(detailArea, _
                                 visitSequence, _
                                 presentTime, _
                                 staffInfo, _
                                 eventKey)

            'ユーザーステータス取得
            Using user As New IC3810601BusinessLogic

                'ユーザーステータス取得処理
                '各権限の全ユーザー情報取得
                Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                    user.GetAcknowledgeStaffList(staffInfo.DlrCD, _
                                                 staffInfo.BrnCD, _
                                                 operationCodeList)

                '各権限オンラインユーザー分ループ
                For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                    '各権限に対するPush処理
                    bis.SendPushServer(userRow.OPERATIONCODE, staffInfo, userRow.ACCOUNT, PushFlag0)

                Next

            End Using

        End Using

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 呼出しボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CallButton_Click(sender As Object, e As System.EventArgs) Handles CallButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Updateフラグ
        Dim blnupdateFlg As Boolean = True

        '呼出し場所
        Dim callPlace As String = Me.DetailsCallPlace.Text
        '呼出No
        Dim callNo As String = Me.DetailsCallNo.Text

        '呼出し前入力項目チェック処理
        blnupdateFlg = CheckCallOperation(callNo, callPlace)

        '呼出し可能かチェック
        If blnupdateFlg Then
            '呼出可能

            '戻り値
            Dim returnCode As Long = RET_SUCCESS

            '選択チップの来店実績連番
            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)
            '選択チップ(表示エリア)
            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

            '表示エリアチェック
            If detailArea <> CType(ChipArea.Reception, Long) Then
                '受付エリア以外のエリア

                '更新日時(ラベル)削除
                Me.DetailsVisitUpdateDateLabel.Text = String.Empty

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id903)

                'エラースクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '終了
                Return

            End If

            'ログインユーザー情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            '現在日時取得
            Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)

            '更新日時
            Dim updateDate As Date = Date.MinValue

            '更新日時取得
            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
                '更新日時を取得できなかった場合

                '最小値を設定
                updateDate = Date.MinValue

            End If

            'SC3140103BusinessLogicのインスタンス
            Using bis As New SC3140103BusinessLogic

                '呼出し処理
                returnCode = bis.CallVisit(visitSeq, staffInfo.Account, nowDate, updateDate)

            End Using


            '処理結果チェック
            If returnCode <> RET_SUCCESS Then
                '処理失敗

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} RETURNCODE = {2} " _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

                '更新日時(ラベル)削除
                Me.DetailsVisitUpdateDateLabel.Text = String.Empty

                'エラー内容によって表示メッセージ変更
                Select Case returnCode
                    Case RET_DBTIMEOUT
                        'DBタイムアウト

                        Me.ShowMessageBox(MsgID.id901)

                    Case RET_NOMATCH
                        '該当データ無し

                        Me.ShowMessageBox(MsgID.id902)

                    Case Else
                        'DBエラー

                        Me.ShowMessageBox(MsgID.id903)

                End Select


                'エラースクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)


            Else
                '処理成功

                '呼出しボタン非表示
                Me.BtnCALL.Style("display") = "none"
                '呼出しキャンセルボタン表示
                Me.BtnCALLCancel.Style("display") = "block"
                '呼出し場所を読取り専用にする
                Me.DetailsCallPlace.ReadOnly = True
                '更新日時を再設定
                Me.DetailsVisitUpdateDateLabel.Text = nowDate.ToString(CultureInfo.CurrentCulture)

                'SC3140103BusinessLogicのインスタンス
                Using bls As New SC3140103BusinessLogic

                    '受付待ちモニターPush送信処理
                    bls.SendPushForCall(staffInfo.DlrCD, staffInfo.BrnCD, True)

                End Using

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
                '更新日時を最新化
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "VisitUpdateDate();", True)

            End If

        Else
            '呼出不可

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 呼出しキャンセルボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CallCancelButton_Click(sender As Object, e As System.EventArgs) Handles CallCancelButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim returnCode As Long = 0

        '選択チップの来店実績連番
        Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

        '選択チップ(表示エリア)
        Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

        'ログインユーザー情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在日時取得
        Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)

        '更新日時
        Dim updateDate As Date = Date.MinValue

        '更新日時取得
        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
            '更新日時を取得できなかった場合

            '最小値を設定
            updateDate = Date.MinValue

        End If

        '表示エリアチェック
        If detailArea <> CType(ChipArea.Reception, Long) Then
            '受付エリア以外のエリア

            '更新日時(ラベル)削除
            Me.DetailsVisitUpdateDateLabel.Text = String.Empty

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            'エラースクリプト設定
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            '終了
            Return

        End If

        'SC3140103BusinessLogicのインスタンス
        Using bis As New SC3140103BusinessLogic

            '呼出しキャンセル処理実行
            returnCode = bis.CallCancelVisit(visitSeq, staffInfo.Account, nowDate, updateDate)

        End Using



        '処理結果チェック
        If returnCode <> RET_SUCCESS Then
            '処理失敗

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} RETURNCODE = {2} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , returnCode))

            '更新日時(ラベル)削除
            Me.DetailsVisitUpdateDateLabel.Text = String.Empty

            'エラー内容によって表示メッセージ変更
            Select Case returnCode
                Case RET_DBTIMEOUT
                    'DBタイムアウト

                    Me.ShowMessageBox(MsgID.id901)

                Case RET_NOMATCH
                    '該当データ無し

                    Me.ShowMessageBox(MsgID.id902)

                Case Else
                    'DBエラー

                    Me.ShowMessageBox(MsgID.id903)

            End Select


            'エラースクリプト設定
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

        Else
            '処理成功

            '呼出しキャンセルボタン非表示
            Me.BtnCALLCancel.Style("display") = "none"
            '呼出しボタン表示
            Me.BtnCALL.Style("display") = "block"
            '呼出し場所を読取り専用を解除する
            Me.DetailsCallPlace.ReadOnly = False
            '更新日時を再設定
            Me.DetailsVisitUpdateDateLabel.Text = nowDate.ToString(CultureInfo.CurrentCulture)

            'SC3140103BusinessLogicのインスタンス
            Using bls As New SC3140103BusinessLogic

                '受付待ちモニターPush送信処理
                bls.SendPushForCall(staffInfo.DlrCD, staffInfo.BrnCD, False)

            End Using

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
            '更新日時を最新化
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "VisitUpdateDate();", True)

        End If
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 呼出し場所更新ダミーボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DetailsCallPlace_Change(sender As Object, e As System.EventArgs) Handles CallPlaceChangeButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Updateフラグ
        Dim blnupdateFlg As Boolean = True

        '呼出し場所
        Dim callPlace As String = Me.DetailsCallPlace.Text

        '呼出し場所入力内容チェック
        If Not Validation.IsValidString(callPlace) Then
            '禁止文字が存在する

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id912)

            '変更前のテキストを取得
            Dim bakCallPlace As String = Me.SetNullToString(Me.BakCallPlace.Value)

            '変更前のテキストに戻す
            Me.DetailsCallPlace.Text = bakCallPlace

            '更新不可
            blnupdateFlg = False

        End If

        '更新チェック
        If blnupdateFlg Then
            '更新可能

            '処理結果
            Dim returnCode As Long = RET_SUCCESS

            '選択チップの来店実績連番
            Dim visitSeq As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value)

            '選択チップ(表示エリア)
            Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

            '表示エリアチェック
            If detailArea <> CType(ChipArea.Reception, Long) Then
                '受付エリア以外のエリア

                '更新日時(ラベル)削除
                Me.DetailsVisitUpdateDateLabel.Text = String.Empty

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id903)

                'エラースクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                '終了
                Return

            End If

            'ログインユーザー情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            '現在日時取得
            Dim nowDate As DateTime = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)

            '更新日時
            Dim updateDate As Date = Date.MinValue

            '更新日時取得
            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
                '更新日時を取得できなかった場合

                '最小値を設定
                updateDate = Date.MinValue

            End If

            'SC3140103BusinessLogicのインスタンス
            Using bis As New SC3140103BusinessLogic

                '呼び出し場所更新処理実行
                returnCode = bis.CallPlaceChange(visitSeq, _
                                                 callPlace.Trim, _
                                                 staffInfo.Account, _
                                                 nowDate, _
                                                 updateDate)

            End Using

            '処理結果チェック
            If returnCode <> RET_SUCCESS Then
                '処理失敗

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} RETURNCODE = {2} " _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

                '更新日時(ラベル)削除
                Me.DetailsVisitUpdateDateLabel.Text = String.Empty

                'エラー内容によって表示メッセージ変更
                Select Case returnCode
                    Case RET_DBTIMEOUT
                        'DBタイムアウト

                        Me.ShowMessageBox(MsgID.id901)

                    Case RET_NOMATCH
                        '該当データ無し

                        Me.ShowMessageBox(MsgID.id902)

                    Case Else
                        'DBエラー

                        Me.ShowMessageBox(MsgID.id903)

                End Select


                'エラースクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            Else
                '処理成功

                '更新日時を最新に書換える
                Me.DetailsVisitUpdateDateLabel.Text = nowDate.ToString(CultureInfo.CurrentCulture)

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
                '更新日時を最新化
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "VisitUpdateDate();", True)

            End If

        Else
            '更新不可

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 顧客検索開始のダミーボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客検索の検索開始ボタンを押下した際に隠しボタンである当ボタンが
    ''' クライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    Protected Sub SearchCustomerButton_Click(sender As Object, e As System.EventArgs) Handles SearchCustomerDummyButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '自社客検索処理
        Me.GetCustomerInfomation()

        'SearchDataUpdate更新
        Me.SearchDataUpdate.Update()

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 顧客付替え前確認ボタンのダミーボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客付替えボタンを押下した際に隠しボタンである当ボタンが
    ''' クライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    Protected Sub BeforeChipChanges_Click(sender As Object, e As System.EventArgs) Handles BeforeChipChangesDummyButton.Click

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '付替え元情報取得
        '来店実績連番取得
        Dim visitNumber As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)

        '車両登録番号取得
        Dim registrationNumber As String = Me.SetNullToString(Me.SearchRegistrationNumberChange.Value)

        '基幹顧客ID取得
        Dim dmsId As String = Me.SetNullToString(Me.SearchDMSIdChange.Value)

        'VIN取得
        Dim vinNumber As String = Me.SetNullToString(Me.SearchVinChange.Value)

        'チップ表示時更新日時
        Dim updateDate As Date = Date.MinValue

        '排他制御用のチップ表示時更新日時の取得
        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
            '更新日時の取得に失敗

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} DetailsVisitUpdateDate IS NOTHING" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            'クライアントにエラーコード返却
            Me.ChipResultChange.Value = SC3140103BusinessLogic.ChangeResultErr.ToString(CultureInfo.CurrentCulture)

            '処理終了スクリプト設定
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            '処理中断
            Exit Sub

        End If


        'ログイン情報管理機能（ログイン情報取得）
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3140103BusinessLogicのインスタンス
        Using bl As New SC3140103BusinessLogic

            '顧客付替え前確認処理
            Dim row As SC3140103BeforeChangeCheckResultRow = _
                bl.GetCustomerChangeCheck(staffInfo, _
                                          visitNumber, _
                                          registrationNumber, _
                                          vinNumber, _
                                          updateDate, _
                                          dmsId)


            '付替え前処理チェック
            If SC3140103BusinessLogic.ChangeResultTrue.Equals(row.CHANGECHECKRESULT) Then
                '付替え可能

                '付替え前確認結果をHiddenFieldへの設定処理
                Me.SetHiddenStatus(row)

            ElseIf SC3140103BusinessLogic.ChangeReusltCheck.Equals(row.CHANGECHECKRESULT) Then
                '付替えは可能(担当SAが付替え先の担当SAが異なるためコンフォームを出力する)


                '付替え前確認結果をHiddenFieldへの設定処理
                Me.SetHiddenStatus(row)

                '文言設定
                '選択された顧客の予約または来店情報がありますが、他のSAが担当となっています。このまま処理を続行しますか？
                Me.ChipConfirmChange.Value = WebWordUtility.GetWord(APPLICATIONID, MsgID.id907)


            ElseIf SC3140103BusinessLogic.ChangeResultErr.Equals(row.CHANGECHECKRESULT) Then
                '予期せぬエラー(排他等)の場合

                '処理終了スクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id903)

            Else
                'その他エラー(再描画無し)

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id917)

                '処理中断
                Exit Sub

            End If

            'クライアントにエラーコード返却
            Me.ChipResultChange.Value = row.CHANGECHECKRESULT.ToString(CultureInfo.CurrentCulture)

        End Using


        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)


        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' 顧客付替えボタンのボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客付替えボタンを押下した際に隠しボタンである当ボタンが
    ''' クライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' 2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
    ''' </history>
    Protected Sub ChipChanges_Click(sender As Object, e As System.EventArgs) Handles ChipChangesDummyButton.Click

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '付替え元情報取得
        '付替え元来店実績連番
        Dim beforeVisitNumber As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)
        '付替え元予約ID
        Dim beforeReserveNumber As Decimal = Me.SetNullToDecimal(Me.ChipReserveNumberBefore.Value, DEFAULT_LONG_VALUE)
        ''付替え元整備受注番号
        'Dim beforeOrderNumber As String = Me.SetNullToString(Me.ChipOrderNumberBefore.Value)


        '付替え先情報取得
        '付替え先来店実績連番
        Dim afterVisitNumber As Long = Me.SetNullToLong(Me.ChipVisitNumberChange.Value, DEFAULT_LONG_VALUE)
        '付替え先予約ID
        Dim afterReserveNumber As Decimal = Me.SetNullToDecimal(Me.ChipReserveNumberChange.Value, DEFAULT_LONG_VALUE)
        '付替え先担当SA
        Dim afterSaCode As String = Me.SetNullToString(Me.ChipSACodeChange.Value)
        '付替え先整備受注番号
        Dim afterOrderNumber As String = Me.SetNullToString(Me.ChipOrderNumberChange.Value)
        '付替え先車両IDを取得
        Dim afterVehicleId As Decimal = Me.SetNullToDecimal(Me.ChipVehicleIdAfter.Value, DEFAULT_LONG_VALUE)


        '検索一覧の行から顧客情報を取得する
        '顧客情報取得
        '車両登録番号
        Dim registrationNumber As String = Me.SetNullToString(Me.SearchRegistrationNumberChange.Value)
        '顧客コード(CST_ID)
        Dim customerCode As String = Me.SetNullToString(Me.SearchCustomerCodeChange.Value)
        '基幹顧客コード
        Dim dmsId As String = Me.SetNullToString(Me.SearchDMSIdChange.Value)
        'VIN
        Dim vinNumber As String = Me.SetNullToString(Me.SearchVinChange.Value)
        'モデルコード
        Dim model As String = Me.SetNullToString(Me.SearchModelChange.Value)
        '顧客名
        Dim customerName As String = Me.SetNullToString(Me.SearchCustomerNameChange.Value)
        '電話番号
        Dim phone As String = Me.SetNullToString(Me.SearchPhoneChange.Value)
        '携帯番号
        Dim mobile As String = Me.SetNullToString(Me.SearchMobileChange.Value)
        '担当SA
        Dim saCode As String = Me.SetNullToString(Me.SearchSACodeChange.Value)

        '選択チップ(表示エリア)
        Dim detailArea As Long = Me.SetNullToLong(Me.DetailsArea.Value)

        'チップ表示時更新日時
        Dim updateDate As Date = Date.MinValue

        '排他制御用のチップ表示時更新日時の取得
        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
            '更新日時の取得に失敗

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} DetailsVisitUpdateDate IS NOTHING" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            'クライアントにエラーコード返却
            Me.ChipResultChange.Value = SC3140103BusinessLogic.ChangeResultErr.ToString(CultureInfo.CurrentCulture)

            '処理終了スクリプト設定
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            '処理中断
            Exit Sub

        End If


        'ログイン情報管理機能（ログイン情報取得）
        Dim staffInfo As StaffContext = StaffContext.Current

        '付替え結果
        Dim customerChange As Long = RET_SUCCESS

        'SC3140103BusinessLogicのインスタンス
        Using bl As New SC3140103BusinessLogic

            '顧客付替え登録処理
            customerChange = bl.SetCustomerChange(staffInfo, _
                                                  beforeVisitNumber, _
                                                  afterVisitNumber, _
                                                  afterReserveNumber, _
                                                  afterOrderNumber, _
                                                  registrationNumber, _
                                                  customerCode, _
                                                  dmsId, _
                                                  vinNumber, _
                                                  model, _
                                                  customerName, _
                                                  phone, _
                                                  mobile, _
                                                  afterVehicleId, _
                                                  updateDate, _
                                                  detailArea)




            '顧客付替え登録処理結果
            If SC3140103BusinessLogic.ResultSuccess <> customerChange Then
                '処理失敗

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} Err:SetCustomerChange" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー内容によって表示メッセージ変更
                Select Case customerChange
                    Case RET_DBTIMEOUT
                        'DBタイムアウト

                        Me.ShowMessageBox(MsgID.id901)

                    Case RET_NOMATCH
                        '該当データ無し

                        Me.ShowMessageBox(MsgID.id902)

                    Case Else
                        'DBエラー

                        Me.ShowMessageBox(MsgID.id903)

                End Select

                '処理中断
                Exit Sub

            End If

            'OperationCodeリスト
            Dim operationCodeList As New List(Of Long)

            '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
            'OperationCodeリスト(WBS用)
            Dim operationCodeListWBS As New List(Of Decimal)
            '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

            'OperationCodeリストに権限"52"：SVRを設定
            operationCodeList.Add(Operation.SVR)

            'OperationCodeリストに権限"9"：SAを設定
            operationCodeList.Add(Operation.SA)

            '表示エリアのチェック
            If detailArea = CType(ChipArea.Assignment, Long) Then
                '振当待ちエリアの場合
                '振当て処理をしているのでCT/ChTにPUSH

                'OperationCodeリストに権限"55"：CTを設定
                operationCodeList.Add(Operation.CT)

                'OperationCodeリストに権限"62"：CHTを設定
                operationCodeList.Add(Operation.CHT)

            End If


            '付替え元または付替え先で予約があるかチェック
            If 0 < beforeReserveNumber OrElse 0 < afterReserveNumber Then
                '予約がある場合

                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
                'OperationCodeリストに権限"63"：WBSを設定
                'operationCodeList.Add(Operation.WBS)
                operationCodeListWBS.Add(Operation.WBS)

                'WBS権限の全ユーザー情報の取得
                Dim utility As New VisitUtilityBusinessLogic
                Dim userdtWBS As VisitUtilityUsersDataTable = _
                    utility.GetUsers(staffInfo.DlrCD, staffInfo.BrnCD, operationCodeListWBS, Nothing, DeleteFlagNone)
                utility = Nothing

                'WBS権限全ユーザー分ループ
                For Each userRowWBS As VisitUtilityUsersRow In userdtWBS

                    'WBS権限に対するPush処理
                    bl.SendPushServer(userRowWBS.OPERATIONCODE, staffInfo, userRowWBS.ACCOUNT, PushFlag0)
                Next
                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

            End If

            'ユーザーステータス取得
            Using user As New IC3810601BusinessLogic

                'ユーザーステータス取得処理
                '各権限の商談中以外のユーザー情報取得
                Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                    user.GetAcknowledgeStaffList(staffInfo.DlrCD, _
                                                 staffInfo.BrnCD, _
                                                 operationCodeList)

                '各権限の商談中以外のユーザー分ループ
                For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                    '各権限に対するPush処理
                    bl.SendPushServer(userRow.OPERATIONCODE, staffInfo, userRow.ACCOUNT, PushFlag0)

                Next

            End Using

        End Using

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        '更新日時を最新に書換える
        Me.DetailsVisitUpdateDate.Value = DateTimeFunc.Now(staffInfo.DlrCD).ToString(CultureInfo.CurrentCulture)

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' 顧客解除ボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客解除ボタンがクライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    ''' <history>
    ''' 2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
    ''' </history>
    Protected Sub SearchCustomerClearButton_Click(sender As Object, e As System.EventArgs) Handles ChipClearDummyButton.Click

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '解除対象チップの情報を取得
        '来店実績連番
        Dim removeVisitNumber As Long = Me.SetNullToLong(Me.DetailsVisitNo.Value, DEFAULT_LONG_VALUE)
        '予約ID
        Dim removeReserveNumber As Decimal = Me.SetNullToDecimal(Me.DetailsRezId.Value, DEFAULT_LONG_VALUE)

        'ログイン情報管理機能（ログイン情報取得）
        Dim staffInfo As StaffContext = StaffContext.Current

        '日付管理機能（現在日付取得）
        Dim nowTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        'チップ表示時更新日時
        Dim updateDate As Date = Date.MinValue

        '排他制御用のチップ表示時更新日時の取得
        If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
            '更新日時の取得に失敗

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} DetailsVisitUpdateDate IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            'クライアントにエラーコード返却
            Me.ChipResultChange.Value = SC3140103BusinessLogic.ChangeResultErr.ToString(CultureInfo.CurrentCulture)

            '処理終了スクリプト設定
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            '処理中断
            Exit Sub

        End If

        '更新結果
        Dim customerChange As Long = 0

        'SC3140103BusinessLogicのインスタンス
        Using bl As New SC3140103BusinessLogic

            '顧客解除処理
            customerChange = bl.SetCustomerClear(staffInfo, _
                                                 removeVisitNumber, _
                                                 nowTime, _
                                                 updateDate)


            '顧客解除処理結果
            If SC3140103BusinessLogic.ResultSuccess <> customerChange Then
                '処理失敗

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} Err:SetCustomerClear VISITSEQ {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , removeVisitNumber))

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

                'エラー内容によって表示メッセージ変更
                Select Case customerChange
                    Case RET_DBTIMEOUT
                        'DBタイムアウト

                        Me.ShowMessageBox(MsgID.id901)

                    Case RET_NOMATCH
                        '該当データ無し

                        Me.ShowMessageBox(MsgID.id902)

                    Case Else
                        'DBエラー

                        Me.ShowMessageBox(MsgID.id903)

                End Select

                '処理中断
                Exit Sub

            End If

            'OperationCodeリスト
            Dim operationCodeList As New List(Of Long)

            '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
            'OperationCodeリスト(WBS用)
            Dim operationCodeListWBS As New List(Of Decimal)
            '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

            'OperationCodeリストに権限"52"：SVRを設定
            operationCodeList.Add(Operation.SVR)

            '予約があるかチェック
            If 0 < removeReserveNumber Then
                '予約がある場合

                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
                'OperationCodeリストに権限"63"：WBSを設定
                'operationCodeList.Add(Operation.WBS)
                operationCodeListWBS.Add(Operation.WBS)

                'WBS権限の全ユーザー情報の取得
                Dim utility As New VisitUtilityBusinessLogic
                Dim userdtWBS As VisitUtilityUsersDataTable = _
                    utility.GetUsers(staffInfo.DlrCD, staffInfo.BrnCD, operationCodeListWBS, Nothing, DeleteFlagNone)
                utility = Nothing

                'WBS権限全ユーザー分ループ
                For Each userRowWBS As VisitUtilityUsersRow In userdtWBS

                    'WBS権限に対するPush処理
                    bl.SendPushServer(userRowWBS.OPERATIONCODE, staffInfo, userRowWBS.ACCOUNT, PushFlag1)
                Next
                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

            End If

            'ユーザーステータス取得
            Using user As New IC3810601BusinessLogic

                'ユーザーステータス取得処理
                '各権限の商談中以外のユーザー情報取得
                Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                    user.GetAcknowledgeStaffList(staffInfo.DlrCD, _
                                                 staffInfo.BrnCD, _
                                                 operationCodeList)

                '各権限の商談中以外のユーザー分ループ
                For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                    '各権限に対するPush処理
                    bl.SendPushServer(userRow.OPERATIONCODE, staffInfo, userRow.ACCOUNT, PushFlag1)

                Next

            End Using

        End Using

        '更新日時を最新に書換える
        Me.DetailsVisitUpdateDate.Value = nowTime.ToString(CultureInfo.CurrentCulture)

        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' 事前準備ポップアップに表示するチップの取得用ダミーボタン処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' フッターの事前予約ボタンを押下した際に隠しボタンである当ボタンを
    ''' クライアント側でクリックすることでイベントが発生します。
    ''' </remarks>
    ''' <history>
    ''' </history>
    Protected Sub AdvancePreparations_Click(sender As Object,
                                            e As System.EventArgs) Handles AdvancePreparationsClick.Click
        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable = Nothing
        Dim bl As SC3140103BusinessLogic = _
                New SC3140103BusinessLogic(Me.deliverypreAbnormalLt)

        Try
            ' 現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
            'dt = bl.GetReserveChipInfo()
            dt = bl.GetReserveChipInfo(SASelector.SelectedValue.ToString)
            '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

            Dim todayCount As Long = 0
            Dim nextCount As Long = 0
            ' 事前準備チップ情報を取得

            '当日、および翌日の件数を取得
            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
                'SA事前準備フラグが未完了のもの（整備受注Noがないものも含まれる）を件数にカウントする
                If PreparationMiddle.Equals(row.SASTATUSFLG) Then
                    If (row.TODAYFLG.Equals("1")) Then
                        '当日件数加算
                        todayCount += 1
                    Else
                        '翌日件数加算
                        nextCount += 1
                    End If
                End If
            Next

            'アイコンの固定文字列取得
            Dim strRightIcnD As String = WebWordUtility.GetWord(APPLICATIONID, 7)
            Dim strRightIcnI As String = WebWordUtility.GetWord(APPLICATIONID, 8)
            Dim strRightIcnS As String = WebWordUtility.GetWord(APPLICATIONID, 9)

            '固定文字列「～」取得
            wordFixedString = WebWordUtility.GetWord(APPLICATIONID, 32)
            '事前準備エリアチップ初期設定
            Me.InitAdvancePreparations(dt, strRightIcnD, strRightIcnI, strRightIcnS)

            ' 取得した事前準備件数を事前準備ボタンに反映
            Dim buttonStatus As String
            Dim countResult As String
            If todayCount = 0 Then
                If nextCount = 0 Then
                    ' 当日・翌日とも0件なら件数表示なし
                    buttonStatus = "0"
                    countResult = " "
                Else
                    buttonStatus = "1"
                    countResult = nextCount.ToString(CultureInfo.CurrentCulture)
                End If
            Else
                buttonStatus = "2"
                countResult = (todayCount + nextCount).ToString(CultureInfo.CurrentCulture)
            End If
            Me.AdvancePreparationsCntHidden.Value = countResult
            Me.AdvancePreparationsColorHidden.Value = buttonStatus

            Me.UpdatePanel1.Update()
            Me.FotterUpdatePanel.Update()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        Finally
            If dt IsNot Nothing Then
                dt.Dispose()
                dt = Nothing
            End If
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try

        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
        'タイマークリア
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)
        '2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    '2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 START

    ''' <summary>
    ''' フッター「Otherjob」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' Otherjob画面に遷移します。
    ''' </remarks>
    Protected Sub OtherjobButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OtherjobDummyButton.Click

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Otherjob画面遷移処理(パラメータ設定)
        Me.RedirectOtherjob()

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 END

#End Region

#Region "Privateメソッド"

#Region "初期表示"

    ''' <summary>
    ''' 各工程チップ情報取得
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Sub InitVisitChip(Optional ByVal inAssignmentRefreshFla As String = "")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在日時の取得
        Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

        Dim bl As SC3140103BusinessLogic = _
            New SC3140103BusinessLogic(Me.deliverypreAbnormalLt)

        Try

            '標準時間情報取得
            Dim rowStanderdLt As StandardLTListRow = Me.GetStandardLTList(bl, staffInfo.DlrCD, staffInfo.BrnCD)

            'アイコンの固定文字列取得
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            strRightIcnM = WebWordUtility.GetWord(APPLICATIONID, WordID.id10001)
            strRightIcnB = WebWordUtility.GetWord(APPLICATIONID, WordID.id10002)
            strRightIcnE = WebWordUtility.GetWord(APPLICATIONID, WordID.id10003)
            strRightIcnT = WebWordUtility.GetWord(APPLICATIONID, WordID.id10004)
            strRightIcnP = WebWordUtility.GetWord(APPLICATIONID, WordID.id10005)
            strRightIcnL = WebWordUtility.GetWord(APPLICATIONID, WordID.id10006)
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            Dim strRightIcnD As String = WebWordUtility.GetWord(APPLICATIONID, 7)
            Dim strRightIcnI As String = WebWordUtility.GetWord(APPLICATIONID, 8)
            Dim strRightIcnS As String = WebWordUtility.GetWord(APPLICATIONID, 9)

            '固定文字列「～」取得
            wordFixedString = WebWordUtility.GetWord(APPLICATIONID, 32)

            '振当待ちエリアチップ初期設定
            Me.InitAssignment(bl, staffInfo.DlrCD, staffInfo.BrnCD, Me.nowDateTime, strRightIcnD, strRightIcnI, strRightIcnS)

            '呼出元確認(振当待ちエリアリフレッシュ関数の呼出の場合は下記使用しない)
            If Not AssignmentRefreshFlag.Equals(inAssignmentRefreshFla) Then
                '初期表示の場合

                '受付エリアチップ初期設定
                Me.InitReception(bl, staffInfo, Me.nowDateTime, strRightIcnD, strRightIcnI, strRightIcnS)

                '作業中・納車準備・納車作業エリアチップ初期設定
                Me.InitMainChip(bl, staffInfo, Me.nowDateTime, strRightIcnD, strRightIcnI, strRightIcnS, rowStanderdLt)


                '事前準備が使用されているときのみ件数の取得
                If Me.AdvancePreparationsButton.Visible Then
                    '事前準備ボタンの表示がある場合

                    '事前準備件数を設定
                    Me.SetAdvancePreparationsCount(bl, staffInfo, Me.nowDateTime)

                End If

            End If

        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 標準時間情報取得
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Function GetStandardLTList(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                                     , ByVal inDealerCode As String _
                                     , ByVal inBranchCode As String) _
                                       As StandardLTListRow


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))


        'サービス標準LT取得
        Dim dtStanderdLt As StandardLTListDataTable = _
                inSC3140103BusinessLogic.GetStandardLTList(inDealerCode,
                                                           inBranchCode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} OUT:IC3810701.COUNT = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , dtStanderdLt.Rows.Count))

        '標準時間ROW
        Dim rowStanderdLt As StandardLTListRow

        'サービス標準LT情報取得確認
        If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
            '取得成功

            'ROWに変換
            rowStanderdLt = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)

            '追加作業承認標準時間
            If rowStanderdLt.IsADDWORK_STANDARD_LTNull Then

                '値がない場合は初期値0を設定
                rowStanderdLt.ADDWORK_STANDARD_LT = 0
            End If

            '洗車標準時間
            If rowStanderdLt.IsWASHTIMENull Then

                '値がない場合は初期値0を設定
                rowStanderdLt.WASHTIME = 0
            End If

            '納車準備標準時間
            If rowStanderdLt.IsDELIVERYPRE_STANDARD_LTNull Then

                '値がない場合は初期値0を設定
                rowStanderdLt.DELIVERYPRE_STANDARD_LT = 0
            End If

            '納車標準時間
            If rowStanderdLt.IsDELIVERYWR_STANDARD_LTNull Then

                '値がない場合は初期値0を設定
                rowStanderdLt.DELIVERYWR_STANDARD_LT = 0
            End If

        Else
            '取得失敗

            '新しい行を作成
            rowStanderdLt = dtStanderdLt.NewStandardLTListRow

            '初期値を設定
            rowStanderdLt.ADDWORK_STANDARD_LT = 0
            rowStanderdLt.WASHTIME = 0
            rowStanderdLt.DELIVERYPRE_STANDARD_LT = 0
            rowStanderdLt.DELIVERYWR_STANDARD_LT = 0

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rowStanderdLt

    End Function

    ''' <summary>
    ''' 振当待ちエリアチップ初期設定
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitAssignment(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                             , ByVal inDealerCode As String _
                             , ByVal inBranchCode As String _
                             , ByVal inPresentTime As Date _
                             , ByVal strRightIcnD As String _
                             , ByVal strRightIcnI As String _
                             , ByVal strRightIcnS As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '振当待ちエリア情報を取得
        Dim dtVisitInfo As SC3140103DataSet.SC3140103VisitManagementInfoDataTable = _
            inSC3140103BusinessLogic.GetAssignmentInfo(inDealerCode, inBranchCode, inPresentTime)

        '振当待ちエリア情報をコントロールにバインドする
        Me.AssignmentRepeater.DataSource = dtVisitInfo

        'バインド処理
        Me.AssignmentRepeater.DataBind()

        Dim Assignment As Control
        Dim row As SC3140103DataSet.SC3140103VisitManagementInfoRow

        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String
        '駐車場コード
        Dim strParkingNumber As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START        
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To AssignmentRepeater.Items.Count - 1

            Assignment = AssignmentRepeater.Items(i)
            row = dtVisitInfo(i)

            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME
            '駐車場コード
            strParkingNumber = row.PARKINGCODE
            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK

            '車両登録番号を設定
            CType(Assignment.FindControl("RegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(Assignment.FindControl("CustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(Assignment.FindControl("RepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            '駐車場コードを設定
            CType(Assignment.FindControl("ParkingNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strParkingNumber, C_DEFAULT_CHIP_SPACE)

            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                Assignment.FindControl("RightIcnD").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '   'アイコン表示
            '   Assignment.FindControl("RightIcnI").Visible = True
            'Else
            '   'アイコン非表示
            '   Assignment.FindControl("RightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                Assignment.FindControl("RightIcnM").Visible = True
                Assignment.FindControl("RightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                Assignment.FindControl("RightIcnM").Visible = False
                Assignment.FindControl("RightIcnB").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnM").Visible = False
                Assignment.FindControl("RightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                Assignment.FindControl("RightIcnE").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                Assignment.FindControl("RightIcnT").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnT").Visible = False
            End If

            'P/Lアイコンを設定
            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                Assignment.FindControl("RightIcnP").Visible = True
                Assignment.FindControl("RightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                Assignment.FindControl("RightIcnP").Visible = False
                Assignment.FindControl("RightIcnL").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnP").Visible = False
                Assignment.FindControl("RightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            If strSSCMark.Equals(IconFlagOn) Then
                'アイコン表示
                Assignment.FindControl("RightIcnS").Visible = True
            Else
                'アイコン非表示
                Assignment.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            '予約アイコン文言
            CType(Assignment.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコン文言
            'CType(Assignment.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(Assignment.FindControl("RightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(Assignment.FindControl("RightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(Assignment.FindControl("RightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(Assignment.FindControl("RightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(Assignment.FindControl("RightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(Assignment.FindControl("RightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコン文言
            CType(Assignment.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS


            divDeskDevice = CType(Assignment.FindControl("AssignmentDeskDevice"), HtmlContainerControl)

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実績連番
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)

                '整備受注NO
                .Attributes("orderNo") = row.ORDERNO

                '追加作業承認ID
                .Attributes("approvalId") = String.Empty

                '更新日時
                .Attributes("updatedate") = row.UPDATEDATE.ToString(CultureInfo.CurrentCulture)

                '予約IDチェック
                If Not row.IsREZIDNull Then
                    '予約ID有り

                    '予約ID
                    .Attributes("rezid") = row.REZID.ToString(CultureInfo.CurrentCulture)

                End If

                'クラス名
                .Attributes("class") = ContentsBoder

            End With


            divElapsedTime = DirectCast(Assignment.FindControl("ElapsedTime"), HtmlContainerControl)

            'カウンターエリア設定
            With divElapsedTime

                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup

                '来店日時を設定
                .Attributes(AttributesPropertyLimittime) = CType(row.VISITTIMESTAMP, String)

                .Attributes("Area") = "Assignment"

            End With

        Next

        'データ表示件数を表示する
        Me.AssignmentNumber.Text = _
        AssignmentRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 受付エリアチップ初期設定
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitReception(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                            , ByVal inStaffInfo As StaffContext _
                            , ByVal inPresentTime As Date _
                            , ByVal strRightIcnD As String _
                            , ByVal strRightIcnI As String _
                            , ByVal strRightIcnS As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))


        '受付エリア情報を取得
        Dim dtVisitInfo As SC3140103DataSet.SC3140103VisitManagementInfoDataTable = _
            inSC3140103BusinessLogic.GetReceptionInfo(inStaffInfo.DlrCD, _
                                                      inStaffInfo.BrnCD, _
                                                      inStaffInfo.Account, _
                                                      inPresentTime)

        '受付エリア情報をコントロールにバインドする
        Me.ReceptionRepeater.DataSource = dtVisitInfo

        'バインド処理
        Me.ReceptionRepeater.DataBind()

        Dim reception As Control
        Dim row As SC3140103DataSet.SC3140103VisitManagementInfoRow

        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String
        '駐車場コード
        Dim strParkingNumber As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To ReceptionRepeater.Items.Count - 1

            reception = ReceptionRepeater.Items(i)
            row = dtVisitInfo(i)

            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME
            '駐車場コード
            strParkingNumber = row.PARKINGCODE
            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK


            '車両登録番号を設定
            CType(reception.FindControl("RegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(reception.FindControl("CustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(reception.FindControl("RepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            '駐車場コードを設定
            CType(reception.FindControl("ParkingNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strParkingNumber, C_DEFAULT_CHIP_SPACE)


            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                reception.FindControl("RightIcnD").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '    'アイコン表示
            '    reception.FindControl("RightIcnI").Visible = True
            'Else
            '    'アイコン非表示
            '    reception.FindControl("RightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                reception.FindControl("RightIcnM").Visible = True
                reception.FindControl("RightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                reception.FindControl("RightIcnM").Visible = False
                reception.FindControl("RightIcnB").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnM").Visible = False
                reception.FindControl("RightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                reception.FindControl("RightIcnE").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                reception.FindControl("RightIcnT").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnT").Visible = False
            End If

            'P/Lアイコンを設定
            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                reception.FindControl("RightIcnP").Visible = True
                reception.FindControl("RightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                reception.FindControl("RightIcnP").Visible = False
                reception.FindControl("RightIcnL").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnP").Visible = False
                reception.FindControl("RightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            If strSSCMark.Equals(IconFlagOn) Then
                'アイコン表示
                reception.FindControl("RightIcnS").Visible = True
            Else
                'アイコン非表示
                reception.FindControl("RightIcnS").Visible = False
            End If


            'アイコンの文言設定
            '予約アイコン文言
            CType(reception.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコン文言
            'CType(reception.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(reception.FindControl("RightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(reception.FindControl("RightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(reception.FindControl("RightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(reception.FindControl("RightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(reception.FindControl("RightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(reception.FindControl("RightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコン文言
            CType(reception.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS


            divDeskDevice = CType(reception.FindControl("ReceptionDeskDevice"), HtmlContainerControl)

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実連番
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)

                '整備受注番号
                .Attributes("orderNo") = row.ORDERNO

                '追加作業承認ID
                .Attributes("approvalId") = String.Empty

                '更新日時
                .Attributes("updatedate") = row.UPDATEDATE.ToString(CultureInfo.CurrentCulture)

                '予約IDチェック
                If Not row.IsREZIDNull Then
                    '予約ID有り

                    '予約ID
                    .Attributes("rezid") = row.REZID.ToString(CultureInfo.CurrentCulture)

                End If

                'ここで排他の制御を入れるべき？？(行ロックバージョン)
                '更新日時
                '.Attributes("updatedate") = row.UPDATEDATE.ToString(CultureInfo.CurrentCulture)

                '呼出ステータス
                .Attributes("callStatus") = row.CALLSTATUS

                '仕掛中チェック(チップの色を変化)
                'RO_INFOテーブルにデータがあれば仕掛中
                If row.IsRO_INFO_VISIT_IDNull Then
                    '仕掛中ではない
                    'チップの色を白
                    .Attributes("class") = ContentsBoder
                Else
                    '仕掛中
                    'チップの色を青
                    .Attributes("class") = ContentsBoderAqua
                End If

            End With

        Next

        'データ表示件数を表示する
        Me.ReceptionDeskTipNumber.Text = _
            ReceptionRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 作業中・納車準備・納車作業エリアチップ初期設定
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <param name="inRowStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitMainChip(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                           , ByVal inStaffInfo As StaffContext _
                           , ByVal inPresentTime As Date _
                           , ByVal strRightIcnD As String _
                           , ByVal strRightIcnI As String _
                           , ByVal strRightIcnS As String _
                           , ByVal inRowStanderdLt As StandardLTListRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '追加作業・作業中・納車準備・納車作業エリアチップ情報取得
        Dim dtMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable = _
            inSC3140103BusinessLogic.GetMainChipInfo(inStaffInfo.DlrCD, _
                                                     inStaffInfo.BrnCD, _
                                                     inStaffInfo.Account, _
                                                     inPresentTime)



        '追加作業エリアチップ初期設定
        Me.InitApproval(dtMainChipInfo, strRightIcnD, strRightIcnI, strRightIcnS, inRowStanderdLt)

        '作業中エリアチップ初期設定
        Me.InitWork(dtMainChipInfo, strRightIcnD, strRightIcnI, strRightIcnS)

        '納車準備エリアチップ初期設定
        Me.InitPreparation(dtMainChipInfo, strRightIcnD, strRightIcnI, strRightIcnS, inRowStanderdLt)

        '納車作業エリアチップ初期設定
        Me.InitDelivery(dtMainChipInfo, strRightIcnD, strRightIcnI, strRightIcnS, inRowStanderdLt)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加承認エリアチップ初期設定
    ''' </summary>
    ''' <param name="inDtMainChipInfo">チップ情報</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <param name="inRowStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    Private Sub InitApproval(ByVal inDtMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable _
                           , ByVal strRightIcnD As String _
                           , ByVal strRightIcnI As String _
                           , ByVal strRightIcnS As String _
                           , ByVal inRowStanderdLt As StandardLTListRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '追加承認エリア情報をコントロールにバインドする
        Me.ApprovalRepeater.DataSource = _
            inDtMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture _
                                  , "ADDAPPROVAL_FLG = '{0}'" _
                                  , C_APPROVAL_STATUS_ON) _
                                  , "RO_CHECK_DATETIME ASC, ASSIGNTIMESTAMP ASC")


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} APPROVALAREA_COUNT = {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ApprovalRepeater.Items.Count))

        'バインド処理
        Me.ApprovalRepeater.DataBind()


        Dim rowMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoRow() = _
            DirectCast(ApprovalRepeater.DataSource, SC3140103DataSet.SC3140103MainChipInfoRow())

        Dim approval As Control
        Dim row As SC3140103DataSet.SC3140103MainChipInfoRow

        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String
        '起票テクニシャン
        Dim strChargeTechnician As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To ApprovalRepeater.Items.Count - 1

            approval = ApprovalRepeater.Items(i)
            row = rowMainChipInfo(i)

            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME
            '起票テクニシャン
            strChargeTechnician = row.LAST_TC_NAME
            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK


            '車両登録番号を設定
            CType(approval.FindControl("ApprovalRegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(approval.FindControl("ApprovalCustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(approval.FindControl("ApprovalRepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            '起票テクニシャンを設定
            CType(approval.FindControl("ApprovalChargeTechnician"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)


            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                approval.FindControl("RightIcnD").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '    'アイコン表示
            '    approval.FindControl("RightIcnI").Visible = True
            'Else
            '    'アイコン非表示
            '    approval.FindControl("RightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                approval.FindControl("RightIcnM").Visible = True
                approval.FindControl("RightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                approval.FindControl("RightIcnM").Visible = False
                approval.FindControl("RightIcnB").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnM").Visible = False
                approval.FindControl("RightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                approval.FindControl("RightIcnE").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                approval.FindControl("RightIcnT").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnT").Visible = False
            End If

            ' P/Lマーク表示
            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                approval.FindControl("RightIcnP").Visible = True
                approval.FindControl("RightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                approval.FindControl("RightIcnP").Visible = False
                approval.FindControl("RightIcnL").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnP").Visible = False
                approval.FindControl("RightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            If strSSCMark.Equals(IconFlagOn) Then
                'アイコン表示
                approval.FindControl("RightIcnS").Visible = True
            Else
                'アイコン非表示
                approval.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            '予約アイコン文言
            CType(approval.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'JDPアイコン文言
            'CType(approval.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(approval.FindControl("RightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(approval.FindControl("RightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(approval.FindControl("RightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(approval.FindControl("RightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(approval.FindControl("RightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(approval.FindControl("RightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコン文言
            CType(approval.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(approval.FindControl("ApprovalDeskDevice"), HtmlContainerControl)

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実績連番
                .Attributes("visitNo") = row.VISIT_ID.ToString(CultureInfo.CurrentCulture)

                '整備受注NO
                .Attributes("orderNo") = row.RO_NUM

                '追加作業承認ID
                .Attributes("approvalId") = row.RO_SEQ.ToString(CultureInfo.CurrentCulture)

                'ここで排他の制御を入れるべき？？(行ロックバージョン)
                '更新日時
                '.Attributes("updatedate") = row.UPDATEDATE.ToString(CultureInfo.CurrentCulture)

                'クラス名
                .Attributes("class") = ContentsBoder

            End With

            '納車予定日時を設定
            CType(approval.FindControl("ApprovalDeliveryPlanTime"), HtmlContainerControl) _
                .InnerHtml = Me.SetDateTimeToString(row.SCHE_DELI_DATETIME)

            '追加作業の最大枝番を設定
            CType(approval.FindControl("AdditionalWorkNumber"), HtmlContainerControl) _
                .InnerHtml = row.MAX_RO_SEQ

            divElapsedTime = DirectCast(approval.FindControl("ApprovalElapsedTime"), HtmlContainerControl)

            'カウンター&遅れ見込みの計算用データを設定する
            With divElapsedTime

                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup

                'SA承認依頼日時＋追加作業標準時間
                .Attributes(AttributesPropertyLimittime) = CType(row.RO_CHECK_DATETIME.AddMinutes(inRowStanderdLt.ADDWORK_STANDARD_LT), String)

                '納車見込遅れ時刻
                .Attributes(AttributesPropertyOvertime1) = CType(row.LIMIT_DELI_DATETIME, String)

                '納車予定時刻
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '.Attributes(AttributesPropertyOvertime2) = CType(row.SCHE_DELI_DATETIME, String)
                .Attributes(AttributesPropertyOvertime2) = FormatTimeStringToDate(row.SCHE_DELI_DATETIME, Date.MinValue)
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            End With
        Next


        'データ表示件数を表示する
        Me.ApprovalNumber.Text = _
            ApprovalRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 作業中エリアチップ初期設定
    ''' </summary>
    ''' <param name="inDtMainChipInfo">チップ情報</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <remarks></remarks>
    Private Sub InitWork(ByVal inDtMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable _
                       , ByVal strRightIcnD As String _
                       , ByVal strRightIcnI As String _
                       , ByVal strRightIcnS As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '作業中エリアコード
        Dim areaCode As String = CType(ChipArea.Work, String)

        '作業中エリア情報をコントロールにバインドする
        Me.WorkRepeater.DataSource = _
            inDtMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture _
                                  , "DISP_AREA = '{0}'" _
                                  , areaCode) _
                                  , "MAX_SCHE_END_DATETIME ASC, ASSIGNTIMESTAMP ASC")


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} WORKAREA_COUNT = {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , WorkRepeater.Items.Count))


        'バインド処理
        Me.WorkRepeater.DataBind()


        Dim rowMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoRow() = _
            DirectCast(WorkRepeater.DataSource, SC3140103DataSet.SC3140103MainChipInfoRow())


        Dim work As Control
        Dim row As SC3140103DataSet.SC3140103MainChipInfoRow


        '作業完了予定日時
        Dim strCompletionPlanTime As String
        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To WorkRepeater.Items.Count - 1

            work = WorkRepeater.Items(i)
            row = rowMainChipInfo(i)

            '作業完了予定日時
            strCompletionPlanTime = row.MAX_SCHE_END_DATETIME.ToString(CultureInfo.CurrentCulture)
            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME

            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK

            '作業完了予定日時
            CType(work.FindControl("WorkCompletionPlanTime"), HtmlContainerControl) _
                .InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strCompletionPlanTime), ChipArea.Work)
            '車両登録番号を設定
            CType(work.FindControl("WorkRegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(work.FindControl("WorkCustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(work.FindControl("WorkRepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)


            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                work.FindControl("WorkRightIcnD").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '    'アイコン表示
            '    work.FindControl("WorkRightIcnI").Visible = True
            'Else
            '    'アイコン非表示
            '    work.FindControl("WorkRightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                work.FindControl("WorkRightIcnM").Visible = True
                work.FindControl("WorkRightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                work.FindControl("WorkRightIcnM").Visible = False
                work.FindControl("WorkRightIcnB").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnM").Visible = False
                work.FindControl("WorkRightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                work.FindControl("WorkRightIcnE").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                work.FindControl("WorkRightIcnT").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnT").Visible = False
            End If

            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                work.FindControl("WorkRightIcnP").Visible = True
                work.FindControl("WorkRightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                work.FindControl("WorkRightIcnP").Visible = False
                work.FindControl("WorkRightIcnL").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnP").Visible = False
                work.FindControl("WorkRightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            'If work.Equals(IconFlagOn) Then
            If strSSCMark.Equals(IconFlagOn) Then
                '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                'アイコン表示
                work.FindControl("WorkRightIcnS").Visible = True
            Else
                'アイコン非表示
                work.FindControl("WorkRightIcnS").Visible = False
            End If


            'アイコンの文言設定
            '予約アイコン文言
            CType(work.FindControl("WorkRightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコン文言
            'CType(work.FindControl("WorkRightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(work.FindControl("WorkRightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(work.FindControl("WorkRightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(work.FindControl("WorkRightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(work.FindControl("WorkRightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(work.FindControl("WorkRightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(work.FindControl("WorkRightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコン文言
            CType(work.FindControl("WorkRightIcnS"), HtmlContainerControl).InnerText = strRightIcnS


            divDeskDevice = CType(work.FindControl("Working"), HtmlContainerControl)

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実績連番
                .Attributes("visitNo") = row.VISIT_ID.ToString(CultureInfo.CurrentCulture)

                '整備受注NO
                .Attributes("orderNo") = row.RO_NUM

                '追加作業承認ID
                .Attributes("approvalId") = String.Empty


                'ここで排他の制御を入れるべき？？(行ロックバージョン)
                '更新日時
                '.Attributes("updatedate") = row.UPDATEDATE.ToString(CultureInfo.CurrentCulture)


            End With

            '仕掛中の判定
            If StatusInstructionsWait.Equals(row.RO_STATUS) Then
                '仕掛前(チップ白色)

                CType(work.FindControl("WorkDeskDevice"), HtmlContainerControl).Attributes("class") = ContentsBoder
            Else
                '仕掛中(チップ青色)

                CType(work.FindControl("WorkDeskDevice"), HtmlContainerControl).Attributes("class") = ContentsBoderAqua
            End If


            '納車予定日時を設定
            CType(work.FindControl("WorkDeliveryPlanTime"), HtmlContainerControl) _
                .InnerHtml = Me.SetDateTimeToString(row.SCHE_DELI_DATETIME)

            '追加作業が存在するかチェック
            If 0 < row.RO_SEQ Then
                '追加作業が存在する

                '追加作業アイコンを表示
                CType(work.FindControl("WorkAdditionalIcon"), HtmlContainerControl).Visible = True

                '追加作業の最大枝番を設定
                CType(work.FindControl("AdditionalWorkNumber"), HtmlContainerControl) _
                    .InnerHtml = row.MAX_RO_SEQ

            End If


            divElapsedTime = DirectCast(work.FindControl("WorkElapsedTime"), HtmlContainerControl)

            'カウンター&遅れ見込みの計算用データを設定する
            With divElapsedTime

                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup

                '作業終了予定時刻(最後の作業完了日時)
                .Attributes(AttributesPropertyLimittime) = CType(row.MAX_SCHE_END_DATETIME, String)

                '納車見込遅れ時刻
                .Attributes(AttributesPropertyOvertime1) = CType(row.LIMIT_DELI_DATETIME, String)

                '納車予定時刻
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '.Attributes(AttributesPropertyOvertime2) = CType(row.SCHE_DELI_DATETIME, String)
                .Attributes(AttributesPropertyOvertime2) = FormatTimeStringToDate(row.SCHE_DELI_DATETIME, Date.MinValue)
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            End With

        Next


        'データ表示件数を表示する
        Me.WorkNumber.Text = _
            WorkRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 納車準備エリアチップ初期設定
    ''' </summary>
    ''' <param name="inDtMainChipInfo">チップ情報</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <param name="inRowStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    Private Sub InitPreparation(ByVal inDtMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable _
                              , ByVal strRightIcnD As String _
                              , ByVal strRightIcnI As String _
                              , ByVal strRightIcnS As String _
                              , ByVal inRowStanderdLt As StandardLTListRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '納車準備エリアコード
        Dim areaCode As String = CType(ChipArea.Preparation, String)

        '納車準備エリア情報をコントロールにバインドする
        Me.PreparationRepeater.DataSource = _
            inDtMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture _
                                  , "DISP_AREA = '{0}'" _
                                  , areaCode) _
                                  , "SCHE_DELI_DATETIME ASC, ASSIGNTIMESTAMP ASC")


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} PREPARATIONAREA_COUNT = {2} " _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , PreparationRepeater.Items.Count))


        'バインド処理
        Me.PreparationRepeater.DataBind()

        '
        Dim rowMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoRow() = _
            DirectCast(PreparationRepeater.DataSource, SC3140103DataSet.SC3140103MainChipInfoRow())


        Dim preparation As Control
        Dim row As SC3140103DataSet.SC3140103MainChipInfoRow

        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String
        '最終作業テクニシャン
        Dim strChargeTechnician As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To PreparationRepeater.Items.Count - 1

            preparation = PreparationRepeater.Items(i)
            row = rowMainChipInfo(i)

            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME
            '最終作業テクニシャン
            strChargeTechnician = row.LAST_TC_NAME

            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK

            '車両登録番号を設定
            CType(preparation.FindControl("PreparationRegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(preparation.FindControl("PreparationCustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(preparation.FindControl("PreparationRepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            '最終作業テクニッシャン
            CType(preparation.FindControl("PreparationChargeTechnician"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)


            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                preparation.FindControl("RightIcnD").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '    'アイコン表示
            '    preparation.FindControl("RightIcnI").Visible = True
            'Else
            '    'アイコン非表示
            '    preparation.FindControl("RightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                preparation.FindControl("RightIcnM").Visible = True
                preparation.FindControl("RightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                preparation.FindControl("RightIcnM").Visible = False
                preparation.FindControl("RightIcnB").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnM").Visible = False
                preparation.FindControl("RightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                preparation.FindControl("RightIcnE").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                preparation.FindControl("RightIcnT").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnT").Visible = False
            End If

            'P/Lアイコンを設定
            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                preparation.FindControl("RightIcnP").Visible = True
                preparation.FindControl("RightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                preparation.FindControl("RightIcnP").Visible = False
                preparation.FindControl("RightIcnL").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnP").Visible = False
                preparation.FindControl("RightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            If strSSCMark.Equals(IconFlagOn) Then
                'アイコン表示
                preparation.FindControl("RightIcnS").Visible = True
            Else
                'アイコン非表示
                preparation.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            '予約アイコン文言
            CType(preparation.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコン文言
            'CType(preparation.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(preparation.FindControl("RightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(preparation.FindControl("RightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(preparation.FindControl("RightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(preparation.FindControl("RightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(preparation.FindControl("RightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(preparation.FindControl("RightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコン文言
            CType(preparation.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(preparation.FindControl("PreparationDeskDevice"), HtmlContainerControl)

            '標準時間
            Dim addMinutes As Long = 0

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実績連番
                .Attributes("visitNo") = row.VISIT_ID.ToString(CultureInfo.CurrentCulture)

                '整備受注NO
                .Attributes("orderNo") = row.RO_NUM

                '追加作業承認ID
                .Attributes("approvalId") = String.Empty

                '仕掛中チェック & 標準時間の設定
                'サービスステータスチェック
                If ServiceStatusDropOff.Equals(row.SVC_STATUS) OrElse _
                   ServiceStatusWaitDelivery.Equals(row.SVC_STATUS) Then
                    'サービスステータス（11：預かり中）、サービスステータス（12：納車待ち）

                    '仕掛中(チップ青色)
                    .Attributes("class") = ContentsBoderAqua

                    '標準時間の設定
                    '納車準備標準時間を設定
                    addMinutes = inRowStanderdLt.DELIVERYPRE_STANDARD_LT

                Else
                    '上記以外
                    '洗車チェック
                    If WashNeedFlagTrue.Equals(row.CARWASH_NEED_FLG) Then

                        '洗車有り

                        '洗車が開始されているかチェック
                        If row.WASH_RSLT_START_DATETIME = Date.MinValue Then
                            '洗車が開始されていない

                            '洗車が開始されていない場合でも清算書が印刷されていない場合は仕掛中にする
                            '清算書印刷チェック
                            If StatusDeliveryWork.Equals(row.RO_STATUS) Then
                                '清算書が印刷されている

                                '仕掛中(チップ青色)
                                .Attributes("class") = ContentsBoderAqua

                            Else
                                '清算書が印刷されていない

                                '仕掛前(チップ白色)
                                .Attributes("class") = ContentsBoder

                            End If

                        Else
                            '洗車開始中

                            '仕掛中(チップ青色)
                            .Attributes("class") = ContentsBoderAqua

                        End If


                        '標準時間の設定
                        '洗車標準時間と納車標準時間を比較して時間が長いほうを設定する
                        If inRowStanderdLt.WASHTIME < inRowStanderdLt.DELIVERYPRE_STANDARD_LT Then
                            '納車準備標準時間が大きい

                            '納車標準時間を設定する
                            addMinutes = inRowStanderdLt.DELIVERYPRE_STANDARD_LT

                        Else
                            '洗車標準時間が大きい

                            '洗車標準時間を設定する
                            addMinutes = inRowStanderdLt.WASHTIME

                        End If

                    Else

                        '洗車無し
                        '仕掛中(チップ青色)
                        .Attributes("class") = ContentsBoderAqua

                        '標準時間の設定
                        '納車準備標準時間を設定
                        addMinutes = inRowStanderdLt.DELIVERYPRE_STANDARD_LT

                    End If

                End If

            End With


            '納車予定時刻
            CType(preparation.FindControl("PreparationDeliveryPlanTime"), HtmlContainerControl) _
                .InnerHtml = Me.SetDateTimeToString(row.SCHE_DELI_DATETIME)

            '追加作業アイコンを表示チェック
            '追加作業が存在するかチェック
            If 0 < row.RO_SEQ Then
                '追加作業が存在する

                '追加作業アイコンを表示
                CType(preparation.FindControl("PreparationAdditionalIcon"), HtmlContainerControl).Visible = True

                '追加作業の最大枝番を設定
                CType(preparation.FindControl("AdditionalWorkNumber"), HtmlContainerControl) _
                    .InnerHtml = row.MAX_RO_SEQ

            End If

            '清算書アイコンチェック
            'ROステータスが85：納車作業中の場合は清算書が発行されているので非表示
            '清算書が未発行の場合は表示
            If Not StatusDeliveryWork.Equals(row.RO_STATUS) Then
                '清算書未発行

                '清算書アイコンを表示
                CType(preparation.FindControl("InVoiceIcon"), HtmlContainerControl).Visible = True

            End If


            '洗車アイコンチェック
            '洗車必要かつ洗車が終了しているか確認
            If WashNeedFlagTrue.Equals(row.CARWASH_NEED_FLG) _
                AndAlso row.WASH_RSLT_END_DATETIME = Date.MinValue Then
                '洗車が終了していない

                '洗車アイコンを表示
                CType(preparation.FindControl("WashIcon"), HtmlContainerControl).Visible = True

            End If


            divElapsedTime = _
                DirectCast(preparation.FindControl("PreparationElapsedTime"), HtmlContainerControl)

            'カウンター&遅れ見込みの計算用データを設定する
            With divElapsedTime

                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup

                '完成検査完了時刻＋納車準備標準時間　または　洗車標準時間
                .Attributes(AttributesPropertyLimittime) = CType(row.MAX_INSPECTION_DATE.AddMinutes(addMinutes), String)

                '納車見込遅れ時刻
                .Attributes(AttributesPropertyOvertime1) = CType(row.LIMIT_DELI_DATETIME, String)

                '納車予定時刻
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '.Attributes(AttributesPropertyOvertime2) = CType(row.SCHE_DELI_DATETIME, String)
                .Attributes(AttributesPropertyOvertime2) = FormatTimeStringToDate(row.SCHE_DELI_DATETIME, Date.MinValue)
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            End With

        Next


        'データ表示件数を表示する
        Me.PreparationNumber.Text = _
            PreparationRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 納車作業エリアチップ初期設定
    ''' </summary>
    ''' <param name="inDtMainChipInfo">チップ情報</param>
    ''' <param name="strRightIcnD">予約マーク 文言</param>
    ''' <param name="strRightIcnI">JDP調査対象客マーク 文言</param>
    ''' <param name="strRightIcnS">SSCマーク 文言</param>
    ''' <param name="inRowStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    Private Sub InitDelivery(ByVal inDtMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable _
                           , ByVal strRightIcnD As String _
                           , ByVal strRightIcnI As String _
                           , ByVal strRightIcnS As String _
                           , ByVal inRowStanderdLt As StandardLTListRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        '納車作業エリアコード
        Dim areaCode As String = CType(ChipArea.Delivery, String)

        '納車作業エリア情報をコントロールにバインドする
        Me.DeliveryRepeater.DataSource = _
            inDtMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture _
                                  , "DISP_AREA = '{0}'" _
                                  , areaCode) _
                                  , "SCHE_DELI_DATETIME ASC, ASSIGNTIMESTAMP ASC")


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} DELIVERYAREA_COUNT = {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , DeliveryRepeater.Items.Count))


        'バインド処理
        Me.DeliveryRepeater.DataBind()


        Dim rowMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoRow() = _
            DirectCast(DeliveryRepeater.DataSource, SC3140103DataSet.SC3140103MainChipInfoRow())


        Dim delivery As Control
        Dim row As SC3140103DataSet.SC3140103MainChipInfoRow

        '車両登録番号
        Dim strRegistrationNumber As String
        '顧客氏名
        Dim strCustomerName As String
        '代表整備項目
        Dim strRepresentativeWarehousing As String
        '最終作業テクニシャン
        Dim strChargeTechnician As String

        '予約アイコンフラグ
        Dim strReserveMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'スマイル年間保守フラグ
        Dim strMBMark As String
        '延長保守フラグ
        Dim strEMark As String
        'テレマ会員フラグ
        Dim strTMark As String
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        'JDPアイコンフラグ
        Dim strJDPMark As String
        'SSCアイコンフラグ
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl

        ' 各チップにデータを設定する
        For i = 0 To DeliveryRepeater.Items.Count - 1

            delivery = DeliveryRepeater.Items(i)
            row = rowMainChipInfo(i)

            '車両登録番号
            strRegistrationNumber = row.VCLREGNO
            '顧客氏名
            strCustomerName = row.NAME
            '代表整備項目
            strRepresentativeWarehousing = row.MERCHANDISENAME
            '最終作業テクニシャン
            strChargeTechnician = row.LAST_TC_NAME

            '予約アイコンフラグ
            strReserveMark = row.REZ_MARK
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'スマイル年間保守フラグ
            strMBMark = row.SML_AMC_FLG
            '延長保守フラグ
            strEMark = row.EW_FLG
            'テレマ会員フラグ
            strTMark = row.TLM_MBR_FLG
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'JDPアイコンフラグ
            strJDPMark = row.JDP_MARK
            'SSCアイコンフラグ
            strSSCMark = row.SSC_MARK

            '車両登録番号を設定
            CType(delivery.FindControl("DeliveryRegistrationNumber"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            '顧客氏名を設定
            CType(delivery.FindControl("DeliveryCustomerName"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            '代表整備項目を設定
            CType(delivery.FindControl("DeliveryRepresentativeWarehousing"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            '最終作業テクニッシャン
            CType(delivery.FindControl("DeliveryChargeTechnician"), HtmlContainerControl) _
                .InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)


            '予約アイコンを設定
            If strReserveMark.Equals(IconFlagOn) Then
                'アイコン表示
                delivery.FindControl("RightIcnD").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnD").Visible = False
            End If

            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            ''JDPアイコンを設定
            'If strJDPMark.Equals(IconFlagOn) Then
            '    'アイコン表示
            '    delivery.FindControl("RightIcnI").Visible = True
            'Else
            '    'アイコン非表示
            '    delivery.FindControl("RightIcnI").Visible = False
            'End If

            'M/Bアイコンを設定
            If IconFlagOn.Equals(strMBMark) Then
                'Mアイコン表示
                delivery.FindControl("RightIcnM").Visible = True
                delivery.FindControl("RightIcnB").Visible = False
            ElseIf IconFlagOn2.Equals(strMBMark) Then
                'Bアイコン表示
                delivery.FindControl("RightIcnM").Visible = False
                delivery.FindControl("RightIcnB").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnM").Visible = False
                delivery.FindControl("RightIcnB").Visible = False
            End If

            'Eアイコンを設定
            If IconFlagOn.Equals(strEMark) Then
                'アイコン表示
                delivery.FindControl("RightIcnE").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnE").Visible = False
            End If

            'Tアイコンを設定
            If IconFlagOn.Equals(strTMark) Then
                'アイコン表示
                delivery.FindControl("RightIcnT").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnT").Visible = False
            End If

            'P/Lマークを設定
            If IconFlagOn.Equals(strJDPMark) Then
                'Pアイコン表示
                delivery.FindControl("RightIcnP").Visible = True
                delivery.FindControl("RightIcnL").Visible = False
            ElseIf IconFlagOn2.Equals(strJDPMark) Then
                'Lアイコン表示
                delivery.FindControl("RightIcnP").Visible = False
                delivery.FindControl("RightIcnL").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnP").Visible = False
                delivery.FindControl("RightIcnL").Visible = False
            End If
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'SSCアイコンを設定
            If strSSCMark.Equals(IconFlagOn) Then
                'アイコン表示
                delivery.FindControl("RightIcnS").Visible = True
            Else
                'アイコン非表示
                delivery.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            '予約アイコン文言
            CType(delivery.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'JDPアイコン文言
            'CType(delivery.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            'Mアイコン文言
            CType(delivery.FindControl("RightIcnM"), HtmlContainerControl).InnerText = strRightIcnM
            'Bアイコン文言
            CType(delivery.FindControl("RightIcnB"), HtmlContainerControl).InnerText = strRightIcnB
            'Eアイコン文言
            CType(delivery.FindControl("RightIcnE"), HtmlContainerControl).InnerText = strRightIcnE
            'Tアイコン文言
            CType(delivery.FindControl("RightIcnT"), HtmlContainerControl).InnerText = strRightIcnT
            'Pアイコン文言
            CType(delivery.FindControl("RightIcnP"), HtmlContainerControl).InnerText = strRightIcnP
            'Lアイコン文言
            CType(delivery.FindControl("RightIcnL"), HtmlContainerControl).InnerText = strRightIcnL
            ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
            'SSCアイコン文言
            CType(delivery.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS


            divDeskDevice = CType(delivery.FindControl("DeliveryDeskDevice"), HtmlContainerControl)

            '必要な情報をタグに追加
            With divDeskDevice

                '来店実績連番
                .Attributes("visitNo") = row.VISIT_ID.ToString(CultureInfo.CurrentCulture)

                '整備受注NO
                .Attributes("orderNo") = row.RO_NUM

                '追加作業承認ID
                .Attributes("approvalId") = String.Empty

                '仕掛中(チップ青色)
                .Attributes("class") = ContentsBoderAqua

            End With


            '納車予定時刻
            CType(delivery.FindControl("DeliveryDeliveryPlanTime"), HtmlContainerControl) _
                .InnerHtml = Me.SetDateTimeToString(row.SCHE_DELI_DATETIME)


            divElapsedTime = _
                DirectCast(delivery.FindControl("DeliveryElapsedTime"), HtmlContainerControl)

            'カウンター&遅れ見込みの計算用データを設定する
            With divElapsedTime

                .Attributes(AttributesPropertyName) = AttributesPropertyProcgroup

                '清算書印刷時刻＋納車作業標準時間
                .Attributes(AttributesPropertyLimittime) = CType(row.INVOICE_PRINT_DATETIME.AddMinutes(inRowStanderdLt.DELIVERYWR_STANDARD_LT), String)

                '納車見込遅れ時刻
                .Attributes(AttributesPropertyOvertime1) = CType(row.LIMIT_DELI_DATETIME, String)

                '納車予定時刻
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '.Attributes(AttributesPropertyOvertime2) = CType(row.SCHE_DELI_DATETIME, String)
                .Attributes(AttributesPropertyOvertime2) = FormatTimeStringToDate(row.SCHE_DELI_DATETIME, Date.MinValue)
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

            End With

        Next

        'データ表示件数を表示する
        Me.DeliveryNumber.Text = _
            DeliveryRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 事前準備件数を設定
    ''' </summary>
    ''' <param name="inSC3140103BusinessLogic">BusinessLogic</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <remarks></remarks>
    Private Sub SetAdvancePreparationsCount(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                                          , ByVal inStaffInfo As StaffContext _
                                          , ByVal inPresentTime As Date)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))


        '事前準備表示件数を取得
        Dim countList As List(Of Long) = inSC3140103BusinessLogic.GetAdvancePreparationsCount(inStaffInfo, inPresentTime)

        '当日分件数
        Dim todayCount As Long = countList(0)
        '翌日分件数
        Dim nextCount As Long = countList(1)

        '取得した事前準備件数を事前準備ボタンに反映
        'ボタンステータス(ボタンの表示色)
        Dim buttonStatus As String
        '件数
        Dim countResult As String

        '件数のチェック
        If todayCount = 0 Then
            '当日分無し

            If nextCount = 0 Then
                '当日・翌日とも0件なら件数表示なし

                'ボタンの表示色(透明？)
                buttonStatus = "0"

                '件数表示無し
                countResult = " "
            Else
                '翌日分のみ件数有り

                'ボタンの表示色(薄水色)
                buttonStatus = "1"

                '表示件数設定(翌日分のみ)
                countResult = nextCount.ToString(CultureInfo.CurrentCulture)
            End If
        Else
            '当日分有り

            'ボタンの表示色(赤色)
            buttonStatus = "2"

            '表示件数設定(当日＋翌日)
            countResult = (todayCount + nextCount).ToString(CultureInfo.CurrentCulture)

        End If

        '件数をHiddenに設定
        '件数設定
        Me.AdvancePreparationsCntHidden.Value = countResult

        'ボタンの色の設定
        Me.AdvancePreparationsColorHidden.Value = buttonStatus

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "チップ詳細情報取得"

    ''' <summary>
    ''' チップ詳細に情報を設定
    ''' </summary>
    ''' <param name="inChipDetail">チップ詳細情報</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inSelectChipArea">選択チップ表示エリア</param>
    ''' <remarks>
    ''' </remarks>
    ''' <histiry>
    ''' </histiry>
    Private Sub SetDetailPopupInfo(ByVal inChipDetail As ChipDetail _
                                 , ByVal inStaffInfo As StaffContext _
                                 , ByVal inSelectChipArea As Long)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START SELECTCHIPARE = {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inSelectChipArea))

        '文言の取得「--:--」
        Dim wordNullDateTime As String = WebWordUtility.GetWord(APPLICATIONID, 42)

        'ステータスを設定
        Me.IconStatsLabel.Text = inChipDetail.Status

        '納車予定時刻を設定
        Me.DeliveryTimeLabel.Text = Me.SetNullToString(inChipDetail.DeliveryPlanDate, wordNullDateTime)

        '納車予定変更回数のチェック
        If 0 < inChipDetail.DeliveryPlanDateUpdateCount Then
            '変更回数が1件以上の場合

            'スラッシュの表示
            Me.FixSlashLabel.Visible = True

            '納車予定時刻変更回数を設定
            Me.ChangeCountLabel.Text = WebWordUtility.GetWord(APPLICATIONID, 39) _
                                      .Replace("%1", CType(inChipDetail.DeliveryPlanDateUpdateCount, String))

        Else
            ' 変更回数が0件の場合

            'スラッシュの非表示
            Me.FixSlashLabel.Visible = False

        End If

        '変更回数をHiddenに格納(変更回数エリアを表示・非表示の判定用)
        Me.HiddenDeliveryPlanUpdateCount.Value = CType(inChipDetail.DeliveryPlanDateUpdateCount, String)

        '納車見込時刻を設定
        Me.DeliveryEstimateLabel.Text = Me.SetNullToString(inChipDetail.DeliveryHopeDate, wordNullDateTime)

        '受付モニターが使用されているかチェック
        '選択チップが受付エリアかチェック
        If UseReceptionFlag.Equals(Me.UseReception.Value) _
            AndAlso CType(ChipArea.Reception, Long) = inSelectChipArea Then

            '受付モニターを使用する場合
            '来店者呼出エリアを表示
            Me.VisitCustomer.Style("display") = "block"

            '呼出場所を非表示
            Me.CallPlaceTable.Style("display") = "none"

            '来店者呼出しエリア設定
            Me.SetVisitArea(inChipDetail)

        Else
            '上記以外
            '受付待ちモニターを使用しない場合

            '来店者呼出エリアを非表示
            Me.VisitCustomer.Style("display") = "none"

            '使用しない場合で振当待ちエリアまたは受付エリアの場合は
            '呼出場所を詳細下部に表示させる
            '振当待ちエリアまたは受付エリアをチェック
            If CType(ChipArea.Reception, Long) = inSelectChipArea _
                  OrElse CType(ChipArea.Assignment, Long) = inSelectChipArea Then
                '振当待ちエリアまたは受付エリアの場合

                '呼出場所を表示
                Me.CallPlaceTable.Style("display") = ""

                '呼出場所を設定
                Me.DetailsCallPlace02.Text = inChipDetail.CallPlace


            Else
                '上記以外のエリア

                '呼出場所を非表示
                Me.CallPlaceTable.Style("display") = "none"

            End If

        End If


        '車両登録No.を設定
        Me.DetailsRegistrationNumber.Text = Me.SetNullToString(inChipDetail.VehicleRegNo, String.Empty)

        'プロビンスチェック
        If inChipDetail.RegisterAreaName Is Nothing Then
            'プロビンス情報無し

            'プロビンス領域非表示
            Me.DetailsProvince.Visible = False
        Else
            'データがある場合

            'プロビンス領域表示
            Me.DetailsProvince.Visible = True

            'プロビンスを設定
            Me.DetailsProvince.Text = inChipDetail.RegisterAreaName

        End If

        '予約マークを設定
        If DETAILS_MARK_ACTIVE.Equals(inChipDetail.WalkIn) Then

            '予約マーク表示
            Me.DetailsRightIconD.Visible = True
        Else

            '予約マーク非表示
            Me.DetailsRightIconD.Visible = False
        End If
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''JDP調査対象客マークを設定
        'If DETAILS_MARK_ACTIVE.Equals(inChipDetail.JdpType) Then

        '    'JDP調査対象客マーク表示
        '    Me.DetailsRightIconI.Visible = True
        'Else

        '    'JDP調査対象客マーク非表示
        '    Me.DetailsRightIconI.Visible = False
        'End If

        'P/Lマークを設定
        If DETAILS_MARK_ACTIVE.Equals(inChipDetail.JdpType) Then
            'Pマーク表示
            Me.DetailsRightIconP.Visible = True
            'Lマーク非表示
            Me.DetailsRightIconL.Visible = False
        ElseIf DETAILS_MARK_ACTIVE_2.Equals(inChipDetail.JdpType) Then
            'Lマーク表示
            Me.DetailsRightIconL.Visible = True
            'Pマーク非表示
            Me.DetailsRightIconP.Visible = False
        Else
            'Pマーク非表示
            Me.DetailsRightIconP.Visible = False
            'Lマーク非表示
            Me.DetailsRightIconL.Visible = False
        End If
        ' 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        'SSCマークを設定
        If DETAILS_MARK_ACTIVE.Equals(inChipDetail.SscType) Then

            'SSCマーク表示
            Me.DetailsRightIconS.Visible = True
        Else

            'SSCマーク非表示
            Me.DetailsRightIconS.Visible = False
        End If

        '車種名を設定
        Me.DetailsCarModel.Text = Me.SetNullToString(inChipDetail.VehicleName, String.Empty)

        If inChipDetail.Grade Is Nothing Then
            'グレード情報無し

            'グレード領域非表示
            Me.DetailsModel.Visible = False
        Else
            'データがある場合

            'グレード領域表示
            Me.DetailsModel.Visible = True

            'グレードを設定
            Me.DetailsModel.Text = inChipDetail.Grade

        End If

        'グレードを設定
        Me.DetailsModel.Text = Me.SetNullToString(inChipDetail.Grade, String.Empty)

        '顧客名を設定
        Me.DetailsCustomerName.Text = Me.SetNullToString(inChipDetail.CustomerName, String.Empty)

        '電話番号を設定
        Me.DetailsPhoneNumber.Text = Me.SetNullToString(inChipDetail.TelNo, String.Empty)

        '携帯電話番号を設定
        Me.DetailsMobileNumber.Text = Me.SetNullToString(inChipDetail.Mobile, String.Empty)

        '整備内容を設定
        Me.DetailsServiceContents.Text = Me.SetNullToString(inChipDetail.MerchandiseName, String.Empty)

        '待ち方を設定
        If REZ_RECEPTION_WAITING.Equals(inChipDetail.ReserveReception) Then
            '店内待ちの場合

            Me.DetailsWaitPlan.Text = WebWordUtility.GetWord(APPLICATIONID, 44)

        ElseIf REZ_RECEPTION_DROP_OFF.Equals(inChipDetail.ReserveReception) Then
            '店外待ちの場合

            Me.DetailsWaitPlan.Text = WebWordUtility.GetWord(APPLICATIONID, 45)

        Else
            '上記以外

            '空文字を設定
            Me.DetailsWaitPlan.Text = String.Empty

        End If

        'TcatのモデルコードをI-crop側が未取引客の場合に必要のため格納
        Me.HiddenVehicleModel.Value = Me.SetNullToString(inChipDetail.Model, String.Empty)


        '追加作業エリアかつ起票者がテクニシャンかチェック
        If ChipArea.Approval = inSelectChipArea _
            AndAlso TechnicianIssuance.Equals(inChipDetail.ReissueVouchers) Then
            '追加作業エリアかつ起票者がテクニシャンの場合
            '追加作業エリアのチップ詳細で起票者がTCの場合は起票者ストールを表示する

            '起票者エリアを表示
            Me.DrawerTable.Style("display") = ""

            '起票者名を設定
            Me.DetailsDrawer.Text = inChipDetail.AddAccountName

        Else
            '上記以外

            '起票者エリアを非表示
            Me.DrawerTable.Style("display") = "none"
            Me.DetailsDrawer.Text = ""

        End If


        '中断理由エリア設定処理
        Me.SetStopReason(inChipDetail)

        '納車時刻変更履歴エリア設定処理
        Me.SetDeliveryChage(inChipDetail, inStaffInfo)

        '行ロックバージョンの設定
        Me.DetailRowLockVersion.Value = CType(inChipDetail.ServiceinLockVersion, String)

        '標準ボタン・サブボタンの設定
        'チップエリアチェック
        If ChipArea.Approval = inSelectChipArea Then
            '追加作業エリア

            Me.SetSubMenuButtonApproval(inChipDetail.StatusRight)
        Else
            '上記以外

            Me.SetSubMenuButton(inChipDetail.StatusLeft)
        End If


        '受付エリアと振当待ちエリアの場合、Deleteボタン表示
        '受付エリアと振当待ちエリアでボタン文言の変更
        If ChipArea.Reception = inSelectChipArea Then
            '受付エリアの場合

            '受付エリアは「差戻しボタン」
            'ボタン文言の設定
            Me.ButtonDeleteWord01.Visible = True
            Me.ButtonDeleteWord02.Visible = False


        ElseIf ChipArea.Assignment = inSelectChipArea Then
            '振当待ちエリアの場合

            '振当待ちエリアは「退店ボタン」
            'ボタン文言の設定
            Me.ButtonDeleteWord01.Visible = False
            Me.ButtonDeleteWord02.Visible = True

        End If

        '更新日時ラベル
        Me.DetailsVisitUpdateDateLabel.Text = String.Empty

        'チップ詳細エリアをUPDATE
        Me.ContentUpdatePanelDetail.Update()


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 来店管理エリア設定
    ''' </summary>
    ''' <param name="inChipDetail">チップ詳細情報</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Sub SetVisitArea(ByVal inChipDetail As ChipDetail)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '振当待ちエリア・受付エリアはステータスの変化が大きいので
        'チップ表示時に更新日時をセットしてその更新日時を排他制御に使う
        'DB排他制御ため、更新日時を格納する
        'Me.DetailsVisitUpdateDate.Value = inChipDetail.UpdateDate.ToString(CultureInfo.CurrentCulture)

        '呼出No.を設定
        Me.DetailsCallNo.Text = Me.SetNullToString(inChipDetail.CallNO, String.Empty)

        '呼出場所を設定
        Me.DetailsCallPlace.Text = Me.SetNullToString(inChipDetail.CallPlace, String.Empty)

        '来店者氏名を設定
        '来店者氏名のチェック
        If Not String.IsNullOrEmpty(inChipDetail.VisitName) Then
            '来店者氏名有り

            '来店者氏名を設定
            Me.DetailsVisitName.Text = inChipDetail.VisitName
        Else
            '来店者氏名無し

            '顧客氏名を設定
            Me.DetailsVisitName.Text = inChipDetail.CustomerName
        End If

        '来店者電話番号を設定
        '来店者電話番号のチェック
        If Not String.IsNullOrEmpty(inChipDetail.VisitTelNO) Then
            '来店者電話番号有り

            '来店者電話番号を設定
            Me.DetailsVisitTelno.Text = inChipDetail.VisitTelNO

        ElseIf Not String.IsNullOrEmpty(inChipDetail.Mobile) Then
            '来店者電話番号が無く、携帯番号がある場合

            '携帯番号を設定
            Me.DetailsVisitTelno.Text = inChipDetail.Mobile

        ElseIf Not String.IsNullOrEmpty(inChipDetail.TelNo) Then
            '来店者電話番号が無く、電話番号がある場合

            '電話番号を設定
            Me.DetailsVisitTelno.Text = inChipDetail.TelNo
        Else
            '上記以外の場合

            '値無し(空)をセットする
            Me.DetailsVisitTelno.Text = String.Empty
        End If


        '呼出ステータスによって表示ボタンを変更
        If inChipDetail.CallStatus = CALLSTATUS_CALLING Then
            '呼出中の場合

            '呼出中キャンセルボタンを表示
            Me.BtnCALLCancel.Style("display") = "block"
            Me.BtnCALL.Style("display") = "none"

            '呼出場所を読取り専用に設定
            Me.DetailsCallPlace.ReadOnly = True

        ElseIf inChipDetail.CallStatus = CALLSTATUS_NOTCALL Then
            '未呼出の場合

            '呼出中ボタンを表示
            Me.BtnCALLCancel.Style("display") = "none"
            Me.BtnCALL.Style("display") = "block"

            '呼出場所を編集可能に設定
            Me.DetailsCallPlace.ReadOnly = False

        Else
            '以外の場合

            Me.BtnCALLCancel.Style("display") = "none"
            Me.BtnCALL.Style("display") = "none"
            Me.DetailsCallPlace.ReadOnly = True

        End If


        '呼出ステータスを設定
        Me.DetailsCallStatus.Value = inChipDetail.CallStatus


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 中断理由エリア設定
    ''' </summary>
    ''' <param name="inChipDetail">チップ詳細情報</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Sub SetStopReason(ByVal inChipDetail As ChipDetail)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '中断理由が存在するかチェック
        If inChipDetail.StopReasonList IsNot Nothing _
            AndAlso 0 < inChipDetail.StopReasonList.Count Then
            '中断理由が存在する場合

            '中断理由用のテーブル定義
            Using dtInterruptionInfo As New SC3140103InterruptionInfoDataTable

                '中断理由分ループ
                For Each item As StopReason In inChipDetail.StopReasonList

                    '新しいROWを作成
                    Dim rowInterruptionInfo As SC3140103InterruptionInfoRow = _
                        DirectCast(dtInterruptionInfo.NewRow(), SC3140103InterruptionInfoRow)

                    '中断理由を設定
                    rowInterruptionInfo.InterruptionCause = item.ResultStatus

                    '中断注釈を設定
                    rowInterruptionInfo.InterruptionDetails = item.StopMemo

                    'テーブルに追加
                    dtInterruptionInfo.AddSC3140103InterruptionInfoRow(rowInterruptionInfo)

                Next

                '中断理由情報をリピーターアイテムにバインド
                Me.InterruptionCauseRepeater.DataSource = dtInterruptionInfo
                Me.InterruptionCauseRepeater.DataBind()

            End Using
        Else
            '中断理由がない場合

            '中断理由リピーターアイテムにNothingを設定
            Me.InterruptionCauseRepeater.DataSource = Nothing
            Me.InterruptionCauseRepeater.DataBind()

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 納車時刻変更履歴エリア設定
    ''' </summary>
    ''' <param name="inChipDetail">チップ詳細情報</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Sub SetDeliveryChage(ByVal inChipDetail As ChipDetail _
                               , ByVal inStaffInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '納車時刻変更履歴が存在するかチェック
        If inChipDetail.DeliveryChgList IsNot Nothing _
            AndAlso 0 < inChipDetail.DeliveryChgList.Count Then

            '納車納車時刻変更履歴が存在する場合

            '納車納車時刻変更履歴テーブル定義
            Using dtDeliChange As New SC3140103DeliveryTimeChangeLogInfoDataTable

                '現在日時の取得
                Dim nowDate As Date = DateTimeFunc.Now(inStaffInfo.DlrCD)

                '納車納車時刻変更履歴情報分ループ
                For Each item As DeliveryChg In inChipDetail.DeliveryChgList

                    '新しいROWを作成
                    Dim rowDeliChange As SC3140103DeliveryTimeChangeLogInfoRow = _
                        DirectCast(dtDeliChange.NewRow(), SC3140103DeliveryTimeChangeLogInfoRow)

                    '変更前納車予定時刻を設定
                    rowDeliChange.ChangeFromTime = Me.SetDateTimeToStringDetail(item.OldDeliveryHopeDate, nowDate)

                    '変更後納車予定時刻を設定
                    rowDeliChange.ChangeToTime = Me.SetDateTimeToStringDetail(item.NewDeliveryHopeDate, nowDate)

                    '変更日時を設定
                    rowDeliChange.UpdateTime = Me.SetDateTimeToStringDetail(item.ChangeDate, nowDate)

                    '変更理由を設定
                    rowDeliChange.UpdatePretext = item.ChangeReason

                    'テーブルに追加
                    dtDeliChange.AddSC3140103DeliveryTimeChangeLogInfoRow(rowDeliChange)

                Next


                '納車納車時刻変更履歴情報をリピーターアイテムにバインド
                Me.ChangeTimeRepeater.DataSource = dtDeliChange
                Me.ChangeTimeRepeater.DataBind()


            End Using
        Else
            '納車納車時刻変更履歴がない場合

            '納車納車時刻変更履歴リピーターアイテムにNothingを設定
            Me.ChangeTimeRepeater.DataSource = Nothing
            Me.ChangeTimeRepeater.DataBind()

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 標準ボタン・サブボタンの設定(追加作業エリア用)
    ''' </summary>
    ''' <param name="statusCode">ステータスコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正
    ''' </history>
    Private Sub SetSubMenuButtonApproval(ByVal statusCode As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:statusCode = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , statusCode))

        '★★★チップ詳細ステータス★★★
        Select Case (statusCode)

            Case StatusCodeRight205,
                 StatusCodeRight206,
                 StatusCodeRight207

                '★★★SA見積確定待ち・SA追加作業起票中・お客様承認待ち★★★
                '標準ボタン(追加作業確認)

                '顧客情報サブボタン(非表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOn

                '2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正 START
                '標準ボタンエリア非表示
                Me.DetailBottomBox.Style("display") = "block"
                '2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正 END

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(追加作業確認)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 106)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

            Case Else
                '上記以外の場合
                '標準ボタン・サブボタン非表示

                '顧客情報サブボタン(非表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOff

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア非表示
                Me.DetailBottomBox.Style("display") = "none"

                '通常標準ボタン(非表示)
                Me.DetailbottomDiv.Visible = False

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                'ボタンテキスト設定(空文字) 
                Me.DetailbottomButton.Text = String.Empty
                Me.DetailbottomButton02.Text = String.Empty
                Me.DetailbottomButton03.Text = String.Empty

        End Select

        '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
        Me.DetailbottomButton04.Visible = False
        '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' 標準ボタン・サブボタンの設定
    ''' </summary>
    ''' <param name="statusCode">ステータスコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SetSubMenuButton(ByVal statusCode As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:statusCode = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , statusCode))

        '★★★チップ詳細ステータス★★★
        Select Case (statusCode)

            Case StatusCodeLeft135  '★★★SA振当て待ち(新規お客様登録)★★★

                '顧客情報サブボタン(非表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOff

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '新規顧客登録機能使用チェック
                If UseNewCustomerFlagOn.Equals(Me.UseNewCustomer.Value) Then
                    '新規顧客登録を使用する場合

                    '標準ボタンエリア表示
                    Me.DetailBottomBox.Style("display") = "block"

                    '通常標準ボタン(表示)
                    Me.DetailbottomDiv.Visible = True

                    'ボタンテキスト設定(新規お客様登録)
                    Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 52)

                    'ボタンステータスをタグに設定
                    Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                    '2ボタン標準ボタンエリア(非表示)
                    Me.DetailbottomDiv02.Visible = False
                    Me.DetailbottomDiv03.Visible = False

                Else
                    '新規顧客登録を使用しない場合

                    ''標準ボタンエリア非表示
                    Me.DetailBottomBox.Style("display") = "none"

                End If

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case StatusCodeLeft136  '★★★SA振当て待ち(RO作成)★★★

                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア表示
                Me.DetailBottomBox.Style("display") = "block"

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(RO作成)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 53)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case StatusCodeLeft137  '★★★SA振当て待ち(RO編集)★★★

                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア表示
                Me.DetailBottomBox.Style("display") = "block"

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(RO編集)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 54)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case Else
                '受付エリア以降の場合

                '標準ボタン・サブボタンの設定(SA振当以降)
                Me.SetSubMenuButtonReception(statusCode)

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 標準ボタン・サブボタンの設定(受付エリア以降)
    ''' </summary>
    ''' <param name="statusCode">ステータスコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SetSubMenuButtonReception(ByVal statusCode As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:statusCode = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , statusCode))

        '★★★チップ詳細ステータス★★★
        Select Case (statusCode)

            Case StatusCodeLeft104  '★★★新規お客様登録待ち(新規お客様登録)★★★

                '顧客情報サブボタン(非表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOff

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '新規顧客登録機能仕様チェック
                If UseNewCustomerFlagOn.Equals(Me.UseNewCustomer.Value) Then
                    '新規顧客登録を使用する場合

                    '標準ボタンエリア表示
                    Me.DetailBottomBox.Style("display") = "block"

                    '通常標準ボタン(表示)
                    Me.DetailbottomDiv.Visible = True

                    'ボタンテキスト設定(新規お客様登録)
                    Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 52)

                    'ボタンステータスをタグに設定
                    Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                    '2ボタン標準ボタンエリア(非表示)
                    Me.DetailbottomDiv02.Visible = False
                    Me.DetailbottomDiv03.Visible = False

                Else
                    '新規顧客登録を使用しない場合

                    ''標準ボタンエリア非表示
                    Me.DetailBottomBox.Style("display") = "none"


                End If

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case StatusCodeLeft105  '★★★RO未作成(RO作成)★★★

                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア表示
                Me.DetailBottomBox.Style("display") = "block"

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(RO作成)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 53)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case StatusCodeLeft106  '★★★RO作成中(RO編集)★★★

                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア表示
                Me.DetailBottomBox.Style("display") = "block"

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(RO作成)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 54)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case Else
                '上記以外の場合

                '標準ボタン・サブボタンの設定(作業中エリア以降)
                Me.SetSubMenuButtonWrok(statusCode)

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 標準ボタン・サブボタンの設定(作業中エリア以降)
    ''' </summary>
    ''' <param name="statusCode">ステータスコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発
    ''' 2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正 
    ''' </history>
    Private Sub SetSubMenuButtonWrok(ByVal statusCode As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:statusCode = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , statusCode))

        '★★★チップ詳細ステータス★★★
        Select Case (statusCode)

            Case StatusCodeLeft107,
                 StatusCodeLeft108,
                 StatusCodeLeft109,
                 StatusCodeLeft110,
                 StatusCodeLeft111,
                 StatusCodeLeft112,
                 StatusCodeLeft113,
                 StatusCodeLeft114,
                 StatusCodeLeft115,
                 StatusCodeLeft116,
                 StatusCodeLeft117,
                 StatusCodeLeft118,
                 StatusCodeLeft119,
                 StatusCodeLeft120,
                 StatusCodeLeft122,
                 StatusCodeLeft124,
                 StatusCodeLeft125,
                 StatusCodeLeft126
                '★★★着工指示待ち・着工指示待ち/部品準備待ち・着工指示待ち/部品準備中・着工指示待ち/部品準備済み★★★
                '★★★作業開始待ち・着工指示済み/部品準備待ち・着工指示済み/部品準備中・作業中★★★
                '★★★中断中・完成検査待ち・洗車待ち/CloseJob待ち・洗車中/CloseJob待ち・洗車完了/CloseJob待ち★★★
                '★★★CloseJob待ち・中断中・完成検査待ち★★★
                '標準ボタン(追加作業起票/RO編集)


                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOn

                '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 START
                ''標準ボタンエリア表示
                'Me.DetailBottomBox.Style("display") = "block"

                ''通常標準ボタン(非表示)
                'Me.DetailbottomDiv.Visible = False

                ''ボタンステータスをタグに設定
                'Me.DetailbottomButton02.Attributes(SubMenuButtonStatusCalss) = statusCode
                'Me.DetailbottomButton03.Attributes(SubMenuButtonStatusCalss) = statusCode

                ''2ボタン標準ボタン(表示)
                ''左側
                'Me.DetailbottomDiv02.Visible = True
                ''左側ボタンテキスト設定(追加作業起票)
                'Me.DetailbottomButton02.Text = WebWordUtility.GetWord(APPLICATIONID, 104)

                ''右側
                'Me.DetailbottomDiv03.Visible = True
                ''右側ボタンテキスト設定(RO編集)
                'Me.DetailbottomButton03.Text = WebWordUtility.GetWord(APPLICATIONID, 54)

                '追加作業起票ボタン蓋締めフラグのチェック
                If AddWorkCloseTypeOn.Equals(Me.AddWorkCloseType.Value) Then
                    '蓋締めする場合
                    '追加作業起票ボタンを表示しないレイアウトに設定

                    '標準ボタンエリア表示
                    Me.DetailBottomBox.Style("display") = "block"

                    '通常標準ボタン(表示)
                    Me.DetailbottomDiv.Visible = True

                    'ボタンテキスト設定(RO編集)
                    Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 54)

                    'ボタンステータスをタグに設定
                    Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                    '2ボタン標準ボタンエリア(非表示)
                    Me.DetailbottomDiv02.Visible = False
                    Me.DetailbottomDiv03.Visible = False

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START

                    ' チップステータスが洗車待ち/CloseJob待ち・洗車中/CloseJob待ち・洗車完了/CloseJob待ち・CloseJob待ちの場合
                    If StatusCodeLeft117.Equals(statusCode) Or
                        StatusCodeLeft118.Equals(statusCode) Or
                        StatusCodeLeft119.Equals(statusCode) Or
                        StatusCodeLeft120.Equals(statusCode) Then

                        '標準ボタン4(表示)
                        Me.DetailbottomDiv04.Visible = True

                        'ボタンテキスト設定(チェックシート印刷)
                        Me.DetailbottomButton04.Text = WebWordUtility.GetWord(APPLICATIONID, 105)

                        'ボタンステータスをタグに設定
                        Me.DetailbottomButton04.Attributes(SubMenuButtonStatusCalss) = statusCode

                    Else

                        '標準ボタン4(非表示)
                        Me.DetailbottomDiv04.Visible = False

                    End If

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

                Else
                    '上記以外の場合
                    '追加作業起票ボタンを表示するレイアウトに設定

                    '2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正 START

                    '標準ボタンエリア表示
                    Me.DetailBottomBox.Style("display") = "block"

                    '2014/09/14 TMEJ 小澤 BTS不具合対応 標準ボタンの制御修正 END

                    '通常標準ボタン(非表示)
                    Me.DetailbottomDiv.Visible = False

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START

                    ' チップステータスが洗車待ち/CloseJob待ち・洗車中/CloseJob待ち・洗車完了/CloseJob待ち・CloseJob待ちの場合
                    If StatusCodeLeft117.Equals(statusCode) Or
                        StatusCodeLeft118.Equals(statusCode) Or
                        StatusCodeLeft119.Equals(statusCode) Or
                        StatusCodeLeft120.Equals(statusCode) Then

                        '標準ボタン4(表示)
                        Me.DetailbottomDiv04.Visible = True

                        'ボタンテキスト設定(チェックシート印刷)
                        Me.DetailbottomButton04.Text = WebWordUtility.GetWord(APPLICATIONID, 105)

                        'ボタンステータスをタグに設定
                        Me.DetailbottomButton04.Attributes(SubMenuButtonStatusCalss) = statusCode

                    Else

                        '標準ボタン4(非表示)
                        Me.DetailbottomDiv04.Visible = False

                    End If

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

                    'ボタンステータスをタグに設定
                    Me.DetailbottomButton02.Attributes(SubMenuButtonStatusCalss) = statusCode
                    Me.DetailbottomButton03.Attributes(SubMenuButtonStatusCalss) = statusCode

                    '2ボタン標準ボタン(表示)
                    '左側
                    Me.DetailbottomDiv02.Visible = True
                    '左側ボタンテキスト設定(追加作業起票)
                    Me.DetailbottomButton02.Text = WebWordUtility.GetWord(APPLICATIONID, 104)

                    '右側
                    Me.DetailbottomDiv03.Visible = True
                    '右側ボタンテキスト設定(RO編集)
                    Me.DetailbottomButton03.Text = WebWordUtility.GetWord(APPLICATIONID, 54)

                End If
                '2014/04/24 TMEJ 小澤 サービスタブレットＤＭＳ連携追加開発 END

            Case Else
                '上記以外の場合

                '標準ボタン・サブボタンの設定(SA振当以降納車準備エリア以降)
                Me.SetSubMenuButtonDelivery(statusCode)

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 標準ボタン・サブボタンの設定(納車準備エリア以降)
    ''' </summary>
    ''' <param name="statusCode">ステータスコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SetSubMenuButtonDelivery(ByVal statusCode As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:statusCode = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , statusCode))

        '★★★チップ詳細ステータス★★★
        Select Case (statusCode)

            Case StatusCodeLeft121,
                 StatusCodeLeft132,
                 StatusCodeLeft133
                '★★★洗車待ち/CloseJob済み・洗車中/CloseJob済み・納車待ち★★★


                '顧客情報サブボタン(表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOn

                'RO参照サブボタン(表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOn

                '標準ボタンエリア表示
                Me.DetailBottomBox.Style("display") = "block"

                '通常標準ボタン(表示)
                Me.DetailbottomDiv.Visible = True

                'ボタンテキスト設定(チェックシート印刷)
                Me.DetailbottomButton.Text = WebWordUtility.GetWord(APPLICATIONID, 105)

                'ボタンステータスをタグに設定
                Me.DetailbottomButton.Attributes(SubMenuButtonStatusCalss) = statusCode

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

            Case Else
                '上記以外の場合
                '標準ボタン・サブボタン非表示

                '顧客情報サブボタン(非表示)
                Me.HiddenDetailsCustomerButtonStatus.Value = SubMenuButtonOff

                'RO参照サブボタン(非表示)
                Me.HiddenDetailsROButtonStatus.Value = SubMenuButtonOff

                '標準ボタンエリア非表示
                Me.DetailBottomBox.Style("display") = "none"

                '通常標準ボタン(非表示)
                Me.DetailbottomDiv.Visible = False

                '2ボタン標準ボタンエリア(非表示)
                Me.DetailbottomDiv02.Visible = False
                Me.DetailbottomDiv03.Visible = False

                'ボタンテキスト設定(空文字) 
                Me.DetailbottomButton.Text = String.Empty
                Me.DetailbottomButton02.Text = String.Empty
                Me.DetailbottomButton03.Text = String.Empty

                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Me.DetailbottomDiv04.Visible = False
                '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub


#End Region

#Region "チップ詳細標準ボタン処理"

#Region "来店テーブルステータス更新処理"

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ' ''' <summary>
    ' ''' 来店テーブルステータス更新処理
    ' ''' 振当待ちエリア：SA振当状態にして、呼出ステータスを「2：呼出完了にする」
    ' ''' 受付エリア：呼出ステータスを「2：呼出完了にする」
    ' ''' </summary>
    ' ''' <param name="inSC3140103BusinessLogic">SC3140103BusinessLogi</param>
    ' ''' <param name="inStaffInfo">スタッフ情報</param>
    ' ''' <param name="inVisitSeq">来店実績連番</param>
    ' ''' <param name="inDetailArea">表示エリア</param>
    ' ''' <param name="inPresentTime">現在時間</param>
    ' ''' <param name="inAssignFlg">振当て登録フラグ(True：振当て処理を行う　False:振当て処理を行わない)</param>
    ' ''' <remarks>
    ' ''' </remarks>
    ' ''' <histiry>
    ' ''' </histiry>
    'Private Function UpdateVisitManageStatus(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
    '                                       , ByVal inStaffInfo As StaffContext _
    '                                       , ByVal inVisitSeq As Long _
    '                                       , ByVal inDetailArea As Long _
    '                                       , ByVal inPresentTime As Date _
    '                                       , ByVal inAssignFlg As Boolean) As Long
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
    ''' <summary>
    ''' 来店テーブルステータス更新処理
    ''' 振当待ちエリア：SA振当状態にして、呼出ステータスを「2：呼出完了にする」
    ''' 受付エリア：呼出ステータスを「2：呼出完了にする」
    ''' </summary>
    ''' <param name="inSC3140103BusinessLogic">SC3140103BusinessLogi</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inDetailArea">表示エリア</param>
    ''' <param name="inPresentTime">現在時間</param>
    ''' <param name="inAssignFlg">振当て登録フラグ(True：振当て処理を行う　False:振当て処理を行わない)</param>
    ''' <param name="inRezId">サービス入庫ID</param>
    ''' <remarks>
    ''' </remarks>
    ''' <histiry>
    ''' </histiry>
    Private Function UpdateVisitManageStatus(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                                           , ByVal inStaffInfo As StaffContext _
                                           , ByVal inVisitSeq As Long _
                                           , ByVal inDetailArea As Long _
                                           , ByVal inPresentTime As Date _
                                           , ByVal inAssignFlg As Boolean _
                                           , ByVal inRezId As Decimal) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START VISITSEQ = {2} DETAILAREA = {3} PRESENTTIME = {4} ASSIGNFLG = {5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitSeq, inDetailArea, inPresentTime, inAssignFlg))

        '更新結果
        Dim returnCode As Long = RET_SUCCESS

        '表示エリアのチェック
        If inDetailArea = CType(ChipArea.Reception, Long) _
            OrElse inDetailArea = CType(ChipArea.Assignment, Long) Then
            '振当待ちエリアまたは受付エリアの場合

            '来店テーブルのステータス更新処理
            '振当待ちエリア：SA振当状態にして、呼出ステータスを「3：呼出完了にする」
            '受付エリア：呼出ステータスを「2：呼出完了にする」


            'チップ表示時更新日時
            Dim updateDate As Date = Date.MinValue

            '排他制御用のチップ表示時更新日時の取得
            If Not Date.TryParse(Me.DetailsVisitUpdateDate.Value, updateDate) Then
                '更新日時の取得に失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} DetailsVisitUpdateDate IS NOTHING" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return RET_EXCLUSION

            End If

            '振当処理を行うかチェックかつ表示エリアのチェック
            If inAssignFlg AndAlso _
                inDetailArea = CType(ChipArea.Assignment, Long) Then
                '振当待ちエリアの場合

                'SA振当て登録処理
                returnCode = inSC3140103BusinessLogic.RegisterSA(inDetailArea, _
                                                                 inVisitSeq, _
                                                                 updateDate, _
                                                                 inStaffInfo, _
                                                                 inPresentTime)

                'SA振当てで更新日時が変更になったので最新に変更
                updateDate = inPresentTime

                'SA振当てで表示エリアが変更になったので最新に更新(受付エリアに変更)
                inDetailArea = CType(ChipArea.Reception, Long)

            End If

            '更新チェックかつ表示エリアのチェック
            If returnCode = RET_SUCCESS AndAlso _
               inAssignFlg AndAlso _
               inDetailArea = CType(ChipArea.Reception, Long) Then
                '更新が成功している場合または更新していない場合
                '受付エリアの場合

                'チップの呼出ステータスの取得
                Dim callStatus As String = Me.SetNullToString(Me.DetailsCallStatus.Value)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} CALLSTATUS = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , callStatus))

                '呼出ステータスのチェック
                If Not callStatus.Equals(CType(CALLSTATUS_CALLED, String)) Then
                    '呼出ステータスが「2：呼出完了」以外の場合

                    '呼出ステータスを「2：呼出完了」に変更する
                    returnCode = inSC3140103BusinessLogic.CallCompleted(inVisitSeq, _
                                                                        updateDate, _
                                                                        inStaffInfo, _
                                                                        inPresentTime)

                    '更新処理結果
                    If returnCode = RET_SUCCESS Then
                        '更新成功
                        'Push処理

                        'OperationCodeリスト
                        Dim operationCodeList As New List(Of Long)

                        'OperationCodeリストに権限"52"：SVRを設定
                        operationCodeList.Add(Operation.SVR)

                        'OperationCodeリストに権限"9"：SAを設定
                        operationCodeList.Add(Operation.SA)

                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                        ''OperationCodeリストに権限"55"：CTを設定
                        'operationCodeList.Add(Operation.CT)

                        ''OperationCodeリストに権限"62"：CHTを設定
                        'operationCodeList.Add(Operation.CHT)
                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                        'ユーザーステータス取得
                        Using user As New IC3810601BusinessLogic

                            'ユーザーステータス取得処理
                            '各権限の全ユーザー情報取得
                            Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                                user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                                             inStaffInfo.BrnCD, _
                                                             operationCodeList)

                            '各権限オンラインユーザー分ループ
                            For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                                '各権限に対するPush処理
                                inSC3140103BusinessLogic.SendPushServer(userRow.OPERATIONCODE, inStaffInfo, userRow.ACCOUNT, PushFlag0)

                            Next

                        End Using

                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                        ' CT/CHTへのPush処理
                        SendPushServerCTAndCHT(inSC3140103BusinessLogic, inStaffInfo, inRezId)
                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                    End If
                End If
            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END RETURNCODE = {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , returnCode))

        Return returnCode

    End Function

#End Region

#Region "事前準備用処理"

    ''' <summary>
    '''顧客車両情報(基幹側)設定
    ''' </summary>
    ''' <param name="rowReserve">事前準備予約情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function SetCustomerInfo(ByVal rowReserve As SC3140103AdvancePreparationsReserveInfoRow) _
                                     As SC3140103AdvancePreparationsReserveInfoRow


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , LOG_START))


        'チップ詳細で取得しているモデルコード(車両型式)が存在するか確認
        If Not String.IsNullOrEmpty(Me.HiddenVehicleModel.Value.Trim) Then
            'モデルコード(車両型式)が存在する

            'チップ詳細で取得しているモデルコードを予約情報に設定
            rowReserve.MODELCODE = Me.HiddenVehicleModel.Value.Trim

        End If

        'チップ詳細で取得している電話番号が存在するか確認
        If Not String.IsNullOrEmpty(Me.DetailsPhoneNumber.Text.Trim) Then
            '電話番号が存在する

            'チップ詳細で取得している電話番号を予約情報に設定
            rowReserve.TELNO = Me.DetailsPhoneNumber.Text.Trim

        End If

        'チップ詳細で取得している携帯電話番号が存在するか確認
        If Not String.IsNullOrEmpty(Me.DetailsMobileNumber.Text.Trim) Then
            '携帯電話番号が存在する

            'チップ詳細で取得している携帯電話番号を予約情報に設定
            rowReserve.MOBILE = Me.DetailsMobileNumber.Text.Trim

        End If

        'チップ詳細で取得している顧客名が存在するか確認
        If Not String.IsNullOrEmpty(Me.DetailsCustomerName.Text.Trim) Then
            '顧客名が存在する

            'チップ詳細で取得している顧客名を予約情報に設定
            rowReserve.CUSTOMERNAME = Me.DetailsCustomerName.Text.Trim

        End If


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


        Return rowReserve


    End Function

#End Region

#Region "チップ詳細からの画面遷移処理"

    ''' <summary>
    ''' チップ詳細からの画面遷移処理 (ボタン用)
    ''' </summary>
    ''' <param name="inSC3140103Bis">SC3140103BusinessLogic</param>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inButtonStatus">ボタンステータス</param>
    ''' <param name="inEventButtonStatus">2ボタン標準ボタン時の左右どちらが押されたか判定するステータス</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub NextScreenVisitChipDetailButton(ByVal inSC3140103Bis As SC3140103BusinessLogic,
                                                ByVal inDetailArea As Long,
                                                ByVal inButtonStatus As String,
                                                ByVal inEventButtonStatus As String,
                                                ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                                ByVal inPresentTime As Date)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} BUTTONSTATUS = {4} EVENTBUTTONSTATUS = {5} PRESENTTIME = {6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea, inButtonStatus, inEventButtonStatus, inPresentTime))


        '★★★ボタンステータスに応じて遷移先を決定する★★★

        Select Case inButtonStatus

            Case StatusCodeLeft135,
                 StatusCodeLeft104
                '"135":"104"

                '★★★新規顧客登録★★★

                '新規顧客登録画面遷移処理
                Me.ChipDetailNewCustormerButton(inDetailArea, _
                                                inRowVisitInfo)

            Case StatusCodeLeft136,
                 StatusCodeLeft105
                '"136":"105"

                '★★★R/O作成★★★

                'R/O作成画面移処理
                Me.ChipDetailROCreationButton(inSC3140103Bis, _
                                              inDetailArea, _
                                              inRowVisitInfo, _
                                              inPresentTime)

            Case StatusCodeLeft137,
                 StatusCodeLeft106
                '"137":"106"

                '★★★R/O編集★★★

                'R/O編集画面移処理
                Me.ChipDetailROEditButton(inDetailArea, _
                                          inRowVisitInfo)

            Case Else
                '上記以外

                'チップ詳細からの画面遷移処理 (ボタン用作業中エリア以降)
                Me.NextScreenVisitChipDetailWork(inDetailArea, _
                                                 inButtonStatus, _
                                                 inEventButtonStatus, _
                                                 inRowVisitInfo, _
                                                 inPresentTime)

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' チップ詳細からの画面遷移処理 (ボタン用作業中エリア以降)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inButtonStatus">ボタンステータス</param>
    ''' <param name="inEventButtonStatus">2ボタン標準ボタン時の左右どちらが押されたか判定するステータス</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub NextScreenVisitChipDetailWork(ByVal inDetailArea As Long,
                                              ByVal inButtonStatus As String,
                                              ByVal inEventButtonStatus As String,
                                              ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                              ByVal inPresentTime As Date)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} BUTTONSTATUS = {4} EVENTBUTTONSTATUS = {5} PRESENTTIME = {6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea, inButtonStatus, inEventButtonStatus, inPresentTime))


        '★★★ボタンステータスに応じて遷移先を決定する★★★

        Select Case inButtonStatus

            Case StatusCodeLeft107,
                 StatusCodeLeft108,
                 StatusCodeLeft109,
                 StatusCodeLeft110,
                 StatusCodeLeft111,
                 StatusCodeLeft112,
                 StatusCodeLeft113,
                 StatusCodeLeft114,
                 StatusCodeLeft115,
                 StatusCodeLeft116,
                 StatusCodeLeft117,
                 StatusCodeLeft118,
                 StatusCodeLeft119,
                 StatusCodeLeft120,
                 StatusCodeLeft122,
                 StatusCodeLeft124,
                 StatusCodeLeft125,
                 StatusCodeLeft126
                '"107":"108":"109":"110":"111":"112":"113":"114":"115":"116"
                '"117":"118":"119":"120":"122":"124":"125":"126"

                '2ボタン標準ボタン

                '左右どちらのボタンが押されたかチェック
                If DetailbottomLeft.Equals(inEventButtonStatus) Then
                    '左側のボタン

                    '★★★追加作業起票★★★

                    '追加作業起票画面移処理
                    Me.ChipDetailApprovalButton(inDetailArea, _
                                                inRowVisitInfo, _
                                                False)

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                    'Else
                    ''右側のボタン
                ElseIf DetailbottomRight.Equals(inEventButtonStatus) Or Detailbottom3.Equals(inEventButtonStatus) Then
                    '標準ボタン2または標準ボタン3
                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

                    '★★★R/O編集★★★

                    'R/O編集画面移処理
                    Me.ChipDetailROEditButton(inDetailArea, _
                                              inRowVisitInfo)

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START
                Else

                    'チェックシート印刷画面遷移処理
                    Me.ChipDetailCheckSheetButton(inDetailArea, _
                                                    inRowVisitInfo)

                    '2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

                End If


            Case Else
                '上記以外

                'チップ詳細からの画面遷移処理 (ボタン用作業中エリア以降)
                Me.NextScreenVisitChipDetailDelivery(inDetailArea, _
                                                     inButtonStatus, _
                                                     inEventButtonStatus, _
                                                     inRowVisitInfo, _
                                                     inPresentTime)

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' チップ詳細からの画面遷移処理 (ボタン用納車準備エリア以降)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inButtonStatus">ボタンステータス</param>
    ''' <param name="inEventButtonStatus">2ボタン標準ボタン時の左右どちらが押されたか判定するステータス</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub NextScreenVisitChipDetailDelivery(ByVal inDetailArea As Long,
                                                  ByVal inButtonStatus As String,
                                                  ByVal inEventButtonStatus As String,
                                                  ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                                  ByVal inPresentTime As Date)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} BUTTONSTATUS = {4} EVENTBUTTONSTATUS = {5} PRESENTTIME = {6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea, inButtonStatus, inEventButtonStatus, inPresentTime))


        '★★★ボタンステータスに応じて遷移先を決定する★★★

        Select Case inButtonStatus

            Case StatusCodeLeft121,
                 StatusCodeLeft132,
                 StatusCodeLeft133
                '"121":"133":"134"

                '★★★チェックシート印刷★★★

                'チェックシート印刷画面遷移処理
                Me.ChipDetailCheckSheetButton(inDetailArea, _
                                              inRowVisitInfo)


            Case StatusCodeRight205,
                 StatusCodeRight206,
                 StatusCodeRight207
                '追加作業ボタンステータス
                '"205":"206":"207"

                '★★★追加作業確認★★★

                '追加作業承認画面移処理
                Me.ChipDetailApprovalButton(inDetailArea, _
                                            inRowVisitInfo, _
                                            True)

            Case Else
                '上記以外

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:BUTTONSTATUS={2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , inButtonStatus))

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id917)

                '遷移なし

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub


#End Region

#End Region

#Region "画面遷移メソッド"

#Region "新規顧客登録画面"

    ''' <summary>
    ''' 新規顧客登録画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <history>
    ''' </history>
    Private Sub ChipDetailNewCustormerButton(ByVal inDetailArea As Long,
                                             ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))



        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' 新規顧客登録画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub RedirectCustomerNew()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '●●新規顧客登録機能完成時に作成する●●
        '●●セッション値を設定する●●

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_END))

    End Sub

#End Region

#Region "R/O作成画面"

    ''' <summary>
    ''' R/O作成画面遷移処理(パラメータチェック：RO_INFO作成)
    ''' </summary>
    ''' <param name="inSC3140103Bis">SC3140103BusinessLogic</param>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub ChipDetailROCreationButton(ByVal inSC3140103Bis As SC3140103BusinessLogic,
                                           ByVal inDetailArea As Long,
                                           ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                           ByVal inPresentTime As Date)


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))

        '処理結果
        Dim resultCode As Long = RET_SUCCESS

        '必須項目チェック

        'DMS販売店コードのチェック
        If inRowVisitInfo.IsDMSDLRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSDLRCD.Trim) Then
            'DMS販売店コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSDLRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMS店舗コードのチェック
        If inRowVisitInfo.IsDMSSTRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSSTRCD.Trim) Then
            'DMS店舗コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSSTRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSアカウントのチェック
        If inRowVisitInfo.IsDMSACCOUNTNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSACCOUNT.Trim) Then
            'DMSアカウントが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSACCOUNT = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSIDのチェック
        If inRowVisitInfo.IsDMSIDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
            'DMSIDが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'RO_INFO作成処理
        resultCode = inSC3140103Bis.CreateROInfo(inDetailArea, inRowVisitInfo, inPresentTime)

        '処理結果チェック
        If resultCode <> RET_SUCCESS Then
            '処理失敗

            'エラーログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:CreateROInfo = NG  RESULTCODE = {2} VISITSEQ = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , resultCode, inRowVisitInfo.VISITSEQ))


            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id908)

            '処理中断
            Exit Sub

        End If

        'R/O作成画面遷移処理(パラメータ設定)
        Me.RedirectOrderNew(inSC3140103Bis, _
                            inDetailArea, _
                            inRowVisitInfo, _
                            inPresentTime)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' R/O作成画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inSC3140103Bis">SC3140103BusinessLogic</param>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub RedirectOrderNew(ByVal inSC3140103Bis As SC3140103BusinessLogic,
                                 ByVal inDetailArea As Long,
                                 ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                 ByVal inPresentTime As Date)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))


        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, inRowVisitInfo.DMSDLRCD)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, inRowVisitInfo.DMSSTRCD)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, inRowVisitInfo.DMSACCOUNT)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, inRowVisitInfo.VISITSEQ)

        'DMS予約ID
        '値チェック
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam05, inRowVisitInfo.DMS_JOB_DTL_ID)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)

        End If

        'RO
        Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
        'RO_JOB_SEQ
        Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        'DMSID
        Me.SetValue(ScreenPos.Next, SessionParam10, inRowVisitInfo.DMSID)

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

        'ContactParson
        '値チェック
        If Not inRowVisitInfo.IsVISITNAMENull Then
            '値有り
            Me.SetValue(ScreenPos.Next, SessionParam11, HttpUtility.UrlEncode(inRowVisitInfo.VISITNAME))

        Else
            '値無し
            Me.SetValue(ScreenPos.Next, SessionParam11, String.Empty)

        End If

        'ContactTEL
        '値チェック
        If Not inRowVisitInfo.IsVISITTELNONull Then
            '値有り
            Me.SetValue(ScreenPos.Next, SessionParam12, HttpUtility.UrlEncode(inRowVisitInfo.VISITTELNO))

        Else
            '値無し
            Me.SetValue(ScreenPos.Next, SessionParam12, String.Empty)

        End If

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDERNEW)


        '予約情報ポップアップ出力フラグ（True: 出力／False: 未出力）
        Dim blnPopUpReserveFlg As Boolean = False

        '表示エリアチェック
        If Not (CType(ChipArea.AdvancePreparations, Long).Equals(inDetailArea)) Then
            '事前準備エリア以外のチップの場合（ = メインチップの場合）
            '予約ポップアップを表示する

            '翌日の日付を取得
            Dim nextDate As Date = DateAdd(DateInterval.Day, 1, inPresentTime)

            '予約情報の取得基準日を「翌日」に設定
            'Dim strBaseDate = DateTimeFunc.FormatDate(CONVERTDATE_YMD, nextDate)
            Dim strBaseDate = String.Format(CultureInfo.CurrentCulture, "{0:yyyyMMdd}", nextDate)

            '予約情報ポップアップへデータをセット
            Dim dt As SC3140103ReserveListDataTable

            '予約情報ポップアップ用の一覧データ取得
            dt = inSC3140103Bis.GetPopupReservationList(inRowVisitInfo, _
                                                        strBaseDate)

            '予約情報チェック
            If Not IsNothing(dt) Then
                '翌日以降の該当予約が存在する場合（※翌日を含む）

                '予約ポップアップ出力設定
                blnPopUpReserveFlg = True

                '画面作成処理
                'バインド
                Me.ReserveListRepeater.DataSource = dt
                Me.ReserveListRepeater.DataBind()

                '[OK」ボタンの文言を設定
                Me.PopUpReserveListFooterButton.Text = WebWordUtility.GetWord(APPLICATIONID, 95)

                ' 画面表示処理
                Dim script As New StringBuilder
                '顧客詳細ボタン非表示
                script.AppendLine("$('#DetailButtonLeft').css('display', 'none');")
                'R/O参照ボタン非表示
                script.AppendLine("$('#DetailButtonCenter').css('display', 'none');")
                '追加作業ボタン非表示
                script.AppendLine("$('#DetailButtonRight').css('display', 'none');")
                'クルクル非表示（左）
                script.AppendLine("$('#IconLoadingPopup').css('display', 'none');")
                'スクロール設定
                script.AppendLine("$('#PopUpReserveListContents').fingerScroll();")
                '表示設定
                script.AppendLine("$('#PopUpReserveList').fadeIn(100);")
                'OKボタンタップイベント設定
                script.AppendLine("$('#PopUpReserveListFooterButton').bind('click', function (e) {$('#PopUpReserveList').fadeOut(100); ")
                'タイマーセット
                script.AppendLine("commonRefreshTimer(RefreshDisplay); });")
                'クライアントにスクリプト設定
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PopUpReserve", script.ToString, True)
                '更新
                Me.UpdatePanel2.Update()

            End If

        End If

        ' 予約情報ポップアップを出力しない場合
        ' 　（予約情報ポップアップを出力する場合は、
        ' 　　ポップアップ内のOKボタン押下(detailReservePopupButton_Clickイベント)により、R/O作成画面に遷移を行う。）
        If Not (blnPopUpReserveFlg) Then

            '基幹画面連携用フレーム呼出処理
            Me.ScreenTransition()

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#End Region

#Region "R/O編集画面"

    ''' <summary>
    ''' R/O編集画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub ChipDetailROEditButton(ByVal inDetailArea As Long,
                                       ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))

        '必須項目チェック

        'DMS販売店コードのチェック
        If inRowVisitInfo.IsDMSDLRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSDLRCD.Trim) Then
            'DMS販売店コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSDLRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMS店舗コードのチェック
        If inRowVisitInfo.IsDMSSTRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSSTRCD.Trim) Then
            'DMS店舗コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSSTRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSアカウントのチェック
        If inRowVisitInfo.IsDMSACCOUNTNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSACCOUNT.Trim) Then
            'DMSアカウントが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSACCOUNT = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSIDのチェック
        If inRowVisitInfo.IsDMSIDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
            'DMSIDが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If


        'R/O編集遷移処理(パラメータ設定)
        Me.RedirectOrderEdit(inDetailArea, inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' R/O編集画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Sub RedirectOrderEdit(ByVal inDetailArea As Long,
                                  ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} DETAILAREA = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDetailArea))


        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, inRowVisitInfo.DMSDLRCD)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, inRowVisitInfo.DMSSTRCD)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, inRowVisitInfo.DMSACCOUNT)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, inRowVisitInfo.VISITSEQ)

        'DMS予約ID
        '値チェック
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam05, inRowVisitInfo.DMS_JOB_DTL_ID)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)

        End If

        'RO
        '値チェック
        If Not inRowVisitInfo.IsRO_NUMNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam06, inRowVisitInfo.RO_NUM)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)

        End If

        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        Me.SetValue(ScreenPos.Next, SessionParam07, ParentJobSeq)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        'DMSID
        Me.SetValue(ScreenPos.Next, SessionParam10, inRowVisitInfo.DMSID)

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

        'ContactParson
        '値チェック
        If Not inRowVisitInfo.IsVISITNAMENull Then
            '値有り
            Me.SetValue(ScreenPos.Next, SessionParam11, HttpUtility.UrlEncode(inRowVisitInfo.VISITNAME))

        Else
            '値無し
            Me.SetValue(ScreenPos.Next, SessionParam11, String.Empty)

        End If

        'ContactTEL
        '値チェック
        If Not inRowVisitInfo.IsVISITTELNONull Then
            '値有り
            Me.SetValue(ScreenPos.Next, SessionParam12, HttpUtility.UrlEncode(inRowVisitInfo.VISITTELNO))

        Else
            '値無し
            Me.SetValue(ScreenPos.Next, SessionParam12, String.Empty)

        End If

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDERNEW)


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#End Region

#Region "追加作業起票・追加作業承認画面"

    ''' <summary>
    ''' 追加作業起票・追加作業承認画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inEditModeFlag">編集フラグ(追加作業の起票か編集か判定する)True：編集モード</param>
    ''' <remarks></remarks>
    Private Sub ChipDetailApprovalButton(ByVal inDetailArea As Long,
                                         ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow, _
                                         ByVal inEditModeFlag As Boolean)


        'RO作業連番を取得
        '画面遷移用来店情報では追加作業のRO作業連番を特定できないため
        'チップ初期表示時に保存している値を使用する
        Dim approvalId As String = Me.SetNullToString(Me.DetailsApprovalId.Value)


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} EDITMODEFLAG = {4} APPROVALID = {5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea, inEditModeFlag, approvalId))


        '必須項目チェック

        'DMS販売店コードのチェック
        If inRowVisitInfo.IsDMSDLRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSDLRCD.Trim) Then
            'DMS販売店コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSDLRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMS店舗コードのチェック
        If inRowVisitInfo.IsDMSSTRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSSTRCD.Trim) Then
            'DMS店舗コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSSTRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSアカウントのチェック
        If inRowVisitInfo.IsDMSACCOUNTNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSACCOUNT.Trim) Then
            'DMSアカウントが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSACCOUNT = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSIDのチェック
        If inRowVisitInfo.IsDMSIDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
            'DMSIDが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'ROのチェック
        If inRowVisitInfo.IsRO_NUMNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.RO_NUM.Trim) Then
            'ROが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:RO_NUM = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If


        '編集モード(追加作業編集)かチェック
        If inEditModeFlag Then
            '編集モード

            'RO_JOB_SEQのチェック
            'RO_JOB_SEQをLongに変換できるかチェック
            If String.IsNullOrEmpty(approvalId) _
                OrElse Not Long.TryParse(approvalId, inRowVisitInfo.RO_SEQ) Then
                'RORO_JOB_SEQが存在しない場合またはLongに変換できない場合

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:RO_JOB_SEQ = NOTHING  VISITSEQ = {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , inRowVisitInfo.VISITSEQ))

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                'エラーメッセージ
                Me.ShowMessageBox(MsgID.id917)

                '処理中断
                Exit Sub

            End If

        Else
            '起票モード

            '追加作業が起票できるROステータスかチェック
            Select Case inRowVisitInfo.RO_STATUS

                Case StatusInstructionsWait,
                     StatusWork,
                     StatusDeliveryWait
                    'ROステータス
                    '"50"：着工指示待ち　"60"：作業中　"80"：納車準備
                    '追加作業起票OK

                    'RO_JOB_SEQは起票の際は新たに採番されるためNULLを設定し、セッションには設定しない
                    inRowVisitInfo.SetRO_SEQNull()

                Case Else
                    '上記以外のROステータス

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} ERR:RO_STATUS = NOT STATUS VISITSEQ = {2} VISITSEQ = {3}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , inRowVisitInfo.VISITSEQ, inRowVisitInfo.RO_STATUS))

                    'タイマークリア
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

                    'エラーメッセージ
                    Me.ShowMessageBox(MsgID.id918)

                    '処理中断
                    Exit Sub

            End Select

        End If


        '追加作業承認画面遷移処理(パラメータ設定)
        Me.RedirectApproval(inDetailArea, inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 追加作業承認画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定
    ''' </history>
    Private Sub RedirectApproval(ByVal inDetailArea As Long,
                                 ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} DETAILAREA = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDetailArea))


        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, inRowVisitInfo.DMSDLRCD)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, inRowVisitInfo.DMSSTRCD)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, inRowVisitInfo.DMSACCOUNT)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, inRowVisitInfo.VISITSEQ)

        'DMS予約ID
        '値チェック
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam05, inRowVisitInfo.DMS_JOB_DTL_ID)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)

        End If

        'RO
        Me.SetValue(ScreenPos.Next, SessionParam06, inRowVisitInfo.RO_NUM)

        'RO_JOB_SEQ
        '値チェック
        If Not inRowVisitInfo.IsRO_SEQNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam07, inRowVisitInfo.RO_SEQ)

        Else
            '値無し

            '2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定 START

            'Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)

            Me.SetValue(ScreenPos.Next, SessionParam07, SessionSEQNONew)

            '2014/12/19 TMEJ 小澤 追加作業起票時のパラメータを設定 END

        End If

        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        ''DMSID
        Me.SetValue(ScreenPos.Next, SessionParam10, inRowVisitInfo.DMSID)
        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDERNEW)


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "チェックシート印刷画面"

    ''' <summary>
    ''' チェックシート印刷画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub ChipDetailCheckSheetButton(ByVal inDetailArea As Long,
                                           ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))

        '必須項目チェック

        'RO_NUMのチェック
        If inRowVisitInfo.IsRO_NUMNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.RO_NUM.Trim) Then
            'RO_NUMが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:RO_NUM = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If


        'チェックシート印刷画面遷移処理(パラメータ設定)
        Me.RedirectCheckSheet(inDetailArea, inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' チェックシート印刷画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub RedirectCheckSheet(ByVal inDetailArea As Long,
                                   ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))


        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Space(1))
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Space(1))
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Space(1))
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, inRowVisitInfo.VISITSEQ)

        'DMS予約ID
        '値チェック
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionBASREZID, inRowVisitInfo.DMS_JOB_DTL_ID)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionBASREZID, String.Empty)

        End If

        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, inRowVisitInfo.RO_NUM)
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        Me.SetValue(ScreenPos.Next, SessionSEQNO, ParentJobSeq)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, ReadMode)


        'チェックシート印刷画面に遷移
        Me.RedirectNextScreen(APPLICATIONID_CHECKSHEET)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "R/O参照画面"

    ''' <summary>
    ''' R/O参照画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub ChipDetailOrderDispButton(ByVal inDetailArea As Long,
                                          ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DETAILAREA = {3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDetailArea))

        '必須項目チェック

        'DMS販売店コードのチェック
        If inRowVisitInfo.IsDMSDLRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSDLRCD.Trim) Then
            'DMS販売店コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSDLRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMS店舗コードのチェック
        If inRowVisitInfo.IsDMSSTRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSSTRCD.Trim) Then
            'DMS店舗コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSSTRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSアカウントのチェック
        If inRowVisitInfo.IsDMSACCOUNTNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSACCOUNT.Trim) Then
            'DMSアカウントが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSACCOUNT = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'DMSIDのチェック
        If inRowVisitInfo.IsDMSIDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
            'DMSIDが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        'RO_NUMのチェック
        If inRowVisitInfo.IsRO_NUMNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.RO_NUM.Trim) Then
            'RO_NUMが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:RO_NUM = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If


        'R/O参照画面遷移処理
        Me.RedirectOrderDisp(inDetailArea, inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' R/O参照画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub RedirectOrderDisp(ByVal inDetailArea As Long,
                                  ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} DETAILAREA = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDetailArea))

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, inRowVisitInfo.DMSDLRCD)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, inRowVisitInfo.DMSSTRCD)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, inRowVisitInfo.DMSACCOUNT)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, inRowVisitInfo.VISITSEQ)

        'DMS予約ID
        '値チェック
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '値有り

            Me.SetValue(ScreenPos.Next, SessionParam05, inRowVisitInfo.DMS_JOB_DTL_ID)

        Else
            '値無し

            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)

        End If

        'RO
        Me.SetValue(ScreenPos.Next, SessionParam06, inRowVisitInfo.RO_NUM)
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        Me.SetValue(ScreenPos.Next, SessionParam07, ParentJobSeq)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        'Format
        Me.SetValue(ScreenPos.Next, SessionParam10, PreviewFlag)
        'SVCIN_NUM
        Me.SetValue(ScreenPos.Next, SessionParam11, String.Empty)
        'SVCIN_DealerCode
        Me.SetValue(ScreenPos.Next, SessionParam12, String.Empty)
        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDEROUT)

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#End Region

#Region "全画面共通(基幹画面連携用フレーム呼出処理)"

    ''' <summary>
    ''' 基幹画面連携用フレーム呼出処理
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ScreenTransition()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '基幹画面連携用フレーム呼出
        Me.RedirectNextScreen(APPLICATIONID_FRAMEID)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "顧客詳細画面"

    ''' <summary>
    ''' 顧客詳細画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </history>
    Private Sub ChipDetailCustomerButton(ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '必須項目チェック

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '顧客詳細遷移用に予約の顧客を優先にDMS_CST_CDを取得するように変更

        ''DMSIDのチェック
        'If inRowVisitInfo.IsDMSIDNull _
        '    OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
        '    'DMSIDが存在しない場合

        '    'エラーログ
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , inRowVisitInfo.VISITSEQ))

        '    'タイマークリア
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

        '    'エラーメッセージ
        '    Me.ShowMessageBox(MsgID.id917)

        '    '処理中断
        '    Exit Sub

        'End If

        'DMS_CST_CDのチェック
        If inRowVisitInfo.IsDMS_CST_CDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMS_CST_CD.Trim) Then
            'DMS_CST_CDが存在しない場合

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMS_CST_CD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id917)

            '処理中断
            Exit Sub

        End If

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END


        '顧客詳細画面遷移処理(パラメータ設定)
        Me.RedirectCustomer(inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 顧客詳細画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </history>
    Private Sub RedirectCustomer(ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '次画面遷移パラメータ設定

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '顧客詳細遷移用に予約の顧客を優先にDMS_CST_CDを取得するように変更

        ''DMS予約ID
        'Me.SetValue(ScreenPos.Next, SessionDMSID, inRowVisitInfo.DMSID)

        '基幹顧客コード
        Me.SetValue(ScreenPos.Next, SessionDMSID, inRowVisitInfo.DMS_CST_CD)

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        'VINチェック
        If Not inRowVisitInfo.IsVINNull _
            AndAlso Not String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then

            'VIN
            Me.SetValue(ScreenPos.Next, SessionVIN, inRowVisitInfo.VIN)

        End If


        '顧客詳細画面遷移
        Me.RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "R/O一覧画面"

    ''' <summary>
    ''' R/O一覧画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderList()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3140103BusinessLogicインスタンス
        Using bis As New SC3140103BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = bis.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDERLIST)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


    End Sub

#End Region

#Region "商品訴求コンテンツ画面"

    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectProductsAppealContent()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Space(1))
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Space(1))
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Space(1))
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, String.Empty)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, String.Empty)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, String.Empty)
        'RO_JOB_SEQ            
        Me.SetValue(ScreenPos.Next, SessionSEQNO, String.Empty)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, String.Empty)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, ReadMode)


        '商品訴求コンテンツ画面遷移
        Me.RedirectNextScreen(APPLICATIONID_PRODUCTSAPPEALCONTENT)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


    End Sub

#End Region

#Region "キャンペーン画面"

    ''' <summary>
    ''' キャンペーン画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectCampaign()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3140103BusinessLogicインスタンス
        Using bis As New SC3140103BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = bis.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)

            'ViewMode
            '2014/07/01 TMEJ 丁 　TMT_UAT対応 START
            'Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
            '2014/07/01 TMEJ 丁 　TMT_UAT対応 END

            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_CAMPAIGN)

        End Using


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


    End Sub

#End Region

    '2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成　START

#Region "Otherjob"

    ''' <summary>
    ''' Otherjob画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOtherjob()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3140103BusinessLogicインスタンス
        Using bis As New SC3140103BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = bis.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            'DMS販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            'DMS店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'ユーザーアカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店管理番号
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO番号
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO作業連番
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'モード
            Me.SetValue(ScreenPos.Next, SessionParam09, String.Empty)

            '画面ID
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_OTHERJOB)

        End Using


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


    End Sub

#End Region
    '2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成　END

#End Region

#Region "呼出し項目チェック"

    ''' <summary>
    ''' 呼出し前入力項目チェック処理
    ''' </summary>
    ''' <param name="callNo">呼出番号</param>
    ''' <param name="callPlace">呼出場所</param>
    ''' <returns>呼出し可能判定結果(True：呼出し可能)</returns>
    ''' <remarks></remarks>
    Private Function CheckCallOperation(ByVal callNo As String, _
                                        ByVal callPlace As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim rtFlg As Boolean = True

        '発券番号チェック
        If String.IsNullOrEmpty(callNo) _
            OrElse String.IsNullOrEmpty(callNo.Trim) Then
            '発券番号が存在しない

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id915)

            '処理不可能を設定
            rtFlg = False

            '処理終了
            Return rtFlg

        End If

        '呼出し場所チェック
        If String.IsNullOrEmpty(callPlace) _
              OrElse String.IsNullOrEmpty(callPlace.Trim) Then
            '呼出し場所が存在しない

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id911)

            '処理不可能を設定
            rtFlg = False

            '処理終了
            Return rtFlg

        End If

        '呼出し場所の禁止文字チェック
        If Not Validation.IsValidString(callPlace) Then
            '禁止文字が存在する場合

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id912)

            '呼出し場所にフォーカスを当てる
            Me.DetailsCallPlace.Focus()

            '処理不可能を設定
            rtFlg = False

            '処理終了
            Return rtFlg

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
          , "{0}.{1} END" _
          , Me.GetType.ToString _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '処理結果(成功)
        Return rtFlg


    End Function

#End Region

#Region "顧客情報検索"

    ''' <summary>
    ''' 顧客情報検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetCustomerInfomation()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '検索条件(どの検索条件で検索されているか判定するフラグ)
        Dim SearchTypeIndexHidden As String = Me.SearchTypeIndexHidden.Value
        '検索テキスト：車両登録番号
        Dim searchRegistrationNumber As String = Me.SearchRegistrationNumberHidden.Value
        '検索テキスト：VIN
        Dim searchVin As String = Me.SearchVinHidden.Value
        '検索テキスト：顧客名
        Dim searchCustomerName As String = Me.SearchCustomerNameHidden.Value
        '検索テキスト：基幹予約ID
        Dim SearchAppointNumber As String = Me.SearchAppointNumberHidden.Value

        '検索開始行
        Dim searchStartRow As Long = CType(Me.SearchStartRowHidden.Value, Long)
        '検索最終行
        Dim searchEndRow As Long = CType(Me.SearchEndRowHidden.Value, Long)
        '検索全件数
        Dim searchAllCount As Long = Me.SetNullToLong(Me.SearchCustomerAllCountHidden.Value)
        '前のN件ボタンまたは次のN件ボタン判定フラグ
        Dim searchSelectType As Long = CType(Me.SearchSelectTypeHidden.Value, Long)


        '前のN件ボタン
        Dim divFrontLink As HtmlContainerControl
        divFrontLink = CType(Me.FrontLink, HtmlContainerControl)

        '次のN件ボタン
        Dim divNextLink As HtmlContainerControl
        divNextLink = CType(Me.NextLink, HtmlContainerControl)

        '検索結果が見つかりません文言設定
        Dim divSearchList As HtmlContainerControl = CType(Me.NoSearchImage, HtmlContainerControl)
        divSearchList.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 71)


        'テキスト内容チェック用
        Dim searchText As String = String.Empty

        '検索条件チェック
        Select Case SearchTypeIndexHidden

            Case SearchAreaRegNo
                '車両登録番号で検索

                '車両登録番号
                searchText = searchRegistrationNumber

            Case SearchAreaVin
                'VINで検索

                'VIN
                searchText = searchVin

            Case SearchAreaName
                '顧客名で検索

                '顧客名
                searchText = searchCustomerName

            Case SearchAreaAppoint
                '基幹予約IDで検索

                '基幹予約ID
                searchText = SearchAppointNumber

            Case Else
                '上記以外
                '予期せぬエラー

                '前のN件ボタン非表示
                divFrontLink.Style("display") = "none"
                '次のN件ボタン
                divNextLink.Style("display") = "none"
                '検索結果エリア非表示
                divSearchList.Style("display") = "none"

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

                'メッセージの表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Return

        End Select

        'テキスト入力チェック
        'テキストが空文字かチェック
        If 0 < searchText.Length Then
            'テキスト内容有り

            'テキスト内容(禁止文字)チェック
            If Not Validation.IsValidString(searchRegistrationNumber) Then
                'テキストに禁止文字があった場合

                '前のN件ボタン非表示
                divFrontLink.Style("display") = "none"
                '次のN件ボタン
                divNextLink.Style("display") = "none"
                '検索結果エリア非表示
                divSearchList.Style("display") = "none"

                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

                'メッセージの表示
                Me.ShowMessageBox(MsgID.id906)

                '処理終了
                Return

            End If

        Else
            'テキスト内容無し

            '前のN件ボタン非表示
            divFrontLink.Style("display") = "none"
            '次のN件ボタン
            divNextLink.Style("display") = "none"
            '検索結果エリア非表示
            divSearchList.Style("display") = "none"

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

            'メッセージの表示
            Me.ShowMessageBox(MsgID.id905)


            '処理終了
            Return

        End If

        '顧客情報テーブル
        Dim result As SC3140103SearchResult

        'SC3140103BusinessLogicのインスタンス
        Using bl As New SC3140103BusinessLogic

            '自社客検索結果取得
            result = bl.GetCustomerList(staffInfo, _
                                        searchRegistrationNumber, _
                                        searchVin, _
                                        searchCustomerName, _
                                        SearchAppointNumber, _
                                        searchStartRow, _
                                        searchEndRow, _
                                        searchSelectType, _
                                        searchAllCount)

        End Using

        '自社客検索結果取得チェック
        If result Is Nothing Then
            'Nothingの場合
            '予期せぬエラーが発生

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END ERR:CreateCustomerSearchXmlDocument IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '前のN件ボタン非表示
            divFrontLink.Style("display") = "none"
            '次のN件ボタン
            divNextLink.Style("display") = "none"
            '検索結果エリア非表示
            divSearchList.Style("display") = "none"

            'メッセージの表示
            Me.ShowMessageBox(MsgID.id916)

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimerSA();", True)

            '顧客情報をリセット
            Me.SearchRepeater.DataSource = Nothing
            Me.SearchRepeater.DataBind()

            '処理終了            
            Exit Sub

        End If

        '顧客情報を設定
        Me.SearchRepeater.DataSource = result.DataTable.Select()
        'コントロールにバインド
        Me.SearchRepeater.DataBind()

        '自社客検索結果取得チェック
        If 0 < result.DataTable.Count Then
            '自社客検索結果取得が存在する場合

            Dim searchData As Control
            Dim row As SC3140103DataSet.SC3140103VisitSearchResultRow

            'Rowに変換
            Dim rowList As SC3140103DataSet.SC3140103VisitSearchResultRow() = _
                DirectCast(Me.SearchRepeater.DataSource, SC3140103DataSet.SC3140103VisitSearchResultRow())

            Dim CustomerChangeParameterDiv As HtmlContainerControl

            '自社客検索結果分ループ
            For i = 0 To SearchRepeater.Items.Count - 1

                searchData = SearchRepeater.Items(i)
                row = rowList(i)

                '画像
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                'CType(searchData.FindControl("SearchPhotoImage"), HtmlImage).Attributes("src") = Me.ResolveClientUrl(row.IMAGEFILE)
                CType(searchData.FindControl("SearchPhotoImage"), HtmlImage).Attributes("src") = String.Concat(Me.ResolveClientUrl(row.IMAGEFILE), _
                                                                                                               "?" + Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
                '車両登録No
                CType(searchData.FindControl("SearchRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.VCLREGNO)
                'VIN
                CType(searchData.FindControl("SearchVinNo"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.VIN)
                '顧客名称
                CType(searchData.FindControl("SearchCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.CUSTOMERNAME)
                '車種＋PROVINCE
                CType(searchData.FindControl("SearchModel"), HtmlContainerControl).InnerHtml = Me.SetConnectString(row.VEHICLENAME, row.PROVINCE)
                '電話番号
                CType(searchData.FindControl("SearchPhone"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.TELNO)
                '携帯電話番号
                CType(searchData.FindControl("SearchMobile"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.MOBILE)

                '付替え用データの設定
                CustomerChangeParameterDiv = DirectCast(searchData.FindControl("CustomerChangeParameter"), HtmlContainerControl)

                With CustomerChangeParameterDiv

                    '顧客コード
                    .Attributes("CustomerCodeParameter") = Me.SetNullToString(row.CUSTOMERCODE)
                    '基幹顧客ID
                    .Attributes("DmsIdParameter") = Me.SetNullToString(row.DMSID)
                    'モデル
                    .Attributes("ModelParameter") = Me.SetNullToString(row.MODEL)
                    'SAコード
                    .Attributes("SacodeParameter") = Me.SetNullToString(row.SACODE)

                End With

            Next

            '次の表示件数，および前の表示件数の設定
            '検索結果全件数
            Dim customerCount As Long = result.ResultCustomerCount
            '検索開始行
            Dim resultStartRow As Long = result.ResultStartRow
            '検索終了行
            Dim resultEndRow As Long = result.ResultEndRow
            '標準表示件数
            Dim standardCount As Long = result.StandardCount

            '次回表示件数の設定（次のN件，前のN件）
            Me.SetOtherDisplay(resultStartRow, resultEndRow, customerCount, standardCount)

            '検索結果無しを非表示
            divSearchList.Style("display") = "none"

            'スクロール設定
            '差分
            Dim differenceRow As Long = 0

            '呼出元確認
            If 0 < searchSelectType Then
                '次のN件ボタン

                '前回終了位置と今回開始位置の差分を求める
                Dim beforeEndRow As Long = searchEndRow - 4

                differenceRow = beforeEndRow - resultStartRow + 1

            ElseIf searchSelectType < 0 Then
                '前のN件ボタン

                '前回開始位置と今回開始位置の差分を求める
                differenceRow = searchStartRow - resultStartRow - 1

            End If

            '開始行チェック
            If 1 < resultStartRow Then
                '1行目以降(前のN件表示時)

                '今回開始位置が1行目以降の場合、前のN件表示分、行が加算される
                Me.ScrollPositionHidden.Value = _
                    ((differenceRow + 1) * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)

            Else
                '1行目

                Me.ScrollPositionHidden.Value = _
                    (differenceRow * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)

            End If

        Else
            '自社客検索結果取得が存在しない場合

            '検索結果無しの表示
            divSearchList.Style("display") = "block"
            '前のN件ボタン非表示
            divFrontLink.Style("display") = "none"
            '次のN件ボタン非表示
            divNextLink.Style("display") = "none"

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' 次回表示件数の設定（次のN件，前のN件）
    ''' </summary>
    ''' <param name="startRow">開始行番号</param>
    ''' <param name="endRow">終了行番号</param>
    ''' <param name="customerCount">顧客検索件数</param>
    ''' <param name="standardCount">標準取得件数</param>
    Private Sub SetOtherDisplay(ByVal startRow As Long, _
                                ByVal endRow As Long, _
                                ByVal customerCount As Long, _
                                ByVal standardCount As Long)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:startRow = {3}, endRow = {4}," & _
                                   " customerCount = {5}, standardCount = {6}," _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , CType(startRow, String) _
                                 , CType(endRow, String) _
                                 , CType(customerCount, String) _
                                 , CType(standardCount, String)))


        '検索結果全件数の確認
        If 0 < customerCount Then
            '1件上の場合

            '前のN件コントロールの宣言
            Dim displayFront As String
            Dim divFrontLink As HtmlContainerControl
            Dim divFrontList As HtmlContainerControl
            Dim divFrontListSearching As HtmlContainerControl
            divFrontLink = CType(Me.FrontLink, HtmlContainerControl)
            divFrontList = CType(Me.FrontList, HtmlContainerControl)
            divFrontListSearching = CType(Me.FrontListSearching, HtmlContainerControl)

            '開始行が1行目以降かチェック
            If 1 < startRow Then
                '1行目以降

                '標準表示件数と検索開始行を比較
                If startRow <= standardCount Then
                    '標準件数のほうが大きい場合

                    '前のN件に検索開始行－1を設定
                    displayFront = (startRow - 1).ToString(CultureInfo.CurrentCulture)
                Else
                    '検索開始行のほうが大きい場合

                    '前のN件に標準表示件数を設定
                    displayFront = standardCount.ToString(CultureInfo.CurrentCulture)
                End If

                '前のN件をラベルに設定
                divFrontList.InnerHtml = _
                    WebWordUtility.GetWord(APPLICATIONID, 72).Replace("{0}", CType(displayFront, String))
                '読込み中をラベルに設定
                divFrontListSearching.InnerHtml = _
                    WebWordUtility.GetWord(APPLICATIONID, 73).Replace("{0}", CType(displayFront, String))

                '前のN件設定
                divFrontLink.Style("display") = "block"

            Else
                '1行目の場合

                '前のN件非表示
                divFrontLink.Style("display") = "none"

            End If

            '次のN件コントロールの宣言
            Dim displayNext As String
            Dim divNextLink As HtmlContainerControl
            Dim divNextList As HtmlContainerControl
            Dim divNextListSearching As HtmlContainerControl
            divNextLink = CType(Me.NextLink, HtmlContainerControl)
            divNextList = CType(Me.NextList, HtmlContainerControl)
            divNextListSearching = CType(Me.NextListSearching, HtmlContainerControl)

            '検索結果全件数と検索終了行を比較
            If endRow < customerCount Then
                '検索結果全件数のほうが大きい場合

                '全検索結果全件数と検索終了行との差分を計算する
                Dim differenceEndRow As Long = customerCount - endRow

                '差分と標準表示件数を比較
                If differenceEndRow < standardCount Then
                    '標準表示件数件数のほうが大きい場合

                    '次のN件に差分を設定
                    displayNext = CType(differenceEndRow, String)
                Else
                    '差分のほうが大きい場合

                    '次のN件に標準表示件数を設定
                    displayNext = standardCount.ToString(CultureInfo.CurrentCulture)
                End If

                '前のN件をラベルに設定
                divNextList.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 74) _
                .Replace("{0}", CType(displayNext, String))
                '読込み中をラベルに設定
                divNextListSearching.InnerHtml = WebWordUtility.GetWord(APPLICATIONID, 75) _
                .Replace("{0}", CType(displayNext, String))

                '次のN件表示
                divNextLink.Style("display") = "block"
            Else
                '検索終了行が大きいまたはイコールの場合

                '次のN件非表示
                divNextLink.Style("display") = "none"

            End If


            '隠しフィールドに開始行を設定
            Me.SearchStartRowHidden.Value = startRow.ToString(CultureInfo.CurrentCulture)
            '隠しフィールドに終了行を設定
            Me.SearchEndRowHidden.Value = endRow.ToString(CultureInfo.CurrentCulture)
            '隠しフィールドに全件数を設定
            Me.SearchCustomerAllCountHidden.Value = customerCount.ToString(CultureInfo.CurrentCulture)

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#End Region

#Region "付替え前確認処理"

    ''' <summary>
    ''' 付替え前確認結果をHiddenFieldへの設定処理
    ''' </summary>
    ''' <param name="row">付替え前情報</param>
    ''' <history>
    ''' </history>
    Private Sub SetHiddenStatus(ByVal row As SC3140103DataSet.SC3140103BeforeChangeCheckResultRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '付替え先来店実績連番
        If Not row.IsAFTERVISITNONull Then
            '付替え先来店実績連番がある場合

            'クライアントに設定する
            Me.ChipVisitNumberChange.Value = CType(row.AFTERVISITNO, String)

        Else
            '付替え先来店実績連番がない場合

            '空文字
            Me.ChipVisitNumberChange.Value = String.Empty

        End If

        '付替え先予約ID
        If Not row.IsAFTERRESERVENONull Then
            '付替え先予約IDがある場合

            'クライアントに設定する
            Me.ChipReserveNumberChange.Value = CType(row.AFTERRESERVENO, String)

        Else
            '付替え先予約IDがない場合

            '空文字
            Me.ChipReserveNumberChange.Value = String.Empty

        End If

        '付替え先整備受注番号
        If Not row.IsAFTERORDERNONull Then
            '付替え先整備受注番号がある場合

            'クライアントに設定する
            Me.ChipOrderNumberChange.Value = row.AFTERORDERNO

        Else
            '付替え先整備受注番号がない場合

            '空文字
            Me.ChipOrderNumberChange.Value = String.Empty

        End If

        '付替え先担当SA
        If Not row.IsAFTERSACODENull Then
            '付替え先担当SAがある場合

            'クライアントに設定する
            Me.ChipSACodeChange.Value = row.AFTERSACODE

        Else
            '付替え先担当SAがない場合

            '空文字
            Me.ChipSACodeChange.Value = String.Empty

        End If

        '付替え元整備受注番号
        If Not row.IsBEFOREORDERNONull Then
            '付替え元整備受注番号がある場合

            'クライアントに設定する
            Me.ChipOrderNumberBefore.Value = row.BEFOREORDERNO

        Else
            '付替え元整備受注番号がない場合

            '空文字
            Me.ChipOrderNumberBefore.Value = String.Empty

        End If

        '付替え元予約ID
        If Not row.IsBEFORERESERVENONull Then
            '付替え元予約IDがある場合

            'クライアントに設定する
            Me.ChipReserveNumberBefore.Value = CType(row.BEFORERESERVENO, String)

        Else
            '付替え元予約IDがない場合

            '空文字
            Me.ChipReserveNumberBefore.Value = String.Empty

        End If

        '車両IDチェック
        If Not row.IsAFTERVCLIDNull AndAlso 0 < row.AFTERVCLID Then
            '車両IDがある場合

            'クライアントに設定する
            Me.ChipVehicleIdAfter.Value = CType(row.AFTERVCLID, String)

        Else
            '車両IDがない場合

            '空文字
            Me.ChipVehicleIdAfter.Value = String.Empty

        End If

        '顧客IDをHiddenFiledより取得
        Dim customerCode As String = Me.SetNullToString(Me.SearchCustomerCodeChange.Value)

        '顧客IDチェック
        If Not row.IsAFTERCSTIDNull AndAlso 0 < row.AFTERCSTID Then
            '顧客IDが取得できた場合

            '顧客IDをHiddenFiledに設定する
            Me.SearchCustomerCodeChange.Value = CType(row.AFTERCSTID, String)

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "事前準備関連"

    '事前準備はまったく手をつけていない

    ''' <summary>
    ''' 事前準備エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitAdvancePreparations(ByVal dt As SC3140103AdvancePreparationsDataTable,
                                        ByVal strRightIcnD As String,
                                        ByVal strRightIcnI As String,
                                        ByVal strRightIcnS As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        Dim columnsList As New List(Of List(Of SC3140103AdvancePreparationsRow))
        Dim rowList As New List(Of SC3140103AdvancePreparationsRow)
        Dim j As Integer = 0
        ' 事前準備チップのデータセットをリストに設定、さらに4件ずつ別のリストに設定する
        For Each rowIFAdvancePreparations As SC3140103DataSet.SC3140103AdvancePreparationsRow In _
        dt.Select(String.Format(CultureInfo.CurrentCulture, "SASTATUSFLG = '{0}'", "1"), "REZ_PICK_DATE")
            If j Mod 4 = 0 Then
                rowList = New List(Of SC3140103AdvancePreparationsRow)
                columnsList.Add(rowList)
            End If
            rowList.Add(rowIFAdvancePreparations)
            j = j + 1
        Next

        ' コントロールにバインドする
        Me.Repeater1.DataSource = columnsList
        Me.Repeater1.DataBind()

        Dim advancePreparations As Control
        Dim advancePreparationsBox As Control
        Dim rowBox As List(Of SC3140103DataSet.SC3140103AdvancePreparationsRow)
        Dim row As SC3140103DataSet.SC3140103AdvancePreparationsRow

        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strRepresentativeWarehousing As String
        Dim strParkingNumber As String = ""

        Dim strReserveMark As String = "1"
        Dim strJDPMark As String
        Dim strSSCMark As String

        Dim divDeskDevice As HtmlContainerControl
        Dim deliveryPlanTime As HtmlContainerControl

        Dim divApoint As HtmlContainerControl

        Dim strTodayFlag As String
        Dim strDeliveryPlanTime As String

        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To Repeater1.Items.Count - 1
            '4チップセットのリストを取得する
            advancePreparationsBox = Repeater1.Items(i)
            rowBox = columnsList(i)
            ' 4チップセットのリストをバインド
            Dim rowListRepeater As Repeater = CType(advancePreparationsBox.FindControl("AdvancePreparationsRepeater"), Repeater)
            rowListRepeater.DataSource = rowBox
            rowListRepeater.DataBind()
            ' 4チップセットからチップ情報を取得する
            For k = 0 To rowListRepeater.Items.Count - 1

                advancePreparations = rowListRepeater.Items(k)
                row = rowBox(k)

                strRegistrationNumber = row.VCLREGNO
                strCustomerName = row.CUSTOMERNAME
                strTodayFlag = row.TODAYFLG
                ' 納車予定時刻は、取得データによって表示形式を切り替える
                strDeliveryPlanTime = Me.SetTimeOrDateToString(row.REZ_DELI_DATE.ToString(CultureInfo.CurrentCulture),
                                                               row.REZ_PICK_DATE.ToString(CultureInfo.CurrentCulture))
                strRepresentativeWarehousing = row.MERCHANDISENAME

                strJDPMark = row.JDP_MARK
                strSSCMark = row.SSC_MARK

                ' チップ内の文字列表示設定
                CType(advancePreparations.FindControl("AdvancePreparationsRegistrationNumber"), HtmlContainerControl) _
                    .InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
                CType(advancePreparations.FindControl("AdvancePreparationsCustomerName"), HtmlContainerControl) _
                    .InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
                CType(advancePreparations.FindControl("AdvancePreparationsDeliveryPlanTime"), HtmlContainerControl) _
                    .InnerHtml = strDeliveryPlanTime
                CType(advancePreparations.FindControl("AdvancePreparationsRepresentativeWarehousing"), HtmlContainerControl) _
                    .InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)

                ' アイコンの表示設定
                If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                    advancePreparations.FindControl("RightIcnD").Visible = True
                Else
                    advancePreparations.FindControl("RightIcnD").Visible = False
                End If

                If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                    advancePreparations.FindControl("RightIcnI").Visible = True
                Else
                    advancePreparations.FindControl("RightIcnI").Visible = False
                End If

                If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                    advancePreparations.FindControl("RightIcnS").Visible = True
                Else
                    advancePreparations.FindControl("RightIcnS").Visible = False
                End If

                'アイコンの文言設定
                CType(advancePreparations.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
                CType(advancePreparations.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
                CType(advancePreparations.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

                ' チップの内部保持属性の設定
                divDeskDevice = CType(advancePreparations.FindControl("AdvancePreparationsDeskDevice"), HtmlContainerControl)
                With divDeskDevice
                    .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                    .Attributes("orderNo") = row.ORDERNO
                    .Attributes("rezId") = row.REZID
                    .Attributes("class") = "ColumnContentsBoder"
                End With

                ' チップ内の納車予定時刻の文字色設定
                deliveryPlanTime = CType(advancePreparations.FindControl("AdvancePreparationsDeliveryPlanTime"), HtmlContainerControl)
                If strTodayFlag.Equals("1") Then
                    deliveryPlanTime.Style("color") = "#F00"
                Else
                    deliveryPlanTime.Style("color") = "#666666"
                End If

                divApoint = CType(advancePreparations.FindControl("AdvancePreparations"), HtmlContainerControl)
                With divApoint
                    .Style("top") = (104 * i).ToString(CultureInfo.CurrentCulture) & "px"
                    .Style("left") = (160 * k).ToString(CultureInfo.CurrentCulture) & "px"
                End With
            Next

        Next

        Dim divFlickableBox As HtmlContainerControl
        divFlickableBox = CType(Me.flickableBox, HtmlContainerControl)
        divFlickableBox.Style("height") = (114 * Repeater1.Items.Count).ToString(CultureInfo.CurrentCulture) & "px"
        divFlickableBox.Style("height") = (106 * Repeater1.Items.Count).ToString(CultureInfo.CurrentCulture) & "px"

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' 固定文字列付与「～」
    ''' </summary>
    ''' <param name="appendTakeTime">付与対象引取時間文字列</param>
    ''' <param name="appendVisitTime">付与対象納車時間文字列</param>
    ''' <returns>固定文字列付与値</returns>
    Private Function SetTimeFromToAppendTimes(ByVal appendTakeTime As String,
                                              ByVal appendVisitTime As String) As String

        ' 空白チェック
        If String.IsNullOrEmpty(appendTakeTime) Or String.IsNullOrEmpty(appendVisitTime) Then
            Return String.Empty
        End If

        ' 表示する文字列の設定
        Dim rtnVal As StringBuilder = New StringBuilder
        With rtnVal
            .Append(appendTakeTime)
            .Append(wordFixedString)
            .Append(appendVisitTime)
        End With

        Return rtnVal.ToString

    End Function

    ''' <summary>
    ''' 納車時間文字列の表示設定（日付と時刻）
    ''' </summary>
    ''' <param name="TakingTime">付与対象引取時刻文字列</param>
    ''' <param name="VisitTime">付与対象納車時刻文字列</param>
    ''' <returns>納車予定時刻文字列</returns>
    Private Function SetTimeOrDateToString(ByVal VisitTime As String,
                                           ByVal TakingTime As String) As String
        ' 空白チェック
        If String.IsNullOrEmpty(VisitTime) Or String.IsNullOrEmpty(TakingTime) Then
            Return String.Empty
        End If
        ' Date型にフォーマット
        Dim TimeFromDate As Date = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, TakingTime)
        Dim TimeToDate As Date = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, VisitTime)
        Dim strResult As String
        ' 時刻の文字列に切り分け
        Dim strFromTime As String = DateTimeFunc.FormatDate(14, TimeFromDate)
        Dim strToTime As String
        ' 日付の文字列に切り分け
        Dim strFromDate As String = DateTimeFunc.FormatDate(11, TimeFromDate)
        Dim strToDate As String = DateTimeFunc.FormatDate(11, TimeToDate)
        ' 日付が同一の場合、時刻表示、異なる場合日付表示を設定
        If strFromDate.Equals(strToDate) Then
            strToTime = DateTimeFunc.FormatDate(14, TimeToDate)
            strResult = SetTimeFromToAppendTimes(strFromTime, strToTime)
        Else
            strResult = SetTimeFromToAppendTimes(strFromTime, strToDate)
        End If

        Return strResult

    End Function

    ''' <summary>
    ''' 事前準備お客詳細
    ''' </summary>
    ''' <param name="dtReserve">受付情報</param>
    ''' <param name="dtVisit">来店情報</param>
    ''' <param name="rezID">予約ID</param>
    ''' <param name="staffInfo">ログイン情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function AdvancePrepDetailCustomer(ByVal dtReserve As SC3140103AdvancePreparationsReserveInfoDataTable, _
                         ByVal dtVisit As SC3140103AdvancePreparationsServiceVisitManagementDataTable, _
                         ByVal rezID As Long, ByVal staffInfo As StaffContext) As Boolean
        Dim rowReserve As SC3140103AdvancePreparationsReserveInfoRow = Nothing
        Dim rowAdvanceVisit As SC3140103AdvancePreparationsServiceVisitManagementRow = Nothing
        Dim tempVisitSeq As Nullable(Of Long) = Nothing
        ' 事前準備チップ予約情報の取得
        If dtReserve IsNot Nothing AndAlso 0 < dtReserve.Rows.Count Then
            Dim dtRow As SC3140103AdvancePreparationsReserveInfoRow() = _
                DirectCast(dtReserve.Select("", "WORKTIME ASC"), SC3140103AdvancePreparationsReserveInfoRow())
            rowReserve = CType(dtRow(0), SC3140103AdvancePreparationsReserveInfoRow)
        Else
            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)
            Me.ShowMessageBox(MsgID.id903)
            Return False
        End If

        ' 事前準備チップサービス来店管理情報の取得
        If dtVisit IsNot Nothing AndAlso 0 < dtVisit.Rows.Count Then
            rowAdvanceVisit = DirectCast(dtVisit.Rows(0), SC3140103AdvancePreparationsServiceVisitManagementRow)
            tempVisitSeq = rowAdvanceVisit.VISITSEQ
        End If
        '空の場合は自分のアカウントを入れる
        If String.IsNullOrEmpty(SASelector.SelectedValue) Then
            rowReserve.ACCOUNT_PLAN = staffInfo.Account
        Else
            'SA振当てが一致しない場合はエラー
            If Not (SASelector.SelectedValue.ToString.Equals(rowReserve.ACCOUNT_PLAN)) Then
                'SA未振当て
                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)
                Me.ShowMessageBox(MsgID.id903)
                Return False
            End If
        End If
        If Not (rowReserve.IsCUSTOMERFLAGNull) AndAlso _
           Not (Me.CheckCustomerType(rowReserve.CUSTOMERFLAG, TBL_STALLREZINFO)) Then
            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)
            '自社客でない/SA振当て済み
            Me.ShowMessageBox(MsgID.id903)
            Return False
        End If

        If Not IsNothing(rowAdvanceVisit) Then
            '事前準備チップの来店管理情報がある場合
            If (Not Me.CheckCustomerType(rowAdvanceVisit.CUSTSEGMENT, _
                                         TBL_SERVICE_VISIT_MANAGEMENT)) OrElse
                (Not Me.CheckAssignStatus(rowAdvanceVisit.ASSIGNSTATUS)) Then

                '自社客でない/SA振当て済み
                'タイマークリア
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "Error", "ErrorRefreshScript();", True)
                Me.ShowMessageBox(MsgID.id903)
                Return False
            End If
        End If


        Return True

    End Function

    ''' <summary>
    ''' 顧客区分をチェック
    ''' </summary>
    ''' <param name="customerType"></param>
    ''' <param name="tableName"></param>
    ''' <returns>チェック結果（true:自社客  false:未取引客）</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CheckCustomerType(ByVal customerType As String,
                                       ByVal tableName As String) As Boolean


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2} P1:{3} P2:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , LOG_START, customerType, tableName))

        If customerType.Equals("1") Then
            Return True
        End If

        Return False

    End Function

    ''' <summary>
    ''' SA振当て済みﾁｪｯｸ
    ''' </summary>
    ''' <param name="status"></param>
    ''' <returns>true:SA未振当て  False:SA振当て済み</returns>
    ''' <remarks></remarks>
    Private Function CheckAssignStatus(ByVal status As String) As Boolean
        If status.Equals("2") Then
            'SA振当て済み
            'Me.ShowMessageBox(MsgID.id903)
            Return False
        End If

        Return True
    End Function


#End Region

#Region "CT/ChTへのPush処理"

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' CT/ChTへのPush処理
    ''' </summary>
    ''' <param name="inSC3140103BusinessLogic">SC3140103BusinessLogic</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inRezId">サービス入庫ID</param>
    ''' <remarks></remarks>
    Private Sub SendPushServerCTAndCHT(ByVal inSC3140103BusinessLogic As SC3140103BusinessLogic _
                                     , ByVal inStaffInfo As StaffContext _
                                     , ByVal inRezId As Decimal)

        Using serviceCommonbiz As New ServiceCommonClassBusinessLogic

            ' サービス入庫IDよりストールリストを取得
            Dim stallListDataTable As ServiceCommonClassDataSet.StallInfoDataTable
            stallListDataTable = serviceCommonbiz.GetStallListToReserve(inRezId, ResultsFlgOn, CancelFlgOff)

            ' ストールIDのリスト生成
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            For Each row As ServiceCommonClassDataSet.StallInfoRow In stallListDataTable.Rows
                stallIdList.Add(row.STALL_ID)
            Next

            ' ストールIDリストが存在しない場合
            If stallIdList.Count <= 0 Then
                ' 処理を終了する
                Return
            End If

            ' ストールIDよりPush送信先のユーザー情報(CT、Cht)を取得する
            Dim operationCodeList As New List(Of Decimal)
            operationCodeList.Add(Operation.CT)
            operationCodeList.Add(Operation.CHT)

            Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
            staffInfoDataTable = serviceCommonbiz.GetNoticeSendAccountListToStall(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stallIdList, operationCodeList)

            ' 取得ユーザーに対しPush通知を送信する
            For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                'CT、CHT画面リフレッシュ通知
                inSC3140103BusinessLogic.SendPushServer(CLng(row.OPERATIONCODE), inStaffInfo, row.ACCOUNT, PushFlag0)
            Next
        End Using
    End Sub
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

#End Region


#End Region

#Region "共通部品"

    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>変換値</returns>
    Private Function SetDateTimeToString(ByVal time As DateTime) As String

        '結果
        Dim strResult As String

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            '時間無し
            '空文字
            '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            'Return String.Empty

            '文言の取得「--:--」
            Dim wordNullDateTime As String = WebWordUtility.GetWord(APPLICATIONID, 42)
            Return wordNullDateTime
            '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        End If

        ' 時間範囲チェック
        If Me.nowDateTime.ToString("yyyyMMdd", CultureInfo.CurrentCulture) _
            .Equals(time.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then

            ' 当日 (hh:mm)
            strResult = DateTimeFunc.FormatDate(14, time)
        Else
            ' 上記以外 (mm/dd)

            strResult = DateTimeFunc.FormatDate(11, time)
        End If

        Return strResult

    End Function

    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>変換値</returns>
    Private Function SetDateStringToString(ByVal time As String) As String

        ' 空白チェック
        If String.IsNullOrEmpty(time) Then
            '空文字ならそのまま

            Return String.Empty
        End If

        '結果
        Dim result As DateTime

        ' 日付チェック
        If Not DateTime.TryParse(time, result) Then
            '日付に変換できなかった

            Return String.Empty
        End If

        ' 日付チェック
        If result.Equals(DateTime.MinValue) Then
            '最小値なら空文字

            Return String.Empty
        End If

        Return SetDateTimeToString(result)

    End Function

    ''' <summary>
    ''' 文字列変換
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToString(ByVal str As String, Optional ByVal strNull As String = "") As String

        ' 空白チェック
        If String.IsNullOrEmpty(str) Then
            '空文字ならデフォルト値
            Return strNull
        End If

        ' 空白チェック
        If String.IsNullOrEmpty(str.Trim()) Then
            '空文字ならデフォルト値
            Return strNull
        End If

        Return str

    End Function

    ''' <summary>
    ''' 数値変換
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToLong(ByVal num As String, Optional ByVal lngNull As Long = 0) As Long

        '結果
        Dim result As Long

        '数値チェック
        If Not Long.TryParse(num, result) Then
            '数値に変換できなかったらデフォルト値

            result = lngNull
        End If

        '数値チェック
        If result = 0 Then
            '0の場合デフォルト値

            result = lngNull
        End If

        Return result

    End Function

    ''' <summary>
    ''' 数値変換(Decimal変換)
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToDecimal(ByVal num As String, Optional ByVal lngNull As Decimal = 0) As Decimal

        '結果
        Dim result As Decimal

        '数値チェック
        If Not Decimal.TryParse(num, result) Then
            '数値に変換できなかったらデフォルト値

            result = lngNull
        End If

        '数値チェック
        If result = 0 Then
            '0の場合デフォルト値

            result = lngNull
        End If

        Return result

    End Function

    ''' <summary>
    ''' 固定文字列付与「～」
    ''' </summary>
    ''' <param name="appendTime">付与対象文字列</param>
    ''' <param name="ChipArea">工程管理エリア</param>
    ''' <returns>固定文字列付与値</returns>
    Private Function SetTimeFromToAppend(ByVal appendTime As String, ByVal chipArea As ChipArea) As String

        ' 空白チェック
        If String.IsNullOrEmpty(appendTime) Then
            Return C_DEFAULT_CHIP_SPACE
        End If

        ' 工程管理エリア確認
        Dim rtnVal As StringBuilder = New StringBuilder
        With rtnVal
            ' 工程管理エリア確認
            Select Case chipArea
                Case chipArea.Reception
                    ' 受付エリア

                    ' XXX～
                    .Append(appendTime)
                    .Append(wordFixedString)

                Case chipArea.Approval,
                     chipArea.Preparation,
                     chipArea.Delivery,
                     chipArea.Work
                    ' 追加承認エリア、納車準備エリア、納車作業エリア、作業エリア

                    ' ～XXX
                    .Append(wordFixedString)
                    .Append(appendTime)

                Case Else
                    '上記以外

                    '無し
                    .Append(appendTime)
            End Select

        End With

        Return rtnVal.ToString

    End Function

    ''' <summary>
    ''' 時間変換 (hh:mm)
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>変換値</returns>
    Private Function SetDateTimeToStringDetail(ByVal time As DateTime, _
                                               ByVal inNowDate As Date) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:time={3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , time))

        Dim staffInfo As StaffContext = StaffContext.Current

        Dim strResult As String

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        Try
            If Not inNowDate.Date = time.Date Then

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                '(MM/dd hh:mm)
                'strResult = time.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture)

                strResult = String.Concat(DateTimeFunc.FormatDate(11, time), Space(1), DateTimeFunc.FormatDate(14, time))

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            Else
                ' (hh:mm)
                strResult = DateTimeFunc.FormatDate(14, time)
            End If

        Catch ex As FormatException
            strResult = String.Empty
        End Try

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Result:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , strResult))

        Return strResult

    End Function

    ''' <summary>
    ''' 文字列の結合(間にスペース有り)
    ''' </summary>
    ''' <param name="firstWord">前方文字列</param>
    ''' <param name="secondWord">後方文字列</param>
    Private Function SetConnectString(ByVal firstWord As String, _
                                      ByVal secondWord As String) As String

        '前方文字列
        Dim setFirst As String = SetNullToString(firstWord)

        '後方文字列
        Dim setSecond As String = SetNullToString(secondWord)

        '前方文字列 + スペース + 後方文字列
        Dim resultWord As String = setFirst & "  " & setSecond

        Return resultWord

    End Function

    '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    ''' <summary>
    ''' Date型を文字列に変換(yyyy/MM/dd HH:mm)
    ''' </summary>
    ''' <param name="_date">変換元</param>
    ''' <param name="strNull">変換できない場合に返す値</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function FormatTimeStringToDate(ByVal _date As Date, ByVal strNull As Date) As String

        Dim str As String
        Try

            'str = DateTimeFunc.FormatDate(2, _date)

            '日付チェック(yyyy/MM/dd HH:mm)
            If _date <> Date.MinValue Then
                '日付(最小値)以外の場合

                '引数でフォーマットする
                str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", _date)

            Else
                '日付(最小値)の場合

                'デフォルト値でフォーマットする(yyyy/MM/dd HH:mm)
                str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", strNull)

            End If

        Catch ex As FormatException

            'str = DateTimeFunc.FormatDate(2, strNull)
            '(yyyy/MM/dd HH:mm)
            str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", strNull)


        End Try

        Return str
    End Function
    '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()

        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitFooterEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ヘッダ表示設定
        '戻るボタン非活性化
        CType(Me.Master.Master, CommonMasterPage).IsRewindButtonEnabled = False

        'フッタ表示設定
        'サブメニューボタンを設定（イベントハンドラ割り当て）

        'メインメニュー
        Dim mainMenuButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        mainMenuButton.OnClientClick = "return FooterButtonControl();"

        '顧客情報画面(ヘッダー顧客検索機能へフォーカス)
        Dim customerButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
        customerButton.OnClientClick = "return false;"

        'R/O(RO一覧に遷移) 
        Dim roMakeButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
        AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
        roMakeButton.OnClientClick = "return FooterButtonControl();"

        '商品訴求コンテンツ
        Dim productsAppealContentButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
        AddHandler productsAppealContentButton.Click, AddressOf productsAppealContentButton_Click
        productsAppealContentButton.OnClientClick = "return FooterButtonControl();"

        'キャンペーン
        Dim campaignButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
        AddHandler campaignButton.Click, AddressOf campaignButton_Click
        campaignButton.OnClientClick = "return FooterButtonControl();"

        '予約管理
        Dim visitButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
        AddHandler visitButton.Click, AddressOf VisitManagementFooterButton_Click
        visitButton.OnClientClick = "return FooterButtonControl();"

        'SMB
        Dim smbButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
        AddHandler smbButton.Click, AddressOf SMBButton_Click
        smbButton.OnClientClick = "return FooterButtonControl();"

        '連絡先
        Dim telephoneBookButton As CommonMasterFooterButton = _
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub mainMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try

            '再表示
            Me.RedirectNextScreen(MAINMENUID)


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} mainMenuButton_Click IS DBTIMEOUT" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id901)

        End Try

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' フッター「R/Oボタン」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' R/O一覧画面に遷移します。
    ''' </remarks>
    Private Sub roMakeButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O一覧画面遷移処理(パラメータ設定)
        Me.RedirectOrderList()

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' フッター「商品訴求コンテンツ」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' 商品訴求コンテンツ画面に遷移します。
    ''' </remarks>
    Private Sub productsAppealContentButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O一覧画面遷移処理(パラメータ設定)
        Me.RedirectProductsAppealContent()

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' フッター「キャンペーン」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' キャンペーン画面に遷移します。
    ''' </remarks>
    Private Sub campaignButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'キャンペーン画面遷移処理(パラメータ設定)
        Me.RedirectCampaign()

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 予約管理ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VisitManagementFooterButton_Click(ByVal sender As Object, _
                                                  ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '来店管理画面に遷移する
        Me.RedirectNextScreen(VISIT_MANAGEMENT_LIST_PAGE)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' フッター「SMBボタンを押した時の処理」クリック時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

End Class
