<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="SC3040802.aspx.vb" Inherits="Pages_SC3040802" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3040802/SC3040802.css" type="text/css" media="screen,print"/>
    <script type="text/javascript" src="../Scripts/SC3040802/SC3040802.js?20120413000000"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server" overflow="scroll">
<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3040802.aspx
'─────────────────────────────────────
'機能： 通知一覧(MG用)
'補足： 
'作成： 2012/01/05 TCS 明瀬
'更新： 2012/04/18 TCS 明瀬 HTMLエンコード対応
'更新： 2012/01/10 TCS 森 Aカード情報相互連携開発
'更新： 2014/05/10 TCS 武田 受注後フォロー機能開発
'更新： 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────
-->
    <!-- 通知が選択されたときにjsで押される不可視ボタン -->
    <asp:Button ID="nextButton" runat="server" CssClass="hiddenButton" />
    <!-- 基盤から通知連絡があったときにjsで押される不可視ボタン -->
    <asp:Button ID="postBackButton" runat="server" CssClass="hiddenButton" />
   
    <div id="SC3040802Main">
        <!-- ここからコンテンツ -->
        <!-- pop -->
        <div id="popUpDiv" class="scNscPopUpContactList scNscPopUpContactSelect48">
            <div class="scNscPopUpContactListWindownBox WindownBox48">
                <!-- 閉じるボタン　依頼一覧 -->
                <div class="scNscPopUpContactListHeader">
                    <a href="#" style="display:block;">
                       <input type="image" id="openCloseButton" class="IcnButton" src="../Styles/Images/SC3040802/nsc01IcnButton.png" alt="OpenClose"/>
                    </a>
                    <h3>
                        <icrop:CustomLabel ID="HeaderLabel" runat="server" Width="270" UseEllipsis="False" CssClass="clip" TextWordNo="1" />
                    </h3>                        
                </div>

                <%-- 2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL --%>
                <%-- 2014/05/10 TCS 武田 受注後フォロー機能開発 END --%>

                <!-- 通知データあり -->
                <asp:Panel ID="noticeInfoPanel" runat="server">
        
                    <div class="scNscPopUpContactListMain"> 
                        <icrop:CustomRepeater ID="noticeRepeater" runat="server" OnClientRender="noticeRepeater_Render" Width="319" Height="568" PageRows="10" maxCacheRows="50"/>

                        <script type="text/javascript">
                            function noticeRepeater_Render(row, view) {

                                //システムエラー発生時
                                if (row.NO == "9999") {
                                    alert(row.ERRMESSAGE);
                                    return false;
                                }

                                var liNoticeId = "liNotice" + row.NO;
                                //次画面呼び出しとI/F呼び出しのパラメータの作成(使用するパラメータの選別はコードビハインドで行う)
                                var prmArray = new Array(13);
                                //2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
                                prmArray[0] = encodeURIComponent(row.REQNOTICE);
                                prmArray[1] = encodeURIComponent(row.REQCTGID);
                                prmArray[2] = encodeURIComponent(row.REQCLASSID);
                                prmArray[3] = encodeURIComponent(row.CSTKIND);
                                prmArray[4] = encodeURIComponent(row.CSTCLASS);
                                prmArray[5] = encodeURIComponent(row.CRCSTID);
                                prmArray[6] = encodeURIComponent(row.FROMSTAFFACCOUNT);
                                prmArray[7] = encodeURIComponent(row.FROMSTAFFNAME);
                                prmArray[8] = encodeURIComponent(row.CUSTOMERNAME);
                                prmArray[9] = encodeURIComponent(row.SALESSTAFFCD);
                                prmArray[10] = encodeURIComponent(row.FLLWUPBOXSTRCD);
                                prmArray[11] = encodeURIComponent(row.FLLWUPBOX);
                                prmArray[12] = encodeURIComponent(row.STATUS);
                                //2012/04/18 TCS 明瀬 HTMLエンコード対応 End

                                var reqIcnClass = "";
                                var btmTxt1Class = "";
                                var btmTxt2Class = "";
                                var btmTxt1 = "";
                                var btmTxt2 = "";
                                //通知依頼種別が"02"(価格相談)の場合
                                if (row.REQCTGID == "02") {
                                    reqIcnClass = "Icn02";
                                    btmTxt1Class = "BottomText01Req";
                                    btmTxt2Class = "BottomText02";
                                    btmTxt1 = row.MESSAGE1.split("|")[0] + " ";
                                    btmTxt2 = row.MESSAGE1.split("|")[1];

                                    //通知依頼種別が"03"(ヘルプ)の場合
                                } else if (row.REQCTGID == "03") {
                                    reqIcnClass = "Icn01";
                                    btmTxt1Class = "BottomText01Help";
                                    btmTxt2Class = "BottomText02None";
                                    btmTxt1 = row.MESSAGE1;
                                    // 2012/01/10 TCS 森 Aカード情報相互連携開発 START
                                    // 通知依頼種別が"08"(契約承認)の場合
                                } else if (row.REQCTGID == "08") {
                                    reqIcnClass = "Icn08";
                                    btmTxt1Class = "BottomText01Req";
                                    btmTxt2Class = "BottomText02None";
                                    btmTxt1 = row.MESSAGE1.split("|")[0] + " ";
                                    btmTxt2 = row.MESSAGE1.split("|")[1];
                                }
                                // 2012/01/10 TCS 森 Aカード情報相互連携開発 END
                                //通知送信者アイコンを動的に変更する
                                var txtName01bg = "url(../Styles/Images/Authority/" + row.ICON_IMGFILE + ") 0 0 no-repeat;";

                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                                // LoyalCustomer用レイアウトの切替え（通知送信者名／顧客名）
                                var classNameSuffixLoyal = "";
                                if (row.LOYALCUSTOMER_FLG == "1") {
                                    classNameSuffixLoyal = "LoyalCst";
                                }
                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

                                //HTML作成開始-->
                                var elementParent = "";
                                elementParent = $("<li style='list-style:none' id=" + liNoticeId + " class='" + reqIcnClass + "' onclick=selectNotice('" + prmArray + "')></li> ");

                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                                var elementStaff = "";
                                elementStaff = $("<div class='TextName01" + classNameSuffixLoyal + " ellipsis' style='background:" + txtName01bg + "'>" + row.FROMSTAFFNAME + "</div>");

                                var elementCust = "";
                                elementCust = $("<div class='TextName02" + classNameSuffixLoyal + " ellipsis'>" + row.CUSTOMERNAME + "</div>");

                                // LoyalCustomerアイコン
                                var elementLoyalCstIcon = "";
                                if (row.LOYALCUSTOMER_FLG == "1") {
                                    elementLoyalCstIcon = $("<div class='loyalCstIcon ellipsis' />");
                                }
                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

                                var elementTime = "";
                                elementTime = $("<div class='Time ellipsis'>" + row.TIMEMESSAGE + "</div>");

                                var elementMsg1 = "";
                                elementMsg1 = $("<div class='" + btmTxt1Class + " ellipsis'>" + btmTxt1 + "<span>" + btmTxt2 + "</span> </div>");

                                var elementMsg2 = "";
                                elementMsg2 = $("<div class='" + btmTxt2Class + " ellipsis'>" + row.MESSAGE2 + "</div>");

                                elementParent.append(elementStaff);
                                elementParent.append(elementCust);
                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                                if (row.LOYALCUSTOMER_FLG == "1") {
                                    elementParent.append(elementLoyalCstIcon);
                                }
                                // 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                                elementParent.append(elementTime);
                                elementParent.append(elementMsg1);
                                elementParent.append(elementMsg2);

                                view.append(elementParent);

                                //最後の行まで表示時は、「次の10件を読み込む...」を消す
                                if (row.NO == row.MAXROW) {
                                    $('.icrop-CustomRepeater-inner-bottomPager').addClass('hiddenBottomPager');
                                }
                            }
                        </script> 
                    </div>
                </asp:Panel>
            </div>
        </div>        
        <!-- pop end-->
        <!-- ここまでコンテンツ -->
    </div>

    <asp:HiddenField ID="reqNoticeHidden" runat="server" />
	<asp:HiddenField ID="reqCtgIdHidden" runat="server" />
   	<asp:HiddenField ID="reqClassIdHidden" runat="server" />
   	<asp:HiddenField ID="cstKindHidden" runat="server" />
	<asp:HiddenField ID="cstClassHidden" runat="server" />    
	<asp:HiddenField ID="crCstIdHidden" runat="server" />
    <asp:HiddenField ID="toAccountHidden" runat="server" />
	<asp:HiddenField ID="toAccountNameHidden" runat="server" />    
	<asp:HiddenField ID="cstNameHidden" runat="server" />
    <asp:HiddenField ID="salesStaffCDHidden" runat="server" Value=""/>
    <asp:HiddenField ID="fllwUpBoxStrCDHidden" runat="server" Value=""/>
    <asp:HiddenField ID="fllwUpBoxHidden" runat="server" Value=""/>
    <asp:HiddenField ID="lastStatusHidden" runat="server" Value=""/>
    <asp:HiddenField ID="noticeCountHidden" runat="server" Value="0"/>
    <asp:HiddenField ID="openCloseHidden" runat="server" Value="close"/>

    <%'サーバー処理中のオーバーレイとアイコン %>
    <div id="serverProcessOverlayBlack"></div>
    <div id="serverProcessIcon"></div>

</asp:Content>

