<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3290102.ascx.vb" Inherits="Pages_SC3290102" %>
<%@ Register src="~/Pages/SC3290104.ascx" tagname="SC3290104" tagprefix="uc1" %>

<%' スタイルシート %>
<link type="text/css" href="../Styles/SC3290102/SC3290102.css?20140714000000" rel="stylesheet" />

<%' スクリプト %>
<script type="text/javascript" src="../Scripts/SC3290102/SC3290102.js?20140701400000"></script>

<%' リマインダー画面 %>
<div id="SC3290102_Panel" class="contentsFrame02">
    <%' 画面タイトル %>
    <h2 class="contentTitle">
        <icrop:CustomLabel ID="SC3290102_Label_Title" runat="server" UseEllipsis="False" width="175px" class="Ellipsis" />
    </h2>

    <div id="SC3290102_TBL_Box02">

        <table class="SC3290102_table02" border="0" cellspacing="0" cellpadding="0">
            <thead>
      			<tr id="SC3290102_table02HeadR">
                        <th class="TBL_HDST"><div class="Hidden W189"><icrop:CustomLabel ID="SC3290102_ExpirationTitle" runat="server" UseEllipsis="False" width="100px" /></div></th> 
                        <th class="WhiteHD"></th>
                        <th class="TBL_HDCT"><div class="Hidden W189"><icrop:CustomLabel ID="SC3290102_StaffNameTitle" runat="server" UseEllipsis="False" width="100px" /></div></th> 
                        <th class="WhiteHD"></th> 
                        <th class="TBL_HDED"><div class="Hidden W189"><icrop:CustomLabel ID="SC3290102_IrregularItemNameTitle" runat="server" UseEllipsis="False" width="300px" /></div></th> 
                </tr>
    		</thead>
        </table>

        <%' 読み込み中に操作不可とするため全体に設定する透明なオーバーレイ %>
        <div id="SC3290102_ProgressPanel"></div>
        <%' 処理中のローディング %>
        <div id="SC3290102_LoadingAnimation2" class="show2"></div>
        <%' フォロー設定項目無し メッセージ %>
        <div id="SC3290102_ItemNothing">
            <icrop:CustomLabel ID="SC3290102_ItemNotingLabel" runat="server" UseEllipsis="False" width="600px" class="Ellipsis" />  
        </div>

        <asp:UpdatePanel ID="SC3290102_FollwListUpdatePanel" runat="server" RenderMode="Block" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="SC3290102_table02" border="0" cellspacing="0" cellpadding="0">
                    <tbody id="SC3290102_table02Body" style="overflow:hidden; display:none;">
                        <tr id="SC3290102_PreButtonRow">
                            <td  colspan="5" class="TBL_DTCT WhiteBKDT" >
                                <%'読込み中の表示 %>
                                <div class="PreLoad LoadingButton">
                                    <icrop:CustomLabel ID="SC3290102_LoadingName1" runat="server" UseEllipsis="False" width="100px" class="Ellipsis  ItemLoad" />
                                    <div class="LoadingAnimation3 show2 ItemLoad"></div>
                                </div>
                                <%'前のN件ボタン %>
                                <div class="PreButton">
                                    <icrop:CustomLabel ID="SC3290102_PreButtonName" runat="server" UseEllipsis="False" width="150px" class="Ellipsis" />          
                                </div>
                            </td>
                        </tr>

                        <%' 再描画用の隠しボタン %>
                        <asp:Button ID="SC3290102_LoadSpinButton" runat="server" style="display:none;" />
                        <%' 前のN件の隠しボタン %>
                        <asp:Button ID="SC3290102_HidePreButton" runat="server" style="display:none;" />
                        <%' 後のN件の隠しボタン %>
                        <asp:Button ID="SC3290102_HideNextButton" runat="server" style="display:none;" />
                        
                        <%' 隠し項目 %>
                        <%' 全体件数 %>
                        <asp:HiddenField ID="SC3290102_ItemsField" runat="server"/>
                        <%' 表示最大件数 %>
                        <asp:HiddenField ID="SC3290102_MaxItemsField" runat="server"/>
                        <%' 表示開始行番号 %>
                        <asp:HiddenField ID="SC3290102_GetBeginLineField" runat="server"  />
                        <%' 表示終了行番号 %>
                        <asp:HiddenField ID="SC3290102_GetEndLineField" runat="server" />

                        <asp:Repeater ID="SC3290102_FollwListRepeater" runat="server" >
                            <ItemTemplate>
      					        <tr id="SC3290102_FollwPopup" class="SC3290102_FollwPopup">
                                    <asp:HiddenField ID="IrregFllwId" runat="server"/>
                                    <td class="TBL_DTST GrayBKDT"><icrop:CustomLabel ID="SC3290102_ExpirationLiteral" runat="server" UseEllipsis="False" width="110px" class="Ellipsis" /></td>
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTCT GrayBKDT"><icrop:CustomLabel ID="SC3290102_StaffNameLiteral" runat="server" UseEllipsis="False" width="100px" class="Ellipsis" /></td> 
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTED GrayBKDT"><icrop:CustomLabel ID="SC3290102_IrregularItemNameLiteral" runat="server" UseEllipsis="False" width="360px" class="Ellipsis" /></td> 
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="SC3290102_FollwPopup" class="SC3290102_FollwPopup">
                                    <asp:HiddenField ID="IrregFllwId" runat="server"/>
                                    <td class="TBL_DTCT WhiteBKDT"><icrop:CustomLabel ID="SC3290102_ExpirationLiteral" runat="server" UseEllipsis="False" width="110px" class="Ellipsis" /></td>
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTCT WhiteBKDT"><icrop:CustomLabel ID="SC3290102_StaffNameLiteral" runat="server" UseEllipsis="False" width="100px" class="Ellipsis" /></td> 
                                    <td class="WhiteDT"></td>
                                    <td class="TBL_DTED WhiteBKDT"><icrop:CustomLabel ID="SC3290102_IrregularItemNameLiteral" runat="server" UseEllipsis="False" width="360px" class="Ellipsis" /></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>


                        <tr id="SC3290102_NextButtonRow">
                            <td colspan="5" class="TBL_DTCT WhiteBKDT" >
                                <%'読込み中の表示 %>  
                                <div class="NextLoad LoadingButton">
                                    <icrop:CustomLabel ID="SC3290102_LoadingName2" runat="server" UseEllipsis="False" width="100px" class="Ellipsis ItemLoad" /> 
                                    <div class="LoadingAnimation3 show2 ItemLoad"></div>
                                </div>
                                <%'次のN件ボタン %> 
                                <div class="NextButton">
                                    <icrop:CustomLabel ID="SC3290102_NextButtonName" runat="server" UseEllipsis="False" width="150px" class="Ellipsis" />      
                                </div>
                            </td>
                        </tr>
                    </tbody>
 		        </table>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</div>

<%-- フォロー設定画面のユーザコントロール --%>
<uc1:SC3290104 ID="SC3290104" runat="server"  />
