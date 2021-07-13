<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3180205.aspx.vb" Inherits="SC3180205" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3180205/SC3180205.css?201401210000002" />
    <script type="text/javascript" src="../Scripts/SC3180205/SC3180205.js?201401210000002"></script>
    <script type="text/javascript">
        $("document").ready(function () {
    
            window.parent.document.getElementById('SELECTUSER').value = "0";

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>
    
    <div id="ServerProcessOverlayBlack"></div>
    <div id="ServerProcessIcon"></div>

<div class="popover active" id="ApprovalList">
    <div class="header ApprovalListHeader">
        <div id="CancelButtonDiv" class="CTConfirmPopTitleBlockButtonLeft"><icrop:CustomLabel runat="server" id="CancelButton" Width="60px" CssClass="Ellipsis" /></div>
        <div id="ApprovalListTitleDiv"><icrop:CustomLabel runat="server" id="ApprovalListTitle" Width="140px" CssClass="Ellipsis" /></div>
        <div id="RegisterButtonDiv" class="CTConfirmPopTitleBlockButtonRightOff"><icrop:CustomLabel runat="server" id="RegisterButton" Width="60px" CssClass="Ellipsis" /></div>
    </div>
    <div class="content">
        <div id="dashboardFrame_base">
            <asp:UpdatePanel ID="MainAreaPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="MainAreaReload" runat="server" style="display:none;" />
                    <asp:HiddenField ID="HiddenSelectAccount" runat="server" />
                        <div class="PoPuPBlockSSA2001ContentBodyWrap">
                            <ul class="CTConfirmPopList">
                                <asp:Repeater runat="server" id="AccountAreaRepeater" EnableViewState="false">
                                    <ItemTemplate>
                                        <li runat="server" id="AccountRecord" class="AccountRecord">
                                            <div class="nsc413OnOffIcn"><img runat="server" id="PresenceImage" src="" width="34" height="27" alt="no image" /></div>
                                            <div class="nsc413OnOffWord"><icrop:CustomLabel runat="server" ID="AccountName" width="180px" CssClass="Ellipsis" /></div>
                                            <div id="SelectCheck"></div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>


</asp:Content>
