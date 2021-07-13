<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPageCustomer.Master" AutoEventWireup="false" CodeFile="SC3080201.aspx.vb" Inherits="Pages_SC3080201_Control" %>
<%@ Register src="SC3080201.ascx" tagname="SC3080201" tagprefix="uc1" %>
<%@ Register src="SC3080202.ascx" tagname="SC3080202" tagprefix="uc1" %>
<%@ Register src="SC3080203.ascx" tagname="SC3080203" tagprefix="uc1"%>
<%@ Register src="SC3080204.ascx" tagname="SC3080204" tagprefix="uc1" %>
<%-- $01 Add Start --%>
<%-- '2016/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 試乗・査定画面削除 --%>
<%-- ヘルプ依頼 --%>
<%@ Register src="SC3080401.ascx" tagname="SC3080401" tagprefix="uc1" %>
<%-- $01 Add End --%>

<%-- $02 Add Start --%>
<%@ Register src="SC3080216.ascx" tagname="SC3080216" tagprefix="uc1"%>
<%-- $02 Add End --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../Styles/SC3080201/Common.css?201902150000" type="text/css" media="screen,print" />
    <script src="../Scripts/SC3080201/Common.js?201902150000" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    
    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
    <%'AJAX用 %>
    <%--<asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>--%>
    <asp:ScriptManager ID="MyScriptManager" runat="server" EnablePageMethods="True">
    </asp:ScriptManager>
    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
     
    <%'中央部分 (ページインデックスナビゲータ)%>
    <div id="scNscCircleArea">
	    <p class="customerDetail1Navi scNscCircleOn">&nbsp;</p>
	    <p class="customerDetail2Navi scNscCircleOff">&nbsp;</p>
	    <p class="customerDetail3Navi scNscCircleOff">&nbsp;</p>
  	    <%'2012/04/24 TCS 安田 【SALES_1A】初期入力時に、画面が少し動くバグ修正（ユーザー課題 No.71）%>
	    <a href="#" id="dummyInitButton"></a>
  	    <%'2012/04/24 TCS 安田 【SALES_1A】初期入力時に、画面が少し動くバグ修正（ユーザー課題 No.71）%>
	    <p class="clearboth"></p>
        <asp:HiddenField ID="PageNumberClassHidden" runat="server" />
    </div>

    <div id="scNscOnePageDisplayArea">
        <%'３ページ全てを囲う枠 %>
        <asp:Panel id="scNscAllBoxContentsArea" runat="server">
            <%'ページ１ %>
            <div id="custDtlPage1" class="scNscOneBoxContentsWrap ">
                <uc1:SC3080201 ID="Sc3080201Page" runat="server" />
                <input type="text" id="dummyPage1Text" class="dummyFocusControlText" tabindex="3999"/>
            </div>
            <%'ページ２ %>
            <div id="custDtlPage2" class="scNscOneBoxContentsWrap ">
                <uc1:SC3080202 ID="Sc3080202Page" runat="server" />
                <input type="text" id="dummyPage2Text" class="dummyFocusControlText" tabindex="4999"/>
            </div>

            <%-- $02  Start --%>
            <%'ページ３ %>
            <%--<div id="custDtlPage3" class="scNscOneBoxContentsWrap ">
                <uc1:SC3080203 ID="Sc3080203Page" runat="server" />
                <input type="text" id="dummyPage3Text" class="dummyFocusControlText" tabindex="5999"/>
            </div>--%>

            <div id="custDtlPage3" class="scNscOneBoxContentsWrap ">
                <asp:placeholder ID="pagePlaceholder" runat="server" >
                    <uc1:SC3080203 ID="Sc3080203Page" runat="server" />
                    <uc1:SC3080216 ID="Sc3080216Page" runat="server" />
                </asp:placeholder>
                <input type="text" id="dummyPage3Text" class="dummyFocusControlText" tabindex="5999"/>
            </div>
            <%-- $02 End --%>
            <p class="clearboth"></p>
        </asp:Panel>
        <asp:HiddenField ID="CustDetailPageCountHidden" runat="server" />
    </div>

    <!-- 顧顧客メモ START -->
    <div id="CustomerMemoEdit" style="display: none">
        <div id="custDtlPage4" class="scNscOneBoxContentsWrap">
            <uc1:SC3080204 ID="Sc3080204Page" runat="server" />
        </div>
    </div>
    <!-- 顧客メモ END -->

    <%-- $01 Add Start --%>
    <%-- 試乗ポップアップ呼び出し --%>
    <%-- '2016/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 試乗ボタン削除 --%>

    <%-- エラーメッセージ --%>
    <asp:HiddenField ID="ErrWord4" runat="server" Value="" />
    <asp:HiddenField ID="ErrWord5" runat="server" Value="" />
    <%-- 2013/03/06 TCS 河原 GL0874 START --%>
    <asp:HiddenField ID="ErrWord6" runat="server" Value="" />

    <asp:HiddenField ID="SC3080201ContractCancelFlg" runat="server" Value="0" />
    <%-- 2013/03/06 TCS 河原 GL0874 END --%>

    <%-- ステータスポップアップ --%>
    <asp:HiddenField ID="UseAutoOpening" runat="server" Value="" />

    <%-- $01 End Start --%>
    
    <%-- 2013/03/06 TCS 河原 GL0874 START --%>
    <asp:HiddenField ID="ContractCancelStartFlg" runat="server" Value="0" />
    <asp:HiddenField ID="ContractCancelFllwStrcd" runat="server" Value="0" />
    <asp:HiddenField ID="ContractCancelFllwSeqno" runat="server" Value="0" />
    <%-- 2013/03/06 TCS 河原 GL0874 END --%>

    <!-- 2012/05/17 TCS 安田 クルクル対応 START -->
    <asp:Button ID="refreshButton" runat="server" Text="再描画する" CssClass="disableButton" />
    <asp:HiddenField ID="refreshProgramHidden" runat="server" />
    <asp:Button ID="insertRefreshButton" runat="server" Text="再描画する" CssClass="disableButton" />
    <!-- 2012/05/17 TCS 安田 クルクル対応 END -->

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server" >
    <%'2013/12/09 TCS 市川 Aカード情報相互連携開発 START %>
    <%-- $01 Add Start --%>
    <%-- '2016/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 査定ボタン削除 --%>
    <!-- ヘルプ依頼 START -->
    <uc1:SC3080401 ID="SC3080401" runat="server" TriggerClientID="MstPG_FootItem_Sub_203" />
    <!-- ヘルプ依頼 END -->     
    <%-- $01 End Start --%>
    
    <div class="RegisterButtonWrap">
        <asp:LinkButton ID="RegistButton" runat="server">
            <icrop:CustomLabel ID="RegistButtonLabel" runat="server" TextWordNo="30357" ></icrop:CustomLabel>
        </asp:LinkButton>
    </div>
<%'2013/12/09 TCS 市川 Aカード情報相互連携開発 END %>

    <%'登録時のオーバーレイ %>
    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>

</asp:Content>




