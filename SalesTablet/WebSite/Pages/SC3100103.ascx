<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3100103.ascx.vb" Inherits="Pages_SC3100103_Control" EnableViewState="false" %>

<%'スタイルシート %>
<link rel="Stylesheet" href="../Styles/SC3100103/SC3100103.css?20121003000000" type="text/css" media="screen" />

<%'スクリプト(画面固有) %>
<script type="text/javascript" src="../Scripts/SC3100103/SC3100103_ascx.js?20120817000000"></script>

<icrop:PopOverForm ID="StandByStaffPopOverForm" PreventBottom="true" PreventLeft="true" PreventRight="true" PreventTop="false" runat="server" PageCapacity="0" Width="600px" Height="570px" OnClientRender="StandByStaffPopOverForm_render" OnClientClose="StandByStaffPopOverForm_close">
</icrop:PopOverForm>

<%'表示するパネル %>
<asp:Panel ID="Panel_SC3100103" runat="server" style="display:none;">
  <iframe ID="Frame_SC3100103" runat="server" src="SC3100103.aspx"></iframe>
</asp:Panel>

<%'Hidden値設定 %>
<asp:HiddenField ID="StandByStaffClickStatus" runat="server" />
<asp:HiddenField ID="StandByStaffWordTitle" runat="server" />
<asp:HiddenField ID="StandByStaffWordCancel" runat="server" />
<asp:HiddenField ID="StandByStaffWordRegister" runat="server" />

