<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3170209.aspx.vb" Inherits="Pages_SC3170209" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />

    <link rel="stylesheet" href="Styles/SC3170209/SC3170209.css?20191028000001" type="text/css" media="screen" />
    <script type="text/javascript" src="Scripts/jquery-1.5.2.min.js"></script>
    <script type="text/javascript" src="Scripts/icropScript.js?20131202180000"></script>
    <script type="text/javascript" src="Scripts/SC3170209/jquery.flexslider.js"></script>
    <script type="text/javascript" src="Scripts/SC3170209/SC3170209.js?20191028000001"></script>

    <title></title>
</head>

<body id="RoThumbnailImage">
<form runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true"/>
    <!-- クルクル領域 -->
    <div id="LoadingScreen" runat="Server">
        <div id="LoadingWrap">
            <div class="loadingIcn">
                <img src="Styles/Images/animeicn-1.png" width="38" height="38" alt="" />
            </div>
        </div>
    </div>
    <div class="flex-container">
        <div class="FlexSliderUpper">
            <div class="ControlNaviArea"></div>
            <div class="ControlNaviAreaRight">
                <asp:Label ID="Label_PhotoCount" runat="server" class="PhotoCount"></asp:Label>
                <a href="#" id="A_CameraButtom" runat="server" class="CameraBottom">
                    <img id="Img_Camera" src="Styles/Images/SC3170209/ssa08_AdviceIcon01.png"/>
                </a>
            </div>
       </div>

        <div class="flexslider">
            <asp:ListView runat="server" ID="ListView_ThumbnailImgList">

                <LayoutTemplate>
                    <ul class="slides">
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                    </ul>
                </LayoutTemplate>

                <ItemTemplate>
                    <li>
                        <img src="<%#Eval("image1") %>"/>
                        <img src="<%#Eval("image2") %>"/>
                    </li>
                </ItemTemplate>

                <EmptyDataTemplate>
                </EmptyDataTemplate>

            </asp:ListView>
        </div>
    </div>

    <!-- 隠し項目 -->
    <asp:HiddenField id="Hidden_DlrCd" runat="server" />
    <asp:HiddenField id="Hidden_BrnCd" runat="server" />
    <asp:HiddenField id="Hidden_VisitSeq" runat="server" />
    <asp:HiddenField id="Hidden_BasrezId" runat="server" />
    <asp:HiddenField id="Hidden_RoNo" runat="server" />
    <asp:HiddenField id="Hidden_RoSeqNo" runat="server" />
    <asp:HiddenField id="Hidden_VinNo" runat="server" />
    <asp:HiddenField id="Hidden_PictureGroup" runat="server" />
    <asp:HiddenField id="Hidden_CaptureGroup" runat="server" />
    <asp:HiddenField id="Hidden_LoginUserId" runat="server" />
    <asp:HiddenField id="Hidden_LinkSysType" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailId" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath" runat="server" />
    <asp:HiddenField id="Hidden_PictureFormat" runat="server" />
    <!-- 画像アップロード管理項目("0":非アップロード / "1":アップロード中) -->
    <asp:HiddenField id="Hidden_UploadFlag" runat="server" />

    <!-- メッセージ関連 -->
    <asp:HiddenField ID="Hidden_MessageSaveImageFailure" runat="server" />

</form>
</body>
</html>
