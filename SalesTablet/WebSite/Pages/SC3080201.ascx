<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080201.ascx
'─────────────────────────────────────
'機能： 顧客情報
'補足： 
'作成： 2011/11/18 TCS 山口
'更新： 2012/01/26 TCS 安田 【SALES_1B】レイアウト調整
'更新： 2012/03/12 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.58、No.100)
'更新： 2012/04/17 TCS 安田 【SALES_2】日付項目フォーカス対応　(ユーザー課題No24)
'更新： 2012/04/17 TCS 安田 【SALES_2】タップしても、隠れた文字が表示されない（ユーザー課題 No.39）
'更新： 2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応
'更新： 2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない
'更新： 2012/06/01 TCS 河原 FS開発
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
'更新： 2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発
'更新： 2013/11/27 TCS 市川 Aカード情報相互連携開発
'更新： 2013/12/25 TCS 市川 Aカード情報相互連携開発 追加要望
'更新： 2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計
'更新： 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動)
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354)
'更新： 2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80)
'更新： 2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-117
'更新： 2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応
'更新： 2015/04/01 TCS 外崎 セールスタブレット:M014
'更新： 2017/11/20 TCS 河原 TKM独自機能開発  
'更新： 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1  
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
'更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更
'更新： 2019/04/08 TS  舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える
'更新： 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究
'─────────────────────────────────────
-->
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080201.ascx.vb" Inherits="Pages_SC3080201" %>

    <link rel="Stylesheet" href="../Styles/SC3080201/SC3080201.css?20181122000001" type="text/css" media="screen,print"  />
    <script src="../Scripts/SC3080201/SC3080201.js?20200309000000" type="text/javascript"></script>

<%--2012/02/15 TCS 山口 【SALES_2】 START--%>
<%@ Register src="SC3080215.ascx" TagName="SC3080215" TagPrefix="uc1" %>
<%--2012/02/15 TCS 山口 【SALES_2】 END--%>

<!-- 顧客情報編集 START -->
<asp:Button ID="customerEditKanryoButton" runat="server" Text="" CssClass="disableButton" />
<asp:Button ID="vehicleKanryoButton" runat="server" Text="" CssClass="disableButton" />
<asp:Button ID="actvctgryidButton" runat="server" Text="" CssClass="disableButton" />
<icrop:CustomLabel ID="createCustomerLabel" runat="server" TextWordNo="40001" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="editCustomerLabel" runat="server" TextWordNo="40002" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="createVehicleLabel" runat="server" TextWordNo="50032" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="editVehicleLabel" runat="server" TextWordNo="50001" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="cancelLabel" runat="server" TextWordNo="40045" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="completionLabel" runat="server" TextWordNo="40046" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="nextVehicleLabel" runat="server" TextWordNo="40047" Text="" CssClass="disableLabel" />
<%--2017/11/20 TCS 河原 TKM独自機能開発 START--%>
<icrop:CustomLabel ID="dataCleansingLabel" runat="server" TextWordNo="40075" Text="" CssClass="disableLabel" />
<icrop:CustomLabel ID="Cleansingerror" runat="server" TextWordNo="40994" Text="" CssClass="disableLabel" />
<%--2017/11/20 TCS 河原 TKM独自機能開発 END--%>
<!-- 顧客情報編集 END -->

<asp:UpdatePanel ID="NameListActvctgryReasonListPanel" runat="server" UpdateMode="Always">
<ContentTemplate>
<asp:Panel runat="server" ID="NameListActvctgryReasonListVisiblePanel" Visible="false" >
<!-- 敬称リスト START -->
<asp:Panel ID="nameListPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scNameTitlePopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="nameTitleLabel2" runat="server" TextWordNo="40002" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="nameTitleLabel" runat="server"  
                                       TextWordNo="40006" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="ListBox01">

			<div class="dataWind2">
                <ul class="nscListBoxSetIn">
                    <asp:Repeater ID="nameTitleRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                        
                            <%--'2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START--%>
                            <li id="nameTitlelist<%# DataBinder.Eval(Container.DataItem, "NAMETITLE_CD").Trim()%>_<%# DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_CD").Trim()%>" class="nameTitlelist">                            
                            <%--'2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END--%>
                                <p class="nameTitleLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "NAMETITLE"))%></p>
                                <p class="namecdHidden"><%# DataBinder.Eval(Container.DataItem, "NAMETITLE_CD").Trim()%></p>
                                <p class="dispHidden"><%# DataBinder.Eval(Container.DataItem, "DISPFLG")%></p>
                                <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
                                <p class="privateFleetHidden"><%# DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_CD").Trim()%></p>
                                <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 敬称リスト START -->

<!-- 活動区分リスト START -->
<asp:Panel ID="actvctgryPopWndPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scActvctgryPopWindown">

		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="actvctgryTitleLabel2" runat="server" width="75px" TextWordNo="40002" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="actvctgryTitleLabel" runat="server"  
                                       TextWordNo="40039" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="Div3">

			<div class="dataWind2">
                <ul class="actvctgryListBoxSetIn">
                    <asp:Repeater ID="actvctgryRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="actvctgrylist<%# DataBinder.Eval(Container.DataItem, "ACTVCTGRYID")%>" class="actvctgrylist" value="<%# DataBinder.Eval(Container.DataItem, "ACTVCTGRYID")%>">
                                <p class="actvctgryLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ACTVCTGRYNAME"))%></p>
                                <p class="actvctgryHidden"><%# DataBinder.Eval(Container.DataItem, "ACTVCTGRYID")%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div style="height:15px;"></div>
			</div>
           
			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 活動区分リスト END -->

<%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
<!-- 年式リスト START -->
<asp:Panel ID="modelYearListPanel" runat="server" style="display:none">

    <!-- ここからコンテンツ -->
    <div id="scModelYearPopWindown">

        <!-- タブメニュー -->
        <div class="popWind">
            <div class="subWind">
                <div class="PopUpBtn01">
                    <div class="buttonClose">
                        <icrop:CustomLabel ID="modelYearBackLabel" runat="server" width="75px" TextWordNo="2020006" Text="" UseEllipsis="True" />
                    </div>
                    <div class="Arrow"></div>
                    <div class="title">
                        <icrop:CustomLabel ID="modelYearTitleLabel" runat="server" TextWordNo="2020004" Text="" UseEllipsis="True" />
                    </div>
                </div>

                <div class="dataWind1">
                    <div class="ListBox01" id="Div6">
                        <div class="dataWind2">
                            <ul class="modelYearListBoxSetIn">
                                <asp:Repeater ID="modelYearRepeater" runat="server" ClientIDMode="Predictable">
                                    <ItemTemplate>
                                        <li id="modelYearList<%# DataBinder.Eval(Container.DataItem, "MODEL_YEAR")%>" class="modelyearlist" value="<%# DataBinder.Eval(Container.DataItem, "MODEL_YEAR")%>">
                                            <p class="modelYearLabel"><icrop:CustomLabel ID="modelYearLiLabel" runat="server" Width="400px" UseEllipsis="true" CssClass="ellipsis" Text='<%#HttpUtility.HtmlEncode(Eval("MODEL_YEAR"))%>' /></p>
                                            <p class="modelYearCdHidden"><%# DataBinder.Eval(Container.DataItem, "MODEL_YEAR")%></p>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div style="height:15px;"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Panel>
<!-- 年式リスト END   -->
<%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>

<!-- 断念理由リスト START -->
<asp:Panel ID="reasonListPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scReasonPopWindown">

		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="reasonBackLabel" runat="server" width="75px"
                                       TextWordNo="40039" Text="" UseEllipsis="True" />
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START --%>
                    <icrop:CustomLabel ID="reasonTitleLabel" runat="server"  
                                       Text="" UseEllipsis="True" />
                    <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END --%>
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="Div2">

			<div class="dataWind2">
                <ul class="reasonListBoxSetIn">
                    <asp:Repeater ID="reasonRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START --%>
                            <li id="reasonlist<%# DataBinder.Eval(Container.DataItem, "ACT_CAT_TYPE")%>-<%# DataBinder.Eval(Container.DataItem, "REASONID")%>" class="reasonlist" value="<%# DataBinder.Eval(Container.DataItem, "REASONID")%>">
                            <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END --%>
                                <p class="reasoncdLabel"><icrop:CustomLabel ID="reasoncdLiLabel" runat="server" Width="400px" UseEllipsis="true" CssClass="ellipsis" Text='<%#HttpUtility.HtmlEncode(Eval("REASON"))%>' />
                                </p>
                                <p class="reasoncdHidden"><%# DataBinder.Eval(Container.DataItem, "REASONID")%></p>
                                <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START --%>
                                <p class="actvctgryidHidden"><%# DataBinder.Eval(Container.DataItem, "ACT_CAT_TYPE")%></p>
                                <%-- '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END --%>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div style="height:15px;"></div>
			</div>
           
			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 断念理由リスト START -->
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
<!-- 個人法人項目リスト START -->
<asp:Panel ID="privateFleetItemPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scPrivateFleetItemPopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="privateFleetItemLabel2" runat="server" TextWordNo="40002" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="privateFleetItemLabel" runat="server"  
                                       TextWordNo="40064" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="PrivateFleetItemListBox">

			<div class="dataWind2">
                <ul class="privateFleetItemListBoxSetIn">
                    <asp:Repeater ID="privateFleetItemRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="privateFleetItemList<%# DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_CD").Trim()%>" class="privateFleetItemList">
                                <p class="privateFleetItemLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_NAME"))%></p>
                                <p class="privateFleetItemHidden"><%# DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_CD").Trim()%></p>
                                <p class="fleetHidden"><%# DataBinder.Eval(Container.DataItem, "FLEET_FLG")%></p>
                                <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                <p class="cstJoinType" style="display: none"><%# DataBinder.Eval(Container.DataItem, "CST_JOIN_TYPE")%></p>
                                <p class="cstOrgnzNameRefType" style="display: none"><%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_NAME_REFERENCE_TYPE")%></p>
                                <p class="cstOrgnzNameInputType" style="display: none"><%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_NAME_INPUT_TYPE")%></p>
                                <p class="cstOrgnzNameDispType" style="display: none"><%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_NAME_DISP_TYPE")%></p>
                                <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 個人法人項目リスト END -->

<%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
<!-- 顧客組織リスト START -->
<asp:Panel ID="custOrgnzPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scCustOrgnzPopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="custOrgnzLabel2" runat="server" TextWordNo="40002" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="custOrgnzLabel" runat="server"
                                       TextWordNo="4000001" UseEllipsis="True" />
                </div>
			</div>

			<div class="dataWind1">
            
			<div class="ListBox01" id="CustOrgnzListBox">

			<div class="dataWind2">
                <table id="custOrgnzNameTextBoxTable">
                    <tr>
                        <td>
                            <icrop:CustomTextBox ID="custOrgnzNameTextBox" runat="server"
                                                 Width="330px" />
                            <icrop:CustomTextBox ID="custOrgnzNameSuggestiveTextBox" runat="server"
                                                 Width="330px" />
                        </td>
                    </tr>
                </table>
                <ul class="custOrgnzListBoxSetIn">
                    <asp:Repeater ID="custOrgnzRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="custOrgnzList<%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_CD").Trim()%>" class="custOrgnzList">
                                <p class="custOrgnzLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CST_ORGNZ_NAME"))%></p>
                                <p class="custOrgnzHidden"><%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_CD").Trim()%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 顧客組織リスト END -->

<!-- サブカテゴリ2リスト START -->
<asp:Panel ID="custSubCtgry2Panel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scCustSubCtgry2PopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="custSubCtgry2Label2" runat="server" TextWordNo="40002" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="custSubCtgry2Label" runat="server"  
                                       TextWordNo="4000002" UseEllipsis="True" />
                </div>
			</div>

			<div class="dataWind1">
                        
			<div class="ListBox01" id="CustSubCtgry2ListBox">

			<div class="dataWind2">
                <ul class="custSubCtgry2ListBoxSetIn">
                    <asp:Repeater ID="custSubCtgry2Repeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="custSubCtgry2List<%# DataBinder.Eval(Container.DataItem, "CST_SUBCAT2_CD").Trim()%>" class="custSubCtgry2List">
                                <p class="custSubCtgry2Label"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CST_SUBCAT2_NAME"))%></p>
                                <p class="custSubCtgry2Hidden"><%# DataBinder.Eval(Container.DataItem, "CST_SUBCAT2_CD").Trim()%></p>
                                <p class="custSubCtgry2PrivateFleetItemCd" style="display: none"><%# DataBinder.Eval(Container.DataItem, "PRIVATE_FLEET_ITEM_CD")%></p>
                                <p class="custSubCtgry2CustOrgnzCd" style="display: none"><%# DataBinder.Eval(Container.DataItem, "CST_ORGNZ_CD")%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- サブカテゴリ2リスト END -->
<%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>

<!-- 州リスト START -->
<asp:Panel ID="statePanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scStatePopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="stateLabel2" runat="server" TextWordNo="40002" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="stateLabel" runat="server"  
                                       TextWordNo="40056" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="stateListBox">

			<div class="dataWind2">
                <ul class="stateListBoxSetIn">
                    <asp:Repeater ID="stateRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="stateList<%# DataBinder.Eval(Container.DataItem, "STATE_CD").Trim()%>" class="stateList">
                                <p class="stateLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "STATE_NAME"))%></p>
                                <p class="stateHidden"><%# DataBinder.Eval(Container.DataItem, "STATE_CD").Trim()%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 州リスト END -->
<!-- 地域リスト START -->
<asp:Panel ID="districtPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scDistrictPopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="districtLabel2" runat="server" TextWordNo="40056" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="districtLabel" runat="server"  
                                       TextWordNo="40057" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="districtListBox">

			<div class="dataWind2">
                <ul class="districtListBoxSetIn">
                    <asp:Repeater ID="districtRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="districtList<%# DataBinder.Eval(Container.DataItem, "DISTRICT_CD").Trim()%>" class="districtList">
                                <p class="districtLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DISTRICT_NAME"))%></p>
                                <p class="districtHidden"><%# DataBinder.Eval(Container.DataItem, "DISTRICT_CD").Trim()%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 地域リスト END -->
<!-- 市リスト START -->
<asp:Panel ID="cityPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scCityPopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="cityLabel2" runat="server" TextWordNo="40057" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="cityLabel" runat="server"  
                                       TextWordNo="40058" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="cityListBox">

			<div class="dataWind2">
                <ul class="cityListBoxSetIn">
                    <asp:Repeater ID="cityRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="cityList<%# DataBinder.Eval(Container.DataItem, "CITY_CD").Trim()%>" class="cityList">
                                <p class="cityLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CITY_NAME"))%></p>
                                <p class="cityHidden"><%# DataBinder.Eval(Container.DataItem, "CITY_CD").Trim()%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 市リスト END -->
<!-- 地区リスト START -->
<asp:Panel ID="locationPanel" runat="server" style="display:none">

	<!-- ここからコンテンツ -->
	<div id="scLocationPopWindown">
    
		<!-- タブメニュー -->
		<div class="popWind">
		<div class="subWind">
			<div class="PopUpBtn01">
                <div class="buttonClose">
                    <icrop:CustomLabel ID="locationLabel2" runat="server" TextWordNo="40058" width="75px" Text=""/>
                </div>
				<div class="Arrow"></div>
				<div class="title">
                    <icrop:CustomLabel ID="locationLabel" runat="server"  
                                       TextWordNo="40059" Text="" UseEllipsis="True" />
                </div>
			</div>
                        
			<div class="dataWind1">
                        
			<div class="ListBox01" id="locationListBox">

			<div class="dataWind2">
                <ul class="locationListBoxSetIn">
                    <asp:Repeater ID="locationRepeater" runat="server" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <li id="locationList<%# DataBinder.Eval(Container.DataItem, "LOCATION_CD").Trim()%>" class="locationList">
                                <p class="locationLabel"><%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "LOCATION_NAME"))%></p>
                                <p class="locationHidden"><%# DataBinder.Eval(Container.DataItem, "LOCATION_CD").Trim()%></p>
                                <p class="locationZipHidden"><%# DataBinder.Eval(Container.DataItem, "ZIP_CD").Trim()%></p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>                    
                </ul>                
                <div style="height:30px;"></div>
			</div>

			</div>

			</div>

		</div>
		</div>
	</div>

</asp:Panel>
<!-- 地区リスト END -->
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
</asp:Panel>
</ContentTemplate> 
</asp:UpdatePanel> 

<%--<2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 START>--%>
<div id="CustomerEditOverlayBlack"></div>
<%--<2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 END>--%>
<!-- 顧客情報編集 START -->
    <asp:UpdatePanel ID="customerEditPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
    <asp:Button runat="server" ID="CustomerEditPopupOpenButton" style="display:none" />
	<!-- ここからコンテンツ -->
        <%--<2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 START>--%>
        <div id="CustomerEditOverlayBlack"></div>
        <%--<2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 END>--%>
          		<div id="scNscCustomerEditingWindown" style="display:none;">

		<div id="scNscCustomerEditingWindownBox">
			<div class="scNscCustomerEditingHadder">
                <h3>
                    <icrop:CustomLabel ID="customerTitleLabel" runat="server" TextWordNo="40001" Text=""/>
                </h3>
				<a href="#" id="scNscCustomerEditingCancell" class="scNscCustomerEditingCancellButton">
                    <icrop:CustomLabel ID="cancelButtonLabel" runat="server" 
                    TextWordNo="40045" Text="" UseEllipsis="False" Width="75px"/>
                </a>
                <a href="#" id="scNscCustomerEditingCompletion" class="scNscCustomerEditingCompletionButton">
                    <icrop:CustomLabel ID="completionButtonLabel" runat="server" 
                    TextWordNo="40046" Text="" UseEllipsis="False" Width="75px"/>
                </a>
                <a href="#" class="scNscCustomerEditingCompletionArrow">
                </a>
			</div>
			<div class="scNscCustomerEditingListArea" style="overflow:hidden;">
                <asp:Panel runat="server" ID="CustomerEditVisiblePanel" Visible="false" >
				<div class="scNscCustomerEditingListBox page1">
				    <div class="scNscCustomerEditingListBox2">
                    <%--2017/11/20 TCS 河原 TKM独自機能開発 START--%>
					<div class="scNscCustomerEditingListItemBox">
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--ファーストネーム、ミドルネーム、ラストネーム--%>
						<div class="scNscCustomerEditingListItem1">
                            <asp:Panel ID="namePanel" runat="server">
							<table id="nameTable" border="0" cellspacing="0" cellpadding="0">
								<tr id="row01">
								    <th id="header01" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="customerFiestNameLabel" runat="server" 
                                            TextWordNo="40048" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis scNscCustomerEditingListItemRedTxt"/>
                                        <icrop:CustomLabel ID="customerFiestNameLabel2" runat="server" 
                                            TextWordNo="40062" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis scNscCustomerEditingListItemRedTxt" style="display:none" />
                                    </th>
								    <td id="data01" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="nameTextBox" runat="server" MaxLength="64" width="340px"
                                            TabIndex="2001"></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row02">
								    <th id="header02" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="customerMiddleNameLabel" runat="server" 
                                            TextWordNo="40049" Width="85px" 
                                            Text="" UseEllipsis="True" CssClass="ellipsis" />
                                    </th>
								    <td id="data02" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="middleNameTextBox" runat="server" MaxLength="64" width="340px"
                                            TabIndex="2001"></icrop:CustomTextBox>
                                    </td>
								</tr>
    						    <tr id="row03">
								        <th id="header03" >
                                            <icrop:CustomLabel ID="customerLastNameLabel" runat="server" 
                                                TextWordNo="40050" Width="85px"
                                                Text="" UseEllipsis="True" CssClass="ellipsis" />
                                            <icrop:CustomLabel ID="customerLastNameLabel2" runat="server" 
                                                TextWordNo="40063" Width="85px"
                                                Text="" UseEllipsis="True" CssClass="ellipsis" style="display:none" />
                                        </th>
								        <td id="data03" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                            <icrop:CustomTextBox ID="lastNameTextBox" runat="server" MaxLength="64" width="340px"
                                                TabIndex="2001"></icrop:CustomTextBox>
                                        </td>
							    </tr>
							</table>
                            </asp:Panel>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--敬称--%>
						<div class="scNscCustomerEditingNameTitle">
							<table id="row05" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header05">
                                        <icrop:CustomLabel ID="customerNameTitleLabel" runat="server" 
                                            TextWordNo="40005" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis" />
                                    </th>
								    <td id="data05" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="nameTitle" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" PlaceHolderWordNo="40005" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
                        
						<p class="clearboth"></p>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--性別--%>
						<div class="scNscCustomerEditingListItem3">
							<table id="row04" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header04">
                                        <icrop:CustomLabel ID="CustomLabel10" runat="server" TextWordNo="40007"  Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                						<div class="scText">
                                            <icrop:CheckMark
                                            ID="manCheckBox" runat="server"  TextWordNo="40008" 
                                            Height="30px"
                                                Text="" CssClass="scMunCheck" 
                                                onclick="selectSex();" TextAlign="Left" />
                                        </div>
                                                 
                                    </td>
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                                        <icrop:CheckMark
                                        ID="girlCheckBox" runat="server"  TextWordNo="40009" 
                                        Height="30px"
                                            Text="" CssClass="scGirlCheck" />
                                    </td>
                                    <asp:Panel ID="sexOtherCol" runat="server">
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                                        <icrop:CheckMark
                                        ID="otherCheckBox" runat="server"  TextWordNo="40051" 
                                        Height="30px"
                                            Text="" CssClass="scOtherCheck" />
                                    </td>
                                    </asp:Panel>
                                    <asp:Panel ID="sexUnknownCol" runat="server">
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                                        <icrop:CheckMark
                                        ID="unknownCheckBox" runat="server"  TextWordNo="40052" 
                                        Height="30px"
                                            Text="" CssClass="scUnknownCheck" />
                                    </td>
                                    </asp:Panel>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--個人/法人--%>
						<div class="scNscCustomerEditingListItem4">
							<table id="row06" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header06">
                                        <icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="40010" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                                        <icrop:CheckMark
                                        ID="kojinCheckBox" runat="server"  TextWordNo="40011" 
                                        Height="30px" Text="" TextAlign="Left" 
                                            CssClass="scNscCustomerEditingListItemBlueTxt scKojinCheck" />
                                    </td>
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemBox">
                                        <icrop:CheckMark
                                        ID="houjinCheckBox" runat="server"  TextWordNo="40012" 
                                        Height="30px" Text="" TextAlign="Left" 
                                            CssClass="scNscCustomerEditingListItemBlueTxt scHoujinCheck" />
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--個人法人項目(サブ区分)--%>
						<div class="scNscCustomerEditingPrivateFleetItem" id="PrivateFleetItem">
							<table id="row07" border="0" cellspacing="0" cellpadding="0">
								<tr>
<%--2015/04/01 TCS 外崎 セールスタブレット:M014 START--%>
								    <th id="header07">
                                        <icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="40074" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
<%--2015/04/01 TCS 外崎 セールスタブレット:M014 END--%>
								    <td class="scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="privateFleetItem" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

                        <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                        <%--顧客組織名称--%>
                        <div class="scNscCustomerEditingCustOrgnz" id="CustOrgnz">
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th id="headerCustOrgnz">
                                        <icrop:CustomLabel runat="server" width="85px"
                                            TextWordNo="4000001" UseEllipsis="true" CssClass="ellipsis" />
                                    </th>
                                    <td class="scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack">
                                        <icrop:CustomTextBox ID="custOrgnz" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt"
                                            Width="330px"
                                            ReadOnly="true" UseEllipsis="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <%--顧客サブカテゴリ2--%>
                        <div class="scNscCustomerEditingCustSubCtgry2" id="CustSubCtgry2">
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th id="headerCustSubCtgry2">
                                        <icrop:CustomLabel ID="CustomLabel23" runat="server" width="85px"
                                            TextWordNo="4000002" UseEllipsis="true" CssClass="ellipsis" />
                                    </th>
                                    <td class="scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack">
                                        <icrop:CustomTextBox ID="custSubCtgry2" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt"
                                            Width="330px"
                                            ReadOnly="true" UseEllipsis="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>

						<p class="clearboth">&nbsp;</p>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--法人入力項目--%>
                        <asp:Panel ID="houjinPanel" runat="server" style="display:none">
						<div class="scNscCustomerEditingListItem5" id="法人入力">
							<table id="houjinTable" border="0" cellspacing="0" cellpadding="0">
								<tr id="row08">
								    <th id="header08" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="40013" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data08" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="employeenameTextBox" runat="server" MaxLength="256" TabIndex="2002" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row09">
								    <th id="header09" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="40014" 
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data09" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="employeedepartmentTextBox" runat="server" 
                                            MaxLength="64" TabIndex="2003" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row10">
								    <th id="header10">
                                        <icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="40016"
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data10" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="employeepositionTextBox" runat="server" MaxLength="64" TabIndex="2004" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
                        </asp:Panel>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--電話番号--%>
						<div class="scNscCustomerEditingListItem5">
							<table id="telTable" border="0" cellspacing="0" cellpadding="0">
								<tr id="row11">
								    <th id="header11" class="scNscCustomerEditingListItemBottomBorder">
                                        <span class="scNscCustomerEditingListItemRedTxt">
                                            <icrop:CustomLabel ID="CustomLabel15" runat="server" TextWordNo="40018" 
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"  />
                                        </span>
                                    </th>
								    <td id="data11" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
                                        <icrop:CustomTextBox ID="mobileTextBox" runat="server" MaxLength="128" TabIndex="2005" 
                                            Width="300px" ></icrop:CustomTextBox>                                        
                                        <asp:Image ID="mobileSerchButtonImage" runat="server" 
                                            ImageUrl="~/Styles/Images/SC3080205/scNscCustomerEditingListBackMagnifying.png" 
                                            ImageAlign="Right"
                                            CssClass="scNscCustomerEditingZipButton"  />
                                        <asp:Button runat="server" ID="mobileSerchButton" style="display:none" />
                                        <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>
                                    </td>
								</tr>
								<tr id="row12">
								    <th id="header12" class="scNscCustomerEditingListItemBottomBorder">
                                        <span class="scNscCustomerEditingListItemRedTxt">
                                            <icrop:CustomLabel ID="CustomLabel16" runat="server" TextWordNo="40020" 
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                        </span>
                                    </th>
								    <td id="data12" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
                                        <icrop:CustomTextBox ID="telnoTextBox" runat="server" MaxLength="64" TabIndex="2006" PlaceHolderWordNo="40076"
                                            Width="300px" ></icrop:CustomTextBox>
                                        <asp:Image ID="telnoSerchButtonImage" runat="server" 
                                            ImageUrl="~/Styles/Images/SC3080205/scNscCustomerEditingListBackMagnifying.png" 
                                            ImageAlign="Right"
                                            CssClass="scNscCustomerEditingZipButton"  />
                                        <asp:Button runat="server" ID="telnoSerchButton" style="display:none" />
                                        <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>
                                    </td>
								</tr>
								<tr id="row13">
								    <th id="header13" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="CustomLabel17" runat="server" TextWordNo="40021" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data13" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="businesstelnoTextBox" runat="server" MaxLength="64" TabIndex="2007" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row14">
								    <th id="header14">
                                        <icrop:CustomLabel ID="CustomLabel18" runat="server" TextWordNo="40022" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data14" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="faxnoTextBox" runat="server" MaxLength="64" TabIndex="2008" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
                        <%--住所--%>
						<div class="scNscCustomerEditingListItem5" id="addressAll">
							<table id="addressTable" border="0" cellspacing="0" cellpadding="0">
								<%--郵便番号--%>
								<tr id="row15">
								    <th id="header15" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="CustomLabel19" runat="server" TextWordNo="40023" 
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data15" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="zipcodeTextBox" runat="server" MaxLength="32" TabIndex="2009" 
                                            Width="300px" onKeyUp="changeZipCode(this, zipSerchButton)" ></icrop:CustomTextBox>

                                        <asp:Image ID="zipSerchButton" runat="server" 
                                                   ImageUrl="~/Styles/Images/SC3080205/scNscCustomerEditingListBackMagnifying.png" 
                                                   ImageAlign="Right"
                                                   CssClass="scNscCustomerEditingZipButton"  />
                                    </td>
								</tr>
								<%--ADD 住所(1、2、3、州、地域、市、地区)--%>
								<tr id="row16">
								    <th id="header16" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="Address1Label" runat="server" TextWordNo="40053" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data16" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="addressTextBox" runat="server" MaxLength="256" TabIndex="2010" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row17">
								    <th id="header17" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="Address2Label" runat="server" TextWordNo="40054" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data17" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="address2TextBox" runat="server" MaxLength="256" TabIndex="2010" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row18">
								    <th id="header18" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="Address3Label" runat="server" TextWordNo="40055" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data18" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="address3TextBox" runat="server" MaxLength="256" TabIndex="2010" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row19" class="scNscCustomerEditingState">
								    <th id="header19" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="stateTitleLabel" runat="server" 
                                            TextWordNo="40056"  
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data19" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="addressState" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" PlaceHolderWordNo="40056" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
								<tr id="row20" class="scNscCustomerEditingDistrict">
								    <th id="header20" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="districtTitleLabel" runat="server" 
                                            TextWordNo="40057"  
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data20" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="addressDistrict" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" PlaceHolderWordNo="40057" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
								<tr id="row21" class="scNscCustomerEditingCity">
								    <th id="header21" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="cityTitleLabel" runat="server" 
                                            TextWordNo="40058"  
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data21" class="scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="addressCity" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" PlaceHolderWordNo="40058" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
								<tr id="row22" class="scNscCustomerEditingLocation">
								    <th id="header22">
                                        <icrop:CustomLabel ID="locationTitleLabel" runat="server" 
                                            TextWordNo="40059"  
                                            Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data22" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt scNscCustomerEditingListItemArrowBack" >
                                        <icrop:CustomTextBox ID="addressLocation" runat="server"
                                            CssClass="scNscCustomerEditingNameTitleItemBlueTxt" 
                                            Width="330px" PlaceHolderWordNo="40059" 
                                            ReadOnly="True" UseEllipsis="True"></icrop:CustomTextBox>
                                        <div  class="icon01"></div>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--本籍--%>
						<div class="scNscCustomerEditingListItem5" id="Domicile">
							<table id="row23" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header23">
                                        <icrop:CustomLabel ID="domicileLabel" runat="server" TextWordNo="40060" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data23" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="domicileTextBox" runat="server" MaxLength="320" TabIndex="2013" Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--e-Mail--%>
						<div class="scNscCustomerEditingListItem5" id="mail">
							<table id="emailTable" border="0" cellspacing="0" cellpadding="0">
								<tr id="row24">
								    <th id="header24" class="scNscCustomerEditingListItemBottomBorder">
                                        <icrop:CustomLabel ID="CustomLabel21" runat="server" TextWordNo="40027" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data24" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemBottomBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="email1TextBox" runat="server" MaxLength="128" TabIndex="2011" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
								<tr id="row25">
								    <th id="header25">
                                        <icrop:CustomLabel ID="CustomLabel22" runat="server" TextWordNo="40029" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data25" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="email2TextBox" runat="server" MaxLength="128" TabIndex="2012" 
                                            Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--国籍--%>
						<div class="scNscCustomerEditingListItem5" id="country" >
							<table id="row26" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header26">
                                        <icrop:CustomLabel ID="countryLabel" runat="server" TextWordNo="40061" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data26" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="countryTextBox" runat="server" MaxLength="64" TabIndex="2013" Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--国民ID--%>
						<div class="scNscCustomerEditingListItem5" id="socialId">
							<table id="row27" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header27" >
                                        <icrop:CustomLabel ID="CustomLabel123" runat="server" TextWordNo="40030" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data27" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
<%--
                                        <icrop:CustomTextBox ID="socialidTextBox" runat="server" MaxLength="31" TabIndex="2013" Width="340px" ></icrop:CustomTextBox>
--%>
                                        <icrop:CustomTextBox ID="socialidTextBox" runat="server" MaxLength="31" TabIndex="2013" Width="300px" ></icrop:CustomTextBox>
										<%--国民ID検索ボタン--%>
                                        <asp:Image ID="socialIdSearchButtonImage" runat="server" 
                                            ImageUrl="~/Styles/Images/SC3080205/scNscCustomerEditingListBackMagnifying.png" 
                                            ImageAlign="Right"
                                            CssClass="scNscCustomerEditingZipButton"  />
                                        <asp:Button runat="server" ID="socialIdSearchButton" style="display:none" />
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>

<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--誕生日--%>
						<div class="scNscCustomerEditingListItem5" id="birthday">
							<table id="row28" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header28">
                                        <icrop:CustomLabel ID="CustomLabel124" runat="server" TextWordNo="40032" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis"/>
                                    </th>
								    <td id="data28" class="scNscCustomerEditingListItemLeftBorder  scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <%--2012/04/17 TCS 安田 【SALES_2】日付項目フォーカス対応(ユーザー課題No24) START--%>
                                        <%--TabIndexプロパティをなくす --%>
<%'2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START %>
                                        <icrop:DateTimeSelector ID="birthdayTextBox" runat="server" width="100%" height="100%" />
<%'2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END %>
                                        <%--2012/04/17 TCS 安田 【SALES_2】日付項目フォーカス対応(ユーザー課題No24) END--%>
                                    </td>
								</tr>
							</table>
						</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
                        <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START --%>
                        <%-- 2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START --%>
                        <%-- 商業情報受取区分 --%>
						<div class="scNscCustomerEditingListItem5" id="commercialRecvType">
							<table id="row36" cellspacing="0">
								<tr>
								    <th id="header36">
                                        <icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="40069" Text="" Width="85px" />
                                    </th>
								    <td style="width:110px;" class="scNscCustomerEditingListItemLeftBorder ">
                                        <icrop:CheckMark ID="commercialRecvType_Empty" runat="server" TextWordNo="40070" Text="" Height="30px" CssClass="CommercialRecvType" />
                                    </td>
								    <td style="width:110px;" class="scNscCustomerEditingListItemLeftBorder ">
                                        <icrop:CheckMark ID="commercialRecvType_Yes" runat="server" TextWordNo="40071" Text="" Height="30px" CssClass="CommercialRecvType" />
                                     </td>
								    <td style="width:115px;" class="scNscCustomerEditingListItemLeftBorder ">
                                        <icrop:CheckMark ID="commercialRecvType_No" runat="server" TextWordNo="40072" Text="" Height="30px" CssClass="CommercialRecvType" />
                                    </td>
								</tr>
							</table>
						</div>
                        <%-- 2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END --%>
                        <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END --%>
                        <%-- 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START --%>
						<div class="scNscCustomerEditingListItem5" id="income">
							<table border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th>
                                        <icrop:CustomLabel ID="incomeTextBoxLabel" runat="server" TextWordNo="40073" Text=""/>
                                    </th>
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemGrayTxt Fontgray">
                                        <icrop:CustomTextBox ID="incomeTextBox" runat="server" MaxLength="32" TabIndex="2014" Width="340px" ></icrop:CustomTextBox>
                                    </td>
								</tr>
							</table>
						</div>
                        <%-- 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END --%>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
						<%--活動区分--%>
<asp:Panel ID="actvctgryPanel" runat="server">
						<div id="idNscCustomerEditingWindown" class="scNscCustomerEditingActvctgry scNscCustomerEditingListItem5">
							<table id="row29" border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th id="header29">
                                        <icrop:CustomLabel ID="CustomLabel125" runat="server" TextWordNo="40034" Width="85px"
                                            Text="" UseEllipsis="True" CssClass="ellipsis" PlaceHolderWordNo="40034" />
                                    </th>
								    <td id="data29" class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemArrowBack">

                                        <icrop:CustomLabel ID="actvctgryLabel" runat="server" 
                                            TextWordNo="0" 
                                            Text="" 
                                            CssClass="scNscCustomerEditingListItemWeightNormalTxt" 
                                            UseEllipsis="True" Width="330px" />    
                                        <div  class="icon01" id="actvctgry"></div>

                                    </td>

								</tr>
							</table>
						</div>
</asp:Panel>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
<asp:Panel ID="rmmPanel" runat="server">
						<div class="scNscCustomerEditingListItem6">
							<table border="0" cellspacing="0" cellpadding="0">
								<tr>
								    <th>
                                        <icrop:CustomLabel ID="CustomLabel26" runat="server" TextWordNo="40041" Text=""/>
								    </th>
<asp:Panel ID="smsPanel" runat="server">
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt">
                                        <icrop:CheckMark
                                            ID="smsCheckButton" runat="server"  TextWordNo="40042" Width="70px" 
                                            Height="30px" Text="" CssClass="scSmsCheck"/>
                                    </td>
</asp:Panel>
<asp:Panel ID="emailPanel" runat="server">
								    <td class="scNscCustomerEditingListItemLeftBorder scNscCustomerEditingListItemWeightNormalTxt">
                                            <icrop:CheckMark
                                            ID="emailCheckButton" runat="server"  TextWordNo="40043" Width="90px" 
                                            Height="30px" Text="" CssClass="scEmailCheck"/>
                                    </td>
</asp:Panel>
								</tr>
							</table>
<asp:Panel ID="dmailPanel" runat="server">
                            <div class="Dmail">
                                        <icrop:CheckMark
                                        ID="dmailCheckButton" runat="server"  TextWordNo="40044" Width="80px" 
                                        Height="30px" Text="" CssClass="scNscCustomerEditingListItemBlueTxt scDmailCheck"/>
                            </div>
</asp:Panel>
						</div>
</asp:Panel>
                        <div style="height:15px;"></div>
					</div>
                    <%--2017/11/20 TCS 河原 TKM独自機能開発 END--%>
                    </div>
                    
					<div class="dataWindNameTitle">

					</div>
					<div class="dataWindActvctgry">

					</div>
					<div class="dataWindReason">

					</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
					<div class="dataWindPrivateFleetItem">

					</div>
                    <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                    <div class="dataWindCustOrgnz">
                    
                    </div>
                    <div class="dataWindCustSubCtgry2">
                    
                    </div>
                    <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
					<div class="dataWindState">

					</div>
					<div class="dataWindDistrict">

					</div>
					<div class="dataWindCity">

					</div>
					<div class="dataWindLocation">

					</div>
<%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
                    <%--'2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START--%>
					<div class="dataWindModelYear">
					</div>
                    <%--'2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END  --%>
   				</div>
                </asp:Panel> 
			</div>
		</div>
                
		</div><!-- ここまでタブメニュー -->

    </ContentTemplate>
    </asp:UpdatePanel>
<!-- 顧客情報編集 END -->


<!-- 車両情報編集 START -->

<asp:UpdatePanel ID="VehicleUpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!-- ここからコンテンツ -->
<%--2012/02/15 TCS 山口 【SALES_2】 START--%>
<asp:Button runat="server" ID="CustomerCarEditPopupOpenButton" style="display:none" />
<div id="scVehicleEditingWindown" style="display:none;">
	<div id="scVehicleEditingWindownBox">
		<div class="scVehicleEditingHadder">
			<h3>
                <icrop:CustomLabel ID="vehicleTitleLabel" runat="server" TextWordNo="50001" Text=""/>
            </h3>
			<a href="#" class="scVehicleEditingCancellButton">
                <icrop:CustomLabel ID="vehicleCancelButtonLabel" runat="server" TextWordNo="50002" Text="" UseEllipsis="False" Width="75px"/>
            </a>
            <a href="#" class="scVehicleEditingCompletionButton">
                <icrop:CustomLabel ID="vehicleCompletionButtonLabel" runat="server" TextWordNo="50003" Text="" UseEllipsis="False" Width="75px"/>
            </a>
	</div>
	<div class="scVehicleEditingListArea" style="overflow:hidden;">
        <asp:Panel runat="server" ID="CustomerCarEditVisiblePanel" Visible="false" >
			<div class="scVehicleEditingListBox page1">
				<div class="scVehicleEditingListBox2">
				<div class="scVehicleEditingListItemBox">
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel30" runat="server" TextWordNo="50004" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <span class="Boder">
                                        <icrop:CustomTextBox ID="makerTextBox" Width="330px" runat="server" 
                                        MaxLength="128" TabIndex="3001"></icrop:CustomTextBox>
                                    </span>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder2">
                                    <icrop:CustomLabel ID="CustomLabel31" runat="server" TextWordNo="50005" Text="" CssClass="scVehicleEditingListItemRedTxt" />
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder2 scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="modelTextBox" Width="330px" runat="server" 
                                        MaxLength="32" TabIndex="3002"></icrop:CustomTextBox>
                            </td>
						    </tr>
    <asp:Panel ID="orgcustPanel0" runat="server">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel32" runat="server" TextWordNo="50011" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="gradeTextBox" Width="330px" runat="server" Enabled="True"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel33" runat="server" TextWordNo="50009" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="bdyclrTextBox" Width="330px" runat="server" Enabled="True"></icrop:CustomTextBox>
                                </td>
						    </tr>
    </asp:Panel>
					    </table>
				    </div>
                    <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                    <div class="scVehicleEditingListItem5 scNscVehicleEditingModelYear">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel201" runat="server" TextWordNo="2020005" Text=""/>
                                </th>
                                <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="vclMileTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <icrop:CustomLabel ID="CustomLabel202" runat="server" TextWordNo="2020004" Text=""/>
                                </th>
                                <td class="scVehicleEditingModelYear scNscCustomerEditingListItemArrowBack">

                                    <icrop:CustomLabel ID="modelYearLabel2" runat="server" 
                                        TextWordNo="0" 
                                        Text="" 
                                        CssClass="scNscCustomerEditingListItemWeightNormalTxt" 
                                        UseEllipsis="True" Width="330px" />    
                                    <div  class="icon01" id="Div5"></div>

                                </td>
                            </tr>
                        </table>
                    </div>
                    <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>                            
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel34" runat="server" TextWordNo="50006" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="vclregnoTextBox" Width="330px" runat="server" 
                                        MaxLength="32" TabIndex="3003"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder2">
                                    <icrop:CustomLabel ID="CustomLabel35" runat="server" TextWordNo="50007" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder2 scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="vinTextBox" Width="330px" runat="server" 
                                        MaxLength="128" TabIndex="3004"></icrop:CustomTextBox>
                                </td>
						    </tr>
    <asp:Panel ID="orgcustPanel1" runat="server">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel36" runat="server" TextWordNo="50008" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="fueldvsTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel37" runat="server" TextWordNo="50010" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="enginenoTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel38" runat="server" TextWordNo="50029" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="baseTypeTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
    </asp:Panel>
					    </table>
				    </div>
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
    <asp:Panel ID="orgcustPanel2" runat="server">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel39" runat="server" TextWordNo="50012" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                        <icrop:CustomTextBox ID="vclregdateTextBox" Width="100px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
    </asp:Panel>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder2">
                                    <icrop:CustomLabel ID="CustomLabel40" runat="server" TextWordNo="50013" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder2 scVehicleEditingListItemWeightNormalTxt">
                                    <%--2012/04/17 TCS 安田 【SALES_2】日付項目フォーカス対応(ユーザー課題No24) START--%>
                                    <%--TabIndexプロパティをなくす --%>
<%'2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START %>
                                    <icrop:DateTimeSelector ID="vcldelidateDateTime" runat="server" width="100%" height="100%" PlaceHolderWordNo="50037" />
<%'2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END %>
                                    <%--2012/04/17 TCS 安田 【SALES_2】日付項目フォーカス対応(ユーザー課題No24) END--%>
                                    <icrop:CustomTextBox ID="vcldelidateTextBox" Width="100px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
    <asp:Panel ID="orgcustPanel3" runat="server">
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel41" runat="server" TextWordNo="50014" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                        <icrop:CustomTextBox ID="registdateTextBox" Width="100px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
    </asp:Panel>
					    </table>
				    </div>
    <asp:Panel ID="orgcustPanel4" runat="server">
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel42" runat="server" TextWordNo="50015" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <span class="Boder">
                                        <icrop:CustomTextBox ID="newvcldvsTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                    </span>
                                </td>
						    </tr>
    <asp:Panel ID="cpoDisplayFlgPanel" runat="server">
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="50016" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="cponmTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
    </asp:Panel>
					    </table>
				    </div>
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
						    <th>
                                <icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="50017" Text=""/>
                            </th>
						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                <icrop:CustomTextBox ID="mileageTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                            </td>
						    </tr>
					    </table>
				    </div>
				    <div class="scVehicleEditingListItem5 scNscVehicleEditingActvctgry">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
						    <th>
                                <icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="50018" Text=""/>
                            </th>
						    <td class="scVehicleEditingActvctgry scNscCustomerEditingListItemArrowBack">

                                <icrop:CustomLabel ID="actvctgryLabel2" runat="server" 
                                    TextWordNo="0" 
                                    Text="" 
                                    CssClass="scNscCustomerEditingListItemWeightNormalTxt" 
                                    UseEllipsis="True" Width="330px" />    
                                <div  class="icon01" id="Div4"></div>

                            </td>
						    </tr>
					    </table>
				    </div>
				    <div class="scVehicleEditingListItem10">
                        <icrop:CustomLabel ID="CustomLabel60" runat="server" TextWordNo="50031" Text=""/>
				    </div>
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="50019" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="systemidTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="50020" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="regstatusTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
					    </table>
				    </div>
                            
    <asp:Panel ID="telemaDisplayFlgPanel" runat="server">
				    <div class="scVehicleEditingListItem10">
                        <icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="50038" Text=""/>
				    </div>
				    <div class="scVehicleEditingListItem5">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="50021" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="contractstatusTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="50022" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="connectdvsTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="50023" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="contractstartdateTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="50024" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="contractenddateTextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel52" runat="server" TextWordNo="50025" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="telematelnumber1TextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel53" runat="server" TextWordNo="50026" Text=""/>
                                </th>
    						    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="telematelnumber2TextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th class="scVehicleEditingListItemBottomBorder">
                                    <icrop:CustomLabel ID="CustomLabel54" runat="server" TextWordNo="50027" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemBottomBorder scVehicleEditingListItemWeightNormalTxt">
                                    <icrop:CustomTextBox ID="telematelnumber3TextBox" Width="330px" runat="server"></icrop:CustomTextBox>
                                </td>
						    </tr>
						    <tr>
							    <th>
                                    <icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="50028" Text=""/>
                                </th>
							    <td class="scVehicleEditingListItemLeftBorder scVehicleEditingListItemWeightNormalTxt">
                                        <icrop:CheckMark
                                        ID="gbookCheckButton" runat="server"  TextWordNo="50039" Width="100px" 
                                        Height="30px" Text="" Enabled="False" CssClass="scGBookCheck" />
                                </td>
						    </tr>
					    </table>
				    </div>
    </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="newVehiclePanel" runat="server">
				    <div class="scVehicleEditingListItem7 scVehicleAppendButton">
					    <table border="0" cellspacing="0" cellpadding="0">
						    <tr>
						    <td class="scVehicleEditingListItemTxtCenter">
                                 <icrop:CustomLabel ID="CustomLabel58" runat="server" TextWordNo="50030" Text=""/>
                            </td>
						    </tr>
					    </table>
				    </div>
    </asp:Panel>	                
                <div style="height:15px;"></div>
				</div>
				</div>
                
				<div class="dataWindActvctgry">

				</div>
				<div class="dataWindReason">

				</div>
				<%--'2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START--%>
				<div class="dataWindModelYear">
				</div>
				<%--'2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END  --%>
			</div>
            </asp:Panel>
		</div>
	</div>
</div>
<%--2012/02/15 TCS 山口 【SALES_2】 END--%>
</ContentTemplate>
</asp:UpdatePanel>
<!-- 車両情報編集 END -->



<%--	<div id="scNscCircleArea">
		<p class="scNscCircleOn">&nbsp;</p>
		<p class="scNscCircleOff">&nbsp;</p>
		<p class="scNscCircleOff">&nbsp;</p>
		<p class="clearboth"></p>
	</div>--%>
    <uc1:SC3080215 ID="SC3080215" runat="server" EnableViewState="false" TriggerClientID="CSSurveyButton" />    

	<div id="scNscCustomerLeftArea" class="contentsFrame" style="height: 614px;">
		<h2 class="contentTitle" >
            <icrop:CustomLabel ID="WordLiteral101" runat="server" Width="200px" CssClass="styleCut" TextWordNo="10101" />
        </h2>
		<div class="scNscCustomerInfoArea">

            <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
<%--            <button type="button" id="CSSurveyButton" runat="server" class="CSSurveyBtn colorSetON" onclick="CSSurveyClick();" >
                <icrop:CustomLabel ID="CSSurveyLabelOn" runat="server" Width="80px" CssClass="styleCut" />
            </button>
            <button type="button" id="CSSurveyButtonOff" runat="server" class="CSSurveyBtn colorSetOFF" visible="false" >
                <icrop:CustomLabel ID="CSSurveyLabelOff" runat="server" Width="80px" CssClass="styleCut" />
            </button>--%>
            <%--2012/02/15 TCS 山口 【SALES_2】 END--%>

            <asp:UpdatePanel ID="customerInfoPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
			        <!-- 顧客個人情報 -->
                    <asp:Panel ID="EditCustomerNamePanel" runat="server" Visible="true" >
                        <div class="scNscCustomerNameArea">		
					        <div id="CustomerPhotoArea" class="scNscCustomerPhoto" runat="server" onclick="photoSelectOpen();">
                                <asp:ImageButton ID="facePictureButton"  runat="server" ImageAlign="NotSet" width="60" height="60" ImageUrl="~/Styles/Images/SC3080201/bgFamilyBirth.png" />
                                <asp:Panel ID="facePicturePanel" runat="server" style="height:100%;" >
                                    <table id="facePictureTable">
                                        <tr>
                                            <td style="vertical-align:middle; text-align: center;" >
                                                <icrop:CustomLabel ID="facePictureLabel" runat="server" TextWordNo="10157" Width="55px" UseEllipsis="true" CssClass="ellipsis" ></icrop:CustomLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="Customer">
					            <p id="P1" class="scNscCustomerName" runat="server"  onclick="CustomerEditPopUpOpen();">
                                    <icrop:CustomLabel ID="NameLabel" runat="server" Width="275px" Height="28px" UseEllipsis="True" CssClass="ellipsis" />
                                    <asp:HiddenField ID="nameHiddenField" runat="server" />

                                    <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 START--%>
                                    <asp:image runat="server" ID="VIP_Icon" CssClass="mainblockContentLeftCustomerNameIco01" ImageUrl="~/Styles/Images/SC3080201/customerVIP.png" Visible="false" />
                                    <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 END--%>
                                    <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
                                    <span class="mainblockContentLeftCustomerNameIco02">
                                        <icrop:CustomLabel ID="CustomerKind" runat="server" Width="21px" Height="23px" CssClass="styleCut" />
                                    </span>
                                    <span runat="server" id="CustomerTypeIcon" class="mainblockContentLeftCustomerNameIco03">
                                        <icrop:CustomLabel ID="CustomerType" runat="server" Width="21px" Height="23px" CssClass="styleCut" />
                                    </span>
                                    <%--2012/02/15 TCS 山口 【SALES_2】 END--%>
                                    <%-- 2018/06/27 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                    <asp:image runat="server" ID="JDP_Icon" Width="21px" Height="23px" CssClass="loyalCustomerIcon" ImageUrl="~/Styles/Images/SC3080201/L.png" Visible="False" />
                                    <%-- 2018/06/27 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>

                                    <%-- 2012/06/01 TCS 河原 FS開発 START --%>
                                    <%-- 2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 MOD START --%>
                                    <%-- 
                                    <div id="Icon_Renren" class="Sns_Icon"></div>
                                    <div id="Icon_Kaixin" class="Sns_Icon"></div>
                                    <div id="Icon_Weibo" class="Sns_Icon"></div>
                                    --%>
                                    <div id="Icon_Renren" runat="server" class="Sns_Icon"></div>
                                    <div id="Icon_Kaixin" runat="server" class="Sns_Icon"></div>
                                    <div id="Icon_Weibo" runat="server" class="Sns_Icon"></div>
                                    <%-- 2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 MOD END --%>

                                    <asp:HiddenField ID="SnsOpenFlg" runat="server" />
                                    <asp:HiddenField ID="SnsOpenMode" runat="server" />

                                    <asp:UpdatePanel ID="SnsIdInputPopupUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="SnsIdInputPopup" class="SnsIdInputPopup">
                                            	<div class="SnsIdInputPopupArrowOther" style=""></div>
                                            	<div class="SnsIdInputPopupWindownBox">
                                            		<div class="SnsIdInputPopupHeader">
                                            			<h3 class="clip">
                                                            <icrop:CustomLabel ID="Title_Renren" runat="server" TextWordNo="10189"  />
                                                            <icrop:CustomLabel ID="Title_Kaixin" runat="server" TextWordNo="10190"  />
                                                            <icrop:CustomLabel ID="Title_Weibo" runat="server" TextWordNo="10191"  />
                                            			</h3>
                                            			<a href="javascript:void(0)" class="PopUpCancelButton clip" onclick="SnsIdInputPopupClose();">
                                            				<icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="10125" />
                                            			</a>
                                                        <asp:LinkButton ID="SnsIdPopUpCompleteButton" runat="server" CssClass="PopUpCompleteButton"></asp:LinkButton>
                                            		</div>
                                                    <div class="SnsIdInputPopupListArea ellipsis">
                                                        <div id="SnsIdInputPopupInputTextWrap">
                                                            <icrop:CustomTextBox ID="SnsIdInputPopupInputText" runat="server" MaxLength="128" Width="287px" Height="26px" PlaceHolderWordNo="10192"/>
                                                        </div>
                                                    </div>
                                            	</div>
                                            </div>
                                        <asp:HiddenField ID="Snsid_Renren_Hidden" runat="server" />
                                        <asp:HiddenField ID="Snsid_Kaixin_Hidden" runat="server" />
                                        <asp:HiddenField ID="Snsid_Weibo_Hidden" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <%-- 2012/06/01 TCS 河原 FS開発 END --%>

                                    <br />
					                <span class="scNscCustomerNameId">
                                        <icrop:CustomLabel ID="DmsLabel" runat="server" Width="300px" UseEllipsis="True" CssClass="ellipsis" />                                    
                                    </span>
                                </p>
                                <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
                                <button type="button" id="CSSurveyButton" runat="server" class="CSSurveyBtn colorSetON" onclick="CSSurveyClick();" >
                                    <icrop:CustomLabel ID="CSSurveyLabelOn" runat="server" Width="80px" CssClass="styleCut" />
                                </button>
                                <button type="button" id="CSSurveyButtonOff" runat="server" class="CSSurveyBtn colorSetOFF" visible="false" >
                                    <icrop:CustomLabel ID="CSSurveyLabelOff" runat="server" Width="80px" CssClass="styleCut" />
                                </button>
                                <%--2012/02/15 TCS 山口 【SALES_2】 END--%>
                            </div>
					        <p class="clearboth"></p>



				        </div>
                            
                        <div>
					        <table border="0" class="scNscCustomerAddressArea NoBorderTable">
						        <tr>
							        <th class="aliginMiddle paddingTop4">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerAddressIcon1.png" width="16" height="16"/>
                                    </th>
							        <td class="aliginMiddle">
                                        <icrop:CustomLabel ID="MobileLabel" runat="server" Width="200px" UseEllipsis="True" CssClass="ellipsis" />
                                    </td>
							        <th rowspan="3" class="aliginTop">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerAddressIcon4.png" width="16" height="16"/>
                                    </th>
							        <td rowspan="3" class="scNscCustomerAddressBlueTxt customerAddress aliginTop">
                                        <p id="PAddress" runat="server" onclick="googleMapOpen();">
                                            <icrop:CustomLabel ID="ZIPLabel" runat="server" Width="125px" UseEllipsis="True" CssClass="ellipsis" /><br />
                                            <asp:TextBox ID="AddressLabel" ReadOnly="true" Width="200px" Height="38px" runat="server" TextMode="MultiLine" CssClass="scNscCustomerAddressStyle" />
                                        </p>
                                    </td>
						        </tr>
						        <tr>
							        <th class="aliginMiddle paddingTop4">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerAddressIcon2.png" width="16" height="16"/>
                                    </th>
							        <td class="aliginMiddle">
                                        <icrop:CustomLabel ID="TelLabel" runat="server" Width="200px" UseEllipsis="True" CssClass="ellipsis" />
                                    </td>
						        </tr>
						        <tr>
							        <th class="aliginMiddle paddingTop6">
                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerAddressIcon3.png" width="16" height="16"/>
                                    </th>
							        <td class="scNscCustomerAddressBlueTxt aliginMiddle">
                                        <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                        <asp:HyperLink ID="EmailLink" runat="server"><icrop:CustomLabel ID="EmailLabel" runat="server" Width="200px" UseEllipsis="True" CssClass="scNscCustomerAddressBlueTxt ellipsis" /></asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" class="scNscCustomerAddressArea NoBorderTable">
                                <tr>
                                    <td class="aliginMiddle">
                                        <%-- Customer Category --%><icrop:CustomLabel runat="server" TextWordNo="40010"/>
                                    </td>
                                    <td class="scNscCustomerAddressBlueTxt aliginMiddle">
                                        <icrop:CustomLabel ID="custInfoCustCtgryLabel" runat="server" Width="200px" UseEllipsis="true" CssClass="ellipsis" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="aliginMiddle">
                                        <%-- Customer Subcategory 1 --%><icrop:CustomLabel runat="server" TextWordNo="40074"/>
                                    </td>
                                    <td class="scNscCustomerAddressBlueTxt aliginMiddle">
                                        <icrop:CustomLabel ID="custInfoCustSubCtgry1Label" runat="server" Width="200px" UseEllipsis="true" CssClass="ellipsis" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="aliginMiddle">
                                        Organization Name
                                    </td>
                                    <td class="scNscCustomerAddressBlueTxt aliginMiddle">
                                        <icrop:CustomLabel ID="custInfoCustOrgnzNameLabel" runat="server" Width="200px" UseEllipsis="true" CssClass="ellipsis" />
                                    </td>
                                </tr>
                            </table>
                            <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
                        </div>
                        <hr />
                    </asp:Panel>
                    <div class="Customer" onclick="CustomerEditPopUpOpen();">
                        <asp:Panel ID="NewCustomerNamePanel" runat="server" Visible="false" class="scNscCustomerName" >
                            <div class="scNscCustomerNameAreaNotSelected">
					            <div class="NotSelectImage">
                                    <asp:Image ID="Image20" runat="server" ImageUrl="~/Styles/Images/SC3080201/nsc414NoUserDatas.png" width="457" height="160" />
                                    <h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral109" runat="server" TextWordNo="10109" /></h4>
                                </div>
				            </div>
                        </asp:Panel>
                    </div> 
                    <div style="display:none;" >
                        <asp:Button ID="customerInfoUpdateButton" runat="server" Text="" />
                        <asp:TextBox ID="customerAddressTextBox" runat="server" type="hidden" />
                        <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                        <icrop:DateTimeSelector ID="customerBirthday" runat="server" />
                        <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                    </div>
                    <asp:Button ID="customerReload" runat="server" style="display:none" />
                    <asp:HiddenField ID="editModeHidden" runat="server" />                
                    <asp:HiddenField ID="customerIdTextBox" runat="server" />
                    <asp:HiddenField ID="uploadPathTextBox" runat="server" />
                    <%--<asp:HiddenField ID="faceFileNameTimeHiddenField" runat="server" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>            
            <button type="button" id="customerPopUpOpen" class="CustomerPopUpOpen" onclick="CustomerEditPopUpOpen();" ></button>
			<asp:Button ID="customerReloadAll" runat="server" style="display:none" />
            <asp:HiddenField ID="customerEditPopUpAutoOpenFlg" runat="server" />
            
            <!-- 保有車種情報 -->

			<asp:UpdatePanel id="customerCarUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
            		<asp:Button ID="customerCarReload" runat="server" style="display:none" />
                    <div style="height: 129px;">
			            <asp:Panel ID="EditCustomerCarTypePanel" runat="server" Visible="true">                
                            <asp:HiddenField ID="selectKey" runat="server" Value="" />
					        <div class="scNscCustomerCarTypeArea" >
						        <h4>
                                    <icrop:CustomLabel ID="WordLiteral102" runat="server" Width="380" CssClass="styleCut" TextWordNo="10102" />
                                </h4>
						        <div class="scNscCustomerCarTypeNumber" id="CustomerCarTypeNumber" runat="server" onclick="CustomerCarSelectPopUpOpen();">
                                    <icrop:CustomLabel runat="server" ID="CustomerCarTypeNumberLabel"  BorderStyle="NotSet" />
                                </div>
						        <p class="clearboth"></p>

                                
                                    
                                <div id="CustomerCarEditPopUpOpenEria1" runat="server" class="CustomerCarEdit" onclick="CustomerCarEditPopUpOpen();">
						            <div class="scNscCustomerCarTypeLeftArea">
							            <div id="carTypeLogoLbl" runat="server" visible="true" class="CarType">
								            <table border="0" class="NoBorderTable">
								                <tr>
								                    <td width="63" height="24"><icrop:CustomLabel ID="customerCarSeriesCdLabel" runat="server" Width="63px" UseEllipsis="true" CssClass="ellipsis" /></td>
								                    <td class="CarTypeBoldText"><icrop:CustomLabel ID="customerCarSeriesNmLabel" runat="server" Width="140px" UseEllipsis="true" CssClass="ellipsis" /></td>
							                    </tr>
							                </table>
						                </div>
							            <div id="carTypeLogoImg" runat="server" visible="false" >
                                            <asp:Image ID="CarTypeLogo" runat="server" ImageUrl="" width="185" height="30" />
                                        </div>
							            <table border="0" class="scNscCustomerCarTypeData1">
								            <tr>
								                <th class="scNscCustomerCarTypeData1Th">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerCarTypeIcon1.png" width="16" height="16" />
                                                </th>
								                <td class="scNscCustomerCarTypeData1Td1">
                                                    <icrop:CustomLabel ID="CustomerCarGradeLabel" runat="server" Width="200px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								            </tr>
								            <tr>
								                <th class="scNscCustomerCarTypeData1Th">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Styles/Images/SC3080201/scNscCustomerCarTypeIcon2.png" width="16" height="16" />
                                                </th>
								                <td class="scNscCustomerCarTypeData1Td2">
                                                    <icrop:CustomLabel ID="CustomerCarsBdyclrnmLabel" runat="server" Width="200px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								            </tr>
                                           <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                            <tr>
                                                <td colspan="2">
                                                    <table class="scNscCustomerCarTypeData2">
                                                        <tr>
            								                <th>
                                                                <icrop:CustomLabel ID="WordLiteral2020004" runat="server" Width="60px" TextWordNo="2020004" />
                                                            </th>
						            		                <td>
                                                                <icrop:CustomLabel ID="CustomerCarsModelYearLabel" runat="server" CssClass="ellipsis" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                           <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1   END --%>
							            </table>
						            </div>
						            <div class="scNscCustomerCarTypeRightArea">
							            <table border="0" class="scNscCustomerCarTypeData2">
								            <tr>
								                <th>
                                                   <icrop:CustomLabel ID="WordLiteral116" runat="server" Width="30px" TextWordNo="10116" />
                                                </th>
								                <td colspan="2">
                                                    <icrop:CustomLabel ID="CustomerCarsRegLabel" runat="server" Width="180px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								            </tr>
								            <tr>
								                <th>
                                                    <icrop:CustomLabel ID="WordLiteral117" runat="server" Width="30px" TextWordNo="10117" />
                                                </th>
								                <td colspan="2">
                                                    <icrop:CustomLabel ID="CustomerCarsVINLabel" runat="server" Width="180px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								            </tr>
								            <tr>
								                <th>
                                                    <icrop:CustomLabel ID="WordLiteral118" runat="server" Width="30px" TextWordNo="10118" />
                                                </th>
								                <td colspan="2">
                                                    <icrop:CustomLabel ID="CustomerCarsVCLDateLabel" runat="server" CssClass="ellipsis" />
                                                </td>
								            </tr>
								            <tr>
								                <th>
                                                    <icrop:CustomLabel ID="WordLiteral119" runat="server" Width="30px" TextWordNo="10119" />
                                                </th>
								                <td class="kmBox">
                                                    <icrop:CustomLabel ID="CustomerCarsKmLabel" runat="server" Width="55px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								                <td class="Renewal kmBox0">
                                                    <icrop:CustomLabel ID="CustomerCarsDateLabel" runat="server" Width="120px" UseEllipsis="True" CssClass="ellipsis" />
                                                </td>
								            </tr>
                                            <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
								            <tr>
								                <th colspan="2">
                                                    <icrop:CustomLabel ID="WordLiteral2020005" runat="server" Width="100%" TextWordNo="2020005" />
                                                </th>
								                <td>
                                                    <icrop:CustomLabel ID="CustomerCarsDistanceCoveredLabel" runat="server" CssClass="ellipsis" />
                                                </td>
								            </tr>
                                            <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1   END --%>
							            </table>
						            </div>
                                    <p class="clearboth"></p>
                                </div> 
					        </div>
                        </asp:Panel>
                        <div id="CustomerCarEditPopUpOpenEria2" runat="server" class="CustomerCarEdit" onclick="CustomerCarEditPopUpOpen();">
                            <asp:Panel ID="NewCustomerCarTypePanel" runat="server" Visible="false" >                        
                                <div id="popupVehicleTrigger" >
                                    <div class="scNscCustomerCarTypeAreaNotSelected">
						                <div class="NotSelectImage">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/Images/SC3080201/nsc414NoUserCarDatas.png" width="457" height="111" />
                                            <h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral110" runat="server" TextWordNo="10110" /></h4>
                                        </div>
					                </div>
                                </div>
                            </asp:Panel>
                        </div> 
                        </div>
                        <!-- 担当情報 -->
                        <div class="scNscCustomerCarTypeArea ">
                        <table border="0" class="scNscCustomerCarTypeData3">
					        <tr>
						        <th>
                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Styles/Images/Authority/SC.png" width="14" height="18" />
                                </th>
						        <td><icrop:CustomLabel ID="CustomerCarSCNameLabel" runat="server" Width="180px" UseEllipsis="True" CssClass="ellipsis" />
                                </td>
						        <th>
                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/Styles/Images/Authority/SA.png" width="14" height="18" />                           
                                </th>
						        <td><icrop:CustomLabel ID="CustomerCarSANameLabel" runat="server" Width="180px" UseEllipsis="True" CssClass="ellipsis" />
                                </td>
					        </tr>
				        </table>
                        </div>    
                        <asp:HiddenField ID="selectVinHidden" runat="server" />
                        <asp:HiddenField ID="selectSeqnoHidden" runat="server" />
                        <asp:HiddenField ID="editVehicleModeHidden" runat="server" />
                    <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                    <asp:Button runat="server" ID="CustomerCarSelectPopupOpenButton" style="display:none" />
                    <asp:HiddenField runat="server" ID="customerCarsSelectedHiddenField" />
                    <div id="scNscSelectionWindownVehicleSelect" style="display:none; z-index: 10;">
			            <div id="scNscSelectionWindownBoxVehicleSelect" runat="server" >
				                <div class="scNscSelectionListArea">
                            <asp:Panel runat="server" ID="CustomerCarVisiblePanel" Visible="false" >
					                <div class="scNscSelectionListBox">
                                        <asp:Button ID="customerCarButtonDummy" runat="server" style="display:none" />
                                        <table id="SelectCarType" border="0" cellspacing="0" cellpadding="0" style="width: 100%">
                                            <asp:Repeater ID="CarTypeRepeater" runat="server" EnableViewState="False">
                                                <ItemTemplate>
                                                    <tbody>
                                                        <tr><td>                                                        
                                                        <asp:panel id="SelectCarTypePanel" runat="server" >
                                                            <div id="Div1" class="CarTypeSelect" runat="server" index='<%#Eval("INDEX")%>' >
                                                                <div id="carTypeDivMain" runat="server"  class='<%#Eval("CARTYPESELECTION")%>' >
							                                        <div class="scNscSelectionCassetteLeft">
                                                                        <div id="carTypeLogoLbl" runat="server" visible='<%#Eval("SHOWLABEL")%>'>
								                                            <table runat="server" id="customerCarSeriesTable" border="0" cellspacing="0" cellpadding="0" class='<%#Eval("CARTYPESELECTIONSTYLET")%>'>
								                                                <tr>
								                                                    <td width="63" height="24">
                                                                                        <icrop:CustomLabel ID="customerCarSeriesCdLabel" runat="server" Width="80px" UseEllipsis="true" CssClass="ellipsis" Text='<%#Eval("SERIESCD")%>' />
                                                                                    </td>
								                                                    <td id="customerCarSeriesNmTd" class='<%#Eval("CARTYPESELECTIONSTYLETD")%>'>
                                                                                        <icrop:CustomLabel ID="customerCarSeriesNmLabel" runat="server" Width="135px" UseEllipsis="true" CssClass="ellipsis" text='<%#Eval("SERIESNM")%>' />
                                                                                    </td>
							                                                    </tr>
							                                                </table>
						                                                </div>
                                                                        <div id="carTypeLogoImg" runat="server" visible='<%# Eval("SHOWLOGO")%>' >
                                                                            <asp:Image ID="carTypeLogoP" runat="server" width="185" height="30" CssClass="carTypeLogoP" />
                                                                        </div>
                                                                        <div id="customerCarGradeDiv" runat="server" class='<%#Eval("CARTYPESELECTIONSTYLED1")%>'>
                                                                            <icrop:CustomLabel ID="customerCarGradeLabelP" runat="server" Width="190px" UseEllipsis="True" Text='<%#Eval("GRADE")%>' CssClass="customerCarGradeLabelP ellipsis" />
                                                                        </div>
                                                                        <div id="customerCarsBdyclrnmDiv" runat="server" class='<%#Eval("CARTYPESELECTIONSTYLED2")%>'>
                                                                            <icrop:CustomLabel ID="customerCarsBdyclrnmLabelP" runat="server" Width="190px" UseEllipsis="True" Text='<%#Eval("BDYCLRNM")%>' CssClass="customerCarsBdyclrnmLabelP ellipsis" />
                                                                        </div>
                                                                        <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                                                        <div id="customerCarsModelYearDiv" runat="server">
                                                                            <table id="customerCarsModelYearTable" runat="server" border="0" cellspacing="0" cellpadding="0" class='<%#Eval("CARTYPESELECTIONITEM")%>'>
                                                                                <tr>
                                                                                    <th style="width:60px;"><p style="width:60px;"><icrop:CustomLabel ID="WordLiteral2020004_P" runat="server" Width="60px" TextWordNo="2020004" /></p></th>
                                                                                    <td colspan="2">
                                                                                        <icrop:CustomLabel ID="customerCarsModelYearLabelP" runat="server" Width="130px" UseEllipsis="True" Text='<%#Eval("MODEL_YEAR")%>' CssClass="customerCarsModelYearLabelP ellipsis" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>
								                                    </div>
							                                        <div class="scNscSelectionCassetteRight">
								                                        <table id="customerCarsRightTable" runat="server" border="0" cellspacing="0" cellpadding="0" class='<%#Eval("CARTYPESELECTIONITEM")%>'>
									                                        <tr>
										                                        <th><p><icrop:CustomLabel ID="WordLiteral116_P" runat="server" Width="30px" TextWordNo="10116" /></p></th>
										                                        <td colspan="2">
                                                                                    <icrop:CustomLabel ID="customerCarsRegLabelP" runat="server" Width="180px" UseEllipsis="True" Text='<%#Eval("VCLREGNO")%>' CssClass="customerCarsRegLabelP ellipsis" />
                                                                                </td>
										                                        </tr>
									                                        <tr>
										                                        <th><p><icrop:CustomLabel ID="WordLiteral117_P" runat="server" Width="30px" TextWordNo="10117" /></p></th>
										                                        <td colspan="2">
                                                                                    <icrop:CustomLabel ID="customerCarsVINLabelP" runat="server" Width="180px" UseEllipsis="True" Text='<%#Eval("VIN")%>' CssClass="customerCarsVINLabelP ellipsis" />
                                                                                </td>
										                                        </tr>
									                                        <tr>
										                                        <th><p><icrop:CustomLabel ID="WordLiteral118_P" runat="server" Width="30px" TextWordNo="10118" /></p></th>
										                                        <td colspan="2">
                                                                                    <icrop:CustomLabel ID="customerCarsVCLDateLabelP" runat="server" Text='<%#Eval("VCLDELIDATESTRING")%>' CssClass="customerCarsVCLDateLabelP ellipsis" />
                                                                                </td>
										                                        </tr>
									                                        <tr style="vertical-align: top">
										                                        <th><p><icrop:CustomLabel ID="WordLiteral119_P" runat="server" Width="30px" TextWordNo="10119" /></p></th>
										                                        <td class="kmBox">
                                                                                    <icrop:CustomLabel ID="customerCarsKmLabelP" runat="server" Width="55px" UseEllipsis="True" Text='<%#Eval("MILEAGE")%>' CssClass="customerCarsKmLabelP ellipsis" />
                                                                                </td>
										                                        <td class="Renewal kmBox0">
                                                                                    <icrop:CustomLabel ID="customerCarsDateLabelP" runat="server" Width="120px" UseEllipsis="True" Text='<%#Eval("UPDATEDATESTRING")%>' CssClass="customerCarsDateLabelP ellipsis" />
                                                                                </td>
										                                    </tr>
                                                                            <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
									                                        <tr>
										                                        <th colspan="2"><p style="width:100%;"><icrop:CustomLabel ID="WordLiteral2020005_P" runat="server" Width="100%" TextWordNo="2020005" /></p></th>
										                                        <td>
                                                                                    <icrop:CustomLabel ID="customerCarsDistanceCoveredLabelP" runat="server" Width="120px" UseEllipsis="True" Text='<%#Eval("VCL_MILE")%>' CssClass="customerCarsDistanceCoveredLabelP ellipsis" />
                                                                                </td>
										                                    </tr>
                                                                            <%-- 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>
									                                    </table>
						                                            </div>
							                                        <div class="clearboth">&nbsp;</div></div></div></asp:panel><asp:HiddenField runat="server" ID="customerCarKey" Value='<%#Eval("KEY")%>' />
                                                        <asp:HiddenField ID="logoNotSelectid" runat="server" Value='<%#Eval("LOGO_NOTSELECTED")%>' />
                                                        <asp:HiddenField ID="logoSelectid" runat="server" Value='<%#Eval("LOGO_SELECTED")%>' />
                                                        </td></tr>
                                                    </tbody>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                            </asp:Panel>
                                </div>
                        </div>
                    </div>
                    <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--</icrop:PopOver>--%>
                <!-- 車両編集 -->
			<hr />
		
            <!-- 顧客関連情報 -->		
			<div id="CustomerRelatedArea">
				<h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral103" runat="server" Width="320px" TextWordNo="10103" /></h4>

                <%--2012/06/01 TCS 河原 FS開発 START--%>
                <asp:HiddenField ID="KeywordSearchOpenFlg" runat="server" />

                <%-- 2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 MOD START --%>
                <%-- 
                <div id="KeywordSearch" class="KeywordSearch buttonOn">
                --%>
                <div id="KeywordSearch" runat="server" class="KeywordSearch buttonOn">
                <%-- 2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 MOD END --%>
                <icrop:CustomLabel ID="CustomLabel10193" runat="server"　TextWordNo="10193" />
                </div>

                <asp:UpdatePanel ID="KeywordSearchInputPopupUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="KeywordSearchInputPopup" class="KeywordSearchInputPopup">
                        	<div class="KeywordSearchInputPopupArrowOther" style=""></div>
                        	<div class="KeywordSearchInputPopupWindownBox">
                        		<div class="KeywordSearchInputPopupHeader">
                        			<h3 class="clip">
                        				<icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="10194" />
                        			</h3>
                        			<a href="javascript:void(0)" class="PopUpCancelButton clip" onclick="KeywordSearchInputPopupClose();">
                        				<icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="10125" />
                        			</a>
                                    <asp:LinkButton ID="KeywordSearchPopUpCompleteButton" runat="server" CssClass="PopUpCompleteButton"></asp:LinkButton>
                                    </div>
                                    <div class="KeywordSearchInputPopupListArea ellipsis">
                                    <div id="KeywordSearchInputPopupInputTextWrap">
                                        <icrop:CustomTextBox ID="KeywordSearchInputPopupInputText" runat="server" MaxLength="256" Width="562px" Height="26px" PlaceHolderWordNo="10195" />
                                    </div>
                                </div>
                        	</div>
                        </div>
                        <asp:HiddenField ID="Keyword_Hidden" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>


                <%--2012/06/01 TCS 河原 FS開発 END--%>

                <%--　＊＊＊＊＊＊＊＊＊＊顧客職業＊＊＊＊＊＊＊＊＊＊--%>
                <table border="0" class="NoBorderTable">
                    <tr>
                        <td style="width: 115px">
                            <asp:UpdatePanel ID="CustomerRelatedOccupationUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="CustomerRelatedOccupationArea" runat="server" onclick="setPopupOccupationPageOpen();">
                            	        <asp:Panel ID="CustomerRelatedOccupationSelectedPanel" runat="server">
                                            <asp:Panel id="CustomerRelatedOccupationSelectedImage" runat="server">
                                                <div class="OccupationCount">&nbsp;</div><div class="OccupationText CustomerRelatedTitleFont">
                                                    <icrop:CustomLabel ID="CustomerRelatedOccupationSelectedLabel" CssClass="ellipsis" runat="server" Width="95" UseEllipsis="true" />
                                                </div>
                                            </asp:Panel>
					                    </asp:Panel>
                                        <asp:Panel ID="CustomerRelatedOccupationNewPanel" runat="server" style="height:100%;width:100%;">
                                            <table style="height:100%;width:100%;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:center;">
                                                        <icrop:CustomLabel ID="WordLiteral111_1" runat="server" Width="98" UseEllipsis="true" TextWordNo="10111" CssClass="CustomerRelatedTitleFont ellipsis" />
                                                    <td>
                                                </tr>
                                            </table>
					                    </asp:Panel>
                                    </div>
                                    <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                                    <asp:Button runat="server" ID="OccupationOpenButton" style="display:none;" />
                                    <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 START--%>
                                    <asp:Panel ID="CustomerRelatedOccupationPopupArea" runat="server" style="display:none;" Height="555px" CssClass="7Rows">
	                                    <div class="popUpHeader">
                                            <div class="btnL" style="display:none;">                                        
                                                <div>
                                                    <a onclick="setPopupOccupationPage('page1');" class="styleCut" ><icrop:CustomLabel iD="CustomerRelatedOccupationCancelLabel" runat="server" TextWordNo="10125"></icrop:CustomLabel></a><%--<asp:button ID="CustomerRelatedOccupationCancelButton" runat="server" style="display:none" />--%></div></div><h3 class="popUpTitle" >
                                                <icrop:CustomLabel ID="CustomerRelatedOccupationTitleLabel" runat="server" Width="195px" class="styleCut" TextWordNo="10122"></icrop:CustomLabel></h3><div class="btnR" style="display:none;">
                                                <div>
                                                    <asp:LinkButton ID="CustomerRelatedOccupationRegistButton" runat="server" CssClass="styleCut" OnClientClick="return checkOtherOccupation();"></asp:LinkButton></div></div></div><asp:Panel runat="server" ID="OccupationPopopBody" CssClass="popUpBG" Height="555px" >
                                            <div class="popUpArea" style="overflow:hidden;">
                                            <asp:Panel runat="server" ID="OccupationVisiblePanel" Visible="false" >
                                                <div id="CustomerRelatedOccupationPageArea" >
                                                    <asp:Panel ID="occupationPopOverForm_1" runat="server" style="width:365px;float:left;" Height="555px">
                                                        <asp:Repeater ID="CustomerRelatedOccupationButtonRepeater" runat="server">
                                                            <ItemTemplate>
                                                                <asp:Panel ID="CustomerRelatedOccupationPanel" runat="server" CssClass="popUpIcon">
                                                                    <asp:LinkButton ID="CustomerRelatedOccupationHyperLink" runat="server">
                                                                        <icrop:CustomLabel ID="CustomerRelatedOccupationText" runat="server" Width="72px" UseEllipsis="True" ClientIDMode="Predictable" CssClass="popupIconTextCenter ellipsis"></icrop:CustomLabel>
                                                                    </asp:LinkButton><asp:HiddenField ID="CustomerRelatedOccupationSelectedHiddenField" runat="server" />
                                                                    <asp:HiddenField ID="CustomerRelatedOccupationIdHiddenField" runat="server" />
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <p class="popUpIconClear"></p>
                                                    </asp:Panel>
                                                    <asp:Panel ID="occupationPopOverForm_2" runat="server" style="width:370px;float:left;margin-left:5px;">
			                                            <div class="occupationOtherRelationship" >
                                                            <icrop:CustomTextBox ID="CustomerRelatedOccupationOtherCustomTextBox" runat="server" CssClass="TextArea" PlaceHolderWordNo="10124" Width="338"  MaxLength="30" TabIndex="1001"></icrop:CustomTextBox><asp:HiddenField ID="CustomerRelatedOccupationOtherIdHiddenField" runat="server" />
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
		                                    </div>
                                        </asp:Panel>
	                                    <div class="popUpFooterJob"></div>
                                    </asp:Panel>
                                    <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 END--%>
                                    <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                                    <asp:HiddenField ID="OccupationPopuupTitlePage1" runat="server" />
                                    <asp:HiddenField ID="OccupationPopuupTitlePage2" runat="server" />
                                    <asp:HiddenField ID="OccupationOtherErrMsg" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <%--　＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊--%>
                        <%--　＊＊＊＊＊＊＊＊＊＊家族構成＊＊＊＊＊＊＊＊＊＊--%>	
                        <td style="width: 115px">
                        <asp:UpdatePanel ID="CustomerRelatedFamilyUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="CustomerRelatedFamilyArea" runat="server" onclick="setPopupFamilyPageOpen();">
                                <asp:Panel ID="CustomerRelatedFaimySelectedEditPanel" runat="server">
                                    <asp:Panel id="CustomerRelatedFamilySelectedImage" runat="server">
                                        <div class="FamilyCount">
                                            <asp:Label ID="FamilyCountLabel" runat="server"></asp:Label></div><div class="FamilyText CustomerRelatedTitleFont">
                                            <icrop:CustomLabel ID="CustomerRelatedFamilyTitleLabel" runat="server" TextWordNo="10121" Width="95" UseEllipsis="true" CssClass="ellipsis" />
                                        </div>
                                    </asp:Panel>
                                </asp:Panel> 
                                <asp:Panel ID="CustomerRelatedFamilySelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                    <table style="height:100%;width:100%;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:center;">
                                                <icrop:CustomLabel ID="WordLiteral112" runat="server" Width="98" UseEllipsis="true" TextWordNo="10112" class="CustomerRelatedTitleFont ellipsis" />
                                            <td>
                                        </tr>
                                    </table>
					            </asp:Panel>
                            </div>
                            <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                            <asp:Button runat="server" ID="FamilyOpenButton" style="display:none" />
                            <div id="CustomerRelatedFamilyPopupArea" style="display:none; top: 10px;">
                                <div class="popUpHeaderFamily">
                                    <div class="btnL">
                                        <div>
                                            <a onclick="CancelCustomerRelatedFamily()" class="styleCut"><icrop:CustomLabel ID="CustomerRelatedFamilyCancelLabel" runat="server" TextWordNo="10125" /></a>
                                            <%--<asp:button ID="CustomerRelatedFamilyCancelButton" runat="server" style="display:none" />--%>
                                        </div> 
                                    </div>
                                    <h3 class="popUpTitle" style="margin-left: 3px;">
                                        <icrop:CustomLabel ID="CustomerRelatedFamilyPopUpTitleLabel" runat="server" CssClass="styleCut" Width="148px" TextWordNo="10147" />
                                    </h3>
                                    <div class="btnR">
                                        <div>
                                            <asp:LinkButton ID="CustomerRelatedFamilyRegistButton" CssClass="styleCut" runat="server" OnClientClick="return RegistCustomerRelatedFamily();"></asp:LinkButton></div></div></div><div class="popUpBGFamily">
                                    <asp:Panel runat="server" ID="FamilyVisiblePanel" Visible="false" >
                                        <div class="FamilypopUpArea" style="width:320px;height:325px;overflow:hidden;">
                                            <div id="CustomerRelatedFamilyPageArea" >
                                                <asp:Panel ID="CustomerRelatedFamilyPage1" runat="server" style="width:320px;height:325px;float:left;" >
                                                    <div id="FamilyListWrap" class="familyAreaScroll popupScrollArea">			
                                                        <div class="familyCountTitle">
                                                            <h4><icrop:CustomLabel ID="FamilyNumberWordLabel" runat="server" Width="300px" UseEllipsis="true" TextWordNo="10148" CssClass="ellipsis" /></h4>
                                                        </div>
                                                        <div id="FamilyCountBox" class="familyCountBox">
                                                            <ul>
				                                                <li onclick="SelectFamilyCount(0);"><a id="FamilyCount1" runat="server" >1</a></li><li onclick="SelectFamilyCount(1);"><a id="FamilyCount2" runat="server" >2</a></li><li onclick="SelectFamilyCount(2);"><a id="FamilyCount3" runat="server" >3</a></li><li onclick="SelectFamilyCount(3);"><a id="FamilyCount4" runat="server" >4</a></li><li onclick="SelectFamilyCount(4);"><a id="FamilyCount5" runat="server" >5</a></li></ul><ul style="margin-top:10px;">
                                                                <li onclick="SelectFamilyCount(5);"><a id="FamilyCount6" runat="server" >6</a></li><li onclick="SelectFamilyCount(6);"><a id="FamilyCount7" runat="server" >7</a></li><li onclick="SelectFamilyCount(7);"><a id="FamilyCount8" runat="server" >8</a></li><li onclick="SelectFamilyCount(8);"><a id="FamilyCount9" runat="server" >9</a></li><li onclick="SelectFamilyCount(9);"><a id="FamilyCount10" runat="server" >10</a></li></ul><div id="TriangulArrowDown" class="TriangulArrowDown" onclick="transitionFamilyCountBox(true);"></div>
                                                            <div id="TriangulArrowUp" class="TriangulArrowUp" onclick="transitionFamilyCountBox(false);"></div>
                                                        </div>
                                                        <div class="familyBirthdayTitle">
                                                            <h4 class="TitleLeft"><icrop:CustomLabel ID="FamilyOrganizationWordLabel" runat="server" TextWordNo="10149" Width="95px" UseEllipsis="true" CssClass="ellipsis" /></h4>
                                                            <%--2013/12/25 TCS 市川 Aカード情報相互連携開発 追加要望 START--%>
                                                            <h4 class="TitleCenter"><icrop:CustomLabel ID="FamilyBirthdayWordLabel" runat="server" TextWordNo="10152" Width="100%" UseEllipsis="true" CssClass="ellipsis styleCut" /></h4>
                                                            <%--2013/12/25 TCS 市川 Aカード情報相互連携開発 追加要望 END--%>
                                                            <div class="clearboth">&nbsp;</div></div><div id="familyBirthdayListArea" class="familyBirthdayListArea">
                                                            <ul>
                                                                <asp:Repeater ID="familyBirthdayList" runat="server">
                                                                    <ItemTemplate>
                                                                        <li ID="familyBirthdayList_Row" runat="server" ClientIDMode="Predictable">
                                                                            <icrop:CustomLabel ID="familyBirthdayListRelationLabel_Row" runat="server" CssClass="type styleCut" ClientIDMode="Predictable" />
<%' 2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START %>
                                                                    <icrop:DateTimeSelector ID="familyBirthdayListBirthdayDate_Row" runat="server" CssClass="Calendar" ClientIDMode="Predictable" Format="Date" PlaceHolderWordNo="10152" height="16px" />
<%' 2013/10/02 TCS 藤井 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END %>
                                                                           <asp:HiddenField ID="familyBirthdayHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                            <asp:HiddenField ID="familyBirthdayListRelationNoHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                            <asp:HiddenField ID="familyBirthdayListFamilyNoHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                            <asp:HiddenField ID="familyBirthdayListRelationOtherHidden_Row" runat="server" ClientIDMode="Predictable"/>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <asp:panel ID="CustomerRelatedFamilyPage2" runat="server" style="width:320px;height:325px;float:left;">
                                                    <div id="FamilyRelationshipWrap" class="familyAreaScroll popupScrollArea">
                                                        <div id="familyRelationship" class="familyRelationship">
                                                            <ul>
                                                                <asp:Repeater ID="FamilyRelationshipRepeater" runat="server" EnableViewState="False">
                                                                    <ItemTemplate>
                                                                        <li id="familyRelationshipList_No" runat="server" >
                                                                            <icrop:CustomLabel ID="familyRelationshipLabel_No" runat="server" Width="250px" CssClass="ellipsis" UseEllipsis="true"/>
                                                                            <asp:HiddenField ID="familyRelationshipNoHidden_No" runat="server"/>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </asp:panel>
                                                <asp:panel ID="CustomerRelatedFamilyPage3" runat="server" style="width:320px;height:325px;float:left;">
                                                    <div class="familyAreaScroll">
                                                        <div class="familyOtherRelationship" >
                                                            <icrop:CustomTextBox ID="familyOtherRelationshipTextBox" runat="server" CssClass="TextArea" PlaceHolderWordNo="10153" Width="285" MaxLength="30" TabIndex="1002" />
                                                        </div>
                                                    </div>
                                                </asp:panel>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="popUpFooterFamily"></div>
                            </div>
                            <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                            <asp:HiddenField ID="RelationOtherWordHidden" runat="server"/>
                            <asp:HiddenField ID="RelationOtherNoHidden" runat="server"/>
                            <asp:HiddenField ID="FamilyCount" runat="server"/>
                            <asp:HiddenField ID="FamilyPopuupTitlePage1" runat="server"/>
                            <asp:HiddenField ID="FamilyPopuupTitlePage2" runat="server"/>
                            <asp:HiddenField ID="FamilyPopuupTitlePage3" runat="server"/>
                            <asp:HiddenField ID="RelationOtherErrMsgHidden" runat="server" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <%--　＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊--%>
                    <%--　＊＊＊＊＊＊＊＊＊＊顧客趣味＊＊＊＊＊＊＊＊＊＊--%>
                    <td style="width: 115px">
                        <asp:UpdatePanel ID="CustomerRelatedHobbyUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <div id="CustomerRelatedHobbyArea" runat="server" onclick="setPopupHobbyPageOpen();">
                            <asp:Panel ID="CustomerRelatedHobbySelectedEditPanel" runat="server">
                                <asp:Panel id="CustomerRelatedHobbySelectedImage" runat="server">
                                    <div class="HobbyCount">
                                        <asp:Label ID="HobbyCountLabel" runat="server"></asp:Label></div><div class="HobbyText CustomerRelatedTitleFont">
                                        <icrop:CustomLabel ID="CustomerRelatedHobbySelectedLabel" runat="server" Width="95" UseEllipsis="true" CssClass="ellipsis" />
                                    </div>
                                </asp:Panel>
					        </asp:Panel>
                            <asp:Panel ID="CustomerRelatedHobbySelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                <table style="height:100%;width:100%;">
                                    <tr>
                                        <td style="vertical-align:middle;text-align:center;">
                                            <icrop:CustomLabel ID="WordLiteral113" runat="server" Width="98" UseEllipsis="true" TextWordNo="10113" CssClass="CustomerRelatedTitleFont ellipsis" />
                                        <td>
                                    </tr>
                                </table>
					        </asp:Panel>
                        </div>
                        <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                        <asp:Button runat="server" ID="HobbyOpenButton" style="display:none" />
                        <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 START--%>
                        <asp:Panel runat="server" ID="CustomerRelatedHobbyPopupArea" style="display:none;" CssClass="7Rows" >
	                        <div class="popUpHeader">
                            	<div class="btnL">
                                    <div>
                                        <a onclick="cancelCustomerRelatedHobby()" class="styleCut"><icrop:CustomLabel ID="CustomerRelatedHobbyPopupCancelLabel" runat="server" TextWordNo="10125" /></a>
                                        <asp:button ID="CustomerRelatedHobbyPopupCancelButton" runat="server" style="display:none" />
                                    </div> 
                                </div>
                                <h3 class="popUpTitle" style="margin-left: 3px;">
                                    <icrop:CustomLabel ID="CustomerRelatedHobbyPopupTitleLabel" runat="server" Width="195px" CssClass="styleCut" TextWordNo="10127"></icrop:CustomLabel></h3><div class="btnR">
                                    <div>
                                        <asp:LinkButton ID="registCustomerRelatedHobbyButton" runat="server" CssClass="styleCut" OnClientClick="return registCustomerRelatedHobby();"></asp:LinkButton></div></div></div><asp:Panel runat="server" ID="HobbyPopupBody" CssClass="HobbyPopupBG4columns" Height="555px" >
                            	<div class="HobbypopUpArea4columns" style="overflow:hidden;">
                            	 <asp:Panel runat="server" ID="HobbyVisiblePanel" Visible="false">
                                    <div id="CustomerRelatedHobbyPopupPageArea" >
                                    	<asp:panel ID="CustomerRelatedHobbyPopupPage1" runat="server" CssClass="CustomerRelatedHobbyPopupPage" Height="555px" >
                                        	<asp:repeater id="CustomerRelatedHobbyPopupSelectButtonRepeater" runat="server">
                                            	<ItemTemplate>
                                                	<asp:Panel ID="CustomerRelatedHobbyPopupSelectButtonPanel_Row" runat="server" class="hobbyIcon" ViewStateMode="Enabled" ClientIDMode="Predictable">
                                                    	<icrop:CustomLabel ID="CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row" runat="server" Width="72px" class="ellipsis" ClientIDMode="Predictable" UseEllipsis="True" />
                                                        <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonOther_Row" runat="server" ClientIDMode="Predictable" />
                                                        <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row" runat="server" ClientIDMode="Predictable" />
                                                        <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectButtonCheck_Row" runat="server" ClientIDMode="Predictable" />
                                                        <asp:HiddenField ID="CustomerRelatedHobbyPopupSelectedButtonPath_Row" runat="server" ClientIDMode="Predictable" />
                                                        <asp:HiddenField ID="CustomerRelatedHobbyPopupNotSelectedButtonPath_Row" runat="server" ClientIDMode="Predictable" />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                             </asp:repeater>
                                             <p class="clearboth"></p>
                                             <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHiddenField" runat="server" />
                                         </asp:panel>
                                         <asp:panel ID="CustomerRelatedHobbyPopupPage2" runat="server" CssClass="CustomerRelatedHobbyPopupPage">
                                         	<div id="CustomerRelatedHobbyPopupOtherWrap">
                                            	<icrop:CustomTextBox ID="CustomerRelatedHobbyPopupOtherText" runat="server" CssClass="TextArea" PlaceHolderWordNo="10129" Width="330" MaxLength="30" TabIndex="1003" />
                                            </div>
                                         </asp:panel>
                                         <p class="clearboth"></p>
                                    </div>
                                    <p class="clearboth"></p>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                            <div class="popUpFooterHobby"></div>
                        </asp:Panel>
                        <%--2013/11/27 TCS 市川 Aカード情報相互連携開発 END--%>
                        <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupRowCount" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHobbyNo" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupOtherHobbyDefaultText" runat="server" />
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupTitlePage1" runat="server"/>
                        <asp:HiddenField ID="CustomerRelatedHobbyPopupTitlePage2" runat="server"/>
                        <asp:HiddenField ID="HobbyOthererrMsg" runat="server"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td> 
                    <%--　＊＊＊＊＊＊＊＊＊＊コンタクト方法＊＊＊＊＊＊＊＊＊＊--%>
                        <td style="width: 115px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                            <div id="CustomerRelatedContactArea" runat="server" onclick="setPopupContactPageOpen();">
                                <asp:Panel ID="CustomerRelatedContactSelectedEditPanel" runat="server">
                                    <asp:Panel id="CustomerRelatedContactSelectedImage" runat="server">
                                        <div class="CustomerRelatedContactSelectedImageAria" style="text-align: center; top: 5px; position: relative;">
                                            <asp:Image ID="CustomerRelatedContactTelImg" runat="server" ImageUrl="" /> 
                                            <asp:Image ID="CustomerRelatedContactMailImg" runat="server" ImageUrl="" />
                                            <div class="ContactText CustomerRelatedTitleFont">
                                                <icrop:CustomLabel ID="CustomerRelatedContactMobileLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10159" /> 
                                                <icrop:CustomLabel ID="CustomerRelatedContactHomeLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10160" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactSMSLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10161" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactEMailLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10162" />
                                                <icrop:CustomLabel ID="CustomerRelatedContactDMLabel" runat="server" Width="35" UseEllipsis="True" CssClass="ellipsis" TextWordNo="10163" />
                                            </div>
                                        </div>
                                    </asp:Panel>
					            </asp:Panel>
                                <asp:Panel ID="CustomerRelatedContactSelectedNewPanel" runat="server" style="height:100%;width:100%;">
                                    <table style="height:100%;width:100%;">
                                        <tr>
                                            <td style="vertical-align:middle;text-align:center;">
                                                <icrop:CustomLabel ID="CustomLabel2" runat="server" Width="98" CssClass="CustomerRelatedTitleFont ellipsis" UseEllipsis="true" TextWordNo="10114" />
                                            <td>
                                        </tr>
                                    </table>
					            </asp:Panel>
                            </div>
                            <%--2012/03/08 TCS 山口 【SALES_2】性能改善 START--%>
                            <asp:Button runat="server" ID="ContactOpenButton" style="display:none" />
                            <div id="CustomerRelatedContactPopupArea" class="scNscPopUpContactSelect scNscPopUpContactSelect48" style="display:none;">
                                <div class="scNscPopUpContactSelectWindownBox WindownBox48">
                                    <div class="scNscPopUpContactSelectHeader">
                                        <div>
                                            <a onclick="cancelContact();" class="scNscPopUpContactCancelButton styleCut"><icrop:CustomLabel ID="ContactHeaderCancelLabel" runat="server" TextWordNo="10125" /></a>                                            
                                            <asp:button ID="CustomerRelatedContactPopupCancelButton" runat="server" style="display:none" />
                                        </div> 
                                        <h3 class="popUpTitle">
                                            <icrop:CustomLabel id="ContactHeaderTitleLabel" runat="server" CssClass="styleCut" Width="250px" TextWordNo="10133" />
                                        </h3>                                        
                                        <div>
                                            <asp:LinkButton ID="ContactHeaderRegistLinkButton" runat="server" CssClass="scNscPopUpContactCompleteButton styleCut" OnClientClick="return registContact();"></asp:LinkButton></div></div><div class="scNscPopUpContactSelectListArea">
                                        <asp:Panel runat="server" ID="ContactVisiblePanel" Visible="false" > 
                                            <div class="ContactWish">
                                                <div>
                                                    <h4><icrop:CustomLabel ID="ContactWishTitleLabel" runat="server" Width="400px" CssClass="styleCut" TextWordNo="10134" /></h4>
                                                </div>
                                                <div id="ContactToolWrap">
                                                    <ul class="scNscPopUpContactSelect5Button">
                                                        <li id="ContactToolMobileLi" runat="server" onclick="selectContactTool(1);" ><asp:HiddenField ID="ContactToolMobileHidden" runat="server" /><asp:panel id="ContactToolMobileImage" runat="server" class="ContactToolIcon" /></li>
                                                        <li id="ContactToolTelLi" runat="server" onclick="selectContactTool(2);" ><asp:HiddenField ID="ContactToolTelHidden" runat="server" /><asp:panel id="ContactToolTelImage" runat="server" class="ContactToolIcon" /></li>
                                                        <li id="ContactToolSMSLi" runat="server" onclick="selectContactTool(3);" ><asp:HiddenField ID="ContactToolSMSHidden" runat="server" /><asp:panel id="ContactToolSMSImage" runat="server" class="ContactToolIcon" /></li>
                                                        <li id="ContactToolEmailLi" runat="server" onclick="selectContactTool(4);" ><asp:HiddenField ID="ContactToolEmailHidden" runat="server" /><asp:panel id="ContactToolEmailImage" runat="server" class="ContactToolIcon" /></li>
                                                        <li id="ContactToolDMLi" runat="server" onclick="selectContactTool(5);" ><asp:HiddenField ID="ContactToolDMHidden" runat="server" /><asp:panel id="ContactToolDMImage" runat="server" class="ContactToolIcon" /></li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="ContactWeek1">
                                                <div class="TimeZoneTitle">
                                                    <h4 style="height: 12px">
                                                    <icrop:CustomLabel ID="ContactWeek1TitleLabel" runat="server" CssClass="styleCut" Width="230px" TextWordNo="10135" />
                                                    </h4>
                                                    <p class="DayOrWeek">
                                                        <icrop:CustomLabel ID="ContactWeek1WeekdayLabel" runat="server" Width="70px" TextWordNo="10136" CssClass="dayBlue styleCut" onclick="selectContactWeekday(1);" />
                                                        <icrop:CustomLabel ID="ContactWeek1DelimiterLabel" runat="server" TextWordNo="10137" />
                                                        <icrop:CustomLabel ID="ContactWeek1WeekendLabel" runat="server" Width="70px" TextWordNo="10138" CssClass="dayBlue styleCut" onclick="selectContactWeekend(1);" />
                                                    </p>
                                                </div>
                                                <div>
                                                    <ul class="scNscPopUpContactSelect7Button">
                                                        <li id="ContactWeek1MonLi" runat="server" onclick="selectContactWeek(1,[1]);"><asp:HiddenField ID="ContactWeek1MonHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1MonLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10139" /></div></li>
                                                        <li id="ContactWeek1TueLi" runat="server" onclick="selectContactWeek(1,[2]);"><asp:HiddenField ID="ContactWeek1TueHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1TueLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10140" /></div></li>
                                                        <li id="ContactWeek1WedLi" runat="server" onclick="selectContactWeek(1,[3]);"><asp:HiddenField ID="ContactWeek1WedHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1WedLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10141" /></div></li>
                                                        <li id="ContactWeek1TurLi" runat="server" onclick="selectContactWeek(1,[4]);"><asp:HiddenField ID="ContactWeek1TurHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1TurLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10142" /></div></li>
                                                        <li id="ContactWeek1FriLi" runat="server" onclick="selectContactWeek(1,[5]);"><asp:HiddenField ID="ContactWeek1FriHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1FriLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10143" /></div></li>
                                                        <li id="ContactWeek1SatLi" runat="server" onclick="selectContactWeek(1,[6]);"><asp:HiddenField ID="ContactWeek1SatHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1SatLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10144" /></div></li>
                                                        <li id="ContactWeek1SunLi" runat="server" onclick="selectContactWeek(1,[7]);"><asp:HiddenField ID="ContactWeek1SunHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek1SunLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10145" /></div></li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="ContactTime1">
                                                <ul class="scNscPopUpContactSelect5Button">
                                                    <asp:Repeater id="ContactTime1Repeater" runat="server">
                                                        <ItemTemplate>
                                                            <li id="ContactTime1Li_Row" runat="server" ClientIDMode="Predictable" >
                                                                <div style=" overflow:hidden;height: 39px; ">
                                                                    <div class="Center" style="line-height: 13.5px;" >                                                                
                                                                        <icrop:customLabel ID="ContactTime1Label_Row" runat="server" Width="65" style="word-wrap:break-word;" ClientIDMode="Predictable"/>
                                                                        <asp:HiddenField ID="ContactTime1Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                        <asp:HiddenField ID="ContactTimeZoneNo1Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                    </div>
                                                                </div>
                                                            </li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </div>
                                            <div class="ContactWeek2">
                                                <div class="TimeZoneTitle">
                                                    <h4 style="height: 12px">
                                                        <icrop:CustomLabel ID="ContactWeek2TitleLabel" runat="server" CssClass="styleCut" Width="230px" TextWordNo="10146" />
                                                    </h4>
                                                    <p class="DayOrWeek">
                                                        <icrop:CustomLabel ID="ContactWeek2WeekdayLabel" runat="server" Width="70px" TextWordNo="10136" class="dayBlue styleCut" onclick="selectContactWeekday(2);" />
                                                        <icrop:CustomLabel ID="ContactWeek2DelimiterLabel" runat="server" TextWordNo="10137" />
                                                        <icrop:CustomLabel ID="ContactWeek2WeekendLabel" runat="server" Width="70px" TextWordNo="10138" class="dayBlue styleCut" onclick="selectContactWeekend(2);" />
                                                    </p>
                                                </div>
                                                <div>
                                                    <ul class="scNscPopUpContactSelect7Button">
                                                        <li id="ContactWeek2MonLi" runat="server" onclick="selectContactWeek(2,[1]);"><asp:HiddenField ID="ContactWeek2MonHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2MonLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10139" /></div></li>
                                                        <li id="ContactWeek2TueLi" runat="server" onclick="selectContactWeek(2,[2]);"><asp:HiddenField ID="ContactWeek2TueHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2TueLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10140" /></div></li>
                                                        <li id="ContactWeek2WedLi" runat="server" onclick="selectContactWeek(2,[3]);"><asp:HiddenField ID="ContactWeek2WedHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2WedLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10141" /></div></li>
                                                        <li id="ContactWeek2TurLi" runat="server" onclick="selectContactWeek(2,[4]);"><asp:HiddenField ID="ContactWeek2TurHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2TurLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10142" /></div></li>
                                                        <li id="ContactWeek2FriLi" runat="server" onclick="selectContactWeek(2,[5]);"><asp:HiddenField ID="ContactWeek2FriHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2FriLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10143" /></div></li>
                                                        <li id="ContactWeek2SatLi" runat="server" onclick="selectContactWeek(2,[6]);"><asp:HiddenField ID="ContactWeek2SatHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2SatLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10144" /></div></li>
                                                        <li id="ContactWeek2SunLi" runat="server" onclick="selectContactWeek(2,[7]);"><asp:HiddenField ID="ContactWeek2SunHidden" runat="server" /><div style="width:100%; text-align: center;" ><icrop:CustomLabel ID="ContactWeek2SunLabel" runat="server" CssClass="styleCut" Width="40px" TextWordNo="10145" /></div></li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="ContactTime2">
                                                <ul class="scNscPopUpContactSelect5Button">
                                                    <asp:Repeater id="ContactTime2Repeater" runat="server">
                                                        <ItemTemplate>
                                                            <li id="ContactTime2Li_Row" runat="server" ClientIDMode="Predictable">
                                                                <div style=" overflow:hidden;height: 39px; ">
                                                                    <div class="Center" style="line-height: 13.5px;" >
                                                                        <icrop:customLabel ID="ContactTime2Label_Row" runat="server" Width="65" style="word-wrap:break-word;" ClientIDMode="Predictable"/>
                                                                        <asp:HiddenField ID="ContactTime2Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                        <asp:HiddenField ID="ContactTimeZoneNo2Hidden_Row" runat="server" ClientIDMode="Predictable" />
                                                                    </div>
                                                                </div>
                                                            </li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div class="scNscPopUpContactSelectFootetr"></div>
                                </div>
                            </div>
                            <%--2012/03/08 TCS 山口 【SALES_2】性能改善 END--%>
                            <asp:HiddenField ID="ContactErrMsg" runat="server"/>
                            <asp:HiddenField ID="ContactTime1Count" runat="server" />
                            <asp:HiddenField ID="ContactTime2Count" runat="server" />
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr> 
                </table> 
				<p class="clearboth"></p>
			</div>
			<hr />
            
			<div class="scNscCustomerMemoArea">
				<h4 style="overflow:hidden;"><icrop:CustomLabel ID="WordLiteral104" runat="server" Width="320px" TextWordNo="10104" /></h4>
                <asp:UpdatePanel id="CustomerMemoUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="CustomerMemoEditCloseButton" runat="server" style="display:none" />
                        <div id="CustomerMemo_Click" runat="server" onclick="setPopupCustomerMemoOpen();" >
					        <div class="scNscCustomerMemoPaper">
						        <asp:Panel ID="EditCustomerMemoPanel" runat="server" Visible="true" >
							        <p class="scNscCustomerMemoPaperDay">
                                        <icrop:CustomLabel ID="CustomerMemoDayLabel" runat="server" Text="" ></icrop:CustomLabel></p><p>
                                        <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
                                        <asp:TextBox ID="CustomerMemoLabel" ReadOnly="true" MaxLength="1024" Width="438" Height="35" runat="server" TextMode="MultiLine" />
                                        <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
                                        <%--<icrop:CustomLabel ID="CustomerMemoLabel" runat="server" Width="438" Height="71" Text="" />--%>
                                    </p>
                                    <%--2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない START--%>
                                    <div id="CustomerMemoDummyArea"></div>
                                    <asp:HiddenField ID="CustomerMemoDummyAreaFlg" runat="server" Value="0" />
                                    <%--2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない END--%>
                                </asp:Panel>
						        <asp:Panel ID="NewCustomerMemoPanel" runat="server" Visible="false" >
							        <p class="scNscCustomerMemoPaperDay">&nbsp;</p><p class="scNscCustomerMemoPaperTxt"><br /></p>
                                </asp:Panel>
                            </div>
				        </div>
                    </ContentTemplate> 
                </asp:UpdatePanel> 
			</div>
		</div>
	</div>

	<div id="scNscCustomerRightArea" class="contentsFrame" style="height: 614px; right: 10px; position: relative;">
		<h2 class="contentTitle" >
            <icrop:CustomLabel ID="WordLiteral105" runat="server" CssClass="styleCut" Width="200px" TextWordNo="10105" />
        </h2>
		<div class="scNscCurriculumArea">
            <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
            <%--＊＊＊＊＊＊＊＊＊＊重要事項＊＊＊＊＊＊＊＊＊＊--%>
            <div id="ImportantContactArea" runat="server" >
                <div class="nsc40-02Text01">
                    <icrop:CustomLabel ID="ImportantContactLabel" runat="server" CssClass="styleCut" TextWordNo="10178" />
                </div>
                <div class="nsc40-02TableBox">
                    <div class="nsc40-02BoxLeft">
                        <div class="arrowOpen" onclick="importantContactOpen(1);"></div>
                        <div class="arrowClose" onclick="importantContactOpen(0);" style="display:none;"></div>
                        <div class="textBox01">
                            <icrop:CustomLabel ID="ComplaintCategoryLabel" runat="server" CssClass="ellipsis" />
                        </div>
                        <div class="textData">
                            <icrop:CustomLabel ID="ReceptionDateLabel" runat="server" CssClass="styleCut" />
                        </div>
                        <div class="textBox02">
                            <p><icrop:CustomLabel ID="ComplaintOverviewLabel" runat="server" Width="300px" CssClass="ellipsis fontBold" /></p>
                            <p><icrop:CustomLabel ID="ComplaintDetailLabel" runat="server" Width="300px" CssClass="ellipsis" /></p>
                        </div>
                    </div>
                    <div class="nsc40-02BoxRight">
                        <div class="textBox01">
                            <icrop:CustomLabel ID="ComplaintStatusLabel" runat="server" CssClass="fontBold styleCut" />
                        </div>
                        <div class="textBox02">
                            <asp:Image ID="ComplaintAccountImg" runat="server" ImageUrl="" /> 
                            <%-- 2012/04/17 TCS 安田 【SALES_2】タップしても、隠れた文字が表示されない（ユーザー課題 No.39） START --%>
                            <icrop:CustomLabel ID="ComplaintAccount" runat="server" CssClass="ellipsis" UseEllipsis="True" Width="75px" />
                            <%-- 2012/04/17 TCS 安田 【SALES_2】タップしても、隠れた文字が表示されない（ユーザー課題 No.39） END --%>
                        </div>
                    </div>
			        <div class="clearboth">&nbsp;</div></div><asp:HiddenField ID="ImportantContactLeftAreaOpenFlg" runat="server" Value="0" />
            </div>

			<div class="scNscCurriculumTabArea">
				<div class="scNscCurriculumTabAllAc" id="TabAll" >
                    <icrop:CustomLabel ID="WordLiteral106" runat="server" CssClass="styleCut" Width="95px" TextWordNo="10106" />
                </div>
				<div class="scNscCurriculumTabMargin"> </div>
				<div class="scNscCurriculumTabSalesOff" id="TabSales">
                    <div class="styleCut" style="width:95px; margin-left: 5px;"><img id="imageSales" alt="" src="../Styles/Images/SC3080201/ico113.png" width="20px" height="16px" style="padding: 0px 5px 0px 0px;" /><icrop:CustomLabel ID="WordLiteral107" runat="server" TextWordNo="10107"/></div>
                </div>
				<div class="scNscCurriculumTabMargin"> </div>
                <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
				<div class="scNscCurriculumTabServiceOff" id="TabService">
                    <%-- 2012/03/13 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.235) START --%>
                    <div class="styleCut" style="width:95px; margin-left: 5px;"><img alt="" id="imageService" src="../Styles/Images/SC3080201/contact_service_off.png" width="18px" height="17px" style="padding: 0px 5px 0px 0px;" /><icrop:CustomLabel ID="CustomLabel6" runat="server" TextWordNo="10154"/></div> 
                    <%-- 2012/03/13 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.235) END --%>
                </div>
                <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
                <div class="scNscCurriculumTabMargin"> </div>
                <div class="scNscCurriculumTabCrOff" id="TabCr">
                    <div class="styleCut" style="width:95px; margin-left: 5px;"><img id="imageCr" alt="" src="../Styles/Images/SC3080201/ico115.png" width="15px" height="16px" style="padding: 0px 5px 0px 0px;"/><icrop:CustomLabel ID="CustomLabel7" runat="server" TextWordNo="10155"/></div> 
                </div>
				<p class="clearboth"></p>
			</div>
            <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
			<div id="ContactHistoryListArea" runat="server" class="scNscCurriculumListArea">
				<div id="ContactHistoryListBox" runat="server" class="scNscCurriculumListBox"> 
                    <asp:HiddenField runat="server" ID="ContactHistoryTabIndex" value="0"/>
                    <asp:HiddenField runat="server" ID="ContactHistoryCountHidden" value="0"/>
                    <asp:HiddenField runat="server" ID="reloadFlg" value="1"/>
                    <icrop:CustomRepeater ID="ContactHistoryRepeater" runat="server" OnClientRender="contactHistoryRepeater_Render" Width="420px" Height="500px" OnClientLoadCallbackResponse="contactHistoryRepeater_LoadCallbackResponse" />
                        <script type="text/javascript">
                            function contactHistoryRepeater_Render(row, view) {
                                
                                //取得したデータを保持
                                var no = row.NO;
                                var actualKindImg = row.ACTUALKINDIMG;
                                var actualDateString = row.ACTUALDATESTRING;
                                var contact = row.CONTACT;
                                var crActStatusImg = row.CRACTSTATUSIMG;
                                var operationCodeImg = row.OPERATIONCODEIMG;
                                var userName = row.USERNAME;
                                var complaintOverview = row.COMPLAINT_OVERVIEW;
                                var actualDetail = row.ACTUAL_DETAIL;
                                var memo = row.MEMO;
                                var actualKind = row.ACTUALKIND;
                                var colorFlg = row.COLORFLG;
                                var serviceInInfo = row.SERVICEININFO;

                                //入庫関連の項目取得
                                var mileage = row.MILEAGE
                                var infomation = row.INFOMATION
                                var menteinfo = row.MENTEINFO
                                var mainteamount = row.MAINTEAMOUNT
                                var vclregno = row.VCLREGNO

                                //HTML作成用変数
                                var htmlStart = "";
                                var divActualKindIcon = "";
                                var pActualDateLabel = "";
                                var pActualLabel = "";
                                var divActualDateActualLabel = "";
                                var divStatusIcon = "";
                                var divOperationIcon = "";
                                var divOperationNameLabel = "";
                                var divCrArea = "";

                                var divMileInfo = "";
                                var pMile = "";
                                var pInfo = "";
                                var divMente = "";
                                var pMente = "";

                                //HTML作成開始
                                //行タグ
                                var webkitSize = "";
                                if (actualKind == '3') {
                                    webkitSize = "style='background-size:100% 100%,auto;'"
                                }
                                if (colorFlg == 0) {
                                    htmlStart = $("<li class='scNscCurriculumListBackGray' actualKind='" + actualKind + "' " + webkitSize + "> </li>");
                                } else {
                                    htmlStart = $("<li class='scNscCurriculumListBackWhite' actualKind='" + actualKind + "' " + webkitSize + "> </li>");
                                }

                                //列タグ
                                //活動種類アイコン
                                divActualKindIcon = $("<div class='scNscCurriculumListIcon1'><img alt='' src='" + actualKindImg + "' /></div>");

                                //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                                //サービスの場合処理を分岐
                                if (actualKind == '1' || actualKind == '3' || actualKind == '4') {
                                    //活動日
                                    pActualDateLabel = "<p class='scNscCurriculumListTxtDay'><span class='scNscCurriculumListTxt1Label ellipsis' >" + actualDateString + "</span></p>";
                                    if (actualKind == '4') {
                                        //活動内容
                                        pActualLabel = "<p><span class='scNscCurriculumListTxt1Label fontBold useEllipsis' >" + contact + "</span></p>";
                                        //活動日-活動内容
                                        divActualDateActualLabel = $("<div class='scNscCurriculumListTxt1Lng'>" + pActualDateLabel + pActualLabel + "</div>");
                                    } else {
                                        //活動内容
                                        pActualLabel = "<p><span class='scNscCurriculumListTxt1Label fontBold useEllipsis' >" + contact + "</span></p>";
                                        //活動日-活動内容
                                        divActualDateActualLabel = $("<div class='scNscCurriculumListTxt1'>" + pActualDateLabel + pActualLabel + "</div>");
                                        //ステータス(アイコン)
                                        if (crActStatusImg == "") {
                                            divStatusIcon = $("<div class='scNscCurriculumListIcon2'></div>");
                                        } else {
                                            divStatusIcon = $("<div class='scNscCurriculumListIcon2'><img alt='' src='" + crActStatusImg + "'  /></div>");
                                        }
                                    }
                                    //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

                                    //実施者権限(アイコン)
                                    if (operationCodeImg == '') {
                                        divOperationIcon = $("<div class='scNscCurriculumListIcon3'></div>");
                                    } else {
                                        divOperationIcon = $("<div class='scNscCurriculumListIcon3'><img alt='' src='" + operationCodeImg + "' /></div>");
                                    }
                                } else {
                                    //活動日
                                    pActualDateLabel = "<p class='scNscCurriculumListTxtDay'><span class='scNscCurriculumListTxt1Label ellipsis' >" + actualDateString + "</span></p>";
                                    //活動内容
                                    if (vclregno == "-") {
                                        pActualLabel = "<p style='text-align:center;'><span class='scNscCurriculumListTxt1Label fontBold useEllipsis'>" + vclregno + "</span></p>";
                                    } else {
                                        pActualLabel = "<p class='useEllipsis'><span class='scNscCurriculumListTxt1Label fontBold useEllipsis' >" + vclregno + "</span></p>";
                                    }
                                    //活動日-活動内容
                                    divActualDateActualLabel = $("<div class='scNscCurriculumListTxt3'>" + pActualDateLabel + pActualLabel + "</div>");
                                    //走行距離 + Infomation Source
                                    pMile = "<p class='mileageTextLabel'><span class='useEllipsis mileageTextLabel ' >" + mileage + "</span></p>";
                                    pInfo = "<p class='infomationTextLavel'><span class='infomationTextLavel useEllipsis' >" + infomation + "</span></p>";
                                    divMileInfo = $("<div class='scNscCurriculumListTxt4'>" + pMile + pInfo + "</div>");
                                    //点検名称
                                    if (menteinfo == "-") {
                                        pMente = "<p class='menteTextLavel'><span class='menteTextLavel useEllipsis' style='text-align:center;'>" + menteinfo + "</span></p>";
                                    } else {
                                        pMente = "<p class='menteTextLavel'><span class='menteTextLavel useEllipsis' >" + menteinfo + "</span></p>";
                                    }
                                    divMente = $("<div class='scNscCurriculumListTxt5'>" + pMente + "</div>");
                                    //実施者権限(アイコン)
                                    if (operationCodeImg == '') {
                                        divOperationIcon = $("<div class='scNscCurriculumListIcon3'></div>");
                                    } else {
                                        divOperationIcon = $("<div class='scNscCurriculumListIcon3'><img alt='' src='" + operationCodeImg + "' /></div>");
                                    }
                                }

                                //実施者名
                                //2012/04/17 TCS 安田 【SALES_2】タップしても、隠れた文字が表示されない（ユーザー課題 No.39） START
                                divOperationNameLabel = $("<div class='scNscCurriculumListTxt2'></div>");
                                elementUserName = $("<span class='scNscCurriculumListTxt1Label' style='display:inline-block;width:80px;'>" + userName + "</span>");
                                elementUserName.CustomLabel({ 'useEllipsis': 'true' });
                                divOperationNameLabel.append(elementUserName);
                                //2012/04/17 TCS 安田 【SALES_2】タップしても、隠れた文字が表示されない（ユーザー課題 No.39） END
                                
                                //整備詳細
                                if (actualKind == '2') {

                                    //整備詳細を設定
                                    var serviceInInfoDetailRow = serviceInInfo.split("|||");
                                    var serviceInInfoDetail
                                    var cnt = serviceInInfoDetailRow.length - 1;
                                    var i
                                    divCrArea = "";
                                    divCrArea = divCrArea + "<div class='contactHistoryCrArea contactHistoryServiceArea'>";

                                    for (i = 0; i < cnt; i++) {
                                        serviceInInfoDetail = serviceInInfoDetailRow[i].split("||");
                                        divCrArea = divCrArea + "<p class='useEllipsis mainte'><span class='cnt'>" + (i + 1) + "</span><span class='menteinfo useEllipsis'>" + serviceInInfoDetail[0] + "</span></p>";
                                    }

                                    //整備料金を設定
                                    divCrArea = divCrArea + "<p class='useEllipsis amount'><span class='mainteamount'>" + $("#mainteamount").val() + "</span><span class='useEllipsis'>" + mainteamount + "</span></p>";

                                    divCrArea = divCrArea + "</div>";
                                    divCrArea = $(divCrArea);
                                }

                                //苦情概要など
                                if (actualKind == '3') {
                                    divCrArea = "<div class='contactHistoryCrArea'>";
                                    //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                                    divCrArea = divCrArea + "<p><span class='fontBold'>" + complaintOverview + "</span></p>";
                                    //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END
                                    divCrArea = divCrArea + "<p><span>" + actualDetail + "</span></p>";
                                    divCrArea = divCrArea + "<p><span>" + memo + "</span></p>";
                                    divCrArea = divCrArea + "</div>";
                                    divCrArea = $(divCrArea);
                                }

                                htmlStart.append(divActualKindIcon);
                                htmlStart.append(divActualDateActualLabel);

                                if (actualKind == '1' || actualKind == '3') {
                                    htmlStart.append(divStatusIcon);
                                    htmlStart.append(divOperationIcon);
                                    //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                                } else if (actualKind == '4') {
                                    htmlStart.append(divOperationIcon);
                                    //2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END
                                } else {
                                    htmlStart.append(divMileInfo);

                                    htmlStart.append(divMente);
                                    htmlStart.append(divOperationIcon);
                                }

                                htmlStart.append(divOperationNameLabel);
                                htmlStart.append(divCrArea);

                                //ツールチップ設定
                                htmlStart.find(".useEllipsis").CustomLabel({ 'useEllipsis': 'true' });

                                view.append(htmlStart);

                            }
                            function contactHistoryRepeater_LoadCallbackResponse(result) {
                                //リロード中フラグOFF
                                $("#reloadFlg").val("0")
                                //コンタクト履歴処理中イメージのz-index復元
                                $("#ContactHistoryRepeater .icrop-CustomRepeater-progress").css({ "z-index": "100000" });
                            }
                        </script>
				</div>
			</div>
            <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
            <%--2012/02/15 TCS 山口 【SALES_2】 END--%>
		</div>
	</div>
	<p class="clearboth"></p>

    <%--2012/02/15 TCS 山口 【SALES_2】 START--%>
    <!--ポップアップ系OPEN時の他ボタン制御 -->
    <div id="messageWinPopupBlack">&nbsp;</div><%--2012/02/15 TCS 山口 【SALES_2】 END--%><div style="display:none;">
    <asp:UpdatePanel ID="SC3080201HiddenFieldUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <!-- 顧客・車両情報編集 START -->
            <asp:HiddenField ID="useNameTitleHidden" runat="server" />
            <asp:HiddenField ID="useActvctgryHidden" runat="server" />
            <asp:HiddenField ID="nameTitleHidden" runat="server" />
            <asp:HiddenField ID="nameTitleTextHidden" runat="server" />
            <asp:HiddenField ID="actvctgryidHidden" runat="server" />
            <%-- 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
            <asp:HiddenField ID="modelYearHidden" runat="server" />
            <asp:HiddenField ID="modelYearNameHidden" runat="server" />
            <asp:HiddenField ID="lcVcldlrlcverHidden" runat="server" />
            <asp:HiddenField ID="tempModelYearidHidden" runat="server" />
            <asp:HiddenField ID="tempModelYearnmHidden" runat="server" />
            <%-- 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END   --%>
            <asp:HiddenField ID="reasonidHidden" runat="server" />
            <asp:HiddenField ID="updatefuncflgHidden" runat="server" />
            <asp:HiddenField ID="smsDisplayFlgHidden" runat="server" />
            <asp:HiddenField ID="emailDisplayFlgHidden" runat="server" />
            <asp:HiddenField ID="postsrhFlgHidden" runat="server" />
            <asp:HiddenField ID="dmailDisplayFlgHidden" runat="server" />
            <asp:HiddenField ID="orginputFlgHidden" runat="server" />
        
            <asp:HiddenField ID="custFlgHidden" runat="server" />
            <asp:HiddenField ID="serverProcessFlgHidden" runat="server" />
        
            <asp:HiddenField ID="custPageHidden" runat="server" />
            <asp:HiddenField ID="vehiclePageHidden" runat="server" />
            <%-- 2014/04/01 TCS 松月 TMT不具合対応 Modify Start START --%>    
            <asp:HiddenField ID="actvctgryidHidden_Old" runat="server" />
            <asp:HiddenField ID="reasonidHidden_Old" runat="server" />
            <%-- 2014/04/01 TCS 松月 TMT不具合対応 Modify Start START --%>
                                                        
            <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
            <asp:HiddenField ID="nameBeforeHidden" runat ="server" />
            <asp:HiddenField ID="dummyNameFlgHidden" runat ="server" />
            <asp:HiddenField ID="telSerchFlgHidden" runat ="server" />
            <asp:HiddenField ID="birthdayHidden" runat ="server" />
            <%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>
            <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
            <asp:HiddenField ID="usePrivateFleetItemHidden" runat="server" />
            <asp:HiddenField ID="useStateHidden" runat="server" />
            <asp:HiddenField ID="useDistrictHidden" runat="server" />
            <asp:HiddenField ID="useCityHidden" runat="server" />
            <asp:HiddenField ID="useLocationHidden" runat="server" />

            <asp:HiddenField ID="privateFleetItemHidden" runat="server" />
            <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
            <asp:HiddenField ID="cstOrgnzNameRefType" runat="server" />
            <asp:HiddenField ID="custOrgnzHidden" runat="server" />
            <asp:HiddenField ID="custOrgnzNameInputTypeHidden" runat="server" />
            <asp:HiddenField ID="custOrgnzInputTypeHidden" runat="server" />
            <asp:HiddenField ID="custSubCtgry2Hidden" runat="server" />
            <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
            <%-- 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START --%>
            <asp:HiddenField ID="custOrgnzNameHidden" runat="server" />
            <%-- 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END END --%>
            <asp:HiddenField ID="stateHidden" runat="server" />
            <asp:HiddenField ID="districtHidden" runat="server" />
            <asp:HiddenField ID="cityHidden" runat="server" />
            <asp:HiddenField ID="locationHidden" runat="server" />
            <asp:HiddenField ID="locationZipHidden" runat="server" />

            <asp:HiddenField ID="labelNametitleSettingHidden" runat="server" />
            <asp:HiddenField ID="addressDirectionHidden" runat="server" />
            <asp:HiddenField ID="addressDataCleansingHidden" runat="server" />
            <asp:HiddenField ID="postSearchVisibleHidden" runat="server" />
            <%-- 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START --%>
            <asp:HiddenField ID="address1AutoInputHidden" runat="server" />
            <%-- 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END --%>

            <asp:HiddenField ID="inputSettingHidden01" runat="server" />
            <asp:HiddenField ID="inputSettingHidden02" runat="server" />
            <asp:HiddenField ID="inputSettingHidden03" runat="server" />
            <asp:HiddenField ID="inputSettingHidden04" runat="server" />
            <asp:HiddenField ID="inputSettingHidden05" runat="server" />
            <asp:HiddenField ID="inputSettingHidden06" runat="server" />
            <asp:HiddenField ID="inputSettingHidden07" runat="server" />
            <asp:HiddenField ID="inputSettingHidden08" runat="server" />
            <asp:HiddenField ID="inputSettingHidden09" runat="server" />
            <asp:HiddenField ID="inputSettingHidden10" runat="server" />
            <asp:HiddenField ID="inputSettingHidden11" runat="server" />
            <asp:HiddenField ID="inputSettingHidden12" runat="server" />
            <asp:HiddenField ID="inputSettingHidden13" runat="server" />
            <asp:HiddenField ID="inputSettingHidden14" runat="server" />
            <asp:HiddenField ID="inputSettingHidden15" runat="server" />
            <asp:HiddenField ID="inputSettingHidden16" runat="server" />
            <asp:HiddenField ID="inputSettingHidden17" runat="server" />
            <asp:HiddenField ID="inputSettingHidden18" runat="server" />
            <asp:HiddenField ID="inputSettingHidden19" runat="server" />
            <asp:HiddenField ID="inputSettingHidden20" runat="server" />
            <asp:HiddenField ID="inputSettingHidden21" runat="server" />
            <asp:HiddenField ID="inputSettingHidden22" runat="server" />
            <asp:HiddenField ID="inputSettingHidden23" runat="server" />
            <asp:HiddenField ID="inputSettingHidden24" runat="server" />
            <asp:HiddenField ID="inputSettingHidden25" runat="server" />
            <asp:HiddenField ID="inputSettingHidden26" runat="server" />
            <asp:HiddenField ID="inputSettingHidden27" runat="server" />
            <asp:HiddenField ID="inputSettingHidden28" runat="server" />
            <asp:HiddenField ID="inputSettingHidden29" runat="server" />
            <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
            <%--2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START--%>
            <asp:HiddenField ID="inputSettingHidden36" runat="server" />
            <%--2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END--%>
            <!-- 顧客・車両情報編集 END -->

            <!-- 車両情報編集 初期表示時用 START -->
            <%--2012/02/15 TCS 山口 【SALES_2】 START--%> 
            <%--
            <asp:HiddenField ID="makerTextBoxBackHidden" runat="server" />
            <asp:HiddenField ID="modelTextBoxBackHidden" runat="server" />
            <asp:HiddenField ID="vclregnoTextBoxBackHidden" runat="server" />
            <asp:HiddenField ID="vinTextBoxBackHidden" runat="server" />
            <asp:HiddenField ID="vcldelidateDateTimeBackHidden" runat="server" />            
            <asp:HiddenField ID="actvctgryLabel2BackHidden" runat="server" />
            --%>
            <%--2012/02/15 TCS 山口 【SALES_2】 END--%>
            <asp:HiddenField ID="editVehicleModeBackHidden" runat="server" />
            <!-- 車両情報編集 初期表示時用 END -->

            <%--2012/02/15 TCS 山口 【SALES_2】 START--%> 
            <!-- 顧客情報編集 初期表示時用 START -->
        
            <asp:HiddenField ID="custNoNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoTelNoErrMsg" runat="server" />
            <%--2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
            <asp:HiddenField ID="custNoDummyNameFlgErrMsg" runat="server" />
            <%--2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>
            <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 START--%>
            <asp:HiddenField ID="custNoMiddleNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoLastNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoSexErrMsg" runat="server" />
            <asp:HiddenField ID="custNoNameTitleErrMsg" runat="server" />
            <asp:HiddenField ID="custNoCustypeErrMsg" runat="server" />
            <asp:HiddenField ID="custNoPrivateFleetItemErrMsg" runat="server" />
            <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
            <asp:HiddenField ID="custNoOrgnzNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoSubCtgry2ErrMsg" runat="server" />
            <%-- 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
            <asp:HiddenField ID="custNoEmpNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoEmpDeptErrMsg" runat="server" />
            <asp:HiddenField ID="custNoEmpPosErrMsg" runat="server" />
            <asp:HiddenField ID="custNoFaxErrMsg" runat="server" />
            <asp:HiddenField ID="custNoBussinessTelErrMsg" runat="server" />
            <asp:HiddenField ID="custNoZipErrMsg" runat="server" />
            <asp:HiddenField ID="custNoAddress1ErrMsg" runat="server" />
            <asp:HiddenField ID="custNoAddress2ErrMsg" runat="server" />
            <asp:HiddenField ID="custNoAddress3ErrMsg" runat="server" />
            <asp:HiddenField ID="custNoStateErrMsg" runat="server" />
            <asp:HiddenField ID="custNoDistrictErrMsg" runat="server" />
            <asp:HiddenField ID="custNoCityErrMsg" runat="server" />
            <asp:HiddenField ID="custNoLocationErrMsg" runat="server" />
            <asp:HiddenField ID="custNoDomicileErrMsg" runat="server" />
            <asp:HiddenField ID="custNoEmail1ErrMsg" runat="server" />
            <asp:HiddenField ID="custNoEmail2ErrMsg" runat="server" />
            <asp:HiddenField ID="custNoCountryErrMsg" runat="server" />
            <asp:HiddenField ID="custNoSocialIdErrMsg" runat="server" />
            <asp:HiddenField ID="custNoBirtydayErrMsg" runat="server" />
            <asp:HiddenField ID="custNoActvctgryErrMsg" runat="server" />
            <asp:HiddenField ID="custNoFirmNameErrMsg" runat="server" />
            <asp:HiddenField ID="custNoContactPersonErrMsg" runat="server" />
            <%--2013/11/27 TCS 各務 Aカード情報相互連携開発 END--%>
            <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START --%>
            <asp:HiddenField ID="custNoCommercialRecvType" runat="server" />
            <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END --%>

            <asp:HiddenField ID="vehicleNoModelErrMsg" runat="server" />

            <%--2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
            <asp:HiddenField ID="DefaultMaker" runat="server" />
            <%--2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>
        
            <asp:HiddenField ID="actvctgryNameHidden" runat="server" />
            <asp:HiddenField ID="reasonNameHidden" runat="server" />
        
            <asp:HiddenField ID="tempActvctgryidHidden" runat="server" />
            <asp:HiddenField ID="tempReasonidHidden" runat="server" />
            <asp:HiddenField ID="tempActvctgrynmHidden" runat="server" />
            <asp:HiddenField ID="tempReasonnmHidden" runat="server" />

            <asp:HiddenField ID="nextVehicleFlg" runat="server" />
            <asp:HiddenField ID="vehiclePopUpAutoOpenFlg" runat="server" />

            <!-- 顧客情報編集 初期表示時用 END -->
            <%--2012/02/15 TCS 山口 【SALES_2】 END--%>

            <%--2013/06/30 TCS 黄 2013/10対応版　既存流用 START--%>
            <asp:HiddenField ID="CustomerLockVersion" runat="server" />
            <asp:HiddenField ID="CustomerDLRLockVersion" runat="server" />
            <%-- 2018/09/05 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
            <asp:HiddenField ID="CustomerLocalLockVersion" runat="server" />
            <%-- 2018/09/05 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
            <asp:HiddenField ID="cusLockvrHidden" runat="server" />
            <asp:HiddenField ID="cusVCLLockvrHidden" runat="server" />
            <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START --%>
            <asp:HiddenField ID="cusDLRLockvrHidden" runat="server" />
            <%-- 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END --%>
            <asp:HiddenField ID="vcllcverHidden" runat="server" />
            <asp:HiddenField ID="vcldlrlcverHidden" runat="server" />
            <asp:HiddenField ID="cstvcllcverHidden" runat="server" />
            <asp:HiddenField ID="cstvclidHidden" runat="server" />
            <asp:HiddenField ID="vclupdateHidden" runat="server" />
            <%--2013/06/30 TCS 黄 2013/10対応版　既存流用 END--%>
        </ContentTemplate>
    </asp:UpdatePanel>
        
        <%--2012/02/15 TCS 山口 【SALES_2】 START--%> 
        <!-- 顧客情報編集 初期表示時用 START -->
        <%--
        <asp:HiddenField ID="nameTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="nameTitleHiddenBackHidden" runat="server" />
        <asp:HiddenField ID="nameTitleTextHiddenBackHidden" runat="server" />
        <asp:HiddenField ID="manCheckBoxBackHidden" runat="server" />
        <asp:HiddenField ID="girlCheckBoxBackHidden" runat="server" />
        <asp:HiddenField ID="kojinCheckBoxBackHidden" runat="server" />
        <asp:HiddenField ID="houjinCheckBoxBackHidden" runat="server" />
        <asp:HiddenField ID="employeenameTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="employeedepartmentTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="employeepositionTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="mobileTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="telnoTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="businesstelnoTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="faxnoTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="zipcodeTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="addressTextBoxBackHiddenx" runat="server" />
        <asp:HiddenField ID="email1TextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="email2TextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="socialidTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="birthdayTextBoxBackHidden" runat="server" />
        <asp:HiddenField ID="actvctgryidHiddenBackHidden" runat="server" />
        <asp:HiddenField ID="reasonidHiddenBackHidden" runat="server" />
        <asp:HiddenField ID="actvctgryLabelBackHidden" runat="server" />
        <asp:HiddenField ID="smsCheckButtonBackHidden" runat="server" />
        <asp:HiddenField ID="emailCheckButtonBackHidden" runat="server" />
        
        <asp:HiddenField ID="custNoNameErrMsg" runat="server" />
        <asp:HiddenField ID="custNoTelNoErrMsg" runat="server" />
        
        <asp:HiddenField ID="vehicleNoModelErrMsg" runat="server" />
        
        <asp:HiddenField ID="actvctgryNameHidden" runat="server" />
        <asp:HiddenField ID="reasonNameHidden" runat="server" />
        
        <asp:HiddenField ID="actvctgryNameBackHidden" runat="server" />
        <asp:HiddenField ID="reasonNameBackHidden" runat="server" />

        <asp:HiddenField ID="tempActvctgryidHidden" runat="server" />
        <asp:HiddenField ID="tempReasonidHidden" runat="server" />
        <asp:HiddenField ID="tempActvctgrynmHidden" runat="server" />
        <asp:HiddenField ID="tempReasonnmHidden" runat="server" />

        <asp:HiddenField ID="nextVehicleFlg" runat="server" />
        <asp:HiddenField ID="vehiclePopUpAutoOpenFlg" runat="server" />

        --%>
        <!-- 顧客情報編集 初期表示時用 END -->
        <%--2012/02/15 TCS 山口 【SALES_2】 END--%>

        <!-- 顧客詳細 ポップアップ等抑制判定用 -->
        <asp:HiddenField ID="ReadOnlyFlagHidden" runat="server" />
        
        <!--$01 Redirect廃止-->
        <!-- 顧客情報新規登録再読込みURL -->
        <asp:HiddenField ID="CustomerReLoadURL" runat="server" />

        <%-- 2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START --%>
        <asp:HiddenField ID="FacePicUploadPath" runat="server" />
        <%-- 2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END --%>

        <%-- 2012/06/01 TCS 河原 FS開発 START --%>
        <asp:HiddenField ID="Snsurl_Search_Renren_Hidden" runat="server" />
        <asp:HiddenField ID="Snsurl_Account_Renren_Hidden" runat="server" />
        <asp:HiddenField ID="Snsurl_Search_Kaixin_Hidden" runat="server" />
        <asp:HiddenField ID="Snsurl_Account_Kaixin_Hidden" runat="server" />
        <asp:HiddenField ID="Snsurl_Search_Weibo_Hidden" runat="server" />
        <asp:HiddenField ID="Snsurl_Account_Weibo_Hidden" runat="server" />
        <asp:HiddenField ID="Search_Baidu_Hidden" runat="server" />
        <asp:HiddenField ID="MoveFlg" runat="server" />
        <asp:HiddenField ID="Url_Scheme_Hidden" runat="server" />
        <asp:HiddenField ID="Url_Schemes_Hidden" runat="server" />
        <%-- 2012/06/01 TCS 河原 FS開発 END --%>

        <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
        <asp:HiddenField ID="mainteamount" runat="server" />
        <%-- 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>

        <%-- 2017/11/20 TCS 河原 TKM独自機能開発 START --%>
        <asp:UpdatePanel ID="CleansingUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- 事前クレンジング結果 -->
                <asp:HiddenField ID="CleansingResult" runat="server" Value="0" />
           
                <!-- 顧客編集ポップアップ表示時にクレンジングモードで開くかどうか -->
                <asp:HiddenField ID="CleansingModeFlg" runat="server" Value="0" />
           
                <!-- 顧客編集ポップアップ表示中にクレンジングモードかどうか -->
                <asp:HiddenField ID="CleansingMode" runat="server" Value="0" />
           
                <!-- 顧客編集ポップアップ表示中にクレンジングモードかどうか -->
                <asp:HiddenField ID="Use_Customerdata_Cleansing_Flg" runat="server" Value="" />
                 
                <asp:HiddenField ID="cust_flg_hidden" runat="server" Value="" />
             
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:HiddenField ID="Use_Direct_Billing_Flg" runat="server" Value="" />
        <%-- 2017/11/20 TCS 河原 TKM独自機能開発 END --%>
    </div>
