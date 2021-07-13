<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Master/CommonMasterPage.Master" CodeFile="SC3080225.aspx.vb" Inherits="Pages_SC3080225" %>
<%@ Register Src="SC3080204.ascx" TagName="SC3080204" TagPrefix="uc1" %>
<%@ Register src="SC3080214.ascx" Tagname="SC3080214" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../Styles/SC3080225/SC3080225.css?20200325000001" type="text/css" media="screen,print" />
    <link rel="stylesheet" href="../Styles/SC3080225/popupWindow.css?20140301000001" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/icrop.clientapplication.js?20140612000000"></script>
    <script type="text/javascript" src="../Scripts/icrop.push.js?20140612000000"></script>
    <script type="text/javascript" src="../Scripts/SC3080225/SC3080225.js?20200325000001"></script>
    <script type="text/javascript" src="../Scripts/SC3080225/SC3080225.Fingerscroll.js?20140301000001"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <asp:ScriptManager ID="AjaxListManager" runat="server" EnablePageMethods="True" ></asp:ScriptManager>
    
    <%--クルクル領域--%>
    <div id="LoadingScreen">
        <div id="LoadingWrap">
            <div class="loadingIcn">
                <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
            </div>
        </div>
    </div>

    <%--クルクル領域--%>
    <div id="LoadingScreenRight" style="display:none;">
        <div id="LoadingWrapRight">
            <div class="loadingIcnRight">
                <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
            </div>
        </div>
    </div>

    <%--ポップアップ外領域--%>
    <div id="PupupBackGroud" style="width:100%;height:100%;z-index:991;position:absolute;display:none;top:0px;left:0px;">
    </div>
    <asp:UpdatePanel ID="MainPageArea" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:HiddenField ID="HiddenFieldDmsCustomerCode" runat="server" />
    <asp:HiddenField ID="HiddenFieldVin" runat="server" />
    <asp:HiddenField ID="HiddenFieldIcropDmsCustomerCode" runat="server" />
    <asp:HiddenField ID="HiddenFieldVehicleListJsonData" runat="server" />
    <asp:HiddenField ID="HiddenFieldFileUpLoadPath" runat="server" />
    <%'2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START %>
    <asp:HiddenField ID="HiddenFieldFileUpLoadUrl" runat="server" />
    <%'2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END %>
    <%--(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START --%>
    <asp:HiddenField ID="HiddenFieldExtension" runat="server" Value="" />
    <%--(トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END --%>
    <%--(トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START--%>
    <asp:HiddenField ID="HiddenFieldMaxDisplayCount" runat="server" Value="" />
    <asp:HiddenField ID="HiddenFieldDefaultReadCount" runat="server" Value="" />
    <%--(トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END--%>
    <asp:Button ID="MainPageReloadButton" runat="server" style="display:none;" />
    <div id="contentsFrame" class="contentsFrame">
        <!--ここまでページネーション-->
	    <div id="mainblockContent">
            <!--ここから左カラム-->
		    <div id="mainblockContentLeft">
			    <h2 class="LongType">
                    <%--顧客情報ヘッダー--%>
                    <icrop:CustomLabel ID="CustomerInfomationHeader" runat="server" Width="200px" TextWordNo="1" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                </h2>
			    <div class="mainblockContentLeftWrap">
				    <!--ここから顧客情報-->
				    <div class="mainblockContentLeftCustomer">
					    <div class="mainblockContentLeftCustomerPhoto">
                            <asp:UpdatePanel ID="CuostomerPhotoArea" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
						            <p>
                                        <img onclick="javascript:OpenPhotoRegister();" runat="server" id="CustomerPhotoIcon" alt="" src="../Styles/Images/SC3080225/photo00.png" width="60" height="60" />
                                    </p>
                                    <asp:Button ID="CustomerPhotoRegistButton" runat="server" style="display:none;" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
					    </div>
					    <div class="mainblockContentLeftCustomerName">
						    <dl>
							    <dt id="CustomerNameArea">
                                    <%--顧客氏名--%>
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                    <%-- <icrop:CustomLabel runat="server" ID="CustomerName" Width="300px" CssClass="SC3080225Ellipsis" /> --%>
                                    <icrop:CustomLabel runat="server" ID="CustomerName" Width="290px" CssClass="SC3080225Ellipsis" />
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                </dt>
							    <dd runat="server" ID="CustomerIconArea" style="display:none;">
                                    <%--アイコンラベル--%>
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                    <icrop:CustomLabel runat="server" ID="LIcon" Width="23px" CssClass="IcoL SC3080225Ellipsis" style="display:none;"/>
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                    <icrop:CustomLabel runat="server" ID="FreetIcon" Width="23px" CssClass="mainblockContentLeftCustomerNameIco03 SC3080225Ellipsis" />
                                    <icrop:CustomLabel runat="server" ID="CustomerTypeIcon" Width="23px" CssClass="mainblockContentLeftCustomerNameIco02 SC3080225Ellipsis" />                                    
                                </dd>
						    </dl>
                            <div id="DmsIdArea">
                                <%--基幹顧客コード--%>
                                <icrop:CustomLabel runat="server" ID="DmsIdWord" Width="60px" CssClass="SC3080225Ellipsis" TextWordNo="89" />
                                <icrop:CustomLabel runat="server" ID="DmsId" Width="315px" CssClass="SC3080225Ellipsis" />
                            </div>
					    </div>
				    </div>
              
				    <div class="mainblockContentLeftCustomerDetail clearfix">
					    <ul class="mainblockContentLeftCustomerDetailLeft">
						    <li class="mainblockContentLeftCustomerCell">
                                <%--顧客携帯電話番号--%>
                                <icrop:CustomLabel runat="server" ID="CstMobile" Width="200px" CssClass="SC3080225Ellipsis" />
                            </li>
						    <li class="mainblockContentLeftCustomerTel">
                                <%--顧客電話番号--%>
                                <icrop:CustomLabel runat="server" ID="CstPhone" Width="200px" CssClass="SC3080225Ellipsis" />
                            </li>
						    <li class="mainblockContentLeftCustomerEmail">
                                <%--<a href="#">watanabe@toyota.com</a>--%>
                                <%--顧客EMAILアドレス--%>
                                <icrop:CustomLabel runat="server" ID="CstEmail" Width="200px" CssClass="SC3080225Ellipsis" />
                            </li>
					    </ul>
					    <ul class="mainblockContentLeftCustomerDetailRight">
						    <li class="mainblockContentLeftCustomerAddress">
                                <%--顧客郵便番号--%>
                                <icrop:CustomLabel runat="server" ID="CstZipCode" Width="200px" CssClass="SC3080225Ellipsis" />
                            </li>
						    <li>
                                <%--顧客住所--%>
                                <icrop:CustomLabel runat="server" ID="CstAddress" Width="200px" Height="38px" UseEllipsis="false" />
                            </li>
					    </ul>
				    </div>
                    <!--ここまで顧客情報-->
                    <!--ここから保有車種-->
				    <div class="mainblockContentLeftCustomerCar">
					    <div class="mainblockContentLeftCustomerCarTitle">
						    <h3>
                                <%--車両情報ヘッダー--%>
                                <icrop:CustomLabel ID="VehicleInformationHeader" runat="server" Width="200px" TextWordNo="2" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </h3>
						    <p>
                                <%--保有車両台数--%>
                                <icrop:CustomLabel ID="NumberOfVehicles" runat="server" Width="34px" CssClass="mainblockContentLeftCustomerCarNum SC3080225Ellipsis" style="display:none;" ></icrop:CustomLabel>
                                <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START --%>
                                <span runat="server" id="SSCIcon" class="IcoSSC" style="display: none;">
                                    <icrop:CustomLabel runat="server" id="SSCWord" CssClass="Ellipsis" /></span>
                                <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END --%>
                            </p>
					    </div>
					    <div class="mainblockContentLeftCustomerCarName">
						    <dl>
							    <dt id="LogoArea" runat="server">                                 
                                    <%--ロゴ--%>
                                    <img id="VehicleLogoIcon" runat="server" alt="" src="../Styles/Images/SC3080225/car_logo01.png" width="185" height="30" style="display:none;" />
                                    
								    <table runat="server" id="VehicleMakerModelTable" border="0" class="NoBorderTable" style="display:none;">
								        <tr>
								            <td width="63px" height="24px">
                                                <icrop:CustomLabel ID="VehicleMakerName" runat="server" Width="63px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                            </td>
								            <td class="CarTypeBoldText">
                                                <icrop:CustomLabel ID="VehicleModelName" runat="server" Width="140px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                            </td>
							            </tr>
							        </table>
                                </dt>
                                <dd class="mainblockContentLeftCustomerEdition">                                    
                                    <%--グレード--%>
                                    <icrop:CustomLabel ID="VehicleGrade" runat="server" Width="185px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
							    <dd class="mainblockContentLeftCustomerColor">                                       
                                    <%--外装色--%>
                                    <icrop:CustomLabel ID="VehicleBodyColor" runat="server" Width="185px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>							    
						    </dl>
						    <div class="mainblockContentLeftCustomerCarDetail">
							    <ul>
								    <li class="mainblockContentLeftCustomerCarDetail1">
                                        <%--車両登録番号--%>
                                        <icrop:CustomLabel ID="VehicleRegNoWord" runat="server" Width="30px" TextWordNo="8" CssClass="BackGroundGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START --%>
                                        <%-- <icrop:CustomLabel ID="VehicleRegNo" runat="server" Width="160px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel> --%>
                                        <icrop:CustomLabel ID="VehicleRegNo" runat="server" Width="140px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END --%>
                                    </li>
								    <li class="mainblockContentLeftCustomerCarDetail5">
                                        <%--Province--%>                                        
                                        <icrop:CustomLabel ID="VehicleProvince" runat="server" Width="160px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                    </li>
								    <li class="mainblockContentLeftCustomerCarDetail2">
                                        <%--VIN--%>
                                        <icrop:CustomLabel ID="VehicleVinWord" runat="server" Width="30px" TextWordNo="9" CssClass="BackGroundGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                        <%--<icrop:CustomLabel ID="VehicleVin" runat="server" Width="160px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>--%>
                                        <icrop:CustomLabel ID="VehicleVin" runat="server" Width="130px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <%--Pアイコン--%>
                                        <span runat="server" id="PIcon" class="IconP" style="display: none;">
                                        <icrop:CustomLabel ID="PWord" runat ="server" Width = "15px" CssClass="IconP" /></span>
                                        <%--Tアイコン--%>
                                        <span runat="server" id="TIcon" class="IconT" style="display: none;">
                                        <icrop:CustomLabel ID="TWord" runat ="server" Width = "15px" CssClass="IconT" /></span>
                                        <%--Eアイコン--%>
                                        <span runat="server" id="EIcon" class="IconE" style="display: none;">
                                        <icrop:CustomLabel ID="EWord" runat ="server" Width = "15px" CssClass="IconE" /></span>
                                        <%--Bアイコン--%>
                                        <span runat="server" id="BIcon" class="IconB" style="display: none;">
                                        <icrop:CustomLabel ID="BWord" runat ="server" Width = "15px" CssClass="IconB" /></span>
                                        <%--Mアイコン--%>
                                        <span runat="server" id="MIcon" class="IconM" style="display: none;">
                                        <icrop:CustomLabel ID="MWord" runat ="server" Width = "15px" CssClass="IconM" /></span>
                                        <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                    </li>
								    <li class="mainblockContentLeftCustomerCarDetail3">
                                        <%--納車日--%>
                                        <icrop:CustomLabel ID="VehicleDeliveryDateWord" runat="server" Width="30px" TextWordNo="10" CssClass="BackGroundGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="VehicleDeliveryDate" runat="server" Width="160px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                    </li>
								    <li class="mainblockContentLeftCustomerCarDetail4">
                                        <%--最新走行距離--%>
                                        <icrop:CustomLabel ID="LatestMileageWord" runat="server" Width="30px" TextWordNo="11" CssClass="BackGroundGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <icrop:CustomLabel ID="LatestMileage" runat="server" Width="85px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>                                        
                                        <%--最新走行距離更新日文言--%>
                                        <icrop:CustomLabel ID="LatestMileageUpdateDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="170" ></icrop:CustomLabel>
                                        <%--最新走行距離更新日--%>
                                        <icrop:CustomLabel ID="LatestMileageUpdateDate" runat="server" Width="45px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                    </li>
							    </ul>
						    </div>
						    <div class="mainblockContentLeftCustomerCarStaff">
							    <ul>
								    <li class="mainblockContentLeftCustomerCarStaffSale">
                                        <%--セールス担当者名称--%>
                                        <icrop:CustomLabel ID="SalesStaffName" runat="server" Width="180px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                    </li>
								    <li class="mainblockContentLeftCustomerCarStaffMaintenance">
                                        <%--サービス担当者名称--%>
                                        <icrop:CustomLabel ID="ServiceStaffName" runat="server" Width="180px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                    </li>
							    </ul>
						    </div>
					    </div>
				    </div>
                    <!--ここまで保有車種-->
                    <!--ここから顧客関連情報-->
                    <div id="custDtlPage1" class="scNscOneBoxContentsWrap loding" style="top:400px;">
                        <uc1:SC3080214 ID="SC3080214Page" runat="server" />
                    </div>
    
                    <div id="CustomerMemoEdit" style="display: none">
                        <div id="custDtlPage4" class="scNscOneBoxContentsWrap">
                            <uc1:SC3080204 ID="Sc3080204Page" runat="server" />
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="birthdayTextBox" Value="2099/12/31" />
                    <!--ここまで最新顧客メモ-->
				</div>
			</div>
            <!--ここまで左カラム-->

            <!--ここから右カラム-->
			<div id="mainblockContentRight">
			    <h2>
                    <%--入庫履歴ヘッダー--%>
                    <icrop:CustomLabel ID="ServiceInHistoryHeader" runat="server" Width="200px" TextWordNo="3" CssClass="SC3080225Ellipsis SC3080225Ellipsis" ></icrop:CustomLabel>
                </h2>
                <div class="mainblockContentRightWrap">
                    <div class="mainblockContentRightTabWrap">
                        <div class="mainblockContentRightTabWrapScroll">
                            <asp:UpdatePanel ID="AjaxHistoryPanel" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Repeater ID="ServiceInHistoryRepeater" runat="server" EnableViewState="false">
                                        <ItemTemplate>
                                            <dl id="mainblockContentRightTabAll01" runat="server">
                                                <dt class="mainblockContentRightTabAll01-1">
                                                    <img alt="" src="../Styles/Images/SC3080225/ico32.png" width="15" height="16">
                                                </dt>
                                                <dd class="mainblockContentRightTabAll01-2">
                                                    <icrop:CustomLabel ID="ServiceInDate" runat="server" Width="93px" CssClass="SC3080225Ellipsis" style="display:block;" ></icrop:CustomLabel>
                                                    <icrop:CustomLabel ID="RepairOrderNo" runat="server" Width="93px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                                </dd>
                                                <dd class="mainblockContentRightTabAll01-6">
                                                    <icrop:CustomLabel ID="MaintenanceType" runat="server" Width="70px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                                </dd>
                                                <dd class="mainblockContentRightTabAll01-7">
                                                    <icrop:CustomLabel ID="ServiceName" runat="server" Width="81px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                                </dd>
                                                <dd class="mainblockContentRightTabAll01-5">
                                                    <icrop:CustomLabel ID="StaffName" runat="server" Width="85px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                                </dd>
                                            </dl>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div id="AllDispLinkDiv" runat="server" style="display:none;" >
                                        <icrop:CustomLabel ID="ShowAllWord" runat="server" Width="420px" TextWordNo="16" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <asp:LinkButton runat="server" ID="AllDispLink" OnClientClick="clickAllLink();" />
                                    </div>
                                    <div id="NextDispLinkDiv" runat="server" style="display:none;" >
                                        <icrop:CustomLabel ID="NextDispLinkDivLabel" runat="server" Width="420px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                        <asp:LinkButton runat="server" ID="NextDispLink" OnClientClick="clickNextLink();" />
                                    </div>
                                    <div id="NextLodingDiv" runat="server" style="display:none;" >
                                        <div id="LoadImage"></div>
                                        <icrop:CustomLabel ID="NextLoadingDivLabel" runat="server" Width="200px" CssClass="SC3080225Ellipsis" />
                                    </div>
                                    <div style="padding-bottom:20px;"></div>
                                    <asp:HiddenField ID="HiddenFieldOtherHistoryDispCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="HiddenFieldDisplayCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="HiddenFieldCstId" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldDealerCode" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldStoreCode" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldOrderNumber" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldServiceInNumber" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldServiceInVin" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldServiceInRegisterNumber" runat="server" Value="" />
                                    <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START --%>
                                    <asp:HiddenField ID="HiddenFieldSscFlag" runat="server" Value="" />
                                    <%-- 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END --%>
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START --%>
                                    <asp:HiddenField ID="HiddenFieldImpFlg" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldSmlAmcFlg" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldEwFlg" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenFieldTlmMbrFlg" runat="server" Value="" />
                                    <%--2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END --%>
                                    <asp:Button ID="ServiceInEventButton" runat="server" style="display:none;" />
                                    <asp:Button ID="ServiceInResetButton" runat="server" style="display:none;" />
                                    <asp:Button ID="AllDispLinkButton" runat="server" style="display:none;" />
                                    <asp:Button ID="NextDispLinkButton" runat="server" style="display:none;" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="AllDispLink" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="NextDispLink" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    </div>
				</div>
                <!--ここまで右カラム-->
			</div>
		</div>
        <!-- ここまでメインブロック -->
				
        <!-- ここからポップアップ用 -->
        <!-- ここから顧客情報用ポップアップ -->
		<div class="popBase popWindowSizeW503 popWindowSizeH615 ArrowCoordinate07">
		    <div id="CustomerInfo" class="popWindowBase popWindowCoordinate0a07">
                <div class="Balloon">
		            <div class="borderBox">
                        <div class="Arrow">&nbsp;</div>
		                <div class="myDataBox">&nbsp;</div>
	                </div>
		            <div class="gradationBox">
                        <div class="ArrowMask">
                            <div class="Arrow">&nbsp;</div>
                        </div>
		                <div class="scNscPopUpHeaderBg">&nbsp;</div>
		                <div class="scNscPopUpDataBg">&nbsp;</div>
	                </div>
	            </div>
		        <div class="PopUpHeader">
		            <h3>
                        <%--タイトル--%>
                        <icrop:CustomLabel ID="CstPopHeaderTitle" runat="server" Width="315px" CssClass="SC3080225Ellipsis" TextWordNo="18" ></icrop:CustomLabel>
                    </h3>
		            <div id ="CustomerLeftBtn" class="LeftBtn">
                        <%--閉じる--%>
                        <icrop:CustomLabel ID="CstPopHeaderCloseWord" runat="server" Width="50px" TextWordNo="19" CssClass="SC3080225EllipsisNotToolChip SC3080225Ellipsis" ></icrop:CustomLabel>
                    </div>
                </div>
		        <div class="dataBox">
		            <div class="innerDataBox">
		                <!-- Window内部 -->
                        <div class="InnerMyBox">
                            <!-- １個目のブロック -->
                            <dl class="PoPuPS-CM-05Block1">
                                <%--ファーストネーム--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopFirstNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="20" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopFirstName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--ミドルネーム--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopMiddleNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="21" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopMiddleName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--ラストネーム--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopLastNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="22" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopLastName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>                                    
                                </dd>  
                                <%--敬称--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopNameTitleWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="23" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopNameTitle" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                            <!-- ２個目のブロック -->
                            <dl class="PoPuPS-CM-05Block2">
                                <%--性別--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopSexWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="24" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2" id="CstPopMaleWordArea" runat="server">
                                    <icrop:CustomLabel ID="CstPopMaleWord" runat="server" Width="150px" CssClass="SC3080225Ellipsis" TextWordNo="25" ></icrop:CustomLabel>
                                </dd>
                                <dd class="PoPuPS-CM-05Block1-2" id="CstPopFemaleWordArea" runat="server">
                                    <icrop:CustomLabel ID="CstPopFemaleWord" runat="server" Width="150px" CssClass="SC3080225Ellipsis" TextWordNo="26" ></icrop:CustomLabel>
                                </dd>
                                <%--顧客タイプ--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopPrivateCorporationWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="27" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2" id="CstPopPrivateWordArea" runat="server">
                                    <icrop:CustomLabel ID="CstPopPrivateWord" runat="server" Width="150px" CssClass="SC3080225Ellipsis" TextWordNo="28" ></icrop:CustomLabel>
                                </dd>
                                <dd class="PoPuPS-CM-05Block2-2" id="CstPopCorporationWordArea" runat="server">
                                    <icrop:CustomLabel ID="CstPopCorporationWord" runat="server" Width="150px" CssClass="SC3080225Ellipsis" TextWordNo="29" ></icrop:CustomLabel>
                                </dd>
                                <%--サブ顧客タイプ--%>
                                <dt class="PoPuPS-CM-05Block2-1" style="border-bottom: #BBB 1px solid; border-bottom-left-radius: 6px;">
                                    <icrop:CustomLabel ID="CstPopSubCustomerTypeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="33" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2 wLong" style="border-bottom: #BBB 1px solid; border-bottom-right-radius: 6px;">
                                    <icrop:CustomLabel ID="CstPopSubCustomerType" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                            </dl>
                            <!-- ３個目のブロック -->
                            <dl class="PoPuPS-CM-05Block3">
                                <%--携帯電話番号--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopMobileWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="34" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopMobile" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--自宅電話番号--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopHomeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="35" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopHome" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--自宅FAX番号--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopFaxWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="36" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopFax" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--勤務先電話番号--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopOfficeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="37" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopOffice" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                            <!-- 4個目のブロック -->
                            <dl class="PoPuPS-CM-05Block4">
                                <%--電子メールアドレス1--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopEmail1Word" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="38" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopEmail1" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--電子メールアドレス2--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopEmail2Word" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="39" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopEmail2" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                            </dl>
                            <!-- 5個目のブロック -->
                            <dl class="PoPuPS-CM-05Block4">
                                <%--郵便番号--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopZipCodeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="40" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopZipCode" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--住所1--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopAddress1Word" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="41" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopAddress1" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--住所2--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopAddress2Word" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="42" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopAddress2" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--住所3--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopAddress3Word" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="43" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopAddress3" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                            <!-- 6個目のブロック -->
                            <dl class="PoPuPS-CM-05Block4">
                                <%--国籍--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopNationalityWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="44" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopNationality" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--本籍--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopDomicileWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="45" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopDomicile" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--生年月日--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopBirthDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="46" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopBirthDate" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                      
                            <!-- 7個目のブロック -->
                            <dl class="PoPuPS-CM-05Block4">
                                <%--会社名称--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopCompanyNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="47" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopCompanyName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--担当者氏名（法人）--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopEmployeeNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="48" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopEmployeeName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--担当者部署名（法人）--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopDepartmentWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="49" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopDepartment" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>  
                                <%--役職（法人）--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopOfficialPositionWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="50" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopOfficialPosition" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                      
                            <!-- 8個目のブロック -->
                            <dl class="PoPuPS-CM-05Block4">
                                <%--基幹顧客ID--%>
                                <dt class="PoPuPS-CM-05Block1-1">
                                    <icrop:CustomLabel ID="CstPopDmsCustomerCodeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="51" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block1-2">
                                    <icrop:CustomLabel ID="CstPopDmsCustomerCode" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--国民番号--%>
                                <dt class="PoPuPS-CM-05Block2-1">
                                    <icrop:CustomLabel ID="CstPopSocialIdWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="52" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block2-2">
                                    <icrop:CustomLabel ID="CstPopSocialId" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>
                                <%--未取引ユーザーID--%>
                                <dt class="PoPuPS-CM-05Block3-1">
                                    <icrop:CustomLabel ID="CstPopNewCustomerIdWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="53" ></icrop:CustomLabel>
                                </dt>
                                <dd class="PoPuPS-CM-05Block3-2">
                                    <icrop:CustomLabel ID="CstPopNewCustomerId" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                                </dd>                                                 
                            </dl>
                        </div>
                        <!-- /Window内部 -->
                        <div style="padding-bottom:5px;"></div>
		            </div>
		            <div class="OverShadow">&nbsp;</div>
	            </div>
	        </div>
        </div>
        <!-- ここまで顧客情報用ポップアップ -->

        <!-- ここから車両情報用ポップアップ -->
        <div class="popBase popWindowSizeW503 popWindowSizeH615 ArrowCoordinate08">
            <div id = "VehicleInfo" class="popWindowBase popWindowCoordinate0a07">
                <div class="Balloon">
                    <div class="borderBox">
                        <div class="Arrow">&nbsp;</div>
                        <div class="myDataBox">&nbsp;</div>
                    </div>
                    <div class="gradationBox">
                        <div class="ArrowMask"><div class="Arrow">&nbsp;</div>
                    </div>
                    <div class="scNscPopUpHeaderBg">&nbsp;</div>
                    <div class="scNscPopUpDataBg">&nbsp;</div>
                </div>
            </div>
            <div class="PopUpHeader">
                <h3>
                    <icrop:CustomLabel ID="VclPopHeaderTitle" runat="server" Width="315px" CssClass="SC3080225Ellipsis" TextWordNo="54" ></icrop:CustomLabel>
                </h3>
                <div id="VehicleLeftBtn" class="LeftBtn">
                    <icrop:CustomLabel ID="VclPopHeaderCloseWord" runat="server" Width="50px" TextWordNo="19" CssClass="SC3080225EllipsisNotToolChip" ></icrop:CustomLabel>
                </div>
            </div>
            <div class="dataBox">
                <div class="innerDataBox">
                    <!-- Window内部 -->
                    <div class="InnerMyBox">

                        <!-- 1個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--メーカー名--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopMakerNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="55" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">                                
                                <icrop:CustomLabel ID="VclPopMakerName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>                                    
                            </dd>
                            <%--モデルコード--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopModelNameWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="56" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block3-2">
                                <icrop:CustomLabel ID="VclPopModelName" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                        </dl>

                        <!-- 2個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--車両登録No--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopRegNoWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="57" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">
                                <icrop:CustomLabel ID="VclPopRegNo" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--車両登録エリア名称--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopProvinceWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="58" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopProvince" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--VIN--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopVinWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="59" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block3-2">
                                <icrop:CustomLabel ID="VclPopVin" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>                                                 
                        </dl>

                        <!-- 3個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--基本型式--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopKatashikiWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="60" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">
                                <icrop:CustomLabel ID="VclPopKatashiki" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--燃料--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopFuelWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="61" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopFuel" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--外板色名称--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopBodyColorWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="62" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopBodyColor" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--エンジンNo--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopEngineNoWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="63" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopEngineNo" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--トランスミッション--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopTransmissionWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="64" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block3-2">
                                <icrop:CustomLabel ID="VclPopTransmission" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>                                                  
                        </dl>

                        <!-- 4個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--登録日--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopRegDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="65" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">
                                <icrop:CustomLabel ID="VclPopRegDate" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--納車日--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopDeliDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="66" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block3-2">
                                <icrop:CustomLabel ID="VclPopDeliDate" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                        </dl>

                        <!-- 5個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--車両区分--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopVehicleTypeWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="67" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">
                                <icrop:CustomLabel ID="VclPopVehicleType" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--最終整備完了日--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopServiceCompletedDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="68" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopServiceCompletedDate" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--最新走行距離--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopMileageWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="69" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2" style="border-bottom: #BBB 1px solid; border-bottom-right-radius: 6px;">
                                <icrop:CustomLabel ID="VclPopMileage" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>                        
                        </dl>

                        <!-- 7個目のブロック -->
                        <dl class="PoPuPS-CM-05Block4">
                            <%--保険会社名--%>
                            <dt class="PoPuPS-CM-05Block1-1">
                                <icrop:CustomLabel ID="VclPopInsuranceCompanyWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="71" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block1-2">
                                <icrop:CustomLabel ID="VclPopInsuranceCompany" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--保険証券番号--%>
                            <dt class="PoPuPS-CM-05Block2-1">
                                <icrop:CustomLabel ID="VclPopInsurancePolicyNoWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="72" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block2-2">
                                <icrop:CustomLabel ID="VclPopInsurancePolicyNo" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>
                            <%--保険満期日--%>
                            <dt class="PoPuPS-CM-05Block3-1">
                                <icrop:CustomLabel ID="VclPopInsuranceExpiryDateWord" runat="server" Width="65px" CssClass="SC3080225Ellipsis" TextWordNo="73" ></icrop:CustomLabel>
                            </dt>
                            <dd class="PoPuPS-CM-05Block3-2">
                                <icrop:CustomLabel ID="VclPopInsuranceExpiryDate" runat="server" Width="365px" CssClass="SC3080225Ellipsis" ></icrop:CustomLabel>
                            </dd>                                                 
                        </dl>                            
                    </div>
                    <!-- /Window内部 -->
                    <div style="padding-bottom:5px;"></div>
                </div>
                <div class="OverShadow">&nbsp;</div>
            </div>
        </div>
    </div>
    <!-- ここまで車両情報用ポップアップ -->
    
    <%--ここから車両選択用ポップアップ--%>    		
    <div id="VclSelectPop" style="display:none;">
        <div class="PoPuPS-CM-07ContentInBlock">
            <div class="PoPuPS-CM-07ContentBody">
	                <div class="PoPuPS-CM-07ContentBodyWrap1">
                    <%'2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START%>
                    <%--<asp:Repeater ID="VehicleSelectListRepeater" runat="server" EnableViewState="false" >
                        <ItemTemplate>
					        <div runat="server" id="VclSelPopRecord" class="PoPuPS-CM-07Block1">
					            <div class="mainblockContentLeftCustomerCarName">
						            <dl>
							            <dt id="VclSelPopLogoArea" runat="server">--%>                                
                                            <%--ロゴ--%>
                                            <%--<img runat="server" id="VclSelPopVehicleLogoIcon" alt="" src="../Styles/Images/SC3080225/car_logo01.png" width="185" height="30"  />
                                    
								            <table runat="server" id="VclSelPopVehicleMakerModelTable" border="0" class="NoBorderTable" style="display:none;">
								                <tr>
								                    <td width="63px" height="24px">
                                                        <icrop:CustomLabel ID="VclSelPopMakerName" runat="server" Width="63px" Text='<%# HttpUtility.HtmlEncode(Eval("MakerCode")) %>' CssClass="SC3080225Ellipsis GrayWord2" ></icrop:CustomLabel>
                                                    </td>
								                    <td class="CarTypeBoldText">
                                                        <icrop:CustomLabel ID="VclSelPopModelName" runat="server" Width="140px" Text='<%# HttpUtility.HtmlEncode(Eval("SERIESCD")) %>' CssClass="SC3080225Ellipsis BlackWord" ></icrop:CustomLabel>
                                                    </td>
							                    </tr>
							                </table>
                                        </dt>
                                        <dd id="VclSelPopGradeArea" class="mainblockContentLeftCustomerEdition">--%>                                    
                                            <%--グレード--%>
                                            <%--<icrop:CustomLabel ID="VclSelPopGrade" runat="server" Width="185px" Text='<%# HttpUtility.HtmlEncode(Eval("Grade")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>
                                        </dd>
							            <dd id="VclSelPopBodyColorArea" class="mainblockContentLeftCustomerColor">--%>                                       
                                            <%--外装色--%>
                                            <%--<icrop:CustomLabel ID="VclSelPopBodyColor" runat="server" Width="185px" Text='<%# HttpUtility.HtmlEncode(Eval("BodyColorName")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>
                                        </dd>							            
						            </dl>
						            <div class="mainblockContentLeftCustomerCarDetail">
							            <ul>
								            <li class="mainblockContentLeftCustomerCarDetail1">--%>
                                                <%--車両登録番号--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopRegNoWord" runat="server" Width="30px" Height="14px" TextWordNo="8" CssClass="PopBGGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                                <icrop:CustomLabel ID="VclSelPopRegNo" runat="server" Width="160px" Height="14px" Text='<%# HttpUtility.HtmlEncode(Eval("VehicleRegistrationNumber")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>
                                            </li>
								            <li class="mainblockContentLeftCustomerCarDetail2">--%>
                                                <%--VIN--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopVinWord" runat="server" Width="30px" Height="14px" TextWordNo="9" CssClass="PopBGGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                                <icrop:CustomLabel ID="VclSelPopVin" runat="server" Width="160px" Height="14px" Text='<%# HttpUtility.HtmlEncode(Eval("Vin")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>
                                            </li>
								            <li class="mainblockContentLeftCustomerCarDetail3">--%>
                                                <%--納車日--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopDeliveryDateWord" runat="server" Width="30px" Height="14px" TextWordNo="10" CssClass="PopBGGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                                <icrop:CustomLabel ID="VclSelPopDeliveryDate" runat="server" Width="160px" Height="14px" Text='<%# HttpUtility.HtmlEncode(Eval("VehicleDeliveryDate")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>
                                            </li>
								            <li class="mainblockContentLeftCustomerCarDetail4">--%>
                                                <%--最新走行距離--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopLatestMileageWord" runat="server" Width="30px" Height="14px" TextWordNo="11" CssClass="PopBGGrayWordLabel SC3080225Ellipsis" ></icrop:CustomLabel>
                                                <icrop:CustomLabel ID="VclSelPopLatestMileage" runat="server" Width="85px" Height="14px" Text='<%# HttpUtility.HtmlEncode(Eval("Mileage")) %>' CssClass="SC3080225Ellipsis GrayWord1" ></icrop:CustomLabel>--%>                                        
                                                <%--最新走行距離更新日文言--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopLatestMileageUpdateDateWord" runat="server" Width="65px" Height="14px" TextWordNo="170" CssClass="SC3080225Ellipsis GrayWord2" ></icrop:CustomLabel>--%>
                                                <%--最新走行距離更新日--%>
                                                <%--<icrop:CustomLabel ID="VclSelPopLatestMileageUpdateDate" runat="server" Width="45px" Text='<%# HttpUtility.HtmlEncode(Eval("LastUpdateDate")) %>' CssClass="SC3080225Ellipsis GrayWord2" ></icrop:CustomLabel>
                                            </li>
							            </ul>
						            </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>--%>
                    <%'2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END%>
                    </div>
                </div>
		    <div class="PoPuPArrowLeftS-CM-07-1">
			    <div class="PoPuPArrowLeftS-CM-07-2">
				    <div class="PoPuPArrowLeftS-CM-07-3"></div>
			    </div>
		    </div>
        </div>
    </div>
	<%--ここまで車両選択用ポップアップ--%>

    <!-- ここまでポップアップ用 -->


    </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="MainPageReloadButton" EventName="Click" />
                                </Triggers>
    </asp:UpdatePanel>
</asp:Content>