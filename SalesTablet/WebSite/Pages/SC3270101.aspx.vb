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
Imports Toyota.eCRB.iCROP.BizLogic.SC3270101
Imports Toyota.eCRB.iCROP.DataAccess.SC3270101
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Pages_SC3270101
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
    Private Const SUBMENU_BOOKING_EXPLAIN As Integer = 205

    ''' <summary>
    ''' メニューID TCV 見積作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_ESTIMATEINFO As Integer = 305

    ''' <summary>
    ''' メニューＩＤ TCV 受注時説明
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_SALESBKG_DESCRIPTION As Integer = 306

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
    Private Const M_SYSTEM_SETTING_SALESBKG_DESCRIPTION_URL As String = "SALESBKG_DESCRIPTION_URL"

#End Region

#Region "販売店システム設定パラメータ"

    ''' <summary>
    ''' 販売店システム設定パラメータ 査定機能使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEM_ENV_SETTING_DLR_USED_FLG_ASSESS As String = "USED_FLG_ASSESS"

#End Region

#Region "文言コード"

    ''' <summary>
    ''' 文言コード プレビューボタンラベル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_CD_PREVIEW_BUTTON_LABEL As String = "15736"

    ''' <summary>
    ''' 文言コード 保存ボタンラベル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_CD_SAVE_BUTTON_LABEL As String = "16366"

    ''' <summary>
    ''' 文言コード 変更時の確認メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_CD_MODIFY_MESSAGE As String = "17252"

    ''' <summary>
    ''' 文言コード 受注時説明画面タイトル
    ''' </summary>
    Private Const WORD_CD_TITLE As String = "20046"

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

#Region "受注時説明モード"

    ''' <summary>
    ''' 受注時説明モード お客様ご説明モード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESBOOKING_DESCRIPTION_VIEW_MODE_CUSTOMER As String = "1"

    ''' <summary>
    ''' 受注時説明モード スタッフ予定変更モード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESBOOKING_DESCRIPTION_VIEW_MODE_STAFF As String = "2"

#End Region

#Region "見積の契約状況"

    ''' <summary>
    ''' 見積の契約状況 1:契約済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATEINFO_CONTRACT As String = "1"

#End Region

#Region "画面ID"

    ''' <summary>
    ''' 画面ID SCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_SCMAIN As String = "SC3010203"

    ''' <summary>
    ''' 画面ID 顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_CUSTOMER As String = "SC3080201"

    ''' <summary>
    ''' 画面ID ショールームステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_SHOWROOM_STATUS As String = "SC3100101"

    ''' <summary>
    ''' 画面ID 見積作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_ESTIMATE As String = "SC3070201"

#End Region

#Region "TCV向けの初期表示画面ID"

    ''' <summary>
    ''' TCV向けの初期表示画面ID デフォルト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_START_PAGE_ID_DEFAULT As String = "N-CV-10"

#End Region


#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3270101BusinessLogic

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
        InitFooterEvent()

        ' 受注時説明URLを取得する
        Dim systemSettingDataTable As SC3270101DataSet.SC3270101SystemSettingDataTable
        Dim systemSettingSalesbookingDescriptionUrlRow As SC3270101DataSet.SC3270101SystemSettingRow = Nothing
        Logger.Info("Page_Load Call_Start GetSystemSettingData Param[" & M_SYSTEM_SETTING_SALESBKG_DESCRIPTION_URL & "]")
        systemSettingDataTable = businessLogic.GetSystemSettingData(M_SYSTEM_SETTING_SALESBKG_DESCRIPTION_URL)
        If (IsNothing(systemSettingDataTable) = False) And (systemSettingDataTable.Rows.Count > 0) Then
            systemSettingSalesbookingDescriptionUrlRow = systemSettingDataTable.Rows(0)
            ' 受注時説明URLを設定する
            Me.SalesbookingDescriptionUrl.Value = systemSettingSalesbookingDescriptionUrlRow.SETTING_VAL
        End If
        Logger.Info("Page_Load Call_End GetSystemSettingData Ret[" & (Not IsNothing(systemSettingSalesbookingDescriptionUrlRow)) & "]")

        Logger.Info("Page_Load Param[" & M_SYSTEM_SETTING_SALESBKG_DESCRIPTION_URL & "] GetValue=[" & Me.SalesbookingDescriptionUrl.Value & "]")

        Logger.Debug(String.Format("Page_Load Param[{0}] GetValue=[{1}]", M_SYSTEM_SETTING_SALESBKG_DESCRIPTION_URL, Me.SalesbookingDescriptionUrl.Value))

        ' 文言を取得する
        GetWords()

        ' Getパラメータを指定する
        Dim staff As StaffContext = StaffContext.Current
        ' TBL_USERSよりデータ取得
        Dim users As Users = New Users
        Dim userRow As UsersDataSet.USERSRow
        userRow = users.GetUser(staff.Account)

        ' ユーザーのアカウント
        Me.UrlParamAccount.Value = userRow.ACCOUNT
        ' ユーザーの更新日時
        Me.UrlParamUpdateDate.Value = userRow.UPDATEDATE.ToString(FormatDateTimeLoginTime)
        ' 商談ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_ID) Then
            Me.UrlParamSalesId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString()
        End If
        ' 見積管理ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ESTIMATEID) Then
            Me.UrlParamEstimateId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_ESTIMATEID, False).ToString()
        End If
        ' TODO: 注文番号
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ORDER_NO) Then
            Me.UrlParamSalesbkgNum.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_ORDER_NO, False).ToString()
        End If
        ' 受注時説明表示モード
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE) Then
            Me.UrlParamSalesbookingDescriptionViewMode.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE, False).ToString()
        End If
        ' 契約条件変更フラグ
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CONTRACT_ASK_CHG_FLG) Then
            Me.UrlParamContractAskChgFlg.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CONTRACT_ASK_CHG_FLG, False).ToString()
        End If
        ' 顧客ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CST_ID) Then
            Me.UrlParamCstId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False).ToString()
        End If
		' 顧客種別
		Dim cstType As String = String.Empty
		If ContainsKey(ScreenPos.Current, SESSION_KEY_CST_TYPE) Then
			cstType = Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False).ToString()
		End If
		' 顧客車両区分
		Dim cstVclType As String = String.Empty
		If ContainsKey(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE) Then
			cstVclType = Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False).ToString()
		End If



        ' パラメタをログに出力
		Logger.Debug(String.Format("Params: SalesId:{0}; EstimateId:{1}; SalesbkgNum:{2}; SalesbookingDescriptionViewMode:{3}; ContractAskChgFlg:{4}; CstId:{5}; CstType:{6}; CstVclType:{7}; ",
				 Me.UrlParamSalesId.Value,
				 Me.UrlParamEstimateId.Value,
				 Me.UrlParamSalesbkgNum.Value,
				 Me.UrlParamSalesbookingDescriptionViewMode.Value,
				 Me.UrlParamContractAskChgFlg.Value,
				 Me.UrlParamCstId.Value,
				 cstType,
				 cstVclType))

        Logger.Info("Page_Load End")

    End Sub

    ''' <summary>
    ''' メインメニューボタンのクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub mainButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        ' SCメインに遷移する
        Me.RedirectNextScreen(DISPID_SCMAIN)

        ' パラメタをログに出力
        Logger.Debug(String.Format("mainButton_Click:{0}", DISPID_SCMAIN))

    End Sub

    ''' <summary>
    ''' 顧客詳細ボタンのクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub customerButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        ' 顧客詳細に遷移する
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SEARCH_KEY_FOLLOW_UP_BOX, Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False))
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SEARCH_KEY_CRCUSTID, Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False))
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SEARCH_KEY_CSTKIND, Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False))
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SEARCH_KEY_CUSTOMERCLASS, Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False))

        Me.RedirectNextScreen(DISPID_CUSTOMER)

        ' パラメタをログに出力
        Logger.Debug(String.Format("customerButton_Click:{0}, SESSION_KEY_SEARCH_KEY_FOLLOW_UP_BOX:{1}, SESSION_KEY_SEARCH_KEY_CRCUSTID:{2}, SESSION_KEY_SEARCH_KEY_CSTKIND:{3}, SESSION_KEY_SEARCH_KEY_CUSTOMERCLASS:{4}",
                                   DISPID_CUSTOMER,
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False)))

    End Sub

    ''' <summary>
    ''' カンマ区切り見積ID等を引数に設定する
    ''' </summary>
    ''' <param name="paramEstimateIdStringBuilder">カンマ区切り見積ID</param>
    ''' <param name="paramSelectedEstimateIndex">選択している見積IDのindex</param>
    ''' <param name="existsContract">契約済の見積が存在するかどうか</param>
    ''' <remarks></remarks>
    Private Sub SetEstimateIdCsv(ByRef paramEstimateIdStringBuilder As StringBuilder, ByRef paramSelectedEstimateIndex As Integer, ByRef existsContract As Boolean)

        ' 商談IDから見積IDを取得する
        Dim salesId As Decimal = 0
        If Not Decimal.TryParse(Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString(), salesId) Then
            salesId = 0
        End If

        Dim myEstimateId As Long = 0
        If Not Long.TryParse(Me.GetValue(ScreenPos.Current, SESSION_KEY_ESTIMATEID, False).ToString(), myEstimateId) Then
            myEstimateId = 0
        End If

        Dim estimatesDataTable As SC3270101DataSet.SC3270101EstimateInfoDataTable = businessLogic.GetEstimateData(salesId)

        If (estimatesDataTable IsNot Nothing) AndAlso (estimatesDataTable.Rows.Count > 0) Then
            Dim estimateIdx As Integer = 0
            For Each estimateDataRow As SC3270101DataSet.SC3270101EstimateInfoRow In estimatesDataTable.Rows
                Dim estimateId As Long = estimateDataRow.ESTIMATEID

                ' カンマ区切りの見積IDを作成する
                If paramEstimateIdStringBuilder.Length > 0 Then
                    paramEstimateIdStringBuilder.Append(",")
                End If

                paramEstimateIdStringBuilder.Append(estimateId.ToString())

                ' 選択している見積IDのindexを特定する
                If estimateId = myEstimateId Then
                    paramSelectedEstimateIndex = estimateIdx
                End If

                ' 契約済みの見積があるかどうかを保持する
                If ESTIMATEINFO_CONTRACT.Equals(estimateDataRow.CONTRACTFLG) Then
                    existsContract = True
                End If

                estimateIdx += 1
            Next
        End If

    End Sub


    ''' <summary>
    ''' ショールームステータスボタンのクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub showRoomStatusButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("showRoomStatusButton_Click Start")

        Me.RedirectNextScreen(DISPID_SHOWROOM_STATUS)

        Logger.Info("showRoomStatusButton_Click End")

    End Sub

    ''' <summary>
    ''' TCVボタンのクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("tcvButton_Click Start")

        ' カンマ区切り見積IDと、契約済の見積が存在するかどうかを取得する
        Dim paramEstimateIdStringBuilder As StringBuilder = New StringBuilder()
        Dim paramSelectedEstimateIndex As Integer = 0
        Dim existsContract As Boolean = False
        SetEstimateIdCsv(paramEstimateIdStringBuilder, paramSelectedEstimateIndex, existsContract)

        Dim businessFlg As Boolean = False
        If IsSales() Then
            businessFlg = True
        End If

        ' 見積が契約済ならReadOnlyFlg=True
        Dim readOnlyFlg As Boolean = False
        If existsContract Then
            readOnlyFlg = True
        End If

        Dim context As StaffContext = StaffContext.Current

        e.Parameters.Add("DataSource", "EstimateId")
        e.Parameters.Add("MenuLockFlag", False)
        e.Parameters.Add("CloseCallback", "closeCallbackFunction")
        e.Parameters.Add("StatusCallback", "statusCallbackFunction")
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("DlrCd", context.DlrCD)
        e.Parameters.Add("StrCd", String.Empty)
        e.Parameters.Add("StartPageId", TCV_START_PAGE_ID_DEFAULT)
        e.Parameters.Add("EstimateId", paramEstimateIdStringBuilder.ToString())
        e.Parameters.Add("SelectedEstimateIndex", paramSelectedEstimateIndex)
        e.Parameters.Add("OperationCode", context.OpeCD)
        e.Parameters.Add("BusinessFlg", businessFlg)
        e.Parameters.Add("ReadOnlyFlg", readOnlyFlg)


        ' パラメタをログに出力
        Dim log As String = "Params:"
        For Each key In e.Parameters.Keys
            log += String.Format("{0}:{1}", key, e.Parameters(key).ToString())
        Next
        Logger.Debug(String.Format("tcvButton_Click:{0}", log))

        Logger.Info("tcvButton_Click End")

    End Sub

    ''' <summary>
    ''' TCV 見積ボタンのクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub tcvEstimateButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("tcvEstimateButton_Click Start")

        ' カンマ区切り見積IDを取得する
        Dim paramEstimateIdStringBuilder As StringBuilder = New StringBuilder()
        Dim paramSelectedEstimateIndex As Integer = 0
        Dim existsContract As Boolean = False
        SetEstimateIdCsv(paramEstimateIdStringBuilder, paramSelectedEstimateIndex, existsContract)

        Me.SetValue(ScreenPos.Next, "MenuLockFlag", False)
        Me.SetValue(ScreenPos.Next, "StartPageId", DISPID_ESTIMATE)
        Me.SetValue(ScreenPos.Next, "EstimateId", paramEstimateIdStringBuilder.ToString())
        Me.SetValue(ScreenPos.Next, "SelectedEstimateIndex", paramSelectedEstimateIndex)

        Me.RedirectNextScreen(DISPID_ESTIMATE)

        ' パラメタをログに出力
        Logger.Debug(String.Format("tcvEstimateButton_Click:{0}, MenuLockFlag:{1}, StartPageId:{2}, EstimateId:{3}, SelectedEstimateIndex:{4}",
                                   DISPID_ESTIMATE, "False", DISPID_ESTIMATE, paramEstimateIdStringBuilder.ToString(), paramSelectedEstimateIndex))

        Logger.Info("tcvEstimateButton_Click End")

    End Sub

    ''' <summary>
    ''' 納車時説明ボタン
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub newCarExplainButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Debug("newCarExplainButton_Click Start")
        '納車時説明ツールへ遷移するためのパラメータを設定する

        ' 商談ID
        Me.SetValue(ScreenPos.Next, "SalesId", Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False))
        ' 顧客ID
        Me.SetValue(ScreenPos.Next, "CstId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False))
        ' 顧客種別
        Me.SetValue(ScreenPos.Next, "CstType", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False))
        ' 顧客車両区分
        Me.SetValue(ScreenPos.Next, "CstVclType", Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False))

        ' パラメタをログに出力
        Logger.Debug(String.Format("newCarExplainButton_Click: SalesId:{0}, CstId:{1}, CstType:{2}, CstVclType:{3}",
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_ID, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_TYPE, False),
                                   Me.GetValue(ScreenPos.Current, SESSION_KEY_CST_VCL_TYPE, False)))

        Logger.Debug("newCarExplainButton_Click End")

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

        ' 受注時説明表示モードを取得しておく
        Dim viewMode As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE) Then
            viewMode = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE, False).ToString()
        End If

        ' 顧客詳細からの遷移＝受注時説明表示モードがスタッフ予定変更モード
        ' TCVからの遷移＝受注時説明表示モードがお客様説明モード
        If viewMode = SALESBOOKING_DESCRIPTION_VIEW_MODE_STAFF Then
            category = FooterMenuCategory.Customer
            Return New Integer() {SUBMENU_TESTDRIVE, SUBMENU_VAL, SUBMENU_HELP, SUBMENU_BOOKING_EXPLAIN}
        Else
            category = FooterMenuCategory.TCV
            Return New Integer() {SUBMENU_TCV_ESTIMATEINFO, SUBMENU_TCV_SALESBKG_DESCRIPTION}
        End If

    End Function

    ''' <summary>
    ''' ヘッダーの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        Logger.Info("InitHeaderEvent Start")

        ' 戻るボタン非活性化
        CType(Me.Master, CommonMasterPage).IsRewindButtonEnabled = False

        ' 検索エリア非活性化
        CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False
        CType(Me.Master, CommonMasterPage).SearchBox.Visible = False

        'ログアウト
        ' '活動破棄チェックのクライアントサイドスクリプトを埋め込む ※保存が可能な商談中は、そもそもコンテキストメニューを表示しないため、処理不要
        ' CType(Me.Master, CommonMasterPage).GetHeaderButton(HeaderButton.Logout).OnClientClick = "return inputUpdateCheck();"

        If IsSales() Then
            '商談中 の場合はi-cropアイコン使用不可
            CType(Me.Master, CommonMasterPage).ContextMenu.Enabled = False
        Else
            '上記以外は場合はi-cropアイコン使用可能
            CType(Me.Master, CommonMasterPage).ContextMenu.Enabled = True
        End If


        Logger.Info("InitHeaderEvent End")

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Logger.Info("InitFooterEvent Start")

        Dim staff As StaffContext = StaffContext.Current
        Dim commonMasterPageRef As CommonMasterPage = CType(Me.Master, CommonMasterPage)

        ' 権限によりフッタボタンの制御を行う

        ' 受注時説明表示モードを取得しておく
        Dim viewMode As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE) Then
            viewMode = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESBOOKING_DESCRIPTION_VIEW_MODE, False).ToString()
        End If



        ' メインメニュー --------------------------------------
        Dim mainButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.MainMenu)

        ' 商談(一時対応・営業活動・納車作業)中の場合、メインメニューを非活性
        If IsSales() Then
            mainButton.Enabled = False
        Else
            mainButton.Enabled = True
            AddHandler mainButton.Click, AddressOf mainButton_Click
        End If

        mainButton.OnClientClick = "return inputUpdateCheck();"


        ' 顧客詳細 ----------------------------------------------
        Dim customerButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.Customer)
        customerButton.Enabled = True
        customerButton.Selected = False
        AddHandler customerButton.Click, AddressOf customerButton_Click
        customerButton.OnClientClick = "return inputUpdateCheck();"

        ' 顧客詳細 子フッター --------------------------------------------
        ' 試乗
        Dim customerTestDriveButtion As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_TESTDRIVE)
        ' 査定
        Dim customerValButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_VAL)
        ' ヘルプ
        Dim customerHelpButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_HELP)
        ' 受注時説明
        Dim customerSalesbkgDescriptionButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_BOOKING_EXPLAIN)

        ' 顧客詳細 子フッターは、顧客詳細からの遷移＝受注時説明表示モードがスタッフ予定変更モードのときのみ表示する
        If viewMode = SALESBOOKING_DESCRIPTION_VIEW_MODE_STAFF Then

            ' 試乗、査定、ヘルプは非表示
            If customerTestDriveButtion IsNot Nothing Then
                customerTestDriveButtion.Visible = False
            End If

            If customerValButton IsNot Nothing Then
                customerValButton.Visible = False
            End If

            If customerHelpButton IsNot Nothing Then
                customerHelpButton.Visible = False
            End If

            ''査定機能が蓋締めの場合、非表示にする
            'Dim dlrEnvDt As New DealerEnvSetting
            'Dim dlrEnvRow As DlrEnvSettingDataSet.DLRENVSETTINGRow
            'dlrEnvRow = dlrEnvDt.GetEnvSetting(StaffContext.Current.DlrCD, SYSTEM_ENV_SETTING_DLR_USED_FLG_ASSESS)
            'Dim assessFlg As String = dlrEnvRow.PARAMVALUE
            'If String.Equals(assessFlg, "0") Then
            '    commonMasterPageRef.GetFooterButton(SUBMENU_VAL).Visible = False
            'End If

            '' 試乗を活性、査定・ヘルプを非活性にする
            'customerTestDriveButtion.Enabled = True
            'customerValButton.Enabled = False
            'customerHelpButton.Enabled = False

            ''商談IDがある場合、査定・ヘルプを活性にする
            'If IsSession(SESSION_KEY_SALES_ID) Then
            '    customerValButton.Enabled = True
            '    customerHelpButton.Enabled = True
            'End If

            ''現在表示している活動と、商談中の活動が異なる場合、査定・ヘルプを非活性にする
            'If IsSession(SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            '    Dim fllwSeq As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALES_ID, False).ToString()
            '    Dim fllwSeqSales As String = Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False).ToString()
            '    If Not String.Equals(fllwSeq, fllwSeqSales) Then
            '        customerValButton.Enabled = False
            '        customerHelpButton.Enabled = False
            '    End If
            'End If

            'customerTestDriveButtion.OnClientClick = "return inputUpdateCheck();"
            'customerValButton.OnClientClick = "return inputUpdateCheck();"
            'customerHelpButton.OnClientClick = "return inputUpdateCheck();"

            ' 受注時説明は選択状態にする
            customerSalesbkgDescriptionButton.Selected = True
            customerSalesbkgDescriptionButton.Enabled = False
        Else
            If customerTestDriveButtion IsNot Nothing Then
                customerTestDriveButtion.Visible = False
            End If

            If customerValButton IsNot Nothing Then
                customerValButton.Visible = False
            End If

            If customerHelpButton IsNot Nothing Then
                customerHelpButton.Visible = False
            End If

            If customerSalesbkgDescriptionButton IsNot Nothing Then
                customerSalesbkgDescriptionButton.Visible = False
            End If
        End If


        ' ショールームステータス --------------------------------------------
        Dim showRoomStatusButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        If showRoomStatusButton IsNot Nothing Then

            ' Sales Manager ならば 表示
            If StaffContext.Current.OpeCD = OPERATIONCODE_SSM Then
                showRoomStatusButton.Visible = True

                ' 商談(一時対応・営業活動・納車作業)中の場合、非活性
                If IsSales() Then
                    showRoomStatusButton.Enabled = False
                Else
                    AddHandler showRoomStatusButton.Click, AddressOf showRoomStatusButton_Click
                    showRoomStatusButton.OnClientClick = "return inputUpdateCheck();"
                End If
            Else
                showRoomStatusButton.Visible = False
            End If

        End If


        ' TCV ---------------------------------------------
        Dim tcvButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.TCV)
        If tcvButton IsNot Nothing Then
            AddHandler tcvButton.Click, AddressOf tcvButton_Click
            tcvButton.OnClientClick = "return inputUpdateCheck();"
            tcvButton.Selected = False
        End If

        ' TCV 見積 -----------------------------------------
        Dim tcvEstimateButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_TCV_ESTIMATEINFO)
        If tcvEstimateButton IsNot Nothing Then
            If viewMode = SALESBOOKING_DESCRIPTION_VIEW_MODE_CUSTOMER Then
                tcvEstimateButton.Visible = True
                AddHandler tcvEstimateButton.Click, AddressOf tcvEstimateButton_Click
                tcvEstimateButton.OnClientClick = "return inputUpdateCheck();"
            Else
                tcvEstimateButton.Visible = False
            End If
        End If

        ' TCV 受注時説明ツール ------------------------------
        ' TCVからの遷移＝受注時説明表示モードがお客様説明モードのときのみ表示する
        Dim tcvSalesbkgDescriptionButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(SUBMENU_TCV_SALESBKG_DESCRIPTION)
        If tcvSalesbkgDescriptionButton IsNot Nothing Then
            If viewMode = SALESBOOKING_DESCRIPTION_VIEW_MODE_CUSTOMER Then
                tcvSalesbkgDescriptionButton.Visible = True
                tcvSalesbkgDescriptionButton.Selected = True
                tcvSalesbkgDescriptionButton.Enabled = False
            Else
                tcvSalesbkgDescriptionButton.Visible = False
            End If
        End If


        'SPM -------------------------------------------------------
        'カテゴリ値は基盤の定数を使用する ※今は仮置き  ←見積もり画面での記載を参考

        Dim spmButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.SPM)
        If spmButton IsNot Nothing Then
	        ' 商談(一時対応・営業活動・納車作業)中の場合、非活性
	        If IsSales() Then
	            spmButton.Enabled = False
	        Else
	            spmButton.Enabled = True
	            'AddHandler spmButton.Click, AddressOf spmButton_Click
	            spmButton.OnClientClick = "return inputUpdateCheck();"
	        End If
        End If

        '納車時説明ツール -----------------------------------------
        Dim newCarExplainButton As CommonMasterFooterButton = commonMasterPageRef.GetFooterButton(FooterMenuCategory.NewCarExplain)
        If newCarExplainButton IsNot Nothing Then
            AddHandler newCarExplainButton.Click, AddressOf newCarExplainButton_Click
            newCarExplainButton.OnClientClick = "return inputUpdateCheck();"
        End If


        Logger.Info("InitFooterEvent End")

    End Sub

    ''' <summary>
    ''' 商談(一時対応・営業活動・納車作業)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>
    ''' ステータスを参照して商談中か判断する
    '''    ※下記★マークをTrueとして返す
    '''    大分類 小分類   状態
    '''    -------------------------------------
    '''    1      0        スタンバイ
    '''    1      1        スタンバイ（営業活動中）★
    '''    2      0        商談中★
    '''    2      1        商談中（一時対応）★
    '''    2      2        納車作業中★
    '''    2      3        納車作業中(一時対応)★
    '''    3      0        退席中
    '''    4      0        オフライン
    ''' </remarks>
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

    ''' <summary>
    ''' 文言を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetWords()

        Dim wordDataTable As SC3270101DataSet.SC3270101WordDataTable
        Dim wordDataRow As SC3270101DataSet.SC3270101WordRow

        wordDataTable = businessLogic.GetWordData(WORD_CD_PREVIEW_BUTTON_LABEL)
        If (IsNothing(wordDataTable) = False) And (wordDataTable.Rows.Count > 0) Then
            wordDataRow = wordDataTable.Rows(0)
            Me.PreviewButtonLabel.Text = If(String.IsNullOrWhiteSpace(wordDataRow.WORD_VAL), wordDataRow.WORD_VAL_ENG, wordDataRow.WORD_VAL)
        End If

        wordDataTable = businessLogic.GetWordData(WORD_CD_SAVE_BUTTON_LABEL)
        If (IsNothing(wordDataTable) = False) And (wordDataTable.Rows.Count > 0) Then
            wordDataRow = wordDataTable.Rows(0)
            Me.SaveButtonLabel.Text = If(String.IsNullOrWhiteSpace(wordDataRow.WORD_VAL), wordDataRow.WORD_VAL_ENG, wordDataRow.WORD_VAL)
        End If

        wordDataTable = businessLogic.GetWordData(WORD_CD_MODIFY_MESSAGE)
        If (IsNothing(wordDataTable) = False) And (wordDataTable.Rows.Count > 0) Then
            wordDataRow = wordDataTable.Rows(0)
            Me.ModifiedMessageField.Value = If(String.IsNullOrWhiteSpace(wordDataRow.WORD_VAL), wordDataRow.WORD_VAL_ENG, wordDataRow.WORD_VAL).Replace("\n", " ").Replace("\r", " ")
        End If

        wordDataTable = businessLogic.GetWordData(WORD_CD_TITLE)
        If (IsNothing(wordDataTable) = False) And (wordDataTable.Rows.Count > 0) Then
            wordDataRow = wordDataTable.Rows(0)
            Me.HiddenTitle.Value = If(String.IsNullOrWhiteSpace(wordDataRow.WORD_VAL), wordDataRow.WORD_VAL_ENG, wordDataRow.WORD_VAL)
        End If
    End Sub

#End Region

End Class
