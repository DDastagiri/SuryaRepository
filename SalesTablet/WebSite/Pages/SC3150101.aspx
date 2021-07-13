<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3150101.aspx.vb" Inherits="Pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%'スタイルシート %>
<link rel="Stylesheet" href="../Styles/SC3150101/SC3150101.css?201202271800" type="text/css" media="screen,print" />
<link rel="Stylesheet" href="../Styles/SC3150101/Chips.css" type="text/css" media="screen,print" />
<script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Main.js?201202231530"></script>
<script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Chip.js?201202271730"></script>
<script type="text/javascript" src="../Scripts/SC3150101/SC3150101.Meter.js?201202161940"></script>
<script type="text/javascript" src="../Scripts/SC3150101/SC3150101.flickable.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <!--<div id="BaseBox">--><!--　←サイズ確認用のタグです -->
	<!--<div id="container">--><!--　←全体を含むタグです。　-->

		<!-- 中央部分-->
		<!--<div id="main">-->
		<!-- ここからコンテンツ -->
			<!--<div id="contents">-->
<!-- BaseBox ～ contents までのタグは、Maseter の head に含まれる -->
				<div id="contentsFrame" class="contentsFrame">
                    <asp:HiddenField ID="HiddenJsonData" runat="server" />
                    <asp:HiddenField ID="HiddenFieldOrderNo" runat="server" />
                    <asp:HiddenField ID="HiddenFieldChildNo" runat="server" />
                    <asp:HiddenField ID="HiddenFieldRepairOrderFilter" runat="server" />
                    <asp:HiddenField ID="HiddenFieldRepairOrderIcon" runat="server" />
                    <asp:HiddenField ID="HiddenWarnNextDate" runat="server" />
                    <asp:HiddenField ID="HiddenServerTime" runat="server" />
                    <asp:HiddenField ID="HiddenStartTimeWord" runat="server" />
                    <asp:HiddenField ID="HiddenEndTimeWord" runat="server" />
                    <asp:HiddenField ID="HiddenResultStartTimeWord" runat="server" />
                    <asp:HiddenField ID="HiddenResultEndTimeWord" runat="server" />
                    <asp:HiddenField ID="HiddenPopupPartsCancelWord" runat="server" />
                    <asp:HiddenField ID="HiddenPopupPartsTitleWord" runat="server" />
                    <asp:HiddenField ID="HiddenReloadFlag" runat="server" />
                  <!-- ここからストール情報 -->
                  <div class="stc01Box01">
                  <asp:HiddenField ID="HiddenRestText" runat="server" />
                  <asp:HiddenField ID="HiddenUnavailableText" runat="server" />
                  <asp:HiddenField ID="HiddenCandidateId" runat="server" />
                  <asp:HiddenField ID="HiddenStallStartTime" runat="server" />
                  <asp:HiddenField ID="HiddenStallEndTime" runat="server" />
                  <asp:HiddenField ID="HiddenScrollLeft" runat="server" />
                  <asp:HiddenField ID="HiddenSelectedId" runat="server" />
                  <asp:HiddenField ID="HiddenSelectedChip" runat="server" Value="0" />
                  <asp:HiddenField ID="HiddenSelectedReserveId" runat="server" />
                  <asp:HiddenField ID="HiddenOrderStatus" runat="server" />
                  <asp:HiddenField ID="HiddenHistoryOrderNumber" runat="server" />
                  <!-- JavaScriptより動作させるため、非表示とするボタン -->
                  <asp:Button ID="HiddenButtonFlickRepairOrder" runat="server" CssClass="HiddenButton" />
                  <asp:Button ID="HiddenButtonRepairOrderIcon" runat="server" CssClass="HiddenButton" />
                  <asp:Button ID="HiddenButtonChipTap" runat="server" CssClass="HiddenButton" />
                  <asp:Button ID="HiddenButtonRefresh" runat="server" CssClass="HiddenButton" />
                  <asp:Button ID="HiddenButtonHistory" runat="server" CssClass="HiddenButton" />
                  <%--<asp:HiddenField ID="HiddenPostBack" runat="server" />--%>
                    <div class="Box01In">
                      <div class="Box01Title">
                          <asp:Label ID="LabelStallName" runat="server" ></asp:Label>
                      </div>
                      <div class="Box01Title02">
                          <%--<asp:Label ID="LabelAuthorityName" runat="server" Text=""></asp:Label>--%>
                          <icrop:CustomLabel ID="CustomerLiteral2" runat="server" TextWordNo="2" />
                          &nbsp;&nbsp;
                          <asp:Label ID="LabelEngineerName" runat="server" ></asp:Label>
                      </div>
                      <div class="Box01GraphBox" id="Box01GraphBox">

                        <div class="Box01GraphLine" id="Box01GraphLine">
                            <div class="Box01GraphLineFilter" id="Box01GraphLineFilter"></div>
                        </div><!--Box01GraphLine-->

                        <div class="CurrentBox" id="CurrentBox">
                      	  <div class="CurrentBoxTime" id="CurrentBoxTime"></div>
                      	</div>

                      </div><!--Box01GraphBox-->
                    </div><!--Box01In-->
                  </div><!--contentsFrame-->
                  <!-- ここまでストール情報 -->
                  <!-- ここから作業進捗情報 -->
                  <div class="stc01Box02">
                    <h2 class="contentTitle">
                        <%--<asp:Label ID="LabelTaskProgressTitle" runat="server"></asp:Label>--%>
                        <icrop:CustomLabel ID="CustomerLiteral4" runat="server" TextWordNo="4" />
                    </h2>
                    <div class="Box02In">
                      <div class="TimeBox">
                        <%--<icrop:CustomLabel ID="CustomerLiteral7" runat="server" TextWordNo="7" />&nbsp;<strong><asp:Label ID="LabelMeterStartTime" runat="server"></asp:Label></strong><br/>
                        <icrop:CustomLabel ID="CustomerLiteral8" runat="server" TextWordNo="8" />&nbsp;<strong><asp:Label ID="LabelMeterEndTime" runat="server"></asp:Label></strong>--%>
                        <asp:Label ID="LiteralStartTimeText" runat="server" ></asp:Label>&nbsp;<strong><asp:Label ID="LabelMeterStartTime" runat="server"></asp:Label></strong><br/>
                        <asp:Label ID="LiteralEndTimeText" runat="server" ></asp:Label>&nbsp;<strong><asp:Label ID="LabelMeterEndTime" runat="server"></asp:Label></strong>
                      </div>
                      <span>
                        <%--<asp:Label ID="LabelRONumberTitle" runat="server"></asp:Label>--%>
                         <icrop:CustomLabel ID="CustomerLiteral5" runat="server" TextWordNo="5" />
                      </span>
                      &nbsp;
                      <span><asp:Label ID="LabelRONumber" runat="server"></asp:Label></span><br />
                      <span>
                        <%--<asp:Label ID="LabelChargeSATitle" runat="server"></asp:Label>--%>
                         <icrop:CustomLabel ID="CustomerLiteral6" runat="server" TextWordNo="6" />
                      </span>
                      &nbsp;
                      <span><asp:Label ID="LabelChargeSA" runat="server"></asp:Label></span>
                    <div class="Meter">
                      <div class="MeterColor" id="MeterColor"></div>
                      <p class="MeterStartText">
                        <%--<asp:Label ID="LabelMeterStartText" runat="server" Text=""></asp:Label>--%>
                        <icrop:CustomLabel ID="CustomerLiteral9" runat="server" TextWordNo="9" />
                      </p>
                      <p class="MeterCompletionText">
                        <%--<asp:Label ID="LabelMeterEndText" runat="server" Text=""></asp:Label>--%>
                        <icrop:CustomLabel ID="CustomerLiteral10" runat="server" TextWordNo="10" />
                      </p>
                    </div>
                    </div>
                    <!-- グレーフィルター -->
                    <div id="stc02Box02Filter" class="stc02Box02Filter"></div>
                    <!-- ここまでグレーフィルター -->
                  </div>
                  <!-- ここまで作業進捗情報 -->
                  
                  <!-- R/O情報パネル -->

                 <div id="roInfomationBox">
                     <iframe id="stc01Box03" class="stc01Box03" src="SC3150102.aspx" name="stc01Box03" scrolling="no" seamless="seamless"></iframe>
                     <%'読み込み中アイコン %>
                     <div id="loadingroInfomation"></div>
                 </div>
                  <%--<div class="stc01Box03" ></div>--%>
                  <asp:HiddenField ID="HiddenPartsComp" runat="server" />
                  <asp:HiddenField ID="HiddenPartsCount" runat="server" />
                  <asp:HiddenField ID="HiddenBackOrderCount" runat="server" />
                  <asp:HiddenField ID="HiddenSelectedTabNumber" runat="server" />

                  <!--<div class="stc01Box03">-->
                  <!-- ここから基本情報・ご用命事項・作業内容パネル-->
                  <!-- ここまで基本情報・ご用命事項・作業内容パネル-->
                  <!--</div>-->
                  <!-- ここまでstc01Box03 -->

                </div>
				<!-- 右カラム -->
            <!--</div>-->
		<!-- ここまでコンテンツ -->
	    <!--</div>-->
        <!-- ここまで中央部分 -->
	<!--</div>--><!--　←全体を含むタグ終わり　-->
<!--</div>--><!--　←サイズ確認用のタグ終わり　-->

    <asp:HiddenField ID="HiddenPushedFooter" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenBreakPopup" runat="server" Value="0" />
    <div id="tcvNsc31Main" style="display:none">
    <div class="tcvNsc31Black" id="tcvNsc31Black" style="display:none"></div>
    <div class="popWind" id="popWind" >
		<div class="PopUpBtn01">
			<ul>
				<li class="buttonC" onclick="confirm(true);"><a href="#"><icrop:CustomLabel ID="CustomLabelBreakCancel" runat="server" TextWordNo="22" /></a></li>
				<li class="title"><icrop:CustomLabel ID="CustomLabelBreakTitle" runat="server" TextWordNo="21" /></li>
			</ul>
		</div>
		<div class="dataWind1">
			<div class="TextBox">
				<div class="TextBoxIn">
					<ul>
						<li class="ListTitle"><icrop:CustomButton ID="ButtonDoNotBreak" class="ListTitleButton" runat="server" TextWordNo="23" /></li>
						<li class="ListDate"><icrop:CustomButton ID="ButtonTakeBreak" class="DateTitleButton" runat="server" TextWordNo="24" /></li>
					</ul>
				</div>
			</div>
		</div>
		<div class="baseWind1">
			<div class="boxBoder">
			</div>
			<div class="box">&nbsp;</div>
		</div>
	</div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <div ID="FooterCustomButton" style="float:right; margin-right:20px;">
        <!-- icrop:CustomButton にてhight,widthが設定されなくなったため、とりあえず直にボタンの幅・高さを設定しています. -->
        <%--<icrop:CustomButton ID="ButtonConnectParts" runat="server" class="footerCustomButton_ConnectParts" TextWordNo="13" />--%>

        <button ID="ButtonConnectParts" class="footerCustomButton_ConnectParts" onclick="FooterButtonClick('ButtonConnectParts');"/>
            <icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="13" />
        </button>
        <icrop:CustomButton ID="ButtonSuspendWork" runat="server" class="footerCustomButton_SuspendWork" TextWordNo="15" OnClientClick="return FooterButtonClick('ButtonSuspendWork');" />
        <icrop:CustomButton ID="ButtonStartCheck" runat="server" class="footerCustomButton_StartCheck" TextWordNo="14" OnClientClick="return FooterButtonClick('ButtonStartCheck');" />
        <icrop:CustomButton ID="ButtonStartWork" runat="server" class="footerCustomButton_StartWork" TextWordNo="12" OnClientClick="return FooterButtonClick('ButtonStartWork');" />
    </div>
    <div style="clear:right;">
    </div>
</asp:Content>
