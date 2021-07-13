<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="MasterPageTest.aspx.vb" Inherits="MasterPageTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("body")
            .unbind("touchmove.icropScript")
            .css("overflow", "");
    });


</script>
<style type="text/css">
    th 
    {
        background-color: #99BBFF;
        padding:3px;
    }
    td 
    {
        background-color: #DDDDDD;
        padding:3px;
        vertical-align:middle;
    }
    td table tr td
    {
        background-color: #FFFFFF;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <table style="width:100%">
    <tr>
        <th>権限</th>
        <td style="margin-left: 40px">
            <icrop:ItemSelector ID="operationItemSelector" runat="server" AutoPostBack="true"></icrop:ItemSelector>
        </td>
    </tr>
    <tr>
        <th>openNoticeList</th>
        <td style="margin-left: 40px">
            <button type="button" onclick="icropScript.ui.openNoticeList();">Call</button>
        </td>
    </tr>
    <tr>
        <th>ステイタス</th>
        <td>
            現在のステイタス:
            <icrop:NumericBox ID="presenceCategoryNumericBox" runat="server" 
                AcceptDecimalPoint="False" AutoPostBack="False" CancelLabel="Cancel" 
                CancelLabelWordNo="0" CompletionLabel="OK" CompletionLabelWordNo="0" 
                MaxDigits="12"></icrop:NumericBox>
            <icrop:NumericBox ID="presenceDetailNumericBox" runat="server" 
                AcceptDecimalPoint="False" AutoPostBack="False" CancelLabel="Cancel" 
                CancelLabelWordNo="0" CompletionLabel="OK" CompletionLabelWordNo="0" 
                MaxDigits="12"></icrop:NumericBox>
            <icrop:CustomButton ID="updatePresenceCustomButton" runat="server" 
                Text="ステイタス変更" />
        
        </td>
    </tr>
    <tr>
        <th>戻る・進むボタン</th>
        <td>
            <asp:Button ID="Button2" runat="server" Text="戻るボタン制御"/>
        </td>
    </tr>
    <tr>
        <th>検索バー</th>
        <td>
        
            Enabled:<icrop:SwitchButton ID="searchBarEnabledSwitchButton" runat="server" 
                AutoPostBack="True" Checked="True" />
&nbsp; Visible:<icrop:SwitchButton ID="searchBarVisibleSwitchButton" runat="server" 
                AutoPostBack="True" Checked="True" />
&nbsp;Text:<icrop:CustomTextBox ID="searchBarTextCustomTextBox" runat="server" 
                Width="103px"></icrop:CustomTextBox>
        
        </td>
    </tr>
    <tr>
        <th>コンテキストメニュー</th>
        <td>
            <div>
                Enabled:<icrop:SwitchButton ID="cmenu_enabledSwitchButton" runat="server" Checked="true" /> AutoPostBack:<icrop:SwitchButton ID="cmenu_autoPostBackSwitchButton" runat="server" Checked="false" /> UseAutoOpening:<icrop:SwitchButton ID="cmenu_useAutoOpeningSwitchButton" runat="server" Checked="false" /></div>
            <table>
                <tr>
                    <th>MenuItem</th>
                    <th>Use</th>
                    <th>Visible</th>
                    <th>Enabled</th>
                    <th>Presence</th>
                </tr>
                <tr>
                    <td>Menu Item1</td>
                    <td><icrop:SwitchButton ID="item1_useSwitchButton" runat="server" Checked="false" /></td>
                    <td><icrop:SwitchButton ID="item1_visibleSwitchButton" runat="server" Checked="true" /></td>
                    <td><icrop:SwitchButton ID="item1_enabledSwitchButton" runat="server" Checked="true" /></td>
                    <td>2,0</td>
                </tr>
                <tr>
                    <td>Menu Item2</td>
                    <td><icrop:SwitchButton ID="item2_useSwitchButton" runat="server" Checked="false" /></td>
                    <td><icrop:SwitchButton ID="item2_visibleSwitchButton" runat="server" Checked="true" /></td>
                    <td><icrop:SwitchButton ID="item2_enabledSwitchButton" runat="server" Checked="true" /></td>
                    <td>3,1</td>
                </tr>
                <tr>
                    <td>Logout</td>
                    <td><icrop:SwitchButton ID="itemLogout_useSwitchButton" runat="server" Checked="true" /></td>
                    <td><icrop:SwitchButton ID="itemLogout_visibleSwitchButton" runat="server" Checked="true" /></td>
                    <td><icrop:SwitchButton ID="itemLogout_enabledSwitchButton" runat="server" Checked="true" /></td>
                    <td>1,0</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <th>フッタボタン</th>
        <td>
            <table>
                <tr>
                    <th>Button</th>
                    <th>Visible</th>
                    <th>Enabled</th>
                    <th>Selected</th>
                </tr>
                <tr>
                    <td>Main Menu</td>
                    <th><icrop:SwitchButton ID="mainMenu_visibleSwitchButton" runat="server" Checked="true" /></th>
                    <th><icrop:SwitchButton ID="mainMenu_enabledSwitchButton" runat="server" Checked="true" /></th>
                    <th><icrop:SwitchButton ID="mainMenu_selectedSwitchButton" runat="server" Checked="true" /></th>
                </tr>
                <tr>
                    <td>Schedule</td>
                    <th><icrop:SwitchButton ID="schedule_visibleSwitchButton" runat="server" Checked="true" /></th>
                    <th><icrop:SwitchButton ID="schedule_enabledSwitchButton" runat="server" Checked="true" /></th>
                    <th><icrop:SwitchButton ID="schedule_selectedSwitchButton" runat="server" Checked="false" /></th>
                </tr>
            </table>
       </td>
    </tr>
    </table>
    <div style="text-align:center;">
        <icrop:CustomButton ID="reloadContextMenuCustomButton" runat="server" style="margin:2em;width:300px;height:66px;" Text="再表示" />
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>