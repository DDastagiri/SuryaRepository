<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3190601.aspx.vb" Inherits="Pages_SC3190601" %>

<%-- B/O 部品入力 --%>
<%@ Register src="~/Pages/SC3190602.ascx" tagname="SC3190602" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190601.aspx
'─────────────────────────────────────
'機能： B/O 管理ボード
'補足： 
'作成： 2013/08/26 TMEJ M.Asano
'更新： 
'─────────────────────────────────────
-->

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

         <%'スタイルシート(画面固有) %>
        <link rel="stylesheet" href="../Styles/SC3190601/SC3190601.css?20130826000000" type="text/css" media="screen,print" />
        
        <%'スクリプト %>
        <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery-ui-1.8.16.custom.min.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.ui.ipad.altfix.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.doubletap.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.flickable.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.json-2.3.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.popover.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.fingerscroll.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/icropScript.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CheckButton.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CustomButton.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CustomLabel.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CustomTextBox.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.DateTimeSelector.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.PopOverForm.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.SegmentedButton.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.SwitchButton.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.CustomRepeater.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/jquery.NumericKeypad.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/icropBase.js?20130826000000"></script>
        <script type="text/javascript" src="../Scripts/SC3190601/SC3190601.js?20130826000003"></script>

    </head>
    <body>
        <div id="MainFrame">
            <%--処理中のローディング--%>
            <div id="SC3190601_LoadingScreen" class="registOverlay">
                <div class="registWrap">
                    <div class="processingServer"></div>
                </div>
            </div>

            <%-- 部品入力画面表示時のオーバーレイ--%>
            <div id="SC3190601_Overlay" class="registOverlay"></div>

            <form id="this_form" runat="server">
            
                <%' 非同期読み込みのためのScriptManagerタグ %>
                <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>

                <%' 自動ページング時間① %>
                <asp:HiddenField ID="AutoPagingTimeFirstField" runat="server"/>
                <%' 自動ページング時間② %>
                <asp:HiddenField ID="AutoPagingTimeSecondField" runat="server"/>
                <%' 直近到着予定部品判定日数 %>
                <asp:HiddenField ID="JudgementDaysField" runat="server"/>
                <%' ページ遷移用ボタン %>
                <asp:Button ID="ScreenTransitionButton" runat="server" style="display:none;"></asp:Button>

                <div class="TitleSet">
                    <div class="Title01">
                        <p>
                            <span>
                                <icrop:CustomLabel ID="SC3190601_Label_Title" runat="server" width="790px" UseEllipsis="False"  class="Ellipsis" />
                            </span>
                        </p>
                    </div>
                    <div class="Title02">
                        <p>
                            <icrop:CustomLabel ID="SC3190601_Label_CurStatus" runat="server"  width="260px" UseEllipsis="False" class="Ellipsis" style="font-size: 36px;" />
                            <icrop:CustomLabel ID="SC3190601_Label_Colon" runat="server"  width="15px" UseEllipsis="False" class="Ellipsis" />
                            <span>
                                <icrop:CustomLabel ID="SC3190601_Label_POTotal" runat="server"  width="55px" UseEllipsis="False" class="Ellipsis"/> 
                                <icrop:CustomLabel ID="SC3190601_Label_POTotal_Val" runat="server"  width="35px" UseEllipsis="False" class="Ellipsis" /> 
                                <icrop:CustomLabel ID="SC3190601_Label_PODelay" runat="server"  width="55px" UseEllipsis="False" class="FontRed Ellipsis" />
                                <icrop:CustomLabel ID="SC3190601_Label_PSTotal" runat="server"  width="90px" UseEllipsis="False" class="Ellipsis" /> 
                                <icrop:CustomLabel ID="SC3190601_Label_PSTotal_Val" runat="server"  width="35px" UseEllipsis="False" class="Ellipsis" />
                                <icrop:CustomLabel ID="SC3190601_Label_PSDelay" runat="server"  width="55px" UseEllipsis="False" class="FontRed Ellipsis" />
                            </span>
                        </p>
                    </div>
                    <div class="PlusBtn"></div>
                    <div class="PageIconbox">
                        <div class="PageIcon Icon01" PageIndex="1"></div>
                        <div class="PageIcon Icon02" PageIndex="2"></div>
                        <div class="PageIcon Icon03" PageIndex="3"></div>
                        <div class="PageIcon Icon04" PageIndex="4"></div>
                        <div class="PChangeRight"><div class="NextPageIcon Active"></div></div>
                    </div>
                </div>
                <div class="ListSet">
                    <table border="0" cellspacing="0" cellpadding="0" class="ListTitle">
                        <tr>
                            <th><icrop:CustomLabel ID="SC3190601_Label_No" runat="server"  width="101px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_PoNo" runat="server"  width="175px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_RoNo" runat="server"  width="177px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Operation" runat="server"  width="255px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Parts" runat="server"  width="404px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Qty" runat="server"  width="95px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_OdrDate" runat="server"  width="175px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Eta" runat="server"  width="173px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Vcl" runat="server"  width="96px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th><icrop:CustomLabel ID="SC3190601_Label_Apt" runat="server"  width="182px" UseEllipsis="False" class="Ellipsis"/></th>
                        </tr>
                    </table>
                    
                    <asp:UpdatePanel ID="SC3290102_FollwListUpdatePanel" runat="server" RenderMode="Block" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%' ページング用ボタン %>
                            <asp:Button ID="PagingButton" runat="server" style="display:none;"></asp:Button>
                            <%' 最大ページ数 %>
                            <asp:HiddenField ID="MaxPageCount" runat="server"/>
                            <%' 現在ページ数 %>
                            <asp:HiddenField ID="NowPageCount" runat="server"/>
                            <%' P/O 全数 %>
                            <asp:HiddenField ID="POTotalValField" runat="server"/>
                            <%' P/O 遅れ数 %>
                            <asp:HiddenField ID="PODelayField" runat="server"/>
                            <%' Parts 全数 %>
                            <asp:HiddenField ID="PSTotalValField" runat="server"/>
                            <%' Parts 遅れ数 %>
                            <asp:HiddenField ID="PSDelayField" runat="server"/>
                            <%' リスト行押下用のDiv %>
                            <div class="PartsListRowDiv" RowIndex="0"></div>
                            <div class="PartsListRowDiv" RowIndex="1"></div>
                            <div class="PartsListRowDiv" RowIndex="2"></div>
                            <div class="PartsListRowDiv" RowIndex="3"></div>
                            <div class="PartsListRowDiv" RowIndex="4"></div>
                            <div class="PartsListRowDiv" RowIndex="5"></div>
                            <div class="PartsListRowDiv" RowIndex="6"></div>
                            <div class="PartsListRowDiv" RowIndex="7"></div>
                            <div class="PartsListRowDiv" RowIndex="8"></div>
                            <%' リストエリア %>
                            <table id="SC3190601_PartsList" border="0" cellspacing="10" cellpadding="0" class="ListBox">
                                <asp:Repeater ID="SC3190601_PartsInfoListRepeater" runat="server" >
                                    <ItemTemplate>
                                        <tr id="SC3190601_PartsListRow" runat="server">
                                            <td id="SC3190601_NoRow" runat="server" valign="middle"><icrop:CustomLabel ID="SC3190601_PartsList_No" runat="server" width="88px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td id="SC3190601_PoNoRow" runat="server" valign="middle"><icrop:CustomLabel ID="SC3190601_PartsList_PoNo" runat="server" width="170px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td id="SC3190601_RoNoRow" runat="server" valign="middle"><icrop:CustomLabel ID="SC3190601_PartsList_RoNo" runat="server"  width="170px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td id="SC3190601_OperationRow" runat="server"><icrop:CustomLabel ID="SC3190601_PartsList_Operation" runat="server" width="250px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td>
                                                <div class="TopBox"><icrop:CustomLabel ID="SC3190601_PartsList_PartsName" runat="server"  width="385px" UseEllipsis="False" class="Ellipsis"/></div>
                                                <div class="BottomBox"><icrop:CustomLabel ID="SC3190601_PartsList_PartsCode" runat="server"  width="385px" UseEllipsis="False" class="Ellipsis"/></div>
                                            </td>
                                            <td><icrop:CustomLabel ID="SC3190601_PartsList_Qty" runat="server"  width="88px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td><icrop:CustomLabel ID="SC3190601_PartsList_OdrDate" runat="server"  width="170px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td><icrop:CustomLabel ID="SC3190601_PartsList_Eta" runat="server"  width="170px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td><icrop:CustomLabel ID="SC3190601_PartsList_Vcl" runat="server"  width="88px" UseEllipsis="False" class="Ellipsis"/></td>
                                            <td><asp:HiddenField ID="BoIdField" runat="server"/><icrop:CustomLabel ID="SC3190601_PartsList_Apt" runat="server"  width="170px" UseEllipsis="False" class="Ellipsis"/></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <%-- B/O入力画面のユーザコントロール --%>
                <uc1:SC3190602 ID="SC3190602" runat="server"  />

            </form>
        </div>
    </body>
</html>
