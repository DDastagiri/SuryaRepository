<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3040801.aspx.vb" Inherits="Pages_SC3040801" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--スクリプト(画面固有)--%>
    <link rel="Stylesheet" href="../styles/SC3040801/SC3040801.css" type="text/css" media="all" />
    <script type="text/javascript" src="../Scripts/SC3040801/SC3040801.js"></script>
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
                                    <div class='ItemMsg' >
                                        <div class='MessageLabl'><%# DataBinder.Eval(Container.DataItem, "MESSAGE")%></div>
                                    </div>
                                    <%--アイコンとタイムメッセージ--%>
                                    <div class='IconMessage' >
                                        <div id='Icons' runat="server"></div>
                                        <div class='ItemTimeMsg'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TIMEMESSAGE"))%></div>
                                    </div>
                                    <%--キャンセルボタン--%>
                                    <div class='CancelButton' id='CancelButton' runat="server" onclick='<%# Server.HtmlEncode("CancelBtnClick(" & DataBinder.Eval(Container.DataItem, "NOTICEREQID") & ")")%>' >
                                        <icrop:CustomLabel runat="server" TextWordNo="8" UseEllipsis="False" id='CancelText'></icrop:CustomLabel>
                                    </div>
                                    <%--Listの隠しフィールド--%>
                                    <input type='hidden' id='<%# Server.HtmlEncode("SessionValue" & DataBinder.Eval(Container.DataItem, "LISTID"))%>' value='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "SESSIONVALUE"))%>' />
                                 </li>
                             </ItemTemplate>
                        </asp:Repeater>
                        <%--次の6件ボタン--%>                      
                        <div class="NextButton" runat="server" onclick="NextBtnClick()" style="display:none;" >                            
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
    <asp:HiddenField ID="LinkValueField" runat="server" />
</asp:Content>