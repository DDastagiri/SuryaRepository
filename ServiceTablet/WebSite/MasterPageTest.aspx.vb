Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core

Public Class MasterPageTest
    Inherits BasePage
    Implements ICustomerForm


    Private Const SUBMENU_SCHEDULE As Integer = 101
    Private Const SUBMENU_MAIL As Integer = 102

    Private commonMaster As CommonMasterPage

    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
        Me.commonMaster = commonMaster
        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        Return {SUBMENU_SCHEDULE, SUBMENU_MAIL}
    End Function

    Public Overrides Function DeclareCommonMasterContextMenu(commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage) As Integer()
        Dim itemIds As New List(Of Integer)
        Dim criteria As String
        criteria = Page.Request("ctl00$content$item1_useSwitchButton")
        If (criteria IsNot Nothing AndAlso criteria.Equals("on")) Then
            itemIds.Add(1)
        End If
        criteria = Page.Request("ctl00$content$item2_useSwitchButton")
        If (criteria IsNot Nothing AndAlso criteria.Equals("on")) Then
            itemIds.Add(2)
        End If

        If (itemIds.Count = 0) Then
            Return MyBase.DeclareCommonMasterContextMenu(commonMaster)
        Else
            criteria = Page.Request("ctl00$content$itemLogout_useSwitchButton")
            If (Me.IsPostBack = False OrElse (criteria IsNot Nothing AndAlso criteria.Equals("on"))) Then
                itemIds.Add(CInt(CommonMasterContextMenuBuiltinMenuID.LogoutItem))
            End If
            Return itemIds.ToArray()
        End If
    End Function

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim staff As StaffContext = StaffContext.Current()

        'presenceCategoryNumericBox  presenceDetailNumericBox
        presenceCategoryNumericBox.Value = CDec(staff.PresenceCategory)
        presenceDetailNumericBox.Value = CDec(staff.PresenceDetail)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim staff As StaffContext = StaffContext.Current()

        If (Not Me.IsPostBack) Then
            'operationItemSelector
            For Each opValue As Integer In System.Enum.GetValues(GetType(Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation))
                Dim item As New ListItem(String.Format("{0}[{1}]", System.Enum.GetName(GetType(Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation), opValue), opValue), opValue.ToString())
                item.Selected = (CInt(staff.OpeCD) = opValue)
                operationItemSelector.Items.Add(item)
            Next
        End If

        With Me.commonMaster.SearchBox
            .Enabled = searchBarEnabledSwitchButton.Checked
            .Visible = searchBarVisibleSwitchButton.Checked
            .SearchText = searchBarTextCustomTextBox.Text
        End With

        With Me.commonMaster.ContextMenu
            .Enabled = cmenu_enabledSwitchButton.Checked
            .AutoPostBack = cmenu_autoPostBackSwitchButton.Checked
            .UseAutoOpening = cmenu_useAutoOpeningSwitchButton.Checked

            AddHandler .Open, AddressOf contextMenu_Open
            AddHandler .Close, AddressOf contextMenu_Close

            Dim menuItem1 As CommonMasterContextMenuItem = .GetMenuItem(1)
            If (menuItem1 IsNot Nothing) Then
                With menuItem1
                    .Text = "Menu Item1"
                    .PresenceCategory = "2"
                    .PresenceDetail = "0"
                    .Enabled = item1_enabledSwitchButton.Checked
                    .Visible = item1_visibleSwitchButton.Checked
                    AddHandler .Click, AddressOf menuItem1_Click
                End With
            End If
            Dim menuItem2 As CommonMasterContextMenuItem = .GetMenuItem(2)
            If (menuItem2 IsNot Nothing) Then
                With menuItem2
                    .Text = "Menu Item2"
                    .PresenceCategory = "3"
                    .PresenceDetail = "1"
                    .Enabled = item2_enabledSwitchButton.Checked
                    .Visible = item2_visibleSwitchButton.Checked
                    AddHandler .Click, AddressOf menuItem2_Click
                End With
            End If
            Dim menuItemLogout As CommonMasterContextMenuItem = .GetMenuItem(CommonMasterContextMenuBuiltinMenuID.LogoutItem)
            If (menuItemLogout IsNot Nothing) Then
                With menuItemLogout
                    .Enabled = itemLogout_enabledSwitchButton.Checked
                    .Visible = itemLogout_visibleSwitchButton.Checked
                    AddHandler .Click, AddressOf menuItemLogout_Click
                End With
            End If
        End With

        Dim mainMenu As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.MainMenu)
        With mainMenu
            .Enabled = mainMenu_enabledSwitchButton.Checked
            .Visible = mainMenu_visibleSwitchButton.Checked
            .Selected = mainMenu_selectedSwitchButton.Checked
        End With

        'スケジュールボタンを設定（イベントハンドラ割り当て　（※イベントハンドラは、ポストバック時でも常に割り当てる必要があります））
        Dim scheduleButton As CommonMasterFooterButton = commonMaster.GetFooterButton(101)
        If (scheduleButton IsNot Nothing) Then
            AddHandler scheduleButton.Click, AddressOf scheduleButton_Click
            With scheduleButton
                .Enabled = schedule_enabledSwitchButton.Checked
                .Visible = schedule_visibleSwitchButton.Checked
                .Selected = schedule_selectedSwitchButton.Checked
            End With
        End If

        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
        ''TCVボタンを設定（イベントハンドラ割り当て）
        'Dim tcvButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.TCV)
        'If (tcvButton IsNot Nothing) Then
        '    AddHandler tcvButton.Click, AddressOf tcvButton_Click
        'End If
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

        AddHandler commonMaster.Rewinding, AddressOf commonMaster_Rewinding
        AddHandler commonMaster.Forwarding, AddressOf commonMaster_Forwarding
        AddHandler commonMaster.Logout, AddressOf commonMaster_Logout


        HeaderButtonControl()
    End Sub

    Protected Sub operationItemSelector_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles operationItemSelector.SelectedIndexChanged
        Dim staff As StaffContext = StaffContext.Current()
        Dim staffType As Type = GetType(StaffContext)
        Dim opeCDInfo As FieldInfo = staffType.GetField("_opeCd", (BindingFlags.SetField Or BindingFlags.NonPublic Or BindingFlags.Instance))
        opeCDInfo.SetValue(staff, System.Enum.Parse(GetType(Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation), operationItemSelector.SelectedValue))
    End Sub


    Protected Sub updatePresenceCustomButton_Click(sender As Object, e As System.EventArgs) Handles updatePresenceCustomButton.Click
        Dim staff As StaffContext = StaffContext.Current()
        staff.UpdatePresence(CInt(presenceCategoryNumericBox.Value.Value).ToString(), CInt(presenceDetailNumericBox.Value.Value).ToString())
    End Sub

    Private Sub menuItem1_Click(sender As Object, e As EventArgs)
        Logger.Debug("menuItem1_Click")
    End Sub

    Private Sub menuItem2_Click(sender As Object, e As EventArgs)
        Logger.Debug("menuItem2_Click")
    End Sub

    Private Sub menuItemLogout_Click(sender As Object, e As EventArgs)
        Logger.Debug("menuItemLogout_Click")
        ShowMessageBox(0)
    End Sub

    Private Sub contextMenu_Open(sender As Object, e As EventArgs)
        Logger.Debug("contextMenu_Open")
        ShowMessageBox("contextMenu_Open", "contextMenu_Open is called", 0)
    End Sub

    Private Sub contextMenu_Close(sender As Object, e As EventArgs)
        Logger.Debug("contextMenu_Close")
        ShowMessageBox("contextMenu_Close", "contextMenu_Close is called", 0)
    End Sub

    Private Sub scheduleButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        Logger.Debug("scheduleButton_Click")
    End Sub

    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)
        Logger.Debug("tcvButton_Click")
        'TCV機能に渡す引数を設定
        e.Parameters.Add("CustomerId", 9999999)
        e.Parameters.Add("Branch", "01 ")
    End Sub

    Public ReadOnly Property DefaultOperationLocked As Boolean Implements ICustomerForm.DefaultOperationLocked
        Get
            Return False
        End Get
    End Property


    Protected Sub HeaderButtonControl()
        '戻るボタンがタップされた場合に実行する、クライアント側スクリプトを設定
        'commonMaster.GetHeaderButton(HeaderButton.Rewind).OnClientClick = "return checkFunc();"
        '進むボタンがタップされた場合に実行する、クライアント側スクリプトを設定
        'commonMaster.GetHeaderButton(HeaderButton.Forward).OnClientClick = "return checkFunc();"
        'ログアウトボタンがタップされた場合に実行する、クライアント側スクリプトを設定
        'commonMaster.GetHeaderButton(HeaderButton.Logout).OnClientClick = "return checkFunc();"
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        If (Date.Now.Millisecond) Mod 2 = 0 Then
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = False
        Else
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = True
        End If
    End Sub

    Private Sub commonMaster_Rewinding(sender As Object, e As CancelEventArgs)
        Return
    End Sub

    Private Sub commonMaster_Forwarding(sender As Object, e As CancelEventArgs)
        Return
    End Sub

    Private Sub commonMaster_Logout(sender As Object, e As CancelEventArgs)
        Return
    End Sub

    Protected Sub reloadContextMenuCustomButton_Click(sender As Object, e As System.EventArgs) Handles reloadContextMenuCustomButton.Click

    End Sub


End Class
