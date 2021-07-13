<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3240701.ascx.vb" Inherits="Pages_SC3240701" %>

<!-- スクリプトファイルの参照は圧縮済ファイルを対象とする -->
<script type="text/javascript" src="../Scripts/SC3240701/SC3240701.min.js?20170913000000"></script>
<script type="text/javascript" src="../Scripts/SC3240701/SC3240701.Event.min.js?20170913000000"></script>
<script type="text/javascript" src="../Scripts/SC3240701/SC3240701.Define.min.js?20170913000000"></script>
<link rel="Stylesheet" href="../Styles/SC3240701/SC3240701.css?20170913000000" type="text/css" media="screen,print" />

<%-- ストール使用不可画面 Start--%>
<div id="UnavailableSettingPopup" style="display:none;">
	<div id="UnavailableSettingPopupContent" class="UnavailablePopStyle">
		<div class="Balloon">
			<div id="UnavailableBorderBoxDiv" class="borderBox">
				<div class="Arrow">&nbsp;</div>
				<div id="UnavailableMyDataBoxDiv" class="myDataBox">&nbsp;</div>
			</div>
			<div id="UnavailableGradationBoxDiv" class="gradationBox">
				<div id="UnavailableArrowMask" class="ArrowMask">
					<div class="Arrow">&nbsp;</div>
				</div>
				<div id="UnavailableSettingNscPopUpHeaderBgDiv" class="scNscPopUpHeaderBg">&nbsp;</div>
				<div id="UnavailableSettingNscPopUpDataBgDiv" class="scNscPopUpDataBg">&nbsp;</div>
			</div>
		</div>

        <div id="UnavailableDetailOverShadowDiv" class="OverShadow">&nbsp;</div>

        <%--アクティブインジケータ--%>
        <div id="UnavailableActiveIndicator"></div>

        <%--ヘッダー Start--%>
		<div id="UnavailableSettingHeaderDiv" class="PopUpHeader">

            <%--ヘッダー左--%>
            <div id="UnavailableLeftBtnDiv" runat="server" class="LeftBtn">
                <asp:Button ID="UnavailableCancelBtn" runat="server" OnClientClick="return UnavailableCancelButton();"/>
            </div>

			<%--ヘッダー中央--%>
            <h3>
                <icrop:CustomLabel runat="server" ID="UnavailableHeaderLabel" CssClass="UnavailableEllipsis" Width="175px"></icrop:CustomLabel>
            </h3>

            <%--ヘッダー右--%>
            <div id="UnavailableRightBtnDiv" runat="server" class="RightBtn">
                <asp:Button ID="UnavailableRegisterBtn" runat="server" OnClientClick="return UnavailableRegisterButton();"/>
            </div>
		</div>
        <%--ヘッダー End--%>

        <div style="clear:both;"></div>

        <%--詳細エリア Start--%>
		<div id="UnavailableSettingDetail" class="dataBox">
            <%--詳細エリア コンテンツ Start--%>
			<div id="UnavailableSettingDetailContent" class="UnavailableInnerDataBox">
                <div class="UnavailableInnerDataBox02">
                    <div>
                    <%--使用不可時間エリア--%>
	                    <ul id="IdleDateTimeUl" runat="server" class="IdleDateTimeTable">
		                    <li><%--開始予定日時--%>
			                    <dl>
				                    <dt>
					                    <icrop:CustomLabel runat="server" ID="StartDateTimeWordLabel" Width="65px" CssClass="UnavailableEllipsis"></icrop:CustomLabel>
				                    </dt>
				                    <dd>
					                    <icrop:CustomLabel runat="server" ID="StartIdleDateTimeLabel" Width="250px" CssClass="UnavailableEllipsis"></icrop:CustomLabel>
					                    <icrop:DateTimeSelector ID="StartIdleDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
				                    </dd>
			                    </dl>
		                    </li><%--開始予定日時 End--%>
		                    <li><%--終了予定日時--%>
			                    <dl>
				                    <dt>
					                    <icrop:CustomLabel runat="server" ID="FinishDateTimeWordLabel" Width="65px" CssClass="UnavailableEllipsis"></icrop:CustomLabel>
				                    </dt>
				                    <dd>
					                    <icrop:CustomLabel runat="server" ID="FinishIdleDateTimeLabel" Width="250px" CssClass="UnavailableEllipsis"></icrop:CustomLabel>
					                    <icrop:DateTimeSelector ID="FinishIdleDateTimeSelector" runat="server" Format="DateTime" style="position: relative; top: -29px; width: 250px; height: 29px; opacity: 0;"/>
				                    </dd>
			                    </dl>
		                    </li><%--開始予定日時End--%>
	                    </ul><%--使用不可時間エリア End--%>
                        <%--メモエリア--%>
		                <ul runat="server" id="IdleMemoUl" class="IdleMemo">
			                <li>
				                <dl>
					                <dt id="IdleMemoDt">
						                <icrop:CustomLabel runat="server" ID="IdleMemoLabel" Width="60px" CssClass="UnavailableEllipsis"></icrop:CustomLabel>
					                </dt>
					                <dd>
						                <div class="TextareaBox">
							                <asp:TextBox ID="IdleMemoTxt" runat="server" TextMode="MultiLine" Width="255px" maxlen="600"></asp:TextBox>
						                </div>
					                </dd>
				                </dl>
			                </li>
			                <li style="clear:both;"></li>
		                </ul><%--メモエリア End--%>
                        <div style="height:10px; clear:both;"></div>
                    </div>
                </div><%--UnavailableInnerDataBox02 End--%>
            </div><%--UnavailableInnerDataBox End--%>
        </div><%--UnavailableSetting End--%>
        <div id="SC3240701HiddenArea">
			<div id="SC3240701HiddenContents">
                <asp:HiddenField runat="server" ID="IdleTimeHidden"/> <%--使用不可時間--%>
			</div>
		</div>
    </div><%--UnavailablePopupContent End--%>
</div><%--UnavailableChipPopup End--%>