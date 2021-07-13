<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3100302.ascx.vb" Inherits="Pages_SC3100302" EnableViewState="false" %>
<div>
	<asp:ScriptManagerProxy ID="SC3100302ScriptManagerProxy" runat="server">
	</asp:ScriptManagerProxy>
                <asp:UpdatePanel runat="server" ID="VisitSales" UpdateMode="Conditional">
                <ContentTemplate>
                <div class="inRightBox1 reftOrder4">
                    <h3 class="subTitle02ToDo todoType4">
                        <span class="titleName ">
                            <icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="16" />
                        </span>
                        <span class="titleCount">
                            <span><asp:Label ID="VisitSalesCount" runat="server"></asp:Label></span>
                        </span>
                    </h3>
                    <div class="clearboth loadingVisitActual">&nbsp;</div>
                    <div id="VisitBoxOut" class="SizeS">
                        <div id="VisitBoxIn" class="todoBoxIn" style="overflow: hidden;">
                            <asp:Repeater ID="ActualVisitRepeater" runat="server">
                                <ItemTemplate>
                                    <li id="VisitActualRow" runat="server">
                                    <div class="SCMainChip">
                                        <div class="InnerBox">
                                            <span class="inUserName useEllipsis">
                                               <asp:Literal ID="CustomerName" runat="server" Mode="Encode" Text='<%#Eval("CUST_NAME_WITH_TITLE")%>'></asp:Literal>
                                            </span>
                                            <br>
                                            <span class="addInfomation useEllipsis">
                                               <asp:Literal ID="Infomation" runat="server" Mode="Encode" Text='<%#Eval("CST_SERVICE_NAME")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint3">
                                               <asp:Literal ID="SalesStartDate" runat="server" Mode="Encode" Text='<%#Eval("SALES_DATE")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint4 useEllipsis">
                                               <asp:Literal ID="TempStaffName" runat="server" Mode="Encode" Text='<%#Eval("TEMP_STAFFNAME")%>'></asp:Literal>
                                            </span>
                                            <span class="checkPoint5">
                                               <img id="TempStaffOperationIcon" src='<%#Server.HTMLEncode(Eval("TEMP_STAFF_OPERATIONCODE_ICON"))%>' width="22" height="23" runat="server" alt=""></img>
                                            </span>
                                        </div>
                                    </div>
                                    <input type="hidden" class="Dlrcd" value="<%#Eval("DLRCD")%>" />
                                    <input type="hidden" class="Strcd" value="<%#Eval("STRCD")%>" />
                                    <input type="hidden" class="FllwupBoxSeqno" value="<%#Eval("FLLWUPBOX_SEQNO")%>" />
                                    <input type="hidden" class="SalesStatus" value="<%#Eval("SALES_STATUS")%>" />
                                    <input type="hidden" class="CustomerSegment" value="<%#Eval("CUSTSEGMENT")%>" />
                                    <input type="hidden" class="CustomerClass" value="<%#Eval("CUSTOMERCLASS")%>" />
                                    <input type="hidden" class="CustomerId" value="<%#Eval("CRCUSTID")%>" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <asp:Button ID="VisitSalesTrigger" runat="server" Text="Button" CssClass="VisitSalesTrigger" />

                </ContentTemplate>  
                </asp:UpdatePanel></div>
<link rel="Stylesheet" href="../Styles/SC3100302/SC3100302.css?20120224000001" />
<link rel="stylesheet" href="../Styles/SC3100302/SC3100302.PullDownRefresh.css?20120224000001" type="text/css" media="all" />
<script type="text/javascript" src="../Scripts/SC3100302/SC3100302.js?20131002000000"></script>
<script type="text/javascript" src="../Scripts/SC3100302/SC3100302.PullDownRefresh.js?20120224000000"></script>
<script type="text/javascript" src="../Scripts/SC3100302/SC3100302.MainMenuFingerscroll.js?20120224000000"></script>
<asp:HiddenField ID="WalkinCompFlg" runat="server" value=""/>
<asp:HiddenField ID="day" runat="server" value=""/>
