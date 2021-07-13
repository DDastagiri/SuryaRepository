<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPageSmall.master" AutoEventWireup="false" CodeFile="SC3320101.aspx.vb" Inherits="Pages_SC3320101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3320101/common.css?201409242140111" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3320101/MDR_2000U.css?201410092140111" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3320101/SC3320101.PullDownRefresh.css?201409242140111" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3320101/OriginalKeyBoard.css?201409242140111" type="text/css" media="screen,print" />

    <script type="text/javascript" src="../Scripts/SC3320101/SC3320101.js?201410092140111"></script>
    <script type="text/javascript" src="../Scripts/SC3320101/SC3320101.fingerscroll.js?201409242140111"></script>
    <script type="text/javascript" src="../Scripts/SC3320101/SC3320101.Define.js?201409242140111"></script>
    <script type="text/javascript" src="../Scripts/SC3320101/jquery.cookie.js?201409242140111"></script>
    <script type="text/javascript" src="../Scripts/SC3320101/originalKeyBoard.js?201409242140111"></script>
    <script type="text/javascript" src="../Scripts/SC3320101/SC3320101.PullDownRefresh.js?201409242140111"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    	<div class="SearchBox">
    		<div class="SearchIcon"></div>
            <%--2019/01/21 NSK 河谷 (FS)次世代オペレーションの試行に向けた評価 UAT-0203 CarWashメインの検索バーをタップしても入力できない START--%>
    		<%--<div class="SearchArea"><input name="search" id="search" onclick="SwitchSearchMode(this,0);" readonly="true" type="text" placeholder="<%:WebWordUtility.GetWord("SC3320101", 11)%>" /></div>--%>
    		<div class="SearchArea"><input name="search" id="search" onclick="SwitchSearchMode(this,0);" readonly="true" type="text" placeholder="<%:WebWordUtility.GetWord("SC3320101", 11)%>" maxlength="32" /></div>
            <%--2019/01/21 NSK 河谷 (FS)次世代オペレーションの試行に向けた評価 UAT-0203 CarWashメインの検索バーをタップしても入力できない END--%>
        </div>

<div id="LoadingScreen"></div> 
<div id="MM_Main_Contents" Runat="server" > 
    	<div class="Bottom_Box">
          <div class="VisitInfo_TBL">

          <%--<asp:TextBox ID="Search" class="TXC_03" readonly="true" OnFocus="SwitchSearchMode(this,0);" runat="server" Width="98" />--%>

    		<table class="Top_TBL" border="0" cellspacing="0" cellpadding="0">
    	    	<tr class="TBL_head HG38">
    	    	  <th class="head_Type01 W01 TC_BG05 head_InBox01">
                    	<icrop:CustomLabel runat="server" id="SaName" class="TX_01 Ellipsis" TextWordNo="2" Width="241"/>
                  </th>
    	    	  <th class="head_Type02 W02 TC_BG05 head_InBox02">
                  <icrop:CustomLabel ID="RegNo" runat="server" class="TX_02 Ellipsis" TextWordNo="3" Width="241"/>
                  </th>
  	    	  </tr>
    	    	<tr class="TBL_head HG39">
    	      		<th class="head_Type01 W01 TC_BG06 head_InBox03">
                    <icrop:CustomLabel ID="ModelName" runat="server" class="TX_01 Ellipsis" TextWordNo="4" Width="241"/>
                  	</th>
    	      		<th class="head_Type02 W02 TC_BG06 head_InBox04">
                    <icrop:CustomLabel ID="LocationCode" runat="server" class="TX_02 Ellipsis" TextWordNo="5" Width="241"/>
              		</th>
  	      		</tr>
  	    	</table>
            
            <div id="VisitInfoContents">
            <div id="PullDownToRefreshDiv" class="PullDownToRefreshDiv"></div>
   		  <table class="Bottom_TBL" border="0" cellspacing="0" cellpadding="0">
            <asp:Repeater ID="VisitServiceInfoRepeater" runat="server">
                <ItemTemplate>
    	    	<tr class="DisplayOn">
    	      		<td class="Contents_Type01 W03" runat="server">
                    	<div class="Contents_InBox"><icrop:CustomLabel ID="SANameLabel" runat="server" Class="TXC_01 Ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("USERNAME")) %>' Width="213"></icrop:CustomLabel></div>
                 	</td>
    	      		<td class="Contents_Type02 W04">
                    	<div class="Contents_InBox"><icrop:CustomLabel ID="RegNoLabel" runat="server" Class="TXC_02 Ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("VCLREGNO")) %>' Width="337"></icrop:CustomLabel></div>
             		</td>
  	      		</tr>
    	    	<tr class="DisplayOn">
    	      		<td class="Contents_Type01 W03">
                    	<div class="Contents_InBox"><icrop:CustomLabel ID="ModelNamemLabel" runat="server" Class="TXC_01 Ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("MODEL_NAME")) %>' Width="213"></icrop:CustomLabel></div>
                 	</td>
    	      		<td class="Contents_Type02 W04">
                    	<div class="Contents_InBox" >
                        <div class="Loc_BoxAlp">               
                        <asp:TextBox ID="ParkingCodeAlpTxt" class="TXC_03" runat="server" onClick="SwitchEditingMode(this,1);" OnChange="ChangeFocus();" readonly="true"  MaxLength="1" Width="98" Text='<%# HttpUtility.HtmlEncode(Left(Eval("PARKINGCODE"),1).trim()) %>'/>
                        </div>
                        <div class="Loc_BoxNum">
                        <asp:TextBox ID="ParkingCodeNumTxt" class="TXC_03" runat="server" onClick="SwitchEditingMode(this,0);" readonly="true"  MaxLength="2" Width="148" Text='<%# HttpUtility.HtmlEncode(Eval("PARKINGCODE").SubString(1).trim()) %>'  visitseq='<%# HttpUtility.HtmlEncode(Eval("VISITSEQ")) %>' regnum='<%# HttpUtility.HtmlEncode(Eval("VCLREGNO")) %>' />
                        </div>
                        </div>
             		</td>
  	      		</tr>
                </ItemTemplate>
            </asp:Repeater>          
  	    </table>
        </div>
            
           </div>
<%--           <div class="TC_Button02 TC_BG09 Ellipsis"><%:WebWordUtility.GetWord("SC3320101", 6)%></div>--%>
           
        </div>

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
                                <icrop:CustomLabel ID="FixMessagStep0" runat="server" TextWordNo="7" CssClass="pullDownToRefresh-message-step0 Ellipsis"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep1" runat="server" TextWordNo="8" CssClass="pullDownToRefresh-message-step1 Ellipsis"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep2" runat="server" TextWordNo="9" CssClass="pullDownToRefresh-message-step2 Ellipsis"></icrop:CustomLabel><br />
                                <icrop:CustomLabel ID="FixMessageUpdateTime" runat="server" TextWordNo="10" CssClass="pullDownToRefresh-message-updateTime Ellipsis"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="MessageUpdateTime" runat="server"  CssClass="pullDownToRefresh-message-updateTime Ellipsis"></icrop:CustomLabel>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <!--プルダウンリフレッシュエリア END-->
        </div>
         <div id ="OriginalKeyBoard" style='top:365px;left:0px;height:550px;width:640px;position:fixed;visibility:hidden;border:none;background-color:#FFF'/>
    </div>
    <!--メインコンテンツここまで-->
    <div id="SC3320101HiddenArea">
    <asp:HiddenField runat="server" ID="HeadTitleHidden"/>              <%--ヘッダー文言--%>
    <asp:HiddenField runat="server" ID="NotChangeErrMsgHidden"/>       <%--ロケーションが変更されていない時に登録ボタンを押された場合--%>
    <asp:HiddenField runat="server" ID="NotSelectedErrMsgHidden"/>       <%--RFIDを読取った時にテキストが選択されていなかった場合--%>
    <asp:HiddenField runat="server" ID="RefureshTimeHidden"/>       <%--システム設定から取得した自動リフレッシュタイム--%>
    <asp:HiddenField runat="server" ID="ServerTimeHidden"/>       <%--サーバ時間--%>
    <asp:HiddenField ID="hidDateFormatMMdd" runat="server"/>
    <asp:HiddenField ID="hidDateFormatHHmm" runat="server"/>
    </div>
</asp:Content>

