'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3140103BusinessLogic.vb
'─────────────────────────────────────
'機能： メインメニュー(SA) ビジネスロジック
'補足： 
'作成： 2012/01/16 KN   小林
'更新:  2012/03/19 KN   森下  【SERVICE_1】仕様変更対応(顧客情報の表示優先順位)
'更新： 2012/03/21 KN   上田  【SERVICE_1】仕様変更対応(追加作業関連の遷移先変更)
'更新： 2012/04/04 KN   西田  【SERVICE_1】プレユーザーテスト課題 No.91 作業予定の無い場合、作業完了予定時刻の表示を「00'00」対応
'更新： 2012/04/05 KN   西田  【SERVICE_1】企画_プレユーザーテスト課題 No.31 追加作業承認一覧APIの引数対応
'更新： 2012/04/10 KN   森下  【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応
'更新： 2012/04/10 KN   森下  【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応
'更新： 2012/04/10 KN   森下  【SERVICE_1】企画_プレユーザーテスト課題 No.227 チップ詳細の車名を表示するための項目を変更
'更新： 2012/04/10 KN   森下  【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更
'更新： 2012/04/17 KN   西田  【SERVICE_1】R/O作成のAPIに渡すVINとRegistryNoはブランクの場合、" "に変換
'更新： 2012/04/18 KN   上田  【SERVICE_1】ユーザテスト課題 No.76 予約無し時の洗車フラグの初期値を"0"⇒空白に修正
'更新： 2012/06/18 KN   西岡  【SERVICE_2】事前準備対応
'更新： 2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加)
'更新： 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
'更新： 2012/08/22 TMEJ 日比野【SERVICE_2】チップに代表整備項目が表示されないことがある
'更新： 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
'更新： 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発
'更新： 2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応
'更新： 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
'更新： 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34）
'更新： 2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
'更新： 2013/01/10 TMEJ 小澤  【SERVICE_2】次世代サービスROステータス切り離し対応
'更新： 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新： 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新： 2015/08/17 TMEJ 井上 問連対応「TR-SVT-20150721-002(SvR権限への通知処理)」
'更新： 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
'更新： 2017/01/19 NSK  加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
'更新： 2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web

Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSetTableAdapters

Imports Toyota.eCRB.iCROP.BizLogic.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSetTableAdapters

Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic

Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic.IC3810701BusinessLogic
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSetTableAdapters
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic

Imports Toyota.eCRB.SMBLinkage.Reservation.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.DataAccess

'通知用
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.BizLogic

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic.IC3800709BusinessLogic
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess

Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801102
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102.IC3801102DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102.IC3801102TableAdapters

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001.IC3801001DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001.IC3801001TableAdapter

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801002
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002.IC3801002DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002.IC3801002DataSetTableAdapters.IC3801002DataSetTableAdapter

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801003
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003TableAdapter

'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSetTableAdapters

''2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
''2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

'2012/05/31 西岡 STEP2事前準備追加対応 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801012
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801012

'2012/05/31 西岡 STEP2事前準備追加対応 END
'2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800706
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800706
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800707
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800707
'2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
'2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START

'2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.BizLogic.IC3801901
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.DataAccess.IC3801901
'2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

'2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
#Region "列挙体定義"
''' <summary>
''' 追加作業関連画面列挙体
''' </summary>
''' <remarks></remarks>
Public Enum AddWorkRedirect
    ''' <summary>追加作業一覧</summary>
    SC3170101
    ''' <summary>追加作業入力(新規)</summary>
    SC3170201New
    ''' <summary>追加作業入力(編集)</summary>
    SC3170201Edit
    ''' <summary>追加作業入力(代行)</summary>
    SC3170203Acting
    ''' <summary>追加作業入力(参照)</summary>
    SC3170203Preview
    ''' <summary>追加作業プレビュー</summary>
    SC3170302
    ''' <summary>対象外</summary>
    Invalid
End Enum
#End Region
'2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

''' <summary>
''' SC3140103
''' </summary>
''' <remarks></remarks>
Public Class SC3140103BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable


#Region "定数定義"

    ' 表示区分
    Public Const DisplayDivNone As String = "0"            ' なし
    Public Const DisplayDivReception As String = "1"       ' 受付
    Public Const DisplayDivApproval As String = "2"        ' 承認依頼
    Public Const DisplayDivPreparation As String = "3"     ' 納車準備
    Public Const DisplayDivDelivery As String = "4"        ' 納車作業
    Public Const DisplayDivWork As String = "5"            ' 作業中
    Public Const DisplayDivAdvance As String = "6"         ' 事前準備
    Public Const DisplayDivAssignment As String = "7"      ' 振当待ち

    ' 仕掛中
    Public Const DisplayStartNone As String = "0"          ' 仕掛前
    Public Const DisplayStartStart As String = "1"         ' 仕掛中

    ' チップ詳細ボタン
    Public Const ButtonNone As String = "0"              ' なし
    Public Const ButtonCustomer As String = "1"          ' 顧客詳細ボタン
    Public Const ButtonNewCustomer As String = "2"       ' 新規顧客登録ボタン
    Public Const ButtonNewRO As String = "3"             ' R/O作成ボタン
    Public Const ButtonRODisplay As String = "4"         ' R/O参照ボタン
    Public Const ButtonWork As String = "5"              ' 追加作業登録ボタン
    Public Const ButtonApproval As String = "6"          ' 追加承認ボタン
    Public Const ButtonCheckSheet As String = "7"        ' チェックシートボタン
    Public Const ButtonSettlement As String = "8"        ' 清算入力ボタン

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    Public Const ButtonWorkPreview As String = "9"       ' 追加作業プレビューボタン
    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    ' ストール予定；洗車フラグ
    Private Const C_STALLREZINFO_WASH_ON As String = "1"    ' あり
    ' ストール予定；来店フラグ
    Private Const C_STALLREZINFO_WALKIN_REZ As String = "0"     ' 予約

    ' ストール実績：実績ステータス
    Private Const C_STALLPROSESS_NONE As String = "00"          ' 未入庫
    Private Const C_STALLPROSESS_CAR_IN As String = "10"        ' 入庫
    Private Const C_STALLPROSESS_WORKING As String = "20"       ' 作業中
    Private Const C_STALLPROSESS_ITEM_NONE As String = "30"     ' 部品欠品
    Private Const C_STALLPROSESS_CUST_WAIT As String = "31"     ' お客様連絡待ち
    Private Const C_STALLPROSESS_STALL_WAIT As String = "38"    ' ストール待機
    Private Const C_STALLPROSESS_ETC As String = "39"           ' その他
    Private Const C_STALLPROSESS_WASH_WAIT As String = "40"     ' 洗車待ち
    Private Const C_STALLPROSESS_WASH_DOING As String = "41"    ' 洗車中
    Private Const C_STALLPROSESS_INSP_WAIT As String = "42"     ' 検査待ち
    Private Const C_STALLPROSESS_INSP_DOING As String = "43"    ' 検査中
    Private Const C_STALLPROSESS_INSP_NG As String = "44"       ' 検査不合格
    Private Const C_STALLPROSESS_CARRY_WAIT As String = "50"    ' 預かり中
    Private Const C_STALLPROSESS_DELI_WAIT As String = "60"     ' 納車待ち
    Private Const C_STALLPROSESS_PFINISH As String = "97"       ' 関連チップの前工程作業終了
    Private Const C_STALLPROSESS_MFINISH As String = "98"       ' MidFinish
    Private Const C_STALLPROSESS_COMPLETE As String = "99"      ' 完了

    ' 追加作業承認
    Private Const C_APPROVAL_STATUS_ON As String = "1"      ' あり   
    ' 追加作業承認印刷
    Private Const C_APPROVAL_OUTPUT_ON As String = "1"      ' あり   

    ' R/O作成画面表示(API)
    Private Const C_RO_CREATE_STATUS_OK As String = "1"     ' 表示
    ' 完成検査有無(API)
    Private Const C_COMP_INS_FLAG_ON As String = "1"        ' あり
    ' チェックシート有無(API)
    Private Const C_CHECKSHEET_FLAG_ON As String = "1"      ' あり(印刷済み)
    ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 START
    Private Const C_NONE_CHECKSHEET_FLAG_ON As String = "2" ' あり(未印刷)
    ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 END
    ' チェックシート印刷(API)
    Private Const C_CHECKSHET_OUTPUT_OK As String = "1"     ' 印刷
    ' 精算書発行(API)
    Private Const C_SETTLEMENT_OUTPUT_OK As String = "1"    ' 印刷
    ' 顧客区分(API)
    Public Const CustomerSegmentON As String = "1"           ' 自社客

    ' R/Oステータス(API)
    Private Const C_RO_STATUS_NONE As String = "0"          ' なし
    Private Const C_RO_STATUS_RECEPTION As String = "1"     ' 受付
    Private Const C_RO_STATUS_WORKING As String = "2"       ' 作業中
    Private Const C_RO_STATUS_ITEM_WAIT As String = "4"     ' 部品待ち
    Private Const C_RO_STATUS_ESTI_WAIT As String = "5"     ' 見積確認待ち
    Private Const C_RO_STATUS_INSP_OK As String = "7"       ' 検査完了
    Private Const C_RO_STATUS_SALE_OK As String = "3"       ' 売上済み
    Private Const C_RO_STATUS_MANT_OK As String = "6"       ' 整備完了
    Private Const C_RO_STATUS_FINISH As String = "8"        ' 納車完了

    ' フラグ無し
    Private Const C_FLAG_OFF = "0"                          ' フラグなし
    Private Const C_FLAG_ON = "1"                           ' フラグあり

    ' 作業区分
    Private Const C_WORK_NONE As String = "0"       ' 作業なし
    Private Const C_WORK_WAIT As String = "1"       ' 作業前
    Private Const C_WORK_START As String = "2"      ' 作業中
    Private Const C_WORK_END As String = "3"        ' 作業完了

    ' 画面ID
    Private Const MAINMENUID As String = "SC3140103"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_SUCCESS As Long = 0
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DBTIMEOUT As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_NOMATCH As Long = 902
    ''' <summary>
    ''' エラー:更新失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DBERROR As Long = 903
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DIFFSACODE As Long = 1
    ''' <summary>
    ''' エラー:整備受注No作成で異常発生
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_CREATEORDERNO_ERROR As Long = 1

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

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    ''' <summary>起票者(TC)</summary>
    Private Const DRAWER_TC As String = "1"
    ''' <summary>起票者(SA)</summary>
    Private Const DRAWER_SA As String = "2"
    ''' <summary>追加作業ステータス(1: TC起票中)</summary>
    Private Const ADD_WORK_STATUS_TC_TACTING As String = "1"
    ''' <summary>追加作業ステータス(2: CT承認待ち)</summary>
    Private Const ADD_WORK_STATUS_CT_APPROVAL As String = "2"
    ''' <summary>追加作業ステータス(3: PS部品見積待ち)</summary>
    Private Const ADD_WORK_STATUS_PS_PARTS_ESTIMATE_WAIT As String = "3"
    ''' <summary>追加作業ステータス(4: SA見積確定待ち)</summary>
    Private Const ADD_WORK_STATUS_SA_ESTIMATE_WAIT As String = "4"
    ''' <summary>追加作業ステータス(5: 顧客承認待ち)</summary>
    Private Const ADD_WORK_STATUS_CUSTOMER_APPROVAL_WAIT As String = "5"
    ''' <summary>追加作業ステータス(6: CT着工指示/PS部品出荷待ち)</summary>
    Private Const ADD_WORK_STATUS_CT_WORK_INSTRUCTION As String = "6"
    ''' <summary>追加作業ステータス(7: TC作業開始待ち)</summary>
    Private Const ADD_WORK_STATUS_TC_WORK_WAIT As String = "7"
    ''' <summary>追加作業ステータス(8: 整備中)</summary>
    Private Const ADD_WORK_STATUS_WORK As String = "8"
    ''' <summary>追加作業ステータス(9: 完成検査完了)</summary>
    Private Const ADD_WORK_STATUS_INSPECTION_COMPLETION As String = "9"
    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '2012/05/31 西岡 STEP2事前準備追加対応 START
    Private Const PreparationMiddle As String = "1"
    Private Const PreparationEnd As String = "5"
    '2012/05/31 西岡 STEP2事前準備追加対応 END

    '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    ''' <summary>実績有無:実績あり</summary>
    Private Const C_RESULT_TYPE_TRUE As String = "1"
    ''' <summary>実績有無:実績なし</summary>
    Private Const C_RESULT_TYPE_FALSE As String = "1"
    '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConstFacePictureUploadUrl As String = "FACEPIC_UPLOADURL"

    ''' <summary>画像登録なし時のアイコン</summary>
    Private Const NO_IMAGE_ICON As String = "../Styles/Images/Nnsc05-01Portraits01.png"

    ''' <summary>顧客付替え前確認結果</summary>
    Public Const ChangeResultTrue As Long = 0
    Public Const ChangeReusltCheck As Long = 101
    Public Const ChangeResultBeforeHold As Long = 201
    Public Const ChangeResultAfterHold As Long = 202
    Public Const ChangeResultDifference As Long = 203
    Public Const ChangeResultApproval As Long = 204
    Public Const ChangeResultErr As Long = 999
    ''' <summary>振当ステータス</summary>
    Private Const ASSIGN_STATUS_GUIDANCE As String = "0"
    Private Const ASSIGN_STATUS_RECEPTION As String = "1"
    Private Const ASSIGN_STATUS_ASSIGN_FINISH As String = "2"
    Private Const ASSIGN_STATUS_HOLD As String = "9"
    ''' <summary>DB更新結果</summary>
    Public Const ResultSuccess As Long = 0
    Public Const ResultTimeout As Long = 901
    Public Const ResultDBError As Long = 902
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    Public Const ResultDBExclusion As Long = 903
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    ''' <summary>事前準備フラグ</summary>
    Private Const RESERVE_FLAG_TRUE As String = "0"
    Private Const RESERVE_FLAG_RESERVE As String = "1"
    ''' <summary>来店連番、予約IDのデフォルト値</summary>
    Private Const DEFAULT_LONG_VALUE As Long = -1
    ''' <summary>入庫日時のデフォルト値</summary>
    Private Const DEFAULT_STOCKTIME_VALUE As String = "190001010000"
    ''' <summary>検索標準読み込み数</summary>
    Private Const DEFAULT_READ_COUNT As String = "SC3140103_DEFAULT_READ_COUNT"
    ''' <summary>検索最大表示数</summary>
    Private Const MAX_DISPLAY_COUNT As String = "SC3140103_MAX_DISPLAY_COUNT"

    Private Const DateFormateYYYYMMDDHHMM As String = "yyyyMMddHHmm"
    '2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END
    '2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    Private Const CONVERTDATE_MD As Integer = 11        'DateTimeFuncにて、"MM/dd"形式にコンバートするための定数
    Private Const CONVERTDATE_HM As Integer = 14        'DateTimeFuncにて、"hh:mm"形式にコンバートするための定数
    '2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    ''' <summary>
    ''' 権限コード：受付待モニター
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeRwm As Integer = 60
    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END


    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

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
    ''' 事前準備フラグ(本R/O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrder As String = "0"

    ''' <summary>
    ''' 顧客検索Sortフラグ("0"：VehRegNo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortRegistNo As String = "0"

    ''' <summary>
    ''' 顧客検索Sortフラグ("1"：CustomerName)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortCustomerName As String = "1"

    ''' <summary>
    ''' 顧客検索タイプ("0"：完全一致)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ExactMatch As String = "0"

    ''' <summary>
    ''' 顧客検索タイプ("1"：前方一致)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ForwardMatch As String = "1"

    ''' <summary>
    ''' 顧客検索タイプ("2"：後方一致)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BackwordMatch As String = "2"

    ''' <summary>
    ''' 顧客種別("1"：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustsegmentMyCustomer As String = "1"

    ''' <summary>
    ''' 顧客種別("2"：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustsegmentNewCustomer As String = "2"

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
        ''' <summary>事前準備</summary>
        AdvancePreparations
        ''' <summary>振当待ち</summary>
        Assignment

    End Enum

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

    ''' <summary>
    ''' 敬称利用区分("1"：後方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBack As String = "1"

    ''' <summary>
    ''' 敬称利用区分("2"：前方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeFront As String = "2"

    ''' <summary>
    ''' 基幹顧客用＠
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ATmark As String = "@"

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
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID

        ''' <summary>通知用文言("109"：退店)</summary>
        id109 = 109
        ''' <summary>通知用文言("110"：振当てキャンセル)</summary>
        id110 = 110
        ''' <summary>通知用文言("111"：お客様)</summary>
        id111 = 111
        ''' <summary>通知用文言("112"：～)</summary>
        id112 = 112

    End Enum

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END 



#End Region

    '2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
#Region "文言ID定数 "

    ''' <summary>
    ''' 文言ID（-）
    ''' </summary>
    Private Const WordIdHyphen As Integer = 91

    ''' <summary>
    ''' 文言ID（未作成）
    ''' </summary>
    Private Const WordIdUnCreated As Integer = 92

    ''' <summary>
    ''' 文言ID（作成中）
    ''' </summary>
    Private Const WordIdUnderCreation As Integer = 93

    ''' <summary>
    ''' ページID
    ''' </summary>
    Private Const WordIdPageId As String = "SC3140103"

#End Region
    '2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

#Region "変数定義"

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    '' 表示区分
    'Private dispDiv As String
    '' 仕掛中
    'Private dispStart As String

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    ' チップ詳細ボタン関連
    Private buttonLeft As String
    Private buttonRight As String
    Private buttonEnabledLeft As Boolean
    Private buttonEnabledRight As Boolean

    ' 2012/06/08 日比野 事前準備対応 START
    Public ReadOnly Property GetButtonLeft() As String
        Get
            Return buttonLeft
        End Get
    End Property

    Public ReadOnly Property GetButtonRight() As String
        Get
            Return buttonRight
        End Get
    End Property

    Public ReadOnly Property GetButtonEnabledLeft() As Boolean
        Get
            Return buttonEnabledLeft
        End Get
    End Property

    Public ReadOnly Property GetButtonEnabledRight() As Boolean
        Get
            Return buttonEnabledRight
        End Get
    End Property
    ' 2012/06/08 日比野 事前準備対応 END

    ' 納車準備_異常表示標準時間（分）
    Private deliveryPreAbnormalLT As Long

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' 現在時刻
    'Private nowDate As Date

    '' R/Oステータス
    'Private orderStatus As String

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

#End Region

#Region "コンストラクタ"

    '''-------------------------------------------------------
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New()
        Me.deliveryPreAbnormalLT = 0

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        'Me.nowDate = DateTime.MinValue

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    End Sub

    '''-------------------------------------------------------
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="lngDeliveryPreAbnormalLt">納車準備_異常表示標準時間（分）</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New(ByVal lngDeliveryPreAbnormalLT As Long)
        Me.deliveryPreAbnormalLT = lngDeliveryPreAbnormalLT

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
        'Me.nowDate = nowDate
        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END
    End Sub

#End Region

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
    '(GL版となりモジュール一新のため一度全てコメントアウト)

    '#Region " サービス来店実績取得"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 来店チップ情報取得
    '    ''' </summary>
    '    ''' <returns>来店チップデータセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Function GetVisitChip() As SC3140103VisitChipDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim dtChip As SC3140103VisitChipDataTable = New SC3140103VisitChipDataTable
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        Dim dt As SC3140103VisitDataTable
    '        '外部IF
    '        Dim dtIFGetNoDeliveryROList As IC3801003DataSet.IC3801003NoDeliveryRODataTable
    '        Dim dtIFConfirmAddList As IC3801002DataSet.ConfirmAddListDataTable

    '        Dim dtService As SC3140103ServiceVisitManagementDataTable
    '        Dim dtRezinfo As SC3140103StallRezinfoDataTable
    '        Dim dtProcess As SC3140103StallProcessDataTable

    '        Using da As New SC3140103DataTableAdapter
    '            'IF検索処理
    '            ' SA別未納者R/O一覧
    '            dtIFGetNoDeliveryROList = Me.GetIFNoDeliveryROList(staffInfo)
    '            ' 追加承認待ち情報取得
    '            dtIFConfirmAddList = Me.GetIFApprovalConfirmAddList(staffInfo)

    '            '検索処理
    '            dtService = da.GetVisitManagement(staffInfo.DlrCD, _
    '                                              staffInfo.BrnCD, _
    '                                              staffInfo.Account, _
    '                                              dtIFGetNoDeliveryROList, _
    '                                              Me.nowDate)

    '            dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, _
    '                                                      staffInfo.BrnCD, _
    '                                                      dtService)

    '            dtProcess = da.GetStallProcess(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
    '        End Using

    '        ' サービス来店実績・ストール予約実績取得
    '        dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)
    '        ' IFマージ処理
    '        dt = Me.SetVisitMargin(dt, dtIFGetNoDeliveryROList)

    '        Dim dtmRezDeliDate As DateTime
    '        Dim rowChip As SC3140103VisitChipRow
    '        Dim rowChipApproval As SC3140103VisitChipRow

    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '        Dim smbCommonBis As New SMBCommonClassBusinessLogic
    '        Dim initCommonResult = smbCommonBis.InitCommon(staffInfo.DlrCD, _
    '                                                       staffInfo.BrnCD, _
    '                                                       DateTimeFunc.Now(staffInfo.DlrCD))

    '        If initCommonResult = 901 Then
    '            Throw New TimeoutException("TimeOut", New OracleExceptionEx())
    '        End If
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ' チップ情報チェック
    '        For Each row As SC3140103VisitRow In dt.Rows
    '            rowChip = DirectCast(dtChip.NewRow(), SC3140103VisitChipRow)

    '            ' チップ状態チェック
    '            Me.CheckChipStatus(row)
    '            If Me.dispDiv.Equals(DisplayDivNone) Then
    '                ' 削除
    '                Continue For
    '            End If

    '            ' 納車予定日時日付変換
    '            If Not String.IsNullOrEmpty(row.REZ_DELI_DATE.Trim) Then
    '                dtmRezDeliDate = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, row.REZ_DELI_DATE)
    '            Else
    '                ' 納車予定日日時がない場合
    '                If Not Me.IsDateTimeNull(row.ENDTIME) Then
    '                    ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
    '                    dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.deliveryPreAbnormalLT)
    '                Else
    '                    dtmRezDeliDate = DateTime.MinValue
    '                End If
    '            End If

    '            ' チップ情報形成
    '            rowChip = Me.GetRowChip(row, rowChip, dtmRezDeliDate, smbCommonBis, initCommonResult, nowDate)

    '            ' 行追加
    '            dtChip.AddSC3140103VisitChipRow(rowChip)

    '            ' 追加承認チェック
    '            For Each rowIFApproval As IC3801002DataSet.ConfirmAddListRow _
    '                In dtIFConfirmAddList.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                                           "ORDERNO = '{0}'", _
    '                                                           row.ORDERNO))

    '                rowChipApproval = DirectCast(dtChip.NewRow(), SC3140103VisitChipRow)

    '                '追加承認待ち情報形成
    '                rowChipApproval = Me.GetRowChipApporoval(row, _
    '                                                         dtmRezDeliDate, _
    '                                                         rowChip, _
    '                                                         rowChipApproval, _
    '                                                         rowIFApproval)

    '                ' 行追加
    '                dtChip.AddSC3140103VisitChipRow(rowChipApproval)
    '            Next
    '        Next

    '        ' 全チップ情報をログ出力
    '        Me.OutPutIFLog(dtChip, "SC3140103VisitChipDataTable")

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dtChip
    '    End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '    ' '''-------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' 来店チップ詳細情報取得
    '    ' ''' </summary>
    '    ' ''' <param name="visitSeq">来店実績連番</param>
    '    ' ''' <param name="displayDiv">表示区分</param>
    '    ' ''' <returns>来店チップ詳細データセット</returns>
    '    ' ''' <remarks></remarks>
    '    ' '''-------------------------------------------------------
    '    'Public Function GetVisitChipDetail(ByVal visitSeq As Long, ByVal displayDiv As String) As SC3140103VisitChipDetailDataTable

    '    '    '開始ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '                            , "{0}.{1} {2} IN:visitSeq = {3}, displayDiv = {4}" _
    '    '                            , Me.GetType.ToString _
    '    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '                            , LOG_START _
    '    '                            , visitSeq _
    '    '                            , displayDiv))

    '    '    Dim dtChip As SC3140103VisitChipDetailDataTable = New SC3140103VisitChipDetailDataTable
    '    '    Dim staffInfo As StaffContext = StaffContext.Current

    '    '    Dim dt As SC3140103VisitDataTable
    '    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '    '    ''外部IF
    '    '    'Dim dtIFOrderCommon As IC3801001OrderCommDataTable
    '    '    '' 顧客参照
    '    '    'Dim dtIFSrvCustomerDataTable As IC3800703SrvCustomerDataTable
    '    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END

    '    '    Dim dtService As SC3140103ServiceVisitManagementDataTable
    '    '    Dim dtRezinfo As SC3140103StallRezinfoDataTable
    '    '    Dim dtProcess As SC3140103StallProcessDataTable

    '    '    Using da As New SC3140103DataTableAdapter
    '    '        '検索処理
    '    '        dtService = da.GetVisitManagement(visitSeq)
    '    '        dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
    '    '        dtProcess = da.GetStallProcessDetail(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
    '    '    End Using

    '    '    '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '    Me.orderStatus = String.Empty
    '    '    '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '    '    ' サービス来店実績・ストール予約実績取得
    '    '    '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START
    '    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '    '    'dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)
    '    '    'dt = Me.SetVisitDetail(dtService, dtRezinfo, dtProcess, staffInfo)
    '    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '    '    dt = Me.SetVisitDetail(dtService, dtRezinfo, dtProcess, staffInfo, displayDiv)
    '    '    '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 END

    '    '    ' チップ情報詳細チェック
    '    '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

    '    '        Dim row As SC3140103VisitRow = DirectCast(dt.Rows(0), SC3140103VisitRow)
    '    '        Dim rowChip As SC3140103VisitChipDetailRow = DirectCast(dtChip.NewRow(), SC3140103VisitChipDetailRow)

    '    '        '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '    '        '' IF処理確認
    '    '        'If Not String.IsNullOrEmpty(row.ORDERNO) Then
    '    '        '    ' IF-R/O基本情報参照処理
    '    '        '    dtIFOrderCommon = Me.GetIFROBaseInformationList(staffInfo, row.ORDERNO)
    '    '        '    ' IFマージ処理
    '    '        '    row = Me.SetVisitDetailOrderMargin(row, dtIFOrderCommon)

    '    '        '    '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '        '    If dtIFOrderCommon IsNot Nothing AndAlso dtIFOrderCommon.Rows.Count > 0 Then
    '    '        '        orderStatus = dtIFOrderCommon(0).OrderStatus
    '    '        '    End If
    '    '        '    '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END
    '    '        '    '2012/03/13 KN 森下 半角スペースデータの対応 START
    '    '        '    'ElseIf String.IsNullOrEmpty(row.ORDERNO) And Not String.IsNullOrEmpty(row.VIN) Then
    '    '        'ElseIf String.IsNullOrEmpty(row.ORDERNO) And Not String.IsNullOrEmpty(row.VIN.Trim()) Then
    '    '        '    '2012/03/13 KN 森下 半角スペースデータの対応 END
    '    '        '    ' IF-顧客参照処理
    '    '        '    dtIFSrvCustomerDataTable = Me.GetIFCustomerInformation(row)
    '    '        '    ' IFマージ処理
    '    '        '    row = Me.SetVisitDetailCustomerMargin(row, dtIFSrvCustomerDataTable)
    '    '        'End If
    '    '        '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END

    '    '        ' 来店実績連番
    '    '        rowChip.VISITSEQ = row.VISITSEQ
    '    '        ' 販売店コード
    '    '        rowChip.DLRCD = row.DLRCD
    '    '        ' 店舗コード
    '    '        rowChip.STRCD = row.STRCD
    '    '        ' 予約ID
    '    '        rowChip.FREZID = row.FREZID
    '    '        ' 表示区分
    '    '        rowChip.DISP_DIV = displayDiv
    '    '        ' VIPマーク
    '    '        rowChip.VIP_MARK = row.VIP_MARK
    '    '        ' 予約マーク
    '    '        rowChip.REZ_MARK = row.REZ_MARK
    '    '        ' JDP調査対象客マーク
    '    '        rowChip.JDP_MARK = row.JDP_MARK
    '    '        ' 技術情報マーク
    '    '        rowChip.SSC_MARK = row.SSC_MARK
    '    '        ' 登録番号
    '    '        rowChip.VCLREGNO = row.VCLREGNO
    '    '        ' 車種
    '    '        rowChip.VEHICLENAME = row.VEHICLENAME
    '    '        ' モデル
    '    '        rowChip.MODELCODE = row.MODELCODE
    '    '        '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '    '        ' グレード
    '    '        rowChip.GRADE = row.GRADE
    '    '        '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '    '        ' VIN
    '    '        rowChip.VIN = row.VIN
    '    '        ' 走行距離
    '    '        rowChip.MILEAGE = row.MILEAGE
    '    '        ' 顧客名
    '    '        rowChip.CUSTOMERNAME = row.CUSTOMERNAME
    '    '        ' 電話番号
    '    '        rowChip.TELNO = row.TELNO
    '    '        ' 携帯番号
    '    '        rowChip.MOBILE = row.MOBILE
    '    '        ' 代表入庫項目
    '    '        rowChip.MERCHANDISENAME = row.MERCHANDISENAME
    '    '        ' 来店時刻
    '    '        rowChip.VISITTIMESTAMP = row.VISITTIMESTAMP
    '    '        ' 作業開始
    '    '        rowChip.ACTUAL_STIME = row.ACTUAL_STIME
    '    '        ' 作業終了予定時刻
    '    '        rowChip.ENDTIME = row.ENDTIME

    '    '        ' 納車予定日時日付変換
    '    '        Dim dtmRezDeliDate As DateTime
    '    '        If Not String.IsNullOrEmpty(row.REZ_DELI_DATE) Then
    '    '            dtmRezDeliDate = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, row.REZ_DELI_DATE)
    '    '        Else
    '    '            ' 納車予定日日時がない場合
    '    '            If Not Me.IsDateTimeNull(row.ENDTIME) Then
    '    '                ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
    '    '                dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.deliveryPreAbnormalLT)
    '    '            Else
    '    '                dtmRezDeliDate = DateTime.MinValue
    '    '            End If
    '    '        End If
    '    '        ' 納車予定日時
    '    '        rowChip.REZ_DELI_DATE = dtmRezDeliDate

    '    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '    '        ' 納車日
    '    '        rowChip.DELIVERDATE = row.DELIVERDATE
    '    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END

    '    '        '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '        ' チップ詳細ボタンチェック
    '    '        Me.CheckChipDetailButton(row.CUSTSEGMENT, displayDiv, Me.orderStatus, row.ORDERNO, row.CHECKSHEET_FLAG)
    '    '        '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '    '        ' 左ボタン
    '    '        rowChip.BUTTON_LEFT = Me.buttonLeft
    '    '        rowChip.BUTTON_ENABLED_LEFT = Me.buttonEnabledLeft
    '    '        ' 右ボタン
    '    '        rowChip.BUTTON_RIGHT = Me.buttonRight
    '    '        rowChip.BUTTON_ENABLED_RIGHT = Me.buttonEnabledRight

    '    '        ' 行追加
    '    '        dtChip.AddSC3140103VisitChipDetailRow(rowChip)

    '    '    End If

    '    '    '終了ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '                            , "{0}.{1} {2}" _
    '    '                            , Me.GetType.ToString _
    '    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '                            , LOG_END))

    '    '    '処理結果返却
    '    '    Return dtChip
    '    'End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店実績マージ(チップ情報)
    '    ''' </summary>
    '    ''' <param name="dt">サービス来店情報</param>
    '    ''' <param name="dtIFNoDeliveryRO">SA別未納者R/O一覧</param>
    '    ''' <returns>来店チップデータセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetVisitMargin(ByVal dt As SC3140103VisitDataTable, ByVal dtIFNoDeliveryRO As IC3801003NoDeliveryRODataTable) As SC3140103VisitDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim rowNoDeliveryRO As IC3801003NoDeliveryRORow
    '        Dim aryRow As DataRow()

    '        For Each row As SC3140103VisitRow In dt.Rows

    '            aryRow = dtIFNoDeliveryRO.Select(String.Format(CultureInfo.CurrentCulture, "ORDERNO = '{0}'", row.ORDERNO))

    '            If aryRow IsNot Nothing AndAlso aryRow.Length > 0 Then
    '                rowNoDeliveryRO = DirectCast(aryRow(0), IC3801003NoDeliveryRORow)

    '                If Not IsDBNull(rowNoDeliveryRO.Item("ORDERNO")) Then
    '                    row.ORDERNO = Me.SetReplaceString(row.ORDERNO, rowNoDeliveryRO.ORDERNO)                                'R/O No
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("ORDERSTATUS")) Then
    '                    row.RO_STATUS = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.ORDERSTATUS)                         'R/Oステータス
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("IFLAG")) Then
    '                    row.JDP_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.IFLAG)                                'JDP調査対象客フラグ
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("SFLAG")) Then
    '                    row.SSC_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.SFLAG)                                'SSCフラグ
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("CUSTOMERNAME")) Then
    '                    row.CUSTOMERNAME = Me.SetReplaceString(row.CUSTOMERNAME, rowNoDeliveryRO.CUSTOMERNAME)                 '顧客名
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("REGISTERNO")) Then
    '                    row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowNoDeliveryRO.REGISTERNO)                           '車両登録No.
    '                End If
    '                If Not IsDBNull(rowNoDeliveryRO.Item("ADDSRVCOUNT")) Then
    '                    ' データ先空白チェック
    '                    If Not String.IsNullOrEmpty(rowNoDeliveryRO.ADDSRVCOUNT) Then
    '                        ' データ先あり
    '                        row.APPROVAL_COUNT = CType(rowNoDeliveryRO.ADDSRVCOUNT, Long)                                       '追加作業数
    '                    End If
    '                End If
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '                If Not IsDBNull(rowNoDeliveryRO.Item("DELIVERYHOPEDATE")) Then
    '                    '納車予定時間はR/O情報を優先に表示
    '                    ' データ先空白チェック
    '                    If Not String.IsNullOrEmpty(rowNoDeliveryRO.DELIVERYHOPEDATE) Then
    '                        ' データ先あり
    '                        Dim deliveryHopeDate = CType(rowNoDeliveryRO.DELIVERYHOPEDATE.ToString(CultureInfo.CurrentCulture), DateTime).ToString(DateFormateYYYYMMDDHHMM, CultureInfo.CurrentCulture)
    '                        row.REZ_DELI_DATE = Me.SetReplaceString(deliveryHopeDate, row.REZ_DELI_DATE)            '納車予定時刻
    '                    End If

    '                End If
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END

    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '                If Not rowNoDeliveryRO.IsCOLSINGPRINTTIMENull Then
    '                    row.COLSINGPRINTTIME = rowNoDeliveryRO.COLSINGPRINTTIME             '清算書印刷時刻
    '                Else
    '                    row.COLSINGPRINTTIME = Date.MinValue
    '                End If

    '                If Not rowNoDeliveryRO.IsEXAMINETIMENull Then
    '                    row.EXAMINETIME = rowNoDeliveryRO.EXAMINETIME                       '完成検査完了時刻
    '                Else
    '                    row.EXAMINETIME = Date.MinValue
    '                End If
    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '            End If
    '        Next

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店実績-R/O基本情報マージ(チップ詳細)
    '    ''' </summary>
    '    ''' <param name="row">サービス来店情報</param>
    '    ''' <param name="dtIFOrderCommon">R/O基本情報参照</param>
    '    ''' <returns>来店チップレコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetVisitDetailOrderMargin(ByVal row As SC3140103StallRezinfoRow, ByVal dtIFOrderCommon As IC3801001OrderCommDataTable) As SC3140103StallRezinfoRow
    '        'Private Function SetVisitDetailOrderMargin(ByVal row As SC3140103VisitRow, ByVal dtIFOrderCommon As IC3801001OrderCommDataTable) As SC3140103VisitRow
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim rowIFOrderCommon As IC3801001OrderCommRow

    '        If dtIFOrderCommon IsNot Nothing AndAlso dtIFOrderCommon.Rows.Count > 0 Then
    '            rowIFOrderCommon = DirectCast(dtIFOrderCommon.Rows(0), IC3801001OrderCommRow)

    '            'If Not IsDBNull(rowIFOrderCommon.Item("OrderIFlag")) Then
    '            '    row.JDP_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderIFlag)                           'JDP調査対象客フラグ
    '            'End If
    '            'If Not IsDBNull(rowIFOrderCommon.Item("OrderSFlag")) Then
    '            '    row.SSC_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderSFlag)                           'SSCフラグ
    '            'End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderRegisterNo")) Then
    '                'row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowIFOrderCommon.OrderRegisterNo)                      '車両登録No.
    '                row.VCLREGNO = Me.SetReplaceString(rowIFOrderCommon.OrderRegisterNo, row.VCLREGNO)                      '車両登録No.                
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderVhcName")) Then
    '                'row.VEHICLENAME = Me.SetReplaceString(row.VEHICLENAME, rowIFOrderCommon.OrderVhcName)                   '車種名称
    '                row.VEHICLENAME = Me.SetReplaceString(rowIFOrderCommon.OrderVhcName, row.VEHICLENAME)                   '車種名称
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderGrade")) Then
    '                'row.MODELCODE = Me.SetReplaceString(row.MODELCODE, rowIFOrderCommon.OrderGrade)                         'MODEL
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 START
    '                ''2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '                ''row.MODELCODE = Me.SetReplaceString(rowIFOrderCommon.OrderGrade, row.MODELCODE)                         'MODEL
    '                'row.GRADE = Me.SetReplaceString(rowIFOrderCommon.OrderGrade, row.MODELCODE)                              'GRADE
    '                ''2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '                row.GRADE = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderGrade)                              'GRADE
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 END
    '            End If
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderModel")) Then
    '                row.MODELCODE = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderModel)                         'MODEL
    '            End If
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderVinNo")) Then
    '                'row.VIN = Me.SetReplaceString(row.VIN, rowIFOrderCommon.OrderVinNo)                                     'VINNO
    '                row.VIN = Me.SetReplaceString(rowIFOrderCommon.OrderVinNo, row.VIN)                                     'VINNO
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderMileAge")) Then
    '                'row.MILEAGE = Me.SetReplaceLong(CType(row.MILEAGE, Long), CType(rowIFOrderCommon.OrderMileAge, Long))   '走行距離
    '                row.MILEAGE = Me.SetReplaceLong(CType(rowIFOrderCommon.OrderMileAge, Long), CType(row.MILEAGE, Long))   '走行距離
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("TwcTel1")) Then
    '                'row.TELNO = Me.SetReplaceString(row.TELNO, rowIFOrderCommon.TwcTel1)                                    'サービスフォロー顧客電話1
    '                row.TELNO = Me.SetReplaceString(rowIFOrderCommon.TwcTel1, row.TELNO)                                    'サービスフォロー顧客電話1
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("TwcTel2")) Then
    '                'row.MOBILE = Me.SetReplaceString(row.MOBILE, rowIFOrderCommon.TwcTel2)                                  'サービスフォロー顧客電話2
    '                row.MOBILE = Me.SetReplaceString(rowIFOrderCommon.TwcTel2, row.MOBILE)                                  'サービスフォロー顧客電話2
    '            End If
    '            If Not IsDBNull(rowIFOrderCommon.Item("OrderCustomerName")) Then
    '                row.CUSTOMERNAME = Me.SetReplaceString(rowIFOrderCommon.OrderCustomerName, row.CUSTOMERNAME)            '顧客名
    '            End If
    '            'If Not IsDBNull(rowIFOrderCommon.Item("printFlag")) Then
    '            '    row.CHECKSHEET_FLAG = Me.SetReplaceString(String.Empty, rowIFOrderCommon.printFlag)                     'チェックシート印刷有無
    '            'End If
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '            Dim deliveryHopeDate As String = ""
    '            If Not IsDBNull(rowIFOrderCommon.Item("DeliveryHopeDate")) Then
    '                '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 START
    '                'row.REZ_DELI_DATE = Me.SetReplaceString(rowIFOrderCommon.DeliveryHopeDate.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture), row.REZ_DELI_DATE)            '納車予定時間
    '                deliveryHopeDate = rowIFOrderCommon.DeliveryHopeDate
    '                '追加作業納車予定日時がNullでないか確認(最新追加作業がある場合、日時データが入れられるので追加作業納車予定日時をチップ詳細に表示)
    '                If Not IsDBNull(rowIFOrderCommon.Item("addDeliveryHopeDate")) Then
    '                    deliveryHopeDate = rowIFOrderCommon.addDeliveryHopeDate
    '                End If
    '                ' R/O情報の納車予定日時の確認
    '                If Not String.IsNullOrEmpty(deliveryHopeDate) Then
    '                    row.REZ_DELI_DATE = Me.SetReplaceString(CType(deliveryHopeDate, Date).ToString(DateFormateYYYYMMDDHHMM, CultureInfo.CurrentCulture), row.REZ_DELI_DATE)            '納車予定時間
    '                Else
    '                    ' R/O情報がない場合、予約の納車予定日時
    '                    row.REZ_DELI_DATE = row.REZ_DELI_DATE
    '                End If

    '                '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 END
    '            End If
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return row
    '    End Function

    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店実績-顧客参照情報マージ(チップ詳細)
    '    ''' </summary>
    '    ''' <param name="row">サービス来店情報</param>
    '    ''' <param name="dtIFSrvCustomer">顧客参照情報</param>
    '    ''' <param name="displayDiv">表示区分</param>
    '    ''' <returns>来店チップデータセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetVisitDetailCustomerMargin(ByVal row As SC3140103VisitRow, ByVal dtIFSrvCustomer As IC3800703SrvCustomerDataTable, ByVal displayDiv As String) As SC3140103VisitRow

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim rowIFSrvCustomer As IC3800703SrvCustomerFRow

    '        If dtIFSrvCustomer IsNot Nothing AndAlso dtIFSrvCustomer.Rows.Count > 0 Then
    '            rowIFSrvCustomer = DirectCast(dtIFSrvCustomer.Rows(0), IC3800703SrvCustomerFRow)

    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            If Not IsDBNull(rowIFSrvCustomer.Item("REGISTERNO")) Then
    '                'row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowIFSrvCustomer.REGISTERNO)           '登録NO.
    '                row.VCLREGNO = Me.SetReplaceString(rowIFSrvCustomer.REGISTERNO, row.VCLREGNO)           '登録NO.
    '            End If
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.227 チップ詳細の車名を表示するための項目を変更 START
    '            ''2012/03/31 KN 森下【SERVICE_1】顧客参照の型名の取得項目の修正 START
    '            ''If Not IsDBNull(rowIFSrvCustomer.Item("MODEL")) Then
    '            'If Not IsDBNull(rowIFSrvCustomer.Item("VHCCODE")) Then
    '            '    '2012/03/31 KN 森下【SERVICE_1】顧客参照の型名の取得項目の修正 END
    '            '    'row.VEHICLENAME = Me.SetReplaceString(row.VEHICLENAME, rowIFSrvCustomer.MODEL)          '型名
    '            '    '2012/03/31 KN 森下【SERVICE_1】顧客参照の型名の取得項目の修正 START
    '            '    'row.VEHICLENAME = Me.SetReplaceString(rowIFSrvCustomer.MODEL, row.VEHICLENAME)          '型名
    '            '    row.VEHICLENAME = Me.SetReplaceString(rowIFSrvCustomer.VHCCODE, row.VEHICLENAME)          '型名
    '            '    '2012/03/31 KN 森下【SERVICE_1】顧客参照の型名の取得項目の修正 END
    '            'End If
    '            If Not IsDBNull(rowIFSrvCustomer.Item("VHCNAME")) Then
    '                row.VEHICLENAME = Me.SetReplaceString(rowIFSrvCustomer.VHCNAME, row.VEHICLENAME)          '車名
    '            End If
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.227 チップ詳細の車名を表示するための項目を変更 END
    '            If Not IsDBNull(rowIFSrvCustomer.Item("GRADE")) Then
    '                'row.MODELCODE = Me.SetReplaceString(row.MODELCODE, rowIFSrvCustomer.GRADE)              'モデル
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 START
    '                ''2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '                ''row.MODELCODE = Me.SetReplaceString(rowIFSrvCustomer.GRADE, row.MODELCODE)              'モデル
    '                'row.GRADE = Me.SetReplaceString(rowIFSrvCustomer.GRADE, row.GRADE)                      'GRADE
    '                ''2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '                row.GRADE = Me.SetReplaceString(rowIFSrvCustomer.GRADE, row.GRADE)                      'GRADE
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 END
    '            End If
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '            If Not IsDBNull(rowIFSrvCustomer.Item("MODEL")) Then
    '                row.MODELCODE = Me.SetReplaceString(rowIFSrvCustomer.MODEL, row.MODELCODE)                'MODEL
    '            End If
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '            If Not IsDBNull(rowIFSrvCustomer.Item("VINNO")) Then
    '                'row.VIN = Me.SetReplaceString(row.VIN, rowIFSrvCustomer.VINNO)                          'VINNO
    '                row.VIN = Me.SetReplaceString(rowIFSrvCustomer.VINNO, row.VIN)                          'VINNO
    '            End If
    '            If Not IsDBNull(rowIFSrvCustomer.Item("MILEAGE")) Then
    '                'row.MILEAGE = Me.SetReplaceLong(CType(row.MILEAGE, Long), rowIFSrvCustomer.MILEAGE)     '走行距離
    '                row.MILEAGE = Me.SetReplaceLong(rowIFSrvCustomer.MILEAGE, CType(row.MILEAGE, Long))     '走行距離
    '            End If
    '            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERTEL1")) Then
    '                'row.TELNO = Me.SetReplaceString(row.TELNO, rowIFSrvCustomer.BUYERTEL1)                  'サービスフォロー顧客電話1
    '                row.TELNO = Me.SetReplaceString(rowIFSrvCustomer.BUYERTEL1, row.TELNO)                  'サービスフォロー顧客電話1
    '            End If
    '            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERTEL2")) Then
    '                'row.MOBILE = Me.SetReplaceString(row.MOBILE, rowIFSrvCustomer.BUYERTEL2)                'サービスフォロー顧客電話2
    '                row.MOBILE = Me.SetReplaceString(rowIFSrvCustomer.BUYERTEL2, row.MOBILE)                'サービスフォロー顧客電話2
    '            End If
    '            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERNAME")) Then
    '                'row.CUSTOMERNAME = Me.SetReplaceString(row.CUSTOMERNAME, rowIFSrvCustomer.BUYERNAME)    '顧客名
    '                row.CUSTOMERNAME = Me.SetReplaceString(rowIFSrvCustomer.BUYERNAME, row.CUSTOMERNAME)    '顧客名
    '            End If
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '            If Not IsDBNull(rowIFSrvCustomer.Item("VHCSALESDATE")) Then
    '                '日付チェック
    '                If Not IsDateTimeNull(rowIFSrvCustomer.VHCSALESDATE) Then
    '                    row.DELIVERDATE = rowIFSrvCustomer.VHCSALESDATE                                     '納車日
    '                End If
    '            End If
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START
    '            ' R/O発行確認(R/O未発行かつ受付エリアのチップ)
    '            If String.IsNullOrEmpty(row.ORDERNO) And displayDiv.Equals(DisplayDivReception) Then
    '                If Not IsDBNull(rowIFSrvCustomer.Item("JDPFLAG")) Then
    '                    row.JDP_MARK = rowIFSrvCustomer.JDPFLAG                           'JDP調査対象客フラグ
    '                End If
    '                If Not IsDBNull(rowIFSrvCustomer.Item("SSCFLAG")) Then
    '                    row.SSC_MARK = rowIFSrvCustomer.SSCFLAG                           'SSCフラグ
    '                End If
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 END
    '            End If
    '        End If
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return row
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店管理情報取得
    '    ''' </summary>
    '    ''' <param name="visitSeq">来店実績連番</param>
    '    ''' <returns>サービス来店実績データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Function GetVisitManager(ByVal visitSeq As Long) As SC3140103ServiceVisitManagementDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} IN:visitSeq = {3}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START _
    '                                , visitSeq))

    '        Dim dt As SC3140103ServiceVisitManagementDataTable

    '        Using da As New SC3140103DataTableAdapter
    '            '検索処理
    '            dt = da.GetVisitManagement(visitSeq)
    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店実績情報取得
    '    ''' </summary>
    '    ''' <param name="visitSeq">来店実績連番</param>
    '    ''' <param name="displayDiv">表示区分</param>
    '    ''' <returns>来店実績データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Function GetVisitChipDetailForNextScreen(ByVal visitSeq As Long, ByVal displayDiv As String) As SC3140103VisitDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} IN:visitSeq = {3}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START _
    '                                , visitSeq))

    '        Dim dt As SC3140103VisitDataTable = New SC3140103VisitDataTable
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        Dim dtService As SC3140103ServiceVisitManagementDataTable
    '        Dim dtRezinfo As SC3140103StallRezinfoDataTable
    '        Dim dtProcess As SC3140103StallProcessDataTable

    '        Using da As New SC3140103DataTableAdapter
    '            '検索処理
    '            dtService = da.GetVisitManagement(visitSeq)
    '            dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
    '            dtProcess = da.GetStallProcessDetail(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
    '        End Using

    '        ' サービス来店実績・ストール予約実績取得
    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START
    '        ''2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '        ''dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)
    '        'dt = Me.SetVisitDetail(dtService, dtRezinfo, dtProcess, staffInfo)
    '        ''2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '        dt = Me.SetVisitDetail(dtService, dtRezinfo, dtProcess, staffInfo, displayDiv)

    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function


    '    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店管理情報取得（受付待ちエリア用）
    '    ''' </summary>
    '    ''' <param name="inDealerCode">販売店コード</param>
    '    ''' <param name="inBranchCode">店舗コード</param>
    '    ''' <returns>受付待ち情報データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Function GetAssignmentInfo(ByVal inDealerCode As String _
    '                                    , ByVal inBranchCode As String) _
    '                                      As SC3140103AssignmentInfoDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START _
    '                                , inDealerCode, inBranchCode))

    '        '現在時間の取得
    '        Dim presentTime As Date = DateTimeFunc.Now(inDealerCode)

    '        Using da As New SC3140103DataTableAdapter

    '            '受付待ちエリア情報取得
    '            Dim dt As SC3140103AssignmentInfoDataTable = da.GetAssignmentInfo(inDealerCode, inBranchCode, presentTime)


    '            '終了ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "{0}.{1} {2}" _
    '                                    , Me.GetType.ToString _
    '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                    , LOG_END))

    '            '処理結果返却
    '            Return dt

    '        End Using
    '    End Function

    '    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


    '#End Region

    '#Region " 来店チップ情報取得"
    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ情報形成
    '    ''' </summary>
    '    ''' <param name="row">来店チップレコード</param>
    '    ''' <param name="rowChip">チップ情報設定レコード</param>
    '    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    '    ''' <param name="smbCommonBis">共通関数オブジェクト</param>
    '    ''' <param name="inNowDate">現在時刻</param>
    '    ''' <returns>チップ情報設定レコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetRowChip(ByVal row As SC3140103VisitRow _
    '                              , ByVal rowChip As SC3140103VisitChipRow _
    '                              , ByVal dtmRezDeliDate As DateTime _
    '                              , ByVal smbCommonBis As SMBCommonClassBusinessLogic _
    '                              , ByVal initCommonResult As Long _
    '                              , ByVal inNowDate As Date) As SC3140103VisitChipRow
    '        'Private Function GetRowChip(ByVal row As SC3140103VisitRow , ByVal rowChip As SC3140103VisitChipRow , ByVal dtmRezDeliDate As DateTime) As SC3140103VisitChipRow
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ''開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2} IN: dtmRezDeliDate= {3}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START _
    '        '                        , dtmRezDeliDate))
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '        'Dim staffInfo As StaffContext = StaffContext.Current
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '        ' 来店実績連番
    '        rowChip.VISITSEQ = row.VISITSEQ
    '        ' 販売店コード
    '        rowChip.DLRCD = row.DLRCD
    '        ' 店舗コード
    '        rowChip.STRCD = row.STRCD
    '        ' 予約ID
    '        rowChip.FREZID = row.FREZID
    '        ' 表示区分
    '        rowChip.DISP_DIV = Me.dispDiv
    '        ' 仕掛中
    '        rowChip.DISP_START = Me.dispStart
    '        ' VIPマーク
    '        rowChip.VIP_MARK = row.VIP_MARK
    '        ' 予約マーク
    '        rowChip.REZ_MARK = row.REZ_MARK
    '        ' JDP調査対象客マーク
    '        rowChip.JDP_MARK = row.JDP_MARK
    '        ' 技術情報マーク
    '        rowChip.SSC_MARK = row.SSC_MARK
    '        '2012/04/11 KN 西田 顧客名もIFの値を参照 START
    '        ' 顧客名
    '        rowChip.CUSTOMERNAME = row.CUSTOMERNAME
    '        '2012/04/11 KN 西田 顧客名もIFの値を参照 END
    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START
    '        ' R/O発行確認
    '        If String.IsNullOrEmpty(row.ORDERNO) And Me.dispDiv.Equals(DisplayDivReception) Then
    '            'R/O未発行かつ受付エリアのチップの場合、JDP調査対象客マーク、技術情報マークがないので顧客情報から取得            
    '            Dim dtIFSrvCustomerDataTable As IC3800703SrvCustomerDataTable
    '            'VINがなければ取得しない
    '            '2012/04/18 KN 森下 【SERVICE_1】仕様変更対応(様変更管理状況.xls No20 判定条件に車両登録ナンバー追加) START
    '            'If Not String.IsNullOrEmpty(row.VIN.Trim()) Then
    '            If (Not String.IsNullOrEmpty(row.VIN.Trim())) Or _
    '                (Not String.IsNullOrEmpty(row.VCLREGNO.Trim())) Then
    '                '2012/04/18 KN 森下 【SERVICE_1】仕様変更対応(様変更管理状況.xls No20 判定条件に車両登録ナンバー追加) END
    '                'IF-顧客参照処理
    '                dtIFSrvCustomerDataTable = Me.GetIFCustomerInformation(row)
    '                Dim rowIFSrvCustomer As IC3800703SrvCustomerFRow

    '                If dtIFSrvCustomerDataTable IsNot Nothing _
    '                    AndAlso dtIFSrvCustomerDataTable.Rows.Count > 0 Then

    '                    rowIFSrvCustomer = _
    '                        DirectCast(dtIFSrvCustomerDataTable.Rows(0), IC3800703SrvCustomerFRow)
    '                    'JDP調査対象客マーク
    '                    If Not IsDBNull(rowIFSrvCustomer.Item("JDPFLAG")) Then
    '                        rowChip.JDP_MARK = rowIFSrvCustomer.JDPFLAG    'JDP調査対象客マーク
    '                    End If
    '                    '技術情報マーク
    '                    If Not IsDBNull(rowIFSrvCustomer.Item("SSCFLAG")) Then
    '                        rowChip.SSC_MARK = rowIFSrvCustomer.SSCFLAG    '技術情報マーク
    '                    End If

    '                    '2012/04/11 KN 西田 顧客名もIFの値を参照 START
    '                    '顧客名
    '                    If Not IsDBNull(rowIFSrvCustomer.Item("BUYERNAME")) Then
    '                        rowChip.CUSTOMERNAME = rowIFSrvCustomer.BUYERNAME
    '                    End If
    '                    '2012/04/11 KN 西田 顧客名もIFの値を参照 END
    '                End If
    '            End If
    '        End If
    '        '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 END
    '        ' 登録番号
    '        rowChip.VCLREGNO = row.VCLREGNO
    '        '2012/04/11 KN 西田 顧客名もIFの値を参照 START
    '        '' 顧客名
    '        'rowChip.CUSTOMERNAME = row.CUSTOMERNAME
    '        '2012/04/11 KN 西田 顧客名もIFの値を参照 END
    '        ' 代表入庫項目
    '        rowChip.MERCHANDISENAME = row.MERCHANDISENAME
    '        ' 駐車場コード
    '        rowChip.PARKINGCODE = row.PARKINGCODE
    '        ' 担当テクニシャン名
    '        rowChip.STAFFNAME = row.STAFFNAME
    '        ' 追加作業承認数
    '        rowChip.APPROVAL_COUNT = row.APPROVAL_COUNT
    '        ' 整備受注NO
    '        rowChip.ORDERNO = row.ORDERNO
    '        ' 追加承認待ちID
    '        rowChip.APPROVAL_ID = String.Empty
    '        '納車予定時刻
    '        rowChip.REZ_DELI_DATE = dtmRezDeliDate
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '        '呼出ステータス
    '        rowChip.CALLSTATUS = row.CALLSTATUS
    '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '        ' チップ情報チェック
    '        Select Case Me.dispDiv
    '            Case DisplayDivReception       ' 受付
    '                ' 表示順
    '                rowChip.DISP_SORT = _
    '                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)  ' SA割振り日時
    '                ' 表示日時
    '                rowChip.ITEM_DATE = row.VISITTIMESTAMP      ' 来店時刻
    '                ' 残計算日時
    '                rowChip.PROC_DATE = row.ASSIGNTIMESTAMP     ' SA割振り日時

    '            Case DisplayDivPreparation     ' 納車準備
    '                ' 表示順 (納車予定日時＋ SA割振り日時)
    '                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
    '                       dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
    '                       row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))

    '                ' 表示日時
    '                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '                ' 残計算日時
    '                'rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時
    '                rowChip.PROC_DATE = row.EXAMINETIME          ' 完成検査完了時刻
    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            Case DisplayDivDelivery        ' 納車作業
    '                ' 表示順  (納車予定日時＋ SA割振り日時)
    '                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
    '                            dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
    '                            row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))
    '                ' 表示日時
    '                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '                ' 残計算日時
    '                'rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時
    '                rowChip.PROC_DATE = row.COLSINGPRINTTIME     ' 清算書印刷時刻
    '                '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            Case DisplayDivWork            ' 作業中
    '                ' 表示順  (作業終了予定時刻＋SA割振り日時)
    '                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
    '                        row.ENDTIME.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
    '                        row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))
    '                ' 表示日時
    '                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

    '                '2012/04/04 nishida プレユーザーテスト課題 No.91 作業予定の無い場合、作業完了予定時刻の表示を「00'00」対応 START
    '                ' 残計算日時
    '                If row.ENDTIME.Equals(row.STARTTIME) Then
    '                    '作業開始予定時刻と作業終了予定時刻が同じ場合、未配置の予約なのでカウンターの表示を「00'00」とするため
    '                    rowChip.PROC_DATE = DateTime.MinValue
    '                Else
    '                    rowChip.PROC_DATE = row.ENDTIME             ' 作業終了予定時刻
    '                End If

    '                '' 残計算日時
    '                'rowChip.PROC_DATE = row.ENDTIME             ' 作業終了予定時刻
    '                '2012/04/04 nishida プレユーザーテスト課題 No.91 作業予定の無い場合、作業完了予定時刻の表示を「00'00」対応 END

    '            Case Else
    '                ' 表示順
    '                rowChip.DISP_SORT = String.Empty
    '                ' 表示日時
    '                rowChip.ITEM_DATE = DateTime.MinValue
    '                ' 残計算日時
    '                rowChip.PROC_DATE = DateTime.MinValue

    '        End Select

    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '        '受付中でない AND 有効以外件数=0
    '        If (Not DisplayDivReception.Equals(Me.dispDiv)) _
    '            And row.UNVALID_REZ_COUNT = 0 _
    '            And initCommonResult = 0 Then

    '            Try
    '                '納車見込遅れ日時
    '                rowChip.DELAY_DELI_TIME = _
    '                    smbCommonBis.GetDeliveryDelayDate _
    '                                    (CType(Me.dispDiv, SMBCommonClassBusinessLogic.DisplayType), _
    '                                    dtmRezDeliDate, _
    '                                    row.LAST_ENDTIME, _
    '                                    row.EXAMINETIME, _
    '                                    Me.StringParseDate(row.RESULT_WASH_START), _
    '                                    Me.StringParseDate(row.RESULT_WASH_END), _
    '                                    row.COLSINGPRINTTIME, _
    '                                    row.WORK_TIME, _
    '                                    row.WASHFLG, _
    '                                    inNowDate)

    '            Catch ex As ArgumentException
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} Exception:{2}" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                        , ex.Message))
    '                rowChip.DELAY_DELI_TIME = Date.MaxValue
    '            End Try

    '        Else
    '            '受付中のアイコンは青色で表示する
    '            rowChip.DELAY_DELI_TIME = Date.MaxValue     '納車見込遅れ日時
    '        End If
    '        '洗車フラグ
    '        rowChip.WASHFLG = row.WASHFLG
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ''終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))

    '        Return rowChip
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加承認待ち情報形成
    '    ''' </summary>
    '    ''' <param name="row">来店チップレコード</param>
    '    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    '    ''' <param name="rowChip">チップ情報設定レコード</param>
    '    ''' <param name="rowChipApproval">追加承認待ちチップ情報設定レコード</param>
    '    ''' <param name="rowIFApproval">追加承認待ちレコード</param>
    '    ''' <returns>追加承認待ちチップ情報設定レコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetRowChipApporoval(ByVal row As SC3140103VisitRow _
    '                                       , ByVal dtmRezDeliDate As DateTime _
    '                                       , ByVal rowChip As SC3140103VisitChipRow _
    '                                       , ByVal rowChipApproval As SC3140103VisitChipRow _
    '                                       , ByVal rowIFApproval As IC3801002DataSet.ConfirmAddListRow) _
    '                                   As SC3140103VisitChipRow

    '        ''開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2} IN: dtmRezDeliDate= {3}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START _
    '        '                        , dtmRezDeliDate))

    '        ' 来店実績連番
    '        rowChipApproval.VISITSEQ = rowChip.VISITSEQ
    '        ' 販売店コード
    '        rowChipApproval.DLRCD = rowChip.DLRCD
    '        ' 店舗コード
    '        rowChipApproval.STRCD = rowChip.STRCD
    '        ' 予約ID
    '        rowChipApproval.FREZID = rowChip.FREZID
    '        ' 表示区分
    '        rowChipApproval.DISP_DIV = DisplayDivApproval
    '        ' 仕掛中
    '        ' IFマージ処理
    '        If Not IsDBNull(rowIFApproval.Item("PRINTFLAG")) Then
    '            row.APPROVAL_OUTPUT = rowIFApproval.PRINTFLAG
    '        End If
    '        ' 追加作業承認印刷チェック
    '        If row.APPROVAL_OUTPUT.Equals(C_APPROVAL_OUTPUT_ON) Then
    '            ' 追加作業承認印刷完了
    '            rowChipApproval.DISP_START = DisplayStartStart  ' 仕掛中
    '        Else
    '            ' 上記以外
    '            rowChipApproval.DISP_START = DisplayStartNone   ' 仕掛前
    '        End If
    '        ' VIPマーク
    '        rowChipApproval.VIP_MARK = rowChip.VIP_MARK
    '        ' 予約マーク
    '        rowChipApproval.REZ_MARK = rowChip.REZ_MARK
    '        ' JDP調査対象客マーク
    '        rowChipApproval.JDP_MARK = rowChip.JDP_MARK
    '        ' 技術情報マーク
    '        rowChipApproval.SSC_MARK = rowChip.SSC_MARK
    '        ' 登録番号
    '        rowChipApproval.VCLREGNO = rowChip.VCLREGNO
    '        ' 顧客名
    '        rowChipApproval.CUSTOMERNAME = rowChip.CUSTOMERNAME
    '        ' 代表入庫項目
    '        rowChipApproval.MERCHANDISENAME = rowChip.MERCHANDISENAME
    '        ' 駐車場コード
    '        rowChipApproval.PARKINGCODE = rowChip.PARKINGCODE
    '        ' 担当テクニシャン名
    '        rowChipApproval.STAFFNAME = rowChip.STAFFNAME
    '        ' 追加作業承認数
    '        rowChipApproval.APPROVAL_COUNT = rowChip.APPROVAL_COUNT
    '        ' 整備受注NO
    '        rowChipApproval.ORDERNO = rowChip.ORDERNO
    '        ' 追加承認待ちID
    '        If Not IsDBNull(rowIFApproval.Item("SRVADDSEQ")) Then
    '            rowChipApproval.APPROVAL_ID = rowIFApproval.SRVADDSEQ
    '        End If

    '        ' 表示日時
    '        rowChipApproval.ITEM_DATE = dtmRezDeliDate                  ' 納車予定日時
    '        ' 残計算日時
    '        rowChipApproval.PROC_DATE = DateTime.MinValue
    '        If Not IsDBNull(rowIFApproval.Item("SACONFIRMRELYDATE")) Then
    '            If Not String.IsNullOrEmpty(rowIFApproval.SACONFIRMRELYDATE) Then
    '                rowChipApproval.PROC_DATE = _
    '                    CType(rowIFApproval.SACONFIRMRELYDATE.ToString(CultureInfo.CurrentCulture),  _
    '                          DateTime)   ' SA承認待ち時刻
    '            End If
    '        End If

    '        ' 2012/02/23 KN 森下【SERVICE_1】START
    '        ' 表示順  (追加作業承認依頼時刻＋SA割振り日時)
    '        rowChipApproval.DISP_SORT = _
    '            String.Format(CultureInfo.CurrentCulture, "{0}{1}",
    '                        rowChipApproval.PROC_DATE.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture) _
    '                      , row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))
    '        ' 2012/02/23 KN 森下【SERVICE_1】END

    '        '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '        rowChipApproval.REZ_DELI_DATE = dtmRezDeliDate              ' 納車予定日時
    '        rowChipApproval.DELAY_DELI_TIME = rowChip.DELAY_DELI_TIME   ' 納車予定見込み日時
    '        '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '        ''終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))

    '        Return rowChipApproval
    '    End Function

    '#End Region

    '#Region " チップ状態チェック"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ状態チェック
    '    ''' </summary>
    '    ''' <param name="row">来店チップレコード</param>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' 2013/01/10 TMEJ 小澤  【SERVICE_2】次世代サービスROステータス切り離し対応
    '    ''' </history>
    '    '''-------------------------------------------------------
    '    Private Sub CheckChipStatus(ByVal row As SC3140103VisitRow)

    '        ''開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2} IN:ORDERNO = {3}, RO_STATUS = {4}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START _
    '        '                        , row.ORDERNO _
    '        '                        , row.RO_STATUS))
    '        ' 1 : 受付チェック

    '        ' 整備受注NOチェック
    '        If String.IsNullOrEmpty(row.ORDERNO) Then
    '            ' 整備受注NOなし
    '            Me.dispDiv = DisplayDivReception  ' 受付
    '            Me.dispStart = DisplayStartNone   ' 仕掛前
    '            Return
    '        End If

    '        ' R/Oステータスチェック
    '        If row.RO_STATUS.Equals(C_RO_STATUS_RECEPTION) Then
    '            ' R/Oステータス：受付
    '            Me.dispDiv = DisplayDivReception  ' 受付
    '            Me.dispStart = DisplayStartStart  ' 仕掛中
    '            Return

    '        ElseIf row.RO_STATUS.Equals(C_RO_STATUS_ESTI_WAIT) Then
    '            ' R/Oステータス：見積確認待ち
    '            Me.dispDiv = DisplayDivReception  ' 受付
    '            Me.dispStart = DisplayStartStart  ' 仕掛中
    '            Return
    '        End If

    '        ' 2 : 作業中チェック

    '        ' R/Oステータスチェック
    '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '        'If row.RO_STATUS.Equals(C_RO_STATUS_WORKING) Or
    '        '   row.RO_STATUS.Equals(C_RO_STATUS_ITEM_WAIT) Or
    '        '   row.RO_STATUS.Equals(C_RO_STATUS_INSP_OK) Then
    '        If row.RO_STATUS.Equals(C_RO_STATUS_WORKING) Or
    '           row.RO_STATUS.Equals(C_RO_STATUS_ITEM_WAIT) Then
    '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '            ' R/Oステータス：作業中、部品待ち、検査完了

    '            ' ストール予約取得チェック
    '            If row.REZINFO_FREZID > SC3140103DataTableAdapter.MinReserveId Then
    '                ' ストール予約あり

    '                '2013/01/10 TMEJ 小澤  【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '                '' 作業開始時間チェック
    '                '' 作業終了時間チェック
    '                'If Not IsDateTimeNull(row.ACTUAL_STIME) And _
    '                '    Not IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                '    ' 作業開始時間あり
    '                '    ' 作業終了時間あり

    '                'ElseIf IsDateTimeNull(row.ACTUAL_STIME) And _
    '                '        IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                '    ' 作業開始時間なし
    '                '    ' 作業終了時間なし
    '                '    Me.dispDiv = DisplayDivWork       ' 作業中
    '                '    Me.dispStart = DisplayStartNone   ' 仕掛前
    '                '    Return

    '                'ElseIf Not IsDateTimeNull(row.ACTUAL_STIME) And _
    '                '            IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                '    ' 作業開始時間あり
    '                '    ' 作業終了時間なし
    '                '    Me.dispDiv = DisplayDivWork       ' 作業中
    '                '    Me.dispStart = DisplayStartStart  ' 仕掛中
    '                '    Return
    '                'End If
    '                '作業中エリアに設定
    '                Me.dispDiv = DisplayDivWork       ' 作業中
    '                ' 作業開始時間チェック
    '                ' 作業終了時間チェック
    '                If Not IsDateTimeNull(row.ACTUAL_STIME) And _
    '                    Not IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                    ' 作業開始時間あり
    '                    ' 作業終了時間あり
    '                    Me.dispStart = DisplayStartStart  ' 仕掛中
    '                    Return

    '                ElseIf IsDateTimeNull(row.ACTUAL_STIME) And _
    '                        IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                    ' 作業開始時間なし
    '                    ' 作業終了時間なし
    '                    Me.dispStart = DisplayStartNone   ' 仕掛前
    '                    Return

    '                ElseIf Not IsDateTimeNull(row.ACTUAL_STIME) And _
    '                            IsDateTimeNull(row.ACTUAL_ETIME) Then
    '                    ' 作業開始時間あり
    '                    ' 作業終了時間なし
    '                    Me.dispStart = DisplayStartStart  ' 仕掛中
    '                    Return
    '                End If
    '                '2013/01/10 TMEJ 小澤  【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '            End If
    '        End If

    '        ' 3 : 納車準備チェック

    '        ' R/Oステータスチェック
    '        If row.RO_STATUS.Equals(C_RO_STATUS_INSP_OK) Then
    '            ' R/Oステータス：検査完了

    '            ' ストール予約・実績取得チェック
    '            If row.REZINFO_FREZID > SC3140103DataTableAdapter.MinReserveId And
    '                row.PROCESS_FREZID > SC3140103DataTableAdapter.MinReserveId Then
    '                ' ストール予約・実績あり
    '                ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '                '' 実績ステータスチェック
    '                'If row.RESULT_STATUS.Equals(C_STALLPROSESS_WASH_WAIT) Then
    '                '    ' 実績ステータス：洗車待ち
    '                '    Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '    Me.dispStart = DisplayStartNone       ' 仕掛前
    '                '    Return

    '                'ElseIf row.RESULT_STATUS.Equals(C_STALLPROSESS_WASH_DOING) Or
    '                '       row.RESULT_STATUS.Equals(C_STALLPROSESS_CARRY_WAIT) Or
    '                '       row.RESULT_STATUS.Equals(C_STALLPROSESS_DELI_WAIT) Then
    '                '    ' 実績ステータス：洗車中、納車待ち、預かり中
    '                '    Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '    Me.dispStart = DisplayStartStart      ' 仕掛中
    '                '    Return
    '                'End If
    '                '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
    '                'Dim bizOrderStatus As New IC3801901BusinessLogic
    '                'Dim drOrderStatus As IC3801901DataSet.OrderStatusDataRow = _
    '                '    bizOrderStatus.GetOrderStatus(row.DLRCD, row.STRCD, row.ORDERNO)
    '                ''追加作業チェック
    '                'If (drOrderStatus.IsADDStatusNull OrElse String.IsNullOrEmpty(drOrderStatus.ADDStatus)) OrElse
    '                '   "9".Equals(drOrderStatus.ADDStatus) Then
    '                '    '「追加作業なし OrElse 追加作業ステータス＝9:検査完了」の場合
    '                '    If "1".Equals(row.WASHFLG) AndAlso _
    '                '       (row.IsRESULT_WASH_STARTNull OrElse _
    '                '       String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '                '       row.RESULT_WASH_START.Count = 0) Then
    '                '        '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                '        Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '        Me.dispStart = DisplayStartNone       ' 仕掛前
    '                '        Return
    '                '    Else
    '                '        Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '        Me.dispStart = DisplayStartStart      ' 仕掛中
    '                '        Return
    '                '    End If
    '                'ElseIf Not ("9".Equals(drOrderStatus)) Then
    '                '    '「追加作業あり、追加作業ステータス≠9:検査完了」の場合
    '                '    Me.dispDiv = DisplayDivWork       ' 作業中
    '                '    Me.dispStart = DisplayStartStart  ' 仕掛中
    '                '    Return
    '                'End If

    '                ' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）START
    '                'Dim bizAddRepairStatusList As New IC3800804BusinessLogic
    '                'Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
    '                '    DirectCast(bizAddRepairStatusList.GetAddRepairStatusList(row.DLRCD, row.ORDERNO),  _
    '                '        IC3800804AddRepairStatusDataTableDataTable)

    '                'If Not IsNothing(dtAddRepairStatus) Or 0 < dtAddRepairStatus.Count Then
    '                '    '追加作業が存在する場合
    '                '    Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
    '                '        (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray
    '                '    If 0 < rowAddList.Count Then
    '                '        '「追加作業ステータス≠9」が存在する場合
    '                '        Me.dispDiv = DisplayDivWork       ' 作業中
    '                '        Me.dispStart = DisplayStartStart  ' 仕掛中
    '                '        Return
    '                '    Else
    '                '        '「追加作業ステータス≠9」が存在しない場合
    '                '        If "1".Equals(row.WASHFLG) AndAlso _
    '                '           (row.IsRESULT_WASH_STARTNull OrElse _
    '                '           String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '                '           row.RESULT_WASH_START.Count = 0) Then
    '                '            '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                '            Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '            Me.dispStart = DisplayStartNone       ' 仕掛前
    '                '            Return
    '                '        Else
    '                '            Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '            Me.dispStart = DisplayStartStart      ' 仕掛中
    '                '            Return
    '                '        End If
    '                '    End If
    '                'Else
    '                '    '追加作業が存在しない場合
    '                '    If "1".Equals(row.WASHFLG) AndAlso _
    '                '       (row.IsRESULT_WASH_STARTNull OrElse _
    '                '       String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '                '       row.RESULT_WASH_START.Count = 0) Then
    '                '        '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                '        Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '        Me.dispStart = DisplayStartNone       ' 仕掛前
    '                '        Return

    '                '    Else
    '                '        Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                '        Me.dispStart = DisplayStartStart      ' 仕掛中
    '                '        Return
    '                '    End If
    '                'End If
    '                Dim deliveryCheck As Boolean
    '                deliveryCheck = CheckChipStatusDelivery(row)
    '                If deliveryCheck = True Then
    '                    Return
    '                End If
    '                '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
    '                ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    '                ' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END
    '            End If
    '        End If

    '        ' 4 : 納車作業チェック

    '        ' R/Oステータスチェック
    '        If row.RO_STATUS.Equals(C_RO_STATUS_SALE_OK) Then
    '            ' R/Oステータス：売上済み
    '            Me.dispDiv = DisplayDivDelivery       ' 納車作業
    '            Me.dispStart = DisplayStartStart      ' 仕掛中
    '            Return

    '        ElseIf row.RO_STATUS.Equals(C_RO_STATUS_MANT_OK) Then
    '            ' R/Oステータス：整備完了
    '            Me.dispDiv = DisplayDivDelivery       ' 納車作業
    '            Me.dispStart = DisplayStartStart      ' 仕掛中
    '            Return

    '            'ElseIf row.RO_STATUS.Equals(C_RO_STATUS_ESTI_WAIT) Then
    '            '    ' R/Oステータス：見積確認待ち
    '            '    Me.mDispDiv = DisplayDivDelivery       ' 納車作業
    '            '    Me.mDispStart = DisplayStartStart      ' 仕掛中
    '            '    Return
    '        End If

    '        ' 5 : 完了チェック 

    '        ' R/Oステータスチェック
    '        If row.RO_STATUS.Equals(C_RO_STATUS_FINISH) Then
    '            ' R/Oステータス：納車完了
    '            Me.dispDiv = DisplayDivNone       ' なし
    '            Me.dispStart = DisplayStartNone   ' 仕掛前
    '            Return
    '        End If

    '        ' その他 
    '        Me.dispDiv = DisplayDivNone       ' なし
    '        Me.dispStart = DisplayStartNone   ' 仕掛前

    '        '' 終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))
    '    End Sub
    '    ' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）SATRT
    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 納車準備チェック
    '    ''' </summary>
    '    ''' <param name="row">来店チップレコード</param>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function CheckChipStatusDelivery(ByVal row As SC3140103VisitRow) As Boolean
    '        Dim bizAddRepairStatusList As New IC3800804BusinessLogic
    '        Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
    '            DirectCast(bizAddRepairStatusList.GetAddRepairStatusList(row.DLRCD, row.ORDERNO),  _
    '                IC3800804AddRepairStatusDataTableDataTable)

    '        If Not IsNothing(dtAddRepairStatus) Or 0 < dtAddRepairStatus.Count Then
    '            '追加作業が存在する場合
    '            Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
    '                (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray
    '            If 0 < rowAddList.Count Then
    '                '「追加作業ステータス≠9」が存在する場合
    '                Me.dispDiv = DisplayDivWork       ' 作業中
    '                Me.dispStart = DisplayStartStart  ' 仕掛中
    '                Return True
    '            Else
    '                '「追加作業ステータス≠9」が存在しない場合
    '                If "1".Equals(row.WASHFLG) AndAlso _
    '                   (row.IsRESULT_WASH_STARTNull OrElse _
    '                   String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '                   row.RESULT_WASH_START.Count = 0) Then
    '                    '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                    Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                    Me.dispStart = DisplayStartNone       ' 仕掛前
    '                    Return True
    '                Else
    '                    Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                    Me.dispStart = DisplayStartStart      ' 仕掛中
    '                    Return True
    '                End If
    '            End If
    '        Else
    '            '追加作業が存在しない場合
    '            If "1".Equals(row.WASHFLG) AndAlso _
    '               (row.IsRESULT_WASH_STARTNull OrElse _
    '               String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '               row.RESULT_WASH_START.Count = 0) Then
    '                '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                Me.dispStart = DisplayStartNone       ' 仕掛前
    '                Return True
    '            Else
    '                Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                Me.dispStart = DisplayStartStart      ' 仕掛中
    '                Return True
    '            End If
    '        End If
    '        Return False
    '    End Function
    '    ' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

    '    ' 2012/06/05 日比野　事前準備対応　START
    '    ' 2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ詳細ボタンチェック
    '    ''' </summary>
    '    ''' <param name="inCustomerType">顧客区分(1:自社客  2:未取引客)</param>
    '    ''' <param name="inDisplayDiv">表示区分</param>
    '    ''' <param name="inOrderStatus">R/Oステータス</param>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Sub CheckChipDetailButton(ByVal inCustomerType As String, _
    '                                     ByVal inDisplayDiv As String, _
    '                                     ByVal inOrderStatus As String, _
    '                                     ByVal inOrderNo As String, _
    '                                     ByVal inCheckSheetStatus As String)
    '        'Private Sub CheckChipDetailButton(ByVal row As SC3140103VisitRow, ByVal dispDiv As String, ByVal orderStatus As String)


    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} IN:customerType = {3}, displayDiv = {4}, orderStatus = {5}, orderNo = {6}, checkSheetStatus = {7}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START _
    '                                , inCustomerType _
    '                                , inDisplayDiv _
    '                                , inOrderStatus _
    '                                , inOrderNo _
    '                                , inCheckSheetStatus))

    '        '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '        Me.buttonLeft = ButtonNone
    '        Me.buttonRight = ButtonNone
    '        Me.buttonEnabledLeft = True
    '        Me.buttonEnabledRight = True


    '        ' チップ情報チェック
    '        Select Case inDisplayDiv
    '            Case DisplayDivAdvance         '事前準備
    '                If inCustomerType.Equals(CustomerSegmentON) Then
    '                    ' 自社客
    '                    Me.buttonLeft = ButtonCustomer

    '                Else
    '                    ' 未取引客
    '                    Me.buttonLeft = ButtonNewCustomer
    '                    ' R/O 作成ロック
    '                    Me.buttonEnabledRight = False
    '                End If

    '                Me.buttonRight = ButtonNewRO
    '            Case DisplayDivReception       ' 受付
    '                ' 顧客区分チェック
    '                If inCustomerType.Equals(CustomerSegmentON) Then
    '                    ' 自社客
    '                    Me.buttonLeft = ButtonCustomer

    '                Else
    '                    ' 未取引客
    '                    Me.buttonLeft = ButtonNewCustomer
    '                    ' R/O 作成ロック
    '                    Me.buttonEnabledRight = False

    '                End If

    '                Me.buttonRight = ButtonNewRO

    '            Case DisplayDivApproval        ' 承認依頼
    '                Me.buttonLeft = ButtonRODisplay
    '                Me.buttonRight = ButtonApproval

    '            Case DisplayDivPreparation     ' 納車準備
    '                ' チェックシート有無チェック
    '                ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 START
    '                'If Not row.CHECKSHEET_FLAG.Equals(C_CHECKSHEET_FLAG_ON) ThenC_NONE_CHECKSHEET_FLAG_ON
    '                If Not (inCheckSheetStatus.Equals(C_CHECKSHEET_FLAG_ON) Or inCheckSheetStatus.Equals(C_NONE_CHECKSHEET_FLAG_ON)) Then
    '                    ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 END
    '                    ' チェックシート印刷なし
    '                    Me.buttonEnabledLeft = False

    '                End If

    '                Me.buttonLeft = ButtonCheckSheet
    '                Me.buttonRight = ButtonSettlement

    '            Case DisplayDivDelivery        ' 納車作業
    '                ' チェックシート有無チェック
    '                ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 START
    '                'If Not row.CHECKSHEET_FLAG.Equals(C_CHECKSHEET_FLAG_ON) Then
    '                If Not (inCheckSheetStatus.Equals(C_CHECKSHEET_FLAG_ON) Or inCheckSheetStatus.Equals(C_NONE_CHECKSHEET_FLAG_ON)) Then
    '                    ' 2012/04/13 KN 森下【SERVICE_1】【開発チェック】次世代サービス_プレユーザーテスト_不具合管理表.xlsのNo9 チェックシートボタンの活性制御対応 END
    '                    ' チェックシート印刷なし
    '                    Me.buttonEnabledLeft = False

    '                End If

    '                Me.buttonLeft = ButtonCheckSheet
    '                Me.buttonRight = ButtonSettlement

    '            Case DisplayDivWork            ' 作業中
    '                Me.buttonLeft = ButtonRODisplay

    '                '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '                '遷移先画面取得
    '                Dim redirect As AddWorkRedirect = GetAddWorkRedirect(inOrderNo)

    '                '右ボタン名称設定・活性制御
    '                If redirect.Equals(AddWorkRedirect.SC3170302) Then
    '                    '「追加作業プレビュー」を設定
    '                    Me.buttonRight = ButtonWorkPreview

    '                    Me.buttonEnabledRight = True
    '                Else
    '                    '「追加作業登録」を設定
    '                    Me.buttonRight = ButtonWork

    '                    'ボタンの活性制御
    '                    If C_RO_STATUS_WORKING.Equals(inOrderStatus) OrElse _
    '                       C_RO_STATUS_INSP_OK.Equals(inOrderStatus) Then
    '                        'R/Oステータスが2(整備中)又は、7(検査完了)の場合、ボタン活性
    '                        Me.buttonEnabledRight = True
    '                    Else
    '                        'R/Oステータスが上記以外の場合、ボタン非活性
    '                        Me.buttonEnabledRight = False
    '                    End If
    '                End If
    '                '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '            Case Else

    '        End Select
    '        ' 2012/06/05 日比野　事前準備対応　END

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '    End Sub

    '#End Region

    '#Region " サービス来店実績・ストール予約・実績"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' サービス来店実績・ストール予約実績取得
    '    ''' </summary>
    '    ''' <param name="dtService">サービス来店情報データセット</param>
    '    ''' <param name="dtRezinfo">ストール予約データセット</param>
    '    ''' <param name="dtProcess">ストール実績データセット</param>
    '    ''' <returns>来店チップデータセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetVisit(ByVal dtService As SC3140103ServiceVisitManagementDataTable, _
    '                              ByVal dtRezinfo As SC3140103StallRezinfoDataTable, _
    '                              ByVal dtProcess As SC3140103StallProcessDataTable) As SC3140103VisitDataTable

    '        '開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START))

    '        Dim dt As SC3140103VisitDataTable = New SC3140103VisitDataTable
    '        Dim rowRezinfo As SC3140103StallRezinfoRow
    '        Dim rowProcess As SC3140103StallProcessRow

    '        For Each rowService As SC3140103ServiceVisitManagementRow In dtService.Rows
    '            Dim row As SC3140103VisitRow = dt.NewSC3140103VisitRow()

    '            'チップ情報ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "{0}.{1} {2} IN:VISITSEQ ={3}, FREZID ={4}" _
    '                                    , Me.GetType.ToString _
    '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                    , LOG_START _
    '                                    , rowService.VISITSEQ _
    '                                    , rowService.FREZID))

    '            ' 来店実績連番
    '            row.VISITSEQ = rowService.VISITSEQ
    '            ' 販売店コード
    '            row.DLRCD = rowService.DLRCD
    '            ' 店舗コード
    '            row.STRCD = rowService.STRCD
    '            ' 予約ID
    '            row.FREZID = rowService.FREZID
    '            ' 割振りSA
    '            row.SACODE = rowService.SACODE
    '            ' VIPマーク
    '            row.VIP_MARK = C_FLAG_OFF
    '            ' JDP調査対象客マーク
    '            row.JDP_MARK = C_FLAG_OFF
    '            ' 技術情報マーク
    '            row.SSC_MARK = C_FLAG_OFF
    '            ' 駐車場コード
    '            row.PARKINGCODE = rowService.PARKINGCODE
    '            ' 来店時刻
    '            row.VISITTIMESTAMP = rowService.VISITTIMESTAMP
    '            ' チェックシート有無
    '            row.CHECKSHEET_FLAG = C_FLAG_OFF
    '            ' SA割振り日時
    '            row.ASSIGNTIMESTAMP = rowService.ASSIGNTIMESTAMP
    '            ' 整備受注NO
    '            row.ORDERNO = rowService.ORDERNO
    '            ' 追加作業承認数
    '            row.APPROVAL_COUNT = 0
    '            ' 追加作業承認
    '            row.APPROVAL_STATUS = C_FLAG_OFF
    '            ' 追加作業承認印刷
    '            row.APPROVAL_OUTPUT = C_FLAG_OFF
    '            ' 追加作業承認依頼時刻
    '            row.APPROVAL_TIME = DateTime.MinValue
    '            ' 顧客区分
    '            row.CUSTSEGMENT = rowService.CUSTSEGMENT
    '            ' 顧客コード
    '            row.CUSTID = rowService.DMSID
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '            ' 呼出ステータス
    '            row.CALLSTATUS = rowService.CALLSTATUS
    '            '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    '            'ストール予約取得
    '            Dim fRezId As Long
    '            If Not rowService.IsFREZIDNull() Then
    '                fRezId = rowService.FREZID
    '            Else
    '                fRezId = SC3140103DataTableAdapter.MinReserveId
    '            End If
    '            rowRezinfo = Me.GetVisitRezinfo(fRezId, dtRezinfo)
    '            ' 予約マーク
    '            If rowRezinfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
    '                row.REZ_MARK = C_FLAG_ON
    '            Else
    '                row.REZ_MARK = C_FLAG_OFF
    '            End If
    '            ' 登録番号
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.VCLREGNO = Me.SetReplaceString(rowRezinfo.VCLREGNO, rowService.VCLREGNO)
    '            row.VCLREGNO = Me.SetReplaceString(rowService.VCLREGNO, rowRezinfo.VCLREGNO)
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' 車種
    '            row.VEHICLENAME = rowRezinfo.VEHICLENAME
    '            '' モデル
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.MODELCODE = Me.SetReplaceString(rowRezinfo.MODELCODE, rowService.MODELCODE)
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '            'グレード(MODELは英語表記上GRADE)
    '            'row.MODELCODE = Me.SetReplaceString(rowService.MODELCODE, rowRezinfo.MODELCODE)
    '            row.GRADE = Me.SetReplaceString(rowService.MODELCODE, rowRezinfo.MODELCODE)
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' VIN
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.VIN = Me.SetReplaceString(rowRezinfo.VIN, rowService.VIN)
    '            row.VIN = Me.SetReplaceString(rowService.VIN, rowRezinfo.VIN)
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' 走行距離
    '            row.MILEAGE = rowRezinfo.MILEAGE
    '            ' 納車予定日時
    '            row.REZ_DELI_DATE = rowRezinfo.REZ_DELI_DATE
    '            ' 顧客名
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.CUSTOMERNAME = Me.SetReplaceString(rowRezinfo.CUSTOMERNAME, rowService.NAME)
    '            row.CUSTOMERNAME = Me.SetReplaceString(rowService.NAME, rowRezinfo.CUSTOMERNAME)
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' 電話番号
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.TELNO = Me.SetReplaceString(rowRezinfo.TELNO, rowService.TELNO)
    '            row.TELNO = Me.SetReplaceString(rowService.TELNO, rowRezinfo.TELNO)
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' 携帯番号
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START
    '            'row.MOBILE = Me.SetReplaceString(rowRezinfo.MOBILE, rowService.MOBILE)
    '            row.MOBILE = Me.SetReplaceString(rowService.MOBILE, rowRezinfo.MOBILE)
    '            '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END
    '            ' 代表入庫項目
    '            row.MERCHANDISENAME = rowRezinfo.MERCHANDISENAME
    '            '2012/04/04 nishida プレユーザーテスト課題 No.91 作業予定の無い場合、作業完了予定時刻の表示を「00'00」対応 START
    '            '作業開始予定時刻
    '            row.STARTTIME = rowRezinfo.STARTTIME
    '            '2012/04/04 nishida プレユーザーテスト課題 No.91 作業予定の無い場合、作業完了予定時刻の表示を「00'00」対応
    '            ' 作業終了予定時刻
    '            row.ENDTIME = rowRezinfo.ENDTIME
    '            ' 作業開始
    '            row.ACTUAL_STIME = rowRezinfo.ACTUAL_STIME
    '            ' 作業終了
    '            row.ACTUAL_ETIME = rowRezinfo.ACTUAL_ETIME
    '            ' 完成検査有無
    '            row.COMP_INS_FLAG = C_COMP_INS_FLAG_ON
    '            ' 洗車有無
    '            row.WASHFLG = rowRezinfo.WASHFLG
    '            ' 予約ID
    '            row.REZINFO_FREZID = rowRezinfo.PREZID

    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            ' ストール実績取得
    '            Dim rezId As Long
    '            If Not rowService.IsREZIDNull() Then
    '                rezId = rowRezinfo.REZID
    '            Else
    '                rezId = SC3140103DataTableAdapter.MinReserveId
    '            End If
    '            'rowProcess = Me.GetVisitProcess(rezId, dtProcess)
    '            rowProcess = Me.GetVisitProcess(fRezId, dtProcess)
    '            '2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            ' 実績ステータス
    '            row.RESULT_STATUS = rowProcess.RESULT_STATUS

    '            '2012/08/22 TMEJ 日比野【SERVICE_2】チップのカウンターが非表示となる不具合の修正　START
    '            Dim rowProcessAsRezId As SC3140103StallProcessRow = Me.GetVisitProcess(rezId, dtProcess)

    '            ' 作業終了予定時刻
    '            If Not String.IsNullOrEmpty(rowProcessAsRezId.REZ_END_TIME) Then
    '                ' 予定_ストール終了日時時刻で更新
    '                'row.ENDTIME = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowProcess.REZ_END_TIME)
    '                row.ENDTIME = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowProcessAsRezId.REZ_END_TIME)
    '            End If
    '            '2012/08/22 TMEJ 日比野【SERVICE_2】チップのカウンターが非表示となる不具合の修正　END

    '            ' 洗車開始
    '            row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
    '            ' 洗車終了
    '            row.RESULT_WASH_END = rowProcess.RESULT_WASH_END
    '            ' 担当テクニシャン
    '            row.STAFFCD = 0
    '            ' 担当テクニシャン名
    '            row.STAFFNAME = rowProcess.STAFFNAME
    '            ' 予約ID
    '            row.PROCESS_FREZID = rowRezinfo.PREZID

    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '            '作業開始日時(初回)
    '            row.FIRST_STARTTIME = rowProcess.FIRST_STARTTIME
    '            '使用終了日時(最後)
    '            row.LAST_ENDTIME = rowProcess.LAST_ENDTIME
    '            '作業時間(未実施合計)
    '            row.WORK_TIME = rowProcess.WORK_TIME
    '            '有効以外件数
    '            row.UNVALID_REZ_COUNT = rowProcess.UNVALID_REZ_COUNT
    '            ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END

    '            dt.AddSC3140103VisitRow(row)
    '        Next

    '        '終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) START

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' チップ詳細-サービス来店実績・ストール予約実績取得
    '    ''' </summary>
    '    ''' <param name="dtService">サービス来店情報データセット</param>
    '    ''' <param name="dtRezinfo">ストール予約データセット</param>
    '    ''' <param name="dtProcess">ストール実績データセット</param>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <param name="displayDiv">表示区分</param>
    '    ''' <returns>来店チップデータセット</returns>
    '    ''' <remarks></remarks>
    '    ''' <histiry>
    '    ''' 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34）
    '    ''' </histiry>
    '    '''-------------------------------------------------------
    '    Private Function SetVisitDetail(ByVal dtService As SC3140103ServiceVisitManagementDataTable, ByVal dtRezinfo As SC3140103StallRezinfoDataTable, ByVal dtProcess As SC3140103StallProcessDataTable, ByVal staffInfo As StaffContext, ByVal displayDiv As String) As SC3140103VisitDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim dt As SC3140103VisitDataTable = New SC3140103VisitDataTable
    '        Dim rowRezinfo As SC3140103StallRezinfoRow
    '        Dim rowProcess As SC3140103StallProcessRow

    '        'R/O基本情報参照
    '        Dim dtIFOrderCommon As IC3801001OrderCommDataTable
    '        ' 顧客参照
    '        Dim dtIFSrvCustomerDataTable As IC3800703SrvCustomerDataTable

    '        For Each rowService As SC3140103ServiceVisitManagementRow In dtService.Rows
    '            Dim row As SC3140103VisitRow = dt.NewSC3140103VisitRow()

    '            'チップ情報ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "{0}.{1} {2} IN:VISITSEQ ={3}, FREZID ={4}" _
    '                                    , Me.GetType.ToString _
    '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                    , LOG_START _
    '                                    , rowService.VISITSEQ _
    '                                    , rowService.FREZID))

    '            ' 来店実績連番
    '            row.VISITSEQ = rowService.VISITSEQ
    '            ' 販売店コード
    '            row.DLRCD = rowService.DLRCD
    '            ' 店舗コード
    '            row.STRCD = rowService.STRCD
    '            ' 予約ID
    '            row.FREZID = rowService.FREZID
    '            ' 割振りSA
    '            row.SACODE = rowService.SACODE
    '            ' VIPマーク
    '            row.VIP_MARK = C_FLAG_OFF
    '            ' JDP調査対象客マーク
    '            row.JDP_MARK = C_FLAG_OFF
    '            ' 技術情報マーク
    '            row.SSC_MARK = C_FLAG_OFF
    '            ' 駐車場コード
    '            row.PARKINGCODE = rowService.PARKINGCODE
    '            ' 来店時刻
    '            row.VISITTIMESTAMP = rowService.VISITTIMESTAMP
    '            ' チェックシート有無
    '            row.CHECKSHEET_FLAG = C_FLAG_OFF
    '            ' SA割振り日時
    '            row.ASSIGNTIMESTAMP = rowService.ASSIGNTIMESTAMP
    '            ' 整備受注NO
    '            row.ORDERNO = rowService.ORDERNO
    '            ' 追加作業承認数
    '            row.APPROVAL_COUNT = 0
    '            ' 追加作業承認
    '            row.APPROVAL_STATUS = C_FLAG_OFF
    '            ' 追加作業承認印刷
    '            row.APPROVAL_OUTPUT = C_FLAG_OFF
    '            ' 追加作業承認依頼時刻
    '            row.APPROVAL_TIME = DateTime.MinValue
    '            ' 顧客区分
    '            row.CUSTSEGMENT = rowService.CUSTSEGMENT
    '            ' 顧客コード
    '            row.CUSTID = rowService.DMSID
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '            ' 納車日
    '            row.DELIVERDATE = DateTime.MinValue
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END

    '            'ストール予約取得
    '            Dim fRezId As Long
    '            If Not rowService.IsFREZIDNull() Then
    '                fRezId = rowService.FREZID
    '            Else
    '                fRezId = SC3140103DataTableAdapter.MinReserveId
    '            End If
    '            rowRezinfo = Me.GetVisitRezinfo(fRezId, dtRezinfo)
    '            ' 予約マーク
    '            If rowRezinfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
    '                row.REZ_MARK = C_FLAG_ON
    '            Else
    '                row.REZ_MARK = C_FLAG_OFF
    '            End If

    '            ' R/O基本情報参照
    '            If Not String.IsNullOrEmpty(rowService.ORDERNO) Then
    '                ' IF-R/O基本情報参照処理
    '                dtIFOrderCommon = Me.GetIFROBaseInformationList(staffInfo, rowService.ORDERNO)
    '                ' IFマージ処理
    '                rowRezinfo = Me.SetVisitDetailOrderMargin(rowRezinfo, dtIFOrderCommon)

    '                ' R/O基本情報の値を反映
    '                If dtIFOrderCommon IsNot Nothing AndAlso dtIFOrderCommon.Rows.Count > 0 Then
    '                    Dim rowIFOrderCommon As IC3801001OrderCommRow = DirectCast(dtIFOrderCommon.Rows(0), IC3801001OrderCommRow)

    '                    If Not IsDBNull(rowIFOrderCommon.Item("OrderIFlag")) Then
    '                        row.JDP_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderIFlag)                           'JDP調査対象客フラグ
    '                    End If
    '                    If Not IsDBNull(rowIFOrderCommon.Item("OrderSFlag")) Then
    '                        row.SSC_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderSFlag)                           'SSCフラグ
    '                    End If
    '                    If Not IsDBNull(rowIFOrderCommon.Item("printFlag")) Then
    '                        row.CHECKSHEET_FLAG = Me.SetReplaceString(String.Empty, rowIFOrderCommon.printFlag)                     'チェックシート印刷有無
    '                    End If
    '                    '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 START
    '                    If Not IsDBNull(rowIFOrderCommon.Item("DeliverDate")) Then
    '                        '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 START
    '                        'row.DELIVERDATE = rowIFOrderCommon.DeliverDate                                                          '納車日
    '                        If Not String.IsNullOrEmpty(rowIFOrderCommon.DeliverDate) Then
    '                            row.DELIVERDATE = CType(rowIFOrderCommon.DeliverDate, Date)                                          '納車日
    '                        End If
    '                        '2012/04/16 KN 森下 【SERVICE_1】IFの項目タイプ変更対応 END
    '                    End If
    '                    '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.90 納車予定時刻の表示対応 END

    '                End If
    '                '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '                If dtIFOrderCommon IsNot Nothing AndAlso dtIFOrderCommon.Rows.Count > 0 Then
    '                    Me.orderStatus = dtIFOrderCommon(0).OrderStatus
    '                End If
    '                '2012/03/20 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '            End If

    '            ' 登録番号
    '            row.VCLREGNO = Me.SetReplaceString(rowService.VCLREGNO, rowRezinfo.VCLREGNO)
    '            ' 車種
    '            row.VEHICLENAME = rowRezinfo.VEHICLENAME
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 START
    '            ' モデル
    '            'row.MODELCODE = Me.SetReplaceString(rowService.MODELCODE, rowRezinfo.MODELCODE)
    '            row.MODELCODE = rowRezinfo.MODELCODE
    '            ' グレード
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 START
    '            'row.GRADE = Me.SetReplaceString(rowService.MODELCODE, rowRezinfo.GRADE)
    '            row.GRADE = rowRezinfo.GRADE
    '            '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.228 チップ詳細のグレードを表示するための項目を変更 END
    '            '2012/04/02 KN 森下 【SERVICE_1】モデルの使用項目の修正 END
    '            ' VIN
    '            row.VIN = Me.SetReplaceString(rowService.VIN, rowRezinfo.VIN)
    '            ' 走行距離
    '            row.MILEAGE = rowRezinfo.MILEAGE
    '            ' 納車予定日時
    '            row.REZ_DELI_DATE = rowRezinfo.REZ_DELI_DATE
    '            ' 顧客名
    '            row.CUSTOMERNAME = Me.SetReplaceString(rowService.NAME, rowRezinfo.CUSTOMERNAME)
    '            ' 電話番号
    '            row.TELNO = Me.SetReplaceString(rowService.TELNO, rowRezinfo.TELNO)
    '            ' 携帯番号
    '            row.MOBILE = Me.SetReplaceString(rowService.MOBILE, rowRezinfo.MOBILE)

    '            ' 顧客参照IF
    '            '2012/04/18 KN 森下 【SERVICE_1】仕様変更対応(様変更管理状況.xls No20 判定条件に車両登録ナンバー追加) START
    '            'If Not String.IsNullOrEmpty(row.VIN.Trim()) Then
    '            If (Not String.IsNullOrEmpty(row.VIN.Trim())) Or (Not String.IsNullOrEmpty(row.VCLREGNO.Trim())) Then
    '                '2012/04/18 KN 森下 【SERVICE_1】仕様変更対応(様変更管理状況.xls No20 判定条件に車両登録ナンバー追加) END
    '                'IF-顧客参照処理
    '                dtIFSrvCustomerDataTable = Me.GetIFCustomerInformation(row)
    '                ' IFマージ処理
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 START
    '                'row = Me.SetVisitDetailCustomerMargin(row, dtIFSrvCustomerDataTable)
    '                row = Me.SetVisitDetailCustomerMargin(row, dtIFSrvCustomerDataTable, displayDiv)
    '                '2012/04/10 KN 森下 【SERVICE_1】企画_プレユーザーテスト課題 No.39 受付エリアのチップのssc,jdpマーク表示対応 END
    '            End If

    '            ' 代表入庫項目
    '            row.MERCHANDISENAME = rowRezinfo.MERCHANDISENAME
    '            ' 作業終了予定時刻
    '            row.ENDTIME = rowRezinfo.ENDTIME
    '            ' 作業開始
    '            row.ACTUAL_STIME = rowRezinfo.ACTUAL_STIME
    '            ' 作業終了
    '            row.ACTUAL_ETIME = rowRezinfo.ACTUAL_ETIME
    '            ' 完成検査有無
    '            row.COMP_INS_FLAG = C_COMP_INS_FLAG_ON
    '            ' 洗車有無
    '            row.WASHFLG = rowRezinfo.WASHFLG
    '            ' 予約ID
    '            row.REZINFO_FREZID = rowRezinfo.PREZID

    '            ' ストール実績取得
    '            Dim rezId As Long
    '            If Not rowService.IsREZIDNull() Then
    '                rezId = rowRezinfo.REZID
    '            Else
    '                rezId = SC3140103DataTableAdapter.MinReserveId
    '            End If
    '            rowProcess = Me.GetVisitProcess(rezId, dtProcess)
    '            ' 実績ステータス
    '            row.RESULT_STATUS = rowProcess.RESULT_STATUS
    '            ' 作業終了予定時刻
    '            If Not String.IsNullOrEmpty(rowProcess.REZ_END_TIME) Then
    '                ' 予定_ストール終了日時時刻で更新
    '                row.ENDTIME = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowProcess.REZ_END_TIME)
    '            End If
    '            ' 洗車開始
    '            row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
    '            ' 洗車終了
    '            row.RESULT_WASH_END = rowProcess.RESULT_WASH_END
    '            ' 担当テクニシャン
    '            row.STAFFCD = 0
    '            ' 担当テクニシャン名
    '            row.STAFFNAME = rowProcess.STAFFNAME
    '            ' 予約ID
    '            row.PROCESS_FREZID = rowRezinfo.PREZID

    '            ' 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    '            ' 顧客コード
    '            row.CUSTCD = rowRezinfo.CUSTCD
    '            ' 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

    '            dt.AddSC3140103VisitRow(row)
    '        Next

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '2012/03/19 KN 森下 【SERVICE_1】仕様変更対応(顧客情報の表示優先順位) END

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' ストール予約取得
    '    ''' </summary>
    '    ''' <param name="fRezId">初回予約ID</param>
    '    ''' <param name="dtRezinfo">ストール予約データセット</param>
    '    ''' <returns>ストール予約レコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetVisitRezinfo(ByVal fRezId As Long, ByVal dtRezinfo As SC3140103StallRezinfoDataTable) As SC3140103StallRezinfoRow

    '        ''開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2} IN:fRezId ={3}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START _
    '        '                        , fRezId))

    '        Dim row As SC3140103StallRezinfoRow = dtRezinfo.NewSC3140103StallRezinfoRow()
    '        Dim aryDtRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", fRezId), " ENDTIME DESC, STARTTIME DESC")

    '        ' 件数チェック
    '        If aryDtRezInfo Is Nothing OrElse aryDtRezInfo.Length = 0 Then
    '            ' 該当行無し
    '            ' 販売店コード
    '            row.DLRCD = String.Empty
    '            ' 店舗コード
    '            row.STRCD = String.Empty
    '            ' 予約ID
    '            'row.REZID = fRezId
    '            row.REZID = SC3140103DataTableAdapter.MinReserveId
    '            ' 管理予約ID
    '            'row.PREZID = fRezId
    '            row.PREZID = SC3140103DataTableAdapter.MinReserveId
    '            ' 使用開始日時
    '            row.STARTTIME = DateTime.MinValue
    '            ' 使用開始日時
    '            row.ENDTIME = DateTime.MinValue
    '            ' 顧客コード
    '            row.CUSTCD = String.Empty
    '            ' 氏名
    '            row.CUSTOMERNAME = String.Empty
    '            ' 電話番号
    '            row.TELNO = String.Empty
    '            ' 携帯番号
    '            row.MOBILE = String.Empty
    '            ' 車名
    '            row.VEHICLENAME = String.Empty
    '            ' 登録ナンバー
    '            row.VCLREGNO = String.Empty
    '            ' VIN
    '            row.VIN = String.Empty
    '            ' 商品コード
    '            row.MERCHANDISECD = String.Empty
    '            ' 商品名
    '            row.MERCHANDISENAME = String.Empty
    '            ' モデル
    '            row.MODELCODE = String.Empty
    '            ' 走行距離
    '            row.MILEAGE = -1
    '            '2012/04/18 KN 上田 【SERVICE_1】ユーザテスト課題 No.76 予約無し時の洗車フラグの初期値を"0"⇒空白に修正 START
    '            ' 洗車有無
    '            row.WASHFLG = String.Empty
    '            'row.WASHFLG = C_FLAG_OFF
    '            '2012/04/18 KN 上田 【SERVICE_1】ユーザテスト課題 No.76 予約無し時の洗車フラグの初期値を"0"⇒空白に修正 END
    '            ' 来店フラグ
    '            row.WALKIN = String.Empty
    '            ' 予約_納車_希望日時時刻
    '            row.REZ_DELI_DATE = String.Empty
    '            ' 作業開始
    '            row.ACTUAL_STIME = DateTime.MinValue
    '            ' 作業終了
    '            row.ACTUAL_ETIME = DateTime.MinValue
    '        Else
    '            Dim rowRezInfo As SC3140103StallRezinfoRow = DirectCast(aryDtRezInfo(0), SC3140103StallRezinfoRow)

    '            ' 販売店コード
    '            row.DLRCD = rowRezInfo.DLRCD
    '            ' 店舗コード
    '            row.STRCD = rowRezInfo.STRCD
    '            ' 予約ID
    '            row.REZID = rowRezInfo.REZID
    '            ' 管理予約ID
    '            row.PREZID = rowRezInfo.PREZID
    '            ' 使用開始日時
    '            row.STARTTIME = rowRezInfo.STARTTIME
    '            ' 使用開始日時
    '            row.ENDTIME = rowRezInfo.ENDTIME
    '            ' 顧客コード
    '            row.CUSTCD = rowRezInfo.CUSTCD
    '            ' 氏名
    '            row.CUSTOMERNAME = rowRezInfo.CUSTOMERNAME
    '            ' 電話番号
    '            row.TELNO = rowRezInfo.TELNO
    '            ' 携帯番号
    '            row.MOBILE = rowRezInfo.MOBILE
    '            ' 車名
    '            row.VEHICLENAME = rowRezInfo.VEHICLENAME
    '            ' 登録ナンバー
    '            row.VCLREGNO = rowRezInfo.VCLREGNO
    '            ' VIN
    '            row.VIN = rowRezInfo.VIN

    '            '2012/08/22 TMEJ 日比野【SERVICE_2】チップに代表整備項目が表示されないことがある START
    '            Dim dtParentRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "REZID = {0}", fRezId))

    '            If dtParentRezInfo IsNot Nothing AndAlso dtParentRezInfo.Length = 0 Then
    '                ' 商品コード
    '                row.MERCHANDISECD = String.Empty
    '                ' 商品名
    '                row.MERCHANDISENAME = String.Empty
    '            Else
    '                Dim rowParentRezInfo As SC3140103StallRezinfoRow = DirectCast(dtParentRezInfo(0), SC3140103StallRezinfoRow)

    '                ' 商品コード
    '                row.MERCHANDISECD = rowParentRezInfo.MERCHANDISECD
    '                ' 商品名
    '                row.MERCHANDISENAME = rowParentRezInfo.MERCHANDISENAME
    '            End If


    '            '2012/08/22 TMEJ 日比野【SERVICE_2】チップに代表整備項目が表示されないことがある END
    '            ' モデル
    '            row.MODELCODE = rowRezInfo.MODELCODE
    '            ' 走行距離
    '            row.MILEAGE = rowRezInfo.MILEAGE
    '            ' 洗車有無
    '            row.WASHFLG = rowRezInfo.WASHFLG
    '            ' 来店フラグ
    '            row.WALKIN = rowRezInfo.WALKIN
    '            ' 予約_納車_希望日時時刻
    '            row.REZ_DELI_DATE = rowRezInfo.REZ_DELI_DATE
    '            ' 作業開始
    '            row.ACTUAL_STIME = rowRezInfo.ACTUAL_STIME
    '            ' 作業終了
    '            row.ACTUAL_ETIME = rowRezInfo.ACTUAL_ETIME

    '            ' 作業開始の取得(最小)
    '            Dim rowRezInfoMin As SC3140103StallRezinfoRow
    '            Dim aryDtRezInfoMin As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0} AND ACTUAL_STIME <> '{1}'", fRezId, DateTime.MinValue), " ACTUAL_STIME")

    '            If aryDtRezInfoMin IsNot Nothing AndAlso aryDtRezInfoMin.Length > 0 Then
    '                rowRezInfoMin = DirectCast(aryDtRezInfoMin(0), SC3140103StallRezinfoRow)
    '                ' 作業開始
    '                row.ACTUAL_STIME = rowRezInfoMin.ACTUAL_STIME
    '            End If

    '        End If

    '        ''終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))

    '        Return row
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' ストール実績取得
    '    ''' </summary>
    '    ''' <param name="fRezId">予約ID</param>
    '    ''' <param name="dtProcess">ストール実績データセット</param>
    '    ''' <returns>ストール実績レコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetVisitProcess(ByVal fRezId As Long, _
    '                                     ByVal dtProcess As SC3140103StallProcessDataTable) _
    '                                 As SC3140103StallProcessRow

    '        ''開始ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2} IN:rezId ={3}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_START _
    '        '                        , rezId.ToString(CultureInfo.CurrentCulture)))

    '        Dim row As SC3140103StallProcessRow = dtProcess.NewSC3140103StallProcessRow()

    '        'Dim aryProcess As DataRow() = dtProcess.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", rezId), "DSEQNO DESC, SEQNO DESC, STAFFNAME ASC")
    '        Dim aryProcess As DataRow() = dtProcess.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                                                     "PREZID = {0}", fRezId))
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '        ' 件数チェック
    '        If aryProcess Is Nothing OrElse aryProcess.Length = 0 Then
    '            '該当行無し
    '            '' 販売店コード
    '            'row.DLRCD = String.Empty
    '            '' 店舗コード
    '            'row.STRCD = String.Empty
    '            '' 予約ID
    '            ''row.REZID = rezId
    '            'row.REZID = SC3140103DataTableAdapter.MinReserveId
    '            '' 管理予約ID
    '            ''row.PREZID = rezId
    '            'row.PREZID = SC3140103DataTableAdapter.MinReserveId
    '            '' 日跨ぎシーケンス番号
    '            'row.DSEQNO = 0
    '            '' シーケンス番号
    '            'row.SEQNO = 0
    '            '2012/04/18 KN 上田 【SERVICE_1】ユーザテスト課題 No.76 予約無し時の洗車フラグの初期値を"0"⇒空白に修正 START
    '            ' 洗車有無
    '            row.WASHFLG = String.Empty
    '            'row.WASHFLG = C_FLAG_OFF
    '            '2012/04/18 KN 上田 【SERVICE_1】ユーザテスト課題 No.76 予約無し時の洗車フラグの初期値を"0"⇒空白に修正 END
    '            ' 実績_ステータス
    '            row.RESULT_STATUS = C_STALLPROSESS_NONE
    '            ' 予定_ストール終了日時時刻
    '            row.REZ_END_TIME = String.Empty
    '            ' 洗車開始
    '            row.RESULT_WASH_START = String.Empty
    '            ' 洗車終了
    '            row.RESULT_WASH_END = String.Empty
    '            ' 担当テクニシャン
    '            row.STAFFCD = String.Empty
    '            ' 担当テクニシャン名
    '            row.STAFFNAME = String.Empty
    '            '作業開始日時(初回)
    '            row.FIRST_STARTTIME = Nothing
    '            '使用終了日時(最後)
    '            row.LAST_ENDTIME = Date.MinValue
    '            '作業時間(未実施合計)
    '            row.WORK_TIME = 0
    '            '有効以外件数
    '            row.UNVALID_REZ_COUNT = 0
    '        Else
    '            Dim rowProcess As SC3140103StallProcessRow

    '            rowProcess = DirectCast(aryProcess(0), SC3140103StallProcessRow)

    '            '' 販売店コード
    '            'row.DLRCD = rowProcess.DLRCD
    '            '' 店舗コード
    '            'row.STRCD = rowProcess.STRCD
    '            '' 予約ID
    '            'row.REZID = rowProcess.REZID
    '            '' 管理予約ID
    '            'row.PREZID = rowProcess.PREZID
    '            '' 日跨ぎシーケンス番号
    '            'row.DSEQNO = rowProcess.DSEQNO
    '            '' シーケンス番号
    '            'row.SEQNO = rowProcess.SEQNO
    '            ' 洗車有無
    '            row.WASHFLG = rowProcess.WASHFLG

    '            ' 予定_ストール終了日時時刻
    '            row.REZ_END_TIME = rowProcess.REZ_END_TIME
    '            ' 洗車開始
    '            row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
    '            ' 洗車終了
    '            row.RESULT_WASH_END = rowProcess.RESULT_WASH_END

    '            If C_RESULT_TYPE_TRUE.Equals(rowProcess.RESULT_TYPE) Then
    '                '実績ありの場合

    '                ' 実績_ステータス
    '                row.RESULT_STATUS = rowProcess.RESULT_STATUS
    '                ' 担当テクニシャン
    '                row.STAFFCD = rowProcess.STAFFCD
    '                ' 担当テクニシャン名
    '                row.STAFFNAME = rowProcess.STAFFNAME
    '            Else
    '                '実績なしの場合

    '                ' 実績_ステータス
    '                row.RESULT_STATUS = String.Empty
    '                ' 担当テクニシャン
    '                row.STAFFCD = String.Empty
    '                ' 担当テクニシャン名
    '                row.STAFFNAME = String.Empty
    '            End If

    '            '作業開始日時(初回)
    '            If rowProcess.IsFIRST_STARTTIMENull Then
    '                row.FIRST_STARTTIME = Nothing
    '            Else
    '                row.FIRST_STARTTIME = rowProcess.FIRST_STARTTIME
    '            End If
    '            '使用終了日時(最後)
    '            If rowProcess.IsLAST_ENDTIMENull Then
    '                row.LAST_ENDTIME = Date.MinValue
    '            Else
    '                row.LAST_ENDTIME = rowProcess.LAST_ENDTIME
    '            End If
    '            '作業時間(未実施合計)
    '            If rowProcess.IsWORK_TIMENull Then
    '                row.WORK_TIME = 0
    '            Else
    '                row.WORK_TIME = rowProcess.WORK_TIME
    '            End If

    '            '有効以外件数
    '            If rowProcess.IsUNVALID_REZ_COUNTNull Then
    '                row.UNVALID_REZ_COUNT = 0
    '            Else
    '                row.UNVALID_REZ_COUNT = rowProcess.UNVALID_REZ_COUNT
    '            End If
    '        End If
    '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '        ''終了ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                        , "{0}.{1} {2}" _
    '        '                        , Me.GetType.ToString _
    '        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        '                        , LOG_END))

    '        Return row
    '    End Function

    '#End Region

    '#Region " 外部IF処理"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' SA別未納者R/O一覧
    '    ''' </summary>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <returns>SA別未納者R/O一覧データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetIFNoDeliveryROList(ByVal staffInfo As StaffContext) As IC3801003NoDeliveryRODataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bl As IC3801003BusinessLogic = New IC3801003BusinessLogic
    '        Dim dt As IC3801003NoDeliveryRODataTable




    '        '検索処理
    '        ' IF用にSAコードの調整-「@」より前のSAコード取得
    '        Dim renameSACode As String = Me.SetRenameSACode(staffInfo)

    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList IN:dlrcd={0}, saCode={1}" _
    '                                  , staffInfo.DlrCD _
    '                                  , renameSACode))
    '        Try
    '            ' 2012/07/06 西岡 事前準備対応 START
    '            'dt = bl.GetNoDeliveryROList(staffInfo.DlrCD, renameSACode, "1")
    '            dt = bl.GetNoDeliveryROList(staffInfo.DlrCD, renameSACode, "0")
    '            ' 2012/07/06 西岡 事前準備対応 END
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try

    '        ' IF戻り値をログ出力
    '        Me.OutPutIFLog(dt, "IC3801003BusinessLogic.GetNoDeliveryROList")

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList OUT:Count = {0}" _
    '                                  , dt.Rows.Count))

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' R/O基本情報参照
    '    ''' </summary>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <param name="orderNo">R/O番号</param>
    '    ''' <returns>R/O基本情報データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetIFROBaseInformationList(ByVal staffInfo As StaffContext, ByVal orderNo As String) As IC3801001OrderCommDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bl As IC3801001BusinessLogic = New IC3801001BusinessLogic
    '        Dim dt As IC3801001OrderCommDataTable

    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList IN:dealercode={0}, orderNo={1}" _
    '                                  , staffInfo.DlrCD _
    '                                  , orderNo))
    '        Try
    '            ' R/O基本情報参照
    '            dt = bl.GetROBaseInfoList(staffInfo.DlrCD, orderNo)
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try


    '        ' IF戻り値をログ出力
    '        Me.OutPutIFLog(dt, "IC3801001BusinessLogic.GetROBaseInfoList")

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList OUT:Count = {0}" _
    '                                  , dt.Rows.Count))

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt

    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 追加承認待ち情報取得
    '    ''' </summary>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <returns>追加承認待ち情報データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function GetIFApprovalConfirmAddList(ByVal staffInfo As StaffContext) As IC3801002DataSet.ConfirmAddListDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bl As IC3801002BusinessLogic = New IC3801002BusinessLogic
    '        Dim dt As IC3801002DataSet.ConfirmAddListDataTable

    '        ' IF用にSAコードの調整-「@」より前のSAコード取得
    '        Dim renameSACode As String = Me.SetRenameSACode(staffInfo)

    '        ' 2012/04/05 KN 西田【SERVICE_1】企画_プレユーザーテスト課題 No.31 追加作業承認一覧APIの引数対応 START
    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList IN:dealerCode={0}, branchCode={1}, saCode={2}" _
    '                                  , staffInfo.DlrCD _
    '                                  , String.Empty _
    '                                  , renameSACode))
    '        Try
    '            ' 追加承認待ち情報取得
    '            dt = bl.GetConfirmAddList(staffInfo.DlrCD, String.Empty, renameSACode)
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try

    '        ''IF用ログ
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                          , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList IN:dealerCode={0}, branchCode={1}, saCode={2}" _
    '        '                          , staffInfo.DlrCD _
    '        '                          , staffInfo.BrnCD _
    '        '                          , renameSACode))

    '        '' 追加承認待ち情報取得
    '        'dt = bl.GetConfirmAddList(staffInfo.DlrCD, staffInfo.BrnCD, renameSACode)
    '        ' 2012/04/05 KN 西田【SERVICE_1】企画_プレユーザーテスト課題 No.31 追加作業承認一覧APIの引数対応 END

    '        ' IF戻り値をログ出力
    '        Me.OutPutIFLog(dt, "IC3801002BusinessLogic.GetConfirmAddList")

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList OUT:Count = {0}" _
    '                                  , dt.Rows.Count))

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' SAコードの調整-「@」より前のSAコード取得
    '    ''' </summary>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <returns>「@」より前のSAコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetRenameSACode(ByVal staffInfo As StaffContext) As String

    '        ' IF用にSAコードの調整-「@」より前の文字列取得
    '        Dim splitString() As String
    '        Dim renameSACode As String = staffInfo.Account
    '        splitString = renameSACode.Split(CType("@", Char))
    '        renameSACode = splitString(0)

    '        '処理結果返却
    '        Return renameSACode
    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' SAコードの調整-「@」より前のSAコード取得
    '    ''' </summary>
    '    ''' <param name="staffInfo">スタッフ情報</param>
    '    ''' <returns>「@」より前のSAコード</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetRenameSACode(ByVal staffInfo As String) As String

    '        ' IF用にSAコードの調整-「@」より前の文字列取得
    '        Dim splitString() As String
    '        Dim renameSACode As String
    '        splitString = staffInfo.Split(CType("@", Char))
    '        renameSACode = splitString(0)

    '        '処理結果返却
    '        Return renameSACode

    '    End Function

    '    ' 2012/06/05 日比野 事前準備対応　START
    '    ''' <summary>
    '    ''' 整備受注№作成
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="registerNo">車両登録No.</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <param name="modelCode">モデルコード</param>
    '    ''' <param name="customerName">顧客名</param>
    '    ''' <param name="telNo">電話番号</param>
    '    ''' <param name="mobileNo">携帯番号</param>
    '    ''' <param name="reserveId">予約ID</param>
    '    ''' <param name="branchCode">店舗コード</param>
    '    ''' <param name="saCode">SAコード</param>
    '    ''' <param name="wash">洗車フラグ</param>
    '    ''' <param name="visitSeq">来店実績連番</param>
    '    ''' <param name="staffInfo">ログインスタッフ情報</param>
    '    ''' <param name="advanceCheck">事前準備フラグ</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <histiry>
    '    ''' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力
    '    ''' </histiry>
    '    Public Function GetIFCreateOrderNo(ByVal dealerCode As String, _
    '                                       ByVal registerNo As String, _
    '                                       ByVal vin As String, _
    '                                       ByVal modelCode As String, _
    '                                       ByVal customerName As String, _
    '                                       ByVal telNo As String, _
    '                                       ByVal mobileNo As String, _
    '                                       ByVal reserveId As Long, _
    '                                       ByVal branchCode As String, _
    '                                       ByVal saCode As String, _
    '                                       ByVal wash As String, _
    '                                       ByVal visitSeq As Long, _
    '                                       ByVal staffInfo As StaffContext, _
    '                                       ByVal stockTime As Date, _
    '                                       ByVal advanceCheck As String) As String()

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} {2} IN:dealerCode = {3}, registerNo = {4}, vin = {5}, modelCode = {6}, customerName = {7}, " + _
    '                "telNo = {8}, mobileNo = {9}, reserveId ={10}, branchCode = {11}, saCode = {12}, wash = {13}, " + _
    '                "visitSeq = {14}, displayAreaAdvanceFlag = {15}" _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , LOG_START _
    '              , dealerCode, registerNo, vin, modelCode, customerName, telNo, mobileNo _
    '              , reserveId, branchCode, saCode, wash, visitSeq, advanceCheck))

    '        ' 整備受注NO 作成情報(0-整備受注NO、1-UpDate結果)
    '        Dim createOrderInformation(2) As String

    '        Dim dtIF As IC3801102CreateOrderStructDataTable      ' 外部インターフェースIF(BMTS) 整備受注NO
    '        Dim bl As IC3801102BusinessLogic = New IC3801102BusinessLogic

    '        Dim upDateCheck As Long = Long.MinValue
    '        Dim orderNo As String = String.Empty


    '        ' IF用にSAコードの調整-「@」より前のSAコード取得
    '        Dim renameSACode As String = Me.SetRenameSACode(saCode)

    '        Dim strRezId As String = String.Empty

    '        If Not reserveId = -1 Then
    '            strRezId = CType(reserveId, String)
    '        End If

    '        If String.IsNullOrEmpty(vin) Then
    '            vin = " "
    '        End If

    '        If String.IsNullOrEmpty(registerNo) Then
    '            registerNo = " "
    '        End If

    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '          , "CALL IF:IC3801102BusinessLogic.AddOrderSave IN:dealerCode={0}, registerNo={1}, vinNo={2}, model={3}, customerName={4}, customTel1={5}, customTel2={6}, rezid={7}, brncd={8}, saCode={9}, cleanflg={10}, displayAreaAdvanceFlag = {11}, stockDate = {12}" _
    '          , dealerCode, registerNo, vin, modelCode, customerName _
    '          , telNo, mobileNo, strRezId, branchCode, renameSACode, wash, advanceCheck, stockTime.ToString(CultureInfo.CurrentCulture)))

    '        Try
    '            ' 整備受注NO作成処理
    '            dtIF = bl.AddOrderSave(dealerCode, _
    '                                  registerNo, _
    '                                  vin, _
    '                                  modelCode, _
    '                                  customerName, _
    '                                  telNo, _
    '                                  mobileNo, _
    '                                  strRezId, _
    '                                  branchCode, _
    '                                  renameSACode, _
    '                                  wash, _
    '                                  stockTime, _
    '                                  advanceCheck)
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try


    '        ' IF戻り値をログ出力
    '        Me.OutPutIFLog(dtIF, "IC3801102BusinessLogic.AddOrderSave")

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "CALL IF:IC3801102BusinessLogic.AddOrderSave OUT:Count = {0}", dtIF.Rows.Count))

    '        Try
    '            ' 外部インターフェースから取得したData確認
    '            If Not IsNothing(dtIF) AndAlso dtIF.Count = 1 Then
    '                '初回予約IDで条件の絞込みを行っているため、複数件データが取得されることはありえない
    '                Dim rowIF As IC3801102CreateOrderStructRow = DirectCast(dtIF.Rows(0), IC3801102CreateOrderStructRow)

    '                ' 整備受注NO作成結果確認
    '                If rowIF.ISCREATE Then

    '                    ' 整備受注NO保持
    '                    orderNo = rowIF.CREATEORDERNO

    '                    Dim rowAddOrderSave As IC3810301inOrderSaveRow

    '                    ' 引数設定
    '                    Using dtAddOrderSave As New IC3810301inOrderSaveDataTable
    '                        rowAddOrderSave = dtAddOrderSave.NewIC3810301inOrderSaveRow()
    '                    End Using

    '                    rowAddOrderSave.DLRCD = rowIF.CREATEDEALERCODE      '販売店コード
    '                    rowAddOrderSave.STRCD = branchCode                  '店舗コード
    '                    rowAddOrderSave.ORDERNO = rowIF.CREATEORDERNO       'R/O番号
    '                    rowAddOrderSave.VISITSEQ = visitSeq                 '来店実績連番
    '                    rowAddOrderSave.SACODE = saCode                     'SAコード
    '                    rowAddOrderSave.ACCOUNT = staffInfo.Account         'ログインID
    '                    rowAddOrderSave.SYSTEM = MAINMENUID                 '画面ID
    '                    rowAddOrderSave.REZID = reserveId                   '予約ID

    '                    Logger.Info("CALL IF:IC3810301BusinessLogic.AddOrderSave")

    '                    ' 整備受注NO反映
    '                    Using blSC3810301 As IC3810301BusinessLogic = New IC3810301BusinessLogic
    '                        upDateCheck = blSC3810301.AddOrderSave(rowAddOrderSave)
    '                    End Using

    '                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                          , "CALL IF:IC3810301BusinessLogic.AddOrderSave OUT:RETURNCODE = {0}", upDateCheck))

    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 START
    '                Else
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} dealerCode={3} registerNo={4} vin={5} modelCode={6} customerName={7} " _
    '                                & "telNo={8} mobileNo={9} strRezId={10} branchCode={11} renameSACode={12} wash={13} " _
    '                                & "stockTime={14} advanceCheck={15}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , "IF:IC3801102BusinessLogic.AddOrderSave RETURN ISCREATE==FALSE" _
    '                                , dealerCode, registerNo, vin, modelCode, customerName, telNo, mobileNo _
    '                                , strRezId, branchCode, renameSACode, wash, stockTime, advanceCheck))
    '                    ' 2012/10/22 TMEJ 河原  問連暫定対応「GTMC121015030」ERRORLOG出力 END
    '                End If
    '            End If
    '        Finally
    '            dtIF.Dispose()
    '        End Try

    '        ' 反映結果確認
    '        Select Case upDateCheck
    '            Case C_RET_SUCCESS
    '                createOrderInformation(0) = orderNo         ' 整備受注NO
    '                createOrderInformation(1) = C_RET_SUCCESS.ToString(CultureInfo.CurrentCulture)
    '            Case C_RET_DBTIMEOUT
    '                createOrderInformation(1) = C_RET_DBTIMEOUT.ToString(CultureInfo.CurrentCulture)  ' タイムアウト
    '            Case C_RET_NOMATCH
    '                createOrderInformation(1) = C_RET_NOMATCH.ToString(CultureInfo.CurrentCulture)    ' その他
    '            Case C_RET_DIFFSACODE
    '                createOrderInformation(1) = C_RET_DIFFSACODE.ToString(CultureInfo.CurrentCulture) ' 担当SA外
    '            Case Else
    '                createOrderInformation(1) = C_RET_NOMATCH.ToString(CultureInfo.CurrentCulture)    ' その他
    '        End Select

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} {2} OUT:CreateOrderInformation(0) = {3}, CreateOrderInformation(1) = {4}" _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , LOG_END _
    '              , createOrderInformation(0) _
    '              , createOrderInformation(1)))

    '        '処理結果返却
    '        Return createOrderInformation
    '    End Function
    '    '' 2012/06/05 日比野 事前準備対応　END

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 顧客参照
    '    ''' </summary>
    '    ''' <param name="serviceInfo">来店実績データロウ</param>
    '    ''' <returns>顧客情報格納データセット</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Public Function GetIFCustomerInformation(ByVal serviceInfo As SC3140103VisitRow) As IC3800703SrvCustomerDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bl As IC3800703BusinessLogic = New IC3800703BusinessLogic
    '        Dim dt As IC3800703SrvCustomerDataTable = New IC3800703SrvCustomerDataTable

    '        If serviceInfo IsNot Nothing Then
    '            'IF用ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo IN:registerNo={0}, vinNo={1}, dealerCode={2}" _
    '                                      , serviceInfo.VCLREGNO _
    '                                      , serviceInfo.VIN _
    '                                      , serviceInfo.DLRCD))
    '            Try
    '                ' 顧客参照処理
    '                dt = bl.GetCustomerInfo(serviceInfo.VCLREGNO, serviceInfo.VIN, serviceInfo.DLRCD)
    '            Finally
    '                If bl IsNot Nothing Then
    '                    bl = Nothing
    '                End If
    '            End Try


    '            ' IF戻り値をログ出力
    '            Me.OutPutIFLog(dt, "IC3800703BusinessLogic.GetCustomerInfo")

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
    '                                      , dt.Rows.Count))
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function


    '    ''' <summary>
    '    ''' サービス標準LT取得
    '    ''' </summary>
    '    ''' <param name="inDealerCode">販売店コード</param>
    '    ''' <param name="inStoreCode">店舗コード</param>
    '    ''' <returns>標準LT</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetStandardLTList(ByVal inDealerCode As String, _
    '                                      ByVal inStoreCode As String) _
    '                                      As IC3810701DataSet.StandardLTListDataTable
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:inDealerCode = {3}, inStoreCode = {4}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , inDealerCode, inStoreCode))

    '        Dim bl As New IC3810701BusinessLogic
    '        Dim dt As IC3810701DataSet.StandardLTListDataTable

    '        Try
    '            dt = bl.GetStandardLTList(inDealerCode, inStoreCode)
    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            Throw
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl.Dispose()
    '                bl = Nothing
    '            End If
    '        End Try


    '        ' IF戻り値をログ出力
    '        Me.OutPutIFLog(dt, "IC3800703BusinessLogic.GetCustomerInfo")

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
    '                                  , dt.Rows.Count))

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        Return dt
    '    End Function

    '    ''' <summary>
    '    ''' R/O基本情報参照
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="orderNo">整備受注No.</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Function GetROBaseInfoList(ByVal dealerCode As String,
    '                                      ByVal orderNo As String) As IC3801001DataSet.IC3801001OrderCommDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bl As IC3801001BusinessLogic = New IC3801001BusinessLogic
    '        Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable

    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList IN:dealercode={0}, orderNo={1}" _
    '                                , dealerCode _
    '                                , orderNo))
    '        Try
    '            ' 顧客参照処理
    '            dt = bl.GetROBaseInfoList(dealerCode, orderNo)
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList OUT:Count = {0}" _
    '                                    , dt.Rows.Count))

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))

    '        '処理結果返却
    '        Return dt
    '    End Function

    '#End Region

    '#Region " その他処理"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 時間チェック
    '    ''' </summary>
    '    ''' <param name="time">対象時間</param>
    '    ''' <returns>True:正常値 False:エラー値</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function IsDateTimeNull(ByVal time As DateTime) As Boolean

    '        ' 日付チェック
    '        If time.Equals(DateTime.MinValue) Then
    '            Return True
    '        End If

    '        Return False

    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' データ置換
    '    ''' </summary>
    '    ''' <param name="valBefore">データ元</param>
    '    ''' <param name="valAfter">データ先</param>
    '    ''' <returns>置換データ</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetReplaceString(ByVal valBefore As String, ByVal valAfter As String) As String

    '        ' データ元存在チェック
    '        '2012/03/13 KN 森下 半角スペースデータの対応 START
    '        'If Not String.IsNullOrEmpty(valBefore) Then
    '        If Not String.IsNullOrEmpty(valBefore.Trim()) Then
    '            '2012/03/13 KN 森下 半角スペースデータの対応 END
    '            ' データ元あり
    '            Return valBefore
    '        End If

    '        ' データ先空白チェック
    '        '2012/03/13 KN 森下 半角スペースデータの対応 START
    '        'If String.IsNullOrEmpty(valAfter) Then
    '        If String.IsNullOrEmpty(valAfter.Trim()) Then
    '            '2012/03/13 KN 森下 半角スペースデータの対応 END
    '            ' データ先なし
    '            Return valBefore
    '        End If

    '        ' データ先で置換
    '        Return valAfter

    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' データ置換
    '    ''' </summary>
    '    ''' <param name="valBefore">データ元</param>
    '    ''' <param name="valAfter">データ先</param>
    '    ''' <returns>置換データ</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetReplaceLong(ByVal valBefore As Long, ByVal valAfter As Long) As Long

    '        ' データ元存在チェック
    '        If valBefore > 0 Then
    '            ' データ元あり
    '            Return valBefore
    '        End If

    '        ' データ先空白チェック
    '        If valAfter = 0 Then
    '            ' データ先なし
    '            Return valBefore
    '        End If

    '        ' データ先で置換
    '        Return valAfter

    '    End Function

    '    ''' <summary>
    '    ''' 文字列をDate型に変換
    '    ''' </summary>
    '    ''' <param name="str"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function StringParseDate(ByVal str As String) As DateTime
    '        Dim rtnDate As DateTime

    '        If String.IsNullOrEmpty(str) Then
    '            rtnDate = Nothing
    '        Else
    '            Try
    '                rtnDate = DateTime.Parse(str, CultureInfo.InvariantCulture())
    '            Catch ex As FormatException
    '                rtnDate = Nothing
    '            End Try
    '        End If


    '        Return rtnDate
    '    End Function

    '#End Region

    '#Region " ストール設定情報取得"
    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) START
    '    ' '''-------------------------------------------------------
    '    ' ''' <summary>
    '    ' ''' ストール設定情報取得
    '    ' ''' </summary>
    '    ' ''' <returns>ストール設定データセット</returns>
    '    ' ''' <remarks></remarks>
    '    ' '''-------------------------------------------------------
    '    'Public Function GetStallControl() As SC3140103StallCtl2DataTable

    '    '    '開始ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '                            , "{0}.{1} {2}" _
    '    '                            , Me.GetType.ToString _
    '    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '                            , LOG_START))

    '    '    Dim dt As SC3140103StallCtl2DataTable
    '    '    Dim staffInfo As StaffContext = StaffContext.Current

    '    '    Using da As New SC3140103DataTableAdapter
    '    '        '検索処理
    '    '        dt = da.GetStallControl(staffInfo.DlrCD, staffInfo.BrnCD)
    '    '    End Using

    '    '    '終了ログ
    '    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '                            , "{0}.{1} {2}" _
    '    '                            , Me.GetType.ToString _
    '    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '                            , LOG_END))

    '    '    '処理結果返却
    '    '    Return dt
    '    'End Function
    '    ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更) END
    '#End Region

    '    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) START
    '#Region "追加作業関連遷移先取得"
    '    ''' <summary>
    '    ''' 追加作業関連遷移先取得
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <returns>追加作業関連画面列挙体</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetAddWorkRedirect(ByVal orderNo As String) As AddWorkRedirect

    '        Dim rtnValue As AddWorkRedirect = AddWorkRedirect.Invalid

    '        'R/Oステータス取得
    '        Dim roStatus As String = Me.GetROStatus(orderNo)

    '        If roStatus.Equals(C_RO_STATUS_WORKING) OrElse roStatus.Equals(C_RO_STATUS_INSP_OK) Then
    '            'R/Oステータスが「2(整備中)」又は、「7(検査完了)」の場合

    '            Dim staffInfo As StaffContext = StaffContext.Current

    '            '追加作業情報取得
    '            Dim dt As DataTable = Me.GetAddRepairStatusInfo(staffInfo.DlrCD, orderNo)

    '            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
    '                '追加作業なしの場合
    '                '追加作業入力(新規)へ画面遷移
    '                rtnValue = AddWorkRedirect.SC3170201New
    '            Else
    '                '追加作業ありの場合
    '                '取得情報の最終行を取得する
    '                Dim dr As IC3800804AddRepairStatusDataTableRow = DirectCast(dt.Rows(dt.Rows.Count - 1), IC3800804AddRepairStatusDataTableRow)

    '                'TODO 起票者の判定方法が決まり次第、再度実装すること！！
    '                If dr.DRAWER.Equals(DRAWER_SA) Then
    '                    '起票者が「SA」の場合
    '                    Select Case dr.STATUS
    '                        Case ADD_WORK_STATUS_SA_ESTIMATE_WAIT
    '                            '4(SA見積確定待ち)の場合
    '                            '追加作業入力(編集)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170201Edit
    '                        Case ADD_WORK_STATUS_CUSTOMER_APPROVAL_WAIT
    '                            '5(顧客承認待ち)の場合
    '                            '追加作業プレビューへ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170302
    '                        Case ADD_WORK_STATUS_CT_WORK_INSTRUCTION,
    '                             ADD_WORK_STATUS_TC_WORK_WAIT,
    '                             ADD_WORK_STATUS_WORK,
    '                             ADD_WORK_STATUS_INSPECTION_COMPLETION
    '                            '6(CT着工指示/PS部品出荷待ち),7(TC作業開始待ち),8(整備中),9(完成検査完了)の場合
    '                            '追加作業入力(新規)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170201New
    '                        Case Else
    '                            '上記以外は、追加作業一覧へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170101
    '                    End Select
    '                ElseIf dr.DRAWER.Equals(DRAWER_TC) Then
    '                    '起票者が「TC」の場合
    '                    Select Case dr.STATUS
    '                        Case ADD_WORK_STATUS_TC_TACTING
    '                            '1(TC起票中)の場合
    '                            '追加作業入力(代行)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170203Acting
    '                        Case ADD_WORK_STATUS_CT_APPROVAL,
    '                             ADD_WORK_STATUS_PS_PARTS_ESTIMATE_WAIT
    '                            '2(CT承認待ち), 3(PS部品見積待ち)の場合
    '                            '追加作業入力(参照)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170203Preview
    '                        Case ADD_WORK_STATUS_SA_ESTIMATE_WAIT
    '                            '4(SA見積確定待ち)の場合
    '                            '追加作業入力(編集)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170201Edit
    '                        Case ADD_WORK_STATUS_CUSTOMER_APPROVAL_WAIT
    '                            '5(顧客承認待ち)の場合
    '                            '追加作業プレビューへ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170302
    '                        Case Else
    '                            '6(CT着工指示/PS部品出荷待ち),7(TC作業開始待ち),8(整備中),9(完成検査完了)の場合
    '                            '追加作業入力(新規)へ画面遷移
    '                            rtnValue = AddWorkRedirect.SC3170201New
    '                    End Select
    '                Else
    '                    '起票者不明のため、追加作業一覧へ遷移
    '                    rtnValue = AddWorkRedirect.SC3170101
    '                End If
    '            End If
    '        Else
    '            ''R/Oステータスが「2(整備中)」又は、「7(検査完了)」以外のため、追加作業一覧へ遷移
    '            rtnValue = AddWorkRedirect.SC3170101
    '        End If

    '        Return rtnValue

    '    End Function
    '#End Region

    '#Region "R/Oステータス取得"
    '    ''' <summary>
    '    ''' R/Oステータス取得
    '    ''' </summary>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <returns>R/Oステータス</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetROStatus(ByVal orderNo As String) As String

    '        Dim resultOrderStatus = String.Empty

    '        'スタッフ情報取得
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        'R/O基本情報取得
    '        Dim dt As IC3801001OrderCommDataTable = Me.GetIFROBaseInformationList(staffInfo, orderNo)

    '        If (dt IsNot Nothing AndAlso dt.Rows.Count > 0) Then
    '            'R/Oステータス取得
    '            resultOrderStatus = dt(0).OrderStatus
    '        End If

    '        Return resultOrderStatus

    '    End Function
    '#End Region

    '#Region "追加作業ステータス情報取得"
    '    ''' <summary>
    '    ''' 追加作業ステータス情報取得
    '    ''' </summary>
    '    ''' <param name="dlrCd">販売店コード</param>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <returns>追加作業ステータス情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetAddRepairStatusInfo(ByVal dlrCd As String, ByVal orderNo As String) As DataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START))

    '        Dim bizLogic As New IC3800804BusinessLogic
    '        Dim dt As DataTable

    '        'IF用ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                  , "CALL IF:IC3800804BusinessLogic.GetAddRepairStatusList IN:dealercode={0}, orderNo={1}" _
    '                                  , dlrCd _
    '                                  , orderNo))

    '        '追加作業ステータス情報取得
    '        dt = bizLogic.GetAddRepairStatusList(dlrCd, orderNo)

    '        OutPutIFLog(dt, "IC3800804BusinessLogic.GetAddRepairStatusList")

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END))
    '        Return dt

    '    End Function

    '#End Region

    '#Region "追加作業最大枝番取得"
    '    ''' <summary>
    '    ''' 追加作業最大枝番取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="orderNo">整備受注NO</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Function GetAddRepairMaxSeq(ByVal dealerCode As String, ByVal orderNo As String) As Integer

    '        Dim srvaddSeq As Integer = 0

    '        '追加作業情報取得
    '        Dim dt As DataTable = Me.GetAddRepairStatusInfo(dealerCode, orderNo)

    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
    '            '取得情報の最終行を取得する
    '            Dim dr As IC3800804AddRepairStatusDataTableRow = DirectCast(dt.Rows(dt.Rows.Count - 1), IC3800804AddRepairStatusDataTableRow)
    '            srvaddSeq = CType(dr.SRVADDSEQ, Integer)
    '        End If

    '        Return srvaddSeq

    '    End Function
    '#End Region
    '    '2012/03/21 上田 仕様変更対応(追加作業関連の画面遷移先変更) END

    '#Region "ログ出力(IF戻り値用)"
    '    ''' <summary>
    '    ''' ログ出力(IF戻り値用)
    '    ''' </summary>
    '    ''' <param name="dt">戻り値(DataTable)</param>
    '    ''' <param name="ifName">使用IF名</param>
    '    ''' <remarks></remarks>
    '    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

    '        If dt Is Nothing Then
    '            Return
    '        End If

    '        Logger.Info(ifName + " Result START " + " OutPutCount: " + CType(dt.Rows.Count, String))

    '        Dim log As New Text.StringBuilder

    '        For j = 0 To dt.Rows.Count - 1

    '            log = New Text.StringBuilder()
    '            Dim dr As DataRow = dt.Rows(j)

    '            log.Append("RowNum: " + CType(j + 1, String) + " -- ")

    '            For i = 0 To dt.Columns.Count - 1
    '                log.Append(dt.Columns(i).Caption)
    '                If IsDBNull(dr(i)) Then
    '                    log.Append(" IS NULL")
    '                Else
    '                    log.Append(" = ")
    '                    log.Append(dr(i).ToString)
    '                End If

    '                If i <= dt.Columns.Count - 2 Then
    '                    log.Append(", ")
    '                End If
    '            Next

    '            Logger.Info(log.ToString)
    '        Next

    '        Logger.Info(ifName + " Result END ")

    '    End Sub
    '#End Region

    '    '2012/05/31 西岡 STEP2事前準備追加対応 START
    '#Region "事前準備件数取得"

    '    ''' <summary>
    '    ''' 事前準備件数取得
    '    ''' </summary>
    '    ''' <returns>List(0:todayCount, 1:nextCount)</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetAdvancePreparationsCount() As List(Of Long)
    '        'Public Sub GetAdvancePreparationsCount(ByRef todayCount As Long, ByRef nextCount As Long)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        '日付管理機能（現在日付取得）
    '        Dim nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
    '        Dim nowDateString As String = DateTimeFunc.FormatDate(9, nowDateTime)

    '        '翌稼働日取得
    '        Dim nextDateString As String
    '        Using commonClass As New SMBCommonClassBusinessLogic
    '            commonClass.InitCommon(staffInfo.DlrCD, staffInfo.BrnCD, nowDateTime)
    '            Dim nextDate As Date = commonClass.GetWorkingDays(nowDateTime, 1)
    '            nextDateString = DateTimeFunc.FormatDate(9, nextDate)
    '        End Using


    '        '事前準備情報取得
    '        Dim dt As SC3140103AdvancePreparationsDataTable
    '        Using da As New SC3140103DataTableAdapter
    '            dt = da.GetAdvancePreparationsChipData(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString, staffInfo.Account)
    '        End Using

    '        Dim orderList As New List(Of String)
    '        '事前準備情報中の整備受注NoがNULL以外のものをリストに詰める
    '        For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '            If Not String.IsNullOrEmpty(row.ORDERNO) Then
    '                orderList.Add(row.ORDERNO)
    '            End If
    '        Next

    '        'R/O事前準備状態一覧取得
    '        Dim saOrder As IC3801012DataSet.REZROStatusListDataTable
    '        Dim bl02 As New IC3801012BusinessLogic
    '        Dim saOrderRow As IC3801012DataSet.REZROStatusListRow
    '        '整備受注Noのリスト有無で処理を分岐
    '        If orderList.Count > 0 Then
    '            saOrder = bl02.GetREZROStatusList(staffInfo.DlrCD, staffInfo.BrnCD, orderList)
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                'SA事前準備フラグは初期値に未完了("1")を設定
    '                row.SASTATUSFLG = PreparationMiddle
    '                '整備受注NoがNull以外だったものは、取得した一覧と比較する
    '                If Not String.IsNullOrEmpty(row.ORDERNO) Then
    '                    Dim aryRow As DataRow() = saOrder.Select(String.Format(CultureInfo.CurrentCulture, _
    '                     "ORDERNO = '{0}'", row.ORDERNO))

    '                    ' 取得一覧の整備受注Noと一致したものがあった場合SA事前準備フラグを設定する
    '                    If Not (aryRow Is Nothing OrElse aryRow.Length = 0) Then
    '                        saOrderRow = DirectCast(aryRow(0), IC3801012DataSet.REZROStatusListRow)
    '                        row.SASTATUSFLG = saOrderRow.STATUS
    '                    Else
    '                        row.SASTATUSFLG = PreparationEnd
    '                    End If
    '                End If
    '            Next

    '        Else
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                'SA事前準備フラグは初期値に未完了("1")を設定
    '                row.SASTATUSFLG = PreparationMiddle
    '            Next
    '        End If

    '        Dim todayCount As Long = 0
    '        Dim nextCount As Long = 0

    '        '当日、および翌日の件数を取得
    '        For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '            'SA事前準備フラグが未完了のもの（整備受注Noがないものも含まれる）を件数にカウントする
    '            If PreparationMiddle.Equals(row.SASTATUSFLG) Then
    '                If (row.TODAYFLG.Equals("1")) Then
    '                    '当日件数加算
    '                    todayCount += 1
    '                Else
    '                    '翌日件数加算
    '                    nextCount += 1
    '                End If
    '            End If
    '        Next

    '        Dim resultList As New List(Of Long)

    '        resultList.Add(todayCount)
    '        resultList.Add(nextCount)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2} Result:todayCount={3}, nextCount={4}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END _
    '           , todayCount _
    '           , nextCount))

    '        Return resultList

    '    End Function

    '#End Region

    '#Region "事前準備チップ予約情報取得"

    '    ''' <summary>
    '    ''' 事前準備チップ予約情報の取得
    '    ''' </summary>
    '    ''' <param name="SACODE">SAコード</param> 
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    '    ''' </history>
    '    Public Function GetReserveChipInfo(ByVal saCode As String) As SC3140103AdvancePreparationsDataTable
    '        '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '        'Public Function GetReserveChipInfo() As SC3140103AdvancePreparationsDataTable
    '        '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

    '        'Public Sub GetReserveChipInfo(ByRef todayCount As Long,
    '        '                              ByRef nextCount As Long,
    '        '                              ByRef dt As SC3140103AdvancePreparationsDataTable)

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        Dim dt As SC3140103AdvancePreparationsDataTable = Nothing

    '        '日付管理機能（現在日付取得）
    '        Dim nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
    '        Dim nowDateString As String = DateTimeFunc.FormatDate(9, nowDateTime)

    '        '翌稼働日取得
    '        Dim nextDateString As String
    '        Using commonClass As New SMBCommonClassBusinessLogic
    '            commonClass.InitCommon(staffInfo.DlrCD, staffInfo.BrnCD, nowDateTime)
    '            Dim nextDate As Date = commonClass.GetWorkingDays(nowDateTime, 1)
    '            nextDateString = DateTimeFunc.FormatDate(9, nextDate)
    '        End Using

    '        '事前準備情報取得
    '        Using da As New SC3140103DataTableAdapter
    '            '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
    '            'dt = da.GetAdvancePreparationsChipData(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString, staffInfo.Account)
    '            If String.IsNullOrEmpty(saCode.Trim) Then
    '                dt = da.GetAdvancePreparationsChipDataNoSA(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString)
    '            Else
    '                dt = da.GetAdvancePreparationsChipData(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString, saCode)
    '            End If
    '            '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
    '        End Using

    '        Dim orderList As New List(Of String)
    '        '事前準備情報中の整備受注NoがNULL以外のものをリストに詰める
    '        For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '            If Not String.IsNullOrEmpty(row.ORDERNO) Then
    '                orderList.Add(row.ORDERNO)
    '            End If
    '        Next

    '        'R/O事前準備状態一覧取得
    '        '整備受注Noのリスト有無で処理を分岐
    '        If orderList.Count > 0 Then
    '            Dim saOrder As IC3801012DataSet.REZROStatusListDataTable
    '            Dim bl02 As New IC3801012BusinessLogic
    '            saOrder = bl02.GetREZROStatusList(staffInfo.DlrCD, staffInfo.BrnCD, orderList)

    '            Dim saOrderRow As IC3801012DataSet.REZROStatusListRow
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                'SA事前準備フラグは初期値に未完了("1")を設定
    '                row.SASTATUSFLG = PreparationMiddle
    '                '整備受注NoがNull以外だったものは、取得した一覧と比較する
    '                If Not String.IsNullOrEmpty(row.ORDERNO) Then
    '                    Dim aryRow As DataRow() = saOrder.Select(String.Format(CultureInfo.CurrentCulture, _
    '                     "ORDERNO = '{0}'", row.ORDERNO))

    '                    ' 取得一覧の整備受注Noと一致したものがあった場合SA事前準備フラグを設定する
    '                    If Not (aryRow Is Nothing OrElse aryRow.Length = 0) Then
    '                        saOrderRow = DirectCast(aryRow(0), IC3801012DataSet.REZROStatusListRow)
    '                        row.SASTATUSFLG = saOrderRow.STATUS
    '                    Else
    '                        row.SASTATUSFLG = PreparationEnd
    '                    End If
    '                End If
    '            Next

    '        Else
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                'SA事前準備フラグは初期値に未完了("1")を設定
    '                row.SASTATUSFLG = PreparationMiddle
    '            Next
    '        End If

    '        'JDP・SSC情報取得
    '        dt = Me.GetMarkInfo(staffInfo, dt)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END))

    '        Return dt
    '    End Function

    '    ''' <summary>
    '    ''' 事前準備チップのJDP・SSC情報取得
    '    ''' </summary>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function GetMarkInfo(staffInfo As StaffContext, dt As SC3140103AdvancePreparationsDataTable) As SC3140103AdvancePreparationsDataTable

    '        'JDP・SSC情報取得
    '        Dim dtCustomerInfoDataTable As IC3800703SrvCustomerDataTable = Nothing
    '        Try
    '            Dim blCustomerInfoBusinessLogic As New IC3800703BusinessLogic
    '            For Each row As SC3140103AdvancePreparationsRow In dt.Rows
    '                ' 整備受注NoがNullのものの中で、VINまたは車両登録NoがNull以外のものを対象として、顧客情報を取得する	
    '                If PreparationMiddle.Equals(row.SASTATUSFLG) Then
    '                    If (Not String.IsNullOrEmpty(row.VIN.Trim())) Or (Not String.IsNullOrEmpty(row.VCLREGNO.Trim())) Then
    '                        dtCustomerInfoDataTable = blCustomerInfoBusinessLogic.GetCustomerInfo(row.VCLREGNO.Trim(), row.VIN.Trim(), staffInfo.DlrCD)

    '                        Dim rowCustomerInfo As IC3800703SrvCustomerFRow
    '                        '顧客情報が取得できた場合、事前準備情報に設定する
    '                        If dtCustomerInfoDataTable IsNot Nothing AndAlso dtCustomerInfoDataTable.Rows.Count > 0 Then
    '                            rowCustomerInfo = DirectCast(dtCustomerInfoDataTable.Rows(0), IC3800703SrvCustomerFRow)

    '                            If Not IsDBNull(rowCustomerInfo.Item("JDPFLAG")) Then
    '                                row.JDP_MARK = rowCustomerInfo.JDPFLAG     'JDP調査対象客マーク
    '                            End If
    '                            If Not IsDBNull(rowCustomerInfo.Item("SSCFLAG")) Then
    '                                row.SSC_MARK = rowCustomerInfo.SSCFLAG     '技術情報マーク
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        Finally
    '            If Not IsNothing(dtCustomerInfoDataTable) Then
    '                dtCustomerInfoDataTable.Dispose()
    '            End If
    '        End Try

    '        Return dt

    '    End Function

    '    '' 2012/06/07 Hibino 事前準備対応 START
    '    ''' <summary>
    '    ''' 事前準備チップ予約情報取得
    '    ''' </summary>
    '    ''' <param name="reserveId">対象の予約ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Function GetAdvancePreparationsReserveInfo(ByVal dealerCode As String, _
    '      ByVal branchCode As String, _
    '      ByVal reserveId As Long) As SC3140103AdvancePreparationsReserveInfoDataTable
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))


    '        Dim dt As SC3140103AdvancePreparationsReserveInfoDataTable

    '        Using adapter As New SC3140103DataTableAdapter
    '            dt = adapter.GetAdvancePreparationsReserveInfoData(dealerCode, branchCode, reserveId)
    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END))

    '        Return dt

    '    End Function

    '    ''' <summary>
    '    ''' 事前準備チップサービス来店管理情報取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="branchCode">店舗コード</param>
    '    ''' <param name="reserveId">予約ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Function GetAdvancePreparationsVisitManager(ByVal dealerCode As String, _
    '          ByVal branchCode As String, _
    '          ByVal reserveId As Long) As SC3140103AdvancePreparationsServiceVisitManagementDataTable
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        Dim dt As SC3140103AdvancePreparationsServiceVisitManagementDataTable

    '        Using adapter As New SC3140103DataTableAdapter
    '            dt = adapter.GetAdvancePreparationsServiceVisitManagementData(dealerCode, branchCode, reserveId)
    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END))

    '        Return dt

    '    End Function
    '    '' 2012/06/07 Hibino 事前準備対応 END
    '#End Region
    '    '2012/05/31 西岡 STEP2事前準備追加対応 END

    '    '2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
    '#Region "自社客検索情報取得"

    '    ''' <summary>
    '    ''' 自社客検索結果取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="registrationNo">車両登録No.</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <param name="customerName">氏名</param>
    '    ''' <param name="phone">電話番号</param>
    '    ''' <param name="startRow">現在の表示開始行</param>
    '    ''' <param name="endRow">現在の表示終了行</param>
    '    ''' <param name="selectLoad">指定読み込み値</param>
    '    ''' <returns>自社客検索結果</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetCustomerList(ByVal dealerCode As String, _
    '                                    ByVal storeCode As String, _
    '                                    ByVal registrationNo As String, _
    '                                    ByVal vin As String, _
    '                                    ByVal customerName As String, _
    '                                    ByVal phone As String, _
    '                                    ByVal startRow As Long, _
    '                                    ByVal endRow As Long, _
    '                                    ByVal selectLoad As Long) As SC3140103SearchResult

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, vclRegNo = {5}, vin = {6}, customerName = {7}, " & _
    '                                 "phone = {8}, startRow = {9}, endRow = {10}, selectLoad = {11}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , dealerCode _
    '                                 , storeCode _
    '                                 , registrationNo _
    '                                 , vin _
    '                                 , customerName _
    '                                 , phone _
    '                                 , startRow _
    '                                 , endRow _
    '                                 , selectLoad))

    '        Dim systemEnv As New SystemEnvSetting

    '        ' 検索標準読み込み数取得
    '        Dim loadCount As Long = _
    '            CType(systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT).PARAMVALUE, Long)

    '        ' 検索最大表示数取得
    '        Dim maxDispCount As Long = _
    '            CType(systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT).PARAMVALUE, Long)

    '        ' 顧客数取得
    '        Dim customerCount As Long = 0
    '        Dim blCustomerCount As New IC3800706BusinessLogic
    '        customerCount = blCustomerCount.GetCustomerCount(dealerCode, _
    '                                                         storeCode, _
    '                                                         registrationNo, _
    '                                                         vin, _
    '                                                         customerName, _
    '                                                         phone)

    '        Dim searchStartRow As Long = 0
    '        Dim searchEndRow As Long = 0
    '        ' 検索処理呼び出し方法による分岐
    '        ' 検索アイコンタップ時
    '        If selectLoad = 0 Then
    '            searchStartRow = 1
    '            If customerCount < loadCount Then
    '                searchEndRow = customerCount
    '            Else
    '                searchEndRow = loadCount
    '            End If
    '            ' 次のN件表示タップ時
    '        ElseIf 1 <= selectLoad Then
    '            ' 終了行の設定
    '            Dim setEndMax As Long = endRow + loadCount
    '            If customerCount < setEndMax Then
    '                searchEndRow = customerCount
    '            Else
    '                searchEndRow = setEndMax
    '            End If
    '            ' 開始行の設定
    '            Dim setStartMax As Long = searchEndRow - startRow + 1
    '            If setStartMax <= maxDispCount Then
    '                searchStartRow = startRow
    '            Else
    '                searchStartRow = searchEndRow - maxDispCount + 1

    '                If searchStartRow <= 0 Then
    '                    searchStartRow = 1
    '                End If
    '            End If
    '            ' 前のN件表示タップ時
    '        Else
    '            ' 開始行の設定
    '            Dim setStartMin As Long = startRow - loadCount
    '            If setStartMin <= 0 Then
    '                searchStartRow = 1
    '            Else
    '                searchStartRow = setStartMin
    '            End If
    '            ' 終了行の設定
    '            Dim setEndMin As Long = endRow - searchStartRow + 1
    '            If setEndMin < maxDispCount Then
    '                searchEndRow = endRow
    '            Else
    '                searchEndRow = searchStartRow + maxDispCount - 1
    '            End If
    '            If customerCount < searchEndRow Then
    '                searchEndRow = customerCount
    '            End If
    '        End If

    '        Dim dtCustomerSearch As IC3800707DataSet.CustomerListDataTable
    '        Dim result As New SC3140103SearchResult
    '        ' 顧客検索処理
    '        Dim blCustomerSearch As New IC3800707BusinessLogic
    '        dtCustomerSearch = blCustomerSearch.GetCustomerList(dealerCode, _
    '                                                            storeCode, _
    '                                                            registrationNo, _
    '                                                            vin, _
    '                                                            customerName, _
    '                                                            phone, _
    '                                                            searchStartRow, _
    '                                                            searchEndRow)
    '        result.SearchResult = 0

    '        Dim dt As SC3140103VisitSearchResultDataTable = New SC3140103VisitSearchResultDataTable
    '        ' 顧客写真情報取得
    '        If dtCustomerSearch IsNot Nothing AndAlso 0 < dtCustomerSearch.Count Then
    '            dt = GetPhotoData(dealerCode, storeCode, dtCustomerSearch)
    '        End If
    '        ' 返却値の形成
    '        result.DataTable = dt
    '        result.ResultStartRow = searchStartRow
    '        result.ResultEndRow = searchEndRow
    '        result.ResultCustomerCount = customerCount
    '        result.StandardCount = loadCount

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2} OUT:COUNT = {3}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END _
    '           , result.DataTable.Rows.Count))

    '        Return result
    '    End Function

    '    ''' <summary>
    '    ''' 顧客写真情報取得および、返却データテーブルの作成
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="dtCustomerSearch">自社客検索結果</param>
    '    ''' <returns>顧客写真情報取得結果</returns>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </History>
    '    Private Function GetPhotoData(ByVal dealerCode As String, _
    '                                  ByVal storeCode As String, _
    '                                  ByVal dtCustomerSearch As IC3800707DataSet.CustomerListDataTable) _
    '                                            As SC3140103VisitSearchResultDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        Dim dt As SC3140103VisitSearchResultDataTable = New SC3140103VisitSearchResultDataTable
    '        Dim dtRow As SC3140103VisitSearchResultRow
    '        Dim dtPhotoInfo As SC3140103VisitPhotoInfoDataTable
    '        Dim drPhotoInfo As SC3140103VisitPhotoInfoRow
    '        Dim systemEnv As New SystemEnvSetting
    '        Dim systemEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
    '            systemEnv.GetSystemEnvSetting(ConstFacePictureUploadUrl)
    '        Dim imageUrl As String = systemEnvRow.PARAMVALUE.Trim()

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        Dim commonClass As New SMBCommonClassBusinessLogic

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        Using da As New SC3140103DataTableAdapter
    '            For Each row As IC3800707DataSet.CustomerListRow In dtCustomerSearch.Rows
    '                ' 返却情報をNullチェック後設定
    '                dtRow = DirectCast(dt.NewRow(), SC3140103VisitSearchResultRow)
    '                If row.IsVCLREGNONull Then
    '                    dtRow.VCLREGNO = String.Empty
    '                Else
    '                    dtRow.VCLREGNO = row.VCLREGNO.Trim()
    '                End If
    '                If row.IsVINNull Then
    '                    dtRow.VIN = String.Empty
    '                Else
    '                    dtRow.VIN = row.VIN.Trim()
    '                End If
    '                If row.IsCUSTOMERIDNull Then
    '                    dtRow.DMSID = String.Empty
    '                Else

    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


    '                    'dtRow.DMSID = row.CUSTOMERID.Trim()

    '                    '受信した基幹顧客IDに販売店コード＠を追加
    '                    dtRow.DMSID = commonClass.ReplaceBaseCustomerCode(dealerCode, row.CUSTOMERID.Trim())


    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                End If
    '                If row.IsCUSTOMERNAMENull Then
    '                    dtRow.CUSTOMERNAME = String.Empty
    '                Else
    '                    dtRow.CUSTOMERNAME = row.CUSTOMERNAME.Trim()
    '                End If
    '                If row.IsTELNONull Then
    '                    dtRow.TELNO = String.Empty
    '                Else
    '                    dtRow.TELNO = row.TELNO.Trim()
    '                End If
    '                If row.IsMOBILENull Then
    '                    dtRow.MOBILE = String.Empty
    '                Else
    '                    dtRow.MOBILE = row.MOBILE.Trim()
    '                End If
    '                If row.IsSERIESNAMENull Then
    '                    dtRow.VEHICLENAME = String.Empty
    '                Else
    '                    dtRow.VEHICLENAME = row.SERIESNAME.Trim()
    '                End If
    '                If row.IsGRADENull Then
    '                    dtRow.GRADE = String.Empty
    '                Else
    '                    dtRow.GRADE = row.GRADE.Trim()
    '                End If
    '                If row.IsMODELNull Then
    '                    dtRow.MODEL = String.Empty
    '                Else
    '                    dtRow.MODEL = row.MODEL.Trim()
    '                End If
    '                If row.IsSACODENull Then
    '                    dtRow.SACODE = String.Empty
    '                Else
    '                    dtRow.SACODE = row.SACODE.Trim()
    '                End If
    '                ' 顧客写真情報の取得処理
    '                dtPhotoInfo = da.GetCustomerPhotoData(dealerCode, storeCode, dtRow.DMSID)
    '                If dtPhotoInfo IsNot Nothing AndAlso 0 < dtPhotoInfo.Rows.Count Then
    '                    drPhotoInfo = DirectCast(dtPhotoInfo.Rows(0), SC3140103VisitPhotoInfoRow)
    '                    If Not drPhotoInfo.IsIMAGEFILE_SNull Then
    '                        dtRow.IMAGEFILE = imageUrl + drPhotoInfo.IMAGEFILE_S.Trim()
    '                    Else
    '                        dtRow.IMAGEFILE = NO_IMAGE_ICON
    '                    End If
    '                    If Not drPhotoInfo.IsORIGINALIDNull Then
    '                        dtRow.CUSTOMERCODE = drPhotoInfo.ORIGINALID.Trim()
    '                    Else
    '                        dtRow.CUSTOMERCODE = String.Empty
    '                    End If
    '                Else
    '                    dtRow.IMAGEFILE = NO_IMAGE_ICON
    '                    dtRow.CUSTOMERCODE = String.Empty
    '                End If

    '                dt.AddSC3140103VisitSearchResultRow(dtRow)
    '            Next
    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2} OUT:COUNT = {3}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END _
    '           , dt.Rows.Count))

    '        Return dt
    '    End Function

    '#End Region

    '#Region "顧客付替え前確認"

    '    ''' <summary>
    '    ''' 顧客付替え前確認
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    '    ''' <param name="registrationNumber">付替え元予約ID</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Public Function GetCustomerChangeCheck(ByVal dealerCode As String, _
    '                                           ByVal storeCode As String, _
    '                                           ByVal beforeVisitNumber As Long, _
    '                                           ByVal registrationNumber As String, _
    '                                           ByVal vin As String) As SC3140103BeforeChangeCheckResultRow

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        Dim dtResult As New SC3140103BeforeChangeCheckResultDataTable
    '        Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
    '            DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

    '        '付替え元来店管理情報取得
    '        Dim dtBeforeVisit As SC3140103ChangesServiceVisitManagementDataTable
    '        Using da As New SC3140103DataTableAdapter
    '            dtBeforeVisit = da.GetServiseVisitManagementForChangeDate(dealerCode,
    '                                                                      storeCode,
    '                                                                      beforeVisitNumber)
    '        End Using

    '        Dim rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow = _
    '            DirectCast(dtBeforeVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        ' 振当SAチェック
    '        If Not rowBeforeVisit.SACODE.Equals(staffInfo.Account) Then
    '            resultRow.CHANGECHECKRESULT = ChangeResultDifference
    '            'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)
    '            Return resultRow
    '        End If


    '        ' 付替え元来店情報ホールドチェック
    '        If Not rowBeforeVisit.IsASSIGNSTATUSNull Then
    '            If ASSIGN_STATUS_HOLD.Equals(rowBeforeVisit.ASSIGNSTATUS) Then
    '                resultRow.CHANGECHECKRESULT = ChangeResultBeforeHold
    '                'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)
    '                Return resultRow
    '            End If
    '        End If

    '        ' 付け替え元来店情報にR/O情報がある場合、R/O基本情報をチェックする
    '        Dim checkResult As Long = Me.CheckROBaseInfo(dealerCode, rowBeforeVisit)

    '        If Not ChangeResultTrue.Equals(checkResult) Then
    '            resultRow.CHANGECHECKRESULT = ChangeResultApproval
    '            'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)

    '            Return resultRow
    '        End If

    '        '日付管理機能（現在日付取得）
    '        Dim nowDateTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)
    '        Dim nowDateString As String = DateTimeFunc.FormatDate(9, nowDateTime)

    '        '付替え先予約・来店取得
    '        Dim dtAfterReserve As SC3140103SearchChangesReserveDataTable
    '        Dim dtAfterVisit As SC3140103SearchChangesVisitDataTable

    '        Using da As New SC3140103DataTableAdapter
    '            dtAfterReserve = da.SearchStallReserveInfoChangeData(dealerCode, _
    '                                                                 storeCode, _
    '                                                                 registrationNumber, _
    '                                                                 vin, _
    '                                                                 nowDateString)

    '            dtAfterVisit = da.SearchServiceVisitManagementChangeData(dealerCode, _
    '                                                                     storeCode, _
    '                                                                     registrationNumber, _
    '                                                                     vin, _
    '                                                                     nowDateString)
    '        End Using

    '        Try
    '            resultRow = Me.GetAfterReplaceInfo(dtAfterVisit, dtAfterReserve, rowBeforeVisit, beforeVisitNumber)
    '        Catch ex As ArgumentException
    '            '付替え先来店情報のSA振当てステータスがホールド中の場合
    '            resultRow.CHANGECHECKRESULT = ChangeResultAfterHold
    '            'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)
    '            Return resultRow
    '        End Try



    '        '付替え先の担当が他SAでないかのチェック
    '        Dim checkChangeConfirmationResult As Long = Me.CheckChangeConfirmation(resultRow)

    '        If Not ChangeResultTrue.Equals(checkChangeConfirmationResult) Then
    '            resultRow.CHANGECHECKRESULT = checkChangeConfirmationResult
    '            'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)
    '            Return resultRow
    '        End If

    '        If Not rowBeforeVisit.IsFREZIDNull AndAlso _
    '          0 < rowBeforeVisit.FREZID Then
    '            resultRow.BEFORERESERVENO = rowBeforeVisit.FREZID
    '        End If

    '        If String.IsNullOrEmpty(rowBeforeVisit.ORDERNO) Then
    '            resultRow.BEFOREORDERNO = rowBeforeVisit.ORDERNO
    '        End If

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        '車両IDの確認
    '        If resultRow.IsAFTERVCLIDNull OrElse resultRow.AFTERVCLID <= 0 Then

    '            '車両IDが取得できていない場合、顧客車両情報を取得する
    '            resultRow = Me.GetAfterVehicleInfo(resultRow, dealerCode, registrationNumber, vin)

    '        End If

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        resultRow.CHANGECHECKRESULT = ChangeResultTrue

    '        'dtResult.AddSC3140103BeforeChangeCheckResultRow(ResultRow)

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_END))

    '        Return resultRow

    '    End Function


    '    ''' <summary>
    '    ''' R/O基本情報をチェック
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="rowBeforeVisit">R/O基本情報</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckROBaseInfo(ByVal dealerCode As String,
    '                                     ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow) As Long

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        If Not rowBeforeVisit.IsORDERNONull Then
    '            ' R/O基本情報を取得
    '            Dim dtROBaseInfo As IC3801001OrderCommDataTable = Me.GetROBaseInfoList(dealerCode, rowBeforeVisit.ORDERNO)

    '            If dtROBaseInfo IsNot Nothing AndAlso 0 < dtROBaseInfo.Rows.Count Then
    '                Dim drROBaseInfo As IC3801001OrderCommRow = _
    '                    DirectCast(dtROBaseInfo.Rows(0), IC3801001OrderCommRow)

    '                If drROBaseInfo.IsOrderStatusNull Then
    '                    '整備受注NoがNullの場合、NG返却
    '                    Return ChangeResultApproval
    '                Else
    '                    If Not (C_RO_STATUS_RECEPTION.Equals(drROBaseInfo.OrderStatus) Or _
    '                        C_RO_STATUS_ESTI_WAIT.Equals(drROBaseInfo.OrderStatus)) Then

    '                        'R/O承認済みの場合、NG返却
    '                        Return ChangeResultApproval
    '                    End If
    '                End If
    '            Else
    '                Return ChangeResultApproval
    '            End If
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '        Return ChangeResultTrue
    '    End Function

    '    ''' <summary>
    '    ''' 付け替え先情報の取得(来店)
    '    ''' </summary>
    '    ''' <param name="dtAfterVisit">付替え先予約情報</param>
    '    ''' <param name="dtAfterReserve">付替え先来店情報</param>
    '    ''' <param name="rowBeforeVisit">付替え元来店管理情報</param>
    '    ''' <param name="beforeVisitNumber">来店実績連番</param>
    '    ''' <returns>付け替え先情報</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Function GetAfterReplaceInfo(ByVal dtAfterVisit As SC3140103SearchChangesVisitDataTable, _
    '                                         ByVal dtAfterReserve As SC3140103SearchChangesReserveDataTable, _
    '                                         ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
    '                                         ByVal beforeVisitNumber As Long) As SC3140103BeforeChangeCheckResultRow
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim dtResult As New SC3140103BeforeChangeCheckResultDataTable
    '        Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
    '            DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        If dtAfterVisit IsNot Nothing Then
    '            For Each rowAfterVisit As SC3140103SearchChangesVisitRow In dtAfterVisit.Select("", "VISITTIMESTAMP ASC")
    '                Dim afterVisitOrderNo As String = String.Empty

    '                If Not rowAfterVisit.IsORDERNONull AndAlso _
    '                    Not String.IsNullOrEmpty(rowAfterVisit.ORDERNO) Then
    '                    afterVisitOrderNo = rowAfterVisit.ORDERNO
    '                End If

    '                If Not beforeVisitNumber.Equals(rowAfterVisit.VISITSEQ) Then
    '                    '付替え元先で来店実績連番が異なる場合
    '                    If Not rowAfterVisit.IsASSIGNSTATUSNull Then
    '                        If ASSIGN_STATUS_HOLD.Equals(rowAfterVisit.ASSIGNSTATUS) Then
    '                            '付替え先来店情報のSA振当てステータスがホールド中の場合
    '                            Throw New ArgumentException

    '                        ElseIf Not ASSIGN_STATUS_ASSIGN_FINISH.Equals(rowAfterVisit.ASSIGNSTATUS) Then
    '                            '付替え先来店情報のSA振当てステータスが未振当ての場合
    '                            resultRow.AFTERVISITNO = rowAfterVisit.VISITSEQ
    '                            resultRow.AFTERRESERVENO = rowAfterVisit.FREZID
    '                            resultRow.AFTERORDERNO = afterVisitOrderNo

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                            '車両IDを追加                            
    '                            resultRow.AFTERVCLID = rowAfterVisit.VCL_ID

    '                            '顧客IDを追加
    '                            resultRow.AFTERCSTID = rowAfterVisit.CST_ID

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                            Exit For
    '                        Else
    '                            '付替え先来店情報のSA振当てステータスが振当て済の場合

    '                            If rowAfterVisit.IsORDERNONull Then
    '                                '整備受注NOが未採番の場合
    '                                resultRow.AFTERVISITNO = rowAfterVisit.VISITSEQ
    '                                resultRow.AFTERRESERVENO = rowAfterVisit.FREZID

    '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                                '車両IDを追加                            
    '                                resultRow.AFTERVCLID = rowAfterVisit.VCL_ID

    '                                '顧客IDを追加
    '                                resultRow.AFTERCSTID = rowAfterVisit.CST_ID

    '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                                If Not String.IsNullOrEmpty(rowAfterVisit.SACODE) Then
    '                                    resultRow.AFTERSACODE = rowAfterVisit.SACODE
    '                                Else
    '                                    If Not String.IsNullOrEmpty(rowAfterVisit.ACCOUNT_PLAN) Then
    '                                        resultRow.AFTERSACODE = rowAfterVisit.ACCOUNT_PLAN
    '                                    End If
    '                                End If

    '                                Exit For
    '                            Else
    '                                Dim dtROBaseInfo As IC3801001OrderCommDataTable

    '                                ' R/O基本情報を取得
    '                                dtROBaseInfo = Me.GetROBaseInfoList(staffInfo.DlrCD, afterVisitOrderNo)

    '                                If dtROBaseInfo IsNot Nothing AndAlso 0 < dtROBaseInfo.Rows.Count Then
    '                                    Dim drROBaseInfo As IC3801001OrderCommRow = _
    '                                        DirectCast(dtROBaseInfo.Rows(0), IC3801001OrderCommRow)
    '                                    If Not drROBaseInfo.IsOrderStatusNull Then
    '                                        If (C_RO_STATUS_RECEPTION.Equals(drROBaseInfo.OrderStatus) Or _
    '                                            C_RO_STATUS_ESTI_WAIT.Equals(drROBaseInfo.OrderStatus)) Then

    '                                            resultRow.AFTERVISITNO = rowAfterVisit.VISITSEQ
    '                                            resultRow.AFTERRESERVENO = rowAfterVisit.FREZID
    '                                            resultRow.AFTERORDERNO = afterVisitOrderNo

    '                                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                                            '車両IDを追加                            
    '                                            resultRow.AFTERVCLID = rowAfterVisit.VCL_ID

    '                                            '顧客IDを追加
    '                                            resultRow.AFTERCSTID = rowAfterVisit.CST_ID

    '                                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                                            If Not String.IsNullOrEmpty(rowAfterVisit.SACODE) Then
    '                                                resultRow.AFTERSACODE = rowAfterVisit.SACODE
    '                                            Else
    '                                                If Not String.IsNullOrEmpty(rowAfterVisit.ACCOUNT_PLAN) Then
    '                                                    resultRow.AFTERSACODE = rowAfterVisit.ACCOUNT_PLAN
    '                                                End If
    '                                            End If

    '                                            Exit For
    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If

    '        '付替え先来店実績連番が取得できなかった場合
    '        If resultRow.IsAFTERVISITNONull Then
    '            resultRow = Me.GetAfterReplaceReserveInfo(dtAfterVisit, dtAfterReserve, rowBeforeVisit)
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '        Return resultRow
    '    End Function

    '    ''' <summary>
    '    ''' 付け替え先情報の取得(予約)
    '    ''' </summary>
    '    ''' <param name="dtAfterVisit">付替え先予約情報</param>
    '    ''' <param name="dtAfterReserve">付替え先来店情報</param>
    '    ''' <param name="rowBeforeVisit">付替え元来店管理情報</param>
    '    ''' <returns>付け替え先情報</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    Private Function GetAfterReplaceReserveInfo(ByVal dtAfterVisit As SC3140103SearchChangesVisitDataTable, _
    '                                                ByVal dtAfterReserve As SC3140103SearchChangesReserveDataTable, _
    '                                                ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow) _
    '                                            As SC3140103BeforeChangeCheckResultRow
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim dtResult As New SC3140103BeforeChangeCheckResultDataTable
    '        Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
    '            DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

    '        If dtAfterReserve IsNot Nothing AndAlso 0 < dtAfterReserve.Rows.Count Then
    '            For Each rowAfterReserve As SC3140103SearchChangesReserveRow _
    '                In dtAfterReserve.Select("", "REZ_PICK_DATE ASC")

    '                Dim afterReserveOrderNo As String = String.Empty
    '                If Not rowAfterReserve.IsORDERNONull AndAlso Not String.IsNullOrEmpty(rowAfterReserve.ORDERNO) Then
    '                    afterReserveOrderNo = rowAfterReserve.ORDERNO
    '                End If

    '                Dim afterReserveSaCode As String = String.Empty
    '                If Not rowAfterReserve.IsACCOUNT_PLANNull AndAlso _
    '                    Not String.IsNullOrEmpty(rowAfterReserve.ACCOUNT_PLAN) Then
    '                    afterReserveSaCode = rowAfterReserve.ACCOUNT_PLAN
    '                End If

    '                If Not rowBeforeVisit.FREZID.Equals(rowAfterReserve.RESERVEID) Then
    '                    Dim dataRowCount As Long = 0

    '                    If dtAfterVisit IsNot Nothing Then
    '                        Dim dataRow As DataRow() = _
    '                            dtAfterVisit.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                                              "FREZID = '{0}'", rowAfterReserve.RESERVEID))
    '                        If dataRow IsNot Nothing Then
    '                            dataRowCount = dataRow.Count
    '                        End If
    '                    End If

    '                    If dataRowCount = 0 Then
    '                        If Not rowAfterReserve.IsORDERNONull Then
    '                            resultRow.AFTERRESERVENO = rowAfterReserve.RESERVEID
    '                            resultRow.AFTERORDERNO = afterReserveOrderNo
    '                            resultRow.AFTERSACODE = afterReserveSaCode

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '                            '車両IDを追加
    '                            resultRow.AFTERVCLID = rowAfterReserve.VCL_ID

    '                            ' 
    '                            '顧客IDを追加
    '                            resultRow.AFTERCSTID = rowAfterReserve.CST_ID

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                            Exit For
    '                        Else
    '                            resultRow.AFTERRESERVENO = rowAfterReserve.RESERVEID
    '                            resultRow.AFTERSACODE = afterReserveSaCode

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '                            '車両IDを追加
    '                            resultRow.AFTERVCLID = rowAfterReserve.VCL_ID

    '                            '顧客IDを追加
    '                            resultRow.AFTERCSTID = rowAfterReserve.CST_ID

    '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                            Exit For
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '        Return resultRow

    '    End Function

    '    ''' <summary>
    '    ''' 付替え先の担当が他SAでないかのチェック
    '    ''' </summary>
    '    ''' <param name="resultRow">付け替え先情報</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckChangeConfirmation(ByVal resultRow As SC3140103BeforeChangeCheckResultRow) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        '付替え先予約情報が有る場合（予約NoがNullでない場合）
    '        If Not resultRow.IsAFTERRESERVENONull AndAlso 0 < resultRow.AFTERRESERVENO Then

    '            If (Not resultRow.IsAFTERSACODENull) AndAlso _
    '                Not staffInfo.Account.Equals(resultRow.AFTERSACODE) And _
    '                Not String.IsNullOrEmpty(resultRow.AFTERSACODE) Then

    '                Return ChangeReusltCheck
    '            End If
    '        End If

    '        '付替え先来店情報がある場合（来店実績連番がNullでない場合）
    '        If Not resultRow.IsAFTERVISITNONull Then
    '            If (Not resultRow.IsAFTERSACODENull) And _
    '                (Not staffInfo.Account.Equals(resultRow.AFTERSACODE)) And _
    '                (Not String.IsNullOrEmpty(resultRow.AFTERSACODE)) Then

    '                Return ChangeReusltCheck
    '            End If
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '        Return ChangeResultTrue

    '    End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '    ''' <summary>
    '    ''' 付替え先顧客車両情報取得
    '    ''' </summary>
    '    ''' <param name="resultRow">付替え先情報</param>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="resisterNumber">車両登録番号</param>
    '    ''' <param name="vinNumber">VIN</param>
    '    ''' <returns>付替え先情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetAfterVehicleInfo(ByVal resultRow As SC3140103BeforeChangeCheckResultRow, _
    '                                         ByVal dealerCode As String, _
    '                                         ByVal resisterNumber As String, _
    '                                         ByVal vinNumber As String) As SC3140103BeforeChangeCheckResultRow

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        'Dacのインスタンス
    '        Using da As New SC3140103DataTableAdapter

    '            '付替え先車両情報取得
    '            Dim dtAfterVehicleInfo As SC3140103AfterVehicleInfoDataTable = _
    '                da.GetDBAfterVehicleInfo(dealerCode, resisterNumber, vinNumber)

    '            '付替え先車両情報取得確認
    '            If 0 < dtAfterVehicleInfo.Count Then
    '                '付替え先車両情報が取得できた場合

    '                '車両ID確認
    '                If Not dtAfterVehicleInfo(0).IsVCL_IDNull Then
    '                    '車両IDが存在する場合

    '                    '車両情報の先頭行の車両IDを結果に設定
    '                    resultRow.AFTERVCLID = dtAfterVehicleInfo(0).VCL_ID

    '                End If

    '                '顧客ID確認
    '                If Not dtAfterVehicleInfo(0).IsCST_IDNull Then
    '                    '顧客IDが存在する場合

    '                    '顧客車両情報の先頭行の顧客IDを結果に設定
    '                    resultRow.AFTERCSTID = dtAfterVehicleInfo(0).CST_ID

    '                End If

    '            End If

    '        End Using

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '        Return resultRow

    '    End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '#End Region

    '#Region "顧客付替え処理"
    '    ''' <summary>
    '    ''' 顧客付替え処理
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    '    ''' <param name="afterVisitNumber">付替え先来店実績連番</param>
    '    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    '    ''' <param name="afterOrderNumber">付替え先整備受注No.</param>
    '    ''' <param name="registrationNumber">車両登録No</param>
    '    ''' <param name="customerCode">顧客コード</param>
    '    ''' <param name="basicCustomerId">基幹顧客ID</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <param name="modelCode">モデルコード</param>
    '    ''' <param name="customerName">氏名</param>
    '    ''' <param name="phone">電話番号</param>
    '    ''' <param name="mobile">携帯番号</param>
    '    ''' <param name="afterVehicleId">付替え先車両ID</param>
    '    ''' <returns>成功：0　失敗：その他</returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    <EnableCommit()>
    '    Public Function SetCustomerChange(ByVal dealerCode As String, _
    '                                  ByVal storeCode As String, _
    '                                  ByVal beforeVisitNumber As Long, _
    '                                  ByVal afterVisitNumber As Long, _
    '                                  ByVal afterReserveNumber As Long, _
    '                                  ByVal afterOrderNumber As String, _
    '                                  ByVal registrationNumber As String, _
    '                                  ByVal customerCode As String, _
    '                                  ByVal basicCustomerId As String, _
    '                                  ByVal vin As String, _
    '                                  ByVal modelCode As String, _
    '                                  ByVal customerName As String, _
    '                                  ByVal phone As String, _
    '                                  ByVal mobile As String, _
    '                                  ByVal afterVehicleId As Long) As Long

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'Public Function SetCustomerChange(ByVal dealerCode As String, _
    '        '                                  ByVal storeCode As String, _
    '        '                                  ByVal beforeVisitNumber As Long, _
    '        '                                  ByVal afterVisitNumber As Long, _
    '        '                                  ByVal afterReserveNumber As Long, _
    '        '                                  ByVal afterOrderNumber As String, _
    '        '                                  ByVal registrationNumber As String, _
    '        '                                  ByVal customerCode As String, _
    '        '                                  ByVal basicCustomerId As String, _
    '        '                                  ByVal vin As String, _
    '        '                                  ByVal modelCode As String, _
    '        '                                  ByVal customerName As String, _
    '        '                                  ByVal phone As String, _
    '        '                                  ByVal mobile As String) As Long]

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '           , "{0}.{1} {2}" _
    '           , Me.GetType.ToString _
    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '           , LOG_START))

    '        '付替え元来店管理情報取得
    '        Dim dtBeforeVisit As SC3140103ChangesServiceVisitManagementDataTable
    '        Using da As New SC3140103DataTableAdapter
    '            dtBeforeVisit = da.GetServiseVisitManagementForChangeDate(dealerCode, _
    '                                                                      storeCode, _
    '                                                                      beforeVisitNumber)
    '        End Using

    '        Dim rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow = _
    '            DirectCast(dtBeforeVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

    '        Dim beforeReserveNumberResult As Long = -1

    '        If Not rowBeforeVisit.IsFREZIDNull AndAlso 0 < rowBeforeVisit.FREZID Then
    '            beforeReserveNumberResult = rowBeforeVisit.FREZID
    '        End If

    '        Dim beforeOrderNumberResult As String = String.Empty
    '        If String.IsNullOrEmpty(rowBeforeVisit.ORDERNO) Then
    '            beforeOrderNumberResult = rowBeforeVisit.ORDERNO
    '        End If

    '        Dim beforeAssignDate As Date = Nothing
    '        If Not rowBeforeVisit.IsASSIGNTIMESTAMPNull Then
    '            beforeAssignDate = rowBeforeVisit.ASSIGNTIMESTAMP
    '        End If

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        ' 振当SAチェック
    '        If Not rowBeforeVisit.SACODE.Equals(staffInfo.Account) Then
    '            Return ChangeResultDifference
    '        End If

    '        Dim beforeAssignStatus As String = String.Empty
    '        ' 付替え元来店情報ホールドチェック
    '        If ASSIGN_STATUS_HOLD.Equals(rowBeforeVisit.ASSIGNSTATUS) Then
    '            Return ChangeResultBeforeHold
    '        Else
    '            beforeAssignStatus = rowBeforeVisit.ASSIGNSTATUS
    '        End If

    '        ' 付替え元来店情報の予約情報から、ストール予約情報取得
    '        Dim beforeStockDate As Date = Nothing

    '        If Not rowBeforeVisit.IsFREZIDNull AndAlso 0 < rowBeforeVisit.FREZID Then
    '            beforeStockDate = Me.GetBeforeStockDate(dealerCode, storeCode, rowBeforeVisit.FREZID)
    '        End If

    '        ' R/O情報がある場合、R/O基本情報を参照する
    '        Dim beforeOrderStatus As String = String.Empty

    '        If Not rowBeforeVisit.IsORDERNONull Then
    '            Try
    '                beforeOrderStatus = Me.GetBeforeOrderStatus(dealerCode, rowBeforeVisit.ORDERNO)
    '            Catch ex As ArgumentException
    '                Return ChangeResultApproval
    '            End Try
    '        End If

    '        '付替え先来店連番がある場合、付替え先来店管理情報のホールド状態をチェック
    '        Dim checkHoldResult As Long = Me.CheckAfterVisitAssignStatus(dealerCode, _
    '                                                                     storeCode, _
    '                                                                     afterVisitNumber)
    '        If Not ChangeResultTrue.Equals(checkHoldResult) Then
    '            Return checkHoldResult
    '        End If


    '        ' R/O情報がある場合、R/O基本情報を参照する
    '        Dim dtAfterROBaseInfo As IC3801001OrderCommDataTable
    '        Dim rowAfterROBaseInfo As IC3801001OrderCommRow
    '        Dim afterOrderStatus As String = String.Empty
    '        If Not String.IsNullOrEmpty(afterOrderNumber) Then
    '            ' R/O基本情報を取得
    '            dtAfterROBaseInfo = Me.GetROBaseInfoList(dealerCode, afterOrderNumber)
    '            If dtAfterROBaseInfo IsNot Nothing AndAlso 0 < dtAfterROBaseInfo.Rows.Count Then
    '                rowAfterROBaseInfo = DirectCast(dtAfterROBaseInfo.Rows(0), IC3801001OrderCommRow)
    '                If Not rowAfterROBaseInfo.IsOrderStatusNull Then
    '                    afterOrderStatus = rowAfterROBaseInfo.OrderStatus
    '                End If

    '                'R/Oステータスが1or5以外の場合
    '                If Not (C_RO_STATUS_RECEPTION.Equals(afterOrderStatus) Or _
    '                         C_RO_STATUS_ESTI_WAIT.Equals(afterOrderStatus)) Then
    '                    afterOrderNumber = String.Empty
    '                    afterReserveNumber = -1
    '                    afterVisitNumber = -1
    '                End If
    '            Else
    '                afterOrderNumber = String.Empty
    '                afterReserveNumber = -1
    '                afterVisitNumber = -1
    '            End If
    '        End If

    '        Try

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            'Dim updateResult As Long = Me.UpdateCustomerChange(afterReserveNumber, _
    '            '                                                   afterOrderNumber, _
    '            '                                                   beforeAssignDate, _
    '            '                                                   beforeReserveNumberResult, _
    '            '                                                   beforeOrderNumberResult, _
    '            '                                                   beforeAssignStatus, _
    '            '                                                   beforeOrderStatus, _
    '            '                                                   beforeStockDate, _
    '            '                                                   rowBeforeVisit, _
    '            '                                                   beforeVisitNumber, _
    '            '                                                   customerCode, _
    '            '                                                   basicCustomerId, _
    '            '                                                   customerName, _
    '            '                                                   phone, _
    '            '                                                   mobile, _
    '            '                                                   registrationNumber, _
    '            '                                                   vin, _
    '            '                                                   modelCode, _
    '            '                                                   afterVisitNumber)

    '            Dim updateResult As Long = Me.UpdateCustomerChange(afterReserveNumber, _
    '                                                               afterOrderNumber, _
    '                                                               beforeAssignDate, _
    '                                                               beforeReserveNumberResult, _
    '                                                               beforeOrderNumberResult, _
    '                                                               beforeAssignStatus, _
    '                                                               beforeOrderStatus, _
    '                                                               beforeStockDate, _
    '                                                               rowBeforeVisit, _
    '                                                               beforeVisitNumber, _
    '                                                               customerCode, _
    '                                                               basicCustomerId, _
    '                                                               customerName, _
    '                                                               phone, _
    '                                                               mobile, _
    '                                                               registrationNumber, _
    '                                                               vin, _
    '                                                               modelCode, _
    '                                                               afterVisitNumber, _
    '                                                               afterVehicleId)

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '            If Not ResultSuccess.Equals(updateResult) Then
    '                Me.Rollback = True
    '                ''終了ログの出力
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                             , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                             , Me.GetType.ToString _
    '                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                             , updateResult))
    '                Return updateResult
    '            End If

    '            Return ResultSuccess

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'ORACLEのタイムアウトのみ処理
    '            Me.Rollback = True
    '            ''終了ログの出力
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                         , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                         , Me.GetType.ToString _
    '                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                         , ResultTimeout))
    '            Return ResultTimeout
    '        Catch ex As Exception
    '            Me.Rollback = True
    '            ''エラーログの出力
    '            Logger.Error(ex.Message, ex)
    '            Throw
    '        Finally
    '            'TODO いろいろとDispose
    '        End Try

    '    End Function

    '    ''' <summary>
    '    ''' 付替え元来店情報の予約情報から入庫日時を取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="reserveId">管理予約ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function GetBeforeStockDate(ByVal dealerCode As String, _
    '                                        ByVal storeCode As String, _
    '                                        ByVal reserveId As Long) As Date
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        ' 付替え元来店情報の予約情報から、ストール予約情報取得
    '        Dim dtBeforeReserve As SC3140103ChangesStallReserveDataTable

    '        Dim beforeStockDate As Date = Nothing

    '        Using da As New SC3140103DataTableAdapter
    '            dtBeforeReserve = da.GetStallReserveInfoForChangeData(dealerCode, _
    '                                                                  storeCode, _
    '                                                                  reserveId)
    '        End Using

    '        If dtBeforeReserve IsNot Nothing AndAlso 0 < dtBeforeReserve.Rows.Count Then
    '            '作業開始日時の昇順でソート
    '            Dim rowBeforeReserve As SC3140103ChangesStallReserveRow = _
    '                DirectCast(dtBeforeReserve.Select("", "STARTTIME ASC")(0), SC3140103ChangesStallReserveRow)

    '            If Not rowBeforeReserve Is Nothing Then
    '                If Not rowBeforeReserve.IsSTOCKTIMENull Then

    '                    beforeStockDate = _
    '                        DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowBeforeReserve.STOCKTIME)
    '                End If
    '            End If
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))
    '        Return beforeStockDate
    '    End Function

    '    ''' <summary>
    '    ''' 付替え元来店情報のR/Oステータスをチェック
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="orderNo">整備受注No</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function GetBeforeOrderStatus(ByVal dealerCode As String, _
    '                                          ByVal orderNo As String) As String

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:dealerCode={3}, orderNo={4}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , dealerCode _
    '                                 , orderNo))

    '        ' R/O情報がある場合、R/O基本情報を参照する
    '        Dim dtBeforeROBaseInfo As IC3801001OrderCommDataTable = Nothing
    '        Dim rowBeforeROBaseInfo As IC3801001OrderCommRow = Nothing
    '        Dim beforeOrderStatus As String = String.Empty

    '        ' R/O基本情報を取得
    '        dtBeforeROBaseInfo = Me.GetROBaseInfoList(dealerCode, orderNo)

    '        If dtBeforeROBaseInfo IsNot Nothing AndAlso 0 < dtBeforeROBaseInfo.Rows.Count Then
    '            rowBeforeROBaseInfo = DirectCast(dtBeforeROBaseInfo.Rows(0), IC3801001OrderCommRow)
    '            If rowBeforeROBaseInfo.IsOrderStatusNull Then
    '                '整備受注NoがNullの場合、NG返却
    '                Throw New ArgumentException
    '            Else
    '                beforeOrderStatus = rowBeforeROBaseInfo.OrderStatus
    '                If Not (C_RO_STATUS_RECEPTION.Equals(beforeOrderStatus) Or _
    '                    C_RO_STATUS_ESTI_WAIT.Equals(beforeOrderStatus)) Then
    '                    'R/O承認済みの場合、NG返却
    '                    Throw New ArgumentException
    '                End If
    '            End If
    '        Else
    '            'R/O承認済みの場合、NG返却
    '            Throw New ArgumentException
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , beforeOrderStatus))
    '        Return beforeOrderStatus
    '    End Function

    '    ''' <summary>
    '    ''' 付け替え先来店情報のホールド状態をチェック
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="afterVisitNumber">来店実績連番</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckAfterVisitAssignStatus(ByVal dealerCode As String, _
    '                                                 ByVal storeCode As String, _
    '                                                 ByVal afterVisitNumber As Long) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:dealerCode={3}, orderNo={4}, afterVisitNumber={5}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , dealerCode _
    '                                 , storeCode _
    '                                 , afterVisitNumber))

    '        Dim dtAfterVisit As SC3140103ChangesServiceVisitManagementDataTable
    '        Dim rowAfterVisit As SC3140103ChangesServiceVisitManagementRow

    '        If 0 < afterVisitNumber Then
    '            Using da As New SC3140103DataTableAdapter
    '                dtAfterVisit = da.GetServiseVisitManagementForChangeDate(dealerCode, storeCode, afterVisitNumber)
    '                rowAfterVisit = DirectCast(dtAfterVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)
    '                ' 付替え先来店情報ホールドチェック
    '                If Not rowAfterVisit.IsASSIGNSTATUSNull Then
    '                    If ASSIGN_STATUS_HOLD.Equals(rowAfterVisit.ASSIGNSTATUS) Then
    '                        Return ChangeResultAfterHold
    '                    End If
    '                End If
    '            End Using
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ChangeResultTrue))
    '        Return ChangeResultTrue
    '    End Function

    '    ''' <summary>
    '    ''' 顧客付替え処理(DB更新)
    '    ''' </summary>
    '    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    '    ''' <param name="afterOrderNumber">付替え先整備受注No.</param>
    '    ''' <param name="beforeAssignDate">付替え元来店時間</param>
    '    ''' <param name="beforeReserveNumberResult">付替え元予約ID</param>
    '    ''' <param name="beforeOrderNumberResult">付替え元整備受注No.</param>
    '    ''' <param name="beforeAssignStatus">付替え元来店ステータス</param>
    '    ''' <param name="beforeOrderStatus">付替え元ステータス</param>
    '    ''' <param name="beforeStockDate">付替え元入庫日時</param>
    '    ''' <param name="rowBeforeVisit">付替え元来店管理情報</param>
    '    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    '    ''' <param name="customerCode">顧客コード</param>
    '    ''' <param name="basicCustomerId">基幹顧客ID</param>
    '    ''' <param name="customerName">氏名</param>
    '    ''' <param name="phone">電話番号</param>
    '    ''' <param name="mobile">携帯番号</param>
    '    ''' <param name="registrationNumber">車両登録No</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <param name="modelCode">モデルコード</param>
    '    ''' <param name="afterVisitNumber">付替え先来店実績連番</param>
    '    ''' <param name="afterVehicleId">付替え先車両ID</param>
    '    ''' <returns>成功：0　失敗：その他</returns>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </History>
    '    Private Function UpdateCustomerChange(ByVal afterReserveNumber As Long, _
    '                                          ByVal afterOrderNumber As String, _
    '                                          ByVal beforeAssignDate As Date, _
    '                                          ByVal beforeReserveNumberResult As Long, _
    '                                          ByVal beforeOrderNumberResult As String, _
    '                                          ByVal beforeAssignStatus As String, _
    '                                          ByVal beforeOrderStatus As String, _
    '                                          ByVal beforeStockDate As Date, _
    '                                          ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
    '                                          ByVal beforeVisitNumber As Long, _
    '                                          ByVal customerCode As String, _
    '                                          ByVal basicCustomerId As String, _
    '                                          ByVal customerName As String, _
    '                                          ByVal phone As String, _
    '                                          ByVal mobile As String, _
    '                                          ByVal registrationNumber As String, _
    '                                          ByVal vin As String, _
    '                                          ByVal modelCode As String, _
    '                                          ByVal afterVisitNumber As Long, _
    '                                          ByVal afterVehicleId As Long) As Long

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        'Private Function UpdateCustomerChange(ByVal afterReserveNumber As Long, _
    '        '                                  ByVal afterOrderNumber As String, _
    '        '                                  ByVal beforeAssignDate As Date, _
    '        '                                  ByVal beforeReserveNumberResult As Long, _
    '        '                                  ByVal beforeOrderNumberResult As String, _
    '        '                                  ByVal beforeAssignStatus As String, _
    '        '                                  ByVal beforeOrderStatus As String, _
    '        '                                  ByVal beforeStockDate As Date, _
    '        '                                  ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
    '        '                                  ByVal beforeVisitNumber As Long, _
    '        '                                  ByVal customerCode As String, _
    '        '                                  ByVal basicCustomerId As String, _
    '        '                                  ByVal customerName As String, _
    '        '                                  ByVal phone As String, _
    '        '                                  ByVal mobile As String, _
    '        '                                  ByVal registrationNumber As String, _
    '        '                                  ByVal vin As String, _
    '        '                                  ByVal modelCode As String, _
    '        '                                  ByVal afterVisitNumber As Long) As Long

    '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current

    '        '日付管理機能（現在日付取得）
    '        Dim nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

    '        Dim blCommon As New SMBCommonClassBusinessLogic

    '        Try

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            'サービス入庫行ロック処理
    '            Dim lockResult As Long = Me.ServiceInLock(beforeReserveNumberResult, _
    '                                                      afterReserveNumber, _
    '                                                      blCommon, _
    '                                                      staffInfo, _
    '                                                      nowDateTime)

    '            'ロック処理結果確認
    '            If Not ResultSuccess.Equals(lockResult) Then
    '                'ロック失敗

    '                Return lockResult
    '            End If


    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '            If 0 < afterReserveNumber Or _
    '                (Not afterOrderNumber Is Nothing AndAlso Not String.IsNullOrEmpty(afterOrderNumber)) Then
    '                '付替え先担当SA変更

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


    '                'Dim resultSaChange As Long = blCommon.ChangeSACode(staffInfo.DlrCD, _
    '                '                                                   staffInfo.BrnCD, _
    '                '                                                   afterReserveNumber, _
    '                '                                                   afterOrderNumber, _
    '                '                                                   staffInfo.Account, _
    '                '                                                   RESERVE_FLAG_TRUE, _
    '                '                                                   beforeAssignDate, _
    '                '                                                   staffInfo.Account, _
    '                '                                                   nowDateTime)

    '                Dim resultSaChange As Long = blCommon.ChangeSACode(staffInfo.DlrCD, _
    '                                                                   staffInfo.BrnCD, _
    '                                                                   afterReserveNumber, _
    '                                                                   afterOrderNumber, _
    '                                                                   staffInfo.Account, _
    '                                                                   RESERVE_FLAG_TRUE, _
    '                                                                   beforeAssignDate, _
    '                                                                   staffInfo.Account, _
    '                                                                   nowDateTime, _
    '                                                                   MAINMENUID)


    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                If Not ResultSuccess.Equals(resultSaChange) Then
    '                    Return resultSaChange
    '                End If
    '            End If
    '            '付替え元R/O情報が有る場合
    '            If 0 < beforeReserveNumberResult And _
    '                (Not beforeOrderNumberResult Is Nothing AndAlso _
    '                 Not String.IsNullOrEmpty(beforeOrderNumberResult)) Then

    '                If ASSIGN_STATUS_ASSIGN_FINISH.Equals(beforeAssignStatus) Then

    '                    If (C_RO_STATUS_RECEPTION.Equals(beforeOrderStatus) Or _
    '                        C_RO_STATUS_ESTI_WAIT.Equals(beforeOrderStatus)) Then

    '                        '付替え元担当SA変更

    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                        'Dim resultBeforeSaChange As Long = blCommon.ChangeSACode(staffInfo.DlrCD, _
    '                        '                                                         staffInfo.BrnCD, _
    '                        '                                                         SMBCommonClassBusinessLogic.NoReserveId, _
    '                        '                                                         beforeOrderNumberResult, _
    '                        '                                                         staffInfo.Account, _
    '                        '                                                         RESERVE_FLAG_RESERVE, _
    '                        '                                                         beforeStockDate, _
    '                        '                                                         staffInfo.Account, _
    '                        '                                                         nowDateTime)

    '                        Dim resultBeforeSaChange As Long = blCommon.ChangeSACode(staffInfo.DlrCD, _
    '                                                                                 staffInfo.BrnCD, _
    '                                                                                 NoReserveId, _
    '                                                                                 beforeOrderNumberResult, _
    '                                                                                 staffInfo.Account, _
    '                                                                                 RESERVE_FLAG_RESERVE, _
    '                                                                                 beforeStockDate, _
    '                                                                                 staffInfo.Account, _
    '                                                                                 nowDateTime, _
    '                                                                                 MAINMENUID)


    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                        If Not ResultSuccess.Equals(resultBeforeSaChange) Then
    '                            Return resultBeforeSaChange
    '                        End If
    '                    End If
    '                End If
    '            End If

    '            If 0 < beforeReserveNumberResult Or 0 < afterReserveNumber Then
    '                '入庫予定日時付替え処理

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                'Dim resultStockDateChange As Long = blCommon.ChangeCarInDate(staffInfo.DlrCD, _
    '                '                                                             staffInfo.BrnCD, _
    '                '                                                             beforeReserveNumberResult, _
    '                '                                                             afterReserveNumber, _
    '                '                                                             beforeAssignDate, _
    '                '                                                             staffInfo.Account, _
    '                '                                                             nowDateTime)

    '                Dim resultStockDateChange As Long = blCommon.ChangeCarInDate(staffInfo.DlrCD, _
    '                                                                             staffInfo.BrnCD, _
    '                                                                             beforeReserveNumberResult, _
    '                                                                             afterReserveNumber, _
    '                                                                             beforeAssignDate, _
    '                                                                             staffInfo.Account, _
    '                                                                             nowDateTime, _
    '                                                                             MAINMENUID)

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                If Not ResultSuccess.Equals(resultStockDateChange) Then
    '                    Return resultStockDateChange
    '                End If

    '            End If

    '            '付替え元来店情報が有る場合
    '            If rowBeforeVisit IsNot Nothing Then
    '                '付替え元サービス来店顧客更新
    '                Using da As New SC3140103DataTableAdapter

    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                    'Dim resultUpdateVisit As Long = da.SetVisitCustomer(beforeVisitNumber, _
    '                    '                                                    "1", _
    '                    '                                                    customerCode, _
    '                    '                                                    basicCustomerId, _
    '                    '                                                    customerName, _
    '                    '                                                    phone, _
    '                    '                                                    mobile, _
    '                    '                                                    "0", _
    '                    '                                                    registrationNumber,
    '                    '                                                    vin, _
    '                    '                                                    modelCode, _
    '                    '                                                    afterReserveNumber, _
    '                    '                                                    afterOrderNumber,
    '                    '                                                    staffInfo.Account, _
    '                    '                                                    staffInfo.Account, _
    '                    '                                                    nowDateTime, _
    '                    '                                                    staffInfo.Account, _
    '                    '                                                    MAINMENUID)


    '                    Dim resultUpdateVisit As Long = da.SetVisitCustomer(beforeVisitNumber, _
    '                                                                        "1", _
    '                                                                        customerCode, _
    '                                                                        basicCustomerId, _
    '                                                                        customerName, _
    '                                                                        phone, _
    '                                                                        mobile, _
    '                                                                        "0", _
    '                                                                        registrationNumber,
    '                                                                        vin, _
    '                                                                        modelCode, _
    '                                                                        afterReserveNumber, _
    '                                                                        afterOrderNumber,
    '                                                                        staffInfo.Account, _
    '                                                                        staffInfo.Account, _
    '                                                                        nowDateTime, _
    '                                                                        staffInfo.Account, _
    '                                                                        MAINMENUID, _
    '                                                                        afterVehicleId)

    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '                    If resultUpdateVisit <> 1 Then
    '                        Return ResultDBError
    '                    End If
    '                End Using
    '            End If

    '            '付替え先来店実績連番があり、付替え先担当SAが変更された場合
    '            If 0 < afterVisitNumber Then
    '                '付替え先サービス来店顧客クリア
    '                Using da As New SC3140103DataTableAdapter
    '                    Dim resultClearVisit As Long = da.VisitCustomerClear(afterVisitNumber, _
    '                                                                         nowDateTime, _
    '                                                                         staffInfo.Account, _
    '                                                                         MAINMENUID)
    '                    If resultClearVisit <> 1 Then
    '                        Return ResultDBError
    '                    End If
    '                End Using
    '            End If

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            ''付替え先予約履歴の作成
    '            If 0 < afterReserveNumber Then
    '                'Dim resultAfterReserveHistory As Long = _
    '                '    blCommon.RegisterStallReserveHis(staffInfo.DlrCD, _
    '                '                                     staffInfo.BrnCD, _
    '                '                                     afterReserveNumber, _
    '                '                                     nowDateTime,
    '                '                                     SMBCommonClassBusinessLogic.RegisterType.ReserveHisAll)

    '                'If Not ResultSuccess.Equals(resultAfterReserveHistory) Then
    '                '    Return resultAfterReserveHistory
    '                'End If

    '                blCommon.RegisterStallReserveHis(staffInfo.DlrCD, _
    '                                                 staffInfo.BrnCD, _
    '                                                 afterReserveNumber, _
    '                                                 nowDateTime,
    '                                                 RegisterType.RegisterServiceIn, _
    '                                                 staffInfo.Account, _
    '                                                 MAINMENUID, _
    '                                                 NoActivityId)


    '            End If
    '            '付替え元予約履歴の作成
    '            If 0 < beforeReserveNumberResult Then
    '                'Dim resultBeforeReserveHistory As Long = _
    '                '    blCommon.RegisterStallReserveHis(staffInfo.DlrCD, _
    '                '                                     staffInfo.BrnCD, _
    '                '                                     beforeReserveNumberResult, _
    '                '                                     nowDateTime, _
    '                '                                     SMBCommonClassBusinessLogic.RegisterType.ReserveHisAll)

    '                'If Not ResultSuccess.Equals(resultBeforeReserveHistory) Then
    '                '    Return resultBeforeReserveHistory
    '                'End If

    '                blCommon.RegisterStallReserveHis(staffInfo.DlrCD, _
    '                                                 staffInfo.BrnCD, _
    '                                                 beforeReserveNumberResult, _
    '                                                 nowDateTime,
    '                                                 RegisterType.RegisterServiceIn, _
    '                                                 staffInfo.Account, _
    '                                                 MAINMENUID, _
    '                                                 NoActivityId)

    '            End If

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '            '終了ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} {2} OUT:RETURNCODE = {3}" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , LOG_END _
    '                       , ResultSuccess))
    '        Finally
    '            If blCommon IsNot Nothing Then
    '                blCommon.Dispose()
    '                blCommon = Nothing
    '            End If

    '        End Try
    '        Return ResultSuccess
    '    End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '    ''' <summary>
    '    ''' サービス入庫行ロック処理
    '    ''' </summary>
    '    ''' <param name="beforeReserveNumberResult">付替え元予約ID</param>
    '    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function ServiceInLock(ByVal beforeReserveNumberResult As Long, _
    '                                   ByVal afterReserveNumber As Long, _
    '                                   ByVal blCommon As SMBCommonClassBusinessLogic, _
    '                                   ByVal staffInfo As StaffContext, _
    '                                   ByVal inNowDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:BEFOREREZID={3}, AFTERREZID={4}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , beforeReserveNumberResult _
    '                                 , afterReserveNumber))

    '        '行ロック結果
    '        Dim commonResult As Long = 0

    '        '行ロックバージョン
    '        Dim beforelockVersion As Long = -1

    '        '行ロックバージョン
    '        Dim afterlockVersion As Long = -1

    '        Using da As New SC3140103DataTableAdapter

    '            '付替え元予約情報確認
    '            If 0 < beforeReserveNumberResult Then
    '                '付替え元の予約情報がある場合

    '                'ストール予約の最新情報の取得(行ロックバージョン)
    '                Dim dtBeforeNewReserve As SC3140103NewestStallRezInfoDataTable = da.GetDBNewestStallRezInfo(beforeReserveNumberResult)

    '                '予約情報に取得確認
    '                If 0 < dtBeforeNewReserve.Count Then
    '                    '予約が取得できた場合
    '                    beforelockVersion = dtBeforeNewReserve.Item(0).ROW_LOCK_VERSION

    '                End If

    '                'サービス入庫行ロック処理
    '                commonResult = blCommon.LockServiceInTable(beforeReserveNumberResult, _
    '                                                           beforelockVersion, _
    '                                                           "0", _
    '                                                           staffInfo.Account, _
    '                                                           inNowDate, _
    '                                                           MAINMENUID)


    '            End If

    '            '付替え先予約情報確認
    '            If 0 < afterReserveNumber AndAlso ResultSuccess.Equals(commonResult) Then
    '                '付替え先の予約情報がある場合

    '                'ストール予約の最新情報の取得(行ロックバージョン)
    '                Dim dtAfterNewReserve As SC3140103NewestStallRezInfoDataTable = da.GetDBNewestStallRezInfo(afterReserveNumber)

    '                '予約情報に取得確認
    '                If 0 < dtAfterNewReserve.Count Then
    '                    '予約が取得できた場合
    '                    afterlockVersion = dtAfterNewReserve.Item(0).ROW_LOCK_VERSION

    '                End If

    '                'サービス入庫行ロック処理
    '                commonResult = blCommon.LockServiceInTable(afterReserveNumber, _
    '                                                           afterlockVersion, _
    '                                                           "0", _
    '                                                           staffInfo.Account, _
    '                                                           inNowDate, _
    '                                                           MAINMENUID)

    '            End If



    '            '終了ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} {2} Retrun:{3}" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , LOG_END _
    '                       , commonResult))

    '            Return commonResult

    '        End Using
    '    End Function

    '    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '#End Region

    '#Region "顧客解除処理"
    '    ''' <summary>
    '    ''' 顧客解除処理
    '    ''' </summary>
    '    ''' <param name="removeVisitNumber">解除対象の来店実績連番</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    '''  2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </History>
    '    <EnableCommit()>
    '    Public Function SetCustomerClear(ByVal removeVisitNumber As Long) As Long

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_START))

    '        'ログイン情報管理機能（ログイン情報取得）
    '        Dim staffInfo As StaffContext = StaffContext.Current
    '        Dim dealerCode As String = staffInfo.DlrCD
    '        Dim storeCode As String = staffInfo.BrnCD
    '        '日付管理機能（現在日付取得）
    '        Dim nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

    '        'サービス来店管理情報取得
    '        Dim dtRemoveVisit As SC3140103ChangesServiceVisitManagementDataTable
    '        Using da As New SC3140103DataTableAdapter
    '            dtRemoveVisit = da.GetServiseVisitManagementForChangeDate(dealerCode, _
    '                                                                      storeCode, _
    '                                                                      removeVisitNumber)
    '        End Using

    '        Dim rowRemoveVisit As SC3140103ChangesServiceVisitManagementRow = _
    '            DirectCast(dtRemoveVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

    '        ' ストール予約情報取得
    '        Dim removeReserveNumberResult As Long = -1
    '        Dim removeStockDate As Date = Nothing

    '        If Not rowRemoveVisit.IsFREZIDNull AndAlso 0 < rowRemoveVisit.FREZID Then
    '            removeReserveNumberResult = rowRemoveVisit.FREZID
    '            '入庫日時の取得
    '            removeStockDate = Me.GetRemoveStockDate(staffInfo.DlrCD, _
    '                                                    staffInfo.BrnCD, _
    '                                                    rowRemoveVisit.FREZID)
    '        End If


    '        ' R/O情報がある場合、R/O基本情報を参照する
    '        Dim blCommon As New SMBCommonClassBusinessLogic
    '        Dim dtRemoveROBaseInfo As IC3801001OrderCommDataTable = Nothing
    '        Dim rowRemoveROBaseInfo As IC3801001OrderCommRow
    '        Dim removeOrderStatus As String = String.Empty
    '        Dim removeOrderNumber As String = String.Empty

    '        Try
    '            If Not rowRemoveVisit.IsORDERNONull AndAlso _
    '                Not String.IsNullOrEmpty(rowRemoveVisit.ORDERNO) Then

    '                removeOrderNumber = rowRemoveVisit.ORDERNO
    '                'IF用ログ
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                          , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList IN:dealercode={0}, orderNo={1}" _
    '                          , staffInfo.DlrCD _
    '                          , removeOrderNumber))
    '                ' R/O基本情報を取得
    '                dtRemoveROBaseInfo = Me.GetROBaseInfoList(dealerCode, removeOrderNumber)
    '                If dtRemoveROBaseInfo IsNot Nothing AndAlso 0 < dtRemoveROBaseInfo.Rows.Count Then

    '                    rowRemoveROBaseInfo = DirectCast(dtRemoveROBaseInfo.Rows(0), IC3801001OrderCommRow)
    '                    If Not rowRemoveROBaseInfo.IsOrderStatusNull Then
    '                        removeOrderStatus = rowRemoveROBaseInfo.OrderStatus
    '                    End If
    '                End If
    '            End If

    '            ' 顧客解除処理 更新前チェック
    '            Dim checkResult As Long = Me.CheckCustomerClearState(rowRemoveVisit, _
    '                                                                 dtRemoveROBaseInfo, _
    '                                                                 removeOrderStatus, _
    '                                                                 staffInfo)

    '            If Not ChangeResultTrue.Equals(checkResult) Then
    '                Return checkResult
    '            End If

    '            If dtRemoveROBaseInfo IsNot Nothing AndAlso 0 < dtRemoveROBaseInfo.Rows.Count Then
    '                If 0 < removeReserveNumberResult And Not String.IsNullOrEmpty(removeOrderNumber) Then
    '                    If (C_RO_STATUS_RECEPTION.Equals(removeOrderStatus) Or _
    '                      C_RO_STATUS_ESTI_WAIT.Equals(removeOrderStatus)) Then

    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                        '付替え元担当SA変更
    '                        'Dim resultRemoveSaChange As Long = blCommon.ChangeSACode(dealerCode, _
    '                        '                                                         storeCode, _
    '                        '                                                         SMBCommonClassBusinessLogic.NoReserveId, _
    '                        '                                                         removeOrderNumber, _
    '                        '                                                         staffInfo.Account, _
    '                        '                                                         RESERVE_FLAG_RESERVE, _
    '                        '                                                         removeStockDate, _
    '                        '                                                         staffInfo.Account, _
    '                        '                                                         nowDateTime)

    '                        Dim resultRemoveSaChange As Long = blCommon.ChangeSACode(dealerCode, _
    '                                                                                 storeCode, _
    '                                                                                 SMBCommonClassBusinessLogic.NoReserveId, _
    '                                                                                 removeOrderNumber, _
    '                                                                                 staffInfo.Account, _
    '                                                                                 RESERVE_FLAG_RESERVE, _
    '                                                                                 removeStockDate, _
    '                                                                                 staffInfo.Account, _
    '                                                                                 nowDateTime, _
    '                                                                                 MAINMENUID)

    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                        If Not ResultSuccess.Equals(resultRemoveSaChange) Then
    '                            Me.Rollback = True
    '                            ''終了ログの出力
    '                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                     , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                                     , Me.GetType.ToString _
    '                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                     , resultRemoveSaChange))
    '                            Return resultRemoveSaChange
    '                        End If
    '                    End If
    '                End If
    '            End If

    '            If Not rowRemoveVisit Is Nothing Then
    '                Dim updateResult As Long = Me.UpdateCustomerClear(staffInfo.DlrCD, _
    '                                                                  staffInfo.BrnCD, _
    '                                                                  removeReserveNumberResult, _
    '                                                                  removeVisitNumber, _
    '                                                                  staffInfo, _
    '                                                                  nowDateTime)

    '                If Not ResultSuccess = updateResult Then
    '                    Me.Rollback = True
    '                    ''終了ログの出力
    '                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , updateResult))
    '                    Return updateResult
    '                End If
    '            End If

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'ORACLEのタイムアウトのみ処理
    '            Me.Rollback = True
    '            ''終了ログの出力
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                     , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                     , Me.GetType.ToString _
    '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                     , ResultTimeout))
    '            Return ResultTimeout
    '        Catch ex As Exception
    '            Me.Rollback = True
    '            ''エラーログの出力
    '            Logger.Error(ex.Message, ex)
    '            Throw
    '        Finally
    '            If blCommon IsNot Nothing Then
    '                blCommon.Dispose()
    '                blCommon = Nothing
    '            End If
    '        End Try
    '        Return ResultSuccess

    '    End Function

    '    ''' <summary>
    '    ''' 顧客解除処理 更新前チェック
    '    ''' </summary>
    '    ''' <param name="rowRemoveVisit">サービス来店管理情報</param>
    '    ''' <param name="dtRemoveROBaseInfo">R/O基本情報</param>
    '    ''' <param name="removeOrderStatus">整備受注No</param>
    '    ''' <param name="staffInfo">ログイン情報</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function CheckCustomerClearState(ByVal rowRemoveVisit As SC3140103ChangesServiceVisitManagementRow, _
    '                                             ByVal dtRemoveROBaseInfo As IC3801001OrderCommDataTable, _
    '                                             ByVal removeOrderStatus As String, _
    '                                             ByVal staffInfo As StaffContext) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        ' 付替え元来店情報ホールドチェック
    '        If Not rowRemoveVisit.IsASSIGNSTATUSNull Then
    '            If ASSIGN_STATUS_HOLD.Equals(rowRemoveVisit.ASSIGNSTATUS) Then
    '                Return ChangeResultBeforeHold
    '            End If
    '        End If
    '        ' 振当SAチェック
    '        If Not rowRemoveVisit.IsSACODENull Then
    '            If Not rowRemoveVisit.SACODE.Equals(staffInfo.Account) Then
    '                Return ChangeResultDifference
    '            End If
    '        End If

    '        ' R/O作成状態チェック
    '        If Not rowRemoveVisit.IsORDERNONull AndAlso _
    '            Not String.IsNullOrEmpty(rowRemoveVisit.ORDERNO) Then

    '            If dtRemoveROBaseInfo IsNot Nothing AndAlso _
    '                0 < dtRemoveROBaseInfo.Rows.Count Then

    '                If Not (C_RO_STATUS_RECEPTION.Equals(removeOrderStatus) Or _
    '                    C_RO_STATUS_ESTI_WAIT.Equals(removeOrderStatus)) Then
    '                    Return ChangeResultApproval
    '                End If
    '            Else
    '                Return ChangeResultApproval
    '            End If
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Return:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ChangeResultTrue))

    '        Return ChangeResultTrue
    '    End Function


    '    ''' <summary>
    '    ''' 入庫日時の取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="reserveId">予約ID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function GetRemoveStockDate(ByVal dealerCode As String, _
    '                                        ByVal storeCode As String, _
    '                                        ByVal reserveId As Long) As Date
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START))

    '        Dim dtRemoveReserve As SC3140103ChangesStallReserveDataTable
    '        Dim rowRemoveReserve As SC3140103ChangesStallReserveRow
    '        Dim removeStockDate As Date = Nothing

    '        Using da As New SC3140103DataTableAdapter
    '            dtRemoveReserve = da.GetStallReserveInfoForChangeData(dealerCode, _
    '                                                                  storeCode, _
    '                                                                  reserveId)
    '        End Using

    '        If dtRemoveReserve IsNot Nothing AndAlso 0 < dtRemoveReserve.Rows.Count Then
    '            '作業開始日時の昇順でソート
    '            rowRemoveReserve = _
    '                DirectCast(dtRemoveReserve.Select("", "STARTTIME ASC")(0), SC3140103ChangesStallReserveRow)
    '            If Not rowRemoveReserve Is Nothing Then
    '                If Not rowRemoveReserve.IsSTOCKTIMENull Then
    '                    removeStockDate = _
    '                        DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowRemoveReserve.STOCKTIME)
    '                End If
    '            End If
    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END))

    '        Return removeStockDate
    '    End Function

    '    ''' <summary>
    '    ''' 顧客解除処理 更新処理
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <param name="removeReserveNumberResult">予約ID</param>
    '    ''' <param name="removeVisitNumber">来店実績連番</param>
    '    ''' <param name="staffInfo">ログイン情報</param>
    '    ''' <param name="inNowDate">現在時刻</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <History>
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </History>
    '    Private Function UpdateCustomerClear(ByVal dealerCode As String, _
    '                                         ByVal storeCode As String, _
    '                                         ByVal removeReserveNumberResult As Long, _
    '                                         ByVal removeVisitNumber As Long, _
    '                                         ByVal staffInfo As StaffContext, _
    '                                         ByVal inNowDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:detailArea = {3}, storeCode = {4}," & _
    '                                   " removeReserveNumberResult = {5}, removeVisitNumber = {6}," & _
    '                                   " staffInfo = (StaffContext), inNowDate = {7} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , dealerCode _
    '                                 , storeCode _
    '                                 , removeReserveNumberResult _
    '                                 , removeVisitNumber _
    '                                 , inNowDate))

    '        Dim blCommon As New SMBCommonClassBusinessLogic

    '        Try
    '            If 0 < removeReserveNumberResult Then
    '                '入庫予定日時付替え処理

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                'サービス入庫行ロック処理
    '                Dim lockResult As Long = Me.ServiceInLock(removeReserveNumberResult, _
    '                                                          -1, _
    '                                                          blCommon, _
    '                                                          staffInfo, _
    '                                                          inNowDate)

    '                'ロック処理結果確認
    '                If Not ResultSuccess.Equals(lockResult) Then
    '                    'ロック失敗

    '                    Return lockResult
    '                End If

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                'Dim resultStockDateChange As Long = _
    '                '    blCommon.ChangeCarInDate(dealerCode, _
    '                '                            storeCode, _
    '                '                            removeReserveNumberResult, _
    '                '                            DEFAULT_LONG_VALUE, _
    '                '                            DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, DEFAULT_STOCKTIME_VALUE), _
    '                '                            staffInfo.Account, _
    '                '                            inNowDate)

    '                Dim resultStockDateChange As Long = _
    '                    blCommon.ChangeCarInDate(dealerCode, _
    '                                            storeCode, _
    '                                            removeReserveNumberResult, _
    '                                            DEFAULT_LONG_VALUE, _
    '                                            DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, DEFAULT_STOCKTIME_VALUE), _
    '                                            staffInfo.Account, _
    '                                            inNowDate, _
    '                                            MAINMENUID)

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                If Not ResultSuccess.Equals(resultStockDateChange) Then
    '                    Return resultStockDateChange
    '                End If

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                blCommon.RegisterStallReserveHis(dealerCode, _
    '                                                 storeCode, _
    '                                                 removeReserveNumberResult, _
    '                                                 inNowDate, _
    '                                                 RegisterType.RegisterServiceIn, _
    '                                                 staffInfo.Account, _
    '                                                 MAINMENUID, _
    '                                                 NoActivityId)

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '            End If

    '            'サービス来店顧客クリア
    '            Using da As New SC3140103DataTableAdapter
    '                Dim resultClearVisit As Long = da.VisitCustomerClear(removeVisitNumber, _
    '                                                                     inNowDate, _
    '                                                                     staffInfo.Account, _
    '                                                                     MAINMENUID)
    '                If resultClearVisit <> 1 Then
    '                    Return ResultDBError
    '                End If
    '            End Using

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            'If 0 < removeReserveNumberResult Then
    '            '    '付替え先予約履歴の作成
    '            '    Dim resultRemoveReserveHistory As Long = _
    '            '        blCommon.RegisterStallReserveHis(dealerCode, _
    '            '                                         storeCode, _
    '            '                                         removeReserveNumberResult, _
    '            '                                         inNowDate, _
    '            '                                         SMBCommonClassBusinessLogic.RegisterType.ReserveHisAll)
    '            '    If Not ResultSuccess.Equals(resultRemoveReserveHistory) Then
    '            '        Return resultRemoveReserveHistory
    '            '    End If
    '            'End If

    '            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        Finally
    '            If blCommon IsNot Nothing Then
    '                blCommon.Dispose()
    '                blCommon = Nothing
    '            End If
    '        End Try

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ResultSuccess))
    '        Return ResultSuccess
    '    End Function
    '#End Region
    '    '2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
    '    ''' <summary>
    '    ''' 削除処理
    '    ''' </summary>
    '    ''' <param name="inVisitSequence">来店実績連番</param>
    '    ''' <param name="inNowDate">現在日時</param>
    '    ''' <param name="inAccountInfo">ユーザー情報</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    ''' <history>
    '    ''' 2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
    '    ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    '    ''' </history>
    '    <EnableCommit()>
    '    Public Function SetReceptDelete(ByVal inVisitSequence As Long _
    '                                  , ByVal inNowDate As DateTime _
    '                                  , ByVal inAccountInfo As StaffContext) As Long
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} START inVisitSequence:{2} inNowDate:{3}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
    '                  , inNowDate.ToString(CultureInfo.CurrentCulture)))

    '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '        'Dim returnCode As Integer = 0
    '        Dim returnCode As Long = 0
    '        '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
    '        Dim da As New SC3140103DataTableAdapter
    '        Dim commonClass As New SMBCommonClassBusinessLogic
    '        Try
    '            Dim reserveId As Long = -1

    '            Dim lockVersion As Long = -1

    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '            Dim orderNo As String = String.Empty
    '            Dim commonReturnCode As Long = 0
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
    '            '削除処理実行
    '            Dim updateCount As Integer = _
    '                da.UpdateVisitSequence(inVisitSequence, _
    '                                       inNowDate, _
    '                                       inAccountInfo.Account, _
    '                                       MAINMENUID)
    '            '正常更新できなかった場合
    '            If updateCount <> 1 Then
    '                returnCode = C_RET_DBERROR
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} SC3140103DataTableAdapter.UpdateVisitSequence ERROR:{2}" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                           , returnCode.ToString(CultureInfo.CurrentCulture)))
    '            Else
    '                '来店実績情報取得
    '                Dim dtServiceVisitManagement As SC3140103ServiceVisitManagementDataTable = _
    '                    da.GetVisitManagement(inVisitSequence)
    '                '予約IDを来店実績管理テーブルから取得する
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '                If Not IsNothing(dtServiceVisitManagement) AndAlso 0 < dtServiceVisitManagement.Count Then

    '                    If Not dtServiceVisitManagement.Item(0).IsREZIDNull Then
    '                        reserveId = dtServiceVisitManagement.Item(0).REZID
    '                    End If

    '                    If Not dtServiceVisitManagement.Item(0).IsORDERNONull Then
    '                        orderNo = dtServiceVisitManagement.Item(0).ORDERNO
    '                    End If

    '                Else
    '                    '取得できなかった場合はエラー
    '                    returnCode = C_RET_NOMATCH
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} SC3140103DataTableAdapter.GetVisitManagement ERROR:{2}" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                               , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                End If
    '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
    '            End If

    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '            '整備受注Noがある場合は本予約から仮予約にする
    '            If returnCode = 0 AndAlso Not String.IsNullOrEmpty(orderNo) Then

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                'commonReturnCode = commonClass.ChangeSACode(inAccountInfo.DlrCD, _
    '                '                                            inAccountInfo.BrnCD, _
    '                '                                            SMBCommonClassBusinessLogic.NoReserveId, _
    '                '                                            orderNo, _
    '                '                                            inAccountInfo.Account, _
    '                '                                            "1", _
    '                '                                            inNowDate, _
    '                '                                            inAccountInfo.Account, _
    '                '                                            inNowDate)

    '                commonReturnCode = commonClass.ChangeSACode(inAccountInfo.DlrCD, _
    '                                                            inAccountInfo.BrnCD, _
    '                                                            NoReserveId, _
    '                                                            orderNo, _
    '                                                            inAccountInfo.Account, _
    '                                                            "1", _
    '                                                            inNowDate, _
    '                                                            inAccountInfo.Account, _
    '                                                            inNowDate, _
    '                                                            MAINMENUID)

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                '正常終了できなかった場合はエラー
    '                If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '                    returnCode = commonReturnCode
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} SMBCommonClassBusinessLogic.ChangeSACode ERROR:{2}" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                               , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                End If
    '            End If
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END

    '            '予約が有る場合は入庫情報更新
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
    '            'If reserveId > 0 Then
    '            '    '共通関数
    '            '    '入庫日時付替え
    '            '    Dim commonReturnCode As Long = commonClass.ChangeCarInDate(inAccountInfo.DlrCD, _
    '            '                                                               inAccountInfo.BrnCD, _
    '            '                                                               reserveId, _
    '            '                                                               SMBCommonClassBusinessLogic.NoReserveId, _
    '            '                                                               Date.MinValue, _
    '            '                                                               inAccountInfo.Account, _
    '            '                                                               inNowDate)
    '            '    'リターンコードチェック
    '            '    If commonReturnCode = CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '            '        '予約履歴作成
    '            '        commonReturnCode = _
    '            '            commonClass.RegisterStallReserveHis(inAccountInfo.DlrCD, _
    '            '                                                inAccountInfo.BrnCD, _
    '            '                                                reserveId, _
    '            '                                                inNowDate, _
    '            '                                                SMBCommonClassBusinessLogic.RegisterType.ReserveHisIndividual)
    '            '        If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '            '            returnCode = C_RET_DBERROR
    '            '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '            '                       , "{0}.{1} SMBCommonClassBusinessLogic.RegisterStallReserveHis ERROR:{2}" _
    '            '                       , Me.GetType.ToString _
    '            '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '            '                       , returnCode.ToString(CultureInfo.CurrentCulture)))
    '            '        End If
    '            '    Else
    '            '        returnCode = C_RET_DBERROR
    '            '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '            '                   , "{0}.{1} SMBCommonClassBusinessLogic.ChangeCarInDate ERROR:{2}" _
    '            '                   , Me.GetType.ToString _
    '            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '            '                   , returnCode.ToString(CultureInfo.CurrentCulture)))
    '            '    End If
    '            'End If
    '            '予約が有る場合は入庫情報更新
    '            If returnCode = 0 AndAlso reserveId > 0 Then
    '                '共通関数
    '                '入庫日時付替え

    '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '                '予約IDが取得できた場合
    '                'ストール予約の最新情報の取得(行ロックバージョン)
    '                Dim dtNewReserve As SC3140103NewestStallRezInfoDataTable = da.GetDBNewestStallRezInfo(reserveId)

    '                '予約情報に取得確認
    '                If 0 < dtNewReserve.Count Then
    '                    '予約が取得できた場合
    '                    lockVersion = dtNewReserve.Item(0).ROW_LOCK_VERSION

    '                End If


    '                'サービス入庫行ロック処理
    '                commonReturnCode = commonClass.LockServiceInTable(reserveId, _
    '                                                                  lockVersion, _
    '                                                                  "0", _
    '                                                                  inAccountInfo.Account, _
    '                                                                  inNowDate, _
    '                                                                  MAINMENUID)

    '                '行ロック確認
    '                If commonReturnCode = CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '                    '行ロック成功

    '                    'commonReturnCode = commonClass.ChangeCarInDate(inAccountInfo.DlrCD, _
    '                    '                                               inAccountInfo.BrnCD, _
    '                    '                                               reserveId, _
    '                    '                                               SMBCommonClassBusinessLogic.NoReserveId, _
    '                    '                                               Date.MinValue, _
    '                    '                                               inAccountInfo.Account, _
    '                    '                                               inNowDate)

    '                    commonReturnCode = commonClass.ChangeCarInDate(inAccountInfo.DlrCD, _
    '                                                                   inAccountInfo.BrnCD, _
    '                                                                   reserveId, _
    '                                                                   NoReserveId, _
    '                                                                   Date.MinValue, _
    '                                                                   inAccountInfo.Account, _
    '                                                                   inNowDate, _
    '                                                                   MAINMENUID)


    '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                    '正常終了できなかった場合はエラー
    '                    If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '                        returnCode = commonReturnCode
    '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                   , "{0}.{1} SMBCommonClassBusinessLogic.ChangeCarInDate ERROR:{2}" _
    '                                   , Me.GetType.ToString _
    '                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                   , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                    Else
    '                        '予約履歴作成

    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                        'commonReturnCode = _
    '                        '    commonClass.RegisterStallReserveHis(inAccountInfo.DlrCD, _
    '                        '                                        inAccountInfo.BrnCD, _
    '                        '                                        reserveId, _
    '                        '                                        inNowDate, _
    '                        '                                        SMBCommonClassBusinessLogic.RegisterType.ReserveHisIndividual)

    '                        commonReturnCode = _
    '                            commonClass.RegisterStallReserveHis(inAccountInfo.DlrCD, _
    '                                                                inAccountInfo.BrnCD, _
    '                                                                reserveId, _
    '                                                                inNowDate, _
    '                                                                RegisterType.RegisterServiceIn,
    '                                                                inAccountInfo.Account, _
    '                                                                MAINMENUID, _
    '                                                                NoActivityId)


    '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '                        If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
    '                            returnCode = C_RET_DBERROR
    '                            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                       , "{0}.{1} SMBCommonClassBusinessLogic.RegisterStallReserveHis ERROR:{2}" _
    '                                       , Me.GetType.ToString _
    '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                       , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                        End If
    '                    End If

    '                Else
    '                    '行ロックエラー
    '                    returnCode = commonReturnCode
    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                               , "{0}.{1} SMBCommonClassBusinessLogic.LockServiceInTable ERROR:{2}" _
    '                               , Me.GetType.ToString _
    '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                               , returnCode.ToString(CultureInfo.CurrentCulture)))

    '                End If

    '            End If
    '            '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
    '            '正常終了していない場合はロールバックする
    '            If returnCode <> 0 Then
    '                Me.Rollback = True
    '            End If
    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            ''ORACLEのタイムアウトのみ処理
    '            Me.Rollback = True
    '            returnCode = C_RET_DBTIMEOUT
    '            ''終了ログの出力
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END RETURNCODE:{2}" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , returnCode.ToString(CultureInfo.CurrentCulture)))
    '            Return returnCode
    '        Finally
    '            da.Dispose()
    '            commonClass.Dispose()
    '        End Try
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                  , "{0}.{1} END RETURNCODE:{2}" _
    '                  , Me.GetType.ToString _
    '                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                  , returnCode.ToString(CultureInfo.CurrentCulture)))
    '        Return returnCode
    '    End Function
    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

    '    ' 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START
    '    ''' <summary>
    '    ''' 予約情報ポップアップ用の一覧データ取得
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="branchCode">店舗コード</param>
    '    ''' <param name="customerCode">顧客コード</param>
    '    ''' <param name="registerNo">車両登録No.</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <param name="baseDate">取得基準日</param>
    '    ''' <returns>予約情報ポップアップ用データセット</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetPopupReservationList(ByVal dealerCode As String, _
    '                                            ByVal branchCode As String, _
    '                                            ByVal customerCode As String, _
    '                                            ByVal registerNo As String, _
    '                                            ByVal vin As String, _
    '                                            ByVal baseDate As String) As SC3140103ReserveListDataTable

    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, customerCode = {5}, registerNo = {6}, vin = {7}, baseDate = {8}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_START _
    '                                , dealerCode _
    '                                , branchCode _
    '                                , customerCode _
    '                                , registerNo _
    '                                , vin _
    '                                , baseDate))

    '        '戻り値データ
    '        Dim dtRet As SC3140103ReserveListDataTable
    '        Dim bl As New IC3811501BusinessLogic

    '        Try
    '            '取得基準日以降の予約情報を取得（※取得基準日を含む）
    '            Dim dt As IC3811501DataSet.IC3811501ReservationListDataTable = _
    '                bl.GetReservationList(dealerCode, _
    '                                      branchCode, _
    '                                      customerCode, _
    '                                      registerNo, _
    '                                      vin, _
    '                                      baseDate)

    '            '予約情報が存在しない場合は処理終了（予約情報ポップアップは出力しない）
    '            If IsNothing(dt) Then
    '                Return Nothing
    '            End If
    '            If dt.Rows.Count = 0 Then
    '                Return Nothing
    '            End If

    '            '文言の取得
    '            Dim hyphenWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdHyphen)                '-
    '            Dim unCreatedWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdUnCreated)          '未作成
    '            Dim underCreationWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdUnderCreation)  '作成中

    '            '予約情報が存在する場合、戻り値データ用に編集してDataTableを返却する
    '            Dim dtMod As New SC3140103ReserveListDataTable
    '            For Each row As IC3811501DataSet.IC3811501ReservationListRow In dt
    '                '予約開始日時
    '                Dim reserveFrom As String
    '                Dim fromMD As String
    '                Dim fromHM As String
    '                fromMD = DateTimeFunc.FormatDate(CONVERTDATE_MD, row.REZSTARTTIME)      'MM/dd
    '                fromHM = DateTimeFunc.FormatDate(CONVERTDATE_HM, row.REZSTARTTIME)      'hh:mm
    '                reserveFrom = fromMD & " " & fromHM                                     'MM/dd hh:mm

    '                '予約終了日時
    '                Dim reserveTo As String
    '                Dim toMD As String
    '                Dim toHM As String
    '                toMD = DateTimeFunc.FormatDate(CONVERTDATE_MD, row.REZENDTIME)          'MM/dd
    '                toHM = DateTimeFunc.FormatDate(CONVERTDATE_HM, row.REZENDTIME)          'hh:mm
    '                '予約開始日と予約終了日が同じ場合（ = 予約が１日で終了する場合）
    '                If fromMD.Equals(toMD) Then
    '                    reserveTo = toHM                                                    'hh:mm
    '                Else
    '                    '予約開始日と予約終了日が違う場合（ = 予約が複数日に跨る場合）
    '                    reserveTo = toMD & " " & toHM                                       'MM/dd hh:mm
    '                End If

    '                Dim dr As SC3140103ReserveListRow = dtMod.NewSC3140103ReserveListRow
    '                With dr
    '                    '予約開始日時 - 予約終了日時
    '                    .RESERVEFROMTO = reserveFrom & " " & hyphenWord & " " & reserveTo
    '                    'サービス名称
    '                    .SERVICENAME = row.SERVICENAME
    '                    'ROの作成ステータス（ROステータスが"0: 未作成"の場合　→　『未作成』とする）
    '                    If row.ROSTATUSCODE.Equals("0") Then
    '                        .ROSTATUS = unCreatedWord
    '                    Else
    '                        'ROの作成ステータス（ROステータスが"1: 作成済（未作成以外）"の場合　→　『作成中』とする）
    '                        .ROSTATUS = underCreationWord
    '                    End If
    '                End With

    '                '行追加
    '                dtMod.Rows.Add(dr)
    '            Next

    '            dtRet = dtMod

    '            '終了ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "{0}.{1} {2}" _
    '                                    , Me.GetType.ToString _
    '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                    , LOG_END))
    '            Return dtRet
    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'ORACLEのタイムアウトのみ処理
    '            ''終了ログの出力
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                     , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                     , Me.GetType.ToString _
    '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                     , ResultTimeout))
    '            Return Nothing
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl.Dispose()
    '                bl = Nothing
    '            End If
    '        End Try
    '    End Function
    '    ' 2012/11/29 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END

    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    '#Region "お客様呼び出し処理"
    '    ''' <summary>
    '    ''' お客様呼び出し処理
    '    ''' </summary>
    '    ''' <param name="inVisitSequence">来店者SEQ</param>
    '    ''' <param name="inAccount">担当SA</param>
    '    ''' <param name="inNowdate">現在日時</param>
    '    ''' <param name="inUpdateDate">更新日時</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    <EnableCommit()>
    '    Public Function CallVisit(ByVal inVisitSequence As Long, _
    '                                     ByVal inAccount As String, _
    '                                     ByVal inNowdate As Date, _
    '                                     ByVal inUpdateDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccount:{4} inNowdate:{5} inUpdateDate:{6}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , inVisitSequence _
    '                                 , inAccount _
    '                                 , inNowdate.ToString(CultureInfo.CurrentCulture) _
    '                                 , inUpdateDate))
    '        Dim returnCode As Integer = 0
    '        '呼出ステータス更新
    '        Using da As New SC3140103DataTableAdapter
    '            Try
    '                Dim resultCallVisit As Long = da.UpdateVisitStausCall(inVisitSequence, _
    '                                                                     inUpdateDate, _
    '                                                                     inNowdate, _
    '                                                                     inAccount, _
    '                                                                     MAINMENUID)
    '                If resultCallVisit <> 1 Then
    '                    Return ResultDBExclusion
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                ''ORACLEのタイムアウトのみ処理
    '                Me.Rollback = True
    '                returnCode = ResultTimeout
    '                ''終了ログの出力
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} END RETURNCODE:{2}" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                           , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                Return returnCode
    '            Catch ex As OracleExceptionEx
    '                Me.Rollback = True
    '                returnCode = ResultDBError
    '                ''エラーログの出力
    '                Logger.Error(ex.Message, ex)
    '                Return returnCode
    '            End Try
    '        End Using
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ResultSuccess))
    '        Return ResultSuccess
    '    End Function
    '#End Region

    '#Region "お客様呼び出しキャンセル処理"
    '    ''' <summary>
    '    ''' お客様呼び出しキャンセル処理
    '    ''' </summary>
    '    ''' <param name="inVisitSequence">来店者SEQ</param>
    '    ''' <param name="inAccount">担当SA</param>
    '    ''' <param name="inNowdate">現在日時</param>
    '    ''' <param name="inUpdateDate">更新日時</param>
    '    ''' <returns>returnCode</returns>
    '    ''' <remarks></remarks>
    '    <EnableCommit()>
    '    Public Function CallCancelVisit(ByVal inVisitSequence As Long, _
    '                                     ByVal inAccount As String, _
    '                                     ByVal inNowdate As Date, _
    '                                     ByVal inUpdateDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccoun:{4} inNowdate:{5} inUpdateDate:{6} " _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , inVisitSequence _
    '                                 , inAccount _
    '                                 , inNowdate.ToString(CultureInfo.CurrentCulture) _
    '                                 , inUpdateDate))
    '        Dim returnCode As Integer = 0
    '        '呼出ステータス更新
    '        Using da As New SC3140103DataTableAdapter
    '            Try
    '                Dim resultCallVisit As Long = da.UpdateVisitStausCallCancel(inVisitSequence, _
    '                                                                     inUpdateDate, _
    '                                                                     inNowdate, _
    '                                                                     inAccount, _
    '                                                                     MAINMENUID)
    '                If resultCallVisit <> 1 Then
    '                    Return ResultDBExclusion
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                ''ORACLEのタイムアウトのみ処理
    '                Me.Rollback = True
    '                returnCode = ResultTimeout
    '                ''終了ログの出力
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} END RETURNCODE:{2}" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                           , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                Return returnCode
    '            Catch ex As OracleExceptionEx
    '                Me.Rollback = True
    '                returnCode = ResultDBError
    '                ''エラーログの出力
    '                Logger.Error(ex.Message, ex)
    '                Return returnCode
    '            End Try
    '        End Using
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ResultSuccess))
    '        Return ResultSuccess
    '    End Function
    '#End Region

    '#Region "呼び出し場所更新処理"
    '    ''' <summary>
    '    ''' 呼び出し場所更新処理
    '    ''' </summary>
    '    ''' <param name="inVisitSequence">来店者SEQ</param>
    '    ''' <param name="inCallPlace">呼出場所</param>
    '    ''' <param name="inAccount">担当SA</param>
    '    ''' <param name="inNowdate">現在日時</param>
    '    ''' <param name="inUpdateDate">更新日時</param>
    '    ''' <returns>returnCode</returns>
    '    ''' <remarks></remarks>
    '    <EnableCommit()>
    '    Public Function CallPlaceChange(ByVal inVisitSequence As Long, _
    '                                 ByVal inCallPlace As String, _
    '                                 ByVal inAccount As String, _
    '                                 ByVal inNowdate As Date, _
    '                                 ByVal inUpdateDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inCallPlace:{4} inAccount:{5} " & _
    '                                   " inNowdate:{6} inUpdateDate = {7}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , inVisitSequence _
    '                                 , inCallPlace _
    '                                 , inAccount _
    '                                 , inNowdate.ToString(CultureInfo.CurrentCulture) _
    '                                 , inUpdateDate))
    '        Dim returnCode As Integer = 0
    '        '呼出ステータス更新
    '        Using da As New SC3140103DataTableAdapter
    '            Try
    '                Dim resultCallVisit As Long = da.UpdateCallPlace(inVisitSequence, _
    '                                                                     inCallPlace, _
    '                                                                     inUpdateDate, _
    '                                                                     inNowdate, _
    '                                                                     inAccount, _
    '                                                                     MAINMENUID)
    '                If resultCallVisit <> 1 Then
    '                    Return ResultDBExclusion
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                ''ORACLEのタイムアウトのみ処理
    '                Me.Rollback = True
    '                returnCode = ResultTimeout
    '                ''終了ログの出力
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} END RETURNCODE:{2}" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                           , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                Return returnCode
    '            Catch ex As OracleExceptionEx
    '                Me.Rollback = True
    '                returnCode = ResultDBError
    '                ''エラーログの出力
    '                Logger.Error(ex.Message, ex)
    '                Return returnCode
    '            End Try
    '        End Using
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ResultSuccess))
    '        Return ResultSuccess
    '    End Function
    '#End Region

    '#Region "呼出完了更新"
    '    ''' <summary>
    '    ''' 呼出完了更新
    '    ''' </summary>
    '    ''' <param name="inVisitSequence">来店者SEQ</param>
    '    ''' <param name="inAccount">担当SA</param>
    '    ''' <param name="inNowdate">現在日時</param>
    '    ''' <param name="inUpdateDate">更新日時</param>
    '    ''' <returns>returnCode</returns>
    '    ''' <remarks></remarks>
    '    <EnableCommit()>
    '    Public Function CallCompleted(ByVal inVisitSequence As Long, _
    '                                 ByVal inAccount As String, _
    '                                 ByVal inNowdate As Date, _
    '                                 ByVal inUpdateDate As Date) As Long
    '        '開始ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccount:{4} " & _
    '                                   " inNowdate:{5} inUpdateDate:{6}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , LOG_START _
    '                                 , inVisitSequence _
    '                                 , inAccount _
    '                                 , inNowdate.ToString(CultureInfo.CurrentCulture) _
    '                                 , inUpdateDate))
    '        Dim returnCode As Integer = 0
    '        '呼出ステータス更新
    '        Using da As New SC3140103DataTableAdapter
    '            Try
    '                Dim resultCallVisit As Long = da.UpdateCallCompleted(inVisitSequence, _
    '                                                                     inUpdateDate, _
    '                                                                     inNowdate, _
    '                                                                     inAccount, _
    '                                                                     MAINMENUID)
    '                If resultCallVisit <> 1 Then
    '                    Return ResultDBExclusion
    '                End If
    '            Catch ex As OracleExceptionEx When ex.Number = 1013
    '                ''ORACLEのタイムアウトのみ処理
    '                Me.Rollback = True
    '                returnCode = ResultTimeout
    '                ''終了ログの出力
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                           , "{0}.{1} END RETURNCODE:{2}" _
    '                           , Me.GetType.ToString _
    '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                           , returnCode.ToString(CultureInfo.CurrentCulture)))
    '                Return returnCode
    '            Catch ex As OracleExceptionEx
    '                Me.Rollback = True
    '                returnCode = ResultDBError
    '                ''エラーログの出力
    '                Logger.Error(ex.Message, ex)
    '                Return returnCode
    '            End Try
    '        End Using
    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} Retrun:{3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , LOG_END _
    '                   , ResultSuccess))
    '        Return ResultSuccess
    '    End Function
    '#End Region

    '#Region "Push送信お客様呼出"
    '    ''' <summary>
    '    ''' Push送信お客様呼出
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <remarks></remarks>
    '    Public Sub SendPushForCall(ByVal dealerCode As String, ByVal storeCode As String)
    '        ' Logger.Debug("SendPushToPc_Start Pram[" & dealerCode & "," & storeCode & "]")

    '        'スタッフ情報の取得(SSV)
    '        Dim stuffCodeList As New List(Of Decimal)
    '        stuffCodeList.Add(OperationCodeRwm)

    '        'オンラインユーザー情報の取得
    '        Dim utility As New VisitUtilityBusinessLogic
    '        Dim sendPushUsers As VisitUtilityUsersDataTable = _
    '            utility.GetOnlineUsers(dealerCode, storeCode, stuffCodeList)
    '        utility = Nothing

    '        '来店通知命令の送信
    '        For Each userRow As VisitUtilityUsersRow In sendPushUsers

    '            '送信処理
    '            TransmissionForCall(userRow.ACCOUNT)
    '        Next
    '        ' Logger.Debug("SendPushToPc_End")
    '    End Sub

    '#End Region

    '#Region "送信お客様呼出"
    '    ''' <summary>
    '    ''' 送信お客様呼出（受付待ちモニター画面再描画）
    '    ''' </summary>
    '    ''' <param name="staffCode">スタッフコード</param>
    '    ''' <remarks></remarks>
    '    Private Sub TransmissionForCall(ByVal staffCode As String)
    '        ' Logger.Debug("SendPushSsvUpdate_Start Pram[" & staffCode & "]")

    '        'POST送信メッセージの作成
    '        Dim postSendMessage As New StringBuilder
    '        postSendMessage.Append("cat=action")
    '        postSendMessage.Append("&type=main")
    '        postSendMessage.Append("&sub=js")
    '        postSendMessage.Append("&uid=" & staffCode)
    '        postSendMessage.Append("&time=0")
    '        postSendMessage.Append("&js1=addCallee()")

    '        '送信処理
    '        Dim visitUtility As New VisitUtility
    '        visitUtility.SendPushPC(postSendMessage.ToString)

    '        ' Logger.Debug("SendPushSsvUpdate_End]")
    '    End Sub
    '#End Region

    '#Region "Push送信呼出キャンセル"
    '    ''' <summary>
    '    ''' Push送信呼出キャンセル
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="storeCode">店舗コード</param>
    '    ''' <remarks></remarks>
    '    Public Sub SendPushForCallCancel(ByVal dealerCode As String, ByVal storeCode As String)
    '        ' Logger.Debug("SendPushToPc_Start Pram[" & dealerCode & "," & storeCode & "]")

    '        'スタッフ情報の取得(SSV)
    '        Dim stuffCodeList As New List(Of Decimal)
    '        stuffCodeList.Add(OperationCodeRwm)

    '        'オンラインユーザー情報の取得
    '        Dim utility As New VisitUtilityBusinessLogic
    '        Dim sendPushUsers As VisitUtilityUsersDataTable = _
    '            utility.GetOnlineUsers(dealerCode, storeCode, stuffCodeList)
    '        utility = Nothing

    '        '来店通知命令の送信
    '        For Each userRow As VisitUtilityUsersRow In sendPushUsers

    '            '送信処理
    '            TransmissionCallCancel(userRow.ACCOUNT)
    '        Next
    '        ' Logger.Debug("SendPushToPc_End")
    '    End Sub

    '#End Region

    '#Region "送信呼出キャンセル"
    '    ''' <summary>
    '    ''' 送信呼出キャンセル（受付待ちモニター画面再描画）
    '    ''' </summary>
    '    ''' <param name="staffCode">スタッフコード</param>
    '    ''' <remarks></remarks>
    '    Private Sub TransmissionCallCancel(ByVal staffCode As String)
    '        ' Logger.Debug("SendPushSsvUpdate_Start Pram[" & staffCode & "]")

    '        'POST送信メッセージの作成
    '        Dim postSendMessage As New StringBuilder
    '        postSendMessage.Append("cat=action")
    '        postSendMessage.Append("&type=main")
    '        postSendMessage.Append("&sub=js")
    '        postSendMessage.Append("&uid=" & staffCode)
    '        postSendMessage.Append("&time=0")
    '        postSendMessage.Append("&js1=delCallee()")

    '        '送信処理
    '        Dim visitUtility As New VisitUtility
    '        visitUtility.SendPushPC(postSendMessage.ToString)

    '        ' Logger.Debug("SendPushSsvUpdate_End]")
    '    End Sub
    '#End Region
    '    '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#Region "初期表示"

#Region "Public"

    ''' <summary>
    ''' 振当待ちエリアチップ情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>振当待ち情報データセット</returns>
    Public Function GetAssignmentInfo(ByVal inDealerCode As String _
                                    , ByVal inBranchCode As String _
                                    , ByVal inPresentTime As Date) _
                                      As SC3140103VisitManagementInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode))


        Using da As New SC3140103DataTableAdapter

            '振当待ちエリアチップ情報取得
            Dim dtVisitInfo As SC3140103VisitManagementInfoDataTable = _
                da.GetAssignmentChipInfo(inDealerCode, inBranchCode, inPresentTime)


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dtVisitInfo.Rows.Count))

            '処理結果返却
            Return dtVisitInfo

        End Using

    End Function


    ''' <summary>
    ''' 受付エリアチップ情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inAccount">ログインSAアカウント</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>受付情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetReceptionInfo(ByVal inDealerCode As String _
                                   , ByVal inBranchCode As String _
                                   , ByVal inAccount As String _
                                   , ByVal inPresentTime As Date) _
                                     As SC3140103VisitManagementInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode, inAccount))

        Using da As New SC3140103DataTableAdapter

            '受付待ちエリア情報取得
            Dim dtVisitInfo As SC3140103VisitManagementInfoDataTable = _
                da.GetReceptionChipInfo(inDealerCode, inBranchCode, inAccount, inPresentTime)


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dtVisitInfo.Rows.Count))

            '処理結果返却
            Return dtVisitInfo

        End Using

    End Function


    ''' <summary>
    ''' 追加作業・作業中・納車準備・納車作業エリアチップ情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inAccount">ログインSAアカウント</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <remarks>>MainChip情報データセット</remarks>
    ''' <history>
    ''' </history>
    Public Function GetMainChipInfo(ByVal inDealerCode As String _
                                  , ByVal inBranchCode As String _
                                  , ByVal inAccount As String _
                                  , ByVal inPresentTime As Date) _
                                    As SC3140103MainChipInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode, inAccount))


        Using sc3140103Dac As New SC3140103DataTableAdapter

            '作業中・納車準備・納車作業エリアチップ情報取得
            Dim dtMainChipInfo As SC3140103MainChipInfoDataTable = sc3140103Dac.GetMainChipInfo(inDealerCode, _
                                                                                                inBranchCode, _
                                                                                                inAccount)

            '作業中・納車準備・納車作業エリアチップ情報取得確認
            If 0 < dtMainChipInfo.Count Then
                '作業中・納車準備・納車作業エリアチップ情報がある場合
                '表示用チップ情報を作成する

                '追加作業エリアチップ情報取得
                Dim dtAddApprovalChipInfo As SC3140103AddApprovalChipInfoDataTable = _
                    sc3140103Dac.GetAddApprovalChipInfo(inDealerCode, _
                                                        inBranchCode, _
                                                        dtMainChipInfo)

                '表示用MainChip情報作成
                dtMainChipInfo = Me.CreateMainChipInfo(inDealerCode, _
                                                       inBranchCode, _
                                                       inAccount, _
                                                       inPresentTime, _
                                                       dtMainChipInfo, _
                                                       dtAddApprovalChipInfo, _
                                                       sc3140103Dac)

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dtMainChipInfo.Rows.Count))

            Return dtMainChipInfo


        End Using

    End Function


    ''' <summary>
    ''' 事前準備表示件数取得
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>List(0:todayCount, 1:nextCount)</returns>
    ''' <remarks></remarks>
    Public Function GetAdvancePreparationsCount(ByVal inStaffInfo As StaffContext _
                                              , ByVal inPresentTime As Date) As List(Of Long)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'YYYYMMDDに変換
        Dim nowDateString As String = DateTimeFunc.FormatDate(9, inPresentTime)

        '翌稼働日取得
        Dim nextDateString As String

        'SMBCommonClassインスタンス
        Using commonClass As New SMBCommonClassBusinessLogic

            ''SMBCommonClass初期処理
            commonClass.InitCommon(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inPresentTime)

            '翌稼働日取得
            Dim nextDate As Date = commonClass.GetWorkingDays(inPresentTime, 1)

            '翌稼働日(YYYYMMDDに変換)
            nextDateString = DateTimeFunc.FormatDate(9, nextDate)

        End Using


        '事前準備情報用DataTable
        Dim dt As SC3140103AdvancePreparationsDataTable

        'SC3140103DataTableAdapterインスタンス
        Using da As New SC3140103DataTableAdapter

            '事前準備情報取得
            dt = da.GetAdvancePreparationsChipData(inStaffInfo.DlrCD, _
                                                   inStaffInfo.BrnCD, _
                                                   nowDateString, _
                                                   nextDateString, _
                                                   inStaffInfo.Account)

        End Using

        'RO番号リスト
        Dim orderList As New List(Of String)
        '事前準備情報分ループ
        For Each row As SC3140103AdvancePreparationsRow In dt.Rows

            'RO番号チェック
            If Not String.IsNullOrEmpty(row.ORDERNO) Then
                'RO番号が存在する場合

                '事前準備情報中の整備受注NoがNULL以外のものをリストに詰める
                orderList.Add(row.ORDERNO)

            End If
        Next


        '整備受注Noのリスト有無で処理を分岐
        If 0 < orderList.Count Then
            'RO番号リストに値が存在する

            '今まではBMTSのAPIから情報を取得していたが
            'GL版では使用しない
            '今後事前準備の機能開発時に仕様が決まる

        Else
            'RO番号リストに値がない

            '事前準備情報分ループ
            For Each row As SC3140103AdvancePreparationsRow In dt.Rows

                '全ての情報に
                'SA事前準備フラグは初期値に未完了("1")を設定
                row.SASTATUSFLG = PreparationMiddle

            Next
        End If

        '当日分の件数
        Dim todayCount As Long = 0

        ''翌営業日分の件数
        Dim nextCount As Long = 0

        '当日、および翌日の件数を取得
        '事前準備情報分ループ
        For Each row As SC3140103AdvancePreparationsRow In dt.Rows

            'SA事前準備フラグが未完了のもの（整備受注Noがないものも含まれる）を件数にカウントする
            'SA事前準備フラグはチェック
            If PreparationMiddle.Equals(row.SASTATUSFLG) Then
                '事前準備

                '当日分か翌日分かチェック
                If (row.TODAYFLG.Equals("1")) Then
                    '当日分

                    '当日件数加算
                    todayCount += 1
                Else
                    '翌営業日分

                    '翌日件数加算
                    nextCount += 1
                End If
            End If
        Next

        '結果格納用
        Dim resultList As New List(Of Long)

        '当日分の件数を設定
        resultList.Add(todayCount)
        '翌営業日分の件数を設定
        resultList.Add(nextCount)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} Result:todayCount={3}, nextCount={4}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_END _
                  , todayCount _
                  , nextCount))

        Return resultList

    End Function


#End Region

#Region "Private"

    ''' <summary>
    ''' 表示用MainChip情報作成処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inAccount">ログインSAアカウント</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <param name="inDtMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
    ''' <param name="inDtAddApprovalChipInfo">追加作業エリアチップ情報</param>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <remarks>>MainChip情報データセット</remarks>
    ''' <history>
    ''' </history>
    Private Function CreateMainChipInfo(ByVal inDealerCode As String _
                                      , ByVal inBranchCode As String _
                                      , ByVal inAccount As String _
                                      , ByVal inPresentTime As Date _
                                      , ByVal inDtMainChipInfo As SC3140103MainChipInfoDataTable _
                                      , ByVal inDtAddApprovalChipInfo As SC3140103AddApprovalChipInfoDataTable _
                                      , ByVal inSC3140103Dac As SC3140103DataTableAdapter) _
                                        As SC3140103MainChipInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} DTMAINCHIPINFO = {6} DTADDAPPROVALINFO = {7}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode, inAccount, inDtMainChipInfo.Count, inDtAddApprovalChipInfo.Count))

        '作業中・納車準備・納車作業エリアチップ情報分ループ
        For Each rowMainChipInfo In inDtMainChipInfo

            '追加作業が存在するかSELECT
            Dim rowAddApprovalChipInfo As SC3140103AddApprovalChipInfoRow() = _
                DirectCast(inDtAddApprovalChipInfo.Select(String.Format(CultureInfo.CurrentCulture, _
                                                         "VISIT_ID = '{0}'", _
                                                         rowMainChipInfo.VISIT_ID)), SC3140103AddApprovalChipInfoRow())

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} VISITSEQ = {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , rowMainChipInfo.VISIT_ID))

            '追加作業か存在するか確認
            If 0 < rowAddApprovalChipInfo.Count Then
                '追加作業か存在する場合

                '追加作業表示フラグを設定
                rowMainChipInfo.ADDAPPROVAL_FLG = C_APPROVAL_STATUS_ON

                'RO作業連番を設定
                rowMainChipInfo.RO_SEQ = rowAddApprovalChipInfo(0).RO_SEQ

                'FM承認日時を設定
                rowMainChipInfo.RO_CHECK_DATETIME = rowAddApprovalChipInfo(0).RO_CHECK_DATETIME

                '起票者(TC)を設定                
                rowMainChipInfo.LAST_TC_NAME = rowAddApprovalChipInfo(0).ISSUANCE_TC_NAME

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} ADDAPPROVALINFO ON VISITSEQ = {2} RO_CHECK_DATETIME = {3} ISSUANCE_TC_NAME = {4}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , rowMainChipInfo.VISIT_ID _
                                        , rowAddApprovalChipInfo(0).RO_CHECK_DATETIME _
                                        , rowAddApprovalChipInfo(0).ISSUANCE_TC_NAME))

            End If


            '★★★表示エリア判定★★★
            '▲▲ROステータス(1番進んでいるステータス)で判定する▲▲
            Select Case rowMainChipInfo.RO_STATUS


                Case StatusInstructionsWait,
                     StatusWork                 '▲▲50：着工指示待ち、60：作業中▲▲


                    '★★★作業中★★★

                    '表示区分に作業中を設定
                    rowMainChipInfo.DISP_AREA = DisplayDivWork


                Case StatusDeliveryWait         '▲▲80：納車準備待ち▲▲


                    'ROステータスが80で納車準備状態でも追加作業が存在すれば作業中へ
                    '追加作業の存在チェック
                    If C_APPROVAL_STATUS_ON.Equals(rowMainChipInfo.ADDAPPROVAL_FLG) Then
                        '追加作業が存在する場合

                        '★★★作業中★★★

                        '表示区分に作業中を設定
                        rowMainChipInfo.DISP_AREA = DisplayDivWork

                    Else
                        '追加作業が存在しない場合

                        '最小のROステータスチェック
                        If StatusDeliveryWait.Equals(rowMainChipInfo.MIN_RO_STATUS) Then
                            '全てのROステータスが80：納車準備待ちの場合


                            '★★★納車準備★★★

                            '表示区分に納車準備を設定
                            rowMainChipInfo.DISP_AREA = DisplayDivPreparation

                        Else
                            'ROステータスが80：納車準備待ち未満があるがある場合
                            '全てのチップが終了していない場合

                            '★★★作業中★★★

                            '表示区分に作業中を設定
                            rowMainChipInfo.DISP_AREA = DisplayDivWork

                        End If

                    End If


                Case StatusDeliveryWork         '▲▲85：納車作業中▲▲
                    
                    'サービスステータスチェック
                    If ServiceStatusDropOff.Equals(rowMainChipInfo.SVC_STATUS) OrElse _
                       ServiceStatusWaitDelivery.Equals(rowMainChipInfo.SVC_STATUS) Then
                        '「11：預かり中」「12：納車待ち」の場合
                        '★★★納車★★★

                        '表示区分に納車を設定
                        rowMainChipInfo.DISP_AREA = DisplayDivDelivery


                    Else
                        '上記以外の場合
                        'ROステータスが85でも洗車が終わっていなければ納車準備へ
                        '洗車有りかつ洗車完了実績が入ってない場合
                        If WashNeedFlagTrue.Equals(rowMainChipInfo.CARWASH_NEED_FLG) _
                            AndAlso rowMainChipInfo.WASH_RSLT_END_DATETIME = Date.MinValue Then

                            '洗車が終了していない

                            '★★★納車準備★★★

                            '表示区分に納車準備を設定
                            rowMainChipInfo.DISP_AREA = DisplayDivPreparation

                        Else
                            '上記以外

                            '★★★納車★★★

                            '表示区分に納車を設定
                            rowMainChipInfo.DISP_AREA = DisplayDivDelivery

                        End If

                    End If

            End Select

            'RO番号チェック
            If String.IsNullOrEmpty(rowMainChipInfo.RO_NUM) Then
                'RO番号が存在しない

                'データ不整合

                '表示区分にエラーを設定
                rowMainChipInfo.DISP_AREA = DisplayDivNone

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RO = {2} DISP_AREA = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , rowMainChipInfo.RO_NUM, rowMainChipInfo.DISP_AREA))

            '納車予定時間の設定
            '納車予定日時の判定
            If rowMainChipInfo.IsSCHE_DELI_DATETIMENull Then
                '納車予定日日時がない場合

                '作業終了予定日時を設定する
                '作業終了予定日時の確認
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If Not rowMainChipInfo.IsMAX_SCHE_END_DATETIMENull Then
                '    '作業予定日時がある場合

                '    '作業終了予定時刻＋納車準備_異常表示標準時間（分）
                '    rowMainChipInfo.SCHE_DELI_DATETIME = rowMainChipInfo.MAX_SCHE_END_DATETIME.AddMinutes(Me.deliveryPreAbnormalLT)
                'Else
                '    '作業予定日時がない場合

                '    '最小値の設定
                '    rowMainChipInfo.SCHE_DELI_DATETIME = DateTime.MinValue
                'End If
                rowMainChipInfo.SCHE_DELI_DATETIME = DateTime.MinValue
                '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            End If

        Next


        '納車見込遅れ日時取得
        inDtMainChipInfo = Me.SetDeliveryDelayDate(inDealerCode, _
                                                   inBranchCode, _
                                                   inPresentTime, _
                                                   inDtMainChipInfo, _
                                                   inSC3140103Dac)

        '最終作業テクニシャン取得
        inDtMainChipInfo = Me.SetLastTechnician(inDealerCode, _
                                                inBranchCode, _
                                                inDtMainChipInfo, _
                                                inSC3140103Dac)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} OUT:COUNT = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , inDtMainChipInfo.Rows.Count))

        Return inDtMainChipInfo




    End Function


    ''' <summary>
    ''' 最終作業テクニシャン取得処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inDtMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <remarks>>MainChip情報データセット</remarks>
    ''' <history>
    ''' </history>
    Private Function SetLastTechnician(ByVal inDealerCode As String _
                                     , ByVal inBranchCode As String _
                                     , ByVal inDtMainChipInfo As SC3140103MainChipInfoDataTable _
                                     , ByVal inSC3140103Dac As SC3140103DataTableAdapter) _
                                       As SC3140103MainChipInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} DTMAINCHIPINFO = {5}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode, inDtMainChipInfo.Count))


        '最終作業テクニシャン名情報取得
        Dim dtLastTechnicianInfo As SC3140103LastTechnicianInfoDataTable = _
            inSC3140103Dac.GetLastTechnicianInfo(inDealerCode, inBranchCode, inDtMainChipInfo)

        '最終作業テクニシャン名情報分ループ
        For Each rowLastTechnicianInfo In dtLastTechnicianInfo

            '取得した最終作業テクニシャン名情報の予約IDでMainChip情報をSELECT
            Dim rowMainChipInfo As SC3140103MainChipInfoRow() = _
                DirectCast(inDtMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture, _
                                                   "SVCIN_ID = '{0}'", _
                                                   rowLastTechnicianInfo.SVCIN_ID)), SC3140103MainChipInfoRow())

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} SVCIN_ID = {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , rowLastTechnicianInfo.SVCIN_ID))

            'SELECT結果の確認
            If 0 < rowMainChipInfo.Count Then

                '同じサービス入庫IDがある場合
                '最終作業テクニシャンの名前を設定
                rowMainChipInfo(0).LAST_TC_NAME = rowLastTechnicianInfo.LAST_TC_NAME


                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} LASTTECHNICIAN  SVCIN_ID = {2} LAST_TC_NAME = {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , rowLastTechnicianInfo.SVCIN_ID, rowMainChipInfo(0).LAST_TC_NAME))

            End If

        Next


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} OUT:COUNT = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , inDtMainChipInfo.Rows.Count))

        Return inDtMainChipInfo




    End Function


    ''' <summary>
    ''' 納車見込遅れ日時取得処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <param name="inDtMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <remarks>>MainChip情報データセット</remarks>
    ''' <history>
    ''' </history>
    Private Function SetDeliveryDelayDate(ByVal inDealerCode As String _
                                        , ByVal inBranchCode As String _
                                        , ByVal inPresentTime As Date _
                                        , ByVal inDtMainChipInfo As SC3140103MainChipInfoDataTable _
                                        , ByVal inSC3140103Dac As SC3140103DataTableAdapter) _
                                          As SC3140103MainChipInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} DTMAINCHIPINFO = {5}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inDealerCode, inBranchCode, inDtMainChipInfo.Count))


        '各工程チップの残作業時間情報取得
        Dim dtRemainingTimeInfo As SC3140103RemainingTimeInfoDataTable = _
            inSC3140103Dac.GetRemainingTimeInfo(inDealerCode, inBranchCode, inDtMainChipInfo)

        'SMBCommonClassインスタンス
        Using smbCommonBis As New SMBCommonClassBusinessLogic

            ''SMBCommonClass初期処理
            Dim initCommonResult As Long = smbCommonBis.InitCommon(inDealerCode _
                                                                 , inBranchCode _
                                                                 , inPresentTime)

            '初期処理に失敗した場合はタイムアウトException
            If initCommonResult = ReturnCode.ErrDBTimeout Then

                Throw New TimeoutException("TimeOut", New OracleExceptionEx())

            End If

            'チップ情報分ループ
            For Each rowDtMainChipInfo In inDtMainChipInfo

                'チップの予約IDで残作業時間情報をSELECTし残作業時間を取得
                Dim rowRemainingTimeInfo As SC3140103RemainingTimeInfoRow() = _
                    DirectCast(dtRemainingTimeInfo.Select(String.Format(CultureInfo.CurrentCulture, _
                                                       "SVCIN_ID = '{0}'", _
                                                       rowDtMainChipInfo.SVCIN_ID)), SC3140103RemainingTimeInfoRow())

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} SVCIN_ID = {2} " _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , rowDtMainChipInfo.SVCIN_ID))

                '残作業時間
                Dim remainingTime As Long = 0

                'SELECT結果の確認
                If 0 < rowRemainingTimeInfo.Count Then

                    '同じサービス入庫IDがある場合
                    '取得した残作業時間を設定
                    remainingTime = rowRemainingTimeInfo(0).REMAININGTIME


                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                            , "{0}.{1} GET REMAININGTIME  SVCIN_ID = {2} REMAININGTIME = {3}" _
                                            , Me.GetType.ToString _
                                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                            , rowRemainingTimeInfo(0).SVCIN_ID, rowRemainingTimeInfo(0).REMAININGTIME))

                End If

                Try

                    '完成検査完了日時(最終)
                    Dim inspectionDate As Date = Date.MinValue

                    '完成検査完了日時が日付最小値かチェック
                    If rowDtMainChipInfo.MIN_INSPECTION_DATE = Date.MinValue Then
                        '日付最小値の場合
                        '完成検査が終了していないため

                        '日付最小値を設定
                        inspectionDate = Date.MinValue

                    Else
                        '上記以外の場合
                        '完成検査終了しているまたは親子全て終了していない場合

                        '最終完成検査完了日時を設定
                        inspectionDate = rowDtMainChipInfo.MAX_INSPECTION_DATE

                    End If

                    '納車見込遅れ日時取得
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'rowDtMainChipInfo.LIMIT_DELI_DATETIME = _
                    '    smbCommonBis.GetDeliveryDelayDate(CType(rowDtMainChipInfo.DISP_AREA, SMBCommonClassBusinessLogic.DisplayType), _
                    '                                      rowDtMainChipInfo.SCHE_DELI_DATETIME, _
                    '                                      rowDtMainChipInfo.MAX_SCHE_END_DATETIME, _
                    '                                      inspectionDate, _
                    '                                      rowDtMainChipInfo.WASH_RSLT_START_DATETIME, _
                    '                                      rowDtMainChipInfo.WASH_RSLT_END_DATETIME, _
                    '                                      rowDtMainChipInfo.INVOICE_PRINT_DATETIME, _
                    '                                      remainingTime, _
                    '                                      rowDtMainChipInfo.CARWASH_NEED_FLG, _
                    '                                      inPresentTime)
                    rowDtMainChipInfo.LIMIT_DELI_DATETIME = _
                        smbCommonBis.GetDeliveryDelayDate(CType(rowDtMainChipInfo.DISP_AREA, SMBCommonClassBusinessLogic.DisplayType), _
                                                          rowDtMainChipInfo.SCHE_DELI_DATETIME, _
                                                          rowDtMainChipInfo.MAX_SCHE_END_DATETIME, _
                                                          inspectionDate, _
                                                          rowDtMainChipInfo.WASH_RSLT_START_DATETIME, _
                                                          rowDtMainChipInfo.WASH_RSLT_END_DATETIME, _
                                                          rowDtMainChipInfo.INVOICE_PRINT_DATETIME, _
                                                          remainingTime, _
                                                          rowDtMainChipInfo.CARWASH_NEED_FLG, _
                                                          inPresentTime, _
                                                          rowDtMainChipInfo.REMAINING_INSPECTION_TYPE)
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    '引数
                    '表示区分
                    '納車予定日時
                    '作業終了予定日時
                    '完成検査完了日時
                    '洗車開始日時
                    '洗車終了日時
                    '清算書印刷日時
                    '残作業時間(分)
                    '洗車有無
                    '現在時刻


                Catch ex As ArgumentException
                    'データ不整合のときのArgumentException

                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Exception:{2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ex.Message))

                    'DATE最大値を設定
                    rowDtMainChipInfo.LIMIT_DELI_DATETIME = Date.MaxValue

                End Try

            Next

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} OUT:COUNT = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , inDtMainChipInfo.Rows.Count))

        Return inDtMainChipInfo




    End Function


#End Region

#End Region

#Region "SA振当て登録処理"

#Region "Public"

    ''' <summary>
    ''' SA振当て登録処理
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inUpDateTime">更新日時</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function RegisterSA(ByVal inDetailArea As Long _
                             , ByVal inVisitSeq As Long _
                             , ByVal inUpDateTime As Date _
                             , ByVal inStaffInfo As StaffContext _
                             , ByVal inPresentTime As Date) _
                               As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3} ACCOUNT:{4} PRESENTTIME:{5} DETAILAREA:{6}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inStaffInfo.Account, inPresentTime, inDetailArea))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        'DataSetの宣言
        Using da As New SC3140103DataTableAdapter

            Try

                'SA振当用来店管理情報取得
                Dim dtSAAssignInfo As SC3140103SAAssignInfoDataTable = _
                    da.GetSAAssignInfo(inVisitSeq, inUpDateTime)

                '来店管理情報の取得確認(更新日時を条件にしているので検索結果がなければ排他エラー)
                If dtSAAssignInfo IsNot Nothing _
                    AndAlso 0 < dtSAAssignInfo.Count Then
                    '取得成功

                    'ROWに変換
                    Dim rowSAAssignInfo As SC3140103SAAssignInfoRow = _
                        DirectCast(dtSAAssignInfo.Rows(0), SC3140103SAAssignInfoRow)

                    '振当SAの設定
                    rowSAAssignInfo.SACODE = inStaffInfo.Account

                    '現在日時の設定
                    rowSAAssignInfo.PRESENTTIME = inPresentTime

                    'SA登録処理
                    returnCode = Me.RegisterSACommit(rowSAAssignInfo, inStaffInfo, da)

                Else
                    '取得失敗

                    '排他エラー
                    returnCode = C_RET_DBERROR

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR: RETURNCODE = {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , returnCode))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = C_RET_DBTIMEOUT

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' SA登録処理(EnableCommit用)
    ''' </summary>
    ''' <param name="inRowSAAssignInfo">SA振当用来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function RegisterSACommit(ByVal inRowSAAssignInfo As SC3140103SAAssignInfoRow, _
                                      ByVal inStaffInfo As StaffContext, _
                                      ByVal inSC3140103Dac As SC3140103DataTableAdapter) _
                                      As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowSAAssignInfo.REZID, inRowSAAssignInfo.ORDERNO, inStaffInfo.Account))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        '更新件数
        Dim upDateCount As Integer = 0

        Try

            '予約の確認
            If 0 < inRowSAAssignInfo.REZID Then            
                '予約有り

                '最新予約情報取得
                Dim dtNewReservationInfo As SC3140103NewReservationInfoDataTable =
                    inSC3140103Dac.GetNewReservationInfo(inRowSAAssignInfo.REZID)

                '予約情報取得確認
                If dtNewReservationInfo IsNot Nothing _
                    AndAlso 0 < dtNewReservationInfo.Count Then
                    '取得成功

                    'ROWに変換
                    Dim rowNewReservationInfo As SC3140103NewReservationInfoRow =
                        DirectCast(dtNewReservationInfo.Rows(0), SC3140103NewReservationInfoRow)

                    '受付担当予定者の確認
                    If Not String.IsNullOrEmpty(rowNewReservationInfo.ACCOUNT_PLAN) Then
                        '受付担当予定者が取得できた場合

                        '最新の情報に書換え
                        inRowSAAssignInfo.DEFAULTSACODE = rowNewReservationInfo.ACCOUNT_PLAN
                    End If

                    '整備受注Noのチェック
                    If Not rowNewReservationInfo.IsORDERNONull Then
                        '整備受注Noが取得できた場合

                        '最新の情報に書換え
                        inRowSAAssignInfo.ORDERNO = rowNewReservationInfo.ORDERNO
                    End If

                    '行更新カウント
                    If Not rowNewReservationInfo.IsROW_LOCK_VERSIONNull Then
                        '行更新カウントが取得できた場合

                        '最新の情報に書換え
                        inRowSAAssignInfo.ROW_LOCK_VERSION = rowNewReservationInfo.ROW_LOCK_VERSION
                    End If

                    '予約情報更新処理
                    returnCode = Me.UpdateStallRezInfo(inRowSAAssignInfo, inStaffInfo)

                    '受付担当予定者のチェック
                    If Not inRowSAAssignInfo.DEFAULTSACODE.Equals(inRowSAAssignInfo.SACODE) Then
                        '取得した受付担当予定者と、選択行の「SAコード」が異なっている場合

                        '選択されたSAをデフォルトSAに設定
                        inRowSAAssignInfo.DEFAULTSACODE = inRowSAAssignInfo.SACODE

                    End If
                End If
            End If

            '処理結果確認
            If returnCode = C_RET_SUCCESS Then
                '処理が成功している場合

                '来店管理テーブルの更新処理(SA振当登録・SA変更登録)
                upDateCount = inSC3140103Dac.RegisterSAAssign(inRowSAAssignInfo, inStaffInfo.Account)

            End If

            '来店管理テーブルの更新結果確認
            If 0 < upDateCount Then
                '処理が成功している場合

                '更新成功
                returnCode = C_RET_SUCCESS

            Else
                '更新失敗

                '更新失敗
                returnCode = C_RET_DBERROR

                'ロールバック
                Me.Rollback = True

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} ERR:EXCLUSION RETURNCODE = {2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , returnCode))

            End If


        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトエラー
            returnCode = C_RET_DBTIMEOUT

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

        Catch ex As OracleExceptionEx

            'DBエラー
            returnCode = C_RET_DBERROR

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

        Catch ex As Exception

            'その他処理エラー
            returnCode = C_RET_DBERROR

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

            Throw

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' 予約情報更新処理
    ''' </summary>
    ''' <param name="inRowSAAssignInfo">SA振当用来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function UpdateStallRezInfo(ByVal inRowSAAssignInfo As SC3140103SAAssignInfoRow, _
                                        ByVal inStaffInfo As StaffContext) _
                                        As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowSAAssignInfo.REZID, inRowSAAssignInfo.ORDERNO, inStaffInfo.Account))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        'IF結果
        Dim retCode As Long = C_RET_SUCCESS

        'SMB共通関数の宣言
        Using commonClass As New SMBCommonClassBusinessLogic

            'SA振当登録処理

            'サービス入庫行ロック処理
            retCode = commonClass.LockServiceInTable(inRowSAAssignInfo.REZID, _
                                                     inRowSAAssignInfo.ROW_LOCK_VERSION, _
                                                     "0", _
                                                     inStaffInfo.Account, _
                                                     inRowSAAssignInfo.PRESENTTIME, _
                                                     MAINMENUID)
            '更新処理の結果確認
            If retCode = C_RET_SUCCESS Then
                'ストールロック成功

                '入庫日付替え処理
                retCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                                                      inStaffInfo.BrnCD, _
                                                      NoReserveId, _
                                                      inRowSAAssignInfo.REZID, _
                                                      inRowSAAssignInfo.PRESENTTIME, _
                                                      inStaffInfo.Account, _
                                                      inRowSAAssignInfo.PRESENTTIME, _
                                                      MAINMENUID)


                '更新処理の結果確認
                If retCode = C_RET_SUCCESS Then
                    '入庫日付替え更新成功

                    'チップ操作履歴の登録
                    commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                        inStaffInfo.BrnCD, _
                                                        inRowSAAssignInfo.REZID, _
                                                        inRowSAAssignInfo.PRESENTTIME, _
                                                        RegisterType.RegisterServiceIn, _
                                                        inStaffInfo.Account, _
                                                        MAINMENUID, _
                                                        NoActivityId)

                    '受付担当予定者のチェック
                    If Not inRowSAAssignInfo.DEFAULTSACODE.Equals(inRowSAAssignInfo.SACODE) Then
                        '取得した受付担当予定者と、選択行の「SAコード」が異なっていた場合

                        'ストール予約とR/Oの担当SA変更
                        retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                                           inStaffInfo.BrnCD, _
                                                           inRowSAAssignInfo.REZID, _
                                                           inRowSAAssignInfo.ORDERNO, _
                                                           inRowSAAssignInfo.SACODE,
                                                           RepairOrder, _
                                                           inRowSAAssignInfo.PRESENTTIME, _
                                                           inStaffInfo.Account, _
                                                           inRowSAAssignInfo.PRESENTTIME, _
                                                           MAINMENUID)

                    End If
                End If
            End If


            '結果チェック
            Select Case retCode
                Case C_RET_SUCCESS
                    '成功

                    returnCode = C_RET_SUCCESS

                Case C_RET_NOMATCH
                    'DBタイムアウト

                    returnCode = C_RET_DBTIMEOUT

                Case Else
                    'その他エラー

                    returnCode = C_RET_DBERROR

            End Select

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURNCODE = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , returnCode))

        Return returnCode

    End Function

#End Region

#End Region

#Region "呼出し完了更新処理"

    ''' <summary>
    ''' 呼出し完了更新処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>returnCode</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CallCompleted(ByVal inVisitSeq As Long _
                                , ByVal inUpDateTime As Date _
                                , ByVal inStaffInfo As StaffContext _
                                , ByVal inPresentTime As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inStaffInfo.Account, inPresentTime))

        '更新結果
        Dim returnCode As Long = C_RET_SUCCESS

        '呼出ステータス更新
        Using da As New SC3140103DataTableAdapter

            Try

                '呼び出し完了更新処理
                Dim upDateCount As Integer = da.UpdateCallCompleted(inVisitSeq, _
                                                                    inUpDateTime, _
                                                                    inStaffInfo.Account, _
                                                                    inPresentTime)

                '更新結果チェック
                If upDateCount <= 0 Then
                    '更新失敗

                    '更新失敗
                    returnCode = C_RET_DBERROR

                    'ロールバック
                    Me.Rollback = True

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERR:EXCLUSION RETURNCODE = {2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , returnCode))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'DBタイムアウトエラー
                returnCode = C_RET_DBTIMEOUT

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As OracleExceptionEx

                'DBエラー
                returnCode = C_RET_DBERROR

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))
            End Try

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

#End Region

#Region "事前準備チップ予約情報取得"

    ''' <summary>
    ''' 事前準備チップ予約情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">対象の予約ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdvancePreparationsReserveInfo(ByVal dealerCode As String, _
                                                      ByVal branchCode As String, _
                                                      ByVal reserveId As Decimal) _
                                                      As SC3140103AdvancePreparationsReserveInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_START))

        'SC3140103DataTableAdapterの新スタンス
        Using adapter As New SC3140103DataTableAdapter

            '事前準備チップ予約情報取得
            Dim dt As SC3140103AdvancePreparationsReserveInfoDataTable = _
                adapter.GetAdvancePreparationsReserveInfoData(dealerCode, branchCode, reserveId)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} {2}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , LOG_END))

            Return dt

        End Using

    End Function

    ''' <summary>
    ''' 事前準備チップサービス来店管理情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdvancePreparationsVisitManager(ByVal dealerCode As String, _
                                                       ByVal branchCode As String, _
                                                       ByVal reserveId As Decimal) _
                                                       As SC3140103AdvancePreparationsServiceVisitManagementDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'SC3140103DataTableAdapterの新スタンス
        Using adapter As New SC3140103DataTableAdapter

            '事前準備チップサービス来店管理情報取得
            Dim dt As SC3140103AdvancePreparationsServiceVisitManagementDataTable = _
                adapter.GetAdvancePreparationsServiceVisitManagementData(dealerCode, branchCode, reserveId)


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} {2}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , LOG_END))

            Return dt

        End Using

    End Function

    ''' <summary>
    ''' 事前準備チップ予約情報の取得
    ''' </summary>
    ''' <param name="SACODE">SAコード</param> 
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    ''' </history>
    Public Function GetReserveChipInfo(ByVal saCode As String) As SC3140103AdvancePreparationsDataTable
        '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
        'Public Function GetReserveChipInfo() As SC3140103AdvancePreparationsDataTable
        '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

        'Public Sub GetReserveChipInfo(ByRef todayCount As Long,
        '                              ByRef nextCount As Long,
        '                              ByRef dt As SC3140103AdvancePreparationsDataTable)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_START))

        'ログイン情報管理機能（ログイン情報取得）
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dt As SC3140103AdvancePreparationsDataTable = Nothing

        '日付管理機能（現在日付取得）
        Dim nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim nowDateString As String = DateTimeFunc.FormatDate(9, nowDateTime)

        '翌稼働日取得
        Dim nextDateString As String
        Using commonClass As New SMBCommonClassBusinessLogic
            commonClass.InitCommon(staffInfo.DlrCD, staffInfo.BrnCD, nowDateTime)
            Dim nextDate As Date = commonClass.GetWorkingDays(nowDateTime, 1)
            nextDateString = DateTimeFunc.FormatDate(9, nextDate)
        End Using

        '事前準備情報取得
        Using da As New SC3140103DataTableAdapter
            '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
            'dt = da.GetAdvancePreparationsChipData(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString, staffInfo.Account)
            If String.IsNullOrEmpty(saCode.Trim) Then
                dt = da.GetAdvancePreparationsChipDataNoSA(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString)
            Else
                dt = da.GetAdvancePreparationsChipData(staffInfo.DlrCD, staffInfo.BrnCD, nowDateString, nextDateString, saCode)
            End If
            '2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
        End Using

        Dim orderList As New List(Of String)
        '事前準備情報中の整備受注NoがNULL以外のものをリストに詰める
        For Each row As SC3140103AdvancePreparationsRow In dt.Rows
            If Not String.IsNullOrEmpty(row.ORDERNO) Then
                orderList.Add(row.ORDERNO)
            End If
        Next

        'R/O事前準備状態一覧取得
        '整備受注Noのリスト有無で処理を分岐
        'If orderList.Count > 0 Then
        '    Dim saOrder As IC3801012DataSet.REZROStatusListDataTable
        '    Dim bl02 As New IC3801012BusinessLogic
        '    saOrder = bl02.GetREZROStatusList(staffInfo.DlrCD, staffInfo.BrnCD, orderList)

        '    Dim saOrderRow As IC3801012DataSet.REZROStatusListRow
        '    For Each row As SC3140103AdvancePreparationsRow In dt.Rows
        '        'SA事前準備フラグは初期値に未完了("1")を設定
        '        row.SASTATUSFLG = PreparationMiddle
        '        '整備受注NoがNull以外だったものは、取得した一覧と比較する
        '        If Not String.IsNullOrEmpty(row.ORDERNO) Then
        '            Dim aryRow As DataRow() = saOrder.Select(String.Format(CultureInfo.CurrentCulture, _
        '             "ORDERNO = '{0}'", row.ORDERNO))

        '            ' 取得一覧の整備受注Noと一致したものがあった場合SA事前準備フラグを設定する
        '            If Not (aryRow Is Nothing OrElse aryRow.Length = 0) Then
        '                saOrderRow = DirectCast(aryRow(0), IC3801012DataSet.REZROStatusListRow)
        '                row.SASTATUSFLG = saOrderRow.STATUS
        '            Else
        '                row.SASTATUSFLG = PreparationEnd
        '            End If
        '        End If
        '    Next

        'Else
        '    For Each row As SC3140103AdvancePreparationsRow In dt.Rows
        '        'SA事前準備フラグは初期値に未完了("1")を設定
        '        row.SASTATUSFLG = PreparationMiddle
        '    Next
        'End If

        'JDP・SSC情報取得
        dt = Me.GetMarkInfo(dt)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_END))

        Return dt
    End Function

    ''' <summary>
    ''' 事前準備チップのJDP・SSC情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMarkInfo(ByVal dt As SC3140103AdvancePreparationsDataTable) As SC3140103AdvancePreparationsDataTable

        'JDP・SSC情報取得
        'Dim dtCustomerInfoDataTable As IC3800703SrvCustomerDataTable = Nothing
        'Try
        '    Dim blCustomerInfoBusinessLogic As New IC3800703BusinessLogic
        '    For Each row As SC3140103AdvancePreparationsRow In dt.Rows
        '        ' 整備受注NoがNullのものの中で、VINまたは車両登録NoがNull以外のものを対象として、顧客情報を取得する	
        '        If PreparationMiddle.Equals(row.SASTATUSFLG) Then
        '            If (Not String.IsNullOrEmpty(row.VIN.Trim())) Or (Not String.IsNullOrEmpty(row.VCLREGNO.Trim())) Then
        '                dtCustomerInfoDataTable = blCustomerInfoBusinessLogic.GetCustomerInfo(row.VCLREGNO.Trim(), row.VIN.Trim(), staffInfo.DlrCD)

        '                Dim rowCustomerInfo As IC3800703SrvCustomerFRow
        '                '顧客情報が取得できた場合、事前準備情報に設定する
        '                If dtCustomerInfoDataTable IsNot Nothing AndAlso dtCustomerInfoDataTable.Rows.Count > 0 Then
        '                    rowCustomerInfo = DirectCast(dtCustomerInfoDataTable.Rows(0), IC3800703SrvCustomerFRow)

        '                    If Not IsDBNull(rowCustomerInfo.Item("JDPFLAG")) Then
        '                        row.JDP_MARK = rowCustomerInfo.JDPFLAG     'JDP調査対象客マーク
        '                    End If
        '                    If Not IsDBNull(rowCustomerInfo.Item("SSCFLAG")) Then
        '                        row.SSC_MARK = rowCustomerInfo.SSCFLAG     '技術情報マーク
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Next
        'Finally
        '    If Not IsNothing(dtCustomerInfoDataTable) Then
        '        dtCustomerInfoDataTable.Dispose()
        '    End If
        'End Try

        Return dt

    End Function


#End Region

#Region "標準ボタン処理"

    ''' <summary>
    ''' 画面遷移用来店情報取得
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン(排他用)</param>
    ''' <param name="inPreviewFlg">参照フラグ(RO参照・顧客参照へ遷移する際は排他制御が必要ないため)True：参照モード</param>
    ''' <param name="inChangeDmsIdFlg">DMSID変換フラグ(True：有り False：無し)</param>
    ''' <returns>来店実績データセット</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </History>
    Public Function GetNextScreenVisitInfo(ByVal inDetailArea As Long _
                                         , ByVal inVisitSeq As Long _
                                         , ByVal inStaffInfo As StaffContext _
                                         , ByVal inRowLockVersion As Long _
                                         , ByVal inPreviewFlg As Boolean _
                                         , Optional ByVal inChangeDmsIdFlg As Boolean = True) _
                                           As SC3140103NextScreenVisitInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:VISITSEQ = {3} ROWLOCKVERSION = {4} PREVIEWFLAG = {5}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inVisitSeq, inRowLockVersion, inPreviewFlg))

        'データアクセスのインスタンス
        Using da As New SC3140103DataTableAdapter

            '画面遷移用来店情報取得
            Dim dt As SC3140103NextScreenVisitInfoDataTable = _
                da.GetNextScreenVisitInfo(inDetailArea, _
                                          inVisitSeq, _
                                          inStaffInfo.DlrCD, _
                                          inStaffInfo.BrnCD, _
                                          inRowLockVersion, _
                                          inPreviewFlg)

            '画面遷移用来店情報のチェック
            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
                '画面遷移用来店情報が存在する場合


                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                '顧客詳細遷移用に予約の顧客を優先にDMS_CST_CDを取得するように変更
                '新たにSQLで取得する

                '顧客詳細遷移用情報取得
                Dim dtCustomerDetail As SC3140103InfoToPassCustomerDetailDataTable = _
                    da.GetInfoToPassCustomerDetail(inVisitSeq, _
                                                   inStaffInfo.DlrCD, _
                                                   inStaffInfo.BrnCD)

                '顧客詳細遷移用情報のチェック
                If dtCustomerDetail IsNot Nothing AndAlso 0 < dtCustomerDetail.Rows.Count Then
                    '顧客詳細遷移用情報が存在する場合

                    'DMS_CST_CDの値チェック
                    If Not dtCustomerDetail(0).IsDMS_CST_CDNull Then
                        'DMS_CST_CDが存在する

                        '顧客詳細遷移用に別途基幹顧客コードを設定
                        dt(0).DMS_CST_CD = dtCustomerDetail(0).DMS_CST_CD

                    End If

                End If

                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END


                'ログインアカウントを設定
                dt(0).ACCOUNT = inStaffInfo.Account

                '基幹コードへ変換処理
                Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = Me.ChangeDmsCode(inStaffInfo)

                'DMS販売店コードを設定
                dt(0).DMSDLRCD = rowDmsCodeMap.CODE1

                'DMS店舗コードを設定
                dt(0).DMSSTRCD = rowDmsCodeMap.CODE2

                'DMSアカウントを設定
                dt(0).DMSACCOUNT = rowDmsCodeMap.ACCOUNT

                '基幹顧客コード変換処理フラグチェック
                If inChangeDmsIdFlg Then
                    '変換有り

                    'DMSIDの値チェック
                    If Not dt(0).IsDMSIDNull Then
                        'DMSIDが存在する

                        'SMBCommonClassBusinessLogicのインスタンス
                        Using smbCommon As New SMBCommonClassBusinessLogic

                            '基幹顧客コード変換処理
                            dt(0).DMSID = dt(0).DMSID.Replace(String.Concat(inStaffInfo.DlrCD, ATmark), String.Empty)

                        End Using

                    End If

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    'DMS_CST_CDの値チェック
                    If Not dt(0).IsDMS_CST_CDNull Then
                        'DMS_CST_CDが存在する

                        'SMBCommonClassBusinessLogicのインスタンス
                        Using smbCommon As New SMBCommonClassBusinessLogic

                            '基幹顧客コード変換処理
                            dt(0).DMS_CST_CD = dt(0).DMS_CST_CD.Replace(String.Concat(inStaffInfo.DlrCD, ATmark), String.Empty)

                        End Using

                    End If

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                End If

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} NEXTSCREENVISITINFODATATABLE.COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Count))

            '処理結果返却
            Return dt

        End Using

    End Function

    ''' <summary>
    ''' RO_INFO作成処理
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>来店実績データセット</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CreateROInfo(ByVal inDetailArea As Long,
                                 ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow,
                                 ByVal inPresentTime As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:VISITSEQ = {3} DETAILAREA = {4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inRowVisitInfo.VISITSEQ, inDetailArea))

        'IC3810301BusinessLogicのインスタンス
        Using ic3810301Biz As New IC3810301BusinessLogic

            '処理結果
            Dim resultCode As Long = ResultSuccess

            'R/O情報登録
            resultCode = ic3810301Biz.InsertRepairOrderInfo(inRowVisitInfo.REZID, _
                                                            inRowVisitInfo.DLRCD, _
                                                            inRowVisitInfo.STRCD, _
                                                            inRowVisitInfo.VISITSEQ, _
                                                            inRowVisitInfo.ACCOUNT, _
                                                            inPresentTime, _
                                                            MAINMENUID)

            '処理結果チェック
            If resultCode <> ResultSuccess Then
                '処理失敗

                'ロールバック処理
                Me.Rollback = True

                'エラーログ出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:InsertRepairOrderInfo = NG  RESULTCODE = {2} VISITSEQ = {3}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , resultCode, inRowVisitInfo.VISITSEQ))

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} RESULTCODE = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , resultCode))

            '処理結果返却
            Return resultCode

        End Using

    End Function

    ''' <summary>
    ''' 予約情報ポップアップ用の一覧データ取得
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <param name="inBaseDate">取得基準日</param>
    ''' <returns>予約情報ポップアップ用データセット</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
    ''' </History>
    Public Function GetPopupReservationList(ByVal inRowVisitInfo As SC3140103NextScreenVisitInfoRow, _
                                            ByVal inBaseDate As String) As SC3140103ReserveListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:baseDate = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , inBaseDate))

        '戻り値データ
        Dim dtRet As SC3140103ReserveListDataTable
        Dim bl As New IC3811501BusinessLogic

        Try

            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
            '取得基準日以降の予約情報を取得（※取得基準日を含む）
            Dim dt As IC3811501DataSet.IC3811501ReservationListDataTable = _
                bl.GetReservationList(inRowVisitInfo.DLRCD, _
                                      inRowVisitInfo.STRCD, _
                                      inRowVisitInfo.DMSID, _
                                      inRowVisitInfo.VCLREGNO, _
                                      inRowVisitInfo.VIN, _
                                      inBaseDate, _
                                      True)
            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END


            '予約情報が存在しない場合は処理終了（予約情報ポップアップは出力しない）
            If IsNothing(dt) Then
                Return Nothing
            End If

            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            '文言の取得
            Dim hyphenWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdHyphen)                '-
            Dim unCreatedWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdUnCreated)          '未作成
            Dim underCreationWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdUnderCreation)  '作成中

            '予約情報が存在する場合、戻り値データ用に編集してDataTableを返却する
            Dim dtMod As New SC3140103ReserveListDataTable

            For Each row As IC3811501DataSet.IC3811501ReservationListRow In dt

                '予約開始日時
                Dim reserveFrom As String
                Dim fromMD As String
                Dim fromHM As String

                fromMD = DateTimeFunc.FormatDate(CONVERTDATE_MD, row.REZSTARTTIME)      'MM/dd
                fromHM = DateTimeFunc.FormatDate(CONVERTDATE_HM, row.REZSTARTTIME)      'hh:mm
                reserveFrom = fromMD & " " & fromHM                                     'MM/dd hh:mm

                '予約終了日時
                Dim reserveTo As String
                Dim toMD As String
                Dim toHM As String

                toMD = DateTimeFunc.FormatDate(CONVERTDATE_MD, row.REZENDTIME)          'MM/dd
                toHM = DateTimeFunc.FormatDate(CONVERTDATE_HM, row.REZENDTIME)          'hh:mm

                '予約開始日と予約終了日が同じ場合（ = 予約が１日で終了する場合）
                If fromMD.Equals(toMD) Then

                    reserveTo = toHM                                                    'hh:mm
                Else
                    '予約開始日と予約終了日が違う場合（ = 予約が複数日に跨る場合）

                    reserveTo = toMD & " " & toHM                                       'MM/dd hh:mm
                End If

                Dim dr As SC3140103ReserveListRow = dtMod.NewSC3140103ReserveListRow

                With dr

                    '予約開始日時 - 予約終了日時
                    .RESERVEFROMTO = reserveFrom & " " & hyphenWord & " " & reserveTo
                    'サービス名称
                    .SERVICENAME = row.SERVICENAME

                    'ROの作成ステータス（ROステータスが"0: 未作成"の場合　→　『未作成』とする）
                    If row.ROSTATUSCODE.Equals("0") Then

                        .ROSTATUS = unCreatedWord
                    Else

                        'ROの作成ステータス（ROステータスが"1: 作成済（未作成以外）"の場合　→　『作成中』とする）
                        .ROSTATUS = underCreationWord
                    End If

                End With

                '行追加
                dtMod.Rows.Add(dr)
            Next

            dtRet = dtMod

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END))

            Return dtRet

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理

            '終了ログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} OUT:RETURNCODE = {2}" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                     , ResultTimeout))

            Return Nothing

        Finally

            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If

        End Try

    End Function

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic


            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

    End Function

#End Region

#Region "退店・振当解除処理"

#Region "Public"

    ''' <summary>
    ''' 退店・振当解除処理
    ''' </summary>
    ''' <param name="inDetailArea">チップ詳細表示エリア</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpDateTime">更新日時(排他用)</param>
    ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン(排他用)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function SetReceptDelete(ByVal inDetailArea As Long _
                                  , ByVal inVisitSeq As Long _
                                  , ByVal inPresentTime As DateTime _
                                  , ByVal inStaffInfo As StaffContext _
                                  , ByVal inUpDateTime As DateTime _
                                  , ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START DETAILAREA:{2} VISITSEQ:{3} PRESENTTIME:{4} UPDATETIME:{5} ROWLOCKVERSIO:{6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inDetailArea, inVisitSeq, inPresentTime, inUpDateTime, inRowLockVersion))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        'SC3140103DataTableAdapterのインスタンス
        Using sc3140103Dac As New SC3140103DataTableAdapter

            Try

                '表示されているエリアで処理変更
                '表示エリアチェック
                If CType(ChipArea.Assignment, Long) = inDetailArea Then
                    '振当待ちエリア

                    '退店処理
                    returnCode = Me.RegisterReceptDelete(sc3140103Dac, inVisitSeq, inPresentTime, inStaffInfo, inUpDateTime)

                ElseIf CType(ChipArea.Reception, Long) = inDetailArea Then
                    '受付エリア

                    '振当解除処理
                    returnCode = Me.RegisterAssignmentUndo(sc3140103Dac, inVisitSeq, inPresentTime, inStaffInfo, inUpDateTime, inRowLockVersion)

                Else
                    '上記以外のエリア

                    'エラー
                    returnCode = C_RET_DBERROR

                End If

                '正常終了していない場合はロールバックする
                If returnCode <> C_RET_SUCCESS Then
                    '処理失敗

                    'ロールバック
                    Me.Rollback = True

                    '終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} END ERR: RETURNCODE:{2} DETAILAREA:{3} " _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , returnCode, inDetailArea))



                End If


            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理

                'ロールバック
                Me.Rollback = True

                'エラーコード(DBTIMEOUT)
                returnCode = C_RET_DBTIMEOUT


                '終了ログの出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END ERR:DBTIMEOUT RETURNCODE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

            Catch ex As OracleExceptionEx
                'DBエラーの場合

                'ロールバック
                Me.Rollback = True

                'エラーコード(DBERR)
                returnCode = C_RET_NOMATCH


                '終了ログの出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END ERR:DBERR RETURNCODE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))


            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END RETURNCODE:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , returnCode.ToString(CultureInfo.CurrentCulture)))

        '処理結果
        Return returnCode

    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' 退店処理
    ''' </summary>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpDateTime">更新日時(排他用)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function RegisterReceptDelete(ByVal inSC3140103Dac As SC3140103DataTableAdapter _
                                        , ByVal inVisitSeq As Long _
                                        , ByVal inPresentTime As DateTime _
                                        , ByVal inStaffInfo As StaffContext _
                                        , ByVal inUpDateTime As DateTime) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START VISITSEQ:{2} PRESENTTIME:{3} UPDATETIME:{4}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitSeq, inPresentTime, inUpDateTime))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        '更新件数
        Dim upDateCount As Integer = 0

        '退店登録処理
        upDateCount = inSC3140103Dac.RegisterChipDelete(inVisitSeq, inUpDateTime, inStaffInfo.Account, inPresentTime)

        '更新確認
        If upDateCount = 0 Then
            '更新失敗

            '排他エラー
            returnCode = C_RET_DBERROR

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' 振当解除処理
    ''' </summary>
    ''' <param name="inSC3140103Dac">SC3140103DataTableAdapter</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpDateTime">更新日時(排他用)</param>
    ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン(排他用)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2017/01/19 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
    ''' </History>
    Private Function RegisterAssignmentUndo(ByVal inSC3140103Dac As SC3140103DataTableAdapter _
                                          , ByVal inVisitSeq As Long _
                                          , ByVal inPresentTime As DateTime _
                                          , ByVal inStaffInfo As StaffContext _
                                          , ByVal inUpDateTime As DateTime _
                                          , ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START VISITSEQ:{2} PRESENTTIME:{3} UPDATETIME:{4} ROWLOCKVERSIO:{5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitSeq, inPresentTime, inUpDateTime, inRowLockVersion))

        '処理結果
        Dim returnCode As Long = C_RET_SUCCESS

        'SMBCommonClass用処理結果
        Dim commonReturnCode As Long = C_RET_SUCCESS

        '更新件数
        Dim upDateCount As Integer = 0

        '予約ID
        Dim reserveId As Decimal = -1

        '振当解除登録処理
        upDateCount = inSC3140103Dac.RegisterAssignmentUndo(inVisitSeq, _
                                                            inUpDateTime, _
                                                            inStaffInfo.Account, _
                                                            inPresentTime)
        '登録処理チェック
        If upDateCount <> 1 Then
            '登録失敗

            'エラーコード
            returnCode = C_RET_DBERROR

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:RegisterAssignmentUndo RETURNCODE:{2} VISITSEQ:{3} UPDATETIME:{4}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , returnCode, inVisitSeq, inUpDateTime))

            '処理中断
            Return returnCode

        Else
            '登録成功

            '選択チップの来店管理情報取得
            Dim dtServiceVisitManagement As SC3140103ServiceVisitManagementDataTable = _
                inSC3140103Dac.GetVisitManagement(inVisitSeq)


            '来店管理情報取得チェック
            If dtServiceVisitManagement.Count <= 0 Then
                '取得できなかった場合エラー

                'エラーコード
                returnCode = C_RET_NOMATCH

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} SC3140103DataTableAdapter.GetVisitManagement ERROR:{2} VISITSEQ:{3}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode, inVisitSeq))

                '処理中断
                Return returnCode

            End If

            '予約IDを設定
            reserveId = dtServiceVisitManagement(0).FREZID

        End If

        '予約が有る場合は入庫情報更新
        If 0 < reserveId Then

            '共通関数のインスタンス
            Using commonClass As New SMBCommonClassBusinessLogic


                'サービス入庫行ロック処理
                commonReturnCode = commonClass.LockServiceInTable(reserveId, _
                                                                  inRowLockVersion, _
                                                                  "0", _
                                                                  inStaffInfo.Account, _
                                                                  inPresentTime, _
                                                                  MAINMENUID)

                'サービス入庫行ロックチェック
                If commonReturnCode = CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
                    'サービス入庫行ロック成功

                    '入庫日時付替え処理
                    commonReturnCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                                                                   inStaffInfo.BrnCD, _
                                                                   reserveId, _
                                                                   NoReserveId, _
                                                                   Date.MinValue, _
                                                                   inStaffInfo.Account, _
                                                                   inPresentTime, _
                                                                   MAINMENUID)


                    '入庫日時付替え処理チェック
                    If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
                        '処理失敗
                        'エラー

                        'エラーコード
                        returnCode = commonReturnCode

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} SMBCommonClassBusinessLogic.ChangeCarInDate ERROR:{2} VISITSEQ:{3} REZID:{4}" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , returnCode, inVisitSeq, reserveId))

                    Else
                        '処理成功

                        'チップ操作履歴作成処理
                        commonReturnCode = commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                                               inStaffInfo.BrnCD, _
                                                                               reserveId, _
                                                                               inPresentTime, _
                                                                               RegisterType.RegisterServiceIn,
                                                                               inStaffInfo.Account, _
                                                                               MAINMENUID, _
                                                                               NoActivityId)

                        'チップ操作履歴作成処理チェック
                        If commonReturnCode <> CType(SMBCommonClassBusinessLogic.ReturnCode.Success, Long) Then
                            '処理失敗
                            'エラー

                            'エラーコード
                            returnCode = C_RET_DBERROR

                            'エラーログ
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                       , "{0}.{1} SMBCommonClassBusinessLogic.RegisterStallReserveHis ERROR:{2} VISITSEQ:{3} REZID:{4}" _
                                       , Me.GetType.ToString _
                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                       , returnCode, inVisitSeq, reserveId))

                        End If

                    End If

                    '2017/01/19 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
                ElseIf Not commonReturnCode = CType(SMBCommonClassBusinessLogic.ReturnCode.ErrorNoDataFound, Long) Then
                    'サービス入庫行が見つからない（既にキャンセル済み）場合はエラーとしない
                    'サービス入庫行ロック失敗

                    'エラーコード
                    returnCode = commonReturnCode

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} SMBCommonClassBusinessLogic.LockServiceInTable ERROR:{2} VISITSEQ:{3} REZID:{4} ROWLOCKVERSION:{5}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , returnCode, inVisitSeq, reserveId, inRowLockVersion))

                End If
                '2017/01/19 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END

            End Using

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END RETURNCODE:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , returnCode.ToString(CultureInfo.CurrentCulture)))

        '処理結果
        Return returnCode

    End Function

#End Region

#End Region

#Region "呼出し処理"

    ''' <summary>
    ''' 呼出し処理
    ''' </summary>
    ''' <param name="inVisitSequence">来店者SEQ</param>
    ''' <param name="inAccount">担当SA</param>
    ''' <param name="inNowdate">現在日時</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CallVisit(ByVal inVisitSequence As Long, _
                              ByVal inAccount As String, _
                              ByVal inNowdate As Date, _
                              ByVal inUpdateDate As Date) As Long


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccount:{4} inNowdate:{5} inUpdateDate:{6}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inVisitSequence _
                                 , inAccount _
                                 , inNowdate _
                                 , inUpdateDate))

        '処理結果
        Dim returnCode As Integer = 0

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            Try

                '呼出し登録処理
                Dim updateCount As Integer = da.UpdateVisitStausCall(inVisitSequence, _
                                                                     inUpdateDate, _
                                                                     inNowdate, _
                                                                     inAccount)

                '登録処理結果チェック
                If updateCount <> 1 Then
                    '更新失敗

                    'ロールバック
                    Me.Rollback = True

                    'エラーコード
                    Return ResultDBExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理

                'ロールバック
                Me.Rollback = True

                'エラーコード(タイムアウト)
                returnCode = ResultTimeout

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END RETURNCODE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

                '返却
                Return returnCode

            Catch ex As OracleExceptionEx
                'その他オラクルエラー

                'ロールバック
                Me.Rollback = True

                'エラーコード
                returnCode = ResultDBError

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END MESSAGE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , ex.Message))

                '返却
                Return returnCode

            End Try

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Retrun:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ResultSuccess))

        '返却
        Return ResultSuccess

    End Function

#End Region

#Region "呼出しキャンセル処理"

    ''' <summary>
    ''' 呼出しキャンセル処理
    ''' </summary>
    ''' <param name="inVisitSequence">来店者SEQ</param>
    ''' <param name="inAccount">担当SA</param>
    ''' <param name="inNowdate">現在日時</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <returns>returnCode</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CallCancelVisit(ByVal inVisitSequence As Long, _
                                    ByVal inAccount As String, _
                                    ByVal inNowdate As Date, _
                                    ByVal inUpdateDate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccount:{4} inNowdate:{5} inUpdateDate:{6}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inVisitSequence _
                                 , inAccount _
                                 , inNowdate _
                                 , inUpdateDate))

        '処理結果
        Dim returnCode As Integer = 0

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            Try

                '呼出しキャンセル登録処理
                Dim updateCount As Integer = da.UpdateVisitStausCallCancel(inVisitSequence, _
                                                                           inUpdateDate, _
                                                                           inNowdate, _
                                                                           inAccount)

                '登録処理結果チェック
                If updateCount <> 1 Then
                    '更新失敗

                    'ロールバック
                    Me.Rollback = True

                    'エラーコード
                    Return ResultDBExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理

                'ロールバック
                Me.Rollback = True

                'エラーコード(タイムアウト)
                returnCode = ResultTimeout

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END RETURNCODE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

                '返却
                Return returnCode

            Catch ex As OracleExceptionEx
                'その他オラクルエラー

                'ロールバック
                Me.Rollback = True

                'エラーコード
                returnCode = ResultDBError

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END MESSAGE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , ex.Message))

                '返却
                Return returnCode

            End Try

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Retrun:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ResultSuccess))

        '返却
        Return ResultSuccess

    End Function

#End Region

#Region "受付待ちモニターPush送信処理"

    ''' <summary>
    ''' 受付待ちモニターPush送信処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="callFlg">呼出元フラグ(Ture：呼出し処理　False：呼出しキャンセル処理)</param>
    ''' <remarks></remarks>
    Public Sub SendPushForCall(ByVal dealerCode As String, _
                               ByVal storeCode As String, _
                               ByVal callFlg As Boolean)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:dealerCode:{3} storeCode:{4} callFlag:{5}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , dealerCode, storeCode, callFlg))

        'OperationCodeリスト
        Dim stuffCodeList As New List(Of Decimal)
        'OperationCodeリストに権限"60"：STMを設定
        stuffCodeList.Add(Operation.STM)

        'VisitUtilityBusinessLogicのインスタンス
        Dim utility As New VisitUtilityBusinessLogic

        '全オンラインユーザー情報取得
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetOnlineUsers(dealerCode, storeCode, stuffCodeList)

        '後処理
        utility = Nothing

        '"60"：STMのオンラインユーザー分ループ
        For Each userRow As VisitUtilityUsersRow In sendPushUsers

            '送信処理
            TransmissionForCall(userRow.ACCOUNT, callFlg)
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 送信処理
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="callFlag">呼出元フラグ(Ture：呼出し処理　False：呼出しキャンセル処理)</param>
    ''' <remarks></remarks>
    Private Sub TransmissionForCall(ByVal staffCode As String, _
                                    ByVal callFlag As Boolean)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:staffCode:{3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , staffCode))

        'POST送信メッセージの作成
        Dim postSendMessage As New StringBuilder
        postSendMessage.Append("cat=action")
        postSendMessage.Append("&type=main")
        postSendMessage.Append("&sub=js")
        postSendMessage.Append("&uid=" & staffCode)
        postSendMessage.Append("&time=0")

        '呼出元フラグチェック
        If callFlag Then
            '呼出し処理

            '呼出し時
            postSendMessage.Append("&js1=addCallee()")

        Else
            '呼出しキャンセル処理

            '呼出しキャンセル時
            postSendMessage.Append("&js1=delCallee()")
        End If

        '送信処理
        Dim visitUtility As New VisitUtility

        visitUtility.SendPushPC(postSendMessage.ToString)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "呼び出し場所更新処理"

    ''' <summary>
    ''' 呼び出し場所更新処理
    ''' </summary>
    ''' <param name="inVisitSequence">来店者SEQ</param>
    ''' <param name="inCallPlace">呼出場所</param>
    ''' <param name="inAccount">担当SA</param>
    ''' <param name="inNowdate">現在日時</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <returns>returnCode</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CallPlaceChange(ByVal inVisitSequence As Long, _
                                    ByVal inCallPlace As String, _
                                    ByVal inAccount As String, _
                                    ByVal inNowdate As Date, _
                                    ByVal inUpdateDate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inVisitSequence:{3} inAccount:{4} inNowdate:{5} inUpdateDate:{6}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inVisitSequence _
                                 , inAccount _
                                 , inNowdate _
                                 , inUpdateDate))

        '処理結果
        Dim returnCode As Integer = 0

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            Try
                Dim updateCount As Integer = da.UpdateCallPlace(inVisitSequence, _
                                                                     inCallPlace, _
                                                                     inUpdateDate, _
                                                                     inNowdate, _
                                                                     inAccount)

                '登録処理結果チェック
                If updateCount <> 1 Then
                    '更新失敗

                    'ロールバック
                    Me.Rollback = True

                    'エラーコード
                    Return ResultDBExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理

                'ロールバック
                Me.Rollback = True

                'エラーコード(タイムアウト)
                returnCode = ResultTimeout

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END RETURNCODE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , returnCode))

                '返却
                Return returnCode

            Catch ex As OracleExceptionEx
                'その他オラクルエラー

                'ロールバック
                Me.Rollback = True

                'エラーコード
                returnCode = ResultDBError

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END MESSAGE:{2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , ex.Message))

                '返却
                Return returnCode

            End Try

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Retrun:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ResultSuccess))

        '返却
        Return ResultSuccess

    End Function

#End Region

#Region "自社客検索情報取得"

#Region "Public"

    ''' <summary>
    ''' 自社客検索結果取得
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="registrationNo">車両登録No.</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="customerName">氏名</param>
    ''' <param name="appointNumber">基幹予約ID</param>
    ''' <param name="startRow">現在の表示開始行</param>
    ''' <param name="endRow">現在の表示終了行</param>
    ''' <param name="selectLoad">指定読み込み値</param>
    ''' <param name="searchAllCount">検索全件数</param>
    ''' <returns>自社客検索結果</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerList(ByVal inStaffInfo As StaffContext, _
                                    ByVal registrationNo As String, _
                                    ByVal vin As String, _
                                    ByVal customerName As String, _
                                    ByVal appointNumber As String, _
                                    ByVal startRow As Long, _
                                    ByVal endRow As Long, _
                                    ByVal selectLoad As Long, _
                                    ByVal searchAllCount As Long) As SC3140103SearchResult

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, vclRegNo = {5}, vin = {6}, customerName = {7}, " & _
                                 "phone = {8}, startRow = {9}, endRow = {10}, selectLoad = {11}, selectLoad = {12}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inStaffInfo.DlrCD _
                                 , inStaffInfo.BrnCD _
                                 , registrationNo _
                                 , vin _
                                 , customerName _
                                 , appointNumber _
                                 , startRow _
                                 , endRow _
                                 , selectLoad _
                                 , searchAllCount))

        'SystemEnvSettingのインスタンス
        Dim systemEnv As New SystemEnvSetting

        '検索標準読み込み数取得
        Dim loadCount As Long = _
            CType(systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT).PARAMVALUE, Long)

        '検索最大表示数取得
        Dim maxDispCount As Long = _
            CType(systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT).PARAMVALUE, Long)

        '全検索結果件数
        Dim customerCount As Long = searchAllCount
        '検索開始行
        Dim searchStartRow As Long = 0
        '検索終了行
        Dim searchEndRow As Long = 0


        '検索処理呼出し方法による分岐
        'イベントチェック
        If selectLoad = 0 Then
            '検索アイコンタップ時(初回検索時)

            '検索開始行：1行目
            searchStartRow = 1

            '検索終了行：検索標準読み込み数
            searchEndRow = loadCount + 1

        ElseIf 1 <= selectLoad Then
            '次のN件表示タップ時

            '現在表示している最終行＋検索標準読み込み数
            Dim setEndMax As Long = endRow + loadCount + 1

            '全件数より(現在表示している行＋検索標準読み込み数)のほうが大きいかチェック
            If customerCount < setEndMax Then
                '大きい場合

                '検索終了行：全件数
                searchEndRow = customerCount + 1

            Else
                '小さい場合

                '検索終了行：現在表示している行＋検索標準読み込み数)
                searchEndRow = setEndMax

            End If

            '表示しようとしている件数の計算(検索終了行－現在表示している開始行)
            Dim setStartMax As Long = searchEndRow - startRow

            '最大表示件数より表示しようとしている件数が大きいかチェック
            If setStartMax <= maxDispCount Then
                '大きい場合

                '検索開始行：(検索終了行－現在表示している開始行)
                searchStartRow = startRow

            Else
                '小さい場合

                '検索開始行：(検索終了行－検索最大表示数)
                searchStartRow = searchEndRow - maxDispCount

                '検索開始行が正の数字かチェック
                If searchStartRow <= 0 Then
                    '負の数字の場合

                    '検索開始行：1行目
                    searchStartRow = 1

                End If

            End If

        Else
            '前のN件表示タップ時

            '開始行の計算(検索開始行－検索標準読み込み数)
            Dim setStartMin As Long = startRow - loadCount

            '検索開始行が正の数字かチェック
            If setStartMin <= 0 Then
                '負の数字の場合

                '検索開始行：1行目
                searchStartRow = 1

            Else
                '正の数字

                '検索開始行：1行目(検索開始行－検索標準読み込み数)
                searchStartRow = setStartMin

            End If

            '表示しようとしている件数の計算(検索終了行－検索開始行)
            Dim setEndMin As Long = endRow - searchStartRow

            '最大表示件数より表示しようとしている件数が大きいかチェック
            If setEndMin < maxDispCount Then
                '大きい場合

                '検索終了行：現在表示している終了行
                searchEndRow = endRow + 1

            Else
                '小さい場合

                '検索終了行：(検索開始行＋最大表示件数)
                searchEndRow = searchStartRow + maxDispCount

            End If

            '全件数より検索終了行が大きいかチェック
            If customerCount < searchEndRow Then
                '大きい場合

                '検索終了行：全件数
                searchEndRow = customerCount
            End If
        End If

        '取得件数の取得
        Dim searchCount As Long = searchEndRow - searchStartRow

        Dim result As New SC3140103SearchResult
        '顧客検索処理

        '顧客検索情報用DataTable
        Dim dtCustomerSearchResult As IC3800709DataSet.CustomerSearchResultDataTable

        'IC3800709BusinessLogic
        Using ic3800709Biz As New IC3800709BusinessLogic

            '顧客検索情報取得用パラメータ
            Dim searchXmlClass As New CustomerSearchXmlDocumentClass

            '顧客検索情報取得用パラメータ作成処理
            searchXmlClass = Me.CreateCustomerSearchXmlDocument(inStaffInfo, _
                                                                registrationNo, _
                                                                vin, _
                                                                customerName, _
                                                                appointNumber, _
                                                                searchStartRow, _
                                                                searchCount, _
                                                                searchXmlClass)

            '顧客検索情報取得
            dtCustomerSearchResult = ic3800709Biz.CallGetCustomerSearchInfoWebService(searchXmlClass)

        End Using

        '顧客検索情報取得チェック
        If dtCustomerSearchResult Is Nothing Then
            'Nothingの場合
            '予期せぬエラーが発生

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END ERR:CreateCustomerSearchXmlDocument IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '処理終了
            'Nothingを返却
            Return Nothing

        End If

        '処理結果？？
        result.SearchResult = 0

        '画面表示用自社客検索結果情報テーブル
        Dim dt As SC3140103VisitSearchResultDataTable = New SC3140103VisitSearchResultDataTable

        '顧客検索情報取得チェック
        If 0 < dtCustomerSearchResult.Count Then
            '検索結果が存在する場合

            'ResultCodeチェック
            If C_RET_SUCCESS <> dtCustomerSearchResult(0).ResultCode Then
                '失敗している場合

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END ERR:CreateCustomerSearchXmlDocument COUNT = {2}　RESULTCODE(ROWS0) = {3}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , dtCustomerSearchResult.Count, dtCustomerSearchResult(0).ResultCode))

                '処理終了
                'Nothingを返却
                Return Nothing

            End If


            '顧客写真情報取得および返却データテーブルの作成処理
            dt = GetPhotoData(inStaffInfo, dtCustomerSearchResult, dt)

            '全検索結果件数をLongに変換
            If Not Long.TryParse(dtCustomerSearchResult(0).AllCount, customerCount) Then

                '失敗した場合は0を設定
                customerCount = 0

            End If
        End If

        ''現在表示行のチェック
        'If searchEndRow <> customerCount Then
        '    '最大件数と異なる場合は-1をする

        '    searchEndRow = searchEndRow - 1

        'End If

        '返却値の形成
        'DataTable
        result.DataTable = dt
        '検索開始行
        result.ResultStartRow = searchStartRow
        '検索終了行
        result.ResultEndRow = searchEndRow - 1
        '全検索結果件数
        result.ResultCustomerCount = customerCount
        '検索標準読み込み数
        result.StandardCount = loadCount

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2} OUT:COUNT = {3}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_END _
           , result.DataTable.Rows.Count))

        Return result
    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' 顧客検索情報取得用パラメータ作成処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inRegistrationNo">車両登録No.</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inCustomerName">氏名</param>
    ''' <param name="inAppointNumber">電話番号</param>
    ''' <param name="inStartRow">現在の表示開始行</param>
    ''' <param name="inSearchCount">取得件数</param>
    ''' <param name="inSearchXmlClass">CustomerSearchXmlDocumentClass</param>
    ''' <returns>CustomerSearchXmlDocumentClass</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Function CreateCustomerSearchXmlDocument(ByVal inStaffInfo As StaffContext, _
                                                     ByVal inRegistrationNo As String, _
                                                     ByVal inVin As String, _
                                                     ByVal inCustomerName As String, _
                                                     ByVal inAppointNumber As String, _
                                                     ByVal inStartRow As Long, _
                                                     ByVal inSearchCount As Long, _
                                                     ByVal inSearchXmlClass As CustomerSearchXmlDocumentClass) _
                                                     As CustomerSearchXmlDocumentClass

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_START))

        'Headerタグ
        '送信日時
        inSearchXmlClass.Head.TransmissionDate = String.Format(CultureInfo.CurrentCulture, "{0:dd/MM/yyyy HH:mm:ss}", DateTimeFunc.Now(inStaffInfo.DlrCD))

        'Detailタグ

        'Commonタグ

        'DealerCode：販売店コード
        inSearchXmlClass.Detail.Common.DealerCode = inStaffInfo.DlrCD

        'BranchCode：店舗コード
        inSearchXmlClass.Detail.Common.BranchCode = inStaffInfo.BrnCD

        'StaffCode：スタッフアカウント
        inSearchXmlClass.Detail.Common.StaffCode = inStaffInfo.Account


        'SearchConditionタグ

        'Start：検索開始行
        inSearchXmlClass.Detail.SearchCondition.Start = inStartRow.ToString(CultureInfo.CurrentCulture)

        'Count：検索件数
        inSearchXmlClass.Detail.SearchCondition.Count = inSearchCount.ToString(CultureInfo.CurrentCulture)

        'Sort1：第１ソート(車両登録番号)
        inSearchXmlClass.Detail.SearchCondition.Sort1 = SortRegistNo

        'Sort2：第２ソート(顧客名)
        inSearchXmlClass.Detail.SearchCondition.Sort2 = SortCustomerName

        'VclRegNo：車両登録番号
        inSearchXmlClass.Detail.SearchCondition.VclRegNo = inRegistrationNo

        'VclRegNoチェック
        If Not String.IsNullOrEmpty(inRegistrationNo) Then
            'VclRegNoがあるときのみ設定する

            'VclRegNo_MatchType：車両登録番号検索タイプ(後方一致)
            inSearchXmlClass.Detail.SearchCondition.VclRegNo_MatchType = BackwordMatch

        End If

        'CustomerName：顧客名
        inSearchXmlClass.Detail.SearchCondition.CustomerName = inCustomerName

        'CustomerNameチェック
        If Not String.IsNullOrEmpty(inCustomerName) Then
            'CustomerNameがあるときのみ設定する

            'CustomerName_MatchType：顧客名検索タイプ(前方一致)
            inSearchXmlClass.Detail.SearchCondition.CustomerName_MatchType = ForwardMatch

        End If

        'Vin：VIN
        inSearchXmlClass.Detail.SearchCondition.Vin = inVin

        'Vinチェック
        If Not String.IsNullOrEmpty(inVin) Then
            'Vinがあるときのみ設定する

            'Vin_MatchType：VINの検索タイプ(後方一致)
            inSearchXmlClass.Detail.SearchCondition.Vin_MatchType = BackwordMatch

        End If

        'BasRezid：基幹予約ID
        inSearchXmlClass.Detail.SearchCondition.BasRezid = inAppointNumber

        'BasRezidチェック
        If Not String.IsNullOrEmpty(inAppointNumber) Then
            'BasRezidがあるときのみ設定する

            'BasRezid_MatchType：基幹予約ID検索タイプ(後方一致)
            inSearchXmlClass.Detail.SearchCondition.BasRezid_MatchType = BackwordMatch

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2} OUT:COUNT = {3}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_END _
           , inSearchXmlClass))


        Return inSearchXmlClass

    End Function

    ''' <summary>
    ''' 顧客写真情報取得および返却データテーブルの作成処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inDtCustomerSearchResult">顧客検索情報取得</param>
    ''' <param name="inDtVisitSearchResult">自社客検索結果情報</param>
    ''' <returns>自社客検索結果情報</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Function GetPhotoData(ByVal inStaffInfo As StaffContext, _
                                  ByVal inDtCustomerSearchResult As IC3800709DataSet.CustomerSearchResultDataTable, _
                                  ByVal inDtVisitSearchResult As SC3140103VisitSearchResultDataTable) _
                                  As SC3140103VisitSearchResultDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_START))

        '自社客検索結果情報Row
        Dim dtRow As SC3140103VisitSearchResultRow

        'SystemEnvSetting
        Dim systemEnv As New SystemEnvSetting

        '顧客写真URL取得
        Dim systemEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            systemEnv.GetSystemEnvSetting(ConstFacePictureUploadUrl)

        '顧客写真URL
        Dim imageUrl As String = systemEnvRow.PARAMVALUE.Trim()

        ''SMBCommonClassBusinessLogicのインスタンス
        'Dim commonClass As New SMBCommonClassBusinessLogic

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '顧客検索情報取得分ループ
            For Each row As IC3800709DataSet.CustomerSearchResultRow In inDtCustomerSearchResult


                '返却情報をNullチェック後設定
                dtRow = DirectCast(inDtVisitSearchResult.NewRow(), SC3140103VisitSearchResultRow)

                '車両登録番号チェック
                If row.IsVehicleRegistrationNumberNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.VCLREGNO = String.Empty

                Else
                    '値が存在する

                    dtRow.VCLREGNO = row.VehicleRegistrationNumber.Trim()
                End If

                'VINチェック
                If row.IsVinNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.VIN = String.Empty
                Else
                    '値が存在する

                    dtRow.VIN = row.Vin.Trim()
                End If

                '基幹顧客IDチェック
                If row.IsCustomerCodeNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.DMSID = String.Empty

                    '顧客写真URL設定(NotPhotoURL設定)
                    dtRow.IMAGEFILE = NO_IMAGE_ICON

                Else
                    '値が存在する

                    'APIの中で変換してるためGL版ではいらない
                    ''受信した基幹顧客IDに販売店コード＠を追加
                    'dtRow.DMSID = commonClass.ReplaceBaseCustomerCode(inStaffInfo.DlrCD, row.CustomerCode.Trim())

                    'DMSTIDを設定する
                    dtRow.DMSID = row.CustomerCode.Trim()

                    '顧客写真情報取得処理
                    Dim dtPhotoInfo As SC3140103VisitPhotoInfoDataTable = _
                        da.GetCustomerPhotoData(inStaffInfo.DlrCD, inStaffInfo.BrnCD, dtRow.DMSID)

                    '顧客写真情報取得チェック
                    If 0 < dtPhotoInfo.Rows.Count Then
                        '顧客写真情報が存在する

                        'Rowに変換
                        Dim drPhotoInfo As SC3140103VisitPhotoInfoRow = DirectCast(dtPhotoInfo.Rows(0), SC3140103VisitPhotoInfoRow)

                        '顧客写真URLのチェック
                        If Not drPhotoInfo.IsIMAGEFILE_SNull Then
                            'URLが存在する

                            'SystemEnvSetting + 顧客写真URL
                            dtRow.IMAGEFILE = imageUrl + drPhotoInfo.IMAGEFILE_S.Trim()
                        Else
                            'URLが存在しない

                            'NotPhotoURL設定
                            dtRow.IMAGEFILE = NO_IMAGE_ICON
                        End If

                        'CST_IDチェック
                        If Not drPhotoInfo.IsORIGINALIDNull Then
                            '存在する

                            'CST_IDを設定
                            dtRow.CUSTOMERCODE = drPhotoInfo.ORIGINALID.Trim()
                        Else
                            '存在しない

                            '空文字を設定
                            dtRow.CUSTOMERCODE = String.Empty
                        End If
                    Else
                        '顧客写真情報が存在しない

                        'NotPhotoURL設定
                        dtRow.IMAGEFILE = NO_IMAGE_ICON

                        '空文字を設定
                        dtRow.CUSTOMERCODE = String.Empty

                    End If

                End If

                '顧客名チェック
                If row.IsCustomerNameNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.CUSTOMERNAME = String.Empty
                Else
                    '値が存在する

                    dtRow.CUSTOMERNAME = row.CustomerName.Trim()
                End If

                '電話番号チェック
                If row.IsTelNumberNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.TELNO = String.Empty
                Else
                    '値が存在する

                    dtRow.TELNO = row.TelNumber.Trim()
                End If

                '携帯番号チェック
                If row.IsMobileNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.MOBILE = String.Empty
                Else
                    '値が存在する

                    dtRow.MOBILE = row.Mobile.Trim()
                End If

                '車名チェック
                If row.IsSeriesNameNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.VEHICLENAME = String.Empty
                Else
                    '値が存在する

                    dtRow.VEHICLENAME = row.SeriesName.Trim()
                End If

                'プロビンスチェック
                If row.IsVehicleAreaCodeNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.PROVINCE = String.Empty
                Else
                    '値が存在する

                    dtRow.PROVINCE = row.VehicleAreaCode.Trim()
                End If

                'モデルコード
                If row.IsModelCodeNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.MODEL = String.Empty
                Else
                    '値が存在する

                    dtRow.MODEL = row.ModelCode.Trim()
                End If

                'SAコード
                If row.IsServiceAdviserCodeNull Then
                    '値が存在しない

                    '空文字を設定
                    dtRow.SACODE = String.Empty
                Else
                    '値が存在する

                    dtRow.SACODE = row.ServiceAdviserCode.Trim()
                End If

                inDtVisitSearchResult.AddSC3140103VisitSearchResultRow(dtRow)

            Next
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2} OUT:COUNT = {3}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , LOG_END _
           , inDtVisitSearchResult.Rows.Count))

        Return inDtVisitSearchResult

    End Function

#End Region

#End Region

#Region "顧客付替え前確認"

#Region "Public"

    ''' <summary>
    ''' 顧客付替え前確認
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    ''' <param name="registrationNumber">付替え元予約ID</param>
    ''' <param name="inUpdateDate">更新日時(排他用)</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="inDmsId">基幹顧客コード</param>
    ''' <returns></returns>
    ''' <remarks>顧客付替え情報</remarks>
    ''' <history>
    ''' </history>
    Public Function GetCustomerChangeCheck(ByVal inStaffInfo As StaffContext, _
                                           ByVal beforeVisitNumber As Long, _
                                           ByVal registrationNumber As String, _
                                           ByVal vin As String, _
                                           ByVal inUpdateDate As Date, _
                                           ByVal inDmsId As String) As SC3140103BeforeChangeCheckResultRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '顧客付替え前確認テーブル
        Using dtResult As New SC3140103BeforeChangeCheckResultDataTable
            '顧客付替え前確認Row
            Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
                DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

            '付替え元来店管理情報取得テーブル
            Dim dtBeforeVisit As SC3140103ChangesServiceVisitManagementDataTable

            'SC3140103DataTableAdapterのインスタンス
            Using da As New SC3140103DataTableAdapter

                '付替え確認用サービス来店管理情報取得処理
                dtBeforeVisit = da.GetServiseVisitManagementForChangeDate(inStaffInfo.DlrCD, _
                                                                          inStaffInfo.BrnCD, _
                                                                          beforeVisitNumber, _
                                                                          inUpdateDate)
            End Using

            '付替え確認用サービス来店管理情報取得チェック
            If dtBeforeVisit.Count < 0 Then
                '取得失敗

                'ResultCode設定(予期せぬエラー)
                resultRow.CHANGECHECKRESULT = ChangeResultErr

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END ERR:GetServiseVisitManagementForChangeDate IS NOTHING" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '処理終了
                Return resultRow

            End If

            '取得成功した場合Rowに変換処理
            Dim rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow = _
                DirectCast(dtBeforeVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

            'ROステータスのチェック
            '事前準備機能が無い為、ROステータスのチェックではなく
            '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
            If Not rowBeforeVisit.IsORDERNONull Then
                '整備受注番号がある場合

                '顧客承認済み
                resultRow.CHANGECHECKRESULT = ChangeResultErr

                '処理終了
                Return resultRow

            End If

            '日付管理機能（現在日付取得）
            Dim nowDateTime As Date = DateTimeFunc.Now(inStaffInfo.DlrCD)
            '日付フォーマット変換(YYYYMMDDHH24MISS)
            'Dim nowDateString As String = DateTimeFunc.FormatDate(9, nowDateTime)
            Dim nowDateString As String = String.Format(CultureInfo.CurrentCulture, "{0:yyyyMMdd}", nowDateTime)

            '付替え先予約情報
            Dim dtAfterReserve As SC3140103SearchChangesReserveDataTable
            '付替え先来店管理情報
            Dim dtAfterVisit As SC3140103SearchChangesVisitDataTable

            'SC3140103DataTableAdapterのインスタンス
            Using da As New SC3140103DataTableAdapter

                '付替え先予約取得処理処理
                dtAfterReserve = da.SearchStallReserveInfoChangeData(inStaffInfo.DlrCD, _
                                                                     inStaffInfo.BrnCD, _
                                                                     registrationNumber, _
                                                                     vin, _
                                                                     nowDateString, _
                                                                     inDmsId)

                '付替え先サービス来店管理情報取得処理
                dtAfterVisit = da.SearchServiceVisitManagementChangeData(inStaffInfo.DlrCD, _
                                                                         inStaffInfo.BrnCD, _
                                                                         registrationNumber, _
                                                                         vin, _
                                                                         nowDateString)
            End Using

            '付替え先情報の取得
            resultRow = Me.GetAfterReplaceInfo(dtAfterVisit, _
                                               dtAfterReserve, _
                                               rowBeforeVisit, _
                                               beforeVisitNumber)



            '付替え先の担当が他SAでないかのチェック処理
            Dim checkChangeConfirmationResult As Long = Me.CheckChangeConfirmation(inStaffInfo, resultRow)

            'チェック結果確認
            If Not ChangeResultTrue.Equals(checkChangeConfirmationResult) Then
                '0以外(付替え先のSAが異なる場合)

                'ResultCode設定(異なるSAコンファーム用)
                resultRow.CHANGECHECKRESULT = checkChangeConfirmationResult

                '処理終了
                Return resultRow

            End If

            '付替え元予約IDチェック
            If Not rowBeforeVisit.IsFREZIDNull AndAlso _
              0 < rowBeforeVisit.FREZID Then
                '付替え元予約IDが存在する

                '付替え元予約IDを設定
                resultRow.BEFORERESERVENO = rowBeforeVisit.FREZID
            End If

            '車両IDの確認
            If resultRow.IsAFTERVCLIDNull _
                OrElse resultRow.AFTERVCLID <= 0 Then
                '車両IDが取得できていない場合

                '車両IDが取得できていない場合、顧客車両情報を取得する
                resultRow = Me.GetAfterVehicleInfo(resultRow, inStaffInfo.DlrCD, registrationNumber, vin, inDmsId)

            End If

            'ResultCode設定(付替え可能)
            resultRow.CHANGECHECKRESULT = ChangeResultTrue

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END))

            Return resultRow

        End Using

    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' 付替え先情報の取得(サービス来店管理)
    ''' </summary>
    ''' <param name="dtAfterVisit">付替え先サービス来店管理情報</param>
    ''' <param name="dtAfterReserve">付替え先予約情報</param>
    ''' <param name="rowBeforeVisit">付替え元サービス来店管理情報</param>
    ''' <param name="beforeVisitNumber">来店実績連番</param>
    ''' <returns>付替え先情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function GetAfterReplaceInfo(ByVal dtAfterVisit As SC3140103SearchChangesVisitDataTable, _
                                         ByVal dtAfterReserve As SC3140103SearchChangesReserveDataTable, _
                                         ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
                                         ByVal beforeVisitNumber As Long) As SC3140103BeforeChangeCheckResultRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '付替え先情報DataTable
        Dim dtResult As New SC3140103BeforeChangeCheckResultDataTable

        '付替え先情報Row
        Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
            DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

        '付替え先予約ID(特殊パターン)
        Dim afterReserveNumber As Decimal = -1

        '付替え先顧客ID(特殊パターン)
        Dim afterCustomerId As Decimal = -1

        '付替え先車両ID(特殊パターン)
        Dim afterVehicleId As Decimal = -1


        '付替え先サービス来店管理情報チェック
        If dtAfterVisit IsNot Nothing Then
            'テーブルは存在する

            '付替え先サービス来店管理情報を来店日時の昇順でソートしてループ
            For Each rowAfterVisit As SC3140103SearchChangesVisitRow In dtAfterVisit.Select("", "VISITTIMESTAMP ASC")

                '付替え先整備受注番号
                Dim afterVisitOrderNo As String = String.Empty

                '付替え先サービス来店管理情報の整備受注番号のチェック
                If Not rowAfterVisit.IsORDERNONull AndAlso _
                    Not String.IsNullOrEmpty(rowAfterVisit.ORDERNO) Then
                    '値が存在する場合

                    '整備受注番号を設定
                    afterVisitOrderNo = rowAfterVisit.ORDERNO

                End If

                '付替え元と付替え先の来店実績連番の比較
                If Not beforeVisitNumber.Equals(rowAfterVisit.VISITSEQ) Then
                    '付替え元と付替え先の来店実績連番が異なる場合

                    '振当てステータスチェック
                    If Not rowAfterVisit.IsASSIGNSTATUSNull Then
                        '振当てステータス値有り

                        '振当てステータスチェック
                        If Not ASSIGN_STATUS_ASSIGN_FINISH.Equals(rowAfterVisit.ASSIGNSTATUS) Then
                            '付替え先来店情報のSA振当てステータスが未振当ての場合

                            '来店実績連番を設定
                            resultRow.AFTERVISITNO = rowAfterVisit.VISITSEQ

                            '予約IDを設定
                            resultRow.AFTERRESERVENO = rowAfterVisit.FREZID

                            '整備受注番号を設定
                            resultRow.AFTERORDERNO = afterVisitOrderNo

                            '車両IDを設定                          
                            resultRow.AFTERVCLID = rowAfterVisit.VCL_ID

                            '顧客IDを設定
                            resultRow.AFTERCSTID = rowAfterVisit.CST_ID

                            'ループ終了
                            Exit For

                        Else
                            '付替え先来店情報のSA振当てステータスが振当て済の場合

                            'ROステータスのチェック
                            '事前準備機能が無い為、ROステータスのチェックではなく
                            '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
                            '来店管理テーブルに整備受注番号がある場合は顧客承認されているため付替えはできない

                            If rowAfterVisit.IsORDERNONull Then
                                '整備受注NOが未採番の場合

                                '来店実績連番を設定
                                resultRow.AFTERVISITNO = rowAfterVisit.VISITSEQ

                                '予約IDを設定
                                resultRow.AFTERRESERVENO = rowAfterVisit.FREZID

                                '車両IDを追加                            
                                resultRow.AFTERVCLID = rowAfterVisit.VCL_ID

                                '顧客IDを追加
                                resultRow.AFTERCSTID = rowAfterVisit.CST_ID

                                '振当SAのチェック
                                If Not String.IsNullOrEmpty(rowAfterVisit.SACODE) Then
                                    '振当SAがある場合

                                    'SAアカウントを設定(サービス来店管理)
                                    resultRow.AFTERSACODE = rowAfterVisit.SACODE

                                Else
                                    '振当SAがない場合

                                    '予約の担当SA
                                    If Not String.IsNullOrEmpty(rowAfterVisit.ACCOUNT_PLAN) Then
                                        '予約の担当SAがある場合

                                        'SAアカウントを設定(予約の担当SA)
                                        resultRow.AFTERSACODE = rowAfterVisit.ACCOUNT_PLAN

                                    End If

                                End If

                                'ループ終了
                                Exit For
                            
                            End If
                        End If
                    End If

                Else
                    '付替え元と付替え先の来店実績連番が同じ場合
                    'TMT版の現在、新規顧客登録機能が存在しないので顧客付替えで対応する仕様である
                    'その為、既に紐付いている予約を救う処理を追加する
                    '(Icrop側で未取引客で来店し予約有りの場合に顧客付替えした時に紐つくように処理をする)
                    '但し他に紐ついていない予約があればそちらを優先する

                    'ROステータスのチェック
                    '事前準備機能が無い為、ROステータスのチェックではなく
                    '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
                    '来店管理テーブルに整備受注番号がある場合は顧客承認されているため付替えはできない

                    If rowAfterVisit.IsORDERNONull Then
                        '整備受注NOが未採番の場合

                        '予約IDを設定
                        afterReserveNumber = rowAfterVisit.FREZID

                        '車両IDを追加                            
                        afterVehicleId = rowAfterVisit.VCL_ID

                        '顧客IDを追加
                        afterCustomerId = rowAfterVisit.CST_ID

                    End If

                End If

            Next

        End If

        '来店実績連番のチェック 
        If resultRow.IsAFTERVISITNONull Then
            '付替え先来店実績連番が取得できなかった場合

            '付け替え先情報の取得(予約)
            resultRow = Me.GetAfterReplaceReserveInfo(dtAfterVisit, dtAfterReserve, rowBeforeVisit)

        End If

        '付替え先予約情報取得
        'Icrop側未取引客で来店で予約有りで
        '同じ顧客の基幹側(自社客)に付替えされた場合で
        '付替え先予約が取得できなかった時に現在既に紐ついている予約を紐つけるための処理
        resultRow = Me.GetAfterReserveInfo(resultRow, afterReserveNumber, afterVehicleId, afterCustomerId)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return resultRow

    End Function

    ''' <summary>
    ''' 付け替え先情報の取得(予約)
    ''' </summary>
    ''' <param name="dtAfterVisit">付替え先サービス来店管理情報</param>
    ''' <param name="dtAfterReserve">付替え先予約情報</param>
    ''' <param name="rowBeforeVisit">付替え元サービス来店管理情報</param>
    ''' <returns>付け替え先情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function GetAfterReplaceReserveInfo(ByVal dtAfterVisit As SC3140103SearchChangesVisitDataTable, _
                                                ByVal dtAfterReserve As SC3140103SearchChangesReserveDataTable, _
                                                ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow) _
                                                As SC3140103BeforeChangeCheckResultRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '付替え先情報DataTable
        Dim dtResult As New SC3140103BeforeChangeCheckResultDataTable

        '付替え先情報Row
        Dim resultRow As SC3140103BeforeChangeCheckResultRow = _
            DirectCast(dtResult.NewRow(), SC3140103BeforeChangeCheckResultRow)

        '付替え先予約情報チェック
        If dtAfterReserve IsNot Nothing AndAlso 0 < dtAfterReserve.Rows.Count Then

            '納車予定日時の昇順でソートしてループ
            For Each rowAfterReserve As SC3140103SearchChangesReserveRow _
                In dtAfterReserve.Select("", "REZ_PICK_DATE ASC")

                '付替え先整備受注番号
                Dim afterReserveOrderNo As String = String.Empty

                '付替え先予約情報の整備受注番号のチェック
                If Not rowAfterReserve.IsORDERNONull _
                    AndAlso Not String.IsNullOrEmpty(rowAfterReserve.ORDERNO) Then

                    '整備受注番号を設定
                    afterReserveOrderNo = rowAfterReserve.ORDERNO

                End If

                '予約の担当SA
                Dim afterReserveSaCode As String = String.Empty

                '予約の担当SAのチェック
                If Not rowAfterReserve.IsACCOUNT_PLANNull AndAlso _
                    Not String.IsNullOrEmpty(rowAfterReserve.ACCOUNT_PLAN) Then
                    '予約の担当SAが存在する場合

                    'SAアカウントを設定(予約の担当SA)
                    afterReserveSaCode = rowAfterReserve.ACCOUNT_PLAN

                End If

                '付替え元の予約IDと付替え先予約情報の予約IDの比較
                If Not rowBeforeVisit.FREZID.Equals(rowAfterReserve.RESERVEID) Then
                    '付替え元の予約IDと付替え先予約情報の予約IDが異なる場合

                    '検索件数
                    Dim dataRowCount As Long = 0

                    '付替え先サービス来店管理情報のチェック
                    If dtAfterVisit IsNot Nothing Then

                        '付替え先予約の予約IDで付替え先サービス来店管理情報を検索
                        Dim dataRow As DataRow() = _
                            dtAfterVisit.Select(String.Format(CultureInfo.CurrentCulture, _
                                                              "FREZID = '{0}'", rowAfterReserve.RESERVEID))

                        '検索結果確認
                        If dataRow IsNot Nothing Then
                            '検索結果が存在する場合

                            '件数を設定
                            dataRowCount = dataRow.Count
                        End If

                    End If

                    '検索件数チェック
                    '検索件数がある場合はすでに来店と紐付いている為、除外する
                    If dataRowCount = 0 Then
                        '検索件数が0件の場合

                        '整備受注番号チェック
                        If Not rowAfterReserve.IsORDERNONull Then
                            '整備受注番号が存在する場合

                            '予約IDを設定
                            resultRow.AFTERRESERVENO = rowAfterReserve.RESERVEID
                            '整備受注番号を設定
                            resultRow.AFTERORDERNO = afterReserveOrderNo
                            'SAアカウントを設定(予約の担当SA)
                            resultRow.AFTERSACODE = afterReserveSaCode
                            '車両IDを設定
                            resultRow.AFTERVCLID = rowAfterReserve.VCL_ID
                            '顧客IDを設定
                            resultRow.AFTERCSTID = rowAfterReserve.CST_ID

                            '次の行へ
                            Exit For
                        Else
                            '整備受注番号が存在しない場合

                            '予約IDを設定
                            resultRow.AFTERRESERVENO = rowAfterReserve.RESERVEID
                            'SAアカウントを設定(予約の担当SA)
                            resultRow.AFTERSACODE = afterReserveSaCode
                            '車両IDを追加
                            resultRow.AFTERVCLID = rowAfterReserve.VCL_ID
                            '顧客IDを追加
                            resultRow.AFTERCSTID = rowAfterReserve.CST_ID

                            '次の行へ
                            Exit For
                        End If
                    End If
                End If

            Next

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return resultRow

    End Function

    ''' <summary>
    ''' 付替え先の担当が他SAでないかのチェック処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <param name="resultRow">付替え先情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckChangeConfirmation(ByVal inStaffInfo As StaffContext, _
                                             ByVal resultRow As SC3140103BeforeChangeCheckResultRow) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '付替え先予約情報チェック
        If Not resultRow.IsAFTERRESERVENONull _
            AndAlso 0 < resultRow.AFTERRESERVENO Then

            '付替え先予約情報が有る場合（予約NoがNullでない場合）

            '予約の担当SAとログインSAを比較
            If (Not resultRow.IsAFTERSACODENull) AndAlso _
                Not inStaffInfo.Account.Equals(resultRow.AFTERSACODE) And _
                Not String.IsNullOrEmpty(resultRow.AFTERSACODE) Then
                '違う担当SAの場合

                'クライアント側でコンファームを表示するためチェックコード101を設定
                Return ChangeReusltCheck

            End If

        End If

        '付替え先サービス来店管理情報チェック
        If Not resultRow.IsAFTERVISITNONull Then

            '付替え先サービス来店情報がある場合（来店実績連番がNullでない場合）

            '付替え先サービス来店情報の担当SAとログインSAを比較
            If (Not resultRow.IsAFTERSACODENull) And _
                (Not inStaffInfo.Account.Equals(resultRow.AFTERSACODE)) And _
                (Not String.IsNullOrEmpty(resultRow.AFTERSACODE)) Then

                'クライアント側でコンファームを表示するためチェックコード101を設定
                Return ChangeReusltCheck

            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return ChangeResultTrue

    End Function

    ''' <summary>
    ''' 付替え先顧客車両情報取得
    ''' </summary>
    ''' <param name="resultRow">付替え先情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="resisterNumber">車両登録番号</param>
    ''' <param name="vinNumber">VIN</param>
    ''' <param name="inDmsId">基幹顧客コード</param>
    ''' <returns>付替え先情報</returns>
    ''' <remarks></remarks>
    Private Function GetAfterVehicleInfo(ByVal resultRow As SC3140103BeforeChangeCheckResultRow, _
                                         ByVal dealerCode As String, _
                                         ByVal resisterNumber As String, _
                                         ByVal vinNumber As String, _
                                         ByVal inDmsId As String) As SC3140103BeforeChangeCheckResultRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'Dacのインスタンス
        Using da As New SC3140103DataTableAdapter

            '付替え先車両情報取得
            Dim dtAfterVehicleInfo As SC3140103AfterVehicleInfoDataTable = _
                da.GetDBAfterVehicleInfo(dealerCode, resisterNumber, vinNumber, inDmsId)

            '付替え先車両情報取得確認
            If 0 < dtAfterVehicleInfo.Count Then
                '付替え先車両情報が取得できた場合

                '車両ID確認
                If Not dtAfterVehicleInfo(0).IsVCL_IDNull Then
                    '車両IDが存在する場合

                    '車両情報の先頭行の車両IDを結果に設定
                    resultRow.AFTERVCLID = dtAfterVehicleInfo(0).VCL_ID

                End If

                '顧客ID確認
                If Not dtAfterVehicleInfo(0).IsCST_IDNull Then
                    '顧客IDが存在する場合

                    '顧客車両情報の先頭行の顧客IDを結果に設定
                    resultRow.AFTERCSTID = dtAfterVehicleInfo(0).CST_ID

                End If

            End If

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return resultRow

    End Function

    ''' <summary>
    ''' 付替え先予約情報取得
    ''' Icrop側未取引客で来店で予約有りで
    ''' 同じ顧客の基幹側(自社客)に付替えされた場合で
    ''' 付替え先予約が取得できなかった時に現在既に紐ついている予約を紐つけるための処理
    ''' </summary>
    ''' <param name="inResultRow">付替え先情報</param>
    ''' <param name="inAfterReserveNumber">付替え先予約ID</param>
    ''' <param name="inAfterVehicleId">付替え先車両ID</param>
    ''' <param name="inAfterCustomerId">付替え先顧客ID</param>
    ''' <returns>付替え先情報</returns>
    ''' <remarks></remarks>
    Private Function GetAfterReserveInfo(ByVal inResultRow As SC3140103BeforeChangeCheckResultRow, _
                                         ByVal inAfterReserveNumber As Decimal, _
                                         ByVal inAfterVehicleId As Decimal, _
                                         ByVal inAfterCustomerId As Decimal) As SC3140103BeforeChangeCheckResultRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} AFTERRESERVEID = {3} AFTERVEHICLEID = {4} AFTERCUSTOMERID = {5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START, inAfterReserveNumber, inAfterVehicleId, inAfterCustomerId))

        '付替え先予約IDのチェック
        If inResultRow.IsAFTERRESERVENONull OrElse inResultRow.AFTERRESERVENO < 0 Then
            '付替え先予約IDが存在しない場合

            'Icrop側未取引客で来店で予約有りで
            '同じ顧客の基幹側(自社客)に付替えされた場合で
            '付替え先予約が取得できなかった時に現在既に紐ついている予約を紐つけるための処理

            '予約IDチェック
            If 0 < inAfterReserveNumber Then

                '値が存在する場合

                '予約IDを設定
                inResultRow.AFTERRESERVENO = inAfterReserveNumber

            End If

            '車両IDチェック
            If 0 < inAfterVehicleId Then

                '値が存在する場合

                '車両IDを追加                            
                inResultRow.AFTERVCLID = inAfterVehicleId

            End If

            '顧客IDチェック
            If 0 < inAfterCustomerId Then

                '値が存在する場合

                '顧客IDを追加
                inResultRow.AFTERCSTID = inAfterCustomerId

            End If

        End If


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


        Return inResultRow

    End Function

#End Region

#End Region

#Region "顧客付替え登録処理"

#Region "Public"

    ''' <summary>
    ''' 顧客付替え登録処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    ''' <param name="afterVisitNumber">付替え先来店実績連番</param>
    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    ''' <param name="afterOrderNumber">付替え先整備受注No.</param>
    ''' <param name="registrationNumber">車両登録No</param>
    ''' <param name="customerCode">顧客コード</param>
    ''' <param name="basicCustomerId">基幹顧客ID</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="modelCode">モデルコード</param>
    ''' <param name="customerName">氏名</param>
    ''' <param name="phone">電話番号</param>
    ''' <param name="mobile">携帯番号</param>
    ''' <param name="afterVehicleId">付替え先車両ID</param>
    ''' <param name="inUpdateDate">更新日時(排他用)</param>
    ''' <param name="inDetailArea">表示エリア</param>
    ''' <returns>成功：0　失敗：その他</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </history>
    <EnableCommit()>
    Public Function SetCustomerChange(ByVal inStaffInfo As StaffContext, _
                                      ByVal beforeVisitNumber As Long, _
                                      ByVal afterVisitNumber As Long, _
                                      ByVal afterReserveNumber As Decimal, _
                                      ByVal afterOrderNumber As String, _
                                      ByVal registrationNumber As String, _
                                      ByVal customerCode As String, _
                                      ByVal basicCustomerId As String, _
                                      ByVal vin As String, _
                                      ByVal modelCode As String, _
                                      ByVal customerName As String, _
                                      ByVal phone As String, _
                                      ByVal mobile As String, _
                                      ByVal afterVehicleId As Decimal, _
                                      ByVal inUpdateDate As Date, _
                                      ByVal inDetailArea As Long) As Long


        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '付替え元来店管理情報取得
        Dim dtBeforeVisit As SC3140103ChangesServiceVisitManagementDataTable

        '日付管理機能（現在日付取得）
        Dim nowDateTime = DateTimeFunc.Now(inStaffInfo.DlrCD)

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '付替え確認用サービス来店管理情報取得処理
            dtBeforeVisit = da.GetServiseVisitManagementForChangeDate(inStaffInfo.DlrCD, _
                                                                      inStaffInfo.BrnCD, _
                                                                      beforeVisitNumber, _
                                                                      inUpdateDate)

        End Using

        '付替え確認用サービス来店管理情報取得チェック
        If dtBeforeVisit.Count < 0 Then
            '取得失敗

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END ERR:GetServiseVisitManagementForChangeDate IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'ResultCode設定(予期せぬエラー)
            Return ChangeResultErr

        End If

        '取得成功した場合Rowに変換
        Dim rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow = _
            DirectCast(dtBeforeVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

        '付替え元予約ID
        Dim beforeReserveNumberResult As Decimal = -1

        '付替え元予約IDチェック
        If Not rowBeforeVisit.IsFREZIDNull AndAlso 0 < rowBeforeVisit.FREZID Then
            '付替え元予約IDが存在する

            '付替え元予約IDを設定
            beforeReserveNumberResult = rowBeforeVisit.FREZID
        End If

        ''付替え元整備受注番号
        'Dim beforeOrderNumberResult As String = String.Empty

        ''付替え元整備受注番号チェック
        'If String.IsNullOrEmpty(rowBeforeVisit.ORDERNO) Then
        '    '付替え元振当日時が存在する

        '    '付替え元整備受注番号を設定
        '    beforeOrderNumberResult = rowBeforeVisit.ORDERNO
        'End If

        '付替え元振当日時
        Dim beforeAssignDate As Date = Nothing

        '付替え元振当日時チェック
        If Not rowBeforeVisit.IsASSIGNTIMESTAMPNull Then
            '付替え元振当日時が存在する

            '付替え元振当日時を設定
            beforeAssignDate = rowBeforeVisit.ASSIGNTIMESTAMP

        Else
            '付替え元振当日時が存在しない場合(振当していない場合)

            '現在日時を設定
            beforeAssignDate = nowDateTime

        End If

        '付替え元振当てステータス
        Dim beforeAssignStatus As String = rowBeforeVisit.ASSIGNSTATUS

        '入庫日時
        Dim beforeStockDate As Date = Nothing

        '付替え元予約IDチェック
        If Not rowBeforeVisit.IsFREZIDNull _
            AndAlso 0 < rowBeforeVisit.FREZID Then
            '付替え元予約IDが存在する

            '付替え元サービス来店管理情報の予約IDから予約情報の入庫日時を取得する
            beforeStockDate = Me.GetBeforeStockDate(inStaffInfo.DlrCD, _
                                                    inStaffInfo.BrnCD, _
                                                    rowBeforeVisit.FREZID)

        End If

        '付替え元ROステータスのチェック
        '事前準備機能が無い為、ROステータスのチェックではなく
        '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
        If Not rowBeforeVisit.IsORDERNONull Then
            '付替え元整備受注番号がある場合

            'ResultCode設定(RO顧客承認済み)
            Return ChangeResultApproval

        End If

        '付替え先ROステータスのチェック
        '事前準備機能が無い為、ROステータスのチェックではなく
        '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
        If Not String.IsNullOrEmpty(afterOrderNumber) Then
            '付替え先整備受注番号がある場合
            '顧客承認されているため削除

            '付替え先整備受注番号をクリア
            afterOrderNumber = String.Empty
            '付替え先予約IDをクリア
            afterReserveNumber = -1
            '付替え先をクリア
            afterVisitNumber = -1

        End If

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '来店者氏名
        Dim visitName As String = String.Empty
        '来店者電話番号
        Dim visitTel As String = String.Empty
        '予約顧客氏名
        Dim reserveCustomerName As String = String.Empty
        '予約顧客電話番号
        Dim reserveCustomerTel As String = String.Empty

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '付替え先来店実績連番が存在するかチェック
            If 0 < afterVisitNumber Then
                '付替え先来店実績連番が存在する場合

                '付替え先来店管理情報取得
                Dim dtAfterVisit As SC3140103AfterServiceVisitManagementInfoDataTable = _
                    da.GetAfterServiceVisitManagementInfo(afterVisitNumber, _
                                                          inStaffInfo.DlrCD, _
                                                          inStaffInfo.BrnCD)

                '付替え先来店管理情報取得チェック
                If 0 < dtAfterVisit.Count Then
                    '付替え先来店管理情報が存在する

                    '取得した来店者氏名を設定
                    visitName = dtAfterVisit(0).VISITNAME
                    '取得した来店者電話番号を設定
                    visitTel = dtAfterVisit(0).VISITTELNO

                End If

            End If

            '付替え先予約IDが存在するかチェック
            If 0 < afterReserveNumber Then
                '付替え先予約IDが存在する場合

                '付替え先来店管理情報取得
                Dim dtAfterlReserve As SC3140103AfterlReserveCustomerInfoDataTable = _
                    da.GetAfterlReserveCustomerInfo(afterReserveNumber, _
                                                    inStaffInfo.DlrCD, _
                                                    inStaffInfo.BrnCD)

                '付替え先来店管理情報取得チェック
                If 0 < dtAfterlReserve.Count Then
                    '付替え先来店管理情報が存在する

                    '取得した予約顧客氏名を設定
                    reserveCustomerName = dtAfterlReserve(0).CST_NAME
                    '取得した予約顧客電話番号を設定
                    reserveCustomerTel = dtAfterlReserve(0).CST_PHONE

                End If

            End If

        End Using

        '来店者氏名が存在するかチェック
        If String.IsNullOrEmpty(visitName) Then
            '存在しない場合

            '来店者氏名に予約顧客氏名を設定
            visitName = reserveCustomerName

        End If

        '来店者電話番号が存在するかチェック
        If String.IsNullOrEmpty(visitTel) Then
            '存在しばい場合

            '来店者電話番号に予約顧客電話番号を設定
            visitTel = reserveCustomerTel

        End If

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END


        '付替え先と付替え元の予約IDチェックかつ振当待ちエリアかチェック
        If beforeReserveNumberResult = afterReserveNumber _
            AndAlso inDetailArea = CType(ChipArea.Assignment, Long) Then
            '付替え先と付替え元の予約IDが同じ
            'かつ　振当待ちエリアの場合

            '振当待ちエリアの場合は
            '先と元予約が同じであっても振当て処理を行うので
            '付替え元は削除し付替え先の予約情報だけにして振当て処理を行う

            '付替え元の予約IDに-1を設定
            beforeReserveNumberResult = -1


        End If

        'Dim beforeOrderStatus As String = String.Empty

        Try

            '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            ''顧客付替え登録処理(DB更新)
            'Dim updateResult As Long = Me.UpdateCustomerChange(inStaffInfo, _
            '                                                   afterReserveNumber, _
            '                                                   beforeAssignDate, _
            '                                                   beforeReserveNumberResult, _
            '                                                   beforeAssignStatus, _
            '                                                   beforeStockDate, _
            '                                                   rowBeforeVisit, _
            '                                                   beforeVisitNumber, _
            '                                                   customerCode, _
            '                                                   basicCustomerId, _
            '                                                   customerName, _
            '                                                   phone, _
            '                                                   mobile, _
            '                                                   registrationNumber, _
            '                                                   vin, _
            '                                                   modelCode, _
            '                                                   afterVisitNumber, _
            '                                                   afterVehicleId)

            '顧客付替え登録処理(DB更新)
            Dim updateResult As Long = Me.UpdateCustomerChange(inStaffInfo, _
                                                               afterReserveNumber, _
                                                               beforeAssignDate, _
                                                               beforeReserveNumberResult, _
                                                               beforeAssignStatus, _
                                                               beforeStockDate, _
                                                               rowBeforeVisit, _
                                                               beforeVisitNumber, _
                                                               customerCode, _
                                                               basicCustomerId, _
                                                               customerName, _
                                                               phone, _
                                                               mobile, _
                                                               registrationNumber, _
                                                               vin, _
                                                               modelCode, _
                                                               afterVisitNumber, _
                                                               afterVehicleId, _
                                                               visitName, _
                                                               visitTel)

            '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            ' 顧客付替え登録処理結果
            If Not ResultSuccess.Equals(updateResult) Then
                '登録失敗

                'ロールバック
                Me.Rollback = True

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:UpdateCustomerChange  RETURNCODE = {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , updateResult))

                '処理終了
                Return updateResult

            End If

            '処理結果
            Return ResultSuccess

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'ORACLEのタイムアウトのみ処理

            'ロールバック
            Me.Rollback = True

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} OUT:RETURNCODE = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ResultTimeout))

            Return ResultTimeout

        Catch ex As Exception
            'その他全てのエラー

            'ロールバック
            Me.Rollback = True

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} OUT:MESSAGE = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ex.Message))

            Throw

        End Try

    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' 付替え元サービス来店管理情報の予約IDから予約情報の入庫日時を取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="reserveId">付替え元サービス来店管理情報の予約ID</param>
    ''' <returns>入庫日時</returns>
    ''' <remarks></remarks>
    Private Function GetBeforeStockDate(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal reserveId As Decimal) As Date
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '付替え元来店情報の予約情報から、ストール予約情報取得
        Dim dtBeforeReserve As SC3140103ChangesStallReserveDataTable

        Dim beforeStockDate As Date = Nothing

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '付替え元予約情報取得
            dtBeforeReserve = da.GetStallReserveInfoForChangeData(dealerCode, _
                                                                  storeCode, _
                                                                  reserveId)
        End Using

        '付替え元予約情報取得チェック
        If dtBeforeReserve IsNot Nothing _
            AndAlso 0 < dtBeforeReserve.Rows.Count Then
            '付替え元予約情報が存在する

            '付替え元予約情報を作業開始日時の昇順でソートし、1行目をRowに変換する
            Dim rowBeforeReserve As SC3140103ChangesStallReserveRow = _
                DirectCast(dtBeforeReserve.Select("", "STARTTIME ASC")(0), SC3140103ChangesStallReserveRow)

            'Rowチェック
            If Not rowBeforeReserve Is Nothing Then
                'Rowに変換できた場合

                '入庫日チェック
                If Not rowBeforeReserve.IsSTOCKTIMENull Then
                    '入庫日時が存在する

                    '入庫日時を設定(YYYYMMDDHHMM)
                    beforeStockDate = _
                        DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowBeforeReserve.STOCKTIME)

                End If

            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return beforeStockDate

    End Function

    ''' <summary>
    ''' 顧客付替え登録処理(DB更新)
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    ''' <param name="beforeAssignDate">付替え元来店時間</param>
    ''' <param name="beforeReserveNumberResult">付替え元予約ID</param>
    ''' <param name="beforeAssignStatus">付替え元振当ステータス</param>
    ''' <param name="beforeStockDate">付替え元入庫日時</param>
    ''' <param name="rowBeforeVisit">付替え元サービス来店管理情報</param>
    ''' <param name="beforeVisitNumber">付替え元来店実績連番</param>
    ''' <param name="customerCode">顧客ID</param>
    ''' <param name="basicCustomerId">基幹顧客ID</param>
    ''' <param name="customerName">氏名</param>
    ''' <param name="phone">電話番号</param>
    ''' <param name="mobile">携帯番号</param>
    ''' <param name="registrationNumber">車両登録No</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="modelCode">モデルコード</param>
    ''' <param name="afterVisitNumber">付替え先来店実績連番</param>
    ''' <param name="afterVehicleId">付替え先車両ID</param>
    ''' <param name="inVisitName">来店者氏名</param>
    ''' <param name="inVisitTel">来店者電話番号</param>
    ''' <returns>成功：0　失敗：その他</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ''' </History>
    Private Function UpdateCustomerChange(ByVal inStaffInfo As StaffContext, _
                                          ByVal afterReserveNumber As Decimal, _
                                          ByVal beforeAssignDate As Date, _
                                          ByVal beforeReserveNumberResult As Decimal, _
                                          ByVal beforeAssignStatus As String, _
                                          ByVal beforeStockDate As Date, _
                                          ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
                                          ByVal beforeVisitNumber As Long, _
                                          ByVal customerCode As String, _
                                          ByVal basicCustomerId As String, _
                                          ByVal customerName As String, _
                                          ByVal phone As String, _
                                          ByVal mobile As String, _
                                          ByVal registrationNumber As String, _
                                          ByVal vin As String, _
                                          ByVal modelCode As String, _
                                          ByVal afterVisitNumber As Long, _
                                          ByVal afterVehicleId As Decimal, _
                                          ByVal inVisitName As String, _
                                          ByVal inVisitTel As String) As Long

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        'Private Function UpdateCustomerChange(ByVal inStaffInfo As StaffContext, _
        '                              ByVal afterReserveNumber As Decimal, _
        '                              ByVal beforeAssignDate As Date, _
        '                              ByVal beforeReserveNumberResult As Decimal, _
        '                              ByVal beforeAssignStatus As String, _
        '                              ByVal beforeStockDate As Date, _
        '                              ByVal rowBeforeVisit As SC3140103ChangesServiceVisitManagementRow, _
        '                              ByVal beforeVisitNumber As Long, _
        '                              ByVal customerCode As String, _
        '                              ByVal basicCustomerId As String, _
        '                              ByVal customerName As String, _
        '                              ByVal phone As String, _
        '                              ByVal mobile As String, _
        '                              ByVal registrationNumber As String, _
        '                              ByVal vin As String, _
        '                              ByVal modelCode As String, _
        '                              ByVal afterVisitNumber As Long, _
        '                              ByVal afterVehicleId As Decimal) As Long

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} BEFORESTOCKDATE = {3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START, beforeStockDate))


        '日付管理機能（現在日付取得）
        Dim nowDateTime = DateTimeFunc.Now(inStaffInfo.DlrCD)

        'SMBCommonClassBusinessLogicのインスタンス
        Dim blCommon As New SMBCommonClassBusinessLogic

        Try
            '付替え元と付替え先の予約IDが同じかチェック
            If beforeReserveNumberResult <> afterReserveNumber Then
                '付替え元と付替え先の予約IDが異なる場合のみ処理をする


                '付替え元・付替え先予約情報のサービス入庫行ロック処理
                Dim lockResult As Long = Me.ServiceInLock(beforeReserveNumberResult, _
                                                          afterReserveNumber, _
                                                          blCommon, _
                                                          inStaffInfo, _
                                                          nowDateTime)

                'ロック処理結果確認
                If Not ResultSuccess.Equals(lockResult) Then
                    'ロック失敗

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "{0}.{1} Err:ServiceInLock  BEFOREREZID = {2}  AFTERREZID = {3}" _
                              , Me.GetType.ToString _
                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                              , beforeReserveNumberResult, afterReserveNumber))


                    '処理終了
                    Return ResultDBError

                End If

                '付替え先予約IDチェック
                If 0 < afterReserveNumber Then
                    '予約IDが存在する場合


                    '付替え先担当SA変更
                    Dim resultSaChange As Long = blCommon.ChangeSACode(inStaffInfo.DlrCD, _
                                                                       inStaffInfo.BrnCD, _
                                                                       afterReserveNumber, _
                                                                       String.Empty, _
                                                                       inStaffInfo.Account, _
                                                                       RESERVE_FLAG_TRUE, _
                                                                       beforeAssignDate, _
                                                                       inStaffInfo.Account, _
                                                                       nowDateTime, _
                                                                       MAINMENUID)


                    '付替え先担当SA変更確認
                    If Not ResultSuccess.Equals(resultSaChange) Then
                        '付替え先担当SA変更失敗

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} Err:ChangeSACode  BEFOREREZID = {2}  AFTERREZID = {3}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , beforeReserveNumberResult, afterReserveNumber))

                        Return ResultDBError

                    End If

                End If


                '付替え元・付替え先予約IDチェック
                If 0 < beforeReserveNumberResult Or 0 < afterReserveNumber Then
                    'どちらかが両方あるまたはどちらかが存在する場合

                    '入庫予定日時付替え処理
                    Dim resultStockDateChange As Long = blCommon.ChangeCarInDate(inStaffInfo.DlrCD, _
                                                                                 inStaffInfo.BrnCD, _
                                                                                 beforeReserveNumberResult, _
                                                                                 afterReserveNumber, _
                                                                                 beforeAssignDate, _
                                                                                 inStaffInfo.Account, _
                                                                                 nowDateTime, _
                                                                                 MAINMENUID)

                    '処理チェック
                    If Not ResultSuccess.Equals(resultStockDateChange) Then
                        '処理失敗

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} Err:ChangeCarInDate  BEFOREREZID = {2}  AFTERREZID = {3}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , beforeReserveNumberResult, afterReserveNumber))

                        '処理終了
                        Return ResultDBError

                    End If

                End If

            End If

            '付替え元サービス来店管理情報がチェック
            If rowBeforeVisit IsNot Nothing Then
                '付替え元サービス来店管理情報が有る場合

                'SC3140103DataTableAdapterのインスタンス
                Using da As New SC3140103DataTableAdapter

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    ''顧客付替えサービス来店管理テーブル顧客情報更新処理
                    'Dim resultUpdateVisit As Long = da.SetVisitCustomer(beforeVisitNumber, _
                    '                                                    CustsegmentMyCustomer, _
                    '                                                    customerCode, _
                    '                                                    basicCustomerId, _
                    '                                                    customerName, _
                    '                                                    phone, _
                    '                                                    mobile, _
                    '                                                    "0", _
                    '                                                    registrationNumber,
                    '                                                    vin, _
                    '                                                    modelCode, _
                    '                                                    afterReserveNumber, _
                    '                                                    String.Empty,
                    '                                                    inStaffInfo.Account, _
                    '                                                    inStaffInfo.Account, _
                    '                                                    nowDateTime, _
                    '                                                    inStaffInfo.Account, _
                    '                                                    beforeAssignStatus, _
                    '                                                    beforeAssignDate, _
                    '                                                    afterVehicleId, _
                    '                                                    True)

                    '顧客付替えサービス来店管理テーブル顧客情報更新処理
                    Dim resultUpdateVisit As Long = da.SetVisitCustomer(beforeVisitNumber, _
                                                                        CustsegmentMyCustomer, _
                                                                        customerCode, _
                                                                        basicCustomerId, _
                                                                        customerName, _
                                                                        phone, _
                                                                        mobile, _
                                                                        "0", _
                                                                        registrationNumber,
                                                                        vin, _
                                                                        modelCode, _
                                                                        afterReserveNumber, _
                                                                        String.Empty,
                                                                        inStaffInfo.Account, _
                                                                        inStaffInfo.Account, _
                                                                        nowDateTime, _
                                                                        inStaffInfo.Account, _
                                                                        beforeAssignStatus, _
                                                                        beforeAssignDate, _
                                                                        afterVehicleId, _
                                                                        True, _
                                                                        inVisitName, _
                                                                        inVisitTel)

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    '更新結果チェック
                    If resultUpdateVisit <> 1 Then
                        '更新失敗

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} Err:SetVisitCustomer  VISITSEQ = {2}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , beforeVisitNumber))

                        '処理終了
                        Return ResultDBError

                    End If

                End Using

            End If

            '付替え先来店実績連番がある場合
            '付替え先のサービス来店管理情報は全てクリアされれ(真っ白チップ)

            '付替え先来店実績連番チェック
            If 0 < afterVisitNumber Then
                '付替え先来店実績連番がある場合

                'SC3140103DataTableAdapterのインスタンス
                Using da As New SC3140103DataTableAdapter

                    '顧客付替え・顧客解除用サービス来店管理テーブル顧客情報クリア処理
                    Dim resultClearVisit As Long = da.VisitCustomerClear(afterVisitNumber, _
                                                                         nowDateTime, _
                                                                         inStaffInfo.Account, _
                                                                         beforeAssignStatus, _
                                                                         beforeAssignDate, _
                                                                         False)

                    '顧客情報クリア処理結果チェック
                    If resultClearVisit <> 1 Then
                        '処理失敗

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} Err:VisitCustomerClear  VISITSEQ = {2}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , afterVisitNumber))

                        '処理終了
                        Return ResultDBError

                    End If

                End Using

            End If

            '付替え先元予約IDの確認
            If 0 < beforeReserveNumberResult Then
                '付替え元予約IDがある場合

                '付替え元予約履歴の作成
                blCommon.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                 inStaffInfo.BrnCD, _
                                                 beforeReserveNumberResult, _
                                                 nowDateTime,
                                                 RegisterType.RegisterServiceIn, _
                                                 inStaffInfo.Account, _
                                                 MAINMENUID, _
                                                 NoActivityId)

            End If

            '付替え先予約IDの確認
            If 0 < afterReserveNumber Then
                '付替え先予約IDがある場合

                '付替え先予約履歴の作成
                blCommon.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                 inStaffInfo.BrnCD, _
                                                 afterReserveNumber, _
                                                 nowDateTime,
                                                 RegisterType.RegisterServiceIn, _
                                                 inStaffInfo.Account, _
                                                 MAINMENUID, _
                                                 NoActivityId)


            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} OUT:RETURNCODE = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , ResultSuccess))

        Finally

            If blCommon IsNot Nothing Then
                blCommon.Dispose()
                blCommon = Nothing
            End If

        End Try

        '処理結果
        Return ResultSuccess

    End Function

    ''' <summary>
    ''' サービス入庫行ロック処理
    ''' </summary>
    ''' <param name="beforeReserveNumberResult">付替え元予約ID</param>
    ''' <param name="afterReserveNumber">付替え先予約ID</param>
    ''' <returns>ロック処理結果</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
    ''' </History>
    Private Function ServiceInLock(ByRef beforeReserveNumberResult As Decimal, _
                                   ByRef afterReserveNumber As Decimal, _
                                   ByVal blCommon As SMBCommonClassBusinessLogic, _
                                   ByVal staffInfo As StaffContext, _
                                   ByVal inNowDate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:BEFOREREZID={3}, AFTERREZID={4}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , beforeReserveNumberResult _
                                 , afterReserveNumber))

        '行ロック結果
        Dim commonResult As Long = 0

        '行ロックバージョン
        Dim beforelockVersion As Long = -1

        '行ロックバージョン
        Dim afterlockVersion As Long = -1

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '付替え元予約情報確認
            If 0 < beforeReserveNumberResult Then
                '付替え元の予約情報がある場合

                '顧客付替え予約の行ロックバージョン取得処理
                Dim dtBeforeNewReserve As SC3140103NewestStallRezInfoDataTable = da.GetDBNewestStallRezInfo(beforeReserveNumberResult)

                '予約情報の取得確認
                If 0 < dtBeforeNewReserve.Count Then
                    '予約が取得できた場合

                    '行ロックバージョンの設定
                    beforelockVersion = dtBeforeNewReserve.Item(0).ROW_LOCK_VERSION

                End If

                '付替え元予約情報のサービス入庫行ロック処理
                commonResult = blCommon.LockServiceInTable(beforeReserveNumberResult, _
                                                           beforelockVersion, _
                                                           "0", _
                                                           staffInfo.Account, _
                                                           inNowDate, _
                                                           MAINMENUID)

                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
                '予約情報取得チェック
                If (commonResult = CType(SMBCommonClassBusinessLogic.ReturnCode.ErrorNoDataFound, Long)) Then
                    '予約情報が見つからない（キャンセル済み）場合

                    '付替え元の予約IDをなしとする
                    beforeReserveNumberResult = -1

                    'エラーとしない
                    commonResult = 0
                End If
                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END

            End If

            '付替え先予約情報確認
            If 0 < afterReserveNumber AndAlso ResultSuccess.Equals(commonResult) Then
                '付替え先の予約情報がある場合

                '顧客付替え予約の行ロックバージョン取得処理
                Dim dtAfterNewReserve As SC3140103NewestStallRezInfoDataTable = da.GetDBNewestStallRezInfo(afterReserveNumber)

                '予約情報の取得確認
                If 0 < dtAfterNewReserve.Count Then
                    '予約が取得できた場合

                    '行ロックバージョンの設定
                    afterlockVersion = dtAfterNewReserve.Item(0).ROW_LOCK_VERSION

                End If

                '付替え先予約情報のサービス入庫行ロック処理
                commonResult = blCommon.LockServiceInTable(afterReserveNumber, _
                                                           afterlockVersion, _
                                                           "0", _
                                                           staffInfo.Account, _
                                                           inNowDate, _
                                                           MAINMENUID)

                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
                '予約情報取得チェック
                If (commonResult = CType(SMBCommonClassBusinessLogic.ReturnCode.ErrorNoDataFound, Long)) Then
                    '予約情報が見つからない（キャンセル済み）場合

                    '付替え元の予約IDをなしとする
                    afterReserveNumber = -1

                    'エラーとしない
                    commonResult = 0
                End If
                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Retrun:{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , commonResult))

            '処理結果
            Return commonResult

        End Using

    End Function

#End Region

#End Region

#Region "顧客解除処理"

#Region "Public"

    ''' <summary>
    ''' 顧客解除処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情</param>
    ''' <param name="removeVisitNumber">解除対象の来店実績連番</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inUpdateDate">更新日時(排他用)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    <EnableCommit()>
    Public Function SetCustomerClear(ByVal inStaffInfo As StaffContext, _
                                     ByVal removeVisitNumber As Long, _
                                     ByVal inNowDate As Date, _
                                     ByVal inUpdateDate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}  VISITSEQ = {3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_START _
                   , removeVisitNumber))


        '解除チップのサービス来店管理情報
        Dim dtRemoveVisit As SC3140103ChangesServiceVisitManagementDataTable


        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '解除チップのサービス来店管理情報取得処理
            dtRemoveVisit = da.GetServiseVisitManagementForChangeDate(inStaffInfo.DlrCD, _
                                                                      inStaffInfo.BrnCD, _
                                                                      removeVisitNumber, _
                                                                      inUpdateDate)
        End Using

        '解除チップのサービス来店管理情報取得チェック
        If dtRemoveVisit.Count < 0 Then
            '取得失敗

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END ERR:GetServiseVisitManagementForChangeDate IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'ResultCode設定(予期せぬエラー)
            Return ChangeResultErr

        End If

        '取得成功した場合Rowに変換
        Dim rowRemoveVisit As SC3140103ChangesServiceVisitManagementRow = _
            DirectCast(dtRemoveVisit.Rows(0), SC3140103ChangesServiceVisitManagementRow)

        '予約ID
        Dim removeReserveNumberResult As Decimal = -1

        '入庫日時
        Dim removeStockDate As Date = Nothing

        '解除チップの振当日時
        Dim assignDate As Date = Nothing

        '解除チップの振当日時チェック
        If Not rowRemoveVisit.IsASSIGNTIMESTAMPNull Then
            '付替え元振当日時が存在する

            '付替え元振当日時を設定
            assignDate = rowRemoveVisit.ASSIGNTIMESTAMP
        End If

        '解除チップの振当てステータス
        Dim assignStatus As String = rowRemoveVisit.ASSIGNSTATUS

        '来店管理の予約IDチェック
        If Not rowRemoveVisit.IsFREZIDNull _
            AndAlso 0 < rowRemoveVisit.FREZID Then
            '予約IDが存在する場合

            '予約IDの設定
            removeReserveNumberResult = rowRemoveVisit.FREZID

            '解除チップの予約情報の入庫日時の取得
            removeStockDate = Me.GetRemoveStockDate(inStaffInfo.DlrCD, _
                                                    inStaffInfo.BrnCD, _
                                                    rowRemoveVisit.FREZID)
        End If


        '解除チップROステータスのチェック
        '事前準備機能が無い為、ROステータスのチェックではなく
        '来店管理テーブルに整備受注番号が無いことで顧客承認されていなこととする
        If Not rowRemoveVisit.IsORDERNONull Then
            '解除チップ整備受注番号がある場合

            'ResultCode設定(RO顧客承認済み)
            Return ChangeResultApproval

        End If

        'R/O情報がある場合、R/O基本情報を参照する
        Dim blCommon As New SMBCommonClassBusinessLogic


        Try

            '顧客解除処理DB更新処理
            Dim updateResult As Long = Me.UpdateCustomerClear(inStaffInfo.DlrCD, _
                                                              inStaffInfo.BrnCD, _
                                                              removeReserveNumberResult, _
                                                              removeVisitNumber, _
                                                              inStaffInfo, _
                                                              inNowDate, _
                                                              assignStatus, _
                                                              assignDate)

            '顧客解除処理DB更新処理結果
            If Not ResultSuccess = updateResult Then
                '更新失敗

                'ロールバック
                Me.Rollback = True

                'エラーログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} ERR:UpdateCustomerClear  RETURNCODE = {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , updateResult))

                '処理終了
                Return updateResult

            End If

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理

            'ロールバック
            Me.Rollback = True

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} OUT:RETURNCODE = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ResultTimeout))

            '処理終了
            Return ResultTimeout

        Catch ex As Exception
            'その他全てのエラー

            'ロールバック
            Me.Rollback = True

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} OUT:MESSAGE = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ex.Message))

            Throw

        Finally

            If blCommon IsNot Nothing Then

                blCommon.Dispose()
                blCommon = Nothing

            End If

        End Try

        Return ResultSuccess

    End Function

#End Region

#Region "Private"

    ''' <summary>
    ''' 入庫日時の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRemoveStockDate(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal reserveId As Decimal) As Date

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:dealerCode = {3}, storeCode = {4}, reserveId = {5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dealerCode _
                  , storeCode _
                  , reserveId))

        '解除チップの予約情報
        Dim dtRemoveReserve As SC3140103ChangesStallReserveDataTable

        '入庫日時
        Dim removeStockDate As Date = Nothing

        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '解除チップの予約情報
            dtRemoveReserve = da.GetStallReserveInfoForChangeData(dealerCode, _
                                                                  storeCode, _
                                                                  reserveId)

        End Using

        '解除チップの予約情報チェック
        If dtRemoveReserve IsNot Nothing _
            AndAlso 0 < dtRemoveReserve.Rows.Count Then
            '解除チップの予約情報が存在する

            '解除チップの予約情報の作業開始日時の昇順でソートし、1行目をRowに変換する
            Dim rowRemoveReserve As SC3140103ChangesStallReserveRow = _
                DirectCast(dtRemoveReserve.Select("", "STARTTIME ASC")(0), SC3140103ChangesStallReserveRow)

            'Rowチェック
            If Not rowRemoveReserve Is Nothing Then
                'Rowに変換できた場合

                '入庫日チェック
                If Not rowRemoveReserve.IsSTOCKTIMENull Then
                    '入庫日時が存在する

                    '入庫日時を設定(YYYYMMDDHHMM)
                    removeStockDate = _
                        DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowRemoveReserve.STOCKTIME)

                End If

            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

        Return removeStockDate

    End Function

    ''' <summary>
    ''' 顧客解除処理DB更新処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="removeReserveNumberResult">予約ID</param>
    ''' <param name="removeVisitNumber">来店実績連番</param>
    ''' <param name="staffInfo">ログイン情報</param>
    ''' <param name="inNowDate">現在時刻</param>
    ''' <param name="inAssignStatus">振当ステータス</param>
    ''' <param name="inAssignDate">振当て日時</param>
    ''' <returns>登録処理結果</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
    ''' </History>
    Private Function UpdateCustomerClear(ByVal dealerCode As String, _
                                         ByVal storeCode As String, _
                                         ByVal removeReserveNumberResult As Decimal, _
                                         ByVal removeVisitNumber As Long, _
                                         ByVal staffInfo As StaffContext, _
                                         ByVal inNowDate As Date, _
                                         ByVal inAssignStatus As String, _
                                         ByVal inAssignDate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:detailArea = {3}, storeCode = {4}," & _
                                   " removeReserveNumberResult = {5}, removeVisitNumber = {6}," & _
                                   " staffInfo = (StaffContext), inNowDate = {7} , inNowDate = {8}, inNowDate = {9}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , dealerCode _
                                 , storeCode _
                                 , removeReserveNumberResult _
                                 , removeVisitNumber _
                                 , inNowDate _
                                 , inAssignStatus _
                                 , inAssignDate))

        'SMBCommonClassBusinessLogicのインスタンス
        Dim blCommon As New SMBCommonClassBusinessLogic

        Try

            '解除チップの予約ID確認
            If 0 < removeReserveNumberResult Then
                '解除チップの予約IDが存在する

                'サービス入庫行ロック処理
                Dim lockResult As Long = Me.ServiceInLock(removeReserveNumberResult, _
                                                          -1, _
                                                          blCommon, _
                                                          staffInfo, _
                                                          inNowDate)

                'ロック処理結果確認
                If Not ResultSuccess.Equals(lockResult) Then
                    'ロック失敗

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "{0}.{1} Err:ServiceInLock  REZID = {2} " _
                              , Me.GetType.ToString _
                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                              , removeReserveNumberResult))


                    '処理終了
                    Return lockResult

                End If

                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
                '解除チップの予約ID確認
                If 0 < removeReserveNumberResult Then
                    '予約IDが存在する（キャンセルされていない）場合

                    '入庫予定日時付替え処理
                    Dim resultStockDateChange As Long = blCommon.ChangeCarInDate(dealerCode, _
                                                                                 storeCode, _
                                                                                 removeReserveNumberResult, _
                                                                                 DEFAULT_LONG_VALUE, _
                                                                                 DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, DEFAULT_STOCKTIME_VALUE), _
                                                                                 staffInfo.Account, _
                                                                                 inNowDate, _
                                                                                 MAINMENUID)

                    '処理チェック
                    If Not ResultSuccess.Equals(resultStockDateChange) Then
                        '処理失敗

                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} Err:ChangeCarInDate  REZID = {2} " _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , removeReserveNumberResult))

                        '処理終了
                        Return resultStockDateChange

                    End If

                    '解除チップ予約履歴の作成
                    blCommon.RegisterStallReserveHis(dealerCode, _
                                                     storeCode, _
                                                     removeReserveNumberResult, _
                                                     inNowDate, _
                                                     RegisterType.RegisterServiceIn, _
                                                     staffInfo.Account, _
                                                     MAINMENUID, _
                                                     NoActivityId)

                End If
                '2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END
            End If

            'SC3140103DataTableAdapterのインスタンス
            Using da As New SC3140103DataTableAdapter

                '顧客付替え・顧客解除用サービス来店管理テーブル顧客情報クリア処理
                Dim resultClearVisit As Long = da.VisitCustomerClear(removeVisitNumber, _
                                                                     inNowDate, _
                                                                     staffInfo.Account, _
                                                                     inAssignStatus, _
                                                                     inAssignDate, _
                                                                     False)


                '顧客情報クリア処理結果チェック
                If resultClearVisit <> 1 Then
                    '処理失敗

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "{0}.{1} Err:VisitCustomerClear  VISITSEQ = {2}" _
                              , Me.GetType.ToString _
                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                              , removeVisitNumber))

                    '処理終了
                    Return ResultDBError

                End If

            End Using

        Finally

            If blCommon IsNot Nothing Then

                blCommon.Dispose()
                blCommon = Nothing

            End If

        End Try

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Retrun:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ResultSuccess))

        Return ResultSuccess

    End Function

#End Region

#End Region

#Region "サービス標準LT取得処理"

    ''' <summary>
    ''' サービス標準LT取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>標準LT</returns>
    ''' <remarks></remarks>
    Public Function GetStandardLTList(ByVal inDealerCode As String, _
                                      ByVal inStoreCode As String) _
                                      As IC3810701DataSet.StandardLTListDataTable
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inDealerCode = {3}, inStoreCode = {4}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDealerCode, inStoreCode))

        'IC3810701BusinessLogicのインスタンス
        Dim bl As New IC3810701BusinessLogic
        '標準LT情報テーブル
        Dim dt As IC3810701DataSet.StandardLTListDataTable

        Try

            'サービス標準LT取得処理
            dt = bl.GetStandardLTList(inDealerCode, inStoreCode)


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理


            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} Err:GetStandardLTList  OUT:RETURNCODE = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ResultTimeout))


            Throw

        Finally

            If bl IsNot Nothing Then

                bl.Dispose()
                bl = Nothing

            End If
        End Try

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return dt
    End Function

#End Region

#Region "通知処理"

#Region "通知用定数"

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
    ''' 顧客詳細画面用セッション名("SessionKey.DMS_CST_ID")
    ''' </summary>
    Private Const SessionDMSID As String = "SessionKey.DMS_CST_ID,String,"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.VIN")
    ''' </summary>
    Private Const SessionVIN As String = "SessionKey.VIN,String,"

    ' ''' <summary>
    ' ''' 未取引客のリンク文字列
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const NewCustomerLink As String = "<a id='SC30802250' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 自社客のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyCustomerLink As String = "<a id='SC30802250' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SVR：未振当て一覧へのPush  来店管理へのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSVRPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=Send_Visit()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(WB：ウェルカムボードへのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshWBPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=RefreshVisitList()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SA：振当待ちエリアへのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAAssignmentPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=AssignmentRefresh()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SA：全体へのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=MainRefresh()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(CT/CHT権限(SMBへのPush))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshCTAndCHTPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=RefreshSMB()"

    ''' <summary>
    ''' リフレッシュ通知のAccount置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshAccountReplaceWord As String = "#USER_ACCOUNT#"

    ''' <summary>
    ''' 作成するメッセージフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MessageType

        ''' <summary>
        ''' 自社客かつ車両登録No情報有
        ''' </summary>
        ''' <remarks></remarks>
        MyCustomer = 1

        ''' <summary>
        ''' 未取引客
        ''' </summary>
        ''' <remarks></remarks>
        NewCustomer = 2

    End Enum

#End Region

#Region "Publicメソッド"

    ' ''' <summary>
    ' ''' 通知送信用情報取得
    ' ''' </summary>
    ' ''' <param name="inVisitSeq">来店実績連番</param>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inBranchCode">店舗コード</param>
    ' ''' <returns>通知送信用情報データセット</returns>
    ' ''' <remarks></remarks>
    'Public Function GetNoticeProcessingInfo(ByVal inVisitSeq As Long _
    '                                      , ByVal inDealerCode As String _
    '                                      , ByVal inBranchCode As String) _
    '                                        As SC3140103NoticeProcessingInfoDataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} VISITSEQ:{2} DEALERCODE:{3} BRANCHCODE:{4}" _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inVisitSeq, inDealerCode, inBranchCode))


    '    'データアクセスのインスタンス
    '    Using da As New SC3140103DataTableAdapter

    '        '画面遷移用来店情報取得
    '        Dim dt As SC3140103NoticeProcessingInfoDataTable = _
    '            da.GetNoticeProcessingInfo(inVisitSeq, _
    '                                       inDealerCode, _
    '                                       inBranchCode)


    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} {2} SC3140103NoticeProcessingInfoDataTable.COUNT = {3}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , LOG_END _
    '                                , dt.Count))

    '        '処理結果返却
    '        Return dt

    '    End Using

    'End Function

    ''' <summary>
    ''' 通知処理
    ''' </summary>
    ''' <param name="inDetailArea">表示エリア</param>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub NoticeProcessing(ByVal inDetailArea As Long, _
                                ByVal inVisitSeq As Long, _
                                ByVal inPresentTime As DateTime, _
                                ByVal inStaffInfo As StaffContext, _
                                ByVal inEventKey As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START DETAILAREA:{2} VISITSEQ:{3} PRESENTTIME:{4} EVENTKEY:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDetailArea, inVisitSeq, inPresentTime, inEventKey))


        'SC3140103DataTableAdapterのインスタンス
        Using da As New SC3140103DataTableAdapter

            '通知送信用情報取得
            Dim dtNoticeProcessingInfo As SC3140103NoticeProcessingInfoDataTable = _
                da.GetNoticeProcessingInfo(inVisitSeq, _
                                           inStaffInfo.DlrCD, _
                                           inStaffInfo.BrnCD)

            '通知送信用情報取得チェック
            If 0 < dtNoticeProcessingInfo.Count Then
                '取得できた場合

                'Rowに変換
                Dim rowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow = _
                    DirectCast(dtNoticeProcessingInfo.Rows(0), SC3140103NoticeProcessingInfoRow)

                '現在日時を設定
                rowNoticeProcessingInfo.PRESENTTIME = inPresentTime

                'イベント情報判定
                Select Case inEventKey

                    Case CType(EventKeyId.SAUndo, String)
                        'SA振当解除

                        'SA解除通知処理の実行
                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, EventKeyId.SAUndo)

                    Case CType(EventKeyId.StoreOut, String)
                        '退店

                        '退店通知処理の実行
                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, inStaffInfo, EventKeyId.StoreOut)

                        '来店管理画面と未振当一覧画面とのPush関数が同じ関数のため
                        '現在は来店管理にPushする必要なし、プレゼンで未振当一覧にはPushしているので
                        '今後来店管理の仕様が変更になった時にコメントアウトをはずす

                        ''予約の確認
                        'If 0 < rowNoticeProcessingInfo.REZID Then
                        '    '予約がある場合
                        '    '来店管理にPush処理

                        '    'OperationCodeリスト
                        '    Dim operationCodeList As New List(Of Long)

                        '    'OperationCodeリストに権限"52"：SVRを設定
                        '    operationCodeList.Add(Operation.SVR)

                        '    'ユーザーステータス取得
                        '    Using user As New IC3810601BusinessLogic

                        '        'ユーザーステータス取得処理
                        '        'SVRの全ユーザー情報取得
                        '        Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                        '            user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                        '                                         inStaffInfo.BrnCD, _
                        '                                         operationCodeList)

                        '        'SVRオンラインユーザー分ループ
                        '        For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                        '            'SVR(来店管理にPush)に対するPush処理
                        '            Me.SendPushServer(userRow.OPERATIONCODE, inStaffInfo, userRow.ACCOUNT, PushFlag1)

                        '        Next

                        '    End Using

                        'End If

                End Select

            Else
                '取得失敗

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} GetNoticeProcessingInfo IS NOTHING" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))


            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 各権限に対するPush処理
    ''' SVR/WB/CT/CHT/SA
    ''' </summary>
    ''' <param name="inOperationCode">権限コード</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inAccount">リフレッシュ先アカウント</param>
    ''' <param name="inPushFlg">PushFlag(SVR:"0":来店管理にPush無し・"1":来店管理にPush有り  SA:"0":振当エリアのPush・"1":全体のPush)</param>
    ''' <remarks></remarks>
    Public Sub SendPushServer(ByVal inOperationCode As Long, _
                              ByVal inStaffInfo As StaffContext, _
                              ByVal inAccount As String, _
                              ByVal inPushFlg As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュ文字列
        Dim pushWord As String = String.Empty

        'Push
        Dim visitUtility As New VisitUtility

        '権限毎処理の分岐
        Select Case inOperationCode

            Case Operation.SVR
                'SVR権限

                '未振当て一覧と来店管理の通知用関数は同じ名前だが
                '今後の汎用性を鑑み分岐させておく

                'PushFlagでリフレッシュ先変更
                If PushFlag0.Equals(inPushFlg) Then
                    'SVR:"0":来店管理にPush無し

                    'リフレッシュの文字列作成
                    pushWord = RefreshSVRPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                Else
                    'SVR:"1":来店管理にPush有り

                    'リフレッシュの文字列作成
                    pushWord = RefreshSVRPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                End If

            Case Operation.WBS
                'ウェルカムボード権限
                
                'Push処理実行
                visitUtility.SendPushReconstructionPC(inStaffInfo.Account, inAccount, String.Empty)

                '2度Pushしないために空文字を設定
                pushWord = String.Empty

            Case Operation.CT,
                 Operation.CHT
                'CT/CHT権限(SMB)

                'リフレッシュの文字列作成
                pushWord = RefreshCTAndCHTPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

            Case Operation.SA
                'SA権限

                'ログインアカウントと送信先アカウントの比較
                If Not inStaffInfo.Account.Equals(inAccount) Then
                    'ログインアカウントと送信先アカウントが異なる場合
                    'Push処理

                    'PushFlagでリフレッシュ先変更
                    If PushFlag0.Equals(inPushFlg) Then
                        'SA:"0":振当エリアのPush

                        'リフレッシュの文字列作成
                        pushWord = RefreshSAAssignmentPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                    Else
                        'SA:"1":全体のPush

                        'リフレッシュの文字列作成
                        pushWord = RefreshSAPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                    End If

                End If

        End Select

        'リフレッシュ文字列チェック
        If Not String.IsNullOrEmpty(pushWord) Then
            '文字列が存在してる場合

            'Push処理実行
            visitUtility.SendPush(pushWord)

        End If

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks></remarks>
    Private Sub NoticeMainProcessing(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inEventKey As EventKeyId)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))


        '送信先アカウント情報設定
        Dim account As List(Of XmlAccount) = Me.CreateAccount(inStaffInfo, inEventKey)

        '2015/08/17 TMEJ 井上 問連対応「TR-SVT-20150721-002(SvR権限への通知処理)」 START
        '送信先アカウントチェック
        If account.Count = 0 Then
            '送信先アカウントが存在しない場合
            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} account is Nothing" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Exit Sub
        End If
        '2015/08/17 TMEJ 井上 問連対応「TR-SVT-20150721-002(SvR権限への通知処理)」 END

        '通知履歴登録情報の設定
        Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeProcessingInfo, inStaffInfo, inEventKey)

        'Push情報作成処理の設定
        Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeProcessingInfo, inEventKey)

        '設定したものを格納し、通知APIをコール
        Using noticeData As New XmlNoticeData

            '現在時間データの格納
            noticeData.TransmissionDate = inRowNoticeProcessingInfo.PRESENTTIME
            '送信ユーザーデータ格納
            noticeData.AccountList.AddRange(account.ToArray)
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

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 送信先アカウント情報作成処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inStaffInfo As StaffContext, _
                                   ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'OperationCodeリスト
        Dim operationCodeList As New List(Of Long)

        'OperationCodeリストに権限"52"：SVRを設定
        operationCodeList.Add(Operation.SVR)

        'ユーザーステータス取得
        Using user As New IC3810601BusinessLogic

            'ユーザーステータス取得処理
            '各権限の全ユーザー情報取得
            Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             operationCodeList)

            'オンラインユーザー分ループ
            For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                '送信先アカウント情報
                Using account As New XmlAccount

                    '受信先のアカウント設定
                    account.ToAccount = userRow.ACCOUNT

                    '受信者名設定
                    account.ToAccountName = userRow.USERNAME

                    '送信先アカウント情報リストに送信先アカウント情報を追加
                    accountList.Add(account)

                End Using

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList


    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>通知履歴登録情報</returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                         ByVal inStaffInfo As StaffContext, _
                                         ByVal inEventKey As EventKeyId) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'XmlRequestNoticeのインスタンス
        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD

            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD

            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account

            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName

            '顧客種別(リンク制御で使用)
            Dim customerType As Integer = MessageType.NewCustomer

            '通知履歴にリンクをつけるか判定
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            '自社客チェック
            If CustsegmentMyCustomer.Equals(inRowNoticeProcessingInfo.CUSTSEGMENT) _
                AndAlso Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMSID.Trim) Then
                '自社客の場合

                '自社客設定
                customerType = MessageType.MyCustomer

            End If

            '通知履歴用メッセージ作成設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeProcessingInfo, inEventKey, customerType)

            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeProcessingInfo, customerType)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return requestNotice

        End Using

    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inCustomerType">顧客種別(リンク制御用"1"：自社客)</param>
    ''' <returns>通知履歴用メッセージ情報</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                                ByVal inEventKey As EventKeyId, _
                                                ByVal inCustomerType As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        'イベントごとに処置分岐
        Select Case inEventKey
            Case EventKeyId.SAUndo
                'SA振当解除

                '文言：振当キャンセル 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id110))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

            Case EventKeyId.StoreOut
                '退店

                '文言：退店 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id109))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

        End Select


        '自社客チェック
        If inCustomerType = MessageType.MyCustomer Then
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            '自社客のAタグを設定
            workMessage.Append(MyCustomerLink)

        End If


        'メッセージ組立：車両登録番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VCLREGNO) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            End If

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(WebWordUtility.GetWord(MsgID.id111))

        End If


        '自社客チェック
        If inCustomerType = MessageType.MyCustomer Then
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            'Aタグ終了を設定
            workMessage.Append(EndLikTag)

        End If

        'メッセージ間にスペースの設定
        workMessage.Append(Space(3))

        'メッセージ組立：予約情報：作業開始日時・作業終了日時
        If Not inRowNoticeProcessingInfo.IsSCHE_START_DATETIMENull _
            AndAlso Not inRowNoticeProcessingInfo.IsSCHE_END_DATETIMENull Then
            '作業開始日時・作業終了日時がある場合

            '作業開始日時・作業終了日時の内容チェック
            If inRowNoticeProcessingInfo.SCHE_START_DATETIME <> Date.MinValue _
                AndAlso inRowNoticeProcessingInfo.SCHE_END_DATETIME <> Date.MinValue Then
                '作業開始日時・作業終了日時がある場合

                '作業開始日時を設定
                workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowNoticeProcessingInfo.SCHE_START_DATETIME))

                '文言：～ 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id112))

                '作業終了日時を設定
                workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowNoticeProcessingInfo.SCHE_END_DATETIME))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

            End If

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
            '商品名がある場合

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

        End If


        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inCustomerType">顧客種別(リンク制御用"1"：自社客)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                                ByVal inCustomerType As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim notifySession As String = String.Empty

        'メッセージ種別判定
        Select Case inCustomerType
            Case MessageType.NewCustomer
                '「0:未取引客」の場合

                '未取引客のセッション情報を作成
                notifySession = CreateNewCustomerSession()

            Case MessageType.MyCustomer
                '「1:自社客かつ車両登録No有」の場合

                '自社客かつDMSISがあるときの通知用セッション情報作成処理
                notifySession = CreateCustomerSession(inRowNoticeProcessingInfo)

        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifySession

    End Function

    ''' <summary>
    ''' 未取引客の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNewCustomerSession() As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return Nothing

    End Function

    ''' <summary>
    ''' 自社客かつDMSISがあるときの通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCustomerSession(inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DMSIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionDMSID, inRowNoticeProcessingInfo.DMSID)

        'VINの設定
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VIN.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionVIN, inRowNoticeProcessingInfo.VIN)

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
                                         ByVal SessionValueData As String) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If workSession.Length <> 0 Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)

        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>Push情報</returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                    ByVal inEventKey As EventKeyId) As XmlPushInfo

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

            'Push用メッセージ作成
            pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

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
    ''' Push用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>Puss用メッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreatePusuMessage(ByVal inRowNoticeProcessingInfo As SC3140103NoticeProcessingInfoRow, _
                                       ByVal inEventKey As EventKeyId) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        'イベントごとに処置分岐
        Select Case inEventKey
            Case EventKeyId.SAUndo
                'SA振当解除

                '文言：振当キャンセル 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id110))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

            Case EventKeyId.StoreOut
                '退店

                '文言：退店 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id109))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

        End Select

        'メッセージ組立：車両登録番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VCLREGNO) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            End If

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(WebWordUtility.GetWord(MsgID.id111))

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：予約情報：作業開始日時・作業終了日時
        If Not inRowNoticeProcessingInfo.IsSCHE_START_DATETIMENull _
            AndAlso Not inRowNoticeProcessingInfo.IsSCHE_END_DATETIMENull Then
            '作業開始日時・作業終了日時がある場合

            '作業開始日時・作業終了日時の内容チェック
            If inRowNoticeProcessingInfo.SCHE_START_DATETIME <> Date.MinValue _
                AndAlso inRowNoticeProcessingInfo.SCHE_END_DATETIME <> Date.MinValue Then
                '作業開始日時・作業終了日時がある場合

                '作業開始日時を設定
                workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowNoticeProcessingInfo.SCHE_START_DATETIME))

                '文言：～ 設定
                workMessage.Append(WebWordUtility.GetWord(MsgID.id112))

                '作業終了日時を設定
                workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowNoticeProcessingInfo.SCHE_END_DATETIME))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

            End If

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
            '商品名がある場合

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

        End If


        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ' ''' <summary>
    ' ''' 通知履歴用セッション情報作成メソッド
    ' ''' </summary>
    ' ''' <param name="inRowVisitInfo">来店者情報表示欄</param>
    ' ''' <param name="inStaffInfo">ログイン情報</param>
    ' ''' <param name="inKindNumber">メッセージ種別「0:未取引客、1:自社客かつ車両登録No有、2:自社客かつ車両登録No無」</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function CreateNoticeRequestSession(ByVal inRowVisitInfo As SC3140103NoticeProcessingInfoRow, _
    '                                            ByVal inStaffInfo As StaffContext, _
    '                                            ByVal inKindNumber As MessageType) As String

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim notifySession As String = String.Empty

    '    'メッセージ種別判定
    '    Select Case inKindNumber
    '        Case MessageType.NewCustomer
    '            '「0:未取引客」の場合

    '            '未取引客のセッション情報を作成
    '            notifySession = CreateNewCustomerSession(inRowVisitInfo)

    '        Case MessageType.CustomerRegsterNo
    '            '「1:自社客かつ車両登録No有」の場合

    '            '自社客のセッション情報を作成
    '            notifySession = CreateCustomerSession(inRowVisitInfo, inStaffInfo)

    '    End Select

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return notifySession

    'End Function

    ' ''' <summary>
    ' ''' 未取引客の通知用セッション情報作成メソッド
    ' ''' </summary>
    ' ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ' ''' <returns>戻り値</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function CreateNewCustomerSession(ByVal inRowVisitInfo As SC3140103NoticeProcessingInfoRow) As String

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim workSession As New StringBuilder

    '    '来店管理連番のセッション値設定
    '    Me.SetSessionValueWord(workSession, _
    '                           SessionValueVisitSequence, _
    '                           inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture))

    '    '名前の設定
    '    If Not inRowVisitInfo.IsNAMENull Then
    '        '名前がある場合

    '        '名前のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueName, inRowVisitInfo.NAME)

    '    End If

    '    '車両登録番号の設定
    '    If Not inRowVisitInfo.IsVCLREGNONull Then
    '        '車両登録番号がある場合は設定

    '        '車両登録Noのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueRegisterNo, inRowVisitInfo.VCLREGNO)

    '    End If

    '    'VINの設定
    '    If Not inRowVisitInfo.IsVINNull Then
    '        'VINがある場合は設定

    '        'VINのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

    '    End If

    '    'モデルコードの設定
    '    If Not inRowVisitInfo.IsMODELCODENull Then
    '        'モデルコードがある場合は設定

    '        'モデルコードのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueModelCode, inRowVisitInfo.MODELCODE)

    '    End If

    '    '電話番号の設定
    '    If Not inRowVisitInfo.IsTELNONull Then
    '        '電話番号がある場合は設定

    '        '電話番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo1, inRowVisitInfo.TELNO)

    '    End If

    '    '携帯番号の設定
    '    If Not inRowVisitInfo.IsMOBILENull Then
    '        '携帯番号がある場合は設定

    '        '携帯番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo2, inRowVisitInfo.MOBILE)

    '    End If

    '    '予約IDの設定
    '    If Not inRowVisitInfo.IsREZIDNull _
    '        AndAlso inRowVisitInfo.REZID >= 0 Then
    '        '予約IDがある場合は設定

    '        '予約IDのセッション値作成
    '        Me.SetSessionValueWord(workSession, _
    '                               SessionValueReserveId, _
    '                               inRowVisitInfo.REZID.ToString(CultureInfo.CurrentCulture))

    '    Else
    '        '予約IDがない場合は空を設定

    '        '予約IDのセッション値作成(空文字)
    '        Me.SetSessionValueWord(workSession, SessionValueReserveId, String.Empty)

    '    End If

    '    '事前準備フラグのセッション値設定
    '    Me.SetSessionValueWord(workSession, SessionValuePrepareChipType, RepairOrder)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return workSession.ToString

    'End Function

    ' ''' <summary>
    ' ''' 自社客かつ車両登録NOがあるときの通知用セッション情報作成メソッド
    ' ''' </summary>
    ' ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ' ''' <param name="inStaffInfo">ログイン情報</param>
    ' ''' <returns>戻り値</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function CreateCustomerSession(ByVal inRowVisitInfo As SC3140103NoticeProcessingInfoRow, _
    '                                       ByVal inStaffInfo As StaffContext) As String

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim workSession As New StringBuilder

    '    '来店管理連番のセッション値設定
    '    Me.SetSessionValueWord(workSession, _
    '                           SessionValueVisitSequence, _
    '                           inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture))

    '    '車両登録番号のセッション値設定
    '    Me.SetSessionValueWord(workSession, SessionValueRegisterNo, inRowVisitInfo.VCLREGNO)

    '    'VINの設定
    '    If Not inRowVisitInfo.IsVINNull Then
    '        'VINがある場合は設定

    '        'VINのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

    '    End If

    '    'モデルコードの設定
    '    If Not inRowVisitInfo.IsMODELCODENull Then
    '        'モデルコードがある場合は設定

    '        'モデルコードのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueModelCode, inRowVisitInfo.MODELCODE)

    '    End If

    '    '電話番号の設定
    '    If Not inRowVisitInfo.IsTELNONull Then
    '        '電話番号がある場合は設定

    '        '電話番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo1, inRowVisitInfo.TELNO)

    '    End If

    '    '携帯番号の設定
    '    If Not inRowVisitInfo.IsMOBILENull Then
    '        '携帯番号がある場合は設定

    '        '携帯番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo2, inRowVisitInfo.MOBILE)

    '    End If

    '    '予約IDの設定
    '    If Not inRowVisitInfo.IsREZIDNull _
    '        AndAlso inRowVisitInfo.REZID >= 0 Then
    '        '予約IDがある場合は設定

    '        '予約IDのセッション値作成
    '        Me.SetSessionValueWord(workSession, _
    '                               SessionValueReserveId, _
    '                               inRowVisitInfo.REZID.ToString(CultureInfo.CurrentCulture))

    '    Else
    '        '予約IDがない場合は空を設定

    '        '予約IDのセッション値作成(空文字)
    '        Me.SetSessionValueWord(workSession, SessionValueReserveId, String.Empty)

    '    End If

    '    '販売店コードのセッション設定
    '    Me.SetSessionValueWord(workSession, SessionValueDealerCode, inStaffInfo.DlrCD)

    '    '顧客詳細フラグのセッション設定
    '    Me.SetSessionValueWord(workSession, SessionValueType, CustsegmentMyCustomer)

    '    '事前準備フラグのセッション設定
    '    Me.SetSessionValueWord(workSession, SessionValuePrepareChipType, RepairOrder)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return workSession.ToString

    'End Function

    ' ''' <summary>
    ' ''' SessionValue文字列作成
    ' ''' </summary>
    ' ''' <param name="workSession">追加元文字列</param>
    ' ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ' ''' <param name="SessionValueData">追加するデータ</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
    '                                     ByVal SessionValueWord As String, _
    '                                     ByVal SessionValueData As String) As StringBuilder

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'カンマの設定
    '    If workSession.Length <> 0 Then
    '        'データがある場合

    '        '「,」を結合する
    '        workSession.Append(SessionValueKanma)

    '    End If

    '    'セッションキーを設定
    '    workSession.Append(SessionValueWord)

    '    'セッション値を設定
    '    workSession.Append(SessionValueData)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return workSession

    'End Function

#End Region

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
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
