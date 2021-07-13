<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3100104.aspx.vb" Inherits="Pages_SC3100104"%>

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
  <link rel="Stylesheet" href="../Styles/SC3100104/SC3100104.css?20180719000000" type="text/css"
    media="screen" />
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
  <script type="text/javascript" src="../Scripts/SC3100104/SC3100104_aspx.js?20140708141600"></script>
</head>
<body>
  <form id="Form1" name="Search" method="get" action="#" runat="server" >
  <%'オーバーレイ %>
  <div id="SC3100104_OverRay" class="OverRay">
  </div>
  <div id="LoadingAnimation2" class="show2" runat="server">
  </div>
  <asp:Panel ID="LoadSpinPanel" runat="server">
    <asp:Button ID="LoadSpinButton" runat="server" Style="display: none;" />
    <script type="text/javascript">
      pageInit();
    </script>
  </asp:Panel>
  <%--非同期読み込みのためのScriptManagerタグ--%>
  <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
  </asp:ScriptManager>
  <%' 検索タイプ %>
  <asp:HiddenField ID="SerchType" Value="4" runat="server"></asp:HiddenField>
  <%' 選択データ %>
  <asp:HiddenField ID="SelectedCustName" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustNameTitle" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedRegNo" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedVIN" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedPersonNumber" Value="1" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustKubun" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustID" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustType" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustStaffCode" Value="" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedCustomerFlag" Value="0" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedSearchTypeFlag" Value="1" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="SelectedPersonNumberFlag" Value="1" runat="server" ></asp:HiddenField>
  <asp:HiddenField ID="InputSearchText" Value="" runat="server" ></asp:HiddenField>
  <div class="NS-00-0010_popWindow">
    <div class="dataBox">
      <div class="innerDataBox">
        <div class="ButtonHDSet">
          <%'来店人数ボタン %>
          <ul class="NoButton">
            <li class="PersonButton SelectedBottun">1</li>
            <li class="PersonButton">2</li>
            <li class="PersonButton">3</li>
            <li class="PersonButton">4</li>
            <li class="PersonButton">5</li>
          </ul>
          <%'検索種別 %>
          <div class="ButtonBox">
            <ul class="SearchBottun">
              <li class="SearchTypeButton clip" value="2">
                <asp:Literal ID="SearchTypeCustomerName" runat="server"/></li>
              <li class="SearchTypeButton SelectedBottun clip" value="4">
                <asp:Literal ID="SearchTypeTelephone" runat="server"/></li>
              <li class="SearchTypeButton clip" value="5">
                <asp:Literal ID="SearchTypeSocialNumber" runat="server"/></li>
              <li class="SearchTypeButton clip " value="1">
                <asp:Literal ID="SearchTypeVehicleNo" runat="server"/></li>
              <li class="SearchTypeButton clip" value="3">
                <asp:Literal ID="SearchTypeVehicleVin" runat="server"/></li>
            </ul>

          </div>
          <div class="SearchSet">
            <div class="Search1">
              <%'検索ボックス %>
              <%'iframeの中で標準のテキストボックスコントロールを使用すると、クリアボタン押下時の処理で挙動がおかしくなるため、inputタグを使用する %>
              <input ID="SearchTextString" type="text" runat="server" class="Search2" />
              <div id="ClearButton"></div>
              <asp:ImageButton runat="server" type="image" ID="CustomerSearchButton" class="Search3" src="../Styles/Images/serchImage01.png" alt="Search" name="searchBtn1" />
              <icrop:CustomLabel ID="SearchTextStringDummy" type="text" runat="server" style="height: 28px; position: absolute;width: 264px;top: 8px;left: 30px; display:none;font-weight: normal; font-size: 11px;" />
            </div>
          </div>
        </div>
        <%'ローディングアニメーション(検索用) %>
        <div id="LoadingAnimation" class="show" runat="server"></div>
        <div class="ListSet">

          <%' 登録ボタン用パネル %>
          <asp:UpdatePanel ID="CreateChipUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <% '登録ボタン用 %>
              <asp:Button ID="RegisterButton" runat="server" Style="display: none;" />
              <% '登録ボタン後処理用 %>
              <input type="button" id="RegisterButton_Pre" style="display: none;" onclick="redirectSC3100104();" />
              <%' チップ作成フラグ %>
              <asp:HiddenField ID="CreateChipEndFlg" Value="0" runat="server"></asp:HiddenField>
            </ContentTemplate>
          </asp:UpdatePanel>

          <%' アップデートパネルトリガー %>
          <asp:UpdatePanel ID="CustomerListUpdateTriger" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RegisterButton" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
          </asp:UpdatePanel>
          <ul class="TitleSet">
            <li class="clip">
              <asp:Literal ID="ColumNameCustomerName" runat="server" /></li>
            <li class="clip">
              <asp:Literal ID="ColumNameTelephone" runat="server" /></li>
            <li class="clip">
              <asp:Literal ID="ColumNameSocialNumber" runat="server" /></li>
            <li class="clip">
              <asp:Literal ID="ColumNameVehicle" runat="server"/></li>
            <li class="clip">
              <asp:Literal ID="ColumNameSalesStaff" runat="server"/></li>
          </ul>
          <%' 顧客リストアップデートパネル %>
          <asp:UpdatePanel ID="UpdateAreaCustomerList" runat="server" UpdateMode="Conditional">
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="CustomerSearchButton" />
            </Triggers>
            <ContentTemplate>
            
              <%' 顧客検索終了フラグ %>
              <asp:HiddenField ID="CustomerSerchEnd" Value="0" runat="server"></asp:HiddenField>
              <div class="ListContent" runat="server" id="SearchResultList">
              <%'検索結果0の文言 %>
                <div id="CustomerNotFound" runat="server" visible="false" class="NotFoundText ellipssis">
                  <asp:Literal ID="CustomerNotFoundLiteral" runat="server"></asp:Literal>
                </div>
                <%'検索結果が多すぎるときの文言  %>
                <div id="CustomerOverFlow" runat="server" visible="false" class="OverFlowText ellipssis">
                  <asp:Literal ID="OverFlowLiteral1" runat="server" ></asp:Literal><br />
                  <asp:Literal ID="OverFlowLiteral2" runat="server"></asp:Literal>
                </div>
                <%' 顧客リスト %>
                <% '一覧スクロール範囲 %>
                <div id="CustomerList" class="CustomerInnerDataBox" runat="server">
                <asp:Repeater ID="CustomerRepeater" runat="server" >
                  <ItemTemplate>
                    <% '顧客情報 %>
                    <li id="CustomerRow">
                      <asp:HiddenField ID="CurrentCustName" runat="server" Value='<%# Server.HTMLEncode(Eval("NAME").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustNameTitle" runat="server" Value='<%# Server.HTMLEncode(Eval("NAMETITLE").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustID" runat="server" Value='<%# Server.HTMLEncode(Eval("CUSTCD").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustKBN" runat="server" Value='<%# Server.HTMLEncode(Eval("CUSTKBN").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustVclRegNo" runat="server" Value='<%# Server.HTMLEncode(Eval("VCLREGNO").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustVIN" runat="server" Value='<%# Server.HTMLEncode(Eval("VIN").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustStaffCode" runat="server" Value='<%# Server.HTMLEncode(Eval("STUFFCD").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField ID="CurrentCustType" runat="server" Value='<%# Server.HTMLEncode(Eval("CUSTYPE").ToString()) %>'></asp:HiddenField>
                      <% '顧客情報リスト %>
                      <div class="ListTextBox01">
                        <div class="ListTextBoxIn ellipsis">
                           <asp:Literal ID="CustomerNameLiteral" runat="server" />
                        </div>
                        <%-- 2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START--%>
                        <div ID="Lmark" runat="server" text="" visible="False" class="Lmark ellipsis"></div>
                        <%-- 2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END--%>
                      </div>
                      <div class="ListTextBox02">
                        <div class="ListTextBoxIn01 ellipsis">
                          <asp:Literal ID="MobilePhoneNumberLiteral" runat="server" />
                        </div>
                        <div class="ListTextBoxIn02 ellipsis">
                          <asp:Literal ID="TelePhoneNumberLiteral" runat="server" />
                        </div>
                      </div>
                      <div class="ListTextBox05">
                        <div class="ListTextBoxIn05 ellipsis">
                           <asp:Literal ID="SocialNumberLiteral" runat="server" />
                        </div>
                      </div>
                      <div class="ListTextBox03">
                        <div class="ListTextBoxIn01">
                          <div class="ListTextBoxIn01a ellipsis">
                            <asp:Literal ID="RegNoLiteral" runat="server" />
                          </div>
                          <div class="ListTextBoxIn01b ellipsis">
                            <asp:Literal ID="VehicleNameLiteral" runat="server" />
                          </div>
                        </div>
                        <div class="ListTextBoxIn02 ellipsis">
                          <asp:Literal ID="VINLiteral" runat="server" />
                        </div>
                      </div>
                      <div class="ListTextBox04">
                        <div class="ListTextBoxIn ellipsis">
                          <asp:Literal ID="StaffNameLiteral" runat="server" />
                        </div>
                      </div>
                    </li>
                  </ItemTemplate>
                </asp:Repeater>
                </div>
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
  </form>
</body>
</html>
