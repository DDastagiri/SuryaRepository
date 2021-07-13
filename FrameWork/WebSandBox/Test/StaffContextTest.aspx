<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StaffContextTest.aspx.vb" Inherits="Test_StaffContextTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="border:1px black solid">
        ID:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        Pssword:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br />
        MAC:<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Login" />
    </div>
    <div style="border:1px black solid;margin-top:10px;">
        Account:<asp:Label ID="Label1" runat="server"></asp:Label><br />
        UserName:<asp:Label ID="Label2" runat="server"></asp:Label><br />
        DlrCd:<asp:Label ID="Label3" runat="server"></asp:Label><br />
        DlrName:<asp:Label ID="Label4" runat="server"></asp:Label><br />
        BrnCode:<asp:Label ID="Label5" runat="server"></asp:Label><br />
        BrnName:<asp:Label ID="Label6" runat="server"></asp:Label><br />
        OpeCd:<asp:Label ID="Label7" runat="server"></asp:Label><br />
        OpeName:<asp:Label ID="Label8" runat="server"></asp:Label><br />
        UserPermission:<asp:Label ID="Label9" runat="server"></asp:Label><br />
        TeamCd:<asp:Label ID="Label10" runat="server"></asp:Label><br />
        TeamName:<asp:Label ID="Label11" runat="server"></asp:Label><br />
        TeamLeader:<asp:Label ID="Label12" runat="server"></asp:Label><br />
        TimeDiff:<asp:Label ID="Label13" runat="server"></asp:Label><br />
        IsCreated:<asp:Label ID="Label14" runat="server"></asp:Label><br />
        <asp:Button ID="Button2" runat="server" Text="read" />
    </div>
    </form>
</body>
</html>
