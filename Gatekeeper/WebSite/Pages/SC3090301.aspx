<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPageSales.master" AutoEventWireup="false" CodeFile="SC3090301.aspx.vb" Inherits="Pages_SC3090301" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <!-- ページ毎CSS -->
    <link rel="stylesheet" href="../Styles/SC3090301/SC3090301.css?20190228000000" type="text/css" media="screen,print" />    
    <script type="text/javascript" src="../Scripts/SC3090301/SC3090301.js?20190228000000"></script>
      <%--非同期読み込みのためのScriptManagerタグ--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<!--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090301.aspx
'─────────────────────────────────────
'機能： ゲートキーパーメイン
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/13 KN  y.nakamura STEP2開発 　　$01
'更新： 2012/05/23 KN  m.asano    クルクル対応　$02
'更新： 2013/04/16 TMEJ m.asano   ウェルカムボード仕様変更対応 $03
'更新： 2013/09/27 TMEJ m.asano   iOS7対応 $04
'更新： 2013/12/02 TMEJ t.shimamura   次世代e-CRBサービス 店舗展開に向けた標準作業確立 $05
'更新： 2014/01/17 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06
'更新： 2015/02/18 TMEJ y.gotoh   UAT課題#158 $07
'更新： 2018/02/19 NSK h.kawatani   REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 $08
'更新： 2019/02/28 NSK h.kawatani   REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $09
'─────────────────────────────────────
-->

    <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
 	<%-- <div id="BaseBox"> --%>
 	<div id="ContentBox">
    <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
		<asp:HiddenField ID="firstLoad" runat="server" Value="1"/>
        <asp:Button ID="CarButton" runat="server" style="display:none"/>
        <asp:Button ID="PersonButton" runat="server" style="display:none"/>
        <%-- $08 start 予約一覧ボタンの追加 --%>
        <asp:Button ID="ReserveListButton" runat="server" style="display:none"/>
        <%-- $08 end 予約一覧ボタンの追加 --%>
        <asp:Button ID="initButton" runat="server" style="display:none"/>
        <asp:Button ID="DeleteButton" runat="server" Style="display: none;" />
        <%--$02 start クルクル対応--%>
        <asp:Button ID="submitButton" runat="server" style="display:none"/>
        <asp:Button ID="refreshButton" runat="server" style="display:none"/>
        <%--$02 end クルクル対応--%>
        <asp:Button ID="NextDataShowButton" runat="server" style="display:none"/>
        <asp:Button ID="PreviewDataShowButton" runat="server" style="display:none"/>
         <%--$05 start 削除ボタン--%>
        <asp:Button ID="AllDeleteButton" runat="server" style="display:none" />
         <%--$05 end 削除ボタン--%>
        <asp:HiddenField ID="dispType" runat="server" />
        <asp:HiddenField ID="unsetDataCount" runat="server" />
        <asp:HiddenField ID="personNum" runat="server" />
        <asp:HiddenField ID="purposeType" runat="server" />
        <asp:HiddenField ID="visitDate" runat="server"  />
        <asp:HiddenField ID="selectVclNoIndex" runat="server" Value="0"/>
        <asp:HiddenField ID="selectCustIndex" runat="server" Value="0"/>
        <asp:HiddenField ID="preSelectVclNoIndex" runat="server" Value="0"/>
        <%--画面遷移後、表示するべきレコードまで画面をスクロールするかのフラグ
         0:しない、1:次へボタンからの遷移、2;前へボタンからの遷移 --%>

        <asp:HiddenField ID="autoScrollFlag" runat="server"/>
                <%--$05 start 現在表示中範囲の先頭レコードが何件目か--%>
        <asp:HiddenField ID="CurrentDisplayHeaderNumber" runat="server" Value="1"/>
                <%--$05 end 現在表示中範囲の先頭レコードが何件目か--%>
        <asp:HiddenField ID="MaxDisplayCountNumber" runat="server" Value="0"/>
        <asp:HiddenField ID="NextOrPreviewDisplayCountNumber" runat="server" Value="0"/>
        <asp:HiddenField ID ="AllDeleteText" runat="server"/>

        <%-- $08 start 車両登録番号を手入力できる機能の追加 --%>
        <asp:HiddenField ID ="ConfirmMessageText" runat="server"/>
        <asp:HiddenField ID ="InputRegNumber" runat="server"/>
        <%-- $08 end 車両登録番号を手入力できる機能の追加 --%>

        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <asp:HiddenField ID ="SalesTabletUseFlg" runat="server"/>
        <asp:HiddenField ID ="ServiceTabletUseFlg" runat="server"/>
        <asp:HiddenField ID ="VclRegNoInputType" runat="server"/>
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>

		<!-- Upper block -->
        <div class="popover" id="popOver_Delete_popover" style="display:none ">
            <div class="header"></div>
            <div class="content">
                <div id="popOver_Delete_content">
                    <div id="AllDeleteButton_Click">
                        <asp:Label ID="DeleteText" CssClass="clip" runat="server"/>
                    </div>
                </div>
            </div>
            <div class="triangle bottom" class="popover" style="top: -16.5px; left:91px; ">
                <div class="triangleBorder">
                    <div class="triangleInner"></div>
                </div>
            </div>
        </div>

        <div id="btn_CarAria" >      
            <%--$02 start クルクル対応--%>
    	    <%--<div id="btn_car_o" onclick="$('#CarButton').click();"><div class="img_car"  ><img src="../Styles/Images/VisitCommon/icon_car_o.png" width="41" height="36"  alt="" /></div></div>  --%>  
    	    <%--<div id="btn_car" onclick="$('#CarButton').click();"><div class="img_car"  ><img src="../Styles/Images/VisitCommon/icon_car.png" width="41" height="36"  alt=""/></div></div> --%>
    	    <div id="btn_car_o" ><div class="img_car"  ><img src="../Styles/Images/VisitCommon/icon_car_o.png" width="41" height="36"  alt="" /></div></div>    
    	    <div id="btn_car" ><div class="img_car"  ><img src="../Styles/Images/VisitCommon/icon_car.png" width="41" height="36"  alt=""/></div></div>       
            <%--$02 end クルクル対応--%>
        </div>
        <div id="btn_PersonAria" style="position: absolute; left: 0px; top:0px;">        
    	    <%--$02 start クルクル対応--%>
            <%--<div id="btn_person_o" onclick="$('#PersonButton').click();"><div class="img_person"  ><img src="../Styles/Images/VisitCommon/icon_person_o.png"  width="22" height="51"  alt="" /></div></div>  --%>
    	    <%--<div id="btn_person" onclick="$('#PersonButton').click();"><div class="img_person"  ><img src="../Styles/Images/VisitCommon/icon_person.png"  width="22" height="51"  alt=""/></div></div>  --%>
            <div id="btn_person_o" ><div class="img_person"  ><img src="../Styles/Images/VisitCommon/icon_person_o.png"  width="22" height="51"  alt="" /></div></div>  
    	    <div id="btn_person" ><div class="img_person"  ><img src="../Styles/Images/VisitCommon/icon_person.png"  width="22" height="51"  alt=""/></div></div>   
            <%--$02 end クルクル対応--%>
        </div>
        <%-- $08 start 予約一覧ボタンの追加 --%>
        <div id="btn_AppListAria" style="position: absolute; left: 0px; top:0px;">        
            <div id="btn_AppList_o" ><div class="img_AppList"  ><img src="../Styles/Images/VisitCommon/icon_list_on.png"  width="40" height="51"  alt="" /></div></div>
    	    <div id="btn_AppList" ><div class="img_AppList"  ><img src="../Styles/Images/VisitCommon/icon_list.png"  width="40" height="51"  alt=""/></div></div>
        </div>
        <%-- $08 end 予約一覧ボタンの追加 --%>
        <!-- upper block end-->
    
		<!-- Contents block -->
        <%-- 待機画面 --%>
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%-- <div id="dispWait" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:372px"> --%>
        <div id="dispWait" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:562px">
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
            <div id="input_dispWait" class="input"></div>       
			<div id="tableArea_dispWait" class="tableArea"></div>
        </div> 

        <%-- 車両登録番号読取画面 --%>
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>        
        <%-- <div id="vclNoRead"  class="contentsAria" runat="server" style="overflow:hidden ; position:relative; top:85px; width:640px; height:372px;"> --%>
        <div id="vclNoRead"  class="contentsAria" runat="server" style="overflow:hidden ; position:relative; top:85px; width:640px; height:562px;">
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
            <%-- 車両登録番号毎の情報 --%>
            <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
            <%-- <div id="topAria" class="contents" style="position:relative; width:640px; height:372px" > --%>
            <div id="topAria" class="contents" style="position:relative; width:640px; height:562px" >
            <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
                <div id="downCursor" class="flickRelease-icon" runat="server" ><asp:Image ID="Image1" runat="server" ImageUrl="../Styles/Images/SC3090301/downicon.png"  style="width: 40px; height: 40px;" /></div>
                <div id="loadCursor" class="flickRelease-loding" runat="server" ></div>
                <asp:Label ID="pullDownString" CssClass="flickRelease-text" runat="server" style="position: absolute; top: 300px; left: 200px; font-size: 25px; display:none;" />
                <asp:Label ID="releaseString" CssClass="flickRelease-text" runat="server" style="position: absolute; top: 300px; left: 200px; font-size: 25px; display:none;" />
                <asp:Label ID="loadString" CssClass="flickRelease-text" runat="server" style="position: absolute; top: 300px; left: 200px; font-size: 25px; display:none;" />
            </div>
            <asp:Repeater ID="repCar" runat="server">
                <ItemTemplate>
                    <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>        
                    <%-- <div id="contents1" class="contents" style="position:relative; width:640px; height:372px" > --%>
                    <div id="contents1" class="contents" style="position:relative; width:640px; height:562px" >
					<%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
                        <div id="input1" class="input">
                            
                            <asp:HiddenField ID="vclNoIndex" runat="server" Value='<%# Server.HTMLEncode(Container.ItemIndex) %>'/>

                            <!-- 車両登録番号 -->
                            <div id="input_left1" class="input_left">
            	                <div id="txt_car1" class="txt_car">
                                    <%-- $08 start 車両登録番号をフル桁表示する --%>
                                    <%--<icrop:CustomLabel ID="txt_car_number1" runat="server" CssClass="txt_car_number" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNO")) %>'></icrop:CustomLabel>--%>
                                    <asp:Label ID="txt_car_number1" runat="server" CssClass="txt_car_number" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VCLREGNO")) %>'></asp:Label>
                                    <%-- $08 end 車両登録番号をフル桁表示する --%>
            	                </div>
                            </div>

                            <%-- 来店時間 --%>
                            <div id="input_right1" runat="server" class="input_right">
                                <icrop:CustomLabel ID="lbl_time1" runat="server" CssClass="lbl_time" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "VISITTIMESTAMP","{0:HH:mm}")) %>'></icrop:CustomLabel>
                            </div>
                                                    
                            <!-- スクロールランプ -->
                            <asp:Panel ID="lampOddAria" class="lampAria" runat="server">
                                <div id="input_lamp_odd16" class="input_lamp_odd16"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd16_n" class="input_lamp_odd16" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd14" class="input_lamp_odd14"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd14_n" class="input_lamp_odd14" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd12" class="input_lamp_odd12"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd12_n" class="input_lamp_odd12" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd10" class="input_lamp_odd10" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd10_n" class="input_lamp_odd10" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd8" class="input_lamp_odd8"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd8_n" class="input_lamp_odd8" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd6" class="input_lamp_odd6"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd6_n" class="input_lamp_odd6" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd4" class="input_lamp_odd4"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd4_n" class="input_lamp_odd4" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd2" class="input_lamp_odd2"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd2_n" class="input_lamp_odd2" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd1" class="input_lamp_odd1" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd1_n" class="input_lamp_odd1" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd3" class="input_lamp_odd3"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd3_n" class="input_lamp_odd3" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd5" class="input_lamp_odd5"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd5_n" class="input_lamp_odd5" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd7" class="input_lamp_odd7"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd7_n" class="input_lamp_odd7" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd9" class="input_lamp_odd9"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd9_n" class="input_lamp_odd9" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd11" class="input_lamp_odd11" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd11_n" class="input_lamp_odd11" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd13" class="input_lamp_odd13"  runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd13_n" class="input_lamp_odd13" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd15" class="input_lamp_odd15" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd15_n" class="input_lamp_odd15" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_odd17" class="input_lamp_odd17" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_odd17_n" class="input_lamp_odd17" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                            </asp:Panel>
                            
                            <asp:Panel ID="lampEvenAria" class="lampAria" runat="server">
                                <div id="input_lamp_even16" class="input_lamp_even16" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even16_n" class="input_lamp_even16" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even14" class="input_lamp_even14" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even14_n" class="input_lamp_even14" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even12" class="input_lamp_even12" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even12_n" class="input_lamp_even12" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even10" class="input_lamp_even10" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even10_n" class="input_lamp_even10" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even8" class="input_lamp_even8" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even8_n" class="input_lamp_even8" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even6" class="input_lamp_even6" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even6_n" class="input_lamp_even6" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even4" class="input_lamp_even4" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even4_n" class="input_lamp_even4" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even2" class="input_lamp_even2" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even2_n" class="input_lamp_even2" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even1" class="input_lamp_even1" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even1_n" class="input_lamp_even1" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even3" class="input_lamp_even3" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even3_n" class="input_lamp_even3" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even5" class="input_lamp_even5" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even5_n" class="input_lamp_even5" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even7" class="input_lamp_even7" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even7_n" class="input_lamp_even7" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even9" class="input_lamp_even9" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even9_n" class="input_lamp_even9" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even11" class="input_lamp_even11" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even11_n" class="input_lamp_even11" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even13" class="input_lamp_even13" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even13_n" class="input_lamp_even13" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                                <div id="input_lamp_even15" class="input_lamp_even15" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp_n.png" alt=""/></div>
                                <div id="input_lamp_even15_n" class="input_lamp_even15" style="display:none" runat="server" visible="false"><img src="../Styles/Images/SC3090301/gate_lamp.png" alt=""/></div>
                            </asp:Panel>
                        </div>

                        <%-- 顧客毎の情報 --%>
    			        <div id="tableArea1" class="tableArea">
		                    <div id="slidemask1" class="slidemask">
			                    <ul id="photo1" class="photo">
                                    <asp:Repeater ID="repCustomer" runat="server" datasource='<%# Container.DataItem.Row.GetChildRows("relation") %>'>
                                        <ItemTemplate>
	   		                                <!-- tableArea -->
                                            <asp:HiddenField ID="visitVclSeq" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[VISITVCLSEQ]")) %>' />
                                            <asp:HiddenField ID="visitTimestamp" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[VISITTIMESTAMP]")) %>' />
                                            <asp:Label ID="vclRegNo" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[VCLREGNO]")) %>' style="display:none;" />
                                            <asp:HiddenField ID="custCount" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTCOUNT]")) %>'/>
                                            <asp:Label ID="name" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NAME]")) %>' style="display:none;" />
                                            <asp:Label ID="nameTitle" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NAMETITLE]")) %>' style="display:none;"/>
                                            <asp:Label ID="nameDisp" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NAMEDISP]")) %>' style="display:none;" />
                                            <asp:Label ID="nameTitleDisp" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NAMETITLEDISP]")) %>' style="display:none;"/>
                                            <asp:HiddenField ID="custKbn" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTKBN]")) %>'/>
                                            <%-- $01 start step2開発 --%>
                                            <%-- <asp:HiddenField ID="custPic" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTPIC]")) %>' /> --%>
                                            <%-- $01 end step2開発 --%>
                                            <asp:Label ID="carInfo1" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CARINFO1]")) %>' style="display:none;" />
                                            <asp:Label ID="carInfo2" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CARINFO2]")) %>' style="display:none;" />
                                            <%-- $06 start TMEJ次世代サービス 工程管理機能開発 --%>
                                            <asp:Label ID="provinceName" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[PROVINCE]")) %>' style="display:none;"/>
                                            <%-- $06 end   TMEJ次世代サービス 工程管理機能開発 --%>
                                            <%-- $01 start step2開発 --%>
                                            <%-- <asp:HiddenField ID="carPic" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CARPIC]")) %>' /> --%>
                                            <%-- $01 end step2開発 --%>
                                            <asp:HiddenField ID="custCd" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTCD]")) %>' />
                                            <asp:HiddenField ID="stuffCd" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[STUFFCD]")) %>' />
                                            <asp:HiddenField ID="sex" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[SEX]")) %>' />
                                            <asp:HiddenField ID="vin" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[VIN]")) %>' />
                                            <asp:HiddenField ID="seqNo" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[SEQNO]")) %>' />
                                            <asp:HiddenField ID="saCode" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[SACODE]")) %>' />
                                            <%-- $03 start ウェルカムボード仕様変更対応 --%>
                                            <asp:HiddenField ID="custType" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTYPE]")) %>' />
                                            <%-- $03 end ウェルカムボード仕様変更対応 --%>
                                            <%-- $07 start UAT課題#158 --%>
                                            <asp:HiddenField ID="reservFlg" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[RESERVFLG]")) %>' />
                                            <%-- $07 end UAT課題#158 --%>
                                            <div id="custInfo" runat="server">

            			                    <li id="slide01" class="slide">
                                            <asp:HiddenField ID="custIndex" runat="server" Value='<%# Server.HTMLEncode(Container.ItemIndex) %>'/>
                			                    <table id="tableUp" runat="server">
                				                    <tr>
                                                        <%-- $01 start step2開発 --%>
                   					                    <%-- <td width="240px"><asp:Image ID="imgCar" runat="server" ImageUrl='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CARPIC]")) %>' width="240px" height="130px" alt="" /></td> --%>
                                                        <td width="60px"></td>
                                                        <%-- $01 end step2開発 --%>
                        				                <td>
                                                        <%-- $08 start 車両情報をフル桁で表示 --%>
                                                        <%--<icrop:CustomLabel ID="carInfo" runat="server" CssClass="ul.photo li span" Text=""></icrop:CustomLabel>--%>
                                                        <asp:Label ID="carInfo" runat="server" Class="CarInfoLabelWidth" Text="" />
                                                        <%-- $08 end 車両情報をフル桁で表示 --%>
                                                        <%-- $06 start TMEJ次世代サービス 工程管理機能開発  --%>
                                                        <icrop:CustomLabel ID="province" runat="server" class="span_province" Text=""></icrop:CustomLabel>
                                                        <%-- $06 end TMEJ次世代サービス 工程管理機能開発 --%>
                                                        </td>
                                                        <%-- $07 start UAT課題#158 --%>
                                                        <td width="105px"><icrop:CustomLabel ID="appointmentIcon" runat="server" class="appointmentIcon" Text=""></icrop:CustomLabel></td>
                                                        <%-- $07 end UAT課題#158 --%>
                    				                </tr>
                    			                </table>
                    			                <hr id="tableLine" runat="server"  class="hr_table" />
                    			                <table id="tableBottom" runat="server">
                    				                <tr>
                                                        <%-- $01 start step2開発 --%>
                    					                <%-- <td width="240px"><asp:Image ID="imgCust" runat="server" ImageUrl='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[CUSTPIC]")) %>' style="position:absolute;top:150px; left:75px;" width="90px" height="100px"  alt="" /></td> --%>
                                                        <td width="60px"></td>
                                                        <%-- $01 end step2開発 --%>
                                                        <%-- $08 start 顧客名をフル桁で表示 --%>
                            				            <%-- <td><icrop:CustomLabel ID="custName" runat="server" CssClass="ul.photo li span" Text=""></icrop:CustomLabel></td> --%>
                                                        <td><asp:Label ID="custName" runat="server" Text="" class="CustomerLabelWidth" ></asp:Label></td>
                                                        <%-- $08 end 顧客名をフル桁で表示 --%>
                        			                </tr>
                    			                </table>        
                    							<%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
                                                <%-- <asp:Label id="noVclInfo" runat="server" Visible="false" style=" position:absolute; top:111px; width: 640px; text-align: center; height: 280px;"></asp:Label> --%>
                                                <asp:Label id="noVclInfo" runat="server" Visible="false" style=" position:absolute; top:180px; width: 640px; text-align: center; height: 280px;"></asp:Label>
												<%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
                		                    </li>
                                            </div>
                                            <div id="noCustInfo" runat="server">
                                            <li id="slide02" class="slide">
            	                                <div id="img_newCustomerPic" style="position:relative; top:90px; height:90px">
                    								<%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
                                                    <%-- <div id="imgCustomerPic" style="position:absolute; left:150px"> --%>
                                                    <div id="imgCustomerPic" style="position:absolute; top:65px; left:100px">
                    								<%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
					                                    <img src="../Styles/Images/VisitCommon/icon_newCust.png" alt=""  width="83" height="83"  />
                                                    </div>
                              						<%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
                                                    <%-- <icrop:CustomLabel ID="newCustomerWord" runat="server" Text="" style="position: absolute; top:20px; left:250px; height:60px; color: #6E6E6E; text-shadow: 0px -1px 2px #777777;"></icrop:CustomLabel> --%>
                                                    <icrop:CustomLabel ID="newCustomerWord" runat="server" Text="" style="position: absolute; top:90px; left:200px; height:60px; color: #6E6E6E; text-shadow: 0px -1px 2px #777777;"></icrop:CustomLabel>
                    								<%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
				                                </div>
                		                    </li>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                    		    </ul>
                    	    </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div id="bottomAria" class="contents" style="position:relative; width:640px; height:372px" >
            </div>
        </div>       
        <%-- 新規登録画面[車]画面 --%>
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%-- <div id="newCar" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:372px"> --%>
        <div id="newCar" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:562px">
		<%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
	    	<div id="input_newCar" class="input">

                <%-- $08 start 車両登録番号を手入力できる機能の追加 --%>
                <%-- 車両登録番号 --%>
                <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
                <%-- <div id="input_left_newCar" class="input_left" style="position:absolute; top:18px; left:10px"> --%>
                <%--     <icrop:CustomTextBox ID="RegNumTxt" runat="server" Width="400px" Height="50px" Font-Size="38px" MaxLength = "32" PlaceHolderWordNo="19" ></icrop:CustomTextBox> --%>
                <%-- </div> --%>
                <div id="input_left_newCar" class="input_left" style="position:absolute; top:28px; left:10px">
                    <icrop:CustomTextBox ID="RegNumTxt" runat="server" Width="400px" Height="50px" Font-Size="44px" MaxLength = "32" PlaceHolderWordNo="19" ></icrop:CustomTextBox>
                </div>
                <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
                <%-- $08 end 車両登録番号を手入力できる機能の追加 --%>

                <%-- 来店時間 --%>
                <div id="input_right_newCar" class="input_right">
                    <icrop:CustomLabel ID="lbl_time_newCar" runat="server" CssClass="lbl_time"></icrop:CustomLabel>
                </div>
            </div>

			<div id="img_contents1" class="img_contents">
            	<div id="img_bigcar">
					<img src="../Styles/Images/SC3090301/gate_bigcar.png" alt="" />
				</div>
            </div>
        </div> 
        <%-- 新規登録画面[歩き]画面 --%>
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%-- <div id="newWalk" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:372px"> --%>
        <div id="newWalk" class="contentsAria" runat="server" style="overflow:hidden; position:relative; top:85px; width:640px; height:562px">
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
            <div id="input_newWalk" class="input">
                <%-- 来店時間 --%>
                <div id="input_right_newWalk" class="input_right">
                    <icrop:CustomLabel ID="lbl_time_newWalk" runat="server" CssClass="lbl_time"></icrop:CustomLabel>
                </div>
            </div>       

			<div id="img_contents2" class="img_contents">
            	<div id="img_bigper">
					<img src="../Styles/Images/SC3090301/gate_bigper.png" alt="" />
				</div>
            </div>
        </div>

         <%-- $05 start前のデータ表示 --%>
        <div ID="lbl_PreviewDataCount" runat="server" style="display:none">
        <asp:Label ID="Text_PreviewDataCount" class ="clip" runat="server" Text=""/>
        </div>
         <%-- $05 end 前のデータ表示 --%>
         <%-- %05 start 次のデータ表示 --%>
        <div ID="lbl_NextDataCount" runat="server" style="display:none">
                <asp:Label ID="Text_NextDataCount" class ="clip"  runat="server" Text=""/>
        </div>
         <%-- %05 end 次のデータ表示 --%>

        <%-- 未送信データ件数 --%>
        <icrop:CustomLabel ID="lbl_unSendCount" runat="server" Text="" />




		<!-- Contents block end -->

        <!-- Bottom block -->
       	<!-- button_1-5 -->
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%-- <div id="number"> --%>
        <div id="number" style="display:none;">
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>
            <div id="btn_01" class="ninzuButton">
                <div id="lbl_01">1</div>
            </div>
            <div id="btn_01_o" class="ninzuButton" style="display:none">
                <div id="lbl_01_o">1</div>
            </div>
            <div id="btn_01_n" class="ninzuButton" style="display:none">
                <div id="lbl_01_n">1</div>
            </div>
            <div id="btn_02" class="ninzuButton">
                <div id="lbl_02">2</div>
            </div>
            <div id="btn_02_o" class="ninzuButton" style="display:none">
                <div id="lbl_02_o">2</div>
            </div>
            <div id="btn_02_n" class="ninzuButton" style="display:none">
                <div id="lbl_02_n">2</div>
            </div>
            <div id="btn_03" class="ninzuButton">
                <div id="lbl_03">3</div>
            </div>
            <div id="btn_03_o" class="ninzuButton" style="display:none">
                <div id="lbl_03_o">3</div>
            </div>
            <div id="btn_03_n" class="ninzuButton" style="display:none">
                <div id="lbl_03_n">3</div>
            </div>
            <div id="btn_04" class="ninzuButton">
                <div id="lbl_04">4</div>
            </div>
            <div id="btn_04_o" class="ninzuButton" style="display:none">
                <div id="lbl_04_o">4</div>
            </div>
            <div id="btn_04_n" class="ninzuButton" style="display:none">
                <div id="lbl_04_n">4</div>
            </div>
            <div id="btn_05" class="ninzuButton">
                <div id="lbl_05">5</div>
            </div>
            <div id="btn_05_o" class="ninzuButton" style="display:none">
                <div id="lbl_05_o">5</div>
            </div>
            <div id="btn_05_n" class="ninzuButton" style="display:none">
                <div id="lbl_05_n">5</div>
            </div>
        </div>

        <!-- new_button -->
        <div id="purpose">
        	<div id="btn_new" class="purposeButton">
                <div style="display:none">1</div>
          		<div id="img_new">
            		<img src="../Styles/Images/SC3090301/gate_new.png" width="106" height="69"  alt=""/>
              	</div>
	    	</div>
            <div id="btn_new_o" class="purposeButton" style="display:none">
                <div style="display:none">1</div>
          	    <div id="img_new_o">
            	    <img src="../Styles/Images/SC3090301/gate_new_o.png" width="106" height="69"  alt=""/>
              	</div>
	    	</div>
            <div id="btn_new_n" class="purposeButton" style="display:none">
                <div style="display:none">1</div>
          	    <div id="img_new_n">
            	    <img src="../Styles/Images/SC3090301/gate_new.png" width="106" height="69"  alt=""/>
               	</div>
	    	</div>
    
            <!-- repair_button -->
            <div id="btn_repair" class="purposeButton">
                <div style="display:none">2</div>
              	<div id="img_repair" class="Shape 6 Rs"  >
                	<img src="../Styles/Images/SC3090301/gate_repair.png" width="83" height="83"  alt=""/>
          		</div>
    		</div>
            <div id="btn_repair_o" class="purposeButton" style="display:none">
                <div style="display:none">2</div>
              	<div id="img_repair_o">
                	<img src="../Styles/Images/SC3090301/gate_repair_o.png" width="83" height="83"  alt=""/>
          		</div>
	    	</div>
            <div id="btn_repair_n" class="purposeButton" style="display:none">
                <div style="display:none">2</div>
              	<div id="img_repair_n">
                	<img src="../Styles/Images/SC3090301/gate_repair.png" width="83" height="83"  alt=""/>
          		</div>
			</div>
        
       		<!-- other_button -->
            <div id="btn_other" class="purposeButton">
            　　<div style="display:none">3</div>
            	<div id="img_other">
                	<img src="../Styles/Images/SC3090301/gate_other.png" width="77" height="17"  alt=""/>
          	    </div>
       		</div>
            <div id="btn_other_o" class="purposeButton" style="display:none">
                <div style="display:none">3</div>
              	<div id="img_other_o">
                	<img src="../Styles/Images/SC3090301/gate_other_o.png" width="77" height="17"  alt=""/>
          		</div>
    		</div>
            <div id="btn_other_n" class="purposeButton" style="display:none">
                <div style="display:none">3</div>
              	<div id="img_other_n">
                	<img src="../Styles/Images/SC3090301/gate_other.png" width="77" height="17"  alt=""/>
          		</div>
			</div>
        </div>

        <!-- submit button -->
        <%--$02 start クルクル対応--%>
        <!--<icrop:CustomButton ID="btn_submit_o" runat="server" Width="586" Height="83"  Text="" style="position:absolute; left:28px; top:356px; display:none" /> -->
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%--<div id="btn_submit_o" style="position:absolute; left:28px; top:356px; display:none" >--%>
        <%--    <icrop:CustomLabel ID="wordSubmit_o" CssClass="lbl_submit" runat="server" Text="" />--%>
		<%--</div>--%>
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>

        <%--$02 start クルクル対応--%>
        <%-- $09 start Gate Keeper機能の視認性操作性改善 --%>
        <%-- <div id="btn_submit_n"> --%>
        <%--     <icrop:CustomLabel ID="wordSubmit_n" CssClass="lbl_submit" runat="server" Text="" /> --%>
		<%-- </div> --%>
        <%-- $09 end Gate Keeper機能の視認性操作性改善 --%>

        <!-- Bottom block end-->

  	</div>
</asp:Content>
