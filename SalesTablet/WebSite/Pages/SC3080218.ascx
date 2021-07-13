<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080218.aspx
'─────────────────────────────────────
'機能： 顧客詳細(活動内容)
'補足： 
'作成： 2012/02/08 TCS 安田
'更新： 2012/04/26 TCS 河原 HTMLエンコード対応
'更新： 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/10/04 TCS 市川  次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
'─────────────────────────────────────
-->

<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080218.ascx.vb" Inherits="Pages_SC3080218" %>

<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
<%--<link href="../Styles/SC3080218/SC3080218.css?20120312041000" rel="stylesheet" type="text/css" />

<script src="../Scripts/SC3080218/SC3080218.js?20120806001000" type="text/javascript"></script>--%>
<link href="../Styles/SC3080218/SC3080218.css?20131006000000" rel="stylesheet" type="text/css" />

<script src="../Scripts/SC3080218/SC3080218.js?20131003002000" type="text/javascript"></script>
<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>

<div id="confirmContents218">

    <div id="scNscOneBoxContentsArea" class="contentsFrame BoxtypeNSC61A">
        <h2 class="contentTitle">
        <icrop:CustomLabel id="Sc3080218Label1" runat="server" TextWordNo="30301" Text="活動内容" UseEllipsis="False" width="170px" CssClass="clip" />
        </h2>
	    
        <div class="nscListBoxSetLeft">
            <ul class="Activities01">
                <li>
                    <div class="Line01">&nbsp;</div>
                    <div class="Title"><icrop:CustomLabel id="Sc3080218Label2" runat="server" TextWordNo="30302" Text="日付" UseEllipsis="False" width="80px" CssClass="clip"/></div>
                    <div class="ActTime Data Arrow" id="Sc3080218ActTimePopupTrigger"></div>
                    <icrop:PopOver ID="Sc3080218ActTimePopOver" runat="server" TriggerClientID="Sc3080218ActTimePopupTrigger" Width="200px" Height="200px" HeaderStyle="None">
                        <div id="scNscActTimeWindown6003">
                            <div id="scNscActTimeWindownBox">
                                <div class="scNscActTimeHadder">
                                    <h3>
                                        <icrop:CustomLabel ID="Sc3080218Label36" runat="server" TextWordNo="30326" Text="活動日" UseEllipsis="False" width="130px" CssClass="clip"/>
                                    </h3>
                                    <div class="scNscActTimeCancellButton">
                                        <a class="Square" href="#">
                                            <icrop:CustomLabel ID="Sc3080218Label20" runat="server" TextWordNo="30324" Text="キャンセル" UseEllipsis="False" width="70px" CssClass="clip"/>
                                        </a>
                                    </div>
                                    <a href="javascript:void(0)" class="scNscActTimeCompletionButton ">
                                        <icrop:CustomLabel ID="Sc3080218Label30" runat="server" TextWordNo="30325" Text="完了" UseEllipsis="False" width="70px" CssClass="clip"/>
                                    </a>
                                </div>

                                <div class="scNscActTimeListArea">
                                    <asp:UpdatePanel runat="server" ID="Sc3080218ActTimeUpdatePanel" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="Sc3080218ActTimePanel" runat="server" Visible="false">
                                                <div class="scNscActTimeListBox">
                                                    <div class="scNscActTimeListItemBox">
                                                        <div class="scNscActTimeListItem5">
                                                            <dl class="nscListBoxSetIn">
                                                                <dt><icrop:CustomLabel ID="Sc3080218Label21" runat="server" TextWordNo="30320" Text="開始" UseEllipsis="False" width="70px" CssClass="clip"/></dt>
                                                                <dd style="height:16px;"><icrop:DateTimeSelector ID="Sc3080218ActTimeFromSelector" runat="server" PlaceHolderWordNo="0" Format="DateTime" ForeColor="#375388" /></dd>
                                                                <dt class="BlueBack end"><icrop:CustomLabel ID="Sc3080218Label25" runat="server" TextWordNo="30321" Text="終了" UseEllipsis="False" width="70px" CssClass="clip"/></dt>
                                                                <dd style="height:16px;" class="BlueBack end"><icrop:DateTimeSelector ID="Sc3080218ActTimeToSelector" runat="server" PlaceHolderWordNo="0" Format="Time" ForeColor="#375388" /></dd>
                                                                <div class="clearboth">&nbsp;</div>
                                                            </dl>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                        </div>
                    </icrop:PopOver>
                </li>
                <li>
                    <div class="Line01">&nbsp;</div>
                    <div class="Title"><icrop:CustomLabel id="Sc3080218Label3" runat="server" TextWordNo="30303" Text="担当SC" UseEllipsis="False" width="80px" CssClass="clip"/></div>
                    <div class="scNscStaffName Data Arrow" id="Sc3080218UsersTrigger"></div>
                    <icrop:PopOver ID="Sc3080218PopOver8" runat="server" TriggerClientID="Sc3080218UsersTrigger" Width="200px" Height="200px" HeaderStyle="None">
                        <div id="scNscStaffWindown">
                            <div id="scNscStaffWindownBox">
                                <div class="scNscStaffHadder">
                                    <h3><icrop:CustomLabel ID="Sc3080218Label39" runat="server" TextWordNo="30329" Text="対応SC" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
                                </div>
                                <div class="scNscStaffListArea">
                                    <div class="scNscStaffListBox">
                                        <div class="scNscStaffListItemBox">
                                            <div class="scNscStaffListItem5">
                                                <asp:UpdatePanel runat="server" ID="Sc3080218StaffListUpdatePanel" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="Sc3080218StaffListPanel" runat="server" Visible="false">
                                                            <ul class="nscListBoxSetIn">
                                                                <asp:Repeater ID="Sc3080218StaffListRepeater" runat="server" DataSourceID ="UsersDataSource" ClientIDMode="Predictable">
                                                                    <ItemTemplate>
                                                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                        <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "USERNAME")) %>" id="Sc3080218Stafflist<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ACCOUNT")) %>" class="Stafflist ellipsis" value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ACCOUNT")) %>">
                                                                            <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "USERNAME")) %><span value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ACCOUNT")) %>"></span>
                                                                        </li>
                                                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </icrop:PopOver>
                </li>
                <li>
                    <div class="Line01">&nbsp;</div>
                    <div class="Title"><icrop:CustomLabel ID="Sc3080218Label4" runat="server" TextWordNo="30304" Text="分類" UseEllipsis="False" width="80px" CssClass="clip"/></div>
                    <div class="scNscActContactName Data Arrow" id="Sc3080218ActContactTrigger"></div>
                    <icrop:PopOver ID="Sc3080218PopOver9" runat="server" TriggerClientID="Sc3080218ActContactTrigger" Width="200px" Height="200px">
                        <div id="scNscActContactWindown">
                            <div id="scNscActContactWindownBox">
                                <div class="scNscActContactHadder">
                                    <h3><icrop:CustomLabel ID="Sc3080218Label40" runat="server" TextWordNo="30330" Text="分類" UseEllipsis="False"/></h3>
                                </div>
                                <div class="scNscActContactListArea">
                                    <div class="scNscActContactListBox">
                                        <div class="scNscActContactListItemBox">
                                            <div class="scNscActContactListItem5">
                                                <asp:UpdatePanel ID="Sc3080218ActContactListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="Sc3080218ActContactListPanel" runat="server" Visible="false">
                                                            <ul class="nscListBoxSetIn">
                                                                <asp:Repeater ID="Sc3080218ActContactListRepeater" runat="server" DataSourceID ="ActContactDataSource" ClientIDMode="Predictable">
                                                                    <ItemTemplate>
                                                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                        <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CONTACT")) %>" id="Sc3080218ActContactlist<%# DataBinder.Eval(Container.DataItem, "CONTACTNO")%>" class="ActContactlist ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "CONTACTNO")%>">
                                                                            <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CONTACT")) %><span value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "PROCESS")) %>"></span>
                                                                        </li>
                                                                        <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </icrop:PopOver>
                </li>
            </ul>
        </div>


         <% 'プロセス %>
        <div class="nscListBoxSetRight2">
	        <dl style="white-space:nowrap">
	            <dt class="end nscListIcn">
                    <icrop:CustomLabel ID="Sc3080218Label5" runat="server" TextWordNo="30305" Text="プロセス" UseEllipsis="False" width="80px" CssClass="clip"/>
                </dt>
	            <dd class="end">
	                <div class="nscListIcnAset">
                        <% 'カタログ %>
	                    <div class="nscListIcnA1" id="Sc3080218popupTrigger4">
                            <asp:Label class="clip" id="Sc3080218CatalogWord" text="" runat="server" Width="60px" />
                        </div>
                        <icrop:PopOver ID="Sc3080218PopOver4" runat="server" TriggerClientID="Sc3080218popupTrigger4" Width="200px" Height="200px">
                            <div id="scNscCatalogWindown">
                                <div id="scNscCatalogWindownBox">
                                    <div class="scNscCatalogHadder">
                                        <h3><asp:Label class="clip" id="Sc3080218CatalogTitle" text="" runat="server" Width="140px" /></h3>
                                        <div class="scNscCatalogCancellButton">
                                            <a class="Square" href="#">
                                                <icrop:CustomLabel ID="Sc3080218Label22" runat="server" TextWordNo="30324" Text="キャンセル" UseEllipsis="False" width="70px" CssClass="clip"/>
                                            </a>
                                            <span class="tgLeft">&nbsp;</span>
                                        </div>
                                        <a href="#" class="scNscCatalogCompletionButton ">
                                            <icrop:CustomLabel ID="Sc3080218Label31" runat="server" TextWordNo="30325" Text="完了" UseEllipsis="False" width="70px" CssClass="clip"/>
                                        </a>
                                    </div>
                                    <div class="scNscCatalogListArea">
                                        <div class="scNscCatalogListBox">
                                            <div class="scNscCatalogListItemBox">
                                                <div class="scNscCatalogListItem5">
                                                    <asp:UpdatePanel ID="Sc3080218CatalogListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Sc3080218CatalogListPanel" runat="server" Visible="false">
                                                                <ul class="nscListBoxSetIn">
                                                                    <asp:Repeater ID="Sc3080218CatalogListRepeater" runat="server" DataSourceID ="FllwSeriesDataSource" ClientIDMode="Predictable" EnableViewState="false">
                                                                        <ItemTemplate>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                            <li id="Sc3080218Cataloglist<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>" class="Cataloglist" value="<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>">
                                                                                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SERIESNM")) %>
                                                                            </li>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </ul>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </icrop:PopOver>
                            
                        <% '試乗 %>
	                    <div class="nscListIcnA2" id="Sc3080218popupTrigger5">
                            <asp:Label class="clip" id="Sc3080218TestDriveWord" text="" runat="server" Width="60px"/>
                        </div>
                        <icrop:PopOver ID="Sc3080218PopOver5" runat="server" TriggerClientID="Sc3080218popupTrigger5" Width="200px" Height="200px">
                            <div id="scNscTestDriveWindown">
                                <div id="scNscTestDriveWindownBox">
                                    <div class="scNscTestDriveHadder">
                                        <h3><asp:Label class="clip" id="Sc3080218TestDriveTitle" text="" runat="server" Width="140px" /></h3>
                                        <div class="scNscTestDriveCancellButton">
                                            <a class="Square" href="#">
                                                <icrop:CustomLabel ID="Sc3080218Label23" runat="server" TextWordNo="30324" Text="キャンセル" UseEllipsis="False" width="70px" CssClass="clip"/>
                                            </a>
                                            <span class="tgLeft">&nbsp;</span>
                                        </div>
                                        <a href="javascript:void(0)" class="scNscTestDriveCompletionButton ">
                                            <icrop:CustomLabel ID="Sc3080218Label32" runat="server" TextWordNo="30325" Text="完了" UseEllipsis="False" width="70px" CssClass="clip"/>
                                        </a>
                                    </div>
                                    <div class="scNscTestDriveListArea">
                                        <div class="scNscTestDriveListBox">
                                            <div class="scNscTestDriveListItemBox">
                                                <div class="scNscTestDriveListItem5">
                                                    <asp:UpdatePanel ID="Sc3080218TestDriveListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Sc3080218TestDriveListPanel" runat="server" Visible="false">
                                                                <ul class="nscListBoxSetIn">
                                                                    <asp:Repeater ID="Sc3080218TestDriveListRepeater" runat="server" DataSourceID ="FllwModelDataSource" ClientIDMode="Predictable" EnableViewState="false">
                                                                        <ItemTemplate>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                            <li id="Sc3080218TestDrivelist<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>" class="TestDrivelist" value="<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>">
                                                                                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SERIESNM")) %> <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "VCLMODEL_NAME")) %>
                                                                            </li>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </ul>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </icrop:PopOver>

                        <% '査定 %>
	                    <div class="nscListIcnA3">
                            <asp:Label class="clip" id="Sc3080218AssesmentWord" text="" runat="server" Width="60px"/>
                        </div>

                        <% '見積 %>
	                    <div class="nscListIcnA4" id="Sc3080218popupTrigger6">
                            <asp:Label class="clip" id="Sc3080218ValuationWord" text="" runat="server" Width="60px"/>
                        </div>
                        <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
                        <%--<icrop:PopOver ID="Sc3080218PopOver6" runat="server" TriggerClientID="Sc3080218popupTrigger6" Width="200px" Height="200px">--%>
                        <icrop:PopOver ID="Sc3080218PopOver6" runat="server" TriggerClientID="Sc3080218popupTrigger6" Width="650px" Height="220px">
                            <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
                            <div id="scNscValuationWindown">
                                <div id="scNscValuationWindownBox">
                                    <div class="scNscValuationHadder">
                                        <h3><asp:Label class="clip" id="Sc3080218ValuationTitle" text="" runat="server" Width="140px" /></h3>
                                        <div class="scNscValuationCancellButton">
                                            <a class="Square" href="#">
                                                <icrop:CustomLabel ID="Sc3080218Label24" runat="server" TextWordNo="30324" Text="キャンセル" UseEllipsis="False" width="70px" CssClass="clip"/>
                                            </a>
                                            <span class="tgLeft">&nbsp;</span>
                                        </div>
                                        <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
<%--                                    <a href="javascript:void(0)" class="scNscValuationCompletionButton ">
                                            <icrop:CustomLabel ID="Sc3080218Label33" runat="server" TextWordNo="30325" Text="完了" UseEllipsis="False" width="70px" CssClass="clip"/>
                                        </a>--%>
                                        <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
                                    </div>
                                    <div class="scNscValuationListArea">
                                        <div class="scNscValuationListBox">
                                            <div class="scNscValuationListItemBox">
                                                <div class="scNscValuationListItem5">
                                                    <asp:UpdatePanel ID="Sc3080218ValuationListUpdatePanel" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Sc3080218ValuationListPanel" runat="server" Visible="false">
                                                                <ul class="nscListBoxSetIn">
                                                                    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
                                                                    <%--<asp:Repeater ID="Sc3080218ValuationListRepeater" runat="server" DataSourceID ="FllwColorDataSource" ClientIDMode="Predictable" EnableViewState="false">
                                                                        <ItemTemplate>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                            <li id="Sc3080218Valuationlist<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>" class="Valuationlist" value="<%# DataBinder.Eval(Container.DataItem, "SEQNO")%>">
                                                                                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SERIESNM")) %> <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "VCLMODEL_NAME")) %> <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DISP_BDY_COLOR")) %>
                                                                            </li>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 END%>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>--%>
                                                                    <asp:Repeater ID="Sc3080218ValuationListRepeater" runat="server" DataSourceID ="EstimateCarDataSource" ClientIDMode="Predictable" EnableViewState="false">
                                                                        <ItemTemplate>
                                                                            <li id="Sc3080218Valuationlist<%# DataBinder.Eval(Container.DataItem, "ESTIMATEID")%>" class="Valuationlist" value="<%# DataBinder.Eval(Container.DataItem, "ESTIMATEID")%>">                                                                               
                                                                                <div style="display:inline-block;width:400px">
                                                                                    <div class="ellipsis" style="display:inline-block;float:left;width:250px">
                                                                                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "SERIESNM")) %> <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "VCLMODEL_NAME")) %> <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DISP_BDY_COLOR")) %>
                                                                                    </div>
                                                                                    <div style="display:inline-block;float:right;text-align:right;width:150px">
                                                                                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DISPLAY_PRICE"))%>
                                                                                    </div>
                                                                                </div>
                                                                            </li>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>
                                                                </ul>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </icrop:PopOver>
	                    <div class="clearboth">&nbsp;</div>
	                </div>
	            </dd>
	            <div class="clearboth">&nbsp;</div>
	        </dl>
	    </div>
	    <div class="clearboth">&nbsp;</div>
	</div>
</div>

<% 'ダミーボタン %>
<asp:UpdatePanel runat="server" ID="Sc3080218ButtonUpdatePanel" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button runat="server" ID="Sc3080218ActTimeButton" style="display:none" />
        <asp:Button runat="server" ID="Sc3080218StaffListButton" style="display:none" />
        <asp:Button runat="server" ID="Sc3080218ActContactListButton" style="display:none" />
        <asp:Button runat="server" ID="Sc3080218CatalogListButton" style="display:none" />
        <asp:Button runat="server" ID="Sc3080218TestDriveListButton" style="display:none" />
        <asp:Button runat="server" ID="Sc3080218ValuationListButton" style="display:none" />
    </ContentTemplate>
</asp:UpdatePanel>

<% '読み込み完了フラグ %>
<asp:UpdatePanel runat="server" ID="Sc3080218PopupFlgUpdatePanel" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="Sc3080218ActTimePopupFlg" runat="server" Value="0" />
        <asp:HiddenField ID="Sc3080218StaffListPopupFlg" runat="server" Value="0" />
        <asp:HiddenField ID="Sc3080218ActContactListPopupFlg" runat="server" Value="0" />
        <asp:HiddenField ID="Sc3080218CatalogListPopupFlg" runat="server" Value="0" />
        <asp:HiddenField ID="Sc3080218TestDriveListPopupFlg" runat="server" Value="0" />
        <asp:HiddenField ID="Sc3080218ValuationListPopupFlg" runat="server" Value="0" />
    </ContentTemplate>
</asp:UpdatePanel>

<% 'ObjectDataSource Start %>
<% '対応SC %>
<asp:ObjectDataSource id="UsersDataSource" runat="server"  SelectMethod="GetUsers" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" />

<% '分類 %>
<asp:ObjectDataSource id="ActContactDataSource" runat="server"  SelectMethod="GetActContact" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218BookedFlg" Name="bookedafterflg" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>

<% 'カタログ %>
<asp:ObjectDataSource id="FllwSeriesDataSource" runat="server" SelectMethod="GetFllwSeries" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwstrcd" Name="FllwStrcd" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwSeq" Name="fllwupboxseqno" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>

<% '試乗 %>
<asp:ObjectDataSource id="FllwModelDataSource" runat="server"  SelectMethod="GetFllwModel" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwstrcd" Name="FllwStrcd" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwSeq" Name="fllwupboxseqno" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>   
                            
<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>                            
<%--<% '見積り %>
<asp:ObjectDataSource id="FllwColorDataSource" runat="server"  SelectMethod="GetFllwColor" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwstrcd" Name="FllwStrcd" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwSeq" Name="fllwupboxseqno" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>--%>

<asp:ObjectDataSource id="EstimateCarDataSource" runat="server"  SelectMethod="GetEstimateCar" TypeName="Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic" >
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwdlrcd" Name="dlrcd" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwstrcd" Name="strcd" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218fllwSeq" Name="fllwupboxseqno" PropertyName="Value" />
    </SelectParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="Sc3080218cntcd" Name="cntcd" PropertyName="Value" />
    </SelectParameters>
</asp:ObjectDataSource>
<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%> 

<% 'ObjectDataSource End %>

<% 'アイコンのパス %>
<asp:HiddenField ID="Sc3080218CatalogSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218CatalogNonSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218TestDriveSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218TestDriveNonSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218AssesmentSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218AssesmentNonSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218ValuationSelPath" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218ValuationNonSelPath" runat="server" Value="" />
    
<input type="hidden" name="Sc3080218HD_nscListIcnA1" value="0" />

<% '月日のデータフォーマット %>
<asp:HiddenField ID="Sc3080218dateFormt" runat="server" Value="" />

<% '活動日時 %>
<icrop:DateTimeSelector ID="Sc3080218ActTimeFromSelectorWK" runat="server" Format="DateTime" ForeColor="#375388" style="display:none;"/>
<icrop:DateTimeSelector ID="Sc3080218ActTimeFromSelectorWK2" runat="server" Format="DateTime" ForeColor="#375388" style="display:none;"/>
<icrop:DateTimeSelector ID="Sc3080218ActTimeToSelectorWK" runat="server" Format="Time" ForeColor="#375388" style="display:none;"/>
<icrop:DateTimeSelector ID="Sc3080218ActTimeToSelectorWK2" runat="server" Format="Time" ForeColor="#375388" style="display:none;"/>

<% '対応SC %>
<asp:HiddenField ID="Sc3080218selectStaff" runat="server" Value="0" />
<asp:HiddenField ID="Sc3080218selectStaffName" runat="server" Value="0" />

<% '活動方法 %>
<asp:HiddenField ID="Sc3080218selectActContact" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218selectActContactTitle" runat="server" Value="" />

<% 'Follow-upBox用SeqNo %>
<asp:HiddenField ID="Sc3080218fllwSeq" runat="server" Value="" />

<% 'Follow-upBox用店舗コード %>
<asp:HiddenField ID="Sc3080218fllwstrcd" runat="server" Value="" />

<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
<% 'Follow-upBox用販売店コード %>
<asp:HiddenField ID="Sc3080218fllwdlrcd" runat="server" Value="" />
<% '国コード %>
<asp:HiddenField ID="Sc3080218cntcd" runat="server" Value="" />
<%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>

<% '受注車両 %>
<asp:HiddenField ID="Sc3080218selectSelSeries" runat="server" Value="" />

<% 'エラーメッセージ %>
<asp:HiddenField ID="Sc3080218ErrWord1" runat="server" Value="" />
<asp:HiddenField ID="Sc3080218ErrWord2" runat="server" Value="" />

<% '外部から更新がかかる項目はUpdatePanel内に配置 %>
<asp:UpdatePanel ID="ParamUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <% 'プロセス有無 %>
        <asp:HiddenField ID="Sc3080218ProcessFlg" runat="server" Value="" />
        
        <% '受注後フラグ %>
        <asp:HiddenField ID="Sc3080218BookedFlg" runat="server" Value="" />

    </ContentTemplate>
</asp:UpdatePanel>

<% '外部から更新がかかる項目はUpdatePanel内に配置 %>
<asp:UpdatePanel ID="SC3080218HiddenFieldUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <% 'プロセス(カタログ) %>
        <asp:HiddenField ID="Sc3080218selectActCatalog" runat="server" Value="" />
        <asp:HiddenField ID="Sc3080218selectActCatalogWK" runat="server" Value="" />
        
        <% 'プロセス(試乗) %>
        <asp:HiddenField ID="Sc3080218selectActTestDrive" runat="server" Value="" />
        <asp:HiddenField ID="Sc3080218selectActTestDriveWK" runat="server" Value="" />
        
        <% 'プロセス(査定) %>
        <asp:HiddenField ID="Sc3080218selectActAssesment" runat="server" Value="" />
        <asp:HiddenField ID="Sc3080218selectActAssesmentWK" runat="server" Value="" />
        
        <% 'プロセス(見積り) %>
        <asp:HiddenField ID="Sc3080218selectActValuation" runat="server" Value="" />
        <asp:HiddenField ID="Sc3080218selectActValuationWK" runat="server" Value="" />

    </ContentTemplate>
</asp:UpdatePanel>

    <%-- 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START --%>
    <asp:HiddenField ID="DispPage3Flg" runat="server" Value="0" />
    <asp:HiddenField ID="FastDispTime" runat="server" Value="" />
    <asp:HiddenField ID="SC3080218UpdateRWFlg" runat="server" Value="0" />
    <%-- 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END --%>

    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START --%>
    <% '査定依頼機能フラグ %>
    <asp:HiddenField ID="SC3080218usedFlgAssess" runat="server" Value="" />
    <%-- 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END --%>