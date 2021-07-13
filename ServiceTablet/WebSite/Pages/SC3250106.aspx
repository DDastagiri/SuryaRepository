<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false"  CodeFile="SC3250106.aspx.vb" Inherits="SC3250106"  %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="cont_content" ContentPlaceHolderID="content" Runat="Server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="format-detection" content="telephone=no" />
    <link rel="Stylesheet" href="../Styles/SC3250106/SC3250106.css?201409021432" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3250106/SC3250106.js?201408211103"></script>	
    <!-- 横スクロールを実装するためのJQuery -->
    <script type="text/javascript" src="../Scripts/SC3250106/SC3250106.flickable.js?201408202057"></script>

    <!-- 中央部分-->
    <div class="MsgArea"><icrop:CustomLabel id="lblErrMessage" runat="server" UseEllipsis="true" /></div>
    <div id="main" runat="server">

        <!-- HiddenField宣言 start -->
        <asp:HiddenField ID="hdnSelectChart" runat="server"/>
        <!-- HiddenField宣言 end -->

        <!--4つめのコンテンツブロック-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" id="MainGraph" runat="server">
            <tr>
                <td scope="col">
                <div class="SubTitleArea"><h5 id="SubTitleTextFL" runat="server"></h5></div>
                    <div id="ChartArea1" runat="server">
                    <div onclick="ClickChart('1');">
                    <asp:Chart id="MyChartFL" class="MyChart" runat="server" ImageStorageMode="UseHttpHandler" ImageType="Jpeg" Compression="10">
                    </asp:Chart>
                    </div>
                    </div>
                </td>
                <td scope="col">
                <div class="SubTitleArea"><h5 id="SubTitleTextFR" runat="server"></h5></div>
                    <div id="ChartArea2" runat="server">
                    <div onclick="ClickChart('2');">
                    <asp:Chart id="MyChartFR" class="MyChart" runat="server" ImageStorageMode="UseHttpHandler" ImageType="Jpeg" Compression="10">
                    </asp:Chart>
                    </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td scope="col">
                <div class="SubTitleArea"><h5 id="SubTitleTextRL" runat="server"></h5></div>
                    <div id="ChartArea3" runat="server">
                    <div onclick="ClickChart('3');">
                    <asp:Chart id="MyChartRL" class="MyChart" runat="server" ImageStorageMode="UseHttpHandler" ImageType="Jpeg" Compression="10">
                    </asp:Chart>
                    </div>
                    </div>
                </td>
                <td scope="col">
                <div class="SubTitleArea"><h5 id="SubTitleTextRR" runat="server"></h5></div>
                    <div id="ChartArea4" runat="server">
                    <div onclick="ClickChart('4');">
                    <asp:Chart id="MyChartRR" class="MyChart" runat="server" ImageStorageMode="UseHttpHandler" ImageType="Jpeg" Compression="10">
                    </asp:Chart>
                    </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <!-- 2014/08/19 拡大画面表示機能追加　START　↓↓↓-->
    <!-- 拡大画面用 -->
    <div id="contentsMainonBoard" style="display:none" onclick="ClosePopUp();" runat="server"></div>
    <div id="closeBtn" style="display:none;" runat="server" onclick="ClosePopUp();"/>
    
    <div id="popUpWindow" class="pdcontentBox" style="display:none;" runat="server">
        <h4><icrop:CustomLabel id="LargeContentTitle" runat="server" UseEllipsis="true"/></h4>
        <div id="LargeScrollArea">
        <asp:Chart id="MyLargeChart" class="MyLargeChart" runat="server" ImageStorageMode="UseHttpHandler" ImageType="Jpeg" Compression="10">
        </asp:Chart>
        </div>
        <!-- スクロールエリアの上に配置するスクロールしない余白部分 -->
        <div id="CoverLeft"></div>
        <div id="CoverRight" >
            <div id="CoverRight2" style="position:absolute;"></div>
        </div>
    </div>

    <!-- クルクル画面用 -->
    <div id="ServerProcessOverlayBlack"></div>
    <div id="ServerProcessIcon"></div>
    <!-- 2014/08/19 拡大画面表示機能追加　END　　↑↑↑-->

</asp:Content>
