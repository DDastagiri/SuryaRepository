<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070203.ascx.vb" Inherits="Pages_SC3070203" EnableTheming="false" EnableViewState="false" %>
	<div id="SC3070203PopOverForm" runat="server" 
		Estimateid="" 
		RequestPrice=""
        RequestStaffMemo=""
		Customerid=""
		CustomerName=""
		CustomerClass=""
		CustomerKind=""
		FollowUpBoxStoreCode=""
		FollowUpBoxNumber=""
		VehicleSequenceNumber=""
		SalesStaffCode=""
		>
		<div id='SC3070203_PopOverFormHeader' class='icrop-PopOverForm-header'>
			<div class='icrop-PopOverForm-header-left'>
				<a href="#" class="nscPopUpCancelButton"></a>
			</div>
			<div class='icrop-PopOverForm-header-title' style="line-height:28px;">
				<h3 style="position: absolute; width:170px; left:100px; text-align:left;">
					<icrop:CustomLabel ID="SC3070203_HeaderTitle" runat="server" class="useEllipsis"></icrop:CustomLabel>
				</h3>
			</div>
			<div class='icrop-PopOverForm-header-right'></div>
			<%'処理中のオーバーレイ %>
			<div id="SC3070203_registOverlayBlack"></div>
			<div id="SC3070203_processingServer"></div>
		</div> 
        <%'2015/03/05 TSC 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】MOD START %>
		<%--<div class="icrop-PopOverForm-content" style="width:256px;height:232px;overflow:hidden;" >--%>
        <div class="icrop-PopOverForm-content" style="width:256px;overflow:hidden;" >
        <%'2015/03/05 TSC 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】MOD END %>
			<div id="SC3070203_PopOverFormSheet" class="icrop-PopOverForm-sheet dataBox" style="width:1280px;" >
				<%'価格相談メイン %>
				<div class="icrop-PopOverForm-page innerDataBox" id="SC3070203_Main" style="float:left;overflow-y:hidden;overflow-x:hidden;">
					<%'価格相談中情報 %>
					<div class="innerDataBoxItem" id="SC3070203_UnderRequestArea" runat="server">
                        <div class="AssessmentText">
                            <asp:Literal ID="SC3070203_UnderRequest" runat="server" Mode="Encode" ></asp:Literal>
                        </div>
                        <div class="Time useCut">
                            <asp:Literal ID="SC3070203_RequestDate" runat="server" Mode="Encode" ></asp:Literal>
                        </div>
					</div>

					<%'履歴内容 %>
					<div class="innerDataBoxItem" id="SC3070203_NewestHistoryArea" runat="server">
					  <div>
						<asp:Literal ID="SC3070203_ApprovedDate" runat="server" Mode="Encode" ></asp:Literal>
						<br/>
						<asp:Literal ID="SC3070203_ApprovedPrice" runat="server" Mode="Encode" ></asp:Literal>
					  </div>
					</div>

					<%'価格相談内容 %>
					<div class="innerDataBoxItem" id="SC3070203_SelectedSalesMangerArea" runat="server">
					  <div class="innerDataBoxContent" id="SC3070203_SelectedSalesMangerNameAreaRow" runat="server">
						<div id="SC3070203_SelectedSalesMangerNameArea" class="useEllipsis" style="width:230px;">
							<asp:Literal ID="SC3070203_SelectedSalesMangerName_Display" runat="server" Mode="Encode" ></asp:Literal>
						</div>
						
					  </div>
					  <asp:HiddenField ID="SC3070203_SelectedSalesMangerName" runat="server" />
					  <asp:HiddenField ID="SC3070203_SelectedManagerAccount" runat="server" />
					  <asp:HiddenField ID="SC3070203_SelectedManagerOnlineStatus" runat="server" />
					  <asp:HiddenField ID="SC3070203_NoticeRequestid" runat="server" />
					  <asp:HiddenField ID="SC3070203_IsUnderRequest" runat="server" />
					  <asp:HiddenField ID="SC3070203_HasHistory" runat="server" />
					</div>

					<div class="innerDataBoxItem" id="SC3070203_SelectedReasonArea" runat="server">
					  <div class="innerDataBoxContent" id="SC3070203_SelectedResonNameAreaRow" runat="server">
						<div id="SC3070203_SelectedResonNameArea" class="useEllipsis" style="width:230px;">
							<asp:Literal ID="SC3070203_SelectedResonName_Display" runat="server" Mode="Encode" ></asp:Literal>
						</div>
					  </div>
					  <asp:HiddenField ID="SC3070203_SelectedResonName" runat="server" />
					  <asp:HiddenField ID="SC3070203_SelectedResonid" runat="server" />
					</div>
					<div class="innerDataBoxItem">
					  <div class="innerDataBoxContent2" id="RequestPriceNewArea" runat="server">
						<asp:Literal ID="RequestPriceNew_Display" runat="server" Mode="Encode" ></asp:Literal>
					  </div>
					  <asp:HiddenField ID="RequestPriceNew" runat="server" />
					</div>
                    <%'2015/03/05 TSC 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】MOD START %>
				    <%'コメント %>
					<div class="innerDataBoxItem">
                        <div id="SC3070203_StaffMemoArea" class="" runat="server">
						    <div id="SC3070203_StaffMemoDisplayArea">
        						<asp:Literal ID="SC3070203_StaffMemoDisplay" runat="server" Mode="Encode" ></asp:Literal>
                            </div>
						</div>
                        <div style="height:48px;"></div>
    					<asp:HiddenField ID="SC3070203_StaffMemo" runat="server" />
					</div>
                    <div id="SC3070203_HiddenButtonArea" style="height:32px;">
			            <div id="SC3070203_RequestButton" runat="server">
				            <asp:Literal ID="SC3070203_RequestButtonLiteral" runat="server" Mode="Encode" ></asp:Literal>
			            </div>
			            <div id="SC3070203_CancelButton" runat="server">
				            <asp:Literal ID="SC3070203_CancelButtonLiteral" runat="server" Mode="Encode" ></asp:Literal>
			            </div>
                    </div>
                    <%'2015/03/05 TSC 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】MOD END %>
				</div>

				<%'2ページ目表示領域(セールスマネージャー一覧　Or　値引き理由一覧を表示する) %>
				<div class="icrop-PopOverForm-page innerDataBox" id="SC3070203_DisplayPage" style="float:left;">
				</div>

				<%'セールスマネージャー一覧 %>
				<div class="icrop-PopOverForm-page innerDataBox" id="SC3070203_SalesManagerList" style="float:left;overflow-y:scroll;overflow-x:hidden;">
					<div class="innerDataBoxItem">
						<div class="innerDataBoxListItem">
							<ul>
							<asp:Repeater ID="SC3070203_SalesManagerRepeater" runat="server">
									<ItemTemplate>
										<li class="Check" id="SC3070203_SalesMangerRow" runat="server" >
											<div class="ncv51OnOffIcn ncv51OnIcn" id="SC3070203_OnlineStatusIconArea" runat="server" ></div>
											<icrop:CustomLabel ID="SC3070203_SalesMangerName_Display" runat="server" Text='<%# Server.HTMLEncode(Eval("USERNAME"))%>'  class="useEllipsis" style="width:180px;"></icrop:CustomLabel>
											<%'2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START %>
<%--                                            <asp:HiddenField ID="SalesMangerName" runat="server" Value='<%# Eval("USERNAME")%>' />
											<asp:HiddenField ID="SalesMangerAccount" runat="server" Value='<%#Eval("ACCOUNT")%>' />
											<asp:HiddenField ID="OnlineStatus" runat="server" Value='<%#Eval("PRESENCECATEGORY")%>' />--%>
                                            <input type="hidden" class="SalesMangerName" value="<%# Eval("USERNAME")%>" />
                                            <input type="hidden" class="SalesMangerAccount" value="<%# Eval("ACCOUNT")%>" />
                                            <input type="hidden" class="OnlineStatus" value="<%# Eval("PRESENCECATEGORY")%>" />
                                            <%'2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END %>
										</li>
									</ItemTemplate>
							</asp:Repeater>
							</ul>

						</div>
					</div>
				</div>

				<%'値引き理由一覧 %>
				<div class="icrop-PopOverForm-page innerDataBox" id="PriceConsultationResonList" style="float:left;overflow-y:scroll;overflow-x:hidden;">
					<div class="innerDataBoxItem">
					  <div class="innerDataBoxListItem">
						<ul>
						<asp:Repeater ID="PriceConsultationResonRepeater" runat="server">
								<ItemTemplate>
									<li class="Check" id="PriceConsultationResonRow" runat="server"  >
										<div ID="ResonName_DisplayArea" class="useEllipsis" style="width:180px;">
											<asp:Literal ID="ResonName_Display" runat="server" Mode="Encode" Text='<%#Eval("MSG_DLR")%>'></asp:Literal>
										</div>
                                        <%'2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START %>
										<%--<asp:HiddenField ID="ResonName" runat="server" Value='<%#Eval("MSG_DLR")%>' />
										<asp:HiddenField ID="Resonid" runat="server" Value='<%#Eval("ID")%>' />--%>
                                        <input type="hidden" class="ResonName" value="<%#Eval("MSG_DLR")%>" />
                                        <input type="hidden" class="Resonid" value="<%#Eval("ID")%>" />
                                        <%'2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END %>
									</li>
								</ItemTemplate>
						</asp:Repeater>
						</ul>
					  </div>
						
					</div>
				</div>
                
				<%'対応者いない画面 %>
				<div class="icrop-PopOverForm-page innerDataBox" id="SC3070203_NoSendAccountArea" style="float:left;">
					<div id="SC3070203_NoSendAccountBox" class="innerDataBoxItem" runat="server" >
						<div id="SC3070203_NoSendAccountImg">&nbsp;</div>
						<asp:Literal ID="SC3070203_NoSendAccountLabel" runat="server" Mode="Encode" ></asp:Literal>
						<asp:HiddenField ID="SC3070203_IsExistManager" runat="server"  />
					</div>
				</div>
			</div>
            <div id="SC3070203_ButtonArea"></div>
		</div>
   </div>
   



   
   <%'エラーメッセージエリア 開始 %>
   <span id="SC3070203_HeaderTitleWord1" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 1)%></span>
   <span id="SC3070203_HeaderCancelWord" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 2)%></span>
   <span id="SC3070203_HeaderBack1" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 6)%></span>
   <span id="SC3070203_HeaderTitleWord2" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 7)%></span>
   <span id="SC3070203_HeaderTitleWord3" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 8)%></span>
   <span id="SC3070203_HeaderTitleWord4" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 13)%></span>
   <span id="SC3070203_NumericPadCancel" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 10)%></span>
   <span id="SC3070203_NumericPadOk" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 11)%></span>
   <span id="SC3070203_CommentPlaceHolderWord" style="visibility:hidden"><%:WebWordUtility.GetWord("SC3070203", 12)%></span>

   <%'エラーメッセージエリア 終了 %>

   <link rel="Stylesheet" href="../Styles/SC3070203/SC3070203.css?20150326000000" />
   <script type="text/javascript" src="../Scripts/TCS/jquery.NumericKeypad.js"></script>
   <script type="text/javascript" src="../Scripts/TCS/jquery.popover.js"></script>
   <script type="text/javascript" src="../Scripts/TCS/jquery.flickable.js"></script>
   <script type="text/javascript" src="../Scripts/TCS/jquery.PopOverForm.js"></script>
   <script type="text/javascript" src="../Scripts/SC3070203/SC3070203.js?20150326000001"></script>



