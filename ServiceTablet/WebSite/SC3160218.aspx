<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3160218.aspx.vb" Inherits="Pages_SC3160218" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exterior Check</title>
    <%-- スタイルシート(画面固有) --%>
    <link href="Styles/SC3160218/common.css" rel="stylesheet" type="text/css" media="screen,print" />
    <link href="Styles/SC3160218/contents.css" rel="stylesheet" type="text/css" media="screen,print" />
    <link href="Styles/SC3160218/s-sa.css" rel="stylesheet" type="text/css" media="screen,print" />
    <link href="Styles/SC3160218/s-sa-06.css" rel="stylesheet" type="text/css" media="screen,print" />
    <%-- スクリプトライブラリ --%>
    <script src="Scripts/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="Scripts/icropScript.js?201311250001" type="text/javascript"></script>
    <%-- スクリプト(画面固有) --%>
    <script type="text/javascript" src="Scripts/SC3160218/SC3160218.js?201311300001"></script>
    <script type="text/javascript">
        $(function () {
            // 外観チェック画面初期処理
            readySC3160218();
        });
    </script>
</head>
<body id="SC3160218" style="overflow:hidden">
<form id="form1" runat="server">
     <div id="mainblock">
			<div class="mainblockWrap">

				<div id="mainblockContent">
					<div class="mainblockContentArea">
                    	<div class="mainblockContentAreaWrap">
						<!-- ここから外観チェック(右) -->
						<div class="S-SA-06Right">
							<h2 class="contentTitle"><asp:Literal ID="StaticPageTitle" runat="server" /></h2>

							
							<div class="S-SA-06Right1-1 S-SA-06Right1-1b"></div>
							<ul class="S-SA-06Right1-2 S-SA-06Right1-2b">
  	        <li id="FSkirt"><a href="#"></a></li>
  	        <li id="FrontBumper"><a href="#"></a></li>
  	        <li id="Grill"><a href="#"></a></li>
  	        <li id="Hood"><a href="#"></a></li>
  	        <li id="FrontWindow"><a href="#"></a></li>
  	        <li id="RightFender"><a href="#"></a></li>
  	        <li id="RightFrontDoor"><a href="#"></a></li>
  	        <li id="RightRearDoor"><a href="#"></a></li>
  	        <li id="RightQuarter"><a href="#"></a></li>
  	        <li id="RightLocker"><a href="#"></a></li>
  	        <li id="RightFrontPillar"><a href="#"></a></li>
  	        <li id="RightFrontSideWindow"><a href="#"></a></li>
  	        <li id="RightCenterPillar"><a href="#"></a></li>
  	        <li id="RightRearSideWindow" ><a href="#"></a></li>
  	        <li id="RightRearPillar"><a href="#"></a></li>
  	        <li id="Roof"><a href="#"></a></li>
  	        <li id="LeftFender"><a href="#"></a></li>
  	        <li id="LeftFrontDoor"><a href="#"></a></li>
  	        <li id="LeftRearDoor"><a href="#"></a></li>
  	        <li id="LeftQuarter"><a href="#"></a></li>
  	        <li id="LeftLocker"><a href="#"></a></li>
  	        <li id="LeftFrontPillar"><a href="#"></a></li>
  	        <li id="LeftFrontSideWindow"><a href="#"></a></li>
  	        <li id="LeftCenterPillar"><a href="#"></a></li>
  	        <li id="LeftRearSideWindow"><a href="#"></a></li>
  	        <li id="LeftRearPillar"><a href="#"></a></li>
  	        <li id="RearWindow"><a href="#"></a></li>
  	        <li id="Trunk"><a href="#"></a></li>
  	        <li id="BackPanel"><a href="#"></a></li>
  	        <li id="RearBumper"><a href="#"></a></li>
  	        <li id="RearSkirt"><a href="#"></a></li>
  	        <li id="RightFrontWheel"><a href="#"></a></li>
  	        <li id="RightRearWheel"><a href="#"></a></li>
  	        <li id="LeftFrontWheel"><a href="#"></a></li>
  	        <li id="LeftRearWheel"><a href="#"></a></li>
  	        <li id="SpareWheel"><a href="#"></a></li>
							</ul>
							<div class="KSMainblockCheckIconContents">
							</div>
							<div class="KSMainblockCheckExplodedViewBox">
  	                            <p class="KSCheckInjuryBox"><asp:Literal ID="StaticNoDamage" runat="server" /><input type="checkbox" name="Check1" value="true" id="chkNoDamage" class="KSCheckInjuryBoxChecked" /></p>
  	                            <p class="KSCheckInjuryBox2"><asp:Literal ID="StaticCanNotCheck" runat="server" /><input type="checkbox" name="Check2" value="true" id="chkCanNotCheck" class="KSCheckInjuryBoxChecked" /></p>
							</div>
						</div>
				</div>
			</div>
			</div>
				<!-- ここまでメインコンテンツ -->
			</div>
		</div>
</form>
<!--
<div id="debug">
</div>
-->
</body>
</html>
