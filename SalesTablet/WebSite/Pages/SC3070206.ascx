<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070206.ascx.vb" Inherits="Pages_SC3070206" EnableTheming="false" EnableViewState="false" %>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3070206.ascx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 価格相談回答                                                       %>
<%'補足：                                                                    %>
<%'作成： 2013/12/02 TCS 外崎  Aカード情報相互連携開発                       %>
<%'─────────────────────────────────────%>

<%'スタイル %>  		  
<link rel="Stylesheet" href="../Styles/SC3070206/SC3070206.css?20140226000000" />
<script type="text/javascript" src="../Scripts/SC3070206/SC3070206.js?20131206010000"></script>

<div id="tcvNcv206Main" style="margin-top:10px">
<div id="divDiscountApprovalArea" class="tcvNcvBoxSetWide tcvNcvBoxSetWideEnd">
  <% If Not Me.EditMode Then%>
    <input id="CloseButton" type="button" TabIndex="98" />
  <% End If%>
  <h4><icrop:CustomLabel ID="CustomLabel77" runat="server" TextWordNo="77" UseEllipsis="False" Width="160px" CssClass="clip" /></h4>
  <div class="Pint_bWindowb02">
    <table border="0" cellspacing="0" cellpadding="0" class="InputBox">
    <tr>
      <%' □スタッフ名 %>
      <td width="172" valign="top" class="Titletext">
        <icrop:CustomLabel ID="CustomLabel78" runat="server" TextWordNo="78" UseEllipsis="False" />
      </td>
      <td width="190"><div class="TextBox01">
        <p class="TextAreaLeft"><icrop:CustomLabel ID="staffNameLabel" runat="server" />&nbsp;</p></div></td><td width="224">&nbsp;</td><td width="190">&nbsp;</td></tr><tr>
      <%' □スタッフ値引き額 %>
      <td valign="top" class="Titletext">
        <icrop:CustomLabel ID="CustomLabel79" runat="server" TextWordNo="79" UseEllipsis="False" />
      </td>
      <td><div class="TextBox01">
        <p class="TextAreaRight">&nbsp;<icrop:CustomLabel ID="requestDiscountPriceLabel" runat="server" /></p>
      </div></td>
      <td><div class="Titletext02">
        <icrop:CustomLabel ID="CustomLabel80" runat="server" TextWordNo="80" UseEllipsis="False" />
      </div></td>
      <td><div class="TextBox01R">
        <p class="TextAreaRight">&nbsp;<icrop:CustomLabel ID="RequestTotalPriceLabel" runat="server" /></p>
      </div></td>
    </tr>
    <tr>
      <%' □値引き理由 %>
      <td valign="top" class="Titletext">
        <icrop:CustomLabel ID="CustomLabel81" runat="server" TextWordNo="81" UseEllipsis="False" />
      </td>
      <td colspan="3"><div class="TextBox02">
        <p class="TextAreaLeft"><icrop:CustomLabel ID="reasonLabel" runat="server" />&nbsp;</p></div></td></tr><tr>
      <%' □マネージャー値引き額 %>
      <td valign="top" class="Titletext">
        <icrop:CustomLabel ID="CustomLabel82" runat="server" TextWordNo="82" UseEllipsis="False" />
      </td>
      <td><div class="divApproval">
        <% If Me.EditMode Then%>
          <asp:TextBox ID="ApprovalDiscountPriceTextBox" runat="server" class="approvalPrice ListBoxRight" type="text" TabIndex="45" />
        <% Else%>
          <div class="TextBox01">
            <p class="TextAreaRight">&nbsp;<icrop:CustomLabel ID="ApprovalDiscountPriceLabel" runat="server" /></p>
          </div>
        <% End If%>
      </div></td>
      <td><div class="Titletext02">
        <icrop:CustomLabel ID="CustomLabel83" runat="server" TextWordNo="83" UseEllipsis="False" />
      </div></td>
      <td><div class="TextBox01R">
        <p class="TextAreaRight">&nbsp;<icrop:CustomLabel ID="ApprovalTotalPriceLabel" runat="server" /></p>
      </div></td>
    </tr>
    <tr>
      <%' □マネージャーコメント %>
      <td valign="top" class="Titletext">
        <icrop:CustomLabel ID="CustomLabel84" runat="server" TextWordNo="84" UseEllipsis="False" />
      </td>
      <td colspan="3">
        <% If Me.EditMode Then%>
          <asp:TextBox ID="managerMemoTextbox" runat="server" onchange="" class="ListBoxLeft" type="text" TabIndex="46" MaxLength="128" />
        <% Else%>
          <div class="TextBox02">
            <p class="TextAreaLeft"><icrop:CustomLabel ID="managerMemoLabel" runat="server" />&nbsp;</p>
          </div>
        <% End If%>
      </td>
    </tr>
    </table>
    
    <%' □送信ボタン %>
    <% If Me.EditMode Then%>
      <div class="TransmissionDiv">
        <icrop:CustomButton id="sendButton" runat="server" TextWordNo="85" class="Transmission" TabIndex="99"  OnClientClick="return sendButtonClick();"/>
      </div>
    <% End If%>
  </div>
  </div>
    <asp:HiddenField ID="seqNoHiddenField" runat="server" value="" />
    <asp:HiddenField ID="staffCdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="approvalPriceStaffHiddenField" runat="server" value="" />
    <asp:HiddenField ID="approvalPriceHiddenField" runat="server" value="" />
    <asp:HiddenField ID="approvalDiscountMsgHiddenField" runat="server" value="" />
</div>
