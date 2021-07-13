<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3220201.aspx.vb" Inherits="Pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%-- スタイルシート --%>
    <link rel="Stylesheet" href="../Styles/SC3220201/SC3220201.css?20180612000000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3220201/SC3220201.js?20171026000000"></script>	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    
    <%-- サーバー処理中のオーバーレイとアイコン --%>
    <div id="ServerProcessOverlayBlack"></div>
    <div id="ServerProcessIcon"></div>
    
    <%-- メイン START --%>
    <asp:UpdatePanel ID="ContentUpdateMainPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="MainAreaReload" runat="server" style="display:none;" />
            <div id="MainArea">
                <div id="Inner">
      	            <div class="tsl01Img01">
                        <%-- 予約エリア START --%>
        	            <div class="ColumnBox type01">
          	                <h3>
            	                <div class="titleName"><icrop:CustomLabel runat="server" ID="ReserveAreaTitle" width="90px" CssClass="Ellipsis" /></div>
            	                <div class="chipCount"><icrop:CustomLabel runat="server" ID="ReserveAreaChipCount" width="90px" CssClass="Ellipsis" /></div>
                            </h3>
                            <div class="chipsArea" id="ReserveArea" runat="server">
                                <div>
                                    <ul>
                                        <asp:Repeater runat="server" id="ReserveAreaRepeater" EnableViewState="false">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="ColumnContents02Boder ColumnContentsVerticalLineA">
                                                        <div class="ColumnContents02BoderIn" id="chip" runat="server">
                                                            <div class="IcnSet" id="chipIcon" runat="server">
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <div class="RightIcnM" runat="server" id="RightIcnM"><icrop:CustomLabel runat="server" ID="ReserveAreaMIcon" /></div>
                                                                <div class="RightIcnB" runat="server" id="RightIcnB"><icrop:CustomLabel runat="server" ID="ReserveAreaBIcon" /></div>
                                                                <div class="RightIcnE" runat="server" id="RightIcnE"><icrop:CustomLabel runat="server" ID="ReserveAreaEIcon" /></div>
                                                                <div class="RightIcnT" runat="server" id="RightIcnT"><icrop:CustomLabel runat="server" ID="ReserveAreaTIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnD" runat="server" id="RightIcnD"><icrop:CustomLabel runat="server" ID="ReserveAreaReserveIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <%-- <div class="RightIcnI" runat="server" id="RightIcnI"><icrop:CustomLabel runat="server" ID="ReserveAreaIIcon" /></div>--%>
                                                                <div class="RightIcnP" runat="server" id="RightIcnP"><icrop:CustomLabel runat="server" ID="ReserveAreaPIcon" /></div>
                                                                <div class="RightIcnL" runat="server" id="RightIcnL"><icrop:CustomLabel runat="server" ID="ReserveAreaLIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnS" runat="server" id="RightIcnS"><icrop:CustomLabel runat="server" ID="ReserveAreaSIcon" /></div>
                                                            </div>
                                                            <div class="ColumnTextBox" id="chipInfo" runat="server">
                                                                <div class="textA01"><icrop:CustomLabel runat="server" width="75px" CssClass="Ellipsis" ID="ReserveAreaVclNo" /></div>
                                                                <div class="textA02"><icrop:CustomLabel runat="server" width="53px" CssClass="Ellipsis" ID="ReserveAreaName" /></div>
                                                                <div class="textA03"><icrop:CustomLabel runat="server" width="42px" CssClass="Ellipsis" ID="ReserveAreaDeliveryDate" /></div>
                                                                <div class="textA04"><icrop:CustomLabel runat="server" width="35px" CssClass="Ellipsis" ID="ReserveAreaFixitem" /></div>
                                                                <div class="IcnNo"><icrop:CustomLabel runat="server" ID="ReserveAreaAddWork" CssClass="Ellipsis" /></div>
                                                            </div>
                                                        </div>
                                                        <div id="ChipBorder" runat="server"></div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <%-- 予約エリア END --%>

                        <%-- 受付エリア START --%>
                        <div class="ColumnBox type02">
                            <h3>
            	                <div class="titleName"><icrop:CustomLabel runat="server" ID="ReceptionistAreaTitle" width="90px" CssClass="Ellipsis" /></div>
            	                <div class="chipCount"><icrop:CustomLabel runat="server" ID="ReceptionistAreaChipCount" width="90px" CssClass="Ellipsis" /></div>
                            </h3>
                            <div class="chipsArea" id="ReceptionistArea" runat="server">
                                <div>
                                    <ul>
                                        <asp:Repeater runat="server" id="ReceptionistAreaRepeater" EnableViewState="false">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="ColumnContents02Boder ColumnContentsVerticalLineA">
                                                        <div class="ColumnContents02BoderIn" id="chip" runat="server">
                                                            <div class="IcnSet" id="chipIcon" runat="server">
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <div class="RightIcnM" runat="server" id="RightIcnM"><icrop:CustomLabel runat="server" ID="ReceptionistAreaMIcon" /></div>
                                                                <div class="RightIcnB" runat="server" id="RightIcnB"><icrop:CustomLabel runat="server" ID="ReceptionistAreaBIcon" /></div>
                                                                <div class="RightIcnE" runat="server" id="RightIcnE"><icrop:CustomLabel runat="server" ID="ReceptionistAreaEIcon" /></div>
                                                                <div class="RightIcnT" runat="server" id="RightIcnT"><icrop:CustomLabel runat="server" ID="ReceptionistAreaTIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnD" runat="server" id="RightIcnD"><icrop:CustomLabel runat="server" ID="ReceptionistAreaReserveIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <%--<div class="RightIcnI" runat="server" id="RightIcnI"><icrop:CustomLabel runat="server" ID="ReceptionistAreaIIcon" /></div>--%>
                                                                <div class="RightIcnP" runat="server" id="RightIcnP"><icrop:CustomLabel runat="server" ID="ReceptionistAreaPIcon" /></div>
                                                                <div class="RightIcnL" runat="server" id="RightIcnL"><icrop:CustomLabel runat="server" ID="ReceptionistAreaLIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnS" runat="server" id="RightIcnS"><icrop:CustomLabel runat="server" ID="ReceptionistAreaSIcon" /></div>
                                                            </div>
                                                            <div class="ColumnTextBox" id="chipInfo" runat="server">
                                                                <div class="textA01"><icrop:CustomLabel runat="server" width="75px" CssClass="Ellipsis" ID="ReceptionistAreaVclNo" /></div>
                                                                <div class="textA02"><icrop:CustomLabel runat="server" width="53px" CssClass="Ellipsis" ID="ReceptionistAreaName" /></div>
                                                                <div class="textA03"><icrop:CustomLabel runat="server" width="42px" CssClass="Ellipsis" ID="ReceptionistAreaDeliveryDate" /></div>
                                                                <div class="textA04"><icrop:CustomLabel runat="server" width="35px" CssClass="Ellipsis" ID="ReceptionistAreaFixitem" /></div>
                                                                <div class="IcnNo"><icrop:CustomLabel runat="server" ID="ReceptionistAreaAddWork" CssClass="Ellipsis" /></div>
                                                            </div>
                                                        </div>
                                                        <div id="ChipBorder" runat="server"></div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <%-- 受付エリア END --%>
                    
                        <%-- 作業エリア START --%>
                        <div class="ColumnBox type03">
                            <h3>
            	                <div class="titleName"><icrop:CustomLabel runat="server" ID="WorkAreaTitle" width="400px" CssClass="Ellipsis" /></div>
            	                <div class="chipCount"><icrop:CustomLabel runat="server" ID="WorkAreaChipCount" width="90px" CssClass="Ellipsis" /></div>
                            </h3>
                            <div class="chipsArea" id="WorkArea" runat="server">
                                <div>
                                    <ul>
                                        <asp:Repeater runat="server" id="WorkAreaRepeater" EnableViewState="false">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:Repeater runat="server" id="WorkAreaRowRepeater" EnableViewState="false">
                                                        <ItemTemplate>
                                                            <div class="ColumnContents02Boder ColumnContentsVerticalLine01" runat="server" id="mainChip">
                                                                <div class="ColumnContents02BoderIn" id="chip" runat="server">
                                                                        <div class="IcnSet" id="chipIcon" runat="server">
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                        <div class="RightIcnM" runat="server" id="RightIcnM"><icrop:CustomLabel runat="server" ID="WorkAreaMIcon" /></div>
                                                                        <div class="RightIcnB" runat="server" id="RightIcnB"><icrop:CustomLabel runat="server" ID="WorkAreaBIcon" /></div>
                                                                        <div class="RightIcnE" runat="server" id="RightIcnE"><icrop:CustomLabel runat="server" ID="WorkAreaEIcon" /></div>
                                                                        <div class="RightIcnT" runat="server" id="RightIcnT"><icrop:CustomLabel runat="server" ID="WorkAreaTIcon" /></div>
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                        <div class="RightIcnD" runat="server" id="RightIcnD"><icrop:CustomLabel runat="server" ID="WorkAreaReserveIcon" /></div>
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                        <%--<div class="RightIcnI" runat="server" id="RightIcnI"><icrop:CustomLabel runat="server" ID="WorkAreaIIcon" /></div>--%>
                                                                        <div class="RightIcnP" runat="server" id="RightIcnP"><icrop:CustomLabel runat="server" ID="WorkAreaPIcon" /></div>
                                                                        <div class="RightIcnL" runat="server" id="RightIcnL"><icrop:CustomLabel runat="server" ID="WorkAreaLIcon" /></div>
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                        <div class="RightIcnS" runat="server" id="RightIcnS"><icrop:CustomLabel runat="server" ID="WorkAreaSIcon" /></div>
                                                                    </div>
                                                                    <div class="ColumnTextBox" id="chipInfo" runat="server">
                                                                        <div class="textA01"><icrop:CustomLabel runat="server" width="75px" CssClass="Ellipsis" ID="WorkAreaVclNo" /></div>
                                                                        <div class="textA02"><icrop:CustomLabel runat="server" width="53px" CssClass="Ellipsis" ID="WorkAreaName" /></div>
                                                                        <div class="textA03"><icrop:CustomLabel runat="server" width="42px" CssClass="Ellipsis" ID="WorkAreaDeliveryDate" /></div>
                                                                        <div class="textA04"><icrop:CustomLabel runat="server" width="35px" CssClass="Ellipsis" ID="WorkAreaFixitem" /></div>
                                                                        <div class="IcnNo"><icrop:CustomLabel runat="server" ID="WorkAreaAddWork" /></div>
                                                                    </div>
                                                                </div>
                                                                <div id="ChipBorder" runat="server"></div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <%-- 作業エリア END --%>
                    
                        <%-- 洗車エリア START --%>
                        <div class="ColumnBox type04">
                            <h3>
            	                <div class="titleName"><icrop:CustomLabel runat="server" ID="WashAreaTitle" width="90px" CssClass="Ellipsis" /></div>
            	                <div class="chipCount"><icrop:CustomLabel runat="server" ID="WashAreaChipCount" width="90px" CssClass="Ellipsis" /></div>
                            </h3>
                            <div class="chipsArea" id="WashArea" runat="server">
                                <div>
                                    <ul>
                                        <asp:Repeater runat="server" id="WashAreaRepeater" EnableViewState="false">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="ColumnContents02Boder ColumnContentsVerticalLineA">
                                                        <div class="ColumnContents02BoderIn" id="chip" runat="server">
                                                            <div class="IcnSet" id="chipIcon" runat="server">
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <div class="RightIcnM" runat="server" id="RightIcnM"><icrop:CustomLabel runat="server" ID="WashAreaMIcon" /></div>
                                                                <div class="RightIcnB" runat="server" id="RightIcnB"><icrop:CustomLabel runat="server" ID="WashAreaBIcon" /></div>
                                                                <div class="RightIcnE" runat="server" id="RightIcnE"><icrop:CustomLabel runat="server" ID="WashAreaEIcon" /></div>
                                                                <div class="RightIcnT" runat="server" id="RightIcnT"><icrop:CustomLabel runat="server" ID="WashAreaTIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnD" runat="server" id="RightIcnD"><icrop:CustomLabel runat="server" ID="WashAreaReserveIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <%--<div class="RightIcnI" runat="server" id="RightIcnI"><icrop:CustomLabel runat="server" ID="WashAreaIIcon" /></div>--%>
                                                                <div class="RightIcnP" runat="server" id="RightIcnP"><icrop:CustomLabel runat="server" ID="WashAreaPIcon" /></div>
                                                                <div class="RightIcnL" runat="server" id="RightIcnL"><icrop:CustomLabel runat="server" ID="WashAreaLIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnS" runat="server" id="RightIcnS"><icrop:CustomLabel runat="server" ID="WashAreaSIcon" /></div>
                                                            </div>
                                                            <div class="ColumnTextBox" id="chipInfo" runat="server">
                                                                <div class="textA01"><icrop:CustomLabel runat="server" width="75px" CssClass="Ellipsis" ID="WashAreaVclNo" /></div>
                                                                <div class="textA02"><icrop:CustomLabel runat="server" width="53px" CssClass="Ellipsis" ID="WashAreaName" /></div>
                                                                <div class="textA03"><icrop:CustomLabel runat="server" width="42px" CssClass="Ellipsis" ID="WashAreaDeliveryDate" /></div>
                                                                <div class="textA04"><icrop:CustomLabel runat="server" width="35px" CssClass="Ellipsis" ID="WashAreaFixitem" /></div>
                                                                <div class="IcnNo"><icrop:CustomLabel runat="server" ID="WashAreaAddWork" /></div>
                                                            </div>
                                                        </div>
                                                        <div id="ChipBorder" runat="server"></div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <%-- 洗車エリア END --%>
                    
                        <%-- 納車エリア START --%>
                        <div class="ColumnBox type05">
                            <h3>
            	                <div class="titleName"><icrop:CustomLabel runat="server" ID="DeliveryAreaTitle" width="90px" CssClass="Ellipsis" /></div>
            	                <div class="chipCount"><icrop:CustomLabel runat="server" ID="DeliveryAreaChipCount" width="90px" CssClass="Ellipsis" /></div>
                            </h3>
                            <div class="chipsArea" id="DeliveryArea" runat="server">
                                <div>
                                    <ul>
                                        <asp:Repeater runat="server" id="DeliveryAreaRepeater" EnableViewState="false">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="ColumnContents02Boder ColumnContentsVerticalLineA">
                                                        <div class="ColumnContents02BoderIn" id="chip" runat="server">
                                                                    <div class="IcnSet" id="chipIcon" runat="server">
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <div class="RightIcnM" runat="server" id="RightIcnM"><icrop:CustomLabel runat="server" ID="DeliveryAreaMIcon" /></div>
                                                                <div class="RightIcnB" runat="server" id="RightIcnB"><icrop:CustomLabel runat="server" ID="DeliveryAreaBIcon" /></div>
                                                                <div class="RightIcnE" runat="server" id="RightIcnE"><icrop:CustomLabel runat="server" ID="DeliveryAreaEIcon" /></div>
                                                                <div class="RightIcnT" runat="server" id="RightIcnT"><icrop:CustomLabel runat="server" ID="DeliveryAreaTIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnD" runat="server" id="RightIcnD"><icrop:CustomLabel runat="server" ID="DeliveryAreaReserveIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                <%--<div class="RightIcnI" runat="server" id="RightIcnI"><icrop:CustomLabel runat="server" ID="DeliveryAreaIIcon" /></div>--%>
                                                                <div class="RightIcnP" runat="server" id="RightIcnP"><icrop:CustomLabel runat="server" ID="DeliveryAreaPIcon" /></div>
                                                                <div class="RightIcnL" runat="server" id="RightIcnL"><icrop:CustomLabel runat="server" ID="DeliveryAreaLIcon" /></div>
                                                                <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                <div class="RightIcnS" runat="server" id="RightIcnS"><icrop:CustomLabel runat="server" ID="DeliveryAreaSIcon" /></div>
                                                            </div>
                                                            <div class="ColumnTextBox" id="chipInfo" runat="server">
                                                                <div class="textA01"><icrop:CustomLabel runat="server" width="75px" CssClass="Ellipsis" ID="DeliveryAreaVclNo" /></div>
                                                                <div class="textA02"><icrop:CustomLabel runat="server" width="53px" CssClass="Ellipsis" ID="DeliveryAreaName" /></div>
                                                                <div class="textA03"><icrop:CustomLabel runat="server" width="42px" CssClass="Ellipsis" ID="DeliveryAreaDeliveryDate" /></div>
                                                                <div class="textA04"><icrop:CustomLabel runat="server" width="35px" CssClass="Ellipsis" ID="DeliveryAreaFixitem" /></div>
                                                                <div class="IcnNo"><icrop:CustomLabel runat="server" ID="DeliveryAreaAddWork" /></div>
                                                            </div>
                                                        </div>
                                                        <div id="ChipBorder" runat="server"></div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <%-- 納車エリア END --%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- メイン END --%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <%-- 来店管理ボタン --%>
   <%-- <div class="InnerBox01">
        <div class="VisitManagementFooterIcon"></div>
        <div class="text"><icrop:CustomLabel runat="server" ID="VisitManagementFooterLabel" /></div>
        <asp:Button runat="server" ID="VisitManagementFooterButton" style="display:none;" />
    </div>--%>
    <%-- 全体管理ボタン --%>
   <%-- <div class="InnerBox02">
        <div class="AllManagementFooterIcon"></div>
        <div class="ActiveText"><icrop:CustomLabel runat="server" ID="AllManagementFooterLabel" /></div>
    </div>--%>
     <div id="CustomFooterButtonSA" class="CustomFooterButtonSA" runat="server" >
         <div id="CustomFooterSAName" runat="server" class="CustomFooterSALabel"><icrop:CustomLabel ID="SAButtonLabel" runat="server" TextWordNo="14" UseEllipsis="False"></icrop:CustomLabel></div>
     </div>
     <asp:Button ID="FooterButtonSADummy" runat="server" style="display: none" />
</asp:Content>

