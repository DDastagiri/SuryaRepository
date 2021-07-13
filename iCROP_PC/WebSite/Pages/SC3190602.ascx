<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3190602.ascx.vb" Inherits="Pages_SC3190602_Control" %>

<%' スタイルシート %>
<link rel="stylesheet" href="../Styles/SC3190602/themes/base/jquery.ui.all.css?20140916000001" type="text/css" />
<link rel="stylesheet" href="../Styles/SC3190602/SC3190602.css?20140925000001" type="text/css" />

<%' スクリプト %>
<script type="text/javascript" src="../Scripts/SC3190602/jquery.ui.datepicker.js?20140916000001"></script>
<script type="text/javascript" src="../Scripts/SC3190602/SC3190602.js?20150422000001"></script>

<%' ポップアップの背景色 %>
<div id="PopUp_Back"></div>

<%' 全体エリア %>
<div id="SC3190602_Panel">

    <%' ボディー（アップデートパネル） %>
    <asp:UpdatePanel ID="SC3190602_Content" runat="server" class='content' UpdateMode="Conditional">
        <ContentTemplate>

            <%' 読み込み中に操作不可とするため全体に設定する透明なオーバーレイ %>
            <div id="SC3190602_Overlay"></div>
            <%' 処理中のローディング %>
            <div id="SC3190602_ProcessingServer"></div>

            <%' 再描画用の隠しボタン %>
            <asp:Button ID="SC3190602_LoadSpinButton" runat="server" style="display:none;" />

            <%' 隠し項目 %>
            <%' B/O ID %>
            <asp:HiddenField ID="SC3190602_BoId" runat="server" />
            <%' P/O 番号 %>
            <asp:HiddenField ID="SC3190602_PoNumHD" runat="server" />
            <%' R/O 番号 %>
            <asp:HiddenField ID="SC3190602_RoNumHD" runat="server" />
            <%' お客様約束日 %>
            <asp:HiddenField ID="SC3190602_CstAppDateHD" runat="server" />
            <%' 作業文言 %>
            <asp:HiddenField ID="SC3190602_JobNameWordHD" runat="server" />
            <%' 必須入力エラー時文言 %>
            <asp:HiddenField ID="SC3190602_CompulsoryInputWordHD" runat="server" />
            <%' 登録ボタン押下時文言 %>
            <asp:HiddenField ID="SC3190602_RegisterWordHD" runat="server" />

            <div class="PopUp">

                <div class="PopUp_Head_Box">
                    <span><icrop:CustomLabel ID="SC3190602_Title" runat="server" Width="1300px" UseEllipsis="False" class="Ellipsis" /></span>
                    <div class="CloseBtn"></div>
                </div>
                <div class="PopUp_Contents_Box">
                    <div class="Upper_Line01 MGL06">
                        <div class="Line_Title"><icrop:CustomLabel ID="SC3190602_PoNumWord" runat="server" Width="195px" UseEllipsis="False" class="Ellipsis" /></div>
                        <div class="Line_InputBox">
                            <input type="text" class="BO_PopInput SC3190602Tub" id="SC3190602_PoNum" maxlength="20"/>
                        </div>
                        <div class="Line_Title MGL53"><icrop:CustomLabel ID="SC3190602_RoNumWord" runat="server" Width="195px" UseEllipsis="False" class="Ellipsis" /></div>
                        <div class="Line_InputBox">
                            <input type="text" class="BO_PopInput SC3190602Tub" id="SC3190602_RoNum" maxlength="20" />
                        </div>
                    </div>

                    <div id="OperationScroll">
                        <%' 作業情報 %>
                        <asp:Repeater ID="JobRepeater" runat="server">   
                            <ItemTemplate>
                                <div class="OperationArea">
                                    <div class="Upper_Line01 MGT10 MGL06">
                                        <div class="Line_Title JobLabel"><icrop:CustomLabel ID="SC3190602_JobNameWord" runat="server" Width="195px" UseEllipsis="False" class="Ellipsis" /></div>
                                        <div class="Line_InputBox">
                                            <input type="text" class="BO_PopInput SC3190602_JobName SC3190602Tub" ID="SC3190602_JobName1" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "JOB_NAME")) %>" maxlength="30" />
                                        </div>
                                        <div id="SC3190602_JobDelIcon" class="IconBox"><div class="IconMinus"></div></div>
                                        <div id="SC3190602_PartsAddIcon" class="IconBox"><div class="IconPlus"></div></div>
                                    </div>

                                    <%' 部品情報 %>
                                    <div class="Parts_TBL_Box">
                                        <table width="1358" class="Top_TBL" border="0" cellspacing="6" cellpadding="0">
                                            <tr>
                                                <th class="Parts_TBL_Head W576 TAL"><icrop:CustomLabel ID="SC3190602_PartsNameWord" runat="server" Width="545px" UseEllipsis="False" class="Ellipsis"/></th>
                                                <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_PartsCdWord" runat="server" Width="170px" UseEllipsis="False" class="Ellipsis" /></th>
                                                <th class="Parts_TBL_Head W100 F24"><icrop:CustomLabel ID="SC3190602_PartsAmountWord" runat="server" Width="95px" UseEllipsis="False" class="Ellipsis" /></th>
                                                <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_OrdDateWord" runat="server" Width="175px" UseEllipsis="False" class="Ellipsis" /></th>
                                                <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_ArrivalScheDateWord" runat="server" Width="175px" UseEllipsis="False" class="Ellipsis" /></th>
                                                <th class="Parts_TBL_Head W100"><icrop:CustomLabel ID="SC3190602_CheckWord" runat="server"  Width="95px" UseEllipsis="False" class="Ellipsis"/></th>
                                            </tr>
                                        </table>
                                        <div class="Bottom_TBL">
                                            <table class="partsTable" width="1358" border="0" cellspacing="6" cellpadding="0">
                                                <asp:Repeater ID="PartsRepeater" runat="server" datasource='<%# Container.DataItem.Row.GetChildRows("relationJob") %>'>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <th class="Parts_TBL_Contents W576 TAL">
                                                                <div id="SC3190602_PartsDelIcon" class="IconBox"><div class="IconMinus"></div></div>
                                                                <div class="TextBox">
                                                                    <input type="text" class="SC3190602_PartsName SC3190602Tub" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "PARTS_NAME")) %>" maxlength="30"  style="width: 98%; margin-top: 10px;" />
                                                                </div>
                                                            </th>
                                                            <th class="Parts_TBL_Contents W180"><input type="text"  class="SC3190602_PartsCd SC3190602Tub" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "PARTS_CD")) %>" maxlength="50" /></th>
                                                            <th class="Parts_TBL_Contents W100"><input type="number" class="SC3190602_PartsAmount SC3190602Tub" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "PARTS_AMOUNT")) %>" min="0" max="99" /></th>
                                                            <th class="Parts_TBL_Contents W180"><input type="text" class="SC3190602_OrdDate SC3190602Tub" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ODR_DATE", "{0:dd/MM/yyyy}")) %>" /></th>
                                                            <th class="Parts_TBL_Contents W180"><input type="text" class="SC3190602_ArrivalScheDate SC3190602Tub" value="<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ARRIVAL_SCHE_DATE", "{0:dd/MM/yyyy}")) %>" /></th>
                                                            <th class="Parts_TBL_Contents W100 SC3190602_Check"></th>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="Upper_Line01 MGT30 MGL06 MGT15">
                        <div class="Line_Title"><icrop:CustomLabel ID="SC3190602_VclPartakeFlgWord" runat="server" Width="195px" UseEllipsis="False" class="Ellipsis" /></div>
                        <div class="Line_InputBox">
                            <select id="SC3190602_VclPartakeFlg" class="BO_PopInput SC3190602Tub" style="line-height: 41px;" runat="server">
                                <option value=" " runat="server"></option>
                                <option value="0" runat="server"></option>
                                <option value="1" runat="server"></option>
                            </select>
                        </div>
                        <div class="Line_Title F24 MGL53"><icrop:CustomLabel ID="SC3190602_CstAppointmentDateWord" runat="server"  Width="195px" UseEllipsis="False" class="Ellipsis"/></div>
                        <div class="Line_InputBox">
                            <input type="text" ID="SC3190602_CstAppointmentDate" class="BO_PopInput SC3190602Tub" value="" />
                        </div>
                    </div>
                    <div id="RegisterButton" class="Button_OutBox MGT24">
                        <div class="Button_01 BG03">
                            <span><icrop:CustomLabel ID="SC3190602_Registration" runat="server" Width="195px" UseEllipsis="False" class="Ellipsis"/></span>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <%' ボディー（アップデートパネル）更新用 %>
    <asp:UpdatePanel ID="SC3190602_ContentUpDate" runat="server" class='content' UpdateMode="Conditional">
        <ContentTemplate>
            <%' 登録処理用の隠しボタン %>
            <asp:Button ID="SC3190602_RegisterButton" runat="server" style="display:none;" />
            <%' 入力内容 %>
            <asp:HiddenField ID="SC3190602_Input" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <%' 作業情報の動的追加用HTML %>
    <div id="OperationAreaHD" style="display: none;">
        <div class="OperationArea">
            <div class="Upper_Line01 MGT10 MGL06">
                <div class="Line_Title">
                    <icrop:CustomLabel ID="SC3190602_JobNameWord" runat="server"  Width="195px" UseEllipsis="False" class="Ellipsis"/>
                </div>
                <div class="Line_InputBox">
                    <input type="text" class="BO_PopInput SC3190602_JobName SC3190602Tub" id="SC3190602_JobName1" value="" maxlength="30">
                </div>
                <div id="SC3190602_JobDelIcon" class="IconBox"><div class="IconMinus"></div></div>
                <div id="SC3190602_PartsAddIcon" class="IconBox"><div class="IconPlus"></div></div>
            </div>
            <div class="Parts_TBL_Box">
                <table width="1358" class="Top_TBL" border="0" cellspacing="6" cellpadding="0">
                    <tbody>
                        <tr>
                            <th class="Parts_TBL_Head W576 TAL"><icrop:CustomLabel ID="SC3190602_PartsNameWordHD" runat="server" Width="545px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_PartsCdWordHD" runat="server"  Width="170px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th class="Parts_TBL_Head W100 F24"><icrop:CustomLabel ID="SC3190602_PartsAmountWordHD" runat="server" Width="95px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_OrdDateWordHD" runat="server" Width="175px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th class="Parts_TBL_Head W180"><icrop:CustomLabel ID="SC3190602_ArrivalScheDateWordHD" runat="server" Width="175px" UseEllipsis="False" class="Ellipsis"/></th>
                            <th class="Parts_TBL_Head W100"><icrop:CustomLabel ID="SC3190602_CheckWordHD" runat="server" Width="95px" UseEllipsis="False" class="Ellipsis"/></th>
                        </tr>
                    </tbody>
                </table>
                <div class="Bottom_TBL">
                    <table  class="partsTable" width="1358" border="0" cellspacing="6" cellpadding="0">
                        <tbody>
                            <tr>
                                <th class="Parts_TBL_Contents W576 TAL">
                                    <div id="SC3190602_PartsDelIcon" class="IconBox"><div class="IconMinus"></div></div>
                                    <div class="TextBox">
                                        <input type="text" class="SC3190602_PartsName SC3190602Tub" value="" maxlength="30"  style="width: 98%; margin-top: 10px;" />
                                    </div>
                                </th>
                                <th class="Parts_TBL_Contents W180"><input type="text" class="SC3190602_PartsCd SC3190602Tub" value="" maxlength="50"></th>
                                <th class="Parts_TBL_Contents W100"><input type="number" class="SC3190602_PartsAmount SC3190602Tub" value="" min="0" max="99"></th>
                                <th class="Parts_TBL_Contents W180"><input type="text" class="SC3190602_OrdDate SC3190602Tub" value=""></th>
                                <th class="Parts_TBL_Contents W180"><input type="text" class="SC3190602_ArrivalScheDate SC3190602Tub" value=""></th>
                                <th class="Parts_TBL_Contents W100 SC3190602_Check"></th>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
