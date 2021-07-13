<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/NoHeaderMasterPage.Master" CodeFile="SC3150102.aspx.vb" Inherits="Pages_SC3150102" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/SC3150102/SC3150102.css?201202201720" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3150102/SC3150102.flickable.js"></script>
    <script type="text/javascript" src="../Scripts/SC3150102/SC3150102.js?201202171530"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<div id="contents">
<!-- ここからR/O情報欄に対するフィルター -->
<div class="stc01Box03Filter">
<asp:HiddenField ID="Hidden01Box03Filter" runat="server" />
</div>
<!-- ここまでR/O情報欄に対するフィルター -->
<!-- ここからR/O情報欄 -->
<div class="stc01Box03">
    <asp:HiddenField ID="HiddenFieldOrderStatus" runat="server" />
    <!-- ここから基本情報・ご用命事項・作業内容パネル-->
    <asp:HiddenField ID="HiddenFieldSAName" runat="server" />
    <div class="Box03In">
        <div class="TabButtonSet">
        <ul>
            <!-- 基本情報 -->
            <li class="TabButton01">
                <div class="Button">
                    <%--<asp:Label ID="LabelBasicTab" runat="server" ></asp:Label>--%>
                    <icrop:CustomLabel ID="CustomerLiteral101" runat="server" TextWordNo="101" />
                </div>
            </li>
            <!-- ご用命事項 -->
            <li class="TabButton02">
                <div class="Rollover">
                    <%--<asp:Label ID="LabelOrdersTab" runat="server" ></asp:Label>--%>
                    <icrop:CustomLabel ID="CustomerLiteral201" runat="server" TextWordNo="201" />
                </div>
            </li>
            <!-- 作業内容 -->
            <li class="TabButton03">
                <div class="Button">
                    <%--<asp:Label ID="LabelWorkTab" runat="server" ></asp:Label>--%>
                    <icrop:CustomLabel ID="CustomerLiteral301" runat="server" TextWordNo="301" />
                </div>
            </li>
        </ul>
        </div>
        <!-- ここから基本情報パネル -->
        <div class="TabBox01">
        <div class="S-TC-05">
            <div class="S-TC-05Left">
				<div class="S-TC-05Left1-1">
                    <h2><icrop:CustomLabel ID="CustomerLiteral102" runat="server" TextWordNo="102" /></h2>
					<div class="S-TC-05Left1-1Wrap">
	                    <dl class="S-TC-05Left1-2">
                            <dt><icrop:CustomLabel ID="CustomerLiteral103" runat="server" TextWordNo="103" /></dt>
                            <dd><asp:Literal ID="LiteralBuyerName" runat="server" Mode="Encode"></asp:Literal></dd>
                        </dl>
						<dl class="S-TC-05Left1-3">
                            <dt><icrop:CustomLabel ID="CustomerLiteral104" runat="server" TextWordNo="104" /></dt>
                            <dd><asp:Literal ID="LiteralOrderCustomerName" runat="server" Mode="Encode"></asp:Literal></dd>
                        </dl>
	                    <dl class="S-TC-05Left1-4">
                            <dt>
                                <asp:Literal ID="LiteralMakerType" runat="server" Mode="Encode"></asp:Literal> ／ 
                                <asp:Literal ID="LiteralOrderVehicleName" runat="server" Mode="Encode"></asp:Literal> ／ 
                                <asp:Literal ID="LiteralOrderGrade" runat="server" Mode="Encode"></asp:Literal>
                            </dt>
                        </dl>
	                    <dl class="S-TC-05Left1-5">
                            <dt><icrop:CustomLabel ID="CustomerLiteral105" runat="server" TextWordNo="105" /></dt>
                            <dd><asp:Literal ID="LiteralOrderVinNo" runat="server" Mode="Encode"></asp:Literal></dd>
                        </dl>
	                    <dl class="S-TC-05Left1-6">
                            <dt><icrop:CustomLabel ID="CustomerLiteral106" runat="server" TextWordNo="106" /></dt>
                            <dd><asp:Literal ID="LiteralOrderRegisterNo" runat="server" Mode="Encode"></asp:Literal></dd>
                         </dl>
	                    <dl class="S-TC-05Left1-7">
                            <dt><icrop:CustomLabel ID="CustomerLiteral107" runat="server" TextWordNo="107" /></dt>
                            <dd>
                                <asp:Literal ID="LiteralOrderModel" runat="server" Mode="Encode"></asp:Literal> <asp:Literal ID="LiteralDeliverDate" runat="server"></asp:Literal>
                            </dd>
                        </dl>
	                    <dl class="S-TC-05Left1-8">
                            <dt><icrop:CustomLabel ID="CustomerLiteral108" runat="server" TextWordNo="108" /></dt>
                            <dd>
                                <asp:Literal ID="LiteralOrderMileage" runat="server" Mode="Encode"></asp:Literal>
                                <%--<icrop:CustomLabel ID="CustomerLiteral109" runat="server" TextWordNo="109" />--%>
                            </dd>
                        </dl>
					</div>
				</div>

				<div class="S-TC-05Left2-1">
                    <h2><icrop:CustomLabel ID="CustomerLiteral110" runat="server" TextWordNo="110" /></h2>
					<div class="S-TC-05Left2-1Wrap">
                        <asp:HiddenField ID="HiddenField05_Fuel" runat="server" />
						<dl class="S-TC-05Left2-2">
							<dt><icrop:CustomLabel ID="CustomerLiteral111" runat="server" TextWordNo="111" /></dt>
							<dd><icrop:CustomLabel ID="CustomerLiteral112" runat="server" TextWordNo="112" /></dd>
							<dd>
								<ul class="S-TC-05Left2-3">
									<li id="TC05_Fuel01" class="S-TC-05Left2-3-1Off"></li>
									<li id="TC05_Fuel02" class="S-TC-05Left2-3-2Off"></li>
									<li id="TC05_Fuel03" class="S-TC-05Left2-3-3Off"></li>
									<li id="TC05_Fuel04" class="S-TC-05Left2-3-4Off"></li>
								</ul>                              
							<dd><icrop:CustomLabel ID="CustomerLiteral113" runat="server" TextWordNo="113" /></dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_Audio" runat="server" />
						<dl class="S-TC-05Left2-5">
							<dt><icrop:CustomLabel ID="CustomerLiteral114" runat="server" TextWordNo="114" /></dt>
							<dd>
								<ul class="S-TC-05Left2-6">
									<li id="TC05_AudioOff" class="S-TC-05Left2-6-1Off"><icrop:CustomLabel ID="CustomerLiteral115" runat="server" TextWordNo="115" /></li>
									<li id="TC05_AudioCD" class="S-TC-05Left2-6-2Off"><icrop:CustomLabel ID="CustomerLiteral116" runat="server" TextWordNo="116" /></li>
									<li id="TC05_AudioFM" class="S-TC-05Left2-6-3Off"><icrop:CustomLabel ID="CustomerLiteral117" runat="server" TextWordNo="117" /></li>
								</ul>
							</dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_AirConditioner" runat="server" />
						<dl class="S-TC-05Left2-7">
							<dt><icrop:CustomLabel ID="CustomerLiteral118" runat="server" TextWordNo="118" /></dt>
							<dd>
								<ul class="S-TC-05Left2-8">
									<li id="TC05_AirConditionerOff" class="S-TC-05Left2-8-1Off"><icrop:CustomLabel ID="CustomerLiteral119" runat="server" TextWordNo="119" /></li>
									<li id="TC05_AirConditionerOn" class="S-TC-05Left2-8-2Off"><icrop:CustomLabel ID="CustomerLiteral120" runat="server" TextWordNo="120" /></li>
								</ul>
							</dd>
							<dd>
                                <asp:Literal ID="LiteralAirConditionerTemperature" runat="server"></asp:Literal>
                            </dd>
						</dl>
                        <asp:HiddenField ID="HiddenField05_Accessory1" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory2" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory3" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory4" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory5" runat="server" />
                        <asp:HiddenField ID="HiddenField05_Accessory6" runat="server" />
						<ul class="S-TC-05Left2-9">
							<li id="TC05_Accessory1" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral122" runat="server" TextWordNo="122" /></li>
							<li id="TC05_Accessory2" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral123" runat="server" TextWordNo="123" /></li>
							<li id="TC05_Accessory3" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral124" runat="server" TextWordNo="124" /></li>
							<li id="TC05_Accessory4" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral125" runat="server" TextWordNo="125" /></li>
							<li id="TC05_Accessory5" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral126" runat="server" TextWordNo="126" /></li>
							<li id="TC05_Accessory6" class="S-TC-05Left2-9Checked"><icrop:CustomLabel ID="CustomerLiteral127" runat="server" TextWordNo="127" /></li>
						</ul>
	                    <dl class="S-TC-05Left2-10">
                            <dt><icrop:CustomLabel ID="CustomerLiteral128" runat="server" TextWordNo="128" /></dt>
                            <dd><asp:Literal ID="LiteralValuablesMemo" runat="server" Mode="Encode"></asp:Literal></dd>
                        </dl>
					</div>
				</div>
            </div>

            <div class="S-TC-05Right">
				<h2><icrop:CustomLabel ID="CustomerLiteral129" runat="server" TextWordNo="129" /></h2>
				<div class="S-TC-05RightWrap">
					<div id="S-TC-05RightScroll" class="S-TC-05RightScroll">
                        <asp:Repeater ID="RepeaterHistoryInfo" runat="server">
                            <ItemTemplate>
                                <dl class="S-TC-05Right1-1">
	                                <dt><img src="../Styles/Images/SC3150102/ico59.png" width="15" height="16"></dt>
	                                <dd>
                                        <span><asp:Literal ID="LiteralHAcceptDate" runat="server" ></asp:Literal>&nbsp;</span><br />
                                        <asp:Literal ID="LiteralHOrderNo" runat="server" ></asp:Literal>&nbsp;
                                    </dd>
	                                <dd><asp:Literal ID="LiteralHTypicalSrvTypeName" runat="server" ></asp:Literal>&nbsp;</dd>
	                                <dd><asp:Literal ID="LiteralHTypicalSrvType" runat="server" ></asp:Literal>&nbsp;</dd>
	                                <dd><asp:Literal ID="LiteralHCustomerName" runat="server" ></asp:Literal>&nbsp;</dd>

                                    <asp:HiddenField ID="HiddenFieldHAcceptDate" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("OrderDate")) %>' />
                                    <asp:HiddenField ID="HiddenFieldHOrderNo" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("OrderNo")) %>' />
                                    <asp:HiddenField ID="HiddenFieldHTypicalSrvTypeName" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("TypicalSrvTypeName")) %>' />
                                    <asp:HiddenField ID="HiddenFieldHOrderCustomerName" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("CustomerName")) %>' />
                                    <asp:HiddenField ID="HiddenFieldHTypicalSrvType" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("OrderSrvName")) %>' />
	                            </dl>
                            </ItemTemplate>
                        </asp:Repeater>
					</div>
				</div>
            </div>
        </div>
        </div>
        <!-- ここまで基本情報パネル -->

        <!-- ここからご用命事項パネル -->
        <div class="TabBox02">
        <div class="S-TC-07TabWrap">
            <div class="S-TC-07Left">
                <h2><icrop:CustomLabel ID="CustomerLiteral201Second" runat="server" TextWordNo="283" /></h2>
                <p id="S-TC-07LeftMemo2" class="S-TC-07LeftMemo2"><asp:Literal ID="LiteralOrderMemo" runat="server" Mode="Encode"></asp:Literal></p>
            </div>

            <!--<div class="S-TC-07Right">-->
            <div class="S-TC-07Right">
                <div class="S-TC-07RightTab">
                    <ul>
                        <li id="S-TC-07RightTab_01"><icrop:CustomLabel ID="CustomerLiteral202" runat="server" TextWordNo="202" /></li>
                        <li id="S-TC-07RightTab_02" class="S-TC-07RightTabNoSelected"><icrop:CustomLabel ID="CustomerLiteral203" runat="server" TextWordNo="203" /></li>
                    </ul>
                </div>
                <div id="S-TC-07RightTabWrap" class="S-TC-07RightTabWrap">
                    <!-- 確認事項 -->
                    <div id="S-TC-07RightBody" class="S-TC-07RightBody" >
                    <div id="S-TC-07RightBodyFlick" class="S-TC-07RightBodyFlick">
                        <asp:HiddenField ID="HiddenField07_ExchangeParts" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral204" runat="server" TextWordNo="204" /></dt>
                            <dd id="TC07_ExchangeParts1" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral205" runat="server" TextWordNo="205" /></dd>
                            <dd id="TC07_ExchangeParts2" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral206" runat="server" TextWordNo="206" /></dd>
                            <dd id="TC07_ExchangeParts3" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral207" runat="server" TextWordNo="207" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Waiting" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral208" runat="server" TextWordNo="208" /></dt>
                            <dd id="TC07_WaitingIn" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral209" runat="server" TextWordNo="209" /></dd>
                            <dd id="TC07_WaitingOut" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral210" runat="server" TextWordNo="210" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Washing" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral211" runat="server" TextWordNo="211" /></dt>
                            <dd id="TC07_WashingDo" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral212" runat="server" TextWordNo="212" /></dd>
                            <dd id="TC07_WashingNone" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral213" runat="server" TextWordNo="213" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Payment" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral214" runat="server" TextWordNo="214" /></dt>
                            <dd id="TC07_PaymentCash" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral215" runat="server" TextWordNo="215" /></dd>
                            <dd id="TC07_PaymentCard" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral216" runat="server" TextWordNo="216" /></dd>
                            <dd id="TC07_PaymentOther" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral217" runat="server" TextWordNo="217" /></dd>
                        </dl>
                        <%--<dl>
                            <dt>&nbsp;</dt>
                            <dd class="S-TC-07Right01-1"><icrop:CustomLabel ID="CustomerLiteral218" runat="server" TextWordNo="218" /></dd>
                            <dd class="S-TC-07Right01-2"><icrop:CustomLabel ID="CustomerLiteral219" runat="server" TextWordNo="219" /></dd>
                            <dd class="S-TC-07Right01-3"><icrop:CustomLabel ID="CustomerLiteral220" runat="server" TextWordNo="220" /></dd>
                        </dl>
                        <dl>
                            <dt>&nbsp;</dt>
                            <dd class="S-TC-07Right02-1"><icrop:CustomLabel ID="CustomerLiteral221" runat="server" TextWordNo="221" /></dd>
                            <dd class="S-TC-07Right02-2"><icrop:CustomLabel ID="CustomerLiteral222" runat="server" TextWordNo="222" /></dd>
                        </dl>--%>
                        <dl>
                            <dt><span><icrop:CustomLabel ID="CustomerLiteral223" runat="server" TextWordNo="223" /></span></dt>
                            <dd class="S-TC-07Right05-1">
                                <div class="S-TC-07Right05-1Memo">
                                    <div class="S-TC-07Right05-1MemoBg">
                                        <asp:Literal ID="LiteralInvoiceAddress" runat="server"></asp:Literal>&nbsp;
                                    </div>
                                </div>
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Csi" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral224" runat="server" TextWordNo="224" /></dt>
                            <dd id="TC07_CSI_AM" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral225" runat="server" TextWordNo="225" /></dd>
                            <dd id="TC07_CSI_PM" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral226" runat="server" TextWordNo="226" /></dd>
                            <dd id="TC07_CSI_Always" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral227" runat="server" TextWordNo="227" /></dd>
                        </dl>
                    </div>
                    </div>
                    <!-- 問診項目 -->
                    <div id="S-TC-07RightScroll" class="S-TC-07RightScroll" >
                    <div id="S-TC-07RightScrollFlick" class="S-TC-07RightScrollFlick" >
                        <asp:HiddenField ID="HiddenField07_Warning" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral228" runat="server" TextWordNo="228" /></dt>
                            <dd id="TC07_WNG_Always" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral229" runat="server" TextWordNo="229" /></dd>
                            <dd id="TC07_WNG_Often" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral230" runat="server" TextWordNo="230" /></dd>
                            <dd id="TC07_WNG_None" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral231" runat="server" TextWordNo="231" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Occurrence" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral232" runat="server" TextWordNo="232" /></dt>
                            <dd id="TC07_Occurrence_Recently" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral233" runat="server" TextWordNo="233" /></dd>
                            <dd id="TC07_Occurrence_Week" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral234" runat="server" TextWordNo="234" /></dd>
                            <dd id="TC07_Occurrence_Other" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral235" runat="server" TextWordNo="235" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Frequency" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral236" runat="server" TextWordNo="236" /></dt>
                            <dd id="TC07_Frequency_High" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral237" runat="server" TextWordNo="237" /></dd>
                            <dd id="TC07_Frequency_Often" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral238" runat="server" TextWordNo="238" /></dd>
                            <dd id="TC07_Frequency_Once" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral239" runat="server" TextWordNo="239" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Reappear" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral240" runat="server" TextWordNo="240" /></dt>
                            <dd id="TC07_Reappear_Yes" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral241" runat="server" TextWordNo="241" /></dd>
                            <dd id="TC07_Reappear_No" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral242" runat="server" TextWordNo="242" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_WaterT" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral243" runat="server" TextWordNo="243" /></dt>
                            <dd id="TC07_WaterT_Low" class="S-TC-07Right03-1Off"><icrop:CustomLabel ID="CustomerLiteral244" runat="server" TextWordNo="244" /></dd>
                            <dd id="TC07_WaterT_High" class="S-TC-07Right03-2Off"><icrop:CustomLabel ID="CustomerLiteral245" runat="server" TextWordNo="245" /></dd>
                            <dd class="S-TC-07Right03-3">
                                <!--<input type="text"></input>-->
                                <%--<asp:TextBox ID="TextBoxHearingWTemperature" runat="server" ReadOnly="true"></asp:TextBox>--%>
                                <icrop:CustomLabel ID="CustomLabelHearingWTemperature" runat="server" />
                                <icrop:CustomLabel ID="CustomerLiteral246" runat="server" TextWordNo="246" />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Temperature" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral247" runat="server" TextWordNo="247" /></dt>
                            <dd id="TC07_Temperature_Low" class="S-TC-07Right03-1Off"><icrop:CustomLabel ID="CustomerLiteral248" runat="server" TextWordNo="248" /></dd>
                            <dd id="TC07_Temperature_High" class="S-TC-07Right03-2Off"><icrop:CustomLabel ID="CustomerLiteral249" runat="server" TextWordNo="249" /></dd>
                            <dd class="S-TC-07Right03-3">
                                <!--<input type="text"></input>-->
                                <%--<asp:TextBox ID="TextBoxHearingTemperature" runat="server" ReadOnly="true" ></asp:TextBox>--%>
                                <icrop:CustomLabel ID="CustomLabelHearingTemperature" runat="server" />
                                <icrop:CustomLabel ID="CustomerLiteral246Second" runat="server" TextWordNo="284" />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_Place" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral250" runat="server" TextWordNo="250" /></dt>
                            <dd id="TC07_Place_Parking" class="S-TC-07Right04-1Off"><icrop:CustomLabel ID="CustomerLiteral251" runat="server" TextWordNo="251" /></dd>
                            <dd id="TC07_Place_Ordinary" class="S-TC-07Right04-2Off"><icrop:CustomLabel ID="CustomerLiteral252" runat="server" TextWordNo="252" /></dd>
                            <dd id="TC07_Place_Motorway" class="S-TC-07Right04-3Off"><icrop:CustomLabel ID="CustomerLiteral253" runat="server" TextWordNo="253" /></dd>
                            <dd id="TC07_Place_Slope" class="S-TC-07Right04-4Off"><icrop:CustomLabel ID="CustomerLiteral254" runat="server" TextWordNo="254" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_TrafficJam" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral255" runat="server" TextWordNo="255" /></dt>
                            <dd id="TC07_Trafficjam_Happen" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral256" runat="server" TextWordNo="256" /></dd>
                            <dd id="TC07_Trafficjam_None" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral257" runat="server" TextWordNo="257" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_CarStatus_Startup" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Idling" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Cold" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Warm" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral258" runat="server" TextWordNo="258" /></dt>
                            <dd id="TC07_CarStatus_Startup" class="S-TC-07Right04-1Off"><icrop:CustomLabel ID="CustomerLiteral259" runat="server" TextWordNo="259" /></dd>
                            <dd id="TC07_CarStatus_Idlling" class="S-TC-07Right04-2Off"><icrop:CustomLabel ID="CustomerLiteral260" runat="server" TextWordNo="260" /></dd>
                            <dd id="TC07_CarStatus_Cold" class="S-TC-07Right04-3Off"><icrop:CustomLabel ID="CustomerLiteral261" runat="server" TextWordNo="261" /></dd>
                            <dd id="TC07_CarStatus_Warm" class="S-TC-07Right04-4Off"><icrop:CustomLabel ID="CustomerLiteral262" runat="server" TextWordNo="262" /></dd>
                        </dl>
                        <dl>
                            <dt>&nbsp;</dt>
                            <dd id="S-SA-07Tab02Right1-5-1" class="S-SA-07Tab02Right1-5-1Off"><icrop:CustomLabel ID="CustomerLiteral263" runat="server" TextWordNo="263" /></dd>
                        </dl>
						
                        <div id="S-SA-07Tab02Right1-5-1Display" class="S-SA-07Tab02Right1-5-1Display">
                        <asp:HiddenField ID="HiddenField07_Traveling" runat="server" />
						<div class="S-TC-07RightList">
						    <ul>
						    <li id="TC07_Traveling_Lowspeed" ><icrop:CustomLabel ID="CustomerLiteral264" runat="server" TextWordNo="264" /></li>
						    <li id="TC07_Traveling_Acceleration" ><icrop:CustomLabel ID="CustomerLiteral265" runat="server" TextWordNo="265" /></li>
						    <li id="TC07_Traveling_Slowdown" ><icrop:CustomLabel ID="CustomerLiteral266" runat="server" TextWordNo="266" /></li>
						    </ul>
						</div>
                        <asp:HiddenField ID="HiddenField07_CarStatus_Parking" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Advance" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_ShiftChange" runat="server" />
                        <dl>
                            <dt>&nbsp;</dt>
                            <dd id="TC07_CarControl1_Parking" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral267" runat="server" TextWordNo="267" /></dd>
                            <dd id="TC07_CarControl1_Advance" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral268" runat="server" TextWordNo="268" /></dd>
                            <dd id="TC07_CarControl1_ShiftChange" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral269" runat="server" TextWordNo="269" /></dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_CarStatus_Back" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Brake" runat="server" />
                        <asp:HiddenField ID="HiddenField07_CarStatus_Detour" runat="server" />
                        <dl>
                            <dt>&nbsp;</dt>
                            <dd id="TC07_CarControl2_Back" class="S-TC-07Right01-1Off"><icrop:CustomLabel ID="CustomerLiteral270" runat="server" TextWordNo="270" /></dd>
                            <dd id="TC07_CarControl2_Brake" class="S-TC-07Right01-2Off"><icrop:CustomLabel ID="CustomerLiteral271" runat="server" TextWordNo="271" /></dd>
                            <dd id="TC07_CarControl2_Detour" class="S-TC-07Right01-3Off"><icrop:CustomLabel ID="CustomerLiteral272" runat="server" TextWordNo="272" /></dd>
                        </dl>
                        </div>

                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral273" runat="server" TextWordNo="273" /></dt>
                            <dd class="S-TC-07Right06-1">
                                <!--<input type="text" />-->
                                <asp:TextBox ID="TextBoxHearingSpeedRate" runat="server" ReadOnly="true"></asp:TextBox>
                                <%--<icrop:CustomLabel ID="CustomLabelHearingSpeedRate" runat="server" />--%>
                                <icrop:CustomLabel ID="CustomerLiteral274" runat="server" TextWordNo="274" />
                            </dd>
                            <dd class="S-TC-07Right06-2">
                                <!--<input type="text" />-->
                                <asp:TextBox ID="TextBoxHearingSpeedGear" runat="server" ReadOnly="true" ></asp:TextBox>
                                <icrop:CustomLabel ID="CustomerLiteral275" runat="server" TextWordNo="275" />
                                <%--<icrop:CustomLabel ID="CustomLabelHearingSpeedGear" runat="server" />--%>
                            </dd>
                        </dl>
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral276" runat="server" TextWordNo="276" /></dt>
                            <dd class="S-TC-07Right07-1">
                                <!--<input type="text" />-->
                                <asp:TextBox ID="TextBoxHearingPeopleNumber" runat="server" ReadOnly="true" ></asp:TextBox>
                                <%--<icrop:CustomLabel ID="CustomLabelHearingPeopleNumber" runat="server" />--%>
                                <icrop:CustomLabel ID="CustomerLiteral277" runat="server" TextWordNo="277" />
                            </dd>
                            <dd class="S-TC-07Right07-2">
                                <icrop:CustomLabel ID="CustomerLiteral279" runat="server" TextWordNo="279" />
                                <!--<input type="text" />-->
                                <asp:TextBox ID="TextBoxHearingPeopleTooHeavy" runat="server" ReadOnly="true"></asp:TextBox>
                                <%--<icrop:CustomLabel ID="CustomLabelHearingPeopleTooHeavy" runat="server" />--%>
                                <icrop:CustomLabel ID="CustomerLiteral278" runat="server" TextWordNo="278" />
                            </dd>
                        </dl>
                        <asp:HiddenField ID="HiddenField07_NonGenuine" runat="server" />
                        <dl>
                            <dt><icrop:CustomLabel ID="CustomerLiteral280" runat="server" TextWordNo="280" /></dt>
                            <dd id="TC07_NonGenuine_Yes" class="S-TC-07Right02-1Off"><icrop:CustomLabel ID="CustomerLiteral281" runat="server" TextWordNo="281" /></dd>
                            <dd id="TC07_NonGenuine_No" class="S-TC-07Right02-2Off"><icrop:CustomLabel ID="CustomerLiteral282" runat="server" TextWordNo="282" /></dd>
                        </dl>
                    </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
        <!-- ここまでご用命事項パネル -->

        <!-- ここから作業内容パネル -->
        <div class="TabBox03">
            <div class="S-TC-01">
				<div class="S-TC-01Wrap">
					<h2 class="contentTitle"><icrop:CustomLabel ID="CustomerLiteral302" runat="server" TextWordNo="302" /></h2>
                    <asp:HiddenField ID="HiddenFieldAddWorkCount" runat="server" />
                    <asp:HiddenField ID="HiddenFieldSelectedAddWork" runat="server" />
                    <asp:HiddenField ID="HiddenFieldRepairOrderInitialWord" runat="server" />

					<ul id="S-TC-01Paging" class="S-TC-01Paging">
                        <li class="liFast"></li>
                        <li class="liScroll">
                            <div id="divScroll">
                                <%--<div class="S-TC-01PagingOn"><icrop:CustomLabel ID="CustomerLiteral316" runat="server" TextWordNo="316" /></div>
						        <div class="S-TC-01PagingOff"><span>1</span></div>
						        <div class="S-TC-01PagingOff"><span>2</span></div>
                                <div class="S-TC-01PagingOff"><span>3</span></div>--%>
                            </div>
                        </li>
                        <li class="liLast"></li>
					</ul>

					<div class="S-TC-01Wrap2">
						<div class="S-TC-01Left">
							<h3><icrop:CustomLabel ID="CustomerLiteral303" runat="server" TextWordNo="303" /></h3>
							<dl class="S-TC-01LeftHead">
								<dt><icrop:CustomLabel ID="CustomerLiteral304" runat="server" TextWordNo="304" /></dt>
								<dd><icrop:CustomLabel ID="CustomerLiteral305" runat="server" TextWordNo="305" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral306" runat="server" TextWordNo="306" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral307" runat="server" TextWordNo="307" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral308" runat="server" TextWordNo="308" /></dd>
							</dl>
							<div id="S-TC-01LeftBody" class="S-TC-01LeftScroll">
                                <table>
                                <asp:Repeater ID="RepeaterWorkInfo" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><asp:Literal ID="LiteralWorkNo" runat="server" Mode="Encode"></asp:Literal>&nbsp;</td>
                                            <td><asp:Literal ID="LiteralSrvName" runat="server" Mode="Encode"></asp:Literal>&nbsp;</td>
                                            <td>
                                                <asp:Literal ID="LiteralWorkHours" runat="server" Mode="Encode"></asp:Literal>&nbsp;
                                                <%--<icrop:CustomLabel ID="CustomerLiteral309" runat="server" TextWordNo="309" />--%>
                                            </td>
                                            <td><asp:Literal ID="LiteralSellWorkPrice" runat="server" Mode="Encode"></asp:Literal>&nbsp;</td>
                                            <td><asp:Literal ID="LiteralSubtotal" runat="server" Mode="Encode"></asp:Literal>&nbsp;</td>

                                            <asp:HiddenField ID="HiddenFieldSrvName" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SrvName")) %>' />
                                            <asp:HiddenField ID="HiddenFieldWorkHours" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("WorkHours")) %>' />
                                            <asp:HiddenField ID="HiddenFieldSellWorkPrice" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SellWorkPrice")) %>' />
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </table>
							</div>
						</div>

						<div class="S-TC-01Right">
							<h3><icrop:CustomLabel ID="CustomerLiteral311" runat="server" TextWordNo="311" /></h3>
							<dl class="S-TC-01RightHead">
								<dt><icrop:CustomLabel ID="CustomerLiteral304Second" runat="server" TextWordNo="319" /></dt>
								<dd><icrop:CustomLabel ID="CustomerLiteral312" runat="server" TextWordNo="312" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral313" runat="server" TextWordNo="313" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral314" runat="server" TextWordNo="314" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral317" runat="server" TextWordNo="317" /></dd>
								<dd><icrop:CustomLabel ID="CustomerLiteral318" runat="server" TextWordNo="318" /></dd>
							</dl>
                            <!-- ここからR/O情報欄に対するフィルター -->
                            <div class="S-TC-01RightScrollFilter">
                            </div>
                            <asp:HiddenField ID="HiddenFieldPartsReady" runat="server" />
                            <asp:HiddenField ID="HiddenFieldPartsCount" runat="server" />
                            <asp:HiddenField ID="HiddenFieldPartsBackOrderCount" runat="server" />
                            <!-- ここまでR/O情報欄に対するフィルター -->
							<div id="S-TC-01RightBody" class="S-TC-01RightScroll">
                                <table>
                                <asp:Repeater ID="RepeaterPartsInfo" runat="server">                                    
                                    <ItemTemplate>
                                        <tr>
                                            <td><asp:Literal ID="LiteralPartsNo" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td><asp:Literal ID="LiteralPartsName" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td><asp:Literal ID="LiteralPartsType" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td><asp:Literal ID="LiteralPartsQuantity" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td><asp:Literal ID="LiteralPartsUnit" runat="server" Mode="Encode"></asp:Literal></td>
                                            <td><asp:Literal ID="LiteralPartsOrderStatus" runat="server" Mode="Encode"></asp:Literal></td>

                                            <asp:HiddenField ID="HiddenFieldPartsName" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("PartsName")) %>' />
                                            <asp:HiddenField ID="HiddenFieldPartsType" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SrvTypeName")) %>' />
                                            <asp:HiddenField ID="HiddenFieldPartsQuantity" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("Quantity")) %>' />
                                            <asp:HiddenField ID="HiddenFieldPartsUnit" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("Unit")) %>' />
                                            <asp:HiddenField ID="HiddenFieldPartsOrderStatus" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("BOFlag")) %>' />
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
        <!-- ここまで作業内容パネル -->

    </div>
    <!-- ここまで基本情報・ご用命事項・作業内容パネル -->
</div>
<!-- ここまでR/O情報欄 -->
</div>
</asp:Content>
