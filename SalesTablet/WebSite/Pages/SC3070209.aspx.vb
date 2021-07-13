'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070209.aspx.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2014/07/15 TCS 高橋
'更新：  
'─────────────────────────────────────

Imports System.Data
Imports System.Data.SqlTypes
Imports System.Globalization
Imports System.Reflection
Imports System.Web.Services
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
Partial Class Pages_SC3070209
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

#End Region

#Region "メンバ変数"

    Private commonMasterPage As CommonMasterPage
    Private mainMenuButton As CommonMasterFooterButton
    Private customerButton As CommonMasterFooterButton
    Private BtnPrint As LinkButton
    Private BtnDiscountApproval As LinkButton

    '呼び出し時の引数
    Protected Account As String
    Protected Dlrcd As String
    Protected Strcd As String
    Protected EstimateId As String
    Protected SelectedEstimateId As String
    Protected SalesFlg As String
    Protected DispModeFlg As String
    Protected ApprovalStatus As String
    Protected NoCustomerFlg As String

#End Region

#Region "画面イベント"
    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub SC3070201_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3070201_Load Start")

        'Test
        initParam()
        'Test

        '契約承認状況取得
        GetContractApproval()

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then

            '初期設定
            InitialSetting()

            '初期データ取得、表示
            DispInitData()

            '画面モード判定
            DispModeSetting()

            If Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) And Not String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
                '活動に紐づく見積管理IDをセッションに設定
                SetEstimateIdSession()
                '見積管理IDをHIDDEN値に設定
                SetEstimateIdHidden()
            End If

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

                Me.actionModeHiddenField.Value = vbEmpty

            Else

            End If

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
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRender Start")

        '注文承認依頼状況取得
        GetContractApproval()

        'セッション値読み込みと読取専用フラグ判定
        'InitTcvParam()

        '画面モード判定
        DispModeSetting()

        '見積作成画面URL設定
        SetEstimateInfoURL()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRender End")

    End Sub

    ''' <summary>
    ''' 画面描画直前イベント全完了時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_PreRenderComplete Start")

        'MG通知一覧制御
        InitMGInfoList()

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

        'URL作成
        Dim sb As New StringBuilder

        sb.Append(EstimateInfoUrl)

        Me.SetValue(ScreenPos.Current, "EstimateId", Me.SelectedEstimateId)
        Me.SetValue(ScreenPos.Current, "SelectedEstimateIndex", "0")

        sb.Append("?")
        sb.Append(Account)
        sb.Append("&")
        sb.Append(Dlrcd)
        sb.Append("&")
        sb.Append(Strcd)
        sb.Append("&EstimateId=")
        sb.Append(Me.EstimateId)
        sb.Append("&SelectedEstimateId=")
        sb.Append(Me.SelectedEstimateId)
        sb.Append("&SalesFlg=")
        sb.Append(Me.SalesFlg)
        sb.Append("&DispModeFlg=")
        sb.Append(Me.DispModeFlg)
        sb.Append("&ApprovalStatus=")
        sb.Append(Me.ApprovalStatus)
        sb.Append("&NoCustomerFlg=")
        sb.Append(Me.NoCustomerFlg)

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

        Me.estimateIdHiddenField.Value = Me.EstimateId
        Me.selectedEstimateIndexHiddenField.Value = "0"

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval End")

    End Sub

    ''' <summary>
    ''' 活動に紐づく見積管理IDをSessionに設定
    ''' </summary>
    ''' <remarks>フォローアップBoxに該当する見積管理IDを全て取得し、セッションに格納する</remarks>
    Private Sub SetEstimateIdSession()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession Start")

        MyBase.SetValue(ScreenPos.Current, "EstimateId", Me.EstimateId)
        MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", 0)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession End")
    End Sub

    ''' <summary>
    ''' 見積管理IDをHiddenに設定
    ''' </summary>
    ''' <remarks>見積管理IDをHiddenに格納する</remarks>
    Private Sub SetEstimateIdHidden()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden Start")

        Me.lngEstimateIdHiddenField.Value = Me.EstimateId
        Me.estimateIdHiddenField.Value = Me.EstimateId
        Me.selectedEstimateIndexHiddenField.Value = "0"

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden End")
    End Sub

    ''' <summary>
    ''' MG通知一覧制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitMGInfoList()
        'TLログイン 且つ 商談中(又は ロック中）の場合、MG通知一覧を非表示にする。
        Me.HideMGInfoList()
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
        'Dim readOnlyFlg As Boolean           '読取専用フラグ

        If Me.ContainsKey(ScreenPos.Current, "OperationCode") Then
            'セッションに格納されている場合はセッション値を使用
            operationCode = CType(Me.GetValue(ScreenPos.Current, "OperationCode", False), Integer)
        Else
            'セッションから取得できない場合はログインユーザのOperationCodeを使用
            Dim staffInfo As StaffContext = StaffContext.Current
            operationCode = StaffContext.Current.OpeCD
        End If

        'HIDDEN値設定
        Me.operationCodeHiddenField.Value = CType(operationCode, String)

        Me.businessFlgHiddenField.Value = CType(False, String)

        Me.readOnlyFlgHiddenField.Value = CType(True, String)

        '回答入力欄非表示
        Me.approvalFieldFlgHiddenField.Value = StrFalse

        'TEST

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitTcvParam End")
    End Sub

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
    ''' 初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitialSetting()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitialSetting Start")

        'セッション情報取得
        Dim lngEstimateId As Long               '見積管理ID
        'Dim blnLockStatus As Boolean            'ロック状態
        Dim estimateId As String
        Dim selectedEstimateIndex As Long

        '見積ID(カンマ区切り)
        'estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)

        '選択している見積IDのIndex
        'If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
        '    selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        'Else
        '    selectedEstimateIndex = 0
        'End If

        estimateId = Me.SelectedEstimateId
        selectedEstimateIndex = 0

        '選択している見積ID
        lngEstimateId = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

        'If Me.ContainsKey(ScreenPos.Current, "MenuLockFlag") Then
        '    blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)
        'Else
        '    blnLockStatus = False
        'End If

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

        Me.ReferenceModeHiddenField.Value = CType(True, String)
        Me.operationLockedHiddenField.Value = Me.ReferenceModeHiddenField.Value

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispModeSetting End")

    End Sub

#Region " セッション取得・設定バイパス処理 "
    Public Function GetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.GetValueBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function

    Public Sub SetValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal value As Object) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.SetValueBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Sub ShowMessageBoxBypass(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.ShowMessageBoxBypass
        Me.ShowMessageBox(wordNo, wordParam)
    End Sub

    Public Function ContainsKeyBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.ContainsKeyBypass
        Return Me.ContainsKey(pos, key)
    End Function

    Public Sub RemoveValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.RemoveValueBypass
        Me.RemoveValue(pos, key)
    End Sub

    Public Function OperationLockedBypass() As Boolean Implements Toyota.eCRB.iCROP.BizLogic.SC3070201.IEstimateInfoControl.OperationLockedBypass
        Return Me.OperationLocked
    End Function
#End Region

#Region "パラメータ取得"
    ''' <summary>
    ''' 呼び出しパラメータ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initParam()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("initParam Start")

        Account = Request("Account")
        Dlrcd = Request("Dlrcd")
        Strcd = Request("Strcd")
        EstimateId = Request("EstimateId")
        SelectedEstimateId = Request("SelectedEstimateId")
        SalesFlg = Request("SalesFlg")
        DispModeFlg = Request("DispModeFlg")
        ApprovalStatus = Request("ApprovalStatus")
        NoCustomerFlg = Request("NoCustomerFlg")

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("initParam End")

    End Sub

#End Region

End Class
