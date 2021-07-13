<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="ICustomerFormTest.aspx.vb" Inherits="Test_ICustomerFormTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:Button ID="Button1" runat="server" Text="Value" OnClientClick="alert(icropScript.OperationLocked);return false;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

