<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080204.ascx.vb" Inherits="Pages_SC3080204uc" ViewStateMode="Disabled" %>

<%'スタイル %>    
<link href="../Styles/SC3080204/SC3080204.css?20121210150000" rel="Stylesheet" />
    
<%'スクリプト %>    
<script type="text/javascript" src="../Scripts/SC3080204/jquery.touchSwipe-1.2.5.js"></script>
    
<script type="text/javascript" src="../Scripts/SC3080204/SC3080204.js?20160617000000"></script>

<icrop:CustomLabel ID="noMemoText" runat="server" TextWordNo="70901" CssClass="disableLabel" />
<icrop:CustomLabel ID="deleteLabel" runat="server" TextWordNo="70003" CssClass="disableLabel" />

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button ID="CustomerMemoEditOpenButton" runat="server" style="display:none" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="customerMemoPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Button ID="SaveMemoButton" runat="server" CssClass="disableButton" />

<asp:Panel ID="CustomerMemoVisiblePanel" runat="server">

<div id="scNscCustomerMemo">
	<div id="scNscCustomerMemoListArea">
		<div class="scNscCustomerMemoListHadder">
				<h3><icrop:CustomLabel ID="countLabel" runat="server" TextWordNo="0" /></h3>
				<a class="scNscCustomerMemoListHadderCustomerButoon" onclick="CustomerMemoCloseButton();">
                 <icrop:CustomLabel ID="CustomLabel2Memo" runat="server" TextWordNo="70001" Width="50" onclick="CustomerMemoCloseButton();"/>
                </a>
				<div class="scNscCustomerMemoListHadderCustomerButoonArrow">&nbsp;</div>
		</div>
        <div id="messageInner01">
            <div id="messageInner02">
                    <ul class="scNscCustomerMemoListBox">
                        <asp:Repeater ID="memoRepeater" runat="server" ClientIDMode="Predictable">
                            <ItemTemplate>
						        <li id="memolist<%# DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO")%>" class="scNscCustomerMemoListBoxDisable" value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>">
							        <p class="scNscCustomerMemoListTxt"><icrop:CustomLabel ID="customLabelMemoTxt" CssClass="memolistTxtEllipsis" runat="server" TextWordNo="0" Text='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "FIRSTMEMO"))%>' Width="200" /></p>
							        <p class="scNscCustomerMemoListTime"><%# DataBinder.Eval(Container.DataItem, "UPDATEDATESTR")%></p>
							        <p class="clearboth"></p>
							        <p class="memoDetailHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MEMO"))%></p>
							        <p class="updateDayHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATEDAY"))%></p>
							        <p class="updateTimeHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATETIME"))%></p>
                                    <asp:HiddenField ID="seqnoMemoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>'></asp:HiddenField>
                                    <!--TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START-->
                                    <input type="hidden" class="cstMemoLockVersionHidden" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ROW_LOCK_VERSION"))%>'>
                                    <!--TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END-->
                                    <!--2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START-->
                                    <input type="hidden" class="DBDiv" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DBDiv"))%>'>
                                    <!--2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END-->
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
            </div>
		</div>
	</div>
	<div id="scNscCustomerMemoDetailsArea">
		<div class="scNscCustomerMemoContentsHadder">
			<h3><icrop:CustomLabel ID="titleLabelMemo" runat="server" TextWordNo="0" Width="250" UseEllipsis="True"/></h3>
			<a class="scNscCustomerMemoContentsCancellationButoon"><icrop:CustomLabel ID="CustomLabel5Memo" runat="server" TextWordNo="70004" /></a>
			<a class="scNscCustomerMemoContentsSaveButoon"><icrop:CustomLabel ID="CustomLabel1Memo" runat="server" TextWordNo="70005" /></a>
            <a class="scNscCustomerMemoContentsPlusButoon"></a>
		</div>
		<div class="scNscCustomerMemoDetailsPaperArea">
			<p class="scNscCustomerMemoDetailsPaperAreaDay"><icrop:CustomLabel ID="dateLabel" runat="server" TextWordNo="0" /></p>
			<p class="scNscCustomerMemoDetailsPaperAreaTime"><icrop:CustomLabel ID="timeLabel" runat="server" TextWordNo="0" /></p>
			<p class="clearboth"></p>
            <p class="scNscCustomerMemoDetailsPaperAreaTxt">                            
                <asp:TextBox ID="memoTextBox" runat="server" Rows="20" Columns="70" 
                    TextMode="MultiLine" CssClass="ListIn" TabIndex="6000"></asp:TextBox>
                 <!--2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START-->
                 <input type="hidden" ID="DBDiv" value=''>
                 <!--2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END-->
                </p>
		</div>
	</div>
	<p class="clearboth"></p>
</div>


<p class="clearboth"></p>

<asp:HiddenField ID="activeSEQNOMemo" runat="server" />
<asp:HiddenField ID="modeMemo" runat="server" />

<asp:HiddenField ID="todayHidden" runat="server" />
<asp:HiddenField ID="nowTimeHidden" runat="server" />

<asp:HiddenField ID="listCountHidden" runat="server" />

<!--TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START-->
<asp:HiddenField ID="activeCSTMemoLockVersionHidden" runat="server" />
<asp:HiddenField ID="cstLockVersionHidden" runat="server" />
<!--TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END-->

</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
