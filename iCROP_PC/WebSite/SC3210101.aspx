<%@ Import Namespace="Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic" %>
<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="SC3010101.aspx.vb" Inherits="Pages_SC3010101" %>

<!DOCTYPE html>
<html lang="ja">
<head id="Head1" runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="format-detection" content="telephone=no" />

    <%'タイトル %>
    <title></title>

    <%'スタイルシート %>
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Style.css"))%>" />
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/jquery.popover.css"))%>" />
   	<link rel="stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Controls.css"))%>" />

    <%'スタイルシート(画面固有) %>
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/SC3210101/SC3210101.css"))%>" type="text/css" media="screen,print" />

    <%'スクリプト %>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-1.5.2.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.ui.ipad.altfix.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.doubletap.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.flickable.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.json-2.3.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.popover.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.fingerscroll.js"))%>"></script>

    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/icropScript.js"))%>"></script>
    
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.VScroll.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckMark.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomLabel.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomTextBox.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.DateTimeSelector.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.PopOverForm.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SegmentedButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SwitchButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomRepeater.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.NumericKeypad.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomCheckBox.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/icropBase.js"))%>"></script>

    <%'スクリプト(画面固有) %>
    <script type="text/javascript" src="Scripts/SC3210101/SC3210101.js?20130405000001"></script>

</head>
<body>
<div id="bodyFrame">
<form id="this_form" runat="server">

<div id="baseBox">
    <div id="mainLogin">
        <!-- ここからコンテンツ -->
        <div id="contents">
            
            <!-- 入力 -->
            <asp:Panel ID="login" runat="server">
                <div id="loginId">
                    <icrop:CustomTextBox ID="id" runat="server" PlaceHolderWordNo="1" type="text" onkeyup="checkInput();" MaxLength="26" />
                </div>
                <div id="loginPw">
                    <icrop:CustomTextBox ID="password" runat="server" PlaceHolderWordNo="2" type="password" onkeyup="checkInput();" MaxLength="10" />
                </div>
            </asp:Panel>

            <!-- Loading -->
            <asp:Panel ID="loading" runat="server" CssClass="loading" style="display:none;">
                <table border="0" width="100%">
                    <tr>
                        <td align="center">
                            <table border="0">
                                <colgroup>
                                    <col style="width: 25px;" />
                                    <col />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <td>
                                            <div class="loadingVertical">
                                                <div class="loadingIcn">
                                                    <img src="Styles/Images/SC3210101/animeicn.png" width="32" height="32" alt="" />
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="loadingVertical">
                                                <div class="loadingChar">
                                                    <icrop:CustomLabel runat="server" ID="lblLoginIn" TextWordNo="5" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <!-- ボタン -->
            <div id="loginBtn" class="mousUp">
                <div class="loginChar">
                    <icrop:CustomLabel runat="server" ID="lblLogin" TextWordNo="3" />
                </div>
            </div>
            <div id="loginDown" style="display: none;">
                <div class="loginChar">
                    <asp:Button runat="server" UseSubmitBehavior="true" CssClass="mousDown" ID="logOnBtn02" OnClientClick="login()" />
                </div>
            </div>
        </div>
        <!-- ここかまでコンテンツ -->
        <div id="divValue">
            <asp:HiddenField runat="server" ID="hdnMac" Value="PC-SI-TE-LO-GI-N0" />
            <asp:HiddenField runat="server" ID="hdnUploadFlg" />
        </div>
        <!-- 接続エラー -->
        <asp:Panel runat="server" ID="pnlError" Visible="false" Width="100%">
            <table width="100%">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="left" style="word-break: break-all;">
                                    <icrop:CustomLabel runat="server" ID="clError" />
                                </td>
                                <td align="left">
                                    <div class="loginChar">
                                        <asp:Button runat="server" ID="btnRefresh" CssClass="mousDown" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</div>

</form>
</div>
</body>
</html>
