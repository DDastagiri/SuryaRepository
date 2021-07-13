<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080214.ascx.vb" Inherits="PagesSC3080214" %>
    
    <link rel="Stylesheet" href="../Styles/SC3080214/SC3080214.css?20111280945000" type="text/css" media="screen,print"  />
    <script src="../Scripts/SC3080214/SC3080214.js?20120106172000" type="text/javascript"></script>

	<div id="scNscCustomerLeftArea" class="contentsFrame" style="height: 240px;">
		<div class="scNscCustomerInfoArea">
		    <!-- 顧客関連情報 -->		
			<div id="CustomerRelatedArea">
				<h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral103" runat="server" Width="320px" TextWordNo="10103" /></h4>
                <%--　＊＊＊＊＊＊＊＊＊＊顧客職業＊＊＊＊＊＊＊＊＊＊--%>
                <table border="0" class="NoBorderTable">
                    <tr>
                        <td style="width: 115px">
                            <asp:UpdatePanel ID="CustomerRelatedOccupationUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="CustomerRelatedOccupationArea" runat="server" onclick="setPopupOccupationPageOpen();">
                            	        <asp:Panel ID="CustomerRelatedOccupationSelectedPanel" runat="server">
                                            <asp:Panel id="CustomerRelatedOccupationSelectedImage" runat="server">
                                                <div class="OccupationCount">&nbsp;</div><div class="OccupationText CustomerRelatedTitleFont">
                                                    <icrop:CustomLabel ID="CustomerRelatedOccupationSelectedLabel" CssClass="ellipsis" runat="server" Width="95" UseEllipsis="true" />
                                                </div>
                                            </asp:Panel>
					                    </asp:Panel>
                                        <asp:Panel ID="CustomerRelatedOccupationNewPanel" runat="server" style="height:100%;width:100%;">
                                            <table style="height:100%;width:100%;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:center;">
                                                        <icrop:CustomLabel ID="WordLiteral111_1" runat="server" Width="98" UseEllipsis="true" TextWordNo="10111" CssClass="CustomerRelatedTitleFont ellipsis" />
                                                    <td>
                                                </tr>
                                            </table>
					                    </asp:Panel>
                                    </div>
                                    <div id="CustomerRelatedOccupationPopupArea" style="display:none; top: -290px;">
	                                    <div class="popUpHeader">
                                            <div class="btnL" style="display:none;">                                        
                                                <div>
                                                    <a onclick="setPopupOccupationPage('page1');" class="styleCut" ><icrop:CustomLabel iD="CustomerRelatedOccupationCancelLabel" runat="server" TextWordNo="10125"></icrop:CustomLabel></a><asp:button ID="CustomerRelatedOccupationCancelButton" runat="server" style="display:none" />
                                                </div>
                                            </div>
                                            <h3 class="popUpTitle" >
                                                <icrop:CustomLabel ID="CustomerRelatedOccupationTitleLabel" runat="server" Width="195px" class="styleCut" TextWordNo="10122"></icrop:CustomLabel></h3><div class="btnR" style="display:none;">
                                                <div>
                                                    <asp:LinkButton ID="CustomerRelatedOccupationRegisterButton" runat="server" CssClass="styleCut" OnClientClick="return checkOtherOccupation();"></asp:LinkButton></div></div></div><div class="popUpBG">
		                                    <div class="popUpArea" style="overflow:hidden;">
                                                <div id="CustomerRelatedOccupationPageArea" >
                                                    <asp:Panel ID="OccupationPopoverForm_1" runat="server" style="width:370px;float:left;">
                                                        <div style="width:360px;">
                                                        <asp:Repeater ID="CustomerRelatedOccupationButtonRepeater" runat="server">
                                                            <ItemTemplate>
                                                                <asp:Panel ID="CustomerRelatedOccupationPanel" runat="server" CssClass="popUpIcon">
                                                                    <asp:LinkButton ID="CustomerRelatedOccupationHyperLink" runat="server">
                                                                        <icrop:CustomLabel ID="CustomerRelatedOccupationText" runat="server" Width="72px" UseEllipsis="True" ClientIDMode="Predictable" CssClass="popupIconTextCenter ellipsis"></icrop:CustomLabel>
                                                                    </asp:LinkButton>
                                                                    <asp:HiddenField ID="CustomerRelatedOccupationSelectedHiddenField" runat="server" />
                                                                    <asp:HiddenField ID="CustomerRelatedOccupationIdHiddenField" runat="server" />
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="OccupationPopoverForm_2" runat="server" style="width:370px;float:left;">
			                                            <div class="occupationOtherRelationship" >
                                                            <icrop:CustomTextBox ID="CustomerRelatedOccupationOtherCustomTextBox" runat="server" CssClass="TextArea" PlaceHolderWordNo="10124" Width="338"  MaxLength="30" TabIndex="1001"></icrop:CustomTextBox>
                                                            <asp:HiddenField ID="CustomerRelatedOccupationOtherIdHiddenField" runat="server" />
                                                        </div>
                                                    </asp:Panel>
                                                </div>
		                                    </div>
	                                    </div>
	                                    <div class="popUpFooterJob"></div>
                                    </div>
                                    <asp:HiddenField ID="OccupationPopupTitlePage1" runat="server" />
                                    <asp:HiddenField ID="OccupationPopupTitlePage2" runat="server" />
                                    <asp:HiddenField ID="OccupationOtherErrMsg" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <%--　＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊--%>
                        <%--　＊＊＊＊＊＊＊＊＊＊家族構成＊＊＊＊＊＊＊＊＊＊--%>	
                        <td style="width: 115px">
                        <asp:UpdatePanel ID="CustomerRelatedFamilyUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="CustomerRelatedFamilyArea" runat="server" onclick="setPopupFamilyPageOpen();">
                                <asp:Panel ID="CustomerRelatedFamilySelectedEditPanel" runat="server">
                                    <asp:Panel id="CustomerRelatedFamilySelectedImage" runat="server">
                                        <div class="FamilyCount">
                                            <asp:Label ID="FamilyCountLabel" runat="server"></asp:Label></div><div class="FamilyText CustomerRelatedTitleFont">
                                            <icrop:CustomLabel ID="CustomerRelatedFamilyTitleLabel" runat="server" TextWordNo="10121" Width="95" UseEllipsis="true" CssClass="ellipsis" />
                                        </div>
                                    </asp:Panel>
                                </asp:Panel> 
                                <asp:Panel ID="CustomerRelatedFamilySelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                    <table style="height:100%;width:100%;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:center;">
                                                <icrop:CustomLabel ID="WordLiteral112" runat="server" Width="98" UseEllipsis="true" TextWordNo="10112" class="CustomerRelatedTitleFont ellipsis" />
                                            <td>
                                        </tr>
                                    </table>
					            </asp:Panel>
                            </div>
                            <div id="CustomerRelatedFamilyPopupArea" style="display:none; top: -365px;">
                                <div class="popUpHeaderFamily">
                                    <div class="btnL">
                                        <div>
                                            <a onclick="CancelCustomerRelatedFamily()" class="styleCut"><icrop:CustomLabel ID="CustomerRelatedFamilyCancelLabel" runat="server" TextWordNo="10125" /></a>
                                            <asp:button ID="CustomerRelatedFamilyCancelButton" runat="server" style="display:none" />
                                        </div> 
                                    </div>
                                    <h3 class="popUpTitle" style="margin-left: 3px;">
                                        <icrop:CustomLabel ID="CustomerRelatedFamilyPopupTitleLabel" runat="server" CssClass="styleCut" Width="148px" TextWordNo="10147" />
                                    </h3>
                                    <div class="btnR">
                                        <div>
                                            <asp:LinkButton ID="CustomerRelatedFamilyRegisterButton" CssClass="styleCut" runat="server" OnClientClick="return RegistCustomerRelatedFamily();"></asp:LinkButton></div></div></div><div class="popUpBGFamily">
                                    <div class="FamilypopUpArea" style="width:320px;height:325px;overflow:hidden;">
                                        <div id="CustomerRelatedFamilyPageArea" >
                                            <asp:Panel ID="CustomerRelatedFamilyPage1" runat="server" style="width:320px;height:325px;float:left;" >
                                                <div id="FamilyListWrap" class="familyAreaScroll popupScrollArea">			
                                                    <div class="familyCountTitle">
                                                        <h4><icrop:CustomLabel ID="FamilyNumberWordLabel" runat="server" Width="300px" UseEllipsis="true" TextWordNo="10148" CssClass="ellipsis" /></h4>
                                                    </div>
                                                    <div id="FamilyCountBox" class="familyCountBox">
                                                        <ul>
				                                            <li onclick="SelectFamilyCount(0);"><a id="FamilyCount1" runat="server" >1</a></li><li onclick="SelectFamilyCount(1);"><a id="FamilyCount2" runat="server" >2</a></li><li onclick="SelectFamilyCount(2);"><a id="FamilyCount3" runat="server" >3</a></li><li onclick="SelectFamilyCount(3);"><a id="FamilyCount4" runat="server" >4</a></li><li onclick="SelectFamilyCount(4);"><a id="FamilyCount5" runat="server" >5</a></li></ul><ul style="margin-top:10px;">
                                                            <li onclick="SelectFamilyCount(5);"><a id="FamilyCount6" runat="server" >6</a></li><li onclick="SelectFamilyCount(6);"><a id="FamilyCount7" runat="server" >7</a></li><li onclick="SelectFamilyCount(7);"><a id="FamilyCount8" runat="server" >8</a></li><li onclick="SelectFamilyCount(8);"><a id="FamilyCount9" runat="server" >9</a></li><li onclick="SelectFamilyCount(9);"><a id="FamilyCount10" runat="server" >10</a></li></ul><div id="TriangulArrowDown" class="TriangulArrowDown" onclick="transitionFamilyCountBox(true);"></div>
                                                        <div id="TriangulArrowUp" class="TriangulArrowUp" onclick="transitionFamilyCountBox(false);"></div>
                                                    </div>
                                                    <div class="FamilyBirthdayTitle">
                                                        <h4 class="TitleLeft"><icrop:CustomLabel ID="FamilyOrganizationWordLabel" runat="server" TextWordNo="10149" Width="95px" UseEllipsis="true" CssClass="ellipsis" /></h4>
                                                        <h4 class="TitleCenter"><icrop:CustomLabel ID="FamilyBirthdayWordLabel" runat="server" TextWordNo="10152" Width="145px" UseEllipsis="true" CssClass="ellipsis" /></h4>
                                                        <h4 class="TitleRight"><icrop:CustomLabel ID="FamilyCalendarWordLabel" runat="server" TextWordNo="10150" Width="52px" CssClass="styleCut"/></h4>
                                                        <div class="clearboth">&nbsp;</div></div><div id="FamilyBirthdayListArea" class="FamilyBirthdayListArea">
                                                        <ul>
                                                            <asp:Repeater ID="FamilyBirthdayList" runat="server">
                                                                <ItemTemplate>
                                                                    <li ID="FamilyBirthdayList_Row" runat="server" ClientIDMode="Predictable">
                                                                        <icrop:CustomLabel ID="FamilyBirthdayListRelationLabel_Row" runat="server" CssClass="type styleCut" ClientIDMode="Predictable" />
                                                                        <icrop:DateTimeSelector ID="FamilyBirthdayListBirthdayDate_Row" runat="server" CssClass="Calendar" ClientIDMode="Predictable" Format="Date" PlaceHolderWordNo="10152" />
                                                                        <asp:HiddenField ID="FamilyBirthdayHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                        <asp:HiddenField ID="FamilyBirthdayListRelationNoHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                        <asp:HiddenField ID="FamilyBirthdayListFamilyNoHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                        <asp:HiddenField ID="FamilyBirthdayListRelationOtherHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:panel ID="CustomerRelatedFamilyPage2" runat="server" style="width:320px;height:325px;float:left;">
                                                <div id="FamilyRelationshipWrap" class="familyAreaScroll popupScrollArea">
                                                    <div id="familyRelationship" class="familyRelationship">
                                                        <ul>
                                                            <asp:Repeater ID="FamilyRelationshipRepeater" runat="server" EnableViewState="False">
                                                                <ItemTemplate>
                                                                    <li id="familyRelationshipList_No" runat="server" >
                                                                        <icrop:CustomLabel ID="familyRelationshipLabel_No" runat="server" Width="250px" CssClass="ellipsis" UseEllipsis="true"/>
                                                                        <asp:HiddenField ID="familyRelationshipNoHidden_No" runat="server"/>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </asp:panel>
                                            <asp:panel ID="CustomerRelatedFamilyPage3" runat="server" style="width:320px;height:325px;float:left;">
                                                <div class="familyAreaScroll">
                                                    <div class="familyOtherRelationship" >
                                                        <icrop:CustomTextBox ID="FamilyOtherRelationshipTextBox" runat="server" CssClass="TextArea" PlaceHolderWordNo="10153" Width="285" MaxLength="30" TabIndex="1002" />
                                                    </div>
                                                </div>
                                            </asp:panel>
                                        </div>
                                    </div>
                                </div>
                                <div class="popUpFooterFamily"></div>
                            </div>
                            <asp:HiddenField ID="RelationOtherWordHidden" runat="server"/>
                            <asp:HiddenField ID="RelationOtherNoHidden" runat="server"/>
                            <asp:HiddenField ID="FamilyCount" runat="server"/>
                            <asp:HiddenField ID="FamilyPopupTitlePage1" runat="server"/>
                            <asp:HiddenField ID="FamilyPopupTitlePage2" runat="server"/>
                            <asp:HiddenField ID="FamilyPopupTitlePage3" runat="server"/>
                            <asp:HiddenField ID="RelationOtherErrMsgHidden" runat="server" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <%--　＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊--%>
                    <%--　＊＊＊＊＊＊＊＊＊＊顧客趣味＊＊＊＊＊＊＊＊＊＊--%>
                    <td style="width: 115px">
                        <asp:UpdatePanel ID="CustomerRelatedHobbyUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <div id="CustomerRelatedHobbyArea" runat="server" onclick="setPopupHobbyPageOpen();">
                            <asp:Panel ID="CustomerRelatedHobbySelectedEditPanel" runat="server">
                                <asp:Panel id="CustomerRelatedHobbySelectedImage" runat="server">
                                    <div class="HobbyCount">
                                        <asp:Label ID="HobbyCountLabel" runat="server"></asp:Label></div><div class="HobbyText CustomerRelatedTitleFont">
                                        <icrop:CustomLabel ID="CustomerRelatedHobbySelectedLabel" runat="server" Width="95" UseEllipsis="true" CssClass="ellipsis" />
                                    </div>
                                </asp:Panel>
					        </asp:Panel>
                            <asp:Panel ID="CustomerRelatedHobbySelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                <table style="height:100%;width:100%;">
                                    <tr>
                                        <td style="vertical-align:middle;text-align:center;">
                                            <icrop:CustomLabel ID="WordLiteral113" runat="server" Width="98" UseEllipsis="true" TextWordNo="10113" CssClass="CustomerRelatedTitleFont ellipsis" />
                                        <td>
                                    </tr>
                                </table>
					        </asp:Panel>
                        </div>
                        <div id="CustomerRelatedHobbyPopupArea" style="display:none; top: -370px;">
	                        <div class="popUpHeader">
                            	<div class="btnL">
                                    <div>
                                        <a onclick="cancelCustomerRelatedHobby()" class="styleCut"><icrop:CustomLabel ID="CustomerRelatedHobbyPopupCancelLabel" runat="server" TextWordNo="10125" /></a>
                                        <asp:button ID="CustomerRelatedHobbyPopupCancelButton" runat="server" style="display:none" />
                                    </div> 
                                </div>
                                <h3 class="popUpTitle" style="margin-left: 3px;">
                                    <icrop:CustomLabel ID="CustomerRelatedHobbyPopupTitleLabel" runat="server" Width="195px" CssClass="styleCut" TextWordNo="10127"></icrop:CustomLabel></h3><div class="btnR">
                                    <div>
                                        <asp:LinkButton ID="RegisterCustomerRelatedHobbyButton" runat="server" CssClass="styleCut" OnClientClick="return registCustomerRelatedHobby();"></asp:LinkButton></div></div></div><div class="HobbyPopupBG4columns">
                            	<div class="HobbypopUpArea4columns">
                                    <div id="CustomerRelatedHobbyPopupPageArea">
                                        <div id="CustomerRelatedHobbyPopupPageWrap">
                                            <asp:panel ID="CustomerRelatedHobbyPopupPage1" runat="server" class="CustomerRelatedHobbyPopupPage">
                                                <asp:repeater id="CustomerRelatedHobbyPopupSelectButtonRepeater" runat="server">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="CustomerRelatedHobbyPopupSelectButtonPanel_Row" runat="server" class="hobbyIcon" ViewStateMode="Enabled" ClientIDMode="Predictable">
                                                            <icrop:CustomLabel ID="CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row" runat="server" Width="72px" class="ellipsis" ClientIDMode="Predictable" UseEllipsis="True" />
                                                            <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonOther_Row" runat="server" ClientIDMode="Predictable" />
                                                            <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row" runat="server" ClientIDMode="Predictable" />
                                                            <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonCheck_Row" runat="server" ClientIDMode="Predictable" />
                                                            <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectedButtonPath_Row" runat="server" ClientIDMode="Predictable" />
                                                            <asp:HiddenField ID="CustomerRelatedHobbyPopupNotSelectedButtonPath_Row" runat="server" ClientIDMode="Predictable" />
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHiddenField" runat="server" />
                                                <div class="popUpIconClear"></div>
                                            </asp:panel>
                                            <asp:panel ID="CustomerRelatedHobbyPopupPage2" runat="server" class="CustomerRelatedHobbyPopupPage">
                                                <div id="CustomerRelatedHobbyPopupOtherWrap">
                                                    <icrop:CustomTextBox ID="CustomerRelatedHobbyPopupOtherText" runat="server" CssClass="TextArea" PlaceHolderWordNo="10129" Width="330" MaxLength="30" TabIndex="1003" />
                                                </div>
                                            </asp:panel>
                                            <div class="popUpIconClear"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="popUpFooterHobby"></div>
                        </div>
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupRowCount" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHobbyNo" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHobbyDefaultText" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupTitlePage1" runat="server"/>
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupTitlePage2" runat="server"/>
                        <asp:HiddenField ID="HobbyOtherErrorMessage" runat="server"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td> 
                    <%--　＊＊＊＊＊＊＊＊＊＊コンタクト方法＊＊＊＊＊＊＊＊＊＊--%>
                        <td style="width: 115px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <div id="CustomerRelatedContactArea" runat="server" onclick="setPopupContactPageOpen();">
                                <asp:Panel ID="CustomerRelatedContactSelectedEditPanel" runat="server">
                                    <asp:Panel id="CustomerRelatedContactSelectedImage" runat="server">
                                        <div class="CustomerRelatedContactSelectedImageAria" style="text-align: center; top: 5px; position: relative;">
                                            <asp:Image ID="CustomerRelatedContactTelImage" runat="server" ImageUrl="" /> 
                                            <asp:Image ID="CustomerRelatedContactMailImage" runat="server" ImageUrl="" />
                                            <div class="ContactText CustomerRelatedTitleFont">
                                                <icrop:CustomLabel ID="CustomerRelatedContactMobileLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10159" /> 
                                                <icrop:CustomLabel ID="CustomerRelatedContactHomeLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10160" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactShortMessageServiceLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10161" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactEmailLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10162" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactDMLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10163" />
                                            </div>
                                        </div>
                                    </asp:Panel>
					            </asp:Panel>
                                <asp:Panel ID="CustomerRelatedContactSelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                    <table style="height:100%;width:100%;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:center;">
                                                <icrop:CustomLabel ID="CustomLabel2" runat="server" Width="98" CssClass="CustomerRelatedTitleFont ellipsis" UseEllipsis="true" TextWordNo="10114" />
                                            <td>
                                        </tr>
                                    </table>
					            </asp:Panel>
                            </div>
                            <div id="CustomerRelatedContactPopupArea" class="scNscPopUpContactSelect scNscPopUpContactSelect48" style="display:none;top:-380px;">
                                <div class="scNscPopUpContactSelectWindownBox WindownBox48">
                                    <div class="scNscPopUpContactSelectHeader">
                                        <div>
                                            <a onclick="cancelContact();" class="scNscPopUpContactCancelButton styleCut"><icrop:CustomLabel ID="ContactHeaderCancelLabel" runat="server" TextWordNo="10125" /></a>                                            
                                            <asp:button ID="CustomerRelatedContactPopupCancelButton" runat="server" style="display:none" />
                                        </div> 
                                        <h3 class="popUpTitle">
                                            <icrop:CustomLabel id="ContactHeaderTitleLabel" runat="server" CssClass="styleCut" Width="250px" TextWordNo="10133" />
                                        </h3>                                        
                                        <div>
                                            <asp:LinkButton ID="ContactHeaderRegisterLinkButton" runat="server" CssClass="scNscPopUpContactCompleteButton styleCut" OnClientClick="return registContact();"></asp:LinkButton></div></div><div class="scNscPopUpContactSelectListArea">
                                        <div class="ContactWish">
                                            <div>
                                                <h4><icrop:CustomLabel ID="ContactWishTitleLabel" runat="server" Width="400px" CssClass="styleCut" TextWordNo="10134" /></h4>
                                            </div>
                                            <div id="ContactToolWrap">
                                                <ul class="scNscPopUpContactSelect5Button">
                                                    <li id="ContactToolMobileLI" runat="server" onclick="selectContactTool(1);" ><asp:HiddenField ID="ContactToolMobileHidden" runat="server" /><asp:panel id="ContactToolMobileImage" runat="server" class="ContactToolIcon" /></li>
                                                    <li id="ContactToolTelLI" runat="server" onclick="selectContactTool(2);" ><asp:HiddenField ID="ContactToolTelHidden" runat="server" /><asp:panel id="ContactToolTelImage" runat="server" class="ContactToolIcon" /></li>
                                                    <li id="ContactToolShortMessageServiceLI" runat="server" onclick="selectContactTool(3);" ><asp:HiddenField ID="ContactToolShortMessageServiceHidden" runat="server" /><asp:panel id="ContactToolShortMessageServiceImage" runat="server" class="ContactToolIcon" /></li>
                                                    <li id="ContactToolEmailLI" runat="server" onclick="selectContactTool(4);" ><asp:HiddenField ID="ContactToolEmailHidden" runat="server" /><asp:panel id="ContactToolEmailImage" runat="server" class="ContactToolIcon" /></li>
                                                    <li id="ContactToolDirectMailLI" runat="server" onclick="selectContactTool(5);" ><asp:HiddenField ID="ContactToolDirectMailHidden" runat="server" /><asp:panel id="ContactToolDirectMailImage" runat="server" class="ContactToolIcon" /></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="ContactWeek1">
                                            <div class="TimeZoneTitle">
                                                <h4 style="height: 12px">
                                                <icrop:CustomLabel ID="ContactWeek1TitleLabel" runat="server" CssClass="styleCut" Width="230px" TextWordNo="10135" />
                                                </h4>
                                                <p class="DayOrWeek">
                                                    <icrop:CustomLabel ID="ContactWeek1WeekdayLabel" runat="server" Width="70px" TextWordNo="10136" CssClass="dayBlue styleCut" onclick="selectContactWeekday(1);" />
                                                    <icrop:CustomLabel ID="ContactWeek1DelimiterLabel" runat="server" TextWordNo="10137" />
                                                    <icrop:CustomLabel ID="ContactWeek1WeekendLabel" runat="server" Width="70px" TextWordNo="10138" CssClass="dayBlue styleCut" onclick="selectContactWeekend(1);" />
                                                </p>
                                            </div>
                                            <div>
                                                <ul class="scNscPopUpContactSelect7Button">
                                                    <li id="ContactWeek1MonLI" runat="server" onclick="selectContactWeek(1,[1]);"><asp:HiddenField ID="ContactWeek1MonHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1MonLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10139" /></div></li>
                                                    <li id="ContactWeek1TueLI" runat="server" onclick="selectContactWeek(1,[2]);"><asp:HiddenField ID="ContactWeek1TueHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1TueLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10140" /></div></li>
                                                    <li id="ContactWeek1WedLI" runat="server" onclick="selectContactWeek(1,[3]);"><asp:HiddenField ID="ContactWeek1WedHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1WedLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10141" /></div></li>
                                                    <li id="ContactWeek1ThuLI" runat="server" onclick="selectContactWeek(1,[4]);"><asp:HiddenField ID="ContactWeek1ThuHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1ThuLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10142" /></div></li>
                                                    <li id="ContactWeek1FriLI" runat="server" onclick="selectContactWeek(1,[5]);"><asp:HiddenField ID="ContactWeek1FriHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1FriLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10143" /></div></li>
                                                    <li id="ContactWeek1SatLI" runat="server" onclick="selectContactWeek(1,[6]);"><asp:HiddenField ID="ContactWeek1SatHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1SatLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10144" /></div></li>
                                                    <li id="ContactWeek1SunLI" runat="server" onclick="selectContactWeek(1,[7]);"><asp:HiddenField ID="ContactWeek1SunHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1SunLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10145" /></div></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="ContactTime1">
                                            <ul class="scNscPopUpContactSelect5Button">
                                                <asp:Repeater id="ContactTime1Repeater" runat="server">
                                                    <ItemTemplate>
                                                        <li id="ContactTime1Li_Row" runat="server" ClientIDMode="Predictable" >
                                                            <div style=" overflow:hidden;height: 39px; ">
                                                                <div class="Center" style="line-height: 13.5px;" >                                                                
                                                                    <icrop:customLabel ID="ContactTime1Label_Row" runat="server" Width="65" style="word-wrap:break-word;" ClientIDMode="Predictable"/>
                                                                    <asp:HiddenField ID="ContactTime1Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                    <asp:HiddenField ID="ContactTimeZoneNo1Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                </div>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                        <div class="ContactWeek2">
                                            <div class="TimeZoneTitle">
                                                <h4 style="height: 12px">
                                                    <icrop:CustomLabel ID="ContactWeek2TitleLabel" runat="server" CssClass="styleCut" Width="230px" TextWordNo="10146" />
                                                </h4>
                                                <p class="DayOrWeek">
                                                    <icrop:CustomLabel ID="ContactWeek2WeekdayLabel" runat="server" Width="70px" TextWordNo="10136" class="dayBlue styleCut" onclick="selectContactWeekday(2);" />
                                                    <icrop:CustomLabel ID="ContactWeek2DelimiterLabel" runat="server" TextWordNo="10137" />
                                                    <icrop:CustomLabel ID="ContactWeek2WeekendLabel" runat="server" Width="70px" TextWordNo="10138" class="dayBlue styleCut" onclick="selectContactWeekend(2);" />
                                                </p>
                                            </div>
                                            <div>
                                                <ul class="scNscPopUpContactSelect7Button">
                                                    <li id="ContactWeek2MonLI" runat="server" onclick="selectContactWeek(2,[1]);"><asp:HiddenField ID="ContactWeek2MonHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2MonLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10139" /></div></li>
                                                    <li id="ContactWeek2TueLI" runat="server" onclick="selectContactWeek(2,[2]);"><asp:HiddenField ID="ContactWeek2TueHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2TueLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10140" /></div></li>
                                                    <li id="ContactWeek2WedLI" runat="server" onclick="selectContactWeek(2,[3]);"><asp:HiddenField ID="ContactWeek2WedHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2WedLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10141" /></div></li>
                                                    <li id="ContactWeek2ThuLI" runat="server" onclick="selectContactWeek(2,[4]);"><asp:HiddenField ID="ContactWeek2ThuHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2ThuLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10142" /></div></li>
                                                    <li id="ContactWeek2FriLI" runat="server" onclick="selectContactWeek(2,[5]);"><asp:HiddenField ID="ContactWeek2FriHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2FriLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10143" /></div></li>
                                                    <li id="ContactWeek2SatLI" runat="server" onclick="selectContactWeek(2,[6]);"><asp:HiddenField ID="ContactWeek2SatHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2SatLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10144" /></div></li>
                                                    <li id="ContactWeek2SunLI" runat="server" onclick="selectContactWeek(2,[7]);"><asp:HiddenField ID="ContactWeek2SunHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2SunLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10145" /></div></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="ContactTime2">
                                            <ul class="scNscPopUpContactSelect5Button">
                                                <asp:Repeater id="ContactTime2Repeater" runat="server">
                                                    <ItemTemplate>
                                                        <li id="ContactTime2Li_Row" runat="server" ClientIDMode="Predictable">
                                                            <div style=" overflow:hidden;height: 39px; ">
                                                                <div class="Center" style="line-height: 13.5px;" >
                                                                    <icrop:customLabel ID="ContactTime2Label_Row" runat="server" Width="65" style="word-wrap:break-word;" ClientIDMode="Predictable"/>
                                                                    <asp:HiddenField ID="ContactTime2Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                    <asp:HiddenField ID="ContactTimeZoneNo2Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                </div>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="scNscPopUpContactSelectFootetr"></div>
                                </div>
                            </div>
                            <asp:HiddenField ID="ContactErrMsg" runat="server"/>
                            <asp:HiddenField ID="ContactTime1Count" runat="server" />
                            <asp:HiddenField ID="ContactTime2Count" runat="server" />
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr> 
                </table> 
				<p class="clearboth"></p>
			</div>
			<hr />
            <!-- 最新顧客メモ -->		
		 	<div class="scNscCustomerMemoArea">
				<h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral104" runat="server" Width="320px" TextWordNo="10104" /></h4>
                <asp:UpdatePanel id="CustomerMemoUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="CustomerMemoEditOpenButton" runat="server" style="display:none" />
                        <asp:Button ID="CustomerMemoEditCloseButton" runat="server" style="display:none" />
                        <div id="CustomerMemo_Click" runat="server" onclick="setPopupCustomerMemoOpen();" >
					        <div class="scNscCustomerMemoPaper">
						        <asp:Panel ID="EditCustomerMemoPanel" runat="server" Visible="true" >
							        <p class="scNscCustomerMemoPaperDay">
                                        <icrop:CustomLabel ID="CustomerMemoDayLabel" runat="server" Text="　" ></icrop:CustomLabel></p><p>
                                        <asp:TextBox ID="CustomerMemoLabel" ReadOnly="true" MaxLength="1024" Width="438" Height="71" runat="server" TextMode="MultiLine" />
                                        <%--<icrop:CustomLabel ID="CustomerMemoLabel" runat="server" Width="438" Height="71" Text="" />--%>
                                    </p>
                                </asp:Panel>
						        <asp:Panel ID="NewCustomerMemoPanel" runat="server" Visible="false" >
							        <p class="scNscCustomerMemoPaperDay">&nbsp;</p><p class="scNscCustomerMemoPaperTxt"><br /></p>
                                </asp:Panel>
                            </div>
				        </div>
                    </ContentTemplate> 
                </asp:UpdatePanel> 
			</div>
		</div>
	</div>
