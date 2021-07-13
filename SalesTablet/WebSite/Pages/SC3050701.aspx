<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3050701.aspx.vb" Inherits="Pages_SC3050701" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--スクリプト(画面固有)--%>
    <link rel="Stylesheet" href="../Styles/SC3050701/SC3050701.css?20121123000000" type="text/css" media="all" />
    <script type="text/javascript" src="../Scripts/SC3050701/SC3050701.js?20130214100000"></script>
</asp:Content>

<asp:Content id="Content2" ContentPlaceHolderID="content" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
	<%--コンテンツフレーム Start--%>
    <div id="mainblock">
        <div class="mainblockWrap">
            <div id="mainblockContent">
                <div class="mainblockContentArea">
                    <div class="mainblockContentAreaWrap">
		                <%--一覧上部 Start--%>
                        <div class="upside">
                            <table>
                                <tr>
                                    <td>
                                        <%--Ajax送出エリア Start--%>
                                        <asp:UpdatePanel ID="AjaxCurLineup" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--車種選択リスト--%>
                                                <asp:DropDownList ID="DropDownCarLineup" class="vehicle" runat="server" AutoPostBack="False" OnChange="changeCarLineup()" />
                                                <%--非同期イベントを送出する非表示ボタン--%>
                                                <asp:Button ID="RestoreButton" runat="server" Text="" style="display:none;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--Ajax送出エリア End--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
		                <%--一覧上部 End--%>
                        
                        <%--Ajax送出エリア Start--%>
                        <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <%--一覧全体 Start--%>
                                <div class="contentsMenuListArea">
                                    <%--一覧ヘッダ Start--%>
                                    <div class="contentsMenuListHead">
                                        <table>
                                            <tr>
                                                <th><icrop:CustomLabel ID="HeaderMenu" runat="server" TextWordNo="2" UseEllipsis="false" Width="128px" CssClass="ellipsis" /></th>
                                                <th><icrop:CustomLabel ID="HeaderIcon" runat="server" TextWordNo="3" UseEllipsis="false" Width="194px" CssClass="ellipsis" /></th>
                                                <th><icrop:CustomLabel ID="HeaderURL" runat="server" TextWordNo="4" UseEllipsis="false" Width="467px" CssClass="ellipsis" /></th>
                                                <th><icrop:CustomLabel ID="HeaderOrder" runat="server" TextWordNo="5" UseEllipsis="false" Width="54px" CssClass="ellipsis" /></th>
                                                <th><icrop:CustomLabel ID="HeaderDelete" runat="server" TextWordNo="6" UseEllipsis="false" Width="75px" CssClass="ellipsis" /></th>
                                            </tr>
			                            </table>
		                            </div>
                                    <%--一覧ヘッダ End--%>
        
                                    <%--一覧明細 Start--%>
                                    <div id="boxscroll" class="contentsMenuList">
                                        <table id="boxscrollTable">
                                            <asp:Repeater ID="RepeaterList" runat="server" >
                                                <ItemTemplate>
                                                        <tr>
                                                            <td><input type="text" id="SC3050701_Menu" runat="server" maxlength="7" onchange="edit()" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "name"))%>' /></td>
                                                            <td>
                                                                <div class="iconArea">
                                                                    <div name="SC3050701_Frame" class='<%# IIf(String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "IconPath")), "frameOn", "frameOff")%>'>
                                                                        <img name="SC3050701_Icon" class='<%# IIf(String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "IconPath")), "hidden", "visible")%>' src='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "IconPath"))%>' alt="" />
                                                                    </div>
                                                                    <div class="reference">
                                                                        <asp:FileUpload ID="SC3050701_File" runat="server" onchange="edit()" />
                                                                        <input type="hidden" id="SC3050701_IconNameNew" runat="server" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "IconNameNew"))%>' />
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td><input type="text" id="SC3050701_Url" runat="server" maxlength="512" onchange="edit()" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "url"))%>' /></td>
                                                            <td><input type="text" id="SC3050701_Order" runat="server" maxlength="1" onchange="edit()" value='<%# ToOrderForDisplay(DataBinder.Eval(Container.DataItem, "Order"))%>' /></td>
                                                            <td><div class="del" onclick="clearRow(<%# Container.ItemIndex%>)"><a href="#"><icrop:CustomLabel ID="ListDelete" runat="server" TextWordNo="7" /></a></div></td>
                                                            <input type="hidden" id="SC3050701_ID" runat="server" value='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "id"))%>' />
                                                        </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <%--一覧明細 End--%>
                                </div>
                                <%--一覧全体 End--%>

                                <%--保持情報--%>
                                <asp:HiddenField ID="HiddenFooterJson" runat="server" />
                                <asp:HiddenField ID="HiddenTimeStamp" runat="server" />
                                <%--非同期イベントを送出する非表示ボタン--%>
                                <asp:Button ID="RefreshButton" runat="server" Text="" style="display:none;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--Ajax送出エリア End--%>
                        
                        <%--Ajax送出エリア Start--%>
                        <asp:UpdatePanel ID="AjaxValidator" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <%--保持情報--%>
                                <asp:HiddenField ID="HiddenState" runat="server" />
                                <%--非同期イベントを送出する非表示ボタン--%>
                                <asp:Button ID="ValidationButton" runat="server" Text="" style="display:none;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--Ajax送出エリア End--%>

                        <%--保持情報--%>
                        <asp:HiddenField ID="HiddenMsgConfirmDiscard" runat="server" />
                        <asp:HiddenField ID="HiddenMsgConfirmDelete" runat="server" />
                        <asp:HiddenField ID="HiddenErrRequiredMenu" runat="server" />
                        <asp:HiddenField ID="HiddenErrInvalidMenu" runat="server" />
                        <asp:HiddenField ID="HiddenErrRequiredURL" runat="server" />
                        <asp:HiddenField ID="HiddenErrInvalidURL" runat="server" />
                        <asp:HiddenField ID="HiddenErrFileSize" runat="server" />
                        <asp:HiddenField ID="HiddenErrFileKind" runat="server" />
                        <asp:HiddenField ID="HiddenErrNumericOrder" runat="server" />
                        <asp:HiddenField ID="HiddenMaxFileSize" runat="server" />
                        <asp:HiddenField ID="HiddenSeries" runat="server" />
                        <asp:HiddenField ID="HiddenAppId" runat="server" />
                        <%--同期イベントを送出する非表示ボタン--%>
                        <asp:Button ID="FooterButton" runat="server" Text="" style="display:none;" />
                        <asp:Button ID="SaveButton" runat="server" Text="" style="display:none;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--コンテンツフレーム End--%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

    <div id="FooterOriginalButton">
        <asp:LinkButton ID="SaveButtonLink" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return validate();" >
            <icrop:CustomLabel ID="SaveButtonLabel" runat="server" TextWordNo="8" ></icrop:CustomLabel>
        </asp:LinkButton>
        <asp:Label ID="Label1" runat="server" Width="10"></asp:Label>
    </div>

    <%--ローディング--%>
    <div id="registOverlayBlack" runat="server"></div>
    <div id="processingServer" runat="server"></div>

</asp:Content>