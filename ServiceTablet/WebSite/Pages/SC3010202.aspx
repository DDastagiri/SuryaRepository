<%@ Page Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3010202.aspx.vb" Inherits="PagesSC3010202" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   	<link rel="stylesheet" href="../Styles/Controls.css" />
    <link rel="Stylesheet" href="../Styles/ControlStyle.css" />
    <link rel="Stylesheet" href="../Styles/CommonMasterPage.css" />
    <link rel="stylesheet" href="../Styles/SC3010202/SC3010202.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3010202/SC3010202.js"></script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<div id="bodyFrame" style="width:296px;">
<h4 class="dashboardBoxTitle">
    <icrop:CustomLabel ID="Label_Title" runat="server" TextWordNo="0" 
        UseEllipsis="False"></icrop:CustomLabel>
</h4>
<p class="dashboardBoxTime">
<icrop:CustomLabel ID="Label_UpdateTime" runat="server" TextWordNo="0" 
        UseEllipsis="False"></icrop:CustomLabel>&nbsp;
<icrop:CustomLabel ID="Label_UpdateText" runat="server" TextWordNo="0" 
        UseEllipsis="False"></icrop:CustomLabel>
</p>
<p class="clearboth"></p>

<div id="dashboardBox" class="dashboardBoxAll">
							
	<table border="0" cellpadding="0" cellspacing="0" class="progress">
		<tr>
			<td valign="middle" align="left" class="Column06" colspan="3">
                <div class="hText01">
                <icrop:CustomLabel ID="Label_Subtitle_Action" runat="server" 
                        TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" >&nbsp;</td>
			<td valign="middle" >&nbsp;</td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_WalkIn" runat="server" TextWordNo="0" 
                        UseEllipsis="False" ForeColor="#666666"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">    
                    <icrop:CustomLabel ID="Label_WalkIn_Target" 
                        runat="server" Text="-" TextWordNo="0" UseEllipsis="False" 
                        ForeColor="#666666"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">    
                    <icrop:CustomLabel ID="Label_WalkIn_Result" 
                    runat="server" Text="-" TextWordNo="0" UseEllipsis="False" ForeColor="#666666"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_WalkIn_Graph"><span class="Bar01" style="width:<%=GraphWidthWalkIn%>px">&nbsp;</span></asp:PlaceHolder> 
                <span class="<%=CSSClassNameForGraphBar(GraphWidthWalkIn)%>">
                <icrop:CustomLabel ID="Label_WalkIn_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel></span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_Quotation" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <span class="hText03">
                <icrop:CustomLabel ID="Label_Quotation_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </span>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Quotation_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>    
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Quotation_Graph"><span class="Bar01" style="width:<%=GraphWidthQuotation%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthQuotation)%>">
                <icrop:CustomLabel ID="Label_Quotation_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_TestDrive" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_TestDrive_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_TestDrive_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_TestDrive_Graph"><span class="Bar01" style="width:<%=GraphWidthTestDrive%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthTestDrive)%>">
                <icrop:CustomLabel ID="Label_TestDrive_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_Evaluation" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Evaluation_Target" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Evaluation_Result" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Evaluation_Graph"><span class="Bar01" style="width:<%=GraphWidthEvaluation%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthEvaluation)%>">
                <icrop:CustomLabel ID="Label_Evaluation_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_Delivery" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Delivery_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Delivery_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>    
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Delivery_Graph"><span class="Bar01" style="width:<%=GraphWidthDelivery%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthDelivery)%>">
                <icrop:CustomLabel ID="Label_Delivery_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td colspan="3" valign="middle" class="Column06">
                <div class="hText01">
                    <icrop:CustomLabel ID="Label_Subtitle_Prospect" runat="server" 
                        TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" >&nbsp;</td>
			<td valign="middle" >&nbsp;</td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                <icrop:CustomLabel ID="Label_Cold" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Cold_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Cold_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Cold_Graph"><span class="Bar02" style="width:<%=GraphWidthCold%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthCold)%>">
                <icrop:CustomLabel ID="Label_Cold_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                    <icrop:CustomLabel ID="Label_Warm" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Warm_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
            <td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Warm_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Warm_Graph"><span class="Bar02" style="width:<%=GraphWidthWarm%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthWarm)%>">
                <icrop:CustomLabel ID="Label_Warm_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                    <icrop:CustomLabel ID="Label_Hot" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Hot_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>    
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Hot_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Hot_Graph"><span class="Bar02" style="width:<%=GraphWidthHot%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthHot)%>">
                <icrop:CustomLabel ID="Label_Hot_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td colspan="3" valign="middle" class="Column06">
                <div class="hText01">
                    <icrop:CustomLabel ID="Label_Subtitle_Sale" runat="server" 
                        TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" >&nbsp;</td>
			<td valign="middle" >&nbsp;</td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                    <icrop:CustomLabel ID="Label_Order" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Order_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>        
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Order_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>    
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Order_Graph"><span class="Bar03" style="width:<%=GraphWidthOrder%>px">&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthOrder)%>">
                <icrop:CustomLabel ID="Label_Order_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText02">
                    <icrop:CustomLabel ID="Label_Sale" runat="server" TextWordNo="0" 
                        UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Sale_Target" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03">
                <div class="hText03">
                <icrop:CustomLabel ID="Label_Sale_Result" 
                    runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>        
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05" >
                <asp:PlaceHolder runat="server" ID="Div_Sale_Graph"><span class="Bar03" style="width:<%=GraphWidthSale%>px" >&nbsp;</span></asp:PlaceHolder>
                <span class="<%=CSSClassNameForGraphBar(GraphWidthSale)%>">
                <icrop:CustomLabel ID="Label_Sale_Percent" runat="server" 
                    TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>%</span>
            </td>
		</tr>
	</table>
</div>
</div>
</asp:Content>
