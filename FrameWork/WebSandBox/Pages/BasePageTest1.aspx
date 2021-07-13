<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="BasePageTest1.aspx.vb" Inherits="Test_BasePageTest1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <div style="float:left;">
            <asp:Label ID="Label1" runat="server"></asp:Label><br />
            <asp:Button ID="Button8" runat="server" Text="Prev Test2" /><br />
            <asp:Button ID="Button5" runat="server" Text="Next Test2" /><br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" style="height:1000px;width:500px;"></asp:TextBox>
        </div>
        <div style="float:left;">
            <asp:Button ID="Button1" runat="server" Text="Test1 Current Set" /><br />
            <asp:Button ID="Button2" runat="server" Text="Test1 Current Get" /><br />
            <asp:Button ID="Button10" runat="server" Text="Test1 Current Remove" /><br />
            <asp:Button ID="Button3" runat="server" Text="Test1 Next Set" /><br />
            <asp:Button ID="Button4" runat="server" Text="Test1 Next Get" /><br />
            <asp:Button ID="Button11" runat="server" Text="Test1 Next Remove" /><br />
            <asp:Button ID="Button13" runat="server" Text="Test2 Prev Get" /><br />
            <asp:Button ID="Button6" runat="server" Text="Test1 Last Set" /><br />
            <asp:Button ID="Button7" runat="server" Text="Test1 Last Get" /><br />
            <asp:Button ID="Button9" runat="server" Text="Test1 Current Last Get" /><br />
            <asp:Button ID="Button12" runat="server" Text="Test1 Last Remove" /><br />
            <asp:Button ID="Button14" runat="server" Text="Get PrevScreenId" /><br />
            <asp:Button ID="Button15" runat="server" Text="RedirectPrevScreen" /><br />
            <asp:Button ID="Button16" runat="server" Text="OpenDialog fadein" /><br />
            <asp:Button ID="Button17" runat="server" Text="OpenDialog top" /><br />
            <asp:Button ID="Button18" runat="server" Text="OpenDialog left" /><br />
            <asp:Button ID="Button19" runat="server" Text="OpenDialog right" /><br />
            <asp:Button ID="Button20" runat="server" Text="OpenDialog bottom" /><br />
            <asp:Button ID="Button21" runat="server" Text="ShowMessageBox" /><br />
        <div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>