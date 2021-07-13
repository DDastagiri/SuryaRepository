<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SC3160219.aspx.vb" Inherits="Pages_SC3160219" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>

    <link rel="stylesheet" href="Styles/SC3160219/SC3160219.css?20200929000001" type="text/css" media="screen,print" />

    <script type="text/javascript" src="Scripts/jquery-1.5.2.min.js?20140220000000"></script>
    <script type="text/javascript" src="Scripts/SC3160219/SC3160219.js?20200929000001"></script>

    <title></title>
</head>

<body style="margin:0px 0px;">
<form id="PopUpForm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true"/>
    <!-- クルクル領域 -->
    <div id="LoadingScreen" runat="Server">
        <div id="LoadingWrap">
            <div class="loadingIcn">
                <img src="Styles/Images/animeicn-1.png" width="38" height="38" alt="" />
            </div>
        </div>
    </div>
    <!-- RO損傷登録ポップアップ -->
    <div id="PopUpBlock" runat="Server" class="PopUpContentBody">

        <div class="DamageInfoInputArea">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

           <!-- ダメージ種別ボタン -->
           <div id="Div_DamageTypeDammyButton_1" runat="server" class="DamageTypeButton">
               <span class="LegendPoint">
                   <span id="Span_DamageTypeSymbol_1" runat="server" class="InRound"/>
               </span>
                <p><a href="#" id="Anchor_DamageTypeButton_1" runat="server" /></p>
            </div>
 
            <div id="Div_DamageTypeDammyButton_2" runat="server" class="DamageTypeButton">
                <span class="LegendPoint">
                    <span id="Span_DamageTypeSymbol_2" runat="server" class="InRound"/>
                </span>
                <p><a href="#" id="Anchor_DamageTypeButton_2" runat="server"/></p>
            </div>

            <div id="Div_DamageTypeDammyButton_3" runat="server" class="DamageTypeButton">
                <span class="LegendPoint">
                    <span id="Span_DamageTypeSymbol_3" runat="server" class="InRound"/>
                </span>
                <p><a href="#" id="Anchor_DamageTypeButton_3" runat="server"/></p>
            </div>

            <div id="Div_DamageTypeDammyButton_4" runat="server" class="DamageTypeButton">
                <span class="LegendPoint">
                    <span id="Span_DamageTypeSymbol_4" runat="server" class="InRound"/>
                </span>
                <p><a href="#" id="Anchor_DamageTypeButton_4" runat="server"/></p>
            </div>

            <div id="Div_DamageTypeDammyButton_5" runat="server" class="DamageTypeButton">
                <span class="LegendPoint">
                    <span id="Span_DamageTypeSymbol_5" runat="server" class="InRound"/>
                </span>
                <p><a href="#" id="Anchor_DamageTypeButton_5" runat="server"/></p>
            </div>	
                   
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- MEMO -->
        <icrop:CustomTextBox id="TextBox_Memo" runat="server" TextMode="MultiLine" OnClientClear="" CssClass="MemoInputArea" MaxLength="256" PlaceHolderWordNo="12" TabIndex="3" />

        </div>
        <%--<div class="scroll-contents">--%>
        <!-- 写真 -->
            <div id="Div_PhotoArea" runat="server" class="PhotoArea">
                <div class="PhotoBox" runat="server" id="PhotoBox">
                    <input type="image" id="Input_DeletePhotoButton1" runat="server" src="Styles/Images/SC3160219/Minusbotton.png" class="DeletePhotoButton"/>
                    <img id="Img_DmagePhoto1" runat="server" src="Styles/Images/SC3160219/ssa06photo.png" width="240" height="180" />
                </div>
                <div class="PhotoBox1" runat="server" id="PhotoBox1">
                    <input type="image" id="Input_DeletePhotoButton2" runat="server" src="Styles/Images/SC3160219/Minusbotton.png" class="DeletePhotoButton"/>
                    <img id="Img_DmagePhoto2" runat="server" src="Styles/Images/SC3160219/ssa06photo.png" width="240" height="180" />
                </div>
                <div class="PhotoBox2" runat="server" id="PhotoBox2">
                    <input type="image" id="Input_DeletePhotoButton3" runat="server" src="Styles/Images/SC3160219/Minusbotton.png" class="DeletePhotoButton"/>
                    <img id="Img_DmagePhoto3" runat="server" src="Styles/Images/SC3160219/ssa06photo.png" width="240" height="180" />
                </div>
                <div class="PhotoBox3" runat="server" id="PhotoBox3">
                    <input type="image" id="Input_DeletePhotoButton4" runat="server" src="Styles/Images/SC3160219/Minusbotton.png" class="DeletePhotoButton"/>
                    <img id="Img_DmagePhoto4" runat="server" src="Styles/Images/SC3160219/ssa06photo.png" width="240" height="180" />
                </div>
                <div class="PhotoBox4" runat="server" id="PhotoBox4">
                    <input type="image" id="Input_DeletePhotoButton5" runat="server" src="Styles/Images/SC3160219/Minusbotton.png" class="DeletePhotoButton"/>
                    <img id="Img_DmagePhoto5" runat="server" src="Styles/Images/SC3160219/ssa06photo.png" width="240" height="180" />
                </div>
            </div>
<%--        </div>--%>
        <!-- カメラ -->
        <div id="Div_CameraArea" runat="server" class="CameraArea">
            <div id="Div_CameraDammyButtom_Img" runat="server">
                <a href="#" id="A_CameraButtom" runat="server" class="CameraBottomBlue">
                    <img id="Img_Camera" src="Styles/Images/SC3160219/KSPopIcon.png" class="CameraButtomImg"/>
                </a>
            </div>
            <div id="Div_CameraDammyButtom_Retake" runat="server">
<%--                <a href="disabled" id="A_CameraButtom_Retake" runat="server" class="CameraBottomWhite"/>--%>
                <a href="javascript:void(0)" id="A_CameraButtom_Retake" runat="server" class="CameraBottomWhite"/>
                               <img id="Img_Camera1" src="Styles/Images/SC3160219/KSPopIcon.png" class="CameraButtomImg"/>

                 </div>
        </div>
    </div>
 
    <!-- 隠し項目 -->
    <asp:HiddenField id="Hidden_DamageTypeCount" runat="server" />
    <asp:HiddenField id="Hidden_DispMode" runat="server" />
    <asp:HiddenField id="Hidden_RoExteriorId" runat="server" />
    <asp:HiddenField id="Hidden_PartsType" runat="server" />
    <asp:HiddenField id="Hidden_LoginUserId" runat="server" />
    <asp:HiddenField id="Hidden_Title" runat="server" />
    <asp:HiddenField id="Hidden_CancelTitle" runat="server" />
    <asp:HiddenField id="Hidden_DoneTitle" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdOrg1" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdOrg2" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdOrg3" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdOrg4" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdOrg5" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdDel1" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdDel2" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdDel3" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdDel4" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailIdDel5" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath1" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath2" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath3" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath4" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPath5" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgPathSeq" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgSeq1" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgSeq2" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgSeq3" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgSeq4" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgSeq5" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgOrg1" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgOrg2" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgOrg3" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgOrg4" runat="server" />
    <asp:HiddenField id="Hidden_RoThumbnailImgOrg5" runat="server" />
    <asp:HiddenField id="Hidden_DoneClickFlg" runat="server" />
    <asp:HiddenField id="Hidden_OriginalImageFilePath1" runat="server" />
    <asp:HiddenField id="Hidden_OriginalImageFilePath2" runat="server" />
    <asp:HiddenField id="Hidden_OriginalImageFilePath3" runat="server" />
    <asp:HiddenField id="Hidden_OriginalImageFilePath4" runat="server" />
    <asp:HiddenField id="Hidden_OriginalImageFilePath5" runat="server" />
    <!-- 画像アップロード管理項目("0":非アップロード / "1":アップロード中) -->
    <asp:HiddenField id="Hidden_UploadFlag" runat="server" />
    <!-- メッセージ関連 -->
    <asp:HiddenField ID="Hidden_MessageSaveImageFailure" runat="server" />
    <!-- javascript書き出し -->
    <asp:Literal id="Literal_JavaScript" runat="server"/>

</form>
</body>
</html>
