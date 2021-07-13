'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080201.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/01/27 TCS 河原 【SALES_1B】
'更新： 2012/04/13 TCS 河原 【SALES_2】 号口課題No.114対応
'更新： 2012/05/17 TCS 安田 クルクル対応 
'更新： 2012/07/25 TCS 河原 顧客IDスペース対応
'更新： 2012/08/13 TCS 安田 商談中断メニューの追加
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2012/11/22 TCS 坪根 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/11/06 TCS 山田 i-CROP再構築後の新車納車システムに追加したリンク対応
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 山口,高橋　受注後フォロー機能開発
'更新： 2014/05/07 TCS 高橋 受注後フォロー機能開発
'更新： 2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008） 
'更新： 2014/04/02 TCS 河原 性能改善
'更新： 2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移)
'更新： 2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応)
'更新： 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2016/09/09 TCS 藤井 セールスタブレット性能改善 
'更新： 2016/09/14 TCS 河原 セールスタブレット性能改善 
'更新： 2018/06/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
'─────────────────────────────────────

Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationManager

Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess

Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080201DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Common
Imports System.Globalization

'2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
Imports Toyota.eCRB.CommonUtility.DataAccess
'2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START

'2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
Imports System.Web.Services
Imports System.Web.Script.Services
Imports Toyota.eCRB.CommonUtility.BizLogic
'2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

'2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
Imports System.Web.Script.Serialization
'2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

Partial Class Pages_SC3080201_Control
    Inherits BasePage
    Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl
    Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl

#Region " セッションキー "

    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>FBOX SEQNO</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"

    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    ''' <summary>FOLLOW_UP_BOX_NEW</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_NEW As String = "SearchKey.FOLLOW_UP_BOX_NEW"

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>来店実績連番</summary>
    Private Const SESSION_KEY_VISITSEQ As String = "SearchKey.VISITSEQ"

    ''' <summary>来店実績の仮登録氏名</summary>
    Private Const SESSION_KEY_TENTATIVENAME As String = "SearchKey.TENTATIVENAME"

    ''' <summary>来店実績の車両登録No.</summary>
    Private Const SESSION_KEY_VCLREGNO As String = "SearchKey.VCLREGNO"

    ''' <summary>来店実績の来店人数</summary>
    Private Const SESSION_KEY_WALKINNUM As String = "SearchKey.WALKINNUM"

    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
    ''' <summary>来店実績の電話番号</summary>
    Private Const SESSION_KEY_TELNO As String = "SearchKey.TELNO"
    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END

    ''' <summary>顧客名</summary>
    Private Const SESSION_KEY_CUSTNAME As String = "SearchKey.CUSTNAME"

    ''' <summary>商談中Follow-upBox内連番</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_SALES As String = "SearchKey.FOLLOW_UP_BOX_SALES"

    ''' <summary>商談中Follow-upBox店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD_SALES As String = "SearchKey.FLLWUPBOX_STRCD_SALES"

    ''' <summary>顧客名 + 敬称</summary>
    Private Const SESSION_KEY_NAME As String = "SearchKey.NAME"

    ''' <summary>ステータス</summary>
    Private Const SESSION_KEY_PRESENCECATEGORY As String = "SearchKey.PRESENCECATEGORY"
    Private Const SESSION_KEY_PRESENCEDETAIL As String = "SearchKey.PRESENCEDETAIL"

    ''' <summary>表示ページ</summary>
    Private Const SESSION_KEY_DISPPAGE As String = "SearchKey.DISPPAGE"

    ''' <summary> セッションキー 車両登録No初期表示フラグ (1:車両登録Noを表示する)</summary>
    Public Const SESSION_KEY_VCLREGNODISPFLG As String = "SearchKey.VCLREGNODISPFLG"

    '2012/01/24 TCS 河原 【SALES_1B】 END
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' 受注NO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ORDER_NO As String = "SearchKey.ORDER_NO"
    ''' <summary>
    ''' 未取引客ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_NEW_CUST_ID As String = "SearchKey.NEW_CUST_ID"
    ''' <summary>
    ''' セールスステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALES_STATUS As String = "SearchKey.SALES_STATUS"
    ''' <summary>
    ''' 受注後フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALESAFTER As String = "SearchKey.SALESAFTER"
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ' ''' <summary>
    ' ''' 商談シーケンスNO
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSION_KEY_SALES_SEQNO As String = "SearchKey.SALES_SEQNO"
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    ' 2012/03/09 TCS 山口 【SALES_2】CSSurvey初期起動の場合コンテキストメニューを非活性に START
    ''' <summary>
    ''' 回答ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ANSWERID As String = "SearchKey.ANSWERID"
    ' 2012/03/09 TCS 山口 【SALES_2】CSSurvey初期起動の場合コンテキストメニューを非活性に END

    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' 活動ID
    Private Const SESSION_KEY_ACT_ID As String = "SearchKey.ACT_ID"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

#End Region

#Region " 定数 "
    ''' <summary>
    ''' 最大ページ数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGECOUNT_MAX As Integer = 3

    '2012/01/24 TCS 河原 【SALES_1B】 START
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除

    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' 活動ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SALES_START As Integer = 1
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    Private Const C_SALES_END As Integer = 2
    'Private Const C_SALES_CANCEL As Integer = 2
    ' 2012/02/15 TCS 相田 【SALES_2】 END
    Private Const C_BUSINESS_START As Integer = 3
    Private Const C_BUSINESS_CANCEL As Integer = 4
    Private Const C_CORRESPOND_START As Integer = 5
    Private Const C_CORRESPOND_END As Integer = 6
    '2012/01/24 TCS 河原 【SALES_1B】 END

    ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
    Private Const C_SALES_STOP As Integer = 7
    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    '納車作業開始
    Private Const C_DELIVERY_START As Integer = 8
    '納車作業終了
    Private Const C_DELIVERY_END As Integer = 9
    '納車作業開始(一時対応)
    Private Const C_DELIVERYCORRESPOND_START As Integer = 10
    '納車作業終了(一時対応)
    Private Const C_DELIVERYCORRESPOND_END As Integer = 11
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    '2012/01/27 TCS 平野 【SALES_1B】 START
    Private Const CONTRACT As String = "1"
    '2012/01/27 TCS 平野 【SALES_1B】 END

    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORGCUSTFLG As String = "1"
    ''' <summary>
    ''' 受注フラグ (0：受注時)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESATER_ORDER As String = "0"
    ''' <summary>
    ''' 受注フラグ (1：受注後工程フォロー)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESATER_AFTER As String = "1"
    ''' <summary>
    ''' 登録フラグ　未登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REGISTFLG_NOTREGIST As String = "0"
    ''' <summary>
    ''' 登録フラグ　登録済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REGISTFLG_REGIST As String = "1"
    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' 接客区分:商談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CST_SERVICE_TYPE_SALES As String = "1"

    ''' <summary>
    ''' 接客区分:納車作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CST_SERVICE_TYPE_DELIVERY As String = "2"
    '2014/02/12 TCS 高橋 受注後フォロー機能開発 END


    ''2012/05/17 TCS 安田 クルクル対応 START

    ''' <summary>
    ''' 在席状態：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_WAIT As String = "10"

    ''' <summary>
    ''' 在席状態：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_NEGOTIATION As String = "20"

    ''' <summary>
    ''' 在席状態：営業活動中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_BUSINESS As String = "11"

    ''' <summary>
    ''' 在席状態：一時対応
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_CORRESPOND As String = "21"

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    ''' <summary>
    ''' 在席状態：納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_DELIVERY As String = "22"

    ''' <summary>
    ''' 在席状態：納車作業中(一時対応)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_DELIVERYCORRESPOND As String = "23"
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    ''' <summary>
    ''' プログラムID：活動結果登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_SC3080203 As String = "SC3080203"

    ''' <summary>
    ''' プログラムID：受注後工程フォロー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_SC3080216 As String = "SC3080216"

    ''2012/05/17 TCS 安田 クルクル対応 END

    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <summary>
    ''' CR活動結果(SUCCESS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_SUCCESS As String = "3"

    ''' <summary>
    ''' サクセスフラグ(サクセス済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUCCESS_FLG_COMPLETED As String = "1"
    '2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除

    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    '新車納車システム連携メニュー
    Private Const LINK_MENU As Integer = FooterMenuCategory.LinkMenu
    'リンク先URL
    Private Const C_LINK_MENU_URL As String = "LINK_MENU_URL"
    'URLスキーム
    Private Const URL_SCHEME As String = "TABLET_BROWSER_URL_SCHEME"
    Private Const URL_SCHEMES As String = "TABLET_BROWSER_URL_SCHEMES"
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD START
    ''' <summary>
    ''' マスターページ文言取得ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MSTPG_DISPLAYID As String = "MASTERPAGEMAIN"
    '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD END

#End Region

#Region " 変数 "

#End Region

    ''' <summary>
    ''' 処理判定済みフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private BookedAfterCheckFlg As Boolean = False

    ''' <summary>
    ''' 契約No.
    ''' </summary>
    ''' <remarks></remarks>
    Private retContractNo As String = String.Empty

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' 同一クエリ重複呼び出し回避
    ''' </summary>
    ''' <remarks></remarks>
    Private _activityStatus As SC3080202GetStatusToDataTable

    ''' <summary>
    ''' 同一クエリ重複呼び出し回避
    ''' </summary>
    ''' <remarks></remarks>
    Private _estimateId As String

    ''' <summary>
    ''' 同一クエリ重複呼び出し回避
    ''' </summary>
    ''' <remarks></remarks>
    Private _getestimateIdFlg As Boolean = False
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

#Region " ページロード "
    ''' <summary>
    ''' ページ初期化処理
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>子コントロール／イベントハンドラを動的に作成／割り当てする場合に使用する</remarks>
    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        RegisterPageInterfaceHandlers()
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2016/09/14 TCS 河原 TMTタブレット性能改善 START
        'コンテキストメニュー
        RegisterMenuEventHandler()
        '2016/09/14 TCS 河原 TMTタブレット性能改善 END

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ログ出力 Start ***************************************************************************
        Logger.Info("SC3080201(aspx) Page_Load Start")
        'ログ出力 End *****************************************************************************

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
        Dim params As String = CType(Master.FindControl("MstPG_TCV_Params"), TextBox).Text

        '2016/09/14 TCS 河原 TMTタブレット性能改善 START
        If Me.Page.IsCallback AndAlso Me.Page.IsPostBack Then
            Return
        End If
        '2016/09/14 TCS 河原 TMTタブレット性能改善 END

        If Not String.IsNullOrEmpty(Trim(params)) Then
            'JSON形式の文字列を変換
            Dim serializer As New JavaScriptSerializer
            Dim args As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(params)

            If CStr(args("StartPageId")).Equals("SC3070201") Then
                SetValue(ScreenPos.Current, "StartPageId", "SC3070201")
                Exit Sub
            End If
        End If
        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

        ''遷移元画面判定の為のログ出力暫定処理
        If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
            '型の判定
            Dim type As Integer = VarType(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False))
            If type <> 8 Then
                '遷移元画面判定の為のログ出力暫定処理
                Dim dispId As String = GetPrevScreenId().ToString()
                Dim errorMsg As New StringBuilder
                errorMsg.Append("遷移元画面:")
                errorMsg.Append(dispId)
                Logger.Error(errorMsg.ToString())
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            End If
        End If

        If Not Page.IsPostBack Then
            '検索ボックスの設定
            InitSearchBox()

            '' 2012/02/15 TCS 相田 【SALES_2】 START
            'If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_STATUS) Then
            '    Dim status As Integer = CInt(GetValue(ScreenPos.Current, SESSION_KEY_SALES_STATUS, False).ToString())
            '    'ステータス更新　営業活動開始
            '    updateStatus(status)
            '    RemoveValue(ScreenPos.Current, SESSION_KEY_SALES_STATUS)
            'End If
            '' 2012/02/15 TCS 相田 【SALES_2】 END
        End If

        Dim folloupseq As String = String.Empty
        Dim crcustid As String = String.Empty

        'TCVコールバックチェック
        TcvCheckAndSessionSet()

        '担当スタッフ
        SetStaffCd()

        '2012/01/24 TCS 河原 【SALES_1B】 START

        GetSession()

        '文言設定
        SetWord()

        '来店実績連番引き当て
        GetVisitSeq()

        '来店実績取得
        GetVisitResult()

        '2013/03/06 TCS 河原 GL0874 START
        SetContractCancelStartFlg()
        '2013/03/06 TCS 河原 GL0874 END

        'コンテキストメニュー初期表示フラグ
        Dim contextMenuOpenFlg As Boolean

        'ポストバックのときは非表示にする
        If Page.IsPostBack Then
            contextMenuOpenFlg = False
        Else
            contextMenuOpenFlg = True
            ' 2012/02/15 TCS 松野 【SALES_2】 START
            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_STATUS) Then
                Dim status As Integer = CInt(GetValue(ScreenPos.Current, SESSION_KEY_SALES_STATUS, False).ToString())
                'ステータス更新　営業活動開始
                updateStatus(status)
                RemoveValue(ScreenPos.Current, SESSION_KEY_SALES_STATUS)
            End If
            ' 2012/02/15 TCS 松野 【SALES_2】 END

            ' 2012/03/09 TCS 山口 【SALES_2】CSSurvey初期起動の場合コンテキストメニューを非活性に START
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_ANSWERID) = True) Then
                contextMenuOpenFlg = False
            End If
            ' 2012/03/09 TCS 山口 【SALES_2】CSSurvey初期起動の場合コンテキストメニューを非活性に END

            '2016/09/14 TCS 河原 TMTタブレット性能改善 START
            'コンテキストメニュー作成
            SetContextMenu(contextMenuOpenFlg)
            '2016/09/14 TCS 河原 TMTタブレット性能改善 END

        End If


        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        SetValue(ScreenPos.Current, SESSION_KEY_PRESENCECATEGORY, PresenceCategory)
        SetValue(ScreenPos.Current, SESSION_KEY_PRESENCEDETAIL, PresenceDetail)
        '2012/01/24 TCS 河原 【SALES_1B】 END

        ' 2012/02/15 TCS 相田 【SALES_2】 START
        Dim cstKind As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
            cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        End If
        If Not String.IsNullOrEmpty(Trim(cstKind)) Then
            If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID) Then
                SetCustId(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            End If
        End If
        ' 2012/02/15 TCS 相田 【SALES_2】 END

        '2012/07/25 TCS 河原 顧客IDスペース対応 START
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
            '活動先
            crcustid = Trim(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, crcustid)
        End If
        '2012/07/25 TCS 河原 顧客IDスペース対応 END

        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
            'FOLLOW_UP_BOX
            folloupseq = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()
        End If

        ' 2012/02/15 TCS 相田 【SALES_2】 START
        If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_ORDER_NO) Then
            '受注NOをセット
            SetOrderInfo(folloupseq)
        End If
        ' 2012/02/15 TCS 相田 【SALES_2】 END



        '各コントロールの制御
        SetPageControls(crcustid, folloupseq)

        '初期ページ設定
        SetInitPagePosition(crcustid, folloupseq)

        '2012/01/24 TCS 河原 【SALES_1B】 START
        '活動状況ステータスによる画面表示制御
        SetStatus()
        '2012/01/24 TCS 河原 【SALES_1B】 END

        'ヘッダー制御
        InitHeaderEvent()
        'フッター制御
        InitFooterEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SC3080201(aspx) Page_Load End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 初期表示の場合で顧客担当スタッフが存在しない場合、ＤＢから検索してセッションにセット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetStaffCd()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetStaffCd Start")
        'ログ出力 End *****************************************************************************

        '初期表示＆顧客情報ありで遷移してきた場合
        If Me.IsPostBack Or Not ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
            Return
        End If

        '文字数チェック
        If GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString().Length <= 0 Then
            Return
        End If

        '顧客情報ありで遷移した場合
        Using dataParam As New SC3080201DataSet.SC3080201CustInfoDataTable

            '担当セールススタッフ検索条件を作成
            Dim dr As SC3080201DataSet.SC3080201CustInfoRow = dataParam.NewSC3080201CustInfoRow()

            '顧客ＩＤ（未取ＩＤ or 自社客連番）
            dr.CUSTID = GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            '顧客種別
            dr.CUSTKIND = GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()

            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            '販売店コード
            dr.DLRCD = StaffContext.Current.DlrCD
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            'パラメータ追加
            dataParam.AddSC3080201CustInfoRow(dr)
            '検索処理
            Dim returnDt As SC3080201DataSet.SC3080201CustInfoDataTable = SC3080201BusinessLogic.GetCustInfo(dtParam:=dataParam)

            If returnDt.Rows.Count >= 1 Then
                Dim retrunDr As SC3080201DataSet.SC3080201CustInfoRow = CType(returnDt.Rows(0), SC3080201DataSet.SC3080201CustInfoRow)
                SetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, retrunDr.STAFFCD)
            Else
                SetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, StaffContext.Current.Account)
            End If

        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetStaffCd End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' TCVからコールバックされたかどうかのチェックを行い、
    ''' コールバックの場合、コールバック引数の値を次画面用セッションに格納します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TcvCheckAndSessionSet()

        'ログ出力 Start ***************************************************************************
        Logger.Info("TcvCheckAndSessionSet Start")
        'ログ出力 End *****************************************************************************

        If Not Me.IsPostBack And ContainsKey(ScreenPos.Current, "StartPageId") Then

            '顧客ＩＤがなければ即終了
            If Not ContainsKey(ScreenPos.Current, "CRCustId") Then
                Return
            ElseIf GetValue(ScreenPos.Current, "CRCustId", False).ToString().Length <= 0 Then
                Return
            End If

            'TCVからコールバックされた場合のセッションがある場合は、画面用のキーに変換して再セット
            For Each param As String In {"CRCustId", "CstKind", "CustomerClass", "FollowupBox_SeqNo", "StrCd"}

                'キー存在チェック
                If Not ContainsKey(ScreenPos.Current, param) Then
                    Continue For
                End If

                Select Case param
                    Case "CRCustId"
                        SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, GetValue(ScreenPos.Current, "CRCustId", False))
                    Case "CstKind"
                        SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, GetValue(ScreenPos.Current, "CstKind", False))
                    Case "CustomerClass"
                        SetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, GetValue(ScreenPos.Current, "CustomerClass", False))
                    Case "FollowupBox_SeqNo"
                        If GetValue(ScreenPos.Current, "FollowupBox_SeqNo", False).ToString.Length > 0 Then
                            SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, GetValue(ScreenPos.Current, "FollowupBox_SeqNo", False))
                        End If
                    Case "StrCd"
                        SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, GetValue(ScreenPos.Current, "StrCd", False))

                End Select
            Next

            RemoveValue(ScreenPos.Current, "StartPageId")

        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("TcvCheckAndSessionSet End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' ロード完了時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        'ログ出力 Start ***************************************************************************
        Logger.Info("Page_LoadComplete Start")
        'ログ出力 End *****************************************************************************

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '顧客編集ポップアップで電話番号検索ボタンがタップされた場合、顧客検索画面へ遷移
        If CType(Sc3080201Page.FindControl("telSerchFlgHidden"), HiddenField).Value.Equals("1") Then
            Me.RedirectNextScreen("SC3080101")
        End If
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '2016/09/14 TCS 河原 TMTタブレット性能改善 START
        If Me.IsCallback OrElse (ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack) Then
            Exit Sub
        End If
        '2016/09/14 TCS 河原 TMTタブレット性能改善 END

        If Me.IsPostBack Then

            Dim folloupseq As String = String.Empty
            Dim crcustid As String = String.Empty

            Dim VisibleFlg As Boolean = Sc3080203Page.Visible

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            Dim cstKind As String = String.Empty
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
                cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            End If
            If Not String.IsNullOrEmpty(Trim(cstKind)) Then
                If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID) Then
                    SetCustId(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
                End If
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                '活動先
                crcustid = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            End If

            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
                'FOLLOW_UP_BOX
                folloupseq = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()
            End If

            Me.SetActivityControl(crcustid, folloupseq)

            '2012/01/24 TCS 河原 【SALES_1B】 START
            '表示ページ更新
            SetStatus()

            'ヘッダーボタンの制御
            InitHeaderEvent()

            'コンテキストメニュー表示フラグ
            Dim contextMenuOpenFlg As Boolean
            contextMenuOpenFlg = False

            'コンテキストメニュー作成

            SetContextMenu(contextMenuOpenFlg)

            '文言取得
            SetWord()

            '3枚目の初期処理
            If Sc3080203Page.Visible Then
                If IsSession(SESSION_KEY_DISPPAGE) Then
                    Dim dispPage As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_DISPPAGE, False).ToString()
                    Me.RemoveValue(ScreenPos.Current, SESSION_KEY_DISPPAGE)
                    If Not String.Equals(dispPage, "3") Then
                        If Not VisibleFlg And Sc3080203Page.Visible Then
                            CType(Sc3080203Page, ISC3080203Control).ChangeFollow()
                        End If
                    End If
                Else
                    If Not VisibleFlg And Sc3080203Page.Visible Then
                        CType(Sc3080203Page, ISC3080203Control).ChangeFollow()
                    End If
                End If
            End If

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            '3枚目の初期処理
            If Sc3080216Page.Visible Then
                If IsSession(SESSION_KEY_DISPPAGE) Then
                    Dim dispPage As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_DISPPAGE, False).ToString()
                    Me.RemoveValue(ScreenPos.Current, SESSION_KEY_DISPPAGE)
                    If Not String.Equals(dispPage, "3") Then
                        If Sc3080216Page.Visible Then
                            CType(Sc3080216Page, ISC3080203Control).ChangeFollow()
                        End If
                    End If
                Else
                    If Sc3080216Page.Visible Then
                        CType(Sc3080216Page, ISC3080203Control).ChangeFollow()
                    End If
                End If
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            '2012/01/24 TCS 河原 【SALES_1B】 END
        End If

        '検索ボックス設定
        InitSearchBox()

        'フッターボタンの制御
        InitFooterEvent()

        '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD START
        'TCVボタン
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)

        If tcvButton.Enabled And IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            Dim openTcvScript As String = BuildOpenTcvScript()
            JavaScriptUtility.RegisterStartupScript(Me, openTcvScript, "openTcv", True)
        End If
        '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD END


        '2013/03/06 TCS 河原 GL0874 START
        SetContractCancelStartFlgAfter()
        '2013/03/06 TCS 河原 GL0874 END

        'ログ出力 Start ***************************************************************************
        Logger.Info("Page_LoadComplete End")
        'ログ出力 End *****************************************************************************

    End Sub

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' コンテキストメニュー作成
    ''' </summary>
    ''' <param name="contextMenuOpenFlg">自動初期表示フラグ</param>
    ''' <remarks>コンテキストメニュー作成する</remarks>
    Private Sub SetContextMenu(ByVal contextMenuOpenFlg As Boolean)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetContextMenu Start")
        'ログ出力 End *****************************************************************************

        Dim VisiFlg() As Boolean = GetVisiFlg()
        'キャンセル時のConfirm表示フラグ
        Dim checkFlg As Boolean = False
        '活動の情報があるが、それがまだFollow-upBoxに存在しない場合(今回新規で作成した場合)
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            Dim fllwupbox_dlrcd As String = StaffContext.Current.DlrCD
            Dim fllwupbox_strcd As String = StaffContext.Current.BrnCD
            Dim fllwupbox_seqno As Long = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)
            Dim biz As New SC3080201BusinessLogic
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            If Not biz.CountFllwupbox(fllwupbox_seqno) Then
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                'キャンセル時に確認メッセージを表示するためのフラグ設定
                checkFlg = True
            End If
        End If
        Dim menuItem As CommonMasterContextMenuItem
        With Me.localCommonMaster.ContextMenu

            'コンテキストメニューを自動表示するか設定
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
            If (VisiFlg(C_SALES_START) Or VisiFlg(C_BUSINESS_START) Or VisiFlg(C_CORRESPOND_START) Or
                VisiFlg(C_DELIVERY_START) Or VisiFlg(C_DELIVERYCORRESPOND_START)) And contextMenuOpenFlg Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                .UseAutoOpening = True

                'RegNo.ありで顧客登録直後は車両登録ポップアップを出す為にステータス変更ポップアップを出さない
                If IsSession(SESSION_KEY_VCLREGNODISPFLG) Then
                    Dim regDispFlg As String
                    regDispFlg = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG, False), String).ToString()
                    If String.Equals(regDispFlg, "1") Then
                        .UseAutoOpening = False
                    End If
                End If

            Else
                .UseAutoOpening = False
            End If

            '商談開始ボタン
            menuItem = .GetMenuItem(C_SALES_START)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10164)
                    .PresenceCategory = "2"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf SalesStartButton_Click
                    .Visible = VisiFlg(C_SALES_START)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            '商談キャンセルボタン
            'menuItem = .GetMenuItem(C_SALES_CANCEL)
            'If (menuItem IsNot Nothing) Then
            '    With menuItem
            '        .Text = WebWordUtility.GetWord(10165)
            '        .PresenceCategory = "1"
            '        .PresenceDetail = "0"
            '        AddHandler .Click, AddressOf SalesCancelButton_Click
            '        If checkFlg Then
            '            .OnClientClick = "return cancelCheck(1);"
            '        Else
            '            .OnClientClick = "return startServerCallback();"
            '        End If
            '        .Visible = VisiFlg(C_SALES_CANCEL)
            '    End With
            'End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END
            '営業活動開始
            menuItem = .GetMenuItem(C_BUSINESS_START)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10166)
                    .PresenceCategory = "1"
                    .PresenceDetail = "1"
                    AddHandler .Click, AddressOf BusinessStartButton_Click
                    .Visible = VisiFlg(C_BUSINESS_START)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            '営業活動キャンセル
            menuItem = .GetMenuItem(C_BUSINESS_CANCEL)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10167)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf BusinessCancelButton_Click
                    If checkFlg Then
                        .OnClientClick = "return cancelCheck(2);"
                    Else
                        .OnClientClick = "return startServerCallback();"
                    End If
                    .Visible = VisiFlg(C_BUSINESS_CANCEL)

                End With
            End If
            '一時対応開始
            menuItem = .GetMenuItem(C_CORRESPOND_START)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10168)
                    .PresenceCategory = "2"
                    .PresenceDetail = "1"
                    AddHandler .Click, AddressOf TempCorrespondStartButton_Click
                    .Visible = VisiFlg(C_CORRESPOND_START)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            '一時対応キャンセル
            menuItem = .GetMenuItem(C_CORRESPOND_END)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10169)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf TempCorrespondEndButton_Click
                    .Visible = VisiFlg(C_CORRESPOND_END)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            '商談終了
            menuItem = .GetMenuItem(C_SALES_END)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10188)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf salesEndButton_Click
                    .Visible = VisiFlg(C_SALES_END)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
            '商談中断
            menuItem = .GetMenuItem(C_SALES_STOP)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10196)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf salesStopButton_Click
                    .Visible = VisiFlg(C_SALES_STOP)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
            '納車作業開始
            menuItem = .GetMenuItem(C_DELIVERY_START)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10199)
                    .PresenceCategory = "2"
                    .PresenceDetail = "2"
                    AddHandler .Click, AddressOf DeliveryStartButton_Click
                    .Visible = VisiFlg(C_DELIVERY_START)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If

            '納車作業終了
            menuItem = .GetMenuItem(C_DELIVERY_END)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10200)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf DeliveryEndButton_Click
                    .Visible = VisiFlg(C_DELIVERY_END)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If

            '納車作業開始(一時対応)
            menuItem = .GetMenuItem(C_DELIVERYCORRESPOND_START)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10201)
                    .PresenceCategory = "2"
                    .PresenceDetail = "3"
                    AddHandler .Click, AddressOf DeliveryCorrespondStartButton_Click
                    .Visible = VisiFlg(C_DELIVERYCORRESPOND_START)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If

            '納車作業終了(一時対応)
            menuItem = .GetMenuItem(C_DELIVERYCORRESPOND_END)
            If (menuItem IsNot Nothing) Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10202)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf DeliveryCorrespondEndButton_Click
                    .Visible = VisiFlg(C_DELIVERYCORRESPOND_END)
                    .OnClientClick = "return startServerCallback();"
                End With
            End If
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

        End With

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetContextMenu End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 来店実績連番取得
    ''' </summary>
    ''' <remarks>来店実績連番を取得する</remarks>
    Private Sub GetVisitSeq()

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisitSeq Start")
        'ログ出力 End *****************************************************************************

        '顧客情報が有る場合、それを元に来店実績連番を紐付けに行く
        If IsSession(SESSION_KEY_CRCUSTID) Then

            '活動先顧客コード
            Dim CrcustId As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            '顧客種別
            Dim CstKind As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()
            '来店実績連番
            Dim VisitSeq As Long

            Using param As New SC3080201SalesStartDataTable
                Dim dr As SC3080201SalesStartRow = param.NewSC3080201SalesStartRow()
                dr.CUSTSEGMENT = CstKind
                dr.CRCUSTID = CrcustId
                param.AddSC3080201SalesStartRow(dr)
                Dim bizClass As New SC3080201BusinessLogic
                VisitSeq = bizClass.GetVisitSeq(param)
            End Using

            '取得できた場合(0以外)セッションに設定
            If VisitSeq <> 0 Then
                SetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, VisitSeq)
            Else
                RemoveValue(ScreenPos.Current, SESSION_KEY_VISITSEQ)
            End If

        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisitSeq End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 来店実績取得
    ''' </summary>
    ''' <remarks>来店実績より来店人数、仮氏名、車両登録Noを取得</remarks>
    Private Sub GetVisitResult()

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisitResult Start")
        'ログ出力 End *****************************************************************************

        If IsSession(SESSION_KEY_VISITSEQ) Then
            Dim resultTable As SC3080201VisitResultDataTable
            Using param As New SC3080201VisitSeqDataTable
                Dim dr As SC3080201VisitSeqRow = param.NewSC3080201VisitSeqRow()
                Dim visitseq As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, False).ToString()
                dr.VISITSEQ = CLng(visitseq)
                param.AddSC3080201VisitSeqRow(dr)
                '来店実績取得
                Dim biz As New SC3080201BusinessLogic
                resultTable = biz.GetVclregNo(param)
            End Using

            Dim resultRow As SC3080201VisitResultRow

            resultRow = CType(resultTable.Rows(0), SC3080201VisitResultRow)

            If Not resultRow.IsVCLREGNONull Then
                SetValue(ScreenPos.Current, SESSION_KEY_VCLREGNO, resultRow.VCLREGNO)
            End If

            If Not resultRow.IsTENTATIVENAMENull And Not IsSession(SESSION_KEY_CRCUSTID) Then
                SetValue(ScreenPos.Current, SESSION_KEY_TENTATIVENAME, resultRow.TENTATIVENAME)
            End If

            If Not resultRow.IsVISITPERSONNUMNull And Not IsSession(SESSION_KEY_CRCUSTID) Then
                SetValue(ScreenPos.Current, SESSION_KEY_WALKINNUM, resultRow.VISITPERSONNUM)
            End If

            '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
            If Not (resultRow.IsTELNONull OrElse String.IsNullOrWhiteSpace(resultRow.TELNO)) And Not IsSession(SESSION_KEY_TELNO) Then
                SetValue(ScreenPos.Current, SESSION_KEY_TELNO, resultRow.TELNO)
            End If
            '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START

        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisitResult End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 文言を設定
    ''' </summary>
    ''' <remarks>文言を取得</remarks>
    Private Sub SetWord()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetWord Start")
        'ログ出力 End *****************************************************************************

        Me.ErrWord4.Value = WebWordUtility.GetWord(10912)
        Me.ErrWord5.Value = WebWordUtility.GetWord(10913)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetWord End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 表示フラグ取得
    ''' </summary>
    ''' <returns>表示フラグ</returns>
    ''' <remarks>各コンテキストメニューの表示設定を取得</remarks>
    Private Function GetVisiFlg() As Boolean()

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisiFlg Start")
        'ログ出力 End *****************************************************************************

        Dim SalesStart As Boolean = False
        'Dim SalesCancel As Boolean = False
        Dim BusinessStart As Boolean = False
        Dim BusinessCancel As Boolean = False
        Dim CorrespondStart As Boolean = False
        Dim CorrespondEnd As Boolean = False
        ' 2012/02/15 TCS 相田 【SALES_2】 START
        Dim SalesEnd As Boolean = False
        ' 2012/02/15 TCS 相田 【SALES_2】 END

        ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        Dim SalesStop As Boolean = False
        ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
        Dim DeliveryStart As Boolean = False
        Dim DeliveryEnd As Boolean = False
        Dim DeliveryCorrespondStart As Boolean = False
        Dim DeliveryCorrespondEnd As Boolean = False
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        Dim Account As String = StaffContext.Current.Account
        Dim Dlrcd As String = StaffContext.Current.DlrCD
        Dim Brncd As String = StaffContext.Current.BrnCD

        If String.Equals(StaffContext.Current.OpeCD, Operation.SSF) Then
            If String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0") Then
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                '商談中の場合、商談終了
                'SalesCancel = True
                SalesEnd = True
                ' 2012/02/15 TCS 相田 【SALES_2】 END

                ' 2012/08/13 TCS 安田 商談中断メニューの追加START
                '商談中の場合、商談中断
                SalesStop = True
                ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

            ElseIf String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1") Then
                '営業活動中の場合、営業活動中キャンセルのみ
                BusinessCancel = True
            ElseIf String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1") Then
                '一時対応中の場合、一時対応中キャンセル
                CorrespondEnd = True

                ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
                '一時対応中の場合、商談中断
                SalesStop = True
                ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
            ElseIf String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2") Then
                '納車作業中の場合、納車作業終了
                DeliveryEnd = True
            ElseIf String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3") Then
                '納車作業中(一時対応)の場合、納車作業(一時対応)終了
                DeliveryCorrespondEnd = True
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

            Else
                Dim SalesStaff As String
                If IsSession(SESSION_KEY_SALESSTAFFCD) Then
                    '顧客担当セールススタッフ
                    SalesStaff = GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False).ToString()
                Else
                    SalesStaff = ""
                End If
                '顧客の情報が有るか？
                If IsSession(SESSION_KEY_CRCUSTID) Then
                    '顧客の情報が有る場合
                    '活動の情報が有るか？
                    If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                        '表示している活動の状態を取得
                        Dim rsltDt As SC3080202GetStatusToDataTable = GetFollowupboxStatus()
                        ' 2012/02/15 TCS 相田 【SALES_2】START
                        If rsltDt.Rows.Count > 0 Then
                            ' 2012/02/15 TCS 相田 【SALES_2】END

                            If rsltDt(0).ENABLEFLG Then
                                '継続中の場合
                                '自分が顧客担当か？
                                If String.Equals(SalesStaff, Account) Then
                                    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                                    If Not rsltDt(0).IsCONTRACTNONull AndAlso Not String.IsNullOrEmpty(rsltDt(0).CONTRACTNO) Then
                                        '受注後工程フォローの場合
                                        If IsSession(SESSION_KEY_VISITSEQ) Then
                                            '未対応来店客一覧からの場合、営業活動開始は表示しない
                                            DeliveryStart = True
                                        Else
                                            DeliveryStart = True
                                            BusinessStart = True
                                        End If
                                    Else
                                        '未対応来店客一覧からの場合、営業活動開始は表示しない
                                        If IsSession(SESSION_KEY_VISITSEQ) Then
                                            SalesStart = True
                                        Else
                                            SalesStart = True
                                            BusinessStart = True
                                        End If
                                    End If
                                    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                                Else
                                    '自分が活動担当か
                                    '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）Start
                                    Dim SalesStaffCD = rsltDt(rsltDt.Rows.Count - 1).ACCOUNT_PLAN
                                    For i = 0 To rsltDt.Rows.Count - 1
                                        If String.Equals(rsltDt(i).REQCATEGORY, " ") Then
                                            SalesStaffCD = rsltDt(i).ACCOUNT_PLAN
                                        End If
                                    Next
                                    If String.Equals(SalesStaffCD, Account) Then
                                        '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）End
                                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                                        If Not rsltDt(0).IsCONTRACTNONull AndAlso Not String.IsNullOrEmpty(rsltDt(0).CONTRACTNO) Then
                                            '受注後工程フォローの場合
                                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                                '未対応来店客一覧からの場合、営業活動開始は表示しない
                                                DeliveryStart = True
                                            Else
                                                DeliveryStart = True
                                                BusinessStart = True
                                            End If
                                        Else
                                            '未対応来店客一覧からの場合、営業活動開始は表示しない
                                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                                SalesStart = True
                                            Else
                                                SalesStart = True
                                                BusinessStart = True
                                            End If
                                        End If
                                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                                    Else
                                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                                        If Not rsltDt(0).IsCONTRACTNONull AndAlso Not String.IsNullOrEmpty(rsltDt(0).CONTRACTNO) Then
                                            '受注後工程フォローの場合
                                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                                '未対応来店客一覧からの場合、納車作業(一時対応)を表示
                                                DeliveryCorrespondStart = True
                                            End If
                                        Else
                                            '未対応来店客一覧からの場合、一時対応を表示
                                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                                CorrespondStart = True
                                            End If
                                        End If
                                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                                    End If
                                End If
                            Else
                                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                                '完了済の場合
                                If Not SC3080201BusinessLogic.IsExistsNotCompleteAction(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString(), Account) Then
                                    '他に自分が担当している継続中の活動が無い場合
                                    '自分が顧客担当か、もしくは他の人が担当している活動すらない場合
                                    If String.Equals(SalesStaff, Account) And Not (SC3080201BusinessLogic.IsExistsNotCompleteAction(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString(), "")) Then
                                        '未対応来店客一覧からの場合、営業活動開始は表示しない
                                        If IsSession(SESSION_KEY_VISITSEQ) Then
                                            SalesStart = True
                                        Else
                                            SalesStart = True
                                            BusinessStart = True
                                        End If
                                    End If
                                End If
                                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                                ' 2012/02/15 TCS 相田 【SALES_2】 START
                                If IsSession(SESSION_KEY_ORDER_NO) Then
                                    '受注Noが存在する場合
                                    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                                    Dim fllwupboxSeqno As Decimal = Decimal.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)

                                    If ActivityInfoBusinessLogic.IsExistsUnexecutedAfterOdrAct(fllwupboxSeqno) Then
                                        '未活動の受注後活動が存在する場合
                                        '振当待ち,入金待ち,納車待ち,納車済みの場合
                                        If IsSession(SESSION_KEY_VISITSEQ) Then
                                            '未対応来店客一覧からの場合、営業活動開始は表示しない
                                            SalesStart = True
                                        Else
                                            SalesStart = True
                                            BusinessStart = True
                                        End If
                                    End If
                                    '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                                End If
                                ' 2012/02/15 TCS 相田 【SALES_2】 END
                            End If
                        Else
                            ' 2012/02/15 TCS 相田 【SALES_2】START
                            SalesStart = True
                            BusinessStart = True
                            ' 2012/02/15 TCS 相田 【SALES_2】END
                        End If

                    Else
                        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                        '自身が顧客担当か？
                        If String.Equals(SalesStaff, Account) Or SC3080201BusinessLogic.IsExistsNotCompleteAction(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString(), Account) Then
                            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                            '活動が無い場合(顧客の新規登録直後、活動が無い顧客を検索)
                            '未対応来店客一覧からの場合、営業活動開始は表示しない
                            '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 START
                                'If Not IsBookedAfter() Then
                                If String.IsNullOrEmpty(IsBookedAfter()) Then
                                    SalesStart = True
                                Else
                                    DeliveryStart = True
                                End If
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 END
                            Else
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 START
                                'If Not IsBookedAfter() Then
                                If String.IsNullOrEmpty(IsBookedAfter()) Then
                                    SalesStart = True
                                    BusinessStart = True
                                Else
                                    DeliveryStart = True
                                    BusinessStart = True
                                End If
                            End If
                            '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 END
                        Else
                            '未対応来店客一覧から遷移した場合
                            '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                            If IsSession(SESSION_KEY_VISITSEQ) Then
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 START
                                'If Not IsBookedAfter() Then
                                If String.IsNullOrEmpty(IsBookedAfter()) Then
                                    CorrespondStart = True
                                Else
                                    DeliveryCorrespondStart = True
                                End If
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追加したリンク対応 END
                            End If
                            '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
                        End If
                    End If
                End If
            End If
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetVisiFlg End")
        'ログ出力 End *****************************************************************************

        ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        Return {False, SalesStart, SalesEnd, BusinessStart, BusinessCancel, CorrespondStart, CorrespondEnd, SalesStop, DeliveryStart, DeliveryEnd, DeliveryCorrespondStart, DeliveryCorrespondEnd}
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
        ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

    End Function

    ''' <summary>
    ''' セッション情報の設定
    ''' </summary>
    ''' <remarks>セッション情報の設定を行う</remarks>
    Private Sub GetSession()

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetSession Start")
        'ログ出力 End *****************************************************************************

        '商談中で、活動の情報がある場合
        If IsSales() And IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            '商談中活動情報が無い場合
            If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
                '商談中に他画面から遷移してきた場合として、商談中の活動情報を設定する
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, StaffContext.Current.BrnCD)
            End If
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetSession End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2012/01/24 TCS 河原 【SALES_1B】 END
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' ステータスの更新
    ''' </summary>
    ''' <param name="status">ステータス</param>
    ''' <remarks></remarks>
    Private Sub updateStatus(ByVal status As Integer)
        Select Case status
            Case C_SALES_START
                StaffContext.Current.UpdatePresence("2", "0")
                SalesStart(C_SALES_START)
            Case C_SALES_END
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_SALES_END)
            Case C_BUSINESS_START
                StaffContext.Current.UpdatePresence("1", "1")
                SalesStart(C_BUSINESS_START)
            Case C_BUSINESS_CANCEL
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_BUSINESS_CANCEL)
            Case C_CORRESPOND_START
                StaffContext.Current.UpdatePresence("2", "1")
                SalesStart(C_CORRESPOND_START)
            Case C_CORRESPOND_END
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_CORRESPOND_END)
                ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
            Case C_SALES_STOP
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_SALES_STOP)
                ' 2012/08/13 TCS 安田 商談中断メニューの追加 【SALES_3】 END

                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
            Case C_DELIVERY_START
                StaffContext.Current.UpdatePresence("2", "2")
                SalesStart(C_DELIVERY_START)
            Case C_DELIVERY_END
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_DELIVERY_END)
            Case C_DELIVERYCORRESPOND_START
                StaffContext.Current.UpdatePresence("2", "3")
                SalesStart(C_DELIVERYCORRESPOND_START)
            Case C_DELIVERYCORRESPOND_END
                StaffContext.Current.UpdatePresence("1", "0")
                SalesStart(C_DELIVERYCORRESPOND_END)

                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

        End Select
    End Sub

    ''' <summary>
    ''' 受注Noをセット
    ''' </summary>
    ''' <param name="folloupseq">FollowupBox内連番</param>
    ''' <remarks>受注No・受注後フラグを取得</remarks>
    Private Sub SetOrderInfo(ByVal folloupseq As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetOrderInfo Start")
        'ログ出力 End *****************************************************************************

        If Not String.IsNullOrEmpty(folloupseq) Then
            Dim fllwupbox_dlrcd As String = StaffContext.Current.DlrCD
            Dim fllwupbox_strcd As String = ""
            Dim fllwupbox_seqno As Long = Long.Parse(folloupseq, CultureInfo.CurrentCulture)

            Dim bizClass As New SC3080201BusinessLogic
            Dim contractNo As String = bizClass.GetContractNo(fllwupbox_dlrcd, fllwupbox_strcd, fllwupbox_seqno)
            '受注Noセット
            If Not String.IsNullOrEmpty(contractNo) Then
                SetValue(ScreenPos.Current, SESSION_KEY_ORDER_NO, contractNo)
                '受注後フラグをセット
                Dim sater As String = bizClass.CountFllwupboxRslt(fllwupbox_dlrcd, fllwupbox_strcd, fllwupbox_seqno)
                SetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, sater)
            End If
        End If

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetOrderInfo End")
        'ログ出力 End *****************************************************************************
    End Sub

    ''' <summary>
    ''' 未取引客IDをセット
    ''' </summary>
    ''' <param name="custId">顧客ID</param>
    ''' <remarks>未取引客IDを取得</remarks>
    Private Sub SetCustId(ByVal custId As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetCustId Start")
        'ログ出力 End *****************************************************************************

        '未取引客ユーザID・自社客ID取得
        Dim custKind As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()
        Dim bizClass As New SC3080201BusinessLogic
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        Dim dataSet As SC3080201DataSet.SC3080201CustchrgDataTable = bizClass.GetNewCstId(custId)
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        If dataSet.Rows.Count > 0 Then
            Dim newCstId As String = CStr(dataSet(0).Item("CSTID"))
            Dim orgCstId As String = CStr(dataSet(0).Item("ORIGINALID"))

            If ORGCUSTFLG.Equals(custKind) Then
                '自社客の場合
                '未取引客IDをセット
                If Not String.IsNullOrEmpty(newCstId) Then
                    SetValue(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID, newCstId)
                End If
            Else
                '未取引客の場合
                If orgCstId.Trim().Length > 0 Then
                    SetValue(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID, newCstId)
                    '顧客種別←自社客
                    SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, ORGCUSTFLG)
                    '顧客ID←自社客個人ID
                    SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, orgCstId)
                End If

            End If
        End If

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetCustId End")
        'ログ出力 End *****************************************************************************

    End Sub
    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2013/03/06 TCS 河原 GL0874 START
    ''' <summary>
    ''' 契約キャンセル後の商談終了・中断判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetContractCancelStartFlg()

        Logger.Info("SetContractCancelStartFlg Start")

        '来店実績からの遷移
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_STATUS) Then
            Me.ContractCancelStartFlg.Value = "1"
        End If

        '未対応来店客一覧からの遷移
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VISITSEQ) Then
            Dim visitSeq As Long = CLng(Me.GetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, False))
            Dim dataSet As SC3080201DataSet.SC3080201VisitFllwSeqDataTable = SC3080201BusinessLogic.GetVisitFllwSeq(visitSeq)

            If dataSet.Count > 0 Then
                Me.ContractCancelStartFlg.Value = "1"
                Dim retrunDr As SC3080201DataSet.SC3080201VisitFllwSeqRow = CType(dataSet.Rows(0), SC3080201VisitFllwSeqRow)

                If Not retrunDr.IsFLLWUPBOX_STRCDNull Then
                    Me.ContractCancelFllwStrcd.Value = retrunDr.FLLWUPBOX_STRCD
                    Me.ContractCancelFllwSeqno.Value = CType(retrunDr.FLLWUPBOX_SEQNO, String)
                End If

            End If
        End If

        Logger.Info("SetContractCancelStartFlg End")

    End Sub

    Private Sub SetContractCancelStartFlgAfter()

        Logger.Info("SetContractCancelStartFlgAfter Start")

        Dim fllwupBoxStrcd As String = Nothing
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD) Then
            fllwupBoxStrcd = StaffContext.Current.BrnCD
        End If

        Dim fllwupBoxSeqno As String = Nothing
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX) Then
            fllwupBoxSeqno = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()
        End If

        If String.Equals(Me.ContractCancelStartFlg.Value, "1") AndAlso (fllwupBoxStrcd <> Me.ContractCancelFllwStrcd.Value Or fllwupBoxSeqno <> Me.ContractCancelFllwSeqno.Value) Then
            Me.ContractCancelStartFlg.Value = "0"
        End If

        Logger.Info("SetContractCancelStartFlgAfter End")

    End Sub
    '2013/03/06 TCS 河原 GL0874 END

    '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 START
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' 受注後工程として活動ができるか判定する
    ''' </summary>
    ''' <returns>受注後工程:True、受注後工程以外:False</returns>
    ''' <remarks>受注後工程として活動ができるか判定する</remarks>
    Private Function IsBookedAfter() As String
        '    Private Function IsBookedAfter() As Boolean
        '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 END
        Logger.Info("getFllwupbox Start")

        '2014/04/02 TCS 河原 性能改善 Start
        If BookedAfterCheckFlg = False Then
            BookedAfterCheckFlg = True

            ' 引数用DataRow宣言
            Dim getActivityListFromRow As SC3080202DataSet.SC3080202GetActivityListFromRow
            Dim fllwupboxSeqno As Nullable(Of Long)
            Dim fllwupboxStrcd As String = String.Empty
            Dim dlrCd As String = StaffContext.Current.DlrCD
            Dim strCd As String = StaffContext.Current.BrnCD

            '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 START
            'Dim bookedAfterFlg As Boolean = False
            'Dim retContractNo As String = String.Empty
            '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 END

            Using getActivityListFromDataTable As New SC3080202DataSet.SC3080202GetActivityListFromDataTable
                getActivityListFromRow = getActivityListFromDataTable.NewSC3080202GetActivityListFromRow
                getActivityListFromRow.CUSTFLG = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                getActivityListFromRow.INSDID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                getActivityListFromRow.DLRCD = dlrCd
                getActivityListFromRow.STRCD = strCd

                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 START
                If ContainsKey(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES) Then
                    getActivityListFromRow.SALESFLLWSTRCD = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, False), String)
                Else
                    getActivityListFromRow.SALESFLLWSTRCD = Nothing
                End If
                If ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
                    getActivityListFromRow.SALESFLLWSEQNO = CLng(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String))
                Else
                    getActivityListFromRow.SALESFLLWSEQNO = Nothing
                End If
                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 END

                '未取引客ID設定
                If ContainsKey(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID) Then
                    getActivityListFromRow.NEWCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID, False), String)
                End If
                getActivityListFromDataTable.Rows.Add(getActivityListFromRow)

                '活動情報取得
                Dim getActivityListToDataTable As SC3080202DataSet.SC3080202GetActivityListToDataTable
                getActivityListToDataTable = SC3080202BusinessLogic.GetActivityList(getActivityListFromDataTable)

                Dim fllwupbox_dlrcd As String = Nothing
                Dim fllwupbox_strcd As String = Nothing
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                Dim fllwupbox_seqno As Decimal = Nothing
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

                ' 最新活動を取得
                If IsNothing(fllwupboxSeqno) Then
                    ' 最新の活動を選択する
                    For Each resentRow In getActivityListToDataTable
                        If resentRow.ENABLEFLG = True Then
                            fllwupbox_dlrcd = dlrCd
                            fllwupbox_seqno = resentRow.FLLWUPBOX_SEQNO
                            fllwupbox_strcd = resentRow.STRCD
                            Exit For
                        End If
                    Next
                End If

                '有効な活動がある場合
                If Not String.IsNullOrEmpty(fllwupbox_strcd) Then
                    Dim bizClass As New SC3080201BusinessLogic

                    '活動情報取得
                    Dim contractNo As String = bizClass.GetContractNo(fllwupbox_dlrcd, fllwupbox_strcd, fllwupbox_seqno)
                    If Not String.IsNullOrEmpty(contractNo) Then
                        Using param As New SC3080202GetStatusFromDataTable
                            Dim dr As SC3080202GetStatusFromRow = param.NewSC3080202GetStatusFromRow()
                            dr.DLRCD = fllwupbox_dlrcd
                            dr.STRCD = fllwupbox_strcd
                            dr.FLLWUPBOX_SEQNO = fllwupbox_seqno
                            param.AddSC3080202GetStatusFromRow(dr)
                            Dim rsltDt As SC3080202GetStatusToDataTable = SC3080202BusinessLogic.GetFollowupboxStatus(param)
                            If rsltDt(0).ENABLEFLG Then
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 START
                                'bookedAfterFlg = True
                                retContractNo = contractNo
                                '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 END
                            End If
                        End Using
                    End If
                End If
            End Using
            Logger.Info("getFllwupbox End")
        End If
        '2014/04/02 TCS 河原 性能改善 End
        '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 START
        'Return bookedAfterFlg
        Return retContractNo
        '2013/11/06 TCS 山田 MOD i-CROP再構築後の新車納車システムに追隠ｵたリンク対応 END
    End Function
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    ''' <summary>
    ''' ページ間連携処理を行うイベントハンドラを登録する
    ''' </summary>
    Private Sub RegisterPageInterfaceHandlers()
        '商談画面のイベントをハンドル
        'イベント(新規活動開始)
        AddHandler CType(Sc3080202Page, ISC3080202Control).CreateFollow, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080203Page.Visible Then
                    CType(Sc3080203Page, ISC3080203Control).UpdateActivityResult()
                End If
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                If Sc3080216Page.Visible Then
                    CType(Sc3080216Page, ISC3080203Control).UpdateActivityResult()
                End If
                ' 2012/02/15 TCS 相田 【SALES_2】 END
            End Sub
        'イベント(希望車種変更)
        AddHandler CType(Sc3080202Page, ISC3080202Control).ChangeSelectedSeries, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080203Page.Visible Then
                    CType(Sc3080203Page, ISC3080203Control).UpdateActivityResult()
                End If
            End Sub
        'イベント(活動変更)
        AddHandler CType(Sc3080202Page, ISC3080202Control).ChangeFollow, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080203Page.Visible Then
                    CType(Sc3080203Page, ISC3080203Control).ChangeFollow()
                End If
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                If Sc3080216Page.Visible Then
                    CType(Sc3080216Page, ISC3080203Control).ChangeFollow()
                End If
                ' 2012/02/15 TCS 相田 【SALES_2】 END
            End Sub


        '活動登録のイベントハンドル
        'イベント(継続で活動登録)
        AddHandler CType(Sc3080203Page, ISC3080203Control).ContinueActivity, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080202Page.Visible Then
                    scNscAllBoxContentsArea.CssClass = "page2"
                    PageNumberClassHidden.Value = "page2"
                    CType(Sc3080202Page, ISC3080202Control).RefreshSalesCondition()
                    CType(Sc3080201Page, ISC3080201Control).RegistActivityAfter()
                End If
            End Sub
        'イベント(完了で活動登録)
        AddHandler CType(Sc3080203Page, ISC3080203Control).SuccessActivity, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080202Page.Visible Then
                    scNscAllBoxContentsArea.CssClass = "page2"
                    PageNumberClassHidden.Value = "page2"
                    CType(Sc3080202Page, ISC3080202Control).RefreshSalesCondition()
                    CType(Sc3080201Page, ISC3080201Control).RegistActivityAfter()
                End If
            End Sub

        ' 2012/02/15 TCS 相田 【SALES_2】 START
        '活動登録のイベントハンドル
        'イベント(完了で活動登録)
        AddHandler CType(Sc3080216Page, ISC3080203Control).SuccessActivity, _
            Sub(sender As Object, e As EventArgs)
                If Sc3080202Page.Visible Then
                    scNscAllBoxContentsArea.CssClass = "page2"
                    PageNumberClassHidden.Value = "page2"
                    CType(Sc3080202Page, ISC3080202Control).RefreshSalesCondition()
                    CType(Sc3080201Page, ISC3080201Control).RegistActivityAfter()
                End If
            End Sub
        ' 2012/02/15 TCS 相田 【SALES_2】 END
    End Sub

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' コンテキストメニューのイベント登録
    ''' </summary>
    ''' <remarks>イベント登録はPageInitにて行う</remarks>
    Private Sub RegisterMenuEventHandler()

        Dim menuItem As CommonMasterContextMenuItem
        With Me.localCommonMaster.ContextMenu

            '商談開始ボタン
            menuItem = .GetMenuItem(C_SALES_START)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10164)
                    .PresenceCategory = "2"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf SalesStartButton_Click
                End With
            End If
            '営業活動開始
            menuItem = .GetMenuItem(C_BUSINESS_START)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10166)
                    .PresenceCategory = "1"
                    .PresenceDetail = "1"
                    AddHandler .Click, AddressOf BusinessStartButton_Click
                End With
            End If
            '営業活動キャンセル
            menuItem = .GetMenuItem(C_BUSINESS_CANCEL)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10167)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf BusinessCancelButton_Click
                End With
            End If
            '一時対応開始
            menuItem = .GetMenuItem(C_CORRESPOND_START)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10168)
                    .PresenceCategory = "2"
                    .PresenceDetail = "1"
                    AddHandler .Click, AddressOf TempCorrespondStartButton_Click
                End With
            End If
            '一時対応キャンセル
            menuItem = .GetMenuItem(C_CORRESPOND_END)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10169)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf TempCorrespondEndButton_Click
                End With
            End If
            '商談終了
            menuItem = .GetMenuItem(C_SALES_END)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10188)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf salesEndButton_Click
                End With
            End If
            '商談中断
            menuItem = .GetMenuItem(C_SALES_STOP)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10196)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf salesStopButton_Click
                End With
            End If
            '納車作業開始
            menuItem = .GetMenuItem(C_DELIVERY_START)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10199)
                    .PresenceCategory = "2"
                    .PresenceDetail = "2"
                    AddHandler .Click, AddressOf DeliveryStartButton_Click
                End With
            End If
            '納車作業終了
            menuItem = .GetMenuItem(C_DELIVERY_END)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10200)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf DeliveryEndButton_Click
                End With
            End If
            '納車作業開始(一時対応)
            menuItem = .GetMenuItem(C_DELIVERYCORRESPOND_START)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10201)
                    .PresenceCategory = "2"
                    .PresenceDetail = "3"
                    AddHandler .Click, AddressOf DeliveryCorrespondStartButton_Click
                End With
            End If
            '納車作業終了(一時対応)
            menuItem = .GetMenuItem(C_DELIVERYCORRESPOND_END)
            If Not menuItem Is Nothing Then
                With menuItem
                    .Text = WebWordUtility.GetWord(10202)
                    .PresenceCategory = "1"
                    .PresenceDetail = "0"
                    AddHandler .Click, AddressOf DeliveryCorrespondEndButton_Click
                End With
            End If
        End With

    End Sub
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

#End Region

#Region " 各ページの制御 "
    ''' <summary>
    ''' 各ページのコントロールを制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetPageControls(ByVal crcustid As String, ByVal folloupseq As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetPageControls Start")
        'ログ出力 End *****************************************************************************

        'ページ数の初期値設定
        CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)

        '表示／非表示初期化
        Sc3080201Page.Visible = True
        Sc3080202Page.Visible = True
        Sc3080203Page.Visible = False
        ' 2012/02/15 TCS 相田 【SALES_2】 START
        Sc3080216Page.Visible = False
        pagePlaceholder.Visible = False
        ' 2012/02/15 TCS 相田 【SALES_2】 END

        If String.IsNullOrEmpty(crcustid) And String.IsNullOrEmpty(folloupseq) Then
            '顧客なし・活動なし
            Sc3080202Page.Visible = False
            Sc3080203Page.Visible = False
            CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 2).ToString(CultureInfo.CurrentCulture)
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            Sc3080216Page.Visible = False
            pagePlaceholder.Visible = False
            ' 2012/02/15 TCS 相田 【SALES_2】 END
        End If

        If Not Me.IsPostBack Then
            SetActivityControl(crcustid, folloupseq)
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetPageControls End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 活動登録画面の制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetActivityControl(ByVal crcustid As String, ByVal folloupseq As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetActivityControl Start")
        'ログ出力 End *****************************************************************************

        If Not String.IsNullOrEmpty(crcustid) And Not String.IsNullOrEmpty(folloupseq) Then

            '活動状態取得
            Dim resultTable As SC3080202GetStatusToDataTable = GetFollowupboxStatus()
            Dim status As Boolean = True
            'Dim count As Integer = resultTable.Where(Function(row) row.ENABLEFLG = True).Count

            '2012/01/24 TCS 河原 【SALES_1B】 START
            'Dim enableCount As Integer = (From n In resultTable Where n.ENABLEFLG = True).Count
            Dim enableCount As Integer = 0

            For Each enableCountRow As SC3080202GetStatusToRow In resultTable
                If enableCountRow.ENABLEFLG = True Then
                    enableCount = enableCount + 1
                End If
            Next
            '2012/01/24 TCS 河原 【SALES_1B】 END

            'Follow-up Boxのレコードありで活動完了済の場合
            If resultTable.Count > 0 And enableCount <= 0 Then
                status = False
            End If

            If Not status Then
                '完了している場合は活動登録不可
                Sc3080203Page.Visible = False
                CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                Sc3080216Page.Visible = False
                pagePlaceholder.Visible = False
                ' 2012/02/15 TCS 相田 【SALES_2】 END
            End If

            '新規活動中フラグ
            JavaScriptUtility.RegisterStartupScript(Me, "SC3080201.newActivityFlg = " _
                                                    & (resultTable.Count = 0).ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.InvariantCulture) & ";" _
                                                    & "SC3080201.redirectMessage = '" _
                                                    & HttpUtility.JavaScriptStringEncode(WebWordUtility.GetWord(20908)) & "';", _
                                                    "newActivityFlg", _
                                                    True)
        Else

            Dim staffCd As String = String.Empty

            If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD) Then
                '顧客担当セールススタッフ
                staffCd = GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False).ToString()
            End If

            '顧客あり、活動なしで顧客担当でない場合、３ページ目は非表示
            If Not String.IsNullOrEmpty(crcustid) _
                And Not StaffContext.Current.Account.Equals(staffCd) Then
                Sc3080203Page.Visible = False
                CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                Sc3080216Page.Visible = False
                pagePlaceholder.Visible = False
                ' 2012/02/15 TCS 相田 【SALES_2】 END
            End If

            '新規活動中フラグ
            JavaScriptUtility.RegisterStartupScript(Me, "SC3080201.newActivityFlg = false;", _
                                                    "newActivityFlg", _
                                                    True)
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetActivityControl End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 画面表示時の初期ページ位置を設定
    ''' </summary>
    ''' <param name="crcustid"></param>
    ''' <param name="folloupseq"></param>
    ''' <remarks></remarks>
    Private Sub SetInitPagePosition(ByVal crcustid As String, ByVal folloupseq As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetInitPagePosition Start")
        'ログ出力 End *****************************************************************************

        'デフォルト表示ページCSSクラス設定
        Dim pageClass As String = "page1"

        If Not Me.IsPostBack Then
            '初期表示
            If Not String.IsNullOrEmpty(crcustid) And Not String.IsNullOrEmpty(folloupseq) Then
                '初期表示の場合は、商談を画面をデフォルトで開く
                pageClass = "page2"
            End If

            'HIDDENにも保存
            PageNumberClassHidden.Value = pageClass
        Else
            'ポストバック
            pageClass = PageNumberClassHidden.Value
        End If

        '初期表示の場合のデフォルト表示ページを設定
        scNscAllBoxContentsArea.CssClass = pageClass

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetInitPagePosition End")
        'ログ出力 End *****************************************************************************

    End Sub

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <returns>状態(true:生きている活動、false:完了)</returns>
    ''' <remarks></remarks>
    Private Function GetFollowupboxStatus() As SC3080202GetStatusToDataTable

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetFollowupboxStatus Start")
        'ログ出力 End *****************************************************************************

        If Me._activityStatus Is Nothing Then

            Dim fllwupboxSeqno As Long = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)
            Dim fllwupboxStrcd As String = StaffContext.Current.BrnCD
            Dim context As StaffContext = StaffContext.Current()
            Dim returnFlg As Boolean = False

            Using param As New SC3080202GetStatusFromDataTable
                Dim dr As SC3080202GetStatusFromRow = param.NewSC3080202GetStatusFromRow()
                dr.DLRCD = context.DlrCD
                dr.STRCD = fllwupboxStrcd
                dr.FLLWUPBOX_SEQNO = fllwupboxSeqno
                param.AddSC3080202GetStatusFromRow(dr)
                Me._activityStatus = SC3080202BusinessLogic.GetFollowupboxStatus(param)
            End Using
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetFollowupboxStatus End")
        'ログ出力 End *****************************************************************************

        Return Me._activityStatus

    End Function
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' 活動状況ステータス設定
    ''' </summary>
    ''' <remarks>活動状況ステータスを設定する</remarks>
    Private Sub SetStatus()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetStatus Start")
        'ログ出力 End *****************************************************************************

        ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 START
        Dim parmAfterOdrFlg As String = String.Empty
        '受注後工程利用フラグ取得
        parmAfterOdrFlg = SC3080201BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)
        ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 END 

        '権限コード取得
        Dim OpeCd As Integer = StaffContext.Current.OpeCD

        'セールススタッフ権限の場合　
        If OpeCd = Operation.SSF Then

            '商談中の活動と表示中の活動が異なる場合
            Dim fllwSeq As String = ""
            Dim fllwSeqSales As String = ""
            If IsSession(SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
                fllwSeq = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()
                fllwSeqSales = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False).ToString()
                If Not String.Equals(fllwSeq, fllwSeqSales) Then
                    Sc3080203Page.Visible = False
                    CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                    ' 2012/02/15 TCS 相田 【SALES_2】 START
                    Sc3080216Page.Visible = False
                    pagePlaceholder.Visible = False
                    ' 2012/02/15 TCS 相田 【SALES_2】 END
                End If
            End If

            Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
            Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

            '商談中・営業活動中・納車作業中であれば活動登録画面を作成
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
            If ((String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
                (String.Equals(PresenceCategory, "2") And (String.Equals(PresenceDetail, "0") Or
                                                           String.Equals(PresenceDetail, "2")))) And String.Equals(fllwSeq, fllwSeqSales) Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                ' 2012/02/15 TCS 相田 【SALES_2】 START
                '受注Noが存在すれば受注後フォロー画面を作成
                '2013/03/06 TCS 河原 GL0874 START
                Dim ContractFlg As String
                Using datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable = New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
                    datatableFrom.Rows.Add(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, fllwSeq)
                    ContractFlg = SC3080201BusinessLogic.GetContractFlg(datatableFrom)
                    ' 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応 START
                    'プロパティにセット
                    Sc3080203Page.ContractStatusFlg = ContractFlg
                    ' 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応 END
                End Using

                If String.Equals(ContractFlg, "2") Then
                    Me.ErrWord6.Value = WebWordUtility.GetWord(30935)
                    Me.SC3080201ContractCancelFlg.Value = "1"
                Else
                    Me.SC3080201ContractCancelFlg.Value = "0"
                End If
                '2013/03/06 TCS 河原 GL0874 END
                If IsSession(SESSION_KEY_ORDER_NO) Or String.Equals(ContractFlg, "2") Then
                    ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 MOD START
                    If String.Equals(parmAfterOdrFlg, "1") Then
                        '受注後工程を利用する場合、SC3080216を表示
                        Sc3080203Page.Visible = False
                        Sc3080216Page.Visible = True
                    ElseIf String.Equals(parmAfterOdrFlg, "0") Then
                        '受注後工程を利用しない場合、SC308203を表示
                        Sc3080203Page.Visible = True
                        Sc3080216Page.Visible = False
                    End If
                    ' 2015/12/02 TCS 鈴木 受注後工程蓋閉め対応 MOD END
                    pagePlaceholder.Visible = True
                Else
                    Sc3080203Page.Visible = True
                    Sc3080216Page.Visible = False
                    pagePlaceholder.Visible = True
                End If
                CustDetailPageCountHidden.Value = (PAGECOUNT_MAX).ToString(CultureInfo.CurrentCulture)
                ' 2012/02/15 TCS 相田 【SALES_2】 END
            Else
                If Sc3080203Page.Visible Or Sc3080216Page.Visible Then
                    Sc3080203Page.Visible = False
                    ' 2012/02/15 TCS 相田 【SALES_2】 START
                    Sc3080216Page.Visible = False
                    pagePlaceholder.Visible = False
                    ' 2012/02/15 TCS 相田 【SALES_2】 END
                    CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                    scNscAllBoxContentsArea.CssClass = "page2"
                    PageNumberClassHidden.Value = "page2"
                Else
                    If String.Equals(scNscAllBoxContentsArea.CssClass, "page3") Then
                        scNscAllBoxContentsArea.CssClass = "page2"
                        PageNumberClassHidden.Value = "page2"
                    End If
                End If
            End If


            If IsSession(SESSION_KEY_CRCUSTID) Then
                '自分が担当している活動があるか判定
                Dim ActFlg As Boolean
                Dim Dlrcd As String = StaffContext.Current.DlrCD
                Dim Strcd As String = StaffContext.Current.BrnCD
                Dim Account As String = StaffContext.Current.Account
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                ActFlg = SC3080201BusinessLogic.IsExistsNotCompleteAction(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString(),
                                                                          Account)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                '顧客担当でないかつ、自分が担当している継続中の活動無しの場合
                Dim SalesStaff As String
                If IsSession(SESSION_KEY_SALESSTAFFCD) Then
                    SalesStaff = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False).ToString()
                    '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）Start
                    If (Not String.Equals(SalesStaff, Account) And Not String.Equals(SalesStaff, " ")) And Not ActFlg Then
                        '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）End
                        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                        If String.Equals(PresenceCategory, "2") And (String.Equals(PresenceDetail, "1") Or
                                                                     String.Equals(PresenceDetail, "3")) Then
                            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

                            '一時対応中または納車作業中(一時対応中)であれば2枚目を表示
                            Sc3080202Page.Visible = True
                            Sc3080203Page.Visible = False
                            ' 2012/02/15 TCS 相田 【SALES_2】 START
                            Sc3080216Page.Visible = False
                            pagePlaceholder.Visible = False
                            ' 2012/02/15 TCS 相田 【SALES_2】 END
                            CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                            scNscAllBoxContentsArea.CssClass = "page1"
                            PageNumberClassHidden.Value = "page1"
                            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                            'チームリーダーの場合、マネージャと同じように振舞う(自分の部下だったら)
                        ElseIf StaffContext.Current.TeamLeader _
                            AndAlso ActivityInfoBusinessLogic.IsMyTeamMember(SalesStaff) Then '部下判定呼ぶ 
                            Sc3080202Page.Visible = True
                            CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                        Else
                            '一時対応中または納車作業中(一時対応中)でなければ1枚目を表示
                            Sc3080202Page.Visible = False
                            Sc3080203Page.Visible = False
                            ' 2012/02/15 TCS 相田 【SALES_2】 START
                            Sc3080216Page.Visible = False
                            pagePlaceholder.Visible = False
                            ' 2012/02/15 TCS 相田 【SALES_2】 END
                            CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 2).ToString(CultureInfo.CurrentCulture)
                            scNscAllBoxContentsArea.CssClass = "page1"
                            PageNumberClassHidden.Value = "page1"
                        End If
                    End If
                End If
            End If

            '商談・営業活動中・納車作業中であれば2枚目を表示
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
            If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
                (String.Equals(PresenceCategory, "2") And (String.Equals(PresenceDetail, "0") Or String.Equals(PresenceDetail, "2"))) Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                scNscAllBoxContentsArea.CssClass = "page2"
                PageNumberClassHidden.Value = "page2"
            End If

        End If

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '権限がマネージャ権限であれば2枚目を表示
        '(チームリーダーの場合、セールス/活動/一時対応 が担当外の場合)
        If OpeCd = Operation.SSM Or OpeCd = Operation.BM Then
            'If OpeCd = Operation.SSM OrElse OpeCd = Operation.BM _
            '    OrElse (StaffContext.Current.TeamLeader AndAlso Not Sc3080202Page.Visible) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            Sc3080202Page.Visible = True
            CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
        End If

        'Follow-upBoxSeqNoが有る場合、2枚目を作る
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            If Not Sc3080202Page.Visible Then
                Sc3080202Page.Visible = True
                CustDetailPageCountHidden.Value = (PAGECOUNT_MAX - 1).ToString(CultureInfo.CurrentCulture)
                scNscAllBoxContentsArea.CssClass = "page2"
                PageNumberClassHidden.Value = "page2"
            End If
        End If

        '表示対象ページが指定されている場合
        If IsSession(SESSION_KEY_DISPPAGE) Then
            Dim dispPage As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_DISPPAGE, False).ToString()
            Dim page As New StringBuilder
            page.Append("page")
            page.Append(dispPage)
            scNscAllBoxContentsArea.CssClass = page.ToString
            PageNumberClassHidden.Value = page.ToString
            If Not Me.IsPostBack Then
                Me.RemoveValue(ScreenPos.Current, SESSION_KEY_DISPPAGE)
            End If
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("SetStatus End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2012/01/24 TCS 河原 【SALES_1B】 END
#End Region

#Region " セッション取得・設定バイパス処理 "
    Public Function GetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl.GetValueBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function

    Public Sub SetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal value As Object) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl.SetValueBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Sub ShowMessageBoxBypass(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl.ShowMessageBoxBypass
        Me.ShowMessageBox(wordNo, wordParam)
    End Sub

    Public Function ContainsKeyBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl.ContainsKeyBypass
        Return Me.ContainsKey(pos, key)
    End Function

    Public Sub RemoveValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ICustomerDetailControl.RemoveValueBypass
        Me.RemoveValue(pos, key)
    End Sub

    Public Sub SetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object) Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl.SetValueCommonBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Function GetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl.GetValueCommonBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function

#End Region

#Region " フッター制御・ヘッダー制御 "

    '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
    '納車時説明画面遷移パラメータ
    ''' <summary>商談ID</summary>
    Private Const SESSION_KEY_NEWCAREXP_SALES_ID As String = "SalesId"
    ''' <summary>顧客ID</summary>
    Private Const SESSION_KEY_NEWCAREXP_CST_ID As String = "CstId"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_NEWCAREXP_CST_TYPE As String = "CstType"
    ''' <summary>顧客車両区分</summary>
    Private Const SESSION_KEY_NEWCAREXP_CST_VCL_TYPE As String = "CstVclType"
    '2018/06/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>契約ID</summary>
    Private Const SESSION_KEY_NEWCAREXP_CONTRACT_NO As String = "ContractNo"
    ''' <summary>販売店コード</summary>
    Private Const SESSION_KEY_NEWCAREXP_DEALER_CODE As String = "DealerCode"
    ''' <summary>店舗コード</summary>
    Private Const SESSION_KEY_NEWCAREXP_BRANCH_CODE As String = "BranchCode"
    '2018/06/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    '受注時説明画面遷移パラメータ
    ''' <summary>商談ID</summary>
    Private Const SESSION_KEY_BKGEXP_SALES_ID As String = "SalesId"
    ''' <summary>見積管理ID</summary>
    Private Const SESSION_KEY_BKGEXP_EST_ID As String = "EstimateId"
    ''' <summary>受注時説明表示モード</summary>
    Private Const SESSION_KEY_BKGEXP_VIEW_MODE As String = "SalesbookingDescriptionViewMode"
    ''' <summary>契約条件変更フラグ</summary>
    Private Const SESSION_KEY_BKGEXP_CHG_FLG As String = "ContractAskChgFlg"
    ''' <summary>顧客ID</summary>
    Private Const SESSION_KEY_BKGEXP_CST_ID As String = "CstId"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_BKGEXP_CST_TYPE As String = "CstType"
    ''' <summary>顧客車両区分</summary>
    Private Const SESSION_KEY_BKGEXP_CST_VCL_TYPE As String = "CstVclType"
    '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

    'メニューのＩＤを定義
    Private Const CUSTOMER_SEARCH As Integer = 200
    Private Const SUBMENU_TESTDRIVE As Integer = 201
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
    Private Const SUBMENU_HELP As Integer = 203
    '2014/05/07 TCS 高橋 受注後フォロー機能開発
    Private Const SUBMENU_BOOKING_EXPLAIN = 205
    '2014/05/07 TCS 高橋 受注後フォロー機能開発
    Private Const MAIN_MENU As Integer = 100
    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ''' <summary>
    ''' ショールーム
    ''' </summary>
    Private Const SHOW_ROOM As Integer = 1200
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' マスタページ
    ''' </summary>
    ''' <remarks></remarks>
    Private localCommonMaster As CommonMasterPage
    '2012/01/24 TCS 河原 【SALES_1B】 END

    ''' <summary>
    ''' フッター作成
    ''' </summary>
    ''' <param name="commonMaster"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeclareCommonMasterFooter Start")
        'ログ出力 End *****************************************************************************

        Me.localCommonMaster = commonMaster
        category = FooterMenuCategory.Customer

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeclareCommonMasterFooter End")
        'ログ出力 End *****************************************************************************

        '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
        Return {SUBMENU_TESTDRIVE, SUBMENU_HELP, SUBMENU_BOOKING_EXPLAIN}
        '2014/05/07 TCS 高橋 受注後フォロー機能開発 END
    End Function

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' コンテキストメニュー作成
    ''' </summary>
    ''' <param name="commonMaster">マスタページ</param>
    ''' <returns>表示内容</returns>
    ''' <remarks>コンテキストメニューの作成</remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()
        '表示する可能性があるものを全て表示する
        ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        Return New Integer() {C_SALES_START, C_SALES_END, C_DELIVERY_START, C_DELIVERY_END, C_BUSINESS_START, C_BUSINESS_CANCEL,
                              C_CORRESPOND_START, C_CORRESPOND_END, C_SALES_STOP, C_DELIVERYCORRESPOND_START, C_DELIVERYCORRESPOND_END,
                              CommonMasterContextMenuBuiltinMenuID.SuspendItem, CommonMasterContextMenuBuiltinMenuID.StandByItem, CommonMasterContextMenuBuiltinMenuID.LogoutItem}
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        ' 2012/08/13 TCS 安田 商談中断メニューの追加 END
    End Function
    '2012/01/24 TCS 河原 【SALES_1B】 END

    ''' <summary>
    ''' 完了ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RegistButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegistButton.Click
        ' 2012/02/15 TCS 相田 【SALES_2】 START
        If Sc3080203Page.Visible Then
            CType(Sc3080203Page, ISC3080203Control).RegistActivity()
        End If
        If Sc3080216Page.Visible Then
            CType(Sc3080216Page, ISC3080203Control).RegistActivity()
        End If
        ' 2012/02/15 TCS 相田 【SALES_2】 END
    End Sub

    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info("InitHeaderEvent Start")
        'ログ出力 End *****************************************************************************

        '2012/01/24 TCS 河原 【SALES_1B】 START
        If IsSales() Then
            '戻るボタンを非活性
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = False
            '戻る・進む(商談中はログアウトが無いため)
            For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward}
                '活動破棄チェックのクライアントサイドスクリプトを埋め込む
                CType(Me.Master, CommonMasterPage).GetHeaderButton(buttonId).OnClientClick = "return SC3080201.cancellationCheck();"
            Next
        Else
            '戻るボタンを活性()
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = True
            '戻る・進む・ログアウト
            For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward, HeaderButton.Logout}
                '活動破棄チェックのクライアントサイドスクリプトを埋め込む
                CType(Me.Master, CommonMasterPage).GetHeaderButton(buttonId).OnClientClick = "return SC3080201.cancellationCheck();"
            Next
        End If
        '2012/01/24 TCS 河原 【SALES_1B】 END

        'ログ出力 Start ***************************************************************************
        Logger.Info("InitHeaderEvent End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info("InitFooterEvent Start")
        'ログ出力 End *****************************************************************************

        ' ボタン非活性
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Enabled = False
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled = False

        '2012/01/24 TCS 河原 【SALES_1B】 START

        'ログイン権限がセールススタッフでない場合、フッターボタンを非表示
        Dim OpeCD As Integer = StaffContext.Current.OpeCD
        Dim SSF As Integer = Operation.SSF
        If OpeCD <> SSF Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Visible = False
            '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Visible = False
            'CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).Visible = False
        End If

        '押下時イベント
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).OnClientClick = "return false;"
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).OnClientClick = "return false;"

        '査定機能が蓋締めの場合非表示に
        Dim dlrenvdt As New DealerEnvSetting
        Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除

        '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
        '商談中・営業活動中・一時対応中の場合、メインメニューを非活性
        If IsSales() Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Enabled = False
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Enabled = False
        Else
            CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Enabled = True
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Enabled = True
        End If
        '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        SetTestDriveButton()
        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        'Follow-upBox内連番がある場合、査定依頼・ヘルプ依頼を活性
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled = True
            '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
        End If

        '現在表示している活動と、商談中の活動が異なる場合、査定依頼・ヘルプ依頼を非活性
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            Dim fllwSeq As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()
            Dim fllwSeqSales As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False).ToString()
            If String.Equals(fllwSeq, fllwSeqSales) = False Then
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled = False
                '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除
            End If
        End If

        '2枚目が表示されないケースのみ共通部分でTCVボタンの制御を行う
        If Not Sc3080202Page.Visible Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV).Enabled = False
            '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
            '納車時説明ボタン(非活性表示)
            Dim newCarExplainBtn As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain)
            If newCarExplainBtn IsNot Nothing Then
                'セールススタッフ、チームリーダーにのみ開放される
                newCarExplainBtn.Enabled = False
            End If
            '受注時説明ボタン(非表示)
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_BOOKING_EXPLAIN).Visible = False
            '2014/05/07 TCS 高橋 受注後フォロー機能開発 END
        End If

        'ヘルプボタンが非活性の場合、ポップアップを非表示にする
        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled Then
            SC3080401.Visible = True
        Else
            SC3080401.Visible = False
        End If

        '2012/01/24 TCS 河原 【SALES_1B】 END

        'メニュー
        AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Click, _
            Sub()
                'メニューに遷移
                Me.RedirectNextScreen("SC3010203")
            End Sub

        CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).OnClientClick = "return SC3080201.cancellationCheck();"

        '顧客詳細
        CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).OnClientClick = "return false;"


        '2016/09/09 TCS 藤井 セールスタブレット性能改善 DEL START
        '2016/09/09 TCS 藤井 セールスタブレット性能改善 DEL END

        '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
        '納車時説明ボタン
        Dim newCarExplainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain)
        If newCarExplainButton IsNot Nothing Then
            'セールススタッフ、チームリーダーにのみ開放される
            AddHandler newCarExplainButton.Click, AddressOf NewCarExplainButton_Click

        End If

        '受注時説明ボタン
        Dim bookingExplainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_BOOKING_EXPLAIN)
        AddHandler bookingExplainButton.Click, AddressOf BookingExplainButton_Click

        '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

        ' 2012/02/29 TCS 小野 【SALES_2】 START
        'ショールーム
        Dim ssvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SHOW_ROOM)
        If ssvButton IsNot Nothing Then
            AddHandler ssvButton.Click, _
            Sub()
                '受付メインに遷移
                Me.RedirectNextScreen("SC3100101")
            End Sub
        End If
        ' 2012/02/29 TCS 小野 【SALES_2】 END

        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
        '新車納車システム連携メニュー
        Dim linkMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(LINK_MENU)
        ''リンク先URLを販売店環境設定TBLより取得
        dlrenvrw = dlrenvdt.GetEnvSetting(StaffContext.Current.DlrCD, C_LINK_MENU_URL)
        If dlrenvrw IsNot Nothing Then
            If Not String.IsNullOrWhiteSpace(dlrenvrw.PARAMVALUE) Then
                ''URLを取得できた場合、新車納車システム連携メニューを表示。
                If linkMenuButton IsNot Nothing Then
                    linkMenuButton.Visible = True
                    ''システム環境設定より別ブラウザのURLスキーム取得。
                    Dim sysenv As New SystemEnvSetting
                    Dim rw1 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEME)
                    Dim rw2 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEMES)
                    ''新車納車システムへのパラメータ取得。
                    Dim parmDmsId As String = String.Empty
                    Dim parmContNo As String = String.Empty
                    ''活動先顧客コードが存在するか(新規顧客作成時でないか)
                    If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                        Dim crcustId As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
                        If Not String.IsNullOrEmpty(crcustId) Then
                            ''新規顧客でないとき、受注後工程であれば注文番号を取得
                            parmContNo = IsBookedAfter()
                            ''DMSIDを取得
                            If (Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()).Equals(ORGCUSTFLG) Then
                                ''自社客のとき
                                parmDmsId = SC3080201BusinessLogic.GetDmsIdOrg(crcustId)
                            Else
                                ''未取引客のとき
                                If Not String.IsNullOrEmpty(parmContNo) Then
                                    ''未取引客、かつ受注後工程(注文番号が取得できた)
                                    parmDmsId = SC3080201BusinessLogic.GetDmsIdNew(StaffContext.Current.DlrCD, parmContNo)
                                End If
                            End If
                        End If
                    End If
                    ''新車納車システムへのリンクURL作成。(URLスキーム置き換え)
                    Dim linkUrl As String = dlrenvrw.PARAMVALUE
                    linkUrl = linkUrl.Replace("http://", rw1.PARAMVALUE + "://")
                    linkUrl = linkUrl.Replace("https://", rw2.PARAMVALUE + "://")
                    linkUrl = linkUrl.Replace("$1", HttpUtility.UrlEncode(StaffContext.Current.DlrCD))
                    linkUrl = linkUrl.Replace("$2", HttpUtility.UrlEncode(StaffContext.Current.BrnCD))
                    linkUrl = linkUrl.Replace("$3", HttpUtility.UrlEncode(StaffContext.Current.Account))
                    linkUrl = linkUrl.Replace("$4", HttpUtility.UrlEncode(parmContNo))
                    linkUrl = linkUrl.Replace("$5", HttpUtility.UrlEncode(parmDmsId))

                    ''メニューをタップしたときに実行されるJavaScript。
                    linkMenuButton.OnClientClick = BindParameters("return linkMenu('{0}');", {linkUrl})

                End If
            End If
        End If

        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END
        'ログ出力 Start ***************************************************************************
        Logger.Info("InitHeaderEvent End")
        'ログ出力 End *****************************************************************************

    End Sub

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    ''' <summary>
    ''' 文字列にパラメータをバインドします。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="parameters">パラメータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function BindParameters(ByVal value As String, ByVal parameters As Object()) As String
        Return String.Format(CultureInfo.InvariantCulture, value, parameters)
    End Function

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    '2012/01/24 TCS 河原 【SALES_1B】DELETE START
    '2012/01/24 TCS 河原 【SALES_1B】DELETE END

    ''' <summary>
    ''' 見積りID取得
    ''' </summary>
    ''' <param name="fboxDlrCd">販売店コード</param>
    ''' <param name="fboxStrCd">店舗コード</param>
    ''' <param name="fboxSeqNo">Follow-up box 連番</param>
    ''' <returns>見積もりＩＤ（複数件存在する場合は、カンマ区切り）</returns>
    ''' <remarks>TCVに遷移する際に必要となる見積もりＩＤを取得します</remarks>
    Private Function GetEstimatedId(ByVal fboxDlrCd As String, ByVal fboxStrCd As String, ByVal fboxSeqNo As Long) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetEstimatedId Start")
        'ログ出力 End *****************************************************************************

        '返却用の見積もりＩＤ変数
        Dim returnEstId As New StringBuilder

        Using param As New SC3080202GetEstimateidFromDataTable

            '検索条件となるレコードを作製
            Dim conditionRow As SC3080202GetEstimateidFromRow = param.NewSC3080202GetEstimateidFromRow()
            conditionRow.DLRCD = fboxDlrCd
            conditionRow.STRCD = fboxStrCd
            conditionRow.FLLWUPBOX_SEQNO = fboxSeqNo
            '検索条件を登録
            param.AddSC3080202GetEstimateidFromRow(conditionRow)

            '検索処理
            Dim result As SC3080202GetEstimateidToDataTable = SC3080202BusinessLogic.GetEstimatedId(param)

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            'カンマ区切りに編集
            For Each dr As SC3080202GetEstimateidToRow In result.Rows
                'カンマ編集
                If returnEstId.Length > 0 Then
                    returnEstId.Append(",")
                End If

                '１件分の見積もりＩＤセット
                returnEstId.Append(dr.ESTIMATEID.ToString(CultureInfo.CurrentCulture))
            Next
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetEstimatedId End")
        'ログ出力 End *****************************************************************************

        '処理結果返却
        Return returnEstId.ToString()
    End Function

    '2012/01/27 TCS 平野 【SALES_1B】 START
    ''' <summary>
    ''' 契約状況取得処理
    ''' </summary>
    ''' <param name="EstimateId">見積もりID</param>
    ''' <returns>True:契約済み False:契約済み以外</returns>
    ''' <remarks></remarks>
    Private Function GetContractFlg(ByVal EstimateId As String) As Boolean
        'ログ出力 Start ***************************************************************************
        Logger.Info("GetContractFlg Start")
        'ログ出力 End *****************************************************************************

        Dim result As SC3080201ContractDataTable = Nothing
        Dim rtnFlg As Boolean = True

        Using param As New SC3080201ESTIMATEINFODataTable
            Dim conditionRow As SC3080201ESTIMATEINFORow = param.NewSC3080201ESTIMATEINFORow
            conditionRow.ESTIMATEID = EstimateId

            '検索条件を登録
            param.AddSC3080201ESTIMATEINFORow(conditionRow)

            '検索処理
            result = SC3080201BusinessLogic.GetContractFlg(param)
        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Info("GetContractFlg End")
        'ログ出力 End *****************************************************************************

        '処理結果返却
        If result.Rows.Count > 0 Then
            Dim dr As SC3080201DataSet.SC3080201ContractRow = CType(result.Rows(0), SC3080201DataSet.SC3080201ContractRow)
            If Not (dr.CONTRACT_APPROVAL_STATUS.Equals("1") OrElse dr.CONTRACT_APPROVAL_STATUS.Equals("2")) Then
                rtnFlg = False
            End If
        End If

        Return rtnFlg
    End Function
    '2012/01/27 TCS 平野 【SALES_1B】 END

    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' 検索ボックスの制御
    ''' </summary>
    ''' <remarks>商談中・営業・一時対応中は検索ボックスを非活性にする</remarks>
    Private Sub InitSearchBox()

        'ログ出力 Start ***************************************************************************
        Logger.Info("InitSearchBox Start")
        'ログ出力 End *****************************************************************************

        '商談中(営業活動・一時対応も)の場合、検索ボックスに名前を入れ非活性に
        If IsSales() Then
            Me.localCommonMaster.SearchBox.Enabled = False
            If IsSession(SESSION_KEY_NAME) Then
                Me.localCommonMaster.SearchBox.SearchText = Me.GetValue(ScreenPos.Current, SESSION_KEY_NAME, False).ToString()
            Else
                Me.localCommonMaster.SearchBox.SearchText = ""
            End If
        ElseIf Me.localCommonMaster.SearchBox.Enabled = False Then
            '検索ボックスの状態を元に戻す
            Me.localCommonMaster.SearchBox.Enabled = True
            Me.localCommonMaster.SearchBox.SearchText = ""
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("InitSearchBox End")
        'ログ出力 End *****************************************************************************

    End Sub

    '2016/09/09 TCS 藤井 セールスタブレット性能改善 DEL START
    '2016/09/09 TCS 藤井 セールスタブレット性能改善 DEL END

    '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD START
    ''' <summary>
    ''' TCV機能に渡す引数の設定
    ''' </summary>
    ''' <param name="params">TCV機能に渡す引数</param>
    ''' <remarks>フッター（TCV）タップ時にTCV機能へ渡す引数を設定する。</remarks>
    Private Sub AddTcvParameters(ByRef params As Dictionary(Of String, Object))

        'ログ出力 Start ***************************************************************************
        Logger.Info("AddTcvParameters Start")
        'ログ出力 End *****************************************************************************

        '商談フラグの設定(商談中・営業活動中・一時対応中の場合Trueを設定)
        Dim BusinessFlg As Boolean = False
        If IsSales() Then
            BusinessFlg = True
        End If

        '読み取り専用フラグ設定
        Dim ReadOnlyFlg As Boolean = True

        '営業活動中・商談中・一時対応中・納車作業中(一時対応)以外は読取専用にする
        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3")) Then
            ReadOnlyFlg = False
        End If

        Dim OpeCd As Integer = StaffContext.Current.OpeCD

        If Not _getestimateIdFlg Then
            '見積りID取得
            _estimateId = GetEstimatedId(StaffContext.Current.DlrCD,
                                         StaffContext.Current.BrnCD,
                                         CLng(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()))

            _getestimateIdFlg = True

        End If

        If _estimateId.Length <= 0 Then
            '見積りIDがない場合
            params.Add("DataSource", "None")
            params.Add("DlrCd", StaffContext.Current.DlrCD)
            params.Add("StrCd", StaffContext.Current.BrnCD)
            params.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            params.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString())
            params.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString())
            params.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            params.Add("Account", StaffContext.Current.Account)
            params.Add("AccountStrCd", StaffContext.Current.BrnCD)
            params.Add("MenuLockFlag", False)
            params.Add("OperationCode", OpeCd)
            params.Add("BusinessFlg", BusinessFlg)
            params.Add("ReadOnlyFlg", ReadOnlyFlg)
        Else
            '見積りIDがある場合

            If ReadOnlyFlg = False Then
                For Each estId In _estimateId.Split(","c)
                    ReadOnlyFlg = GetContractFlg(estId)
                    If ReadOnlyFlg Then
                        Exit For
                    End If
                Next
            End If

            params.Add("DataSource", "EstimateId")
            params.Add("DlrCd", StaffContext.Current.DlrCD)
            params.Add("StrCd", StaffContext.Current.BrnCD)
            params.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            params.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString())
            params.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString())
            params.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            params.Add("StartPageId", "SC3050201")
            params.Add("EstimateId", _estimateId)
            params.Add("SelectedEstimateIndex", "0")
            params.Add("Account", StaffContext.Current.Account)
            params.Add("AccountStrCd", StaffContext.Current.BrnCD)
            params.Add("MenuLockFlag", False)
            params.Add("OperationCode", OpeCd)
            params.Add("BusinessFlg", BusinessFlg)
            params.Add("ReadOnlyFlg", ReadOnlyFlg)
        End If
        params.Add("CloseCallback", "icropScript.tcvCloseCallback")
        params.Add("StatusCallback", "icropScript.tcvStatusCallback")

        'ログ出力 Start *************************************************************************** 
        Logger.Info("AddTcvParameters End")
        'ログ出力 End ***************************************************************************** 

    End Sub

    ''' <summary>
    ''' TCV機能呼出Script作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>TCV機能を呼び出すScriptを作成する。</remarks>
    Private Function BuildOpenTcvScript() As String

        'ログ出力 Start ***************************************************************************
        Logger.Info("BuildOpenTcvScript Start")
        'ログ出力 End *****************************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function openTcv() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf)
        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf)
        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf)
        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson())).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start ***************************************************************************
        Logger.Info("BuildOpenTcvScript End")
        'ログ出力 End *****************************************************************************

        Return sb.ToString
    End Function

    ''' <summary>
    ''' JSON 文字列変換
    ''' </summary>
    ''' <returns>JSON 文字列に変換した文字列</returns>
    ''' <remarks>オブジェクトをJSON 文字列に変換する。</remarks>
    Private Function BuildTcvParametersAsJson() As String

        'ログ出力 Start ***************************************************************************
        Logger.Info("BuildTcvParametersAsJson Start")
        'ログ出力 End *****************************************************************************

        Dim tcvParams As New Dictionary(Of String, Object)
        AddTcvParameters(tcvParams)
        Dim serializer As New JavaScriptSerializer
        Dim tcvParamsJson As String = serializer.Serialize(tcvParams)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TCV Call parameter:" & tcvParamsJson)
        Logger.Info("BuildTcvParametersAsJson End")
        'ログ出力 End *****************************************************************************

        Return tcvParamsJson
    End Function
    '2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD END


    '2014/05/07 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' フッター 受注時説明ボタン
    ''' </summary>
    ''' <remarks>
    ''' 遷移パラメータの設定＋画面遷移
    ''' </remarks>
    Private Sub BookingExplainButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)
        Logger.Info("BookingExplainButton_Click Start")

        '商談ID
        Dim salesid As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_SALES_ID, salesid)
        '見積ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_EST_ID, String.Empty)
        '受注時説明表示モード(2 スタッフ予定変更モード)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_VIEW_MODE, "2")
        '契約条件変更フラグ
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_CHG_FLG, String.Empty)
        '顧客ID
        Dim cstid As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_CST_ID, cstid)
        '顧客種別
        Dim cstType As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_CST_TYPE, cstType)
        '顧客車両種別
        Dim cstVclType As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_BKGEXP_CST_VCL_TYPE, cstVclType)

        '受注時説明へ遷移
        Me.RedirectNextScreen("SC3270101")

        Logger.Info("BookingExplainButton_Click End")
    End Sub

    ''' <summary>
    ''' フッター 納車時説明ボタン
    ''' </summary>
    ''' <remarks>
    ''' 遷移パラメータの設定
    ''' </remarks>
    Private Sub NewCarExplainButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)
        Logger.Info("NewCarExplainButton_Click Start")

        '商談ID
        Dim salesid As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_SALES_ID, salesid)
        '顧客ID
        Dim cstid As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_CST_ID, cstid)
        '顧客種別
        Dim cstType As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_CST_TYPE, cstType)
        '顧客車両種別
        Dim cstVclType As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_CST_VCL_TYPE, cstVclType)

        '2018/06/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '契約ID
        Dim constNo As String = IsBookedAfter()
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_CONTRACT_NO, constNo)

        '販売店コード（DMSコード）
        Dim dmsDlrCd As String = String.Empty
        '店舗コード（DMSコード）
        Dim dmsBrnCd As String = String.Empty

        '基幹コード取得
        Dim dmsCodeMap As New DmsCodeMap
        Dim drDmsCodeMap As DmsCodeMapDataSet.DMSCODEMAPRow _
            = dmsCodeMap.GetDmsCodeMap(C_DMS_CODE_TYPE_BRANCH, StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)

        If Not drDmsCodeMap Is Nothing Then
            '店舗コードの設定
            'プログラム設定より使用するDMS店舗コードを選択
            Dim programSettingV4 As New ProgramSettingV4
            Dim drProgramSettingV4 As ProgramSettingV4DataSet.PROGRAMSETTINGV4Row _
                = programSettingV4.GetProgramSettingV4(C_DMS_PROGRAM_SETTING_PROGRAM_CD, C_DMS_PROGRAM_SETTING_SETTING_SECTION, C_DMS_PROGRAM_SETTING_SETTING_KEY)

            If Not drProgramSettingV4 Is Nothing Then
                Dim strProgramSettingV4Val As String = drProgramSettingV4.SETTING_VAL
                If C_DMS_CODE_MAP_DMS_CD_3.Equals(strProgramSettingV4Val) Then
                    dmsBrnCd = drDmsCodeMap.DMS_CD_3
                Else
                    dmsBrnCd = drDmsCodeMap.DMS_CD_2
                End If
            Else
                'プログラム設定が取得できなかった場合は基幹コード2を取得
                dmsBrnCd = drDmsCodeMap.DMS_CD_2
            End If

            '販売店コードの設定
            dmsDlrCd = drDmsCodeMap.DMS_CD_1
        End If

        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_DEALER_CODE, dmsDlrCd)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_NEWCAREXP_BRANCH_CODE, dmsBrnCd)
        '2018/06/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        Logger.Info("NewCarExplainButton_Click End")
    End Sub
    '2014/05/07 TCS 高橋 受注後フォロー機能開発 END

    ''' <summary>
    ''' 商談開始ボタン
    ''' </summary>
    ''' <remarks>商談開始ボタン</remarks>
    Private Sub SalesStartButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesStartButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesStart(C_SALES_START)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesStartButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ' 2012/02/15 TCS 相田 【SALES_2】 START　
    ' ''' <summary>
    ' ''' 商談キャンセルボタン
    ' ''' </summary>
    ' ''' <remarks>商談キャンセルボタン</remarks>
    'Private Sub SalesCancelButton_Click(sender As Object, e As EventArgs)

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SalesCancelButton_Click Start")
    '    'ログ出力 End *****************************************************************************

    '    RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW)
    '    SalesEnd(C_SALES_CANCEL)

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SalesCancelButton_Click End")
    '    'ログ出力 End *****************************************************************************

    'End Sub
    ' 2012/02/15 TCS 相田 【SALES_2】 END　

    ''' <summary>
    ''' 一時対応開始ボタン
    ''' </summary>
    ''' <remarks>一時対応開始ボタン</remarks>
    Private Sub TempCorrespondStartButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TempCorrespondStartButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesStart(C_CORRESPOND_START)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TempCorrespondStartButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 一時対応終了ボタン
    ''' </summary>
    ''' <remarks>一時対応終了ボタン</remarks>
    Private Sub TempCorrespondEndButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TempCorrespondEndButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesEnd(C_CORRESPOND_END)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TempCorrespondEndButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 営業活動開始ボタン
    ''' </summary>
    ''' <remarks>営業活動開始ボタン</remarks>
    Private Sub BusinessStartButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("BusinessStartButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesStart(C_BUSINESS_START)

        'ログ出力 Start ***************************************************************************
        Logger.Info("BusinessStartButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 営業キャンセルボタン
    ''' </summary>
    ''' <remarks>営業キャンセルボタン</remarks>
    Private Sub BusinessCancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("BusinessCancelButton_Click Start")
        'ログ出力 End *****************************************************************************

        RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW)
        SalesEnd(C_BUSINESS_CANCEL)

        'ログ出力 Start ***************************************************************************
        Logger.Info("BusinessCancelButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' 商談終了ボタン
    ''' </summary>
    ''' <remarks>商談終了ボタン</remarks>
    Private Sub salesEndButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("salesEndButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesEnd(C_SALES_END)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("salesEndButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub
    ' 2012/02/15 TCS 相田 【SALES_2】 END

    ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
    ''' <summary>
    ''' 商談中断ボタン
    ''' </summary>
    ''' <remarks>商談中断ボタン</remarks>
    Private Sub salesStopButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("salesStopButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesEnd(C_SALES_STOP)

        '来店実績連番引き当て
        GetVisitSeq()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("salesStopButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub
    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    ''' <summary>
    ''' 納車作業開始ボタン
    ''' </summary>
    ''' <remarks>納車作業開始ボタン</remarks>
    Private Sub DeliveryStartButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryStartButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesStart(C_DELIVERY_START)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryStartButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 納車作業終了ボタン
    ''' </summary>
    ''' <remarks>納車作業終了ボタン</remarks>
    Private Sub DeliveryEndButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryEndButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesEnd(C_DELIVERY_END)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryEndButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub


    ''' <summary>
    ''' 納車作業開始(一時対応)ボタン
    ''' </summary>
    ''' <remarks>納車作業開始(一時対応)ボタン</remarks>
    Private Sub DeliveryCorrespondStartButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryCorrespondStartButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesStart(C_DELIVERYCORRESPOND_START)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryCorrespondStartButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub


    ''' <summary>
    ''' 納車作業終了(一時対応)ボタン
    ''' </summary>
    ''' <remarks>納車作業終了(一時対応)ボタン</remarks>
    Private Sub DeliveryCorrespondEndButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryCorrespondEndButton_Click Start")
        'ログ出力 End *****************************************************************************

        SalesEnd(C_DELIVERYCORRESPOND_END)

        'ログ出力 Start ***************************************************************************
        Logger.Info("DeliveryCorrespondEndButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End


    ''' <summary>
    ''' 商談開始系の処理
    ''' </summary>
    ''' <param name="status">ステータス</param>
    ''' <remarks>商談開始系の処理</remarks>
    Private Sub SalesStart(ByVal status As Integer)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesStart Start")
        'ログ出力 End *****************************************************************************

        Using param As New SC3080201SalesStartDataTable
            '登録に必要な値をセット
            Dim dr As SC3080201SalesStartRow = param.NewSC3080201SalesStartRow()
            dr.DLRCD = StaffContext.Current.DlrCD

            Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
            Dim PresenceDetail As String = StaffContext.Current.PresenceDetail
            'Dim newFllwFlg As Boolean = False
            dr.NEWFLG = False

            'Follow-upBox内連番の存在確認
            If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                Dim rsltDt As SC3080202GetStatusToDataTable = GetFollowupboxStatus()
                If rsltDt.Rows.Count > 0 Then
                    If Not rsltDt(0).ENABLEFLG Then
                        dr.NEWFLG = True
                    End If
                Else
                    dr.NEWFLG = True
                End If

            Else
                dr.NEWFLG = True
            End If

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            If dr.NEWFLG Then
                If IsSession(SESSION_KEY_ORDER_NO) Then
                    '受注Noが存在する場合
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                    Dim fllwupboxSeqno As Decimal = Decimal.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)

                    If ActivityInfoBusinessLogic.IsExistsUnexecutedAfterOdrAct(fllwupboxSeqno) Then
                        '未活動の受注後活動が存在する場合
                        dr.NEWFLG = False
                    End If
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                End If
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            '2013/03/06 TCS 河原 GL0874 START
            If Me.ContractCancelStartFlg.Value = "1" Then
                dr.NEWFLG = False
            End If
            '2013/03/06 TCS 河原 GL0874 END

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 ADD START
            'TMT過渡期対応(受注後工程無効化)
            '受注後工程が使用不可
            If IsSession(SESSION_KEY_SALESAFTER) AndAlso Not ActivityInfoBusinessLogic.CheckUsedB2D() Then
                Dim afterFllow = CType(GetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, False), String)
                '成約の活動結果が登録済みの場合、新規活動とする
                If ActivityInfoBusinessLogic.SALESAFTER_YES.Equals(afterFllow) Then dr.NEWFLG = True
            End If
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 ADD END

            ' 2012/03/27 TCS 安田 【SALES_2】 START
            If dr.NEWFLG Then
                If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                    Dim seqNo As String
                    Dim biz As New SC3080201BusinessLogic
                    Dim fllwupboxStrcd As String = StaffContext.Current.BrnCD
                    Dim strCRCUSTID As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
                    Dim strCUSTSEGMENT As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()
                    Dim strNEWCUSTID As String = String.Empty
                    If ContainsKey(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID) Then
                        strNEWCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID, False), String)
                    End If

                    seqNo = biz.GetSalesActiveList(dr.DLRCD, fllwupboxStrcd, strCRCUSTID, strCUSTSEGMENT, strNEWCUSTID)

                    If (Not String.IsNullOrEmpty(seqNo)) Then
                        dr.NEWFLG = False
                        SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, seqNo)
                    End If
                End If
            End If
            ' 2012/03/27 TCS 安田 【SALES_2】 END

            '新規活動フラグがTrueで、一時対応開始ではない場合
            'If dr.NEWFLG And Not String.Equals(status, C_CORRESPOND_START) Then
            If dr.NEWFLG Then
                '新規で采番する
                Dim biz As New SC3080201BusinessLogic
                dr.FLLWUPBOX_SEQNO = biz.GetFllowSeq()
                dr.STRCD = StaffContext.Current.BrnCD

                'セッションにセット
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, dr.FLLWUPBOX_SEQNO.ToString(CultureInfo.CurrentCulture))
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, dr.STRCD)
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW, dr.FLLWUPBOX_SEQNO.ToString(CultureInfo.CurrentCulture))
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                '活動ID情報を破棄
                RemoveValue(ScreenPos.Current, SESSION_KEY_ACT_ID)
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            Else
                If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                    dr.FLLWUPBOX_SEQNO = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)
                    dr.STRCD = StaffContext.Current.BrnCD
                Else
                    dr.FLLWUPBOX_SEQNO = Nothing
                    dr.STRCD = Nothing
                End If
            End If

            If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                '商談中の活動情報をセッションに保存
                SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
                SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, StaffContext.Current.BrnCD)
            End If

            '顧客情報を設定
            dr.CUSTSEGMENT = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()
            dr.CUSTOMERCLASS = Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString()
            dr.CRCUSTID = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            dr.SALESSTAFFCD = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False).ToString()

            '来店人数設定
            If IsSession(SESSION_KEY_WALKINNUM) Then
                dr.WALKINNUM = Integer.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_WALKINNUM, False)), CultureInfo.CurrentCulture)
            Else
                dr.WALKINNUM = 0
            End If

            'ステータスを設定
            dr.STATUS = CStr(status)

            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            '接客区分(営業開始の場合は、接客区分を更新しないので、この値は使用しない)
            If IsSession(SESSION_KEY_ORDER_NO) AndAlso Not dr.NEWFLG Then
                '注文番号がある + 活動を継続する場合、納車作業を設定
                dr.CST_SERVICE_TYPE = CST_SERVICE_TYPE_DELIVERY
            Else
                '上記以外の場合、商談を設定
                dr.CST_SERVICE_TYPE = CST_SERVICE_TYPE_SALES
            End If
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            '登録フラグ・商談シーケンスNoの設定
            dr.REGISTFLG = GetRegistFlg(dr.DLRCD, dr.STRCD, dr.FLLWUPBOX_SEQNO)
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
            'If IsSession(SESSION_KEY_SALES_SEQNO) Then
            '    dr.SALES_SEQNO = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_SEQNO, False)), CultureInfo.CurrentCulture)
            'End If
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            param.AddSC3080201SalesStartRow(dr)

            Dim msgId As Integer
            Dim bizClass As New SC3080201BusinessLogic

            '更新処理呼び出し
            Dim rlstFlg As Boolean = bizClass.StartVisitSales(param, msgId)
            If rlstFlg Then
                '商談開始・一時対応開始・納車作業開始・納車作業開始(一時対応)の場合、Pushサーバーへの通知を実施
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                If status = C_SALES_START Or status = C_CORRESPOND_START Or
                    status = C_DELIVERY_START Or status = C_DELIVERYCORRESPOND_START Then
                    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                    'Push.サーバーへの通知処理
                    bizClass.PushUpdateVisitSalesStart()
                End If
            Else
                'ステータスをスタンバイに戻す
                Dim PresenceCategorySession As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_PRESENCECATEGORY, False).ToString()
                Dim PresenceDetailSession As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_PRESENCEDETAIL, False).ToString()
                StaffContext.Current.UpdatePresence(PresenceCategorySession, PresenceDetailSession)

                '商談終了時にFollow-upBox情報を破棄
                RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW)

                Dim wordNo As Integer
                If msgId = 5002 Then
                    '既に他の人が商談を開始している場合
                    wordNo = 10914
                Else
                    'その他システムエラー
                    wordNo = 10915
                End If
                'エラーメッセージ表示
                Dim word As String = WebWordUtility.GetWord(wordNo)
                Dim alert As New StringBuilder
                alert.Append("<script type='text/javascript'>")
                alert.Append("  icropScript.ShowMessageBox('','" & HttpUtility.JavaScriptStringEncode(word) & "','')")
                alert.Append("</script>")
                Dim cs As ClientScriptManager = Page.ClientScript
                cs.RegisterStartupScript(Me.GetType, "alert", alert.ToString)
            End If
        End Using

        '2ページ目の内容を更新
        CType(Sc3080202Page, ISC3080202Control).ReflectionActivityStatus()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesStart End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 商談終了系の処理
    ''' </summary>
    ''' <param name="status">ステータス</param>
    ''' <remarks>商談終了系の処理</remarks>
    Private Sub SalesEnd(ByVal status As Integer)

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesEnd Start")
        'ログ出力 End *****************************************************************************

        Using param As New SC3080201SalesStartDataTable
            '登録に必要な値をセット
            Dim dr As SC3080201SalesStartRow = param.NewSC3080201SalesStartRow()

            '顧客情報を設定
            dr.CUSTSEGMENT = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()
            dr.CUSTOMERCLASS = Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString()
            dr.CRCUSTID = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            dr.SALESSTAFFCD = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False).ToString()

            dr.DLRCD = StaffContext.Current.DlrCD

            If IsSession(SESSION_KEY_FLLWUPBOX_STRCD) Then
                dr.STRCD = StaffContext.Current.BrnCD
            Else
                dr.STRCD = Nothing
            End If

            If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                dr.FLLWUPBOX_SEQNO = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False)), CultureInfo.CurrentCulture)
            Else
                dr.FLLWUPBOX_SEQNO = Nothing
            End If

            dr.STATUS = CStr(status)

            dr.CUSTNAME = Me.GetValue(ScreenPos.Current, SESSION_KEY_NAME, False).ToString()

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
            'dr.SALES_SEQNO = Long.Parse(CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_SEQNO, False)), CultureInfo.CurrentCulture)
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            param.AddSC3080201SalesStartRow(dr)

            Dim rlstFlg As Boolean
            Dim msgId As Integer
            Dim bizClass As New SC3080201BusinessLogic

            '更新処理呼び出し
            rlstFlg = bizClass.EndVisitSales(param, msgId)
            If rlstFlg Then
                ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
                '商談終了・商談中断・一時対応完了・納車作業終了・納車作業終了(一時対応)の場合、Pushサーバーへの通知を実施
                'If status = C_SALES_END Or status = C_CORRESPOND_END Then
                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                If status = C_SALES_END Or status = C_SALES_STOP Or status = C_CORRESPOND_END Or
                    status = C_DELIVERY_END Or status = C_DELIVERYCORRESPOND_END Then
                    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
                    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

                    '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 START
                    'Pushサーバーへの通知処理
                    bizClass.PushUpdateVisitSalesEnd(CStr(status))
                    '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 END
                End If
            Else
                'ステータスをスタンバイに戻す
                Dim PresenceCategory As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_PRESENCECATEGORY, False).ToString()
                Dim PresenceDetail As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_PRESENCEDETAIL, False).ToString()
                StaffContext.Current.UpdatePresence(PresenceCategory, PresenceDetail)

                Dim wordNo As Integer
                'ここでエラーが出るケースはシステムエラー固定
                wordNo = 10915
                'エラーメッセージ表示
                Dim word As String = WebWordUtility.GetWord(wordNo)
                Dim alert As New StringBuilder
                alert.Append("<script type='text/javascript'>")
                alert.Append("  icropScript.ShowMessageBox('','" & HttpUtility.JavaScriptStringEncode(word) & "','')")
                alert.Append("</script>")
                Dim cs As ClientScriptManager = Page.ClientScript
                cs.RegisterStartupScript(Me.GetType, "alert", alert.ToString)
            End If
        End Using

        '商談終了時にFollow-upBox情報を破棄
        RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX)
        RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD)
        RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES)
        RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES)

        '2ページ目の内容を更新
        CType(Sc3080202Page, ISC3080202Control).ReflectionActivityStatus()

        'ログ出力 Start ***************************************************************************
        Logger.Info("SalesEnd End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2012/01/24 TCS 河原 【SALES_1B】 END

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

#End Region

    '2012/01/24 TCS 河原 【SALES_1B】 START
#Region " 共通処理 "
    ''' <summary>
    ''' 商談(一時対応・営業活動)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>ステータスを参照して商談中か判断する</remarks>
    Private Function IsSales() As Boolean

        'ログ出力 Start ***************************************************************************
        Logger.Info("IsSales Start")
        'ログ出力 End *****************************************************************************

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3")) Then
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

            'ログ出力 Start ***************************************************************************
            Logger.Info("IsSales End")
            'ログ出力 End *****************************************************************************

            Return True
        Else

            'ログ出力 Start ***************************************************************************
            Logger.Info("IsSales End")
            'ログ出力 End *****************************************************************************

            Return False
        End If
    End Function

    ''' <summary>
    ''' セッション存在判定
    ''' </summary>
    ''' <param name="SessionName">判定対象のセッション名</param>
    ''' <returns>True:あり False:なし</returns>
    ''' <remarks>セッション存在を判定</remarks>
    Private Function IsSession(ByVal sessionName As String) As Boolean

        'ログ出力 Start ***************************************************************************
        Logger.Info("IsSession Start")
        'ログ出力 End *****************************************************************************

        If Me.ContainsKey(ScreenPos.Current, sessionName) Then
            If Not String.IsNullOrEmpty(Me.GetValue(ScreenPos.Current, sessionName, False).ToString()) Then

                'ログ出力 Start ***************************************************************************
                Logger.Info("IsSession End")
                'ログ出力 End *****************************************************************************

                Return True
            End If
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info("IsSession End")
        'ログ出力 End *****************************************************************************

        Return False
    End Function
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 登録フラグの取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <remarks>登録フラグ取得</remarks>
    Public Function GetRegistFlg(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal) As String

        Logger.Info("GetRegistFlg Start")

        Dim bizClass As New SC3080201BusinessLogic
        Dim dataSet As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable =
            bizClass.GetSalesSeqNoByRegitFlg(fllwupboxseqno)
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Dim regitFlg As String = String.Empty
        Dim salesSeqNo As Int64 = 0
        If dataSet.Rows.Count > 0 Then
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
            'Dim rw As SC3080201DataSet.SC3020801FllwUpBoxSaleRow
            'rw = CType(dataSet.Rows(0), SC3080201DataSet.SC3020801FllwUpBoxSaleRow)
            'SetValue(ScreenPos.Current, SESSION_KEY_SALES_SEQNO, rw.SALES_SEQNO)
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
            regitFlg = REGISTFLG_NOTREGIST
        Else
            '新規追加用にシーケンスNo取得
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
            'salesSeqNo = bizClass.GetSalesSeqNo()
            'SetValue(ScreenPos.Current, SESSION_KEY_SALES_SEQNO, salesSeqNo)
            ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
            regitFlg = REGISTFLG_REGIST
        End If

        Return regitFlg
        Logger.Info("GetRegistFlg End")

    End Function
    ' 2012/02/15 TCS 相田 【SALES_2】 END
#End Region
    '2012/01/24 TCS 河原 【SALES_1B】 END

    '2012/03/28 TCS 安田 【SALES_2】 START
    ''' <summary>クライアント文字コード</summary>
    Private Const SESSION_KEY_ACCEPT_LANGUAGE As String = "SearchKey.ACCEPT_LANGUAGE"

    Public Overrides Sub ProcessRequest(ByVal context As HttpContext)

        'クライアント文字コードの取得・セッションに設定
        Try
            Dim lung As String = context.Request.ServerVariables("HTTP_ACCEPT_LANGUAGE")
            SetValue(ScreenPos.Current, SESSION_KEY_ACCEPT_LANGUAGE, lung)
        Catch ex As Exception

        End Try

        MyBase.ProcessRequest(context)

    End Sub
    '2012/03/28 TCS 安田 【SALES_2】 END

    ''2012/05/17 TCS 安田 クルクル対応 START

    ' ''' <summary>
    ' ''' 再表示ボタン(隠しボタン)押下時
    ' ''' </summary>
    ' ''' <param name="sender">ページオブジェクト</param>
    ' ''' <param name="e">イベント引数</param>
    ' ''' <remarks></remarks>
    Protected Sub refreshButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles refreshButton.Click

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click Start")
        'ログ出力 End *****************************************************************************

        Dim procFlg As Boolean = False
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail

        'ステータスを「スタンバイ」に更新
        staffInfo.UpdatePresence("1", "0")

        '商談開始　→　商談終了
        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then
            SalesEnd(C_SALES_END)
            procFlg = True
        End If

        '営業活動中　→　営業活動キャンセル
        If String.Equals(staffStatus, STAFF_STATUS_BUSINESS) Then
            RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_NEW)
            SalesEnd(C_BUSINESS_CANCEL)
            procFlg = True
        End If

        '一時対応　→　一時対応終了
        If String.Equals(staffStatus, STAFF_STATUS_CORRESPOND) Then
            SalesEnd(C_CORRESPOND_END)
            procFlg = True
        End If

        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
        '納車作業開始　→　納車作業終了
        If String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then
            SalesEnd(C_DELIVERY_END)
            procFlg = True
        End If

        '納車作業開始(一時対応)　→　納車作業終了(一時対応)
        If String.Equals(staffStatus, STAFF_STATUS_DELIVERYCORRESPOND) Then
            SalesEnd(C_DELIVERYCORRESPOND_END)
            procFlg = True
        End If
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

        'すでに、処理がされている場合 (スタンバイ状態)
        If (procFlg = False) Then

            If (refreshProgramHidden.Value = PROGRAM_SC3080203) Then

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click PROGRAM_SC3080203")
                'ログ出力 End *****************************************************************************

                '活動結果登録
                CType(Sc3080203Page, ISC3080203Control).ChangeFollow()
                '顧客商談
                CType(Sc3080202Page, ISC3080202Control).RefreshSalesCondition()
                '顧客詳細(コンタクト履歴)
                CType(Sc3080201Page, ISC3080201Control).RegistActivityAfter()


            ElseIf (refreshProgramHidden.Value = PROGRAM_SC3080216) Then

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click PROGRAM_SC3080216)")
                'ログ出力 End *****************************************************************************

                '顧客商談
                CType(Sc3080202Page, ISC3080202Control).RefreshSalesCondition()
                '顧客詳細(コンタクト履歴)
                CType(Sc3080201Page, ISC3080201Control).RegistActivityAfter()

                '受注後工程フォロー
                CType(Sc3080216Page, ISC3080203Control).ChangeFollow()

            Else

                'ポップアップメニューからの遷移 (商談開始・営業活動開始等・・)
                '2ページ目の内容を更新
                ''Follow-upBox情報を破棄
                RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES)
                RemoveValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES)

                CType(Sc3080202Page, ISC3080202Control).ReflectionActivityStatus()

            End If

        End If

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''2012/05/17 TCS 安田 クルクル対応 END

    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START

    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 削除

    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

#Region "TKMローカル"

    ''' <summary>
    ''' 現地試乗車画面URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const L_TESTDRIVE_URL As String = "L_TESTDRIVE_URL"

    ''' <summary>
    ''' 基幹コード区分:店舗(2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_TYPE_BRANCH As String = "2"

    ''' <summary>
    ''' プログラム設定検索条件（プログラムコード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_PROGRAM_CD As String = "SC3080201"

    ''' <summary>
    ''' プログラム設定検索条件（設定セクション）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_SECTION As String = "SC3080201"

    ''' <summary>
    ''' プログラム設定検索条件（設定キー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_KEY As String = "DMS_CODE_MAP_BRN_CD"

    ''' <summary>
    ''' 使用基幹コード(基幹コード2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_2 As String = "DMS_CD_2"

    ''' <summary>
    ''' 使用基幹コード(基幹コード3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_3 As String = "DMS_CD_3"

    ''' <summary>
    ''' DMS販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private DMSdlr_cd As String

    ''' <summary>
    ''' DMS店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private DMSbrn_cd As String

    Private Sub AddTestDriveEvent()

        Dim TestDriveURL As String = CreateTestDriveURL()

        'window.location = "icrop:iurl:20::73::980::624::0::" + url;
        Dim sb As New StringBuilder
        sb.Append("window.location = 'icrop:iurl:20::73::980::624::0::")
        sb.Append(TestDriveURL)
        sb.Append("';")

        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).OnClientClick = sb.ToString

    End Sub

    ''' <summary>
    ''' 現地試乗車画面URL作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateTestDriveURL() As String

        Dim TestDriveURL As String = String.Empty    '試乗車画面URL
        Dim Account As String = String.Empty         'ログインユーザーアカウント
        Dim NewCustomerID As String = String.Empty   '顧客ID ※未取引客の場合のみ値を設定する。
        Dim CustomerCode As String = String.Empty    '基幹顧客コード ※自社客の場合のみ値を設定する。
        Dim FollowUpNo As String = String.Empty      '商談ID

        '試乗車画面URL
        Dim dlrenvdt As New SystemEnvSetting
        Dim dlrenvrw As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        dlrenvrw = dlrenvdt.GetSystemEnvSetting(L_TESTDRIVE_URL)
        TestDriveURL = dlrenvrw.PARAMVALUE

        'ログインユーザーアカウント
        Account = StaffContext.Current.Account

        '顧客ID、基幹顧客コード
        If IsSession(SESSION_KEY_CRCUSTID) Then
            Dim crcustId As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString()
            If (Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString()).Equals(ORGCUSTFLG) Then
                '自社客のとき
                CustomerCode = SC3080201BusinessLogic.GetDmsIdOrg(crcustId)
            Else
                '未取引客のとき
                NewCustomerID = crcustId
            End If
        End If

        '商談ID
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            FollowUpNo = CStr(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False))
        End If

        GetDmsCodeMap()

        Dim sb As New StringBuilder

        sb.Append(TestDriveURL)
        sb.Append("?Account=")
        sb.Append(Account)
        sb.Append("&NewCustomerID=")
        sb.Append(NewCustomerID)
        sb.Append("&CustomerCode=")
        sb.Append(CustomerCode)
        sb.Append("&FollowUpNo=")
        sb.Append(FollowUpNo)
        sb.Append("&DealerCode=")
        sb.Append(DMSdlr_cd)
        sb.Append("&BranchCode=")
        sb.Append(DMSbrn_cd)

        Return sb.ToString

    End Function

    ''' <summary>
    ''' 基幹販売店情報取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDmsCodeMap()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContractApproval Start")

        Dim dmsCodeMap As New DmsCodeMap
        'Dim drDmsCodeMap As DmsCodeMapDataSet.DMSCODEMAPRow = dmsCodeMap.GetDmsCodeMap(C_DMS_CODE_TYPE_BRANCH, Trim(StaffContext.Current.BrnCD))
        Dim drDmsCodeMap As DmsCodeMapDataSet.DMSCODEMAPRow = dmsCodeMap.GetDmsCodeMap(C_DMS_CODE_TYPE_BRANCH, Trim(StaffContext.Current.DlrCD), Trim(StaffContext.Current.BrnCD))

        '空のDataTableを用意
        Dim dtDmsCodeMap As New ActivityInfoDataSet.ActivityInfoDMSCODEMAPDataTable

        If Not drDmsCodeMap Is Nothing Then

            'プログラム設定より使用するDMS店舗コードを選択
            Dim programSettingV4 As New ProgramSettingV4
            Dim drProgramSettingV4 As ProgramSettingV4DataSet.PROGRAMSETTINGV4Row = programSettingV4.GetProgramSettingV4(C_DMS_PROGRAM_SETTING_PROGRAM_CD, C_DMS_PROGRAM_SETTING_SETTING_SECTION, C_DMS_PROGRAM_SETTING_SETTING_KEY)

            If Not drProgramSettingV4 Is Nothing Then
                Dim strProgramSettingV4Val As String = drProgramSettingV4.SETTING_VAL
                If C_DMS_CODE_MAP_DMS_CD_3.Equals(strProgramSettingV4Val) Then
                    DMSbrn_cd = drDmsCodeMap.DMS_CD_3
                Else
                    DMSbrn_cd = drDmsCodeMap.DMS_CD_2
                End If
            Else
                'プログラム設定が取得できなかった場合は基幹コード2を取得
                DMSbrn_cd = drDmsCodeMap.DMS_CD_2
            End If

            DMSdlr_cd = drDmsCodeMap.DMS_CD_1

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContractApproval End")

    End Sub

    ''' <summary>
    ''' 試乗車ボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetTestDriveButton()

        '顧客が登録済みか判定
        If IsSession(SESSION_KEY_CRCUSTID) Then
            '試乗ボタンを活性
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Enabled = True

            '試乗ボタン押下時のイベントを定義
            AddTestDriveEvent()
        Else
            '試乗ボタンを非活性
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Enabled = False
        End If

    End Sub

#End Region


End Class
