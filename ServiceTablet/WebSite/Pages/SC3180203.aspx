<%@ Page Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false"
    CodeFile="SC3180203.aspx.vb" Inherits="Pages_SC3180203" %>

    <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

        <!-- ページ毎CSS -->
        <link rel="stylesheet" href="../Styles/SC3180203/SC3180203.css?201911270000" type="text/css" media="screen,print" />
        <link rel="stylesheet" href="../Styles/SC3180203/ssa04b.css?201905210000" type="text/css" media="screen,print" />
        <!-- API -->
        <script src="../Scripts/icrop.clientapplication.js?20140612000000" type="text/javascript"></script>
        <script src="../Scripts/icrop.push.js?20140612000000" type="text/javascript"></script>

        <script src="../Scripts/SC3180203/SC3180203.js?20120902000002" type="text/javascript"></script>
        <script language="javascript" type="text/javascript">
            $(function () { 
                $("#main1").fingerScroll(); 

                // ---------------------
                // 初期化処理
                // ---------------------
// 2014/07/09 タイトルをデザイン固定にするため削除
//                // タイトルの初期化
//                $("p.TitleText1").html(" ");
//                $("p.TitleText2").html(" ");

                // ラベルの初期化
                $("div[id^=_SILABEL_]").html("");
                $("div[id^=_SITEXT_]").html("");

                // アイコンの初期化
                $("p[id^=_SIVAL_]").removeAttr("class");

                // 全要素にHOFF・DOFFの適用
                $("div.CassetteTitle").each(function(){
                    if ($(this).hasClass("HON") == true) {
                        $(this).removeClass("HON");
                    }
                    if ($(this).hasClass("HOFF") == false) {
                        $(this).addClass("HOFF");
                    }
                });

                $("li[class^=Cassette]").each(function(){
                    if ($(this).hasClass("DON") == true) {
                        $(this).removeClass("DON");
                    }
                    if ($(this).hasClass("DOFF") == false) {
                        $(this).addClass("DOFF");
                    }
                });

// 2014/07/09 タイトルをデザイン固定にするため削除
//                // ---------------------
//                // タイトル設定
//                // ---------------------
//                function TitleLabelSetting(code, title) {
//                    if (typeof code === 'undefined' || typeof title === 'undefined') {
//                        return false;
//                    }

//                    code = parseInt(code, 10)

//                    var silbl = $("#_SILABEL_" + code);

//                    if (silbl.length == 0) {
//                        // 指定のコードに対応する要素が存在しない場合は処理しない。
//                        return false;
//                    } else {
//                        var si_title = silbl.parent().parent().find("p.TitleText1");
//                        if (si_title.length == 1) {
//                            // タイトルの適用
//                            si_title.html(title);
//                        } else {
//                            return false;
//                        }

//                    }

//                    return true;
//                }

// 2014/07/09 背景色設定の方法変更
                // ---------------------
                // 背景色（スタイル）設定
                // ---------------------
//                function TitleSetting(code) {
//                    if (typeof code === 'undefined') {
//                        return false;
//                    }

//                    code = parseInt(code, 10)

//                    var silbl = $("#_SILABEL_" + code);

//                    if (silbl.length == 0) {
//                        // 指定のコードに対応する要素が存在しない場合は処理しない。
//                        return false;
//                    } else {
//                        var si_title = silbl.parent().parent().find("p.TitleText1");
//                        if (si_title.length == 1) {
//                            // 背景色（style）の適用
//                            si_title.parent().removeClass("HOFF");
//                            si_title.parent().addClass("HOFF");
//                            si_title.parent().parent().removeClass("DOFF");
//                            si_title.parent().parent().addClass("DON");
//                            return false;
//                        }

//                    }

//                    return true;
//                }

                function TitleBackGroundSetting() {
                    //Cassette～から始まるliタグをループ処理
                    $("li[class^=Cassette]").each(function () {
                        //ColumnText01を探す
                        var silbls = $(this).find("div.ColumnText01,div.ColumnText01a")
                        var cnt = 0;
                        //各要素のテキストの長さを判定し、データがセットされていたらカウントアップする
                        for (var i = 0, len = silbls.length; i < len; ++i) {
                            if (silbls[i].innerText != "") {
                                cnt += 1;
                            };
                        };
                        //サブ点検項目が1個以上あったときは背景を白に変更する
                        if (cnt > 0) {
                            // 背景色(style)の適用
                            $(this).removeClass("DOFF");
                            $(this).addClass("DON");
                            $(this).find("div.CassetteTitleCyan").removeClass("HOFF");
                            $(this).find("div.CassetteTitleCyan").addClass("HON");
                        } else {
                            return true;//false; 2015/01/30 背景グレー表示不具合修正
                        };
                    });
                    return true;
                }

                // ---------------------
                // ラベル設定
                // ---------------------
                function LabelsSetting(code, label, text, val) {
                    if (typeof code === 'undefined' || typeof label === 'undefined') {
                        return false;
                    }
                    if (typeof text === 'undefined') {
                        text = " ";
                    }
                    if (typeof val === 'undefined') {
                        val = "";
                    }

                    code = parseInt(code, 10)

                    var silbl = $("#_SILABEL_" + code);
                    var sitxt = $("#_SITEXT_" + code);
                    var sival = $("#_SIVAL_" + code);

                    if (silbl.length == 0) {
                        // 指定のコードに対応する要素が存在しない場合は処理しない。
                        return false;
                    } else {
                        // ラベルのセット
                        silbl.html(label);
                        //$("#_SILABEL_" + code).html(label);
                    }


                    if (sitxt.length == 1) {
                        // テキストのセット
                        sitxt.html(text);
                    }

                    if (sival.length == 1 || val != "") {
                        // 値のセット(アイコンのセット)
                        sival.removeClass();
                        sival.addClass(val);
                        //sival.css("background-image", val);
                        //sival.attr("style", val);
                    }

                    return true;
                }

        	    // 2014/07/09 タイトルをデザイン固定にするため DetailTitleData 削除 -->
                function dataInit() {
                    <%= HeaderData%>
                    <%= DetailData%>
                }

                dataInit();
            });

        function print() {
            $("body").fingerScroll({ startEvent: "mousedown touchstart" });
          
            document.getElementById("printDiv").style.visibility="hidden";
            // APIを使用して印刷
            var ret = icrop.clientapplication.print({
                Orientation : 1,
                Color:"YES",
                Target : "InsidePopupWebView",  
                Size : "A4",
				Margin : "5mm/5mm/5mm/5mm",
                HtmlWidth : "1024px"
            });


        }

        function ClosePopup() {
            window.close; 
            return false;
        }
    
        </script>
        <!-- printDivの印刷時非表示指定 -->
        <style type="text/css" media="print">
	            #printDiv {visibility:hidden;}
            </style>
    </asp:Content>
    <asp:Content ID="content2" ContentPlaceHolderID="content" runat="server">
    <!-- 中央部分-->
    <div id="main" class="PrintType">
<!--        <div id="printDiv" class="PrintBotton" style="z-index:101; top:0px; left:470px;  "> -->
        <div id="printDiv" class="PrintBotton" style="z-index:101; top:0px; margin-left:auto; margin-right:auto;  ">
		<a href="javascript:print()" style="color:White;">
			<icrop:CustomLabel ID="CustomLabel2" runat="server" Text="" TextWordNo="19">
			</icrop:CustomLabel>
		</a>
	</div>

	    <!-- ここからコンテンツ -->
	    <div id="contents2">
               </div>
               
            <div id="main1" class="ssa04Base ssa04popWindow" style="z-index:100">

                <!-- 閉じるボタン（必要に応じてトリ） 
                <%--<div class="closeBox">&nbsp;</div>--%>
               <div id="closeButton" class="closeBox"><p class="mainblockDialogBoxClose">
					<asp:LinkButton runat="server" ID="btnClose" Text="" Height="30px" Width="30px"/>
				</p></div> -->

                <!-- テンプレ読み込み位置 -->
                <%=TemplateString %>

	            <asp:HiddenField ID="textmsg" runat="server" />
			    <asp:HiddenField ID="isshow" runat="server" />
			    <asp:HiddenField ID="Yearhidden" runat="server" />
			    <asp:HiddenField ID="Monthhidden" runat="server" />
			    <asp:HiddenField ID="Dayhidden" runat="server" />
			    <asp:HiddenField ID="HourHidden" runat="server" />
			    <asp:HiddenField ID="MinuteHidden" runat="server" />
                <asp:HiddenField ID="vinNoHiddenField" runat="server" />
                <asp:HiddenField ID="resultAddStautsHiddenField" runat="server" />
	      </div>
        
        <!-- ここまでコンテンツ -->
    </div>

    <div style="display:none;">
        <asp:Button ID="BtnLeft" runat="server" />
        <asp:Button ID="BtnRight" runat="server"/>
    </div>
     
<!-- ここまでメインブロック -->
<div style="display:none;"><asp:TextBox ID="HiddenText" runat="server" Text=""></asp:TextBox></div>

<%'サーバー処理中のオーバーレイとアイコン %>
<div id="serverProcessOverlayBlack"></div>
<div id="serverProcessIcon"></div>
</asp:Content>
<%--
<asp:Content ContentPlaceHolderID="footer" ID="contentfooter" runat="server">
    <!-- ここからフッタ -->
     <div id="FooterCustomButton" style="float:right; margin-right:20px;">
            <asp:Button ID="BtnEdit" runat="server" BackColor="#3333FF" Font-Size="Medium" ForeColor="White" Height="42px" Width="80px"  />
            <asp:Button ID="BtnPrint" runat="server" BackColor="#3333FF" Font-Size="Medium" ForeColor="White" Height="42px" Width="80px" />
            <asp:Button ID="BtnRO" runat="server" BackColor="#3333FF" Font-Size="Medium" ForeColor="White" Height="42px" Width="80px" />
     </div>
</asp:Content>
--%>