<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3050703.aspx.vb" Inherits="Pages_SC3050703" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3050703/SC3050703.css?20121123000000" />
    <script type="text/javascript" src="../Scripts/SC3050703/SC3050703.js?20130218140000"></script>
    <script type="text/javascript" src="../Scripts/SC3050703/jquery.HScroll.js?20121123000000"></script>
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

                        <!--ここからアングル一覧-->
                        <div id="angleList" class="angle" >
                            <table id="angleListDetails" border="1px" cellspacing="0px" >
                                <tr>
                                    <asp:Repeater ID="repeaterthumbnailInfo" runat="server" >
                                        <ItemTemplate>
                                            <td width="115px" height="64px" onclick='selectAngle("<%# DataBinder.Eval(Container.DataItem, "id")%>", "<%# DataBinder.Eval(Container.DataItem, "gridPath")%>", false);' >
                                                <img id="thumbnailImg" src='<%# DataBinder.Eval(Container.DataItem, "thumbnailPath")%>' alt="" runat="server" />
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </table>
                        </div>
                        <!--ここまでアングル一覧-->

                        <div id="settings" class="settingArea">
                            <div id="settingInner">

                                <div id="imageOuter">
                                    <!--ここからポイント指定-->
                                    <div id="setPointArea" class="setPoint" runat="server" >
                                        <div id="imageFrame" runat="server" >
                                            <asp:table id="frame" border="1" cellspacing="0" runat="server" >
                                            </asp:table>
                                            <div id="SalesPointOverView" style="display:none;left:1px;top:1px;" class="tcvNcvPointSwindown" onclick="return false;" ></div>
                                        </div>
                                    </div>
                                    <!--ポイント指定-->
                                </div>
                            
                                <div class="list">
                                <h2><icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="5" UseEllipsis="false" Width="160px" CssClass="ellipsis" /></h2>
                                    <div class="listWrap">
                                        <table>
                                            <tr>
                                                <td class="inputLabel"><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="3" UseEllipsis="false" Width="120px" CssClass="ellipsis" /></td>
                                                <td class="inputItem"><input id="salesPointTxt" type="text" value="" runat="server" onchange="onChangeDisplay();" maxlength="32" /></td>
                                            </tr>
                                            <tr>
                                                <td class="inputLabel"><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="4" UseEllipsis="false" Width="120px" CssClass="ellipsis" /></td>
                                                <td class="inputItem">
                                                    <textarea id="contentsTxt" runat="server" onchange="onChangeDisplay();" maxlength="512"></textarea>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="summary" >
                                    <h2><icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="6" UseEllipsis="false" Width="160px" CssClass="ellipsis" /></h2>
                                    <div class="summaryWrap">
                                        <table>
                                            <tr>
                                                <td class="inputLabelSummary"><icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="7" UseEllipsis="false" Width="90px" CssClass="ellipsis" /></td>
                                                <td class="inputItemSummary">
                                                    <div class="file" onclick="onClickImage(0);" ><a href="#" id="overViewLink" runat="server"><icrop:CustomLabel ID="overViewFile" runat="server" UseEllipsis="false" width="200px" CssClass="ellipsis" /></a></div>
                                                    <div class="inputFile"><asp:FileUpload ID="summaryFile" runat="server" onchange="onChangeUploadFile(1);" /></div>
                                                </td>
                                                <td class="inputItemSummaryDel" >
                                                    <div class="del" onclick="deleteSummaryFile();" ><a href="#"><icrop:CustomLabel ID="CustomLabel6" runat="server" TextWordNo="8" UseEllipsis="false" Width="80px" CssClass="ellipsis" /></a></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="detail">
                                    <h2><icrop:CustomLabel ID="CustomLabel7" runat="server" TextWordNo="9" UseEllipsis="false" Width="160px" CssClass="ellipsis" /></h2>
                                    <div class="detailWrap">
                                        <table>
                                            <tr>
                                                <td class="inputLabelDetail"><icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="10" UseEllipsis="false" Width="90px" CssClass="ellipsis" /></td>
                                                <td class="inputItemDetail">
                                                    <div class="file" onclick="onClickImage(1);"><a href="#" id="popUpLink" runat="server"><icrop:CustomLabel ID="popUpFile" runat="server" UseEllipsis="false" width="200px" CssClass="ellipsis" /></a></div>
                                                    <div class="inputFile"><asp:FileUpload ID="detailFile" runat="server" onchange="onChangeUploadFile(2);setDetailPopupDisabled();" /></div>
                                                </td>
                                                <td class="inputItemDetailDel">
                                                    <div class="del" onclick="deleteDetailFile();" ><a href="#"><icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="11" UseEllipsis="false" Width="80px" CssClass="ellipsis" /></a></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="detailPopup">
                                    <h2><icrop:CustomLabel ID="CustomLabel10" runat="server" TextWordNo="12" UseEllipsis="false" Width="160px" CssClass="ellipsis" /></h2>
                                    <div class="detailWrapPopup">
                                        <table>
                                            <tr>
                                                <td class="inputLabelDetailPopup"><icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="13" UseEllipsis="false" Width="90px" CssClass="ellipsis" /></td>
                                                <td class="inputItemDetailPopup">
                                                    <div class="file" onclick="onClickImage(2);" ><a href="#" id="fullPopUpLink" runat="server"><icrop:CustomLabel ID="fullPopUpFile" runat="server" UseEllipsis="false" width="200px" CssClass="ellipsis" /></a></div>
                                                    <div class="inputFile"><asp:FileUpload ID="detailPopupFile" runat="server" onchange="onChangeUploadFile(3);" /></div>
                                                </td>
                                                <td class="inputItemDetailDelPopup">
                                                    <div class="del" onclick="deleteDetailPopupFile();" ><a href="#"><icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="14" UseEllipsis="false" Width="80px" CssClass="ellipsis" /></a></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="grade">
                                    <h2><icrop:CustomLabel ID="gradeLabel1" runat="server" TextWordNo="2" UseEllipsis="false" Width="160px" CssClass="ellipsis" /></h2>
                                    <div id="gradeWrapArea" class="gradeWrap">
                                    <table id="boxscroll">
                                    <tr>
                                    <td>
                                        <ul class="gradeCheck">
                                            <asp:Repeater ID="repeaterGradeInfo" runat="server" >
                                                <ItemTemplate>
                                                    <li class="gradeCheckLi">
                                                        <div>
                                                            <input type="checkbox" id="grade" runat="server" onchange="onChangeDisplay();" /><icrop:CustomLabel ID="gradeName" runat="server" UseEllipsis="false" text='<%# DataBinder.Eval(Container.DataItem, "name")%>' />
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>                                                                                
                                    </td>
                                    </tr>
                                    </table>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <%'遷移パラメータ %>
                        <asp:HiddenField ID="carSelectField" runat="server" />
                        <asp:HiddenField ID="exInField" runat="server" />
                        <asp:HiddenField ID="salesPointIdField" runat="server" />

                        <%'画面情報 %>
                        <asp:HiddenField ID="targetID" runat="server" />
                        <asp:HiddenField ID="angleField" runat="server" />
                        <asp:HiddenField ID="angleBackUpField" runat="server" />
                        <asp:HiddenField ID="defaultGridPathField" runat="server" />
                        <asp:HiddenField ID="salesPointNoField" runat="server" />
                        <asp:HiddenField ID="topPointField" runat="server" />
                        <asp:HiddenField ID="leftPointField" runat="server" />
                        <asp:HiddenField ID="topOverPointField" runat="server" />
                        <asp:HiddenField ID="leftOverPointField" runat="server" />
                        <asp:HiddenField ID="overViewFilePathField" runat="server" />
                        <asp:HiddenField ID="popUpFilePathField" runat="server" />
                        <asp:HiddenField ID="fullPopUpFilePathField" runat="server" />
                        <asp:HiddenField ID="topPointBkField" runat="server" />
                        <asp:HiddenField ID="leftPointBkField" runat="server" />

                        <%'ファイル名称 %>
                        <asp:HiddenField ID="overViewFileNameField" runat="server" />
                        <asp:HiddenField ID="popUpFileNameField" runat="server" />
                        <asp:HiddenField ID="fullPopUpFileNameField" runat="server" />

                        <%'JSON情報 %>
                        <asp:HiddenField ID="salesPointJsonField" runat="server" />

                        <%'メッセージ関連 %>
                        <asp:HiddenField ID="pointMessageField" runat="server" />
                        <asp:HiddenField ID="salesPointMessageField" runat="server" />
                        <asp:HiddenField ID="greadMessageField" runat="server" />
                        <asp:HiddenField ID="summaryFileSizeMessageField" runat="server" />
                        <asp:HiddenField ID="detailFileSizeImageMessageField" runat="server" />
                        <asp:HiddenField ID="detailFileSizeMovieMessageField" runat="server" />
                        <asp:HiddenField ID="detailPopupFileSizeMessageField" runat="server" />
                        <asp:HiddenField ID="summaryMessageField" runat="server" />
                        <asp:HiddenField ID="detailMessageField" runat="server" />
                        <asp:HiddenField ID="detailPopupMessageField" runat="server" />
                        <asp:HiddenField ID="summaryAlertField" runat="server" />
                        <asp:HiddenField ID="detailAlertField" runat="server" />
                        <asp:HiddenField ID="detailPopupAlertField" runat="server" />
                        <asp:HiddenField ID="deleteAlertField" runat="server" />
                        <asp:HiddenField ID="modifyMessageField" runat="server" />
                        <asp:HiddenField ID="modifyDvsField" runat="server" />

                        <asp:HiddenField ID="refleshDvsField" runat="server" />
                        <asp:HiddenField ID="overViewImageMaxFileSizeField" runat="server" />
                        <asp:HiddenField ID="popUpImageMaxFileSizeField" runat="server" />
                        <asp:HiddenField ID="fullPopUpImageMaxFileSizeField" runat="server" />
                        <asp:HiddenField ID="movieMaxFileSizeField" runat="server" />

                    </div>
                </div>
            </div>
        </div>

        <%'削除のダミーボタン %>
        <asp:Button ID="DeleteButton" runat="server" style="display:none" />

        <%'保存のダミーボタン %>
        <asp:Button ID="SendButton" runat="server" style="display:none" />

        <%'リフレッシュのダミーボタン %>
        <asp:Button ID="RefleshButton" runat="server" style="display:none" />

        <asp:UpdatePanel ID="SalesPointListPanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <%'入力チェックのダミーボタン %>
            <asp:Button ID="CheckButton" runat="server" style="display:none" />
            <asp:HiddenField ID="ajaxErrorField" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <!-- ここまでメインブロック -->

    <!-- 画像ポップアップ -->
    <div class="tcvNcvPointBigWindowbBox" id="tcvNcvPointBigWindowbBox" style="display:none;">
        <%'読み込み中 %>
        <div id="loadingPopup"></div>

        <div class="tcvNcvPointBigWindowBackFrame" id="tcvNcvPointBigWindowBackFrame">
            <div class="tcvNcvPointBigWindowBackFrameClose">
                <img id="tcvNcvPointBigWindowBackFrameCloseBtn" src="../Styles/Images/SC3050703/tcvNcvBigWindownClose.png" width="29" height="29" alt="" />
            </div>
            <h4 id="tcvNcvPointBigWindowbBoxTitle"></h4>
            <div class="tcvNcvPointContentsArea">
                <div class="tcvNcvPointImgArea" style="width:534px;height:408px;overflow:auto;">
                    <img id="tcvNcvPointBigWindowbBoxImage" src="" alt="" style="" />
                    <video id="tcvNcvPointBigWindowbBoxVideo" src="" width="516" height="397" controls="controls"></video>
                </div>
            </div>
        </div>
    </div>
    <!-- 画像ポップアップ -->

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">

    <%'登録時のオーバーレイ %>
    <div id="registOverlayBlack"></div>
    <div id="processingServer"></div>

    <div id="FooterOriginalButton">
        <asp:LinkButton ID="DelButtonLink" class="send" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return deleteSalesPointInfo();" >
            <icrop:CustomLabel ID="DelButtonLabel" runat="server" TextWordNo="16" UseEllipsis="false" Width="80px" CssClass="ellipsis" />
        </asp:LinkButton>
        <asp:LinkButton ID="SendButtonLink" class="send" runat="server" Width="75" Height="46" CausesValidation="False" OnClientClick="return sendSalesPointInfo();" >
            <icrop:CustomLabel ID="SendButtonLabel" runat="server" TextWordNo="15" UseEllipsis="false" Width="80px" CssClass="ellipsis" />
        </asp:LinkButton>
        <asp:Label ID="Label1" runat="server" Width="10"></asp:Label>
    </div>

</asp:Content>

