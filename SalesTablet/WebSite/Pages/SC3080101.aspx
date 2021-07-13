<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3080101.aspx.vb" Inherits="Pages_SC3080101" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3080101/SC3080101.css?20120509010500" />
    <script type="text/javascript" src="../Scripts/SC3080101/SC3080101.js?20120920000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080101.aspx
'─────────────────────────────────────
'機能： 顧客検索一覧
'補足： 
'作成： 2011/11/18 TCS 安田
'更新： 2012/01/26 TCS 安田 【SALES_1B】レイアウト調整
'更新： 2012/04/26 TCS 安田 HTMLエンコード対応
'更新： 2012/05/17 TCS 安田 クルクル対応
'更新： 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/05 TCS 森    Aカード情報相互連携開発
'更新： 2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2019/05/28 TS 髙橋(龍) 画像形式（拡張子）変更対応(TR-SVT-TMT-20170725-001)
'─────────────────────────────────────
-->

<asp:Button ID="sortButton" runat="server" Text="ソート処理" CssClass="disableButton" />
<asp:Button ID="nextButton" runat="server" Text="顧客詳細画面へ遷移する" CssClass="disableButton" />
<!--　2012/05/17 TCS 安田 クルクル対応 START　-->
<asp:Button ID="refreshButton" runat="server" Text="再描画する" CssClass="disableButton" />
<!--　2012/05/17 TCS 安田 クルクル対応 END　-->
	<div id="BaseBox"><!--　←サイズ確認用のタグです　-->
	<div id="container"><!--　←全体を含むタグです。　-->
		<!-- 中央部分-->
		<div id="main">
		<!-- ここからコンテンツ -->
			<div id="contents">
				<div id="TcvNsc05-01Main">
					<h2><span><icrop:CustomLabel ID="goukeiLabel" runat="server" TextWordNo="0" Text="合計100件" Width="300px" /></span></h2>
                    
<asp:Panel ID="resultListPanel" runat="server">

					<table border="0" cellpadding="0" cellspacing="0" class="ncs5001TitleTable">
						<tr>
							<th class="column1 tableHeader1" align="center" valign="middle">
                                <span>        
                                    <a href="#" class="scCoutomerNameButton">
                                        <icrop:CustomLabel ID="CustomLabel23" runat="server" TextWordNo="2" Text="お客様"/>
                                    </a>
                                </span>
                            </th>
							<th class="column2 tableHeader2" align="center" valign="middle">
                                <icrop:CustomLabel ID="CustomLabel1" runat="server" TextWordNo="3" 
                                    Text="Mobile/Home" CssClass="noBorder" UseEllipsis="False"/>
                            </th>
							<th class="column3 tableHeader3" align="center" valign="middle">
                                <span>
                                    <a href="#" class="scVclregButton">
                                        <icrop:CustomLabel ID="CustomLabel2" runat="server" TextWordNo="4" Text="保有車両"/>
                                    </a>
                                </span>
                            </th>
							<th class="column4 tableHeader4" align="center" valign="middle">
                                <span>
                                    <a href="#" class="scSCButton">
                                        <icrop:CustomLabel ID="ScLabel" runat="server" TextWordNo="0" Text="SC"/>
                                        
                                    </a>
                                </span>
                            </th>
							<th class="column5 tableHeader5" align="center" valign="middle">
                                <span>
                                    <a href="#" class="scSAButton">
                                        <icrop:CustomLabel ID="SaLabel" runat="server" TextWordNo="0" Text="SA"/>
                                        
                                    </a>
                                </span>
                            </th>
						</tr>
					</table>

                        <icrop:CustomRepeater ID="customerRepeater" runat="server" 
                            OnClientRender="customerRepeater_Render" Width="947" Height="500" 
                            PageRows="50" maxCacheRows="100"/>
                        <script type="text/javascript">
                            function customerRepeater_Render(row, view) {

                                var str = "";
                                var strColor = "";

                                var columeId = "divTable" + row.NO;
                                var nameId = "dataUserName_" + row.NO;

                                //偶数／奇数行によって背景色を変える
                                if (row.flg == 0) {
                                    strColor = "ColorWhite";
                                } else {
                                    strColor = "ColorGray";
                                }

                                //顧客詳細呼び出しパラメーター作成
                                var prm = ""
                                prm = prm + "'" + row.updateFlg + "',";
                                prm = prm + "'" + row.CSTKIND + "',";
                                prm = prm + "'" + row.CRCUSTID + "',";
                                if (row.CSTKIND == "2") {
                                    //2:未顧客
                                    prm = prm + "'" + row.SEQNO + "',";
                                } else {
                                    //1:自社客
                                    //2013/06/30 TCS 趙 2013/10対応版 既存流用 START
                                    prm = prm + "'" + row.SEQNO + "',";
                                    //2013/06/30 TCS 趙 2013/10対応版 既存流用 END
                                }
                                prm = prm + "'" + row.STAFFCD + "',";
                                prm = prm + "'" + columeId + "'";

                                // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                                //HTML作成
                                elementParent = $("<div id=" + columeId + " class='ncs5001TitleTableBox " + strColor + "' onclick=selectCoustomer(" + prm + ")></div> ");

                                //1カラム目 (顧客名等)
                                elementColumn1 = $("<div class='leftDiv column1 tableHeader1 " + strColor + "' > </div>");
                                // 2012/01/26 TCS 安田 【SALES_1B】レイアウト調整 classの追加 START
                                // 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 START
                                //str = "<span class='dataPortraits'><img src='" + row.IMAGEPATH + "'  alt='人物写真' width='60' height='61' class='imgPortraits'></span> "
                                str = "<span class='dataPortraits'><img src='" + row.IMAGEPATH + "'  alt='人物写真' width='60' height='60' class='imgPortraits'></span> "
                                // 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 END
                                // 2012/01/26 TCS 安田 【SALES_1B】レイアウト調整 classの追加 END
                                        + "<span class='dataSettingIcon1'><span>" + row.CSTKINDNM + "</span></span> ";
                                str += isNaN(parseInt(row.joinType, 10))
                                    ? ''
                                    : '<span class="dataSettingIcon2"><span>' + (['', 'I', 'C'][row.joinType]) + '</span></span>';
                                // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

                                //2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
                                if (row.imgLoyalCustomerL == "") {
                                    str = str + "	";
                                } else {
                                    str = str + "<span class='dataSettingIcon3' style='background-image:url(" + row.imgLoyalCustomerL + ");'></span>";

                                }
                                //2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END

                                elementCustInfo = $(str);
                                elementName = $("<span class='dataUserName' id='" + nameId + "' style='display:inline-block;width:135px;'>" + row.NAME + "</span>");
                                elementName.CustomLabel({ 'useEllipsis': 'true' });
                                
                                elementSocicalId = $("<span class='dataSocicalId' id='" + nameId + "' style='display:inline-block;width:135px;'>" + row.CST_SOCIALNUM + "</span>");
                                elementSocicalId.CustomLabel({ 'useEllipsis': 'true' });
                                
                                elementColumn1.append(elementCustInfo);
                                elementColumn1.append(elementSocicalId);
                                elementColumn1.append(elementName);

                                //2カラム目 (携帯/電話番号)
                                elementColumn2 = $("<div class='leftDiv column2 tableHeader2 " + strColor + "' > </div> ");
                                elementMobile = $("<span class='telMobile' style='display:inline-block;width:150px;'>" + row.MOBILE + "</span>");
                                elementLine1 = $("<span class='SplitLine1'>&nbsp;</span> ");
                                elementTelHome = $("<span class='telHome' style='display:inline-block;width:150px;'>" + row.TELNO + "</span>");
                                elementColumn2.append(elementMobile);
                                elementColumn2.append(elementLine1);
                                elementColumn2.append(elementTelHome);

                                //3カラム目 (車両情報)
                                elementColumn3 = $("<div class='leftDiv column3 tableHeader3 " + strColor + "' > </div> ");
                                elementVclregno = $("<span class='MyFleet' style='display:inline-block;width:150px;'>" + row.VCLREGNO + "</span> ");
                                elementSerisnm = $("<span class='MyCarName' style='display:inline-block;width:100px;'>" + row.SERIESNM + "</span> ");
                                elementLine2 = $("<span class='SplitLine2'>&nbsp;</span> ");
                                elementMyCarNumber = $("<span class='MyCarNumber' style='display:inline-block;width:240px;'>" + row.VIN + "</span> ");

                                elementVclregno.CustomLabel({ 'useEllipsis': 'true' });
                                elementSerisnm.CustomLabel({ 'useEllipsis': 'true' });
                                elementMyCarNumber.CustomLabel({ 'useEllipsis': 'true' });

                                elementColumn3.append(elementVclregno);
                                elementColumn3.append(elementSerisnm);
                                elementColumn3.append(elementLine2);
                                elementColumn3.append(elementMyCarNumber);

                                //4カラム目 (SS)
                                elementColumn4 = $("<div class='leftDiv column4 tableHeader4 " + strColor + "' align='center'></div>");
                                elementSsName = $("<span class='ssName' style='display:inline-block;width:145px;'>" + row.SSUSERNAME + "</span> ");
                                elementSsName.CustomLabel({ 'useEllipsis': 'true' });
                                elementColumn4.append(elementSsName);

                                //5カラム目 (SA)                                
                                elementColumn5 = $("<div class='leftDiv column5 tableHeader5 " + strColor + "' align='center'></div>");
                                elementSaName = $("<span class='saName' style='display:inline-block;width:145px;'>" + row.SAUSERNAME + "</span> ");
                                elementSaName.CustomLabel({ 'useEllipsis': 'true' });
                                elementColumn5.append(elementSaName);

                                elementParent.append(elementColumn1);
                                elementParent.append(elementColumn2);
                                elementParent.append(elementColumn3);
                                elementParent.append(elementColumn4);
                                elementParent.append(elementColumn5);

                                view.append(elementParent);

                                //2012/04/26 TCS 安田 HTMLエンコード対応 START
                                if (row.NO == 1) {
                                    //最初の１行目に対して、HTMLデコードする
                                    $("#customerRepeater .icrop-CustomRepeater-inner-bottomPager-label").text(SC3080101HTMLDecode($("#customerRepeater .icrop-CustomRepeater-inner-bottomPager-label").text()));
                                    $("#customerRepeater .icrop-CustomRepeater-inner-topPager-label").text(SC3080101HTMLDecode($("#customerRepeater .icrop-CustomRepeater-inner-topPager-label").text()));
                                }
                                //2012/04/26 TCS 安田 HTMLエンコード対応 END
                            }


                        </script> 
</asp:Panel>
				</div>
			</div>
		<!-- ここまでコンテンツ -->
		</div>
		<!-- ここまで中央部分 -->
		
	</div>
	</div><!--　←全体を含むタグ終わり　-->
	
    <asp:HiddenField ID="fromNoHidden" runat="server" />
	<asp:HiddenField ID="tonoHidden" runat="server" />
	<asp:HiddenField ID="currentPageHidden" runat="server" />
	<asp:HiddenField ID="sortTypeHidden" runat="server" />
	<asp:HiddenField ID="sortOrderHidden" runat="server" />
    
	<asp:HiddenField ID="cstkindHidden" runat="server" />
	<asp:HiddenField ID="customerClassHidden" runat="server" />    
	<asp:HiddenField ID="crcustidHidden" runat="server" />
	<asp:HiddenField ID="vclHidden" runat="server" />
	<asp:HiddenField ID="salessStaffcdHidden" runat="server" />
        
<!-- 2012/01/26 TCS 安田 【SALES_1B】メッセージ用隠し項目の追加 START -->
	<asp:HiddenField ID="nextMessageHidden" runat="server" />
	<asp:HiddenField ID="nextLastMessageHidden" runat="server" />
    
	<asp:HiddenField ID="forwordMessageHidden" runat="server" />
	<asp:HiddenField ID="forwordFirstMessageHidden" runat="server" />
<!-- 2012/01/26 TCS 安田 【SALES_1B】メッセージ用隠し項目の追加 END -->

<!-- 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START -->
	<asp:HiddenField ID="searchFlgHidden" runat="server" />
	<asp:HiddenField ID="selectConfirmHidden" runat="server" />
	<asp:HiddenField ID="backFlgHidden" runat="server" />
	<asp:Button ID="backButton" runat="server" Text="顧客詳細画面へ戻る" CssClass="disableButton" />
<!-- 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END -->
<%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START--%>
    <asp:HiddenField ID="updateVisitCustomerInfoFlg" runat="server" />
<%-- 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END--%>
<%-- 2013/12/05 TCS 森    Aカード情報相互連携開発 START --%>
    <asp:HiddenField ID="searchOrgTeamList" runat="server" />
<%-- 2013/12/05 TCS 森    Aカード情報相互連携開発 END --%>

    <%'サーバー処理中のオーバーレイとアイコン %>
    <div id="serverProcessOverlayBlack"></div>
    <div id="serverProcessIcon"></div>

</asp:Content>

