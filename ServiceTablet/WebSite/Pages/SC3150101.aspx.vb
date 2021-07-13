'------------------------------------------------------------------------------
'SC3150101.aspx.vb
'------------------------------------------------------------------------------
'機能：メインメニュー（TC）
'補足：
'作成：2012/01/28 KN 渡辺
'更新：2012/03/02 KN 上田    【SERVICE_1】課題管理番号-BMTS_0229_YW_02の不具合修正(フッタボタン制御)
'更新：2012/03/16 KN 森下    【SERVICE_1】課題管理番号-KN_0315_YM_1の不具合修正(完成検査入力画面の遷移時修正)
'更新：2012/03/21 KN 上田    仕様変更対応(追加作業関連の遷移先変更)  
'更新：2012/03/24 KN 上田    仕様変更対応(完成検査入力の受け渡し枝番変更対応)  
'更新：2012/03/26 KN 上田    仕様変更対応(追加作業関連の遷移先再変更)  
'更新：2012/03/27 KN 森下    【SERVICE_1】システムテストの不具合修正No79 部品連絡ポップアップに部品が表示されない。
'更新：2012/03/28 KN 西田    【SERVICE_1】システムテストの不具合修正No76 作業進捗のR/O No.の表示誤り
'更新：2012/04/05 KN 西田    【SERVICE_1】プレユーザーテスト課題No.78 追加作業入力に編集モードで遷移できない
'更新：2012/04/09 KN 西田    【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加
'更新：2012/04/12 KN 西田    【SERVICE_1】ユーザーテスト課題No.7 他画面に遷移した後、TCに戻ると遷移前のチップが選択されない
'更新：2012/04/17 KN 西田    【SERVICE_1】ユーザーテスト課題No.7 他画面に遷移した後、TCに戻ると遷移前のチップが選択されない
'更新：2012/04/18 KN 日比野  【SERVICE_1】企画_プレユーザーテスト課題No.307 戻るボタンが非活性となっていない。
'更新：2012/06/01 KN 西田    STEP1 重要課題対応
'更新：2012/06/05 KN 彭      コード分析対応
'更新：2012/06/14 KN 西田    STEP1 重要課題対応 DevPartner指摘対応
'更新：2012/06/15 KN 西田    STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない
'更新：2012/07/25 KN 彭      【SERVICE_1】仕分け課題対応
'更新：2012/08/01 KN 彭      サービス緊急対応（FMへの呼び出し通知機能を追加）→GTMC専用機能
'更新：2012/08/14 KN 彭      SAストール予約受付機能開発（No.27カゴナンバー表示）
'更新：2012/11/05 TMEJ 彭    問連修正（GTMC121029047）
'更新：2012/11/14 TMEJ 彭健  アクティブインジゲータ対応（クルクルのタイムアウト対応）
'更新：2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
'更新：2013/02/26 TMEJ 成澤 【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニター起動待機時間の取得)
'更新：2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)
'更新：2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発
'更新：2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正
'更新：2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新：2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移
'更新：2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
'更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
'更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新：2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'Imports Toyota.eCRB.iCROP.BizLogic.IC3810501
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
Imports Toyota.eCRB.iCROP.BizLogic.SC3150101
Imports Toyota.eCRB.iCROP.DataAccess.SC3150101
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
Imports Toyota.eCRB.iCROP.BizLogic.SC3150102
Imports Toyota.eCRB.Technician.MainMenu
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

'2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
'2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

Partial Class Pages_Default
    Inherits BasePage

    ''' セッションキー
    Public Const SESSION_KEY_STALL_ID As String = "SC3150101.StallId"  'ストールID


#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150101"
    '2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
    ''' <summary>
    ''' 他店R/Oプレビュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_PREVIEW_PAGE_FOR_OTHER_DLR As String = "SC3080223"
    '2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END
    ''' <summary>
    ''' R/Oプレビュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_PREVIEW_PAGE As String = "SC3160208"
    ''' <summary>
    ''' 追加作業依頼書プレビュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_REPAIR_PREVIEW_PAGE As String = "SC3170302"
    ' ''' <summary>
    ' ''' 部品連絡画面ID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PARTS_CONTACT_PAGE_ID = "SC3190303"
    ''' <summary>
    ''' 追加作業一覧ページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_LIST_ID As String = "SC3170101"
    ''' <summary>
    ''' TC起票追加作業登録画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_PAGE_ID As String = "SC3170203"
    ''' <summary>
    ''' 完成検査一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMPLETION_CHECK_PAGE_ID As String = "SC3180101"
    ''' <summary>
    ''' 完成検査チェックシート入力画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMPLETION_CHECK_INPUT_PAGE_ID As String = "SC3180204"

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    ''' <summary>
    ''' SA起票追加作業登録画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_SA_PAGE_ID As String = "SC3170201"
    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
    ''' <summary>
    ''' プログラムID：TCメイン画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_TEC As String = "SC3150101"
    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END

    ''' <summary>
    ''' TCステータスモニター画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Status_Monitor_PAGE_ID As String = "SC3150201"

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発　START
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_FM As String = "SC3230101"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE_ID As String = "SC3240101"
    ''' <summary>
    ''' サービス用共通関数処理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERVICE_COMMON_PAGE_ID As String = "SC3010501"

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発　END

    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100

    ''' <summary>
    ''' 追加作業起票画面セッション：追加作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITIONAL_WORK_SEND_VALUE As String = "New"
    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発　START

    ' ''' <summary>
    ' ''' フッターコード：カスタマー
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_CUSTOMER As Integer = 200
    '' ''' <summary>
    '' ''' フッターコード：TCV
    '' ''' </summary>
    '' ''' <remarks></remarks>
    ''Private Const FOOTER_TCV As Integer = 300
    ' ''' <summary>
    ' ''' フッターコード：追加作業（サブ）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_ADDITIONAL_WORK As Integer = 1100
    ' ''' <summary>
    ' ''' フッターコード：追加作業（サブ）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_SUB_ADDITIONAL_WORK As Integer = 1101
    ' ''' <summary>
    ' ''' フッターコード：完成検査
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const FOOTER_COMP_EXAM As Integer = 1000
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

    ''' <summary>
    ''' フッターコード：TCメインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TC_MAIN_MEUN As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_FM_MAIN_MEUN As Integer = 300
    ''' <summary>
    ''' フッターコード：R/O一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RO_LIST As Integer = 500
    ''' <summary>
    ''' フッターコード：電話帳
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = 600
    ''' <summary>
    ''' フッターコード：追加作業一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADDITIONAL_WORK_LIST As Integer = 1200

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発　END

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
    ''' 押したフッタボタンの状態：開始処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_START_WORK = "1"
    ''' <summary>
    ''' 押したフッタボタンの状態：当日処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_SUSPEND_WORK = "3"
    ''' <summary>
    ''' 押したフッタボタンの状態：検査開始
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_START_CHECK = "4"
    ' ''' <summary>
    ' ''' 押したフッタボタンの状態：部品連絡
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PUSHED_FOOTER_BUTTON_CONNECT_PARTS = "5"

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) 　START

    ''' <summary>
    ''' 押したフッタボタンの状態：作業終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_FINISH_WORK = "6"
    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)　END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
    ''' <summary>
    ''' 押したフッタボタンの状態：作業中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_STOP_WORK = "7"
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

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
    ''' 開始イベントのエラーコード：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_CODE_START_WORK_SUCCESSFULL As Integer = 0
    ''' <summary>
    ''' R/O作業ステータス：受付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_RECEPTION As String = "1"
    ''' <summary>
    ''' R/O作業ステータス：見積確定待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_WAITING_ESTIMATE As String = "5"
    ''' <summary>
    ''' R/O作業ステータス：部品待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_WAITING_PARTS As String = "4"
    ''' <summary>
    ''' R/O作業ステータス：整備中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_ON_WORK_ORDER As String = "2"
    ''' <summary>
    ''' R/O作業ステータス：検査完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_INSPECTION As String = "7"
    ''' <summary>
    ''' R/O作業ステータス：整備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_WORK As String = "6"
    ''' <summary>
    ''' R/O作業ステータス：売上済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_SALES As String = "3"
    ''' <summary>
    ''' R/O作業ステータス：納車完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_DELIVERY = "8"
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21

    ''' <summary>
    ''' 実績ステータス：待機中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_WAIT As String = "1"
    ''' <summary>
    ''' 実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_WORKING As String = "2"
    ''' <summary>
    ''' 実績ステータス：作業完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_COMP As String = "3"

    ''' <summary>
    ''' ステータス：ストール本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_RESERVE As Integer = 1
    ''' <summary>
    ''' ステータス：ストール仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_TEMP_RESERVE As Integer = 0
    ''' <summary>
    ''' ステータス：引取納車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_DELIVALY As Integer = 4
    ''' <summary>
    ''' ステータス：使用不可
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_UNAVAILABLE As Integer = 3
    ''' <summary>
    ''' ステータス：休憩
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_REST As Integer = 99

    ' ''' <summary>
    ' ''' ポストバックをしたことを示すフラグ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const POSTBACK_TRUE As String = "1"

    ''' <summary>
    ''' チップ選択がなされてない状態を示す
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SELECTED_CHIP_OFF As String = "0"
    ''' <summary>
    ''' チップ選択がなされている状態を示す
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SELECTED_CHIP_ON As String = "1"

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    ''' <summary>
    ''' 追加作業ステータス：TC起票中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_VOUCHER As String = "1"
    ''' <summary>
    ''' 追加作業ステータス：CT承認待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_CONSENT As String = "2"
    ''' <summary>
    ''' 追加作業ステータス：PS部品見積待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_PARTS As String = "3"
    ''' <summary>
    ''' 追加作業ステータス：SA見積確定待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_ESTIMATE As String = "4"
    ''' <summary>
    ''' 追加作業ステータス：顧客承認待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_CUSTOMER As String = "5"
    ''' <summary>
    ''' 追加作業ステータス：CT着工指示／PS部品出荷待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_SHIPPING As String = "6"
    ''' <summary>
    ''' 追加作業ステータス：TC作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_WAITING_START As String = "7"
    ''' <summary>
    ''' 追加作業ステータス：整備中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_ON_WORK_ORDER As String = "8"
    ''' <summary>
    ''' 追加作業ステータス：完成検査完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_STATUS_COMPLETE_INSPECTION As String = "9"

    ''' <summary>
    ''' 追加作業の起票者：SA
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_DRAWER_SA As String = "2"

    ''' <summary>
    ''' 追加作業の起票者：TC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_DRAWER_TC As String = "1"

    ''' <summary>
    ''' 追加作業入力画面遷移時パターン：新規
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_NEW As String = "0"

    ''' <summary>
    ''' 追加作業入力画面遷移時パターン：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_EDIT As String = "1"

    ''' <summary>
    ''' 追加作業入力画面遷移時パターン：警告
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_WORK_WORNING As String = "-1"

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' SC3170201用編集フラグ(0: 新規/編集)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SC3170201_EDIT_FLAG_NEW_EDIT As String = "0"
    ' ''' <summary>
    ' ''' SC3170203用編集フラグ(0: 編集)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SC3170203_NEW_EDIT_FLAG_EDIT As String = "0"
    ' ''' <summary>
    ' ''' SC3170203用編集フラグ(1: 参照)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SC3170203_EDIT_FLAG_PREVIEW As String = "1"

    ''' <summary>
    ''' C3010501用編集フラグ(0: 新規/編集)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SC3010501_EDIT_FLAG_NEW_EDIT As String = "0"
    ''' <summary>
    ''' 0：完成検査依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_0 As String = "0"
    ''' <summary>
    ''' 13：ROプレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_13 As String = "13"
    ''' <summary>
    ''' 25：ROプレビュー(過去)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_25 As String = "25"
    ''' <summary>
    ''' 14：R/O一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_14 As String = "14"
    ''' <summary>
    ''' 22：追加作業一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_22 As String = "22"
    ''' <summary>
    ''' 23：追加作業起票
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_23 As String = "23"



    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
    ''' <summary>
    ''' 作業連番
    ''' 0：未計画
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORKSEQ_NOPLAN_PARENT As String = "0"

    ''' <summary>
    ''' 戻り値：OK
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RETURN_VALUE_OK As Integer = 0
    ''' <summary>
    ''' 戻り値：NG（当日処理）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RETURN_VALUE_NG_SUSPEND As Integer = 907

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
    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

    '2012/06/15 KN 西田 STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない START
    ''' <summary>
    ''' 作業終了フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_END_FLG As String = "1"
    '2012/06/15 KN 西田 STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない END

    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 START
    ''' <summary>
    ''' TCステータスモニター自動遷移時間の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Default_StatusStand_Time As Integer = 180
    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 END

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' オペレーションコード：テクニシャン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_CODE_TC As Integer = 14
    ''' <summary>
    ''' オペレーションコード：チーフテクニシャン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_CODE_CHT As Integer = 62
    ''' <summary>
    ''' RO_ステータス：着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_STATUS_CUSTOMER_APPROVAL As String = "50"
    ''' <summary>
    ''' RO_ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_STATUS_WORKING As String = "60"
    ''' <summary>
    ''' RO_ステータス：納車準備待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_STATUS_DELIVERY As String = "80"

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
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
    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END


    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
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

    ''' <summary>
    ''' 中断理由区分：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STOP_REASON_TYPE_OTHER As String = "99"

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

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

#End Region

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
#Region "列挙体定義"
    ''' <summary>
    ''' 追加作業関連画面列挙体
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AddWorkRedirect
        ''' <summary>追加作業一覧</summary>
        SC3170101
        ''' <summary>SA起票_追加作業入力(新規)</summary>
        SC3170201_New
        ''' <summary>SA起票_追加作業入力(編集)</summary>
        SC3170201_Edit
        ''' <summary>TC起票_追加作業入力(新規)</summary>
        SC3170203_New
        ''' <summary>TC起票_追加作業入力(編集)</summary>
        SC3170203_Edit
        ''' <summary>追加作業入力(参照)</summary>
        SC3170203_Preview
        ''' <summary>ワーニング(TC起票)</summary>
        Warning_TC
        ''' <summary>ワーニング(SA起票)</summary>
        Warning_SA
        ''' <summary>対象外</summary>
        Invalid
    End Enum
#End Region
    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

#Region "メンバ変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3150101BusinessLogic

    ''' <summary>
    ''' ログイン中のストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private stallId As Decimal
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

    '2012/03/16 KN森下 【SERVICE_1】課題管理番号-KN_0315_YM_1の不具合修正(完成検査入力画面の遷移時修正) START
    ''' <summary>
    ''' R/O情報欄のフリック時フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private flickRoInformationFlag As Integer = 0
    '2012/03/16 KN森下 【SERVICE_1】課題管理番号-KN_0315_YM_1の不具合修正(完成検査入力画面の遷移時修正) END

    ''' <summary>
    ''' サービスコモンのインスタンス
    ''' </summary>
    ''' <remarks></remarks>
    Private serviceCommon As New ServiceCommonClassBusinessLogic
#End Region

#Region "初期処理"
    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SESSION情報にSC3150102より先にアクセスすることで、「戻る」実施時の戻り先をSC3150101に設定する.
        MyBase.ContainsKey(ScreenPos.Current, "Redirect.ORDERNO")

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
        Dim InStallId As Decimal = 0

        'タブレットSMBより遷移してきた場合のセッションの取得
        If MyBase.ContainsKey(ScreenPos.Current, SESSION_KEY_STALL_ID) Then
            'セッションの値を取得
            InStallId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, False), Decimal)

            '現地画面より遷移してきた場合のセッションの取得
        ElseIf MyBase.ContainsKey(ScreenPos.Last, SESSION_KEY_STALL_ID) Then
            'セッションの値を取得
            InStallId = DirectCast(GetValue(ScreenPos.Last, SESSION_KEY_STALL_ID, False), Decimal)
        End If


        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current
        'フッタボタンの初期化を行う.
        InitFooterButton()
        'ログインアカウントよりストール情報を取得する.
        SetStallInfo(InStallId)
        'サーバ時間を取得し、設定する.
        SetServerCurrentTime()
        'TCステータスモニター起動までの待機時間設定.
        HiddenStatusStandTimeValue()

        AddWorkButtonView()
        '初回呼び出し時の処理を実施する.
        If (Not Page.IsPostBack) Then

            '以前のセッション値の影響を受けて中途半端に表示される問題を回避するために、Redirect.SELECTED_ID以外のセッション値を一旦クリアする
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.ORDERNO")
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.SRVADDSEQ")     ' TACT側の枝番
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.FILTERFLG")
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.WORKSEQ")       ' 作業連番
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.REZID")         ' 予約ID
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.INSTRUCT")      ' 着工指示区分
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.VCLREGNO")      ' 車輌登録番号(FMへの呼出し通知用)
            MyBase.RemoveValue(ScreenPos.Current, "Redirect.STALLNAME")     ' ストール名(FMへの呼出し通知用)

            '初回表示時にHiddenにデータを格納する.
            PageLoadInit()

            ''サーバよりチップ情報を取得する.
            GetChipDataFromServer()

            '他画面に遷移した後、TCに戻ると遷移前のチップが選択されるように
            Me.SetSelectedChipRedirectBefore()

            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            '中断理由ウィンドウに中断メモのテンプレートをバインドする
            Me.BindStopMemoTemplate()
            'ポップアップ文言取得
            Me.GetMessegeBoxWord()
            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' 初回ページ読込時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageLoadInit()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Javascript用
        '作業進捗の文言の取得.
        Me.HiddenStartTimeWord.Value = WebWordUtility.GetWord(7)
        Me.HiddenEndTimeWord.Value = WebWordUtility.GetWord(8)
        Me.HiddenResultStartTimeWord.Value = WebWordUtility.GetWord(25)
        Me.HiddenResultEndTimeWord.Value = WebWordUtility.GetWord(26)
        'チップの休憩・使用不可に表示する文字列.
        Me.HiddenRestText.Value = WebWordUtility.GetWord(11)
        Me.HiddenUnavailableText.Value = WebWordUtility.GetWord(20)
        '日跨ぎエラー文字列.
        Me.HiddenWarnNextDate.Value = WebWordUtility.GetWord(904)
        '部品連絡ポップアップに使用する文言.
        Me.HiddenPopupPartsCancelWord.Value = WebWordUtility.GetWord(18)
        Me.HiddenPopupPartsTitleWord.Value = WebWordUtility.GetWord(17)

        '追加作業時の確認メッセージ
        Me.HiddenAddWorkConfirmWord.Value = WebWordUtility.GetWord(922)      '「确定制作追加R/O吗？」

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' ログインアカウントよりストール情報を取得し、テキストに格納する.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetStallInfo(Optional ByVal inStallId As Decimal = 0)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ストール情報の取得.
        Dim stallDataTable As SC3150101DataSet.SC3150101BelongStallInfoDataTable
        stallDataTable = businessLogic.GetBelongStallData(inStallId)
        'ストール時間に初期値を格納する.
        Me.stallActualStartTime = DateTimeFunc.Now(objStaffContext.DlrCD).Date
        Me.stallActualEndTime = Me.stallActualStartTime.AddDays(1)

        'ストール情報を設定.
        Dim strStallName As String = ""
        For Each eachStallData As DataRow In stallDataTable

            Me.stallId = CType(eachStallData("STALLID"), Decimal)
            'Logger.Info("SetStallInfo StallInfo Roop_StallID:" + CType(Me.stallId, String))

            strStallName = CType(eachStallData("STALLNAME"), String)
            Me.stallActualStartTime = ExchangeStallHourToDate(CType(eachStallData("PSTARTTIME"), String))
            Me.stallActualEndTime = ExchangeStallHourToDate(CType(eachStallData("PENDTIME"), String))
            'ストール時間が、開始時間より終了時間が小さくなってしまう場合、終了時間に1日加算する.
            If (Me.stallActualEndTime < Me.stallActualStartTime) Then
                'Logger.Info("SetStallInfo StallInfo Roop If stallActualEndTime < stallActualStartTime")
                Me.stallActualEndTime = Me.stallActualEndTime.AddDays(1)
            End If

        Next

        '取得したストール情報より、エンジニア名を取得する.
        Dim stallStaffDataTable As SC3150101DataSet.SC3150101BelongStallStaffDataTable
        stallStaffDataTable = businessLogic.GetBelongStallStaffData(Me.stallId)

        'エンジニア名を設定.
        Dim strEngineerNameText As New System.Text.StringBuilder
        For Each eachStaffName As DataRow In stallStaffDataTable

            Dim staffName As String
            staffName = CType(eachStaffName("USERNAME"), String)
            'Logger.Info("SetStallInfo Roop_Engineer EngineerName:" + staffName)

            'エンジニア名が既に格納されている場合、エンジニア名の分割文字を追加する.
            If (0 < strEngineerNameText.Length) Then
                'Logger.Info("SetStallInfo Roop_Engineer If Engineers")
                strEngineerNameText.Append(WebWordUtility.GetWord(3))
            End If

            strEngineerNameText.Append(staffName)
        Next

        '取得したストール情報を表示する.
        LabelStallName.Text = strStallName
        LabelEngineerName.Text = strEngineerNameText.ToString()

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
        'HiddenStallStartTime.Value = DateTimeFunc.FormatDate(2, Me.stallActualStartTime)
        'HiddenStallEndTime.Value = DateTimeFunc.FormatDate(2, Me.stallActualEndTime)
        HiddenStallStartTime.Value = Me.stallActualStartTime.ToString(CultureInfo.CurrentCulture())
        HiddenStallEndTime.Value = Me.stallActualEndTime.ToString(CultureInfo.CurrentCulture())
        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        Me.HiddenOpretionCode.Value = objStaffContext.OpeCD.ToString()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub


    ''' <summary>
    ''' ストール時間を取得し、Date型に変換する
    ''' </summary>
    ''' <param name="stallHour">5桁の（HH:mm）形式の時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeStallHourToDate(ByVal stallHour As String) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START,param1:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , stallHour))

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
        ''返す値の初期値として、当日の0時を設定する.
        'Dim stallDate As Date = DateTimeFunc.Now(objStaffContext.DlrCD).Date

        'Dim stallDateString As New System.Text.StringBuilder

        '当日日付を追加
        'stallDateString.Append(DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYY_MM_DD, stallDate))
        'stallDateString.Append(" ")
        'stallDateString.Append(stallHour.Substring(0, 5))

        '生成した文字列を使用して、日付型データを取得する.

        'stallDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", stallDateString.ToString())

        '時間と分に分割する
        Dim hourUnit As String() = stallHour.Split(":"c)

        '返す値の初期値として、当日の0時を設定する.
        Dim stallDate As Date = DateTimeFunc.Now(objStaffContext.DlrCD).Date

        '分割した時間と分を本日の日付に足す
        stallDate = stallDate.AddHours(Double.Parse(hourUnit(0), CultureInfo.InvariantCulture))
        stallDate = stallDate.AddMinutes(Double.Parse(hourUnit(1), CultureInfo.InvariantCulture))
        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} START,return:{2}" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                 , DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, stallDate)))

        Return stallDate

    End Function


    ''' <summary>
    ''' 現在のサーバ時間をHiddenFieldにセットする.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetServerCurrentTime()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

        'サーバ時間を文字列として取得して、HiddenFieldに格納.（yyyy/MM/dd HH:mm:ss形式）
        'Me.HiddenServerTime.Value = DateTimeFunc.FormatDate(1, DateTimeFunc.Now(objStaffContext.DlrCD))
        Me.HiddenServerTime.Value = DateTimeFunc.Now(objStaffContext.DlrCD).ToString(CultureInfo.CurrentCulture())

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END SetTime:{2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , Me.HiddenServerTime.Value))

    End Sub

    '2012/04/11 KN 西田  ユーザーテスト課題No.7 他画面に遷移した後、TCに戻ると遷移前のチップが選択されない START

    Private Sub SetSelectedChipRedirectBefore()

        If MyBase.ContainsKey(ScreenPos.Current, "Redirect.SELECTED_ID") Then
            Me.HiddenSelectedId.Value = MyBase.GetValue(ScreenPos.Current, "Redirect.SELECTED_ID", False).ToString().Trim()
            '他画面から戻る際に前回と同じチップをタップしたような動きになるので、一度チップが選択されている未状態にする。（クライアント側で選択される）
            Me.HiddenSelectedChip.Value = "1"
        End If
    End Sub

    '2012/04/11 KN 西田  ユーザーテスト課題No.7 他画面に遷移した後、TCに戻ると遷移前のチップが選択されない END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断理由を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStopMemoTemplate()
        Dim dtStopMemo As SC3150101DataSet.SC3150101StopMemoTempDataTable = _
            businessLogic.GetStopMemoTemplate(objStaffContext.DlrCD, objStaffContext.BrnCD)

        Me.dpDetailStopMemo.Items.Clear()
        Me.dpDetailStopMemo.DataSource = dtStopMemo
        Me.dpDetailStopMemo.DataTextField = "STOP_MEMO_TEMPLATE"
        Me.dpDetailStopMemo.DataBind()
        Me.dpDetailStopMemo.Items.Insert(0, WebWordUtility.GetWord("SC3150101", 41))

        Me.lblDetailStopMemo.Text = WebWordUtility.GetWord("SC3150101", 41)

    End Sub

    ''' <summary>
    ''' ポップアップ用文言取得、格納
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetMessegeBoxWord()

        'エラーIDにより、エラー文言を取得
        Dim strStartMsg As String = WebWordUtility.GetWord(APPLICATION_ID, 940)
        Dim strFinishMsg As String = WebWordUtility.GetWord(APPLICATION_ID, 939)
        Dim strMinMsg As String = WebWordUtility.GetWord(APPLICATION_ID, 39)


        Me.HiddenConfirmStartWording.Value = strStartMsg
        Me.HiddenConfirmFinishWording.Value = strFinishMsg
        Me.HiddenStopTimeWord.Value = strMinMsg

    End Sub
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

#End Region

#Region "チップ情報の取得処理"
    ''' <summary>
    ''' チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetChipDataFromServer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ情報の最新を取得し、作業対象チップを設定する
        Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtChipInfo = GetNewestChipInfo()
        GetCandidateChipInfo(dtChipInfo)

        '受信したデータをJSON形式に変換する
        Dim chipDataJson As String
        chipDataJson = businessLogic.DataTableToJson(dtChipInfo)
        Logger.Debug("GetChipDataFromServer ChipData:" + chipDataJson)

        '取得したJSON形式のデータをHiddenに格納する
        Me.HiddenJsonData.Value = chipDataJson

        'End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' データベースより最新のチップ情報を取得する
    ''' </summary>
    ''' <returns>StallInfoDataSet.CHIPINFODataTable:差分チップデータ</returns>
    ''' <remarks></remarks>
    Private Function GetNewestChipInfo() As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '休憩チップ情報を取得し、先のチップ情報に追加する.
        Dim dtBreakChipData As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtBreakChipData = businessLogic.GetBreakData(Me.stallId)

        ''使用不可チップ情報を取得し、先のチップ情報に追加する.
        Dim dtUnavailableChipData As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtUnavailableChipData = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)
        dtBreakChipData.Merge(dtUnavailableChipData, False)

        '最新のチップ情報を取得する.
        Dim dtNewestChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable

        dtNewestChipInfo = businessLogic.GetStallChipInfo(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        'チップデータ取得時にエラーが発生した場合
        If (Not IsNothing(dtNewestChipInfo)) AndAlso _
            (Not dtNewestChipInfo.Rows.Count = 0) AndAlso _
            (dtNewestChipInfo.Item(0).ERROR_CODE <> 0) Then
            'エラーメッセージ表示
            MyBase.ShowMessageBox(dtNewestChipInfo.Item(0).ERROR_CODE)
        End If

        dtBreakChipData.Merge(dtNewestChipInfo, False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtBreakChipData

    End Function

    ''' <summary>
    ''' 作業対象チップ情報を特定し、作業対象チップ情報行を返す.
    ''' </summary>
    ''' <param name="dtData">作業チップデータセット</param>
    ''' <returns>作業対象チップ情報行</returns>
    ''' <remarks></remarks>
    Private Function GetCandidateChipInfo(ByVal dtData As SC3150101DataSet.SC3150101ChipInfoDataTable) As SC3150101DataSet.SC3150101ChipInfoRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim drCandidateWorkInfo As SC3150101DataSet.SC3150101ChipInfoRow = Nothing
        Dim dtmOldestStartTime As DateTime = DateTime.MaxValue

        For Each drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow In dtData.Rows

            'チップ情報が、本予約・仮予約・引取納車のいずれかの場合、判定処理を実施する.
            If ((drChipInfo.STATUS = STATUS_RESERVE) Or (drChipInfo.STATUS = STATUS_TEMP_RESERVE) Or (drChipInfo.STATUS = STATUS_DELIVALY)) Then

                Dim dtmChipStartTime As DateTime
                dtmChipStartTime = CType(drChipInfo("STARTTIME"), Date)

                If (IsDBNull(drChipInfo("RESULT_STATUS"))) OrElse RESULT_STATUS_WAIT.Equals(drChipInfo("RESULT_STATUS")) Then
                    'チップの実績ステータスがない、または、待機中である場合
                    '該当のレコードの開始時間（予定）を取得し、現在所持している時間と比較して小さい場合、更新する
                    If (dtmChipStartTime < dtmOldestStartTime) Then
                        dtmOldestStartTime = dtmChipStartTime
                        drCandidateWorkInfo = drChipInfo
                    End If
                ElseIf RESULT_STATUS_WORKING.Equals(drChipInfo("RESULT_STATUS")) Then
                    'チップの実績ステータスが作業中である場合、該当のレコードを作業対象に設定し、ループを抜ける
                    drCandidateWorkInfo = drChipInfo
                    Exit For
                End If
            End If
        Next

        '取得したチップ情報をページの作業対象チップ情報に加える
        If Not IsNothing(drCandidateWorkInfo) Then
            Dim strCandidateId As New System.Text.StringBuilder

            strCandidateId.Append(drCandidateWorkInfo("REZID").ToString())
            strCandidateId.Append("_")
            strCandidateId.Append(drCandidateWorkInfo("SEQNO").ToString())
            strCandidateId.Append("_")
            strCandidateId.Append(drCandidateWorkInfo("DSEQNO").ToString())

            Me.HiddenCandidateId.Value = strCandidateId.ToString()
        Else
            Me.HiddenCandidateId.Value = String.Empty
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return drCandidateWorkInfo

    End Function

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter( _
        ByVal commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, _
        ByRef category As Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory) As Integer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} START" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'ログインスタッフがChT権限の場合
        If Operation.CHT = objStaffContext.OpeCD Then
            '自ページの所属メニューを宣言
            category = FooterMenuCategory.TechnicianMain

        End If
        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '表示非表示に関わらず、使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterButton()

        Logger.Info("InitFooterButton.S")

        'ヘッダ表示設定
        '戻るボタン非活性化
        CType(Me.Master, CommonMasterPage).IsRewindButtonEnabled = False


        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        ''メインメニューボタンの設定
        'Dim mainMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        'AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click

        ' ''作業追加ボタンの設定
        'Dim addWorkButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADDITIONAL_WORK)
        'AddHandler addWorkButton.Click, AddressOf AddWorkButton_Click

        ''完成検査ボタンの設定
        'Dim compExamButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_COMP_EXAM)
        'AddHandler compExamButton.Click, AddressOf CompletionCheckButton_Click

        ''スケジュールボタンのイベント設定
        'Dim scheduleButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SCHEDULE)
        'scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'mainMenuButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
        mainMenuButton.OnClientClick = "return FooterButtonClickAndLoadingScreen();"
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

        ''電話帳ボタンのイベント設定
        Dim telDirectoryButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
        telDirectoryButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'R/O一覧の設定
        Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO_LIST)
        AddHandler roListButton.Click, AddressOf RoListButton_Click
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'roListButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
        roListButton.OnClientClick = "return FooterButtonClickAndLoadingScreen();"
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

        '追加作業一覧の設定
        Dim additionalWorkListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADDITIONAL_WORK_LIST)
        AddHandler additionalWorkListButton.Click, AddressOf AddWorkListButton_Click
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'additionalWorkListButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
        additionalWorkListButton.OnClientClick = "return FooterButtonClickAndLoadingScreen();"
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

        'ログインスタッフがChT権限の場合
        If Operation.CHT = objStaffContext.OpeCD Then

            '戻るボタン非活性化
            CType(Me.Master, CommonMasterPage).IsRewindButtonEnabled = True

            'TCメインメニューボタンの設定
            Dim tcMainMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TC_MAIN_MEUN)
            AddHandler tcMainMenuButton.Click, AddressOf TcMainMeneButton_Click
            '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
            'tcMainMenuButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
            tcMainMenuButton.OnClientClick = "return FooterButtonClickAndLoadingScreen();"
            '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

            'FMメインメニューボタンの設定
            Dim fmMainMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_FM_MAIN_MEUN)
            AddHandler fmMainMenuButton.Click, AddressOf FmMainMeneButton_Click
            '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
            'fmMainMenuButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
            fmMainMenuButton.OnClientClick = "return FooterButtonClickAndLoadingScreen();"
            '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

        End If


        '2012/11/14 TMEJ 彭健  アクティブインジゲータ対応（クルクルのタイムアウト対応）　CHG_S
        'addWorkButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
        'compExamButton.OnClientClick = "reloadPageIfNoResponse(); return FooterButtonClick();"
        '2012/11/14 TMEJ 彭健  アクティブインジゲータ対応（クルクルのタイムアウト対応）　CHG_E


        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info("InitFooterButton.E")

    End Sub

#End Region

#Region "フッターサブメニュー処理"

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1}  START. " _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

        If OPERATION_CODE_TC = objStaffContext.OpeCD Then

            ' 再表示
            Me.RedirectNextScreen(APPLICATION_ID)

        ElseIf OPERATION_CODE_CHT = objStaffContext.OpeCD Then

            'ストールIDのセッションをクリア
            MyBase.RemoveValue(ScreenPos.Current, SESSION_KEY_STALL_ID)
            ' チーフテクニシャンのメインメニュー画面に遷移
            Me.RedirectNextScreen(PROCESS_CONTROL_PAGE_ID)

        End If
        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1}  END. " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' R/O一覧ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RoListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1}  START. " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '他システムとの画面連携
        ScreenLinkage(DISPLAY_NUMBER_14)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1}  END. " _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 追加作業一覧ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AddWorkListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1}  START. " _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '他システムとの画面連携
        ScreenLinkage(DISPLAY_NUMBER_22)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1}  END. " _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' TCメインメニューボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TcMainMeneButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1}  START. " _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チーフテクニシャンの場合
        If OPERATION_CODE_CHT = objStaffContext.OpeCD Then

            Me.SetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, stallId)

        End If

        ' 再表示
        Me.RedirectNextScreen(APPLICATION_ID)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1}  END. " _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' FMメインメニューボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FmMainMeneButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1}  START. " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チーフテクニシャンの場合
        If OPERATION_CODE_CHT = objStaffContext.OpeCD Then

            Me.SetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, stallId)

        End If

        ' FMメインメニュー画面に遷移
        Me.RedirectNextScreen(MAINMENU_ID_FM)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    ' ''' <summary>
    ' ''' 追加作業ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub AddWorkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)


    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

    'Logger.Error("AddWorkButton_Click.S")

    ''選択中のチップ情報を取得する.
    'Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
    'selectedChipInfo = GetSelectedChipInfo()

    ''いずれかの作業チップを選択している場合
    'If (Me.HiddenSelectedChip.Value = SELECTED_CHIP_ON) Then

    '    '選択中のチップが作業対象チップである場合
    '    If (Not IsNothing(selectedChipInfo)) Then
    '        'R/O作業ステータスを取得する.
    '        Dim repairOrderStatus As String
    '        repairOrderStatus = CType(Me.HiddenOrderStatus.Value, String)

    '        '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    '        'R/O作業ステータスが、整備中・検査完了のいずれかに属する場合
    '        If (ORDER_STATUS_ON_WORK_ORDER.Equals(repairOrderStatus) Or ORDER_STATUS_COMPLETE_INSPECTION.Equals(repairOrderStatus)) Then

    '            Dim dlrCd As String = Me.objStaffContext.DlrCD
    '            Dim orderNumber As String = Me.HiddenFieldOrderNo.Value

    '            '追加作業ステータス情報取得
    '            Dim dt As IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable = businessLogic.GetAddRepairStatusList(dlrCd, orderNumber)

    '            ' 取得結果チェック
    '            If dt Is Nothing OrElse dt.Rows.Count = 0 Then      '追加作業が存在しないため、追加作業入力(新規)に画面遷移
    '                ' 次画面遷移パラメータ設定
    '                Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)                        ' 整備受注NO
    '                Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", SC3170203_NEW_EDIT_FLAG_EDIT)       ' 編集フラグ
    '                Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", "0")                              ' 追加作業ユニークID
    '                ClientScript.RegisterStartupScript(Me.GetType(), "key", "confirmBeforeRedirectSC3170203();", True)
    '            Else
    '                '追加作業が存在する場合

    '                ' 追加作業入力画面に遷移時パターンの取得
    '                Dim pattern As AddWorkRedirect = Me.GetAddWorkNextScreenPattern(dt)

    '                '最終行の枝番取得
    '                Dim dr As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow = DirectCast(dt.Rows(dt.Rows.Count - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)

    '                'パターン別遷移処理
    '                Select Case pattern
    '                    Case AddWorkRedirect.SC3170101          '追加作業一覧へ遷移
    '                        RedirectAddRepairList()
    '                    Case AddWorkRedirect.SC3170201_New      '追加作業入力(新規)へ遷移
    '                        ' 次画面遷移パラメータ設定
    '                        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)                        ' 整備受注NO
    '                        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", SC3170201_EDIT_FLAG_NEW_EDIT)       ' 編集フラグ
    '                        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", "0")                              ' 追加作業ユニークID
    '                        ClientScript.RegisterStartupScript(Me.GetType(), "key", "confirmBeforeRedirectSC3170201();", True)
    '                    Case AddWorkRedirect.SC3170201_Edit
    '                        '追加作業入力(編集)へ遷移
    '                        RedirectAddRepairSC3170201(orderNumber, SC3170201_EDIT_FLAG_NEW_EDIT, dr.SRVADDSEQ)
    '                    Case AddWorkRedirect.SC3170203_New      '追加作業入力(新規)へ遷移
    '                        ' 次画面遷移パラメータ設定
    '                        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)                        ' 整備受注NO
    '                        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", SC3170203_NEW_EDIT_FLAG_EDIT)       ' 編集フラグ
    '                        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", "0")                              ' 追加作業ユニークID
    '                        ClientScript.RegisterStartupScript(Me.GetType(), "key", "confirmBeforeRedirectSC3170203();", True)
    '                    Case AddWorkRedirect.SC3170203_Edit
    '                        '追加作業入力(編集)へ遷移
    '                        RedirectAddRepairSC3170203(orderNumber, SC3170203_NEW_EDIT_FLAG_EDIT, dr.SRVADDSEQ)
    '                    Case AddWorkRedirect.SC3170203_Preview
    '                        '追加作業入力(参照)へ遷移
    '                        RedirectAddRepairSC3170203(orderNumber, SC3170203_EDIT_FLAG_PREVIEW, dr.SRVADDSEQ)
    '                    Case AddWorkRedirect.Warning_TC
    '                        '警告メッセージ出力
    '                        Me.ShowMessageBox(911)
    '                    Case AddWorkRedirect.Warning_SA
    '                        '警告メッセージ出力
    '                        Me.ShowMessageBox(912)
    '                    Case Else
    '                End Select

    '            End If
    '        Else
    '            'R/O作業ステータスが、整備中・検査完了以外の場合、追加作業一覧へ遷移
    '            RedirectAddRepairList()
    '        End If
    '    Else
    '        '選択中のチップが作業対象チップ以外の場合、追加作業一覧へ遷移
    '        RedirectAddRepairList()
    '    End If
    'Else
    '    '作業チップが未選択の場合、追加作業一覧へ遷移
    '    RedirectAddRepairList()
    'End If

    'Logger.Error("AddWorkButton_Click.E")


    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    'End Sub

    '2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) END

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

    'Protected Sub ButtonRedirectSC3170201_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRedirectSC3170201.Click

    '    Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '    With logWork
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.ORDERNO", False).ToString()))          'TODO: containsKey check
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.EDITFLG", False).ToString()))        'TODO: containsKey check
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", False).ToString()))    'TODO: containsKey check
    '    End With
    '    Logger.Error(logWork.ToString())

    '    Me.RedirectNextScreen(ADDITION_WORK_SA_PAGE_ID)
    'End Sub

    'Protected Sub ButtonRedirectSC3170203_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRedirectSC3170203.Click
    '    Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '    With logWork
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.ORDERNO", False).ToString()))          'TODO: containsKey check
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.EDITFLG", False).ToString()))        'TODO: containsKey check
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", MyBase.GetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", False).ToString()))    'TODO: containsKey check
    '    End With
    '    Logger.Error(logWork.ToString())

    '    Me.RedirectNextScreen(ADDITION_WORK_PAGE_ID)
    'End Sub

    ''2012/03/21 上田 仕様変更対応(追加作業関連の遷移先変更) START
    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' 追加作業一覧遷移
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' '''-----------------------------------------------------------------------
    'Private Sub RedirectAddRepairList()

    '    Dim logAddList As StringBuilder = New StringBuilder(String.Empty)
    '    With logAddList
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", ADDITION_WORK_LIST_ID))
    '    End With
    '    Logger.Error(logAddList.ToString())

    '    ' 追加作業一覧画面に遷移
    '    Me.RedirectNextScreen(ADDITION_WORK_LIST_ID)

    'End Sub

    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' 追加作業入力(SC3170201)画面遷移
    ' ''' </summary>
    ' ''' <param name="orderNo">整備受注NO</param>
    ' ''' <param name="editFlg">編集フラグ(0: 新規, 1: 編集)</param>
    ' ''' <param name="srvaddSeq">追加作業ユニークID(0: 新規, それ以外: 枝番)</param>
    ' ''' <remarks></remarks>
    ' '''-----------------------------------------------------------------------
    'Private Sub RedirectAddRepairSC3170201(ByVal orderNo As String, ByVal editFlg As String, ByVal srvaddSeq As String)

    '    Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '    With logWork
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", ADDITION_WORK_SA_PAGE_ID))
    '        .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", editFlg))
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", srvaddSeq))
    '    End With
    '    Logger.Error(logWork.ToString())

    '    ' 次画面遷移パラメータ設定
    '    Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)            ' 整備受注NO
    '    Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editFlg)            ' 編集フラグ
    '    Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvaddSeq)        ' 追加作業ユニークID

    '    ' 追加作業入力画面(SC3170201)に遷移
    '    Me.RedirectNextScreen(ADDITION_WORK_SA_PAGE_ID)

    'End Sub

    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' 追加作業入力(SC3170203)画面遷移
    ' ''' </summary>
    ' ''' <param name="orderNo">整備受注NO</param>
    ' ''' <param name="editFlg">編集フラグ(0: 新規, 1: 編集)</param>
    ' ''' <param name="srvaddSeq">追加作業ユニークID(0: 新規, それ以外: 枝番)</param>
    ' ''' <remarks></remarks>
    ' '''-----------------------------------------------------------------------
    'Private Sub RedirectAddRepairSC3170203(ByVal orderNo As String, ByVal editFlg As String, ByVal srvaddSeq As String)

    '    Dim logWork As StringBuilder = New StringBuilder(String.Empty)
    '    With logWork
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", ADDITION_WORK_PAGE_ID))
    '        .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", editFlg))
    '        .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", srvaddSeq))
    '    End With
    '    Logger.Error(logWork.ToString())

    '    ' 次画面遷移パラメータ設定
    '    Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)            ' 整備受注NO
    '    Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editFlg)            ' 編集フラグ
    '    Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvaddSeq)        ' 追加作業ユニークID

    '    ' 追加作業入力画面(SC3170203)に遷移
    '    Me.RedirectNextScreen(ADDITION_WORK_PAGE_ID)

    'End Sub

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2012/06/05 KN 彭 コード分析対応 START

    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' 追加作業入力画面に遷移時パターンの確認
    ' ''' </summary>
    ' ''' <param name="dt">追加作業ステータス情報</param>
    ' ''' <returns>遷移時パターン</returns>
    ' ''' <remarks></remarks>
    ' '''-----------------------------------------------------------------------
    'Private Function GetAddWorkNextScreenPattern(ByVal dt As IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable) As AddWorkRedirect

    '    Dim rtnValue As AddWorkRedirect = AddWorkRedirect.Invalid

    '    '最終行取得
    '    Dim dr As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow = DirectCast(dt.Rows(dt.Rows.Count - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} DRAWER: {1}, STATUS: {2}, CREUSER: {3}", "GetAddWorkNextScreenPattern.S", dr.DRAWER, dr.STATUS, dr.CREUSER))

    '    If ADD_WORK_DRAWER_SA.Equals(dr.DRAWER) Then
    '        '起票者がSAの場合
    '        If ADD_WORK_STATUS_WAITING_ESTIMATE.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_WAITING_CUSTOMER.Equals(dr.STATUS) Then
    '            '追加作業ステータスが「SA見積確定待ち」又は、「顧客承認待ち」の場合
    '            'ワーニングメッセージを出力する
    '            rtnValue = AddWorkRedirect.Warning_SA
    '        ElseIf ADD_WORK_STATUS_WAITING_SHIPPING.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_WAITING_START.Equals(dr.STATUS) OrElse _
    '               ADD_WORK_STATUS_ON_WORK_ORDER.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_COMPLETE_INSPECTION.Equals(dr.STATUS) Then
    '            '「CT着工指示／PS部品出荷待ち」又は、「TC作業開始待ち」又は、「整備中」又は、「完成検査完了」の場合
    '            '2012/03/26 上田 遷移先修正 START
    '            'SC3170203_追加作業入力(新規)へ画面遷移
    '            rtnValue = AddWorkRedirect.SC3170203_New
    '            ''SC3170201_追加作業入力(新規)へ画面遷移
    '            'rtnValue = AddWorkRedirect.SC3170201_New
    '            '2012/03/26 上田 遷移先修正 END
    '        Else
    '            '上記以外の場合は、追加作業一覧へ画面遷移
    '            rtnValue = AddWorkRedirect.SC3170101
    '        End If
    '    Else
    '        '起票者がTCの場合
    '        If ADD_WORK_STATUS_VOUCHER.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_WAITING_CONSENT.Equals(dr.STATUS) OrElse _
    '           ADD_WORK_STATUS_WAITING_PARTS.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_WAITING_ESTIMATE.Equals(dr.STATUS) Then
    '            '「TC起票中」又は、「CT承認待ち」又は、「PS部品見積待ち」又は、「SA見積確定待ち」の場合

    '            ' 2012/04/05 KN 西田【SERVICE_1】プレユーザーテスト課題No.78 追加作業入力に編集モードで遷移できない START
    '            '起票者取得
    '            Dim preCreateUser As String = String.Format(CultureInfo.InvariantCulture, "{0}@{1}", dr.CREUSER.Trim(), Me.objStaffContext.DlrCD)
    '            'Dim preCreateUser As String = String.Format("{0}@{1}", dr.CREUSER, Me.objStaffContext.DlrCD)
    '            ' 2012/04/05 KN 西田【SERVICE_1】プレユーザーテスト課題No.78 追加作業入力に編集モードで遷移できない END

    '            '起票者チェック
    '            If preCreateUser.Equals(Me.objStaffContext.Account) Then
    '                '同一起票者の場合
    '                '2012/03/26 上田 遷移先修正 START
    '                'SC3170203_追加作業入力(編集)へ画面遷移
    '                rtnValue = AddWorkRedirect.SC3170203_Edit

    '                'If ADD_WORK_STATUS_VOUCHER.Equals(dr.STATUS) OrElse ADD_WORK_STATUS_WAITING_CONSENT.Equals(dr.STATUS) OrElse _
    '                '   ADD_WORK_STATUS_WAITING_PARTS.Equals(dr.STATUS) Then
    '                '    '「TC起票中」又は、「CT承認待ち」又は、「PS部品見積待ち」の場合
    '                '    'SC3170203_追加作業入力(編集)へ画面遷移
    '                '    rtnValue = AddWorkRedirect.SC3170203_Edit
    '                'Else
    '                '    '「SA見積確定待ち」の場合
    '                '    'SC3170201_追加作業入力(編集)へ画面遷移
    '                '    rtnValue = AddWorkRedirect.SC3170201_Edit
    '                'End If
    '                '2012/03/26 上田 遷移先修正 END
    '            Else
    '                '起票者が異なる場合
    '                'SC3170203_追加作業入力(参照)へ画面遷移
    '                rtnValue = AddWorkRedirect.SC3170203_Preview
    '            End If
    '        ElseIf ADD_WORK_STATUS_WAITING_CUSTOMER.Equals(dr.STATUS) Then
    '            '「顧客承認待ち」の場合
    '            'ワーニングメッセージ出力
    '            rtnValue = AddWorkRedirect.Warning_TC
    '        Else
    '            '「CT着工指示／PS部品出荷待ち」又は、「TC作業開始待ち」又は、「整備中」又は、「完成検査完了」の場合
    '            '2012/03/26 上田 遷移先修正 START
    '            'SC3170203_追加作業入力(新規)へ画面遷移
    '            rtnValue = AddWorkRedirect.SC3170203_New
    '            ''SC3170201_追加作業入力(新規)へ画面遷移
    '            'rtnValue = AddWorkRedirect.SC3170201_New
    '            '2012/03/26 上田 遷移先修正 END
    '        End If
    '    End If

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} returnValue: {1}", "GetAddWorkNextScreenPattern.E", rtnValue.ToString()))
    '    Return rtnValue

    'End Function

    '更新：2012/03/20 KN 日比野　追加作業ボタン押下時の遷移判断の修正  END
    '2012/06/05 KN 彭 コード分析対応 END

    ''' <summary>
    ''' 完成検査ボタンを押した時の処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CompletionCheckButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '選択中のチップ情報を取得する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = GetSelectedChipInfo()

        '遷移先画面IDの初期値として、完成検査一覧ページを指定
        'Dim nextScreenId As String
        'nextScreenId = COMPLETION_CHECK_PAGE_ID

        'Logger.Info("CompletionCheckButton_Click SelectedChip=" + Me.HiddenSelectedChip.Value)
        '作業対象チップ情報を取得している場合のみ、判定処理を実施する.
        '作業チップを選択している場合、
        If (Me.HiddenSelectedChip.Value.Equals(SELECTED_CHIP_ON)) Then
            '選択中のチップが作業対象チップである場合、
            If (Not IsNothing(selectedChipInfo)) Then

                '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

                'Logger.Info("CompletionCheckButton_Click Not IsNothing selectedChipInfo")
                'Logger.Info("CompletionCheckButton_Click RESULT_STATUS=" + selectedChipInfo.RESULT_STATUS)

                '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
                '選択中チップが作業中である場合、
                'If (selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WORKING)) Then

                '選択中チップが作業中か、作業完了である場合、
                If (selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WORKING)) Or _
                    (selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_COMP)) Then
                    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

                    'Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
                    'Dim workSeq As String = Me.HiddenSelectedWorkSeq.Value
                    'Dim srvAddCount As String = "0"

                    'If Not String.IsNullOrEmpty(workSeq) AndAlso _
                    '   Not WORKSEQ_NOPLAN_PARENT.Equals(workSeq) Then
                    '    'TACTの枝番取得
                    '    srvAddCount = businessLogic.GetTactChildNo(Me.objStaffContext.DlrCD, _
                    '                                          orderNumber,
                    '                                          CType(workSeq, Integer))
                    'End If

                    'Logger.Error("Redirect.ORDERNO=" & orderNumber)
                    'Logger.Error("Redirect.SRVADDSEQ=" & srvAddCount)
                    'Logger.Error("Redirect.EDIT=0")
                    'Logger.Error("Redirect.REZID=" & selectedChipInfo.SEQNO.ToString(CultureInfo.InvariantCulture))
                    'Logger.Info("Redirect.TC_REDIRECT_FLAG=1")

                    ''完成検査チェックシート入力ページに渡す引数をセッションに格納
                    'MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
                    'MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvAddCount)
                    'MyBase.SetValue(ScreenPos.Next, "Redirect.EDIT", "0")
                    ''2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
                    ''MyBase.SetValue(ScreenPos.Next, "Redirect.REZID=", selectedChipInfo.REZID.ToString(CultureInfo.InvariantCulture))   '仕分け課題No.4対応で追加
                    ''作業内容IDを格納
                    'MyBase.SetValue(ScreenPos.Next, "Redirect.REZID=", selectedChipInfo.SEQNO.ToString(CultureInfo.InvariantCulture))
                    ''2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END
                    MyBase.SetValue(ScreenPos.Next, "Redirect.TC_REDIRECT_FLAG", "1")   '他画面に遷移した後、TCに戻ると遷移前のチップが選択されない問題の対応

                    'nextScreenId = COMPLETION_CHECK_INPUT_PAGE_ID

                    ScreenLinkage(DISPLAY_NUMBER_0)

                    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

                End If
            End If
        End If

        'R/O情報欄のフリック時か確認-フリック時は完成検査入力画面に遷移(編集フラグを「1」にすることで参照遷移とする)
        If (Not IsNothing(selectedChipInfo)) Then

            '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
            'If Not (selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WORKING)) And　(Me.flickRoInformationFlag = 1) Then

            'フリック時に、選択中チップが作業中か、作業完了である場合、
            If (Not selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WAIT)) AndAlso _
                   (Me.flickRoInformationFlag = 1) Then
                '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

                '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START

                'Dim orderNumberFlick As String = Me.HiddenFieldOrderNo.Value
                'Dim workSeqFlick As String = Me.HiddenSelectedWorkSeq.Value
                'Dim srvAddCountFlick As String = "0"

                'If Not String.IsNullOrEmpty(workSeqFlick) AndAlso _
                '   Not WORKSEQ_NOPLAN_PARENT.Equals(workSeqFlick) Then
                '    'TACTの枝番取得
                '    srvAddCountFlick = businessLogic.GetTactChildNo(Me.objStaffContext.DlrCD, _
                '                                                    orderNumberFlick,
                '                                                    CType(workSeqFlick, Integer))
                'End If

                'Logger.Error("Redirect.ORDERNO=" & orderNumberFlick)
                'Logger.Error("Redirect.SRVADDSEQ=" & srvAddCountFlick)
                'Logger.Error("Redirect.EDIT=1")
                'Logger.Error("Redirect.REZID=" & selectedChipInfo.SEQNO.ToString(CultureInfo.InvariantCulture))
                'Logger.Info("Redirect.TC_REDIRECT_FLAG=1")

                ''完成検査チェックシート入力ページに渡す引数をセッションに格納
                'MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumberFlick)
                'MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", srvAddCountFlick)
                'MyBase.SetValue(ScreenPos.Next, "Redirect.EDIT", "1")

                ''2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
                ''MyBase.SetValue(ScreenPos.Next, "Redirect.REZID=", selectedChipInfo.REZID.ToString(CultureInfo.InvariantCulture))   '仕分け課題No.4対応で追加
                ''作業内容IDを格納
                'MyBase.SetValue(ScreenPos.Next, "Redirect.REZID=", selectedChipInfo.SEQNO.ToString(CultureInfo.InvariantCulture))
                ''2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

                MyBase.SetValue(ScreenPos.Next, "Redirect.TC_REDIRECT_FLAG", "1")   '他画面に遷移した後、TCに戻ると遷移前のチップが選択されない問題の対応

                'nextScreenId = COMPLETION_CHECK_INPUT_PAGE_ID

                ScreenLinkage(DISPLAY_NUMBER_0)


            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START DISPLAY_NUMBER:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , DISPLAY_NUMBER_0))

        ' 完成検査へ遷移
        'Me.RedirectNextScreen(nextScreenId)

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Sub

    ''' <summary>
    ''' 選択中のチップ情報を取得する.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedChipInfo() As SC3150101DataSet.SC3150101ChipInfoRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START,HiddenSelectedId={2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , Me.HiddenSelectedId.Value))

        '返却する選択されているチップ情報を初期化する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = Nothing

        '予約・実績チップデータセットを取得する.
        Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtChipInfo = businessLogic.GetStallChipInfo(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        '取得した予約・実績チップのデータセットをループ処理する.
        For Each eachData As SC3150101DataSet.SC3150101ChipInfoRow In dtChipInfo.Rows

            'チップのチップIDを取得する.
            Dim chipId As String
            chipId = CreateChipId(eachData)
            'Logger.Info("GetSelectedChipInfo roop chipId=" + chipId)

            '選択中のチップと合致する場合、返却値に現在選択中のチップ情報を格納しループ処理を抜ける.
            If (Me.HiddenSelectedId.Value.Equals(chipId)) Then
                'Logger.Info("GetSelectedChipInfo chipId equals selectedId")

                selectedChipInfo = eachData
                Exit For
            End If

        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return selectedChipInfo

    End Function

    ''' <summary>
    ''' 選択されているチップを特定するための、チップIDを作成する.
    ''' </summary>
    ''' <param name="drChipInfo">作業対象チップ情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateChipId(ByVal drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップIDを生成する.
        Dim chipIdStringBuilder As New System.Text.StringBuilder

        '取得した作業対象チップ情報がNothingでない場合、値を取得する.
        If Not IsNothing(drChipInfo) Then
            'Logger.Info("CreateChipId if Not IsNothing ParamChipInfo")

            chipIdStringBuilder.Append(drChipInfo.REZID)
            chipIdStringBuilder.Append("_")
            chipIdStringBuilder.Append(drChipInfo.SEQNO)
            chipIdStringBuilder.Append("_")
            chipIdStringBuilder.Append(drChipInfo.DSEQNO)
        End If

        Dim chipId As String
        chipId = chipIdStringBuilder.ToString()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return chipId

    End Function

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 他システムとの画面連携
    ''' </summary>
    ''' <param name="displayNumber"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正
    ''' 2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub ScreenLinkage(ByVal displayNumber As String, _
                              Optional ByVal inRepiarOrder As String = "0", _
                              Optional ByVal serviceInNumber As String = " ", _
                              Optional ByVal serviceInDealerCode As String = " ", _
                              Optional ByVal inRepiarOrderSeq As String = "0")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1}  START. DISP_NUM:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , displayNumber))

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        '変数宣言
        Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        Dim strJobDetailId As String = Me.HiddenSelectedJobDetailId.Value
        Dim jobDetailId As Decimal = 0

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Decimal.TryParse(strJobDetailId, jobDetailId)

        '作業内容IDのHidden値チェック
        If Not (String.IsNullOrEmpty(Trim(strJobDetailId))) Then
            '存在する場合
            'Hidden値を設定
            jobDetailId = Decimal.Parse(strJobDetailId, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        Dim saChipID As String = ""
        Dim basRezId As String = ""
        Dim vin As String = ""
        Dim dmsDealerCode As String = ""
        Dim dmsBraunchCode As String = ""
        Dim dmsLoginAccount As String = ""

        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START
        Dim dmsOldOrderDealerCode As String = ""
        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

        'RO番号がない場合取得しない
        If Not String.IsNullOrEmpty(orderNumber) Then

            Dim dtCmpInsScreenLinkageInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoDataTable
            '画面連携に必要な引数を取得
            dtCmpInsScreenLinkageInfo = businessLogic.CompletionScreenLinkageInfo(objStaffContext.DlrCD, _
                                                                                  objStaffContext.BrnCD, _
                                                                                  jobDetailId)
            If dtCmpInsScreenLinkageInfo.Rows.Count > 0 Then
                'ロウに格納
                Dim drCmpInsScreenLinkageInfo As SC3150101DataSet.SC3150101ScreenLinkageInfoRow = _
                       DirectCast(dtCmpInsScreenLinkageInfo.Rows(0), SC3150101DataSet.SC3150101ScreenLinkageInfoRow)
                '変数に格納
                saChipID = CType(drCmpInsScreenLinkageInfo.VISITSEQ, String).Trim()
                basRezId = drCmpInsScreenLinkageInfo.DMS_JOB_DTL_ID.Trim()
                vin = drCmpInsScreenLinkageInfo.VIN
                'repairOrderSeq = drCmpInsScreenLinkageInfo.RO_SEQ
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                              , "{0}.{1} ScreenLinkageInfoDataTable.Rows.Count:{2}" _
                                              , Me.GetType.ToString _
                                              , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                              , dtCmpInsScreenLinkageInfo.Rows.Count.ToString(CultureInfo.CurrentCulture())))
            End If


        End If

        '基幹情報の取得
        Using dtServiceCommon As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
            Me.serviceCommon.GetIcropToDmsCode(objStaffContext.DlrCD, _
                                               DmsCodeType.BranchCode, _
                                               objStaffContext.DlrCD, _
                                               objStaffContext.BrnCD, _
                                               Nothing, _
                                               objStaffContext.Account)

            'ロウに格納
            Dim drServiceCommon As ServiceCommonClassDataSet.DmsCodeMapRow = _
                 DirectCast(dtServiceCommon.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

            '変数に格納
            dmsDealerCode = drServiceCommon.CODE1
            dmsBraunchCode = drServiceCommon.CODE2
            dmsLoginAccount = drServiceCommon.ACCOUNT
        End Using

        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START
        '過去プレビュー用の販売店コードが存在する場合
        If Not (String.IsNullOrWhiteSpace(serviceInDealerCode)) Then
            '過去プレビューの基幹販売店コードを取得する
            '基幹情報の取得
            Using dtOldOrderServiceCommon As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                Me.serviceCommon.GetIcropToDmsCode(serviceInDealerCode, _
                                                   DmsCodeType.DealerCode, _
                                                   serviceInDealerCode, _
                                                   Nothing, _
                                                   Nothing, _
                                                   Nothing)

                'ロウに格納
                Dim drOldOrderServiceCommon As ServiceCommonClassDataSet.DmsCodeMapRow = _
                     DirectCast(dtOldOrderServiceCommon.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '変数に格納
                dmsOldOrderDealerCode = drOldOrderServiceCommon.CODE1

            End Using

        End If
        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

        'セッションに設定
        Select Case displayNumber
            Case DISPLAY_NUMBER_23
                '追加作業起票
                Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDealerCode)                   ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param2", dmsBraunchCode)                  ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "Session.Param3", dmsLoginAccount)                 ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "Session.Param4", saChipID)                        ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "Session.Param5", basRezId)                        ' DMS予約ID
                Me.SetValue(ScreenPos.Next, "Session.Param6", orderNumber)                     ' RO番号
                Me.SetValue(ScreenPos.Next, "Session.Param7", ADDITIONAL_WORK_SEND_VALUE)      ' RO作業連番
                Me.SetValue(ScreenPos.Next, "Session.Param8", vin)                             ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "Session.Param9", ADD_WORK_NEW)                    ' 「0：編集」固定
                Me.SetValue(ScreenPos.Next, "Session.Param10", strJobDetailId)                 ' 作業内容ID
                Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_23)             ' 「23：追加作業起票」固定
            Case DISPLAY_NUMBER_22
                '追加作業一覧
                Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDealerCode)                   ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param2", dmsBraunchCode)                  ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "Session.Param3", dmsLoginAccount)                 ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "Session.Param4", "")                              ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "Session.Param5", "")                              ' DMS予約ID
                Me.SetValue(ScreenPos.Next, "Session.Param6", "")                              ' RO番号
                Me.SetValue(ScreenPos.Next, "Session.Param7", "")                              ' RO作業連番
                Me.SetValue(ScreenPos.Next, "Session.Param8", "")                              ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "Session.Param9", ADD_WORK_NEW)                    ' 「0：編集」固定
                Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_22)             ' 「22：追加作業一覧」固定
            Case DISPLAY_NUMBER_14
                'R/O一覧
                Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDealerCode)                   ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param2", dmsBraunchCode)                  ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "Session.Param3", dmsLoginAccount)                 ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "Session.Param4", "")                              ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "Session.Param5", "")                              ' DMS予約ID
                Me.SetValue(ScreenPos.Next, "Session.Param6", "")                              ' RO番号
                Me.SetValue(ScreenPos.Next, "Session.Param7", "")                              ' RO作業連番
                Me.SetValue(ScreenPos.Next, "Session.Param8", "")                              ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "Session.Param9", ADD_WORK_NEW)                    ' 「0：編集」固定
                Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_14)             ' 「14：R/O一覧」固定
            Case DISPLAY_NUMBER_0
                '完成検査依頼
                Me.SetValue(ScreenPos.Next, "DealerCode", " ")                                 ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "BranchCode", " ")                                 ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "LoginUserID", " ")                                ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "SAChipID", saChipID)                              ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "BASREZID", basRezId)                              ' DMS予約ID
                Me.SetValue(ScreenPos.Next, "R_O", orderNumber)                                ' RO番号
                Me.SetValue(ScreenPos.Next, "SEQ_NO", "0")                                     ' RO作業連番
                Me.SetValue(ScreenPos.Next, "VIN_NO", vin)                                     ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "ViewMode", ADD_WORK_NEW)                          '「0：編集」固定
                Me.SetValue(ScreenPos.Next, "JOB_DTL_ID", strJobDetailId)                      '予約ID(作業内容ID)
                '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
                'Case DISPLAY_NUMBER_13
            Case DISPLAY_NUMBER_25
                '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END
                'ROプレビュー（過去）
                Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDealerCode)                   ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param2", dmsBraunchCode)                  ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "Session.Param3", dmsLoginAccount)                 ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "Session.Param4", saChipID)                        ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "Session.Param5", basRezId)                    ' DMS予約ID
                If Not (inRepiarOrder.Equals("0")) Then
                    Me.SetValue(ScreenPos.Next, "Session.Param6", inRepiarOrder)               ' RO番号
                Else
                    Me.SetValue(ScreenPos.Next, "Session.Param6", "")                          ' RO番号
                End If
                Me.SetValue(ScreenPos.Next, "Session.Param7", "0")                             ' RO作業連番
                Me.SetValue(ScreenPos.Next, "Session.Param8", vin)                             ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "Session.Param9", ADD_WORK_EDIT)                   ' 「1：編集(過去)」固定
                Me.SetValue(ScreenPos.Next, "Session.Param10", ADD_WORK_EDIT)                  ' 「1：過去サービス」固定
                Me.SetValue(ScreenPos.Next, "Session.Param11", serviceInNumber)                ' 入庫管理番号

                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START
                'Me.SetValue(ScreenPos.Next, "Session.Param12", serviceInDealerCode)            ' 入庫履歴の基幹版売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param12", dmsOldOrderDealerCode)            ' 入庫履歴の基幹版売店コード
                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

                '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
                'Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_13)             ' 「13：R/O参照」固定
                Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_25)             ' 「25：ROプレビュー（過去）」固定
                '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END

                '2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移 START
            Case DISPLAY_NUMBER_13
                'ROプレビュー
                Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDealerCode)                   ' ログインユーザーのDMS販売店コード
                Me.SetValue(ScreenPos.Next, "Session.Param2", dmsBraunchCode)                  ' ログインユーザーのDMS店舗コード
                Me.SetValue(ScreenPos.Next, "Session.Param3", dmsLoginAccount)                 ' ログインユーザーのアカウント
                Me.SetValue(ScreenPos.Next, "Session.Param4", saChipID)                        ' 来店管理番号
                Me.SetValue(ScreenPos.Next, "Session.Param5", basRezId)                        ' DMS予約ID
                If Not (inRepiarOrder.Equals("0")) Then
                    Me.SetValue(ScreenPos.Next, "Session.Param6", inRepiarOrder)               ' RO番号
                Else
                    Me.SetValue(ScreenPos.Next, "Session.Param6", "")                          ' RO番号
                End If
                Me.SetValue(ScreenPos.Next, "Session.Param7", inRepiarOrderSeq)                ' RO作業連番
                Me.SetValue(ScreenPos.Next, "Session.Param8", vin)                             ' 車両登録No.のVIN
                Me.SetValue(ScreenPos.Next, "Session.Param9", ADD_WORK_EDIT)                   ' 「1：編集(過去)」固定
                Me.SetValue(ScreenPos.Next, "Session.Param10", ADD_WORK_NEW)                   ' 「0：プレビュー」固定
                Me.SetValue(ScreenPos.Next, "Session.Param11", String.Empty)                   ' 空文字
                Me.SetValue(ScreenPos.Next, "Session.Param12", String.Empty)                   ' 空文字
                Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", DISPLAY_NUMBER_13)             ' 「13：ROプレビュー 」固定
                '2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　END
        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1}  END. " _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ''チーフテクニシャンの場合
        If OPERATION_CODE_CHT = objStaffContext.OpeCD Then

            Me.SetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, stallId)

        End If
        If displayNumber.Equals(DISPLAY_NUMBER_0) Then
            '完成検査依頼画面へ遷移
            Me.RedirectNextScreen(COMPLETION_CHECK_INPUT_PAGE_ID)
        Else
            '他システム連携画面へ遷移
            Me.RedirectNextScreen(SERVICE_COMMON_PAGE_ID)
        End If

    End Sub

    ''' <summary>
    ''' 追加作業ボタン表示設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddWorkButtonView()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1}  START. " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        objStaffContext = StaffContext.Current

        '追加作業ボタン表示フラグの初期値をOFFで設定
        Me.HiddenAddWorkButtonFlg.Value = "0"

        '選択中のチップの作業内容を取得
        Dim strJobDetailId As String = Me.HiddenSelectedJobDetailId.Value
        Dim DicJobDetailId As Decimal = 0
        '取得した作業内容が空ではない場合
        If Not String.IsNullOrEmpty(strJobDetailId) Then

            DicJobDetailId = CType(strJobDetailId, Decimal)

            'ROステータスの取得
            Dim dtRepairOrderStatus As SC3150101DataSet.SC3150101RepairOrderStatusDataTable = _
                businessLogic.GetRepairOrderStatus(DicJobDetailId, objStaffContext.DlrCD, objStaffContext.BrnCD)

            'RO番号が空の場合処理しない
            If dtRepairOrderStatus.Rows.Count > 0 Then

                '取得したROの数だけ繰り返す
                For Each eachData As SC3150101DataSet.SC3150101RepairOrderStatusRow In dtRepairOrderStatus.Rows

                    If (eachData.RO_STATUS.Equals(RO_STATUS_CUSTOMER_APPROVAL)) Or _
                       (eachData.RO_STATUS.Equals(RO_STATUS_WORKING)) Or
                       (eachData.RO_STATUS.Equals(RO_STATUS_DELIVERY)) Then

                        '追加作業ボタン表示フラグオン
                        Me.HiddenAddWorkButtonFlg.Value = "1"

                    End If
                Next
            End If
        End If
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  END. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
#End Region

#Region "画面固有フッターボタン処理"

    ' ''' <summary>
    ' ''' 部品連絡ボタン処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub ButtonConnectParts_Click(sender As Object, e As System.EventArgs) _
    '    Handles ButtonConnectParts.Click

    '    Logger.Info("ButtonConnectParts_Click Start")

    '    '押したフッタボタンの状態を、「部品連絡」に設定する.
    '    HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_CONNECT_PARTS

    '    '部品連絡のポップアップをコールする.
    '    'ここにコールする関数を記載すればOK.
    '    Me.RedirectNextScreen(PARTS_CONTACT_PAGE_ID)
    '    '休憩ポップアップ表示のため
    '    'HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

    '    Logger.Info("ButtonConnectParts_Click End")

    'End Sub

    ''' <summary>
    ''' 「休憩をとらない」選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonDoNotBreak_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles ButtonDoNotBreak.Click

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
    Protected Sub ButtonTakeBreak_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles ButtonTakeBreak.Click

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
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub SelectedTakeBreak(ByVal selectedBreak As Boolean)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '押したフッタボタンの状態を取得する.
        Dim pushedFooterStatus = Me.HiddenPushedFooter.Value
        'Logger.Info("SelectedTakeBreak pushedFooterStatus=" + pushedFooterStatus)

        '表示されている、休憩による作業伸長ポップアップの非表示フラグをセットする
        Me.HiddenBreakPopup.Value = POPUP_BREAK_NONE
        'フッタボタンの状態を初期化する.
        Me.HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_INIT

        'フッタボタンの状態に応じて、処理を分岐する.
        If (PUSHED_FOOTER_BUTTON_START_WORK.Equals(pushedFooterStatus)) Then
            'Logger.Info("SelectedTakeBreak pushedFooterStatus is Start_Work button Param:" + selectedBreak.ToString())
            StartWorkProcess(selectedBreak, True)

        ElseIf (PUSHED_FOOTER_BUTTON_SUSPEND_WORK.Equals(pushedFooterStatus)) Then
            'Logger.Info("SelectedTakeBreak pushedFooterStatus is Suspend_Work button Param:" + selectedBreak.ToString())
            SuspendWorkProcess(selectedBreak, True)

            '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
        ElseIf (PUSHED_FOOTER_BUTTON_FINISH_WORK.Equals(pushedFooterStatus)) Then
            FinishWorkProcess(selectedBreak, True)
            '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) END

            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        ElseIf (PUSHED_FOOTER_BUTTON_STOP_WORK.Equals(pushedFooterStatus)) Then

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

            'StopWorkProcess(selectedBreak, True)

            StopWorkProcess(selectedBreak)

            '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

            '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 作業開始ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonStartWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles HiddenButtonStartWork.Click
        'Protected Sub ButtonStartWork_Click(sender As Object, e As System.EventArgs) _
        '    Handles ButtonStartWork.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2012/03/02 上田 フッタボタン制御 Start
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Try
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END
            '押したフッタボタンの状態を、「作業開始」に設定する.
            HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_START_WORK

            '選択されているチップの予約IDを取得する.
            Dim selectedRezId As String
            selectedRezId = Me.HiddenSelectedReserveId.Value
            'Logger.Info("ButtonStartWork_Click selectedRezId=" + selectedRezId)

            '取得した予約IDがNull値でない場合のみ
        If (Not String.IsNullOrEmpty(selectedRezId)) Then
            'Logger.Info("ButtonStartWork_Click Not IsNullOrEmpty selectedRezId")

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

            ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
            'If (resultInterference = INTERFERENCE_FAILE) Then
            '    'Logger.Info("ButtonStartWork_Click resultInterference is faile")
            '    'Logger.Info("ButtonStartWork_Click HiddenBreakPopup=" + POPUP_BREAK_DISPLAY)
            '    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

            'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then
            '    'Logger.Info("ButtonStartWork_Click ValidationInterferenceBreakUnavailable is successfull")
            '    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、開始登録処理関数を呼び出す.
            '    StartWorkProcess(False)

            'End If

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

                    If (resultInterference = INTERFERENCE_FAILE) Then
                        '休憩に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                        HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY
                    ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then
                        '干渉しない場合、開始登録処理関数を呼び出す.
                        StartWorkProcess(False)
                    End If
                Else
                    '開始登録処理関数を呼び出す.
                    StartWorkProcess(False)
                End If

            End Using

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        End If
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Finally
        'Me.HiddenReloadFlag.Value = String.Empty
        'End Try
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END
        '2012/03/02 上田 フッタボタン制御 End

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 開始処理
    ''' </summary>
    ''' <param name="breakExtention">休憩取得有無</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Sub StartWorkProcess(ByVal breakExtention As Boolean, _
                                 Optional ByVal breakBottomFlg As Boolean = False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value
        'Logger.Info("StartWorkProcess selectedRezId=" + selectedRezId)

        Dim orderNo As String = Me.HiddenFieldOrderNo.Value
        'Logger.Info("StartWorkProcess orderNo=" + orderNo)

        ' 2012/11/05 TMEJ 彭    問連修正（GTMC121029047）START
        Dim selectedUpdateCount As Long
        selectedUpdateCount = CType(Me.HiddenSelectedUpdateCount.Value.Trim, Long)

        '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        '中断中作業の開始フラグ
        Dim restartStopJobFlg As Boolean = True

        '確認ダイアログでキャンセルが押された場合
        If Me.HiddenRestartStopJobFlg.Value.Equals("1") Then
            restartStopJobFlg = False
        End If
        '2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        '干渉チェックをせずに開始イベントを実施する.
        Dim resultEvent As Integer
        resultEvent = businessLogic.StartWork(objStaffContext.DlrCD, _
                                              objStaffContext.BrnCD, _
                                              CType(selectedRezId, Decimal), _
                                              Me.stallId, _
                                              objStaffContext.Account, _
                                              selectedUpdateCount, _
                                              orderNo, _
                                              restartStopJobFlg, _
                                              breakExtention, _
                                              breakBottomFlg)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''正常に終了していない場合、エラーメッセージを表示する.
        'If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
        '    'MyBase.ShowMessageBox(resultEvent)
        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultEvent, workStartFlg)
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END
        '    Exit Sub
        'End If

        '開始処理結果チェック
        If resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultEvent <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ

            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            'showErrMsgAndRefresh(resultEvent, workStartFlg)
            showErrMsgAndRefresh(resultEvent, workStartFlg, selectedRezId)
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' 2012/11/05 TMEJ 彭    問連修正（GTMC121029047）END

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        '選択中のチップの作業内容ID取得
        Dim jobDatilId As Decimal = 0
        If Not String.IsNullOrEmpty(Me.HiddenSelectedJobDetailId.Value) Then
            jobDatilId = CType(Me.HiddenSelectedJobDetailId.Value, Decimal)
        End If

        'Push送信
        businessLogic.WorkStartSendPush(objStaffContext.DlrCD, _
                                        objStaffContext.BrnCD, _
                                        objStaffContext.Account, _
                                        Me.stallId, _
                                        jobDatilId)

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()
        '開始処理を実施すると、シーケンス番号が更新されるため、チップのIDが変更される.
        'チップの選択状態を保持するため、選択中チップのIDを変更する.
        Me.HiddenSelectedId.Value = Me.HiddenCandidateId.Value

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '開始処理結果チェック
        If resultEvent = ActionResult.WarningOmitDmsError Then
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
    ''' 当日処理ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonSuspendWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonSuspendWork.Click
        'Protected Sub ButtonSuspendWork_Click(sender As Object, e As System.EventArgs) Handles ButtonSuspendWork.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2012/03/02 上田 フッタボタン制御 Start
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Try
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END
        '押したフッタボタンの状態を、「当日処理」に設定する.
        HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_SUSPEND_WORK
        'Logger.Info("ButtonSuspendWork_Click HiddenPushedFooter=" + PUSHED_FOOTER_BUTTON_SUSPEND_WORK)

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value
        'Logger.Info("ButtonSuspendWork_Click selectedRezId=" + selectedRezId)

        '取得した予約IDがNull値でない場合のみ
        If (Not String.IsNullOrEmpty(selectedRezId)) Then
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()
            ''Logger.Info("ButtonSuspendWork_Click resultInterference=" + CType(resultInterference, String))

            ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
            'If (resultInterference = INTERFERENCE_FAILE) Then
            '    'Logger.Info("ButtonSuspendWork_Click HiddenBreakPopup=" + POPUP_BREAK_DISPLAY)
            '    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

            'ElseIf (ValidationInterferenceBreakUnavailable() = INTERFERENCE_SUCCESSFULL) Then
            '    'Logger.Info("ButtonSuspendWork_Click ValidationInterferenceBreakUnavailable is successfull")
            '    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、当日処理関数を呼び出す.
            '    SuspendWorkProcess(False)

            'End If

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

                    If (resultInterference = INTERFERENCE_FAILE) Then
                        '休憩に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                        HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY
                    ElseIf (ValidationInterferenceBreakUnavailable() = INTERFERENCE_SUCCESSFULL) Then
                        '干渉しない場合、当日処理関数を呼び出す.
                        SuspendWorkProcess(False)
                    End If
                Else
                    '当日処理関数を呼び出す.
                    SuspendWorkProcess(False)
                End If

            End Using

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        End If
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Finally
        'Me.HiddenReloadFlag.Value = String.Empty
        'End Try
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END
        '2012/03/02 上田 フッタボタン制御 End

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 当日終了処理
    ''' </summary>
    ''' <param name="breakExtention">休憩取得有無</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <remarks></remarks>
    Private Sub SuspendWorkProcess(ByVal breakExtention As Boolean, _
                                   Optional ByVal breakBottomFlg As Boolean = False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value
        'Logger.Info("SuspendWorkProcess selectedId=" + selectedRezId)

        '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START

        Dim orderNo As String = Me.HiddenFieldOrderNo.Value

        '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)　START
        '行ロックバージョンを取得する
        Dim selectedUpdateCount As Long
        selectedUpdateCount = CType(Me.HiddenSelectedUpdateCount.Value.Trim, Long)
        '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)　END

        '当日終了処理を実施する.
        Dim resultEvent As Integer
        resultEvent = businessLogic.SuspendWork(objStaffContext.DlrCD, _
                                                objStaffContext.BrnCD, _
                                                CType(selectedRezId, Decimal), _
                                                Me.stallId, _
                                                objStaffContext.Account, _
                                                orderNo, _
                                                selectedUpdateCount, _
                                                breakExtention, _
                                                breakBottomFlg)

        'Logger.Info("SuspendWorkProcess resultEvent=" + CType(resultEvent, String))
        '2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''正常に終了していない場合、干渉エラーのエラーメッセージを表示する.
        'If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then
        '    'Logger.Info("SuspendWorkProcess The selected Chip is overlapping with next  arranged Chip")
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
        '    'MyBase.ShowMessageBox(resultEvent, "The selected Chip is overlapping with next  arranged Chip")
        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultEvent, workMidFinishFlg)
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END
        '    Exit Sub
        'End If

        '当日終了処理結果チェック
        If resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultEvent <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            showErrMsgAndRefresh(resultEvent, workStartFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using tabletSmbCommonClass As New TabletSMBCommonClassBusinessLogic
            'SAへ日跨ぎ終了の通知処理
            tabletSmbCommonClass.SendNoticeByJobStop(objStaffContext, Me.stallId)
        End Using

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()

        '2012/06/15 KN 西田 STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない START
        Me.HiddenFieldEndWorkFlg.Value = WORK_END_FLG
        '2012/06/15 KN 西田 STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない END

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '当日終了処理結果チェック
        If resultEvent = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' 検査開始ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonStartCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonStartCheck.Click
        'Protected Sub ButtonStartCheck_Click(sender As Object, e As System.EventArgs) Handles ButtonStartCheck.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '押したフッタボタンの状態を、「検査開始」に設定する.
        HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_START_CHECK
        'Logger.Info("ButtonStartCheck_Click HiddenPushedFooter=" + PUSHED_FOOTER_BUTTON_START_CHECK)

        '完成検査入力を押下したときと同様の動作
        CompletionCheckButton_Click(sender, e)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)　START

    ''' <summary>
    ''' 作業終了ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonFinishWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonFinishWork.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '押したフッタボタンの状態を、「作業終了」に設定する.
        HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_FINISH_WORK

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value

        '取得した予約IDがNull値でない場合のみ
        If Not String.IsNullOrEmpty(selectedRezId) Then
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

            ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
            'If (resultInterference = INTERFERENCE_FAILE) Then

            '    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

            'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

            '    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、作業終了処理関数を呼び出す.
            '    FinishWorkProcess(False)

            'End If

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

                    If (resultInterference = INTERFERENCE_FAILE) Then
                        '休憩に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                        HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY
                    ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then
                        '干渉しない場合、作業終了処理関数を呼び出す.
                        FinishWorkProcess(False)
                    End If
                Else
                    '作業終了処理関数を呼び出す.
                    FinishWorkProcess(False)
                End If

            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        End If

        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Me.HiddenReloadFlag.Value = String.Empty
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 作業終了処理
    ''' </summary>
    ''' <param name="breakExtention">休憩取得有無</param>
    ''' <param name="breakBottomFlg">休憩取得判定ボタン押下有無</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Sub FinishWorkProcess(ByVal breakExtention As Boolean, _
                                  Optional ByVal breakBottomFlg As Boolean = False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim orderNo As String = Me.HiddenFieldOrderNo.Value

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value

        '行ロックバージョンを取得する
        Dim selectedUpdateCount As Long
        selectedUpdateCount = CType(Me.HiddenSelectedUpdateCount.Value.Trim, Long)

        '作業終了処理を実施する.
        Dim resultEvent As Integer
        resultEvent = businessLogic.FinishWork(objStaffContext.DlrCD, _
                                               objStaffContext.BrnCD, _
                                               CType(selectedRezId, Decimal), _
                                               Me.stallId, _
                                               objStaffContext.Account, _
                                               selectedUpdateCount, _
                                               orderNo, _
                                               breakExtention, _
                                               breakBottomFlg)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''正常に終了していない場合、干渉エラーのエラーメッセージを表示する.
        'If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
        '    'MyBase.ShowMessageBox(resultEvent)
        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultEvent, workFinishFlg)
        '    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END
        '    Exit Sub
        'End If

        '終了処理結果チェック
        If resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultEvent <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            Me.showErrMsgAndRefresh(resultEvent, workFinishFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        'PUSH送信実行判断
        If businessLogic.NeedPushFinishSingleJob Or businessLogic.NeedPushStopSingleJob Then


            '選択中のチップの作業内容ID取得
            Dim jobDatilId As Decimal = 0
            If Not String.IsNullOrEmpty(Me.HiddenSelectedJobDetailId.Value) Then
                jobDatilId = CType(Me.HiddenSelectedJobDetailId.Value, Decimal)
            End If

            'SAへの通知処理
            businessLogic.NoticeMainProcessing(jobDatilId, objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext)

            'Push送信
            businessLogic.WorkEndSendPush(objStaffContext.DlrCD, _
                                          objStaffContext.BrnCD, _
                                          objStaffContext.Account, _
                                          orderNo, _
                                          jobDatilId, _
                                          Me.stallId)
        End If
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()

        Me.HiddenFieldEndWorkFlg.Value = WORK_END_FLG

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '終了処理結果チェック
        If resultEvent = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理)　END

    ''' <summary>
    ''' 追加作業ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonAddWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonAddWork.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1}  START. " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '他システムとの画面連携
        ScreenLinkage(DISPLAY_NUMBER_23)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1}  END. " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START

    ''' <summary>
    ''' 作業中断ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonJobStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonJobStop.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1}  START. " _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '押したフッタボタンの状態を、「作業終了」に設定する.
        HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_STOP_WORK

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value

        '取得した予約IDがNull値でない場合のみ
        If Not String.IsNullOrEmpty(selectedRezId) Then
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

            ''休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
            'If (resultInterference = INTERFERENCE_FAILE) Then

            '    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

            'ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then

            '    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、作業終了処理関数を呼び出す.
            '    StopWorkProcess(False)

            'End If

            Using biz As New TabletSMBCommonClassBusinessLogic
                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

                    If (resultInterference = INTERFERENCE_FAILE) Then
                        '休憩に完了する場合、作業伸長ポップアップの表示フラグをセットする.
                        HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY
                    ElseIf (resultInterference = INTERFERENCE_SUCCESSFULL) Then
                        '干渉しない場合、作業中断処理関数を呼び出す.
                        StopWorkProcess(False)
                    End If
                Else
                    '作業中断処理関数を呼び出す.
                    StopWorkProcess(False)
                End If

            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        End If

        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 START
        'Me.HiddenReloadFlag.Value = String.Empty
        '2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題 END


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1}  END. " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 作業中断処理
    ''' </summary>
    ''' <param name="breakExtention">休憩取得有無</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Private Sub StopWorkProcess(ByVal breakExtention As Boolean)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START
        'Private Sub StopWorkProcess(ByVal breakExtention As Boolean, _
        '                        Optional ByVal breakBottomFlg As Boolean = False)
        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim orderNo As String = Me.HiddenFieldOrderNo.Value
        Dim minuteString As String = WebWordUtility.GetWord(APPLICATION_ID, 39)

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String = Me.HiddenSelectedReserveId.Value
        Dim intSelectedRezId As Decimal = 0

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Decimal.TryParse(selectedRezId, intSelectedRezId)

        'ストール利用IDのHidden値チェック
        If Not (String.IsNullOrEmpty(Trim(selectedRezId))) Then
            '存在する場合
            'Hidden値を設定
            intSelectedRezId = Decimal.Parse(selectedRezId, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        Dim stopReasonType As String = STOP_REASON_TYPE_OTHER
        stopReasonType = Me.HiddenStopReasonType.Value

        Dim strStopTime As String = Nothing
        strStopTime = Me.HiddenStopTime.Value

        Dim stopMemo As String = Space(1)
        stopMemo = Me.HiddenStopMemo.Value

        Dim longStopTime As Long = 0
        Dim restFlg As String = TakeBreakFlg

        '行ロックバージョンを取得する
        Dim selectedUpdateCount As Long = CType(Me.HiddenSelectedUpdateCount.Value.Trim, Long)

        '中断時間の単位を削除し、数値型に変換
        strStopTime = strStopTime.Replace(minuteString, "")

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Long.TryParse(strStopTime, longStopTime)

        '中断時間値チェック
        If Not (String.IsNullOrEmpty(Trim(strStopTime))) Then
            '存在する場合
            'Hidden値を設定
            longStopTime = Long.Parse(strStopTime, CultureInfo.InvariantCulture)

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '休憩取得ポップアップで、取得しないを選択した場合
        If Not breakExtention Then
            restFlg = DoNotBreakFlg
        End If

        '作業中断処理を実施する.
        Dim resultEvent As Long
        resultEvent = businessLogic.JobStop(intSelectedRezId, _
                                            longStopTime, _
                                            stopMemo, _
                                            stopReasonType, _
                                            restFlg, _
                                            selectedUpdateCount, _
                                            APPLICATION_ID)

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''正常に終了していない場合、干渉エラーのエラーメッセージを表示する.
        'If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then

        '    'エラーメッセージを出して、画面リフレッシュ
        '    showErrMsgAndRefresh(resultEvent, workFinishFlg)

        '    Exit Sub
        'End If

        '作業中断処理結果チェック
        If resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL AndAlso _
           resultEvent <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」ではない場合
            'エラーメッセージを出して、画面リフレッシュ
            showErrMsgAndRefresh(resultEvent, workFinishFlg)
            Exit Sub

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'PUSH送信実行判断
        If businessLogic.NeedPushStopSingleJob Then

            '選択中のチップの作業内容ID取得
            Dim jobDatilId As Decimal = 0
            If Not String.IsNullOrEmpty(Me.HiddenSelectedJobDetailId.Value) Then
                jobDatilId = CType(Me.HiddenSelectedJobDetailId.Value, Decimal)
            End If


            'Push送信
            businessLogic.WorkEndSendPush(objStaffContext.DlrCD, _
                                          objStaffContext.BrnCD, _
                                          objStaffContext.Account, _
                                          orderNo, _
                                          jobDatilId, _
                                          Me.stallId)

            '実績チップに変わるのでフィルターを設定
            Me.HiddenFieldEndWorkFlg.Value = WORK_END_FLG
        End If

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '作業中断処理結果チェック
        If resultEvent = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            Me.showMessageWarningOmitDmsError()

        End If

        '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

#End Region

#Region "バリデーション"

    'BizLogicの開始処理・終了処理・当日処理内にて実施するため、処理開始前のバリデーションはコメントアウトする.

    ' ''' <summary>
    ' ''' バリデーション（入庫済み）
    ' ''' </summary>
    ' ''' <returns>成否</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationEnterTheShed(aCandidateDataInfo As DataRow) As Boolean

    '    Logger.Info("ValidationEnterTheShed Start")

    '    Dim isEnterTheShed As Boolean = False

    '    '入庫日時がDBNullでない場合、バリデーション開始
    '    If Not (IsDBNull(aCandidateDataInfo("STRDATE"))) Then

    '        'Dim dtmStrDate As DateTime = Date.ParseExact(aCandidateDataInfo("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        'Dim dtmStrDate As DateTime = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", aCandidateDataInfo("STRDATE").ToString)
    '        '入庫日時が設定されているため、入庫済みであると判定
    '        isEnterTheShed = True

    '    End If

    '    Logger.Info("ValidationEnterTheShed End")
    '    Return isEnterTheShed

    'End Function


    ' ''' <summary>
    ' ''' バリデーション（本予約）
    ' ''' </summary>
    ' ''' <returns>成否</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationReserve(aCandidateDataInfo As DataRow) As Boolean

    '    Logger.Info("ValidationReserve Start")

    '    Dim blnValidation As Boolean = False

    '    'ステータスがDBNullでない場合、バリデーション開始
    '    If Not (IsDBNull(aCandidateDataInfo("STATUS"))) Then

    '        Dim intStatus As Integer = CType(aCandidateDataInfo("STATUS").ToString(), Integer)
    '        'ステータスが本予約の値であるならば、チェックを通す
    '        If (intStatus = STATUS_RESERVE) Then
    '            blnValidation = True
    '        End If

    '    End If

    '    Logger.Info("ValidationReserve End")
    '    Return blnValidation

    'End Function



    ' ''' <summary>
    ' ''' バリデーション（チップ干渉）
    ' ''' </summary>
    ' ''' <param name="aChipInfo"></param>
    ' ''' <param name="aCandidateDataInfo"></param>
    ' ''' <returns>バリデーション結果</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterference(aChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable, _
    '                                        aCandidateDataInfo As DataRow) As Integer

    '    Logger.Info("ValidationInterference Start")

    '    Dim isInterfere As Integer = INTERFERENCE_FAILE
    '    Dim dtmEstimateEndTime As Date

    '    '該当するチップの開始時間（予定）と終了時間（予定）を取得する.
    '    If (IsDBNull(aCandidateDataInfo("STARTTIME")) Or IsDBNull(aCandidateDataInfo("ENDTIME"))) Then
    '        isInterfere = INTERFERENCE_FAILE
    '    Else
    '        Dim objRezId As Object = aCandidateDataInfo("REZID")
    '        'Dim dtmStart As Date = Date.ParseExact(aCandidateDataInfo("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        'Dim dtmEnd As Date = Date.ParseExact(aCandidateDataInfo("ENDTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        Dim dtmStart As Date = CType(aCandidateDataInfo("STARTTIME"), Date)
    '        Dim dtmEnd As Date = CType(aCandidateDataInfo("ENDTIME"), Date)
    '        Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)

    '        '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
    '        Dim dtmNowTime As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
    '        dtmEstimateEndTime = dtmNowTime.Add(ts)

    '        '作業チップとの干渉チェックを実施する.
    '        If (ValidationInterferenceWorkChip(dtmNowTime, dtmEstimateEndTime, objRezId, aChipInfo)) Then

    '        End If

    '        '作業チップと休憩チップの干渉チェックを実施する.
    '        Dim breakInterfereEndTime As Date
    '        breakInterfereEndTime = ValidationInterferenceBreakChip(dtmNowTime, dtmEstimateEndTime)
    '        '休憩チップとの干渉チェックをした結果、返り値の時間が引数の終了時間と異なる場合、干渉が発生している.
    '        If (breakInterfereEndTime <> dtmEstimateEndTime) Then

    '        End If

    '        '使用不可チップとの干渉をチェックした結果、返り値の時間が引数の終了時間と異なる場合、干渉が発生している.
    '        Dim unavailableInterfereEndTime As Date
    '        unavailableInterfereEndTime = ValidationInterferenceUnavailableChip(dtmNowTime, dtmEstimateEndTime)
    '        '休憩チップ、使用不可チップとの干渉チェックの結果、時間が更新されていれば重複があったものする.

    '        '再度チップと干渉を確認する（休憩・使用不可チップの伸長により、作業終了予定時間が変更されている可能性があるため）.

    '    End If

    '    Logger.Info("ValidationInterference End")

    '    isInterfere = INTERFERENCE_SUCCESSFULL

    '    Return isInterfere

    'End Function



    ' ''' <summary>
    ' ''' 作業チップの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aNowTime">作業開始時刻</param>
    ' ''' <param name="aEstimateEndTime">推定作業完了時刻</param>
    ' ''' <param name="aCandidateRezId">現在選択中チップ情報</param>
    ' ''' <param name="aChipInfo">作業チップ情報</param>
    ' ''' <returns>干渉結果</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceWorkChip(aNowTime As Date, aEstimateEndTime As Date, aCandidateRezId As Object, _
    '                                                                    aChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable) As Boolean

    '    '仮予約チップと干渉した場合、その仮予約チップを動かした状態で干渉チェックを実施することになる.
    '    '選択中チップの推定作業完了時刻は変更ないが、仮予約と干渉した場合、その仮予約の推定作業完了時刻を推定作業完了時刻として以降をチェックする.
    '    '最終的に、その状態で、他の本予約チップと干渉しない＋ストール作業時間内であるという条件を満たせば、
    '    '選択チップの推定作業完了時刻でOKということになる.
    '    '前提条件として、作業チップ情報は、開始時間でソートされている必要がある.

    '    Logger.Info("ValidationInterferenceWorkChip Start")

    '    Dim isInterfere As Boolean = True

    '    Dim dtmNowTime As Date = aNowTime
    '    Dim dtmEstimateEndTime As Date = aEstimateEndTime

    '    '所持しているチップ情報をすべてループして、他のチップとの干渉をチェック
    '    'For Each dr As DataRow In aChipInfo
    '    For i As Integer = 0 To (aChipInfo.Count - 1) Step 1

    '        Dim dr As DataRow = aChipInfo(i)

    '        '対象チップ以外の場合のみ干渉をチェック
    '        If Not aCandidateRezId.Equals(dr("REZID")) Then

    '            Dim dtmSTime2 As DateTime
    '            If Not IsDBNull(dr("RESULT_START_TIME")) Then
    '                '開始時間（実績）を取得
    '                'dtmSTime2 = ExchangeTimeString(dr("RESULT_START_TIME").ToString())
    '                'dtmSTime2 = Date.ParseExact(dr("RESULT_START_TIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmSTime2 = CType(dr("RESULT_START_TIME"), Date)
    '            Else
    '                '開始時間（実績）がない場合、開始時間（予定）を取得
    '                'dtmSTime2 = Date.ParseExact(dr("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmSTime2 = CType(dr("STARTTIME"), Date)
    '            End If

    '            Dim dtmETime2 As DateTime
    '            If Not IsDBNull(dr("RESULT_END_TIME")) Then
    '                '終了時間（実績）を取得
    '                'dtmETime2 = ExchangeTimeString(dr("RESULT_END_TIME").ToString())
    '                'dtmETime2 = Date.ParseExact(dr("RESULT_END_TIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmETime2 = CType(dr("RESULT_END_TIME"), Date)
    '            Else
    '                '終了時間（実績）がない場合、終了時間（予定）を取得
    '                'dtmETime2 = Date.ParseExact(dr("ENDTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmETime2 = CType(dr("ENDTIME"), Date)
    '            End If

    '            '計算対象の開始時間が、干渉チェック中チップの終了時間より小さい場合、且つ、
    '            '干渉チェック中チップの開始時間が、計算対象の終了時間より小さい場合、「干渉する」と判定
    '            If ((dtmNowTime < dtmETime2) And (dtmSTime2 < dtmEstimateEndTime)) Then
    '                'チップの干渉が確認された際に、干渉チェック中のチップステータスを確認する
    '                'Dim drStatus = CType(dr("Status"), Integer)
    '                'ここで干渉後の
    '                isInterfere = False
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceWorkChip End")
    '    Return isInterfere

    'End Function


    ' ''' <summary>
    ' ''' 休憩チップと作業対象チップの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aNowTime">作業対象チップの開始時間</param>
    ' ''' <param name="aEstimateEndTime">作業対象チップの終了時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceBreakChip(aNowTime As Date, aEstimateEndTime As Date) As Date

    '    Logger.Info("ValidationInterferenceBreakChip Start param1:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aNowTime) + _
    '                " param2:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aEstimateEndTime))

    '    '返り値となる終了時間を初期化する.
    '    Dim checkedEndTime As Date = aEstimateEndTime

    '    '時間ソートされた休憩チップ情報を取得する.
    '    'Dim breakDataTable As SC3150101DataSet.SC3150101BreakChipInfoDataTable
    '    Dim breakDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
    '    breakDataTable = businessLogic.GetBreakData(Me.stallId)

    '    '取得した休憩チップ情報をループ処理し、作業対象チップとの干渉を検証する.
    '    '干渉が発生した場合、その干渉を加算した終了時間を計算する.
    '    For Each eachBreakData As DataRow In breakDataTable.Rows

    '        Dim eachStartTime As Date = CType(eachBreakData("STARTTIME"), Date)
    '        Dim eachEndTime As Date = CType(eachBreakData("ENDTIME"), Date)

    '        If ((aNowTime < eachEndTime) And (eachStartTime < checkedEndTime)) Then
    '            Dim breakTime = eachEndTime - eachStartTime
    '            checkedEndTime = checkedEndTime.Add(breakTime)
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceBreakChip End Return:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, checkedEndTime))
    '    Return checkedEndTime

    'End Function


    ' ''' <summary>
    ' ''' 使用不可チップと作業対象チップとの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aTargetStartTime">作業対象チップの開始時間</param>
    ' ''' <param name="aTargetEndTime">作業対象チップの終了時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceUnavailableChip(aTargetStartTime As Date, aTargetEndTime As Date) As Date

    '    Logger.Info("ValidationInterferenceUnavailableChip Start param1:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aTargetStartTime) + _
    '                " param2:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aTargetEndTime))

    '    '返り値となる終了時間を初期化する.
    '    Dim checkedEndTime As Date = aTargetEndTime

    '    '時間ソートされた使用不可チップ情報を取得する.
    '    Dim unavailableDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
    '    unavailableDataTable = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

    '    '取得した使用不可チップ情報をループ処理し、作業対象チップとの干渉を検証する.
    '    '干渉が発生した場合、その干渉を加算した終了時間を計算する.
    '    For Each eachUnavailableData As DataRow In unavailableDataTable.Rows

    '        Dim eachStartTime As Date = CType(eachUnavailableData("STARTTIME"), Date)
    '        Dim eachEndTime As Date = CType(eachUnavailableData("ENDTIME"), Date)

    '        If ((aTargetStartTime < eachEndTime) And (eachStartTime < checkedEndTime)) Then
    '            Dim breakTime = eachEndTime - eachStartTime
    '            checkedEndTime = checkedEndTime.Add(breakTime)
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceUnavailableChip End Return:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, checkedEndTime))
    '    Return checkedEndTime

    'End Function

    '↓ここから休憩・使用不可検証
    ''' <summary>
    ''' バリデーション（休憩・使用不可チップとの干渉）
    ''' </summary>
    ''' <returns>バリデーション結果</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceBreakUnavailable() As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ''チップ情報の最新を取得し、作業対象チップを取得する.
        'Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        'dtChipInfo = GetNewestChipInfo()
        'Dim drCandidateChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        'drCandidateChipInfo = GetCandidateChipInfo(dtChipInfo)

        '選択中のチップ情報を取得する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = GetSelectedChipInfo()

        Dim isInterfere As Integer = INTERFERENCE_SUCCESSFULL
        Dim dtmEstimateEndTime As Date
        Dim dtmNowTime As Date

        '作業対象チップが存在する場合、バリデーションを実施する.
        'If (Not IsNothing(drCandidateChipInfo)) Then
        If (Not IsNothing(selectedChipInfo)) Then
            'Logger.Info("ValidationInterferenceBreakUnavailable Not IsNothing selectedChipInfo")
            '該当するチップの開始時間（予定）と終了時間（予定）を取得する.
            'If (IsDBNull(drCandidateChipInfo.STARTTIME) Or IsDBNull(drCandidateChipInfo.ENDTIME)) Then
            If (selectedChipInfo.IsSTARTTIMENull()) Or (selectedChipInfo.IsENDTIMENull()) Then
                'Logger.Info("ValidationInterferenceBreakUnavailable selectedChipInfo.STARTTIME is DBNull or selectedChipInfo.ENDTIME is DBNull")
                isInterfere = INTERFERENCE_FAILE
            Else

                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START

                'チップが作業中か、一部作業中断中の場合
                If (Not selectedChipInfo.IsSTALL_USE_STATUSNull) AndAlso _
                    ((selectedChipInfo.STALL_USE_STATUS.Equals(stallUseStatus_Working)) Or _
                     (selectedChipInfo.STALL_USE_STATUS.Equals(stallUseStatus_StopPart))) Then

                    '実績開始日時と終了見込日時を格納する
                    dtmNowTime = selectedChipInfo.RESULT_START_TIME
                    dtmEstimateEndTime = selectedChipInfo.RESULT_END_TIME

                Else
                    '予定開始日時と予定終了日時を格納する
                    Dim dtmStart As Date = selectedChipInfo.STARTTIME
                    Dim dtmEnd As Date = selectedChipInfo.ENDTIME
                    Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)

                    '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
                    dtmNowTime = DateTimeFunc.Now(objStaffContext.DlrCD)
                    dtmEstimateEndTime = dtmNowTime.Add(ts)
                End If

                'Dim dtmStart As Date = drCandidateChipInfo.STARTTIME
                'Dim dtmEnd As Date = drCandidateChipInfo.ENDTIME
                'Dim dtmStart As Date = selectedChipInfo.STARTTIME
                'Dim dtmEnd As Date = selectedChipInfo.ENDTIME
                'Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)
                'Logger.Info("ValidationInterferenceBreakUnavailable startTime:" + DateTimeFunc.FormatDate(1, dtmStart))
                'Logger.Info("ValidationInterferenceBreakUnavailable endTime:" + DateTimeFunc.FormatDate(1, dtmEnd))
                'Logger.Info("ValidationInterferenceBreakUnavailable endTime-startTime=timespan:" + ts.ToString())

                '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
                'Dim dtmNowTime As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
                'dtmEstimateEndTime = dtmNowTime.Add(ts)
                'Logger.Info("ValidationInterferenceBreakUnavailable estimateEndTime:" + DateTimeFunc.FormatDate(1, dtmEstimateEndTime))

                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

                '休憩チップとの干渉チェックし、干渉が発生する場合、干渉発生を返す.
                If (ValidationInterferenceBreakChip(dtmNowTime, dtmEstimateEndTime)) Then
                    'Logger.Info("ValidationInterferenceBreakUnavailable Interference BreakChip")
                    isInterfere = INTERFERENCE_FAILE

                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    'Else
                    '    'Logger.Info("ValidationInterferenceBreakUnavailable ")
                    '    '使用不可チップとの干渉をチェックし、干渉が発生する場合、干渉発生を返す.
                    '    If (ValidationInterferenceUnavailableChip(dtmNowTime, dtmEstimateEndTime)) Then
                    '        'Logger.Info("ValidationInterferenceBreakUnavailable Interference UnavailableChip")
                    '        isInterfere = INTERFERENCE_FAILE
                    '    End If
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                End If
            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
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
        'Dim breakDataTable As SC3150101DataSet.SC3150101BreakChipInfoDataTable
        Dim breakDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
        breakDataTable = businessLogic.GetBreakData(Me.stallId)

        '取得した休憩チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachBreakData As DataRow In breakDataTable.Rows
            'Logger.Info("ValidationInterferenceBreakChip ")

            Dim eachStartTime As Date = CType(eachBreakData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachBreakData("ENDTIME"), Date)
            'Logger.Info("ValidationInterferenceBreakChip eachStartTime:" + DateTimeFunc.FormatDate(1, eachStartTime))
            'Logger.Info("ValidationInterferenceBreakChip eachEndTime:" + DateTimeFunc.FormatDate(1, eachEndTime))

            If ((aNowTime < eachEndTime) And (eachStartTime < aEstimateEndTime)) Then
                'Logger.Info("ValidationInterferenceBreakChip ((startTime < eachEndTime) AND (eachEndTime < endTime))")
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
        Dim unavailableDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
        unavailableDataTable = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        '取得した使用不可チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachUnavailableData As DataRow In unavailableDataTable.Rows
            'Logger.Info("ValidationInterferenceUnavailableChip ")

            Dim eachStartTime As Date = CType(eachUnavailableData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachUnavailableData("ENDTIME"), Date)
            'Logger.Info("ValidationInterferenceUnavailableChip eachStartTime:" + DateTimeFunc.FormatDate(1, eachStartTime))
            'Logger.Info("ValidationInterferenceUnavailableChip eachEndTime:" + DateTimeFunc.FormatDate(1, eachEndTime))

            If ((aTargetStartTime < eachEndTime) And (eachStartTime < aTargetEndTime)) Then
                'Logger.Info("ValidationInterferenceUnavailableChip ((startTime < eachEndTime) AND (eachEndTime < endTime))")
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

#End Region

#Region "JavaScriptのイベントよりコールされる処理"

    ''' <summary>
    ''' R/O情報欄をフリックした際のイベント処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonFlickRepairOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles HiddenButtonFlickRepairOrder.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O情報欄のフリック時フラグオン
        Me.flickRoInformationFlag = 1

        '完成検査入力画面へ遷移 CompletionCheckButton
        CompletionCheckButton_Click(sender, e)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' R/O情報の追加作業アイコンをタップした際の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRepairOrderIcon_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles HiddenButtonRepairOrderIcon.Click

        '    Logger.Error("HiddenButtonRepairOrderIcon_Click.S")

        '    Dim iconNumber As Integer = CType(Me.HiddenFieldRepairOrderIcon.Value, Integer)     '=WorkSeq
        '    'Logger.Info("HiddenButtonRepairOrderIcon_Click AddWorkIconNumber:" + CType(iconNumber, String))

        '    Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        '    Dim editValue As String = "0"
        '    Dim nextPageId As String = REPAIR_ORDERE_PREVIEW_PAGE

        '    '追加作業ボタンが押下された場合、追加作業情報プレビュー画面へ遷移.
        '    If (0 < iconNumber) Then
        '        'Logger.Info("HiddenButtonRepairOrderIcon_Click Pushed AddWorkIcon")

        '        Dim tactSrvAddSeq As String = businessLogic.GetTactChildNo(Me.objStaffContext.DlrCD, orderNumber, iconNumber)        'TACTの枝番取得

        '        Logger.Error("Redirect.ORDERNO:" + orderNumber)
        '        Logger.Error("Redirect.SRVADDSEQ:" + tactSrvAddSeq)
        '        Logger.Info("Redirect.EDITFLG:" + editValue)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", tactSrvAddSeq)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editValue)

        '        nextPageId = ADD_REPAIR_PREVIEW_PAGE
        '    Else
        '        'Rボタンが押下された場合、R/Oプレビュー画面へ遷移.
        '        'Logger.Info("HiddenButtonRepairOrderIcon_Click Pushed R/O Icon")

        '        Logger.Error("OrderNo:" + orderNumber)
        '        MyBase.SetValue(ScreenPos.Next, "OrderNo", orderNumber)

        '        nextPageId = REPAIR_ORDERE_PREVIEW_PAGE
        '    End If

        '    Logger.Error("HiddenButtonRepairOrderIcon_Click.E RedirectNextScreenID:" + nextPageId)
        '    Me.RedirectNextScreen(nextPageId)

        '2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　START
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O番号を取得する.
        Dim orderNumber As String
        orderNumber = Me.HiddenHistoryOrderNumber.Value

        'RO枝番を取得
        Dim orderNumberSeq As String = Me.HiddenHistoryOrderNumberSeq.Value

        ScreenLinkage(DISPLAY_NUMBER_13, _
                      orderNumber, _
                      String.Empty, _
                      String.Empty, _
                      orderNumberSeq)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　END

    End Sub

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 本ページに使用しているインラインフレームに渡すセッション情報を設定する.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonChipTap_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles HiddenButtonChipTap.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'セッションに格納する前に、格納する値をセッションより除去する.
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.ORDERNO")
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.SRVADDSEQ")     ' TACT側の枝番（Removeするだけでよい。SC3150102の方にこのセッション値を使っていないため、設定不要）
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.FILTERFLG")
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.SELECTED_ID")
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.WORKSEQ")       ' 作業連番
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.REZID")         ' 予約ID
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.INSTRUCT")      ' 着工指示区分
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.VCLREGNO")      ' 車輌登録番号(FMへの呼出し通知用)
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.STALLNAME")     ' ストール名(FMへの呼出し通知用)

        Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        Dim repairOrderFilter As String = Me.HiddenFieldRepairOrderFilter.Value
        Dim selectedId As String = Me.HiddenSelectedId.Value
        Dim workSeq As String = Me.HiddenSelectedWorkSeq.Value
        If String.IsNullOrEmpty(Me.HiddenSelectedWorkSeq.Value) Then
            workSeq = "-1"
        End If
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Dim rezId As String = Me.HiddenSelectedReserveId.Value
        Dim jobDetailId As String = Me.HiddenSelectedJobDetailId.Value
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        Dim instruct As String = Me.HiddenFieldInstruct.Value
        Dim vclRegNo As String = Me.HiddenSelectedVclRegNo.Value
        Dim stallName As String = Me.LabelStallName.Text
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
        Dim stallUseId As String = Me.HiddenSelectedReserveId.Value
        Dim rowUpdateCount As String = Me.HiddenSelectedUpdateCount.Value
        Dim stallUseStatus As String = Me.HiddenSelectedStallUseStatus.Value
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

        MyBase.SetValue(ScreenPos.Current, "Redirect.ORDERNO", orderNumber)             ' オーダーナンバーをセッションに格納する.
        MyBase.SetValue(ScreenPos.Current, "Redirect.FILTERFLG", repairOrderFilter)     ' R/O情報欄のフィルターフラグをセッションに格納する.
        MyBase.SetValue(ScreenPos.Current, "Redirect.SELECTED_ID", selectedId)          ' 他画面に遷移した後、TCに戻ると遷移前のチップが選択されない問題の対処
        MyBase.SetValue(ScreenPos.Current, "Redirect.WORKSEQ", workSeq)                 ' 作業連番
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'MyBase.SetValue(ScreenPos.Current, "Redirect.REZID", rezId)                     ' 予約ID
        MyBase.SetValue(ScreenPos.Current, "Redirect.REZID", jobDetailId)                     ' 予約ID(作業内容ID)
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        MyBase.SetValue(ScreenPos.Current, "Redirect.INSTRUCT", instruct)               ' 着工指示区分
        MyBase.SetValue(ScreenPos.Current, "Redirect.VCLREGNO", vclRegNo)               ' 車輌登録番号(FMへの呼出し通知用)
        MyBase.SetValue(ScreenPos.Current, "Redirect.STALLNAME", stallName)             ' ストール名(FMへの呼出し通知用)
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
        MyBase.SetValue(ScreenPos.Current, "Redirect.STALLUSEID", stallUseId)
        MyBase.SetValue(ScreenPos.Current, "Redirect.STALLID", Me.stallId)
        MyBase.SetValue(ScreenPos.Current, "Redirect.ROWUPDATECOUNT", rowUpdateCount)
        MyBase.SetValue(ScreenPos.Current, "Redirect.STALLUSESTATUS", stallUseStatus)
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

        Logger.Info("Redirect.ORDERNO:" + orderNumber)
        Logger.Info("Redirect.FILTERFLG:" + repairOrderFilter)
        Logger.Info("Redirect.SELECTED_ID:" + selectedId)
        Logger.Info("Redirect.WORKSEQ:" + workSeq)            ' 作業連番
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Logger.Info("Redirect.REZID:" + rezId)                 ' 予約ID
        Logger.Info("Redirect.REZID(JOB_DTL_ID):" + jobDetailId) ' 作業内容ID
        '2013/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        Logger.Info("Redirect.INSTRUCT;" + instruct)           ' 着工指示区分
        Logger.Info("Redirect.VCLREGNO:" + vclRegNo)          ' 車輌登録番号(FMへの呼出し通知用)
        Logger.Info("Redirect.STALLNAME;" + stallName)         ' ストール名(FMへの呼出し通知用)
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  START
        Logger.Info("Redirect.STALLUSEID:" + stallName)          ' ストール利用ID
        Logger.Info("Redirect.STALLUSESTATUS:" + stallUseStatus) ' ストール利用ステータス
        Logger.Info("Redirect.Redirect.STALLID:" + Me.stallId.ToString(CultureInfo.CurrentCulture()))      ' 休憩フラグ
        Logger.Info("Redirect.ROWUPDATECOUNT:" + rowUpdateCount) ' 行ロックバージョン
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発  END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' Push通信がきたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRefresh.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        If OPERATION_CODE_TC = objStaffContext.OpeCD Then

            ' 再表示
            Me.RedirectNextScreen(APPLICATION_ID)

        ElseIf OPERATION_CODE_CHT = objStaffContext.OpeCD Then

            'ストールIDをセッションに格納
            Me.SetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, stallId)
            ' チーフテクニシャンのメインメニュー画面に遷移
            Me.RedirectNextScreen(APPLICATION_ID)

        End If
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' 履歴情報がタップされたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonHistory.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
         , "{0}.{1}  STRAT. " _
         , Me.GetType.ToString _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O番号を取得する.
        Dim orderNumber As String
        orderNumber = Me.HiddenHistoryOrderNumber.Value

        '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        '入庫管理番号取得
        Dim serviceInNumber As String = Me.HiddenServiceInNumber.Value
        '基幹販売店コード
        Dim ServiceInDealerCode As String = Me.HiddenHistoryDealerCode.Value

        'R/O番号が空文字でない場合、遷移処理を実施する.
        'If (orderNumber.Length > 0) Then
        If Not orderNumber.Equals(Space(1)) Then

            'Logger.Info("HiddenButtonHistory_Click orderNumber is not blank")

            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_ORDERNO:" + orderNumber)
            'MyBase.SetValue(ScreenPos.Next, "OrderNo", orderNumber)

            ''2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
            ''R/Oプレビュー画面へ遷移.
            ''Logger.Info("HiddenButtonHistory_Click End NextScreen:" + REPAIR_ORDERE_PREVIEW_PAGE)
            ''Me.RedirectNextScreen(REPAIR_ORDERE_PREVIEW_PAGE)
            ''販売店CDを取得する.
            'Dim dealerCode As String
            'dealerCode = Me.HiddenHistoryDealerCode.Value

            'If Me.objStaffContext.DlrCD.Trim.Equals(dealerCode) Then
            '    'R/Oプレビュー画面へ遷移.
            '    Logger.Info("HiddenButtonHistory_Click.E NextScreen:" + REPAIR_ORDERE_PREVIEW_PAGE)
            '    Me.RedirectNextScreen(REPAIR_ORDERE_PREVIEW_PAGE)
            'Else
            '    If dealerCode.Length > 0 Then
            '        Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION ORDERDELERCODE:" + dealerCode)
            '    Else
            '        Logger.Error("HiddenButtonRepairOrderIcon_Click SESSION ORDERDELERCODE:" + dealerCode)
            '    End If
            '    MyBase.SetValue(ScreenPos.Next, "ORDERDELERCODE", dealerCode)

            '    '他店R/Oプレビュー画面へ遷移.
            '    Logger.Info("HiddenButtonHistory_Click.E NextScreen:" + REPAIR_ORDERE_PREVIEW_PAGE_FOR_OTHER_DLR)
            '    Me.RedirectNextScreen(REPAIR_ORDERE_PREVIEW_PAGE_FOR_OTHER_DLR)
            'End If
            ''2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

            '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START
            '他システムとの画面連携
            'ScreenLinkage(DISPLAY_NUMBER_13, _
            '              orderNumber, _
            '              serviceInNumber, _
            '              ServiceInDealerCode)
            ScreenLinkage(DISPLAY_NUMBER_25, _
                          orderNumber, _
                          serviceInNumber, _
                          ServiceInDealerCode)
            '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END

            '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        End If
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
          , "{0}.{1}  END. " _
          , Me.GetType.ToString _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


#End Region

#Region "TCステータスモニター処理"
    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 START

    ''' <summary>
    ''' HiddenButtonRedirectSC3150201ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRedirectSC3150201_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRedirectSC3150201.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
        'チーフテクニシャンでログインしている場合
        If OPERATION_CODE_CHT = objStaffContext.OpeCD Then
            Me.SetValue(ScreenPos.Next, "SessionKey.StallId", stallId)
        End If
        '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        'TCステータスモニターへ遷移
        Me.RedirectNextScreen(Status_Monitor_PAGE_ID)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 隠しフィールドのTCステータスモニター起動までの待機時間格納メソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HiddenStatusStandTimeValue()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '変数宣言
        Dim inpStatusStandTime As Integer
        'リフレッシュタイム格納
        inpStatusStandTime = SetTcStatusStandTime()
        '隠しフィールドに格納
        Me.HiddenTcStatusStandTime.Value = inpStatusStandTime.ToString(CultureInfo.CurrentCulture())

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' TCステータスモニター起動までの待機時間取得
    ''' </summary>
    ''' <returns>待機時間</returns>
    ''' <remarks></remarks>
    Private Function SetTcStatusStandTime() As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '変数宣言、初期値は180
        Dim tcStatusStandTime As Integer = Default_StatusStand_Time

        'リフレッシュタイムのデータセットを取得する
        Dim refreshTimeDataTable As SC3150101DataSet.SC3150101TcStatusStandTimeDataTable
        refreshTimeDataTable = businessLogic.GetTcStatusStandTime()

        'DBのカラム数が０ではない場合
        If Not refreshTimeDataTable.Count = &H0 Then
            'DBNULLではない場合
            If Not IsDBNull(refreshTimeDataTable(0)("TCSTATUS_STANDBY_TIME")) Then
                'データセットの内容をキャストして変数に格納
                tcStatusStandTime = CType(refreshTimeDataTable(0)("TCSTATUS_STANDBY_TIME"), Integer)
            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return tcStatusStandTime


    End Function
    '2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 END
#End Region

    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 START

#Region "エラー処理"

    ''' <summary>
    ''' エラーIDより、エラーメッセージを出して、画面リフレッシュ
    ''' </summary>
    ''' <param name="errId">エラーID</param>
    ''' <param name="workFlg">開始・日跨ぎ・終了の判断フラグ</param>
    ''' <param name="rezId">予約ID(STALL_USE_ID)</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
    ''' </history>
    Private Sub showErrMsgAndRefresh(ByVal errId As Long, ByVal workFlg As Integer, Optional ByVal rezId As String = "")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START,param1:{2},param2:{3},param3:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , errId _
                    , workFlg _
                    , rezId))

        'エラーIDにより、エラー文言を取得
        Dim strMsg As String = WebWordUtility.GetWord(PGMID_TEC, errId)

        '取得できてなかった場合、各操作のディフォルトメッセージを出す
        If String.IsNullOrEmpty(strMsg) Then
            If workFlg = workStartFlg Then
                'Cannot start
                errId = 906
            ElseIf workFlg = workFinishFlg Then
                'Cannot finish.
                errId = 931
            Else
                'Cannot handling in the day
                errId = 907
            End If
            strMsg = WebWordUtility.GetWord(PGMID_TEC, errId)
        End If

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

        If errId = 943 AndAlso Not String.IsNullOrEmpty(rezId) Then
            Dim stallName As String = serviceCommon.GetStallNameWithRelationChip(rezId, Me.stallId)
            If Not String.IsNullOrEmpty(stallName) Then
                strMsg = String.Format(CultureInfo.CurrentCulture, _
                                       WebWordUtility.GetWord(PGMID_TEC, 944), _
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
        sbScript.Append("reloadPage();")

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
        Dim errorMessage As String = WebWordUtility.GetWord(PGMID_TEC, 942)

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

    '2014/04/21 TMEJ 張 【開発】IT9669_サービスタブレットDMS連携作業追加機能開発 END

End Class
