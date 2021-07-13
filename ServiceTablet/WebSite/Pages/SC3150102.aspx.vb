'------------------------------------------------------------------------------
'SC3150102.aspx.vb
'------------------------------------------------------------------------------
'機能：TCメインメニュー_R/O情報タブ
'補足：
'作成：2012/01/30 KN 渡辺
'更新：2012/03/10 KN 西田 【SERVICE_1】課題管理番号-BMTS_0306_YW_03 PAYMENTFLAGがCHARの2byteのため、スペース削除
'更新：2012/03/12 KN 西田 【SERVICE_1】課題管理番号-BMTS_0310_YW_03の不具合修正 作業進捗エリアのR/ONoに枝番表示
'更新：2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合
'更新：2012/03/13 KN 上田 追加承認チップ部品準備完了情報を考慮するように修正
'更新：2012/03/14 KN 西田 作業項目の単価、合計項目が参照しているデータ変更
'更新：2012/03/14 KN 西田 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更
'更新：2012/03/23 KN 森下 仕様変更書_20120309_TC問診表画面にボタン追加
'更新：2012/03/27 KN 森下【SERVICE_1】システムテストの不具合修正No79 部品連絡ポップアップに部品が表示されない。
'更新：2012/04/07 KN 日比野【SERVICE_1】企画_プレユーザテストの不具合修正No3　部品エリアの単位の表示誤り
'更新：2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加
'更新：2012/04/13 KN 西田 開発_プレユーザーテスト 課題No.112 持込者名の名前が所有者名で表示される
'更新：2012/06/01 KN 西田 STEP1 重要課題対応
'更新：2012/06/06 KN 彭   コード分析対応
'更新：2012/06/18 KN 西田 DevPartner解析結果反映
'更新：2012/07/25 KN 彭   【SERVICE_1】仕分け課題対応
'更新：2012/08/01 KN 彭   サービス緊急対応（FMへの呼び出し通知機能を追加）→GTMC専用機能
'更新：2012/08/14 KN 彭   SAストール予約受付機能開発（No.27カゴナンバー表示）
'更新：2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
'更新：2013/12/10 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新：2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
'更新：2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応
'更新：2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
'更新：2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し
'更新：2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
'更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新：2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新：2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない
'更新：2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（R/O情報タブ表示でエラー発生を修正）
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：2019/12/19 NSK 夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801104
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801110
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801113
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801004
'Imports Toyota.eCRB.iCROP.BizLogic.IC3801801
Imports Toyota.eCRB.iCROP.BizLogic.SC3150102
'Imports Toyota.eCRB.iCROP.DataAccess.IC3810801
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
''2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
'Imports Toyota.eCRB.DMSLinkage.OrderHistory.DataAccess.IC3801601
''2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END
'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

'2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
Imports Toyota.eCRB.Technician.MainMenu
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic

'2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

'2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
'2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

'2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
'2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END


Partial Class Pages_SC3150102
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 空白文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRING_SPACE As String = ""
    ''' <summary>
    ''' 工数の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_WORK_HOURS As Integer = 0

    ''' <summary>
    ''' 単価の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_SELL_HOUR_RATE As Integer = 0

    ''' <summary>
    ''' 金額の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_SELL_WORK_PRICE As Integer = 0

    ''' <summary>
    ''' 部品数量の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_PARTS_QUANTITY As Integer = 0

    ''' <summary>
    ''' 基本情報・顧客情報　メーカー名：「TOYOTA」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAKER_TYPE_TOYOTA As String = "TOYOTA"

    ''' <summary>
    ''' 基本情報・顧客情報　メーカー名：「OTHERS」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAKER_TYPE_OTHERS As String = "OTHERS"

    ''' <summary>
    ''' 基本情報・初期状態・燃料：空
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_EMPTY As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_QUARTER As String = "1"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_HALF As String = "2"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の3
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_THREE_QUARTER As String = "3"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：満タン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_FULL As String = "4"

    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：オフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_OFF As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：CD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_CD As String = "1"
    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：FM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_FM As String = "2"

    ''' <summary>
    ''' 基本情報・初期状態・エアコン：OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AIR_CONDITIONER_OFF As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・エアコン：ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AIR_CONDITIONER_ON As String = "1"

    ''' <summary>
    ''' 基本情報・初期状態・付属品：チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_ACCESSORY_CHECKED As String = "1"

    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：持帰り
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_TAKEOUT As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：保険提出
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_INSURANCE As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：店内処分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_DISPOSE As String = "2"

    ''' <summary>
    ''' ご用命事項・確認事項・待ち方：店内
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WAITING_IN As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・待ち方：店外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WAITING_OUT As String = "1"

    ''' <summary>
    ''' ご用命事項・確認事項・洗車：する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WASHING_DO As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・洗車：しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WASHING_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_CASH As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：カード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_CARD As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_OTHER As String = "2"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：午前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_AM As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：午後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_PM As String = "2"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：指定なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_ALWAYS As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・WNG：常時点灯
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_ALWAYS As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・WNG：頻繁に点灯
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_OFTEN As String = "2"
    ''' <summary>
    ''' ご用命事項・問診項目・WNG：表示なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・故障発生時間：最近
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_RECENTLY As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・故障派生時間：一週間前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_WEEK As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生時間：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_OTHER As String = "2"

    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：頻繁に
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_HIGH As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：時々
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_OFTEN As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：一回だけ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_ONCE As String = "2"

    ''' <summary>
    ''' ご用命事項・問診項目・再現可能：はい
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_REAPPEAR_YES As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・再現項目：いいえ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_REAPPEAR_NO As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・水温：冷
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WATERT_LOW As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・水温：熱
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WATERT_HIGH As String = "1"

    ''' <summary>
    ''' ご用命事項・問診項目・気温：寒
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TEMPERATURE_LOW As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・気温：暑
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TEMPERATURE_HIGH As String = "1"

    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：駐車場
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_PARKING As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：一般道路
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_ORDINARY As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：高速道路
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_MOTORWAY As String = "2"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：坂道
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_SLOPE As String = "3"

    ''' <summary>
    ''' ご用命事項・問診項目・渋滞状況：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAFFICJAM_HAPPEN As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・渋滞状況：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAFFICJAM_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・車両状態：オン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CARSTATUS_ON As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・車両状態：オフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CARSTATUS_OFF As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・走行時：穏速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_LOWSPEED As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・走行時：加速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_ACCELERATION As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・走行時：減速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_SLOWDOWN As String = "2"

    ''' <summary>
    ''' ご用命事項・問診項目・非純正用品：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_NONGENUINE_YES As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・非純正用品：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_NONGENUINE_NO As String = "0"

    ''' <summary>
    ''' 部品準備完了フラグ：準備完了していない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_UNPREPARED As String = "0"
    ''' <summary>
    ''' 部品準備完了フラグ：準備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_PREPARED As String = "1"

    ''' <summary>
    ''' R/O情報欄のフィルタフラグ：フィルタをかけない
    ''' </summary>
    ''' <remarks></remarks>
    Private REPAIR_ORDER_FILTER_OFF As String = "0"

    ''' <summary>
    ''' 部品項目で、B/Oを表示するフラグの文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_BACK_ORDER_DISP_FLG As String = "1"

    ''' <summary> 完成検査承認フラグ 0：完成検査前（承認済みも含む） </summary>
    Private Const INSPECTION_APPROVAL_BEFORE As String = "0"
    ''' <summary> 完成検査承認フラグ 1：完成検査承認依頼中 </summary>
    Private Const INSPECTION_APPROVAL_WAIT As String = "1"

    ''' <summary>
    ''' 作業連番
    ''' 0：未計画
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORKSEQ_NOPLAN_PARENT As String = "0"

    ''' <summary>
    ''' 着工未指示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSTRUCT_UNDIRECTION As String = "0"

    ''' <summary>
    ''' HTMLの文字を改行するためのタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHAR_MULTI_LINE As String = "<br>"

    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' HTMLの文字を改行するためのタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHAR_HTML_SPACE As String = "&nbsp"
    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategoryPopup As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionTypeHeader As String = "1"

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispTypeText As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColorYellow As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ViewMode As String = "1"
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 画面連携：車両内部情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const displayNumber20 As Integer = 20
    ''' <summary>
    ''' 画面連携：顧客要求情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const displayNumber5 As Integer = 5
    ''' <summary>
    ''' 設定名：OTHER_LINKAGE_DOMAIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_LINKAGE_DOMAIN As String = "OTHER_LINKAGE_DOMAIN"
    ''' <summary>
    '''日付最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MINDATE As String = "1900/01/01 0:00:00"
    ''' <summary>
    ''' 省略値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_VALUE As String = " "
    ''' <summary>
    ''' 完成検査ステータス:完成検査未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSPECTION_INCOMPLETE As String = "0"
    ''' <summary>
    ''' 作業連番:省略値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_SEQ_DEFAULT As String = "-1"
    ''' <summary>
    ''' 入庫管理番号の分解文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUNCTUATION_STRING As String = "@"
    ''' <summary>
    ''' 着工指示フラグ：指示済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STARTWORK_INSTRUCT_FLG_NO As String = "1"
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150102"

    ''' <summary>
    ''' 干渉バリデーション結果：作業チップと干渉するため処理不可
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_FAILE As Integer = 1

    ''' <summary>
    ''' 干渉バリデーション結果：休憩をとらなければ、処理可能
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_DONOT_BREAK As Integer = 2

    ''' <summary>
    ''' 干渉バリデーション結果：休憩をとっても、とらなくても処理可能
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_TAKE_BREAK As Integer = 3

    ''' <summary>
    ''' 干渉バリデーション結果：作業チップとも休憩チップとも干渉なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_SUCCESSFULL As Integer = 4

    ''' <summary>
    ''' ストール利用ステータス_作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private stallUseStatus_Working As String = "02"

    ''' <summary>
    ''' ストール利用ステータス_一部作業中断
    ''' </summary>
    ''' <remarks></remarks>
    Private stallUseStatus_StopPart As String = "04"

    ''' <summary>
    ''' ストールの稼動開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualStartTime As Date

    ''' <summary>
    ''' ストールの稼動終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualEndTime As Date

    ''' <summary>
    ''' 休憩による作業伸長ポップアップの表示フラグ：表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_BREAK_DISPLAY = "1"

    ''' <summary>
    ''' 休憩による作業伸長ポップアップの表示フラグ：表示しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_BREAK_NONE = "0"
    ''' <summary>
    ''' 押したフッタボタンの状態：初期状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_INIT = "0"

    ''' <summary>
    ''' 作業開始フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStartFlg As String = "0"

    ''' <summary>
    ''' 作業終了フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workFinishFlg As String = "1"

    ''' <summary>
    ''' 作業中断フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStopFlg As String = "2"

    ''' <summary>
    ''' 開始イベントのエラーコード：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_CODE_START_WORK_SUCCESSFULL As Integer = 0

    ''' <summary>
    ''' 休憩取得フラグ(休憩を取得する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TakeBreakFlg As String = "1"

    ''' <summary>
    ''' 休憩取得フラグ(休憩を取得しない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DoNotBreakFlg As String = "0"
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

    '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
    ''' <summary>
    ''' 画面更新フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshFlg As String = "1"
    '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' アイコンの表示フラグ（1：Mマーク・Eマーク・Tマーク・Pマーク表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconFlg1 As String = "1"
    ''' <summary>
    ''' アイコンの表示フラグ（2：Bマーク・Lマーク表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconFlg2 As String = "2"
    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

#Region "変数定義"
    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private accountStaffContext As StaffContext
    ''' <summary>
    ''' オーダーNo
    ''' </summary>
    ''' <remarks></remarks>
    Private repairOrderNo As String
    ''' <summary>
    ''' 作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private workSeq As String
    ''' <summary>
    ''' 予約ID
    ''' </summary>
    ''' <remarks></remarks>
    Private rezId As String
    ''' <summary>
    ''' 着工指示区分
    ''' </summary>
    ''' <remarks></remarks>
    Private instruct As String
    ''' <summary>
    ''' 車輌登録番号
    ''' </summary>
    ''' <remarks></remarks>
    Private vclRegNo As String
    ''' <summary>
    ''' ストール名
    ''' </summary>
    ''' <remarks></remarks>
    Private stallName As String

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' ストール利用ID
    ''' </summary>
    ''' <remarks></remarks>
    Private stallUseId As Decimal
    ''' <summary>
    ''' ストール利用ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private stallUseStatus As String
    ''' <summary>
    ''' ストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private stallId As Decimal
    ''' <summary>
    ''' チップの開始日時
    ''' </summary>
    ''' <remarks></remarks>
    Private startTime As Date
    ''' <summary>
    ''' チップの終了日時
    ''' </summary>
    ''' <remarks></remarks>
    Private endTime As Date
    ''' <summary>
    ''' 行ロックバージョン
    ''' </summary>
    ''' <remarks></remarks>
    Private updateCount As Long
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3150102BusinessLogic
    ''' <summary>
    ''' サービスコモンのインスタンス
    ''' </summary>
    ''' <remarks></remarks>
    Private serviceCommon As New ServiceCommonClassBusinessLogic

    'Private repairOrderDataTable As IC3801001DataSet.IC3801001OrderCommDataTable
    'Private repairOrderData As IC3801001DataSet.IC3801001OrderCommRow
    Private repairOrderDataTable As SC3150102DataSet.SC3150102GetRoInfoDataTable
    Private repairOrderData As SC3150102DataSet.SC3150102GetRoInfoRow

    'Private dtWorkgroupInfo As IC3801104DataSet.IC3801104WorkGroupInfoDataTable

#End Region

    '2012/06/06 KN 彭　コード分析対応 START

#Region "初期処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
    ''' </History>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Logger.Info("Page_Init Start")
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Page.IsPostBack Then
            Logger.Info("Page.IsPostBack")
        End If

        'LabelNow.Text = DateTime.Now.ToString()

        'ページ内変数の初期化（セッション情報の格納）.
        SetInitVariable()

        'R/Oの追加作業アイコンの親オーダー時の文字を格納する.
        Me.HiddenFieldRepairOrderInitialWord.Value = WebWordUtility.GetWord(316)

        Dim jobDetailId As Decimal = 0

        If Not String.IsNullOrEmpty(Me.rezId) Then
            jobDetailId = CType(Me.rezId, Decimal)
        End If

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Dim repairOrderDataTable As New SC3150102DataSet.SC3150102GetRoInfoDataTable

        'Logger.Info("Page_Init OrderNumber:" + Me.repairOrderNo)

        'If (Not String.IsNullOrEmpty(Me.repairOrderNo)) Then
        '    'Logger.Info("Page_Load Not IsNullOrEmpty OrderNumber")

        '    'R/O基本情報の取得.
        '    repairOrderDataTable = Me.businessLogic.GetRepairOrderBaseData(Me.accountStaffContext.DlrCD, _
        '                                                                   Me.accountStaffContext.BrnCD, _
        '                                                                   Me.repairOrderNo, _
        '                                                                   Integer.Parse(Me.workSeq, CultureInfo.InvariantCulture))
        'End If

        'Dim repairOrderDataRow As SC3150102DataSet.SC3150102GetRoInfoRow = _
        'CType(repairOrderDataTable.Rows(0), SC3150102DataSet.SC3150102GetRoInfoRow)

        'Me.repairOrderData = Nothing
        'If (Not IsNothing(repairOrderDataTable)) Then
        '    'Logger.Info("Page_Load Not IsNothing repairOrderDataTable")
        '    'Logger.Info("Page_Load repairOrderDataTable.Rows.Count:" + CType(repairOrderDataTable.Rows.Count, String))

        '    If (repairOrderDataTable.Rows.Count > 0) Then
        '        'Logger.Info("Page_Load repairOrderDataTable.Rows.Count > 0")
        '        Me.repairOrderData = CType(repairOrderDataTable.Rows(0), IC3801001DataSet.IC3801001OrderCommRow)
        '    End If
        'End If

        ''R/O基本情報を取得できていない場合に備え、各項目に初期値を設定する.
        'Me.HiddenFieldOrderStatus.Value = "0"
        'Me.HiddenFieldAddWorkCount.Value = "0"
        'Me.HiddenFieldSAName.Value = ""
        ''2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
        'Me.HiddenFieldInspectionApprovalFlag.Value = INSPECTION_APPROVAL_BEFORE
        ''2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END

        ''R/O基本情報を取得できている場合のみ、各項目への情報格納処理を実施する.
        'If (Not IsNothing(Me.repairOrderData)) Then
        '    'Logger.Info("Page_Load Start Not IsNothing(Me.repairOrderData)")

        '    'R/O作業ステータス情報を設定
        '    If (Not Me.repairOrderData.IsOrderStatusNull) Then
        '        'Logger.Info("Page_Load OrderStatus is not DBNull")
        '        Me.HiddenFieldOrderStatus.Value = Me.repairOrderData.OrderStatus
        '    End If

        '    '追加作業件数を設定
        '    If (Not Me.repairOrderData.IsaddSrvCountNull) Then
        '        'Logger.Info("Page_Load addSrvCount is not DBNull")
        '        Me.HiddenFieldAddWorkCount.Value = CType(Me.repairOrderData.addSrvCount, String)
        '    End If

        '    'SA名をHiddenに格納する.
        '    If (Not Me.repairOrderData.IsOrderSaNameNull) Then
        '        'Logger.Info("Page_Load OrderSaName is not DBNull")
        '        Me.HiddenFieldSAName.Value = Me.repairOrderData.OrderSaName
        '    End If

        'Dim cageNo As String = String.Empty                     'カゴ番号

        'If Not String.IsNullOrEmpty(Me.workSeq) AndAlso _
        '   Not WORKSEQ_NOPLAN_PARENT.Equals(Me.workSeq) Then    ' 追加作業の場合
        '    Dim drChildChip As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow = _
        '            businessLogic.GetChildChipDataRow(Me.accountStaffContext.DlrCD _
        '                                            , Me.repairOrderNo _
        '                                            , CType(Me.workSeq, Integer))

        '    Dim srvAddSeq As String = String.Empty                  'Tact側の枝番
        '    Dim inspectionApprovalFlag As String = String.Empty     '完成検査フラグ

        '    If Not IsNothing(drChildChip) Then
        '        If Not drChildChip.IsSRVADDSEQNull Then
        '            srvAddSeq = drChildChip.SRVADDSEQ.Trim
        '        End If
        '        If Not drChildChip.IsINSPECTIONAPPROVALFLAGNull Then
        '            inspectionApprovalFlag = drChildChip.INSPECTIONAPPROVALFLAG.Trim
        '        End If
        '        If Not drChildChip.IsCAGENONull Then
        '            cageNo = drChildChip.CAGENO.Trim
        '        End If
        '    End If

        '    Me.HiddenFieldInspectionApprovalFlag.Value = inspectionApprovalFlag

        '    Me.HiddenFieldTactSrvAddSeq.Value = srvAddSeq
        '    MyBase.SetValue(ScreenPos.Current, "Redirect.SRVADDSEQ", Me.HiddenFieldTactSrvAddSeq.Value)     'TACTの枝番をHidden、セッションに格納
        '    Logger.Info("SESSION_SRVADDSEQ: " + Me.HiddenFieldTactSrvAddSeq.Value)
        'Else                                '親作業、又は着工未指示の場合
        '    If Not Me.repairOrderData.IscageNONull Then
        '        cageNo = Me.repairOrderData.cageNO.Trim()
        '    End If

        '    Me.HiddenFieldInspectionApprovalFlag.Value = Me.repairOrderData.INSPECTIONAPPROVALFLAG.Trim()   '完成検査フラグ

        '    MyBase.SetValue(ScreenPos.Current, "Redirect.SRVADDSEQ", "0")   'TODO:0固定でよい？
        '    Logger.Info("InitialRO. SESSION_SRVADDSEQ: 0")
        'End If


        '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START

        'カゴ番号格納用変数
        Dim cageNo As StringBuilder = New StringBuilder(String.Empty)
        'RO番号がある場合
        If Not String.IsNullOrEmpty(Me.repairOrderNo) Then

            Dim dtCageInfo As SC3150102DataSet.SC3150102CageInfoDataTable
            'カゴ番号の情報取得
            dtCageInfo = Me.businessLogic.GetCageNumber(accountStaffContext.DlrCD, accountStaffContext.BrnCD, Me.repairOrderNo)

            '取得したカゴ番号分だけ繰り返す
            For i = 0 To dtCageInfo.Rows.Count - 1

                If Not dtCageInfo(i).IsCAGE_NONull Then

                    'カゴ番号ごとに「/」で区切る
                    If i <> 0 Then
                        cageNo.Append("/")
                    End If
                    'かご番号格納
                    cageNo.Append(dtCageInfo(i).CAGE_NO)

                End If

            Next

            'かご番号の要素の幅変更javaScript実行
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetCagoNumber", _
                                                "SetCagoNumberWidth(" + (dtCageInfo.Rows.Count).ToString(CultureInfo.CurrentCulture()) + ");", True)

        End If

        'Me.lblCageNo.Text = cageNo
        'Logger.Info("CageNo=" + cageNo)
        Me.lblCageNo.Text = cageNo.ToString()
        Logger.Info("CageNo=" + cageNo.ToString())

        ''2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END

        Logger.Info("Page_Init OrderNumber:" + Me.repairOrderNo)

        If (Not String.IsNullOrEmpty(Me.repairOrderNo)) Then
            'Logger.Info("Page_Load Not IsNullOrEmpty OrderNumber")

            'R/O基本情報の取得.
            Me.repairOrderDataTable = Me.businessLogic.GetRepairOrderBaseData(Me.accountStaffContext.DlrCD, _
                                                                              Me.accountStaffContext.BrnCD, _
                                                                              Me.repairOrderNo, _
                                                                              Integer.Parse(Me.workSeq, CultureInfo.InvariantCulture()))
        End If

        Me.repairOrderData = Nothing
        If (Not IsNothing(repairOrderDataTable)) Then

            If (repairOrderDataTable.Rows.Count > 0) Then

                Me.repairOrderData = CType(repairOrderDataTable.Rows(0), SC3150102DataSet.SC3150102GetRoInfoRow)
            End If
        End If

        'R/O基本情報を取得できていない場合に備え、各項目に初期値を設定する.
        Me.HiddenFieldOrderStatus.Value = "0"
        Me.HiddenFieldAddWorkCount.Value = "0"
        Me.HiddenFieldSAName.Value = ""
        Me.HiddenFieldInspectionApprovalFlag.Value = INSPECTION_APPROVAL_BEFORE

        Dim saChipID As Long = 0
        Dim basRezId As String = ""
        Dim vin As String = ""
        Dim addWorkCount As Long = -1

        'R/O基本情報を取得できている場合のみ、各項目への情報格納処理を実施する.
        If (Not IsNothing(Me.repairOrderData)) Then

            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            Me.stallActualStartTime = DateTimeFunc.Now(accountStaffContext.DlrCD).Date
            Me.stallActualEndTime = Me.stallActualStartTime.AddDays(1)
            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            ''R/O作業ステータス情報を設定
            'If (Not Me.repairOrderData.IsRO_STATUSNull) Then
            '    'Logger.Info("Page_Load OrderStatus is not DBNull")
            '    Me.HiddenFieldOrderStatus.Value = Me.repairOrderData.RO_STATUS
            'End If

            '追加作業件数を設定
            If (Not Me.repairOrderData.IsADD_SVC_COUNTNull) Then
                'Logger.Info("Page_Load addSrvCount is not DBNull")
                Me.HiddenFieldAddWorkCount.Value = CType(Me.repairOrderData.ADD_SVC_COUNT, String)
                addWorkCount = repairOrderData.ADD_SVC_COUNT
            End If

            'SA名をHiddenに格納する.
            If (Not Me.repairOrderData.IsSTF_NAMENull) AndAlso _
                (Not String.IsNullOrEmpty(Me.repairOrderData.STF_NAME)) Then

                'Logger.Info("Page_Load OrderSaName is not DBNull")
                Me.HiddenFieldSAName.Value = Me.repairOrderData.STF_NAME
            End If


            '画面連携に必要な引数を取得
            Using dtScreenLinkageInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable = _
                Me.businessLogic.GetScreenLinkageInfo(accountStaffContext.DlrCD, _
                                                      accountStaffContext.BrnCD, _
                                                      jobDetailId)

                '取得したレコードが0行の場合処理しない
                If Not dtScreenLinkageInfo.Rows.Count = 0 Then

                    'ロウに格納
                    Dim drScreenLinkageInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow = _
                           DirectCast(dtScreenLinkageInfo.Rows(0), SC3150102DataSet.SC3150102ScreenLinkageInfoRow)

                    '変数に格納
                    saChipID = CType(drScreenLinkageInfo.VISITSEQ, Long)
                    basRezId = drScreenLinkageInfo.DMS_JOB_DTL_ID
                    vin = drScreenLinkageInfo.VIN
                Else
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                              , "{0}.{1} ScreenLinkageInfoDataTable.Rows.Count:{2}" _
                                              , Me.GetType.ToString _
                                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                              , dtScreenLinkageInfo.Rows.Count.ToString(CultureInfo.CurrentCulture())))
                End If

            End Using

            '基幹情報の取得
            Using dtServiceCommon As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                Me.serviceCommon.GetIcropToDmsCode(accountStaffContext.DlrCD, _
                                                DmsCodeType.BranchCode, _
                                                accountStaffContext.DlrCD, _
                                                accountStaffContext.BrnCD, _
                                                Nothing, _
                                                accountStaffContext.Account)
                'ロウに格納
                Dim drServiceCommon As ServiceCommonClassDataSet.DmsCodeMapRow = _
                     DirectCast(dtServiceCommon.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基本情報パネルのデータを表示.
                SetBasicInfoData(saChipID, basRezId, drServiceCommon.CODE1, drServiceCommon.CODE2, drServiceCommon.ACCOUNT, vin)
                'ご用命事項パネルのデータを表示.
                SetOrdersInfoData(saChipID, basRezId, drServiceCommon.CODE1, drServiceCommon.CODE2, drServiceCommon.ACCOUNT, vin)

            End Using

            '基本情報パネルの顧客情報の設定.
            SetBasicInfo()

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START

            '履歴情報の設定.
            SetHistoryInfo(0)

            '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START

        End If
        'Logger.Info("Page_Load Start HiddenFieldPartsReady:" + Me.HiddenFieldPartsReady.Value)
        'Logger.Info("Page_Load Start HiddenFieldOrderStatus:" + Me.HiddenFieldOrderStatus.Value)
        'Logger.Info("Page_Load Start HiddenFieldAddWorkCount:" + Me.HiddenFieldAddWorkCount.Value)
        'Logger.Info("Page_Load Start HiddenFieldSAName:" + Me.HiddenFieldSAName.Value)


        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '履歴情報の設定.
        'SetHistoryInfo()

        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START


        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        '' 着工未指示の場合、作業内容パネルは表示しない
        'If Not String.IsNullOrEmpty(Me.instruct) AndAlso _
        '   Not INSTRUCT_UNDIRECTION.Equals(Me.instruct) AndAlso _
        '   Not String.IsNullOrEmpty(Me.workSeq) Then
        '    '作業内容パネルの作業内容の設定.
        '    SetWorkInfo()
        '    '作業内容パネルの部品項目の設定.
        '    SetPartsInfo()
        'End If


        '作業内容パネルの作業内容の設定.
        SetWorkInfo()

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        '中断処理の存在判定
        HasStopJob()
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        If addWorkCount > -1 Then
            '作業内容パネルの部品項目の設定.
            SetPartsInfo()
        End If

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        'Logger.Info("Page_Init End")
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 初回表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} START " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'For i = 0 To RepeaterWorkInfo.Items.Count - 1
        '    Dim rWorkInfo As Control = RepeaterWorkInfo.Items(i)

        '    '作業グループ名を表示する
        '    Dim strWorkgroupInfo As String = String.Empty
        '    Dim strWorkByCode As String = CType(rWorkInfo.FindControl("HiddenFieldWorkByCode"), HiddenField).Value.Trim()

        '    If Not String.IsNullOrEmpty(strWorkByCode) Then
        '        For Each dr As IC3801104DataSet.IC3801104WorkGroupInfoRow In dtWorkgroupInfo
        '            If (Not dr.IsWORKGROUPCODENull) AndAlso (Not String.IsNullOrEmpty(dr.WORKGROUPCODE)) Then
        '                If strWorkByCode.Equals(dr.WORKGROUPCODE.Trim()) Then
        '                    strWorkgroupInfo = dr.WORKGROUPNAME
        '                    Exit For
        '                End If
        '            End If
        '        Next
        '    End If
        '    CType(rWorkInfo.FindControl("LabelWorkgroupInfo"), Label).Text = strWorkgroupInfo
        'Next

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' ページ内変数の初期設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub SetInitVariable()

        Logger.Info("SetInitVariable Start")

        'ユーザ情報の取得
        Me.accountStaffContext = StaffContext.Current

        'ビジネスロジックを新規作成
        Me.businessLogic = New SC3150102BusinessLogic

        '各情報の初期化
        Me.repairOrderNo = ""
        Me.Hidden01Box03Filter.Value = REPAIR_ORDER_FILTER_OFF
        Me.workSeq = "0"
        Me.rezId = "0"
        Me.instruct = "0"

        'セッション情報の取得処理.
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.ORDERNO")) Then
            'オーダー番号を取得する
            Me.repairOrderNo = MyBase.GetValue(ScreenPos.Current, "Redirect.ORDERNO", False).ToString().Trim()
            Logger.Info("SetInitVariable SESSION ORDERNO: " + Me.repairOrderNo)
        End If

        'R/O情報にグレーフィルタ情報を取得する.
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.FILTERFLG")) Then
            Me.Hidden01Box03Filter.Value = MyBase.GetValue(ScreenPos.Current, "Redirect.FILTERFLG", False).ToString()
            Logger.Info("SetInitVariable SESSION FILTERFLG: " + Me.Hidden01Box03Filter.Value)
        End If

        ' 作業連番（データ取得用）
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.WORKSEQ")) Then
            Me.workSeq = MyBase.GetValue(ScreenPos.Current, "Redirect.WORKSEQ", False).ToString()
            Logger.Info("SetInitVariable SESSION WORKSEQ: " + Me.workSeq)
        End If

        ' 予約ID（作業項目欄フィルター用）
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.REZID")) Then
            Me.rezId = MyBase.GetValue(ScreenPos.Current, "Redirect.REZID", False).ToString()
            Logger.Info("SetInitVariable SESSION REZID: " + Me.rezId)
        End If
        ' 着工指示区分
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.INSTRUCT")) Then
            Me.instruct = MyBase.GetValue(ScreenPos.Current, "Redirect.INSTRUCT", False).ToString()
            Logger.Info("SetInitVariable SESSION INSTRUCT: " + Me.instruct)
        End If

        ' 車輌登録番号
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.VCLREGNO")) Then
            Me.vclRegNo = MyBase.GetValue(ScreenPos.Current, "Redirect.VCLREGNO", False).ToString()
            Logger.Info("SetInitVariable SESSION VCLREGNO: " + Me.vclRegNo)
        End If
        ' ストール名
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.STALLNAME")) Then
            Me.stallName = MyBase.GetValue(ScreenPos.Current, "Redirect.STALLNAME", False).ToString()
            Logger.Info("SetInitVariable SESSION STALLNAME: " + Me.stallName)
        End If

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        ' ストールID
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.STALLID")) Then
            Dim strStallId As String = MyBase.GetValue(ScreenPos.Current, "Redirect.STALLID", False).ToString()

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

            'Decimal.TryParse(strStallId, Me.stallId)

            'ストールID値チェック
            If Not (String.IsNullOrEmpty(Trim(strStallId))) Then
                '存在する場合
                '値を設定
                Me.stallId = Decimal.Parse(strStallId, CultureInfo.InvariantCulture)

            End If

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

            Logger.Info("SetInitVariable SESSION STALLID: " + strStallId)
        End If

        ' ストール利用ID
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.STALLUSEID")) Then
            Dim strStallUseId As String = MyBase.GetValue(ScreenPos.Current, "Redirect.STALLUSEID", False).ToString()

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

            'Decimal.TryParse(strStallUseId, Me.stallUseId)

            'ストール利用ID値チェック
            If Not (String.IsNullOrEmpty(Trim(strStallUseId))) Then
                '存在する場合
                '値を設定
                Me.stallUseId = Decimal.Parse(strStallUseId, CultureInfo.InvariantCulture)

            End If

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

            Logger.Info("SetInitVariable SESSION STALLUSEID: " + strStallUseId)
        End If

        ' ストール利用ID
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.STALLUSESTATUS")) Then
            Me.stallUseStatus = MyBase.GetValue(ScreenPos.Current, "Redirect.STALLUSESTATUS", False).ToString()
            Logger.Info("SetInitVariable SESSION STALLUSESTATUS: " + stallUseStatus)
        End If

        ' 行ロックバージョン
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.ROWUPDATECOUNT")) Then
            Dim strUpdateCount As String = MyBase.GetValue(ScreenPos.Current, "Redirect.ROWUPDATECOUNT", False).ToString()

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

            'Long.TryParse(strUpdateCount, Me.updateCount)

            '行ロックバージョン値チェック
            If Not (String.IsNullOrEmpty(Trim(strUpdateCount))) Then
                '存在する場合
                '値を設定
                Me.updateCount = Long.Parse(strUpdateCount, CultureInfo.InvariantCulture)

            End If

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

            Logger.Info("SetInitVariable SESSION ROWUPDATECOUNT: " + strUpdateCount)
        End If
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        Logger.Info("SetInitVariable End. Me.repairOrderNo" + Me.repairOrderNo)

    End Sub

#End Region

#Region "データ格納処理"
    ''' <summary>
    ''' 基本情報パネルの顧客情報の設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBasicInfo()

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Logger.Info("SetBasicInfo Start")

        ''2012/04/05 KN 西田　企画_プレユーザーテスト課題No.112　名前の「…」部分をﾀｯﾌﾟしても全部が表示されない START
        ''オーナー名の表示.
        'Dim buyerName As String = ""
        'If (Not repairOrderData.IsbuyerNameNull()) Then
        '    buyerName = repairOrderData.buyerName
        'End If
        'Me.LblBuyerName.Text = buyerName

        ''顧客名称の表示.
        ''2012/04/13 KN 西田 開発_プレユーザーテスト 課題No.112 持込者名の名前が所有者名で表示される START
        'Dim customerName As String = ""
        'If Not repairOrderData.IsTwcUserNull() Then
        '    customerName = repairOrderData.TwcUser
        'End If
        ''Dim customerName As String = repairOrderData.OrderCustomerName
        ''2012/04/13 KN 西田 開発_プレユーザーテスト 課題No.112 持込者名の名前が所有者名で表示される END
        'Me.LblOrderCustomerName.Text = customerName

        ''メーカー名の表示.
        'Dim makerName As String = ""
        ''2012/03/22 nishida 現地スルーテスト課題・不具合対応 メーカ名の表示をフラグによって制御 START
        'If (Not repairOrderData.IsmakerTypeNull()) Then
        '    Select Case repairOrderData.makerType
        '        Case MAKER_TYPE_TOYOTA      '0の場合、「TOYOTA」を表示   MAKER_TYPE_TOYOTA
        '            makerName = WebWordUtility.GetWord(323)
        '        Case MAKER_TYPE_OTHERS      '1の場合、「OTHERS」を表示   MAKER_TYPE_OTHERS
        '            makerName = WebWordUtility.GetWord(324)
        '    End Select
        'End If
        ''2012/03/22 nishida 現地スルーテスト課題・不具合対応 メーカ名の表示をフラグによって制御 END

        ''車種名称の表示.
        'Dim vehiclesName As String = ""
        'If (Not repairOrderData.IsOrderVhcNameNull()) Then
        '    vehiclesName = repairOrderData.OrderVhcName
        'End If

        ''グレードの表示.
        'Dim vehiclesGrade As String = ""
        'If (Not repairOrderData.IsOrderGradeNull()) Then
        '    vehiclesGrade = repairOrderData.OrderGrade
        'End If

        'Dim strSplit As String = "／"

        'Dim makerVehicleGrade As StringBuilder = New StringBuilder(String.Empty)
        'With makerVehicleGrade
        '    .Append(makerName)
        '    .Append(strSplit)
        '    .Append(vehiclesName)
        '    .Append(strSplit)
        '    .Append(vehiclesGrade)
        'End With

        'Me.LblMakerVehicleGrade.Text = makerVehicleGrade.ToString()

        ''VINの表示.
        'Dim vinNumber As String = ""
        'If (Not repairOrderData.IsOrderVinNoNull()) Then
        '    vinNumber = repairOrderData.OrderVinNo
        'End If
        'Me.LblOrderVinNo.Text = vinNumber

        ''車両番号の表示.
        'Dim registerNumber As String = ""
        'If (Not repairOrderData.IsOrderRegisterNoNull()) Then
        '    registerNumber = repairOrderData.OrderRegisterNo
        'End If
        'Me.LblOrderRegisterNo.Text = registerNumber

        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(表示内容修正(年式の表示は不要)) START
        ' ''年式の表示.
        ''Dim modelValue As String = ""
        ''If (Not repairOrderData.IsOrderModelNull()) Then
        ''    modelValue = repairOrderData.OrderModel
        ''End If
        ''Me.LiteralOrderModel.Text = modelValue
        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(表示内容修正(年式の表示は不要)) END

        ''納車日の表示.
        'Dim deliveryDate As String = ""
        'If (Not repairOrderData.IsDeliverDateNull()) Then
        '    '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 START
        '    ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(納車日の表示形式修正) START
        '    'deliveryDate = DateTimeFunc.FormatDate(21, repairOrderData.DeliverDate)
        '    ''deliveryDate = DateTimeFunc.FormatDate(3, repairOrderData.DeliverDate)
        '    ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(納車日の表示形式修正) END
        '    If Not String.IsNullOrEmpty(repairOrderData.DeliverDate) Then
        '        '2012/06/18 KN 西田 DevPartner解析結果反映 START
        '        'deliveryDate = DateTimeFunc.FormatDate(21, CDate(repairOrderData.DeliverDate))
        '        deliveryDate = DateTimeFunc.FormatDate(21, CType(repairOrderData.DeliverDate, Date))
        '        '2012/06/18 KN 西田 DevPartner解析結果反映 END
        '    End If
        '    '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 END
        'End If
        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(納車日の表示形式修正) START
        'Me.LblDeliverDate.Text = ExchangeDataToHtmlString(deliveryDate)
        ''Me.LiteralDeliverDate.Text = ExchangeDataToHtmlString(repairOrderData("deliverDate").ToString())
        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(納車日の表示形式修正) END

        ''走行距離の表示.
        'Dim mailageStringBuilder As New StringBuilder
        'If (Not repairOrderData.IsOrderMileAgeNull()) Then
        '    '2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(走行距離カンマ編集) START
        '    mailageStringBuilder.Append(String.Format(CultureInfo.CurrentCulture, "{0:#,0}", repairOrderData.OrderMileAge))
        '    'mailageStringBuilder.Append(CType(repairOrderData.OrderMileAge, String))
        '    '2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(走行距離カンマ編集) END
        '    mailageStringBuilder.Append(WebWordUtility.GetWord(109).Replace("%1", " "))
        'End If
        'Me.LblOrderMileage.Text = mailageStringBuilder.ToString()

        'Logger.Info("SetBasicInfo End")


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dtCutomerInfo As SC3150102DataSet.SC3150102CutomerInfoDataTable = _
             Me.businessLogic.GetCustomerInfo(accountStaffContext.DlrCD, accountStaffContext.BrnCD, repairOrderNo)

            'レコードが1行も取得できなかった場合処理しない
            If Not dtCutomerInfo.Rows.Count = 0 Then

                Dim drScreenLinkageInfo As SC3150102DataSet.SC3150102CutomerInfoRow = _
                          DirectCast(dtCutomerInfo.Rows(0), SC3150102DataSet.SC3150102CutomerInfoRow)

                'オーナー名の表示.
                Dim buyerName As String = ""
                buyerName = ExchangeDataToHtmlString(drScreenLinkageInfo.CST_NAME)
                Me.LblBuyerName.Text = buyerName

                '顧客名称の表示.
                Dim customerName As String = ""
                customerName = ExchangeDataToHtmlString(drScreenLinkageInfo.CONTACT_PERSON)
                Me.LblOrderCustomerName.Text = customerName

                'メーカー名の表示.
                Dim makerName As String = ""
                If (Not drScreenLinkageInfo.IsMAKER_CDNull()) Then
                    Select Case drScreenLinkageInfo.MAKER_CD
                        Case MAKER_TYPE_TOYOTA      '0の場合、「TOYOTA」を表示   MAKER_TYPE_TOYOTA
                            makerName = WebWordUtility.GetWord(323)
                        Case MAKER_TYPE_OTHERS      '1の場合、「OTHERS」を表示   MAKER_TYPE_OTHERS
                            makerName = WebWordUtility.GetWord(324)
                    End Select
                End If

                '車種名称の表示.
                Dim vehiclesName As String = ""
                vehiclesName = ExchangeDataToHtmlString(drScreenLinkageInfo.MODEL_NAME)

                'グレードの表示.
                Dim vehiclesGrade As String = ""
                vehiclesGrade = ExchangeDataToHtmlString(drScreenLinkageInfo.GRADE_NAME)

                Dim strSplit As String = "／"

                'メーカー名、車種名称、グレードの文字列結合
                Dim makerVehicleGrade As StringBuilder = New StringBuilder(String.Empty)
                With makerVehicleGrade
                    .Append(makerName)
                    .Append(strSplit)
                    .Append(vehiclesName)
                    .Append(strSplit)
                    .Append(vehiclesGrade)
                End With
                Me.LblMakerVehicleGrade.Text = makerVehicleGrade.ToString()

                'VINの表示.
                Dim vinNumber As String = ""
                vinNumber = ExchangeDataToHtmlString(drScreenLinkageInfo.VCL_VIN)
                Me.LblOrderVinNo.Text = vinNumber

                '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                'P/Lマークの表示
                If (IconFlg1.Equals(drScreenLinkageInfo.IMP_VCL_FLG)) Then
                    Me.Pmark.Visible = True
                    Me.Lmark.Visible = False
                ElseIf (IconFlg2.Equals(drScreenLinkageInfo.IMP_VCL_FLG)) Then
                    Me.Pmark.Visible = False
                    Me.Lmark.Visible = True
                Else
                    Me.Pmark.Visible = False
                    Me.Lmark.Visible = False
                End If

                'Tマークの表示
                If (drScreenLinkageInfo.TLM_MBR_FLG.Equals(IconFlg1)) Then
                    Me.Tmark.Visible = True
                Else
                    Me.Tmark.Visible = False
                End If

                'Eマークの表示
                If (drScreenLinkageInfo.EW_FLG.Equals(IconFlg1)) Then
                    Me.Emark.Visible = True
                Else
                    Me.Emark.Visible = False
                End If

                'M/Bマークの表示
                If (drScreenLinkageInfo.SML_AMC_FLG.Equals(IconFlg1)) Then
                    Me.Mmark.Visible = True
                    Me.Bmark.Visible = False

                ElseIf (drScreenLinkageInfo.SML_AMC_FLG.Equals(IconFlg2)) Then
                    Me.Mmark.Visible = False
                    Me.Bmark.Visible = True
                Else
                    Me.Mmark.Visible = False
                    Me.Bmark.Visible = False
                End If
                '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                '車両番号の表示.
                Dim registerNumber As String = ""
                registerNumber = ExchangeDataToHtmlString(drScreenLinkageInfo.REG_NUM)
                Me.LblOrderRegisterNo.Text = registerNumber

                '納車日の表示.
                Dim deliveryDate As String = ""
                If (Not drScreenLinkageInfo.IsDELI_DATENull) AndAlso _
                    (Not drScreenLinkageInfo.DELI_DATE = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", Nothing)) Then

                    deliveryDate = DateTimeFunc.FormatDate(21, CType(drScreenLinkageInfo.DELI_DATE, Date))

                End If

                Me.LblDeliverDate.Text = ExchangeDataToHtmlString(deliveryDate)

                '走行距離の表示.
                Dim mailageStringBuilder As New StringBuilder
                If (Not drScreenLinkageInfo.IsSVCIN_MILENull()) Then
                    mailageStringBuilder.Append(String.Format(CultureInfo.CurrentCulture, "{0:#,0}", drScreenLinkageInfo.SVCIN_MILE))
                    mailageStringBuilder.Append(WebWordUtility.GetWord(109).Replace("%1", " "))
                End If
                Me.LblOrderMileage.Text = mailageStringBuilder.ToString()

            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ''' <summary>
    ''' 履歴情報の設定.
    ''' </summary>
    ''' <param name="dispPageCount">表示回数</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' 2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
    ''' </history>
    Private Sub SetHistoryInfo(ByVal dispPageCount As Integer)

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Logger.Info("SetHistoryInfo Start")

        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
        ''整備内容参照を取得する.
        'Dim dt As IC3801004DataSet.IC3801004OderSrvDataTable
        'dt = Me.businessLogic.GetHistoryData(Me.accountStaffContext.DlrCD, Me.repairOrderNo)

        ''取得した整備内容がNothingでない場合、情報の設定を実施する.
        'If (Not IsNothing(dt)) Then

        '    'Logger.Info("SetHistoryInfo GetHistoryData_Count=" + CType(dt.Rows.Count, String))
        '    'コントロールにバインドする.
        '    Me.RepeaterHistoryInfo.DataSource = dt
        '    Me.RepeaterHistoryInfo.DataBind()

        '    '当日日付情報を取得する.

        '    'データを設定する.
        '    For i = 0 To RepeaterHistoryInfo.Items.Count - 1
        '        'Logger.Info("SetHistoryInfo Repeater Roop Index=" + CType(i, String))

        '        Dim rInfo As Control = RepeaterHistoryInfo.Items(i)
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '        ''受注日を表示する.
        '        'Dim strOrderAcceptDate As String = STRING_SPACE
        '        'If Not String.IsNullOrEmpty(Trim(CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value)) Then
        '        '    'Logger.Info("SetHistoryInfo Not IsNullOrEmpty AcceptDate")
        '        '    Dim acceptDate As Date
        '        '    If (Not Date.TryParse(CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value, acceptDate)) Then
        '        '        'Logger.Info("SetHistoryInfo Date Parse Faile:AcceptDate")
        '        '        acceptDate = Date.MinValue
        '        '    End If
        '        '    'Dim acceptDate As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value)
        '        '    If acceptDate > Date.MinValue Then
        '        '        'Logger.Info("SetHistoryInfo acceptDate is bigger than Date.MinValue")
        '        '        '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 START
        '        '        If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
        '        '            'If (acceptDate.Date = Date.Today) Then
        '        '            'Logger.Info("SetHistoryInfo acceptDate is Today")
        '        '            '受注日が当日である場合、HH:mm形式にて表示.
        '        '            strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
        '        '        Else
        '        '            'Logger.Info("SetHistoryInfo acceptDate is not Today")
        '        '            '受注日が当日でない場合、MM/dd形式にて表示.
        '        '            strOrderAcceptDate = DateTimeFunc.FormatDate(11, acceptDate)
        '        '        End If
        '        '        '2012/03/12 nishida 現在時間をサーバの時間を取得するよう変更 END
        '        '        'If String.IsNullOrEmpty(strOrderAcceptDate) Then
        '        '    Else
        '        '        'Logger.Info("SetHistoryInfo acceptDate is smaller than Date.MinValue")
        '        '        strOrderAcceptDate = STRING_SPACE
        '        '    End If
        '        'End If
        '        ''Logger.Info("SetHistoryInfo AcceptDate=" + strOrderAcceptDate)
        '        'CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate

        '        Dim dr As IC3801004DataSet.IC3801004OderSrvRow = _
        '            DirectCast(dt.Rows(i), IC3801004DataSet.IC3801004OderSrvRow)
        '        '受注日を表示する.
        '        Dim strOrderAcceptDate As String = STRING_SPACE
        '        If Not (dr.IsORDERDATENull OrElse String.IsNullOrEmpty(Trim(dr.ORDERDATE))) Then
        '            Dim acceptDate As Date
        '            If (Not Date.TryParse(dr.ORDERDATE, acceptDate)) Then
        '                acceptDate = Date.MinValue
        '            End If
        '            If acceptDate > Date.MinValue Then
        '                If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
        '                    '受注日が当日である場合、HH:mm形式にて表示.
        '                    strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
        '                Else
        '                    '受注日が当日でない場合、MM/dd形式にて表示.
        '                    strOrderAcceptDate = DateTimeFunc.FormatDate(11, acceptDate)
        '                End If
        '            Else
        '                strOrderAcceptDate = STRING_SPACE
        '            End If
        '        End If
        '        CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

        '        'オーダーNoを表示する.
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '        'Dim stringOrderNo As String = CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value
        '        'If String.IsNullOrEmpty(stringOrderNo) Then
        '        '    'Logger.Info("SetHistoryInfo orderNo is NullOrEmpty")
        '        '    stringOrderNo = STRING_SPACE
        '        'End If
        '        ''Logger.Info("SetHistoryInfo OrderNo=" + stringOrderNo)
        '        'CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
        '        Dim stringOrderNo As String = STRING_SPACE
        '        If Not (dr.IsORDERNONull OrElse String.IsNullOrEmpty(dr.ORDERNO)) Then
        '            stringOrderNo = dr.ORDERNO
        '        End If
        '        CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
        '        CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value = stringOrderNo
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

        '        '代表整備名称を表示する.
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '        'Dim strSrvTypeName As String = CType(rInfo.FindControl("HiddenFieldHTypicalSrvTypeName"), HiddenField).Value
        '        'If String.IsNullOrEmpty(strSrvTypeName) Then
        '        '    'Logger.Info("SetHistoryInfo typicalSrvTypeName is NullOrEmpty")
        '        '    strSrvTypeName = STRING_SPACE
        '        'End If
        '        ''Logger.Info("SetHistoryInfo TypicalSrvTypeName=" + strSrvTypeName)
        '        'CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName
        '        Dim strSrvTypeName As String = STRING_SPACE
        '        If Not (dr.IsTYPICALSRVTYPENAMENull OrElse String.IsNullOrEmpty(dr.TYPICALSRVTYPENAME)) Then
        '            strSrvTypeName = dr.TYPICALSRVTYPENAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

        '        '代表整備項目を表示する.
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '        'Dim strSrvType As String = CType(rInfo.FindControl("HiddenFieldHTypicalSrvType"), HiddenField).Value
        '        'If String.IsNullOrEmpty(strSrvType) Then
        '        '    'Logger.Info("SetHistoryInfo typicalSrvType is NullOrEmpty")
        '        '    strSrvType = STRING_SPACE
        '        'End If
        '        ''Logger.Info("SetHistoryInfo TypicalSrvType=" + strSrvType)
        '        'CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType
        '        Dim strSrvType As String = STRING_SPACE
        '        If Not (dr.IsORDERSRVNAMENull OrElse String.IsNullOrEmpty(dr.ORDERSRVNAME)) Then
        '            strSrvType = dr.ORDERSRVNAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

        '        '担当SAを表示する
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        '        'Dim strSaName As String = CType(rInfo.FindControl("HiddenFieldHSaName"), HiddenField).Value
        '        'If String.IsNullOrEmpty(strSaName) Then
        '        '    strSaName = STRING_SPACE
        '        'End If
        '        'CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
        '        Dim strSaName As String = STRING_SPACE
        '        If Not (dr.IsSANAMENull OrElse String.IsNullOrEmpty(dr.SANAME)) Then
        '            strSaName = dr.SANAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
        '        '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END
        '    Next
        'End If

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        ''入庫履歴を5件取得する
        'Dim dt As IC3801601DataSet.ORDERHISTORYDataTable
        'dt = Me.businessLogic.GetAllHistoryInfoInit(Me.accountStaffContext.DlrCD, _
        '                                            Trim(Me.LblOrderRegisterNo.Text), _
        '                                            Trim(Me.LblOrderVinNo.Text), _
        '                                            1, _
        '                                            5)

        ''取得した整備内容がNothingでない場合、情報の設定を実施する.
        'If (Not IsNothing(dt)) Then

        '    'Logger.Info("SetHistoryInfo GetHistoryData_Count=" + CType(dt.Rows.Count, String))
        '    'コントロールにバインドする.
        '    Me.RepeaterHistoryInfo.DataSource = dt
        '    Me.RepeaterHistoryInfo.DataBind()

        '    '当日日付情報を取得する.

        '    'データを設定する.
        '    For i = 0 To RepeaterHistoryInfo.Items.Count - 1
        '        Dim rInfo As Control = RepeaterHistoryInfo.Items(i)
        '        Dim drOrderHistory As IC3801601DataSet.ORDERHISTORYRow = _
        '            DirectCast(dt.Rows(i), IC3801601DataSet.ORDERHISTORYRow)

        '        '受注日を表示する.
        '        Dim strOrderAcceptDate As String = STRING_SPACE
        '        If Not (drOrderHistory.IsORDERDATENull) Then
        '            Dim acceptDate As Date = drOrderHistory.ORDERDATE
        '            If acceptDate > Date.MinValue Then
        '                If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
        '                    '受注日が当日である場合、HH:mm形式にて表示.
        '                    strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
        '                Else
        '                    '受注日が当日でない場合、YYYY/MM/dd形式にて表示.
        '                    strOrderAcceptDate = DateTimeFunc.FormatDate(3, acceptDate)
        '                End If
        '            Else
        '                strOrderAcceptDate = STRING_SPACE
        '            End If
        '        End If
        '        CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate
        '        'オーダーNoを表示する.
        '        Dim stringOrderNo As String = STRING_SPACE
        '        If Not (drOrderHistory.IsORDERNONull OrElse String.IsNullOrEmpty(drOrderHistory.ORDERNO)) Then
        '            stringOrderNo = drOrderHistory.ORDERNO
        '        End If
        '        CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
        '        CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value = stringOrderNo

        '        '販売店CDを格納する.
        '        Dim stringDlrCD As String = STRING_SPACE
        '        If Not (drOrderHistory.IsDEALERCODENull OrElse String.IsNullOrEmpty(drOrderHistory.DEALERCODE)) Then
        '            stringDlrCD = Trim(drOrderHistory.DEALERCODE)
        '        End If
        '        CType(rInfo.FindControl("HiddenFieldHDealerCode"), HiddenField).Value = stringDlrCD

        '        '代表整備名称を表示する.
        '        Dim strSrvTypeName As String = STRING_SPACE
        '        If Not (drOrderHistory.IsSRVTYPENAMENull OrElse String.IsNullOrEmpty(drOrderHistory.SRVTYPENAME)) Then
        '            strSrvTypeName = drOrderHistory.SRVTYPENAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName

        '        '代表整備項目を表示する.
        '        Dim strSrvType As String = STRING_SPACE
        '        If Not (drOrderHistory.IsSRVNAMENull OrElse String.IsNullOrEmpty(drOrderHistory.SRVNAME)) Then
        '            strSrvType = drOrderHistory.SRVNAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType

        '        '担当SAを表示する
        '        Dim strSaName As String = STRING_SPACE
        '        If Not (drOrderHistory.IsEMPLOYEENAMENull OrElse String.IsNullOrEmpty(drOrderHistory.EMPLOYEENAME)) Then
        '            strSaName = drOrderHistory.EMPLOYEENAME
        '        End If
        '        CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
        '    Next
        'End If
        ''2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

        ''2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START
        ''整備受注Noがあり、他店入庫履歴が存在する場合のみ文言設定
        'If Not (String.IsNullOrEmpty(Me.repairOrderNo)) Then
        '    Me.AllDispLink.Text = WebWordUtility.GetWord(133)
        '    Me.NextDispLink.Text = String.Format(CultureInfo.CurrentCulture, _
        '                                         WebWordUtility.GetWord(134), _
        '                                         "20")
        '    CType(Me.AllDispLinkDiv, HtmlContainerControl).Style("display") = "block"
        'End If
        ''2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} START " _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報の取得
        Me.accountStaffContext = StaffContext.Current

        '文言「定期検査」設定
        Dim periodicInspection As String = WebWordUtility.GetWord(130)
        '文言「一般検査」設定
        Dim generalInspection As String = WebWordUtility.GetWord(131)

        Using dtOrderHistory As SC3150102DataSet.SC3150102GetServiceInHistoryDataTable = _
            Me.businessLogic.GetAllHistoryInfo(accountStaffContext.DlrCD, accountStaffContext.BrnCD, repairOrderNo, dispPageCount)

            '整備受注Noがあり、他店入庫履歴が存在する場合のみ文言設定
            If IsNothing(dtOrderHistory) OrElse dtOrderHistory.Count > 0 Then
                '表示件数（終了）
                Dim dispEndCount As Integer = 5
                '表示件数が取得件数を上回った場合は取得件数に置き換える

                If Not (String.IsNullOrEmpty(Me.repairOrderNo)) Then
                    Me.AllDispLink.Text = WebWordUtility.GetWord(133)
                    Me.NextDispLink.Text = String.Format(CultureInfo.CurrentCulture, _
                                                         WebWordUtility.GetWord(134), _
                                                         "20")
                    CType(Me.AllDispLinkDiv, HtmlContainerControl).Style("display") = "block"
                End If

                '表示するDataRowをDataTableに格納する
                Using dtOrderHistoryNew As New SC3150102DataSet.SC3150102GetServiceInHistoryDataTable
                    For i = 0 To dtOrderHistory.Rows.Count - 1
                        Dim drOrderHistoryNew As SC3150102DataSet.SC3150102GetServiceInHistoryRow = dtOrderHistoryNew.NewSC3150102GetServiceInHistoryRow
                        Dim drOrderHistoryOld As SC3150102DataSet.SC3150102GetServiceInHistoryRow = _
                            DirectCast(dtOrderHistory.Rows(i), SC3150102DataSet.SC3150102GetServiceInHistoryRow)
                        '入庫実績日時
                        If Not (drOrderHistoryOld.IsSVCIN_DELI_DATENull) Then
                            drOrderHistoryNew.SVCIN_DELI_DATE = drOrderHistoryOld.SVCIN_DELI_DATE
                        End If

                        'RO番号
                        drOrderHistoryNew.SVCIN_NUM = ExchangeDataToHtmlString(drOrderHistoryOld.SVCIN_NUM)

                        '整備名称
                        drOrderHistoryNew.MAINTE_NAME = ExchangeDataToHtmlString(drOrderHistoryOld.MAINTE_NAME)

                        'サービス名称
                        drOrderHistoryNew.SVC_NAME_MILE = ExchangeDataToHtmlString(drOrderHistoryOld.SVC_NAME_MILE)

                        'SA名称
                        drOrderHistoryNew.STF_NAME = ExchangeDataToHtmlString(drOrderHistoryOld.STF_NAME)

                        '販売店CD
                        drOrderHistoryNew.DLR_CD = ExchangeDataToHtmlString(drOrderHistoryOld.DLR_CD)

                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                        drOrderHistoryNew.MAINTE_NAME_HIS = ExchangeDataToHtmlString(drOrderHistoryOld.MAINTE_NAME_HIS)
                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END

                        'データ格納
                        dtOrderHistoryNew.AddSC3150102GetServiceInHistoryRow(drOrderHistoryNew)

                        '設定表示件数に達した場合抜ける
                        If i = dispEndCount - 1 Then
                            Exit For
                        End If
                    Next

                    '表示ページ数をHIDDENに格納
                    Me.HiddenFieldOtherHistoryDispPageCount.Value = CType(20, String)
                    'コントロールにバインドする
                    Me.RepeaterHistoryInfo.DataSource = dtOrderHistoryNew
                    Me.RepeaterHistoryInfo.DataBind()

                    'データを設定する.
                    For i = 0 To RepeaterHistoryInfo.Items.Count - 1
                        Dim rInfo As Control = RepeaterHistoryInfo.Items(i)
                        Dim drOrderHistory As SC3150102DataSet.SC3150102GetServiceInHistoryRow = _
                            DirectCast(dtOrderHistoryNew.Rows(i), SC3150102DataSet.SC3150102GetServiceInHistoryRow)

                        '受注日を表示する.
                        Dim strOrderAcceptDate As String = STRING_SPACE
                        If Not (drOrderHistory.IsSVCIN_DELI_DATENull) Then
                            Dim acceptDate As Date = drOrderHistory.SVCIN_DELI_DATE
                            If acceptDate > Date.MinValue Then
                                If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
                                    '受注日が当日である場合、HH:mm形式にて表示.
                                    strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
                                Else
                                    '受注日が当日でない場合、YYYY/MM/dd形式にて表示.
                                    strOrderAcceptDate = DateTimeFunc.FormatDate(3, acceptDate)
                                End If
                            Else
                                strOrderAcceptDate = STRING_SPACE
                            End If
                        End If
                        CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate
                        'オーダーNoを表示する.
                        Dim stringServiceInNumber As String = STRING_SPACE
                        If Not (drOrderHistory.IsSVCIN_NUMNull) Then
                            stringServiceInNumber = ExchangeDataToHtmlString(drOrderHistory.SVCIN_NUM)
                        End If

                        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

                        ''文字列の分割位置の取得
                        'Dim StringIndex As Integer = stringServiceInNumber.IndexOf(PUNCTUATION_STRING)

                        '文字列の分割位置の取得
                        Dim StringIndex As Integer = stringServiceInNumber.IndexOf(PUNCTUATION_STRING, StringComparison.CurrentCulture)

                        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

                        Dim stringOrderNo As String = stringServiceInNumber

                        If StringIndex > 0 Then
                            '入庫管理番号を分割し、RO番号の部分を取得
                            stringOrderNo = stringServiceInNumber.Substring(0, StringIndex)
                        End If

                        CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
                        CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value = stringOrderNo

                        '販売店CDを格納する.
                        Dim stringDealerCode As String = STRING_SPACE
                        If Not (drOrderHistory.IsDLR_CDNull) Then
                            stringDealerCode = Trim(ExchangeDataToHtmlString(drOrderHistory.DLR_CD))
                        End If
                        CType(rInfo.FindControl("HiddenFieldHDealerCode"), HiddenField).Value = stringDealerCode

                        '代表整備名称を表示する.
                        Dim strSrvTypeName As String = periodicInspection

                        '代表整備項目を表示する.
                        Dim strSrvType As String = STRING_SPACE

                        If Not drOrderHistory.IsMAINTE_NAMENull Then

                            strSrvType = ExchangeDataToHtmlString(drOrderHistory.SVC_NAME_MILE)

                            '整備名称とサービス名称の内容同じ場合、サービス名称がない時なので、一般点検を表示
                            If drOrderHistory.MAINTE_NAME.Equals(drOrderHistory.SVC_NAME_MILE) Then
                                strSrvTypeName = generalInspection
                                strSrvType = ExchangeDataToHtmlString(drOrderHistory.MAINTE_NAME)
                            End If
                        End If

                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                        If Not (drOrderHistory.IsMAINTE_NAME_HISNull) AndAlso _
                           Not (String.IsNullOrEmpty(Trim(drOrderHistory.MAINTE_NAME_HIS))) Then
                            '整備履歴.整備名称がある場合は、左記を代表整備項目へ設定
                            strSrvType = ExchangeDataToHtmlString(drOrderHistory.MAINTE_NAME_HIS)
                        End If
                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END

                        CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName

                        CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType

                        '担当SAを表示する
                        Dim strSaName As String = STRING_SPACE
                        If Not (drOrderHistory.IsSTF_NAMENull) Then
                            strSaName = ExchangeDataToHtmlString(drOrderHistory.STF_NAME)
                        End If
                        CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
                    Next
                End Using
            Else
                'コントロールに空をバインドする
                Me.RepeaterHistoryInfo.DataSource = Nothing
                Me.RepeaterHistoryInfo.DataBind()
            End If
        End Using

        If Not (String.IsNullOrEmpty(Me.repairOrderNo)) Then
            Me.AllDispLink.Text = WebWordUtility.GetWord(133)
            Me.NextDispLink.Text = String.Format(CultureInfo.CurrentCulture, _
                                                 WebWordUtility.GetWord(134), _
                                                 "20")
            CType(Me.AllDispLinkDiv, HtmlContainerControl).Style("display") = "block"
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ''' <summary>
    ''' 作業内容パネルの作業内容の設定.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub SetWorkInfo()

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Logger.Info("SetWorkInfo Start")

        ''整備内容参照を取得する.
        'Dim dt As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable
        'dt = Me.businessLogic.GetServiceDetailData(Me.accountStaffContext.DlrCD, Me.repairOrderNo, Integer.Parse(Me.workSeq, CultureInfo.InvariantCulture))

        ''取得した作業内容がNothingでない場合、情報の設定を実施する.
        'If (Not IsNothing(dt)) Then
        '    'Logger.Info("SetWorkInfo GetServiceDetailData_Count=" + CType(dt.Rows.Count, String))

        '    'コントロールにバインドする.
        '    ' 選択中のチップをレコードの上へ並び替え
        '    Me.RepeaterWorkInfo.DataSource = Me.SetStallInfo(rezId, dt)
        '    Me.RepeaterWorkInfo.DataBind()

        '    '選択中の予約ID
        '    Dim selectedRezId As Long = CType(Me.rezId, Long)
        '    Dim targetRezId As Long
        '    Dim isSelectedRezId As Boolean = False
        '    'ストールが決まっていない場合の文言取得
        '    Dim strNoPlace As String = WebWordUtility.GetWord(326)  '文言：「未定」取得
        '    'ストール情報管理
        '    Dim dtStallInfo As IC3810801DataSet.IC3810801StallDataTable
        '    'ストール情報取得
        '    dtStallInfo = Me.businessLogic.GetStallInfoForWork(Me.accountStaffContext.DlrCD _
        '                                                     , Me.accountStaffContext.BrnCD _
        '                                                     , Me.repairOrderNo)
        '    'ストール情報保持 KEY:予約ID / VALUE:ストール欄表示値
        '    Dim dicStallInfo As Dictionary(Of Long, String) = New Dictionary(Of Long, String)
        '    '選択中チップの値取得
        '    dicStallInfo.Add(selectedRezId, Me.GetStallDisplayValue(selectedRezId, dtStallInfo, strNoPlace))

        '    'ストール情報取得
        '    dtWorkgroupInfo = Me.businessLogic.GetWorkgroupList(Me.accountStaffContext.DlrCD)
        '    Me.RepeaterWorkgroupInfo.DataSource = dtWorkgroupInfo
        '    Me.RepeaterWorkgroupInfo.DataBind()

        '    'データカウンタを初期化する.
        '    Dim dataCount = 0
        '    '工数単位の文字列を取得する.
        '    Dim unitWork As String
        '    unitWork = WebWordUtility.GetWord(309)
        '    unitWork = unitWork.Replace("%1", " ")

        '    'データを設定する.
        '    For i = 0 To RepeaterWorkInfo.Items.Count - 1
        '        'Logger.Info("SetWorkInfo Repeater Roop Index=" + CType(i, String))

        '        Dim rWorkInfo As Control = RepeaterWorkInfo.Items(i)

        '        'データカウンタを更新する.
        '        dataCount += 1

        '        'Logger.Info("SetWorkInfo WorkNo=" + CType(dataCount, String))
        '        CType(rWorkInfo.FindControl("LabelWorkNo"), Label).Text = CType(dataCount, String)

        '        '作業名称を表示する.
        '        Dim strSrvName As String = CType(rWorkInfo.FindControl("HiddenFieldSrvName"), HiddenField).Value
        '        If String.IsNullOrEmpty(strSrvName) Then
        '            'Logger.Info("SetWorkInfo srvName is NullOrEmpty")
        '            strSrvName = STRING_SPACE
        '        End If
        '        'Logger.Info("SetWorkInfo SrvName=" + strSrvName)
        '        CType(rWorkInfo.FindControl("LabelSrvName"), Label).Text = strSrvName

        '        '工数を表示する.
        '        Dim strWorkHours As String = CType(rWorkInfo.FindControl("HiddenFieldWorkHours"), HiddenField).Value
        '        Dim dblWorkHours As Double = INIT_WORK_HOURS
        '        If Not String.IsNullOrEmpty(strWorkHours.ToString()) Then
        '            'Logger.Info("SetWorkInfo workHours is NullOrEmpty")
        '            dblWorkHours = CType(strWorkHours, Double)
        '        End If
        '        Dim builderWorkHours As New System.Text.StringBuilder
        '        builderWorkHours.Append(dblWorkHours.ToString("0.00", CultureInfo.CurrentCulture()))
        '        builderWorkHours.Append(unitWork)
        '        'Logger.Info("SetWorkInfo WorkHours=" + builderWorkHours.ToString())
        '        CType(rWorkInfo.FindControl("LabelWorkHours"), Label).Text = builderWorkHours.ToString()

        '        '単価を表示する
        '        Dim strSellHourRate As String = CType(rWorkInfo.FindControl("HiddenFieldSellHourRate"), HiddenField).Value
        '        Dim dblSellHourRate As Double = INIT_SELL_HOUR_RATE
        '        If Not String.IsNullOrEmpty(strSellHourRate) Then
        '            dblSellHourRate = CType(strSellHourRate, Double)
        '        End If
        '        CType(rWorkInfo.FindControl("LabelSellHourRate"), Label).Text = dblSellHourRate.ToString("#,0.00", CultureInfo.CurrentCulture())

        '        '小計を表示する
        '        Dim strSellWorkPrice As String = CType(rWorkInfo.FindControl("HiddenFieldSellWorkPrice"), HiddenField).Value
        '        Dim dblSellWorkPrice As Double = INIT_SELL_WORK_PRICE
        '        If Not String.IsNullOrEmpty(strSellWorkPrice) Then
        '            dblSellWorkPrice = CType(strSellWorkPrice, Double)
        '        End If
        '        CType(rWorkInfo.FindControl("LabelSellWorkPrice"), Label).Text = dblSellWorkPrice.ToString("#,0.00", CultureInfo.CurrentCulture())

        '        'ストール名を表示する
        '        Dim strStallInfo As String = String.Empty

        '        isSelectedRezId = False  'グレーフィルターフラグ
        '        Dim strRezId As String = CType(rWorkInfo.FindControl("HiddenFieldRezId"), HiddenField).Value

        '        If String.IsNullOrEmpty(strRezId) Then
        '            strStallInfo = strNoPlace   '予約IDが無い場合は「不明」を表示させておく
        '        Else
        '            targetRezId = CType(strRezId, Long)

        '            If dicStallInfo.ContainsKey(targetRezId) Then

        '                strStallInfo = dicStallInfo(targetRezId)

        '                '選択中の予約IDと一致した場合
        '                If selectedRezId.Equals(targetRezId) Then
        '                    isSelectedRezId = True  'グレーフィルターをつけない
        '                End If
        '            Else
        '                '表示する値を取得する
        '                strStallInfo = Me.GetStallDisplayValue(targetRezId, dtStallInfo, strNoPlace)
        '                dicStallInfo.Add(targetRezId, strStallInfo)
        '            End If
        '        End If

        '        CType(rWorkInfo.FindControl("LabelStallInfo"), Label).Text = strStallInfo

        '        'グレーフィルター設定
        '        If isSelectedRezId Then
        '            CType(rWorkInfo.FindControl("LabelWorkgroupInfo"), Label).Attributes("class") = "Popoverable"   'touchされると、作業グループ選択用のPopoverが表示される。
        '        Else
        '            CType(rWorkInfo.FindControl("WorkInfoRow"), HtmlContainerControl).Attributes("class") = "S-TC-01LeftGrayZone"
        '        End If
        '    Next1 
        'End If

        'Logger.Info("SetWorkInfo End")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START " _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '整備内容参照を取得する.
        Dim dt As SC3150102DataSet.SC3150102OperationDetailInfoDataTable

        dt = Me.businessLogic.GetServiceDetailData(Me.accountStaffContext.DlrCD, _
                                                   Me.accountStaffContext.BrnCD, _
                                                   Me.repairOrderNo, _
                                                   Integer.Parse(Me.workSeq, CultureInfo.InvariantCulture))

        '取得した作業内容がNothingでない場合、情報の設定を実施する.
        If (Not IsNothing(dt)) Then

            dt = SetStallInfo(Me.rezId, dt)
            'コントロールにバインドする.
            ' 選択中のチップをレコードの上へ並び替え
            Me.RepeaterWorkInfo.DataSource = dt
            Me.RepeaterWorkInfo.DataBind()

            '選択中の予約ID
            Dim selectedRezId As Decimal = CType(Me.rezId, Decimal)
            Dim targetRezId As Decimal
            Dim isSelectedRezId As Boolean = True
            'ストールが決まっていない場合の文言取得
            Dim strNoPlace As String = WebWordUtility.GetWord(326)  '文言：「未定」取得

            'データカウンタを初期化する.
            Dim dataCount = 0
            '工数単位の文字列を取得する.
            Dim unitWork As String
            unitWork = WebWordUtility.GetWord(309)
            unitWork = unitWork.Replace("%1", " ")

            'javascript格納用文字列
            Dim javaScriptWord As StringBuilder = New StringBuilder

            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            Me.HiddenStallUseStatus.Value = Me.stallUseStatus
            '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            'データを設定する.
            For i = 0 To RepeaterWorkInfo.Items.Count - 1

                Dim rWorkInfo As Control = RepeaterWorkInfo.Items(i)

                Dim dr As SC3150102DataSet.SC3150102OperationDetailInfoRow = _
                    DirectCast(dt.Rows(i), SC3150102DataSet.SC3150102OperationDetailInfoRow)

                'データカウンタを更新する.
                dataCount += 1

                CType(rWorkInfo.FindControl("LabelWorkNo"), Label).Text = CType(dataCount, String)


                '作業名称を表示する.
                Dim strSrvName As String = ExchangeDataToHtmlString(dr.JOB_NAME)
                CType(rWorkInfo.FindControl("LabelSrvName"), Label).Text = strSrvName

                '工数を表示する.
                Dim strWorkHours As Long = dr.STD_WORKTIME
                Dim dblWorkHours As Double = INIT_WORK_HOURS
                If Not String.IsNullOrEmpty(strWorkHours.ToString(CultureInfo.CurrentCulture())) Then
                    dblWorkHours = CType(strWorkHours, Double)
                End If

                Dim builderWorkHours As New System.Text.StringBuilder
                builderWorkHours.Append(dblWorkHours.ToString("0.00", CultureInfo.CurrentCulture()))
                builderWorkHours.Append(unitWork)

                CType(rWorkInfo.FindControl("LabelWorkHours"), Label).Text = builderWorkHours.ToString()

                '2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない START
                ''スタッフグループ名を表示する.
                'Dim strStaffGropName As String = ExchangeDataToHtmlString(dr.JOB_STF_GROUP_NAME)
                'CType(rWorkInfo.FindControl("LabelWorkgroupInfo"), Label).Text = strStaffGropName
                '2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない END

                '小計を表示する
                Dim strSellWorkPrice As Long = dr.WORK_PRICE
                Dim dblSellWorkPrice As Double = INIT_SELL_WORK_PRICE

                If Not String.IsNullOrEmpty(strSellWorkPrice.ToString(CultureInfo.CurrentCulture())) Then
                    dblSellWorkPrice = CType(strSellWorkPrice, Double)
                End If
                CType(rWorkInfo.FindControl("LabelSellWorkPrice"), Label).Text = dblSellWorkPrice.ToString("#,0.00", CultureInfo.CurrentCulture())

                'ストール名を表示する
                Dim strStallInfo As String = String.Empty
                '格納用変数
                Dim rtnVal As String = String.Empty
                ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（R/O情報タブ表示でエラー発生を修正） START
                ' 'ストール省略名取得
                ' Dim strStallName As String = ExchangeDataToHtmlString(dr.STALL_NAME_SHORT)

                ' ストール省略名取得
                Dim strStallName As String = String.Empty
                If Not (dr.IsSTALL_NAME_SHORTNull) Then
                    strStallName = ExchangeDataToHtmlString(dr.STALL_NAME_SHORT)
                End If
                ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（R/O情報タブ表示でエラー発生を修正） END

                '実績開始日時取得
                Dim resultStartTime As Date = dr.RSLT_START_DATETIME
                '予定開始日時
                Dim scheduledStartTime As Date = dr.SCHE_START_DATETIME

                isSelectedRezId = True  'グレーフィルターフラグ

                '作業内容IDの設定
                Dim strRezId As String = Nothing
                If Not dr.IsJOB_DTL_IDNull Then
                    strRezId = dr.JOB_DTL_ID.ToString(CultureInfo.CurrentCulture())
                End If


                'ストール情報保持 KEY:予約ID / VALUE:ストール欄表示値
                Dim dicStallInfo As Dictionary(Of Decimal, String) = New Dictionary(Of Decimal, String)

                If Not strStallName.Equals(DEFAULT_VALUE) Then

                    '初期値に実績開始日時を設定
                    Dim strStarttime As Date = resultStartTime

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

                    ''実績開始日時が省略値の場合
                    'If dr.RSLT_START_DATETIME = Date.Parse(MINDATE) Then
                    '    '予定開始日時を設定
                    '    strStarttime = scheduledStartTime
                    'End If

                    '実績開始日時が省略値の場合
                    If dr.RSLT_START_DATETIME = Date.Parse(MINDATE, CultureInfo.CurrentCulture) Then
                        '予定開始日時を設定
                        strStarttime = scheduledStartTime
                    End If

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

                    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    'ストール欄に表示用に形成（ストール名 + 改行 + 開始時間 + "-" + 終了時間）
                    'rtnVal = strStallName & _
                    '         CHAR_MULTI_LINE & _
                    '         Me.ConvertDisplayStartTime(strStarttime)

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

                    ''ストール欄に表示用に形成（ストール名 + スペース + 開始時間 + "-" + 終了時間）
                    'rtnVal = strStallName & _
                    '         CHAR_HTML_SPACE & _
                    '         Me.ConvertDisplayStartTime(strStarttime)

                    'ストール欄に表示用に形成（ストール名 + スペース + 開始時間 + "-" + 終了時間）
                    Dim sb As New StringBuilder
                    sb.Append(strStallName)
                    sb.Append(CHAR_HTML_SPACE)
                    sb.Append(Me.ConvertDisplayStartTime(strStarttime))

                    '値を設定
                    rtnVal = sb.ToString

                    '初期化
                    sb = Nothing

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

                    '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                End If

                '選択中チップの値取得
                dicStallInfo.Add(selectedRezId, rtnVal)
                '完成検査ステータスを取得
                Dim strInspectionStatus As String = INSPECTION_INCOMPLETE
                If Not String.IsNullOrEmpty(dr.INSPECTION_STATUS) Then
                    strInspectionStatus = dr.INSPECTION_STATUS
                End If

                '着工指示フラグを取得
                Dim strStartWorkInstructFlg As String = INSTRUCT_UNDIRECTION
                If Not String.IsNullOrEmpty(dr.STARTWORK_INSTRUCT_FLG) Then
                    strStartWorkInstructFlg = dr.STARTWORK_INSTRUCT_FLG
                End If

                '選択チップの作業内容ID
                targetRezId = CType(strRezId, Decimal)

                '整備の作業内容IDが選択チップの作業内容IDか判定
                If dicStallInfo.ContainsKey(targetRezId) Then

                    strStallInfo = dicStallInfo(targetRezId)
                Else
                    '表示する値を取得する
                    strStallInfo = rtnVal
                    dicStallInfo.Add(targetRezId, strStallInfo)
                End If

                '選択中のチップの作業内容IDが一致しない、
                '完成検査ステータスが完成検査完了の場合
                If (Not selectedRezId.Equals(targetRezId)) OrElse _
                   (strInspectionStatus.Equals("2")) Then
                    isSelectedRezId = False  'グレーフィルターをつける
                End If

                'グレーフィルター設定
                If Not (isSelectedRezId) Then
                    CType(rWorkInfo.FindControl("WorkInfoRow"), HtmlContainerControl).Attributes("class") = "S-TC-01LeftGrayZone"
                End If

                '着工指示フラグが未着工の場合「不明」を表示
                If strStartWorkInstructFlg.Equals("0") Then
                    strStallInfo = strNoPlace
                    CType(rWorkInfo.FindControl("WorkInfoRow"), HtmlContainerControl).Attributes("class") = "S-TC-01LeftGrayZone2"
                End If

                CType(rWorkInfo.FindControl("LabelStallInfo"), Label).Text = strStallInfo


                'RO連番の取得.
                Dim RepairOrderSequence As String = RO_SEQ_DEFAULT
                If Not dr.IsRO_SEQNull Then
                    RepairOrderSequence = dr.RO_SEQ.ToString(CultureInfo.CurrentCulture())
                End If

                'ROアイコンの設定をするjavaScript
                javaScriptWord.Append(" var temp = document.getElementById('RepairOrderIcon');" _
                                    + "temp.id= 'RepairOrderIcon" + i.ToString(CultureInfo.CurrentCulture()) + "';")

                If Not RepairOrderSequence.Equals("0") Then
                    '追加作業アイコンに設定
                    javaScriptWord.Append(" var temp = document.getElementById('RepairOrderIcon" + i.ToString(CultureInfo.CurrentCulture()) + "');" _
                                        + "temp.className = 'imgicon02';")
                    '作業連番を設定
                    javaScriptWord.Append(" var temp2 = document.getElementById('RepairOrderIcon" + i.ToString(CultureInfo.CurrentCulture()) + "');" _
                                       + "temp2.innerHTML = '<p>" + RepairOrderSequence + "</p>';")
                End If

                '作業指示IDの設定
                Dim strJobInstructId As String = Nothing
                If Not dr.IsJOB_INSTRUCT_IDNull Then
                    strJobInstructId = dr.JOB_INSTRUCT_ID.ToString(CultureInfo.CurrentCulture())
                End If

                CType(rWorkInfo.FindControl("HiddenJobInstructId"), HiddenField).Value = strJobInstructId

                '作業指示枝番の設定
                Dim strJobInstructSeq As String = Nothing
                If Not dr.IsJOB_INSTRUCT_SEQNull Then
                    strJobInstructSeq = dr.JOB_INSTRUCT_SEQ.ToString(CultureInfo.CurrentCulture())
                End If

                CType(rWorkInfo.FindControl("HiddenJobInstructSeq"), HiddenField).Value = strJobInstructSeq

                '実績開始日時の設定
                Dim strJobRestStartDateTime As String = Nothing
                If (Not dr.IsJOB_RSLT_START_DATETIMENull) AndAlso _
                 (Not dr.JOB_RSLT_START_DATETIME = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", Nothing)) Then
                    strJobRestStartDateTime = dr.JOB_RSLT_START_DATETIME.ToString(CultureInfo.CurrentCulture())
                End If

                CType(rWorkInfo.FindControl("HiddenRsltSTratDatetime"), HiddenField).Value = strJobRestStartDateTime

                '実績終了日時の設定
                Dim strJobRestEndDateTime As String = Nothing
                If (Not dr.IsJOB_RSLT_END_DATETIMENull) AndAlso _
                 (Not dr.JOB_RSLT_END_DATETIME = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", Nothing)) Then
                    strJobRestEndDateTime = dr.JOB_RSLT_END_DATETIME.ToString(CultureInfo.CurrentCulture())
                End If

                CType(rWorkInfo.FindControl("HiddenRsltEndDatetime"), HiddenField).Value = strJobRestEndDateTime

                '作業ステータスの設定
                Dim strJobStatus As String = Nothing
                If Not dr.IsJOB_STATUSNull Then
                    strJobStatus = dr.JOB_STATUS
                End If

                CType(rWorkInfo.FindControl("HiddenJobStatus"), HiddenField).Value = strJobStatus


                'javaScript実行
                javaScriptWord.Append("JobActionClassConvert( " + i.ToString(CultureInfo.CurrentCulture()) + ");")

                'javaScript実行
                javaScriptWord.Append(" JobActionBattonConvert( " + i.ToString(CultureInfo.CurrentCulture()) + ");")
            Next
            'javaScript実行
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "value", javaScriptWord.ToString, True)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
#Region "ストール欄形成"
    ''' <summary>
    ''' 作業項目テーブル形成
    ''' </summary>
    ''' <param name="selectedRezId"></param>
    ''' <param name="dtOrigin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetStallInfo(ByVal selectedRezId As String, ByVal dtOrigin As SC3150102DataSet.SC3150102OperationDetailInfoDataTable) _
        As SC3150102DataSet.SC3150102OperationDetailInfoDataTable

        'Private Function SetStallInfo(ByVal selectedRezId As String, ByVal dtOrigin As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable) _
        '    As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

        'Dim dt As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable = DirectCast(dtOrigin.Clone(), IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable)

        ''予約IDが一致するデータ抜き出し
        'Dim arySelectStallInfo As IC3801110DataSet.IC3801110SrvDetailDataTableCommRow() _
        '    = DirectCast(dtOrigin.Select(String.Format(CultureInfo.InvariantCulture, "REZID = '{0}'", selectedRezId) _
        '        , String.Empty, DataViewRowState.CurrentRows), IC3801110DataSet.IC3801110SrvDetailDataTableCommRow())

        'For Each row As IC3801110DataSet.IC3801110SrvDetailDataTableCommRow In arySelectStallInfo
        '    dt.ImportRow(row)           '一致した行を抜き出し
        '    dtOrigin.Rows.Remove(row)   '一致した行を削除
        'Next

        Dim dt As SC3150102DataSet.SC3150102OperationDetailInfoDataTable = DirectCast(dtOrigin.Clone(), SC3150102DataSet.SC3150102OperationDetailInfoDataTable)

        '予約IDが一致するデータ抜き出し
        Dim arySelectStallInfo As SC3150102DataSet.SC3150102OperationDetailInfoRow() _
            = DirectCast(dtOrigin.Select(String.Format(CultureInfo.InvariantCulture, "JOB_DTL_ID = '{0}'", selectedRezId) _
                , String.Empty, DataViewRowState.CurrentRows), SC3150102DataSet.SC3150102OperationDetailInfoRow())

        For Each row As SC3150102DataSet.SC3150102OperationDetailInfoRow In arySelectStallInfo

            '着工指示フラグが指示済みの場合
            If row.STARTWORK_INSTRUCT_FLG.Equals(STARTWORK_INSTRUCT_FLG_NO) Then
                dt.ImportRow(row)           '一致した行を抜き出し
                dtOrigin.Rows.Remove(row)   '一致した行を削除
            End If

        Next

        'マージ
        dt.Merge(dtOrigin)

        Return dt

    End Function

    ' ''' <summary>
    ' ''' ストール欄表示値形成
    ' ''' </summary>
    ' ''' <param name="rzId">予約ID</param>
    ' ''' <param name="dt">ストール情報データテーブル</param>
    ' ''' <returns>ストール欄表示値</returns>
    ' ''' <remarks></remarks>
    'Private Function GetStallDisplayValue(ByVal rzId As Long, ByVal dt As IC3810801DataSet.IC3810801StallDataTable, Optional ByVal strReplace As String = "") As String

    '    Dim rtnVal As String = String.Empty
    '    Dim decRezId As Decimal = CType(rzId, Decimal)

    '    For Each dr As IC3810801DataSet.IC3810801StallRow In dt
    '        '引数の予約IDと一致したレコードがある場合
    '        If decRezId.Equals(dr.REZID) Then
    '            'ストール名がブランクの場合
    '            If Not dr.IsSTALLNAME_SNull OrElse Not String.IsNullOrEmpty(dr.STALLNAME_S) Then
    '                'ストール欄に表示用に形成（ストール名 + 改行 + 開始時間 + "-" + 終了時間）
    '                rtnVal = dr.STALLNAME_S & _
    '                         CHAR_MULTI_LINE & _
    '                         Me.ConvertDisplayStartTime(dr.STARTTIME)
    '                'DateTimeFunc.FormatDate(14, dr.STARTTIME)
    '            End If
    '            '一致する予約IDは１件しかない
    '            Exit For
    '        End If
    '    Next

    '    If String.IsNullOrEmpty(rtnVal) Then
    '        '一致する予約IDがない場合、「不明」を設定
    '        rtnVal = strReplace
    '    End If

    '    Return rtnVal
    'End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    ' ''' <summary>
    ' ''' 作業項目欄．ストール表示開始時間変換
    ' ''' </summary>
    ' ''' <param name="startTime">開始時刻</param>
    ' ''' <returns>表示用開始時刻文字列</returns>
    ' ''' <remarks>当日はHH:mm / 当日以外はMM/DD HH:mm</remarks>
    'Private Function ConvertDisplayStartTime(ByVal startTime As DateTime) As String

    '    Dim rtnVal = String.Empty

    '    '当日日付を取得
    '    Dim nowDateTime As Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD, Me.accountStaffContext.BrnCD)
    '    Dim nowDate As Date = New Date(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day)
    '    '開始日付を取得
    '    Dim startDate As Date = New Date(startTime.Year, startTime.Month, startTime.Day)

    '    Dim strTime As String = DateTimeFunc.FormatDate(14, startTime)  'HH:mm形式に変換

    '    If nowDate.Equals(startDate) Then
    '        rtnVal = strTime
    '    Else
    '        '当日以外の場合、MM/DD HH:mm
    '        rtnVal = String.Format(CultureInfo.InvariantCulture, "{0}", DateTimeFunc.FormatDate(11, startTime))
    '    End If

    '    Return rtnVal
    'End Function

    ''' <summary>
    ''' 作業項目欄．ストール表示開始時間変換
    ''' </summary>
    ''' <param name="inStartTime">開始時刻</param>
    ''' <returns>表示用開始時刻文字列</returns>
    ''' <remarks>当日はHH:mm / 当日以外はMM/DD HH:mm</remarks>
    Private Function ConvertDisplayStartTime(ByVal inStartTime As DateTime) As String

        Dim rtnVal = String.Empty

        '当日日付を取得
        Dim nowDateTime As Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD, Me.accountStaffContext.BrnCD)
        Dim nowDate As Date = New Date(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day)
        '開始日付を取得
        Dim startDate As Date = New Date(inStartTime.Year, inStartTime.Month, inStartTime.Day)

        Dim strTime As String = DateTimeFunc.FormatDate(14, inStartTime)  'HH:mm形式に変換

        If nowDate.Equals(startDate) Then
            rtnVal = strTime
        Else
            '当日以外の場合、MM/DD HH:mm
            rtnVal = String.Format(CultureInfo.InvariantCulture, "{0}", DateTimeFunc.FormatDate(11, inStartTime))
        End If

        Return rtnVal
    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

#End Region
    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

    ''' <summary>
    ''' 作業内容パネルの部品項目の設定.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない
    ''' </history>
    Private Sub SetPartsInfo()

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Logger.Info("SetPartsInfo Start")

        ''部品明細数を初期化する.
        'Me.HiddenFieldPartsCount.Value = "0"
        ''部品明細のB/O数を初期化する.
        'Me.HiddenFieldPartsBackOrderCount.Value = "0"

        ''部品明細参照を取得する.
        'Dim dt As IC3801113DataSet.IC3801113PartsDataTable
        '' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        'dt = Me.businessLogic.GetPartsDetailData(Me.accountStaffContext.DlrCD, Me.repairOrderNo, Integer.Parse(Me.workSeq, CultureInfo.InvariantCulture))
        ''dt = Me.businessLogic.GetPartsDetailData(Me.accountStaffContext.DlrCD, Me.repairOrderNo, Me.childNumber)
        '' 2012/06/01 KN 西田 STEP1 重要課題対応 END
        ''擬似的なデータを作成し、取得する
        ''Dim dt As DataTable = CreateSamplePartsInfoDataTable()

        ''2012/03/14 nishida 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更 START
        ''B/O項目を表示する文言を取得する
        'Dim strBackOrder As String = WebWordUtility.GetWord(321)
        ''2012/03/14 nishida 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更 END

        ''取得した部品明細がNothingでない場合、情報の設定を実施する.
        'If (Not IsNothing(dt)) Then
        '    'Logger.Info("SetPartsInfo GetPartsDetailData_Count=" + CType(dt.Rows.Count, String))

        '    'コントロールにバインドする.
        '    Me.RepeaterPartsInfo.DataSource = dt
        '    Me.RepeaterPartsInfo.DataBind()

        '    '2012/04/07 KN日比野 【SERVICE_1】企画_プレユーザテストの不具合修正No3　部品エリアの単位の文言対応 START
        '    Dim partsUnitWord As String = WebWordUtility.GetWord(325)
        '    '2012/04/07 KN日比野 【SERVICE_1】企画_プレユーザテストの不具合修正No3　部品エリアの単位の文言対応 END

        '    'データカウンタを初期化する.
        '    Dim dataCount = 0
        '    'B/Oカウンタを初期化する.
        '    Dim backOrderCount = 0
        '    '部品明細数を格納する.
        '    Me.HiddenFieldPartsCount.Value = CType(RepeaterPartsInfo.Items.Count, String)

        '    'データを設定する.
        '    For i = 0 To RepeaterPartsInfo.Items.Count - 1
        '        'Logger.Info("SetPartsInfo Repeater Roop Index=" + CType(i, String))

        '        Dim partsInfo As Control = RepeaterPartsInfo.Items(i)

        '        'データカウンタを更新する.
        '        dataCount += 1
        '        'Logger.Info("SetPartsInfo PartsNo=" + CType(dataCount, String))
        '        CType(partsInfo.FindControl("LiteralPartsNo"), Literal).Text = CType(dataCount, String)

        '        '部品名称を表示する.
        '        Dim strPartsName As String = CType(partsInfo.FindControl("HiddenFieldPartsName"), HiddenField).Value
        '        If String.IsNullOrEmpty(strPartsName) Then
        '            'Logger.Info("SetPartsInfo partsName is NullOrEmpty")
        '            strPartsName = STRING_SPACE
        '        End If
        '        'Logger.Info("SetPartsInfo PartsName=" + strPartsName)
        '        CType(partsInfo.FindControl("LiteralPartsName"), Literal).Text = strPartsName

        '        '区分を表示する.
        '        Dim strPartsType As String = CType(partsInfo.FindControl("HiddenFieldPartsType"), HiddenField).Value
        '        If String.IsNullOrEmpty(strPartsType) Then
        '            'Logger.Info("SetPartsInfo partsType is NullOrEmpty")
        '            strPartsType = STRING_SPACE
        '        End If
        '        'Logger.Info("SetPartsInfo PartsType=" + strPartsType)
        '        CType(partsInfo.FindControl("LiteralPartsType"), Literal).Text = strPartsType

        '        '数量を表示する.
        '        Dim strPartsQuantity As String = CType(partsInfo.FindControl("HiddenFieldPartsQuantity"), HiddenField).Value
        '        Dim dblPartsQuantity As Double = INIT_PARTS_QUANTITY
        '        If Not String.IsNullOrEmpty(strPartsQuantity) Then
        '            'Logger.Info("SetPartsInfo partsQuantity is NullOrEmpty")
        '            dblPartsQuantity = CType(strPartsQuantity, Double)
        '        End If
        '        'Logger.Info("SetPartsInfo PartsQuantity=" + CType(dblPartsQuantity, String))
        '        CType(partsInfo.FindControl("LiteralPartsQuantity"), Literal).Text = dblPartsQuantity.ToString("0", CultureInfo.CurrentCulture())

        '        '単位を表示する.
        '        Dim strPartsUnit As String = CType(partsInfo.FindControl("HiddenFieldPartsUnit"), HiddenField).Value
        '        If String.IsNullOrEmpty(strPartsUnit) Then
        '            'Logger.Info("SetPartsInfo partsUnit is NullOrEmpty")
        '            strPartsUnit = STRING_SPACE
        '        Else
        '            If strPartsUnit.Equals("K") Then
        '                strPartsUnit = partsUnitWord        '部品エリアの単位の文言
        '            End If
        '        End If
        '        'Logger.Info("SetPartsInfo PartsUnit=" + strPartsUnit)
        '        CType(partsInfo.FindControl("LiteralPartsUnit"), Literal).Text = strPartsUnit

        '        'B/Oを表示する.
        '        Dim strPartsOrderStatus As String = CType(partsInfo.FindControl("HiddenFieldPartsOrderStatus"), HiddenField).Value
        '        If Not String.IsNullOrEmpty(strPartsOrderStatus) AndAlso PARTS_BACK_ORDER_DISP_FLG.Equals(strPartsOrderStatus.Trim()) Then
        '            CType(partsInfo.FindControl("LiteralPartsOrderStatus"), Literal).Text = strBackOrder
        '            'B/Oカウンタを更新する.
        '            backOrderCount += 1
        '        End If

        '    Next

        '    'B/Oカウンタを格納する.
        '    'Logger.Info("SetPartsInfo backOrderCount=" + CType(backOrderCount, String))
        '    Me.HiddenFieldPartsBackOrderCount.Value = CType(backOrderCount, String)
        'End If

        ''Logger.Info("SetPartsInfo PartsCount=" + Me.HiddenFieldPartsCount.Value)
        'Logger.Info("SetPartsInfo End")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START.RO_SEQ_COUNT:{2} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , Me.repairOrderDataTable(0).ADD_SVC_COUNT.ToString(CultureInfo.CurrentCulture())))

        '部品明細数を初期化する.
        Me.HiddenFieldPartsCount.Value = "0"

        ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
        ' '部品明細のB/O数を初期化する.
        ' Me.HiddenFieldPartsBackOrderCount.Value = "0"
        ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

        '引数用文字列配列
        Dim workSeqArrayList As New ArrayList

        'RO作業連番の数だけ繰り替える
        For i = 0 To Me.repairOrderDataTable(0).ADD_SVC_COUNT - 1
            'RO作業連番格納
            workSeqArrayList.Add(Me.repairOrderDataTable(CType(i, Integer)).RO_SEQ)
        Next

        '部品詳細参照を取得する.
        Dim dt As IC3802504DataSet.IC3802504PartsDetailDataTable
        ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
        dt = Me.businessLogic.GetPartsDetailData(Me.accountStaffContext.DlrCD, Me.accountStaffContext.BrnCD, Me.repairOrderNo, workSeqArrayList)

        ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
        ' 'B/O項目を表示する文言を取得する
        ' Dim strBackOrder As String = WebWordUtility.GetWord(321)
        ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

        '取得した部品明細がNothingでなく、レコードが1行以上ある場合、情報の設定を実施する.
        If (Not IsNothing(dt)) AndAlso (dt.Rows.Count > 0) Then

            '0行目のリザルトコードの確認
            If IC3802504BusinessLogic.Result.Success <> dt.Item(0).ResultCode Then
                'リザルトコードが「0」以外の場合、エラーメッセージ表示
                Select Case dt.Item(0).ResultCode
                    Case IC3802504BusinessLogic.Result.TimeOutError
                        ' タイムアウトエラー
                        MyBase.ShowMessageBox(901)
                    Case IC3802504BusinessLogic.Result.DmsError
                        ' 基幹側のエラー

                        '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し START

                        'MyBase.ShowMessageBox(902)
                        MyBase.ShowMessageBox(926)

                        '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し END

                    Case IC3802504BusinessLogic.Result.OtherError
                        ' その他のエラー
                        MyBase.ShowMessageBox(903)

                        '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し START

                    Case IC3802504BusinessLogic.Result.XmlParseError
                        ' XMLの解析エラー
                        MyBase.ShowMessageBox(924)

                    Case IC3802504BusinessLogic.Result.XmlMandatoryItemsError
                        ' XMLの必須タグエラー
                        MyBase.ShowMessageBox(925)

                        '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し END

                End Select

                Exit Sub
            End If

            'コントロールにバインドする.
            Me.RepeaterPartsInfo.DataSource = dt
            Me.RepeaterPartsInfo.DataBind()

            Dim partsUnitWord As String = WebWordUtility.GetWord(325)

            'データカウンタを初期化する.
            Dim dataCount = 0

            ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
            ' 'B/Oカウンタを初期化する.
            ' Dim backOrderCount = 0
            ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

            '部品明細数を格納する.
            Me.HiddenFieldPartsCount.Value = CType(RepeaterPartsInfo.Items.Count, String)

            'データを設定する.
            For i = 0 To RepeaterPartsInfo.Items.Count - 1

                Dim partsInfo As Control = RepeaterPartsInfo.Items(i)

                'データカウンタを更新する.
                dataCount += 1
                CType(partsInfo.FindControl("LiteralPartsNo"), Label).Text = CType(dataCount, String)

                '部品名称を表示する.
                Dim strPartsName As String = CType(partsInfo.FindControl("HiddenFieldPartsName"), HiddenField).Value
                CType(partsInfo.FindControl("LiteralPartsName"), Label).Text = ExchangeDataToHtmlString(strPartsName)

                '区分を表示する.
                Dim strPartsType As String = CType(partsInfo.FindControl("HiddenFieldPartsType"), HiddenField).Value
                CType(partsInfo.FindControl("LiteralPartsType"), Label).Text = ExchangeDataToHtmlString(strPartsType)

                '数量を表示する.
                Dim strPartsQuantity As String = CType(partsInfo.FindControl("HiddenFieldPartsQuantity"), HiddenField).Value
                Dim dblPartsQuantity As New StringBuilder

                If Not String.IsNullOrEmpty(strPartsQuantity) Then
                    dblPartsQuantity.Append(strPartsQuantity)
                    dblPartsQuantity.Append(partsUnitWord)
                Else
                    dblPartsQuantity.Append(INIT_PARTS_QUANTITY.ToString("0", CultureInfo.CurrentCulture()))
                End If
                CType(partsInfo.FindControl("LiteralPartsQuantity"), Label).Text = dblPartsQuantity.ToString

                ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
                ' 'B/Oを表示する.
                ' Dim strPartsOrderStatus As String = CType(partsInfo.FindControl("HiddenFieldPartsOrderStatus"), HiddenField).Value
                ' If Not String.IsNullOrEmpty(strPartsOrderStatus) AndAlso PARTS_BACK_ORDER_DISP_FLG.Equals(strPartsOrderStatus.Trim()) Then
                '     CType(partsInfo.FindControl("LiteralPartsOrderStatus"), Label).Text = strBackOrder
                '     'B/Oカウンタを更新する.
                '     backOrderCount += 1
                ' End If
                ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END
            Next

            ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
            ' 'B/Oカウンタを格納する.
            ' Me.HiddenFieldPartsBackOrderCount.Value = CType(backOrderCount, String)
            ' 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    End Sub

    ''' <summary>
    ''' 基本情報パネルのデータを表示.
    ''' </summary>
    ''' <param name="saChipId">SAチップID</param>
    ''' <param name="basRezId">基幹予約ID</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="BraunchCode">店舗コード</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="vin">VIN</param>
    ''' <remarks></remarks>
    Private Sub SetBasicInfoData(ByVal saChipId As Long, _
                                 ByVal basRezId As String, _
                                 ByVal dealerCode As String, _
                                 ByVal braunchCode As String, _
                                 ByVal account As String, _
                                 ByVal vin As String)

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        'Logger.Info("SetBasicInfoData Start")

        '基本情報パネル
        'Me.LabelBasicTab.Text = "基本情報"

        ''燃料初期状態
        'Me.HiddenField05_Fuel.Value = repairOrderData("fuelStatus").ToString()
        ''Logger.Info("SetBasicInfoData fuelStatus=" + repairOrderData("fuelStatus").ToString())

        ''オーディオ初期状態
        'Me.HiddenField05_Audio.Value = repairOrderData("audioStatus").ToString()
        ''Logger.Info("SetBasicInfoData audioStatus=" + repairOrderData("audioStatus").ToString())

        ''エアコン初期状態
        'Me.HiddenField05_AirConditioner.Value = repairOrderData("airConStatus").ToString()
        ''Logger.Info("SetBasicInfoData airConStatus=" + repairOrderData("airConStatus").ToString())

        ''付属品1初期状態
        'Me.HiddenField05_Accessory1.Value = repairOrderData("accesOne").ToString()
        ''Logger.Info("SetBasicInfoData accesOne=" + repairOrderData("accesOne").ToString())

        ''付属品2初期状態
        'Me.HiddenField05_Accessory2.Value = repairOrderData("accesTwo").ToString()
        ''Logger.Info("SetBasicInfoData accesTwo=" + repairOrderData("accesTwo").ToString())

        ''付属品3初期状態
        'Me.HiddenField05_Accessory3.Value = repairOrderData("accesThree").ToString()
        ''Logger.Info("SetBasicInfoData accesThree=" + repairOrderData("accesThree").ToString())

        ''付属品4初期状態
        ''Me.HiddenField05_Accessory4.Value = repairOrderData("accesFour").ToString()
        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(付属品4の設定値変更) START
        'Me.HiddenField05_Accessory4.Value = repairOrderData("ACCESFOUR").ToString()
        ''Me.HiddenField05_Accessory4.Value = repairOrderData("ccesFour").ToString()
        ''2012/03/13 KN 上田 【SERVICE_1】課題管理番号-BMTS_0312_YW_01の不具合修正 R/O情報表示項目の不具合(付属品4の設定値変更) END
        ''Logger.Info("SetBasicInfoData accesFour=" + repairOrderData("ccesFour").ToString())

        ''付属品5初期状態
        'Me.HiddenField05_Accessory5.Value = repairOrderData("accesFive").ToString()
        ''Logger.Info("SetBasicInfoData accesFive=" + repairOrderData("accesFive").ToString())

        ''付属品6初期状態
        'Me.HiddenField05_Accessory6.Value = repairOrderData("accesSix").ToString()
        ''Logger.Info("SetBasicInfoData accesSix=" + repairOrderData("accesSix").ToString())

        ''エアコン温度初期状態
        'Dim airControlerTemp As New StringBuilder
        'airControlerTemp.Append(ExchangeDataToHtmlString(repairOrderData("airContpr").ToString()))
        'airControlerTemp.Append(WebWordUtility.GetWord(121).Replace("%1", " "))
        'Me.LiteralAirConditionerTemperature.Text = airControlerTemp.ToString()
        ''Logger.Info("SetBasicInfoData airConditionerTemp=" + airControlerTemp.ToString())

        ''貴重品メモ初期状態
        'Dim valuablesMemo As String
        'valuablesMemo = ExchangeDataToHtmlString(repairOrderData("valMemo").ToString())
        ''2012/04/05 KN 西田　企画_プレユーザーテスト課題No.112　名前の「…」部分をﾀｯﾌﾟしても全部が表示されない START
        'Me.LblValuablesMemo.Text = valuablesMemo
        ''Me.LiteralValuablesMemo.Text = valuablesMemo
        ''2012/04/05 KN 西田　企画_プレユーザーテスト課題No.112　名前の「…」部分をﾀｯﾌﾟしても全部が表示されない END
        ''Logger.Info("SetBasicInfoData valMemo=" + valuablesMemo)

        'Logger.Info("SetBasicInfoData End")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.accountStaffContext = StaffContext.Current



        Dim displayUrl As String = Nothing

        displayUrl = Me.businessLogic.GetDisplay(displayNumber20)

        Dim systemEnv As New SystemEnvSetting

        ' 画面連携ドメイン取得
        Dim linkageDomain As String = _
            systemEnv.GetSystemEnvSetting(OTHER_LINKAGE_DOMAIN).PARAMVALUE

        'スペースが入ってる場合、空文字にする
        basRezId = DeleteSpaceString(basRezId)
        repairOrderNo = DeleteSpaceString(repairOrderNo)
        workSeq = DeleteSpaceString(workSeq)

        'iframeに連携する画面のURLを設定
        Me.HiddenFieldCutDtlIframeUrl.Value = String.Format(CultureInfo.CurrentCulture _
                                                            , displayUrl _
                                                            , linkageDomain _
                                                            , dealerCode _
                                                            , braunchCode _
                                                            , account _
                                                            , saChipId _
                                                            , basRezId _
                                                            , repairOrderNo _
                                                            , workSeq _
                                                            , vin _
                                                            , ViewMode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END URL:{2}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , Me.HiddenFieldCutDtlIframeUrl.Value))

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ''' <summary>
    ''' ご用命事項パネルのデータを表示.
    ''' </summary>
    ''' <param name="saChipId">SAチップID</param>
    ''' <param name="basRezId">基幹予約ID</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="BraunchCode">店舗コード</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="vin">VIN</param>
    ''' <remarks></remarks>
    Private Sub SetOrdersInfoData(ByVal saChipId As Long, _
                                  ByVal basRezId As String, _
                                  ByVal dealerCode As String, _
                                  ByVal BraunchCode As String, _
                                  ByVal account As String, _
                                  ByVal vin As String)

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Logger.Info("SetOrdersInfoData Start")

        'ご用命項目パネルタイトル
        'Me.LabelOrdersTab.Text = "ご用命事項"

        ''ご用命事項エリア
        'Me.HiddenField07_ExchangeParts.Value = repairOrderData("CHANGEDACCFLAG").ToString() '部品交換後処理
        ''Logger.Info("SetOrdersInfoData changeDaccFlag=" + repairOrderData("CHANGEDACCFLAG").ToString())
        'Me.HiddenField07_Waiting.Value = repairOrderData("WAITFLAG").ToString() '待ち方
        ''Logger.Info("SetOrdersInfoData waitFlag=" + repairOrderData("WAITFLAG").ToString())
        'Me.HiddenField07_Washing.Value = repairOrderData("CLEANFLAG").ToString() '洗車
        ''Logger.Info("SetOrdersInfoData cleanFlag=" + repairOrderData("CLEANFLAG").ToString())
        ''2012/03/10 nishida BMTS_0306_YW_03 PAYMENTFLAGがCHARの2byteのため、スペース削除 START
        'Me.HiddenField07_Payment.Value = repairOrderData("PAYMENTFLAG").ToString().Trim() '支払方法
        ''Me.HiddenField07_Payment.Value = repairOrderData("PAYMENTFLAG").ToString() '支払方法
        ''2012/03/10 nishida BMTS_0306_YW_03 PAYMENTFLAGがCHARの2byteのため、スペース削除 END
        ''Logger.Info("SetOrdersInfoData paymentFlag=" + repairOrderData("PAYMENTFLAG").ToString())
        'Me.HiddenField07_Csi.Value = repairOrderData("CSIFLAG").ToString() 'CSI時間
        ''Logger.Info("SetOrdersInfoData csiFlag=" + repairOrderData("CSIFLAG").ToString())
        'Dim invoiceAddress As String = ExchangeDataToHtmlString(repairOrderData("INVOICEADDRESS").ToString()) '請求書送付先
        'Me.LiteralInvoiceAddress.Text = invoiceAddress.Replace(vbCrLf, "<br/>")
        ''Logger.Info("SetOrdersInfoData invoiceAddress=" + invoiceAddress)

        'Dim orderMemo As String = ExchangeDataToHtmlString(repairOrderData("orderMemo").ToString()) 'ご用命事項
        'Me.LiteralOrderMemo.Text = orderMemo.Replace(vbCrLf, "<br/>")
        ''Logger.Info("SetOrdersInfoData orderMemo=" + orderMemo)

        'Me.HiddenField07_Warning.Value = repairOrderData("WNGFLAG").ToString() 'WNG
        ''Logger.Info("SetOrdersInfoData wngFlag=" + repairOrderData("WNGFLAG").ToString())
        'Me.HiddenField07_Occurrence.Value = repairOrderData("TRBOCCURTIME").ToString() '故障発生時間
        ''Logger.Info("SetOrdersInfoData trbOccurTime=" + repairOrderData("TRBOCCURTIME").ToString())
        'Me.HiddenField07_Frequency.Value = repairOrderData("TRBOCCURCYC").ToString() '故障発生頻度
        ''Logger.Info("SetOrdersInfoData trbOccurCyc=" + repairOrderData("TRBOCCURCYC").ToString())
        'Me.HiddenField07_Reappear.Value = repairOrderData("RECURRENCEFLAG").ToString() '再現可能
        ''Logger.Info("SetOrdersInfoData recurrenceFlag" + repairOrderData("RECURRENCEFLAG").ToString())
        'Me.HiddenField07_WaterT.Value = repairOrderData("wtemFlag").ToString() '水温フラグ
        ''Logger.Info("SetOrdersInfoData waterTempFlag=" + repairOrderData("wtemFlag").ToString())
        'Dim waterTemperature As String = ExchangeDataToHtmlString(repairOrderData("WTEMP").ToString()) '水温
        ''Me.TextBoxHearingWTemperature.Text = ExchangeDataToHtmlString(repairOrderData("WTEMP").ToString())
        'Me.CustomLabelHearingWTemperature.Text = waterTemperature
        ''Logger.Info("SetOrdersInfoData waterTemp=" + waterTemperature)
        'Me.HiddenField07_Temperature.Value = repairOrderData("AIRTEMPFLAG").ToString() '気温フラグ
        ''Logger.Info("SetOrdersInfoData airTempFlag=" + repairOrderData("AIRTEMPFLAG").ToString())
        'Dim airTemperature As String = ExchangeDataToHtmlString(repairOrderData("AIRTEMP").ToString()) '気温
        ''Me.TextBoxHearingTemperature.Text = ExchangeDataToHtmlString(repairOrderData("AIRTEMP").ToString())
        'Me.CustomLabelHearingTemperature.Text = airTemperature
        ''Logger.Info("SetOrdersInfoData airTemp=" + airTemperature)
        'Me.HiddenField07_Place.Value = repairOrderData("OCCURPLACEFLAG").ToString() '発生場所
        ''Logger.Info("SetOrdersInfoData occurPlaceFlag=" + repairOrderData("OCCURPLACEFLAG").ToString())
        'Me.HiddenField07_TrafficJam.Value = repairOrderData("TRAFFICJAMFLAG").ToString() '渋滞状況
        ''Logger.Info("SetOrdersInfoData trafficJamFlag=" + repairOrderData("TRAFFICJAMFLAG").ToString())
        'Me.HiddenField07_CarStatus_Startup.Value = repairOrderData("VHCSTATUS1").ToString() '車両状態1（起動時）
        ''Logger.Info("SetOrdersInfoData vhcStatus1=" + repairOrderData("VHCSTATUS1").ToString())
        'Me.HiddenField07_CarStatus_Idling.Value = repairOrderData("VHCSTATUS2").ToString() '車両状態2（アイドリング時）
        ''Logger.Info("SetOrdersInfoData vhcStatus2=" + repairOrderData("VHCSTATUS2").ToString())
        'Me.HiddenField07_CarStatus_Cold.Value = repairOrderData("VHCSTATUS3").ToString() '車両状態3（冷間時）
        ''Logger.Info("SetOrdersInfoData vhcStatus3=" + repairOrderData("VHCSTATUS3").ToString())
        'Me.HiddenField07_CarStatus_Warm.Value = repairOrderData("VHCSTATUS4").ToString() '車両状態4（熱間時）
        ''Logger.Info("SetOrdersInfoData vhcStatus4=" + repairOrderData("VHCSTATUS4").ToString())
        'Me.HiddenField07_Traveling.Value = repairOrderData("VHCSTATUS6").ToString() '車両状態6（穏速・加速・減速）
        ''Logger.Info("SetOrdersInfoData vhcStatus6=" + repairOrderData("VHCSTATUS6").ToString())
        'Me.HiddenField07_CarStatus_Parking.Value = repairOrderData("VHCSTATUS9").ToString() '車両状態9（駐車時）
        ''Logger.Info("SetOrdersInfoData vhcStatus9=" + repairOrderData("VHCSTATUS9").ToString())
        'Me.HiddenField07_CarStatus_Advance.Value = repairOrderData("VHCSTATUS10").ToString() '車両状態10（進行時）
        ''Logger.Info("SetOrdersInfoData vhcStatus10=" + repairOrderData("VHCSTATUS10").ToString())
        'Me.HiddenField07_CarStatus_ShiftChange.Value = repairOrderData("VHCSTATUS11").ToString() '車両状態11（変速時）
        ''Logger.Info("SetOrdersInfoData vhcStatus11=" + repairOrderData("VHCSTATUS11").ToString())
        'Me.HiddenField07_CarStatus_Back.Value = repairOrderData("VHCSTATUS12").ToString() '車両状態12（後退時）
        ''Logger.Info("SetOrdersInfoData vhcStatus12=" + repairOrderData("VHCSTATUS12").ToString())
        'Me.HiddenField07_CarStatus_Brake.Value = repairOrderData("VHCSTATUS13").ToString() '車両状態13（ブレーキ時）
        ''Logger.Info("SetOrdersInfoData vhcStatus13=" + repairOrderData("VHCSTATUS13").ToString())
        'Me.HiddenField07_CarStatus_Detour.Value = repairOrderData("VHCSTATUS14").ToString() '車両状態14（曲がる時）
        ''Logger.Info("SetOrdersInfoData vhcStatus14=" + repairOrderData("VHCSTATUS14").ToString())
        'Me.HiddenField07_NonGenuine.Value = repairOrderData("ACCESSORYFLAG").ToString() '非純正部品使用フラグ
        ''Logger.Info("SetOrdersInfoData accessoryFlag=" + repairOrderData("ACCESSORYFLAG").ToString())
        ''2012/03/23 森下 仕様変更書_20120309_TC問診表画面にボタン追加 START
        'Me.HiddenField07_CarStatus_SteeringWheel.Value = repairOrderData("VHCSTATUS15").ToString() '車両状態15（旋回時）
        ''2012/03/23 森下 仕様変更書_20120309_TC問診表画面にボタン追加 END
        'Dim carSpeed As String = ExchangeDataToHtmlString(repairOrderData("CARSPEED").ToString()) '車両速度
        'Me.TextBoxHearingSpeedRate.Text = carSpeed
        ''Logger.Info("SetOrdersInfoData carSpeed=" + carSpeed)
        'Dim carGear As String = ExchangeDataToHtmlString(repairOrderData("CARBADDISH").ToString()) '車両ギア
        'Me.TextBoxHearingSpeedGear.Text = carGear
        ''Logger.Info("SetOrdersInfoData carBadDish=" + carGear)
        'Dim passengerCount As String = ExchangeDataToHtmlString(repairOrderData("PASSENGER").ToString()) '乗車人数
        'Me.TextBoxHearingPeopleNumber.Text = passengerCount
        ''Logger.Info("SetOrdersInfoData passenger=" + passengerCount)
        'Dim passengerLoad As String = ExchangeDataToHtmlString(repairOrderData("LOAD").ToString()) '車両負荷
        'Me.TextBoxHearingPeopleTooHeavy.Text = passengerLoad
        ''Logger.Info("SetOrdersInfoData load=" + passengerLoad)

        'Logger.Info("SetOrdersInfoData End")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} START" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim displayUrl As String = Nothing

        displayUrl = Me.businessLogic.GetDisplay(displayNumber5)

        Dim systemEnv As New SystemEnvSetting

        ' 画面連携ドメイン取得
        Dim linkageDomain As String = _
            systemEnv.GetSystemEnvSetting(OTHER_LINKAGE_DOMAIN).PARAMVALUE

        'スペースが入ってる場合、空文字にする
        basRezId = DeleteSpaceString(basRezId)
        repairOrderNo = DeleteSpaceString(repairOrderNo)
        workSeq = DeleteSpaceString(workSeq)

        'iframeに連携する画面のURLを設定
        Me.HiddenFieldCutReqIframeUrl.Value = String.Format(CultureInfo.CurrentCulture _
                                                            , displayUrl _
                                                            , linkageDomain _
                                                            , dealerCode _
                                                            , BraunchCode _
                                                            , account _
                                                            , saChipId _
                                                            , basRezId _
                                                            , repairOrderNo _
                                                            , workSeq _
                                                            , vin _
                                                            , ViewMode)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END URL:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , Me.HiddenFieldCutReqIframeUrl.Value))

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ''' <summary>
    ''' 文字列データに対して、空の場合もHTMLに表示するためスペース文字を返す.
    ''' </summary>
    ''' <param name="aStrData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeDataToHtmlString(ByVal aStrData As String) As String

        'Logger.Info("ExchangeDataToHtmlString Start param1:" + aStrData)

        Dim strHtmlString As String = aStrData
        If String.IsNullOrEmpty(strHtmlString) Then
            strHtmlString = STRING_SPACE
        End If

        'Logger.Info("ExchangeDataToHtmlString End return:" + strHtmlString)
        Return strHtmlString

    End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 文字列データに対して、スペース文字の場合空文字を返す.
    ''' </summary>
    ''' <param name="aStrData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteSpaceString(ByVal aStrData As String) As String

        'スペースが入っている場合、空文字を格納する
        If aStrData.Equals(Space(1)) Then
            aStrData = STRING_SPACE
        End If

        Return aStrData

    End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 作業グループ登録ボタンがClickされたときの処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub HiddenButtonRegisterWorkgroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRegisterWorkgroup.Click

    '    Logger.Info("HiddenButtonRegisterWorkgroup_Click Start")

    '    Using dt As New IC3801801DataSet.IC3801801WorkGroupInfoDataTable
    '        Dim dr As IC3801801DataSet.IC3801801WorkGroupInfoRow
    '        Dim cnt As Long = 0

    '        'データを設定する.
    '        For i = 0 To RepeaterWorkInfo.Items.Count - 1
    '            Dim rWorkInfo As Control = RepeaterWorkInfo.Items(i)

    '            Dim WorkByCodeNew As String = CType(rWorkInfo.FindControl("HiddenFieldWorkByCode"), HiddenField).Value.Trim()
    '            Dim WorkByCodeOriginal As String = CType(rWorkInfo.FindControl("HiddenFieldWorkByCodeOriginal"), HiddenField).Value.Trim()

    '            If (WorkByCodeNew <> WorkByCodeOriginal) Then

    '                dr = DirectCast(dt.NewRow(), IC3801801DataSet.IC3801801WorkGroupInfoRow)

    '                dr.inDealerCode = Me.accountStaffContext.DlrCD
    '                dr.inOrderNo = Me.repairOrderNo
    '                dr.inSrvCode = CType(rWorkInfo.FindControl("HiddenFieldSrvCode"), HiddenField).Value.Trim()
    '                dr.inSrvSequece = CType(rWorkInfo.FindControl("HiddenFieldSrvSeq"), HiddenField).Value.Trim()
    '                dr.inNewWorkByCode = WorkByCodeNew

    '                dt.Rows.Add(dr)     'dt.AddIC3801801DataSetRow(dr)

    '                CType(rWorkInfo.FindControl("HiddenFieldWorkByCodeOriginal"), HiddenField).Value = CType(rWorkInfo.FindControl("HiddenFieldWorkByCode"), HiddenField).Value.Trim()
    '                cnt += 1
    '            End If
    '        Next

    '        Dim rtnVal As Long = 1
    '        If (cnt > 0) Then
    '            'rtnVal = Me.businessLogic.UpdateWorkGroup(dt)
    '        End If

    '        If (rtnVal = 0) Then
    '            Logger.Info("HiddenButtonRegisterWorkgroup_Click End (OK). cnt=" & cnt.ToString(CultureInfo.CurrentCulture) & ", rtnVal=" & rtnVal.ToString(CultureInfo.CurrentCulture))
    '        Else
    '            Logger.Error("HiddenButtonRegisterWorkgroup_Click End (NG). cnt=" & cnt.ToString(CultureInfo.CurrentCulture) & ", rtnVal=" & rtnVal.ToString(CultureInfo.CurrentCulture))
    '        End If
    '    End Using
    'End Sub

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断Job存在判定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub HasStopJob()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} START" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値格納用変数
        Dim resultValue As Boolean = False
        '作業内容ID
        Dim intJobDetailID As Decimal = 0


        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''作業内容IDを数値に変換して格納
        'Decimal.TryParse(Me.rezId, intJobDetailID)

        '作業内容ID値チェック
        If Not (String.IsNullOrEmpty(Trim(Me.rezId))) Then
            '存在する場合
            'Hidden値を設定
            intJobDetailID = Decimal.Parse(Me.rezId, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '中断Job存在判定
        resultValue = businessLogic.HasStopJob(intJobDetailID)

        '隠しフィールドに値の格納
        If resultValue Then
            Me.HiddenHasStopJobValue.Value = "1"
        Else
            Me.HiddenHasStopJobValue.Value = "0"
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} END" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

#Region "イベント"

    ''' <summary>
    ''' 開始ボタンクリック処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonJobStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonJobStart.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try
            '押したフッタボタンの状態を、「作業開始」に設定する.
            HiddenChildPushedFooter.Value = workStartFlg

            '取得した予約IDがNull値でない場合のみ
            If (Me.stallUseId > 0) Then

                '選択中のチップ情報を取得する.
                Dim selectedChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoRow
                selectedChipInfo = GetSelectedChipInfo()

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                'If (resultInterference = INTERFERENCE_FAILE) Then

                '    Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

                '    'Job開始処理関数を呼び出す.
                '    StartJobProcess()

                'End If

                Using biz As New TabletSMBCommonClassBusinessLogic
                    '休憩を自動判定しない場合
                    If Not biz.IsRestAutoJudge() Then
                        Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                        If (resultInterference = INTERFERENCE_FAILE) Then
                            '休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                            Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                        ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

                            '干渉なしの場合Job開始処理関数を呼び出す.
                            StartJobProcess()

                        End If

                    Else
                        'Job開始処理関数を呼び出す.
                        StartJobProcess()
                    End If
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            End If
        Finally
            'Me.HiddenReloadFlag.Value = String.Empty
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} END" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' Job開始処理
    ''' </summary>
    ''' <param name="inputRestFlg">休憩取得フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub StartJobProcess(Optional ByVal inputRestFlg As String = "1")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '引数変数宣言
        Dim selectedRezId As String
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        'Dim restFlg As String = inputRestFlg
        Dim restFlg As String
        Using biz As New TabletSMBCommonClassBusinessLogic
            '休憩を自動判定する場合
            If biz.IsRestAutoJudge() Then
                restFlg = TakeBreakFlg
            Else
                restFlg = inputRestFlg
            End If
        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        Dim jobInstructId As String = Nothing
        Dim jobInstructSeq As Long = Nothing
        Dim selectedUpdateCount As Long
        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(accountStaffContext.DlrCD)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''ストール利用ID取得
        'selectedRezId = Me.stallUseId.ToString()

        'ストール利用ID取得
        selectedRezId = Me.stallUseId.ToString(CultureInfo.CurrentCulture)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '作業指示ID
        jobInstructId = Me.HiddenSelectedJobInstructId.Value

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''作業指示枝番
        'Long.TryParse(Me.HiddenSelectedJobInstructSeq.Value, jobInstructSeq)

        '作業指示枝番値チェック
        If Not (String.IsNullOrEmpty(Trim(Me.HiddenSelectedJobInstructSeq.Value))) Then
            '存在する場合
            '値を設定
            jobInstructSeq = Long.Parse(Me.HiddenSelectedJobInstructSeq.Value, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '行ロックバージョン
        selectedUpdateCount = Me.updateCount

        '開始処理実施
        Dim resultCode As Long
        resultCode = businessLogic.JobStart(Me.stallUseId, _
                                             nowDateTime, _
                                             restFlg, _
                                             jobInstructId, _
                                             jobInstructSeq, _
                                             nowDateTime, _
                                             selectedUpdateCount, _
                                             APPLICATION_ID)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ' ''正常に終了していない場合、エラーメッセージを表示する.
        'If (resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL) Then


        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultCode, workStartFlg)

        '    Exit Sub
        'End If

        '開始処理結果チェック
        If resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultCode <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            showErrMsgAndRefresh(resultCode, workStartFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'PUSH送信を行うか判定する
        If businessLogic.HasSendPush(workStartFlg) Then

            '選択中のチップの作業内容ID取得
            Dim jobDatilId As Decimal = 0
            If Not String.IsNullOrEmpty(Me.rezId) Then
                jobDatilId = CType(Me.rezId, Decimal)
            End If

            'Push送信
            businessLogic.WorkStartSendPush(accountStaffContext.DlrCD, _
                                            accountStaffContext.BrnCD, _
                                            accountStaffContext.Account, _
                                            Me.stallId, _
                                            jobDatilId)
        End If

        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
        ''画面リフレッシュ
        'ScriptManager.RegisterStartupScript(Me, _
        '                                    Me.GetType, _
        '                                    "ShowMessageAndRefresh", _
        '                                    "parentScreenReLoad();", _
        '                                    True)

        '画面更新フラグを設定
        Me.HiddenRefreshFlg.Value = RefreshFlg
        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '開始処理結果チェック
        If resultCode = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            'DMS除外エラーメッセージ表示
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 終了ボタンクリック処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonJobFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonJobFinish.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Try
            '押したフッタボタンの状態を、「作業開始」に設定する.
            HiddenChildPushedFooter.Value = workFinishFlg


            '取得した予約IDがNull値でない場合のみ
            If (Me.stallUseId > 0) Then

                '選択中のチップ情報を取得する.
                Dim selectedChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoRow
                selectedChipInfo = GetSelectedChipInfo()

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                'If (resultInterference = INTERFERENCE_FAILE) Then

                '    Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

                '    'Job終了処理関数を呼び出す.
                '    FinishJobProcess()

                'End If

                Using biz As New TabletSMBCommonClassBusinessLogic
                    '休憩を自動判定しない場合
                    If Not biz.IsRestAutoJudge() Then
                        Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                        If (resultInterference = INTERFERENCE_FAILE) Then
                            '休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                            Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                        ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

                            '干渉なしの場合Job終了処理関数を呼び出す.
                            FinishJobProcess()

                        End If

                    Else
                        'Job終了処理関数を呼び出す.
                        FinishJobProcess()
                    End If
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            End If
        Finally
            'Me.HiddenReloadFlag.Value = String.Empty
        End Try


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} END" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' Job終了処理
    ''' </summary>
    ''' <param name="inputRestFlg">休憩取得フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub FinishJobProcess(Optional ByVal inputRestFlg As String = "1")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '引数変数宣言
        Dim selectedRezId As String
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        'Dim restFlg As String = inputRestFlg
        Dim restFlg As String
        Using biz As New TabletSMBCommonClassBusinessLogic
            '休憩を自動判定する場合
            If biz.IsRestAutoJudge() Then
                restFlg = TakeBreakFlg
            Else
                restFlg = inputRestFlg
            End If
        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        Dim jobInstructId As String = Nothing
        Dim jobInstructSeq As Long = Nothing
        Dim selectedUpdateCount As Long
        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(accountStaffContext.DlrCD)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''ストール利用ID取得
        'selectedRezId = Me.stallUseId.ToString()

        'ストール利用ID取得
        selectedRezId = Me.stallUseId.ToString(CultureInfo.InvariantCulture)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '作業指示ID
        jobInstructId = Me.HiddenSelectedJobInstructId.Value

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Long.TryParse(Me.HiddenSelectedJobInstructSeq.Value, jobInstructSeq)

        '作業指示枝番のHidden値チェック
        If Not (String.IsNullOrEmpty(Trim(Me.HiddenSelectedJobInstructSeq.Value))) Then
            '存在する場合
            'Hidden値を設定
            jobInstructSeq = Long.Parse(Me.HiddenSelectedJobInstructSeq.Value, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '行ロックバージョン
        selectedUpdateCount = Me.updateCount

        '終了処理実施
        Dim resultCode As Long
        resultCode = businessLogic.JobFinish(Me.stallUseId, _
                                             nowDateTime, _
                                             restFlg, _
                                             jobInstructId, _
                                             jobInstructSeq, _
                                             nowDateTime, _
                                             selectedUpdateCount, _
                                             APPLICATION_ID)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ' ''正常に終了していない場合、エラーメッセージを表示する.
        'If (resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL) Then

        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultCode, workFinishFlg)

        '    Exit Sub
        'End If

        '終了処理結果チェック
        If resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultCode <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            showErrMsgAndRefresh(resultCode, workStartFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'PUSH送信を行うか判定する
        If businessLogic.HasSendPush(workFinishFlg) Then

            '選択中のチップの作業内容ID取得
            Dim jobDatilId As Decimal = 0
            If Not String.IsNullOrEmpty(Me.rezId) Then
                jobDatilId = CType(Me.rezId, Decimal)
            End If

            'SAへの通知処理
            businessLogic.NoticeMainProcessing(jobDatilId, accountStaffContext.DlrCD, accountStaffContext.BrnCD, accountStaffContext)

            'Push送信
            businessLogic.WorkEndSendPush(accountStaffContext.DlrCD, _
                                          accountStaffContext.BrnCD, _
                                          accountStaffContext.Account, _
                                          Me.repairOrderNo,
                                          jobDatilId, _
                                          Me.stallId)
        End If

        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
        '画面リフレッシュ
        'ScriptManager.RegisterStartupScript(Me, _
        '                                    Me.GetType, _
        '                                    "ShowMessageAndRefresh", _
        '                                    "parentScreenReLoad();", _
        '                                    True)

        '画面更新フラグを設定
        Me.HiddenRefreshFlg.Value = RefreshFlg
        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

        '開始処理を実施すると、シーケンス番号が更新されるため、チップのIDが変更される.
        'チップの選択状態を保持するため、選択中チップのIDを変更する.
        'Me.HiddenSelectedId.Value = Me.HiddenCandidateId.Value

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '終了処理結果チェック
        If resultCode = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            'DMS除外エラーメッセージ表示
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Jobt中断ボタンクリック処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonJobStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonJobStop.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try

            '押したフッタボタンの状態を、「作業開始」に設定する.
            HiddenChildPushedFooter.Value = workStopFlg

            '取得したストール利用IDがNull値でない場合のみ
            If (Me.stallUseId > 0) Then

                '選択中のチップ情報を取得する.
                Dim selectedChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoRow
                selectedChipInfo = GetSelectedChipInfo()

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                'If (resultInterference = INTERFERENCE_FAILE) Then

                '    Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

                '    'Job終了処理関数を呼び出す.
                '    StopJobProcess()

                'End If

                Using biz As New TabletSMBCommonClassBusinessLogic

                    '休憩を自動判定しない場合
                    If Not biz.IsRestAutoJudge() Then
                        Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable(selectedChipInfo)

                        If (resultInterference = INTERFERENCE_FAILE) Then
                            '休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                            Me.HiddenBreakPopupChild.Value = POPUP_BREAK_DISPLAY

                        ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then
                            '干渉なしの場合Job中断処理関数を呼び出す.
                            StopJobProcess()

                        End If
                    Else
                        'Job中断処理関数を呼び出す.
                        StopJobProcess()

                    End If

                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            End If

        Finally
            'Me.HiddenReloadFlag.Value = String.Empty
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' Job中断処理
    ''' </summary>
    ''' <param name="inputRestFlg">休憩取得フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub StopJobProcess(Optional ByVal inputRestFlg As String = "1")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} START" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '引数変数宣言
        Dim selectedRezId As String
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        'Dim restFlg As String = inputRestFlg
        Dim restFlg As String
        Using biz As New TabletSMBCommonClassBusinessLogic
            '休憩を自動判定する場合
            If biz.IsRestAutoJudge() Then
                restFlg = TakeBreakFlg
            Else
                restFlg = inputRestFlg
            End If
        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        Dim jobInstructId As String = Nothing
        Dim jobInstructSeq As Long = Nothing
        Dim selectedUpdateCount As Long
        Dim stopReasonType As String = Me.HiddenChildStopReasonType.Value
        Dim strStopTime As String = Me.HiddenChildStopTime.Value
        Dim stopMemo As String = Space(1)
        Dim minuteString As String = WebWordUtility.GetWord("SC3150101", 39)

        Dim longStopTime As Long = 0

        '現在の時刻取得
        Dim nowDateTime As Date = DateTimeFunc.Now(accountStaffContext.DlrCD)


        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''ストール利用ID取得
        'selectedRezId = Me.stallUseId.ToString()

        'ストール利用ID取得
        selectedRezId = Me.stallUseId.ToString(CultureInfo.CurrentCulture)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '作業指示ID
        jobInstructId = Me.HiddenSelectedJobInstructId.Value

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''作業指示枝番
        'Long.TryParse(Me.HiddenSelectedJobInstructSeq.Value, jobInstructSeq)

        '作業指示枝番のHidden値チェック
        If Not (String.IsNullOrEmpty(Trim(Me.HiddenSelectedJobInstructSeq.Value))) Then
            '存在する場合
            'Hidden値を設定
            jobInstructSeq = Long.Parse(Me.HiddenSelectedJobInstructSeq.Value, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '行ロックバージョン
        selectedUpdateCount = Me.updateCount

        '中断時間の単位を削除し、数値型に変換
        strStopTime = strStopTime.Replace(minuteString, "")

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Long.TryParse(strStopTime, longStopTime)

        '中断時間値チェック
        If Not (String.IsNullOrEmpty(Trim(strStopTime))) Then
            '存在する場合
            '中断時間を設定
            longStopTime = Long.Parse(strStopTime, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '中断メモが入力されている場合
        If String.IsNullOrEmpty(Me.HiddenChildStopMemo.Value) Then
            stopMemo = Me.HiddenChildStopMemo.Value
        End If

        '中断処理実施
        Dim resultCode As Long
        resultCode = businessLogic.JobStop(Me.stallUseId, _
                                             nowDateTime, _
                                             longStopTime, _
                                             stopMemo, _
                                             stopReasonType, _
                                             restFlg, _
                                             jobInstructId, _
                                             jobInstructSeq, _
                                             nowDateTime, _
                                             selectedUpdateCount, _
                                             APPLICATION_ID)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ' ''正常に終了していない場合、エラーメッセージを表示する.
        'If (resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL) Then

        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultCode, workStopFlg)

        '    Exit Sub
        'End If

        '中断処理結果チェック
        If resultCode <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultCode <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            showErrMsgAndRefresh(resultCode, workStartFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'PUSH送信を行うか判定する
        If businessLogic.HasSendPush(workStopFlg) Then


            '選択中のチップの作業内容ID取得
            Dim jobDatilId As Decimal = 0
            If Not String.IsNullOrEmpty(Me.rezId) Then
                jobDatilId = CType(Me.rezId, Decimal)
            End If

            'Push送信
            businessLogic.WorkEndSendPush(accountStaffContext.DlrCD, _
                                          accountStaffContext.BrnCD, _
                                          accountStaffContext.Account, _
                                          Me.repairOrderNo,
                                          Me.stallId, _
                                          jobDatilId)
        End If

        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
        '画面リフレッシュ
        'ScriptManager.RegisterStartupScript(Me, _
        '                                    Me.GetType, _
        '                                    "ShowMessageAndRefresh", _
        '                                    "parentScreenReLoad();", _
        '                                    True)

        '画面更新フラグを設定
        Me.HiddenRefreshFlg.Value = RefreshFlg
        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '中断処理結果チェック
        If resultCode = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            'DMS除外エラーメッセージ表示
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} END" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 全ての履歴を表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AllDispLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllDispLink.Click
        '全ての履歴情報取得処理
        Me.SetAllHistoryInfo(1)
    End Sub

    ''' <summary>
    ''' 次をＮ件を表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub NextDispLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextDispLink.Click
        '全ての履歴情報取得処理
        Me.SetAllHistoryInfo(CType(Me.HiddenFieldOtherHistoryDispPageCount.Value, Integer) + 1)
    End Sub

    ''' <summary>
    ''' 「休憩をとらない」選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonChildDoNotBreakk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonChildDoNotBreak.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        SelectedTakeBreak(False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 「休憩をとる」選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonChildTakeBreak_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonChildTakeBreak.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        SelectedTakeBreak(True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 「休憩をとる」「休憩をとらない」の選択時処理
    ''' </summary>
    ''' <param name="selectedBreak"></param>
    ''' <remarks></remarks>
    Private Sub SelectedTakeBreak(ByVal selectedBreak As Boolean)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '押したフッタボタンの状態を取得する.
        Dim pushedFooterStatus = Me.HiddenChildPushedFooter.Value
        Dim restFlg As String = TakeBreakFlg

        '表示されている、休憩による作業伸長ポップアップの非表示フラグをセットする
        Me.HiddenBreakPopupChild.Value = POPUP_BREAK_NONE

        'フッタボタンの状態を初期化する.
        Me.HiddenChildPushedFooter.Value = PUSHED_FOOTER_BUTTON_INIT

        If Not selectedBreak Then

            restFlg = DoNotBreakFlg

        End If

        'フッタボタンの状態に応じて、処理を分岐する.
        If (workStartFlg.Equals(pushedFooterStatus)) Then

            StartJobProcess(restFlg)

        ElseIf (workFinishFlg.Equals(pushedFooterStatus)) Then

            FinishJobProcess(restFlg)

        ElseIf (workStopFlg.Equals(pushedFooterStatus)) Then

            StopJobProcess(restFlg)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    '2012/06/06 KN 彭　コード分析対応 END

#Region "メソッド"

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    'Protected Sub ButtonSendNoticeToFM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSendNoticeToFM.Click
    '    Logger.Info("ButtonSendNoticeToFM_Click Start")

    '    NotificationAPI()

    '    Logger.Info("ButtonSendNoticeToFM_Click End")
    'End Sub
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END


    '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START

    ''' <summary>
    ''' 全ての履歴情報取得処理
    ''' </summary>
    ''' <param name="dispPageCount">表示ページ数</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' 2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
    ''' </history>
    Private Sub SetAllHistoryInfo(ByVal dispPageCount As Integer)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        '車両登録Noををキーに全ての入庫履歴を取得する
        'Using dtOrderHistory As IC3801601DataSet.ORDERHISTORYDataTable = _
        '    Me.businessLogic.GetAllHistoryInfo(Trim(Me.LblOrderRegisterNo.Text), Trim(Me.LblOrderVinNo.Text))
        'ユーザ情報の取得
        Me.accountStaffContext = StaffContext.Current

        '文言「定期検査」設定
        Dim periodicInspection As String = WebWordUtility.GetWord(130)
        '文言「一般検査」設定
        Dim generalInspection As String = WebWordUtility.GetWord(131)

        Using dtOrderHistory As SC3150102DataSet.SC3150102GetServiceInHistoryDataTable = _
        Me.businessLogic.GetAllHistoryInfo(accountStaffContext.DlrCD, accountStaffContext.BrnCD, repairOrderNo, dispPageCount)

            '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

            If IsNothing(dtOrderHistory) OrElse dtOrderHistory.Count > 0 Then
                '表示件数（終了）
                Dim dispEndCount As Integer = 20 * dispPageCount
                '表示件数が取得件数を上回った場合は取得件数に置き換える
                If dispEndCount >= dtOrderHistory.Count Then
                    dispEndCount = dtOrderHistory.Count
                    '「次のＮ件を表示」を非表示にする
                    CType(Me.NextDispLinkDiv, HtmlContainerControl).Style("display") = "none"
                Else
                    '「次のＮ件を表示」を表示にする
                    CType(Me.NextDispLinkDiv, HtmlContainerControl).Style("display") = "block"
                End If

                '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

                ''表示するDataRowをDataTableに格納する
                'Dim dtOrderHistoryNew As New IC3801601DataSet.ORDERHISTORYDataTable
                'For i = 0 To dispEndCount - 1
                '    Dim drOrderHistoryNew As IC3801601DataSet.ORDERHISTORYRow = dtOrderHistoryNew.NewORDERHISTORYRow
                '    Dim drOrderHistoryOld As IC3801601DataSet.ORDERHISTORYRow = _
                '        DirectCast(dtOrderHistory.Rows(i), IC3801601DataSet.ORDERHISTORYRow)
                '    '入庫日
                '    If Not (drOrderHistoryOld.IsORDERDATENull) Then
                '        drOrderHistoryNew.ORDERDATE = drOrderHistoryOld.ORDERDATE
                '    End If
                '    '整備受注番号
                '    If Not (drOrderHistoryOld.IsORDERNONull) Then
                '        drOrderHistoryNew.ORDERNO = drOrderHistoryOld.ORDERNO
                '    End If
                '    '整備タイプ
                '    If Not (drOrderHistoryOld.IsSRVTYPENAMENull) Then
                '        drOrderHistoryNew.SRVTYPENAME = drOrderHistoryOld.SRVTYPENAME
                '    End If
                '    '代表整備項目
                '    If Not (drOrderHistoryOld.IsSRVNAMENull) Then
                '        drOrderHistoryNew.SRVNAME = drOrderHistoryOld.SRVNAME
                '    End If
                '    'SA名称
                '    If Not (drOrderHistoryOld.IsEMPLOYEENAMENull) Then
                '        drOrderHistoryNew.EMPLOYEENAME = drOrderHistoryOld.EMPLOYEENAME
                '    End If
                '    '販売店CD
                '    If Not (drOrderHistoryOld.IsDEALERCODENull) Then
                '        drOrderHistoryNew.DEALERCODE = drOrderHistoryOld.DEALERCODE
                '    End If
                '    'データ格納
                '    dtOrderHistoryNew.AddORDERHISTORYRow(drOrderHistoryNew)
                'Next

                '表示するDataRowをDataTableに格納する
                Using dtOrderHistoryNew As New SC3150102DataSet.SC3150102GetServiceInHistoryDataTable
                    For i = 0 To dtOrderHistory.Rows.Count
                        Dim drOrderHistoryNew As SC3150102DataSet.SC3150102GetServiceInHistoryRow = dtOrderHistoryNew.NewSC3150102GetServiceInHistoryRow
                        Dim drOrderHistoryOld As SC3150102DataSet.SC3150102GetServiceInHistoryRow = _
                            DirectCast(dtOrderHistory.Rows(i), SC3150102DataSet.SC3150102GetServiceInHistoryRow)
                        '入庫実績日時
                        If Not (drOrderHistoryOld.IsSVCIN_DELI_DATENull) Then
                            drOrderHistoryNew.SVCIN_DELI_DATE = drOrderHistoryOld.SVCIN_DELI_DATE
                        End If

                        'RO番号
                        drOrderHistoryNew.SVCIN_NUM = ExchangeDataToHtmlString(drOrderHistoryOld.SVCIN_NUM)

                        '整備名称
                        drOrderHistoryNew.MAINTE_NAME = ExchangeDataToHtmlString(drOrderHistoryOld.MAINTE_NAME)

                        'サービス名称
                        drOrderHistoryNew.SVC_NAME_MILE = ExchangeDataToHtmlString(drOrderHistoryOld.SVC_NAME_MILE)

                        'SA名称
                        drOrderHistoryNew.STF_NAME = ExchangeDataToHtmlString(drOrderHistoryOld.STF_NAME)

                        '販売店CD
                        drOrderHistoryNew.DLR_CD = ExchangeDataToHtmlString(drOrderHistoryOld.DLR_CD)

                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                        drOrderHistoryNew.MAINTE_NAME_HIS = ExchangeDataToHtmlString(drOrderHistoryOld.MAINTE_NAME_HIS)
                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END

                        'データ格納
                        dtOrderHistoryNew.AddSC3150102GetServiceInHistoryRow(drOrderHistoryNew)

                        '設定表示件数に達した場合抜ける
                        If i = dispEndCount - 1 Then
                            Exit For
                        End If
                    Next
                    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

                    '表示ページ数をHIDDENに格納
                    Me.HiddenFieldOtherHistoryDispPageCount.Value = CType(dispPageCount, String)
                    'コントロールにバインドする
                    Me.RepeaterHistoryInfo.DataSource = dtOrderHistoryNew
                    Me.RepeaterHistoryInfo.DataBind()

                    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

                    ''データを設定する.
                    'For i = 0 To RepeaterHistoryInfo.Items.Count - 1
                    '    Dim rInfo As Control = RepeaterHistoryInfo.Items(i)
                    '    Dim drOrderHistory As IC3801601DataSet.ORDERHISTORYRow = _
                    '        DirectCast(dtOrderHistoryNew.Rows(i), IC3801601DataSet.ORDERHISTORYRow)

                    '    '受注日を表示する.
                    '    Dim strOrderAcceptDate As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsORDERDATENull) Then
                    '        Dim acceptDate As Date = drOrderHistory.ORDERDATE
                    '        If acceptDate > Date.MinValue Then
                    '            If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
                    '                '受注日が当日である場合、HH:mm形式にて表示.
                    '                strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
                    '            Else
                    '                '受注日が当日でない場合、YYYY/MM/dd形式にて表示.
                    '                strOrderAcceptDate = DateTimeFunc.FormatDate(3, acceptDate)
                    '            End If
                    '        Else
                    '            strOrderAcceptDate = STRING_SPACE
                    '        End If
                    '    End If
                    '    CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate
                    '    'オーダーNoを表示する.
                    '    Dim stringOrderNo As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsORDERNONull OrElse String.IsNullOrEmpty(drOrderHistory.ORDERNO)) Then
                    '        stringOrderNo = drOrderHistory.ORDERNO
                    '    End If
                    '    CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
                    '    CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value = stringOrderNo

                    '    '販売店CDを格納する.
                    '    Dim stringDealerCode As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsDEALERCODENull OrElse String.IsNullOrEmpty(drOrderHistory.DEALERCODE)) Then
                    '        stringDealerCode = Trim(drOrderHistory.DEALERCODE)
                    '    End If
                    '    CType(rInfo.FindControl("HiddenFieldHDealerCode"), HiddenField).Value = stringDealerCode

                    '    '代表整備名称を表示する.
                    '    Dim strSrvTypeName As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsSRVTYPENAMENull OrElse String.IsNullOrEmpty(drOrderHistory.SRVTYPENAME)) Then
                    '        strSrvTypeName = drOrderHistory.SRVTYPENAME
                    '    End If
                    '    CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName

                    '    '代表整備項目を表示する.
                    '    Dim strSrvType As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsSRVNAMENull OrElse String.IsNullOrEmpty(drOrderHistory.SRVNAME)) Then
                    '        strSrvType = drOrderHistory.SRVNAME
                    '    End If
                    '    CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType

                    '    '担当SAを表示する
                    '    Dim strSaName As String = STRING_SPACE
                    '    If Not (drOrderHistory.IsEMPLOYEENAMENull OrElse String.IsNullOrEmpty(drOrderHistory.EMPLOYEENAME)) Then
                    '        strSaName = drOrderHistory.EMPLOYEENAME
                    '    End If
                    '    CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
                    'Next

                    'データを設定する.
                    For i = 0 To RepeaterHistoryInfo.Items.Count - 1
                        Dim rInfo As Control = RepeaterHistoryInfo.Items(i)
                        Dim drOrderHistory As SC3150102DataSet.SC3150102GetServiceInHistoryRow = _
                            DirectCast(dtOrderHistoryNew.Rows(i), SC3150102DataSet.SC3150102GetServiceInHistoryRow)

                        '受注日を表示する.
                        Dim strOrderAcceptDate As String = STRING_SPACE
                        If Not (drOrderHistory.IsSVCIN_DELI_DATENull) Then
                            Dim acceptDate As Date = drOrderHistory.SVCIN_DELI_DATE
                            If acceptDate > Date.MinValue Then
                                If (acceptDate.Date = DateTimeFunc.Now(Me.accountStaffContext.DlrCD).Date) Then
                                    '受注日が当日である場合、HH:mm形式にて表示.
                                    strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
                                Else
                                    '受注日が当日でない場合、YYYY/MM/dd形式にて表示.
                                    strOrderAcceptDate = DateTimeFunc.FormatDate(3, acceptDate)
                                End If
                            Else
                                strOrderAcceptDate = STRING_SPACE
                            End If
                        End If
                        CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate
                        'オーダーNoを表示する.
                        Dim stringServiceInNumber As String = STRING_SPACE
                        If Not (drOrderHistory.IsSVCIN_NUMNull) Then
                            stringServiceInNumber = ExchangeDataToHtmlString(drOrderHistory.SVCIN_NUM)
                        End If

                        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

                        ''文字列の分割位置の取得
                        'Dim StringIndex As Integer = stringServiceInNumber.IndexOf(PUNCTUATION_STRING)

                        '文字列の分割位置の取得
                        Dim StringIndex As Integer = stringServiceInNumber.IndexOf(PUNCTUATION_STRING, StringComparison.CurrentCulture)

                        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

                        Dim stringOrderNo As String = stringServiceInNumber

                        If Not StringIndex < 0 Then
                            '入庫管理番号を分割し、RO番号の部分を取得
                            stringOrderNo = stringServiceInNumber.Substring(0, StringIndex)
                        End If

                        CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo
                        CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value = stringOrderNo

                        '販売店CDを格納する.
                        Dim stringDealerCode As String = STRING_SPACE
                        If Not (drOrderHistory.IsDLR_CDNull) Then
                            stringDealerCode = Trim(ExchangeDataToHtmlString(drOrderHistory.DLR_CD))
                        End If
                        CType(rInfo.FindControl("HiddenFieldHDealerCode"), HiddenField).Value = stringDealerCode

                        '代表整備名称を表示する.
                        Dim strSrvTypeName As String = periodicInspection

                        '代表整備項目を表示する.
                        Dim strSrvType As String = STRING_SPACE

                        If Not drOrderHistory.IsMAINTE_NAMENull Then

                            strSrvType = ExchangeDataToHtmlString(drOrderHistory.SVC_NAME_MILE)

                            '整備名称とサービス名称の内容同じ場合、サービス名称がない時なので、一般点検を表示
                            If drOrderHistory.MAINTE_NAME.Equals(drOrderHistory.SVC_NAME_MILE) Then
                                strSrvTypeName = generalInspection
                                strSrvType = ExchangeDataToHtmlString(drOrderHistory.MAINTE_NAME)
                            End If
                        End If

                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                        If Not (drOrderHistory.IsMAINTE_NAME_HISNull) AndAlso _
                           Not (String.IsNullOrEmpty(Trim(drOrderHistory.MAINTE_NAME_HIS))) Then
                            '整備履歴.整備名称がある場合は、左記を代表整備項目へ設定
                            strSrvType = ExchangeDataToHtmlString(drOrderHistory.MAINTE_NAME_HIS)
                        End If
                        '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END

                        CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName

                        CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType

                        '担当SAを表示する
                        Dim strSaName As String = STRING_SPACE
                        If Not (drOrderHistory.IsSTF_NAMENull) Then
                            strSaName = ExchangeDataToHtmlString(drOrderHistory.STF_NAME)
                        End If
                        CType(rInfo.FindControl("LiteralHSaName"), Literal).Text = strSaName
                    Next

                    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

                End Using
            Else
                'コントロールに空をバインドする
                Me.RepeaterHistoryInfo.DataSource = Nothing
                Me.RepeaterHistoryInfo.DataBind()
            End If

        End Using

        'リストが変更されたので再度、レコードのタップイベントをバインドする
        Dim updateBind As New StringBuilder
        'フリックを再バインド
        updateBind.Append("$(""#S-TC-05RightScroll .S-TC-05Right1-1"").bind(""touch click"", function () { clickHistory(this); });")
        '「全ての履歴を表示」を非表示にする
        updateBind.Append("$('div.S-TC-05Right2-1').hide();")
        '操作不可用のDIVを元に戻す
        updateBind.Append("$('div.DisabledDiv').hide();")
        'イベント実行
        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "UpdateBind", _
                                            updateBind.ToString(), _
                                            True)
        updateBind = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' 選択中のチップ情報を取得する.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedChipInfo() As SC3150102DataSet.SC3150102ChipDateTimeInfoRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START,HiddenSelectedId={2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , Me.stallUseId))

        '返却する選択されているチップ情報を初期化する.
        Dim selectedChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoRow
        selectedChipInfo = Nothing

        '選択中チップの情報を取得する.
        Dim dtChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoDataTable
        dtChipInfo = businessLogic.GetChipInfo(Me.stallUseId)

        'データセットをデータロウに変換
        If (Not IsNothing(dtChipInfo)) AndAlso _
            (Not dtChipInfo.Rows.Count = 0) Then

            selectedChipInfo = DirectCast(dtChipInfo.Rows(0), SC3150102DataSet.SC3150102ChipDateTimeInfoRow)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return selectedChipInfo

    End Function

    ''' <summary>
    ''' バリデーション（休憩・使用不可チップとの干渉）
    ''' </summary>
    ''' <returns>バリデーション結果</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceBreakUnavailable(ByVal selectedChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoRow) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))




        Dim isInterfere As Integer = INTERFERENCE_SUCCESSFULL
        Dim dtmNowTime As Date
        Dim dtmEstimateEndTime As Date

        '作業対象チップが存在する場合、バリデーションを実施する.
        If (Not IsNothing(selectedChipInfo)) Then

            '該当するチップの開始時間（予定）と終了時間（予定）を取得する.
            If (selectedChipInfo.IsSCHE_START_DATETIMENull()) Or (selectedChipInfo.IsSCHE_END_DATETIMENull()) Then
                isInterfere = INTERFERENCE_FAILE
            Else

                'チップが作業中か、一部作業中断中の場合
                If (Not selectedChipInfo.IsSTALL_USE_STATUSNull) AndAlso _
                    ((selectedChipInfo.STALL_USE_STATUS.Equals(stallUseStatus_Working)) Or _
                     (selectedChipInfo.STALL_USE_STATUS.Equals(stallUseStatus_StopPart))) Then

                    '実績開始日時と終了見込日時を格納する
                    dtmNowTime = selectedChipInfo.RSLT_START_DATETIME
                    dtmEstimateEndTime = selectedChipInfo.PRMS_END_DATETIME

                Else
                    '予定開始日時と予定終了日時を格納する
                    Dim dtmStart As Date = selectedChipInfo.SCHE_START_DATETIME
                    Dim dtmEnd As Date = selectedChipInfo.SCHE_END_DATETIME
                    Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)

                    '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
                    dtmNowTime = DateTimeFunc.Now(accountStaffContext.DlrCD)
                    dtmEstimateEndTime = dtmNowTime.Add(ts)
                End If


                '休憩チップとの干渉チェックし、干渉が発生する場合、干渉発生を返す.
                If (ValidationInterferenceBreakChip(dtmNowTime, dtmEstimateEndTime)) Then

                    isInterfere = INTERFERENCE_FAILE

                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    'Else

                    '    '使用不可チップとの干渉をチェックし、干渉が発生する場合、干渉発生を返す.
                    '    If (ValidationInterferenceUnavailableChip(dtmNowTime, dtmEstimateEndTime)) Then
                    '        isInterfere = INTERFERENCE_FAILE

                    '    End If
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                End If
            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return isInterfere

    End Function

    ''' <summary>
    ''' 休憩チップと作業対象チップの干渉チェック
    ''' </summary>
    ''' <param name="aNowTime">作業対象チップの開始時間</param>
    ''' <param name="aEstimateEndTime">作業対象チップの終了時間</param>
    ''' <returns>干渉する：true,干渉しない：false</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceBreakChip(ByVal aNowTime As Date, ByVal aEstimateEndTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '返り値を初期化する.
        Dim resultCheck As Boolean = False

        '時間ソートされた休憩チップ情報を取得する.
        Dim breakDataTable As SC3150102DataSet.SC3150102BreakChipInfoDataTable
        breakDataTable = businessLogic.GetBreakData(accountStaffContext.DlrCD, accountStaffContext.BrnCD, Me.stallId)

        '取得した休憩チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachBreakData As DataRow In breakDataTable.Rows

            Dim eachStartTime As Date = CType(eachBreakData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachBreakData("ENDTIME"), Date)

            If ((aNowTime < eachEndTime) And (eachStartTime < aEstimateEndTime)) Then
                resultCheck = True
                Exit For
            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return resultCheck

    End Function

    ''' <summary>
    ''' 使用不可チップと作業対象チップとの干渉チェック
    ''' </summary>
    ''' <param name="aTargetStartTime">作業対象チップの開始時間</param>
    ''' <param name="aTargetEndTime">作業対象チップの終了時間</param>
    ''' <returns>干渉する：true,干渉しない：false</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceUnavailableChip(ByVal aTargetStartTime As Date, ByVal aTargetEndTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '返り値となる値を初期化する.
        Dim resultCheck As Boolean = False

        '時間ソートされた使用不可チップ情報を取得する.
        Dim unavailableDataTable As SC3150102DataSet.SC3150102UnavailableChipInfoDataTable
        unavailableDataTable = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        '取得した使用不可チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachUnavailableData As DataRow In unavailableDataTable.Rows

            Dim eachStartTime As Date = CType(eachUnavailableData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachUnavailableData("ENDTIME"), Date)


            If ((aTargetStartTime < eachEndTime) And (eachStartTime < aTargetEndTime)) Then

                resultCheck = True
                Exit For
            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return resultCheck

    End Function

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
#End Region

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    '#Region "通知系"
    '    ''' <summary>
    '    ''' 通知用メッセージ作成メソッド
    '    ''' </summary>
    '    ''' <returns>作成したメッセージ文言</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateDisplayContents() As String
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

    '        Dim workMessage As New StringBuilder

    '        ' メッセージ組立 車両登録No
    '        If Not String.IsNullOrEmpty(Me.vclRegNo) Then
    '            workMessage.Append(Me.vclRegNo)
    '            workMessage.Append(" ")
    '        End If

    '        ' メッセージ組立 R/O番号
    '        If Not String.IsNullOrEmpty(Me.repairOrderNo) Then
    '            workMessage.Append(WebWordUtility.GetWord(333))
    '            workMessage.Append(Me.repairOrderNo)
    '            workMessage.Append(" ")
    '        End If

    '        ' メッセージ組立 ストール名
    '        If Not String.IsNullOrEmpty(Me.stallName) Then
    '            workMessage.Append(Me.stallName)
    '            workMessage.Append(" ")
    '        End If

    '        workMessage.Append(WebWordUtility.GetWord(331))

    '        Logger.Info("displayContents=" & workMessage.ToString)
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    '        Return workMessage.ToString
    '    End Function

    '    ''' <summary>
    '    ''' 通知用メッセージ作成メソッド
    '    ''' </summary>
    '    ''' <returns>作成したメッセージ文言</returns>
    '    ''' <remarks></remarks>
    '    Private Function CreateNotifyMessage() As String
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

    '        Dim workMessage As New StringBuilder

    '        ' メッセージ組立 車両登録No
    '        If Not String.IsNullOrEmpty(Me.vclRegNo) Then
    '            workMessage.Append(Me.vclRegNo)
    '            workMessage.Append(" ")
    '        End If

    '        ' メッセージ組立 R/O番号
    '        If Not String.IsNullOrEmpty(Me.repairOrderNo) Then
    '            workMessage.Append(WebWordUtility.GetWord(333))
    '            workMessage.Append(Me.repairOrderNo)
    '            workMessage.Append(" ")
    '        End If

    '        ' メッセージ組立 ストール名
    '        If Not String.IsNullOrEmpty(Me.stallName) Then
    '            workMessage.Append(Me.stallName)
    '            workMessage.Append(" ")
    '        End If

    '        workMessage.Append("<a")
    '        workMessage.Append(" id='SC31702030'")
    '        workMessage.Append(" Class='SC3170203'")
    '        workMessage.Append(" href='/Website/Pages/SC3170203.aspx'")
    '        workMessage.Append(" onclick='return ServiceLinkClick(event)'")
    '        workMessage.Append(">")
    '        workMessage.Append(WebWordUtility.GetWord(331))
    '        workMessage.Append("</a>")

    '        Logger.Info("notifyMessage=" & workMessage.ToString)
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    '        Return workMessage.ToString
    '    End Function

    '    ''' <summary>
    '    ''' 通知用セッション情報作成メソッド
    '    ''' </summary>
    '    ''' <returns>戻り値</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' </history>
    '    Private Function CreateNotifySession() As String
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

    '        Dim workSession As New StringBuilder

    '        '整備受注NO
    '        workSession.Append("Redirect.ORDERNO,String")
    '        workSession.Append(",")
    '        workSession.Append(Me.repairOrderNo)

    '        '枝番
    '        workSession.Append(",Redirect.SRVADDSEQ,String")
    '        workSession.Append(",")
    '        workSession.Append("0")

    '        '編集フラグ
    '        workSession.Append(",Redirect.EDITFLG,String")
    '        workSession.Append(",")
    '        workSession.Append("0")

    '        'TCアカウント(@以降は切り捨て)
    '        workSession.Append(",Redirect.TCACCOUNT,String")
    '        workSession.Append(",")
    '        workSession.Append(Split(Me.accountStaffContext.Account, "@")(0))

    '        Logger.Info(workSession.ToString)
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    '        Return workSession.ToString
    '    End Function

    '    ''' <summary>
    '    ''' 通知API起動メソッド
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Sub NotificationAPI()
    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

    '        Dim noticeData As XmlNoticeData = Nothing
    '        Dim account1 As XmlAccount = Nothing
    '        Dim account2 As XmlAccount = Nothing
    '        Dim pushInfo As XmlPushInfo = Nothing
    '        Dim requestNotice As XmlRequestNotice = Nothing

    '        Try
    '            requestNotice = New XmlRequestNotice
    '            requestNotice.DealerCode = Me.accountStaffContext.DlrCD             '送信者の販売店コード
    '            requestNotice.StoreCode = Me.accountStaffContext.BrnCD              '送信者の店舗コード
    '            requestNotice.FromAccount = Me.accountStaffContext.Account          '送信者のアカウント
    '            requestNotice.FromAccountName = Me.accountStaffContext.UserName     '送信者名
    '            requestNotice.Message = CreateNotifyMessage()                       '表示メッセージ文言
    '            requestNotice.SessionValue = CreateNotifySession()                  'Sessionに格納するキー名、値の型、値をカンマ区切りで格納

    '            pushInfo = New XmlPushInfo
    '            pushInfo.PushCategory = NotifyPushCategoryPopup                     'カテゴリータイプ
    '            pushInfo.PositionType = NotifyPotisionTypeHeader                    '表示位置
    '            pushInfo.Time = 3                                                   '通知表示時間(秒)
    '            pushInfo.DisplayType = NotifyDispTypeText                           '表示タイプ(1:text)
    '            pushInfo.DisplayContents = CreateDisplayContents()                  '表示内容(メッセージ文言)
    '            pushInfo.Color = NotifyColorYellow                                  'ポップアップの色:薄い黄色(F9EDBE64) -> 担当者必須レベル
    '            pushInfo.DisplayFunction = NotifyDispFunction                       '表示時に実行したい関数(JavaScript)

    '            noticeData = New XmlNoticeData
    '            noticeData.TransmissionDate = DateTimeFunc.Now(Me.accountStaffContext.DlrCD)    '送信した日時

    '            ' 受信先のアカウント     'TODO: GTMC専用の通知受信者の二人のFMアカウント（ハードコーディングでよいとのことは上司に了承済み）
    '            account1 = New XmlAccount
    '            account2 = New XmlAccount
    '            account1.ToAccount = "FMA001@44A10"
    '            account2.ToAccount = "FMA002@44A10"
    '            noticeData.AccountList.Add(account1)
    '            noticeData.AccountList.Add(account2)

    '            noticeData.RequestNotice = requestNotice
    '            noticeData.PushInfo = pushInfo

    '            Using ic3040801Biz As New IC3040801BusinessLogic
    '                ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)
    '            End Using

    '        Finally
    '            noticeData.Dispose()
    '            account1.Dispose()
    '            account2.Dispose()
    '            pushInfo.Dispose()
    '            requestNotice.Dispose()
    '        End Try

    '        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")
    '    End Sub
    '#End Region

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

#Region "エラー処理"

    ''' <summary>
    ''' エラーIDより、エラーメッセージを出して、画面リフレッシュ
    ''' </summary>
    ''' <param name="errId">エラーID</param>
    ''' <param name="workFlg">開始・日跨ぎ・終了の判断フラグ</param>
    ''' <remarks></remarks>
    Private Sub showErrMsgAndRefresh(ByVal errId As Long, ByVal workFlg As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START,param1:{2},param2:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , errId _
                    , workFlg))

        'エラーIDにより、エラー文言を取得
        Dim strMsg As String = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, errId))

        '取得できてなかった場合、各操作のディフォルトメッセージを出す
        If String.IsNullOrEmpty(strMsg) Then
            If workFlg.Equals(workStartFlg) Then
                'Cannot start
                errId = 906
            ElseIf workFlg.Equals(workFinishFlg) Then
                'Cannot finish.
                errId = 913
            ElseIf workFlg.Equals(workStopFlg) Then
                'Cannot stop the job.
                errId = 916
            End If
            strMsg = WebWordUtility.GetWord(APPLICATION_ID, errId)
        End If

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
        If errId = 927 Then
            Dim stallName As String = serviceCommon.GetStallNameWithRelationChip(CStr(stallUseId), Me.stallId)
            If Not String.IsNullOrEmpty(stallName) Then
                strMsg = String.Format(CultureInfo.CurrentCulture, _
                                       WebWordUtility.GetWord(APPLICATION_ID, 928), _
                                       stallName)
            End If
        End If

        strMsg = strMsg.Replace("\", "\\").Replace("'", "\'")
        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

        'Scriptを作成
        Dim sbScript As New StringBuilder()
        sbScript.Append("alert('")
        sbScript.Append(strMsg)
        sbScript.Append("');")

        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
        'sbScript.Append("parentScreenReLoad();")
        '画面更新フラグを設定
        Me.HiddenRefreshFlg.Value = RefreshFlg
        '2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

        'エラーメッセージを出して、画面リフレッシュ
        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "ShowMessageAndRefresh", _
                                            sbScript.ToString(), _
                                            True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END,Show Message:{2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , strMsg))
    End Sub

    '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' DMS除外エラーメッセージ表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub showMessageWarningOmitDmsError()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '「-9000：DMS除外エラーの警告」ではない場合
        'エラーメッセージを出して
        Dim errorMessage As String = WebWordUtility.GetWord(APPLICATION_ID, 923)

        'Scriptを作成
        Dim sbScript As New StringBuilder()
        sbScript.Append("alert('")
        sbScript.Append(errorMessage)
        sbScript.Append("');")

        'エラーメッセージを出力
        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "ShowMessageWarningOmitDmsError", _
                                            sbScript.ToString(), _
                                            True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END,Show Message:{2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , errorMessage))
    End Sub

    '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

End Class