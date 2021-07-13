<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3290101.ascx.vb" Inherits="Pages_SC3290101_Control" %>

<%' スタイルシート %>
<link type="text/css" href="../Styles/SC3290101/SC3290101.css?20140718000001" rel="stylesheet" />

<%'スクリプト(画面固有) %>
<script type="text/javascript" src="../Scripts/SC3290101/SC3290101_ascx.js?20140718000001"></script>

<div id="SC3290101_Panel" class="contentsFrame02">
    <h2 class="contentTitle"><icrop:CustomLabel ID="SC3290101_Title" runat="server" UseEllipsis="False" width="175px" class="Ellipsis"/></h2>
    <%'更新日時 %>
    <div id="SC3290101_DateBox">
        <icrop:CustomLabel ID="SC3290101_LastUpdateText" runat="server" UseEllipsis="False" />
        <icrop:CustomLabel ID="SC3290101_LastUpdateTime" runat="server" UseEllipsis="False" />
    </div>
    <div id="SC3290101_TBL_Box01">
        <%'ヘッダ部 %>
        <table class="SC3290101_table01" border=0 cellspacing=0 cellpadding=0>
            <tr id="SC3290101_table01HeadR">
                <th class="TBL_HDST">
                    <div class="Hidden" style="width:381px;">
                        <icrop:CustomLabel ID="SC3290101_IrregularityItemTitle" runat="server" UseEllipsis="False" />
                    </div>
                </th>
                <th class="WhiteHD"></th>
                <th class="TBL_HDCT">
                    <div class="Hidden" style="width:118px;">
                        <icrop:CustomLabel ID="SC3290101_NoOfStaffsTitle" runat="server" UseEllipsis="False" />
                    </div>
                </th>
                <th class="WhiteHD"></th>
                <th class="TBL_HDED">
                    <div class="Hidden" style="width:118px;">
                        <icrop:CustomLabel ID="SC3290101_NoOfIrregularitiesTitle" runat="server" UseEllipsis="False" />
                    </div>
                </th>
            </tr>
        </table>
        <%' 読み込み中に操作不可とするため全体に設定する透明なオーバーレイ %>
        <div id="SC3290101_ProgressPanel"></div>
        <%' 処理中のローディング %>
        <div id="SC3290101_LoadingAnimation" class="show2"></div>
        <%'異常リストの検索結果 %>
        <iframe id="SC3290101_iframe" runat="server" src="SC3290101.aspx"></iframe>
        <%' 異常項目無し メッセージ %>
        <div id="SC3290101_ItemNothing">
            <icrop:CustomLabel ID="SC3290101_ItemNotingLabel" runat="server" UseEllipsis="False" class="Ellipsis" />  
        </div>
    </div>
</div>

