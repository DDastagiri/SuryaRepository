<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SystemEnvSettingTest.aspx.vb" Inherits="Test_SystemEnvSettingTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        1:<br />
        <asp:GridView ID="GridView1" runat="server" ViewStateMode="Disabled"></asp:GridView><br />
        <asp:Button ID="Button1" runat="server" text="Start" /><br />
        2:<br />
        <asp:GridView ID="GridView2" runat="server" ViewStateMode="Disabled"></asp:GridView><br />
        <asp:Button ID="Button2" runat="server" text="Start" /><br />
    </div>
    </form>
</body>
</html>
