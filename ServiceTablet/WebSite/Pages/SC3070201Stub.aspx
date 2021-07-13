<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3070201Stub.aspx.vb" Inherits="Pages_SC3070201input" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<div id="BaseBox"><!--　←サイズ確認用のタグです　-->
<div id="container"><!--　←全体を含むタグです。　-->
<!-- 中央部分-->
<div id="main">
<!-- ここからコンテンツ -->
<icrop:CustomLabel ID="CustomLabel1" runat="server" Text="見積作成画面テスト用スタブ" />


    <table style="width: 100%;">
        <tr>
            <td>
                <icrop:CustomLabel ID="estimateIdLabel" runat="server" Text="EstimateId" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="estimateIdTextBox" runat="server" Text="3" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <icrop:CustomLabel ID="lockStatusLabel" runat="server" Text="MenuLockFlag" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="lockStatusTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <icrop:CustomLabel ID="CustomLabel2" runat="server" Text="NewActFlag" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="NewActFlagTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
    </table>

<asp:Button ID="goButton" runat="server" Text="見積作成画面へ" />
</div>
</div>
</div>

</asp:Content>

