<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3070101.ascx.vb" Inherits="Pages_SC3070101"%>

<%'スタイル %>  		  
<link rel="Stylesheet" href="../Styles/SC3070101/SC3070101.css?20130722000001" />
<link rel="Stylesheet" href="../Styles/SC3070101/SC3070101_GL2.css?20131220000001" />


<%'スクリプト %>  	
<script src="../Scripts/SC3070101/SC3070101.js?20140508000001" type="text/javascript"></script>
<script src="../Scripts/SC3100101/jquery.popoverEx.js?20140508000000" type="text/javascript"  ></script>

<asp:UpdatePanel id="UpdateAreaStock" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<%--リスト更新用ボタン--%>
<asp:Button ID="LoadButton" runat="server" Text="" style="display:none;" />
<asp:Button ID="ListUpdateGradeButton" runat="server" style="display:none;"></asp:Button>
<asp:Button ID="ListUpdateButton" runat="server" style="display:none;"></asp:Button>
<%--エラーメッセージ--%>
<asp:HiddenField id="ErrorMessage" runat="server"></asp:HiddenField>
<%--エラーメッセージ送信用ボタン--%>
<asp:Button ID="SendErrorMessageButton" runat="server" style="display:none;"></asp:Button>

<asp:HiddenField id="GradeCodeSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="GradeSuffixSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="GradeSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="SuffixSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="SuffixSearchValueChange" runat="server"></asp:HiddenField>
<asp:HiddenField id="ColorCodeSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="ColorSearchValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="DisplayClassValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="UpDateTypeValue" runat="server"></asp:HiddenField>
<asp:HiddenField id="LoadingFlg" runat="server"></asp:HiddenField>
<asp:HiddenField id="GradeSerchNumber" runat="server"></asp:HiddenField>
<div id="zaikoMain">
<div class="zaikoBoxSetWide">
	<h4><icrop:CustomLabel ID="Lable_ZaikoJokyo" CssClass="EllipsisCat" Width="156px" runat="server"></icrop:CustomLabel></h4>
	<div class="zaiko_bWindowb" id = "zaiko_GL1" runat="server">
	<div class="zaikoContents_Area">
		<ul class="Selection">
		<li class="SelectionButton1">
            <strong><icrop:CustomLabel ID="Lable_Model" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_ModelSearch" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="GradeSelectAria" class="SelectionButton2">
            <strong><icrop:CustomLabel ID="Lable_Grade" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_GradeSearch" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="SuffixSelectAria" class="SelectionButton2">
            <strong><icrop:CustomLabel ID="Lable_Suffix" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_SuffixSearch" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="ColorSelectAria" class="SelectionButton3">
            <strong><icrop:CustomLabel ID="Lable_Color" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_ColorSearch" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
	    </ul>
        
        <div id="GradeWindown" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="GradeWindownBox" data-triggerClientID="trigger1">
                <div class='GradeHadder'>
	                <h3><icrop:CustomLabel ID="Lable_GradeSelect" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="GradeListArea">
                    <div class="Grade-sheet">
                        <div class="Grade-page page0">
		                    <div class="GradeListItemBox">
	                            <ul class="GradeListBoxSetIn1">
                                    <asp:Repeater ID="RepGradeListBox" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="select" >
							                    <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "GradeName")) %>'></icrop:CustomLabel>
                                                <asp:HiddenField ID="GradeCode" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ModelCode")) %>'></asp:HiddenField>
                                                <asp:HiddenField ID="GradeSuffix" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Suffix")) %>'></asp:HiddenField>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="SfxWindown" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="SfxWindownBox" data-triggerClientID="trigger1">
                <div class='SfxHadder'>
	                <h3><icrop:CustomLabel ID="Lable_SuffixSelect" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="SfxListArea">
                    <div class="Sfx-sheet">
                        <div class="Sfx-page page0">
		                    <div class="SfxListItemBox">
	                            <ul class="SfxListBoxSetIn1">
                                    <li class="Arrow" id="select" >
							            <div class="Selection" style="display:none">&nbsp;</div>
                                        <icrop:CustomLabel ID="SuffixListNoSelect" runat="server" CssClass="EllipsisAdd" Width="220px" Text=''></icrop:CustomLabel>
                                    </li>
                                    <asp:Repeater ID="RepSfxListBox" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="select" >
							                    <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "SuffixName")) %>'></icrop:CustomLabel>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="ExteriorWindown" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="ExteriorWindownBox" data-triggerClientID="trigger1">
                <div class='ExteriorHadder'>
	                <h3><icrop:CustomLabel ID="Lable_ColorSelect" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="ExteriorListArea">
                    <div class="Exterior-sheet">
                        <div class="Exterior-page page0">
		                    <div class="ExteriorListItemBox">
	                            <ul class="ExteriorListBoxSetIn1">
                                    <li class="Arrow" id="Li1" >
							            <div class="Selection" style="display:none">&nbsp;</div>
                                        <icrop:CustomLabel ID="ExteriorListNoSelect" runat="server" CssClass="EllipsisAdd" Width="220px" Text=''></icrop:CustomLabel>
                                        <asp:HiddenField ID="ColorCode" runat="server" Value=''></asp:HiddenField>
                                    </li>
                                    <asp:Repeater ID="RepExteriorListBox" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="select" >
							                <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ExteriorColorName")) %>'></icrop:CustomLabel>
                                                <asp:HiddenField ID="ColorCode" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[ExteriorColorCode]")) %>'></asp:HiddenField>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

		<div>
		<table border="0" cellspacing="0" cellpadding="0" class="SelectionList">
		    <tr>
		        <th class="GradeTitle"><div class="Boder" style="width:225px"><icrop:CustomLabel ID="Lable_GradeTitle" CssClass="EllipsisCat" Width="205px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="SfxTitle"><div class="Boder" style="width:121px"><icrop:CustomLabel ID="Lable_SfxTitle" CssClass="EllipsisCat" Width="108px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="OrderTitle"><div class="Boder" style="width:180px"><icrop:CustomLabel ID="Lable_OrderTitle" CssClass="EllipsisCat" Width="168px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="StockTitle"><div class="Boder" style="width:404px"><icrop:CustomLabel ID="Lable_StockTitle" CssClass="EllipsisCat" Width="396px" runat="server"></icrop:CustomLabel></div></th>
	        </tr>
            <tr id="LoadingAria" class="Loading DisplayNone">
                <td style="height:90px">
                    <div class="Loadingicn">
                        <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="30" height="30" alt="" />
                    </div>
                </td>
            </tr>
            <asp:Repeater ID="RepStockListBox" runat="server">   
                <ItemTemplate>
		        <tr id="DetailAria">   
		            <td ID="GradeContent" style="width:225px" runat="server"><icrop:CustomLabel ID="Lable_GradeContent" CssClass="EllipsisAdd" Width="205px" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Model")) %>'></icrop:CustomLabel></td>
		            <td ID="SfxContent" style="width:121px" runat="server"><icrop:CustomLabel ID="Lable_SfxContent"  CssClass="EllipsisAdd" Width="108px" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Suffix")) %>'></icrop:CustomLabel></td>
		            <td ID="OrderContent" style="width:179px; vertical-align: top;" runat="server">
                        <asp:Repeater ID="RepOrderContent" runat="server" datasource='<%# Container.DataItem.Row.GetChildRows("relationOrder") %>'>
                            <ItemTemplate>
                            <p id="TipBlock"  class="icn01" runat="server">
                                <asp:HiddenField ID="DateValue" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NewlyDeliveryDate]")) %>'></asp:HiddenField>
                                <icrop:CustomLabel ID="TipValueColor" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[Color]")) %>'></icrop:CustomLabel>
                                <icrop:CustomLabel ID="TipValueDate" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[NewlyDeliveryDate]","{0:MM/dd}")) %>' style="display:none"></icrop:CustomLabel>
                            </p>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
		            <td ID="StockContent" style="width:400px; vertical-align: top;" runat="server">
                        <asp:Repeater ID="RepStockContent" runat="server" datasource='<%# Container.DataItem.Row.GetChildRows("relationStock") %>'>
                            <ItemTemplate>
                            <p id="TipBlock" class="icn01" runat="server">
                                <asp:HiddenField ID="DateValue" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[AcceptDate]")) %>'></asp:HiddenField>
                                <icrop:CustomLabel ID="TipValueColor" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[Color]")) %>'></icrop:CustomLabel>
                                <icrop:CustomLabel ID="TipValueDate" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[AcceptDate]","{0:MM/dd}")) %>' style="display:none"></icrop:CustomLabel>
                            </p>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
	            </tr>
                </ItemTemplate>
            </asp:Repeater>
	    </table>
		</div>
	</div>
	</div>
  <div class="zaiko_bWindowb" id = "zaiko_GL2" runat="server">
	<div class="zaikoContents_Area">
		<ul class="Selection">
		<li class="SelectionButton1">
            <strong><icrop:CustomLabel ID="Lable_ModelGL2" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_ModelSearchGL2" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="GradeSelectAriaGL2" class="SelectionButton2">
            <strong><icrop:CustomLabel ID="Lable_GradeGL2" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_GradeSearchGL2" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="SuffixSelectAriaGL2" class="SelectionButton2">
            <strong><icrop:CustomLabel ID="Lable_SuffixGL2" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_SuffixSearchGL2" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
		<li id="ColorSelectAriaGL2" class="SelectionButton3">
            <strong><icrop:CustomLabel ID="Lable_ColorGL2" CssClass="EllipsisCat" Width="200px" runat="server"></icrop:CustomLabel></strong>
		    <p><icrop:CustomLabel ID="Lable_ColorSearchGL2" CssClass="EllipsisAdd" Width="200px" runat="server"></icrop:CustomLabel></p>
	    </li>
	    </ul>
        
        <div id="GradeWindownGL2" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="GradeWindownBoxGL2" data-triggerClientID="trigger1">
                <div class='GradeHadder'>
	                <h3><icrop:CustomLabel ID="Lable_GradeSelectGL2" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="GradeListArea">
                    <div class="Grade-sheet">
                        <div class="Grade-page page0">
		                    <div class="GradeListItemBox">
	                            <ul class="GradeListBoxSetIn1">
                                    <asp:Repeater ID="RepGradeListBoxGL2" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="select" >
							                    <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel  runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "GradeName")) %>'></icrop:CustomLabel>
                                                <asp:HiddenField ID="GradeCodeGL2" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ModelCode")) %>'></asp:HiddenField>
                                                <asp:HiddenField ID="GradeSuffixGL2" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Suffix")) %>'></asp:HiddenField>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="SfxWindownGL2" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="SfxWindownBoxGL2" data-triggerClientID="trigger1">
                <div class='SfxHadder'>
	                <h3><icrop:CustomLabel ID="Lable_SuffixSelectGL2" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="SfxListArea">
                    <div class="Sfx-sheet">
                        <div class="Sfx-page page0">
		                    <div class="SfxListItemBox">
	                            <ul class="SfxListBoxSetIn1">
                                    <li class="Arrow" id="selectGL2" >
							            <div class="Selection" style="display:none">&nbsp;</div>
                                        <icrop:CustomLabel ID="SuffixListNoSelectGL2" runat="server" CssClass="EllipsisAdd" Width="220px" Text=''></icrop:CustomLabel>
                                    </li>
                                    <asp:Repeater ID="RepSfxListBoxGL2" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="selectGL2" >
							                    <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "SuffixName")) %>'></icrop:CustomLabel>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="ExteriorWindownGL2" style="display:none" class="popoverEx">
            <div class="triangle"></div>
            <div id="ExteriorWindownBoxGL2" data-triggerClientID="trigger1">
                <div class='ExteriorHadder'>
	                <h3><icrop:CustomLabel ID="Lable_ColorSelectGL2" CssClass="EllipsisCat" Width="144px" runat="server"></icrop:CustomLabel></h3> 
	                </div>
                <div class="ExteriorListArea">
                    <div class="Exterior-sheet">
                        <div class="Exterior-page page0">
		                    <div class="ExteriorListItemBox">
	                            <ul class="ExteriorListBoxSetIn1">
                                    <li class="Arrow" id="Li1" >
							            <div class="Selection" style="display:none">&nbsp;</div>
                                        <icrop:CustomLabel ID="ExteriorListNoSelectGL2" runat="server" CssClass="EllipsisAdd" Width="220px" Text=''></icrop:CustomLabel>
                                        <asp:HiddenField ID="ColorCodeGL2" runat="server" Value=''></asp:HiddenField>
                                    </li>
                                    <asp:Repeater ID="RepExteriorListBoxGL2" runat="server">      
                                        <ItemTemplate>
                                            <li class="Arrow" id="selectGL2" >
							                <div class="Selection" style="display:none">&nbsp;</div>
                                                <icrop:CustomLabel runat="server" CssClass="EllipsisAdd" Width="220px" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "ExteriorColorName")) %>'></icrop:CustomLabel>
                                                <asp:HiddenField ID="ColorCodeGL2" runat="server" Value='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[ExteriorColorCode]")) %>'></asp:HiddenField>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
	                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

		<div>
		<table border="0" cellspacing="0" cellpadding="0" class="SelectionList">
		    <tr>
		        <th class="GradeTitle"><div class="Boder" style="width:225px"><icrop:CustomLabel ID="Lable_GradeTitleGL2" CssClass="EllipsisCat" Width="205px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="SfxTitle"><div class="Boder" style="width:121px"><icrop:CustomLabel ID="Lable_SfxTitleGL2" CssClass="EllipsisCat" Width="108px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="ColorTitle"><div class="Boder" style="width:100px"><icrop:CustomLabel ID="Lable_ColorTitleGL2" CssClass="EllipsisCat" Width="90px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="OrderTitle"><div class="Boder" style="width:238px"><icrop:CustomLabel ID="Lable_OrderTitleGL2" CssClass="EllipsisCat" Width="220px" runat="server"></icrop:CustomLabel></div></th>
		        <th class="StockTitle"><div class="Boder" style="width:238px"><icrop:CustomLabel ID="Lable_StockTitleGL2" CssClass="EllipsisCat" Width="220px" runat="server"></icrop:CustomLabel></div></th>
	        </tr>
            <tr id="LoadingAriaGL2" class="Loading DisplayNone">
                <td style="height:90px">
                    <div class="Loadingicn">
                        <img src="<%=ResolveClientUrl("~/Styles/Images/animeicn-1.png")%>" width="30" height="30" alt="" />
                    </div>
                </td>
            </tr>
            <asp:Repeater ID="RepStockListBoxGL2" runat="server">   
                <ItemTemplate>
		        <tr id="DetailAriaGL2">
		            <td ID="GradeContentGL2" style="width:225px" runat="server"><icrop:CustomLabel ID="Lable_GradeContentGL2" CssClass="EllipsisAdd" Width="205px" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Model")) %>'></icrop:CustomLabel></td>
		            <td ID="SfxContentGL2" style="width:121px" runat="server"><icrop:CustomLabel ID="Lable_SfxContentGL2"  CssClass="EllipsisAdd" Width="108px" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Suffix")) %>'></icrop:CustomLabel></td>
 		            <td ID="BodyColorContentGL2" style="width:100px" runat="server"><icrop:CustomLabel ID="Lable_BodyColorContentGL2"  CssClass="EllipsisAdd" Width="90px" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "Color")) %>'></icrop:CustomLabel></td>
		            <td ID="OrderContentGL2" style="width:240px; vertical-align: top;" runat="server">
                    <p id="OrderTipBlockBlue"  class="icn01" runat="server">
                        <icrop:CustomLabel ID="TipValueNumberOrder1" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[OrderNumber1st]")) %>'></icrop:CustomLabel>
                    </p>
                    <p id="OrderTipBlockYellow"  class="icn02" runat="server">
                        <icrop:CustomLabel ID="TipValueNumberOrder2" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[OrderNumber2nd]")) %>'></icrop:CustomLabel>
                    </p>
                    <p id="OrderTipBlockRed"  class="icn03" runat="server">
                        <icrop:CustomLabel ID="TipValueNumberOrder3" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[OrderNumber3rd]")) %>'></icrop:CustomLabel>
                    </p>
               </td>
                <td ID="StockContentGL2" style="width:240px; vertical-align: top;" runat="server">
                     <p id="StockTipBlockBlue" class="icn01" runat="server">
                        <icrop:CustomLabel ID="TipValueColorStock1" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[StockNumber1st]")) %>'></icrop:CustomLabel>
                     </p>
                     <p id="StockTipBlockYellow" class="icn02" runat="server">
                        <icrop:CustomLabel ID="TipValueColorStock2" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[StockNumber2nd]")) %>'></icrop:CustomLabel>
                     </p>

                     <p id="StockTipBlockRed" class="icn03" runat="server">
                        <icrop:CustomLabel ID="TipValueColorStock3" CssClass="TipValue" runat="server" Text='<%# Server.HTMLEncode(""&DataBinder.Eval(Container.DataItem, "[StockNumber3rd]")) %>'></icrop:CustomLabel>
                     </p>

                </td>
	        </tr>
              </ItemTemplate>
            </asp:Repeater>
	    </table>
		</div>
	</div>
	</div>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>