<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SC3010201.master" AutoEventWireup="false" CodeFile="SC3140103.aspx.vb" Inherits="Pages_SC3140103" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SC3010201head" Runat="Server">
    <%'HEAD %>
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3140103/SC3140103.css?20180704000000" />
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.flickable.js?20121217000000"></script>
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.Main.js?20190719000000"></script>
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.CustomLabelEx.js?20140117000000"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SC3010201leftBottomBox" Runat="Server">
    <%'ダッシュボード %>
    <div id="dashboardBox">
        <%'2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START %>
        <%--<iframe id="dashboardFrame"  height="100%" width="100%" src="SC3140102.aspx"></iframe>--%>
        <%'2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END %>
        <%'読み込み中 %>
        <%--<div id="loadingDashboard"></div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SC3010201rightBox" Runat="Server">
        <%--カウンター対応--%>
    <script type="text/javascript">
        var diffseconds = (new Date('<%=Me.nowDateTime%>')).getTime() - (parseInt((new Date()).getTime() / 1000) * 1000);
        setInterval("proccounter(diffseconds);", 1000);
    </script>
    <%--カウンター対応--%>    

    <%--詳細ポップアップウィンドウ用--%>
    <%-- 詳細画面ポップアップ --%>
    <div id="CustomerPopOver2" class="saPopOver2">
        <%-- ヘッダー --%>
	    <div class="header" >
			<div class="headerScroll">
				<div class="headerInner">
                    <div id="statusHeader">
						<%--<input type="button" class="ButtonLeft" ID="SearchCancel" runat="server" onclick="SlideStatus('click');"/>--%>
                        <div class="ButtonLeft"id="ButtonLeft" onclick="SlideStatus('click');">
                            <icrop:CustomLabel ID="SearchCancel" CssClass="SearchCancelDiv Ellipsis" UseEllipsis="False" TextWordNo="60" runat="server" Text="" />
                        </div>
						<h3>
                            <icrop:CustomLabel ID="CustomLabel2" CssClass="Ellipsis" runat="server" TextWordNo="10" UseEllipsis="False"  Width="220" Height="28px"></icrop:CustomLabel>
                        </h3>
                        <%--<input type="button" ID="ChipChanges" runat="server" class="ButtonRight" onclick="ChipChange();"/>--%>
                        <div class="ButtonRight" id="ButtonRight" onclick="ChipChange();">
                            <icrop:CustomLabel ID="ChipChanges" CssClass="ChipChangesDiv Ellipsis" UseEllipsis="False" TextWordNo="61" runat="server" Text="" />
                        </div>
					</div>
					<%--<div id="statusHeader">
						<icrop:CustomLabel ID="PopupHeader2" runat="server" TextWordNo="10" UseEllipsis="False" Height="19px"></icrop:CustomLabel>
					</div>
					<div id="searchHeader">
						<input type="button" class="ButtonLeft" ID="SearchCancel" runat="server" onclick="SlideStatus();"/>
						<icrop:CustomLabel ID="SearchHeader" runat="server" TextWordNo="10" UseEllipsis="False" Height="19px"></icrop:CustomLabel>
						<input type="button" ID="ChipChanges" runat="server" class="ButtonRight" onclick="ChipChange();"/>
					</div>--%>
				</div>
			</div>
		</div>
        <%-- 詳細 --%>
		<div class="contentScroll">            
			<div class="contentInner">
				<div id="status" class="content">
                <div class="OverShadow">&nbsp;</div>
                <%-- 詳細ポップアップウィンドウの読み込み中アイコン --%>
				<div id="IconLoadingPopup" class="loadingPopup" runat="server"></div>
					<asp:UpdatePanel ID="ContentUpdatePanelDetail" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:HiddenField ID="HiddenDetailsROButtonStatus" runat="server" />
							<asp:HiddenField ID="HiddenDetailsCustomerButtonStatus" runat="server" />
							<asp:HiddenField ID="HiddenDetailsApprovalButtonStatus" runat="server" />
							<asp:HiddenField ID="HiddenServerTime" runat="server" />
							<asp:HiddenField ID="HiddenDeliveryPlanUpdateCount" runat="server" />
                            <asp:HiddenField ID="HiddenDetailsInspectionButtonStatus" runat="server" />
                            <%-- 2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START--%>
                            <asp:HiddenField ID="HiddenVehicleModel" runat="server" />
                            <%-- 2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END--%>
                            <div>
							    <div class="DetailFlickableBox">
							    	<div>
							    		<div class="DetailInnerBox">
							    			<div  id="StatusInfoAreaDiv" class="StatusInfoAreaDiv">
							    				<div id="StatusInfoInnaerDataBoxDiv" class="StatusInfoInnaerDataBoxDiv">
							    					<div class="StatusDiv" id="StatusDiv">
                                                        <div id="StatusInnerDiv">
							    						<div id="IconStatusDiv">
							    							<icrop:CustomLabel ID="IconStatsLabel" runat="server" CssClass="Ellipsis" Width="345"></icrop:CustomLabel>
							    						</div>
				                                        <div id="InterruptionCauseDiv">
                                                            <div id="InterruptionCauseRepeaterDiv">
							    							<asp:Repeater ID="InterruptionCauseRepeater" runat="server">
							    								<HeaderTemplate>
							    									<div class="addStatus">
							    								</HeaderTemplate>

							    								<ItemTemplate>
							    									<icrop:CustomLabel ID="InterruptionCauseLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("InterruptionCause")) %>' CssClass="Ellipsis" Width="345"></icrop:CustomLabel>
							    									<icrop:CustomLabel ID="InterruptionDetailsLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("InterruptionDetails")) %>' CssClass="Ellipsis" Width="345"></icrop:CustomLabel>
							    								</ItemTemplate>

							    								<FooterTemplate>
							    						
							    								</FooterTemplate>
							    							</asp:Repeater>
                                                            </div>
							    						</div>
							    						<div class="AddInformationBox">
                  			    							<div class="AddInformationPlan">
							    								<icrop:CustomLabel ID="FixDeliveryTimeLabel" runat="server" TextWordNo="37" CssClass="Ellipsis" Width="52"></icrop:CustomLabel>
							    								<icrop:CustomLabel ID="DeliveryTimeLabel" runat="server" CssClass="Ellipsis"></icrop:CustomLabel>
							    								<icrop:CustomLabel ID="FixSlashLabel" runat="server" TextWordNo="38" CssClass="Ellipsis" ></icrop:CustomLabel>
							    								<icrop:CustomLabel ID="ChangeCountLabel" runat="server" CssClass="Ellipsis" Width="56" ></icrop:CustomLabel>
							    							</div>
                  			    							<div class="AddInformationArrow"><icrop:CustomLabel ID="FixDownArrow" TextWordNo="40" runat="server" CssClass="Ellipsis" ></icrop:CustomLabel></div>
                  			    							<div class="AddInformationExpected">
							    								<icrop:CustomLabel ID="FixDeliveryEstimateLabel" runat="server" TextWordNo="41" CssClass="Ellipsis" Width="65" ></icrop:CustomLabel>
							    								<icrop:CustomLabel ID="DeliveryEstimateLabel" runat="server"  CssClass="Ellipsis" ></icrop:CustomLabel>
							    							</div>
							    						</div>
                                                        </div>
							    					</div>
							    					<div id="HeadInfomationPullDiv" class="HeadInfomationPullDiv">
              				    						<ul>
							    							<asp:Repeater ID="ChangeTimeRepeater" runat="server">
							    								<ItemTemplate>
                			    									<li>
                    		    										<div class="ChangeTimeDiv">
							    											<icrop:CustomLabel ID="ChangeFromTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeFromTime")) %>' CssClass="Ellipsis"></icrop:CustomLabel>
							    											<icrop:CustomLabel ID="RightArrowLabel" runat="server" TextWordNo="50" CssClass="Ellipsis"></icrop:CustomLabel>
							    											<icrop:CustomLabel ID="ChangeToTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeToTime")) %>' CssClass="Ellipsis"></icrop:CustomLabel>
							    										</div>
                    		    										<div class="UpdateTimeDiv"><icrop:CustomLabel ID="UpdateTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdateTime")) %>' CssClass="Ellipsis"></icrop:CustomLabel></div>
							    										<div class="UpdatePretextDiv"><icrop:CustomLabel ID="UpdatePretextLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdatePretext")) %>' CssClass="Ellipsis" Width="310"></icrop:CustomLabel></div>
							    									</li>
							    								</ItemTemplate>
							    							</asp:Repeater>
                			    							<li class="PullButton"><icrop:CustomLabel ID="FixUpArrow" runat="server" TextWordNo="51" CssClass="Ellipsis" ></icrop:CustomLabel></li>
							    						</ul>
							    					</div>
							    				</div>
							    			</div>
							    			<div>
							    			<%-- 来店者呼出エリア 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START--%>
                                                <div id="VisitCustomer" runat="server" style="display:none">
                                                <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                                                      <tbody><tr>
                                                        <th><icrop:CustomLabel ID="ItemCallNo" runat="server" TextWordNo="96" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
                                                        <td>
                                                            <icrop:CustomLabel ID="DetailsCallNo" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel>
                                                        </td>
                                                      </tr>
                                                      <tr>
                                                        <th><icrop:CustomLabel ID="ItemVisitName" runat="server"  TextWordNo="100" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
                                                        <td><icrop:CustomLabel ID="DetailsVisitName" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
                                                      </tr>
                                                      <tr>
                                                        <th><icrop:CustomLabel ID="ItemVisitTelno" runat="server"  TextWordNo="101" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
                                                        <td><icrop:CustomLabel ID="DetailsVisitTelno" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
                                                      </tr>
                                                      <tr>
                                                        <th class="ListEnd"><icrop:CustomLabel ID="ItemCallPlace" runat="server"  TextWordNo="97" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
                                                        <td class="ListEnd"><icrop:CustomTextBox ID="DetailsCallPlace" runat="server" Text="" CssClass="td" Width="201" MaxLength="128" onchange="CallPlaceChange();"></icrop:CustomTextBox></td>
                                                      </tr>
                                                    </tbody>
                                                </table>
                                                    <div class="ListBtn01" ID="BtnCALL" onclick="CustomerCall();" runat="server"><icrop:CustomLabel ID="CustomerCall" runat="server"  TextWordNo="98" CssClass="Ellipsis" Width="350"></icrop:CustomLabel></div>
                                                    <div class="ListBtn02" ID="BtnCALLCancel" onclick="CustomerCallCancel();" runat="server"><icrop:CustomLabel ID="CallCancel" runat="server"  TextWordNo="99" CssClass="Ellipsis" Width="350"></icrop:CustomLabel></div>
                                                    </div>
							    				<%-- 来店者呼出エリア 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END--%>
							    				<table border="0" cellspacing="0" cellpadding="0" class="ListSet">
							    					<tr>
							    						<th>
							    						    <div class="SearchIcon" onclick="return SlideSearch();"></div>
							    						    <icrop:CustomLabel ID="ItemRegistrationNumber" runat="server" TextWordNo="11" CssClass="Ellipsis" Width="90"></icrop:CustomLabel>
							    						</th>
							    						<td>
                                                            <icrop:CustomLabel ID="DetailsRegistrationNumber" runat="server" Text="" CssClass="Ellipsis" Width="155"></icrop:CustomLabel>
                                                            <icrop:CustomLabel ID="DetailsProvince" runat="server" Text="" CssClass="Ellipsis" Width="155" ></icrop:CustomLabel>
							    							<div class="IcnSet">
							    								<icrop:CustomLabel  ID="DetailsRightIconD" runat="server" text="" visible="False"  CssClass="PopoverRightIcnD" TextWordNo="7"></icrop:CustomLabel>
							    								<%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                                <%-- <icrop:CustomLabel  ID="DetailsRightIconI" runat="server" text="" visible="False"  CssClass="PopoverRightIcnI" TextWordNo="8"></icrop:CustomLabel> --%>
							    								<icrop:CustomLabel  ID="DetailsRightIconP" runat="server" text="" visible="False"  CssClass="PopoverRightIcnP" TextWordNo="10005"></icrop:CustomLabel>
                                                                <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
							    								<icrop:CustomLabel  ID="DetailsRightIconS" runat="server" text="" visible="False"  CssClass="PopoverRightIcnS" TextWordNo="9"></icrop:CustomLabel>
							    							</div>
							    						</td>
							    					</tr>
							    					<tr>
							    						<th class="ListEnd"><icrop:CustomLabel ID="ItemCarModel" runat="server" TextWordNo="12" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td class="ListEnd">
							    							<icrop:CustomLabel ID="DetailsCarModel" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel>
							    							<icrop:CustomLabel ID="DetailsModel" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel>
							    						</td>
							    					</tr>
							    				</table>
							    				<table border="0" cellspacing="0" cellpadding="0" class="ListSet">
							    					<tr>
							    						<th><icrop:CustomLabel ID="ItemCustomerName" runat="server" Text="" TextWordNo="17" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td><%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                            <%-- <icrop:CustomLabel ID="DetailsCustomerName" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel> --%>
                                                            <icrop:CustomLabel ID="DetailsCustomerName" runat="server" Text="" CssClass="Ellipsis" Width="195"></icrop:CustomLabel>
                                                            <div class="IcnSet2">
                                                                <icrop:CustomLabel  ID="DetailsRightIconL" runat="server" text="" visible="False"  CssClass="PopoverRightIcnL" TextWordNo="10006"></icrop:CustomLabel></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                        </td>
							    					</tr>
							    					<tr>
							    						<th><icrop:CustomLabel ID="ItemPhoneNumber" runat="server" Text="" TextWordNo="18" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td><icrop:CustomLabel ID="DetailsPhoneNumber" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
							    					</tr>
							    					<tr>
							    						<th class="ListEnd"><icrop:CustomLabel ID="ItemMobileNumber" runat="server" Text="" TextWordNo="19" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td class="ListEnd"><icrop:CustomLabel ID="DetailsMobileNumber" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
							    					</tr>
							    				</table>
							    				<table border="0" cellspacing="0" cellpadding="0" class="ListSet">
							    					<tr>
							    						<th><icrop:CustomLabel ID="ItemServiceContents" runat="server" Text="" TextWordNo="21" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td><icrop:CustomLabel ID="DetailsServiceContents" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
							    					</tr>
							    					<tr>
							    						<th class="ListEnd"><icrop:CustomLabel ID="ItemWaitPlan" runat="server" Text="" TextWordNo="43" CssClass="Ellipsis" Width="114"></icrop:CustomLabel></th>
							    						<td class="ListEnd"><icrop:CustomLabel ID="DetailsWaitPlan" runat="server" Text="" CssClass="Ellipsis" Width="201"></icrop:CustomLabel></td>
							    					</tr>
							    				</table>
							    				<table border="0" cellspacing="0" cellpadding="0" class="ListSet" id="DrawerTable" runat="server" style="display:none;">
							    					<tr>
							    						<th class="ListEnd"><icrop:CustomLabel ID="ItemDrawer" runat="server" Text="" TextWordNo="80" CssClass="Ellipsis" Width="114" /></th>
							    						<td class="ListEnd"><icrop:CustomLabel ID="DetailsDrawer" runat="server" Text="" CssClass="Ellipsis" Width="201" /></td>
							    					</tr>
							    				</table>
                                                <%--受付モニター無し用の呼出場所エリア--%>
                                                <table border="0" cellspacing="0" cellpadding="0" class="ListSet" id="CallPlaceTable" runat="server" style="display:none;">
							    					<tr>
							    						<th class="ListEnd"><icrop:CustomLabel ID="ItemCallPlace02" runat="server" Text="" TextWordNo="97" CssClass="Ellipsis" Width="114" /></th>
							    						<td class="ListEnd"><icrop:CustomLabel ID="DetailsCallPlace02" runat="server" Text="" CssClass="Ellipsis" Width="201" /></td>
							    					</tr>
							    				</table>
							    			</div>
							    			<div class="DetailFooterBox">
							    				<%--画面遷移ボタン --%>
							    				<asp:Button ID="DetailButtonLeft" runat="server" Text="" CssClass="FooterButton01" OnClientClick="return DetailCustomerButton();" />
							    				<%--<asp:Button ID="DetailButtonCenter" runat="server" Text="" CssClass="FooterButton02" OnClientClick="return  DetailOrderButton();" />--%>
							    				<%--<asp:Button ID="DetailButtonRight" runat="server" Text="" CssClass="FooterButton03" OnClientClick="return DetailApprovalButton();" />--%>
                                                <asp:Button ID="DetailButtonRight" runat="server" Text="" CssClass="FooterButton02" OnClientClick="return DetailOrderButton();" />
                                                <%--完成検査承認アイコン --%>
                                                <%--<asp:Button ID="DetailButtonInspection" runat="server" Text="" CssClass="FooterButton04" OnClientClick="return DetailInspectionButton();" style="display: none" />--%>
							    				<%--画面遷移ボタン押下時の2度押し防止用ダミーボタン --%>
							    				<asp:Button ID="DetailButtonLeftDummy" runat="server" Text="" style="display: none" />
							    				<%--<asp:Button ID="DetailButtonCenterDummy" runat="server" Text="" style="display: none" />--%>
							    				<asp:Button ID="DetailButtonRightDummy" runat="server" Text="" style="display: none" /> 
                                                <asp:Button ID="CallButton" runat="server" style="display:none" />
                                                <asp:Button ID="CallCancelButton" runat="server" style="display:none" />
                                                <asp:Button ID="CallPlaceChangeButton" runat="server" style="display:none" />
                                                <icrop:CustomLabel ID="DetailsVisitUpdateDateLabel" runat="server" Text="" style="display:none"></icrop:CustomLabel>                                                
							    			</div>
							    			<div class="DetailDeleteFooterBox" style="display:none;">
							    	            <%--<asp:Button ID="DetailButtonDelete" runat="server" Text="" CssClass="BottomDeleteButton" OnClientClick="return DetailDeleteButton();"/>--%>
                                                <div id="DetailButtonDeleteDiv" class="BottomDeleteButton" onclick="return DetailDeleteButton();">
                                                    <icrop:CustomLabel ID="ButtonDeleteWord01" CssClass="ButtonDeleteWord Ellipsis" runat="server" Text="" DelBtnStatus="1" TextWordNo="107" visible="False" />
                                                    <icrop:CustomLabel ID="ButtonDeleteWord02" CssClass="ButtonDeleteWord Ellipsis" runat="server" Text="" DelBtnStatus="2" TextWordNo="82" visible="False" />
                                                </div>
							    			</div>
							    			<%--チップをタップ時に詳細ポップアップウィンドウを表示情報取得--%>
							    			<asp:Button ID="DetailPopupButton" runat="server" style="display:none" />
                                        </div>
							    	</div>
							    </div>
                            </div>
							<div id="DetailBottomBox" runat="server" class="DetailBottomBox">
								<%--<asp:Button ID="DetailbottomButton" runat="server" Text="" CssClass="BottomButton" OnClientClick="return ButtonControl('#DetailbottomButton');"/>--%>
                                <div id="DetailbottomDiv" class="BottomButton" onclick="return ButtonControl('#DetailbottomButton');" runat="server" >
                                    <icrop:CustomLabel ID="DetailbottomButton" CssClass="BottomButtonLabel Ellipsis" runat="server" BtnStatus="3" Text="" />
                                </div>
                                <div id="DetailbottomDiv02" class="BottomButton02" onclick="return ButtonControl('#DetailbottomButton02');" runat="server" visible="False">
                                    <icrop:CustomLabel ID="DetailbottomButton02" CssClass="BottomButtonLabel02 Ellipsis" runat="server" BtnStatus="1" Text=""/>
                                </div>
                                <div id="DetailbottomDiv03" class="BottomButton03" onclick="return ButtonControl('#DetailbottomButton03');" runat="server" visible="False">
                                    <icrop:CustomLabel ID="DetailbottomButton03" CssClass="BottomButtonLabel03 Ellipsis" runat="server" BtnStatus="2" Text=""/>
                                </div>
                                <div id="DetailbottomDiv04" class="BottomButton05" onclick="return ButtonControl('#DetailbottomButton04');" runat="server" visible="False">
                                    <icrop:CustomLabel ID="DetailbottomButton04" CssClass="BottomButtonLabel06 Ellipsis" runat="server" BtnStatus="4" Text=""/>
                                </div>
								<%--チップ詳細ボタン押下時の共通遷移イベント発生ボタン --%>
								<asp:Button ID="DetailNextScreenCommonButton" runat="server" style="display:none" />
								<%--チップ詳細ボタン押下時の押下されたボタンステータス格納用--%>
								<asp:HiddenField ID="DetailClickButtonStatus" runat="server" />
                                <%--チップ詳細2ボタン標準ボタン押下時の押下されたボタンステータス格納用--%>
								<asp:HiddenField ID="DetailClickButtonCheck" runat="server" />
                                <%--入庫テーブルの行ロックバージョン--%>
								<asp:HiddenField ID="DetailRowLockVersion" runat="server" />
								<%--退店ボタンのタップ時の文言--%>
			                    <icrop:CustomLabel ID="HiddenDeleteConfirmWord" style="display:none;" runat="server" TextWordNo="108"></icrop:CustomLabel>
                                <%--振当解除ボタンのタップ時の文言--%>
			                    <icrop:CustomLabel ID="HiddenDeleteConfirmWord02" style="display:none;" runat="server" TextWordNo="83"></icrop:CustomLabel>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
			        <%--受付チップ削除ダミーボタン--%>
			        <asp:Button ID="DetailButtonDeleteDummy" runat="server" Text="" style="display: none" />
                    <%--完成検査承認ダミーボタン--%>
                    <asp:Button ID="DetailButtonInspectionDummy" runat="server" Text="" style="display: none" />
				</div>
				<div id="search" class="content">
					<div id="headerSearchType">
						<div class="SelectionButton">
							<ul>
								<li onclick="return SelectSearchType(this);" id="Selection1"><icrop:CustomLabel ID="SelectRegNo" runat="server" TextWordNo="62" CssClass="Ellipsis" UseEllipsis="False"  Width="68"></icrop:CustomLabel></li>
								<li onclick="return SelectSearchType(this);" id="Selection2"><icrop:CustomLabel ID="SelectVin" runat="server" TextWordNo="63" CssClass="Ellipsis" UseEllipsis="False"  Width="68"></icrop:CustomLabel></li>
								<li onclick="return SelectSearchType(this);" id="Selection3"><icrop:CustomLabel ID="SelectName" runat="server" TextWordNo="64" CssClass="Ellipsis" UseEllipsis="False"  Width="68"></icrop:CustomLabel></li>
								<li onclick="return SelectSearchType(this);" id="Selection4"><icrop:CustomLabel ID="SelectTelNo" runat="server" TextWordNo="65" CssClass="Ellipsis" UseEllipsis="False"  Width="68"></icrop:CustomLabel></li>
							</ul>
						</div>
						<div style="display:none;">
							<input id="SearchFocusInDummyButton" onclick="FocusInSearchTextBox();"/>
						</div>
						<div class="SearchBox">
							<div class="SearchArea" id="SearchArea">
								<div class="SearchButton" onclick="return SearchCustomer();"></div>
								<input name="TextArea" class="TextArea" id="SearchText" placeholder=" " type="search" />
								<%-- 検索PlaceHold用 --%>
								<icrop:CustomLabel ID="SearchPlaceRegNo" runat="server" TextWordNo="66" style="display:none"></icrop:CustomLabel>
								<icrop:CustomLabel ID="SearchPlaceVin" runat="server" TextWordNo="67" style="display:none"></icrop:CustomLabel>
								<icrop:CustomLabel ID="SearchPlaceName" runat="server" TextWordNo="68" style="display:none"></icrop:CustomLabel>
								<icrop:CustomLabel ID="SearchPlacePhone" runat="server" TextWordNo="69" style="display:none"></icrop:CustomLabel>
								<div class="ClearButton" onclick="return TextClear();"></div>
							</div>
						</div>

					</div>
					<div class="SearchDataBox">
						<div class="SearchDataBoxInner" runat="server" ID="SearchDataBoxInner">
							<asp:UpdatePanel ID="SearchDataUpdate" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<ul id="SearchListBox">
										<li class="FrontLink" ID="FrontLink" runat="server" onclick="SearchFrontList();">
											<div class="FrontList" ID="FrontList" runat="server"></div>
											<span class="FrontSearchingImage"></span>
											<span class="FrontListSearching" ID="FrontListSearching" runat="server"></span>
										</li>
										<%-- 顧客検索結果表示 --%>
										<asp:Repeater ID="SearchRepeater" runat="server">
											<ItemTemplate>
												<li>
													<div class="SearchData">
														<div ID="SearchPhotoArea" runat="server">
                                                        <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START --%>
                                                            <%-- <img src="" runat="server" id="SearchPhotoImage" height="58" alt="" /> --%>
															<img src="" runat="server" id="SearchPhotoImage" height="60" width="60" alt="" />
                                                        <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END --%>
														</div>
														<div ID="SearchRegistrationNumber" runat="server" class="Ellipsis" style="width:140px"></div>
														<div ID="SearchVinNo" runat="server" class="Ellipsis" style="width:155px"></div>
														<div ID="SearchCustomerName" runat="server" class="Ellipsis" style="width:300px"></div>
														<div ID="SearchModel" runat="server" class="Ellipsis" style="width:300px"></div>
														<div ID="SearchPhone" runat="server" class="Ellipsis" style="width:125px"></div>
														<div ID="SearchMobile" runat="server" class="Ellipsis" style="width:125px"></div>
														<%-- 顧客検索付替え用パラメータ --%>
                                                        <div id="CustomerChangeParameter" runat="server"></div>
													</div>
												</li>
											</ItemTemplate>
										</asp:Repeater>
										<li class="NextLink" id="NextLink" runat="server" onclick="SearchNextList();">
											<div class="NextList" ID="NextList" runat="server"></div>
											<span class="NextSearchingImage"></span>
											<span class="NextListSearching" ID="NextListSearching" runat="server"></span>
										</li>
									</ul>
									<div class="NoSearchImage" ID="NoSearchImage" runat="server"></div>
									<%-- 顧客検索結果格納 --%>
									<asp:Button ID="SearchCustomerDummyButton" runat="server" style="display:none" />
									<asp:HiddenField ID="SearchStartRowHidden" runat="server" />
									<asp:HiddenField ID="SearchEndRowHidden" runat="server" />
                                    <asp:HiddenField ID="SearchCustomerAllCountHidden" runat="server" />
									<asp:HiddenField ID="ScrollPositionHidden" runat="server" />
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</div>
					<div id="SearchDataLoading" class="loadingPopup" runat="server"></div>
					<div id="SearchBottomBox" class="SearchBottomBox">
						<%--<input type="button" id="SearchBottomButton" runat="server" class="BottomButton02 BottomButtonDisable" onclick="CustomerClear();"/>--%>                      
                        <div id="SearchBottomDiv" class="BottomButton" onclick="CustomerClear();">
                            <icrop:CustomLabel ID="SearchBottomButton" CssClass="BottomButtonLabel BottomButtonDisable Ellipsis" runat="server" Text="" TextWordNo="70"/>
                        </div>
                        <div id="SearchBottomDiv02" class="BottomButton04" style="display:none" >
                            <icrop:CustomLabel ID="CreateCustomerButton" CssClass="BottomButtonLabel04 Ellipsis" runat="server" Text="" />
                            <icrop:CustomLabel ID="CreateCustomerButton02" CssClass="BottomButtonLabel05 Ellipsis" runat="server" Text="" />
                        </div>
                        <%--<input type="button" id="CreateCustomerButton" runat="server" class="BottomButton03" onclick="CustomerClear();"/>--%>
					</div>
				</div>
			</div>
		</div>
		<asp:UpdatePanel ID="CustomerSetButtonUpdatePanel" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<asp:Button ID="BeforeChipChangesDummyButton" runat="server" style="display:none" />
				<asp:Button ID="ChipChangesDummyButton" runat="server" style="display:none" />
				<asp:Button ID="ChipClearDummyButton" runat="server" style="display:none" />
				<%-- 顧客検索付替えinputパラメータ --%>
				<asp:HiddenField ID="SearchRegistrationNumberChange" runat="server"/>
				<asp:HiddenField ID="SearchCustomerCodeChange" runat="server" />
				<asp:HiddenField ID="SearchDMSIdChange" runat="server" />
				<asp:HiddenField ID="SearchVinChange" runat="server" />
				<asp:HiddenField ID="SearchModelChange" runat="server" />
				<asp:HiddenField ID="SearchCustomerNameChange" runat="server"/>
				<asp:HiddenField ID="SearchPhoneChange" runat="server" />
				<asp:HiddenField ID="SearchMobileChange" runat="server" />
				<asp:HiddenField ID="SearchSACodeChange" runat="server" />
				<asp:HiddenField ID="ChipReserveNumberBefore" runat="server" />
				<asp:HiddenField ID="ChipOrderNumberBefore" runat="server" />
				<asp:HiddenField ID="ChipResultChange" runat="server" />
				<asp:HiddenField ID="ChipVisitNumberChange" runat="server" />
				<asp:HiddenField ID="ChipReserveNumberChange" runat="server" />
				<asp:HiddenField ID="ChipSACodeChange" runat="server" />
				<asp:HiddenField ID="ChipOrderNumberChange" runat="server" />
				<asp:HiddenField ID="ChipConfirmChange" runat="server" />

                <%-- 2013/06/03 TMEJ 河原【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 START --%>
                <asp:HiddenField ID="ChipVehicleIdAfter" runat="server" />
                <%-- 2013/06/03 TMEJ 河原【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 END --%>

			</ContentTemplate>
		</asp:UpdatePanel>
        <%-- ポップアップの三角アイコン --%>
        <div class="PoPuPArrowLeft1-1">
	　      <div class="PoPuPArrowLeft1-2">
	　   	    <div class="PoPuPArrowLeft1-3"></div>
	        </div>
	    </div>
    </div>

	<%-- 顧客検索inputパラメータ --%>
	<asp:HiddenField ID="SearchRegistrationNumberHidden" runat="server"/>
	<asp:HiddenField ID="SearchVinHidden" runat="server" />
	<asp:HiddenField ID="SearchCustomerNameHidden" runat="server" />
	<%--<asp:HiddenField ID="SearchPhoneNumberHidden" runat="server" />--%>
    <asp:HiddenField ID="SearchAppointNumberHidden" runat="server" />
	<asp:HiddenField ID="SearchSelectTypeHidden" runat="server" />
    <asp:HiddenField ID="SearchTypeIndexHidden" runat="server" />    

    <%-- 選択されたチップ詳細情報格納 --%>
    <asp:HiddenField ID="DetailsVisitNo" runat="server" />
    <asp:HiddenField ID="DetailsArea" runat="server" />
    <asp:HiddenField ID="DetailsOrderNo" runat="server" />
    <asp:HiddenField ID="DetailsApprovalId" runat="server" />
	<asp:HiddenField ID="DetailsRezId" runat="server" />
    <asp:HiddenField ID="DetailsCallStatus" runat="server" />
    <asp:HiddenField ID="DetailsVisitUpdateDate" runat="server" />

    <%-- 選択されたチップ詳細呼出場所退避 --%>
    <asp:HiddenField ID="BakCallPlace" runat="server" />

    <%-- 予期せぬエラーメッセージ --%>
    <asp:HiddenField ID="UnanticipatedMessageField" runat="server" />

    <%-- 工程管理ボックス --%>
    <div id="contentsRightBox1">
        <asp:UpdatePanel ID="ContentUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- 通知リフレッシュ時に工程管理ボックスは触れないように --%>
            <div id="contentsRightBox_LoadingScreen">
            </div>
            <div class="WhatNewDisableDiv">
            </div>
            <%-- 受付エリア --%>
            <%--<div class="ColumnBox01">--%>
                <%--通知ポーリング処理--%>
                <%--<h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordReception" runat="server" CssClass="Ellipsis" Width="100" Text="" TextWordNo="2"></icrop:CustomLabel>
                </h2>--%>
                <%-- 受付状態のチップ数 --%>
                <%--<div class="contentTitleNo">--%>
                  <%-- 通知リフレッシュボタン(隠しボタン) --%>
                    <%--<asp:Button ID="MainPolling" runat="server" CssClass="MainRefreshStyle" />
                    <icrop:CustomLabel ID="ReceptionDeskTipNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable1" class="ColumnContentsFlame">
                    <ul>--%>
                        <%-- 受付情報の表示 --%>
                        <%--<asp:Repeater ID="ReceptionRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipRight" id='Reception'>--%>

                                        <%-- チップエリア --%>
                                        <%--<div id="ReceptionDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">--%>
                                                <%-- チップ上段(マーク) --%>
                                                <%--<div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>--%>
                                                <%-- チップ下段(詳細情報) --%>
                                                <%--<div class="ColumnTextBox">
                                                    <div ID="RegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="CustomerName" runat="server" class="Ellipsis" style="width:130px"></div>--%>
                                                    <%--<div ID="VisitTime" runat="server" class="Ellipsis" style="width:70px"></div>--%>
                                                    <%--<div ID="RepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="ParkingNumber" runat="server" class="EllipsisText" style="width:60px"></div>
                                                    </div>
                                                    <div id="ColumnCount" class="ColumnCount"></div>
                                                    <div ID="ElapsedTime" runat="server"></div>
                                                </div>
                                            </div>--%>
                                        <%-- チップエリア終了 --%>
                                        <%--</div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                        
                </div>
            </div>--%>

            <%-- 追加承認エリア --%>
            <%--<div class="ColumnBox02">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WorkApproval" runat="server" Text="" TextWordNo="4" CssClass="Ellipsis" Width="100"></icrop:CustomLabel>
                </h2>--%>
                <%-- 追加承認中のチップ数 --%>
                <%--<div class="contentTitleNo">
                    <icrop:CustomLabel ID="ApprovalNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable2" class="ColumnContentsFlame">
                    <ul>--%>
                        <%-- 追加承認情報の表示 --%>
                        <%--<asp:Repeater ID="ApprovalRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipRight" id='Approval'>--%>
                                        <%-- チップエリア --%>
                                       <%-- <div id="ApprovalDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">--%>
                                                <%-- チップ上段(マーク) --%>
                                                <%--<div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>--%>
                                                <%-- チップ下段(詳細情報) --%>
                                                <%--<div class="ColumnTextBox">
                                                    <div ID="ApprovalRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="ApprovalCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>--%>
                                                    <%--もともとコメント<div ID="ApprovalDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>--%>
                                                    <%--<div ID="ApprovalRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="ApprovalChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                    </div>
                                                    <div id="ColumnTime" class="ColumnTime">
                                                        <div ID="ApprovalDeliveryPlanTime" runat="server" text=""></div>
                                                    </div>
                                                    <div id="ColumnCount" class="ColumnCount"></div>
                                                    <div id="ApprovalElapsedTime" runat="server" ></div>
                                                </div>
                                            </div>--%>
                                        <%-- チップエリア終了 --%>
                                        <%--</div>
                                    </div>
                                <li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                       
                </div>
            </div>--%>

            <%-- 納車準備エリア --%>
            <%--<div class="ColumnBox03">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordPreparation" runat="server" Text="" TextWordNo="5" CssClass="Ellipsis" Width="100"></icrop:CustomLabel>
                </h2>--%>
                <%-- 納車準備エリアのチップ数 --%>
                <%--<div class="contentTitleNo">
                    <icrop:CustomLabel ID="PreparationNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable3" class="ColumnContentsFlame">
                    <ul>--%>
                        <%-- 納車準備情報の表示 --%>
                        <%--<asp:Repeater ID="PreparationRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipLeft" id='Preparation'>--%>
                                        <%-- チップエリア --%>
                                        <%--<div id="PreparationDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">--%>
                                                <%-- チップ上段(マーク) --%>
                                                <%--<div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>--%>
                                                <%-- チップ下段(詳細情報) --%>
                                                <%--<div class="ColumnTextBox">
                                                    <div ID="PreparationRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="PreparationCustomerName" runat="server" class="Ellipsis" style="width:110px"></div>--%>
                                                    <%--もともとコメント<div ID="PreparationDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>--%>
                                                    <%--<div ID="PreparationRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="PreparationChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                    </div>
                                                    <div id="ColumnTime" class="ColumnTime">
                                                        <div ID="PreparationDeliveryPlanTime" runat="server" text=""></div>
                                                    </div>
                                                    <div id="ColumnCount" class="ColumnCount"></div>
                                                    <div id="PreparationElapsedTime" runat="server"></div>
                                                    <div id="WorkIcon" runat="server" class=""></div>
                                                </div>
                                            </div>--%>
                                        <%-- チップエリア終了 --%>
                                        <%--</div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                        
                </div>
            </div>--%>

            <%-- 納車作業エリア --%>
            <%--<div class="ColumnBox04">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordDelivery" runat="server" Text="" CssClass="Ellipsis" Width="100" TextWordNo="6"></icrop:CustomLabel>
                </h2>--%>
                <%-- 納車作業エリアのチップ数 --%>
                <%--<div class="contentTitleNo">
                    <icrop:CustomLabel ID="DeliveryNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable4" class="ColumnContentsFlame">
                    <ul>--%>
                        <%-- 納車作業情報の表示 --%>
                        <%--<asp:Repeater ID="DeliveryRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipLeft" id='Delivery'>--%>
                                        <%-- チップエリア --%>
                                        <%--<div id="DeliveryDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">--%>
                                                <%-- チップ上段(マーク) --%>
                                                <%--<div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>--%>
                                                <%-- チップ下段(詳細情報) --%>
                                                <%--<div class="ColumnTextBox">
                                                    <div ID="DeliveryRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="DeliveryCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>--%>
                                                    <%--もともとコメント<div ID="DeliveryDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>--%>
                                                    <%--<div ID="DeliveryRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="DeliveryChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                    </div>
                                                    <div id="ColumnTime" class="ColumnTime">
                                                        <div ID="DeliveryDeliveryPlanTime" runat="server" text=""></div>
                                                    </div>
                                                    <div id="ColumnCount" class="ColumnCount"></div>
                                                    <div id="DeliveryElapsedTime" runat="server" ></div>
                                                    <div id="WorkIcon" runat="server" class=""></div>
                                                </div>
                                            </div>--%>
                                        <%-- チップエリア終了 --%>
                                        <%--</div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                       
                </div>
            </div>--%>

            <%-- 作業中エリア --%>
            <%--<div class="ColumnSide">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordWork" runat="server" Text="" TextWordNo="3" CssClass="Ellipsis" Width="500"></icrop:CustomLabel>
                </h2>--%>
                <%-- 作業中エリアの表示チップ数 --%>
               <%-- <div class="contentTitleNo">
                    <icrop:CustomLabel ID="WorkNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable5"  class="ColumnSideFrame">
                    <div class="ColumnSideBoderIn">
                        <table border="0" cellspacing="0" cellpadding="0" class="">
                            <tr>--%>
                                <%--作業中エリアの表示 --%>
                                <%--<asp:Repeater ID="WorkRepeater" runat="server">
                                    <ItemTemplate>
                                        <td>
                                            <div id='Work' class="CustomerChipTop">--%>
                                                <%-- チップエリア --%>
                                                <%--<div id="Working" runat="server" class="" visible="true">
                                                    <div class="ColumnContents02BoderIn">--%>
                                                        <%-- チップ詳細情報 --%>
                                                        <%--<div class="WorkIcnSet">
                                                            <div ID="WorkRightIcnD" runat="server" text="" visible="False" class="WorkRightIcnD"></div>
                                                            <div ID="WorkRightIcnI" runat="server" text="" visible="False" class="WorkRightIcnI"></div>
                                                            <div ID="WorkRightIcnS" runat="server" text="" visible="False" class="WorkRightIcnS"></div>
                                                        </div>
                                                        <div id="ColumnCount" class="ColumnWatch"></div>
                                                        <div id="ColumnTime" class="ColumnTime">
                                                            <div ID="WorkDeliveryPlanTime" runat="server" text=""></div>
                                                        </div>
                                                        <div id="WorkElapsedTime" runat="server" ></div>--%>
                                                        <%--もともとコメント<div class="ColumnTimeGray">
                                                            <div ID="WorkTimeLag" runat="server" text=""></div>
                                                        </div>--%>

                                                        <%--<div id="WorkTextBox" runat="server" class="" visible="true">--%>
                                                        <%--<div id="WorkTextBox" runat="server" class="ColumnTextBox" visible="true">
                                                            <div ID="WorkRegistrationNumber" runat="server" text="" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="WorkCustomerName" runat="server" text=""  class="Ellipsis" style="width:75px"></div>
                                                            <div ID="WorkCompletionPlanTime" runat="server" text=""  class="Ellipsis" style="width:75px"></div>
                                                            <div ID="WorkRepresentativeWarehousing" runat="server" text="" class="Ellipsis" style="width:55px"></div>
                                                            <div class="IcnNo"  visible="False">
                                                                <div ID="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                            </div>
                                                            <div id="WorkIcon" runat="server" class=""></div>
                                                        </div>
                                                    </div>--%>
                                                <%-- チップエリア終了 --%>
                                                <%--</div>
                                            </div>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>--%>            
            <div id="Div1">
	            <div class="ColumnBox01">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WordAssignment" runat="server" Text="" TextWordNo="103" CssClass="Ellipsis" Width="112"></icrop:CustomLabel>
                    </h2>
                    <asp:UpdatePanel ID="AssignmentUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%-- 振当待ち状態のチップ数 --%>
                            <div class="contentTitleNo">
                                <icrop:CustomLabel ID="AssignmentNumber" runat="server" Text=""></icrop:CustomLabel>
                            </div>
                            <div class="AssignmentLoadingDiv" style="display:none" >
                                <div class="AssignmentLoadingImage"></div>
                            </div>
                            <%-- 振当てエリア限定通知リフレッシュボタン(隠しボタン) --%>
                            <asp:Button ID="AssignmentRefreshButton" runat="server"  style="display:none" />
	                        <div class="ColumnContentsFlame">
	                            <div class="ColumnContentsFlameIn" id='AssignmentArea'>
	                                <ul>
                                        <%-- 振当待ち情報の表示 --%>
                                        <asp:Repeater ID="AssignmentRepeater" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="CustomerChipRight" id='Assignment'>
                                                    <div class="TipBlackOut"></div>
                                                        <%-- チップエリア --%>
                                                        <div id="AssignmentDeskDevice" runat="server" class="" visible="true">
                                                            <div class="ColumnContentsBoderIn">
                                                                <%-- チップ上段(マーク) --%>
                                                                <div class="IcnSet">
                                                                        <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                                        <div ID="RightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                                        <div ID="RightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                                        <div ID="RightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                                        <div ID="RightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                                        <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                                        <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                                        <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                                        <%-- <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                                        <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                                        <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                                        <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                                        <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                                </div>
                                                                <%-- チップ下段(詳細情報) --%>
                                                                <div class="ColumnTextBox">
                                                                    <div ID="RegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                                    <div ID="CustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                                    <div ID="VisitTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                                    <div ID="RepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                                    <div class="ColumnNo">
                                                                        <div ID="ParkingNumber" runat="server" class="EllipsisText" style="width:60px"></div>
                                                                    </div>
                                                                    <div id="ColumnCount" class="ColumnCount"></div>
                                                                    <div ID="ElapsedTime" runat="server"></div>
                                                                </div>
                                                            </div>
                                                        <%-- チップエリア終了 --%>
                                                        </div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <div class="AdjustmentDiv"></div>
	                            </div>
                            </div>                            
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
	            <div class="ColumnBox02">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WordReception" runat="server" CssClass="Ellipsis" Width="112" Text="" TextWordNo="2"></icrop:CustomLabel>
                    </h2>
	                <%-- 受付状態のチップ数 --%>
                    <div class="contentTitleNo">
                        <%-- 通知リフレッシュボタン(隠しボタン) --%>
                        <asp:Button ID="MainPolling" runat="server" CssClass="MainRefreshStyle" />
                        <icrop:CustomLabel ID="ReceptionDeskTipNumber" runat="server" Text=""></icrop:CustomLabel>
                    </div>
	                <div class="ColumnContentsFlame">
	                    <div class="ColumnContentsFlameIn" id='ReceptionArea'>
	                        <ul>
                                <%-- 受付情報の表示 --%>
                                <asp:Repeater ID="ReceptionRepeater" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="CustomerChipRight" id='Reception'>
                                            <div class="TipBlackOut"></div>
                                                <%-- チップエリア --%>
                                                <div id="ReceptionDeskDevice" runat="server" class="" visible="true">
                                                    <div class="ColumnContentsBoderIn">
                                                        <%-- チップ上段(マーク) --%>
                                                        <div class="IcnSet">
                                                                <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                                <div ID="RightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                                <div ID="RightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                                <div ID="RightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                                <div ID="RightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                                <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                                <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                                <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                                <%-- <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                                <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                                <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                                <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                                <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                        </div>
                                                        <%-- チップ下段(詳細情報) --%>
                                                        <div class="ColumnTextBox">
                                                            <div ID="RegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="CustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                            <div ID="VisitTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="RepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div class="ColumnNo">
                                                                <div ID="ParkingNumber" runat="server" class="EllipsisText" style="width:60px"></div>
                                                            </div>
                                                            <div id="ColumnCount" class="ColumnCount"></div>
                                                            <div ID="ElapsedTime" runat="server"></div>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="AdjustmentDiv"></div>
	                    </div>
                    </div>
                </div>
	            <div class="ColumnBox03">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WorkApproval" runat="server" Text="" TextWordNo="4" CssClass="Ellipsis" Width="112"></icrop:CustomLabel>
                    </h2>
	                <div class="contentTitleNo">
                        <icrop:CustomLabel ID="ApprovalNumber" runat="server" Text=""></icrop:CustomLabel>
                    </div>
	                <div class="ColumnContentsFlame">
	                    <div class="ColumnContentsFlameIn" id='ApprovalArea'>
	                        <ul>
                            <%-- 追加承認情報の表示 --%>
                                <asp:Repeater ID="ApprovalRepeater" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="CustomerChipRight" id='Approval'>
                                            <div class="TipBlackOut"></div>
                                                <%-- チップエリア --%>
                                                <div id="ApprovalDeskDevice" runat="server" class="" visible="true">
                                                    <div class="ColumnContentsBoderIn">
                                                        <%-- チップ上段(マーク) --%>
                                                        <div class="IcnSet">
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <div ID="RightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                            <div ID="RightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                            <div ID="RightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                            <div ID="RightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <%-- <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                            <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                            <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                        </div>
                                                        <%-- チップ下段(詳細情報) --%>
                                                        <div class="ColumnTextBox">
                                                            <div ID="ApprovalRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="ApprovalCustomerName" runat="server" class="Ellipsis" style="width:70px"></div>                                                   
                                                            <div ID="ApprovalRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div class="ColumnNo">
                                                                <div ID="ApprovalChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                            </div>
                                                            <div id="ColumnTime" class="ColumnTime">
                                                                <div ID="ApprovalDeliveryPlanTime" runat="server" text=""></div>
                                                            </div>
                                                            <div id="ColumnCount" class="ColumnCount"></div>
                                                            <div id="ApprovalElapsedTime" runat="server" ></div>
                                                            <div class="ColumnIconSet">
	                                                            <div class="ColumnIcon01" id="ApprovalAdditionalIcon" runat="server" >
                                                                    <div ID="AdditionalWorkNumber" class="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                                </div>
	                                                        </div>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        <li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            <div class="AdjustmentDiv"></div>
                        </div>
                    </div>
                </div>
	            <div class="ColumnBox04">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WordWork" runat="server" Text="" TextWordNo="3" CssClass="Ellipsis" Width="112"></icrop:CustomLabel>
                    </h2>
	                <div class="contentTitleNo">
                        <icrop:CustomLabel ID="WorkNumber" runat="server" Text=""></icrop:CustomLabel>
                    </div>
	                <div class="ColumnContentsFlame">
	                    <div class="ColumnContentsFlameIn" id='WorkArea'>
                            <ul>
                                <%-- 作業中エリアの表示 --%>
                                <asp:Repeater ID="WorkRepeater" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="CustomerChipRight" id="Work">
                                            <div class="TipBlackOut"></div>
                                                <%-- チップエリア --%>
                                                <div id="WorkDeskDevice" runat="server" class="" visible="true">
                                                    <div id="Working" runat="server" class="" visible="true"></div>
                                                    <div class="ColumnContentsBoderIn">
                                                        <%-- チップ上段(マーク) --%>
                                                        <div class="IcnSet">
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <div ID="WorkRightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                            <div ID="WorkRightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                            <div ID="WorkRightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                            <div ID="WorkRightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="WorkRightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <%-- <div ID="WorkRightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                            <div ID="WorkRightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                            <div ID="WorkRightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="WorkRightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                        </div>
                                                        <div id="ColumnCount" class="ColumnWatch"></div>
                                                        <div id="ColumnTime" class="ColumnTime">
                                                            <div ID="WorkDeliveryPlanTime" runat="server" text=""></div>
                                                        </div>
                                                        <div id="WorkElapsedTime" runat="server" ></div>
                                                        <%-- チップ下段(詳細情報) --%>
                                                        <div class="ColumnTextBox">
                                                            <div ID="WorkRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="WorkCustomerName" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="WorkCompletionPlanTime" runat="server" text=""  class="Ellipsis" style="width:70px"></div>                                             
                                                            <div ID="WorkRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <%--<div class="IcnNo"  visible="False">
                                                                <div ID="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                            </div>--%>
                                                            <div class="ColumnIconSet">
	                                                            <div class="ColumnIcon01" id="WorkAdditionalIcon" runat="server" visible="false">
                                                                    <div ID="AdditionalWorkNumber" class="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                                </div>
	                                                         </div>
                                                            <%--<div id="WorkIcon" runat="server" class=""></div>--%>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>	
                            <div class="AdjustmentDiv"></div>                        
                        </div>
                    </div>
                </div>
	            <div class="ColumnBox05">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WordPreparation" runat="server" Text="" TextWordNo="5" CssClass="Ellipsis" Width="112"></icrop:CustomLabel>
                    </h2>
	                <div class="contentTitleNo">
                        <icrop:CustomLabel ID="PreparationNumber" runat="server" Text=""></icrop:CustomLabel>
                    </div>
	                <div class="ColumnContentsFlame">
	                    <div class="ColumnContentsFlameIn" id='PreparationArea'>
	                        <ul>
                                <%-- 納車準備情報の表示 --%>
                                <asp:Repeater ID="PreparationRepeater" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="CustomerChipRight" id='Preparation' ;>
                                            <div class="TipBlackOut"></div>
                                                <%-- チップエリア --%>
                                                <div id="PreparationDeskDevice" runat="server" class="" visible="true">
                                                    <div class="ColumnContentsBoderIn">
                                                        <%-- チップ上段(マーク) --%>
                                                        <div class="IcnSet">
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <div ID="RightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                            <div ID="RightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                            <div ID="RightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                            <div ID="RightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <%-- <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                            <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                            <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                        </div>
                                                        <%-- チップ下段(詳細情報) --%>
                                                        <div class="ColumnTextBox">
                                                            <div ID="PreparationRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="PreparationCustomerName" runat="server" class="Ellipsis" style="width:70px"></div>                                                           
                                                            <div ID="PreparationRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div class="ColumnNo">
                                                                <div ID="PreparationChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                            </div>
                                                            <div id="ColumnTime" class="ColumnTime">
                                                                <div ID="PreparationDeliveryPlanTime" runat="server" text=""></div>
                                                            </div>
                                                            <div id="ColumnCount" class="ColumnCount"></div>
                                                            <div id="PreparationElapsedTime" runat="server"></div>
                                                            <div class="ColumnIconSet">
	                                                            <div class="ColumnIcon01" id="PreparationAdditionalIcon" runat="server" visible="false">
                                                                    <div ID="AdditionalWorkNumber" class="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                                </div>
                                                                <div class="ColumnIcon02" id="InVoiceIcon" runat="server" visible="false"></div>
	                                                            <div class="ColumnIcon03" id="WashIcon" runat="server" visible="false"></div>
	                                                         </div>
                                                            <%--<div id="WorkIcon" runat="server" class=""></div>--%>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="AdjustmentDiv"></div>
                        </div>
                    </div>
                </div>
	            <div class="ColumnBox06">
	                <h2 class="contentTitle">
                        <icrop:CustomLabel ID="WordDelivery" runat="server" Text="" CssClass="Ellipsis" Width="112" TextWordNo="6"></icrop:CustomLabel>
                    </h2>
	                <div class="contentTitleNo">
                        <icrop:CustomLabel ID="DeliveryNumber" runat="server" Text=""></icrop:CustomLabel>
                    </div>
	                <div class="ColumnContentsFlame">
	                    <div class="ColumnContentsFlameIn" id='DeliveryArea'>
	                        <ul>
                                <%-- 納車作業情報の表示 --%>
                                <asp:Repeater ID="DeliveryRepeater" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="CustomerChipRight" id='Delivery'>
                                            <div class="TipBlackOut"></div>
                                                <%-- チップエリア --%>
                                                <div id="DeliveryDeskDevice" runat="server" class="" visible="true">
                                                    <div class="ColumnContentsBoderIn">
                                                        <%-- チップ上段(マーク) --%>
                                                        <div class="IcnSet">
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START--%>
                                                            <div ID="RightIcnM" runat="server" text="" visible="False" class="RightIcnM"></div>
                                                            <div ID="RightIcnB" runat="server" text="" visible="False" class="RightIcnB"></div>
                                                            <div ID="RightIcnE" runat="server" text="" visible="False" class="RightIcnE"></div>
                                                            <div ID="RightIcnT" runat="server" text="" visible="False" class="RightIcnT"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                            <%-- <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div> --%>
                                                            <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                                            <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                                            <%-- 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END--%>
                                                            <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                        </div>
                                                        <%-- チップ下段(詳細情報) --%>
                                                        <div class="ColumnTextBox">
                                                            <div ID="DeliveryRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div ID="DeliveryCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>                                                            
                                                            <div ID="DeliveryRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                            <div class="ColumnNo">
                                                                <div ID="DeliveryChargeTechnician" runat="server" class="EllipsisTextRight" style="width:70px"></div>
                                                            </div>
                                                            <div id="ColumnTime" class="ColumnTime">
                                                                <div ID="DeliveryDeliveryPlanTime" runat="server" text=""></div>
                                                            </div>
                                                            <div id="ColumnCount" class="ColumnCount"></div>
                                                            <div id="DeliveryElapsedTime" runat="server" ></div>
                                                            <div id="WorkIcon" runat="server" class=""></div>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="AdjustmentDiv"></div>
                        </div>
                    </div>
                </div>
	        </div>

            <%-- 工程管理ボックスの読み込み中アイコン --%>
            <div id="loadingSchedule"  runat="server"></div>
            <asp:HiddenField ID="UseReception" runat="server" />
            <asp:HiddenField ID="UseNewCustomer" runat="server" />
            <asp:HiddenField ID="AddWorkCloseType" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--カウンター対応--%>
    <script type="text/javascript">
        proccounter();
    </script>
    <%--カウンター対応--%>
	<%--事前準備ポップアップウィンドウ用--%>
    <%-- 事前準備ポップアップ --%>
    <div id="CustomerPopOver" class="saPopOver">
        <div class="triangle top"></div>
        <%-- ヘッダー --%>
	    <div class="header" >
            <icrop:CustomLabel ID="PopupHeader" runat="server" TextWordNo="34" UseEllipsis="False" Height="19px"></icrop:CustomLabel>
        </div>
        <%-- 事前準備 --%>
        <div class="content">
            <%--2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START --%>
            <%--担当SA選択欄--%>
            <asp:UpdatePanel ID="SASelectorPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <div id="SASelectorbox">   <ASP:Dropdownlist ID="SASelector" runat="server" height="25">
                    </ASP:Dropdownlist></Div>
                    <div id="SATitle" class="SASelectTitle" >
                    <icrop:CustomLabel id="SASelectorTitle" runat="server" TextWordNo="81"></icrop:CustomLabel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END --%>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<div id="flickableF" class="ColumnContentsFlame">
						<div class="flickableBox" runat="server" id="flickableBox">
							<asp:Repeater ID="Repeater1" runat="server">
								<ItemTemplate>
									<%-- 事前準備情報の表示 --%>
									<asp:Repeater ID="AdvancePreparationsRepeater" runat="server">
										<ItemTemplate>
												<div id="AdvancePreparations" runat="server" class="CustomerChipFooter">
													<%-- チップエリア --%>
													<div id="AdvancePreparationsDeskDevice" runat="server" class="" visible="true">
														<div class="ColumnContentsBoderIn">
															<%-- チップ上段(マーク) --%>
															<div class="IcnSet">
																<div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
																<div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
																<div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
															</div>
															<%-- チップ下段(詳細情報) --%>
															<div class="ColumnTextBox">
																<div ID="AdvancePreparationsRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
																<div ID="AdvancePreparationsCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
																<div ID="AdvancePreparationsDeliveryPlanTime" runat="server" class="Ellipsis" style="width:130px"></div>
																<div ID="AdvancePreparationsRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
															</div>
														</div>
													<%-- チップエリア終了 --%>
													</div>
												</div>
										</ItemTemplate>
									</asp:Repeater>	
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
					<asp:Button ID="AdvancePreparationsClick" runat="server" style="display:none" />
				</ContentTemplate>
			</asp:UpdatePanel>
 			<%-- 読み込み中アイコン --%>
			<div id="LoadAdvancePreparations"  runat="server"></div>
       </div>
    </div>

    <%--受付登録用のポップアップ--%>
    <icrop:PopOver ID="poReceptionRegister" runat="server" TriggerClientID="ButtonReceptionRegister" HeaderTextWordNo="77" Width="300px" Height="200px" HeaderStyle="Text" PreventBottom="true" PreventRight="true" PreventLeft="true">
        <div class="dataBox">
	        <div class="dataBoxShadowMask">
		        <div class="dataBoxShadowMask2">
			        <div class="dataBoxShadow"></div>
		        </div>
	        </div>
	        <div class="dataBoxShadowMaskUnder">
		        <div class="dataBoxShadowMask2">
			        <div class="dataBoxShadowUnder"></div>
		        </div>
	        </div>
            <div id = "divPopoverScroll">
	            <div class="innerDataBox">
                    <icrop:CustomTextBox ID="txtRegNo" runat="server" PlaceHolderWordNo="78" Width="240" MaxLength="32" OnClientClear="inputChanged" />
                    <icrop:CustomButton ID="ButtonRegister" runat="server" TextWordNo="79" Width="80" Height="30" />
	            </div>
            </div>
        </div>
    </icrop:PopOver>
	<asp:HiddenField ID="hfRegNo" runat="server" />

    <asp:Button ID="RefreshButton" runat="server" style="display:none" />
    
	<div class="BlackWindow">
	</div>

    <%-- 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） START--%>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="PopUpReserveList" style="display:none; z-index:1000;">
                <div id="PopUpReserveListHeader">
                    <icrop:CustomLabel runat="server" ID="PopUpReserveListHeaderLabel" TextWordNo="94" CssClass="Ellipsis" Width="460px" Height="30px"></icrop:CustomLabel>
                </div>
                <div id="PopUpReserveListContents" style="overflow:hidden;">
                    <div style="padding-bottom:2px;">
                        <asp:Repeater ID="ReserveListRepeater" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <div id="ReserveListItem">
                                    <div id="ReserveListItemContents">
                                        <table>
                                            <tr valign="middle" style="height:50px;">
                                                <td>
                                                    <icrop:CustomLabel runat="server" ID="FromLabel" Text='<%# HttpUtility.HtmlEncode(Eval("RESERVEFROMTO")) %>' Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                </td>
                                                <td>
                                                    <icrop:CustomLabel runat="server" ID="ServiceNameLabel" Text='<%# HttpUtility.HtmlEncode(Eval("SERVICENAME")) %>' Width="130px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                </td>
                                                <td>
                                                    <icrop:CustomLabel runat="server" ID="ROStatusLabel" Text='<%# HttpUtility.HtmlEncode(Eval("ROSTATUS")) %>' Width="90px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div id="PopUpReserveListFooter" runat="server">
                    <asp:Button ID="PopUpReserveListFooterButton" CssClass="Ellipsis" runat="server" style="background-color:Gray;" Width="460px" Height="40px"/>
                </div>
            </div><!--PopUpReserveList End-->
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- 2012/11/27 TMEJ 岩城 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.34） END--%>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SC301020footer" Runat="Server">
<%-- 2012/12/14 TMEJ 彭 受付登録ボタンを蓋閉め
    <div ID="FooterCustomButton" style="float:right; margin-right:100px;z-index:900;">
--%>
    <div id="FooterCustomButton" style="display:none;">
        <p id="ButtonReceptionRegister" class="footerCustomButton_ReceptionRegister"><icrop:CustomLabel ID="ReceptionRegister" runat="server" TextWordNo="76" /></p>
    </div>
    <%--2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 START--%>
         <asp:Button ID="OtherjobDummyButton" runat="server" style="display:none" />
          <div id="FooterButton999" runat="server" class="FooterButton">
		    <div id="FooterButtonIcon999" runat="server"></div>
            <icrop:CustomLabel ID="FooterButtonLabel999" class="FooterName_Off" runat="server" TextWordNo="10007" UseEllipsis="False"></icrop:CustomLabel>
        </div>
    <%--'2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 END--%>
    <div class="CustomFooterBox">
        <%-- 事前準備ボタン --%>
	    <asp:UpdatePanel ID="FotterUpdatePanel" runat="server" UpdateMode="Conditional">
	    	<ContentTemplate>
	    		<div id="AdvancePreparationsButton" onclick="AdvancePreparations();"　runat="server">
	    			<div class="AdvancePreparationsNumber">
	    				<icrop:CustomLabel ID="AdvancePreparationsCnt" runat="server" Text=""></icrop:CustomLabel>
	    			</div>
	    			<div class="AdvancePreparationsName"><icrop:CustomLabel runat="server" ID="CustomLabel1" TextWordNo="35" UseEllipsis="False"></icrop:CustomLabel>
	    			</div>
	    		</div>
	    		<%-- 事前準備用のHiddenステータス --%>
	    		<asp:HiddenField ID="AdvancePreparationsCntHidden" runat="server" />
	    		<asp:HiddenField ID="AdvancePreparationsColorHidden" runat="server" />
	    	</ContentTemplate>
	    </asp:UpdatePanel>
        <%-- 来店管理ボタン --%>
        <%--<div class="InnerBox01">
            <div class="VisitManagementFooterIcon"></div>
            <div class="text"><icrop:CustomLabel runat="server" ID="VisitManagementFooterLabel" TextWordNo="102"/></div>
            <asp:Button runat="server" ID="VisitManagementFooterButton"  style="display:none"/>
        </div>--%>
    </div>
</asp:Content>

