<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070207.ascx.vb" Inherits="Pages_SC3070207" %>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3070207.ascx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 注文承認                                                           %>
<%'補足：                                                                    %>
<%'作成： 2013/12/09 TCS 山口  Aカード情報相互連携開発                       %>
<%'─────────────────────────────────────%>

<%'スタイル %>  		  
<link rel="Stylesheet" href="../Styles/SC3070207/SC3070207.css?20150327000000" />
<%'スクリプト %>  	
<script src="../Scripts/SC3070207/SC3070207.js?20140801000000" type="text/javascript"></script>

<%'処理中のオーバーレイ %>
<div id="SC3070207_RegistOverlayBlack"></div>
<div id="SC3070207_ProcessingServer"></div>
<asp:UpdatePanel id="SC3070207_UpdateArea" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <%--更新用ボタン--%>
        <asp:Button ID="SC3070207_ApprovalButton" runat="server" style="display:none;"></asp:Button>
        <asp:Button ID="SC3070207_DenialButton" runat="server" style="display:none;"></asp:Button>
        <%'注文承認 メインエリア %>  	
        <div id="SC3070207_Main" style="margin-top:10px">
            <div class="SC3070207_BoxSetWide">
                <h4>
                    <icrop:CustomLabel ID="SC3070207_HeaderTitleLabel" CssClass="useCut" Width="156px" runat="server">
                        <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", 70001))%>
                    </icrop:CustomLabel>
                </h4>
                <div class="SC3070207_MainArea">
                	<div class="SC3070207_InputArea">
	                	<div class="SC3070207_InputLabel">
	            			<icrop:CustomLabel ID="SC3070207_CommentLabel" runat="server" class="useCut" Width="160px" >
                                <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", 70004))%>
                            </icrop:CustomLabel>
	        			</div>
	        			<div class="SC3070207_InputTextBox">
	                		<icrop:CustomTextBox ID="SC3070207_CommentTextBox" runat="server" MaxLength="128" TabIndex="1"/>
	                	</div>
                	</div>
                    <div class="SC3070207_ButtonArea">
                        <div class="SC3070207_Button Blue" id="SC3070207_ApprovalButtonArea">
                            <div class="SC3070207_ButtonTextArea">
					            <icrop:CustomLabel ID="SC3070207_ApprovalButtonLabel" runat="server" class="useCut">
                                    <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", 70002))%>
                                </icrop:CustomLabel>
                                <input type="hidden" id="SC3070207_ApprovalButtonInputFlg" />
                            </div>
				        </div>
                        <div class="SC3070207_Button Red" id="SC3070207_DenialButtonArea">
                            <div class="SC3070207_ButtonTextArea">
					            <icrop:CustomLabel ID="SC3070207_DenialButtonLabel" runat="server" class="useCut">
                                    <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", 70003))%>
                                </icrop:CustomLabel>
                                <input type="hidden" id="SC3070207_DenialButtonInputFlg" />
                            </div>
				        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="ErrorFlg" />
    </ContentTemplate>
</asp:UpdatePanel>