<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3240501.ascx.vb" Inherits="Pages_SC3240501" %>
<%@ Register Assembly="Toyota.eCRB.iCROP.BizLogic.SC3240501" Namespace="Toyota.eCRB.SMB.ReservationManagement.BizLogic" TagPrefix="SC3240501_cc" %>

<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い START%>
<!-- スクリプトファイルの参照は圧縮済ファイルを対象とする -->
<script type="text/javascript" src="../Scripts/SC3240501/SC3240501.min.js?20200124000000"></script>
<script type="text/javascript" src="../Scripts/SC3240501/SC3240501.Event.min.js?20200124000000"></script>
<script type="text/javascript" src="../Scripts/SC3240501/SC3240501.Define.min.js?2019120200000"></script>
<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い END%>

<link rel="Stylesheet" href="../Styles/SC3240501/SC3240501.css?20140718000000" type="text/css" media="screen,print"/>

<%--チップ新規作成 Start--%>
<div id="NewChipPopup" style="display:none;">
	<asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>

	<div id="NewChipPopupContent" class="NewChipPopStyle">
		<div class="Balloon">
			<div id="NewChipBorderBoxDiv" class="borderBox">
				<div class="Arrow">&nbsp;</div>
				<div id="NewChipMyDataBoxDiv" class="myDataBox">&nbsp;</div>
			</div>
			<div id="NewChipGradationBoxDiv" class="gradationBox">
				<div id="NewChipArrowMask" class="ArrowMask">
					<div class="Arrow">&nbsp;</div>
				</div>
				<div id="NewChipNscPopUpHeaderBgDiv" class="scNscPopUpHeaderBg">&nbsp;</div>
				<div id="NewChipNscPopUpDataBgDiv" class="scNscPopUpDataBg">&nbsp;</div>
			</div>
		</div>

		<div id="NewChipOverShadowDiv" class="OverShadow">&nbsp;</div>

		<%--アクティブインジケータ--%>
		<div id="NewChipActiveIndicator"></div>

		<%--チップ新規作成(共通)ヘッダー Start--%>
		<div id="NewChipPopupHeaderDiv" class="NewChipPopupHeader">
			<%--ヘッダー左--%>
			<div id="NewChipDetailLeftBtnDiv" runat="server" class="LeftBtn">
				<asp:Button ID="NewChipCancelBtn" runat="server" OnClientClick="return NewChipCancelButton();" style="display:none;"/>
				<asp:Button ID="SearchCancelBtn" runat="server" OnClientClick="return SearchCancelButton();" style="display:none;"/>
			</div>

			<%--ヘッダー中央--%>
			<h3>
				<icrop:CustomLabel runat="server" ID="NewChipHeaderLabel" CssClass="NewChipEllipsis" Width="130px" style="display:none;"></icrop:CustomLabel>
				<icrop:CustomLabel runat="server" ID="SearchHeaderLabel" CssClass="NewChipEllipsis" Width="130px" style="display:none;"></icrop:CustomLabel>
			</h3>

			<%--ヘッダー右--%>
			<div id="NewChipDetailRightBtnDiv" runat="server" class="RightBtn">
				<asp:Button ID="NewChipRegisterBtn" runat="server" OnClientClick="return NewChipRegisterButton();" style="display:none;"/>
				<asp:Button ID="SearchRegisterBtn" runat="server" OnClientClick="return SearchRegisterButton();" style="display:none;"/>
			</div>
		</div>
		<%--チップ新規作成(共通)ヘッダー End--%>

		<%--チップ新規作成コンテンツ Start--%>
		<div id="NewChipDataBox" class="dataBox">
			<div class="contentScroll">
				<div class="contentInner">

					<%--チップ新規作成画面コンテンツ Start--%>
					<div id="NewChipContent" class="newChipInnerDataBox">
						<div class="newChipInnerDataBox02">

							<%--チップ新規作成ステータス Start--%>
							<div id="NewChipStatusDiv" class="newChipHeadInfomation newChipHeadInfomation_margin0">
								<div class="newChipInnaerDataBox">
									<h4>
										<icrop:CustomLabel runat="server" ID="NewChipChipStatusLabel" Width="338px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
									</h4>
								</div>
							</div>
							<%--チップ新規作成ステータス End--%>

							<%--チップ新規作成 Start--%>
							<div>
								<%--車両情報エリア--%>
								<ul id="NewChipVclInfoUl" class="newChipTableEntryNo newChipTableEntryNo_Height01">
									<li>
										<dl>
											<dt>
												<%--虫眼鏡（顧客検索画面へ遷移）--%>
												<div class="SearchIcon" onclick="return SlideSearch();"></div>
												
												<icrop:CustomLabel runat="server" ID="NewChipRegNoWordLabel" Width="45px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipRegNoText" Width="250px" MaxLength="32" CssClass="NewChipEllipsis"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipVinWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipVinText" Width="250px" MaxLength="128" CssClass="NewChipEllipsis"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipVehicleWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipVehicleText" Width="250px" MaxLength="128" CssClass="NewChipEllipsis"></asp:TextBox>
											</dd>
										</dl>
									</li>
								</ul>

								<%--顧客情報エリア--%>
								<ul id="NewChipCustInfoUl" class="newChipTableEntryNo newChipTableEntryNo_Height02">
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipCstNameWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipCstNameText" Width="250px" MaxLength="256" CssClass="NewChipEllipsis"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipTitleWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd class="newChipTitle" id="NewChipTitle">
		                                        <icrop:CustomLabel runat="server" ID="NewChipTitleLabel" Width="250px" CssClass="NewChipEllipsis" style="z-index:10;"></icrop:CustomLabel>
		                                        <SC3240501_cc:SC3240501DropDownList runat="server" ID="NewChipTitleList" Width="269px" Height="28px" style="opacity:0; position:relative; top:-29px;"></SC3240501_cc:SC3240501DropDownList>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipHomeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipHomeText" Width="250px" MaxLength="64" CssClass="NewChipEllipsis" type="tel"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipMobileWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<asp:TextBox runat="server" ID="NewChipMobileText" Width="250px" MaxLength="64" CssClass="NewChipEllipsis" type="tel"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li class="newChipCstAddress">
										<dl id="NewChipCstAddressDl">
											<dt id="NewChipCstAddressDt">
												<icrop:CustomLabel runat="server" ID="NewChipCstAddressWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd id="NewChipCstAddressDd">
												<asp:TextBox runat="server" ID="NewChipCstAddressText" TextMode="MultiLine" rows="2" cols="20" Width="250px" Wrap="true" maxlen="320"></asp:TextBox>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipSAWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd class="newChipSA" id="NewChipSA">
												<icrop:CustomLabel runat="server" ID="NewChipSALabel" Width="250px" CssClass="NewChipEllipsis" style="z-index:10;"></icrop:CustomLabel>
												<SC3240501_cc:SC3240501DropDownList runat="server" ID="NewChipSAList" Width="269px" Height="28px" style="opacity:0; position:relative; top:-29px;"></SC3240501_cc:SC3240501DropDownList>
											</dd>
										</dl>
									</li>
								</ul>

								<%--整備種類エリア--%>
								<ul id="NewChipMaintenanceTypeUl" runat="server" class="newChipTableMaintenance">
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipMaintenanceTypeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
		                                        <icrop:CustomLabel runat="server" ID="NewChipMaintenanceTypeLabel" Width="250px" CssClass="NewChipEllipsis" style="z-index:10;"></icrop:CustomLabel>
		                                        <SC3240501_cc:SC3240501DropDownList runat="server" ID="NewChipMaintenanceTypeList" Width="269px" Height="28px" style="opacity:0; position:relative; top:-29px;"></SC3240501_cc:SC3240501DropDownList>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipMercWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
		                                        <icrop:CustomLabel runat="server" ID="NewChipMercLabel" Width="250px" CssClass="NewChipEllipsis" style="z-index:10;"></icrop:CustomLabel>
		                                        <SC3240501_cc:SC3240501DropDownList runat="server" ID="NewChipMercList" Width="269px" Height="28px" style="opacity:0; position:relative; top:-29px;"></SC3240501_cc:SC3240501DropDownList>
											</dd>
										</dl>
									</li>
								</ul>

								<%--予定・実績日時時間エリア--%>
								<ul id="NewChipTimeUl" runat="server" class="newChipTableTime">
									<%--予定・実績日時エリアヘッダー--%>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipVisitTimeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipPlanVisitTimeLabel" Width="250px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
												<icrop:DateTimeSelector ID="NewChipPlanVisitDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipStartTimeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipPlanStartTimeLabel" Width="250px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
												<icrop:DateTimeSelector ID="NewChipPlanStartDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipFinishTimeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipPlanFinishTimeLabel" Width="250px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
												<icrop:DateTimeSelector ID="NewChipPlanFinishDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
											</dd>
										</dl>
									</li>
									<li>
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipDeliveredTimeWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipPlanDeriveredTimeLabel" Width="250px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
												<icrop:DateTimeSelector ID="NewChipPlanDeriveredDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
											</dd>
										</dl>
									</li>
								</ul>

								<%--チェックエリア--%>
								<ul id="NewChipCheckUl" class="newChipTableCheck">
									<%--予約有無--%>
									<li id="NewChipReserveLi" runat="server">
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipReservationCheckWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipReservationYesWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipWalkInWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
										</dl>
									</li>
									<%--待ち方--%>
									<li id="NewChipWaitingLi" runat="server">
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipWaitingCheckWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipWaitingInsideWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipWaitingOutsideWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
										</dl>
									</li>
									<%--洗車有無--%>
									<li id="NewChipCarWashLi" runat="server">
										<dl>
											<dt>
												<icrop:CustomLabel runat="server" ID="NewChipCarWashCheckWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipCarWashYesWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
											<dd>
												<icrop:CustomLabel runat="server" ID="NewChipCarWashNoWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dd>
										</dl>
									</li>
		                            <%--完成検査有無--%>
								    <li id="NewChipCompleteExaminationLi" runat="server">
									    <dl>
										    <dt>
											    <icrop:CustomLabel runat="server" ID="NewChipCompleteExaminationCheckWordLabel" Width="65px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
										    </dt>
										    <dd>
											    <icrop:CustomLabel runat="server" ID="NewChipCompleteExaminationYesWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
										    </dd>
										    <dd>
											    <icrop:CustomLabel runat="server" ID="NewChipCompleteExaminationNoWordLabel" Width="100px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
										    </dd>
									    </dl>
								    </li>
								</ul>

	                            <%-- スクロール位置ずれ調整 --%>
	                            <%--<asp:Button runat = "server" ID="NewChipDummyBtn" Width = "1px" Height = "1px" style="opacity:0; position:absolute;"></asp:Button>--%>

								<%--ご用命エリア--%>
								<ul runat="server" id="NewChipOrderUl" class="newChipTableOrder">
									<li>
										<dl>
											<dt id="NewChipOrderDt">
												<icrop:CustomLabel runat="server" ID="NewChipOrderWordLabel" Width="60px" CssClass="NewChipEllipsis"></icrop:CustomLabel>
											</dt>
											<dd>
												<div class="TextareaBox">
													<asp:TextBox ID="NewChipOrderTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="1300"></asp:TextBox>
												</div>
											</dd>
										</dl>
									</li>
									<li style="clear:both;"></li>
								</ul>

								<%--顧客情報ボタンとR/O参照ボタン--%>
								<div class="FooterButtonSet">
		                            <div id="NewChipCustBtnDiv" runat="server" class="FooterButtonLeftDiv" >
										<asp:Button ID="NewChipCustDetailBtn" runat="server" CssClass="FooterButtonLeft" OnClientClick="return NewChipCustButton();"/>
		                            </div>
		                            <div id="NewChipRORefBtnDiv" runat="server" class="FooterButtonRightDiv" >
										<asp:Button ID="NewChipRORefBtn" runat="server" CssClass="FooterButtonRight" OnClientClick="return SC3240501SubmitCancel();"/>
		                            </div>
								</div>

								<%--画面スクロールの高さ調整--%>
								<div style="height:10px; clear:both;"></div>
							</div>
							<%--チップ新規作成 End--%>

						</div><%--newChipInnerDataBox02 End--%>
					</div><%--newChipInnerDataBox End--%>
					<%--チップ新規作成画面コンテンツ End--%>

					<%--顧客検索画面コンテンツ Start--%>
					<div id="search" class="content">

						<%--検索条件エリア Start--%>
						<div id="headerSearchType">
							<div class="SelectionButton">
								<ul>
                                    <%--2014/07/18 TMEJ 明瀬 予約客検索の検索方法切替ボタンのツールチップ非表示対応 START--%>
									<%--<li onclick="return SelectSearchType(this);" id="Selection1"><icrop:CustomLabel ID="SelectRegNo" runat="server" Width="80px" CssClass="NewChipEllipsis"></icrop:CustomLabel></li>--%>
									<%--<li onclick="return SelectSearchType(this);" id="Selection2"><icrop:CustomLabel ID="SelectVin" runat="server" Width="80px" CssClass="NewChipEllipsis"></icrop:CustomLabel></li>--%>
									<%--<li onclick="return SelectSearchType(this);" id="Selection3"><icrop:CustomLabel ID="SelectName" runat="server" Width="80px" CssClass="NewChipEllipsis"></icrop:CustomLabel></li>--%>
									<%--<li onclick="return SelectSearchType(this);" id="Selection4"><icrop:CustomLabel ID="SelectTelNo" runat="server" Width="80px" CssClass="NewChipEllipsis"></icrop:CustomLabel></li>--%>
									<li onclick="return SelectSearchType(this);" id="Selection1"><icrop:CustomLabel ID="SelectRegNo" runat="server" Width="80px" CssClass="NewChipNotToolChipEllipsis"></icrop:CustomLabel></li>
									<li onclick="return SelectSearchType(this);" id="Selection2"><icrop:CustomLabel ID="SelectVin" runat="server" Width="80px" CssClass="NewChipNotToolChipEllipsis"></icrop:CustomLabel></li>
									<li onclick="return SelectSearchType(this);" id="Selection3"><icrop:CustomLabel ID="SelectName" runat="server" Width="80px" CssClass="NewChipNotToolChipEllipsis"></icrop:CustomLabel></li>
									<li onclick="return SelectSearchType(this);" id="Selection4"><icrop:CustomLabel ID="SelectTelNo" runat="server" Width="80px" CssClass="NewChipNotToolChipEllipsis"></icrop:CustomLabel></li>
                                    <%--2014/07/18 TMEJ 明瀬 予約客検索の検索方法切替ボタンのツールチップ非表示対応 END--%>
								</ul>
							</div>
							<div class="SearchBox">
								<div class="SearchArea" id="SearchArea">
									<div class="SearchButton" onclick="return SearchCustomer();"></div> 
									<%--<div class="SearchButton" runat="server" OnClientClick="return SearchCustomer();"></div> --%>

									<input name="TextArea" class="TextArea" id="SearchText" placeholder=" " type="search" /> 
									<%-- 検索PlaceHold用 --%>
									<icrop:CustomLabel ID="SearchPlaceRegNo" runat="server" style="display:none"></icrop:CustomLabel>
									<icrop:CustomLabel ID="SearchPlaceVin" runat="server" style="display:none"></icrop:CustomLabel>
									<icrop:CustomLabel ID="SearchPlaceName" runat="server" style="display:none"></icrop:CustomLabel>
									<icrop:CustomLabel ID="SearchPlacePhone" runat="server" style="display:none"></icrop:CustomLabel>
									<div class="ClearButton" onclick="return TextClear();"></div>
								</div>
							</div>
						</div>
						<%--検索条件エリア End--%>

						<%--検索結果エリア Start--%>
						<div class="SearchDataBox">
							<div class="SearchDataBoxInner" runat="server" ID="SearchDataBoxInner">
								<asp:UpdatePanel ID="SearchDataUpdate" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<ul id="SearchListBox">
											<li class="FrontLink" ID="FrontLink" runat="server" onclick="SearchFrontList();">
												<div class="FrontList NewChipClip" ID="FrontList" runat="server"></div>
												<span class="FrontSearchingImage"></span>
												<span class="FrontListSearching NewChipClip" ID="FrontListSearching" runat="server"></span>
											</li>
											<%-- 顧客検索結果表示 --%>
											<asp:Repeater ID="SearchRepeater" runat="server">
												<ItemTemplate>
													<li>
														<div class="SearchData">
                                                            <div ID="SearchCustomerName" runat="server" class="Ellipsis"></div>
															<div ID="SearchRegistrationNumber" runat="server" class="Ellipsis"></div>
															<div ID="SearchVinNo" runat="server" class="Ellipsis"></div>															
															<div ID="SearchModel" runat="server" class="Ellipsis"></div>
															<div ID="SearchPhone" runat="server" class="Ellipsis"></div>
															<div ID="SearchMobile" runat="server" class="Ellipsis"></div>
															<%-- 顧客検索紐付け用パラメータ --%>
															<div id="CustomerChangeParameter" runat="server"></div>
														</div>
													</li>
												</ItemTemplate>
											</asp:Repeater>
											<li class="NextLink" id="NextLink" runat="server" onclick="SearchNextList();">
												<div class="NextList NewChipClip" ID="NextList" runat="server"></div>
												<span class="NextSearchingImage"></span>
												<span class="NextListSearching NewChipClip" ID="NextListSearching" runat="server"></span>
											</li>
										</ul>
										<div class="NoSearchImage NewChipEllipsis" ID="NoSearchImage" runat="server"></div>

										<%-- 顧客検索結果格納 --%>
										<asp:Button ID="SearchCustomerDummyButton" runat="server" style="display:none" />
										<asp:HiddenField ID="SearchStartRowHidden" runat="server" />
										<asp:HiddenField ID="SearchEndRowHidden" runat="server" />
										<asp:HiddenField ID="ScrollPositionHidden" runat="server" />
									</ContentTemplate>
								</asp:UpdatePanel>
							</div>
						</div>
						<%--検索結果エリア End--%>

						<div id="SearchDataLoading" class="loadingPopup" runat="server"></div>

						<%--顧客詳細ボタン--%>
						<div id="SearchBottomBox" class="SearchBottomBox">
							<asp:Button ID="SearchBottomButton" runat="server" CssClass="BottomButton BottomButtonDisable" OnClientClick="return NewChipCustButton_Serch();"/>
						</div>
					</div>
					<%--顧客検索画面コンテンツ End--%>

				</div><%--contentInner End--%>
			</div><%--contentScroll End--%>


			<asp:UpdatePanel ID="CustomerSetButtonUpdatePanel" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<asp:HiddenField ID="SearchRegistrationNumberChange" runat="server"/>
					<asp:HiddenField ID="SearchVinChange" runat="server" />
					<asp:HiddenField ID="SearchVehicleChange" runat="server" />
					<asp:HiddenField ID="SearchCustomerNameChange" runat="server"/>
					<asp:HiddenField ID="SearchPhoneChange" runat="server" />
					<asp:HiddenField ID="SearchMobileChange" runat="server" />
					<asp:HiddenField ID="SearchSANameChange" runat="server" />
					<asp:HiddenField ID="SearchCustomerAddressChange" runat="server" />					
					<asp:HiddenField ID="SearchDmsCstCodeChange" runat="server" />
					<asp:HiddenField ID="SearchTitleChange" runat="server" />
					<asp:HiddenField ID="SearchTitleCodeChange" runat="server" />
                    
					<asp:HiddenField ID="InsertSaCode" runat="server" />
					<asp:HiddenField ID="InsertCstId" runat="server" />
					<asp:HiddenField ID="InsertVin" runat="server" />
					<asp:HiddenField ID="InsertVclId" runat="server" />
                    <asp:HiddenField ID="InsertCstVclType" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div><%--dataBox End--%>
		<%--チップ新規作成コンテンツ End--%>

		<div id="SC3240501HiddenArea">
			<div id="SC3240501HiddenContents">
				<asp:HiddenField runat="server" ID="NewChipRezFlgHidden"/>                    <%--予約フラグ         (1:予約/0:飛び込み)--%>
                <asp:HiddenField runat="server" ID="NewChipCompleteExaminationFlgHidden"/>    <%--完成検査有無フラグ (1:有り/0:無し)--%>
				<asp:HiddenField runat="server" ID="NewChipCarWashFlgHidden"/>                <%--洗車有無フラグ     (1:有り/0:無し)--%>
				<asp:HiddenField runat="server" ID="NewChipWaitingFlgHidden"/>                <%--待ち方フラグ       (0:店内/1:店外)--%>
                <asp:HiddenField runat="server" ID="NewChipWorkTimeHidden"/>                  <%--作業時間--%>
                <asp:HiddenField runat="server" ID="NewChipWordDuplicateRestOrUnavailableHidden"/>   <%--登録時に休憩／使用不可チップと重複する場合の文言--%>
                <asp:HiddenField runat="server" ID="NewChipCstBtnErrMsgHidden"/>              <%--顧客詳細ボタン押下時に顧客登録情報が無い場合の文言--%>
                <asp:HiddenField runat="server" ID="NewChipMandatoryFlgHidden"/>              <%--入庫日時・納車日時必須フラグ (1:必須)--%>
                <asp:HiddenField runat="server" ID="NewChipMercMandatoryTypeHidden"/>         <%--サービス・商品項目必須区分 (0:1:2)--%>

				<%-- 顧客検索inputパラメータ --%>
				<asp:HiddenField ID="SearchRegistrationNumberHidden" runat="server"/>
				<asp:HiddenField ID="SearchVinHidden" runat="server" />
				<asp:HiddenField ID="SearchCustomerNameHidden" runat="server" />
				<asp:HiddenField ID="SearchPhoneNumberHidden" runat="server" />
				<asp:HiddenField ID="SearchSelectTypeHidden" runat="server" />

				<%-- 顧客検索エラーメッセージ --%>
				<asp:HiddenField ID="SearchErrMsg1Hidden" runat="server" />
				
                <asp:Button ID="NewChipCustButtonDummy" runat="server" Text="" style="display: none" />   <%--顧客詳細画面へ遷移する為のダミーボタン--%>

                <% '2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START %>
                <%-- 予定入庫日時、予定納車日時計算用パラメータ --%>
                <asp:HiddenField ID="ScheSvcinDeliAutoDispFlg" runat="server" />    <%-- 予定入庫納車自動表示フラグ --%>
                <asp:HiddenField ID="StdAcceptanceTime" runat="server" />           <%-- 標準受付時間 --%>
                <asp:HiddenField ID="StdInspectionTime" runat="server" />           <%-- 標準検査時間 --%>
                <asp:HiddenField ID="StdDeliPreparationTime" runat="server" />      <%-- 標準納車準備時間 --%>
                <asp:HiddenField ID="StdCarwashTime" runat="server" />              <%-- 標準洗車時間 --%>
                <asp:HiddenField ID="StdDeliTime" runat="server" />                 <%-- 標準納車時間 --%>
                <% '2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END   %>
			</div>
		</div>
	</div><%--NewChipPopupContent End--%>
</div><%--NewChipPopup End--%>
<%--チップ新規作成 End--%>
