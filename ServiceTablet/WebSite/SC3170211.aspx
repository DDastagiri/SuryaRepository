<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3170211.aspx.vb" Inherits="Pages_SC3170211" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8" />

    <title></title>
    <!-- 基本CSS -->
    <link rel="stylesheet" href="./Styles/SC3170211/common.css?20140211180000" type="text/css" media="screen,print" />
    <!-- ページ毎CSS -->
    <link rel="stylesheet" href="./Styles/SC3170211/s-sa.css?20140211180000" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="./Styles/SC3170211/s-sa0605.css?20140211180000" type="text/css" media="screen,print" />

    <script type="text/javascript" src="Scripts/jquery-1.5.2.min.js?20140211180000"></script>
    <script type="text/javascript" src="Scripts/icropScript.js?20140211180000" ></script>
    <script type="text/javascript" src="Scripts/SC3170211/SC3170211.js?20140211180000"></script>
</head>

<body style="margin:0px 0px;">
    <!-- 中央部分-->
    <div id="main">


	    <!-- ここからコンテンツ -->
	    <div id="contents"><!-- 右カラム -->
	      <div class="Header"><h1><asp:Label ID="TitleLabel" runat="server" Align="center" Text="Window Title" ></asp:Label></h1>
	        <div class="PhotoButton"><p></p></div>
	        <div class="DoneButton"><p><asp:Label ID="StaticCaptionCloseButton" runat="server" Align="center" Text="Window Title" ></asp:Label></p></div>
	      </div>
          <img id="Img_Photo" runat="server" width="1024" height="768" />
        </div>
	    <!-- ここまでコンテンツ -->
    </div>
    <!-- ここまで中央部分 -->

    <form id="Form1" runat="server">
        <!-- 隠し項目 -->
        <asp:HiddenField id="Hidden_Title" runat="server" />
        <asp:HiddenField id="Hidden_FileName" runat="server" />
        <asp:HiddenField id="Hidden_PictURL" runat="server" />
        <asp:HiddenField id="Hidden_Mode" runat="server" />
        <asp:HiddenField id="Hidden_CameraFilePath" runat="server" />
    </form>
</body>
</html>
