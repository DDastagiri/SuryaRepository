<%--
''' <summary>
''' ToDo一覧
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' <para>作成： 2012/02/01 TCS 竹内</para>
''' <para>更新： 2012/03/13 TCS 渡邊 $01 SalesStep2ユーザーテスト課題No.15、18、36</para>
''' <para>更新： 2012/05/29 TCS 神本 クルクル対応</para>
''' <para>更新： 2013/01/11 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発</para>
''' <para>更新： 2014/02/17 TCS 山田 受注後フォロー機能開発</para>
''' <para>更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発</para>
''' <para>更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3</para>
''' <para>更新： 2019/05/28 TS 髙橋(龍) 画像形式（拡張子）変更対応(TR-SVT-TMT-20170725-001)
''' <para>更新： 2020/02/17 TS 髙橋(龍) iOS13.3不具合対応(TR-SLT-TMT-20200218-001)
''' </history>
--%>
<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3010401.aspx.vb" Inherits="SC3010401" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3010401/SC3010401.css" />
	<script type="text/javascript" src="../Scripts/jquery.CustomCheckBox.js"></script>
	<script type="text/javascript" src="../Scripts/SC3010401/jquery.touchSwipe-1.2.5.js"></script>
    <script type="text/javascript" src="../Scripts/SC3010401/SC3010401.js?20200217000001"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

<asp:Button ID="sortButton" runat="server" Text="ソート処理" CssClass="disableButton" />
<asp:Button ID="nextButton" runat="server" Text="商談画面へ遷移する" CssClass="disableButton" />
<!--　2012/05/29 TCS 神本 クルクル対応 START　-->
<asp:Button ID="refreshButton" runat="server" Text="画面再表示" CssClass="disableButton" />
<!--　2012/05/29 TCS 神本 クルクル対応 END　-->
	<div id="BaseBox"><!--　←サイズ確認用のタグです　-->
	    <div id="container"><!--　←全体を含むタグです。　-->
		    <!-- 中央部分-->
		    <div id="main"> <!-- ここからコンテンツ -->
    			<div id="contents">
				    <div id="TcvNsc05-01Main">
		                <div id="SetIcons" class="SetIconsHeightS">
                            <span class="Total"><icrop:CustomLabel ID="goukeiLabel" runat="server" TextWordNo="0" Width="300px"/></span>
                            <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
　                          <div id="ToDoSearchType">
                                <icrop:SegmentedButton ID="ToDoSegmentedButton" runat="server" OnClientSelect="ToDoSearchTypeSegmenteButton_select"></icrop:SegmentedButton>

                                <div style="display:none;">
                                    <input ID="FocusinDummyButton" Text="focusin" OnClick="FocusInToDoSearchTextBox('dummy');"/>
                                </div>
                            </div>

                            <div id="ToDoSearchArea">
                                <div id="ToDoSearchTextBoxArea" style="display:inline-block;"> 
                                    <icrop:CustomTextBox type="search" ID="ToDoSearchTextBox" runat="server" onkeydown="InputInToDoSearchTextBox();" onfocus="FocusInToDoSearchTextBox('TextBox');"/>
                                </div>
                            </div>
                            <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                            <div id="AddIconRight" style="z-index:20;">
                                <span><img src="../styles/images/SC3010401/nsc05LoupeIcon.png" alt="Start"/></span>
        				    </div>
                            <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                            <!--  2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 START　-->
                            <ul id="CheckBoxArea" class="SetIconsList" style="display:none;">
                            <!--  2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 END　-->
                                <% '$01 Modify Start
                                    '<li class="YCount0 XCount0 bgTypeOn">
                            	     %>
                            	<li class="YCount1 XCount0">
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkDelay" type="checkbox" class="dateCriteria" style="margin-right:5px;vertical-align:middle;" runat="server"/>
								</div>
                                    <icrop:CustomLabel ID="CustomLabelDelay" runat="server" TextWordNo="3" Text="Delay" ForeColor="White" CssClass="AddText"/>
								</li>
                                <% '$01 Modify Start
                                    '<li class="YCount0 XCount1 bgTypeOn">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                	            <li class="YCount1 XCount1">
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkDue" type="checkbox" class="dateCriteria" style="margin-right:5px;vertical-align:middle;" runat="server"/>
								</div>
                                    <icrop:CustomLabel ID="CustomLabelDue" runat="server" TextWordNo="4" Text="Today" ForeColor="White" CssClass="AddText"/>
<!--								<span class="AddText">今日</span> -->
								</li>
                                <% '$01 Modify Start
                                    '<li class="YCount0 XCount2 bgTypeOff">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                            	<li class="YCount1 XCount2">
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkFuture" type="checkbox" class="dateCriteria" style="margin-right:5px;vertical-align:middle;" runat="server"/>
								</div>
                                    <icrop:CustomLabel ID="CustomLabelFuture" runat="server" TextWordNo="5" Text="Today" ForeColor="White" CssClass="AddText"/>
<!--								<span class="AddText">未来</span> -->
								</li>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                            	<li class="YCount1 XCount3">
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="CheckAllBefore" type="checkbox" class="AllBeforeCriteria" style="margin-right:5px;vertical-align:middle;" runat="server"/>
								</div>
                                    <icrop:CustomLabel ID="CustomLabelAllBefore" runat="server" TextWordNo="18" Text="P2B" ForeColor="White" CssClass="AddText"/>
								</li>
                                <!-- 2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START -->
                                <asp:Panel ID="b2dPanel" runat="server">
                                    <li class="YCount1 XCount4">
                                        <div class="icrop-CustomCheckBox" style="float: left; padding: 5px; border-radius: 8px;">
                                            <input id="CheckAllAfter" type="checkbox" class="AllAfterCriteria" style="margin-right: 5px;
                                                vertical-align: middle;" runat="server" />
                                        </div>
                                        <icrop:CustomLabel ID="CustomLabelAllAfter" runat="server" TextWordNo="19" Text="B2D"
                                            ForeColor="White" CssClass="AddText" />
                                    </li>
                                </asp:Panel>
                                <!-- 2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END -->
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                                <% '$01 Modify Start
                                    '<li class="YCount1 XCount0 bgTypeOn">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                	            <li class="YCount2 XCount0">
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkCold" type="checkbox" class="BeforeCriteria" style="margin-right:5px;vertical-align:middle;" runat="server" />
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
								</div>
								<span class="AddIcon"><img src="../styles/images/SC3010401/nsc05SetIcons01.png" width="45" height="26" alt="Cold"/></span>
								</li>
                                <% '$01 Modify Start
                                    '<li class="YCount1 XCount1 bgTypeOn">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                	            <li class="YCount2 XCount1">
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkWarm" type="checkbox" class="BeforeCriteria" style="margin-right:5px;vertical-align:middle;" runat="server" />
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
								</div>
								<span class="AddIcon"><img src="../styles/images/SC3010401/nsc05SetIcons02.png" width="45" height="26" alt="Prospect"/></span>
								</li>
                                <% '$01 Modify Start
                                    '<li class="YCount1 XCount2 bgTypeOn">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
            	                <li class="YCount2 XCount2">
                                <% '$01 Modify End %>
								<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									<input id="checkHot" type="checkbox" class="BeforeCriteria" style="margin-right:5px;vertical-align:middle;" runat="server" />
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
								</div>
								<span class="AddIcon"><img src="../styles/images/SC3010401/nsc05SetIcons03.png" width="45" height="26" alt="Hot"/></span>
								</li>
                                <% '$01 Modify Start
                                    '<li class="YCount1 XCount3 bgTypeOn">
                            	     %>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
                                <asp:Repeater ID="customCheckBoxRepeater" runat="server" ClientIDMode="Predictable">
                                    <ItemTemplate>
                                        <li class='YCount<%# DataBinder.Eval(Container.DataItem, "YCOUNT")%> XCount<%# DataBinder.Eval(Container.DataItem, "XCOUNT")%>'>
						            		<div class="icrop-CustomCheckBox" style="float:left; padding:5px; border-radius:8px;" >
									            <input id="checkAfter" type="checkbox" class="AfterCriteria" name="AfterOdrProc" style="margin-right:5px;vertical-align:middle;" runat="server" />
            								</div>
			            					<span class="AddIcon"><img src='<%# DataBinder.Eval(Container.DataItem, "ICON_PATH")%>' width="45" height="26" alt="AfterOdrProc"/></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->
                            </ul>
              	            <div class="WindDown">&nbsp;</div>
          	                <div class="WindUp">&nbsp;</div>
                        </div>

                        <asp:Panel ID="resultListPanel" runat="server">

                            <table border="0" cellpadding="0" cellspacing="0" class="ncs5002TitleTable">
                                <tr class='ncs5002TitleTableTr'>
    	    	     				<th class="column1 tableHeader1" align="center" valign="middle">
                                        <span>
                                            <icrop:CustomLabel ID="CustomLabelName" runat="server" TextWordNo="6" Text="Customer/Todo" ForeColor="White"/>
                                        </span>
                                    </th>
            		    	       	<th class="column2 tableHeader2" align="center" valign="middle">
                                        <span>        
                                            <a href="#" class="Button2">
                                                <icrop:CustomLabel ID="CustomLabelCar" runat="server" TextWordNo="7" Text="CarName" ForeColor="White" Font-Underline="True" />
                                            </a>
                                        </span>
                                    </th>
        				            <th class="column3 tableHeader3" align="center" valign="middle">
                                        <span>
                                            <a href="#" class="Button3">
                                                <icrop:CustomLabel ID="CustomLabelStatus" runat="server" TextWordNo="8" Text="Status" ForeColor="White" Font-Underline="True" />
                                            </a>
                                        </span>
                                    </th>
        	    		    		<th class="column4 tableHeader4" align="center" valign="middle">
                                        <span>
                                            <a href="#" class="Button4">
                                                <icrop:CustomLabel ID="CustomLabelCRACT" runat="server" TextWordNo="9" Text="NextDate" ForeColor="White" Font-Underline="True" />
                                            </a>
                                        </span>
                                    </th>
                                </tr>
		    		        </table>
                                
                            <icrop:CustomRepeater ID="customerRepeater" runat="server" 
                                    OnClientRender="customerRepeater_Render" 
									OnClientLoadCallbackResponse="customRepeater1_LoadCallbackResponse"
									Width="951px" Height="490px" 
                                    PageRows="50" maxCacheRows="100"/>
                            <script type="text/javascript">
                                function customRepeater1_LoadCallbackResponse(result) {
                                    $("#goukeiLabel").text(result.totalCountMessage);

                                    if (result.totalCount == 0) {
                                        alert(result.message);
                                    }

                                    sc3010401Script.closeCriteria();
                                }
                                function customerRepeater_Render(row, view) {

                                    var str = "";
                                    var strColor = "";

                                    var columeId = "divTable" + row.NO;
                                    //                                var nameId = "dataUserName_" + row.NO;

                                    //2013/01/11 TCS 橋本 【A.STEP2】Mod Start
                                    /*
                                    //偶数／奇数行によって背景色を変える
                                    if (row.flg == 0) {
                                        strColor = "ColorWhite";
                                    } else {
                                        strColor = "ColorGray";
                                    }
                                    */

                                    //偶数/奇数行/完了行によって背景色を変える
                                    strColor = row.BACKGROUNDCOLOR;
                                    //2013/01/11 TCS 橋本 【A.STEP2】Mod End

                                    //商談詳細呼び出しパラメーター作成

                                    var prm = ""
                                    prm = prm + "'" + row.CSTKIND + "',";
                                    prm = prm + "'" + row.CUSTOMERCLASS + "',";
                                    prm = prm + "'" + row.CRCUSTID + "',";
                                    prm = prm + "'" + row.FOLLOWUPBOX + "',";
                                    prm = prm + "'" + row.FLLWUPBOXSTRCD + "',";
                                    prm = prm + "'" + columeId + "'";

                                    //HTML作成
                                    //elementParent = $("<div id=" + columeId + " class='ncs5002TitleTableBox " + strColor + "' ></div> ");
                                    //elementParent = $("<div id=" + columeId + " class='ncs5001TitleTableBox " + strColor + "' onclick=selectCoustomer(" + prm + ")></div> ");
                                    elementParent = $("<div id=\"" + columeId + "\" class=\"ncs5002TitleTableBox " + strColor + "\" onclick=\"sc3010401Script.selectCustomer(" + prm + ")\"></div> ");

                                    //1カラム目 (ToDo名称 イメージ/苦情/顧客区分/顧客タイプ)
                                    //                                elementColumn1 = $("<div class='leftDiv column1 tableHeader1" + strColor + "' > </div>");
                                    elementColumn1 = $("<td align='left' valign='top' class='column11 tableHeader11 " + strColor + "' >");
                                    //2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 START
                                    //str = "<span class='dataPortraits'><img src='" + row.IMAGEPATH + "' width='60' height='61' alt='人物写真'></span> "
                                    str = "<span class='dataPortraits'><img src='" + row.IMAGEPATH + "' width='60' height='60' alt='人物写真'></span> "
                                    //2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 END
                                        + "<span class='dataActivity'>" + sc3010401Script.escapeHTML(row.TODONAME) + "</span> "
                                    if (row.CLM == "") {
                                        str = str + "	";
                                    } else {
                                        str = str + "	<span class='dataSettingIcon1On'><span class='dataSettingIcon1OnSpan'>" + row.CLM + "</span></span> ";
                                    }
                                    str = str + "<span class='dataSettingIcon2'><span class='dataSettingIcon2Span'>" + row.KINDNM + "</span></span> "
                                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                                    str += isNaN(parseInt(row.joinType, 10))
                                        ? ''
                                        : '<span class="dataSettingIcon3"><span class="dataSettingIcon3Span">' + (['', 'I', 'C'][row.joinType]) + '</span></span>';
                                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
                                    //2014/02/17 TCS 山田 受注後フォロー機能開発 START
                                    str = str + "	<span class='dataContactName'>" + sc3010401Script.escapeHTML(row.CONTACTNAME) + "</span> ";
                                    //2014/02/17 TCS 山田 受注後フォロー機能開発 END
                                    elementCustInfo = $(str);
                                    elementCustInfo.CustomLabel({ 'useEllipsis': 'true' });
                                    elementColumn1.append(elementCustInfo);

                                    //2カラム目 (車両名称/モデル名称)
                                    elementColumn2 = $("<td align='left' valign='top' class='column22 tableHeader22 " + strColor + "' ></td> ");
                                    elementMobile = $("<span class='telMobile'>" + sc3010401Script.escapeHTML(row.SERIESNM) + "</span> ");
                                    elementLine1 = $("<span class='SplitLine1'>&nbsp;</span> ");
                                    elementTelHome = $("<span class='telHome'>" + sc3010401Script.escapeHTML(row.MODELNM) + "</span>");
                                    elementMobile.CustomLabel({ 'useEllipsis': 'true' });
                                    elementColumn2.append(elementMobile);
                                    elementColumn2.append(elementLine1);
                                    elementTelHome.CustomLabel({ 'useEllipsis': 'true' });
                                    elementColumn2.append(elementTelHome);

                                    //3カラム目 (ステイタス)
                                    elementColumn3 = $("<td class='column33 tableHeader33 " + strColor + "' align='center' valign='middle'></td>");
                                    elementSsName = $("<span class='icon1'><img class='tableHeader33Img' src='" + row.STATUSICO + "' width='46' height='46'></span> ");
                                    elementSsName.CustomLabel({ 'useEllipsis': 'true' });
                                    elementColumn3.append(elementSsName);

                                    //4カラム目 (次回活動日)                                
                                    elementColumn4 = $("<td class='column44 tableHeader44 " + strColor + "' align='left' valign='top'></td>");
                                    if (row.PASTFLG == "1") {
                                        elementSaName = $("<div class=' NextDaySet'><span class='AddNextIcon'><img class='tableHeader44Img' src='" + row.CONTACTICO + "' width='35' height='35'></span><span class='AddNextDate colorRed'>" + row.CONTACTDATE + "</span> ");
                                    } else {
                                        elementSaName = $("<div class=' NextDaySet'><span class='AddNextIcon'><img class='tableHeader44Img' src='" + row.CONTACTICO + "' width='35' height='35'></span><span class='AddNextDate'>" + row.CONTACTDATE + "</span> ");
                                    }
                                    elementColumn4.append(elementSaName);


                                    elementParent.append(elementColumn1);
                                    elementParent.append(elementColumn2);
                                    elementParent.append(elementColumn3);
                                    elementParent.append(elementColumn4);

                                    view.append(elementParent);

                                    //最後の行まで表示時は、次の25件を表示を消す
                                    if (row.NO == row.maxrow) {
                                        str = str + "<script type='text/javascript'>";
                                        str = str + "    $(function () {";
                                        str = str + "        $('.icrop-CustomRepeater-pager').css('display','none');";
                                        str = str + "    });"
                                        str = str + "</" + "script>";
                                    }

                                    //最初の行まで表示時は、次の25件を表示を表示する
                                    if (row.NO == 0) {
                                        str = str + "<script type='text/javascript'>";
                                        str = str + "    $(function () {";
                                        str = str + "        $('.icrop-CustomRepeater-inner-bottomPager').css('display','block');";
                                        str = str + "    });"
                                        str = str + "</" + "script>";
                                    }

                                }
                            </script> 
                        </asp:Panel>
		            </div><!-- ここまでコンテンツ --> 
                </div><!-- 全体を含む -->
	        </div>
		</div><!--　←全体を含むタグ終わり　-->
	</div><!-- サイズ確認用 -->

  	<asp:HiddenField ID="cstkindHidden" runat="server" />
	<asp:HiddenField ID="customerclassHidden" runat="server" />    
	<asp:HiddenField ID="crcustidHidden" runat="server" />
	<asp:HiddenField ID="fllwupboxseqHidden" runat="server" />
	<asp:HiddenField ID="strcdHidden" runat="server" />

    <asp:HiddenField ID="sortTypeHidden" runat="server" />
	<asp:HiddenField ID="sortOrderHidden" runat="server" />

   	<asp:HiddenField ID="nextMessageHidden" runat="server" />
	<asp:HiddenField ID="nextLastMessageHidden" runat="server" />
    
	<asp:HiddenField ID="forwordMessageHidden" runat="server" />
	<asp:HiddenField ID="forwordFirstMessageHidden" runat="server" />

    <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 START　-->
	<asp:HiddenField ID="ToDoSearchTypeWordNameHidden" runat="server" />
	<asp:HiddenField ID="ToDoSearchTypeWordVinHidden" runat="server" />
	<asp:HiddenField ID="ToDoSearchTypeWordTelHidden" runat="server" />
	<asp:HiddenField ID="ToDoSearchTypeWordBookingNoHidden" runat="server" />
	<asp:HiddenField ID="ToDoSearchTypeWordSocialIDHidden" runat="server" />
    <!--　2014/02/17 TCS 山田 受注後フォロー機能開発 END　-->

    <%'サーバー処理中のオーバーレイとアイコン %>
    <div id="serverProcessOverlayBlack"></div>
    <div id="serverProcessIcon"></div>
    
    <% '$01 Delete Start %>
    <!-- ここからフッタ -->
    <!-- 
	<div id="footer"><img src="../styles/images/footerimg.gif" width="1024" height="49" alt="footer"/></div>
	 -->
    <!-- ここまでフッタ -->
    <% '$01 Delete End %>
</asp:Content>