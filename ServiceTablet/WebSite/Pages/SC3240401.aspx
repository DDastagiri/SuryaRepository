<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3240401.aspx.vb" Inherits="SC3240401" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3240401/SC3240401.css?20180627000000" />
    <script type="text/javascript" src="../Scripts/SC3240401/SC3240401.Fingerscroll.js?20130901000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240401/SC3240401.Main.js?20170914000001"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager><br/>
    
    <div id="ServerProcessOverlayBlack"></div>
    <div id="ServerProcessIcon"></div>
    <div id="ServerProcessListOverlay"></div>
    <div id="ServerProcessListIcon"></div>
    <div id="OrderListOverlayBlack"></div>

<div class="MainBorderBox">
    <div id="mainblockContent">
        <div class="mainblockContentArea">
            <asp:UpdatePanel ID="ContentUpdateMainPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="MainAreaReload" runat="server" style="display:none;" />
                    <asp:Button ID="RegisterSortButton" runat="server" style="display:none;" />
                    <asp:Button ID="CustomerSortButton" runat="server" style="display:none;" />
                    <asp:Button ID="BackPageButton" runat="server" style="display:none;" />
                    <asp:Button ID="NextPageButton" runat="server" style="display:none;" />
                    <div class="mainblockContentAreaWrap">
                        <div id="mainblockContentAreaNCM0201">
                            <h2 class="mainblockContentAreaNCM0201Result"><asp:Label runat="server" ID="SearchCount"></asp:Label></h2>
                            <div class="mainblockContentAreaNCM0201ResultList">
                                <ul>
                                    <li class="RegisterSort">
                                        <icrop:CustomLabel runat="server" ID="RegisterHeader" width="267px" CssClass="Ellipsis UnderLine" />
                                    </li>
                                    <li class="CustomerSort">
                                        <icrop:CustomLabel runat="server" ID="CustomerHeader" width="245px" CssClass="Ellipsis UnderLine" />
                                    </li>
                                    <li><icrop:CustomLabel runat="server" ID="TelMobileHeader" width="130px" CssClass="Ellipsis" /></li>
                                    <li><icrop:CustomLabel runat="server" ID="ReserveHeader" width="285px" CssClass="Ellipsis" /></li>
                                </ul>
                            </div>
							<div ID="NoSearchImage" runat="server">
                                <icrop:CustomLabel runat="server" ID="NoSearchWord" CssClass="Ellipsis" />
                            </div>
                            <div runat="server" id="CustomerSearchArea" class="mainblockContentAreaNCM0201ResultScroll">
                                <div class="mainblockContentAreaNCM0201ResultList2">
                                    <div runat="server" id="BackPage" class="NCM0201ResultListNextMore" style="display:none;">
                                        <icrop:CustomLabel runat="server" ID="BackPageWord" />
                                    </div>
                                    <div runat="server" id="BackPageLoad" class="NCM0201ResultListNextMore" style="display:none;">
                                        <icrop:CustomLabel runat="server" ID="BackPageLoadWord" />
                                        <span class="LoadImage"></span>
                                    </div>
                                    <asp:Repeater runat="server" id="ChipReserveAreaRepeater" EnableViewState="false">
                                        <ItemTemplate>
                                            <ul runat="server" id="chipReserveRow">
                                                <%-- 車両情報エリア --%>
                                                <li runat="server" id="vehicleRecord" class="VehicleRecordClass">
                                                    <div class="RecordAreaDiv">
                                                        <div class="NCM0201ResultList2-1">
                                                            <div class="NCM0201ResultList2-1-1">
                                                                <p class="NCM0201ResultList2-1-1-1"><icrop:CustomLabel runat="server" ID="RegisterNo" width="140px" CssClass="Ellipsis" /></p>
                                                                <p class="NCM0201ResultList2-1-1-2"><icrop:CustomLabel runat="server" ID="VehicleName" width="104px" CssClass="Ellipsis" /></p>
                                                            </div>
                                                            <div class="NCM0201ResultList2-1-2">
                                                                <p class="NCM0201ResultList2-1-1-3"><icrop:CustomLabel runat="server" ID="Vin" width="245px" CssClass="Ellipsis" /></p>
                                                                <div class = "IconArea">
                                                                <%-- 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                    <span runat="server" id="MIcon" class="IcoM" style="display: none;">
                                                                        <icrop:CustomLabel runat="server" ID="MWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="BIcon" class="IcoB" style="display: none;">
                                                                        <icrop:CustomLabel runat="server" ID="BWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="EIcon" class="IcoE" style="display: none;">
                                                                        <icrop:CustomLabel runat="server" ID="EWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="TIcon" class="IcoT" style="display: none;">
                                                                        <icrop:CustomLabel runat="server" ID="TWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="PIcon" class="IcoP" style="display: none;">
                                                                        <icrop:CustomLabel runat="server" ID="PWord" CssClass="Ellipsis" /></span>
                                                                </div>
                                                                <%-- 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </li>
                                                <%-- 顧客情報エリア --%>
                                                <li runat="server" id="customerRecord" class="CustomerRecordClass">
                                                    <div class="RecordAreaDiv">
                                                        <div class="NCM0201ResultList2-2">
                                                            <div class="NCM0201ResultList2-2-1">
                                                            <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START --%>
                                                                <%-- <p class="NCM0201ResultList2-2-1-1"><asp:Image runat="server" ID="CustomerImageIcon" Width="57px" Height="56px" /></p> --%>
                                                                <p class="NCM0201ResultList2-2-1-1"><asp:Image runat="server" ID="CustomerImageIcon" Width="60px" Height="60px" /></p>
                                                            <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END --%>
                                                            </div>
                                                            <div class="NCM0201ResultList2-2-2">
                                                                <p class="NCM0201ResultList2-2-1-2"><icrop:CustomLabel runat="server" ID="CustomerName" width="158px" CssClass="Ellipsis" /></p>
                                                                <p class="NCM0201ResultList2-2-1-3">
                                                                    <span runat="server" id="VipIcon" class="IcoVip2" style="display:none;"><icrop:CustomLabel runat="server" ID="VipWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="MyCompanyIcon" class="IcoJi2" style="display:none;"><icrop:CustomLabel runat="server" ID="MyCompanyWord" CssClass="Ellipsis" /></span>
                                                                    <span runat="server" id="MyVehicleIcon" class="IcoKo2" style="display:none;"><icrop:CustomLabel runat="server" ID="MyVehicleWord" CssClass="Ellipsis" /></span>
                                                                    <%-- 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                    <span runat="server" id="LIcon" class="IcoL" style="display:none;"><icrop:CustomLabel runat="server" ID="LWord" CssClass="Ellipsis" /></span>
                                                                    <%-- 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </li>
                                                <%-- 電話番号情報エリア --%>
                                                <li runat="server" id="telRecord" class="CustomerTelClass">
                                                    <div class="RecordAreaDiv">
                                                        <div class="NCM0201ResultList2-3">
                                                            <div class="NCM0201ResultList2-3-1">
                                                                <p class="NCM0201ResultList2-3-1-3"><icrop:CustomLabel runat="server" ID="MobileNo" width="110px" CssClass="Ellipsis" /></p>
                                                            </div>
                                                            <div class="NCM0201ResultList2-3-2">
                                                                <p class="NCM0201ResultList2-3-1-1"><icrop:CustomLabel runat="server" ID="TelNo" width="110px" CssClass="Ellipsis" /></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </li>
                                                <%-- 予約情報エリア --%>
                                                <li runat="server" id="reserveRecord">
                                                    <div class="RecordAreaDiv">
                                                        <asp:Repeater runat="server" id="ReserveListAreaRepeater" EnableViewState="false">
                                                            <ItemTemplate>
                                                                <div runat="server" id="reserveInfoRecord" class="AppointmentDate">
                                                                    <div class="AppointmentDateBox">
                                                                        <div class="AppointmentDateSet01">
                                                                            <div class="sBox01">
                                                                                <icrop:CustomLabel runat="server" ID="StartDate" width="110px" CssClass="Ellipsis" />
                                                                                <icrop:CustomLabel runat="server" ID="EndDate" width="132px" CssClass="Ellipsis" />
                                                                            </div>
                                                                            <%--<div class="sBox02"><icrop:CustomLabel runat="server" ID="StartEndDate" width="137px" CssClass="Ellipsis" /></div>--%>
                                                                            <div class="sBox03">
                                                                                <p runat="server" ID="ReserveIcon" class=""><icrop:CustomLabel runat="server" ID="ReserveWord" width="23px" CssClass="Ellipsis" /></p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="AppointmentDateSet02">
                                                                            <div class="sBox01"><icrop:CustomLabel runat="server" ID="StallName" width="70px" CssClass="Ellipsis" /></div>
                                                                            <div class="sBox02"><icrop:CustomLabel runat="server" ID="ServiceName" width="42px" CssClass="Ellipsis" /></div>
                                                                            <div runat="server" ID="ServiceInIcon" class="sBox03 icon01"></div>
                                                                            <div class="sBox04"><icrop:CustomLabel runat="server" ID="StaffName" width="134px" CssClass="Ellipsis" /></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div runat="server" id="NextPage" class="NCM0201ResultListNextMore" style="display:none;">
                                        <icrop:CustomLabel runat="server" ID="NextPageWord" />
                                    </div>
                                    <div runat="server" id="NextPageLoad" class="NCM0201ResultListNextMore" style="display:none;">
                                        <icrop:CustomLabel runat="server" ID="NextPageLoadWord" />
                                        <span class="LoadImage"></span>
                                    </div>
                                </div>
                            </div>
                        <!--ここまで中カラム-->
                        </div>
                    </div>
		            <asp:HiddenField runat="server" ID="HiddenOperationCode" />
		            <asp:HiddenField runat="server" ID="HiddenSearchListCount" />
                    <asp:HiddenField runat="server" ID="HiddenRegisterSortType" />
                    <asp:HiddenField runat="server" ID="HiddenCustomerSortType" />
		            <asp:HiddenField runat="server" ID="HiddenSearchType" />
		            <asp:HiddenField runat="server" ID="HiddenSearchValue" />
		            <asp:HiddenField runat="server" ID="HiddenStartIndex" />
		            <asp:HiddenField runat="server" ID="HiddenEndIndex" />
		            <asp:HiddenField runat="server" ID="HiddenLoadCount" />
		            <asp:HiddenField runat="server" ID="HiddenMaxDisplayCount" />
                    <asp:HiddenField runat="server" ID="HiddenSelectStallUseId" />
                    <asp:HiddenField runat="server" ID="HiddenSelectSvcinId" />
                    <asp:HiddenField runat="server" ID="HiddenSelectAddType" />
                    <asp:HiddenField runat="server" ID="HiddenSelectCustomerId" />
                    <asp:HiddenField runat="server" ID="HiddenSelectVehicleId" />
                    <%-- 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START --%>
                    <asp:HiddenField runat="server" ID="HiddenSelectRoNum" />
                    <asp:HiddenField runat="server" ID="HiddenSelectRoSeq" /> 
                    <asp:HiddenField runat="server" ID="HiddenSelectTempFlag" />
                    <asp:HiddenField runat="server" ID="HiddenBranchOperationDateTime" />
                    <%-- 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END --%> 
                </ContentTemplate>
            </asp:UpdatePanel>

            <%-- RO一覧ポップアップエリア --%>
            <asp:UpdatePanel ID="ContentUpdatePopuupPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="PopUpOrderListClass" style="display:none;">
                        <div class="PopUpOrderListHeaderClass">
                            <icrop:CustomLabel runat="server" ID="PopUpOrderListHeaderLabel" CssClass="Ellipsis" Width="560px" Height="30px"></icrop:CustomLabel>
                        </div>
                        <div class="PopUpOrderListContentsClass" style="overflow:hidden;">
                            <div style="padding-bottom:2px;">
                                <asp:Repeater ID="OrderListRepeater" runat="server" EnableViewState="false">
                                    <ItemTemplate>
                                        <div runat="server" id="OrderListItem" class="OrderListItemClass">
                                            <div class="OrderListItemContentsClass">
                                                <table>
                                                    <tr valign="middle" style="height:50px;">
                                                        <td>
                                                            <icrop:CustomLabel runat="server" ID="OrderNumber" Width="150px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                        </td>
                                                        <td>
                                                            <icrop:CustomLabel runat="server" ID="OrderStartEndDate" Width="190px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                        </td>
                                                        <td>
                                                            <icrop:CustomLabel runat="server" ID="OrderStallName" Width="90px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                        </td>
                                                        <td>
                                                            <icrop:CustomLabel runat="server" ID="OrderServiceName" Width="90px" CssClass="Ellipsis"></icrop:CustomLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="PopUpOrderListFooterClass" id="PopUpOrderListFooter" runat="server">
                            <asp:Button ID="PopUpOrderListFooterButton" CssClass="PopUpOrderListFooterButtonOff" runat="server" OnClientClick="return CloseOrderList();" />
                        </div>
                    </div>
                    <asp:Button ID="OrderAreaEventButton" runat="server" style="display:none;" />
                    <asp:HiddenField runat="server" ID="HiddenSelectOrderNumber" />
                    <asp:HiddenField runat="server" ID="HiddenVisitId" />
                    <asp:HiddenField runat="server" ID="HiddenDmsJobDtlId" />
                    <asp:HiddenField runat="server" ID="HiddenVin" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <%-- 隠しボタンエリア --%>
            <asp:UpdatePanel ID="ContentUpdateButtonPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="HiddenOrderListDisplayType" />
                    <asp:HiddenField runat="server" ID="HiddenNewCustomerConfirmType" />
                    <asp:HiddenField runat="server" ID="HiddenNewCustomerConfirmWord" />
                    <asp:Button ID="VehicleAreaEventButton" runat="server" style="display:none;" />
                    <asp:Button ID="CustomerAreaEventButton" runat="server" style="display:none;" />
                    <asp:Button ID="ReserveAreaEventButton" runat="server" style="display:none;" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
    
</asp:Content>
