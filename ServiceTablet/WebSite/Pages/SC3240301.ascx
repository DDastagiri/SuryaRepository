<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3240301.ascx.vb" Inherits="Pages_SC3240301" %>
<link rel="Stylesheet" href="../Styles/SC3240301/subchip.css?201410101040" type="text/css" media="screen,print"/>

<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い START%>
<!-- スクリプトファイルの参照は圧縮済ファイルを対象とする -->
<script type="text/javascript" src="../Scripts/SC3240301/SC3240301.fingerscroll.min.js?20170911000000"></script>
<script type="text/javascript" src="../Scripts/SC3240301/SubChip.min.js?20201008000000"></script>
<script type="text/javascript" src="../Scripts/SC3240301/SubChipPrototype.min.js?20180709000000"></script>
<%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い END%>

<asp:HiddenField ID="hiddenupdatetime" runat="server"/>
<asp:HiddenField ID="hidJsonDatasubchip" runat="server"/>
<asp:HiddenField ID="hidSubMsgData" runat="server"/>
<%--アクティブインジケータ--%>
<div id="SubChipAreaActiveIndicator"></div>
<div id="SubChip_LoadingScreen" style="width: 1024px; height: 179px; display: none; "></div>   
<%--受付エリア--%>
<div class="SubChipReception">
            <div class="ReceptionBack02">         
              <div class="ReceptionBack"></div>
            </div>
           	  <div class="ReceptionTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 1)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--追加作業エリア--%>
<div class="SubChipAdditionalWork">
            <div class="AdditionalWorkBack02">         
              <div class="AdditionalWorkBack"></div>
            </div>
           	  <div class="AdditionalWorkTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 2)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--完成検査エリア--%>
<div class="SubChipCompletionInspection">
            <div class="CompletionInspectionBack02">         
              <div class="CompletionInspectionBack"></div>
            </div>
           	  <div class="CompletionInspectionTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 3)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--洗車エリア--%>
<div class="SubChipCarWash">
            <div class="CarWashBack02">         
              <div class="CarWashBack"></div>
            </div>
           	  <div class="CarWashTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 4)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--納車待ちエリア--%>
<div class="SubChipWaitingDelivered">
            <div class="WaitingDeliveredBack02">         
              <div class="WaitingDeliveredBack"></div>
            </div>
           	  <div class="WaitingDeliveredTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 5)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--NoShowエリア--%>
 <div class="SubChipNoShow">
            <div class="NoShowBack02">         
              <div class="NoShowBack"></div>
            </div>
           	  <div class="NoShowTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 6)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
<%--中断エリア--%>
<div class="SubChipStop">
            <div class="StopBack02">         
              <div class="StopBack"></div>
            </div>
           	  <div class="StopTitle SubBOXTitle"><%:WebWordUtility.GetWord("SC3240301", 7)%></div>
              <div class ="SubChipBox">
              <div class="SubChipArea"></div>
              </div>            
            <div class="Triangle"></div>
		  </div>
