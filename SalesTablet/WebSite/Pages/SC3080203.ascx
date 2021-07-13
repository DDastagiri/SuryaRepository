<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203.ascx
'─────────────────────────────────────
'機能： 顧客詳細(活動登録)
'補足： 
'作成：            TCS 河原【SALES_1A】
'更新： 2012/02/29 TCS 河原【SALES_2】
'       2012/03/16 TCS 高橋【SALES_2】(Sales1A ユーザテスト No.219)
'       2012/03/20 TCS 相田【SALES_2】(TCS_0315ka_04 対応)
'       2012/04/26 TCS 河原 HTMLエンコード対応
'       2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
'       2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'       2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'       2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'       2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
'       2013/12/03 TCS 市川 Aカード情報相互連携開発
'       2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ)
'       2015/12/11 TCS 鈴木 受注後工程蓋閉め対応
'─────────────────────────────────────
-->

<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3080203.ascx.vb" Inherits="Pages_SC3080203" %>
<link href="../Styles/SC3080203/SC3080203.popup.css?201402040001" rel="stylesheet" type="text/css" />
<link href="../Styles/SC3080203/SC3080203.css?201310030000" rel="stylesheet" type="text/css" />
<script src="../Scripts/SC3080203/SC3080203.js?20140418000000" type="text/javascript"></script>
<script src="../Scripts/SC3080203/SC3080203.NextActivity.js?20140418000000" type="text/javascript"></script>

<div id="confirmContents60">

    <asp:ObjectDataSource id="NextActContactDataSource" runat="server"  SelectMethod="GetNextActContact" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic" />
    
    <asp:ObjectDataSource id="FollowContactDataSource" runat="server"  SelectMethod="GetFollowContact" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic" />

    <%'2013/06/30 TCS 趙 2013/10対応版 既存流用 START %>
    <asp:ObjectDataSource id="Competition_MakermasterDataSource" runat="server"  SelectMethod="GetNoCompetitionMakermaster" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic" />
    <%'2013/06/30 TCS 趙 2013/10対応版 既存流用 END %>
    
    <asp:ObjectDataSource id="CompetitorMasterObjectDataSource" runat="server"  SelectMethod="GetCompetitorMaster" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic" />
    <%'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START %>
    <asp:ObjectDataSource id="FllwSelectedDataSource" runat="server"  SelectMethod="GetPreferredCar" TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic" >
        <SelectParameters>
            <asp:ControlParameter ControlID="fllwstrcd" Name="FllwStrcd" PropertyName="Value" />
                </SelectParameters>
                <SelectParameters>
            <asp:ControlParameter ControlID="fllwSeq" Name="fllwupboxseqno" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>  
    <%'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END %>
    
    <!-- 活動内容ユーザーコントロールの表示領域 -->
    <%@ Register src="SC3080218.ascx" tagname="SC3080218" tagprefix="uc1" %>
    <uc1:SC3080218 ID="Sc3080218Page" runat="server" />
    
    <%--活動結果エリア START--%>
    <%--**************************************--%>
    <div class="nscListBoxSet">
        <h4><icrop:CustomLabel ID="CustomLabel19" runat="server" TextWordNo="30319" Text="活動結果" UseEllipsis="False" width="170px" CssClass="clip"/></h4>
        <div class="nscListBoxSetIn">
            <div class="nscListIcnBset">
            <%--2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD START --%>
            <asp:Panel runat="server" ID="ButtonColdPanel"><div class="nscListIcnB1"><icrop:CustomLabel ID="ColdLabel" runat="server" TextWordNo="30306" Text="Cold" UseEllipsis="False" width="90px" CssClass="clip"/></div></asp:Panel>
            <asp:Panel runat="server" ID="ButtonWarmPanel"><div class="nscListIcnB2"><icrop:CustomLabel ID="WarmLabel" runat="server" TextWordNo="30307" Text="Warm" UseEllipsis="False" width="90px" CssClass="clip"/></div></asp:Panel>
            <asp:Panel runat="server" ID="ButtonHotPanel"><div class="nscListIcnB3"><icrop:CustomLabel ID="HotLabel" runat="server" TextWordNo="30308" Text="Hot" UseEllipsis="False" width="90px" CssClass="clip"/></div></asp:Panel>
            <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 START--%>
            <asp:Panel runat="server" ID="pnlSuccessButton" CssClass="nscListIcnB4"><icrop:CustomLabel ID="SuccessLabel" runat="server" TextWordNo="30309" Text="受注" UseEllipsis="False" width="90px" CssClass="clip"/></asp:Panel>
            <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 END--%>
            <asp:Panel runat="server" ID="ButtonGiveUpPanel"><div class="nscListIcnB5"><icrop:CustomLabel ID="GiveupLabel" runat="server" TextWordNo="30310" Text="断念" UseEllipsis="False" width="90px" CssClass="clip"/></div></asp:Panel>
            <%--2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD END --%>
            <div class="clearboth">&nbsp;</div>
            </div>
        </div>
    </div>
    <%--**************************************--%>
    <%--活動結果エリア END--%>
    
    <%--次回活動エリア START--%>
    <%--**************************************--%>
    <!-- Walk-in、Prospect、Hot用 -->

    <asp:UpdatePanel runat="server" ID="ButtonUpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="NextActContactButton" style="display:none" />
            <asp:Button runat="server" ID="FollowContactButton" style="display:none" />
            <asp:Button runat="server" ID="GiveupReasonButton" style="display:none" />
            <asp:Button runat="server" ID="NextActTimeButton" style="display:none" />
            <asp:Button runat="server" ID="FollowTimeButton" style="display:none" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="nscListBoxSet HeightB" style="display:block">
        <h4><icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="30311" Text="次回活動" UseEllipsis="False" width="170px" CssClass="clip"/></h4>
        <div class="nscListBoxSetIn HeightB2">
            <div class="nscListBoxSetLeft" id="NextActBoxSet">
                <dl>
                    <dt class="padding2">
                        <icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="30312" Text="分類" UseEllipsis="False" width="80px" CssClass="clip"/>
                    </dt>
                    <dd class="Arrow padding2" id="NextActContactTrigger" >
                        <div class="scNscNextActContactName ellipsis" style="width:300px" ></div>
                        <icrop:PopOver ID="PopOver10" runat="server" TriggerClientID="NextActContactTrigger" Width="200px" Height="200px">
                            <div id="scNscNextActContactWindown">
                                <div id="scNscNextActContactWindownBox">
                                    <div class="scNscNextActContactHadder">
                                        <h3><icrop:CustomLabel ID="CustomLabel41" runat="server" TextWordNo="30387" Text="分類" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
                                    </div>
                                    <div class="scNscNextActContactListArea">
                                        <div class="scNscNextActContactListBox">
                                            <div class="scNscNextActContactListItemBox">
                                                <div class="scNscNextActContactListItem5">
                                                    <asp:UpdatePanel runat="server" ID="NextActContactUpdatePanel" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="NextActContactPanel" runat="server" Visible="false">
                                                                <ul class="nscListBoxSetIn">
                                                                    <asp:Repeater ID="NextActContactRepeater" runat="server" DataSourceID ="NextActContactDataSource" ClientIDMode="Predictable">
                                                                        <ItemTemplate>
                                                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                                                            <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CONTACT")) %>" id="NextActContactlist<%# DataBinder.Eval(Container.DataItem, "CONTACTNO")%>" class="NextActContactlist ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "CONTACTNO")%>">
                                                                                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CONTACT")) %><span value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "NEXTACTIVITY") & "_" & DataBinder.Eval(Container.DataItem, "FROMTO")) %>"></span>
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
                    </dd>

                    <dt class="end padding2 nextAct">
                        <icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="30313" Text="日付" UseEllipsis="False" width="80px" CssClass="clip"/>
                    </dt>
                    <dd class="end Arrow padding2 nextAct" id="NextActpopTri">
                        <table width="340" border="0" cellpadding="0" cellspacing="0" class="">
	                        <tr>
	                            <td align="left" valign="middle">
                                    <div class="NextActTime ellipsis"></div>
                                </td>
	                            <td id="nextActTimeAndArt" width="100" align="left" valign="middle" class="nscListIcnC"><div class="NextActAletName">&nbsp;</div></td>
	                        </tr>
                        </table>
                    </dd>
                    <div class="clearboth">&nbsp;</div>
                </dl>
            </div>
 
            <div class="nscListBoxSetRight" id="FollowBoxSet">
                <dl>
                    <dt class="padding2">
                        <icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="30314" Text="予約フォロー" UseEllipsis="False" width="80px" CssClass="clip"/>
                    </dt>
                    <dd class="Arrow padding2" id="FollowContactTrigger">
                        <div class="scNscFollowContactName ellipsis" style="width:300px"></div>
                        <icrop:PopOver ID="PopOver11" runat="server" TriggerClientID="FollowContactTrigger" Width="200px" Height="200px">
                            <div id="scNscFollowContactWindown">
                                <div id="scNscFollowContactWindownBox">
                                    <div class="scNscFollowContactHadder">
                                        <h3><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="30332" Text="予約フォロー" UseEllipsis="False" width="130px" CssClass="clip"/></h3>
                                    </div>
                                    <div class="scNscFollowContactListArea">
                                        <div class="scNscFollowContactListBox">
                                            <div class="scNscFollowContactListItemBox">
                                                <div class="scNscFollowContactListItem5">
                                                    <asp:UpdatePanel runat="server" ID="FollowContactUpdatePanel" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="FollowContactPanel" runat="server" Visible="false">
                                                                <ul class="nscListBoxSetIn">
                                                                    <asp:Label ID="followContactList" runat="server" Text=""></asp:Label>
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
                    </dd>

                    <dt class="end padding2">
                        <icrop:CustomLabel ID="CustomLabel29" runat="server" TextWordNo="30386" Text="日付" UseEllipsis="False" width="80px" CssClass="clip"/>
                    </dt>
                    <dd class="end Arrow padding3" id="FollowpopTri">
                    <!--nscListIcnPding-->
                        <table width="340" border="0" cellpadding="0" cellspacing="0">
	                        <tr>
	                            <td align="left" valign="middle">
                                    <div class="FollowTime"></div>
                                </td>
                                <td width="100" align="left" valign="middle" class="nscListIcnCFollow">
                                    <div style="display:none;" class="FollowAletName">&nbsp;</div>&nbsp;
                                </td>

	                        </tr>
                        </table>

                    <% 'フォロー日時用ポップアップ %>
                    </dd>
                    <div class="clearboth">&nbsp;</div>
                </dl>
            </div>
            <div class="clearboth">&nbsp;</div>
        </div>
    </div>
	
    <%'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START %>
    <!-- Success用 -->
    <%'2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD START %>
    <asp:Panel runat="server" ID="nscListBoxSet_HeightC_Panel">
        <div class="nscListBoxSet HeightC">
            <h4><icrop:CustomLabel ID="CustomLabel15" runat="server" TextWordNo="30315" Text="受注車両" UseEllipsis="False" width="170px" CssClass="clip"/></h4>
            <div class="nscListBoxSetIn HeightB3">
                <div id="SuccessSelectedCar">
                    <asp:UpdatePanel ID="SuccessSeriesUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                           <ul id="SuccessSelectedCarUl">
                                <asp:Repeater ID="SuccessSeriesRepeater" runat="server" DataSourceID ="FllwSelectedDataSource" EnableViewState="false">
                                    <ItemTemplate>
                                        <li title="<%# Eval("KEYVALUE")%>" id="SelectedCarlist<%# Eval("KEYVALUE")%>" class="SelectedCarlist FontBlack" value="<%# Eval("KEYVALUE")%>">
                                        
                                            <div style="display:inline-block;width:620px">
                                                <div class="ellipsis" style="display:inline-block;float:left;width:460px">
                                                    <%# HttpUtility.HtmlEncode(Eval("SERIESNM") & " " & Eval("VCLMODEL_NAME") & " " & Eval("DISP_BDY_COLOR"))%>
                                                </div>
                                                <div style="display:inline-block;float:right;width:150px;text-align:right">
                                                    <%# HttpUtility.HtmlEncode(Eval("DISPLAY_PRICE"))%>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%'2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD END %>
    <%'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END %>

    <!-- Give-up用 -->
    <div class="nscListBoxSet HeightD" style="display:block;" MaxHeight="172px">
        <h4><icrop:CustomLabel id="CustomLabel16" runat="server" TextWordNo="30316" Text="断念理由" UseEllipsis="False" width="170px" CssClass="clip"/></h4>
        <div class="nscListBoxSetIn HeightB4">
	        <dl class="hiLong">
	            <dt class="GiveupCarTitle" style="width:100px">
                    <icrop:CustomLabel id="CustomLabel17" runat="server" TextWordNo="30317" Text="他社成約車両" UseEllipsis="False" width="100px" CssClass="clip"/>
                </dt>
	            <dd class="Long Arrow BlackBoldTxt " id="popOverButton2">
                    <div class="Giveup"></div>    
                        &nbsp;
                </dd>
    <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 START--%>
	            <dt class="RedTxt end">
                    <icrop:CustomLabel id="CustomLabel18" runat="server" TextWordNo="30318" Text="詳細" UseEllipsis="False" width="80px" CssClass="clip"/>
                </dt>
	            <dd class="Long Arrow BlackBoldTxt end" id="popOverButton_GiveupReason">
                    <div id="dispGiveupReason" class="ellipsis" style="margin-right:30px;" ></div>
                    &nbsp;
                </dd>
    <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 END--%>
                <p class="clearboth">&nbsp;</p>
	        </dl>
        <div class="clearboth">&nbsp;</div>
        </div>
    </div>
    <%--**************************************--%>
    <%--次回活動エリア END--%>

    <input type="hidden" name="HD_nscListIcnA1" value="0" />
	
    <!-- Walk-in、Prospect、Hot、Success、Give-up用 -->
    <input type="hidden" name="HD_nscListIcnB1" value="0" />
    <input type="hidden" name="HD_nscListIcnB2" value="0" />
    <input type="hidden" name="HD_nscListIcnB3" value="0" />
    <input type="hidden" name="HD_nscListIcnB4" value="0" />
    <input type="hidden" name="HD_nscListIcnB5" value="0" />

    <% '2012/02/29 TCS 河原【SALES_STEP2】START
        '2012/02/29 TCS 河原【SALES_STEP2】END %>
        
    <asp:HiddenField ID="Cstkind" runat="server" Value="" />
    <asp:HiddenField ID="Insdid" runat="server" Value="" />

    <% '月日のデータフォーマット %>
    <asp:HiddenField ID="dateFormt" runat="server" Value="" />
    <asp:HiddenField ID="convid" runat="server" Value="" />

    <asp:HiddenField ID="ActDayFrom" runat="server" Value="" />
    <asp:HiddenField ID="ActDayTo" runat="server" Value="" />

    <% '2012/02/29 TCS 河原【SALES_STEP2】START
        '2012/02/29 TCS 河原【SALES_STEP2】END %>

    <% '活動結果 %>
    <asp:HiddenField ID="selectActRlst" runat="server" Value="" />

    <% '次回活動方法 %>
    <asp:HiddenField ID="selectNextActContact" runat="server" Value="0" />

    <asp:HiddenField ID="NextActDayFrom" runat="server" Value="" />
    <asp:HiddenField ID="NextActDayToFlg" runat="server" Value="" />
    <asp:HiddenField ID="NextActDayTo" runat="server" Value="" />

    <% '次回活動アラート %>
    <asp:HiddenField ID="selectNextActAlert" runat="server" Value="0" />
    <asp:HiddenField ID="selectNextActAlertWK" runat="server" Value="0" />

    <% 'フォロー有無 %>
    <asp:HiddenField ID="FollowFlg" runat="server" Value="0" />

    <% 'フォロー方法 %>
    <asp:HiddenField ID="selectFollowContact" runat="server" Value="0" />

    <asp:HiddenField ID="FollowDayFrom" runat="server" Value="" />
    <asp:HiddenField ID="FollowDayToFlg" runat="server" Value="" />
    <asp:HiddenField ID="FollowDayTo" runat="server" Value="" />

    <% 'フォローアラート %>
    <asp:HiddenField ID="selectFollowAlert" runat="server" Value="0" />
    <asp:HiddenField ID="selectFollowAlertWK" runat="server" Value="0" />

    <asp:HiddenField ID="selectGiveupMaker" runat="server" Value="" />

    <asp:HiddenField ID="selectGiveupCar" runat="server" Value="" />

    <% '断念理由 %>
    <asp:HiddenField ID="selectGiveupReason" runat="server" Value="" />
    <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 START--%>
    <asp:HiddenField ID="selectGiveupReasonID" runat="server" Value="" />
    <asp:HiddenField ID="selectGiveupReasonOtherFlg" runat="server" Value="false" />
    <%--2013/12/03 TCS 市川 Aカード情報相互連携開発 END--%>

    <asp:HiddenField ID="selectGiveupMakerWK" runat="server" Value="" />
    <asp:HiddenField ID="selectGiveupCarWK" runat="server" Value="" />

    <asp:HiddenField ID="selectGiveupCarName" runat="server" Value="" />

    <asp:HiddenField ID="selectGiveupMakerNameWK" runat="server" Value="" />

    <asp:HiddenField ID="selectActContactWK" runat="server" Value="0" />

    <asp:HiddenField ID="fllwStatus" runat="server" Value="" />

    <% '2012/02/29 TCS 河原【SALES_STEP2】START
        '2012/02/29 TCS 河原【SALES_STEP2】END %>

<%'2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START %>
    <input id="NextActTimeFromSelector_WK" type="datetime-local" style="display:none;" value="" />
    <input id="NextActTimeFromSelectorTime_WK" type="datetime-local" style="display:none;" value="" />
    <input id="NextActTimeToSelector_WK" type="time" style="display:none;" value="" />

    <input id="FollowTimeFromSelector_WK" type="datetime-local" style="display:none;" value="" />
    <input id="FollowTimeFromSelectorTime_WK" type="datetime-local" style="display:none;" value="" />
    <input id="FollowTimeToSelector_WK" type="time" style="display:none;" value="" />
<%'2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END %>

    <asp:HiddenField ID="LatestActTimeEnd" runat="server" Value="" />

<%'2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START %>
    <input id="Temp" type="datetime-local" style="display:none;" value="" />
<%'2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END %>

    <asp:HiddenField ID="ErrWord1" runat="server" Value="" />
    <asp:HiddenField ID="ErrWord2" runat="server" Value="" />
    <asp:HiddenField ID="ErrWord3" runat="server" Value="" />
    <asp:HiddenField ID="PopWord1" runat="server" Value="" />
    <asp:HiddenField ID="PopWord2" runat="server" Value="" />

    <% 'Follow-upBox種別 %>
    <asp:HiddenField ID="FllwDvs" runat="server" Value="" />

    <% '次回活動方法タイトル %>
    <asp:HiddenField ID="NextActContactTitle" runat="server" Value="" />
    <asp:HiddenField ID="FollowContactTitle" runat="server" Value="" />

    <% '次回活動方法タイトル %>
    <asp:HiddenField ID="NextActContactNextactivity" runat="server" Value="" />

    <div style="display:none"><icrop:DateTimeSelector ID="ActTimeFromSelectorWK" runat="server" Format="DateTime" ForeColor="#375388" /></div>
    <div style="display:none"><icrop:DateTimeSelector ID="ActTimeToSelectorWK" runat="server" Format="Time" ForeColor="#375388" /></div>

    <div style="display:none">
    <icrop:DateTimeSelector ID="NextActStartDateTimeSelectorWK" runat="server" Format="DateTime" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="NextActStartDateSelectorWK" runat="server" Format="Date" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="FollowStartDateTimeSelectorWK" runat="server" Format="DateTime" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="FollowStartDateSelectorWK" runat="server" Format="Date" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="NextActStartTimeSelectorWK" runat="server" Format="Time" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="FollowStartTimeSelectorWK" runat="server" Format="Time" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="NextActEndDateTimeSelectorWK" runat="server" Format="Time" ForeColor="#375388" />
    <icrop:DateTimeSelector ID="FollowEndDateTimeSelectorWK" runat="server" Format="Time" ForeColor="#375388" />
    </div>

    <% '外部から更新がかかる項目はUpdatePanel内に配置 %>
    <asp:UpdatePanel ID="HiddenFieldUpdatePanelPage3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <% 'Follow-upBox用SeqNo %>
            <asp:HiddenField ID="fllwSeq" runat="server" Value="" />

            <% 'Follow-upBox用店舗コード %>
            <asp:HiddenField ID="fllwstrcd" runat="server" Value="" />

            <asp:HiddenField ID="Vclseq" runat="server" Value="" />

            <% '来店人数 %>
            <asp:HiddenField ID="walkinNum" runat="server" Value="" />

            <% '2012/02/29 TCS 河原【SALES_STEP2】START
                '2012/02/29 TCS 河原【SALES_STEP2】END %>
                
            <% '受注車両 %>
            <asp:HiddenField ID="selectSelSeries" runat="server" Value="" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <% '2012/02/29 TCS 河原【SALES_STEP2】START %>
    <% '読み込み完了フラグ %>
    <asp:UpdatePanel runat="server" ID="PopupFlgUpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="NextActPopupFlg" runat="server" Value="0" />
            <asp:HiddenField ID="FollowContactPopupFlg" runat="server" Value="0" />
            <asp:HiddenField ID="GiveupReasonPopupFlg" runat="server" Value="0" />
            <asp:HiddenField ID="NextActTimePopupFlg" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="NextActNextAct" runat="server" Value="" />
    <asp:HiddenField ID="NextActFromTo" runat="server" Value="" />
    <asp:HiddenField ID="FollowFromTo" runat="server" Value="" />

    <% '2012/02/29 TCS 河原【SALES_STEP2】END %>
</div>
    
<% '予約フォロー日時 %>
<asp:Panel ID="FollowPanelAlertSel" runat="server" style="display:none;height:156px">
<div class="scNscFollowTimeListDiv" id="scNscFollowTimeListSelDiv" style="height:156px">
    <div class="scNscFollowTimeListArea" style="height:156px">
        <div class="scNscFollowTimeListBox" style="height:134px;width:298px;margin: 11px auto 9px;">
            <div class="scNscFollowTimeListItemBox">
                <div class="scNscFollowTimeListItem5">
                    <ul class="nscListBoxSetIn">
                        <asp:Label ID="FollowAlertSelLabel" runat="server" Text="Label"></asp:Label>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Panel>

<asp:Panel ID="FollowPanelAlertNonSel" runat="server" style="display:none;height:156px">
<div class="scNscFollowTimeListDiv" id="scNscFollowTimeListNonSelDiv" style="height:156px">
    <div class="scNscFollowTimeListArea" style="height:156px">
        <div class="scNscFollowTimeListBox" style="height:134px;width:298px;margin: 11px auto 9px;">
            <div class="scNscFollowTimeListItemBox">
                <div class="scNscFollowTimeListItem5">
                    <ul class="nscListBoxSetIn">
                        <asp:Label ID="FollowAlertNonSelLabel" runat="server" Text="Label"></asp:Label>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Panel>

<%'他社購入車種ポップアップ %>
<%'2019/05/30 TS 舩橋  UAT-0144 (FS)次世代タブレット試行店運用にむけた業務適合性検証 START%>
<icrop:PopOverForm ID="popOverForm2" runat="server" TriggerClientID="popOverButton2" PageCapacity="2"  HeaderTextWordNo="30331" Width="320px" Height="240px" OnClientOpen="popOverForm2_open" OnClientRender="popOverForm2_render" onClientClose="popOverForm2_close"></icrop:PopOverForm>
<%'2019/05/30 TS 舩橋  UAT-0144 (FS)次世代タブレット試行店運用にむけた業務適合性検証 END%>
<asp:Panel ID="popOverForm2_1" runat="server" style="display:none">
    <div class="scNscGiveupReasonListArea">
        <div class="scNscGiveupReasonListBox">
            <div class="scNscGiveupReasonListItemBox">
                <div class="scNscGiveupReasonListItem">
                    <asp:UpdatePanel runat="server" ID="GiveupReasonUpdatePanel" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="GiveupReasonPanel" runat="server" Visible="false">
                                <ul class="nscGiveupListBoxSetIn">
                                    <asp:Repeater ID="GiveupReasonRepeater" runat="server" DataSourceID ="Competition_MakermasterDataSource" ClientIDMode="Predictable">
                                        <ItemTemplate>
                                            <%'2012/04/26 TCS 河原 HTMLエンコード対応 START%>
                                            <li title="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "COMPETITIONMAKER")) %>" id="GiveupCarlist<%# DataBinder.Eval(Container.DataItem, "COMPETITIONMAKERNO") %>" class="GiveupCarlist popOverForm2_1_buttons FontBlack ellipsis" value="<%# DataBinder.Eval(Container.DataItem, "COMPETITIONMAKERNO")%>">
                                                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "COMPETITIONMAKER")) %>
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
</asp:Panel>
            
<asp:Label ID="GiveupCarLabel" runat="server" Text="Label"></asp:Label>

<script type="text/javascript">
    // 2019/05/30 TS 舩橋  UAT-0144 (FS)次世代タブレット試行店運用にむけた業務適合性検証 START
    function popOverForm2_open(pop) {
        if (pop.pageIndex > 0) {
            //PopOverOpen後、強制的に1ページ目へ移動する。
            pop.pageIndex = 0;
            setTimeout(function (pop) { pop.contentElement.flickable('select', pop.pageIndex); }, 0, pop);
        }
    }
    // 2019/05/30 TS 舩橋  UAT-0144 (FS)次世代タブレット試行店運用にむけた業務適合性検証 END

    function popOverForm2_render(pop, index, args, container, header) {
        var page;
        var wk;
        var listname = "#popOverForm2_1 #GiveupCarlist";
        var listvalue = this_form.selectGiveupMaker.value;
        $(".GiveupCarlist").removeClass("Selection");
        $(listname + listvalue).addClass("Selection");

        //◆を非表示に
        $("#popOverForm2_1").parents(".popover").find(".GiveuptgLeft").css("display", "none");

        if (index == 0) {
            page = container.children("#popOverForm2_1");
            page = $("#popOverForm2_1").css("display", "block");
            container.empty().append(page);
            container = $(".scNscGiveupReasonListBox").fingerScroll();

            $(".nscGiveupListBoxSetIn li:last-child").addClass("end");

            //ヘッダーの文言変更
            $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").text($("#PopWord1").attr("value"));

            //ポップアップのデザイン調整
            $("#popOverForm2_1").parents(".popover").css("border", "0px solid black");
            $("#popOverForm2_1").parents(".popover").css("background", "Transparent");
            $("#popOverForm2_1").parents(".popover").css("box-shadow", "None");
            $("#popOverForm2_1").parents(".popover").find(".content").css("padding", "0px");
            $("#popOverForm2_1").parents(".popover").find(".content").css("margin", "0px");
            $("#popOverForm2_1").parents(".popover").find(".content").css("background", "Transparent");
            $("#popOverForm2_1").parents(".popover").find(".content").css("border", "none");

            $("#popOverForm2_1").parents(".popover").css("border", "10px solid black");
            $("#popOverForm2_1").parents(".popover").css("background", "Transparent");
            $("#popOverForm2_1").parents(".popover").css("box-shadow", "none");
            $("#popOverForm2_1").parents(".popover").css("width", "330px");
            $("#popOverForm2_1").parents(".popover").css("height", "288px");
            $("#popOverForm2_1").parents(".popover").css("background-color", "#666");
            $("#popOverForm2_1").parents(".popover").css("border-radius", "8px");
            $("#popOverForm2_1").parents(".popover").css("background", "#FFF");
            $("#popOverForm2_1").parents(".popover").css("border", "#1c232f 1px solid");
            $("#popOverForm2_1").parents(".popover").css("box-shadow", "0px 0px 12px #333");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) START */
            //$("#popOverForm2_1").parents(".popover").css("background", "-webkit-gradient(linear, left top, left bottom, from(rgba(201,203,208,0.9)),color-stop(0.002, rgba(120,128,147,0.9)),color-stop(0.015, rgba(87,95,114,0.9)),color-stop(0.036, rgba(29,40,59,0.9)),color-stop(0.0365, rgba(3,11,26,0.9)),to(rgba(7,11,29,0.9)))");
            $("#popOverForm2_1").parents(".popover").css("background", "-webkit-gradient(linear, 0% 0%, 0% 100%, from(rgb(103, 110, 128)), color-stop(0.075, rgb(42, 52, 69)), color-stop(0.075, rgb(15, 22, 35)), to(rgb(3, 11, 25)))");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) END */
            $("#popOverForm2_1").parents(".icrop-PopOverForm-header").css("position", "relative");
            $("#popOverForm2_1").parents(".icrop-PopOverForm-header").css("width", "330px");
            $("#popOverForm2_1").parents(".icrop-PopOverForm-header").css("height", "45px");

            $("#popOverForm2_1").parents(".content").css("padding-left", "5px");
            $("#popOverForm2_1").parents(".content").css("height", "237px");
            $("#popOverForm2_1").parents(".content").css("overflow-x", "hidden");
            $("#popOverForm2_1").parents(".content").css("overflow-y", "hidden");

            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) START */
            //$("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").css("padding", "11px");
            $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").css("padding", "8px 0px 0px 10px");
            //$("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").css("font-size", "0.9em");
            $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").css("font-size", "20px");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) END */
            $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").addClass("clip")

            $("#popOverForm2_1").parents(".icrop-PopOverForm-content").css("position", "static");
            $("#popOverForm2_1").parents(".icrop-PopOverForm-content").css("border-radius", "10px");

            page.find(".popOverForm2_1_buttons").die("click")
            page.find(".popOverForm2_1_buttons").live("click", function (e) {
                //2012/04/26 TCS 河原 HTMLエンコード対応 START
                this_form.selectGiveupMakerNameWK.value = HtmlEncode($(this).attr("title"));
                //2012/04/26 TCS 河原 HTMLエンコード対応 END
                pop.pushPage({ itemId: "#" + $(this).attr("id") });
            });
        } else if (index != 0) {
            //ヘッダーの文言変更
            $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").text($("#PopWord2").attr("value"));

            wk = args.itemId
            this_form.selectGiveupMakerWK.value = wk.replace("#GiveupCarlist", "")
            page = container.children(args.itemId);
            page = $(args.itemId).css("display", "block");
            container.empty().append(page.clone(true));
            page = $(args.itemId).css("display", "none");
            var rightDiv = header.find(".icrop-PopOverForm-header-right");
            commitButton = $("<div id='PopupRegistButton' calss='PopupRegistButton'><icrop:CustomLabel runat='server' TextWordNo='30325'/></div>");
            commitButton
                .click(function (e) {
                    $(".GiveupCarList").removeClass("Selection");
                    $(".Giveup").html(this_form.selectGiveupMakerNameWK.value);
                    this_form.selectGiveupCarName.value = this_form.selectGiveupMakerNameWK.value;
                    this_form.selectGiveupMaker.value = this_form.selectGiveupMakerWK.value;
                    this_form.selectGiveupCar.value = "";
                    pop.closePopOver();
                })
            commitButton.appendTo(rightDiv);
            container = $(".scNscGiveupReasonListBox").fingerScroll();

            $("#detailTextBox").click(function (e) {
                pop.closePopOver($(this).val());
            });
            page.find("#detailTextBox").val("ItemId:" + args.itemId);

            //Backボタン押下時
            $(".icrop-PopOverForm-header-left").click(function (e) {
                //ヘッダーの文言変更
                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-title").text($("#PopWord1").attr("value"));
                
                //◆を非表示に
                $("#popOverForm2_1").parents(".popover").find(".GiveuptgLeft").css("display","none");
            });

            setTimeout(function () {
                //Backボタンの文言変更
                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-back").text("");
                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-back").append("<icrop:CustomLabel runat='server' TextWordNo='30331'/>");
                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-back span").css("position", "relative");
                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-back span").css("top", "1px");

                $("#popOverForm2_1").parents(".popover").find(".icrop-PopOverForm-header-left").before("<span class='GiveuptgLeft'>&nbsp;</span>")
            }, 1)
        }
    }
    function popOverForm2_close(pop, result) {
        return false;
    }

</script>

<%--2013/12/03 TCS 市川 Aカード情報相互連携開発 START--%>
<%--断念理由--%>
<icrop:PopOverForm ID="popOverForm_GiveupReason" runat="server" TriggerClientID="popOverButton_GiveupReason" PageCapacity="1" HeaderStyle="None" Width="300px" Height="240px" OnClientOpen="popOverForm_GiveupReason_open" OnClientRender="popOverForm_GiveupReason_render" onClientClose="popOverForm_GiveupReason_close">
</icrop:PopOverForm>
<div id="GiveUpReasonList_page1" style="display:none;overflow:hidden;" class="popOverBody">
    <asp:Repeater runat="server" ID="GiveUpReasonListRepeater" ViewStateMode="Enabled">
        <HeaderTemplate>
            <ul class="itemBox">
                <li class="itemRow ellipsis" style="border-top:none;" 
        </HeaderTemplate>
        <ItemTemplate>
            value="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ACT_RSLT_ID"))%>" data-HasDetail="<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "OTHER_FLG"))%>" >
                <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "RSLT_CAT_NAME"))%>
        </ItemTemplate>
        <SeparatorTemplate ></li><li class="itemRow ellipsis" </SeparatorTemplate>
        <FooterTemplate ></li></ul></FooterTemplate>
    </asp:Repeater>
</div>
<div id="GiveUpReasonList_page2" class="popoverBody" style="display:none;">
    <textarea id="GiveupReasonDetail" cols="34" rows="11" tabindex="5001"></textarea>
</div>
<div id="GiveUpReasonList_header" class="popOverHeader"  style="display:none;width:310px;" >
    <icrop:CustomLabel runat="server" ID="cancelSelectGiveupReason" TextWordNo="10125" CssClass="cancelButton" />
    <icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="30318" CssClass="titleLabel" style="width:135px;" />
    <icrop:CustomLabel ID="commitSelectGiveupReason" runat="server" TextWordNo="20014" CssClass="commitButton" />
    <div class="defaultButtonBox" style="display:none;"><div class="icrop-PopOverForm-header-left"></div></div>
</div>

<script type="text/javascript">
    function popOverForm_GiveupReason_render(pop, index, args, container, header) {
        var page;
        var headerPage;

        headerPage = header.children("#GiveUpReasonList_header");
        if (headerPage.size() == 0) {

            //Header初期化
            headerPage = $("#GiveUpReasonList_header").css("display", "block");
            header.empty().append(headerPage);

            //完了ボタンイベント設定
            headerPage.find(".commitButton").click(function (e) {

                var backButton = $("#GiveUpReasonList_header .icrop-PopOverForm-header-back");
                var commitButton = $("#GiveUpReasonList_header .commitButton").attr("otherText", $("#selectGiveupReason").val()); ;

                if (commitButton.attr("otherFlg") == "true" && commitButton.attr("pageIndex") == "1") {
                    //入力チェック
                    var strWk = $("#GiveupReasonDetail").val();
                    strWk = strWk.replace(/^[\s]+/g, "").replace(/[\s]+$/g, "");

                    if (strWk == "") {
                        alert(this_form.ErrWord3.value);
                    } else {
                        //$("#selectGiveupReason").val($("#GiveupReasonDetail").val());
                        backButton.click();
                        commitButton.attr("pageIndex", 0).attr("otherText", $("#GiveupReasonDetail").val());
                        //1ページ目へ移動後、自動で閉じる
                        setTimeout(function (pop) { pop.closePopOver(true); }, 600, pop);
                    }
                } else {
                    pop.closePopOver(true);
                }
            });

            //キャンセルボタンイベント設定
            headerPage.find(".cancelButton").click(function (e) {
                var backButton = $("#GiveUpReasonList_header .icrop-PopOverForm-header-back");
                var commitButton = $("#GiveUpReasonList_header .commitButton");
                if (commitButton.attr("otherFlg") == "true" && commitButton.attr("pageIndex") == "1") {
                    backButton.click();
                    commitButton.attr("pageIndex", 0);
                    //1ページ目へ移動後、自動で閉じる
                    setTimeout(function (pop) { pop.closePopOver(false); }, 600, pop);
                } else {
                    pop.closePopOver(false);
                }
            });
        }

        if (index == 0) {
            page = container.children("#GiveUpReasonList_page1");
            if (page.size() == 0) {
                page = $("#GiveUpReasonList_page1").css("display", "block");
                container.empty().append(page);
                page.fingerScroll();
                page.find(".itemRow").each(function () { if ($(this).attr("data-HasDetail") == "True") $(this).addClass("Arrow"); })
                .click(function (e) {

                    $(this).parent(0).children(".selected").removeClass("selected");
                    $(this).parent(0).children(".ArrowSelected").removeClass("ArrowSelected").addClass("Arrow");

                    //commitボタンに選択中プロパティつける。
                    var commitButton = $("#GiveUpReasonList_header .commitButton")
                        .attr("selectingItemId", $(this).attr("value"))
                        .attr("selectingItemName", $(this).text())
                        .attr("otherFlg", "false");
                    if ($(this).attr("data-HasDetail") == "True") {
                        $(this).removeClass("Arrow").addClass("ArrowSelected");
                        commitButton.attr("otherFlg", "true").attr("pageIndex", "1");
                        $("#GiveupReasonDetail").val(commitButton.attr("otherText"));
                        pop.pushPage({ itemId: $(this).attr("value") });
                    } else {
                        $(this).addClass("selected");
                    }
                });
            }
        } else if (index == 1) {
            //2page
            page = container.children("#GiveUpReasonList_page2");
            if (page.size() == 0) {
                page = $("#GiveUpReasonList_page2").css("display", "block");
                container.empty().append(page);
            }
        }
    }

    function popOverForm_GiveupReason_open(pop) {

        page = $("#GiveUpReasonList_page1");

        //選択状態をクリア
        page.find(".selected").removeClass("selected");
        page.find(".ArrowSelected").removeClass("ArrowSelected").addClass("Arrow");

        //選択済み状態を復元
        if ($("#selectGiveupReasonID").val() != "") {
            if ($("#selectGiveupReasonOtherFlg").val() == "true") {
                page.find(".Arrow").removeClass("Arrow").addClass("ArrowSelected");
              //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD START
                $("#GiveUpReasonList_header .commitButton").attr("otherText", $("#selectGiveupReason").val()).attr("otherFlg", "false").attr("pageIndex", "0");
                $("#GiveupReasonDetail").val($("#selectGiveupReason").val());
              //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD END
            } else {
                page.find(".itemRow").each(function () { if ($(this).attr("value") == $("#selectGiveupReasonID").val()) $(this).addClass("selected"); })
              //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD START
                $("#GiveUpReasonList_header .commitButton").attr("otherText", "").attr("otherFlg", "false").attr("pageIndex", "0");
                $("#GiveupReasonDetail").val("");
              //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD END
            }
        }
        //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) DELETE START
        //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) DELETE END

        if (pop.pageIndex > 0) {
            //PopOverOpen後、強制的に1ページ目へ移動する。
            pop.pageIndex = 0;
            setTimeout(function (pop) { pop.contentElement.flickable('select', pop.pageIndex); }, 600, pop);
        }
    }

    function popOverForm_GiveupReason_close(pop, result) {
        var selectedItemName = "";
        if (result) {
            var commit = $("#GiveUpReasonList_header .commitButton");
            $("#selectGiveupReasonID").val(commit.attr("selectingItemId"));
            selectedItemName = commit.attr("selectingItemName");
            if (commit.attr("otherFlg") == "true") {
                $("#selectGiveupReason").val($("#GiveupReasonDetail").val());
                $("#selectGiveupReasonOtherFlg").val("true");
                selectedItemName = selectedItemName + '(' + $("#selectGiveupReason").val() + ')';
            } else {
                $("#GiveupReasonDetail").val("");
                //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) MODIFY START
                $("#selectGiveupReason").val(jQuery.trim(selectedItemName));
                //2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) MODIFY END
                $("#selectGiveupReasonOtherFlg").val("false");
            }
            $("#dispGiveupReason").text(selectedItemName);
            commit.attr("otherText", "");
        } else {
            //キャンセルボタンにて閉じた場合
        }

        //ポストバックさせたい場合のみ、trueを返す
        return false;
    }
</script>
<%--2013/12/03 TCS 市川 Aカード情報相互連携開発 END--%>

<% '次回活動日時 %>
<asp:Panel ID="NextActPanelAlertSel" runat="server" style="display:none">
<div class="scNscNextActTimeListDiv" id="scNscNextActTimeListSelDiv">
    <div class="scNscNextActTimeListArea">
        <div class="scNscNextActTimeListBox">
            <div class="scNscNextActTimeListItemBox">
                <div class="scNscNextActTimeListItem5">
                    <ul class="nscListBoxSetIn">
                        <asp:Label ID="NextActAlertSelLabel" runat="server" Text="Label"></asp:Label>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Panel>

<asp:Panel ID="NextActPanelAlertNonSel" runat="server" style="display:none">
<div class="scNscNextActTimeListDiv" id="scNscNextActTimeListNonSelDiv">
    <div class="scNscNextActTimeListArea">
        <div class="scNscNextActTimeListBox">
            <div class="scNscNextActTimeListItemBox">
                <div class="scNscNextActTimeListItem5">
                    <ul class="nscListBoxSetIn">
                        <asp:Label ID="NextActAlertNonSelLabel" runat="server" Text="Label"></asp:Label>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Panel>


<%'--------------------------------------------%>
<%'次回活動日・予約フォロー選択ポップアップ Start %>
<%'--------------------------------------------%>
<div id="scNscNextActivityTimeWindown">
    <div id="scNscNextActivityTimeWindownBox">

        <%'ヘッダー Start %>
        <div class="scNscNextActivityTimeHadder">
            
            <%'タイトル %>
            <h3 class="clip"><icrop:CustomLabel ID="nextActivityPopupTitle" runat="server" TextWordNo="30327"/></h3>
            
            <%'戻るボタン %>
            <div class="scNscNextActivityTimeCancellButton">
                <a class="Square" href="javascript:void(0)"><icrop:CustomLabel CssClass="title" ID="nextActivityPopupBack1" runat="server" TextWordNo="30324"/><icrop:CustomLabel ID="nextActivityPopupBack2" runat="server" TextWordNo="30324" UseEllipsis="False" width="70px" CssClass="clip"/></a>
            </div>

            <%'完了ボタン %>
            <a class="scNscNextActivityTimeCompletionButton" href="javascript:void(0)">
                <icrop:CustomLabel ID="CustomLabel37" runat="server" TextWordNo="30325" UseEllipsis="False" width="70px" CssClass="clip"/>
            </a>
        </div>
        <%'ヘッダー End %>

        <%'メイン Start %>
        <div class="scNscNextActivityListBody">
            <div id="scNscNextActivityListArea" class="scNscNextActivityListArea page1">

                <%'１ページ目 %>
                <div id="scNscNextActivityPopPage1" class="popupPage">
                    <asp:UpdatePanel runat="server" ID="NextActTimeUpdatePanel" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Panel ID="NextActTimePanel" runat="server" Visible="false">
                    <ul>
                        <%'開始 %>
                        <li class="startDateTime">
                            <icrop:CustomLabel CssClass="title" ID="NextActTimePopupTitle1" runat="server" TextWordNo="30320"/>
                            <icrop:CustomLabel CssClass="title" ID="NextActTimePopupTitle2" runat="server" TextWordNo="30326"/>
                            <span class="value">
                                <icrop:DateTimeSelector ID="NextActStartDateTimeSelector" runat="server" Format="DateTime" ForeColor="#375388" />
                                <icrop:DateTimeSelector ID="NextActStartDateSelector" runat="server" Format="Date" ForeColor="#375388" />
                                <icrop:DateTimeSelector ID="FollowStartDateTimeSelector" runat="server" Format="DateTime" ForeColor="#375388" />
                                <icrop:DateTimeSelector ID="FollowStartDateSelector" runat="server" Format="Date" ForeColor="#375388" />
                            </span>
                        </li>
                        <li class="startTime">
                            <icrop:CustomLabel CssClass="title" ID="CustomLabel38" runat="server" TextWordNo="30390"/>
                            <span class="value">
                                <icrop:DateTimeSelector ID="NextActStartTimeSelector" runat="server" Format="Time" ForeColor="#375388" />
                                <icrop:DateTimeSelector ID="FollowStartTimeSelector" runat="server" Format="Time" ForeColor="#375388" />
                            </span>
                        </li>
                        <%'終了 %>
                        <li class="endTime">
                            <icrop:CustomLabel CssClass="title" ID="CustomLabel34" runat="server" TextWordNo="30321"/>
                            <span class="value">
                                <icrop:DateTimeSelector ID="NextActEndDateTimeSelector" runat="server" Format="Time" ForeColor="#375388" />
                                <icrop:DateTimeSelector ID="FollowEndDateTimeSelector" runat="server" Format="Time" ForeColor="#375388" />
                            </span>
                        </li>
                        <%'アラート %>
                        <li class="Arrow AlertArea">
                            <icrop:CustomLabel CssClass="title" ID="CustomLabel35" runat="server" TextWordNo="30322"/>
                            <span id="nextActivityPopupSelectAlert" class="value"></span>
                        </li>
                    </ul>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <%'２ページ目 %>
                <div id="scNscNextActivityPopPage2" class="popupPage">
                    <ul>
                        <asp:Repeater id="NextActivityAlertRepeater" runat="server" EnableViewState="false" DataSourceID="NextActivityAlertDataSource">
                            <ItemTemplate>
                                <li alertno="<%#Eval("ALARMNO")%>">
                                    <span class="alert">
                                        <asp:Literal runat="server" Mode="Encode" Text='<%#Eval("TITLE")%>'></asp:Literal>
                                    </span>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                    <%'スクロール用の余白 %>
                    <div style="height:10px">
                        <%'次回活動アラート %>
                        <asp:HiddenField ID="NextActivityAlertNoHidden" runat="server" Value="0" />
                        <asp:HiddenField ID="NextActivityFromToFlg" runat="server" Value="0" />

                        <%'次回フォロー %>
                        <asp:HiddenField ID="FollowAlertNoHidden" runat="server" Value="0" />
                        <asp:HiddenField ID="FollowFromToFlg" runat="server" Value="0" />
                    </div>

                    <%'アラームマスタデータソース %>
                    <asp:ObjectDataSource ID="NextActivityAlertDataSource" runat="server" 
                                          SelectMethod="GetAlertSel" 
                                          TypeName="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic"></asp:ObjectDataSource>
                </div>
                <p class="clearboth"></p>

            </div>
        </div>
        <%'メイン End %>

    </div>
</div>
<%'次回活動日・予約フォロー選択ポップアップ End %>
<%'2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START %>
<asp:HiddenField ID="SC3080203UpdateRWFlg" runat="server" Value="0" />
<%'2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END %>
