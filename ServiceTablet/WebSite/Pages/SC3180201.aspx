<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3180201.aspx.vb" Inherits="Pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../Styles/SC3180201/SC3180201.css?20191024000000" type="text/css" media="screen,print" />

    <script type="text/javascript" src="../Scripts/SC3180201/SC3180201.Main.js?20191024000000"></script>
    <script type="text/javascript" language="javascript">

        $("document").ready(function () {

        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

        <input type="hidden" id="R_O" name="R_O" value="<%= roNum%>"/>
        <input type="hidden" id="VIN_NO" name="VIN_NO" value="<%= vin%>"/>
        <input type="hidden" id="ViewMode" name="ViewMode" value="<%= viewMode%>"/>
        <input type="hidden" id="JOB_DTL_ID" name="JOB_DTL_ID" value="<%= jobDtlId%>"/>
        <input type="hidden" id="BASREZID" name="BASREZID" value="<%= basrezId%>"/>

        <input type="hidden" id="BeforeText" name="BeforeText" value="<%= BeforeText%>"/>
        <input type="hidden" id="AfterText" name="AfterText" value="<%= AfterText%>"/>
        <input type="hidden" id="ServiceID" name="ServiceID" value="<%= svcinId%>"/>

        <input type="hidden" id="SRV_RowLockVer" name="SRV_RowLockVer" value="<%= svcinRowLockVersion%>"/>
        <input type="hidden" id="RO_RowLockVer" name="RO_RowLockVer" value="<%= roRowLockVersion%>"/>

        <asp:HiddenField ID="VehicleChartNo" runat="server" />
        <asp:HiddenField ID="UserName" runat="server" />
        <asp:HiddenField ID="FromFMMain" runat="server" />
        <asp:HiddenField ID="TextEditedFlg" runat="server" />
        <asp:HiddenField ID="ItemCheckErrorMessage" runat="server" />
        <asp:HiddenField ID="ErrorFlg" runat="server" />
        <asp:HiddenField ID="EditedMessage" runat="server" />

        <asp:HiddenField ID="EngineRoomCheckCount" runat="server" />
        <asp:HiddenField ID="InroomCheckCount" runat="server" />
        <asp:HiddenField ID="LeftCheckCount" runat="server" />
        <asp:HiddenField ID="RightCheckCount" runat="server" />
        <asp:HiddenField ID="UnderCheckCount" runat="server" />
        <asp:HiddenField ID="TrunkCheckCount" runat="server" />
        <asp:HiddenField ID="MaintenanceCheckCount" runat="server" />
        <asp:HiddenField ID="hdnErrorMsg" runat="server" />
        <asp:HiddenField ID="hdnWarningMsg" runat="server" />
        <!-- 2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応  Start -->
        <asp:HiddenField ID="overText" runat="server" />
        <!-- 2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応  End -->


		<!-- ここからメインブロック -->
<!--
  		<div id="mainblock">
-->
		  <div class="mainblockWrap">
		    <div id="mainblockContent">
		      <div class="mainblockContentArea">
		        <div class="mainblockContentAreaWrap">
		          <!--ここから登録情報-->
		          <div id="mainblockRegistdata">
		            <div class="mainblockRegistdataWrapper">
		              <dl class="mainblockRegistdata1">
		                <dt class="mainblockRegistdata1-1"><icrop:CustomLabel ID="CustomLabel3" runat="server" UseEllipsis="true" Width="50" TextWordNo="1"/></dt>
		                <dd><icrop:CustomLabel ID="RegisterNoLabel" runat="server" UseEllipsis="true" Width="80" ></icrop:CustomLabel></dd>
		                <dt><icrop:CustomLabel ID="CustomLabel1" runat="server" UseEllipsis="true" Width="50" TextWordNo="2"/></dt>
		                <dd><icrop:CustomLabel ID="OrderNoLabel" runat="server" UseEllipsis="true" Width="80" style=" margin-left:3px;"></icrop:CustomLabel></dd>
		                <dt><icrop:CustomLabel ID="CustomLabel2" runat="server" UseEllipsis="true" Width="50" TextWordNo="3"/></dt>
		                <dd><icrop:CustomLabel ID="BuyerNameLabel" runat="server" UseEllipsis="true" Width="80"></icrop:CustomLabel></dd>
	                  </dl>
		              <dl class="mainblockRegistdata22">
		                <dt class="mainblockRegistdata22-1"><icrop:CustomLabel ID="CustomLabel29" runat="server" UseEllipsis="true" Width="100" TextWordNo="36"/></dt>
		                <dd><span class="IcoReserve"<%=amarkView%>><icrop:CustomLabel ID="AMark" runat="server" ></icrop:CustomLabel></span></dd>
		                <dt class="mainblockRegistdata22-2"><icrop:CustomLabel ID="ContactPersonNameLabel" runat="server" UseEllipsis="true" Width="160"></icrop:CustomLabel></dt>
		                <dt class="mainblockRegistdata22-3"><icrop:CustomLabel ID="ContactPersonTelLabel" runat="server" UseEllipsis="true" Width="160"></icrop:CustomLabel></dt>
	                  </dl>
		              <dl class="mainblockRegistdata3">
		                <dt class="mainblockRegistdata3-1"><icrop:CustomLabel ID="CustomLabel4" runat="server" UseEllipsis="true" Width="43" TextWordNo="4"/></dt>
                        <dd><icrop:CustomLabel ID="Series1Label" runat="server" UseEllipsis="true" Width="60"></icrop:CustomLabel></dd>
		                <dt><icrop:CustomLabel ID="CustomLabel5" runat="server" UseEllipsis="true" Width="43" TextWordNo="5"/></dt>
		                <dd><icrop:CustomLabel ID="VINLabel" runat="server" UseEllipsis="true" Width="140" ></icrop:CustomLabel></dd>
		                <dt><icrop:CustomLabel ID="CustomLabel6" runat="server" UseEllipsis="true" Width="85" TextWordNo="6"/></dt>
		                <dd><icrop:CustomLabel ID="DeliveryDate" runat="server" UseEllipsis="true" Width="113" ></icrop:CustomLabel></dd>
	                  </dl>
		              <div class="mainblockRegistdata4">
		                <p class="mainblockRegistdata4Name"><icrop:CustomLabel ID="Series2Label" runat="server" UseEllipsis="true" Width="130"></icrop:CustomLabel></p>
		                <p class="mainblockRegistdata4Time"><icrop:CustomLabel ID="CustomLabel7" runat="server" UseEllipsis="true" Width="149" TextWordNo="7"/></p>
		                <p class="mainblockRegistdata4Date">
		                  <time datetime="2011-11-16"><icrop:CustomLabel ID="ReceptionTimeLabel" runat="server" UseEllipsis="true" Width="100" ></icrop:CustomLabel></time>
	                    </p>
	                  </div>
		              <div class="mainblockRegistdata5">
		                <p><span class="mainblockRegistdata5-1"><icrop:CustomLabel ID="CustomLabel8" runat="server" UseEllipsis="true" Width="140" TextWordNo="8"/></span>
                          <icrop:CustomLabel ID="ScheDeliDate" runat="server"></icrop:CustomLabel>
	                    </p>
		                <p>&nbsp;</p>
	                  </div>
	                </div>
		          </div>
		          <!--ここまで登録情報-->
		          <div class="SCT03BlockLeftWork">
		            <!--作業項目-->
		            <h2><icrop:CustomLabel ID="CustomLabel22" runat="server" TextWordNo="28"/></h2>
		            <div class="SCT03BlockLeftWorkBox">
		              <table border="0" cellspacing="0" cellpadding="0" class="SCT03BlockLeftWorkMain">
		                <tr>
		                  <td><icrop:CustomLabel ID="CustomLabel23" runat="server" TextWordNo="29"/></td>
		                  <td><icrop:CustomLabel ID="CustomLabel24" runat="server" TextWordNo="30"/></td>
		                  <td><icrop:CustomLabel ID="CustomLabel25" runat="server" TextWordNo="31"/></td>
		                  <td><icrop:CustomLabel ID="CustomLabel26" runat="server" TextWordNo="32"/></td>
		                  <td><icrop:CustomLabel ID="CustomLabel27" runat="server" TextWordNo="33"/></td>

	                    </tr>
	                  </table>

		              <div class="SCT03BlockLeftWorkBox2">
<%  intRecCount = 0%>
                        <asp:Repeater ID="InspecCodeList" runat="server" >
                          <ItemTemplate>
<%  intRecCount += 1%>
		                  <table border="0" cellspacing="0" cellpadding="0" class="SCT03BlockLeftWorkSub">
		                    <tr>
		                      <td><%# Server.HtmlEncode(Container.ItemIndex + 1)%></td>
		                      <td><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_CD"))%></td>
		                      <td><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_NAME"))%></td>
		                      <td><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "OPERATION_TYPE_NAME"))%></td>
		                      <td><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "USERNAME"))%></td>
		                    </tr>
	                      </table>
                          </ItemTemplate>
                        </asp:Repeater>
<%  If 4 > intRecCount Then%>
<%  For intIdx As Integer = intRecCount + 1 To 4%>
		                  <table border="0" cellspacing="0" cellpadding="0" class="SCT03BlockLeftWorkSub">
		                    <tr>
		                      <td><%= intIdx%></td>
		                      <td>&nbsp</td>
		                      <td>&nbsp</td>
		                      <td>&nbsp</td>
		                      <td>&nbsp</td>
		                    </tr>
	                      </table>
<%  Next intIdx%>
<%  End If%>
	                  </div>

	                </div>
	              </div>
		          <div class="SCT03BlockLeftAdvice">
		            <!--アドバイス-->
		            <h2><icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="11"/></h2>
		            <div class="SCT03BlockLeftAdviceWrap">
		              <div class="SCT03BlockLeftAdviceMemo">
		                <textarea class="ChangeInput" id="InputStrMsg" name="TechnicianAdvice" onblur="changetext()" ><%= TechnicianAdvice %></textarea>
		                <%-- スクロールの位置がおかしくなるためDivでのスクロールは廃止
		                <div class="AdviceMemo" id="AdviceMemo">
		                <p></p>
                        <div id="TextStrMsg" class="ChangeText" onclick="changeinput();" onload="changetext('<%= TechnicianAdvice%>')"></div>
                        <input type="hidden" name="TechnicianAdvice" id="TechnicianAdvice" value="<%= TechnicianAdvice%>"/>
	                    </div>
		                --%>
	                  </div>
	                </div>
	              </div>
		          <!--ここからメイン-->
		          <div class="SCT03Block">
		            <!--ここから左カラム-->
<table>
<tr><td style="vertical-align: top;">
		            <div class="SCT03BlockLeft">
		              <div class="SCT03BlockLeftCheck">
		                <!--チェック項目-->
		                <h2 style="width:240px;"><icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="9"/></h2>
		                <div class="SCT03BlockLeftCheckWrap">
		                  <ul class="SCT03BlockLeftCheckWrap3-1">
		                    <li class="SCT03BlockLeftCheck1-1"><a style="<%=EngineRoomBtnColor%>" ID="EngineRoomBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=EngineRoomBtnDisabled%> VehicleChartClick(1);"></a></li>
		                    <li class="SCT03BlockLeftCheck1-2"><a style="<%=InroomBtnColor%>" ID="InroomBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=InroomBtnDisabled%> VehicleChartClick(2);"></a></li>
		                    <li class="SCT03BlockLeftCheck1-3"><a style="<%=LeftBtnColor%>" ID="LeftBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=LeftBtnDisabled%> VehicleChartClick(3);"></a></li>
		                    <li class="SCT03BlockLeftCheck1-4"><a style="<%=RightBtnColor%>" ID="RightBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=RightBtnDisabled%> VehicleChartClick(4);"></a></li>
		                    <li class="SCT03BlockLeftCheck1-5"><a style="<%=UnderBtnColor%>" ID="UnderBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=UnderBtnDisabled%> VehicleChartClick(5);"></a></li>
		                    <li class="SCT03BlockLeftCheck1-6"><a style="<%=TrunkBtnColor%>" ID="TrunkBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=TrunkBtnDisabled%> VehicleChartClick(6);"></a></li>
	                      </ul>
		                  <p class="SCT03BlockLeftCheckBase"><img src="../Styles/images/SC3180201/deployment1-1.png" width="214" height="252"></p>
		                  <p class="SCT03BlockLeftCheckInfo"><span class="SCT03BlockLeftCheck1-7"><a style="<%=MaintenanceBtnColor%>" ID="MaintenanceBtn" href="#" onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="<%=MaintenanceBtnDisabled%> VehicleChartClick(0);"></a></span><icrop:CustomLabel ID="CustomLabel10" runat="server" TextWordNo="10"/></p>
	                    </div>
                      </div>
                    </div>
</td>
<td style="width: 10px;"></td>
<td style="vertical-align: top;">
    	            <!--ここから右カラム-->
		            <div class="SCT03BlockRight">
		                <div class="SCT03BlockRightItem">

		                  <h2><icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="12"/></h2>

<!--ここから右カラム(検査項目)-->

		              <div id="OperationItems_Engine">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="EngineRoomLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel15" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Engine">
<% intPosIndex = 1%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Engine" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe200" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                   
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

		              <div id="OperationItems_Inroom">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="InroomLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel17" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel18" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel19" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Inroom">
<% intPosIndex = 2%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Inroom" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe201" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                                            
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

		              <div id="OperationItems_Left">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="LeftLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel20" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel21" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel28" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Left">
<% intPosIndex = 3%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Left" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe202" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                                            
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

		              <div id="OperationItems_Right">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="RightLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel30" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel31" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel32" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Right">
<% intPosIndex = 4%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Right" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe203" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                                            
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

		              <div id="OperationItems_Under">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="UnderLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel33" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel34" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel35" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Under">
<% intPosIndex = 5%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Under" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe204" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                                            
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

		              <div id="OperationItems_Trunk">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="TrunkLabel" runat="server" ></icrop:CustomLabel></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel36" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel37" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"><icrop:CustomLabel ID="CustomLabel38" runat="server" TextWordNo="15"/></p>
	                      </div>

		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Trunk">
<% intPosIndex = 6%>
<% intIndex = 0%>
                        <asp:ListView ID="InspecItemsList_Trunk" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
                            <div id="InspecItemsTitle" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemNameViewStyle"))%>">
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt class="FontBlue"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "INSPEC_ITEM_NAME"))%></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox"></div>
	                          </dd>
		                      <dd class="CompletedJobBox"></dd>
	                        </dl>
	                        </div>
		                    <dl class="STC04BlockRightMain1-1 STC04BRM2 BackColor01" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemViewStyle_Color"))%>">
		                      <dt><strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUB_INSPEC_ITEM_NAME"))%></strong></dt>
		                      <dd class="InspectionResult">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Good"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_1" id="Check<%= intPosIndex%>_<%= intIndex%>_1" value="1" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Good"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_1').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_1" class="Icon1 Good_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Good"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Good"))%>"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="16"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Inspect"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_2" id="Check<%= intPosIndex%>_<%= intIndex%>_2" value="2" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Inspect"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_2').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_2" class="Icon2 Inspect_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Inspect"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Inspect"))%>"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="17"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Replace"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_3" id="Check<%= intPosIndex%>_<%= intIndex%>_3" value="3" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Replace"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_3').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_3" class="Icon3 Replace_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Replace"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Replace"))%>"><icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="18"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Fix"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_4" id="Check<%= intPosIndex%>_<%= intIndex%>_4" value="4" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Fix"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_4').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_4" class="Icon4 Fix_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Fix"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Fix"))%>"><icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="19"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Cleaning"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_5" id="Check<%= intPosIndex%>_<%= intIndex%>_5" value="5" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Cleaning"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_5').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_5" class="Icon5 Cleaning_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Cleaning"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Cleaning"))%>"><icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="20"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_Swap"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_6" id="Check<%= intPosIndex%>_<%= intIndex%>_6" value="6" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_Swap"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_6').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_6" class="Icon6 Swap_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_Swap"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_Swap"))%>"><icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="21"/></div></span>
                                          </div>
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewStyle_No_Check"))%>">
                                          <span style="display:none"><input type="radio" name="Check<%= intPosIndex%>_<%= intIndex%>_7" id="Check<%= intPosIndex%>_<%= intIndex%>_7" value="7" onClick="IconRadioChange(<%= intPosIndex%>, <%= intIndex%>, this.value)" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusSelect_No_Check"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle"))%>></span>
                                          <span onTouchStart="isTouch = true" onTouchMove="isTouch = false" onTouchEnd="document.getElementById('Check<%= intPosIndex%>_<%= intIndex%>_7').click()"><div id="CheckIcon<%= intPosIndex%>_<%= intIndex%>_7" class="Icon7 No_Check_<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusColor_No_Check"))%>" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsStatusViewPos_No_Check"))%>"><icrop:CustomLabel ID="CustomLabe205" runat="server" TextWordNo="53"/></div></span>
                                          </div>
                                          <input type="hidden" name="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" id="HiddenAllData<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "HiddenAllData"))%>">
                                          <div style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextViewStyle"))%>">
                                    	  <span class="Area_Before"><input type="Text" readonly NAME="BeforeText<%= intPosIndex%>_<%= intIndex%>" ID="BeforeText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextBefore"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>&nbsp;
                                    	  <span class="Area_After"><input type="Text" readonly NAME="AfterText<%= intPosIndex%>_<%= intIndex%>" ID="AfterText<%= intPosIndex%>_<%= intIndex%>" style="width:55px; height:24px; font-size:16px; font-weight:bold; color:#666;" MaxLength="6" Value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsTextAfter"))%>" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>></span>
                                          </div>
                                    	</li>
                                    </ul>
                                </div>
		                      </dd>
                              <dd class="CompletedJobBox">
		                        <div class="InspectionResultBox">
                                	<ul class="InspectionResultIconBox">
                                    	<li>
                                       	  <div class="CompletedJob">
                                            <select multiple name="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" ID="InspecItemsSelector<%= intPosIndex%>_<%= intIndex%>" style="width:108px; height:26px; font-weight:bold;font-size:16px;-webkit-text-size-adjust: 100%;" Size="4" OnClick="" onblur="" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemInputStyle2"))%>>
<%--                                            
                                            <option value="1" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Replaced"))%>><icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="24"/></option>
                                            <option value="2" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Fixed"))%>><icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="25"/></option>
                                            <option value="3" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Cleaned"))%>><icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="26"/></option>
                                            <option value="4" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "InspecItemsSelectViewStyle_Swapped"))%>><icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="27"/></option>
--%>                                            
<%# DataBinder.Eval(Container.DataItem, "InspecItemsSelect_Options")%>                                            
                                            </select>
                                         </div>
                                   	    </li>
                                    </ul>
                                </div>
                              </dd>
	                        </dl>

                          </ItemTemplate>
                        </asp:ListView>

	                      </div>
	                      </div>
	                    </div>

	                  </div>

<!--ここまで右カラム(検査項目)-->
<!--ここから右カラム(メンテナンス)-->

		              <div id="OperationItems_Maintenance">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="CustomLabel42" runat="server" TextWordNo="10"/></h3>
		                  <div class="STC04BlockRightHead">
		                    <p class="STC04BlockRightHead1"><icrop:CustomLabel ID="CustomLabel16" runat="server" TextWordNo="13"/></p>
		                    <p class="STC04BlockRightHead2"><icrop:CustomLabel ID="CustomLabel39" runat="server" TextWordNo="14"/></p>
		                    <p class="STC04BlockRightHead3"></p>
	                      </div>
		                  <div class="STC04BlockRightMain">
		                  <div id="ScrollBlock_Maintenance">
<%  intPosIndex = 7%>
<% intIndex = 0%>
                        <asp:Repeater ID="InspecItemsList_Maintenance" runat="server" >
                          <ItemTemplate>
<% intIndex += 1%>
		                    <dl class="STC04BlockRightMain1-1 Mainte" style="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteViewStyle_Color"))%>">
		                      <dt><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_NAME"))%></dt>
		                      <dd class="InspectionResult">
                          	    <label for="t01_01"><input class="RadioA" type="radio" name="Maintenance<%= intPosIndex%>_<%= intIndex%>" id="Maintenance<%= intPosIndex%>_<%= intIndex%>_1" value="1" onclick="allcheck();" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteSelect_UncarriedOut"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteInputStyle"))%>><span><icrop:CustomLabel ID="CustomLabel42" runat="server" TextWordNo="41"/></span></label>
                          	    &nbsp;&nbsp;&nbsp;&nbsp;<label for="t01_02"><input class="RadioA" type="radio" name="Maintenance<%= intPosIndex%>_<%= intIndex%>" id="Maintenance<%= intPosIndex%>_<%= intIndex%>_2" value="2" onclick="allcheck();" <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteSelect_Enforcement"))%> <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteInputStyle"))%>><span><icrop:CustomLabel ID="CustomLabel40" runat="server" TextWordNo="42"/></span></label>
		                      &nbsp;&nbsp;&nbsp;&nbsp;</dd>
                              <dd class="CompletedJobBox">
		                      </dd>
	                        </dl>
                            <input type="hidden" name="MainteRegistMode<%= intPosIndex%>_<%= intIndex%>" id="MainteRegistMode<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteRegistMode"))%>">
                            <input type="hidden" name="MainteMode<%= intPosIndex%>_<%= intIndex%>" id="MainteMode<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteMode"))%>">
                            <input type="hidden" name="MainteCheck<%= intPosIndex%>_<%= intIndex%>" id="MainteCheck<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "MainteCheck"))%>">
                            <input type="hidden" name="JobDtlID<%= intPosIndex%>_<%= intIndex%>" id="JobDtlID<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_DTL_ID"))%>">
                            <input type="hidden" name="JobInstructID<%= intPosIndex%>_<%= intIndex%>" id="JobInstructID<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_INSTRUCT_ID"))%>">
                            <input type="hidden" name="JobInstructSeq<%= intPosIndex%>_<%= intIndex%>" id="JobInstructSeq<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "JOB_INSTRUCT_SEQ"))%>">
                            <input type="hidden" name="StallUseID<%= intPosIndex%>_<%= intIndex%>" id="StallUseID<%= intPosIndex%>_<%= intIndex%>"  value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "STALL_USE_ID"))%>">
                            <input type="hidden" name="TRN_RowLockVer<%= intPosIndex%>_<%= intIndex%>" id="TRN_RowLockVer<%= intPosIndex%>_<%= intIndex%>" value="<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TrnRowLockVersion"))%>">
                          </ItemTemplate>
                        </asp:Repeater>

	                    </div>
	                  </div>
	                  </div>

	                  </div>

<!--ここまで右カラム(メンテナンス)-->
<!--ここから右カラム(エラー)-->

		              <div id="OperationItems_Error">

		                <div class="STC04BlockRightItemWrap">
		                  <h3><icrop:CustomLabel ID="ErrorMessage" runat="server" ></icrop:CustomLabel></h3>
	                    </div>

	                  </div>

<!--ここまで右カラム(エラー)-->

                      </div>
		            <!--ここまで右カラム-->
	                </div>
</td></tr>
</table>
		            <p style="clear:both;"></p>
                    <div id="Div1"></div>
		              <!--ここまで左カラム-->
	              </div>
		        <!--ここまでメイン-->
	            </div>
	        </div>
		    </div>
	    </div>
<!--
	      </div>
    </div>
-->
		<!-- ここまでメインブロック -->
</asp:Content>

<asp:Content ContentPlaceHolderID="footer" ID="contentfooter" runat="server">
    <div id="FooterCustomButton" style="float:right; margin-right:16px;">
        <asp:Button ID="HiddenButtonApproveWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>
        <asp:Button ID="HiddenButtonRejectWork" runat="server" CssClass="HiddenButton" OnClientClick="reloadPageIfNoResponse(); return FooterButtonClick()"/>
        <asp:Button ID="HiddenButtonWarning" runat="server" CssClass="HiddenButton"/>

        <table class="fotterButtonTable">
            <tr>
                <td>
                    <p id="ButtonRejectWork" class="footerCustomButton_RejectWork" ><icrop:CustomLabel ID="RejectWork" runat="server" TextWordNo="34" /></p>
                </td>
                <td>
                    <p id="ButtonApproveWork" class="footerCustomButton_ApproveWork" ><icrop:CustomLabel ID="ApproveWork" runat="server" TextWordNo="35" /></p>
                </td>
            </tr>
        </table>
    </div>
    <div style="clear:right;">
    </div>
</asp:Content>
