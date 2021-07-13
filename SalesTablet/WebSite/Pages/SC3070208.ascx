<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070208.ascx.vb" Inherits="Pages_SC3070208" EnableTheming="false" EnableViewState="false" %>
<div id="SC3070208_PopOverForm" runat="server">
	<div id="SC3070208_PopOverFormHeader"  class='icrop-PopOverForm-header nscPopUpHeader'>
		<div class='icrop-PopOverForm-header-left'>
			<a href="#" class="nscPopUpCancelButton"></a>
		</div>
		<div class='icrop-PopOverForm-header-title' style="line-height:28px;">
			<h3 style="position: absolute; width:170px; left:100px; text-align:left;">
				<icrop:CustomLabel ID="SC3070208_HeaderTitle" runat="server" class="useEllipsis">
                    <%: HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070208", 1))%>
                </icrop:CustomLabel>
			</h3>
		</div>
		<div class='icrop-PopOverForm-header-right'></div>
		<%'処理中のオーバーレイ %>
		<div id="SC3070208_registOverlayBlack"></div>
		<div id="SC3070208_processingServer"></div>
	</div> 
    <div class="icrop-PopOverForm-content" style="width:256px;overflow:hidden;" >
		<div id="SC3070208_PopOverFormSheet" class="icrop-PopOverForm-sheet dataBox" style="width:1025px;" >
			<%'注文承認依頼メイン %>
			<div class="icrop-PopOverForm-page innerDataBox" id="SC3070208_Main" style="float:left;overflow-y:hidden;overflow-x:hidden;">
				<%'注文承認中情報 %>
				<div class="innerDataBoxItem" id="SC3070208_UnderRequestArea" runat="server">
                    <div class="AssessmentText">
                        <asp:Literal ID="SC3070208_UnderRequest" runat="server" Mode="Encode" ></asp:Literal>
                    </div>
                    <div class="Time useCut">
                        <asp:Literal ID="SC3070208_RequestDate" runat="server" Mode="Encode" ></asp:Literal>
                    </div>
				</div>

				<%'依頼先 %>
                <div id="SC3070208_SelectedSalesMangerArea" class="innerDataBoxItem">
                    <div id="SC3070208_SelectedSalesMangerNameAreaBox" runat="server" class="">
                        <div id="SC3070208_SelectedSalesMangerNameArea" class="useEllipsis" style="width:230px;">
                            <asp:Literal ID="SC3070208_SelectedSalesMangerName_Display" runat="server" Mode="Encode" ></asp:Literal>
                        </div>
                    </div>
                    <asp:HiddenField ID="SC3070208_SelectedSalesMangerName" runat="server" />
                    <asp:HiddenField ID="SC3070208_SelectedManagerAccount" runat="server" />
                    <asp:HiddenField ID="SC3070208_SelectedManagerOnlineStatus" runat="server" />
                    <asp:HiddenField ID="SC3070208_NoticeRequestid" runat="server" />
                    <asp:HiddenField ID="SC3070208_IsUnderRequest" runat="server" />
                </div>

				<%'コメント %>
				<div class="innerDataBoxItem">
                    <div id="SC3070208_StaffMemoArea" class="" runat="server">
						<div id="SC3070208_StaffMemoDisplayArea">
                        </div>
					</div>
                    <div style="height:48px;"></div>
    				<asp:HiddenField ID="SC3070208_StaffMemo" runat="server" />
				</div>
                    
				<%'依頼／キャンセルボタン %>
                <div id="SC3070208_HiddenButtonArea">
			        <div id="SC3070208_RequestButton" runat="server">
				        <asp:Literal ID="SC3070208_RequestButtonLiteral" runat="server" Mode="Encode" ></asp:Literal>
			        </div>
			        <div id="SC3070208_CancelButton" runat="server">
				        <asp:Literal ID="SC3070208_CancelButtonLiteral" runat="server" Mode="Encode" ></asp:Literal>
			        </div>
                </div>
			</div>

			<%'2ページ目表示領域(セールスマネージャー一覧　Or　値引き理由一覧を表示する) %>
			<div class="icrop-PopOverForm-page innerDataBox" id="SC3070208_DisplayPage" style="float:left;">
			</div>

			<%'セールスマネージャー一覧 %>
			<div class="icrop-PopOverForm-page innerDataBox" id="SC3070208_SalesManagerList" style="float:left;overflow-y:scroll;overflow-x:hidden;">
				<div class="innerDataBoxItem">
					<div class="innerDataBoxListItem">
						<ul>
						<asp:Repeater ID="SC3070208_ApprovalStaffRepeater" runat="server" EnableViewState="False">
								<ItemTemplate>
									<li class="Check" id="SC3070208_ApprovalStaffRow" runat="server" >
										<div class="SC3070208_OnOffIcn" id="SC3070208_OnlineStatusIconArea" runat="server" ></div>
        								<icrop:CustomLabel ID="SC3070208_ApprovalStaffNameLabel" runat="server" Text='<%# Server.HTMLEncode(Eval("USERNAME"))%>'  class="useEllipsis" style="width:180px;"></icrop:CustomLabel>
                                        <input type="hidden" class="SalesMangerName" value="<%# Eval("USERNAME")%>" />
                                        <input type="hidden" class="SalesMangerAccount" value="<%# Eval("ACCOUNT")%>" />
                                        <input type="hidden" class="OnlineStatus" value="<%# Eval("PRESENCECATEGORY")%>" />
									</li>
								</ItemTemplate>
						</asp:Repeater>
						</ul>
					</div>
				</div>
			</div>

			<%'対応者いない画面 %>
			<div class="icrop-PopOverForm-page innerDataBox" id="SC3070208_NoSendAccountArea" style="float:left;">
				<div id="SC3070208_NoSendAccountBox" class="innerDataBoxItem" runat="server" >
					<div id="SC3070208_NoSendAccountImg">&nbsp;</div>
					<icrop:CustomLabel ID="SC3070208_NoSendAccountLabel" runat="server" class="useCut"></icrop:CustomLabel>
					<asp:HiddenField ID="SC3070208_IsExistManager" runat="server"  />
				</div>
			</div>
		</div>
        <div id="SC3070208_ButtonArea"></div>
	</div>
</div>

   
<%'エラーメッセージエリア 開始 %>
<span id="SC3070208_HeaderTitleWord1" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 1)%></span>
<span id="SC3070208_HeaderCancelWord" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 2)%></span>
<span id="SC3070208_HeaderBack1" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 1)%></span>
<span id="SC3070208_HeaderTitleWord2" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 9)%></span>
<span id="SC3070208_HeaderTitleWord4" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 8)%></span>
<span id="SC3070208_CommentPlaceHolderWord" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 7)%></span>
<span id="SC3070208_SelfWord" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070208", 3)%></span>
<span id="SC3070208_SelfAccount" style="visibility:hidden"><%:StaffContext.Current.Account%></span>
<%'エラーメッセージエリア 終了 %>

<link rel="Stylesheet" href="../Styles/SC3070208/SC3070208.css?20150326000000" />
<script type="text/javascript" src="../Scripts/TCS/jquery.NumericKeypad.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.popover.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.flickable.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.PopOverForm.js"></script>
<script type="text/javascript" src="../Scripts/SC3070208/SC3070208.js?20150326000001"></script>
