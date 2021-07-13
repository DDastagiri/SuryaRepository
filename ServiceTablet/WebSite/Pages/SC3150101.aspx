<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3150101.aspx.vb" Inherits="Pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3150101/SC3150101.css?20141003130000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3150101/Chips.css?20180706000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3150101/PopUpBase.css?20141003130000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3150101/SC3150101.flickable.js"></script>
    <script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Chip.js?20180706000000"></script>
    <script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Meter.js?20141020101500"></script>
    <script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Main.js?20200221000000" ></script>
    <script type="text/javascript" src="../Scripts/SC3150101/SC3150101.PopUp.js?20170916000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<div id="contentsFrame" class="contentsFrame">
        <asp:HiddenField ID="HiddenJsonData" runat="server" />
        <asp:HiddenField ID="HiddenFieldOrderNo" runat="server" />
        <asp:HiddenField ID="HiddenFieldChildNo" runat="server" />
        <asp:HiddenField ID="HiddenFieldRepairOrderFilter" runat="server" />
        <asp:HiddenField ID="HiddenFieldRepairOrderIcon" runat="server" />
        <asp:HiddenField ID="HiddenWarnNextDate" runat="server" />
        <asp:HiddenField ID="HiddenServerTime" runat="server" />
        <asp:HiddenField ID="HiddenStartTimeWord" runat="server" />
        <asp:HiddenField ID="HiddenEndTimeWord" runat="server" />
        <asp:HiddenField ID="HiddenResultStartTimeWord" runat="server" />
        <asp:HiddenField ID="HiddenResultEndTimeWord" runat="server" />
        <asp:HiddenField ID="HiddenPopupPartsCancelWord" runat="server" />
        <asp:HiddenField ID="HiddenPopupPartsTitleWord" runat="server" />
        <asp:HiddenField ID="HiddenAddWorkConfirmWord" runat="server" />
        <asp:HiddenField ID="HiddenReloadFlag" runat="server" />
        <asp:HiddenField ID="HiddenFieldInspectionApprovalFlag" runat="server" />
        <asp:HiddenField ID="HiddenChipResultStatus" runat="server" />
        <asp:HiddenField ID="HiddenBreakPopUpFlg" runat="server" />
        <asp:HiddenField ID="HiddenJobStopWindowFlg" runat="server" />
        <asp:HiddenField ID="HiddenStopReasonType" runat="server" />
        <asp:HiddenField ID="HiddenStopTime" runat="server"  />
        <asp:HiddenField ID="HiddenStopMemo" runat="server"  />
        <asp:HiddenField ID="HiddenConfirmStartWording" runat="server" />
        <asp:HiddenField ID="HiddenConfirmFinishWording" runat="server" />
        <asp:HiddenField ID="HiddenStopTimeWord" runat="server" />
        <asp:HiddenField ID="HiddenOpretionCode" runat="server" />
        <asp:HiddenField ID="HiddenRestartStopJobFlg" runat="server" />
        <%-- ここからストール情報 --%>
        <div class="stc01Box01">
        <asp:HiddenField ID="HiddenRestText" runat="server" />
        <asp:HiddenField ID="HiddenUnavailableText" runat="server" />
        <asp:HiddenField ID="HiddenCandidateId" runat="server" />
        <asp:HiddenField ID="HiddenStallStartTime" runat="server" />
        <asp:HiddenField ID="HiddenStallEndTime" runat="server" />
        <asp:HiddenField ID="HiddenScrollLeft" runat="server" />
        <asp:HiddenField ID="HiddenSelectedId" runat="server" />
        <asp:HiddenField ID="HiddenSelectedChip" runat="server" Value="0" />
        <asp:HiddenField ID="HiddenSelectedReserveId" runat="server" />
        <asp:HiddenField ID="HiddenSelectedJobDetailId" runat="server" />
        <asp:HiddenField ID="HiddenSelectedStallUseStatus" runat="server" />
        <asp:HiddenField ID="HiddenOrderStatus" runat="server" />
        <asp:HiddenField ID="HiddenHistoryOrderNumber" runat="server" />
        <asp:HiddenField ID="HiddenHistoryDealerCode" runat="server" />
        <asp:HiddenField ID="HiddenSelectedWorkSeq" runat="server" />
        <asp:HiddenField ID="HiddenFieldInstruct" runat="server" />
        <asp:HiddenField ID="HiddenFieldEndWorkFlg" runat="server" />
        <asp:HiddenField ID="HiddenSelectedVclRegNo" runat="server" />
        <asp:HiddenField ID="HiddenSelectedUpdateCount" runat="server" />
        <asp:HiddenField ID="HiddenTcStatusStandTime" runat="server" />
        <asp:HiddenField ID="HiddenSelectdealerCode" runat="server" />
        <asp:HiddenField ID="HiddenAddWorkButtonFlg" runat="server" />
        <asp:HiddenField ID="HiddenServiceInNumber" runat="server" />
        <asp:HiddenField ID="HiddenChipStartDateTime" runat="server" />
        <asp:HiddenField ID="HiddenChipEndDateTime" runat="server" />
        <asp:HiddenField ID="HiddenHistoryOrderNumberSeq" runat="server" />
        <%-- JavaScriptより動作させるため、非表示とするボタン --%>
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START--%>
        <%--<asp:Button ID="HiddenButtonFlickRepairOrder" runat="server" CssClass="HiddenButton" OnClientClick="LoadingScreen();"/>--%>
        <%--<asp:Button ID="HiddenButtonRepairOrderIcon" runat="server" CssClass="HiddenButton" OnClientClick="LoadingScreen();"/>--%>
        <asp:Button ID="HiddenButtonFlickRepairOrder" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonRepairOrderIcon" runat="server" CssClass="HiddenButton" />
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END--%>
        <asp:Button ID="HiddenButtonChipTap" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonRefresh" runat="server" CssClass="HiddenButton" />
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START--%>
        <%--<asp:Button ID="HiddenButtonHistory" runat="server" CssClass="HiddenButton" OnClientClick="LoadingScreen();" />--%>
        <asp:Button ID="HiddenButtonHistory" runat="server" CssClass="HiddenButton" />
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END--%>
        <asp:Button ID="HiddenButtonRedirectSC3170201" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonRedirectSC3170203" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonRedirectSC3150201" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="ButtonDoNotBreak" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="ButtonTakeBreak" runat="server" CssClass="HiddenButton" />

        <div class="Box01In">
            <div class="Box01Title">
                <icrop:CustomLabel ID="LabelStallName" runat="server" UseEllipsis="true" Width="82px" />
            </div>
            <div class="Box01Title02">
                <icrop:CustomLabel ID="CustomerLiteral2" runat="server" UseEllipsis="true" Width="26px" TextWordNo="2" />
                &nbsp;&nbsp;
                <icrop:CustomLabel ID="LabelEngineerName" runat="server" UseEllipsis="true" Width="400px" />
            </div>
            <div class="Box01GraphBox" id="Box01GraphBox">

            <div class="Box01GraphLine" id="Box01GraphLine">
                <div class="Box01GraphLineFilter" id="Box01GraphLineFilter"></div>
            </div><%--Box01GraphLine--%>

            <div class="CurrentBox" id="CurrentBox">
                <div class="CurrentBoxTime" id="CurrentBoxTime"></div>
            </div>

            </div><%--Box01GraphBox--%>
        </div><%--Box01In--%>
        </div><%--contentsFrame--%>
        <%-- ここまでストール情報 --%>

        <%-- ここから作業進捗情報 --%>
        <%-- グレーフィルター --%>
        <div id="stc02Box02Filter" class="stc02Box02Filter"></div>
        <%-- ここまでグレーフィルター --%>
        <div class="stc01Box02" onclick="ScreenSeverLoad()">
        <h2 class="contentTitle">
            <icrop:CustomLabel ID="CustomerLiteral4" class="contentTitleText" runat="server" TextWordNo="4" UseEllipsis="true" Width="190px" />
        </h2>
        <div class="Box02In">
            <span>
                <icrop:CustomLabel ID="CustomerLiteral5" UseEllipsis="true" Width="80px" runat="server" TextWordNo="5" />
            </span>
            &nbsp;
            <span>
                <icrop:CustomLabel ID="LabelRONumber" runat="server" UseEllipsis="true" Width="160px" />
            </span>
            <br />
            <span>
                <icrop:CustomLabel ID="CustomerLiteral6" UseEllipsis="true" Width="80px" runat="server" TextWordNo="6" />
            </span>
            &nbsp;
            <span>
                <icrop:CustomLabel ID="LabelChargeSA" runat="server" UseEllipsis="true" Width="160px" />
            </span><br />
            <div class="TimeBox">
                <icrop:CustomLabel ID="LiteralStartTimeText" UseEllipsis="true" Width="120px" runat="server" />&nbsp;<strong><asp:Label ID="LabelMeterStartTime" Width="65px" runat="server"></asp:Label></strong><br/>
                <icrop:CustomLabel ID="LiteralEndTimeText" UseEllipsis="true" Width="120px" runat="server" />&nbsp;<strong><asp:Label ID="LabelMeterEndTime" Width="65px" runat="server"></asp:Label></strong>
            </div>
        </div>
        <%-- ロケーション番号 --%>
         <div class="locationBox">
            <icrop:CustomLabel  ID="locationLabel" UseEllipsis="true" Width="70px" runat="server"/>
        </div>   
        </div>
        <%-- ここまで作業進捗情報 --%>                  

        <%-- R/O情報パネル --%>
        <div id="roInfomationBox" >
            <iframe id="stc01Box03" class="stc01Box03" src="SC3150102.aspx" name="stc01Box03" scrolling="no" seamless="seamless"></iframe>
            <%-- 読み込み中アイコン --%>
            <div id="loadingroInfomation"></div>
        </div>
        <asp:HiddenField ID="HiddenPartsCount" runat="server" />

        <%-- 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
        <%-- <asp:HiddenField ID="HiddenBackOrderCount" runat="server" /> --%>
        <%-- 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>

        <asp:HiddenField ID="HiddenSelectedTabNumber" runat="server" />
    </div>


    <asp:HiddenField ID="HiddenPushedFooter" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenBreakPopup" runat="server" Value="0" />
    <div id="tcvNsc31Main" style="display:none">
    <div class="tcvNsc31Black" id="tcvNsc31Black" style="display:none"></div>
    <div class="popWind" id="popWind" >
		<div class="PopUpBtn01">
			<ul>
				<li class="buttonC" onclick="BreakPopupConfirm(true);"><a href="#"><icrop:CustomLabel ID="CustomLabelBreakCancel" runat="server" TextWordNo="22" /></a></li>
				<li class="title"><icrop:CustomLabel ID="CustomLabelBreakTitle" runat="server" TextWordNo="21" /></li>
			</ul>
		</div>
		<div class="dataWind1">
			<div class="TextBox">
				<div class="TextBoxIn">
					<ul>
						<li class="ListTitle"><icrop:CustomLabel ID="DummyBtnDoNotBreak" class="ListTitleButton" runat="server" TextWordNo="23"/></li>
						<li class="ListDate"><icrop:CustomLabel ID="DummyBtnTakeBreak" class="DateTitleButton" runat="server" TextWordNo="24" /></li>
					</ul>
				</div>
			</div>
		</div>
		<div class="baseWind1">
			<div class="boxBoder">
			</div>
			<div class="box">&nbsp;</div>
		</div>
	</div>
    </div>

          <%--中断理由画面--%>
		<div  class="popStopWindowBase"  style="display:none">
		    <div class="Balloon">
		    <div class="borderBox">
                <div class="Arrow">&nbsp;</div>
		        <div class="myDataBox">&nbsp;</div>
	        </div>
		    <div class="gradationBox">
                <%--<div class="ArrowMaskR"><div class="ArrowR">&nbsp;</div></div>
                <div class="ArrowMaskL"><div class="ArrowL">&nbsp;</div></div>--%>
		        <div class="scStopPopUpHeaderBg">&nbsp;</div>
		        <div class="scStopPopUpDataBg">&nbsp;</div>
	        </div>
	        </div>
		    <div class="PopUpHeader">
                <icrop:CustomLabel ID="lblStopReasonTitle" runat="server" TextWordNo="31" Width="158px" UseEllipsis="true"></icrop:CustomLabel>
		        <div class="LeftBtn"  onclick='CancelStopWindow()'>
                    <icrop:CustomLabel ID="lblStopCancelBtn" runat="server" TextWordNo="32" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
		        <div class="RightBtn" onclick="ConfirmStopWindow()">
                    <icrop:CustomLabel ID="lblStopLoginBtn" runat="server" TextWordNo="33" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
            </div>
		    <div class="dataBox">
                <div id="StopMemoScrollBox">
		            <div class="innerDataBox">
                    <icrop:CustomLabel ID="lblStopReason" runat="server" TextWordNo="34" UseEllipsis="False" class="PopInnerTitle Ellipsis" ></icrop:CustomLabel>
                    <asp:Button runat="server" ID="btnJobStopDummy" Width="1px" Height="1px" style="opacity:0; position: absolute;" />
                    <!-- Window内部 -->
                    <ul class="DataListTable">
                        <li class="Check" onclick="SelectStopArea(0)">
                            <icrop:CustomLabel ID="CustomLabel29" runat="server" TextWordNo="35" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                        <li onclick="SelectStopArea(1)">
                            <icrop:CustomLabel ID="CustomLabel30" runat="server" TextWordNo="36" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                        <li onclick="SelectStopArea(2)">
                            <icrop:CustomLabel ID="CustomLabel31" runat="server" TextWordNo="37" UseEllipsis="False" class="Ellipsis" ></icrop:CustomLabel>
                        </li>
                    </ul>
                    <icrop:CustomLabel ID="CustomLabel32" runat="server" TextWordNo="38" UseEllipsis="False" class="PopInnerTitle Ellipsis"></icrop:CustomLabel>
                    <ul class="TableWorkingHours">
                        <li>
                        <dl>
                            <dd>
                                <p class="LeftArrow" onclick="ChangeStopMinutes(-5)"><span></span></p>
                                <div class="StopTimeLabel" onclick="ClickStopTime()">
                                    <icrop:CustomLabel ID="CustomLabel33" runat="server" TextWordNo="39" UseEllipsis="False"></icrop:CustomLabel>
                                </div>
                                <icrop:CustomTextBox runat="server" ID="StopTimeTxt" Width="90px" CssClass="ChipDetailEllipsis" MaxLength="10"  onblur="BindStopWndEvent()"></icrop:CustomTextBox>
                                <p class="RightArrow" onclick="ChangeStopMinutes(5)"><span></span></p>
                            </dd>
                        </dl>
                        </li>
                    </ul>
              
                    <icrop:CustomLabel ID="CustomLabel34" runat="server" TextWordNo="40" UseEllipsis="False" class="PopInnerTitle Ellipsis"></icrop:CustomLabel>
                    <ul class="DataListTable">
                    <li class="NextArrow">
                        <icrop:CustomLabel runat="server" ID="lblDetailStopMemo" Width="230px"  CssClass="ChipDetailEllipsis" ></icrop:CustomLabel>
                        <asp:DropDownList runat="server" ID="dpDetailStopMemo" ></asp:DropDownList>
                     </li>
                    </ul>
                    <ul class="DataListTable">
                        <li class=" Hg111">
                             <asp:TextBox ID="txtStopMemo" runat="server" TextMode="MultiLine" width="292px" Height="100px" maxlen="200" CssClass="ChipDetailEllipsis"></asp:TextBox>
                        </li>
                    </ul>
              
                    <!-- /Window内部 -->
	                </div>
	            </div>
            </div>
	    </div>
		<div id="BlackWindow" class="BlackWindow"></div>

        <icrop:PopOver ID="CTConfirmPop" runat="server">
            <div class="CTConfirmPopContentBody">
                <ul class="CTConfirmPopList">
                 <li>
                <icrop:CustomLabel  CssClass ="CTConfirmPopTitleBlockButtonLeft" ID="PopUpButtonSuspendWork"  runat="server" TextWordNo="15">
                </icrop:CustomLabel>
                 </li>
                 <li>
                <icrop:CustomLabel  CssClass ="CTConfirmPopTitleBlockButtonLeft" ID="PopUpButtonFinishWork"  runat="server" TextWordNo="27">
                </icrop:CustomLabel>
                 </li>
                </ul> 
            </div>
            <div id="CTConfirmPop_Header" class="CTConfirmPopTitleBlock">
                <h3>
                </h3>
            </div>
        </icrop:PopOver>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <div id="FooterCustomButton" style="float:right; margin-right:20px;">
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START--%>
        <%--<asp:Button ID="HiddenButtonSuspendWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <%--<asp:Button ID="HiddenButtonStartCheck" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <%--<asp:Button ID="HiddenButtonStartWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <%--<asp:Button ID="HiddenButtonFinishWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <%--<asp:Button ID="HiddenButtonJobStop" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <%--<asp:Button ID="HiddenButtonAddWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>--%>
        <asp:Button ID="HiddenButtonSuspendWork" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonStartCheck" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonStartWork" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonFinishWork" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonJobStop" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonAddWork" runat="server" CssClass="HiddenButton" />
        <%--2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END--%>

        <p id="ButtonSuspendWork" class="footerCustomButton_SuspendWork" ><icrop:CustomLabel ID="SuspendWork" runat="server" TextWordNo="15" /></p>
        <p id="ButtonStartCheck" class="footerCustomButton_StartCheck"><icrop:CustomLabel ID="StartCheck" runat="server" TextWordNo="14" /></p>
        <p id="ButtonStartWork" class="footerCustomButton_StartWork"><icrop:CustomLabel ID="StartWork" runat="server" TextWordNo="12" /></p>
<%--        <p ID="ButtonConnectParts" class="footerCustomButton_ConnectParts"><icrop:CustomLabel ID="ConnectParts" runat="server" TextWordNo="13" /></p>--%>
<%--        <p id="ButtonFinishWork" class="footerCustomButton_FinishWork"><icrop:CustomLabel ID="FinishWork" runat="server" TextWordNo="27" /></p>--%>
        <p id="ButtonStopWork" class="footerCustomButton_JobStop"><icrop:CustomLabel ID="AllJobStop" runat="server" TextWordNo="42" /></p>
        <p id="ButtonAddWork" class="footerCustomButton_AddWork"><icrop:CustomLabel ID="AddWork" runat="server" TextWordNo="29" /></p>
    </div>
    <div style="clear:right;">
    </div>
</asp:Content>
