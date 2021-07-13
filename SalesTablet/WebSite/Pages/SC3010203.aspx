<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SC3010201.master" AutoEventWireup="false"
    CodeFile="SC3010203.aspx.vb" Inherits="Pages_SC3010203" %>
<%@ Import Namespace="Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic" %>
<%@ Register src="SC3290101.ascx" tagname="SC3290101" tagprefix="uc1" %>
<%@ Register src="SC3290102.ascx" tagname="SC3290102" tagprefix="uc2" %>
<%@ Register src="SC3100302.ascx" tagname="SC3100302" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SC3010201head" runat="Server">
    <%'HEAD %>
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3010203/SC3010203.css?20140930000000" media="all" />
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Data.js?20140618000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Layout.js?20140704000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Drag.js?20190528000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Ajax.js?20140618000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Main.js?20190528000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.TestData.js?20140618000000"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SC3010201leftBottomBox2" runat="Server">
    <%'ダッシュボード %>
    <h2 class="contentTitle wt01">
        <icrop:CustomLabel style="width:150px;" ID="CustomLabel8" runat="server" CssClass="cutTextWord useEllipsis" TextWordNo="11" />
    </h2>
    <div id="dashboardBox" class="loading">
        <iframe id="dashboardFrame" height="100%" width="100%" src="SC3010202.aspx" seamless="seamless" style="position: absolute; top: 32px;">
        </iframe>
        <%'読み込み中 %>
        <div id="loadingDashboard">
        </div>
    </div>
    <%'Process KPI %>
    <h2 class="contentTitle wt01">
        <icrop:CustomLabel style="width:150px;" ID="CustomLabel9" runat="server" CssClass="cutTextWord useEllipsis" TextWordNo="12" />
    </h2>
    <div id="processKpiBox" class="loading">
        <iframe id="processKpiFrame" height="100%" width="100%" src="SC3010204.aspx" seamless="seamless"></iframe>
        <%'読み込み中 %>
        <div id="loadingProcessKpi"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SC3010201leftBottomBox" runat="Server">
    <%'スケジュール %>
    <div id="DateScheduleTitle">
        <h2 class="contentTitle wt01">
            <icrop:CustomLabel ID="CustomLabel1" runat="server" CssClass="cutTextWord" TextWordNo="7" />
        </h2>
    </div>
    <%'終日イベント %>
    <div id="DateScheduleBox">
        <div id="DateScheduleInner">
            <div class="dateScheduleMarginArea">
            </div>
            <ul class="normalMode useEllipsis">
            </ul>
            <span id="dayEventNotFound">
                <icrop:CustomLabel ID="notFoundDayEventLabel" runat="server" Width="300px" UseEllipsis="true" TextWordNo="9" />
            </span>
        </div>
        <p class="moreDayEvent">
            <%'拡大 %>
            <asp:LinkButton ID="DayEventBigSizeLink" CssClass="cutTextWord" runat="server" OnClientClick="return false">
                <icrop:CustomLabel ID="DayEventNText1" runat="server" TextWordNo="4" />
                <span id="DayEventOtherCount">0</span>
                <icrop:CustomLabel ID="DayEventNText2" runat="server" TextWordNo="5" />
            </asp:LinkButton>
            <%'縮小 %>
            <asp:LinkButton ID="DayEventNormalSizeLink" CssClass="cutTextWord" Style="display: none" runat="server" OnClientClick="return false">&nbsp;</asp:LinkButton>
        </p>
    </div>
    <%'時間スケジュールを囲う枠 %>
    <div id="timeScheduleBoxOut">
        <div id="timeScheduleBoxIn">
            <%'時間ボックス %>
            <div id="timeScheduleLeftBox">
                <asp:Repeater ID="TimeRepeater" runat="server">
                    <ItemTemplate>
                        <p class="cutTextWord">
                            <asp:Literal ID="timeLiteral" runat="server" Mode="Encode" Text='<%#Container.DataItem%>' />
                        </p>
                    </ItemTemplate>
                </asp:Repeater>
                <%'ドラッグ中のナビゲーション用 %>
                <p class="naviminute">:XX</p>
            </div>
            <%'チップ格納ボックス %>
            <div id="timeScheduleRightBox">
                <%'30分毎のメモリ線 %>
                <asp:Repeater ID="TimeLineBorderRepeater" runat="server">
                    <ItemTemplate>
                        <div class="timeLineBorder">
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <%'30分毎のメモリ線  %>
                <div class="marginArea">
                </div>
                <%'チップ格納ボックス (JSで格納） %>
                <div id="timeScheduleChipBox">
                </div>
            </div>
        </div>
        <%'現在時刻 %>
        <div class="borderLine">
        </div>
        <%'サーバー時間 %>
        <asp:HiddenField ID="Yearhidden" runat="server" />
        <asp:HiddenField ID="Monthhidden" runat="server" />
        <asp:HiddenField ID="Dayhidden" runat="server" />
        <asp:HiddenField ID="HourHidden" runat="server" />
        <asp:HiddenField ID="MinuteHidden" runat="server" />
        <%'エラーメッセージ %>
        <asp:HiddenField ID="CaldavRegistErrorMessage" runat="server" />
        <asp:HiddenField ID="CaldavSelectErrorMessage" runat="server" />
        <%'スクロール %>
        <div id="timeScheduleHidden">
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SC3010201rightBox" runat="Server">

<%If String.Equals(StaffContext.Current.OpeCD, Operation.SSF) Then%>

    <div id="parentRightBox" class="parentFrame">
        <%'右カラム %>
        <div id="contentsRightBox" class="contentsFrame">
            <h2 class="contentTitle  wt01">
                <icrop:CustomLabel ID="CustomLabel2" runat="server" CssClass="cutTextWord" TextWordNo="2" /></h2>
            <div class="todowitches">
                <div class="Date">
                    <icrop:CustomLabel ID="NowDateLiteral" runat="server" EnableViewState="false" CssClass="cutTextWord" UseEllipsis="true" Width="90px" />
                </div>
                <div class="DateSWPrev" style="cursor: pointer;" onclick="todoPrev();">
                    <icrop:CustomLabel ID="Prev" runat="server" CssClass="cutTextWord" TextWordNo="17" />
                </div>
                <div class="DateSWToday" style="cursor: pointer;" onclick="todoToday();">
                    <icrop:CustomLabel ID="Today" runat="server" CssClass="cutTextWord" TextWordNo="18" />
                </div>
                <div class="DateSWNext" style="cursor: pointer;" onclick="todoNext();">
                    <icrop:CustomLabel ID="CustomLabel4" runat="server" CssClass="cutTextWord" TextWordNo="19" />
                </div>
                <div class="ViewSW">
                    <% If Me.isToDoBox.Value.Equals("1") Then%>
                    <span id="NoButtom" runat="server" style="display: none"></span>
                    <% Else%>
                    <icrop:SegmentedButton ID="ToDoDispSegmentedButton" name="ToDoDispSegmentedButton" runat="server" class="SwitchButton"  onClick="ToDoDispChange();" TabIndex="1"></icrop:SegmentedButton>
                    <% End If%>
                </div>
                <div class="Notice">
                    <span id="Span9" style="float: left; height: 31px; width: 30px; overflow: hidden; cursor: pointer;" onclick="todoTransfer();"></span>
                </div>
            </div>
            <%'TODOエリアを囲むタグ %>
            <div class="InnerBox01">
                <%'ToDo一覧(受注前工程タスク) %>
                <div class="inRightBox1 reftOrder1">
                    <h3 class="subTitle02ToDo todoType1">
                        <span class="titleName ">
                            <icrop:CustomLabel ID="CustomLabell2" runat="server" TextWordNo="13" />
                        </span>
                        <span class="titleCount" id="CntSalesTodoChip">
<%--                            <span id="unCompCntSalesTodoChip" class="unComp"></span>
                            <span class="slash" style="display: none" >/</span>
                            <span id="TotalCntSalesTodoChip" class="total"></span>--%>
                        </span>
                    </h3>
                    <div class="clearboth">&nbsp;</div>
                    <%'TODOリスト %>
                    <div id="toDoBoxOut">
                        <div id="toDoBoxIn" class="todoBoxIn loadingToDo1">
                            <%'TODOスクロール用 %>
                            <div id="todoChipBox" class="todoChipBox">
                                <%'TODOチップ格納ボックス (JSで格納） %>
                                <div id="SalestodoChipBoxInner">
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <%'ToDo一覧(受注後工程タスク) %>
                <div class="inRightBox1 reftOrder2">
                    <h3 class="subTitle02ToDo todoType2">
                        <span class="titleName ">
                            <icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="14" />
                        </span>
                        <span class="titleCount" id="CntBookedAfterTodoChip">
<%--                            <span id="unCompCntBookedAfterTodoChip" class="unComp"></span>
                            <span class="slash" style="display: none" >/</span>
                            <span id="TotalCntBookedAfterTodoChip" class="total"></span>--%>
                        </span>
                    </h3>
                    <div class="clearboth">&nbsp;</div>
                    <%'TODOリスト %>
                    <div id="BookedAftertoDoBoxOut">
                        <div id="BookedAftertoDoBoxIn" class="todoBoxIn loadingToDo2">
                            <%'TODOスクロール用 %>
                            <div id="todoChipBox" class="BookedAftertodoChipBox">
                                <%'TODOチップ格納ボックス (JSで格納） %>
                                <div id="BookedAftertodoChipBoxInner">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%'ToDo一覧(納車後工程タスク) %>
                <div class="inRightBox1 reftOrder3">
                    <h3 class="subTitle02ToDo todoType3">
                        <span class="titleName ">
                            <icrop:CustomLabel ID="CustomLabel10" runat="server" TextWordNo="15" />
                        </span>
                        <span class="titleCount" id="CntDeliAfterTodoChip">
<%--                            <span id="unCompCntDeliAfterTodoChip" class="unComp"></span>
                            <span class="slash" style="display: none" >/</span>
                            <span id="TotalCntDeliAfterTodoChip" class="total"></span>--%>
                        </span>
                    </h3>
                    <div class="clearboth">&nbsp;</div>
                    <%'TODOリスト %>
                    <div id="DeliAftertoDoBoxOut" class="SizeS">
                        <div id="DeliAftertoDoBoxIn" class="todoBoxIn loadingToDo3">
                            <%'TODOスクロール用 %>
                            <div id="todoChipBox" class="DeliAftertodoChipBox">
                                <%'TODOチップ格納ボックス (JSで格納） %>
                                <div id="DeliAftertodoChipBoxInner">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%'ToDo一覧(来店実績) %>
                <uc1:SC3100302 ID="SC3100302" runat="server"/>
<%--                <asp:UpdatePanel runat="server" ID="VisitSales" UpdateMode="Conditional">
                <ContentTemplate>
                <div class="inRightBox1 reftOrder4">
                    <h3 class="subTitle02ToDo todoType4">
                        <span class="titleName ">
                            <icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="16" />
                        </span>
                        <span class="titleCount">
                            <span><asp:Label ID="VisitSalesCount" runat="server"></asp:Label></span>
                        </span>
                    </h3>
                    <div class="clearboth loadingVisitActual">&nbsp;</div>
                    <div id="VisitBoxOut" class="SizeS">
                        <div id="VisitBoxIn" class="todoBoxIn" style="overflow: hidden;">
                            <asp:Repeater ID="ActualVisitRepeater" runat="server">
                                <ItemTemplate>
                                    <li id="VisitActualRow" runat="server">
                                    <div class="SCMainChip">
                                        <div class="InnerBox">
                                            <span class="inUserName useEllipsis">
                                               <asp:Literal ID="CustomerName" runat="server" Mode="Encode" Text='<%#Eval("CUST_NAME_WITH_TITLE")%>'></asp:Literal>
                                            </span>
                                            <br>
                                            <span class="addInfomation useEllipsis">
                                               <asp:Literal ID="Infomation" runat="server" Mode="Encode" Text='<%#Eval("CST_SERVICE_NAME")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint3">
                                               <asp:Literal ID="SalesStartDate" runat="server" Mode="Encode" Text='<%#Eval("SALES_DATE")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint4 useEllipsis">
                                               <asp:Literal ID="TempStaffName" runat="server" Mode="Encode" Text='<%#Eval("TEMP_STAFFNAME")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint5">
                                               <img id="TempStaffOperationIcon" src='<%#Server.HTMLEncode(Eval("TEMP_STAFF_OPERATIONCODE_ICON"))%>' width="22" height="23" runat="server" alt=""></img>
                                            </span>
                                        </div>
                                    </div>
                                    <input type="hidden" class="Dlrcd" value="<%#Eval("DLRCD")%>" />
                                    <input type="hidden" class="Strcd" value="<%#Eval("STRCD")%>" />
                                    <input type="hidden" class="FllwupBoxSeqno" value="<%#Eval("FLLWUPBOX_SEQNO")%>" />
                                    <input type="hidden" class="SalesStatus" value="<%#Eval("SALES_STATUS")%>" />
                                    <input type="hidden" class="CustomerSegment" value="<%#Eval("CUSTSEGMENT")%>" />
                                    <input type="hidden" class="CustomerClass" value="<%#Eval("CUSTOMERCLASS")%>" />
                                    <input type="hidden" class="CustomerId" value="<%#Eval("CRCUSTID")%>" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <asp:Button ID="VisitSalesTrigger" runat="server" Text="Button" CssClass="VisitSalesTrigger" />

                </ContentTemplate>  
                </asp:UpdatePanel>--%>
            </div>
        </div>
        <%'読み込み中 %>
        <div id="loadingSchedule">
        </div>
    </div>

    <%ElseIf String.Equals(StaffContext.Current.OpeCD, Operation.SSM) Or String.Equals(StaffContext.Current.OpeCD, Operation.BM) Then%>
        
        <div id="SC3290101Div">
            <uc1:SC3290101 ID="SC3290101" runat="server" Visible="true" />
        </div>

        <div id="SC3290102Div">
            <uc2:SC3290102 ID="SC3290102" runat="server" Visible="true" />
        </div>

    <%End If%>

    <%'選択されたチップ情報格納用 %>
    <asp:HiddenField ID="selectDLRCD" runat="server" />
    <asp:HiddenField ID="selectSTRCD" runat="server" />
    <asp:HiddenField ID="selectFOLLOWUPBOXSEQNO" runat="server" />
    <asp:HiddenField ID="selectCSTKIND" runat="server" />
    <asp:HiddenField ID="selectCUSTOMERCLASS" runat="server" />
    <asp:HiddenField ID="selectCRCUSTID" runat="server" />
    <asp:HiddenField ID="selectSALESSTATUS" runat="server" />
    <asp:HiddenField ID="isContactHistoryTransfer" runat="server" />
    <asp:Button ID="refreshButton" runat="server" Text="再描画する" Style="display: none" />
    <%'チップ選択で顧客詳細に遷移するためのパラメータ %>
    <asp:Button ID="CustDetailDummyButton" runat="server" Style="display: none" />
    <%'ToDo Pre に遷移するためのダミーボタン %>
    <asp:Button ID="toDoPrevButtom" runat="server" Style="display: none" />
    <%'ToDo Pre に遷移するためのダミーボタン %>
    <asp:Button ID="toDoTodayButtom" runat="server" Style="display: none" />
    <%'ToDo Pre に遷移するためのダミーボタン %>
    <asp:Button ID="toDoNextButtom" runat="server" Style="display: none" />
    <%'ToDo一覧画面に遷移するためのダミーボタン %>
    <asp:Button ID="toDoTitleButton" runat="server" Style="display: none" />

    <asp:HiddenField ID="contactHistoryNowLoading" runat="server" Value="0" />
    <asp:HiddenField ID="isSwipeLockHidden" runat="server" />
    <asp:HiddenField ID="isToDoChipDrop" runat="server" />
    <asp:HiddenField ID="isDisplayDate" runat="server" />
    <asp:HiddenField ID="isToDoBox" runat="server" />
    <asp:HiddenField ID="toDoButtom" runat="server" />
    <asp:HiddenField ID="visitSalesTipColor" runat="server" />
    <asp:HiddenField ID="slash" runat="server" />
    <%'2014/05/20 TCS 河原 マネージャー機能 Start%>
    <asp:Button ID="moveMainFrameDummyButton" runat="server" Style="display: none" />
    <asp:HiddenField ID="transitionsDiv" runat="server" />
    <asp:HiddenField ID="abnormalClassCD" runat="server" />
    <asp:HiddenField ID="abnormalItemCD" runat="server" />
    <asp:HiddenField ID="opeCD" runat="server" />
    <%'2014/05/20 TCS 河原 マネージャー機能 End%>

</asp:Content>
