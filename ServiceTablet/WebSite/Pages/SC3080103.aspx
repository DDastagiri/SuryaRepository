<%@ Page Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false"
    CodeFile="SC3080103.aspx.vb" Inherits="Pages_SC3080103" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3080103/SC3080103.css?20180612000000" />
    <script type="text/javascript" src="../Scripts/SC3080103/SC3080103.Fingerscroll.js?20131219000000"></script>
    <script type="text/javascript" src="../Scripts/SC3080103/SC3080103.Main.js?20170227000000"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:ScriptManager ID="MyScriptManager" runat="server">
    </asp:ScriptManager>
    <br />
    <div id="ServerProcessOverlayBlack">
    </div>
    <div id="ServerProcessIcon">
    </div>
    <div id="ServerProcessListOverlay">
    </div>
    <div id="ServerProcessListIcon">
    </div>
    <div id="OrderListOverlayBlack">
    </div>
    <!-- ここからメインブロック -->
    <div id="mainblock">
        <div class="mainblockWrap">
            <div id="mainblockContent">
                <div class="mainblockContentArea">
                    <asp:UpdatePanel ID="ContentUpdateMainPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="MainAreaReload" runat="server" Style="display: none;" />
                            <asp:Button ID="RegisterSortButton" runat="server" Style="display: none;" />
                            <asp:Button ID="CustomerSortButton" runat="server" Style="display: none;" />
                            <asp:Button ID="SASortButton" runat="server" Style="display: none;" />
                            <asp:Button ID="SCSortButton" runat="server" Style="display: none;" />
                            <asp:Button ID="BackPageButton" runat="server" Style="display: none;" />
                            <asp:Button ID="NextPageButton" runat="server" Style="display: none;" />
                            <div class="mainblockContentAreaWrap">
                                <div id="mainblockContentAreaNCM0201">
                                    <h2 class="mainblockContentAreaNCM0201Result">
                                        <asp:Label runat="server" ID="SearchCount"></asp:Label></h2>
                                    <div class="mainblockContentAreaNCM0201ResultList">
                                        <ul>
                                            <li class="RegisterSort">
                                                <icrop:CustomLabel runat="server" ID="RegisterHeader" Width="267px" CssClass="Ellipsis UnderLine" />
                                            </li>
                                            <li class="CustomerSort">
                                                <icrop:CustomLabel runat="server" ID="CustomerHeader" Width="245px" CssClass="Ellipsis UnderLine" />
                                            </li>
                                            <li>
                                                <icrop:CustomLabel runat="server" ID="TelMobileHeader" Width="130px" CssClass="Ellipsis" />
                                            </li>
                                            <li class="SASort">
                                                <icrop:CustomLabel runat="server" ID="SAHeader" Width="131px" CssClass="Ellipsis UnderLine" />
                                                <li class="SCSort">
                                                    <icrop:CustomLabel runat="server" ID="SCHeader" Width="132px" CssClass="Ellipsis UnderLine" />
                                        </ul>
                                    </div>
                                    <div id="NoSearchImage" runat="server">
                                        <icrop:CustomLabel runat="server" ID="NoSearchWord" CssClass="Ellipsis" />
                                    </div>
                                    <div runat="server" id="CustomerSearchArea" class="mainblockContentAreaNCM0201ResultScroll">
                                        <div class="mainblockContentAreaNCM0201ResultList2">
                                            <div runat="server" id="BackPage" class="NCM0201ResultListNextMore" style="display: none;">
                                                <icrop:CustomLabel runat="server" ID="BackPageWord" />
                                            </div>
                                            <div runat="server" id="BackPageLoad" class="NCM0201ResultListNextMore" style="display: none;">
                                                <icrop:CustomLabel runat="server" ID="BackPageLoadWord" />
                                                <span class="LoadImage"></span>
                                            </div>
                                            <asp:Repeater runat="server" ID="CustomerReserveAreaRepeater" EnableViewState="false">
                                                <ItemTemplate>
                                                    <ul runat="server" id="chipReserveRow">
                                                        <%-- 車両情報エリア --%>
                                                        <li runat="server" id="vehicleRecord" class="VehicleRecordClass">
                                                            <div class="RecordAreaDiv">
                                                                <div class="NCM0201ResultList2-1">
                                                                    <div class="NCM0201ResultList2-1-1">
                                                                        <p class="NCM0201ResultList2-1-1-1">
                                                                            <icrop:CustomLabel runat="server" ID="RegisterNo" Width="140px" CssClass="Ellipsis" /></p>
                                                                        <div class="NCM0201ResultList_AreaCar">
                                                                            <div class="NCM0201ResultList_AreaName1">
                                                                                <p class="NCM0201ResultList2-1-1-2">
                                                                                    <icrop:CustomLabel runat="server" ID="Province" Width="104px" CssClass="Ellipsis" /></p>
                                                                            </div>
                                                                            <div class="NCM0201ResultList_CarName1">
                                                                                <p class="NCM0201ResultList2-1-1-2">
                                                                                    <icrop:CustomLabel runat="server" ID="VehicleName" Width="104px" CssClass="Ellipsis" /></p>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="NCM0201ResultList2-1-2">
                                                                        <p class="NCM0201ResultList2-1-1-3">
                                                                            <icrop:CustomLabel runat="server" ID="Vin" Width="245px" CssClass="Ellipsis" /></p>
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                        <div class = "IconArea">
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                        <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START --%>
                                                                        <span runat="server" id="SSCIcon" class="IcoSSC" style="display: none;">
                                                                            <icrop:CustomLabel runat="server" ID="SSCWord" CssClass="Ellipsis" /></span>
                                                                        <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END --%>
                                                                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
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
                                                                        <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <%-- 顧客情報エリア --%>
                                                        <li runat="server" id="customerRecord" class="CustomerRecordClass">
                                                            <div class="RecordAreaDiv">
                                                                <div class="NCM0201ResultList2-2">
                                                                    <div class="NCM0201ResultList2-2-1">
                                                                        <p class="NCM0201ResultList2-2-1-1">
                                                                        <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START --%>
                                                                            <%-- <asp:Image runat="server" ID="CustomerImageIcon" Width="57px" Height="56px" /></p> --%>
                                                                            <asp:Image runat="server" ID="CustomerImageIcon" Width="60px" Height="60px" /></p>
                                                                        <%-- 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END --%>
                                                                    </div>
                                                                    <div class="NCM0201ResultList2-2-2">
                                                                        <p class="NCM0201ResultList2-2-1-2">
                                                                            <icrop:CustomLabel runat="server" ID="CustomerName" Width="158px" CssClass="Ellipsis" /></p>
                                                                        <p class="NCM0201ResultList2-2-1-3">
                                                                            <span runat="server" id="VipIcon" class="IcoVip2" style="display: none;">
                                                                                <icrop:CustomLabel runat="server" ID="VipWord" CssClass="Ellipsis" /></span>
                                                                            <span runat="server" id="MyCompanyIcon" class="IcoJi2" style="display: none;">
                                                                                <icrop:CustomLabel runat="server" ID="MyCompanyWord" CssClass="Ellipsis" /></span>
                                                                            <span runat="server" id="MyVehicleIcon" class="IcoKo2" style="display: none;">
                                                                                <icrop:CustomLabel runat="server" ID="MyVehicleWord" CssClass="Ellipsis" /></span>
                                                                            <span runat="server" id="MyAppointmentIcon" class="IcoA2" style="display: none;">
                                                                                <icrop:CustomLabel runat="server" ID="MyAppointmentWord" CssClass="Ellipsis" /></span>
                                                                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                                                            <span runat="server" id="LIcon" class="IcoL" style="display: none;">
                                                                                <icrop:CustomLabel runat="server" ID="LWord" CssClass="Ellipsis" /></span>
                                                                            <%-- 2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
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
                                                                        <p class="NCM0201ResultList2-3-1-3">
                                                                            <icrop:CustomLabel runat="server" ID="MobileNo" Width="110px" CssClass="Ellipsis" /></p>
                                                                    </div>
                                                                    <div class="NCM0201ResultList2-3-2">
                                                                        <p class="NCM0201ResultList2-3-1-1">
                                                                            <icrop:CustomLabel runat="server" ID="TelNo" Width="110px" CssClass="Ellipsis" /></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <%-- SAエリア --%>
                                                        <li runat="server" id="SARecord" class="CustomerSAClass">
                                                            <div class="RecordAreaDiv">
                                                                <div class="NCM0201ResultList2-4">
                                                                    <div class="NCM0201ResultList2-4-1">
                                                                        <p class="NCM0201ResultList2-4-1-1">
                                                                            <icrop:CustomLabel runat="server" ID="SA_NAME" Width="110px" CssClass="Ellipsis" /></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <%-- SCエリア --%>
                                                        <li runat="server" id="SCRecord" class="CustomerSCClass">
                                                            <div class="RecordAreaDiv">
                                                                <div class="NCM0201ResultList2-5">
                                                                    <div class="NCM0201ResultList2-5-1">
                                                                        <p class="NCM0201ResultList2-5-1-1">
                                                                            <icrop:CustomLabel runat="server" ID="SC_NAME" Width="110px" CssClass="Ellipsis" /></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <div runat="server" id="NextPage" class="NCM0201ResultListNextMore" style="display: none;">
                                                <icrop:CustomLabel runat="server" ID="NextPageWord" />
                                            </div>
                                            <div runat="server" id="NextPageLoad" class="NCM0201ResultListNextMore" style="display: none;">
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
                            <asp:HiddenField runat="server" ID="HiddenSASortType" />
                            <asp:HiddenField runat="server" ID="HiddenSCSortType" />
                            <asp:HiddenField runat="server" ID="HiddenSearchType" />
                            <asp:HiddenField runat="server" ID="HiddenSearchValue" />
                            <asp:HiddenField runat="server" ID="HiddenStartIndex" />
                            <asp:HiddenField runat="server" ID="HiddenEndIndex" />
                            <asp:HiddenField runat="server" ID="HiddenLoadCount" />
                            <asp:HiddenField runat="server" ID="HiddenMaxDisplayCount" />
                            <asp:HiddenField runat="server" ID="HiddenSelectStallUseId" />
                            <asp:HiddenField runat="server" ID="HiddenSelectAddType" />
                            <!--顧客コード-->
                            <asp:HiddenField runat="server" ID="HiddenSelectCustomerId" />
                            <!--VCLID-->
                            <asp:HiddenField runat="server" ID="HiddenSelectVehicleId" />
                            <!--VIN-->
                            <asp:HiddenField runat="server" ID="HiddenSelectVIN" />
                            <!--販売店コード-->
                            <asp:HiddenField runat="server" ID="HiddenSelectDlrCd" />
                            <!--店舗コード-->
                            <asp:HiddenField runat="server" ID="HiddenSelectStrCd" />
                            <!--基幹顧客ID-->
                            <asp:HiddenField runat="server" ID="HiddenSelectDMSCSTID" />
                            <!--車両登録No.-->
                            <asp:HiddenField runat="server" ID="HiddenSelectVehRegNo" />
                            <!--モデルコード-->
                            <asp:HiddenField runat="server" ID="HiddenSelectModelCode" />
                            <!--顧客名-->
                            <asp:HiddenField runat="server" ID="HiddenSelectCustomerName" />
                            <!--電話番号-->
                            <asp:HiddenField runat="server" ID="HiddenSelectTelNumber" />
                            <!--携帯番号-->
                            <asp:HiddenField runat="server" ID="HiddenSelectMobileNumber" />
                            <!--振当SA-->
                            <asp:HiddenField runat="server" ID="HiddenSelectSACode" />
                            <!--車名-->
                            <asp:HiddenField runat="server" ID="HiddenSelectModelName" />
                            <!--E-MAILアドレス1-->
                            <asp:HiddenField runat="server" ID="HiddenSelectEMail" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- RO一覧ポップアップエリア --%>
                <asp:UpdatePanel ID="ContentUpdatePopuupPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="PopUpOrderListClass" style="display: none;">
                            <div class="PopUpOrderListHeaderClass">
                                <icrop:CustomLabel runat="server" ID="PopUpOrderListHeaderLabel" CssClass="Ellipsis"
                                    Width="410px" Height="30px"></icrop:CustomLabel>
                            </div>
                            <div class="PopUpOrderListContentsClass" style="overflow: hidden;">
                                <div style="padding-bottom: 2px;">
                                    <asp:Repeater ID="OrderListRepeater" runat="server" EnableViewState="false">
                                        <ItemTemplate>
                                            <div runat="server" id="OrderListItem" class="OrderListItemClass">
                                                <div class="OrderListItemContentsClass">
                                                    <table>
                                                        <tr valign="middle" style="height: 50px;">
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="OrderNumber" Width="150px" CssClass="Ellipsis"
                                                                    Style="display: none; "></icrop:CustomLabel>
                                                            </td>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="RoJobSeq" Width="150px" CssClass="Ellipsis"
                                                                    Style="display: none; "></icrop:CustomLabel>
                                                            </td>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="OrderStartEndDate" Width="190px" CssClass="Ellipsis"
                                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"></icrop:CustomLabel>
                                                            </td>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="OrderStallName" Width="90px" CssClass="Ellipsis"
                                                                    Style="display: none; "></icrop:CustomLabel>
                                                            </td>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="OrderServiceName" Width="70px" CssClass="Ellipsis"
                                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"></icrop:CustomLabel>
                                                            </td>
                                                            <%-- 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START --%>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="NewCustomerName" Width="0px" CssClass="Ellipsis NewCstName"
                                                                    Style="display:none; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"></icrop:CustomLabel>
                                                            </td>
                                                            <%-- 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END --%>
                                                            <td>
                                                                <icrop:CustomLabel runat="server" ID="ROIssuingDisp" Width="110px" CssClass="Ellipsis"
                                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis; "></icrop:CustomLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div id="ROCreateButtonDiv" runat="server" class="ROCreateButtonClass">
                                        <icrop:CustomLabel runat="server" ID="ROCreate" Width="390px" CssClass="Ellipsis" />
                                    </div>
                                </div>
                            </div>
                            <div class="PopUpOrderListFooterClass" id="PopUpOrderListFooter" runat="server">
                                <asp:Button ID="PopUpOrderListFooterButton" CssClass="PopUpOrderListFooterButtonOff"
                                    runat="server" OnClientClick="return CloseOrderList();" />
                            </div>
                        </div>
                        <asp:Button ID="OrderAreaEventButton" runat="server" Style="display: none;" />
                        <asp:Button ID="ROCreateButton" runat="server" Style="display: none;" />
                        <asp:HiddenField runat="server" ID="HiddenSelectOrderNumber" />
                        <asp:HiddenField runat="server" ID="HiddenSelectRoJobSeq" />
                        <asp:HiddenField runat="server" ID="HiddenSelectSvcIn" />
                        <asp:HiddenField runat="server" ID="HiddenSelectDmsJobDtlId" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- 隠しボタンエリア --%>
                <asp:UpdatePanel ID="ContentUpdateButtonPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="HiddenOrderListDisplayType" />
                        <asp:HiddenField runat="server" ID="HiddenNewCustomerConfirmType" />
                        <asp:HiddenField runat="server" ID="HiddenNewCustomerConfirmWord" />
                        <asp:Button ID="VehicleAreaEventButton" runat="server" Style="display: none;" />
                        <asp:Button ID="CustomerAreaEventButton" runat="server" Style="display: none;" />
                        <%-- 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START --%>
                        <asp:HiddenField runat="server" ID="HiddenOrderListCstNameType" />
                        <%-- 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END --%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

