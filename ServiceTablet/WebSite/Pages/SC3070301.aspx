<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3070301.aspx.vb" Inherits="Pages_SC3070301" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3070301.aspx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： 契約書印刷                                                         %>
<%'補足：                                                                    %>
<%'作成： 2011/12/01 TCS 相田                                                %>
<%'更新： 2012/02/03 TCS 藤井  【SALES_1A】号口(課題No.46)対応               %>
<%'─────────────────────────────────────%>
                                    

<link rel="Stylesheet" href="../Styles/SC3070301/SC3070301.css?20120203212800" media="screen,print" />
<link rel="Stylesheet" href="../Styles/SC3070301/common.css?20120117132200" media="screen,print" />

<script src="../Scripts/SC3070301/SC3070301.js?20120203212800" type="text/javascript"></script>

<script type="text/javascript">

</script> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server" overflow="scroll">
    <div id="Ncv5001Main">
            <div id="scrollInner" style="height:2500px; position:absolute; width:1024px; top:0px; left:0px;"> 
             
            <div id="contractHcv50PaperFrame">
                
                <div class="tcvNcvPointBigWindowBackFrameClose">
                    <asp:Button ID="CloseButton" runat="server" CssClass="closeButtonImage buttonNone" UseSubmitBehavior="False" OnClientClick="closeButtonClick()"/>
                </div>

                <div class="contractHcv50FootButtoms">
                    <asp:Button ID="PrintButton" runat="server" CssClass="buttonGlay1 buttonsNone stringCut" text-align="center" style="width:75px;height:30px"  UseSubmitBehavior="False" />
<%'2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 MODIFY START %>  
                    <asp:Button ID="SendButton" runat="server" CssClass="buttonGlay2 buttonsNone stringCut" text-align="center" style="width:75px;height:30px"  UseSubmitBehavior="True" OnClientClick="return sendButtonClick();"/>
                    <asp:Button ID="CancelButton" runat="server" CssClass="buttonGlay2 buttonsNone stringCut" text-align="center" style="width:75px;height:30px" UseSubmitBehavior="True" OnClientClick="return cancelButtonClick();"/>
<%'2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 MODIFY END %>  
		        </div>
            
                <div class="contractHcv50PaperTitle">
                    <!-- 店舗名 -->
                    <div class="tenpoClass">
                        广汽丰田第一店
                    </div> 
                    <!-- タイトル -->
                    <div class="contractHcv50TitleTxt">
                        车 辆 销 售 合 同
                    </div>
                    <div class="contractHcv50NoArea">
                        合同编号
                        <span class="contractHcv50InputFrame">
                            <icrop:CustomLabel ID="contractNoLabel" runat="server" Width="150px" UseEllipsis="true" />
                        </span>
                    </div>
                </div>
                <br />
                    <div id="contractHcv50PaperFrameBody">
				        <table border="0" cellspacing="0" cellpadding="0" class="contractHcv50AddressArea">
				          <tr>
				            <th class="contractHcv50AddressTitle01">
                                <div class="cutChara74"><icrop:CustomLabel ID="dealerNameLabel" runat="server" TextWordNo="102"></icrop:CustomLabel></div>
                                
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents01" align="left">
                                <icrop:CustomLabel ID="dealerNameWordLabel" runat="server" Width="295px" UseEllipsis="true" />
                            </td>

				            <th class="contractHcv50AddressTitle02">
                                <div class="cutChara157">
                                    <icrop:CustomLabel ID="buyerNameLabel" runat="server" TextWordNo="107"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td colspan="4" class="contractHcv50AddressContents02">
                                <div class="contractHcv50AddressInputFrameArea02">
                                    <icrop:CustomLabel ID="buyerNameWordLabel" runat="server" Width="300px" UseEllipsis="true" />
                                    
                                </div>
                            </td>
				          </tr>

				          <tr>
				            <th class="contractHcv50AddressTitle01">
                                <div class="cutChara74">
                                    <icrop:CustomLabel ID="dealerAddressLabel" runat="server" TextWordNo="103"></icrop:CustomLabel>
                                </div> 
                            </th> 
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents01">
                                <icrop:CustomLabel ID="dealerAddressWordLabel" runat="server" Width="295px" UseEllipsis="true" />

                            </td>

				            <th class="contractHcv50AddressTitle02">
                                <div class="cutChara157">
                                    <icrop:CustomLabel ID="buyerAddressLabel" runat="server" TextWordNo="108"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td colspan="4" class="contractHcv50AddressContents02">
                                <div class="contractHcv50AddressInputFrameArea02">
                                    <icrop:CustomLabel ID="buyerAddressWordLabel" runat="server" Width="300px" UseEllipsis="true" />
                                    
                                </div>
                            </td>
				          </tr>

				          <tr>
				            <th class="contractHcv50AddressTitle01">
                                <div class="cutChara74">
                                    <icrop:CustomLabel ID="dealerSalesHotLineLabel" runat="server" TextWordNo="104"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents01">
                                <icrop:CustomLabel ID="dealerSalesHotLineWordLabel" runat="server" Width="295px" UseEllipsis="true" />

                            </td>
				            <th class="contractHcv50AddressTitle02">
                                <div class="cutChara157">
                                    <icrop:CustomLabel ID="buyerIdLabel" runat="server" TextWordNo="109"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td colspan="4" class="contractHcv50AddressContents02">
                                <div class="contractHcv50AddressInputFrameArea02">
                                    <icrop:CustomLabel ID="buyerIdWordLabel" runat="server" Width="300px" UseEllipsis="true" />
                                    
                                </div>
                            </td>
				          </tr>

				          <tr>
				            <th class="contractHcv50AddressTitle01">
                                <div class="cutChara74">
                                    <icrop:CustomLabel ID="dealerServiceHotLineLabel" runat="server" TextWordNo="105"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents01">
                                <icrop:CustomLabel ID="dealerServiceHotLineWordLabel" runat="server" Width="295px" UseEllipsis="true" />
                               
                            </td>
				            <th class="contractHcv50AddressTitle02">
                                <div class="cutChara157">
                                    <icrop:CustomLabel ID="buyerTellHomeLabel" runat="server" TextWordNo="110"></icrop:CustomLabel>
                                </div> 
                            </th>
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents03">
                                <div class="contractHcv50AddressInputFrameArea03">
                                    <icrop:CustomLabel ID="buyerTellHomeWordLabel" runat="server" Width="110px" UseEllipsis="true" />

                                </div>
                            </td>
				            <td class="contractHcv50AddressTitle03">
                                <div class="cutChara74">
                                    <icrop:CustomLabel ID="buyerFaxLabel" runat="server" TextWordNo="112"></icrop:CustomLabel>
                                </div> 
                            </td>
				            <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents04">
                                <div class="contractHcv50AddressInputFrameArea04">
                                    <icrop:CustomLabel ID="buyerFaxWordLabel" runat="server" Width="110px" UseEllipsis="true" />
                                   
                                </div>
                            </td>
                          </tr>

				          <tr>
				            <th class="contractHcv50AddressTitle01">
                                <div class="cutChara74">
                                 <icrop:CustomLabel ID="dealerFaxLabel" runat="server" TextWordNo="106"></icrop:CustomLabel>
                                </div> 
                            </th> 
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents01">
                                <icrop:CustomLabel ID="dealerFaxWordLabel" runat="server" Width="295px" UseEllipsis="true" />

                            </td>
				            <th class="contractHcv50AddressTitle02">
                             <div class="cutChara157">
                                <icrop:CustomLabel ID="buyerTellMobileLabel" runat="server" TextWordNo="111"></icrop:CustomLabel>
                            </div> 
                            </th> 
			                <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents03">
                                <div class="contractHcv50AddressInputFrameArea03">
                                    <icrop:CustomLabel ID="buyerTellMobileWordLabel" runat="server" Width="110px" UseEllipsis="true" />
                                </div>
                            </td>
				            <td class="contractHcv50AddressTitle03">
                                <div class="cutChara74">
                                    <icrop:CustomLabel ID="buyerPostNoLabel" runat="server" TextWordNo="113"></icrop:CustomLabel>
                                </div> 
                            </td>
				            <td class="contractHcv50AddressColon01">：</td>
				            <td class="contractHcv50AddressContents04">
                                <div class="contractHcv50AddressInputFrameArea04">
                                    <icrop:CustomLabel ID="buyerPostNoWordLabel" runat="server" Width="110px" UseEllipsis="true" />
                                </div>
                            </td>
				          </tr>
				        </table>

				        <p>
                        甲、乙双方依据《中华人民共和国合同法》及相关法律法规的规定，在平等、自愿、协商一致的基础上，就买卖汽车事宜订立本合同，具体条款如下：
                        </p>
				        <!-- 第一条 -->
				        <div id="contractHcv50Condition1">
				          <h3>第一条：合同车辆</h3>
				          <table width="960px" border="0" cellspacing="0" cellpadding="0">
				            <tr>
				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="carNameLabel" runat="server" TextWordNo="114"></icrop:CustomLabel></div>
                              </th>
				              <td class="contractHcv50Condition1Contents01">
                                <div class="contractHcv50Condition1InputFrameArea">
                                    <icrop:CustomLabel ID="carNameWordLabel" runat="server" Width="160px" UseEllipsis="true" />
                                </div>
                              </td>

				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="gradeLabel" runat="server" TextWordNo="115"></icrop:CustomLabel></div>
                              </th>
				              <td class="contractHcv50Condition1Contents01">
                                <div class="contractHcv50Condition1InputFrameArea">
                                    <icrop:CustomLabel ID="gradeWordLabel" runat="server" Width="150px" UseEllipsis="true" />
                                </div>
                              </td>

				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="modelLabel" runat="server" TextWordNo="116"></icrop:CustomLabel></div>
                              </th>
				              <td class="contractHcv50Condition1Contents01">
                                <div class="contractHcv50Condition1InputFrameArea">
                                    <icrop:CustomLabel ID="modelWordLabel" runat="server" Width="150px" UseEllipsis="true" />
                                </div>
                              </td>

				              <th class="contractHcv50Condition1Title02">
                                    <div class="cutChara"><icrop:CustomLabel ID="suffixLabel" runat="server" TextWordNo="117"></icrop:CustomLabel></div>
                              </th>
                              <td class="contractHcv50Condition1Contents01">
                                <icrop:CustomLabel ID="suffixWordLabel" runat="server" Width="110px" UseEllipsis="true" />
                                    
                              </td>

				            </tr>

				            <tr>
				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="bodyColorLabel" runat="server" TextWordNo="118"></icrop:CustomLabel></div>
                              </th>
				              <td class="contractHcv50Condition1Contents01">
                                <div class="contractHcv50Condition1InputFrameArea">
                                    <icrop:CustomLabel ID="bodyColorWordLabel" runat="server" Width="150px" UseEllipsis="true" />
                                    
                                </div>
                              </td>

				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="interiorColorLabel" runat="server" TextWordNo="119"></icrop:CustomLabel></div>
                              </th>

                              <td class="contractHcv50Condition1Contents01">
                                <div class="contractHcv50Condition1InputFrameArea">
                                    <icrop:CustomLabel ID="interiorColorWordLabel" runat="server" Width="150px" UseEllipsis="true" />
                                   
                                </div>
                              </td>

				              <th class="contractHcv50Condition1Title01">
                                    <div class="cutChara"><icrop:CustomLabel ID="vinNoLabel" runat="server" TextWordNo="120"></icrop:CustomLabel></div>
                              </th> 
                              <td colspan="3" class="contractHcv50Condition1Contents03">
                                <div class="contractHcv50Condition3InputFrameArea">　</div>
                              </td>
				            </tr>
				          </table>
				        </div>
                        
                        

				        <!-- 第二条 -->
				        <div id="contractHcv50Condition2">
				          <h3>第二条：合同款项</h3>
				          <table width="960" border="0" cellspacing="0" cellpadding="0">
				            <tr>
				              <th class="contractHcv50Condition2Title01">项目</th>
				              <th class="contractHcv50Condition2Title01 border_left">金额[大写]</th>
				              <th class="contractHcv50Condition2Title01 border_left">金额[小写]</th>
				              <th class="contractHcv50Condition2Title01 border_left">备注</th>
				              </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                   <icrop:CustomLabel ID="vehicleAmountLabel" runat="server" TextWordNo="122"></icrop:CustomLabel>
                                </div> 
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="vehicleAmountKanjiLabel" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="vehicleAmountWordLabel" runat="server" Width="130px" UseEllipsis="true" />
                                </div>
			                    <div class="contractHcv50Condition2InputFrameAreaL">
                                    <icrop:CustomLabel ID="vehicleAmount" runat="server" TextWordNo="121"></icrop:CustomLabel>
                                </div>								  
                                <p class="clearboth">
                                </p>
                              </th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="vehicleAmountRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				            </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="optionLabel" runat="server" TextWordNo="123"></icrop:CustomLabel>
                                </div> 
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="optionKanjiLiteral" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">

                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="optionWordLabel" runat="server" Width="130px" UseEllipsis="true" />
                                </div>
                                
			                  <div class="contractHcv50Condition2InputFrameAreaL">
                                <icrop:CustomLabel ID="option" runat="server" TextWordNo="121"></icrop:CustomLabel>
                              </div>								  <p class="clearboth"></p></th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="optionRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				            </tr>


				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="insuranceCostsLabel" runat="server" TextWordNo="124"></icrop:CustomLabel>
                                </div>  
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="insuranceCostsKanjiLabel" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="insuranceCostsWordLabel" runat="server" Width="130px" UseEllipsis="true" />
                                </div>
			                  <div class="contractHcv50Condition2InputFrameAreaL">
                                <icrop:CustomLabel ID="insuranceCosts" runat="server" TextWordNo="121"></icrop:CustomLabel>
                              </div>								  <p class="clearboth"></p></th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="insuranceCostsRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				              </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="additionalCostsLabel" runat="server" TextWordNo="135"></icrop:CustomLabel>
                                </div> 
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="additionalCostsKanjiLabel" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="additionalCostsWordLabel" runat="server" Width="130px" UseEllipsis="true" />

                                </div>
			                  <div class="contractHcv50Condition2InputFrameAreaL">
                                <icrop:CustomLabel ID="additionalCosts" runat="server" TextWordNo="121"></icrop:CustomLabel>
                              </div>								  <p class="clearboth"></p></th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="additionalCostsRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				            </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="additionalCosts2Label" runat="server" TextWordNo="140"></icrop:CustomLabel>
                                </div> 
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="additionalCostsKanji2Label" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="additionalCosts2WordLabel" runat="server" Width="130px" UseEllipsis="true" />
                                </div>
			                    <div class="contractHcv50Condition2InputFrameAreaL">
                                    <icrop:CustomLabel ID="additionalCosts2" runat="server" TextWordNo="121"></icrop:CustomLabel>
                                </div>
                              </th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="additionalCosts2RemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				            </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="carCountLabel" runat="server" TextWordNo="136" CssClass="stringCut"></icrop:CustomLabel>
                                </div>
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="carCountKanjiLiteral" runat="server"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="carCountLiteral" runat="server" TextWordNo="217" ></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="carCountWordLabel" runat="server" Width="130px" UseEllipsis="true" />

                                </div>
			                    <div class="contractHcv50Condition2InputFrameAreaL">
                                    <icrop:CustomLabel ID="carCount" runat="server" TextWordNo="217"></icrop:CustomLabel>
                                </div>								  <p class="clearboth"></p></th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="carCountRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                               
                              </th>
				            </tr>

				            <tr>
				              <th class="contractHcv50Condition2Title02">
                                <div class="cutCharaTd">
                                    <icrop:CustomLabel ID="priceAmountLabel" runat="server" TextWordNo="126"></icrop:CustomLabel>
                                </div> 
                              </th>
				              <th class="contractHcv50Condition2Contents01">
                                <icrop:CustomLabel ID="priceAmountKanjiLabel" runat="server"></icrop:CustomLabel>
                              </th>
				              <th class="contractHcv50Condition2Contents02">
                                <div class="contractHcv50Condition2InputFrameArea">
                                    <icrop:CustomLabel ID="priceAmountWordLabel" runat="server" Width="130px" UseEllipsis="true" />
                                </div>
			                    <div class="contractHcv50Condition2InputFrameAreaL">
                                    <icrop:CustomLabel ID="priceAmount" runat="server" TextWordNo="121"></icrop:CustomLabel>   
                                </div>								  <p class="clearboth"></p></th>
				              <th class="contractHcv50Condition2Contents03">
                                <icrop:CustomLabel ID="priceAmountRemarksLabel" runat="server" Width="330px" UseEllipsis="true" />
                                
                              </th>
				            </tr>
				          </table>
				        </div>


				        <!-- 第三条 -->
				        <div id="contractHcv50Condition3">
				          <h3>第三条：付款方式</h3>
				          <p class="Padding_Right20">
                            本合同签订后，乙方通过以下支付方式[在
                            <asp:Image runat="server" id="checkOffImage" src="../Styles/Images/SC3070301/check_off.png" width="16" height="16" alt="" />
                            内打√]向甲方支付合同款项(仅限一种)：
                          </p>
				          <div class="contractHcv50Condition3Txt_Box">
                            <asp:Image runat="server" id="checkBoxOnInstallmentImg" src="../Styles/Images/SC3070301/check_on.png" width="16" height="16" alt="" />
                            <asp:Image runat="server" id="checkBoxOffInstallmentImg" src="../Styles/Images/SC3070301/check_off.png" width="16" height="16" alt="" />
                            乙方于合同签订当日支付订金[大写]
                            <span class="contractHcv50Underline">
                                <icrop:CustomLabel ID="depositKanjiLabel" runat="server" Width="300px" UseEllipsis="True"></icrop:CustomLabel>
                            </span>
                             [ 
                             <span class="contractHcv50InputFrame">
                                <icrop:CustomLabel ID="depositLabel" runat="server" Width="130px" UseEllipsis="True"></icrop:CustomLabel>
                             </span> 
                             <icrop:CustomLabel ID="deposit" runat="server" TextWordNo="121"></icrop:CustomLabel>
                             ]。
                             <br />
				            &nbsp;&nbsp;&nbsp;&nbsp;余款
                            <span class="contractHcv50Underline">
                                <icrop:CustomLabel ID="onlyPayKanjiLabel" runat="server" Width="300px" UseEllipsis="True"></icrop:CustomLabel>
                            </span>
                            [
                            <span class="contractHcv50InputFrame">
                                <icrop:CustomLabel ID="onlyPayWordLabel" runat="server" Width="130px" UseEllipsis="True"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="onlyPay" runat="server" TextWordNo="121"></icrop:CustomLabel>
                            </span> 
                            ]在提车前结清。甲方收到全款后，开具有效发票。
                            <br />
			              </div>

				          <p class="clearboth"></p>
				          <div class="contractHcv50Condition3Txt_Box">
                            <asp:Image runat="server" id="checkBoxOnlumpSumImg" src="../Styles/Images/SC3070301/check_on.png" width="16" height="16" alt="" />
                            <asp:Image runat="server" id="checkBoxOfflumpSumImg" src="../Styles/Images/SC3070301/check_off.png" width="16" height="16" alt="" />
                            乙方于合同签订当日支付全额合同款项[大写] 
                            <icrop:CustomLabel ID="lumpSumKanjiLiteral" runat="server" Width="300px" UseEllipsis="True"></icrop:CustomLabel>
                            <span class="contractHcv50Underline">
                             小写　
                            </span>
                             [ 
                            <span class="contractHcv50Underline">
                                <icrop:CustomLabel ID="lumpSumLiteral" runat="server" Width="130px" UseEllipsis="True"></icrop:CustomLabel>
                            </span>
                             <icrop:CustomLabel ID="LumpSumPayment" runat="server" TextWordNo="121"></icrop:CustomLabel>
                             ]<br />&nbsp;&nbsp;&nbsp;&nbsp;甲方收到全款后，开具有效发票。
                          </div>
				          <p class="clearboth"></p>
				          <div class="contractHcv50Condition3Txt_Box">
                          <asp:Image runat="server" id="checkBoxOnLoanImg" src="../Styles/Images/SC3070301/check_on.png" width="16" height="16" alt="" />
                          <asp:Image runat="server" id="checkBoxOffLoanImg" src="../Styles/Images/SC3070301/check_off.png" width="16" height="16" alt="" />
                          乙方于合同签订当日支付订金[大写]
                           <span class="contractHcv50Underline">
                               <icrop:CustomLabel ID="loanDayKanjiLiteral" runat="server" Width="300px" UseEllipsis="True"></icrop:CustomLabel>
                           </span>
                           小写　[ 
                           <span class="contractHcv50InputFrame contractHcv50Underline">
                            <icrop:CustomLabel ID="loanDayLiteral" runat="server" Width="130px" UseEllipsis="True"></icrop:CustomLabel>
                            </span>
                             <icrop:CustomLabel ID="loanDay" runat="server" TextWordNo="121"></icrop:CustomLabel>
                             ]，并于按揭<br />
                             &nbsp;&nbsp;&nbsp;&nbsp;申请通过后
                             <span class="contractHcv50InputFrame contractHcv50Underline">
                                <icrop:CustomLabel ID="loanTimeForPaymentLabel" runat="server" Width="20px" UseEllipsis="True"></icrop:CustomLabel>
                             </span> 
                             天内向甲方支付金融公司/银行批核的车价首付款[减除订金部分] 以及除车价外的其他合同款项。<br />
				            &nbsp;&nbsp;&nbsp;&nbsp;甲方收到全款后开具有效发票。待金融公司/银行为乙方支付的款项部分[即车价的贷款部分] 到达甲方帐户后，<br />
				            &nbsp;&nbsp;&nbsp;&nbsp;甲方及时安排交付车辆。若乙方自办按揭，则需全款到账后，方可办理上牌。<br />
				          </div>
				          <p class="clearboth"></p>
				        </div>

				        <!-- 第四条 -->
				        <div id="contractHcv50Condition4">
				          <h3>第四条：交车时间及交车地点</h3>
				          <p>
                          甲乙双方约定交车时间为
                          <span class="contractHcv50InputFrame contractHcv50Underline">
                            <icrop:CustomLabel ID="DeliveryDateLabel" runat="server" Width="100px" UseEllipsis="True"></icrop:CustomLabel>
                          </span>
                          [此时间为最迟交车时间[不含上牌时间]，甲方将尽量争取提前交车]。
                          <br />交车地点：广州市黄埔大道中243号[员村四横路口正对面]。</p>
				          </div>

				        <!-- 第五条 -->
				        <div id="contractHcv50Condition5">
				          <h3>第五条：保修约定</h3>
				          <p>关于整车、零部件总成的保修期限执行生产厂保修条款的规定。</p>
				          </div>
				        <!-- 第六条 -->
				        <div id="contractHcv50Condition6">
				          <h3>第六条：车辆质量</h3>
				          <ul>
				            <li>甲方向乙方出售的车辆，其质量必须符合国家汽车产品标准或行业标准，并符合出厂检验标准，符合安全驾驶和说明书载明的基本使用要求。</li>
				            <li>双方对车辆质量的认定有争议的，以国家汽车质量监督检验中心[各地方]的书面鉴定意见为处理争议的依据。</li>
				            </ul>
				          </div>

                                <!-- 第七条 -->
				        <div id="contractHcv50Condition7">
				          <h3>第七条：附营业务</h3>
				          <ul>
				            <li>附营业务指的是甲方为乙方提供的精品加装、代购保险、代办上牌等业务。</li>
				            <li>由甲方安装的精品，质保期根据精品生产厂家公布的为准。</li>
				            <li>由甲方代购的保险，发生车辆事故时的报案、出险、赔付等标准及流程，以相关事项发生时保险公司实施政策的为准。</li>
				            <li>关于甲方代办的上牌服务：<br />
				              ①甲方代办服务不包括选号，选号环节必须由车主本人[单位/公司/组织等为经办人]参与。<br />
                              ②若车辆在上牌过程中受损，甲方协助修复该车辆，费用由造成车辆损伤的责任方承担；若涉及第三方损失/损伤，则通过保险解决。</li>
				            </ul>
				          </div>

			<br />
                        <!-- ページ -->
                        <div class="pageNumClass">
                            第 1 页，共 2 页
                        </div> 
                        <br />
                        <!-- 店舗名 -->
                        <div class="tenpoClass">
                            广汽丰田第一店
                        </div> 
                        <br />

				        <!-- 第八条 -->
				        <div id="contractHcv50Condition8">
				          <h3>第八条：不可抗力</h3>
				          <ul>
				            <li>任何一方对由于不可抗力[地震、台风、战争、大规模瘟疫等]造成的部分或全部不能履行本合同不负责任。但迟延履行后发生不可抗力的，不能免除责任。</li>
				            <li>遇有不可抗力的一方，应在三日内将事件的情况以书面形式通知另一方，并在事件发生后十日内，向另一方提交合同不能履行或部分不能履行或需要延期履行理由的报告。</li>
				            </ul>
				          </div>


				

				        <!-- 第九条 -->
				        <div id="contractHcv50Condition9">
				          <h3>第九条：违约责任</h3>
				          <ul><li>乙方不能按时支付车款，自延期之日起超过七天的，甲方有权解除合同。</li>
				            <li>甲方不能按时交付车辆，自延期之日起超过七天的，乙方有权解除合同。</li>
				            <li>如乙方在本合同约定交车时间前，要求变更所订的车辆型号、颜色，本合同交车期自动顺延2个月。如要求变更车主，本合同交车期自动顺延1个月。</li>
				            <li>经国家授权的汽车检验机构鉴定，乙方所购汽车确实存在设计、制造缺陷，由此缺陷造成的人身和财产损害，如甲方无过错，乙方有权向生产厂主张赔偿，甲方有积极协助的义务。若甲方在该车有缺陷或存在其他特殊的使用要求时，应该明示告知而未明示告知，则应承担相应赔偿责任。</li>
				            <li>在本合同签定之日起至合同交车最后期限之前，双方必须履行本合同要求，如双方任意一方违约，将扣除订金的20%作为违约金处理。</li>
				            </ul>
				          </div>

				        <!-- 第十条 -->
				        <div id="contractHcv50Condition10">
				          <h3>第十条：合同争议的解决方法</h3>
				          <ul>
				            <li>本合同引起的争议由双方友好协商解决。</li>
				            <li>争议不能解决时，双方可以依法提出仲裁[仲裁是终局的]或提起诉讼。</li>
				          </ul>
				        </div>


				        <!-- 第十一条 -->
				        <div id="contractHcv50Condition11">
				          <h3>第十一条：其他约定</h3>
				          <ul>
				            <li>双方前列地址、电话如有改变，须及时书面通知对方。因一方延迟通知而造成的损失，由过错方承担责任。</li>
				            <li>购车的所有款项以本合同签订时为准，因市场价格变动或因合同签订后甲方推出的各种优惠活动，造成合同签订前后与此车型售价有差异的，甲方不承担乙方的差价损失。</li>
				            <li>若乙方自行办理按揭手续，需合同全款到达甲方账户后，甲方方可开具有效发票并安排交付车辆。</li>
				            <li>若出现多付车款情况，甲方将在收到多付款项后15个工作日内将多收款项退回支付方。</li>
				            <li>合同车辆的附件资料中不含临时牌照；即，临时牌照需开具整车销售发票后，另行办理。</li>
				            <li>本合同的未尽事宜及本合同在履行过程中需变更的事项，双方应通过订立补充条款或补充协议进行约定。本合同的补充条款、补充协议及附件均为本合同不可分割的部分，与本合同具有同等效力。</li>
				            <li>本合同的其他补充约定：</li>
				          </ul>

                          <br />
                          <br />
                          <br />

				          <div class="Txt_Area">&nbsp;</div>

				          </div>

				        <div class="contractHcv50FooterArea">
				          <p class="contractHcFooterTxt">
                            本合同自双方签字或盖章之日起生效，但非经销售经理确认签署该合同不生效。本合同壹式<span class="underlineClass">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>份，具有同等法律效力。其中，甲方执<span class="underlineClass">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>份，乙方执<span class="underlineClass">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>份。
                          </p>
				          <div class="contractHcv50NameArea">
				            <div class="contractHcv50LeftArea">
				              <table width="400" border="0" cellspacing="0" cellpadding="0">
				                <tr>
				                  <td class="contractHcv50NameAreaTitle01">
                                    甲方[盖章]
                                  </td>
				                  <td class="contractHcv50NameAreaColon01">:</td>
				                  <td class="contractHcv50NameAreaName01">&nbsp;</td>
				                </tr>

				                <tr>
				                  <td class="contractHcv50NameAreaTitle01">
                                    销售顾问[签字]
                                  </td>
				                  <td class="contractHcv50NameAreaColon01">:</td>
				                  <td class="contractHcv50NameAreaName01">&nbsp;</td>
				                </tr>

				                <tr>
				                  <td class="contractHcv50NameAreaTitle01">
                                    销售经理[签字]
                                  </td>
				                  <td class="contractHcv50NameAreaColon01">:</td>
				                  <td class="contractHcv50NameAreaName01">&nbsp;</td>
				                </tr>
				              </table>
				              <p class="contractHcv50NameAreaDay">
                                日期：　　　　　　年　　　　　　月　　　　　　日　
                              </p>
			                </div>

				            <div class="contractHcv50RightArea">
				              <table width="400" border="0" cellspacing="0" cellpadding="0">
				                <tr>
				                  <td class="contractHcv50NameAreaTitle01">
                                    乙方[签字/盖章]
                                  </td>
				                  <td class="contractHcv50NameAreaColon01">:</td>
				                  <td class="contractHcv50NameAreaName01">&nbsp;</td>
				                </tr>

				                <tr>
				                  <td class="contractHcv50NameAreaTitle01">
                                    法人/代理人[签字]
                                  </td>
				                  <td class="contractHcv50NameAreaColon01">:</td>
				                  <td class="contractHcv50NameAreaName01">&nbsp;</td>
				                </tr>

				                <tr>
				                  <td class="contractHcv50NameAreaTitle01" style="color:White">
                                    
                                  </td>
				                  <td class="contractHcv50NameAreaColon01"></td>
				                  <td class="contractHcv50NameAreaTitle01">&nbsp;</td>
				                </tr>
				              </table>
				              <p class="contractHcv50NameAreaDay">
                                日期：　　　　　　年　　　　　　月　　　　　　日　
                              </p>
			                </div>
				            <p class="clearboth"></p>
				            </div>
				          </div> 
                          <!-- ページ -->
                          <br />
                          <div class="pageNum2Class">
                                第 2 页，共 2 页
                          </div> 
                </div>
         </div>
      </div>
      <%'印刷フラグ %>
      <asp:HiddenField ID="printFlgHiddenField" runat="server" />
    </div>
<%'2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD START %>    
      <%'ボタン押下時のメッセージ %>
    <asp:HiddenField ID="sendCheckMsg" runat="server" />
    <asp:HiddenField ID="cancelCheckMsg" runat="server" />

      <%'サーバー処理中のオーバーレイとアイコン %>
    <div id="serverProcessOverlayBlack"></div>
    <div id="serverProcessIcon"></div>
<%'2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD END %>    

</asp:Content>
