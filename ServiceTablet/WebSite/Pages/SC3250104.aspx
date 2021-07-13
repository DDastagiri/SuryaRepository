<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false"  CodeFile="SC3250104.aspx.vb" Inherits="SC3250104"  %>

<asp:Content ID="cont_content" ContentPlaceHolderID="content" Runat="Server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="format-detection" content="telephone=no" />
    <link rel="Stylesheet" href="../Styles/SC3250104/SC3250104.css?201408130000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3250104/SC3250104.js?201906060001"></script>	

    <!-- 中央部分-->
    <div id="SC3250104OldNew">
        
        <div id="main">
            <asp:HiddenField ID="hdnRegisterFile" runat="server"/>
            <asp:HiddenField ID="hdnKeepKey" runat="server"/>
            <!--2つめのコンテンツブロック-->
            <div id="oldParts">
                <%-- <asp:Image id="imageOldParts" width="435" height="263" alt="" runat="server" /> --%>
                <asp:Image id="imageOldParts" width="435" height="326" alt="" runat="server" />
                <p><strong><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="1" /></strong></p>
            </div>
            <div id="newParts">
                <%-- <asp:Image id="imageNewParts" width="435" height="263" alt="" runat="server" /> --%>
                <asp:Image id="imageNewParts" width="435" height="326" alt="" runat="server" />
                <p><strong><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="2" /></strong></p>
            </div>
            <div id="ThumbnailCount" class="PartsIcon" runat="server"></div>
            <br class="clear" />

        </div>
    </div>
    <!-- ここまで中央部分 -->

</asp:Content>
