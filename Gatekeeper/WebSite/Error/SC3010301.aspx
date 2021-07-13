<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" Inherits="Toyota.eCRB.SystemFrameworks.Web.SC3010301" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="server">
    <div style="text-align:center; padding:1em; color:Red;">
 		<div>
            <icrop:CustomLabel ID="ErrorPageLiteral" runat="server" TextWordNo="7" />
        </div>
        <div style="width:100%;text-align:left; margin-left:auto; margin-right:auto; margin-top:1em; color:Black;">
            <asp:Table ID="TblInfo" runat="server" CellSpacing="0" Width="100%">
                <asp:TableRow ID="TableRow0" runat="server" Height="20px">
                    <asp:TableCell ID="CellIdName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                    <icrop:CustomLabel ID="CustomLabel0" runat="server" TextWordNo="8"/>
                    </asp:TableCell>
                    <asp:TableCell ID="CellIdValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow1" runat="server" Height="20px">
                    <asp:TableCell ID="CellTimeName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                    <icrop:CustomLabel ID="TimeLiteral" runat="server" TextWordNo="4" />
                    </asp:TableCell>
                    <asp:TableCell ID="CellTimeValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow2" runat="server" Height="20px">
                    <asp:TableCell ID="CellServerName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                        <icrop:CustomLabel ID="ServerLiteral" runat="server" TextWordNo="2" />
                        </asp:TableCell>
                    <asp:TableCell ID="CellServerValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow3" runat="server" Height="20px">
                    <asp:TableCell ID="CellUserIdName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                        <icrop:CustomLabel ID="AccountLiteral" runat="server" TextWordNo="5" />
                        </asp:TableCell>
                    <asp:TableCell ID="CellUserIdValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow4" runat="server" Height="20px">
                    <asp:TableCell ID="CellScreenIdName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                        <icrop:CustomLabel ID="ScreenIdLiteral" runat="server" TextWordNo="1" />
                        </asp:TableCell>
                    <asp:TableCell ID="CellScreenIdValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" style="word-break:break-all;" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server" Height="20px">
                    <asp:TableCell ID="CellSessionIdName" runat="server"
                        BackColor="#B7D0E9" Font-Bold="True" ForeColor="Black" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px">
                        <icrop:CustomLabel ID="SessionNameLiteral" runat="server" TextWordNo="3" />
                        </asp:TableCell>
                    <asp:TableCell ID="CellSessionIdValue" runat="server" BackColor="#EEEEEE" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
		<div style="margin:1em;">
            <icrop:CustomButton ID="BackButton" runat="server" OnClick="BackButton_Click" TextWordNo="9" Width="200" Height="44" />
        </div>
    </div>
</asp:Content>

