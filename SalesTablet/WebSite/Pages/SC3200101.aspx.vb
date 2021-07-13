'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3200101.aspx.vb
'─────────────────────────────────────
'機能： CTメインメニューコードビハインド
'補足： 
'作成： 2012/01/26 KN 鶴田
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Pages_SC3200101
    Inherits BasePage
    'Inherits System.Web.UI.Page

#Region "定数"
    '====================================================
    ' 定数設定
    '====================================================
    ' ---------------------------------------------------
    ' フッターボタン用
    ' ---------------------------------------------------
    ' 顧客情報(200)
    'Private Const SUBMENU_CUSTOMERS_INFORMATION As Integer = 200
    Private Const SUBMENU_CUSTOMERS_INFORMATION As Integer = FooterMenuCategory.Customer
    ' SMB(800)
    'Private Const SUBMENU_SMB As Integer = 800
    Private Const SUBMENU_SMB As Integer = FooterMenuCategory.SMB
    ' R/O参照(600)
    Private Const SUBMENU_REPAIR_ORDER_REFERENCE As Integer = FooterMenuCategory.RO
    ' 追加作業(1100)
    'Private Const SUBMENU_ADDITION_WORK As Integer = 1100
    Private Const SUBMENU_ADDITION_WORK As Integer = FooterMenuCategory.AddOperation
    ' 追加作業(サブ)(1101)
    Private Const SUBMENU_ADDITION_WORK_SUB As Integer = 1101
    ' 完成検査(1000)
    'Private Const SUBMENU_COMPLETION_CHECK As Integer = 100
    Private Const SUBMENU_COMPLETION_CHECK As Integer = FooterMenuCategory.Examination
    ' スケジューラ(400)
    'Private Const SUBMENU_SCHEDULER As Integer = 400
    Private Const SUBMENU_SCHEDULER As Integer = FooterMenuCategory.Schedule
    ' 電話帳(500)
    'Private Const SUBMENU_TELEPHONE_DIRECTORY As Integer = 500
    Private Const SUBMENU_TELEPHONE_DIRECTORY As Integer = FooterMenuCategory.TELDirectory

    ' ---------------------------------------------------
    ' 遷移先ページID
    ' ---------------------------------------------------
    ' 追加作業ページID
    Private Const ADDITION_WORK_PAGE_ID As String = "SC3170101"
    ' 完成検査ページID
    Private Const COMPLETION_CHECK_PAGE_ID As String = "SC3180101"
    ' 顧客情報ページID
    Private Const CUSTOMERS_INFORMATION_PAGE_ID As String = "SC3080101"
    ' R/O作成ページID
    Private Const REPAIR_ORDER_REFERENCE_PAGE_ID As String = "SC3160101"
    ' スケジューラページID
    Private Const SCHEDULE_PAGE_ID As String = "SC3000113"

#End Region


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        ' フッターボタンの設定を行う
        setFooterButton()

    End Sub

#Region "フッター制御"
    ' フッター用
    Private commonMaster As CommonMasterPage
    ' 追加作業ボタンの表示/非表示フラグ(0:非表示, 1:表示)
    'Private additionWorkButtonDispFlg As Integer = 0

    ''' <summary>
    ''' フッターの宣言
    ''' </summary>
    ''' <param name="commonMaster"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        Me.commonMaster = commonMaster

        '自ページの所属メニューを宣言
        'category = FooterMenuCategory.MainMenu
        '    category = FooterMenuCategory.RO

        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        ' (顧客情報)、SMB、R/O参照、追加作業(サブ)、完成検査、スケジューラ、電話帳
        'Return {SUBMENU_SMB, _
        '        SUBMENU_REPAIR_ORDER_REFERENCE, _
        '        SUBMENU_ADDITION_WORK_SUB, _
        '        SUBMENU_COMPLETION_CHECK, _
        '        SUBMENU_SCHEDULER, _
        '        SUBMENU_TELEPHONE_DIRECTORY}
        ' 追加作業
        '    Return {SUBMENU_ADDITION_WORK}
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setFooterButton()

        Me.commonMaster = CType(Me.Master, CommonMasterPage)

        ' ------------------------------------------------------
        ' サブメニューボタンを設定（イベントハンドラ割り当て）
        ' ------------------------------------------------------
        ' スケジューラボタン
        Dim scheduleButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_SCHEDULER)
        'AddHandler scheduleButton.Click, AddressOf scheduleButton_Click
        scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"

        ' SMBボタン
        'Dim SMBButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_SMB)
        'AddHandler SMBButton.Click, AddressOf SMBButton_Click
        'SMBButton.Enabled = False
        'SMBButton.Visible = False '非表示

        ' R/O参照ボタン
        Dim repairOrderReferenceButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_REPAIR_ORDER_REFERENCE)
        AddHandler repairOrderReferenceButton.Click, AddressOf repairOrderReferenceButton_Click

        ' 追加作業(サブ)ボタン
        'Dim additionWorkButtonSub As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_ADDITION_WORK_SUB)
        'additionWorkButtonSub = commonMaster.GetFooterButton(SUBMENU_ADDITION_WORK_SUB)
        'AddHandler additionWorkButtonSub.Click, AddressOf additionWorkButtonSub_Click
        'If (IsPostBack = False) Then
        '    ' 初期表示は非表示にする
        '    additionWorkButtonSub.Visible = False
        '    additionWorkButtonSub.Enabled = False
        '    additionWorkButtonDispFlg = 0
        '    ' hiddenフィールド(追加作業ボタンの表示/非表示フラグ(0:非表示, 1:表示))の設定
        '    hdnAdditionWorkButtonSubDispFlag.Value = "0"
        'End If

        ' 追加作業ボタン
        Dim additionWorkButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_ADDITION_WORK)
        AddHandler additionWorkButton.Click, AddressOf additionWorkButton_Click

        ' 完成検査ボタン
        Dim completionCheckButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_COMPLETION_CHECK)
        AddHandler completionCheckButton.Click, AddressOf completionCheckButton_Click

        ' 電話帳ボタン
        Dim telephoneDirectoryButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TELEPHONE_DIRECTORY)
        'AddHandler telephoneDirectoryButton.Click, AddressOf telephoneDirectoryButton_Click
        telephoneDirectoryButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'サブメニューボタンを設定（無効化）
        'Dim mailButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_MAIL)
        'mailButton.Enabled = False

        ' ------------------------------------------------------
        ' メニューボタンを設定（イベントハンドラ割り当て）
        ' ------------------------------------------------------
        ' メインメニューボタン
        Dim mainMenuButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click

        ' 顧客情報ボタン
        Dim customerButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.Customer)
        AddHandler customerButton.Click, AddressOf customerButton_Click

        ' TCVボタン
        '（※TCVと連携する画面以外、基本的にイベントハンドラの割り当ては不要）
        'Dim tcvButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.TCV)
        'AddHandler tcvButton.Click, AddressOf tcvButton_Click
        'tcvButton.Visible = False

    End Sub

    ' SMBボタンクリック
    Private Sub SMBButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 画面へ遷移
        'Me.RedirectNextScreen("")
    End Sub

    ' R/O参照ボタンクリック
    Private Sub repairOrderReferenceButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 画面へ遷移
        Me.RedirectNextScreen(REPAIR_ORDER_REFERENCE_PAGE_ID)

        ' 追加作業(サブ)ボタンの表示/非表示制御
        'Dim additionWorkButtonSub As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_ADDITION_WORK_SUB)
        'If (hdnAdditionWorkButtonSubDispFlag.Value.Equals("0")) Then
        '    additionWorkButtonSub.Visible = True
        '    additionWorkButtonSub.Enabled = True
        '    ' hiddenフィールドの値を変更
        '    hdnAdditionWorkButtonSubDispFlag.Value = "1"
        'Else
        '    additionWorkButtonSub.Visible = False
        '    additionWorkButtonSub.Enabled = False
        '    ' hiddenフィールドの値を変更
        '    hdnAdditionWorkButtonSubDispFlag.Value = "0"
        'End If

    End Sub

    ' 追加作業(サブ)ボタンクリック
    Private Sub additionWorkButtonSub_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 追加作業画面へ遷移
        Me.RedirectNextScreen(ADDITION_WORK_PAGE_ID)
    End Sub

    ' 追加作業ボタンクリック
    Private Sub additionWorkButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 追加作業画面へ遷移
        Me.RedirectNextScreen(ADDITION_WORK_PAGE_ID)
    End Sub

    ' 完成検査ボタンクリック
    Private Sub completionCheckButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 完成検査画面へ遷移
        Me.RedirectNextScreen(COMPLETION_CHECK_PAGE_ID)
    End Sub

    ' スケジューラボタンクリック
    Private Sub scheduleButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        'スケジュール画面へ遷移
        'Me.RedirectNextScreen(SCHEDULE_PAGE_ID)
    End Sub

    ' 電話帳ボタンクリック
    Private Sub telephoneDirectoryButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 画面へ遷移
        'Me.RedirectNextScreen("")
    End Sub

    ' メインメニューボタンクリック
    Private Sub mainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 画面へ遷移
        'Me.RedirectNextScreen("")
    End Sub

    ' 顧客情報ボタンクリック
    Private Sub customerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        ' 画面へ遷移
        Me.RedirectNextScreen(CUSTOMERS_INFORMATION_PAGE_ID)
    End Sub

    'Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
    '    'TCV機能に渡す引数を設定
    '    e.Parameters.Add("CustomerId", "9999999")
    'End Sub
#End Region

End Class
