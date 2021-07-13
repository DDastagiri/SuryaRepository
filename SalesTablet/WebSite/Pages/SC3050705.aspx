<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3050705.aspx.vb" Inherits="Pages_SC3050705" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3050705/SC3050705.css?20130205000000" />
    <script type="text/javascript" src="../Scripts/SC3050705/SC3050705.js?20130205140000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

    <%'AJAX用 %>
    <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>

    <!-- ここからメインブロック -->
    <div id="mainblock">
		<div class="mainblockWrap">
			<div id="mainblockContent">
				<div class="mainblockContentArea">
				    <h2><icrop:CustomLabel ID="carName" runat="server" UseEllipsis="false" width="260px" CssClass="ellipsis" /></h2>
                    <div class="mainblockContentAreaWrap">
                        <div id="boxscroll" class="settingArea">
                            <div class="mainblockContents">
                                <table id="boxscrollTable" border="0" cellspacing="0" cellpadding="0" class="ListSet">
				                    <tr>
				                        <th><icrop:CustomLabel ID="OptionNameLabel" runat="server" TextWordNo="3" UseEllipsis="false" width="120px" CssClass="ellipsis" /></th>
				                        <td>
                                            <% If Me.HiddenDisplayMode.Value.Equals(DisplayModeInputAll) Then%>
                                                <icrop:CustomTextBox class="option" ID="OptionName" maxlength="32" runat="server" onchange="edit()"/>
                                            <% Else%>
                                                <icrop:CustomLabel ID="LabelOptionName" runat="server" />
                                            <% End If%>
                                        </td> 
				                    </tr>
				                    <tr>
				                        <th><icrop:CustomLabel ID="PriceLabel" runat="server" TextWordNo="4" UseEllipsis="false" width="120px" CssClass="ellipsis"/></th>
				                        <td>
                                            <% If Me.HiddenDisplayMode.Value.Equals(DisplayModeInputAll) Then%>
                                                <icrop:CustomTextBox class="price" ID="Price" maxlength="12" runat="server" onchange="edit()"/>
                                            <% Else%>
                                                <icrop:CustomLabel ID="LabelPrice" runat="server" />
                                            <% End If%>
                                        </td>
				                    </tr>
				                    <tr>
				                        <th><icrop:CustomLabel ID="ImageLabel" runat="server" TextWordNo="5" UseEllipsis="false" width="120px" CssClass="ellipsis"/></th>
				                        <td>
				                            <div class="imageArea">
                                                <% If Me.HiddenDisplayMode.Value.Equals(DisplayModeInputAll) Then%>
											    <div class="imageTextArea" onclick="onClickImage()">
                                                <% Else%>
                                                <div class="imageLabelArea" onclick="onClickImage()">
                                                <% End If%>
                                                    <asp:HiddenField ID="HiddenOptionImagePath" runat="server" />
                                                    <a href="#" id="OptionImageLink" runat="server">
                                                        <icrop:CustomLabel ID="OptionImageName" runat="server" />
                                                    </a>
                                                </div>
                                                
                                                <% If Me.HiddenDisplayMode.Value.Equals(DisplayModeInputAll) Then%>
											        <div class="imageButtonArea">
												        <div class="reference"><asp:FileUpload runat="server" id="reference" css="referenceButton" onchange="editUploadFile()"/></div>
												        <div class="del"><a href="#" onclick="delImage()"><icrop:CustomLabel ID="ImageDelLabel" runat="server" TextWordNo="8" /></a></div>
											        </div>
                                                <% End If%>
										    </div>
				                        </td>
				                    </tr>
				                    <tr>
				                        <th><icrop:CustomLabel ID="RecommendLabel" runat="server" TextWordNo="6" UseEllipsis="false" width="120px" CssClass="ellipsis"/></th>
				                        <td>
					                        <div class="reccomendWrap">
											    <ul class="reccomendCheck">
                                                    <asp:Repeater ID="repeaterRecommendInfo" runat="server" >
                                                    <ItemTemplate>
												        <li>
                                                            <input type="hidden" id="RecommendId" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "RecommendId")%>' />
                                                            <div>
                                                                <% If Not Me.HiddenDisplayMode.Value.Equals(DisplayModeDisplayOnly) Then%>
                                                                    <input type="checkbox" runat="server" id="Recommend" onchange="edit()"/>
                                                                <% End If%>
                                                                <icrop:CustomLabel ID="recommendName" runat="server" UseEllipsis="false" text='<%# DataBinder.Eval(Container.DataItem, "RecommendName")%>' />
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                    </asp:Repeater>
											    </ul>                       
					                        </div>
				                        </td>
				                    </tr>
				                    <tr>
				                        <th class="ListEnd"><icrop:CustomLabel ID="GradeLabel" runat="server" TextWordNo="7" UseEllipsis="false" width="120px" CssClass="ellipsis"/></th>
				                        <td class="ListEnd">
					                        <div class="gradeWrap">
											    <ul class="gradeCheck">
                                                    <asp:Repeater ID="repeaterGradeInfo" runat="server" >
                                                    <ItemTemplate>
												        <li>
                                                            <div>
                                                                <% If Me.HiddenDisplayMode.Value.Equals(DisplayModeInputAll) Then%>
                                                                    <input type="checkbox" runat="server" id="Grade" onchange="edit()"/>
                                                                <% End If%>
                                                                <icrop:CustomLabel ID="gradeName" runat="server" UseEllipsis="false" text='<%# DataBinder.Eval(Container.DataItem, "name")%>' />
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                    </asp:Repeater>
											    </ul>
					                        </div>
				                        </td>
				                    </tr>
				                </table>
                                
                                <%--非表示項目--%>
                                <asp:HiddenField ID="HiddenState" runat="server" />
                                <asp:HiddenField ID="HiddenTimeStamp" runat="server" />
                                <asp:HiddenField ID="HiddenAppId" runat="server" />
                                <%--非表示ボタン--%>
                                <asp:Button ID="SaveButton" runat="server" Text="" style="display:none;" />
                                <asp:Button ID="DeleteButton" runat="server" Text="" style="display:none;" />
                                <asp:Button ID="RefleshButton" runat="server" Text="" style="display:none;" />
                                <asp:Button ID="FooterButton" runat="server" Text="" style="display:none;" />
                            </div>
                        </div>
                    </div>
			    </div>
            </div>
		</div>
    </div>
    <!-- 画像ポップアップ -->
    <div class="windowbBox" id="windowbBox" style="display:none;">
        <%'読み込み中 %>
        <div id="loadingPopup"></div>

        <div class="windowBackFrame" id="windowBackFrame">
            <div class="windowBackFrameClose">
                <img id="windowBackFrameCloseBtn" alt="" src="../Styles/Images/SC3050705/tcvNcvBigWindownClose.png" />
            </div>
            <div id="windowbBoxTitle">
                <div id="windowbBoxName" ></div>
                <div id="windowbBoxPrice" ></div>
                <div id="windowbBoxBorder"></div>
            </div>
            <div class="contentsArea">
                <div class="imgArea" style="width:534px;height:408px;overflow:auto;">
                    <img id="windowbBoxImage" alt="" src="" style="" />
                </div>
            </div>
        </div>
    </div>
    <!-- 画像ポップアップ -->
    <!-- ここまでメインブロック -->
    <asp:HiddenField ID="HiddenOptionId" runat="server" />
    <asp:HiddenField ID="HiddenOptionKind" runat="server" />
    <asp:HiddenField ID="HiddenCarId" runat="server" />
    <asp:HiddenField ID="HiddenProcessId" runat="server" />
    <asp:HiddenField ID="HiddenDisplayMode" runat="server" />
    <asp:HiddenField ID="HiddenDeleteButtonDispFlg" runat="server" />
    <asp:HiddenField ID="HiddenTitle" runat="server" />
    <asp:HiddenField ID="HiddenConfirmMessage" runat="server" />
    <asp:HiddenField ID="HiddenDeleteMessage" runat="server" />
    <asp:HiddenField ID="HiddenImageDeleteMessage" runat="server" />
    <asp:HiddenField ID="HiddenRequiredMessage" runat="server" />
    <asp:HiddenField ID="HiddenPriceMessage" runat="server" />
    <asp:HiddenField ID="HiddenDecimalMessage" runat="server" />
    <asp:HiddenField ID="HiddenUploadMessage" runat="server" />
    <asp:HiddenField ID="HiddenUploadFileSizeMessage" runat="server" />
    <asp:HiddenField ID="HiddenGreadMessage" runat="server" />
    <asp:HiddenField ID="HiddenImageMaxFileSizeField" runat="server" />
    <asp:HiddenField ID="HiddenDecimalPoint" runat="server" />
    <asp:HiddenField ID="modifyDvsField" runat="server" />
    <asp:HiddenField ID="refleshDvsField" runat="server" />
    <%'ファイル名称 %>
    <asp:HiddenField ID="HiddenFileName" runat="server" />
    <asp:HiddenField ID="HiddenFilePath" runat="server" />

    <asp:UpdatePanel ID="OptionInfoCheck" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <%'入力チェックのダミーボタン %>
        <asp:Button ID="ValidationButton" runat="server" style="display:none" />
        <asp:HiddenField ID="ajaxErrorField" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
    <div id="FooterOriginalButton">
        <asp:LinkButton ID="delButtonLink" css="send" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return del();" >
            <icrop:CustomLabel ID="delButtonLabel" runat="server" TextWordNo="10" UseEllipsis="false" Width="80px" CssClass="ellipsis"></icrop:CustomLabel>
        </asp:LinkButton>
        <asp:LinkButton ID="saveButtonLink" css="send" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return save();" >
            <icrop:CustomLabel ID="saveButtonLabel" runat="server" TextWordNo="9" UseEllipsis="false" Width="80px" CssClass="ellipsis"></icrop:CustomLabel>
        </asp:LinkButton>
        <asp:Label ID="Label1" runat="server" Width="10"></asp:Label>
    </div>

    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>
</asp:Content>

