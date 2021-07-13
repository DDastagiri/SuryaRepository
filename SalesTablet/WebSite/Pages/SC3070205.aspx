<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3070205.aspx.vb" Inherits="Pages_SC3070205" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="Stylesheet" href="../Styles/SC3070205/SC3070205.css?20140626000000" />
<script type="text/javascript" src="../Scripts/SC3070205/SC3070205.js?20140626000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<%--非同期読み込みのためのScriptManagerタグ--%>
<asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
</asp:ScriptManager>
<!-- 中央部分-->
<!-- ここからコンテンツ -->
<div id="tcvNcv50Main" style="margin-top:0px">
    <asp:UpdatePanel id="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--更新用ボタン--%>
            <asp:Button ID="UpdateButton" runat="server" style="display:none;"></asp:Button>
            <asp:HiddenField ID="approvalButtonFlgHiddenField" runat="server" value="" />
            <asp:Button ID="PrintButtonDummy" runat="server" style="display:none;"></asp:Button>
            <asp:HiddenField ID="mandatryCheckFlgHiddenField" runat="server" value="False" />
            <asp:HiddenField ID="mandatryCheckMsgHiddenField" runat="server" value="" />
            <asp:HiddenField ID="deliDateInitialValueHiddenField" runat="server" value="" />
            <asp:HiddenField ID="periodInitialValueHiddenField" runat="server" value="" />
            <asp:HiddenField ID="firstPayInitialValueHiddenField" runat="server" value="" />
            <asp:HiddenField ID="savedEstimationFlgHiddenField" runat="server" value="0" />
            <asp:HiddenField ID="blnInputChangedClientHiddenField" runat="server" value="False" />
            <asp:HiddenField ID="payMethodHiddenField" runat="server" value="" />
            <asp:HiddenField ID="payTotalHiddenField" runat="server" value="" />
            <asp:Button ID="DummySaveButton" runat="server" style="display:none;"></asp:Button>
            <asp:HiddenField ID="saveFlg" runat="server" value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%'■作成日／契約日 %>
    <div class="CreationDate" style="margin-top:15px;">
        <icrop:CustomLabel ID="estPrintDateLabel" runat="server" TextWordNo="1" />
        <icrop:CustomLabel ID="contractDateLabel" runat="server" TextWordNo="2" />
        <icrop:CustomLabel ID="dateLabel" runat="server" Text="" />
        <icrop:CustomLabel ID="CustomLabel14" runat="server" Width="10px"/>
        <icrop:CustomLabel ID="contractNoTitleLabel" runat="server" TextWordNo="91" />
        <icrop:CustomLabel ID="contractNoLabel" runat="server" Text="" />
    </div>
    <%'■カーアイコン %>
    <% If String.Equals(ApprovalStatus, "1") or String.Equals(ApprovalStatus,"2") Then%>
        <ul class="tcvNcvMyCarsList" id="carIcon"  style="margin-top:15px;display:none;">
    <% Else%>
        <ul class="tcvNcvMyCarsList" id="Ul1"  style="margin-top:15px;">
    <% End If%>
        <li id="carIcon0" class="carIcon tcvNcvCarsSwitch01 tcvNcvCarsSwitchOff"  runat="server" value="0">
            <p>
            <icrop:CustomLabel ID="carIconLabel" runat="server" TextWordNo="3" />
            </p>
        </li>
        <li id="carIcon1" class="carIcon tcvNcvCarsSwitch02 tcvNcvCarsSwitchOff" runat="server"  value="1">
            <p>
            <icrop:CustomLabel ID="carIconLabel2" runat="server" TextWordNo="86" />
            </p>
        </li>
        <li id="carIcon2" class="carIcon tcvNcvCarsSwitch03 tcvNcvCarsSwitchOff" runat="server"  value="2">
            <p>
            <icrop:CustomLabel ID="carIconLabel3" runat="server" TextWordNo="87" />
            </p>
        </li>
        <li id="carIcon3" class="carIcon tcvNcvCarsSwitch04 tcvNcvCarsSwitchOff" runat="server"  value="3">
            <p>
            <icrop:CustomLabel ID="carIconLabel4" runat="server" TextWordNo="88" />
            </p>
        </li>
        <li id="carIcon4" class="carIcon tcvNcvCarsSwitch05 tcvNcvCarsSwitchOff" runat="server"  value="4">
            <p>
            <icrop:CustomLabel ID="carIconLabel5" runat="server" TextWordNo="89" />
            </p>
        </li>
    </ul>
    <div class="tcvNcvBoxLeft" style="margin-top:50px;">
        <%'  ■見積／契約者情報 %>
        <div class="tcvNcvBoxSet">
        <h4>
        <icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="4" UseEllipsis="False" Width="160px" CssClass="clip"/>
        </h4>
        <%' □所有者/使用者 %>
        <div>
            <icrop:SegmentedButton ID="custClassSegmentedButton" name="custClassName" runat="server" class="SwitchButton1"  onClick="custChange();" TabIndex="1">
            </icrop:SegmentedButton>
        </div>
        <%' ■□所有者 %>
        <div id="syoyusya" >
        <div class="tcvNcvBoxSetIn tcvNcvBoxSetIn01">
        <%' □氏名 %>
        <div class="lefttext textRed" style="position:absolute; margin-top:3px;">
            <icrop:CustomLabel ID="CustomLabel7" runat="server" TextWordNo="7" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div class="rightbox" nowrap>
            <div>
                <icrop:CustomLabel ID="shoyusyaKeisyoMaeLabel" runat="server" UseEllipsis="False" CssClass="clip textsize20" />
                <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
                    <icrop:CustomTextBox ID="shoyusyaNameTextBox" runat="server" class="righttextName TextArea00 textsize20" AutoCompleteType="Disabled" MaxLength="256" onchange="inputChangedClient();" TabIndex="2" OnClientClear="inputChangedClient"/>
                <% Else%>
                    <% If String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Or OperationLocked = True Or Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
                        <icrop:CustomLabel ID="shoyusyaNameLabel" runat="server" class="textsize20" Width="190px" UseEllipsis="true" style="position:relative; margin-top:4px;"/>
                    <% Else%>
                        <asp:LinkButton ID="shoyusyaNameLinkButton" runat="server" class="textsize20" Width="190px" Height="46" OnClientClick="return shoyusyaNameLinkClick();">
                        <icrop:CustomLabel ID="shoyusyaNameLabel2" runat="server" class="textsize20" Width="190px" UseEllipsis="true" />
                        </asp:LinkButton>
                    <% End If%>
                <% End If%>
                <icrop:CustomLabel ID="shoyusyaKeisyoAtoLabel" runat="server" UseEllipsis="False" Width="90px" CssClass="clip textsize20" />
            </div>
        </div>
        <%' □住所 %>
        <div class="clearbothMgn">
            &nbsp;
        </div>
        <div class="lefttext textRed">
            <icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="8" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div class="rightbox">
            <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
                <icrop:CustomTextBox ID="shoyusyaZipCodeTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext1 TextArea textsize14" MaxLength="32" PlaceHolderWordNo="69" TabIndex="3" />
            <% Else%>
                <icrop:CustomLabel ID="shoyusyaZipCodeLabel" runat="server" Width="196px" UseEllipsis="true" style="position:relative; margin-top:4px;"/>
            <% End If%>
            <div class="clearbothMgn">
                &nbsp;
            </div>
            <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
                <icrop:CustomTextBox ID="shoyusyaAddressTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext2 TextArea textsize14" MaxLength="320" PlaceHolderWordNo="8" TabIndex="4"/>
            <% Else%>
                <icrop:CustomLabel ID="shoyusyaAddressLabel" runat="server"  Width="397px" UseEllipsis="true" />
            <% End If%>
        </div>
        <%' □連絡先 %>
        <div class="clearbothMgn">
            &nbsp;
        </div>
        <div class="lefttext textRed" style="padding-top:5px;">
            <icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="9" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <% Else%>
        <div class="rightboxTelNo">
        <% End If%>
        <icrop:CustomLabel ID="shoyusyaMobile" runat="server" TextWordNo="10" UseEllipsis="False" Width="29px" class="textsize12" style="position:relative; top:-1px;"/>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shoyusyaMobileTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext3 TextArea textsize14" MaxLength="128" Width="160px" TabIndex="5" style="margin-left:-5px;"/>
        <% Else%>
        <icrop:CustomLabel ID="shoyusyaMobileLabel" runat="server" class="Label3" Width="157px" UseEllipsis="true" />
        <% End If%>
        <icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="11"  UseEllipsis="False" Width="29px" class="textsize12" style="margin-left:8px; text-align:right; position:relative; top:-1px;"/>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shoyusyaTelTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext3 TextArea textsize14" MaxLength="64" Width="160px" TabIndex="6" />
        <% Else%>
        <icrop:CustomLabel ID="shoyusyaTelLabel" runat="server" class="Label3" Width="157px" UseEllipsis="true" style="margin-left:5px;"/>
        <% End If%>
        </div>
        <%' □E-Mail %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext">
        <icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="12" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div>
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <icrop:CustomTextBox ID="shoyusyaEmailTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext2 TextArea textsize14" MaxLength="128" TabIndex="7" />
        </div>
        <% Else%>
        <div class="rightboxEmail">
        <icrop:CustomLabel ID="shoyusyaEmailLabel" runat="server" Width="397px" UseEllipsis="true" />
        </div>
        <% End If%>
        <%' □国民ID %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext textRed">
        <icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="13" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <% Else%>
        <div class="rightboxKokuminid">
        <% End If%>
        <ul class="Selection0">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <li class="SelectionButton1">
        <icrop:CustomTextBox ID="shoyusyaIDTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext1 TextArea textsize14" MaxLength="32" TabIndex="8" />
        </li>
        <% Else%>
        <span>
        <li class="SelectionButton1_Label">
        <icrop:CustomLabel ID="shoyusyaIDLabel" runat="server" Width="160px" UseEllipsis="true" style="position:absolute; margin-top:-1px;"/>
        </span>
        <% End If%>
        </li>
        <icrop:CustomLabel ID="Space" runat="server" width="60px" border="1"/>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <table id="TblKubun" border = "0">
        <tr>
        <td id="ShoyushaKojin" width="22" onClick="onClickShoyushaKojin();">
        <img runat="server" id="imgChkKojin" alt="" src="../Styles/Images/checkMark02.png" TabIndex="9" />
        </td>
        <td onClick="onClickShoyushaKojin();">
        <icrop:CustomLabel ID="CustomLabelShoyusyaKojin" runat="server" TextWordNo="14" CssClass="clip" width="50px" height="20px" />
        </td>
        <td id="ShoyushaHojin" width="22" onClick="onClickShoyushaHojin();">
        <asp:Image runat="server" id="imgChkHojin" src="../Styles/Images/checkMark02.png" width="22" alt="" TabIndex="10"/>
        </td>
        <td onClick="onClickShoyushaHojin();">
        <icrop:CustomLabel ID="CustomLabelShoyusyaHojin" runat="server" TextWordNo="15" CssClass="clip" width="50px" height="20px" />
        </td>
        </tr>
        </table>
        <% Else%>
        <li class="SelectionButton2">
        <span>
        <icrop:CustomLabel ID="CustomLabelShoyusyaHojinLock" runat="server" TextWordNo="15" />
        </span>
        <span>
        <asp:Image runat="server" id="imgChkHojinLock" src="../Styles/Images/checkMark02.png" width="22"　alt="" />
        </span>
        <span>
        <icrop:CustomLabel ID="CustomLabelShoyusyaKojinLock" runat="server" TextWordNo="14" />
        </span>
        <span>
        <asp:Image runat="server" id="imgChkKojinLock" src="../Styles/Images/checkMark02.png" width="22"　alt="" />
        </span>
        </li>
        <% End If %>
        <asp:HiddenField ID="shoyusyaKojinCheckMark" runat="server" />
        <asp:HiddenField ID="shoyusyaHojinCheckMark" runat="server" />
        </ul>
        </div>
        <%' □顧客区分 %>
        <div class="clearboth">&nbsp;
        </div>
        </div>
        </div>
        <%' ■□使用者 %>
        <div id="shiyosya" style="display: none">
        <%' □コピーボタン %>
        <div>
        <input id="copyButton" type="button" value="<%=WebWordUtility.GetWord(62) %>" class="GrayIcn02" onClick="customerInfoCopy();" TabIndex="20" />
        </div>
        <div class="tcvNcvBoxSetIn tcvNcvBoxSetIn01">
        <%' □氏名 %>
        <div class="lefttext textRed" style="margin-top:3px;">
        <icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="7" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div class="rightbox" nowrap>
        <div>
        <icrop:CustomLabel ID="shiyosyaKeisyoMaeLabel" runat="server" Text="" UseEllipsis="False" CssClass="clip textsize20" />
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shiyosyaNameTextBox" runat="server" class="righttextName TextArea00 textsize20" AutoCompleteType="Disabled" MaxLength="256" onchange="inputChangedClient();" OnClientClear="inputChangedClient" TabIndex="11"/>
        <% Else%>
        <icrop:CustomLabel ID="shiyosyaNameLabel" runat="server" class="textsize20" Width="190px" UseEllipsis="true" style="position:relative; margin-top:4px;"/>
        <% End If%>
        <icrop:CustomLabel ID="shiyosyaKeisyoAtoLabel" runat="server" Text="" UseEllipsis="False" Width="90px" CssClass="clip textsize20" />
        </div>
        </div>
        <%' □住所 %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext textRed">
        <icrop:CustomLabel ID="CustomLabel52" runat="server" TextWordNo="8" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div class="rightbox">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shiyosyaZipCodeTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext1 TextArea textsize14" MaxLength="32" PlaceHolderWordNo="69" TabIndex="12" />
        <% Else%>
        <icrop:CustomLabel ID="shiyosyaZipCodeLabel" runat="server" Width="196px" UseEllipsis="true" style="position:relative; margin-top:4px;"/>
        <% End If%>
        <div class="clearbothMgn">&nbsp;
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shiyosyaAddressTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext2 TextArea textsize14" MaxLength="320" PlaceHolderWordNo="8" TabIndex="13" />
        <% Else%>
        <icrop:CustomLabel ID="shiyosyaAddressLabel" runat="server"  Width="397px" UseEllipsis="true" />
        <% End If%>
        </div>
        <%' □連絡先 %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext textRed" style="padding-top:5px;">
        <icrop:CustomLabel ID="CustomLabel62" runat="server" TextWordNo="9" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <% Else%>
        <div class="rightboxTelNo">
        <% End If%>
        <icrop:CustomLabel ID="CustomLabel63" runat="server" TextWordNo="10" UseEllipsis="False" Width="29px" class="textsize12" style="position:relative; top:-1px;"/>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shiyosyaMobileTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext3 TextArea textsize14" MaxLength="128" Width="160px" TabIndex="14" style="margin-left:-5px;"/>
        <% Else%>
        <icrop:CustomLabel ID="shiyosyaMobileLabel" runat="server" class="Label3" Width="157px" UseEllipsis="true" />
        <% End If%>
        <icrop:CustomLabel ID="CustomLabel64" runat="server" TextWordNo="11" UseEllipsis="False" Width="29px" class="textsize12" style="margin-left:8px; text-align:right; position:relative; top:-1px;" />
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <icrop:CustomTextBox ID="shiyosyaTelTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext3 TextArea textsize14" MaxLength="64" Width="160px" TabIndex="15"/>
        <% Else%>
        <icrop:CustomLabel ID="shiyosyaTelLabel" runat="server" class="Label3" Width="157px" UseEllipsis="true" style="margin-left:5px;" />
        <% End If%>
        </div>
        <%' □E-Mail %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext">
        <icrop:CustomLabel ID="CustomLabel65" runat="server" TextWordNo="12" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <div>
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <icrop:CustomTextBox ID="shiyosyaEmailTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext2 TextArea textsize14" MaxLength="128" TabIndex="16" />
        </div>
        <% Else%>
        <div class="rightboxEmail">
        <icrop:CustomLabel ID="shiyosyaEmailLabel" runat="server" Width="397px" UseEllipsis="true" />
        </div>
        <% End If%>
        <%' □国民ID %>
        <div class="clearbothMgn">&nbsp;
        </div>
        <div class="lefttext textRed">
        <icrop:CustomLabel ID="CustomLabel66" runat="server" TextWordNo="13" UseEllipsis="False" Width="47px" CssClass="clip" />
        </div>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="rightbox">
        <% Else%>
        <div class="rightboxKokuminid">
        <% End If%>
        <ul class="Selection0">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <li class="SelectionButton1">
        <icrop:CustomTextBox ID="shiyosyaIDTextBox" runat="server" onchange="inputChangedClient();" OnClientClear="inputChangedClient" class="righttext1 TextArea textsize14" MaxLength="32" TabIndex="17" />
        <% Else%>
        <span>
        <li class="SelectionButton1_Label">
        <icrop:CustomLabel ID="shiyosyaIDLabel" runat="server" Width="160px" UseEllipsis="true" style="position:absolute; margin-top:-1px;"/>
        </span>
        <% End If%>
        </li>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <table id="TblKubun" border = "0">
        <tr>
        <td id="ShiyosyaKojin" width="22" onClick="onClickShiyosyaKojin();">
        <img runat="server" id="imgChkShiyosyaKojin" alt="" src="../Styles/Images/checkMark02.png" TabIndex="18"/>
        </td>
        <td onClick="onClickShiyosyaKojin();">
        <icrop:CustomLabel ID="CustomLabelshiyosyaKojin" runat="server" TextWordNo="14" CssClass="clip" width="50px" height="20px" />
        </td>
        <td id="ShiyosyaHojin" width="22" onClick="onClickShiyosyaHojin();">
        <asp:Image runat="server" id="imgChkShiyosyaHojin" src="../Styles/Images/checkMark02.png" width="22"　alt="" TabIndex="19" />
        </td>
        <td onClick="onClickShiyosyaHojin();">
        <icrop:CustomLabel ID="CustomLabelshiyosyaHojin" runat="server" TextWordNo="15" CssClass="clip" width="50px" height="20px" />
        </td>
        </tr>
        </table>
        <% Else%>
        <li class="SelectionButton2">
        <span>
        <icrop:CustomLabel ID="CustomLabelshiyosyaHojinLock" runat="server" TextWordNo="15" />
        </span>
        <span>
        <asp:Image runat="server" id="imgChkShiyosyaHojinLock" src="../Styles/Images/checkMark02.png" width="22"　alt="" />
        </span>
        <span>
        <icrop:CustomLabel ID="CustomLabelshiyosyaKojinLock" runat="server" TextWordNo="14" />
        </span>
        <span>
        <asp:Image runat="server" id="imgChkShiyosyaKojinLock" src="../Styles/Images/checkMark02.png" width="22"　alt="" />
        </span>
        </li>
        <% End If%>
        <asp:HiddenField ID="shiyosyaKojinCheckMark" runat="server" />
        <asp:HiddenField ID="shiyosyaHojinCheckMark" runat="server" />
        </ul>
        </div>
        <%' □顧客区分 %>
        <div class="clearboth">&nbsp;
        </div>
        </div>
        </div>
        </div>
        <%' ■車種情報 %>
        <div class="tcvNcvBoxSet tcvNcvBoxSet02">
        <h4>
        <icrop:CustomLabel ID="CustomLabel16" runat="server" TextWordNo="16" UseEllipsis="False" Width="160px" CssClass="clip" />
        </h4>
        <div class="tcvNcvBoxSetIn tcvNcvBoxSetIn02">
        <div class="tcvNcv50car">
        <img runat="server" id="carImg" alt="" src="" height="100" width="160"/>
        <asp:HiddenField ID="carImgFileHidden" runat="server" value="" />
        <asp:Repeater runat="server" ID="vclInfoRepeater" ClientIDMode="Predictable">
        <ItemTemplate>
        </div>
        <table width="450" border="0" cellpadding="0" cellspacing="0" class="TableSet1">
        <%' □車種 %>
        <tr>
        <td width="109">
        <icrop:CustomLabel ID="CustomLabel17" runat="server" TextWordNo="17" Width="105px" Height="24px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td colspan="5">
        <icrop:CustomLabel ID="seriesNameLabel" runat="server" Width="160px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("SERIESNM")) %>/>
        </td>
        </tr>
        <%' □グレード/スペック %>
        <tr>
        <td>
        <icrop:CustomLabel ID="CustomLabel18" runat="server" TextWordNo="18" Width="105px" Height="40px" UseEllipsis="False" CssClass="clip2" />
        </td>
        <td colspan="5">
        <icrop:CustomLabel ID="CustomLabel2" runat="server"  Width="160px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("MODELNM")) %>/>
        </td>
        </tr>
        <%' □ボディータイプ、排気量、駆動 %>
        <tr>
        <td>
        <icrop:CustomLabel ID="CustomLabel19" runat="server" TextWordNo="19" Width="105px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td width="75">
        <icrop:CustomLabel ID="bodyTypeLabel" runat="server" Width="75px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("BODYTYPE")) %> />
        </td>
        <td width="53">
        <icrop:CustomLabel ID="CustomLabel20" runat="server" TextWordNo="20" Width="48px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td width="80">
        <icrop:CustomLabel ID="displacementLabel" runat="server" Width="80px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("DISPLACEMENT")) %> />
        </td>
        <td width="53">
        <icrop:CustomLabel ID="CustomLabel21" runat="server" TextWordNo="21" Width="48px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td width="80">
        <icrop:CustomLabel ID="driveSystemLabel" runat="server" Width="80px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("DRIVESYSTEM")) %> />
        </td>
        </tr>
        <%' □ミッション、外装色、内装色 %>
        <tr>
        <td>
        <icrop:CustomLabel ID="CustomLabel22" runat="server" TextWordNo="22" Width="105px" Height="24px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td>
        <icrop:CustomLabel ID="transmissionLabel" runat="server" Width="75px" Height="24px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("TRANSMISSION")) %>/>
        </td>
        <td>
        <icrop:CustomLabel ID="CustomLabel23" runat="server" TextWordNo="23" Width="48px" Height="24px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td>
        <icrop:CustomLabel ID="extColorLabel" runat="server" Width="80px" Height="24px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("EXTCOLOR")) %>/>
        </td>
        <td>
        <icrop:CustomLabel ID="CustomLabel24" runat="server" TextWordNo="24" Width="48px" Height="24px" UseEllipsis="False" CssClass="clip" />
        </td>
        <td>
        <icrop:CustomLabel ID="intColorLabel" runat="server" Width="80px" Height="24px" UseEllipsis="true" Text=<%# HttpUtility.HtmlEncode(Eval("INTCOLOR")) %>/>
        </td>
        </tr>
        </table>
        </ItemTemplate>
        </asp:Repeater>
        <%' □車両価格 %>
        <div class="RedBar">
        <p class="LeftBox">
        <icrop:CustomLabel ID="CustomLabel26" runat="server" TextWordNo="26" Width="340px" UseEllipsis="False" CssClass="clip" />
        </p>
        <p class="RightBox">
        <icrop:CustomLabel ID="basePriceLabel" runat="server" Text="" />
        </p>
        <div class="clearboth">&nbsp;
        </div>
        </div>
        <%' □オプション %>
        <table id="tblOption" width="448" border="0" cellpadding="0" cellspacing="0" class="TableTextB1">
        <tr>
        <td>
        <div class="title1">
        <icrop:CustomLabel ID="CustomLabel27" runat="server" TextWordNo="27" Width="169px" UseEllipsis="False" CssClass="clip" />
        </div>
        </td>
        <td width="90">
        <div class="title2">
        <icrop:CustomLabel ID="CustomLabel28" runat="server" TextWordNo="28" Width="85px" UseEllipsis="False" CssClass="clip" />
        </div>
        </td>
        <td width="89">
        <div class="title3">
        <icrop:CustomLabel ID="CustomLabel29" runat="server" TextWordNo="29" Width="85px" UseEllipsis="False" CssClass="clip" />
        </div>
        </td>
        <td width="97">
        <div class="title4">
        <icrop:CustomLabel ID="CustomLabel30" runat="server" TextWordNo="30" Width="95px" UseEllipsis="False" CssClass="clip" />
        </div>
        </td>
        </tr>
        <%' □外装オプション %>
        <%' □内装オプション %>
        <%' □メーカーオプション（TCV） %>
        <asp:Repeater ID="mkrOptionRepeater" runat="server">
        <ItemTemplate>
        <tr>
        <td class="TableText1">
        <div>
        <icrop:CustomLabel runat="server" type="text" ID="mkrOptionNameLabelCustomLabel" Text='<%# HttpUtility.HtmlEncode(Eval("OPTIONNAME")) %>' Width="160px" UseEllipsis="False" CssClass="textsize14 SC3070205Ellipsis"/>
        </div>
        </td>
        <td class="TableText3">
        <div>
        <asp:TextBox ID="mkrOptionPriceText" runat="server" class="mkrOptionPrice TableTextArea2" Text='<%# HttpUtility.HtmlEncode(Eval("PRICE")) %>' type="text" style="width:85px; color:#666d74;" TabIndex="21" ReadOnly="True" />
        </div>
        </td>
        <td class="TableText4">
        </td>
        <td class="TableText2" style="padding-right:3px; position:relative; right:3px;">
        <icrop:CustomLabel ID="mkrOptionTotalValueLabelCustomLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("PRICE")) %>' Width="95px" UseEllipsis="true" />
        </td>
        <%' オプション区分 %>
        <td style="display:none"><%# HttpUtility.HtmlEncode(Eval("OPTIONPART"))%></td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="dlrOptionRepeater" runat="server">
        <ItemTemplate>
        <tr>
        <%' オプション名 %>
        <td class="TableText1">
        <div>
        <icrop:CustomLabel runat="server" type="text" ID="tcvDlrOptionNameCustomLabel" Text='<%# HttpUtility.HtmlEncode(Eval("OPTIONNAME")) %>' Width="160px" UseEllipsis="False" CssClass="textsize14 SC3070205Ellipsis"/>
        </div>
        </td>
        <%' 価格 %>
        <td class="TableText3">
        <div>
        <asp:TextBox ID="tcvDlrOptionPriceText" runat="server" class="dlrOptionPrice TableTextArea2" Text='<%# HttpUtility.HtmlEncode(Eval("PRICE")) %>' type="text" style="width:85px; color:#666d74;" TabIndex="21" ReadOnly="True" />
        </div>
        </td>
        <%' 取付費用 %>
        <td class="TableText3">
        <div>
        <asp:TextBox ID="tcvDlrOptionInstallCostText" runat="server" class="TableTextArea3" Text='<%# HttpUtility.HtmlEncode(Eval("INSTALLCOST")) %>' type="text" style="width:85px; color:#666d74;" TabIndex="21" ReadOnly="True" />
        </div>
        </td>
        <%' 合計費用 %>
        <td class="TableText2" style="padding-right:3px; position:relative; right:3px;">
        <icrop:CustomLabel ID="tcvDlrOptionTotalValueCustomLabel" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("PRICE") + Eval("INSTALLCOST")) %>' Width="95px" UseEllipsis="true" />
        </td>
        <%' オプション区分 %>
        <td style="display:none"><%#HttpUtility.HtmlEncode(Eval("OPTIONPART")) %></td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        </table>
        <%' □販売店オプション（i-CROP） %>
        <table id="tblDlrOption" width="448" border="0" cellpadding="0" cellspacing="0" class="TableTextB1 TableTextB2">
        <% For Each dr As Data.DataRow In dlrOptionDataTable%>
        <% dlrOptionCount = dlrOptionCount + 1%>
        <tr>
        <td class="TableText3">
        <div>
        <input class="TableTextArea1" name="optionNameText<%= dlrOptionCount%>" type="text" value="<%=HttpUtility.HtmlEncode(dr("OPTIONNAME")) %>" style = 'width:166px;color:#666d74; font-size:14px;' TabIndex="21" onchange="inputChangedClient();" />
        </div>
        </td>
        <td class="TableText3">
        <div>
        <input class="dlrOptionPrice TableTextArea2" name="optionPriceText<%= dlrOptionCount%>" value="<%=HttpUtility.HtmlEncode(dr("PRICE")) %>" type="text" style = 'width:85px;color:#666d74; ' TabIndex="21" readonly="true" />
        </div>
        </td>
        <td class="TableText3">
        <div>
        <input class="TableTextArea3" name="optionMoneyText<%= dlrOptionCount%>" value="<%=HttpUtility.HtmlEncode(dr("INSTALLCOST")) %>" type="text" style = 'width:85px;color:#666d74;' TabIndex="21" readonly="true" />
        </div>
        </td>
        <td class="optionAmountText">
        <label class = "TableOptionSum"/>
        </td>
        </tr>
        <% Next%>
        <% dlrOptionCount = dlrOptionCount + 1%>
        <tr>
        <td class="TableText3">
        <div>
        <input class="TableTextArea1" type="text" name="optionNameText<%= dlrOptionCount%>" style = 'width:166px;color:#666d74; font-size:14px;' TabIndex="22" onchange="inputChangedClient();" />
        </div>
        </td>
        <td class="TableText3">
        <div>
        <input class="dlrOptionPrice TableTextArea2" name="optionPriceText<%= dlrOptionCount%>"  type="text" style = 'width:85px;color:#666d74; ' TabIndex="22" readonly="true" />
        </div>
        </td>
        <td class="TableText3">
        <div>
        <input class="TableTextArea3" name="optionMoneyText<%= dlrOptionCount%>"  type="text" style = 'width:85px;color:#666d74;' TabIndex="22" readonly="true" />
        </div>
        </td>
        <td class="optionAmountText">
        <label class = "TableOptionSum" />
        </td>
        </tr>
        <%' □オプション合計額 %>
        <tr>
        <td colspan="3" align="left" class="TableText1b">
        <icrop:CustomLabel ID="CustomLabel31" runat="server" TextWordNo="31" UseEllipsis="False" Width="340px" CssClass="clip" />
        </td>
        <td class="TableText2b" style="width:90px;">
        <label class = "TableOptionSum" />
        </td>
        </tr>
        </table>
        <div class="clearboth">&nbsp;
        </div>
        </div>
        </div>
        </div>
        <div class="tcvNcvBoxRight" style="margin-top:50px;">
        <%' ■諸費用 %>
        <div class="tcvNcvBoxSet tcvNcvBoxSet03">
        <h4>
        <icrop:CustomLabel ID="CustomLabel32" runat="server" TextWordNo="32" UseEllipsis="False" Width="160px" CssClass="clip" />
        </h4>
        <icrop:SegmentedButton ID="chargeSegmentedButton" name="chargeSegBtn" runat="server" onClick="chargeChange();" class="SwitchButton1 clip" TabIndex="23" >
        </icrop:SegmentedButton>
        <br>
        <div class="tcvNcvBoxSetIn tcvNcvBoxSetIn03">
        <table width="450" border="0" cellpadding="0" cellspacing="0" id="tblCharge" >
        <tr>
        <td width="352">
        <div class="TableTitle1">
        <icrop:CustomLabel ID="CustomLabel33" runat="server" TextWordNo="33" UseEllipsis="False" Width="348px" CssClass="clip" />
        </div>
        </td>
        <td width="98">
        <div class="TableTitle2">
        <icrop:CustomLabel ID="CustomLabel34" runat="server" TextWordNo="34" UseEllipsis="False" Width="96px" CssClass="clip" />
        </div>
        </td>
        </tr>
        <%' □車両購入税 %>
        <tr>
        <td class="TableText1">
        <icrop:CustomLabel ID="CarBuyTaxLabelCustomLabel" runat="server" TextWordNo="64" UseEllipsis="False" Width="300px" CssClass="clip" />
        </td>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <td class="TableText2">
        <asp:TextBox ID="CarBuyTaxTextBox" runat="server" onchange="inputChangedClient();" Width="88" class="CarBuyTax TextBox2" type="text" TabIndex="23" Text="" ReadOnly="True" />
        </td>
        <% Else%>
        <td class="TableText2">
        <icrop:CustomLabel ID="CarBuyTaxCustomLabel" runat="server"  Width="96px" UseEllipsis="true"/>
        </td>
        <% End If%>
        </tr>
        <%' □登録費用 %>
        <tr>
        <td class="TableText1">
        <icrop:CustomLabel ID="regPriceLabelCustomLabel" runat="server" TextWordNo="65" UseEllipsis="False" Width="300px" CssClass="clip redText" />
        </td>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <td class="TableText2">
        <asp:TextBox ID="regPriceTextBox" runat="server" onchange="inputChangedClient();" Width="88" class="regCost TextBox2" type="text" TabIndex="23" Text="0.00" ReadOnly="True" />
        </td>
        <% Else%>
        <td class="TableText2" >
        <icrop:CustomLabel ID="regPriceLabel" runat="server"  Width="96px" UseEllipsis="true"/>
        </td>
        <% End If%>
        </tr>
        <%' □手入力諸費用 %>
        <asp:Repeater ID="chargeInfoDataTableRep" runat="server">
        <ItemTemplate>
        <tr>
        <td class="TableText1">
        <div>
        <input class="ChargeInfoTextArea1" name='chargeInfoText<%#HttpUtility.HtmlEncode(Eval("ITEMCODE")) %>' value='<%#HttpUtility.HtmlEncode(Eval("ITEMNAME")) %>' type="text" id='chargeInfoNameTextBox1' style = 'width:341px;color:#666d74; font-size:14px;' tabIndex="23" onchange="inputChangedClient();" />
        </div>
        </td>
        <td class="TableText2">
        <div>
        <input class="ChargeInfoTextArea2" name="chargeInfoPrice<%#HttpUtility.HtmlEncode(Eval("ITEMCODE")) %>"  value='<%#HttpUtility.HtmlEncode(Eval("PRICE")) %>' id="chargeInfoPriceTextBox1" type="text" style = 'width:90px;color:#666d74;' tabIndex="23" readonly="true" />
        </div>
        </td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        <%' □諸費用合計 %>
        <tr>
        <td align="left" class="TableText1b">
        <icrop:CustomLabel ID="chargeInfoTotalLabelCustomLabel" runat="server" TextWordNo="35" UseEllipsis="False" Width="348px" CssClass="clip" />
        </td>
        <td class="TableText2b">
        <icrop:CustomLabel ID="chargeInfoTotalCustomLabel" runat="server"  Width="96px" UseEllipsis="true"/>
        </td>
        </tr>
        </table>
        <div>
        </div>
        </div>
        </div>
        <%' ■保険 %>
        <div class="tcvNcvBoxSet tcvNcvBoxSet04">
        <h4>
        <icrop:CustomLabel ID="CustomLabel36" runat="server" TextWordNo="36" UseEllipsis="False" Width="160px" CssClass="clip" />
        </h4>
        <div class="bottomSwitchSet2">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <table id="TblJisyaTasya" border = "0">
        <tr>
        <td id="Jisya" width="22" onClick="onClickJisya();">
        <img runat="server" id="imgChkJisya" alt="" src="../Styles/Images/checkMark02.png" TabIndex="24"/>
        </td>
        <td onClick="onClickJisya();">
        <icrop:CustomLabel ID="CustomLabelJisya" runat="server" TextWordNo="37" CssClass="clip" style="width:32px; height:20px;" />
        </td>
        <td id="Tasya" width="22" onClick="onClickTasya();">
        <asp:Image runat="server" id="imgChkTasya" src="../Styles/Images/checkMark02.png" width="22"　alt="" TabIndex="25" />
        </td>
        <td onClick="onClickTasya();">
        <icrop:CustomLabel ID="CustomLabelTasya" runat="server" TextWordNo="38" CssClass="clip" style="width:32px; height:20px;" />
        </td>
        </tr>
        </table>
        <% Else%>
        <span>
        <icrop:CustomLabel ID="CustomLabelTasyaLock" runat="server" TextWordNo="38" />
        </span>
        <span>
        <asp:Image runat="server" id="imgChkTasyaLock" src="../Styles/Images/checkMark02.png" width="22"　alt="" />
        </span>
        <span>
        <icrop:CustomLabel ID="CustomLabelJisyaLock" runat="server" TextWordNo="37" />
        </span>
        <span>
        <img runat="server" id="imgChkJisyaLock" alt="" src="../Styles/Images/checkMark02.png"/>
        </span>
        <% End If%>
        <span>
        <asp:HiddenField ID="jisyaCheckMark" runat="server" />
        </span>
        <span>
        <asp:HiddenField ID="tasyaCheckMark" runat="server" />
        </span>
        </div>
        <div class="tcvNcvBoxSetIn">
        <ul class="Selection">
        <%' □保険会社 %>
        <li class="SelectionButton1">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div id="InsComdiv" class="InsComdiv">
        <icrop:CustomLabel ID="dispSelectedInsCom" runat="server" Width="65px" Height="25"  UseEllipsis="False" CssClass="clip" />
        </div>
        <% Else%>
        <p class="ListLockArea2">
        <icrop:CustomLabel ID="insuComLabel" runat="server" Width="73px" Height="25"  UseEllipsis="True" CssClass="clip" />
        </p>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel39" runat="server" TextWordNo="39" UseEllipsis="False" Width="60px" CssClass="clip" />&nbsp;
        </span>
        </li>
        <%' □種類 %>
        <li class="SelectionButton2">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div id="InsKinddiv" class="InsKinddiv">
        <icrop:CustomLabel ID="dispSelectedInsKind" runat="server" Width="65px" Height="25"  UseEllipsis="False" CssClass="clip" />
        </div>
        <% Else%>
        <span>&nbsp
        <icrop:CustomLabel ID="insuComKindLabel" runat="server" Width="73px" Height="25"  UseEllipsis="True" />
        </span>
        <% End If%>
        <%'保険会社・種類用 %>
        <asp:HiddenField ID="InsComInsuComCdHidden" runat="server" />
        <asp:HiddenField ID="InsComInsuKubunHidden" runat="server" />
        <asp:HiddenField ID="InsComInsuComNameHidden" runat="server" />
        <asp:HiddenField ID="InsKindInsuComCdHidden" runat="server" />
        <asp:HiddenField ID="InsKindInsuKindCdHidden" runat="server" />
        <asp:HiddenField ID="InsKindInsuKindNmHidden" runat="server" />
        <asp:HiddenField ID="SelectInsuComCdHidden" runat="server" />
        <asp:HiddenField ID="SelectInsuComNmHidden" runat="server" />
        <asp:HiddenField ID="FirstInsuComCdHidden" runat="server" />
        <asp:HiddenField ID="SelectInsuKindCdHidden" runat="server" />
        <asp:HiddenField ID="SelectInsuKindNmHidden" runat="server" />
        <asp:HiddenField ID="FirstInsuKindCdHidden" runat="server" />
        <span>
        <icrop:CustomLabel ID="CustomLabel40" runat="server" TextWordNo="40" UseEllipsis="False" Width="52px" CssClass="clip" />&nbsp;
        </span>
        </li>
        <%' □年額 %>
        <li class="SelectionButton3">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divPayMethod2">
        <asp:TextBox ID="insuranceAmountTextBox" runat="server" onchange="inputChangedClient();" class="TextBox3 insuAmount" type="text" TabIndex="28" ReadOnly="True" />
        </p>
        <span>
        <icrop:CustomLabel ID="CustomLabel41" runat="server" TextWordNo="41" UseEllipsis="False" Width="53px" CssClass="clip" />&nbsp;
        </span>
        <% Else%>
        <span>
        <icrop:CustomLabel ID="insuranceAmountLabel" runat="server" UseEllipsis="False" Width="77px" CssClass="clip LabelMoney" />
        </span>
        <span>
        <icrop:CustomLabel ID="CustomLabel5_Lock" runat="server" TextWordNo="41" UseEllipsis="False" Width="53px" CssClass="clip" />&nbsp;
        </span>
        <% End If%>
        </li>
        </ul>
        <div>
        </div>
        </div>
        </div>
        <%' ■お支払い方法 %>
        <div class="tcvNcvBoxSet tcvNcvBoxSet05">
        <h4>
        <icrop:CustomLabel ID="CustomLabel42" runat="server" TextWordNo="42" UseEllipsis="False" Width="160px" CssClass="clip" />
        </h4>
        <icrop:SegmentedButton ID="payMethodSegmentedButton" name="payMethodSegBtn" runat="server" onClick="payMethodChange();" class="SwitchButton1" TabIndex="29" >
        </icrop:SegmentedButton>
        <%' ■□現金 %>
        <div id="cash" class="tcvNcvBoxSetIn" >
        <ul class="Selection">
        <%' □頭金 %>
        <li class="SelectionButton1">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="ListLockArea2">
        <asp:TextBox ID="cashDepositTextBox" runat="server" class="cashDeposit TextBox3" type="text" TabIndex="30" ReadOnly="True" />
        </p>
        <% Else%>
        <span>
        <p class="divRight">
        <icrop:CustomLabel ID="cashDepositLabel" runat="server" Width="75px" CssClass="clip LabelMoney" />
        </p>
        </span>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel70" runat="server" TextWordNo="48" UseEllipsis="False" Width="57px" CssClass="clip redText" />&nbsp;
        </span>
        </li>
        </ul>
        <div>
        </div>
        </div>
        <%' ■□ローン %>
        <div id="loan" class="tcvNcvBoxSetIn" style="display: none">
        <ul class="Selection">
        <%' □融資会社 %>
        <li class="SelectionButton1">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <dd class="Arrow" id="UsersTrigger">
        <div id="loanFinanceComdiv" class="loanFinanceComdiv">
        <icrop:CustomLabel ID="dispSelectedFinanceCom" runat="server" Width="65px" Height="25"  UseEllipsis="False" CssClass="clip" />
        </div>
        </dd>
        <% Else%>
        <dd class="LockArrow" id="Dd1">
        <div id="LockloanFinanceComdiv" class="LockloanFinanceComdiv">
        <icrop:CustomLabel ID="loanFinanceComLabel" runat="server" runat="server" UseEllipsis="True"  Width="70px"/>
        </div>
        </dd>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel45" runat="server" TextWordNo="45" UseEllipsis="False" CssClass="clip redText" Width="60px" />&nbsp;
        </span>
        </li>
        <asp:HiddenField ID="SelectFinanceComHiddenField" runat="server" value="" />
        <%' □期間 %>
        <li class="SelectionButton2">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divPayMethod2">
        <icrop:NumericBox ID="loanPayPeriodNumericBox" runat=server MaxDigits="3" class="TextBox3 TextNumber" onchange="inputChangedClient();" TabIndex="32" CompletionLabelWordNo="72" CancelLabelWordNo="71" >
        </icrop:NumericBox>
        </p>
        <% Else%>
        <span>&nbsp
        <icrop:CustomLabel ID="loanPayPeriodLabel" runat="server" UseEllipsis="False" Width="75px" CssClass="clip LabelNumber" />
        </span>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel46" runat="server" TextWordNo="46" UseEllipsis="False" Width="57px" CssClass="clip redText" />&nbsp;
        </span>
        </li>
        <%' □月額 %>
        <li class="SelectionButton3">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divPayMethod2">
        <asp:TextBox ID="loanMonthlyPayTextBox" runat="server" class="TextBox3 loanMonthlyPay " type="text" TabIndex="33" ReadOnly="True" style="background-color:#FFF;" />
        </p>
        <span>
        <icrop:CustomLabel ID="CustomLabel47" runat="server" TextWordNo="47" UseEllipsis="False" Width="55px" CssClass="clip redText" />&nbsp;
        </span>
        <% Else%>
        <span>
        <icrop:CustomLabel ID="loanMonthlyPayLabel" runat="server" UseEllipsis="False" Width="77px" CssClass="clip LabelMoney" />
        </span>
        <span>
        <icrop:CustomLabel ID="CustomLabel47_Lock" runat="server" TextWordNo="47" UseEllipsis="False" Width="53px" CssClass="clip redText" />&nbsp;
        </span>
        <% End If%>
        </li>
        </ul>
        <ul class="Selection">
        <%' □頭金 %>
        <li class="SelectionButton1">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divRight">
        <asp:TextBox ID="loanDepositTextBox" runat="server" class="loanDeposit TextBox3" type="text" TabIndex="34" ReadOnly="True" style="background-color:#FFF;"/>
        </p>
        <% Else%>
        <p class="ListLockAreaLoan_MidRow">
        <icrop:CustomLabel ID="loanDepositLabel" runat="server" Width="75px" CssClass="clip LabelMoney" />
        </p>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel48" runat="server" TextWordNo="48" UseEllipsis="False" Width="57px" CssClass="clip redText" />&nbsp;
        </span>
        </li>
        <%' □ボーナス %>
        <li class="SelectionButton2">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divPayMethod2">
        <asp:TextBox ID="loanBonusPayTextBox" runat="server" class="loanBonus TextBox3" type="text" TabIndex="35" ReadOnly="True" style="background-color:#FFF; "/>
        </p>
        <% Else%>
        <span>&nbsp
        <icrop:CustomLabel ID="loanBonusPayLabel" runat="server" UseEllipsis="False" Width="75px" CssClass="clip LabelMoney" />
        </span>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel49" runat="server" TextWordNo="49" UseEllipsis="False" Width="57px" CssClass="clip" />&nbsp;
        </span>
        </li>
        <%' □初回支払（日） %>
        <li class="SelectionButton3">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divShokaiShiharaitxt">
        <icrop:NumericBox ID="loanDueDateNumericBox" runat=server MaxDigits="3" class="TextBox3 TextBox4 TextNumber" onchange="inputChangedClient();" TabIndex="36" CompletionLabelWordNo="72" CancelLabelWordNo="71" >
        </icrop:NumericBox>
        </p>
        <% Else%>
        <span>
        <icrop:CustomLabel ID="loanDueDateLabel" runat="server" UseEllipsis="False" Width="47px" CssClass="clip LabelNumber" />
        </span>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel50" runat="server" TextWordNo="50" UseEllipsis="False" Width="84px" CssClass="clip redText" />&nbsp;
        </span>
        </li>
        </ul>
        <ul class="Selection">
        <%' □利息 %>
        <li class="SelectionButton1">
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <p class="divRight">
        <asp:TextBox ID="loanInterestrateTextBox" runat="server" class="loanIntrate TextBox3" type="text" TabIndex="36" ReadOnly="True" style="background-color:#FFF;"/>
        </p>
        <% Else%>
        <p class="ListLockArea2">
        <icrop:CustomLabel ID="loanInterestrateLabel" runat="server" Width="75px" Height="15" CssClass="clip LabelMoney" />
        </p>
        <% End If%>
        <span>
        <icrop:CustomLabel ID="CustomLabel92" runat="server" TextWordNo="92" UseEllipsis="False" Width="57px" CssClass="clip" />&nbsp;
        </span>
        </li>
        <li class="SelectionButton2">&nbsp;
        </li>
        <li class="SelectionButton3">&nbsp;
        </li>
        </ul>
        </div>
        </div>
        <%' ■お支払い金額 %>
        <div class="tcvNcvBoxSet">
        <h4>
        <icrop:CustomLabel ID="CustomLabel51" runat="server" TextWordNo="51" UseEllipsis="False" Width="160px" CssClass="clip" />
        </h4>
        <div class="tcvNcvBoxSetIn tcvNcvBoxSetIn06">
        <%' □下取り額 %>
        <asp:UpdatePanel id="UpdateArea" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <%' □情報取得ボタン %>
        <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
        <div class="tradeInCarButtonDiv">
            <asp:Button id="tradeInCarButton"  runat="server" textwordno="75" class="tradeInCarButton" TabIndex="39" />
        </div>
        <% End If%>
        <div>
            <icrop:CustomLabel ID="CustomLabel53" runat="server" TextWordNo="53" UseEllipsis="False" Width="160px" CssClass="clip" />
        </div>
        <table id = "tblTradeInCar" width="450" border="0" cellpadding="0" cellspacing="0" class="tblMgn">
            <tr>
            <td width="352">
            <div class="TableTitle1">
                <icrop:CustomLabel ID="CustomLabel54" runat="server" TextWordNo="54" UseEllipsis="False" Width="335px" CssClass="clip" />
            </div>
            </td>
            <td width="98">
            <div class="TableTitle2">
                <icrop:CustomLabel ID="CustomLabel55" runat="server" TextWordNo="55" UseEllipsis="False" Width="96px" CssClass="clip" />
            </div>
            </td>
            </tr>
            <asp:Repeater ID="tradeInCarDataTableRep" runat="server">
                <ItemTemplate>
                <tr>
                <td class="TableText1">
                <div>
                <input class="TradeInCarTextArea1" name='tradeInCarText<%#HttpUtility.HtmlEncode(Eval("NO")) %>' value='<%#HttpUtility.HtmlEncode(Eval("VEHICLENAME")) %>' type="text" ID="TradeInCarNameTextBox1" style = 'width:341px;color:#666d74; font-size:14px;' TabIndex="40" onchange="inputChangedClient();" />
                </div>
                </td>
                <td class="TableText2">
                <div>
                    <icrop:CustomLabel ID="minusLabel1test" runat="server" TextWordNo="58" class= "TradeInCarLabel clip" UseEllipsis="False" Width="10px" CssClass="" style="font-size:24px; margin-top:-1px; margin-left:-4x; font-weight:normal;" />
                    <input class="TradeInCarTextArea2" name="tradeInCarPrice<%#HttpUtility.HtmlEncode(Eval("NO")) %>"  value='<%#HttpUtility.HtmlEncode(Eval("ASSESSEDPRICE")) %>' ID="TradeInCarPriceTextBox1" type="text" style = 'width:80px;color:#666d74;' TabIndex="40" readonly="true" />
                </div>
                </td>
                </tr>
                </ItemTemplate>
            </asp:Repeater>
            <td align="left" class="TableText1b">
            <icrop:CustomLabel ID="CustomLabel56" runat="server" TextWordNo="56" UseEllipsis="False" Width="332px" CssClass="clip" readonly="true" />
            </td>
            <td class="TableText2b">
                <label id ="TradeInCarTotalPriceTotalLabel" style = "display:inline-block;width:96px;" />
            </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        <%' □値引き額 %>
        <div id="divDiscountPriceArea" class="ListBoxSet ListBoxSet0" >
            <div class="ListBoxLeft">
                <input id="NebikiHideButton" type="button" TabIndex="41"/>
                <icrop:CustomLabel ID="CustomLabel57" runat="server" TextWordNo="57" UseEllipsis="False" Width="260px" CssClass="clip" />
            </div>
            <div class="divNebiki">
                <asp:TextBox ID="discountPriceTextBox" runat="server" onchange="inputChangedClient();" class="discountPrice ListBoxRight" type="text" TabIndex="43" ReadOnly="True"  />
                <div id="ListBoxRightNebiki" class="ListBoxRightNebiki">
                    <icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="58" UseEllipsis="False" Width="160px" CssClass="clip" style="font-size:24px; margin-top:-5px;" />
                    <icrop:CustomLabel ID="discountPriceLabel" runat="server" class="TableText2b" style="position:relative; top:-3px;"/>
                </div>
            </div>
            <div id="ListBoxRight03" class="ListBoxRight03">
                <icrop:CustomLabel ID="CustomLabel58" runat="server" TextWordNo="58" UseEllipsis="False" Width="160px" CssClass="clip" style="font-size:24px; margin-top:-5px;"/>
            </div>
            <div class="clearboth">
                &nbsp;
            </div>
        </div>
        <%' □納車予定日 %>
        <div class="ListBoxSet">
            <div class="ListBoxLeft">
                <icrop:CustomLabel ID="CustomLabel59" runat="server" TextWordNo="59" UseEllipsis="False" Width="300px" CssClass="clip" />
            </div>
            <% If Me.ReferenceModeHiddenField.Value.Equals(StrFalse) And  Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then%>
            <div class="ListBoxRight02">
                <icrop:DateTimeSelector ID="deliDateDateTimeSelector" runat="server" PlaceHolderWordNo="99" Format="Date" onchange="inputChangedClient();" ForeColor="#666D74" TabIndex="-1" />
            </div>
            <% Else%>
            <icrop:CustomLabel ID="deliDateLabel" runat="server" class="LabelRight"/>
            <% End If%>
            <div class="clearboth">
                &nbsp;
            </div>
        </div>
        <%' □支払い総額 %>
        <div id="divTotalPriceArea" class="RedBar">
            <p class="LeftBox">
            <icrop:CustomLabel ID="CustomLabel60" runat="server" TextWordNo="60" UseEllipsis="False" Width="300px" CssClass="clip" />
            </p>
            <p class="RightBox">
            <icrop:CustomLabel ID="PayTotalLabel" runat="server" style="position:absolute; margin-left:-8px; text-align:right; width:140px;"/>
            </p>
            <div class="clearboth">
                &nbsp;
            </div>
        </div>
        </div>
        </div>
        <%' ■メモ %>
        <div class="tcvNcvBoxSet">
            <h4>
            <icrop:CustomLabel ID="CustomLabel61" runat="server" TextWordNo="61" UseEllipsis="False" Width="160px" CssClass="clip" />
            </h4>
            <div class="tcvNcvBoxSetIn">
            <asp:TextBox ID="memoTextBox" runat="server" class="TextAreaSet" onchange="inputChangedClient();" Width="434" Height="54" MaximumSize="100%" TextMode="MultiLine" TabIndex="44">
            </asp:TextBox>
            </div>
        </div>
        </div>
        <asp:PlaceHolder runat="server" ID="PlaceHolder1" />
        <br/>
        <div class="clearboth">
        </div>
        <!-- 上書きポップオーバー -->
        <div id="divSavePop">
            <icrop:PopOver ID="popOver1" runat="server" TriggerClientID="carIcon1" HeaderTextWordNo="1" HeaderStyle="ClientId" HeaderClientId="popOver1Header" >
                <asp:LinkButton ID="saveLinkButton" runat="server" Width="80" Height="46" >
                <icrop:CustomLabel ID="saveCustomLabel" runat="server" TextWordNo="68" Height="20" >
                </icrop:CustomLabel>
                </asp:LinkButton>
            </icrop:PopOver>
        </div>
        <!-- 保険会社ポップオーバー -->
        <div id="InsComSelector" data-TriggerClientID="InsComdiv" style="display:none">
            <div class='icrop-PopOverForm-header' style="width:330px;">
            <h3>
            <icrop:CustomLabel ID="PopTitleInsCom" runat="server" TextWordNo="39" UseEllipsis="False" Width="130px" CssClass="clip" />
            </h3>
            </div>
            <div class="icrop-PopOverForm-content" style="width:220px;height:100px;overflow:hidden;">
            <div class="icrop-PopOverForm-sheet" style="width:0px;">
            <div class="icrop-PopOverForm-page dataBoxSC3070205" style="width:220px;height:100px;float:left;">
            </div>
            </div>
            </div>
        </div>
        <!-- 保険種類ポップオーバー -->
        <div id="InsKindSelector" data-TriggerClientID="InsKinddiv" style="display:none">
            <div class='icrop-PopOverForm-header' style="width:330px;">
            <h3>
            <icrop:CustomLabel ID="PopTitleInsKind" runat="server" TextWordNo="40" UseEllipsis="False" Width="130px" CssClass="clip" />
            </h3>
            </div>
            <div class="icrop-PopOverForm-content" style="width:220px;height:100px;overflow:hidden;">
            <div class="icrop-PopOverForm-sheet" style="width:0px;">
            <div class="icrop-PopOverForm-page dataBoxSC3070205" style="width:220px;height:100px;float:left;">
            </div>
            </div>
            </div>
        </div>
        <!-- 融資会社ポップオーバー -->
        <icrop:PopOver ID="loanFinanceComSelector" runat="server" TriggerClientID="loanFinanceComdiv" Width="200px" Height="200px" HeaderStyle="None">
            <div id="loanFinanceComWindow">
            <div id="loanFinanceComWindowBox">
            <div class="loanFinanceComHadder">
            <h3>
            <icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="45" UseEllipsis="False" width="130px" CssClass="clip"/>
            </h3>
            </div>
            <div class="loanFinanceComListArea">
            <div class="loanFinanceComListBox">
            <div class="loanFinanceComListItemBox">
            <div class="loanFinanceComListItem5">
            <ul class="nscListBoxSetIn">
            <li title="" id="loanFinanceComList" class="loanFinanceComlist ellipsis" value="">&nbsp
            <span value="">
            </span>
            </li>
            <asp:Repeater ID="loanFinanceComRepeater" runat="server" ClientIDMode="Predictable">
            <ItemTemplate>
            <li title="<%# Server.HTMLEncode(DataBinder.Eval(Container.DataItem, "FINANCECOMNAME"))%>" id="loanFinanceComList <%# Server.HTMLEncode(DataBinder.Eval(Container.DataItem, "FINANCECOMCODE"))%>" class="loanFinanceComlist ellipsis" value="<%# Server.HTMLEncode(DataBinder.Eval(Container.DataItem, "FINANCECOMCODE"))%>">
            <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "FINANCECOMNAME"))%>
            <span value="<%# Server.HTMLEncode(DataBinder.Eval(Container.DataItem, "FINANCECOMCODE"))%>">
            </span>
            </li>
            </ItemTemplate>
            </asp:Repeater>
            </ul>
            </div>
            </div>
            <!-- リストの最下部が切れる為、要素追加 -->
            <div style="height:15px;">
            </div>
            </div>
            </div>
            </div>
            </div>
        </icrop:PopOver>
        <%' ■HIDDEN項目 %>
        <asp:HiddenField ID="lngEstimateIdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strDlrcdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strStrCdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="lngFollowupBoxSeqNoHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strCstKindHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strCustomerClassHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strCRCustIdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="blnLockStatusHiddenField" runat="server" value="" />
        <asp:HiddenField ID="operationCodeHiddenField" runat="server" value="" />
        <asp:HiddenField ID="businessFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="readOnlyFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="ReferenceModeHiddenField" runat="server" value="FALSE" />
        <asp:HiddenField ID="basePriceHiddenField" runat="server" value="" />
        <asp:HiddenField ID="actionModeHiddenField" runat="server" value="" />
        <asp:HiddenField ID="contractFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="contractAfterFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="regPriceHiddenField" runat="server" value="" />
        <asp:HiddenField ID="memoMaxHiddenField" runat="server" value="" />
        <asp:HiddenField ID="seriesNameHiddenField" runat="server" value="" />
        <asp:HiddenField ID="modelNameHiddenField" runat="server" value="" />
        <asp:HiddenField ID="seriesCdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="modelCdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="suffixCdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="extColorCdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="modelNumberHiddenField" runat="server" value="" />
        <asp:HiddenField ID="extOptionFlgHiddenField" runat="server" value="0" />
        <asp:HiddenField ID="intOptionFlgHiddenField" runat="server" value="0" />
        <asp:HiddenField ID="extOptionPriceHiddenField" runat="server" value="0" />
        <asp:HiddenField ID="intOptionPriceHiddenField" runat="server" value="0" />
        <asp:HiddenField ID="mkrOptionCountHiddenField" runat="server" value="" />
        <asp:HiddenField ID="dlrOptionCountHiddenField" runat="server" value="" />
        <asp:HiddenField ID="tradeInCarCountHiddenField" runat="server" value="" />
        <asp:HiddenField ID="minusLabelHiddenField" runat="server" value="" />
        <asp:HiddenField ID="discountPriceFlgHiddenField" runat="server" value="False" />
        <asp:HiddenField ID="insuAmountValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="cashDepositValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanMonthlyValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanDepositValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanBonusValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="regCostValueHiddenField" runat="server" value="0" />
        <asp:HiddenField ID="discountPriceValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="deliDateAfterValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="initialFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="createDateHiddenField" runat="server" value="" />
        <asp:HiddenField ID="customerInfoFlgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="strApprovalModeHiddenField" runat="server" value="" />
        <asp:HiddenField ID="noticeReqIdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="cstEstNameHiddenField" runat="server" value="" />
        <asp:HiddenField ID="cstNameHiddenField" runat="server" value="" />
        <asp:HiddenField ID="selectFinanceComNmHiddenField" runat="server" value="" />
        <asp:HiddenField ID="tcvDlrOptionCountHiddenField" runat="server" value="" />
        <asp:HiddenField ID="chargeInfoCountHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanInterestrateValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="contractApprovalStatus" runat="server" value="" />
        <%' ■HIDDEN項目(クライアント側使用文言) %>
        <asp:HiddenField ID="shoyusyaNameMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shoyusyaZipcodeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shoyusyaAddressMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shoyusyaIdMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shiyosyaNameMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shiyosyaZipcodeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shiyosyaAddressMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shiyosyaIdMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="optionPriceMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="optionInstallFeeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="regFeeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="insuranceFeeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="cashDownMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanMonthlyPayMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanDownMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanBonusMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="discountMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="tradeInPriceMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="inputDataDeleteMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="customerDeleteMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="numericKeyPadCancelHiddenField" runat="server" value="" />
        <asp:HiddenField ID="numericKeyPadDoneHiddenField" runat="server" value="" />
        <asp:HiddenField ID="carBuyTaxFeeMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="chargeInfoPriceMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="loanInterestrateMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="payTotalMsgHiddenField" runat="server" value="" />
        <asp:HiddenField ID="carBuyTaxHiddenField" runat="server" value="" />
        <asp:HiddenField ID="estimateIdHiddenField" runat="server" value="" />
        <asp:HiddenField ID="selectedEstimateIndexHiddenField" runat="server" value="" />
        <asp:HiddenField ID="operationLockedHiddenField" runat="server" value="" />
        <asp:HiddenField ID="periodChangeValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="firstPayChangeValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="deliDateChangeValueHiddenField" runat="server" value="" />
        <asp:HiddenField ID="shoyusyaNameHiddenField" runat="server" value="" />
        <asp:HiddenField ID="DispModeHiddenFlg" runat="server" value="" />
        <asp:HiddenField ID="errorWord901" runat="server" value="" />
        <asp:HiddenField ID="errorWord902" runat="server" value="" />
        <asp:HiddenField ID="errorWord903" runat="server" value="" />
        <asp:HiddenField ID="errorWord938" runat="server" value="" />
        <asp:HiddenField ID="errorWord904" runat="server" value="" />
        <asp:HiddenField ID="errorWord905" runat="server" value="" />
        <asp:HiddenField ID="errorWord906" runat="server" value="" />
        <asp:HiddenField ID="errorWord907" runat="server" value="" />
        <asp:HiddenField ID="errorWord939" runat="server" value="" />
        <asp:HiddenField ID="errorWord908" runat="server" value="" />
        <asp:HiddenField ID="errorWord936" runat="server" value="" />
        <asp:HiddenField ID="errorWord943" runat="server" value="" />
        <asp:HiddenField ID="errorWord944" runat="server" value="" />
        <asp:HiddenField ID="errorWord945" runat="server" value="" />
        <asp:HiddenField ID="errorWord946" runat="server" value="" />
        <asp:HiddenField ID="errorWord947" runat="server" value="" />
        <asp:HiddenField ID="errorWord948" runat="server" value="" />
        <asp:HiddenField ID="errorWord949" runat="server" value="" />
        <asp:HiddenField ID="errorWord950" runat="server" value="" />
        <asp:HiddenField ID="errorWord951" runat="server" value="" />
        </div>
        </div>
        </div>
    </div>
    </div>
</div>
<div id="serverProcessOverlayBlack">
</div>
<div id="serverProcessIcon">
</div>
</asp:Content>
