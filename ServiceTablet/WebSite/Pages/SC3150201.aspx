<%@ Page Language="VB"MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3150201.aspx.vb" Inherits="Pages_SC3150201" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server" >
    <%--スクリプト(画面固有)--%>
    <link rel="Stylesheet" href="../styles/SC3150201/SC3150201.css?20140224135900" type="text/css" media="all" />
    <script type="text/javascript" src="../Scripts/SC3150201/SC3150201.js?20160330210000"></script>
</asp:Content>

<asp:Content id="Content2" ContentPlaceHolderID="content" runat="server" onload="LoadProcess();" >
    <div id="LoadingScreen">
        <div id="LoadingWrap">
            <div class="loadingIcn">
                <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
            </div>
        </div>
    </div>

    <div id ="screensaver" class="screensaver">
   
	    <div id="bgImages" class="normal">
		    <div id="EntryNo" class="EntryNo"><strong></strong><p></p></div>
		    <div id="EstimatedTime" class="EstimatedTime"><strong></strong></div>
		    <div id="ConstructionPlan" class="ConstructionPlan"><strong></strong></div>
		    <div id="ConstructionResults" class="ConstructionResults"><strong></strong></div>
		    <div id="Completed" class="Completed"><strong></strong></div>
	     </div>

    </div>
    <asp:UpdatePanel ID="AjaxHistoryPanel" runat="server" RenderMode="Block" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" runat="server"/> 
            <asp:Button ID="HiddenButtonRefreshtSC3150201" runat="server" CssClass="HiddenButton" />
            <asp:Button ID="HiddenButtonRedirectSC3150101" runat="server" CssClass="HiddenButton" />
            <asp:Button ID="HiddenButtonRefresh" runat="server" CssClass="HiddenButton" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HiddenRefreshTime" runat="server" />
    <asp:HiddenField ID="MstPG_RefreshTimerTime" runat="server" />
    <asp:HiddenField ID="MstPG_RefreshTimerMessage1" runat="server" />
</asp:Content>
