<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3100303.aspx.vb" Inherits="Pages_SC3100303" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--スクリプト(画面固有)--%>
    <link rel="Stylesheet" href="../styles/SC3100303/SC3100303.css?20180220000000" type="text/css" media="all" />
    <script type="text/javascript" src="../Scripts/SC3100303/SC3100303.fingerScroll.js?20130304000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100303/SC3100303.js?20180220000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100303/SC3100303.ChipPrototype.js?20180220000000"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <%'2018/02/22 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START%>
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>
    <%'2018/02/22 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END%>
	<div id="MainArea">
        <div id="Inner">
            <div class="tsl02-01_Title">
                <%'日付エリア %>
			    <div class="SMBLogo">
                    <%'日付 %>
                    <div class="Date">
                        <icrop:CustomLabel ID="pCalendar" runat="server" Width="106px" UseEllipsis="true"></icrop:CustomLabel>
                    </div>
                    <%'左ボタン %>
                    <div class="LeftButton_trimming" onclick="imgbtnPrevDate_onClick();">
                        <div class="LeftButton"></div>
                    </div>
                    <%'右ボタン %>
                    <div class="RightButton_trimming" onclick="imgbtnNextDate_onClick();">
                        <div class="RightButton"></div>
                    </div>
                </div>
                <%'タイトル %>
                <icrop:CustomLabel ID="lblTitle" runat="server" TextWordNo="2"></icrop:CustomLabel>
                <%'来店実績台数 %>
                <div class="DivAllNumber">
                    <icrop:CustomLabel ID="lblVstCarCnt" runat="server" Width="300px" UseEllipsis="true"></icrop:CustomLabel>
                </div>
            </div>
            <div class="shadowBox"></div>
            <div class="blackBackGround"></div>
            <div class="tsl02-01_bodyBox">
                <div class="tsl02-01_innerBox">
                    <ul class="TimesBox">
                    <%'メインエリア %>
                    <asp:Repeater ID="stallTimeRepeater" runat="server" EnableViewState="false" ClientIDMode="Static">
                        <ItemTemplate>
                            <li class="hourSet p<%# HttpUtility.HtmlEncode(Eval("No"))%>">
                                <h3><%# HttpUtility.HtmlEncode(Eval("Time"))%></h3>
                                <div class="Number"></div>
                                <div class="DivNumberWord">
                                    <icrop:CustomLabel ID="NumberWord" runat="server" TextWordNo="4" Width="25px" UseEllipsis="true" class="SpanNumberWord"></icrop:CustomLabel>
                                </div>
                                <div class="DataSet">
                                <div class="halfLines"></div>
                                </div>
                            </li>
                            <li class="wbChipsArea p<%# HttpUtility.HtmlEncode(Eval("No"))%>">
                                <ul class="Inner">
                                </ul>
                            </li>
                            <li class="wbChipsArea p<%# HttpUtility.HtmlEncode(Eval("No"))%>h">
                                <ul class="Inner">
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    </ul>
                </div>
                <%'時刻線 %>
                <div class="GreenTimeLine TimeLinePosition"></div>
            </div>
        </div>

        <%'表示している日付 %>
        <asp:HiddenField ID="hidShowDate" runat="server"/>
        <%'サーバー時間 %>
        <asp:HiddenField ID="hidServerTime" runat="server"/>
        <%'チップのデータ %>
        <asp:HiddenField ID="hidJsonData" runat="server"/>
        <%'営業開始、終了時間 %>
        <asp:HiddenField ID="hidStallStartTime" runat="server"/>
        <asp:HiddenField ID="hidStallEndTime" runat="server"/>
        <%'遅刻時間、リフレッシュ時間 %>
        <asp:HiddenField ID="hidDelayTime" runat="server"/>
        <asp:HiddenField ID="hidRefreshTime" runat="server"/>
        <%'権限%>
        <asp:HiddenField ID="hidOpeCD" runat="server"/>
        <%'エラーメッセージ%>
        <asp:HiddenField ID="hidMsgData" runat="server"/>
        <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START%>
        <%'選択チップのREZID(サービス入庫ID)%>
        <asp:HiddenField ID="hidSelectedRezId" runat="server"/>
        <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <%'2014/01/17 TMEJ  陳　【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START %>
    <%--<div id="InitFooterArea" runat="server">
        <div class="InitFooterButton_Space"></div>
        <%'全体管理ボタン %>
        <div id="FooterButton100" runat="server"  onclick="FooterEvent(1);">
		    <div id="FooterButtonIcon100" runat="server"></div>
		    <div id="FooterButtonName100" runat="server" class="FooterName"><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="10" Width="78px" UseEllipsis="true"></icrop:CustomLabel></div>
        </div>
        <%'来店管理ボタン %>
        <div id="FooterButton200" runat="server"  onclick="FooterEvent(2);">
		    <div id="FooterButtonIcon200" runat="server"></div>
		    <div id="FooterButtonName200" runat="server" class="SelectedFooterName"><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="6" Width="78px" UseEllipsis="true"></icrop:CustomLabel></div>
        </div>
    </div>--%>
    <%'2014/01/17 TMEJ  陳　【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END %>
    <div id="ChipFooterArea" runat="server">
        <div class="ChipFooterButton_Space"></div>
        <%'フォローボタン %>
        <div id="FooterButton300" runat="server"  onclick="FooterEvent(3);">
		    <div id="FooterButtonIcon300" runat="server"></div>
		    <div id="FooterButtonName300" runat="server" class="ChipFooterName"><icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="5" Width="78px" UseEllipsis="true"></icrop:CustomLabel></div>
        </div>
        <%'フォロー解除ボタン %>
        <div id="FooterButton400" runat="server"  onclick="FooterEvent(4);">
		    <div id="FooterButtonIcon400" runat="server"></div>
		    <div id="FooterButtonName400" runat="server" class="ChipFooterName"><icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="9" Width="78px" UseEllipsis="true"></icrop:CustomLabel></div>
        </div>
        <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START%>
        <%'R/Oボタン %>
        <div id="FooterButton500" runat="server"  onclick="FooterEvent(5);">
		    <div id="FooterButtonIcon500" runat="server"></div>
		    <div id="FooterButtonName500" runat="server" class="ChipFooterName"><icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="19" Width="78px" UseEllipsis="true"></icrop:CustomLabel></div>
        </div>
        <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END%>
    </div>
    <%'2014/01/17 TMEJ  陳　【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START %>
    <%--<%'全体管理に遷移するためのパラメータ %>
    <asp:Button ID="GeneralMngButton" runat="server" style="display:none" />--%>
    <%'2014/01/17 TMEJ  陳　【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END %>
    <%'顧客に遷移するためのパラメータ %>
    <asp:Button ID="CustomerButton" runat="server" style="display:none" />
    <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START%>
    <asp:UpdatePanel ID="ContentUpdateButtonPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%'R/O作成に遷移するためのダミーボタン %>
            <asp:Button ID="ROCreateButton" runat="server" style="display:none" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END%>
</asp:Content>