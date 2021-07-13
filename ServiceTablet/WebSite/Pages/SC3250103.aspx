<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false"  CodeFile="SC3250103.aspx.vb" Inherits="SC3250103"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3250103/SC3250103.css?201906060000" type="text/css" media="screen,print" />
    <script type="text/javascript" src="../Scripts/SC3250103/SC3250103.js?201409250001"></script>	
    <script type="text/javascript" src="../Scripts/SC3250103/SC3250103.MainMenuFingerscroll.js?201408060000"></script>
</asp:Content>

<asp:Content ID="cont_content" ContentPlaceHolderID="content" Runat="Server">
    <!-- HiddenField宣言 start -->
    <asp:HiddenField ID="hdnContent1_PageId" runat="server"/>
    <asp:HiddenField ID="hdnDisplayFlag" runat="server"/>
    <asp:HiddenField ID ="hdnContent2URL" runat="server" />
    <asp:HiddenField ID ="hdnContent3URL" runat="server" />
    <asp:HiddenField ID ="hdnContent4URL" runat="server" />
    <asp:HiddenField ID ="hdnContent5URL" runat="server" />
    <!-- HiddenField宣言 end -->

    <!-- 中央部分-->
    <div id="mainScroll">
        <div id="contentsMain">
            <div id="contentsBox">
                <div id="contentsArea">
                    <div id="contentsTitle">
                        <h3><Label ID="InspecItemName2" runat="server" /></h3>
                    </div>

                    <!--1つめのコンテンツブロック-->
                    <div id="ContentBox1" class="pdcontentBox ShowContents1Before" runat="server">
                        <div style="position:relative;" onclick="ClickContentHeader('1');">
                            <h4><icrop:CustomLabel ID="Content1_Title" runat="server" TextWordNo="2" Width="900px" Height="30px" UseEllipsis="true"/></h4>
                        </div>
	            	    <div style="position:relative; width:100%; height:733px; overflow:hidden;">
                            <iframe id="iFrame1" class="ContentsFrame" runat="server" style="transform: scale(0.97,1) translate(-16px,0px);"></iframe>
                            <div id="ServerProcessIcon1" runat="server"></div>
                	        <!--<div id="Cover1" style="position:absolute; top:0px; left:0px; width:100%; height:100%;" onTouchStart="IsTouch=true" onTouchMove="IsTouch=false" onTouchEnd="CoverTouchEnd('Cover1')"></div>-->
            		    </div>
                    </div>

                    <!--2つめのコンテンツブロック-->
                    <div id="ContentBox2" class="pdcontentBox ShowContents2" runat="server" style="display:none;">
                        <h4 onclick="ClickContentHeader('2');"><icrop:CustomLabel ID="Content2_Title" runat="server" TextWordNo="3" Width="900px" Height="30px" UseEllipsis="true"/></h4>
	            		<div style="position:relative; width:100%; height:100%;">
                            <div style="position:relative;"><iframe id="iFrame2" class="ContentsFrame" runat="server"></iframe></div>
                            <div id="ServerProcessIcon2" runat="server"></div>
                			<!--<div id="Cover2" style="position:absolute; top:0px; left:0px; width:100%; height:100%;" onTouchStart="IsTouch=true" onTouchMove="IsTouch=false" onTouchEnd="CoverTouchEnd('Cover2')"></div>-->
            			</div>
                    </div>

                    <!--3つめのコンテンツブロック-->
                    <div id="ContentBox3" class="pdcontentBox ShowContents3" runat="server" style="display:none;">
                        <h4 onclick="ClickContentHeader('3');"><icrop:CustomLabel ID="Content3_Title" runat="server" TextWordNo="4" Width="900px" Height="30px" UseEllipsis="true"/></h4>
	            		<div style="position:relative; width:100%; height:100%;">
                            <div style="position:relative;"><iframe id="iFrame3" class="ContentsFrame" runat="server"></iframe></div>
                            <div id="ServerProcessIcon3" runat="server"></div>
                			<!--<div id="Cover3" style="position:absolute; top:0px; left:0px; width:100%; height:100%;"></div>-->
            			</div>
                    </div>

                    <!--4つめのコンテンツブロック-->
                    <div id="ContentBox4" class="pdcontentBox ShowContents4" runat="server" style="display:none;">
                    	<h4 onclick="ClickContentHeader('4');"><icrop:CustomLabel ID="Content4_Title" runat="server" TextWordNo="5" Width="900px" Height="30px" UseEllipsis="true"/></h4>
	            		<div style="position:relative; width:100%; height:100%;">
                            <div style="position:relative;"><iframe id="iFrame4" class="ContentsFrame" runat="server"></iframe></div>
                            <div id="ServerProcessIcon4" runat="server"></div>
                			<!--<div id="Cover4" style="position:absolute; top:0px; left:0px; width:100%; height:100%;" onTouchStart="IsTouch=true" onTouchMove="IsTouch=false" onTouchEnd="CoverTouchEnd('Cover4')"></div>-->
            			</div>
                    </div>

                    <!--5つめのコンテンツブロック-->
                    <div id="ContentBox5" class="pdcontentBox ShowContents5" runat="server" style="display:none;">
                    	<h4 onclick="ClickContentHeader('5');"><icrop:CustomLabel ID="Content5_Title" runat="server" Width="900px" Height="30px" UseEllipsis="true"/></h4>
	            		<div style="position:relative; width:100%; height:100%;">
                            <div style="position:relative;">
                            <iframe id="iFrame5" class="ContentsFrame" runat="server" style="transform: scale(0.97,1) translate(-16px,0px);"></iframe>
                            </div>
                            <div id="ServerProcessIcon5" runat="server"></div>
                			<!--<div id="Cover5" style="position:absolute; top:0px; left:0px; width:100%; height:100%;" onTouchStart="IsTouch=true" onTouchMove="IsTouch=false" onTouchEnd="CoverTouchEnd('Cover5')"></div>-->
            			</div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- ここまで中央部分 -->
</asp:Content>

<asp:Content ID="cont_footer" ContentPlaceHolderID="footer" Runat="Server">
    <div style="margin:0px 0px 0px 925px;">
        <asp:Button ID="ButtonCart" runat="server" BackColor="#3333FF" Font-Size="Medium" ForeColor="White" Height="42px" Width="80px" style="display:none;" />
        <ul>
            <li style="float:left;margin-right:5px;">
                <div ID="imgCart" class="Cart_Enable" onclick="OnClickCart();" runat="server" />
            </li>
        </ul>
	</div>
</asp:Content>
