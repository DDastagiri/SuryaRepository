<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPageSmall.master" AutoEventWireup="false" CodeFile="SC3340101.aspx.vb" Inherits="Pages_SC3340101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../Styles/SC3340101/common.css?201501280000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/header.css?201501280000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/tcmain.css?201501280000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/CW010_common.css?201902200000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/footer.css?201502040000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/SC3340101.PullDownRefresh.css?201501280000000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3340101/SC3340101.OriginalKeyBoard.css?201501280000000" type="text/css" media="screen,print" />

    <script type="text/javascript" src="../Scripts/SC3340101/SC3340101.js?20150303000000"></script>
    <script type="text/javascript" src="../Scripts/SC3340101/SC3340101.fingerscroll.js?201501300000000"></script>
    <script type="text/javascript" src="../Scripts/SC3340101/SC3340101.originalKeyBoard.js?201501280000000"></script>
    <script type="text/javascript" src="../Scripts/SC3340101/SC3340101.jquery.cookie.js?201501280000000"></script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering = "true"  runat="server"></asp:ScriptManager>

    <div class="SearchBox">
    	<div class="SearchIcon"></div>
    	<div class="SearchArea">
            <%--2019/01/21 NSK 河谷 (FS)次世代オペレーションの試行に向けた評価 UAT-0203 CarWashメインの検索バーをタップしても入力できない START--%>
            <%--<input name="search" id="search" onclick="SwitchSearchMode(this,0);" readonly="true" type="text" placeholder="<%:WebWordUtility.GetWord("SC3340101", 7)%>" />--%>
            <input name="search" id="search" onclick="SwitchSearchMode(this,0);" readonly="true" type="text" placeholder="<%:WebWordUtility.GetWord("SC3340101", 7)%>" maxlength="32" />
            <%--2019/01/21 NSK 河谷 (FS)次世代オペレーションの試行に向けた評価 UAT-0203 CarWashメインの検索バーをタップしても入力できない END--%>
        </div>
    </div>
    <!--メインコンテンツここから-->
  
	<div id="Operation_Contents">     
  	    <div class="InnerBox"> 
            <div class="insiderSelect" style="display: none"></div>      
            <div id="PullDownToRefreshDiv" class="PullDownToRefreshDiv">
            
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
                                    <icrop:CustomLabel ID="FixMessagStep0" runat="server" TextWordNo="8" CssClass="pullDownToRefresh-message-step0 Ellipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel ID="FixMessageStep1" runat="server" TextWordNo="9" CssClass="pullDownToRefresh-message-step1 Ellipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel ID="FixMessageStep2" runat="server" TextWordNo="10" CssClass="pullDownToRefresh-message-step2 Ellipsis"></icrop:CustomLabel>
                                    <br />
                                    <icrop:CustomLabel ID="FixMessageUpdateTime" runat="server" TextWordNo="11" CssClass="pullDownToRefresh-message-updateTime Ellipsis"></icrop:CustomLabel>
                                    <icrop:CustomLabel ID="MessageUpdateTime" runat="server"  CssClass="pullDownToRefresh-message-updateTime Ellipsis"></icrop:CustomLabel>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--プルダウンリフレッシュエリア END-->
            </div>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildAsTrigger = "true"> 
                <ContentTemplate>
                    <div class="InnerScrollDiv">          
                        <asp:Repeater ID="CarWashRepeater" runat="server">
                            <ItemTemplate>
                                <!--内側コンテンツここから-->
                                <div class="WCBoxType01 <%# HttpUtility.HtmlEncode(Eval("SVCIN_ID")) %>" >
                                    <div id="divDelayColor"  runat="server">
      	                                <div  class="line01 DelayL" ></div>
                                        <div id="divHeadBox" runat="server">
                                            <icrop:CustomLabel runat="server" CssClass="Model Ellipsis " Text='<%# HttpUtility.HtmlEncode(Eval("MODEL_NAME")) %>' ></icrop:CustomLabel>
                                            <div class="NumberBox">
                                                <icrop:CustomLabel runat="server" CssClass="Number Ellipsis " Text='<%# HttpUtility.HtmlEncode(Eval("REG_NUM")) %>'></icrop:CustomLabel>
                                            </div>
                                            <!--2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START-->
                                            <div id ="LIcon" runat="server" class="TkmIconL" text="" visible="false"></div>
                                            <div id ="PIcon" runat="server" class="TkmIconP" text="" visible="false"></div>
                                            <!--2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END-->
                                        </div>

                                        <div id="divFootBox" class="FootBox" runat="server">
                                            <icrop:CustomLabel runat="server" CssClass="Model Ellipsis" TextWordNo="2"></icrop:CustomLabel>
                                            <icrop:CustomLabel id="lblScheDeliDate" runat="server" CssClass="TimeBox Ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("SHOW_SCHE_DELIDATE")) %>' ></icrop:CustomLabel>
                                        </div>
                                        <div class="RightBox">
        	                                <div id="divPickDeli" runat="server" class="icHuman pickDeliTypeIcon" >
                                                <span >&nbsp;</span>
                                            </div>
        	                                <div id="divAcceptanceType" runat="server" class="icTime acceptanceTypeIcon" >
                                                <span >&nbsp;</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="boderLine <%# HttpUtility.HtmlEncode(Eval("SVCIN_ID")) %>Line">&nbsp;</div>
                            </ItemTemplate>
                        </asp:Repeater>          
    	                <!--内側コンテンツここまで-->           
                    </div>
                  
                    <div id="divCarCount"  style="display:none" runat="server">                             
                        <div id="CarCountShow" onclick = "GetNextCarWash();" runat="server" class="CarCount Ellipsis">
                            <div id="CarCountHtml" class="wordSet Ellipsis" runat="server"></div>                                            
                        </div>
                    </div>
                    <div id="ClickCarCount" style="display:none">                      
                        <img runat="server" id="imgChkJisya" alt="" src="../Styles/Images/SC3340101/animeicn-1.png"/>               
                        <div id="CustomLabeClickCount" runat="server" class="CarCountReplace Ellipsis">
                            <div id="DisCarCountHtml" class="wordSet Ellipsis" runat="server"></div>                      
                        </div>
                    </div> 
                            
                    <asp:Button ID="btnMainLoading" runat="server" Text="" style="display: none" />
                    <asp:Button ID="btnAddLoading" runat="server" Text="" style="display: none" />
                    
                    <!--メインコンテンツここまで-->
                    <div id="SC3340101HiddenArea">
                        <asp:HiddenField runat="server" ID="HeadTitleHidden"/>          <%--ヘッダー文言--%>
                        <asp:HiddenField runat="server" ID="ServerTimeHidden"/>         <%--サーバ時間--%>
                        <asp:HiddenField runat="server" ID="RefreshTimeHidden"/>        <%--システム設定から取得した自動リフレッシュタイム--%>
                        <asp:HiddenField runat="server" ID="ReadCountHidden"/>          <%--システム設定から取得した取得件数--%>
                        <asp:HiddenField runat="server" ID="CarWashHiddenInfo"/>        <%--洗車バナー情報--%>
                        <asp:HiddenField runat="server" ID="hidDateFormatMMdd" />
                        <asp:HiddenField runat="server" ID="hidDateFormatHHmm" />
                        <asp:HiddenField runat="server" ID="hidErrorMeg"/>
                        <asp:HiddenField runat="server" ID="hidNextCount"/>

                        <asp:HiddenField ID="hidPostBackParamClass" runat="server" />
                    </div>

                </ContentTemplate> 
            </asp:UpdatePanel>
  	    </div>
    </div>

    <!--メインコンテンツここまで-->
    
    <%--ダミーボタン--%>
    <div>
        <asp:Button ID="btnCarWashStart" runat="server" Text="" style="display: none" />
        <asp:Button ID="btnCarWashUndo" runat="server" Text="" style="display: none" />
        <asp:Button ID="btnCarWashSkip" runat="server" Text="" style="display: none" />
        <asp:Button ID="btnCarWashFinish" runat="server" Text="" style="display: none" />
    </div>

    <%'プルダウンリフレッシュのレイアウトテンプレートエリア %>
    <div id="pullDownToRefreshTemplate" style="display:none">
        
    </div>

    <div class="SelectWindow" style="display:none"></div> 
    <div class="SelectWindowLeft" style="display:none"></div> 
    <div class="SelectWindowRight" style="display:none"></div> 
    <div class="SelectWindowBottom" style="display:none"></div> 
    <!--フッターここから-->
	<div id="footer">
       
        <ul class="NaviSet">
            
            <!--洗車開始ボタン(非活性) -->
            <li id="btnStart" class="linker11">
                <a href="#" onclick="return ClickBtnStart();">
                    <icrop:CustomLabel ID="CustomLabel9" CssClass="Ellipsis" runat="server" TextWordNo="3"></icrop:CustomLabel>
                </a>
            </li>
            
            <!--洗車スキップボタン(非活性) -->
            <li id="btnSkip" class="linker12">
                <a href="#" onclick="return ClickBtnSkip();">
                    <icrop:CustomLabel ID="CustomLabel10" CssClass="Ellipsis" runat="server" TextWordNo="4"></icrop:CustomLabel>
                </a>
            </li>

            <!--洗車終了ボタン(非活性) -->
            <li id="btnFinish" class="linker13">
                <a href="#" onclick="return ClickBtnFinish();">
                    <icrop:CustomLabel ID="CustomLabel11" CssClass="Ellipsis" runat="server" TextWordNo="5"></icrop:CustomLabel>
                </a>
            </li>

            <!--洗車Undoボタン(非活性) -->
            <li id="btnUndo" class="linker14">
                <a href="#" onclick="return ClickBtnUndo();">
                    <icrop:CustomLabel ID="CustomLabel12" CssClass="Ellipsis" runat="server" TextWordNo="6"></icrop:CustomLabel>
                </a>
            </li>


            <!--洗車開始ボタン(活性) -->
            <li id="btnStartOn" class="linker11_on" style="visibility: hidden">
                <a href="#" onclick="return ClickBtnStart();">
                    <icrop:CustomLabel ID="CustomLabel13" CssClass="Ellipsis" runat="server" TextWordNo="3" ></icrop:CustomLabel>
                </a>
            </li>
                       
            <!--洗車スキップボタン(活性) -->
            <li id="btnSkipOn" class="linker12_on" style="visibility: hidden">
                <a href="#" onclick="return ClickBtnSkip();">
                    <icrop:CustomLabel ID="CustomLabel14" CssClass="Ellipsis" runat="server" TextWordNo="4"></icrop:CustomLabel>
                </a>
            </li>
                
            <!--洗車終了ボタン(活性) -->
            <li id="btnFinishOn" class="linker13_on" style="visibility: hidden">
                <a href="#" onclick="return ClickBtnFinish();">
                    <icrop:CustomLabel ID="CustomLabel15" CssClass="Ellipsis" runat="server" TextWordNo="5"></icrop:CustomLabel>
                </a>
            </li>
                
            <!--洗車Undoボタン(活性) -->
    	    <li id="btnUndoOn" class="linker14_on" style="visibility: hidden">
                <a href="#" onclick="return ClickBtnUndo();">
                    <icrop:CustomLabel ID="CustomLabel16" CssClass="Ellipsis" runat="server" TextWordNo="6"></icrop:CustomLabel>
                </a>
            </li>
            
        </ul>
           
	</div>
    <div id ="OriginalKeyBoard" style='top:365px;left:0px;height:550px;width:640px;position:fixed;visibility:hidden;border:none;background-color:#FFF'/>
	<!--フッターここから-->
</asp:Content>
