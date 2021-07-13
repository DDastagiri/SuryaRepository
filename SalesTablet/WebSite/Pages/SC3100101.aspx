
<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3100101.aspx.vb" Inherits="PagesSC3100101" %>

<%@ Register src="~/Pages/SC3110101.ascx" tagname="SC3110101" tagprefix="uc1" %>
<%@ Register src="~/Pages/SC3100103.ascx" tagname="SC3100103" tagprefix="uc1" %>
<%@ Register src="~/Pages/SC3100104.ascx" tagname="SC3100104" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!--
    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    'SC3100101.aspx
    '─────────────────────────────────────
    '機能： 受付メイン
    '補足： 
    '作成： 2011/12/12 KN t.mizumoto
    '更新： 2012/08/17 TMEJ m.okamura 新車受付機能改善 $01
    '更新： 2013/01/10 TMEJ m.asano   新車タブレットショールーム管理機能開発 $02
    '更新： 2013/02/27 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $03
    '更新： ----/--/-- TMEJ ----- 新車受付係りによる来店チップ作成機能開発 $04
    '更新： 2014/03/10 TMEJ y.nakamura 受注後フォロー機能開発 $05
    '更新： 2020/02/06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $06
    '更新： 2020/03/12 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $07
    '─────────────────────────────────────
    -->
    <%' $01 start 新車受付機能改善 %>
    <link rel="Stylesheet" href="../Styles/SC3100101/SC3100101.css?20200416000000" type="text/css" media="screen" />
    <script type="text/javascript" src="../Scripts/SC3100101/Common.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100101/SC3100101.js?20200318000000"></script>
    <%' $01 end   新車受付機能改善 %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <%--読み込み中ウィンドウ--%>
    <div class="MstPG_LoadingScreen">
        <div class="MstPG_LoadingWrap">
            <div class="loadingIcn">
                <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
            </div>
        </div>
    </div>

    <%-- ======================== 画面共通エリア ======================== --%>
    <%-- ロック解除秒数 --%>
    <asp:HiddenField id="LockResetInterval" runat="server"></asp:HiddenField>
    <%--来店時間警告秒数--%>
    <asp:HiddenField id="VisitTimeAlertSpan" runat="server"></asp:HiddenField>
    <%--待ち時間警告秒数--%>
    <asp:HiddenField id="WaitTimeAlertSpan" runat="server"></asp:HiddenField>

    <%' $02 start 新車タブレットショールーム管理機能開発 %>
    <%--接客不要警告秒数(第１段階)--%>
    <asp:HiddenField id="UnNecessaryFirstTimeAlertSpan" runat="server"></asp:HiddenField>
    <%--接客不要警告秒数(第２段階)--%>
    <asp:HiddenField id="UnNecessarySecondTimeAlertSpan" runat="server"></asp:HiddenField>
    <%--商談中断警告秒数--%>
    <asp:HiddenField id="StopTimeAlertSpan" runat="server"></asp:HiddenField>
    <%' $02 end   新車タブレットショールーム管理機能開発 %>

    <%--査定警告秒数 --%>
    <asp:HiddenField id="AssessmentAlertSpan" runat="server"></asp:HiddenField>
    <%--価格相談警告秒数--%>
    <asp:HiddenField id="PriceAlertSpan" runat="server"></asp:HiddenField>
    <%--ヘルプ警告秒数--%>
    <asp:HiddenField id="HelpAlertSpan" runat="server"></asp:HiddenField>

    <%' $06 start TKM Change request development for Next Gen e-CRB (CR075) %>
    <%--定期リフレッシュ秒数--%>
    <asp:HiddenField id="RefreshInterval" runat="server"></asp:HiddenField>
    <%' $06 end TKM Change request development for Next Gen e-CRB (CR075) %>

    <%--エラーメッセージ--%>
    <asp:HiddenField id="ErrorMessage" runat="server"></asp:HiddenField>
    <%--エラーメッセージ送信用ボタン--%>
    <asp:Button ID="SendErrorMessageButton" runat="server" style="display:none;"></asp:Button>
    <%--非同期読み込みのためのScriptManagerタグ--%>
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <%--現在日付--%>
    <asp:HiddenField id="NowDateString" runat="server"></asp:HiddenField>
    
    <%--警告音出力フラグ--%>
    <asp:HiddenField id="AlarmOutputStatus" runat="server" value="1"></asp:HiddenField>
    <%--操作ステータス--%>
    <asp:HiddenField id="OperationStatus" runat="server"></asp:HiddenField>
    <%--処理停止フラグ--%>
    <asp:HiddenField id="LogicStopStatus" runat="server"></asp:HiddenField>

    <%-- $07 start TKM Change request development for Next Gen e-CRB (CR060) --%>
    <asp:HiddenField id="TentativeNameCharacterType" runat="server"></asp:HiddenField>
    <asp:HiddenField id="TelNumberCharacterType" runat="server"></asp:HiddenField>
    <%-- $07 end TKM Change request development for Next Gen e-CRB (CR060) --%>
    <%-- ======================== デバッグ用エリア ======================== --%>
    <div id="DebugArea" runat="server" style="position:absolute; padding:5px 5px 0px 5px; top:0px; left:0px; width:87%; height:75px; z-index:100; font-size:11px; background:rgba(255, 255, 255, 0.75);" Visible="false">
        <div style="margin:0px 10px 0px 10px; float:left;">
            <div id="pageData"></div>
            <div id="frameData"></div>
        </div>
        <div style="margin:0px 5px 0px 5px; float:left">
            <div>ロード回数：<span id="LoadData">0</span>回</div>
            <div>ロード中：<span id="LoadFlag" style="display:none;">★</span></div>
        </div>
        <div style="margin:0px 5px 0px 5px; float:left">
            <div>非同期通信回数：<span id="PageMethodsCount">0</span>回</div>
            <div>非同期通信中：<span id="PageMethodsFlag" style="display:none;">★</span></div>
        </div>
        <div style="margin:0px 5px 0px 5px; float:left">
            <div>部分読込回数：<span id="PartialRenderingCount">0</span>回</div>
            <div>部分読込中：<span id="PartialRenderingFlag" style="display:none;">★</span></div>
        </div>
        <div style="margin:0px 5px 0px 5px; float:left">
            <div>警告チップ数（前）：<span id="BeforeAlertCount">0</span>回</div>
            <div>警告チップ数（後）：<span id="AfterAlertCount">0</span>回</div>
        </div>
        <div style="margin:36px 5px 0px 5px;">
            <input type="button" id="LockButton" class="likeButton" value="ロック" />
            <input type="button" id="ResetButton" class="likeButton" value="ロック解除" />
            <input type="button" id="ReloadRequestButton" class="likeButton" value="リロード要求" />
            <input type="button" id="ReloadForceButton" class="likeButton" value="強制リロード" />
            <input type="button" id="PushReserveButton" class="likeButton" value="Push受信" />
            <select id="functionNo">
                <option value="01">01</option>
                <option value="02">02</option>
                <option value="03">03</option>
                <option value="99">99</option>
            </select>
            <select id="logicNo">
                <option value="01">01</option>
                <option value="02">02</option>
                <option value="03">03</option>
                <option value="04">04</option>
                <option value="05">05</option>
                <option value="06">06</option>
                <option value="07">07</option>
                <option value="08">08</option>
            </select>
        </div>
    </div>

    <%-- ====================== お客様情報入力画面 ====================== --%>
    <div id="CustomerPopOver" class="popoverEx">
        <div class="triangle"></div>
        <div>
            <asp:UpdatePanel id="UpdateArea" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--ロードをキックするためのボタン--%>
                    <asp:Button ID="CustomerDialogDisplayButton" runat="server" style="display:none;"></asp:Button>
                    <asp:HiddenField id="CustomerDialogVisitSeq" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerDialogVisitStatus" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerDialogCustomerSegment" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerDialogSalesTableNoOld" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerDialogSalesTableNoNew" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerDialogVehicleRegistrationNo" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="CustomerPopoverErrorMessage" runat="server"></asp:HiddenField>
                    <%-- $07 start TKM Change request development for Next Gen e-CRB (CR060) --%>
                    <%-- <div class="popBase popWindowSizeW307 popWindowSizeH549 ArrowCoordinate01"> --%>
                    <div class="popBase popWindowSizeW307 popWindowSizeH610 ArrowCoordinate01">
                    <%-- $07 start TKM Change request development for Next Gen e-CRB (CR060) --%>
                        <div class="popWindowBase popWindowCoordinate01">
                            <div class="Balloon">
                                <div class="borderBox">
                                    <div class="myDataBox">&nbsp;</div>
                                </div>
                                <div class="gradationBox">
                                    <div class="scNscPopUpHeaderBg">&nbsp;</div>
                                    <div class="scNscPopUpDataBg">&nbsp;</div>
                                </div>
                            </div>
                            <div class="scNscPopUpContactVisitHeader">
                                <h3><asp:Literal ID="VisitDialogTitleLiteral" runat="server" /></h3>
                                
                                    <a href="#" id="scNscPopUpCancelButton" class="scNscPopUpCancelButton" ><asp:Literal ID="CustomerDialogCancelLiteral" runat="server" /></a>
                                
                                
                                    <a href="#" id="scNscPopUpCompleteButton" class="scNscPopUpCompleteButton"><asp:Literal ID="CustomerDialogCompleteLiteral" runat="server" /></a>
                                
                            </div>
                            
                            <%--読み込み中ウィンドウ表示のために追加--%>
                            <div id="CustomerPopOverBody">
                                <%--読み込み中ウィンドウ--%>
                                <div class="MstPG_LoadingScreen">
                                    <div class="MstPG_LoadingWrap">
                                        <div class="loadingIcn">
                                            <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
                                        </div>
                                    </div>
                                </div>
                                <div class="dataBox">
                                    
                                    <div class="scNscPopUpContactVisitSttl01 "><asp:Literal ID="CustomerNameInputLiteral" runat="server" /></div>
                                    <%--顧客名テキストボックス--%>
                                    <div class="scNscPopUpContactVisitBox01">
                                    <div class="scNscPopUpContactVisitBox01">

                                        <div id="CustomerNameTextBoxArea"  class="scNscPopUpContactVisitTextArea02" runat="server">
                                                <asp:Literal ID="CustomerNameTextBoxLiteral" runat="server" />
                                                <icrop:CustomTextBox ID="CustomerNameTextBox" type="text" runat="server" MaxLength="256" Height="21px"></icrop:CustomTextBox>
                                        </div>
                                    </div>
                                    </div>
                                    <%-- $07 start TKM Change request development for Next Gen e-CRB (CR060) --%>
                                    <%--電話番号テキストボックス--%>
                                    <div class="scNscPopUpContactVisitBox01">
                                    <div class="scNscPopUpContactVisitBox01">

                                        <div id="CustomerTelNumberTextBoxArea"  class="scNscPopUpContactVisitTextArea02" runat="server">
                                                <asp:Literal ID="CustomerTelNumberTextBoxLiteral" runat="server" />
                                                <icrop:CustomTextBox ID="CustomerTelNumberTextBox" type="text" runat="server" MaxLength="13" Height="21px"></icrop:CustomTextBox>
                                        </div>
                                    </div>
                                    </div>
                                    <%-- $07 end TKM Change request development for Next Gen e-CRB (CR060) --%>

                                    <%--スタンバイスタッフに送信ボタン--%>
                                    <div class="scNscPopUpContactVisitBox01">
                                        <div id="PopupContactVisitSubmitButtonOn" class="scNscPopUpContactVisitSubmitButtonOn" runat="server" visible="false">
                                            <asp:Literal ID="CustomerDialogBroadcastLiteral1" runat="server" />
                                        </div>
                                        <div id="PopupContactVisitSubmitButtonOff" class="scNscPopUpContactVisitSubmitButtonOff" runat="server" visible="true">
                                            <asp:Literal ID="CustomerDialogBroadcastLiteral2" runat="server" />
                                        </div>
                                    </div>
                                    
                                    <%-- $02 start 新車タブレットショールーム管理機能開発 --%>
                                    <%--接客不要ボタン--%>
                                    <div class="scNscPopUpContactVisitBox01">
                                        <div id="PopupUnNecessarySubmitButtonOn" class="scNscPopUpContactVisitSubmitButtonOn" runat="server" visible="false">
                                            <asp:Literal ID="CustomerDialogUnNecessaryLiteral1" runat="server" />
                                        </div>
                                        <div id="PopupUnNecessarySubmitButtonOff" class="scNscPopUpContactVisitSubmitButtonOff" runat="server" visible="true">
                                            <asp:Literal ID="CustomerDialogUnNecessaryLiteral2" runat="server" />
                                        </div>
                                    </div>
                                    <%-- $02 end   新車タブレットショールーム管理機能開発 --%>
                                    
                                    <div class="scNscPopUpContactVisitSttl02"><asp:Literal ID="CustomerDialogTableNoInputLiteral" runat="server" /></div>
                                    <div class="scNscPopUpContactVisitBox02">
                                        <ul class="scNscPopUpContactVisitNoButton">
                                            <asp:Repeater ID="SalesTableNoRepeater" runat="server" enableViewState="false">
                                                <ItemTemplate>
                                                    <asp:Literal ID="SalesTableAreaLeft" runat="server" text="<li>" visible="false"></asp:Literal>
                                                    <asp:Literal ID="SalesTableAreaCenter" runat="server" text="<li class='NoButtonCenters'>" visible="false"></asp:Literal>
                                                    <asp:Literal ID="SalesTableAreaRight" runat="server" text="<li class='NoButtonRight'>" visible="false"></asp:Literal>
                                                    <asp:Literal ID="SalesTableNoOff" runat="server" text="<div class='NoButtonOff'>" visible="false"></asp:Literal>
                                                    <asp:Literal ID="SalesTableNoSelected" runat="server" text="<div class='NoButtonSelected'>" visible="false"></asp:Literal>
                                                    <asp:Literal ID="SalesTableNoLiteral" runat="server"></asp:Literal>
                                                    <asp:HiddenField id="SelectSalesTableNo" runat="server" Value='<%# Server.HTMLEncode(Eval("SALESTABLENO").ToString()) %>'></asp:HiddenField>
                                                    <asp:Literal ID="SalesTableNoEnd" runat="server" text="</div>"></asp:Literal>
                                                    <asp:Literal ID="SalesTableAreaEnd" runat="server" text="</li>"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="clearboth">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--===== 商談中詳細画面 ========================--%>
	<div id="StaffDetailPopOver" class="popoverEx">
		<div class="triangle"></div>
		<div>
			<asp:UpdatePanel id="StaffDetailUpdateArea" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<%--ロードをキックするためのボタン--%>
					<asp:Button ID="StaffDetailDisplayButton" runat="server" style="display:none;"></asp:Button>
					<%-- 画面で保持しておく情報 --%>
					<asp:HiddenField id="StaffDetailDialogVisitSeq" runat="server"></asp:HiddenField>
          <%' $03 start 納車作業ステータス対応 %>
          <asp:HiddenField id="StaffDetailDialogVisitStatus" runat="server"></asp:HiddenField>
          <%' $03 end   納車作業ステータス対応 %>
					<asp:HiddenField id="StaffDetailDialogIndex" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailDialogCustomerSegment" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailDialogSalesTableNoOld" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailDialogSalesTableNoNew" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailPopoverErrorMessage" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailDialogSalesStartTime" runat="server"></asp:HiddenField>
					<asp:HiddenField id="StaffDetailDialogCustId" runat="server"></asp:HiddenField>
					<%-- 依頼通知送信日時時間リスト --%>
					<asp:HiddenField id="SendDateList" runat="server"></asp:HiddenField>

					<div class="scNscPopUpStaffDetail scNscPopUpContactSelect48">
						<div class="scNscPopUpStaffDetailWindownBox WindownBox48">
							<div class="scNscPopUpStaffDetailHeader">
								<%--ヘッダタイトル--%>
								<h3 id="StaffName" class="ellipsis">
									<asp:Literal ID="StaffDetailDialogTitleLiteral" runat="server" />
								</h3>
								<h3 id="TableNo" style="display:none;">
									<asp:Literal ID="StaffDetailDialogTableTitleLiteral" runat="server" />
								</h3>

								<%--キャンセルボタン--%>
								<a href="#" id="scNscPopUpStaffDetailCancelButton" class="scNscPopUpStaffDetailCancelButton">
									<asp:Literal ID="StaffDetailDialogCancelLiteral" runat="server" />
								</a>

								<%-- スタッフ名表示ボタン --%>
								<div id="scNscPopUpStaffDetailCustomerButton" class="nscGiveupReasonCancellButton" style="display:none;">
									<a href="#" class="ellipsis"><asp:Literal ID="StaffDetailDialogNameLiteral" runat="server" /></a>
									<span class="tgLeft"></span>
								</div>

								<%--登録ボタン--%>
								<a href="#" id="scNscPopUpStaffDetailCompleteButton" class="scNscPopUpStaffDetailCompleteButton" runat="server">
									<asp:Literal ID="StaffDetailDialogCompleteLiteral" runat="server" />
								</a>
							</div>

							<%--読み込み中ウィンドウ表示のために追加--%>
							<div id="StaffDetailPopOverBody">
								<%--読み込み中ウィンドウ--%>
								<div class="MstPG_LoadingScreen">
									<div class="MstPG_LoadingWrap">
										<div class="loadingIcn">
											<img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
										</div>
									</div>
								</div>

								<%-- 分割ページの為の設定 --%>
								<div id="StaffDetailPopOverFlickArea">
									<div id="StaffDetailPopOverSheet">
										<%-- 1ページ目 --%>
										<div class="StaffDetailPopOverPage">
											<div class="scNscPopUpStaffDetailListArea">
												<%-- 商談状況 --%>
												<div class="scNscPopUpStaffDetailSttl01">
													<asp:Literal ID="StaffDetailNegoLiteral" runat="server" /><asp:Literal ID="VisitCountLiteral" runat="server" /><asp:Literal ID="StaffDetailNowVisitLiteral" runat="server" /><div id="NegotiateTime" class="LeftBoxTime"></div>
												</div>
												<%-- 依頼リスト、顧客詳細、プロセスリストの表示 --%>
												<div class="scNscPopUpStaffDetailBox01">
													<div class="scNscPopUpStaffDetailScroll">
														<%-- 依頼リスト表示部分 --%>
														<asp:Repeater ID="NoticeListRepeater" runat="server" enableViewState="false">
															<HeaderTemplate>
																<ul class="ListSet01">
															</HeaderTemplate>
															<ItemTemplate>
							    									<li id="NoticeName" class="ellipsis" runat="server">
																	<asp:HiddenField id="NoticeReqctg" runat="server" Value='<%# Server.HTMLEncode(Eval("NOTICEREQCTG").ToString()) %>'></asp:HiddenField>
																	<asp:Literal ID="NoticeNameLiteral" runat="server" />
																	<p id="NoticeTime"></p>
																</li>
															</ItemTemplate>
															<FooterTemplate>
																</ul>
															</FooterTemplate>
														</asp:Repeater>
														<%-- 商談の詳細 --%>
														<ul class="ListSet02">
														<%-- 権限別リンク：マネージャーの場合は青色 --%>
															<li id="CustomerNameLink" runat="server" class="list1 ellipsis">
																<%--画面遷移でキックするためのボタン--%>
																<asp:Button ID="StaffDetailCustomerNameButton" runat="server" style="display:none;"></asp:Button>
																<asp:Literal ID="StaffDetailCustomerName" runat="server" />
																<%-- アイコンの表示 --%>
																<div class="listIcnSet">
																	<%-- 苦情アイコン --%>
																	<div id="ClaimIcon" runat="server" class="listIcn1"><asp:Literal ID="StaffDetailClaimIconLiteral" runat="server" /></div>
																</div>
															</li>
															<li class="list2">
																<%-- 来店人数 --%>
																<asp:Literal ID="VisitPersonNumLiteral" runat="server" /><asp:Literal ID="StaffDetailVisitPersonLiteral" runat="server" />
															</li>
															<%--  権限別リンク：マネージャーの場合は黒,テーブルNo表示,未設定の場合は「"-"」 --%>
															<li id="TableNoLink" runat="server" class="list3">
																<icrop:CustomLabel ID="StaffDetailTableNoLiteral" type="text" runat="server" /><icrop:CustomLabel ID="DisplayTableNo" type="text" runat="server" />
																<div id="arrowPicture" class="listIcnSet" runat="server">
																	<img src="<%=ResolveClientUrl("~/Styles/Images/SC3100101/StaffDetail/yazirushi.png")%>" width="9" height="13" alt="">
																</div>
															</li>
															<%-- 希望(もしくは成約)車種の表示 --%>
															<li class="list4  ellipsis"><asp:Literal ID="CarName" runat="server" /></li>
															<li class="list5  ellipsis"><asp:Literal ID="GradeName" runat="server" /></li>
														</ul>
														<%-- プロセス：ステータス --%>
                                                        <%-- 受注後フォロー機能開発 $05 --%>
                                                        <ul class="ListSet03" ID="ProcessList" runat="server">
                                                        <%-- 受注後フォロー機能開発 $05 --%>
															<li ID="StaffDetailProcess1" runat="server" ></li>
															<li ID="StaffDetailProcess2" runat="server" ></li>
															<li ID="StaffDetailProcess3" runat="server" ></li>
															<li ID="StaffDetailProcess4" runat="server" ></li>
															<li ID="StaffDetailStatus" runat="server" ></li>

                                                            <%-- 受注後フォロー機能開発 $05 --%>
                                                            <asp:Repeater ID="BookedAfterProcessRepeater" runat="server" enableViewState="false">
                                                                <ItemTemplate>
                                                                    <li ID="StaffDetailProcess" class="ellipsis" runat="server" ></li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <%-- 受注後フォロー機能開発 $05 --%>
														</ul>
													</div>
												</div>
											</div>
										</div>

										<%-- 2ページ目 --%>
										<div class="StaffDetailPopOverPage">
											<div class="scNscPopUpStaffDetailListArea2">
												<div class="scNscPopUpStaffDetailBox02">
													<ul class="scNscPopUpStaffDetailNoButton">
														<asp:Repeater ID="StaffDetailSalesTableNoRepeater" runat="server" enableViewState="false">
															<ItemTemplate>
																<asp:Literal ID="SalesTableAreaLeft" runat="server" text="<li>" visible="false"></asp:Literal>
																<asp:Literal ID="SalesTableAreaCenter" runat="server" text="<li class='NoButtonCenters'>" visible="false"></asp:Literal>
																<asp:Literal ID="SalesTableAreaRight" runat="server" text="<li class='NoButtonRight'>" visible="false"></asp:Literal>
																<asp:Literal ID="SalesTableNoOff" runat="server" text="<div class='NoButtonOff'>" visible="false"></asp:Literal>
																<asp:Literal ID="SalesTableNoSelected" runat="server" text="<div class='NoButtonSelected'>" visible="false"></asp:Literal>
																<asp:Literal ID="SalesTableNoLiteral" runat="server"></asp:Literal>
																<asp:HiddenField id="SelectSalesTableNo" runat="server" Value='<%# Server.HTMLEncode(Eval("SALESTABLENO").ToString()) %>'></asp:HiddenField>
																<asp:Literal ID="SalesTableNoEnd" runat="server" text="</div>"></asp:Literal>
																<asp:Literal ID="SalesTableAreaEnd" runat="server" text="</li>"></asp:Literal>
															</ItemTemplate>
														</asp:Repeater>
													</ul>
													<div class="clearboth">&nbsp;</div>
												</div>
											</div>
										</div>

									</div>
								</div>

							</div>
						</div>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</div>
    
    <%' $01 start 複数顧客に対する商談平行対応 %>
    <%--
    <%' ======================= 紐付け解除画面 ======================= %>
    <div id="LinkingCancelPopOver" class="popoverEx">
        <div class="triangle"></div>
        <div>
            <asp:UpdatePanel id="LinkingCancelUpdateArea" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%'ロードをキックするためのボタン%>
                    <asp:Button ID="LinkingCancelDialogDisplayButton" runat="server" style="display:none;"></asp:Button>
                    <asp:HiddenField id="LinkingCancelDialogAccount" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="LinkingCancelDialogLinkingCount" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="LinkingCancelDialogStaffStatus" runat="server"></asp:HiddenField>
                    <asp:HiddenField id="LinkingCancelPopoverErrorMessage" runat="server"></asp:HiddenField>
                    <%'来店時間リスト%>
                    <asp:HiddenField id="VisitTimeList" runat="server"></asp:HiddenField>

                    <div class="scNscPopUpLinkingCancel">
                        <div class="scNscPopUpLinkingCancelWindownBox">
                            <div class="scNscPopUpLinkingCancelHeader">
                                <h3><asp:Literal ID="LinkingCancelDialogTitleLiteral" runat="server" /></h3>
                                <%'キャンセルボタン%>
                                <a href="#" id="scNscPopUpLinkingCancelCancelButton" class="scNscPopUpCancelButton"><asp:Literal ID="LinkingCancelDialogCancelLiteral" runat="server" /></a>
                                <%'登録ボタン%>
                                <a href="#" id="scNscPopUpLinkingCancelCompleteButton" class="scNscPopUpCompleteButtonOff"><asp:Literal ID="LinkingCancelDialogCompleteLiteral" runat="server" /></a>
                            </div>

                            <%'読み込み中ウィンドウ表示のために追加%>
                            <div id="LinkingCancelPopOverBody">
                                <%'読み込み中ウィンドウ%>
                                <div class="MstPG_LoadingScreen">
                                    <div class="MstPG_LoadingWrap">
                                        <div class="loadingIcn">
                                            <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
                                        </div>
                                    </div>
                                </div>

                                <div class="scNscPopUpLinkingCancelListArea">
                                    <div class="scNscPopUpLinkingCancelBox01">
                                        <div class="scNscPopUpLinkingCancelTextArea02">
                                            <%'フリックスクロール設定用%>
                                            <div class="scNscPopUpLinkingCancelScroll">
                                                <ul class="ListSet01">
                                                    <%' 紐付けられている顧客情報の表示%>
                                                    <asp:Repeater ID="LinkingCancelCustomerRepeater" runat="server" enableViewState="false">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="LinkingCancelCustomerAreaTop" runat="server" Text="<li class='customerList list1 ellipsis'>" visible="false"></asp:Literal>
                                                            <asp:Literal ID="LinkingCancelCustomerAreaCenter" runat="server" Text="<li class='customerList list2 ellipsis'>" visible="false"></asp:Literal>
                                                            <asp:Literal ID="LinkingCancelCustomerAreaBottom" runat="server" Text="<li class='customerList list3 ellipsis'>" visible="false"></asp:Literal>
                                                            <asp:HiddenField id="LinkingCustomerVisitSeq" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSEQ").ToString()) %>'></asp:HiddenField>
                                                            <asp:HiddenField id="LinkingCustomerVisitStatus" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSTATUS").ToString()) %>'></asp:HiddenField>
                                                            <asp:Literal ID="LinkingCancelCustomerName" runat="server"></asp:Literal>
                                                            <p class="LinkingCancelVisitTime">&nbsp;</p>
                                                            <asp:Literal ID="LinkingCancelCustomerAreaEnd" runat="server" Text="</li>"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    --%>
    <%' $01 end   複数顧客に対する商談平行対応 %>
    
    <%-- ======================== メインエリア ======================== --%>
    <div id="frameArea">
        <%--読み込み中ウィンドウ--%>
        <div class="MstPG_LoadingScreen">
            <div class="MstPG_LoadingWrap">
                <div class="loadingIcn">
                    <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="38" height="38" alt="" />
                </div>
            </div>
        </div>
    </div>
    
    <%-- 試乗入力画面のユーザコントロール --%>
    <uc1:SC3110101 ID="SC3110101" runat="server" TriggerClientID="MstPG_FootItem_Sub_201" />
    <%' $01 start スタンバイスタッフ並び順変更対応 %>
    <uc1:SC3100103 ID="SC3100103" runat="server" TriggerClientID="MstPG_FootItem_Sub_1201" />
    <%' $01 end   スタンバイスタッフ並び順変更対応 %>
    <%' $04 start 新車受付係りによる来店チップ作成機能開発 %>
    <uc1:SC3100104 ID="SC3100104" runat="server" TriggerClientID="MstPG_FootItem_Sub_1202" />
    <%' $04 end   新車受付係りによる来店チップ作成機能開発 %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>
