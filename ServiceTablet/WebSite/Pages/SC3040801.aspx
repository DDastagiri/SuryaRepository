<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3040801.aspx.vb" Inherits="Pages_SC3040801" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--スクリプト(画面固有)--%>
    <link rel="Stylesheet" href="../styles/SC3040801/SC3040801.css?201211150000003" type="text/css" media="all" />
    <script type="text/javascript" src="../Scripts/SC3040801/SC3040801.js?20140604000000"></script>
</asp:Content>

<asp:Content id="Content2" ContentPlaceHolderID="content" runat="server">
<asp:ScriptManager ID="AjaxListManager" runat="server" EnablePageMethods="True" ></asp:ScriptManager>
    <div class="DataBox">
        <%--DisabledDiv--%>
        <div class="DisabledDiv" id="DisabledDiv" style="display:none;" ></div>
        <%--ロード中の画面--%>
        <asp:Panel ID="LoadPanel" runat="server" >
            <div class="LoadScreen">
                <div class="LoadImage"></div>
            </div>
        </asp:Panel>            
        <%--通知情報の無いときの画面--%>
        <asp:Panel id="NoDataPanel" runat="server" style="display:none;">
            <div class="Panel_NotVisitorList" id="Panel_NotVisitorList">
                <div class="Panelnot">
                    <div class="NoDataImage"></div>
		            <icrop:CustomLabel id="NoDataText" runat="server" TextWordNo="901" 
                                       UseEllipsis="False" ></icrop:CustomLabel>
                </div>
            </div>
         </asp:Panel>    
        <%--一覧が存在する場合--%>
        <asp:Panel ID="DataPanel" runat="server">
            <div class='Datas'>                    
                <asp:UpdatePanel ID="AjaxListPanel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater ID="RepeaterNoticeInfo" runat="server" >
                            <ItemTemplate>
                                <li id='DataList' runat="server" name='<%# Server.HTMLEncode("No" & DataBinder.Eval(Container.DataItem, "LISTID"))%>' >
                                    <%--写真--%>
                                    <div class='ItemImage'>
                                        <img src='<%# Me.ResolveClientUrl(DataBinder.Eval(Container.DataItem, "ORG_IMGFILE"))%>' width='58px' height='58px' alt='NOTFOUND' />
                                    </div>
                                    <%--メッセージ--%>
                                    <div class='ItemMsg' style='<%#If(StaffContext.Current.OpeCD = Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation.CT OrElse StaffContext.Current.OpeCD = Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Operation.FM, "width:290px;", "width:310px;")%>' >
                                        <div class='MessageLabl'><%# DataBinder.Eval(Container.DataItem, "MESSAGE")%></div>
                                    </div>
                                    <%--アイコンとタイムメッセージ--%>
                                    <div class='IconMessage' >
                                        <div id='Icons' runat="server"></div>
                                        <div class='ItemTimeMsg'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TIMEMESSAGE"))%></div>
                                    </div>
                                    <%-- 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start--%>
                                    <%-- チェックボックス押下時に対応ステータスを更新する--%>
                                    <div class='SupportStatus' >
                                        <div id="SupportStatusCheckBox" class="SupportStatusCheckBox_Gray" runat="server" visible="false" onclick='<%# Server.HtmlEncode("SupportStatusCheckBoxClick(" & DataBinder.Eval(Container.DataItem, "NOTICEID")  & "," &  DataBinder.Eval(Container.DataItem, "LISTID") & ")")%>'></div>
                                    </div>
                                    <%-- 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End--%>
                                    <%--キャンセルボタン--%>
                                    <div class='CancelButton' id='CancelButton' runat="server" onclick='<%# Server.HtmlEncode("CancelBtnClick(" & DataBinder.Eval(Container.DataItem, "NOTICEREQID") & ")")%>' >
                                        <icrop:CustomLabel runat="server" TextWordNo="8" UseEllipsis="False" id='CancelText'></icrop:CustomLabel>
                                    </div>
                                    <%--Listの隠しフィールド--%>
                                    <input type='hidden' id='<%# Server.HtmlEncode("SessionValue" & DataBinder.Eval(Container.DataItem, "LISTID"))%>' value='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SESSIONVALUE"))%>' />
                                    <asp:HiddenField  id="SupportStatusList" runat="server" value='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SUPPORTSTATUS"))%>'/>
                                 </li>
                             </ItemTemplate>
                        </asp:Repeater>
                        <%-- 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start--%>
                        <%-- チェックボックス押下時に対応ステータスを更新する--%>
                        <asp:Button ID="DetailCheckBoxButton" runat="server" style="display:none" />
                        <%-- 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End--%>
                        <%--次の6件ボタン--%>                      
                        <div id="NextButton" class="NextButton" runat="server" onclick="NextBtnClick()" visible="False" >                            
                            <icrop:CustomLabel id="NextText" runat="server" TextWordNo="9" UseEllipsis="False" ></icrop:CustomLabel>                   
                        </div>
                        <%--読込み中の表示--%>
                        <div class="NextLoad" style="display:none;" >
                            <div class= 'CenterDiv'>
                                <div class="NextLoadImage"></div>
                                <icrop:CustomLabel id="LoadingText" runat="server" TextWordNo="10" UseEllipsis="False" ></icrop:CustomLabel>
                            </div>
                        </div>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="HideNextButton" EventName="Click" />
                        </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
    </div>
    <asp:Button ID="HideCancelButton" runat="server" Text="" style="display:none;" />
    <asp:Button ID="LinkButton" runat="server" Text="" style="display:none;" />
    <asp:Button ID="HideNextButton" runat="server" Text="" style="display:none;" />
    <asp:Button ID="LoadButton" runat="server" Text="" style="display:none;" />
    <asp:HiddenField ID="CancelField" runat="server" />
    <asp:HiddenField ID="PageIdField" runat="server" />
    <asp:HiddenField ID="LinkIdField" runat="server" />
    <asp:HiddenField ID="SupportStatusNoticeId" runat="server" />
    <asp:HiddenField ID="SupportStatus" runat="server" />
    <asp:HiddenField ID="ListIndex" runat="server" />
    <asp:HiddenField ID="LinkValueField" runat="server" />
</asp:Content>