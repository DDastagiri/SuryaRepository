<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3100103.aspx.vb" Inherits="Pages_SC3100103" %>

<!DOCTYPE html>

<html lang="ja">
  <head id="Head1" runat="server">

    <%'タイトル %>
    <title></title>
    
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/Style.css?20120817000000" />
    <link rel="Stylesheet" href="../Styles/jquery.popover.css?20120817000000" />
   	<link rel="stylesheet" href="../Styles/Controls.css?20120817000000" />
    <link rel="Stylesheet" href="../Styles/CommonMasterPage.css?20120817000000" />

    <%'スタイルシート(画面固有) %>
    <link rel="Stylesheet" href="../Styles/SC3100103/SC3100103.css?20121003000000" type="text/css" media="screen" />

    <%'スクリプト(Masterページと合わせる) %>
    <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.16.custom.min.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.ipad.altfix.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.doubletap.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.flickable.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.json-2.3.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.popover.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.fingerscroll.js?20120817000000"></script>

    <script type="text/javascript" src="../Scripts/icropScript.js?20120817000000"></script>
    
    <script type="text/javascript" src="../Scripts/jquery.CheckButton.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomButton.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomLabel.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomTextBox.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.DateTimeSelector.js?20120817000000"></script>

    <script type="text/javascript" src="../Scripts/jquery.SegmentedButton.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SwitchButton.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomRepeater.js?20120817000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.NumericKeypad.js?20120817000000"></script>

    <%'スクリプト(画面固有) %>
    <script type="text/javascript" src="../Scripts/SC3100103/SC3100103_aspx.js?20120817000000"></script>

  </head>
  <body>
    <div id="bodyFrame" style="height:570px;">
      <form id="this_form" runat="server">
    
        <%'処理中のローディング Start %>
        <div id="registOverlayBlackSC3100103"></div>
        <div id="processingServerSC3100103"></div>
        <asp:Panel ID="LoadSpinPanel" runat="server">
        <asp:Button ID="LoadSpinButton" runat="server" style="display:none;" />
            <script type="text/javascript">
              pageInit();
            </script>
        </asp:Panel>
        <%'処理中のローディング End %>
        
        <%'0件表示パネル %>
        <asp:Panel ID="NotStandByStaffPanel" runat="server">
          <div class="NoData">
            <asp:Image ID="ImageNotStandByStaff" runat="server" ImageUrl="~/Styles/Images/SC3100103/sc_big.png" Width="101" Height="122" /><br/>
            <br/>
	          <p class="ellipsis"><asp:Literal ID="NotStandByStaffStatus" runat="server" /></p>
          </div>
        </asp:Panel>
        
        <%'スタンバイスタッフ並び順変更表示パネル %>
        <asp:Panel ID="StandByStaffPanel" runat="server">
        
          <% '登録ボタン用 %>
          <input type="button" id="RegisterButton_Pre" style="display:none;" onclick="redirectSC3100103();" />
          <asp:Button ID="RegisterButton" runat="server" style="display:none;" />
          <%--エラーメッセージ--%>
          <asp:HiddenField id="StandByStaffErrorMessage" runat="server"></asp:HiddenField>
          
          <% '矢印ボタン %>
          <div id="SelectTopButton" style="display:none;"><img class="TopButton" src="../Styles/Images/SC3100103/TopButton.png" alt="" /></div>
          <div id="SelectBottomButton" style="display:none;"><img class="BottomButton" src="../Styles/Images/SC3100103/BottomButton.png" alt="" /></div>
          
          <% 'スタッフ一覧ヘッダ %>
          <div class="StandByStaffHead">
	          <div class="Col2h">
		          <p class="ellipsis"><asp:Literal ID="UserNameHeadLiteral" runat="server" /></p>
	          </div>
	          <div class="Col3h">
		          <p class="ellipsis"><asp:Literal ID="ResultCountHeadLiteral" runat="server" /></p>
	          </div>
	          <div class="Col4h">
		          <p class="ellipsis"><asp:Literal ID="MaxSalesEndHeadLiteral" runat="server" /></p>
	          </div>
          </div>
          
          <%-- スタッフ行繰り返し--%>
          <asp:Repeater ID="StandByStaffRepeater" runat="server">
            <HeaderTemplate>
              <% 'スタッフ一覧スクロール範囲 %>
              <div class="innerDataBox">
            </HeaderTemplate>
            <ItemTemplate>
                <% '使用する情報 %>
                <asp:HiddenField ID="Account" runat="server" Value='<%# Server.HTMLEncode(Eval("ACCOUNT").ToString()) %>' />
                <asp:HiddenField ID="PresenceCategoryDate" runat="server" Value='<%# Server.HTMLEncode(Eval("PRESENCECATEGORYDATE").ToString()) %>' />
                
                <% 'スタッフ行 %>
                <div id="StaffChipRow" class="NormalRow" runat="server">
	                <div class="Col1">
                    <asp:Image ID="OrgImgFileImage" runat="server" width="40" height="40" />
	                </div>
	                <div class="Col2">
		                <p class="ellipsis"><asp:Literal ID="UserNameLiteral" runat="server" /></p>
	                </div>
	                <div class="Col3">
		                <img src="../Styles/Images/SC3100103/ico05.png" alt="" />
		                <p class="ellipsis"><asp:Literal ID="ResultCountLiteral" runat="server" /></p>
	                </div>
	                <div class="Col4">
		                <p><asp:Literal ID="MaxSalesEndLiteral" runat="server" /></p>
	                </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
              </div>
            </FooterTemplate> 
          </asp:Repeater>
        </asp:Panel>

      </form>
    </div>
  </body>
</html>
