'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3120201.aspx.vb
'──────────────────────────────────────────
'機能： SPMフレーム
'補足： 
'作成： 2014/01/24 TMEJ m.asano
'更新： 2014/07/02 TMEJ m.asano タブレットSPMによるSC管理機能開発に向けたシステム設計 $01
'──────────────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

''' <summary>
''' SPMフレーム
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3120201
    Inherits BasePage

#Region "非公開定数"

    ''' <summary>
    ''' 操作権限コード（Assistant(Branch)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeAb As Integer = 4

    ''' <summary>
    ''' 操作権限コード（SGM）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSgm As Integer = 5

    ''' <summary>
    ''' 操作権限コード（BM）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeBm As Integer = 6

    ''' <summary>
    ''' 操作権限コード（SSM）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSsm As Integer = 7

    ''' <summary>
    ''' 操作権限コード（SC）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSc As Integer = 8

    ''' <summary>
    ''' 環境設定パラメータ（タブレットSPMURL）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TabletSpmUrl As String = "TABLET_SPM_URL"

    ''' <summary>
    ''' Getパラメータ区切り文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GetParameterSeparator As String = "/"

    ''' <summary>
    ''' Getパラメータ日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GetParameterDateFormat As String = "yyyyMMddHHmmss"

    ' $01 START

    ''' <summary>
    ''' セッションキー：異常分類コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyIrregClassCode As String = "IRREGULAR_CLASS_CD"

    ''' <summary>
    ''' セッションキー：異常項目コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyIrregItemCode As String = "IRREGULAR_ITEM_CD"

    ''' <summary>
    ''' フッターボタンID（お客様チップ作成）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuCreateSpmIrregularList As Integer = 1601


    ''' <summary>
    ''' 異常分類コード:活動遅れ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCodeDelayActivity As String = "30"

    ''' <summary>
    ''' 異常項目コード:受注前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCodeBeforeOrder As String = "01"




    ' $01 END

#End Region

#Region "非公開変数"

    ' $01 START
    ''' <summary>
    ''' セッション値：異常分類コード
    ''' </summary>
    Private SessionValueIrregClassCode As String

    ''' <summary>
    ''' セッション値：異常項目コード
    ''' </summary>
    Private SessionValueIrregItemCode As String

    ''' <summary>
    ''' ページ用マスタページ
    ''' </summary>
    ''' <remarks></remarks>
    Private commonMasterPage As CommonMasterPage
    ' $01 END

#End Region

#Region "イベント処理"

#Region "ページロード"

    ''' <summary>
    ''' ページロード時の処理。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load Start")

        ' フッターの制御
        InitFooterEvent()

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then
            Logger.Info("Page_Load_End PostBack")
            Return
        End If

        ' ヘッダーの制御
        InitHeaderEvent()

        ' SPM接続先URLを取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Logger.Info("Page_Load Call_Start GetSystemEnvSetting Param[" & TabletSpmUrl & "]")
        sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(TabletSpmUrl)
        Logger.Info("Page_Load Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetTitlePosRow) & "]")

        ' SPM接続先URLを設定
        Me.SpmUrl.Value = sysEnvSetTitlePosRow.PARAMVALUE
        Logger.Info("Page_Load Param[" & TabletSpmUrl & "] GetValue=[" & sysEnvSetTitlePosRow.PARAMVALUE & "]")

        ' Getパラメータを指定
        Dim staff As StaffContext = StaffContext.Current
        ' TBL_USERSよりデータ取得
        Dim users As Users = New Users
        Dim userRow As UsersDataSet.USERSRow
        userRow = users.GetUser(staff.Account)

        Me.UrlParam.Value = staff.Account

        ' $01 START
        ' セッション値を取得
        Me.GetSessionValue()

        ' 異常分類コード・異常項目コードのどちらか一方でも未設定の場合は、Getパラメータに空文字を指定。
        If String.IsNullOrEmpty(SessionValueIrregClassCode) OrElse String.IsNullOrEmpty(SessionValueIrregItemCode) Then

            Me.IrregClassCode.Value = String.Empty
            Me.IrregItemCode.Value = String.Empty
        Else

            Me.IrregClassCode.Value = SessionValueIrregClassCode
            Me.IrregItemCode.Value = SessionValueIrregItemCode
        End If

        ' $01 END

        Logger.Info("Page_Load End")
    End Sub

#End Region

#End Region

#Region "非公開メソッド"

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

        Logger.Info("InitHeaderEvent End")

    End Sub

    ''' <summary>
    ''' フッターの制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks>子メニューID配列</remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) _
                                                        As Integer()

        Me.commonMasterPage = commonMaster

        ' 自ページの所属メニューを宣言
        category = FooterMenuCategory.SPM

        Return {FooterIdSubmenuCreateSpmIrregularList}

    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Logger.Info("InitFooterEvent Start")

        Dim staff As StaffContext = StaffContext.Current

        ' 権限によりフッタボタンの制御を行う。
        ' メインメニュー
        Dim mainButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        If mainButton IsNot Nothing Then
            If OperationCodeSc.Equals(staff.OpeCD) Or OperationCodeSsm.Equals(staff.OpeCD) Or OperationCodeBm.Equals(staff.OpeCD) Then
                ' SC or SSM or BM の場合表示
                AddHandler mainButton.Click, _
                Sub()
                    ' SCメインに遷移
                    Me.RedirectNextScreen("SC3010203")
                End Sub
            Else
                ' 上記以外は非表示
                mainButton.Visible = False
            End If
        End If

        ' 顧客
        Dim customerButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
        If customerButton IsNot Nothing Then
            If OperationCodeSc.Equals(staff.OpeCD) Then
                ' SCの場合表示
                AddHandler customerButton.Click, _
                Sub()
                    ' 顧客詳細に遷移
                    Me.RedirectNextScreen("SC3080201")
                End Sub
            Else
                ' 上記以外は非表示
                customerButton.Visible = False
            End If
        End If

        ' ショールームステータス
        Dim ssvButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        If ssvButton IsNot Nothing Then
            If OperationCodeSsm.Equals(staff.OpeCD) Or OperationCodeBm.Equals(staff.OpeCD) Then
                ' SSM or BM の場合表示
                AddHandler ssvButton.Click, _
                Sub()
                    '受付メインに遷移
                    Me.RedirectNextScreen("SC3100101")
                End Sub
            Else
                ' 上記以外は非表示
                ssvButton.Visible = False
            End If
        End If

        ' TCV
        Dim tcvButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        If tcvButton IsNot Nothing Then
            If OperationCodeSc.Equals(staff.OpeCD) Or OperationCodeSsm.Equals(staff.OpeCD) Or OperationCodeBm.Equals(staff.OpeCD) Then
                ' SC or SSM or BM の場合表示
                AddHandler tcvButton.Click, AddressOf tcvButton_Click
            Else
                ' 上記以外は非表示
                tcvButton.Visible = False
            End If
        End If

        ' TCV設定
        Dim tcvSettingButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting)
        If tcvSettingButton IsNot Nothing Then
            If OperationCodeSsm.Equals(staff.OpeCD) Or OperationCodeBm.Equals(staff.OpeCD) Then
                ' SSM or BM の場合表示
                AddHandler tcvSettingButton.Click, _
                Sub()
                    'TCV設定に遷移
                    Me.RedirectNextScreen("SC3050704")
                End Sub
            Else
                ' 上記以外は非表示
                tcvSettingButton.Visible = False
            End If
        End If

        ' SPM、納車時説明ボタンの動作はデフォルト
        ' $01 START
        Logger.Info("InitFooter_009" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuCreateSpmIrregularList & "]")
        Dim createSpmIrregularListLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterIdSubmenuCreateSpmIrregularList)
        Logger.Info("InitFooter_009" & "Call_End GetFooterButton Ret[" & createSpmIrregularListLink.ToString & "]")
        If createSpmIrregularListLink IsNot Nothing Then
            If OperationCodeSgm.Equals(staff.OpeCD) Or OperationCodeAb.Equals(staff.OpeCD) Or
                OperationCodeBm.Equals(staff.OpeCD) Or OperationCodeSsm.Equals(staff.OpeCD) Then
                ' SGM or Assistant(Branch) or BM or SSM の場合表示
                AddHandler createSpmIrregularListLink.Click, _
                  Sub()
                      ' セッションに保持
                      MyBase.SetValue(ScreenPos.Next, SessionKeyIrregClassCode, IrregClassCodeDelayActivity)
                      MyBase.SetValue(ScreenPos.Next, SessionKeyIrregItemCode, IrregItemCodeBeforeOrder)

                      Me.RedirectNextScreen("SC3120201")
                  End Sub
            Else
                ' 上記以外の場合非表示
                createSpmIrregularListLink.Visible = False
            End If
        End If
        ' $01 END

        Logger.Info("InitFooterEvent End")

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

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", False)
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)
        e.Parameters.Add("StrCd", String.Empty)
        e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        e.Parameters.Add("CstKind", String.Empty)
        e.Parameters.Add("CustomerClass", String.Empty)
        e.Parameters.Add("CRCustId", String.Empty)
        e.Parameters.Add("OperationCode", context.OpeCD)
        e.Parameters.Add("BusinessFlg", False)
        e.Parameters.Add("ReadOnlyFlg", False)

        Logger.Info("tcvButton_Click End")
    End Sub

    ' $01 START
    ''' <summary>
    ''' セッション情報の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSessionValue()

        Logger.Info("GetSessionValue Start")

        ' 異常分類コード
        If ContainsKey(ScreenPos.Current, SessionKeyIrregClassCode) Then
            SessionValueIrregClassCode = GetValue(ScreenPos.Current, SessionKeyIrregClassCode, False)
        Else
            SessionValueIrregClassCode = String.Empty
        End If

        ' 異常項目コード
        If ContainsKey(ScreenPos.Current, SessionKeyIrregItemCode) Then
            SessionValueIrregItemCode = GetValue(ScreenPos.Current, SessionKeyIrregItemCode, False)
        Else
            SessionValueIrregItemCode = String.Empty
        End If

        Logger.Info("GetSessionValue SessionValue Name[IRREGULAR_CLASS_CD] Value[" + SessionValueIrregClassCode + "]")
        Logger.Info("GetSessionValue SessionValue Name[IRREGULAR_ITEM_CD] Value[" + SessionValueIrregItemCode + "]")

        Logger.Info("GetSessionValue End")
    End Sub

    ' $01 END

#End Region

End Class
