<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3290101.aspx.vb" Inherits="Pages_SC3290101" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <%'スタイルシート %>
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Style.css"))%>" />
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/jquery.popover.css"))%>" />
   	<link rel="stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/Controls.css"))%>" />
    <link rel="Stylesheet" href="<%=WebResource.GetUrl(ResolveClientUrl("~/Styles/CommonMasterPage.css"))%>" />

    <%'スタイルシート(画面固有) %>
    <link type="text/css" href="../Styles/SC3290101/SC3290101.css?20150130000001" rel="stylesheet" />

    <%'スクリプト(Masterページと合わせる) %>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-1.5.2.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.ui.ipad.altfix.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.doubletap.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.flickable.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.json-2.3.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.popover.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.fingerscroll.js"))%>"></script>

    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/icropScript.js"))%>"></script>

    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CheckMark.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomLabel.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomTextBox.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.DateTimeSelector.js"))%>"></script>
  
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SegmentedButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.SwitchButton.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.CustomRepeater.js"))%>"></script>
    <script type="text/javascript" src="<%=WebResource.GetUrl(ResolveClientUrl("~/Scripts/jquery.NumericKeypad.js"))%>"></script>


    <%'スクリプト(画面固有) %>
    <script type="text/javascript" src="../Scripts/SC3290101/Common.js?20140718000001"></script>
    <script type="text/javascript" src="../Scripts/SC3290101/SC3290101_aspx.js?20140911000001"></script>
</head>
<body style="-webkit-user-select:none">
    <div id="bodyFrame">
        <form id="this_form" runat="server">

            <%' 非同期読み込みのためのScriptManagerタグ %>
            <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true">
            </asp:ScriptManager>

            <%' 更新日時設定用 %>
            <asp:HiddenField ID="SC3290101_TempLastUpdateTime" runat="server"/>

            <div id="SC3290101_TBL_Box02">
                <table class="SC3290101_table02" border=0 cellspacing=0 cellpadding=0>
                    <tbody id="SC3290101_table02Body">
                        <asp:Repeater ID="SC3290101_IrregularListRepeater" runat="server" >
                            <ItemTemplate>
                                <tr>
                                    <td class="TBL_DTST GrayBKDT">
                                        <asp:HiddenField ID="IrregClassCd" runat="server"/>
                                        <asp:HiddenField ID="IrregItemCd" runat="server"/>
                                        <icrop:CustomLabel ID="SC3290101_IrregularityItem" runat="server" UseEllipsis="False" max-width="330px" class="Ellipsis"/>
                                        <div class="Marker" runat="server">
                                            <icrop:CustomLabel ID="SC3290101_Marker" runat="server" UseEllipsis="False"/>
                                        </div>
                                    </td>
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT GrayBKDT">
                                        <icrop:CustomLabel ID="SC3290101_NoOfStaffs" runat="server" UseEllipsis="False"/>
                                    </td> 
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTED GrayBKDT">
                                        <icrop:CustomLabel ID="SC3290101_NoOfIrregularities" runat="server" UseEllipsis="False" />
                                        <div id="SC3290101_FuriateStaffNotDiv" runat="server">
                                            <asp:Panel ID="SC3290101_StaffAssignToCustCountPanel" runat="server">
                                                    <div class="SC3290101_LoadingAnimation2 show2"></div>
                                                    <icrop:CustomLabel id="SC3290101_StaffAssignToCustCount" runat="server" UseEllipsis="False" max-width="118px" class="Ellipsis"/>
                                            </asp:Panel>
                                            <asp:Panel ID="SC3290101_UnallocatedActivityCountPanel" runat="server">
                                                    <div class="SC3290101_LoadingAnimation2 show2"></div>
                                                    <icrop:CustomLabel id="SC3290101_UnallocatedActivityCount" runat="server" UseEllipsis="False" max-width="118px" class="Ellipsis" />
                                            </asp:Panel>
                                        </div>
                                    </td> 
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr>
                                    <td class="TBL_DTST WhiteBKDT">
                                        <asp:HiddenField ID="IrregClassCd" runat="server"/>
                                        <asp:HiddenField ID="IrregItemCd" runat="server"/>
                                        <icrop:CustomLabel ID="SC3290101_IrregularityItem" runat="server" UseEllipsis="False" max-width="330px" class="Ellipsis"/>
                                        <div class="Marker" runat="server">
                                            <icrop:CustomLabel ID="SC3290101_Marker" runat="server" UseEllipsis="False" />
                                        </div>
                                    </td>
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT WhiteBKDT">
                                        <icrop:CustomLabel ID="SC3290101_NoOfStaffs" runat="server" UseEllipsis="False" />
                                    </td> 
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTED WhiteBKDT">
                                        <icrop:CustomLabel ID="SC3290101_NoOfIrregularities" runat="server" UseEllipsis="False"/>
                                        <div id="SC3290101_FuriateStaffNotDiv" runat="server">
                                            <asp:Panel ID="SC3290101_StaffAssignToCustCountPanel" runat="server">
                                                    <div class="SC3290101_LoadingAnimation2 show2"></div>
                                                    <icrop:CustomLabel id="SC3290101_StaffAssignToCustCount" runat="server" UseEllipsis="False" max-width="118px" class="Ellipsis"/>
                                            </asp:Panel>
                                            <asp:Panel ID="SC3290101_UnallocatedActivityCountPanel" runat="server">
                                                    <div class="SC3290101_LoadingAnimation2 show2"></div>
                                                    <icrop:CustomLabel id="SC3290101_UnallocatedActivityCount" runat="server" UseEllipsis="False" max-width="118px" class="Ellipsis"/>
                                            </asp:Panel>
                                        </div>
                                    </td> 
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div> 		

            <%' 担当未振当て件数の更新用パネル %>
            <asp:UpdatePanel ID="SC3290101_FuriateStaffNotUpdatePanel" runat="server" RenderMode="Block" UpdateMode="Conditional" style="display:none;">
                <ContentTemplate>
                    <asp:HiddenField ID="SC3290101_FuriateStaffNotType" runat="server"/>
                    <icrop:CustomLabel id="SC3290101_TempStaffAssignToCustCount" runat="server" UseEllipsis="False" />
                    <icrop:CustomLabel id="SC3290101_TempUnallocatedActivityCount" runat="server" UseEllipsis="False" />
                    <asp:Button ID="SC3290101_FuriateStaffNotUpdateButton" runat="server" style="display:none;" />
                </ContentTemplate>
            </asp:UpdatePanel>	
        </form>
    </div>
</body>
</html>