<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080202.ascx.vb" Inherits="Pages_SC3080202" %>

<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3080202.ascx                                                                              %>
<%'──────────────────────────────────────────────%>
<%'機能： 顧客詳細(商談情報)                                                                   %>
<%'補足：                                                                                      %>
<%'作成： 2011/11/24 TCS 小野                                                                  %>
<%'更新： 2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応                                    %>
<%'更新： 2012/04/26 TCS 河原 HTMLエンコード対応                                               %>
<%'更新： 2013/03/06 TCS 河原 GL0874                                                           %>
<%'更新： 2013/06/30 TCS 黄 A STEP2】i-CROP新DB適応に向けた機能開発(既存流用)                  %>
<%'更新： 2013/12/09 TCS 市川 Aカード情報相互連携開発                                          %>
<%'更新： 2014/02/12 TCS 山口 受注後フォロー機能開発                                           %>
<%'更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応                                         %>
<%'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発                  %>
<%'更新： 2017/11/20 TCS 河原 TKM独自機能開発                                                  %>
<%'更新： 2018/04/18 TCS 前田 (トライ店システム評価)基幹連携を用いたセールス業務実績入力の検証 %>
<%'更新： 2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1     %>
<%'更新： 2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証                 %>
<%'更新： 2018/12/18 TCS 舩橋 TKM-UAT課題No.132 Demand structureの見出しを赤字に変更           %>
<%'更新： 2019/05/09 TS  村井 (FS)納車時オペレーションCS向上にむけた評価（サービス）UAT-0028   %>
<%'更新： 2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証                      %>
<%'更新： 2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)  %>
<%'──────────────────────────────────────────────%>

<link rel="stylesheet" href="../Styles/SC3080202/SC3080202.Popup.css?202002040000" type="text/css" media="screen,print" />
<link rel="stylesheet" href="../Styles/SC3080202/SC3080202.css?201812190000" type="text/css" media="screen,print" />
<script src="../Scripts/SC3080202/SC3080202.Common.js?202009300000" type="text/javascript"></script>
<script src="../Scripts/SC3080202/SC3080202.Series.js?201807100000" type="text/javascript"></script>
<%--2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE --%>
<script src="../Scripts/SC3080202/SC3080202.Condition.js?20120312000000" type="text/javascript"></script>

<%'メインコンテンツ %>
    <div id="scNscOneBoxContentsArea" class="contentsFrame scNscOneBoxContentsArea">
		<h2 class="contentTitle clip">
            <icrop:CustomLabel ID="TitleLabel" runat="Server" TextWordNo="20002" Width="170px" />
        </h2>
		
		<div class="scNscLeftContentsBox">
            
            <%'最新活動 ここから %>
			<div class="scNscNewActionBox">
				<h3 class="Blue ellipsis">
                    <%--2019/05/09 TS  村井 (FS)納車時オペレーションCS向上にむけた評価（サービス）START--%>
                    <icrop:CustomLabel ID="NewActivityLabel" runat="Server" TextWordNo="20003" Width="100px" style="text-align:center;" />
                    <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
                    <icrop:CustomLabel ID="AcardNumOrContractNumTitle" runat="Server" TextWordNo="20048" Width="107px" style="text-align:center;" />
                    <icrop:CustomLabel ID="AcardNumOrContractNumValue" runat="Server" Width="250px" UseEllipsis="true" style="text-align:center;" />
                    <%--2019/05/09 TS  村井 (FS)納車時オペレーションCS向上にむけた評価（サービス）END--%>
                    <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
                    <asp:Button ID="AcardNumOrContractNumDummyButton" runat="server" style="display:none" />
                    <asp:HiddenField ID="EstimateIdHidden" runat="server" />
                    <asp:HiddenField ID="ContractNoFlgHidden" runat="server" />
                    <asp:HiddenField ID="TcvRedirectFlgHidden" runat="server" />
                    <asp:HiddenField ID="AcardNumOrContractNumRedirectFlg" runat="server" />
                    <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
                    <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
                </h3>
                <asp:Button ID="commitActivityButtonDummy" runat="server" style="display:none" />
                <table>
                    <tr>
                        <td class="ActionBoxName ellipsis" >
                            <asp:label ID="dispContactname" runat="Server" />
                        </td>
                        <td class="ActionBoxTime ellipsis">
                            <asp:label ID="dispSalesStartTime" runat="server" />
                        </td>
                        <td class="ActionBoxMember clip">
                            <asp:label ID="dispWalkinnum" runat="Server" />
                        </td>
                        <td class="ActionBoxHuman ellipsis">
                            <asp:label ID="dispAccount" runat="Server" />
                            <asp:HiddenField runat="server" ID="accountOperationHidden" />
                        </td>
                    </tr>
                </table>
			</div>
            <%'最新活動 ここまで %>

            <%'2017/11/20 TCS 河原 TKM独自機能開発 START %>
            <%'直販フラグ %>
            <asp:Panel runat="server" id="dispSelectedDirectBilling" class="NotDirectBilling" >
                <input type="checkbox" id="SelectedDirectBilling" disabled="True" runat="server" />
                <icrop:CustomLabel ID="CustomLabel25" runat="Server" TextWordNo="20084" />
                <asp:Button runat="server" ID="CommitdDirectBillingDummy" Text="CommitdDirectBillingDummy" OnClick="CommitdDirectBillingDummy_Click" style="display:none;" />
            </asp:Panel>
            <asp:UpdatePanel runat="server" id="dispSelectedDirectBillingPanel" class="" >
                <Triggers >
                    <asp:AsyncPostBackTrigger ControlID="CommitdDirectBillingDummy" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <%'2017/11/20 TCS 河原 TKM独自機能開発 END %>

            <%'希望車種エリアここから %>
            <%'2013/12/09 TCS 市川 Aカード情報相互連携開発 START %>
            <div id="scNscSelectCarAreaCover" style="visibility:hidden;"></div>
            <%'2013/12/09 TCS 市川 Aカード情報相互連携開発 END %>
            <div class="scNscSelectCarArea" runat="server" id="scNscSelectCarArea">
                <%'2018/04/18 TCS 前田 (トライ店システム評価)基幹連携を用いたセールス業務実績入力の検証 START %>
                <asp:HiddenField ID="useFlgSuffix" runat="server" />
                <asp:HiddenField ID="useFlgInteriorColor" runat="server" />
                <%'2018/04/18 TCS 前田 (トライ店システム評価)基幹連携を用いたセールス業務実績入力の検証 END %>
                <asp:UpdatePanel id="scNscSelectCarAreaBtnUpdatePanel" runat="server" >
                    <ContentTemplate>
                        <div style="display:none">
                            <asp:Button ID="commitCompleteSelectedSeriesButtonDummy" runat="server" />
                            <asp:Button ID="commitCompleteSeriesQuantiryButtonDummy" runat="server" />
                        </div>
                    </ContentTemplate>
                    <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
                    <Triggers >
                        <asp:AsyncPostBackTrigger ControlID="MostPreferredUpdateDummyButton" EventName="Click" />  
                    </Triggers>
                    <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
                </asp:UpdatePanel>
				<h3>
                    <icrop:CustomLabel ID="CustomLabel1" runat="Server" TextWordNo="20006" class="clip" Width="60px" />
                </h3>

                <asp:UpdatePanel ID="ScNscSelectCarAreaUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
				        <ul class="scNscSelectCarButtonList">
                            <li class="scNscSelectCarButtonArrowFor" id="scNscSelectCarButtonArrowFor"></li>
                    
                            <asp:Repeater runat="server" ID="SelectedCarRepeater" ClientIDMode="Predictable">
                                <ItemTemplate>
                                    <div>
                                        <%'希望車種の連番 %>
                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                        <li class="scNscSelectCarButton NotMost"
                                         id="scNscSelectCarButton" seqno="<%# HttpUtility.HtmlEncode(Eval("SEQNO")) %>"></li>
                                        <asp:hiddenfield runat="server" id="modelcdHidden" value='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>' />
                                        <asp:hiddenfield runat="server" id="modelnmHidden" value='<%# HttpUtility.HtmlEncode(Eval("SERIESNM")) %>' />
                                        <asp:hiddenfield runat="server" id="gradecdHidden" value='<%# HttpUtility.HtmlEncode(Eval("MODELCD")) %>' />
                                        <asp:hiddenfield runat="server" id="gradenmHidden" value='<%# HttpUtility.HtmlEncode(Eval("VCLMODEL_NAME")) %>' />
                                        <asp:hiddenfield runat="server" id="suffixcdHidden" value='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_CD")) %>' />
                                        <asp:hiddenfield runat="server" id="suffixnmHidden" value='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_NAME")) %>' />
                                        <asp:hiddenfield runat="server" id="exteriorColorcdHidden" value='<%# HttpUtility.HtmlEncode(Eval("COLORCD")) %>' />
                                        <asp:hiddenfield runat="server" id="exteriorColornmHidden" value='<%# HttpUtility.HtmlEncode(Eval("DISP_BDY_COLOR")) %>' />
                                        <asp:hiddenfield runat="server" id="InteriorColorcdHidden" value='<%# HttpUtility.HtmlEncode(Eval("INTERIORCLR_CD")) %>' />
                                        <asp:hiddenfield runat="server" id="InteriorColornmHidden" value='<%# HttpUtility.HtmlEncode(Eval("INTERIORCLR_NAME")) %>' />
                                        <asp:hiddenfield runat="server" id="sateiHidden" value="1万元" />
                                        <asp:hiddenfield runat="server" id="quantityHidden" value='<%# HttpUtility.HtmlEncode(Eval("QUANTITY")) %>' />
                                        <asp:hiddenfield runat="server" id="pictureHidden" value='<%# HttpUtility.HtmlEncode(Eval("PICIMAGE")) %>' />
                                        <asp:hiddenfield runat="server" id="logoImageHidden" value='<%# HttpUtility.HtmlEncode(Eval("LOGOIMAGE")) %>' />
                                        <asp:hiddenfield runat="server" id="seqnoHidden" value='<%# HttpUtility.HtmlEncode(Eval("SEQNO")) %>' />
                                        <%'2013/06/30 TCS 趙 2013/10対応版 既存流用 START%>
                                        <asp:hiddenfield runat="server" id="lockvrHidden" value='<%# HttpUtility.HtmlEncode(Eval("ROWLOCKVERSION")) %>' />
                                        <%'2013/06/30 TCS 趙 2013/10対応版 既存流用 END%>
                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                        <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
                                        <asp:HiddenField runat="server" ID="isMostPreferredHidden" Value='<%# HttpUtility.HtmlEncode(Eval("MOST_PREF_VCL_FLG")) %>' />
                                        <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%'希望車種追加ボタン %>
                            <li class="plus" id="plus">
                                <img alt="" src="../Styles/Images/SC3080202/scNscIconSelectCarPuls.png" width="13" height="13" />
                            </li>
                            <%'希望車種次ページ %>
                            <li class="scNscSelectCarButtonArrow" id="scNscSelectCarButtonArrow"></li>
                            <asp:HiddenField runat="server" ID="startPosHidden" />
                            <asp:HiddenField runat="server" ID="endPosHidden" />
                            <asp:HiddenField runat="server" ID="selectPosHidden" />
                            <asp:HiddenField runat="server" ID="selSeqnoHidden" />
                            <asp:HiddenField runat="server" ID="selModelcdHidden" />
                            <asp:HiddenField runat="server" ID="selGradecdHidden" />
                            <asp:HiddenField runat="server" ID="selSuffixcdHidden" />
                            <asp:HiddenField runat="server" ID="selExteriorColorcdHidden" />
                            <asp:HiddenField runat="server" ID="selInteriorColorcdHidden" />
                            <asp:HiddenField runat="server" ID="selColorcdHidden" />
                            <%'2013/06/30 TCS 黄 2013/10対応版　既存流用 START%>
                            <asp:HiddenField runat="server" ID="selLockvrHidden" />
                            <%'2013/06/30 TCS 黄 2013/10対応版　既存流用 END%>
                            <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
                            <asp:HiddenField runat="server" ID="selMostPreferredHidden" />
                            <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
				        </ul>                        
                        <%--2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 DELETE--%>
                    </ContentTemplate>
                </asp:UpdatePanel>

                
                
				<p class="clearboth"></p>
                <asp:panel runat="server" id="dispSelectedMostPreferred" class="NotMost" >
                    <icrop:CustomLabel ID="CustomLabel16" runat="Server" TextWordNo="20049" />
                </asp:panel>
                <asp:Button runat="server" ID="MostPreferredUpdateDummyButton" Text="mostPreferredUpdateDummyButton" OnClick="MostPreferredUpdateDummyButton_Click" style="display:none;" />
                <div id="scNscCarSelectArea1">
                    <%'選択希望車種 車両画像 %>
					<div class="scNscCarPictureArea">
                        <img runat="server" id="dispSelectedPicture" alt="" src="dummy.jpg" />
                    </div>
                    <%'選択希望車種 ステータス %>
					<div class="scNscCarStatusArea">
						<ul>
                            <li style="" ><!--MostPrefered用空間-->&nbsp;</li>
                            <li class="scNscCarNameLogo">
                                <img runat="server" id="dispSelectedLogo" alt="" src="dummy.jpg" />
                            </li>
							<li class="scNscCarIconStar clip">
                                <icrop:CustomLabel ID="dispSelectedModel" runat="server" />
                            </li>
                            <li class="clip">
                                <icrop:CustomLabel ID="dispSelectedSuffix" runat="server" />
                            </li>
							<li class="scNscCarIconColor clip">
                                <icrop:CustomLabel ID="dispSelectedColor" runat="server" />
                            </li>
							<li> 
                            <asp:Label ID="scNscCarIconCar" runat="server" class="scNscCarIconCar clip">
                                <asp:Label ID="dispSelectedQuantity" runat="server" class="scNscCarIconCarTapArea" />
                                <icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="20007" class="scNscCarIconCarTapArea" />
                                <asp:HiddenField ID="inputSelectQuantiryHidden" runat="server" />
                            </asp:Label>
                            </li>
						</ul>
					</div>
                </div>
                <p class="clearboth" />
                <%'2017/11/20 TCS 河原 TKM独自機能開発 END%>
			</div>
            <%'希望車種なしの場合に表示されるエリア %>
            <div id="scNscCarSelectArea2">
                <div class="scNsc51MainSample clip" runat="server" id="scNsc51MainSample">
                    <h3>
                        <icrop:CustomLabel ID="CustomLabel3" runat="Server" TextWordNo="20029" UseEllipsis="False" />
                    </h3>
                </div>
            </div>
            <%'希望車種エリアここまで %>

            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
            <asp:Panel runat="server" ID="AfterOdrPrcsCompeCarPanel" >
            <%'競合車種エリア ここから %>
            <div id="scNscCompeCarArea">
                <asp:UpdatePanel id="ScNscCompeCarAreaUpdatePanelButton" runat="server">
                    <ContentTemplate>
                        <div style="display:none"><asp:button ID="commitCompleteSelectedCompButtonDummy" runat="server" /></div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%'競合車種表示エリア %>
                <asp:UpdatePanel ID="ScNscCompeCarAreaUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="scNscCompetingCarArea" runat="server" id="dispCompeCarCountFlg" >
                            <div id="scNscCompetingCarAreaInner" class="normalMode ellipsis">

                                
                                <icrop:CustomLabel ID="scNscTitleCompeCar" runat="server" CssClass="titleCompeCar" TextWordNo="20008" class="clip"/>
                                <div id="ScNscCompeCarScrollPane">
                                    <table id="compeTable">
                                        <asp:Repeater runat="server" id="CompeRepeater">
                                            <ItemTemplate>
                                                <tr class="ellipsis">
                                                    <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                    <%'メーカー名 %>
                                                    <td class="titleCompeMaker">
                                                        <asp:Label ID="Label266" runat="Server" text='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKER")) %>'/>
                                                        <%'隠し項目エリア %>
                                                        <div class="scNscCompetingCarAreaHidden" style="display:none">
                                                            <asp:HiddenField ID="CompCdHidden" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>'  />
                                                            <asp:HiddenField ID="CompMakerCd" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKERNO")) %>' />
                                                        </div>
                                                    </td>
                                                    <%'セパレータ %>
                                                    <td class="titleCompeSp"><icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="20009"/></td>
                                                    <%'モデル名 %>
                                                    <td class="titleCompeModel"><asp:Label ID="Label666" runat="Server" text='<%# HttpUtility.HtmlEncode(Eval("COMPETITORNM")) %>'/></td>
                                                    <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>

                                <p class="moreCarEvent">
                                    <%'拡大 %>
                                    <a href="javascript:void(0)" class="scNscCompetingCarOther" id="bigSizeLinkButton">
                                        <asp:label runat="server" id="competingOtherCount" />
                                        <asp:HiddenField runat="server" ID="otherCountHidden" />
                                        <img alt="" src="../Styles/Images/SC3080202/Triangular.png" />
                                    </a>
                                    <%'縮小 %>
                                    <a href="javascript:void(0)" class="scNscCompetingCarOther" id="normalSizeLinkButton">
                                        <img alt="" src="../Styles/Images/SC3080202/Triangular02.png" />
                                    </a>
                                </p>
                            </div>
                        </div>

                        <%'競合車種なしの場合に表示されるエリア %>
                        <div class="scNscEntryOtherMakerCar clip" runat="server" id="dispCompeCarCountNoFlg">
                            <icrop:CustomLabel ID="CustomLabel5" runat="Server" TextWordNo="20034" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
                <icrop:NumericBox ID="tt" runat="server" ></icrop:NumericBox></div><%'競合車種エリア ここまで %><%'条件項目ヘッダーエリア ここから %><div class="scNscCarConditionSelectArea">
				<h3 class="clip">
                    <icrop:CustomLabel ID="WordLiteral5" runat="Server" TextWordNo="20011" />
                </h3>
                <div runat="server" id="salesConditionEditMode" style="display:none">
					<p class="scNscCarConditionSelectButtonCancel clip" runat="server" id="salesConditionCancel">
                        <icrop:CustomLabel ID="WordLiteral6" runat="Server" TextWordNo="20013" UseEllipsis="False" />
                    </p>
                    <asp:UpdatePanel id="ScNscCarConditionSelectAreaUpdatePanel" runat="server">
                        <ContentTemplate>
                            <p class="scNscCarConditionSelectButtonComplete clip" ID="salesConditionCompleteButton" runat="server">
                                <icrop:CustomLabel ID="CustomLabel7" runat="Server" TextWordNo="20014" UseEllipsis="False" />
                                <div style="display:none"><asp:button ID="salesConditionCompleteButtonDummy" runat="server" /></div>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div runat="server" id="salesConditionCurrentMode">
                    <p class="scNscCarConditionSelectButtonComplete clip">
                        <icrop:CustomLabel ID="WordLiteral12" runat="Server" TextWordNo="20012"/>
                    </p>
                </div>
			</div>
            <%'条件項目ヘッダーエリア ここまで %>

            <%'条件項目表示エリア %>
            <%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
            <div id="conditionAreaFrame">
                <%'購入分類エリア %>
                <div runat="server" id="demandStructureArea">
                    <%'購入分類ヘッダーエリア %>
                    <div id="demandStructureHeader" class="scNscDemandStructureSelectList clip scNscCarConditionSelectList ">
                        <h4 class="clip">
                            <%-- '2018/12/18 TCS 舩橋 TKM-UAT課題No.145 Demand structureの見出しを赤字に変更 START --%>
                            <icrop:CustomLabel ID="DemandStructureLabel" runat="server" TextWordNo="2020007" class="mandatory fontsmallabel"/>
                            <%-- '2018/12/18 TCS 舩橋 TKM-UAT課題No.145 Demand structureの見出しを赤字に変更 END   --%>
                        </h4>
                        <ul>
                            <%'購入分類項目のループ %>
                            <asp:Repeater runat="server" id="DemandStructureItemRepeater" ClientIDMode="Predictable">
                                <ItemTemplate>
                                    <li id="inputCondition">
                                        <asp:label ID="Label2" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("DEMAND_STRUCTURE_NAME")) %>' class="ellipsis" />
                                        <asp:HiddenField id="DemandCdHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("DEMAND_STRUCTURE_CD")) %>' /> 
                                        <asp:HiddenField id="TradeinEnabledFlgHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("TRADEINCAR_ENABLED_FLG")) %>'/> 
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <p class="clearboth"></p>
                    </div>
                
                    <%'購入分類ヘッダーエリア ここまで %>
                    <%'下取り車両エリア %>
			    	<table border="0" cellpadding="0" cellspacing="0" class="ncs5001TitleTable" style="margin-bottom:15px;">
			    		<tr>
			    			<th class="column1 tableHeader1 ncs5001TitleTable" align="center" valign="middle">
                                <icrop:CustomLabel ID="Trade_in_MakerLabel" runat="server" TextWordNo="2020008" />
                            </th>
			    			<th class="column2 tableHeader2 ncs5001TitleTable" align="center" valign="middle">
                                <icrop:CustomLabel ID="Trade_in_ModelLabel" runat="server" TextWordNo="2020009" />
                            </th>
			    			<th class="column3 tableHeader3 ncs5001TitleTable" align="center" valign="middle">
                                <icrop:CustomLabel ID="Trade_in_MileageLabel" runat="server" TextWordNo="2020010" />
                            </th>
			    			<th class="column4 tableHeader5 ncs5001TitleTable" align="center" valign="middle">
                                <icrop:CustomLabel ID="Trade_in_ModelYearLabel" runat="server" TextWordNo="2020004" />
                            </th>
			    		</tr>
			    		<tr>
			    			<th class="column1 tableItem1 ColorWhite " align="center" valign="middle" id="Trade_in_MakerTrigger">
                                <asp:label ID="Trade_in_Maker" runat="server" class="ellipsis" style="display:inline-block;width:100px;margin: 0px -30px;"/>
                            </th>
			    			<th class="column2 tableItem2 ColorWhite" align="center" valign="middle" id="Trade_in_ModelTrigger">
                                <asp:label ID="Trade_in_Model" runat="server" TextWordNo="" class="ellipsis" style="display:inline-block;width:140px;margin: 0px -30px;" />
                            </th>
			    			<th class="column3 tableItem3 ColorWhite" align="center" valign="middle" id="Trade_in_MileageTrigger">
                                <asp:label ID="Trade_in_Mileage" runat="server" class="ellipsis" style="display:inline-block;width:60px;margin: 0px -30px;"/>
                            </th>
			    			<th class="column4 tableItem5 ColorWhite" align="center" valign="middle" id="Trade_in_ModelYearTrigger">
                                <asp:label ID="Trade_in_ModelYear" runat="server" class="ellipsis" stylestyle="display:inline-block;width:100px;margin: 0px -30px;"/>
                            </th>
			    		</tr>
			    	</table>
                    <%'下取り車両エリア ここまで %>
                </div>
                <%'購入分類エリア ここまで %>
                <div runat="server" id="conditionArea">
                    <%'第1ループ %>
                    <asp:Repeater runat="server" id="ConditionRepeater" ClientIDMode="Predictable" >
                        <ItemTemplate>
                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                            <div class="scNscCarConditionSelectList clip">
                                <h4><asp:label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("TITLE")) %>' CssClass='<%# HttpUtility.HtmlEncode(Eval("TITLE_CSSCLASS")) %>' /></h4>
                                <asp:HiddenField id="SalesConditionNoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("SALESCONDITIONNO")) %>'/>
                                <asp:HiddenField id="AndOrHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("AND_OR")) %>'/> 
			    			    <ul>
                                    <%'第2ループ %>
                                    <asp:Repeater runat="server" id="ConditionItemRepeater" ClientIDMode="Predictable">
                                        <ItemTemplate>
			    			                <li id="inputCondition">
                                                <asp:label ID="Label2" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ITEMTITLE")) %>' class="ellipsis" />
                                                <asp:HiddenField id="SalesConditionNoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("SALESCONDITIONNO")) %>' /> 
                                                <asp:HiddenField id="ItemNoHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("ITEMNO")) %>' /> 
                                                <asp:HiddenField id="CheckFlgHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("CHECKFLG")) %>'/> 
                                                <asp:HiddenField id="OtherHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("OTHER")) %>' /> 
                                                <asp:HiddenField id="OtherSalesConditionHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("OTHERSALESCONDITION")) %>' /> 
                                                <asp:HiddenField id="DefaultItemTitle" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("DEFAULT_ITEMTITLE")) %>' /> 
                                                <asp:HiddenField id="DefaultOtherSalCondHidden" runat="server" value='<%# HttpUtility.HtmlEncode(Eval("OTHERSALESCONDITION")) %>' /> 
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
			    			    </ul>
			    			    <p class="clearboth"></p>
                            </div>
                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                    </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END --%>
            </asp:Panel>
            <asp:Panel runat="server" ID="AfterOdrPrcsCVDIPanel" >
            <%'契約車種詳細情報エリア %>
            <div class="ContractVehicleDetailInfo">
            <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
            <asp:HiddenField ID="DispFlgActStatus" runat="server" />
            <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>
                <ul class="ContractVehicleDetailInfoBox">
                <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
                <asp:HiddenField ID="Flg_On" Value="1" runat="server"/>
                <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>
                    <li class="Title ellipsis"><icrop:CustomLabel ID="CustomLabel17" runat="Server" TextWordNo="20081"/></li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel18" runat="Server" TextWordNo="20054" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIBookingDate" runat="Server"/></div></div>                        
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel19" runat="Server" TextWordNo="20055" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDISuffix" runat="Server" /></div></div>
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel20" runat="Server" TextWordNo="20056" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVIN" runat="Server" /></div></div>
                    </li>
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel11" runat="Server" TextWordNo="20057" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVehicleStatus" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                        <icrop:CustomLabel ID="CustomLabel24" runat="Server" TextWordNo="20058" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIProductionDate" runat="Server" /></div></div>
                    </li>
                    <% End If%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END%>
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>

                    <li class="Title ellipsis"><icrop:CustomLabel ID="CustomLabel40" runat="Server" TextWordNo="20053" /></li>
                    
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li class="GrayBar">
                        <icrop:CustomLabel ID="CustomLabel26" runat="Server" TextWordNo="20059" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIFinanceStatus" runat="Server" /></div></div>
                    </li>
                    <% End If%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END%>
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>

                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel27" runat="Server" TextWordNo="20060" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIFinanceApplicationDate" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                        <icrop:CustomLabel ID="CustomLabel29" runat="Server" TextWordNo="20062" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIFinanceApprovalDate" runat="Server" /></div></div>
                    </li>
                    <li class="GrayBar">
                        <icrop:CustomLabel ID="CustomLabel30" runat="Server" TextWordNo="20063" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIMatchingStatus" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                    <%Else %>
                    <li>
                    <% End If%>
                    <icrop:CustomLabel ID="CustomLabel28" runat="Server" TextWordNo="20064" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIMatchingDate" runat="Server" /></div></div>
                    </li>

                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li class="GrayBar">
                        <icrop:CustomLabel ID="CustomLabel31" runat="Server" TextWordNo="20065" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVDQIStatus" runat="Server" /></div></div>
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel32" runat="Server" TextWordNo="20066" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVDQIOrderDate" runat="Server" /></div></div>
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel33" runat="Server" TextWordNo="20067" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVDQIStartDate" runat="Server" /></div></div>
                    </li>
                    <% End If%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END%>
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>
                    
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START%>
                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li>
                       <icrop:CustomLabel ID="CustomLabel34" runat="Server" TextWordNo="20068" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIVDQIFinishDate" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                        <icrop:CustomLabel ID="CustomLabel35" runat="Server" TextWordNo="20069" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIPDSFinishDate" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                        <icrop:CustomLabel ID="CustomLabel36" runat="Server" TextWordNo="20070" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIInsuranceIssueDate" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                    <% Else%>
                    <li>
                    <% End If%>
                    <icrop:CustomLabel ID="CustomLabel37" runat="Server" TextWordNo="20071" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIDeliveryDateTime" runat="Server" /></div></div>
                    </li>

                    <% If DispFlgActStatus.Value.Equals(Flg_On.Value) Then%>
                    <li class="GrayBar">
                        <icrop:CustomLabel ID="CustomLabel38" runat="Server" TextWordNo="20072" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIRegistrationStatus" runat="Server" /></div></div>
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel39" runat="Server" TextWordNo="20073" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIRegDocCollectionDate" runat="Server" /></div></div>
                    </li>
                    <li>
                        <icrop:CustomLabel ID="CustomLabel43" runat="Server" TextWordNo="20074" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIRegPlateDocComplDate" runat="Server" /></div></div>
                    </li>
                    <li class="Margin01">
                        <icrop:CustomLabel ID="CustomLabel44" runat="Server" TextWordNo="20075" CssClass="ColumnName ellipsis" />
                        <div class="BoxMain"><div class="BoxBorder ellipsis"><icrop:CustomLabel ID="CVDIRegistrationHandoverDate" runat="Server" /></div></div>
                    </li>
                    <% End If%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL%>
                    <%'2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END%>
                    <%'2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END%>
                </ul>
            </div>
            <%'契約車種詳細情報エリア ここまで%>
            </asp:Panel>
            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
		</div>
		<!-- 左カラム end -->

		<!-- 右カラム -->
		<div class="scNscRightContentsBox">
            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
            <asp:Panel runat="server" ID="AfterOdrPrcsSalesParametersPanel" >
                <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
                <dl class="salesParameters">
                <%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除 %>
                <%'用件ソース1st %>
                    <dt><icrop:CustomLabel ID="Source1TitleLabel" runat="Server" TextWordNo="20051"  class="mandatory"/></dt>
                    <dd id="Source1SelectPopupTrigger" style="position:relative">
                        <div id="Source1SelectedNameCover" class="salesParameterCover"></div>
                        <div id="Source1SelectedName" style="height:100%;">
                            <asp:UpdatePanel runat="server" ID="Source1ListUpdatePanel" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <icrop:CustomLabel runat="server" ID="Source1SelectedNameLabel" UseEllipsis="True" Width="275px" CssClass="ellipsis" />
                                    <asp:HiddenField runat="server" ID="Source1SelectedCodeHidden" Value="0" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="CommitSource1ButtonDummy" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>                     
                        </div>
                        <asp:Button runat="server" ID="CommitSource1ButtonDummy" Text="CommitSource1ButtonDummy" OnClick="CommitSource1ButtonDummy_Click" style="display:none;" />
                        <icrop:PopOverForm runat="server" ID="popOverSource1List" TriggerClientId="Source1SelectedName" HeaderStyle="None" 
                        OnClientRender="renderPopOver_Source1" OnClientClose="closePopOver_Source1" OnClientOpen="openPopOver_Source1"
                        Width="300px" Height="200px" />                        
                    </dd>
                <%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start %>
                <%'用件ソース2nd %>
                    <dt><icrop:CustomLabel ID="Source2TitleLabel" runat="Server" TextWordNo="2020011" class="mandatory" /></dt>
                    <dd id="Source2SelectPopupTrigger" style="position:relative">
                        <div id="Source2SelectedNameCover" class="salesParameterCover"></div>
                        <div id="Source2SelectedName" style="height:100%;">
                            <asp:UpdatePanel runat="server" ID="Source2ListUpdatePanel" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <icrop:CustomLabel runat="server" ID="Source2SelectedNameLabel" UseEllipsis="True" Width="275px" CssClass="ellipsis" />
                                    <asp:HiddenField runat="server" ID="Source2SelectedCodeHidden" Value="0" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="CommitSource2ButtonDummy" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>                     
                        </div>
                        <asp:Button runat="server" ID="CommitSource2ButtonDummy" Text="CommitSource2ButtonDummy" OnClick="CommitSource2ButtonDummy_Click" style="display:none;" />
                        <icrop:PopOverForm runat="server" ID="popOverSource2List" TriggerClientId="Source2SelectedName" HeaderStyle="None" 
                        OnClientRender="renderPopOver_Source2" OnClientClose="closePopOver_Source2" OnClientOpen="openPopOver_Source2"
                        Width="300px" Height="200px" />                        
                    </dd>
                <%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end %>
                </dl>
            </asp:Panel>
            <asp:Panel runat="server" ID="BeforeAfterOdrPrcsSwitchButtonPanel">
                <div class="BeforeAfterOdrPrcsSwitchButton">
		            <div class="LeftOdrPrcsButton clip PrcsButtonOff"><icrop:CustomLabel ID="CustomLabel41" runat="Server" TextWordNo="20076" /></div>
		            <div class="RightOdrPrcsButton clip PrcsButtonOn"><icrop:CustomLabel ID="CustomLabel42" runat="Server" TextWordNo="20077" /></div>
	            </div>
            </asp:Panel>
            <%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
            <%'プロセスエリア ここから %>
			<div class="scNscProcessAndStatusArea">
				<div class="scNscProcessAndStatusTitleArea">
					<h3 class="scNscTitleProcess clip">
                        <icrop:CustomLabel ID="WordLiteral8" runat="Server" TextWordNo="20017" />
                    </h3>
					<h3 class="scNscTitleStatus clip">
                        <icrop:CustomLabel ID="WordLiteral9" runat="Server" TextWordNo="20018" />
                    </h3>
					<p class="clearboth"></p>
				</div>
				<div class="scNscProcessAndStatusBox">
                    <!-- ' 2012/02/29 TCS 小野 【SALES_2】 START -->
                        <div class="LeftArrow" id="ProcessLeftArrow"></div>
                        <div class="RightArrow" id="ProcessRightArrow"></div>
                        <div id="dispProcessArea" class="dispProcessArea">
                        <div id="dispProcessAreaInner" class="dispProcessAreaInner">
                        <!-- ' 2012/02/29 TCS 小野 【SALES_2】 END -->
					<div class="scNscProcessIconList">
						<ul class="scNscProcessIconListUl">
                            <li class="scNscProcessIconListDocuments clip" runat="server" id="dispProcessCatalog">
                                <asp:Label ID="dispProcessCatalogLabel" runat="server" Text="" />
                            </li>
							<li class="scNscProcessIconListCar01 clip" runat="server" id="dispProcessTestdrive">
                                <asp:Label ID="dispProcessTestdriveLabel" runat="server" Text="" />
                            </li>
							<li class="scNscProcessIconListCar02 clip" runat="server" id="dispProcessEvaluation">
                                <asp:Label ID="dispProcessEvaluationLabel" runat="server" Text="" />
                            </li>
							<li class="scNscProcessIconListPrice clip" runat="server" id="dispProcessQuotation">
                                <asp:Label ID="dispProcessQuotationLabel" runat="server" Text="" />
                            </li>
                            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
                            <asp:Repeater runat="server" id="ProcessBookedAfterRepeater" ClientIDMode="Predictable">
                                <ItemTemplate>
                                    <li class="ProcessBookedAfterList clip" runat="server" id="ProcessBookedAfterLi">
                                        <span class="ProcessBookedAfterNoUse"></span>
                                        <asp:Label ID="ProcessBookedAfterTitleLabel" runat="server" Text="" />
                                        <asp:HiddenField runat="server" ID="ProcessBookedAfterCheckFlg" value='<%# HttpUtility.HtmlEncode(Eval("CHECKFLG")) %>'/>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
                        </ul>
                        <ul class="ProcessHiddenField" style="display:none">
                            <asp:Repeater runat="server" id="ProcessRepeater" ClientIDMode="Predictable">
                                <ItemTemplate>
                                    <li>
                                        <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                        <%'Hiddenの配置順を変更するとScriptでエラーが発生するので注意%>
                                        <asp:HiddenField runat="server" id="ProcessSeqHidden" value='<%# HttpUtility.HtmlEncode(Eval("SEQNO")) %>' />
                                        <asp:HiddenField runat="server" id="ProcessCatalogHidden" value='<%# HttpUtility.HtmlEncode(Eval("CATALOGDATE")) %>' />
                                        <asp:HiddenField runat="server" id="ProcessTestdriveHidden" value='<%# HttpUtility.HtmlEncode(Eval("TESTDRIVEDATE")) %>' />
                                        <asp:HiddenField runat="server" id="ProcessEvaluationHidden" value='<%# HttpUtility.HtmlEncode(Eval("EVALUATIONDATE")) %>' />
                                        <asp:HiddenField runat="server" id="ProcessQuotationHidden" value='<%# HttpUtility.HtmlEncode(Eval("QUOTATIONDATE")) %>' />
                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                        <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
						<p class="clearboth" />
					</div>
                    <!-- ' 2012/02/29 TCS 小野 【SALES_2】 START -->
                    </div>
                    </div>
                    <!-- ' 2012/02/29 TCS 小野 【SALES_2】 END -->
					<div class="scNscStatusIconList">
                        <!-- ' 2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応 START-->
                        <img runat="server" id="CrActResult" class="scNscStatusIcon" alt="" src="" width="50" height="49" /> <!-- ' 2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応 END--></div><p class="clearboth" />
				</div>
                <asp:HiddenField runat="server" ID="AfterOdrPrcsIconCountHidden"/>
                <asp:HiddenField runat="server" ID="AfterOdrPrcsIconMaxPageHidden"/>
                <asp:HiddenField runat="server" ID="AfterOdrPrcsIconPageHidden"/>
			</div>
            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
            <%'プロセスエリア ここまで %>
			<div class="scNsc50Memo" >
				<h3 class="clip">
                    <icrop:CustomLabel ID="WordLiteral10" runat="Server" TextWordNo="20019" />
                </h3>

                <%'メモ更新用ダミーボタン %>
                <asp:UpdatePanel id="scNsc50MemoUpdatePanel" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Button ID="commitTodayMemoButtonDummy" runat="server" style="display:none" />
                    </ContentTemplate>
                </asp:UpdatePanel>  


                <%'メモエリア Start %>
				<div class="scNsc50MemoArea">
                    <%'当日メモ Start %>
					<div class="scNsc50MemoTop">
						<div class="scNsc50MemoInBox InBoxMemo ellipsis">
							<h4><icrop:CustomLabel ID="WordLiteral11" runat="Server" TextWordNo="20020" /></h4>
							<div class="AddMessage">
                                <div class="memoTextBoxInner">
                                    <div id="memoTextBoxInner2">
                                        <asp:TextBox ID="todayMemoTextBox" ReadOnly="false" MaxLength="256" runat="server" TextMode="MultiLine" TabIndex="4001" />
                                        <asp:HiddenField ID="todayMemoTextBoxBefore" runat="server" />
                                    </div>
                                </div>
                            </div>
						</div>
					</div>
                    <%'当日メモ End %>

                    <%'過去メモ Start %>
					<div class="scNsc50MemoBottom">
						<div class="scNsc50MemoInBox">
							<ul class="DottedBoder">
                                <asp:Repeater runat="server" id="MemoRepeater" ClientIDMode="Predictable">
                                    <ItemTemplate>
                                        <li>
                                            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                            <!-- ' 2012/03/14 TCS 寺本 【SALES_2】 START -->
                                            <asp:label ID="Label3" class="MemoHisText" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("MEMO")).Replace(vbCrLf, "<br/>").Replace(vbLf, "<br/>") %>'/>
                                            <!-- ' 2012/03/14 TCS 寺本 【SALES_2】 END -->
                                            <span class="MemoHisStaffIcon"></span>
                                            <icrop:CustomLabel ID="memoHisStaffName" runat="server" class="MemoHisStaff ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("USERNAME")) %>' />
                                            <asp:label class="MemoHisDate ellipsis" ID="Label1" runat="server" Text='<%# Eval("INPUTDATE") %>' />
                                            <asp:HiddenField runat="server" ID="MemoHisStaffIconFileName" Value='<%# Eval("ICON_IMGFILE") %>'/>
                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                            <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div style="height:16px;"></div>
						</div>
					</div>
                    <%'過去メモ End %>
				</div>
                <%'メモエリア End %>

			</div>
					
		<p class="clearboth"></p>
	</div>
                

<p class="clearboth"></p>  
<%'-----------------------------------------%>
<%'最新活動ポップアップ ここから              %>
<%'-----------------------------------------%>
<div id="activityPop_content">
    <div class="popWind">
        <div class="PopUpBtn02">
            <ul>
                <li class="title clip"><icrop:CustomLabel ID="PopupActivityListTitleLabel" runat="Server" TextWordNo="20021" /></li>
            </ul>
        </div>
        <div class="dataWind1">
 
        
 <%'2012/03/27 TCS 松野 【SALES_2】 START%>
        <asp:UpdatePanel ID="activityPopUpPanel" runat="server">
        <ContentTemplate>
	        <asp:Button ID="activityPopUpdateDummyButton" runat="server" style="display:none" />
	        <asp:Panel ID="activityPopPanel" runat="server" Visible="false">
 <%'2012/03/27 TCS 松野 【SALES_2】 END%>
        
            <div class="activityPopScrollWrap">
                    
                <%'一覧リピート %>
                <ul>
                    <asp:Repeater runat="server" ID="ActivityRepeater">
                        <ItemTemplate>
                            <li class="activityListItem">
                                <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                <%'タップエリア %>
                                <a href="javascript:void(0)">
                                    <span class="crActName ellipsis"><asp:Literal ID="Literal1" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("CRACTNAME")) %>'/></span>
                                    <span class="crActStatus ellipsis"><asp:Literal ID="Literal2" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("CRACTSTATUS")) %>'/></span>
                                    <span class="crActDate ellipsis"><asp:Literal ID="Literal3" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("CRACTDATESTRING")) %>'/></span>
                                    <div class="clearboth"></div>
                                </a>
                                <%'隠し項目%>
                                <asp:hiddenfield runat="server" id="EnableFlgHidden" value='<%# HttpUtility.HtmlEncode(Eval("ENABLEFLG")) %>' />
                                <asp:hiddenfield runat="server" id="FllwupboxSeqnoHidden" value='<%# HttpUtility.HtmlEncode(Eval("FLLWUPBOX_SEQNO")) %>' />
                                <asp:hiddenfield runat="server" id="FllwupboxStrCdHidden" value='<%# HttpUtility.HtmlEncode(Eval("STRCD")) %>' />
                                <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div style="height:14px;"></div>
                    
            </div>
            
<%'2012/03/27 TCS 松野 【SALES_2】 START%>
            </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>
<%'2012/03/27 TCS 松野 【SALES_2】 END%>
            
        </div>

        <div class="baseWind1">
            <div class="boxBoder">
                <div class="fukiBoder">
                    <div class="fuki">&nbsp;</div></div><div class="box">&nbsp; </div></div></div></div></div><%'最新活動ポップアップ ここまで %><%'-----------------------------------------%><%'その他条件入力ポップアップ ここから        %><%'-----------------------------------------%><asp:Panel ID="OtherConditionInputPopup" CssClass="scNsc51PopUpModelSelect" runat="server">
    <div class="scNsc51PopUpModelSelectArrowOther"></div>
    <div class="scNsc51PopUpModelSelectWindownBox">
        <%'ポップアップヘッダー %>
        <div class="scNsc51OtherPopUpSelectHeader">
            <%'タイトル %>
			<h3 class="clip">
                <icrop:CustomLabel ID="CustomLabel6" runat="server" TextWordNo="20015" />
            </h3>
            <%'キャンセルボタン %>
			<a href="javascript:void(0)" class="scNscOtherPopUpCancelButton clip">
                <icrop:CustomLabel ID="CustomLabel12" TabIndex="1" runat="server" TextWordNo="20013" />
            </a>
            <%'完了ボタン %>
            <a href="javascript:void(0)" class="scNscOtherPopUpCompleteButton clip">
                <icrop:CustomLabel ID="CustomLabel15" runat="server" TextWordNo="20014" />
            </a>
        </div>
        <div class="scNsc51PopUpModelSelectListArea ellipsis">
            <div id="ScNsc51OtherConditionInputTextWrap">
                <icrop:CustomTextBox ID="ScNsc51OtherConditionInputText" runat="server" MaxLength="30" Width="270px" />
            </div>
        </div>
    </div>
</asp:Panel>        
<%'その他条件入力ポップアップ ここまで%>

<%'-----------------------------------------%>
<%'希望車種選択ポップアップ ここから          %>
<%'-----------------------------------------%>
<asp:Panel ID="SeriesSelectPopup" CssClass="scNsc51PopUpModelSelect" runat="server">
    <div class="scNsc51PopUpModelSelectArrow"></div>
    <div class="scNsc51PopUpModelSelectWindownBox">
        <%'ポップアップヘッダー %>
        <div class="scNsc51PopUpModelSelectHeader clip">
            <%' 2017/11/20 TCS 河原 TKM独自機能開発 START %>
            <%'タイトル %>
			<h3>
                <icrop:CustomLabel ID="SeriesSelectPage1Title" runat="server" TextWordNo="20030" />
                <icrop:CustomLabel ID="SeriesSelectPage2Title" runat="server" TextWordNo="20031" />
                <icrop:CustomLabel ID="SeriesSelectPage3Title" runat="server" TextWordNo="20082" />
                <icrop:CustomLabel ID="SeriesSelectPage4Title" runat="server" TextWordNo="20033" />
                <icrop:CustomLabel ID="SeriesSelectPage5Title" runat="server" TextWordNo="20083" />
            </h3>
            <%'前に戻るボタン %>
			<a href="javascript:void(0)" class="scNscPopUpCancelButton">
                <icrop:CustomLabel ID="SeriesSelectCancelLabel" TabIndex="1" runat="server" TextWordNo="20013" style="display:none" />
                <icrop:CustomLabel ID="SeriesSelectBackModelLabel" TabIndex="1" runat="server" TextWordNo="20030" style="display:none" />
                <icrop:CustomLabel ID="SeriesSelectBackGradeLabel" TabIndex="1" runat="server" TextWordNo="20031" style="display:none" />
                <icrop:CustomLabel ID="SeriesSelectBackSuffixLabel" TabIndex="1" runat="server" TextWordNo="20082" style="display:none" />
                <icrop:CustomLabel ID="SeriesSelectBackExteriorColorLabel" TabIndex="1" runat="server" TextWordNo="20033" style="display:none" />
            </a>
            <%'完了ボタン %>
            <a href="javascript:void(0)" class="scNscPopUpCompleteButton">
                <icrop:CustomLabel ID="SeriesSelectCompLabel" runat="server" TextWordNo="20014" />
            </a>
            <%' 2017/11/20 TCS 河原 TKM独自機能開発 END %>
        </div>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
        <asp:UpdatePanel ID="SeriesSelectPopupPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="SeriesSelectPopupButtonDummy" runat="server" style="display:none" />
                <asp:Button ID="SeriesSelectPopupUpdateButtonDummy" runat="server" style="display:none" />
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

                <div class="scNsc51PopUpModelSelectListArea2">
                    <%' 2017/11/20 TCS 河原 TKM独自機能開発 START %>
                    <%'各リスト選択を囲う枠 ここから %>
                    <div id="scNsc51PopUpListWrap" class="page1 ellipsis">

                        <%'選択項目を格納する隠しフィールド %>
                        <div id="scNsc51PopUpHiddenWrap" style="display:none">
                            <asp:hiddenfield runat="server" id="SelectModelcdHidden" value="" />
                            <asp:hiddenfield runat="server" id="SelectGradecdHidden" value="" />
                            <asp:hiddenfield runat="server" id="SelectSuffixcdHidden" value="" />
                            <asp:hiddenfield runat="server" id="SelectExteriorColorcdHidden" value="" />
                            <asp:hiddenfield runat="server" id="SelectInteriorColorcdHidden" value="" />
                            <asp:hiddenfield runat="server" id="SelectSeriesEidtMode" value="" />
                            <asp:hiddenfield runat="server" id="SelectSeriesDelMode" value="" />
                            <asp:HiddenField runat="server" ID="SelectSeqnoHidden" Value="" />
                            <%'2013/06/30 TCS 黄 2013/10対応版　既存流用 START%>
                            <asp:HiddenField runat="server" ID="SelectLockvrHidden" />
                            <%'2013/06/30 TCS 黄 2013/10対応版　既存流用 END%>
                            <%'2013/12/12 TCS 市川 Aカード情報相互連携開発 START%>
                            <asp:HiddenField runat="server" ID="SelectMostPreferredHidden" />
                            <%'2013/12/12 TCS 市川 Aカード情報相互連携開発 END%>
                        </div>

                        <%'モデル選択 %>
                        <div class="scNsc51PopUpList01">
                            <div class="scNsc51PopUpScrollWrap">
                                <asp:Panel ID="PopUpList01PanelArea" runat="server" Visible="false">
                                <div style="height:6px;"></div>
                                <ul>
                                    <asp:Repeater runat="server" ID="SeriesMasterRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server" class="scNsc51ListLi1" id="scNsc51ListLi1" 
                                                itemid='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>' ClientIDMode="Predictable">
                                                <icrop:CustomLabel ID="scNsc51ListLi1Label" CssClass="ellipsis" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("SERIESNM")) %>' Width="210px" UseEllipsis="false"/>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div style="height:8px;"></div>
                                </asp:Panel>
                            </div>
                        </div>

                        <%'グレード選択 %>
                        <div class="scNsc51PopUpList02">
                            <div class="scNsc51PopUpScrollWrap">
                                <asp:Panel ID="PopUpList02PanelArea" runat="server" Visible="false">
                                <div style="height:6px;"></div>
                                <ul>
                                    <asp:Repeater runat="server" ID="ModelMasterRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server" class="scNsc51ListLi2" id="scNsc51ListLi2" 
                                                itemid='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>' 
                                                itemid2='<%# HttpUtility.HtmlEncode(Eval("VCLMODEL_CODE")) %>' 
                                                ClientIDMode="Predictable" style="display:none">
                                                <icrop:CustomLabel ID="scNsc51ListLi2Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("VCLMODEL_NAME")) %>' Width="210px" UseEllipsis="false"/>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div style="height:8px;"></div>
                                </asp:Panel>
                            </div>
                            <%'希望車種削除ボタン %>
                            <div class="scNsc51PopUpListDeleteButton clip">
                                <icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="20032" />
                            </div>
                        </div>


                        <%'サフィックス選択 %>
                        <div class="scNsc51PopUpList03">
                            <div class="scNsc51PopUpScrollWrap">
                                <asp:Panel ID="PopUpList03PanelArea" runat="server" Visible="false">
                                <div style="height:6px;"></div>
                                <ul>
                                    <asp:Repeater runat="server" ID="SuffixMasterRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server" class="scNsc51ListLi3" id="scNsc51ListLi3" 
                                                itemid='<%# HttpUtility.HtmlEncode(Eval("MODEL_CD")) %>' 
                                                itemid2='<%# HttpUtility.HtmlEncode(Eval("GRADE_CD")) %>' 
                                                itemid3='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_CD")) %>' 
                                                ClientIDMode="Predictable" style="display:none">
                                                <icrop:CustomLabel ID="scNsc51ListLi3Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_NAME")) %>' Width="210px" UseEllipsis="false"/>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div style="height:8px;"></div>
                                </asp:Panel>
                            </div>
                        </div>


                        <%'外装色選択 %>
                        <div class="scNsc51PopUpList04">
                            <div class="scNsc51PopUpScrollWrap">
                                <asp:Panel ID="PopUpList04PanelArea" runat="server" Visible="false">
                                <div style="height:6px;"></div>
                                <ul>
                                    <asp:Repeater runat="server" ID="ExteriorColorMasterRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server" class="scNsc51ListLi4 NoArrow" id="scNsc51ListLi4" 
                                                itemid='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>' 
                                                itemid2='<%# HttpUtility.HtmlEncode(Eval("VCLMODEL_CODE")) %>' 
                                                itemid3='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_CD")) %>' 
                                                itemid4='<%# HttpUtility.HtmlEncode(Eval("BODYCLR_CD")) %>' 
                                                ClientIDMode="Predictable" style="display:none">
                                                <icrop:CustomLabel ID="scNsc51ListLi4Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("DISP_BDY_COLOR")) %>' Width="210px" UseEllipsis="false"/>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div style="height:8px;"></div>
                                </asp:Panel>
                            </div>
                        </div>


                        <%'内装色選択 %>
                        <div class="scNsc51PopUpList05">
                            <div class="scNsc51PopUpScrollWrap">
                                <asp:Panel ID="PopUpList05PanelArea" runat="server" Visible="false">
                                <div style="height:6px;"></div>
                                <ul>
                                    <asp:Repeater runat="server" ID="InteriorColorMasterRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server" class="scNsc51ListLi5 NoArrow" id="scNsc51ListLi5" 
                                                itemid='<%# HttpUtility.HtmlEncode(Eval("MODEL_CD")) %>' 
                                                itemid2='<%# HttpUtility.HtmlEncode(Eval("GRADE_CD")) %>' 
                                                itemid3='<%# HttpUtility.HtmlEncode(Eval("SUFFIX_CD")) %>' 
                                                itemid4='<%# HttpUtility.HtmlEncode(Eval("BODYCLR_CD")) %>' 
                                                itemid5='<%# HttpUtility.HtmlEncode(Eval("INTERIORCLR_CD")) %>' 
                                                ClientIDMode="Predictable"  style="display:none">
                                                <icrop:CustomLabel ID="scNsc51ListLi5Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("INTERIORCLR_NAME")) %>' Width="210px" UseEllipsis="false"/>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div style="height:8px;"></div>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="clearboth"></div>
                    </div>
                    <%--各リスト選択を囲う枠 ここまで --%>
                    <%' 2017/11/20 TCS 河原 TKM独自機能開発 END %>
                </div>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
             </ContentTemplate>
        </asp:UpdatePanel>        
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>
    </div>
</asp:Panel>
<%'希望車種選択ポップアップ ここまで %>



<%'-----------------------------------------%>
<%'競合車種選択ポップアップ ここから          %>
<%'-----------------------------------------%>
<asp:Panel ID="CompCarSelectPopup" CssClass="scNsc51PopUpModelSelect" runat="server" style="position:absolute;">
	<div class="scNsc51PopUpModelSelectArrow"></div>
	<div class="scNsc51PopUpModelSelectWindownBox">

        <%'ヘッダーエリア %>
		<div class="scNsc51CompPopUpModelSelectHeader clip">
            <%'タイトル %>
			<h3>
                <icrop:CustomLabel ID="CompCarPopupMakerTitle" runat="server" TextWordNo="20035" style="display:none;" />
                <icrop:CustomLabel ID="CompCarPopupModelTitle" runat="server" TextWordNo="20036" style="display:none;" />
            </h3>
            <%'戻る・キャンセルボタン %>
			<a href="javascript:void(0)" class="scNscCompPopUpCancelButton">
                <icrop:CustomLabel ID="CompCarPopupCancelLabel" runat="server" TextWordNo="20013" style="display:none;" />
                <icrop:CustomLabel ID="CompCarPopupMakerBkLabel" runat="server" TextWordNo="20035" style="display:none;" />
            </a>
            <%'完了ボタン %>
            <a href="javascript:void(0)" class="scNscCompPopUpCompleteButton">
                <icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="20014" />
            </a>
		</div>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
   		<asp:UpdatePanel ID="CompCarSelectPopupPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="CompCarSelectPopupButtonDummy" runat="server" style="display:none"/>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>
			
		        <div class="scNsc51PopUpModelSelectListArea2 ellipsis">
					
                    <%'各リスト選択を囲う枠 ここから %>
                    <div id="CompCarSelectPopupListWrap" class="page1">
                
                        <%'メーカー選択 Start %>
				        <div class="scNsc51CompPopUpList01">
                            <div class="scNsc51PopUpScrollWrapComp">

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
                                <asp:Panel ID="CompPopUpList01PanelArea" runat="server" Visible="false">
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>
                                            
					                <div style="height:6px;"></div>
                                    <ul>
                                        <asp:Repeater runat="server" ID="CompCarMakerMasterRepeater" ClientIDMode="Predictable">
                                            <ItemTemplate>
					                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
					                            <li makercd='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKERNO")) %>' class="scNsc51CompListLi1">
                                                    <icrop:CustomLabel ID="scNsc51CompListLi1Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKER")) %>' Width="210px" UseEllipsis="false"/>
                                                </li>
                                                <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
					                        </ItemTemplate>
                                        </asp:Repeater>
					                </ul>
                                    <div style="height:8px;"></div>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
                                </asp:Panel>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

                            </div>
				        </div>
                        <%'メーカー選択 End %>
                            
                        <%'モデル選択 Start %>
				        <div class="scNsc51CompPopUpList02">
                            <div class="scNsc51PopUpScrollWrapComp">

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
                                <asp:Panel ID="CompPopUpList02PanelArea" runat="server" Visible="false">
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

                                    <div style="height:6px;"></div>
					                <ul>
                                        <asp:Repeater runat="server" ID="CompCarModelMasterRepeater">
                                            <ItemTemplate>
                                                <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                <li makercd='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKERNO")) %>' 
                                                    compcd='<%# HttpUtility.HtmlEncode(Eval("COMPETITORCD")) %>' 
                                                    class="scNsc51CompListLi2 NoArrow">
                                                    <icrop:CustomLabel ID="scNsc51CompListLi2Label" runat="server" CssClass="ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("COMPETITORNM")) %>' Width="210px" UseEllipsis="false"/>
                                                    <div class="scNsc51CompPopUpList02Hidden" style="display:none">
                                                        <asp:HiddenField id="CompMakerCd"  runat="server" ClientIDMode="Predictable" Value='<%# HttpUtility.HtmlEncode(Eval("COMPETITIONMAKERNO")) %>' />
                                                        <asp:HiddenField id="CompModelCd"  runat="server" ClientIDMode="Predictable" Value='<%# HttpUtility.HtmlEncode(Eval("COMPETITORCD")) %>' />
                                                        <asp:HiddenField id="CompCheckState"  runat="server" ClientIDMode="Predictable" Value="False" />
                                                    </div>
                                                <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
				                    </ul>
                                    <div style="height:8px;"></div>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
                                </asp:Panel>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

                            </div>
				        </div>
                        <%'モデル選択 End %>
                        <div class="clearboth"></div>

                    </div>
                    <%'各リスト選択を囲う枠 ここまで %>

		        </div>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
            </ContentTemplate>
        </asp:UpdatePanel>	
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

	</div>
</asp:Panel>
<%'競合車種選択ポップアップ ここまで %>

<%--2013/12/09 TCS 市川 Aカード情報相互連携開発 START--%>
<%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除  %>

<%
    '-----------------------------------------
    '   用件ソース1st選択ポップアップ
    '   (※基盤PopOverForm利用)
    '-----------------------------------------
%>
<%--ヘッダー部--%>
<div id="popOverSource1ListHeader" class="popOverHeader" style="display:none;width:310px;" >
    <icrop:CustomLabel runat="server" ID="CustomLabel10" TextWordNo="10125" CssClass="cancelButton" />
    <icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="20051" UseEllipsis="true" CssClass="titleLabel ellipsis" style="width:135px;" />
    <icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="20014" CssClass="commitButton" />
</div>
<%--選択エリア--%>
<div id="popOverSource1ListBody" class="popOverBody" style="height:200px;overflow:hidden;display:none;">
    <ul class="itemBox">
        <asp:Repeater runat="server" ID="Source1ListRepeater">
            <HeaderTemplate>
                <li class="itemRow ellipsis" style="border-top:none;" 
            </HeaderTemplate>
            <ItemTemplate>
                value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SOURCE_1_CD"))%>" > <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SOURCE_1_NAME"))%></ItemTemplate><SeparatorTemplate ></li><li class="itemRow ellipsis"</SeparatorTemplate>
            <FooterTemplate ></li></FooterTemplate>
        </asp:Repeater>
    </ul>
</div>
<%--2013/12/09 TCS 市川 Aカード情報相互連携開発 END--%>
<%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start %>
<%
    '-----------------------------------------
    '   用件ソース2st選択ポップアップ
    '   (※基盤PopOverForm利用)
    '-----------------------------------------
%>
<%--ヘッダー部--%>
<div id="popOverSource2ListHeader" class="popOverHeader" style="display:none;width:310px;" >
    <icrop:CustomLabel runat="server" ID="CustomLabel21" TextWordNo="10125" CssClass="cancelButton" />
    <icrop:CustomLabel ID="CustomLabel22" runat="server" TextWordNo="2020011" UseEllipsis="true" CssClass="titleLabel ellipsis" style="width:135px;" />
    <icrop:CustomLabel ID="CustomLabel23" runat="server" TextWordNo="20014" CssClass="commitButton" />
</div>
<%--選択エリア--%>
<div id="popOverSource2ListBody" runat="server" class="popOverBody" style="height:200px;overflow:hidden;display:none;">
    <ul class="itemBox">
<asp:UpdatePanel runat="server" ID="UpdSource2Selector" UpdateMode="Conditional">
    <ContentTemplate>
               <asp:Repeater runat="server" ID="Source2ListRepeater" ClientIDMode="Predictable">
            <HeaderTemplate>
                <li class="itemRow ellipsis" style="border-top:none;" 
            </HeaderTemplate>
            <ItemTemplate>
                value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SOURCE_2_CD"))%>" > <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "REQ_SECOND_CAT_NAME"))%></ItemTemplate><SeparatorTemplate ></li><li class="itemRow ellipsis"</SeparatorTemplate>
            <FooterTemplate ></li></FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
    </ul>
</div>
<%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end %>

<%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
<%'-----------------------------------------%>
<%  '項目名変更ポップアップ ここから        %>
<%'-----------------------------------------%>
<asp:Panel ID="CondItemLabelInputPopup" CssClass="scNsc51PopUpModelSelect" runat="server">
    <div class="scNsc51PopUpModelSelectArrowItmLbl"></div>
    <div class="scNsc51PopUpModelSelectWindownBox">
        <%'ポップアップヘッダー %>
        <div class="scNsc51CondItemLabelPopUpSelectHeader">
            <%'タイトル %>
			<h3 class="clip">
                <icrop:CustomLabel ID="CondItemLabelInputPopupTitle" runat="server"/>
            </h3>
            <%'キャンセルボタン %>
			<a href="javascript:void(0)" class="scNscCondItemLabelPopUpCancelButton clip">
                <icrop:CustomLabel ID="CustomLabel47" TabIndex="1" runat="server" TextWordNo="2020002" />
            </a>
            <%'完了ボタン %>
            <a href="javascript:void(0)" class="scNscCondItemLabelPopUpCompleteButton clip">
                <icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="2020003" />
            </a>
        </div>
        <div class="scNsc51PopUpModelSelectListArea ellipsis">
            <div id="ScNsc51CondItemLabelInputTextWrap">
                <icrop:CustomTextBox ID="ScNsc51CondItemLabelInputText" runat="server" MaxLength="30" Width="270px" />
            </div>
        </div>
    </div>
</asp:Panel>        
<%  '項目名変更ポップアップ ここまで%>
<%'2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END%>

<%  '下取車両メーカーポップアップ START%>
<asp:ObjectDataSource id="Trade_in_MakerDataSource" runat="server"  SelectMethod="GetTradeincarMaker" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic" />
<icrop:PopOver ID="Trade_in_MakerPopOver" runat="server" TriggerClientID="Trade_in_MakerTrigger" Width="200px" Height="200px">
<div id="Trade_in_MakerWindown">
    <div id="Trade_in_MakerWindownBox">
        <div class="Trade_in_MakerHadder">
            <h3><icrop:CustomLabel ID="CustomLabel52" runat="server" TextWordNo="2020008" Text="分類" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
        </div>
        <div class="Trade_in_MakerListArea">
            <div class="Trade_in_MakerListBox">
                <div class="Trade_in_MakerListItemBox">
                    <div class="Trade_in_MakerListItem5">
                        <asp:UpdatePanel runat="server" ID="Trade_in_MakerUpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Trade_in_MakerPanel" runat="server" Visible="false">
                                    <ul class="nscListBoxSetIn">
                                        <asp:Repeater ID="Trade_in_MakerRepeater" runat="server" DataSourceID ="Trade_in_MakerDataSource" ClientIDMode="Predictable">
                                            <ItemTemplate>
                                                <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MAKER_NAME")) %>" id="<%# DataBinder.Eval(Container.DataItem, "MAKER_CD")%>" class="Trade_in_Makerlist ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "MAKER_NAME")%>">
                                                    <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MAKER_NAME")) %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</icrop:PopOver>
<%  '下取車両メーカーポップアップ END%>

<%  '下取車両モデルポップアップ START%>
<asp:ObjectDataSource id="Trade_in_ModelDataSource" runat="server"  SelectMethod="GetTradeincarModel" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Trade_in_MakerValue" Name="tradeincar_maker_cd" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>

<icrop:PopOver ID="Trade_in_ModelPopOver" runat="server" TriggerClientID="Trade_in_ModelTrigger" Width="200px" Height="200px">
<div id="Trade_in_ModelWindown">
    <div id="Trade_in_ModelWindownBox">
        <div class="Trade_in_ModelHadder">
            <h3><icrop:CustomLabel ID="CustomLabel53" runat="server" TextWordNo="2020009" Text="分類" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
        </div>
        <div class="Trade_in_ModelListArea">
            <div class="Trade_in_ModelListBox">
                <div class="Trade_in_ModelListItemBox">
                    <div class="Trade_in_ModelListItem5">
                        <asp:UpdatePanel runat="server" ID="Trade_in_ModelUpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Trade_in_ModelPanel" runat="server" Visible="false">
                                    <ul class="nscListBoxSetIn">
                                        <asp:Repeater ID="Trade_in_ModelRepeater" runat="server" DataSourceID ="Trade_in_ModelDataSource" ClientIDMode="Predictable">
                                            <ItemTemplate>
                                                <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MODEL_NAME")) %>" id="<%# DataBinder.Eval(Container.DataItem, "MODEL_CD")%>" class="Trade_in_Modellist ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "MODEL_NAME")%>">
                                                    <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MODEL_NAME")) %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</icrop:PopOver>
<%  '下取車両モデルポップアップ END%>

<%  '下取車両走行距離ポップアップ START%>
<asp:Panel ID="Trade_in_MileageInputPopup" CssClass="scNsc51PopUpModelSelect" runat="server">
    <div class="scNsc51PopUpModelSelectArrowOther"></div>
    <div class="scNsc51PopUpModelSelectWindownBox">
        <%'ポップアップヘッダー %>
        <div class="scNsc51OtherPopUpSelectHeader">
            <%'タイトル %>
            <h3 class="clip">
                <icrop:CustomLabel ID="Trade_in_MileageTitleLabel" runat="server" TextWordNo="2020010" />
            </h3>
            <%'キャンセルボタン %>
			<a href="javascript:void(0)" class="Trade_in_MileageCancelButton clip">
                <icrop:CustomLabel ID="Trade_in_MileageCancelButtonLabel" TabIndex="1" runat="server" TextWordNo="20013" />
            </a>
            <%'完了ボタン %>
            <a href="javascript:void(0)" class="Trade_in_MileageCompleteButton clip">
                <icrop:CustomLabel ID="Trade_in_MileageCompleteButtonLabel" runat="server" TextWordNo="20014" />
            </a>
        </div>
        <div class="scNsc51PopUpModelSelectListArea ellipsis">
            <div id="ScNsc51OtherConditionInputTextWrap">
                <icrop:CustomTextBox ID="Trade_in_MileageInputText" runat="server" MaxLength="30" Width="270px" />
            </div>
        </div>
    </div>
</asp:Panel>
<%  '下取車両走行距離ポップアップ END%>

<%  '下取車両年式ポップアップ START%>
<asp:ObjectDataSource id="Trade_in_ModelYearDataSource" runat="server"  SelectMethod="GetTradeincarModelYear" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic" />

<icrop:PopOver ID="Trade_in_ModelYearPopOver" runat="server" TriggerClientID="Trade_in_ModelYearTrigger" Width="200px" Height="200px">
<div id="Trade_in_ModelYearWindown">
    <div id="Trade_in_ModelYearWindownBox">
        <div class="Trade_in_ModelYearHadder">
            <h3><icrop:CustomLabel ID="CustomLabel54" runat="server" TextWordNo="2020004" Text="年式" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
        </div>
        <div class="Trade_in_ModelYearListArea">
            <div class="Trade_in_ModelYearListBox">
                <div class="Trade_in_ModelYearListItemBox">
                    <div class="Trade_in_ModelYearListItem5">
                        <asp:UpdatePanel runat="server" ID="Trade_in_ModelYearUpdatePanel" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Trade_in_ModelYearPanel" runat="server" Visible="false">
                                    <ul class="nscListBoxSetIn">
                                        <asp:Repeater ID="Trade_in_ModelYearRepeater" runat="server" DataSourceID ="Trade_in_ModelYearDataSource" ClientIDMode="Predictable">
                                            <ItemTemplate>
                                                <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MODEL_YEAR")) %>" id="<%# DataBinder.Eval(Container.DataItem, "MODEL_YEAR")%>" class="Trade_in_ModelYearlist ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "MODEL_YEAR")%>">
                                                    <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "MODEL_YEAR"))%>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</icrop:PopOver>
<%  '下取車両年式ポップアップ END%>
<%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END --%>

<!-- ポップアップ end -->


<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
<asp:UpdatePanel ID="SC3080202HiddenUpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

    <asp:HiddenField ID="PageEnabledFlgHidden" runat="server" />
    
<%'2012/03/27 TCS 松野 【SALES_2】 START%>
    <asp:HiddenField ID="PageActivityPopEnabledFlgHidden" runat="server" />
    <asp:HiddenField ID="NewActivityFlgHidden" runat="server" />
<%'2012/03/27 TCS 松野 【SALES_2】 END%>

    <%--2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START--%>
    <asp:HiddenField ID="PageMoveFlgHidden" runat="server" />
    <asp:HiddenField ID="PageMoveErrorMessage" runat="server" />
    <%--2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END--%>
    
    <asp:HiddenField ID="CleansingErrorMessage" runat="server" />

    <%'2014/02/12 TCS 山口 受注後フォロー機能開発 START%>
    <asp:HiddenField runat="server" id="selFllwupboxSeqnoHidden" />
    <asp:HiddenField runat="server" ID="selFllwupboxDlrcdHidden" />
    <asp:HiddenField runat="server" ID="selFllwupboxStrcdHidden" />
    <%'2014/02/12 TCS 山口 受注後フォロー機能開発 END%>

    <%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 start %>
    <asp:HiddenField ID="hdnLastSource1" runat="server" />
    <asp:HiddenField ID="hdnLastSource2" runat="server" />
    <asp:HiddenField ID="hdnSource1PossibleFlg" runat="server" />
    <asp:HiddenField ID="hdnSource2PossibleFlg" runat="server" />
    <asp:HiddenField ID="hdnLTRowLockVersion" runat="server" />
    <asp:HiddenField ID="hdnGetTableNO" runat="server" />
    <%'2020/01/22 TS  重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 end  %>

<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start---%>
    </ContentTemplate>
</asp:UpdatePanel>
<%'---2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End---%>

    <asp:HiddenField ID="QuantityErrorMessageReqiored" runat="server" />
    <asp:HiddenField ID="QuantityErrorMessageNumric" runat="server" />
    <asp:HiddenField ID="OtherConditionErrorMessage" runat="server" />
    <asp:HiddenField ID="DestructionMessage" runat="server" />
    <asp:HiddenField ID="CancelNumericMessage" runat="server" />
    <asp:HiddenField ID="CompletionNumericMessage" runat="server" />

    <!-- ' 2012/02/29 TCS 小野 【SALES_2】 START -->
    <asp:HiddenField ID="selFllwupboxSalesBkgno" runat="server" />
    <asp:HiddenField ID="selFllwupboxSalesAfterFlg" runat="server" />
    <asp:HiddenField runat="server" id="ProcessCatalogHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessTestdriveHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessEvaluationHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessQuotationHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessSuccessHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessAllocationHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessPaymentHiddenDefalutName" />
    <asp:HiddenField runat="server" id="ProcessDeliveryHiddenDefalutName" />
    <asp:HiddenField runat="server" ID="MemoOnlyFlgHidden" />
    <!-- ' 2012/02/29 TCS 小野 【SALES_2】 END -->

    <%-- 2013/03/06 TCS 河原 GL0874 START --%>
    <asp:HiddenField ID="SC3080202ContractCancelFlg" runat="server" Value="0" />
    <%-- 2013/03/06 TCS 河原 GL0874 END --%>

    <%-- 2015/12/08 TCS 中村 ADD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START --%>
    <asp:HiddenField ID="UseAfterOdrProcFlgHidden" runat="server" />
    <%-- 2015/12/08 TCS 中村 ADD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END --%>

    <%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START --%>
    <asp:HiddenField ID="CondItemLabelErrorMessage" runat="server" />

    <asp:UpdatePanel runat="server" ID="DemandStructureUpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="Trade_in_MakerButton" style="display:none" />
            <asp:Button runat="server" ID="Trade_in_ModelButton" style="display:none" />
            <asp:Button runat="server" ID="Trade_in_MileageButton" style="display:none" />
            <asp:Button runat="server" ID="Trade_in_ModelYearButton" style="display:none" />
            <asp:HiddenField runat="server" ID="DemandStructureCd" />
            <asp:HiddenField runat="server" ID="TradeinEnabledFlg" />
            <asp:HiddenField runat="server" ID="Trade_in_MakerName" />
            <asp:HiddenField runat="server" ID="Trade_in_MakerValue" />
            <asp:HiddenField runat="server" ID="Trade_in_ModelName" />
            <asp:HiddenField runat="server" ID="Trade_in_ModelValue" />
            <asp:HiddenField runat="server" ID="Trade_in_MileageValue" />
            <asp:HiddenField runat="server" ID="Trade_in_ModelYearValue" />
            <asp:HiddenField runat="server" ID="SalesLocalLockvr" />
            <asp:HiddenField runat="server" ID="BeforeDemandStructureCd" />
            <asp:HiddenField runat="server" ID="BeforeTradeinEnabledFlg" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_MakerName" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_MakerValue" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_ModelName" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_ModelValue" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_MileageValue" />
            <asp:HiddenField runat="server" ID="BeforeTrade_in_ModelYearValue" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <icrop:CustomLabel ID="msg2020913" runat="Server" TextWordNo="2020913" Width="170px" style="display:none;" />
    <asp:HiddenField ID="ReplaceTxtItemTitle" runat="server" />
    <%-- '2018/07/01 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END --%>

</div>
