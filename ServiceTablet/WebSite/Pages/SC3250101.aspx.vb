
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250101.aspx.vb
'─────────────────────────────────────
'機能： 商品訴求メイン（車両）画面 コードビハインド
'補足： 
'作成： 2014/02/XX NEC 鈴木
'更新： 2014/03/xx NEC 上野
'更新： 2014/04/xx NEC 脇谷
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Data
Imports Toyota.eCRB.iCROP.BizLogic.SC3250101
Imports Toyota.eCRB.iCROP.BizLogic.SC3250101.SC3250101WebServiceClassBusinessLogic_CreateXml
Imports Toyota.eCRB.iCROP.DataAccess.SC3250101
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'【***CONN-0090 デフォルト値をシステム設定値から取得***】 start
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'【***CONN-0090 デフォルト値をシステム設定値から取得***】 start
Imports System.Globalization
'Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
'Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.iCROP.BizLogic.SC3250101.SC3250101WebServiceClassBusinessLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet

''' <summary>
''' 商品訴求メイン（車両）画面
''' </summary>
''' <remarks></remarks>
Partial Class SC3250101
    Inherits BasePage

#Region "クラス内定義"

#Region "メンバ変数"
    ''' <summary>key=ImageUrl/value部位ヘッダーに表示するイメージ、key=INSPEC_TYPE/value=点検種類</summary>
    Private dicPartInfoDetail As New Dictionary(Of String, String)

    ''' <summary>key=部位名/value={key=ImageUrl/value部位ヘッダーに表示するイメージ、key=INSPEC_TYPE/value=点検種類}</summary>
    Private dicPartInfo As New Dictionary(Of String, Dictionary(Of String, String))

    ''' <summary>
    ''' Suggestアイコンリスト
    ''' </summary>
    ''' <remarks></remarks>
    Private images As New List(Of String)

    ''' <summary>
    ''' Resultアイコンリスト
    ''' </summary>
    ''' <remarks></remarks>
    Private ResultImages As New List(Of String)

    ''' <summary>
    ''' 部位名リスト
    ''' </summary>
    ''' <remarks></remarks>
    Private PartNames As New List(Of String)

    ''' <summary>
    ''' メイン画面コンテンツエリア
    ''' </summary>
    ''' <remarks></remarks>
    Private holder As ContentPlaceHolder

    ' ''' <summary>
    ' ''' フッター画面コンテンツエリア
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private holderFotter As ContentPlaceHolder

    ''' <summary>
    ''' StaffContext（基盤情報）
    ''' </summary>
    ''' <remarks></remarks>
    Private staffInfo As StaffContext

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    'ビジネスロジックのNewイベントが発生する際に型式利用フラグを設定します
    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private Biz As SC3250101BusinessLogic
    '2019/07/05　TKM要件:型式対応　END　　↑↑↑

    ''' <summary>
    ''' Webサービスビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private BizSrv As New SC3250101WebServiceClassBusinessLogic

    ''' <summary>
    ''' 各部位のGridViewリスト
    ''' </summary>
    ''' <remarks></remarks>
    Private lstGridView As New List(Of GridView)

    ''' <summary>
    ''' Suggestアイコンリスト（初期表示アイテム変換用）
    ''' </summary>
    ''' <remarks></remarks>
    Private SuggestNoList As New List(Of String)

    ''' <summary>
    ''' セッションパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Params As New Parameters

    ''' <summary>
    ''' モデルコード名
    ''' </summary>
    ''' <remarks></remarks>
    Private strModelCode As String                  'モデルコード

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 型式名
    ''' </summary>
    ''' <remarks></remarks>
    Private strKatashiki As String                  '型式
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 型式名
    ''' </summary>
    ''' <remarks></remarks>
    Private strGradeInfo As String                  'グレードコード

    'Private strChangeModelCode As String
    '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
    ''' <summary>
    ''' デフォルトのモデルコード名
    ''' </summary>
    ''' <remarks></remarks>
    Private DefaultModelCode As String = String.Empty
    '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑

    ''' <summary>
    ''' ヘッダーに表示するResult一覧リスト
    ''' </summary>
    ''' <remarks></remarks>
    Private ResultList As New List(Of SC3250101DataSet.ResultListRow)

    ''' <summary>
    ''' DMS変換後の販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private DmsDealerCode As String

    ''' <summary>
    ''' DMS変換後の店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private DmsBranchCode As String

    ''' <summary>
    ''' DMS変換後のログインユーザ
    ''' </summary>
    ''' <remarks></remarks>
    Private DmsLoginUserID As String

    ''' <summary>
    ''' 点検種類
    ''' </summary>
    ''' <remarks></remarks>
    Private InspecType As New SC3250101BusinessLogic.InspectionType

    ''' <summary>
    ''' R/Oステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private ROStatus As String

    '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
    ''' <summary>
    ''' 文言DBより取得した内容：Suggest
    ''' </summary>
    ''' <remarks></remarks>
    Private WordSuggest As String

    ''' <summary>
    ''' 文言DBより取得した内容：Result
    ''' </summary>
    ''' <remarks></remarks>
    Private WordResult As String
    '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑

    ''' <summary>
    ''' ROActiveフラグ true:Activeに存在する False:Activeに存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private isRoActive As Boolean = True

#End Region

#Region "定数"
    ''' <summary>最初の点検種類</summary>
    Private Const FIRST_INSPEC_TYPE As String = "0"

    '【***CONN-0090 デフォルト値をシステム設定値から取得***】 start
    '' <summary>カムリのモデルコード</summary>
    'Private Const CAMRY As String = "CARY"

    ''' <summary>モデルコードシステム設定値</summary>
    Private Const SysModelCode As String = "UPSELL_DEFAULT_MODEL_CODE"
    '【***CONN-0090 デフォルト値をシステム設定値から取得***】 end

    '2014/06/13 ROステータスの定数が重複していたため修正　START　↓↓↓
    ' ''' <summary>R/Oステータス：整備完了</summary>
    'Private Const RO_CLOSE As String = "80"
    ' ''' <summary>R/Oステータス：キャンセル</summary>
    'Private Const RO_CANSEL As String = "99"
    '2014/06/13 ROステータスの定数が重複していたため修正　END　　↑↑↑

    ''' <summary>
    ''' 商品訴求部位の数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GRIDVIEW_NUMBER As Integer = 9

    ''' <summary>
    ''' デフォルトのSuggestアイコン番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_SUGGEST_ICON As String = "5"

    ''' <summary>
    ''' デフォルトの推奨ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_SUGGEST_STATUS As String = "0"

    ''' <summary>
    ''' Suggestアイコンの最大値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_SUGGEST_ICON_NO As Integer = 7

    ''' <summary>
    ''' Suggest変更フラグ（変更有）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUGGEST_CHANGE_FLAG_ON As String = "1"

    ''' <summary>
    ''' Suggest変更フラグ（変更なし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUGGEST_CHANGE_FLAG_OFF As String = "0"

    ''' <summary>
    ''' Suggest変更フラグ（一時ワークに保存）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUGGEST_CHANGE_FLAG_WKON As String = "2"  '一時WKに保存されている項目

    ''' <summary>
    ''' Need Replaceアイコン番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUGGEST_NEED_REPLACE As String = "1"

    ''' <summary>
    ''' Need Replace（強く推奨）アイコン番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_REPLACE_RED_NO As Integer = 7


    ''' <summary>Registerボタンタップ</summary>
    Private Const ProcMode_Register As String = "Register"

    ''' <summary>Cartボタンタップ</summary>
    Private Const ProcMode_Cart As String = "ShowCart"

    ''' <summary>Suggestアイコンタップ</summary>
    Private Const ProcMode_PopUp As String = "ShowPopUp"

    ''' <summary>拡大画面の点検項目名タップ</summary>
    Private Const ProcMode_PartsDetail As String = "ShowPartsDetail"

    ''' <summary>拡大画面の点検項目名タップ（一時保存あり）</summary>
    Private Const ProcMode_PartsDetailWK As String = "SaveWK_ShowPartsDetail"


    ' ''' <summary>Resultヘッダタップ</summary>
    'Private Const ProcMode_ROPreview As String = "ShowROPreview"

    ' ''' <summary>Resultヘッダタップ（一時保存あり）</summary>
    'Private Const ProcMode_SaveWK_ROPreview As String = "ShowROPreview_Save"

    ''' <summary>フッターボタンタップ（一時保存あり）</summary>
    Private Const ProcMode_SaveWK As String = "SaveWK"

    ''' <summary>Registerボタン（有効）</summary>
    Private Const Register_Enable As String = "Register_Enable"

    ''' <summary>Registerボタン（無効）</summary>
    Private Const Register_Disable As String = "Register_Disable"

    ''' <summary>カートボタン（有効）</summary>
    Private Const Cart_Enable As String = "Cart_Enable"

    ''' <summary>カートボタン（無効）</summary>
    Private Const Cart_Disable As String = "Cart_Disable"

    ''' <summary>部位別見出し不明</summary>
    Private Const sUncertain As String = "－"

    ''' <summary>部位別見出し空白</summary>
    Private Const sSpace As String = "　"

    ''' <summary>サービス戻り値(ResultID)：ErrTimeout</summary>
    Private Const ErrTimeout As String = "ErrTimeout"

    ''' <summary>サービス戻り値(ResultID)：ErrOther</summary>
    Private Const ErrOther As String = "ErrOther"

    ''' <summary>サービス戻り値(ResultID)：ServiceSuccess</summary>
    Private Const ServiceSuccess As String = "0"

    ''' <summary>サービス送信時の日付フォーマット</summary>
    Private Const SERVICE_DATE_FORMAT As String = "dd/MM/yyyy HH:mm:ss"


    '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
    ''' <summary>文言DB：Result</summary>
    Private Const WORD_RESULT As Integer = 2

    ''' <summary>文言DB：Suggest</summary>
    Private Const WORD_SUGGEST As Integer = 3
    '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑

    ''' <summary>文言DB：Is it OK to clear?</summary>
    Private Const WORD_SAVE_CHECK As Integer = 23

    ''' <summary>文言DB：It is not possible to display the data.</summary>
    Private Const WORD_NO_DATA As Integer = 25

    ''' <summary>文言DB：ServiceItem is a communication error</summary>
    Private Const WORD_SERVICE_ERR As Integer = 26

    ''' <summary>文言DB：Suggestリストボックスを変更するときのメッセージ</summary>
    Private Const WORD_CART_CLEAR_CHECK As Integer = 27

    'Result/Suggestアイコン
    ''' <summary>
    ''' アイコン：None
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_None As String = "Icon_None"

    ''' <summary>
    ''' アイコン：No Problem
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_NoProblem As String = "Icon_NoProblem"

    ''' <summary>
    ''' アイコン：Already Replace
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_AlredyRepalece As String = "Icon_AlredyRepalece"

    ''' <summary>
    ''' アイコン：Already Fix
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_AlreadyFix As String = "Icon_AlreadyFix"

    ''' <summary>
    ''' アイコン：Already Clean
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_AlredyClean As String = "Icon_AlredyClean"

    ''' <summary>
    ''' アイコン：Already Swap
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_AlreadySwap As String = "Icon_AlreadySwap"

    '2014/07/07　NoActionアイコン追加　START　↓↓↓
    ''' <summary>
    ''' アイコン：No Action
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_NoAction As String = "Icon_NoAction"
    '2014/07/07　NoActionアイコン追加　END　　↑↑↑

    ''' <summary>
    ''' アイコン：Need Inspection
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Inspaction As String = "Icon_Inspaction"

    ''' <summary>
    ''' アイコン：Need Replace（黒色）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Replace_Black As String = "Icon_Replace_Black"

    ''' <summary>
    ''' アイコン：Need Fix
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Fixing As String = "Icon_Fixing"

    ''' <summary>
    ''' アイコン：Need Swap
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Swapping As String = "Icon_Swapping"

    ''' <summary>
    ''' アイコン：Need Clean
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Cleaning As String = "Icon_Cleaning"

    ' ''' <summary>
    ' ''' アイコン：Reset
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const Icon_Reset As String = "Icon_Reset"

    ''' <summary>
    ''' アイコン：Need Replace（赤色）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Replace_Red As String = "Icon_Replace_Red"

    '各部位ヘッダーアイコン
    ''' <summary>
    ''' アイコン：Engine Battery
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_EngineBattery As String = "Icon_EngineBattery"

    ''' <summary>
    ''' アイコン：Electrical
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Electrical As String = "Icon_Electrical"

    ''' <summary>
    ''' アイコン：Body
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Body As String = "Icon_Body"

    ''' <summary>
    ''' アイコン：Break System
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_BreakSystem As String = "Icon_BreakSystem"

    ''' <summary>
    ''' アイコン：Underbody
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_Underbody As String = "Icon_Underbody"

    ''' <summary>
    ''' アイコン：Other Parts
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_OtherParts As String = "Icon_OtherParts"

    ''' <summary>
    ''' アイコン：Steering System
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_SteeringSystem As String = "Icon_SteeringSystem"

    ''' <summary>
    ''' アイコン：Power Transmission
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_PowerTransmission As String = "Icon_PowerTransmission"

    ''' <summary>
    ''' アイコン：Cooling System
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Icon_CoolingSystem As String = "Icon_CoolingSystem"

    '2014/05/21 SpecialCampaignのドメイン取得変更　START　↓↓↓
    ''' <summary>
    ''' SystemEnvSetting名(スペシャルキャンペーンドメイン名)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEMENV_SPECIAL_CAMPAIGN_DOMAIN As String = "OTHER_LINKAGE_DOMAIN"
    '2014/05/21 SpecialCampaignのドメイン取得変更　　END　↑↑↑

    '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
    ''' <summary>
    ''' 13：ROプレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_13 As Long = 13

    ''' <summary>
    ''' R/O Seq. No
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ROPreview_SeqNo As String = "0"

    ' ''' <summary>
    ' ''' ビューモード（リードオンリー）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ROPreview_Readonly As String = "1"

    ''' <summary>
    ''' R/O Preview (Service history)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ROPreview_ServiceHistory As String = "1"

    ''' <summary>
    ''' ポップアップ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ROPreview_PopUpURL As String = "icrop:iurl:16::8::1004::740::-1::"
    '2014/05/27 ポップアップによるROプレビュー（過去）表示　END　　↑↑↑


    ''' <summary>
    ''' SuggestInfo(0):INSPEC_ITEM_CD（点検項目コード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnINSPEC_ITEM_CD As Integer = 0

    ''' <summary>
    ''' SuggestInfo(1):SUGGEST_ICON（現在のSuggestアイコン番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnSUGGEST_ICON As Integer = 1

    ''' <summary>
    ''' SuggestInfo(2):SUGGEST_STATUS（推奨フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnSUGGEST_STATUS As Integer = 2

    ''' <summary>
    ''' SuggestInfo(3):ChangeFlag（変更フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnChangeFlag As Integer = 3

    ''' <summary>
    ''' SuggestInfo(4):DEFAULT_STATUS（Suggest初期アイコン番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_STATUS As Integer = 4

    ''' <summary>
    ''' SuggestInfo(5):BEFORE_STATUS（変更前のSuggestアイコン番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BEFORE_STATUS As Integer = 5


    ''' <summary>ROステータス：車両情報特定前</summary>
    Private Const RO_UNKNOWN_VEHICLE As String = "0"

    ''' <summary>ROステータス：R/O発行前（顧客承認前）</summary>
    Private Const RO_BEFORE_PUBLISH As String = "10"

    '2014/06/13 ROステータスによってアドバイス表示を変更　START　↓↓↓
    ''' <summary>ROステータス：追加作業起票後（PS見積もり後）</summary>
    Private Const RO_AFTER_ADD_WK_MAKE As String = "35"
    '2014/06/13 ROステータスによってアドバイス表示を変更　END　　↑↑↑

    ''' <summary>ROステータス：R/O発行後（顧客承認後）</summary>
    Private Const RO_AFTER_PUBLISH As String = "50"

    '2014/06/13 ROステータスによってアドバイス表示を変更　START　↓↓↓
    ''' <summary>ROステータス：Close Job後</summary>
    Private Const RO_COMPLETE As String = "85"
    '2014/06/13 ROステータスによってアドバイス表示を変更　END　　↑↑↑

    ''' <summary>ROステータス：キャンセル</summary>
    Private Const RO_CANCEL As String = "99"


    ''' <summary>
    ''' 部位名のCSS Class名：1列　黒色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TITLE_ONE_LINE As String = "TbaleTitleLine OneLine"

    ''' <summary>
    ''' 部位名のCSS Class名：1列　赤色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TITLE_ONE_LINE_RED As String = "TbaleTitleLine OneLineRed"

    ''' <summary>
    ''' 部位名のCSS Class名：2列　黒色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TITLE_TWO_LINE As String = "TbaleTitleLine TwoLine"

    ''' <summary>
    ''' 部位名のCSS Class名：2列　赤色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TITLE_TWO_LINE_RED As String = "TbaleTitleLine TwoLineRed"

    ' 画面ID
    ''' <summary>
    ''' 基幹画面連携用フレームID("SC3010501")
    ''' </summary>
    Private Const APPLICATIONID_FRAMEID As String = "SC3010501"

    ' ''' <summary>
    ' ''' 現地にシステム連携用画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PGMID_LOCAL_TACT As String = "SC3010501"

    ''' <summary>
    ''' 画面ID：（SA）メイン画面（SC3140103）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_MAINMENUID As String = "SC3140103"                     '（SA）メイン画面（SC3140103）

    ''' <summary>
    ''' 画面ID：（SM）全体管理画面 （SC3220201）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_GENERALMANAGER As String = "SC3220201"      '（SM）全体管理画面 （SC3220201）

    ''' <summary>
    ''' 画面ID：未使用画面（SC3100401）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_NOASSIGNMENTLIST As String = "SC3100401"    '未使用画面（SC3100401）

    ' ''' <summary>
    ' ''' 画面ID：顧客詳細画面　（SC3080225）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const APPLICATIONID_CUSTOMERNEW As String = "SC3080225"         '顧客詳細画面　（SC3080225）

    ' ''' <summary>
    ' ''' キャンペーン画面（SC3230101）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CAMPAIGN_PAGE As String = "SC3230101"                     'キャンペーン画面（SC3230101）

    ''' <summary>
    ''' 画面ID：予約管理画面　（SC3100303）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_VSTMANAGER As String = "SC3100303"          '予約管理画面　（SC3100303）

    ' ''' <summary>
    ' ''' 画面ID：R/O一覧画面(SC3160101)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const APPLICATIONID_ORDERLIST As String = "SC3160101"           'R/O一覧画面(SC3160101)

    ''' <summary>
    ''' 画面ID：SMB工程管理画面（SC3240101）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"

    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"

    ''' <summary>
    ''' プログラムID：部品説明画面（SC3250103）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_DETAIL_PAGE As String = "SC3250103"


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

    ' ''' <summary>
    ' ''' SessionKey(DearlerCode):ログインユーザーのDMS販売店コード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_DEARLER_CODE As String = "Session.Param1"

    ' ''' <summary>
    ' ''' SessionKey(BranchCode):ログインユーザーのDMS店舗コード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_BRANCH_CODE As String = "Session.Param2"

    ' ''' <summary>
    ' ''' SessionKey(LoginUserID):ログインユーザーのアカウント
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_LOGIN_USER_ID As String = "Session.Param3"

    ' ''' <summary>
    ' ''' SessionKey(SAChipID):来店管理番号
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_SA_CHIP_ID As String = "Session.Param4"

    ' ''' <summary>
    ' ''' SessionKey(BASREZID):DMS予約ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_BASREZID As String = "Session.Param5"

    ' ''' <summary>
    ' ''' SessionKey(R_O):RO番号
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_R_O As String = "Session.Param6"

    ' ''' <summary>
    ' ''' SessionKey(SEQ_NO):RO作業連番
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_SEQ_NO As String = "Session.Param7"

    ' ''' <summary>
    ' ''' SessionKey(VIN_NO):車両登録No.のVIN
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_VIN_NO As String = "Session.Param8"

    ' ''' <summary>
    ' ''' SessionKey(ViewMode)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_VIEW_MODE As String = "Session.Param9"

    ' ''' <summary>
    ' ''' SessionValue(ViewMode)：編集
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONVALUE_EDIT As String = "0"

    ' ''' <summary>
    ' ''' SessionKey(DISP_NUM)：画面番号
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"

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
    ''' セッション名("ReqPartCD")
    ''' </summary>
    Private Const SessionReqPartCD As String = "ReqPartCD"

    ''' <summary>
    ''' セッション名("InspecItemCD")
    ''' </summary>
    Private Const SessionInspecItemCD As String = "InspecItemCD"

    ' ''' <summary>
    ' ''' 顧客詳細画面用セッション名("SessionKey.DMS_CST_ID")
    ' ''' </summary>
    'Private Const SessionDMSID As String = "SessionKey.DMS_CST_ID"

    ' ''' <summary>
    ' ''' 顧客詳細画面用セッション名("SessionKey.VIN")
    ' ''' </summary>
    'Private Const SessionVIN As String = "SessionKey.VIN"


    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    ''' <summary>
    ''' 編集モードフラグ("0"；編集) 
    ''' </summary>
    Private Const EditMode As String = "0"

    ' ''' <summary>
    ' ''' メインメニュー
    ' ''' </summary>
    'Private Const MAIN_MENU As Integer = 100

    ' ''' <summary>
    ' ''' 顧客情報
    ' ''' </summary>
    'Private Const CUSTOMER_INFORMATION As Integer = 200

    ' ''' <summary>
    ' ''' R/O作成
    ' ''' </summary>
    'Private Const SUBMENU_RO_MAKE As Integer = 600

    ' ''' <summary>
    ' ''' スケジューラ
    ' ''' </summary>
    'Private Const SUBMENU_SCHEDULER As Integer = 400

    ' ''' <summary>
    ' ''' 電話帳
    ' ''' </summary>
    'Private Const SUBMENU_TELEPHONE_BOOK As Integer = 500

    ' ''' <summary>
    ' ''' 追加作業一覧
    ' ''' </summary>
    'Private Const SUBMENU_ADD_LIST As Integer = 1100

    ' ''' <summary>
    ' ''' フッターコード：SMB
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_SMB As Integer = 800

    ' ''' <summary>
    ' ''' フッターイベントの置換用文字列
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_RO_LIST As String = "14"

    ''' <summary>
    ''' キャンペーン画面(DISP_NUM:"15")
    ''' </summary>
    Private Const APPLICATIONID_CAMPAIGN As String = "15"


    ' ''' <summary>
    ' ''' フッターコード：顧客詳細
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_CUSTOMER As Integer = 700

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

    'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 start
    ''' <summary>
    ''' DB更新エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MsgID_DBERR As Integer = 999
    'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 end

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' DB更新エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Msg_Exclusion As Integer = 29
    '【***完成検査_排他制御***】 end

#End Region

#Region "列挙体"

    '2014/05/20 完成検査結果データ取得変更　START　↓↓↓
    ''' <summary>
    ''' 選択状態
    ''' </summary>
    ''' <remarks></remarks>
    Enum SelectFlg
        CheckOff = 0 '無効
        CheckOn = 1  '有効
    End Enum

    ''' <summary>
    ''' 点検結果
    ''' </summary>
    ''' <remarks></remarks>
    Enum InspecResultCD
        Notselected = 0 '未実施
        NoProblem = 1
        NeedInspection = 2
        NeedReplace = 3
        NeedFixing = 4
        NeedCleaning = 5
        NeedSwapping = 6
        NoAction = 7        '7:No Action　（2014/07/07　NoActionアイコン追加）
        AlreadyReplace = 8
        AlreadyFixed = 9
        AlreadyCleaning = 10
        AlreadySwapped = 11
    End Enum
    '2014/05/20 完成検査結果データ取得変更　　END　↑↑↑

    ''' <summary>
    ''' 文言ID管理(Client端必要な文言)
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
        ''' <summary>様（男性向け）</summary>
        id001 = 7
        ''' <summary>様（女性向け）</summary>
        id002 = 8
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
        ''' <summary>来店実績:{0}台</summary>
        id003 = 18
        ''' <summary>データベースへのアクセスにてタイムアウトが発生しました。再度実行して下さい。</summary>
        id004 = 901
        'id003 = 901
        ''' <summary>そのチップは、既に他のユーザーによって変更が加えられています。画面を再表示してから再度処理を行ってください。</summary>
        id005 = 902
        'id004 = 902
        ''' <summary>予期せぬエラーが発生しました。画面を再表示してから再度処理を行ってください。</summary>
        id006 = 903
        'id005 = 903
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
    End Enum

#End Region

#Region "クラス"
    ''' <summary>
    ''' Getパラメーター格納用クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class Parameters
        ''' <summary>販売店コード</summary>
        Public DealerCode As String
        ''' <summary>店舗コード</summary>
        Public BranchCode As String
        ''' <summary>ログインユーザID</summary>
        Public LoginUserID As String
        ''' <summary>SAChipID</summary>
        Public SAChipID As String
        ''' <summary>BASREZID</summary>
        Public BASREZID As String
        ''' <summary>R/O</summary>
        Public R_O As String
        ''' <summary>SEQ_NO</summary>
        Public SEQ_NO As String
        ''' <summary>VIN_NO</summary>
        Public VIN_NO As String
        ''' <summary>ViewMode 1=Readonly / 0=Edit</summary>
        Public ViewMode As String
    End Class
#End Region

#End Region

#Region "イベントハンドラ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'バージョンログの記録（＊＊＊＊＊＊＊＊配信時に必ず日付変更すること！！！！＊＊＊＊＊＊＊＊＊＊＊＊＊）
        'Logger.Info("***** Version:2014/06/03 11:57 *****")
        'リクエストURLの記録
        'Logger.Info(String.Format("URL:{0}", Request.Url.ToString))

        ' 初期処理
        InitProc()

        '【***CONN-0090 デフォルト値をシステム設定値から取得***】 START
        If String.IsNullOrWhiteSpace(DefaultModelCode) Then
'           Logger.Error(WebWordUtility.GetWord(WORD_NO_DATA))
            Me.ShowMessageBox(WORD_NO_DATA)
            Exit Sub
        End If
        '【***CONN-0090 デフォルト値をシステム設定値から取得***】 END

        '基幹コードへ変換処理
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
        Dim dmsDlrBrnRow As SC3250101DataSet.DmsCodeMapRow = Me.GetDmsBlnCd(staffInfo.DlrCD, staffInfo.BrnCD, Params.LoginUserID)
        'Dim dmsDlrBrnRow As SC3250101DataSet.DmsCodeMapRow = Me.GetDmsBlnCd(Params.DealerCode, Params.BranchCode)
        'Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(Params.DealerCode, Params.BranchCode)
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

        DmsDealerCode = dmsDlrBrnRow.CODE1
        DmsBranchCode = dmsDlrBrnRow.CODE2
        DmsLoginUserID = dmsDlrBrnRow.ACCOUNT

        'If IsNothing(dmsDlrBrnRow) OrElse dmsDlrBrnRow.IsCODE1Null OrElse dmsDlrBrnRow.IsCODE2Null Then
        '    '変換失敗した場合は、変換前のコードを入れる
        '    DmsDealerCode = Params.DealerCode
        '    DmsBranchCode = Params.BranchCode
        '    DmsLoginUserID = Params.LoginUserID
        'Else
        '    '変換成功時は変換後のコードを入れる
        '    DmsDealerCode = dmsDlrBrnRow.CODE1
        '    DmsBranchCode = dmsDlrBrnRow.CODE2
        '    DmsLoginUserID = dmsDlrBrnRow.ACCOUNT
        'End If

        '変換後の販売店コード、店舗コードをログ出力
        '2014/06/10 エラーログに出力するように変更　START　↓↓↓
        Logger.Info(String.Format("DmsDealerCode:[{0}], DmsBranchCode:[{1}], DmsLoginUserID:[{2}]", _
                                   DmsDealerCode, DmsBranchCode, DmsLoginUserID))
        '2014/06/10 エラーログに出力するように変更　END　　↑↑↑

        'キャンペーンURLを取得して設定する
        'リンク先　→　http://dmstl-dev.toyota.co.th:9082/tops/do/spad017
        'パラメータ　→　http://{0}/tops/do/spad017?DealerCode={1}&BranchCode={2}&LoginUserID={3}&SAChipID={4}&BASREZID={5}&R_O={6}&SEQ_NO={7}&VIN_NO={8}&ViewMode={9}?
        'DISP_NO　→　19
        'SAChipIDが空の時のみ、＠以降を削除する（2014/03/21追加）
        Dim CampaignUser As String
        If 0 <= Params.LoginUserID.IndexOf("@") And String.IsNullOrWhiteSpace(Params.SAChipID) Then
            'ユーザーに「＠」が含まれている
            CampaignUser = Params.LoginUserID.Substring(0, Params.LoginUserID.IndexOf("@"))
        Else
            CampaignUser = Params.LoginUserID
        End If

        'パラメータ作成
        Dim parameterList As New List(Of String)
        parameterList.Add(DmsDealerCode)
        parameterList.Add(DmsBranchCode)
        parameterList.Add(CampaignUser)
        parameterList.Add(Params.SAChipID)
        parameterList.Add(Params.BASREZID)
        parameterList.Add(Params.R_O)
        parameterList.Add(Params.SEQ_NO)
        parameterList.Add(Params.VIN_NO)
        parameterList.Add(Params.ViewMode)

        '2014/05/21 SpecialCampaignのドメイン取得変更　START　↓↓↓
        'TBL_SYSTEMENVからドメイン名を取得
        Dim systemEnv As New SystemEnvSetting
        Dim systemEnvParam As String = String.Empty
        Dim drSystemEnvSetting As SYSTEMENVSETTINGRow = _
            systemEnv.GetSystemEnvSetting(SYSTEMENV_SPECIAL_CAMPAIGN_DOMAIN)

        '取得できた場合のみ設定する
        If Not (IsNothing(drSystemEnvSetting)) Then
            systemEnvParam = drSystemEnvSetting.PARAMVALUE
        End If

        '表示番号とパラメータとドメインからIFrameに表示するURLを作成
        Dim url As String = Me.CreateURL(19, parameterList, systemEnvParam)
        'Dim url As String = Me.CreateURL(19, parameterList, "dmstl-dev.toyota.co.th:9082")
        '2014/05/21 SpecialCampaignのドメイン取得変更　　END　↑↑↑

        Logger.Info(String.Format("CampaignURL:[{0}]", url))

        'IFrameにURLを設定
        roSpcampaigntitleiFrame.Attributes("src") = url      'キャンペーンURLの追加
        'roSpcampaigntitleiFrame.Attributes("src") = "test2.html"      'キャンペーンURLの追加（テスト用）

        '2014/06/13 ROステータスによって参照モードを変更　START　↓↓↓
        If ROStatus = RO_COMPLETE Then
            hdnViewMode.Value = "1"
        End If
        '2014/06/13 ROステータスによって参照モードを変更　END　　↑↑↑

        If IsPostBack Then
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0} OnPostBack_Start", DateTime.Now.ToString("yyyy/MM/dd/ HH:mm.ss")))
            'ログ出力 End *****************************************************************************
            ' ポストバック時処理
            OnPostBack()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0} OnPostBack_End", DateTime.Now.ToString("yyyy/MM/dd/ HH:mm.ss")))
            'ログ出力 End *****************************************************************************
            Exit Sub
        Else
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0} DispProc_Start", DateTime.Now.ToString("yyyy/MM/dd/ HH:mm.ss")))
            'ログ出力 End *****************************************************************************
            ' ページ表示処理
            DispProc()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0} DispProc_End", DateTime.Now.ToString("yyyy/MM/dd/ HH:mm.ss")))
            'ログ出力 End *****************************************************************************
        End If

        ''---動作テスト用としてカートを押したときと同じ処理を行う

        ''適当な項目を変更
        'DirectCast(lstGridView(0).Rows(1).FindControl("hdnChangeFlag"), HiddenField).Value = "1"

        ''押したボタンが「カート」
        'hdnProcMode.Value = "ShowCart"

        ''カートボタンクリック
        'Call ProcRegistData()

        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key", "fingerScrollSet();", True)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' 過去実績一覧（Result一覧）ドロップダウンリスト選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlResult.SelectedIndexChanged

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '2014/05/20 一般整備選択時Resultを「-」に変更　START　↓↓↓
        InspecType.RESULT = String.Empty
        Me.ResetResultData()
        'Logger.Info("★ddlResult.SelectedIndex:" & ddlResult.SelectedIndex.ToString)
        'Logger.Info("★ddlResult.SelectedValue:" & ddlResult.SelectedValue.ToString)
        'Dim GetInspecType As String = ResultList(CInt(ddlResult.SelectedValue.ToString))("INSPEC_TYPE").ToString

        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
        'Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedIndex.ToString)
        'Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedValue.ToString)
        'Dim strDLR_CD As String = ResultList(SelectNo)("DLR_CD").ToString
        'Dim strJOB_CD As String = ResultList(SelectNo)("JOB_CD").ToString
        'Dim strVCL_KATASHIKI As String = ResultList(SelectNo)("VCL_KATASHIKI").ToString

        '定期点検ならば明細部のResult欄を更新する（定期点検フラグの合計数が1以上ならば表示する）
        'If Biz.IsPeriodicInspection(strDLR_CD, strVCL_KATASHIKI, strJOB_CD) Then
        If 0 < ResultList(ddlResult.SelectedIndex).SERVICE Then
            'Resultヘッダに点検名称を入れる
            'InspecType.RESULT = ResultList(Integer.Parse(ddlResult.SelectedValue.ToString))("MERCHANDISENAME").ToString
            InspecType.RESULT = ResultList(ddlResult.SelectedIndex)("MERCHANDISENAME").ToString
            'Result欄に表示する
            SetResultDetail()
        End If
        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        'Suggestアイコンの再表示
        Call AllPartRegenerate()
        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

        'If Not String.IsNullOrWhiteSpace(ResultList(CInt(ddlResult.SelectedValue.ToString))("INSPEC_TYPE").ToString) Then
        '    SetResultDetail()
        'Else
        '    Me.ShowMessageBox(NO_DATA)
        'End If
        '2014/05/20 一般整備選択時Resultを「-」に変更　　END　↑↑↑

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
    ''' <summary>
    ''' 点検種類一覧（Suggest一覧）ドロップダウンリスト選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlSuggest_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSuggest.SelectedIndexChanged

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '現在の点検種類をhdnSuugestから取得する
        InspecType.SUGGEST = hdnSuggest.Value

        '編集モードの時のみDB削除処理を実行（参照モード時は実行しない）
        If hdnViewMode.Value = "0" Then

            '****実績データに登録されている内容を取得
            Dim dtSuggestionResult As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
            dtSuggestionResult = Biz.GetRepairSuggestionResult(staffInfo.DlrCD _
                                                               , staffInfo.BrnCD _
                                                               , staffInfo.Account _
                                                               , Params.SAChipID _
                                                               , InspecType.SUGGEST)


            '****Service送信用XMLの作成
            Dim xmlWebService As ServiceItemsXmlDocumentClass

            If dtSuggestionResult IsNot Nothing AndAlso 0 < dtSuggestionResult.Count Then
                '実績データが登録されていた
                'サービス送信用XMLの作成
                Dim DelSendData As New List(Of String())
                For Each rows In dtSuggestionResult
                    Dim DelSuggestInfo(5) As String
                    DelSuggestInfo(hdnINSPEC_ITEM_CD) = rows.INSPEC_ITEM_CD
                    DelSuggestInfo(hdnSUGGEST_ICON) = "5"
                    DelSuggestInfo(hdnSUGGEST_STATUS) = "0"
                    DelSuggestInfo(hdnChangeFlag) = "0"
                    DelSuggestInfo(DEFAULT_STATUS) = "0"
                    DelSuggestInfo(BEFORE_STATUS) = "0"

                    DelSendData.Add(DelSuggestInfo)
                Next
                'Service送信用XMLの作成
                xmlWebService = CreateXMLOfRegister(DelSendData, SC3250101WebServiceClassBusinessLogic.GetServiceItems_Info.WebServiceIDValue)
            Else
                xmlWebService = Nothing
            End If

            '****一時データ、実績データの削除、Webサービス送信
            Dim ret As Integer
            ret = Biz.DeleteSuggestionResultProcess(staffInfo.DlrCD _
                                                  , staffInfo.BrnCD _
                                                  , staffInfo.Account _
                                                  , Params.SAChipID _
                                                  , InspecType.SUGGEST _
                                                  , dtSuggestionResult _
                                                  , xmlWebService)

            '****Webサービス送信結果をチェックする
            If ret = SC3250101BusinessLogic.WEBSERVICE_ERROR Then
                'Webサービス送信エラー
                'エラーメッセージを表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(WORD_SERVICE_ERR)

                'Suggestリストボックスを変更前の値に戻す
                ddlSuggest.SelectedValue = InspecType.SUGGEST

                Exit Sub
            ElseIf ret = SC3250101BusinessLogic.DATABASE_ERROR Then
                'DB更新エラー
                'Suggestリストボックスを変更前の値に戻す
                ddlSuggest.SelectedValue = InspecType.SUGGEST
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If

        End If

        '****選択したSuggestを表示する
        If 0 < ddlSuggest.Items.Count AndAlso Not String.IsNullOrWhiteSpace(ddlSuggest.SelectedValue) Then
            'AlreadySendフラグをリセットする
            hdnAlreadySendFlag.Value = "0"

            'Changeフラグをリセットする
            hdnChangeFlg.Value = "0"

            'Suggestの点検種類をSuggestのリストで選択した種類に変更する
            InspecType.SUGGEST = ddlSuggest.SelectedValue
            hdnSuggest.Value = InspecType.SUGGEST

            InspecType.SUGGEST_DISP = ddlSuggest.Items(ddlSuggest.SelectedIndex).Text.Replace(" Inspection (M)", "")

            'Result及びSuggestのアイコンを各点検に設定
            SetResultAndSuggestDetail()
        End If


        '****Registerボタンを設定する
        If hdnChangeFlg.Value = "0" Then
            imgRegister.Attributes.Add("class", Register_Disable)
        Else
            imgRegister.Attributes.Add("class", Register_Enable)
        End If

        imgCart.Attributes.Add("class", Cart_Disable)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
    '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑

#End Region

#Region "イベント発生時のページ処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitProc()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        staffInfo = StaffContext.Current
        holder = DirectCast(Me.Master.FindControl("content"), ContentPlaceHolder)

        '【***完成検査_排他制御***】 start

        ''販売店コード(DealerCode)
        'Params.DealerCode = DirectCast(GetValue(ScreenPos.Current, "DealerCode", False), String)
        ''店舗コード(BranchCode)
        'Params.BranchCode = DirectCast(GetValue(ScreenPos.Current, "BranchCode", False), String)

        '販売店コード(DealerCode)
        Params.DealerCode = staffInfo.DlrCD
        '店舗コード(BranchCode)
        Params.BranchCode = staffInfo.BrnCD

        '【***完成検査_排他制御***】 end
        
        'ログインID(LoginUserID)
        Params.LoginUserID = DirectCast(GetValue(ScreenPos.Current, "LoginUserID", False), String)

        '販売店コード、店舗コード、店舗コードに関してはパラメータより取得できなかった場合、
        '基盤から情報を取得する（2014/03/12追加）
        If String.IsNullOrWhiteSpace(Params.DealerCode) Then
            Params.DealerCode = staffInfo.DlrCD
        End If
        If String.IsNullOrWhiteSpace(Params.BranchCode) Then
            Params.BranchCode = staffInfo.BrnCD
        End If
        If String.IsNullOrWhiteSpace(Params.LoginUserID) Then
            Params.LoginUserID = staffInfo.Account
        End If

        '来店実績連番(SAChipID)
        Params.SAChipID = DirectCast(GetValue(ScreenPos.Current, "SAChipID", False), String)
        'DMS予約ID（BASREZID）
        Params.BASREZID = DirectCast(GetValue(ScreenPos.Current, "BASREZID", False), String)
        'RO番号（R_O）
        Params.R_O = DirectCast(GetValue(ScreenPos.Current, "R_O", False), String)
        'RO作業連番（SEQ_NO）
        Params.SEQ_NO = DirectCast(GetValue(ScreenPos.Current, "SEQ_NO", False), String)
        'VIN（VIN_NO）
        Params.VIN_NO = DirectCast(GetValue(ScreenPos.Current, "VIN_NO", False), String)
        '編集モード（ViewMode）
        Params.ViewMode = DirectCast(GetValue(ScreenPos.Current, "ViewMode", False), String)

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        'newした際に型式使用フラグを設定する
        Biz = New SC3250101BusinessLogic(Params.R_O, Params.DealerCode, Params.BranchCode)
        '2019/07/05　TKM要件:型式対応　END　  ↑↑↑

        '2017/XX/XX ライフサイクル対応　↓
        'パラメータのRO情報がある場合に、Activeであるか確認する。
        'パラメータのRO情報が無い場合は即ち過去のデータではないため、Activeの確認の必要はない。
        If Not String.IsNullOrWhiteSpace(Params.R_O) Then

            '販売店コード、店舗コード、RO番号により、開いていたROがActiveであるか確認する。
            isRoActive = Biz.ChkExistParamRoActive(Params.DealerCode, Params.BranchCode, Params.R_O)

        End If

        'RO情報が存在し、Activeでない場合はデータ登録をできないようにするためViewModeをReadとする。
        'If Not isRoActive Then
        'Params.ViewMode = ReadMode
        'End If
        '2017/XX/XX ライフサイクル対応　↑

        'R_Oが空白の場合、編集モードを「1」に設定する
        '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
        'If String.IsNullOrEmpty(Params.R_O) Then
        '    Params.ViewMode = "1"
        'End If
        '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
        hdnViewMode.Value = Params.ViewMode

        '2014/06/10 エラーログに出力するように変更　START　↓↓↓
        '取得したパラメータ情報をログに記録
        Logger.Info(String.Format("Params:DealerCode:[{0}], BranchCode:[{1}], LoginUserID:[{2}], SAChipID:[{3}], BASREZID:[{4}], R_O:[{5}], SEQ_NO:[{6}], VIN_NO:[{7}], ViewMode:[{8}]", _
                                  Params.DealerCode, _
                                  Params.BranchCode, _
                                  Params.LoginUserID, _
                                  Params.SAChipID, _
                                  Params.BASREZID, _
                                  Params.R_O, _
                                  Params.SEQ_NO, _
                                  Params.VIN_NO, _
                                  Params.ViewMode))
        Logger.Info(String.Format("StaffInfo:DlrCD:[{0}], BrnCD:[{1}], Account:[{2}]", staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account))
        '2014/06/10 エラーログに出力するように変更　END　　↑↑↑

        'Resultアイコンの登録
        '2014/05/20 完成検査結果データ取得変更　START　↓↓↓
        '2014/07/07　NoActionアイコン追加　START　↓↓↓
        '　「NoAction：7」を追加することで、AlreadyReplace以降がずれる
        '「SC3180202＿チェックシートプレビュー」と表示方法が異なっているため、リスト修正
        ResultImages.Add(Icon_None)           '0:選択無
        ResultImages.Add(Icon_NoProblem)      '1:No Problem
        ResultImages.Add(Icon_Inspaction)     '2:Need Inspection
        ResultImages.Add(Icon_Replace_Black)  '3:Need Replace 
        ResultImages.Add(Icon_Fixing)         '4:Need Fixing
        ResultImages.Add(Icon_Cleaning)       '5:Need Cleaning
        ResultImages.Add(Icon_Swapping)       '6:Need Swapping
        ResultImages.Add(Icon_NoAction)       '7:No Action
        ResultImages.Add(Icon_AlredyRepalece) '8:Alredy Repalece
        ResultImages.Add(Icon_AlreadyFix)     '9:Already Fix
        ResultImages.Add(Icon_AlredyClean)    '10:Alredy Clean
        ResultImages.Add(Icon_AlreadySwap)    '11:Already Swap
        '2014/07/07　NoActionアイコン追加　END　　↑↑↑

        'ResultImages.Add(Icon_None)              '0
        'ResultImages.Add(Icon_NoProblem)         '1
        'ResultImages.Add(Icon_None)              '2
        'ResultImages.Add(Icon_AlredyRepalece)    '3  
        'ResultImages.Add(Icon_AlreadyFix)        '4
        'ResultImages.Add(Icon_AlredyClean)       '5
        'ResultImages.Add(Icon_AlreadySwap)       '6
        'ResultImages.Add(Icon_None)              '7
        '2014/05/20 完成検査結果データ取得変更　　END　↑↑↑

        'Suggestアイコンの登録
        images.Add(Icon_Inspaction)              'Need Inspaction
        images.Add(Icon_Replace_Black)           'Need Replace   
        images.Add(Icon_Fixing)                  'Need Fixing    
        images.Add(Icon_Swapping)                'Need Swapping  
        images.Add(Icon_Cleaning)                'Neet Cleaning  
        images.Add(Icon_None)                    'None
        images.Add(Icon_None)                    'None
        images.Add(Icon_Replace_Red)             'Need Replace（強く推奨）

        'Suggestアイテムコードの変換  DB上　→　APP上
        '2014/06/24　初期表示用アイテム取得時の変換処理を修正　START　↓↓↓
        SuggestNoList.Add("5")  '0　→　5　選択なし
        SuggestNoList.Add("5")  '1　→　5　NoProblem
        SuggestNoList.Add("0")  '2　→　0　NeedInspection　
        SuggestNoList.Add("1")  '3　→　1　NeedReplace　
        SuggestNoList.Add("2")  '4　→　2　NeedFixing　
        SuggestNoList.Add("4")  '5　→　4　NeedCleaning　
        SuggestNoList.Add("3")  '6　→　3　NeedSwapping　
        SuggestNoList.Add("6")  '7　→　6　Reset　
        'SuggestNoList.Add("5")  '0　→　5　選択なし
        'SuggestNoList.Add("0")  '1　→　5　NoProblem
        'SuggestNoList.Add("1")  '2　→　0　NeedInspection　
        'SuggestNoList.Add("2")  '3　→　1　NeedReplace　
        'SuggestNoList.Add("4")  '4　→　2　NeedFixing　
        'SuggestNoList.Add("3")  '5　→　4　NeedCleaning　
        'SuggestNoList.Add("5")  '6　→　3　NeedSwapping　
        'SuggestNoList.Add("5")  '7　→　6　Reset　
        '2014/06/24　初期表示用アイテム取得時の変換処理を修正　END　　↑↑↑

        Dim ImageURL As New List(Of String)
        ImageURL.Add(Icon_EngineBattery)
        ImageURL.Add(Icon_CoolingSystem)
        ImageURL.Add(Icon_Electrical)
        ImageURL.Add(Icon_PowerTransmission)
        ImageURL.Add(Icon_Body)
        ImageURL.Add(Icon_SteeringSystem)
        ImageURL.Add(Icon_BreakSystem)
        ImageURL.Add(Icon_Underbody)
        ImageURL.Add(Icon_OtherParts)

        For i As Integer = 1 To GRIDVIEW_NUMBER
            Dim strNo As String
            '各部位情報
            strNo = i.ToString.PadLeft(2, "0"c)
            dicPartInfoDetail = New Dictionary(Of String, String)
            'dicPartInfoDetail.Add("ImageUrl", ImageURL(Integer.Parse(ChangeListNo(strNo)) - 1))
            dicPartInfoDetail.Add("ImageUrl", ImageURL(i - 1))
            dicPartInfoDetail.Add("SVC_CD", String.Empty)

            '【***完成検査_排他制御***】 start
            If dicPartInfo.ContainsKey(strNo) Then
                dicPartInfo(strNo) = dicPartInfoDetail
            Else
            	dicPartInfo.Add(strNo, dicPartInfoDetail)
            End If

            '部位名のリスト作成
            If PartNames.Contains(strNo) = False Then
            	PartNames.Add(strNo)
            End If

            If lstGridView.Contains(DirectCast(contentsArea.FindControl(String.Format("List{0}_Data", strNo)), GridView)) = False Then
            'GridViewリストの作成
            	lstGridView.Add(DirectCast(contentsArea.FindControl(String.Format("List{0}_Data", strNo)), GridView))
            End If
            '【***完成検査_排他制御***】 end

        Next

        ''各部位情報
        'For i As Integer = 1 To 9
        '    Dim strNo As String
        '    strNo = String.Format("0{0}", i)
        '    dicPartInfoDetail = New Dictionary(Of String, String)
        '    dicPartInfoDetail.Add("ImageUrl", ImageURL(CInt(ChangeListNo(strNo)) - 1))
        '    dicPartInfoDetail.Add("INSPEC_TYPE", String.Empty)
        '    dicPartInfo.Add(strNo, dicPartInfoDetail)
        'Next

        ''部位名のリスト作成
        'For Each key As String In dicPartInfo.Keys
        '    PartNames.Add(key)
        'Next

        ''GridViewリストの作成
        'lstGridView.Add(List01_Data)
        'lstGridView.Add(List02_Data)
        'lstGridView.Add(List03_Data)
        'lstGridView.Add(List04_Data)
        'lstGridView.Add(List05_Data)
        'lstGridView.Add(List06_Data)
        'lstGridView.Add(List07_Data)
        'lstGridView.Add(List08_Data)
        'lstGridView.Add(List09_Data)

        'ROが空白でなければR/Oステータス（以下のパターンに分類したもの）を取得する
        '①車両情報特定前
        '②R/O発行前（顧客承認前）
        '③追加作業起票後（PS見積もり後）
        '④R/O発行前（顧客承認前）
        '⑤R/O発行後（顧客承認後）
        '⑥Close Job後

        If Not String.IsNullOrWhiteSpace(Params.R_O) Then
            'ROStatus = Biz.GetROStatus(Params.DealerCode, Params.BranchCode, Params.R_O)
            ROStatus = Biz.GetConvROStatus(staffInfo.DlrCD, staffInfo.BrnCD, Params.R_O)
        Else
            'ROが空白のため、ROステータス＝車両情報特定前
            ROStatus = RO_UNKNOWN_VEHICLE
        End If

        'VINからResult一覧を取得してリストに格納する
        '2015/04/14 新販売店追加対応 start
        Dim specifyDlrCdFlgs As Boolean = Biz.ChkDlrCdExistMst(staffInfo.DlrCD)
        'Dim dtResultData As SC3250101DataSet.ResultListDataTable = Biz.GetResultList(Params.VIN_NO)
        Dim dtResultData As SC3250101DataSet.ResultListDataTable = Biz.GetResultList(Params.VIN_NO, specifyDlrCdFlgs)
        '2015/04/14 新販売店追加対応 end

        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
        '①VINをキーにR/O一覧を取得する
        Dim dtRoList As SC3250101DataSet.RO_NUM_ListDataTable = Biz.GetRoList(Params.VIN_NO)

        '②取り出したR/Oをキーに表示する点検種類、名称を入れる
        If 0 < dtResultData.Rows.Count And 0 < dtRoList.Rows.Count Then
            'R/Oリストの数だけループする
            For Each ROrow As SC3250101DataSet.RO_NUM_ListRow In dtRoList

                'RO番号を取り出す
                Dim RONum As String = ROrow.RO_NUM

                'ROチェック・・・パラメータのROと同じ、かつROステータスがCloseJob後以外ならばリストに追加しない
                '過去のROについても、納車済みでなければリストへ追加しない（納車から10日以内のデータの遷移の場合に作業中のROが表示されてしまうため）
                If RONum = Params.R_O AndAlso ROStatus <> RO_COMPLETE Then
                    Continue For
                ElseIf CInt(RO_COMPLETE) > CInt(ROrow.RO_STATUS) Or CInt(ROrow.RO_STATUS) >= CInt(RO_CANCEL) Then
                    Continue For
                End If

                'dtResultDataから特定のR/O番号を取り出す
                Dim ResultLists() As SC3250101DataSet.ResultListRow = _
                    DirectCast(dtResultData.Select(String.Format("RO_NUM = '{0}'", RONum)), SC3250101DataSet.ResultListRow())

                If 0 < ResultLists.Count Then
                    '点検種類、商品名を一つにまとめてResultListに入れる
                    Dim MercNamesTemp As StringBuilder = New StringBuilder
                    Dim MerchandiseNamesTemp As StringBuilder = New StringBuilder
                    Dim Service As Integer
                    For i = 0 To ResultLists.Count - 1

                        If i = 0 Then
                            '1回目のループ
                            '「点検種類　商品名」
                            MercNamesTemp.AppendFormat("{0} {1}", ResultLists(i).MERCHANDISENAME, ResultLists(i).MERC_NAME)
                            '「点検種類」
                            MerchandiseNamesTemp.AppendFormat("{0}", ResultLists(i).MERCHANDISENAME)
                            Service = ResultLists(i).SERVICE
                        Else
                            '2回目以降のループ
                            '「＋　点検種類　商品名」
                            MercNamesTemp.AppendFormat(" + {0} {1}", ResultLists(i).MERCHANDISENAME, ResultLists(i).MERC_NAME)
                            '「＋　点検種類」
                            If 0 < ResultLists(i).SERVICE Then
                                MerchandiseNamesTemp.AppendFormat("+{0}", ResultLists(i).MERCHANDISENAME)
                            End If
                            Service += ResultLists(i).SERVICE
                        End If

                        '最終Resultの決定
                        If Not String.IsNullOrWhiteSpace(ResultLists(i).MERCHANDISENAME) And 0 < ResultLists(i).SERVICE Then
                            InspecType.RESULT = ResultLists(i).SVC_CD
                        End If

                    Next

                    '点検種類、商品名を一つにまとめたものを1番目のResultListに入れる
                    ResultLists(0).MERC_NAMES = MercNamesTemp.ToString

                    '点検種類を一つにまとめたものを1番目のResultListに入れる
                    ResultLists(0).MERCHANDISENAMES = MerchandiseNamesTemp.ToString

                    '定期点検フラグの合計を1番目のResultListに入れる
                    ResultLists(0).SERVICE = Service

                    '定期点検が含まれるR/Oならば最終Resultを1番目のMERCHANDISENAMEに入れる
                    If 0 < Service Then
                        ResultLists(0).MERCHANDISENAME = InspecType.RESULT
                    End If

                    'ResultListにセットする
                    ResultList.Add(ResultLists(0))
                End If

            Next
        End If



        'Dim item As New ListItem
        'For i As Integer = 0 To dtResultData.Rows.Count - 1
        '    If Params.R_O = dtResultData.Rows(i)("RO_NUM").ToString Then
        '        '取得したResultListの中に今回のRO番号があった
        '        '2014/06/13 ROステータスの定数が重複していたため修正　RO_CLOSE　→　RO_COMPLETE
        '        If ROStatus = RO_COMPLETE Then
        '            '「CloseJob後」ならば追加する
        '            ResultList.Add(DirectCast(dtResultData.Rows(i), SC3250101DataSet.ResultListRow))
        '            '最終Resultの決定
        '            If Not String.IsNullOrWhiteSpace(dtResultData.Rows(i)("MERCHANDISENAME").ToString) Then
        '                InspecType.RESULT = dtResultData.Rows(i)("MERCHANDISENAME").ToString
        '                'InspecType.RESULT = dtResultData.Rows(i)("UPPER_DISP").ToString & dtResultData.Rows(i)("LOWER_DISP").ToString
        '            End If
        '        End If
        '    Else
        '        '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
        '        If Not String.IsNullOrWhiteSpace(dtResultData.Rows(i)("RO_NUM").ToString) Then
        '            ResultList.Add(DirectCast(dtResultData.Rows(i), SC3250101DataSet.ResultListRow))
        '            '最終Resultの決定
        '            If Not String.IsNullOrWhiteSpace(dtResultData.Rows(i)("MERCHANDISENAME").ToString) Then
        '                InspecType.RESULT = dtResultData.Rows(i)("MERCHANDISENAME").ToString
        '                'InspecType.RESULT = dtResultData.Rows(i)("UPPER_DISP").ToString & dtResultData.Rows(i)("LOWER_DISP").ToString
        '            End If
        '        End If
        '        '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
        '    End If
        'Next
        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

        '2014/07/10　今回のSuggest取得処理変更　START　↓↓↓
        '今回のSugeestの取得処理の位置を「DispProc」メソッドに移動（この時点ではモデルコードが取得できていないため）
        ''最終Resultの次の点検項目を取得する
        'If String.IsNullOrWhiteSpace(InspecType.RESULT) Then
        '    InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, FIRST_INSPEC_TYPE)
        'Else
        '    InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, InspecType.RESULT)
        'End If
        'Logger.Info(String.Format("InspecType.RESULT:[{0}], InspecType.SUGGEST:[{1}]", InspecType.RESULT, InspecType.SUGGEST))
        '2014/07/10　今回のSuggest取得処理変更　END　　↑↑↑

        'フッター初期化
        Me.InitFooterEvent()

        '編集中に画面遷移をする際に表示する確認メッセージを文言DBより取得(No.23)
        ClearMessageID.Value = WebWordUtility.GetWord(WORD_SAVE_CHECK)

        'Suggestリストボックスを変更する際に表示する確認メッセージを文言DBより取得(No.27)
        ClearCartMessageID.Value = WebWordUtility.GetWord(WORD_CART_CLEAR_CHECK)

        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
        WordResult = WebWordUtility.GetWord(WORD_RESULT)
        WordSuggest = WebWordUtility.GetWord(WORD_SUGGEST)
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑

        '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        'Hiddenフィールドにモデルコードとグレードコード、型式、Suggestがあれば取得する
        If Not String.IsNullOrEmpty(hdnModelCode.Value) Then
            strModelCode = hdnModelCode.Value
        End If
        If Not String.IsNullOrEmpty(hdnKatashiki.Value) Then
            strKatashiki = hdnKatashiki.Value
        End If
        If Not String.IsNullOrEmpty(hdnGradeCode.Value) Then
            strGradeInfo = hdnGradeCode.Value
        End If
        If Not String.IsNullOrEmpty(hdnSuggest.Value) Then
            InspecType.SUGGEST = hdnSuggest.Value
        End If

        '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑
        '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓

            'SERVICE_COMMONよりデフォルトカムリ設定値を取得する
            Dim serviceCommonBiz As New ServiceCommonClassBusinessLogic
		'【***CONN-0090 デフォルト値をシステム設定値から取得***】 START
        If String.IsNullOrWhiteSpace(DefaultModelCode) Then
            DefaultModelCode = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(SysModelCode)
        End If
        '【***CONN-0090 デフォルト値をシステム設定値から取得***】 END
        '2019/07/05　TKM要件:型式対応　END　↑↑↑↓
        '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' ページ表示処理（ポストバック以外）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispProc()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '【***完成検査_排他制御***】 start
        rowLockvs.Value = Biz.GetServiceinRowLockVertion(Params.DealerCode, Params.BranchCode, Params.R_O).ToString
        '【***完成検査_排他制御***】 end
        '拡大画面の消去
        contentsMainonBoard.Style.Add("display", "none")
        popUp.Style.Add("display", "none")
        closeBtn.Style.Add("display", "none")
        popUpList.Style.Add("display", "none")

        '固有ヘッダー部分を作成する（車種ロゴ、Result一覧、Suggest項目）
        SetHeaderDisp()

        '車種情報及びグレード情報をログ出力する
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
        Logger.Info(String.Format("ModelCode:[{0}], Katashiki:[{1}]", strModelCode, strKatashiki))
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
        '型式はデフォルト値　半角スペースでも存在するという判定
        If String.IsNullOrWhiteSpace(Params.VIN_NO) And (String.IsNullOrWhiteSpace(strModelCode) Or String.IsNullOrEmpty(strKatashiki)) Then
            'この時点でVINがなくて、車種情報がなければ、カムリの情報を出す(VINがなくてもROで車種特定しているため変更)
            '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
            strModelCode = DefaultModelCode
            Biz.SetUseFlgKatashiki(False)
            'strModelCode = CAMRY
            '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
            '2014/06/10 エラーログに出力するように変更　START　↓↓↓
            Logger.Info(String.Format("Set Default ModelCode and Katashiki ModelCode:[{0}], Katashiki:[{1}]", strModelCode, strKatashiki))

            '2014/06/10 エラーログに出力するように変更　END　　↑↑↑
            '2014/07/10 今回のSugeest取得処理がこの後にあるため、コメント化　START　↓↓↓
            'InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, FIRST_INSPEC_TYPE)
            'Else
            ''この時点でVINはあるが、過去点検データがなければ初回点検を表示させる
            'If String.IsNullOrWhiteSpace(InspecType.RESULT) Then
            '    InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, FIRST_INSPEC_TYPE)
            'End If
            '2014/07/10 今回のSugeest取得処理がこの後にあるため、コメント化　END　　↑↑↑
        End If

        'この時点でモデル情報がなければ、カムリ情報を出す
        If String.IsNullOrWhiteSpace(strModelCode) Or String.IsNullOrEmpty(strKatashiki) Then
            '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
            strModelCode = DefaultModelCode
            Biz.SetUseFlgKatashiki(False)
            'strModelCode = CAMRY
            '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
            '2014/06/10 エラーログに出力するように変更　START　↓↓↓
            Logger.Info("Set Default ModelCode:" & strModelCode)
            '2014/06/10 エラーログに出力するように変更　END　　↑↑↑
        End If

        '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
        ''この時点で変換後の車種情報がなければカムリ情報を出す
        'If String.IsNullOrWhiteSpace(strChangeModelCode) Then
        '    strChangeModelCode = CAMRY
        '    Logger.Info("Set Default ChangeModelCode:" & strChangeModelCode)
        'End If

        '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓

        '最終Resultの次の点検項目を取得する
        If String.IsNullOrWhiteSpace(InspecType.RESULT) Then
            '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
            InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, strKatashiki, FIRST_INSPEC_TYPE, DefaultModelCode)
            'InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, FIRST_INSPEC_TYPE, CAMRY)
            '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
        Else
            '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
            InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, strKatashiki, InspecType.RESULT, DefaultModelCode)
            'InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, InspecType.RESULT, CAMRY)
            '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
        End If
        Logger.Info(String.Format("InspecType.RESULT:[{0}], InspecType.SUGGEST:[{1}]", InspecType.RESULT, InspecType.SUGGEST))


        '一時データ、実績データに登録されているSuggestを取得する
        'JobClose後は次回の点検内容を表示するため、処理を飛ばす
        If ROStatus <> RO_COMPLETE Then
            Dim SuggestDB As String = Biz.GetSuggestFromREPAIR_SUGGESTION(staffInfo.DlrCD _
                                                                          , staffInfo.BrnCD _
                                                                          , staffInfo.Account _
                                                                          , Params.SAChipID)
            '一時データ、実績に登録されていたら、取得したサービスコードをInspecType.SUGGESTに入れる
            If Not String.IsNullOrEmpty(SuggestDB) Then
                InspecType.SUGGEST = SuggestDB
            End If
        End If

        'モデルコードとグレードコード、型式をHiddenフィールドに入れる
        hdnModelCode.Value = strModelCode
        hdnGradeCode.Value = strGradeInfo
        hdnKatashiki.Value = strKatashiki
        '2019/07/05　TKM要件:型式対応　END　↑↑↑

        'hdnSuggestタグに現在のSuggest値を記録する
        hdnSuggest.Value = InspecType.SUGGEST
        '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑

        '* Suggest表示
        Dim SuggestList As New SC3250101DataSet.TB_M_INSPECTION_ORDER_ListDataTable
        '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        SuggestList = Biz.GetSuggestList(staffInfo.DlrCD _
                                        , staffInfo.BrnCD _
                                        , strModelCode _
                                        , strKatashiki _
                                        , InspecType.SUGGEST _
                                        , DefaultModelCode)
        'SuggestList = Biz.GetSuggestList(staffInfo.DlrCD _
        '                                , staffInfo.BrnCD _
        '                                , strModelCode _
        '                                , InspecType.SUGGEST _
        '                                , CAMRY)
        '2019/07/05　TKM要件:型式対応　END　↑↑↑
        '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
        Dim Item As ListItem
        For Each Row As SC3250101DataSet.TB_M_INSPECTION_ORDER_ListRow In SuggestList
            Item = New ListItem(String.Format("{0} Inspection (M)", Row("MERCHANDISENAME").ToString), Row("SVC_CD").ToString)
            ddlSuggest.Items.Add(Item)
        Next
        If InspecType.SUGGEST = "0" Then
            ddlSuggest.SelectedIndex = 0
            InspecType.SUGGEST = ddlSuggest.SelectedValue
            hdnSuggest.Value = InspecType.SUGGEST
        Else
            ddlSuggest.SelectedValue = InspecType.SUGGEST
        End If

        InspecType.SUGGEST_DISP = ddlSuggest.Items(ddlSuggest.SelectedIndex).Text.Replace(" Inspection (M)", "")

        '明細部のSuggest欄、Result欄にアイコンをセットする
        Me.SetResultAndSuggestDetail()

        ''一括で全部位の検査項目を取得する
        ''2014/03/24 オペレーション変換マスタによる変換後の車種コードで検査項目を取得する方法に変更
        'Dim dtListData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable
        ''dtListData = Biz.GetInspectionList(strModelCode, strGradeInfo)

        ''★レスポンス対策
        ''Before
        ''dtListData = Biz.GetInspectionList(strChangeModelCode, strGradeInfo)
        ''After
        ''dtListData = Biz.GetInspectionList(strChangeModelCode, _
        ''                                strGradeInfo, _
        ''                                Params.DealerCode, _
        ''                                Params.BranchCode, _
        ''                                staffInfo.Account, _
        ''                                Params.R_O, _
        ''                                InspecType.SUGGEST)
        ''2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
        ''dtListData = Biz.GetInspectionList(strChangeModelCode, _
        ''                                strGradeInfo, _
        ''                                staffInfo.DlrCD, _
        ''                                staffInfo.BrnCD, _
        ''                                staffInfo.Account, _
        ''                                Params.R_O, _
        ''                                InspecType.SUGGEST)
        'dtListData = Biz.GetInspectionList(strChangeModelCode, _
        '                                strGradeInfo, _
        '                                staffInfo.DlrCD, _
        '                                staffInfo.BrnCD, _
        '                                staffInfo.Account, _
        '                                Params.SAChipID, _
        '                                InspecType.SUGGEST, _
        '                                CAMRY)
        ''2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑

        ''2014/06/10 指定したモデルで点検マスタが取得出来なかったときの処理追加　START　↓↓↓
        'Dim dtListDataRow() As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow
        'dtListDataRow = DirectCast(dtListData.Select("REQ_ITEM_DISP_SEQ IS NOT NULL"), SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow())
        ''Logger.Info("★★★Count：" & dtListDataRow.Count.ToString)
        'If strChangeModelCode <> CAMRY And dtListDataRow.Count = 0 Then
        '    'カムリ以外のモデルコードで点検マスタが0件の時、モデルコードを「カムリ」にして一度実行
        '    dtListData = Biz.GetInspectionList(CAMRY, _
        '                        strGradeInfo, _
        '                        staffInfo.DlrCD, _
        '                        staffInfo.BrnCD, _
        '                        staffInfo.Account, _
        '                        Params.SAChipID, _
        '                        InspecType.SUGGEST, _
        '                        CAMRY)
        'End If
        'If dtListData.Count <= 0 Then
        '    'カムリでも点検マスタを取得できなかったときはエラーメッセージを表示する
        '    Logger.Error(WebWordUtility.GetWord(WORD_NO_DATA))
        '    Me.ShowMessageBox(WORD_NO_DATA)

        'End If
        ''2014/06/10 指定したモデルで点検マスタが取得出来なかったときの処理追加　END　　↑↑↑

        ''★レスポンス対策
        ' ''before
        ' ''Suggestの初期表示（お勧め点検）
        ''Dim dtDefaultData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
        ''dtDefaultData = Biz.GetSuggestDefaultList(strChangeModelCode, strGradeInfo, InspecType.SUGGEST)

        ' ''Suggestの実績データ
        ''Dim dtSuggestData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable
        ''dtSuggestData = Biz.GetRepairSuggestionRsltOfPart(Params.DealerCode, _
        ''                                                  Params.BranchCode, _
        ''                                                  staffInfo.Account, _
        ''                                                  Params.R_O, _
        ''                                                  InspecType.SUGGEST)

        ' ''Suggestの一時保存データ
        ''Dim dtSuggestWKData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable
        ''dtSuggestWKData = Biz.GetWorkRepairSuggestionRsltOfPart(Params.DealerCode, _
        ''                                                        Params.BranchCode, _
        ''                                                        staffInfo.Account, _
        ''                                                        Params.R_O, _
        ''                                                        InspecType.SUGGEST)

        ' ''明細部を表示する
        ''CreateList("01", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("02", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("03", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("04", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("05", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("06", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("07", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("08", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''CreateList("09", dtListData, dtDefaultData, dtSuggestData, dtSuggestWKData)
        ''dtDefaultData.Dispose()
        ''dtSuggestData.Dispose()
        ''dtSuggestWKData.Dispose()

        ''After
        ''明細部を表示する
        'CreateAllList(dtListData)
        'dtListData.Dispose()

        ''ここまで

        ''過去の実績（Result）を表示する
        'Me.ResetResultData()
        ''If 0 < ResultList.Count Then
        ''If 0 < ddlResult.Items.Count And Not String.IsNullOrWhiteSpace(ddlResult.SelectedValue.ToString) Then
        ''Logger.Info("★ddlResult.SelectedIndex:" & ddlResult.SelectedIndex.ToString)
        ''Logger.Info("★ddlResult.SelectedValue:" & ddlResult.SelectedValue.ToString)
        'If 0 < ddlResult.Items.Count AndAlso Not String.IsNullOrWhiteSpace(ddlResult.SelectedIndex.ToString) Then
        '    'Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedValue.ToString)
        '    Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedIndex.ToString)
        '    Dim strDLR_CD As String = ResultList(SelectNo)("DLR_CD").ToString
        '    Dim strJOB_CD As String = ResultList(SelectNo)("JOB_CD").ToString
        '    Dim strVCL_KATASHIKI As String = ResultList(SelectNo)("VCL_KATASHIKI").ToString
        '    If Biz.IsPeriodicInspection(strDLR_CD, strVCL_KATASHIKI, strJOB_CD) Then
        '        SetResultDetail()
        '    End If
        '    'SetResultDetail()
        'End If
        '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑

        'RO番号をhdnRO_Numに入れておく
        hdnRO_NUM.Value = Params.R_O

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

    ''' <summary>
    ''' ポストバック処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnPostBack()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} ProcMode:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , hdnProcMode.Value))

        Select Case hdnProcMode.Value
            Case ProcMode_PopUp
                '明細部のSuggestアイコンタップ

                '部位拡大画面表示
                ShowPopUp(hdnClickedListNo.Value)
                'ElseIf hdnProcMode.Value = ProcMode_SaveWK_ROPreview Then
                '    '一時ワークに変更された項目を保存する
                '    SetTB_W_REPAIR_SUGGESTION()
                '    'ROプレビュー表示
                '    ShowROPreview()
                'ElseIf hdnProcMode.Value = ProcMode_ROPreview Then
                '    'ROプレビュー表示
                '    ShowROPreview()

            Case ProcMode_Register, ProcMode_Cart
                '「Register」「Cart」ボタンタップ

                'DB登録処理
                ProcRegistData()

                '拡大画面の消去
                contentsMainonBoard.Style.Add("display", "none")
                popUp.Style.Add("display", "none")
                closeBtn.Style.Add("display", "none")
                popUpList.Style.Add("display", "none")

            Case ProcMode_PartsDetail
                '拡大画面の点検項目名タップ

                '部位説明（SC3250103）画面へ遷移
                ShowPartsDetail()

            Case ProcMode_PartsDetailWK
                '拡大画面の点検項目名タップ（一時保存あり）

                '一時ワークに変更された項目を保存する
                ' 保存処理
                'SetTB_W_REPAIR_SUGGESTION()
                Dim ret = SetTB_W_REPAIR_SUGGESTION()

                '【***完成検査_排他制御***】 start
                '排他チェックエラーの場合はダイアログを表示
                If ret = 98 Then
                    '排他チェックエラーメッセージの表示
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                    ShowMessageBox(Msg_Exclusion)
                    DispProc()
                    Exit Sub
                End If
                '【***完成検査_排他制御***】 end

                '更新エラーの場合はダイアログを表示
                If ret <> 1 And ret <> 99 Then
                    'DBエラー
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                    ShowMessageBox(MsgID_DBERR)
                Else
                    '部位説明（SC3250103）画面へ遷移
                    ShowPartsDetail()
                End If
            Case Else
                '上記以外によるポストバック

                '各部位の再表示
                Call AllPartRegenerate()

                '拡大画面の消去
                contentsMainonBoard.Style.Add("display", "none")
                popUp.Style.Add("display", "none")
                closeBtn.Style.Add("display", "none")
                popUpList.Style.Add("display", "none")

                'Registerボタンの有効/無効設定
                If Not String.IsNullOrWhiteSpace(hdnChangeFlg.Value) Then
                    If 0 < Integer.Parse(hdnChangeFlg.Value) Then
                        'holderFotter = DirectCast(Me.Master.FindControl("footer"), ContentPlaceHolder)
                        'DirectCast(holderFotter.FindControl("imgRegister"), HtmlGenericControl).Attributes.Add("class", Register_Enable)
                        imgRegister.Attributes.Add("class", Register_Enable)
                    End If
                End If

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 「Register」ボタン／「Cart」ボタンタップ時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcRegistData()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} ProcMode:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , hdnProcMode.Value))

        Dim grvListData As GridView
        'Dim dtListData As DataTable = CreateListDataColumns()
        Dim strSVC_CD As String
        Dim strINSPEC_ITEM_CD As String
        Dim strSUGGEST_ICON As String
        Dim SendData As New List(Of String()) 'Dim SendData As New List(Of GridViewRow)
        Dim DBSendData As New List(Of ArrayList)
        Dim DBList As ArrayList
        'Dim ChangeItemCode As String = String.Empty
        'Dim RegisterFlag As Boolean
        Dim ret As Integer

        ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 start

        For i As Integer = 0 To lstGridView.Count - 1
            'dtListData = New DataTable
            grvListData = lstGridView(i)
            strSVC_CD = DirectCast(holder.FindControl(String.Format("hdnSVC_CD0{0}", i + 1)), HiddenField).Value

            For j As Integer = 0 To grvListData.Rows.Count - 1
                '変更有のデータをDBに反映

                Dim SuggestInfo() As String = DirectCast(grvListData.Rows(j).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)

                '2014/06/02 レスポンス対策　START　↓↓↓

                'If DirectCast(grvListData.Rows(j).FindControl("hdnChangeFlag"), HiddenField).Value <> SUGGEST_CHANGE_FLAG_OFF Then
                If SuggestInfo(hdnChangeFlag) <> SUGGEST_CHANGE_FLAG_OFF Then

                    'strINSPEC_ITEM_CD = DirectCast(grvListData.Rows(j).Cells(0).FindControl("hdnINSPEC_ITEM_CD"), HiddenField).Value
                    'strSUGGEST_ICON = DirectCast(grvListData.Rows(j).FindControl("hdnSUGGEST_ICON"), HiddenField).Value

                    strINSPEC_ITEM_CD = SuggestInfo(hdnINSPEC_ITEM_CD)
                    strSUGGEST_ICON = SuggestInfo(hdnSUGGEST_ICON)

                    '2014/06/02 レスポンス対策　END　　↑↑↑

                    '2014/06/04 レスポンス対策（不要な処理のため削除）　START　↓↓↓
                    'For cn As Integer = 0 To SuggestNoList.Count - 1
                    '    If strSUGGEST_ICON = SuggestNoList(cn) Then
                    '        ChangeItemCode = cn.ToString
                    '        Exit For
                    '    End If
                    'Next
                    '2014/06/04 レスポンス対策　END　　↑↑↑

                    Select Case hdnProcMode.Value
                        Case ProcMode_Register
                            '実績データに登録済みかどうか確認する
                            'RegisterFlag = Biz.CanGetFromTB_T_REPAIR_SUGGESTION_RSLT( _
                            '                                                       Params.DealerCode _
                            '                                                       , Params.BranchCode _
                            '                                                       , staffInfo.Account _
                            '                                                       , Params.R_O _
                            '                                                       , strSVC_CD _
                            '                                                       , strINSPEC_ITEM_CD _
                            '                                                       , strSUGGEST_ICON _
                            '                                                       )

                            '実績データに更新内容がなければ、データベース更新データ及び、Webサービス送信データに追加する
                            'If RegisterFlag = False Then
                            'データベース更新用のデータリストに追加
                            DBList = New ArrayList
                            DBList.Add(staffInfo.DlrCD)
                            DBList.Add(staffInfo.BrnCD)
                            'DBList.Add(Params.DealerCode)
                            'DBList.Add(Params.BranchCode)
                            DBList.Add(staffInfo.Account)
                            '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
                            DBList.Add(Params.SAChipID)
                            'DBList.Add(Params.R_O)
                            '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
                            DBList.Add(strSVC_CD)
                            DBList.Add(strINSPEC_ITEM_CD)
                            DBList.Add(strSUGGEST_ICON)
                            DBSendData.Add(DBList)

                            'Webサービス用のデータリストに追加
                            'If ChangeItemCode <> "0" And ChangeItemCode <> "7" Then
                            'If CInt(strSUGGEST_ICON) < 5 Then
                            If Integer.Parse(strSUGGEST_ICON) < 6 Then
                                '2014/07/08　引数をGridViewRow→String()に変更　START　↓↓↓
                                SendData.Add(SuggestInfo)
                                'SendData.Add(grvListData.Rows(j))
                                '2014/07/08　引数をGridViewRow→String()に変更　END　　↑↑↑
                            End If
                            'End If

                            '更新処理はサービス正常終了まででコミット単位とする、行単位のコミット処理を削除
                            'Case ProcMode_Cart
                            '    '対象データを更新
                            '    'Logger.Info("★カート：データ更新" & i.ToString & ":" & j.ToString & ":" & DirectCast(grvListData.Rows(j).FindControl("hdnChangeFlag"), HiddenField).Value)
                            '    '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
                            '    'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06
                            '    'Biz.Set_TB_W_REPAIR_SUGGESTION_Process( _
                            '    ret = Biz.Set_TB_W_REPAIR_SUGGESTION_Process( _
                            '        staffInfo.DlrCD _
                            '        , staffInfo.BrnCD _
                            '        , staffInfo.Account _
                            '        , Params.SAChipID _
                            '        , strSVC_CD _
                            '        , strINSPEC_ITEM_CD _
                            '        , strSUGGEST_ICON _
                            '        )
                            'Biz.ShowCart( _
                            '    staffInfo.DlrCD _
                            '    , staffInfo.BrnCD _
                            '    , staffInfo.Account _
                            '    , Params.R_O _
                            '    , strSVC_CD _
                            '    , strINSPEC_ITEM_CD _
                            '    , strSUGGEST_ICON _
                            '    )
                            '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
                            'Biz.ShowCart( _
                            '    Params.DealerCode _
                            '    , Params.BranchCode _
                            '    , staffInfo.Account _
                            '    , Params.R_O _
                            '    , strSVC_CD _
                            '    , strINSPEC_ITEM_CD _
                            '    , strSUGGEST_ICON _
                            '    )



                            'Webサービス用のデータリストに追加
                            'SendData.Add(grvListData.Rows(j))
                    End Select
                End If
            Next
        Next

        'Registerボタンが押されたとき、更新された情報があればデータベース更新及びWebサービス送信を実行する
        If (0 < SendData.Count Or 0 < DBSendData.Count) And hdnProcMode.Value = ProcMode_Register Then
            Dim RetCode As String

            '【***完成検査_排他制御***】 start
            Dim exclusionResult As Boolean = True
            '排他チェック
            exclusionResult = Biz.CheckUpdateRepairSuggestion(Long.Parse(rowLockvs.Value), Params.DealerCode, Params.BranchCode, Params.R_O)
            If exclusionResult = False Then
                '排他チェックエラーメッセージを表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            'DB更新実行
            ret = Biz.RegisterAndDeleteWork(DBSendData)
            If ret = 1 Then

                'Dim dt As SC3250101DataSet.ServiceItemsResultDataTable
                'Dim RetCode As String
                'DB更新完了のため、Webサービス送信実行
                If 0 < SendData.Count Then

                    RetCode = BizSrv.CallGetServiceItemsWebService(CreateXMLOfRegister(SendData, SC3250101WebServiceClassBusinessLogic.GetServiceItems_Info.WebServiceIDValue))

                    '2014/06/30 サービス送信後のチェック処理追加　START　↓↓↓
                    'Dim resultId = dt.Rows(0).Item("ResultId")

                    'テスト用
                    'RetCode = ServiceSuccess

                    'サービスがエラーで返ってきた場合
                    If RetCode <> ServiceSuccess Then
                        '（システム管理者対応後、再送信できるように）レジスターボタンを活性化
                        'holderFotter = DirectCast(Me.Master.FindControl("footer"), ContentPlaceHolder)
                        'DirectCast(holderFotter.FindControl("imgRegister"), HtmlGenericControl).Attributes.Add("class", Register_Enable)
                        imgRegister.Attributes.Add("class", Register_Enable)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                        ShowMessageBox(WORD_SERVICE_ERR)

                        'すべての点検項目変更フラグを0に戻す
                        'AllChangeFlagToZero(lstGridView)
                        '各部位の再表示
                        Call AllPartRegenerate()
                        Exit Sub
                    End If
                    '2014/06/30 サービス送信後のチェック処理追加　END　　↑↑↑

                End If

                'hdnAlreadySendFlagフラグを「1」にする
                hdnAlreadySendFlag.Value = "1"

                'すべての点検項目変更フラグを0に戻す
                Call AllChangeFlagToZero()
                'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 start
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
                'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 end
            End If
        End If

        'カートボタンが押されたとき、カートページへ遷移する
        If hdnProcMode.Value = ProcMode_Cart Then
            ' 保存処理
            ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            Else
                '変換後の販売店コード、店舗コードをログ出力
                Logger.Info("ShowCart:DmsDealerCode:" & DmsDealerCode)
                Logger.Info("ShowCart:DmsBranchCode:" & DmsBranchCode)
                Logger.Info("ShowCart:DmsLoginUserID:" & DmsLoginUserID)

                '販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, DmsDealerCode)
                '店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, DmsBranchCode)
                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, DmsLoginUserID)
                '来店者実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
                'RO
                Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
                'RO_JOB_SEQ
                Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
                'ViewMode
                Me.SetValue(ScreenPos.Next, SessionParam09, Params.ViewMode)
                'DISP_NUM
                Me.SetValue(ScreenPos.Next, SessionDispNum, "17")

                '基幹画面連携用フレーム呼出処理
                Me.ScreenTransition()
            End If
        End If

        'Registerボタンを押したらCartボタンを有効にする
        'holderFotter = DirectCast(Me.Master.FindControl("footer"), ContentPlaceHolder)
        If hdnProcMode.Value = ProcMode_Register And ret = 1 Then
            'DirectCast(holderFotter.FindControl("imgCart"), HtmlGenericControl).Attributes.Add("class", Cart_Enable)
            'DirectCast(holderFotter.FindControl("imgRegister"), HtmlGenericControl).Attributes.Add("class", Register_Disable)
            imgCart.Attributes.Add("class", Cart_Enable)
            imgRegister.Attributes.Add("class", Register_Disable)
        End If

        '各部位の再表示
        Call AllPartRegenerate()

        ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 end

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "固有ヘッダエリア関連"

    ''' <summary>
    ''' 固有ヘッダ部表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHeaderDisp()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        ''* ロゴ表示
        Dim dtModelInfo As New SC3250101DataSet.TB_M_MODELDataTable
        '2014/06/11 パラメータをVINに変更　START　↓↓↓
        'If String.IsNullOrEmpty(Params.R_O) Then
        If String.IsNullOrEmpty(Params.VIN_NO) Then
            '2014/06/11 パラメータをVINに変更　END　↑↑↑
            'VIN指定無しの場合は表示無し
            logoImage.InnerHtml = String.Empty
            ImageLogo.Visible = False
            strModelCode = String.Empty
            '2019/07/05　TKM要件:型式対応　START　↓↓↓
            strKatashiki = String.Empty
        Else
            'VINから車種情報を取り出す
            '2014/06/11 パラメータをVINに変更　START　↓↓↓
            dtModelInfo = Biz.GetModelInfo(Params.VIN_NO)
            'dtModelInfo = Biz.GetModelInfo(Params.R_O)
            '2014/06/11 パラメータをVINに変更　END　↑↑↑
            If dtModelInfo.Rows.Count = 0 Then
                '指定したモデルが見つからなかった場合は表示なし
                Logger.Info("GetModelInfo:[Nothing]")
                logoImage.InnerHtml = String.Empty
                ImageLogo.Visible = False
                strModelCode = String.Empty
                strKatashiki = String.Empty
            Else
                'モデルが見つかった
                Logger.Info(String.Format("MODEL_CD:[{0}], VCL_ID:[{1}]", dtModelInfo.Rows(0)("MODEL_CD").ToString, dtModelInfo.Rows(0)("VCL_ID").ToString))
                If Biz.IsTOYOTA(dtModelInfo.Rows(0)("MODEL_CD").ToString) Then
                    'TOYOTA車である場合
                    Dim LogoPicture As String = dtModelInfo.Rows(0)("LOGO_PICTURE").ToString
                    Logger.Info(String.Format("LogoFilePath, ResolveUrl:[{0}], MapPath[{1}]", ResolveUrl(LogoPicture), Server.MapPath(LogoPicture)))
                    '2014/03/28　MapPathによるファイル検出ができないため、MapPathによる検査を削除
                    'If Not String.IsNullOrWhiteSpace(LogoPicture) AndAlso IO.File.Exists(Server.MapPath(LogoPicture)) Then
                    If Not String.IsNullOrWhiteSpace(LogoPicture) Then
                        '車輌ロゴを表示
                        ImageLogo.ImageUrl = ResolveUrl(LogoPicture)
                        logoImage.InnerHtml = dtModelInfo.Rows(0)("MODEL_CD").ToString
                        'logoImage.Style.Add("background-image", ResolveUrl(LogoPicture))
                        Logger.Info(String.Format("ShowImageLogo:[{0}]", ResolveUrl(LogoPicture)))
                    Else
                        '車輌情報に指定されたイメージファイルが存在しない場合、モデルコードを表示
                        logoImage.InnerHtml = dtModelInfo.Rows(0)("MODEL_CD").ToString
                        ImageLogo.Visible = False
                        Logger.Info(String.Format("ShowModelCode:[{0}]", dtModelInfo.Rows(0)("MODEL_CD").ToString))
                    End If
                Else
                    'TOYOTA車でない場合は表示なし
                    logoImage.InnerHtml = String.Empty
                    ImageLogo.Visible = False
                End If
                strModelCode = dtModelInfo.Rows(0)("MODEL_CD").ToString
                strKatashiki = dtModelInfo.Rows(0)("VCL_KATASHIKI").ToString
            End If
        End If

        If String.IsNullOrEmpty(strKatashiki) Then
            Biz.SetUseFlgKatashiki(False)
        End If
        '2019/07/05　TKM要件:型式対応　END　↑↑↑

        '2014/05/30 オペレーション変換マスタ参照廃止　START　↓↓↓
        ''* 変換後のモデルコードを取得する
        'If Not String.IsNullOrWhiteSpace(strModelCode) Then
        '    strChangeModelCode = Biz.GetChangeModelCode(strModelCode)
        'End If
        '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
        'strChangeModelCode = strModelCode
        '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑
        '2014/05/30 オペレーション変換マスタ参照廃止　END　　↑↑↑

        '2014/06/12 グレード名→型式に変更　START　↓↓↓
        ''* グレード表示
        'Dim dtGradeInfo As New SC3250101DataSet.TB_M_GRADEDataTable
        'If dtModelInfo.Rows.Count = 0 OrElse String.IsNullOrEmpty(dtModelInfo.Rows(0)("MODEL_CD").ToString) Then
        '    '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
        '    'carGrade.InnerHtml = String.Empty
        '    carGrade.Text = String.Empty
        '    '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
        'Else
        '    'モデルコードからグレード情報を取得する
        '    dtGradeInfo = Biz.GetGradeInfo(dtModelInfo.Rows(0)("MODEL_CD").ToString)
        '    If dtGradeInfo.Rows.Count = 0 Then
        '        'グレード名が取得できなかったら表示なし
        '        '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
        '        'carGrade.InnerHtml = String.Empty
        '        carGrade.Text = String.Empty
        '        '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
        '        strGradeInfo = String.Empty
        '    Else
        '        'グレード名を表示する
        '        '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
        '        'carGrade.InnerHtml = dtGradeInfo.Rows(0)("GRADE_NAME").ToString
        '        carGrade.Text = dtGradeInfo.Rows(0)("GRADE_NAME").ToString
        '        '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
        '        strGradeInfo = dtGradeInfo.Rows(0)("GRADE_NAME").ToString
        '    End If
        'End If

        If logoImage.InnerHtml <> String.Empty Then
            '型式を表示
            If Not String.IsNullOrWhiteSpace(dtModelInfo.Rows(0)("VCL_KATASHIKI").ToString) Then
                carGrade.Text = dtModelInfo.Rows(0)("VCL_KATASHIKI").ToString
                strGradeInfo = dtModelInfo.Rows(0)("VCL_KATASHIKI").ToString
            Else
                carGrade.Text = String.Empty
                strGradeInfo = String.Empty
            End If
        End If
        '2014/06/12 グレード名→型式に変更　END　　↑↑↑

        '* Result表示
        'Dim dtResultData As SC3250101DataSet.ResultListDataTable = Biz.GetResultList(Params.VIN_NO)
        Dim item As New ListItem

        ddlResult.Items.Clear()
        '
        '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
        'If ImageLogo.Visible Then
        If 0 < ResultList.Count Then
            '(点検履歴あり)ResultListからひとつずつ取り出し、ドロップダウンリストに追加していく
            For i As Integer = 0 To ResultList.Count - 1
                Dim SvcinDatetime As String = ResultList(i)("RSLT_SVCIN_DATETIME").ToString
                '2014/05/21 「Result一覧」に商品名追加　START　↓↓↓
                'Dim MercName As String = ResultList(i)("MERC_NAME").ToString
                'Dim RegMile As String = ResultList(i)("REG_MILE").ToString
                '2014/05/21 「Result一覧」に商品名追加　　END　↑↑↑
                If Not String.IsNullOrWhiteSpace(SvcinDatetime) Then
                    SvcinDatetime = Format(CDate(SvcinDatetime), "yyyy/MM/dd").ToString
                Else
                    SvcinDatetime = ""
                End If

                '2014/05/21 「Result一覧」に商品名追加　START　↓↓↓
                'If Not String.IsNullOrWhiteSpace(RegMile) AndAlso 0 < CInt(RegMile) Then
                '    RegMile = Format(CInt(ResultList(i)("REG_MILE")), "#,#").ToString & "km"
                'Else
                '    RegMile = ""
                'End If
                '2014/05/21 「Result一覧」に商品名追加　　END　↑↑↑

                '2014/05/29 オペレーション変換マスタの使用廃止　START　↓↓↓
                '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
                item = New ListItem(String.Format("{0} {1}" _
                                                  , SvcinDatetime _
                                                  , ResultList(i)("MERC_NAMES").ToString _
                                                    ) _
                                                  , i.ToString)

                'item = New ListItem(String.Format("{0} {1} {2}" _
                '                                  , SvcinDatetime _
                '                                  , ResultList(i)("MERCHANDISENAME").ToString _
                '                                  , MercName _
                '                                    ) _
                '                                  , i.ToString)
                '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

                'item = New ListItem(String.Format("{0} {1} {2}" _
                '                                  , SvcinDatetime _
                '                                  , ResultList(i)("UPPER_DISP").ToString & ResultList(i)("LOWER_DISP").ToString _
                '                                  , RegMile _
                '                                    ) _
                '                                  , i.ToString)
                '2014/05/29 オペレーション変換マスタの使用廃止　END　　↑↑↑
                ddlResult.Items.Add(item)
            Next
            'ドロップダウンリストの一番最後の項目を表示させる
            ddlResult.SelectedIndex = ddlResult.Items.Count - 1

            ''* Suggest表示
            'InspecType.RESULTからInspecType.SUGGESTを導き出す
            'If 0 < ResultList.Count Then
            '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
            'txtSuggest.Text = String.Format("Every {0} Inspection (M)", InspecType.SUGGEST)
            '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑
            'End If
        Else
            '(点検履歴なし)INSPEC_TYPEを初期化する
            InspecType.RESULT = String.Empty
            '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
            '2019/07/05　TKM要件:型式対応　START　↓↓↓
            InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, strKatashiki, FIRST_INSPEC_TYPE, DefaultModelCode)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            'InspecType.SUGGEST = Biz.GetNextInspecType(staffInfo.DlrCD, staffInfo.BrnCD, strModelCode, FIRST_INSPEC_TYPE, CAMRY)
            '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑
            '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
            'txtSuggest.Text = String.Format("Every {0} Inspection (M)", InspecType.SUGGEST)
            '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑
        End If
        '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑

        'If dtModelInfo.Rows.Count > 0 And dtGradeInfo.Rows.Count > 0 Then
        '    txtSuggest.Text = Biz.GetNextSuggest(dtModelInfo.Rows(0)("MODEL_CD"), dtGradeInfo.Rows(0)("GRADE_CD"), staffInfo)
        'End If

        ' ''アドバイス表示
        '2014/06/13 ROステータスによってアドバイス表示を変更、HTMLエンコード追加　START　↓↓↓
        If (isRoActive And ResultList.Count > 0 AndAlso ResultList(ddlResult.SelectedIndex)("RO_NUM").ToString = Params.R_O) And (ROStatus = RO_COMPLETE Or ROStatus = RO_AFTER_ADD_WK_MAKE) Then
            'ROステータス＝CloseJob後、TC追加作業起票後　→　今回のアドバイスを表示
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            Dim dtAServiceInInfo As SC3250101DataSet.TB_T_SERVICEINDataTable = Biz.GetServiceIn(staffInfo.DlrCD, staffInfo.BrnCD, Params.R_O)
            'Dim dtAServiceInInfo As SC3250101DataSet.TB_T_SERVICEINDataTable = Biz.GetServiceIn(Params.R_O)
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑
            If dtAServiceInInfo IsNot Nothing AndAlso 0 < dtAServiceInInfo.Rows.Count Then
                'RepairAdvice.InnerHtml = dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString
                'AdditionWorkAdvice.InnerHtml = dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString

                '2014/09/03　アドバイスの改行対応　START　↓↓↓
                'RepairAdvice.InnerHtml = Server.HtmlEncode(dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString)
                'AdditionWorkAdvice.InnerHtml = Server.HtmlEncode(dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString)
                RepairAdvice.InnerHtml = ChengeValueEscape(dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString)
                AdditionWorkAdvice.InnerHtml = ChengeValueEscape(dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString)
                '2014/09/03　アドバイスの改行対応　END　　↑↑↑

            Else
                RepairAdvice.InnerHtml = ""
                AdditionWorkAdvice.InnerHtml = ""
            End If
        Else
            'ROステータス＝上記以外　→　前回のアドバイスを表示
            If 0 <= ddlResult.SelectedIndex Then
                '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
                Dim dtAServiceInInfo As SC3250101DataSet.TB_T_SERVICEINDataTable = Biz.GetServiceIn(ResultList(ddlResult.SelectedIndex)("DLR_CD").ToString _
                                                                                                    , ResultList(ddlResult.SelectedIndex)("BRN_CD").ToString _
                                                                                                    , ResultList(ddlResult.SelectedIndex)("RO_NUM").ToString)
                'Dim dtAServiceInInfo As SC3250101DataSet.TB_T_SERVICEINDataTable = Biz.GetServiceIn(ResultList(ddlResult.SelectedIndex)("RO_NUM").ToString)
                '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑
                If dtAServiceInInfo IsNot Nothing AndAlso 0 < dtAServiceInInfo.Rows.Count Then
                    'RepairAdvice.InnerHtml = dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString
                    'AdditionWorkAdvice.InnerHtml = dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString

                    '2014/09/03　アドバイスの改行対応　START　↓↓↓
                    'RepairAdvice.InnerHtml = Server.HtmlEncode(dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString)
                    'AdditionWorkAdvice.InnerHtml = Server.HtmlEncode(dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString)
                    RepairAdvice.InnerHtml = ChengeValueEscape(dtAServiceInInfo.Rows(0)("ADD_JOB_ADVICE").ToString)
                    AdditionWorkAdvice.InnerHtml = ChengeValueEscape(dtAServiceInInfo.Rows(0)("NEXT_SVCIN_INSPECTION_ADVICE").ToString)
                    '2014/09/03　アドバイスの改行対応　END　　↑↑↑

                Else
                    RepairAdvice.InnerHtml = ""
                    AdditionWorkAdvice.InnerHtml = ""
                End If

            End If
        End If
        '2014/06/13 ROステータスによってアドバイス表示を変更、HTMLエンコード追加　END　　↑↑↑

        '* マイレージ表示
        Dim sendxml_Mileage As Request_MileageXmlDocumentClass = CreateXMLOfMileage(SC3250101WebServiceClassBusinessLogic.GetMileage_Info.WebServiceIDValue)
        Dim intMile As Integer

        '2014/06/13 ROステータスの定数が重複していたため修正　RO_CLOSE　→　RO_COMPLETE
        If ROStatus = RO_COMPLETE Then
            'R/Oステータス　クローズ時、入庫履歴から取得する
            Dim RstNo As Integer = ResultList.Count - 1
            If 0 <= RstNo Then
                If Not String.IsNullOrWhiteSpace(ResultList(RstNo)("SVCIN_MILE").ToString) Then
                    intMile = Integer.Parse(ResultList(RstNo)("SVCIN_MILE").ToString)
                    If intMile = 0 Then
                        '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
                        'carMileage.InnerHtml = "0km"
                        carMileage.Text = "0km"
                        '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
                    Else
                        '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
                        'carMileage.InnerHtml = Format(intMile, "#,#").ToString & "km"
                        carMileage.Text = Format(intMile, "#,#").ToString & "km"
                        '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
                    End If
                End If
            End If
        Else
            'R/Oステータス　クローズ以外はサービスによりDMSの値を取得する
            If String.IsNullOrEmpty(Params.SAChipID) Or String.IsNullOrEmpty(Params.VIN_NO) Then
            Else
                Dim retxml_Mileage As SC3250101DataSet.MileageDataTable = BizSrv.CallGetMileageWebService(sendxml_Mileage)
                If retxml_Mileage IsNot Nothing AndAlso 0 < retxml_Mileage.Rows.Count Then
                    If Not String.IsNullOrWhiteSpace(retxml_Mileage.Rows(0)("Mileage").ToString) Then
                        intMile = Integer.Parse(retxml_Mileage.Rows(0)("Mileage").ToString)
                        If intMile = 0 Then
                            '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
                            'carMileage.InnerHtml = "0km"
                            carMileage.Text = "0km"
                            '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
                        ElseIf 0 < intMile Then
                            '2014/06/11 グレードと走行距離をカスタムラベルに変更　START　↓↓↓
                            carMileage.Text = Format(intMile, "#,#").ToString & "km"
                            'carMileage.InnerHtml = Format(intMile, "#,#").ToString & "km"
                            '2014/06/11 グレードと走行距離をカスタムラベルに変更　END　　↑↑↑
                        End If
                    End If
                End If
                retxml_Mileage.Dispose()
            End If
        End If


        '* 写真枚数表示
        ''2014/07/09　ROが空の時は写真枚数を取得しないように変更　START　↓↓↓
        'If Not String.IsNullOrWhiteSpace(Params.R_O) Then
        Dim sendxml_RoThumbnailCount As RoThumbnailCountXmlDocumentClass = CreateXMLOfRoThumbnailCount(SC3250101WebServiceClassBusinessLogic.GetRoThumbnailCount_Info.WebServiceIDValue)

        Dim retxml_RoThumbnailCount As SC3250101DataSet.RoThumbnailCountDataTable = BizSrv.CallGetRoThumbnailCountWebService(sendxml_RoThumbnailCount)
        If retxml_RoThumbnailCount IsNot Nothing AndAlso 0 < retxml_RoThumbnailCount.Rows.Count Then
            If String.IsNullOrEmpty(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) = False Then
                '2014/06/11 応答XMLの戻り値解析追加　START　↓↓↓
                If 0 < Integer.Parse(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) Then
                    bottomImg.InnerHtml = retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString
                End If
                '2014/06/11 応答XMLの戻り値解析追加　END　　↑↑↑
            End If
        End If

        '* カメラアイコンにURLをセット
        'Dim envSettingRow As String = String.Empty
        'Using biz As New ServiceCommonClassBusinessLogic
        '    envSettingRow = biz.GetDlrSystemSettingValueBySettingName(GetServiceItems_Info.WebServiceURL)
        'End Using

        '2014/07/09　カメラポップアップ表示処理変更（SC3170209のjsを参考）　START　↓↓↓
        '* カメラアイコンにURLをセット
        'Dim cameraUrl As String = String.Empty
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
        'Using biz As New ServiceCommonClassBusinessLogic
        'cameraUrl = Biz.GetDlrSystemSettingValueBySettingName("URL_DISP_IMG")

        '写真選択ポップアップ画面を表示するセッションを作成（URLはJavaScriptにて作成）
        Dim Target As StringBuilder = New StringBuilder
        'Dim strUrl As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf("/SC3250101"))

        With Target
            '.Append(strUrl.Substring(0, strUrl.LastIndexOf("/"c)))
            '.Append("/SC3170210.aspx")
            .AppendFormat("?DealerCode={0}", DmsDealerCode)
            .AppendFormat("&BranchCode={0}", DmsBranchCode)
            .AppendFormat("&SAChipID={0}", Params.SAChipID)
            .AppendFormat("&BASREZID={0}", Params.BASREZID)
            .AppendFormat("&R_O={0}", Params.R_O)
            '2014/08/21 仕様変更対応：『全部表示の場合、固定で「0」を渡す』　START　↓↓↓
            '.AppendFormat("&SEQ_NO={0}", Params.SEQ_NO)
            .Append("&SEQ_NO=0")
            '2014/08/21 仕様変更対応：『全部表示の場合、固定で「0」を渡す』　END　　↑↑↑
            .AppendFormat("&VIN_NO={0}", Params.VIN_NO)
            .Append("&PictMode=1")
            .Append("&ViewMode=0")
            .Append("&LinkSysType=0")
            .AppendFormat("&LoginUserID={0}", staffInfo.Account)
        End With

        Dim cameraUrl As String = Target.ToString

        Logger.Info(String.Format("CameraURL_Session:[{0}]", cameraUrl))

        If retxml_RoThumbnailCount IsNot Nothing AndAlso 0 < retxml_RoThumbnailCount.Rows.Count Then
            If String.IsNullOrEmpty(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) = False Then
                If 0 < Integer.Parse(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) Then
                    'btnCamera.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", cameraUrl))
                    btnCamera.Attributes.Add("onclick", String.Format("ShowUrlSchemeNoTitlePopup('{0}');", cameraUrl))
                End If
            End If
        End If
        
    '【***カメラアイコン非表示対応***】 start
        If String.IsNullOrWhiteSpace(Params.R_O) And String.IsNullOrWhiteSpace(Params.SAChipID) Then
            btnCamera.Visible = False
            bottomImg.Visible = False
        End If
    '【***カメラアイコン非表示対応***】 end

        'End Using
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑
        '2014/07/09　カメラポップアップ表示処理変更（SC3170209のjsを参考）　END　↑↑↑
        'End If
        ''2014/07/09　ROが空の時は写真枚数を取得しないように変更　END　　↑↑↑

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    '2014/09/03　アドバイスの改行対応　START　↓↓↓
    ''' <summary>
    ''' エスケープ処理
    ''' </summary>
    ''' <param name="advice">アドバイス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChengeValueEscape(ByVal advice As String) As String
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim wkAdvice As String = String.Empty

        wkAdvice = Replace(Server.HtmlEncode(advice), "\", "\\")
        wkAdvice = Replace(wkAdvice, """", "\""")
        wkAdvice = Replace(wkAdvice, vbCrLf, "<br/>")
        Return wkAdvice

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Function
    '2014/09/03　アドバイスの改行対応　END　　↑↑↑

#End Region

#Region "明細部エリア関連"

    '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
    ''' <summary>
    ''' SuggestとResultのアイコンを表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetResultAndSuggestDetail()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '**** 一括で全部位の検査項目を取得する
        Dim dtListData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        'SuggestList取得時に型式で取れない場合この時点で既に'BizでSetUseFlgKatashiki(False)'が呼び出されているのでモデルのみの検索となる
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
        dtListData = Biz.GetInspectionList(strModelCode, _
                                        strKatashiki, _
                                        staffInfo.DlrCD, _
                                        staffInfo.BrnCD, _
                                        staffInfo.Account, _
                                        Params.SAChipID, _
                                        InspecType.SUGGEST, _
                                        DefaultModelCode)
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
        'dtListData = Biz.GetInspectionList(strModelCode, _
        '                                strGradeInfo, _
        '                                staffInfo.DlrCD, _
        '                                staffInfo.BrnCD, _
        '                                staffInfo.Account, _
        '                                Params.SAChipID, _
        '                                InspecType.SUGGEST, _
        '                                CAMRY)

        '指定したモデルで点検マスタが取得出来ているかチェックする
        Dim dtListDataRow() As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow
        dtListDataRow = DirectCast(dtListData.Select("REQ_ITEM_DISP_SEQ IS NOT NULL"), SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow())

        'If strModelCode <> CAMRY And dtListDataRow.Count = 0 Then
        If dtListDataRow.Count = 0 And Biz.GetUseFlgKatashiki() Then
            'この時点で既に'BizでSetUseFlgKatashiki(False)'が呼び出されているので取り直しすればモデルのみの検索となる
            '型式で検索したときに点検マスタが0件の時、モデルコードで再度、点検項目を取得する
            Biz.SetUseFlgKatashiki(False)
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
            dtListData = Biz.GetInspectionList(strModelCode, _
                                                  strKatashiki, _
                                                  staffInfo.DlrCD, _
                                                  staffInfo.BrnCD, _
                                                  staffInfo.Account, _
                                                  Params.SAChipID, _
                                                  InspecType.SUGGEST, _
                                                  DefaultModelCode)
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
        End If
        If (strModelCode <> DefaultModelCode) And dtListDataRow.Count = 0 Then
            'デフォルトモデルコード以外のモデルコードで点検マスタが0件の時、モデルコードを「デフォルトモデルコード」にして再度、点検項目を取得する
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
            dtListData = Biz.GetInspectionList(DefaultModelCode, _
                                strKatashiki, _
                                staffInfo.DlrCD, _
                                staffInfo.BrnCD, _
                                staffInfo.Account, _
                                Params.SAChipID, _
                                InspecType.SUGGEST, _
                                DefaultModelCode)
            '2020/02/14 TKM要件：型式対応 GRADE_CD  strGradeInfo, _ 廃止 End
        End If


        '2019/07/05　TKM要件:型式対応　END　↑↑↑


        If dtListData.Count <= 0 Then
            'カムリでも点検マスタを取得できなかったときはエラーメッセージを表示する
            Logger.Error(WebWordUtility.GetWord(WORD_NO_DATA))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
            ShowMessageBox(WORD_NO_DATA)
        End If

        '**** 点検項目とSuggest欄の項目を設定する
        '明細部を表示する
        CreateAllList(dtListData)
        dtListData.Dispose()

        '**** 過去の実績（Result）を表示する
        ' Result欄の項目をすべてNone「-」にする
        Me.ResetResultData()

        If 0 < ddlResult.Items.Count AndAlso Not String.IsNullOrWhiteSpace(ddlResult.SelectedIndex.ToString) Then
            'Resutlリストから販売店コード、作業コード、型式を取得する
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
            'Dim SelectNo As Integer = ddlResult.SelectedIndex
            'Dim strDLR_CD As String = ResultList(SelectNo)("DLR_CD").ToString
            'Dim strJOB_CD As String = ResultList(SelectNo)("JOB_CD").ToString
            'Dim strVCL_KATASHIKI As String = ResultList(SelectNo)("VCL_KATASHIKI").ToString
            InspecType.RESULT = ResultList(ddlResult.SelectedIndex)("MERCHANDISENAME").ToString

            'Resultリストボックスで選択したものが定期点検かチェックする
            '定期点検フラグの合計数が1以上ならば定期点検と判断する
            'If Biz.IsPeriodicInspection(strDLR_CD, strVCL_KATASHIKI, strJOB_CD) Then
            If 0 < ResultList(ddlResult.SelectedIndex).SERVICE Then
                '定期点検ならば、各点検結果をResult欄に表示する
                SetResultDetail()
            End If
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
    '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑

    ''' <summary>
    ''' すべての点検項目変更フラグを0に戻す
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AllChangeFlagToZero()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'Dim grvListData As GridView
        For i As Integer = 1 To GRIDVIEW_NUMBER
            'grvListData = lstGridView(i - 1)
            For Each grvListRow As GridViewRow In lstGridView(i - 1).Rows

                '2014/06/02 レスポンス対策　START　↓↓↓

                'If Not String.IsNullOrWhiteSpace(DirectCast(grvListRow.FindControl("hdnChangeFlag"), HiddenField).Value) Then
                '    DirectCast(grvListRow.FindControl("hdnChangeFlag"), HiddenField).Value = "0"
                'End If

                Dim SuggestInfo() As String = DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
                '2014/06/12 Registerボタン活性・非活性処理修正　START　↓↓↓
                If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnChangeFlag)) Then
                    DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value = String.Format("{0},{1},{2},{3},{4},{5}" _
                                                                                                           , SuggestInfo(hdnINSPEC_ITEM_CD) _
                                                                                                           , SuggestInfo(hdnSUGGEST_ICON) _
                                                                                                           , SuggestInfo(hdnSUGGEST_STATUS) _
                                                                                                           , SUGGEST_CHANGE_FLAG_OFF _
                                                                                                           , SuggestInfo(DEFAULT_STATUS) _
                                                                                                           , SuggestInfo(hdnSUGGEST_ICON))
                    'DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value = String.Format("{0},{1},{2},{3},{4},{5}" _
                    '                                                                                       , SuggestInfo(hdnINSPEC_ITEM_CD) _
                    '                                                                                       , SuggestInfo(hdnSUGGEST_ICON) _
                    '                                                                                       , SuggestInfo(hdnSUGGEST_STATUS) _
                    '                                                                                       , SUGGEST_CHANGE_FLAG_OFF _
                    '                                                                                       , SuggestInfo(DEFAULT_STATUS) _
                    '                                                                                       , SuggestInfo(BEFORE_STATUS))
                End If

                hdnChangeFlg.Value = "0"
                '2014/06/12 Registerボタン活性・非活性処理修正　END　　↑↑↑
                '2014/06/02 レスポンス対策　END　　↑↑↑

            Next
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#Region "点検項目／Suggest表示関連"

    '2014/05/29 レスポンス対策　START　↓↓↓
    ''' <summary>
    ''' 点検項目とSuggest欄を作成・表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateAllList(ByVal dtListTable As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} dtListTable_Count:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dtListTable.Rows.Count.ToString))

        '--ヘッダー作成
        CreateAllListHeader()

        '--データ部作成

        For Each PartName As String In PartNames
            Dim dtListData As DataTable = GetListData(PartName, dtListTable)
            If dtListData.Rows.Count = 0 Then
                Continue For
            End If

            '点検内容を表示する
            ShowListData(dtListData, PartName)
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 明細部の部位テーブルのヘッダー部を生成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateAllListHeader()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim dt As DataTable = CreatetListHeaderColumns()
        Dim dtSC3250101 As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        dtSC3250101 = Biz.GetAllPartInfo(PartNames)

        For Each PartName As String In PartNames
            Dim dr As DataRow = dt.NewRow()
            'Dim strListName As String = ""

            dr("ListNo") = PartName
            dr("ImageUrl") = dicPartInfo(PartName)("ImageUrl")
            dicPartInfo(PartName)("SVC_CD") = InspecType.SUGGEST
            dr("SVC_CD") = dicPartInfo(PartName)("SVC_CD")
            '車両判明時の設定
            If (ImageLogo.Visible) Then
                '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
                dr("Result") = InspecType.RESULT
                dr("Suggest") = InspecType.SUGGEST_DISP
                'dr("Result") = String.Format("Result<br/>{0}", InspecType.RESULT)
                'dr("Suggest") = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
            Else '車両不明時の設定
                dr("Result") = sUncertain
                dr("Suggest") = InspecType.SUGGEST_DISP

                'dr("Result") = String.Format("Result<br/>{0}", sUncertain)
                ''2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
                ''dr("Suggest") = String.Format("Suggest<br/>{0}", sSpace)
                'dr("Suggest") = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
                ''2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
                '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
            End If
            If dtSC3250101 IsNot Nothing Then
                Dim rows() As SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTRow = DirectCast(dtSC3250101.Select(String.Format(" PART_CD = '{0}'", PartName)), SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTRow())
                If rows.Count > 0 Then
                    '2014/05/22 文言DBから取得　START　↓↓↓
                    'dr("title") = dtSC3250101.Rows(0)("PART_NAME").ToString
                    'dr("title") = rows(0)("PART_NAME_NO").ToString
                    'dr("title") = WebWordUtility.GetWord(Integer.Parse(rows(0)("PART_NAME_NO").ToString))
                    dr("title") = WebWordUtility.GetWord(rows(0).PART_NAME_NO)
                    '2014/05/22 文言DBから取得　END　　↑↑↑

                    dr("POPUP_URL") = rows(0)("POPUP_URL").ToString
                End If
            End If
            ShowAllListHeader(dr)
        Next
        dtSC3250101.Dispose()
        dt.Dispose()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
    '2014/05/29 レスポンス対策　END　　↑↑↑

    ''' <summary>
    ''' 明細部の部位テーブルのヘッダー部のカラム生成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatetListHeaderColumns() As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("ListNo", GetType(String)))
        dt.Columns.Add(New DataColumn("ImageUrl", GetType(String)))
        dt.Columns.Add(New DataColumn("SVC_CD", GetType(String)))
        dt.Columns.Add(New DataColumn("title", GetType(String)))
        dt.Columns.Add(New DataColumn("POPUP_URL", GetType(String)))
        dt.Columns.Add(New DataColumn("Result", GetType(String)))
        dt.Columns.Add(New DataColumn("Suggest", GetType(String)))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return dt

    End Function

    ''' <summary>
    ''' 部位テーブルのヘッダー部に表示内容をセット
    ''' </summary>
    ''' <param name="dttListHeaderDataRow"></param>
    ''' <remarks></remarks>
    Private Sub ShowAllListHeader(ByVal dttListHeaderDataRow As DataRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '配置位置を変換する
        '2014/06/04 配置位置をCSSで対応　START　↓↓↓
        Dim SetNo As String = dttListHeaderDataRow("ListNo").ToString
        'Dim SetNo As String = "01"
        'SetNo = ChangeListNo(dttListHeaderDataRow("ListNo").ToString)
        '2014/06/04 配置位置をCSSで対応　END　　↑↑↑

        '部位のイメージをセット
        DirectCast(holder.FindControl(String.Format("TitleImage{0}", SetNo)), HtmlGenericControl).Attributes.Add("class", dttListHeaderDataRow("ImageUrl").ToString)


        '部位名に合わせて<BR>をつける
        Dim Title As String = dttListHeaderDataRow("title").ToString
        If 0 <= Title.IndexOf("Battery") Then
            If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
                Title = Title.Insert(Title.IndexOf("Battery"), "<br/>")
            End If
        End If

        If 0 <= Title.IndexOf("System") Then
            If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
                Title = Title.Insert(Title.IndexOf("System"), "<br/>")
            End If
        End If

        If 0 <= Title.IndexOf("Transmission") Then
            If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
                Title = Title.Insert(Title.IndexOf("Transmission"), "<br/>")
            End If
        End If

        '部位名をセット
        DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).Text = Title
        '1行表示 or 2行表示のクラスをセット
        'Dim strSearchChar As String = "<br/>"
        If Title.IndexOf("<br/>") = -1 And Title.IndexOf("<br>") = -1 Then
            DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_ONE_LINE
        Else
            DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_TWO_LINE
        End If

        '点検種別をセット
        DirectCast(holder.FindControl(String.Format("hdnSVC_CD{0}", SetNo)), HiddenField).Value = dttListHeaderDataRow("SVC_CD").ToString
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
        DirectCast(holder.FindControl(String.Format("List{0}_WordResult", SetNo)), Label).Text = WordResult
        DirectCast(holder.FindControl(String.Format("List{0}_Result", SetNo)), Label).Text = dttListHeaderDataRow("Result").ToString
        DirectCast(holder.FindControl(String.Format("List{0}_WordSuggest", SetNo)), Label).Text = WordSuggest
        DirectCast(holder.FindControl(String.Format("List{0}_Suggest", SetNo)), Label).Text = dttListHeaderDataRow("Suggest").ToString
        'DirectCast(holder.FindControl(String.Format("List{0}_Col2", SetNo)), HtmlGenericControl).InnerHtml = dttListHeaderDataRow("Result").ToString
        'DirectCast(holder.FindControl(String.Format("List{0}_Col3", SetNo)), HtmlGenericControl).InnerHtml = dttListHeaderDataRow("Suggest").ToString
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
        DirectCast(holder.FindControl(String.Format("List{0}_PartName", SetNo)), HtmlTableCell).Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", dttListHeaderDataRow("POPUP_URL").ToString))

        Logger.Info(String.Format("PopupURL:[{0}]", dttListHeaderDataRow("POPUP_URL").ToString))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    '2014/05/29 レスポンス対策　START　↓↓↓
    ''' <summary>
    ''' 明細部の部位テーブルのデータ部を生成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetListData(ByVal strListNo As String _
                       , ByVal dtListData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable
                       ) As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} strListNo:[{3}] dtListData(Count):[{4}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strListNo _
                  , dtListData.Rows.Count.ToString))

        '全ての点検項目テーブルより指定した部位番号の点検項目のみ取り出す

        Dim rows() As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow = DirectCast(dtListData.Select(String.Format("REQ_PART_CD = '{0}'", strListNo)), SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow())

        Dim dt As DataTable = CreateListDataColumns()
        Dim dr As DataRow
        Dim GroupTitle As String = Nothing
        Dim ListIndex As Integer = 1
        Dim ChangeFlag As String

        For Each row As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow In rows
            '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
            If String.IsNullOrWhiteSpace(row("REQ_ITEM_DISP_SEQ").ToString) _
                And String.IsNullOrWhiteSpace(row("R_SUGGEST_ICON").ToString) _
                And String.IsNullOrWhiteSpace(row("W_SUGGEST_ICON").ToString) Then
                'SQL実行時にパラメータで指定したモデルコード以外は「REQ_ITEM_DISP_SEQ（並び順）」を空白で取得
                '「REQ_ITEM_DISP_SEQ（並び順）」「R_SUGGEST_ICON（登録実績）」「W_SUGGEST_ICON（一時保存）」がすべて空白なら訴求画面に表示させない
                Continue For
            End If
            '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑

            dr = dt.NewRow()
            dr("ListNo") = strListNo
            If row.INSPEC_ITEM_NAME <> row.SUB_INSPEC_ITEM_NAME Then
                'グループ名とサブ名が違う
                If GroupTitle <> row.INSPEC_ITEM_NAME Then
                    GroupTitle = row.INSPEC_ITEM_NAME
                    '新しいグループ
                    dr("ItemName1") = row.INSPEC_ITEM_NAME
                    dr("ItemName2") = String.Empty
                    dr("Result") = String.Empty
                    dr("ResultImage") = String.Empty
                    'dr("SUGGEST_ICON") = String.Empty
                    'dr("INSPEC_ITEM_CD") = String.Empty
                    dr("ListIndex") = ListIndex.ToString
                    'dr("ChangeFlag") = SUGGEST_CHANGE_FLAG_OFF
                    dr("SuggestInfo") = String.Format("{0},{1},{2},{3},{4},{5}" _
                                                      , String.Empty _
                                                      , String.Empty _
                                                      , String.Empty _
                                                      , SUGGEST_CHANGE_FLAG_OFF _
                                                      , String.Empty _
                                                      , String.Empty)
                    ListIndex += 1
                    dt.Rows.Add(dr)
                    'サブグループの作成
                    dr = dt.NewRow()
                    dr("ListNo") = strListNo
                    dr("ListIndex") = String.Empty
                End If
                dr("ItemName1") = row.INSPEC_ITEM_NAME
                dr("ItemName2") = row.SUB_INSPEC_ITEM_NAME
            Else
                'グループ名とサブ名が同じ
                If GroupTitle <> row.INSPEC_ITEM_NAME Then
                    GroupTitle = row.INSPEC_ITEM_NAME
                    dr("ItemName1") = row.INSPEC_ITEM_NAME
                    dr("ItemName2") = String.Empty
                    dr("ListIndex") = ListIndex.ToString
                    ListIndex += 1
                Else
                    'グループ名とサブ名が同じでも現在のグループ名と同じならばインデックスをつけない
                    dr("ItemName1") = row.INSPEC_ITEM_NAME
                    dr("ItemName2") = row.SUB_INSPEC_ITEM_NAME
                End If
            End If

            ChangeFlag = SUGGEST_CHANGE_FLAG_OFF
            Dim SUGGEST_ICON As String
            Dim SUGGEST_STATUS As String

            '2014/06/17　点検項目が重複する可能性がある内容を修正　START　↓↓↓
            '初期表示アイコンを特定する
            Dim S_ReqItemCd As String = String.Empty
            Dim S_SvcCd As String = String.Empty
            Dim S_SuggestStatus As String = String.Empty
            If row("REQ_ITEM_CD_DEFAULT").ToString <> "" And row("SVC_CD_DEFAULT").ToString = InspecType.SUGGEST Then
                S_ReqItemCd = row("REQ_ITEM_CD_DEFAULT").ToString
                S_SvcCd = row("SVC_CD_DEFAULT").ToString
                S_SuggestStatus = row("SUGGEST_FLAG_DEFAULT").ToString
            End If
            If row("REQ_ITEM_CD").ToString <> "" And row("SVC_CD").ToString = InspecType.SUGGEST Then
                S_ReqItemCd = row("REQ_ITEM_CD").ToString
                S_SvcCd = row("SVC_CD").ToString
                S_SuggestStatus = row("SUGGEST_FLAG").ToString
            End If
            '2014/06/17　点検項目が重複する可能性がある内容を修正　END　　↑↑↑

            If row("W_SUGGEST_ICON").ToString <> "" And row("W_SVC_CD").ToString = InspecType.SUGGEST Then
                '一時ワークから読み込む
                If Integer.Parse(row("W_SUGGEST_ICON").ToString) < MAX_SUGGEST_ICON_NO Then
                    SUGGEST_ICON = row("W_SUGGEST_ICON").ToString
                    ChangeFlag = SUGGEST_CHANGE_FLAG_WKON
                    '一時ファイルに変更項目があれば「Register」ボタンを有効にする
                    'holderFotter = DirectCast(Me.Master.FindControl("footer"), ContentPlaceHolder)
                    'DirectCast(holderFotter.FindControl("imgRegister"), HtmlGenericControl).Attributes.Add("class", Register_Enable)
                    imgRegister.Attributes.Add("class", Register_Enable)
                    hdnChangeFlg.Value = "1"
                Else
                    '不明な表示アイテムコードが出てきた
                    Logger.Info(String.Format("Unknwon SUGGEST_ICON No:[{0}]", row("W_SUGGEST_ICON").ToString))
                    SUGGEST_ICON = DEFAULT_SUGGEST_ICON
                End If
            ElseIf row("R_SUGGEST_ICON").ToString <> "" And row("R_SVC_CD").ToString = InspecType.SUGGEST Then
                '実績データから読み込む
                If Integer.Parse(row("R_SUGGEST_ICON").ToString) < MAX_SUGGEST_ICON_NO Then
                    SUGGEST_ICON = row("R_SUGGEST_ICON").ToString
                    '2014/07/18　Suggestリスト変更時のメッセージ表示判断修正　START　↓↓↓
                    ''実績データに変更項目があれば、hdnAlreadySendFlagを「1」にする（編集モードの時のみ）
                    'If hdnViewMode.Value = "0" Then
                    '    hdnAlreadySendFlag.Value = "1"
                    'End If
                    '2014/07/18　Suggestリスト変更時のメッセージ表示判断修正　END　↑↑↑
                Else
                    '不明な表示アイテムコードが出てきた
                    Logger.Info(String.Format("Unknwon SUGGEST_ICON No:[{0}]", row("R_SUGGEST_ICON").ToString))
                    SUGGEST_ICON = DEFAULT_SUGGEST_ICON
                End If
            ElseIf S_ReqItemCd <> "" And S_SvcCd = InspecType.SUGGEST Then
                '初期データから読み込む
                SUGGEST_STATUS = S_SuggestStatus
                If Integer.Parse(S_ReqItemCd) < MAX_SUGGEST_ICON_NO Then
                    SUGGEST_ICON = SuggestNoList(Integer.Parse(S_ReqItemCd))
                Else
                    '不明な表示アイテムコードが出てきた
                    Logger.Info(String.Format("Unknwon REQ_ITEM_CD No:[{0}]", S_ReqItemCd))
                    SUGGEST_ICON = DEFAULT_SUGGEST_ICON
                End If
            Else
                '上記以外
                SUGGEST_ICON = DEFAULT_SUGGEST_ICON
            End If

            '初期表示アイコンを保持（リセットを押した時に表示する内容）
            Dim DefaultStatus As String
            If S_ReqItemCd <> "" And S_SvcCd = InspecType.SUGGEST Then
                '初期データから読み込む
                If Integer.Parse(S_ReqItemCd) < MAX_SUGGEST_ICON_NO Then
                    DefaultStatus = SuggestNoList(Integer.Parse(S_ReqItemCd))
                    SUGGEST_STATUS = S_SuggestStatus
                Else
                    '不明な表示アイテムコードが出てきた
                    Logger.Info(String.Format("Unknwon REQ_ITEM_CD No:[{0}]", S_ReqItemCd))
                    DefaultStatus = DEFAULT_SUGGEST_ICON
                    SUGGEST_STATUS = DEFAULT_SUGGEST_STATUS
                End If
            Else
                '上記以外
                DefaultStatus = DEFAULT_SUGGEST_ICON
                SUGGEST_STATUS = DEFAULT_SUGGEST_STATUS
            End If

            dr("Result") = String.Empty
            dr("ResultImage") = ResultImages(0)
            dr("NeedIconFlg") = String.Format("{0},{1},{2},{3},{4}" _
                                              , row.DISP_INSPEC_ITEM_NEED_INSPEC _
                                              , row.DISP_INSPEC_ITEM_NEED_REPLACE _
                                              , row.DISP_INSPEC_ITEM_NEED_FIX _
                                              , row.DISP_INSPEC_ITEM_NEED_CLEAN _
                                              , row.DISP_INSPEC_ITEM_NEED_SWAP)
            'SuggestInfo = アイテムコード，表示するアイコンNo，強く表示フラグ，変更フラグ，初期アイコンNo，変更前のSuggestアイコンNo
            dr("SuggestInfo") = String.Format("{0},{1},{2},{3},{4},{5}" _
                                              , row.INSPEC_ITEM_CD _
                                              , SUGGEST_ICON _
                                              , SUGGEST_STATUS _
                                              , ChangeFlag _
                                              , DefaultStatus _
                                              , SUGGEST_ICON)

            dt.Rows.Add(dr)

            '2014/07/18　Suggestリスト変更時のメッセージ表示判断修正　START　↓↓↓
            '実績データに変更項目があれば、hdnAlreadySendFlagを「1」にする（編集モードの時のみ）
            If row("R_SUGGEST_ICON").ToString <> "" And row("R_SVC_CD").ToString = InspecType.SUGGEST Then
                If Integer.Parse(row("R_SUGGEST_ICON").ToString) < MAX_SUGGEST_ICON_NO Then
                    If hdnViewMode.Value = "0" Then
                        hdnAlreadySendFlag.Value = "1"
                    End If
                End If
            End If
            '2014/07/18　Suggestリスト変更時のメッセージ表示判断修正　END　↑↑↑

        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return(Count):[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , dt.Rows.Count.ToString))

        Return dt

    End Function
    '2014/05/29 レスポンス対策　END　　↑↑↑

    ''' <summary>
    ''' 明細部の部位テーブルのデータ部のカラム生成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateListDataColumns() As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("ListNo", GetType(String)))
        dt.Columns.Add(New DataColumn("ItemName1", GetType(String)))
        dt.Columns.Add(New DataColumn("ItemName2", GetType(String)))
        'dt.Columns.Add(New DataColumn("ItemName3", GetType(String)))
        dt.Columns.Add(New DataColumn("Result", GetType(String)))
        dt.Columns.Add(New DataColumn("ResultImage", GetType(String)))
        dt.Columns.Add(New DataColumn("INSPEC_ITEM_CD", GetType(String)))
        dt.Columns.Add(New DataColumn("SuggestImage", GetType(String)))
        dt.Columns.Add(New DataColumn("SUGGEST_ICON", GetType(String)))
        dt.Columns.Add(New DataColumn("ChangeFlag", GetType(String)))
        dt.Columns.Add(New DataColumn("ListIndex", GetType(String)))
        'dt.Columns.Add(New DataColumn("ServiceItem", GetType(String)))
        dt.Columns.Add(New DataColumn("NeedIconFlg", GetType(String)))
        'dt.Columns.Add(New DataColumn("NEED_INSPEC", GetType(String)))
        'dt.Columns.Add(New DataColumn("NEED_REPLACE", GetType(String)))
        'dt.Columns.Add(New DataColumn("NEED_FIX", GetType(String)))
        'dt.Columns.Add(New DataColumn("NEED_CLEAN", GetType(String)))
        'dt.Columns.Add(New DataColumn("NEED_SWAP", GetType(String)))
        'dt.Columns.Add(New DataColumn("SUGGEST_STATUS", GetType(String)))
        dt.Columns.Add(New DataColumn("SuggestInfo", GetType(String)))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return(Count):[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , dt.Rows.Count.ToString))

        Return dt

    End Function

    ''' <summary>
    ''' 明細部の部位テーブルのデータ部に表示内容をセット
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub ShowListData(ByVal dt As DataTable, ByVal strListNo As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} dt_Count:[{3}] strListNo:[{4}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dt.Rows.Count.ToString _
                  , strListNo))

        '配置位置を変換する
        '2014/06/04 配置位置をCSSで対応　START　↓↓↓
        'Dim SetNo As String = "01"
        'SetNo = ChangeListNo(dt.Rows(0)("ListNo").ToString)
        Dim SetNo As String = dt.Rows(0)("ListNo").ToString
        '2014/06/04 配置位置をCSSで対応　END　　↑↑↑

        Dim grd As GridView = DirectCast(holder.FindControl(String.Format("List{0}_Data", SetNo)), GridView)

        For i As Integer = 0 To dt.Rows.Count - 1
            'dt.Rows(i)("ItemName1") = dt.Rows(i)("ItemName1") + dt.Rows(i)("ItemName2")

            If dt.Rows(i)("ListIndex").ToString <> "" Then
                dt.Rows(i)("ListIndex") = dt.Rows(i)("ListIndex").ToString & "."
            Else
                dt.Rows(i)("ItemName1") = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & dt.Rows(i)("ItemName2").ToString
            End If

        Next

        '検査項目を表示させる
        grd.DataSource = dt
        grd.DataBind()

        Dim CellStyle1 As String = "none none none none"
        Dim CellStyle2 As String = "none solid none solid"

        For i As Integer = 0 To grd.Rows.Count - 1
            For j As Integer = 0 To grd.Rows(i).Cells.Count - 1
                grd.Rows(i).Cells(j).CssClass = String.Format("row{0}", j + 1)
                If (j = 2) Then
                    grd.Rows(i).Cells(j).Style.Add("border-style", CellStyle1)
                Else
                    grd.Rows(i).Cells(j).Style.Add("border-style", CellStyle2)
                End If
            Next

            grd.Rows(i).Cells(2).Attributes.Add("onclick", String.Format("OnClickItem('{0}');", SetNo))  'suggestアイコンをクリックしてポップアップする機能を追加

            'If String.IsNullOrEmpty(DirectCast(grd.Rows(i).FindControl("ResultImage"), Image).ImageUrl) Then
            '    DirectCast(grd.Rows(i).FindControl("Result"), Label).CssClass = "roundMark t2"
            '    DirectCast(grd.Rows(i).FindControl("ResultImage"), Image).Style.Add("display", "none")
            'End If
            'DirectCast(grd.Rows(i).FindControl("ResultImage"), Image).ImageUrl = ResolveClientUrl(images(CInt(hdnSUGGEST_ICON.Value)))
            'DirectCast(grd.Rows(i).FindControl("ResultImage"), Image).ImageUrl = ResultImages(1)

            'Suggestアイコンの設定

            '2014/06/02 レスポンス対策　START　↓↓↓

            'Dim hdnSUGGEST_ICON As HiddenField = DirectCast(grd.Rows(i).FindControl("hdnSUGGEST_ICON"), HiddenField)
            'If Not String.IsNullOrWhiteSpace(hdnSUGGEST_ICON.Value) Then
            '    If images.Count <= Integer.Parse(hdnSUGGEST_ICON.Value) Then
            '        hdnSUGGEST_ICON.Value = DEFAULT_SUGGEST_ICON
            '    End If
            '    'DirectCast(grd.Rows(i).FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(CInt(hdnSUGGEST_ICON.Value)))
            '    DirectCast(grd.Rows(i).FindControl("SuggestImage"), Label).CssClass = images(Integer.Parse(hdnSUGGEST_ICON.Value))
            'Else
            '    'DirectCast(grd.Rows(i).FindControl("SuggestImage"), Image).Visible = False
            '    DirectCast(grd.Rows(i).FindControl("SuggestImage"), Label).Visible = False
            'End If

            Dim SuggestInfo() As String = DirectCast(grd.Rows(i).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
            If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnSUGGEST_ICON)) Then
                If images.Count <= Integer.Parse(SuggestInfo(hdnSUGGEST_ICON)) Then
                    SuggestInfo(hdnSUGGEST_ICON) = DEFAULT_SUGGEST_ICON
                End If
                DirectCast(grd.Rows(i).FindControl("SuggestImage"), Label).CssClass = images(Integer.Parse(SuggestInfo(hdnSUGGEST_ICON)))
            Else
                DirectCast(grd.Rows(i).FindControl("SuggestImage"), Label).Visible = False
            End If

            '2014/06/02 レスポンス対策　END　　↑↑↑

            '強く推奨フラグが上がっているかチェック

            '2014/06/02 レスポンス対策　START　↓↓↓

            'Dim hdnSUGGEST_STATUS As HiddenField = DirectCast(grd.Rows(i).FindControl("hdnSUGGEST_STATUS"), HiddenField)
            'If hdnSUGGEST_STATUS.Value <> "" Then
            '    If hdnSUGGEST_ICON.Value = SUGGEST_NEED_REPLACE AndAlso Integer.Parse(DEFAULT_SUGGEST_STATUS) < Integer.Parse(hdnSUGGEST_STATUS.Value) Then
            '        '「Need Replace」で強く推奨フラグが上がっている
            '        '2014/05/20 「強く推奨時」の表示変更　START　↓↓↓
            '        If DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).Text.IndexOf("<br/>") = -1 Then
            '            DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_ONE_LINE_RED
            '        Else
            '            DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_TWO_LINE_RED
            '        End If
            '        DirectCast(grd.Rows(i).FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(7))

            '        'If DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", strListNo)), Label).Text.IndexOf("<br/>") = -1 Then
            '        '    DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", strListNo)), Label).CssClass = TITLE_ONE_LINE_RED
            '        'Else
            '        '    DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", strListNo)), Label).CssClass = TITLE_TWO_LINE_RED
            '        'End If
            '        'DirectCast(grd.Rows(i).FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(7))
            '        '2014/05/20 「強く推奨時」の表示変更　　END　↑↑↑
            '    End If
            'End If

            If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnSUGGEST_STATUS)) Then
                If SuggestInfo(hdnSUGGEST_ICON) = SUGGEST_NEED_REPLACE AndAlso Integer.Parse(DEFAULT_SUGGEST_STATUS) < Integer.Parse(SuggestInfo(hdnSUGGEST_STATUS)) Then
                    '「Need Replace」で強く推奨フラグが上がっている
                    Dim LabelTitle As Label = DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label)
                    If LabelTitle.Text.IndexOf("<br/>") = -1 And LabelTitle.Text.IndexOf("<br>") = -1 Then
                        LabelTitle.CssClass = TITLE_ONE_LINE_RED
                    Else
                        LabelTitle.CssClass = TITLE_TWO_LINE_RED
                    End If
                    DirectCast(grd.Rows(i).FindControl("SuggestImage"), Label).CssClass = images(7)
                End If
            End If

            '2014/06/02 レスポンス対策　END　　↑↑↑

        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 明細部の部位名／Suggestアイコンの再表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AllPartRegenerate()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '各部位の再表示
        'Dim grvListData As GridView
        For i As Integer = 1 To GRIDVIEW_NUMBER
            'grvListData = lstGridView(i - 1)

            '2014/06/25 強く推奨アイコンがあったら色を赤色にする　START　↓↓↓
            '部位名の色を標準色に戻す
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
            'If DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_ONE_LINE_RED Then
            '    DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_ONE_LINE
            'ElseIf DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_TWO_LINE_RED Then
            '    DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_TWO_LINE
            'End If
            Dim ListTitleLabel As Label = DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label)
            Dim ListResultFlag As String = DirectCast(holder.FindControl(String.Format("ResultFlag0{0}", i)), HiddenField).Value
            If ListTitleLabel.CssClass = TITLE_ONE_LINE_RED Then
                If ListResultFlag = "0" Then
                    ListTitleLabel.CssClass = TITLE_ONE_LINE
                End If
            ElseIf ListTitleLabel.CssClass = TITLE_TWO_LINE_RED Then
                If ListResultFlag = "0" Then
                    ListTitleLabel.CssClass = TITLE_TWO_LINE
                End If
            End If
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
            '2014/06/25 強く推奨アイコンがあったら色を赤色にする　END　　↑↑↑

            For Each grvListRow As GridViewRow In lstGridView(i - 1).Rows

                '2014/06/02 レスポンス対策　START　↓↓↓

                'If Not String.IsNullOrWhiteSpace(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value) Then
                '    'DirectCast(grvListRow.FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(CInt(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value)))
                '    DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = ResolveClientUrl(images(Integer.Parse(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value)))
                '    If DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value = "1" AndAlso Integer.Parse(DirectCast(grvListRow.FindControl("hdnSUGGEST_STATUS"), HiddenField).Value) > 0 Then
                '        'DirectCast(grvListRow.FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(7))
                '        DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = ResolveClientUrl(images(7))
                '    End If
                'End If

                Dim SuggestInfo() As String = DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
                If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnSUGGEST_ICON)) Then
                    DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = images(Integer.Parse(SuggestInfo(hdnSUGGEST_ICON)))
                    If SuggestInfo(hdnSUGGEST_ICON) = "1" AndAlso Integer.Parse(SuggestInfo(hdnSUGGEST_STATUS)) > 0 Then
                        DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = images(7)
                        '2014/06/23 ヘッダーの色も再表示するように追加　START　↓↓↓
                        If DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_ONE_LINE Then
                            DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_ONE_LINE_RED
                        ElseIf DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_TWO_LINE Then
                            DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", i)), Label).CssClass = TITLE_TWO_LINE_RED
                        End If
                        '2014/06/23 ヘッダーの色も再表示するように追加　END　　↑↑↑
                    End If
                End If

                '2014/06/02 レスポンス対策　END　　↑↑↑

            Next
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub

#End Region

#Region "Result表示関連"

    ''' <summary>
    ''' 固有ヘッダのResult一覧で選択した点検結果を明細部のResult欄に表示する処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetResultDetail()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '車両特定時
        '2014/06/10 車両特定判定変更（車両判定不要のためコメント化）　START　↓↓↓
        'If ImageLogo.Visible Then
        '2014/06/10 車両特定判定変更（車両判定不要のためコメント化）　END　↑↑↑
        Dim dtResultListData As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable
        '現在Resultが選択されている項目番号を取得する
        'Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedValue.ToString)
        Dim SelectNo As Integer = Integer.Parse(ddlResult.SelectedIndex.ToString)
        If Not String.IsNullOrWhiteSpace(Params.VIN_NO) Then
            'VINから過去の実績をすべて取得する
            dtResultListData = Biz.GetInspectionDetail(Params.VIN_NO)
            '取得した過去実績からResultに選択されたものの項目を取り出す
            'SetResultData(ByVal strJOB_DTL_ID As String, ByVal strDLR_CD As String, ByVal strBRN_CD As String, ByVal dtResultListData As SC3250101DataSet.TB_T_INSPECTION_DETAILDataTable)

            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
            'Dim strJOB_DTL_ID As String = ResultList(SelectNo)("JOB_DTL_ID").ToString
            Dim strRO_NUM As String = ResultList(SelectNo)("RO_NUM").ToString
            Dim strDLR_CD As String = ResultList(SelectNo)("DLR_CD").ToString
            Dim strBRN_CD As String = ResultList(SelectNo)("BRN_CD").ToString
            'SetResultData(strJOB_DTL_ID, strDLR_CD, strBRN_CD, dtResultListData)
            SetResultData(strRO_NUM, strDLR_CD, strBRN_CD, dtResultListData)
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑
        End If
        'End If

        '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
        'ROプレビューのURLを取得する
        Dim ROPreviewURL As String = MakeROPreviewURL()
        For i = 1 To 9
            'ROプレビューのポップアップ化
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
            'DirectCast(holder.FindControl(String.Format("List0{0}_Result", i)), Label).Text = InspecType.RESULT
            DirectCast(holder.FindControl(String.Format("List0{0}_Result", i)), Label).Text = ResultList(Integer.Parse(ddlResult.SelectedIndex.ToString))("MERCHANDISENAMES").ToString
            'DirectCast(holder.FindControl(String.Format("List0{0}_Col2", i)), HtmlGenericControl).InnerHtml = String.Format("Result<br/>{0}", InspecType.RESULT)
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
            DirectCast(holder.FindControl(String.Format("List0{0}_ResultName", i)), HtmlTableCell).Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", ROPreviewURL))
        Next
        '2014/05/27 ポップアップによるROプレビュー（過去）表示　END　　↑↑↑

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 指定した作業内容IDの結果を明細部のResult欄に表示する
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="dtResultListData">完成検査結果詳細データ</param>
    ''' <remarks>2014/09/03　strJOB_DTL_CD→strRO_NUMに変更</remarks>
    Private Sub SetResultData(ByVal strRO_NUM As String, ByVal strDLR_CD As String, ByVal strBRN_CD As String, ByVal dtResultListData As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} strRO_NUM:[{3}] strDLR_CD:[{4}] strBRN_CD:[{5}] dtResultListData(Count):[{6}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strRO_NUM _
                  , strDLR_CD _
                  , strBRN_CD _
                  , dtResultListData.Rows.Count.ToString))

        Dim ResultDataRows() As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILRow
        'Dim ListIndex As Integer = 1

        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
        ResultDataRows = DirectCast(dtResultListData.Select(String.Format("RO_NUM='{0}' AND DLR_CD='{1}' AND BRN_CD='{2}'", strRO_NUM, strDLR_CD, strBRN_CD)), SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILRow())
        'ResultDataRows = DirectCast(dtResultListData.Select(String.Format("JOB_DTL_ID='{0}' AND DLR_CD='{1}' AND BRN_CD='{2}'", strJOB_DTL_ID, strDLR_CD, strBRN_CD)), SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILRow())
        '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

        '条件で取り出した実績データをループ
        '2014/05/20 完成検査結果データ取得変更　START　↓↓↓
        Dim iconIndex As Integer = 0
        Dim priorityNo As Integer = 10
        Dim checkPriorityNo As Integer
        '2014/05/20 完成検査結果データ取得変更　　END　↑↑↑
        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        Dim SetNo As Integer = 0
        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

        Dim SetEditFlag As Boolean = False
        For Each Row As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILRow In ResultDataRows
            SetEditFlag = False
            '部位データをループ
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
            SetNo = 0
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
            For Each grvListData As GridView In lstGridView
                '部位データの項目をループ
                '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
                SetNo += 1
                '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
                For Each grvListRow As GridViewRow In grvListData.Rows

                    Dim SuggestInfo() As String = DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)

                    'If DirectCast(grvListRow.FindControl("hdnINSPEC_ITEM_CD"), HiddenField).Value = Row.Item("INSPEC_ITEM_CD").ToString Then
                    If SuggestInfo(hdnINSPEC_ITEM_CD) = Row.Item("INSPEC_ITEM_CD").ToString Then

                        '2014/06/23 Alreadyアイコンの表示問題修正　START　↓↓↓
                        priorityNo = 10
                        '2014/06/23 Alreadyアイコンの表示問題修正　END　　↑↑↑

                        '2014/05/20 完成検査結果データ取得変更　START　↓↓↓
                        '点検結果アイコンの設定
                        iconIndex = Row.INSPEC_RSLT_CD
                        'iconIndex = Integer.Parse(Row.Item("INSPEC_RSLT_CD").ToString)

                        '2014/07/07　NoActionアイコン追加　START　↓↓↓
                        '「No Action」の場合は、CompletedWorkのチェックは行わない
                        If iconIndex <> InspecResultCD.NoAction Then
                            '作業内容アイコンの設定
                            'ALREADY_REPLACE
                            'If Integer.Parse(Row.Item("OPERATION_RSLT_ALREADY_REPLACE").ToString) = SelectFlg.CheckOn Then
                            If Row.OPERATION_RSLT_ALREADY_REPLACE <> SelectFlg.CheckOff Then
                                checkPriorityNo = Row.DISP_OPE_ITEM_ALREADY_REPLACE
                                'checkPriorityNo = Integer.Parse(Row.Item("DISP_OPE_ITEM_ALREADY_REPLACE").ToString)
                                If 0 < checkPriorityNo And checkPriorityNo < priorityNo Then
                                    iconIndex = InspecResultCD.AlreadyReplace
                                    priorityNo = checkPriorityNo
                                End If
                            End If

                            'ALREADY_FIX
                            'If Integer.Parse(Row.Item("OPERATION_RSLT_ALREADY_FIX").ToString) = SelectFlg.CheckOn Then
                            If Row.OPERATION_RSLT_ALREADY_FIX <> SelectFlg.CheckOff Then
                                checkPriorityNo = Row.DISP_OPE_ITEM_ALREADY_FIX
                                'checkPriorityNo = Integer.Parse(Row.Item("DISP_OPE_ITEM_ALREADY_FIX").ToString)
                                If 0 < checkPriorityNo And checkPriorityNo < priorityNo Then
                                    iconIndex = InspecResultCD.AlreadyFixed
                                    priorityNo = checkPriorityNo
                                End If
                            End If

                            'ALREADY_CLEAN
                            'If Integer.Parse(Row.Item("OPERATION_RSLT_ALREADY_CLEAN").ToString) = SelectFlg.CheckOn Then
                            If Row.OPERATION_RSLT_ALREADY_CLEAN <> SelectFlg.CheckOff Then
                                checkPriorityNo = Row.DISP_OPE_ITEM_ALREADY_CLEAN
                                'checkPriorityNo = Integer.Parse(Row.Item("DISP_OPE_ITEM_ALREADY_CLEAN").ToString)
                                If 0 < checkPriorityNo And checkPriorityNo < priorityNo Then
                                    iconIndex = InspecResultCD.AlreadyCleaning
                                    priorityNo = checkPriorityNo
                                End If
                            End If

                            'ALREADY_SWAP
                            'If Integer.Parse(Row.Item("OPERATION_RSLT_ALREADY_SWAP").ToString) = SelectFlg.CheckOn Then
                            If Row.OPERATION_RSLT_ALREADY_SWAP <> SelectFlg.CheckOff Then
                                checkPriorityNo = Row.DISP_OPE_ITEM_ALREADY_SWAP
                                'checkPriorityNo = Integer.Parse(Row.Item("DISP_OPE_ITEM_ALREADY_SWAP").ToString)
                                If 0 < checkPriorityNo And checkPriorityNo < priorityNo Then
                                    iconIndex = InspecResultCD.AlreadySwapped
                                    priorityNo = checkPriorityNo
                                End If
                            End If
                        End If
                        '2014/07/07　NoActionアイコン追加　END　　↑↑↑

                        'アイコンの表示
                        'DirectCast(grvListRow.FindControl("ResultImage"), Image).ImageUrl = ResultImages(iconIndex)
                        DirectCast(grvListRow.FindControl("ResultImage"), Label).CssClass = ResultImages(iconIndex)

                        'DirectCast(grvListRow.FindControl("ResultImage"), Image).ImageUrl = ResultImages(CInt(Row.Item("INSPEC_RSLT_CD").ToString))
                        '2014/05/20 完成検査結果データ取得変更　　END　↑↑↑

                        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
                        'Result結果が「3:Need Replace」「4:Need Fix」「6:Need Swap」の時、部位名を赤色表示する
                        If iconIndex = InspecResultCD.NeedReplace Or iconIndex = InspecResultCD.NeedFixing Or iconIndex = InspecResultCD.NeedSwapping Then
                            Dim LabelTitile As Label = DirectCast(holder.FindControl(String.Format("List0{0}_Col1_Title", SetNo)), Label)
                            If LabelTitile.CssClass = TITLE_ONE_LINE Then
                                '部位名の色を変更する（一列用）
                                LabelTitile.CssClass = TITLE_ONE_LINE_RED
                            ElseIf LabelTitile.CssClass = TITLE_TWO_LINE Then
                                '部位名の色を変更する（二列用）
                                LabelTitile.CssClass = TITLE_TWO_LINE_RED
                            End If
                            'Result赤色表示フラグをたてる
                            DirectCast(holder.FindControl(String.Format("ResultFlag0{0}", SetNo)), HiddenField).Value = "1"

                        End If
                        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

                        SetEditFlag = True
                        Exit For
                    End If
                    If SetEditFlag = True Then
                        Exit For
                    End If
                Next
            Next
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    '2014/05/20 一般整備選択時Resultを「-」に変更　START　↓↓↓
    ''' <summary>
    ''' 明細部のResult欄の項目をすべてNone「-」にする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetResultData()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '**** Resultヘッダの点検名称をNone「-」にする
        For i = 1 To GRIDVIEW_NUMBER
            'DirectCast(holder.FindControl(String.Format("hdnINSPEC_TYPE0{0}", i)), HiddenField).Value = sUncertain
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
            DirectCast(holder.FindControl(String.Format("List0{0}_Result", i)), Label).Text = sUncertain
            'DirectCast(holder.FindControl(String.Format("List0{0}_Col2", i)), HtmlGenericControl).InnerHtml = String.Format("Result<br/>{0}", sUncertain)
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
            DirectCast(holder.FindControl(String.Format("List0{0}_ResultName", i)), HtmlTableCell).Attributes.Remove("onclick")
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
            'Result欄の推奨フラグを「0」に戻す
            DirectCast(holder.FindControl(String.Format("ResultFlag0{0}", i)), HiddenField).Value = "0"
            '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
        Next

        '**** 明細部のResultアイコンをNone「-」にする
        '部位データをループ
        For Each grvListData As GridView In lstGridView
            '部位データの項目をループ
            For Each grvListRow As GridViewRow In grvListData.Rows

                '2014/06/02 レスポンス対策　START　↓↓↓
                'If Not String.IsNullOrWhiteSpace(DirectCast(grvListRow.FindControl("hdnINSPEC_ITEM_CD"), HiddenField).Value) Then
                '    DirectCast(grvListRow.FindControl("ResultImage"), Label).CssClass = ResultImages(0)
                'End If

                Dim SuggestInfo() As String = DirectCast(grvListRow.FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
                If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnINSPEC_ITEM_CD)) Then
                    DirectCast(grvListRow.FindControl("ResultImage"), Label).CssClass = ResultImages(InspecResultCD.Notselected)
                End If
                '2014/06/02 レスポンス対策　END　　↑↑↑

            Next
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
    '2014/05/20 一般整備選択時Resultを「-」に変更　　END　↑↑↑

#End Region

#End Region

#Region "拡大部エリア関連"

    ''' <summary>
    ''' 部位拡大画面表示
    ''' </summary>
    ''' <param name="strListNo"></param>
    ''' <remarks></remarks>
    Private Sub ShowPopUp(ByVal strListNo As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} strListNo:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strListNo))

        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        '各部位の再表示
        Call AllPartRegenerate()
        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

        '拡大画面を表示する
        contentsMainonBoard.Style.Add("display", "block")
        popUp.Style.Add("display", "block")
        closeBtn.Style.Add("display", "block")
        popUpList.Style.Add("display", "block")

        '--データ作成
        '拡大画面に表示するGridViewを取得する
        Dim grvListData As GridView = lstGridView(Integer.Parse(strListNo) - 1)
        'Dim intRowNo As Integer = 0

        '配置位置を変換する（変換処理不要（配置位置をCSSで対応したため）コメント化）
        '2014/06/04 配置位置をCSSで対応　START　↓↓↓
        'Dim SetNo As String = "01"
        'SetNo = ChangeListNo_PopUp(strListNo)
        'Dim SetNo As String = strListNo
        '2014/06/04 配置位置をCSSで対応　END　　↑↑↑

        '**** ヘッダ作成
        Dim dttListHeader As DataTable = CreateListHeader(strListNo)
        'popUpTitleImage.ImageUrl = ResolveClientUrl(dttListHeader.Rows(0)("ImageUrl"))

        'ヘッダーアイコンの設定
        popUpTitleImageFrame.Attributes.Add("class", dttListHeader.Rows(0)("ImageUrl").ToString)

        'ヘッダータイトルの設定
        '2014/07/04 <br>タグの取り除き処理修正　START　↓↓↓
        Dim TitleName As String = dttListHeader.Rows(0)("title").ToString
        TitleName = TitleName.Replace("<br/>", " ")
        TitleName = TitleName.Replace("<br>", " ")
        popUpTitle.InnerHtml = TitleName
        'popUpTitle.InnerHtml = dttListHeader.Rows(0)("title").ToString.Replace("<br/>", " ")
        ''2014/05/22 文言DBから取得　START　↓↓↓
        'popUpTitle.InnerHtml = dttListHeader.Rows(0)("title").ToString.Replace("<br>", " ")
        ''2014/05/22 文言DBから取得　END　　↑↑↑
        '2014/07/04 <br>タグの取り除き処理修正　END　　↑↑↑

        '2014/05/21 「強く推奨時」の表示処理追加　START　↓↓↓
        popUpTitle.Attributes.Remove("class")
        '2014/05/21 「強く推奨時」の表示処理追加　　END　↑↑↑

        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        'タイトル部分の色を変更する
        Dim CssName As String = DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", strListNo)), Label).CssClass
        If CssName = TITLE_ONE_LINE_RED Or CssName = TITLE_TWO_LINE_RED Then
            popUpTitle.Attributes.Add("class", "OneLineRed")
        End If
        '【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

        '2014/05/21 部位名タップ時の商品紹介画面表示追加　START　↓↓↓
        HeaderCol1.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", dttListHeader.Rows(0)("POPUP_URL").ToString))
        '2014/05/21 部位名タップ時の商品紹介画面表示追加　　END　↑↑↑

        'Dim InspecType As SC3250101BusinessLogic.InspectionType = Biz.GetTitleByTiming(Params.DealerCode, Params.BranchCode, Params.R_O)

        '2014/05/21 Result商品名表示修正　START　↓↓↓
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
        'ヘッダー「Result」の設定
        Header_WordResult.Text = WordResult
        Header_Result.Text = DirectCast(holder.FindControl(String.Format("List{0}_Result", strListNo)), Label).Text
        'HeaderCol2.InnerHtml = DirectCast(holder.FindControl(String.Format("List{0}_Col2", SetNo)), HtmlGenericControl).InnerHtml
        'HeaderCol2.InnerHtml = String.Format("Result<br/>{0}", DirectCast(holder.FindControl(String.Format("hdnINSPEC_TYPE{0}", SetNo)), HiddenField).Value)
        'HeaderCol2.InnerHtml = String.Format("Result<br/>{0}", InspecType.RESULT)
        '2014/05/21 Result商品名表示修正　　END　↑↑↑

        'ヘッダー「Suggest」の設定
        Header_WordSuggest.Text = WordSuggest
        Header_Suggest.Text = DirectCast(holder.FindControl(String.Format("List{0}_Suggest", strListNo)), Label).Text
        'HeaderCol3.InnerHtml = DirectCast(holder.FindControl(String.Format("List{0}_Col3", SetNo)), HtmlGenericControl).InnerHtml
        'HeaderCol3.InnerHtml = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
        'HeaderCol2.InnerHtml = DirectCast(grvListData.Rows(i).FindControl("hdnItemName1"), HiddenField).Value
        'HeaderCol3.InnerHtml = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑

        '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
        'ヘッダー「Result」にROプレビュー（過去）画面を表示させるためのonclickイベントを追加
        HeaderCol2.Attributes.Remove("onclick")
        '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
        'If HeaderCol2.InnerHtml <> String.Format("Result<br/>{0}", sUncertain) Then
        If Header_Result.Text <> sUncertain Then
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
            'ROプレビューのURLを取得する
            Dim ROPreviewURL As String = MakeROPreviewURL()
            'ROプレビューのポップアップ化
            HeaderCol2.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", ROPreviewURL))
        End If
        'HeaderCol2.Attributes.Add("onclick", "ShowROPreview();")
        '2014/05/27 ポップアップによるROプレビュー（過去）表示　END　　↑↑↑

        Logger.Info("ShowPopUp:" & grvListData.Rows.Count)

        '**** 点検項目リスト部分
        '拡大画面に表示するDataTableを作成する
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add(New DataColumn("cell1_val", GetType(String)))
        dt.Columns.Add(New DataColumn("cell2_val", GetType(String)))
        dt.Columns.Add(New DataColumn("cell3_val", GetType(String)))
        dt.Columns.Add(New DataColumn("cell4_img", GetType(String)))
        dt.Columns.Add(New DataColumn("cell5_img_url", GetType(String)))
        dt.Columns.Add(New DataColumn("hdn_NeedIcon_value", GetType(String)))
        'dt.Columns.Add(New DataColumn("hdn_cell_in_value", GetType(String)))
        'dt.Columns.Add(New DataColumn("hdn_cell_rp_value", GetType(String)))
        'dt.Columns.Add(New DataColumn("hdn_cell_fx_value", GetType(String)))
        'dt.Columns.Add(New DataColumn("hdn_cell_cl_value", GetType(String)))
        'dt.Columns.Add(New DataColumn("hdn_cell_sw_value", GetType(String)))

        'Dim grd As GridView = DirectCast(holder.FindControl("popUpDetail"), GridView)
        'Dim grd As GridView = popUpDetail

        For i As Integer = 0 To grvListData.Rows.Count - 1
            'intRowNo = i + 1

            dr = dt.NewRow()
            '点検項目１を追加
            If Not String.IsNullOrEmpty(DirectCast(grvListData.Rows(i).FindControl("hdnItemName1"), HiddenField).Value) Then
                dr("cell1_val") = String.Format("{0}{1}", DirectCast(grvListData.Rows(i).FindControl("ItemNo"), Label).Text, DirectCast(grvListData.Rows(i).FindControl("hdnItemName1"), HiddenField).Value)
            End If
            '点検項目２を追加
            If Not String.IsNullOrWhiteSpace(DirectCast(grvListData.Rows(i).FindControl("hdnItemName2"), HiddenField).Value) Then
                dr("cell1_val") = ""
                dr("cell2_val") = DirectCast(grvListData.Rows(i).FindControl("hdnItemName2"), HiddenField).Value
            End If
            '点検項目３を追加
            'dr("cell3_val") = DirectCast(grvListData.Rows(i).FindControl("hdnItemName3"), HiddenField).Value
            'Result項目の追加
            If Not String.IsNullOrEmpty(DirectCast(grvListData.Rows(i).FindControl("ResultImage"), Label).CssClass) Then
                'dr("cell4_img") = ResolveClientUrl(DirectCast(grvListData.Rows(i).FindControl("ResultImage"), Image).ImageUrl)
                '2014/06/23　拡大画面のアイコン変更　START　↓↓↓
                dr("cell4_img") = DirectCast(grvListData.Rows(i).FindControl("ResultImage"), Label).CssClass & "_Pop"
                '2014/06/23　拡大画面のアイコン変更　END　　↑↑↑
            End If

            'Suggestアイコンの追加
            'Dim ListData As GridView = DirectCast(holder.FindControl(String.Format("List{0}_Data", strListNo)), GridView)

            '2014/06/02 レスポンス対策　START　↓↓↓

            'Dim hdnSUGGEST_ICON As HiddenField = DirectCast(ListData.Rows(i).FindControl("hdnSUGGEST_ICON"), HiddenField)
            'If Not String.IsNullOrWhiteSpace(hdnSUGGEST_ICON.Value) Then
            '    dr("cell5_img_url") = ResolveClientUrl(images(Integer.Parse(hdnSUGGEST_ICON.Value)))
            '    dr("hdn_NeedIcon_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNeedIconFlg"), HiddenField).Value
            '    'dr("hdn_cell_in_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNEED_INSPEC"), HiddenField).Value
            '    'dr("hdn_cell_rp_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNEED_REPLACE"), HiddenField).Value
            '    'dr("hdn_cell_fx_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNEED_FIX"), HiddenField).Value
            '    'dr("hdn_cell_cl_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNEED_CLEAN"), HiddenField).Value
            '    'dr("hdn_cell_sw_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNEED_SWAP"), HiddenField).Value

            '    '2014/05/21 「強く推奨時」の表示処理追加　START　↓↓↓
            '    '強く推奨フラグが上がっているかチェック
            '    Dim hdnSUGGEST_STATUS As HiddenField = DirectCast(grvListData.Rows(i).FindControl("hdnSUGGEST_STATUS"), HiddenField)
            '    If hdnSUGGEST_STATUS.Value <> "" Then
            '        If hdnSUGGEST_ICON.Value = "1" AndAlso 0 < Integer.Parse(hdnSUGGEST_STATUS.Value) Then
            '            '「Need Replace」で強く推奨フラグが上がっている
            '            popUpTitle.Attributes.Add("class", "OneLineRed")
            '            dr("cell5_img_url") = ResolveClientUrl(images(7))
            '        End If
            '    End If
            '    '2014/05/21 「強く推奨時」の表示処理追加　　END　↑↑↑

            'End If

            Dim SuggestInfo() As String = DirectCast(grvListData.Rows(i).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
            If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnSUGGEST_ICON)) Then
                '2014/06/23　拡大画面のアイコン変更　START　↓↓↓
                dr("cell5_img_url") = images(Integer.Parse(SuggestInfo(hdnSUGGEST_ICON))) & "_Pop"
                '2014/06/23　拡大画面のアイコン変更　END　　↑↑↑
                dr("hdn_NeedIcon_value") = DirectCast(grvListData.Rows(i).FindControl("hdnNeedIconFlg"), HiddenField).Value

                '強く推奨フラグが上がっているかチェック
                If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnSUGGEST_STATUS)) Then
                    If SuggestInfo(hdnSUGGEST_ICON) = "1" AndAlso 0 < Integer.Parse(SuggestInfo(hdnSUGGEST_STATUS)) Then
                        '「Need Replace」で強く推奨フラグが上がっている
                        'popUpTitle.Attributes.Add("class", "OneLineRed")
                        '2014/06/23　拡大画面のアイコン変更　START　↓↓↓
                        dr("cell5_img_url") = images(7) & "_Pop"
                        '2014/06/23　拡大画面のアイコン変更　END　　↑↑↑
                    End If
                End If

            End If

            '2014/06/02 レスポンス対策　END　　↑↑↑

            dt.Rows.Add(dr)

        Next

        '2014/09/04 PopUp用GridViewをWithでまとめる　START　↓↓↓
        With popUpDetail
            '**** 拡大画面GridViewにデータバインド
            .DataSource = dt
            .DataBind()

            'Dim Table As HtmlTable = DirectCast(holder.FindControl("popUpDetail2"), HtmlTable)

            'Dim CellStyle1 As String = "none none none none"
            'Dim CellStyle2 As String = "none solid none solid"

            '**** データバインド後の処理
            For i As Integer = 0 To .Rows.Count - 1
                'CSS Class名の設定
                .Rows(i).Cells(0).CssClass = "col1-1"
                .Rows(i).Cells(1).CssClass = "col2"
                .Rows(i).Cells(2).CssClass = "col3"

                'サブ点検項目名を表示している場合は、CSS Class名を変更
                If String.IsNullOrWhiteSpace(DirectCast(.Rows(i).Cells(0).FindControl("cell1_val"), HyperLink).Text.ToString) Then
                    .Rows(i).Cells(0).CssClass = "col1-2"
                End If

                'Result欄にアイコンが指定されていないときは非表示にする
                If String.IsNullOrWhiteSpace(DirectCast(.Rows(i).Cells(1).FindControl("cell4_img"), Label).CssClass.ToString) Then
                    DirectCast(.Rows(i).Cells(1).FindControl("cell4_img"), Label).Style.Add("display", "none")
                    'DirectCast(grd.Rows(i).Cells(1).FindControl("cell4_img"), Image).Style.Add("display", "none")
                End If

                'Suggest欄にアイコンが指定されていないときは非表示、表示されているときはonclickイベント追加
                If String.IsNullOrWhiteSpace(DirectCast(.Rows(i).Cells(2).FindControl("cell5_img_url"), Label).CssClass.ToString) Then
                    DirectCast(.Rows(i).Cells(2).FindControl("cell5_img_url"), Label).Style.Add("display", "none")
                    'DirectCast(grd.Rows(i).Cells(2).FindControl("cell5_img_url"), Image).Style.Add("display", "none")
                Else
                    .Rows(i).Cells(2).Attributes.Add("onclick", String.Format("ShowBalloon('cell{0}_5', {1});return false;", i + 1, i))
                End If

                '2014/08/04　部品説明画面遷移処理追加　START　↓↓↓
                '部品説明画面に遷移するonclickイベントを追加する
                '①点検項目コード（InspecItemCD）を取得する
                Dim SuggestInfo() As String = DirectCast(grvListData.Rows(i).Cells(2).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)

                '②点検項目コードがあればonclickイベントを追加する
                If Not String.IsNullOrWhiteSpace(SuggestInfo(hdnINSPEC_ITEM_CD)) Then
                    If String.IsNullOrWhiteSpace(DirectCast(.Rows(i).Cells(0).FindControl("cell1_val"), HyperLink).Text.ToString) Then
                        'サブ点検項目にonclickイベントを追加
                        DirectCast(.Rows(i).Cells(0).FindControl("cell2_val"), Label).Attributes.Add("onclick", String.Format("OnClickPartsDetail('{0}');", SuggestInfo(hdnINSPEC_ITEM_CD)))
                    Else
                        '点検項目にonclickイベントを追加
                        DirectCast(.Rows(i).Cells(0).FindControl("cell1_val"), HyperLink).Attributes.Add("onclick", String.Format("OnClickPartsDetail('{0}');", SuggestInfo(hdnINSPEC_ITEM_CD)))
                    End If
                End If
                '2014/08/04　部品説明画面遷移処理追加　END　　↑↑↑
            Next
        End With
        '2014/09/04 PopUp用GridViewをWithでまとめる　END　↑↑↑

        ''**** 各部位の再表示
        'AllPartRegenerate(lstGridView)

        '**** 選択されたリストをハイライトする
        For i As Integer = 1 To lstGridView.Count
            'grvListData = lstGridView(i - 1)
            'For Each grvListRow As GridViewRow In grvListData.Rows
            '    If Not String.IsNullOrWhiteSpace(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value) Then
            '        'DirectCast(grvListRow.FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(CInt(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value)))
            '        DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = (images(Integer.Parse(DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value)))
            '        If DirectCast(grvListRow.FindControl("hdnSUGGEST_ICON"), HiddenField).Value = SUGGEST_NEED_REPLACE AndAlso Integer.Parse(DirectCast(grvListRow.FindControl("hdnSUGGEST_STATUS"), HiddenField).Value) > Integer.Parse(DEFAULT_SUGGEST_STATUS) Then
            '            'DirectCast(grvListRow.FindControl("SuggestImage"), Image).ImageUrl = ResolveClientUrl(images(7))
            '            DirectCast(grvListRow.FindControl("SuggestImage"), Label).CssClass = ICON_REPLACE_RED_NO.ToString
            '        End If
            '    End If
            'Next
            '選択されたリストをハイライトし、それ以外のリストはフィルターの下層レイヤーへ
            Dim strListIndex As String = i.ToString.PadLeft(2).Replace(" ", "0")
            If strListIndex = strListNo Then
                DirectCast(holder.FindControl(String.Format("list{0}", strListIndex)), HtmlGenericControl).Style.Add("z-index", "2")
            Else
                DirectCast(holder.FindControl(String.Format("list{0}", strListIndex)), HtmlGenericControl).Style.Remove("z-index")
            End If
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 明細部の部位テーブルのヘッダー部を生成
    ''' </summary>
    ''' <param name="strListNo">部位番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateListHeader(ByVal strListNo As String) As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} strListNo:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strListNo))

        Dim dt As DataTable = CreatetListHeaderColumns()
        Dim dr As DataRow = dt.NewRow()
        'Dim strListName As String = ""

        dr("ListNo") = strListNo
        dr("ImageUrl") = dicPartInfo(strListNo)("ImageUrl")
        dicPartInfo(strListNo)("SVC_CD") = InspecType.SUGGEST
        dr("SVC_CD") = dicPartInfo(strListNo)("SVC_CD")
        '車両判明時の設定
        If (ImageLogo.Visible) Then
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　START　↓↓↓
            dr("Result") = InspecType.RESULT
            dr("Suggest") = InspecType.SUGGEST_DISP
            'dr("Result") = String.Format("Result<br/>{0}", InspecType.RESULT)
            'dr("Suggest") = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
        Else '車両不明時の設定
            dr("Result") = sUncertain
            dr("Suggest") = InspecType.SUGGEST_DISP
            'dr("Result") = String.Format("Result<br/>{0}", sUncertain)
            ''2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
            ''dr("Suggest") = String.Format("Suggest<br/>{0}", sSpace)
            'dr("Suggest") = String.Format("Suggest<br/>{0}", InspecType.SUGGEST)
            ''2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
            '2014/06/18 各テーブルのSuggest,Result項目を文言DBから取得　END　　↑↑↑
        End If

        Dim dtSC3250101 As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        dtSC3250101 = Biz.GetPartInfo(strListNo)
        If dtSC3250101 IsNot Nothing AndAlso 0 < dtSC3250101.Rows.Count Then

            '2014/05/22 文言DBから取得　START　↓↓↓
            'dr("title") = dtSC3250101.Rows(0)("PART_NAME").ToString
            'dr("title") = dtSC3250101.Rows(0)("PART_NAME_NO").ToString
            'dr("title") = WebWordUtility.GetWord(Integer.Parse(dtSC3250101(0)("PART_NAME_NO").ToString))
            dr("title") = WebWordUtility.GetWord(dtSC3250101(0).PART_NAME_NO)
            '2014/05/22 文言DBから取得　END　　↑↑↑

            dr("POPUP_URL") = dtSC3250101.Rows(0)("POPUP_URL").ToString
        End If
        dt.Rows.Add(dr)
        dtSC3250101.Dispose()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} ListNo:[{3}] ImageUrl:[{4}] SVC_CD:[{5}] Result:[{6}] Suggest:[{7}] title:[{8}] POPUP_URL:[{9}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , dr("ListNo").ToString _
                   , dr("ImageUrl").ToString _
                   , dr("SVC_CD").ToString _
                   , dr("Result").ToString _
                   , dr("Suggest").ToString _
                   , dr("title").ToString _
                   , dr("POPUP_URL").ToString))

        Return dt

    End Function

    ''' <summary>
    ''' 部位説明（SC3250103）画面遷移
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowPartsDetail()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '次画面遷移パラメータ設定
        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Params.DealerCode)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Params.BranchCode)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Params.LoginUserID)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, Params.SAChipID)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, Params.BASREZID)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, Params.R_O)
        'RO_JOB_SEQ           
        Me.SetValue(ScreenPos.Next, SessionSEQNO, Params.SEQ_NO)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, Params.VIN_NO)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, Params.ViewMode)
        'ReqPartCD
        Me.SetValue(ScreenPos.Next, SessionReqPartCD, hdnClickedListNo.Value)
        'InspecItemCD
        Me.SetValue(ScreenPos.Next, SessionInspecItemCD, hdnClickedInspecCD.Value)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        '商品訴求コンテンツ画面遷移
        Me.RedirectNextScreen(PARTS_DETAIL_PAGE)

    End Sub

#End Region

#Region "固有ヘッダエリア／明細部エリア／拡大部エリア共通処理"

    ''' <summary>
    ''' URL作成処理
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <param name="inParameterList">置換データリスト</param>
    ''' <returns>URL</returns>
    ''' <remarks></remarks>
    Private Function CreateURL(ByVal inDisplayNumber As Long, _
                               ByVal inParameterList As List(Of String), _
                               ByVal inDomain As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , LOG_START))

        '戻り値宣言
        Dim returnURL As String = String.Empty


        Try
            'URL取得
            Dim dtDisplayRelation As SC3250101DataSet.SC3250101DisplayRelationDataTable = Biz.GetDisplayUrl(inDisplayNumber)

            'URL取得確認
            If 0 < dtDisplayRelation.Count Then
                '取得できた場合
                '戻り値に設定
                returnURL = dtDisplayRelation(0).DMS_DISP_URL

                'ドメイン名を置換する
                returnURL = returnURL.Replace("{0}", inDomain)

                'パラメーターを置換する
                Dim replaceType As Boolean = True
                Dim replacecount As Integer = 1
                While replaceType
                    '置換対象の文字列作成
                    Dim replaceWord As String = String.Concat("{", (replacecount).ToString(CultureInfo.CurrentCulture), "}")

                    '置換対象する文字列の存在チェック
                    If 0 <= returnURL.IndexOf(replaceWord) Then
                        '存在する場合
                        '置換するデータの確認
                        If replacecount <= inParameterList.Count Then
                            '存在する場合
                            '対象データに置換する
                            returnURL = returnURL.Replace(replaceWord, inParameterList(replacecount - 1))

                        Else
                            '存在しない場合
                            '空文字列に置換する
                            returnURL = returnURL.Replace(replaceWord, String.Empty)

                        End If
                    Else
                        '存在しない場合
                        'ループ終了
                        replaceType = False

                    End If

                    replacecount += 1
                End While

            Else
                '取得できなかった場合
                'ログ出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If
            dtDisplayRelation.Dispose()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウト処理
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))

            'DBタイムアウトのメッセージ表示
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
            ShowMessageBox(WordID.id004)

        End Try



        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2} URL:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , LOG_END _
                    , returnURL))

        Return returnURL
    End Function

    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <param name="loginUserID">Sessionより取得したLoginUserID</param>
    ''' <returns>中断情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetDmsBlnCd(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 Optional ByVal loginUserID As String = "") As SC3250101DataSet.DmsCodeMapRow
        'Private Function GetDmsBlnCd(ByVal dealerCode As String, _
        '                             ByVal branchCode As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} dealerCode:[{3}] branchCode:[{4}], loginUserID:[{5}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dealerCode _
                  , branchCode _
                  , loginUserID))

        Dim dmsDlrBrnTable As SC3250101DataSet.DmsCodeMapDataTable = Nothing
        'Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        'Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
        '基幹販売店コード、店舗コードを取得
        dmsDlrBrnTable = Biz.GetIcropToDmsCode(dealerCode, _
                                               SC3250101BusinessLogic.DmsCodeType.BranchCode, _
                                               dealerCode, _
                                               branchCode, _
                                               String.Empty, _
                                               loginUserID)
        'dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dealerCode, _
        '                                                    ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
        '                                                    dealerCode, _
        '                                                    branchCode, _
        '                                                    String.Empty)


        '2014/09/05　DMS変換後のチェックをここで行う（このメソッド呼出後、各々にチェックを行っているため）
        '基幹コード情報Row
        Dim rowDmsCodeMap As SC3250101DataSet.DmsCodeMapRow

        '基幹コードへ変換処理結果チェック
        If dmsDlrBrnTable IsNot Nothing AndAlso 0 < dmsDlrBrnTable.Rows.Count Then
            '基幹コードへ変換処理成功

            'Rowに変換
            rowDmsCodeMap = DirectCast(dmsDlrBrnTable.Rows(0), SC3250101DataSet.DmsCodeMapRow)

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                '変換前のアカウントを設定する
                rowDmsCodeMap.ACCOUNT = loginUserID

            End If

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                '変換前の販売店コードを設定する
                rowDmsCodeMap.CODE1 = dealerCode

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                '変換前の店舗コードを設定する
                rowDmsCodeMap.CODE2 = branchCode

            End If

        Else
            '基幹コードへ変換処理成功失敗

            '新しいRowを作成
            rowDmsCodeMap = DirectCast(dmsDlrBrnTable.NewDmsCodeMapRow, SC3250101DataSet.DmsCodeMapRow)

            '変換前のコードを設定する
            '基幹アカウント
            rowDmsCodeMap.ACCOUNT = loginUserID
            '基幹販売店コード
            rowDmsCodeMap.CODE1 = dealerCode
            '基幹店舗コード
            rowDmsCodeMap.CODE2 = branchCode

        End If

        'If dmsDlrBrnTable.Count <= 0 Then
        '    'データが取得できない場合はエラー
        '    Return Nothing
        'ElseIf 1 < dmsDlrBrnTable.Count Then
        '    'データが2件以上取得できた場合は一意に決定できないためエラー
        '    Return Nothing
        'End If
        'End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:CODE1[{3}] CODE2[{4}] ACCOUNT[{5}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , rowDmsCodeMap.CODE1 _
                   , rowDmsCodeMap.CODE2 _
                   , rowDmsCodeMap.ACCOUNT))

        Return rowDmsCodeMap

    End Function
    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

    ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 start
    ''' <summary>
    ''' Biz関数へ渡すデータセットの作成
    ''' </summary>
    ''' <returns>点検項目の入力データセット</returns>
    ''' <remarks></remarks>
    Private Function CreateInputDataSet() As ArrayList
        Dim table As New ArrayList  'データセット

        '部位毎のループ処理
        For i As Integer = 0 To lstGridView.Count - 1
            ' 変数宣言
            Dim grvListData As GridView = lstGridView(i)
            Dim strSVC_CD As String = DirectCast(holder.FindControl(String.Format("hdnSVC_CD0{0}", i + 1)), HiddenField).Value
            Dim SuggestArray As New ArrayList

            '点検項目毎のループ処理
            For j As Integer = 0 To grvListData.Rows.Count - 1
                '点検項目の設定値取得
                Dim SuggestInfo() As String = DirectCast(grvListData.Rows(j).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
                '項目毎に格納
                SuggestArray.Add(SuggestInfo)
            Next
            '部位毎に格納
            table.Add(New KeyValuePair(Of String, ArrayList)(strSVC_CD, SuggestArray))
        Next

        Return table
    End Function


    ''' <summary>
    ''' 変更があった項目を一時ワーク（TB_W_REPAIR_SUGGESTION）に保存する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SetTB_W_REPAIR_SUGGESTION() As Integer
        '呼び元でエラー処理を行えるようにSub→Functionへ変更
        'Private Sub TB_W_REPAIR_SUGGESTION()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'Dim grvListData As GridView
        'Dim dtListData As DataTable = CreateListDataColumns()
        'Dim strSVC_CD As String
        'Dim strINSPEC_ITEM_CD As String
        'Dim strSUGGEST_ICON As String
        'Dim SendData As New List(Of GridViewRow)
        'Dim ChangeItemCode As String = String.Empty
        Dim result As Integer   '戻り値
        Dim table As ArrayList = CreateInputDataSet() 'データセット

        '【***完成検査_排他制御***】 start
        Dim exclusionResult As Boolean = True
        '排他チェック
        exclusionResult = Biz.CheckUpdateRepairSuggestion(Long.Parse(rowLockvs.Value), Params.DealerCode, Params.BranchCode, Params.R_O)
        If exclusionResult = False Then
            '排他チェックエラー用コードを返却
            Return 98
        End If
        '【***完成検査_排他制御***】 end

        '行単位のコミット処理をコメントアウト、一括してBizに委譲する
        'For i As Integer = 0 To lstGridView.Count - 1
        '    Dim grvListData As GridView = lstGridView(i)
        '    'dtListData = New DataTable
        '    'grvListData = lstGridView(i)
        '    Dim strSVC_CD As String = DirectCast(holder.FindControl(String.Format("hdnSVC_CD0{0}", i + 1)), HiddenField).Value

        '    For j As Integer = 0 To grvListData.Rows.Count - 1
        '        '変更有のデータをDBに反映

        '        '2014/06/02 レスポンス対策　START　↓↓↓

        '        'If DirectCast(grvListData.Rows(j).FindControl("hdnChangeFlag"), HiddenField).Value <> "0" Then
        '        '    strINSPEC_ITEM_CD = DirectCast(grvListData.Rows(j).Cells(0).FindControl("hdnINSPEC_ITEM_CD"), HiddenField).Value
        '        '    strSUGGEST_ICON = DirectCast(grvListData.Rows(j).FindControl("hdnSUGGEST_ICON"), HiddenField).Value
        '        '    'For cn As Integer = 0 To SuggestNoList.Count - 1
        '        '    '    If strSUGGEST_ICON = SuggestNoList(cn) Then
        '        '    '        ChangeItemCode = CStr(cn)
        '        '    '        Exit For
        '        '    '    End If
        '        '    'Next

        '        '    Biz.ShowCart( _
        '        '        Params.DealerCode _
        '        '        , Params.BranchCode _
        '        '        , staffInfo.Account _
        '        '        , Params.R_O _
        '        '        , strSVC_CD _
        '        '        , strINSPEC_ITEM_CD _
        '        '        , strSUGGEST_ICON _
        '        '        )
        '        'End If

        '        Dim SuggestInfo() As String = DirectCast(grvListData.Rows(j).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
        '        If SuggestInfo(hdnChangeFlag) <> "0" Then
        '            'strINSPEC_ITEM_CD = SuggestInfo(hdnINSPEC_ITEM_CD)
        '            'strSUGGEST_ICON = SuggestInfo(hdnSUGGEST_ICON)

        '            'Biz.ShowCart( _
        '            '    Params.DealerCode _
        '            '    , Params.BranchCode _
        '            '    , staffInfo.Account _
        '            '    , Params.R_O _
        '            '    , strSVC_CD _
        '            '    , strINSPEC_ITEM_CD _
        '            '    , strSUGGEST_ICON _
        '            '    )
        '            '2014/06/09 車両不明時も登録できるように変更　START　↓↓↓
        '            result = Biz.Set_TB_W_REPAIR_SUGGESTION_Process( _
        '                staffInfo.DlrCD _
        '                , staffInfo.BrnCD _
        '                , staffInfo.Account _
        '                , Params.SAChipID _
        '                , strSVC_CD _
        '                , SuggestInfo(hdnINSPEC_ITEM_CD) _
        '                , SuggestInfo(hdnSUGGEST_ICON) _
        '                )
        '            'Biz.ShowCart( _
        '            '    staffInfo.DlrCD _
        '            '    , staffInfo.BrnCD _
        '            '    , staffInfo.Account _
        '            '    , Params.R_O _
        '            '    , strSVC_CD _
        '            '    , strINSPEC_ITEM_CD _
        '            '    , strSUGGEST_ICON _
        '            '    )
        '            '2014/06/09 車両不明時も登録できるように変更　END　　↑↑↑
        '        End If

        '        '2014/06/02 レスポンス対策　END　　↑↑↑

        '    Next
        'Next

        ' 保存処理呼出し
        result = Biz.SetTB_W_REPAIR_SUGGESTION(table, staffInfo, Params.SAChipID)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return result
    End Function
    ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 end

    '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
    ''' <summary>
    ''' R/Oプレビュー画面（過去）をポップアップで表示するリンクを作成する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function MakeROPreviewURL() As String

        '開始ログ出力
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} ResultListNo:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , ddlResult.SelectedIndex))

        'ResultをクリックしたときにR/Oプレビューを表示
        'リンク先　→　http://dmstl-dev.toyota.co.th:9082/tops/do/spad013
        'パラメータ　→　http://{0}/tops/do/spad013?DealerCode={1}&BranchCode={2}&LoginUserID={3}&SAChipID={4}&BASREZID={5}&R_O={6}&SEQ_NO={7}&VIN_NO={8}&ViewMode={9}&Format={10}?

        'If ResultList.Count <= 0 Then
        If ddlResult.Items.Count <= 0 Then
            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return:[]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END))
            Return ""
        End If

        '①	入庫管理番号を取得する

        '現在選択されているResultListから店舗コードと販売店コード、ROを取り出す
        'Dim hisBranchCode As String = ResultList(Integer.Parse(ddlResult.SelectedValue))("BRN_CD").ToString
        'Dim hisDealerCode As String = ResultList(Integer.Parse(ddlResult.SelectedValue))("DLR_CD").ToString
        'Dim hisR_O As String = ResultList(Integer.Parse(ddlResult.SelectedValue))("RO_NUM").ToString
        Dim hisBranchCode As String = ResultList(ddlResult.SelectedIndex)("BRN_CD").ToString
        Dim hisDealerCode As String = ResultList(ddlResult.SelectedIndex)("DLR_CD").ToString
        Dim hisR_O As String = ResultList(ddlResult.SelectedIndex)("RO_NUM").ToString

        ''入庫管理番号の作成
        'Dim strSVCIN_NUM As String = Biz.GetSVCIN_NUM(hisBranchCode, hisR_O)

        'ResultListの店舗コードと販売店コードを基幹コードへ変換処理する
        Dim dmsDlrBrnRow As SC3250101DataSet.DmsCodeMapRow = Me.GetDmsBlnCd(hisDealerCode, hisBranchCode)

        Dim hisDmsDealerCode As String = dmsDlrBrnRow.CODE1
        Dim hisDmsBranchCode As String = dmsDlrBrnRow.CODE2

        'If IsNothing(dmsDlrBrnRow) OrElse dmsDlrBrnRow.IsCODE1Null Then
        '    '変換失敗した場合は、変換前のコードを入れておく
        '    hisDmsDealerCode = hisDealerCode
        '    hisDmsBranchCode = hisBranchCode
        'Else
        '    '変換成功時は変換後のコードを入れておく
        '    hisDmsDealerCode = dmsDlrBrnRow.CODE1
        '    hisDmsBranchCode = dmsDlrBrnRow.CODE2
        'End If

        '入庫管理番号の作成
        Dim strSVCIN_NUM As String = Biz.GetSVCIN_NUM(hisDmsBranchCode, hisR_O)


        '②	RO History画面引き渡しパラメータ設定	RO History画面引き渡しパラメータ設定を設定する。
        '【参考資料】SC3150101のROプレビュー（過去）のパラメータ
        'Me.SetValue(ScreenPos.Next, "Session.Param1", DmsDealerCode)                   ' ログインユーザーのDMS販売店コード
        'Me.SetValue(ScreenPos.Next, "Session.Param2", DmsBranchCode)                  ' ログインユーザーのDMS店舗コード
        'Me.SetValue(ScreenPos.Next, "Session.Param3", Params.LoginUserID)                 ' ログインユーザーのアカウント
        'Me.SetValue(ScreenPos.Next, "Session.Param4", saChipID)                        ' 来店管理番号
        'Me.SetValue(ScreenPos.Next, "Session.Param5", basRezId)                    ' DMS予約ID
        'If Not (InRepiarOrder.Equals("0")) Then
        '    Me.SetValue(ScreenPos.Next, "Session.Param6", InRepiarOrder)               ' RO番号
        'Else
        '    Me.SetValue(ScreenPos.Next, "Session.Param6", "")                          ' RO番号
        'End If
        'Me.SetValue(ScreenPos.Next, "Session.Param7", "0")                             ' RO作業連番
        'Me.SetValue(ScreenPos.Next, "Session.Param8", vin)                             ' 車両登録No.のVIN
        'Me.SetValue(ScreenPos.Next, "Session.Param9", "1")                   ' 「1：編集(過去)」固定
        'Me.SetValue(ScreenPos.Next, "Session.Param10", "1")                  ' 「1：過去サービス」固定
        'Me.SetValue(ScreenPos.Next, "Session.Param11", serviceInNumber)                ' 入庫管理番号
        'Me.SetValue(ScreenPos.Next, "Session.Param12", serviceInDealerCode)            ' 入庫履歴の基幹版売店コード
        'Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", "13")             ' 「13：R/O参照」固定

        Dim parameterList As New List(Of String)

        parameterList.Add(DmsDealerCode)            '販売店コード"DealerCode"
        parameterList.Add(DmsBranchCode)            '店舗コード"BranchCode"
        parameterList.Add(DmsLoginUserID)           'アカウント"LoginUserID"
        parameterList.Add(Params.SAChipID)          '来店者実績連番"SAChipID"
        parameterList.Add(Params.BASREZID)          'DMS予約ID"BASREZID"
        parameterList.Add(hisR_O)                   'RO
        parameterList.Add(ROPreview_SeqNo)          'RO_JOB_SEQ"SEQ_NO"
        parameterList.Add(Params.VIN_NO)            'VIN"VIN_NO"
        parameterList.Add(ReadMode)                 'ViewMode
        parameterList.Add(ROPreview_ServiceHistory) 'Format：
        parameterList.Add(strSVCIN_NUM)             '入庫管理番号：SVCIN_NUM
        parameterList.Add(hisDmsDealerCode)         'DMS販売店コード：SVCIN_DealerCode
        If String.IsNullOrWhiteSpace(strSVCIN_NUM) Then
            '入庫管理番号が空なら販売店コードを追加
            parameterList.Add(hisDmsBranchCode)     'DMS店舗コード：SVCIN_BlanchCode
        End If

        '③URLの作成
        '「SC3010501」の初期表示用（MainAreaReload_Click）を参考

        'TBL_SYSTEMENVからドメイン名を取得
        Dim systemEnv As New SystemEnvSetting
        Dim systemEnvParam As String = String.Empty
        Dim drSystemEnvSetting As SYSTEMENVSETTINGRow = _
            systemEnv.GetSystemEnvSetting(SYSTEMENV_SPECIAL_CAMPAIGN_DOMAIN)

        '取得できた場合のみ設定する
        If Not (IsNothing(drSystemEnvSetting)) Then
            systemEnvParam = drSystemEnvSetting.PARAMVALUE
        End If

        '表示番号とパラメータとドメインからURLを作成
        Dim url As New StringBuilder
        url.Append(ROPreview_PopUpURL)
        url.Append(Me.CreateURL(DISPLAY_NUMBER_13, parameterList, systemEnvParam))
        Dim ROPreviewURL As String = url.ToString

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ROPreviewURL))

        Return ROPreviewURL

    End Function
    '2014/05/27 ポップアップによるROプレビュー（過去）表示　　END　　↑↑↑

#End Region

#Region "Webサービス関連"

#Region "XML作成 Register"
    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="lstSendData">送信データ</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXMLOfRegister(ByVal lstSendData As List(Of String()), ByVal WebServiceID As String) As ServiceItemsXmlDocumentClass
        Dim inXmlClass As New ServiceItemsXmlDocumentClass

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'メッセージID
        inXmlClass.Head.MessageId = WebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        'TansmissionDate
        inXmlClass.Head.TransmissionDate = Format(DateTime.Now, SERVICE_DATE_FORMAT).ToString
        '送信日付
        'Dim updDate As Date = DateTime.Now
        'Using smbCommonBiz As New ServiceCommonClassBusinessLogic
        '    Dim dateFormat As String = smbCommonBiz.GetSystemSettingValueBySettingName("DATE_FORMAT")
        '    If String.IsNullOrEmpty(dateFormat) Then
        '        'システム設定値から取得できない場合、固定値とする
        '        inXmlClass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)
        '    Else
        '        'システム設定値から変換したDateFormatで設定
        '        inXmlClass.Head.TransmissionDate = updDate.ToString(dateFormat, CultureInfo.InvariantCulture)
        '    End If
        'End Using

        CreateDetailOfRegister(inXmlClass, lstSendData)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return inXmlClass


    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">XML Template</param>
    ''' <param name="lstSendData">送信データ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub CreateDetailOfRegister(ByRef sendXml As ServiceItemsXmlDocumentClass, _
                             ByVal lstSendData As List(Of String()))

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim Cmn As New ServiceItemsXmlDocumentClass.DetailTag.CommonTag
        Dim lstSrvItem(lstSendData.Count) As ServiceItemsXmlDocumentClass.DetailTag.ServiceItemsTag
        Dim SrvItem As New ServiceItemsXmlDocumentClass.DetailTag.ServiceItemsTag
        Dim lstJbId As New List(Of ServiceItemsXmlDocumentClass.DetailTag.JOBIDsTag)
        Dim TypeCode As New List(Of String)

        TypeCode.Add("IN")      '0:Inspect
        TypeCode.Add("RP")      '1:Replace 
        TypeCode.Add("FX")      '2:Fix     
        TypeCode.Add("SW")      '3:Swap    
        TypeCode.Add("CL")      '4:Cleaning
        'TypeCode.Add("AD")      '5:Adjust
        'TypeCode.Add("XX")      '6:-
        TypeCode.Add("DL")      '5:DELETE
        TypeCode.Add("XX")      '6:-
        TypeCode.Add("XX")      '7:-

        Cmn.DealerCode = staffInfo.DlrCD
        Cmn.BranchCode = staffInfo.BrnCD
        'Cmn.DealerCode = Params.DealerCode
        'Cmn.BranchCode = Params.BranchCode
        Cmn.SAChipID = Params.SAChipID
        Cmn.VIN = Params.VIN_NO
        sendXml.Detail.Common = Cmn

        ReDim sendXml.Detail.ServiceItems(lstSendData.Count - 1)
        For i As Integer = 0 To lstSendData.Count - 1
            sendXml.Detail.ServiceItems(i) = New ServiceItemsXmlDocumentClass.DetailTag.ServiceItemsTag

            '2014/06/02 レスポンス対策　START　↓↓↓

            'sendXml.Detail.ServiceItems(i).ServiceItemCode = DirectCast(lstSendData(i).FindControl("hdnServiceItem"), HiddenField).Value
            'sendXml.Detail.ServiceItems(i).ServiceItemCode = DirectCast(lstSendData(i).FindControl("hdnINSPEC_ITEM_CD"), HiddenField).Value
            'sendXml.Detail.ServiceItems(i).ServiceTypeCode = TypeCode(Integer.Parse(DirectCast(lstSendData(i).FindControl("hdnSUGGEST_ICON"), HiddenField).Value))

            '2014/07/08　引数をGridViewRow→String()に変更　START　↓↓↓
            sendXml.Detail.ServiceItems(i).ServiceItemCode = lstSendData(i)(hdnINSPEC_ITEM_CD)
            sendXml.Detail.ServiceItems(i).ServiceTypeCode = TypeCode(Integer.Parse(lstSendData(i)(hdnSUGGEST_ICON)))
            'Dim SuggestInfo() As String = DirectCast(lstSendData(i).FindControl("hdnSuggestInfo"), HiddenField).Value.Split(","c)
            'sendXml.Detail.ServiceItems(i).ServiceItemCode = SuggestInfo(hdnINSPEC_ITEM_CD)
            'sendXml.Detail.ServiceItems(i).ServiceTypeCode = TypeCode(Integer.Parse(SuggestInfo(hdnSUGGEST_ICON)))
            '2014/07/08　引数をGridViewRow→String()に変更　END　　↑↑↑

            '2014/06/02 レスポンス対策　END　　↑↑↑

        Next

        ReDim sendXml.Detail.JOBIDs(0)
        sendXml.Detail.JOBIDs(0) = New ServiceItemsXmlDocumentClass.DetailTag.JOBIDsTag

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
#End Region

#Region "XML作成 Mileage"
    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXMLOfMileage(ByVal WebServiceID As String) As Request_MileageXmlDocumentClass

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim inXmlClass As New Request_MileageXmlDocumentClass

        'メッセージID
        inXmlClass.Head.MessageId = WebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        'TansmissionDate
        inXmlClass.Head.TransmissionDate = Format(DateTime.Now, SERVICE_DATE_FORMAT).ToString
        ''送信日付
        'Dim updDate As Date = DateTime.Now
        'Using smbCommonBiz As New ServiceCommonClassBusinessLogic
        '    Dim dateFormat As String = smbCommonBiz.GetSystemSettingValueBySettingName("DATE_FORMAT")
        '    If String.IsNullOrEmpty(dateFormat) Then
        '        'システム設定値から取得できない場合、固定値とする
        '        inXmlClass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)
        '    Else
        '        'システム設定値から変換したDateFormatで設定
        '        inXmlClass.Head.TransmissionDate = updDate.ToString(dateFormat, CultureInfo.InvariantCulture)
        '    End If
        'End Using

        CreateDetailOfMileage(inXmlClass)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return inXmlClass
    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">XML Template</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub CreateDetailOfMileage(ByRef sendXml As Request_MileageXmlDocumentClass)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim Cmn As New Request_MileageXmlDocumentClass.DetailTag.CommonTag

        Cmn.DealerCode = staffInfo.DlrCD
        Cmn.BranchCode = staffInfo.BrnCD
        'Cmn.DealerCode = Params.DealerCode
        'Cmn.BranchCode = Params.BranchCode
        Cmn.R_O = Params.R_O
        Cmn.BASREZID = Params.BASREZID
        Cmn.SAChipID = Params.SAChipID
        Cmn.VIN = Params.VIN_NO
        sendXml.Detail.Common = Cmn

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
#End Region

#Region "XML作成 RoThumbnailCount"
    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXMLOfRoThumbnailCount(ByVal WebServiceID As String) As RoThumbnailCountXmlDocumentClass
        Dim inXmlClass As New RoThumbnailCountXmlDocumentClass

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} WebServiceID:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , WebServiceID))

        'メッセージID
        inXmlClass.Head.MessageId = WebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        'TansmissionDate
        inXmlClass.Head.TransmissionDate = Format(DateTime.Now, SERVICE_DATE_FORMAT).ToString
        '送信日付
        'Dim updDate As Date = DateTime.Now
        'Using smbCommonBiz As New ServiceCommonClassBusinessLogic
        '    Dim dateFormat As String = smbCommonBiz.GetSystemSettingValueBySettingName("DATE_FORMAT")
        '    If String.IsNullOrEmpty(dateFormat) Then
        '        'システム設定値から取得できない場合、固定値とする
        '        inXmlClass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)
        '    Else
        '        'システム設定値から変換したDateFormatで設定
        '        inXmlClass.Head.TransmissionDate = updDate.ToString(dateFormat, CultureInfo.InvariantCulture)
        '    End If
        'End Using
        CreateDetailOfMileage(inXmlClass, "0")

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return inXmlClass
    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">XML Template</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub CreateDetailOfMileage(ByRef sendXml As RoThumbnailCountXmlDocumentClass, _
                                      Optional ByVal roSeq As String = "")

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim Cmn As New RoThumbnailCountXmlDocumentClass.DetailTag.CommonTag

        '写真枚数取得Service時は"0"固定対応
        If roSeq = "" Then
            roSeq = Params.SEQ_NO
        End If

        Cmn.SAChipID = Params.SAChipID
        'Cmn.DealerCode = Params.DealerCode
        'Cmn.BranchCode = Params.BranchCode
        Cmn.DealerCode = staffInfo.DlrCD
        Cmn.BranchCode = staffInfo.BrnCD
        Cmn.R_O = Params.R_O
        'Cmn.R_O_SEQNO = Params.SEQ_NO
        Cmn.R_O_SEQNO = roSeq
        Cmn.PictMode = "1" '写真区分(1:追加作業（規定値）、2:外観チェック)
        Cmn.LinkSysType = "1" 'SYSTEM連携種別(1：基幹販売店/店舗コード(規定値)、0:iCROP販売店/店舗コード)
        sendXml.Detail.Common = Cmn

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub
#End Region

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' ハイライトフッター設定
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        If StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SM _
            OrElse StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SA Then
            category = FooterMenuCategory.GoodsSolicitationContents
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'メインメニュー
        Dim mainMenuButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        mainMenuButton.OnClientClick = "return FooterButtonControl();"

        '連絡先
        Dim telephoneBookButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SM Or staffInfo.OpeCD = iCROP.BizLogic.Operation.SA Then

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = "return FooterButtonControl();"

            '商品訴求
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            AddHandler goodsSolicitationContentsButton.Click, AddressOf goodsSolicitationContentsButton_Click
            goodsSolicitationContentsButton.OnClientClick = "return FooterButtonControl();"

            'キャンペーン
            Dim campaignButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            AddHandler campaignButton.Click, AddressOf campaignButton_Click
            campaignButton.OnClientClick = "return FooterButtonControl();"

            '顧客詳細ボタンの設定
            '顧客情報画面(ヘッダー顧客検索機能へフォーカス)
            Dim customerButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            customerButton.OnClientClick = "FooterButtonclick(" & FooterMenuCategory.CustomerDetail & ");"

        End If

        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SM Or staffInfo.OpeCD = iCROP.BizLogic.Operation.SA Or staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then

            'R/O作成
            Dim roMakeButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
            roMakeButton.OnClientClick = "return FooterButtonControl();"

            '来店管理
            Dim reserveManagementButton As CommonMasterFooterButton = _
            DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
            AddHandler reserveManagementButton.Click, AddressOf reserveManagementButton_Click
            reserveManagementButton.OnClientClick = "return FooterButtonControl();"

        End If

        'If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then

        '    '全体管理
        '    Dim wholeManagementButton As CommonMasterFooterButton = _
        '    DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.WholeManagement)
        '    AddHandler wholeManagementButton.Click, AddressOf wholeManagementButton_Click
        '    wholeManagementButton.OnClientClick = "return FooterButtonControl();"

        'End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub mainMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim strMainMenuId As String

        '権限により、別々の画面へ遷移する
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SM Then
            'Service Manager : 全体管理
            strMainMenuId = APPLICATIONID_GENERALMANAGER
        ElseIf staffInfo.OpeCD = iCROP.BizLogic.Operation.SA Then
            'Service Advisor : SAメイン
            strMainMenuId = SA_MAINMENUID
        Else
            strMainMenuId = APPLICATIONID_NOASSIGNMENTLIST
        End If

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} strMainMenuId:[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , strMainMenuId))

        ' メイン画面に遷移する
        Me.RedirectNextScreen(strMainMenuId)

    End Sub

    ' ''' <summary>
    ' ''' フッター「顧客詳細ボタン」クリック時の処理。
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks>
    ' ''' 顧客詳細画面に遷移します。
    ' ''' </remarks>
    'Private Sub CustomerButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)


    '    '開始ログ出力
    '    Logger.Info("CustomerButton_Click_Start")


    '    '次画面遷移パラメータ設定

    '    'DMS予約ID
    '    Me.SetValue(ScreenPos.Next, SessionDMSID, Params.BASREZID)

    '    'VINチェック
    '    If Not String.IsNullOrWhiteSpace(Params.VIN_NO) Then
    '        Me.SetValue(ScreenPos.Next, SessionVIN, Params.VIN_NO)
    '    End If

    '    '終了ログ出力
    '    Logger.Info(String.Format("CustomerButton_Click_End, strMainMenuId:[{0}]", APPLICATIONID_CUSTOMERNEW))

    '    '顧客詳細画面遷移
    '    Me.RedirectNextScreen(APPLICATIONID_CUSTOMERNEW)


    'End Sub

    ''' <summary>
    ''' フッター「R/Oボタン」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' 　R/O一覧画面に遷移します。
    ''' </remarks>
    Private Sub roMakeButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If
        'R/O一覧画面に遷移
        Me.RedirectOrderList()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' R/O一覧画面に遷移
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderList()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        'Dim logOrderList As StringBuilder = New StringBuilder(String.Empty)
        'With logOrderList
        '    .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
        '    .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ORDERLIST))
        'End With
        'Logger.Info(logOrderList.ToString())

        If StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SM Then

            ''ログインスタッフ情報取得
            'Dim staffInfo As StaffContext = StaffContext.Current

            ''基幹コードへ変換処理
            ''2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
            'Dim rowDmsCodeMap As SC3250101DataSet.DmsCodeMapRow = ChangeDmsCode(staffInfo)
            ''Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = ChangeDmsCode(staffInfo)
            ''2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

            ''基幹販売店コードチェック
            'If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
            '    '値無し

            '    'エラーログ
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                   , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '    'エラーメッセージ表示
            '    Me.ShowMessageBox(WordID.id006)

            '    '処理終了
            '    Exit Sub

            'End If

            ''基幹店舗コードチェック
            'If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
            '    '値無し

            '    'エラーログ
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                   , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '    'エラーメッセージ表示
            '    Me.ShowMessageBox(WordID.id006)

            '    '処理終了
            '    Exit Sub

            'End If

            ''基幹アカウントチェック
            'If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
            '    '値無し

            '    'エラーログ
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                   , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '    'エラーメッセージ表示
            '    Me.ShowMessageBox(WordID.id006)

            '    '処理終了
            '    Exit Sub

            'End If

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, DmsDealerCode)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, DmsBranchCode)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, DmsLoginUserID)
            '2014/06/26　パラメータ設定修正　START　↓↓↓
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
            'Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
            'Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
            'Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
            'Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
            'Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
            'Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            '2014/06/26　パラメータ設定修正　END　　↑↑↑
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, SESSIONVALUE_RO_LIST)


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

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If

        '工程管理画面に遷移する
        Logger.Info("Footer:SMBButton_Click:" & PROCESS_CONTROL_PAGE)
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 商品訴求ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub goodsSolicitationContentsButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If


        ''ログインスタッフ情報取得
        'Dim staffInfo As StaffContext = StaffContext.Current

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Params.DealerCode)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Params.BranchCode)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Params.LoginUserID)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, Params.SAChipID)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, Params.BASREZID)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, Params.R_O)
        'RO_JOB_SEQ           
        Me.SetValue(ScreenPos.Next, SessionSEQNO, Params.SEQ_NO)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, Params.VIN_NO)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, Params.ViewMode)


        '商品訴求コンテンツ画面遷移
        Logger.Info("Footer:GoodsSolicitationContentsButton:" & PGMID_GOOD_SOLICITATION_CONTENTS)
        Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub campaignButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If


        ''ログインスタッフ情報取得
        'Dim staffInfo As StaffContext = StaffContext.Current

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, DmsDealerCode)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, DmsBranchCode)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, DmsLoginUserID)
        '2014/06/26　パラメータ設定修正　START　↓↓↓
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
        'Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
        'Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
        'RO
        Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
        'Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
        'RO_JOB_SEQ
        Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
        'Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
        'Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
        'ViewMode
        If String.IsNullOrWhiteSpace(Params.VIN_NO) OrElse String.IsNullOrWhiteSpace(Params.SAChipID) Then
            Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
        Else
            Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        End If
        'Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        '2014/06/26　パラメータ設定修正　END　　↑↑↑ 
        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_CAMPAIGN)

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
    ''' 予約管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub reserveManagementButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '一時ワークに変更された項目を保存する
        If hdnProcMode.Value = ProcMode_SaveWK Then
            'SetTB_W_REPAIR_SUGGESTION()
            ' 保存処理
            Dim ret = SetTB_W_REPAIR_SUGGESTION()

            '【***完成検査_排他制御***】 start
            '排他チェックエラーの場合はダイアログを表示
            If ret = 98 Then
                '排他チェックエラーメッセージの表示
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(Msg_Exclusion)
                DispProc()
                Exit Sub
            End If
            '【***完成検査_排他制御***】 end

            '更新エラーの場合はダイアログを表示
            If ret <> 1 And ret <> 99 Then
                'DBエラー
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
                ShowMessageBox(MsgID_DBERR)
                Exit Sub
            End If
        End If


        '来店管理画面に遷移する
        Logger.Info("Footer:reserveManagementButton:" & APPLICATIONID_VSTMANAGER)
        Me.RedirectNextScreen(APPLICATIONID_VSTMANAGER)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 全体管理ボタンタップイベント
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub wholeManagementButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '一時ワークに変更された項目を保存する
    '    If hdnProcMode.Value = ProcMode_SaveWK Then
    '        SetTB_W_REPAIR_SUGGESTION()
    '    End If


    '    '決定した遷移先に遷移
    '    Logger.Info("Footer:ManagementButton:" & APPLICATIONID_GENERALMANAGER)
    '    Me.RedirectNextScreen(APPLICATIONID_GENERALMANAGER)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    'End Sub

#End Region

#Region "未使用メソッド"
    ''2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
    ' ''' <summary>
    ' ''' 基幹コードへ変換処理
    ' ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ' ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ' ''' </summary>
    ' ''' <param name="inStaffInfo">スタッフ情報</param>
    ' ''' <remarks>基幹コード情報ROW</remarks>
    ' ''' <history>
    ' ''' </history>
    'Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
    '                              As SC3250101DataSet.DmsCodeMapRow

    '    'Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
    '    '                              As ServiceCommonClassDataSet.DmsCodeMapRow

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , LOG_START _
    '              , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

    '    'SMBCommonClassBusinessLogicのインスタンス

    '    'Using smbCommon As New ServiceCommonClassBusinessLogic

    '    '基幹コードへ変換処理
    '    Dim dtDmsCodeMap As SC3250101DataSet.DmsCodeMapDataTable = _
    '        Biz.GetIcropToDmsCode(inStaffInfo.DlrCD, _
    '                              SC3250101BusinessLogic.DmsCodeType.BranchCode, _
    '                              inStaffInfo.DlrCD, _
    '                              inStaffInfo.BrnCD, _
    '                              String.Empty, _
    '                              inStaffInfo.Account)
    '    'Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
    '    '    smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
    '    '                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
    '    '                                inStaffInfo.DlrCD, _
    '    '                                inStaffInfo.BrnCD, _
    '    '                                String.Empty, _
    '    '                                inStaffInfo.Account)

    '    '基幹コード情報Row
    '    Dim rowDmsCodeMap As SC3250101DataSet.DmsCodeMapRow
    '    'Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

    '    '基幹コードへ変換処理結果チェック
    '    If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
    '        '基幹コードへ変換処理成功

    '        'Rowに変換
    '        rowDmsCodeMap = DirectCast(dtDmsCodeMap.Rows(0), SC3250101DataSet.DmsCodeMapRow)
    '        'rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

    '        '基幹アカウントチェック
    '        If rowDmsCodeMap.IsACCOUNTNull Then
    '            '値無し

    '            '空文字を設定する
    '            '基幹アカウント
    '            rowDmsCodeMap.ACCOUNT = String.Empty

    '        End If

    '        '基幹販売店コードチェック
    '        If rowDmsCodeMap.IsCODE1Null Then
    '            '値無し

    '            '空文字を設定する
    '            '基幹販売店コード
    '            rowDmsCodeMap.CODE1 = String.Empty

    '        End If

    '        '基幹店舗コードチェック
    '        If rowDmsCodeMap.IsCODE2Null Then
    '            '値無し

    '            '空文字を設定する
    '            '基幹店舗コード
    '            rowDmsCodeMap.CODE2 = String.Empty

    '        End If

    '    Else
    '        '基幹コードへ変換処理成功失敗

    '        '新しいRowを作成
    '        rowDmsCodeMap = DirectCast(dtDmsCodeMap.NewDmsCodeMapRow, SC3250101DataSet.DmsCodeMapRow)
    '        'rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

    '        '空文字を設定する
    '        '基幹アカウント
    '        rowDmsCodeMap.ACCOUNT = String.Empty
    '        '基幹販売店コード
    '        rowDmsCodeMap.CODE1 = String.Empty
    '        '基幹店舗コード
    '        rowDmsCodeMap.CODE2 = String.Empty

    '    End If


    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
    '               , Me.GetType.ToString _
    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '               , LOG_END _
    '               , dtDmsCodeMap.Count))

    '    '結果返却
    '    Return rowDmsCodeMap

    '    'End Using

    'End Function
    ''2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑
#End Region


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

        '基幹画面連携用フレーム呼出(SC3010501)
        Me.RedirectNextScreen(APPLICATIONID_FRAMEID)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region


#Region "未使用メソッド"

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 明細部の部位テーブルのデータ部を生成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>

    'Private Function GetListData(ByVal strListNo As String _
    '                           , ByVal dtListData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                           , ByVal dtDefaultData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                           , ByVal dtSuggestData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                           , ByVal dtSuggestWKData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                           ) As DataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetListData_Start, strListNo:[{0}], dtListData_Count:[{1}], dtDefaultData_Count:[{2}], dtSuggestData_Count:[{3}], dtSuggestWKData_Count:[{4}]" _
    '                              , strListNo _
    '                              , dtListData.Rows.Count.ToString _
    '                              , dtDefaultData.Rows.Count.ToString _
    '                              , dtSuggestData.Rows.Count.ToString _
    '                              , dtSuggestWKData.Rows.Count.ToString _
    '                              ))

    '    Dim dt As DataTable = CreateListDataColumns()
    '    'strListMpに対応した検査項目名を配置する
    '    dt = SetInspectionItems(strListNo, dtListData)
    '    '今回のSuggestの初期表示アイコンを配置する
    '    dt = GetDefaultSuggestData(InspecType.SUGGEST, dt, dtDefaultData)
    '    '実績テーブルから読み込んだSuggestデータを反映させる
    '    dt = GetRepairSuggestData(strListNo, dt, dtSuggestData)
    '    '一時テーブルから読み込んだSuggestデータを反映させる
    '    dt = GetRepairSuggestWKData(strListNo, dt, dtSuggestWKData)
    '    '終了ログの記録
    '    Logger.Info(String.Format("GetListData_End, Return(DataTable_Count):[{0}]", dt.Rows.Count.ToString))

    '    Return dt

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 指定した部位番号に配置される点検項目名をセットする
    ' ''' </summary>
    ' ''' <param name="strListNo">部位番号</param>
    ' ''' <param name="dtListData">全点検項目データリスト</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetInspectionItems(ByVal strListNo As String _
    '                                  , ByVal dtListData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                                  ) As DataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("SetInspectionItems_Start, strListNo:[{0}], dtListData_Count:[{1}]", strListNo, dtListData.Rows.Count))

    '    '全ての点検項目テーブルより指定した部位番号の点検項目のみ取り出す
    '    Dim rows() As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILRow _
    '        = DirectCast(dtListData.Select(String.Format("REQ_PART_CD = '{0}'", strListNo)) _
    '            , SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILRow())

    '    Dim dt As DataTable = CreateListDataColumns()
    '    Dim dr As DataRow
    '    Dim GroupTitle As String = Nothing
    '    Dim ListIndex As Integer = 1

    '    For Each row As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILRow In rows
    '        dr = dt.NewRow()
    '        dr("ListNo") = strListNo
    '        If row.INSPEC_ITEM_NAME <> row.SUB_INSPEC_ITEM_NAME Then
    '            'グループ名とサブ名が違う
    '            If GroupTitle <> row.INSPEC_ITEM_NAME Then
    '                GroupTitle = row.INSPEC_ITEM_NAME
    '                '新しいグループ
    '                dr("ItemName1") = row.INSPEC_ITEM_NAME
    '                dr("ItemName2") = String.Empty
    '                'dr("ItemName3") = String.Empty
    '                dr("Result") = String.Empty
    '                dr("ResultImage") = String.Empty
    '                dr("SUGGEST_ICON") = String.Empty
    '                dr("INSPEC_ITEM_CD") = String.Empty
    '                dr("ListIndex") = ListIndex.ToString
    '                dr("ChangeFlag") = SUGGEST_CHANGE_FLAG_OFF
    '                'dr("ServiceItem") = String.Empty
    '                ListIndex += 1
    '                dt.Rows.Add(dr)
    '                'サブグループの作成
    '                dr = dt.NewRow()
    '                dr("ListNo") = strListNo
    '                dr("ListIndex") = String.Empty
    '            End If
    '            dr("ItemName1") = row.INSPEC_ITEM_NAME
    '            dr("ItemName2") = row.SUB_INSPEC_ITEM_NAME
    '        Else
    '            'グループ名とサブ名が同じ
    '            GroupTitle = row.INSPEC_ITEM_NAME
    '            dr("ItemName1") = row.INSPEC_ITEM_NAME
    '            dr("ItemName2") = String.Empty
    '            dr("ListIndex") = ListIndex.ToString
    '            ListIndex += 1
    '        End If
    '        'dr("ItemName3") = String.Empty
    '        dr("Result") = String.Empty
    '        dr("ResultImage") = ResultImages(0)
    '        dr("SUGGEST_ICON") = DEFAULT_SUGGEST_ICON
    '        dr("INSPEC_ITEM_CD") = row.INSPEC_ITEM_CD
    '        dr("ChangeFlag") = SUGGEST_CHANGE_FLAG_OFF
    '        'dr("ServiceItem") = row.SERVICE_ITEM_CD

    '        dr("NeedIconFlg") = String.Format("{0},{1},{2},{3},{4}" _
    '                                          , row.DISP_INSPEC_ITEM_NEED_INSPEC _
    '                                          , row.DISP_INSPEC_ITEM_NEED_REPLACE _
    '                                          , row.DISP_INSPEC_ITEM_NEED_FIX _
    '                                          , row.DISP_INSPEC_ITEM_NEED_CLEAN _
    '                                          , row.DISP_INSPEC_ITEM_NEED_SWAP)
    '        'dr("NEED_INSPEC") = row.DISP_INSPEC_ITEM_NEED_INSPEC
    '        'dr("NEED_REPLACE") = row.DISP_INSPEC_ITEM_NEED_REPLACE
    '        'dr("NEED_FIX") = row.DISP_INSPEC_ITEM_NEED_FIX
    '        'dr("NEED_CLEAN") = row.DISP_INSPEC_ITEM_NEED_CLEAN
    '        'dr("NEED_SWAP") = row.DISP_INSPEC_ITEM_NEED_SWAP
    '        dr("SUGGEST_STATUS") = String.Empty

    '        dt.Rows.Add(dr)
    '    Next

    '    '終了ログの記録
    '    Logger.Info(String.Format("SetInspectionItems_End, Return(DataTable_Count):[{0}]", dt.Rows.Count))

    '    Return dt

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 実績テーブルから読み込んだSuggestデータを反映させる
    ' ''' </summary>
    ' ''' <param name="strPartInfo"></param>
    ' ''' <param name="dt"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetRepairSuggestData(ByVal strPartInfo As String _
    '                                      , ByVal dt As DataTable _
    '                                      , ByVal dtSuggestData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                                      ) As DataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestData_Start, strPartInfo:[{0}], dt_Count:[{1}], dtSuggestData_Count:[{2}]" _
    '                              , strPartInfo _
    '                              , dt.Rows.Count.ToString _
    '                              , dtSuggestData.Rows.Count.ToString))


    '    '点検項目データテーブルから点検項目内容を順番に読み込む
    '    For Each dr2 As DataRow In dt.Rows
    '        '取り出した実績データテーブルを順番に読み込む
    '        For Each dr3 As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTRow In dtSuggestData.Rows '一時テーブルより取り出したテーブル
    '            If dr2("INSPEC_ITEM_CD").ToString = dr3("INSPEC_ITEM_CD").ToString Then
    '                dr2("SUGGEST_STATUS") = DEFAULT_SUGGEST_STATUS
    '                'If CInt(dr3("SUGGEST_ICON")) < SuggestNoList.Count Then
    '                If Integer.Parse(dr3("SUGGEST_ICON").ToString) < MAX_SUGGEST_ICON_NO Then
    '                    'dr2("SUGGEST_ICON") = SuggestNoList(Integer.Parse(dr3("SUGGEST_ICON").ToString))
    '                    dr2("SUGGEST_ICON") = dr3("SUGGEST_ICON")
    '                Else
    '                    '不明な表示アイテムコードが出てきた
    '                    Logger.Info(String.Format("Unknwon SUGGEST_ICON No:[{0}]", dr3("SUGGEST_ICON").ToString))
    '                    dr2("SUGGEST_ICON") = DEFAULT_SUGGEST_ICON
    '                End If
    '            End If
    '        Next
    '    Next

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestData_End, Return(DataTable_Count):[{0}]", dt.Rows.Count))

    '    Return dt

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 一時テーブルから読み込んだSuggestデータを反映させる
    ' ''' </summary>
    ' ''' <param name="strPartInfo"></param>
    ' ''' <param name="dt"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetRepairSuggestWKData(ByVal strPartInfo As String _
    '                                        , ByVal dt As DataTable _
    '                                        , ByVal dtSuggestWKData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                                        ) As DataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestWKData_Start, strPartInfo:[{0}], dt_Count:[{1}], dtSuggestWKData_Count:[{2}]" _
    '                              , strPartInfo _
    '                              , dt.Rows.Count.ToString _
    '                              , dtSuggestWKData.Rows.Count.ToString _
    '                              ))

    '    For Each dr2 As DataRow In dt.Rows  'マスタより出したテーブル
    '        For Each dr3 As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTRow In dtSuggestWKData.Rows '一時テーブルより取り出したテーブル
    '            If dr2("INSPEC_ITEM_CD").ToString = dr3("INSPEC_ITEM_CD").ToString Then
    '                dr2("SUGGEST_STATUS") = "0"
    '                'If CInt(dr3("SUGGEST_ICON")) < SuggestNoList.Count Then
    '                If Integer.Parse(dr3("SUGGEST_ICON").ToString) < MAX_SUGGEST_ICON_NO Then
    '                    'dr2("SUGGEST_ICON") = SuggestNoList(CInt(dr3("SUGGEST_ICON")))
    '                    dr2("SUGGEST_ICON") = dr3("SUGGEST_ICON")
    '                Else
    '                    dr2("SUGGEST_ICON") = DEFAULT_SUGGEST_ICON
    '                End If
    '                dr2("ChangeFlag") = SUGGEST_CHANGE_FLAG_ON
    '                '一時ファイルに変更項目があれば「Register」ボタンを有効にする
    '                holderFotter = DirectCast(Me.Master.FindControl("footer"), ContentPlaceHolder)
    '                DirectCast(holderFotter.FindControl("imgRegister"), HtmlGenericControl).Attributes.Add("class", Register_Enable)
    '            End If
    '        Next
    '    Next

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestWKData_End, Return:[{0}]", dt.Rows.Count.ToString))

    '    Return dt

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' Suggestデータのデフォルト（今回のお勧め点検）を反映させる
    ' ''' </summary>
    ' ''' <param name="strInspecType"></param>
    ' ''' <param name="dt"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetDefaultSuggestData(ByVal strInspecType As String _
    '                                       , ByVal dt As DataTable _
    '                                       , ByVal dtDefaultData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                                       ) As DataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetDefaultSuggestData_Start, strInspecType:[{0}], dt_Count:[{1}], dtDefaultData_Count:[{2}]" _
    '                              , strInspecType _
    '                              , dt.Rows.Count.ToString _
    '                              , dtDefaultData.Rows.Count.ToString _
    '                              ))

    '    'Dim dtResultData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable             'データテーブルを作成
    '    'dtResultData = Biz.GetSuggestDefaultList(strChangeModelCode, strGradeInfo, strInspecType)

    '    For Each dr2 As DataRow In dt.Rows  'マスタより出したテーブル
    '        For Each dr3 As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILRow In dtDefaultData.Rows
    '            If dr2("INSPEC_ITEM_CD").ToString = dr3("INSPEC_ITEM_CD").ToString Then
    '                dr2("SUGGEST_STATUS") = dr3("SUGGEST_STATUS").ToString
    '                If Integer.Parse(dr3("REQ_ITEM_CD").ToString) < SuggestNoList.Count Then
    '                    dr2("SUGGEST_ICON") = SuggestNoList(Integer.Parse(dr3("REQ_ITEM_CD").ToString))
    '                Else
    '                    dr2("SUGGEST_ICON") = DEFAULT_SUGGEST_ICON
    '                End If
    '            End If
    '        Next
    '    Next

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetDefaultSuggestData_End, Return_Count:[{0}]", dt.Rows.Count.ToString))

    '    Return dt

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 明細部の部位テーブル作成
    ' ''' </summary>
    ' ''' <param name="strListNo">部位名</param>
    ' ''' <remarks></remarks>
    'Private Sub CreateList(ByVal strListNo As String _
    '                     , ByVal dtListTable As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                     , ByVal dtDefaultData As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable _
    '                     , ByVal dtSuggestData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                     , ByVal dtSuggestWKData As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable _
    '                     )

    '    '開始ログの記録
    '    Logger.Info(String.Format("CreateList_Start, strListNo:[{0}], dtListTable_Count:[{1}], dtDefaultData_Count:[{2}], dtSuggestData_Count:[{3}], dtSuggestWKData_Count:[{4}]" _
    '                              , strListNo _
    '                              , dtListTable.Rows.Count.ToString _
    '                              , dtDefaultData.Rows.Count.ToString _
    '                              , dtSuggestData.Rows.Count.ToString _
    '                              , dtSuggestWKData.Rows.Count.ToString _
    '                              ))

    '    '--ヘッダー作成
    '    Dim dtListHeader As DataTable = CreateListHeader(strListNo)
    '    ShowListHeader(dtListHeader)
    '    '--データ部作成
    '    Dim dtListData As DataTable = GetListData(strListNo, dtListTable, dtDefaultData, dtSuggestData, dtSuggestWKData)
    '    If dtListData.Rows.Count = 0 Then
    '        Exit Sub
    '    End If

    '    '登録済データであるかを設定。未登録データである場合、DB登録対象となる。
    '    'Biz.SetIsRegisted(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, hdnRO_NUM.Value, dtListHeader.Rows(0)("INSPEC_TYPE"), dtListData)

    '    '点検内容を表示する
    '    ShowListData(dtListData, strListNo)

    '    dtListHeader.Dispose()
    '    'dtListData.Dispose()

    '    '終了ログの記録
    '    Logger.Info("CreateList_End")

    'End Sub
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 部位テーブルのヘッダー部に表示内容をセット
    ' ''' </summary>
    ' ''' <param name="dttListHeaderData"></param>
    ' ''' <remarks></remarks>
    'Private Sub ShowListHeader(ByVal dttListHeaderData As DataTable)

    '    '開始ログの記録
    '    Logger.Info(String.Format("ShowListHeader_Start, dttListHeaderData_Count:[{0}]", dttListHeaderData.Rows.Count.ToString))

    '    '配置位置を変換する
    '    Dim SetNo As String = "01"
    '    'SetNo = ChangeListNo(dttListHeaderData.Rows(0)("ListNo").ToString)
    '    SetNo = dttListHeaderData.Rows(0)("ListNo").ToString

    '    '部位のイメージをセット
    '    'DirectCast(holder.FindControl(String.Format("List{0}_Col1_TitleImage", SetNo)), Image).ImageUrl = ResolveUrl(dttListHeaderData.Rows(0)("ImageUrl"))
    '    DirectCast(holder.FindControl(String.Format("TitleImage{0}", SetNo)), HtmlGenericControl).Attributes.Add("class", dttListHeaderData.Rows(0)("ImageUrl").ToString)

    '    '部位名に合わせて<BR>をつける
    '    Dim Title As String = dttListHeaderData.Rows(0)("title").ToString
    '    If 0 <= Title.IndexOf("Battery") Then
    '        '2014/05/22 文言DBから取得　START　↓↓↓
    '        If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
    '            Title = Title.Insert(Title.IndexOf("Battery"), "<br/>")
    '        End If
    '        '2014/05/22 文言DBから取得　END　　↑↑↑
    '    End If

    '    If 0 <= Title.IndexOf("System") Then
    '        '2014/05/22 文言DBから取得　START　↓↓↓
    '        If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
    '            Title = Title.Insert(Title.IndexOf("System"), "<br/>")
    '        End If
    '        '2014/05/22 文言DBから取得　END　　↑↑↑
    '    End If

    '    If 0 <= Title.IndexOf("Transmission") Then
    '        '2014/05/22 文言DBから取得　START　↓↓↓
    '        If Not Title.Contains("<br>") And Not Title.Contains("<br/>") Then
    '            Title = Title.Insert(Title.IndexOf("Transmission"), "<br/>")
    '        End If
    '        '2014/05/22 文言DBから取得　END　　↑↑↑
    '    End If

    '    '部位名をセット
    '    DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).Text = Title
    '    '1行表示 or 2行表示のクラスをセット
    '    Dim strSearchChar As String = "<br/>"
    '    'If dttListHeaderData.Rows(0)("title").IndexOf("<br/>") = -1 Then
    '    '2014/05/22 文言DBから取得　START　↓↓↓
    '    'If Title.IndexOf("<br/>") = -1 Then
    '    If Title.IndexOf("<br/>") = -1 And Title.IndexOf("<br>") = -1 Then
    '        DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_ONE_LINE
    '    Else
    '        DirectCast(holder.FindControl(String.Format("List{0}_Col1_Title", SetNo)), Label).CssClass = TITLE_TWO_LINE
    '    End If
    '    '2014/05/22 文言DBから取得　END　　↑↑↑

    '    '点検種別をセット
    '    DirectCast(holder.FindControl(String.Format("hdnSVC_CD{0}", SetNo)), HiddenField).Value = dttListHeaderData.Rows(0)("SVC_CD").ToString
    '    'Dim strInspectionType As String = DirectCast(holder.FindControl(String.Format("hdnINSPEC_TYPE{0}", SetNo)), HiddenField).Value
    '    'DirectCast(holder.FindControl(String.Format("List{0}_Col2", dttListHeaderData.Rows(0)("ListNo"))), HtmlGenericControl).InnerHtml = String.Format("Result<br/>{0}", strInspectionType)
    '    'DirectCast(holder.FindControl(String.Format("List{0}_Col3", dttListHeaderData.Rows(0)("ListNo"))), HtmlGenericControl).InnerHtml = String.Format("Suggest<br/>{0}", strInspectionType)
    '    DirectCast(holder.FindControl(String.Format("List{0}_Col2", SetNo)), HtmlGenericControl).InnerHtml = dttListHeaderData.Rows(0)("Result").ToString
    '    DirectCast(holder.FindControl(String.Format("List{0}_Col3", SetNo)), HtmlGenericControl).InnerHtml = dttListHeaderData.Rows(0)("Suggest").ToString
    '    DirectCast(holder.FindControl(String.Format("List{0}_PartName", SetNo)), HtmlTableCell).Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');", dttListHeaderData.Rows(0)("POPUP_URL").ToString))
    '    Logger.Info(String.Format("PopupURL:[{0}]", dttListHeaderData.Rows(0)("POPUP_URL").ToString))

    '    '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
    '    'DirectCast(holder.FindControl(String.Format("List{0}_ResultName", SetNo)), HtmlTableCell).Attributes.Add("onclick", "ShowROPreview();")
    '    '2014/05/27 ポップアップによるROプレビュー（過去）表示　END　　↑↑↑

    '    '終了ログの記録
    '    Logger.Info("ShowListHeader_End")

    'End Sub
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' DBから取得した部位マスタの番号を商品訴求画面用番号に変更する
    ' ''' </summary>
    ' ''' <param name="BeforeNo">DBから取得した部位マスタ番号</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ChangeListNo(ByVal BeforeNo As String) As String

    '    '開始ログ出力
    '    Logger.Info(String.Format("ChangeListNo_Start, BeforeNo:[{0}]", BeforeNo))

    '    Dim SetNo As String = "01"
    '    Select Case BeforeNo
    '        Case "01"
    '            SetNo = "01"
    '        Case "02"
    '            SetNo = "09"
    '        Case "03"
    '            SetNo = "02"
    '        Case "04"
    '            SetNo = "08"
    '        Case "05"
    '            SetNo = "03"
    '        Case "06"
    '            SetNo = "07"
    '        Case "07"
    '            SetNo = "04"
    '        Case "08"
    '            SetNo = "05"
    '        Case "09"
    '            SetNo = "06"
    '    End Select

    '    '終了ログ出力
    '    Logger.Info(String.Format("ChangeListNo_End, Return:[{0}]", SetNo))

    '    Return SetNo

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' DBから取得した部位マスタの番号を商品訴求画面用番号に変更する（拡大表示用）
    ' ''' </summary>
    ' ''' <param name="BeforeNo">DBから取得した部位マスタ番号</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ChangeListNo_PopUp(ByVal BeforeNo As String) As String

    '    '開始ログ出力
    '    Logger.Info(String.Format("ChangeListNo_PopUp_Start, BeforeNo:[{0}]", BeforeNo))

    '    Dim SetNo As String = "01"
    '    Select Case BeforeNo
    '        Case "01"
    '            SetNo = "01"
    '        Case "02"
    '            SetNo = "03"
    '        Case "03"
    '            SetNo = "05"
    '        Case "04"
    '            SetNo = "07"
    '        Case "05"
    '            SetNo = "08"
    '        Case "06"
    '            SetNo = "09"
    '        Case "07"
    '            SetNo = "06"
    '        Case "08"
    '            SetNo = "04"
    '        Case "09"
    '            SetNo = "02"
    '    End Select

    '    '終了ログ出力
    '    Logger.Info(String.Format("ChangeListNo_PopUp_End, Return:[{0}]", SetNo))

    '    Return SetNo
    'End Function
#End Region

#Region "未使用メソッド"
    'Private Sub popUpSpanCol(ByVal cell1 As HtmlTableCell, ByVal cell1_val As HyperLink, ByVal cell2 As HtmlTableCell, ByVal cell3 As HtmlTableCell)

    '    '開始ログ出力
    '    Logger.Info("popUpSpanCol_Start")

    '    If String.IsNullOrEmpty(cell2.InnerHtml) AndAlso String.IsNullOrEmpty(cell3.InnerHtml) Then
    '        cell1.ColSpan = 3
    '        cell1.Style.Add("border-right", "1px solid #C8C8C8")
    '    Else
    '        cell1.Style.Remove("border-right")
    '        cell1.ColSpan = 0
    '    End If

    '    '終了ログ出力
    '    Logger.Info("popUpSpanCol_End")

    'End Sub
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' Get渡しされたKeyVieModeがあれば値を取得する
    ' ''' </summary>
    ' ''' <param name="key"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetQueryString(ByVal key As String) As String

    '    '開始ログ出力
    '    Logger.Info(String.Format("GetQueryString_Start, key:[{0}]", key))

    '    Dim ret As String = Nothing

    '    If Request.QueryString.AllKeys.Contains(key) Then
    '        'Get渡しされたKeyVieModeがあれば値を取得する
    '        ret = Request.QueryString(key)
    '    End If

    '    '終了ログ出力
    '    Logger.Info(String.Format("GetQueryString_End, Return:[{0}]", ret))

    '    Return ret

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' R/Oプレビュー画面へ遷移する
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub ShowROPreview()

    '    '開始ログ出力
    '    Logger.Info("ShowROPreview_Start")

    '    'ResultをクリックしたときにR/Oプレビューを表示
    '    'リンク先　→　http://dmstl-dev.toyota.co.th:9082/tops/do/spad013
    '    'パラメータ　→　http://{0}/tops/do/spad013?DealerCode={1}&BranchCode={2}&LoginUserID={3}&SAChipID={4}&BASREZID={5}&R_O={6}&SEQ_NO={7}&VIN_NO={8}&ViewMode={9}&Format={10}?

    '    If ResultList.Count <= 0 Then
    '        Logger.Info(String.Format("ShowROPreview_End, ResultList.Count:[{0}]", ResultList.Count.ToString))
    '        Exit Sub
    '    End If

    '    '1	入庫管理番号利用フラグ取得	「販売店システム設定」より、「入庫管理番号利用フラグ」を取得する。


    '    '2	RO History画面引き渡しパラメータ設定	RO History画面引き渡しパラメータ設定を設定する。
    '    Dim DmsDealerCode2 As String
    '    Dim strSVCIN_NUM As String = ResultList(Integer.Parse(ddlResult.SelectedValue))("SVCIN_NUM").ToString

    '    '現在選択されているResultListからディーラーコードと入庫管理番号を入れる
    '    '基幹コードへ変換処理
    '    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
    '    Dim dmsDlrBrnRow As SC3250101DataSet.DmsCodeMapRow = Me.GetDmsBlnCd(ResultList(Integer.Parse(ddlResult.SelectedValue))("DLR_CD").ToString, String.Empty)
    '    'Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(ResultList(CInt(ddlResult.SelectedValue))("DLR_CD").ToString, String.Empty)
    '    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑
    '    If IsNothing(dmsDlrBrnRow) OrElse dmsDlrBrnRow.IsCODE1Null Then
    '        '変換失敗した場合は、変換前のコードを入れておく
    '        DmsDealerCode2 = Params.DealerCode
    '    Else
    '        '変換成功時は変換後のコードを入れておく
    '        DmsDealerCode2 = dmsDlrBrnRow.CODE1
    '    End If

    '    '販売店コード
    '    Me.SetValue(ScreenPos.Next, "DealerCode", DmsDealerCode)
    '    '店舗コード
    '    Me.SetValue(ScreenPos.Next, "BranchCode", DmsBranchCode)
    '    'アカウント
    '    Me.SetValue(ScreenPos.Next, "LoginUserID", Params.LoginUserID)
    '    '来店者実績連番
    '    Me.SetValue(ScreenPos.Next, "SAChipID", Params.SAChipID)
    '    'DMS予約ID
    '    Me.SetValue(ScreenPos.Next, "BASREZID", Params.BASREZID)
    '    'RO
    '    Me.SetValue(ScreenPos.Next, "R_O", Params.R_O)
    '    'RO_JOB_SEQ
    '    Me.SetValue(ScreenPos.Next, "SEQ_NO", Params.SEQ_NO)
    '    'VIN
    '    Me.SetValue(ScreenPos.Next, "VIN_NO", Params.VIN_NO)
    '    'ViewMode
    '    Me.SetValue(ScreenPos.Next, "ViewMode", Params.ViewMode)
    '    'ViewMode
    '    Me.SetValue(ScreenPos.Next, "Format", "0")
    '    '2.1	RO　HISTORY設定１（入庫管理番号利用フラグ」＝0）
    '    '      「入庫管理番号利用フラグ」＝0の場合、以下の書式で変換し、R/O HISTORYの引き渡しパラメータ．SVCIN_NUMへセットする。
    '    '       DMS店舗コードは設定しない。Replace( Replace( 『販売店システム設定』の設定値, "[RO_NUM]", "GSJ140035" ), "[BRN_CD]", "T01" )
    '    '2.2	RO　HISTORY設定２（入庫管理番号利用フラグ」＝１）
    '    '      「入庫管理番号利用フラグ」＝１の場合、入庫管理番号に空文字、R/O番号、DMS販売店コードDMS店舗コードを設定する。
    '    'ViewMode
    '    Me.SetValue(ScreenPos.Next, "SVCIN_NUM", strSVCIN_NUM)
    '    'ViewMode
    '    Me.SetValue(ScreenPos.Next, "SVCIN_DealerCode", DmsDealerCode2)

    '    Logger.Info(String.Format("SVCIN_NUM:{0}, SVCIN_DealerCode:[{1}]", strSVCIN_NUM, DmsDealerCode2))
    '    Logger.Info("ShowROPreview_End")

    '    '3	画面遷移処理	R/Oプレビュ-画面に遷移する。
    '    '基幹画面連携用フレーム呼出処理
    '    Me.RedirectNextScreen("SC3160208")

    'End Sub
#End Region

#End Region

End Class
