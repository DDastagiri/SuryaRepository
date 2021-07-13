<%@ Page Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3230101.aspx.vb" Inherits="Pages_SC3230101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link type="text/css" rel="stylesheet" href="<%=WebResource.GetUrl("../Styles/SC3230101/SC3230101.css")%>" media="screen,print" />
    <link type="text/css" rel="stylesheet" href="<%=WebResource.GetUrl("../Styles/SC3230101/common.css")%>" media="screen,print" />
    <script type="text/javascript" src="<%=WebResource.GetUrl("../Scripts/SC3230101/SC3230101.js")%>"></script>
<%--    <link type="text/css" rel="stylesheet" href="../Styles/SC3230101/a-fm.css"    media="screen,print" />
    <link type="text/css" rel="stylesheet" href="../Styles/SC3230101/a-fm-01.css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3230101/SC3230101.js"></script>
--%>
    <script type="text/javascript">
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <%-- ここからコンテンツ --%> 
    <asp:HiddenField ID="hdnUrl" runat="server" Value="" />
    <asp:Button ID="hdnBtnNextPage" runat="server" CssClass="HiddenBtn"/>
    <asp:Button ID="hdnBtnRefreshPage" runat="server" CssClass="HiddenBtn"/>

    <div id="mainblock">
        <div class="mainblockWrap">
            <div id="mainblockContent">

                <div class="AddJobApprArea">
<%--                    <iframe src="SC3230102.aspx" id="additionalArea" class="AreaClass" runat="server">この部分は追加作業承認待ちエリアです</iframe>--%> 
                    <div class="ChipArea">
                        <h2 class="ContentTitle">
                            <icrop:CustomLabel ID="lblAddJobApprTitle" CssClass="TitleLabel" runat="server" TextWordNo="2"
                                text="" Width="420px" UseEllipsis="True"></icrop:CustomLabel>
                        </h2>
                        <div class="ContentTitleCount">
                            <icrop:CustomLabel id="lblAddJobApprCount" CssClass="CountLabel" runat="server" 
                                text="0" UseEllipsis="False"></icrop:CustomLabel>
                        </div>
                        <div class="InBox" id="AddJobApprChips" runat="server">
                            <%-- 車両チップ情報動的作成領域 --%> 
                        </div>
                    </div>

                </div>

                <div class="InsRltApprArea">
                    <%--<iframe src="SC3230103.aspx" id="InspectionArea" class="AreaClass" runat="server">この部分は完成検査承認待ちエリアです</iframe>--%> 
                    <div class="ChipArea">
                        <h2 class="ContentTitle">
                            <icrop:CustomLabel ID="lblInsRltApprTitle" CssClass="TitleLabel" runat="server" TextWordNo="3"
                                text="" Width="420px" UseEllipsis="True"></icrop:CustomLabel>
                        </h2>
                        <div class="ContentTitleCount">
                            <icrop:CustomLabel id="lblInsRltApprCount" CssClass="CountLabel" runat="server" 
                                text="0" UseEllipsis="False"></icrop:CustomLabel>
                        </div>
                        <div class="InBox" id="InsRltApprChips" runat="server">
                            <%-- 車両チップ情報動的作成領域 --%> 
                        </div>
                    </div>
                </div>

            </div>  <%-- mainblockContent--%> 
        </div>      <%-- mainblockWrap--%> 
    </div>          <%-- mainblock--%> 
    <%-- ここまでコンテンツ --%> 
</asp:Content>

<asp:Content ID="cont_footer" ContentPlaceHolderID="footer" Runat="Server">
    <%-- ここからフッタ --%> 
    <div style="margin:0px 0px 0px 855px;">
    </div>
    <%-- ここまでフッタ --%> 
</asp:Content>

