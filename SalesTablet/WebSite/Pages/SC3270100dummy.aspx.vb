'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3720101.aspx.vb
'─────────────────────────────────────
'機能： 受注時説明フレーム
'補足： 
'作成： 2014/03/16 SKFC 下元武
'更新： 
'─────────────────────────────────────
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Pages_SC3270100dummy
    Inherits BasePage

#Region "定数"

#Region "メニューＩＤ"

    ''' <summary>
    ''' メニューＩＤ 顧客詳細ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_SEARCH As Integer = 200
    ''' <summary>
    ''' メニューＩＤ 顧客詳細 試乗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TESTDRIVE As Integer = 201
    ''' <summary>
    ''' メニューＩＤ 顧客詳細 査定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_VAL As Integer = 202
    ''' <summary>
    ''' メニューＩＤ 顧客詳細 ヘルプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_HELP As Integer = 203
    ''' <summary>
    ''' メニューＩＤ 顧客詳細 受注時説明
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_SALESBOOKING_DESCRIPTION As Integer = 204

#End Region

#Region "操作権限コード"

    ''' <summary>
    ''' Call Centre Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_CCM As String = "1"
    ''' <summary>
    ''' Call Centre Operator
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_CCO As String = "2"
    ''' <summary>
    ''' Assistant (H/O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_AHO As String = "3"
    ''' <summary>
    ''' Assistant (Branch)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_AB As String = "4"
    ''' <summary>
    ''' Sales General Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_SGM As String = "5"
    ''' <summary>
    ''' Branch Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_BM As String = "6"
    ''' <summary>
    ''' Sales Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_SSM As String = "7"
    ''' <summary>
    ''' Sales Staff
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_SS As String = "8"
    ''' <summary>
    ''' Service Adviser
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_SA As String = "9"
    ''' <summary>
    ''' Service Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATIONCODE_SM As String = "10"

#End Region

#Region "システム設定パラメータ"

    ''' <summary>
    ''' システム設定パラメータ 受注時説明URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_SYSTEM_SETTING_SALESBOOKING_DESCRIPTION_URL As String = "SALESBOOKING_DESCRIPTION_URL"

#End Region

#Region "販売店システム設定パラメータ"

    ''' <summary>
    ''' 販売店システム設定パラメータ 査定機能使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEM_ENV_SETTING_DLR_USED_FLG_ASSESS As String = "USED_FLG_ASSESS"

#End Region

#Region "フォーマット"

    ''' <summary>
    ''' ログイン時間のフォーマット書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatDateTimeLoginTime As String = "yyyyMMddHHmmss"

#End Region

#Region "セッションキー"

    ''' <summary>
    ''' セッションキー 商談ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALES_ID As String = "SalesId"

    ''' <summary>
    ''' セッションキー 見積管理ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ESTIMATEID As String = "EstimateId"

    ''' <summary>
    ''' セッションキー 受注時説明表示モード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE As String = "SalesbookingDescriptionViewMode"

    ''' <summary>
    ''' セッションキー 契約条件変更フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CONTRACT_ASK_CHG_FLG As String = "ContractAskChgFlg"

    ''' <summary>
    ''' セッションキー 顧客ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CST_ID As String = "CstId"

    ''' <summary>
    ''' セッションキー 顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CST_TYPE As String = "CstType"

    ''' <summary>
    ''' セッションキー 顧客車両区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CST_VCL_TYPE As String = "CstVclType"

    ''' <summary>
    ''' セッションキー 顧客詳細遷移用 商談ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SEARCH_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' セッションキー 顧客詳細遷移用 顧客ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SEARCH_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"

    ''' <summary>
    ''' セッションキー 顧客詳細遷移用 顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SEARCH_KEY_CSTKIND As String = "SearchKey.CSTKIND"

    ''' <summary>
    ''' セッションキー 顧客詳細遷移用 顧客車両区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SEARCH_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' セッションキー 商談中Follow-upBox内連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_SALES As String = "SearchKey.FOLLOW_UP_BOX_SALES"

    ''' <summary>
    ''' セッションキー 注文番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ORDER_NO As String = "SearchKey.ORDER_NO"

#End Region

#End Region

#Region "メンバ変数"


#End Region

#Region "イベント"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load Start")

        ' ヘッダーの制御
        InitHeaderEvent()

        ' フッターの制御
        ' InitFooterEvent()

        '' 受注時説明URLを取得する
        'Dim systemSettingDataTable As SC3270100dummyDataSet.TB_M_SYSTEM_SETTINGDataTable
        'Dim systemSettingSalesbookingDescriptionUrlRow As SC3270100dummyDataSet.TB_M_SYSTEM_SETTINGRow = Nothing
        'Logger.Info("Page_Load Call_Start GetSystemSettingData Param[" & M_SYSTEM_SETTING_SALESBOOKING_DESCRIPTION_URL & "]")
        'systemSettingDataTable = businessLogic.GetSystemSettingData(M_SYSTEM_SETTING_SALESBOOKING_DESCRIPTION_URL)
        'If (IsNothing(systemSettingDataTable) = False) And (systemSettingDataTable.Rows.Count > 0) Then
        '    systemSettingSalesbookingDescriptionUrlRow = systemSettingDataTable.Rows(0)
        '    ' 受注時説明URLを設定する
        '    Me.SalesbookingDescriptionUrl.Value = systemSettingSalesbookingDescriptionUrlRow.SETTING_VAL
        'End If
        'Logger.Info("Page_Load Call_End GetSystemSettingData Ret[" & (Not IsNothing(systemSettingSalesbookingDescriptionUrlRow)) & "]")

        'Logger.Info("Page_Load Param[" & M_SYSTEM_SETTING_SALESBOOKING_DESCRIPTION_URL & "] GetValue=[" & Me.SalesbookingDescriptionUrl.Value & "]")

        ' Getパラメータを指定する
        Dim staff As StaffContext = StaffContext.Current
        ' TBL_USERSよりデータ取得
        Dim users As Users = New Users
        Dim userRow As UsersDataSet.USERSRow
        userRow = users.GetUser(staff.Account)

        Me.UrlParamAccount.Value = userRow.ACCOUNT
        Me.UrlParamUpdateDate.Value = userRow.UPDATEDATE.ToString(FormatDateTimeLoginTime)

        'If ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_ID) Then
        '    Me.UrlParamSalesId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString()
        'End If
        'If ContainsKey(ScreenPos.Current, SESSION_KEY_ESTIMATEID) Then
        '    Me.UrlParamEstimateId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_ESTIMATEID, False).ToString()
        'End If
        'If ContainsKey(ScreenPos.Current, SESSION_KEY_ORDER_NO) Then
        '    Me.UrlParamSalesbkgNum.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_ORDER_NO, False).ToString()
        'End If
        'If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE) Then
        '    Me.UrlParamSalesbookingDescriptionViewMode.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE, False).ToString()
        'End If
        'If ContainsKey(ScreenPos.Current, SESSION_KEY_CONTRACT_ASK_CHG_FLG) Then
        '    Me.UrlParamContractAskChgFlg.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CONTRACT_ASK_CHG_FLG, False).ToString()
        'End If

        Logger.Info("Page_Load End")

    End Sub


    Protected Sub BtnMove_Click(sender As Object, e As System.EventArgs) Handles BtnMove.Click

        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALES_ID, Me.TxtFollowUpBox.Text)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_ESTIMATEID, Me.TxtEstimateId.Text)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE, Me.TxtViewMode.Text)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CONTRACT_ASK_CHG_FLG, Me.TxtContractAskChangeFlag.Text)
		Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_ID, Me.TxtCstId.Text)
		Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_TYPE, Me.TxtCstType.Text)
		Me.SetValue(ScreenPos.Next, SESSION_KEY_CST_VCL_TYPE, Me.TxtCstVclType.Text)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_ORDER_NO, Me.TxtOrderID.Text)

        Me.RedirectNextScreen("SC3270101")

    End Sub


    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("tcvButton_Click Start")

        Dim context As StaffContext = StaffContext.Current

        ''TCV機能に渡す引数を設定
        'e.Parameters.Add("DataSource", "none")
        'e.Parameters.Add("MenuLockFlag", False)
        'e.Parameters.Add("Account", context.Account)
        'e.Parameters.Add("AccountStrCd", context.BrnCD)
        'e.Parameters.Add("DlrCd", context.DlrCD)
        'e.Parameters.Add("StrCd", String.Empty)
        'e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        'e.Parameters.Add("CstKind", String.Empty)
        'e.Parameters.Add("CustomerClass", String.Empty)
        'e.Parameters.Add("CRCustId", String.Empty)
        'e.Parameters.Add("OperationCode", context.OpeCD)
        'e.Parameters.Add("BusinessFlg", False)
        'e.Parameters.Add("ReadOnlyFlg", False)

        Dim businessFlg As Boolean = False
        Dim readOnlyFlg As Boolean = True
        If IsSales() Then
            businessFlg = True
            readOnlyFlg = False
        End If

        Dim opeCd As Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation = StaffContext.Current.OpeCD
        Dim estimateId As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_ESTIMATEID, False).ToString()

        If estimateId.Length <= 0 Then
            '見積管理IDがない場合

            e.Parameters.Add("DataSource", "None")
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StrCd", context.BrnCD)
            e.Parameters.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString())
            e.Parameters.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False).ToString())
            e.Parameters.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False).ToString())
            e.Parameters.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False).ToString())
            'e.Parameters.Add("StartPageId", String.Empty)
            'e.Parameters.Add("SelectedEstimateIndex", String.Empty)
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("OperationCode", opeCd)
            e.Parameters.Add("BusinessFlg", businessFlg)
            e.Parameters.Add("ReadOnlyFlg", readOnlyFlg)
        Else
            '見積管理IDがある場合

            If readOnlyFlg = False Then
                For Each estId In estimateId.Split(","c)
                    readOnlyFlg = GetContractFlg(estId)
                    If readOnlyFlg Then
                        Exit For
                    End If
                Next
            End If

            e.Parameters.Add("DataSource", "EstimateId")
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StrCd", context.BrnCD)
            e.Parameters.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString())
            e.Parameters.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False).ToString())
            e.Parameters.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False).ToString())
            e.Parameters.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False).ToString())
            e.Parameters.Add("StartPageId", "SC3050201")
            e.Parameters.Add("EstimateId", estimateId)
            e.Parameters.Add("SelectedEstimateIndex", "0")
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("OperationCode", opeCd)
            e.Parameters.Add("BusinessFlg", businessFlg)
            e.Parameters.Add("ReadOnlyFlg", readOnlyFlg)
        End If

        Logger.Info("tcvButton_Click End")

    End Sub

#End Region

#Region "非公開メソッド"

    ''' <summary>
    ''' フッターの制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, ByRef category As Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory) As Integer()

        ' TODO: 自ベージの所属メニュー、顧客詳細とTCVがあるが、どうするか
        category = FooterMenuCategory.Customer

        Return New Integer() {SUBMENU_TESTDRIVE, SUBMENU_VAL, SUBMENU_HELP, SUBMENU_SALESBOOKING_DESCRIPTION}

    End Function

    ''' <summary>
    ''' ヘッダーの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        Logger.Info("InitHeaderEvent Start")

        '' 戻るボタン非活性化
        'CType(Me.Master, CommonMasterPage).IsRewindButtonEnabled = False

        '' 検索エリア非活性化
        'CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False

        Logger.Info("InitHeaderEvent End")

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Logger.Info("InitFooterEvent Start")

        Dim staff As StaffContext = StaffContext.Current

        ' 権限によりフッタボタンの制御を行う

        ' メインメニュー
        Dim mainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        ' 商談(一時対応・営業活動・納車作業)中の場合、メインメニューを非活性
        If IsSales() Then
            mainButton.Enabled = False
        Else
            mainButton.Enabled = True
            AddHandler mainButton.Click, _
                Sub()
                    ' SCメインに遷移
                    Me.RedirectNextScreen("SC3010203")
                End Sub
        End If

        mainButton.OnClientClick = "return false;"


        ' 顧客詳細
        Dim customerButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
        customerButton.Enabled = True
        customerButton.Selected = False
        AddHandler customerButton.Click, _
            Sub()
                ' 顧客詳細に遷移する
                Me.RedirectNextScreen("SC3080201")
            End Sub

        customerButton.OnClientClick = "return false;"


        ' 試乗
        Dim customerTestDriveButtion As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE)
        ' 査定
        Dim customerValButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL)
        ' ヘルプ
        Dim customerHelpButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP)

        'ログイン権限がセールススタッフでない場合、フッターボタンを非表示にする
        Dim opeCD As Integer = StaffContext.Current.OpeCD
        Dim ssf As Integer = Operation.SSF
        If opeCD <> ssf Then
            customerTestDriveButtion.Visible = False
            customerValButton.Visible = False
            customerHelpButton.Visible = False
        End If

        '査定機能が蓋締めの場合、非表示にする
        Dim dlrEnvDt As New DealerEnvSetting
        Dim dlrEnvRow As DlrEnvSettingDataSet.DLRENVSETTINGRow
        dlrEnvRow = dlrEnvDt.GetEnvSetting(StaffContext.Current.DlrCD, SYSTEM_ENV_SETTING_DLR_USED_FLG_ASSESS)
        Dim assessFlg As String = dlrEnvRow.PARAMVALUE
        If String.Equals(assessFlg, "0") Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL).Visible = False
        End If

        ' 試乗を活性、査定・ヘルプを非活性にする
        customerTestDriveButtion.Enabled = True
        customerValButton.Enabled = False
        customerHelpButton.Enabled = False

        'Follow-upBox内連番がある場合、査定・ヘルプを活性にする
        If IsSession(SESSION_KEY_SALES_ID) Then
            customerValButton.Enabled = True
            customerHelpButton.Enabled = True
        End If

        '現在表示している活動と、商談中の活動が異なる場合、査定・ヘルプを非活性にする
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            Dim fllwSeq As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString()
            Dim fllwSeqSales As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False).ToString()
            If Not String.Equals(fllwSeq, fllwSeqSales) Then
                customerValButton.Enabled = False
                customerHelpButton.Enabled = False
            End If
        End If

        customerTestDriveButtion.OnClientClick = "return false;"
        customerValButton.OnClientClick = "return showAssessmentPopup();"
        customerHelpButton.OnClientClick = "return false;"


        ' ショールームステータス
        Dim showRoomStatusButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        If showRoomStatusButton IsNot Nothing Then
            showRoomStatusButton.Visible = False
        End If

        ' TCV
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        If tcvButton IsNot Nothing Then
            AddHandler tcvButton.Click, AddressOf tcvButton_Click
            tcvButton.OnClientClick = "return false;"
        End If


        ' TODO：ここに納車時説明、SPM、KPIボタンの処理記載予定


        Logger.Info("InitFooterEvent End")

    End Sub

    ''' <summary>
    ''' 商談(一時対応・営業活動・納車作業)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>ステータスを参照して商談中か判断する</remarks>
    Private Function IsSales() As Boolean

        Logger.Debug("IsSales Start")

        Dim presenceCategory As String = StaffContext.Current.PresenceCategory
        Dim presenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(presenceCategory, "1") And String.Equals(presenceDetail, "1")) Or
            (String.Equals(presenceCategory, "2") And String.Equals(presenceDetail, "0")) Or
            (String.Equals(presenceCategory, "2") And String.Equals(presenceDetail, "1")) Or
            (String.Equals(presenceCategory, "2") And String.Equals(presenceDetail, "2")) Or
            (String.Equals(presenceCategory, "2") And String.Equals(presenceDetail, "3")) Then

            Logger.Debug("IsSales End")
            Return True
        Else

            Logger.Debug("IsSales End")
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

        Logger.Debug("IsSession Start")

        If Me.ContainsKey(ScreenPos.Current, sessionName) Then
            If Not String.IsNullOrEmpty(Me.GetValue(ScreenPos.Current, sessionName, False).ToString()) Then

                Logger.Debug("IsSession End")
                Return True
            End If
        End If

        Logger.Debug("IsSession End")
        Return False
    End Function

    ''' <summary>
    ''' 契約状況取得処理
    ''' </summary>
    ''' <param name="EstimateId">見積ID</param>
    ''' <returns>True:契約済み False:契約済み以外</returns>
    ''' <remarks></remarks>
    Private Function GetContractFlg(ByVal estimateId As String) As Boolean

        'Logger.Debug("GetContractFlg Start")

        'Dim result As SC3080201ContractDataTable = Nothing
        'Dim rtnFlg As Boolean = True

        'Using param As New SC3080201ESTIMATEINFODataTable
        '    Dim conditionRow As SC3080201ESTIMATEINFORow = param.NewSC3080201ESTIMATEINFORow
        '    conditionRow.ESTIMATEID = EstimateId

        '    ' 検索条件を登録
        '    param.AddSC3080201ESTIMATEINFORow(conditionRow)

        '    ' 検索処理
        '    result = SC3080201BusinessLogic.GetContractFlg(param)
        'End Using

        'Logger.Debug("GetContractFlg End")

        '' 処理結果返却
        'If result.Rows.Count > 0 Then
        '    Dim dr As SC3080201DataSet.SC3080201ContractRow = CType(result.Rows(0), SC3080201DataSet.SC3080201ContractRow)
        '    If Not (dr.CONTRACT_APPROVAL_STATUS.Equals("1") OrElse dr.CONTRACT_APPROVAL_STATUS.Equals("2")) Then
        '        rtnFlg = False
        '    End If
        'End If

        'Return rtnFlg

        ' TODO:
        Return True

    End Function

#End Region

End Class
