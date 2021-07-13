'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3150101BusinessLogic.vb
'─────────────────────────────────────
'機能： TCメインメニュービジネスロジック
'補足： 
'作成： 2012/01/26 KN 鶴田
'更新： 2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正
'更新： 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正
'更新： 2012/03/08 KN 森下 【SERVICE_1】課題管理番号-BMTS_0307_YW_02の不具合修正
'更新： 2012/03/12 KN 西田 【SERVICE_1】課題管理番号-BMTS_0309_YW_01の不具合修正 APIのDataSetカラム名変更
'更新： 2012/03/19 KN 西田　プレユーザーテスト課題・不具合対応 No.22 開始処理は15分前以前は開始不可とする
'更新： 2012/03/21 KN 上田　仕様変更対応(追加作業関連の遷移先変更)  
'更新： 2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加
'更新： 2012/04/11 KN 西田【SERVICE_1】プレユーザーテスト No.197 開始処理のチェック順変更
'更新： 2012/05/22 KN 森下【SERVICE_1】号口不具合対応 秒の切り捨て処理追加(チップ衝突判定回避)
'更新： 2012/05/28 KN 西田【SERVICE_1】号口不具合対応 秒の切り捨て処理追加(チップ衝突判定回避)
'更新： 2012/06/01 KN 西田 STEP1 重要課題対応
'更新： 2012/06/05 KN 彭健 コード分析対応
'更新： 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応
'更新： 2012/07/26 KN 彭健 STEP1 仕分け課題対応
'更新： 2012/08/14 KN 彭健 SAストール予約受付機能開発（No.27カゴナンバー表示）
'更新： 2012/11/05 TMEJ彭健  問連修正（GTMC121025029、GTMC121029047）、イベントログ出力の削減、内部ログ出力の見直し
'更新： 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
'更新： 2013/02/26 TMEJ 成澤 【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニター起動待機時間の取得)
'更新： 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)
'更新： 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新： 2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新： 2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
'更新： 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
'更新： 2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新： 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新： 2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新:  2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
'更新:                       [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正
'更新： 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3150101
Imports System.Globalization
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.iCROP.DataAccess.StallInfo
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800805
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports System.Text
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

'2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
'2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
'                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
'                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 END


Public Class SC3150101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "SMB実績ステータスの規定値"
    ''' <summary>
    ''' SMB実績ステータス：未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OUT_SHED As String = "0"
    ''' <summary>
    ''' SMB実績ステータス：未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OUT_SHED_00 As String = "00"
    ''' <summary>
    ''' SMB実績ステータス：入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_IN_SHED As String = "10"
    ''' <summary>
    ''' SMB実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_ResultStatusWorking As String = "20"
    ''' <summary>
    ''' SMB実績ステータス：部品欠品
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_PARTSMISS As String = "30"
    ''' <summary>
    ''' SMB実績ステータス：お客様連絡待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_CONNECTION As String = "31"
    ''' <summary>
    ''' SMB実績ステータス：仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_ARRANGEMENT As String = "32"
    ''' <summary>
    ''' SMB実績ステータス：未来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_NOTCOMING_CUSTOMER As String = "33"
    ''' <summary>
    ''' SMB実績ステータス：ストール待機
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_STALL As String = "38"
    ''' <summary>
    ''' SMB実績ステータス：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OTHER As String = "39"
    ''' <summary>
    ''' SMB実績ステータス：洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_WASH As String = "40"
    ''' <summary>
    ''' SMB実績ステータス：洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WASHING As String = "41"
    ''' <summary>
    ''' SMB実績ステータス：検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_INSPECTION As String = "42"
    ''' <summary>
    ''' SMB実績ステータス：検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_INSPECTING As String = "43"
    ''' <summary>
    ''' SMB実績ステータス：検査不合格
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_REJECTED As String = "44"
    ''' <summary>
    ''' SMB実績ステータス：預かり中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_TAKING_CHARGE As String = "50"
    ''' <summary>
    ''' SMB実績ステータス：納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_DELIVERY As String = "60"
    ''' <summary>
    ''' SMB実績ステータス：関連チップの前工程作業終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_FINISHED_PREVIOUS_PROCESS As String = "97"
    ''' <summary>
    ''' SMB実績ステータス：MidFinish
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_MID_FINISH As String = "98"
    ''' <summary>
    ''' SMB実績ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_FINISHED As String = "99"

#End Region

#Region "実績ステータスの規定値"
    ''' <summary>
    ''' 実績ステータス：作業待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private RESULT_STATUS_WAITING As String = "1"
    ''' <summary>
    ''' 実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private ResultStatusWorking As String = "2"
    ''' <summary>
    ''' 実績ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private RESULT_STATUS_FINISHED As String = "3"
#End Region

#Region "SMB実績ステータスの規定値"
    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ' ''' <summary>
    ' ''' SMBステータス：ストール本予約
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SMB_StatusFormallyReserved As Integer = 1
    ' ''' <summary>
    ' ''' SMBステータス：ストール仮予約
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SMB_STATUS_PROPOSED_RESOURCE As Integer = 2
    ' ''' <summary>
    ' ''' SMBステータス：Unavailable
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SMB_STATUS_UNAVAILABLE As Integer = 3
    ' ''' <summary>
    ' ''' SMBステータス：取引納車
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SMB_STATUS_PICK_DELIVERY As Integer = 4

    ''' <summary>
    ''' SMBステータス：ストール本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_StatusFormallyReserved As Integer = 1
    ''' <summary>
    ''' SMBステータス：ストール仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_STATUS_PROPOSED_RESOURCE As Integer = 0
    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
#End Region

    ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
#Region "サービスステータス"
    ''' <summary>
    ''' サービスステータス：作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_WaitingtProcess As String = "04"
    ''' <summary>
    ''' サービスステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_Working As String = "05"
    ''' <summary>
    ''' サービスステータス：次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_WaitingNextProcess As String = "06"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' サービスステータス：洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_WaitingWashing As String = "07"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

    ''' <summary>
    ''' サービスステータス：検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_WaitingInspection As String = "09"

#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' ストール利用ステータス"00":着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus00 As String = "00"
    ''' <summary>
    ''' ストール利用ステータス"01":作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus01 As String = "01"
    ''' <summary>
    ''' ストール利用ステータス"02":作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus02 As String = "02"
    ''' <summary>
    ''' ストール利用ステータス"03":完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus03 As String = "03"
    ''' <summary>
    ''' ストール利用ステータス"04":作業指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus04 As String = "04"
    ''' <summary>
    ''' ストール利用ステータス"05":中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus05 As String = "05"
    ''' <summary>
    ''' ストール利用ステータス"05":日跨ぎ終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus06 As String = "06"
    ''' <summary>
    ''' ストール利用ステータス"07":未来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const stallUseStetus07 As String = "07"
#End Region
    ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ' 2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
#Region "メンバー変数"
    ''' <summary>単独Job終了した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushFinishSingleJob As Boolean
    ''' <summary>単独Job中断した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushStopSingleJob As Boolean

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>サブエリア更新Pushフラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushSubAreaRefresh As Boolean
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

#End Region
    ' 2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END


#Region "定数"

    ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150101"

    ''' <summary>
    ''' 日付最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MINDATE As String = "1900/01/01 00:00:00"

    ''' <summary>
    ''' キャンセルフラグ:0
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CANCEL_FLG As String = "0"

    ''' <summary>
    ''' 休憩取得フラグ:1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REST_FLG_TAkE As String = "1"

    ''' <summary>
    ''' 休憩取得フラグ:0
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REST_FLG_NO_TAkE As String = "0"
    ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyyMMdd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDD As Integer = 9
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21

    ''' <summary>
    ''' データ更新用：Nullで上書き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OverwriteNull As Integer = 0
    ''' <summary>
    ''' データ更新用：指定値で上書き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OverwriteNewValue As Integer = 1
    ''' <summary>
    ''' データ更新用：変更しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeepCurrent As Integer = 2

    ''' <summary>
    ''' 予約履歴登録用：ストール予約 登録時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_INSERT As Integer = 0
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 通常更新時 / ACTUAL_TIME 更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_UPDATE As Integer = 1
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 キャンセル更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_CANCEL As Integer = 2
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 グループ更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_GROUP As Integer = 3

    ''' <summary>
    ''' 戻り値：OK
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnOk As Integer = 0
    ''' <summary>
    ''' 戻り値：NG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnNG As Integer = 906

    ''' <summary>
    ''' 戻り値：NG（当日処理）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnNG_SUSPEND As Integer = 907

    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 戻り値：NG(終了処理)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnNG_FINISH As Integer = 931
    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    ''' <summary>
    ''' MidFinish作業終了時間の調整時間(時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SMB_DISPDATE_ADJUST As String = "SMB_DISPDATE_ADJUST"

    ''' <summary>
    ''' 稼働時間タイプ:Progressive
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_TIME_PROGRESS As Integer = 0
    ''' <summary>
    ''' 稼働時間タイプ:Reservation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_TIME_RESERVE As Integer = 1

    ''' <summary>
    ''' 作業日付配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_DATE_ARRAY_NUMBER As Integer = 3
    ''' <summary>
    ''' 作業日付配列:開始日付の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_START_DATE As Integer = 0
    ''' <summary>
    ''' 作業日付配列:開始時刻の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_START_TIME As Integer = 1
    ''' <summary>
    ''' 作業日付配列:終了時刻の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_END_TIME As Integer = 2

    ''' <summary>
    ''' ストール日時配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_DATE_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' ストール日時配列:開始日付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_START_DATE As Integer = 0
    ''' <summary>
    ''' ストール日時配列:開始時刻
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_START_TIME As Integer = 1

    ''' <summary>
    ''' 時刻の表現タイプ:24時以降表記(25:00など)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TIME_TYPE_OVER24 As Integer = 1
    ''' <summary>
    ''' 時刻の表現タイプ:通常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TIME_TYPE_NORMAL As Integer = 0

    ''' <summary>
    ''' 作業開始時間取得用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 作業開始時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_START As Integer = 0
    ''' <summary>
    ''' 作業開始時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_END As Integer = 1

    ''' <summary>
    ''' 作業終了時間取得用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 作業終了時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_END As Integer = 0
    ''' <summary>
    ''' 作業終了時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_START As Integer = 1

    ''' <summary>
    ''' 対象日後の稼働日用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 対象日後の稼働日用配列:稼働日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_DATE As Integer = 0
    ''' <summary>
    ''' 対象日後の稼働日用配列:非稼働日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_COUNT As Integer = 1

    ''' <summary>
    ''' ログタイプ:情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_INFO As String = "I"
    ''' <summary>
    ''' ログタイプ:エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_ERROR As String = "E"
    ''' <summary>
    ''' ログタイプ:警告
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_WARNING As String = "W"

    ' R/Oステータス
    'Private Const C_RO_STATUS_NONE As String = "0"          ' なし
    Private Const C_RO_STATUS_RECEPTION As String = "1"     ' 受付
    Private Const C_RO_STATUS_ESTI_WAIT As String = "5"     ' 見積確認待ち
    Private Const C_RO_STATUS_ITEM_WAIT As String = "4"     ' 部品待ち
    'Private Const C_RO_STATUS_WORKING As String = "2"       ' 作業中
    'Private Const C_RO_STATUS_INSP_OK As String = "7"       ' 検査完了
    'Private Const C_RO_STATUS_MANT_OK As String = "6"       ' 整備完了
    'Private Const C_RO_STATUS_SALE_OK As String = "3"       ' 売上済み
    'Private Const C_RO_STATUS_FINISH As String = "8"        ' 納車完了

    ''' <summary>
    ''' 追加作業＜9：完成検査完了＞
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_ADD_REPAIR_STATUS_COMPLET As String = "9"

    '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
    ''' <summary>
    ''' 完成検査承認前（承認済みを含む）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSPECTION_APPROVAL_BEFORE = "0"
    '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
    ''' <summary>
    ''' 着工準備区分＜1：着工指示＞
    ''' </summary>
    ''' <remarks>0:未着工→1:着工指示→2:着工準備の順にステータス遷移</remarks>
    Private Const INSTRUCT_DIRECTION = "1"
    ''' <summary>
    ''' 着工指示区分＜2：着工準備＞
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSTRUCT_READY = "2"

    ''' <summary>
    ''' 作業連番
    ''' 0：未計画/親作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORKSEQ_NOPLAN_PARENT As String = "0"

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

    ''' <summary>
    ''' オペレーションコードCT権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeCT As Integer = 55
    ''' <summary>
    ''' オペレーションコードChT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeChT As Integer = 62
    ''' <summary>
    ''' オペレーションコードSA
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSA As Integer = 9
    ''' <summary>
    ''' オペレーションコードPS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodePS As Integer = 54
    ''' <summary>
    ''' オペレーションコードTC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeTC As Integer = 14
    ' ''' <summary>
    ' ''' オペレーションコードFM
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const OperationCodeFM As Integer = 58

    ''' <summary>
    ''' 自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentMyCustomer As String = "1"

    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' サービスコモンのインスタンス
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private serviceCommon As New ServiceCommonClassBusinessLogic

    ''' <summary>
    ''' 作業開始フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStartFlg As Integer = 0
    ''' <summary>
    ''' 作業終了フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workFinishFlg As Integer = 1
    ''' <summary>
    ''' 日跨ぎ終了フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workMidFinishFlg As Integer = 2
    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 作業中断フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStopFlg As Integer = 3
    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
    ''' <summary>
    ''' TabletSMBリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const smbRefreshFunction As String = "RefreshSMB()"
    ''' <summary>
    ''' PSメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const psRefreshFunction As String = "MainRefresh()"
    ''' <summary>
    ''' SAメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const saRefreshFunction As String = "MainRefresh()"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' CWメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CWRefreshFunction As String = "MainRefresh()"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

    ''' <summary>
    ''' 部品ステータス:部品準備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllPartsIssuedCompletely As String = "8"
    ''' <summary>
    ''' 部品準備完了ステータス:完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const partsFlg As String = "1"

    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' 休憩を取得しない（取得しなかった）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IsNoGetRest As Boolean = False
    ''' <summary>
    ''' 休憩を取得する（取得した）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IsGetRest As Boolean = True
    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
#End Region

#Region "日付変換用定数"

    ''' <summary>
    ''' 日付フォーマット変換用：変換前文字列の長さ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_TARGET_LENGTH As Integer = 12
    ''' <summary>
    ''' 日付フォーマット変換用：西暦の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_YEAR_START_INDEX As Integer = 0
    ''' <summary>
    ''' 日付フォーマット変換用：西暦の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_YEAR_LENGTH As Integer = 4
    ''' <summary>
    ''' 日付フォーマット変換用：月の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MONTH_START_INDEX As Integer = 4
    ''' <summary>
    ''' 日付フォーマット変換用：月の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MONTH_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：日の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_DAY_START_INDEX As Integer = 6
    ''' <summary>
    ''' 日付フォーマット変換用：日の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_DAY_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：時間の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_HOUR_START_INDEX As Integer = 8
    ''' <summary>
    ''' 日付フォーマット変換用：時間の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_HOUR_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：分の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MINUTE_START_INDEX As Integer = 10
    ''' <summary>
    ''' 日付フォーマット変換用：分の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MINUTE_LENGTH As Integer = 2
#End Region

    ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
    '                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 START
#Region "文言コード"
    ''' <summary>
    ''' 文言コード：未開始JOBなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordCdNoUnstartedJobToStart As Integer = 946
#End Region
    ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
    '                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 END

#Region "変数定義"
    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
    '2012/03/26 KN 日比野　プレユーザーテスト課題・不具合対応 No.22 開始処理は15分前以前は開始不可とする START
    ' ''' <summary>
    ' ''' 作業開始 有効範囲時間(分)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public standerdStartTime As Integer
    '2012/03/26 KN 日比野　プレユーザーテスト課題・不具合対応 No.22 開始処理は15分前以前は開始不可とする END
    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END
#End Region

#Region "JSON変換"
    ''' <summary>
    ''' 取得した開始時間（実績）、終了時間（実績）はなぜか"yyyymmddhhmm"の文字列にて格納されているため
    ''' "yyyy/mm/dd hh:mm"形式に変換して文字列として返す
    ''' </summary>
    ''' <param name="aTimeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeTimeString(ByVal aTimeData As String) As String

        Dim stringDate As New System.Text.StringBuilder
        Dim inputTimeData As String
        inputTimeData = Trim(aTimeData)
        '取得した文字列が12文字でない場合、変換対象外とみなし、空文字を返す
        '文字列が12文字の場合のみ処理を実施する
        If (inputTimeData.Length() = EXCHANGE_TIME_TARGET_LENGTH) Then
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_YEAR_START_INDEX, EXCHANGE_TIME_YEAR_LENGTH))
            stringDate.Append("/")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_MONTH_START_INDEX, EXCHANGE_TIME_MONTH_LENGTH))
            stringDate.Append("/")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_DAY_START_INDEX, EXCHANGE_TIME_DAY_LENGTH))
            stringDate.Append(" ")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_HOUR_START_INDEX, EXCHANGE_TIME_HOUR_LENGTH))
            stringDate.Append(":")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_MINUTE_START_INDEX, EXCHANGE_TIME_MINUTE_LENGTH))
        End If

        Return stringDate.ToString()

    End Function

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    ''' <remarks></remarks>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String
        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dataTable.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next

        Return JSerializer.Serialize(resultMain)
    End Function
#End Region

#Region "休憩時間の取得"

    ''' <summary>
    ''' 休憩時間データの格納文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_DATA_STRING_LENGTH = 4

    ''' <summary>
    ''' 休憩時間データの時間情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_INDEX = 0
    ''' <summary>
    ''' 休憩時間データの時間情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_LENGTH = 2
    ''' <summary>
    ''' 休憩時間データの分情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_INDEX = 2
    ''' <summary>
    ''' 休憩時間データの分情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_LENGTH = 2

    ''' <summary>
    ''' 休憩であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_BLEAK As Integer = 99
    ''' <summary>
    ''' 使用不可であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_UNAVAILABLE = 3

    ''' <summary>
    ''' 休憩時間を取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>休憩時間のデータセット</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Public Function GetBreakData(ByVal stallId As Decimal) As SC3150101DataSet.SC3150101ChipInfoDataTable
        'Public Function GetBreakData(ByVal stallId As Integer) As SC3150101DataSet.SC3150101BreakChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,param1:{2}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , CType(stallId, String)))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BreakChipInfoDataTable
            Dim userContext As StaffContext = StaffContext.Current

            Using chipInfoTable As New SC3150101DataSet.SC3150101ChipInfoDataTable()

                '休憩チップデータを取得.
                dt = adapter.GetBreakChipInfo(userContext.DlrCD, userContext.BrnCD, stallId)

                Dim dataCount As Long = 0
                '取得した休憩情報は、開始時間・終了時間共にHHMMの4桁文字列で格納されている.
                'この状態では、他のチップとの選択に使用できないため、Date型に変換する.
                For Each dr As DataRow In dt.Rows

                    '開始時間と終了時間をDate型に変換する.
                    Dim startTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("STARTTIME"), String))
                    Dim endTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("ENDTIME"), String))

                    '終了時間が開始時間以下の場合、終了時間のほうが大きくなるように終了時間に1日ずつ加算していく.
                    While endTimeDate <= startTimeDate
                        'endTimeDate.AddDays(1)
                        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計  START
                        endTimeDate = endTimeDate.AddDays(1)
                        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計  END
                    End While

                    '開始時間と終了時間を調整後、各カラムに格納する.
                    'dr("STARTTIME") = startTimeDate
                    'dr("ENDTIME") = endTimeDate
                    Dim chipInfoRow As SC3150101DataSet.SC3150101ChipInfoRow = CType(chipInfoTable.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)

                    chipInfoRow.DLRCD = userContext.DlrCD
                    chipInfoRow.STRCD = userContext.BrnCD
                    chipInfoRow.STARTTIME = startTimeDate
                    chipInfoRow.ENDTIME = endTimeDate
                    'チップステータスに休憩を示す値を格納する
                    chipInfoRow.STATUS = STATUS_BLEAK
                    'DBNull回避
                    chipInfoRow.REZID = -1
                    chipInfoRow.DSEQNO = 0
                    chipInfoRow.SEQNO = dataCount
                    'chipInfoRow.SERVICECODE_2 = "0"
                    chipInfoRow.RESULT_STALLID = stallId
                    chipInfoRow.STALLID = stallId
                    chipInfoRow.REZ_RECEPTION = ""
                    chipInfoRow.CUSTOMERNAME = ""
                    chipInfoRow.VEHICLENAME = ""
                    chipInfoRow.VCLREGNO = ""
                    chipInfoRow.INSDID = ""
                    chipInfoRow.CANCELFLG = ""
                    chipInfoRow.UPDATEACCOUNT = userContext.Account

                    chipInfoTable.Rows.Add(chipInfoRow)

                    dataCount = dataCount + 1
                Next

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                          , "{0}.{1} END" _
                                          , Me.GetType.ToString _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return chipInfoTable
            End Using
        End Using

    End Function


    ''' <summary>
    ''' DBより取得した4桁の休憩時間を当日付けのDate型に変換する.
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="breakHour">4桁の休憩時間</param>
    ''' <returns>Date型の休憩時間</returns>
    ''' <remarks></remarks>
    Private Function ExchangeBreakHourToDate(ByVal dealerCode As String, ByVal breakHour As String) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,param1:{2},param2:{3}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , dealerCode _
                                  , breakHour))

        '返す値の初期値として、当日の0時を設定する.
        Dim breakDate As Date = DateTimeFunc.Now(dealerCode).Date

        '取得した引数が4桁である場合、変換処理を実施する.
        If (breakHour.Length = BREAK_TIME_DATA_STRING_LENGTH) Then

            '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
            'Dim breakDateString As New System.Text.StringBuilder

            '当日日付を追加
            'breakDateString.Append(DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYY_MM_DD, breakDate))
            'breakDateString.Append(" ")
            'breakDateString.Append(breakHour.Substring(BREAK_TIME_HOUR_INDEX, BREAK_TIME_HOUR_LENGTH))
            'breakDateString.Append(":")
            'breakDateString.Append(breakHour.Substring(BREAK_TIME_MINUTE_INDEX, BREAK_TIME_MINUTE_LENGTH))

            ''生成した文字列を使用して、日付型データを取得する.
            'breakDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", breakDateString.ToString())

            '時間と分に分割する
            Dim hourUnit As String = breakHour.Substring(0, 2)
            Dim minuteUnit As String = breakHour.Substring(2)
            '時間と分を格納
            breakDate = breakDate.AddHours(Double.Parse(hourUnit, CultureInfo.CurrentCulture()))
            breakDate = breakDate.AddMinutes(Double.Parse(minuteUnit, CultureInfo.CurrentCulture()))
            '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} END return:{2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, breakDate)))

        Return breakDate

    End Function

#End Region

#Region "使用不可時間の取得"

    ''' <summary>
    ''' 使用不可チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼動開始時間</param>
    ''' <param name="toDate">ストール稼動終了時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnavailableData(ByVal stallId As Decimal,
                                       ByVal fromDate As Date, _
                                       ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable
        'Public Function GetUnavailableData(ByVal stallId As Integer, ByVal fromDate As Date, _
        '                                       ByVal toDate As Date) As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,param1:{2}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , CType(stallId, String)))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            Dim userContext As StaffContext = StaffContext.Current

            '使用不可チップデータを取得.
            Dim dtUnavailable As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable
            dtUnavailable = adapter.GetUnavailableChipInfo(stallId, fromDate, toDate)

            '取得した使用不可チップデータをチップ情報テーブルに格納する.
            Using chipInfoTable As New SC3150101DataSet.SC3150101ChipInfoDataTable
                Dim dataCount As Long = 0
                For Each dr As SC3150101DataSet.SC3150101UnavailableChipInfoRow In dtUnavailable.Rows

                    Dim chipInfoRow As SC3150101DataSet.SC3150101ChipInfoRow = CType(chipInfoTable.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)

                    chipInfoRow.DLRCD = userContext.DlrCD
                    chipInfoRow.STRCD = userContext.BrnCD
                    chipInfoRow.STARTTIME = dr.STARTTIME
                    chipInfoRow.ENDTIME = dr.ENDTIME
                    '実績ステータスに使用不可を示す値を格納する
                    chipInfoRow.STATUS = STATUS_UNAVAILABLE
                    'DBNull回避
                    chipInfoRow.REZID = -2
                    chipInfoRow.DSEQNO = 0
                    chipInfoRow.SEQNO = dataCount
                    'chipInfoRow.SERVICECODE_2 = "0"
                    chipInfoRow.RESULT_STALLID = stallId
                    chipInfoRow.STALLID = stallId
                    chipInfoRow.REZ_RECEPTION = ""
                    chipInfoRow.CUSTOMERNAME = ""
                    chipInfoRow.VEHICLENAME = ""
                    chipInfoRow.VCLREGNO = ""
                    chipInfoRow.INSDID = ""
                    chipInfoRow.CANCELFLG = ""
                    chipInfoRow.UPDATEACCOUNT = userContext.Account

                    chipInfoTable.Rows.Add(chipInfoRow)

                    dataCount = dataCount + 1
                Next

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                          , "{0}.{1} START" _
                                          , Me.GetType.ToString _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return chipInfoTable
            End Using
        End Using
    End Function
#End Region

#Region "ストール情報取得"

    ''' <summary>
    ''' ストール情報の取得.
    ''' </summary>
    ''' <returns>ストール情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetBelongStallData(ByVal stallId As Decimal) As SC3150101DataSet.SC3150101BelongStallInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BelongStallInfoDataTable
            Dim userContext As StaffContext = StaffContext.Current

            '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
            'ストール情報データセットを取得.
            'dt = adapter.GetBelongStallInfo(userContext.Account)

            dt = adapter.GetBelongStallInfo(userContext.Account, stallId)

            '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発　END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} END" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using

    End Function


    ''' <summary>
    ''' ストールに所属するエンジニア名の取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBelongStallStaffData(ByVal stallId As Decimal) As SC3150101DataSet.SC3150101BelongStallStaffDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BelongStallStaffDataTable
            Dim userContext As StaffContext = StaffContext.Current

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            Dim stfStallDispType As String = String.Empty
            Using tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
                stfStallDispType = tabletSmbCommonClass.GetStaffStallDispType(userContext.DlrCD, userContext.BrnCD)
            End Using

            ''スタッフ情報データセットを取得.
            'dt = adapter.GetBelongStallStaff(userContext.DlrCD, _
            '                                 userContext.BrnCD, _
            '                                 stallId)

            'スタッフ情報データセットを取得.
            dt = adapter.GetBelongStallStaff(userContext.DlrCD, _
                                             userContext.BrnCD, _
                                             stallId, _
                                             stfStallDispType)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} END" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return dt
        End Using

    End Function
#End Region

#Region "チップ情報（予約・実績）の取得"

    ''' <summary>
    '''   ストール（チップ）情報の取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="dateFrom">稼働時間From</param>
    ''' <param name="dateTo">稼働時間To</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallChipInfo(ByVal stallId As Decimal, _
                                     ByVal dateFrom As Date, _
                                     ByVal dateTo As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 予約チップ情報を取得
        Dim reserveChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        reserveChipInfo = GetReserveChipData(stallId, dateFrom, dateTo)


        ' 実績チップ情報を取得
        Dim resultChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        resultChipInfo = GetResultChipData(stallId, dateFrom, dateTo)

        '予約・実績チップ情報
        'Dim chipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
        '予約チップ情報と実績チップ情報を追加する.
        'chipInfo.Concat(reserveChipInfo)
        'chipInfo.Concat(resultChipInfo)
        reserveChipInfo.Merge(resultChipInfo, False)

        '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        If reserveChipInfo.Rows.Count > 0 Then
            '部品ステータスの取得
            reserveChipInfo = GetPartsStatus(stallId, reserveChipInfo)
        End If
        '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} END" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return reserveChipInfo

    End Function


    ''' <summary>
    ''' SMBに格納されている、実績ステータスを本システム用に変換する.
    ''' </summary>
    ''' <param name="SmbResultStatus">SMB上の実績ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeChipResultStatus(ByVal SmbResultStatus As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim parameterStatus As String = ""

        '取得したSMB上の実績ステータスより空白を除去する.
        If (Not IsDBNull(SmbResultStatus)) Then
            parameterStatus = Trim(SmbResultStatus)
        End If

        '返り値とする実績ステータスを初期化する.
        Dim resultStatus As String = RESULT_STATUS_WAITING
        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        'If ((SMB_RESULT_STATUS_OUT_SHED.Equals(parameterStatus)) Or _
        '    (SMB_RESULT_STATUS_OUT_SHED_00.Equals(parameterStatus)) Or _
        '    (SMB_RESULT_STATUS_IN_SHED.Equals(parameterStatus))) Then
        '    '未入庫の場合、待機中に設定
        '    resultStatus = RESULT_STATUS_WAITING
        'ElseIf (SMB_ResultStatusWorking.Equals(parameterStatus)) Then
        '    'SMBの実績ステータスが作業中の場合、作業中とする
        '    resultStatus = ResultStatusWorking
        '    'ElseIf (parameterStatus = "") Then
        'ElseIf String.IsNullOrEmpty(parameterStatus) = True Then
        '    resultStatus = RESULT_STATUS_WAITING
        'Else
        '    '上記条件以外の場合、作業完了とする
        '    resultStatus = RESULT_STATUS_FINISHED
        'End If

        If ((stallUseStetus00.Equals(parameterStatus)) Or _
           (stallUseStetus01.Equals(parameterStatus)) Or _
           (stallUseStetus07.Equals(parameterStatus))) Then
            '未入庫の場合、待機中に設定
            resultStatus = RESULT_STATUS_WAITING
            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

            'ElseIf (stallUseStetus02.Equals(parameterStatus)) Then
        ElseIf (stallUseStetus02.Equals(parameterStatus) Or stallUseStetus04.Equals(parameterStatus)) Then

            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発　END
            'ストール利用ステータスが作業中の場合、作業中とする
            resultStatus = ResultStatusWorking

        ElseIf String.IsNullOrEmpty(parameterStatus) = True Then
            resultStatus = RESULT_STATUS_WAITING
        Else
            '上記条件以外の場合、作業完了とする
            resultStatus = RESULT_STATUS_FINISHED
        End If
        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} END" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return resultStatus

    End Function


    ''' <summary>
    ''' 予約チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼動開始日時</param>
    ''' <param name="toDate">ストール稼動終了日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetReserveChipData(ByVal stallId As Decimal,
                                        ByVal fromDate As Date, _
                                        ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim userContext As StaffContext = StaffContext.Current
        Dim returnChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable ' 戻り値用

        Dim childChip As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable
        Dim childChipItem As SC3150101DataSet.SC3150101ChildChipOrderNoRow

        Dim reserveData As SC3150101DataSet.SC3150101ReserveChipInfoDataTable

        Dim adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
        '予約チップ情報の取得.
        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
        'Dim dtReserveData As SC3150101DataSet.SC3150101ReserveChipInfoDataTable

        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

        reserveData = adapter.GetReserveChipInfo(userContext.DlrCD, userContext.BrnCD, stallId, fromDate, toDate)

        ' 予約チップ情報をチップ情報データセットに格納する.
        Dim dtChipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
        Dim reserveItem As SC3150101DataSet.SC3150101ReserveChipInfoRow
        For Each reserveItem In reserveData.Rows

            'チップの作業時間が0以下の場合、チップ情報に追加しない.
            If reserveItem.REZ_WORK_TIME <= 0 Then
                Continue For
            End If

            '予約チップ情報を格納
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
            'Dim drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow = CType(dtChipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
            Dim chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow = _
                                    DirectCast(dtChipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

            '予約チップの値を格納
            chipInfoItem.DLRCD = userContext.DlrCD
            chipInfoItem.STRCD = userContext.BrnCD
            chipInfoItem.REZID = reserveItem.REZID
            chipInfoItem.DSEQNO = reserveItem.DSEQNO
            chipInfoItem.SEQNO = reserveItem.SEQNO
            chipInfoItem.VCLREGNO = reserveItem.VCLREGNO
            '予約チップのサービスコード_Sを、サービスコード項目に格納.
            'drChipInfo.SERVICECODE = reserveItem.SERVICECODE
            chipInfoItem.SERVICECODE = reserveItem.SERVICECODE_S
            chipInfoItem.RESULT_STATUS = ExchangeChipResultStatus(reserveItem.RESULT_STATUS)
            chipInfoItem.REZ_RECEPTION = reserveItem.REZ_RECEPTION
            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            chipInfoItem.IMP_VCL_FLG = reserveItem.IMP_VCL_FLG
            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            If (Not reserveItem.IsREZ_START_TIMENull()) Then
                Dim stringStartTime As String
                stringStartTime = ExchangeTimeString(reserveItem.REZ_START_TIME)
                'If (stringStartTime <> "") Then
                '    drChipInfo.REZ_START_TIME = CType(stringStartTime, Date)
                'End If
                If String.IsNullOrEmpty(stringStartTime) = False Then
                    chipInfoItem.REZ_START_TIME = CType(stringStartTime, Date)
                End If
            End If

            If (Not reserveItem.IsREZ_END_TIMENull()) Then
                Dim stringEndTime As String
                stringEndTime = ExchangeTimeString(reserveItem.REZ_END_TIME)
                'If (stringEndTime <> "") Then
                '    drChipInfo.REZ_END_TIME = CType(stringEndTime, Date)
                'End If
                If String.IsNullOrEmpty(stringEndTime) = False Then
                    chipInfoItem.REZ_END_TIME = CType(stringEndTime, Date)
                End If
            End If

            chipInfoItem.REZ_WORK_TIME = reserveItem.REZ_WORK_TIME

            If (Not reserveItem.IsUPDATE_COUNTNull()) Then
                chipInfoItem.UPDATE_COUNT = reserveItem.UPDATE_COUNT
            Else
                chipInfoItem.UPDATE_COUNT = 0
            End If

            chipInfoItem.UPDATEDATE = reserveItem.UPDATEDATE
            chipInfoItem.STARTTIME = reserveItem.STARTTIME
            chipInfoItem.ENDTIME = reserveItem.ENDTIME
            chipInfoItem.VEHICLENAME = reserveItem.VEHICLENAME

            If (Not reserveItem.IsSTATUSNull()) Then
                chipInfoItem.STATUS = reserveItem.STATUS
            Else
                chipInfoItem.STATUS = 0
            End If

            chipInfoItem.WALKIN = reserveItem.WALKIN
            chipInfoItem.STOPFLG = reserveItem.STOPFLG

            If (Not reserveItem.IsPREZIDNull()) And (reserveItem.PREZID <> -1) Then
                chipInfoItem.PREZID = reserveItem.PREZID
                ' R/O No. を取得
                childChip = adapter.GetChildOrderNo(chipInfoItem.DLRCD, chipInfoItem.STRCD, _
                                                    chipInfoItem.PREZID)
                childChipItem = DirectCast(childChip.Rows(0), SC3150101DataSet.SC3150101ChildChipOrderNoRow)
                If (Not childChipItem.IsORDERNONull()) Then
                    reserveItem.ORDERNO = childChipItem.ORDERNO ' 親チップの R/O No. をセット
                End If
            Else
                chipInfoItem.PREZID = 0
            End If

            chipInfoItem.REZCHILDNO = reserveItem.REZCHILDNO

            If (Not reserveItem.IsCRRYINTIMENull()) Then
                chipInfoItem.CRRYINTIME = reserveItem.CRRYINTIME
            End If

            If (Not reserveItem.IsCRRYOUTTIMENull()) Then
                chipInfoItem.CRRYOUTTIME = reserveItem.CRRYOUTTIME
            End If


            chipInfoItem.STRDATE = reserveItem.STRDATE
            chipInfoItem.CANCELFLG = reserveItem.CANCELFLG
            chipInfoItem.UPDATEACCOUNT = reserveItem.UPDATEACCOUNT
            chipInfoItem.SVCORGNMCT = reserveItem.SVCORGNMCT
            chipInfoItem.SVCORGNMCB = reserveItem.SVCORGNMCB
            chipInfoItem.RELATIONSTATUS = reserveItem.RELATIONSTATUS

            If (Not reserveItem.IsRELATION_UNFINISHED_COUNTNull()) Then
                chipInfoItem.RELATION_UNFINISHED_COUNT = reserveItem.RELATION_UNFINISHED_COUNT
            Else
                chipInfoItem.RELATION_UNFINISHED_COUNT = 0
            End If

            chipInfoItem.ORDERNO = reserveItem.ORDERNO

            'チップの作業時間が0以下の場合、チップ情報に追加しない.
            'If (drChipInfo.REZ_WORK_TIME > 0) Then
            '    dtChipInfo.Rows.Add(drChipInfo)
            'End If

            ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
            chipInfoItem.INSTRUCT = reserveItem.INSTRUCT
            If Not reserveItem.IsWORKSEQNull() Then
                chipInfoItem.WORKSEQ = reserveItem.WORKSEQ
            End If
            chipInfoItem.MERCHANDISEFLAG = reserveItem.MERCHANDISEFLAG
            ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

            '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成 START
            'ロケーション番号を格納
            If (Not reserveItem.IsPARKINGCODENull) AndAlso (Not String.IsNullOrEmpty(reserveItem.PARKINGCODE)) Then
                chipInfoItem.PARKINGCODE = reserveItem.PARKINGCODE
            Else
                chipInfoItem.PARKINGCODE = Space(1)
            End If
            '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成 END

            dtChipInfo.Rows.Add(chipInfoItem)
        Next

        'Logger.Info("GetReserveChipData End")
        'Return dtChipInfo
        returnChipInfo = CType(dtChipInfo.Copy, SC3150101DataSet.SC3150101ChipInfoDataTable)

        reserveData.Dispose()
        adapter.Dispose()
        dtChipInfo.Dispose()


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return returnChipInfo

    End Function


    ''' <summary>
    ''' 実績チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼働開始時間</param>
    ''' <param name="toDate">ストール稼動終了時間ｎ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetResultChipData(ByVal stallId As Decimal, _
                                       ByVal fromDate As Date, _
                                       ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim userContext As StaffContext = StaffContext.Current
        Dim returnChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable ' 戻り値用

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            '実績チップ情報の取得.
            Dim resultData As SC3150101DataSet.SC3150101ResultChipInfoDataTable
            resultData = adapter.GetResultChipInfo(userContext.DlrCD, userContext.BrnCD, stallId, fromDate, toDate)

            '実績チップ情報をチップ情報データセットに格納する.
            Using chipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
                Dim resultItem As SC3150101DataSet.SC3150101ResultChipInfoRow
                For Each resultItem In resultData.Rows

                    '実績チップ情報を格納
                    Dim chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow = _
                                            DirectCast(chipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)

                    chipInfoItem = Me.SetResultChipData(adapter, chipInfoItem, resultItem, userContext)

                    chipInfo.Rows.Add(chipInfoItem)
                Next

                returnChipInfo = DirectCast(chipInfo.Copy, SC3150101DataSet.SC3150101ChipInfoDataTable)

            End Using
        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} END" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnChipInfo

    End Function

    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
    ''' <summary>
    ''' 実績チップ情報を初期化
    ''' </summary>
    ''' <param name="chipInfoItem">実績チップ情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
    ''' </History>
    Private Function InitChipInfoItem(ByVal chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow) As SC3150101DataSet.SC3150101ChipInfoRow
        With chipInfoItem
            .VCLREGNO = ""
            .SERVICECODE = ""
            .REZ_WORK_TIME = 0
            .REZ_PICK_TIME = 0
            .REZ_DELI_TIME = 0
            .UPDATE_COUNT = 0
            .STATUS = 0
            .PREZID = 0
            .REZCHILDNO = 0
            .RELATION_UNFINISHED_COUNT = 0
            ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
            .INSTRUCT = "0"
            .MERCHANDISEFLAG = "0"
            ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

            '2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
            .INSPECTIONREQFLG = "0"
            '2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

        End With
        Return chipInfoItem
    End Function
    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

    ' 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応 START
    ''' <summary>
    ''' ストール実績情報をチップ用に形成
    ''' </summary>
    ''' <param name="adapter">データテーブルアクセスクラス</param>
    ''' <param name="chipInfoItem">チップ情報レコード（設定するテーブルのコピー）</param>
    ''' <param name="resultItem">ストール実績レコード</param>
    ''' <param name="userContext">ユーザー情報</param>
    ''' <returns>チップ情報レコード</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
    ''' </History>
    Private Function SetResultChipData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                       ByVal chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow, _
                                       ByVal resultItem As SC3150101DataSet.SC3150101ResultChipInfoRow, _
                                       ByVal userContext As StaffContext) As SC3150101DataSet.SC3150101ChipInfoRow

        Dim childChip As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable
        Dim childChipItem As SC3150101DataSet.SC3150101ChildChipOrderNoRow

        '実績チップ情報を初期化
        chipInfoItem = Me.InitChipInfoItem(chipInfoItem)

        '実績チップの値を格納
        chipInfoItem.DLRCD = userContext.DlrCD
        chipInfoItem.STRCD = userContext.BrnCD
        chipInfoItem.REZID = resultItem.REZID
        chipInfoItem.DSEQNO = resultItem.DSEQNO
        chipInfoItem.SEQNO = resultItem.SEQNO
        If (Not resultItem.IsVCLREGNONull()) Then : chipInfoItem.VCLREGNO = resultItem.VCLREGNO : End If
        If (Not resultItem.IsSERVICECODENull()) Then : chipInfoItem.SERVICECODE = resultItem.SERVICECODE : End If

        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        'chipInfoItem.RESULT_STATUS = ExchangeChipResultStatus(resultItem.RESULT_STATUS)
        chipInfoItem.RESULT_STATUS = ExchangeChipResultStatus(resultItem.STALL_USE_STATUS)
        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END 

        chipInfoItem.STALL_USE_STATUS = resultItem.STALL_USE_STATUS

        chipInfoItem.REZ_RECEPTION = resultItem.REZ_RECEPTION
        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        chipInfoItem.IMP_VCL_FLG = resultItem.IMP_VCL_FLG
        '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        If (Not resultItem.IsREZ_START_TIMENull()) Then
            Dim stringStartTime As String
            stringStartTime = ExchangeTimeString(resultItem.REZ_START_TIME)

            If String.IsNullOrEmpty(stringStartTime) = False Then
                chipInfoItem.REZ_START_TIME = CType(stringStartTime, Date)
            End If
        End If

        If (Not resultItem.IsREZ_END_TIMENull()) Then
            Dim stringEndTime As String
            stringEndTime = ExchangeTimeString(resultItem.REZ_END_TIME)

            If String.IsNullOrEmpty(stringEndTime) = False Then
                chipInfoItem.REZ_END_TIME = CType(stringEndTime, Date)
            End If
        End If

        If (Not resultItem.IsREZ_WORK_TIMENull()) Then : chipInfoItem.REZ_WORK_TIME = resultItem.REZ_WORK_TIME : End If
        If (Not resultItem.IsUPDATE_COUNTNull()) Then : chipInfoItem.UPDATE_COUNT = resultItem.UPDATE_COUNT : End If
        chipInfoItem.UPDATEDATE = resultItem.UPDATEDATE
        chipInfoItem.STARTTIME = resultItem.STARTTIME
        chipInfoItem.ENDTIME = resultItem.ENDTIME
        chipInfoItem.VEHICLENAME = resultItem.VEHICLENAME
        If (Not resultItem.IsSTATUSNull()) Then : chipInfoItem.STATUS = resultItem.STATUS : End If
        chipInfoItem.WALKIN = resultItem.WALKIN
        chipInfoItem.STOPFLG = resultItem.STOPFLG

        If (Not resultItem.IsPREZIDNull()) And (resultItem.PREZID <> -1) Then
            chipInfoItem.PREZID = resultItem.PREZID
            ' R/O No. を取得
            childChip = adapter.GetChildOrderNo(chipInfoItem.DLRCD, chipInfoItem.STRCD, chipInfoItem.PREZID)
            'childChipItem = CType(childChip.Rows(0), SC3150101DataSet.SC3150101ChildChipOrderNoRow)
            'childChipItem = childChip.NewRow
            'If Not IsDBNull(childChip) Then
            childChipItem = CType(childChip.Rows(0), SC3150101DataSet.SC3150101ChildChipOrderNoRow)
            'End If

            ' 親チップの R/O No. をセット
            If (Not childChipItem.IsORDERNONull()) Then : resultItem.ORDERNO = childChipItem.ORDERNO : End If
        End If

        If (Not resultItem.IsREZCHILDNONull()) Then : chipInfoItem.REZCHILDNO = resultItem.REZCHILDNO : End If
        chipInfoItem.STRDATE = resultItem.STRDATE
        chipInfoItem.CANCELFLG = resultItem.CANCELFLG
        chipInfoItem.UPDATEACCOUNT = resultItem.UPDATEACCOUNT
        chipInfoItem.SVCORGNMCT = resultItem.SVCORGNMCT
        chipInfoItem.SVCORGNMCB = resultItem.SVCORGNMCB
        chipInfoItem.RELATIONSTATUS = resultItem.RELATIONSTATUS
        If (Not resultItem.IsRELATION_UNFINISHED_COUNTNull()) Then : chipInfoItem.RELATION_UNFINISHED_COUNT = resultItem.RELATION_UNFINISHED_COUNT : End If
        chipInfoItem.ORDERNO = resultItem.ORDERNO

        If (Not resultItem.IsRESULT_START_TIMENull()) Then
            Dim stringResultStartTime As String = ExchangeTimeString(resultItem.RESULT_START_TIME)

            If String.IsNullOrEmpty(stringResultStartTime) = False Then
                chipInfoItem.RESULT_START_TIME = CType(stringResultStartTime, Date)
            End If
        End If

        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        'If resultItem.STALL_USE_STATUS.Equals("02") Then
        If resultItem.STALL_USE_STATUS.Equals("02") Or resultItem.STALL_USE_STATUS.Equals("04") Then
            '更新：2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            If (Not resultItem.IsPRMS_END_DATETIMENull()) Then
                Dim stringEndTime As String
                stringEndTime = ExchangeTimeString(resultItem.PRMS_END_DATETIME)

                If String.IsNullOrEmpty(stringEndTime) = False Then
                    chipInfoItem.RESULT_END_TIME = CType(stringEndTime, Date)
                End If
            End If
        Else
            If (Not resultItem.IsRESULT_END_TIMENull()) Then
                Dim stringResultEndTime As String = ExchangeTimeString(resultItem.RESULT_END_TIME)

                If String.IsNullOrEmpty(stringResultEndTime) = False Then
                    chipInfoItem.RESULT_END_TIME = CType(stringResultEndTime, Date)
                End If
            End If
        End If
        '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END 


        chipInfoItem.RESULT_IN_TIME = resultItem.RESULT_IN_TIME
        chipInfoItem.RESULT_WORK_TIME = resultItem.RESULT_WORK_TIME
        chipInfoItem.REZ_PICK_DATE = resultItem.REZ_PICK_DATE
        If (Not resultItem.IsREZ_PICK_TIMENull()) Then : chipInfoItem.REZ_PICK_TIME = resultItem.REZ_PICK_TIME : End If
        chipInfoItem.REZ_DELI_DATE = resultItem.REZ_DELI_DATE
        If (Not resultItem.IsREZ_DELI_TIMENull()) Then : chipInfoItem.REZ_DELI_TIME = resultItem.REZ_DELI_TIME : End If
        chipInfoItem.RESULT_WAIT_END = resultItem.RESULT_WAIT_END

        ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        chipInfoItem.INSTRUCT = resultItem.INSTRUCT
        If Not resultItem.IsWORKSEQNull() Then : chipInfoItem.WORKSEQ = resultItem.WORKSEQ : End If
        chipInfoItem.MERCHANDISEFLAG = resultItem.MERCHANDISEFLAG
        ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

        '2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
        chipInfoItem.INSPECTIONREQFLG = resultItem.INSPECTIONREQFLG
        '2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

        Return chipInfoItem
    End Function
    ' 2012/06/14 KN 西田 STEP1 重要課題対応 DevPartner指摘対応 END

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 部品ステータス取得
    ''' </summary>
    ''' <param name="inStallId"></param>
    ''' <param name="dtChipInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsStatus(ByVal inStallId As Decimal, _
                                    ByVal dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "{0}.{1} START. STALL_ID:{2}" _
                              , Me.GetType.ToString _
                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                              , inStallId))


        'ログインスタッフ情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)

        'データテーブル宣言
        Dim dtPartStatus As IC3802503DataSet.IC3802503PartsStatusDataTable
        Dim dtRepairOrderSeq As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable

        'データロウ宣言
        Dim drRepairOrderSeq As SC3150101DataSet.SC3150101GetRepairOrderSequenceRow

        '本日の指定ストールにあるチップのRO番号とRO連番を取得する
        dtRepairOrderSeq = GetTodayRepiarOrder(userContext.DlrCD, userContext.BrnCD, inStallId, nowDateTime)

        '1件も取得できなった場合処理終了
        If (IsNothing(dtRepairOrderSeq)) OrElse _
           (dtRepairOrderSeq.Rows.Count = 0) Then
            Return dtChipInfo
        End If

        'データテーブル宣言
        Using dtRepairOrderNumInfo As New IC3802503DataSet.IC3802503RONumInfoDataTable
            '取得したデータを引数用のデータテーブルに格納する
            For Each drRepairOrderSeq In dtRepairOrderSeq
                'データロウ宣言
                Dim drRepairOrderNumInfo As IC3802503DataSet.IC3802503RONumInfoRow = dtRepairOrderNumInfo.NewIC3802503RONumInfoRow
                'RO番号とRO連番を格納
                drRepairOrderNumInfo.R_O = drRepairOrderSeq.RO_NUM
                drRepairOrderNumInfo.R_O_SEQNO = drRepairOrderSeq.RO_SEQ
                'データテーブルに行を追加
                dtRepairOrderNumInfo.Rows.Add(drRepairOrderNumInfo)
            Next

            '部品ステータス情報の取得
            dtPartStatus = GetPartsStatusList(userContext.DlrCD, userContext.BrnCD, dtRepairOrderNumInfo)
        End Using

        'データが取得できなかった場合処理しない
        If (Not IsNothing(dtPartStatus)) AndAlso _
            (Not dtPartStatus.Rows.Count = 0) Then

            '0行目のリザルトコードの確認
            If IC3802503BusinessLogic.Result.Success <> dtPartStatus.Item(0).ResultCode Then
                'エラーコードを格納
                dtChipInfo.Item(0).ERROR_CODE = OtherSystemsReturnCodeSelect(dtPartStatus.Item(0).ResultCode)

                Return dtChipInfo
            End If

            '結果コードが0ならステータスの格納処理
            dtRepairOrderSeq = PartsIssueStatusStorage(dtRepairOrderSeq, dtPartStatus)

            '部品準備が完了しているかの判定処理
            dtChipInfo = PartsFlgJudgment(dtChipInfo, dtRepairOrderSeq)

        Else
            Logger.Info("PartsDetailData is Nothing")
        End If


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} END" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtChipInfo

    End Function

    ''' <summary>
    ''' 指定された日付とストールにあるチップのRO番号とRO連番を取得する
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inTodaydateTime">指定日付</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTodayRepiarOrder(ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String, _
                                         ByVal inStallId As Decimal, _
                                         ByVal inTodaydateTime As Date) As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} START. DLR_CD:{2}, BRN_CD:{3}, STALL_ID:{4} ,DATE:{5}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , inDealerCode _
                             , inBranchCode _
                             , inStallId.ToString(CultureInfo.CurrentCulture()) _
                             , inTodaydateTime.ToString(CultureInfo.CurrentCulture())))

        '返却値
        Dim dtRepairOrderSeq As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable = Nothing

        '指定の日付とストールにあるチップのRO番号とRO連番を取得する
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            dtRepairOrderSeq = adapter.GetRepairOrderSequence(inDealerCode, inBranchCode, inStallId, inTodaydateTime)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtRepairOrderSeq

    End Function

    ''' <summary>
    ''' 部品ステータス情報の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inDtRepairOrderNumInfo">引数用データテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsStatusList(ByVal inDealerCode As String, _
                                        ByVal inBranchCode As String, _
                                        ByVal inDtRepairOrderNumInfo As IC3802503DataSet.IC3802503RONumInfoDataTable) As IC3802503DataSet.IC3802503PartsStatusDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} START. DLR_CD:{2}, BRN_CD:{3}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , inDealerCode _
                         , inBranchCode))

        '返却値
        Dim dtPartStatus As IC3802503DataSet.IC3802503PartsStatusDataTable = Nothing

        Using IC3802504BizLogic As New IC3802503BusinessLogic

            '部品ステータス情報の取得
            dtPartStatus = IC3802504BizLogic.GetPartsStatusList(inDealerCode, inBranchCode, inDtRepairOrderNumInfo)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtPartStatus

    End Function

    ''' <summary>
    ''' 部品準備完了ステータスの格納処理
    ''' </summary>
    ''' <param name="inDtRepairOrderSeq">RO番号・RO作業連番テーブル</param>
    ''' <param name="inDtPartStatus">パーツステータステーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PartsIssueStatusStorage(ByVal inDtRepairOrderSeq As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable, _
                                             ByVal inDtPartStatus As IC3802503DataSet.IC3802503PartsStatusDataTable) As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} START" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '行数分繰り返す
        For Each drRepairOrderSeq In inDtRepairOrderSeq
            For Each drPartStatus In inDtPartStatus
                'RO番号、RO連番が一致する場合処理する
                If (drRepairOrderSeq.RO_NUM.Equals(drPartStatus.R_O)) AndAlso
                    (drRepairOrderSeq.RO_SEQ.Equals(drPartStatus.R_O_SEQNO)) Then

                    If (Not drPartStatus.IsPARTS_ISSUE_STATUSNull) AndAlso
                        (Not String.IsNullOrEmpty(drPartStatus.PARTS_ISSUE_STATUS)) Then

                        '取得した部品準備完了ステータスを格納
                        drRepairOrderSeq.PARTS_ISSUE_STATUS = drPartStatus.PARTS_ISSUE_STATUS
                        Exit For
                    End If
                End If
            Next
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return inDtRepairOrderSeq
    End Function

    ''' <summary>
    ''' 部品準備判定処理
    ''' </summary>
    ''' <param name="inDtChipInfo">チップ情報テーブル</param>
    ''' <param name="inDtRepairOrderSeq">RO番号・RO作業連番テーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PartsFlgJudgment(ByVal inDtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable, _
                                      ByVal inDtRepairOrderSeq As SC3150101DataSet.SC3150101GetRepairOrderSequenceDataTable) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} START" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '変数宣言
        Dim partsStatusCompleteCount As Integer = 0
        Dim partsStatusRowCount As Integer = 0

        'チップの数だけ繰り返す
        For Each drChipInfo In inDtChipInfo
            'RO番号がない場合処理しない
            If Not String.IsNullOrEmpty(drChipInfo.ORDERNO) Then

                For Each drRepairOrderSeq In inDtRepairOrderSeq

                    'チップの作業内容IDと作業指示の作業内容IDが一致した場合
                    If (drChipInfo.SEQNO.Equals(drRepairOrderSeq.JOB_DTL_ID)) Then
                        'カウントに＋１
                        partsStatusRowCount += 1
                        '部品ステータスが部品準備完了の場合
                        If (AllPartsIssuedCompletely.Equals(drRepairOrderSeq.PARTS_ISSUE_STATUS)) Then
                            'カウントに＋１
                            partsStatusCompleteCount += 1
                        End If
                    End If
                Next
                '作業内容IDと部品準備完了の数が同じ場合、部品準備完了とする
                If (Not partsStatusRowCount = 0) AndAlso _
                    (partsStatusRowCount = partsStatusCompleteCount) Then

                    drChipInfo.MERCHANDISEFLAG = partsFlg

                End If
                ' カウントをリセット
                partsStatusRowCount = 0
                partsStatusCompleteCount = 0
            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} END" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return inDtChipInfo

    End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

#End Region

#Region "実績チップの取得"
    ''' <summary>
    '''   実績チップ情報を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="dateFrom">稼働時間FROM</param>
    ''' <param name="dateTo">稼働時間TO</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcessChipInfo(ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Decimal, _
                                       ByVal dateFrom As Date, _
                                       ByVal dateTo As Date) As SC3150101DataSet.SC3150101ResultChipInfoDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetProcessChipInfo", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "DATE_FROM:" & CType(dateFrom, String), "DATE_TO:" & CType(dateTo, String))

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            ' ストール実績情報を取得する
            Dim processChipInfo As SC3150101DataSet.SC3150101ResultChipInfoDataTable
            processChipInfo = adapter.GetResultChipInfo(dealerCode, branchCode, stallId, dateFrom, dateTo)
            Dim drProcessChipInfo As SC3150101DataSet.SC3150101ResultChipInfoRow
            If processChipInfo Is Nothing Then
                Return Nothing
            End If
            For Each drProcessChipInfo In processChipInfo.Rows
                drProcessChipInfo.REZID = SetLongNumerData(drProcessChipInfo.Item("REZID"), 0)
                drProcessChipInfo.DSEQNO = SetLongNumerData(drProcessChipInfo.Item("DSEQNO"), 0)
                drProcessChipInfo.SEQNO = SetLongNumerData(drProcessChipInfo.Item("SEQNO"), 0)
                drProcessChipInfo.MODELCODE = SetStringData(drProcessChipInfo.Item("MODELCODE"), "")
                drProcessChipInfo.VCLREGNO = SetStringData(drProcessChipInfo.Item("VCLREGNO"), "")
                drProcessChipInfo.SERVICECODE = SetStringData(drProcessChipInfo.Item("SERVICECODE"), "")
                'drProcessChipInfo.SERVICECODE_MST = SetStringData(drProcessChipInfo.Item("SERVICECODE_MST"), "")
                drProcessChipInfo.RESULT_STATUS = SetStringData(drProcessChipInfo.Item("RESULT_STATUS"), "")
                drProcessChipInfo.RESULT_STALLID = SetLongNumerData(drProcessChipInfo.Item("RESULT_STALLID"), 0)
                drProcessChipInfo.RESULT_START_TIME = SetStringData(drProcessChipInfo.Item("RESULT_START_TIME"), "")
                drProcessChipInfo.RESULT_END_TIME = SetStringData(drProcessChipInfo.Item("RESULT_END_TIME"), "")
                drProcessChipInfo.RESULT_IN_TIME = SetStringData(drProcessChipInfo.Item("RESULT_IN_TIME"), "")
                drProcessChipInfo.RESULT_WORK_TIME = SetLongNumerData(drProcessChipInfo.Item("RESULT_WORK_TIME"), 0)
                drProcessChipInfo.REZ_RECEPTION = SetStringData(drProcessChipInfo.Item("REZ_RECEPTION"), "")
                drProcessChipInfo.REZ_START_TIME = SetStringData(drProcessChipInfo.Item("REZ_START_TIME"), "")
                drProcessChipInfo.REZ_END_TIME = SetStringData(drProcessChipInfo.Item("REZ_END_TIME"), "")
                drProcessChipInfo.REZ_WORK_TIME = SetLongNumerData(drProcessChipInfo.Item("REZ_WORK_TIME"), 0)
                drProcessChipInfo.REZ_WORK_TIME_2 = SetLongNumerData(drProcessChipInfo.Item("REZ_WORK_TIME_2"), 0)
                drProcessChipInfo.REZ_PICK_DATE = SetStringData(drProcessChipInfo.Item("REZ_PICK_DATE"), "")
                drProcessChipInfo.REZ_PICK_TIME = SetLongNumerData(drProcessChipInfo.Item("REZ_PICK_TIME"), 0)
                drProcessChipInfo.REZ_DELI_DATE = SetStringData(drProcessChipInfo.Item("REZ_DELI_DATE"), "")
                drProcessChipInfo.REZ_DELI_TIME = SetLongNumerData(drProcessChipInfo.Item("REZ_DELI_TIME"), 0)
                drProcessChipInfo.RESULT_WAIT_END = SetStringData(drProcessChipInfo.Item("RESULT_WAIT_END"), "")
                drProcessChipInfo.UPDATE_COUNT = SetLongNumerData(drProcessChipInfo.Item("UPDATE_COUNT"), 0)
                drProcessChipInfo.INPUTACCOUNT = SetStringData(drProcessChipInfo.Item("INPUTACCOUNT"), "")
                '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                drProcessChipInfo.STALL_USE_STATUS = SetStringData(drProcessChipInfo.Item("STALL_USE_STATUS"), "")
                drProcessChipInfo.PRMS_END_DATETIME = SetStringData(drProcessChipInfo.Item("PRMS_END_DATETIME"), "")
                '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
                If IsDBNull(drProcessChipInfo.Item("UPDATEDATE")) Then
                    drProcessChipInfo.Item("UPDATEDATE") = ""
                End If
                If IsDBNull(drProcessChipInfo.Item("STARTTIME")) Then
                    drProcessChipInfo.Item("STARTTIME") = ""
                End If
                If IsDBNull(drProcessChipInfo.Item("ENDTIME")) Then
                    drProcessChipInfo.Item("ENDTIME") = ""
                End If
                drProcessChipInfo.CUSTOMERNAME = SetStringData(drProcessChipInfo.Item("CUSTOMERNAME"), "")
                drProcessChipInfo.VEHICLENAME = SetStringData(drProcessChipInfo.Item("VEHICLENAME"), "")
                drProcessChipInfo.STATUS = SetLongNumerData(drProcessChipInfo.Item("STATUS"), 0)
                drProcessChipInfo.INSDID = SetStringData(drProcessChipInfo.Item("INSDID"), "")
                drProcessChipInfo.WALKIN = SetStringData(drProcessChipInfo.Item("WALKIN"), "")
                drProcessChipInfo.STOPFLG = SetStringData(drProcessChipInfo.Item("STOPFLG"), "")
                drProcessChipInfo.PREZID = SetLongNumerData(drProcessChipInfo.Item("PREZID"), 0)
                drProcessChipInfo.REZCHILDNO = SetLongNumerData(drProcessChipInfo.Item("REZCHILDNO"), 0)
                If IsDBNull(drProcessChipInfo.Item("STRDATE")) Then
                    drProcessChipInfo.STRDATE = DateTime.MinValue
                End If
                drProcessChipInfo.ACCOUNT_PLAN = SetStringData(drProcessChipInfo.Item("ACCOUNT_PLAN"), "")
                drProcessChipInfo.CANCELFLG = SetStringData(drProcessChipInfo.Item("CANCELFLG"), "")
                drProcessChipInfo.UPDATEACCOUNT = SetStringData(drProcessChipInfo.Item("UPDATEACCOUNT"), "")
                drProcessChipInfo.SVCORGNMCT = SetStringData(drProcessChipInfo.Item("SVCORGNMCT"), "")
                drProcessChipInfo.SVCORGNMCB = SetStringData(drProcessChipInfo.Item("SVCORGNMCB"), "")
                drProcessChipInfo.RELATIONSTATUS = SetStringData(drProcessChipInfo.Item("RELATIONSTATUS"), "")
                drProcessChipInfo.RELATION_UNFINISHED_COUNT = SetLongNumerData(drProcessChipInfo.Item("RELATION_UNFINISHED_COUNT"), 0)
                drProcessChipInfo.ORDERNO = SetStringData(drProcessChipInfo.Item("ORDERNO"), "")
                'drChipInfo.USERNAME = reserveItem.USERNAME
            Next

            OutputLog(LOG_TYPE_INFO, "[E]GetProcessChipInfo", "", Nothing, "RET:(DataSet)")
            Return processChipInfo

        End Using

    End Function

#End Region

#Region "開始処理"

    '2012/11/05 TMEJ彭健  問連修正（GTMC121029047）START

    ''' <summary>
    '''   開始処理を行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateCount">更新カウント</param>
    ''' <param name="orderNo">R/O No.</param>
    ''' <param name="restartStopJobFlg">中断中作業の開始フラグ</param>
    ''' <param name="isBreak">休憩有無(とる：True、とらない：False)</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <returns>正常終了：0、異常終了：エラーコード、例外：-1</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
    '''                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正
    ''' </History>
    <EnableCommit()>
    Public Function StartWork(ByVal dealerCode As String, _
                              ByVal branchCode As String, _
                              ByVal reserveId As Decimal, _
                              ByVal stallId As Decimal, _
                              ByVal updateAccount As String, _
                              ByVal updateCount As Long, _
                              ByVal orderNo As String, _
                              ByVal restartStopJobFlg As Boolean, _
                              Optional ByVal isBreak As Boolean = False, _
                              Optional ByVal breakBottomFlg As Boolean = False) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]StartWork", "", Nothing, _
                                 "DLRCD:" & dealerCode, _
                                 "STRCD:" & branchCode, _
                                 "REZID:" & CType(reserveId, String), _
                                 "STALLID:" & CType(stallId, String), _
                                 "ACCOUNT:" & updateAccount, _
                                 "UPDATECOUNT:" & updateCount.ToString(CultureInfo.CurrentCulture), _
                                 "ORDERNO:" & orderNo, _
                                 "restartStopJobFlg:" & restartStopJobFlg.ToString(CultureInfo.CurrentCulture()))

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '' 戻り値にエラーを設定
        'StartWork = ReturnNG

        ' 戻り値に正常を設定
        StartWork = ReturnOk

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Dim adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                            New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
        Dim userContext As StaffContext = StaffContext.Current

        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        '当日の日付取得
        'Dim todayDate As Date = nowDateTime.Date
        ''SMBコモンクラスのインスタンス宣言
        'Dim SmbCommonClass As New SMBCommonClassBusinessLogic
        '' チップ衝突フラグ
        'Dim IsCollisionFlg As Boolean = False

        'タブレットSMBコモンクラスのインスタンス宣言
        Dim tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        Try
            ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
            '                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 START
            ' 中断中作業の開始なしの場合
            If Not restartStopJobFlg Then
                ' チップエンティティ
                Dim chipEntityTable As TabletSmbCommonClassChipEntityDataTable = Nothing

                ' TabletSMBCommonClassのテーブルアダプタークラスインスタンスを生成
                Using myTableAdapter As New TabletSMBCommonClassDataAdapter
                    ' チップ情報取得
                    chipEntityTable = myTableAdapter.GetChipEntity(reserveId, 0)
                End Using

                ' 未開始JOBありフラグ
                Dim isBeforeStartJobExsitsFlag As Boolean = False
                ' チップエンティティ行データ
                Dim chipEntityRowData As TabletSmbCommonClassChipEntityRow = Nothing

                For Each chipEntityRowData In chipEntityTable
                    ' 未開始Job存在判定
                    isBeforeStartJobExsitsFlag = tabletSmbCommonClass.HasBeforeStartJob(chipEntityRowData.JOB_DTL_ID)

                    ' 未開始JOBなしの場合
                    If Not isBeforeStartJobExsitsFlag Then
                        Exit For
                    End If
                Next chipEntityRowData

                chipEntityTable = Nothing

                ' 未開始JOBなしの場合
                If Not isBeforeStartJobExsitsFlag Then
                    StartWork = WordCdNoUnstartedJobToStart
                    Exit Try
                End If
            End If
            ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
            '                      [TKM]PUAT-4178　TCメインの作業開始にて、1つもJOBが開始されないが、チップが作業中になる を修正 END

            ' (実際の)作業開始日時を取得する
            Dim actualStratTime As Date = DateTimeFunc.Now(dealerCode)

            ' 2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            'Dim startTime As Date = CType(DateTimeFunc.FormatDate(2, actualStratTime), Date)    '秒の切り捨て(チップ衝突判定回避) 
            Dim startTime As Date = actualStratTime.AddSeconds(-actualStratTime.Second)
            ' 2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            ' -------------------------------------------------------------------------------------
            ' ストール予約情報を取得する
            ' -------------------------------------------------------------------------------------
            Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = _
                                    adapter.GetStallReserveInfo(dealerCode, branchCode, reserveId)
            If reserveInfo Is Nothing Then
                ' ストール予約情報の取得に失敗
                OutputLog(LOG_TYPE_INFO, "StartWork", _
                          "Failed to get the stall reservation information. (reserveId:" & reserveId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                StartWork = ReturnNG

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If
            ' DBNullの項目にデフォルト値を設定する
            reserveInfo = SetStallReserveDefaultValue(reserveInfo)
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                                    DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            ' 他システム変更チェック
            If drReserveInfo.UPDATE_COUNT <> updateCount Then
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", "This chip has been modified by another operator. Please reload and try again.", Nothing)
                StartWork = 923     '「既に他のオペレータによって更新されています。画面をリロードしてからもう一度お試し下さい。」
                Exit Try
            End If

            ' -------------------------------------------------------------------------------------
            ' ストール時間を取得する
            ' -------------------------------------------------------------------------------------
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = _
                                adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)
            If stallInfo Is Nothing Then
                ' ストール時間情報の取得に失敗
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", _
                             "Failed to get the stall time information. (stallId:" & stallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                StartWork = ReturnNG

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = _
                                DirectCast(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

            ' 稼動開始時刻
            Dim startOperationTime As TimeSpan = Me.SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            ' 稼動終了時刻
            Dim endOperationTime As TimeSpan = Me.SetStallTime(drStallInfo.PENDTIME).TimeOfDay

            ' *************************************************************************************
            ' 【ストール稼動時間内の開始時間かチェックする。】
            ' *************************************************************************************
            Dim resultOperationTime As Integer = Me.DecisionOperationTime(reserveInfo, _
                                                                          startTime, _
                                                                          startOperationTime, _
                                                                          endOperationTime)
            If resultOperationTime <> 0 Then
                ' 稼働時間外開始
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", "Cannot start. Out of available stall hours.", Nothing)
                StartWork = 914     '「開始できませんでした。ストールの稼動時間外です。」
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' ストール実績情報を取得する
            ' ---------------------------------------------------------------------------------
            Dim procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable = _
                                adapter.GetStallProcessInfo(dealerCode, branchCode, reserveId)
            If procInfo Is Nothing Then
                ' ストール実績情報の取得に失敗
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", _
                          "Failed to get the stall process information. (reserveId:" & reserveId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                StartWork = ReturnNG

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            ' 日跨ぎ開始放置チップの翌日2重開始チェック(TCメイン画面では開始ボタン自体が出ないはず)
            Dim drProc As SC3150101DataSet.SC3150101StallProcessInfoRow = _
                                DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

            ' *************************************************************************************
            ' 【当該チップの実績ステータスチェック（作業中以降でないこと）。】
            ' *************************************************************************************
            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            'If Not IsDBNull(drProc.Item("RESULT_STATUS")) _
            '   AndAlso SMB_ResultStatusWorking.Equals(drProc.RESULT_STATUS) Then

            If Not IsDBNull(drProc.Item("STALL_USE_STATUS")) _
              AndAlso stallUseStetus02.Equals(drProc.STALL_USE_STATUS) Then
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                ' すでに作業開始されている
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", "The selected chip has already started.", Nothing)
                StartWork = 915     '「開始できませんでした。選択チップはすでに開始されています。」
                Exit Try
            End If

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

            '' *************************************************************************************
            '' 【二重開始チェックチェック。】
            '' *************************************************************************************
            'Dim resultMultiStarts As Integer = Me.CheckMultiStarts(dealerCode, _
            '                                                       branchCode, _
            '                                                       stallId, _
            '                                                       startTime, _
            '                                                       startOperationTime, _
            '                                                       endOperationTime)
            'If resultMultiStarts <> 0 Then
            '    ' すでに作業開始されている
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "Cannot start. There already is another working chip. (stallId:" & stallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            '    StartWork = 916     '「開始できませんでした。すでに作業中のチップがあります。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【ステータスチェック（本予約かどうか）。】
            '' *************************************************************************************
            'If SMB_STATUS_PROPOSED_RESOURCE = drReserveInfo.STATUS Then
            '    ' ストール仮予約のチップ
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "The selected chip is not a permanent reservation.", Nothing)
            '    StartWork = 902     '「選択チップは本予約ではありません。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【入庫済みチェック。】
            '' *************************************************************************************
            'If (IsDBNull(drReserveInfo.Item("STRDATE"))) _
            '    OrElse (drReserveInfo.STRDATE = DateTime.MinValue) Then
            '    ' 未入庫のチップ
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "The selected chip is not service-in.", Nothing)
            '    StartWork = 901     '「選択チップは入庫済みではありません。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【着工指示完了チェック。】
            '' *************************************************************************************
            'If Not INSTRUCT_READY.Equals(drReserveInfo.INSTRUCT) Then
            '    ' 未着工のチップ
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "Cannot start. The selected chip is not ready to start.", Nothing)
            '    StartWork = 920     '「開始できませんでした。選択チップは着工開始準備が完了していません。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【作業担当者の存在チェック。】
            '' *************************************************************************************
            'If Me.IsStallStaffCount(adapter, _
            '                        stallId) <> ReturnOk Then
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "Cannot start. There is no person in charge. (stallId:" & stallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            '    StartWork = 917     '「開始できませんでした。作業担当者が存在しません。」
            '    Exit Try
            'End If

            '開始処理事前チェック
            Dim resultCheckStartWork As Integer = Me.CheckStartWork(dealerCode, _
                                                                    branchCode, _
                                                                    stallId, _
                                                                    startTime, _
                                                                    startOperationTime, _
                                                                    endOperationTime, _
                                                                    drReserveInfo, _
                                                                    adapter)

            '処理結果チェック
            If resultCheckStartWork <> ReturnOk Then
                '「0：成功」以外の場合
                '処理結果を返却
                Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "Cannot start", Nothing)
                StartWork = resultCheckStartWork
                Exit Try

            End If

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            ''作業連番を取得
            'Dim workSeq As Integer = 0
            'If Not drReserveInfo.IsWORKSEQNull() AndAlso _
            '   Not WORKSEQ_NOPLAN_PARENT.Equals(drReserveInfo.WORKSEQ.ToString(CultureInfo.InvariantCulture)) Then
            '    workSeq = CType(drReserveInfo.WORKSEQ, Integer)
            'End If

            '' *************************************************************************************
            '' 親R/Oが着工済みでなければ、追加作業のチップが開始できない
            '' *************************************************************************************
            'If workSeq > 0 Then
            '    If Not drReserveInfo.IsPREZIDNull AndAlso drReserveInfo.PREZID > 0 Then
            '        If Me.GetStartedChipCountOfInitialRO(adapter, dealerCode, branchCode, drReserveInfo.PREZID) <= 0 Then
            '            Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "Cannot start. Master R/O has not started.", Nothing, _
            '                         "workSeq=" & workSeq.ToString(CultureInfo.CurrentCulture) & _
            '                         ",rezid=" & drReserveInfo.REZID.ToString(CultureInfo.CurrentCulture))
            '            StartWork = 921 '「无法开始追加作业。母R/O作业未开始。」
            '            Exit Try
            '        End If
            '    Else
            '        Me.OutputLog(LOG_TYPE_ERROR, "StartWork", "Wrong data in TBL_STALLREZINFO", Nothing, _
            '                     "workSeq=" & workSeq.ToString(CultureInfo.CurrentCulture) & _
            '                     ",rezid=" & drReserveInfo.REZID.ToString(CultureInfo.CurrentCulture))
            '    End If
            'End If
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            '' *************************************************************************************
            '' 【全リレーションChip内の開始中チェック。】
            '' *************************************************************************************
            'If Not drReserveInfo.IsPREZIDNull AndAlso drReserveInfo.PREZID > 0 Then      ' 管理予約IDがNULLの場合、単独チップのため処理を行わない
            '    If Me.GetRelatedWorkingChipCount(adapter, dealerCode, branchCode, drReserveInfo.PREZID) > 0 Then
            '        Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "Cannot start. There already is another working Chip. (prezid:" & drReserveInfo.PREZID.ToString(CultureInfo.CurrentCulture) & ")", Nothing)
            '        StartWork = 916     '「開始できませんでした。すでに作業中のチップがあります。」
            '        Exit Try
            '    End If
            'End If
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内の予約情報の取得
            ' ---------------------------------------------------------------------------------
            ' ストール開始時間
            Dim stallStartTime As TimeSpan = startOperationTime
            ' ストール終了時間
            Dim stallEndTime As TimeSpan = endOperationTime
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDate As Date = startTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDate As Date = GetEndDateRange(fromDate, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                        adapter.GetStallReserveList(dealerCode, branchCode, _
                                                                    stallId, reserveId, fromDate, toDate)
            ' 指定範囲内のストール実績情報を取得
            Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                        adapter.GetStallProcessList(dealerCode, branchCode, _
                                                                    stallId, fromDate, toDate)
            ' 指定範囲内の予約情報の取得
            reserveList = GetReserveList(reserveList, processList, stallId, _
                                            reserveId, fromDate, isBreak)



            ' ---------------------------------------------------------------------------------
            ' 休憩取得有無判定
            ' CheckBreak()
            ' ---------------------------------------------------------------------------------
            ' 休憩時間帯・使用不可時間帯取得
            'Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
            '                                adapter.GetBreakSlot(stallId, fromDate, toDate)

            'Dim reserveStartTime As Date = drReserveInfo.STARTTIME
            'Dim reserveEndTime As Date = drReserveInfo.ENDTIME
            'Dim reserveWorkTime As Integer = CType(drReserveInfo.REZ_WORK_TIME, Integer)
            ' 休憩取得有無判定
            'Dim resultBreak As Boolean = CheckBreak(breakInfo, isBreak, reserveStartTime, _
            '                                        reserveEndTime, reserveWorkTime)


            ' 予約の作業終了予定日時を算出
            ' (実際の)予定終了時刻を算出する
            'Dim workTime As Integer = reserveWorkTime
            'Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date
            'Dim startTimeTemp As Date = startTime
            'dateTemp = CalculateEndTime(stallInfo, _
            '                            dealerCode, branchCode, stallId, _
            '                            startTimeTemp, workTime, _
            '                            resultBreak)
            'Dim endTime As Date = dateTemp(END_TIME_END)

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ' 時間の見直し
            'Dim dateArray(2) As Date
            'dateArray = RevisionTime(startTime, endTime, CType(drStallInfo.TIMEINTERVAL, Integer))

            'Dim startTimeRevision As Date = dateArray(0)
            'Dim startTimeRevision As Date = startTime
            'Dim endTimeRevision As Date = dateArray(1)
            'Dim endTimeRevision As Date = endTime

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            '' 休憩取得ボタン押下有無判定
            'drProc.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProc.REST_FLG)

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定する場合
                If biz.IsRestAutoJudge() Then
                    drProc.REST_FLG = REST_FLG_TAkE
                Else
            ' 休憩取得ボタン押下有無判定
            drProc.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProc.REST_FLG)
                End If
            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '秒数を切り捨てる
            Dim rsltStartDateTime As Date = nowDateTime.AddSeconds(-nowDateTime.Second)

            'チップの作業開始処理
            Dim returnValue As Long = tabletSmbCommonClass.Start(drProc.REZID _
                                                                 , rsltStartDateTime _
                                                                 , drProc.REST_FLG _
                                                                 , nowDateTime _
                                                                 , updateCount _
                                                                 , APPLICATION_ID _
                                                                 , restartStopJobFlg)

            '作業開始処理が失敗した場合
            If returnValue <> ActionResult.Success Then

                '出力するエラーメッセージNoを設定
                StartWork = OtherSystemsReturnCodeSelect(returnValue, workStartFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''作業開始処理に失敗しました
                'OutputLog(LOG_TYPE_ERROR, "tabletSmbCommonClass.Start", "Failed to start of processing chip.", Nothing)
                'Exit Try

                'エラー内容チェック
                If StartWork <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    Me.OutputLog(LOG_TYPE_WARNING, "tabletSmbCommonClass.Start", "Failed to start of processing chip.", Nothing)
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            '' *************************************************************************************
            '' 【作業開始にあたり、他チップの移動処理に失敗しないかチェック。】
            '' *************************************************************************************

            '' 衝突有無判定
            'If IsCollision(reserveList, reserveId, startTime, endTimeRevision) = True Then

            '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            '    'ストールロックテーブル登録処理
            '    If SmbCommonClass.RegisterStallLock(stallId, _
            '                                        todayDate, _
            '                                        updateAccount, _
            '                                        nowDateTime, _
            '                                        APPLICATION_ID) <> ReturnOk Then
            '        Exit Try
            '    End If

            '    IsCollisionFlg = True
            '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
            '    ' 衝突チップを移動する
            '    'Dim resultMoveChip As Integer = MoveCollisionChip(reserveList, stallInfo, breakInfo, dealerCode, _
            '    '                                                    branchCode, reserveId, stallId, _
            '    '                                                    startTime, endTimeRevision, updateAccount, nowDateTime)
            '    '' 衝突チップ移動処理の判定
            '    'If resultMoveChip <> ReturnOk Then
            '    '    StartWork = resultMoveChip
            '    '    Exit Try
            '    'End If
            '    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

            'End If

            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ''サービス入庫テーブルのロック処理
            'If SmbCommonClass.LockServiceInTable(drReserveInfo.PREZID, _
            '                                     drReserveInfo.UPDATE_COUNT, _
            '                                     CANCEL_FLG, _
            '                                     updateAccount, _
            '                                     nowDateTime, _
            '                                     APPLICATION_ID) <> ReturnOk Then
            '    Exit Try
            'End If
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '' 使用開始日時の設定
            'reserveInfo.Rows.Item(0).Item("STARTTIME") = startTimeRevision

            '' 日跨ぎの場合予約情報のendTimeは変更しない
            '' 作業開始後に日跨ぎであるか否か
            'Dim isHimatagi As Boolean = IsStartAfterIsHimatagi(startTime, endTimeRevision, _
            '                                    startOperationTime, endOperationTime)
            'If isHimatagi = False Then
            '    ' 日跨ぎでない場合は使用終了日時を設定
            '    reserveInfo.Rows.Item(0).Item("ENDTIME") = endTimeRevision
            'End If

            '' ストール予約の更新情報を設定する(必要ない気もするが既存処理で行っているので一応)
            'reserveInfo.Rows.Item(0).Item("DLRCD") = drProc.DLRCD ' 販売店コード
            'reserveInfo.Rows.Item(0).Item("STRCD") = drProc.STRCD ' 店舗コード
            'reserveInfo.Rows.Item(0).Item("STALLID") = stallId    ' ストールID


            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ''自動採番の作業IDを取得
            'Dim jobId As SC3150101DataSet.SC3150101JobIDDataTable = adapter.GetSequenceJobId()

            ''作業IDをストール実績に格納
            'procInfo.Rows.Item(0).Item("JOB_ID") = jobId.Rows.Item(0).Item("JOB_ID")

            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '' ---------------------------------------------------------------------------------
            '' 子予約連番の再割振
            '' ReorderRezChildNo()
            '' ---------------------------------------------------------------------------------
            'Dim childNo As Integer = 0
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ''If (Not drReserveInfo.IsREZCHILDNONull) Then
            ''    childNo = CType(drReserveInfo.REZCHILDNO, Integer)
            ''End If
            ' '' 2012/06/01 KN 西田 STEP1 重要課題対応 START
            ' '' 子予約連番の再割振
            ''If Not drReserveInfo.IsPREZIDNull AndAlso drReserveInfo.PREZID > 0 Then      ' 管理予約IDがNULLの場合、単独チップのため処理を行わない
            ''    childNo = Me.ReorderReserveChildNo(dealerCode, branchCode, reserveId, drReserveInfo.PREZID) '既存流用不要箇所
            ''End If
            ''If childNo < 0 Then
            ''    ' 子予約連番の更新に失敗
            ''    OutputLog(LOG_TYPE_ERROR, "StartWork", "Failed to update the 'REZCHILDNO'", Nothing)
            ''    Exit Try
            ''End If
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            ''2012/05/28 KN 西田 【SERVICE_1】号口不具合対応 秒の切り捨て処理追加(チップ衝突判定回避) START
            '' ---------------------------------------------------------------------------------
            '' ストール予約情報を更新する
            '' ---------------------------------------------------------------------------------
            'If (UpdateStallReserveData(adapter, _
            '                        reserveInfo, _
            '                        actualStratTime, _
            '                        updateAccount, _
            '                        childNo, _
            '                        dealerCode, _
            '                        branchCode, _
            '                        reserveId,
            '                        nowDateTime) <> ReturnOk) Then
            '    Exit Try
            'End If
            ''If (UpdateStallReserveData(adapter, _
            ''                      reserveInfo, _
            ''                      startTime, _
            ''                      updateAccount, _
            ''                      childNo, _
            ''                      dealerCode, _
            ''                      branchCode, _
            ''                      reserveId) <> ReturnOk) Then
            ''    Exit Try
            ''End If
            ''2012/05/28 KN 西田 【SERVICE_1】号口不具合対応 秒の切り捨て処理追加(チップ衝突判定回避) END

            '' ---------------------------------------------------------------------------------
            '' ストール実績情報の登録or更新
            '' ---------------------------------------------------------------------------------
            'If (UpdateStallProcessData(adapter, _
            '                           procInfo, _
            '                           reserveInfo, _
            '                           startTime, _
            '                           endTime, _
            '                           drProc.SEQNO, _
            '                           updateAccount, _
            '                           nowDateTime) <> ReturnOk) Then
            '    Exit Try
            'End If
            '' ---------------------------------------------------------------------------------
            '' 作業担当者実績の登録
            '' ---------------------------------------------------------------------------------
            'If (InsertStaffStallData(adapter, _
            '                         procInfo, _
            '                         stallId, _
            '                         updateAccount, _
            '                         nowDateTime) <> ReturnOk) Then
            '    Exit Try
            'End If
            '' ---------------------------------------------------------------------------------
            '' TACTの情報を更新する
            '' ---------------------------------------------------------------------------------
            ''追加作業のみ更新を行う。(最後にTACT側の更新を行うことにより、IF内にコミットしても大丈夫)
            'If Not workSeq = 0 Then
            '    '追加作業更新処理
            '    If Me.UpdateAddRepairStatus(drReserveInfo.DLRCD _
            '                                , orderNo _
            '                                , workSeq) <> 0 Then
            '        StartWork = 909
            '        Exit Try
            '    End If
            'End If


            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了
            'StartWork = ReturnOk

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Finally
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            'If IsCollisionFlg = True Then
            '    'ストールロックテーブル削除処理
            '    SmbCommonClass.DeleteStallLock(stallId, _
            '                                   todayDate, _
            '                                   updateAccount, _
            '                                   nowDateTime, _
            '                                   APPLICATION_ID)
            'End If
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ' 正常終了以外はロールバック
            'If StartWork <> ReturnOk Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If StartWork <> ReturnOk AndAlso StartWork <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            '' リソースを解放
            'If SmbCommonClass IsNot Nothing Then
            '    SmbCommonClass.Dispose()
            '    SmbCommonClass = Nothing
            'End If

            If tabletSmbCommonClass IsNot Nothing Then
                tabletSmbCommonClass.Dispose()
                tabletSmbCommonClass = Nothing
            End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            OutputLog(LOG_TYPE_INFO, "[E]StartWork", "", Nothing, _
                      "RET:" & StartWork.ToString(CultureInfo.CurrentCulture))
        End Try

        Return (StartWork)

    End Function

    '2012/11/05 TMEJ彭健  問連修正（GTMC121029047）END

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    ''' <summary>
    ''' 開始処理事前チェック
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inStartTime">作業開始日時(秒切捨て)</param>
    ''' <param name="inStartOperationTime">稼動開始時刻</param>
    ''' <param name="inEndOperationTime">稼動終了時刻</param>
    ''' <param name="drReserveInfo">予約情報</param>
    ''' <param name="da">SC3150101Dac</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function CheckStartWork(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inStallId As Decimal, _
                                    ByVal inStartTime As Date, _
                                    ByVal inStartOperationTime As TimeSpan, _
                                    ByVal inEndOperationTime As TimeSpan, _
                                    ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow, _
                                    ByVal da As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START,inDealerCode={2},inBranchCode={3},inStallId={4},inStartTime={5},inStartOperationTime={6},inEndOperationTime={7}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode, _
                                  inStallId.ToString(CultureInfo.CurrentCulture), _
                                  inStartTime.ToString(CultureInfo.CurrentCulture), _
                                  inStartOperationTime, _
                                  inEndOperationTime))

        ' *************************************************************************************
        ' 【二重開始チェックチェック。】
        ' *************************************************************************************
        Dim resultMultiStarts As Integer = Me.CheckMultiStarts(inDealerCode, _
                                                               inBranchCode, _
                                                               inStallId, _
                                                               inStartTime, _
                                                               inStartOperationTime, _
                                                               inEndOperationTime)
        If resultMultiStarts <> 0 Then
            ' すでに作業開始されている
            Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "Cannot start. There already is another working chip. (stallId:" & inStallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            Return 916     '「開始できませんでした。すでに作業中のチップがあります。」
        End If

        ' *************************************************************************************
        ' 【ステータスチェック（本予約かどうか）。】
        ' *************************************************************************************
        If SMB_STATUS_PROPOSED_RESOURCE = drReserveInfo.STATUS Then
            ' ストール仮予約のチップ
            Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "The selected chip is not a permanent reservation.", Nothing)
            Return 902     '「選択チップは本予約ではありません。」
        End If

        ' *************************************************************************************
        ' 【入庫済みチェック。】
        ' *************************************************************************************
        If (IsDBNull(drReserveInfo.Item("STRDATE"))) _
            OrElse (drReserveInfo.STRDATE = DateTime.MinValue) Then
            ' 未入庫のチップ
            Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "The selected chip is not service-in.", Nothing)
            Return 901     '「選択チップは入庫済みではありません。」
        End If

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
        ' *************************************************************************************
        ' 【全リレーションChip内の開始中チェック。】
        ' *************************************************************************************
        If Not drReserveInfo.IsPREZIDNull AndAlso drReserveInfo.PREZID > 0 Then      ' 管理予約IDがNULLの場合、単独チップのため処理を行わない
            If Me.GetRelatedWorkingChipCount(da, inDealerCode, inBranchCode, drReserveInfo.PREZID) > 0 Then
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", "Cannot start. There already is another working Chip. (prezid:" & drReserveInfo.PREZID.ToString(CultureInfo.CurrentCulture) & ")", Nothing)
                Return 943     '「開始できませんでした。すでに作業中のチップがあります。」
            End If
        End If
        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

        ' *************************************************************************************
        ' 【着工指示完了チェック。】
        ' *************************************************************************************
        If Not INSTRUCT_READY.Equals(drReserveInfo.INSTRUCT) Then
            ' 未着工のチップ
            Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "Cannot start. The selected chip is not ready to start.", Nothing)
            Return 920     '「開始できませんでした。選択チップは着工開始準備が完了していません。」
        End If

        ' *************************************************************************************
        ' 【作業担当者の存在チェック。】
        ' *************************************************************************************
        If Me.IsStallStaffCount(da, inStallId) <> ReturnOk Then
            Me.OutputLog(LOG_TYPE_INFO, "CheckStartWork", "Cannot start. There is no person in charge. (stallId:" & inStallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            Return 917     '「開始できませんでした。作業担当者が存在しません。」
        End If

        Return ReturnOk
    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 親R/Oの着工済みのChipの数を取得
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="pRezId">管理予約ID</param>
    ' ''' <returns>親R/Oの着工済みのChipの数</returns>
    ' ''' <remarks>追加作業（WorkSeq>0）のChipを作業開始する前に、この関数を使って親R/Oの着工状況を確認する</remarks>
    'Private Function GetStartedChipCountOfInitialRO(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                                ByVal dealerCode As String,
    '                                                ByVal branchCode As String,
    '                                                ByVal pRezId As Decimal) As Long

    '    Me.OutputLog(LOG_TYPE_INFO, "[S]GetStartedChipCountOfInitialRO", "", Nothing, _
    '                                "dealerCode:" & dealerCode, _
    '                                "branchCode:" & branchCode, _
    '                                "pRezId" & pRezId)

    '    Dim cnt As Long = adapter.GetStartedChipCountOfInitialRO(dealerCode, branchCode, pRezId)
    '    OutputLog(LOG_TYPE_INFO, "[E]GetStartedChipCountOfInitialRO", "", Nothing, "cnt=" & cnt.ToString(CultureInfo.CurrentCulture))
    '    Return cnt

    'End Function

    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 同一作業連番内の作業中チップの数を取得
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="pRezId">予約ID</param>
    ''' <returns>同一作業連番内の作業中チップの数</returns>
    ''' <remarks></remarks>
    Private Function GetRelatedWorkingChipCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                                ByVal dealerCode As String, _
                                                ByVal branchCode As String, _
                                                ByVal pRezId As Decimal) As Long

        Me.OutputLog(LOG_TYPE_INFO, "[S]GetRelatedWorkingChipCount", "", Nothing, _
                                    "dealerCode:" & dealerCode, _
                                    "branchCode:" & branchCode, _
                                    "pRezId" & pRezId)

        ' 管理予約IDがNULLの場合、単独チップのため処理を行わない
        If pRezId <= 0 Then
            Return 1
        End If

        Dim cnt As Long = adapter.GetWorkingChipCount(dealerCode, branchCode, pRezId)
        OutputLog(LOG_TYPE_INFO, "[E]GetRelatedWorkingChipCount", "", Nothing, "cnt=" & cnt.ToString(CultureInfo.CurrentCulture))
        Return cnt

    End Function

    ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START

    ''' <summary>
    ''' ストールの作業担当者数をチェック
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function IsStallStaffCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                       ByVal stallId As Decimal) As Integer

        'Private Function IsStallStaffCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
        '                              ByVal dealerCode As String, _
        '                              ByVal branchCode As String, _
        '                              ByVal startTime As Date, _
        '                              ByVal stallId As Integer) As Integer

        'Private Function IsStallStaffCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
        '                                   ByVal startTime As Date, _
        '                                   ByVal stallId As Integer) As Integer
        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
        OutputLog(LOG_TYPE_INFO, "[S]IsStallStaffCount", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture))
        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

        ' 戻り値にエラーを設定
        IsStallStaffCount = ReturnNG
        Try
            ' ストールの作業担当者数の取得
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
            'Dim staffInfo As SC3150101DataSet.SC3150101StallStaffCountDataTable = adapter.GetStaffCount(startTime, stallId)

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'Dim staffInfo As SC3150101DataSet.SC3150101StallStaffCountDataTable = _
            '                    adapter.GetStaffCount(stallId)

            Dim userContext As StaffContext = StaffContext.Current

            Dim stfStallDispType As String = String.Empty
            Using tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
                stfStallDispType = tabletSmbCommonClass.GetStaffStallDispType(userContext.DlrCD, userContext.BrnCD)
            End Using

            Dim staffInfo As SC3150101DataSet.SC3150101StallStaffCountDataTable = _
                                adapter.GetStaffCount(stallId, userContext.DlrCD, userContext.BrnCD, stfStallDispType)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
            If staffInfo Is Nothing OrElse staffInfo.Count = 0 Then
                ' 作業担当者情報の取得に失敗
                OutputLog(LOG_TYPE_INFO, "IsStallStaffCount", "Failed to get the stall staff information", Nothing)
                Exit Try
            Else
                Dim drStaffInfo As SC3150101DataSet.SC3150101StallStaffCountRow = _
                                    DirectCast(staffInfo.Rows(0), SC3150101DataSet.SC3150101StallStaffCountRow)
                ' 作業担当者数の確認
                If drStaffInfo.COUNT <= 0 Then
                    ' 作業担当者がいない
                    OutputLog(LOG_TYPE_INFO, "IsStallStaffCount", "There is no person in charge.", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            IsStallStaffCount = ReturnOk

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]IsStallStaffCount", "", Nothing, _
                      "RET:" & IsStallStaffCount.ToString(CultureInfo.CurrentCulture))
        End Try

        Return IsStallStaffCount

    End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' R/O基本情報の取得処理
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">オーダーNo.</param>
    ' ''' <returns>R/O基本情報データテーブル</returns>
    ' ''' <remarks></remarks>
    'Private Function GetRepairOrderBaseData(ByVal dlrCD As String, ByVal orderNo As String) As IC3801001DataSet.IC3801001OrderCommDataTable

    '    Me.OutputLog(LOG_TYPE_INFO, "[S]GetRepairOrderBaseData", "", Nothing, _
    '              "dlrCD:" & dlrCD, _
    '              "orderNo:" & orderNo)

    '    Dim IC3801001 As IC3801001BusinessLogic = New IC3801001BusinessLogic

    '    Me.OutputLog(LOG_TYPE_INFO, "CALL IC3801001BusinessLogic.GetROBaseInfoList", "CALL", Nothing _
    '                                     , dlrCD, orderNo)

    '    Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable = IC3801001.GetROBaseInfoList(dlrCD, orderNo)

    '    OutPutIFLog(dt, "IC3801001.GetROBaseInfoList")

    '    Me.OutputLog(LOG_TYPE_INFO, "[E]GetRepairOrderBaseData", "", Nothing, _
    '              "RETURN_COUNT:" & dt.Rows.Count.ToString(CultureInfo.CurrentCulture))

    '    Return dt
    'End Function

    ' ''' <summary>
    ' ''' 追加作業ステータス情報取得処理
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">オーダーNo.</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetAddRepairStatusList(ByVal dlrCD As String, ByVal orderNo As String) As IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable

    '    Me.OutputLog(LOG_TYPE_INFO, "[S]GetAddRepairStatusList", "", Nothing, _
    '              "dlrCD:" & dlrCD, _
    '              "orderNo:" & orderNo)

    '    Dim IC3800804 As New IC3800804BusinessLogic

    '    Me.OutputLog(LOG_TYPE_INFO, "CALL IC3800804BusinessLogic.GetAddRepairStatusList", "CALL", Nothing _
    '                                     , dlrCD, orderNo)

    '    Dim dt As DataTable = IC3800804.GetAddRepairStatusList(dlrCD, orderNo)

    '    OutPutIFLog(dt, "IC3800804.GetAddRepairStatusList")

    '    Me.OutputLog(LOG_TYPE_INFO, "[E]GetAddRepairStatusList", "", Nothing, _
    '              "RETURN_COUNT:" & dt.Rows.Count.ToString(CultureInfo.CurrentCulture))

    '    Return DirectCast(dt, IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable)
    'End Function


    ' ''' <summary>
    ' ''' 追加作業の更新
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">予約ID</param>
    ' ''' <param name="workSeq">作業連番</param>
    ' ''' <returns>処理結果 成功：0 / 失敗：1</returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateAddRepairStatus(ByVal dlrCD As String, ByVal orderNo As String, ByVal workSeq As Integer) As Integer

    '    Me.OutputLog(LOG_TYPE_INFO, "[S]UpdateAddRepairStatus", "", Nothing, _
    '              "dlrCD:" & dlrCD, _
    '              "orderNo:" & orderNo, _
    '              "workSeq:" & workSeq.ToString(CultureInfo.CurrentCulture))

    '    Dim IC3800805 As New IC3800805BusinessLogic
    '    Dim srvAddSeq As Integer = workSeq

    '    Me.OutputLog(LOG_TYPE_INFO, "CALL IC3800805BusinessLogic.UpdateAddRepairStatus", "CALL", Nothing _
    '                                     , dlrCD, orderNo, srvAddSeq.ToString(CultureInfo.CurrentCulture))
    '    '追加作業更新処理
    '    Dim rtnVal As Integer = IC3800805.UpdateAddRepairStatus(dlrCD, orderNo, srvAddSeq)

    '    Me.OutputLog(LOG_TYPE_INFO, "[E]UpdateAddRepairStatus", "", Nothing, _
    '              "RET:" & rtnVal.ToString(CultureInfo.CurrentCulture))

    '    Return rtnVal
    'End Function

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START

    ' ''' <summary>
    ' ''' 衝突チップを移動する
    ' ''' </summary>
    ' ''' <param name="reserveList">ストール予約情報</param>
    ' ''' <param name="stallInfo">ストール時間情報</param>
    ' ''' <param name="breakInfo">休憩情報</param>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="reserveID">予約ID</param>
    ' ''' <param name="stallId">予約ID</param>
    ' ''' <param name="startTime">開始日時</param>
    ' ''' <param name="endTimeRevision">終了日時</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    'Private Function MoveCollisionChip(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
    '                                   ByVal stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
    '                                   ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
    '                                   ByVal dealerCode As String, _
    '                                   ByVal branchCode As String, _
    '                                   ByVal reserveId As Long, _
    '                                   ByVal stallId As Integer, _
    '                                   ByVal startTime As Date, _
    '                                   ByVal endTimeRevision As Date, _
    '                                   ByVal updateAccount As String, _
    '                                   ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]MoveCollisionChip", "", Nothing, _
    '              "RESERVELIST:(DataSet)", _
    '              "STALLINFO:(DataSet)", _
    '              "BREAKINFO:(DataSet)", _
    '              "DEALERCODE:" & dealerCode, _
    '              "BRANCHCODE:" & branchCode, _
    '              "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
    '              "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
    '              "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
    '              "ENDTIMEREVISION:" & endTimeRevision.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEACCOUNT:" & updateAccount, _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    MoveCollisionChip = ReturnNG
    '    Try
    '        ' 指定時間への予約の移動
    '        Dim reserveListTemp As SC3150101DataSet.SC3150101StallReserveListDataTable = _
    '                                    MoveReserve(reserveList, stallInfo, breakInfo, _
    '                                                dealerCode, branchCode, reserveId, _
    '                                                stallId, startTime, endTimeRevision)

    '        If reserveListTemp Is Nothing Then
    '            ' 後続チップに干渉する
    '            OutputLog(LOG_TYPE_WARNING, "MoveCollisionChip", _
    '                      "The selected chip is overlapping with next arranged chip.", Nothing)
    '            MoveCollisionChip = 903
    '            Exit Try
    '        End If

    '        ' 時間に変更のあった予約情報の更新
    '        Dim result As Integer = UpdateAllReserve(reserveListTemp, _
    '                                                    reserveId, _
    '                                                    dealerCode, _
    '                                                    branchCode, _
    '                                                    stallId, _
    '                                                    updateAccount, _
    '                                                    updateDate)
    '        If result < 0 Then
    '            ' 後続チップの移動に失敗
    '            OutputLog(LOG_TYPE_ERROR, "MoveCollisionChip", "Failed to move the following chip.", Nothing)
    '            Exit Try
    '        End If

    '        ' 正常終了
    '        MoveCollisionChip = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]MoveCollisionChip", "", Nothing, _
    '                  "RET:" & MoveCollisionChip.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return MoveCollisionChip

    'End Function


    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

    ' ''' <summary>
    ' ''' ストール予約を更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="reserveInfo">ストール予約情報</param>
    ' ''' <param name="startTime">作業開始時間</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <param name="childNo">子予約連番</param>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="reserveId">予約ID</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Private Function UpdateStallReserveData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                        ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                        ByVal startTime As Date, _
    '                                        ByVal updateAccount As String, _
    '                                        ByVal childNo As Integer, _
    '                                        ByVal dealerCode As String, _
    '                                        ByVal branchCode As String, _
    '                                        ByVal reserveId As Long,
    '                                        ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStallReserveData", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "RESERVEINFO:(DataSet)", _
    '              "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEACCOUNT:" & updateAccount, _
    '              "CHILDNO:" & childNo.ToString(CultureInfo.CurrentCulture), _
    '              "DEALERCODE:" & dealerCode, _
    '              "BRANCHCODE:" & branchCode, _
    '              "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    UpdateStallReserveData = ReturnNG
    '    Try
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        'Dim resultUpdRez As Integer = adapter.UpdateStallReserveInfo(reserveInfo, _
    '        '                                                             Date.MinValue, _
    '        '                                                             Date.MaxValue, _
    '        '                                                             KeepCurrent, _
    '        '                                                             KeepCurrent, _
    '        '                                                             updateAccount)
    '        'If (resultUpdRez <= 0) Then
    '        '    ' ストール予約情報の更新に失敗
    '        '    OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to update the stall reservation information.", Nothing)
    '        '    Exit Try
    '        'End If

    '        ' ストール予約情報を更新する
    '        Dim resultUpdRez As Integer = UpdateStallReserveInfo(adapter, _
    '                                                             reserveInfo, _
    '                                                             updateAccount,
    '                                                             updateDate)
    '        If (resultUpdRez <= 0) Then
    '            ' ストール予約情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateStallReserveData", "Failed to update the stall reservation information.", Nothing)
    '            Exit Try
    '        End If

    '        ' ストール予約履歴を登録する
    '        'Dim resultInsRezHis As Integer = adapter.InsertReserveHistory(dealerCode, branchCode, reserveId, 1)
    '        'If (resultInsRezHis <= 0) Then
    '        '    ' ストール予約履歴の登録に失敗
    '        '    OutputLog(LOG_TYPE_ERROR, "UpdateStallReserveData", "Failed to register the stall reservation history.", Nothing)
    '        '    Exit Try
    '        'End If
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' 正常終了
    '        UpdateStallReserveData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStallReserveData", "", Nothing, _
    '                  "RET:" & UpdateStallReserveData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStallReserveData

    'End Function


    ' ''' <summary>
    ' ''' ストール実績の登録または更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="procInfo">ストール実績情報</param>
    ' ''' <param name="reserveInfo">ストール予約情報</param>
    ' ''' <param name="startTime">作業開始時間</param>
    ' ''' <param name="endTime">作業終了時間</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Private Function UpdateStallProcessData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                        ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
    '                                        ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                        ByVal startTime As Date, _
    '                                        ByVal endTime As Date, _
    '                                        ByVal seqNo As Decimal, _
    '                                        ByVal updateAccount As String, _
    '                                        ByRef updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStallProcessData", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "PROCINFO:(DataSet)", _
    '              "RESERVEINFO:(DataSet)", _
    '              "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
    '              "ENDTIME:" & endTime.ToString(CultureInfo.CurrentCulture), _
    '              "SEQNO:" & seqNo.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEACCOUNT:" & updateAccount, _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    UpdateStallProcessData = ReturnNG
    '    Try
    '        ' ストール実績情報の設定
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        'procInfo.Rows.Item(0).Item("RESULT_STATUS") = SMB_ResultStatusWorking ' 実績_ステータス（20:作業中）
    '        procInfo.Rows.Item(0).Item("RESULT_START_TIME") = _
    '           startTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())  ' 実績_ストール開始日時時刻
    '        procInfo.Rows.Item(0).Item("RESULT_END_TIME") = _
    '            endTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())    ' 実績_ストール終了日時時刻


    '        procInfo.Rows.Item(0).Item("STALL_USE_STATUS") = stallUseStetus02 ' ストール利用ステータス（02:作業中)
    '        procInfo.Rows.Item(0).Item("RESULT_STATUS") = ServiceStetus_Working ' 実績_ステータス（05:作業中)


    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        If seqNo = 0 Then

    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '            '' ストール実績情報を登録する
    '            'Dim resultInsProc As Integer = adapter.InsertStallProcessInfo(procInfo, updateAccount, False, False)
    '            'If (resultInsProc <= 0) Then
    '            '    ' ストール実績情報の登録に失敗
    '            '    OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessData", "Failed to register the stall process information.", Nothing)
    '            '    Exit Try
    '            'End If


    '            '自動採番のストール利用IDを取得
    '            Dim stallUseId As SC3150101DataSet.SC3150101StallUseIdDataTable = adapter.GetSequenceStallUseId()

    '            ' ストール実績情報を登録する
    '            Dim resultInsMidFinish As Integer = adapter.InsertStallUseMidFinish(procInfo, _
    '                                                                                reserveInfo, _
    '                                                                                updateAccount, _
    '                                                                                stallUseId, _
    '                                                                                updateDate)
    '            If (resultInsMidFinish <= 0) Then
    '                ' ストール実績情報の登録に失敗
    '                OutputLog(LOG_TYPE_ERROR, "InsertStallUseMidFinish", "Failed to register the stall process information.", Nothing)
    '                Exit Try
    '            End If
    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
    '        Else

    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '            '' ストール実績情報を更新する
    '            'Dim resultUpdProc As Integer = adapter.UpdateStallProcessInfo(procInfo, reserveInfo)
    '            'If (resultUpdProc <= 0) Then
    '            '    ' ストール実績情報の更新に失敗
    '            '    OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessData", "Failed to update the stall process information.", Nothing)
    '            '    Exit Try
    '            'End If

    '            ' ストール実績情報を更新する
    '            Dim resultUpdProc As Integer = UpdateStallProcessInfo(adapter, _
    '                                                                  procInfo, _
    '                                                                  reserveInfo, _
    '                                                                  updateAccount,
    '                                                                  updateDate)
    '            If (resultUpdProc <= 0) Then
    '                ' ストール実績情報の更新に失敗
    '                OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessData", "Failed to update the stall process information.", Nothing)
    '                Exit Try
    '            End If
    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        End If

    '        ' 正常終了
    '        UpdateStallProcessData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStallProcessData", "", Nothing, _
    '                  "RET:" & UpdateStallProcessData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStallProcessData

    'End Function


    ' ''' <summary>
    ' ''' ストール実績の登録または更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="procInfo">ストール実績情報</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Private Function InsertStaffStallData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                      ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
    '                                      ByVal stallId As Integer, _
    '                                      ByVal updateAccount As String, _
    '                                      ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]InsertStaffStallData", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "PROCINFO:(DataSet)", _
    '              "STALLID" & stallId.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEACCONUT:" & updateAccount, _
    '              "UPDATEDATE" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    InsertStaffStallData = ReturnNG
    '    Try
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        ' 作業日付を取得する
    '        'Dim staffWorkTime As Date = GetWorkDate(stallInfo, startTime)

    '        '' 作業担当者実績の作成
    '        'Dim resultInsStaffStall As Integer = adapter.InsertStaffStall(stallId, reserveId, staffWorkTime)
    '        'If (resultInsStaffStall <= 0) Then
    '        '    ' 担当者実績の登録に失敗
    '        '    OutputLog(LOG_TYPE_ERROR, "InsertStaffStallData", "Failed to register stall staff information.", Nothing)
    '        '    Exit Try
    '        'End If

    '        'インスタンス定義
    '        Dim stallStaffDataTable As SC3150101DataSet.SC3150101BelongStallStaffDataTable
    '        'ストールに配置されている全てのスタッフの名前取得
    '        stallStaffDataTable = GetBelongStallStaffData(stallId)

    '        '取得したスタッフの名前数ループ
    '        For Each eachStaffName As SC3150101DataSet.SC3150101BelongStallStaffRow In stallStaffDataTable

    '            'スタッフ作業IDを取得
    '            Dim stfJobId As SC3150101DataSet.SC3150101StaffJobIdDataTable = adapter.GetSequenceStaffJobId
    '            procInfo.Rows.Item(0).Item("STF_JOB_ID") = stfJobId.Rows.Item(0).Item("STF_JOB_ID")

    '            ' 作業担当者実績の作成
    '            Dim resultInsStaffStall As Integer = adapter.InsertStaffStall(procInfo, updateAccount, eachStaffName.STF_CD, updateDate)
    '            If (resultInsStaffStall <= 0) Then
    '                ' 担当者実績の登録に失敗
    '                OutputLog(LOG_TYPE_ERROR, "InsertStaffStallData", "Failed to register stall staff information.", Nothing)
    '                Exit Try
    '            End If
    '        Next

    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' 正常終了
    '        InsertStaffStallData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]InsertStaffStallData", "", Nothing, _
    '                  "RET:" & InsertStaffStallData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return InsertStaffStallData

    'End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

#End Region

#Region "当日処理"
    '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
    ''' <summary>
    '''   当日処理を行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="orderNo">R/O No.</param>
    ''' <param name="updateCount">行ロックバージョン</param>
    ''' <param name="isBreak">休憩取得有無(True：有、False：無)</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </History>
    <EnableCommit()>
    Public Function SuspendWork(ByVal dealerCode As String, _
                                ByVal branchCode As String, _
                                ByVal reserveId As Decimal, _
                                ByVal stallId As Decimal, _
                                ByVal updateAccount As String, _
                                ByVal orderNo As String, _
                                ByVal updateCount As Long, _
                                Optional ByVal isBreak As Boolean = False, _
                                Optional ByVal breakBottomFlg As Boolean = False) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]SuspendWork", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
                  "ACCOUNT:" & updateAccount, _
                  "ORDERNO:" & orderNo, _
                  "UPDATECOUNT:" & updateCount.ToString(CultureInfo.CurrentCulture))

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '' 戻り値にエラーを設定
        'SuspendWork = ReturnNG_SUSPEND
        ''SuspendWork = ReturnNG

        ' 戻り値に正常を設定
        SuspendWork = ReturnOk

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Dim adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                            New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        ' チップ衝突フラグ
        'Dim IsCollisionFlg As Boolean = False
        '現在の時刻取得
        Dim upDateTime As Date = DateTimeFunc.Now(dealerCode)
        ''SMBコモンクラスのインスタンス宣言
        'Dim SmbCommonClass As New SMBCommonClassBusinessLogic
        'タブレットSMBコモンクラスのインスタンス宣言
        Dim tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
        'ログイン中のスタッフ情報
        Dim userContext As StaffContext = StaffContext.Current
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        Try
            ' ストール予約情報を取得する
            Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = _
                                    adapter.GetStallReserveInfo(dealerCode, branchCode, reserveId)
            If reserveInfo Is Nothing OrElse reserveInfo.Count <= 0 Then
                ' ストール予約情報の取得に失敗
                OutputLog(LOG_TYPE_INFO, "SuspendWork", "Failed to get the stall reservation information.", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                SuspendWork = ReturnNG_SUSPEND

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            ' ストール実績情報を取得する
            Dim procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable = _
                                adapter.GetStallProcessInfo(dealerCode, branchCode, reserveId)
            If procInfo Is Nothing OrElse procInfo.Count <= 0 Then
                ' ストール実績情報の取得に失敗
                OutputLog(LOG_TYPE_INFO, "SuspendWork", "Failed to get the stall process information.", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                SuspendWork = ReturnNG_SUSPEND

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            Dim drProc As SC3150101DataSet.SC3150101StallProcessInfoRow = _
                                DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ' 排他処理チェック
            If drProc.UPDATE_COUNT <> updateCount Then
                Me.OutputLog(LOG_TYPE_INFO, "StartWork", "This chip has been modified by another operator. Please reload and try again.", Nothing)
                SuspendWork = 923     '「既に他のオペレータによって更新されています。画面をリロードしてからもう一度お試し下さい。」
                Exit Try
            End If
            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ' 作業中のチップであるかチェック
            'If (Not IsDBNull(drProc.Item("RESULT_STATUS")) And drProc.Item("RESULT_STATUS").Equals("0")) _
            '    OrElse (String.Equals(drProc.RESULT_STATUS, SMB_ResultStatusWorking) = False) _
            '    OrElse (IsDBNull(drProc.Item("RESULT_STATUS"))) Then

            If (Not IsDBNull(drProc.Item("RESULT_STATUS")) And drProc.Item("RESULT_STATUS").Equals("0")) _
           OrElse (String.Equals(drProc.RESULT_STATUS, ServiceStetus_Working) = False) _
           OrElse (IsDBNull(drProc.Item("RESULT_STATUS"))) Then
                ' まだ作業開始されていない
                OutputLog(LOG_TYPE_INFO, "SuspendWork", "Cannot handle in the day. The selected Chip is not in the working status.", Nothing)
                SuspendWork = 918
                Exit Try
            End If
            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
            '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                                    DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            '2012/06/08 KN 西田 STEP1 重要課題対応 START
            '完成検査承認依頼中で無いことを確認
            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            'Dim workSeq As Integer = 0
            'If (Not drReserveInfo.IsWORKSEQNull) Then
            '    workSeq = CType(drReserveInfo.WORKSEQ, Integer)
            'End If

            'Dim inspectionStatus As Integer = 0
            'If (Not drProc.IsINSPECTION_STATUSNull) Then
            '    inspectionStatus = CType(drProc.INSPECTION_STATUS, Integer)
            'End If

            'If Me.IsCheckInspectionApproval(dealerCode, orderNo, inspectionStatus) <> 0 Then
            '    ' 既に完成検査承認依頼中である。
            '    OutputLog(LOG_TYPE_WARNING, "SuspendWork", "Cannot handle in the day. The selected Chip has already requested Final Inspection.", Nothing)
            '    SuspendWork = 919
            '    Exit Try
            'End If
            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END
            '2012/06/08 KN 西田 STEP1 重要課題対応 END
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            ' (実際の)作業開始日時を取得する
            ' 当日の作業開始日時
            Dim resultStartTime As Date = _
                Date.ParseExact(drProc.RESULT_START_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            ' 実績の作業予定終了時間
            Dim procEndTime As Date = _
                Date.ParseExact(drProc.RESULT_END_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)

            ' (実際の)作業終了日時を取得する
            Dim resultEndTime As Date = DateTimeFunc.Now(dealerCode)

            ' ストール時間を取得する
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = _
                                adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)
            If stallInfo Is Nothing OrElse stallInfo.Count <= 0 Then
                ' ストール時間の取得に失敗
                OutputLog(LOG_TYPE_INFO, "SuspendWork", "Failed to get the stall time information.", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                SuspendWork = ReturnNG_SUSPEND

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If
            ' 翌日の時間情報を取得
            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = _
                                DirectCast(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
            ' 予定作業終了時間
            Dim rezEndTime As Date = Date.ParseExact(drProc.RESULT_END_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            ' ストール開始時間
            Dim stallStartTime As TimeSpan = SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            ' ストール終了時間
            Dim stallEndTime As TimeSpan = SetStallTime(drStallInfo.PENDTIME).TimeOfDay

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            '' 翌日の作業開始予定日時:rezSTime
            'Dim nextDayStartTime As Date = GetNextDayStartTime(rezEndTime, stallStartTime)
            '' 翌日の予定作業時間(分):rezWTime
            'Dim nextDayWorkTime As Integer = GetNextDayWorkTime(rezEndTime, stallStartTime)


            '翌日の日付
            Dim tomorrowday As Date = upDateTime.AddDays(1).Date

            '翌日日付の作業終了予定日時
            Dim nextWoringDay As Date = tomorrowday.AddHours(rezEndTime.Hour).AddMinutes(rezEndTime.Minute)

            ' 翌日の作業開始予定日時:rezSTime
            Dim nextDayStartTime As Date = GetNextDayStartTime(nextWoringDay, stallStartTime)

            ' 翌日の予定作業時間(分):rezWTime
            'Dim nextDayWorkTime As Integer = GetNextDayWorkTime(rezEndTime, _
            '                                                    nextDayStartTime, _
            '                                                    stallStartTime, _
            '                                                    SetStallTime(drStallInfo.PSTARTTIME), _
            '                                                    SetStallTime(drStallInfo.PENDTIME))

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            ' 作業時刻終了判定
            resultEndTime = CheckEndTime(dealerCode, branchCode, stallId, resultStartTime, _
                                         resultEndTime, procEndTime)


            ' ---------------------------------------------------------------------------------
            ' 指定範囲内のストール予約情報を取得(当日分)
            ' ---------------------------------------------------------------------------------
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDate As Date = resultStartTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDate As Date = GetEndDateRange(fromDate, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                adapter.GetStallReserveList(dealerCode, branchCode, stallId, _
                                                            reserveId, fromDate, toDate)
            ' 指定範囲内のストール実績情報を取得
            Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                adapter.GetStallProcessList(dealerCode, branchCode, stallId, _
                                                            fromDate, toDate)
            ' 指定範囲内の予約情報の取得
            reserveList = GetReserveList(reserveList, processList, stallId, reserveId, _
                                         fromDate, isBreak)
            ' 休憩時間帯・使用不可時間帯取得
            'Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
            '                    adapter.GetBreakSlot(stallId, fromDate, toDate)

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内のストール予約情報を取得(翌日分)
            ' ---------------------------------------------------------------------------------
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDateOfNextDay As Date = nextDayStartTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDateOfNextDay As Date = GetEndDateRange(fromDateOfNextDay, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim nextDayReserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                            adapter.GetStallReserveList(dealerCode, branchCode, stallId, _
                                                                        reserveId, fromDateOfNextDay, _
                                                                        toDateOfNextDay)
            ' 指定範囲内のストール実績情報を取得
            Dim nextDayProcessList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                            adapter.GetStallProcessList(dealerCode, branchCode, stallId, _
                                                                        fromDateOfNextDay, toDateOfNextDay)
            ' 指定範囲内の予約情報の取得
            nextDayReserveList = GetReserveList(nextDayReserveList, nextDayProcessList, _
                                                stallId, reserveId, fromDateOfNextDay, isBreak)

            '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
            ' 休憩時間帯・使用不可時間帯取得
            'Dim nextDayBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
            '                                adapter.GetBreakSlot(stallId, fromDateOfNextDay, toDateOfNextDay)

            '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END
            '---------------------------------------------------------------------------------

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            ' 販売店環境設定値取得(調整時間(時)取得)
            'Dim envSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable = _
            '                                adapter.GetDealerEnvironmentSettingValue(dealerCode, branchCode, _
            '                                                                            C_SMB_DISPDATE_ADJUST)
            'If envSettingInfo Is Nothing Then
            '    ' 販売店環境設定値の取得に失敗
            '    OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to get the dealer setting.", Nothing)
            '    Exit Try
            'End If
            '' 稼動時間外MidFinish基準時間算出
            '' 稼動時間外MidFinish基準日時
            ''Dim standardTime As Date = CalculateMidFinishStandardTime(envSettingInfo, resultStartTime, _
            ''                                                            stallStartTime, stallEndTime)

            '' MidFinishのresultEndTimeが基準時間後の場合、resultEndTimeをストール稼動終了時間とする
            '' 作業終了時間をストール稼動終了時間とするか否かを判定する
            'If IsSetWorkEndTimeToStallEndTime(standardTime) Then
            '    ' ここに分岐する場合、ストール稼動終了時間は翌日0:00以降
            '    resultEndTime = resultStartTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes)
            'End If

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            ' タグチェックは行わない

            '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
            ' 休憩取得有無チェック
            'Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
            '                        DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
            '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END
            '休憩取得あり (MidFinish当日分)
            'Dim resultBreak As Boolean = CheckBreak(breakInfo, isBreak, resultStartTime, procEndTime, _
            '                                        CType(drReserveInfo.REZ_WORK_TIME, Integer))
            '休憩取得あり (MidFinish翌日分)
            'Dim nextDayResultBreak As Boolean = isBreak


            ' 当日の作業時間を算出
            'Dim resultWorkTime As Integer = CalculateWorkTime(breakInfo, resultStartTime, _
            '                                                    resultEndTime, resultBreak)

            ' 予約の作業終了予定日時を算出
            'Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date
            'dateTemp = CalculateEndTime(stallInfo, _
            '                            dealerCode, _
            '                            branchCode, _
            '                            stallId, _
            '                            nextDayStartTime, _
            '                            nextDayWorkTime, _
            '                            nextDayResultBreak)
            'Dim reserveEndTime As Date = dateTemp(END_TIME_END)

            ' 予約の作業終了予定日時の見直し
            'Dim reserveEndTimeRevision As Date ' (TIMEINTERVAL補正)予約の作業終了予定日時 (MidFinish翌日分)

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            'Dim timeDiff As Integer = CType(reserveEndTime.Minute Mod drStallInfo.TIMEINTERVAL, Integer)
            'If timeDiff > 0 Then
            '    reserveEndTimeRevision = reserveEndTime.AddMinutes(drStallInfo.TIMEINTERVAL - timeDiff)
            'Else
            'reserveEndTimeRevision = reserveEndTime
            'End If

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            ' 休憩取得ボタン押下有無判定
            'drProc.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProc.REST_FLG)

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定する場合
                If biz.IsRestAutoJudge() Then
                    drProc.REST_FLG = REST_FLG_TAkE
                Else
            ' 休憩取得ボタン押下有無判定
            drProc.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProc.REST_FLG)
                End If
            End Using

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

            '秒数を切り捨てる
            Dim rsltEndDateTime As Date = upDateTime
            rsltEndDateTime = rsltEndDateTime.AddSeconds(-rsltEndDateTime.Second)

            resultStartTime = resultStartTime.AddSeconds(-resultStartTime.Second)

            'チップの日跨ぎ終了処理
            Dim returnValue As Long = tabletSmbCommonClass.MidFinish(drReserveInfo.PREZID _
                                                                     , drProc.REZID _
                                                                     , stallId _
                                                                     , resultStartTime _
                                                                     , rsltEndDateTime _
                                                                     , drProc.REST_FLG _
                                                                     , userContext _
                                                                     , Date.ParseExact(drStallInfo.PSTARTTIME, "HH:mm", CultureInfo.CurrentCulture()) _
                                                                     , Date.ParseExact(drStallInfo.PENDTIME, "HH:mm", CultureInfo.CurrentCulture()) _
                                                                     , upDateTime _
                                                                     , APPLICATION_ID _
                                                                     , updateCount)


            '日跨ぎ終了処理が失敗した場合
            If returnValue <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                SuspendWork = OtherSystemsReturnCodeSelect(returnValue, workMidFinishFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                
                ''日跨ぎ終了処理に失敗
                'OutputLog(LOG_TYPE_ERROR, "tabletSmbCommonClass.MidFinish", "Failed to mid finish of processing chip.", Nothing)
                'Exit Try

                'エラー内容チェック
                If returnValue <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    Me.OutputLog(LOG_TYPE_WARNING, "tabletSmbCommonClass.MidFinish", "Failed to mid finish of processing chip.", Nothing)
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If


            '' 翌日チップの衝突判定
            'If IsCollision(nextDayReserveList, reserveId, nextDayStartTime, reserveEndTimeRevision) Then

            '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            '    'ストールロックテーブル登録処理
            '    If SmbCommonClass.RegisterStallLock(stallId, _
            '                                        upDateTime, _
            '                                        updateAccount, _
            '                                        upDateTime, _
            '                                        APPLICATION_ID) <> ReturnOk Then
            '        Exit Try
            '    End If
            '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
            '    '' 衝突チップを移動する
            '    'Dim resultMoveChip As Integer = MoveCollisionChip(nextDayReserveList, stallInfo, nextDayBreakInfo, _
            '    '                                                  dealerCode, branchCode, reserveId, stallId, _
            '    '                                                  nextDayStartTime, reserveEndTimeRevision, updateAccount, upDateTime)
            '    '' 衝突チップ移動処理の判定
            '    'If resultMoveChip <> ReturnOk Then
            '    '    SuspendWork = resultMoveChip
            '    '    Exit Try
            '    'End If
            '    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

            '    IsCollisionFlg = True
            'End If

            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ''サービス入庫テーブルのロック処理
            'If SmbCommonClass.LockServiceInTable(drReserveInfo.PREZID, _
            '                                     drReserveInfo.UPDATE_COUNT, _
            '                                     CANCEL_FLG, _
            '                                     updateAccount, _
            '                                     upDateTime, _
            '                                     APPLICATION_ID) <> ReturnOk Then
            '    Exit Try
            'End If
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            '' ストール予約情報を更新およびストール予約履歴情報を登録
            ''If UpdateStallReserveInfoData(adapter, _
            ''                              reserveInfo, _
            ''                             updateAccount, _
            ''                             upDateTime) <> ReturnOk Then
            ''    Exit Try
            ''End If

            '' 当日分のストール実績情報を更新
            'If UpdateStallProcessInfoData(adapter, _
            '                              procInfo, _
            '                              drReserveInfo.PREZID, _
            '                              resultStartTime, _
            '                              resultEndTime, _
            '                              resultWorkTime, _
            '                              updateAccount, _
            '                              upDateTime) <> ReturnOk Then
            '    Exit Try
            'End If

            '' 翌日のストール実績情報を更新
            'If InsertStallProcessInfoData(adapter, _
            '                              procInfo, _
            '                              reserveInfo, _
            '                              nextDayStartTime, _
            '                              reserveEndTimeRevision, _
            '                              nextDayWorkTime, _
            '                              updateAccount, _
            '                              upDateTime) <> ReturnOk Then
            '    Exit Try
            'End If

            '' 作業日付を取得
            'Dim workTime As Date = GetWorkDate(stallInfo, resultStartTime)

            '' 担当者実績情報を更新
            'If UpdateStaffStallData(adapter, _
            '                        dealerCode, _
            '                        branchCode, _
            '                        stallId, _
            '                        reserveId, _
            '                        workTime, _
            '                        updateAccount, _
            '                        upDateTime) <> ReturnOk Then
            '    Exit Try
            'End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了
            'SuspendWork = ReturnOk

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Finally
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            ''チップが衝突していた場合
            'If IsCollisionFlg = True Then
            '    'ストールロックテーブル削除処理
            '    SmbCommonClass.DeleteStallLock(stallId, _
            '                                   upDateTime, _
            '                                   updateAccount, _
            '                                   upDateTime, _
            '                                   APPLICATION_ID)

            'End If
            ''2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了以外はロールバック
            'If FinishWork <> ReturnOk Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If SuspendWork <> ReturnOk AndAlso SuspendWork <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START 
            ' リソースを解放
            'If SmbCommonClass IsNot Nothing Then
            '    SmbCommonClass.Dispose()
            '    SmbCommonClass = Nothing
            'End If

            If tabletSmbCommonClass IsNot Nothing Then
                tabletSmbCommonClass.Dispose()
                tabletSmbCommonClass = Nothing
            End If
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            OutputLog(LOG_TYPE_INFO, "[E]SuspendWork", "", Nothing, _
                      "RET:" & SuspendWork.ToString(CultureInfo.CurrentCulture))
        End Try

        Return (SuspendWork)

    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 完成検査承認チェック
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">オーダーNo.</param>
    ' ''' <param name="workSeq">作業連番</param>
    ' ''' <returns>完成検査承認待ち前の場合は0(チェックOK)、左記以外はNG</returns>
    ' ''' <remarks></remarks>
    'Private Function IsCheckInspectionApproval(ByVal dlrCD As String, ByVal orderNo As String, ByVal workSeq As Integer) As Integer

    '    Me.OutputLog(LOG_TYPE_INFO, "[S]IsCheckInspectionApproval", "", Nothing, _
    '              "dlrCD:" & dlrCD, _
    '              "orderNo:" & orderNo, _
    '              "workSeq:" & workSeq)

    '    Dim rtnVal As Integer = -1
    '    Dim inspectionApproval As String = String.Empty


    '    If WORKSEQ_NOPLAN_PARENT.Equals(workSeq.ToString(CultureInfo.InvariantCulture)) Then
    '        '親作業の場合
    '        Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable = Me.GetRepairOrderBaseData(dlrCD, orderNo)

    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
    '            Dim dr As IC3801001DataSet.IC3801001OrderCommRow = _
    '                DirectCast(dt(0), IC3801001DataSet.IC3801001OrderCommRow)

    '            If Not dr.IsINSPECTIONAPPROVALFLAGNull Then
    '                inspectionApproval = dr.INSPECTIONAPPROVALFLAG.Trim()
    '            End If

    '        End If
    '    Else
    '        '追加作業の場合
    '        inspectionApproval = Me.GetChildChipInspectionApprovalFlg(dlrCD, orderNo, workSeq)
    '    End If

    '    '完成検査承認前か判定
    '    If INSPECTION_APPROVAL_BEFORE.Equals(inspectionApproval) Then
    '        rtnVal = 0
    '    End If

    '    Me.OutputLog(LOG_TYPE_INFO, "[E]IsCheckInspectionApproval", "", Nothing, _
    '              "RET:" & rtnVal.ToString(CultureInfo.CurrentCulture))

    '    Return rtnVal
    'End Function


    ' ''' <summary>
    ' ''' 完成検査承認情報取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">オーダーNO</param>
    ' ''' <param name="workSeq">作業連番</param>
    ' ''' <returns>完成検査承認フラグ</returns>
    ' ''' <remarks></remarks>
    'Public Function GetChildChipInspectionApprovalFlg(ByVal dlrCD As String, ByVal orderNo As String, ByVal workSeq As Integer) As String

    '    Logger.Info("GetChildChipInspectionApprovalFlg Start param1:" + dlrCD + _
    '                                                        " param2:" + orderNo + _
    '                                                        " param3:" + CType(workSeq, String))

    '    Dim rtnValue As String = String.Empty
    '    Dim IC3800804 As New IC3800804BusinessLogic

    '    '追加作業API取得
    '    Dim dt As DataTable = IC3800804.GetAddRepairStatusList(dlrCD, orderNo)

    '    OutPutIFLog(dt, "IC3800804.GetAddRepairStatusList")

    '    '枝番（追加作業番号）が取得件数以上ない場合、データ不整合
    '    If Not IsNothing(dt) AndAlso workSeq <= dt.Rows.Count Then
    '        'テーブルの配列は0からのため、-1
    '        Dim dRow As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow _
    '            = DirectCast(dt.Rows(workSeq - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)
    '        'データの形式が不明なため、NULLチェックと空白削除を行っておく
    '        If Not dRow.IsINSPECTIONAPPROVALFLAGNull Then
    '            rtnValue = dRow.INSPECTIONAPPROVALFLAG.Trim()
    '        End If
    '    End If

    '    Logger.Info("GetChildChipInspectionApprovalFlg End Return: " + rtnValue)

    '    Return rtnValue
    'End Function

    ' ''' <summary>
    ' ''' ストール予約情報を更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="reserveInfo">ストール予約情報</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Public ReadOnly Property UpdateStallReserveInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                            ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                            ByVal updateAccount As String, _
    '                                            ByVal updateDate As Date) As Integer
    '    Get
    '        OutputLog(LOG_TYPE_INFO, "[S]UpdateStallReserveInfoData", "", Nothing, _
    '                  "ADAPTER:(DataTableAdapter)", _
    '                  "RESERVEINFO:(DataSet)", _
    '                  "UPDATEACCOUNT:" & updateAccount, _
    '                  "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '        ' 戻り値にエラーを設定
    '        UpdateStallReserveInfoData = ReturnNG
    '        Try
    '            ' TBL_STALLREZINFOのUPDATE、およびTBL_STALLREZHISのINSERT
    '            'reserveInfo.Rows.Item(0).Item("STARTTIME") = nextDayStartTime       '翌日の作業開始予定日時
    '            'reserveInfo.Rows.Item(0).Item("ENDTIME") = reserveEndTimeRevision   '翌日の作業終了予定日時
    '            'reserveInfo.Rows.Item(0).Item("REZ_WORK_TIME") = nextDayWorkTime    '翌日の予定作業時間(分)

    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '            '' ストール予約情報を更新する
    '            'Dim resultUpdRez As Integer = adapter.UpdateStallReserveInfo(reserveInfo, _
    '            '                                                             Date.MinValue, _
    '            '                                                             Date.MaxValue, _
    '            '                                                             KeepCurrent, _
    '            '                                                             KeepCurrent, _
    '            '                                                             updateAccount)
    '            'If (resultUpdRez <= 0) Then
    '            '    ' ストール予約情報の更新に失敗
    '            '    OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to update the stall reservation information.", Nothing)
    '            '    Exit Try
    '            'End If

    '            'Dim drStallReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
    '            '            DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

    '            ' ストール予約履歴を登録する
    '            'Dim resultInsRezHis As Integer = adapter.InsertReserveHistory(drStallReserveInfo.DLRCD, _
    '            '                                                              drStallReserveInfo.STRCD, _
    '            '                                                              CType(drStallReserveInfo.REZID, Integer), _
    '            '                                                              1)
    '            'If (resultInsRezHis <= 0) Then
    '            '    ' ストール予約履歴の登録に失敗
    '            '    OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to register the stall reservation history.", Nothing)
    '            '    Exit Try
    '            'End If

    '            'ストール予約情報を更新する
    '            Dim resultUpdRez As Integer = UpdateStallReserveInfo(adapter, _
    '                                                                 reserveInfo, _
    '                                                                 updateAccount, _
    '                                                                 updateDate)
    '            If (resultUpdRez <= 0) Then
    '                ' ストール予約情報の更新に失敗
    '                OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to update the stall reservation information.", Nothing)
    '                Exit Try
    '            End If

    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '            ' 正常終了
    '            UpdateStallReserveInfoData = ReturnOk

    '        Finally
    '            OutputLog(LOG_TYPE_INFO, "[E]UpdateStallReserveInfoData", "", Nothing, _
    '                      "RET:" & UpdateStallReserveInfoData.ToString(CultureInfo.CurrentCulture))
    '        End Try

    '        Return UpdateStallReserveInfoData
    '    End Get
    'End Property


    ' ''' <summary>
    ' ''' 当日分のストール実績情報を更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="procInfo">ストール実績情報</param>
    ' ''' <param name="serviceInId">サービス入庫ID</param>
    ' ''' <param name="resultStartTime">実績_ストール開始日時時刻</param>
    ' ''' <param name="resultEndTime">実績_ストール終了日時時刻</param>
    ' ''' <param name="resultWorkTime">実績_実績時間</param>
    ' ''' <param name="updateAccount">更新者</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Private Function UpdateStallProcessInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                            ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
    '                                            ByVal serviceInId As Long, _
    '                                            ByVal resultStartTime As Date, _
    '                                            ByVal resultEndTime As Date, _
    '                                            ByVal resultWorkTime As Integer, _
    '                                            ByVal updateAccount As String, _
    '                                            ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStallProcessInfoData", "", Nothing, _
    '             "ADAPTER:(DataTableAdapter)", _
    '             "PROCINFO:(DataSet)", _
    '             "RESULTSTARTTIME:" & resultStartTime.ToString(CultureInfo.CurrentCulture), _
    '             "RESULTENDTIME:" & resultEndTime.ToString(CultureInfo.CurrentCulture), _
    '             "RESULTWORKTIME:" & resultWorkTime.ToString(CultureInfo.CurrentCulture), _
    '             "ACCOUNT:" & updateAccount, _
    '             "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    UpdateStallProcessInfoData = ReturnNG
    '    Try
    '        ' 当日分のストール実績情報を更新する
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        ' 実績_ステータス（98:MidFinish）
    '        'procInfo.Rows.Item(0).Item("RESULT_STATUS") = SMB_RESULT_STATUS_MID_FINISH

    '        ' ストール利用ステータス（06:日跨ぎ終了）
    '        procInfo.Rows.Item(0).Item("STALL_USE_STATUS") = stallUseStetus06

    '        Dim drProc As SC3150101DataSet.SC3150101StallProcessInfoRow = _
    '                           DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

    '        If adapter.GetResultRelationChip(drProc.DLRCD, _
    '                                         drProc.STRCD, _
    '                                         serviceInId, _
    '                                         drProc.REZID) Then
    '            ' サービスステータス（06: 次の作業開始待ち）
    '            procInfo.Rows.Item(0).Item("RESULT_STATUS") = ServiceStetus_WaitingNextProcess
    '        Else
    '            ' サービスステータス（04: 作業開始待ち）
    '            procInfo.Rows.Item(0).Item("RESULT_STATUS") = ServiceStetus_WaitingtProcess

    '        End If

    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' (当日の)実績_ストール開始日時時刻
    '        procInfo.Rows.Item(0).Item("RESULT_START_TIME") = _
    '            resultStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())

    '        ' (当日の)実績_ストール終了日時時刻
    '        procInfo.Rows.Item(0).Item("RESULT_END_TIME") = _
    '            resultEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())

    '        ' (当日の)実績_実績時間
    '        procInfo.Rows.Item(0).Item("RESULT_WORK_TIME") = resultWorkTime

    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        '' ストール実績情報を更新する
    '        'Dim resultUpdProc As Integer = adapter.UpdateStallProcessInfo(procInfo, Nothing)
    '        'If (resultUpdProc <= 0) Then
    '        '    ' ストール実績情報の更新に失敗
    '        '    OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessInfoData", "Failed to update the stall process information.", Nothing)
    '        '    Exit Try
    '        'End If

    '        ' ストール実績情報を更新する
    '        Dim resultUpdProc As Integer = UpdateStallProcessInfo(adapter, _
    '                                                              procInfo, _
    '                                                              Nothing, _
    '                                                              updateAccount,
    '                                                              updateDate)
    '        If (resultUpdProc <= 0) Then
    '            ' ストール実績情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessInfoData", "Failed to update the stall process information.", Nothing)
    '            Exit Try
    '        End If
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' 正常終了
    '        UpdateStallProcessInfoData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStallProcessInfoData", "", Nothing, _
    '                  "RET:" & UpdateStallProcessInfoData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStallProcessInfoData

    'End Function


    ' ''' <summary>
    ' ''' 翌日のストール実績情報を更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="procInfo">ストール実績情報</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <History>
    ' ''' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </History>
    'Private Function InsertStallProcessInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                            ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
    '                                            ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                            ByVal nextDayStartTime As Date, _
    '                                            ByVal reserveEndTimeRevision As Date, _
    '                                            ByVal nextDayWorkTime As Integer, _
    '                                            ByVal updateAccount As String, _
    '                                            ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]InsertStallProcessInfoData", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "PROCINFO:(DataSet)", _
    '              "NEXTDAYSTARTTIME:" & nextDayStartTime.ToString(CultureInfo.CurrentCulture), _
    '              "RESERVEENDTIMEREVISION:" & reserveEndTimeRevision.ToString(CultureInfo.CurrentCulture), _
    '              "NEXTDAYWORKTIME:" & reserveEndTimeRevision.ToString(CultureInfo.CurrentCulture), _
    '              "UPDATEACCOUNT:" & updateAccount, _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    InsertStallProcessInfoData = ReturnNG
    '    Try
    '        ' 翌日のストール実績情報の設定
    '        'Dim daySeqNo As Integer = CType(procInfo.Rows.Item(0).Item("DSEQNO"), Integer) + 1
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        'procInfo.Rows.Item(0).Item("DSEQNO") = daySeqNo             ' 日跨ぎシーケンス番号：MAX(DSEQNO)+1
    '        'procInfo.Rows.Item(0).Item("SEQNO") = 1                     ' シーケンス番号：1固定
    '        'procInfo.Rows.Item(0).Item("RESULT_STATUS") = 10            ' 実績_ステータス（当日処理すると10）
    '        procInfo.Rows.Item(0).Item("STALL_USE_STATUS") = stallUseStetus01 ' ストール利用ステータス:作業開始待ち（当日処理すると01）
    '        reserveInfo.Rows.Item(0).Item("STARTTIME") = nextDayStartTime       '翌日の作業開始予定日時
    '        reserveInfo.Rows.Item(0).Item("ENDTIME") = reserveEndTimeRevision   '翌日の作業終了予定日時
    '        reserveInfo.Rows.Item(0).Item("REZ_WORK_TIME") = nextDayWorkTime    '翌日の予定作業時間(分)
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        procInfo.Rows.Item(0).Item("RESULT_START_TIME") = Nothing   ' 実績_ストール開始日時時刻（当日処理するとNULL）
    '        procInfo.Rows.Item(0).Item("RESULT_END_TIME") = Nothing     ' 実績_ストール終了日時時刻（当日処理するとNULL）




    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '        '' 翌日の分のストール実績情報を登録する
    '        'Dim resultInsProc As Integer = adapter.InsertStallProcessInfo(procInfo, _
    '        '                                                              updateAccount, _
    '        '                                                              True,
    '        '                                                              False)
    '        'If (resultInsProc <= 0) Then
    '        '    ' 翌日分のストール実績情報の登録に失敗
    '        '    OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to register the stall process information for the next day.", Nothing)
    '        '    Exit Try
    '        'End If

    '        '自動採番のストール利用IDを取得
    '        Dim stallUseId As SC3150101DataSet.SC3150101StallUseIdDataTable = adapter.GetSequenceStallUseId()

    '        ' ストール実績情報を登録する
    '        Dim resultInsMidFinish As Integer = adapter.InsertStallUseMidFinish(procInfo, _
    '                                                                            reserveInfo, _
    '                                                                            updateAccount, _
    '                                                                            stallUseId, _
    '                                                                            updateDate)
    '        If (resultInsMidFinish <= 0) Then
    '            ' 翌日分のストール実績情報の登録に失敗
    '            OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to register the stall process information for the next day.", Nothing)
    '            Exit Try
    '        End If
    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        ' 正常終了
    '        InsertStallProcessInfoData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]InsertStallProcessInfoData", "", Nothing, _
    '                  "RET:" & InsertStallProcessInfoData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return InsertStallProcessInfoData

    'End Function

    ' ''' <summary>
    ' ''' 担当者実績情報を更新する
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ' ''' <param name="dealerCode">販売店CD</param>
    ' ''' <param name="branchCode">店舗CD</param>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="reserveId">予約ID</param>
    ' ''' <param name="workTime">作業日付</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateStaffStallData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                      ByVal dealerCode As String, _
    '                                      ByVal branchCode As String, _
    '                                      ByVal stallId As Integer, _
    '                                      ByVal reserveId As Long, _
    '                                      ByVal workTime As Date, _
    '                                      ByVal updateAccount As String, _
    '                                      ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStaffStallData", "", Nothing, _
    '                             "ADAPTER:(DataTableAdapter)", _
    '                             "DLRCD:" & dealerCode, _
    '                             "STRCD:" & branchCode, _
    '                             "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
    '                             "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
    '                             "WORK_TIME:" & workTime.ToString(CultureInfo.CurrentCulture), _
    '                             "UPDATEACCONUT:" & updateAccount, _
    '                             "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    ' 戻り値にエラーを設定
    '    UpdateStaffStallData = ReturnNG
    '    Try
    '        ' 担当者実績情報の取得
    '        Dim staffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoDataTable = _
    '                                    adapter.GetStaffResultInfo(reserveId, workTime, updateAccount)
    '        Dim drStaffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoRow
    '        If staffResultInfo IsNot Nothing AndAlso staffResultInfo.Count <> 0 Then
    '            drStaffResultInfo = DirectCast(staffResultInfo.Rows(0), SC3150101DataSet.SC3150101StaffResultInfoRow)

    '            ' 実績ストール終了日時時刻
    '            Dim endTime As String = Nothing
    '            If IsDBNull(drStaffResultInfo.Item("RESULT_END_TIME")) = False Then
    '                endTime = drStaffResultInfo.RESULT_END_TIME
    '            End If

    '            ' 担当者実績情報の更新
    '            Dim staffResult As Integer

    '            'If (SMB_RESULT_STATUS_IN_SHED.Equals(drStaffResultInfo.RESULT_STATUS)) Then
    '            '    ' 実績ステータス：10
    '            If (stallUseStetus01.Equals(drStaffResultInfo.RESULT_STATUS)) Then
    '                ' サービスステータス：04

    '                ' 担当者ストール実績データの削除
    '                staffResult = adapter.DeleteStaffStall(dealerCode, _
    '                                                       branchCode, _
    '                                                       stallId, _
    '                                                       reserveId, _
    '                                                       updateAccount, _
    '                                                       workTime)

    '                'ElseIf (SMB_ResultStatusWorking.Equals(drStaffResultInfo.RESULT_STATUS)) Then
    '                '    ' 実績ステータス：20ServiceStetus_WaitingtProcess
    '            ElseIf (stallUseStetus02.Equals(drStaffResultInfo.RESULT_STATUS)) Then
    '                ' サービスステータス：05

    '                If String.IsNullOrWhiteSpace(endTime) Then

    '                    '' 値がない場合、半角スペースを設定
    '                    'endTime = " "

    '                    ' 値がない場合、日付最小値を設定
    '                    endTime = MINDATE

    '                End If
    '                ' 担当者ストール実績データの更新
    '                staffResult = adapter.UpdateStaffStallAtWork(reserveId, _
    '                                                             updateAccount, _
    '                                                             workTime)

    '            Else
    '                ' 実績ステータス：上記以外
    '                ' 担当者ストール実績データの更新
    '                staffResult = adapter.UpdateStaffStall(reserveId, _
    '                                                       endTime, _
    '                                                       updateAccount, _
    '                                                       updateDate)

    '            End If
    '            If (staffResult <= 0) Then
    '                ' 担当者実績情報の更新に失敗
    '                OutputLog(LOG_TYPE_ERROR, "SuspendWork", "Failed to update the stall staff information.", Nothing)
    '                Exit Try
    '            End If
    '        End If

    '        ' 正常終了
    '        UpdateStaffStallData = ReturnOk

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStaffStallData", "", Nothing, _
    '                  "RET:" & UpdateStaffStallData.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStaffStallData

    'End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

#End Region

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START

#Region "作業終了処理"

    ''' <summary>
    ''' 作業終了処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="updateAccount">更新者アカウント</param>
    ''' <param name="rowUpdateCount">行ロックバージョン</param>
    ''' <param name="orderNo">RO番号</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' 2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) 
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </History>
    <EnableCommit()>
    Public Function FinishWork(ByVal dealerCode As String, _
                               ByVal branchCode As String, _
                               ByVal reserveId As Decimal, _
                               ByVal stallId As Decimal, _
                               ByVal updateAccount As String, _
                               ByVal rowUpdateCount As Long,
                               ByVal orderNo As String, _
                               Optional ByVal isBreak As Boolean = False, _
                               Optional ByVal breakBottomFlg As Boolean = False) As Integer

        '開始ログを出力
        OutputLog(LOG_TYPE_INFO, "[S]FinishWork", "", Nothing, _
                  "DLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, _
                  "REZID:" & CType(reserveId, String), _
                  "STALLID:" & CType(stallId, String), _
                  "ACCOUNT:" & updateAccount, _
                  "ORDERNO:" & orderNo)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '' 戻り値にエラーを設定
        'FinishWork = ReturnNG

        ' 戻り値に正常を設定
        FinishWork = ReturnOk

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' (実際の)作業開始日時を取得する
        Dim actualStratTime As Date = DateTimeFunc.Now(dealerCode)

        ' 2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        '秒の切り捨て
        'Dim startTime As Date = CType(DateTimeFunc.FormatDate(2, actualStratTime), Date)
        Dim startTime As Date = actualStratTime.AddSeconds(-actualStratTime.Second)
        ' 2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        '当日の日付取得
        'Dim todayDate As Date = actualStratTime.Date

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        ''SMBコモンクラスのインスタンス宣言
        'Dim SmbCommonClass As New SMBCommonClassBusinessLogic
        '' チップ衝突フラグ
        'Dim IsCollisionFlg As Boolean = False

        'SMBコモンクラスのインスタンス宣言
        Dim tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
        'ログイン中のスタッフ情報
        'Dim userContext As StaffContext = StaffContext.Current
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
        ' SC3150101TableAdapterクラスのインスタンスを生成
        Dim adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                        New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

        Try
            ' ---------------------------------------------------------------------------------
            ' ストール予約情報を取得する
            ' ---------------------------------------------------------------------------------
            Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable
            ' ストール予約情報を取得
            reserveInfo = adapter.GetStallReserveInfo(dealerCode, branchCode, reserveId)

            If reserveInfo Is Nothing OrElse reserveInfo.Count <= 0 Then
                ' ストール予約情報の取得に失敗
                OutputLog(LOG_TYPE_INFO, "FinishWork", _
                          "Failed to get the stall reservation information.(reserveId:" & reserveId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                FinishWork = ReturnNG_FINISH

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            'ストール予約のDataSetをDataSetRowに変換
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                       DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            ' ---------------------------------------------------------------------------------
            ' ストール実績情報を取得する
            ' ---------------------------------------------------------------------------------
            Dim procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable
            ' ストール実績情報の取得
            procInfo = adapter.GetStallProcessInfo(dealerCode, branchCode, reserveId)

            If procInfo Is Nothing OrElse procInfo.Count <= 0 Then
                ' ストール実績情報の取得に失敗
                Me.OutputLog(LOG_TYPE_INFO, "FinishWork", _
                          "Failed to get the stall process information. (reserveId:" & reserveId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ' 戻り値にエラーを設定
                FinishWork = ReturnNG_FINISH

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Exit Try
            End If

            'ストール実績のDataSetをDataSetRowに変換
            Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow = _
                               DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            '' *************************************************************************************
            '' 【排他処理チェック（他端末からの更新がないこと）。】
            '' *************************************************************************************
            'If drProcInfo.UPDATE_COUNT <> updateCount Then
            '    Me.OutputLog(LOG_TYPE_WARNING, "StartWork", "This chip has been modified by another operator. Please reload and try again.", Nothing)
            '    FinishWork = 923     '「既に他のオペレータによって更新されています。画面をリロードしてからもう一度お試し下さい。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【当該チップの実績ステータスチェック（作業終了以降でないこと）。】
            '' *************************************************************************************
            'If Not IsDBNull(drProcInfo.Item("STALL_USE_STATUS")) _
            '  AndAlso Not stallUseStetus02.Equals(drProcInfo.STALL_USE_STATUS) Then


            '    ' すでに作業終了されている
            '    Me.OutputLog(LOG_TYPE_WARNING, "FinishWork", "Cannot work end.The selected chip is not a work.", Nothing)
            '    FinishWork = 924     '「作業終了できませんでした。選択チップが作業中ではありません。」
            '    Exit Try
            'End If

            '' *************************************************************************************
            '' 【当該チップの完成検査ステータスチェック（完成検査未完了であること）。】
            '' *************************************************************************************
            'If Not IsDBNull(drProcInfo.Item("INSPECTION_STATUS")) _
            '  AndAlso Not INSPECTION_APPROVAL_BEFORE.Equals(drProcInfo.INSPECTION_STATUS) Then


            '    ' 完成検査未完了ではない
            '    Me.OutputLog(LOG_TYPE_WARNING, "FinishWork", "Cannot work end.The selected chip has already complete inspection approved.", Nothing)
            '    FinishWork = 925     '「作業終了できませんでした。選択チップは既に完成検査依頼済みです。」
            '    Exit Try
            'End If
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            ' -------------------------------------------------------------------------------------
            ' ストール時間を取得する
            ' -------------------------------------------------------------------------------------

            'ストールID取得
            stallId = CType(drReserveInfo.STALLID, Decimal)

            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            '' 実績の作業開始日時を取得する
            'Dim procStartTime As Date = Date.ParseExact(drProcInfo.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
            '' 実績の作業終了日時を取得する
            'Dim procEndTime As Date = Date.ParseExact(drProcInfo.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            ' ストール時間を取得する
            Dim stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = _
                adapter.GetStallTimeInfo(dealerCode, branchCode, CType(drReserveInfo.STALLID, Decimal))
            ' 時間情報を取得する
            Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = _
                DirectCast(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

            ' プログレス開始および終了時間を設定する
            If (drStallTimeInfo.IsPSTARTTIMENull = True) Then
                drStallTimeInfo.PSTARTTIME = drStallTimeInfo.STARTTIME
                drStallTimeInfo.PENDTIME = drStallTimeInfo.ENDTIME
            End If

            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            '' (実際の)作業終了日時を取得する
            'Dim resultEndTime As Date = actualStratTime

            '' 作業終了時刻をチェック
            'resultEndTime = CheckEndTime(dealerCode, _
            '                             branchCode, _
            '                             stallId, _
            '                             procStartTime, _
            '                             resultEndTime, _
            '                             procEndTime)
            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            ' 稼動開始時刻
            Dim startOperationTime As TimeSpan = Me.SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
            ' 稼動終了時刻
            Dim endOperationTime As TimeSpan = Me.SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内の予約情報の取得
            ' ---------------------------------------------------------------------------------
            ' ストール開始時間
            Dim stallStartTime As TimeSpan = startOperationTime
            ' ストール終了時間
            Dim stallEndTime As TimeSpan = endOperationTime
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDate As Date = startTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDate As Date = GetEndDateRange(fromDate, stallStartTime, stallEndTime)

            ' 指定範囲内のストール予約情報を取得
            Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                        adapter.GetStallReserveList(dealerCode, branchCode, _
                                                                    stallId, reserveId, fromDate, toDate)
            ' 指定範囲内のストール実績情報を取得
            Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                        adapter.GetStallProcessList(dealerCode, branchCode, _
                                                                    stallId, fromDate, toDate)
            ' 指定範囲内の予約情報の取得
            reserveList = GetReserveList(reserveList, processList, stallId, _
                                            reserveId, fromDate, isBreak)

            ' ---------------------------------------------------------------------------------
            ' 休憩取得有無判定
            ' CheckBreak()
            ' ---------------------------------------------------------------------------------
            ' 休憩時間帯・使用不可時間帯を取得する（チップの移動時に休憩時間を考慮する必要があるため）
            'Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
            '    adapter.GetBreakSlot(CType(drReserveInfo.STALLID, Integer), fromDate, toDate)

            ' 休憩取得有無をチェック（休憩取得有無を取得）
            'Dim resultBreak As Boolean = CheckBreak(breakInfo, _
            '                                        isBreak, _
            '                                        procStartTime, _
            '                                        procEndTime, _
            '                                        CType(drReserveInfo.REZ_WORK_TIME, Integer))

            ' 作業終了時間からの経過時間を取得する
            'Dim timeDiff As Long = resultEndTime.Minute Mod drStallTimeInfo.TIMEINTERVAL
            'Dim endTimeReserve As Date
            'If (timeDiff > 0) Then
            '    endTimeReserve = resultEndTime.AddMinutes(drStallTimeInfo.TIMEINTERVAL - timeDiff)
            'Else
            '    endTimeReserve = resultEndTime
            'End If

            ' 休憩取得ボタン押下有無判定
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'drProcInfo.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProcInfo.REST_FLG)

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定する場合
                If biz.IsRestAutoJudge() Then
                    drProcInfo.REST_FLG = REST_FLG_TAkE
                Else
            ' 休憩取得ボタン押下有無判定
            drProcInfo.REST_FLG = BreakBottomClickCheck(isBreak, breakBottomFlg, drProcInfo.REST_FLG)
                End If
            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

            '秒数を切り捨てる
            Dim rsltEndDateTime As Date = actualStratTime
            rsltEndDateTime = rsltEndDateTime.AddSeconds(-rsltEndDateTime.Second)

            Dim returnValue As Long = tabletSmbCommonClass.Finish(drProcInfo.REZID _
                                                                  , rsltEndDateTime _
                                                                  , drProcInfo.REST_FLG _
                                                                  , actualStratTime _
                                                                  , rowUpdateCount _
                                                                  , APPLICATION_ID)

            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            'PUSH送信フラグの取得(終了) 
            NeedPushFinishSingleJob = tabletSmbCommonClass.NeedPushAfterFinishSingleJob()

            'PUSH送信フラグの取得(中断)
            NeedPushStopSingleJob = tabletSmbCommonClass.NeedPushAfterStopSingleJob()
            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            NeedPushSubAreaRefresh = tabletSmbCommonClass.NeedPushSubAreaRefresh()
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            '作業完了処理が失敗した場合
            If returnValue <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                FinishWork = OtherSystemsReturnCodeSelect(returnValue, workFinishFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''作業開始処理に失敗しました
                'OutputLog(LOG_TYPE_ERROR, "tabletSmbCommonClass.Start", "Failed to start of processing chip.", Nothing)
                'Exit Try

                'エラー内容チェック
                If FinishWork <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    OutputLog(LOG_TYPE_WARNING, "tabletSmbCommonClass.Finish", "Failed to work completion of processing chip.", Nothing)
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            '' *************************************************************************************
            '' 【作業開始にあたり、他チップの移動処理に失敗しないかチェック。】
            '' *************************************************************************************

            '' 衝突有無判定
            'If IsCollision(reserveList, reserveId, startTime, endTimeReserve) = True Then

            '    'ストールロックテーブル登録処理
            '    If SmbCommonClass.RegisterStallLock(stallId, _
            '                                        todayDate, _
            '                                        updateAccount, _
            '                                        actualStratTime, _
            '                                        APPLICATION_ID) <> ReturnOk Then

            '        'ストールロックに失敗
            '        Me.OutputLog(LOG_TYPE_ERROR, "FinishWork", _
            '                 "Failed to stall lock. (stallId:" & stallId.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            '        Exit Try
            '    End If

            '    IsCollisionFlg = True
            'End If

            ''サービス入庫テーブルのロック処理
            'If SmbCommonClass.LockServiceInTable(drReserveInfo.PREZID, _
            '                                     drReserveInfo.UPDATE_COUNT, _
            '                                     CANCEL_FLG, _
            '                                     updateAccount, _
            '                                     actualStratTime, _
            '                                     APPLICATION_ID) <> ReturnOk Then
            '    'ストールロックに失敗
            '    Me.OutputLog(LOG_TYPE_ERROR, "FinishWork", _
            '             "Failed to serviceIn lock. (serviceInId:" & drReserveInfo.PREZID.ToString(CultureInfo.InvariantCulture) & ")", Nothing)
            '    Exit Try
            'End If

            '' 予約リレーションチップを取得する
            'If adapter.GetReserveRelationChip(dealerCode, _
            '                                 branchCode, _
            '                                 drReserveInfo.PREZID) Then

            '    ' サービスステータス（06: 次の作業開始待ち）
            '    reserveInfo.Rows.Item(0).Item("SVC_STATUS") = ServiceStetus_WaitingNextProcess
            'Else
            '    ' サービスステータス（09: 検査待ち）
            '    reserveInfo.Rows.Item(0).Item("SVC_STATUS") = ServiceStetus_WaitingInspection

            'End If

            '' ストール予約に該当する情報を更新する
            'Dim resultUpdRez As Integer = UpdateStallReserveInfo(adapter, _
            '                                                     reserveInfo, _
            '                                                     updateAccount,
            '                                                     actualStratTime)

            'If resultUpdRez <= 0 Then
            '    OutputLog(LOG_TYPE_ERROR, "UpdateStallReserveInfo", "Failed to update the stall reserve information.", Nothing)    'ストール予約に該当する情報の更新に失敗
            '    Exit Try
            'End If

            '' ストール実績情報を更新する
            'procInfo.Rows.Item(0).Item("RESULT_START_TIME") = procStartTime.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
            'procInfo.Rows.Item(0).Item("RESULT_END_TIME") = resultEndTime.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
            'procInfo.Rows.Item(0).Item("RESULT_WORK_TIME") = CalculateWorkTime(breakInfo, procStartTime, resultEndTime, resultBreak)
            'procInfo.Rows.Item(0).Item("STALL_USE_STATUS") = stallUseStetus03

            '' 実績に該当するストール利用の更新
            'If (adapter.UpdateProcessStallUse(procInfo, Nothing, updateAccount, actualStratTime) <= 0) Then
            '    OutputLog(LOG_TYPE_ERROR, "UpdateProcessStallUse", "Failed to update the stall process information.", Nothing)    'ストール利用の更新に失敗
            '    Exit Try
            'End If

            '' 担当者ストール実績データの更新
            'If UpdateStaffStall(adapter, drReserveInfo, stallTimeInfo, procStartTime, updateAccount, actualStratTime) <> ReturnOk Then
            '    OutputLog(LOG_TYPE_ERROR, "UpdateStaffStall", "Failed to update the stall staff information.", Nothing)          '担当者ストール実績データの更新に失敗
            '    Exit Try
            'End If


            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
            
            '' 正常終了
            'FinishWork = ReturnOk

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Finally

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            'If IsCollisionFlg = True Then
            '    'ストールロックテーブル削除処理
            '    Dim stallLock As Integer = SmbCommonClass.DeleteStallLock(stallId, _
            '                                                              todayDate, _
            '                                                              updateAccount, _
            '                                                              actualStratTime, _
            '                                                              APPLICATION_ID)
            '    If (stallLock <> ReturnOk) Then
            '        OutputLog(LOG_TYPE_ERROR, "UpdateStaffStall", "Failed to remove the stall lock.", Nothing)         'ストールロック削除に失敗
            '    End If

            'End If
            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
            
            '' 正常終了以外はロールバック
            'If FinishWork <> ReturnOk Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If FinishWork <> ReturnOk AndAlso FinishWork <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                tabletSmbCommonClass.Dispose()
                adapter = Nothing
            End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
            ' リソースを解放
            'If SmbCommonClass IsNot Nothing Then
            '    SmbCommonClass.Dispose()
            '    SmbCommonClass = Nothing
            'End If

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            OutputLog(LOG_TYPE_INFO, "[E]FinishWork", "", Nothing, _
                      "RET:" & FinishWork.ToString(CultureInfo.CurrentCulture))

        End Try

        Return FinishWork

    End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 担当者ストール実績データの更新
    ' ''' </summary>
    ' ''' <param name="adapter">SC3150101TableAdapterクラス</param>
    ' ''' <param name="drReserveInfo">ストール予約情報</param>
    ' ''' <param name="stallTimeInfo">ストール時間情報</param>
    ' ''' <param name="procStartTime">実績の作業開始日時</param>
    ' ''' <param name="upDateDate">更新日時</param>
    ' ''' <returns>処理結果（正常：0、エラー：-1）</returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateStaffStall(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter _
    '                                  , ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow _
    '                                  , ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable _
    '                                  , ByVal procStartTime As Date _
    '                                  , ByVal updateAccount As String _
    '                                  , ByVal upDateDate As Date) As Long

    '    Logger.Info("[S]UpdateStaffStall()")

    '    ' 作業日付を取得する
    '    Dim workTime As Date = GetWorkDate(stallTimeInfo, procStartTime)

    '    ' 担当者ストール実績データ情報を取得する
    '    Dim staffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoDataTable = adapter.GetStaffResultInfo(drReserveInfo.REZID, _
    '                                                                                                           workTime, _
    '                                                                                                           updateAccount)


    '    Logger.Info("UpdateStaffStall_SC3150101StaffResultInfoDataTable_ROWS_COUNT:" & staffResultInfo.Rows.Count.ToString(CultureInfo.CurrentCulture))
    '    If 0 < staffResultInfo.Rows.Count Then


    '        Dim drStaffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoRow = DirectCast(staffResultInfo.Rows(0), SC3150101DataSet.SC3150101StaffResultInfoRow)


    '        '作業終了日付を取得、初期値に日付最小値を設定
    '        Dim workEndTime As String = MINDATE


    '        If drStaffResultInfo.IsRESULT_END_TIMENull = False Then
    '            workEndTime = drStaffResultInfo.RESULT_END_TIME
    '        End If

    '        ' 担当者ストール実績データの更新
    '        If (adapter.UpdateStaffStall(drReserveInfo.REZID, _
    '                                     workEndTime, _
    '                                     updateAccount, _
    '                                     upDateDate) <= 0) Then
    '            Return ReturnNG
    '        End If
    '    End If

    '    Logger.Info("[E]UpdateStaffStall()")

    '    Return ReturnOk

    'End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

#End Region

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

    ' 2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

#Region "中断処理"

    ''' <summary>
    ''' Job中断処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stopTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="rowUpdateCount">行ロックバージョン</param>
    ''' <param name="applicationId">画面ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function JobStop(ByVal stallUseId As Decimal, _
                            ByVal stopTime As Long, _
                            ByVal stopMemo As String, _
                            ByVal stopReasonType As String, _
                            ByVal restFlg As String, _
                            ByVal rowUpdateCount As Long,
                            ByVal applicationId As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻り値
        Dim resultCode As Long = 0
        'ユーザー情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)


        Try

            'SMBコモンクラスのインスタンスを生成
            Using smbCommonClass As New TabletSMBCommonClassBusinessLogic

                '中断処理
                resultCode = smbCommonClass.JobStop(stallUseId, _
                                                    nowDateTime, _
                                                    stopTime, _
                                                    stopMemo, _
                                                    stopReasonType, _
                                                    restFlg, _
                                                    nowDateTime, _
                                                    rowUpdateCount, _
                                                    applicationId)

                'PUSH送信フラグの取得
                NeedPushStopSingleJob = smbCommonClass.NeedPushAfterStopSingleJob()

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                ' サブエリアリフレッシュフラグの取得
                NeedPushSubAreaRefresh = smbCommonClass.NeedPushSubAreaRefresh()
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            End Using

            '中断処理に失敗した場合
            If resultCode <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                resultCode = Me.OtherSystemsReturnCodeSelect(resultCode, workStopFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                
                ''中断処理に失敗しました
                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                     , "{0}.{1}.{2}" _
                '                     , Me.GetType.ToString _
                '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                     , "Failed to interruption of processing job."))
                'Exit Try

                'エラー内容チェック
                If resultCode <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    '中断処理に失敗しました
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1}.{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , "Failed to interruption of processing job."))
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

        Finally

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了以外はロールバック
            'If resultCode <> ActionResult.Success Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If resultCode <> ReturnOk AndAlso resultCode <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return resultCode

    End Function

#End Region

    ' 2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ' ''' <summary>
    ' ''' 旧DBストール予約に該当するテーブルの更新
    ' ''' </summary>
    ' ''' <param name="adapter"></param>
    ' ''' <param name="reserveInfo">ストール予約情報</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateStallReserveInfo(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                        ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                        ByVal updateAccount As String, _
    '                                        ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStallReserveInfo", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "RESERVEINFO:(DataSet)", _
    '              "ACCOUNT:" & updateAccount, _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    UpdateStallReserveInfo = 1

    '    Try

    '        ' ストール利用情報を更新する
    '        Dim resultUpdRezStallUse As Integer = adapter.UpdateReserveStallUse(reserveInfo, updateAccount, updateDate)

    '        If (resultUpdRezStallUse <= 0) Then
    '            ' ストール利用情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateReserveStallUse", "Failed to update the stall reservation information.", Nothing)
    '            UpdateStallReserveInfo = 0
    '            Exit Try
    '        End If

    '        ' サービス入庫情報を更新する
    '        Dim resultUpdRezServiceIn As Integer = adapter.UpdateReserveServiceIn(reserveInfo)

    '        If (resultUpdRezServiceIn <= 0) Then
    '            'サービス入庫情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateReserveServiceIn", "Failed to update the stall reservation information.", Nothing)
    '            UpdateStallReserveInfo = 0
    '            Exit Try
    '        End If

    '        ' 作業内容情報を更新する
    '        Dim resultUpdRezJobDetail As Integer = adapter.UpdateJobDetail(reserveInfo, updateAccount, updateDate)

    '        If (resultUpdRezJobDetail <= 0) Then
    '            '作業内容情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateJobDetail", "Failed to update the stall reservation information.", Nothing)
    '            UpdateStallReserveInfo = 0
    '            Exit Try
    '        End If

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStallReserveInfo", "", Nothing, _
    '                  "RET:" & UpdateStallReserveInfo.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStallReserveInfo

    'End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 旧DBストール実績に該当するテーブルの更新
    ' ''' </summary>
    ' ''' <param name="adapter"></param>
    ' ''' <param name="procInfo"></param>
    ' ''' <param name="reserveInfo"></param>
    ' ''' <param name="updateAccount"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateStallProcessInfo(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
    '                                        ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
    '                                        ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
    '                                        ByVal updateAccount As String, _
    '                                        ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateStallProcessInfoData", "", Nothing, _
    '              "ADAPTER:(DataTableAdapter)", _
    '              "PROCINFO:(DataSet)", _
    '              "RESERVEINFO:(DataSet)", _
    '              "ACCOUNT:" & updateAccount, _
    '              "UPDATEDATE:" & updateDate.ToString(CultureInfo.CurrentCulture))

    '    UpdateStallProcessInfo = 1

    '    Try

    '        ' ストール利用情報を更新する
    '        Dim resultUpdStallUse As Integer = adapter.UpdateProcessStallUse(procInfo, reserveInfo, updateAccount, updateDate)

    '        If (resultUpdStallUse <= 0) Then
    '            ' ストール利用情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateProcessStallUse", "Failed to update the stall process information.", Nothing)
    '            UpdateStallProcessInfo = 0
    '            Exit Try
    '        End If

    '        ' サービス入庫情報を更新する
    '        Dim resultUpdServiceIn As Integer = adapter.UpdateProcessServiceIn(procInfo, reserveInfo, updateAccount, updateDate)

    '        If (resultUpdServiceIn <= 0) Then
    '            ' サービス入庫情報の更新に失敗
    '            OutputLog(LOG_TYPE_ERROR, "UpdateProcessServiceIn", "Failed to update the stall process information.", Nothing)
    '            UpdateStallProcessInfo = 0
    '            Exit Try
    '        End If

    '    Finally
    '        OutputLog(LOG_TYPE_INFO, "[E]UpdateStallProcessInfo", "", Nothing, _
    '                  "RET:" & UpdateStallProcessInfo.ToString(CultureInfo.CurrentCulture))
    '    End Try

    '    Return UpdateStallProcessInfo

    'End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' 翌日の作業開始予定日時の取得（当日処理用）
    ''' </summary>
    ''' <param name="reserveEndTime">予定ストール終了日時</param>
    ''' <param name="stallStartTime">ストール稼動開始時刻</param>
    ''' <returns>翌日の作業開始予定日時</returns>
    ''' <remarks></remarks>
    Private Function GetNextDayStartTime(ByVal reserveEndTime As Date, _
                                         ByVal stallStartTime As TimeSpan) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetNextDayStartTime", "", Nothing, _
                  "END_TIME:" & reserveEndTime.ToString(CultureInfo.InvariantCulture()), _
                  "START_TIME:" & stallStartTime.ToString())

        Dim nextStartTime As Date ' 翌日の作業開始予定日時

        nextStartTime = reserveEndTime.Date.Add(stallStartTime)

        OutputLog(LOG_TYPE_INFO, "[E]GetNextDayStartTime", "", Nothing, _
                  "RET:" & nextStartTime.ToString(CultureInfo.InvariantCulture()))

        Return nextStartTime
    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 翌日の予定作業時間(分)の取得（当日処理用）
    ' ''' </summary>
    ' ''' <param name="reserveEndTime">予定ストール終了日時</param>
    ' ''' <param name="stallStartTime">ストール稼動開始時刻</param>
    ' ''' <returns>翌日の予定作業時間(分)</returns>
    ' ''' <remarks></remarks>
    'Private Function GetNextDayWorkTime(ByVal reserveEndTime As Date, _
    '                                    ByVal nextDays As Date, _
    '                                    ByVal stallStartTime As TimeSpan, _
    '                                    ByVal workingStratTime As Date, _
    '                                    ByVal workingEndTime As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]GetNextDayWorkTime", "", Nothing, _
    '              "END_TIME:" & reserveEndTime.ToString(CultureInfo.InvariantCulture()), _
    '              "START_TIME:" & stallStartTime.ToString())

    '    Dim duration As TimeSpan
    '    Dim nextDayWorkDay As Integer
    '    Dim nextDayWorkTimeHour As Integer
    '    Dim nextDayWorkTimeMinute As Integer
    '    Dim nextDayWorkTime As Integer ' 翌日の予定作業時間(分)
    '    Dim nextDayStartTime As Date ' 翌日の作業開始予定日時

    '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '    Dim workingTimeDate As TimeSpan
    '    Dim workingTimeHour As Integer
    '    Dim workingTimeMinute As Integer
    '    Dim workingTime As Integer ' 翌日の予定作業時間(分)

    '    '稼働時間の取得
    '    workingTimeDate = workingEndTime.Subtract(workingStratTime)
    '    workingTimeHour = workingTimeDate.Hours
    '    workingTimeMinute = workingTimeDate.Minutes
    '    '稼働時間を分に変換
    '    workingTime = (workingTimeHour * 60) + workingTimeMinute
    '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    nextDayStartTime = nextDays.Date.Add(stallStartTime)
    '    duration = reserveEndTime.Subtract(nextDayStartTime)

    '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '    '予定終了日時が翌日以上の場合1日減算する
    '    'If duration.Days > 1 Then
    '    '    nextDayWorkDay = duration.Days - 1
    '    'Else
    '    nextDayWorkDay = duration.Days
    '    'End If

    '    nextDayWorkTimeHour = duration.Hours
    '    nextDayWorkTimeMinute = duration.Minutes
    '    'nextDayWorkTime = (nextDayWorkTimeHour * 60) + nextDayWorkTimeMinute
    '    nextDayWorkTime = (((nextDayWorkDay * workingTime) + (nextDayWorkTimeHour) * 60)) + nextDayWorkTimeMinute

    '    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    OutputLog(LOG_TYPE_INFO, "[E]GetNextDayWorkTime", "", Nothing, _
    '              "RET:" & CType(nextDayWorkTime, String))

    '    Return nextDayWorkTime
    'End Function
    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 作業終了時刻判定
    ''' 作業開始時刻と作業終了時刻の稼動時間帯が異なる場合、終了時刻を作業予定終了時刻にする
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="endTime">作業終了時間</param>
    ''' <param name="procEndTime">実績の作業予定終了時間</param>
    ''' <returns>終了時間</returns>
    ''' <remarks></remarks>
    Public Function CheckEndTime(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal stallId As Decimal, _
                                 ByVal startTime As Date, _
                                 ByVal endTime As Date, _
                                 ByVal procEndTime As Date) As Date

        'Logger.Info("[S]CheckEndTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CheckEndTime", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "PROC_END_TIME:" & procEndTime.ToString(CultureInfo.CurrentCulture()))


        Dim retEndTime As Date


        Dim adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

        Try
            ' ストール時間を取得する
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable
            stallInfo = adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)

            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
            drStallInfo = CType(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

            Dim operationStartTime As TimeSpan ' 稼働開始時間
            Dim operationEndTime As TimeSpan   ' 稼働終了時間
            operationStartTime = SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            operationEndTime = SetStallTime(drStallInfo.PENDTIME).TimeOfDay

            Dim sTimeKadoStart As Date ' 稼動開始時刻(開始)
            Dim eTimeKadoStart As Date ' 稼動開始時刻(終了)
            If startTime.Add(operationStartTime) < startTime.Add(operationEndTime) Then
                ' 通常稼動の場合は単に日付の差異をチェック
                If startTime.Date <> endTime.Date Then
                    endTime = procEndTime
                End If
            Else
                ' 日跨ぎ稼動の場合は、開始・終了ごとの稼動開始時刻を取得
                ' 開始時刻
                If (startTime.Date.AddDays(-1).Add(operationStartTime) <= startTime) _
                    And (startTime < startTime.Date.Add(operationEndTime)) Then
                    sTimeKadoStart = startTime.Date.AddDays(-1).Add(operationStartTime)
                Else
                    sTimeKadoStart = startTime.Date.Add(operationStartTime)
                End If
                ' 終了時刻
                If (endTime.Date.AddDays(-1).Add(operationStartTime) <= endTime) _
                    And (endTime < endTime.Date.Add(operationEndTime)) Then
                    eTimeKadoStart = endTime.Date.AddDays(-1).Add(operationStartTime)
                Else
                    eTimeKadoStart = endTime.Date.Add(operationStartTime)
                End If

                If sTimeKadoStart.Date <> eTimeKadoStart.Date Then
                    endTime = procEndTime
                End If
            End If

            retEndTime = endTime

        Finally
            ' adapterを破棄する
            If adapter IsNot Nothing Then
                adapter.Dispose()
            End If
        End Try

        'Logger.Info("[E]CheckEndTime()")
        OutputLog(LOG_TYPE_INFO, "[E]CheckEndTime", "", Nothing, _
                  "RET:" & retEndTime.ToString(CultureInfo.CurrentCulture()))
        Return (retEndTime)

    End Function


    ''' <summary>
    ''' 作業日付取得
    ''' 日跨ぎ稼動の場合は作業日付を-1日する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="procDate">作業開始時間</param>
    ''' <returns>作業日付</returns>
    ''' <remarks></remarks>
    Public Function GetWorkDate(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                ByVal procDate As Date) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetWorkDate", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", _
                  "PROC_DATE:" & procDate.ToString(CultureInfo.CurrentCulture()))

        Dim workDate As Date

        If stallTimeInfo Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetWorkDate", "", Nothing, _
                  "RET:" & procDate.ToString(CultureInfo.CurrentCulture()))
            Return procDate
        End If

        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

        '稼動時間帯を取得
        Dim operationStartTimet As TimeSpan
        Dim operationEndTime As TimeSpan
        operationStartTimet = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        workDate = procDate
        'WORKDATEの値を確定
        If procDate.Date.Add(operationStartTimet) > procDate.Date.Add(operationEndTime) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If (procDate.Date.AddDays(-1).Add(operationStartTimet) <= procDate) And _
               (procDate < procDate.Date.Add(operationEndTime)) Then
                '前日の稼動時間帯なら-1日する
                workDate = procDate.AddDays(-1)
            End If
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetWorkDate", "", Nothing, _
                  "RET:" & workDate.ToString(CultureInfo.CurrentCulture()))
        Return workDate

    End Function


    ''' <summary>
    ''' 指定範囲時間の終了時間を取得
    ''' </summary>
    ''' <param name="fromDate">範囲(FROM)</param>
    ''' <param name="procStartTime">開始時間</param>
    ''' <param name="procEndTime">終了時間</param>
    ''' <returns>範囲(TO)</returns>
    ''' <remarks></remarks>
    Private Function GetEndDateRange(ByVal fromDate As Date, _
                                     ByVal procStartTime As TimeSpan, _
                                     ByVal procEndTime As TimeSpan) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetEndDateRange", "", Nothing, _
                  "FROM_DATE:" & fromDate.ToString(CultureInfo.CurrentCulture()), _
                  "PROC_START_TIME:" & procStartTime.ToString(), _
                  "PROC_END_TIME:" & procEndTime.ToString())

        Dim toDate As Date

        '日跨ぎ稼動の場合
        If fromDate.Date.Add(procStartTime) > fromDate.Date.Add(procEndTime) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If fromDate.Date.AddDays(-1).Add(procStartTime) <= fromDate _
                And fromDate < fromDate.Date.Add(procEndTime) Then
                toDate = fromDate.Date.Add(procEndTime)
            Else
                toDate = fromDate.Date.AddDays(1).Add(procEndTime)
            End If
        Else
            toDate = New Date(fromDate.Year, fromDate.Month, fromDate.Day, 23, 59, 59)
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetEndDateRange", "", Nothing, _
                  "RET:" & toDate.ToString(CultureInfo.CurrentCulture()))
        Return toDate

    End Function


    ''' <summary>
    ''' 指定範囲内の予約情報の取得
    ''' ToDateを指定しない場合に本メソッドでToDateを確定する
    ''' </summary>
    ''' <param name="reserveList">予約情報</param>
    ''' <param name="processList">実績情報</param>
    ''' <param name="stallID">ストールID</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="fromDate">範囲時間(FROM)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetReserveList(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                   ByVal processList As SC3150101DataSet.SC3150101StallProcessListDataTable, _
                                   ByVal stallId As Decimal, _
                                   ByVal reserveId As Decimal, _
                                   ByVal fromDate As Date, _
                                   ByVal isBreak As Boolean) As SC3150101DataSet.SC3150101StallReserveListDataTable

        'Logger.Info("[S]GetReserveList()")
        OutputLog(LOG_TYPE_INFO, "[S]GetReserveList", "", Nothing, _
                  "REZ_INFO:(DataSet)", "PROC_INFO:(DataSet)", _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "FROM_DATE:" & fromDate.ToString(CultureInfo.CurrentCulture()))

        ' 引数チェック
        If reserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetReserveList", "", Nothing)
            Return (reserveList)
        End If

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow

        For Each reserveItem In reserveList.Rows

            reserveItem.Movable = "1"
            If CType(reserveItem.REZ_RECEPTION, Integer) = 0 Then
                'If reserveItem.RezStatus = 1 Then
                If reserveItem.STATUS = SMB_StatusFormallyReserved Then
                    reserveItem.Movable = "0"
                End If
            Else
                If IsDBNull(reserveItem.Item("REZ_PICK_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_PICK_DATE = Date.MinValue.ToString("yyyyMMddHHmm", _
                                                                       CultureInfo.CurrentCulture())
                End If
                If IsDBNull(reserveItem.Item("REZ_DELI_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_DELI_DATE = Date.MinValue.ToString("yyyyMMddHHmm", _
                                                                       CultureInfo.CurrentCulture())
                End If
                If reserveItem.STARTTIME < Date.ParseExact(reserveItem.REZ_PICK_DATE, "yyyyMMddHHmm", Nothing) _
                    Or reserveItem.ENDTIME > Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", Nothing) Then
                    reserveItem.Movable = "0"
                End If
            End If
            ' 次世代で追加
            If isBreak Then
                reserveItem.InBreak = "1"
            Else
                reserveItem.InBreak = "0"
            End If
        Next

        ' DBNullの実績データにデフォルト値をセットする
        processList = SetStallProcessListDefaultValue(processList)

        Dim processItem As SC3150101DataSet.SC3150101StallProcessListRow
        Dim drRezList() As SC3150101DataSet.SC3150101StallReserveListRow
        For Each processItem In processList.Rows

            'drRezList = reserveList.Select("REZID = " & processItem.REZID)
            drRezList = CType(reserveList.Select("REZID = " & processItem.REZID),  _
                              SC3150101DataSet.SC3150101StallReserveListRow())
            'RezItem = _ReserveList.Item(processItem.REZID)

            drRezList(0).ProcStatus = processItem.RESULT_STATUS
            If CType(drRezList(0).ProcStatus, Integer) >= CType(SMB_ResultStatusWorking, Integer) Then
                drRezList(0).STARTTIME = Date.ParseExact(processItem.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).ENDTIME = Date.ParseExact(processItem.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).Movable = "0"
            End If

        Next

        'Logger.Info("[E]GetReserveList()")
        OutputLog(LOG_TYPE_INFO, "[E]GetReserveList", "", Nothing, _
                  "RET:" & CType(reserveList.Count, String))
        Return (reserveList)

    End Function


    ''' <summary>
    ''' 作業時間の計算
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="endTime">作業終了日時</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>実作業時間</returns>
    ''' <remarks></remarks>
    Public Function CalculateWorkTime(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                      ByVal startTime As Date, _
                                      ByVal endTime As Date, _
                                      ByVal isBreak As Boolean) As Integer

        'Logger.Info("[S]calculateWorkTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CalculateWorkTime", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "BREAK_FLG:" & CType(isBreak, String))

        Dim workTime As Integer
        Dim breakTime As Integer
        Dim breakStartTime As Date
        Dim breakEndTime As Date

        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        'workTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)
        workTime = CType(endTime.Subtract(startTime).Minutes, Integer)
        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ' 引数チェック
        If breakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]CalculateWorkTime", "", Nothing, _
                  "RET:" & CType(workTime, String))
            Return workTime
        End If

        If isBreak = True Then

            Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
            For Each breakItem In breakList.Rows 'For i As Integer = 1 To breakList.Count

                'Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
                'breakItem = CType(breakList.Rows(i - 1), SC3150101DataSet.SC3150101StallBreakInfoRow)

                breakStartTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                              CultureInfo.CurrentCulture()) & _
                                                          breakItem.STARTTIME)
                breakEndTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                            CultureInfo.CurrentCulture()) & _
                                                        breakItem.ENDTIME)

                If breakStartTime >= endTime Then
                    Exit For
                End If

                If breakEndTime > startTime Then
                    If breakStartTime <= startTime Then
                        If breakEndTime <= endTime Then
                            breakTime = CType(breakEndTime.Subtract(startTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)
                        End If
                    Else
                        If breakEndTime <= endTime Then
                            breakTime = CType(breakEndTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        End If
                    End If
                    workTime = workTime - breakTime

                End If
            Next
        End If


        'Logger.Info("[E]calculateWorkTime()")
        OutputLog(LOG_TYPE_INFO, "[E]CalculateWorkTime", "", Nothing, _
                  "RET:" & CType(workTime, String))
        Return workTime

    End Function


    ''' <summary>
    ''' 作業開始日時の計算
    ''' (規約により参照型引数が使えないので一旦Date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="endTime">作業終了日時</param>
    ''' <param name="workTime">作業予定時間</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>作業開始日時</returns>
    ''' <remarks></remarks>
    Public Function CalculateStartTime(ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                       ByVal endTime As Date, _
                                       ByVal workTime As Integer, _
                                       ByVal isBreak As Boolean) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]CalculateStartTime", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String), "BREAK_FLG:" & CType(isBreak, String))


        Dim dateArray(START_TIME_ARRAY_NUMBER) As Date
        Dim startTime As Date
        Dim breakTime As Integer
        Dim breakStartTime As Date
        Dim breakEndTime As Date
        Dim drBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoRow

        startTime = endTime.AddMinutes(workTime * -1)

        ' 引数チェック
        If breakInfo Is Nothing Then
            dateArray(START_TIME_START) = startTime
            dateArray(START_TIME_END) = endTime

            OutputLog(LOG_TYPE_INFO, "[E]CalculateStartTime", "", Nothing)
            Return dateArray
        End If

        If isBreak = True Then
            For Each drBreakInfo In breakInfo.Rows
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.STARTTIME).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.ENDTIME).Trim())
                If breakEndTime <= startTime Then
                    Exit For
                End If
                If breakStartTime < endTime Then
                    If breakEndTime > endTime Then
                        breakTime = CType(breakStartTime.Subtract(endTime).TotalMinutes, Integer)
                        endTime = breakStartTime
                    Else
                        breakTime = CType(breakStartTime.Subtract(breakEndTime).TotalMinutes, Integer)
                    End If
                    startTime = startTime.AddMinutes(breakTime)
                End If
            Next
        End If

        dateArray(START_TIME_START) = startTime
        dateArray(START_TIME_END) = endTime

        OutputLog(LOG_TYPE_INFO, "[E]CalculateStartTime", "", Nothing)
        Return dateArray

    End Function


    ''' <summary>
    ''' 作業終了時間の計算
    ''' (規約により参照型引数が使えないので一旦Date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>実作業時間</returns>
    ''' <remarks></remarks>
    Public Function CalculateEndTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                     ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal stallId As Decimal, _
                                     ByVal startTime As Date, _
                                     ByVal workTime As Integer, _
                                     ByVal isBreak As Boolean) As Date()

        'Logger.Info("[S]calculateEndTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CalculateEndTime", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "CLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String), "BREAK_FLG:" & CType(isBreak, String))

        Dim dateArray(END_TIME_ARRAY_NUMBER) As Date
        Dim endTime As Date
        'Dim breakTime As Integer


        'Try
        'Call ConvertToStallDateTime(startTime, stallDate, stallTime)
        Dim stallDateTime(STALL_DATE_ARRAY_NUMBER) As Date
        stallDateTime = ConvertToStallDateTime(stallTimeInfo, startTime)
        Dim stallDate As Date
        Dim stallTime As Date
        'Dim stallDate As Date = ConvertToStallDate(stallTimeInfo, startTime)
        'Dim stallTime As Date = ConvertToStallTime(stallTimeInfo, startTime)
        stallDate = stallDateTime(STALL_START_DATE)
        stallTime = stallDateTime(STALL_START_TIME)

        Dim chipDate(WORK_DATE_ARRAY_NUMBER) As Date

        ' 
        chipDate = SimulateChipPutting(stallTimeInfo, _
                                       dealerCode, _
                                       branchCode, _
                                       stallId, _
                                       stallDate, _
                                       stallTime, _
                                       workTime, _
                                       isBreak)

        Dim chipStartDate As Date = chipDate(WORK_START_DATE)
        Dim chipStartTime As Date = chipDate(WORK_START_TIME)
        Dim chipEndTime As Date = chipDate(WORK_END_TIME)

        startTime = chipStartDate.AddTicks(chipStartTime.Ticks())
        endTime = chipStartDate.AddTicks(chipEndTime.Ticks())

        dateArray(END_TIME_END) = endTime
        dateArray(END_TIME_START) = startTime

        OutputLog(LOG_TYPE_INFO, "[E]CalculateEndTime", "", Nothing)
        Return dateArray

    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 作業終了時間をストール稼動終了時間とするか否かを算出する
    ' ''' </summary>
    ' ''' <param name="standardTime">稼動時間外MidFinish基準日時</param>
    ' ''' <returns>作業終了時間をストール稼動終了時間とする場合True、それ以外False</returns>
    ' ''' <remarks></remarks>
    'Public Function IsSetWorkEndTimeToStallEndTime(ByVal standardTime As Date) As Boolean

    '    Dim nowTime As Date
    '    Dim booleanResult As Boolean = False

    '    'Logger.Info("[S]IsSetWorkEndTimeToStallEndTime()")
    '    OutputLog(LOG_TYPE_INFO, "[S]IsSetWorkEndTimeToStallEndTime", "", Nothing, _
    '              "STD_TIME:" & standardTime.ToString(CultureInfo.InvariantCulture()))

    '    '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 START
    '    Dim userContext As StaffContext = StaffContext.Current
    '    nowTime = DateTimeFunc.Now(userContext.DlrCD)
    '    'nowTime = DateTime.Now
    '    '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 END

    '    If standardTime <= nowTime Then
    '        '現在時間が基準時間を超える場合
    '        booleanResult = True
    '    End If

    '    'Logger.Info("[E]IsSetWorkEndTimeToStallEndTime()")
    '    OutputLog(LOG_TYPE_INFO, "[E]IsSetWorkEndTimeToStallEndTime", "", Nothing, _
    '              "RET:" & CType(booleanResult, String))
    '    Return booleanResult

    'End Function


    ' ''' <summary>
    ' ''' 稼動時間外MidFinish基準時間算出
    ' ''' </summary>
    ' ''' <param name="environmentSettingInfo">環境設定情報</param>
    ' ''' <param name="startTime">作業開始日時</param>
    ' ''' <param name="stallStartTime">ストール稼動開始時間</param>
    ' ''' <param name="stallEndTime">ストール稼動終了時間</param>
    ' ''' <returns>稼動時間外MidFinish基準日時</returns>
    ' ''' <remarks></remarks>
    'Public Function CalculateMidFinishStandardTime(ByVal environmentSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable, _
    '                                               ByVal startTime As Date, _
    '                                               ByVal stallStartTime As TimeSpan, _
    '                                               ByVal stallEndTime As TimeSpan) As Date

    '    'Logger.Info("[S]CalculateMidFinishStandardTime()")
    '    OutputLog(LOG_TYPE_INFO, "[S]CalculateMidFinishStandardTime", "", Nothing, _
    '              "ENV_SET_INFO:(DataSet)", _
    '              "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
    '              "STALL_START_TIME:" & stallStartTime.ToString(), _
    '              "STALL_END_TIME:" & stallEndTime.ToString())


    '    Dim midFinishStandardTime As Date
    '    Dim standardTimeAdjustHour As Integer
    '    Dim drEnvSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
    '    Dim standardTimeAdjust As String

    '    If environmentSettingInfo Is Nothing Then
    '        standardTimeAdjustHour = 0
    '    Else
    '        '調整時間(時)取得
    '        'Dim drEnvSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
    '        drEnvSettingInfo = CType(environmentSettingInfo.Rows(0),  _
    '                                 SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow)
    '        'Dim standardTimeAdjust As String
    '        standardTimeAdjust = drEnvSettingInfo.PARAMVALUE

    '        'If standardTimeAdjust.Trim() = "" Then
    '        If String.IsNullOrWhiteSpace(standardTimeAdjust) Then
    '            standardTimeAdjustHour = 0
    '        ElseIf Not IsNumeric(standardTimeAdjust) Then
    '            standardTimeAdjustHour = 0
    '        Else
    '            standardTimeAdjustHour = CType(standardTimeAdjust, Integer) * 1
    '        End If
    '    End If

    '    '基準時間算出
    '    If stallStartTime.TotalMinutes < stallEndTime.TotalMinutes Then
    '        '稼動終了時間<0:00の場合
    '        '基準時間は当日24:00
    '        midFinishStandardTime = startTime.AddDays(1)
    '    Else
    '        '稼動終了時間>=0:00の場合
    '        midFinishStandardTime = startTime.AddDays(1).AddMinutes(stallStartTime.TotalMinutes).AddHours(standardTimeAdjustHour * -1)

    '        If midFinishStandardTime < startTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes) Then
    '            midFinishStandardTime = startTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes)
    '        End If
    '    End If

    '    'Logger.Info("[E]CalculateMidFinishStandardTime()")
    '    OutputLog(LOG_TYPE_INFO, "[E]CalculateMidFinishStandardTime", "", Nothing, _
    '              "RET:" & midFinishStandardTime.ToString(CultureInfo.CurrentCulture()))
    '    Return midFinishStandardTime

    'End Function



    ' ''' <summary>
    ' ''' 作業開始後に日跨ぎであるか否か
    ' ''' </summary>
    ' ''' <param name="startTime">実績開始時間</param>
    ' ''' <param name="endTime">実績開始時間から算出した終了日時</param>
    ' ''' <param name="operationStartTime">稼動開始時間</param>
    ' ''' <param name="operationEndTime">稼動終了時間</param>
    ' ''' <returns>日跨ぎ：true、非日跨ぎ：false</returns>
    ' ''' <remarks></remarks>
    'Private Function IsStartAfterIsHimatagi(ByVal startTime As Date, _
    '                                        ByVal endTime As Date, _
    '                                        ByVal operationStartTime As TimeSpan, _
    '                                        ByVal operationEndTime As TimeSpan) As Boolean

    '    OutputLog(LOG_TYPE_INFO, "[S]IsStartAfterIsHimatagi", "", Nothing, _
    '              "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
    '              "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()), _
    '              "KADO_START_TIME:" & operationStartTime.ToString(), _
    '              "KADO_END_TIME:" & operationEndTime.ToString())

    '    Dim afterStartIsHimatagi As Boolean
    '    afterStartIsHimatagi = False


    '    ' 開始後、日跨ぎ
    '    If operationStartTime.TotalMinutes < operationEndTime.TotalMinutes Then
    '        ' 稼動終了時間<00:00
    '        If endTime > startTime.Date.AddMinutes(operationEndTime.TotalMinutes) Then
    '            afterStartIsHimatagi = True
    '        End If
    '    Else
    '        ' 稼動終了時間>=00:00
    '        If TimeSpan.op_GreaterThan(operationEndTime, startTime.TimeOfDay) Then
    '            If endTime > startTime.Date.AddMinutes(operationEndTime.TotalMinutes) Then
    '                afterStartIsHimatagi = True
    '            End If
    '        Else
    '            If endTime > startTime.Date.AddDays(1).AddMinutes(operationEndTime.TotalMinutes) Then
    '                afterStartIsHimatagi = True
    '            End If
    '        End If
    '    End If

    '    OutputLog(LOG_TYPE_INFO, "[E]IsStartAfterIsHimatagi", "", Nothing, _
    '              "RET:" & CType(afterStartIsHimatagi, String))
    '    Return afterStartIsHimatagi

    'End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 休憩取得有無判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="isBreak">I/Fからの休憩取得フラグ</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <param name="workTime">作業予定時間</param>
    ''' <returns>休憩取得有無</returns>
    ''' <remarks></remarks>
    Public Function CheckBreak(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                               ByVal isBreak As Boolean, _
                               ByVal startTime As Date, _
                               ByVal endTime As Date, _
                               ByVal workTime As Integer) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]CheckBreak", "", Nothing, _
                  "BREAK_INFO:(DataSet)", "BREAK_FLG:" & CType(isBreak, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String))

        Dim iBreak As Boolean

        If isBreak = True Then
            iBreak = True
        ElseIf isBreak = False Then
            iBreak = False
        ElseIf IsBreakTime(breakList, startTime, endTime) = False Then
            iBreak = True
        ElseIf startTime.AddMinutes(workTime) = endTime Then
            iBreak = False
        Else
            iBreak = True
        End If

        OutputLog(LOG_TYPE_INFO, "[E]CheckBreak", "", Nothing, _
                  "RET:" & CType(iBreak, String))
        Return iBreak
    End Function


    ''' <summary>
    ''' 休憩時間にかかるかどうかの判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <returns>休憩にかかる場合、True</returns>
    ''' <remarks></remarks>
    Private Function IsBreakTime(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                ByVal startTime As Date, _
                                ByVal endTime As Date) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsBreakTime", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
        Dim breakStartTime As Date
        Dim breakEndTime As Date

        For Each breakItem In breakList.Rows

            'If breakItem.STARTTIME < endTime Then _
            '    And breakItem.ENDTIME > startTime Then
            breakStartTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                          CultureInfo.InvariantCulture()) & _
                                                      breakItem.STARTTIME.Trim())
            breakEndTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                        CultureInfo.InvariantCulture()) & _
                                                    breakItem.ENDTIME.Trim())

            If breakStartTime < endTime And breakEndTime > startTime Then
                OutputLog(LOG_TYPE_INFO, "[E]suspendWork", "", Nothing, _
                          "RET:" & CType(True, String))
                Return True
            End If
            'If breakItem.STARTTIME < endTime.ToString("HHmm") Then _
            'And breakItem.ENDTIME > startTime.ToString("HHmm") Then
            '    Return True
            'End If

        Next

        OutputLog(LOG_TYPE_INFO, "[E]IsBreakTime", "", Nothing, _
                  "RET:" & CType(False, String))
        Return False

    End Function


    ''' <summary>
    ''' 衝突有無判定
    ''' </summary>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="StartTime">開始日時</param>
    ''' <param name="EndTime">終了日時</param>
    ''' <returns>衝突が発生する場合、True</returns>
    ''' <remarks></remarks>
    Public Function IsCollision(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                ByVal reserveId As Decimal, _
                                ByVal startTime As Date, _
                                ByVal endTime As Date) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsCollision", "", Nothing, _
                  "REZ_INFO:(DataSet)", "REZID:" & CType(reserveId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If reserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]IsCollision", "", Nothing, _
                  "RET:" & CType(False, String))
            Return False
        End If

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow

        For Each reserveItem In reserveList.Rows

            'If (reserveItem.REZID <> reserveId Or (reserveItem.CANCELFLG = "1" And reserveItem.STOPFLG = "1")) _
            If (reserveItem.REZID <> reserveId _
                Or (reserveItem.CANCELFLG.Equals("1") And reserveItem.STOPFLG.Equals("1"))) _
                And (reserveItem.STARTTIME < endTime) _
                And (reserveItem.ENDTIME > startTime) Then

                OutputLog(LOG_TYPE_INFO, "[E]IsCollision", "", Nothing, _
                          "RET:" & CType(True, String), _
                          "REZID" & reserveItem.REZID, _
                          "STARTTIME" & reserveItem.STARTTIME, _
                          "ENDTIME" & reserveItem.ENDTIME)
                Return True

            End If

        Next

        OutputLog(LOG_TYPE_INFO, "[E]IsCollision", "", Nothing, _
                  "RET:" & CType(False, String))
        Return False

    End Function


    ''' <summary>
    ''' 指定された日時からストール日とストール時刻に変換する
    ''' 例えば
    '''   稼働時間 02:00/23:00 の場合
    '''     2011-11-30 03:00 → 2011-11-30 03:00
    '''   稼働時間 09:00/04:00 の場合
    '''     2011-11-30 03:00 → 2011-11-29 27:00
    ''' (規約により参照型引数が使えないので一旦date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="sourceDateTime">処理対象日</param>
    ''' <returns>ストール日, ストール時刻</returns>
    ''' <remarks></remarks>
    Public Function ConvertToStallDateTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal sourceDateTime As Date) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]ConvertToStallDateTime", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", _
                  "TARGET_DATE:" & sourceDateTime.ToString(CultureInfo.InvariantCulture()))

        Dim prevDayAvailableEnd As Date
        Dim retDateArray(STALL_DATE_ARRAY_NUMBER) As Date
        Dim stallDate As Date
        Dim stallTime As Date

        stallDate = New DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day, 0, 0, 0)
        stallTime = New DateTime(1, 1, 1, sourceDateTime.Hour, sourceDateTime.Minute, sourceDateTime.Second)

        Dim availableEndTimeTicks As Long
        availableEndTimeTicks = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        prevDayAvailableEnd = stallDate.AddDays(-1)
        'prevDayAvailableEnd = prevDayAvailableEnd.AddTicks(GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()) 'PROG:0,RES:1
        prevDayAvailableEnd = prevDayAvailableEnd.AddTicks(availableEndTimeTicks) 'PROG:0,RES:1

        If sourceDateTime < prevDayAvailableEnd Then
            'srcDateTimeが前日稼働時間内の場合、ストール用日時に調整
            stallDate = stallDate.AddDays(-1)
            stallTime = stallTime.AddDays(1)
        End If

        retDateArray(STALL_START_DATE) = stallDate
        retDateArray(STALL_START_TIME) = stallTime

        OutputLog(LOG_TYPE_INFO, "[E]ConvertToStallDateTime", "", Nothing)
        Return retDateArray

    End Function


    ''' <summary>
    ''' チップを配置した場合のチップ開始時刻・チップ終了時刻を返却する
    ''' (規約により参照型引数が使えないので一旦string型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDate">処理対象日</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="workTimeMinutes">作業時間</param>
    ''' <param name="isBreak">休憩取得するか否か</param>
    ''' <returns>作業日時配列</returns>
    ''' <remarks></remarks>
    Public Function SimulateChipPutting(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                        ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal stallId As Decimal, _
                                        ByVal targetDate As Date, _
                                        ByVal startTime As Date, _
                                        ByVal workTimeMinutes As Integer, _
                                        ByVal isBreak As Boolean) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]SimulateChipPutting", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "DLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "TARGET_DATE:" & CType(targetDate, String), _
                  "START_TIME:" & CType(startTime, String), _
                  "WORK_TIME:" & CType(workTimeMinutes, String), _
                  "BREAK_FLG:" & CType(isBreak, String))

        Dim chipStartDate As Date
        Dim chipStartTime As Date
        Dim chipEndTime As Date
        Dim dateArray(WORK_DATE_ARRAY_NUMBER) As Date


        '最初から開始時刻が稼動時間外の場合、そのまま返す
        If startTime >= GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS) Then
            chipStartDate = targetDate
            chipStartTime = startTime
        End If

        '後で正規化するので、広いほう(tbl_stalltime.pstarttime, pendtime)で取得
        Dim targetDayStart As Date
        targetDayStart = targetDate.AddTicks(GetAvailableStartTime(stallTimeInfo, _
                                                                   OPERATION_TIME_PROGRESS).Ticks())
        Dim targetDayEnd As Date
        targetDayEnd = targetDate.AddTicks(GetAvailableEndTime(stallTimeInfo, _
                                                               OPERATION_TIME_PROGRESS).Ticks())

        '当日の休憩+Unavailableチップのリスト取得
        'Dim stallBreakListTemp As SC3150101DataSet.SC3150101StallBreakListDataTable
        Dim stallBreakListTemp As New SC3150101DataSet.SC3150101StallBreakListDataTable '20120202
        Dim stallBreakList As New SC3150101DataSet.SC3150101StallBreakListDataTable
        If isBreak Then
            stallBreakListTemp = GetStallBreakList(stallTimeInfo, dealerCode, branchCode, _
                                                   stallId, targetDayStart, targetDayEnd)
            '初日のみ、tbl_stalltime.pstarttime ～ tbl_stalltime.endtime内の休憩を取得 (2011-11時点の仕様)
            'stallBreakList = CType(stallBreakListTemp.Clone, SC3150101DataSet.SC3150101StallBreakListDataTable)
            Dim availableStartTimeA As Date
            Dim availableEndTimeA As Date
            availableStartTimeA = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
            availableEndTimeA = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
            stallBreakList = Normalize(stallBreakListTemp, _
                                       stallBreakList, _
                                       availableStartTimeA, _
                                       availableEndTimeA)

        Else
            stallBreakList = Nothing
        End If

        '■1 開始時刻補正
        Dim tempStartTime As Date
        Dim drStallBreakList As SC3150101DataSet.SC3150101StallBreakListRow
        If isBreak Then
            drStallBreakList = GetOverlapBreak(stallBreakList, startTime)
            If drStallBreakList Is Nothing Then
                '開始時刻が休憩にかからない場合
                chipStartDate = targetDate
                chipStartTime = startTime
            Else
                '開始時刻が休憩にかかる場合、開始時刻を休憩終了時刻にずらす
                'tempStartTime = HHMMTextToDateTime(StringValueOfDB(drStallBreakList.ENDTIME).Trim())
                tempStartTime = drStallBreakList.ENDTIME
                If GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS) <= tempStartTime Then
                    ' 対象日後の稼働日を取得
                    Dim dateAndCountStart(TARGET_DATE_ARRAY_NUMBER) As String
                    dateAndCountStart = GetNextWorkDate(dealerCode, branchCode, stallId, targetDate, 0)
                    Dim targetDateString As String
                    targetDateString = dateAndCountStart(TARGET_DATE_DATE)
                    Dim AvailableStartTime As Date
                    AvailableStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                    Dim targetDateTemp As Date
                    targetDateTemp = Date.ParseExact(targetDateString, "yyyyMMdd", Nothing)
                    '開始時刻が稼動終了以降になった場合、開始時刻を翌稼働日の稼動開始時刻とする
                    dateArray = SimulateChipPutting(stallTimeInfo, _
                                                    dealerCode, _
                                                    branchCode, _
                                                    stallId, _
                                                    targetDateTemp, _
                                                    AvailableStartTime, _
                                                    workTimeMinutes, _
                                                    isBreak)
                    Return dateArray
                Else
                    chipStartDate = targetDate
                    chipStartTime = tempStartTime
                End If
            End If
        Else
            chipStartDate = targetDate
            chipStartTime = startTime
        End If
        ' ■1 開始時刻補正完了

        ' ■2 終了時刻算
        chipEndTime = GetEndTimeAfterRevison(stallTimeInfo, stallBreakListTemp, stallBreakList, _
                                             dealerCode, branchCode, stallId, workTimeMinutes, _
                                             chipStartDate, chipStartTime, isBreak)

        dateArray(WORK_START_DATE) = chipStartDate
        dateArray(WORK_START_TIME) = chipStartTime
        dateArray(WORK_END_TIME) = chipEndTime

        ' 解放
        If stallBreakListTemp IsNot Nothing Then
            stallBreakListTemp.Dispose()
            stallBreakListTemp = Nothing
        End If
        If stallBreakList IsNot Nothing Then
            stallBreakList.Dispose()
            stallBreakList = Nothing
        End If

        OutputLog(LOG_TYPE_INFO, "[E]SimulateChipPutting", "", Nothing)
        Return dateArray

    End Function


    ''' <summary>
    ''' 開始時刻補正後の終了時刻を算出する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="stallBreakListTempSource">休憩情報</param>
    ''' <param name="stallBreakListSource">正規化した休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workTimeMinutes">作業時間</param>
    ''' <param name="chipStartDate">開始日</param>
    ''' <param name="chipStartTime">開始時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>終了時刻</returns>
    ''' <remarks></remarks>
    Private Function GetEndTimeAfterRevison(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                            ByVal stallBreakListTempSource As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                            ByVal stallBreakListSource As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                            ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Decimal, _
                                            ByVal workTimeMinutes As Integer, _
                                            ByVal chipStartDate As Date, _
                                            ByVal chipStartTime As Date, _
                                            ByVal isBreak As Boolean) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetEndTimeAfterRevison", "", Nothing)

        Dim chipEndTime As Date

        '■2 終了時刻算出
        Dim totalEndTime As Date            ' 仮終了時刻(作業開始時刻からの通算)
        Dim tempDate As Date                ' 処理対象日
        Dim tempDateStartTime As Date       ' 対象日における開始時刻(初日は作業開始時刻、翌日以降は稼動開始時刻)
        'Dim tempDateEndTime As Date         ' 対象日における終了時刻(日跨ぎの場合、最終日以外は稼動終了時刻)
        Dim unavailableDaysCount As Integer ' 跨いだ連続非稼動日数
        'Dim tempTime As Date                ' TEMP用
        totalEndTime = chipStartTime.AddMinutes(workTimeMinutes)
        tempDate = chipStartDate
        tempDateStartTime = chipStartTime

        Dim stallBreakListTemp As SC3150101DataSet.SC3150101StallBreakListDataTable
        If stallBreakListTempSource IsNot Nothing Then
            stallBreakListTemp = CType(stallBreakListTempSource.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)
        Else
            stallBreakListTemp = Nothing
        End If
        Dim stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable
        If stallBreakListSource IsNot Nothing Then
            stallBreakList = CType(stallBreakListSource.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)
        Else
            stallBreakList = Nothing
        End If

        ' 後で正規化するので、広いほう(tbl_stalltime.pstarttime, pendtime)で取得
        Dim availableStartTimeTicks As Long
        availableStartTimeTicks = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        Dim availableEndTimeTicks As Long
        availableEndTimeTicks = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        Dim targetDayStart As Date
        targetDayStart = tempDate.AddTicks(availableStartTimeTicks)
        Dim targetDayEnd As Date
        targetDayEnd = tempDate.AddTicks(availableEndTimeTicks)

        Dim outerLoop As Integer
        'Dim intInnerLoop As Integer

        outerLoop = 0
        Do
            If isBreak Then

                '初回は開始時間補正時に取得している
                If stallBreakListTemp Is Nothing Then
                    'stallBreakListTemp = Nothing
                    stallBreakListTemp = GetStallBreakList(stallTimeInfo, _
                                                           dealerCode, branchCode, _
                                                           stallId, targetDayStart, targetDayEnd)

                    Dim availableStartTime As Date
                    Dim availableEndTime As Date
                    availableStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_RESERVE)
                    availableEndTime = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
                    '2日目以降は、tbl_stalltime.starttime ～ tbl_stalltime.endtime内の休憩を取得 (2011-11時点の仕様)
                    stallBreakList = Normalize(stallBreakListTemp, _
                                               stallBreakList, _
                                               availableStartTime, _
                                               availableEndTime)
                End If

                'チップと重なる休憩の合計時間を加算
                totalEndTime = GetTotalEndTime(stallBreakList, tempDateStartTime, totalEndTime)
            End If

            Dim endTimeTemp As Date
            endTimeTemp = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
            If tempDate.AddTicks(endTimeTemp.Ticks()) < chipStartDate.AddTicks(totalEndTime.Ticks()) Then
                '翌日以降に日跨いでいる場合（(開始日+仮終了時刻) > (対象日+稼動終了時刻)）

                '仮終了時刻 = 仮終了時刻 + (非稼働時間 * 1日分)
                Dim unavailableTime As TimeSpan

                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                'unavailableTime = GetUnavailableTimeSpan(stallTimeInfo, OPERATION_TIME_RESERVE)
                unavailableTime = GetUnavailableTimeSpan(stallTimeInfo, OPERATION_TIME_PROGRESS)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                totalEndTime = totalEndTime.Add(unavailableTime)

                '翌稼働日を取得
                Dim dateAndCountEnd(TARGET_DATE_ARRAY_NUMBER) As String

                unavailableDaysCount = 0
                dateAndCountEnd = GetNextWorkDate(dealerCode, branchCode, stallId, tempDate, _
                                               unavailableDaysCount)
                unavailableDaysCount = CType(dateAndCountEnd(TARGET_DATE_COUNT), Integer) ' 非稼働日数

                '処理対象日 = 処理対象日 + 1
                tempDate = tempDate.AddDays(1)

                '(24h * 非稼動日数分)を加算
                totalEndTime = totalEndTime.AddDays(unavailableDaysCount)
                tempDate = tempDate.AddDays(unavailableDaysCount)

            Else
                chipEndTime = totalEndTime
                Exit Do
            End If

            If stallBreakList IsNot Nothing Then
                stallBreakList.Clear()
            End If
            stallBreakListTemp = Nothing
            tempDateStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_RESERVE) '2日目以降は開始時刻=稼動開始時刻

            '無限ループよけ (60日跨ぎ)
            outerLoop = outerLoop + 1
            If outerLoop > 60 Then
                OutputLog(LOG_TYPE_INFO, "[E]GetEndTimeAfterRevison", "infinite loop", Nothing)
                Throw New ApplicationException("Infinite loop occurred by GetEndTimeAfterRevison() function of SC3150101BusinessLogic")
            End If
        Loop
        '■2 終了時刻算出完了

        OutputLog(LOG_TYPE_INFO, "[E]GetEndTimeAfterRevison", "", Nothing)
        Return chipEndTime

    End Function


    ''' <summary>
    ''' 休憩時間を考慮した終了時刻を算出する
    ''' </summary>
    ''' <param name="stallBreakListSoruce">正規化した休憩情報</param>
    ''' <param name="tempDateStartTime">開始時間</param>
    ''' <param name="totalEndTimeSoruce">終了時間</param>
    ''' <returns>休憩時間を考慮した終了時間</returns>
    ''' <remarks></remarks>
    Private Function GetTotalEndTime(ByVal stallBreakListSoruce As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                     ByVal tempDateStartTime As Date, _
                                     ByVal totalEndTimeSoruce As Date) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetTotalEndTime", "", Nothing)

        Dim tempDateEndTime As Date ' 対象日における終了時刻(日跨ぎの場合、最終日以外は稼動終了時刻)
        Dim totalEndTime As Date = totalEndTimeSoruce
        Dim tempTime As Date = tempDateStartTime ' 一時格納用
        Dim stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable
        Dim drStallBreakList As SC3150101DataSet.SC3150101StallBreakListRow
        stallBreakList = CType(stallBreakListSoruce.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)

        Dim i As Integer = 0 ' ループカウンタ
        Do
            drStallBreakList = GetNextBreak(stallBreakList, tempTime)
            If drStallBreakList Is Nothing Then
                Exit Do
            Else
                tempDateEndTime = New DateTime(1, 1, 1, totalEndTime.Hour, totalEndTime.Minute, 0)
                If tempDateEndTime < tempDateStartTime Then
                    tempDateEndTime = tempDateEndTime.AddDays(1)
                End If

                If drStallBreakList.STARTTIME < tempDateEndTime Then
                    ' 休憩開始時刻 < 処理対象日の仮終了時刻 場合

                    ' 休憩時間
                    Dim breakTime As TimeSpan
                    breakTime = DateTime.op_Subtraction(drStallBreakList.ENDTIME, drStallBreakList.STARTTIME)

                    ' 仮終了時刻 = 仮終了時刻 + 休憩時間
                    totalEndTime = totalEndTime.Add(breakTime)
                    tempTime = drStallBreakList.ENDTIME
                Else
                    Exit Do
                End If
            End If

            '無限ループよけ (休憩最大5 + 1日分のUnavailableチップ数)
            i = i + 1
            If i > 60 Then
                OutputLog(LOG_TYPE_INFO, "[E]GetTotalEndTime", "infinite loop", Nothing)
                Throw New ApplicationException("Infinite loop occurred by GetTotalEndTime() function of SC3150101BusinessLogic")
            End If
        Loop

        OutputLog(LOG_TYPE_INFO, "[E]GetTotalEndTime", "", Nothing)
        Return totalEndTime

    End Function


    ''' <summary>
    ''' 処理対象日のUnavailableチップおよび休憩のリストを返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDayStart">対象開始時間</param>
    ''' <param name="targetDayEnd">対象終了時間</param>
    ''' <returns>使用不可チップ、休憩リスト</returns>
    ''' <remarks></remarks>
    Private Function GetStallBreakList(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Decimal, _
                                       ByVal targetDayStart As Date, _
                                       ByVal targetDayEnd As Date) As SC3150101DataSet.SC3150101StallBreakListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetStallBreakList", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "DLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "TARGET_S_DATE:" & targetDayStart.ToString(CultureInfo.InvariantCulture()), _
                  "TARGET_E_DATE:" & targetDayEnd.ToString(CultureInfo.InvariantCulture()))


        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '当日の休憩+Unavailableチップのリスト取得
            Dim unavailableChipInfo As SC3150101DataSet.SC3150101UnavailableChipListDataTable
            unavailableChipInfo = adapter.GetUnavailableList(stallId, _
                                                             targetDayStart, _
                                                             targetDayEnd)

            Dim breakStartDate As Date
            Dim breakEndDate As Date
            Dim breakStartTime As Date
            Dim breakEndTime As Date
            Dim breakList As New SC3150101DataSet.SC3150101StallBreakListDataTable
            Dim drBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
            Dim drUnavailableChipItem As SC3150101DataSet.SC3150101UnavailableChipListRow

            'drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
            'drBreakItem = CType(breakList.Rows(), SC3150101DataSet.SC3150101StallBreakListRow)

            For Each drUnavailableChipItem In unavailableChipInfo.Rows

                breakStartDate = YYYYMMDDTextToDateTime(StringValueOfDB(drUnavailableChipItem.STARTTIME_DAY).Trim())
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drUnavailableChipItem.STARTTIME_TIME).Trim())
                breakEndDate = YYYYMMDDTextToDateTime(StringValueOfDB(drUnavailableChipItem.ENDTIME_DAY).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drUnavailableChipItem.ENDTIME_TIME).Trim())

                'Unavailable開始日時が当日稼動開始時刻より前(日跨ぎ)の場合
                If breakStartDate.AddTicks(breakStartTime.Ticks()) < targetDayStart Then
                    '当日分のみ取得
                    breakStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                End If

                'Unavailable終了日時が当日稼動終了時刻より後の場合
                If breakEndDate.AddTicks(breakEndTime.Ticks()) > targetDayEnd Then
                    '当日分のみ取得
                    breakEndTime = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                End If
                drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
                drBreakItem.STARTTIME = breakStartTime
                drBreakItem.ENDTIME = breakEndTime

                ' データセットに行を追加
                breakList.Rows.Add(drBreakItem)
            Next

            ' 次世代で追加(以下の情報のみで良い気がするが・・・)-------------------------------
            ' ※既存では、休憩情報のみを取得したもの(LoadBreakMaster())とマージしている
            ' 休憩時間帯・使用不可時間帯取得
            Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
            drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
            Dim fromDate As Date
            Dim toDate As Date
            fromDate = targetDayStart.Date.Add(SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay)
            toDate = targetDayEnd.Date.Add(SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay)
            Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable
            breakInfo = adapter.GetBreakSlot(stallId, fromDate, toDate)

            Dim drBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoRow
            For Each drBreakInfo In breakInfo.Rows
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.STARTTIME).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.ENDTIME).Trim())

                drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
                drBreakItem.STARTTIME = breakStartTime
                drBreakItem.ENDTIME = breakEndTime

                ' データセットに行を追加
                breakList.Rows.Add(drBreakItem)
                'breakList.NewRow()
                'breakList.ImportRow(drBreakItem)
            Next
            ' ---------------------------------------------------------------------------------

            OutputLog(LOG_TYPE_INFO, "[E]GetStallBreakList", "", Nothing)
            Return breakList

        End Using

    End Function


    ''' <summary>
    ''' 稼動開始時刻を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="startTimeType">ProgressiveかReservationか</param>
    ''' <returns>稼動開始時刻</returns>
    ''' <remarks></remarks>
    Private Function GetAvailableStartTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal startTimeType As Integer) As DateTime

        OutputLog(LOG_TYPE_INFO, "[S]GetAvailableStartTime", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(startTimeType, String))

        Dim startTime As Date
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow

        'LoadAvailableTime()
        Dim availStartTime As Date  'Progressive稼動開始時刻(日付は持たない)
        Dim availEndTime As Date    'Progressive稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        Dim availStartTimeR As Date 'Reservation稼動開始時刻(日付は持たない)
        Dim availEndTimeR As Date   'Reservation稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        'Dim stallType As Integer    'ストール時間タイプ


        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        availStartTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.STARTTIME.Trim()))
        availEndTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.ENDTIME.Trim()))
        'If StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim()).Equals(String.Empty) Then
        If String.IsNullOrEmpty(StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim())) Then
            'PSTARTTIME, PENDTIMEが未登録の場合、STARTTIME, ENDTIMEを使用
            availStartTime = availStartTimeR
            availEndTime = availEndTimeR
        Else
            availStartTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PSTARTTIME).Trim())
            availEndTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PENDTIME).Trim())
        End If


        If availStartTime > availEndTime Then
            'stallType = TIME_TYPE_OVER24
            availEndTime = availEndTime.AddDays(1)
        Else
            'stallType = TIME_TYPE_NORMAL
        End If

        If availStartTimeR > availEndTimeR Then
            availEndTimeR = availEndTimeR.AddDays(1)
        End If



        If startTimeType = 0 Then
            startTime = availStartTime
        ElseIf startTimeType = 1 Then
            startTime = availStartTimeR
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetAvailableStartTime", "", Nothing, _
                  "RET:" & startTime.ToString(CultureInfo.CurrentCulture()))
        Return startTime
    End Function


    ''' <summary>
    ''' 稼動終了時刻を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="endTimeType">Progressive:0かReservation:1か</param>
    ''' <returns>稼動終了時刻</returns>
    ''' <remarks></remarks>
    Private Function GetAvailableEndTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                         ByVal endTimeType As Integer) As DateTime

        OutputLog(LOG_TYPE_INFO, "[S]GetAvailableEndTime", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(endTimeType, String))

        Dim endTime As Date
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow

        'LoadAvailableTime()
        Dim availStartTime As Date  'Progressive稼動開始時刻(日付は持たない)
        Dim availEndTime As Date    'Progressive稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        Dim availStartTimeR As Date 'Reservation稼動開始時刻(日付は持たない)
        Dim availEndTimeR As Date   'Reservation稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        'Dim stallType As Integer    'ストール時間タイプ

        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        availStartTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.STARTTIME.Trim()))
        availEndTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.ENDTIME.Trim()))
        'If StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim()).Equals(String.Empty) Then
        If String.IsNullOrEmpty(StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim())) Then
            'PSTARTTIME, PENDTIMEが未登録の場合、STARTTIME, ENDTIMEを使用
            availStartTime = availStartTimeR
            availEndTime = availEndTimeR
        Else
            availStartTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PSTARTTIME).Trim())
            availEndTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PENDTIME).Trim())
        End If


        If availStartTime > availEndTime Then
            'stallType = TIME_TYPE_OVER24 'StallTimeTpye.Over24
            availEndTime = availEndTime.AddDays(1)
        Else
            'stallType = TIME_TYPE_NORMAL 'StallTimeTpye.Normal
        End If

        If availStartTimeR > availEndTimeR Then
            availEndTimeR = availEndTimeR.AddDays(1)
        End If

        If endTimeType = 0 Then
            endTime = availEndTime
        ElseIf endTimeType = 1 Then
            endTime = availEndTimeR
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetAvailableEndTime", "", Nothing, _
                  "RET:" & endTime.ToString(CultureInfo.InvariantCulture()))
        Return endTime

    End Function


    ''' <summary>
    ''' 非稼働時間を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="startEndTimeType">ProgressiveかReservationか</param>
    ''' <returns>非稼働時間</returns>
    ''' <remarks></remarks>
    Public Function GetUnavailableTimeSpan(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal startEndTimeType As Integer) As TimeSpan

        OutputLog(LOG_TYPE_INFO, "[S]GetUnavailableTimeSpan", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(startEndTimeType, String))

        Dim notOpetationDate As TimeSpan

        notOpetationDate = DateTime.op_Subtraction(GetAvailableStartTime(stallTimeInfo, _
                                                                         startEndTimeType).AddDays(1), _
                                                   GetAvailableEndTime(stallTimeInfo, _
                                                                       startEndTimeType))

        OutputLog(LOG_TYPE_INFO, "[E]GetUnavailableTimeSpan", "", Nothing, _
                  "RET:" & notOpetationDate.ToString())
        Return notOpetationDate

    End Function


    ''' <summary>
    ''' リスト内の休憩を正規化する
    ''' ・対象時間外の休憩を削除
    ''' ・重複・隣接する休憩を結合
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="retStallBreakList">戻し用休憩情報</param>
    ''' <param name="startTime">正規化対象開始時刻</param>
    ''' <param name="endTime">正規化対象終了時刻</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Normalize(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                               ByVal retStallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                               ByVal startTime As Date, _
                               ByVal endTime As Date) As SC3150101DataSet.SC3150101StallBreakListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]Normalize", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]Normalize", "", Nothing)
            Return retStallBreakList
        End If
        'Dim currBreak As StallBreak
        'Dim newList As New SortedList
        'Dim prevEnd As DateTime
        'Dim prevBreak As StallBreak
        'Dim retStallBreakList As New SC3150101DataSet.SC3150101StallBreakListDataTable

        '稼働時間外を排除する
        '・稼働時間外にはみ出している休憩、稼働時間外の休憩(休憩設定後に稼働時間変更すると存在可能性あり)
        '・稼働時間外にはみ出しているUnavailable、稼働時間外のUnavailable、日跨ぎUnavailable
        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
        For Each stallBreakItem In stallBreakList.Rows

            'unavailableStartTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.STARTTIME_TIME).Trim())
            'unavailableEndTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.ENDTIME_TIME).Trim())

            '開始時刻が稼働時間前の場合
            If stallBreakItem.STARTTIME < startTime Then
                '稼働時間外の分を切り落とす
                stallBreakItem.STARTTIME = startTime
                If stallBreakItem.ENDTIME < startTime Then
                    '完全に稼働時間外の場合は0m扱いとする
                    stallBreakItem.ENDTIME = startTime
                End If
            End If

            '終了時刻が稼働時間後の場合
            If stallBreakItem.ENDTIME > endTime Then
                '稼働時間外の分を切り落とす
                stallBreakItem.ENDTIME = endTime
                If stallBreakItem.STARTTIME > endTime Then
                    '完全に稼働時間外の場合は0m扱いとする
                    stallBreakItem.STARTTIME = endTime
                End If
            End If
        Next stallBreakItem

        '重複する休憩を排除する
        Dim prevEnd As DateTime
        prevEnd = DateTime.MinValue
        For Each stallBreakItem In stallBreakList.Rows

            If TimeSpan.op_Equality(DateTime.op_Subtraction(stallBreakItem.ENDTIME, _
                                                            stallBreakItem.STARTTIME), _
                                    TimeSpan.Zero) <> True Then '0分は無視する
                If stallBreakItem.STARTTIME < prevEnd Then
                    '前休憩の終了時刻より自休憩の開始時刻が前の場合

                    If stallBreakItem.ENDTIME > prevEnd Then
                        '自休憩の終了時刻が前休憩の終了時刻より後の場合、自休憩の開始時刻を前休憩の終了時刻に書換
                        stallBreakItem.STARTTIME = prevEnd
                        prevEnd = stallBreakItem.ENDTIME
                    Else
                        '自休憩の終了時刻が前休憩の終了時刻以前の場合、自休憩を0分に書換
                        stallBreakItem.STARTTIME = stallBreakItem.ENDTIME
                    End If
                Else
                    prevEnd = stallBreakItem.ENDTIME
                End If
            End If
        Next stallBreakItem

        '隣接する休憩を結合する
        Dim prevBreak As SC3150101DataSet.SC3150101StallBreakListRow
        prevBreak = Nothing
        For Each stallBreakItem In stallBreakList.Rows

            If prevBreak Is Nothing Then
                prevBreak = stallBreakItem
            ElseIf TimeSpan.op_Equality(DateTime.op_Subtraction(stallBreakItem.ENDTIME, _
                                                                stallBreakItem.STARTTIME), _
                                        TimeSpan.Zero) <> True Then '0分は無視する
                If DateTime.op_Equality(stallBreakItem.STARTTIME, prevBreak.ENDTIME) Then
                    '自休憩の開始時刻=前休憩の終了時刻の場合
                    prevBreak.ENDTIME = stallBreakItem.ENDTIME
                    stallBreakItem.STARTTIME = stallBreakItem.ENDTIME
                End If

                If prevBreak.ENDTIME < stallBreakItem.ENDTIME Then
                    prevBreak = stallBreakItem
                End If
            End If
        Next stallBreakItem

        For Each stallBreakItem In stallBreakList.Rows

            Dim timeInterval As TimeSpan
            timeInterval = DateTime.op_Subtraction(stallBreakItem.ENDTIME, stallBreakItem.STARTTIME)
            If TimeSpan.op_Equality(timeInterval, TimeSpan.Zero) <> True Then
                '0分ではない休憩のみ追加
                'newList.Add(currBreak.GetStartTime().ToString("ddHHmm") & "0000", currBreak)
                'retStallBreakList.Rows.Add(stallBreakItem)
                retStallBreakList.ImportRow(stallBreakItem)
            End If
        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]Normalize", "", Nothing)
        Return retStallBreakList
    End Function


    ''' <summary>
    ''' 指定時刻を含む休憩を返却する
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <returns>指定時刻を含む休憩、存在しない場合Nothing</returns>
    ''' <remarks></remarks>
    Public Function GetOverlapBreak(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                    ByVal startTime As DateTime) As SC3150101DataSet.SC3150101StallBreakListRow

        OutputLog(LOG_TYPE_INFO, "[S]GetOverlapBreak", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()))

        '引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetOverlapBreak", "", Nothing, "RET:Nothing")
            Return Nothing
        End If

        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow

        '必ず正規化してから呼ぶ
        'Debug.Assert(Me.normalized)

        For Each stallBreakItem In stallBreakList.Rows

            If (stallBreakItem.STARTTIME <= startTime) _
                And (stallBreakItem.ENDTIME >= startTime) Then
                OutputLog("I", "[E]GetOverlapBreak", "", Nothing)
                Return stallBreakItem
            End If
        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]GetOverlapBreak", "", Nothing, "RET:Nothing")
        Return Nothing

    End Function


    ''' <summary>
    ''' 指定時刻以降に開始される休憩を返却する
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="targetTime">対象時間</param>
    ''' <returns>指定時刻以降の休憩、存在しない場合Nothing</returns>
    ''' <remarks></remarks>
    Public Function GetNextBreak(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                 ByVal targetTime As DateTime) As SC3150101DataSet.SC3150101StallBreakListRow

        OutputLog(LOG_TYPE_INFO, "[S]GetNextBreak", "", Nothing, _
                  "BRREAK_INFO:(DataSet)", _
                  "TARGET_TIME:" & targetTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak", "", Nothing, "RET:Nothing")
            Return Nothing
        End If

        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
        'Dim unavailableStatTime As Date
        '必ず正規化してから呼ぶ
        'Debug.Assert(Me.normalized)

        For Each stallBreakItem In stallBreakList.Rows

            'unavailableStatTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.STARTTIME_TIME).Trim())
            If stallBreakItem.STARTTIME >= targetTime Then
                OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak", "", Nothing, _
                          "RET:" & CType(stallBreakItem.ItemArray.Count, String))
                Return stallBreakItem
            End If

        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak", "", Nothing, "RET:Nothing")
        Return Nothing

    End Function


    ''' <summary>
    ''' 処理対象日より後の稼働日を返却する
    ''' (規約により参照型引数が使えないので一旦string型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDate">処理対象日</param>
    ''' <param name="unavailableCount">非稼働日を跨いだ場合その日数</param>
    ''' <returns>稼働日(時刻は持たない)、非稼働日数</returns>
    ''' <remarks></remarks>
    Public Function GetNextWorkDate(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal stallId As Decimal, _
                                    ByVal targetDate As DateTime, _
                                    ByVal unavailableCount As Integer) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetNextWorkDate", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "STALLID:" & CType(stallId, String), _
                  "TARGET_DATE:" & targetDate.ToString(CultureInfo.InvariantCulture()), _
                  "DAY_NUM:" & CType(unavailableCount, String))

        Dim returnArrayValue(TARGET_DATE_ARRAY_NUMBER) As String ' 戻り値
        Dim nextNonworkingDate As DateTime
        Dim tempDate As DateTime
        Dim intLoop As Integer

        Dim dayText As String
        Dim y As Integer
        Dim m As Integer
        Dim d As Integer

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            Dim stallPlanInfo As SC3150101DataSet.SC3150101NextNonworkingDateDataTable
            Dim drStallPlanInfo As SC3150101DataSet.SC3150101NextNonworkingDateRow

            unavailableCount = 0
            tempDate = targetDate

            intLoop = 0
            Do

                stallPlanInfo = adapter.GetNextNonworkingDate(dealerCode, branchCode, _
                                                              stallId, tempDate)

                If IsNothing(stallPlanInfo) Or stallPlanInfo.Count <= 0 Then
                    nextNonworkingDate = DateTime.MinValue
                Else
                    drStallPlanInfo = CType(stallPlanInfo.Rows(0),  _
                                            SC3150101DataSet.SC3150101NextNonworkingDateRow)
                    dayText = drStallPlanInfo.WORKDATE
                    y = Integer.Parse(dayText.Substring(0, 4), CultureInfo.InvariantCulture())
                    m = Integer.Parse(dayText.Substring(4, 2), CultureInfo.InvariantCulture())
                    d = Integer.Parse(dayText.Substring(6, 2), CultureInfo.InvariantCulture())
                    nextNonworkingDate = New DateTime(y, m, d, 0, 0, 0)
                End If

                tempDate = tempDate.AddDays(1)

                '非稼働日が存在しない場合
                If nextNonworkingDate = DateTime.MinValue Then
                    OutputLog(LOG_TYPE_INFO, "[E]GetNextWorkDate", "", Nothing, _
                              "RET:" & tempDate.ToString(CultureInfo.InvariantCulture()))
                    'Return tempDate
                    returnArrayValue(TARGET_DATE_DATE) = tempDate.ToString("yyyyMMdd", _
                                                                           CultureInfo.InvariantCulture())
                    returnArrayValue(TARGET_DATE_COUNT) = CType(unavailableCount, String)
                    Return returnArrayValue
                End If

                '翌日は非稼働日ではない場合
                If nextNonworkingDate <> tempDate Then
                    OutputLog(LOG_TYPE_INFO, "[E]GetNextWorkDate", "", Nothing, _
                              "RET:" & tempDate.ToString(CultureInfo.InvariantCulture()))
                    'Return tempDate
                    returnArrayValue(TARGET_DATE_DATE) = tempDate.ToString("yyyyMMdd", _
                                                                           CultureInfo.InvariantCulture())
                    returnArrayValue(TARGET_DATE_COUNT) = CType(unavailableCount, String)
                    Return returnArrayValue
                End If

                '翌日が非稼働日の場合繰り返す
                unavailableCount = unavailableCount + 1

                '無限ループよけ
                intLoop = intLoop + 1
                If intLoop > 60 Then
                    OutputLog(LOG_TYPE_INFO, "[E]GetNextWorkDate", "infinite loop", Nothing)
                    'Throw New Exception("SC3150101BusinessLogic")
                    Throw New ApplicationException("Infinite loop occurred by GetNextWorkDate() function of SC3150101BusinessLogic")
                End If
            Loop

        End Using

    End Function


    ''' <summary>
    ''' 指定時間への予約の移動
    ''' </summary>
    ''' <param name="reserveList">予約情報リスト</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveID">予約ID</param>
    ''' <param name="stallId">予約ID</param>
    ''' <param name="startTime">開始日時</param>
    ''' <param name="endTime">終了日時</param>
    ''' <returns>予約情報リスト</returns>
    ''' <remarks></remarks>
    Public Function MoveReserve(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                ByVal dealerCode As String, _
                                ByVal branchCode As String, _
                                ByVal reserveId As Decimal, _
                                ByVal stallId As Decimal, _
                                ByVal startTime As DateTime, _
                                ByVal endTime As DateTime) As SC3150101DataSet.SC3150101StallReserveListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserve", "", Nothing, _
                  "REZ_INFO:(DataSet)", "STALL_TIME_INFO:(DataSet)", _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        Dim retReserveList As SC3150101DataSet.SC3150101StallReserveListDataTable

        '_StartPosition = endTime
        retReserveList = MoveReserveSub(reserveList, _
                                        stallTimeInfo, _
                                        breakInfo, _
                                        dealerCode, _
                                        branchCode, _
                                        reserveId, _
                                        stallId, _
                                        startTime, _
                                        endTime)
        If retReserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]MoveReserve", "", Nothing, "RET:Nothing")
            Return Nothing
        End If

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserve", "", Nothing, _
                  "RET:" & CType(retReserveList.Count, String))
        Return retReserveList

    End Function


    ''' <summary>
    ''' 指定時間への予約の移動(再帰呼び出し用サブルーチン)
    ''' </summary>
    ''' <param name="reserveList">予約情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <returns>移動させる予約情報。異常終了した場合、Nothing</returns>
    ''' <remarks></remarks>
    Private Function MoveReserveSub(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                    ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                    ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                    ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal reserveId As Decimal, _
                                    ByVal stallId As Decimal, _
                                    ByVal startTime As DateTime, _
                                    ByVal endTime As DateTime) As SC3150101DataSet.SC3150101StallReserveListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserveSub", "", Nothing, _
                  "REZ_INFO:" & CType(reserveList.Count, String), _
                  "STALL_TIME_INFO:" & CType(stallTimeInfo.Count, String), _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        If reserveList.Count = 0 Then
            Return Nothing
        End If

        Dim targetList As New List(Of SC3150101DataSet.SC3150101StallReserveListRow)
        Dim chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable
        Dim retReserveInfo As SC3150101DataSet.SC3150101StallReserveListDataTable
        Using ReserveInfo As New SC3150101DataSet.SC3150101StallReserveListDataTable
            retReserveInfo = ReserveInfo
        End Using
        Dim drReserveInfoTemp() As SC3150101DataSet.SC3150101StallReserveListRow
        Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveListRow

        Dim kadoTime(2) As Date
        kadoTime = GetOperationTime(stallTimeInfo, startTime)

        Dim sKadoTime As Date = kadoTime(0)
        Dim eKadoTime As Date = kadoTime(1)

        ' 衝突チェック
        If IsCollision(reserveList, reserveId, startTime, endTime) = False Then
            ' 衝突なし
            drReserveInfoTemp = DirectCast(reserveList.Select("REZID = " & CType(reserveId, String)),  _
                                      SC3150101DataSet.SC3150101StallReserveListRow())
            drReserveInfo = MoveReserve(drReserveInfoTemp(0), startTime, endTime)
            retReserveInfo.ImportRow(drReserveInfo)
            OutputLog("I", "[E]MoveReserveSub", "", Nothing, _
                      "RET:" & CType(retReserveInfo.Count, String))
            Return retReserveInfo
        End If

        For Each reserveListItem As SC3150101DataSet.SC3150101StallReserveListRow In _
            From r As SC3150101DataSet.SC3150101StallReserveListRow In reserveList _
            Where (startTime < r.ENDTIME) AndAlso (r.REZID <> reserveId)
            Order By r.STARTTIME
            ' 開始時間により干渉する選択チップ以外のチップをとりあえず移動対象とする
            targetList.Add(reserveListItem)
        Next

        chipTimeList = GetChipTimeList(breakInfo, targetList, eKadoTime)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (tp.STARTTIME < endTime) And (startTime < tp.ENDTIME) Then
                ' 動かせれない
                Return Nothing
            End If
        Next

        drReserveInfoTemp = DirectCast(reserveList.Select("REZID = " & CType(reserveId, String)),  _
                                        SC3150101DataSet.SC3150101StallReserveListRow())
        drReserveInfo = MoveReserve(drReserveInfoTemp(0), startTime, endTime)

        Dim drItem2 As SC3150101DataSet.SC3150101StallReserveListRow
        Do While targetList.Count > 0
            Dim i As Integer = 0
            Dim tp As SC3150101DataSet.SC3150101ChipTimeRow
            Dim et As DateTime ' 作業変数(endtime用)

            Do While i < targetList.Count
                drItem2 = CType(targetList(i), SC3150101DataSet.SC3150101StallReserveListRow)

                Dim chipStartTime As Date
                Dim chipEndTime As Date
                Dim chipInfo(3) As String
                Dim kind As Integer = 0
                If drItem2.Movable.Equals("1") Then
                    chipInfo = assortMovableChip(chipTimeList, _
                                                 stallTimeInfo, _
                                                 drItem2, _
                                                 dealerCode, _
                                                 branchCode, _
                                                 stallId, _
                                                 endTime, _
                                                 sKadoTime)

                    chipStartTime = Date.ParseExact(chipInfo(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipEndTime = Date.ParseExact(chipInfo(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)

                    kind = CType(chipInfo(2), Integer)

                    Dim tpTemp() As SC3150101DataSet.SC3150101ChipTimeRow
                    If kind = 2 Then

                        tpTemp = DirectCast(chipTimeList.Select("REZID = " & CType(drItem2.REZID, String)),  _
                                                SC3150101DataSet.SC3150101ChipTimeRow())
                        tp = tpTemp(0)
                        tp.STARTTIME = drItem2.STARTTIME
                        tp.ENDTIME = drItem2.ENDTIME
                        targetList.RemoveAt(i)
                        Exit Do
                    End If
                    tpTemp = DirectCast(chipTimeList.Select("REZID = " & drItem2.REZID),  _
                                                SC3150101DataSet.SC3150101ChipTimeRow())
                    tp = tpTemp(0)
                    tp.STARTTIME = chipStartTime
                    tp.ENDTIME = chipEndTime
                    If kind = 3 Then

                        drItem2 = MoveReserve(drItem2, tp.STARTTIME, tp.ENDTIME)

                        ' 行をコピー(移動対象を戻り値に追加)
                        retReserveInfo.ImportRow(drItem2)

                        targetList.RemoveAt(i)

                        Exit Do
                    End If
                    If kind < 4 Then
                        i = i + 1
                    End If
                Else
                    targetList.RemoveAt(i)
                End If
            Loop

            et = New DateTime(9999, 12, 31, 23, 59, 59)

            For Each tp In chipTimeList.Rows
                If (tp.ENDTIME < et) AndAlso (endTime < tp.ENDTIME) Then
                    et = tp.ENDTIME
                End If
            Next
            endTime = et
        Loop

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserveSub", "", Nothing, _
                  "RET:" & CType(retReserveInfo.Count, String))
        Return retReserveInfo

    End Function


    ''' <summary>
    ''' 予約日時変更
    ''' </summary>
    ''' <param name="startTime">予約開始日時</param>
    ''' <param name="endTime">予約終了日時</param>
    ''' <returns>日時変更した予約情報</returns>
    ''' <remarks></remarks>
    Private Function MoveReserve(ByVal drReserveList As SC3150101DataSet.SC3150101StallReserveListRow, _
                                 ByVal startTime As Date, ByVal endTime As Date) As SC3150101DataSet.SC3150101StallReserveListRow

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserve", "", Nothing, _
                  "REZ_INFO:(DataRow)", _
                  "STRCD:" & startTime.ToString(CultureInfo.InvariantCulture()))

        drReserveList.PrevStartTime = drReserveList.STARTTIME
        drReserveList.PrevEndTime = drReserveList.ENDTIME

        If (drReserveList.STARTTIME <> startTime) Or (drReserveList.ENDTIME <> endTime) Then
            drReserveList.STARTTIME = startTime
            drReserveList.ENDTIME = endTime
            drReserveList.Moved = "1"
        End If

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserve", "", Nothing, _
                  "RET:(DataSet)" & CType(drReserveList.ItemArray.Count, String))
        Return drReserveList

    End Function


    ''' <summary>
    ''' 稼働時間の確定
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <returns>稼動開始時間、稼動終了時間</returns>
    ''' <remarks></remarks>
    Private Function GetOperationTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                      ByVal startTime As Date) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]GetOperationTime", "", Nothing)

        Dim retTime(2) As Date

        ' 稼動時間帯を取得
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        Dim operationStartTimet As TimeSpan
        Dim operationEndTime As TimeSpan
        operationStartTimet = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        ' 対象の営業時間帯を確定する
        Dim sKadoTime As Date
        Dim eKadoTime As Date

        If startTime.Date.Add(operationStartTimet) > startTime.Date.Add(operationEndTime) Then
            ' 日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If startTime.Date.AddDays(-1).Add(operationStartTimet) <= startTime _
                And startTime < startTime.Date.Add(operationEndTime) Then
                sKadoTime = startTime.Date.AddDays(-1).Add(operationStartTimet)
                eKadoTime = startTime.Date.Add(operationEndTime)
            Else
                sKadoTime = startTime.Date.Add(operationStartTimet)
                eKadoTime = startTime.Date.AddDays(1).Add(operationEndTime)
            End If
        Else
            ' 通常稼動の場合
            sKadoTime = startTime.Date.Add(operationStartTimet)
            eKadoTime = startTime.Date.Add(operationEndTime)
        End If

        retTime(0) = sKadoTime
        retTime(1) = eKadoTime

        OutputLog(LOG_TYPE_INFO, "[E]GetOperationTime", "", Nothing)

        Return retTime

    End Function


    ''' <summary>
    ''' 移動チップ情報を取得
    ''' </summary>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="targetList">移動対象チップ情報</param>
    ''' <param name="eKadoTime">稼動終了時間</param>
    ''' <returns>チップ情報</returns>
    ''' <remarks></remarks>
    Private Function GetChipTimeList(ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                     ByVal targetList As List(Of SC3150101DataSet.SC3150101StallReserveListRow), _
                                     ByVal eKadoTime As Date) As SC3150101DataSet.SC3150101ChipTimeDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetChipTimeList", "", Nothing)

        Dim chipTimeList As New SC3150101DataSet.SC3150101ChipTimeDataTable

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow
        For i As Integer = targetList.Count - 1 To 0 Step -1 ' 降順に取り出す

            Dim tb As New SC3150101DataSet.SC3150101ChipTimeDataTable
            Dim chipItem As SC3150101DataSet.SC3150101ChipTimeRow
            chipItem = CType(tb.NewRow(), SC3150101DataSet.SC3150101ChipTimeRow)
            ' データコピー用
            Dim drChipTimeInfo As SC3150101DataSet.SC3150101ChipTimeRow
            drChipTimeInfo = CType(chipTimeList.NewRow(), SC3150101DataSet.SC3150101ChipTimeRow)

            'drItem = CType(TargetList.GetByIndex(i), SC3150101DataSet.SC3150101StallReserveListRow)
            reserveItem = CType(targetList(i), SC3150101DataSet.SC3150101StallReserveListRow)

            If reserveItem.Movable.Equals("1") Then

                If (Not String.Equals(reserveItem.REZ_RECEPTION, "0")) _
                    And (Not IsDBNull(reserveItem.Item("REZ_DELI_DATE"))) Then
                    chipItem.ENDTIME = Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                Else
                    chipItem.ENDTIME = eKadoTime
                End If

                Dim dateTemp(START_TIME_ARRAY_NUMBER) As Date ' 一時格納用date型配列

                dateTemp = CalculateStartTime(breakInfo, _
                                              chipItem.ENDTIME, _
                                              CType(reserveItem.REZ_WORK_TIME, Integer), _
                                              convertBoolean(reserveItem.InBreak))
                chipItem.STARTTIME = dateTemp(START_TIME_START)
                chipItem.ENDTIME = dateTemp(START_TIME_END) '※既存処理ではByRefで戻り引数になっている

                Dim cl As Boolean
                Do
                    cl = False

                    Dim chipTime(3) As String
                    chipTime = GetChipStartTime(chipTimeList, breakInfo, chipItem.STARTTIME, _
                                                chipItem.ENDTIME, CType(reserveItem.REZ_WORK_TIME, Integer), _
                                                reserveItem.InBreak)
                    Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipItem.STARTTIME = Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipItem.ENDTIME = Date.ParseExact(chipTime(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    cl = convertBoolean(chipTime(2))

                Loop While (cl = True)
            Else
                chipItem.STARTTIME = reserveItem.STARTTIME
                chipItem.ENDTIME = reserveItem.ENDTIME
            End If
            chipItem.REZID = reserveItem.REZID

            ' チップの予約ID、開始時間、終了時間を格納
            drChipTimeInfo.REZID = chipItem.REZID
            drChipTimeInfo.STARTTIME = chipItem.STARTTIME
            drChipTimeInfo.ENDTIME = chipItem.ENDTIME
            chipTimeList.Rows.Add(drChipTimeInfo)

        Next

        OutputLog(LOG_TYPE_INFO, "[E]GetChipTimeList", "", Nothing)

        Return chipTimeList

    End Function


    ''' <summary>
    ''' 移動対象チップを分類する
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="drItem2">予約情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="sKadoTime">稼動開始時間</param>
    ''' <returns>開始時間、終了時間、分類</returns>
    ''' <remarks></remarks>
    Private Function assortMovableChip(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                       ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                       ByVal drItem2 As SC3150101DataSet.SC3150101StallReserveListRow, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Decimal, _
                                       ByVal endTime As Date, _
                                       ByVal sKadoTime As Date) As String()

        OutputLog(LOG_TYPE_INFO, "[S]assortMovableChip", "", Nothing)

        Dim retChipInfo(3) As String
        Dim chipStartTime As Date
        Dim chipEndTime As Date
        Dim kind As Integer = 0
        Dim cl As Boolean
        Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date ' 一時格納用date型配列

        If drItem2.Movable.Equals("1") Then
            kind = 1

            If Not String.Equals(drItem2.REZ_RECEPTION, "0") Then
                chipStartTime = Date.ParseExact(drItem2.REZ_PICK_DATE, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            Else
                chipStartTime = sKadoTime
            End If

            If chipStartTime < endTime Then
                dateTemp = CalculateEndTime(stallTimeInfo, _
                                            dealerCode, _
                                            branchCode, _
                                            stallId, _
                                            endTime, _
                                            CType(drItem2.REZ_WORK_TIME, Integer), _
                                            convertBoolean(drItem2.InBreak))
                chipEndTime = dateTemp(END_TIME_END)
                chipStartTime = dateTemp(END_TIME_START)
                endTime = dateTemp(END_TIME_START) '※
            Else
                dateTemp = CalculateEndTime(stallTimeInfo, _
                                            dealerCode, _
                                            branchCode, _
                                            stallId, _
                                            chipStartTime, _
                                            CType(drItem2.REZ_WORK_TIME, Integer), _
                                            convertBoolean(drItem2.InBreak))
                chipEndTime = dateTemp(END_TIME_END)
                chipStartTime = dateTemp(END_TIME_START) '※
            End If

            Do
                cl = False
                Dim chipTime(3) As String
                chipTime = GetChipEndTime(chipTimeList, stallTimeInfo, dealerCode, _
                                          branchCode, stallId, _
                                          drItem2.REZID, _
                                          chipStartTime, _
                                          chipEndTime, _
                                          CType(drItem2.REZ_WORK_TIME, Integer), _
                                          drItem2.InBreak)
                cl = convertBoolean(chipTime(2))
                If cl = True Then
                    chipEndTime = Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipStartTime = Date.ParseExact(chipTime(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                End If

            Loop While (cl = True)

            ' 移動対象外判定?
            cl = IsMoveingChip(chipTimeList, chipStartTime, drItem2.STARTTIME, cl)
            If (cl = False) And (chipStartTime < drItem2.STARTTIME) Then

                kind = 2

                retChipInfo(0) = chipStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
                retChipInfo(1) = chipEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
                retChipInfo(2) = CType(kind, String)

                OutputLog(LOG_TYPE_INFO, "[E]assortMovableChip", "", Nothing)

                Return retChipInfo
            End If

            If chipStartTime = endTime Then
                kind = 3
            End If

        Else
            kind = 4
        End If


        retChipInfo(0) = chipStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipInfo(1) = chipEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipInfo(2) = CType(kind, String)

        OutputLog(LOG_TYPE_INFO, "[E]assortMovableChip", "", Nothing)

        Return retChipInfo

    End Function

    ''' <summary>
    ''' 移動対象チップの開始(終了)時間の確定
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="breakInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>開始時間、終了時間、判定</returns>
    ''' <remarks></remarks>
    Private Function GetChipStartTime(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                      ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                      ByVal startTime As Date, ByVal endTime As Date, _
                                      ByVal workTime As Integer, ByVal isBreak As String) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetChipStartTime", "", Nothing)

        Dim retChipTime(3) As String
        Dim startTimeTemp As Date
        Dim endTimeTemp As Date
        Dim st As DateTime
        Dim cl As Integer

        startTimeTemp = startTime
        endTimeTemp = endTime

        cl = 0
        st = New DateTime(9999, 12, 31, 23, 59, 59)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (tp.STARTTIME < endTimeTemp) And (startTimeTemp < tp.ENDTIME) Then
                cl = 1
                If tp.STARTTIME < st Then
                    st = tp.STARTTIME
                End If
            End If
        Next

        Dim dateTemp(START_TIME_ARRAY_NUMBER) As Date

        If cl = 1 Then
            endTimeTemp = st 'drTb.ENDTIME
            dateTemp = CalculateStartTime(breakInfo, _
                                          endTimeTemp, _
                                          workTime, _
                                          convertBoolean(isBreak))
            startTimeTemp = dateTemp(START_TIME_START) 'drTb.STARTTIME
            endTimeTemp = dateTemp(START_TIME_END) '※'drTb.ENDTIME
        End If

        retChipTime(0) = startTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(1) = endTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(2) = CType(cl, String)

        OutputLog(LOG_TYPE_INFO, "[E]GetChipStartTime", "", Nothing)

        Return retChipTime

    End Function


    ''' <summary>
    ''' 移動対象チップの終了(開始)時間の確定
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>開始時間、終了時間、判定</returns>
    ''' <remarks></remarks>
    Private Function GetChipEndTime(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                    ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                    ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal stallId As Decimal, _
                                    ByVal reserveId As Decimal, _
                                    ByVal startTime As Date, _
                                    ByVal endTime As Date, _
                                    ByVal workTime As Integer, _
                                    ByVal isBreak As String) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetChipEndTime", "", Nothing)

        Dim retChipTime(3) As String
        Dim startTimeTemp As Date
        Dim endTimeTemp As Date
        Dim et As DateTime
        Dim cl As Integer

        cl = 0
        startTimeTemp = startTime
        endTimeTemp = endTime
        et = New DateTime(1, 1, 1, 0, 0, 0)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (reserveId <> tp.REZID) And (tp.STARTTIME < endTimeTemp) _
                And (startTimeTemp < tp.ENDTIME) Then
                cl = 1
                If et < tp.ENDTIME Then
                    et = tp.ENDTIME
                End If
            End If
        Next

        Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date

        If cl = 1 Then
            startTimeTemp = et
            dateTemp = CalculateEndTime(stallTimeInfo, _
                                        dealerCode, _
                                        branchCode, _
                                        stallId, _
                                        startTimeTemp, _
                                        workTime, _
                                        convertBoolean(isBreak))
            endTimeTemp = dateTemp(END_TIME_END)
            startTimeTemp = dateTemp(END_TIME_START) '※
        End If

        retChipTime(0) = endTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(1) = startTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(2) = CType(cl, String)

        OutputLog(LOG_TYPE_INFO, "[E]GetChipEndTime", "", Nothing)

        Return retChipTime

    End Function
    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 作業進捗エリアに表示するTACTのR/O枝番を取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">予約ID</param>
    ' ''' <param name="workSeq">作業連番</param>
    ' ''' <returns>TACTのR/O枝番</returns>
    ' ''' <remarks></remarks>
    'Public Function GetTactChildNo(ByVal dlrCD As String, ByVal orderNo As String, ByVal workSeq As Integer) As String

    '    Logger.Info("GetTactChildNo Start param1:" + dlrCD + _
    '                                    " param2:" + orderNo + _
    '                                    " param3:" + CType(workSeq, String))

    '    Dim rtnVal As String = String.Empty

    '    Dim IC3800804 As New IC3800804BusinessLogic

    '    '追加作業API取得
    '    Dim dt As DataTable = IC3800804.GetAddRepairStatusList(dlrCD, orderNo)

    '    OutPutIFLog(dt, "IC3800804.GetAddRepairStatusList")

    '    '枝番（追加作業番号）が取得件数以上ない場合、データ不整合
    '    If Not IsNothing(dt) AndAlso workSeq <= dt.Rows.Count Then
    '        'テーブルの配列は0からのため、-1
    '        Dim dRow As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow _
    '            = DirectCast(dt.Rows(workSeq - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)

    '        rtnVal = dRow.SRVADDSEQ
    '    End If

    '    Logger.Info("GetTactChildNo End Return: " + rtnVal)

    '    Return rtnVal
    'End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 移動対象チップ判定？
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="criterionStartTime">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <returns>移動対象：True、非移動対象：False</returns>
    ''' <remarks></remarks>
    Private Function IsMoveingChip(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                   ByVal criterionStartTime As Date, _
                                   ByVal startTime As Date, _
                                   ByVal cl As Boolean) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsMoveingChip", "", Nothing)

        Dim isMove As Boolean = cl

        If criterionStartTime < startTime Then
            isMove = False
            For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
                If (tp.STARTTIME < startTime) And (startTime < tp.ENDTIME) Then
                    'OutputLog(LOG_TYPE_INFO, "[E]IsMoveingChip()", "", Nothing)
                    isMove = True
                End If
            Next
        End If

        OutputLog(LOG_TYPE_INFO, "[E]IsMoveingChip", "", Nothing)

        Return isMove

    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 時間に変更のあった予約情報の更新
    ' ''' </summary>
    ' ''' <param name="reserveList">予約情報</param>
    ' ''' <param name="reserveId">更新対象外の予約ID</param>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="stallID">ストールID</param>
    ' ''' <param name="updateAccount">アカウント</param>
    ' ''' <returns>エラーが発生した場合、-1</returns>
    ' ''' <remarks></remarks>
    'Public Function UpdateAllReserve(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
    '                                 ByVal reserveId As Long, _
    '                                 ByVal dealerCode As String, _
    '                                 ByVal branchCode As String, _
    '                                 ByVal stallId As Integer, _
    '                                 ByVal updateAccount As String, _
    '                                 ByVal updateDate As Date) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]UpdateAllReserve", "", Nothing, _
    '              "REZ_INFO:(DataSet)", "REZID:" & CType(reserveId, String), _
    '              "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
    '              "STALLID:" & CType(stallId, String), "ACCOUNT:" & updateAccount)

    '    ' 引数チェック
    '    If reserveList Is Nothing Then
    '        ' 更新対象がない
    '        OutputLog(LOG_TYPE_WARNING, "[E]UpdateAllReserve", "Argument is nothing, so do nothing.", Nothing)
    '        Return ReturnOk
    '    End If

    '    'Dim itm As ReserveInfo
    '    Dim reserveListItem As SC3150101DataSet.SC3150101StallReserveListRow

    '    Dim resultUpdRez As Integer
    '    'Dim resultInsRezHis As Integer

    '    ' SC3150101TableAdapterクラスのインスタンスを生成
    '    Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
    '        Using reserveInfo As New SC3150101DataSet.SC3150101StallReserveInfoDataTable
    '            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
    '            For Each reserveListItem In reserveList.Rows

    '                'If (reserveListItem.Moved = "1") And (reserveListItem.REZID <> reserveId) Then
    '                If (reserveListItem.Moved.Equals("1")) _
    '                    And (Not String.Equals(reserveListItem.REZID, reserveId)) Then

    '                    drReserveInfo = CType(reserveInfo.NewRow(),  _
    '                                          SC3150101DataSet.SC3150101StallReserveInfoRow)

    '                    ' 更新データの設定
    '                    drReserveInfo.DLRCD = dealerCode
    '                    drReserveInfo.STRCD = branchCode
    '                    drReserveInfo.STALLID = stallId
    '                    drReserveInfo.REZID = reserveListItem.REZID
    '                    drReserveInfo.STARTTIME = reserveListItem.STARTTIME
    '                    drReserveInfo.ENDTIME = reserveListItem.ENDTIME
    '                    drReserveInfo.REZ_WORK_TIME = reserveListItem.REZ_WORK_TIME
    '                    drReserveInfo.STATUS = reserveListItem.STATUS
    '                    drReserveInfo.SVC_STATUS = reserveListItem.RESULT_STATUS
    '                    drReserveInfo.STALL_USE_STATUS = reserveListItem.STALL_USE_STATUS
    '                    If IsDBNull(reserveListItem.Item("STRDATE")) Then
    '                        drReserveInfo.STRDATE = DateTime.MinValue
    '                    Else
    '                        drReserveInfo.STRDATE = reserveListItem.STRDATE
    '                    End If
    '                    drReserveInfo.WASHFLG = reserveListItem.WASHFLG
    '                    drReserveInfo.INSPECTIONFLG = reserveListItem.INSPECTIONFLG
    '                    drReserveInfo.STOPFLG = "0"
    '                    drReserveInfo.CANCELFLG = "0"
    '                    'RezItem.RestFlg = reserveListItem.RestFlg

    '                    ' データセットに行を追加
    '                    reserveInfo.Rows.Add(drReserveInfo)

    '                    ' ストール予約情報を更新する
    '                    resultUpdRez = UpdateStallReserveInfo(adapter, reserveInfo, updateAccount, updateDate)
    '                    If (resultUpdRez <= 0) Then
    '                        OutputLog("I", "[E]UpdateAllReserve", "", Nothing, _
    '                                  "RET:" & CType(ReturnNG, String))
    '                        Return (ReturnNG)
    '                    End If

    '                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '                    ' ストール予約履歴を登録する
    '                    'resultInsRezHis = adapter.InsertRezHistory(dealerCode, branchCode, reserveId, 1)
    '                    ' 2012.02.01 edit 移動した予約チップの履歴をつくるため引数を対象の予約IDになるように修正

    '                    'resultInsRezHis = adapter.InsertReserveHistory(dealerCode, branchCode, _
    '                    '                                               CType(drReserveInfo.REZID, Integer), 1)
    '                    'If (resultInsRezHis <= 0) Then
    '                    '    OutputLog("I", "[E]UpdateAllReserve", "", Nothing, _
    '                    '              "RET:" & CType(ReturnNG, String))
    '                    '    Return (ReturnNG)
    '                    'End If
    '                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '                    ' データクリア
    '                    reserveInfo.Clear()

    '                End If
    '            Next
    '        End Using
    '    End Using

    '    OutputLog(LOG_TYPE_INFO, "[E]UpdateAllReserve", "", Nothing, _
    '              "RET:" & CType(ReturnOk, String))
    '    Return ReturnOk

    'End Function
    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


    'Public Function UpdateStallReserveInfo() As Integer

    '    ' ストール利用情報を更新する
    '    Dim resultUpdRezStallUse As Integer = adapter.UpdateReserveStallUse(reserveInfo, updateAccount)

    '    If (resultUpdRezStallUse <= 0) Then
    '        ' ストール利用情報の更新に失敗
    '        OutputLog(LOG_TYPE_ERROR, "UpdateReserveStallUse", "Failed to update the stall reservation information.", Nothing)

    '    End If

    '    ' サービス入庫情報を更新する
    '    Dim resultUpdRezServiceIn As Integer = adapter.UpdateReserveServiceIn(reserveInfo, updateAccount)

    '    If (resultUpdRezStallUse <= 0) Then
    '        'サービス入庫情報の更新に失敗
    '        OutputLog(LOG_TYPE_ERROR, "UpdateReserveServiceIn", "Failed to update the stall reservation information.", Nothing)

    '    End If

    '    ' サービス入庫情報を更新する
    '    Dim resultUpdRezJobDetail As Integer = adapter.UpdateJobDetail(reserveInfo, updateAccount)

    '    If (resultUpdRezJobDetail <= 0) Then
    '        'サービス入庫情報の更新に失敗
    '        OutputLog(LOG_TYPE_ERROR, "UpdateJobDetail", "Failed to update the stall reservation information.", Nothing)

    '    End If

    '    Return
    'End Function

    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 稼動時間帯の確定
    ''' </summary>
    ''' <param name="reserveInfo">予約情報</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="startOperationTime">稼動開始時刻</param>
    ''' <param name="endOperationTime">稼動終了時刻</param>
    ''' <returns>稼動時間帯：0、非稼動時間帯：-1</returns>
    ''' <remarks></remarks>
    Private Function DecisionOperationTime(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                           ByVal startTime As Date, _
                                           ByVal startOperationTime As TimeSpan, _
                                           ByVal endOperationTime As TimeSpan) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]DecisionOperationTime", "", Nothing, _
                  "REZ_INFO:(DataSet)", "START_TIME:" & CType(startTime, String), _
                  "KADO_START_TIME:" & startOperationTime.ToString(), _
                  "KADO_END_TIME:" & endOperationTime.ToString())

        ' 戻り値にエラーを設定
        DecisionOperationTime = ReturnNG
        Try
            Dim workOperationStartTime As DateTime     ' 作業開始時刻の稼動時間帯の開始時刻
            Dim scheduleOperationStartTime As DateTime ' 予定開始時刻の稼動時間帯の開始時刻
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
            drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            If startTime.Date.Add(startOperationTime) < startTime.Date.Add(endOperationTime) Then
                ' 通常稼動の場合
                'If startTime.Date > drReserveInfo.ENDTIME.Date Or startTime.Date < drReserveInfo.STARTTIME.Date Then
                If (drReserveInfo.ENDTIME.Date < startTime.Date) _
                    Or (startTime.Date < drReserveInfo.STARTTIME.Date) Then
                    ' 稼働時間外開始
                    OutputLog(LOG_TYPE_INFO, "DecisionOperationTime", "Out of service time", Nothing)
                    Exit Try
                End If
            Else
                Dim kadoStartTimeTemp As Date ' 作業用変数
                Dim kadoEndTimeTemp As Date   ' 作業用変数

                '作業開始時刻の稼動時間帯の開始時刻を取得
                kadoStartTimeTemp = startTime.Date.AddDays(-1).Add(startOperationTime)
                kadoEndTimeTemp = startTime.Date.Add(endOperationTime)
                'If startTime.Date.AddDays(-1).Add(startOperationTime) <= startTime And startTime < startTime.Date.Add(endOperationTime) Then
                If (kadoStartTimeTemp <= startTime) And (startTime < kadoEndTimeTemp) Then
                    workOperationStartTime = startTime.AddDays(-1).Date.Add(startOperationTime)
                Else
                    workOperationStartTime = startTime.Date.Add(startOperationTime)
                End If

                ' 予定開始時刻の稼動時間帯の開始時刻を取得
                kadoStartTimeTemp = drReserveInfo.STARTTIME.Date.AddDays(-1).Add(startOperationTime)
                kadoEndTimeTemp = drReserveInfo.STARTTIME.Date.Add(endOperationTime)
                'If drReserveInfo.STARTTIME.Date.AddDays(-1).Add(startOperationTime) <= drReserveInfo.STARTTIME And drReserveInfo.STARTTIME < drReserveInfo.STARTTIME.Date.Add(endOperationTime) Then
                If (kadoStartTimeTemp <= drReserveInfo.STARTTIME) _
                    And (drReserveInfo.STARTTIME < kadoEndTimeTemp) Then
                    scheduleOperationStartTime = drReserveInfo.STARTTIME.AddDays(-1).Date.Add(startOperationTime)
                Else
                    scheduleOperationStartTime = drReserveInfo.STARTTIME.Date.Add(startOperationTime)
                End If

                ' 作業開始時刻の存在する稼動時間帯の開始時刻が、予定終了時刻がより後、または
                ' 作業開始時刻が、予定開始時刻の存在する稼動時間帯の開始時刻より前の場合エラー
                'If workOperationStartTime > drReserveInfo.ENDTIME Or startTime < scheduleOperationStartTime Then
                If (drReserveInfo.ENDTIME < workOperationStartTime) _
                    Or (startTime < scheduleOperationStartTime) Then
                    ' 稼働時間外開始
                    OutputLog(LOG_TYPE_INFO, "DecisionOperationTime", "Out of service time.", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            DecisionOperationTime = ReturnOk

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]DecisionOperationTime", "", Nothing, _
                      "RET:" & DecisionOperationTime.ToString(CultureInfo.CurrentCulture))
        End Try

        Return DecisionOperationTime

    End Function


    ''' <summary>
    ''' 二重作業開始チェック
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="startOperationTime">稼動開始時刻</param>
    ''' <param name="endOperationTime">稼動終了時刻</param>
    ''' <returns>開始可：0、開始不可：-1</returns>
    ''' <remarks></remarks>
    Private Function CheckMultiStarts(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal stallId As Decimal, _
                                      ByVal startTime As Date, _
                                      ByVal startOperationTime As TimeSpan, _
                                      ByVal endOperationTime As TimeSpan) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]CheckMultiStarts", "", Nothing, _
                  "DLR_CD:" & CType(dealerCode, String), _
                  "BRN_CD:" & CType(branchCode, String), _
                  "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & CType(startTime, String), _
                  "KADO_START_TIME:" & startOperationTime.ToString(), _
                  "KADO_END_TIME:" & endOperationTime.ToString())

        Dim operationStart As Date
        Dim operationEnd As Date
        If startTime.Date.Add(startOperationTime) < startTime.Date.Add(endOperationTime) Then
            ' 通常稼動の場合
            operationStart = startTime.Date.Add(startOperationTime)
            operationEnd = startTime.Date.Add(endOperationTime)
        Else
            Dim kadoStartTimeTemp As Date ' 作業用変数
            Dim kadoEndTimeTemp As Date   ' 作業用変数
            kadoStartTimeTemp = startTime.Date.AddDays(-1).Add(startOperationTime)
            kadoEndTimeTemp = startTime.Date.Add(endOperationTime)
            ' 日跨ぎ稼動の場合
            'If startTime.Date.AddDays(-1).Add(startOperationTime) <= startTime And startTime < startTime.Date.Add(endOperationTime) Then
            If (kadoStartTimeTemp <= startTime) And (startTime < kadoEndTimeTemp) Then
                operationStart = startTime.AddDays(-1).Date.Add(startOperationTime)
                operationEnd = startTime.Date.Add(endOperationTime)
            Else
                operationStart = startTime.Date.Add(startOperationTime)
                operationEnd = startTime.AddDays(1).Date.Add(endOperationTime)
            End If
        End If

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            ' 作業中の数を取得
            Dim workingState As SC3150101DataSet.SC3150101WorkingStateCountDataTable
            workingState = adapter.GetWorkingStateCount(dealerCode, branchCode, stallId, operationStart, operationEnd)
            Dim drWorkingState As SC3150101DataSet.SC3150101WorkingStateCountRow
            drWorkingState = CType(workingState.Rows(0), SC3150101DataSet.SC3150101WorkingStateCountRow)
            ' 作業開始数の確認
            If drWorkingState.COUNT > 0 Then
                ' すでに作業開始されている
                OutputLog(LOG_TYPE_INFO, "[E]CheckMultiStarts", "There already is another working chip.", Nothing)
                Return (-1)
            End If

            OutputLog(LOG_TYPE_INFO, "[E]CheckMultiStarts", "", Nothing, _
                          "RET:" & CType(0, String))
            Return 0

        End Using

    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 子予約連番の再割振
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コードID</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="reserveId">予約ID</param>
    ' ''' <param name="parentsReserveId">管理予約ID</param>
    ' ''' <returns>子予約連番、エラー：-99</returns>
    ' ''' <remarks></remarks>
    'Private Function ReorderReserveChildNo(ByVal dealerCode As String, _
    '                                       ByVal branchCode As String, _
    '                                       ByVal reserveId As Long, _
    '                                       ByVal parentsReserveId As Long) As Integer

    '    OutputLog(LOG_TYPE_INFO, "[S]ReorderReserveChildNo", "", Nothing, _
    '              "DLRCD:" & dealerCode, "STRCD:" & branchCode, "REZID:" & CType(reserveId, String))

    '    Dim childNo As Integer = -1

    '    ' SC3150101TableAdapterクラスのインスタンスを生成
    '    Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

    '        Try
    '            'リレーション内の作業終了(97)チップの最大子予約連番(REZCHILDNO)を取得し
    '            '子予約連番の再振当てを行う対象を特定する（作業終了の次の子予約連番からリレーションの最後まで）
    '            Dim relationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoDataTable
    '            relationLastChildNoInfo = adapter.GetRelationLastChildNo(dealerCode, _
    '                                                                     branchCode, _
    '                                                                     parentsReserveId)

    '            Dim drRelationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoRow
    '            drRelationLastChildNoInfo = CType(relationLastChildNoInfo.Rows(0),  _
    '                                              SC3150101DataSet.SC3150101RelationLastChildNoRow)
    '            Dim maxFinishedChildNo As Integer
    '            ' 最大子予約連番を設定
    '            If IsNothing(drRelationLastChildNoInfo) _
    '                Or IsDBNull(drRelationLastChildNoInfo.Item("REZCHILDNO")) Then
    '                'データが無い場合は、リレーション内に完了チップが無い場合なので１から連番
    '                maxFinishedChildNo = 0
    '            Else
    '                maxFinishedChildNo = CType(drRelationLastChildNoInfo.REZCHILDNO, Integer)
    '            End If

    '            ' リレーション内の子予約連番(REZCHILDNO)更新対象を取得
    '            Dim childNoUpdateTarget As SC3150101DataSet.SC3150101TargetChildNoInfoDataTable
    '            childNoUpdateTarget = adapter.GetChildNoUpdateTarget(dealerCode, branchCode, _
    '                                                                 parentsReserveId, _
    '                                                                 maxFinishedChildNo, _
    '                                                                 reserveId)

    '            Dim drChildNoUpdateTarget As SC3150101DataSet.SC3150101TargetChildNoInfoRow
    '            '最初のレコードをmaxFinishedChildNo+1 で更新、以降は前レコードの+1で更新
    '            Dim resultUpdateChildNo As Integer = -1
    '            '符番していく子予約連番はMax+2から（Maxまでは既に符番されており、またMAX+1は開始対象に符番する）
    '            Dim tempChildNo As Integer = maxFinishedChildNo + 2

    '            '１レコード目に開始対象となるデータが設定されているため、２レコード目から更新
    '            For i As Integer = 1 To childNoUpdateTarget.Rows.Count - 1 Step 1

    '                drChildNoUpdateTarget = DirectCast(childNoUpdateTarget.Rows(i), SC3150101DataSet.SC3150101TargetChildNoInfoRow)

    '                '子予約連番更新ログ
    '                OutputLog(LOG_TYPE_INFO, "ReorderReserveChildNo", "", Nothing, _
    '                          "Exchange RezId:" & drChildNoUpdateTarget.REZID, _
    '                          "Before RezChildNo:" & drChildNoUpdateTarget.REZCHILDNO, _
    '                           "After RezChildNo:" & tempChildNo)


    '                ' 子予約連番の更新
    '                resultUpdateChildNo = adapter.UpdateChildNo(dealerCode, _
    '                                                            branchCode, _
    '                                                            drChildNoUpdateTarget.REZID, _
    '                                                            tempChildNo)

    '                ' 子予約連番の更新に失敗
    '                If resultUpdateChildNo <= 0 Then
    '                    ' ロールバック
    '                    'Me.Rollback = True
    '                    OutputLog(LOG_TYPE_ERROR, "ReorderReserveChildNo", "Failed to update the 'REZCHILDNO'", Nothing)
    '                    childNo = -1
    '                    Exit Try
    '                End If

    '                '子予約連番のインクリメント
    '                tempChildNo = tempChildNo + 1
    '            Next
    '            '今回開始する予約を子予約連番の一番若い番号にするため
    '            '作業終了の中で最大の子予約連番＋１の値に更新する

    '            childNo = maxFinishedChildNo + 1
    '        Finally
    '            OutputLog(LOG_TYPE_INFO, "[E]ReorderReserveChildNo", "", Nothing, _
    '                                      "RET:" & CType(childNo, String))
    '        End Try

    '        Return childNo
    '    End Using

    'End Function


    ' ''' <summary>
    ' ''' 時間を見直す
    ' ''' </summary>
    ' ''' <param name="startTime">開始時間</param>
    ' ''' <param name="endTime">終了時間</param>
    ' ''' <param name="interval">インターバル</param>
    ' ''' <returns>開始時間、終了時間</returns>
    ' ''' <remarks></remarks>
    'Private Function RevisionTime(ByVal startTime As Date, ByVal endTime As Date, ByVal interval As Integer) As Date()

    '    OutputLog(LOG_TYPE_INFO, "[S]RevisionTime", "", Nothing)

    '    Dim dateArray(2) As Date
    '    Dim timeDiff As Integer
    '    Dim startTimeRevision As Date
    '    Dim endTimeRevision As Date

    '    timeDiff = CType(startTime.Minute Mod interval, Integer)
    '    If timeDiff > 0 Then
    '        startTimeRevision = startTime.AddMinutes(interval - timeDiff)
    '    Else
    '        startTimeRevision = startTime
    '    End If
    '    timeDiff = CType(endTime.Minute Mod interval, Integer)
    '    If timeDiff > 0 Then
    '        endTimeRevision = endTime.AddMinutes(interval - timeDiff)
    '    Else
    '        endTimeRevision = endTime
    '    End If

    '    dateArray(0) = startTimeRevision
    '    dateArray(1) = endTimeRevision

    '    OutputLog(LOG_TYPE_INFO, "[E]RevisionTime", "", Nothing)

    '    Return dateArray

    'End Function
    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' DBNullのストール予約情報項目にデフォルト値を設定する
    ''' </summary>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <returns>予約情報</returns>
    ''' <remarks></remarks>
    Private Function SetStallReserveDefaultValue(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable) As SC3150101DataSet.SC3150101StallReserveInfoDataTable

        OutputLog(LOG_TYPE_INFO, "[S]SetStallReserveDefaultValue", "", Nothing, "REZ_INFO:(DataSet)")

        Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        drReserveInfo = DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

        drReserveInfo.DLRCD = SetStringData(drReserveInfo.Item("DLRCD"), "")                                           ' 販売店コード
        drReserveInfo.STRCD = SetStringData(drReserveInfo.Item("STRCD"), "")                                           ' 店舗コード
        drReserveInfo.REZID = SetDecimalNumerData(drReserveInfo.Item("REZID"), 0)                                           ' 予約ID
        drReserveInfo.PREZID = SetDecimalNumerData(drReserveInfo.Item("PREZID"), 0)                                         ' 管理予約ID
        drReserveInfo.STALLID = SetDecimalNumerData(drReserveInfo.Item("STALLID"), 0)                                       ' ストールID
        If IsDBNull(drReserveInfo.Item("STARTTIME")) Then
            drReserveInfo.STARTTIME = DateTime.MinValue                                                                ' 使用開始日時
        End If
        If IsDBNull(drReserveInfo.Item("ENDTIME")) Then
            drReserveInfo.ENDTIME = DateTime.MinValue                                                                  ' 使用終了日時
        End If
        drReserveInfo.REZ_WORK_TIME = SetLongNumerData(drReserveInfo.Item("REZ_WORK_TIME"), 0)                           ' 予定_作業時間
        drReserveInfo.REZ_RECEPTION = SetStringData(drReserveInfo.Item("REZ_RECEPTION"), "0")                          ' 予約_受付納車区分
        drReserveInfo.REZ_PICK_LOC = SetStringData(drReserveInfo.Item("REZ_PICK_LOC"), "")                             ' 予約_取引_場所
        drReserveInfo.REZ_PICK_TIME = SetLongNumerData(drReserveInfo.Item("REZ_PICK_TIME"), 0)                           ' 予約_取引_所要時間
        drReserveInfo.REZ_DELI_LOC = SetStringData(drReserveInfo.Item("REZ_DELI_LOC"), "")                             ' 予約_納車_場所
        drReserveInfo.REZ_DELI_TIME = SetLongNumerData(drReserveInfo.Item("REZ_DELI_TIME"), 0)                           ' 予約_納車_所要時間
        drReserveInfo.STATUS = SetLongNumerData(drReserveInfo.Item("STATUS"), 0)                                         ' ステータス
        If IsDBNull(drReserveInfo.Item("STRDATE")) Then
            drReserveInfo.STRDATE = DateTime.MinValue                                                                  ' 入庫日時
        End If
        drReserveInfo.WASHFLG = SetStringData(drReserveInfo.Item("WASHFLG"), "0")                                      ' 洗車フラグ
        drReserveInfo.INSPECTIONFLG = SetStringData(drReserveInfo.Item("INSPECTIONFLG"), "0")                          ' 検査フラグ
        drReserveInfo.STOPFLG = SetStringData(drReserveInfo.Item("STOPFLG"), "0")                                      ' 中断フラグ
        drReserveInfo.CANCELFLG = SetStringData(drReserveInfo.Item("CANCELFLG"), "0")                                  ' キャンセルフラグ
        drReserveInfo.DELIVERY_FLG = SetStringData(drReserveInfo.Item("DELIVERY_FLG"), "0")                            ' 納車済フラグ
        drReserveInfo.REZCHILDNO = SetLongNumerData(drReserveInfo.Item("REZCHILDNO"), 0)

        drReserveInfo.UPDATE_COUNT = SetLongNumerData(drReserveInfo.Item("UPDATE_COUNT"), 0)                             ' 更新カウント

        OutputLog(LOG_TYPE_INFO, "[E]SetStallReserveDefaultValue", "", Nothing, "RET:REZ_INFO(DataSet)")
        Return reserveInfo

    End Function


    ''' <summary>
    ''' DBNullのストール実績リスト情報項目にデフォルト値を設定する
    ''' </summary>
    ''' <param name="ProcessInfo">ストール実績リスト情報</param>
    ''' <returns>実績情報</returns>
    ''' <remarks></remarks>
    Private Function SetStallProcessListDefaultValue(ByVal ProcessInfo As SC3150101DataSet.SC3150101StallProcessListDataTable) As SC3150101DataSet.SC3150101StallProcessListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]SetStallProcessListDefaultValue", "", Nothing, "PROC_INFO:(DataSet)")

        Dim drProcessInfo As SC3150101DataSet.SC3150101StallProcessListRow

        For Each drProcessInfo In ProcessInfo.Rows
            drProcessInfo.DLRCD = SetStringData(drProcessInfo.Item("DLRCD"), "")
            drProcessInfo.STRCD = SetStringData(drProcessInfo.Item("STRCD"), "")
            drProcessInfo.REZID = SetDecimalNumerData(drProcessInfo.Item("REZID"), 0)
            drProcessInfo.DSEQNO = SetLongNumerData(drProcessInfo.Item("DSEQNO"), 0)
            drProcessInfo.SEQNO = SetDecimalNumerData(drProcessInfo.Item("SEQNO"), 0)
            drProcessInfo.RESULT_STATUS = SetStringData(drProcessInfo.Item("RESULT_STATUS"), "0")
            drProcessInfo.RESULT_STALLID = SetLongNumerData(drProcessInfo.Item("RESULT_STALLID"), 0)
            'drProcessInfo.RESULT_START_TIME = ParseDate(SetStringData(drProcessInfo.Item("RESULT_START_TIME"), ""))
            drProcessInfo.RESULT_START_TIME = SetStringData(drProcessInfo.Item("RESULT_START_TIME"), "")
            drProcessInfo.RESULT_END_TIME = SetStringData(drProcessInfo.Item("RESULT_END_TIME"), "")
            drProcessInfo.RESULT_WORK_TIME = SetLongNumerData(drProcessInfo.Item("RESULT_WORK_TIME"), 0)
            drProcessInfo.RESULT_IN_TIME = SetStringData(drProcessInfo.Item("RESULT_IN_TIME"), "")
            drProcessInfo.RESULT_WASH_START = SetStringData(drProcessInfo.Item("RESULT_WASH_START"), "")
            drProcessInfo.RESULT_WASH_END = SetStringData(drProcessInfo.Item("RESULT_WASH_END"), "")
            drProcessInfo.RESULT_INSPECTION_START = SetStringData(drProcessInfo.Item("RESULT_INSPECTION_START"), "")
            drProcessInfo.RESULT_INSPECTION_END = SetStringData(drProcessInfo.Item("RESULT_INSPECTION_END"), "")
            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            'drProcessInfo.RESULT_WAIT_START = SetStringData(drProcessInfo.Item("RESULT_WAIT_START"), "") 
            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
            drProcessInfo.RESULT_WAIT_END = SetStringData(drProcessInfo.Item("RESULT_WAIT_END"), "")
        Next

        OutputLog(LOG_TYPE_INFO, "[E]SetStallProcessListDefaultValue", "", Nothing, _
                  "RET:PROC_INFO(DataSet)")
        Return ProcessInfo

    End Function


    ''' <summary>
    ''' YYYYMMDDHHMMの形式の文字列をDateTime型に変換する
    ''' </summary>
    ''' <param name="Value">YYYYMMDDHHMMの形式の文字列</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function SetStallTime(ByVal value As String) As Date

        'Logger.Info("[S]SetStallTime()")
        OutputLog(LOG_TYPE_INFO, "[S]SetStallTime", "", Nothing, "DATE:" & value)

        '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 START
        Dim userContext As StaffContext = StaffContext.Current
        Dim ret As Date = DateTimeFunc.Now(userContext.DlrCD)
        'Dim ret As Date = DateTime.Now
        '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 END
        Dim hour As Integer
        Dim minute As Integer
        Dim retValue As Date

        If IsDBNull(value) Then 'If IsDBNull(value) = True Then
            OutputLog(LOG_TYPE_INFO, "[E]SetStallTime", "", Nothing, _
                      "RET:" & DateTime.MinValue.ToString(CultureInfo.InvariantCulture()))
            Return DateTime.MinValue
        End If

        If String.IsNullOrWhiteSpace(value) Then 'If value.Trim() = "" Then
            OutputLog(LOG_TYPE_INFO, "[E]SetStallTime", "", Nothing, _
                      "RET:" & DateTime.MinValue.ToString(CultureInfo.InvariantCulture()))
            Return DateTime.MinValue
        End If

        hour = CType(value.Substring(0, 2), Integer)
        minute = CType(value.Substring(3, 2), Integer)

        retValue = New DateTime(ret.Year, ret.Month, ret.Day, hour, minute, 0)

        'Logger.Info("[E]SetStallTime()")
        OutputLog(LOG_TYPE_INFO, "[E]SetStallTime", "", Nothing, _
                  "RET:" & retValue.ToString(CultureInfo.InvariantCulture()))
        Return retValue

    End Function


    ''' <summary>
    ''' 日時変換
    ''' </summary>
    ''' <param name="value">日付文字列</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function ParseDate(ByVal Value As String) As Date
        ' Protected

        OutputLog(LOG_TYPE_INFO, "[S]ParseDate", "", Nothing, "DATE:" & Value)

        Dim ret As Date

        Dim year As Integer = Integer.Parse(Value.Substring(0, 4), CultureInfo.InvariantCulture())
        Dim month As Integer = Integer.Parse(Value.Substring(4, 2), CultureInfo.InvariantCulture())
        Dim day As Integer = Integer.Parse(Value.Substring(6, 2), CultureInfo.InvariantCulture())
        Dim hour As Integer = Integer.Parse(Value.Substring(8, 2), CultureInfo.InvariantCulture())
        Dim minute As Integer = Integer.Parse(Value.Substring(10, 2), CultureInfo.InvariantCulture())

        ret = New Date(year, month, day, hour, minute, 0)

        OutputLog(LOG_TYPE_INFO, "[E]ParseDate", "", Nothing, "RET:" & CType(ret, String))
        Return ret

    End Function


    ''' <summary>
    ''' 日付を表す文字列からDateTimeを生成し返却する
    ''' 引数から年・月・日として妥当な値を取得できない場合、結果は保証しない
    ''' </summary>
    ''' <param name="YYYYMMDDText">時刻を表す文字列"YYYYMMDD"</param>
    ''' <returns>日付を表すDateTime(時刻は持たない)</returns>
    ''' <remarks></remarks>
    Private Function YYYYMMDDTextToDateTime(ByVal YYYYMMDDText As String) As Date

        OutputLog(LOG_TYPE_INFO, "[S]YYYYMMDDTextToDateTime", "", Nothing, "DATE:" & YYYYMMDDText)

        Dim ret As Date
        Dim y As Integer
        Dim m As Integer
        Dim d As Integer


        y = Integer.Parse(YYYYMMDDText.Substring(0, 4), CultureInfo.InvariantCulture())
        m = Integer.Parse(YYYYMMDDText.Substring(4, 2), CultureInfo.InvariantCulture())
        d = Integer.Parse(YYYYMMDDText.Substring(6, 2), CultureInfo.InvariantCulture())

        ret = New DateTime(y, m, d, 0, 0, 0)

        OutputLog(LOG_TYPE_INFO, "[E]YYYYMMDDTextToDateTime", "", Nothing, _
                  "RET:" & ret.ToString(CultureInfo.InvariantCulture()))
        Return ret

    End Function


    ''' <summary>
    ''' 時刻を表す文字列からDateTimeを生成し返却する
    ''' 引数から時・分として妥当な値を取得できない場合、結果は保証しない
    ''' </summary>
    ''' <param name="HHMMText">時刻を表す文字列"HHMM" or "HH:MM"</param>
    ''' <returns>時刻を表すDateTime(日は持たない)</returns>
    ''' <remarks></remarks>
    Private Function HHMMTextToDateTime(ByVal HHMMText As String) As Date

        OutputLog(LOG_TYPE_INFO, "[S]HHMMTextToDateTime", "", Nothing, "DATE:" & HHMMText)

        Dim retDate As Date
        Dim hours As Integer
        Dim minutes As Integer


        hours = Integer.Parse(HHMMText.Substring(0, 2), CultureInfo.InvariantCulture())
        If HHMMText.Length = 4 Then
            minutes = Integer.Parse(HHMMText.Substring(2, 2), CultureInfo.InvariantCulture())
        ElseIf HHMMText.Length = 5 Then
            minutes = Integer.Parse(HHMMText.Substring(3, 2), CultureInfo.InvariantCulture())
        Else
            OutputLog(LOG_TYPE_INFO, "[E]HHMMTextToDateTime", "Argument error", Nothing)
            Throw New ArgumentException("Argument is character string except HHMM or HH:MM")
        End If

        retDate = New DateTime(1, 1, 1, hours, minutes, 0)

        OutputLog(LOG_TYPE_INFO, "[E]HHMMTextToDateTime", "", Nothing, _
                  "RET:" & retDate.ToString(CultureInfo.InvariantCulture()))
        Return retDate

    End Function

    ''' <summary>
    ''' オブジェクトの文字列値を取得し返却する
    ''' </summary>
    ''' <param name="obj">DBから取得した文字列 or DBNull</param>
    ''' <returns>文字列。DBNullの場合、空文字列</returns>
    ''' <remarks></remarks>
    Private Function StringValueOfDB(ByVal obj As Object) As String
        If Convert.IsDBNull(obj) Then
            Return String.Empty
        End If
        Return CType(obj, String)
    End Function


    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetStringData(ByVal src As Object, ByVal defult As String) As String

        Dim returnValue As String

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, String)
        End If

        Return returnValue

    End Function


    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetLongNumerData(ByVal src As Object, ByVal defult As Integer) As Long

        Dim returnValue As Long

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, Long)
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetDecimalNumerData(ByVal src As Object, ByVal defult As Integer) As Decimal

        Dim returnValue As Decimal

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, Decimal)
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' boolean値に変換する
    ''' 1：true、それ以外：false
    ''' </summary>
    ''' <param name="breakFlg">フラグ</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function convertBoolean(ByVal breakFlg As String) As Boolean

        If breakFlg.Equals("1") Then 'If breakFlg = "1" Then
            Return True
        Else
            Return False
        End If

    End Function

    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 START
    ''' <summary>
    '''　TCステータスモニター起動までの待機時間の取得
    ''' </summary>
    ''' <returns>リフレッシュタイム</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/02/26 TMEJ 成澤 【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成
    ''' </History>
    Public ReadOnly Property GetTcStatusStandTime() As SC3150101DataSet.SC3150101TcStatusStandTimeDataTable
        Get
            Logger.Info("GetTcStatusStandTime  Start")

            Dim dt As SC3150101DataSet.SC3150101TcStatusStandTimeDataTable
            Dim userContext As StaffContext = StaffContext.Current

            'データセットの呼び出し
            Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

                'ストール情報を取得、データセットに格納
                dt = adapter.GetTcStatusStandTime(userContext.DlrCD, userContext.BrnCD)

            End Using

            Return dt

            Logger.Info("GetTcStatusStandTime End")
        End Get

    End Property
    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 END

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
    ''' <summary>
    ''' 休憩取得ボタン押下判定
    ''' </summary>
    ''' <param name="isBreak">休憩有無</param>
    ''' <param name="isBreakBottom">休憩取得判定ボタン押下有無</param>
    ''' <param name="InRestFlg">休憩取得フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BreakBottomClickCheck(ByVal isBreak As Boolean, _
                                           ByVal isBreakBottom As Boolean, _
                                           ByVal InRestFlg As String) As String
        Logger.Info("BreakBottomClickCheck  Start")

        '返却用変数
        Dim restFlg As String = InRestFlg

        '休憩取得判定ボタン押下した場合
        If isBreakBottom Then

            If isBreak Then
                '休憩取得を選択した場合
                restFlg = REST_FLG_TAkE
            Else
                '休憩取得しないを選択した場合
                restFlg = REST_FLG_NO_TAkE
            End If
        End If

        Logger.Info("BreakBottomClickCheck END ")
        Return restFlg
    End Function
    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 全リレーションのROステータスの取得
    ''' </summary>
    ''' <param name="jobDetailId">作業内容</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRepairOrderStatus(ByVal jobDetailId As Decimal, _
                                         ByVal dealerCode As String, _
                                         ByVal branchCode As String) As SC3150101DataSet.SC3150101RepairOrderStatusDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dtRepairOrderStatus As SC3150101DataSet.SC3150101RepairOrderStatusDataTable

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '全リレーションのROステータスを取得
            dtRepairOrderStatus = adapter.GetRepairOrderStatus(jobDetailId, dealerCode, branchCode)

        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} END" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtRepairOrderStatus
    End Function

    ''' <summary>
    ''' 完成検査画面連携引数の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="jobDetailId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CompletionScreenLinkageInfo(ByVal dealerCode As String, _
                                                ByVal branchCode As String, _
                                                ByVal jobDetailId As Decimal) As SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'データテーブル宣言
        Dim dtRepairOrderStatus As SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable
        'インスタンス宣言
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '完成検査画面連携引数を取得
            dtRepairOrderStatus = adapter.GetTechnicianScreenLinkageInfo(dealerCode, branchCode, jobDetailId)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} END" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtRepairOrderStatus
    End Function

    ''' <summary>
    ''' 他システムの戻り値からエラーコードを判断する
    ''' </summary>
    ''' <param name="inReturnCode">tabletSmbからの戻り値</param>
    ''' <param name="workFlg">開始・日跨ぎ・終了の判断フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function OtherSystemsReturnCodeSelect(ByVal inReturnCode As Long, _
                                                  Optional ByVal workFlg As Integer = 4) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} START. RETURN_CODE{2},WORK_FLG{3}." _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , inReturnCode.ToString(CultureInfo.CurrentCulture()) _
               , workFlg.ToString(CultureInfo.CurrentCulture())))

        '戻り値
        Dim returnValue As Integer = 0

        Select Case inReturnCode
            Case ActionResult.CheckError
                'オペレーション毎にエラー文言設定
                returnValue = ErrorWordingOfEachOperation(workFlg, ActionResult.CheckError)
            Case ActionResult.OutOfWorkingTimeError
                returnValue = 914
                '開始できませんでした。ストールの稼動時間外です。
            Case ActionResult.NotSetJobSvcClassIdError
                returnValue = 941
                'SMBでサービス区分を入力するよう、コントローラーにお知らせください。
            Case ActionResult.HasWorkingChipInOneStallError
                returnValue = 916
                '開始できませんでした。すでに作業中のチップがあります。
            Case ActionResult.OverlapUnavailableError
                returnValue = 937
                '使用不可チップを他のチップの上に重複させることができません。

                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            Case ActionResult.ChipOverlapUnavailableError
                returnValue = 945
                'ストールは使用不可。使用不可チップを移動するか、作業チップを別ストールに配置するようにして下さい。
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            Case ActionResult.RowLockVersionError
                returnValue = 930
                '該当データが存在しません。画面を再表示し、情報を最新化します。
            Case ActionResult.LockStallError
                returnValue = 923
                'そのチップは、既に他のオペレータによって変更が加えられています。画面を再表示してから再度処理を行ってください。
            Case ActionResult.DBTimeOutError
                returnValue = 929
                'データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。
            Case ActionResult.DmsLinkageError
                returnValue = 928
                '他システムとの連携時にエラーが発生しました。システム管理者に連絡してください。
            Case ActionResult.InspectionStatusFinishError
                If workFlg = workMidFinishFlg Then
                    returnValue = 919
                    '当日処理ができませんでした。選択チップは既に完成検査依頼済みです。
                ElseIf workFlg = workFinishFlg Then
                    returnValue = 925
                    '作業終了できませんでした。選択チップは既に完成検査依頼済みです。
                End If
            Case ActionResult.ParentroNotStartedError
                returnValue = 921
                '開始できませんでした。親R/Oが開始されていません。
            Case ActionResult.NoTechnicianError
                'オペレーション毎にエラー文言設定
                returnValue = ErrorWordingOfEachOperation(workFlg, ActionResult.NoTechnicianError)
            Case ActionResult.NoJobResultDataError
                returnValue = 932
                '作業開始されていないJobが存在するため、チップを作業終了できません。

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                returnValue = ActionResult.WarningOmitDmsError
                'このチップはDMSによって不適切に完了されています。システム内の記録された時間は、実際の作業時間ではありません。

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Case IC3802503BusinessLogic.Result.TimeOutError
                ' タイムアウトエラー
                returnValue = 926
            Case IC3802503BusinessLogic.Result.DmsError
                ' 基幹側のエラー
                returnValue = 927
            Case IC3802503BusinessLogic.Result.OtherError
                ' その他のエラー
                returnValue = 928
            Case Else
                'オペレーション毎にエラー文言設定
                returnValue = ErrorWordingOfEachOperation(workFlg, 0)
        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} END" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return returnValue
    End Function

    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 同一エラーのオペレーション毎のエラー文言設定
    ''' </summary>
    ''' <param name="operationType">オペレーションタイプ</param>
    ''' <param name="errorType">エラータイプ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ErrorWordingOfEachOperation(ByVal operationType As Integer,
                                                 ByVal errorType As Integer) As Integer

        '戻り値
        Dim returnCode As Integer = 0

        If errorType = ActionResult.CheckError Then
            'ステータスチェックエラー
            Select Case operationType
                Case workStartFlg
                    returnCode = 906
                    '開始できませんでした。
                Case workMidFinishFlg
                    returnCode = 918
                    '当日処理ができませんでした。選択チップが作業中ではありません。
                Case workFinishFlg
                    returnCode = 924
                    '作業終了できませんでした。選択チップが作業中ではありません。
                Case workStopFlg
                    returnCode = 936
                    '中断できませんでした。
            End Select
        ElseIf errorType = ActionResult.NoTechnicianError Then
            '作業担当者不在エラー
            Select Case operationType
                Case workStartFlg
                    returnCode = 917
                    '開始できませんでした。作業担当者が存在しません。
                Case workMidFinishFlg
                    returnCode = 934
                    '終了できませんでした。作業担当者が存在しません。
                Case workFinishFlg
                    returnCode = 934
                    '終了できませんでした。作業担当者が存在しません。
                Case workStopFlg
            End Select

        Else
            '例外エラー
            Select Case operationType
                Case workStartFlg
                    returnCode = 906
                    '開始できませんでした。
                Case workMidFinishFlg
                    returnCode = 907
                    '当日処理ができませんでした。
                Case workFinishFlg
                    returnCode = 931
                    '終了できませんでした。
                Case workStopFlg
                    returnCode = 936
                    '中断できませんでした。
            End Select

        End If

        Return returnCode

    End Function

    ''' <summary>
    ''' 中断メモテンプレート取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStopMemoTemplate(ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String) As SC3150101DataSet.SC3150101StopMemoTempDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} START. DLR_CD:{2}, BRN_CD:{3}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , inDealerCode _
                             , inBranchCode _
                             ))

        '返却値
        Dim dtRepairOrderSeq As SC3150101DataSet.SC3150101StopMemoTempDataTable = Nothing

        '中断メモテンプレート取得する
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            dtRepairOrderSeq = adapter.GetStopMemoTemplate(inDealerCode, inBranchCode)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtRepairOrderSeq

    End Function

    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END 

#Region "通知&Push送信"

#Region "Push用定数"

    ''' <summary>
    ''' 削除されていないユーザ(delflg=0)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlgNone As String = "0"
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
    ''' 通知履歴のSessionValue(文字列)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueString As String = "String"

    ''' <summary>
    ''' 通知履歴のSessionValue(販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDealerCode As String = "Session.Param1,"

    ''' <summary>
    ''' 通知履歴のSessionValue(店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBranchCode As String = "Session.Param2,"

    ''' <summary>
    ''' 通知履歴のSessionValue(スタッフコード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueLoginUserID As String = "Session.Param3,"

    ''' <summary>
    ''' 通知履歴のSessionValue(SAチップID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSAChipID As String = "Session.Param4,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹作業内容ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBasRezId As String = "Session.Param5,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueRepiarOrder As String = "Session.Param6,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueRepiarOrderSeq As String = "Session.Param7,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVinNo As String = "Session.Param8,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「0：編集」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueViewMode As String = "Session.Param9,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「0：プレビュー」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueFormat As String = "Session.Param10,"
    ''' <summary>
    ''' 通知履歴のSessionValue(画面番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDisplayNumber As String = "Session.DISP_NUM,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsCustomerCode As String = "SessionKey.DMS_CST_CD,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVin As String = "SessionKey.VIN,"
    ''' <summary>
    ''' 基幹顧客IDの分解文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUNCTUATION_STRING As String = "@"

    ''' <summary>
    ''' 13：ROプレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_13 As String = "13"
    ''' <summary>
    ''' 0：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ViewMode As String = "0"
    ''' <summary>
    ''' 0：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatPreview As String = "0"
    ''' <summary>
    ''' 通知履歴のSessionValue(顧客詳細フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueType As String = "Redirect.FLAG,String,"
    ''' <summary>
    ''' ROプレビューリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const repiarOrderPreviewLink As String = "<a id='SC30105010' Class='SC3010501' href='/Website/Pages/SC3010501.aspx' onclick='return ServiceLinkClick(event)'>"
    ''' <summary>
    ''' 顧客詳細リンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDetailLink As String = "<a id='SC30802251' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"
    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

#End Region

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="jobDatilId">RO番号</param>
    ''' <param name="braunchCode" >販売店コード</param>
    ''' <param name="dealerCode">店舗コード</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <remarks></remarks>
    Public Sub NoticeMainProcessing(ByVal jobDatilId As Decimal, _
                                    ByVal dealerCode As String, _
                                    ByVal braunchCode As String, _
                                    ByVal inStaffInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dtVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable

        Using adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                       New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            dtVisitInfo = adapter.GetTechnicianScreenLinkageInfo(dealerCode, braunchCode, jobDatilId)
        End Using

        If (dtVisitInfo.Rows.Count > 0) AndAlso
            (Not dtVisitInfo(0).IsSACODENull) AndAlso
            (Not String.IsNullOrEmpty(dtVisitInfo(0).SACODE)) Then

            Dim drVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow = _
                DirectCast(dtVisitInfo.Rows(0), SC3150101DataSet.SC3150101ScreenLinkageInfoRow)


            '送信先アカウント情報設定
            Dim account As XmlAccount = Me.CreateAccount(drVisitInfo)

            '通知履歴登録情報の設定
            Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(drVisitInfo, inStaffInfo)

            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(drVisitInfo)

            Dim userContext As StaffContext = StaffContext.Current

            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData

                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(userContext.DlrCD)
                '送信ユーザーデータ格納
                noticeData.AccountList.Add(account)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using
            End Using
        End If
        dtVisitInfo.Dispose()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 送信先アカウント情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow) As XmlAccount

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using account As New XmlAccount

            Dim usersClass As New Users

            Dim rowUsers As UsersDataSet.USERSRow

            If (Not inRowVisitInfo.IsSACODENull) AndAlso _
                (Not String.IsNullOrEmpty(inRowVisitInfo.SACODE)) Then

                'SACODEでユーザー情報の取得
                rowUsers = usersClass.GetUser(inRowVisitInfo.SACODE, DelFlgNone)

                '受信先のアカウント設定
                account.ToAccount = rowUsers.ACCOUNT

                '受信者名設定
                account.ToAccountName = rowUsers.USERNAME
            End If
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return account

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <param name="inStaffInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow, _
                                         ByVal inStaffInfo As StaffContext) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD
            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD
            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account
            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName
            '表示内容設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowVisitInfo)
            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowVisitInfo, inStaffInfo)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return requestNotice
        End Using
    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報表示欄</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow, _
                                                ByVal inStaffInfo As StaffContext) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'インスタンスの宣言
        Using serviceCommon As New ServiceCommonClassBusinessLogic

            '基幹情報の取得
            Using dtServiceCommon As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                 serviceCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                                DmsCodeType.BranchCode, _
                                                inStaffInfo.DlrCD, _
                                                inStaffInfo.BrnCD, _
                                                Nothing, _
                                                inStaffInfo.Account)

                'ロウに格納
                Dim drServiceCommon As ServiceCommonClassDataSet.DmsCodeMapRow = _
                     DirectCast(dtServiceCommon.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '販売店コードのセッション設定
                Me.SetSessionValueWord(workSession, SessionValueDealerCode, drServiceCommon.CODE1)

                '販売店コードのセッション値設定
                Me.SetSessionValueWord(workSession, SessionValueBranchCode, drServiceCommon.CODE2)

                'ログインスタッフコードのセッション値設定
                Me.SetSessionValueWord(workSession, SessionValueLoginUserID, drServiceCommon.ACCOUNT)
            End Using
        End Using

        'VINの設定
        If Not inRowVisitInfo.IsVINNull Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

        End If

        '訪問連番の設定
        If Not inRowVisitInfo.IsVISITSEQNull Then
            '訪問連番がある場合は設定

            '訪問連番のセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSAChipID, inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture()))

        End If

        '基幹作業内容IDの設定
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '基幹作業内容IDがある場合は設定

            '基幹作業内容IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBasRezId, inRowVisitInfo.DMS_JOB_DTL_ID)

        End If

        'RO番号の設定
        If Not inRowVisitInfo.IsRO_NUMNull Then
            'RO番号がある場合は設定

            'RO番号のセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueRepiarOrder, inRowVisitInfo.RO_NUM)

        End If

        'RO連番の設定
        If Not inRowVisitInfo.IsRO_SEQNull Then
            '予約IDがある場合は設定

            '予約IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueRepiarOrderSeq, inRowVisitInfo.RO_SEQ.ToString(CultureInfo.CurrentCulture))
        End If

        'ROプレビュー設定値(ViewMode)のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueViewMode, ViewMode)
        'ROプレビュー設定値(Format)のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueFormat, FormatPreview)
        'ROプレビュー画面番号のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueDisplayNumber, DISPLAY_NUMBER_13)

        '別画面のセッションに切り替え
        workSession.Append(vbTab)

        '基幹顧客IDの設定
        If Not inRowVisitInfo.IsDMS_CST_CDNull Then

            '文字列の分割位置の取得
            Dim StringIndex As Integer = inRowVisitInfo.DMS_CST_CD.IndexOf(PUNCTUATION_STRING, StringComparison.CurrentCulture)
            Dim stringDmsCustomerCode As String = inRowVisitInfo.DMS_CST_CD

            If StringIndex > 0 Then
                '入庫管理番号を分割し、RO番号の部分を取得
                stringDmsCustomerCode = inRowVisitInfo.DMS_CST_CD.Substring(StringIndex + 1)
            End If

            '顧客詳細セッション「基幹顧客ID」設定
            Me.SetSessionValueWord(workSession, SessionValueDmsCustomerCode, stringDmsCustomerCode, False)

        End If

        'VINの設定
        If Not inRowVisitInfo.IsVINNull Then
            'VINがある場合は設定

            '顧客詳細セッション「Vin」設定
            Me.SetSessionValueWord(workSession, SessionValueVin, inRowVisitInfo.VIN)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' SessionValue文字列作成
    ''' </summary>
    ''' <param name="workSession">追加元文字列</param>
    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ''' <param name="SessionValueData">追加するデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
                                         ByVal SessionValueWord As String, _
                                         ByVal SessionValueData As String, _
                                         Optional ByVal switchFlg As Boolean = True) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If (switchFlg) AndAlso (workSession.Length <> 0) Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)
        '文字列型式に設定
        workSession.Append(SessionValueString)
        '「,」を結合する
        workSession.Append(SessionValueKanma)
        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'PUSH内容設定
        Using pushInfo As New XmlPushInfo

            'カテゴリータイプ設定
            pushInfo.PushCategory = NotifyPushCategory
            '表示位置設定
            pushInfo.PositionType = NotifyPotisionType
            '表示時間設定
            pushInfo.Time = NotifyTime
            '表示タイプ設定
            pushInfo.DisplayType = NotifyDispType
            '表示内容設定
            pushInfo.DisplayContents = Me.CreatePusuMessage(inRowVisitInfo)
            '色設定
            pushInfo.Color = NotifyColor
            '表示時関数設定
            pushInfo.DisplayFunction = NotifyDispFunction

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return pushInfo

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workContents As New StringBuilder
        '文言「お客様」を設定
        Dim customerWording As String = WebWordUtility.GetWord(APPLICATION_ID, 30)
        '文言「整備完了」を設定
        workContents.Append(WebWordUtility.GetWord(APPLICATION_ID, 28))

        'メッセージ組立：RO番号
        If Not (inRowVisitInfo.IsRO_NUMNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.RO_NUM)) Then
            'RO番号がある場合

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))

            'ROプレビューリンクのAタグを設定
            workContents.Append(repiarOrderPreviewLink)

            'RO番号を設定
            workContents.Append(inRowVisitInfo.RO_NUM)

            'Aタグ終了を設定
            workContents.Append(EndLikTag)
        End If

        'メッセージ間にスペースの設定
        workContents.Append(Space(3))

        '基幹顧客IDがある場合
        If Not (inRowVisitInfo.IsDMS_CST_CDNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.DMS_CST_CD)) Then


            '顧客詳細リンクのAタグを設定
            workContents.Append(CustomerDetailLink)
        End If

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workContents.Append(inRowVisitInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))
        End If

        'メッセージ組立：名前
        '名前の確認
        If Not (inRowVisitInfo.IsCST_NAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.CST_NAME)) Then
            '名前がある場合

            '敬称と配置区分の確認
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAMETITLE_NAME)) _
                AndAlso Not (inRowVisitInfo.IsPOSITION_TYPENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.POSITION_TYPE)) Then

                '敬称の配置判断
                If inRowVisitInfo.POSITION_TYPE.Equals("1") Then

                    '名前＋「様」を設定
                    workContents.Append(inRowVisitInfo.CST_NAME)
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)

                ElseIf inRowVisitInfo.POSITION_TYPE.Equals("2") Then

                    '「様」＋名前を設定
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workContents.Append(inRowVisitInfo.CST_NAME)
                End If

            Else
                '敬称を付けずに顧客氏名のみ表示
                workContents.Append(inRowVisitInfo.CST_NAME)
            End If

        Else
            '顧客氏名がNULLの場合、文言「お客様」を表示
            workContents.Append(customerWording)
        End If

        'Aタグ終了を設定
        workContents.Append(EndLikTag)

        '戻り値設定
        Dim notifyMessage As String = workContents.ToString()

        '開放処理
        workContents = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' Push用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreatePusuMessage(ByVal inRowVisitInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workContents As New StringBuilder
        '文言「お客様」を設定
        Dim customerWording As String = WebWordUtility.GetWord(APPLICATION_ID, 30)
        '文言「整備完了」を設定
        workContents.Append(WebWordUtility.GetWord(APPLICATION_ID, 28))

        'メッセージ組立：RO番号
        If Not (inRowVisitInfo.IsRO_NUMNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.RO_NUM)) Then
            'RO番号がある場合

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))

            'RO番号を設定
            workContents.Append(inRowVisitInfo.RO_NUM)

        End If

        'メッセージ間にスペースの設定
        workContents.Append(Space(3))

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workContents.Append(inRowVisitInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))
        End If

        'メッセージ組立：名前
        '名前の確認
        If Not (inRowVisitInfo.IsCST_NAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.CST_NAME)) Then
            '名前がある場合

            '敬称と配置区分の確認
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAMETITLE_NAME)) _
                AndAlso Not (inRowVisitInfo.IsPOSITION_TYPENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.POSITION_TYPE)) Then

                '敬称の配置判断
                If inRowVisitInfo.POSITION_TYPE.Equals("1") Then

                    '名前＋「様」を設定
                    workContents.Append(inRowVisitInfo.CST_NAME)
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)

                ElseIf inRowVisitInfo.POSITION_TYPE.Equals("2") Then

                    '「様」＋名前を設定
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workContents.Append(inRowVisitInfo.CST_NAME)
                End If

            Else
                '敬称を付けずに顧客氏名のみ表示
                workContents.Append(inRowVisitInfo.CST_NAME)
            End If

        Else
            '顧客氏名がNULLの場合、文言「お客様」を表示
            workContents.Append(customerWording)
        End If

        '戻り値設定
        Dim pushMessage As String = workContents.ToString()

        '開放処理
        workContents = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return pushMessage

    End Function

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
    ''' <summary>
    ''' 作業開始時のPush処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub WorkStartSendPush(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal staffCode As String, _
                                 ByVal stallId As Decimal, _
                                 ByVal stallUseId As Decimal)

        'スタッフ情報の取得
        Dim stuffCodeList As New List(Of Decimal)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        'stuffCodeList.Add(OperationCodeCT)
        'stuffCodeList.Add(OperationCodeChT)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
        stuffCodeList.Add(OperationCodeSA)
        stuffCodeList.Add(OperationCodePS)
        'stuffCodeList.Add(OperationCodeFM)

        'アダプターのインスタンス宣言
        Using adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                        New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '================================================================
            '         　　　　　　　SA通知送信処理
            '================================================================

            'データセット宣言　
            Dim dtFirstWorkChip As SC3150101DataSet.SC3150101FirstWorkChipDataTable

            '担当SAコードと最初の開始チップの作業内容ID取得
            dtFirstWorkChip = adapter.GetFirstWorkChip(dealerCode, branchCode, stallUseId)

            If dtFirstWorkChip.Rows.Count > 0 Then

                Dim drFirstWorkChip As SC3150101DataSet.SC3150101FirstWorkChipRow =
                    DirectCast(dtFirstWorkChip.Rows(0), SC3150101DataSet.SC3150101FirstWorkChipRow)

                '開始したチップがリレーション内の最初開始の場合、通知する
                If (Not drFirstWorkChip.IsPIC_SA_STF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drFirstWorkChip.PIC_SA_STF_CD)) AndAlso _
                    (stallUseId = drFirstWorkChip.JOB_DTL_ID) Then

                    'SA権限の場合
                    TransmissionForCall(drFirstWorkChip.PIC_SA_STF_CD, saRefreshFunction)

                End If
            End If

            dtFirstWorkChip.Dispose()
            '================================================================
            '         　　　　　　　CT・ChT通知送信処理
            '================================================================
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            'SendPushChiefTechnician(staffCode, stallId)
            SendPushCtChtToStall(dealerCode, branchCode, staffCode, stallId)
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            '================================================================
            '         　　　　　　　PS通知送信処理
            '================================================================

            'オンラインユーザー情報の取得
            Dim utility As New VisitUtilityBusinessLogic
            Dim sendPushUsers As VisitUtilityUsersDataTable = _
                utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
            utility = Nothing

            '通知命令の送信
            For Each userRow As VisitUtilityUsersRow In sendPushUsers
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'If userRow.OPERATIONCODE.Equals(OperationCodeCT) Then
                '    'CT権限の場合
                '    TransmissionForCall(userRow.ACCOUNT, smbRefreshFunction)
                'ElseIf userRow.OPERATIONCODE.Equals(OperationCodePS) Then
                '    'PS権限の場合
                '    TransmissionForCall(userRow.ACCOUNT, psRefreshFunction)
                'End If

                If userRow.OPERATIONCODE.Equals(OperationCodePS) Then
                    'PS権限の場合
                    TransmissionForCall(userRow.ACCOUNT, psRefreshFunction)
                End If
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            Next

        End Using
    End Sub

    ''' <summary>
    ''' 作業終了時のPush処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="repairOrderNumber">RO番号</param>
    ''' <param name="jobDetailId">作業内容ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Public Sub WorkEndSendPush(ByVal dealerCode As String, _
                               ByVal branchCode As String, _
                               ByVal staffCode As String, _
                               ByVal repairOrderNumber As String, _
                               ByVal jobDetailId As Decimal, _
                               ByVal stallId As Decimal)

        'スタッフ情報の取得
        Dim stuffCodeList As New List(Of Decimal)

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

        'stuffCodeList.Add(OperationCodeCT)

        Dim utility As New VisitUtilityBusinessLogic

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

        'アダプターのインスタンス宣言
        Using adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                        New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '================================================================
            '         　　　　　　　SA通知送信処理
            '================================================================

            'データセット宣言
            Dim dtGatLastWorkChip As SC3150101DataSet.SC3150101GetLastWorkChipDataTable

            '最後の作業チップと着工指示フラグの立っていない整備数の取得
            dtGatLastWorkChip = adapter.GetLastWorkChip(dealerCode, branchCode, repairOrderNumber)

            If dtGatLastWorkChip.Rows.Count > 0 Then

                'データロウの宣言、データセットの格納
                Dim drGatLastWorkChip As SC3150101DataSet.SC3150101GetLastWorkChipRow = _
                    DirectCast(dtGatLastWorkChip.Rows(0), SC3150101DataSet.SC3150101GetLastWorkChipRow)

                '着工指示フラグが全て立っており、終了するチップが最後作業の場合、通知する
                If (Not drGatLastWorkChip.IsPIC_SA_STF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drGatLastWorkChip.PIC_SA_STF_CD)) AndAlso _
                    (drGatLastWorkChip.NO_FLG_COUNT = 0) AndAlso _
                    (jobDetailId = drGatLastWorkChip.JOB_DTL_ID) Then

                    'SA権限の場合
                    TransmissionForCall(drGatLastWorkChip.PIC_SA_STF_CD, saRefreshFunction)
                End If

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                'サービスステータスのチェック
                If ServiceStetus_WaitingWashing.Equals(drGatLastWorkChip.SVC_STATUS) Then
                    '「07：洗車待ち」の場合

                    'CW権限を設定
                    stuffCodeList.Add(Operation.CW)

                    'オンラインのCW権限リスト取得
                    Dim dtSendCWUser As VisitUtilityUsersDataTable = utility.GetOnlineUsers(dealerCode, _
                                                                                            branchCode, _
                                                                                            stuffCodeList)

                    '取得件数分リフレッシュPushをする
                    For Each drSendCWUser As VisitUtilityUsersRow In dtSendCWUser
                        Me.TransmissionForCall(drSendCWUser.ACCOUNT, CWRefreshFunction)

                    Next

                    '権限リスト初期化
                    stuffCodeList.Clear()

                End If

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

            End If

            dtGatLastWorkChip.Dispose()

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            '================================================================
            '         　　　　　　　CT/ChT通知送信処理
            '================================================================
            If NeedPushSubAreaRefresh Then
                ' サブエリアリフレッシュフラグがTureの場合
                ' すべてのユーザーにPush通知を送信する
                SendPushCtChtAll(dealerCode, branchCode, staffCode)
            Else
                ' サブエリアリフレッシュフラグがFalseの場合
                ' 対象ストールに紐づくユーザーのみPush通知を送信する
                SendPushCtChtToStall(dealerCode, branchCode, staffCode, stallId)
            End If

            ''================================================================
            ''         　　　　　　　ChT通知送信処理
            ''================================================================

            'SendPushChiefTechnician(staffCode, stallId)

            ''================================================================
            ''         　　　　　　　CT通知送信処理
            ''================================================================

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            ''CT権限を設定
            'stuffCodeList.Add(OperationCodeCT)

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

            ''オンラインユーザー情報の取得

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            ''Dim utility As New VisitUtilityBusinessLogic

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            'Dim sendPushUsers As VisitUtilityUsersDataTable = _
            'utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
            'utility = Nothing

            ''来店通知命令の送信
            'For Each userRow As VisitUtilityUsersRow In sendPushUsers
            '    If userRow.OPERATIONCODE.Equals(OperationCodeCT) Then
            '        'CT権限の場合
            '        TransmissionForCall(userRow.ACCOUNT, smbRefreshFunction)
            '    End If
            'Next
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        End Using

    End Sub

    ''' <summary>
    ''' チーフテクニシャンにPush送信処理
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Private Sub SendPushChiefTechnician(ByVal staffCode As String, ByVal stallId As Decimal)

        Using adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                       New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            'データセット宣言　
            Dim dtChtAccount As SC3150101DataSet.SC3150101ChtStaffCodeDataTable

            'チーフテクニシャンのスタッフコード取得　
            dtChtAccount = adapter.GetChtTechnicianAccount(stallId)

            '取得したチーフテクニシャン分繰り返す
            For Each drChtAccount As SC3150101DataSet.SC3150101ChtStaffCodeRow In dtChtAccount

                '自分以外のチーフテクニシャンに通知する
                If (Not drChtAccount.IsSTF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drChtAccount.STF_CD)) AndAlso
                    (Not staffCode.Equals(drChtAccount.STF_CD)) Then
                    TransmissionForCall(drChtAccount.STF_CD, smbRefreshFunction)
                End If
            Next

            dtChtAccount.Dispose()
        End Using
    End Sub

    ''' <summary>
    ''' Push送信処理
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <remarks></remarks>
    Private Sub TransmissionForCall(ByVal staffCode As String, ByVal refreshFunction As String)

        'POST送信メッセージの作成
        Dim postSendMessage As New StringBuilder
        postSendMessage.Append("cat=action")
        postSendMessage.Append("&type=main")
        postSendMessage.Append("&sub=js")
        postSendMessage.Append("&uid=" & staffCode)
        postSendMessage.Append("&time=0")
        postSendMessage.Append("&js1=" & refreshFunction)

        '送信処理
        Dim visitUtility As New VisitUtility
        visitUtility.SendPush(postSendMessage.ToString)

    End Sub
    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START

    ''' <summary>
    ''' CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtToStall(ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal staffCode As String, _
                                     ByVal stallId As Decimal)

        Using serviceCommonbiz As New ServiceCommonClassBusinessLogic
            ' ストールIDのリスト生成
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            stallIdList.Add(stallId)

            ' 権限コードリスト生成(CT・ChT)
            Dim stuffCodeList As New List(Of Decimal)
            stuffCodeList.Add(OperationCodeCT)
            stuffCodeList.Add(OperationCodeChT)

            ' ストールIDよりPush通知アカウントリスト取得
            Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
            staffInfoDataTable = serviceCommonbiz.GetNoticeSendAccountListToStall(dealerCode, branchCode, stallIdList, stuffCodeList)

            For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                ' 自分以外の場合Push送信する
                If Not String.Equals(row.ACCOUNT, staffCode) Then
                    TransmissionForCall(row.ACCOUNT, smbRefreshFunction)
                End If
            Next
        End Using
    End Sub

    ''' <summary>
    ''' CT/CHTへのPush処理(全て)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtAll(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                     ByVal staffCode As String)
        Dim utility As New VisitUtilityBusinessLogic

        ' 権限コードリスト生成(CT・ChT)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(OperationCodeCT)
        stuffCodeList.Add(OperationCodeChT)

        ' オンラインユーザーの取得
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
        utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
        utility = Nothing

        For Each row As VisitUtilityUsersRow In sendPushUsers.Rows
            ' 自分以外の場合Push送信する
            If Not String.Equals(row.ACCOUNT, staffCode) Then
                TransmissionForCall(row.ACCOUNT, smbRefreshFunction)
            End If
        Next
    End Sub

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

#End Region
    '2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ログを出力する
    ''' </summary>
    ''' <param name="logLevel">ログレベル</param>
    ''' <param name="functionName">関数名</param>
    ''' <param name="msg">メッセージ</param>
    ''' <param name="ex">例外</param>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Private Sub OutputLog(ByVal logLevel As String, _
                          ByVal functionName As String, _
                          ByVal msg As String, _
                          ByVal ex As Exception, _
                          ByVal ParamArray values() As String)

        Dim i As Integer
        Dim logMessage As New System.Text.StringBuilder
        logMessage.Append("")

        For i = 0 To values.Length() - 1
            logMessage.Append("[").Append(values(i)).Append("]")
        Next i

        Dim logData As New System.Text.StringBuilder
        If LOG_TYPE_INFO.Equals(logLevel) Then
            logData.Append("")
            logData.Append(functionName).Append(" ").Append(logMessage.ToString()).Append(" ").Append(msg)
            Logger.Info(logData.ToString())
        ElseIf LOG_TYPE_ERROR.Equals(logLevel) Then
            logData.Append("")
            logData.Append(msg).Append("[FUNC:").Append(functionName).Append("]")
            If ex Is Nothing Then
                Logger.Error(logData.ToString())
            Else
                Logger.Error(logData.ToString(), ex)
            End If
        ElseIf LOG_TYPE_WARNING.Equals(logLevel) Then
            logData.Append("")
            logData.Append(msg).Append("[FUNC:").Append(functionName).Append("]")
            Logger.Warn(logData.ToString())
        End If

    End Sub

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' ログ出力(IF戻り値用)
    ' ''' </summary>
    ' ''' <param name="dt">戻り値(DataTable)</param>
    ' ''' <param name="ifName">使用IF名</param>
    ' ''' <remarks></remarks>
    'Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

    '    If dt Is Nothing Then
    '        Return
    '    End If

    '    Logger.Info(ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

    '    Dim log As New Text.StringBuilder

    '    For j = 0 To dt.Rows.Count - 1

    '        log = New Text.StringBuilder()
    '        Dim dr As DataRow = dt.Rows(j)

    '        log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

    '        For i = 0 To dt.Columns.Count - 1
    '            log.Append(dt.Columns(i).Caption)
    '            If IsDBNull(dr(i)) Then
    '                log.Append(" IS NULL")
    '            Else
    '                log.Append(" = ")
    '                log.Append(dr(i).ToString)
    '            End If

    '            If i <= dt.Columns.Count - 2 Then
    '                log.Append(", ")
    '            End If
    '        Next

    '        Logger.Info(log.ToString)
    '    Next

    '    Logger.Info(ifName + " Result END ")

    'End Sub

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

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

