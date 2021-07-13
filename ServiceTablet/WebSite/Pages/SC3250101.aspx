<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false"  CodeFile="SC3250101.aspx.vb" Inherits="SC3250101"  %>

<asp:Content ID="cont_content" ContentPlaceHolderID="content" Runat="Server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="format-detection" content="telephone=no" />
    <link rel="Stylesheet" href="../Styles/SC3250101/SC3250101.css?201409100000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3250101/SC3250101.js?201409100001"></script>	


		<!-- 中央部分-->
		<div id="main">
            <!-- HiddenField宣言 start -->
            <asp:HiddenField ID="hdnRO_NUM" runat="server" />
            <asp:HiddenField ID="hdnClickedListNo" runat="server" />
            <asp:HiddenField ID="hdnProcMode" runat="server" />
            <asp:HiddenField ID="hdnClickedRowNo" runat="server" />
            <asp:HiddenField ID="hdnViewMode" runat="server" />
            <asp:HiddenField ID="hdnChangeFlg" runat="server" value="0"/>
            <asp:HiddenField ID="ClearMessageID" runat="server" />
            <asp:HiddenField ID="ClearCartMessageID" runat="server" />
            <asp:HiddenField ID="hdnKatashiki" runat="server" />
            <asp:HiddenField ID="hdnModelCode" runat="server" />
            <asp:HiddenField ID="hdnGradeCode" runat="server" />
            <asp:HiddenField ID="hdnSuggest" runat="server" />
            <asp:HiddenField ID="hdnAlreadySendFlag" runat="server" Value="0" />
            <asp:HiddenField ID="hdnClickedInspecCD" runat="server" />
<!-- 【***完成検査_排他制御***】 start -->
            <asp:HiddenField ID="rowLockvs" runat="server" />
<!-- 【***完成検査_排他制御***】 end -->

            
            <!-- HiddenField宣言 end   -->

        	<div id="contentsMain">         
                <div id="roListtitle">
                	<div id="roTitle">
                        <div id="Logos">
                            <div id="LogosIn">
                            <asp:Image ID="ImageLogo" runat="server" width="100" height="15" alt="" onError="ImageErrorFunc();" />
                    	    <h2 id="logoImage" runat="server"></h2>
                            </div>
                        </div>
                        <div id="carTxt" runat="server">
                          <p id="carGradeBox"><icrop:CustomLabel id="carGrade" runat="server" Width="45px" UseEllipsis="true" /></p>
                          <p id="carMileageBox"><icrop:CustomLabel id="carMileage" runat="server" /></p>
                        </div>
                    	<%--<div id="iconBox">
                        <dl>
                        <dd class="search on"><a id="SearchOn" href="#" runat="server">&nbsp;</a></dd>
                        <dd class="report off"><a id="ReportOff" href="#" runat="server">&nbsp;</a></dd>
                        </dl>                        
                        </div>--%>
                        <dl id="roResult">
                        <dt><icrop:CustomLabel ID="ResultLabel" runat="server" TextWordNo="2" Width="63px" Height="16px" UseEllipsis="true" /></dt>

                            <dd><asp:DropDownList ID="ddlResult" runat="server" onchange="ActiveDisplayOn();" AutoPostBack="True"></asp:DropDownList></dd>
                        <dt id="txt01"><icrop:CustomLabel ID="SuggesttLabel" runat="server" TextWordNo="3" Width="63px" Height="16px" UseEllipsis="true" /></dt>
                        <dd class="Iten02"><asp:DropDownList ID="ddlSuggest" runat="server" onchange="OnChangeDdlSuggest();"></asp:DropDownList></dd>
                        </dl>
                    </div>
                    <div id="roAdvice">
                        <div id="bottomImg" runat="server"></div>
                    	<h2 id="txt02"><icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="4" Width="190px" Height="16px" UseEllipsis="true" /></h2>
                        <div id="roAdvice" class="Camera">
                        <asp:Image ID="btnCamera" ImageUrl="../Styles/images/SC3250101/icon_photo.png" runat="server"/>
                        </div>
                        <div id="roAdvicebox" class="textAreaBox" runat="server" >
                            <div id="roAdviceboxContens" runat="server" >
                            <ul>
                            <li><span id="RepairAdvice" runat="server"></span></li>
                            <li><span id="AdditionWorkAdvice" runat="server"></span></li>
                            <br />
                            </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="roSpcampaigntitle">
                    <h2><icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="5" Width="160px" Height="24px" UseEllipsis="true" /></h2>
                    <iframe id="roSpcampaigntitleiFrame" class="ifrm" src="SC3230101.aspx"  frameborder="0" runat="server"></iframe>
                </div>
            
            <div id="contentsBox">
            	<div id="contentsArea" runat="server">

                <!--ラインの位置-->
                <div><img src="../Styles/images/SC3250101/line01.png" width="245" height="147" alt="" id="line01"/></div>
                <div><img src="../Styles/images/SC3250101/line02.png" width="49" height="174" alt="" id="line02"/></div>
                <div><img src="../Styles/images/SC3250101/line03.png" width="121" height="150" alt="" id="line03"/></div>
                <div><img src="../Styles/images/SC3250101/line04.png" width="227" height="131" alt="" id="line04"/></div>
				<div><img src="../Styles/images/SC3250101/line05.png" width="294" height="31" alt="" id="line05"/></div>
                <div><img src="../Styles/images/SC3250101/line07.png" width="135" height="111" alt="" id="line07"/></div>
                <div><img src="../Styles/images/SC3250101/line08.png" width="47" height="171" alt="" id="line08"/></div>
                <div><img src="../Styles/images/SC3250101/line09.png" width="221" height="67" alt="" id="line09"/></div>                                      


               	  <!-- 01.上段左テーブル-->
           	  	  <div id="list01" class="tableShadow" runat="server">
                    <table id="List01_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server">
                        <tr>
                        <th id="List01_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage01" class="floatL" runat="server" />
                                <asp:Label ID="List01_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List01_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List01_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List01_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List01_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD01" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag01" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List01_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List01_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List01_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                        </tr>
                    </table>
                    <div id="scrollArea01">
                    <asp:GridView ID="List01_Data"  runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"  runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 02.下段左テーブル-->
                  <div id="list02" class="tableShadow" runat="server">
                    <table id="List02_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                        <tr>
                        <th id="List02_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage02" class="floatL" runat="server" />
                                <asp:Label ID="List02_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List02_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List02_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List02_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List02_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD02" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag02" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List02_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List02_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List02_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                        </tr>
                    </table>
                    <div id="scrollArea02">
                    <asp:GridView ID="List02_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 03.上段左から2番目テーブル-->
                  <div id="list03" class="tableShadow" runat="server">
                    <table id="List03_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                        <tr>
                        <th id="List03_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage03" class="floatL" runat="server" />
                                <asp:Label ID="List03_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List03_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List03_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List03_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List03_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD03" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag03" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List03_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List03_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List03_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                        </tr>
                    </table>

                    <div id="scrollArea03">
                    <asp:GridView ID="List03_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 04.下段左から2番目テーブル-->
                  <div id="list04" class="tableShadow" runat="server">
                    <table id="List04_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List04_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage04" class="floatL" runat="server" />
                                <asp:Label ID="List04_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List04_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List04_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List04_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List04_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                           </div>
                            <asp:HiddenField ID="hdnSVC_CD04" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag04" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List04_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List04_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List04_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>

                    <div id="scrollArea04">
                    <asp:GridView ID="List04_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 05.上段右から2番目テーブル-->
                  <div id="list05" class="tableShadow" runat="server">
                    <table id="List05_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List05_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage05" class="floatL" runat="server" />
                                <asp:Label ID="List05_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List05_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List05_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List05_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List05_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD05" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag05" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List05_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List05_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List05_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>

                    <div id="scrollArea05">
                    <asp:GridView ID="List05_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 06.下段右から2番目テーブル-->
                  <div id="list06" class="tableShadow" runat="server">
                    <table id="List06_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List06_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage06" class="floatL" runat="server" />
                                <asp:Label ID="List06_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List06_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List06_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List06_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List06_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD06" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag06" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List06_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List06_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List06_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>
                    <div id="scrollArea06">
                    <asp:GridView ID="List06_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
                  
                  <!-- 07.上段右テーブル-->
                  <div id="list07" class="tableShadow" runat="server">
                    <table id="List07_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List07_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage07" class="floatL" runat="server" />
                                <asp:Label ID="List07_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List07_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List07_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List07_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List07_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD07" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag07" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List07_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List07_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List07_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>
                    <div id="scrollArea07">
                    <asp:GridView ID="List07_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>         
                    </div>
                  </div>

                  <!-- 08.中段右テーブル-->
                  <div id="list08" class="tableShadow" runat="server">
                    <table id="List08_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List08_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage08" class="floatL" runat="server" />
                                <asp:Label ID="List08_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List08_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List08_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List08_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List08_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD08" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag08" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List08_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List08_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List08_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>
                    <div id="scrollArea08">
                    <asp:GridView ID="List08_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# Bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>

                  <!-- 09.下段右テーブル-->
                  <div id="list09" class="tableShadow" runat="server">
                    <table id="List09_Header" border="0" cellspacing="0" cellpadding="0" class="listHead" runat="server" >
                      <tr>
                        <th id="List09_PartName" runat="server" scope="col" class="row1">
                            <div class="TableSetMainHD">
                                <div ID="TitleImage09" class="floatL" runat="server" />
                                <asp:Label ID="List09_Col1_Title" runat="server"></asp:Label>
                            </div>
                        </th>
                        <th id="List09_ResultName" runat="server" scope="col" class="tdLineV row2">
                            <div id="List09_Col2" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List09_WordResult" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List09_Result" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                            <asp:HiddenField ID="hdnSVC_CD09" value="" runat="server" />
                            <asp:HiddenField ID="ResultFlag09" Value="0" runat="server" />
                        </th>
                        <th scope="col" class="row3">
                            <div  id="List09_Col3" class="TableSetMainData01" runat="server">
                                <icrop:CustomLabel ID="List09_WordSuggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" /><br />
                                <icrop:CustomLabel ID="List09_Suggest" runat="server" Width="39px" Height="16px" UseEllipsis="true" />
                            </div>
                        </th>
                      </tr>
                    </table>
                    <div id="scrollArea09">
                    <asp:GridView ID="List09_Data" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ItemNo" Text='<%# Bind("ListIndex") %>' runat="server"></asp:Label>
                                    <asp:Label ID="ItemName" Text='<%# bind("ItemName1")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnItemName1" Value='<%# Bind("ItemName1")%>' runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnItemName2" Value='<%# Bind("ItemName2")%>' runat="server"></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate >
                                    <asp:Label ID="ResultImage" CssClass='<%# bind("ResultImage") %>'  Width="20" Height="15"   runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNeedIconFlg" Value='<%# Bind("NeedIconFlg")%>' runat="server" />
                                    <asp:HiddenField ID="hdnSuggestInfo" Value='<%# Bind("SuggestInfo")%>' runat="server" />
                                    <asp:Label ID="SuggestImage" CssClass='<%# Bind("SuggestImage")%>'  Width="20" Height="15"   runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                  </div>
			</div>
            </div>
           
        </div>

        </div>
		<!-- ここまで中央部分 -->
        <!--半透明のボード-->
        <div id="contentsMainonBoard" style="display:none" runat="server">
        </div>

        <div id="closeBtn" style="z-index:3;display:none;" runat="server"><img id="closeBtnImage" onclick="ClosePopUp();" src="../Styles/images/SC3250101/popUpClose.png" width="29" height="29" alt="" /></div>
        
        <table id="popUp" width="420" style="z-index:2;display:none" cellpadding="0" cellspacing="0" runat="server">
                <tr id="popup_header" runat="server" >
                    <th id="HeaderCol1" colspan="3" class="col1">
                        <asp:Image ID="popUpTitleImage" width="27" height="27" alt="" class="floatL" runat="server"/>
                        <div ID="popUpTitleImageFrame" runat="server"></div>
                        <p id="popUpTitle" runat="server"></p>
                    </th>
                    <th id="HeaderCol2" class="col2">
                        <icrop:CustomLabel ID="Header_WordResult" runat="server" Width="59px" Height="16px" UseEllipsis="true" /><br />
                        <icrop:CustomLabel ID="Header_Result" runat="server" Width="59px" Height="16px" UseEllipsis="true" />
                    </th>
                    <th id="HeaderCol3" class="col3">
                        <icrop:CustomLabel ID="Header_WordSuggest" runat="server" Width="59px" Height="16px" UseEllipsis="true" /><br />
                        <icrop:CustomLabel ID="Header_Suggest" runat="server" Width="59px" Height="16px" UseEllipsis="true" />
                    </th>
                </tr>
        </table>
        <div id="popUpList" style="z-index:2;display:none;" runat="server">
        <div id="popUpListIn">

        <asp:GridView ID="popUpDetail" runat="server" BorderStyle="None" class="listData" AutoGenerateColumns="false" ShowHeader="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate >
                        <asp:HyperLink id="cell1_val" Text='<%# bind("cell1_val")%>' runat="server"></asp:HyperLink>
                        <asp:Label ID="cell2_val" Text='<%# bind("cell2_val")%>' runat="server"></asp:Label>
                        <asp:Label ID="cell3_val" Text='<%# bind("cell3_val")%>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate >
                        <asp:Label ID="cell4_img" CssClass='<%# bind("cell4_img") %>'  Width="29" Height="25"  runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="cell5_img_url" CssClass='<%# Bind("cell5_img_url")%>' Width="29" Height="25"  runat="server"/>
                        <asp:HiddenField ID="hdn_NeedIcon_value" Value='<%# Bind("hdn_NeedIcon_value")%>' runat="server" />
                        <asp:HiddenField ID="hdn_cell5_id" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        </div>
        </div>
        
        <!--整備選択コントロール-->
        <div id="balloon_top_back" style="display:none;"></div>
        <div id="balloon_top_front" style="display:none;"></div>
        <div id="floatBox" style="display:none;">
	        <div id="balloon">
                <ul id="Items">
                    <li id="li1">
                        <div id="iconInspect" onclick="SelectItem(0);">
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="16"/>
                            </div>
                        </div>
                    </li>
                    <li id="li2">
                        <div id="iconReplace" onclick="SelectItem(1);" >
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="17"/>
                            </div>
                        </div>
                    </li>
                    <li id="li3">
                        <div id="iconFix" onclick="SelectItem(2);">
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="18"/>
                            </div>
                        </div>
                    </li>
                    <li id="li4">
                        <div id="iconSwap"onclick="SelectItem(3);" >
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="19"/>
                            </div>
                        </div>
                    </li>
                    <li id="li5">
                        <div id="iconCleaning"onclick="SelectItem(4);" >
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="20"/>
                            </div>
                        </div>
                    </li>
                    <li id="li6">
                        <div id="iconNone"onclick="SelectItem(5);" >
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel6" runat="server" TextWordNo="21"/>
                            </div>
                        </div>
                    </li>
                    <li id="li7">
                        <div id="iconReset"onclick="SelectItem(6);" >
                            <div class="IconText">
                                <icrop:CustomLabel ID="CustomLabel7" runat="server" TextWordNo="22"/>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
   <!-- </div> -->
</asp:Content>
<asp:Content ID="cont_footer" ContentPlaceHolderID="footer" Runat="Server">
    <div style="margin:0px 0px 0px 853px;">
        <table class="fotterButtonTable">
            <tr><td>
                <div ID="imgRegister" class="Register_Disable" onclick="OnClickRegister();" runat="server">
                    <icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="15"/>
                </div>
            </td><td>
                <div ID="imgCart" class="Cart_Disable" onclick="OnClickCart();" runat="server" />
            </td></tr>
        </table>
	</div>
    <%'顧客に遷移するためのパラメータ %>
    <asp:Button ID="CustomerButton" runat="server" style="display:none" />
</asp:Content>
