<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPageSales.master" AutoEventWireup="false" CodeFile="SC3090401.aspx.vb" Inherits="Pages_SC3090401" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3090401/common.css?20180219000000" type="text/css"media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3090401/SC3090401.css?20190228000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3090401/SC3090401.PullDownRefresh.css?20180219000000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3090401/SC3090401.js?20190228000000"></script>
    <script type="text/javascript" src="../Scripts/SC3090401/SC3090401.fingerscroll.js?20180219000000"></script>
    <script type="text/javascript" src="../Scripts/SC3090401/SC3090401.Define.js?20190228000000"></script>
    <script type="text/javascript" src="../Scripts/SC3090401/SC3090401.PullDownRefresh.js?20190228000000"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090401.aspx
'─────────────────────────────────────
'機能： 予約一覧
'補足： 
'作成： 2018/02/19 NSK h.kawatani   REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
'更新： 2019/02/28 NSK h.kawatani   REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $01
'─────────────────────────────────────
-->
    <div id="MM_Main_Contents" runat="server">
        <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="StandardReadCountNumber" runat="server" />
        <asp:HiddenField ID="MaxDisplayCountNumber" runat="server" />
        <asp:HiddenField ID="AllDisplayFlag" runat="server" />
        <asp:HiddenField ID="SortType" runat="server" />
        <div class="Bottom_Box">
            <div class="VisitInfo_TBL">
                <div id="ButtonHeader">
                    <div id="BackDisplay">
                        <div id="btn_back_o">
                            <div class="imgBack"><img src="../Styles/Images/SC3090401/icon_back_on.png" width="40px" height="40px" alt="" /></div>
                        </div>
                        <div id="btn_back">
                            <div class="imgBack"><img src="../Styles/Images/SC3090401/icon_back.png" width="40px" height="40px" alt="" /></div>
                        </div>
                    </div>
                    <div id="AllAppointment">
                        <div id="btn_all_o">
                            <div class="imgAll"><img src="../Styles/Images/SC3090401/icon_all_on.png" width="40px" height="40px" alt="" /></div>
                        </div>
                        <div id="btn_all">
                            <div class="imgAll"><img src="../Styles/Images/SC3090401/icon_all.png" width="40px" height="40px" alt="" /></div>
                        </div>
                    </div>
                    <div id="SortChange">
                        <div id="btn_sort_no">
                            <div class="imgSort"><img src="../Styles/Images/SC3090401/icon_No.png" width="40px" height="40px" alt="" /></div>
                        </div>
                        <div id="btn_sort_time">
                            <div class="imgSort"><img src="../Styles/Images/SC3090401/icon_time.png" width="40px" height="40px" alt="" /></div>
                        </div>
                    </div>
                </div>
                <div id="VisitInfoContents">
                    <div id="PullDownToRefreshDiv" class="PullDownToRefreshDiv"></div>
                    <asp:UpdatePanel ID="RezListUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="NoSearchImage" runat="server">
                                <icrop:CustomLabel runat="server" ID="NoSearchWord" CssClass="Ellipsis" />
                                <icrop:CustomLabel runat="server" ID="NoSearchNoShowWord" CssClass="Ellipsis" />
                            </div>
                            <div class="Bottom_TBL">
                                <div runat="server" id="BackPage" class="AppointmentListNextMore" style="display: none;">
                                    <icrop:CustomLabel runat="server" ID="BackPageWord" />
                                </div>
                                <div runat="server" id="BackPageLoad" class="AppointmentListNextMore" style="display: none;">
                                    <icrop:CustomLabel runat="server" ID="BackPageLoadWord" />
                                    <span class="LoadImage"></span>
                                </div>
                                <ul class="DisplayOn">
                                    <asp:Repeater ID="VisitServiceInfoRepeater" runat="server" EnableViewState="false">
                                        <ItemTemplate>
                                            <li runat="server">
                                                <%-- $01 start Gate Keeper機能の視認性操作性改善 --%>
                                                <%-- 予約日時 --%>
                                                <%-- <div class="Contents_InBox W01"> --%>
                                                <%--     <asp:Label ID="ReserveDatetime" runat="server"></asp:Label> --%>
                                                <%-- </div> --%>
                                                
                                                <%-- サービス名称 --%>
                                                <%-- <div class="Contents_InBox W02"> --%>
                                                <%--     <asp:Label ID="ServiceName" runat="server"></asp:Label> --%>
                                                <%-- </div> --%>
                                                
                                                <%-- モデル名 --%>
                                                <%-- <div class="Contents_InBox W01"> --%>
                                                <%--     <asp:Label ID="ModelName" runat="server"></asp:Label> --%>
                                                <%-- </div> --%>
                                                
                                                <%-- 車両登録番号 --%>
                                                <%-- <div class="Contents_InBox W02"> --%>
                                                <%--     <asp:Label ID="VehicleRegNum" runat="server"></asp:Label> --%>
                                                <%-- </div> --%>
                                                
                                                <%-- 顧客名 --%>
                                                <%-- <div class="Contents_InBox W03"> --%>
                                                <%--     <asp:Label ID="CustomerName" runat="server"></asp:Label> --%>
                                                <%-- </div> --%>
                                                <div class="WCBoxType01">
                                                    <div class="LeftBox">
                                                        <%-- モデル名 --%>
                                                        <div class="Contents_InBox W01">
                                                            <asp:Label ID="ModelName" runat="server"></asp:Label>
                                                        </div>
                                                        
                                                        <%-- 車両登録番号 --%>
                                                        <div class="Contents_InBox W02">
                                                            <asp:Label ID="VehicleRegNum" runat="server"></asp:Label>
                                                        </div>
                                                        
                                                        <%-- 顧客名 --%>
                                                        <div class="Contents_InBox W03">
                                                            <asp:Label ID="CustomerName" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="RightBox">
                                                        <%-- 予約日時 --%>
                                                        <div class="W04" >
                                                            <asp:Label ID="ReserveDatetime" runat="server"></asp:Label>
                                                        </div>

                                                        <%-- サービス分類名称 --%>
                                                        <div class="icTime" >
                                                            <asp:Label ID="ServiceName" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- $01 end Gate Keeper機能の視認性操作性改善 --%>
                                                <%-- サービス入庫ID --%>
                                                <asp:HiddenField ID="ServiceinId" runat="server" />
                                                <%-- 更新日時 --%>
                                                <asp:HiddenField ID="UpdateDate" runat="server" />
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div runat="server" id="NextPage" class="AppointmentListNextMore" style="display: none;">
                                    <icrop:CustomLabel runat="server" ID="NextPageWord" />
                                </div>
                                <div runat="server" id="NextPageLoad" class="AppointmentListNextMore" style="display: none;">
                                    <icrop:CustomLabel runat="server" ID="NextPageLoadWord" />
                                    <span class="LoadImage"></span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <%'プルダウンリフレッシュのレイアウトテンプレートエリア %>
        <div id="pullDownToRefreshTemplate" style="display: none">
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
                                <icrop:CustomLabel ID="FixMessagStep0" runat="server" TextWordNo="2" CssClass="pullDownToRefresh-message-step0 Ellipsis"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep1" runat="server" TextWordNo="3" CssClass="pullDownToRefresh-message-step1 Ellipsis"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep2" runat="server" TextWordNo="4" CssClass="pullDownToRefresh-message-step2 Ellipsis"></icrop:CustomLabel><br />
                                <icrop:CustomLabel ID="FixMessageUpdateTime" runat="server" TextWordNo="5" CssClass="pullDownToRefresh-message-updateTime"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="MessageUpdateTime" runat="server" CssClass="pullDownToRefresh-message-updateTime Ellipsis"></icrop:CustomLabel>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <!--プルダウンリフレッシュエリア END-->
        </div>
    </div>
    <!--メインコンテンツここまで-->
    <div id="SC3090401HiddenArea">
        <%--選択した予約のサービス入庫ID--%>
        <asp:HiddenField ID="HiddenSelectServiceinId" runat="server" />
        <%--選択した予約の更新日時--%>
        <asp:HiddenField ID="HiddenSelectUpdateDate" runat="server" />

        <%--サーバ時間--%>
        <asp:HiddenField runat="server" ID="ServerTimeHidden" />
        <%--日付フォーマット--%>
        <asp:HiddenField ID="hidDateFormatMMdd" runat="server" />
        <asp:HiddenField ID="hidDateFormatHHmm" runat="server" />
        <asp:UpdatePanel ID="ButtonUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%-- 来店登録の確認メッセージ --%>
                <asp:HiddenField ID="RegistConfirmMessageText" runat="server" />
                <%-- 来店取消の確認メッセージ --%>
                <asp:HiddenField ID="CancelConfirmMessageText" runat="server" />
                <%-- 開始行番号 --%>
                <asp:HiddenField ID="AppointmentListBeginIndex" runat="server" />
                <%-- 終了行番号 --%>
                <asp:HiddenField ID="AppointmentListEndIndex" runat="server" />

                <asp:Button ID="InitButton" runat="server" Style="display: none" />
                <asp:Button ID="BackButton" runat="server" Style="display: none" />
                <asp:Button ID="AllDisplayButton" runat="server" Style="display: none" />
                <asp:Button ID="SortButton" runat="server" Style="display: none" />
                <asp:Button ID="BackPageButton" runat="server" Style="display: none;" />
                <asp:Button ID="NextPageButton" runat="server" Style="display: none;" />
                <asp:Button ID="PullDownRefreshButton" runat="server" Style="display: none;" />
                <asp:Button ID="VisitEventButton" runat="server" Style="display: none;" />
                <asp:Button ID="VisitCancelButton" runat="server" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
