<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="UnallocatedActivityCountDriver.aspx.vb" Inherits="Pages_UnallocatedActivityCountDriver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 188px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="BaseBox"><!--　←サイズ確認用のタグです　-->
<div id="container"><!--　←全体を含むタグです。　-->
<!-- 中央部分-->
<div id="main">
<!-- ここからコンテンツ -->
<icrop:CustomLabel ID="CustomLabel1" runat="server" Text="担当未割当活動件数取得テスト用ドライバ" />


    <table style="width: 100%;">
        <tr>
            <td class="style1">
                <icrop:CustomLabel ID="UnallocatedActivityLabel" runat="server" Text="担当未割当活動件数:" />&nbsp;
            </td>
            <td>
                <icrop:CustomLabel ID="UnallocatedActivityCountLabel" runat="server" />&nbsp;
            </td>
        </tr>
    </table>

</div>
</div>
</div>

</asp:Content>

