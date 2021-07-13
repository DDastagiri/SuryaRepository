<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204.aspx
'─────────────────────────────────────
'機能： 顧客メモ
'補足： 
'作成： 2011/11/18 TCS 安田
'更新： 2012/01/26 TCS 安田 【SALES_1B】テキストエリア自動サイズ調整スクリプト
'更新： 2012/01/26 TCS 安田 【SALES_1B】スクロール制御
'更新： 2012/06/04 TCS 安田 FS開発
'更新： 2013/06/30 TCS 未  【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2014/11/21 TCS 河原 TMT B案
'─────────────────────────────────────
-->

<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080204.ascx.vb" Inherits="Pages_SC3080204" EnableViewState="false"%>

<%'スタイル %>    
<link href="../Styles/SC3080204/SC3080204.css?20120604000000" rel="Stylesheet" />
    
<%'スクリプト %>    
<script type="text/javascript" src="../Scripts/SC3080204/SC3080204.jquery.touchSwipe.js?20121208000000"></script>

<!--2012/01/26 TCS 安田 【SALES_1B】テキストエリア自動サイズ調整スクリプト START -->
<script type="text/javascript" src="../Scripts/SC3080204/jquery.autoresize.js"></script>
<!--2012/01/26 TCS 安田 【SALES_1B】テキストエリア自動サイズ調整スクリプト END -->

<script type="text/javascript" src="../Scripts/SC3080204/SC3080204.js?20141125000000"></script>

<icrop:CustomLabel ID="noMemoText" runat="server" TextWordNo="70901" CssClass="disableLabel" />

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Button ID="CustomerMemoEditOpenButton" runat="server" style="display:none" />

</ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="customerMemoPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Button ID="saveMemoButton" runat="server" CssClass="disableButton" />

<asp:Panel ID="CustomerMemoVisiblePanel" runat="server">

<div id="scNscCustomerMemo">
	<div id="scNscCustomerMemoListArea">
		<div class="scNscCustomerMemoListHadder">
				<h3><icrop:CustomLabel ID="countLabel" runat="server" TextWordNo="0"/></h3>
				<a class="scNscCustomerMemoListHadderCustomerButoon" onclick="CustomerMemoCloseButton();"><icrop:CustomLabel ID="CustomLabel2Memo" runat="server" TextWordNo="70001" UseEllipsis="True" Width="50" onclick="CustomerMemoCloseButton();"/></a>
                <a href="#" class="scNscCustomerMemoListHadderCustomerButoonArrow"></a>
		</div>
        <div id="messageInner00">
        <div id="messageInner01">
            <div id="messageInner02">
                    <ul class="scNscCustomerMemoListBox">
                        <asp:Repeater ID="memoRepeater" runat="server" ClientIDMode="Predictable">
                            <ItemTemplate>
						        <li id="memolist<%# DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO")%>" class="scNscCustomerMemoListBoxDisable" value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>">
							        <p class="scNscCustomerMemoListTxt"><icrop:CustomLabel ID="customLabelMemoTxt" CssClass="memolistTxt" runat="server" TextWordNo="0" Text='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "FIRSTMEMO"))%>' Width="200" UseEllipsis="True" /></p>
							        <p class="scNscCustomerMemoListTime"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATESTR"))%>&nbsp;&nbsp;&nbsp;</p>
							        <p class="scNecDeleteButton"></p>
                                    <p class="clearboth"></p>
							        <p class="memoDetailHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MEMO"))%></p>
							        <p class="updateDayHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATEDAY"))%></p>
							        <p class="updateTimeHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATETIME"))%></p>
                                    <asp:HiddenField ID="seqnoMemoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>'></asp:HiddenField>
                                    <!--2013/06/30 TCS 未 2013/10対応版　既存流用 START-->
                                    <input type="hidden" class="cstMemoLockVersionHidden" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ROW_LOCK_VERSION"))%>'>
                                    <!--2013/06/30 TCS 未 2013/10対応版　既存流用 END-->
                                    <!--2014/11/21 TCS 河原 TMT B案 START-->
                                    <input type="hidden" class="DBDiv" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DBDiv"))%>'>
                                    <!--2014/11/21 TCS 河原 TMT B案 END-->
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
            </div>
		</div>
		</div>
	</div>
	<div id="scNscCustomerMemoDetailsArea">
		<div class="scNscCustomerMemoContentsHadder">
			<h3><icrop:CustomLabel ID="titleLabelMemo" runat="server" TextWordNo="0" Width="250" UseEllipsis="True"/></h3>
			<a class="scNscCustomerMemoContentsCancellationButoon"><icrop:CustomLabel ID="CustomLabel5Memo" runat="server" TextWordNo="70004"/></a>
			<a class="scNscCustomerMemoContentsSaveButoon"><icrop:CustomLabel ID="CustomLabel1Memo" runat="server" TextWordNo="70005"/></a>
            <a class="scNscCustomerMemoContentsPlusButoon"></a>
		</div>
		<div class="scNscCustomerMemoDetailsPaperArea">
			<p class="scNscCustomerMemoDetailsPaperAreaDay"><icrop:CustomLabel ID="dateLabel" runat="server" TextWordNo="0"/></p>
			<p class="scNscCustomerMemoDetailsPaperAreaTime"><icrop:CustomLabel ID="timeLabel" runat="server" TextWordNo="0"/></p>
			<p class="clearboth"></p>

<!--2012/01/26 TCS 安田 【SALES_1B】スクロール制御 START -->
            <div id="memoEreaInner01">
                <div id="memoEreaInner02">

<!--2012/06/04 TCS 安田 FS開発 START -->
                    <div class="scNscCustomerMemoDetailsPaperAreaView" id="memoView" style="width:565px; white-space:pre-line; overflow:visible">
                    </div>
<!--2012/06/04 TCS 安田 FS開発 END -->

                    <p class="scNscCustomerMemoDetailsPaperAreaTxt">                        
                        <asp:TextBox ID="memoTextBox" runat="server" Rows="20" Columns="70" 
                            TextMode="MultiLine" CssClass="ListIn" TabIndex="6000"></asp:TextBox>
                        <!--2014/11/21 TCS 河原 TMT B案 START-->
                        <input type="hidden" ID="DBDiv" value=''>
                        <!--2014/11/21 TCS 河原 TMT B案 END-->
                    </p>
                </div>
		    </div>
<!--2012/01/26 TCS 安田 【SALES_1B】スクロール制御 END -->
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

<asp:HiddenField ID="deleteHidden" runat="server" />

<!--2012/06/04 TCS 安田 FS開発 START -->
<asp:HiddenField ID="urlSchemeBrowzer" runat="server" />
<asp:HiddenField ID="urlSchemeBrowzers" runat="server" />
<!--2012/06/04 TCS 安田 FS開発 END -->

<!--2013/06/30 TCS 未 2013/10対応版 既存流用 START-->
<asp:HiddenField ID="activeCSTMemoLockVersionHidden" runat="server" />
<asp:HiddenField ID="cstLockVersionHidden" runat="server" />
<!--2013/06/30 TCS 未 2013/10対応版 既存流用 END-->
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
