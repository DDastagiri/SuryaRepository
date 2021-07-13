<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080301.ascx.vb" Inherits="Pages_SC3080301" %>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3080301.ascx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 査定依頼                                                           %>
<%'補足：                                                                    %>
<%'作成： 2012/01/05 TCS 鈴木(恭)                                            %>
<%'更新： 2012/04/13 TCS 鈴木(恭) HTMLエンコード対応                         %>
<%'更新： 2013/12/09 TCS 市川 Aカード情報相互連携開発                        %>
<%'─────────────────────────────────────%>

<link rel="Stylesheet" href="../Styles/SC3080301/SC3080301.css?201312090000" />
<script type="text/javascript" src="../Scripts/SC3080301/SC3080301.js?20171207000000"></script>

<%'-----------------------------------------%>
<%'査定依頼ポップアップ ここから          %>
<%'-----------------------------------------%>
<%'2013/12/09 TCS 市川 Aカード情報相互連携開発 START %>
<asp:Panel ID="AssessmentCarSelectPopup" CssClass="scNsc412PopUpModelSelect" runat="server" style="display:none;z-index:10000;">
<%'2013/12/09 TCS 市川 Aカード情報相互連携開発 END %>
<div class="nsc412popWindow" >
	<div class="scNsc412PopUpModelSelectWindownBox">

        <%'ヘッダーエリア %>
		<div class="scNsc412AssessmentPopUpModelSelectHeader clip">
            <%'タイトル %>
			<h3>
                <icrop:CustomLabel ID="AssessmentCarPopupMakerTitle" runat="server" CssClass="clip" Width="120px" />
                <icrop:CustomLabel ID="AssessmentCarPopupModelTitle" runat="server" CssClass="clip" Width="120px" style="display:none;" />
            </h3>
            <%'キャンセルボタン %>
            <asp:Panel ID="scNscPopUpClosePanel" runat="server" >
			    <a href="javascript:void(0)" class="scNscAssessmentPopUpCancelButton">
                    <icrop:CustomLabel ID="AssessmentCarPopupCancelLabel" runat="server" CssClass="clip" Width="70px" />
                </a>
            </asp:Panel>
            <%'戻るボタン %>
            <asp:Panel ID="scNscPopUpReturnPanel" runat="server" style="display:none;" >
			    <a href="javascript:void(0)" class="scNscAssessmentPopUpReturnButton">
                    <icrop:CustomLabel ID="AssessmentCarPopupMakerBkLabel" runat="server" CssClass="clip" Width="70px" />
                </a>
                <span class="tgLeft">&nbsp;</span>
            </asp:Panel>
		</div>
			
		<div class="scNsc412PopUpModelSelectListArea2 ellipsis">
					
            <%'各リスト選択を囲う枠 ここから %>
            <div id="AssessmentCarSelectPopupListWrap" class="page1">

                <%'査定画面 Start %>
				<div class="scNsc412AssessmentPopUpList01">
                    <div class="scNsc412PopUpScrollWrapAssessment">
                        <asp:Panel ID="RequestStatusPanel" runat="server" >
                            <%'依頼中用 Start %>
                            <div class="nscPopUpAssessmentButton01" >
					            <div class="AssessmentText">
                                    <icrop:CustomLabel ID="RequestIraiLabel" runat="server" CssClass="ellipsis" Width="150px" UseEllipsis="false"/>
                                </div>
					            <div class="Time">
                                    <icrop:CustomLabel ID="RequestTimeLabel" runat="server" CssClass="ellipsis" Width="70px" UseEllipsis="false"/>
                                </div>
                            </div>
                            <div class="nscPopUpAssessmentButton01" >
					            <li class="scNsc412RequestCarWait">
                                    <icrop:CustomLabel ID="OtherRequestLabel" runat="server" CssClass="ellipsis center" Width="210px" UseEllipsis="false"/>
                                    <icrop:CustomLabel ID="RequestRegLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/>
                                    <icrop:CustomLabel ID="RequestCarLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/>
                                </li>
                            </div>
                            <asp:Panel ID="AssessmentEnableCancelButtonPanel" runat="server" >
                            <div class="nscPopUpAssessmentCancelButton" id="assessmentCancelButtonId" >
                                <%--<a href="javascript:void(0)" id="AssessmentCancelLink" >--%>
                                    <icrop:CustomLabel ID="AssessmentCancleLabel" runat="server" CssClass="clip" />
                                <%--</a>--%>
                            </div>  
                            </asp:Panel>
                            <asp:Panel ID="AssessmentDisableCancelButtonPanel" runat="server" >
                                <div class="nscPopUpAssessmentDummyButton">
                                    <icrop:CustomLabel ID="AssessmentCancleDummyLabel" runat="server" CssClass="clip" />
                                </div>
                            </asp:Panel>
                            <%'依頼中用 Start %>
                        </asp:Panel>
                        <asp:Panel ID="EndStatusPanel" runat="server" >
                            <%'査定済み用 End %>
                            <asp:Panel ID="AssessmentResultPanel" runat="server" >
                                <div class="nscPopUpContactCarCheckButton01" >
					                <li class="scNsc412CarCheckSheetLi">
                                        <icrop:CustomLabel ID="AssessmentDateLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/><br />
                                        <icrop:CustomLabel ID="AssessmentPriceLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/>
                                    </li>
                                </div>
                            </asp:Panel>
					        <ul>
                                <ItemTemplate>
                                    <div class="nscPopUpAssessmentButton01" >
					                        <li seqno='' id="AssessmentList1" class="scNsc412AssessmentListLi1">
                                                <icrop:CustomLabel ID="OtherAssessmentLabel" runat="server" CssClass="ellipsis center" Width="210px" UseEllipsis="false"/>
                                                <icrop:CustomLabel ID="AssessmentRegLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/>
                                                <icrop:CustomLabel ID="AssessmentCarLabel" runat="server" CssClass="ellipsis" Width="210px" UseEllipsis="false"/>
                                            </li>
                                    </div>
                                </ItemTemplate>
					        </ul>
                            <asp:Panel ID="AssessmentEnableButtonPanel" runat="server" >
                                <div class="nscPopUpAssessmentButton" >
                                    <%--<a href="javascript:void(0)" id="AssessmentRequestLink" >--%>
                                        <icrop:CustomLabel ID="AssessmentRequestLabel" runat="server" CssClass="clip" />
                                    <%--</a>--%>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="AssessmentDisableButtonPanel" runat="server" >
                                <div class="nscPopUpAssessmentDummyButton">
                                    <icrop:CustomLabel ID="AssessmentRequestDummyLabel" runat="server" CssClass="clip" />
                                </div>
                            </asp:Panel>
                            <%'査定済み用 End %>
                        </asp:Panel>
                        <%'エラー表示用 Start %>
                        <asp:Panel ID="AssessmentErrorPanel" runat="server" style="display:none;" >
                            <div class="ContactRequest"></div>
                            <div class="ContactRequestText">
                                <icrop:CustomLabel ID="ErrorLabel" runat="server" CssClass="center" Width="200px" UseEllipsis="false"/>
                            </div>
                        </asp:Panel>
                        <%'エラー表示用 End %>
                    </div>
				</div>
                <%'査定画面 End %>
                            
                <%'保有車両選択 Start %>
				<div class="scNsc412AssessmentPopUpList02">
                    <div class="scNsc412PopUpScrollWrapAssessment">
					    <ul>
                            <asp:Repeater runat="server" ID="Repeater1" EnableViewState="False">
                                <ItemTemplate>
                                    <li seqno='<%#HttpUtility.HtmlEncode(Eval("SEQNO"))%>'
                                        vin='<%#HttpUtility.HtmlEncode(Eval("VIN"))%>' 
                                        carno='<%#HttpUtility.HtmlEncode(Eval("VCLREGNO"))%>' 
                                        carname='<%#HttpUtility.HtmlEncode(Eval("CARNAME"))%>'
                                        assessno='<%#HttpUtility.HtmlEncode(Eval("ASSESSMENTNO"))%>'
                                        noticeid='<%#HttpUtility.HtmlEncode(Eval("NOTICEREQID"))%>'
                                        insdate='<%#HttpUtility.HtmlEncode(Eval("INSPECTIONDATE"))%>'
                                        price='<%#HttpUtility.HtmlEncode(Eval("APPRISAL_PRICE"))%>'
                                        status='<%#HttpUtility.HtmlEncode(Eval("STATUS"))%>'
                                        retention='1'
                                        class="scNsc412AssessmentListLi2 NoArrow">
                                        <%'2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START %>
                                        <icrop:CustomLabel runat="server" CssClass="ellipsis" Text='<%#HttpUtility.HtmlEncode(Eval("VCLREGNO"))%>' Width="210px" UseEllipsis="false"/><br />
                                        <icrop:CustomLabel runat="server" CssClass="ellipsis" Text='<%#HttpUtility.HtmlEncode(Eval("CARNAME"))%>' Width="210px" UseEllipsis="false"/>
                                        <%'2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END %>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        	<li seqno='0' vin='' carno='' carname='' assessno='' noticeid='' insdate='' price='' retention='0' status='' 
                                class="scNsc412AssessmentListLi2 NoArrow"><icrop:CustomLabel ID="CustomLabelOtherCar" runat="server" CssClass="center" /></li>
				        </ul>
                    </div>
				</div>
                <%'保有車両選択 End %>
                <div class="clearboth"></div>
                <div>
                    <asp:HiddenField ID="SelectAssSeqnoHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectAssVinHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectInspectionDateHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectApprisalPriceHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectAssessmentNoHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectNoticeReqIdHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectStatusHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherAssessmentNoHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherNoticeReqIdHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherDateHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherPriceHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherStatusHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectOtherUpdateDateHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectRetentionHidden" runat="server" value=""/>
                    <asp:HiddenField ID="SelectAccountStatusHidden" runat="server" value=""/>
                    <asp:HiddenField ID="SelectCarnoHidden" runat="server" value="" />
                    <asp:HiddenField ID="SelectCarnameHidden" runat="server" value="" />
                    <asp:HiddenField ID="AssessmentPopupProcessHidden" runat="server" value="" />
                 </div>
            </div>
            <%'各リスト選択を囲う枠 ここまで %>
		</div>
	</div>
	<div class="scNsc412PopUpModelSelectArrow"></div>
    <div style="display:none"><asp:button ID="linkCarCheckSheetButtonDummy" runat="server" UseSubmitBehavior="False" /></div>
    <%'登録時のオーバーレイ Start %>
    <div id="registOverlayBlackSC3080301"></div>
    <div id="processingServerSC3080301"></div>
    <%'2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 %>
    <asp:HiddenField ID="IsCarCheckSheetOpenHidden" runat="server" value="False" />
    <div id="registOverlayBlackSC3080301_Redirect"></div>
    <div id="processingServerSC3080301_Redirect"></div>
    <%'2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 %>
    <%'登録時のオーバーレイ End %>
</div>
</asp:Panel>
<%'査定依頼ポップアップ ここまで %>
   
