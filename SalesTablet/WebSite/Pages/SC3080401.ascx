<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080401.ascx.vb" Inherits="Pages_SC3080401" %>

    <div id="HelpRequestPopOverForm"  runat="server">
        <%' ポップオーバーヘッダ %>       
        <div class='icrop-PopOverForm-header helpRequestPopUpHeader'>
            <%' ヘッダ左部分 %>
<%-- 2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start --%>
            <%--<div class='icrop-PopOverForm-header-left'>--%>
            <div id="helpRequestHeaderLeft" class='icrop-PopOverForm-header-left'>
<%-- 2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End --%>
				<a href="#" id="HeaderCancelButton" class="helpRequestPopUpCancelButton"></a>
			</div>
            <%' ヘッダ中央部分 %>
            <div class='icrop-PopOverForm-header-title' style="line-height:28px;">
				<h3>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START %>
					<icrop:CustomLabel ID="HeaderTitle" runat="server" class="helpRequestUseCut helpRequestHeaderTitle"></icrop:CustomLabel>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END %>
				</h3>
			</div>
            <%' ヘッダ右部分 %>
<%-- 2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start --%>
            <%--<div class='icrop-PopOverForm-header-right'></div>--%>
            <div id="helpRequestHeaderRight" class='icrop-PopOverForm-header-right'></div>
<%-- 2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End --%>
            <%'登録時のオーバーレイ %>
            <div id="registOverlayBlackSC3080401"></div>
            <div id="processingServerSC3080401"></div>

<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START %>
            <%'画面文言エリア 開始 %>
            <span id="WordNo0001PreHiddenField" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3080401", 1)%></span>  
            <span id="WordNo0002PreHiddenField" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3080401", 2)%></span>  
            <asp:HiddenField ID="WordNo0001HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0002HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0003HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0004HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0005HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0006HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0007HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0008HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo0009HiddenField" runat="server" />
            <asp:HiddenField ID="WordNo9001HiddenField" runat="server" />
            <%'画面文言エリア 終了 %>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END %>
        </div> 
        <%' ポップオーバーメインコンテンツ %>
        <div class="icrop-PopOverForm-content" style="width:256px;height:232px;overflow:hidden;" >
		    <div class="icrop-PopOverForm-sheet helpRequestDataBox" style="width:1024px;" >

				<%' ヘルプ依頼メイン %>
                <div class="icrop-PopOverForm-page helpRequestInnerDataBox" id="HelpRequestMain" style="float:left;overflow-y:scroll;overflow-x:hidden;">
					<%' ヘルプ依頼中情報 %>
					<div class="helpRequestPopUpContactAuditBox" id="UnderHelpRequestArea" runat="server">
                      <div class="helpRequestRequestedTime">
						<icrop:CustomLabel ID="RequestDate" runat="server" class="helpRequestUseCut" ></icrop:CustomLabel>
					  </div>
                      <div class="helpRequestUnderRequestText">
						<icrop:CustomLabel ID="UnderRequest" runat="server" class="helpRequestUseCut"></icrop:CustomLabel>
					  </div>
                    </div>

					<%' 依頼先情報 %>
					<div class="helpRequestPopUpContactAuditBox" id="SelectedSendAccountArea" runat="server">
                      <div class="helpRequestPopUpAuditButton" id="SelectedSendAccountNameArea" runat="server">
						<icrop:CustomLabel ID="SelectedSendAccountName_Display" runat="server" class="helpRequestUseEllipsis" style="width:220px;"></icrop:CustomLabel>
					  </div>
					  <asp:HiddenField ID="SelectedSendAccountName" runat="server" />
					  <asp:HiddenField ID="SelectedSendAccount" runat="server" />
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START %>
<%'					  <asp:HiddenField ID="SelectedSendAccountOnlineStatus" runat="server" /> %>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END %>
					  <asp:HiddenField ID="HelpNo" runat="server" />
					  <asp:HiddenField ID="NoticeReqId" runat="server" />
					  <asp:HiddenField ID="IsUnderHelpRequest" runat="server" />
                    </div>
					<%' ヘルプ内容 %>
					<div class="helpRequestPopUpContactAuditBox" id="SelectedHelpMstArea" runat="server">
                      <div class="helpRequestPopUpAuditButton" id="SelectedHelpNameArea" runat="server">
						<icrop:CustomLabel ID="SelectedHelpName_Display" runat="server" class="helpRequestUseEllipsis" style="width:220px;"></icrop:CustomLabel>
						<asp:HiddenField ID="SelectedHelpName" runat="server" />
						<asp:HiddenField ID="SelectedHelpid" runat="server" />
					  </div>
                    </div>
					
					<%' 依頼ボタン %>
					<div class="helpRequestPopUpUnderRequestButton" id="RequestButton" runat="server">
						<icrop:CustomLabel ID="RequestButtonLabel" runat="server"></icrop:CustomLabel>
						<asp:HiddenField ID="IsRequestButtonEnabled" runat="server" />
					</div>
					<%' キャンセルボタン %>
					<div class="helpRequestPopUpUnderCancelButton" id="CancelButton" runat="server">
						<icrop:CustomLabel ID="CancelButtonLabel" runat="server"></icrop:CustomLabel>
						<asp:HiddenField ID="IsCancelButtonEnabled" runat="server" />
					</div>

				    <%'ヘルプ依頼メイン（依頼先不在時） %>
				    <div class="helpRequestPopUpContactAuditBox" id="NoSendAccountArea" runat="server">
					    <div class="helpRequestNoSendAccountImg">&nbsp;</div>
					    <icrop:CustomLabel ID="NoSendAccountLabel" runat="server" class="helpRequestNoSendAccountText"></icrop:CustomLabel>
                    </div>
				</div>

				<%'2ページ目表示領域(依頼先情報一覧 Or ヘルプ内容一覧を表示する) %>
                <div class="icrop-PopOverForm-page helpRequestInnerDataBox" id="DisplayPage" style="float:left;">
				</div>

				<%'依頼先情報一覧 %>
                <div class="icrop-PopOverForm-page helpRequestInnerDataBox" id="SendAccountList" style="float:left;overflow-y:scroll;overflow-x:hidden;">
					<div class="helpRequestPopUpContactAuditBox">
						<div class="helpRequestPopUpAuditButtonAccountList">
							<ul>
							<asp:Repeater ID="SendAccountRepeater" runat="server" EnableViewState="False">
									<ItemTemplate>
										<li class="Check" id="SendAccountRow" runat="server" >
											<div class="ncv51OnOffIcn helpRequestNcv51OnIcn" id="OnlineStatusIconArea" runat="server" ></div>
											<icrop:CustomLabel ID="SendAccountName_Display" runat="server" Text='<% #HttpUtility.HtmlEncode(Eval("USERNAME")) %>'  class="helpRequestUseEllipsis" style="width:180px;"></icrop:CustomLabel>
											<%-- 2013/10/03 TCS 市川 【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START --%>
                                             <input type="hidden" class="SendAccountName" value="<%# HttpUtility.HtmlEncode(Eval("USERNAME")) %>" />
                                             <input type="hidden" class="SendAccount" value="<%# HttpUtility.HtmlEncode(Eval("ACCOUNT")) %>" />
                                             <input type="hidden" class="OnlineStatus" value="<%# HttpUtility.HtmlEncode(Eval("PRESENCECATEGORY")) %>" />
                                             <%-- 2013/10/03 TCS 市川 【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END --%>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START %>
<%'											<asp:HiddenField ID="OnlineStatus2" runat="server" Value='<％ #HttpUtility.HtmlEncode(Eval("PRESENCEDETAIL")) ％>' /> %>
<%'											<asp:HiddenField ID="OperationCode" runat="server" Value='<％ #HttpUtility.HtmlEncode(Eval("OPERATIONCODE")) ％>' /> %>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END %>
										</li>
									</ItemTemplate>
							</asp:Repeater>
							</ul>

						</div>
                    </div>
				</div>

				<%'ヘルプ内容一覧 %>
                <div class="icrop-PopOverForm-page helpRequestInnerDataBox" id="HelpMstList" style="float:left;overflow-y:scroll;overflow-x:hidden;">
					<div class="helpRequestPopUpContactAuditBox">
                      <div class="helpRequestPopUpAuditButtonHelpList">
                        <ul>
						<asp:Repeater ID="HelpMstRepeater" runat="server" EnableViewState="False">
								<ItemTemplate>
									<li class="Check" id="HelpMstRow" runat="server" >
										<icrop:CustomLabel ID="HelpName_Display" runat="server" Text='<% #HttpUtility.HtmlEncode(Eval("MSG_DLR")) %>' class="helpRequestUseEllipsis" style="width:220px;"></icrop:CustomLabel>
                                        <%-- 2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START --%>
										<%--<asp:HiddenField ID="HelpName" runat="server" Value='<% #HttpUtility.HtmlEncode(Eval("MSG_DLR")) %>' />
										<asp:HiddenField ID="Helpid" runat="server" Value='<% #HttpUtility.HtmlEncode(Eval("ID")) %>' />--%>
                                        <input type="hidden" class="HelpName" value="<%# HttpUtility.HtmlEncode(Eval("MSG_DLR")) %>" />
                                        <input type="hidden" class="Helpid" value="<%# HttpUtility.HtmlEncode(Eval("ID")) %>" />
                                        <%-- 2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END --%>
									</li>
								</ItemTemplate>
						</asp:Repeater>
                        </ul>
                      </div>
						
                    </div>
				</div>
            </div>
 		</div>
   </div>

<%'画面文言エリア 開始 %>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START %>
<%'<asp:HiddenField ID="WordNo0001HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0002HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0003HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0004HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0005HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0006HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0007HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0008HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo0009HiddenField" runat="server" /> %>
<%'<asp:HiddenField ID="WordNo9001HiddenField" runat="server" /> %>
<%'2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END %>
<%'画面文言エリア 終了 %>

<%'インクルードファイル 開始 %>
<link type="text/css"  href="../Styles/SC3080401/SC3080401.css?20120420002" rel="stylesheet" />
<script type="text/javascript" src="../Scripts/TCS/jquery.popover.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.flickable.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.PopOverForm.js"></script>
<script type="text/javascript" src="../Scripts/SC3080401/SC3080401.js?2013100300000"></script>
<%'インクルードファイル 終了 %>

