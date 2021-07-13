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
<div class="dashboardBoxTime">
    <icrop:CustomLabel ID="Label_UpdateTime" runat="server" TextWordNo="0" 
            UseEllipsis="False"></icrop:CustomLabel>&nbsp;
    <icrop:CustomLabel ID="Label_UpdateText" runat="server" TextWordNo="0" 
            UseEllipsis="False"></icrop:CustomLabel>
</div>
<p class="clearboth"></p>

<div id="dashboardBox" class="dashboardBoxAll">
    <div class="tableHeadTexts">
        <div class="hText01">&nbsp;</div>
        <div class="hText02">0%</div>
        <div class="hText03">50%</div>
        <div class="hText04">100%</div>
        <div class="hText05">150%</div>
        <div class="clearboth">&nbsp;</div>
    </div>	

	<table border="0" cellpadding="0" cellspacing="0" class="progress">
        <tr>
			<td valign="middle" class="Column01">
                <div class="hText06">
                    <icrop:CustomLabel ID="Label_WalkIn" runat="server" TextWordNo="0" UseEllipsis="False" />
                </div>
            </td>
			<td valign="middle" class="Column02" style="font-size:<%=TargetFontSizeWalkIn%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_WalkIn_Target" runat="server" TextWordNo="0" UseEllipsis="False" />
                </div>
            </td>
			<td valign="middle" class="Column03" style="font-size:<%=ResultFontSizeWalkIn%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_WalkIn_Result" runat="server" TextWordNo="0" UseEllipsis="False"/>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_WalkIn_Graph">
                    <span class="Bar01" style="width:<%=GraphWidthWalkIn%>px">&nbsp;</span>
                </asp:PlaceHolder> 
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText06">
                    <icrop:CustomLabel ID="Label_Evaluation" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02" style="font-size:<%=TargetFontSizeEvaluation%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Evaluation_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03" style="font-size:<%=ResultFontSizeEvaluation%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Evaluation_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Evaluation_Graph">
                    <span class="Bar01" style="width:<%=GraphWidthEvaluation%>px">&nbsp;</span>
                </asp:PlaceHolder>
            </td>
		</tr>
        		<tr>
			<td valign="middle" class="Column01">
                <div class="hText06">
                    <icrop:CustomLabel ID="Label_TestDrive" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02" style="font-size:<%=TargetFontSizeTestDrive%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_TestDrive_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03" style="font-size:<%=ResultFontSizeTestDrive%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_TestDrive_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_TestDrive_Graph">
                    <span class="Bar01" style="width:<%=GraphWidthTestDrive%>px">&nbsp;</span>
                </asp:PlaceHolder>
            </td>
		</tr>
		<tr>
			<td valign="middle" class="Column01">
                <div class="hText06">
                    <icrop:CustomLabel ID="Label_Order" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02" style="font-size:<%=TargetFontSizeOrder%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Order_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03" style="font-size:<%=ResultFontSizeOrder%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Order_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Order_Graph">
                    <span class="Bar01" style="width:<%=GraphWidthOrder%>px">&nbsp;</span>
                </asp:PlaceHolder>
            </td>
		</tr>
        <tr>
			<td valign="middle" class="Column01">
                <div class="hText06">
                    <icrop:CustomLabel ID="Label_Delivery" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column02" style="font-size:<%=TargetFontSizeDelivery%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Delivery_Target" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column03" style="font-size:<%=ResultFontSizeDelivery%>px">
                <div class="hText07">
                    <icrop:CustomLabel ID="Label_Delivery_Result" runat="server" TextWordNo="0" UseEllipsis="False"></icrop:CustomLabel>
                </div>
            </td>
			<td valign="middle" class="Column04">&nbsp;</td>
			<td valign="middle" class="Column05">
                <asp:PlaceHolder runat="server" ID="Div_Delivery_Graph">
                    <span class="Bar01" style="width:<%=GraphWidthDelivery%>px">&nbsp;</span>
                </asp:PlaceHolder>
            </td>
		</tr>    
	</table>
    <div class="GrafLineDot" style="left:<%=GetProgressLinePosition()%>px"></div>
</div>
</div>
</asp:Content>