<%--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070201.ascx
'─────────────────────────────────────
'機能： 見積入力
'補足： 
'更新： 2013/11/27 TCS 河原 Aカード情報相互連携開発
'更新： 2014/05/27 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'更新： 2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価
'─────────────────────────────────────
--%>

<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master"
    AutoEventWireup="false" CodeFile="SC3070201.aspx.vb" Inherits="Pages_SC3070201" %>

<%-- 2019/04/17 TS 村井 (FS)次世代タブレット新興国向けの性能評価 DEL --%>

<%--価格相談--%>
<%@ Register Src="~/Pages/SC3070203.ascx" TagName="SC3070203" TagPrefix="uc2" %>
<%--見積書・契約書印刷--%>
<%@ Register Src="~/Pages/SC3070204.ascx" TagName="SC3070204" TagPrefix="uc3" %>
<%--価格相談回答--%>
<%@ Register Src="~/Pages/SC3070206.ascx" TagName="SC3070206" TagPrefix="uc4" %>
<%--注文承認--%>
<%@ Register Src="~/Pages/SC3070207.ascx" TagName="SC3070207" TagPrefix="uc5" %>
<%--注文承認依頼--%>
<%@ Register Src="~/Pages/SC3070208.ascx" TagName="SC3070208" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3070201/SC3070201.css?20140626000000" />
    <script type="text/javascript" src="../Scripts/SC3070201/SC3070201.js?20191004000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <%--非同期読み込みのためのScriptManagerタグ--%>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
    </asp:ScriptManager>

    <div id="tcvNcv50Main" style="margin-top: -20px;margin-left: 10px;width:1000px;top:10px;right:7px;" >
        <%--見積作成--%>
        <iframe id="EstimateInfo" name="EstimateInfo" src="" frameborder="0" marginheight="0" scrolling="yes" width="100%" height="655px"></iframe>

        <%-- 2019/04/17 TS 村井 (FS)次世代タブレット新興国向けの性能評価 DEL --%>

        <%--相談履歴--%>
        <asp:Panel id="RequestHistoryArea" runat="server" Visible="true">
            <div style="margin:20px 0px 20px 0px">
                <asp:PlaceHolder runat="server" ID="RequestHistory"/>
            </div>
        </asp:Panel>

        <%--価格相談回答--%>
        <asp:Panel id="PriceApprovalArea" runat="server" Visible="false">
            <div style="margin:20px 0px 20px 0px">
                <asp:PlaceHolder runat="server" ID="PriceApproval"/>
            </div>
        </asp:Panel>

        <%--注文承認--%>
        <asp:Panel id="OrderConfirmArea" runat="server" Visible="false">
            <div style="margin:20px 0px 20px 0px">
                <asp:PlaceHolder runat="server" ID="OrderConfirm"/>
            </div>
        </asp:Panel>

    </div>

    <%--読み込み時くるくる--%>
    <div id="serverProcessOverlayBlack">
    </div>
    <div id="serverProcessIcon">
    </div>

    <%--価格相談--%>
    <uc2:SC3070203 ID="AppButton" runat="server" TriggerClientId="ApprovalButton" />
    <%--見積書・契約書印刷--%>
    <uc3:SC3070204 ID="SC1" runat="server" TriggerClientID="PrintButton" />
    <%--注文承認依頼--%>
    <uc6:SC3070208 ID="SC2" runat="server" TriggerClientID="ContractButton" />

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
    
    <%--2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START--%>
    <asp:HiddenField ID="DirectBillingFlag" runat="server" value="" />
    <%--2018/06/15 TCS 舩橋 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END--%>

    <%--見積作成画面URL--%>
    <asp:HiddenField ID="EstimateInfoURL" runat="server" value="0" />

    <%--支払方法区分--%>
    <asp:HiddenField ID="selectedPaymentKbn" runat="server" value="0" />

    <%--顧客担当セールススタッフコード--%>
    <asp:HiddenField ID="staffCd" runat="server" value="0" />

</asp:Content>

<%--フッター領域--%>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <div id="FooterOriginalButton">
        <%--価格相談ボタン--%>
        <div id="ApprovalButton" runat="server" style="display: block;">
            <asp:LinkButton ID="DiscountApprovalButton" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return goUpdateData();">
                <icrop:CustomLabel ID="DiscountApprovalButtonLabel" runat="server" TextWordNo="76">
                </icrop:CustomLabel>
            </asp:LinkButton>
        </div>
        <%--契約承認ボタン--%>
        <div id="ContractButton" runat="server" style="display: block;">
            <asp:LinkButton ID="ContractApprovalButton" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return ContractButtonClick();">
                <icrop:CustomLabel ID="ContractApprovalButtonLabel" runat="server" TextWordNo="93">
                </icrop:CustomLabel>
            </asp:LinkButton>
        </div>
        <%--編集ボタン--%>
        <div id="EditButton" runat="server" style="display: none;">
            <asp:LinkButton ID="EstimateEditButton" runat="server" Width="75" Height="46" CausesValidation="False">
                <icrop:CustomLabel ID="EditButtonLabel" runat="server" TextWordNo="94">
                </icrop:CustomLabel>
            </asp:LinkButton>
        </div>
        <%--印刷ボタン--%>
        <div id="PrintButton" runat="server" style="display: block;">
            <asp:LinkButton ID="printLinkButton" runat="server" Width="75" Height="46" OnClientClick="return printLinkClick();">
                <icrop:CustomLabel ID="printLinkButtonLabel" runat="server" TextWordNo="90">
                </icrop:CustomLabel>
            </asp:LinkButton>
        </div>
    </div>
</asp:Content>
