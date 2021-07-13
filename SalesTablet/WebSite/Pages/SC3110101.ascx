<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3110101.ascx.vb" Inherits="Pages_SC3110101_Control" EnableViewState="false" %>

<%'スタイルシート %>
<link rel="stylesheet" href="../Styles/SC3110101/SC3110101.css" type="text/css" />

<%'スクリプト(画面固有) %>
<script type="text/javascript" src="../Scripts/SC3110101/SC3110101_ascx.js"></script>

<icrop:PopOverForm ID="TestDrivePopOverForm" PreventBottom="true" PreventLeft="true" PreventRight="true" PreventTop="false" runat="server" PageCapacity="0" Width="358px" Height="415px" OnClientRender="TestDrivePopOverForm_render" OnClientClose="TestDrivePopOverForm_close">
</icrop:PopOverForm>

<%'表示するパネル %>
<asp:Panel ID="Panel_SC3110101" runat="server" style="display:none;">
    <iframe id="Frame_SC3110101" runat="server" src="SC3110101.aspx"></iframe>
</asp:Panel>

<%'Hidden値設定 %>
<asp:HiddenField ID="clickStatus" runat="server" />
<asp:HiddenField ID="opeCd" runat="server" />
<asp:HiddenField ID="wordTitle" runat="server" />
<asp:HiddenField ID="wordCancel" runat="server" />
<asp:HiddenField ID="wordSubmit" runat="server" />
