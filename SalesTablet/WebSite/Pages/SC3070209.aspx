<%--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070209.aspx
'─────────────────────────────────────
'機能： 見積フレーム(納車時説明用)
'補足： 
'更新： 2014/07/15 TCS 高橋 受注後フォロー機能開発に向けたシステム設計
'更新： 
'─────────────────────────────────────
--%>

<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master"
    AutoEventWireup="false" CodeFile="SC3070209.aspx.vb" Inherits="Pages_SC3070209" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3070209/SC3070209.css?20140715000000" />
    <script type="text/javascript" src="../Scripts/SC3070209/SC3070209.js?20140717000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <%--非同期読み込みのためのScriptManagerタグ--%>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
    </asp:ScriptManager>

    <div id="tcvNcv50Main" style="margin-top: -20px;margin-left: 10px;width:1000px;top:10px;right:7px;" >
        <%--見積作成--%>
        <iframe id="EstimateInfo" name="EstimateInfo" src="" frameborder="0" marginheight="0" scrolling="yes" width="100%"></iframe>

    </div>

    <%--読み込み時くるくる--%>
    <div id="serverProcessOverlayBlack">
    </div>
    <div id="serverProcessIcon">
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="UpdateButton" runat="server" Style="display: none;"></asp:Button>
            <asp:HiddenField ID="approvalButtonFlgHiddenField" runat="server" Value="" />
            <asp:Button ID="PrintButtonDummy" runat="server" Style="display: none;"></asp:Button>
            <asp:HiddenField ID="mandatryCheckFlgHiddenField" runat="server" Value="False" />
            <asp:HiddenField ID="mandatryCheckMsgHiddenField" runat="server" Value="" />
            <asp:HiddenField ID="deliDateInitialValueHiddenField" runat="server" Value="" />
            <asp:HiddenField ID="periodInitialValueHiddenField" runat="server" Value="" />
            <asp:HiddenField ID="firstPayInitialValueHiddenField" runat="server" Value="" />
            <asp:HiddenField ID="savedEstimationFlgHiddenField" runat="server" Value="0" />
            <asp:HiddenField ID="payMethodHiddenField" runat="server" Value="" />
            <asp:HiddenField ID="payTotalHiddenField" runat="server" Value="" />
            <asp:Button ID="getSelectedPaymentKbnDummy" runat="server" Style="display: none;"></asp:Button>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--更新： 2014/05/27 TCS 安田 受注時説明機能開発（受注後工程スケジュール）START --%>
    <asp:Button ID="OrderAfterButton" runat="server" Style="display: none;"></asp:Button>
    <asp:HiddenField ID="OrderAfterFlgHiddenField" runat="server" Value="" />
    <%--更新： 2014/05/27 TCS 安田 受注時説明機能開発（受注後工程スケジュール）END --%>
    
    <%--全見積管理ID(カンマ区切り)--%>
    <asp:HiddenField ID="lngEstimateIdHiddenField" runat="server" value="" />

    <%--表示中の見積もりのINDEX--%>
    <asp:HiddenField ID="selectedEstimateIndexHiddenField" runat="server" value="" />

    <%--アクションモード--%>
    <asp:HiddenField ID="actionModeHiddenField" runat="server" value="" />

    <%--TCV遷移用パラメータ--%>
    <asp:HiddenField ID="operationCodeHiddenField" runat="server" value="" />
    <asp:HiddenField ID="businessFlgHiddenField" runat="server" value="" />
    <asp:HiddenField ID="contractFlgHiddenField" runat="server" value="" />
    <asp:HiddenField ID="readOnlyFlgHiddenField" runat="server" value="" />
    <asp:HiddenField ID="strDlrcdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="estimateIdHiddenField" runat="server" value="" />

    <asp:HiddenField ID="ReferenceModeHiddenField" runat="server" value="FALSE" />

    <%--顧客ID--%>
    <asp:HiddenField ID="strCRCustIdHiddenField" runat="server" value="" />

    <asp:HiddenField ID="strApprovalModeHiddenField" runat="server" value="" />

    <%--顧客詳細遷移用パラメータ--%>
    <asp:HiddenField ID="strStrCdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="lngFollowupBoxSeqNoHiddenField" runat="server" value="" />
    <asp:HiddenField ID="strCstKindHiddenField" runat="server" value="" />
    <asp:HiddenField ID="strCustomerClassHiddenField" runat="server" value="" />

    <%--価格相談欄表示有無--%>
    <asp:HiddenField ID="approvalFieldFlgHiddenField" runat="server" value="FALSE" />

    <%--在庫状況用パラメータ--%>
    <asp:HiddenField ID="modelCdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="modelNumberHiddenField" runat="server" value="" />
    <asp:HiddenField ID="suffixCdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="extColorCdHiddenField" runat="server" value="" />

    <%--検索ボックス用顧客名--%> 
    <asp:HiddenField ID="cstNameHiddenField" runat="server" value="" />

    <asp:HiddenField ID="approvalSeriescdHiddenField" runat="server" value="" />
    <asp:HiddenField ID="approvalModelcdHiddenField" runat="server" value="" />

    <%--ロックモード--%>
    <asp:HiddenField ID="operationLockedHiddenField" runat="server" Value="" />

    <%--通知依頼ID--%>
    <asp:HiddenField ID="noticeReqIdHiddenField" runat="server" Value="" />

    <asp:HiddenField ID="seriesNameHiddenField" runat="server" value="" />
    <asp:HiddenField ID="modelNameHiddenField" runat="server" value="" />
    <asp:HiddenField ID="seriesCdHiddenField" runat="server" value="" />

    <%--価格相談ボタン初期表示フラグ--%>
    <asp:HiddenField ID="DiscountApprovalButtonFlg" runat="server" value="" />

    <%--契約承認情報--%>
    <asp:HiddenField ID="contractApprovalSatus" runat="server" value="" />
    <asp:HiddenField ID="contractApprovalStaff" runat="server" value="" />
    <asp:HiddenField ID="contractApprovalRequestStaff" runat="server" value="" />
    <%--2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START--%>
    <asp:HiddenField ID="contractNoHidden" runat="server" value="" />
    <%--2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END--%>

    <%--見積作成画面URL--%>
    <asp:HiddenField ID="EstimateInfoURL" runat="server" value="0" />

    <%--支払方法区分--%>
    <asp:HiddenField ID="selectedPaymentKbn" runat="server" value="0" />

    <%--顧客担当セールススタッフコード--%>
    <asp:HiddenField ID="staffCd" runat="server" value="0" />

</asp:Content>
