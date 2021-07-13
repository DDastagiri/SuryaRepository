<%@ Page Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3290103.aspx.vb" Inherits="Pages_SC3290103" %>

<%@ Register src="~/Pages/SC3290104.ascx" tagname="SC3290104" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../Styles/SC3290103/SC3290103.css?20140625000002" type="text/css" />
    <script type="text/javascript" src="../Scripts/SC3290103/SC3290103.js?20140625000002"></script>
</asp:Content>

<asp:Content id="Content2" ContentPlaceHolderID="content" runat="server">

    <%' 非同期読み込みのためのScriptManagerタグ %>
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <asp:HiddenField ID="AlertMessage" runat="server" />
    <asp:HiddenField ID="LoginStaffCode" runat="server" />

<div id="SC3290103_Panel" runat="server">

    <%' ボディー（アップデートパネル） %>
    <asp:UpdatePanel ID="SC3290103_Content" runat="server" class='content' UpdateMode="Conditional">
        <ContentTemplate>

    <%' 再描画用の隠しボタン %>
    <asp:Button ID="SC3290103_LoadSpinButton" runat="server" style="display:none;" />

    <div id="contentsLeftBox1" class="contentsFrame">
        <h2 class="contentTitle wt01">
            <icrop:CustomLabel ID="ColumNameTitle" runat="server" UseEllipsis="False" width="300px" class="Ellipsis" />
        </h2>
        <div class="InnerBox01">

            <div id="TBL_Box01">
                <table id="table01" border=0 cellspacing=0 cellpadding=0>
                    <thead>
                        <tr id = "table01HeadR"> 
                            <th class="TBL_HDST W189">
                                <div class="Hidden W189">
                                    <icrop:CustomLabel ID="ColumNameTeamName" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th> 
                            <th class="TBL_HDCT W189">
                                <div class="Hidden W189">
                                    <icrop:CustomLabel ID="ColumNameStaffName" runat="server" UseEllipsis="False"  />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th>  
                            <th class="TBL_HDCT W120">
                                <div class="Hidden W120">
                                    <icrop:CustomLabel ID="ColumNameMonthlyGoal" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th>  
                            <th class="TBL_HDCT W120">
                                <div class="Hidden W120">
                                    <icrop:CustomLabel ID="ColumNameProgressGoal" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th>  
                            <th class="TBL_HDCT W120">
                                <div class="Hidden W120">
                                    <icrop:CustomLabel ID="ColumNameResult" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th>  
                            <th class="TBL_HDCT W120">
                                <div class="Hidden W120">
                                    <icrop:CustomLabel ID="ColumNameAchievementRate" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                            <th class="WhiteHD"></th> 
                            <th class="TBL_HDED W70">
                                <div class="Hidden W70">
                                    <icrop:CustomLabel ID="ColumNameConfirmation" runat="server" UseEllipsis="False" />
                                </div>
                            </th> 
                        </tr>
                    </thead>
                </table>
            </div>

            <%--読み込み中ウィンドウ--%>
            <div class="MstPG_LoadingScreen">
                <div class="loadingIcn"></div>         
            </div>

            <div id="TBL_Box02">

                <table id = "table02" border=0 cellspacing=0 cellpadding=0>
                    <tbody id = "table02Body">
                        <asp:Repeater ID="IrregularDetailRepeater" runat="server" >
                            <ItemTemplate>
                                <tr class="WhiteBKDT"> 
                                    <td class="TBL_DTST W189">
                                        <icrop:CustomLabel ID="LabelTeamName" runat="server" UseEllipsis="False" width="189px" class="Ellipsis" />
                                    </td>
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT W189">
                                        <icrop:CustomLabel ID="LabelStaffName" runat="server" UseEllipsis="False" width="189px" class="Ellipsis" />
                                    </td>
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT W120">
                                        <icrop:CustomLabel ID="LabelMonthlyGoal" runat="server" UseEllipsis="False" width="120px" class="Ellipsis" />
                                    </td>
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT W120">
                                        <icrop:CustomLabel ID="LabelProgressGoal" runat="server" UseEllipsis="False" width="120px" class="Ellipsis" />
                                    </td> 
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT W120">
                                        <icrop:CustomLabel ID="LabelResult" runat="server" UseEllipsis="False" width="120px" class="Ellipsis" />
                                    </td> 
                                    <td class="WhiteDT"></td> 
                                    <td class="TBL_DTCT W120">
                                        <icrop:CustomLabel ID="LabelAchievementRate" runat="server" UseEllipsis="False" width="120px" class="Ellipsis" />
                                    </td> 
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTED W70 Confirmation">
                                        <div id="ConfirmationDiv">
                                            <div id="MgrCheck" class="MGR_Check" runat="server"></div>
                                            <div id="MgrButton01"  runat="server">
                                                <span>
                                                    <icrop:CustomLabel ID="FollowDate" runat="server" UseEllipsis="False" />
                                                </span>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="IrregClassCd" runat="server" />
                                        <asp:HiddenField ID="IrregItemCd" runat="server" />
                                        <asp:HiddenField ID="StfCd" runat="server" />
                                        <asp:HiddenField ID="FllwPicStfCd" runat="server" />
                                    </td> 
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>

    <%-- フォロー設定画面のユーザコントロール --%>
    <uc1:SC3290104 ID="SC3290104" runat="server"  />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

</asp:Content>