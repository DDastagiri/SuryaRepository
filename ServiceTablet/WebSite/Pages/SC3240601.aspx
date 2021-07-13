<%@ Page Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3240601.aspx.vb" Inherits="Pages_SC3240601" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../Styles/SC3240601/common.css?20140623000015" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3240601/header.css?20140623000015" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3240601/tsmb.css?20140623000015" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3240601/popupWindow.css?20140623000015" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3240601/tsmb0004.css?20141015000000" type="text/css" media="screen,print" />
    <script src="../Scripts/SC3240601/amcharts/amcharts.js?20131219000002" type="text/javascript"></script>
    <script src="../Scripts/SC3240601/amcharts/serial.js?20131219000002" type="text/javascript"></script>
    <script src="../Scripts/SC3240601/amcharts/themes/dark.js?20131219000002" type="text/javascript"></script>
    <script src="../Scripts/SC3240601/SC3240601.Fingerscroll.js?20131219000000" type="text/javascript"></script>
    <script src="../Scripts/SC3240601/SC3240601.Amcharts.js?20131219000004" type="text/javascript"></script>
    <script src="../Scripts/SC3240601/SC3240601.Main.js?20141015000000"　type="text/javascript" ></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:ScriptManager ID="MyScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="ServerProcessListOverlay">
    </div>
    <div id="ServerProcessGraphBtnsOverlay">
    </div>
    <div id="ServerProcessListIcon">
    </div>
    <div id="ServerProcessGraphOverlay">
    </div>
    <div id="ServerProcessGraphIcon">
    </div>
    <div id="OrderListOverlayBlack">
    </div>
    <div id="MainBlock">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="MainAreaReload" runat="server" Style="display: none;" />
                <div class="TSMBHeader">
                    <div class="TSMB_HDText01">
                        <icrop:CustomLabel runat="server" ID="lblTitleVehicleInformation" Width="209px" CssClass="Ellipsis"
                            Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></div>
                    <div class="TSMB_HDBox01">
                        <div class="BoxText">
                            <icrop:CustomLabel runat="server" ID="lblTitleOwner" Width="60px" CssClass="Ellipsis"
                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></div>
                        <div class="BoxArea">
                            <p>
                                <icrop:CustomLabel runat="server" ID="lblOwnerValue" Width="364px" CssClass="Ellipsis"
                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                        </div>
                    </div>
                    <div class="TSMB_HDBox02">
                        <div class="BoxText">
                            <icrop:CustomLabel runat="server" ID="lblTitleModel" Width="60px" CssClass="Ellipsis"
                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></div>
                        <div class="BoxArea">
                            <p>
                                <icrop:CustomLabel runat="server" ID="lblModelValue" Width="364px" CssClass="Ellipsis"
                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                        </div>
                    </div>
                    <div class="TSMB_HDBox03">
                        <div class="BoxText">
                            <icrop:CustomLabel runat="server" ID="lblTitleVin" Width="60px" CssClass="Ellipsis"
                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></div>
                        <div class="BoxArea">
                            <p>
                                <icrop:CustomLabel runat="server" ID="lblVinValue" Width="244px" CssClass="Ellipsis"
                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                        </div>
                    </div>
                    <div class="TSMB_HDBox04">
                        <div class="BoxText">
                            <icrop:CustomLabel runat="server" ID="lblTitleRegNo" Width="64px" CssClass="Ellipsis"
                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></div>
                        <div class="BoxArea">
                            <p>
                                <icrop:CustomLabel runat="server" ID="lblRegNoValue" Width="244px" CssClass="Ellipsis"
                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="ContentUpdateMileageBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="MileageBack">
                <div class="MileageTitle">
                    <icrop:CustomLabel runat="server" ID="lblMileageTrajectoryTitle" Width="400px" CssClass="Ellipsis" /></div>
                <div id="MileageGraph" class="MileageGraphClass">
                </div>
                <div class="Explanation">
                </div>
                <div class="BtnSet">
                    <div id="btnGraphPreYear" class="BtnBox01 BtnOn">
                        <p>
                        </p>
                    </div>
                    <div class="BtnBox02 BtnOFF">
                        <p class="Btn01">
                            <icrop:CustomLabel runat="server" ID="lblGraphDayTitle" Width="59px" CssClass="Ellipsis" /></p>
                        <p class="Btn02">
                            <icrop:CustomLabel runat="server" ID="lblGraphWeekTitle" Width="59px" CssClass="Ellipsis" /></p>
                        <p class="Btn03 BtnOn">
                            <icrop:CustomLabel runat="server" ID="lblGraphMonthTitle" Width="59px" CssClass="Ellipsis" /></p>
                    </div>
                    <div id="btnGraphNextYear" class="BtnBox03 BtnOn">
                        <p>
                        </p>
                    </div>
                </div>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div runat="server" id="WarningInfoArea" class="DisplayList">
            <div class="Knob" id="Knob">
            </div>
            <div class="ListBox">
                <div class="TitleSet">
                    <ul>
                        <li class="Title01">
                            <icrop:CustomLabel runat="server" ID="lblNoHeader" Width="38px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title02">
                            <icrop:CustomLabel runat="server" ID="lblDateHeader" Width="105px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title03">
                            <icrop:CustomLabel runat="server" ID="lblMileageHeader" Width="105px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title04">
                            <icrop:CustomLabel runat="server" ID="lblInformationSourceHeader" Width="203px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title05">
                            <icrop:CustomLabel runat="server" ID="lblCustomerHeader" Width="203px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title06">
                            <icrop:CustomLabel runat="server" ID="lblInformationHeader" Width="240px" CssClass="Ellipsis" />
                        </li>
                        <li class="Title07">
                            <icrop:CustomLabel runat="server" ID="lblDetailHeader" Width="74px" CssClass="Ellipsis" />
                        </li>
                    </ul>
                </div>
                <div class="ListScrollBox">
                    <asp:UpdatePanel ID="ContentUpdateListScrollBox" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="BackPage" class="BackPageClass" style="display: none; text-align: center;
                                line-height: 46px; font-size: 14px;">
                                <icrop:CustomLabel runat="server" ID="BackPageWord" />
                            </div>
                            <div runat="server" id="BackPageLoad" class="BackPageLoadClass" style="display: none; text-align: center; line-height: 46px;
                                font-size: 14px;">
                                <icrop:CustomLabel runat="server" ID="BackPageLoadWord" />
                                <span class="PageLoadBackIcon">
                                </span>
                            </div>
                            <asp:Repeater runat="server" ID="WarningInfoRepeater" EnableViewState="false">
                                <ItemTemplate>
                                    <ul runat="server" id="WarningInfoRow">
                                        <%-- Noエリア --%>
                                        <li runat="server" id="NumberRecord" class="NumberRecordClass">
                                            <div class="RecordAreaDiv">
                                                <icrop:CustomLabel runat="server" ID="lblNumberRecord" Width="38px" CssClass="Ellipsis" />
                                            </div>
                                        </li>
                                        <%-- Dateエリア --%>
                                        <li runat="server" id="DateRecord" class="DateRecordClass">
                                            <div class="RecordAreaDiv">
                                                <icrop:CustomLabel runat="server" ID="lblDateRecord" Width="105px" CssClass="Ellipsis" />
                                            </div>
                                        </li>
                                        <%-- Mileageエリア --%>
                                        <li runat="server" id="MileageRecord" class="MileageRecordClass">
                                            <div class="RecordAreaDiv">
                                                <icrop:CustomLabel runat="server" ID="lblMileageRecord" Width="100px" CssClass="Ellipsis"
                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" />
                                            </div>
                                        </li>
                                        <%-- Information Sourceエリア --%>
                                        <li runat="server" id="ISRecord" class="ISRecordClass">
                                            <div class="RecordAreaDiv">
                                                <icrop:CustomLabel runat="server" ID="lblISRecord" Width="198px" CssClass="Ellipsis"
                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" />
                                            </div>
                                        </li>
                                        <%-- Customer エリア --%>
                                        <li runat="server" id="CustomerRecord" class="CustomerRecordClass">
                                            <div class="RecordAreaDiv">
                                                <icrop:CustomLabel runat="server" ID="lblCustomerRecord" Width="198px" CssClass="Ellipsis"
                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" />
                                            </div>
                                        </li>
                                        <%-- Information エリア --%>
                                        <li runat="server" id="InformationRecord" class="InformationRecordClass">
                                            <div class="RecordAreaDiv">
                                                <asp:Repeater runat="server" ID="InformationRepeater" EnableViewState="false">
                                                    <ItemTemplate>
                                                        <div style="width: 230px; height: 45px">
                                                            <icrop:CustomLabel runat="server" ID="lblInformationRecord" Width="230px" CssClass="Ellipsis"
                                                                Style="padding: 0px 0px 0px 0px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </li>
                                        <%-- Detail Button エリア --%>
                                        <li runat="server" id="DetailButtonRecord" class="DetailButtonRecordClass">
                                            <div class="RecordAreaDiv">
                                                <asp:Repeater runat="server" ID="DetailButtonRepeater" EnableViewState="false">
                                                    <ItemTemplate>
                                                        <div id="DetailButtonDiv" style="width: 50px; height: 45px">
                                                            <span runat="server" id="DetailButtonArea" class="DetailButtonAreaClass BtnBoxOff">
                                                                <icrop:CustomLabel runat="server" ID="lblDetailButtonRecord" Width="50px" CssClass="Ellipsis" /></span>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div id="NextPage" class="NextPageClass" style="display: none; line-height: 46px;
                                font-size: 14px;">
                                <icrop:CustomLabel runat="server" ID="NextPageWord" />
                            </div>
                            <div runat="server" id="NextPageLoad" class="NextPageLoadClass" style="display: none; text-align: center; line-height: 46px;
                                font-size: 14px;">
                                <icrop:CustomLabel runat="server" ID="NextPageLoadWord" />
                                <span class="PageLoadNextIcon">
                                </span>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        
        <div class="dummyrightbar"></div>
        <div class="dummyleftbar"></div>
        <div class="dummylistbar"></div>
        <div class="dummybottombar"></div>
        <div class="dummyscroll"></div>

        <asp:UpdatePanel ID="ContentUpdatePopuupPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="PopUpOrderListClass" style="display:none" >
                    <div class="popBase popWindowSizeW387 popWindowSizeH531">
                        <div class="popWindowBase popWindowCoordinate01">
                            <div class="Balloon">
                                <div class="borderBox">
                                    <div class="Arrow">
                                        &nbsp;</div>
                                    <div class="myDataBox">
                                        &nbsp;</div>
                                </div>
                                <div class="gradationBox">
                                    <div class="scNscPopUpHeaderBg">
                                        &nbsp;</div>
                                    <div class="scNscPopUpDataBg">
                                        &nbsp;</div>
                                </div>
                            </div>
                            <div class="PopUpHeader">
                                <h3>
                                    <icrop:CustomLabel runat="server" ID="lblPopUpTitle" Width="386px" CssClass="Ellipsis" 
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"/></h3>
                                <div>
                                    <asp:Button ID="PopUpCloseButton" Text="Close" CssClass="LeftBtn" runat="server" OnClientClick="return ClosePopUp();" 
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"/>
                                        </div>
                            </div>
                            <div class="dataBox">
                                <div class="innerDataBox">
                                    <!-- Window内部 -->
                                    <div class="InnerDatas">
                                        <ul class="ListBox01">
                                            <li>
                                                <dl>
                                                    <dt>
                                                        <p><icrop:CustomLabel runat="server" ID="lblDate_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dt>
                                                    <dd>
                                                        <p class="Date_Value_Detail" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" ><icrop:CustomLabel runat="server" ID="lblDate_Value_Detail" Width="260px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dd>
                                                </dl>
                                            </li>
                                            <li>
                                                <dl>
                                                    <dt>
                                                        <p><icrop:CustomLabel runat="server" ID="lblCode_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dt>
                                                    <dd>
                                                        <p class="Code_Value_Detail" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" ><icrop:CustomLabel runat="server" ID="lblCode_Value_Detail" Width="260px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dd>
                                                </dl>
                                            </li>
                                            <li>
                                                <dl>
                                                    <dt>
                                                        <p><icrop:CustomLabel runat="server" ID="lblMileage_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dt>
                                                    <dd>
                                                        <p class="Mileage_Value_Detail" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" ><icrop:CustomLabel runat="server" ID="lblMileage_Value_Detail" Width="260px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dd>
                                                </dl>
                                            </li>
                                            <li>
                                                <dl>
                                                    <dt>
                                                        <p><icrop:CustomLabel runat="server" ID="lblName_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dt>
                                                    <dd>
                                                        <p class="Name_Value_Detail" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" ><icrop:CustomLabel runat="server" ID="lblName_Value_Detail" Width="260px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dd>
                                                </dl>
                                            </li>
                                        </ul>
                                        <ul class="ListBox02">
                                            <li>
                                                <dl>
                                                    <dt>
                                                        <p><icrop:CustomLabel runat="server" ID="lblIndicator_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                    </dt>
                                                    <dd>
                                                        <p>
                                                            <asp:Image runat="server" ID="IndicatorImage" Width="261px" Height="197px" /></p>
                                                    </dd>
                                                </dl>
                                            </li>
                                        </ul>
                                        <ul class="ListBox03">
                                            <li>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <p>
                                                                <icrop:CustomLabel runat="server" ID="lblDescription_Title_Detail" Width="65px" CssClass="Ellipsis"
                                                                    Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;" /></p>
                                                        </td>
                                                        <td>
                                                            <p>
                                                                <asp:TextBox runat="server" ID="lbl_Description_Value_Detail" TextMode="MultiLine" Wrap="true" class="Description_Value_Detail" readonly="true"></asp:TextBox>
                                                            </p>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /Window内部 -->
                                </div>
                            </div>
                            <div class="OverShadow">
                                &nbsp;</div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- 隠しボタンエリア --%>
        <asp:UpdatePanel ID="ContentUpdateButtonPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="HiddenDmsName" />
                <asp:HiddenField runat="server" ID="HiddenGraphDataField" />
                <asp:HiddenField runat="server" ID="HiddenWarningDispDays" />
                <asp:HiddenField runat="server" ID="HiddenImageDisplayFlg" />
                <asp:HiddenField runat="server" ID="HiddenImageUrl" />
                <asp:HiddenField runat="server" ID="HiddenOwnerID" />
                <asp:HiddenField runat="server" ID="HiddenKm" />
                <asp:HiddenField runat="server" ID="HiddenGraphLegend1" />
                <asp:HiddenField runat="server" ID="HiddenGraphLegend2" />
                <asp:HiddenField runat="server" ID="HiddenGraphLegend3" />
                <asp:HiddenField runat="server" ID="HiddenGraphLegend4" />
                <asp:HiddenField runat="server" ID="HiddenFlickDisplayListTop" />
                <asp:HiddenField runat="server" ID="HiddenFlickListScrollBoxHeight" />
                <asp:HiddenField runat="server" ID="HiddenStartIndex" />
                <asp:HiddenField runat="server" ID="HiddenEndIndex" />
                <asp:HiddenField runat="server" ID="HiddenLoadCount" />
                <asp:HiddenField runat="server" ID="HiddenMaxDisplayCount" />
                <asp:HiddenField runat="server" ID="HiddenSearchListCount" />
                <asp:HiddenField runat="server" ID="HiddenOrderListDisplayType" />
                <asp:HiddenField runat="server" ID="HiddenVclID" />
                <asp:HiddenField runat="server" ID="HiddenVin" />
                <asp:HiddenField runat="server" ID="HiddenOccurdate" />
                <asp:HiddenField runat="server" ID="HiddenGraphStartDate" />
                <asp:HiddenField runat="server" ID="HiddenGraphEndDate" />
                <asp:HiddenField runat="server" ID="HiddenGraphPreButtonEnable" />
                <asp:HiddenField runat="server" ID="HiddenGraphNextButtonEnable" />
                <asp:HiddenField runat="server" ID="HiddenMileScaleInit" />
                <asp:HiddenField runat="server" ID="HiddenMileScaleDayCount" />
                <asp:HiddenField runat="server" ID="HiddenMileScaleWeeklyCount" />
                <asp:HiddenField runat="server" ID="HiddenMileScaleMonthCount" />
                <asp:HiddenField runat="server" ID="HiddenMileScaleMonthCountDays" />
                <asp:HiddenField runat="server" ID="HiddenUserWarnFlg" />
                <asp:HiddenField runat="server" ID="HiddenMileTlmDispFlg" />
                <asp:HiddenField runat="server" ID="HiddenScrollPosition" />
                <asp:HiddenField runat="server" ID="HiddenWord008GBOOK" />
                <asp:HiddenField runat="server" ID="HiddenWord021Detail" />
                <asp:HiddenField runat="server" ID="HiddenWord023OtherDealer" />
                <asp:HiddenField runat="server" ID="HiddenWord024OwnerSite" />
                <asp:HiddenField runat="server" ID="HiddenWord025SMS" />
                <asp:HiddenField runat="server" ID="HiddenWord026iCROP" />
                <asp:HiddenField runat="server" ID="HiddenWord027GBOOK" />
                <asp:HiddenField runat="server" ID="HiddenWord028GBOOKWarning" />
                <asp:HiddenField runat="server" ID="HiddenWord029Format" />
                <asp:HiddenField runat="server" ID="HiddenWord041Hyphen" />
                <asp:HiddenField runat="server" ID="HiddenTeremaIntroduction" />
                <asp:HiddenField runat="server" ID="HiddenTelemaDisplayCount" />
                <asp:Button ID="BackPageButton" runat="server" Style="display: none;" />
                <asp:Button ID="NextPageButton" runat="server" Style="display: none;" />
                <asp:Button ID="DetailButtonAreaEventButton" runat="server" Style="display: none;" />
                <asp:Button ID="GraphDayButton" runat="server" Style="display: none;" />
                <asp:Button ID="GraphWeekButton" runat="server" Style="display: none;"  />
                <asp:Button ID="GraphMonthButton" runat="server" Style="display: none;"  />
                <asp:Button ID="GraphPreYearButton" runat="server" Style="display: none;" />
                <asp:Button ID="GraphNextYearButton" runat="server" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
