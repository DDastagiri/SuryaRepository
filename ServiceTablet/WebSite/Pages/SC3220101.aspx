<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3220101.aspx.vb" Inherits="Pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/SC3220101/SC3220101.css?20180622000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3220101/SC3220101.PullDownRefresh.css?20121113000000" type="text/css" media="screen,print" />
    <%-- 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START--%>
    <link rel="Stylesheet" href="../Styles/SC3220101/footer.css?20130901000000" type="text/css" media="screen,print" />
    <%-- 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END--%>
    <script type="text/javascript" src="../Scripts/SC3220101/SC3220101.js?20180622000000"></script>
    <script type="text/javascript" src="../Scripts/SC3220101/SC3220101.popoverEx.js?20121114000000"></script>
    <script type="text/javascript" src="../Scripts/SC3220101/SC3220101.MainMenuFingerscroll.js?20121113000000"></script>
    <script type="text/javascript" src="../Scripts/SC3220101/SC3220101.PullDownRefresh.js?20121114000000"></script>
    <script type="text/javascript" src="../Scripts/SC3220101/SC3220101.CustomLabelEx.js?20121113000000"></script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

    <asp:HiddenField ID="HiddenChipData" runat="server" />
    <%-- 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START--%>
    <asp:HiddenField ID="HiddenIconWord" runat="server" />
    <%-- 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END--%>

    <div id="mainblockWrap" class="mainblockWrap">
    <div id="mainblockContent">
    <div class="mainblockContentArea">
    <div class="mainblockContentAreaWrap">

    <table width="68px" border="0" cellspacing="2" cellpadding="0" id="ssvm01VisitTable">
        <tr>
            <td>
                <div id="visitHeaderSet" class="BaseDiv selWidth4 headerSet01">
                    <div class="InnerText6">
                        <icrop:CustomLabel ID="FixInnerText6Label" runat="server" CssClass="Ellipsis AreaName" Width="45" TextWordNo="38"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText6CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="15"></icrop:CustomLabel>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="scrollDiv2">
                    <div class="BaseDiv DatasSet"></div>
                </div>
            </td>
        </tr>
    </table>
    <table width="80%" border="0" cellspacing="2" cellpadding="0" id="ssvm01MaineTable">
        <tr>
            <td>
                <div id="headerSet01" class="BaseDiv selWidth1 headerSet01">
                    <div class="tableTitle1">
                        <icrop:CustomLabel ID="CustomLabel1" runat="server" CssClass="Ellipsis" Width="118" UseEllipsis="true" TextWordNo="2"></icrop:CustomLabel>
                    </div>		
                </div>
            </td>
            <td>
                <div class="BaseDiv selWidth2 headerSet02">
                    <div class="InnerText1">
                        <icrop:CustomLabel ID="FixInnerText1Label" runat="server" CssClass="Ellipsis AreaName" Width="130" TextWordNo="3"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText1CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="25"></icrop:CustomLabel>
                    </div>
                    <div class="InnerText2">
                        <icrop:CustomLabel ID="FixInnerText2Label" runat="server" CssClass="Ellipsis AreaName" Width="130" TextWordNo="4"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText2CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="25"></icrop:CustomLabel>
                    </div>
                    <div class="InnerText3">
                        <icrop:CustomLabel ID="FixInnerText3Label" runat="server" CssClass="Ellipsis AreaName" Width="100" TextWordNo="5"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText3CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="25"></icrop:CustomLabel>
                    </div>
                    <div class="InnerText4">
                        <icrop:CustomLabel ID="FixInnerText4Label" runat="server" CssClass="Ellipsis AreaName" Width="130" TextWordNo="6"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText4CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="25"></icrop:CustomLabel>
                    </div>
                    <div class="InnerText5">
                        <icrop:CustomLabel ID="FixInnerText5Label" runat="server" CssClass="Ellipsis AreaName" Width="135" TextWordNo="7"></icrop:CustomLabel>
                        <icrop:CustomLabel ID="InnerText5CounterLabel" runat="server" CssClass="Ellipsis ChipCounter" Width="25"></icrop:CustomLabel>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="InsetHeaderShadowDiv"></div>
                <div id="scrollDiv">
                    <div id="PullDownToRefreshDiv" class="PullDownToRefreshDiv"></div>
                    <div class="BaseDiv selWidth3 DatasSet">
                  	    <div id="LeftDataRowDiv" class="LeftDataRowDiv"></div>
                        <div id="RightDataRowDiv" class="RightDataRowDiv"></div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </div>
    </div>
    </div>
    </div>

    <!-- ここからポップアップ用 -->
    <div id="PopBase" style="display:none;">
        <div id="PopoverTitleBoxDiv">
            <icrop:CustomLabel ID="DetailsHederLabel" runat="server" TextWordNo="12" CssClass="Ellipsis"></icrop:CustomLabel>
        </div>
        <div id="PopoverDataBoxDiv">
            <asp:UpdatePanel ID="ContentUpdatePanelDetail" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenSelectedVisitSeq" runat="server" />
                    <asp:HiddenField ID="HiddenDetailsROButtonStatus" runat="server" />
                    <asp:HiddenField ID="HiddenDetailsCustomerButtonStatus" runat="server" />
                    <asp:HiddenField ID="HiddenServerTime" runat="server" />
                    <asp:HiddenField ID="HiddenDeliveryPlanUpdateCount" runat="server" />
                    <asp:HiddenField ID="HiddenSelectedDisplayArea" runat="server" />

                    <div id="PopoverScrollDiv">
                    <div>
                    <div id="InnerDataBoxDiv">
                        <div  id="StatsInfoAreaDiv" class="StatsInfoAreaDiv">
                            <div id="StatsInfoInnaerDataBoxDiv" class="StatsInfoInnaerDataBoxDiv">
                                <div class="StatsDiv" id="StatsDiv">
                                    <div id="AiconStatsDiv">
                                        <icrop:CustomLabel ID="AiconStatsLabel" runat="server" Width="270"></icrop:CustomLabel>
                                    </div>
                                    <div id="InterruptionCauseDiv">
                                        <asp:Repeater ID="InterruptionCauseRepeater" runat="server">
                                            <HeaderTemplate>
                                                <div id="addStatus" class="addStatus">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <icrop:CustomLabel ID="InterruptionCauseLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("InterruptionCause")) %>' CssClass="Ellipsis" Width="260"></icrop:CustomLabel>
                                                <icrop:CustomLabel ID="InterruptionDetailsLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("InterruptionDetails")) %>' CssClass="Ellipsis" Width="260"></icrop:CustomLabel>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                </div>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="AddInformationBox">
                  	                    <div class="AddInformationPlan">
                                            <icrop:CustomLabel ID="FixDeliveryTimeLabel" runat="server" Width="52"  TextWordNo="13" ></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="DeliveryTimeLabel" runat="server" CssClass="Ellipsis" UseEllipsis="true" ></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="FixSlashLabel" runat="server" TextWordNo="14"  ></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="ChangeCountLabel" runat="server" ></icrop:CustomLabel>
                                        </div>
                  	                    <div class="AddInformationArrow"><icrop:CustomLabel ID="FixDownArrow" runat="server" TextWordNo="16" ></icrop:CustomLabel></div>
                  	                    <div class="AddInformationExpected">
                                            <icrop:CustomLabel ID="FixDeliveryEstimateLabel" runat="server" Width="65" TextWordNo="17"  ></icrop:CustomLabel>
                                            <icrop:CustomLabel ID="DeliveryEstimateLabel" runat="server" CssClass="Ellipsis"></icrop:CustomLabel>
                                        </div>
                                    </div>
                                </div>
                                <div id="HeadInfomationPullDiv" class="HeadInfomationPullDiv">
              	                    <ul>
                                        <div id="ChangeTimeRepeaterDiv">
                                        <asp:Repeater ID="ChangeTimeRepeater" runat="server">
                                        <ItemTemplate>
                	                        <li>
                    	                        <div class="ChangeTimeDiv">
                                                    <icrop:CustomLabel ID="ChangeFromTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeFromTime")) %>' CssClass="Ellipsis" UseEllipsis="true" ></icrop:CustomLabel>
                                                    <icrop:CustomLabel ID="RightArrowLabel" runat="server" CssClass="Ellipsis"></icrop:CustomLabel>
                                                    <icrop:CustomLabel ID="ChangeToTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("ChangeToTime")) %>' CssClass="Ellipsis" UseEllipsis="true" ></icrop:CustomLabel>
                                                </div>
                    	                        <div class="UpdateTimeDiv"><icrop:CustomLabel ID="UpdateTimeLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdateTime")) %>' CssClass="Ellipsis" UseEllipsis="true" ></icrop:CustomLabel></div>
                                                <div class="UpdatePretextDiv"><icrop:CustomLabel ID="UpdatePretextLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("UpdatePretext")) %>' CssClass="Ellipsis" Width="260" UseEllipsis="true" ></icrop:CustomLabel></div>
                                            </li>
                                        </ItemTemplate>
                                        </asp:Repeater>
                                        </div>
                	                    <li class="PullBtn"><icrop:CustomLabel ID="FixUpArrow" runat="server" TextWordNo="36" CssClass="Ellipsis" ></icrop:CustomLabel></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div>
                            <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                                <tr>
                                    <th><icrop:CustomLabel ID="FixVclregNoLabel" runat="server" TextWordNo="19" Width="70"></icrop:CustomLabel></th>
                                    <td><icrop:CustomLabel ID="VclregNoLabel" runat="server" Width="110"></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="DetailsProvince" runat="server" Text="" Width="110" ></icrop:CustomLabel>
                                        <div class="MagnifyingGlass">
                                            <div class="IcnSet">
                                                <icrop:CustomLabel ID="DetailsRightIconD" runat="server" TextWordNo="20" CssClass="RightIcnD" Visible="FALSE"></icrop:CustomLabel>
                                                <%-- 2018/06/15 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示　START--%>
                                                <%-- <icrop:CustomLabel ID="DetailsRightIconI" runat="server" TextWordNo="21" CssClass="RightIcnI" Visible="FALSE"></icrop:CustomLabel> --%>
                                                <icrop:CustomLabel ID="DetailsRightIconP" runat="server" TextWordNo="10005" CssClass="RightIcnP" Visible="FALSE"></icrop:CustomLabel>
                                                <%-- 2018/06/15 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示　END--%>
                                                <icrop:CustomLabel ID="DetailsRightIconS" runat="server" TextWordNo="22" CssClass="RightIcnS" Visible="FALSE"></icrop:CustomLabel>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="ListEnd"><icrop:CustomLabel ID="FixCarModelLabel" runat="server" TextWordNo="23" Width="70"></icrop:CustomLabel></th>
                                    <td class="ListEnd">
                                        <icrop:CustomLabel ID="CarModelLabel" runat="server" Width="70" ></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="CarGradeLabel" runat="server" CssClass="addSmaleText" Width="110" ></icrop:CustomLabel>
                                    </td>
                                </tr>
                            </table>

                            <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                                <tr>
                                    <th><icrop:CustomLabel ID="FixCustomerNameLabel" runat="server" TextWordNo="24" Width="70" UseEllipsis="true" ></icrop:CustomLabel></th>
                                    <td><icrop:CustomLabel ID="CustomerNameLabel" runat="server" Width="150" UseEllipsis="true" ></icrop:CustomLabel>
                                        <%-- 2018/06/15 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示　START--%>
                                        <icrop:CustomLabel ID="DetailsRightIconL" runat="server" TextWordNo="10006" CssClass="RightIcnL" Visible="FALSE"></icrop:CustomLabel>
                                        <%-- 2018/06/15 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示　END--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th><icrop:CustomLabel ID="FixTelNoLable" runat="server" TextWordNo="25" Width="70"></icrop:CustomLabel></th>
                                    <td><icrop:CustomLabel ID="TelNoLable" runat="server" Width="190"></icrop:CustomLabel></td>
                                </tr>
                                <tr>
                                    <th class="ListEnd"><icrop:CustomLabel ID="FixPortableTelNoLable" runat="server" TextWordNo="26" Width="70"></icrop:CustomLabel></th>
                                    <td class="ListEnd"><icrop:CustomLabel ID="PortableTelNoLable" runat="server" Width="190"></icrop:CustomLabel></td>
                                </tr>
                            </table>

                            <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                                <tr>
                                    <th><icrop:CustomLabel ID="FixServiceContentsLable" runat="server" TextWordNo="27" Width="70" ></icrop:CustomLabel></th>
                                    <td><icrop:CustomLabel ID="ServiceContentsLable" runat="server" Width="190" ></icrop:CustomLabel></td>
                                </tr>
                                <tr>
                                    <th class="ListEnd"><icrop:CustomLabel ID="FixWaitPlanLabel" runat="server" TextWordNo="28" CssClass="Ellipsis" Width="80" UseEllipsis="true" ></icrop:CustomLabel></th>
                                    <td class="ListEnd"><icrop:CustomLabel ID="WaitPlanLabel" runat="server" CssClass="Ellipsis" Width="190" UseEllipsis="true" ></icrop:CustomLabel></td>
                                </tr>
                            </table>

                            <table border="0" cellspacing="0" cellpadding="0" class="ListSet" ID="DrawerTable" runat="server" style="display:none;">
                                <tr>
                                    <th class="ListEnd"><icrop:CustomLabel ID="FixDrawerLabel" runat="server" TextWordNo="39" CssClass="Ellipsis" Width="80" UseEllipsis="true" ></icrop:CustomLabel></th>
                                    <td class="ListEnd"><icrop:CustomLabel ID="DrawerLabel" runat="server" CssClass="Ellipsis" Width="190" UseEllipsis="true" ></icrop:CustomLabel></td>
                                </tr>
                            </table>
                        </div>

                    </div>
                    <asp:Button ID="HiddenButtonDetailPopup" CssClass="HiddenButton"  runat="server" />
                    </div>
                    </div>
                    <div id="loadingroInfomation"></div>
                    
                </ContentTemplate>
            </asp:UpdatePanel>
            

            <div id="FooterButtonBoxDiv" class="FooterButtonBoxDiv">
                <div id="FooterButton01" ></div>
                <div id="FooterButton02" ></div>

                <asp:Button ID="HiddenButtonDetailCustomer" CssClass="HiddenButton" runat="server" />
                <asp:Button ID="HiddenButtonDetailRo" CssClass="HiddenButton" runat="server" />
            </div>
        </div>
    </div>
		<!-- ここまでポップアップ用 -->

        <%'プルダウンリフレッシュのレイアウトテンプレートエリア %>
        <div id="pullDownToRefreshTemplate" style="display:none">
            <!--プルダウンリフレッシュエリア START-->
            <div class="pullDownToRefresh step0">
                <!--内部ボックス-->
                <div class="pullDownToRefresh-inBox">
                    <!--中央寄せのアイコン＆テキスト表示エリア-->    
                    <div class="pullDownToRefresh-center">
                        <!--上下矢印アイコン-->
                        <span class="pullDownToRefresh-icon"></span>
                        <!--読み込み中アイコン-->
                        <span class="pullDownToRefresh-loding"></span>
                        <!--テキスト-->
                        <div class="pullDownToRefresh-text">
                            <!--メッセージ-->
                            <span class="pullDownToRefresh-textBlock pullDownToRefresh-message">
                                <icrop:CustomLabel ID="FixMessagStep0" runat="server" TextWordNo="8" CssClass="pullDownToRefresh-message-step0"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep1" runat="server" TextWordNo="9" CssClass="pullDownToRefresh-message-step1"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep2" runat="server" TextWordNo="10" CssClass="pullDownToRefresh-message-step2"></icrop:CustomLabel><br />
                                <icrop:CustomLabel ID="FixMessageUpdateTime" runat="server" TextWordNo="11" CssClass="pullDownToRefresh-message-updateTime"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="MessageUpdateTime" runat="server"  CssClass="pullDownToRefresh-message-updateTime"></icrop:CustomLabel>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <!--プルダウンリフレッシュエリア END-->
        </div>

        <%'サーバー処理中のオーバーレイとアイコン %>
        <div id="serverProcessOverlayBlack"></div>
        <div id="serverProcessIcon"></div>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <%-- 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START--%>
    <div id="InitFooterArea" runat="server">

        <%-- 2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START--%>
        <%--<div class="InitFooterButton_Space"></div>--%>
        <%-- 2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END--%>

        <%-- 来店管理ボタン --%>
        <%--<div id="FooterButton100" runat="server" onclick="FooterEvent(100);">
            <div id="FooterButtonIcon100" runat="server"></div>
            <div id="FooterButtonName100" runat="server" class="FooterButtonName"><icrop:CustomLabel ID="CustomLabel100" runat="server" TextWordNo="40" UseEllipsis="False"></icrop:CustomLabel></div>
        </div>--%>

        <%-- 全体管理ボタン --%>
        <%--<div id="FooterButton200" runat="server" onclick="FooterEvent(200);">
            <div id="FooterButtonIcon200" runat="server"></div>
            <div id="FooterButtonName200" runat="server" class="FooterButtonName"><icrop:CustomLabel ID="CustomLabel200" runat="server" TextWordNo="41" UseEllipsis="False"></icrop:CustomLabel></div>
        </div>
        <asp:Button ID="FooterButtonDummy100" runat="server" style="display: none" />
        <asp:Button ID="FooterButtonDummy200" runat="server" style="display: none" />--%>

        <div id="FooterButton300" runat="server" onclick="FooterSABtnEvent();">
            <div id="FooterButtonName300" runat="server" class="FooterLabel"><icrop:CustomLabel ID="CustomLabel300" runat="server" TextWordNo="42" UseEllipsis="False"></icrop:CustomLabel></div>
        </div>
        <asp:Button ID="FooterButtonDummy300" runat="server" style="display: none" />

    </div>
    <%-- 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END--%>
</asp:Content>

