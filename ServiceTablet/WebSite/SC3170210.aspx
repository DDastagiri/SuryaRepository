<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3170210.aspx.vb" Inherits="Pages_SC3170210" %>

<!--<!DOCTYPE html>-->
<!DOCTYPE html>

<html>
<head runat="server">
	<title>SC3170210</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<meta name="format-detection" content="telephone=no" />

	<!-- スタイルシート -->
	<link rel="stylesheet" href="./Styles/SC3170210/common.css?20140214000000" type="text/css" media="screen,print" />
	<link rel="stylesheet" href="./Styles/SC3170210/contents.css?20140214000000" type="text/css" media="screen,print" />
	<link rel="stylesheet" href="./Styles/SC3170210/photoWindow.css?20190619000001" type="text/css" media="screen,print" />
	<link rel="stylesheet" href="./Styles/SC3170210/SC3170210.css?20140214000000" type="text/css" media="screen,print" />

	<!-- スクリプト -->
	<script type="text/javascript" src="./Scripts/jquery-1.5.2.min.js?20140214000000" ></script>
	<script type="text/javascript" src="./Scripts/jquery.fingerscroll.js?20140214000000"></script>
	<script type="text/javascript" src="./Scripts/icropScript.js?20140214000000" ></script>

	<script type="text/javascript" src="./Scripts/SC3170210/jquery.lazyload.min.js?20140214000000"></script>
	<script type="text/javascript" src="./Scripts/SC3170210/SC3170210.js?20190920000000" ></script>
</head>
<body>
	<!-- ここからメインブロック -->
	<div id="mainblock">
		<div class="mainblockWrap">
			<div id="mainblockContent">
				<div class="mainblockContentArea">
				</div>
			</div>
			<!--　カメラウインドココから　-->
			<div class="BlackOver">
				<div class="mainblockContentArea">
					<div class="MainView">
						<div id="TargetImage"></div>
					</div>
					<div class="ThumbnailView">
						<ul class="ThumbnailPhotos">
						</ul>
					</div>
					<div class="pointBtn01"><div class="CancelBtn"><asp:Literal ID="StaticCancelBtn" runat="server" /></div></div>
					<div class="pointBtn02"><div class="RegistrationBtn"><asp:Literal ID="StaticRegistrationBtn" runat="server" /></div></div>
					<div class="pointBtn03"><div class="DeleteBtn"><asp:Literal ID="StaticDeleteBtn" runat="server" /></div></div>
					<div class="mainblockDialogBoxClose"></div>
				</div>
			</div>
			<!--　カメラウインドココまで　-->
		</div>
	</div>
		<!-- ここまでメインブロック -->
	<p id="debugArea"></p>
	<form id="form1" runat="server">
	</form>
</body>
</html>
