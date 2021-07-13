<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3060101.aspx.vb" Inherits="Pages_SC3060101" %>

<%@ Register src="~/Pages/SC3110101.ascx" tagname="SC3110101" tagprefix="uc1" %>

<%-- ヘルプ依頼 --%>
<%@ Register src="SC3080401.ascx" tagname="SC3080401" tagprefix="uc1" %>


<%-- 査定依頼 --%>
<%@ Register src="SC3080301.ascx" tagname="SC3080301" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
    <link rel="Stylesheet" href="../Styles/SC3060101/SC3060101.css" />
    <script src="../Scripts/SC3060101/SC3060101.js" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server"  >
            <%'サーバー処理中のオーバーレイとアイコン %>
            <div id="serverProcessOverlayBlack"></div>
            <div id="serverProcessIcon"></div>

	<!-- 中央部分-->
		<div id="SC3060101Display"  >
	<!-- ここからコンテンツ -->
		    <div id="tcvNsc44Main">


			    <div class="nscListBoxSet">
			        <div class="nscListBoxLeft">
			            <h4><icrop:CustomLabel ID="CustomLabel2" runat="server" /></h4>
			            <div class="nscListBoxIn02">
                            <table>
                                <tr>
                                    <td valign="top">
                                        <asp:Image id="CarPhoto"  Width="143" Height="107" runat="server" Visible="false" />
                                    </td>
                                    <td style="padding-left:2px">
                                        <div class="nsc44carText">
                                            <h5><icrop:CustomLabel ID="MakerLabel" runat="server" /></h5>
		                                    <strong><icrop:CustomLabel ID="VehicleLabel" runat="server" /></strong>
		                                    <table border="0" cellspacing="0" cellpadding="0">
		                                        <tr>
		                                            <td width="24" class="icn"><img src="../Styles/images/SC3060101/nsc44carIcn1.png" width="16" height="16"></td>
                                                    <td class="text" style="font-size:14px"><icrop:CustomLabel ID="GradeLabel" runat="server"  /></td>
	                                            </tr>
		                                        <tr>
		                                            <td width="24" class="icn"><img src="../Styles/images/SC3060101/nsc44carIcn2.png" width="16" height="16"></td>
		                                            <td class="text"><icrop:CustomLabel ID="ModelYearLabel" runat="server"  /></td>
	                                            </tr>
		                                        <tr>
		                                            <td width="24" class="icn"><img src="../Styles/images/SC3060101/nsc44carIcn3.png" width="16" height="16"></td>
		                                            <td class="text"><icrop:CustomLabel ID="MileageLabel" runat="server" /></td>
	                                            </tr>
		                                    </table>
			                            </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clearboth">&nbsp;</div>
                        </div>
		            </div>
			      <div class="nscListBoxCenter" id="optList" style=" z-index:2">
                      <h4><icrop:CustomLabel ID="CustomLabel3" runat="server"/></h4>
			              <div class="nscListBoxOpt01" >
               		          <ul >
                                  <asp:Repeater ID="OptionRepeater1" runat="server" ClientIDMode="Predictable" >
                                      <ItemTemplate>
                                          <li class="nscListBoxCenterIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameOptionNumber)) %>"><span><%#HttpUtility.HtmlEncode(Eval(ColumnNameOptionWord)) %></span></li>
                                      </ItemTemplate>
                                  </asp:Repeater>
                              </ul >
                          </div>
                          <div id="DivSlideImage1" class="triangle"  runat="server" visible="false"><img id="downImg" src="../Styles/images/SC3060101/nsc44icntriangle.png" width="12" height="9"></div>

                          <div id="newsBoxInfo" class="nscListBoxOpt02" style="display:none;overflow:hidden;">
                   		      <ul >
                                  <asp:Repeater ID="OptionRepeater2" runat="server" ClientIDMode="Predictable" >
                                     <ItemTemplate>
                                        <li class="nscListBoxCenterIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameOptionNumber)) %>"><span><%#HttpUtility.HtmlEncode(Eval(ColumnNameOptionWord)) %></span></li>
                                     </ItemTemplate>
                                  </asp:Repeater>
                              </ul >
                          </div>
                          <div id="divSlideIMG2" class="triangleUp" style="display:none"><img src="../Styles/images/SC3060101/nsc44icntriangle-up.png" width="12" height="9"></div>


		          </div> 
			      <div class="nscListBoxRight">
			        <h4><icrop:CustomLabel ID="CustomLabel31" runat="server" /></h4>
			        <div class="nscListBoxIn">
			          <div class="PriceText"><icrop:CustomLabel ID="PriceLabel" runat="server" /></div>
			          <div class="nsc44priceimg"><asp:Image id="InspectorImage"  Width="45" Height="45" runat="server"  ImageUrl="../Styles/images/SC3060101/scNscCustomerCarTypeIcon4.png"/><br/>
		              <icrop:CustomLabel ID="InspectorLabel" runat="server" /></div>
                      <div class="BalloonBoder">
                      <div class="BalloonArrowBoder"><div class="BalloonArrow"></div></div>
                     	<div class="Balloon"><icrop:CustomLabel ID="MemoLabel" runat="server" /></div>
                      </div>
                      <div class="Deadline"><icrop:CustomLabel ID="CustomLabel32" runat="server"/> <icrop:CustomLabel ID="InspectLimitLabel" runat="server" /></div>
			        </div>
		          </div>
                </div>
                <div class="clearboth">&nbsp;</div>
			    <div class="nsc44BoxBottom">
			        <h4><icrop:CustomLabel ID="CustomLabel33" runat="server" /></h4>
		                <div class="nsc44BoxBottomIn">
			                <div class="nsc44BoxLeft1">
			                    <ul>
                        	        <li class="nsc44icnP"><icrop:CustomLabel ID="CustomLabel34" runat="server"/></li>
                        	        <li class="nsc44icnB"><icrop:CustomLabel ID="CustomLabel35" runat="server"/></li>
                        	        <li class="nsc44icnX"><icrop:CustomLabel ID="CustomLabel36" runat="server"/></li>
                                </ul>
			                </div>
    		                 <div class="nsc44BoxLeft2">
	    		                 <ul>
                            	    <li><icrop:CustomLabel ID="CustomLabel37" runat="server"/></li>
                            	    <li><icrop:CustomLabel ID="CustomLabel38" runat="server"/></li>
                            	    <li><icrop:CustomLabel ID="CustomLabel39" runat="server"/></li>
                                </ul>
		                </div>
                        <div class="nsc44carpoint ">
                            <div class="SquareBox SquareBoxPosition01"></div>
                            <div class="SquareBox SquareBoxPosition02"></div>
                            <div class="SquareBox SquareBoxPosition03"></div>
                            <div class="SquareBox SquareBoxPosition04"></div>
                            <div class="SquareBox SquareBoxPosition05"></div>

                                <asp:Repeater ID="OuterRepeaterP" runat="server" ClientIDMode="Predictable" >
                                 <ItemTemplate>
                                    <div style="z-index:1" class="nsc44pointIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><%# HttpUtility.HtmlEncode(Eval(ColumnNameRating))%></div>
                                    <div style="z-index:0" class="nsc44pointIcnMark<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><img src="../Styles/images/SC3060101/nsc44icnPNone.png" ></div>
                                 </ItemTemplate>
                             </asp:Repeater>
                             <asp:Repeater ID="OuterRepeaterB" runat="server" ClientIDMode="Predictable" >
                                 <ItemTemplate>
                                    <div style="z-index:1" class="nsc44pointIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><%# HttpUtility.HtmlEncode(Eval(ColumnNameRating))%></div>
                                    <div style="z-index:0" class="nsc44pointIcnMark<%#Eval(ColumnNameRegionCode) %>"><img src="../Styles/images/SC3060101/nsc44icnBNone.png"></div>
                                 </ItemTemplate>
                             </asp:Repeater>
                             <asp:Repeater ID="OuterRepeaterX" runat="server" ClientIDMode="Predictable" >
                                 <ItemTemplate>
                                    <div style="z-index:1" class="nsc44pointIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><%# HttpUtility.HtmlEncode(Eval(ColumnNameRating))%></div>
                                    <div style="z-index:0" class="nsc44pointIcnMark<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><img src="../Styles/images/SC3060101/nsc44icnXNone.png" ></div>
                                 </ItemTemplate>
                             </asp:Repeater>
                             <asp:Repeater ID="OuterRepeater4" runat="server" ClientIDMode="Predictable" >
                                 <ItemTemplate>
                                    <div style="z-index:0" class="nsc44pointIcn<%#HttpUtility.HtmlEncode(Eval(ColumnNameRegionCode)) %>"><%#HttpUtility.HtmlEncode(Eval(ColumnNameRating)) %></div>
                                 </ItemTemplate>
                             </asp:Repeater>

                         </div>
                         
                         <div class="nsc44pointThumbnailSet"  style="z-index:0;position:absolute; left:200px" runat="server" id="SumDiv">
    
                             <asp:Repeater ID="ImageRepeater" runat="server" ClientIDMode="Predictable" >
                                 <ItemTemplate>
                                     <div id = "sumDiv<%#HttpUtility.HtmlEncode(Eval(ColumnNamePhotoNo)) %>" class="nsc44pointThumbnailImg nsc44pointThumbnailImg<%#HttpUtility.HtmlEncode(Eval(ColumnNamePhotoNo) +1)%>"><img id = "sumImg<%#HttpUtility.HtmlEncode(Eval(ColumnNamePhotoNo)) %>" src='<%#HttpUtility.HtmlEncode(Eval(ColumnNameSmallPhoto))%>' width="71" height="53" onclick="selectBigImage(this,'<%#HttpUtility.HtmlEncode(Eval(ColumnNameBigPhoto))%>', 'sumDiv<%#HttpUtility.HtmlEncode(Eval(ColumnNamePhotoNo)) %>', <%#HttpUtility.HtmlEncode(Eval(ColumnNamePhotoNo)) %>);"></div>
                                 </ItemTemplate>
                             </asp:Repeater>

                         </div>
		          </div>
                </div>
              </div>
			</div>
		<!-- ここまでコンテンツ -->
            <div id="tcvNsc31Main" style="display:none">
                <div class="closeWind" >
                    <img src = "../Styles/images/SC3060101/clossIcn.png" onclick="closeBigImage();"  />
                </div>
                <div class="tcvNsc31Black" id="tcvNsc31Black" style="display:none">
                </div>
	            <div class="popWind" id="popWind">
    	            <div class="dataWind1" id="dataWind1"> 
                        <img id="popImg" />
	                </div>
	            </div>
            </div>

		<!-- ここまで中央部分 -->
        </asp:Content>

        <asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

                <!-- ヘルプ依頼 START -->
            <uc1:SC3080401 ID="SC3080401" runat="server" TriggerClientID="MstPG_FootItem_Sub_203" />
            <!-- ヘルプ依頼 END -->

            
            <%-- 試乗入力画面のユーザコントロール --%>
            <uc1:SC3110101 ID="SC3110101" runat="server" TriggerClientID="MstPG_FootItem_Sub_201" />
               <!-- 査定依頼 START -->
            <div id="Div2"  style="z-index:10000;">
                <uc1:SC3080301 ID="Sc3080301Page" runat="server" />
            </div>
            
   
        </asp:Content>

