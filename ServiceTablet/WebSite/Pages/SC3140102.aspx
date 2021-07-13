<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3140102.aspx.vb" Inherits="Pages_SC3140102" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%'スタイルシート %>
    <link rel="stylesheet" href="../Styles/SC3140102/SC3140102.css" type="text/css" media="screen,print" />

    <script type="text/javascript" src="../Scripts/SC3140102/SC3140102.js?20190314000001"></script>
    <script type="text/javascript" src="../Scripts/SC3140102/SC3140102.flickable.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <%'ダッシュボードエリア %>
    <%'前月の目標と進捗率、当月の目標と進捗率 %>
    <div id="dashboardBoxMainAreaStyle" class="dashboardBoxMainAreaStyle" >
        <div id="construction" class="construction">
            <div class="back"></div>
            <div class="text"><icrop:CustomLabel ID="LabelStop" runat="server" TextWordNo="13" UseEllipsis="False"></icrop:CustomLabel></div>
        </div>

        <div id="dashboardBoxflick">
            <ul>
                <%'前月の目標と進捗率 %>
                <li class="rightPadding">
                    <h4 class="dashboardBoxTitle">
                        <icrop:CustomLabel ID="Label_Title_LastMonth" runat="server" TextWordNo="11" UseEllipsis="True" Font-Size="10.5pt" ForeColor="#666666" Width="120"></icrop:CustomLabel>
                    </h4>
                    <p class="dashboardBoxTime">
                        <icrop:CustomLabel ID="Label_UpdateTime_LastMonth" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="Label_UpdateText_LastMonth" runat="server" TextWordNo="2" UseEllipsis="True" Width="25"></icrop:CustomLabel>
                    </p>

                    <p class="clearboth"></p>

		            <div id="PredashboardBox" class="dashboardBoxAll">
						
			            <table border="0" cellpadding="0" cellspacing="0" class="progress">
				            <tr>
					            <td valign="middle" align="left" colspan="3" class="Column08">&nbsp;</td>
					            <td colspan="2" valign="middle" class="Column08">
                                    <div class="Percent">
						                <div class="Percent0">0%</div>
						                <div class="Percent100">100%</div>
						                <div class="Percent200">200%</div>
					                </div>
                                </td>
				            </tr>
                            <%'入庫台数(台) %>
				            <tr>
					            <td colspan="3" valign="middle" class="Column06">
                                <table width="102" border="0" cellpadding="0" cellspacing="0">
					                <tr>
						                <td>
                                            <icrop:CustomLabel ID="Label_PreviewsWarehousingNumber" runat="server" TextWordNo="3" UseEllipsis="True" Width="60"></icrop:CustomLabel>
                                        </td>
						                <td align="right">&nbsp;</td>
					                </tr>
					            </table>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">&nbsp;</td>
				            </tr>
                            <%'入庫台数(台)-合計 %>
				            <tr>
					            <td colspan="3" valign="middle" class="Column07">
                                <table width="102" border="0" cellpadding="0" cellspacing="0">
					                <tr>
						                <td>
                                            <icrop:CustomLabel ID="Label_PreviewsWarehousingNumberTotal" runat="server" TextWordNo="4" UseEllipsis="True" Width="25"></icrop:CustomLabel>
                                        </td>
						                <td align="right">
                                            <icrop:CustomLabel ID="Label_PreviewsWarehousingNumberTotal_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                        </td>
						                <td width="37" align="right">
                                            <icrop:CustomLabel ID="Label_PreviewsWarehousingNumberTotal_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                        </td>
					                </tr>
					            </table>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsWarehousingNumberTotal_Graph">
                                        <span class="Bar01" style="width:<%=GraphWidthPreviewsWarehousingNumberTotal%>px; height: 12px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsWarehousingNumberTotal)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsWarehousingNumberTotal_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                         
                                </td>
				            </tr>
                            <%'入庫台数(台)-定期点検 %>
				            <tr>
					            <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_PreviewsCheck" runat="server" TextWordNo="5"  UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_PreviewsCheck_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_PreviewsCheck_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsCheck_Graph">
                                        <span class="Bar01"  style="width:<%=GraphWidthPreviewsCheck%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsCheck)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsCheck_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
				            </tr>
                            <%'入庫台数(台)-一般整備 %>
				            <tr>
					            <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_PreviewsMaintenance" runat="server" TextWordNo="6" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_PreviewsMaintenance_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_PreviewsMaintenance_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsMaintenance_Graph">
                                        <span class="Bar01" style="width:<%=GraphWidthPreviewsMaintenance%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsMaintenance)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsMaintenance_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
				            </tr>
                            <%'入庫売上(千円) %>
				            <tr>
					            <td colspan="3" valign="middle" class="Column06">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
						                <tr>
						                    <td valign="bottom">
                                                <icrop:CustomLabel ID="PreviewsWarehousingSale" runat="server" TextWordNo="7" UseEllipsis="True" Width="70"></icrop:CustomLabel>
                                            </td>
						                    <td align="right" valign="bottom">&nbsp;</td>
						                </tr>
					                </table>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">&nbsp;</td>
				            </tr>
                            <%'入庫売上(千円)-合計 %>
				            <tr>
					            <td colspan="3" valign="middle" class="Column07">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
					                    <tr>
						                    <td>
                                                <icrop:CustomLabel ID="Label_PreviewsSaleTotal" runat="server" TextWordNo="8"  UseEllipsis="True" Width="25"></icrop:CustomLabel>
                                            </td>
						                    <td align="right">
                                                <icrop:CustomLabel ID="Label_PreviewsSaleTotal_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
						                    <td width="37" align="right">
                                                <icrop:CustomLabel ID="Label_PreviewsSaleTotal_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
					                    </tr>
					                </table>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsSaleTotal_Graph">
                                        <span class="Bar02" style="width:<%=GraphWidthPreviewsSaleTotal%>px; height: 12px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsSaleTotal)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsSaleTotal_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                
                                </td>
				            </tr>
                            <%'入庫売上(千円)-定期点検 %>
				            <tr>
					            <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleCheck" runat="server" TextWordNo="9"  UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleCheck_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleCheck_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsSaleCheck_Graph">
                                        <span class="Bar02"  style="width:<%=GraphWidthPreviewsSaleCheck%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsSaleCheck)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsSaleCheck_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>                
                                </td>
				            </tr>
                            <%'入庫売上(千円)-一般整備 %>
				            <tr>
					            <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleMaintenance" runat="server" TextWordNo="10" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleMaintenance_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_PreviewsSaleMaintenance_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					            <td valign="middle" class="Column04">&nbsp;</td>
					            <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_PreviewsSaleMaintenance_Graph">
                                        <span class="Bar02"  style="width:<%=GraphWidthPreviewsSaleMaintenance%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthPreviewsSaleMaintenance)%>">
                                        <icrop:CustomLabel ID="Label_PreviewsSaleMaintenance_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>                                
                                </td>
				            </tr>
			            </table>
		            </div>
                </li>
                <%'当月の目標と進捗率 %>
                <li>
                    <h4 class="dashboardBoxTitle">
                        <icrop:CustomLabel ID="Title_ThisMonth" runat="server" TextWordNo="1" UseEllipsis="True" Font-Size="10.5pt" ForeColor="#666666"  Width="135"></icrop:CustomLabel>
                    </h4>
                    <p class="dashboardBoxTime">
                        <icrop:CustomLabel ID="Label_UpdateTime_ThisMonth" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="Label_UpdateText_ThisMonth" runat="server" TextWordNo="2" UseEllipsis="True" Width="25"></icrop:CustomLabel>
                    </p>

                    <p class="clearboth"></p>

			        <div id="NowdashboardBox" class="dashboardBoxAll">
						
				        <table border="0" cellpadding="0" cellspacing="0" class="progress">
					        <tr>
						        <td valign="middle" align="left" colspan="3" class="Column08">&nbsp;</td>
						        <td colspan="2" valign="middle" class="Column08">
                                    <div class="Percent">
							            <div class="Percent0">0%</div>
							            <div class="Percent100">100%</div>
							            <div class="Percent200">200%</div>
						            </div>
                                </td>
					        </tr>
                            <%'入庫台数(台) %>
					        <tr>
						        <td colspan="3" valign="middle" class="Column06">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
						                <tr>
							                <td>
                                                <icrop:CustomLabel ID="Label_NowWarehousingNumber" runat="server" TextWordNo="3" UseEllipsis="True" Width="60"></icrop:CustomLabel>
                                            </td>
							                <td align="right">
                                                <icrop:CustomLabel ID="Label_NowWarehousingNumber_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
						                </tr>
						            </table>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">&nbsp;</td>
					        </tr>
                            <%'入庫台数(台)-合計 %>
					        <tr>
						        <td colspan="3" valign="middle" class="Column07">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
						                <tr>
							                <td>
                                                <icrop:CustomLabel ID="Label_NowWarehousingNumberTotal" runat="server" TextWordNo="4" UseEllipsis="True" Width="25"></icrop:CustomLabel>
                                            </td>
							                <td align="right">
                                                <icrop:CustomLabel ID="Label_NowWarehousingNumberTotal_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
							                <td width="37" align="right">
                                                <icrop:CustomLabel ID="Label_NowWarehousingNumberTotal_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
						                </tr>
						            </table>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowWarehousingNumberTotal_Graph">
                                        <span class="Bar01"  style="width:<%=GraphWidthNowWarehousingNumberTotal%>px; height: 12px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowWarehousingNumberTotal)%>">
                                        <icrop:CustomLabel ID="Label_NowWarehousingNumberTotal_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
                            <%'入庫台数(台)-定期点検 %>
					        <tr>
						        <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_NowCheck" runat="server" TextWordNo="5" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                 </td>
						        <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_NowCheck_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_NowCheck_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowCheck_Graph">
                                        <span class="Bar01"  style="width:<%=GraphWidthNowCheck%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowCheck)%>">
                                        <icrop:CustomLabel ID="Label_NowCheck_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
                            <%'入庫台数(台)-一般整備 %>
					        <tr>
						        <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_NowMaintenance" runat="server" TextWordNo="6" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_NowMaintenance_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_NowMaintenance_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowMaintenance_Graph">
                                        <span class="Bar01"  style="width:<%=GraphWidthNowMaintenance%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowMaintenance)%>">
                                        <icrop:CustomLabel ID="Label_NowMaintenance_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
                            <%'入庫売上(千円) %>
					        <tr>
						        <td colspan="3" valign="middle" class="Column06">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
							            <tr>
							                <td valign="bottom">
                                                <icrop:CustomLabel ID="Label_NowWarehousingSale" runat="server" TextWordNo="7" UseEllipsis="True" Width="70"></icrop:CustomLabel>
                                            </td>
							                <td align="right" valign="bottom" >
                                                <icrop:CustomLabel ID="Label_NowWarehousingSale_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
							            </tr>
						            </table>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">&nbsp;</td>
					        </tr>
                            <%'入庫売上(千円)-合計 %>
					        <tr>
						        <td colspan="3" valign="middle" class="Column07">
                                    <table width="102" border="0" cellpadding="0" cellspacing="0">
						                <tr>
							                <td><icrop:CustomLabel ID="Label_NowSaleTotal" runat="server" TextWordNo="8"  UseEllipsis="True" Width="25"></icrop:CustomLabel></td>
							                <td align="right">
                                                <icrop:CustomLabel ID="Label_NowSaleTotal_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
							                <td width="37" align="right">
                                                <icrop:CustomLabel ID="Label_NowSaleTotal_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                            </td>
						                </tr>
						            </table>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowSaleTotal_Graph">
                                        <span class="Bar02"  style="width:<%=GraphWidthNowSaleTotal%>px; height: 12px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowSaleTotal)%>">
                                        <icrop:CustomLabel ID="Label_NowSaleTotal_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
                            <%'入庫売上(千円)-定期点検 %>
					        <tr>
						        <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_NowSaleCheck" runat="server" TextWordNo="9" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_NowSaleCheck_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_NowSaleCheck_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowSaleCheck_Graph">
                                        <span class="Bar02" style="width:<%=GraphWidthNowSaleCheck%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowSaleCheck)%>">
                                        <icrop:CustomLabel ID="Label_NowSaleCheck_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
                            <%'入庫売上(千円)-一般整備 %>
					        <tr>
						        <td valign="middle" class="Column01">
                                    <icrop:CustomLabel ID="Label_NowSaleMaintenance" runat="server" TextWordNo="10" UseEllipsis="True" Width="40"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column02">
                                    <icrop:CustomLabel ID="Label_NowSaleMaintenance_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column03">
                                    <icrop:CustomLabel ID="Label_NowSaleMaintenance_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
						        <td valign="middle" class="Column04">&nbsp;</td>
						        <td valign="middle" class="Column05">
                                    <asp:PlaceHolder runat="server" ID="Div_NowSaleMaintenance_Graph">
                                        <span class="Bar02" style="width:<%=GraphWidthNowSaleMaintenance%>px;">&nbsp;</span>
                                    </asp:PlaceHolder>
                                    <span class="<%=CSSClassNameForGraphBar(GraphWidthNowSaleMaintenance)%>">
                                        <icrop:CustomLabel ID="Label_NowSaleMaintenance_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                                </td>
					        </tr>
				        </table>
			        </div>
                </li>
            </ul>
        </div>
    </div>
    <%'当日の目標と進捗率 %>
    <div id="dashboardBoxSubAreaStyle" class="dashboardBoxSubAreaStyle" >
		<h4 class="dashboardBoxTitle">
            <icrop:CustomLabel ID="Title_Today" runat="server" TextWordNo="12" UseEllipsis="True" Font-Size="10.5pt" ForeColor="#666666"  Width="135"></icrop:CustomLabel>
        </h4>
		<p class="dashboardBoxTime">                    
            <icrop:CustomLabel ID="Label_UpdateTime_Today" runat="server" UseEllipsis="False"></icrop:CustomLabel>
            <icrop:CustomLabel ID="Label_UpdateText_Today" runat="server" TextWordNo="2" UseEllipsis="True" Width="25"></icrop:CustomLabel>
        </p>
		<p class="clearboth"></p>
		<div id="dashboardBox" class="dashboardBoxHalf">
		    <table border="0" cellpadding="0" cellspacing="0" class="progress">
			    <tr>
			    <td valign="middle" align="left" colspan="3" class="Column09">&nbsp;</td>
			    <td colspan="2" valign="top" class="Column09">
                    <div class="PercentB">
				        <div class="Percent0">0%</div>
				        <div class="Percent100">100%</div>
				        <div class="Percent200">200%</div>
			        </div>
                </td>
			    </tr>
                <%'入庫台数(台) %>
			    <tr>
                	<td colspan="3" valign="middle" class="Column06B">
                        <table width="102" border="0" cellpadding="0" cellspacing="0">
					        <tr>
			                    <td valign="middle" class="Column01B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingNumber" runat="server" TextWordNo="3" UseEllipsis="True" Width="60"></icrop:CustomLabel>
                                </td>
			                    <td valign="middle" class="Column02B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingNumber_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
			                    <td valign="middle" class="Column03B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingNumber_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                               </td>
					        </tr>
                       </table>
                   </td>
			        <td valign="middle" class="Column04">&nbsp;</td>
			        <td valign="middle" class="Column05">
                        <asp:PlaceHolder runat="server" ID="Div_TodayWarehousingNumber_Graph">
                            <span class="Bar01" style="width:<%=GraphWidthTodayWarehousingNumber%>px;">&nbsp;</span>
                        </asp:PlaceHolder>
                        <span class="<%=CSSClassNameForGraphBar(GraphWidthTodayWarehousingNumber)%>">
                            <icrop:CustomLabel ID="Label_TodayWarehousingNumber_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                    </td>
			    </tr>
                <%'入庫売上(千円) %>
			    <tr>
                	<td colspan="3" valign="middle" class="Column06B">
                        <table width="102" border="0" cellpadding="0" cellspacing="0">
					        <tr>
			                    <td valign="middle" class="Column01B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingSale" runat="server" TextWordNo="7" UseEllipsis="True" Width="62"></icrop:CustomLabel>
                               </td>
			                    <td valign="middle" class="Column02B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingSale_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
			                    <td valign="middle" class="Column03B">
                                    <icrop:CustomLabel ID="Label_TodayWarehousingSale_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                                </td>
					        </tr>
                       </table>
                   </td>
			        <td valign="middle" class="Column04">&nbsp;</td>
			        <td valign="middle" class="Column05">
                        <asp:PlaceHolder runat="server" ID="Div_TodayWarehousingSale_Graph">
                            <span class="Bar02"  style="width:<%=GraphWidthTodayWarehousingSale%>px;">&nbsp;</span>
                        </asp:PlaceHolder>
                        <span class="<%=CSSClassNameForGraphBar(GraphWidthTodayWarehousingSale)%>">
                            <icrop:CustomLabel ID="Label_TodayWarehousingSale_Percent" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
                    </td>
			    </tr>
		    </table>
		</div>
    </div>
</asp:Content>

