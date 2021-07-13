'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080225.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細 (参照)
'補足： 
'作成： 2014/02/14 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新： 2014/04/24 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2014/07/02 TMEJ 小澤 UAT不具合対応
'更新： 2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正
'更新： 2014/09/22 SKFC 佐藤 e-Mail,Line送信機能
'更新： 2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新： 2016/11/22 NSK 中ノ瀬 TR-SVT-TMT-20161003-001 顧客の名前と苗字の間にスペースを加える
'更新： 2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更
'更新： 2019/06/07 NSK 鈴木 【18PRJ02275-00_(FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】[TKM]UAT-0117 顧客詳細の入庫履歴について、最新の履歴が0000で表示される
'更新： 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.iCROP.BizLogic.SC3080201
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080225DataSet
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic.IC3800708BusinessLogic
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess.IC3800708DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports System.IO
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic
Imports System.Reflection
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic

Partial Class Pages_SC3080225
    Inherits BasePage
    Implements ICustomerDetailControl
    Implements ICallbackEventHandler


#Region "定数"

    ''' <summary>
    ''' SessionKey「基幹顧客コード」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_DMS_CST_ID As String = "SessionKey.DMS_CST_ID"
    ''' <summary>
    ''' SessionKey「VIN」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_VIN As String = "SessionKey.VIN"

    '2014/07/02 TMEJ 小澤 UAT不具合対応 START

    ''' <summary>
    ''' SessionKey「基幹顧客コード」（NTS）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTOMERID As String = "CustomerID"
    ''' <summary>
    ''' SessionKey「VIN」（NTS）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_VINNO As String = "VIN_NO"

    '2014/07/02 TMEJ 小澤 UAT不具合対応 END

    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_SA As String = "SC3140103"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_ALL_MANAGMENT As String = "SC3220201"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_PROCESS_CONTROL As String = "SC3240101"
    ''' <summary>
    ''' メインメニュー(TC)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_TC As String = "SC3150101"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_FM As String = "SC3230101"
    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_VISIT_MANAGMENT As String = "SC3100303"
    ''' <summary>
    ''' 未振当一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_ASSIGNMENT_LIST As String = "SC3100401"
    ''' <summary>
    ''' 商品訴求コンテンツ画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_GOODS_SOLICITATION_CONTENTS As String = "SC3250101"
    ''' <summary>
    ''' 他システム連携画面画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_OTHER_LINKAGE As String = "SC3010501"
    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"
    ''' <summary>
    ''' フッターイベントタップイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CLICK_EVENT As String = "LoadProcess(); return true;"
    ''' <summary>
    ''' フッターイベントタップイベント（顧客詳細）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CLICK_EVENT_CUSTOMER As String = "CustomerDetailButtonClick();"

    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100
    ''' <summary>
    ''' フッターコード：TCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TECHNICIAN_MAIN As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_FORMAN_MAIN As Integer = 300
    ''' <summary>
    ''' フッターコード：来店管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_VISIT_MANAMENT As Integer = 400
    ''' <summary>
    ''' フッターコード：R/Oボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RO As Integer = 500
    ''' <summary>
    ''' フッターコード：連絡先
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = 600
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CUSTOMER As Integer = 700
    ''' <summary>
    ''' フッターコード：商品訴求コンテンツ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CONTENTS As Integer = 800
    ''' <summary>
    ''' フッターコード：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CAMPAIGN As Integer = 900
    ''' <summary>
    ''' フッターコード：全体管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ALL_MANAGMENT As Integer = 1000
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = 1100
    ''' <summary>
    ''' フッターコード：追加作業ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADD_LIST As Integer = 1200

    ''' <summary>
    ''' SessionKey（基幹顧客ID）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_DMSID As String = "SearchKey.DMSID"
    ''' <summary>
    ''' SessionKey（顧客区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>
    ''' SessionKey（所有者フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' 入庫履歴初期表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERVICEIN_HISTORY_INIT_PAGE As Integer = 5
    ''' <summary>
    ''' 全ての入庫履歴初期表示表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERVICEIN_HISTORY_ALL_PAGE As Integer = 20
    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT As String = "SC3080225_DEFAULT_READ_COUNT"

    ''' <summary>
    ''' 顧客アップロードパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FACEPIC_UPLOADPATH As String = "FACEPIC_UPLOADPATH"
    '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
    ''' <summary>
    ''' 顧客写真アップロードURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FACEPIC_UPLOADURL As String = "FACEPIC_UPLOADURL"
    '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
    ''' <summary>
    ''' 顧客写真サイズ（L）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMERPHOTOSIZE_L As String = "_L"
    ''' <summary>
    ''' 顧客写真サイズ（M）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMERPHOTOSIZE_M As String = "_M"
    ''' <summary>
    ''' 顧客写真サイズ（S）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMERPHOTOSIZE_S As String = "_S"
    '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START
    ' ''' <summary>
    ' ''' 顧客写真拡張子
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CUSTOMERPHOTOSIZE_EXTENSION As String = ".png"
    ''' <summary>
    ''' システム設置値名（FILE_UPLOAD_EXTENSION）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_UPLOAD_EXTENSION As String = "FILE_UPLOAD_EXTENSION"
    '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_DEARLERCODE As String = "DealerCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_BRANCHCODE As String = "BranchCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_ACCOUNT As String = "LoginUserID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VISITSEQUENCE As String = "SAChipID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_RESERVEID As String = "BASREZID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_REPAIRORDER As String = "R_O"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_REPAIRORDER_SEQUENCE As String = "SEQ_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VIN As String = "VIN_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VIEWMODE As String = "ViewMode"

    ''' <summary>
    ''' セッションキー(表示番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_DISP_NUM As String = "Session.DISP_NUM"
    '2014/04/24 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START
    ' ''' <summary>
    ' ''' セッションキー(表示番号13：R/O過去プレビュー)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSION_DATA_DISP_NUM_RO_PREVIEW As Long = 13
    ''' <summary>
    ''' セッションキー(表示番号25：R/O過去プレビュー)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_RO_PREVIEW As Long = 25
    '2014/04/24 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START
    ''' <summary>
    ''' セッションキー(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_RO_LIST As Long = 14
    ''' <summary>
    ''' セッションキー(表示番号15：キャンペーン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_CAMPAIGN As Long = 15
    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_ADD_LIST As Long = 22

    ''' <summary>
    ''' セッションキー(パラメーター1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM1 As String = "Session.Param1"
    ''' <summary>
    ''' セッションキー(パラメーター2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM2 As String = "Session.Param2"
    ''' <summary>
    ''' セッションキー(パラメーター3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM3 As String = "Session.Param3"
    ''' <summary>
    ''' セッションキー(パラメーター4)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM4 As String = "Session.Param4"
    ''' <summary>
    ''' セッションキー(パラメーター5)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM5 As String = "Session.Param5"
    ''' <summary>
    ''' セッションキー(パラメーター6)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM6 As String = "Session.Param6"
    ''' <summary>
    ''' セッションキー(パラメーター7)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM7 As String = "Session.Param7"
    ''' <summary>
    ''' セッションキー(パラメーター8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM8 As String = "Session.Param8"
    ''' <summary>
    ''' セッションキー(パラメーター9)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM9 As String = "Session.Param9"
    ''' <summary>
    ''' セッションキー(パラメーター10)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM10 As String = "Session.Param10"
    ''' <summary>
    ''' セッションキー(パラメーター11)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM11 As String = "Session.Param11"
    ''' <summary>
    ''' セッションキー(パラメーター12)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM12 As String = "Session.Param12"

    ''' <summary>
    ''' セッションキデータ(編集モード(0：編集))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_VIEWMODE_EDIT As String = "0"
    ''' <summary>
    ''' セッションキデータ(編集モード(1：プレビュー))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_VIEWMODE_PREVIEW As String = "1"
    ''' <summary>
    ''' セッションキデータ(フォーマット(1：過去))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_FORMAT_HISTORY As String = "1"

    ''' <summary>
    ''' 性別（0：男性）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEX_MAN As String = "0"
    ''' <summary>
    ''' 性別（1：女性）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEX_WOMAN As String = "1"

    ''' <summary>
    ''' 個人法人フラグ（2：法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INDIVIDUALCORPORATIONTYPE_IN As String = "2"
    ''' <summary>
    ''' 個人法人フラグ（1：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INDIVIDUALCORPORATIONTYPE_CO As String = "1"

    ''' <summary>
    ''' WebService日付フォーマット（dd/MM/yyyy HH:mi:ss）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceDateFormat As String = "dd/MM/yyyy HH:mm:ss"

    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>S</summary>
        id208 = 208
        '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
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
        '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
    End Enum
    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' P/Lマーク等のHiddenField初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FLAG_DEFAULT_VALUE As String = "0"
    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ''' <summary>
    ''' 検索最大表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VEHICLE_MAX_DISPLAY_COUNT As String = "SC3080225_VEHICLE_MAX_DISPLAY_COUNT"

    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VEHICLE_DEFAULT_READ_COUNT As String = "SC3080225_VEHICLE_DEFAULT_READ_COUNT"

    ''' <summary>
    ''' 次、前の読み込みを非表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOAD_OFF As String = "0"
    ''' <summary>
    ''' 次、前の読み込みを表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOAD_ON As String = "1"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#End Region
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
#Region "列挙体"

    ''' <summary>
    ''' 列挙体 コールバック結果コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' DBタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DbTimeout = 901

        ''' <summary>
        ''' タイムアウトエラー
        ''' </summary>
        ''' <remarks>基幹側WebService呼出時</remarks>
        TimeOutError = 902

        ''' <summary>
        ''' 基幹側のエラー
        ''' </summary>
        ''' <remarks></remarks>
        DmsError = 903

        ''' <summary>
        ''' その他のエラー
        ''' </summary>
        ''' <remarks>基本的にiCROP側のエラー全般</remarks>
        OtherError = 904

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ''' <remarks></remarks>
        Failure = 905

    End Enum
#End Region
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#Region "イベント"

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

        Dim staffInfo As StaffContext = StaffContext.Current

        '最初のアクセス時にのみ行う
        If Not IsPostBack Then
            '顧客関連情報の処理
            Me.SetCustomerRelationInfo(staffInfo)

        End If

        'フッタボタンの初期化を行う.
        InitFooterButton(staffInfo)

        '2018/11/15 NSK 坂本　TR-SVT-TMT-20160921-001 3000台の車両を所持する顧客情報が詳細画面に表示されない START

        'コールバックスプリクト登録
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "callbackSC3080225",
            String.Format(CultureInfo.InvariantCulture,
                          "callbackSC3080225.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "callbackSC3080225.packedArgument", _
                                                                      "callbackSC3080225.endCallback", "", True)
                          ), True)

        '2018/11/15 NSK 坂本　TR-SVT-TMT-20160921-001 3000台の車両を所持する顧客情報が詳細画面に表示されない END



        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' メインページロード処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub MainPageReloadButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPageReloadButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        Me.InitMainPage(staffInfo)

        Me.MainPageArea.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 保有車両選択処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub ServiceInResetButton_Click(sender As Object, e As System.EventArgs) Handles ServiceInResetButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Try
            '入庫履歴再描画処理
            Me.SetInitServiceInHistoryInfo(staffInfo.DlrCD, _
                                           Me.HiddenFieldServiceInVin.Value, _
                                           Me.HiddenFieldServiceInRegisterNumber.Value)

            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            'SSCアイコン設定
            Me.SetInitSscIconInfo(staffInfo.DlrCD, _
                                  Me.HiddenFieldServiceInVin.Value, _
                                  Me.HiddenFieldServiceInRegisterNumber.Value)
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'M/B/E/T/P/Lアイコン設定
            Me.SetVehicleIcon(staffInfo.DlrCD, _
                              Me.HiddenFieldServiceInVin.Value, _
                              Me.HiddenFieldServiceInRegisterNumber.Value)
            '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウト処理
            'DBタイムアウトのメッセージ表示
            Me.ShowMessageBox(901)
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)


        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 全ての入庫履歴表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub AllDispLinkButton_Click(sender As Object, e As System.EventArgs) Handles AllDispLinkButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080225BusinessLogic
            Try
                '全ての入庫履歴情報を取得する
                Dim dtAllContactHistoryInfo As SC3080225ContactHistoryInfoDataTable = _
                    biz.GetServiceInHistoryInfo(String.Empty, _
                                                Me.HiddenFieldServiceInVin.Value, _
                                                Me.HiddenFieldServiceInRegisterNumber.Value, _
                                                True)

                '入庫履歴表示処理
                Me.SetServiceInHistoryArea(staffInfo.DlrCD, _
                                           Nothing, _
                                           dtAllContactHistoryInfo, _
                                           True)

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(901)
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 入庫履歴一覧タップ処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory>
    ''' 2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正
    ''' </hitory>
    Protected Sub ServiceInEventButton_Click(sender As Object, e As System.EventArgs) Handles ServiceInEventButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080225BusinessLogic
            Try
                'RO情報を取得する
                Dim dtOrderPreviewInfo As SC3080225OrderPreviewInfoDataTable = _
                    biz.GetOrderPreviewInfo(Me.HiddenFieldDealerCode.Value, _
                                            Me.HiddenFieldOrderNumber.Value)

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START
                ''取得情報チェック
                'If Not (IsNothing(dtOrderPreviewInfo)) AndAlso 0 < dtOrderPreviewInfo.Count Then
                '    '取得できた場合
                '    'ログインユーザーの基幹販売店コードを取得
                '    Dim dtDmsCodeMap As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo.DlrCD, _
                '                                                                   staffInfo.BrnCD, _
                '                                                                   staffInfo.Account)

                '    '選択レコードの基幹販売店コードを取得
                '    Dim dtSelectDmsCodeMap As DmsCodeMapDataTable = _
                '        biz.GetDmsDealerData(Me.HiddenFieldDealerCode.Value, _
                '                             String.Empty, _
                '                             String.Empty)

                '    '取得情報チェック
                '    If Not (IsNothing(dtDmsCodeMap)) AndAlso 0 < dtDmsCodeMap.Count AndAlso _
                '       Not (IsNothing(dtSelectDmsCodeMap)) AndAlso 0 < dtSelectDmsCodeMap.Count Then
                '        '取得できた場合
                '        '画面間パラメータを設定
                '        '表示番号
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_RO_PREVIEW)

                '        'DMS販売店コード
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMap(0).CODE1)

                '        'DMS店舗コード
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMap(0).CODE2)

                '        'アカウント
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMap(0).ACCOUNT)

                '        '来店実績連番
                '        '来店実績連番チェック
                '        If Not (dtOrderPreviewInfo(0).IsVISITSEQNull) AndAlso _
                '           0 < dtOrderPreviewInfo(0).VISITSEQ Then
                '            'データが存在する場合
                '            '値を設定
                '            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, dtOrderPreviewInfo(0).VISITSEQ)

                '        Else
                '            'データが存在しない場合
                '            '空文字を設定
                '            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                '        End If

                '        'DMS予約ID
                '        'DMS予約IDチェック
                '        If Not (dtOrderPreviewInfo(0).IsDMS_JOB_DTL_IDNull) AndAlso _
                '           Not (String.IsNullOrEmpty(dtOrderPreviewInfo(0).DMS_JOB_DTL_ID)) Then
                '            'データが存在する場合
                '            '値を設定
                '            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, dtOrderPreviewInfo(0).DMS_JOB_DTL_ID)

                '        Else
                '            'データが存在しない場合
                '            '空文字を設定
                '            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                '        End If

                '        'RO番号
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, dtOrderPreviewInfo(0).RO_NUM)

                '        'RO作業連番
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, "0")

                '        'VIN
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, Me.HiddenFieldServiceInVin.Value)

                '        '編集モード
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_PREVIEW)

                '        '編集モード
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM10, SESSION_DATA_FORMAT_HISTORY)

                '        '入庫管理番号
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM11, Me.HiddenFieldServiceInNumber.Value)

                '        '基幹販売店コード
                '        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM12, dtSelectDmsCodeMap(0).CODE1)

                '        '追加作業画面(枠)に遷移する
                '        Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

                '    Else
                '        '取得できなかった場合
                '        '予期せぬエラー
                '        Me.ShowMessageBox(905)
                '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                                 , "{0}.{1} ERROR:SC3080225BusinessLogic.GetDmsDealerData is Nothing " _
                '                                 , Me.GetType.ToString _
                '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)

                '    End If

                'Else
                '    '取得できなかった場合
                '    '予期せぬエラー
                '    Me.ShowMessageBox(905)
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                             , "{0}.{1} ERROR:SC3080225BusinessLogic.GetOrderPreviewInfo is Nothing" _
                '                             , Me.GetType.ToString _
                '                             , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)

                'End If

                '取得できた場合
                'ログインユーザーの基幹販売店コードを取得
                Dim dtDmsCodeMap As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo.DlrCD, _
                                                                               staffInfo.BrnCD, _
                                                                               staffInfo.Account)

                '選択レコードの基幹販売店コードを取得
                Dim dtSelectDmsCodeMap As DmsCodeMapDataTable = _
                    biz.GetDmsDealerData(Me.HiddenFieldDealerCode.Value, _
                                         String.Empty, _
                                         String.Empty)

                '取得情報チェック
                If Not (IsNothing(dtDmsCodeMap)) AndAlso 0 < dtDmsCodeMap.Count AndAlso _
                   Not (IsNothing(dtSelectDmsCodeMap)) AndAlso 0 < dtSelectDmsCodeMap.Count Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_RO_PREVIEW)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMap(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMap(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMap(0).ACCOUNT)

                    '来店情報チェック
                    If Not (IsNothing(dtOrderPreviewInfo)) AndAlso _
                       0 < dtOrderPreviewInfo.Count Then
                        'データが存在する場合
                        '来店実績連番
                        '来店実績連番チェック
                        If Not (dtOrderPreviewInfo(0).IsVISITSEQNull) AndAlso _
                           0 < dtOrderPreviewInfo(0).VISITSEQ Then
                            'データが存在する場合
                            '値を設定
                            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, dtOrderPreviewInfo(0).VISITSEQ)

                        Else
                            'データが存在しない場合
                            '空文字を設定
                            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                        End If

                        'DMS予約ID
                        'DMS予約IDチェック
                        If Not (dtOrderPreviewInfo(0).IsDMS_JOB_DTL_IDNull) AndAlso _
                           Not (String.IsNullOrEmpty(dtOrderPreviewInfo(0).DMS_JOB_DTL_ID)) Then
                            'データが存在する場合
                            '値を設定
                            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, dtOrderPreviewInfo(0).DMS_JOB_DTL_ID)

                        Else
                            'データが存在しない場合
                            '空文字を設定
                            Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                        End If
                    Else
                        'データが存在しない場合
                        '空文字を設定
                        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)
                        Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    End If


                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, Me.HiddenFieldOrderNumber.Value)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, "0")

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, Me.HiddenFieldServiceInVin.Value)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_PREVIEW)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM10, SESSION_DATA_FORMAT_HISTORY)

                    '入庫管理番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM11, Me.HiddenFieldServiceInNumber.Value)

                    '基幹販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM12, dtSelectDmsCodeMap(0).CODE1)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

                Else
                    '取得できなかった場合
                    '予期せぬエラー
                    Me.ShowMessageBox(905)
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR:SC3080225BusinessLogic.GetDmsDealerData is Nothing " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)

                    '入庫履歴一覧を初期表示状態に戻す
                    Me.SetInitServiceInHistoryInfo(staffInfo.DlrCD, _
                                                   Me.HiddenFieldServiceInVin.Value, _
                                                   Me.HiddenFieldServiceInRegisterNumber.Value)

                End If

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(901)
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "LoadProcessHide();", True)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客写真登録処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history></history>
    Protected Sub CustomerPhotoRegistButton_Click(sender As Object, e As System.EventArgs) Handles CustomerPhotoRegistButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'エラーコード
        Dim returnCode As Long

        '現在日時取得
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        Using biz As New SC3080225BusinessLogic
            Try
                '顧客情報取得
                Dim dtCustomerInfo As SC3080225CustomerInfoDataTable = _
                    biz.GetCustomerInfo(staffInfo.DlrCD, _
                                        Me.HiddenFieldIcropDmsCustomerCode.Value)

                '取得情報チェック
                If Not (IsNothing(dtCustomerInfo)) AndAlso 0 < dtCustomerInfo.Count Then

                    '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START
                    '    '存在する場合
                    '    '登録ファイル名生成(L)
                    '    Dim customerPhotoPathLarge As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                    '                                                         CUSTOMERPHOTOSIZE_L, _
                    '                                                         CUSTOMERPHOTOSIZE_EXTENSION)

                    '    '登録ファイル名生成(M)
                    '    Dim customerPhotoPathMiddle As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                    '                                                          CUSTOMERPHOTOSIZE_M, _
                    '                                                          CUSTOMERPHOTOSIZE_EXTENSION)

                    '    '登録ファイル名生成(S)
                    '    Dim customerPhotoPathSmall As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                    '                                                         CUSTOMERPHOTOSIZE_S, _
                    '                                                         CUSTOMERPHOTOSIZE_EXTENSION)
                    '存在する場合
                    '登録ファイル名生成(L)
                    Dim customerPhotoPathLargeFileName As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                                                                         CUSTOMERPHOTOSIZE_L)
                    Dim customerPhotoPathLarge As String = Me.HiddenFieldExtension.Value.Replace("{0}", customerPhotoPathLargeFileName)

                    '登録ファイル名生成(M)
                    Dim customerPhotoPathMiddleFileName As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                                                                          CUSTOMERPHOTOSIZE_M)
                    Dim customerPhotoPathMiddle As String = Me.HiddenFieldExtension.Value.Replace("{0}", customerPhotoPathMiddleFileName)

                    '登録ファイル名生成(S)
                    Dim customerPhotoPathSmallFileName As String = String.Concat(dtCustomerInfo(0).CST_ID, _
                                                                         CUSTOMERPHOTOSIZE_S)
                    Dim customerPhotoPathSmall As String = Me.HiddenFieldExtension.Value.Replace("{0}", customerPhotoPathSmallFileName)
                    '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END

                    '登録処理実行
                    returnCode = biz.RegisterCustomerPhotoInfo(staffInfo.DlrCD, _
                                                               dtCustomerInfo(0).CST_ID, _
                                                               dtCustomerInfo(0).ROW_LOCK_VERSION, _
                                                               customerPhotoPathLarge, _
                                                               customerPhotoPathMiddle, _
                                                               customerPhotoPathSmall, _
                                                               nowDate, _
                                                               staffInfo.Account)

                    '処理結果チェック
                    If returnCode <> 0 Then
                        '失敗の場合
                        'メッセージ表示
                        Me.ShowMessageBox(905)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} ERROR:SC3080225BusinessLogic.RegisterCustomerPhotoInfo {2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , returnCode).ToString(CultureInfo.CurrentCulture))

                    Else
                        '成功の場合
                        'イメージを設定
                        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                        'Me.CustomerPhotoIcon.Src = String.Concat(Me.HiddenFieldFileUpLoadPath.Value, _
                        '                                         customerPhotoPathSmall)
                        Me.CustomerPhotoIcon.Src = String.Concat(Me.HiddenFieldFileUpLoadUrl.Value, customerPhotoPathSmall, _
                                                     "?", Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
                        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END

                        '顧客写真エリア更新
                        Me.CuostomerPhotoArea.Update()

                    End If

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(901)
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
    ''' <history></history>
    Public Overrides Function DeclareCommonMasterFooter( _
        ByVal commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, _
        ByRef category As Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory) As Integer()

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current


        '権限チェック
        If staffInfo.OpeCD = Operation.SA OrElse staffInfo.OpeCD = Operation.SM Then
            'SA権限、またはSM権限の場合
            category = FooterMenuCategory.CustomerDetail

        Else
            '上記以外の場合
            category = FooterMenuCategory.MainMenu

        End If

        '表示非表示に関わらず、使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <param name="inStaffInfo">ユーザー情報</param>
    ''' <remarks></remarks>
    ''' <history></history>
    Private Sub InitFooterButton(ByVal inStaffInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ヘッダ表示設定

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        mainMenuButton.OnClientClick = FOOTER_CLICK_EVENT

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SA OrElse inStaffInfo.OpeCD = Operation.SM Then
            'SA、またはSM権限の場合

            '顧客詳細ボタンの設定
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CUSTOMER)
            customerButton.OnClientClick = "return false;"

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = FOOTER_CLICK_EVENT

            '商品訴求コンテンツボタンの設定
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CONTENTS)
            AddHandler goodsSolicitationContentsButton.Click, AddressOf GoodsSolicitationContentsButton_Click
            goodsSolicitationContentsButton.OnClientClick = FOOTER_CLICK_EVENT

            'キャンペーンボタンの設定
            Dim campaignButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CAMPAIGN)
            AddHandler campaignButton.Click, AddressOf CampaignButton_Click
            campaignButton.OnClientClick = FOOTER_CLICK_EVENT

            '来店管理ボタンの設定
            Dim visitManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_VISIT_MANAMENT)
            AddHandler visitManagmentButton.Click, AddressOf VisitManagmentButton_Click
            visitManagmentButton.OnClientClick = FOOTER_CLICK_EVENT

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = FOOTER_CLICK_EVENT


        ElseIf inStaffInfo.OpeCD = Operation.CT Then
            'CT権限の場合
            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = FOOTER_CLICK_EVENT

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = FOOTER_CLICK_EVENT

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChT権限の場合
            'TCメインボタンの設定
            Dim technicianMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TECHNICIAN_MAIN)
            technicianMainButton.OnClientClick = "return false;"

            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            technicianMainButton.Enabled = False
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

            'FMメインボタンの設定
            Dim FormanMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_FORMAN_MAIN)
            AddHandler FormanMainButton.Click, AddressOf FormanMainButton_Click
            FormanMainButton.OnClientClick = FOOTER_CLICK_EVENT

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = FOOTER_CLICK_EVENT

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = FOOTER_CLICK_EVENT

        ElseIf inStaffInfo.OpeCD = Operation.FM Then
            'FM権限の場合
            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = FOOTER_CLICK_EVENT

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = FOOTER_CLICK_EVENT

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = FOOTER_CLICK_EVENT

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
        telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL

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
    ''' <history></history>
    Private Sub MainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} START" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.SA Then
            'メインメニュー(SA)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_SA)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_ALL_MANAGMENT)

        ElseIf staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_FM)

        ElseIf staffInfo.OpeCD = Operation.SVR Then
            '未振当一覧に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_ASSIGNMENT_LIST)

        End If

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
    ''' 来店管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub VisitManagmentButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '来店管理画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_VISIT_MANAGMENT)

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
    ''' <hitory></hitory>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080225BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                    biz.GetDmsDealerData(staffInfo.DlrCD, _
                                         staffInfo.BrnCD, _
                                         staffInfo.Account)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_RO_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

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
                Me.ShowMessageBox(901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub GoodsSolicitationContentsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画面間パラメータを設定
        'DMS販売店コード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_DEARLERCODE, Space(1))

        'DMS店舗コード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_BRANCHCODE, Space(1))

        'アカウント
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_ACCOUNT, Space(1))

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VISITSEQUENCE, String.Empty)

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_RESERVEID, String.Empty)

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_REPAIRORDER, String.Empty)

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_REPAIRORDER_SEQUENCE, String.Empty)

        'VIN
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VIN, String.Empty)

        '編集モード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VIEWMODE, SESSION_DATA_VIEWMODE_PREVIEW)

        '商品訴求コンテンツ画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_GOODS_SOLICITATION_CONTENTS)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' キャンペーンボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub CampaignButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080225BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                    biz.GetDmsDealerData(staffInfo.DlrCD, _
                                         staffInfo.BrnCD, _
                                         staffInfo.Account)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_CAMPAIGN)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
                    'Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_PREVIEW)
                    '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

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
                Me.ShowMessageBox(901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 全体管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AllManagmentButtonButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '全体管理（枠）画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_ALL_MANAGMENT)

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
        Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

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
    ''' <hitory></hitory>
    Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3080225BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                    biz.GetDmsDealerData(staffInfo.DlrCD, _
                                         staffInfo.BrnCD, _
                                         staffInfo.Account)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_ADD_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

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
                Me.ShowMessageBox(901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

#Region "コールバック"

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callbackResult As String

    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass
        Public Property Method As String
        Public Property Start As String
        Public Property Count As String
    End Class

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass
        Public Property ResultCode As Integer
        Public Property Message As String
        Public Property Contents As String
        Public Property WordDict As Dictionary(Of String, String)
        Public Property BeforeFlg As String
        Public Property NextFlg As String
        Public Property NowStart As String
        Public Property NowCount As String
    End Class

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return callbackResult

    End Function

    ''' <summary>
    ''' コールバックメソッド名(保有車両リスト表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodVehicleListDisp As String = "VehicleListDisp"

    Private Const MethodVehicleListDispBefore As String = "VehicleListDispBefore"

    Private Const MethodVehicleListDispAfter As String = "VehicleListDispAfter"
    ''' <summary>
    ''' コールバックイベントハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントで生成したパラメータクラスをJSON形式に変換した文字列</param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim serializer = New JavaScriptSerializer

        'コールバック引数用内部クラスのインスタンスを生成し、JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New CallBackArgumentClass

        'コールバック返却用内部クラスのインスタンスを生成
        Dim callResult As New CallBackResultClass

        Using biz As New SC3080225BusinessLogic
            Try

                'clientからのパラメータを取得
                argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

                Dim staffInfo As StaffContext = StaffContext.Current
                Dim start As Integer
                Dim nowStart As Integer
                Dim count As Integer
                Dim nowCount As Integer
                Dim dtCustomerVcl As New CustomerDetailClass


                Select Case argument.Method

                    '保有車両アイコン押下
                    Case MethodVehicleListDisp

                        start = 0
                        count = CInt(Me.HiddenFieldDefaultReadCount.Value)

                        '前のN件を押下
                    Case MethodVehicleListDispBefore

                        nowStart = CInt(argument.Start)
                        start = nowStart - CInt(Me.HiddenFieldDefaultReadCount.Value)
                        count = CInt(Me.HiddenFieldMaxDisplayCount.Value)

                        '次のN件を押下
                    Case MethodVehicleListDispAfter

                        nowStart = CInt(argument.Start)
                        nowCount = CInt(argument.Count)

                        If (nowCount + CInt(Me.HiddenFieldDefaultReadCount.Value)) <= CInt(Me.HiddenFieldMaxDisplayCount.Value) Then

                            start = nowStart
                            count = nowCount + CInt(Me.HiddenFieldDefaultReadCount.Value)
                        Else
                            start = nowStart + CInt(Me.HiddenFieldDefaultReadCount.Value)
                            count = CInt(Me.HiddenFieldMaxDisplayCount.Value)

                        End If

                End Select

                '保有車両情報取得
                dtCustomerVcl = biz.GetHoldingCustomerVehicleInfo(staffInfo.DlrCD, _
                                                                  staffInfo.BrnCD, _
                                                                  Me.HiddenFieldDmsCustomerCode.Value, _
                                                                  Me.HiddenFieldVin.Value, _
                                                                  start, _
                                                                  count)


                '取得結果のチェック
                If dtCustomerVcl.ResultCode <> Result.Success Then
                    '失敗した場合
                    'エラーコードチェック
                    If dtCustomerVcl.ResultCode = Result.TimeOutError Then
                        '通信タイムアウトエラー(6001)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , dtCustomerVcl.ResultCode))

                        'エラーメッセージ表示
                        callResult.ResultCode = ResultCode.TimeOutError
                        callResult.Message = WebWordUtility.GetWord(ResultCode.TimeOutError)

                        Me.callbackResult = serializer.Serialize(callResult)

                    ElseIf dtCustomerVcl.ResultCode = Result.DmsError Then
                        'DMS関連エラー(6002)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , dtCustomerVcl.ResultCode))

                        'エラーメッセージ表示
                        callResult.ResultCode = ResultCode.DmsError
                        callResult.Message = WebWordUtility.GetWord(ResultCode.DmsError)

                        Me.callbackResult = serializer.Serialize(callResult)

                    ElseIf dtCustomerVcl.ResultCode = Result.OtherError Then
                        'ICROP関連エラー(6003)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , dtCustomerVcl.ResultCode))

                        'エラーメッセージ表示
                        callResult.ResultCode = ResultCode.OtherError
                        callResult.Message = WebWordUtility.GetWord(ResultCode.OtherError)

                        Me.callbackResult = serializer.Serialize(callResult)
                    Else
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , dtCustomerVcl.ResultCode))

                        'エラーメッセージ表示
                        '上記以外
                        callResult.ResultCode = ResultCode.Failure
                        callResult.Message = WebWordUtility.GetWord(ResultCode.Failure)

                        Me.callbackResult = serializer.Serialize(callResult)

                    End If
                Else
                    'Hiddenに設定する
                    callResult.NowStart = start.ToString
                    callResult.NowCount = count.ToString

                    '車両マージ
                    Dim dtVehicleInfo As SC3080225VehicleInfoDataTable = _
                                        biz.MergeVehicleInfo(dtCustomerVcl.VhcInfo, staffInfo.DlrCD
                                                             )

                    Dim drVehicleInfo As SC3080225VehicleInfoRow() = dtVehicleInfo.ToArray

                    For Each drSortVehicleInfo As SC3080225VehicleInfoRow In drVehicleInfo
                        'モデルロゴのパスが「~/」(アプリケーションルート)を含む仮想パスの場合、ブラウザで使用できるURLに変換する
                        drSortVehicleInfo.ModelLogoOffURL = ResolveClientUrl(drSortVehicleInfo.ModelLogoOffURL)
                        drSortVehicleInfo.ModelLogoOnURL = ResolveClientUrl(drSortVehicleInfo.ModelLogoOnURL)
                    Next


                    'JSONデータ作成して格納
                    callResult.Contents = Me.CreateJsonData(drVehicleInfo)
                    callResult.WordDict = Me.GetWordVehicleListItem()

                    callResult.ResultCode = CInt(dtCustomerVcl.ResultCode)
                    callResult.Message = String.Empty

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                            , "{0}.{1} " _
                                                            , Me.GetType.ToString _
                                                            , System.Reflection.MethodBase.GetCurrentMethod.Name))


                    '前、次の表示フラグ初期化
                    callResult.NextFlg = LOAD_OFF
                    callResult.BeforeFlg = LOAD_OFF

                    If Not (String.IsNullOrWhiteSpace(dtCustomerVcl.AllCount)) Then
                        '次の件数表示
                        If CInt(dtCustomerVcl.VhcInfo.Count) <> count Then
                            '次のN件非表示
                            callResult.NextFlg = LOAD_OFF
                        ElseIf (start + CInt(dtCustomerVcl.VhcInfo.Count)) < CInt(dtCustomerVcl.AllCount) Then
                            '次のN件表示
                            callResult.NextFlg = LOAD_ON
                        End If

                        '前の件数表示
                        If 0 < start Then
                            callResult.BeforeFlg = LOAD_ON
                        Else
                            callResult.BeforeFlg = LOAD_OFF
                        End If
                    End If

                    Me.callbackResult = serializer.Serialize(callResult)
                End If
            Catch ex As OracleExceptionEx When ex.Number = 1013

                callResult.ResultCode = ResultCode.DbTimeout
                callResult.Message = WebWordUtility.GetWord(ResultCode.DbTimeout)

                Me.callbackResult = serializer.Serialize(callResult)

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} ERROR: " _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name) _
                                             , ex)

            Catch ex As Exception

                '予期せぬエラー
                callResult.ResultCode = ResultCode.Failure
                callResult.Message = WebWordUtility.GetWord(ResultCode.Failure)

                Me.callbackResult = serializer.Serialize(callResult)

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} ERROR: " _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name) _
                                             , ex)
            End Try
        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END


#Region "Privateメソッド"

    ''' <summary>
    ''' 初期表示処理
    ''' </summary>
    ''' <param name="inStaffInfo">ユーザー情報</param>
    ''' <remarks></remarks>
    ''' <history></history>
    Private Sub InitMainPage(ByVal inStaffInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '顧客車両情報設定
        Me.SetCustomerVehicleInfo(inStaffInfo, _
                                  Me.HiddenFieldDmsCustomerCode.Value, _
                                  Me.HiddenFieldIcropDmsCustomerCode.Value, _
                                  Me.HiddenFieldVin.Value)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客関連情報設定処理
    ''' </summary>
    ''' <param name="inStaffInfo">ユーザー情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/07/02 TMEJ 小澤 UAT不具合対応
    ''' </history>
    Private Sub SetCustomerRelationInfo(ByVal inStaffInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2014/07/02 TMEJ 小澤 UAT不具合対応 START

        ''基幹顧客IDをSessionから取得
        'Dim sessionDmsCustomerCode As String = String.Empty

        ''基幹顧客IDのSessionチェック
        'If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_DMS_CST_ID) Then
        '    '存在する場合
        '    '基幹顧客IDをSessionから取得
        '    sessionDmsCustomerCode = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_DMS_CST_ID, False), String)

        '    '基幹顧客IDのデータチェック
        '    If 0 < sessionDmsCustomerCode.IndexOf("@") Then
        '        '「@」が存在する場合
        '        '「@」より前のデータを削除する
        '        sessionDmsCustomerCode = sessionDmsCustomerCode.Split(CChar("@"))(1)

        '    End If

        '    'Hiddenに設定
        '    Me.HiddenFieldDmsCustomerCode.Value = sessionDmsCustomerCode

        'End If

        ''VINをSessionから取得
        'Dim sessionVin As String = String.Empty

        ''VINのSessionチェック
        'If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VIN) Then
        '    '存在する場合
        '    'VINをSessionから取得
        '    sessionVin = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VIN, False), String)

        '    'Hiddenに設定
        '    Me.HiddenFieldVin.Value = sessionVin

        'End If

        '基幹顧客IDをSessionから取得
        Dim sessionDmsCustomerCode As String = String.Empty

        'VINをSessionから取得
        Dim sessionVin As String = String.Empty

        '基幹顧客IDのSessionチェック
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_DMS_CST_ID) Then
            '存在する場合（i-CROPから遷移）
            '基幹顧客IDをSessionから取得
            sessionDmsCustomerCode = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_DMS_CST_ID, False), String)

            '基幹顧客IDのデータチェック
            If 0 < sessionDmsCustomerCode.IndexOf("@") Then
                '「@」が存在する場合
                '「@」より前のデータを削除する
                sessionDmsCustomerCode = sessionDmsCustomerCode.Split(CChar("@"))(1)

            End If

            'Hiddenに設定
            Me.HiddenFieldDmsCustomerCode.Value = sessionDmsCustomerCode

            'VINのSessionチェック
            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VIN) Then
                '存在する場合（i-CROPから遷移）
                'VINをSessionから取得
                sessionVin = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VIN, False), String)

                'Hiddenに設定
                Me.HiddenFieldVin.Value = sessionVin

            End If

        ElseIf Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CUSTOMERID) Then
            '存在する場合（NTSから遷移）
            '基幹顧客IDをSessionから取得
            sessionDmsCustomerCode = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERID, False), String)

            '基幹顧客IDのデータチェック
            If 0 < sessionDmsCustomerCode.IndexOf("@") Then
                '「@」が存在する場合
                '「@」より前のデータを削除する
                sessionDmsCustomerCode = sessionDmsCustomerCode.Split(CChar("@"))(1)

            End If

            'Hiddenに設定
            Me.HiddenFieldDmsCustomerCode.Value = sessionDmsCustomerCode

            'VINのSessionチェック
            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VINNO) Then
                '存在する場合（NTSから遷移）
                'VINをSessionから取得
                sessionVin = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VINNO, False), String)

                'Hiddenに設定
                Me.HiddenFieldVin.Value = sessionVin

            End If

        End If

        '2014/07/02 TMEJ 小澤 UAT不具合対応 END

        '表示件数をシステム設定から取得する
        Dim daSystemEnv As New SystemEnvSetting
        Dim drDisplayCount As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            daSystemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT)

        '取得情報チェック
        If Not (IsNothing(drDisplayCount)) Then
            '存在する場合
            'Hiddenに設定
            Me.HiddenFieldDisplayCount.Value = drDisplayCount.PARAMVALUE

        Else
            '存在しない場合
            'デフォルト値を設定
            Me.HiddenFieldDisplayCount.Value = CType(SERVICEIN_HISTORY_ALL_PAGE, String)

        End If

        '読み込み中の文言設定
        Me.NextDispLinkDivLabel.Text = _
            WebWordUtility.GetWord(17).Replace("{0}", Me.HiddenFieldDisplayCount.Value)

        '読み込み中の文言設定
        Me.NextLoadingDivLabel.Text = _
            WebWordUtility.GetWord(189).Replace("{0}", Me.HiddenFieldDisplayCount.Value)

        '顧客写真ファイルパス情報取得
        Dim drFilePath As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            daSystemEnv.GetSystemEnvSetting(FACEPIC_UPLOADPATH)

        '取得情報チェック
        If Not (IsNothing(drFilePath)) Then
            '存在する場合
            '値を設定
            Me.HiddenFieldFileUpLoadPath.Value = drFilePath.PARAMVALUE

        End If

        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
        '顧客写真ファイルURL情報取得
        Dim drFileUrl As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            daSystemEnv.GetSystemEnvSetting(FACEPIC_UPLOADURL)

        '取得情報チェック
        If Not (IsNothing(drFileUrl)) Then
            '存在する場合
            '値を設定
            Me.HiddenFieldFileUpLoadUrl.Value = drFileUrl.PARAMVALUE

        End If
        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END

        Using biz As New SC3080225BusinessLogic
            Try
                '基幹顧客IDをi-CROP用に変換
                Dim icropDmsCustomerCode As String = biz.ReplaceDmsCustomerId(inStaffInfo.DlrCD, _
                                                                              sessionDmsCustomerCode)

                '基幹顧客IDをHiddenに設定
                Me.HiddenFieldIcropDmsCustomerCode.Value = icropDmsCustomerCode

                '顧客情報取得
                Dim dtCustomerInfo As SC3080225CustomerInfoDataTable = _
                    biz.GetCustomerInfo(inStaffInfo.DlrCD, _
                                        icropDmsCustomerCode)




                '取得情報チェック
                If Not (IsNothing(dtCustomerInfo)) AndAlso _
                   0 < dtCustomerInfo.Count Then
                    'データが存在する場合且つ、顧客種別が正常の場合
                    'Sessionにデータ格納
                    '顧客種別と顧客車両種別のチェック
                    If Not (dtCustomerInfo(0).IsCST_TYPENull) AndAlso _
                       Not (String.IsNullOrEmpty(dtCustomerInfo(0).CST_TYPE)) AndAlso _
                       Not (dtCustomerInfo(0).IsCST_VCL_TYPENull) AndAlso _
                       Not (String.IsNullOrEmpty(dtCustomerInfo(0).CST_VCL_TYPE)) Then
                        'データが存在する場合
                        'データ格納
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, dtCustomerInfo(0).CST_TYPE)
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, dtCustomerInfo(0).CST_VCL_TYPE)
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_DMSID, sessionDmsCustomerCode)

                    End If

                    '2014/09/22 SKFC 佐藤 e-Mail,Line送信機能 START
                    If Not (dtCustomerInfo(0).IsCST_IDNull) Then
                        '顧客IDをHiddenに設定
                        Me.HiddenFieldCstId.Value = dtCustomerInfo(0).CST_ID.ToString
                        '販売店コードをHiddenに設定
                        Me.HiddenFieldDealerCode.Value = inStaffInfo.DlrCD
                        '店舗コードをHiddenに設定
                        Me.HiddenFieldStoreCode.Value = inStaffInfo.BrnCD
                        'email,line送信機能起動のためのROnum取得
                        Me.HiddenFieldOrderNumber.Value = _
                            biz.GetRONumber(inStaffInfo.DlrCD, _
                                            dtCustomerInfo(0).CST_ID, _
                                            Me.HiddenFieldServiceInVin.Value, _
                                            Me.HiddenFieldServiceInRegisterNumber.Value)
                        'セッションへROnum設定
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_GOODS_CONTENTS_REPAIRORDER, Me.HiddenFieldOrderNumber.Value)


                    End If
                    '2014/09/22 SKFC 佐藤 e-Mail,Line送信機能 END


                End If

                '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
                Dim systemEnv As New SystemEnvSetting
                Dim defaultReadCount As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = systemEnv.GetSystemEnvSetting(VEHICLE_DEFAULT_READ_COUNT)
                Dim maxDipslayCount As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = systemEnv.GetSystemEnvSetting(VEHICLE_MAX_DISPLAY_COUNT)
                If CInt(maxDipslayCount.PARAMVALUE) < CInt(defaultReadCount.PARAMVALUE) Then
                    Me.HiddenFieldDefaultReadCount.Value = defaultReadCount.PARAMVALUE
                    Me.HiddenFieldMaxDisplayCount.Value = defaultReadCount.PARAMVALUE
                Else
                    Me.HiddenFieldMaxDisplayCount.Value = maxDipslayCount.PARAMVALUE
                    Me.HiddenFieldDefaultReadCount.Value = defaultReadCount.PARAMVALUE
                End If
                '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(901)

            End Try

        End Using

        '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START
        Dim smbCommonBiz As New ServiceCommonClassBusinessLogic
        'システム設定の取得(ファイルアップロード拡張子取得)
        Me.HiddenFieldExtension.Value = smbCommonBiz.GetSystemSettingValueBySettingName(FILE_UPLOAD_EXTENSION)
        '(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客情報設定取得
    ''' </summary>
    ''' <param name="inStaffInfo">ユーザー情報</param>
    ''' <param name="inDmsCustomerCode">基幹顧客ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <remarks></remarks>
    ''' <history></history>
    Private Sub SetCustomerVehicleInfo(ByVal inStaffInfo As StaffContext, _
                                       ByVal inDmsCustomerCode As String, _
                                       ByVal inIcropDmsCustomerCode As String, _
                                       ByVal inVin As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:StaffContext P2:{2} P3:{3} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDmsCustomerCode, _
                                  inVin))

        Using biz As New SC3080225BusinessLogic
            Try
                '顧客車両情報の取得
                Dim classCustomerDetail As CustomerDetailClass = _
                    biz.GetCustomerVehicleInfo(inStaffInfo.DlrCD, _
                                               inStaffInfo.BrnCD, _
                                               inDmsCustomerCode, _
                                               inVin)

                '取得結果のチェック
                If classCustomerDetail.ResultCode <> Result.Success Then
                    '失敗した場合
                    'エラーコードチェック
                    If classCustomerDetail.ResultCode = Result.TimeOutError Then
                        '通信タイムアウトエラー(6001)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , classCustomerDetail.ResultCode))

                        'エラーメッセージ表示
                        Me.ShowMessageBox(902)

                    ElseIf classCustomerDetail.ResultCode = Result.DmsError Then
                        'DMS関連エラー(6002)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , classCustomerDetail.ResultCode))

                        'エラーメッセージ表示
                        Me.ShowMessageBox(903)

                    ElseIf classCustomerDetail.ResultCode = Result.OtherError Then
                        'ICROP関連エラー(6003)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , classCustomerDetail.ResultCode))

                        'エラーメッセージ表示
                        Me.ShowMessageBox(904)
                    Else
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                 , "{0}.{1} SC3080225BusinessLogic.GetCustomerVehicleInfo ERRORCODE:{2}" _
                                                 , Me.GetType.ToString _
                                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                 , classCustomerDetail.ResultCode))

                        'エラーメッセージ表示
                        '上記以外
                        Me.ShowMessageBox(905)

                    End If

                Else
                    '成功した場合
                    '顧客情報取得
                    Dim dtCustomerInfo As SC3080225CustomerInfoDataTable = _
                        biz.GetCustomerInfo(inStaffInfo.DlrCD, _
                                            inIcropDmsCustomerCode)

                    '顧客情報設定
                    Me.SetCustomerArea(biz, _
                                       inIcropDmsCustomerCode, _
                                       classCustomerDetail, _
                                       dtCustomerInfo)

                    '顧客詳細エリア設定
                    Me.SetCustomerDetailArea(biz, classCustomerDetail)

                    '車両情報、車両詳細情報、保有車両情報、入庫履歴情報設定
                    Me.SetVehicleArea(biz, _
                                      inStaffInfo.DlrCD, _
                                      inIcropDmsCustomerCode, _
                                      inVin, _
                                      classCustomerDetail)

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客エリア設定処理
    ''' </summary>
    ''' <param name="biz">ビジネスロジック</param>
    ''' <param name="inIcropDmsCustomerCode">基幹顧客ID（ICROP）</param>
    ''' <param name="inClassCustomerDetail">顧客車両情報</param>
    ''' <param name="dtCustomerInfo">顧客情報</param>
    ''' <remarks></remarks>
    Private Sub SetCustomerArea(ByVal biz As SC3080225BusinessLogic, _
                                ByVal inIcropDmsCustomerCode As String, _
                                ByVal inClassCustomerDetail As CustomerDetailClass, _
                                ByVal dtCustomerInfo As SC3080225CustomerInfoDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inIcropDmsCustomerCode))

        '2016/11/22 NSK 中ノ瀬 TR-SVT-TMT-20161003-001 顧客の名前と苗字の間にスペースを加える START
        '名前と苗字の間にスペースを入れて連結する
        Dim Name As String = _
            String.Concat(inClassCustomerDetail.Name1)

        If Not String.IsNullOrWhiteSpace(inClassCustomerDetail.Name2) Then
            Name = String.Concat(Name, " ", inClassCustomerDetail.Name2)
        End If

        If Not String.IsNullOrWhiteSpace(inClassCustomerDetail.Name3) Then
            Name = String.Concat(Name, " ", inClassCustomerDetail.Name3)
        End If

        ''顧客名
        ''敬称あり顧客名の取得
        'Dim customerName As String = _
        '    biz.GetCustomerNameInTitleName(inIcropDmsCustomerCode, _
        '                                   String.Concat(inClassCustomerDetail.Name1, _
        '                                                 inClassCustomerDetail.Name2, _
        '                                                 inClassCustomerDetail.Name3), _
        '                                   inClassCustomerDetail.NameTitle)
        'Me.CustomerName.Text = customerName

        '顧客名
        '敬称あり顧客名の取得
        Dim customerName As String = _
            biz.GetCustomerNameInTitleName(inIcropDmsCustomerCode, _
                                           Name, _
                                           inClassCustomerDetail.NameTitle)
        Me.CustomerName.Text = customerName

        '2016/11/22 NSK 中ノ瀬 TR-SVT-TMT-20161003-001 顧客の名前と苗字の間にスペースを加える END


        '基幹顧客ID
        Me.DmsId.Text = inClassCustomerDetail.CustomerCode

        '顔写真
        'アイコンURLチェック
        If Not (IsNothing(dtCustomerInfo)) AndAlso _
           0 < dtCustomerInfo.Count AndAlso _
           Not (dtCustomerInfo(0).IsIMG_FILE_SMALLNull) AndAlso _
           Not (String.IsNullOrEmpty(dtCustomerInfo(0).IMG_FILE_SMALL)) Then
            'データが存在する場合
            'URL設定
            '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
            'Me.CustomerPhotoIcon.Src = String.Concat(Me.HiddenFieldFileUpLoadPath.Value, _
            '                                         dtCustomerInfo(0).IMG_FILE_SMALL)
            Me.CustomerPhotoIcon.Src = String.Concat(Me.HiddenFieldFileUpLoadUrl.Value, dtCustomerInfo(0).IMG_FILE_SMALL, _
                                                     "?", Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
            '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END

        End If

        '携帯電話番号
        Me.CstMobile.Text = inClassCustomerDetail.Mobile

        '電話番号
        Me.CstPhone.Text = inClassCustomerDetail.TelNumber

        'Eメールアドレス
        'Eメールアドレスチェック
        If Not (String.IsNullOrEmpty(inClassCustomerDetail.EMail1)) Then
            'Eメールアドレス1が存在する場合
            'Eメールアドレス1を設定
            Me.CstEmail.Text = inClassCustomerDetail.EMail1

        Else
            'Eメールアドレス1が存在する場合
            'Eメールアドレス2を設定
            Me.CstEmail.Text = inClassCustomerDetail.EMail2

        End If

        'VIPアイコン（表示しない）

        '自社／その他アイコン（自社客しか表示しない）
        Me.FreetIcon.Text = WebWordUtility.GetWord(4)

        '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START

        'If INDIVIDUALCORPORATIONTYPE_IN.Equals(inClassCustomerDetail.CustomerType) Then
        ''「0：法人」の場合
        'Me.CustomerTypeIcon.Text = WebWordUtility.GetWord(7)
        'ElseIf INDIVIDUALCORPORATIONTYPE_CO.Equals(inClassCustomerDetail.CustomerType) Then
        ''「1：個人」の場合
        ''個人のアイコンを設定
        'Me.CustomerTypeIcon.Text = WebWordUtility.GetWord(6)

        '個人法人アイコンの取得
        Dim joinType As String = biz.GetCustomerJoinType(inClassCustomerDetail.SubCustomerType)

        '個人／法人アイコン
        '個人法人チェック
        If INDIVIDUALCORPORATIONTYPE_IN.Equals(joinType) Then
            '「2：法人」の場合
            '法人のアイコンを設定
            Me.CustomerTypeIcon.Text = WebWordUtility.GetWord(7)
        ElseIf INDIVIDUALCORPORATIONTYPE_CO.Equals(joinType) Then
            '「1：個人」の場合
            '個人のアイコンを設定
            Me.CustomerTypeIcon.Text = WebWordUtility.GetWord(6)
            '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END

        Else
            '上記以外
            '非表示にする
            Me.CustomerTypeIcon.Attributes("style") = "display:none;"

        End If

        'アイコンエリアの表示
        Me.CustomerIconArea.Attributes("style") = "display:block;"

        '郵便番号
        Me.CstZipCode.Text = inClassCustomerDetail.ZipCode

        '住所
        Me.CstAddress.Text = String.Concat(inClassCustomerDetail.Address1, _
                                           inClassCustomerDetail.Address2, _
                                           inClassCustomerDetail.Address3)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 車両エリア設定処理
    ''' </summary>
    ''' <param name="biz">ビジネスロジック</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inIcropDmsCustomerCode">基幹顧客ID（ICROP）</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inClassCustomerDetail">車両情報</param>
    ''' <remarks></remarks>
    ''' <history></history>
    Private Sub SetVehicleArea(ByVal biz As SC3080225BusinessLogic, _
                               ByVal inDealerCode As String, _
                               ByVal inIcropDmsCustomerCode As String, _
                               ByVal inVin As String, _
                               ByVal inClassCustomerDetail As CustomerDetailClass)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , inDealerCode _
                                  , inIcropDmsCustomerCode _
                                  , inVin))

        '車両情報チェック
        If Not (IsNothing(inClassCustomerDetail.VhcInfo)) AndAlso 0 < inClassCustomerDetail.VhcInfo.Count Then

            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
            'データが存在する場合
            '設定する車両情報を取得する
            'Dim dtVehicleInfo As SC3080225VehicleInfoDataTable = _
            '    biz.MergeVehicleInfo(inDealerCode, _
            '        inVin, _
            '        inClassCustomerDetail.VhcInfo)
            Dim localVhcinfo As New IC3800708CustomerVhcInfoDataTable
            localVhcinfo.ImportRow(inClassCustomerDetail.VhcInfo(0))

            Dim dtVehicleInfo As SC3080225VehicleInfoDataTable = _
                 biz.MergeVehicleInfo(localVhcinfo, inDealerCode)

            ''ソートキーの昇順、納車日の降順ソートする
            'Dim dtSortVehicleInfo As SC3080225VehicleInfoRow() = _
            '    (From drSortCustomerVehicleInfo As SC3080225VehicleInfoRow In dtVehicleInfo _
            '     Order By drSortCustomerVehicleInfo.SortKey Ascending, _
            '              drSortCustomerVehicleInfo.VehicleDeliveryDate Descending _
            '     Select drSortCustomerVehicleInfo).ToArray

            'Dim drVehicleInfo As SC3080225VehicleInfoRow = dtSortVehicleInfo(0)
            Dim drVehicleInfo As SC3080225VehicleInfoRow = dtVehicleInfo(0)

            ''保有車両数チェック
            'If 1 < CDbl(inClassCustomerDetail.AllCount) Then
            '    '2台以上ある場合
            '    '保有車両設定
            '    Me.SetVehicleSelectArea(dtSortVehicleInfo)

            '    'アイコンを表示に設定
            '    Me.NumberOfVehicles.Attributes("style") = "display:block;"

            'Else
            '    '1台の場合
            '    'アイコンを非表示に設定
            '    Me.NumberOfVehicles.Attributes("style") = "display:none;"

            'End If

            '保有車両数チェック
            If Not (String.IsNullOrWhiteSpace(inClassCustomerDetail.AllCount)) AndAlso 1 < CInt(inClassCustomerDetail.AllCount) Then
                'アイコンを表示に設定
                Me.NumberOfVehicles.Attributes("style") = "display:block;"
                '保有車両数
                Me.NumberOfVehicles.Text = inClassCustomerDetail.AllCount
            ElseIf String.IsNullOrWhiteSpace(inClassCustomerDetail.AllCount) AndAlso 1 < inClassCustomerDetail.VhcInfo.Count Then
                'アイコンを表示に設定
                Me.NumberOfVehicles.Attributes("style") = "display:block;"
            Else
                'アイコンを非表示に設定
                Me.NumberOfVehicles.Attributes("style") = "display:none;"
            End If

            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
            'ロゴURL
            'ロゴ情報のチェック
            If Not (drVehicleInfo.IsModelLogoOffURLNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.ModelLogoOffURL)) AndAlso _
               Not (drVehicleInfo.IsModelLogoOnURLNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.ModelLogoOnURL)) Then
                'ロゴ情報がすべて存在する場合
                'ロゴを表示する
                'ロゴURL
                Me.VehicleLogoIcon.Src = drVehicleInfo.ModelLogoOffURL

                'ロゴを表示
                Me.VehicleLogoIcon.Attributes("style") = "display:block;"

                'メーカー名とモデル名を非表示
                Me.VehicleMakerModelTable.Attributes("style") = "display:none;"

            Else
                'ロゴ情報が1つでも存在しない場合
                'メーカー名とモデル名を表示する
                'メーカー名
                Me.VehicleMakerName.Text = drVehicleInfo.MakerCode
                'モデル名
                Me.VehicleModelName.Text = drVehicleInfo.SERIESCD

                'ロゴを非表示
                Me.VehicleLogoIcon.Attributes("style") = "display:none;"

                'メーカー名とモデル名を表示
                Me.VehicleMakerModelTable.Attributes("style") = "display:block;"

            End If

            'グレード
            Me.VehicleGrade.Text = drVehicleInfo.Grade

            '外装色
            Me.VehicleBodyColor.Text = drVehicleInfo.BodyColorName

            '車両登録No.
            Me.VehicleRegNo.Text = drVehicleInfo.VehicleRegistrationNumber

            '車両登録エリア名称
            Me.VehicleProvince.Text = drVehicleInfo.VehicleAreaName

            'VIN
            Me.VehicleVin.Text = drVehicleInfo.Vin

            '納車日（YYYY/MM/DD）
            Me.VehicleDeliveryDate.Text = drVehicleInfo.VehicleDeliveryDate

            '最新走行距離
            Me.LatestMileage.Text = drVehicleInfo.Mileage

            '最新走行距離更新日（MM/DD）
            Me.LatestMileageUpdateDate.Text = drVehicleInfo.LastUpdateDate

            'セールス担当者名
            Me.SalesStaffName.Text = drVehicleInfo.SalesStaffName

            'サービス担当者名
            Me.ServiceStaffName.Text = drVehicleInfo.ServiceAdviserName

            '車両詳細ポップアップにデータを入れる
            Me.SetVehicleDetailArea(drVehicleInfo)

            '初期表示入庫履歴設定
            Me.SetInitServiceInHistoryInfo(inDealerCode, _
                                           drVehicleInfo.Vin, _
                                           drVehicleInfo.VehicleRegistrationNumber)

            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            'SSCアイコン設定
            Me.SetInitSscIconInfo(inDealerCode, _
                                  drVehicleInfo.Vin, _
                                  drVehicleInfo.VehicleRegistrationNumber)
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
            '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'M/B/E/T/P/Lアイコン設定
            Me.SetVehicleIcon(inDealerCode, _
                              drVehicleInfo.Vin, _
                              drVehicleInfo.VehicleRegistrationNumber)
            '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END


            '現在表示しているVINと車両番号を設定
            Me.HiddenFieldServiceInVin.Value = drVehicleInfo.Vin
            Me.HiddenFieldServiceInRegisterNumber.Value = drVehicleInfo.VehicleRegistrationNumber

        Else
            '存在しない場合
            '車両アイコンを非表示に設定
            Me.NumberOfVehicles.Attributes("style") = "display:none;"

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客詳細エリア設定処理
    ''' </summary>
    ''' <param name="inClassCustomerDetail"></param>
    ''' <remarks></remarks>
    Private Sub SetCustomerDetailArea(ByVal biz As SC3080225BusinessLogic, _
                                      ByVal inClassCustomerDetail As CustomerDetailClass)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ファーストネーム
        Me.CstPopFirstName.Text = inClassCustomerDetail.Name1

        'ミドルネーム
        Me.CstPopMiddleName.Text = inClassCustomerDetail.Name2

        'ラストネーム
        Me.CstPopLastName.Text = inClassCustomerDetail.Name3

        '敬称
        Me.CstPopNameTitle.Text = inClassCustomerDetail.NameTitle

        '性別
        '性別チェック
        If Not (String.IsNullOrEmpty(inClassCustomerDetail.Sex)) AndAlso _
           SEX_MAN.Equals(inClassCustomerDetail.Sex) Then
            '「0：男性」の場合
            Me.CstPopMaleWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2 CheckBlack"

        ElseIf Not (String.IsNullOrEmpty(inClassCustomerDetail.Sex)) AndAlso _
               SEX_WOMAN.Equals(inClassCustomerDetail.Sex) Then
            '「1：女性」の場合
            Me.CstPopFemaleWordArea.Attributes("class") = "PoPuPS-CM-05Block1-2 CheckBlack"

        Else
            '上記以外の場合
            Me.CstPopMaleWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2"
            Me.CstPopFemaleWordArea.Attributes("class") = "PoPuPS-CM-05Block1-2"

        End If

        '顧客タイプ
        '顧客タイプチェック
        If INDIVIDUALCORPORATIONTYPE_IN.Equals(inClassCustomerDetail.Sex) Then
            '「0：法人」の場合
            Me.CstPopPrivateWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2 CheckBlack"

        ElseIf INDIVIDUALCORPORATIONTYPE_CO.Equals(inClassCustomerDetail.Sex) Then
            '「1：個人」の場合
            Me.CstPopCorporationWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2 CheckBlack"

        Else
            '上記以外の場合
            Me.CstPopPrivateWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2"
            Me.CstPopCorporationWordArea.Attributes("class") = "PoPuPS-CM-05Block2-2"

        End If

        'サブ顧客タイプ
        '個人法人項目コードのチェック
        If Not (String.IsNullOrEmpty(inClassCustomerDetail.SubCustomerType)) Then
            'データが存在する場合
            Me.CstPopSubCustomerType.Text = biz.GetPrivateFleetWord(inClassCustomerDetail.SubCustomerType)

        End If

        'VIPフラグ（表示しない）

        '携帯電話番号
        Me.CstPopMobile.Text = inClassCustomerDetail.Mobile

        '自宅電話番号
        Me.CstPopHome.Text = inClassCustomerDetail.TelNumber

        '自宅FAX番号
        Me.CstPopFax.Text = inClassCustomerDetail.FaxNumber

        '勤務先電話番号
        Me.CstPopOffice.Text = inClassCustomerDetail.BusinessTelNumber

        '電子メールアドレス1
        Me.CstPopEmail1.Text = inClassCustomerDetail.EMail1

        '電子メールアドレス2
        Me.CstPopEmail2.Text = inClassCustomerDetail.EMail2

        '郵便番号
        Me.CstPopZipCode.Text = inClassCustomerDetail.ZipCode

        '住所1
        Me.CstPopAddress1.Text = inClassCustomerDetail.Address1

        '住所2
        Me.CstPopAddress2.Text = inClassCustomerDetail.Address2

        '住所3
        Me.CstPopAddress3.Text = inClassCustomerDetail.Address3

        '国籍
        Me.CstPopNationality.Text = inClassCustomerDetail.Country

        '本籍
        Me.CstPopDomicile.Text = inClassCustomerDetail.Domicile

        '生年月日（YYYY/MM/DD）
        '誕生日チェック
        If Not (String.IsNullOrEmpty(inClassCustomerDetail.BirthDay)) Then
            'データが存在する場合
            '生年月日（日付型）のチェック
            Dim birthDay As Date = Date.MinValue

            '日付変換チェック
            If Date.TryParseExact(inClassCustomerDetail.BirthDay, WebServiceDateFormat, Nothing, Nothing, birthDay) Then
                '成功した場合
                '文字列に変換して設定
                Me.CstPopBirthDate.Text = _
                    DateTimeFunc.FormatDate(3, birthDay)

            Else
                '失敗した場合
                '空文字を設定
                Me.CstPopBirthDate.Text = String.Empty

            End If

        End If

        '会社名称
        Me.CstPopCompanyName.Text = inClassCustomerDetail.CompanyName

        '担当者氏名（法人）
        Me.CstPopEmployeeName.Text = inClassCustomerDetail.EmployeeName

        '担当者部署名（法人）
        Me.CstPopDepartment.Text = inClassCustomerDetail.EmployeeDepartment

        '役職（法人）
        Me.CstPopOfficialPosition.Text = inClassCustomerDetail.EmployeePosition

        '顧客コード（基幹顧客ID）
        Me.CstPopDmsCustomerCode.Text = inClassCustomerDetail.CustomerCode

        '国民番号
        Me.CstPopSocialId.Text = inClassCustomerDetail.SocialId

        '未取引客ユーザーID
        Me.CstPopNewCustomerId.Text = inClassCustomerDetail.NewcustomerId

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 車両詳細エリア設定処理
    ''' </summary>
    ''' <param name="drVehicleInfo">車両情報</param>
    ''' <remarks></remarks>
    Private Sub SetVehicleDetailArea(ByVal drVehicleInfo As SC3080225VehicleInfoRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メーカー名
        Me.VclPopMakerName.Text = drVehicleInfo.MakerCode

        'モデルコード
        Me.VclPopModelName.Text = drVehicleInfo.ModelCode

        '車両登録No
        Me.VclPopRegNo.Text = drVehicleInfo.VehicleRegistrationNumber

        '車両登録エリア名称
        Me.VclPopProvince.Text = drVehicleInfo.VehicleAreaName

        'VIN
        Me.VclPopVin.Text = drVehicleInfo.Vin

        '基本型式
        Me.VclPopKatashiki.Text = drVehicleInfo.BaseType

        '燃料
        Me.VclPopFuel.Text = drVehicleInfo.FuelDivisionName

        '外板色名称
        Me.VclPopBodyColor.Text = drVehicleInfo.BodyColorName

        'エンジンNo
        Me.VclPopEngineNo.Text = drVehicleInfo.EngineNumber

        'トランスミッション
        Me.VclPopTransmission.Text = drVehicleInfo.Transmission

        '登録日
        Me.VclPopRegDate.Text = drVehicleInfo.VehicleRegistrationDate

        '納車日
        Me.VclPopDeliDate.Text = drVehicleInfo.VehicleDeliveryDate

        '車両区分
        Me.VclPopVehicleType.Text = drVehicleInfo.NewVehicleDivisionName

        '最終整備完了日
        Me.VclPopServiceCompletedDate.Text = drVehicleInfo.RegistDate

        '最新走行距離
        Me.VclPopMileage.Text = drVehicleInfo.Mileage

        '保険会社名
        Me.VclPopInsuranceCompany.Text = drVehicleInfo.CompanyName

        '保険証券番号
        Me.VclPopInsurancePolicyNo.Text = drVehicleInfo.InsNo

        '保険満期日
        Me.VclPopInsuranceExpiryDate.Text = drVehicleInfo.EndDate

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 保有車両エリア設定処理
    ' ''' </summary>
    ' ''' <param name="dtSortVehicleInfo"></param>
    ' ''' <remarks></remarks>
    'Private Sub SetVehicleSelectArea(ByVal dtSortVehicleInfo As SC3080225VehicleInfoRow())
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} START ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '保有車両情報をバインド
    '    Me.VehicleSelectListRepeater.DataSource = dtSortVehicleInfo
    '    Me.VehicleSelectListRepeater.DataBind()

    '    'データを設定する.
    '    For i = 0 To VehicleSelectListRepeater.Items.Count - 1
    '        'HTMLコントロール取得
    '        Dim vehicleSelectListArea As Control = VehicleSelectListRepeater.Items(i)

    '        'レコード情報取得
    '        Dim drVehicleInfo As SC3080225VehicleInfoRow = dtSortVehicleInfo(i)

    '        '1行目のレコードかチェック
    '        If i = 0 Then
    '            '1行目の場合
    '            '背景を灰色にする
    '            CType(vehicleSelectListArea.FindControl("VclSelPopRecord"), HtmlControl).Attributes("class") = "PoPuPS-CM-07Block1 BGColorGray"

    '        End If

    '        'ロゴURL
    '        'ロゴ情報のチェック
    '        If Not (drVehicleInfo.IsModelLogoOffURLNull) AndAlso _
    '           Not (String.IsNullOrEmpty(drVehicleInfo.ModelLogoOffURL)) AndAlso _
    '           Not (drVehicleInfo.IsModelLogoOnURLNull) AndAlso _
    '           Not (String.IsNullOrEmpty(drVehicleInfo.ModelLogoOnURL)) Then
    '            'ロゴ情報がすべて存在する場合
    '            'ロゴURL設定
    '            CType(vehicleSelectListArea.FindControl("VclSelPopVehicleLogoIcon"), HtmlControl).Attributes("src") = drVehicleInfo.ModelLogoOffURL

    '            'ロゴを表示
    '            CType(vehicleSelectListArea.FindControl("VclSelPopVehicleLogoIcon"), HtmlControl).Attributes("style") = "display:block;"

    '            'メーカー名とモデル名を非表示
    '            CType(vehicleSelectListArea.FindControl("VclSelPopVehicleMakerModelTable"), HtmlControl).Attributes("style") = "display:none;"

    '        Else
    '            'ロゴ情報が1つでも存在しない場合
    '            'ロゴを非表示
    '            CType(vehicleSelectListArea.FindControl("VclSelPopVehicleLogoIcon"), HtmlControl).Attributes("style") = "display:none;"

    '            'メーカー名とモデル名を表示
    '            CType(vehicleSelectListArea.FindControl("VclSelPopVehicleMakerModelTable"), HtmlControl).Attributes("style") = "display:block;"

    '        End If

    '        'レコード番号生成
    '        CType(vehicleSelectListArea.FindControl("VclSelPopRecord"), HtmlControl).Attributes("RecordIndex") = _
    '            i.ToString(CultureInfo.CurrentCulture)

    '    Next

    '    'JSONデータ作成して格納
    '    Me.HiddenFieldVehicleListJsonData.Value = Me.CreateJsonData(dtSortVehicleInfo)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} END ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

    ''' <summary>
    ''' 固有車両データのJSON変換処理
    ''' </summary>
    ''' <param name="dtSortVehicleInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateJsonData(ByVal dtSortVehicleInfo As SC3080225VehicleInfoRow()) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim returnJsonData As String = String.Empty

        '格納用
        Dim resultMain As New Dictionary(Of String, Object)

        'シリアライズ用
        Dim serializer As New JavaScriptSerializer

        'Row配列をDataTableに変換する
        Dim dt As New SC3080225VehicleInfoDataTable
        For Each dr As SC3080225VehicleInfoRow In dtSortVehicleInfo
            Dim drNew As SC3080225VehicleInfoRow = dt.NewSC3080225VehicleInfoRow
            drNew.ItemArray = dr.ItemArray
            dt.Rows.Add(drNew)
        Next

        'JSONデータ生成
        For Each dr As DataRow In dt
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dt.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add(resultMain.Count.ToString(CultureInfo.CurrentCulture), result)
        Next

        'JSONを文字列に変換
        returnJsonData = serializer.Serialize(resultMain)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnJsonData
    End Function

    ''' <summary>
    ''' 初期表示入庫履歴情報設定処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <remarks></remarks>
    Private Sub SetInitServiceInHistoryInfo(ByVal inDealerCode As String, _
                                            ByVal inVin As String, _
                                            ByVal inRegsiterNumber As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , inDealerCode _
              , inVin _
              , inRegsiterNumber))

        Using biz As New SC3080225BusinessLogic
            '自販売店の入庫履歴情報を取得する
            Dim dtMyContactHistoryInfo As SC3080225ContactHistoryInfoDataTable = _
                biz.GetServiceInHistoryInfo(inDealerCode, _
                                            inVin, _
                                            inRegsiterNumber, _
                                            False)

            '全ての入庫履歴情報を取得する
            Dim dtAllContactHistoryInfo As SC3080225ContactHistoryInfoDataTable = _
                biz.GetServiceInHistoryInfo(String.Empty, _
                                            inVin, _
                                            inRegsiterNumber, _
                                            True)

            '入庫履歴表示処理
            Me.SetServiceInHistoryArea(inDealerCode, _
                                       dtMyContactHistoryInfo, _
                                       dtAllContactHistoryInfo, _
                                       False)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 入庫履歴情報の表示設定
    ''' </summary>
    ''' <param name="dtMyContactHistoryInfo">自販売店入庫履歴情報</param>
    ''' <param name="dtAllContactHistoryInfo">全販売店入庫履歴情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正
    ''' 2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
    ''' 2019/06/07 NSK 鈴木 【18PRJ02275-00_(FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】[TKM]UAT-0117 顧客詳細の入庫履歴について、最新の履歴が0000で表示される
    ''' </history>
    Private Sub SetServiceInHistoryArea(ByVal inDealerCode As String, _
                                        ByVal dtMyContactHistoryInfo As SC3080225ContactHistoryInfoDataTable, _
                                        ByVal dtAllContactHistoryInfo As SC3080225ContactHistoryInfoDataTable, _
                                        ByVal inAllType As Boolean)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} START" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim drSetContactHistoryInfo As SC3080225ContactHistoryInfoRow() = Nothing

        '入庫履歴情報チェック
        If Not (inAllType) AndAlso _
           Not (IsNothing(dtMyContactHistoryInfo)) AndAlso _
           0 < dtMyContactHistoryInfo.Count Then
            '自販売店検索且つ、自販売店の入庫履歴が存在する場合
            '現在表示件数を初期化
            Me.HiddenFieldOtherHistoryDispCount.Value = "0"

            '自販売店の入庫履歴情報を使用
            drSetContactHistoryInfo = _
                (From dr As SC3080225ContactHistoryInfoRow In dtMyContactHistoryInfo _
                 Where dr.ROW_COUNT <= SERVICEIN_HISTORY_INIT_PAGE).ToArray

            'コントロールにバインドする
            Me.ServiceInHistoryRepeater.DataSource = drSetContactHistoryInfo
            Me.ServiceInHistoryRepeater.DataBind()

        ElseIf inAllType AndAlso _
               Not (IsNothing(dtAllContactHistoryInfo)) AndAlso _
               0 < dtAllContactHistoryInfo.Count Then
            '全販売店検索且つ、全販売店の入庫履歴が存在する場合
            '現在表示件数を取得
            Dim otherHistoryDispCount As Integer = CType(Me.HiddenFieldOtherHistoryDispCount.Value, Integer)

            '次ページ表示件数を取得
            Dim displayCount As Integer = CType(Me.HiddenFieldDisplayCount.Value, Integer)

            '現在表示件数に次ページ表示件数を加算して格納
            Me.HiddenFieldOtherHistoryDispCount.Value = CType(otherHistoryDispCount + displayCount, String)

            '全販売店の入庫履歴情報を使用
            drSetContactHistoryInfo = _
                (From dr As SC3080225ContactHistoryInfoRow In dtAllContactHistoryInfo _
                 Where dr.ROW_COUNT <= (otherHistoryDispCount + displayCount)).ToArray

            'コントロールにバインドする
            Me.ServiceInHistoryRepeater.DataSource = drSetContactHistoryInfo
            Me.ServiceInHistoryRepeater.DataBind()

        Else
            '上記以外
            '現在表示件数を初期化
            Me.HiddenFieldOtherHistoryDispCount.Value = "0"

        End If

        '「一般点検」の文言取得
        Dim wordGeneralMaintenance As String = WebWordUtility.GetWord(172)

        '「定期点検」の文言取得
        Dim wordPeriodicalInspection As String = WebWordUtility.GetWord(173)

        '現在日時取得
        Dim nowDate As Date = DateTimeFunc.Now(inDealerCode)

        'データを設定する.
        For i = 0 To ServiceInHistoryRepeater.Items.Count - 1
            'HTMLコントロール取得
            Dim serviceInHistoryArea As Control = ServiceInHistoryRepeater.Items(i)

            'レコード情報取得
            Dim drContactHistoryInfo As SC3080225ContactHistoryInfoRow = drSetContactHistoryInfo(i)

            '納車日時
            '納車日時チェック
            If Not (drContactHistoryInfo.IsSVCIN_DELI_DATENull) AndAlso drContactHistoryInfo.SVCIN_DELI_DATE <> Date.MinValue Then
                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START
                'If drContactHistoryInfo.SVCIN_DELI_DATE <> Date.MinValue Then
                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

                'データが存在する場合
                ' 2019/06/07 NSK 鈴木 【18PRJ02275-00_(FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】[TKM]UAT-0117 顧客詳細の入庫履歴について、最新の履歴が0000で表示される START
                ' ' 時間範囲チェック
                ' If String.Equals(nowDate.ToString("yyyyMMdd", CultureInfo.CurrentCulture), _
                '                  drContactHistoryInfo.SVCIN_DELI_DATE.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then
                '     ' 当日の場合
                '     '「hh:mm」に変換して設定
                '     CType(serviceInHistoryArea.FindControl("ServiceInDate"), CustomLabel).Text = _
                '         DateTimeFunc.FormatDate(14, drContactHistoryInfo.SVCIN_DELI_DATE)

                ' Else
                '     ' 上記以外
                '     '「㎜/dd」に変換して設定
                '     CType(serviceInHistoryArea.FindControl("ServiceInDate"), CustomLabel).Text = _
                '         DateTimeFunc.FormatDate(11, drContactHistoryInfo.SVCIN_DELI_DATE)

                'End If

                ' 「dd/mm/yyyy」に変換して設定
                CType(serviceInHistoryArea.FindControl("ServiceInDate"), CustomLabel).Text = _
                    DateTimeFunc.FormatDate(3, drContactHistoryInfo.SVCIN_DELI_DATE)
                ' 2019/06/07 NSK 鈴木 【18PRJ02275-00_(FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】[TKM]UAT-0117 顧客詳細の入庫履歴について、最新の履歴が0000で表示される END

            Else
                '存在しない場合
                '空文字を設定
                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START
                'CType(serviceInHistoryArea.FindControl("ServiceInDate"), CustomLabel).Text = String.Empty
                CType(serviceInHistoryArea.FindControl("ServiceInDate"), CustomLabel).Text = "&nbsp;"
                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

            End If

            'RO番号
            Dim orderNumber As String = String.Empty
            '入庫管理番号チェック
            If Not (drContactHistoryInfo.IsSVCIN_NUMNull) AndAlso _
               Not (String.IsNullOrEmpty(drContactHistoryInfo.SVCIN_NUM)) Then
                '存在する場合
                'データを格納
                orderNumber = drContactHistoryInfo.SVCIN_NUM.Split(CChar("@"))(0)

            End If

            CType(serviceInHistoryArea.FindControl("RepairOrderNo"), CustomLabel).Text = orderNumber

            '整備種類
            'サービス名称チェック
            If Not (drContactHistoryInfo.IsSVC_NAME_MILENull) Then
                '存在する場合
                '「定期点検」を設定
                CType(serviceInHistoryArea.FindControl("MaintenanceType"), CustomLabel).Text = wordPeriodicalInspection

                '整備項目
                CType(serviceInHistoryArea.FindControl("ServiceName"), CustomLabel).Text = _
                    drContactHistoryInfo.SVC_NAME_MILE

            Else
                '存在しない場合
                '「一般点検」を設定
                CType(serviceInHistoryArea.FindControl("MaintenanceType"), CustomLabel).Text = wordGeneralMaintenance

                '整備項目
                '整備名チェック
                If Not (drContactHistoryInfo.IsMAINTE_NAMENull) Then
                    '存在する場合
                    CType(serviceInHistoryArea.FindControl("ServiceName"), CustomLabel).Text = _
                        drContactHistoryInfo.MAINTE_NAME

                End If

            End If

            '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
            If Not (drContactHistoryInfo.IsMAINTE_NAME_HISNull) AndAlso _
               Not (String.IsNullOrEmpty(drContactHistoryInfo.MAINTE_NAME_HIS)) Then
                '整備履歴.整備名称がある場合は、左記を代表整備項目へ設定
                CType(serviceInHistoryArea.FindControl("ServiceName"), CustomLabel).Text = drContactHistoryInfo.MAINTE_NAME_HIS
            End If
            '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END

            '担当者名
            '担当者名チェック
            If Not (drContactHistoryInfo.IsSTF_NAMENull) AndAlso _
               Not (String.IsNullOrEmpty(drContactHistoryInfo.STF_NAME)) Then
                'データが存在する場合
                'スタッフ名を設定
                CType(serviceInHistoryArea.FindControl("StaffName"), CustomLabel).Text = _
                    drContactHistoryInfo.STF_NAME

            End If

            'タップ時で使用するパラメーターを設定「販売店コード, RO番号, 入庫管理番号」
            CType(serviceInHistoryArea.FindControl("mainblockContentRightTabAll01"), HtmlControl).Attributes("serviceinValue") = _
                String.Concat(drContactHistoryInfo.DLR_CD, ",", orderNumber, ",", drContactHistoryInfo.SVCIN_NUM)

        Next

        '全入庫履歴件数のチェック
        If Not (inAllType) AndAlso _
           Not (IsNothing(dtAllContactHistoryInfo)) AndAlso _
           0 < dtAllContactHistoryInfo.Count Then
            '自販売店表示の場合
            '自販売店の件数と全販売店の件数チェック
            If Not (IsNothing(drSetContactHistoryInfo)) AndAlso _
               drSetContactHistoryInfo.Count = dtAllContactHistoryInfo.Count Then
                '自販売店の件数と全販売店の件数が同じの場合
                'リンクをすべて非表示
                Me.AllDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextLodingDiv.Attributes("style") = "display:none;"

            Else
                '自販売店の件数と全販売店の件数が違うの場合
                '「すべての入庫履歴」を表示
                Me.AllDispLinkDiv.Attributes("style") = "display:block;"
                Me.NextDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextLodingDiv.Attributes("style") = "display:none;"

            End If

        ElseIf inAllType AndAlso _
               Not (IsNothing(dtAllContactHistoryInfo)) AndAlso _
               0 < dtAllContactHistoryInfo.Count Then
            '全販売店表示の場合
            '現在表示件数と全販売店の件数チェック
            If CType(Me.HiddenFieldOtherHistoryDispCount.Value, Integer) < dtAllContactHistoryInfo.Count Then
                '表示件数が全販売店より小さい場合
                '「次のN件を表示する」を表示
                Me.AllDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextDispLinkDiv.Attributes("style") = "display:block;"
                Me.NextLodingDiv.Attributes("style") = "display:none;"

            Else
                '表示件数が全販売店より多い場合
                'リンクを非表示
                Me.AllDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextDispLinkDiv.Attributes("style") = "display:none;"
                Me.NextLodingDiv.Attributes("style") = "display:none;"

            End If

        Else
            '上記以外の場合
            Me.AllDispLinkDiv.Attributes("style") = "display:none;"
            Me.NextDispLinkDiv.Attributes("style") = "display:none;"
            Me.NextLodingDiv.Attributes("style") = "display:none;"

        End If

        '入庫エリア更新
        Me.AjaxHistoryPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
    ''' <summary>
    ''' SSCアイコン情報の表示設定
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inRegsiterNumber">車両登録番号</param>
    ''' <remarks></remarks>
    Private Sub SetInitSscIconInfo(ByVal inDealerCode As String, _
                                   ByVal inVin As String, _
                                   ByVal inRegsiterNumber As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , inDealerCode _
              , inVin _
              , inRegsiterNumber))

        Using biz As New SC3080225BusinessLogic
            'SSC対象フラグを取得する
            Dim sscFlag As String = biz.GetSscFlg(inDealerCode, _
                                                  inVin, _
                                                  inRegsiterNumber)

            'アイコンテキスト設定
            Me.SSCWord.Text = WebWordUtility.GetWord(WordId.id208)
            'アイコンフラグ設定
            Me.HiddenFieldSscFlag.Value = sscFlag

        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' M/B/E/T/P/Lアイコン情報の表示設定
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inRegsiterNumber">車両登録番号</param>
    ''' <remarks></remarks>
    Private Sub SetVehicleIcon(ByVal inDealerCode As String, _
                               ByVal inVin As String, _
                               ByVal inRegsiterNumber As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , inDealerCode _
              , inVin _
              , inRegsiterNumber))

        Using biz As New SC3080225BusinessLogic
            '各アイコンのフラグを取得する
            Dim dtvehicleFlag As SC3080225VehicleFlgDataTable = biz.GetVehicleFlg(inDealerCode, _
                                                                                  inVin, _
                                                                                  inRegsiterNumber)
            'アイコンテキスト設定
            Me.MWord.Text = WebWordUtility.GetWord(WordId.id10001)
            Me.BWord.Text = WebWordUtility.GetWord(WordId.id10002)
            Me.EWord.Text = WebWordUtility.GetWord(WordId.id10003)
            Me.TWord.Text = WebWordUtility.GetWord(WordId.id10004)
            Me.PWord.Text = WebWordUtility.GetWord(WordId.id10005)
            Me.LIcon.Text = WebWordUtility.GetWord(WordId.id10006)

            'データが取得できた場合
            If (0 < dtvehicleFlag.Count) Then

                Dim drSetVehicleIcon As SC3080225VehicleFlgRow = dtvehicleFlag.First()
                'アイコンフラグ設定
                Me.HiddenFieldImpFlg.Value = drSetVehicleIcon.IMP_VCL_FLG
                Me.HiddenFieldSmlAmcFlg.Value = drSetVehicleIcon.SML_AMC_FLG
                Me.HiddenFieldEwFlg.Value = drSetVehicleIcon.EW_FLG
                Me.HiddenFieldTlmMbrFlg.Value = drSetVehicleIcon.TLM_MBR_FLG
            Else
                '各アイコンフラグが取得できない場合はアイコン表示用HiddenFieldを初期化する
                Me.HiddenFieldImpFlg.Value = FLAG_DEFAULT_VALUE
                Me.HiddenFieldSmlAmcFlg.Value = FLAG_DEFAULT_VALUE
                Me.HiddenFieldEwFlg.Value = FLAG_DEFAULT_VALUE
                Me.HiddenFieldTlmMbrFlg.Value = FLAG_DEFAULT_VALUE
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} END" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ''' <summary>
    ''' 保有車両リストの文言取得
    ''' </summary>
    ''' <returns>保有車両リストの文言</returns>
    ''' <remarks></remarks>
    Private Function GetWordVehicleListItem() As Dictionary(Of String, String)

        Dim dict = New Dictionary(Of String, String)

        '車両登録番号
        dict("8") = WebWordUtility.GetWord(8)
        'VIN
        dict("9") = WebWordUtility.GetWord(9)
        '納車日
        dict("10") = WebWordUtility.GetWord(10)
        '最新走行距離
        dict("11") = WebWordUtility.GetWord(11)
        '最新走行距離更新日
        dict("170") = WebWordUtility.GetWord(170)
        '前の件数表示のラベル追加
        dict("209") = WebWordUtility.GetWord(209) _
        .Replace("{0}", CType(Me.HiddenFieldDefaultReadCount.Value, String))
        dict("210") = WebWordUtility.GetWord(210) _
        .Replace("{0}", CType(Me.HiddenFieldDefaultReadCount.Value, String))
        '次のN件をラベルに設定
        dict("211") = WebWordUtility.GetWord(211) _
        .Replace("{0}", CType(Me.HiddenFieldDefaultReadCount.Value, String))
        dict("212") = WebWordUtility.GetWord(212) _
        .Replace("{0}", CType(Me.HiddenFieldDefaultReadCount.Value, String))

        Return dict

    End Function

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#End Region

#Region "顧客関連情報に必要な宣言"
    Public Function ContainsKeyBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ICustomerDetailControl.ContainsKeyBypass
        Return Me.ContainsKey(pos, key)
    End Function
    Public Function GetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ICustomerDetailControl.GetValueBypass
        Try
            Return Me.GetValue(pos, key, removeFlg)
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Sub RemoveValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ICustomerDetailControl.RemoveValueBypass
        Me.RemoveValue(pos, key)
    End Sub
    Public Sub SetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal value As Object) Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ICustomerDetailControl.SetValueBypass
        Me.SetValue(pos, key, value)
    End Sub
    Public Sub ShowMessageBoxBypass(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ICustomerDetailControl.ShowMessageBoxBypass
        Me.ShowMessageBox(wordNo, wordParam)
    End Sub
#End Region

End Class
