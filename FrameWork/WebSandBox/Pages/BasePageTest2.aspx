<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoheaderMasterPage.Master" AutoEventWireup="false" CodeFile="BasePageTest2.aspx.vb" Inherits="Test_BasePageTest2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <div style="float:left;">
            <asp:Label ID="Label1" runat="server"></asp:Label><br />
            <asp:Button ID="Button4" runat="server" Text="Prev Test1" /><br />
            <asp:Button ID="Button7" runat="server" Text="Next Test1" /><br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" style="height:1000px;width:500px;"></asp:TextBox>
        </div>
        <div style="float:left;">
            <asp:Button ID="Button1" runat="server" Text="Test1 Next Get" /><br />
            <asp:Button ID="Button2" runat="server" Text="Test2 Prev Set" /><br />
            <asp:Button ID="Button3" runat="server" Text="Test2 Prev Get" /><br />
            <asp:Button ID="Button5" runat="server" Text="Test2 Prev Remove" /><br />
            <asp:Button ID="Button8" runat="server" Text="Get PrevScreenId" /><br />
            <asp:Button ID="Button9" runat="server" Text="RedirectPrevScreen" /><br />
        <div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>
