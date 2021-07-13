'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3280101.aspx.vb
'─────────────────────────────────────
'機能： 納車時説明フレーム
'補足： 
'作成： 2014/04/17 NCN 跡部
'更新： 
'─────────────────────────────────────
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.iCROP.BizLogic.SC3280101
Imports Toyota.eCRB.iCROP.DataAccess.SC3280101
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Pages_SC3280101
    Inherits BasePage

#Region "定数"

#Region "システム設定パラメータ"

    ''' <summary>
    ''' システム設定パラメータ 納車時説明URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_SYSTEM_SETTING_DELI_DESCRIPTION_URL As String = "DELI_DESCRIPTION_URL"

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
    ''' セッションキー 顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CSTID As String = "CstId"

    ''' <summary>
    ''' セッションキー 顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CSTTYPE As String = "CstType"

    ''' <summary>
    ''' セッションキー 顧客車両区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CSTVCLTYPE As String = "CstVclType"

    ''' <summary>
    ''' セッションキー 商談ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALESID As String = "SalesId"

#End Region

#Region "見積の契約状況"

    ''' <summary>
    ''' 見積の契約状況 1:契約済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATEINFO_CONTRACT As String = "1"

#End Region

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3280101BusinessLogic
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

        ' 納車時説明URLを取得する
        Dim systemSettingDataTable As SC3280101DataSet.TB_M_SYSTEM_SETTINGDataTable
        Dim systemSettingDeliveryDescriptionUrlRow As SC3280101DataSet.TB_M_SYSTEM_SETTINGRow = Nothing
        Logger.Info("Page_Load Call_Start GetSystemSettingData Param[" & M_SYSTEM_SETTING_DELI_DESCRIPTION_URL & "]")
        systemSettingDataTable = businessLogic.GetSystemSettingData(M_SYSTEM_SETTING_DELI_DESCRIPTION_URL)
        If (IsNothing(systemSettingDataTable) = False) And (systemSettingDataTable.Rows.Count > 0) Then
            systemSettingDeliveryDescriptionUrlRow = systemSettingDataTable.Rows(0)
            ' 納車時説明URLを設定する
            Me.DeliveryDescriptionUrl.Value = systemSettingDeliveryDescriptionUrlRow.SETTING_VAL
        End If
        Logger.Info("Page_Load Call_End GetSystemSettingData Ret[" & (Not IsNothing(systemSettingDeliveryDescriptionUrlRow)) & "]")

        Logger.Info("Page_Load Param[" & M_SYSTEM_SETTING_DELI_DESCRIPTION_URL & "] GetValue=[" & Me.DeliveryDescriptionUrl.Value & "]")


        ' Getパラメータを指定する
        Dim staff As StaffContext = StaffContext.Current
        ' TBL_USERSよりデータ取得
        Dim users As Users = New Users
        Dim userRow As UsersDataSet.USERSRow
        userRow = users.GetUser(staff.Account)

        Me.UrlParamAccount.Value = userRow.ACCOUNT
        Me.UrlParamUpdateDate.Value = userRow.UPDATEDATE.ToString(FormatDateTimeLoginTime)
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESID) Then
            Me.UrlParamSalesId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESID, False).ToString()
        Else
            ' 商談ID設定
            Me.SetValue(ScreenPos.Current, "SalesId", "")
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTID) Then
            Me.UrlParamCstId.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTID, False).ToString()
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTTYPE) Then
            Me.UrlParamCstType.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTTYPE, False).ToString()
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTVCLTYPE) Then
            Me.UrlParamCstVclType.Value = Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTVCLTYPE, False).ToString()
        End If

        ' ヘッダーの制御
        InitHeaderEvent()

        'フッターイベント初期化
        InitFooterEvent()

        Logger.Info("Page_Load End")

    End Sub

#End Region

#Region "非公開メソッド"

#Region "ボタンイベント"
    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub MainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("MainMenuButton_Click Start")
        ''メインメニューへ遷移
        Me.RedirectNextScreen("SC3010203")

    End Sub

    ''' <summary>
    ''' 顧客詳細画面へ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub CustomerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '顧客詳細画面に渡す引数を設定
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FOLLOW_UP_BOX", Me.UrlParamSalesId.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CRCUSTID", Me.UrlParamCstId.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CSTKIND", Me.UrlParamCstType.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CUSTOMERCLASS", Me.UrlParamCstVclType.Value)

        Me.RedirectNextScreen("SC3080201")

    End Sub

    ''' <summary>
    ''' ショールームステータスへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub ShowRoomButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ''ショールームステータスへ遷移
        Me.RedirectNextScreen("SC3100101")

    End Sub

    ''' <summary>
    ''' TCV画面へ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub TcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        ' カンマ区切り見積IDと、契約済の見積が存在するかどうかを取得する
        Dim paramEstimateIdStringBuilder As StringBuilder = New StringBuilder()
        Dim paramSelectedEstimateIndex As Integer = 0
        Dim existsContract As Boolean = False
        SetEstimateIdCsv(paramEstimateIdStringBuilder, existsContract)

        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        Dim BusinessFlg As Boolean = False
        If IsSales() Then
            BusinessFlg = True
        End If

        '読み取り専用フラグ設定
        ' 見積が契約済ならReadOnlyFlg=True
        Dim ReadOnlyFlg As Boolean = True
        If existsContract Then
            readOnlyFlg = True
        End If

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3")) Then
            ReadOnlyFlg = False
        End If

        ''TCVへ遷移
        If String.IsNullOrEmpty(paramEstimateIdStringBuilder.ToString()) Then
            '見積もりIDなし
            e.Parameters.Add("DataSource", "None")
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("FollowupBox_SeqNo", Me.UrlParamSalesId.Value)
            e.Parameters.Add("CstKind", Me.UrlParamCstType.Value)
            e.Parameters.Add("CustomerClass", Me.UrlParamCstVclType.Value)
            e.Parameters.Add("CRCustId", Me.UrlParamCstId.Value)
            e.Parameters.Add("OperationCode", StaffContext.Current.OpeCD)
            e.Parameters.Add("BusinessFlg", BusinessFlg)
            e.Parameters.Add("ReadOnlyFlg", ReadOnlyFlg)
        Else
            '見積もりIDあり
            e.Parameters.Add("DataSource", "EstimateId")
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StartPageId", "N-CV-10")
            e.Parameters.Add("EstimateId", paramEstimateIdStringBuilder.ToString())
            e.Parameters.Add("SelectedEstimateIndex", "0")
            e.Parameters.Add("OperationCode", StaffContext.Current.OpeCD)
            e.Parameters.Add("BusinessFlg", BusinessFlg)
            e.Parameters.Add("ReadOnlyFlg", ReadOnlyFlg)

        End If

    End Sub

    ''' <summary>
    ''' カンマ区切り見積ID等を引数に設定する
    ''' </summary>
    ''' <param name="paramEstimateIdStringBuilder">カンマ区切り見積ID</param>
    ''' <param name="existsContract">契約済の見積が存在するかどうか</param>
    ''' <remarks></remarks>
    Private Sub SetEstimateIdCsv(ByRef paramEstimateIdStringBuilder As StringBuilder, ByRef existsContract As Boolean)

        ' 商談IDから見積IDを取得する
        Dim salesId As Decimal = 0
        If Not Decimal.TryParse(Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESID, False).ToString(), salesId) Then
            salesId = 0
        End If

        Dim estimatesDataTable As SC3280101DataSet.TBL_ESTIMATEINFODataTable = businessLogic.GetEstimateInfoData(salesId)

        If (estimatesDataTable IsNot Nothing) AndAlso (estimatesDataTable.Rows.Count > 0) Then
            Dim estimateIdx As Integer = 0
            For Each estimateDataRow As SC3280101DataSet.TBL_ESTIMATEINFORow In estimatesDataTable.Rows
                Dim estimateId As Long = estimateDataRow.ESTIMATEID

                ' カンマ区切りの見積IDを作成する
                If paramEstimateIdStringBuilder.Length > 0 Then
                    paramEstimateIdStringBuilder.Append(",")
                End If

                paramEstimateIdStringBuilder.Append(estimateId.ToString())

                ' 契約済みの見積があるかどうかを保持する
                If ESTIMATEINFO_CONTRACT.Equals(estimateDataRow.CONTRACTFLG) Then
                    existsContract = True
                End If

            Next
        End If

    End Sub


    ''' <summary>
    ''' 納車時説明ツールへ遷移する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub nousyaButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        ' 商談ID
        Me.SetValue(ScreenPos.Next, "SalesId", Me.UrlParamSalesId.Value)
        ' 顧客ID
        Me.SetValue(ScreenPos.Next, "CstId", Me.UrlParamCstId.Value)
        ' 顧客種別
        Me.SetValue(ScreenPos.Next, "CstType", Me.UrlParamCstType.Value)
        ' 顧客車両区分
        Me.SetValue(ScreenPos.Next, "CstVclType", Me.UrlParamCstVclType.Value)

    End Sub


#End Region


    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InitFooterEvent Start")

        'メニューボタン定義
        'メインメニュー
        Dim mainMenuButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click

        '顧客
        Dim customerButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
        AddHandler customerButton.Click, AddressOf CustomerButton_Click

        'TCV
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        AddHandler tcvButton.Click, AddressOf TcvButton_Click

        'ショールームステータス
        Dim showRoomButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        If showRoomButton IsNot Nothing Then
            AddHandler showRoomButton.Click, AddressOf ShowRoomButton_Click
        End If

        'SPM
        Dim spmButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM)

        '納車時説明ツール
        Dim nousya As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain)
        AddHandler nousya.Click, AddressOf nousyaButton_Click

        '商談ステータスに応じてフッターボタンを活性、非活性とする
        If IsSales() Then
            '商談中の場合、非活性とする
            spmButton.Enabled = False
            mainMenuButton.Enabled = False
        Else
            '商談中以外の場合、活性とする
            spmButton.Enabled = True
            mainMenuButton.Enabled = True
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InitFooterEvent End")

    End Sub


    ''' <summary>
    ''' ヘッダーの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        Logger.Info("InitHeaderEvent Start")

        ' 検索エリア非活性化
        CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False
        CType(Me.Master, CommonMasterPage).SearchBox.Visible = False
        If IsSales() Then
            '商談中 の場合はi-cropアイコン使用不可
            CType(Me.Master, CommonMasterPage).ContextMenu.Enabled = False
        Else
            '上記以外は場合はi-cropアイコン使用可能
            CType(Me.Master, CommonMasterPage).ContextMenu.Enabled = True
        End If

        Logger.Info("InitHeaderEvent End")

    End Sub

#Region "フッターボタンイベント"


#End Region

    ''' <summary>
    ''' 商談(一時対応・営業活動・納車作業)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>ステータスを参照して商談中か判断する</remarks>
    Private Function IsSales() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("IsSales Start")

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "2")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "3")) Then

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("IsSales End")

            Return True
        Else

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("IsSales End")

            Return False
        End If
    End Function


#End Region

End Class
