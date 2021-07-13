<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3240201.ascx.vb" Inherits="Pages_SC3240201" %>


<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い START%>
<!-- スクリプトファイルの参照は圧縮済ファイルを対象とする -->
<script type="text/javascript" src="../Scripts/SC3240201/SC3240201.min.js?20200221000000"></script>
<script type="text/javascript" src="../Scripts/SC3240201/SC3240201.Event.min.js?20200124000000"></script>
<script type="text/javascript" src="../Scripts/SC3240201/SC3240201.Define.min.js?20160915000000"></script>
<script type="text/javascript" src="../Scripts/SC3240201/SC3240201.Scaling.min.js?20160915000000"></script>
<script type="text/javascript" src="../Scripts/SC3240201/SC3240201.fingerscroll.min.js?20160915000000"></script>
<script type="text/javascript" src="../Scripts/SMBCommon/SMBCommon.js?20140401000001"></script>
<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い END%>

<link rel="Stylesheet" href="../Styles/SC3240201/SC3240201Small.css?20140213000000" type="text/css" media="screen,print"/>
<link rel="Stylesheet" href="../Styles/SC3240201/SC3240201Large.css?20140828160000" type="text/css" media="screen,print"/>
<link rel="Stylesheet" href="../Styles/SC3240201/SC3240201Common.css?20180626000000" type="text/css" media="screen,print"/>

<%--チップ詳細 Start--%>
<div id="ChipDetailPopup" style="display:none;">
	<div id="ChipDetailPopupContent" class="ChipDetailPopStyle">
		<div class="Balloon">
			<div id="ChipDetailBorderBoxDiv" class="borderBox">
				<div class="Arrow">&nbsp;</div>
				<div id="ChipDetailMyDataBoxDiv" class="myDataBox">&nbsp;</div>
			</div>
			<div id="ChipDetailGradationBoxDiv" class="gradationBox">
				<div id="ChipDetailArrowMask" class="ArrowMask">
					<div class="Arrow">&nbsp;</div>
				</div>
				<div id="ChipDetailNscPopUpHeaderBgDiv" class="scNscPopUpHeaderBg">&nbsp;</div>
				<div id="ChipDetailNscPopUpDataBgDiv" class="scNscPopUpDataBg">&nbsp;</div>
			</div>
		</div>

        <div id="ChipDetailOverShadowDiv" class="OverShadow">&nbsp;</div>

        <%--アクティブインジケータ--%>
        <div id="DetailSActiveIndicator"></div>

        <%--チップ詳細(共通)ヘッダー Start--%>
		<div id="ChipDetailPopUpHeaderDiv" class="ChipDetailPopUpHeader">

            <%--ヘッダー左--%>
            <div id="DetailLeftBtnDiv" runat="server" class="LeftBtn">
                <asp:Button ID="DetailCancelBtn" runat="server" OnClientClick="return CloseChipDetail(1);"/>
            </div>

			<%--ヘッダー中央--%>
            <h3>
                <icrop:CustomLabel runat="server" ID="DetailSHeaderLabel" CssClass="ChipDetailEllipsis" Width="130px"></icrop:CustomLabel>
            </h3>

            <%--ヘッダー右--%>
			<div id="DetailExpandDiv" runat="server" class="Expand">
				<a id="ExpansionButton" href="javascript:void(0);"></a>
			</div>
            <div id="DetailShrinkDiv" runat="server" class="Shrink">
                <a id="ShrinkingButton" href="javascript:void(0);"></a>
            </div>

            <div id="DetailRightBtnDiv" runat="server" class="RightBtn">
                <asp:Button ID="DetailRegisterBtn" runat="server" OnClientClick="return RegisterChipDetail();"/>
            </div>
		</div>
        <%--チップ詳細(共通)ヘッダー End--%>

        <div style="clear:both;"></div>

        <%--チップ詳細コンテンツ Start--%>
		<div id="ChipDetailDataBox" class="dataBox">
            <%--チップ詳細(小)コンテンツ Start--%>
			<div id="ChipDetailSContent" class="detailSInnerDataBox">
				<div class="detailSInnerDataBox02">
					<div id="DetailSChipStatusDiv" class="detailSHeadInfomation detailSHeadInfomation_margin0">
						<div class="detailSInnaerDataBox">
							<h4>
								<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                <%--<icrop:CustomLabel runat="server" ID="DetailSChipStatusLabel" Width="338px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>--%>
                                <icrop:CustomLabel runat="server" ID="DetailSChipStatusLabel" Width="338px" Height = "24px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                            </h4>
                            <%--中段理由エリア--%>
							<div id="DetailSInterruptionCauseDiv">
								<div id="DetailSInterruptionCauseRepeaterDiv">
									<asp:Repeater ID="DetailSInterruptionCauseRepeater" runat="server">
										<ItemTemplate>
	                                    <div class="detailSaddStatus2">
											<icrop:CustomLabel ID="DetailSInterruptionCauseLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("INTERRUPTIONCAUSE")) %>' CssClass="ChipDetailEllipsis" Width="337px"></icrop:CustomLabel>
											<icrop:CustomLabel ID="DetailSInterruptionDetailsLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("INTERRUPTIONDETAILS")) %>' CssClass="ChipDetailEllipsis" Width="337px"></icrop:CustomLabel>
										</div>
	                                    </ItemTemplate>
									</asp:Repeater>
								</div>
							</div>

                            <%--納車予定、変更回数、納車見込みエリア--%>
							<div class="detailSAddInformationBox">
                                <div class="detailSAddInformationPlan">
                                    <icrop:CustomLabel runat="server" ID="DetailSDeriveredPlanWordLabel" Width="52px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailSDeriveredPlanTimeLabel" Width="31px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </div>
								<div class="detailSAddInformationArrow">
                                    <icrop:CustomLabel runat="server" ID="DetailSChangeNumberLabel" Width="60px" CssClass="ChipDetailEllipsis" style="text-align: right;"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailSTriangleLabel" Width="12px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </div>
								<div class="detailSAddInformationExpected">
                                    <icrop:CustomLabel runat="server" ID="DetailSDeriveredProspectWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailSDeriveredProspectTimeLabel" Width="31px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>                                   
                                </div>
							</div>
						</div>
					</div>

                    <%--納車予定変更履歴エリア--%>
                    <div id="DetailSHeadInfomationPullDiv" class="detailSHeadInfomationPullDiv">
              			<ul>
							<asp:Repeater ID="DetailSChangeTimeRepeater" runat="server">
								<ItemTemplate>
                					<li>
                    					<div class="detailSChangeTimeDiv">
											<icrop:CustomLabel ID="DetailSChangeFromTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeFromTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="DetailSRightArrowLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("RIGHTARROWLABEL")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
											<icrop:CustomLabel ID="DetailSChangeToTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeToTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										</div>
                    					<div class="detailSUpdateTimeDiv"><icrop:CustomLabel ID="DetailSUpdateTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdateTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel></div>
										<div class="detailSUpdatePretextDiv"><icrop:CustomLabel ID="DetailSUpdatePretextLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdatePretext")) %>' CssClass="ChipDetailEllipsis" Width="340px"></icrop:CustomLabel></div>
									</li>
								</ItemTemplate>
							</asp:Repeater>
                			<li class="detailSPullButton"><icrop:CustomLabel ID="DetailSFixUpArrow" runat="server" CssClass="ChipDetailEllipsis" ></icrop:CustomLabel></li>
						</ul>
					</div>

					<div>
                        <%--車両情報エリア--%>
						<ul id="DetailSVclInfoUl" class="detailSTableEntryNo detailSTableEntryNo_Height01" >
							<li>
								<dl>
									<dt>
                                        <icrop:CustomLabel runat="server" ID="DetailSRegNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSRegNoLabel" Width="200px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										<div class="RightIcnSet">
											<div class="IcnSet">
												<div runat="server" id="DetailSIcnD" class="RightIcnD ChipDetailClip"></div>
												<%-- 2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                <%-- <div runat="server" id="DetailSIcnI" class="RightIcnI ChipDetailClip"></div> --%>
												<div runat="server" id="DetailSIcnP" class="RightIcnP ChipDetailClip"></div>
                                                <%-- 2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
												<div runat="server" id="DetailSIcnS" class="RightIcnS ChipDetailClip"></div>
											</div>
										</div>
									</dd>
								</dl>
							</li>
							<li>
								<dl>
									<dt>
                                        <icrop:CustomLabel runat="server" ID="DetailSVinWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSVinLabel" Width="265px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
							<li>
								<dl>
									<dt>
                                        <icrop:CustomLabel runat="server" ID="DetailSVehicleWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSVehicleLabel" Width="265px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
                        </ul>
                        <%--顧客情報エリア--%>
                        <ul id="DetailSCustInfoUl" class="detailSTableEntryNo detailSTableEntryNo_Height02" >
							<li>
								<dl>
									<dt>
                                        <icrop:CustomLabel runat="server" ID="DetailSCstNameWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSCstNameLabel" Width="240px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										<div class="RightIcnSet">
											<div class="IcnSet">
												<div runat="server" id="DetailSIcnV" class="RightIcnV" style="display:none;"></div>
                                                <%-- 2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                <div runat="server" id="DetailSIcnL" class="RightIcnL ChipDetailClip"></div>
                                                <%-- 2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
											</div>
										</div>
									</dd>
								</dl>
							</li>
                            <li>
	                            <dl>
		                            <dt>
			                            <icrop:CustomLabel runat="server" ID="DetailSMobileWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
		                            </dt>
		                            <dd>
			                            <icrop:CustomLabel runat="server" ID="DetailSMobileLabel" Width="265px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
		                            </dd>
	                            </dl>
                            </li>
                            <li>
	                            <dl>
		                            <dt>
			                            <icrop:CustomLabel runat="server" ID="DetailSHomeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
		                            </dt>
		                            <dd>
			                            <icrop:CustomLabel runat="server" ID="DetailSHomeLabel" Width="265px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
		                            </dd>
	                            </dl>
                            </li>
							<li>
								<dl>
									<dt>
                                        <icrop:CustomLabel runat="server" ID="DetailSSAWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSSALabel" Width="265px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
						</ul>

                        <%--予定・実績日時エリア--%>
						<ul id="DetailSTimeUl" runat="server" class="detailSTableTime" >
                            <%--予定・実績日時エリアヘッダー--%>
							<li>
								<dl>
									<dt></dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSVisitTimeWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSStartTimeWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSFinishTimeWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSDeliveredTimeWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>

                            <%--予定日時--%>
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSPlanTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSPlanVisitLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSPlanVisitDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSPlanStartLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSPlanStartDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSPlanFinishLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSPlanFinishDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSPlanDeriveredLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSPlanDeriveredDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
									</dd>
								</dl>
							</li>

                            <%--実績日時--%>
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSProcessTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessVisitTimeLabel" Width="65px" CssClass="ChipDetailEllipsis" style="color:#000;"></icrop:CustomLabel>
									</dd>
									<dd>
                                        <%'2016/10/05 NSK  秋田谷 開発TR-SVT-TMT-20160824-003 チップ詳細の実績時間を変更できなくする START%>
                                        <%--
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessStartLabel" Width="65px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSProcessStartDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
                                        --%>
                                        <!--常に読み取り専用-->
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessStartLabel" Width="65px" CssClass="TextBlack" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSProcessStartDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;" readonly="true" disabled="true"/>
									</dd>
									<dd>
                                        <%--
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessFinishLabel" Width="65px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSProcessFinishDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
                                        --%>
                                        <!--常に読み取り専用-->
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessFinishLabel" Width="65px" CssClass="TextBlack" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailSProcessFinishDateTimeSelector" runat="server" Format="DateTime" Width="70px" Height="29px" style="position:relative; top:-29px; opacity:0;" readonly="true" disabled="true"/>
                                        <%'2016/10/05 NSK  秋田谷 開発TR-SVT-TMT-20160824-003 チップ詳細の実績時間を変更できなくする END%>
									</dd>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSProcessDeriveredTimeLabel" Width="65px" CssClass="ChipDetailEllipsis" style="color:#000;"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
						</ul>

                        <%--整備種類エリア--%>
						<ul id="DetailSMaintenanceTypeUl" runat="server" class="detailSTableMaintenance" >
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSMaintenanceTypeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSMaintenanceTypeLabel" Width="95px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <asp:DropDownList runat="server" ID="DetailSMaintenanceTypeList" Width="95px" Height="28px" style="opacity:0; position:relative; top:-29px;"></asp:DropDownList>
									</dd>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSMercWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailSMercLabel" Width="95px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <asp:DropDownList runat="server" ID="DetailSMercList" Width="95px" Height="28px" style="opacity:0; position:relative; top:-29px;"></asp:DropDownList>
									</dd>
								</dl>
							</li>
						</ul>

                        <%--作業時間エリア--%>
						<ul id="DetailSWorkTimeUl" runat="server" class="detailSWorkTime" >
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSWorkTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <div id="DetailSWorkTimeLeftArrow" class="detailSLeftArrow" onclick="return DetailSWorkTimeLeft();"><span></span></div>
                                        <div class="detailSInputWorkTimeDiv">
                                            <icrop:CustomLabel runat="server" ID="DetailSWorkTimeLabel" Width="90px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                            <icrop:CustomTextBox runat="server" ID="DetailSWorkTimeTxt" Width="90px" CssClass="ChipDetailEllipsis" MaxLength="10" style="position:relative; top:-29px; opacity:0;"></icrop:CustomTextBox>
                                        </div>
                                        <div id="DetailSWorkTimeRightArrow" class="detailSRightArrow" onclick="return DetailSWorkTimeRight();"><span></span></div>
									</dd>
								</dl>
							</li>
						</ul>

                        <%--チェックエリア--%>
					    <ul id="DetailSCheckUl" class="detailSTableCheck">
                            <%--予約有無--%>
						    <li id="DetailSReserveLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailSReservationCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSReservationYesWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSWalkInWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--待ち方--%>
						    <li id="DetailSWaitingLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailSWaitingCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSWaitingInsideWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSWaitingOutsideWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--洗車有無--%>
						    <li id="DetailSCarWashLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailSCarWashCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSCarWashYesWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSCarWashNoWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
						    <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                            <%--完成検査有無--%>
						    <li id="DetailSCompleteExaminationLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailSCompleteExaminationCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSCompleteExaminationYesWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailSCompleteExaminationNoWordLabel" Width="100px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
						    <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                            <%--2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START--%>
                            <%-- スクロール位置ずれ調整 --%>
                            <%--
                            <asp:Button runat = "server" ID="DetailSDummyBtn" Width = "1px" Height = "1px" style="opacity:0;"></asp:Button>
                            --%>
                            <%--2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END--%>
					    </ul>

                        <%--整備内容エリア--%>
						<ul runat="server" id="detailSTableChipUl" class="detailSTableChip" >
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSMaintenanceNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSMaintenanceWordLabel" Width="130px" CssClass="ChipDetailEllipsis" style="padding:0;"></icrop:CustomLabel>
									</dd>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSStallWordLabel" Width="130px" CssClass="ChipDetailEllipsis" style="padding:0;"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
							<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
							<li id="detailSMaintenanceNoCstApproveLi" runat="server">
								<dl>
									<dt id="detailSMaintenanceNoCstApproveDt">
										<icrop:CustomLabel runat="server" ID="DetailSMaintenanceNoCstApproveLabel" Width="355px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
								</dl>
							</li>
							<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

                            <asp:Repeater runat="server" ID="DetailSMaintenanceRepeater" EnableViewState="false">
                                <ItemTemplate>
							        <li>
				                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
				                        <%--
								        <dl id="DetailSMaintenanceDl" runat="server" rowindex='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>' fixitemcode='<%# HttpUtility.HtmlEncode(Eval("MAINTECODE")) %>' fixitemseq='<%# HttpUtility.HtmlEncode(Eval("MAINTESEQ")) %>' srvaddseq='<%# HttpUtility.HtmlEncode(Eval("SRVADDSEQ")) %>' rojobseq='<%# HttpUtility.HtmlEncode(Eval("ROJOBSEQ")) %>' selectrezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' >
                            			--%>
								        <dl id="DetailSMaintenanceDl" runat="server" rowindex='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>' fixitemcode='<%# HttpUtility.HtmlEncode(Eval("JOB_CD")) %>' rojobseq='<%# HttpUtility.HtmlEncode(Eval("RO_SEQ")) %>' selectrezid='<%# HttpUtility.HtmlEncode(Eval("SELECT_JOB_DTL_ID")) %>' jobinstrucdtlid='<%# HttpUtility.HtmlEncode(Eval("JOB_DTL_ID")) %>' jobinstructid='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_ID")) %>'  jobinstructseq='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_SEQ")) %>' jobstatus='<%# HttpUtility.HtmlEncode(Eval("JOB_STATUS")) %>' >
                          				<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
									        <dt>
										        <icrop:CustomLabel runat="server" id="DetailSMaintenanceNoLabel" Width="50px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>'></icrop:CustomLabel>
									        </dt>
									        <dd>
						                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
						                        <%--
										        <icrop:CustomLabel runat="server" ID="DetailSMaintenanceLabel" Width="120px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("MAINTENAME")) %>'></icrop:CustomLabel>
		                            			--%>
										        <icrop:CustomLabel runat="server" ID="DetailSMaintenanceLabel" Width="120px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("JOB_NAME")) %>'></icrop:CustomLabel>
        		                  				<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
									        </dd>
									        <dd class="Cassette" >
                                                <icrop:CustomLabel runat="server" ID="DetailSStallSingleLineLabel" Width="100px" Height="29px" CssClass="ChipDetailEllipsis SingleLine TextBlue"></icrop:CustomLabel>
                                                <asp:Repeater runat="server" ID="DetailSChipRepeater" EnableViewState="False" DataSource='<%# GetChildView(Container.DataItem, "MaintenanceRelation") %>'>
                                                    <ItemTemplate>
                                                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                                        <%--
                                                        <div id="DetailSStallDiv" runat="server" class="Cassette01" rezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' chipindex='<%# HttpUtility.HtmlEncode(Eval("CHIPINDEX")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' rojobseq2='<%# HttpUtility.HtmlEncode(Eval("ROJOBSEQ2")) %>'>
                                                        --%>
                                                        <div id="DetailSStallDiv" runat="server" class="Cassette01" rezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' chipindex='<%# HttpUtility.HtmlEncode(Eval("CHIPINDEX")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' invisibleinstructflg='<%# HttpUtility.HtmlEncode(Eval("INVISIBLE_INSTRUCT_FLG")) %>'>
                                                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
										                    <icrop:CustomLabel runat="server" ID="DetailSStallLabel" Width="100px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("CHIPINFO")) %>'></icrop:CustomLabel>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
									        </dd>
								        </dl>
							        </li>
                                </ItemTemplate>
                            </asp:Repeater>
						</ul>

                        <%--部品エリア--%>
						<ul runat="server" id="detailSTablePartsUl" class="detailSTableParts" >
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailSPartsNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailSPartsWordLabel" Width="250px" CssClass="ChipDetailEllipsis" style="padding:0;"></icrop:CustomLabel>
									</dd>
								</dl>
							</li>
							<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
							<li id="detailSTablePartsNoCstApproveLi" runat="server">
								<dl>
									<dt id="detailSTablePartsNoCstApproveDt">
										<icrop:CustomLabel runat="server" ID="DetailSTablePartsNoCstApproveLabel" Width="355px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
								</dl>
							</li>
							<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                            <asp:Repeater runat="server" ID="DetailSPartsRepeater" EnableViewState="false">
                                <ItemTemplate>
							        <li>
								        <dl>
									        <dt>
										        <icrop:CustomLabel runat="server" ID="DetailSPartsNoLabel" Width="50px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>'></icrop:CustomLabel>
									        </dt>
									        <dd>
										        <icrop:CustomLabel runat="server" ID="DetailSPartsLabel" Width="261px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("PARTS")) %>'></icrop:CustomLabel>
									        </dd>
								        </dl>
							        </li>
                                </ItemTemplate>
                            </asp:Repeater>
						</ul>

                        <%--ご用命エリア--%>
               	        <ul runat="server" id="DetailSOrderUl" class="detailSTableOrder" >
                            <li>
                                <dl>
                                    <dt id="DetailSOrderDt">
                                        <icrop:CustomLabel runat="server" ID="DetailSOrderWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <div class="TextareaBox">
                                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                            <%--<asp:TextBox ID="DetailSOrderTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="400"></asp:TextBox>--%>
                                            <asp:TextBox ID="DetailSOrderTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="1300"></asp:TextBox>
                                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                                        </div>
                                    </dd>
                                        
                                </dl>
                            </li>
                            <li style="clear:both;"></li>
                        </ul>
                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                        <%--故障原因エリア--%>
                        <%--
                        <ul runat="server" id="DetailSFailureUl" class="detailSTableFailure" >
                            <li>
                                <dl>
                                    <dt id="DetailSFailureDt">
                                        <icrop:CustomLabel runat="server" ID="DetailSFailureWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <div class="TextareaBox">
                                            <asp:TextBox ID="DetailSFailureTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="280"></asp:TextBox>
                                        </div>
                                    </dd>
                                </dl>
                            </li>
                            <li style="clear:both;"></li>
                        </ul>
                        --%>
                        <%--診断結果エリア--%>
                        <%--
                        <ul runat="server" id="DetailSResultUl" class="detailSTableResult" >
                            <li>
                                <dl>
                                    <dt id="DetailSResultDt">
                                        <icrop:CustomLabel runat="server" ID="DetailSResultWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <div class="TextareaBox">
                                            <asp:TextBox ID="DetailSResultTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="280"></asp:TextBox>
                                        </div>
                                    </dd>
                                </dl>
                            </li>
                            <li style="clear:both;"></li>
                        </ul>
                        --%>
                        <%--アドバイスエリア--%>
                        <%--
                        <ul runat="server" id="DetailSAdviceUl" class="detailSTableAdvice" >
                            <li>
                                <dl>
                                    <dt id="DetailSAdviceDt">
                                        <icrop:CustomLabel runat="server" ID="DetailSAdviceWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <div class="TextareaBox">
                                            <asp:TextBox ID="DetailSAdviceTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="1200"></asp:TextBox>
                                        </div>
                                    </dd>
                                </dl>
                            </li>
                            <li style="clear:both;"></li>
                        </ul>
                        --%>

                        <%--メモエリア--%>
                        <ul runat="server" id="DetailSMemoUl" class="detailSTableMemo" >
                            <li>
                                <dl>
                                    <dt id="DetailSMemoDt">
                                        <icrop:CustomLabel runat="server" ID="DetailSMemoWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <div class="TextareaBox">
                                            <asp:TextBox ID="DetailSMemoTxt" runat="server" TextMode="MultiLine" Width="259px" maxlen="2601"></asp:TextBox>
                                        </div>
                                    </dd>
                                </dl>
                            </li>
                            <li style="clear:both;"></li>
                        </ul>
                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

                        <%--顧客情報ボタンとR/O参照ボタン--%>
                        <div class="FooterButtonSet">
                            <div id="DetailSCustBtnDiv" runat="server" class="FooterButtonLeftDiv" >
                                <asp:Button ID="DetailSCustDetailBtn" runat="server" CssClass="FooterButtonLeft" OnClientClick="return DetailSCustButton();"/>
                            </div>
                            <div id="DetailSRORefBtnDiv" runat="server" class="FooterButtonRightDiv" >
                                <asp:Button ID="DetailSRORefBtn" runat="server" CssClass="FooterButtonRight" OnClientClick="return DetailSROButton();"/>
                            </div>
                        </div>

                        <%--画面スクロールの高さ調整--%>
                        <div style="height:10px; clear:both;"></div>
                    </div>
				</div><%--detailSInnerDataBox02 End--%>
			</div><%--detailSInnerDataBox End--%>
            <%--チップ詳細(小)コンテンツ End--%>

            <%--チップ詳細(大)コンテンツ Start--%>
            <div id="ChipDetailLContent" class="detailLInnerDataBox">
				<div class="detailLInnerDataBox02">
					<div id="DetailLChipStatusDiv" class="detailLHeadInfomation detailLHeadInfomation_margin0">
						<div class="detailLInnaerDataBox">
							<h4>
								<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                <%--<icrop:CustomLabel runat="server" ID="DetailLChipStatusLabel" Width="900px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>--%>
                                <icrop:CustomLabel runat="server" ID="DetailLChipStatusLabel" Width="900px" Height = "24px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                            </h4>

                            <%--中段理由エリア--%>
							<div id="DetailLInterruptionCauseDiv">
								<div id="DetailLInterruptionCauseRepeaterDiv">
									<asp:Repeater ID="DetailLInterruptionCauseRepeater" runat="server">
										<ItemTemplate>
	                                    <div class="detailLaddStatus2">
											<icrop:CustomLabel ID="DetailLInterruptionCauseLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("INTERRUPTIONCAUSE")) %>' CssClass="ChipDetailEllipsis" Width="935px"></icrop:CustomLabel>
											<icrop:CustomLabel ID="DetailLInterruptionDetailsLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("INTERRUPTIONDETAILS")) %>' CssClass="ChipDetailEllipsis" Width="935px"></icrop:CustomLabel>
										</div>
	                                    </ItemTemplate>
									</asp:Repeater>
								</div>
							</div>

                            <%--納車予定、変更回数、納車見込みエリア--%>
							<div class="detailLAddInformationBox">
								<div class="detailLAddInformationPlan">
                                    <icrop:CustomLabel runat="server" ID="DetailLDeriveredPlanWordLabel" Width="52px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailLDeriveredPlanTimeLabel" Width="31px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </div>
								<div class="detailLAddInformationArrow">
                                    <icrop:CustomLabel runat="server" ID="DetailLChangeNumberLabel" Width="60px" CssClass="ChipDetailEllipsis" style="text-align: right;"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailLTriangleLabel" Width="12px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </div>
								<div class="detailLAddInformationExpected">
                                    <icrop:CustomLabel runat="server" ID="DetailLDeriveredProspectWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel runat="server" ID="DetailLDeriveredProspectTimeLabel" Width="31px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </div>
							</div>
						</div>
					</div>

                    <%--納車予定変更履歴エリア--%>
                    <div id="DetailLHeadInfomationPullDiv" class="detailLHeadInfomationPullDiv">
              			<ul>
							<asp:Repeater ID="DetailLChangeTimeRepeater" runat="server">
								<ItemTemplate>
                					<li>
                    					<div class="detailLChangeTimeDiv">
											<icrop:CustomLabel ID="DetailLChangeFromTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeFromTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="DetailLRightArrowLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("RIGHTARROWLABEL")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
											<icrop:CustomLabel ID="DetailLChangeToTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeToTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										</div>
                    					<div class="detailLUpdateTimeDiv"><icrop:CustomLabel ID="DetailLUpdateTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdateTime")) %>' CssClass="ChipDetailEllipsis"></icrop:CustomLabel></div>
										<div class="detailLUpdatePretextDiv"><icrop:CustomLabel ID="DetailLUpdatePretextLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdatePretext")) %>' CssClass="ChipDetailEllipsis" Width="938px"></icrop:CustomLabel></div>
									</li>
								</ItemTemplate>
							</asp:Repeater>
                			<li class="detailLPullButton"><icrop:CustomLabel ID="DetailLFixUpArrow" runat="server" CssClass="ChipDetailEllipsis" ></icrop:CustomLabel></li>
						</ul>
					</div>

                    <div>
                        <%--車両情報エリア--%>
						<ul class="detailLTableEntryNo">
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailLRegNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
										<icrop:CustomLabel runat="server" ID="DetailLRegNoLabel" Width="310px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										<div class="RightIcnSet">
											<div class="IcnSet">
												<div runat="server" id="DetailLIcnD" class="RightIcnD ChipDetailClip"></div>
												<%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                <%-- <div runat="server" id="DetailLIcnI" class="RightIcnI ChipDetailClip"></div> --%>
												<div runat="server" id="DetailLIcnP" class="RightIcnP ChipDetailClip"></div>
                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
												<div runat="server" id="DetailLIcnS" class="RightIcnS ChipDetailClip"></div>
											</div>
										</div>
									</dd>
								</dl>
							</li>
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailLVinWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLVinLabel" Width="375px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
								</dl>
							</li>
							<li>
								<dl>
									<dt>
										<icrop:CustomLabel runat="server" ID="DetailLVehicleWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									</dt>
									<dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLVehicleLabel" Width="375px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
								</dl>
							</li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
						    <li>
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLSAWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLSALabel" Width="375px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
						</ul>

                        <%--顧客情報エリア--%>
						<ul class="detailLTableName">
						    <li>
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLCstNameWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCstNameLabel" Width="350px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    <div class="RightIcnSet">
										    <div class="IcnSet">
											    <div runat="server" id="DetailLIcnV" class="RightIcnV" style="display:none;"></div>
                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                <div runat="server" id="DetailLIcnL" class="RightIcnL ChipDetailClip"></div>
                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
										    </div>
									    </div>
								    </dd>
							    </dl>
						    </li>
                            <li class="TwoItems">
                                <dl>
                                    <dt>
                                        <icrop:CustomLabel runat="server" ID="DetailLMobileWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLMobileLabel" Width="145px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
                                    <dt>
                                        <icrop:CustomLabel runat="server" ID="DetailLHomeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLHomeLabel" Width="145px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
                                </dl>
                            </li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                            <%--
						    <li>
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="CustomLabel1" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="CustomLabel2" Width="375px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            --%>
                            <%--アドレス--%>
                            <li class="detailLCstAddress">
                                <dl>
                                    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLCstAddressWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
                                    <dd>
										<asp:TextBox runat="server" ID="DetailLCstAddressLabel" TextMode="MultiLine" rows="2" cols="20" Width="375px" Wrap="true" class="TextBlack" onfocus="this.blur()" readonly=""></asp:TextBox>
                                    </dd>
                                </dl>
                            </li>
                            <%--個人・法人--%>
                            <li id="Li1" class="detailLIndividualOrCorporation" runat="server">
                                <dl>
                                    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLIndividualOrCorporationWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLIndividualWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
                                    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCorporationWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                    </dd>
                                </dl>
                            </li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
					    </ul>

                        <%--予定・実績日時エリア--%>
					    <ul id="DetailLTimeUl" runat="server" class="detailLTableTime">
						    <li>
							    <dl>
								    <dt></dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLVisitTimeWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLStartTimeWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLFinishTimeWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLDeliveredTimeWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--予定日時--%>
						    <li>
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLPlanTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLPlanVisitLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLPlanVisitDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
								    </dd>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLPlanStartLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLPlanStartDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
								    </dd>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLPlanFinishLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLPlanFinishDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
								    </dd>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLPlanDeriveredLabel" Width="50px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLPlanDeriveredDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
								    </dd>
							    </dl>
						    </li>
                            <%--実績日時--%>
						    <li>
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLProcessTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessVisitTimeLabel" Width="80px" CssClass="ChipDetailEllipsis" style="color:#000;"></icrop:CustomLabel>
								    </dd>
								    <dd>
                                        <%'2016/10/05 NSK  秋田谷 開発TR-SVT-TMT-20160824-003 チップ詳細の実績時間を変更できなくする START%>
                                        <%--
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessStartLabel" Width="65px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLProcessStartDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
                                        --%>
                                        <!--常に読み取り専用-->
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessStartLabel" Width="65px" CssClass="TextBlack" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLProcessStartDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;" readonly="true" disable="true"/>
								    </dd>
								    <dd>
                                        <%--
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessFinishLabel" Width="65px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLProcessFinishDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;"/>
                                        --%>
                                        <!--常に読み取り専用-->
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessFinishLabel" Width="65px" CssClass="TextBlack" style="z-index:10;"></icrop:CustomLabel>
                                        <icrop:DateTimeSelector ID="DetailLProcessFinishDateTimeSelector" runat="server" Format="DateTime" Width="100px" Height="29px" style="position:relative; top:-29px; opacity:0;" readonly="true" disable="true"/>
                                        <%'2016/10/05 NSK  秋田谷 開発TR-SVT-TMT-20160824-003 チップ詳細の実績時間を変更できなくする END%>
								    </dd>
								    <dd>
                                        <icrop:CustomLabel runat="server" ID="DetailLProcessDeriveredTimeLabel" Width="80px" CssClass="ChipDetailEllipsis" style="color:#000;"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
					    </ul>

                        <%--チェックエリア--%>
					    <ul class="detailLTableCheck">
                            <%--予約有無--%>
						    <li id="DetailLReserveLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLReservationCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLReservationYesWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLWalkInWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                            <%--洗車有無--%>
                            <%--
						    <li id="Li1" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="CustomLabel1" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="CustomLabel2" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="CustomLabel3" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            --%>
                            <%--待ち方--%>
                            <%--
						    <li id="Li2" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="CustomLabel4" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="CustomLabel5" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="CustomLabel6" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            --%>
                            <%-- スクロール位置ずれ調整 --%>
                            <%--
                            <asp:Button runat = "server" ID="Button1" Width = "1px" Height = "1px" style="opacity:0;"></asp:Button>
                            --%>
                            <%--待ち方--%>
						    <li id="DetailLWaitingLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLWaitingCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLWaitingInsideWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLWaitingOutsideWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
					    </ul>

                        <div id="DetailLMaintenanceDiv" runat="server">
                            <%--整備種類エリア--%>
					        <ul class="detailLTableMaintenance">
						        <li>
							        <dl>
								        <dt>
									        <icrop:CustomLabel runat="server" ID="DetailLMaintenanceTypeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								        </dt>
								        <dd>
                                            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceTypeLabel" Width="140px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                            <asp:DropDownList runat="server" ID="DetailLMaintenanceTypeList" Width="140px" Height="28px" style="opacity:0; position:relative; top:-29px;"></asp:DropDownList>
								        </dd>
										<dt>
											<icrop:CustomLabel runat="server" ID="DetailLMercWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										</dt>
										<dd>
                                            <icrop:CustomLabel runat="server" ID="DetailLMercLabel" Width="165px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                            <asp:DropDownList runat="server" ID="DetailLMercList" Width="165px" Height="28px" style="opacity:0; position:relative; top:-29px;"></asp:DropDownList>
										</dd>
							        </dl>
						        </li>
					        </ul>

                            <%--作業時間エリア--%>
					        <ul class="detailLTableWorkTime">
						        <li>
							        <dl>
								        <dt>
									        <icrop:CustomLabel runat="server" ID="DetailLWorkTimeWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								        </dt>
										<dd>
                                            <div id="DetailLWorkTimeLeftArrow" class="detailLLeftArrow" onclick="return DetailLWorkTimeLeft();"><span></span></div>
                                            <div class="detailLInputWorkTimeDiv">
                                                <icrop:CustomLabel runat="server" ID="DetailLWorkTimeLabel" Width="90px" CssClass="ChipDetailEllipsis" style="z-index:10;"></icrop:CustomLabel>
                                                <icrop:CustomTextBox runat="server" ID="DetailLWorkTimeTxt" Width="90px" CssClass="ChipDetailEllipsis" MaxLength="10" style="position:relative; top:-29px; opacity:0;"></icrop:CustomTextBox>
                                                <%-- スクロール位置ずれ調整 --%>
                                                <%-- <asp:TextBox runat = "server" ID="DetailLDummyTxt" Width = "1px" Height = "1px" style="opacity:0;"></asp:TextBox> --%>
                                            </div>
                                            <div id="DetailLWorkTimeRightArrow" class="detailLRightArrow" onclick="return DetailLWorkTimeRight();"><span></span></div>
										</dd>
							        </dl>
						        </li>
					        </ul>
                        </div>

                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                        <%--チェックエリア--%>
					    <ul class="detailLTableCheck">
                            <%--洗車有無--%>
						    <li id="DetailLCarWashLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLCarWashCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCarWashYesWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCarWashNoWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--完成検査有無--%>
						    <li id="DetailLCompleteExaminationLi" runat="server">
							    <dl>
								    <dt>
									    <icrop:CustomLabel runat="server" ID="DetailLCompleteExaminationCheckWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dt>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCompleteExaminationYesWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
								    <dd>
									    <icrop:CustomLabel runat="server" ID="DetailLCompleteExaminationNoWordLabel" Width="150px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								    </dd>
							    </dl>
						    </li>
                            <%--2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START--%>
                            <%-- スクロール位置ずれ調整 --%>
                            <%--
                            <asp:Button runat = "server" ID="DetailLDummyBtn" Width = "1px" Height = "1px" style="opacity:0;"></asp:Button>
                            --%>
                            <%--2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END--%>
					    </ul>
                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

					    <div runat="server" id="detailLClearDiv" class="detailLClear">
                            <%--整備内容エリア--%>
						    <ul class="detailLTableChip" id="detailLTableChipUl">
							    <li>
								    <dl>
									    <dt>
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    </dt>
									    <dd>
									    <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>
										    <%--<icrop:CustomLabel runat="server" ID="DetailLMaintenanceItemsWordLabel" Width="350px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>--%>
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceItemsWordLabel" Width="280px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
										    <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END--%>
									    </dd>
									    <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>
									    <%--<dd>
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceDivisionWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    </dd>--%>
                                        
                                        <%--<dd>
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceDivisionWordLabel" Width="280px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    </dd>--%>
                                        <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END--%>

	                                    <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
										<%--
									    <dd>
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceWorkGWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    </dd>
										--%>
										<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
								    </dl>
							    </li>
								<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>

                            <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>
                                <li class="BtnBoxAll">
                                    <%--2015/04/16 TMEJ 明瀬 ボタンタップでツールチップが表示されてしまう不具合対応 START--%>
                                    <%--<p><a id="AllStart" class="ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 77)%></a></p>--%>
                                    <%--<p><a id="AllFinish" class="ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 78)%></a></p>--%>
                                    <%--<p><a id="AllStop" class="ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 79)%></a></p>--%>
                                    <p> <a id="AllStart" class="ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 77)%></a></p>
                                    <p><a id="AllFinish" class="ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 78)%></a></p>
                                    <p><a id="AllStop" class="ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 79)%></a></p>
                                    <%--2015/04/16 TMEJ 明瀬 ボタンタップでツールチップが表示されてしまう不具合対応 END--%>
                                </li>
                            <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END--%>
								<li id="detailLMaintenanceNoCstApproveLi" runat="server">
								    <dl>
										<dt id="detailLMaintenanceNoCstApproveDt">
										    <icrop:CustomLabel runat="server" ID="DetailLMaintenanceNoCstApproveLabel" Width="953px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
									    </dt>
								    </dl>
								</li>
								<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

                                <asp:Repeater runat="server" ID="DetailLMaintenanceRepeater" EnableViewState="false">
                                    <ItemTemplate>
                                        <li>
                                        <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>
                                        	<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
											<%--
								            <dl id="DetailLMaintenanceRepeaterDl" runat="server" selectrezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>'>
											--%>
                                            <%--<dl id="Dl1" runat="server" selectrezid='<%# HttpUtility.HtmlEncode(Eval("SELECT_JOB_DTL_ID")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' jobstatus='<%# HttpUtility.HtmlEncode(Eval("JOB_STATUS")) %>'>
								            --%>
                                            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                                            <dl id="DetailLMaintenanceRepeaterDl" runat="server" selectrezid='<%# HttpUtility.HtmlEncode(Eval("SELECT_JOB_DTL_ID")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' jobstatus='<%# HttpUtility.HtmlEncode(Eval("JOB_STATUS")) %>'  jobinstructid='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_ID")) %>' jobinstructseq='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_SEQ")) %>'>
											
                                            <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END--%>
									            <dt>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceNoLabel" Width="50px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>'></icrop:CustomLabel>
									            </dt>
	                                        	<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
												<%--
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceItems1Label" Width="260px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("MAINTENAME")) %>'></icrop:CustomLabel>
									            </dd>
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceItems2Label" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("SRVADDSEQCONTENTS")) %>'></icrop:CustomLabel>
									            </dd>
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceDivisionLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("MAINTETYPENAME")) %>'></icrop:CustomLabel>
									            </dd>
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceWorkGLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("WORKGROUP")) %>'></icrop:CustomLabel>
									            </dd>
												--%>
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceItems1Label" Width="205px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("JOB_NAME")) %>'></icrop:CustomLabel>
									            </dd>
									            <dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceItems2Label" Width="59px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("RO_SEQCONTENTS")) %>'></icrop:CustomLabel>
									            </dd>

                                                <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>
                                                    <li class="BtnBoxSingle">
                                                        <%--2015/04/16 TMEJ 明瀬 ボタンタップでツールチップが表示されてしまう不具合対応 START--%>
                                                        <%--<p><a class="SingleStart ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 80)%></a></p>--%>
                                                        <%--<p><a class="SingleFinish ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 81)%></a></p>--%>
                                                        <%--<p><a class="SingleStop ChipDetailEllipsis"><%:WebWordUtility.GetWord("SC3240201", 82)%></a></p>--%>
                                                        <%--<p><a class="SingleReStart ChipDetailEllipsis" style="display: none;"><%:WebWordUtility.GetWord("SC3240201", 83)%></a></p>--%>
                                                        <p><a class="SingleStart ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 80)%></a></p>
                                                        <p><a class="SingleFinish ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 81)%></a></p>
                                                        <p><a class="SingleStop ChipDetailEllipsisNoToolChip"><%:WebWordUtility.GetWord("SC3240201", 82)%></a></p>
                                                        <p><a class="SingleReStart ChipDetailEllipsisNoToolChip" style="display: none;"><%:WebWordUtility.GetWord("SC3240201", 83)%></a></p>
                                                        <%--2015/04/16 TMEJ 明瀬 ボタンタップでツールチップが表示されてしまう不具合対応 END--%>
                                                    </li>
                                                <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END--%>
									            <%--<dd>
										            <icrop:CustomLabel runat="server" ID="DetailLMaintenanceDivisionLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("OPERATION_TYPE_NAME")) %>'></icrop:CustomLabel>
									            </dd>--%>
                                                <%--2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START--%>

												<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
								            </dl>
							            </li>
                                    </ItemTemplate>
                                </asp:Repeater>
						    </ul>

                            <%--チップ選択エリア--%>
                            <div id="stallArea" class="detailLTableChip2">
                                <div class="detailLTitleCassette">
									<icrop:CustomLabel runat="server" ID="DetailLChipWordLabel" Width="250px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</div>
                   	            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                <%-- <div id="scrollChip" style="width:297px;"> --%>
                                <div id="scrollChip" style="width:396px;">
								<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                                    <table id="chipInfoTable" cellpadding="0" cellspacing="0" style="color:#000; ">
                                        <tr></tr>
		                   	            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
		                   	            <%--<tr id="detailLMaintenanceNoCstApproveLi2" runat="server"></tr>--%>
                                        <tr id="detailLMaintenanceNoCstApproveLi2" runat="server">
                                            <td id="detailLMaintenanceNoCstApproveTd2">
                                                <div id="detailLMaintenanceNoCstApproveDt2" runat="server" style="width:396px; height:29px;"></div>
                                            </td>
                                        </tr>
		                   	            <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
		                   	            
                                        <asp:Repeater runat="server" ID="DetailLMaintenanceRepeater2" EnableViewState="false">
                                            <ItemTemplate>                                            
                                                <%--<tr rowindex='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>' fixitemseq='<%# HttpUtility.HtmlEncode(Eval("MAINTESEQ")) %>' selectrezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' class="chipCheck"> --%>
						                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
						                        <%--
                                                <tr rowindex='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>' fixitemseq='<%# HttpUtility.HtmlEncode(Eval("MAINTESEQ")) %>' selectrezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' class="chipCheck">
						                        --%>
                                                <tr rowindex='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>' selectrezid='<%# HttpUtility.HtmlEncode(Eval("SELECT_JOB_DTL_ID")) %>' class="chipCheck">
						                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                                                    <asp:Repeater runat="server" ID="DetailLCheckRepeater" EnableViewState="False" DataSource='<%# GetChildView(Container.DataItem, "CheckRelation") %>'>
                                                        <ItemTemplate>
                                                            <td>
                                                                <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                                                                <%--
                                                                <div id="DetailLCheckDiv" runat="server" class="DetailLChip" style="width:90px; height:29px;" rezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' chipindex='<%# HttpUtility.HtmlEncode(Eval("CHIPINDEX")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' rojobseq2='<%# HttpUtility.HtmlEncode(Eval("ROJOBSEQ2")) %>'></div>
                                                                --%>
                                                                <div id="DetailLCheckDiv" runat="server" class="DetailLChip" style="width:90px; height:29px;" rezid='<%# HttpUtility.HtmlEncode(Eval("REZID")) %>' chipindex='<%# HttpUtility.HtmlEncode(Eval("CHIPINDEX")) %>' stallusestatus='<%# HttpUtility.HtmlEncode(Eval("STALL_USE_STATUS")) %>' invisibleinstructflg='<%# HttpUtility.HtmlEncode(Eval("INVISIBLE_INSTRUCT_FLG")) %>'></div>
						                                        <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
					</div>

                    <div style="clear:both;"></div>

                    <%--部品エリア--%>
					<ul runat="server" id="detailLTablePartsUl" class="detailLTableParts">
						<li>
							<dl>
								<dt>
									<icrop:CustomLabel runat="server" ID="DetailLPartsNoWordLabel" Width="50px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dt>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsItemsWordLabel" Width="350px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsDivisionWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsQuantityWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsUnitWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsBOWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
								<dd>
									<icrop:CustomLabel runat="server" ID="DetailLPartsStatusWordLabel" Width="80px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dd>
							</dl>
						</li>
						<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
						<li id="detailLTablePartsNoCstApproveLi" runat="server">
							<dl>
								<dt id="detailLTablePartsNoCstApproveDt">
									<icrop:CustomLabel runat="server" ID="DetailLTablePartsNoCstApproveLabel" Width="953px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dt>
							</dl>
						</li>
						<%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

                        <asp:Repeater runat="server" ID="DetailLPartsRepeater" EnableViewState="false">
                            <ItemTemplate>
						        <li>
							        <dl>
								        <dt>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsNoLabel" Width="50px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("INDEX")) %>'></icrop:CustomLabel>
								        </dt>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsItem1Label" Width="260px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("PARTS")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsItem2Label" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("SRVADDSEQCONTENTS")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsDivisionLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("PARTSDIV")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsQuantityLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("AMOUNT")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsUnitLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("PARTSUNIT")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsBOLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("BOFLG")) %>'></icrop:CustomLabel>
								        </dd>
								        <dd>
									        <icrop:CustomLabel runat="server" ID="DetailLPartsStatusLabel" Width="80px" CssClass="ChipDetailEllipsis" Text='<%# HttpUtility.HtmlEncode(Eval("PARTSPREPARE")) %>'></icrop:CustomLabel>
								        </dd>
							        </dl>
						        </li>
                            </ItemTemplate>
                        </asp:Repeater>
					</ul>

                    <%--ご用命エリア--%>
					<ul runat="server" id="DetailLOrderUl" class="detailLTableOrder">
						<li>
							<dl>
								<dt id="DetailLOrderDt">
									<icrop:CustomLabel runat="server" ID="DetailLOrderWordLabel" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
								</dt>
								<dd>
									<div class="TextareaBox">
                                        <asp:TextBox ID="DetailLOrderTxt" runat="server" TextMode="MultiLine" Width="380px" Height="47px" maxlen="400"></asp:TextBox>
                                    </div>
								</dd>
							</dl>
						</li>
                        <li style="clear:both;"></li>
					</ul>

                    <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                    <%--故障原因エリア--%>
                    <%--
                    <ul runat="server" id="DetailLFailureUl" class="detailLTableFailure" >
                        <li>
                            <dl>
                                <dt id="DetailLFailureDt">
                                    <icrop:CustomLabel runat="server" ID="DetailLFailureWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </dt>
                                <dd>
                                    <div class="TextareaBox">
                                        <asp:TextBox ID="DetailLFailureTxt" runat="server" TextMode="MultiLine" Width="380px" Height="47px" maxlen="280"></asp:TextBox>
                                    </div>
                                </dd>
                            </dl>
                        </li>
                        <li style="clear:both;"></li>
                    </ul>

                    <div style="clear:both;"></div>
                    --%>
                    <%--診断結果エリア--%>
                    <%--
                    <ul runat="server" id="DetailLResultUl" class="detailLTableResult" >
                        <li>
                            <dl>
                                <dt id="DetailLResultDt">
                                    <icrop:CustomLabel runat="server" ID="DetailLResultWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </dt>
                                <dd>
                                    <div class="TextareaBox">
                                        <asp:TextBox ID="DetailLResultTxt" runat="server" TextMode="MultiLine" Width="380px" Height="47px" maxlen="280"></asp:TextBox>
                                    </div>
                                </dd>
                            </dl>
                        </li>
                        <li style="clear:both;"></li>
                    </ul>
                    --%>
                    <%--アドバイスエリア--%>
                    <%--
                    <ul runat="server" id="DetailLAdviceUl" class="detailLTableAdvice" >
                        <li>
                            <dl>
                                <dt id="DetailLAdviceDt">
                                    <icrop:CustomLabel runat="server" ID="DetailLAdviceWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </dt>
                                <dd>
                                    <div class="TextareaBox">
                                        <asp:TextBox ID="DetailLAdviceTxt" runat="server" TextMode="MultiLine" Width="380px" Height="47px" maxlen="1200"></asp:TextBox>
                                    </div>
                                </dd>
                            </dl>
                        </li>
                        <li style="clear:both;"></li>
                    </ul>

                    <div style="clear:both;"></div>
                    --%>
                    <%--メモエリア--%>
                    <ul runat="server" id="DetailLMemoUl" class="detailLTableMemo" >
                        <li>
                            <dl>
                                <dt id="DetailLMemoDt">
                                    <icrop:CustomLabel runat="server" ID="DetailLMemoWord1Label" Width="60px" CssClass="ChipDetailEllipsis"></icrop:CustomLabel>
                                </dt>
                                <dd>
                                    <div class="TextareaBox">
                                        <asp:TextBox ID="DetailLMemoTxt" runat="server" TextMode="MultiLine" Width="380px" Height="47px" maxlen="2601"></asp:TextBox>
                                    </div>
                                </dd>
                            </dl>
                        </li>
                        <li style="clear:both;"></li>
                    </ul>

                    <div style="clear:both;"></div>
                    <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>

                    <%--顧客情報ボタンとR/O参照ボタン--%>
                    <div class="FooterButtonSet_clear">
                        <div class="FooterButtonSet">
                            <div id="DetailLCustBtnDiv" runat="server" class="FooterButtonLeftDiv" >
                                <asp:Button ID="DetailLCustDetailBtn" runat="server" CssClass="FooterButtonLeft" OnClientClick="return DetailLCustButton();"/>
                            </div>
                            <div id="DetailLRORefBtnDiv" runat="server" class="FooterButtonRightDiv" >
                                <asp:Button ID="DetailLRORefBtn" runat="server" CssClass="FooterButtonRight" OnClientClick="return DetailLROButton();"/>
                            </div>
                        </div>
                    </div>
                    
                    <%--画面スクロールの高さ調整--%>
                    <div style="height:10px; clear:both;"></div>

				</div><%--detailLInnerDataBox02 End--%>             
            </div><%--detailLInnerDataBox End--%>
            <%--チップ詳細(大)コンテンツ End--%>

        </div><%--dataBox End--%>

        <div id="SC3240201HiddenArea">
            <div id="SC3240201HiddenContents">
                <asp:HiddenField runat="server" ID="RezFlgHidden"/>                   <%--予約フラグ     (1:予約/0:飛び込み)--%>
                <asp:HiddenField runat="server" ID="CarWashFlgHidden"/>               <%--洗車有無フラグ (1:有り/0:無し)--%>
                <asp:HiddenField runat="server" ID="WaitingFlgHidden"/>               <%--待ち方フラグ   (0:店内/1:店外)--%>
                <asp:HiddenField runat="server" ID="JDPMarkFlgHidden"/>               <%--JDPマークフラグ　(0:非表示/1:表示)--%>
                <asp:HiddenField runat="server" ID="SSCMarkFlgHidden"/>               <%--SSCマークフラグ　(0:非表示/1:表示)--%>
                <asp:HiddenField runat="server" ID="WordChipUnselectedHidden"/>       <%--チップ詳細(小)でチップ未選択時の文言--%>
                <asp:HiddenField runat="server" ID="ChipDetailSvcStatusHidden"/>      <%--サービスステータス--%>
                <asp:HiddenField runat="server" ID="ChipDetailStallUseStatusHidden"/> <%--ストール利用ステータス--%>
                <asp:HiddenField runat="server" ID="ChipDetailResvStatusHidden"/>     <%--予約ステータス--%>
                <asp:HiddenField runat="server" ID="DeliveryPlanUpdateCountHidden"/>  <%--納車時刻の変更回数--%>
                <asp:HiddenField runat="server" ID="ChipDetailOrderNoHidden"/>        <%--RO番号--%>
                <asp:HiddenField runat="server" ID="MyJobDtlIdHidden"/>               <%--自分自身の作業内容ID--%>
                <asp:HiddenField runat="server" ID="ChipDetailRoJobSeqHidden"/>       <%--作業連番--%>
                <asp:HiddenField runat="server" ID="WordWorkTimeUnitHidden"/>         <%--作業時間に付加する単位の文言--%>
                <asp:HiddenField runat="server" ID="WordDuplicateRestOrUnavailableHidden"/>   <%--登録時に休憩／使用不可チップと重複する場合の文言--%>
                <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START--%>
                <asp:HiddenField runat="server" ID="CompleteExaminationFlgHidden"/>   <%--完成検査有無フラグ (1:有り/0:無し)--%>
                <asp:HiddenField runat="server" ID="FleetFlgHidden"/>                 <%--法人フラグ (0:個人/1:法人)--%>
                <asp:HiddenField runat="server" ID="CstTypeHidden"/>                  <%--顧客種別--%>
                <asp:HiddenField runat="server" ID="DmsCstCdHidden"/>                 <%--基幹顧客コード --%>
                <asp:HiddenField runat="server" ID="DetailCstBtnErrMsgHidden"/>       <%--顧客詳細ボタン押下時に顧客登録情報が無い場合の文言--%>
                <asp:HiddenField runat="server" ID="PartsDtlErrMsgHidden"/>           <%--部品情報取得のWebServiceでエラーが発生した場合の文言--%>
                <asp:HiddenField runat="server" ID="NameTitleNameHidden"/>            <%--敬称--%>
                <asp:HiddenField runat="server" ID="PositionTypeHidden"/>             <%--配置区分--%>
                <asp:HiddenField runat="server" ID="DmsJobDtlIdHidden"/>              <%--基幹作業内容ID--%>
                <asp:HiddenField runat="server" ID="InvoiceDateTimeHidden"/>          <%--清算準備完了日時--%>
                <asp:HiddenField runat="server" ID="MandatoryFlgHidden"/>             <%--入庫日時・納車日時必須フラグ (1:必須)--%>
                <%--2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END--%>
                <%--2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START--%>
                <asp:HiddenField runat="server" ID="MercMandatoryTypeHidden"/> <%--サービス・商品項目必須区分 (0:1:2)--%>
                <%--2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END--%>

                <%--画面間パラメータの引渡しに使用する項目--%>
                <asp:HiddenField runat="server" ID="Visit_VclRegNoHidden"/>                 <%--【サービス来店者管理】車両登録No--%>
                <asp:HiddenField runat="server" ID="Visit_VINHidden"/>                      <%--【サービス来店者管理】VIN--%>
                <asp:HiddenField runat="server" ID="Visit_TelNoHidden"/>                    <%--【サービス来店者管理】電話番号--%>
                <asp:HiddenField runat="server" ID="Visit_MobileNoHidden"/>                 <%--【サービス来店者管理】携帯番号--%>
                <asp:HiddenField runat="server" ID="Visit_VisitSeqHidden"/>                 <%--【サービス来店者管理】来店者実績連番--%>
                <asp:HiddenField runat="server" ID="Visit_AssignStatusHidden"/>             <%--【サービス来店者管理】振当ステータス--%>
                <asp:HiddenField runat="server" ID="ChipDetail_VclRegNoHidden"/>            <%--車両登録No--%>
                <asp:HiddenField runat="server" ID="ChipDetail_VinHidden"/>                 <%--VIN--%>
                <asp:HiddenField runat="server" ID="ChipDetail_KatashikiHidden"/>           <%--車両型式--%>
                <asp:HiddenField runat="server" ID="ChipDetail_TelNoHidden"/>               <%--電話番号--%>
                <asp:HiddenField runat="server" ID="ChipDetail_MobileNoHidden"/>            <%--携帯番号--%>
                <asp:HiddenField runat="server" ID="ChipDetail_DlrCodeHidden"/>             <%--販売店コード--%>
                <asp:HiddenField runat="server" ID="ChipDetail_ServiceInIDHidden"/>         <%--サービス入庫ID--%>

                <asp:Button ID="DetailCustButtonDummy" runat="server" Text="" style="display: none" />           <%--顧客詳細画面へ遷移する為のダミーボタン--%>
                <asp:Button ID="DetailROButtonDummy" runat="server" Text="" style="display: none" />             <%--RO参照画面へ遷移する為のダミーボタン--%>

                <asp:HiddenField runat="server" ID="JobStartDtlErrMsgHidden"/>       <%--顧客詳細で当日ではない時をJob開始の文言--%>
            </div>
        </div>

    </div><%--ChipDetailPopupContent End--%>
</div><%--ChipDetailPopup End--%>