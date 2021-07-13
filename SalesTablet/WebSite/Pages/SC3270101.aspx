<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master"
    AutoEventWireup="false" CodeFile="SC3270101.aspx.vb" Inherits="Pages_SC3270101" %>

<%-- 試乗 --%>
<%@ Register src="SC3110101.ascx" tagname="SC3110101" tagprefix="uc1" %>
<%-- 査定依頼 --%>
<%@ Register src="SC3080301.ascx" tagname="SC3080301" tagprefix="uc1" %>
<%-- ヘルプ依頼 --%>
<%@ Register src="SC3080401.ascx" tagname="SC3080401" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    'SC3270101.aspx
    '─────────────────────────────────────
    '機能： 受注時説明フレーム
    '補足： 
    '作成： 2014/03/16 SKFC 下元武
    '更新： 2019/02/21 SKFC 中垣     TKM UAT-0492
    '更新： 2020/03/27 SKFC 板津     （FS）e-CRBシステム保守作業における資産管理運用の評価
    '                                 プレビューボタンの表示可否設定をDBから取得するよう変更
    '─────────────────────────────────────
    -->

    <link rel="Stylesheet" href="../Styles/SC3270101/SC3270101.css?20140316000000" />
    <script type="text/javascript" src="../Scripts/SC3270101/SC3270101.js?20200327000000"></script>
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

    <%'顧客ID %>
    <asp:HiddenField ID="UrlParamCstId" runat="server" />

    <%'V4受注時説明画面IFrame %>
    <asp:Panel ID="Pages_SC3B20201" runat="server" style="width: 1024px; height: 655px; position: absolute; top: 45px; left: 0px; overflow: hidden;"></asp:Panel>

    <%'変更時の確認メッセージ %>
    <asp:HiddenField ID="ModifiedMessageField" runat="server" />

    <%'画面タイトル %>
    <asp:HiddenField ID="HiddenTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">

    <div id="FooterOriginalButton">
        <asp:LinkButton ID="PreviewButtonLink" runat="server" Width="75" Height="46" CausesValidation="false" OnClientClick="return onPreviewButtonClick();" disabled="disabled" style="display:none;">
            <icrop:CustomLabel ID="PreviewButtonLabel" runat="server"></icrop:CustomLabel>
        </asp:LinkButton>
        <asp:LinkButton ID="SaveButtonLink" runat="server" Width="75" Height="46" CausesValidation="false" OnClientClick="return onSaveButtonClick();" disabled="disabled">
            <icrop:CustomLabel ID="SaveButtonLabel" runat="server"></icrop:CustomLabel>
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
