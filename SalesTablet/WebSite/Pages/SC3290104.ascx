<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SC3290104.ascx.vb" Inherits="Pages_SC3290104_Control" %>

<%' スタイルシート %>
<link rel="stylesheet" href="../Styles/SC3290104/SC3290104.css?20140626000013" type="text/css" />

<%' スクリプト %>
<script type="text/javascript" src="../Scripts/SC3290104/Common.js?20140611000000"></script>
<script type="text/javascript" src="../Scripts/SC3290104/jquery.popoverEx.js?20140611000000"></script>
<script type="text/javascript" src="../Scripts/SC3290104/SC3290104.js?20140716000001"></script>
<script type="text/javascript" src="../Scripts/icrop.push.js?20140731000001"></script>
<script type="text/javascript" src="../Scripts/icrop.clientapplication.js?20140731000001"></script>

<%' 全体のパネル（ポップオーバーの対象） %>
<div id="SC3290104_Panel" runat="server" class="popover">

    <%' ヘッダー %>
    <div id="SC3290104_Header" class='header'>
        <icrop:CustomLabel ID="SC3290104_CancelButton" runat="server" />
        <icrop:CustomLabel ID="SC3290104_Title" runat="server" />
        <icrop:CustomLabel ID="SC3290104_RegistButton" runat="server" />
    </div>

    <%' ボディー（アップデートパネル） %>
    <asp:UpdatePanel ID="SC3290104_Content" runat="server" class='content' UpdateMode="Conditional">
        <ContentTemplate>

            <%' 読み込み中に操作不可とするため全体に設定する透明なオーバーレイ %>
            <div id="SC3290104_RegistOverlayBlack"></div>
            <%' 読み込み中にボディー部を隠すための白色のオーバーレイ %>
            <div id="SC3290104_ContentOverlayBlack"></div>
            <%' 処理中のローディング %>
            <div id="SC3290104_ProcessingServer"></div>

            <%' 再描画用の隠しボタン %>
            <asp:Button ID="SC3290104_LoadSpinButton" runat="server" style="display:none;" />
            <%' 登録処理用の隠しボタン %>
            <asp:Button ID="SC3290104_RegisterButton" runat="server" style="display:none;" />

            <%' 隠し項目 %>
            <%' 処理タイプ（0:処理なし、1:登録ボタン押下、2:キャンセルボタン押下、3:排他エラー発生） %>
            <asp:HiddenField ID="SC3290104_ActionType" runat="server" />
            <%' 異常フォローID %>
            <asp:HiddenField ID="SC3290104_IrregFllwId" runat="server" />
            <%' 異常分類コード %>
            <asp:HiddenField ID="SC3290104_IrregClassCd" runat="server" />
            <%' 異常項目コード %>
            <asp:HiddenField ID="SC3290104_IrregItemCd" runat="server" />
            <%' スタッフコード %>
            <asp:HiddenField ID="SC3290104_StfCd" runat="server" />
            <%' 本日日付 %>
            <icrop:DateTimeSelector ID="SC3290104_NowDate" Format="Date" runat="server" />
            <%' フォロー完了フラグ（0:未完了、1:完了） %>
            <asp:HiddenField ID="SC3290104_FllwCompleteFlg" runat="server" />
            <%' フォロー完了文言 %>
            <asp:HiddenField ID="SC3290104_FllwCompleteWord" runat="server" />
            <%' フォロー未完了文言 %>
            <asp:HiddenField ID="SC3290104_FllwNotCompleteWord" runat="server" />
            <%' メールタイトル文言 %>
            <asp:HiddenField ID="SC3290104_MailTitle" runat="server" />

            <ul class="ListBox01">
                <li>
                    <dl>
                        <dt>
                            <p><icrop:CustomLabel ID="SC3290104_FllwSetting" runat="server" /></p>
                        </dt>
                        <dd>
                            <input id="SC3290104_FllwFlg" type="checkbox" runat="server" checked="false" style="width:80px;height:30px;" />
                        </dd>
                    </dl>
                </li>
            </ul>
            <ul class="ListBox01">
                <li>
                    <dl>
                        <dt>
                            <p><icrop:CustomLabel ID="SC3290104_FllwExprDateTitle" runat="server" /></p>
                        </dt>
                        <dd class="ListArrow">
                            <%' アプリ基盤のDateTimeSelectorでは、Webサーバの環境によって値の受け渡しがうまくいかないため、HiddenFieldコントロールで値の受け渡しを行う %>
                            <icrop:DateTimeSelector ID="SC3290104_FllwExprDate" runat="server" Format="Date" CssClass="FllwExprDate" />
                            <asp:HiddenField ID="SC3290104_FllwExprDateDummy" runat="server" />
                        </dd>
                    </dl>
                </li>
            </ul>
            <div class="Titlebox01"><icrop:CustomLabel ID="SC3290104_FllwMemoTitle" runat="server" /></div>
            <textarea id="SC3290104_FllwMemo" runat="server" class="MemoBox01" maxlength="1024"></textarea>
            <div id="SC3290104_ClearMemoDiv" class="ClearBtn01"><icrop:CustomLabel ID="SC3290104_ClearMemo" runat="server" /></div>
            <div class="Titlebox01"><icrop:CustomLabel ID="SC3290104_SendMemo" runat="server" /></div>
            <div class="MemoBtnSet">
                <div class="MemoBtn01"><icrop:CustomLabel ID="SC3290104_Notice" runat="server" /></div>
                <div id="SC3290104_Mail" class="MemoBtn02"><icrop:CustomLabel ID="SC3290104_ClearMail" runat="server" /></div>
            </div>
            <div class="Titlebox01"><icrop:CustomLabel ID="SC3290104_FllwCompleteFlgTitle" runat="server" /></div>
            <div id="SC3290104_FllwCompleteFlgButtonDiv" class="MemoEndBtn02"><icrop:CustomLabel ID="SC3290104_FllwCompleteFlgButton" runat="server" /></div>

        </ContentTemplate>

    </asp:UpdatePanel>

    <%' ポップオーバーの三角記号 %>
    <div class="triangle">
        <div class="triangleBorder">
            <div class="triangleInner"></div>
        </div>
    </div>

</div>

