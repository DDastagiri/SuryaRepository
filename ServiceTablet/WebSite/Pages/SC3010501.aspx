<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3010501.aspx.vb" Inherits="SC3010501" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3010501/SC3010501.css?201312310000008" />
    <script type="text/javascript" src="../Scripts/SC3010501/SC3010501.js?201312310000008"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="MainAreaPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="MainAreaReload" runat="server" style="display:none;" />
            <asp:HiddenField ID="HiddenFieldIFrameURL" runat="server" />
            <div class="MainBorder">
<%--                <div style="background-color: red;width: 978px;height: 604px;margin-top: 3px;margin-left: 3px;"></div>--%>
                <iframe id="iFramePage" frameborder="0" class="IFrameStyle" runat="server" src=""></iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
