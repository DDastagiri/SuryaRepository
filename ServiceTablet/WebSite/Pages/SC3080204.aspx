<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.master" AutoEventWireup="false" CodeFile="SC3080204.aspx.vb" Inherits="Pages_SC3080204" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<link href="../Styles/SC3080204/common.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/SC3080204/contents.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Styles/SC3080204/SC3080204.css?20111219170800" rel="Stylesheet" />

    <script type="text/javascript" src="../Scripts/SC3080204/jquery.touchSwipe-1.2.5.js"></script>
    
    <%--<script type="text/javascript" src="../Scripts/SC3080204/jquery.CustomLabel.SC3080204.js"></script>
    <script type="text/javascript" src="../Scripts/SC3080204/jquery.CustomTextBox.SC3080204.js"></script>--%>

    <script type="text/javascript" src="../Scripts/SC3080204/SC3080204.js?20160617000000"></script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

<icrop:CustomLabel ID="noMemoText" runat="server" TextWordNo="901" Text="メモ未入力エラー" CssClass="disableLabel" />

<%--<div id="BaseBox"><!--　←サイズ確認用のタグです　-->
<div id="container"><!--　←全体を含むタグです。　-->
	<!-- 中央部分-->
	<div id="main">--%>
	<!-- ここからコンテンツ -->
		<%--<div id="contents">--%>
			<div id="scNscCustomerMemo">
				<div id="scNscCustomerMemoListArea">
					<div class="scNscCustomerMemoListHadder">
							<h3><icrop:CustomLabel ID="countLabel" runat="server" TextWordNo="0" Text="25件"/></h3>
							<a class="scNscCustomerMemoListHadderCustomerButoon" onclick="CustomerMemoCloseButton();"><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="1" Text="顧客詳細" UseEllipsis="True" Width="50" onclick="CustomerMemoCloseButton();"/></a>
							<div class="scNscCustomerMemoListHadderCustomerButoonArrow">&nbsp;</div>
					</div>
                    <div id="messageInner01">
                        <div id="messageInner02">
                                <ul class="scNscCustomerMemoListBox">
                                    <asp:Repeater ID="memoRepeater" runat="server" ClientIDMode="Predictable">
                                        <ItemTemplate>
						                    <li id="memolist<%# DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO")%>" class="scNscCustomerMemoListBoxDisable" value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>">
							                    <p class="scNscCustomerMemoListTxt"><icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="0" Text='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "FIRSTMEMO"))%>' Width="200" UseEllipsis="True" /></p>
							                    <p class="scNscCustomerMemoListTime"><%# DataBinder.Eval(Container.DataItem, "UPDATEDATESTR")%></p>
							                    <p class="clearboth"></p>
							                    <p class="memoDetailHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MEMO"))%></p>
							                    <p class="updateDayHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATEDAY"))%></p>
							                    <p class="updateTimeHidden"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "UPDATEDATETIME"))%></p>
                                                <asp:HiddenField ID="seqnoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CUSTMEMOHIS_SEQNO"))%>'></asp:HiddenField>
                                                <!--2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START-->
                                                <input type="hidden" class="cstMemoLockVersionHidden" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ROW_LOCK_VERSION"))%>'>
                                                <!--2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END-->
                                            </li>
                                        </ItemTemplate>

                                    </asp:Repeater>
                                </ul>
                        </div>
				    </div>
				</div>
				<div id="scNscCustomerMemoDetailsArea">
					<div class="scNscCustomerMemoContentsHadder">
						<h3><icrop:CustomLabel ID="titleLabel" runat="server" TextWordNo="0" Text="お客様は１年程海外に行っ..." Width="250" UseEllipsis="True"/></h3>
						<a class="scNscCustomerMemoContentsCancellationButoon"><icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="4" Text="キャンセル"/></a>
						<a class="scNscCustomerMemoContentsSaveButoon"><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="5" Text="保存"/></a>
                        <a class="scNscCustomerMemoContentsPlusButoon"></a>
					</div>
					<div class="scNscCustomerMemoDetailsPaperArea">
						<p class="scNscCustomerMemoDetailsPaperAreaDay"><icrop:CustomLabel ID="dateLabel" runat="server" TextWordNo="0" Text="2011/9/30"/></p>
						<p class="scNscCustomerMemoDetailsPaperAreaTime"><icrop:CustomLabel ID="timeLabel" runat="server" TextWordNo="0" Text="15:40"/></p>
						<p class="clearboth"></p>
                        <p class="scNscCustomerMemoDetailsPaperAreaTxt">                            
                            <asp:TextBox ID="memoTextBox" runat="server" Rows="20" Columns="70" TextMode="MultiLine" CssClass="ListIn" TabIndex="2"></asp:TextBox>
                         </p>
					</div>
				</div>
				<p class="clearboth"></p>
			</div>

<icrop:CustomLabel ID="deleteLabel" runat="server" TextWordNo="3" Text="削除" CssClass="disableLabel" />

		<%--</div>--%>
	<!-- ここまでコンテンツ -->
<%--	</div>
	<!-- ここまで中央部分 -->
		
</div><!--　←全体を含むタグ終わり　-->
</div><!--　←サイズ確認用のタグ終わり　-->--%>

    <asp:Button ID="memoSelectButton" runat="server" Text="メモ選択" CssClass="disableButton" />
    <asp:Button ID="saveButton" runat="server" Text="保存" CssClass="disableButton" />
    <asp:Button ID="deleteButton" runat="server" Text="削除" CssClass="disableButton" />

    <asp:HiddenField ID="activeSEQNO" runat="server" />
    <asp:HiddenField ID="mode" runat="server" />

    <asp:HiddenField ID="todayHidden" runat="server" />
    <asp:HiddenField ID="nowTimeHidden" runat="server" />

    <asp:HiddenField ID="listCountHidden" runat="server" />

    <!--2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START-->
    <asp:HiddenField ID="activeCSTMemoLockVersionHidden" runat="server" />
    <asp:HiddenField ID="cstLockVersionHidden" runat="server" />
    <!--2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END-->

</asp:Content>
