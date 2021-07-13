<%@ Page Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.master" AutoEventWireup="false" Inherits="Toyota.eCRB.SystemFrameworks.Web.SC3010304" %>

<%@ MasterType VirtualPath="~/Master/NoHeaderMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
    <div style="text-align:center; padding:1em; color:Red;">
 		<div>
            <icrop:CustomLabel ID="ErrorPageLiteral" runat="server" TextWordNo="2" />
            <asp:Panel id="errorMessagePanel" runat="server" Visible="false">
                <div style="border: 1px solid red; margin:1em; padding:1em;">
                    <asp:Label ID="errorMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>
        </div>
		<div style="margin:1em;">
            <icrop:CustomButton ID="BackButton" runat="server" OnClick="BackButton_Click" TextWordNo="3" Width="200" Height="44" />
        </div>
    </div>
</asp:Content>
