<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3280101Stub.aspx.vb" Inherits="Pages_SC3280101Input" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="BaseBox"><!--　←サイズ確認用のタグです　-->
<div id="container"><!--　←全体を含むタグです。　-->
<!-- 中央部分-->
<div id="main">
<!-- ここからコンテンツ -->
<icrop:CustomLabel ID="CustomLabel1" runat="server" Text="納車時説明画面テスト用スタブ" />


    <table style="width: 100%;">
        <tr>
            <td>
                SalesId&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="salesIdTextBox" runat="server" Text="1" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                CstId&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="CstIdTextBox" runat="server" Text="1" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                CstType&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="CstTypeTextBox" runat="server" Text="1" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                CstVclType&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="CstVclTypeTextBox" runat="server" Text="1" />&nbsp;
            </td>
        </tr>        
        <tr>
            <td>
                StaffContext.PresenceCategory&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="PresenceCategory" runat="server" Text="2" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                StaffContext.PresenceDetail&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="PresenceDetail" runat="server" Text="0" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>

<asp:Button ID="goButton" runat="server" Text="納車時説明画面へ" />
</div>
</div>
</div>

</asp:Content>

