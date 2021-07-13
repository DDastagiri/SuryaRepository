'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070201.aspx.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2011/12/01 TCS 葛西
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） 
'更新： 2014/07/29 TCS 外崎 不具合対応（TMT BTS-UAT-64）
'更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2018/07/04 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価
'更新： 2019/05/08 TS  舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス）
'更新： 2019/05/20 TS  村井 PostUAT-3114
'更新： 2019/09/24 TS  髙橋(龍) TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1
'更新： 2020/01/29 TS  舩橋 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045)
'─────────────────────────────────────

Imports System.Data
Imports System.Data.SqlTypes
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase
Imports System.Web.Services
'2019/05/20 TS  村井 PostUAT-3114 ADD Start
Imports System.Web.Script.Serialization
'2019/05/20 TS  村井 PostUAT-3114 ADD End
Imports Toyota.eCRB.Estimate.Quotation
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic


''' <summary>
''' 見積作成画面
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3070201
    Inherits BasePage
    Implements ICustomerForm, Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl

#Region "定数定義"

    ''' <summary>
    ''' TRUE
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const StrTrue As String = "TRUE"

    ''' <summary>
    ''' FALSE
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const StrFalse As String = "FALSE"

    ''' <summary>
    ''' メインメニュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_MAINMENU As String = "SC3010203"

    ''' <summary>
    ''' 契約状況フラグ（１：契約済み)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CONTRACTFLG_COMP As String = "1"

    ''' <summary>
    '''価格相談モード（0：通常)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeNormal As String = "0"

    ''' <summary>
    '''価格相談モード（1：マネージャ価格相談)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeApprovalManager As String = "1"

    ''' <summary>
    '''価格相談モード（2:スタッフ回答参照)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeApprovalStaff As String = "2"

    ''' <summary>
    '''CR活動結果(SUCCESS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_SUCCESS As String = "3"

    ''' <summary>
    '''CR活動結果(GIVEUP)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_GIVEUP As String = "5"

    ''' <summary>
    '''CR活動結果(ENQUIRY_COMPLETED)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_ENQUIRY_COMPLETED As String = "6"

    ''' <summary>
    ''' フッターメニュー番号（TCV_車両紹介)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_CARINVITATION As Integer = 301
    ''' <summary>
    ''' フッターメニュー番号（TCV_諸元表)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_ORIGINALLIST As Integer = 302
    ''' <summary>
    ''' フッターメニュー番号（TCV_競合車比較)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_COMPARECOMPETITOR As Integer = 303
    ''' <summary>
    ''' フッターメニュー番号（TCV_ライブラリ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_LIBRARY As Integer = 304
    ''' <summary>
    ''' フッターメニュー番号（TCV_見積もり)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_QUOTATION As Integer = 305
    '2014/05/08 NextStep フッターボタン追加対応 TCS 森 START
    ''' <summary>
    ''' フッターメニュー番号（TCV_受注時説明ツール）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_JUTYU As Integer = 306
    '2014/05/08 NextStep フッターボタン追加対応 TCS 森 END

    ''' <summary>
    ''' TCV（車種選択）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_SELECTSERIES As String = "SC3050101"
    ''' <summary>
    ''' TCV（車両紹介）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_CARINVITATION As String = "SC3050201"
    ''' <summary>
    ''' TCV（諸元表）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_ORIGINALLIST As String = "SC3050301"
    ''' <summary>
    ''' TCV（競合車比較）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_COMPARECOMPETITOR As String = "SC3050401"
    ''' <summary>
    ''' TCV（ライブラリ）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_LIBRARY As String = "SC3050501"

    ''' <summary>
    ''' ショールーム画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_SHOWROOM As String = "SC3100101"

    ''' <summary>
    ''' TCVコールバック関数(クローズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CALLBACKMETHOD_CLOSE As String = "icropScript.tcvCallback"

    ''' <summary>
    ''' TCVコールバック関数(ステータス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CALLBACKMETHOD_STATUS As String = "statusCallbackFunction"

    ''' <summary>
    ''' 実行モード（１：契約）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT As String = "1"

    ''' <summary>
    ''' 実行モード（2：メインメニュー移動）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_MENU As String = "2"

    ''' <summary>
    ''' 処理区分（見積切替）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ESTIMATE_CHANGE As String = "3"

    ''' <summary>
    ''' 処理区分（見積書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ESTIMATE_PRINT As String = "4"

    ''' <summary>
    ''' 処理区分（契約書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_PRINT As String = "5"

    ''' <summary>
    ''' 処理区分（契約確定）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_SEND As String = "6"

    ''' <summary>
    ''' 処理区分（契約キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_CANCEL As String = "7"

    ''' <summary>
    ''' 処理区分（注文書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ORDER_PRINT As String = "8"

    ''' <summary>
    ''' TCVパラメータ（データ読み込み元）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ESTIMATEID As String = "EstimateId"

    ''' <summary>
    ''' 通知依頼情報・最終ステータス（2:キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_REQ_STATUS_CANCEL As String = "2"

    ''' <summary>
    ''' 在庫情報IF（希望車名："SearchKey.CAR_NAME"）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFNAME_CAR_NAME As String = "SearchKey.CAR_NAME"

    ''' <summary>
    ''' 在庫情報IF（希望グレード："SearchKey.GRADE"）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFNAME_GRADE As String = "SearchKey.GRADE"

    ''' <summary>
    ''' 在庫情報IF（希望サフィックス："SearchKey.SFX"）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFNAME_SFX As String = "SearchKey.SFX"

    ''' <summary>
    ''' 在庫情報IF（希望外装色："SearchKey.COLOR_NAME"）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFNAME_COLOR_NAME As String = "SearchKey.COLOR_NAME"

    ''' <summary>
    ''' 在庫情報IF（承認モード："EstimateMode.Approval"）
    ''' </summary>
    ''' <remarks>"0": 承認モードでない、"1": 承認モード</remarks>
    Private Const IFNAME_APPROVAL As String = "EstimateMode.Approval"

    ''' <summary>
    ''' 在庫情報IF（承認モード："EstimateMode.Approval"）
    ''' </summary>
    ''' <remarks>
    '''  "0": 価格相談回答時でない（価格相談回答エリアが非表示、または参照モードで表示）
    '''  "1": 価格相談回答時（価格相談回答エリアが編集モードで表示）
    ''' </remarks>
    Private Const IFNAME_PRICE_APPROVAL As String = "EstimateMode.PriceApproval"

    ''' <summary>
    ''' セッションキー(FollowUpBoxNo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' 価格相談ボタン初期表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISP_DISCOUNT_APPROVAL_BUTTON As String = "DISP_DISCOUNT_APPROVAL_BUTTON"

    ''' <summary>
    ''' 見積作成画面URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATEINFO_URL As String = "ESTIMATEINFO_URL"

    ''' <summary>
    ''' 見積作成画面(承認用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATEINFO_APPROVAL_URL = "ESTIMATEINFO_APPROVAL_URL"

    ''' <summary>
    ''' 印刷ボタン使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USED_FLG_PRINTBUTTON = "USED_FLG_PRINTBUTTON"

    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 START
    ''' <summary>
    ''' 納車時説明ツール遷移用 顧客車両区分 所有者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CST_VCL_TYPE_TABLET = "1"

    ''' <summary>
    ''' 受注時説明表示モード お客様説明モード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESBOOKING_DESCRIPTION_VIEWMODE_CST = "1"
    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 END

    ''' <summary>
    ''' 注文承認を表示中フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeatchKeyApprovalActive As String = "ApprovalActive"

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    '他シス連携
    Private Const LINK_MENU As Integer = FooterMenuCategory.LinkMenu
    'リンク先URL
    Private Const C_LINK_MENU_URL As String = "LINK_MENU_URL"
    'URLスキーム
    Private Const URL_SCHEME As String = "TABLET_BROWSER_URL_SCHEME"
    Private Const URL_SCHEMES As String = "TABLET_BROWSER_URL_SCHEMES"
    ' 自社客/未取引客フラグ (1：自社客)
    Private Const ORGCUSTFLG As String = "1"
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END
    '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START

    ''' <summary>
    ''' Order画面呼び出しフラグ　2：Order画面呼び出し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderDispFlg = "2"

    ''' <summary>
    ''' 見積作成画面URL (Order画面)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATEINFO_ORDER_URL = "ESTIMATEINFO_ORDER_URL"

    ''' <summary> セッションキー Order画面呼び出しフラグ　1：Order画面呼び出し</summary>
    Public Const SESSION_KEY_ORDERDISPFLG As String = "EST_DISP_TYPE"

    '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

    '2019/05/20 TS  村井 PostUAT-3114 ADD Start
    ''' <summary>
    ''' マスターページ文言取得ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MSTPG_DISPLAYID As String = "MASTERPAGEMAIN"
    '2019/05/20 TS  村井 PostUAT-3114 ADD End


#End Region

#Region "メンバ変数"

    Private commonMasterPage As CommonMasterPage
    Private mainMenuButton As CommonMasterFooterButton
    Private customerButton As CommonMasterFooterButton
    Private BtnPrint As LinkButton
    Private BtnDiscountApproval As LinkButton

    'Protected dlrOptionCount As Integer                                                              '販売店オプションID用
    'Protected Property dlrOptionDataTable As IEnumerable = New List(Of Integer)                         '販売店オプションテーブル
    'Protected tradeInCarCount As Integer                                                                '下取り車両ID用
    'Private tradeInCarDataTable As DataTable                     '下取り車両テーブル
    'Private tcvMkrOptionDataTable As DataTable                  'TCVメーカーオプションテーブル
    'Private tcvDlrOptionDataTable As DataTable                  'TCV販売店オプションテーブル
    'Private chargeInfoDataTable As DataTable                     '諸費用テーブル

    '2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価 DEL
    Private PriceConsultationAnswerPage As Pages_SC3070206      '価格相談回答     SC3070206
    Private OrderConfirmPage As Pages_SC3070207                 '注文承認         SC3070207

    Private ReadOnly Property IsFreezed As Boolean
        Get
            '承認依頼中もしくは承認済の場合、見積変更不可
            If (Me.contractApprovalSatus.Value.Equals("1") OrElse Me.contractApprovalSatus.Value.Equals("2")) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

#End Region

#Region " フッターボタンイベント "

    '2019/05/20 TS  村井 PostUAT-3114 DEL

    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub MainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        ''メインメニューへ遷移
        Me.RedirectNextScreen(STR_DISPID_MAINMENU)


    End Sub

    ''' <summary>
    ''' 顧客詳細画面へ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub CustomerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '顧客詳細画面に渡す引数を設定
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FLLWUPBOX_STRCD", Me.strStrCdHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FOLLOW_UP_BOX", Me.lngFollowupBoxSeqNoHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CSTKIND", Me.strCstKindHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CUSTOMERCLASS", Me.strCustomerClassHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CRCUSTID", Me.strCRCustIdHiddenField.Value)

        Me.RedirectNextScreen("SC3080201")

    End Sub

    ''' <summary>
    ''' 編集ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EstimateEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EstimateEditButton.Click

        Dim estimateId As Long = CType(Me.lngEstimateIdHiddenField.Value, Long)

        Dim rslt As Boolean
        Dim messageId As Long = 0

        Dim bizLogic As New SC3070201BusinessLogic
        rslt = bizLogic.UpdateContractApprovalStatus(estimateId, messageId)

        If rslt Then
            EditButton.Style.Item("display") = "none"
        Else
            If messageId <> 0 Then
                ShowMessageBox(messageId)
            End If
        End If

    End Sub

    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 START
    ''' <summary>
    ''' SPMへ遷移する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub spmButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        'SPMへ遷移する
        'SPMへのパラメータ設定はなし

    End Sub

    ''' <summary>
    ''' 受注時説明ツールへ遷移する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub jutyuuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '受注時説明ツールへ遷移するためのパラメータを設定する
        Logger.Info("jutyuuButton_Click Start")
        'パラメータ設定

        '商談ID
        Me.SetValue(ScreenPos.Next, "SalesId", Me.lngFollowupBoxSeqNoHiddenField.Value)

        '見積管理ID
        If Me.contractApprovalSatus.Equals("2") Then

            '契約承認済みの場合は見積管理IDを設定しない
            '※承認済みの場合、商談IDで見積管理IDを特定できるため
            Me.SetValue(ScreenPos.Next, "EstimateId", "")

        Else
            Me.SetValue(ScreenPos.Next, "EstimateId", Me.lngEstimateIdHiddenField.Value)
        End If

        '受注時説明表示モード
        Me.SetValue(ScreenPos.Next, "SalesbookingDescriptionViewMode", SALESBOOKING_DESCRIPTION_VIEWMODE_CST)

        '契約条件変更フラグ
        Me.SetValue(ScreenPos.Next, "ContractAskChgFlg", "")

        '顧客ID
        Me.SetValue(ScreenPos.Next, "CstId", Me.strCRCustIdHiddenField.Value)

        '顧客種別
        Me.SetValue(ScreenPos.Next, "CstType", Me.strCstKindHiddenField.Value)

        '顧客車両区分
        Me.SetValue(ScreenPos.Next, "CstVclType", CST_VCL_TYPE_TABLET)

        '受注時説明画面へ遷移
        Me.RedirectNextScreen("SC3270101")

        Logger.Info("jutyuuButton_Click End")

    End Sub

    ''' <summary>
    ''' 納車時説明ツールへ遷移する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub nousyaButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("nousyaButton_Click Start")

        '納車時説明ツールへ遷移するためのパラメータを設定する

        ' 商談ID
        Me.SetValue(ScreenPos.Next, "SalesId", Me.lngFollowupBoxSeqNoHiddenField.Value)

        ' 顧客ID
        Me.SetValue(ScreenPos.Next, "CstId", Me.strCRCustIdHiddenField.Value)

        ' 顧客種別
        Me.SetValue(ScreenPos.Next, "CstType", Me.strCstKindHiddenField.Value)

        ' 顧客車両区分
        Me.SetValue(ScreenPos.Next, "CstVclType", CST_VCL_TYPE_TABLET)

        Logger.Info("nousyaButton_Click End")
    End Sub

    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 END

#End Region

#Region "画面イベント"
    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub SC3070201_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3070201_Load Start")

        '2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価 DEL

        '相談履歴エリア表示
        RequestHistory.Controls.Add(LoadControl("~/Pages/SC3070210.ascx"))

        '契約承認状況取得
        GetContractApproval()

        '注文承認エリア表示
        setOrderConfirmArea()

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
        Else
            SetSessionReload()
        End If

        '価格相談モード設定
        InitApprovalMode()

        'セッション値読み込みと読取専用フラグ判定
        InitTcvParam()

        'ヘッダーボタン定義
        InitHeaderEvent()

        'フッターボタン定義
        InitFooterEvent()

        '2019/05/17 TS 舩橋 PostUAT-3092 START
        SessionInfoEstimateIdRefresh()
        '2019/05/17 TS 舩橋 PostUAT-3092 END

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then

            '初期設定
            InitialSetting()

            '初期データ取得、表示
            DispInitData()

            '2014/05/07 NextStep フッターボタン追加対応 TCS 森 START
            '受注時説明ツールボタン表示設定
            InitJutyuuButton()
            '2014/05/07 NextStep フッターボタン追加対応 TCS 森 END

            '画面モード判定
            DispModeSetting()

            If Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) And Not String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
                '活動に紐づく見積管理IDをセッションに設定
                SetEstimateIdSession()
                '見積管理IDをHIDDEN値に設定
                SetEstimateIdHidden()
            End If

            '検索ボックス設定
            InitSearchBox(CType(Me.businessFlgHiddenField.Value, Boolean))

            '2019/04/17 TS 村井 (FS)次世代タブレット新興国向けの性能評価 DEL

            '遷移時状態チェック
            CheckStatus()
        Else
            'メインメニュー移動モード
            If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_MENU) Then

                'メインメニューへ遷移
                Me.RedirectNextScreen(STR_DISPID_MAINMENU)

            End If

            If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_CHANGE) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_PRINT) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_PRINT) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_SEND) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_CANCEL) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ORDER_PRINT) Then

                If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_CHANGE) Then
                    '見積管理IDをHIDDEN値に設定
                    SetEstimateIdHidden()
                End If

                '初期データ取得、表示
                DispInitData()

                '画面モード判定
                DispModeSetting()

                '2019/04/17 TS 村井 (FS)次世代タブレット新興国向けの性能評価 DEL

                '遷移時状態チェック
                CheckStatus()

                Me.actionModeHiddenField.Value = vbEmpty

            Else

            End If

        End If


        If Not CType(Me.businessFlgHiddenField.Value, Boolean).Equals(IsSales()) Then
            'BusinessFlgとユーザのステータスが異なる場合、エラー表示後メニューへ遷移
            Me.actionModeHiddenField.Value = String.Empty
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "dispLoading();alert(SC3070201HTMLDecode(""" + HttpUtility.HtmlEncode(WebWordUtility.GetWord(985)) + """));this_form.actionModeHiddenField.value = ""2"";this_form.submit();", True)
            Exit Sub
        End If

        If String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
            '活動がない場合
            '印刷ボタン表示
            BtnPrint.Visible = True
            PrintButton.Style.Item("display") = "block"
            '価格相談ボタン非表示
            BtnDiscountApproval.Visible = False
            ApprovalButton.Style.Item("display") = "none"
        End If

        '顧客担当セールススタッフコード取得
        If Not String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Then
            Dim bizLogic As New SC3070201BusinessLogic
            Me.staffCd.Value = bizLogic.GetStaffCd(Me.strCRCustIdHiddenField.Value)
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3070201_Load End")

    End Sub

    ''' <summary>
    ''' PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRender Start")

        '注文承認依頼状況取得
        GetContractApproval()

        'セッション値読み込みと読取専用フラグ判定
        InitTcvParam()

        '画面モード判定
        DispModeSetting()

        '見積作成画面URL設定
        SetEstimateInfoURL()

        'フッターボタン表示制御
        RenderFooterEvent()

        '編集ボタン制御
        InitEditButton()

        '注文承認依頼ボタン制御
        InitContractApprovalButton()

        '価格相談ボタン制御
        InitPriceApprovalButton()

        '印刷ボタン制御
        InitPrintButton()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRender End")

        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
        '新車納車システム連携メニュー
        Dim linkMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(LINK_MENU)
        If linkMenuButton.Visible Then
            ''リンク先URLを販売店環境設定TBLより取得
            Dim dlrenvdt As New DealerEnvSetting
            Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
            dlrenvrw = dlrenvdt.GetEnvSetting(StaffContext.Current.DlrCD, C_LINK_MENU_URL)
            ''システム環境設定より別ブラウザのURLスキーム取得。
            Dim sysenv As New SystemEnvSetting
            Dim rw1 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEME)
            Dim rw2 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEMES)
            ''新車納車システムへのパラメータ取得。
            Dim parmDmsId As String = String.Empty
            Dim parmContNo As String = String.Empty
            ''活動先顧客コードが存在するか(メイン画面からの直接見積もり実施でないか)
            Dim crcustId As String = Me.strCRCustIdHiddenField.Value
            If Not String.IsNullOrEmpty(crcustId) Then
                ''注文番号に画面の値をセット
                parmContNo = Me.contractNoHidden.Value
                ''DMSIDを取得
                If Me.strCstKindHiddenField.Value.Equals(ORGCUSTFLG) Then
                    ''自社客のとき
                    parmDmsId = SC3070201BusinessLogic.GetDmsIdOrg(crcustId)
                Else
                    ''未取引客のとき
                    If Not String.IsNullOrEmpty(parmContNo) Then
                        ''未取引客、かつ受注後工程(注文番号が取得できた)
                        parmDmsId = SC3070201BusinessLogic.GetDmsIdNew(StaffContext.Current.DlrCD, parmContNo)
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
        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRender End")

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

    ''' <summary>
    ''' 画面描画直前イベント全完了時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRenderComplete(sender As Object, e As System.EventArgs) Handles Me.PreRenderComplete
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRenderComplete Start")

        'MG通知一覧制御
        InitMGInfoList()

        '2019/05/20 TS  村井 PostUAT-3114 ADD Start
        'フッターTcv(車両選択)ボタン
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)

        If tcvButton.Enabled Then
            'JavaScriptの埋め込み
            Dim openTcvScript As String = BuildOpenTcvScript()
            JavaScriptUtility.RegisterStartupScript(Me, openTcvScript, "openTcv", True)
        End If

        'フッターTcv(車両紹介)ボタン
        Dim carInvitation As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_CARINVITATION)

        If carInvitation.Enabled Then
            'JavaScriptの埋め込み
            Dim carInvitationscript As String = BuildOpenCarInvitationScript()
            JavaScriptUtility.RegisterStartupScript(Me, carInvitationscript, "carInvitation", True)
        End If

        'フッターTcv(諸元表)ボタン
        Dim originalList As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST)

        '2019/10/04 TS 舩橋 TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 START
        If originalList.Enabled And originalList.Visible Then
            '2019/10/04 TS 舩橋 TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 END
            'JavaScriptの埋め込み
            Dim originalListScript As String = BuildOpenOriginalListScript()
            JavaScriptUtility.RegisterStartupScript(Me, originalListScript, "originalList", True)
        End If

        'フッターTcv(競合車比較)ボタン
        Dim compareCompetitor As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR)

        '2019/10/04 TS 舩橋 TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 START
        If compareCompetitor.Enabled And compareCompetitor.Visible Then
            '2019/10/04 TS 舩橋 TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 END
            'JavaScriptの埋め込み
            Dim compareCompetitorScript As String = BuildOpenCompareCompetitorScript()
            JavaScriptUtility.RegisterStartupScript(Me, compareCompetitorScript, "compareCompetition", True)
        End If

        'フッターTcv(ライブラリ)ボタン
        Dim library As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_LIBRARY)

        If library.Enabled Then
            'JavaScriptの埋め込み
            Dim libraryScript As String = BuildOpenLibraryScript()
            JavaScriptUtility.RegisterStartupScript(Me, libraryScript, "library", True)
        End If
        '2019/05/20 TS  村井 PostUAT-3114 ADD End

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRenderComplete End")
    End Sub
#End Region

    ''' <summary>
    ''' 見積作成画面URL設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetEstimateInfoURL()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateInfoURL Start")

        Dim EstimateInfoUrl As String
        Dim EstimateInfoApprovalUrl As String

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow

        '見積作成画面のURLを取得
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, ESTIMATEINFO_URL)
        If IsNothing(dtrow) Then '取得できなかった場合
            EstimateInfoUrl = "SC3070205.aspx"
        Else
            EstimateInfoUrl = dtrow.PARAMVALUE '日付の計算
        End If
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, ESTIMATEINFO_APPROVAL_URL)
        If IsNothing(dtrow) Then '取得できなかった場合
            EstimateInfoApprovalUrl = "SC3070205.aspx"
        Else
            EstimateInfoApprovalUrl = dtrow.PARAMVALUE '日付の計算
        End If

        '基幹側の販売店、店舗コード取得
        Dim DmsCodeMapDt As SC3070201DataSet.SC3070201DMSCODEMAPDataTable
        Dim bizLogic As New SC3070201BusinessLogic
        DmsCodeMapDt = bizLogic.GetDmsCodeMap()
        Dim DmsCodeMapRw As SC3070201DataSet.SC3070201DMSCODEMAPRow
        DmsCodeMapRw = DmsCodeMapDt.Rows(0)

        'ログインユーザーアカウント
        Dim Account As New StringBuilder
        Account.Append("Account=")
        Account.Append(StaffContext.Current.Account)

        'ログインユーザー販売店コード
        Dim Dlrcd As New StringBuilder
        Dlrcd.Append("Dlrcd=")
        Dlrcd.Append(DmsCodeMapRw.DMS_CD_1)

        'ログインユーザー店舗コード
        Dim Strcd As New StringBuilder
        Strcd.Append("Strcd=")
        Strcd.Append(DmsCodeMapRw.DMS_CD_2)

        '見積もりID

        '不要な,を削除
        Dim tempEstimateIdAry As String()
        tempEstimateIdAry = Split(Me.estimateIdHiddenField.Value, ",")
        Dim tempEstimateId As New StringBuilder
        For i = 0 To tempEstimateIdAry.Length - 1
            If tempEstimateIdAry(i) <> "" Then
                tempEstimateId.Append(tempEstimateIdAry(i))
                If i <> tempEstimateIdAry.Length - 1 And i <> tempEstimateIdAry.Length - 1 AndAlso tempEstimateIdAry(i + 1) <> "" Then
                    tempEstimateId.Append(",")
                End If
            End If
        Next

        Dim EstimateId As New StringBuilder
        EstimateId.Append("EstimateId=")
        EstimateId.Append(tempEstimateId)

        '選択中の見積もりID
        Dim SelectedEstimateId As New StringBuilder
        SelectedEstimateId.Append("SelectedEstimateId=")
        SelectedEstimateId.Append(Me.lngEstimateIdHiddenField.Value)

        '商談中フラグ
        Dim SalesFlg As New StringBuilder
        SalesFlg.Append("SalesFlg=")
        If IsSales() Then
            SalesFlg.Append("1")
        Else
            SalesFlg.Append("0")
        End If

        '画面表示モード
        Dim DispModeFlg As New StringBuilder
        DispModeFlg.Append("DispModeFlg=")
        If OperationLocked Then
            DispModeFlg.Append("3")
        ElseIf Me.ReferenceModeHiddenField.Value Then
            DispModeFlg.Append("2")
        Else
            DispModeFlg.Append("1")
        End If

        '契約承認ステータス
        Dim ApprovalStatus As New StringBuilder
        ApprovalStatus.Append("ApprovalStatus=")

        '0: 未承認、1: 承認依頼中、2: 承認、3: 否認
        Select Case Me.contractApprovalSatus.Value
            Case " "
                ApprovalStatus.Append("0")
            Case "0"
                ApprovalStatus.Append("0")
            Case "1"
                ApprovalStatus.Append("1")
            Case "2"
                ApprovalStatus.Append("2")
            Case "3"
                ApprovalStatus.Append("0")
        End Select

        '顧客未指定フラグ
        Dim NoCustomerFlg As New StringBuilder
        NoCustomerFlg.Append("NoCustomerFlg=")
        If String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Then
            NoCustomerFlg.Append("1")
        Else
            NoCustomerFlg.Append("0")
        End If

        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        '直販フラグ
        Dim DirectBillingFlag As New StringBuilder
        DirectBillingFlag.Append("DirectBillingFlag=")
        If String.IsNullOrWhiteSpace(Me.DirectBillingFlag.Value) Then
            DirectBillingFlag.Append("0")
        Else
            DirectBillingFlag.Append(Me.DirectBillingFlag.Value)
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SetEstimateInfoURL ■CST_TYPE:＞" + Me.strCstKindHiddenField.Value + "＜")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SetEstimateInfoURL ■CST_CD:＞" + Me.strCRCustIdHiddenField.Value + "＜")
        Dim CustomerID As New StringBuilder
        If (Me.strCstKindHiddenField.Value.Equals(ORGCUSTFLG)) Then
            '1:自社客　の場合
            '顧客コード
            CustomerID.Append("CustomerCode=")
            CustomerID.Append(SC3070201BusinessLogic.GetDmsIdOrg(Me.strCRCustIdHiddenField.Value))
        Else
            '2:未取引客　の場合
            '顧客ID
            CustomerID.Append("NewCustomerID=")
            CustomerID.Append(Me.strCRCustIdHiddenField.Value)
        End If
        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        'URL作成
        Dim sb As New StringBuilder

        '注文承認依頼中
        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        Dim tcvOrderDispFlg As String
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_ORDERDISPFLG) Then
            tcvOrderDispFlg = CType(Me.GetValue(ScreenPos.Current, SESSION_KEY_ORDERDISPFLG, False), String)
        Else
            tcvOrderDispFlg = String.Empty
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SetEstimateInfoURL ■OrderFlg ContainsKey:＞" + Me.ContainsKey(ScreenPos.Current, SESSION_KEY_ORDERDISPFLG).ToString() + "＜")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SetEstimateInfoURL ■OrderFlg Value:＞" + tcvOrderDispFlg + "＜")
        If (OrderDispFlg.Equals(tcvOrderDispFlg)) Then
            dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, ESTIMATEINFO_ORDER_URL)
            If IsNothing(dtrow) Then
                sb.Append("SC3070205.aspx")
            Else
                sb.Append(dtrow.PARAMVALUE)
            End If
        Else
            ' 2020/01/29 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) Start
            If (Me.ContainsKey(ScreenPos.Current, "NoticeReqId")) Then
                dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, ESTIMATEINFO_ORDER_URL)
                If IsNothing(dtrow) Then
                    sb.Append("SC3070205.aspx")
                Else
                    sb.Append(dtrow.PARAMVALUE)
                End If
                ' 2020/01/29 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) End
            Else
                sb.Append(EstimateInfoUrl)
            End If
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SetEstimateInfoURL ■EstimatDispID:＞" + sb.ToString() + "＜")
        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        sb.Append("?")
        sb.Append(Account)
        sb.Append("&")
        sb.Append(Dlrcd)
        sb.Append("&")
        sb.Append(Strcd)
        sb.Append("&")
        sb.Append(EstimateId)
        sb.Append("&")
        sb.Append(SelectedEstimateId)
        sb.Append("&")
        sb.Append(SalesFlg)
        sb.Append("&")
        sb.Append(DispModeFlg)
        sb.Append("&")
        sb.Append(ApprovalStatus)
        sb.Append("&")
        sb.Append(NoCustomerFlg)
        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        sb.Append("&")
        sb.Append(DirectBillingFlag)
        sb.Append("&")
        sb.Append(CustomerID)
        '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
        'iframeのURL更新
        Me.EstimateInfoURL.Value = sb.ToString

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateInfoURL End")

    End Sub

    ''' <summary>
    ''' 契約承認状況取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetContractApproval()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval Start")

        'セッションより見積管理ID取得
        Dim selectedEstimateIndex As Long
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If
        Dim estimateId As Long = GetSelectedEstimateId(CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String), selectedEstimateIndex)

        '契約承認状況取得
        Dim ContractApprovalDt As SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable
        Dim bizLogic As New SC3070201BusinessLogic
        ContractApprovalDt = bizLogic.GetContractApproval(estimateId)
        Dim ContractApprovalRw As SC3070201DataSet.SC3070201CONTRACTAPPROVALRow
        ContractApprovalRw = ContractApprovalDt.Rows(0)
        Me.contractApprovalSatus.Value = ContractApprovalRw.CONTRACT_APPROVAL_STATUS
        If Not ContractApprovalRw.IsCONTRACT_APPROVAL_STAFFNull Then
            Me.contractApprovalStaff.Value = ContractApprovalRw.CONTRACT_APPROVAL_STAFF
        End If
        If Not ContractApprovalRw.IsCONTRACT_APPROVAL_REQUESTSTAFFNull Then
            Me.contractApprovalRequestStaff.Value = ContractApprovalRw.CONTRACT_APPROVAL_REQUESTSTAFF
        End If
        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
        If Not ContractApprovalRw.IsCONTRACTNONull Then
            Me.contractNoHidden.Value = ContractApprovalRw.CONTRACTNO
        End If
        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

        '承認中／承認済の場合、見積りはひとつのみ
        'If (Me.IsFreezed) Then
        '    Me.estimateIdHiddenField.Value = estimateId.ToString()
        '    Me.selectedEstimateIndexHiddenField.Value = "0"
        'End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval End")

    End Sub

    ''' <summary>
    ''' 注文承認エリア表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setOrderConfirmArea()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("setOrderConfirmArea Start")

        '通知からきて承認依頼中の場合もしくは承認依頼中で依頼元、依頼先が自分の場合
        'ロック中の場合は非表示
        Dim OrderConfirmPageAddFlg As Boolean = False

        '2014/07/29 TCS 外崎 不具合対応（TMT BTS-UAT-64）START
        'If (String.Equals(Me.contractApprovalSatus.Value, "1")) And _
        '   (OperationLocked = False) Then
        If String.Equals(Me.contractApprovalStaff.Value, StaffContext.Current.Account) _
            And (String.Equals(Me.contractApprovalSatus.Value, "1")) _
            And (OperationLocked = False) Then

            Dim noticeReq As IC3070201DataSet.IC3070201NoticeRequestRow = Nothing
            If (Me.ContainsKey(ScreenPos.Current, "NoticeReqId")) Then
                Dim noticeReqId As Long = CLng(Me.GetValue(ScreenPos.Current, "NoticeReqId", False))
                Dim bizLogic As New SC3070201BusinessLogic
                noticeReq = bizLogic.GetNoticeRequest(noticeReqId)
            End If

            'If ((noticeReq IsNot Nothing AndAlso noticeReq.NOTICEREQCTG = "08") _
            ' OrElse (String.Equals(Me.contractApprovalStaff.Value, Me.contractApprovalRequestStaff.Value) And String.Equals(Me.contractApprovalStaff.Value, StaffContext.Current.Account))) Then
            If ((noticeReq IsNot Nothing AndAlso noticeReq.NOTICEREQCTG = "08") _
                OrElse (String.Equals(Me.contractApprovalStaff.Value, Me.contractApprovalRequestStaff.Value))) Then

                OrderConfirmPageAddFlg = True
                OrderConfirmPage = CType(LoadControl("~/Pages/SC3070207.ascx"), Pages_SC3070207)
                OrderConfirm.Controls.Add(OrderConfirmPage)
                Me.OrderConfirmArea.Visible = True

                '注文承認を表示中フラグをセットする
                MyBase.SetValue(ScreenPos.Current, SeatchKeyApprovalActive, "1")
            End If
        End If
        '2014/07/29 TCS 外崎 不具合対応（TMT BTS-UAT-64）END

        '注文承認を表示中フラグがあれば表示する
        '注文承認回答中の場合はロード処理をしないと注文承認が表示されないため
        '注文承認回答中に、注文承認回答キャンセルがされた場合の対応
        If (MyBase.ContainsKey(ScreenPos.Current, SeatchKeyApprovalActive) = True) Then
            If (OrderConfirmPageAddFlg = False) Then
                OrderConfirmPage = CType(LoadControl("~/Pages/SC3070207.ascx"), Pages_SC3070207)
                OrderConfirm.Controls.Add(OrderConfirmPage)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("OrderConfirmPage Add")
            End If
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("setOrderConfirmArea End")

    End Sub

    ''' <summary>
    ''' 再表示時セッション設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSessionReload()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionReload Start")

        If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_CHANGE) Then
            '見積切替時
            MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", Me.selectedEstimateIndexHiddenField.Value)
            MyBase.RemoveValue(ScreenPos.Current, "NoticeReqId")
        ElseIf Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_SEND) Then
            '契約確定時
            MyBase.SetValue(ScreenPos.Current, "EstimateId", Me.estimateIdHiddenField.Value)
            MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", Me.selectedEstimateIndexHiddenField.Value)
        ElseIf Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_CANCEL) Then
            '契約キャンセル時
            '活動に紐づく見積管理IDをセッションに設定
            SetEstimateIdSession()
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionReload End")
    End Sub

    ''' <summary>
    ''' 活動に紐づく見積管理IDをSessionに設定
    ''' </summary>
    ''' <remarks>フォローアップBoxに該当する見積管理IDを全て取得し、セッションに格納する</remarks>
    Private Sub SetEstimateIdSession()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession Start")

        Dim bizLogic As SC3070201BusinessLogic
        Dim dsEstimateId As SC3070201DataSet.SC3070201EstimateIdDataTable   '活動に紐づく見積管理ID格納用
        Dim lngEstimateId As Long
        Dim selectedEstimateIndex As Long
        Dim estimateId As New StringBuilder
        Dim i As Long

        lngEstimateId = CType(Me.lngEstimateIdHiddenField.Value, Long)

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070201BusinessLogic

        '活動に紐づく全ての見積管理IDを取得
        dsEstimateId = bizLogic.GetEstimateId(CType(Me.strDlrcdHiddenField.Value, String), CType(Me.strStrCdHiddenField.Value, String), CType(Me.lngFollowupBoxSeqNoHiddenField.Value, Decimal))

        For i = 0 To dsEstimateId.Rows.Count - 1
            If Not String.IsNullOrEmpty(estimateId.ToString) Then
                estimateId.Append(",")
            End If
            estimateId.Append(dsEstimateId(i).Item("ESTIMATEID"))
            If dsEstimateId.Rows(i).Item("ESTIMATEID").Equals(lngEstimateId) Then
                '選択している見積管理IDのIndex設定
                selectedEstimateIndex = i
            End If
        Next

        '見積管理ID(カンマ区切り）の設定
        Me.estimateIdHiddenField.Value = CType(estimateId.ToString, String)

        'セッション情報格納
        MyBase.SetValue(ScreenPos.Current, "EstimateId", estimateId.ToString)
        MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", selectedEstimateIndex)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession End")
    End Sub


    ''' <summary>
    ''' 価格相談モード判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitApprovalMode()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitApprovalMode Start")

        Me.strApprovalModeHiddenField.Value = ModeNormal

        'セッション情報取得（通知依頼IDがセットされている場合は価格相談モード＝通知一覧より起動）
        If ((Me.ContainsKey(ScreenPos.Current, "NoticeReqId")) AndAlso (Not String.Equals(Me.contractApprovalSatus.Value, "1"))) Then
            Me.noticeReqIdHiddenField.Value = CType(Me.GetValue(ScreenPos.Current, "NoticeReqId", False), String)

            Dim bizLogic As New SC3070201BusinessLogic
            Dim noticeReq As IC3070201DataSet.IC3070201NoticeRequestRow = bizLogic.GetNoticeRequest(CLng(Me.noticeReqIdHiddenField.Value))

            If (noticeReq.NOTICEREQCTG = "02") Then
                '価格相談回答エリア表示
                PriceConsultationAnswerPage = CType(LoadControl("~/Pages/SC3070206.ascx"), Pages_SC3070206)
                PriceApproval.Controls.Add(PriceConsultationAnswerPage)
                Me.PriceApprovalArea.Visible = True

                If (noticeReq.STATUS = "3") Then
                    Me.strApprovalModeHiddenField.Value = ModeApprovalManager
                    PriceConsultationAnswerPage.EditMode = True
                Else
                    Me.strApprovalModeHiddenField.Value = ModeApprovalStaff
                    PriceConsultationAnswerPage.EditMode = False
                End If
            End If
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitApprovalMode End")
    End Sub

    ''' <summary>
    ''' 見積管理IDをHiddenに設定
    ''' </summary>
    ''' <remarks>見積管理IDをHiddenに格納する</remarks>
    Private Sub SetEstimateIdHidden()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden Start")

        Dim estimateId As String
        Dim selectedEstimateIndex As Long
        Dim lngEstimateId As Long               '見積管理ID

        estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)

        '選択している見積IDのIndex
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If

        '選択している見積ID
        lngEstimateId = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

        Me.lngEstimateIdHiddenField.Value = CType(lngEstimateId, String)
        Me.estimateIdHiddenField.Value = CType(estimateId, String)
        Me.selectedEstimateIndexHiddenField.Value = CType(selectedEstimateIndex, String)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden End")
    End Sub

    ''' <summary>
    ''' フッターを表示します。
    ''' </summary>
    ''' <param name="commonMaster">イベント発生元</param>
    ''' <param name="category">イベントデータ</param>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
        Me.commonMasterPage = commonMaster

        '自ページの所属メニューを宣言
        category = FooterMenuCategory.TCV

        '使用するサブメニューボタンを宣言
        Return {SUBMENU_TCV_CARINVITATION, SUBMENU_TCV_ORIGINALLIST, SUBMENU_TCV_COMPARECOMPETITOR, SUBMENU_TCV_LIBRARY, SUBMENU_TCV_QUOTATION, SUBMENU_TCV_JUTYU}
    End Function

    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitHeaderEvent Start")

        '戻るボタン非活性化
        CType(Master, CommonMasterPage).IsRewindButtonEnabled = False

        'ログアウト
        '活動破棄チェックのクライアントサイドスクリプトを埋め込む
        CType(Me.Master, CommonMasterPage).GetHeaderButton(HeaderButton.Logout).OnClientClick = "return inputUpdateCheck();"

        If CType(Me.businessFlgHiddenField.Value, Boolean) Or _
            OperationLocked Then
            '商談中、又はロック状態の場合はi-cropアイコン使用不可
            Me.commonMasterPage.ContextMenu.Enabled = False
        Else
            '上記以外は場合はi-cropアイコン使用可能
            Me.commonMasterPage.ContextMenu.Enabled = True
        End If

        If OperationLocked Then
            'ロック状態の場合は検索窓非表示
            Me.commonMasterPage.SearchBox.Visible = False
        Else
            Me.commonMasterPage.SearchBox.Visible = True
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitHeaderEvent End")

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitFooterEvent Start")

        'メニューボタン定義
        'メインメニュー
        mainMenuButton = commonMasterPage.GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click

        '顧客
        customerButton = commonMasterPage.GetFooterButton(FooterMenuCategory.Customer)
        AddHandler customerButton.Click, AddressOf CustomerButton_Click

        '2019/05/20 TS  村井 PostUAT-3114 DEL

        'ショールーム
        Dim ssvButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        If ssvButton IsNot Nothing Then

            'ショールームボタンを非表示とする。(NEXT STEPまでのレイアウト暫定対応)
            ssvButton.Visible = False
            'TeamLeaderが商談中(又はロックモード)の場合、ショールームボタンを無効化する。
            ssvButton.Enabled = Not (StaffContext.Current.TeamLeader AndAlso (OperationLocked OrElse IsSales()))

            AddHandler ssvButton.Click, _
            Sub()
                'ショールーム画面に遷移
                Me.RedirectNextScreen(STR_DISPID_SHOWROOM)
            End Sub

            ' ショールーム
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus).OnClientClick = "return inputUpdateCheck();"
        End If

        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
        '新車納車システム連携メニュー
        Dim linkMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(LINK_MENU)
        ''リンク先URLを販売店環境設定TBLより取得
        Dim dlrenvdt As New DealerEnvSetting
        Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
        dlrenvrw = dlrenvdt.GetEnvSetting(StaffContext.Current.DlrCD, C_LINK_MENU_URL)
        If dlrenvrw IsNot Nothing Then
            If Not String.IsNullOrWhiteSpace(dlrenvrw.PARAMVALUE) Then
                ''URLを取得できた場合、新車納車システム連携メニューを表示。
                ''(ただし画面がロック状態のときは非表示)
                If linkMenuButton IsNot Nothing Then
                    If OperationLocked Then
                        linkMenuButton.Visible = False
                    Else
                        linkMenuButton.Visible = True
                    End If
                End If
            End If
        End If
        '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

        'サブメニューボタン定義
        '2019/05/20 TS  村井 PostUAT-3114 DEL
        '見積もり
        Dim quotationButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(SUBMENU_TCV_QUOTATION)
        '選択状態
        quotationButton.Selected = True
        '非活性化
        quotationButton.Enabled = False

        '画面固有ボタン定義
        BtnPrint = Me.printLinkButton
        BtnDiscountApproval = Me.DiscountApprovalButton

        '2014/05/07 NextStep フッターボタン追加対応 TCS 森 START

        Dim businessFlg As Boolean           '商談中フラグ

        If Me.ContainsKey(ScreenPos.Current, "BusinessFlg") Then
            'セッションに格納されている場合はセッション値を使用
            businessFlg = CType(Me.GetValue(ScreenPos.Current, "BusinessFlg", False), Boolean)
        Else
            'セッションに格納されていない場合はユーザのステータスを参照
            businessFlg = IsSales()
        End If

        'SPM
        'カテゴリ値は基盤の定数を使用する ※今は仮置き
        Dim spmButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.SPM)
        'AddHandler spmButton.Click, AddressOf spmButton_Click

        '受注時説明ツール
        Dim jutyuu As CommonMasterFooterButton = commonMasterPage.GetFooterButton(SUBMENU_TCV_JUTYU)
        AddHandler jutyuu.Click, AddressOf jutyuuButton_Click

        '納車時説明ツール
        Dim nousya As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.NewCarExplain)
        AddHandler nousya.Click, AddressOf nousyaButton_Click

        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If (SC3070201BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("0")) Then
            jutyuu.Visible = False
            nousya.Visible = False
        End If
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

        '商談ステータスに応じてフッターボタンを活性、非活性とする
        If businessFlg Then

            '商談中の場合、非活性とする
            spmButton.Enabled = False

        Else

            'スタンバイの場合、活性とする
            spmButton.Enabled = True

        End If

        '2014/05/07 NextStep フッターボタン追加対応 TCS 森 END

        '活動破棄チェックのクライアントサイドスクリプトを埋め込む
        'メニュー
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu).OnClientClick = "return inputUpdateCheck();"

        '入力内容破棄チェックのクライアントサイドスクリプトを埋め込む
        '顧客
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer).OnClientClick = "return inputUpdateCheck();"
        'TCV
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV).OnClientClick = "return inputUpdateCheck();"
        '車両紹介
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_CARINVITATION).OnClientClick = "return inputUpdateCheck();"
        '緒元表
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).OnClientClick = "return inputUpdateCheck();"
        '競合車比較
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).OnClientClick = "return inputUpdateCheck();"
        'ライブラリ
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_LIBRARY).OnClientClick = "return inputUpdateCheck();"

        '受注時説明ツール
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_JUTYU).OnClientClick = "return inputUpdateCheck();"
        '納車時説明ツール
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).OnClientClick = "return inputUpdateCheck();"

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitFooterEvent End")

    End Sub

    Private Sub RenderFooterEvent()
        'TCV
        Dim tcvButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.TCV)

        If CType(Me.ReferenceModeHiddenField.Value, Boolean) Then
            'ロック時

            If CType(Me.businessFlgHiddenField.Value, Boolean) Then
                BtnPrint.Visible = True
                PrintButton.Style.Item("display") = "block"
            Else
                BtnPrint.Visible = False
                PrintButton.Style.Item("display") = "none"
            End If

            If Not OperationLocked Then
                '読取専用フラグ=True

                '価格相談ボタン非表示
                BtnDiscountApproval.Visible = False
                ApprovalButton.Style.Item("display") = "none"

                'メニューボタンを表示
                mainMenuButton.Visible = True
                If CType(Me.businessFlgHiddenField.Value, Boolean) Then
                    '商談フラグ=True
                    'メニューボタンを非活性
                    mainMenuButton.Enabled = False

                Else
                    '商談フラグ=False
                    'メニューボタンを活性
                    mainMenuButton.Enabled = True

                End If

                '顧客ボタンを表示
                customerButton.Visible = True

                'TCVボタン
                tcvButton.Enabled = False

                '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） START
                'Product Presentationアイコン
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.PP).Visible = True

                '諸元表アイコン(Specification)
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).Enabled = True

                '競合車比較アイコン(Comparison)
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).Enabled = True

                '受注時説明アイコン(SalseDES)
                InitJutyuuButton()

                '納車時説明アイコン(Delivery)
                If (SC3070201BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("0")) Then
                    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).Visible = False
                Else
                    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).Visible = True
                End If

                'SPMアイコン
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Visible = True
                '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） END
            Else
                '価格相談ボタン非表示
                BtnDiscountApproval.Visible = False
                ApprovalButton.Style.Item("display") = "none"

                'メニューボタンを非表示
                mainMenuButton.Visible = False

                '顧客ボタンを非表示
                customerButton.Visible = False

                '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） START
                'Product Presentationアイコン
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.PP).Visible = False

                '諸元表アイコン(Specification)
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).Enabled = False

                '競合車比較アイコン(Comparison)
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).Enabled = False

                '受注時説明アイコン(SalseDES)
                CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_JUTYU).Visible = False

                '納車時説明アイコン(Delivery)
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).Visible = False

                'SPMアイコン
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Visible = False
                '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） END
            End If

        Else
            '通常時

            'フッターメニュー

            'メニューボタン
            mainMenuButton.Visible = True
            If CType(Me.businessFlgHiddenField.Value, Boolean) Then
                '商談フラグ=True
                'メニューボタンを非活性
                mainMenuButton.Enabled = False

                'TCVボタン
                tcvButton.Enabled = True
            Else
                '商談フラグ=False
                'メニューボタンを活性
                mainMenuButton.Enabled = True
            End If

            '顧客ボタン
            If Not CType(Me.operationCodeHiddenField.Value, iCROP.BizLogic.Operation).Equals(iCROP.BizLogic.Operation.SSF) And _
                String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Then
                'SalesStaff以外、かつ、CRCustIdがブランク("")の場合
                '顧客ボタンを非表示
                customerButton.Visible = False

            Else
                '顧客ボタンを表示
                customerButton.Visible = True

            End If

            If Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then
                'マネージャー回答時

                '価格相談ボタン非表示
                BtnDiscountApproval.Visible = False
                ApprovalButton.Style.Item("display") = "none"
                BtnPrint.Visible = False
                PrintButton.Style.Item("display") = "none"

            ElseIf Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) Then
                'スタッフ回答参照時
                '契約書ボタン表示
                '価格相談ボタン非表示（再表示のためJS側で非表示化）
                BtnDiscountApproval.Visible = True
                ApprovalButton.Style.Item("display") = "block"
                ''見積書ボタン表示
                BtnPrint.Visible = True
                PrintButton.Style.Item("display") = "block"

            Else
                'スタッフ通常時
                '価格相談ボタン表示
                BtnDiscountApproval.Visible = True
                ApprovalButton.Style.Item("display") = "block"
                BtnPrint.Visible = True
                PrintButton.Style.Item("display") = "block"
            End If

            '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） START
            'Product Presentationアイコン
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.PP).Visible = True

            '諸元表アイコン(Specification)
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).Enabled = True

            '競合車比較アイコン(Comparison)
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).Enabled = True

            '受注時説明アイコン(SalseDES)
            InitJutyuuButton()

            '納車時説明アイコン(Delivery)
            If (SC3070201BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("0")) Then
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).Visible = False
            Else
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain).Visible = True
            End If

            'SPMアイコン
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Visible = True
            '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） END

        End If

        '2019/09/24 TS 髙橋(龍) TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 START
        '諸元表アイコン(Specification)非表示
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).Visible = False

        '競合車比較アイコン(Comparison)非表示
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).Visible = False
        '2019/09/24 TS 髙橋(龍) TKM Next Gen e-CRB Project V4 Cutover (All dealer) Rehearsal Block F-1 END

        If Me.IsFreezed Then
            '価格相談ボタン非表示
            BtnDiscountApproval.Visible = False
            ApprovalButton.Style.Item("display") = "none"

            'TCVボタン非表示
            tcvButton.Enabled = False
        End If
    End Sub

    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 START
    ''' <summary>
    ''' 受注時説明ツールボタン表示設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitJutyuuButton()

        Dim jutyuu As CommonMasterFooterButton = commonMasterPage.GetFooterButton(SUBMENU_TCV_JUTYU)

        'SCメインから直接TCVを経由して見積作成を開いた場合、受注時説明ボタンは非表示とする
        If Me.lngFollowupBoxSeqNoHiddenField.Value.Equals("") Or Me.strCRCustIdHiddenField.Value.Equals("") Then

            jutyuu.Visible = False

        Else
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
            If (SC3070201BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("1")) Then
                jutyuu.Visible = True
            Else
                jutyuu.Visible = False
            End If
            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
        End If
    End Sub

    '2014/05/07 NextStep フッターボタン追加対応 TCS 森 END

    ''' <summary>
    ''' 編集ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitEditButton()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitEditButton Start")

        '承認済み、商談中、非ロック中の場合のみ表示
        If String.Equals(Me.contractApprovalSatus.Value, "2") And IsSales() And Not OperationLocked Then
            EditButton.Style.Item("display") = "block"
        Else
            EditButton.Style.Item("display") = "none"
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitEditButton End")

    End Sub

    ''' <summary>
    ''' 注文承認依頼ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitContractApprovalButton()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitContractApprovalButton Start")

        '未承認/承認依頼中/否認	商談中	非ロック中
        If String.Equals(Me.contractApprovalSatus.Value, "0") Or String.Equals(Me.contractApprovalSatus.Value, "1") Or String.Equals(Me.contractApprovalSatus.Value, "3") Or String.Equals(Me.contractApprovalSatus.Value, " ") Then
            If IsSales() And Not OperationLocked Then
                ContractButton.Style.Item("display") = "block"
            Else
                ContractButton.Style.Item("display") = "none"
            End If
        Else
            ContractButton.Style.Item("display") = "none"
        End If

        '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） START
        If OperationLocked Then
            ContractButton.Visible = False
        Else
            ContractButton.Visible = True
        End If
        '2019/05/08 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス） END

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitContractApprovalButton End")

    End Sub

    ''' <summary>
    ''' 価格相談ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitPriceApprovalButton()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitPriceApprovalButton Start")

        '価格相談ボタン初期表示フラグ取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, DISP_DISCOUNT_APPROVAL_BUTTON)
        If IsNothing(dtrow) Then '取得できなかった場合
            DiscountApprovalButtonFlg.Value = "0"
        Else
            DiscountApprovalButtonFlg.Value = dtrow.PARAMVALUE
        End If

        If String.Equals(DiscountApprovalButtonFlg.Value, "1") Then
            ApprovalButton.Style.Item("display") = "block"
        End If

        '未承認/否認、商談中、非ロック中
        If (String.Equals(Me.contractApprovalSatus.Value, " ") Or String.Equals(Me.contractApprovalSatus.Value, "0") Or String.Equals(Me.contractApprovalSatus.Value, "3")) And IsSales() And Not OperationLocked Then
            ApprovalButton.Style.Item("display") = "block"
        Else
            ApprovalButton.Style.Item("display") = "none"
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitPriceApprovalButton End")

    End Sub

    ''' <summary>
    ''' 印刷ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitPrintButton()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitPrintButton Start")

        PrintButton.Style.Item("display") = "none"

        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow

        '見積作成画面のURLを取得
        dtrow = dlrSetteing.GetEnvSetting(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, USED_FLG_PRINTBUTTON)

        If String.Equals(dtrow.PARAMVALUE, "1") Then
            If StaffContext.Current.OpeCD = "8" Then
                If IsSales() Then
                    PrintButton.Style.Item("display") = "block"
                Else
                    If String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Then
                        PrintButton.Style.Item("display") = "block"
                    End If
                End If
            Else
                If String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Then
                    PrintButton.Style.Item("display") = "block"
                End If
            End If
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitPrintButton End")

    End Sub

    ''' <summary>
    ''' MG通知一覧制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitMGInfoList()
        If StaffContext.Current.TeamLeader AndAlso (IsSales() OrElse OperationLocked) Then
            'TLログイン 且つ 商談中(又は ロック中）の場合、MG通知一覧を非表示にする。
            Me.HideMGInfoList()
        End If
    End Sub

    ''' <summary>
    ''' MG用通知一覧を非表示
    ''' </summary>
    ''' <remarks>※SC3080202.ascx.vbに同一関数有り</remarks>
    Private Sub HideMGInfoList()
        'MG用通知一覧を非表示とする。(毎回PostBack時にマスターページにて復元されるため、復元処理は不要)
        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "noticeListFrameHide", "$(function(){ $('#noticeListFrame').css('display','none');});", True)
    End Sub

    ''' <summary>
    ''' ロック機能
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DefaultOperationLocked As Boolean Implements Toyota.eCRB.SystemFrameworks.Web.ICustomerForm.DefaultOperationLocked
        Get

            Dim blnLockStatus As String            'ロック状態

            If Me.ContainsKey(ScreenPos.Current, "MenuLockFlag") Then
                blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)
            Else
                blnLockStatus = False
            End If

            If String.Equals(blnLockStatus.ToUpper(Globalization.CultureInfo.CurrentCulture), StrTrue) Then
                Return True

            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' TCV関連パラメータ設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitTcvParam()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitTcvParam Start")

        'セッション情報取得
        Dim operationCode As Integer         '権限コード
        Dim businessFlg As Boolean           '商談中フラグ
        Dim readOnlyFlg As Boolean           '読取専用フラグ

        If Me.ContainsKey(ScreenPos.Current, "OperationCode") Then
            'セッションに格納されている場合はセッション値を使用
            operationCode = CType(Me.GetValue(ScreenPos.Current, "OperationCode", False), Integer)
        Else
            'セッションから取得できない場合はログインユーザのOperationCodeを使用
            Dim staffInfo As StaffContext = StaffContext.Current
            operationCode = StaffContext.Current.OpeCD
        End If

        If Me.ContainsKey(ScreenPos.Current, "BusinessFlg") Then
            'セッションに格納されている場合はセッション値を使用
            businessFlg = CType(Me.GetValue(ScreenPos.Current, "BusinessFlg", False), Boolean)
        Else
            'セッションに格納されていない場合はユーザのテータスを参照
            businessFlg = IsSales()
        End If

        If Me.ContainsKey(ScreenPos.Current, "ReadOnlyFlg") Then
            'セッションに格納されている場合はセッション値を使用
            readOnlyFlg = CType(Me.GetValue(ScreenPos.Current, "ReadOnlyFlg", False), Boolean)

            If readOnlyFlg And _
                businessFlg Then
                '読取専用、かつ、商談中の場合

                If Me.IsFreezed = False Then
                    '未契約の場合
                    readOnlyFlg = False

                End If

            End If

        Else
            'セッションに格納されていない場合
            '選択している見積IDのIndex
            Dim selectedEstimateIndex As Long
            If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
                selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
            Else
                selectedEstimateIndex = 0
            End If

            Dim estimateId As Long = GetSelectedEstimateId(CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String), selectedEstimateIndex)
            'CR活動結果取得
            Dim bizLogic As New SC3070201BusinessLogic
            Dim dt As SC3070201DataSet.SC3070201FllwUpBoxDataTable = bizLogic.GetCRActresult(estimateId)
            bizLogic = Nothing
            readOnlyFlg = False

            If dt.Rows.Count > 0 Then

                Dim crActresult As String = CType(dt.Rows(0).Item("CRACTRESULT"), String)

                If crActresult.Equals(CRACTRESULT_SUCCESS) Or _
                    crActresult.Equals(CRACTRESULT_GIVEUP) Or _
                    crActresult.Equals(CRACTRESULT_ENQUIRY_COMPLETED) Then
                    'CR活動結果が終了している場合は読取専用
                    readOnlyFlg = True

                End If

            End If

            If IsSales() = False Then
                '商談中以外は読取専用
                readOnlyFlg = True

            End If

        End If

        'HIDDEN値設定
        Me.operationCodeHiddenField.Value = CType(operationCode, String)

        Me.businessFlgHiddenField.Value = CType(businessFlg, String)

        Me.readOnlyFlgHiddenField.Value = CType(readOnlyFlg, String)

        '読取専用モード時
        'If CType(Me.readOnlyFlgHiddenField.Value, Boolean) Then
        '    '入力不可状態にする
        '    Me.ReferenceModeHiddenField.Value = StrTrue
        'End If

        '在庫状況表示
        'imsinfo = CType(LoadControl("~/Pages/SC3070101.ascx"), Pages_SC3070101)
        'PlaceHolder1.Controls.Add(imsinfo)

        If OperationLocked = False And _
            Not String.Equals(Me.strApprovalModeHiddenField.Value, ModeNormal) Then
            'ロック状態以外、かつ、通常モード以外の場合、回答入力欄表示
            Me.approvalFieldFlgHiddenField.Value = StrTrue

        Else
            '上記以外の場合、回答入力欄非表示
            Me.approvalFieldFlgHiddenField.Value = StrFalse

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitTcvParam End")
    End Sub

    '2019/04/17 TS 村井 (FS)次世代タブレット新興国向けの性能評価 DEL

    ''' <summary>
    ''' 価格入力欄 初期データ取得、表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispInitApprovalData(ByVal dtApproval As SC3070201DataSet.SC3070201EstDiscountApprovalDataTable)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitApprovalData Start")

        For Each dr As SC3070201DataSet.SC3070201EstDiscountApprovalRow In dtApproval

            'シリーズコード
            If Not dr.IsSERIESCDNull Then
                Me.approvalSeriescdHiddenField.Value = dr.Item("SERIESCD").ToString
            End If

            'モデルコード
            If Not dr.IsMODELCDNull Then
                Me.approvalModelcdHiddenField.Value = dr.Item("MODELCD").ToString
            End If

        Next

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitApprovalData End")
    End Sub

    ''' <summary>
    ''' 商談(一時対応・営業活動・納車作業)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>ステータスを参照して商談中か判断する</remarks>
    Private Function IsSales() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsSales Start")

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3")) Then

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsSales End")

            Return True
        Else

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsSales End")

            Return False
        End If
    End Function

    ''' <summary>
    ''' 検索ボックスの制御
    ''' </summary>
    ''' <remarks>商談中・営業・一時対応中は検索ボックスを非活性にする</remarks>
    Private Sub InitSearchBox(ByVal salesFlg As Boolean)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitSearchBox Start")

        '商談中(営業活動・一時対応も)の場合、検索ボックスに名前を入れ非活性に
        If salesFlg Then
            Me.commonMasterPage.SearchBox.Enabled = False
            Me.commonMasterPage.SearchBox.SearchText = Me.cstNameHiddenField.Value
        ElseIf Me.commonMasterPage.SearchBox.Enabled = False Then
            '検索ボックスの状態を元に戻す
            Me.commonMasterPage.SearchBox.Enabled = True
            Me.commonMasterPage.SearchBox.SearchText = String.Empty
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitSearchBox End")
    End Sub

    ''' <summary>
    ''' 遷移時チェック
    ''' </summary>
    ''' <remarks>遷移時の状態をチェックする</remarks>
    Private Sub CheckStatus()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckStatus Start")

        If Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) Then
            'スタッフ通知一覧遷移時

            '車両変更チェック
            If CheckVcl() = False Then
                '車両変更ありの場合
                Me.actionModeHiddenField.Value = String.Empty
                MyBase.ShowMessageBox(981)

            End If

        ElseIf Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then
            'マネージャー通知一覧遷移時

            Dim bizLogic As New SC3070201BusinessLogic
            Dim chkFlg As Boolean = False

            'キャンセル状態取得チェック
            If CheckApprovalStatus() = False Then
                '件数が0件、または、キャンセルの場合
                Me.actionModeHiddenField.Value = String.Empty
                ScriptManager.RegisterStartupScript(Me, _
                                    Me.GetType, _
                                    "PageLoad", _
                                    "dispLoading();alert(SC3070201HTMLDecode(""" + HttpUtility.HtmlEncode(WebWordUtility.GetWord(982)) + """));this_form.actionModeHiddenField.value = ""2"";this_form.submit();", _
                                    True)

            End If

            bizLogic = Nothing

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckStatus End")
    End Sub

    ''' <summary>
    ''' 価格相談状態チェック
    ''' </summary>
    ''' <returns>True:OK（キャンセル以外）、False:NG（キャンセル）</returns>
    ''' <remarks>価格相談状態をチェックする</remarks>
    Private Function CheckApprovalStatus() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckApprovalStatus Start")

        Dim bizLogic As New SC3070201BusinessLogic
        Dim chkFlg As Boolean = False

        '最終ステータス取得
        Dim noticeReqId As Integer = CType(Me.noticeReqIdHiddenField.Value, Integer)
        Dim dtNoticeRequest As SC3070201DataSet.SC3070201NoticeRequestDataTable = bizLogic.GetManagerAnswerCheck(noticeReqId)

        If dtNoticeRequest.Rows.Count = 0 OrElse _
            dtNoticeRequest.Rows(0).Item("STATUS").Equals(NOTICE_REQ_STATUS_CANCEL) Then
            '通知依頼が件数が0件、または、キャンセルの場合
            chkFlg = False

        Else
            'キャンセル以外の場合

            chkFlg = True

        End If

        bizLogic = Nothing

        Return chkFlg

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckApprovalStatus End")
    End Function

    ''' <summary>
    ''' 契約状態チェック
    ''' </summary>
    ''' <returns>True:OK（未契約）、False:NG（契約済み）</returns>
    ''' <remarks>契約状態状態をチェックする</remarks>
    Private Function CheckContract() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckContract Start")

        '現時点の契約状態を取得する
        Dim bizLogic As New SC3070201BusinessLogic
        '選択している見積IDのIndex
        Dim selectedEstimateIndex As Long
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If

        Dim dt As SC3070201DataSet.SC3070201ContractDataTable = bizLogic.GetContract(GetSelectedEstimateId(CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String), selectedEstimateIndex))
        If dt.Rows.Count > 0 Then
            Me.contractFlgHiddenField.Value = dt.Rows(0).Item("CONTRACTFLG")
        End If

        Dim chkFlg As Boolean = False

        If Me.contractFlgHiddenField.Value.Equals(STR_CONTRACTFLG_COMP) Then
            '契約済みの場合
            chkFlg = False
        Else
            '契約済み以外の場合
            chkFlg = True
        End If

        Return chkFlg

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckContract End")
    End Function

    ''' <summary>
    ''' 車両変更・価格相談状態チェック
    ''' </summary>
    ''' <returns>True:OK（変更なし）、False:NG（変更あり）</returns>
    ''' <remarks>車両変更状態、価格相談状態をチェックする</remarks>
    Private Function CheckVcl() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckVcl Start")

        Dim chkFlg As Boolean = False

        If Me.seriesCdHiddenField.Value.Equals(Me.approvalSeriescdHiddenField.Value) And _
            Me.modelCdHiddenField.Value.Equals(Me.approvalModelcdHiddenField.Value) Then
            '車両変更なしの場合

            chkFlg = True

        Else
            '車両変更ありの場合

            chkFlg = False

        End If

        Return chkFlg

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckVcl End")
    End Function

    ''' <summary>
    ''' 対象見積管理ID取得
    ''' </summary>
    ''' <param name="allEstimeId">見積管理ID(カンマ区切り)</param>
    ''' <param name="Index">対象Index番号</param>
    ''' <returns>見積管理ID</returns>
    ''' <remarks>Indexに該当する見積管理IDを返す</remarks>
    Private Function GetSelectedEstimateId(ByVal allEstimeId As String, ByVal Index As Long) As Long
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedEstimateId Start")

        Dim estimetaId = allEstimeId.Split(","c)

        Return CType(estimetaId(Index), Long)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedEstimateId End")
    End Function

    ''' <summary>
    ''' 印刷ポップアップ表示
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub popupPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrintButtonDummy.Click
        Me.approvalButtonFlgHiddenField.Value = "1"
    End Sub

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START

    ''' <summary>表示中の活動の商談ID</summary>
    Private Const SESSION_KEY_SALES_ID As String = "SalesId"
    ''' <summary>承認依頼対象の見積管理ID</summary>
    Private Const SESSION_KEY_EST_ID As String = "EstimateId"
    ''' <summary>お客様ご説明モード</summary>
    Private Const SESSION_KEY_EST_MODE As String = "SalesbookingDescriptionViewMode"
    ''' <summary>契約条件変更フラグ</summary>
    Private Const SESSION_KEY_ODR_CHG_FLG As String = "ContractAskChgFlg"
    ''' <summary>顧客ID</summary>
    Private Const SESSION_KEY_CST_ID As String = "CstId"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CST_TYPE As String = "CstType"
    ''' <summary>顧客車両区分</summary>
    Private Const SESSION_KEY_CST_VCL_TYPE As String = "CstVclType"

    ''' <summary>
    ''' 受注時説明活動画面表示
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub OrderAfterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OrderAfterButton.Click

        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALES_ID, Me.lngFollowupBoxSeqNoHiddenField.Value)   '商談ID：表示中の活動の商談ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_EST_ID, Me.lngEstimateIdHiddenField.Value)           '見積管理ID：承認依頼対象の見積管理ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_EST_MODE, "1")                                       '受注時説明表示モード：1(お客様ご説明モード)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_ODR_CHG_FLG, Me.OrderAfterFlgHiddenField.Value)      '契約条件変更フラグ：1(変更あり)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_ID, Me.strCRCustIdHiddenField.Value)             '顧客ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_TYPE, Me.strCstKindHiddenField.Value)            '顧客種別
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_VCL_TYPE, Me.strCustomerClassHiddenField.Value)  '顧客車両区分

        '受注後機能ツールへ遷移する
        Me.RedirectNextScreen("SC3270101")

    End Sub
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

    ''' <summary>
    ''' 初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitialSetting()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitialSetting Start")

        'セッション情報取得
        Dim lngEstimateId As Long               '見積管理ID
        Dim blnLockStatus As Boolean            'ロック状態
        Dim estimateId As String
        Dim selectedEstimateIndex As Long

        '見積ID(カンマ区切り)
        estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)

        '選択している見積IDのIndex
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If

        '選択している見積ID
        lngEstimateId = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

        If Me.ContainsKey(ScreenPos.Current, "MenuLockFlag") Then
            blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)
        Else
            blnLockStatus = False
        End If

        'HIDDEN値設定
        Me.lngEstimateIdHiddenField.Value = CType(lngEstimateId, String)
        Me.estimateIdHiddenField.Value = CType(estimateId, String)
        Me.selectedEstimateIndexHiddenField.Value = CType(selectedEstimateIndex, String)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitialSetting End")

    End Sub

    ''' <summary>
    ''' 初期データ取得、表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispInitData()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitData Start")

        Dim bizLogic As SC3070201BusinessLogic

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070201BusinessLogic

        '初期表示データ取得（API使用）

        Dim dsEstimation As IC3070201DataSet    '見積情報格納用

        dsEstimation = New IC3070201DataSet

        '見積情報データテーブル作成
        Dim dtEstimateData As New SC3070201DataSet.SC3070201ESTIMATEDATADataTable
        Dim drEstimateData As DataRow = dtEstimateData.NewRow

        drEstimateData("ESTIMATEID") = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
        dtEstimateData.Rows.Add(drEstimateData)
        dsEstimation = bizLogic.GetEstimateInitialData(dtEstimateData)

        'ビューステートに見積情報保存
        ViewState("DataSetEstimation") = dsEstimation

        If dsEstimation.Tables("IC3070201EstimationInfo").Rows.Count <> 0 Then

            'HIDDEN値設定
            Me.strDlrcdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")) Then
                Me.strStrCdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")) Then
                Me.lngFollowupBoxSeqNoHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")) Then
                Me.strCstKindHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")) Then
                Me.strCustomerClassHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")) Then
                Me.strCRCustIdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")
            End If
            '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            ' 直販フラグ
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DIRECT_SALES_FLG")) Then
                Me.DirectBillingFlag.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DIRECT_SALES_FLG")
            End If
            '2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

            'Me.basePriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("BASEPRICE")
            Me.contractFlgHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTFLG")

            '金額
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")) Then
                'Me.discountPriceValueHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")
            End If

            'メモ最大桁数取得
            'Dim drEstMemoMax As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            'drEstMemoMax = bizLogic.GetMemoMax()
            'Me.memoMaxHiddenField.Value = drEstMemoMax.PARAMVALUE

            '初期表示データ取得
            'Dim dsEstimateExtraData As SC3070201DataSet

            '見積情報データテーブル更新
            dtEstimateData.Clear()

            drEstimateData("ESTIMATEID") = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            drEstimateData("DLRCD") = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
            dtEstimateData.Rows.Add(drEstimateData)

            '通知依頼IDをセット
            Dim lngNoticeReqId As Long
            If Not String.IsNullOrEmpty(Me.noticeReqIdHiddenField.Value) Then
                lngNoticeReqId = CType(Me.noticeReqIdHiddenField.Value, Long)
            Else
                lngNoticeReqId = 0
            End If

            '初期表示データ取得
            'dsEstimateExtraData = bizLogic.GetInitialData(dtEstimateData, dsEstimation, lngNoticeReqId)

            '氏名敬称取得

            '敬称の設定値を取得
            'Dim dtSysEnvSet As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
            'Using sysenvDataTbl As New SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
            '    Dim sysenvDataRow As SC3070201DataSet.SC3070201SYSTEMENVSETTINGRow
            '    sysenvDataRow = sysenvDataTbl.NewSC3070201SYSTEMENVSETTINGRow
            '    sysenvDataTbl.Rows.Add(sysenvDataRow)
            '    dtSysEnvSet = bizLogic.GetNameTitleSysenv(sysenvDataTbl)
            'End Using

            Me.seriesNameHiddenField.Value = HttpUtility.HtmlEncode(CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESNM"), String))
            Me.modelNameHiddenField.Value = HttpUtility.HtmlEncode(CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNM"), String))
            Me.seriesCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESCD"), String)
            Me.modelCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD"), String)
            Me.suffixCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SUFFIXCD"), String)
            Me.extColorCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD"), String)
            Me.modelNumberHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNUMBER"), String)

            '価格入力欄設定
            If Not String.IsNullOrEmpty(Me.noticeReqIdHiddenField.Value) Then
                Dim dtEstDiscountApproval As SC3070201DataSet.SC3070201EstDiscountApprovalDataTable = _
                    bizLogic.GetEstDiscountApproval(lngNoticeReqId)
                DispInitApprovalData(dtEstDiscountApproval)

            End If

            'セッションにFollowBoxSeqNoをセット（通知一覧用）
            MyBase.SetValue(ScreenPos.Current, "SearchKey.FOLLOW_UP_BOX", Me.lngFollowupBoxSeqNoHiddenField.Value)

            '氏名敬称取得
            '敬称の設定値を取得
            Dim dtSysEnvSet As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
            Using sysenvDataTbl As New SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
                Dim sysenvDataRow As SC3070201DataSet.SC3070201SYSTEMENVSETTINGRow
                sysenvDataRow = sysenvDataTbl.NewSC3070201SYSTEMENVSETTINGRow
                sysenvDataTbl.Rows.Add(sysenvDataRow)
                dtSysEnvSet = bizLogic.GetNameTitleSysenv(sysenvDataTbl)
            End Using

            Dim name As String = String.Empty      '顧客氏名
            Dim nametitle As String = String.Empty '顧客敬称

            If Not String.IsNullOrEmpty(Me.strCstKindHiddenField.Value) Then
                '顧客がある場合

                '敬称取得
                Dim dtNametitle As SC3070201DataSet.SC3070201CustNametitleDataTable
                dtNametitle = bizLogic.GetCustNametitle(Me.strCstKindHiddenField.Value, Me.strDlrcdHiddenField.Value, Me.strCRCustIdHiddenField.Value)

                If dtNametitle.Rows.Count > 0 Then
                    '顧客氏名
                    If Not IsDBNull(dtNametitle.Rows(0).Item("NAME")) Then
                        name = CType(dtNametitle.Rows(0).Item("NAME"), String)
                    Else
                        name = String.Empty
                    End If
                    '敬称
                    If Not IsDBNull(dtNametitle.Rows(0).Item("NAMETITLE")) Then
                        nametitle = CType(dtNametitle.Rows(0).Item("NAMETITLE"), String)
                    Else
                        nametitle = String.Empty
                    End If

                End If

            End If

            '敬称
            If String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), "1") Then
                Me.cstNameHiddenField.Value = HttpUtility.HtmlEncode(nametitle + " " + name)

            ElseIf String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), "2") Then
                Me.cstNameHiddenField.Value = HttpUtility.HtmlEncode(name + " " + nametitle)
            End If


        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitData End")

    End Sub

    ''' <summary>
    ''' 画面モード設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispModeSetting()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispModeSetting Start")

        '編集モードをベース
        Dim DispMode As Boolean = False

        'ロックモードの場合参照モード
        If OperationLocked Then
            DispMode = True
        End If

        '注文承認依頼中、承認済みの場合参照モード
        If String.Equals(Me.contractApprovalSatus.Value, "1") Or String.Equals(Me.contractApprovalSatus.Value, "2") Then
            DispMode = True
        End If

        '顧客指定済みで権限がSC以外の場合参照モード
        If Not String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) And StaffContext.Current.OpeCD <> Operation.SS Then
            DispMode = True
        End If

        '顧客指定済みで権限がSCでも商談中で無ければ参照モード
        If Not String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) And StaffContext.Current.OpeCD = Operation.SS And Not IsSales() Then
            DispMode = True
        End If

        '読み取り専用ならば参照モード
        If Not String.IsNullOrEmpty(Me.readOnlyFlgHiddenField.Value) And _
            String.Equals(Me.readOnlyFlgHiddenField.Value.ToUpper(Globalization.CultureInfo.CurrentCulture), StrTrue) Then
            DispMode = True
        End If

        Me.ReferenceModeHiddenField.Value = CType(DispMode, String)
        Me.operationLockedHiddenField.Value = Me.ReferenceModeHiddenField.Value

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispModeSetting End")

    End Sub

    '2019/05/17 TS 舩橋 PostUAT-3092 START
    ''' <summary>
    ''' 見積IDが表示可能かを判断し、可能な見積IDのみを残す
    ''' セッション情報からHiddenFiel情報に値を反映する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SessionInfoEstimateIdRefresh()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(System.Reflection.MethodBase.GetCurrentMethod.Name & " Start ")

        Dim estimateId As String
        Dim selectedEstimateIndex As String
        Dim selectedEstimateId As String

        ' セッション情報から見積IDを取得
        estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Get Session [EstimateId] :" + estimateId)

        ' セッション情報から選択見積ID Indexを取得
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), String)
        Else
            selectedEstimateIndex = 0
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Get Session [SelectedEstimateIndex] :" + selectedEstimateIndex)

        selectedEstimateId = GetSelectedEstimateId(estimateId, selectedEstimateIndex)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Get Index of [SelectedEstimateId] :" + selectedEstimateId)


        ' 表示可能な見積IDを判定
        Dim bizLogic As New SC3070201BusinessLogic
        Dim contractinfo As SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable
        Dim estimateIdList As New List(Of String)
        For Each tempEstimateId In Split(estimateId, ",")
            If Not String.IsNullOrWhiteSpace(tempEstimateId) Then
                ' 契約承認ステータス（2: 承認）or 削除フラグ（0: 未削除）の時のみ表示可能
                contractinfo = bizLogic.GetContractApproval(tempEstimateId)
                If (contractinfo.First().CONTRACT_APPROVAL_STATUS = "2" Or contractinfo.First().DELFLG = "0") Then
                    estimateIdList.Add(tempEstimateId)
                End If
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("BusinessLogic ContractApproval info [EstimateId,CONTRACT_APPROVAL_STATUS,DELFLG] :" + tempEstimateId + "," + contractinfo.First().CONTRACT_APPROVAL_STATUS + "," + contractinfo.First().DELFLG)
            End If
        Next
        estimateId = String.Join(",", estimateIdList)
        selectedEstimateIndex = estimateIdList.IndexOf(selectedEstimateId)

        ' セッション情報に見積IDを格納
        MyBase.SetValue(ScreenPos.Current, "EstimateId", estimateId)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Set Session [EstimateId] :" + CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String))

        ' セッション情報に選択見積ID Indexを格納
        MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", selectedEstimateIndex)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Set Session [SelectedEstimateIndex] :" + CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), String))

        ' HiddenFielに見積IDを格納
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Before HiddenFiel [estimateIdHiddenField] :" + Me.estimateIdHiddenField.Value)
        Me.estimateIdHiddenField.Value = estimateId
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("After  HiddenFiel [estimateIdHiddenField] :" + Me.estimateIdHiddenField.Value)

        ' HiddenFielに選択見積ID Indexを格納
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Before  HiddenFiel [selectedEstimateIndexHiddenField] :" + Me.selectedEstimateIndexHiddenField.Value)
        Me.selectedEstimateIndexHiddenField.Value = selectedEstimateIndex
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("After   HiddenFiel [selectedEstimateIndexHiddenField] :" + Me.selectedEstimateIndexHiddenField.Value)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(System.Reflection.MethodBase.GetCurrentMethod.Name & " End ")
    End Sub
    '2019/05/17 TS 舩橋 PostUAT-3092 END

    '2019/05/20 TS  村井 PostUAT-3114 ADD Start

    ''' <summary>
    ''' TCV機能に渡す引数の設定
    ''' </summary>
    ''' <param name="params">TCV機能に渡す引数</param>
    ''' <param name="startPageId">初期表示画面ID</param>
    ''' <remarks>フッター（TCV）タップ時にTCV機能へ渡す引数を設定する</remarks>
    Private Sub AddTcvParameters(ByRef params As Dictionary(Of String, Object), ByVal startPageId As String)

        'ログ出力 Start *************************************************************
        Logger.Info("AddTcvParameters Start")
        'ログ出力 End ***************************************************************

        'パラメータの作成
        params.Add("DataSource", STR_ESTIMATEID)                                    'データ読み込み元
        params.Add("MenuLockFlag", OperationLocked)                                 'メニューロック状態
        params.Add("AccountStrCd", StaffContext.Current.BrnCD)                      'ログインユーザー店舗コード
        params.Add("Account", StaffContext.Current.Account)                         'ログインユーザーアカウント

        params.Add("OperationCode", CType(Me.operationCodeHiddenField.Value, Integer))      '権限コード
        params.Add("BusinessFlg", CType(Me.businessFlgHiddenField.Value, Boolean))          '商談中フラグ
        If Me.IsFreezed Then
            params.Add("ReadOnlyFlg", True)                                         '読み取り専用フラグ
        Else
            params.Add("ReadOnlyFlg", CType(Me.readOnlyFlgHiddenField.Value, Boolean))
        End If

        params.Add("DlrCd", Me.strDlrcdHiddenField.Value)                           '販売店コード
        params.Add("StartPageId", startPageId)                                      '初期表示画面ID

        params.Add("EstimateId", Me.estimateIdHiddenField.Value)                            '見積ID(カンマ区切り)
        params.Add("SelectedEstimateIndex", Me.selectedEstimateIndexHiddenField.Value)      '選択している見積IDのindex

        params.Add("CloseCallback", "icropScript.tcvCloseCallback")
        params.Add("StatusCallback", "icropScript.tcvStatusCallback")

        'ログ出力 Start *************************************************************
        Logger.Info("AddTcvParameters End")
        'ログ出力 End ***************************************************************

    End Sub

    ''' <summary>
    ''' TCV機能(車両選択)呼出Script作成
    ''' </summary>
    ''' <returns>TCV機能(車両選択)を呼び出すScript</returns>
    ''' <remarks>TCV機能(車両選択)を呼び出すScriptを作成する</remarks>
    Private Function BuildOpenTcvScript() As String

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenTcvScript Start")
        'ログ出力 End ***************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function openTcv() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson(STR_DISPID_TCV_SELECTSERIES))).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenTcvScript End")
        'ログ出力 End ***************************************************************

        Return sb.ToString()

    End Function

    ''' <summary>
    ''' TCV機能(車両紹介)呼出Script作成
    ''' </summary>
    ''' <returns>TCV機能(車両紹介)を呼び出すScript</returns>
    ''' <remarks>TCV機能(車両紹介)を呼び出すScriptを作成する</remarks>
    Private Function BuildOpenCarInvitationScript() As String

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenCarInvitationScript Start")
        'ログ出力 End ***************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function carInvitation() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson(STR_DISPID_TCV_CARINVITATION))).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenCarInvitationScript End")
        'ログ出力 End ***************************************************************

        Return sb.ToString()

    End Function

    ''' <summary>
    ''' TCV機能(諸元表)呼出Script作成
    ''' </summary>
    ''' <returns>TCV機能(諸元表)を呼び出すScript</returns>
    ''' <remarks>TCV機能(諸元表)を呼び出すScriptを作成する</remarks>
    Private Function BuildOpenOriginalListScript() As String

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenOriginalListScript Start")
        'ログ出力 End ***************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function originalList() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson(STR_DISPID_TCV_ORIGINALLIST))).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenOriginalListScript End")
        'ログ出力 End ***************************************************************

        Return sb.ToString()

    End Function

    ''' <summary>
    ''' TCV機能(競合車比較)呼出Script作成
    ''' </summary>
    ''' <returns>TCV機能(競合車比較)を呼び出すScript</returns>
    ''' <remarks>TCV機能(競合車比較)を呼び出すScriptを作成する</remarks>
    Private Function BuildOpenCompareCompetitorScript() As String

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenCompareCompetitorScript Start")
        'ログ出力 End ***************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function compareCompetition() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson(STR_DISPID_TCV_COMPARECOMPETITOR))).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenCompareCompetitorScript End")
        'ログ出力 End ***************************************************************

        Return sb.ToString()

    End Function

    ''' <summary>
    ''' TCV機能(ライブラリ)呼出Script作成
    ''' </summary>
    ''' <returns>TCV機能(ライブラリ)を呼び出すScript</returns>
    ''' <remarks>TCV機能(ライブラリ)を呼び出すScriptを作成する</remarks>
    Private Function BuildOpenLibraryScript() As String

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenLibraryScript Start")
        'ログ出力 End ***************************************************************

        Dim sb As New StringBuilder
        Dim commonMasterPage As CommonMasterPage = CType(Me.Master, CommonMasterPage)
        Dim sm As ClientScriptManager = Page.ClientScript
        Dim tcvTitle As String = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

        sb.Append("function library() {").Append(vbCrLf)
        sb.Append("  $('#MstPG_TitleLabel').text('").Append(HttpUtility.HtmlEncode(tcvTitle)).Append("');").Append(vbCrLf)
        sb.Append("  $('#MstPG_WindowTitle').text('").Append(tcvTitle).Append("');").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
        sb.Append("    ").Append(sm.GetPostBackEventReference(commonMasterPage, "TCVCallBack")).Append(";").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
        sb.Append("    $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
        sb.Append("  };").Append(vbCrLf).Append(vbCrLf)

        sb.Append("  g_MstPGshowLoding();").Append(vbCrLf)
        sb.Append("  freezeHeaderOperation();").Append(vbCrLf)
        sb.Append("  location.href = 'icrop:tcv:openWindow?jsonData=' + encodeURIComponent('") _
             .Append(HttpUtility.JavaScriptStringEncode(BuildTcvParametersAsJson(STR_DISPID_TCV_LIBRARY))).Append("');").Append(vbCrLf)
        sb.Append("}").Append(vbCrLf)

        'ログ出力 Start *************************************************************
        Logger.Info("BuildOpenLibraryScript End")
        'ログ出力 End ***************************************************************

        Return sb.ToString()

    End Function

    ''' <summary>
    ''' JSON 文字列変換
    ''' </summary>
    ''' <param name="startPageId">初期表示画面ID</param>
    ''' <returns>JSON 文字列に変換した文字列</returns>
    ''' <remarks>オブジェクトをJSON 文字列に変換する</remarks>
    Private Function BuildTcvParametersAsJson(ByVal startPageId As String) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info("BuildTcvParametersAsJson Start")
        'ログ出力 End *****************************************************************************

        Dim tcvParams As New Dictionary(Of String, Object)
        AddTcvParameters(tcvParams, startPageId)
        Dim serializer As New JavaScriptSerializer
        Dim tcvParamsJson As String = serializer.Serialize(tcvParams)

        'ログ出力 Start ***************************************************************************
        Logger.Info("TCV Call parameter[" & startPageId & "]:" & tcvParamsJson)
        Logger.Info("BuildTcvParametersAsJson End")
        'ログ出力 End *****************************************************************************

        Return tcvParamsJson

    End Function

    '2019/05/20 TS  村井 PostUAT-3114 ADD End

#Region " セッション取得・設定バイパス処理 "
    Public Function GetValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String, removeFlg As Boolean) As Object Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.GetValueBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function

    Public Sub SetValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String, value As Object) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.SetValueBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Sub ShowMessageBoxBypass(wordNo As Integer, ParamArray wordParam() As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.ShowMessageBoxBypass
        Me.ShowMessageBox(wordNo, wordParam)
    End Sub

    Public Function ContainsKeyBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) As Boolean Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.ContainsKeyBypass
        Return Me.ContainsKey(pos, key)
    End Function

    Public Sub RemoveValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.RemoveValueBypass
        Me.RemoveValue(pos, key)
    End Sub

    Public Function OperationLockedBypass() As Boolean Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.OperationLockedBypass
        Return Me.OperationLocked
    End Function
#End Region

End Class
