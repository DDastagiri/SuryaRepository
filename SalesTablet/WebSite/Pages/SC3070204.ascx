<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070204.ascx.vb" Inherits="Pages_SC3070204" %>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3070204.ascx                                                       %>
<%'─────────────────────────────────────%>
<%'機能： 見積書・契約書印刷                                             %>
<%'補足：                                                               %>
<%'作成： 2012/11/25 TCS 坪根                                           %>
<%'更新： 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応  %>
<%'更新： 2013/11/27 TCS 河原 Aカード情報相互連携開発 %>
<%'─────────────────────────────────────%>

	<div id="SC3070204PopOver" runat="server" >
		<div id="SC3070204PopOverHeader" class='SC3070204header'>
			<div class='SC3070204header-left'>
				<a href="#" class="SC3070204PopUpCancelButton">
                    <icrop:CustomLabel ID="SC3070204PopUpCancelButtonLabel" runat="server"  class="useCut" />
                </a>
			</div>
			<div class='SC3070204header-title' style="line-height:28px;">
				<h3>
					<icrop:CustomLabel ID="SC3070204HeaderTitle" runat="server"  class="useCut" />
				</h3>
			</div>
			<div class='SC3070204header-right'></div>

			<%'処理中のオーバーレイ %>
			<div id="registOverlayBlackSC3070204"></div>
			<div id="processingServerSC3070204"></div>
		</div> 
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START %>
		<div id="SC3070204PopOverContent" class="content" style="width:256px;height:225px;overflow:hidden;" >
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END %>
			<%'見積書印刷・契約書印刷メインフレーム %>
			<div id="SC3070204MainFrameContent" style="float:left;overflow-y:hidden;overflow-x:hidden;">
                <asp:LinkButton ID="EstimatePrintButton" runat="server" class="SC3070204buttonStyle" Width="254" Height="32" OnClientClick="return false;">
                    <icrop:CustomLabel ID="EstimatePrintButtonLabel" class="SC3070204buttonLabelStyle" runat="server" />
                </asp:LinkButton>
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START %>
                <asp:LinkButton ID="OrderPrintButton" runat="server" class="SC3070204buttonStyle" Width="254" Height="32" OnClientClick="return false;">
                    <icrop:CustomLabel ID="OrderPrintButtonLabel" class="SC3070204buttonLabelStyle" runat="server" />
                </asp:LinkButton>
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END %>
                <asp:LinkButton ID="ContractPrintButton" runat="server" class="SC3070204buttonStyle" Width="254" Height="32" OnClientClick="return false;">
                    <icrop:CustomLabel ID="ContractPrintButtonLabel" runat="server" class="SC3070204buttonLabelStyle" />
                </asp:LinkButton>
            </div>
            
            <!-- メッセージ文言(クライアント用) -->
            <asp:HiddenField ID="HdnMessage901" runat="server" />
            <asp:HiddenField ID="HdnMessage902" runat="server" />
            <asp:HiddenField ID="HdnMessage903" runat="server" />
            <asp:HiddenField ID="HdnMessage904" runat="server" />
            <asp:HiddenField ID="HdnMessage905" runat="server" />
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START %>
            <asp:HiddenField ID="HdnMessage906" runat="server" />
<%' 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END %>
		</div>
   </div>
   <link rel="Stylesheet" href="../Styles/SC3070204/SC3070204.css?20121205180000" />
   <script type="text/javascript" src="../Scripts/TCS/jquery.popover.js"></script>
   <script type="text/javascript" src="../Scripts/SC3070204/SC3070204.js?20140130000001"></script>