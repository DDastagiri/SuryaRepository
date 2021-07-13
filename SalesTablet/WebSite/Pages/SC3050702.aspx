<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3050702.aspx.vb" Inherits="Pages_SC3050702" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3050702/SC3050702.css?20121123000000" />
    <script type="text/javascript" src="../Scripts/SC3050702/SC3050702.js?20121123000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

<%'AJAX用 %>
<asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>

<!-- ここからメインブロック -->
    <div id="mainblock">
        <asp:UpdatePanel ID="SalesPointListPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="mainblockWrap">
            <div id="mainblockContent">
                <div class="mainblockContentArea">
                    <div class="mainblockContentAreaWrap">
                        <!--ここから一覧上部-->
                        <div class="upside">
                            <table width="100%">
                                <tr>
                                    <td width="17%" align="left">
                                        <%--Ajax送出エリア Start--%>
                                        <asp:UpdatePanel ID="AjaxCurLineup" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--車種選択リスト--%>
                                                <asp:DropDownList ID="DropDownList_Vehicle" class="vehicle" runat="server" AutoPostBack="False" OnChange="changeCarLineUp()" />
                                                <%--非同期イベントを送出する非表示ボタン--%>
                                                <asp:Button ID="RestoreButton" runat="server" Text="" style="display:none;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--Ajax送出エリア End--%>
                                    </td>
                                    <td width="2%"></td>
                                    <td width="28%" align="center" valign="middle">
                                        <span class="switcher">
                                            <div id="exterior" class="switcher_ex_on" onclick="switchOrnament(exterior)">
                                                <span>
                                                    <icrop:CustomLabel ID="exteriorLabel" runat="server" TextWordNo="2" UseEllipsis="false" width="95px" CssClass="ellipsis" />
                                                </span>
                                            </div>
                                            <div id="interior" class="switcher_in_off" onclick="switchOrnament(interior)">
                                                <span>
                                                    <icrop:CustomLabel ID="interiorLabel" runat="server" TextWordNo="3" UseEllipsis="false" width="95px" CssClass="ellipsis" />
                                                </span>
                                            </div>
                                        </span>
                                    </td>
                                    <td width="43%"></td>
                                    <td width="7%" align="right">
                                        <div id="addDiv" class="add" onclick="addSalesPointInfo();" >
                                            <a href="#">
                                                <icrop:CustomLabel ID="addButtonLabel" runat="server" TextWordNo="4" Width="80px" CssClass="ellipsis" />
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <!--ここまで一覧上部-->

                        <div class="salesPointListArea">
                            <!--ここから一覧-->
                            <div class="salesPointListHead">
                                <table>
                                    <tr>
                                        <th><icrop:CustomLabel ID="titleLabel" runat="server" TextWordNo="5" UseEllipsis="false" width="230px" CssClass="ellipsis" /></th>
                                        <th><icrop:CustomLabel ID="contextLabel" runat="server" TextWordNo="6" UseEllipsis="false" width="580px" CssClass="ellipsis" /></th>
                                        <th><icrop:CustomLabel ID="sortNoLabel" runat="server" TextWordNo="7" UseEllipsis="false" width="50px" CssClass="ellipsis" /></th>
                                    </tr>
                                </table>
                            </div>
                            <div id="boxscroll" class="salesPointList" >
                                <table id="boxscrollTable" >
                                <% If Not Me.HiddenRowCount.Value.Equals(RowCountNone) Then%>
                                    <asp:Repeater ID="repeaterSalesPointInfo" runat="server" >
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <div class="salesPointLink ellipsis">
                                                        <a onclick='editSalesPoint("<%# DataBinder.Eval(Container.DataItem, "id")%>");' href="#<%# DataBinder.Eval(Container.DataItem, "id")%>" >
                                                            <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "title"))%>
                                                        </a>
                                                    </div>
                                                </td>
                                                <td>
                                                    <label>
                                                        <icrop:CustomLabel ID="contentsLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "contents"))%>' UseEllipsis="false" Width="588px" CssClass="ellipsis" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <input type="text" id="sortNo" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "sortNo")%>' maxlength="3" onchange="onChangeDisplay();" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <% Else%>
                                        <tr>
                                            <td class="NoRequest" style="width:905px;height:32px;text-align:center;"><icrop:CustomLabel ID="NoRequestMsg" runat="server" /></td>
                                        </tr>
                                    <% End If%>
                                </table>
                            </div>
                            <!--ここまで一覧-->

                            <%'refreshのダミーボタン %>
                            <asp:Button ID="RefreshButton" runat="server" style="display:none" />

                            <asp:HiddenField ID="carSelectField" runat="server" />
                            <asp:HiddenField ID="exInField" runat="server" />
                            <asp:HiddenField ID="salesPointIdField" runat="server" />
                            <asp:HiddenField ID="salesPointJsonField" runat="server" />
                            <asp:HiddenField ID="maxCountField" runat="server" />
                            <asp:HiddenField ID="sortNoMessageField" runat="server" />
                            <asp:HiddenField ID="maxCountMessageField" runat="server" />
                            <asp:HiddenField ID="modifyMessageField" runat="server" />
                            <asp:HiddenField ID="modifyDvsField" runat="server" />
                            <asp:HiddenField ID="HiddenRowCount" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>

        <%'保存のダミーボタン %>
        <asp:Button ID="SendButton" runat="server" style="display:none" />
        <%'編集のダミーボタン %>
        <asp:Button ID="EditButton" runat="server" style="display:none" />
        <%'追加のダミーボタン %>
        <asp:Button ID="AddButton" runat="server" style="display:none" />

    </div>

    <!-- ここまでメインブロック -->



</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

    <div id="FooterOriginalButton">
        <asp:LinkButton ID="SendButtonLink" class="send" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return sendSalesPointInfo();" >
            <icrop:CustomLabel ID="SendButtonLabel" runat="server" TextWordNo="8" UseEllipsis="false" Width="80px" CssClass="ellipsis" />
        </asp:LinkButton>
        <asp:Label ID="Label1" runat="server" Width="10"></asp:Label>
    </div>


    <%'登録時のオーバーレイ %>
    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>

</asp:Content>

