<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3130501.aspx.vb" Inherits="SC3130501" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- スタイルシート(画面固有) --%>
    <link href="../Styles/common.css" rel="stylesheet" type="text/css" media="screen,print"> 
    <link href="../Styles/footer.css" rel="stylesheet" type="text/css" media="screen,print"> 
    <link href="../Styles/SC3130501/tsl05.css?201304180001" rel="stylesheet" type="text/css" media="screen,print"> 
    <%-- スクリプトライブラリ --%>
    <script src="../Scripts/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/icropBase.js?201304240001" type="text/javascript"></script>
    <script src="../Scripts/icropScript.js" type="text/javascript"></script>
    <%-- スクリプト(画面固有) --%>
    <script src="../Scripts/SC3130501/SC3130501.js?201304080001" type="text/javascript"></script>

    <script type="text/javascript">
        icropBase.getUser = function () {
            return "<%=StaffContext.Current.Account%>";
        }
    </script>


</head>
<body>
    <form id="form1" runat="server">

		<!-- ここからメインブロック -->
		<div id="tsl05-02_Main">
        
		<!-- ここからヘッダ -->
        <div class="airport_Header">
          <div class="LeftLogo"></div>
          <div class="CenterTitle"><asp:Literal ID="StaticPageTitle" runat="server" /></div>
          <!--<div class="SMBLogo"></div>-->
          <div class="Data">2012/12/21 10:00</div>
        </div>
		<!-- ここまでヘッダ -->
        
		  <div class="tsl05-02_MainOutFrame">
		    <div class="tsl05-02_Main">
              <div class="tsl05-02_ImgBox">
                <div class="tsl05-02_Title01"><asp:Literal ID="StaticCalleeTitle" runat="server" />
                  <div class="borderBottomLine"></div></div>
                <div class="tsl05-02_Img01">
                  <div class="Text01"><asp:Literal ID="StaticMainNumberFront" runat="server" /></div>
                  <div class="NumbersBoxBG01"></div>
                  <div id="MainNumber" class="NumbersBox NumbersBoxSize01 NumbersBoxP01">2</div>
                  <div class="Text02"><asp:Literal ID="StaticMainNumberBack" runat="server" /></div>
                </div>
                <div class="tsl05-02_Img02">
                  <div class="NumbersBoxBG01"></div>
                  <div id="MainPlace" class="NumbersBox NumbersBoxSize01 NumbersBoxP01">受付</div>
                  <div class="Text02"><asp:Literal ID="StaticMainPlaceBack" runat="server" /></div>
                </div>
                <div class="tsl05-02_Img03">
                  <div class="NumbersBoxBG01"></div>
                  <div id="MainSa" class="NumbersBox NumbersBoxSize01 NumbersBoxP01">Michael J</div>
                  <div class="Text02"><asp:Literal ID="StaticMainSaNameBack" runat="server" /></div>
                </div>
              </div>
		      <div class="tsl05-02_Img04">
            <div class="Text01"><asp:Literal ID="StaticWaitNumberTitle" runat="server" />
              <div class="borderBottomLine"></div></div>
            <div class="Text02"><asp:Literal ID="StaticWaitNumberFront" runat="server" /></div>
            <div class="NumbersBoxBG01"></div>
            <div id="WaitNumber" class="NumbersBox NumbersBoxSize01 NumbersBoxP01">15</div>
            <div class="Text03"><asp:Literal ID="StaticWaitNumberBack" runat="server" /></div>
          </div>
		      <div class="tsl05-02_Img05">
                <div class="tsl05-02_Title02"><asp:Literal ID="StaticHistoryTitle" runat="server" />
                  <div class="borderBottomLine"></div></div>
		        <div class="BodyData">
            	<div class="myTitle01"><asp:Literal ID="StaticHistoryNumber" runat="server" /></div>
            	<div class="myTitle02"><asp:Literal ID="StaticHistoryPlace" runat="server" /></div>
            	<dl class="myListe">
              	<dd><div class="NumberT">1</div><div class="Location">カウンター5</div></dd>
              	<dd><div class="NumberT">2</div><div class="Location">カウンター5</div></dd>
              	<dd><div class="NumberT">3</div><div class="Location">カウンター5</div></dd>
              	<dd><div class="NumberT">4</div><div class="Location">カウンター5</div></dd>
              	<dd><div class="NumberT">5</div><div class="Location">カウンター5</div></dd>
              	<dd><div class="NumberT">6</div><div class="Location">カウンター5</div></dd>
              </dl>
            </div>
          </div>
		    </div>
    <!--<div class="tsl05Footer">
    	<h5>《お知らせ》　オイルキャンペーン実施中！</h5>
    </div>-->
		  </div>

<!-- とりおき
			<div class="tsl05-02_MainOutFrame">
		    <div class="tsl05-02_Main">
		      <div class="tsl05-02_Img01"><img src="images/tsl05-02_img01.png" width="1096" height="185"></div>
		      <div class="tsl05-02_Img02"><img src="images/tsl05-02_img02.png" width="1096" height="199"></div>
		      <div class="tsl05-02_Img03"><img src="images/tsl05-02_img03.png" width="1096" height="199"></div>
		      <div class="tsl05-02_Img04"><img src="images/tsl05-02_img04.png" width="935" height="222"></div>
		      <div class="tsl05-02_Img05"><img src="images/tsl05-02_img05.png" width="526" height="852"></div>
		    </div>
		  </div>

-->
		<!-- ここからフッタ -->
		<!-- div class="footer">
        </div -->
		<!-- ここまでフッタ -->

		</div>
		<!-- ここまでメインブロック -->
    </form>
    <div id="result">
    </div>
</body>
</html>
