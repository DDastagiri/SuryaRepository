<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3100401.aspx.vb" Inherits="Pages_SC3100401" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/SC3100401/SC3100401.css?201806190000001" type="text/css" media="screen,print" />
    <%--スクリプト--%>
    <script type="text/javascript" src="../Scripts/SC3100401/SC3100401.js?20190614000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100401/SC3100401.CustomLabelEx.js?201402100000002"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
 <asp:ScriptManager ID="AjaxListManager" runat="server" EnablePageMethods="True" ></asp:ScriptManager>
        <!-- アクティブインジケータ -->
        <div id="LoadingScreen">
          <div id="LoadingWrap">
            <div class="LoadingIcn">
              <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
            </div>
          </div>
        </div>
        <!-- グレーアウト -->
        <div class="BlackBack" style="display:none"></div>
        <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START --%>
        <div id="VehicleListOverlayBlack"></div>
        <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END --%>
        <!-- ここからメインブロック -->
        <div class="InnerBox">
          <div id="Inner">
            <asp:UpdatePanel ID="AjaxListPanelMain" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <div class="MainBox">
                  <div class="LeftBox">
                    <ul class="LeftBoxTitle">
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel01" runat="server" CssClass="Ellipsis" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel02" runat="server" CssClass="Ellipsis" TextWordNo="3" Width="46" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel03" runat="server" CssClass="Ellipsis" TextWordNo="4" Width="43" ></icrop:CustomLabel><br />
                    	  <icrop:CustomLabel ID="LeftBoxTitleLabel04" runat="server" CssClass="Ellipsis" TextWordNo="5" Width="43" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel05" runat="server" CssClass="Ellipsis" TextWordNo="6" Width="43" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel06" runat="server" CssClass="Ellipsis" TextWordNo="7" Width="110" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel07" runat="server" CssClass="Ellipsis" TextWordNo="8" Width="147" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel08" runat="server" CssClass="Ellipsis" TextWordNo="9" Width="101" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel09" runat="server" CssClass="Ellipsis" TextWordNo="10" Width="55" ></icrop:CustomLabel><br />
                   		  <icrop:CustomLabel ID="LeftBoxTitleLabel10" runat="server" CssClass="Ellipsis" TextWordNo="34" Width="55" ></icrop:CustomLabel></li>
                      <li><icrop:CustomLabel ID="LeftBoxTitleLabel11" runat="server" CssClass="Ellipsis" TextWordNo="11" Width="99" ></icrop:CustomLabel></li>
                    </ul>
                    <div class="LeftBoxList">
                      <div class="LeftBoxListDisabled" ></div>
                      <ul class="LeftBoxListDammy">
                        <li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li>
                        <div class="Stripe01"></div><div class="Stripe02"></div><div class="Stripe03"></div><div class="Stripe04"></div><div class="Stripe05"></div>
                        <div class="Stripe06"></div><div class="Stripe07"></div><div class="Stripe08"></div><div class="Stripe09"></div><div class="Stripe10"></div>
                      </ul>
                      <ul class="LeftBoxListSet">
                        <asp:UpdatePanel ID="AjaxListPanelReception" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                            <asp:Repeater ID="RepeaterReceptionList" runat="server">
                              <ItemTemplate>
                                <li id="ReceptionList" runat="server" >
                                  <div class="WhiteBack" style="display:none"></div>
                                  <ul class="ListCassette">
                                    <li>
                                      <div id="RecCheckBox" class="Check"></div>
                                      <div class="CheckClick"></div>
                                    </li>
                                    <li>
                                      <div id="CallImage" class="Car_Call" runat="server" Visible="False" ></div>
                                      <%--<div class="CarIcon" ></div>
                                      <icrop:CustomLabel ID="LeftBoxListLabel01" runat="server" CssClass="Ellipsis CarText" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNAME"))%>'></icrop:CustomLabel>
                                      <div class="sIconSet">
                                        <div id="ReserveIcon" class="sIcon01" runat="server" Visible="False"></div>
                                        <div class="sIcon02" style="display:none"></div>
                                      </div>--%>
                                      <div id="CallText" class="CallTextArea" runat="server" Visible="False">
                                        <icrop:CustomLabel ID="LeftBoxListLabel00" runat="server" CssClass="LeftBoxListLabel00 Ellipsis" TextWordNo="32" ></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="LeftBoxListLabel01" runat="server" CssClass="LeftBoxListLabel01 Ellipsis" TextWordNo="33" ></icrop:CustomLabel>
                                      </div>
                                    </li>
                                    <li>
                                      <div class="No">
                                        <icrop:CustomLabel ID="LeftBoxListLabel02" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CALLNO"))%>'></icrop:CustomLabel>
                                      </div>
                                      <div id="NoDisabled" class="NoDisabled" runat="server" Visible="False" ></div>
                                    </li>
                                    <li>
                                      <div class="VisitTimeArea">
                                        <icrop:CustomLabel ID="LeftBoxListLabel03" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITTIMESTAMP"))%>' ></icrop:CustomLabel>
                                        <div class="Ellipsis ProgressTimeArea">
                                          <icrop:CustomLabel ID="LeftBoxListLabel04" runat="server" CssClass="Ellipsis" TextWordNo="13" ></icrop:CustomLabel>
                                          <icrop:CustomLabel ID="LeftBoxListLabel05" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ELAPSEDTIME"))%>' ></icrop:CustomLabel>
                                          <icrop:CustomLabel ID="LeftBoxListLabel06" runat="server" CssClass="Ellipsis" TextWordNo="14" ></icrop:CustomLabel>
                                        </div>
                                      </div>
                                    </li>
                                    <li>
                                      <div id="ReserveArea" class="TimeWhite" runat="server">
                                        <icrop:CustomLabel ID="LeftBoxListLabel07" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "MERCHANDISENAME"))%>' ></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="LeftBoxListLabel08" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "PLANSTARTDATE"))%>' ></icrop:CustomLabel>
                                      </div>
                                    </li>
                                    <li>
                                      <div class="CarTypeText">
                                        <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START --%>
                                        <%-- 二重エンコード問題修正 --%>
                                        <%-- <asp:TextBox ID="LeftBoxListTextBox01" class="TextArea CarTypeTextBoxItem" runat="server" MaxLength="16" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNO"))%>' ></asp:TextBox> --%>
                                        <asp:TextBox ID="LeftBoxListTextBox01" class="TextArea CarTypeTextBoxItem" runat="server" MaxLength="16"></asp:TextBox>
                                        <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END --%>
                                      </div>
                                      <icrop:CustomLabel ID="LeftBoxListLabel09" runat="server" CssClass="Ellipsis CarType01" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VEHICLENAME"))%>' ></icrop:CustomLabel>
                                      <div id="CarTypeDisabled" class="CarTypeDisabled" runat="server" Visible="False" ></div>
                                      <%-- 2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                      <div ID="RightIcnP" runat="server" text="" visible="False" class="RightIcnP"></div>
                                      <%-- 2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                    </li>
                                    <li>
                                      <div class="VIPIcon" style="display:none"></div>
                                      <icrop:CustomLabel ID="LeftBoxListLabel10" runat="server" CssClass="Ellipsis OwnerText" ></icrop:CustomLabel>
                                      <%-- 2018/06/11 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                      <div ID="RightIcnL" runat="server" text="" visible="False" class="RightIcnL"></div>
                                      <%-- 2018/06/11 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                      <div class="VisitorEdit">
                                        <asp:TextBox ID="LeftBoxListTextBox02" class="TextArea VisitorTextBoxItem" runat="server" Enabled="True" MaxLength="256"  ></asp:TextBox>
                                      </div>
                                    </li>
                                    <li>
                                      <icrop:CustomLabel ID="LeftBoxListLabel11" runat="server" CssClass="Ellipsis TellNo" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TELNO"))%>' ></icrop:CustomLabel>
                                      <div class="TellNoEdit">
                                        <asp:TextBox ID="LeftBoxListTextBox03" class="TextArea TellNoTextBoxItem" runat="server" Enabled="True" MaxLength="128"  ></asp:TextBox>
                                      </div>
                                    </li>
                                    <li>
                                      <div class="TableNo">
                                        <asp:TextBox ID="LeftBoxListTextBox04" class="TextArea TableNoTextBoxItem" runat="server" Enabled="True" MaxLength="64" ></asp:TextBox>
                                      </div>
                                      <div id="TableNoDisabled" class="TableNoDisabled" runat="server" Visible="False" ></div>
                                    </li>
                                    <li>
                                      <div id="SAButton" class="ClassSAButton" runat="server" Visible="True" ></div>
                                      <%--2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応--%>
                                      <div class="SABtnLabel"><p><icrop:CustomLabel ID="LeftBoxListLabel12" runat="server" CssClass="Ellipsis" Width="63" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "SANAME"))%>' ></icrop:CustomLabel></p></div>
                                      <icrop:CustomLabel ID="LeftBoxListLabel13" runat="server" CssClass="Ellipsis SAText" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DEFAULTSANAME"))%>' ></icrop:CustomLabel>
                                      <div id="SAButtonDisabled" class="SAButtonDisabled" runat="server" Visible="False" ></div>
                                    </li>
                                  </ul>
                                </li>
                              </ItemTemplate>
                            </asp:Repeater>
                          </ContentTemplate>
                        </asp:UpdatePanel>
                      </ul>
                    </div>
                  </div>
                  <div class="RightPopBox" style="display:none">
                    <div class="PopBoxArrow"></div>
                    <div class="PopBoxArrow_shadow"></div>
                  </div>
                  <div class="RightBox">
                    <icrop:CustomLabel ID="RightBoxTitleLabel01" class="RightBoxTitle" runat="server" CssClass="Ellipsis" TextWordNo="20" ></icrop:CustomLabel>
                    <div class="RightBoxList">
                      <ul class="RightBoxListTitle">
                        <li><icrop:CustomLabel ID="RightBoxListTitleLabel02" runat="server" CssClass="Ellipsis" Width="111" TextWordNo="21" ></icrop:CustomLabel></li>
                        <li><icrop:CustomLabel ID="RightBoxListTitleLabel03" runat="server" CssClass="Ellipsis" Width="66" TextWordNo="22" ></icrop:CustomLabel></li>
                        <li><icrop:CustomLabel ID="RightBoxListTitleLabel04" runat="server" CssClass="Ellipsis" Width="52" TextWordNo="23" ></icrop:CustomLabel></li>
                      </ul>
                      <div class="RightBoxSAList">
                        <ul class="RightBoxListSet">
                          <asp:Repeater ID="RepeaterSAList" runat="server">
                            <ItemTemplate>
                              <li class="SAList">
                                <div class="SAListClick" id="SAAccount" runat="server"></div>
                                <ul class="Cassette">
                                  <li>
                                    <div id="SACheck" class="SACheck"></div>
                                    <div class="SANameLabel">
                                      <icrop:CustomLabel ID="RightBoxListSALabel01" runat="server" CssClass="Ellipsis" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "USERNAME"))%>' ></icrop:CustomLabel>
                                    </div>
                                  </li>
                                  <li>
                                    <div class="WorkTimeLabel">
                                      <icrop:CustomLabel ID="RightBoxListSALabel02" runat="server" CssClass="Ellipsis" Text='<%# Server.HtmlEncode("" & DataBinder.Eval(Container.DataItem, "DISPLOADTIME"))%>' ></icrop:CustomLabel>
                                    </div>
                                  </li>
                                  <li>
                                    <div class="WorkLabel">
                                      <icrop:CustomLabel ID="RightBoxListSALabel03" runat="server" CssClass="Ellipsis" Text='<%# Server.HtmlEncode("" & DataBinder.Eval(Container.DataItem, "DISPLOADCOUNT"))%>' ></icrop:CustomLabel>
                                    </div>
                                  </li>
                                </ul>
                              </li>
                            </ItemTemplate>
                          </asp:Repeater>
                        </ul>
                      </div>
                      <div class="RightBoxButton">
                        <div class="RightBoxSARegisterButton" style="display:none" >
                          <icrop:CustomLabel ID="RightBoxListSALabel04" runat="server" CssClass="Ellipsis" TextWordNo="25" Width="208" ></icrop:CustomLabel>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <asp:Button ID="MainLoadingButton" runat="server" style="display:none" />
                <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START --%>
                <%--<asp:Button ID="RegisterRegNoButton" runat="server" style="display:none" />--%>
                <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END --%>
                <asp:Button ID="RegisterAssignButton" runat="server" style="display:none" />
                <asp:Button ID="VisitManageButton" runat="server" style="display:none" />
                <asp:Button ID="CustomFooterButton" runat="server" style="display:none" />

                <asp:Button ID="ReserveManagementButton" runat="server" style="display:none" />
                <asp:Button ID="RepairOrderListButton" runat="server" style="display:none" />
                <asp:Button ID="WholeManagementButton" runat="server" style="display:none" />

                <asp:HiddenField ID="HiddenClientMessage" runat="server" />
                <asp:HiddenField ID="HiddenReceptionListCount" runat="server" />
                <asp:HiddenField ID="HiddenVisitSeq" runat="server" />
                <asp:HiddenField ID="HiddenReserveId" runat="server" />
                <asp:HiddenField ID="HiddenRegNo" runat="server" />
                <asp:HiddenField ID="HiddenSAAccount" runat="server" />
                <asp:HiddenField ID="HiddenUpDateDate" runat="server" />
                <asp:HiddenField ID="HiddenEventKeyID" runat="server" />
                <asp:HiddenField ID="HiddenReceptFlag" runat="server" />
              </ContentTemplate>
            </asp:UpdatePanel>
            <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START --%>
            <%-- 車両一覧ポップアップエリア --%>
            <asp:UpdatePanel ID="ContentUpdatePopupPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="PopUpVehicleListClass" style="display: none;">
                        <div class="PopUpVehicleListHeaderClass">
                            <icrop:CustomLabel runat="server" ID="PopUpVehicleListHeaderLabel" CssClass="Ellipsis"
                                Width="600px" Height="30px" TextWordNo="43"></icrop:CustomLabel>
                        </div>
                        <div class="PopUpVehicleListContentsClass" style="overflow: hidden;">
                            <div style="padding-bottom: 2px;">
                                <asp:Repeater ID="VehicleListRepeater" runat="server" EnableViewState="false">
                                    <ItemTemplate>
                                        <div runat="server" id="VehicleListItem" class="VehicleListItemClass">
                                            <div class="VehicleListItemContentsClass">
                                                <table>
                                                    <tr valign="middle" style="height: 50px;">
                                                        <td>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="CustomerName" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="TelNumber" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="ModelName" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="VclVin" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="MerchandiseName" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                            <div class="VehicleListItemText">
                                                                <icrop:CustomLabel runat="server" ID="PlanStartEndDate" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                            </div>
                                                        </td>
                                                </table>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="PopUpVehicleListFooterClass" id="PopUpVehicleListFooter" runat="server">
                            <asp:Button ID="PopUpVehicleListFooterButton" CssClass="PopUpVehicleListFooterButtonOff"
                                runat="server" OnClientClick="return RegistNewCustomer();" />
                        </div>
                    </div>
                    <asp:Button ID="PopupVehicleListEventButton" runat="server" Style="display: none;" />
                    <asp:HiddenField runat="server" ID="HiddenSelectCstId" />
                    <asp:HiddenField runat="server" ID="HiddenSelectVclId" />
                    <asp:HiddenField runat="server" ID="HiddenSelectRezId" />
                    <asp:HiddenField runat="server" ID="HiddenVehicleListDisplayType" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- ダミーボタンエリア --%>
            <asp:UpdatePanel ID="ContentUpdateButtonPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="RegisterRegNoButton" runat="server" style="display:none" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END --%>
          </div>
        </div>
		<!-- ここまでメインブロック -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
        <div class="FooterArea">
          <%-- 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START --%>
          <%--<div class="MainFooter">
            <dl>
              <dd class="BtnBace BtnManage">
                <div class="InnerBox01">
                  <div class="BtnManageImage"></div>
                  <icrop:CustomLabel ID="FooterLabel01" runat="server" CssClass="Ellipsis MainFooterLabel" Width="68" TextWordNo="26" ></icrop:CustomLabel>
                  <div class="ManageImageOn" style="display:none"></div>
                </div>
              </dd>
            </dl>
          </div>--%>
          <%-- 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END --%>
          <div class="CheckFooter" style="display:none" >
            <dl>
		      <dd class="BtnRedBace BtnDelete">
		        <div class="InnerBox01">
                  <icrop:CustomLabel ID="FooterLabel02" runat="server" CssClass="Ellipsis FooterLabel" Width="80" TextWordNo="29" ></icrop:CustomLabel>
	            </div>
	          </dd>
		      <dd class="BtnRedBace BtnCancel">
		        <div class="InnerBox01">
                  <icrop:CustomLabel ID="FooterLabel03" runat="server" CssClass="Ellipsis FooterLabel" Width="80" TextWordNo="28" ></icrop:CustomLabel><br />
                  <icrop:CustomLabel ID="FooterLabel04" runat="server" CssClass="Ellipsis FooterLabel" Width="80" TextWordNo="36" ></icrop:CustomLabel>
	            </div>
	          </dd>
		      <dd class="BtnBlueBace BtnCall">
		        <div class="InnerBox01">
                  <icrop:CustomLabel ID="FooterLabel05" runat="server" CssClass="Ellipsis FooterLabel" Width="80" TextWordNo="27" ></icrop:CustomLabel>
	            </div>
	          </dd>
	        </dl>
          </div>
        </div>
</asp:Content>