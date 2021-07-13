<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3110101.aspx.vb" Inherits="Pages_SC3110101" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <%'スタイルシート %>
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Style.css"))%>" />
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/jquery.popover.css"))%>" />
   	<link rel="stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Controls.css"))%>" />
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/CommonMasterPage.css"))%>" />

    <%'スタイルシート(画面固有) %>
    <link rel="stylesheet" href="../Styles/SC3110101/SC3110101.css" type="text/css" />

    <%'スクリプト(Masterページと合わせる) %>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-1.5.2.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.ui.ipad.altfix.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.doubletap.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.flickable.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.json-2.3.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.popover.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.fingerscroll.js"))%>"></script>

    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/icropScript.js"))%>"></script>

    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckMark.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomLabel.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomTextBox.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.DateTimeSelector.js"))%>"></script>
  
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SegmentedButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SwitchButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomRepeater.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.NumericKeypad.js"))%>"></script>

    <%'スクリプト(画面固有) %>
    <script type="text/javascript" src="../Scripts/SC3110101/SC3110101_aspx.js"></script>

</head>

<body>
<div id="bodyFrame" style="height:420px;">
    <form id="this_form" runat="server">
        <%'処理中のローディング Start %>
        <div id="registOverlayBlackSC3110101"></div>
        <div id="processingServerSC3110101"></div>
        <%'処理中のローディング End %>
<asp:Panel ID="LoadSpinPanel" runat="server">
<asp:Button ID="LoadSpinButton" runat="server" style="display:none;" />
    <script type="text/javascript">
        pageInit();
    </script>
</asp:Panel>

<%  '0件表示パネル %>
<asp:Panel ID="NotTestDriveCarPanel" runat="server">
    <div class="innerDataBox">
	    <ul class="listCarHandling">
            <li>
                <div class="CarInfo">
                    <div class="CarImage">
                        <asp:Image ID="NotTestDriveCarPicture"  runat="server" ImageUrl="../Styles/Images/SC3110101/notCarImage.png" Width="130" Height="80" />
                    </div>
                    <icrop:CustomLabel ID="NotCarStatus" runat="server" CssClass="CarNoData"></icrop:CustomLabel>
                    
                </div>
            </li>
        </ul>
    </div>
</asp:Panel>

<%  '試乗車データ表示パネル %>
<asp:Panel ID="TestDriveCarPanel" runat="server">

        <% '登録ボタン用 %>
        <input type="button" id="RegisterButton_Pre" style="display:none;" onclick="redirectSC3110101();" />
        <asp:Button ID="RegisterButton" runat="server" style="display:none;" />
        <asp:Repeater ID="TestCarList" runat="server">
            <HeaderTemplate>
                 <div class="innerDataBox">
                    <ul class="listCarHandling">
            </HeaderTemplate>
            <ItemTemplate>

                <% '権限での使用中タップ切り替え %>
                <li id="changeID<%# Container.ItemIndex %>" class="statusCnt" onclick="isUseCar('#changeID','<%# Container.ItemIndex %>')">

                    <% '使用中チップの表示切替 %>
                    <div id="changeID" runat="server" class="inUse" style="visibility:hidden "><asp:Image ID="usePicture" runat="server" Width="141" Height="96" ImageUrl="../Styles/Images/SC3110101/inUseIcon.png" /></div>
                    <div class="CarInfo">
                        <div ID="CarImage" runat="server" class="CarImage"><asp:Image ID="carPicture" runat="server" Width="130" Height="80"/></div>
                        <div ID="CarName" runat="server" style="width:194px; height: 32px;" class="ellipsis CarName">
                            <%-- 2012/05/25 号口課題No.134 START --%>
                            <%-- <img src="../Styles/Images/SC3110101/newIcon.png" width="33" height="29" alt="new" />  --%>
                            <%-- 2012/05/25 号口課題No.134 END --%>
                            <icrop:CustomLabel ID="NotLogo" runat="server" style="position: absolute; top: 14px; left: 37px;"></icrop:CustomLabel>
                            <asp:Image ID="carNamePicture" runat="server" Width="161" Height="29"/>
                        </div>
                        <div ID="CarID" runat="server" style="width:190px;" class="ellipsis CarID"><%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "GRADENAME"))%></div>
                        <div ID="CarColor" runat="server" style="width:190px;" class="ellipsis CarColor"><%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CORTCOLOR"))%></div>
                    </div>

                    <% '使用する情報 %>
                    <asp:HiddenField ID="testDriveCarId" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TESTDRIVECARID"))%>' />
                    <asp:HiddenField ID="testDriveCarStatus" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TESTDRIVECARSTATUS"))%>' />
                    <asp:Label ID="testDriveCarName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TESTDRIVECARNAME"))%>' style="display:none;" />
                    <asp:HiddenField ID="modelLogo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "LOGO_NOTSELECTED"))%>' />
                    <asp:HiddenField ID="modelPicture" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "IMAGEFILE"))%>' />
                    <asp:HiddenField ID="updateDate" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "UPDATEDATE"))%>' />

                    <% '更新のために保持する情報 %>
                    <input type="hidden" id="BeforeStatus<%# Container.ItemIndex %>" name="BeforeStatus<%# Container.ItemIndex %>" value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TESTDRIVECARSTATUS"))%>' />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                    </ul>
                 
                </div>
            </FooterTemplate>             
        </asp:Repeater>
        
        <% '表示件数と権限を持っておく %>
        <asp:HiddenField ID="authority" runat="server" />
</asp:Panel>
    </form>
</div>
</body>
</html>
