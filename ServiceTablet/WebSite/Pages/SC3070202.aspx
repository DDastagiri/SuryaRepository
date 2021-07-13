<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3070202.aspx.vb" Inherits="Pages_SC3070202" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3070202/SC3070202.css" type="text/css" media="screen,print"/>
    <script type="text/javascript" src="../Scripts/SC3070202/SC3070202.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server" overflow="scroll">
    <!-- ここからコンテンツ -->
<!-- このコードは「'__o' は宣言されていません。アクセスできない保護レベルになっています。」というデザイナのバグを削除するためのコードなので削除しないで下さい。-->
<%=""%>

    <div id="Ncv5001Main">
        <%--<div class="Pint_bWindowb_Box">--%>
            <div id="scrollInner" class="Scroll">    
                <asp:HiddenField ID="DisplayHeightValueHiddenField" runat="server"/>       
                <asp:HiddenField ID="ScrollHeightValueHiddenField" runat="server"/> 
                <div id="contractHcv5001PaperFrame">
                    <!-- 閉じるボタン(画像) -->
                    <div class="Pint_bWindowb_Close">
                        <asp:Button ID="closeButton" runat="server" CssClass="closeButtonImage buttonNone" Width="33px" UseSubmitBehavior="False"/>
                    </div>
                    <!-- 印刷ボタン -->
                    <div class="contractHcv5001FootButtoms">
                        <asp:Button ID="printButton" runat="server" class="contractHcv5001ChargeButton buttonGlay1 buttonsNone" text-aligin="center" UseSubmitBehavior="False" Height="30px" Width="75px"/>
                    </div>
			        
                    <div id="contractHcv5001CustomerDataArea">
			            <table border="0" cellspacing="0" cellpadding="0">
			                <tr>
			                    <th rowspan="4" class="contractHcv5001CustomerDataAreaTh1">
			            	        <table width="100%" border="0" cellpadding="0" cellspacing="0">
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="customerNameLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip" TextWordNo="2" /></td>
		            			        </tr>
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="vehicleLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip" TextWordNo="3"/></td>
		            			        </tr>
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="gradeSpecLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip" TextWordNo="4"/></td>
		            			        </tr>
		            		        </table>
			                    </th>
			                    
                                <td rowspan="4" class="contractHcv5001CustomerDataAreaTd1Name">
			            	        <table width="100%" border="0" cellpadding="0" cellspacing="0">
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="customerNameWordLabel" runat="server" Width="488" UseEllipsis="False" CssClass="clip"/></td>
		            			        </tr>
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="vehicleWordLabel" runat="server" Width="488" UseEllipsis="False" CssClass="clip"/></td>
		            			        </tr>
			            		        <tr>
			            			        <td class="boxHeight" valign="middle"><icrop:CustomLabel ID="gradeSpecWordLabel" runat="server" Width="488" UseEllipsis="False" CssClass="clip"/></td>
		            			        </tr>
		            		        </table>
			                    </td>

                                <!--日付-->
			                    <th class="contractHcv5001CustomerDataAreaTh2"><icrop:CustomLabel ID="dateLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip" TextWordNo="5"/></th>
			                    <td class="contractHcv5001CustomerDataAreaTd2">:  </td>
			                    <td class="contractHcv5001CustomerDataAreaTd3"><icrop:CustomLabel ID="dateWordLabel" runat="server" Width="140" UseEllipsis="False" CssClass="clip"/></td>
			                </tr>
			                
                            <!--販売店-->
                            <tr>
			                    <th class="contractHcv5001CustomerDataAreaTh2"><icrop:CustomLabel ID="dealerLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip"　TextWordNo="6"/></th>
			                    <td class="contractHcv5001CustomerDataAreaTd2">:  </td>
			                    <td class="contractHcv5001CustomerDataAreaTd3"><icrop:CustomLabel ID="dealerWordLabel" runat="server" Width="140" UseEllipsis="False" CssClass="clip"/></td>
			                </tr>
                            
                            <!--電話番号-->
                            <tr>
			                    <th class="contractHcv5001CustomerDataAreaTh2"><icrop:CustomLabel ID="telNoLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip"　TextWordNo="7"/></th>
			                    <td class="contractHcv5001CustomerDataAreaTd2">:</td>
			                    <td class="contractHcv5001CustomerDataAreaTd3"><icrop:CustomLabel ID="telNoWordLabel" runat="server" Width="140" UseEllipsis="False" CssClass="clip"/></td>
    			            </tr>

                            <!--セールススタッフ-->
			                <tr>
			                    <th class="contractHcv5001CustomerDataAreaTh2"><icrop:CustomLabel ID="salesStaffLabel" runat="server" Width="120" UseEllipsis="False" CssClass="clip" TextWordNo="8"/></th>
			                    <td class="contractHcv5001CustomerDataAreaTd2">:</td>
			                    <td class="contractHcv5001CustomerDataAreaTd3"><icrop:CustomLabel ID="salesStaffWordLabel" runat="server" Width="140" UseEllipsis="False" CssClass="clip"/></td>
			                </tr>
                        </table>
                    </div>
			      
                    <!--車両情報-->
                    <div id="contractHcv5001CarDataArea">
			            <table width="922" border="0" cellspacing="0" cellpadding="0">
                            <tr>
			                    <td><h4><icrop:CustomLabel ID="bodyTypeLabel" runat="server" Width="160" UseEllipsis="False" CssClass="clip" TextWordNo="9"/></h4><icrop:CustomLabel ID="bodyTypeWordLabel" runat="server" UseEllipsis="False" Width="160" CssClass="clip"/></td>
			                    <td><h4><icrop:CustomLabel ID="displacementLabel" runat="server" Width="160" UseEllipsis="False" CssClass="clip" TextWordNo="10"/></h4><icrop:CustomLabel ID="displacementWordLabel" runat="server" UseEllipsis="False" Width="160" CssClass="clip"/></td>
			                    <td><h4><icrop:CustomLabel ID="drivingLabel" runat="server" Width="160" UseEllipsis="False" CssClass="clip" TextWordNo="11"/></h4><icrop:CustomLabel ID="drivingWordLabel" runat="server" UseEllipsis="False" Width="160" CssClass="clip"/></td>
			                    <td><h4><icrop:CustomLabel ID="missionLabel" runat="server" Width="160" UseEllipsis="False" CssClass="clip" TextWordNo="12"/></h4><icrop:CustomLabel ID="missionWordLabel" runat="server" UseEllipsis="False" Width="160" CssClass="clip"/></td>
			                    <td><h4><icrop:CustomLabel ID="outColorLabel" runat="server" Width="160" UseEllipsis="False" CssClass="clip" TextWordNo="13"/></h4><icrop:CustomLabel ID="outColorWordLabel" runat="server" UseEllipsis="False" Width="160" CssClass="clip"/></td>
			                </tr>
			                <tr>
			                    <td colspan="2"><h4><icrop:CustomLabel ID="inColorLabel" runat="server" UseEllipsis="False" Width="350" CssClass="clip" TextWordNo="14"/></h4><icrop:CustomLabel ID="inColorWordLabel" runat="server" UseEllipsis="False" Width="350" CssClass="clip"/></td>
			                    <td colspan="3"><h4><icrop:CustomLabel ID="carNoLabel" runat="server" UseEllipsis="False" Width="530" CssClass="clip" TextWordNo="15"/></h4><icrop:CustomLabel ID="carNoWordLabel" runat="server" UseEllipsis="False" Width="530" CssClass="clip"/></td>
			                </tr>
			            </table>
			        </div>
			        
                    <div id="contractHcv5001ChargeArea">
			            <div class="contractHcv5001ChargeAreaLeft">
			                <!--車両価格-->
                            <h3><icrop:CustomLabel ID="carPriceLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="16"/></h3>
			                <table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem1">
			                    <tr>
			                        <th><icrop:CustomLabel ID="carBodyPriceLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="17"/></th>
			                        <td><asp:Label ID="carBodyPriceWordLabel" runat="server"/></td>
			                    </tr>
<%--2012/01/05 myose del start
			                    <tr>
			                        <th><icrop:CustomLabel ID="optionPriceLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="18"/></th>
			                        <td><asp:Label ID="optionPriceWordLabel" runat="server"/></td>
			                    </tr>
2012/01/05 myose del end--%>
<%'--2012/01/05 myose add start%>
                                <%'値引き額がゼロ以外の場合、値引き額欄を表示する %>
                                <% If isDiscountHiddenField.Value = "1" Then%>
							    <tr>
								    <th><icrop:CustomLabel ID="discountLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="43"/></th>
								    <td><%="-"%><asp:Label ID="discountWordLabel" runat="server"/></td>
							    </tr>
                                <% End If%> 
<%'--2012/01/05 myose add end%>
			                    <tr class="contractHcv5001ChargeAreaItem2">
			            	        <th class="contractHcv5001GradationBack1"><icrop:CustomLabel ID="summaryCarPriceLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="19"/></th>
			            	        <td class="contractHcv5001GradationBack1"><asp:Label ID="summaryCarPriceWordLabel" runat="server"/></td>
		            	        </tr>
			                    </table>
                            
                            <!--オプション明細-->
                            <h3><icrop:CustomLabel ID="optionDetailLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="20"/></h3>								
                            <table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem7">
                                <% Dim optionName() As String = Me.optionNameHiddenField.Value.Split("|")%>
                                <% Dim optionPrice() As String = Me.optionPriceHiddenField.Value.Split("|")%>
                                <% For i As Integer = 0 To optionName.Length - 1%>
								<tr>
									<th><div class="optionDetailName"><%=optionName(i)%></div></th>
									<td><div><%=optionPrice(i)%></div></td>									
								</tr>
                                <% Next%>
                                <tr>
									<th class="contractHcv5001GradationBack1"><icrop:CustomLabel ID="optionPriceSammaryLabel" UseEllipsis="False" Width="240" CssClass="clip" runat="server" TextWordNo="21"/></th>
									<td class="contractHcv5001GradationBack1"><asp:Label ID="optionPriceSammaryWordLabel" runat="server"/></td>
								</tr>
							</table>

							<div class="contractHcv5001SpaseMemoUp">&nbsp;</div>

                            <!--メモ-->
                            <div class="contractHcv5001ChargeAreaItem6">
                             	<h3><icrop:CustomLabel ID="memoLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="22"/></h3>
                                <asp:Label ID="memoWordLabel" runat="server" class="contractHcv5001ChargeAreaItem6MemoArea" Width="434" Height="64"></asp:Label>
                            </div>       
                        </div>
			        		        
			        <div class="contractHcv5001ChargeAreaRight">

                        <!--諸費用-->
			        	<h3><icrop:CustomLabel ID="expensesLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="23"/></h3>
						<table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem1">
							<tr>
								<th><icrop:CustomLabel ID="carBuyingTaxLabel" UseEllipsis="False" Width="240" CssClass="clip" runat="server" TextWordNo="48"/></th>
								<td><asp:Label ID="carBuyingTaxWordLabel" runat="server"/></td>
							</tr>
							<tr>
								<th><icrop:CustomLabel ID="expenseRegistLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="49"/></th>
								<td><asp:Label ID="expenseRegistWordLabel" runat="server"/></td>
							</tr>
							<tr class="contractHcv5001ChargeAreaItem2">
								<th class="contractHcv5001GradationBack1"><icrop:CustomLabel ID="expenseSammaryLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="50"/></th>
								<td class="contractHcv5001GradationBack1"><asp:Label ID="expenseSammaryWordLabel" runat="server"/></td>
							</tr>
						</table>

                        <!--保険-->
						<h3><icrop:CustomLabel ID="insuranceLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="24"/></h3>								
                        <table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem1">
							<tr>
								<th><icrop:CustomLabel ID="insuranceCompanyLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="25"/></th>
								<td><asp:Label ID="insuranceCompanyWordLabel" runat="server" UseEllipsis="False" Width="167" CssClass="clip"/></td>
							</tr>
							<tr>
								<th><icrop:CustomLabel ID="insuranceTypeLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="26"/></th>
								<td><asp:Label ID="insuranceTypeWordLabel" runat="server" UseEllipsis="False" Width="167" CssClass="clip"/></td>
							</tr>
							<tr class="contractHcv5001ChargeAreaItem2">
								<th class="contractHcv5001GradationBack1"><icrop:CustomLabel ID="yearlyAmountLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="27"/></th>
								<td class="contractHcv5001GradationBack1"><asp:Label ID="yearlyAmountWordLabel" runat="server"/></td>
							</tr>
						</table>

                        <!--お支払い方法-->
						<h3><icrop:CustomLabel ID="paymentLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip"/></h3>
                        <table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem5">
						    <tr>
							    <th><icrop:CustomLabel ID="financeCompanyLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="32"/></th>
							    <td><asp:Label ID="financeCompanyWordLabel" runat="server" UseEllipsis="False" Width="167" CssClass="clip"/></td>
						    </tr>

							<tr>
								<th><icrop:CustomLabel ID="periodLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="33"/></th>
								<td><asp:Label ID="periodWordLabel" runat="server"/></td>
							</tr>

							<tr>
								<th><icrop:CustomLabel ID="monthlyLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="34"/></th>
								<td><asp:Label ID="monthlyWordLabel" runat="server"/></td>
							</tr>

							<tr>
								<th><icrop:CustomLabel ID="depositLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="35"/></th>
								<td><asp:Label ID="depositWordLabel" runat="server"/></td>
							</tr>

							<tr>
								<th><icrop:CustomLabel ID="bonusLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="36"/></th>
								<td><asp:Label ID="bonusWordLabel" runat="server"/></td>
							</tr>

							<tr>
								<th><icrop:CustomLabel ID="firstPaymentDayLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="37"/></th>
								<td><asp:Label ID="firstPaymentDayWordLabel" runat="server"/></td>
							</tr>
						</table>

                        <!--お支払い金額-->
						<h3><icrop:CustomLabel ID="paymentAmountLabel" runat="server" UseEllipsis="False" Width="445" CssClass="clip" TextWordNo="40"/></h3>
                        <table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem4">                               
                        <% Dim rowNum As String = Me.tradeInNumHiddenField.Value%>
                        <% Dim tradeInName() As String = Me.tradeInNameHiddenField.Value.Split("|")%>
                        <% Dim tradeInPrice() As String = Me.tradeInPriceHiddenField.Value.Split("|")%>                                     
                            <tr>
							    <th class="Sc1" rowspan="<%=rowNum%>"><icrop:CustomLabel ID="tradeInValue" runat="server" TextWordNo="41"/></th>
							    <th class="Sc2"><div class="divTradeInName"><%=tradeInName(0)%></div></th>
							    <td><div><%=tradeInPrice(0)%></div></td>
						    </tr>
                            <% For j As Integer = 1 To tradeInName.Length - 1%>
							<tr>
								<th class="Sc2"><div class="divTradeInName"><%=tradeInName(j)%></div></th>
								<td><div><%=tradeInPrice(j)%></div></td>
							</tr>
                            <% Next%>                                    
							<tr class="contractHcv5001ChargeAreaItem2">
								<th class="contractHcv5001GradationBack1" colspan="2"><icrop:CustomLabel ID="tradeInSummaryValueLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="42"/></th>
								<td class="contractHcv5001GradationBack1"><asp:Label ID="tradeInSummaryValueWordLabel" runat="server"/></td>
							</tr>

<%--2012/01/05 myose del start
                            <%'値引き額がゼロ以外の場合、値引き額欄を表示する %>
                            <% If isDiscountHiddenField.Value = "1" Then%>
							<tr class="contractHcv5001ChargeAreaItem2">
								<th class="contractHcv5001GradationBack1" colspan="2"><icrop:CustomLabel ID="discountLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="43"/></th>
								<td class="contractHcv5001GradationBack1"><%="-"%><asp:Label ID="discountWordLabel" runat="server"/></td>
							</tr>
                            <% End If%>
2012/01/05 myose del end--%>
                        </table>

                        <!--納車予定日-->
						<table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem4">
							<tr>
								<th><icrop:CustomLabel ID="carDeliveryDateLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="44"/></th>
								<td><asp:Label ID="carDeliveryDateWordLabel" runat="server"/></td>
							</tr>
						</table>

                        <!--支払い総額-->
						<table border="0" cellspacing="0" cellpadding="0" class="contractHcv5001ChargeAreaItem10">
			          	    <tr>
			          		    <th class="contractHcv5001GradationBack1"><icrop:CustomLabel ID="paymentSummaryLabel" runat="server" UseEllipsis="False" Width="240" CssClass="clip" TextWordNo="45"/></th>
			          		    <td class="contractHcv5001GradationBack1"><asp:Label ID="paymentSummaryWordLabel" runat="server"/></td>
		          		    </tr>
		          	    </table>
			        </div>
                </div>
            </div>
        </div>
  <%--  </div>--%>
</div>
		<!-- ここまでコンテンツ -->
            <asp:HiddenField ID="isDiscountHiddenField" runat="server" Visible="False" />
            <asp:HiddenField ID="optionNameHiddenField" runat="server" Visible="False" />
            <asp:HiddenField ID="optionPriceHiddenField" runat="server" Visible="False" />            
            <asp:HiddenField ID="tradeInNameHiddenField" runat="server" Visible="False" />
            <asp:HiddenField ID="tradeInPriceHiddenField" runat="server" Visible="False" />
            <asp:HiddenField ID="tradeInNumHiddenField" runat="server" Visible="False" />
  
</asp:Content>

