<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master"
    AutoEventWireup="false" CodeFile="SC3270100dummy.aspx.vb" Inherits="Pages_SC3270100dummy" %>

<%-- 試乗 --%>
<%@ Register src="SC3110101.ascx" tagname="SC3110101" tagprefix="uc1" %>
<%-- 査定依頼 --%>
<%@ Register src="SC3080301.ascx" tagname="SC3080301" tagprefix="uc1" %>
<%-- ヘルプ依頼 --%>
<%@ Register src="SC3080401.ascx" tagname="SC3080401" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    'SC3270100dummy.aspx
    '─────────────────────────────────────
    '機能： 受注時説明フレーム
    '補足： 
    '作成： 2014/03/16 SKFC 下元武
    '更新： 
    '─────────────────────────────────────
    -->

    <link rel="Stylesheet" href="../Styles/SC3270101/SC3270101.css?20140316000000" />
    <script type="text/javascript" src="../Scripts/SC3270100dummy/SC3270100dummy.js?20140316000000"></script>
	<style type="text/css">
		<!--
			.ButtonMove:active{
				background-color:Pink;
			}
		-->
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <%'AJAX用 %>
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>

    <%'受注時説明URL %>
    <asp:HiddenField ID="SalesbookingDescriptionUrl" runat="server" />

    <%'ログイン用アカウント %>
    <asp:HiddenField ID="UrlParamAccount" runat="server" />

    <%'ログイン時間 %>
    <asp:HiddenField ID="UrlParamUpdateDate" runat="server" />

    <%'商談ID %>
    <asp:HiddenField ID="UrlParamSalesId" runat="server" />

    <%'見積管理ID %>
    <asp:HiddenField ID="UrlParamEstimateId" runat="server" />

    <%'注文番号 %>
    <asp:HiddenField ID="UrlParamSalesbkgNum" runat="server" />

    <%'受注時説明表示モード %>
    <asp:HiddenField ID="UrlParamSalesbookingDescriptionViewMode" runat="server" />

    <%'契約条件変更フラグ %>
    <asp:HiddenField ID="UrlParamContractAskChgFlg" runat="server" />

<%--
    <%'V4受注時説明画面IFrame %>
    <asp:Panel ID="Pages_SC3B20201" runat="server" style="width: 1024px; height: 655px; position: absolute; top: 45px; left: 0px; overflow: hidden;"></asp:Panel>
--%>
    <table>
        <tr>
            <td><asp:Label ID="Label1" Text="商談ID(フォローアップボックス)" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtFollowUpBox" runat="server">850</asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" Text="見積ID" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtEstimateId" runat="server">478</asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label3" Text="受注時説明表示モード 1:お客様ご説明モード、2:スタッフ予定変更モード" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtViewMode" runat="server">1</asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label4" Text="契約条件変更フラグ 1:変更あり 空：変更なし" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtContractAskChangeFlag" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label6" Text="顧客ID" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtCstId" runat="server">1005414</asp:TextBox></td>
        </tr>
         <tr>
            <td><asp:Label ID="Label7" Text="顧客種別" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtCstType" runat="server">2</asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label8" Text="顧客車両区分" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtCstVclType" runat="server">1</asp:TextBox></td>
        </tr>
       <tr>
            <td><asp:Label ID="Label5" Text="注文番号(未使用)" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="TxtOrderID" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button ID="BtnMove" text="Submit" class="ButtonMove" runat="server" style="width:100px; height:50px;"/></td>
            <td></td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <div id="FooterOriginalButton">
        <asp:LinkButton ID="PreviewButtonLink" runat="server" Width="75" Height="46" CausesValidation="false" OnClientClick="return onPreviewButtonClick();">
            <icrop:CustomLabel ID="PreviewButtonLabel" runat="server" TextWordNo="8"></icrop:CustomLabel>
            <span>Preview</span>
        </asp:LinkButton>
        <asp:LinkButton ID="SaveButtonLink" runat="server" Width="75" Height="46" CausesValidation="false" OnClientClick="return onSaveButtonClick();">
            <icrop:CustomLabel ID="SaveButtonLabel" runat="server" TextWordNo="8"></icrop:CustomLabel>
            <span>Save</span>
        </asp:LinkButton>
        <asp:Label ID="FooterOriginalButtonRightSpaceLabel" runat="server" Width="10"></asp:Label>
    </div>

    <!-- ヘルプ依頼 START -->
    <uc1:SC3080401 ID="SC3080401" runat="server" TriggerClientID="MstPG_FootItem_Sub_203" />
    <!-- ヘルプ依頼 END -->
            
    <%-- 試乗入力画面のユーザコントロール --%>
    <uc1:SC3110101 ID="SC3110101" runat="server" TriggerClientID="MstPG_FootItem_Sub_201" />

    <!-- 査定依頼 START -->
    <div id="Div2"  style="z-index:10000;">
        <uc1:SC3080301 ID="Sc3080301Page" runat="server" />
    </div>
    <!-- 査定依頼 END -->


    <%'登録時のオーバーレイ %>
    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>
</asp:Content>
