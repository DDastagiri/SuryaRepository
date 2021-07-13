<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3100104.ascx.vb" Inherits="Pages_SC3100104_Control"%>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3080104.ascx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 来店チップ作成                                                     %>
<%'補足：                                                                    %>
<%'作成： 2013/08/05 TMEJ m.asano                                            %>
<%'─────────────────────────────────────%>

<%'スタイルシート %>
<link rel="Stylesheet" href="../Styles/SC3100104/SC3100104.css?20130905103000" />

<%'スクリプト(画面固有) %>
<script type="text/javascript" src="../Scripts/SC3100104/SC3100104_ascx.js?20130925100000"></script>

<icrop:PopOverForm ID="CreateCustomerChipPopOverForm" PreventBottom="true" PreventLeft="true" PreventRight="true" PreventTop="false" runat="server" PageCapacity="0" Width="932px" Height="566px" OnClientRender="CreateCustomerChipPopOverForm_render" OnClientClose="CreateCustomerChipPopOverForm_close">
</icrop:PopOverForm>

<%'表示するパネル %>
<asp:Panel ID="Panel_SC3100104" runat="server" style="display:none;">
  <iframe ID="Frame_SC3100104" runat="server" src="SC3100104.aspx"></iframe>
</asp:Panel>

<%'Hidden値設定 %>
<asp:HiddenField ID="CreateCustomerChipClickStatus" runat="server" value="0"/>
<asp:HiddenField ID="CreateCustomerChipCanClick" runat="server" value="1"/>
<asp:HiddenField ID="CreateCustomerChipWordTitle" runat="server" />
<asp:HiddenField ID="CreateCustomerChipWordCancel" runat="server" />
<asp:HiddenField ID="CreateCustomerChipWordRegister" runat="server" />
