<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="UnallocatedCustomerCountDriver.aspx.vb" Inherits="Pages_UnallocatedCustomerCountDriver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 123px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="BaseBox"><!--　←サイズ確認用のタグです　-->
<div id="container"><!--　←全体を含むタグです。　-->
<!-- 中央部分-->
<div id="main">
<!-- ここからコンテンツ -->
<icrop:CustomLabel ID="CustomLabel1" runat="server" Text="担当未割当件数取得テスト用ドライバ" />


    <table style="width: 100%;">
        <tr>
            <td class="style1">
                <icrop:CustomLabel ID="UnallocatedCustomerLabel" runat="server" Text="担当未割当件数:" />&nbsp;
            </td>
            <td>
                <icrop:CustomLabel ID="UnallocatedCustomerCountLabel" runat="server" />&nbsp;
            </td>
        </tr>
    </table>

</div>
</div>
</div>

</asp:Content>

