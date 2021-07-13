<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SC3A01102.aspx.cs" Inherits="AppDownLoad_SC3A01102" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1"><meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= pageTitle %></title>
    <link rel="stylesheet" type="text/css" href="./Content/Site.css" />
</head>

<body class="no-touch">
    <div id="container" class="container">
        
        <div class="header" style="height:30.72px;background-image:url(./images/head_grad.png);">
            <table border="0" style="width:100%;">
                <tr>
                    <td style="width:20%;padding-left:5px;text-align:left;" ></td>
                    <td style="width:60%;text-align:center;color:white"><h4><asp:label ID="Label_Word001" runat="server" text="Label"></asp:label></h4></td>
                    <td style="width:20%;padding-right:20px;text-align:right;">
                        <img alt="" src="./Content/headerImage10.png"  id="_optionBtn" style="height:30.72px;" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="main" style="position:absolute;top:30.72px;bottom:0;" >
            <div style="position:absolute;top:3.84px;bottom:7.68px;right:7.68px;left:7.68px;overflow:auto;background-image:url(./images/backimg.jpg);" class="radius-div">
                <div style="overflow:auto;position:absolute;top:0px;bottom:7.68px;left:7.68px;right:7.68px;">
                    <div style="margin-top:150px; text-align:center; position:absolute; display:table-cell;width:100%;vertical-align:middle;">
                        <div>
                            <h2><asp:label ID="Label_Word002" runat="server" text="Label"></asp:label></h2>
                            <br />
                            <form action="/" method="post">
                                <table style="width:300px;table-layout:fixed;margin:auto;">
                                    <tr>
                                        <!--<th style="text-align:left; border:4px solid #FFFFFF;">USER ID:</th>-->
                                        <td style="text-align:left;">
                                            <b><asp:label ID="Label_Word003" runat="server" text="Label"></asp:label></b>
                                        </td>
                                        <td style="text-align:left;">
                                            <%= appName %>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </form>
                        </div>
                        <div>
                            <p><asp:label ID="Label_Word005" runat="server" text="Label"></asp:label></p>
                            <br />
                            <form id="form1" runat="server">
                            <asp:Table ID="Table1" runat="server" style="width:300px;table-layout:fixed;margin:auto;">
                                <asp:TableRow ID="TableRow1" runat="server">
                                    <asp:TableCell ID="TableCell1" runat="server">
                                        <asp:TextBox ID="stfCd" runat="server" placeholder="<%# placeholder01 %>" style="width:100%;height:120%;background-color:#ffc0cb;"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow2" runat="server">
                                    <asp:TableCell ID="TableCell2" runat="server">
                                        <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="<%# placeholder02 %>" style="width:100%;height:120%;background-color:#ffc0cb;"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Button ID="ButtonDownLoad" runat="server" onclick="ButtonDownLoad_Click" Text="dl" />
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>

<!--
        <script type="text/javascript">
            $(function () {
                //idがstfCd,passwordの項目を必須入力項目として背景色を制御する
                icrop.setRequiredBackground("#stfCd", "#password");

            });
        </script>
-->
    </div>
<!--
    <div style="display:none;">
        <form action="/" data-ajax="true" data-ajax-method="POST" id="defaultSubmitForm" method="post">
            <input id="formCommand" name="formCommand" type="hidden" value="" />
            <input id="formParameter" name="formParameter" type="hidden" value="" />
            <input id="dialogCanBack" name="dialogCanBack" type="hidden" value="False" />
        </form>
    </div>
-->
</body>
</html>

