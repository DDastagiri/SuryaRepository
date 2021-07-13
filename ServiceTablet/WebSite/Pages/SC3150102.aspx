<%@ Page Language="VB" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/Master/NoHeaderMasterPage.Master" CodeFile="SC3150102.aspx.vb" Inherits="Pages_SC3150102" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3150102/SC3150102.css?20191219000000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3150102/SC3150102.flickable.js?20141003130000"></script>
    <script type="text/javascript" src="../Scripts/SC3150102/SC3150102.js?20200221150000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<asp:ScriptManager ID="AjaxListManager" runat="server" EnablePageMethods="True" ></asp:ScriptManager>
<div id="contents">
<%-- ここからR/O情報欄に対するフィルター --%>
<div class="stc01Box03Filter">
<asp:HiddenField ID="Hidden01Box03Filter" runat="server" />
</div>
<%-- ここまでR/O情報欄に対するフィルター --%>
<%-- ここからR/O情報欄 --%>
<div class="stc01Box03">
    <asp:HiddenField ID="HiddenFieldOrderStatus" runat="server" />
    <%--2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START--%>
    <asp:HiddenField ID="HiddenFieldInspectionApprovalFlag" runat="server" />
    <%--2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END--%>
    <%-- ここから基本情報・ご用命事項・作業内容パネル--%>
    <asp:HiddenField ID="HiddenFieldSAName" runat="server" />
    <asp:HiddenField ID="HiddenFieldTactSrvAddSeq" runat="server" />
    <asp:HiddenField ID="HiddenFieldCutReqIframeUrl" runat="server" />
    <asp:HiddenField ID="HiddenFieldCutDtlIframeUrl" runat="server" />
    <asp:HiddenField ID="HiddenBreakPopupChild" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenSelectedJobInstructId" runat="server" />
    <asp:HiddenField ID="HiddenSelectedJobInstructSeq" runat="server"  />
    <asp:HiddenField ID="HiddenChildPushedFooter" runat="server"  />
    <asp:HiddenField ID="HiddenChildStopReasonType" runat="server"  />
    <asp:HiddenField ID="HiddenChildStopTime" runat="server"  />
    <asp:HiddenField ID="HiddenChildStopMemo" runat="server"  />
    <asp:HiddenField ID="HiddenHasStopJobValue" runat="server"  />
    <asp:HiddenField ID="HiddenStallUseStatus" runat="server" />
    <asp:HiddenField ID="HiddenRefreshFlg" runat="server" />
    <div class="Box03In">
        <div class="TabButtonSet">
        <ul>
            <%-- 基本情報 --%>
            <li class="TabButton01">
                <div class="Button">
                    <icrop:CustomLabel ID="CustomerLiteral101" runat="server" TextWordNo="101" UseEllipsis="true" Width="300px" />
                </div>
            </li>
            <%-- ご用命事項 --%>
            <li class="TabButton02">
                <div class="Rollover">
                    <icrop:CustomLabel ID="CustomerLiteral201" runat="server" TextWordNo="201" UseEllipsis="true" Width="300px" />
                </div>
            </li>
            <%-- 作業内容 --%>
            <li class="TabButton03">
                <div class="Button">
                    <icrop:CustomLabel ID="CustomerLiteral301" runat="server" TextWordNo="301" UseEllipsis="true" Width="300px" />
                </div>
            </li>
        </ul>
        </div>
        <%-- ここから基本情報パネル --%>
        <div class="TabBox01">
        <div class="S-TC-05">
            <div class="S-TC-05Left">
                <%--顧客情報--%>
				<div class="S-TC-05Left1-1">
                    <h2><icrop:CustomLabel ID="CustomerLiteral102" runat="server" TextWordNo="102" UseEllipsis="true" Width="160px" /></h2>
					<div class="S-TC-05Left1-1Wrap">
                        <dl class="S-TC-05Left1-2">
                            <%--オーナー名--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral103" runat="server" TextWordNo="103" UseEllipsis="true" Width="64px" /></dt>
                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                            <%-- <dd><icrop:CustomLabel ID="LblBuyerName" runat="server" Mode="Encode" UseEllipsis="true" Width="84px"/></dd> --%>
                            <dd><icrop:CustomLabel ID="LblBuyerName" runat="server" Mode="Encode" UseEllipsis="true" Width="71px"/>
                                <icrop:CustomLabel ID="Lmark" runat="server" TextWordNo="10006" class="Lmark" visible = "false" />
                            </dd>
                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                        </dl>
						<dl class="S-TC-05Left1-3">
                            <%--来店者名--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral104" runat="server" TextWordNo="104" UseEllipsis="true" Width="70px" /></dt>
                            <dd><icrop:CustomLabel ID="LblOrderCustomerName" runat="server" Mode="Encode" UseEllipsis="true" Width="71px" /></dd>
                        </dl>
	                    <dl class="S-TC-05Left1-4">
                            <dt>
                                <icrop:CustomLabel ID="LblMakerVehicleGrade" runat="server" Mode="Encode" UseEllipsis="true" Width="289px" />
                            </dt>
                        </dl>
	                    <dl class="S-TC-05Left1-5">
                            <%--VIN--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral105" runat="server" TextWordNo="105" UseEllipsis="true" Width="85px" /></dt>
                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                            <%-- <dd><icrop:CustomLabel ID="LblOrderVinNo" runat="server" Mode="Encode" UseEllipsis="true" Width="204px" /></dd> --%>
                            <dd><icrop:CustomLabel ID="LblOrderVinNo" runat="server" Mode="Encode" UseEllipsis="true" Width="140px" />
                                <icrop:CustomLabel ID="Pmark" runat="server" TextWordNo="10005" class="Pmark" visible = "false" />
                                <icrop:CustomLabel ID="Tmark" runat="server" TextWordNo="10004" class="Tmark" visible = "false" />
                                <icrop:CustomLabel ID="Emark" runat="server" TextWordNo="10003" class="Emark" visible = "false" />
                                <icrop:CustomLabel ID="Bmark" runat="server" TextWordNo="10002" class="Bmark" visible = "false" />
                                <icrop:CustomLabel ID="Mmark" runat="server" TextWordNo="10001" class="Mmark" visible = "false" />
                            </dd>
                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                        </dl>
	                    <dl class="S-TC-05Left1-6">
                            <%--登録番号--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral106" runat="server" TextWordNo="106" UseEllipsis="true" Width="85px" /></dt>
                            <dd><icrop:CustomLabel ID="LblOrderRegisterNo" runat="server" Mode="Encode" UseEllipsis="true" Width="204px" /></dd>
                         </dl>
	                    <dl class="S-TC-05Left1-7">
                            <%--納車日--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral107" runat="server" TextWordNo="107" UseEllipsis="true" Width="85px" /></dt>
                            <dd><icrop:CustomLabel ID="LblDeliverDate" runat="server" UseEllipsis="true" Width="204px" /></dd>
                        </dl>
	                    <dl class="S-TC-05Left1-8">
                            <%--走行距離--%>
                            <dt><icrop:CustomLabel ID="CustomerLiteral108" runat="server" TextWordNo="108" UseEllipsis="true" Width="85px" /></dt>
                            <dd><icrop:CustomLabel ID="LblOrderMileage" runat="server" Mode="Encode" UseEllipsis="true" Width="204px" /></dd>
                        </dl>
					</div>
				</div>
                <%--初期状態--%>
				<div class="S-TC-05Left2-1">
                    <h2><icrop:CustomLabel ID="CustomerLiteral110" runat="server" TextWordNo="110" UseEllipsis="true" Width="160px" /></h2>
					<%--<div class="S-TC-05Left2-1Wrap">
                        <asp:HiddenField ID="HiddenField05_Fuel" runat="server" />
						<dl class="S-TC-05Left2-2">--%>
                            <%--燃料--%>
							<%--<dt><icrop:CustomLabel ID="CustomerLiteral111" runat="server" TextWordNo="111" UseEllipsis="true" Width="85px" /></dt>
							<dd><icrop:CustomLabel ID="CustomerLiteral112" runat="server" TextWordNo="112" UseEllipsis="true" Width="15px" /></dd>
							<dd>
								<ul class="S-TC-05Left2-3">
									<li id="TC05_Fuel01" class="S-TC-05Left2-3-1Off"></li>
									<li id="TC05_Fuel02" class="S-TC-05Left2-3-2Off"></li>
									<li id="TC05_Fuel03" class="S-TC-05Left2-3-3Off"></li>
									<li id="TC05_Fuel04" class="S-TC-05Left2-3-4Off"></li>
								</ul>
                            </dd>
							<dd><icrop:CustomLabel ID="CustomerLiteral113" runat="server" TextWordNo="113" UseEllipsis="true" Width="15px" /></dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_Audio" runat="server" />
						<dl class="S-TC-05Left2-5">--%>
                            <%--オーディオ--%>
							<%--<dt><icrop:CustomLabel ID="CustomerLiteral114" runat="server" TextWordNo="114" UseEllipsis="true" Width="85px" /></dt>
							<dd>
								<ul class="S-TC-05Left2-6">
									<li id="TC05_AudioOff" class="S-TC-05Left2-6-1Off"><icrop:CustomLabel ID="CustomerLiteral115" runat="server" TextWordNo="115" UseEllipsis="true" Width="58px" /></li>
									<li id="TC05_AudioCD"  class="S-TC-05Left2-6-2Off"><icrop:CustomLabel ID="CustomerLiteral116" runat="server" TextWordNo="116" UseEllipsis="true" Width="60px" /></li>
									<li id="TC05_AudioFM"  class="S-TC-05Left2-6-3Off"><icrop:CustomLabel ID="CustomerLiteral117" runat="server" TextWordNo="117" UseEllipsis="true" Width="59px" /></li>
								</ul>
							</dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_AirConditioner" runat="server" />
						<dl class="S-TC-05Left2-7">--%>
                            <%--エアコン--%>
							<%--<dt><icrop:CustomLabel ID="CustomerLiteral118" runat="server" TextWordNo="118" UseEllipsis="true" Width="85px" /></dt>
							<dd>
								<ul class="S-TC-05Left2-8">
									<li id="TC05_AirConditionerOff" class="S-TC-05Left2-8-1Off"><icrop:CustomLabel ID="CustomerLiteral119" runat="server" TextWordNo="119" UseEllipsis="true" Width="58px" /></li>
									<li id="TC05_AirConditionerOn"  class="S-TC-05Left2-8-2Off"><icrop:CustomLabel ID="CustomerLiteral120" runat="server" TextWordNo="120" UseEllipsis="true" Width="60px" /></li>
								</ul>
							</dd>
							<dd>
                                <icrop:CustomLabel ID="LiteralAirConditionerTemperature" runat="server" UseEllipsis="true" Width="70px" />
                            </dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_Accessory1" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory2" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory3" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory4" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory5" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory6" runat="server" />
						<ul class="S-TC-05Left2-9">--%>
                            <%--付属品--%>
							<%--<li id="TC05_Accessory1" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral122" runat="server" TextWordNo="122" UseEllipsis="true" Width="48px" /></li>
							<li id="TC05_Accessory2" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral123" runat="server" TextWordNo="123" UseEllipsis="true" Width="48px" /></li>
							<li id="TC05_Accessory3" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral124" runat="server" TextWordNo="124" UseEllipsis="true" Width="48px" /></li>
							<li id="TC05_Accessory4" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral125" runat="server" TextWordNo="125" UseEllipsis="true" Width="48px" /></li>
							<li id="TC05_Accessory5" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral126" runat="server" TextWordNo="126" UseEllipsis="true" Width="48px" /></li>
							<li id="TC05_Accessory6" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral127" runat="server" TextWordNo="127" UseEllipsis="true" Width="48px" /></li>
						</ul>
	                    <dl class="S-TC-05Left2-10">--%>
                            <%--貴重品--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral128" runat="server" TextWordNo="128" UseEllipsis="true" Width="85px" /></dt>
                            <dd><icrop:CustomLabel ID="LblValuablesMemo" runat="server" Mode="Encode" UseEllipsis="true" Width="204px" /></dd>
                        </dl>
					</div>--%>
                   <iframe id = "CST_DETAIL_IFRAME" src ="" class="S-TC-05Left2-1Iframe" runat="server" seamless="seamless" scrolling="no"></iframe>
				</div>
            </div>
            <%--入庫履歴--%>
            <div class="S-TC-05Right">
				<h2><icrop:CustomLabel ID="CustomerLiteral129" runat="server" TextWordNo="129" UseEllipsis="true" Width="160px" /></h2>
				<div class="S-TC-05RightWrap">
                    <div class="DisabledDiv" id="DisabledDiv" style="display:none;" ></div>
					<div id="S-TC-05RightScroll" class="S-TC-05RightScroll">
                        <asp:UpdatePanel ID="AjaxHistoryPanel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Repeater ID="RepeaterHistoryInfo" runat="server">
                                    <ItemTemplate>
                                        <dl class="S-TC-05Right1-1">
	                                        <dt><img alt ="" src="../Styles/Images/SC3150102/ico59.png" width="15" height="16"></dt>
	                                        <dd>
                                                <span><asp:Literal ID="LiteralHAcceptDate" runat="server" ></asp:Literal>&nbsp;</span><br />
                                                <asp:Literal ID="LiteralHOrderNo" runat="server" ></asp:Literal>&nbsp;
                                            </dd>
	                                        <dd><asp:Literal ID="LiteralHTypicalSrvTypeName" runat="server" ></asp:Literal>&nbsp;</dd>
	                                        <dd><asp:Literal ID="LiteralHTypicalSrvType" runat="server" ></asp:Literal>&nbsp;</dd>
	                                        <dd><asp:Literal ID="LiteralHSaName" runat="server" ></asp:Literal>&nbsp;</dd>

                                            <asp:HiddenField ID="HiddenFieldHOrderNo" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SVCIN_NUM")) %>' />
                                            <asp:HiddenField ID="HiddenFieldHDealerCode" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("DLR_CD")) %>' />
                                            <asp:HiddenField ID="HiddenFieldServiceInNumber" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SVCIN_NUM")) %>' />
	                                    </dl>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <div class="S-TC-05Right2-1" id="AllDispLinkDiv" runat="server" style="display:none;" >
                                    <asp:LinkButton runat="server" ID="AllDispLink" OnClientClick="clickAllLink();" />
                                </div>
                                <div class="S-TC-05Right2-2" id="NextDispLinkDiv" runat="server" style="display:none;" >
                                    <asp:LinkButton runat="server" ID="NextDispLink" OnClientClick="clickNextLink();" />
                                </div>
                                <div class="S-TC-05Right-NextLoding" id="NextLodingDiv" runat="server" style="display:none;" >
                                    <div class="LoadImage"></div>
                                    <icrop:CustomLabel ID="NextLodingDivLabel" runat="server" TextWordNo="135" UseEllipsis="true" />
                                </div>
                                <asp:HiddenField ID="HiddenFieldOtherHistoryDispPageCount" runat="server" Value="0" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="AllDispLink" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="NextDispLink" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
					</div>
				</div>
            </div>
        </div>
        </div>
        <%-- ここまで基本情報パネル --%>

        <%-- ここからご用命事項パネル --%>
        <div class="TabBox02">
            <div class="S-TC-07TabWrap">
           <%-- <div class="S-TC-07Left">
                <h2><icrop:CustomLabel ID="CustomerLiteral201Second" runat="server" TextWordNo="283" UseEllipsis="true" Width="160px" /></h2>
                <p id="S-TC-07LeftMemo2" class="S-TC-07LeftMemo2"><asp:Literal ID="LiteralOrderMemo" runat="server" ><%--Mode="Encode"--%><%--</asp:Literal></p>
            </div>--%>

           <%-- <div class="S-TC-07Right">
                <div class="S-TC-07RightTab">
                    <ul>
                        <li id="S-TC-07RightTab_01"><icrop:CustomLabel ID="CustomerLiteral202" runat="server" TextWordNo="202" UseEllipsis="true" Width="80px" /></li>
                        <li id="S-TC-07RightTab_02" class="S-TC-07RightTabNoSelected"><icrop:CustomLabel ID="CustomerLiteral203" runat="server" TextWordNo="203" UseEllipsis="true" Width="80px" /></li>
                    </ul>
                </div>
                <div id="S-TC-07RightTabWrap" class="S-TC-07RightTabWrap">--%>
                    <%-- 確認事項 --%>
                    <%--<div id="S-TC-07RightBody" class="S-TC-07RightBody" >
                    <div id="S-TC-07RightBodyFlick" class="S-TC-07RightBodyFlick">
                        <asp:HiddenField ID="HiddenField07_ExchangeParts" runat="server" />
                        <dl>--%>
                            <%--交換部品--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral204" runat="server" TextWordNo="204" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_ExchangeParts1" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral205" runat="server" TextWordNo="205" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_ExchangeParts2" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral206" runat="server" TextWordNo="206" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_ExchangeParts3" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral207" runat="server" TextWordNo="207" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Waiting" runat="server" />
                        <dl>--%>
                            <%--待ち方--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral208" runat="server" TextWordNo="208" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_WaitingIn"  class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral209" runat="server" TextWordNo="209" UseEllipsis="true" Width="163px" /></dd>
                            <dd id="TC07_WaitingOut" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral210" runat="server" TextWordNo="210" UseEllipsis="true" Width="163px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Washing" runat="server" />
                        <dl>--%>
                            <%--洗車--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral211" runat="server" TextWordNo="211" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_WashingDo"   class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral212" runat="server" TextWordNo="212" UseEllipsis="true" Width="163px" /></dd>
                            <dd id="TC07_WashingNone" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral213" runat="server" TextWordNo="213" UseEllipsis="true" Width="163px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Payment" runat="server" />
                        <dl>--%>
                            <%--支払方法--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral214" runat="server" TextWordNo="214" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_PaymentCash"  class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral215" runat="server" TextWordNo="215" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_PaymentCard"  class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral216" runat="server" TextWordNo="216" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_PaymentOther" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral217" runat="server" TextWordNo="217" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                        <dl>--%>
                            <%--領収書宛先--%>
                            <%--<dt><span><icrop:CustomLabel ID="CustomerLiteral223" runat="server" TextWordNo="223" UseEllipsis="true" Width="87px" /></span></dt>
                            <dd class="S-TC-07Right05-1">
                                <div class="S-TC-07Right05-1Memo">
                                    <div class="S-TC-07Right05-1MemoBg">
                                        <asp:Literal ID="LiteralInvoiceAddress" runat="server"></asp:Literal>&nbsp;
                                    </div>
                                </div>
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Csi" runat="server" />
                        <dl>--%>
                            <%--CSI時間--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral224" runat="server" TextWordNo="224" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_CSI_AM"     class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral225" runat="server" TextWordNo="225" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_CSI_PM"     class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral226" runat="server" TextWordNo="226" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_CSI_Always" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral227" runat="server" TextWordNo="227" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                    </div>
                    </div>--%>
                    <%-- 問診項目 --%>
                   <%-- <div id="S-TC-07RightScroll" class="S-TC-07RightScroll" >
                    <div id="S-TC-07RightScrollFlick" class="S-TC-07RightScrollFlick" >
                        <asp:HiddenField ID="HiddenField07_Warning" runat="server" />
                        <dl>--%>
                            <%--WING--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral228" runat="server" TextWordNo="228" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_WNG_Always" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral229" runat="server" TextWordNo="229" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_WNG_Often"  class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral230" runat="server" TextWordNo="230" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_WNG_None"   class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral231" runat="server" TextWordNo="231" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Occurrence" runat="server" />
                        <dl>--%>
                            <%--故障発生時間--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral232" runat="server" TextWordNo="232" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Occurrence_Recently" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral233" runat="server" TextWordNo="233" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_Occurrence_Week"     class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral234" runat="server" TextWordNo="234" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_Occurrence_Other"    class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral235" runat="server" TextWordNo="235" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Frequency" runat="server" />
                        <dl>--%>
                            <%--故障発生頻度--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral236" runat="server" TextWordNo="236" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Frequency_High"  class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral237" runat="server" TextWordNo="237" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_Frequency_Often" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral238" runat="server" TextWordNo="238" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_Frequency_Once"  class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral239" runat="server" TextWordNo="239" UseEllipsis="true" Width="109px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Reappear" runat="server" />
                        <dl>--%>
                            <%--再現可能--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral240" runat="server" TextWordNo="240" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Reappear_Yes" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral241" runat="server" TextWordNo="241" UseEllipsis="true" Width="163px" /></dd>
                            <dd id="TC07_Reappear_No"  class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral242" runat="server" TextWordNo="242" UseEllipsis="true" Width="165px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_WaterT" runat="server" />
                        <dl>--%>
                            <%--水温--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral243" runat="server" TextWordNo="243" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_WaterT_Low"  class="S-TC-07Right03-1Off"><icrop:CustomLabel ID="CustomerLiteral244" runat="server" TextWordNo="244" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_WaterT_High" class="S-TC-07Right03-2Off"><icrop:CustomLabel ID="CustomerLiteral245" runat="server" TextWordNo="245" UseEllipsis="true" Width="109px" /></dd>
                            <dd class="S-TC-07Right03-3">
                                <icrop:CustomLabel ID="CustomLabelHearingWTemperature" runat="server" UseEllipsis="true" Width="80px" />
                                <icrop:CustomLabel ID="CustomerLiteral246" runat="server" TextWordNo="246" UseEllipsis="true" Width="19px" />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Temperature" runat="server" />
                        <dl>--%>
                            <%--気温--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral247" runat="server" TextWordNo="247" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Temperature_Low"  class="S-TC-07Right03-1Off"><icrop:CustomLabel ID="CustomerLiteral248" runat="server" TextWordNo="248" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_Temperature_High" class="S-TC-07Right03-2Off"><icrop:CustomLabel ID="CustomerLiteral249" runat="server" TextWordNo="249" UseEllipsis="true" Width="109px" /></dd>
                            <dd class="S-TC-07Right03-3">
                                <icrop:CustomLabel ID="CustomLabelHearingTemperature" runat="server" UseEllipsis="true" Width="80px" />
                                <icrop:CustomLabel ID="CustomerLiteral246Second" runat="server" TextWordNo="284" UseEllipsis="true" Width="19px"  />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Place" runat="server" />
                        <dl>--%>
                            <%--発生場所--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral250" runat="server" TextWordNo="250" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Place_Parking"  class="S-TC-07Right04-1Off"><icrop:CustomLabel ID="CustomerLiteral251" runat="server" TextWordNo="251" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_Place_Ordinary" class="S-TC-07Right04-2Off"><icrop:CustomLabel ID="CustomerLiteral252" runat="server" TextWordNo="252" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_Place_Motorway" class="S-TC-07Right04-3Off"><icrop:CustomLabel ID="CustomerLiteral253" runat="server" TextWordNo="253" UseEllipsis="true" Width="82px" /></dd>
                            <dd id="TC07_Place_Slope"    class="S-TC-07Right04-4Off"><icrop:CustomLabel ID="CustomerLiteral254" runat="server" TextWordNo="254" UseEllipsis="true" Width="82px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_TrafficJam" runat="server" />
                        <dl>--%>
                            <%--渋滞状況--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral255" runat="server" TextWordNo="255" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_Trafficjam_Happen" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral256" runat="server" TextWordNo="256" UseEllipsis="true" Width="163px" /></dd>
                            <dd id="TC07_Trafficjam_None"   class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral257" runat="server" TextWordNo="257" UseEllipsis="true" Width="165px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_CarStatus_Startup" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Idling" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Cold" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Warm" runat="server" />
                        <dl>--%>
                            <%--車両状態--%>
                           <%-- <dt><icrop:CustomLabel ID="CustomerLiteral258" runat="server" TextWordNo="258" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_CarStatus_Startup" class="S-TC-07Right04-1Off"><icrop:CustomLabel ID="CustomerLiteral259" runat="server" TextWordNo="259" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_CarStatus_Idlling" class="S-TC-07Right04-2Off"><icrop:CustomLabel ID="CustomerLiteral260" runat="server" TextWordNo="260" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_CarStatus_Cold"    class="S-TC-07Right04-3Off"><icrop:CustomLabel ID="CustomerLiteral261" runat="server" TextWordNo="261" UseEllipsis="true" Width="82px" /></dd>
                            <dd id="TC07_CarStatus_Warm"    class="S-TC-07Right04-4Off"><icrop:CustomLabel ID="CustomerLiteral262" runat="server" TextWordNo="262" UseEllipsis="true" Width="82px" /></dd>
                        </dl>
                        <dl>--%>
                            <%--<dt>&nbsp;</dt>--%>
                           <%-- <dd id="S-SA-07Tab02Right1-5-1" class="S-SA-07Tab02Right1-5-1Off" style="margin-left: 87px;"><icrop:CustomLabel ID="CustomerLiteral263" runat="server" TextWordNo="263" UseEllipsis="true" Width="328px" /></dd>
                        </dl>--%>
						<%-- 「走行時」タップ時に表示される項目 START --%>
                        <%--<div id="S-SA-07Tab02Right1-5-1Display" class="S-SA-07Tab02Right1-5-1Display">
                            <asp:HiddenField ID="HiddenField07_Traveling" runat="server" />
						    <div class="S-TC-07RightList">
						        <ul>
						        <li id="TC07_Traveling_Lowspeed" ><icrop:CustomLabel ID="CustomerLiteral264" runat="server" TextWordNo="264" /></li>
						        <li id="TC07_Traveling_Acceleration" ><icrop:CustomLabel ID="CustomerLiteral265" runat="server" TextWordNo="265" /></li>
						        <li id="TC07_Traveling_Slowdown" ><icrop:CustomLabel ID="CustomerLiteral266" runat="server" TextWordNo="266" /></li>
						        </ul>
						    </div>
                        </div>--%>
                        <%-- 「走行時」タップ時に表示される項目 END --%>

                        <%-- 仕様変更書_20120309_(TC問診表画面にボタン追加) START --%>
                        <%--<asp:HiddenField ID="HiddenField07_CarStatus_Parking" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Advance" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_ShiftChange" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Back" runat="server" />
                        <dl>--%>
                            <%--<dt>&nbsp;</dt>--%>
                            <%--<dd id="TC07_CarControl1_Parking"     class="S-TC-07Right04-1Off" style="margin-left: 87px;"><icrop:CustomLabel ID="CustomerLiteral267" runat="server" TextWordNo="267" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_CarControl1_Advance"     class="S-TC-07Right04-2Off"><icrop:CustomLabel ID="CustomerLiteral268" runat="server" TextWordNo="268" UseEllipsis="true" Width="81px" /></dd>
                            <dd id="TC07_CarControl1_ShiftChange" class="S-TC-07Right04-3Off"><icrop:CustomLabel ID="CustomerLiteral269" runat="server" TextWordNo="269" UseEllipsis="true" Width="82px" /></dd>
                            <dd id="TC07_CarControl1_Back"        class="S-TC-07Right04-4Off"><icrop:CustomLabel ID="CustomerLiteral270" runat="server" TextWordNo="270" UseEllipsis="true" Width="82px" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_CarStatus_Brake" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Detour" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_SteeringWheel" runat="server" />
                        <dl>--%>
                            <%--<dt>&nbsp;</dt>--%>
                           <%-- <dd id="TC07_CarControl2_Brake"         class="S-TC-07Right01-1Off" style="margin-left: 87px;"><icrop:CustomLabel ID="CustomerLiteral271" runat="server" TextWordNo="271" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_CarControl2_Detour"        class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral272" runat="server" TextWordNo="272" UseEllipsis="true" Width="109px" /></dd>
                            <dd id="TC07_CarControl2_SteeringWheel" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral319" runat="server" TextWordNo="322" UseEllipsis="true" Width="109px" /></dd>
                        </dl>--%>
                        <%-- 仕様変更書_20120309_(TC問診表画面にボタン追加) END --%>
                        <%--<dl>--%>
                            <%--スピード--%>
                          <%--  <dt><icrop:CustomLabel ID="CustomerLiteral273" runat="server" TextWordNo="273" UseEllipsis="true" Width="87px" /></dt>
                            <dd class="S-TC-07Right06-1">
                                <asp:TextBox ID="TextBoxHearingSpeedRate" runat="server" ReadOnly="true"></asp:TextBox>
                                <icrop:CustomLabel ID="CustomerLiteral274" runat="server" TextWordNo="274" UseEllipsis="true" Width="76px" />
                            </dd>--%>
                            <%--ギア--%>
                           <%-- <dd class="S-TC-07Right06-2">
                                <asp:TextBox ID="TextBoxHearingSpeedGear" runat="server" ReadOnly="true" ></asp:TextBox>
                                <icrop:CustomLabel ID="CustomerLiteral275" runat="server" TextWordNo="275" UseEllipsis="true" Width="38px" />
                            </dd>
                        </dl>
                        <dl>--%>
                            <%--乗車人数--%>
                            <%--<dt><icrop:CustomLabel ID="CustomerLiteral276" runat="server" TextWordNo="276" UseEllipsis="true" Width="87px" /></dt>
                            <dd class="S-TC-07Right07-1">
                                <asp:TextBox ID="TextBoxHearingPeopleNumber" runat="server" ReadOnly="true" ></asp:TextBox>
                                <icrop:CustomLabel ID="CustomerLiteral277" runat="server" TextWordNo="277" UseEllipsis="true" Width="31px" />
                            </dd>--%>
                            <%--過重--%>
                           <%-- <dd class="S-TC-07Right07-2">
                                <icrop:CustomLabel ID="CustomerLiteral279" runat="server" TextWordNo="279" UseEllipsis="true" Width="25px" />
                                <asp:TextBox ID="TextBoxHearingPeopleTooHeavy" runat="server" ReadOnly="true"></asp:TextBox>
                                <icrop:CustomLabel ID="CustomerLiteral278" runat="server" TextWordNo="278" UseEllipsis="true" Width="30px" />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_NonGenuine" runat="server" />
                        <dl>--%>
                            <%--非純正用品--%>
                         <%--   <dt><icrop:CustomLabel ID="CustomerLiteral280" runat="server" TextWordNo="280" UseEllipsis="true" Width="87px" /></dt>
                            <dd id="TC07_NonGenuine_Yes" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral281" runat="server" TextWordNo="281" UseEllipsis="true" Width="163px" /></dd>
                            <dd id="TC07_NonGenuine_No"  class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral282" runat="server" TextWordNo="282" UseEllipsis="true" Width="163px" /></dd>
                        </dl>--%>
                        
                    <%--</div>
                    </div>
                </div>
            </div>--%>
                <iframe id = "CST_REQUEST_IFRAME" src="" class="S-TC-07Main" runat="server" seamless="seamless" scrolling="no"></iframe>
            </div>
        </div>
        <%-- ここまでご用命事項パネル --%>

        <%-- ここから作業内容パネル --%>
        <div class="TabBox03">
            <div class="S-TC-01">
				<div class="S-TC-01Wrap">
					<h2 class="contentTitle"><icrop:CustomLabel ID="CustomerLiteral302" runat="server" TextWordNo="302" UseEllipsis="true" Width="180px" /></h2>
                    <asp:HiddenField ID="HiddenFieldAddWorkCount" runat="server" />
                    <asp:HiddenField ID="HiddenFieldRepairOrderInitialWord" runat="server" />

                    <%-- FM呼出 --%>
                    <%--<icrop:CustomButton ID="ButtonSendNoticeToFM" runat="server" TextWordNo="330" OnClientClick="parent.LoadingScreen(); parent.reloadPageIfNoResponse(); return true;"/>--%>
                    
					<%--<ul id="S-TC-01Paging" class="S-TC-01Paging">
                        <li class="liFast"></li>
                        <li class="liScroll">
                            <div id="divScroll">
                                <div class="S-TC-01PagingOn"><icrop:CustomLabel ID="CustomerLiteral316" runat="server" TextWordNo="316" /></div>
						        <div class="S-TC-01PagingOff"><span>1</span></div>
						        <div class="S-TC-01PagingOff"><span>2</span></div>
                                <div class="S-TC-01PagingOff"><span>3</span></div>
                            </div>
                        </li>
                        <li class="liLast"></li>
					</ul>--%>

					<div class="S-TC-01Wrap2">
						<div class="S-TC-01Left">
							<h3><icrop:CustomLabel ID="CustomerLiteral303" runat="server" TextWordNo="303"  CssClass="Ellipsis"/></h3>
							<dl class="S-TC-01LeftHead">
								<dt><icrop:CustomLabel ID="CustomerLiteral304" runat="server" TextWordNo="304" Width="28px"  CssClass="Ellipsis" /></dt>
								<dd>&nbsp;</dd>
                                <dd class="TitleSet03">
                                    <div class="STtl01"><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="305" Width="122px" CssClass="Ellipsis" /></div>
                                    <div class="BorderBox">
                                        <div class="STtl02"><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="306" Width="73px" CssClass="Ellipsis" /></div>
                                        <div class="STtl03"><icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="308" Width="73px" CssClass="Ellipsis" /></div>
                                    </div>
                                </dd>
                                <dd class="TitleSet04">
                                    <%-- 2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない START --%>
                                    <%--<div class="STtl01"><icrop:CustomLabel ID="CustomerLiteral327" runat="server" TextWordNo="333" Width="82px" CssClass="Ellipsis" /></div>--%>
                                    <%--<div class="STtl02"><icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="334" Width="82px" CssClass="Ellipsis" /></div>--%>
                                    <icrop:CustomLabel ID="CustomerLiteral327" runat="server" TextWordNo="333" Width="82px" CssClass="Ellipsis" />
                                    <%-- 2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない END --%>
                                </dd>
                                <dd>
                                    <icrop:CustomLabel ID="StartSingleJob" runat="server" TextWordNo="340" Width="144px" CssClass="Ellipsis" />
                                </dd>
							</dl>
							<div id="S-TC-01LeftBody" class="S-TC-01LeftScroll">
                                <table border="0" cellspacing="0" cellpadding="0" class="S-TC-01DataTable">
                                <asp:Repeater ID="RepeaterWorkInfo" runat="server">
                                    <ItemTemplate>
                                        <tr id="WorkInfoRow" runat="server" class="S-TC-01LeftScrollBase">
                                           <td>
                                             <div class="InnerBox w01 fst ">
                                               <icrop:CustomLabel ID="LabelWorkNo" runat="server"/>
                                             </div>
                                           </td>
                                           <td>
                                             <div class="InnerBox w02">
                                                <div id = "RepairOrderIcon" class="imgicon01"></div>
                                                    <asp:HiddenField ID="HiddenFieldHOrderNo" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("RO_NUM")) %>' />
                                                    <asp:HiddenField ID="HiddenFieldHOrderNoSeq" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("RO_SEQ")) %>' />
                                             </div>
                                           </td>
                                           <td>
                                             <div class="InnerBox w03">
                                               <div class="Textbox01">
                                                 <icrop:CustomLabel ID="LabelSrvName" runat="server"  Width="123px" CssClass="Ellipsis"/>
                                               </div>
                                               <div class="TextBoxSet">
                                                 <div class="Textbox02">
                                                    <icrop:CustomLabel ID="LabelWorkHours" runat="server" width= "68px"  CssClass="Ellipsis"/>
                                                 </div>
                                                 <div class="Textbox03">
                                                    <icrop:CustomLabel ID="LabelSellWorkPrice" runat="server" width= "72px"  CssClass="Ellipsis"/>
                                                 </div>
                                               </div>
                                             </div>
                                           </td>
                                           <td>
                                             <div class="InnerBox w04">
	                                           <%-- 2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない START --%>
                                               <%--<div class="Textbox01">--%>
                                                 <%--<icrop:CustomLabel ID="LabelStallInfo" runat="server"  CssClass="Ellipsis"/>--%>
                                               <%--</div>--%>
                                               <%--<div class="Textbox02">--%>
                                                   <%--<icrop:CustomLabel ID="LabelWorkgroupInfo" runat="server"  Width="88px" CssClass="Ellipsis"/>--%>
                                               <%--</div>--%>
                                               <icrop:CustomLabel ID="LabelStallInfo" runat="server"  CssClass="Ellipsis"/>
                                               <%-- 2019/12/19 NSK夏目 TR-SVT-TKM-20191209-001 Technician Main Menuにテクニシャン名が表示されない END --%>
                                             </div>
                                           </td>
                                           <td>
                                             <div class="InnerBox w05">
                                               <div class="Textbox01" id = "Textbox01">
                                                 <%-- 開始ボタン --%>
                                                 <div class="BtnOn" id="JobStartButton">
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される START --%>
                                                    <%--<icrop:CustomLabel ID="StartSingleJob" runat="server" TextWordNo="336" CssClass="Ellipsis" />--%>
                                                    <icrop:CustomLabel ID="StartSingleJob" runat="server" TextWordNo="336" CssClass="EllipsisNoToolChip" />
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される END --%>
                                                 </div>
                                               </div>
                                               <div class="Textbox02" id = "Textbox02">
                                                 <%-- 終了ボタン --%>
                                                 <div class="BtnOn" id="JobFinishButton">
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される START --%>
                                                    <%--<icrop:CustomLabel ID="FinishSingleJob" runat="server" TextWordNo="337" CssClass="Ellipsis" />--%>
                                                    <icrop:CustomLabel ID="FinishSingleJob" runat="server" TextWordNo="337" CssClass="EllipsisNoToolChip" />
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される END --%>
                                                 </div>
                                               </div>
                                               <div class="Textbox03" id = "Textbox03">
                                                 <%-- 中断ボタン --%>
                                                 <div class="BtnOn" id="JobStopButton">
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される START --%>
                                                    <%--<icrop:CustomLabel ID="StopSingleJob" runat="server" TextWordNo="338" CssClass="Ellipsis" />--%>
                                                    <icrop:CustomLabel ID="StopSingleJob" runat="server" TextWordNo="338" CssClass="EllipsisNoToolChip" />
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される END --%>
                                                 </div>
                                                 <%-- 再開ボタン --%>
                                                 <div class="BtnOn" id="JobReStartButton">
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される START --%>
                                                    <%--<icrop:CustomLabel ID="ReStartJob" runat="server" TextWordNo="339" CssClass="Ellipsis" Width="49px"/>--%>
                                                    <icrop:CustomLabel ID="ReStartJob" runat="server" TextWordNo="339" CssClass="EllipsisNoToolChip" Width="49px"/>
                                                    <%-- 2015/04/03 TMEJ 明瀬 TMT２販社号口後フォロー IDX635 ボタンタップ時に文言のツールチップが表示される END --%>
                                                 </div>
                                               </div>
                                               <asp:HiddenField ID="HiddenJobInstructId" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_ID")) %>' />
                                               <asp:HiddenField ID="HiddenJobInstructSeq" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("JOB_INSTRUCT_SEQ")) %>' />
                                               <asp:HiddenField ID="HiddenRsltSTratDatetime" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("JOB_RSLT_START_DATETIME")) %>' />
                                               <asp:HiddenField ID="HiddenRsltEndDatetime" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("JOB_RSLT_END_DATETIME")) %>' />
                                               <asp:HiddenField ID="HiddenJobStatus" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("JOB_STATUS")) %>' />
                                             </div>
                                           </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </table>
							</div>
						</div>

						<div class="S-TC-01Right">
							<h3>
                                <icrop:CustomLabel ID="CustomerLiteral311" runat="server" TextWordNo="311" />
                                <%-- カゴ番号 --%>
                                <span id="spnCageNo">
                                    <span>(</span>
                                    <icrop:CustomLabel ID="CustomerLiteral332" runat="server" TextWordNo="332" />
                                    <icrop:CustomLabel ID="lblCageNo" runat="server" CssClass="Ellipsis" Width="6px" />
                                    <span>)</span>
                                </span>
                            </h3>
							<dl class="S-TC-01RightHead">
								<dt><icrop:CustomLabel ID="CustomerLiteral304Second" runat="server" TextWordNo="319" CssClass="Ellipsis" Width="29px" /></dt>
								<%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
								<%-- <dd><icrop:CustomLabel ID="CustomerLiteral312" runat="server" TextWordNo="312" CssClass="Ellipsis" Width="125px" /></dd> --%>
								<%-- <dd><icrop:CustomLabel ID="CustomerLiteral313" runat="server" TextWordNo="313" CssClass="Ellipsis" Width="87px" /></dd> --%>
								<%--<dd><icrop:CustomLabel ID="CustomerLiteral317" runat="server" TextWordNo="317" UseEllipsis="true" Width="55px" /></dd>--%>
								<%-- <dd><icrop:CustomLabel ID="CustomerLiteral314" runat="server" TextWordNo="314" CssClass="Ellipsis" Width="97px" /></dd> --%>
								<%-- <dd><icrop:CustomLabel ID="CustomerLiteral318" runat="server" TextWordNo="318" CssClass="Ellipsis" Width="48px" /></dd> --%>
								<dd><icrop:CustomLabel ID="CustomerLiteral312" runat="server" TextWordNo="312" CssClass="Ellipsis" Width="173px" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral313" runat="server" TextWordNo="313" CssClass="Ellipsis" Width="87px" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral314" runat="server" TextWordNo="314" CssClass="Ellipsis" Width="97px" /></dd>
								<%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>
							</dl>
                            <%-- ここからR/O情報欄に対するフィルター --%>
                           <%-- <div class="S-TC-01RightScrollFilter">
                            </div>--%>
                            <asp:HiddenField ID="HiddenFieldPartsCount" runat="server" />
                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
                            <%-- <asp:HiddenField ID="HiddenFieldPartsBackOrderCount" runat="server" /> -->
                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>
                            <%-- ここまでR/O情報欄に対するフィルター --%>
							<div id="S-TC-01RightBody" class="S-TC-01RightScroll">
                                <table border="0" cellspacing="0" cellpadding="0" class="S-TC-01DataTable" >
                                <asp:Repeater ID="RepeaterPartsInfo" runat="server">                                    
                                    <ItemTemplate>
                                        <tr>
                                           <%-- <td class="InnerBox w11 fst "><asp:Literal ID="LiteralPartsNo" runat="server" Mode="Encode" ></asp:Literal></td>
                                            <td class="InnerBox w12"><asp:Literal ID="LiteralPartsName" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td class="InnerBox w13"><asp:Literal ID="LiteralPartsType" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td class="InnerBox w14"><asp:Literal ID="LiteralPartsQuantity" runat="server" Mode="Encode"></asp:Literal></td>
                                           <%-- <td><asp:Literal ID="LiteralPartsUnit" runat="server" Mode="Encode"></asp:Literal></td>--%>
                                            <%--<td class="InnerBox w15 end"><asp:Literal ID="LiteralPartsOrderStatus" runat="server" Mode="Encode"></asp:Literal></td>--%>

                                             <td class="InnerBox w11 fst ">
                                             <icrop:CustomLabel ID="LiteralPartsNo" runat="server"  Width="28px"  CssClass="Ellipsis"/>
                                              <asp:HiddenField ID="HiddenFieldPartsQuantity" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("PartsAmount")) %>' />
                                             </td>
                                            <td class="InnerBox w12">
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
                                            <%-- <icrop:CustomLabel ID="LiteralPartsName" runat="server"  Width="118px" CssClass="Ellipsis"/> --%>
                                            <icrop:CustomLabel ID="LiteralPartsName" runat="server"  Width="165px" CssClass="Ellipsis"/>
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>
                                             <asp:HiddenField ID="HiddenFieldPartsName" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("PartsName")) %>' />
                                            </td>
                                            <td class="InnerBox w13">
                                            <icrop:CustomLabel ID="LiteralPartsType" runat="server" Width="78px" CssClass="Ellipsis"/>
                                            <asp:HiddenField ID="HiddenFieldPartsType" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("PartsType")) %>' />
                                            </td>
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
                                            <%-- <td class="InnerBox w14"> --%>
                                            <td class="InnerBox w14 end">
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>
                                             <icrop:CustomLabel ID="LiteralPartsQuantity" runat="server" Width="97px" CssClass="Ellipsis"/>
                                               <asp:HiddenField ID="HiddenFieldPartsUnit" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("PartsUnit")) %>' />
                                            </td>
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START --%>
                                            <%-- <td class="InnerBox w15 end"> --%>
                                            <%--  <icrop:CustomLabel ID="LiteralPartsOrderStatus" runat="server" Width="47px" CssClass="Ellipsis"/> --%>
                                            <%--  <asp:HiddenField ID="HiddenFieldPartsOrderStatus" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("BO_Scheduled_DateTime")) %>' /> --%>
                                            <%-- </td> --%>
                                            <%-- 2019/06/03 NSK鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END   --%>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </table>
							</div>
						</div>
					</div>
				</div>
            </div>
        </div>

        
        <%-- ここまで作業内容パネル --%>

        <asp:Button ID="HiddenButtonRegisterWorkgroup" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonJobStart" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonJobFinish" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonJobStop" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonJobReStart" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonChildDoNotBreak" runat="server" CssClass="HiddenButton" />
        <asp:Button ID="HiddenButtonChildTakeBreak" runat="server" CssClass="HiddenButton" />
    </div>
    <%-- ここまで基本情報・ご用命事項・作業内容パネル --%>
</div>
<%-- ここまでR/O情報欄 --%>
</div>

<%--以下、作業グループ選択用のポップアップ--%>
<%--<icrop:PopOver ID="poWorkgroupList" runat="server" TriggerClientID="" HeaderTextWordNo="328" Width="200px" Height="200px" HeaderStyle="Text">
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
		        <ul>
                    <asp:Repeater ID="RepeaterWorkgroupInfo" runat="server">
                        <ItemTemplate>
		                    <li><%# HttpUtility.HtmlEncode(Eval("WORKGROUPNAME")) %></li>
                            <asp:HiddenField ID="HiddenFieldWorkgroupCode" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("WORKGROUPCODE")) %>' />
                        </ItemTemplate>
                    </asp:Repeater>
		        </ul>
	        </div>
        </div>
    </div>
</icrop:PopOver>--%>

</asp:Content>
