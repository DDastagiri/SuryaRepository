<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080216.aspx
'─────────────────────────────────────
'機能： 顧客詳細(受注後工程フォロー)
'補足： 
'作成： 2014/02/13 TCS 森 受注後フォロー機能開発
'─────────────────────────────────────
-->

<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080216.ascx.vb" Inherits="Pages_SC3080216uc" %>

<link href="../Styles/SC3080216/SC3080216.css?20140930000000" rel="stylesheet" type="text/css" />

<script src="../Scripts/SC3080216/SC3080216.js?20141003004000" type="text/javascript"></script>

<%--活動内容エリア START--%>
<%--**************************************--%>
    <!-- 活動内容ユーザーコントロールの表示領域 -->
    <%@ Register src="SC3080218.ascx" tagname="SC3080218" tagprefix="uc1" %>
    <uc1:SC3080218 ID="Sc3080218Page" runat="server" />
        
<%--活動内容エリア END--%>
<%--**************************************--%>


<%--活動結果エリア START--%>
<%--**************************************--%>
<asp:Panel ID="useB2DPanel" runat="server">
    <div id="SC3080216_activity_list">
        <div class="SC3080216_title">
            <icrop:CustomLabel ID="SC3080216_TitleLabel" runat="server" UseEllipsis="true"/>
        </div>
        <div id="SC3080216_toggle1">
            <div id="SC3080216_ToDoToggle" class="SC3080216_left SC3080216_toggle_on">
                <a>
                    <icrop:CustomLabel ID="SC3080216_ToDoLabel" runat="server" UseEllipsis="False"/>
                </a>
            </div>
            <div id="SC3080216_AllToggle" class="SC3080216_right SC3080216_toggle_off">
                <a>
                    <icrop:CustomLabel ID="SC3080216_AllLabel" runat="server" UseEllipsis="False"/>
                </a>
            </div>
        </div>
        <div id="SC3080216_toggle2">
            <div id="SC3080216_TimeToggle" class="SC3080216_left SC3080216_toggle_on">
                <a>
                    <icrop:CustomLabel ID="SC3080216_TimeLabel" runat="server" UseEllipsis="False"/>
                </a>
            </div>
            <div id="SC3080216_ProcessToggle" class="SC3080216_right SC3080216_toggle_off">
                <a>
                    <icrop:CustomLabel ID="SC3080216_ProcessLabel" runat="server" UseEllipsis="False"/>
                </a>
            </div>
        </div>

        <div id="rightBox">
            <div id="act_Time" style="display:block">
                <!-- 日時別表示 -->
                <ul id="AfterActivityDaysMain">
                    <%'動的行生成 %>
                    <asp:Repeater ID="SC3080216_AfterActivityDaysRepeater" runat="server">
                        <ItemTemplate>
                            <li class="rightBoxTitle" runat="server" id="rightBoxTitle" style="" value="" >
                                <icrop:CustomLabel runat="server" ID ="rightBoxTitleText_Days" Text='<%# HttpUtility.HtmlEncode(Eval("DATEORTIME"))%>'></icrop:CustomLabel>
                                <asp:HiddenField runat="server" ID="SC3080216_Title_Flg_Days" Value="" />
                            </li>
                            <li class="rightBoxRow" runat="server" id="rightBoxRow" style="" >
                                <asp:HiddenField runat="server" ID="SC3080216_Start_Dateortime" Value='<%# HttpUtility.HtmlEncode(Eval("START_DATEORTIME"))%>' />
                                <asp:HiddenField runat="server" ID="SC3080216_End_Dateortime" Value='<%# HttpUtility.HtmlEncode(Eval("END_DATEORTIME"))%>' />
                                <div class="rightBoxRowRight">
                                    <icrop:CustomLabel runat="server" ID ="After_Odr_Act_Name_Days" class="off" Text='<%# HttpUtility.HtmlEncode(Eval("AFTER_ODR_ACT_NAME"))%>'></icrop:CustomLabel>
                                </div>
                                <div runat="server" id="CheckBorderAreaDays" style="" class="icon1">
                                    <div runat="server" id="CheckBorderImageAreaDays" style="" class="icon1_rightbottom">
                                        <div class=""></div>
                                        <asp:HiddenField runat="server" ID="SC3080216_Act_Comp_Days" Value='<%# HttpUtility.HtmlEncode(Eval("COMPLETION_FLG"))%>' />
                                        <asp:HiddenField runat="server" ID="SC3080216_save_flg_days" Value='<%# HttpUtility.HtmlEncode(Eval("COMPLETION_FLG"))%>'/>
                                        <asp:HiddenField runat="server" ID="SC3080216_After_Act_Code_days" Value='<%# HttpUtility.HtmlEncode(Eval("AFTER_ODR_ACT_ID"))%>' />
                                        <asp:HiddenField runat="server" ID="SC3080216AfterActNoCheckDays" Value='0' />
                                        <asp:HiddenField runat="server" ID="SC3080216AfterActCheckMarkFlgDays" Value='0' />
                                    </div>
                                </div>
                                <div class="icon2">
                                    <asp:HiddenField runat="server" ID="SC3080216ContactIconDays" Value='<%# Eval("ICON_PATH_CONTACT_MTD")%>' />
                                </div>
                                <div class="icon3">
                                    <asp:HiddenField runat="server" ID="SC3080216PrcsIconDays" Value='<%# Eval("ICON_PATH_AFTER_ODR_PRCS_CD")%>' />
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div id="act_Process" style="display:none">
                <!-- 工程別表示 -->
                <ul id="AfterActivityPrcsMain">
                    <%' 動的列生成 %>
                    <asp:Repeater ID="SC3080216_AfterActivityPrcsRepeater" runat="server">
                        <ItemTemplate>
                            <li class="rightBoxTitle ellipsis" runat="server" id="rightBoxTitle" style="" value="">
                                <icrop:CustomLabel runat="server" ID ="rightBoxTitleText_Prcs" class="rightBoxTitleText_Prcs ellipsis" Text='<%# HttpUtility.HtmlEncode(Eval("AFTER_ODR_PRCS_NAME"))%>'></icrop:CustomLabel>
                                <asp:HiddenField runat="server" ID="SC3080216_Title_Flg_Prcs" Value="" />
                            </li>
                            <li class="rightBoxRow" runat="server" id="rightBoxRow" style="" >
                                <div class="rightBoxRowRight">
                                    <icrop:CustomLabel runat="server" ID ="After_Odr_Act_Name_Prcs" class="off" Text='<%# HttpUtility.HtmlEncode(Eval("AFTER_ODR_ACT_NAME"))%>'></icrop:CustomLabel>
                                </div>
                                <div class="dateBox1"><%# Eval("DATEORTIME")%></div>
                                <div runat="server" id="CheckBorderAreaPrcs" style="" class="icon1">
                                    <div runat="server" id="CheckBorderImageAreaPrcs" style="" class="icon1_rightbottom">
                                        <div class=""></div>
                                        <asp:HiddenField runat="server" ID="SC3080216_Act_Comp_Prcs" value='<%# HttpUtility.HtmlEncode(Eval("COMPLETION_FLG"))%>' />
                                        <asp:HiddenField runat="server" ID="SC3080216_save_flg_prcs" Value='<%# HttpUtility.HtmlEncode(Eval("COMPLETION_FLG"))%>' />
                                        <asp:HiddenField runat="server" ID="SC3080216_After_Act_Code_prcs" Value='<%# HttpUtility.HtmlEncode(Eval("AFTER_ODR_ACT_ID"))%>' />
                                        <asp:HiddenField runat="server" ID="SC3080216AfterActNoCheckPrcs" Value='0' />
                                        <asp:HiddenField runat="server" ID="SC3080216AfterActCheckMarkFlgPrcs" Value='0' />
                                    </div>
                                </div>
                                <div class="icon2">
                                    <asp:HiddenField runat="server" ID="SC3080216ContactIconPrcs" Value='<%# Eval("ICON_PATH_CONTACT_MTD")%>' />
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="HeaderTitleDays" />
        <asp:HiddenField runat="server" ID="HeaderTitlePrcs" />
        <asp:HiddenField runat="server" ID="display_none_flg" />
        <asp:HiddenField runat="server" ID="UpdAfterActCdList" />
        <asp:HiddenField runat="server" ID="UpdAfterActCompFlgList" />
        <asp:HiddenField runat="server" ID="ActCheckOffMsg" />
        <asp:HiddenField runat="server" ID="ActContractCode" />
    </div>
</asp:Panel>

<!-- Hidden項目 -->    
<% '対応SC %>
<asp:HiddenField ID="SC308216selectStaff" runat="server" Value="0" />

<% '活動方法 %>
<asp:HiddenField ID="SC308216selectActContact" runat="server" Value="0" />

<% 'プロセス有無 %>
<asp:HiddenField ID="SC308216processFlg" runat="server" Value="" />
                    
<% 'Follow-upBox用SeqNo %>
<asp:HiddenField ID="SC308216fllwSeq" runat="server" Value="" />

<% 'Follow-upBox用店舗コード %>
<asp:HiddenField ID="SC308216fllwstrcd" runat="server" Value="" />
            
<% '選択車種 %>
<asp:HiddenField ID="SC308216Vclseq" runat="server" Value="" />
            
<% '来店人数 %>
<asp:HiddenField ID="SC308216walkinNum" runat="server" Value="" />
            
<% '新規活動フラグ %>
<asp:HiddenField ID="SC308216newFllwFlg" runat="server" Value="" />

<% '活動日時 %>
<icrop:DateTimeSelector ID="SC308216ActTimeFromSelectorWK" runat="server" Format="DateTime" ForeColor="#375388" style="display:none;"/>
<icrop:DateTimeSelector ID="SC308216ActTimeToSelectorWK" runat="server" Format="Time" ForeColor="#375388"  style="display:none;"/>

<% '顧客区分　自社客：1　未取引客：2 %>
<asp:HiddenField ID="SC308216cstkind" runat="server" Value="" />

<% '顧客ID　自社客：自社客連番　未取引客：未取引客ユーザID%>
<asp:HiddenField ID="SC308216insdid" runat="server" Value="" />

<asp:HiddenField ID="SC3080216UpdateRWFlg" runat="server" Value="0" />

<%--**************************************--%>
<%--活動結果エリア END--%>
