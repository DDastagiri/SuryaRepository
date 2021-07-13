<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3070201Stub.aspx.vb" Inherits="Pages_SC3070201Input" %>

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
                <icrop:CustomTextBox ID="estimateIdTextBox" runat="server" Text="2000" width="600"/>&nbsp;
            </td>
        </tr>
<%' 2012/10/29 TCS 上田 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START %>
        <tr>
            <td>
                <icrop:CustomLabel ID="selectedEstimateIndexLabel" runat="server" Text="SelectedEstimateIndex" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="selectedEstimateIndexTextBox" runat="server" Text="0" />&nbsp;
            </td>
        </tr>
<%' 2012/10/29 TCS 上田 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END %>
        <tr>
            <td>
                <icrop:CustomLabel ID="lockStatusLabel" runat="server" Text="MenuLockFlag" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="lockStatusTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
<%--        <tr>
            <td>
                <icrop:CustomLabel ID="CustomLabel2" runat="server" Text="NewActFlag" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="NewActFlagTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
--%>        <tr>
            <td>
                <icrop:CustomLabel ID="CustomLabel3" runat="server" Text="OperationCode" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="OperationCodeTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <icrop:CustomLabel ID="CustomLabel4" runat="server" Text="BusinessFlg" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="BusinessFlgTextBox" runat="server" Text="False" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <icrop:CustomLabel ID="CustomLabel5" runat="server" Text="ReadOnlyFlg" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="ReadOnlyFlgTextBox" runat="server" Text="False" />&nbsp;
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
                <icrop:CustomLabel ID="CustomLabel7" runat="server" Text="NoticeReqId" />&nbsp;
            </td>
            <td>
                <icrop:CustomTextBox ID="NoticeReqIdTextBox" runat="server" Text="" />&nbsp;
            </td>
        </tr>
    </table>

<asp:Button ID="goButton" runat="server" Text="見積作成画面へ" />
</div>
</div>
</div>

</asp:Content>

