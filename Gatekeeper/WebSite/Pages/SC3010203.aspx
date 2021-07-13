<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SC3010201.master" AutoEventWireup="false" CodeFile="SC3010203.aspx.vb" Inherits="Pages_SC3010203" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SC3010201head" Runat="Server">
    <%'HEAD %>
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3010203/SC3010203.css?20111221000000" media="all" />
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Data.js?20111221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Layout.js?20111221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Drag.js?20111221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Ajax.js?20111221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.Main.js?20111222000000"></script>
    <script type="text/javascript" src="../Scripts/SC3010203/SC3010203.TestData.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SC3010201leftBottomBox" Runat="Server">
    <%'ダッシュボード %>
    <div id="dashboardBox" class="loading">
        <iframe id="dashboardFrame" height="100%" width="100%" src="SC3010202.aspx" seamless="seamless"></iframe>
        <%'読み込み中 %>
        <div id="loadingDashboard"></div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SC3010201rightBox" Runat="Server">
<%'右カラム %>
<div id="contentsRightBox" class="contentsFrame">
    <h2 class="contentTitle"><icrop:CustomLabel runat="server" CssClass="cutTextWord" Width="200px" TextWordNo="2"/></h2>
    <p class="katudouDate">
        <icrop:CustomLabel ID="NowDateLiteral" runat="server" EnableViewState="false" CssClass="cutTextWord" UseEllipsis="true" Width="200px" />
    </p>
    <div class="InnerBox01 LodingInnerBox01">
        
        <%'スケジュールエリアを囲むタグ %>
        <div class="inLeftBox1">

            <%'タイトル %>
            <h3 class="subTitle01"><icrop:CustomLabel runat="server" Width="200px" CssClass="cutTextWord" TextWordNo="7" /></h3>
            
            <%'終日イベント %>
            <div id="DateScheduleBox">
                
                <div id="DateScheduleInner">
                    <div class="dateScheduleMarginArea"></div>
                    <ul class="normalMode"></ul>
                    <span id="dayEventNotFound"><icrop:CustomLabel ID="notFoundDayEventLabel" runat="server" Width="300px" UseEllipsis="true" TextWordNo="9"/></span>
                </div>
                <p class="moreDayEvent">
                    <%'拡大 %>
                    <asp:LinkButton ID="DayEventBigSizeLink" CssClass="cutTextWord" runat="server" OnClientClick="return false">
                        <icrop:CustomLabel ID="DayEventNText1" runat="server" TextWordNo="4"/><span id="DayEventOtherCount">0</span><icrop:CustomLabel ID="DayEventNText2" runat="server" TextWordNo="5"/>
                    </asp:LinkButton>
                    <%'縮小 %>
                    <asp:LinkButton ID="DayEventNormalSizeLink" CssClass="cutTextWord" style="display:none" runat="server" OnClientClick="return false">&nbsp;</asp:LinkButton>
                </p>
            </div>

            <%'時間スケジュールを囲う枠 %>
            <div id="timeScheduleBoxOut">
                
                <div id="timeScheduleBoxIn" >
                    
                    <%'時間ボックス %>
                    <div id="timeScheduleLeftBox">
                        <asp:Repeater ID="TimeRepeater" runat="server">
                            <ItemTemplate>
                                <p class="cutTextWord"><asp:Literal ID="timeLiteral" runat="server" Mode="Encode" Text='<%#Container.DataItem%>'/></p>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%'ドラッグ中のナビゲーション用 %>
                        <p class="naviminute">:XX</p>
                    </div>
                    
                    <%'チップ格納ボックス %>
                    <div id="timeScheduleRightBox">

                        <%'30分毎のメモリ線 %>
                        <asp:Repeater ID="TimeLineBorderRepeater" runat="server">
                            <ItemTemplate><div class="timeLineBorder"></div></ItemTemplate>
                        </asp:Repeater>
                        <%'30分毎のメモリ線  %>

                        <div class="marginArea"></div>

                        <%'チップ格納ボックス (JSで格納） %>
                        <div id="timeScheduleChipBox">
                        </div>
                    </div>
                </div>

                <%'現在時刻 %>
                <div class="borderLine"></div>  

                <%'サーバー時間 %>
                <asp:HiddenField ID="Yearhidden" runat="server" />
                <asp:HiddenField ID="Monthhidden" runat="server" />
                <asp:HiddenField ID="Dayhidden" runat="server" />
                <asp:HiddenField ID="HourHidden" runat="server" />
                <asp:HiddenField ID="MinuteHidden" runat="server" />
                <%'エラーメッセージ %>
                <asp:HiddenField ID="CaldavRegistErrorMessage" runat="server" />
                <asp:HiddenField ID="CaldavSelectErrorMessage" runat="server" />
            </div>

            <%'スクロール %>
            <div id="timeScheduleHidden"></div>

        </div>

        <%'TODOエリアを囲むタグ %>
        <div class="inRightBox1">
            <h3 class="subTitle02"><icrop:CustomLabel ID="todoLabelName" runat="server" TextWordNo="3"/></h3>
            <div class="clearboth">&nbsp;</div>

            <%'TODOリスト %>
            <div id="toDoBoxOut">
                <div id="toDoBoxIn">
                    <p class="toDoBoxNote cutTextWord">
                        <icrop:CustomLabel ID="ToDoBoxNoteInnerLabel" runat="server" class="toDoBoxNoteInner" Width="100%">
                            <icrop:CustomLabel ID="noteLabelLeft" runat="server" TextWordNo="6"/><span id="todoDelayCount">0</span><icrop:CustomLabel ID="noteLabelRight" runat="server" TextWordNo="5"/>
                        </icrop:CustomLabel>
                    </p>
                    <%'TODOスクロール用 %>
                    <div id="todoChipBox">
                        <%'TODOチップ格納ボックス (JSで格納） %>
                        <div id="todoChipBoxInner"></div>
                    </div>
                </div>
            </div>

            <%'選択されたチップ情報格納用 %>
            <asp:HiddenField ID="selectDLRCD" runat="server" />
            <asp:HiddenField ID="selectSTRCD" runat="server" />
            <asp:HiddenField ID="selectFOLLOWUPBOXSEQNO" runat="server" />
            <%'チップ選択で顧客詳細に遷移するためのパラメータ %>
            <asp:Button ID="CustDetailDummyButton" runat="server" style="display:none" />

        </div>

    </div>
    <%'読み込み中 %>
    <div id="loadingSchedule"></div>
</div>
</asp:Content>

