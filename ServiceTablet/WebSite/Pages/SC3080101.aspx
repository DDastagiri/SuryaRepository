<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3080101.aspx.vb" Inherits="Pages_SC3080101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3080101/SC3080101.css?20111228180001" />
    <script type="text/javascript" src="../Scripts/SC3080101/SC3080101.js?20111227180000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<asp:Button ID="sortButton" runat="server" Text="ソート処理" CssClass="disableButton" />
<asp:Button ID="nextButton" runat="server" Text="顧客詳細画面へ遷移する" CssClass="disableButton" />
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
                            OnClientRender="customerRepeater_Render" Width="942" Height="500" 
                            PageRows="50" maxCacheRows="100"/>
                        <script type="text/javascript">
                            function customerRepeater_Render(row, view) {

                                var str = "";
                                var strColor = "";

                                var columeId = "divTable" + row.NO;
                                var nameId = "dataUserName_" + row.NO;

                                //偶数／奇数行によって背景色を変える
                                if (row.flg == 0){
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
                                    prm = prm + "'" + row.VIN + "',";
                                }
                                prm = prm + "'" + row.STAFFCD + "',";
                                prm = prm + "'" + columeId + "'";

                                //HTML作成
                                elementParent = $("<di id=" + columeId + " class='ncs5001TitleTableBox " + strColor + "' onclick=selectCoustomer(" + prm + ")></div> ");

                                //1カラム目 (顧客名等)
                                elementColumn1 = $("<div class='leftDiv column1 tableHeader1 " + strColor + "' > </div>");
                                    str = "<span class='dataPortraits'><img src='" + row.IMAGEPATH + "' width='60' height='61' alt='人物写真'></span> "
                                        + "<span class='dataSettingIcon1'><span>" + row.CSTKINDNM + "</span></span> ";
                                    if (row.CUSTYPE == "") {
                                        str = str + "	";
                                    } else {
                                        str = str + "	<span class='dataSettingIcon2'><span>" + row.CUSTYPE + "</span></span> ";
                                    }
                                    elementCustInfo = $(str);
                                    elementName = $("<span class='dataUserName' id='" + nameId + "' style='display:inline-block;width:135px;'>" + row.NAME + "</span>");
                                    elementName.CustomLabel({ 'useEllipsis': 'true' });
                                elementColumn1.append(elementCustInfo);
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
                                
                                //最後の行まで表示時は、次の50件を表示を消す
                                if (row.NO == row.maxrow) {
                                    str = str + "<script type='text/javascript'>";
                                    str = str + "    $(function () {";
                                    str = str + "        $('.icrop-CustomRepeater-pager').css('display','none');";
                                    str = str + "    });"
                                    str = str + "</" + "script>";
                                }

                                //最初の行まで表示時は、次の50件を表示を表示する
                                if (row.NO == 0) {
                                    str = str + "<script type='text/javascript'>";
                                    str = str + "    $(function () {";
                                    str = str + "        $('.icrop-CustomRepeater-inner-bottomPager').css('display','block');";
                                    str = str + "    });"
                                    str = str + "</" + "script>";
                                }

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
        
	<asp:HiddenField ID="nextMessageHidden" runat="server" />

    <%'サーバー処理中のオーバーレイとアイコン %>
    <div id="serverProcessOverlayBlack"></div>
    <div id="serverProcessIcon"></div>

</asp:Content>

