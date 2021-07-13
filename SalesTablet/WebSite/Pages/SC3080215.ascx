<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080215.ascx
'─────────────────────────────────────
'機能： CSSurvey一覧・詳細
'補足： 
'作成： 2012/02/20 TCS 明瀬
'更新： 2012/04/13 TCS 明瀬 HTMLエンコード対応
'更新： 2019/06/06 TS  重松 ポップアップの表示位置を制御 上下の表示設定を消去（UAT-0504）
'─────────────────────────────────────
-->

<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080215.ascx.vb" Inherits="Pages_SC3080215" %>

<link rel="Stylesheet" href="../Styles/SC3080215/SC3080215.css?20120327000000" />
<%--2019/06/06 TS  重松 ポップアップの表示位置を制御 上下の表示設定を消去（UAT-0504） START--%>                                                                                                                             
<script type="text/javascript" src="../Scripts/SC3080215/SC3080215.js?20190607000000"></script>
<%--2019/06/06 TS  重松 ポップアップの表示位置を制御 上下の表示設定を消去（UAT-0504） END--%>                                                                                                                             
<script type="text/javascript" src="../Scripts/TCS/jquery.flickable.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.PopOverForm.js"></script>
<script type="text/javascript" src="../Scripts/TCS/jquery.popover.js"></script>

<asp:Panel runat="server" ID="CSserveyMainPanel" Visible="true">
           
    <!-- data-TriggerClientIDの値は親画面から設定されたプロパティ値に依存する -->
    <div id="CSSurveyPopOverForm" runat="server" data-TriggerClientID="CSSurveyButton">

        <!-- PopOverForm ヘッダー部 Start -->
        <div id="CSSurveyHeader" class='icrop-PopOverForm-header' style='height:30px;'>
            <!-- ヘッダー左部分 -->
            <div id="CSSurveyBackButton" runat="server" class='icrop-PopOverForm-header-left CSSurveyTitleButtonLeft CSSurveyEllipsis' style='display:none;' onclick="moveToCSServeyList()">
                <a href="#" runat="server" class="CSSurveyEllipsis"></a>
                <span class="tgLeft">&nbsp;</span>
			</div>
            <!-- ヘッダー中央部分 -->
            <div class='icrop-PopOverForm-header-title CSSurveyTitleName'>
				<h3>
                    <icrop:CustomLabel ID="CSSurveyTitleLabel" runat="server" CssClass="CSSurveyClip" UseEllipsis="false" Width="310px" style="margin-left:10px; margin-top:2px;"></icrop:CustomLabel>
				</h3>
			</div>
            <!-- ヘッダー右部分 -->
            <div id="CSSurveyHeaderRight" class='icrop-PopOverForm-header-right'></div>

            <!-- コールバック時のオーバーレイ -->
            <div id="registOverlayBlackSC3080215"></div>
            <div id="processingServerSC3080215"></div>

            <!-- 詳細の第２ヘッダータップ時にグレーゾーンが出る不具合対応のdivコントロール -->
            <div id="detailheadsetOverlay1" style="display:none; position:absolute; width:480px; height:28px; opacity:0.001; top:44px; left:16px; background:#FFF; z-index:100001;"></div>
            <div id="detailheadsetOverlay2" style="display:none; position:absolute; width:10px; height:100px; opacity:0.001; top:44px; left:486px; background:#FFF; z-index:100001;"></div>

        </div> 
        <!-- PopOverForm ヘッダー部 End -->

        <!-- PopOverForm メインコンテンツ Start -->
        <div class="icrop-PopOverForm-content" style="width:480px; height:560px; overflow:hidden;">
		    <div class="icrop-PopOverForm-sheet" style="width:1000px; height:552px;">
                            
                <!-- CSServey一覧(１ページ目) Start -->
                <div id="CSSurveyPage1" class="icrop-PopOverForm-page CSSurveyListContent" style="float:left; overflow-y:scroll; overflow-x:hidden; background-color:rgba(0, 0, 0, 0);">
                    <div id="CSSurveyListScroll" class="CSSurveyListContentBody" style="height:550px; background-color:rgba(0, 0, 0, 0);">
                        <div style="padding-bottom:10px;">
						    <ul class="CSSurveyListData" style=" background-color:rgba(0, 0, 0, 0); width:470px;">
                                <asp:Repeater runat="server" ID="CSSurveyListRepeater" EnableViewState="False">
                                    <ItemTemplate>
                                        <li id="CSSurveyListRow<%# DataBinder.Eval(Container.DataItem, "ROWINDEX")%>" onclick='selectCSServeyList(CSSurveyListRow<%# DataBinder.Eval(Container.DataItem, "ROWINDEX")%>)' answerid='<%# HttpUtility.HtmlEncode(Eval("ANSWERID"))%>' papername='<%# HttpUtility.HtmlEncode(Eval("PAPERNAME"))%>' iconfilename='<%# HttpUtility.HtmlEncode(Eval("ICONFILENAME"))%>' staffname='<%# HttpUtility.HtmlEncode(Eval("STAFFNAME"))%>' seriesname='<%# HttpUtility.HtmlEncode(Eval("SERIESNAME"))%>' vclregno='<%# HttpUtility.HtmlEncode(Eval("VCLREGNO"))%>' dateword='<%# HttpUtility.HtmlEncode(Eval("DATEWORD"))%>'>
                                            <span id="surveyNameBox" runat="server" class="surveyNameBox CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("PAPERNAME"))%></span>
                                            <span id="vehicleBox" runat="server" class="vehicleBox CSSurveyEllipsis">
                                                <%# HttpUtility.HtmlEncode(Eval("SERIESNAME"))%><br/>
                                                <%# HttpUtility.HtmlEncode(Eval("VCLREGNO"))%>
                                            </span>
                                            <span id="updateBox" runat="server" class="updateBox" style='background:url(../Styles/Images/Authority/<%# HttpUtility.HtmlEncode(Eval("ICONFILENAME"))%>) left bottom no-repeat;'>
                                                <span class="updateDate CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("DATEWORD"))%></span>
                                                <span id="updateAccountName" runat="server" class="updateAccountName CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("STAFFNAME"))%></span>
                                            </span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
						    </ul>
                        </div>
                    </div>
                </div>
                <!-- CSServey一覧(１ページ目) End -->

                <!-- CSServey詳細(２ページ目) Start -->
                <div id="CSSurveyPage2" class="icrop-PopOverForm-page CSSurveyDetailContent" style="margin-left:3px; float:left; overflow-y:scroll; overflow-x:hidden; ">
                    <div class="CSSurveyDetailContentBody">
                        <!-- 第２ヘッダー Start -->
                        <div class="headerSet" style="width:470px;">
                            <asp:Label ID="updStaffNameLabel" runat="server" CssClass="updateAccountName CSSurveyEllipsis"></asp:Label>
                            <asp:Label ID="vehicleLabel" runat="server" CssClass="vehicle CSSurveyEllipsis"></asp:Label>
                            <asp:Label ID="regNoLabel" runat="server" CssClass="regNo CSSurveyEllipsis"></asp:Label>
                            <asp:Label ID="dateWordLabel" runat="server" CssClass="updateDate CSSurveyEllipsis"></asp:Label>
                        </div>
                        <!-- 第２ヘッダー End -->

				        <div id="CSSurveyDetailScroll" class="CSSurveyDetailContentBodyWrap">
					        <div style="padding-bottom:2px;">
						        <ul class="CSSurveyDetailList" style="width:470px;">        
                                    <!-- 詳細の親リピーター -->
                                    <asp:Repeater runat="server" ID="CSSurveyDetailRepeater" EnableViewState="False">
                                        <ItemTemplate>   
                                                                  
                                            <!-- 質問行 Start -->  
                                            <table id="questionTable" runat="server" border="0" cellspacing="0" cellpadding="0" style="table-layout:fixed; border:1px solid #BBB; background:#dcdcdc;" width="100%">
                                                <tr>
                                                    <th valign="middle" align="center" class="qBox">
                                                        <div style="width:55px;">
                                                            <h6 class="CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("QUESTION_INDEXNAME"))%></h6>
                                                        </div>
                                                    </th>

                                                    <td valign="middle" style="border-left:1px solid #BBB;">   
                                                        <%--2012/04/13 TCS 明瀬 HTMLエンコード対応 Start--%>                                                                                                                             
                                                        <div id="questionText" runat="server" style="width:370px; margin:10px 10px 10px 10px; color:#8B8B8B;" class="CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("QUESTIONCONTENT"))%></div>
                                                        <%--2012/04/13 TCS 明瀬 HTMLエンコード対応 End--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- 質問行 End --> 

                                            <!-- 回答行 Start -->
                                            <table id="answerTable" runat="server" border="0" cellspacing="0" cellpadding="0" style="table-layout:fixed; border-right:1px solid #BBB; border-left:1px solid #BBB; background:#fff;" width="100%">
                                                <tr>
                                                    <th runat="server" id="answerIndexBox" valign="middle" align="center" class="aBox" >
                                                        <div style="width:55px;">
                                                            <h6 class="CSSurveyEllipsis"><%# HttpUtility.HtmlEncode(Eval("ANSWER_INDEXNAME"))%></h6>
                                                        </div>
                                                    </th>

                                                    <td id="answerTd" runat="server" valign="middle" align="left" class="aContentBox" style="border-left:1px solid #BBB; ">
                                                        <asp:Repeater runat="server" ID="CSSurveyDetailAnswerRepeater" EnableViewState="False" DataSource='<%# GetChildView(Container.DataItem, "DetailRelation") %>' OnItemDataBound="CSSurveyDetailAnswerRepeater_ItemDataBound">
                                                            <ItemTemplate>
                                                                <div id="answerText" runat="server" style="width:370px; margin:10px 10px 10px 10px; color:#8B8B8B;" class=""><%# HttpUtility.HtmlEncode(Eval("TEXTRESULT"))%></div>
                                                                <dd id="answerContentBox" runat="server" class="CSSurveyEllipsis" style="text-align:left; margin:7px 10px 7px 10px;"><%# HttpUtility.HtmlEncode(Eval("ANSWERCONTENT"))%></dd>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- 回答行 End -->
                                        </ItemTemplate>
                                    </asp:Repeater>
						        </ul>
					        </div>
				        </div>
                    </div>
                </div>
                <!-- CSServey詳細(２ページ目) End -->
            </div>
            <!-- 一覧から詳細にデータを渡すためのHidden -->
            <asp:HiddenField ID="answerIdHidden" runat="server" />
            <asp:HiddenField ID="paperNameHidden" runat="server" />
            <asp:HiddenField ID="iconFileNameHidden" runat="server" />
            <asp:HiddenField ID="staffNameHidden" runat="server" />
            <asp:HiddenField ID="seriesNameHidden" runat="server" />
            <asp:HiddenField ID="vclRegNoHidden" runat="server" />
            <asp:HiddenField ID="dateWordHidden" runat="server" />

            <!-- 文言テーブルから取得した文言を保持するHidden -->
            <asp:HiddenField ID="SC3080215Word0001Hidden" runat="server" />

            <!-- 詳細画面の直接表示を一度でも行ったかどうかをチェックするHidden -->
            <asp:HiddenField ID="SC3080215FirstOpenHidden" runat="server" />

            <!-- アンケート選択の2度押し防止フラグHidden -->
            <asp:HiddenField ID="SC3080215SelectedFlgHidden" runat="server" />
        </div>
        <!-- PopOverForm メインコンテンツ End -->
    </div>                
</asp:Panel>
