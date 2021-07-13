<%@ Page Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3120201.aspx.vb" Inherits="Pages_SC3120201" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
    <!--
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    'SC3120201.aspx
    '─────────────────────────────────────
    '機能： SPMフレーム
    '補足： 
    '作成： 2014/01/24 TMEJ m.asano
    '更新： 2014/07/02 TMEJ m.asano タブレットSPMによるSC管理機能開発に向けたシステム設計
    '─────────────────────────────────────
    -->
    <script type="text/javascript" src="../Scripts/SC3120201/SC3120201.js?20140124000000"></script>
</asp:Content> 

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    
    <%-- タブレットSPMURL --%>
    <asp:HiddenField id="SpmUrl" runat="server"></asp:HiddenField>
    
    <%-- ログイン用アカウント --%>
    <asp:HiddenField id="UrlParam" runat="server"></asp:HiddenField>

    <%-- 異常分類コード --%>
    <asp:HiddenField id="IrregClassCode" runat="server"></asp:HiddenField>

    <%-- 異常項目コード --%>
    <asp:HiddenField id="IrregItemCode" runat="server"></asp:HiddenField>

    <%-- タブレットSPM画面表示用IFrameエリア --%>
    <asp:Panel ID="Pages_SC3120201" runat="server" style="width: 1024px; height: 655px; position: absolute; top: 45px; left: 0px; overflow: hidden;"></asp:Panel>

</asp:Content> 

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

</asp:Content>