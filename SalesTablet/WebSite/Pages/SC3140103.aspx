<%@ Page Title="" Language="VB" MasterPageFile="~/Pages/SC3010201.master" AutoEventWireup="false" CodeFile="SC3140103.aspx.vb" Inherits="Pages_SC3140103" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SC3010201head" Runat="Server">
    <%'HEAD %>
    <link rel="Stylesheet" type="text/css" href="../Styles/SC3140103/SC3140103.css" />
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.Main.js"></script>
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.popoverEx.js"></script>
    <script type="text/javascript" src="../Scripts/SC3140103/SC3140103.flickable.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SC3010201leftBottomBox" Runat="Server">
    <%'ダッシュボード %>
    <div id="dashboardBox">
        <iframe id="dashboardFrame"  height="100%" width="100%" src="SC3140102.aspx"></iframe>
        <%'読み込み中 %>
        <div id="loadingDashboard"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SC3010201rightBox" Runat="Server">
        <%--カウンター対応--%>
    <script type="text/javascript">
        var diffseconds = (new Date('<%=Me.mNow%>')).getTime() - (parseInt((new Date()).getTime() / 1000) * 1000);
        setInterval("proccounter(diffseconds);", 1000);
    </script>
    <%--カウンター対応--%>

    <%--詳細ポップアップウィンドウ用--%>
    <%-- 詳細画面ポップアップ --%>
    <div id="CustomerPopOver" class="saPopOver">
        <div class="triangle"></div>
        <%-- ヘッダー --%>
	    <div class="header" >
            <h3><icrop:CustomLabel ID="PopupHeader" runat="server" TextWordNo="10" UseEllipsis="False"></icrop:CustomLabel></h3>
        </div>
        <%-- 詳細 --%>
        <div class="content">
            <asp:UpdatePanel ID="ContentUpdatePanelDetail" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="IcnSet">
                        <icrop:CustomLabel  ID="DetailsRightIconD" runat="server" text="" visible="False"  CssClass="PopoverRightIcnD" TextWordNo="7"></icrop:CustomLabel>
                        <icrop:CustomLabel  ID="DetailsRightIconI" runat="server" text="" visible="False"  CssClass="PopoverRightIcnI" TextWordNo="8"></icrop:CustomLabel>
                        <icrop:CustomLabel  ID="DetailsRightIconS" runat="server" text="" visible="False"  CssClass="PopoverRightIcnS" TextWordNo="9"></icrop:CustomLabel>
                    </div>
                    <div>
                        <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                            <tr>
                                <th><icrop:CustomLabel ID="ItemRegistrationNumber" runat="server" TextWordNo="11" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsRegistrationNumber" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th><icrop:CustomLabel ID="ItemCarModel" runat="server" TextWordNo="12" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsCarModel" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th><icrop:CustomLabel ID="ItemModel" runat="server" TextWordNo="13" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsModel" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th><icrop:CustomLabel ID="ItemVin" runat="server" TextWordNo="14" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsVin" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th><icrop:CustomLabel ID="ItemMileage" runat="server" Text="" TextWordNo="15" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsMileage" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th class="ListEnd"><icrop:CustomLabel ID="ItemDeliveryCarDay" runat="server" Text="" TextWordNo="16" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td class="ListEnd"><icrop:CustomLabel ID="DetailsDeliveryCarDay" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                        </table>
                        <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                            <tr>
                                <th><icrop:CustomLabel ID="ItemCustomerName" runat="server" Text="" TextWordNo="17" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsCustomerName" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th><icrop:CustomLabel ID="ItemPhoneNumber" runat="server" Text="" TextWordNo="18" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsPhoneNumber" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th class="ListEnd"><icrop:CustomLabel ID="ItemMobileNumber" runat="server" Text="" TextWordNo="19" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td class="ListEnd"><icrop:CustomLabel ID="DetailsMobileNumber" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                        </table>
                        <table border="0" cellspacing="0" cellpadding="0" class="ListSet">
                            <tr>
                                <th><icrop:CustomLabel ID="ItemTime" runat="server" Text="" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td><icrop:CustomLabel ID="DetailsVisitTime" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                            <tr>
                                <th class="ListEnd"><icrop:CustomLabel ID="ItemRepresentativeWarehousing" runat="server" Text="" TextWordNo="21" CssClass="Ellipsis" Width="70"></icrop:CustomLabel></th>
                                <td class="ListEnd"><icrop:CustomLabel ID="DetailsRepresentativeWarehousing" runat="server" Text="" CssClass="Ellipsis" Width="170"></icrop:CustomLabel></td>
                            </tr>
                        </table>
                        <%--画面遷移ボタン --%>
                        <asp:Button ID="DetailButtonLeft" runat="server" Text="" CssClass="FooterButton01" OnClientClick="ButtonControl('#DetailButtonLeft');" />
                        <asp:Button ID="DetailButtonRight" runat="server" Text="" CssClass="FooterButton02" OnClientClick="ButtonControl('#DetailButtonRight');" />
                        <%--画面遷移ボタン押下時の2度押し防止用ダミーボタン --%>
                        <asp:Button ID="DetailButtonLeft_Dammy" runat="server" Text="" CssClass="FooterButton01" style="display: none" Enabled="False" />
                        <asp:Button ID="DetailButtonRight_Dammy" runat="server" Text="" CssClass="FooterButton02" style="display: none" Enabled="False" />
                    </div>

                    <%--タップをダブルタップ時に詳細ポップアップウィンドウを表示情報取得--%>
                    <asp:Button ID="DetailPopupButton" runat="server" style="display:none" />
                    <%-- 詳細ポップアップウィンドウの読み込み中アイコン --%>
                    <div id="IconLoadingPopup" class="loadingPopup" runat="server"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--チップ詳細ボタン押下時の共通遷移イベント発生ボタン --%>
            <asp:Button ID="DetailNextScreenCommonButton" runat="server" style="display:none" />
            <%--チップ詳細ボタン押下時の押下されたボタン名称格納用--%>
            <asp:HiddenField ID="DetailClickButtonName" runat="server" />
        </div>
    </div>

    <%-- 選択されたチップ詳細情報格納 --%>
    <asp:HiddenField ID="DetailsVisitNo" runat="server" />
    <asp:HiddenField ID="DetailsArea" runat="server" />
    <asp:HiddenField ID="DetailsOrderNo" runat="server" />
    <asp:HiddenField ID="DetailsApprovalId" runat="server" />

    <%-- 工程管理ボックス --%>
    <div id="contentsRightBox1">

        <asp:UpdatePanel ID="ContentUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <%-- 受付エリア --%>
            <div class="ColumnBox01">
                <%--通知ポーリング処理--%>
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordReception" runat="server" CssClass="Ellipsis" Width="100" Text="" TextWordNo="2"></icrop:CustomLabel>
                </h2>
                <%-- 受付状態のチップ数 --%>
                <div class="contentTitleNo">
                    <%-- 通知リフレッシュボタン(隠しボタン) --%>
                    <asp:Button ID="MainPolling" runat="server" CssClass="MainRefreshStyle" />

                    <icrop:CustomLabel ID="ReceptionDeskTipNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable1" class="ColumnContentsFlame">
                    <ul>
                        <%-- 受付情報の表示 --%>
                        <asp:Repeater ID="ReceptionRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipRight" id='Reception'>

                                        <%-- チップエリア --%>
                                        <div id="ReceptionDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">
                                                <%-- チップ上段(マーク) --%>
                                                <div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>
                                                <%-- チップ下段(詳細情報) --%>
                                                <div class="ColumnTextBox">
                                                    <div ID="RegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="CustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                    <div ID="VisitTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="RepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="ParkingNumber" runat="server" class="EllipsisTextRight" style="width:60px"></div>
                                                    </div>
                                                    <div class="ColumnTime">
                                                        <div ID="ElapsedTime" runat="server" text=""></div>
                                                    </div>
                                                </div>
                                            </div>
                                        <%-- チップエリア終了 --%>
                                        </div>
                                    </div>
                                </li>
                           </ItemTemplate>
                        </asp:Repeater>
                    </ul>                        
                </div>
            </div>

            <%-- 追加承認エリア --%>
            <div class="ColumnBox02">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WorkApproval" runat="server" Text="" TextWordNo="4" CssClass="Ellipsis" Width="100"></icrop:CustomLabel>
                </h2>
                <%-- 追加承認中のチップ数 --%>
                <div class="contentTitleNo">
                    <icrop:CustomLabel ID="ApprovalNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable2" class="ColumnContentsFlame">
                    <ul>
                        <%-- 追加承認情報の表示 --%>
                        <asp:Repeater ID="ApprovalRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipRight" id='Approval'>
                                        <%-- チップエリア --%>
                                        <div id="ApprovalDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">
                                                <%-- チップ上段(マーク) --%>
                                                <div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                                </div>
                                                <%-- チップ下段(詳細情報) --%>
                                                <div class="ColumnTextBox">
                                                    <div ID="ApprovalRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="ApprovalCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                    <div ID="ApprovalDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="ApprovalRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="ApprovalChargeTechnician" runat="server" class="EllipsisTextRight" style="width:60px"></div>
                                                    </div>
                                                    <div class="ColumnTime">
                                                        <div ID="ApprovalElapsedTime" runat="server" text=""></div>
                                                    </div>
                                                </div>
                                            </div>
                                        <%-- チップエリア終了 --%>
                                        </div>
                                    </div>
                                <li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                       
                </div>
            </div>

            <%-- 納車準備エリア --%>
            <div class="ColumnBox03">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordPreparation" runat="server" Text="" TextWordNo="5" CssClass="Ellipsis" Width="100"></icrop:CustomLabel>
                </h2>
                <%-- 納車準備エリアのチップ数 --%>
                <div class="contentTitleNo">
                    <icrop:CustomLabel ID="PreparationNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable3" class="ColumnContentsFlame">
                    <ul>
                        <%-- 納車準備情報の表示 --%>
                        <asp:Repeater ID="PreparationRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipLeft" id='Preparation'>
                                        <%-- チップエリア --%>
                                        <div id="PreparationDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">
                                                <%-- チップ上段(マーク) --%>
                                                <div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                               </div>
                                                <%-- チップ下段(詳細情報) --%>
                                                <div class="ColumnTextBox">
                                                    <div ID="PreparationRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="PreparationCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                    <div ID="PreparationDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="PreparationRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="PreparationChargeTechnician" runat="server" class="EllipsisTextRight" style="width:60px"></div>
                                                    </div>
                                                    <div class="ColumnTime">
                                                        <div ID="PreparationElapsedTime" runat="server" text=""></div>
                                                    </div>
                                                </div>
                                            </div>
                                        <%-- チップエリア終了 --%>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                        
                </div>
            </div>

            <%-- 納車作業エリア --%>
            <div class="ColumnBox04">
                <h2 class="contentTitle">
                   <icrop:CustomLabel ID="WordDelivery" runat="server" Text="" CssClass="Ellipsis" Width="100" TextWordNo="6"></icrop:CustomLabel>
                </h2>
                <%-- 納車作業エリアのチップ数 --%>
                <div class="contentTitleNo">
                    <icrop:CustomLabel ID="DeliveryNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable4" class="ColumnContentsFlame">
                    <ul>
                        <%-- 納車作業情報の表示 --%>
                        <asp:Repeater ID="DeliveryRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="CustomerChipLeft" id='Delivery'>
                                        <%-- チップエリア --%>
                                        <div id="DeliveryDeskDevice" runat="server" class="" visible="true">
                                            <div class="ColumnContentsBoderIn">
                                                <%-- チップ上段(マーク) --%>
                                                <div class="IcnSet">
                                                    <div ID="RightIcnD" runat="server" text="" visible="False" class="RightIcnD"></div>
                                                    <div ID="RightIcnI" runat="server" text="" visible="False" class="RightIcnI"></div>
                                                    <div ID="RightIcnS" runat="server" text="" visible="False" class="RightIcnS"></div>
                                               </div>
                                                <%-- チップ下段(詳細情報) --%>
                                                <div class="ColumnTextBox">
                                                    <div ID="DeliveryRegistrationNumber" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="DeliveryCustomerName" runat="server" class="Ellipsis" style="width:130px"></div>
                                                    <div ID="DeliveryDeliveryPlanTime" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div ID="DeliveryRepresentativeWarehousing" runat="server" class="Ellipsis" style="width:70px"></div>
                                                    <div class="ColumnNo">
                                                        <div ID="DeliveryChargeTechnician" runat="server" class="EllipsisTextRight" style="width:60px"></div>
                                                    </div>
                                                    <div class="ColumnTime">
                                                        <div ID="DeliveryElapsedTime" runat="server" text=""></div>
                                                    </div>
                                                </div>
                                            </div>
                                        <%-- チップエリア終了 --%>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>                       
                </div>
            </div>

            <%-- 作業中エリア --%>
            <div class="ColumnSide">
                <h2 class="contentTitle">
                    <icrop:CustomLabel ID="WordWork" runat="server" Text="" TextWordNo="3" CssClass="Ellipsis" Width="500"></icrop:CustomLabel>
               </h2>
                <%-- 作業中エリアの表示チップ数 --%>
                <div class="contentTitleNo">
                    <icrop:CustomLabel ID="WorkNumber" runat="server" Text=""></icrop:CustomLabel>
                </div>
                <div id="flickable5"  class="ColumnSideFrame">
                    <div class="ColumnSideBoderIn">
                        <table border="0" cellspacing="0" cellpadding="0" class="">
                            <tr>
                               <%--作業中エリアの表示 --%>
                                <asp:Repeater ID="WorkRepeater" runat="server">
                                    <ItemTemplate>
                                        <td>
                                            <div id='Work' class="CustomerChipTop">
                                                <%-- チップエリア --%>
                                                <div id="Working" runat="server" class="" visible="true">
                                                    <div class="ColumnContents02BoderIn">
                                                        <%-- チップ詳細情報 --%>
                                                        <div class="WorkIcnSet">
                                                            <div ID="WorkRightIcnD" runat="server" text="" visible="False" class="WorkRightIcnD"></div>
                                                            <div ID="WorkRightIcnI" runat="server" text="" visible="False" class="WorkRightIcnI"></div>
                                                            <div ID="WorkRightIcnS" runat="server" text="" visible="False" class="WorkRightIcnS"></div>
                                                        </div>
                                                        <div class="ColumnWatch">
                                                            <div ID="WorkCompletionPlanTime" runat="server" text=""></div>
                                                        </div>

                                                        <div class="ColumnTimeGray">
                                                            <div ID="WorkTimeLag" runat="server" text=""></div>
                                                        </div>

                                                        <%--<div id="WorkTextBox" runat="server" class="" visible="true">--%>
                                                        <div id="WorkTextBox" runat="server" class="ColumnTextBox" visible="true">
                                                            <div ID="WorkRegistrationNumber" runat="server" text="" class="Ellipsis" style="width:110px"></div>
                                                            <div ID="WorkCustomerName" runat="server" text=""  class="Ellipsis" style="width:110px"></div>
                                                            <div ID="WorkDeliveryPlanTime" runat="server" text=""  class="Ellipsis" style="width:75px"></div>
                                                            <div ID="WorkRepresentativeWarehousing" runat="server" text="" class="Ellipsis" style="width:75px"></div>
                                                            <div class="IcnNo"  visible="False">
                                                                <div ID="AdditionalWorkNumber" runat="server" text="" style="text-align:right;"></div>
                                                            </div>
                                                            <div id="WorkIcon" runat="server" class=""></div>
                                                        </div>
                                                    </div>
                                                <%-- チップエリア終了 --%>
                                                </div>
                                            </div>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <%-- 工程管理ボックスの読み込み中アイコン --%>
            <div id="loadingSchedule"  runat="server"></div>
        </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <%--カウンター対応--%>
    <script type="text/javascript">
        proccounter();
    </script>
    <%--カウンター対応--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SC301020footer" Runat="Server">
</asp:Content>

