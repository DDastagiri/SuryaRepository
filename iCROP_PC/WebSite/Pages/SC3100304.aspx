<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3100304.aspx.vb" Inherits="Pages_SC3100304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090301.aspx
'─────────────────────────────────────
'機能： ウェルカムボード
'補足： 
'作成： 2013/03/14 TMEJ t.shimamura
'更新： 2013/04/16 TMEJ m.asano     ウェルカムボード仕様変更対応 $01
'─────────────────────────────────────
-->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link rel="stylesheet" href="../Styles/SC3100304/common.css?20130416000000" type="text/css" media="screen,print" />
  <link rel="stylesheet" href="../Styles/SC3100304/SC3100304_01.css?20130416000000" type="text/css" media="screen,print" />
  <link rel="stylesheet" href="../Styles/SC3100304/SC3100304_02.css?20130416000000" type="text/css" media="screen,print" />
  <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20130416000000"></script>
  <script type="text/javascript" src="../Scripts/jquery-1.5.2.js?20130416000000"></script>
  <script type="text/javascript" src="../Scripts/SC3100304/SC3100304.js?20130416000000"></script>
  <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20130416000000"></script>
  <script type="text/javascript" src="../Scripts/icropBase.js?20130416000001"></script>
  <script type="text/javascript" src="../Scripts/icropScript.js?20130416000000"></script>
  <title></title>
    <script type="text/javascript">
        icropBase.getUser = function () {
            return "<%=StaffContext.Current.Account%>";
        }
    </script>

</head>

<body>
<form id="Form1" runat="server" class="BgImageR">
   <%-- ======================== デバッグ用エリア ======================== --%>
  <div id="DebugArea" runat="server" style="position:absolute; padding:5px 5px 0px 5px; top:0px; left:0px; width:100%; height:75px; z-index:100; font-size:11px; background:rgba(255, 255, 255, 0.75);" Visible="false">
        <div style="margin:0px 10px 0px 10px; float:left;">
            <div id="pageData"></div>
            <div id="frameData"></div>
        </div>
        <div style="margin:0px 5px 0px 5px; float:left">  
        </div>
        <div style="margin:36px 5px 0px 5px;">
            <input type="button" id="TestPushNewCustomerButton" class="likeButton" value="新規顧客来店" />
            <input type="button" id="TestPushOrgCustomerButton" class="likeButton" value="既存顧客来店(表示名+1)"/>
            <div style="top:10px; left:400px; font-size:10pt; position:absolute; float:left">
              <div>カウンターの値(チップ1)：<span id="CounterNum0">0</span>sec</div>
              <div>カウンターの値(チップ2)：<span id="CounterNum1">0</span>sec</div>
              <div>カウンターの値(チップ3)：<span id="CounterNum2">0</span>sec</div>
            </div> 
        </div>
  </div>

  <%-- 来店通知画面 --%>
  <%-- $01 START ウェルカムボード仕様変更対応  --%>
  <div id="WelcomeMain" class="BgImageR" style="z-index:5; width:1920px;">
  <%-- $01 END ウェルカムボード仕様変更対応  --%>
    <%--位置変動エリア --%> 
    <div id = "AddRegion" class = "AddRegion" align = "center">
      <div id = "CenteringCell" class = "CenteringCell">
        <img src="../Styles/images/SC3100304/WelcomeTitle01.png" width="500" height="168" alt="Welcome" />
        <div class= "DummyCellMiddle"></div>
        <%-- 顧客名リスト --%>
        <ul class="WelcomeNamesData" id = "WelcomeCustomerList"></ul>
        <div class="WelcomeMess1 ellipsis"><asp:Literal ID="WelcomeMessageFooter" runat="server"  /></div>
      </div>
    </div>
    <div id="CustomerName"></div>
    <div class="footer">
      <h4 class="ShopName ellipsis"><asp:Literal ID="BranchName02" runat="server" /></h4>
      <div class="Titles"><img src="../Styles/images/SC3100304/bottomTitles01a.png" width="274" height="80" alt="TOYOTA" /><img src="../Styles/images/SC3100304/bottomTitles01b.png" width="270" height="80" alt="広州TOYOTA" /></div>
    </div>
  </div>
</form>
</body>
</html>
