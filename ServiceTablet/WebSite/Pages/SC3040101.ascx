<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3040101.ascx.vb" Inherits="Pages_SC3040101"  EnableViewState="false" %>

    <link rel="stylesheet" href="../Styles/SC3040101/SC3040101.css?20120106171000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3040101/SC3040101.js?20120106231600"></script>

            <div id="tcvNsc31Main">
                    <div class="popWind">
                        <div class="PopUpBtn01">
                        <div><asp:Button runat="server" ID="cancelButton" CssClass="buttonC stringCut" 
                                     OnClientClick ="return cancel();" TabIndex="4"/></div>
                        <div class="title stringCut"><icrop:CustomLabel ID ="titleCustomLabel" runat ="server"/></div>
                        <div>
                             <div id="divActive" style="display:none;">
                                <asp:Button runat="server" ID="postButton" CssClass="buttonD stringCut" 
                                ValidationGroup ="check" TabIndex="5" OnClientClick="return check();"/>
                             </div>
                             <div id="divDisable" class="buttonE stringCut"><icrop:CustomLabel runat ="server" ID ="postCustomLabel" /></div>
                         </div>
                         </div>

                         <div class="dataWind1">
                            <div class="TextBox">
                                <div class="TextBoxIn">
                                    <ul>
                                    <%'メッセージタイトル %>
                                        <li class="ListTitle">
                                                <icrop:CustomTextBox ID ="messageCustomTitleTextBox" runat ="server" CssClass="ListIn" 
                                                    MaxLength="25" onkeyup="checkInput();" onchange="checkInput();" TabIndex="1"/>                                             
                                        </li>
                                    <%'メッセージ %>
                                        <li class="ListMessage">
                                                <asp:TextBox ID ="messagesCustomTextBox" runat ="server" 
                                                    CssClass="ListIn" MaxLength="128" Rows="5" TextMode="MultiLine" 
                                                    onkeyup="checkInput();" onchange="checkInput();" TabIndex="2"/>                                              
                                        </li>
                                    <%'表示期限 %>
                                        <li class="ListDate">
                                            <div class="ListIn">
                                                <div class="DateTitle stringCut"><icrop:CustomLabel ID ="displayPeriodCustomLabel" runat ="server"/></div>
                                                <icrop:DateTimeSelector ID="displayPeriodCustomDateTimeSelector" 
                                                    CssClass="DateDetail stringCut" runat="server" Format="Date" 
                                                    onblur="checkInput();" onkeyup="checkInput();" onchange="checkInput();" TabIndex="3"/>
                                            </div>                                           
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="baseWind2">
                            <div class="box1">&nbsp;</div>
                            <div class="fuki1">&nbsp;</div>
                        </div>
                        <div class="baseWind1">
                            <div class="boxBoder">
                                <div class="fukiBoder">
                                    <div class="fuki">&nbsp;</div>
                                </div>
                                <div class="box">&nbsp;</div>
                            </div>
                        </div>
                    </div>


         <%'サーバー時間 %>
                <asp:HiddenField ID="displayPeriodHidden" runat="server" />
                <asp:HiddenField ID="displayPerioderrHidden" runat="server" />
         <%'エラーメッセージ %>
                <asp:HiddenField ID="errMsg1Hidden" runat="server" />
                <asp:HiddenField ID="errMsg2Hidden" runat="server" />
                <asp:HiddenField ID="errMsg3Hidden" runat="server" />
         <%'処理中フラグ %>>
                <asp:HiddenField ID="serverProcessFlgHidden" runat="server" />
         <%'必須入力チェック %>
                <asp:RequiredFieldValidator ID="TitleRequiredFieldValidator" 
                    runat="server" ControlToValidate="messageCustomTitleTextBox" Display="Dynamic" 
                    SetFocusOnError="True" ValidationGroup ="check" EnableClientScript="False"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="MessagesRequiredFieldValidator" 
                    runat="server" ControlToValidate="messagesCustomTextBox" Display="Dynamic" 
                    SetFocusOnError="True" ValidationGroup ="check" EnableClientScript="False"></asp:RequiredFieldValidator>
              </div>