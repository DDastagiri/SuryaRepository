<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false"  CodeFile="SC3250105.aspx.vb" Inherits="SC3250105"  %>

<asp:Content ID="cont_content" ContentPlaceHolderID="content" Runat="Server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="format-detection" content="telephone=no" />
    <link rel="Stylesheet" href="../Styles/SC3250105/SC3250105.css?201408120000" type="text/css" media="screen,print" />

    <!-- 中央部分-->
    <div id="main">
        <!--3つめのコンテンツブロック-->
        <dl>
            <dd class="pLeftIltems">
                <h5><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="1" /></h5>
                <div id="LastChangeDate" class="dateSet" runat="server"></div>
            </dd>
            <dd class="pRightIltems">
                <h5><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="2" /></h5>
                <div id="LastChangeMileage" class="dateSet" runat="server"></div>
            </dd>
        </dl>
    </div>
    <!-- ここまで中央部分 -->

</asp:Content>
