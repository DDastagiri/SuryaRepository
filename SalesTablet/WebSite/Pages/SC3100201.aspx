<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3100201.aspx.vb" Inherits="Pages_SC3100201" %>

<!DOCTYPE html>
<html lang="ja">
<head runat="server">
    <title></title>
    
    <%' $02 start 複数顧客に対する商談平行対応 %>
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/Style.css?20120828000000" />
    <link rel="Stylesheet" href="../Styles/jquery.popover.css?20120828000000" />
   	<link rel="stylesheet" href="../Styles/Controls.css?20120828000000" />
    <link rel="Stylesheet" href="../Styles/CommonMasterPage.css?20120828000000" />

    <%'スタイルシート(画面固有) %>
    <link rel="Stylesheet" href="../Styles/SC3100201/SC3100201.css?20120828000000" />

    <%'スクリプト(Masterページと合わせる) %>
    <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.16.custom.min.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.ipad.altfix.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.doubletap.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.flickable.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.js?20120828000000on-2.3.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.popover.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.fingerscroll.js?20120828000000"></script>

    <script type="text/javascript" src="../Scripts/icropScript.js?20120828000000"></script>
    
    <script type="text/javascript" src="../Scripts/jquery.CheckButton.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomButton.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomLabel.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomTextBox.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.DateTimeSelector.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.PopOverForm.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SegmentedButton.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SwitchButton.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomRepeater.js?20120828000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.NumericKeypad.js?20120828000000"></script>

    <%'スクリプト(画面固有) %>
    <script type="text/javascript" src="../Scripts/SC3100201/SC3100201.js?20120828000000"></script>
    <%' $02 end   複数顧客に対する商談平行対応 %>
</head>
<body>
<form id="this_form" runat="server">
    <%'処理中のローディング Start %>
    <div id="registOverlayBlackSC3100201"></div>
    <div id="processingServerSC3100201"></div>
    <%'処理中のローディング End %>

    <%'処理対象キー、および、画面遷移用 %>
    <asp:HiddenField ID="SelectedItemIndex" runat="server"/>
    <asp:HiddenField ID="SelectedVisitSeq" runat="server"/>
    <asp:HiddenField ID="SelectedCustomerSegment" runat="server"/>
    <asp:HiddenField ID="SelectedCustomerClass" runat="server"/>
    <asp:HiddenField ID="SelectedCustomerId" runat="server"/>
    <asp:Button ID="ButtonCustomer" runat="server" style="display:none;" />
    <asp:Button ID="ButtonConsent" runat="server" style="display:none;" />
    <asp:Button ID="ButtonWait" runat="server" style="display:none;" />
    <asp:Button ID="ButtonNotConsent" runat="server" style="display:none;" />


    <%'初期表示用ボタン %>
    <asp:Panel ID="Panel_PageInit" runat="server" Visible="false">
        <asp:Button ID="PageInitButton" runat="server" style="display:none;" />
        <script type="text/javascript">
            pageInit();
        </script>
    </asp:Panel>

    <%'画面遷移用ボタン %>
    <asp:Panel ID="Panel_Redirect" runat="server" Visible="false">
        <asp:Button ID="RedirectButton" runat="server" style="display:none;" />
        <script type="text/javascript">
            redirectSC3080201();
        </script>
    </asp:Panel>

    <%'未対応来店客が存在しない場合 %>
    <asp:Panel ID="Panel_NotVisitorList" runat="server" Visible="false">
        <div class="innerDataBox">
        <div class="nsc03noguest">
            <asp:Image ID="Image_NotVisit" runat="server" ImageUrl="~/Styles/Images/SC3100201/icon_noguest.png" Width="101" Height="122" /><br/>
            <icrop:CustomLabel ID="Label_NotVisit" runat="server"></icrop:CustomLabel>
        </div>
        </div>
    </asp:Panel>

    <%'一覧が存在する場合 %>
    <asp:Panel ID="Panel_VisitorList" runat="server" Visible="false">
        <div class="innerDataBox" id="Div_VisitorList">

        <%'未対応警告時間 %>
        <asp:HiddenField ID="NotDealTimeAlertSpan" runat="server"/>
 
        <%'未対応来店客一覧 %>
        <asp:Repeater ID="NotDealVisitList" runat="server" OnItemDataBound="NotDealVisitList_ItemDataBound">
        <HeaderTemplate>
            <table border="0" cellspacing="0" cellpadding="0" class="nsc03List">
                <tr>
                    <th><icrop:CustomLabel ID="Header_VisitTime" runat="server"></icrop:CustomLabel></th>
                    <th><icrop:CustomLabel ID="Header_CustInfo" runat="server"></icrop:CustomLabel></th>
                    <th class="NoBoder"><icrop:CustomLabel ID="Header_DealStatus" runat="server"></icrop:CustomLabel></th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <asp:HiddenField ID="visitSeq" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITSEQ"))%>' />
                <asp:HiddenField ID="updateDate" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "UPDATEDATE"))%>' />
                <asp:HiddenField ID="visitTimestamp" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITTIMESTAMP", "{0:yyyy/MM/dd HH:mm:ss}"))%>' />
                <asp:HiddenField ID="custmerId" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTID"))%>' />
                <asp:HiddenField ID="customerImage" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERIMAGEFILE"))%>' />
                <%' $02 start 複数顧客に対する商談平行対応 %>
                <asp:HiddenField ID="stopTime" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "STOPTIME", "{0:yyyy/MM/dd HH:mm:ss}"))%>' />
                <%' $02 end   複数顧客に対する商談平行対応 %>
                <asp:Label ID="customerName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERNAME"))%>' style="display:none;" />
                <asp:Label ID="customerNameTitle" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERNAMETITLE"))%>' style="display:none;" />
                <asp:Label ID="tentativeName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TENTATIVENAME"))%>' style="display:none;" />
                <asp:Label ID="vclRegNo" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNO"))%>' style="display:none;" />
                <asp:HiddenField ID="visitMeans" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITMEANS"))%>' />
                <asp:HiddenField ID="visitPersonNum" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITPERSONNUM"))%>' />
                <asp:HiddenField ID="salesTableNo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "SALESTABLENO"))%>' />
                <asp:Label ID="customerStaffName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTSTAFFNAME"))%>' style="display:none;" />
                <asp:HiddenField ID="visitStatus" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITSTATUS"))%>' />
                <asp:Label ID="dealStaffName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DEALSTAFFNAME"))%>' style="display:none;" />
                <asp:HiddenField ID="dealStaffImage" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DEALSTAFFIMAGE"))%>' />
                <%' $01 start step2開発 %>
                <asp:HiddenField ID="claimInfo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CLAIMINFO"))%>' />
                <%' $01 end   step2開発 %>
                <%' $02 start 複数顧客に対する商談平行対応 %>
                <asp:HiddenField ID="DispClass" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DISPCLASS"))%>' />
                <%' $02 end   複数顧客に対する商談平行対応 %>
                <tr>
                  <%' $02 start 複数顧客に対する商談平行対応 %>
	                <asp:Literal ID="tdStartTime" runat="server"/>
                  <%' $02 end   複数顧客に対する商談平行対応 %>
                    <icrop:CustomLabel ID="Label_VisitTimestamp" runat="server"></icrop:CustomLabel>
                  <%' $02 start 複数顧客に対する商談平行対応 %>
                  <asp:Literal ID="tdEndTime" runat="server" Text="</td>" />
	                <asp:Literal ID="tdStartInfo" runat="server"/>
                  <%' $02 end   複数顧客に対する商談平行対応 %>
                      <asp:Panel ID="Panel_InfoBox" runat="server" CssClass="InfoBox">
		                  <div class="InfoBoxLeft"><asp:Image ID="Image_Customer" runat="server" Width="60" Height="60" /></div>
		                  <div class="InfoBoxRight">
			                  <div class="TextName"><icrop:CustomLabel ID="Label_CustomerName" runat="server" Width="140" CssClass="ellipsis" style="line-height:23px"></icrop:CustomLabel>
                                  <%' $01 start step2開発 %>
			                      <div class="TextNameRight">
                                      <table border="0" cellpadding="0" cellspacing="0">
                                          <tr>
						                      <td width="28">&nbsp;</td>
                                              <td width="28"><div id="Div_Claim" class="IcnBox1" runat="server"><icrop:CustomLabel runat="server" ID="Claim_Icon_Word"></icrop:CustomLabel></div></td>
						                      <td width="21" class="IcnPadding"><img src="../Styles/Images/SC3100201/icon_salestable.png" width="17" height="15" alt=""/></td>
                                              <td class="IcnPadding"><span><icrop:CustomLabel ID="Label_SalesTableNo" runat="server"></icrop:CustomLabel></span></td>
                                          </tr>
                                      </table>
                                  </div>
                                  <%' $01 end   step2開発 %>
                              </div>
			                  <table border="0" cellspacing="0" cellpadding="0" class="IcnBox">
				                  <tr>
                                      <%' $01 start step2開発 %>
                                      <td width="93" class="NoBox">
                                          <div id="Div_VclRegNo" runat="server"><p class="ellipsis" style="width:85px;"><asp:Literal ID="Label_VclRegNo" runat="server"></asp:Literal></p></div>
                                          <div id="Div_MeansCar" runat="server" class="TextNameRightGray"><img src="../Styles/Images/SC3100201/icon_car.png" width="12" height="16" alt=""/></div>
                                          <div id="Div_MeansWalk" runat="server" class="TextNameRightGray"><img src="../Styles/Images/SC3100201/icon_person.png" width="12" height="16" alt=""/></div>
                                      </td>
						              <td width="17"><img src="../Styles/Images/SC3100201/icon_personnum.png" width="17" height="15" alt=""/></td>
						              <td width="16"><span><icrop:CustomLabel ID="Label_VisitPersonNum" runat="server"></icrop:CustomLabel></span></td>
						              <td width="13"><img src="../Styles/Images/SC3100201/icon_customerstaff.png" width="13" height="15" alt=""/></td>
						              <td><p class="ellipsis" style="width:65px;"><asp:Literal ID="Label_CustStaffName" runat="server"></asp:Literal></p></td>
                                      <%' $01 end   step2開発 %>
				                  </tr>
			                  </table>
		                  </div>
                      </asp:Panel>
                  <%' $02 start 複数顧客に対する商談平行対応 %>
                  <asp:Literal ID="tdEndInfo" runat="server" Text="</td>" />
	                <asp:Literal ID="tdStartSupport" runat="server"/>
                  <%' $02 end   複数顧客に対する商談平行対応 %>
		                  <div class="SupportBoxLeft">
                              <asp:Image ID="Image_DealStaff" runat="server" Width="32" Height="33" /><br>
			                  <icrop:CustomLabel ID="Label_DealStaffName" runat="server" Width="42" CssClass="ellipsis"></icrop:CustomLabel>
		                  </div>
		                  <div class="SupportBoxRight">
                        <div class="NotDealVisit_Timer" id="NotDealVisit_Timer<%# Container.ItemIndex %>">
                          <icrop:CustomLabel ID="NotDealVisit_Timer_Data" CssClass="Timer_Data" runat="server" style="display:none;"></icrop:CustomLabel>
                          <div class="Timer_Disp"><p><br/></p></div>
                        </div>
			                  <div class="ButtonLeft">
				                  <div id="Div_BlueButton" runat="server"><div class="BlueButton" onclick='onClickButtonConsent(<%# Server.HTMLEncode(Container.ItemIndex) %>);'><img src="../Styles/Images/SC3100201/button_consent.png" width="14" height="20" alt=""/></div></div>
				                  <div id="Div_BlueButtonOff" runat="server" class="BlueButtonOff"><img src="../Styles/Images/SC3100201/button_consent_off.png" width="14" height="20" alt=""/></div>
			                  </div>
			                  <div class="ButtonLeft02">
				                  <div id="Div_YellowButton" runat="server"><div class="YellowButton" onclick='onClickButtonWait(<%# Server.HTMLEncode(Container.ItemIndex) %>);'><img src="../Styles/Images/SC3100201/button_wait.png" width="20" height="20" alt=""/></div></div>
				                  <div id="Div_YellowButtonOff" runat="server" class="YellowButtonOff"><img src="../Styles/Images/SC3100201/button_wait_off.png" width="20" height="20" alt=""/></div>
			                  </div>
			                  <div class="ButtonRight">
				                  <div id="Div_RedButton" runat="server"><div class="RedButton" onclick='onClickButtonNotConsent(<%# Server.HTMLEncode(Container.ItemIndex) %>);'><img src="../Styles/Images/SC3100201/button_notconsent.png" width="20" height="20" alt=""/></div></div>
				                  <div id="Div_RedButtonOff" runat="server" class="RedButtonOff"><img src="../Styles/Images/SC3100201/button_notconsent_off.png" width="20" height="20" alt=""/></div>
			                  </div>
		                  </div>
                  <%' $02 start 複数顧客に対する商談平行対応 %>
                  <asp:Literal ID="tdEndSupport" runat="server" Text="</td>" />
                  <%' $02 end   複数顧客に対する商談平行対応 %>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
        </asp:Repeater>

        <%'参考情報一覧 %>
        <asp:Repeater ID="ReferenceVisitList" runat="server" OnItemDataBound="ReferenceVisitList_ItemDataBound">
        <HeaderTemplate>
            <table border="0" cellspacing="0" cellpadding="0" class="nsc03List nsc03ListReference">
                <tr>
                    <th colspan="3" class="Reference"><icrop:CustomLabel ID="Header_Reference" runat="server"></icrop:CustomLabel></th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <asp:HiddenField ID="custmerId" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTID"))%>' />
                <asp:HiddenField ID="customerImage" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERIMAGEFILE"))%>' />
                <asp:Label ID="customerName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERNAME"))%>' style="display:none;" />
                <asp:Label ID="customerNameTitle" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTOMERNAMETITLE"))%>' style="display:none;" />
                <asp:Label ID="tentativeName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "TENTATIVENAME"))%>' style="display:none;" />
                <asp:Label ID="vclRegNo" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNO"))%>' style="display:none;" />
                <asp:HiddenField ID="visitMeans" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITMEANS"))%>' />
                <asp:HiddenField ID="visitPersonNum" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITPERSONNUM"))%>' />
                <asp:HiddenField ID="salesTableNo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "SALESTABLENO"))%>' />
                <asp:HiddenField ID="customerStaffId" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTSTAFFCD"))%>' />
                <asp:Label ID="customerStaffName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CUSTSTAFFNAME"))%>' style="display:none;" />
                <asp:HiddenField ID="visitStatus" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITSTATUS"))%>' />
                <asp:Label ID="dealStaffName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DEALSTAFFNAME"))%>' style="display:none;" />
                <asp:HiddenField ID="dealStaffImage" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "DEALSTAFFIMAGE"))%>' />
                <%' $01 start step2開発 %>
                <asp:HiddenField ID="visitTimestamp" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITTIMESTAMP", "{0:yyyy/MM/dd HH:mm:ss}"))%>' />
                <asp:HiddenField ID="claimInfo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "CLAIMINFO"))%>' />
                <%' $01 end   step2開発 %>

                <tr>
                    <%' $02 start 複数顧客に対する商談平行対応 %>
	                  <asp:Literal ID="tdStartTime" runat="server"/>
                    <%' $02 end   複数顧客に対する商談平行対応 %>
                      <icrop:CustomLabel ID="Label_VisitTimestamp" runat="server"></icrop:CustomLabel>
                    <%' $02 start 複数顧客に対する商談平行対応 %>
                    <asp:Literal ID="tdEndTime" runat="server" Text="</td>" />
	                  <asp:Literal ID="tdStartInfo" runat="server"/>
                    <%' $02 end   複数顧客に対する商談平行対応 %>
                        <div class="InfoBoxLeft"><asp:Image ID="Image_Customer" runat="server" Width="60" Height="60" /></div>
                        <div class="InfoBoxRight">
                            <div class="TextName"><icrop:CustomLabel ID="Label_CustomerName" runat="server" Width="140" CssClass="ellipsis" style="line-height:23px"></icrop:CustomLabel>
                                <%' $01 start step2開発 %>
                                <div class="TextNameRight">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
						                    <td width="28">&nbsp;</td>
                                            <td width="28"><div id="Div_Claim" class="IcnBox1" runat="server"><icrop:CustomLabel runat="server" ID="Claim_Icon_Word"></icrop:CustomLabel></div></td>
						                    <td width="21" class="IcnPadding"><img src="../Styles/Images/SC3100201/icon_salestable.png" width="17" height="15" alt=""/></td>
                                            <td class="IcnPadding"><span><icrop:CustomLabel ID="Label_SalesTableNo" runat="server"></icrop:CustomLabel></span></td>
				                        </tr>
                                    </table>
                                </div>
                                <%' $01 end   step2開発 %>
                            </div>
                            <table border="0" cellspacing="0" cellpadding="0" class="IcnBox">
                                <tr>
                                    <%' $01 start step2開発 %>
                                    <td width="93" class="NoBox">
                                        <div id="Div_VclRegNo" runat="server"><p class="ellipsis" style="width:85px;"><asp:Literal ID="Label_VclRegNo" runat="server"></asp:Literal></p></div>
                                        <div id="Div_MeansCar" runat="server" class="TextNameRightGray"><img src="../Styles/Images/SC3100201/icon_car.png" width="12" height="16" alt=""/></div>
                                        <div id="Div_MeansWalk" runat="server" class="TextNameRightGray"><img src="../Styles/Images/SC3100201/icon_person.png" width="12" height="16" alt=""/></div>
                                    </td>
					                <td width="17"><img src="../Styles/Images/SC3100201/icon_personnum.png" width="17" height="15" alt=""/></td>
					                <td width="16"><span><icrop:CustomLabel ID="Label_VisitPersonNum" runat="server"></icrop:CustomLabel></span></td>
					                <td width="13"><img src="../Styles/Images/SC3100201/icon_customerstaff.png" width="13" height="15" alt=""/></td>
					                <td><p class="ellipsis" style="width:65px;"><asp:Literal ID="Label_CustStaffName" runat="server"></asp:Literal></p></td>
                                    <%' $01 end   step2開発 %>
                                </tr>
                            </table>
                        </div>
                    <%' $02 start 複数顧客に対する商談平行対応 %>
                    <asp:Literal ID="tdEndInfo" runat="server" Text="</td>" />
	                  <asp:Literal ID="tdStartSupport" runat="server"/>
                    <%' $02 end   複数顧客に対する商談平行対応 %>
                        <div class="SupportBoxLeft">
                            <asp:Image ID="Image_DealStaff" runat="server" Width="32" Height="33" /><br>
			                <icrop:CustomLabel ID="Label_DealStaffName" runat="server" Width="42" CssClass="ellipsis"></icrop:CustomLabel>
                        </div>
                        <div class="SupportBoxRight">
                            <div class="TextBox"><icrop:CustomLabel ID="Label_ReferenceStatus" runat="server"></icrop:CustomLabel></div>
                        </div>
                  <%' $02 start 複数顧客に対する商談平行対応 %>
                  <asp:Literal ID="tdEndSupport" runat="server" Text="</td>" />
                  <%' $02 end   複数顧客に対する商談平行対応 %>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
        </asp:Repeater>

        </div>
    </asp:Panel>

</form>

</body>
</html>
