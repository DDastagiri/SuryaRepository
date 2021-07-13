<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070210.ascx.vb" Inherits="Pages_SC3070210" %>
<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3070210.ascx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 相談履歴                                                           %>
<%'補足：                                                                    %>
<%'作成： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発          %>
<%'─────────────────────────────────────%>

<%'スタイル %>  		  
<link rel="Stylesheet" href="../Styles/SC3070210/SC3070210.css?20150326000001" />
<%'スクリプト %>  	
<script src="../Scripts/SC3070210/SC3070210.js?20150326000001" type="text/javascript"></script>

<%'処理中のオーバーレイ %>
<asp:UpdatePanel id="SC3070210_UpdateArea" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="SC3070210_Main" style="margin-top:10px">
            <asp:Panel ID="SC3070210_CommentHistoryPanel" runat="server" class="BoxSetWide">
                <h4>
                    <icrop:CustomLabel ID="SC3070210_HeaderTitleLabel" CssClass="useCut" Width="156px" runat="server">
                        <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", 71001))%>
                    </icrop:CustomLabel>
                </h4>
                <div id="SC3070210_CommentArea" style="position:relative">
                    <div class="SC3070210_CommentList">
					    <asp:Repeater ID="CommentRepeater" runat="server" >
						    <ItemTemplate>
                                <div class="SC3070210_Comment">
                                    <div class='SC3070210_Comment_Left <%# Eval("L_CSSCLASS")%>'>
                                        <div class="SC3070210_Comment_StaffName"><%# Server.HtmlEncode(Eval("L_USERNAME"))%></div>
                                        <div class="SC3070210_Comment_Cloud">
                                            <div class="SC3070210_Comment_Header"><%# Server.HtmlEncode(Eval("L_COMMENTTITLE"))%></div>
                                            <div class="SC3070210_Comment_Body"><%# Eval("L_COMMENT") %></div>
                                            <div class="SC3070210_Comment_Footer"><%# Server.HtmlEncode(Eval("L_COMMENTDATE"))%></div>
                                        </div>
                                    </div>
                                    <div class='SC3070210_Comment_Right <%# Eval("R_CSSCLASS")%>'>
                                        <div class="SC3070210_Comment_StaffName SC3070210_Comment_StaffNameR"><%# Server.HtmlEncode(Eval("R_USERNAME"))%></div>
                                        <div class="SC3070210_Comment_Cloud SC3070210_Comment_CloudR">
                                            <div class="SC3070210_Comment_Header"><%# Server.HtmlEncode(Eval("R_COMMENTTITLE"))%></div>
                                            <div class="SC3070210_Comment_Body"><%# Eval("R_COMMENT")%></div>
                                            <div class="SC3070210_Comment_Footer"><%# Server.HtmlEncode(Eval("R_COMMENTDATE"))%></div>
                                        </div>
                                    </div>
                                    <div style="clear:both"></div>
                                </div>
						    </ItemTemplate>
					    </asp:Repeater>
			        </div>
                    <div class="SC3070210_Pager">
                        <asp:HyperLink ID="SC3070210_ShowRecentLink" runat="server"  Visible="False" Width="200" OnClick="if (SC3070210.reload(false)) return false;" />
                        <asp:HyperLink ID="SC3070210_ShowAllLink" runat="server" Visible="False" Width="200" OnClick="if (SC3070210.reload(true)) return false;" />
                    </div>
			        <div id="SC3070210_ProcessingBlock" style="display:none">
                        <div id="SC3070210_ProcessingIcon"></div>
                    </div>
			    </div>
            </asp:Panel>
            <div style="display:none">
                <asp:HiddenField ID="SC3070210_IsShowingAll" runat="server" Value="False" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>