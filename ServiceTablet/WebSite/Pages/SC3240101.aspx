<%@ Page Title="" Language="VB" EnableEventvalidation="False" MasterPageFile="~/Master/CommonMasterPage.master" AutoEventWireup="false" CodeFile="SC3240101.aspx.vb" Inherits="Pages_SC3240101" %>
<%@ Register src="SC3240201.ascx" TagName="SC3240201" TagPrefix="uc1" %>
<%@ Register Src="SC3240301.ascx" TagName="SC3240301" TagPrefix="uc2" %>
<%@ Register src="SC3240501.ascx" TagName="SC3240501" TagPrefix="uc3" %>
<%@ Register src="SC3240701.ascx" TagName="SC3240701" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━%>
<%'SC3240101.aspx                                                            %>
<%'─────────────────────────────────────%>
<%'機能： SMBメイン画面                                                      %>
<%'補足：                                                                    %>
<%'作成： 2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)               %>
<%'更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発      %>
<%'更新： 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発      %>
<%'更新： 2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発          %>
<%'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発              %>
<%'更新： 2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) %>
<%'更新： 2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い %>
<%'更新： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 %>
<%'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする %>
<%'更新： 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 %>
<%'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 %>
<%'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 %>
<%'─────────────────────────────────────────%>
    <link rel="Stylesheet" href="../Styles/SC3240101/common.css?20131009000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3240101/smb.css?20171004000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3240101/footer.css?20200221000000" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3240101/popbase.css?201310091040" type="text/css" media="screen,print" />
    <link rel="Stylesheet" href="../Styles/SC3240101/chips.css?2018070600000" type="text/css" media="screen,print" />
    <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
    <link rel="Stylesheet" href="../Styles/SC3240101/SC3240101.PullDownRefresh.css?201310091040" type="text/css" media="screen,print" />
    <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %>  

    <%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い START%>
    <!-- スクリプトファイルの参照は圧縮済ファイルを対象とする -->
    <script type="text/javascript" src="../Scripts/SC3240101/SmbMain.flickable.min.js?20170914000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/SmbMain.fingerScroll.min.js?20170914000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/jquery.ui.touch-punch.min.min.js?20170914000000"></script>        
    <script type="text/javascript" src="../Scripts/SC3240101/Define.min.js?20200221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/Common.min.js?20171019000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/ChipPrototype.min.js?2018070600000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/SC3240101.min.js?20190807000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/Table.min.js?20200221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/Chip.min.js?20200221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/Footer.min.js?20200221000000"></script>
    <script type="text/javascript" src="../Scripts/SC3240101/SC3240101.StopmemoFingerscroll.min.js?20170911000000"></script>
    <%'2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い END%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <!-- ここからメインブロック -->
    <div id="MainArea">
        <div class="Timing_Balloon"><div class="Time">13:05</div></div>
        <div id="MainAreaBackGroundColor" class="SMBBackGroundZIndex2"></div>
        <div id="MainAreaBackGroundLogo" class="SMBBackGroundZIndex1"></div>
        <div id="MainAreaActiveIndicator" class="show"></div>
        <div id="Inner">
            <div class="SMB_MainOutFrame">
                <div class="SMB_Main">
                    <div class="SMBLogo">
                        <div class="Date">
                            <p id="pCalendar" runat="server"></p>
                            <icrop:DateTimeSelector ID="dtCalendar" runat="server" Format="Date" onblur="ChangeDateValue(1);" onchange="ChangeDateValue(0);"  onkeyup="ChangeDateValue(0);" />
                        </div>
                        <div class="LeftButton_trimming" onclick="imgbtnPrevDate_onClick();">
                            <input type="image"  src="../Styles/Images/SC3240101/arrowLeft.png" onclick="imgbtnDate_onClick(); return false;"/>
                        </div>
                        <div class="RightButton_trimming" onclick="imgbtnNextDate_onClick();">
                            <input type="image"  src="../Styles/Images/SC3240101/arrowRight.png" onclick="imgbtnDate_onClick(); return false;"/>
                        </div>
                     </div>
                    <div class="NameList">
                        <div id="divScrollStall">
                            <ul id="ulStall">
                                <asp:Repeater ID="stallNameRepeater" runat="server" EnableViewState="false" ClientIDMode="Static">
                                    <ItemTemplate>
                                        <li id='stallId_<%# HttpUtility.HtmlEncode(Eval("STALLID")) %>' class="stallNo<%# HttpUtility.HtmlEncode(Eval("STALLNO")) %>">
                                            <div class="Stole" onclick="ClickStall(<%# HttpUtility.HtmlEncode(Eval("STALLID")) %>)">
                                                <p><icrop:CustomLabel ID="lblStallName" runat="server"  Text='<%# HttpUtility.HtmlEncode(Eval("STALLNAME")) %>'></icrop:CustomLabel></p>
                                            </div>
                                            <div class="Technician">
                                                <span id='spanTechnician<%# HttpUtility.HtmlEncode(Eval("STALLNO")) %>_1' class="TechnicianName"></span>
                                                <span id='spanTechnician<%# HttpUtility.HtmlEncode(Eval("STALLNO")) %>_2' class="TechnicianName"></span>
                                                <span id='spanTechnician<%# HttpUtility.HtmlEncode(Eval("STALLNO")) %>_3' class="TechnicianName"></span>
                                                <span id='spanTechnician<%# HttpUtility.HtmlEncode(Eval("STALLNO")) %>_4' class="TechnicianName"></span>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                    <div class="TimeLine">
                        <div class="TimeLine_Time" id="divScrollTime">
                            <asp:Repeater ID="stallTimeRepeater" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <div class="TimeCassette">
                                        <icrop:CustomLabel ID="lblTime" runat="server" style="line-height:33px;"  Text='<%# HttpUtility.HtmlEncode(Eval("COL1")) %>'></icrop:CustomLabel>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="ChipArea_OutFrame">
                        <div class="ChipArea_trimming">
                        	<%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
                            <div id="PullDownToRefreshDiv" class="PullDownToRefreshDiv"></div>
                            <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %>  
                            <div class="ChipArea">
                                <div class="ChipAreaBkGround">
                                    <div id="BlueDiv" class="TapBlueBack"></div>
                                </div>
                                <ul id="ulChipAreaBack_lineBox">
                                </ul>
                            </div>
                            <div class="TimingLineSet">
                                <div class="TimingLine01"></div>
                	            <div class="TimingLine02"></div>
                	            <div class="TimingLine03"></div>
                            </div>
                            <div class="TimingLineDeli">
                                <div class="TimingLine01"></div>
                	            <div class="TimingLine02"></div>
                	            <div class="TimingLine03"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hidShowDate" runat="server"/>
        <asp:HiddenField ID="hidMaxRow" runat="server"/>
        <asp:HiddenField ID="hidStallStartTime" runat="server"/>
        <asp:HiddenField ID="hidStallEndTime" runat="server"/>
        <asp:HiddenField ID="hidIntervlTime" runat="server"/>
        <asp:HiddenField ID="hidJsonData" runat="server"/>
        <asp:HiddenField ID="hidMsgData" runat="server"/>
        <asp:HiddenField ID="hidServerTime" runat="server"/>
        <asp:HiddenField ID="hidDlrCD" runat="server"/>
        <asp:HiddenField ID="hidBrnCD" runat="server"/>
        <asp:HiddenField ID="hidAccount" runat="server"/> 
        <asp:HiddenField ID="hidSelectedChipId" runat="server"/>
        <asp:HiddenField ID="hidStandardWashTime" runat="server"/>
        <asp:HiddenField ID="hidStandardDeliPreTime" runat="server"/>
        <asp:HiddenField ID="hidStandardDeliWrTime" runat="server"/>
        <%'2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START%>
        <asp:HiddenField ID="hidStandardInspectionTime" runat="server"/>
        <%'2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END%>
        <asp:HiddenField ID="hidTabletSmbRefreshInterval" runat="server"/>
        <asp:HiddenField ID="hidSessionValue" runat="server"/>
        <asp:HiddenField ID="hidOpeCode" runat="server"/>
        <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <asp:HiddenField ID="hidDateFormatMMdd" runat="server"/>
        <asp:HiddenField ID="hidDateFormatHHmm" runat="server"/>
        <asp:HiddenField ID="hidDateFormatYYYYMMddHHmm" runat="server"/>
        <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %> 
        <%'2019/12/06 NSK皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START %> 
        <asp:HiddenField ID="hidRestAutoJudgeFlg" runat="server"/>
        <%'2019/12/06 NSK皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END %> 
        <%--休憩オプション画面--%>
        <div class="popRestWindow">
            <div class="Balloon">
                <div class="borderBox">
                    <div class="Arrow">&nbsp;</div>
                    <div class="myDataBox">&nbsp;</div>
                </div>
                <div class="gradationBox">
                    <div class="ArrowMaskUpper">
                        <div class="ArrowUpper">&nbsp;</div>
                    </div>
                    <div class="ArrowMaskBelow">
                        <div class="ArrowBelow">&nbsp;</div>
                    </div>
                    <div class="scRestPopUpHeaderBg">&nbsp;</div>
                    <div class="scRestPopUpDataBg">&nbsp;</div>
                </div>
            </div>
            <div class="PopUpHeader">
                <icrop:CustomLabel ID="lblRestTitle" runat="server" TextWordNo="14" Width="156px" UseEllipsis="True"></icrop:CustomLabel>
                <div class="LeftBtn" onclick="CancelRestWindow()">
                    <icrop:CustomLabel ID="lblCancelBtn" runat="server" TextWordNo="15" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
                <div class="RightBtn" onclick="ConfirmRestWindow()">
                    <icrop:CustomLabel ID="lblLoginBtn" runat="server" TextWordNo="16" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
            </div>
            <div class="dataBox">
                <div class="innerDataBox">
                    <ul class="DataListTable NoMargin">
                        <li class="Check" onclick="SelectRestArea(0)">
                            <icrop:CustomLabel ID="lblTakeBreak" runat="server" TextWordNo="17" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                        <li onclick="SelectRestArea(1)">
                            <icrop:CustomLabel ID="lblNotTakeBreak" runat="server" TextWordNo="18" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="OverShadow">&nbsp;</div>
        </div>

		<%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <%--テクニシャン選択画面--%>
        <div class="popTechnicianWindow">
            <%--アクティブインジケータ--%>
            <div id="TechnicianActiveIndicator"></div>

            <div class="Balloon">
                <div class="borderBox">
                    <div class="Arrow">&nbsp;</div>
                    <div class="myDataBox">&nbsp;</div>
                </div>
                <div class="gradationBox">
                    <div class="ArrowMask">
                        <div class="Arrow">&nbsp;</div>
                    </div>
                    <div class="scRestPopUpHeaderBg">&nbsp;</div>
                    <div class="scRestPopUpDataBg">&nbsp;</div>
                </div>
            </div>
            <div class="PopUpHeader">
                <icrop:CustomLabel ID="lblTechnicianTitle" runat="server" TextWordNo="30" Width="106px" UseEllipsis="True"></icrop:CustomLabel>
                <div class="LeftBtn" onclick="CancelTechnicianWindow()">
                    <icrop:CustomLabel ID="CustomLabel36" runat="server" TextWordNo="31" UseEllipsis="False" class="Ellipsis" ></icrop:CustomLabel>
                </div>
                <div class="RightBtn" onclick="ConfirmTechnicianWindow()">
                    <icrop:CustomLabel ID="CustomLabel37" runat="server" TextWordNo="32" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
            </div>
            <div class="dataBox">
                <div class="innerDataBox">
                    <ul class="DataListTable NoMargin">
                    </ul>
                </div>
            </div>
            <div class="OverShadow">&nbsp;</div>
        </div>
        <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %>  

        <%--中断理由画面--%>
		<div class="popStopWindowBase">
		    <div class="Balloon">
		    <div class="borderBox">
                <div class="Arrow">&nbsp;</div>
		        <div class="myDataBox">&nbsp;</div>
	        </div>
		    <div class="gradationBox">
                <div class="ArrowMaskR"><div class="ArrowR">&nbsp;</div></div>
                <div class="ArrowMaskL"><div class="ArrowL">&nbsp;</div></div>
		        <div class="scStopPopUpHeaderBg">&nbsp;</div>
		        <div class="scStopPopUpDataBg">&nbsp;</div>
	        </div>
	        </div>
		    <div class="PopUpHeader">
                <icrop:CustomLabel ID="lblStopReasonTitle" runat="server" TextWordNo="19" Width="158px" UseEllipsis="true"></icrop:CustomLabel>
		        <div class="LeftBtn" onclick="CancelStopWindow()">
                    <icrop:CustomLabel ID="lblStopCancelBtn" runat="server" TextWordNo="20" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
		        <div class="RightBtn" onclick="ConfirmStopWindow()">
                    <icrop:CustomLabel ID="lblStopLoginBtn" runat="server" TextWordNo="21" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                </div>
            </div>
		    <div class="dataBox">
                <div id="StopMemoScrollBox">
		            <div class="innerDataBox">
                    <icrop:CustomLabel ID="lblStopReason" runat="server" TextWordNo="22" UseEllipsis="False" class="PopInnerTitle Ellipsis" ></icrop:CustomLabel>
                    <asp:Button runat="server" ID="btnJobStopDummy" Width="1px" Height="1px" style="opacity:0; position: absolute;" />
		                <!--<div class="innerDataBox02">-->
                    <!-- Window内部 -->
                    <ul class="DataListTable">
                        <li class="Check" onclick="SelectStopArea(0)">
                            <icrop:CustomLabel ID="CustomLabel29" runat="server" TextWordNo="23" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                        <li onclick="SelectStopArea(1)">
                            <icrop:CustomLabel ID="CustomLabel30" runat="server" TextWordNo="24" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                        <li onclick="SelectStopArea(2)">
                            <icrop:CustomLabel ID="CustomLabel31" runat="server" TextWordNo="25" UseEllipsis="False" class="Ellipsis"></icrop:CustomLabel>
                        </li>
                    </ul>
                    <icrop:CustomLabel ID="CustomLabel32" runat="server" TextWordNo="26" UseEllipsis="False" class="PopInnerTitle Ellipsis"></icrop:CustomLabel>
                    <ul class="TableWorkingHours">
                        <li>
                        <dl>
                            <dd>
                                <p class="LeftArrow" onclick="ChangeStopMinutes(-5)"><span></span></p>
                                <div class="StopTimeLabel" onclick="ClickStopTime()">
                                    <icrop:CustomLabel ID="CustomLabel33" runat="server" TextWordNo="27" UseEllipsis="False"></icrop:CustomLabel>
                                </div>
                                <icrop:CustomTextBox runat="server" ID="StopTimeTxt" Width="90px" CssClass="ChipDetailEllipsis" MaxLength="10"></icrop:CustomTextBox>
                                <p class="RightArrow" onclick="ChangeStopMinutes(5)"><span></span></p>
                            </dd>
                        </dl>
                        </li>
                    </ul>
              
                    <icrop:CustomLabel ID="CustomLabel34" runat="server" TextWordNo="28" UseEllipsis="False" class="PopInnerTitle Ellipsis"></icrop:CustomLabel>
                    <ul class="DataListTable">
                    <li class="NextArrow">
                        <icrop:CustomLabel runat="server" ID="lblDetailStopMemo" Width="230px" CssClass="ChipDetailEllipsis" ></icrop:CustomLabel>
                        <asp:DropDownList runat="server" ID="dpDetailStopMemo"></asp:DropDownList>
                    </li>
                    </ul>
                    <ul class="DataListTable">
                        <li class=" Hg111">
                             <asp:TextBox ID="txtStopMemo" runat="server" TextMode="MultiLine" width="273px" Height="100px" maxlen="200" CssClass="ChipDetailEllipsis"></asp:TextBox>
                        </li>
                    </ul>
              
                    <!-- /Window内部 -->
	                <!--</div>-->
	                </div>
               </div>
	    </div>
		    <div class="OverShadow">&nbsp;</div>
	    </div>

	  	<%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <%'プルダウンリフレッシュのレイアウトテンプレートエリア %>
        <div id="pullDownToRefreshTemplate" style="display:none">
            <!--プルダウンリフレッシュエリア START-->
            <div class="pullDownToRefresh step0">
                <!--内部ボックス-->
                <div class="pullDownToRefresh-inBox">
                    <!--中央寄せのアイコン＆テキスト表示エリア-->    
                    <div class="pullDownToRefresh-center">
                        <!--上下矢印アイコン-->
                        <span class="pullDownToRefresh-icon"></span>
                        <!--読み込み中アイコン-->
                        <span class="pullDownToRefresh-loding"></span>
                        <!--テキスト-->
                        <div class="pullDownToRefresh-text">
                            <!--メッセージ-->
                            <span class="pullDownToRefresh-textBlock pullDownToRefresh-message">
                                <icrop:CustomLabel ID="FixMessagStep0" runat="server" TextWordNo="33" CssClass="pullDownToRefresh-message-step0"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep1" runat="server" TextWordNo="34" CssClass="pullDownToRefresh-message-step1"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="FixMessageStep2" runat="server" TextWordNo="34" CssClass="pullDownToRefresh-message-step2"></icrop:CustomLabel><br />
                                <icrop:CustomLabel ID="FixMessageUpdateTime" runat="server" TextWordNo="35" CssClass="pullDownToRefresh-message-updateTime"></icrop:CustomLabel>
                                <icrop:CustomLabel ID="MessageUpdateTime" runat="server"  CssClass="pullDownToRefresh-message-updateTime"></icrop:CustomLabel>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <!--プルダウンリフレッシュエリア END-->
        </div>
		<%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %>  

        <%--チップ詳細画面--%>
        <uc1:SC3240201 ID="SC3240201Page" runat="server" EnableViewState="false"/>
        <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <%--チップ新規作成画面--%>
        <uc3:SC3240501 ID="SC3240501Page" runat="server" EnableViewState="false"/>    
        <%'2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END %>  
        
        <%'2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START %>
        <uc4:SC3240701 ID="SC3240701Page" runat="server" EnableViewState="false"/>
        <%'2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END%> 
    </div>
	 <%--サブチップ画面--%>
    <div id="SubArea" runat="server">   
    <uc2:SC3240301 ID="SC3240301Page" runat="server" />
	</div>
    <!-- ここまでメインブロック -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
<!-- ここからフッター -->
    <div id="InitFooterArea" runat="server">
	    <div class="InitFooterButton_Space"></div>
        <div id="FooterButton100" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(100);">
		    <div id="FooterButtonIcon100" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount100" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName100" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="100" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox100" runat="server"></div>
        </div>
        <div id="FooterButton200" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(200);">
		    <div id="FooterButtonIcon200" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount200" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName200" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="200" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox200" runat="server"></div>
        </div>
        <div id="FooterButton300" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(300);">
		    <div id="FooterButtonIcon300" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount300" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName300" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel3" runat="server" TextWordNo="300" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox300" runat="server"></div>
        </div>
        <div id="FooterButton400" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(400);">
		    <div id="FooterButtonIcon400" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount400" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName400" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel4" runat="server" TextWordNo="400" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox400" runat="server"></div>				
        </div>
        <div id="FooterButton500" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(500);">
		    <div id="FooterButtonIcon500" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount500" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName500" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel5" runat="server" TextWordNo="500" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox500" runat="server"></div>				
        </div>
	    <div class="InitFooterButton_Space"></div>
        <div id="FooterButton600" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(600);">
		    <div id="FooterButtonIcon600" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount600" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName600" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel6" runat="server" TextWordNo="600" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox600" runat="server"></div>
        </div>
        <div id="FooterButton700" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(700);">
		    <div id="FooterButtonIcon700" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount700" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName700" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel7" runat="server" TextWordNo="700" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox700" runat="server"></div>				
        </div>
        <div id="FooterButton800" runat="server" class="FooterButton VisibilityIcon_Off" onclick="FooterEvent(800);">
		    <div id="FooterButtonIcon800" runat="server"></div>
		    <div class="FooterNumber"><icrop:CustomLabel ID="FooterButtonCount800" runat="server" Text=""></icrop:CustomLabel></div>
		    <div id="FooterButtonName800" runat="server" class="FooterName_Off"><icrop:CustomLabel ID="CustomLabel8" runat="server" TextWordNo="800" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox800" runat="server"></div>				
        </div>
    </div>
    <div id="ChipFooterArea" runat="server">
	    <div class="ChipFooterButton_Space"></div>
        <div id="FooterButton2800" runat="server" class="FooterButton NormalIcon_Off" onclick="return false;" style="display:none;">
		    <div id="FooterButtonIcon2800" runat="server"></div>
		    <div id="FooterButtonName2800" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel28" runat="server" TextWordNo="2800" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2800" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2700" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2700);" style="display:none;">
		    <div id="FooterButtonIcon2700" runat="server"></div>
		    <div id="FooterButtonName2700" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel9" runat="server" TextWordNo="2700" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2700" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2600" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2600);" style="display:none;">
		    <div id="FooterButtonIcon2600" runat="server"></div>
		    <div id="FooterButtonName2600" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel10" runat="server" TextWordNo="2600" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2600" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2500" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2500);" style="display:none;">
		    <div id="FooterButtonIcon2500" runat="server"></div>
		    <div id="FooterButtonName2500" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel11" runat="server" TextWordNo="2500" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2500" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2400" runat="server" class="FooterButton NormalIcon_Off" onclick="return false;" style="display:none;">
		    <div id="FooterButtonIcon2400" runat="server"></div>
		    <div id="FooterButtonName2400" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel12" runat="server" TextWordNo="2400" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2400" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2300" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2300);" style="display:none;">
		    <div id="FooterButtonIcon2300" runat="server"></div>
		    <div id="FooterButtonName2300" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel13" runat="server" TextWordNo="2300" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2300" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton2200" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2200);" style="display:none;">
		    <div id="FooterButtonIcon2200" runat="server"></div>
		    <div id="FooterButtonName2200" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel14" runat="server" TextWordNo="2200" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2200" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <div id="FooterButton2900" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2900);" style="display:none;">
		    <div id="FooterButtonIcon2900" runat="server"></div>
		    <div id="FooterButtonName2900" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel35" runat="server" TextWordNo="2900" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2900" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END %>  
	    <div id="FooterButton2100" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2100);" style="display:none;">
		    <div id="FooterButtonIcon2100" runat="server"></div>
		    <div id="FooterButtonName2100" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel15" runat="server" TextWordNo="2100" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2100" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START %>  
        <div id="FooterButton3000" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3000);" style="display:none;">
		    <div id="FooterButtonIcon3000" runat="server"></div>
		    <div id="FooterButtonName3000" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel38" runat="server" TextWordNo="3000" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox3000" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END %>  
	    <div id="FooterButton2000" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(2000);" style="display:none;">
		    <div id="FooterButtonIcon2000" runat="server"></div>
		    <div id="FooterButtonName2000" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel16" runat="server" TextWordNo="2000" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox2000" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1900" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1900);" style="display:none;">
		    <div id="FooterButtonIcon1900" runat="server"></div>
		    <div id="FooterButtonName1900" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel17" runat="server" TextWordNo="1900" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1900" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1800" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1800);" style="display:none;">
		    <div id="FooterButtonIcon1800" runat="server"></div>
		    <div id="FooterButtonName1800" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel18" runat="server" TextWordNo="1800" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1800" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1700" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1700);" style="display:none;">
		    <div id="FooterButtonIcon1700" runat="server"></div>
		    <div id="FooterButtonName1700" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel19" runat="server" TextWordNo="1700" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1700" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1600" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1600);" style="display:none;">
		    <div id="FooterButtonIcon1600" runat="server"></div>
		    <div id="FooterButtonName1600" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel20" runat="server" TextWordNo="1600" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1600" runat="server" class="FooterLightBox"></div>
	    </div>
        <%'2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START %> 
        <div id="FooterButton3400" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3400);" style="display:none;" >
            <div id="FooterButtonIcon3400" runat="server"></div>
            <div id="FooterButtonName3400" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel41"  runat="server" TextWordNo="3400" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox3400" runat="server" class="FooterLightBox"></div>
        </div>     
        <%'2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END %> 
	    <div id="FooterButton1500" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1500);" style="display:none;">
		    <div id="FooterButtonIcon1500" runat="server"></div>
		    <div id="FooterButtonName1500" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel21" runat="server" TextWordNo="1500" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1500" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1400" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1400);" style="display:none;">
		    <div id="FooterButtonIcon1400" runat="server"></div>
		    <div id="FooterButtonName1400" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel22" runat="server" TextWordNo="1400" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1400" runat="server" class="FooterLightBox"></div>
	    </div>
	    <div id="FooterButton1300" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1300);" style="display:none;">
		    <div id="FooterButtonIcon1300" runat="server"></div>
		    <div id="FooterButtonName1300" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel23" runat="server" TextWordNo="1300" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1300" runat="server" class="FooterLightBox"></div>				
	    </div>
	    <div id="FooterButton1200" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1200);" style="display:none;">
		    <div id="FooterButtonIcon1200" runat="server"></div>
		    <div id="FooterButtonName1200" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel24" runat="server" TextWordNo="1200" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1200" runat="server" class="FooterLightBox"></div>				
	    </div>
	    <div id="FooterButton1100" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1100);" style="display:none;">
		    <div id="FooterButtonIcon1100" runat="server"></div>
		    <div id="FooterButtonName1100" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel25" runat="server" TextWordNo="1100" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1100" runat="server" class="FooterLightBox"></div>				
	    </div>
	    <div id="FooterButton1000" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(1000);" style="display:none;">
		    <div id="FooterButtonIcon1000" runat="server"></div>
		    <div id="FooterButtonName1000" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel26" runat="server" TextWordNo="1000" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox1000" runat="server" class="FooterLightBox"></div>				
	    </div>

        <%'2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START %>  
        <div id="FooterButton3100" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3100);" style="display:none;">
		    <div id="FooterButtonIcon3100" runat="server"></div>
		    <div id="FooterButtonName3100" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel39" runat="server" TextWordNo="3100" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox3100" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END %>  

        <%'2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START %>  
        <div id="FooterButton3200" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3200);" style="display:none;">
		    <div id="FooterButtonIcon3200" runat="server"></div>
		    <div id="FooterButtonName3200" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel40" runat="server" TextWordNo="3200" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox3200" runat="server" class="FooterLightBox"></div>
	    </div>
	    <%'2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END %>  

	    <div id="FooterButton900" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(900);" style="display:none;">
		    <div id="FooterButtonIcon900" runat="server"></div>
		    <div id="FooterButtonName900" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel27" runat="server" TextWordNo="900" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox900" runat="server" class="FooterLightBox"></div>				
	    </div>

        <%'2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START %>
        <div id="FooterButton3300" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3300)" style="display:none;">
		    <div id="FooterButtonIcon3300" runat="server"></div>
		    <div id="FooterButtonName3300" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel42" runat="server" TextWordNo="3300" UseEllipsis="False"></icrop:CustomLabel></div>
            <div id="FooterButtonBox3300" runat="server" class="FooterLightBox"></div>				
	    </div>
        <%'2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END %>

        <%'2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START %>
        <div id="FooterButton3500" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3500);" style="display:none;">
		    <div id="FooterButtonIcon3500" runat="server"></div>
		    <div id="FooterButtonName3500" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel43" runat="server" TextWordNo="3500" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox3500" runat="server" class="FooterLightBox"></div>
	    </div>
        <div id="FooterButton3600" runat="server" class="FooterButton NormalIcon_Off" onclick="FooterEvent(3600);" style="display:none;">
		    <div id="FooterButtonIcon3600" runat="server"></div>
		    <div id="FooterButtonName3600" runat="server" class="FooterName_On"><icrop:CustomLabel ID="CustomLabel44" runat="server" TextWordNo="3600" UseEllipsis="False"></icrop:CustomLabel></div>
		    <div id="FooterButtonBox3600" runat="server" class="FooterLightBox"></div>
	    </div>
        <%'2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START %>
    </div>
</asp:Content>

