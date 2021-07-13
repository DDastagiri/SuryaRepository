<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master"
    AutoEventWireup="false" CodeFile="SC3280101.aspx.vb" Inherits="Pages_SC3280101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    'SC3280101.aspx
    '─────────────────────────────────────
    '機能： 納車時説明フレーム
    '補足： 
    '作成： 2014/04/17 NCN 跡部
    '更新： 
    '─────────────────────────────────────
    -->
    
    <script type="text/javascript" src="../Scripts/SC3280101/SC3280101.js?20140819000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <%'AJAX用 %>
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>

    <%'納車時説明URL %>
    <asp:HiddenField ID="DeliveryDescriptionUrl" runat="server" />

    <%'ログイン用アカウント %>
    <asp:HiddenField ID="UrlParamAccount" runat="server" />

    <%'ログイン時間 %>
    <asp:HiddenField ID="UrlParamUpdateDate" runat="server" />

    <%'商談ID %>
    <asp:HiddenField ID="UrlParamSalesId" runat="server" />

    <%'顧客ID %>
    <asp:HiddenField ID="UrlParamCstId" runat="server" />

    <%'顧客種別 %>
    <asp:HiddenField ID="UrlParamCstType" runat="server" />

    <%'顧客車両区分 %>
    <asp:HiddenField ID="UrlParamCstVclType" runat="server" />

    <%'V4納車時説明画面IFrame %>
    <asp:Panel ID="Pages_SC3B203" runat="server" style="width: 1024px; height: 655px; position: absolute; top: 45px; left: 0px; overflow: hidden;"></asp:Panel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">    
    <%'登録時のオーバーレイ %>
    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>
</asp:Content>
