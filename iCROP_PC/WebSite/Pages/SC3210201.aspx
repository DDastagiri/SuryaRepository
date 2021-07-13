<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3210201.aspx.vb" Inherits="PagesSC3210201" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9"> 
    <title></title>

    <%'スタイルシート %>
    <!-- 基本CSS -->
    <link rel="stylesheet" href="../Styles/common.css?20121003000000" type="text/css" media="screen,print" />
    <!-- ヘッダCSS -->
    <link rel="stylesheet" href="../Styles/header.css?20121003000000" type="text/css" media="screen,print" />
    <!-- フッタCSS -->
    <link rel="stylesheet" href="../Styles/footer.css?20121003000000" type="text/css" media="screen,print" />
    <%'スタイルシート(画面固有) %>
    <!-- ドキュメントCSS -->
    <link rel="stylesheet" href="../Styles/SC3210201/SC3210201.css?20121003000000" type="text/css" />
    
    <%'スクリプト %>
    <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.16.custom.min.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.ipad.altfix.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.doubletap.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.flickable.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.json-2.3.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.popover.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.fingerscroll.js?20121003000000"></script>

    <script type="text/javascript" src="../Scripts/icropScript.js?20121003000000"></script>
    
    <script type="text/javascript" src="../Scripts/jquery.CheckButton.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomButton.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomLabel.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomTextBox.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.DateTimeSelector.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.PopOverForm.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SegmentedButton.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SwitchButton.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomRepeater.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.NumericKeypad.js?20121003000000"></script>
    
    <script type="text/javascript" src="../Scripts/icropBase.js?20121003000000"></script>

    <script type="text/javascript" src="../Scripts/SC3210201/Common.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/SC3210201/SC3210201.js?20121003000000"></script>
  </head>

  <body>
    <div id="bodyFrame">
      <form id="Form1" runat="server">
        <%--処理中のローディング--%>
        <div class="registOverlay">
          <div class="registWrap">
            <div class="processingServer"></div>
          </div>
        </div>
    
        <%--======================== デバッグ用エリア Sta ========================--%>
        <div id="DebugArea" runat="server" style="position:absolute; margin-top:5px; top:0px; left:0px; width:100%; height:45px; z-index:9; font-size:11px; background:rgba(255, 255, 255, 0.6);" Visible="false">
          <div style="margin:0px 10px 0px 10px; float:left;">
              <div id="pageData"></div>
              <div id="frameData"></div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div>ロード回数：<span id="LoadData">0</span>回</div>
              <div>ロード中：<span id="LoadFlag" style="display:none;">★</span></div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div>非同期通信回数：<span id="PageMethodsCount">0</span>回</div>
              <div>非同期通信中：<span id="PageMethodsFlag" style="display:none;">★</span></div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div>部分読込回数：<span id="PartialRenderingCount">0</span>回</div>
              <div>部分読込中：<span id="PartialRenderingFlag" style="display:none;">★</span></div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div id="LockButton" class="likeButton">ロック</div>
              <div id="ResetButton" class="likeButton">ロック解除</div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div id="ReloadRequestButton" class="likeButton">リロード要求</div>
              <div id="ReloadForceButton" class="likeButton">強制リロード</div>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div style="margin:0px 5px 0px 5px; float:left">
                  <div id="PushReserveButton" class="likeButton">Push受信</div>
              </div>
              <br/>
              <select id="functionNo">
                  <option value="01">01</option>
                  <option value="02">02</option>
                  <option value="03">03</option>
                  <option value="99">99</option>
              </select>
              <select id="logicNo">
                  <option value="01">01</option>
                  <option value="02">02</option>
                  <option value="03">03</option>
                  <option value="04">04</option>
                  <option value="05">05</option>
                  <option value="06">06</option>
                  <option value="07">07</option>
                  <option value="08">08</option>
              </select>
          </div>
          <div style="margin:0px 5px 0px 5px; float:left">
              <div>総数増加警告音：<span id="Sound1">off</span></div>
              <div>異常警告音：<span id="Sound2">off</span></div>
          </div>
        </div>
        <%--======================== デバッグ用エリア End ========================--%>

        <%--======================== 画面共通エリア Sta ========================--%>
        <%--ロック解除秒数--%>
        <asp:HiddenField id="LockResetInterval" runat="server"></asp:HiddenField>
        <%--来店時間警告秒数--%>
        <asp:HiddenField id="VisitTimeAlertSpan" runat="server"></asp:HiddenField>
        <%--待ち時間警告秒数--%>
        <asp:HiddenField id="WaitTimeAlertSpan" runat="server"></asp:HiddenField>
        <%--査定警告秒数--%>
        <asp:HiddenField id="AssessmentAlertSpan" runat="server"></asp:HiddenField>
        <%--価格相談警告秒数--%>
        <asp:HiddenField id="PriceAlertSpan" runat="server"></asp:HiddenField>
        <%--ヘルプ警告秒数--%>
        <asp:HiddenField id="HelpAlertSpan" runat="server"></asp:HiddenField>
        <%--エラーメッセージ--%>
        <asp:HiddenField id="ErrorMessage" runat="server"></asp:HiddenField>
        <%--エラーメッセージ送信用ボタン--%>
        <asp:Button ID="SendErrorMessageButton" runat="server" style="display:none;"></asp:Button>
        <%--非同期読み込みのためのScriptManagerタグ--%>
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
        <%--現在日付--%>
        <asp:HiddenField id="NowDateString" runat="server"></asp:HiddenField>
        <%--警告音出力フラグ--%>
        <asp:HiddenField id="AlarmOutputStatus" runat="server" value="1"></asp:HiddenField>
        <%--======================== 画面共通エリア End ========================--%>
        
        <%--======================== 商談中詳細画面 Sta ========================--%>
        <!-- pop -->
        <div class="scNscPopUpContactVisit nsc61winCoordinate1 popoverEx">
          <%--吹き出し(triangle)--%>
          <div class="triangle"></div>
			    <asp:UpdatePanel id="UpdateArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
		          <%--ロードをキックするためのボタン--%>
		          <asp:Button ID="StaffDetailDisplayButton" runat="server" style="display:none;" />
              <%--画面で保持しておく情報--%>
	            <asp:HiddenField id="StaffDetailDialogVisitSeq" runat="server" />
					    <asp:HiddenField id="StaffDetailDialogIndex" runat="server" />
	            <asp:HiddenField id="StaffDetailPopoverErrorMessage" runat="server" />
	            <asp:HiddenField id="StaffDetailDialogSalesStartTime" runat="server" />
					    <%-- 依頼通知送信日時時間リスト --%>
					    <asp:HiddenField id="SendDateList" runat="server"></asp:HiddenField>
              <%--ポップボックス--%>
              <div class="scNscPopUpContactVisitWindownBox">
                <%--ヘッダバックエリア--%>
      	        <div class="scNscPopUpHeaderBg"></div>
                <%--商談状況バックエリア--%>
      	        <div class="scNscPopUpDataBg">
                </div>
                <%--ヘッダタイトル--%>
                <div class="scNscPopUpContactVisitHeader">
                  <h3 class="ellipsis"><asp:Literal ID="StaffDetailDialogTitleLiteral" runat="server" /></h3>
                </div>
                <%--商談状況表示エリア--%>
                <div class="scNscPopUpContactVisitListArea">
                  <%--処理中のローディング--%>
                  <div class="registOverlay">
                    <div class="registWrap">
                      <div class="processingServer"></div>
                    </div>
                  </div>
                  <%--来店回数・商談時間--%>
                  <div class="scNscPopUpContactVisitSttl01">
                    <asp:Literal ID="StaffDetailNegotiationLiteral" runat="server" /> <asp:Literal ID="VisitCountLiteral" runat="server" /><asp:Literal ID="StaffDetailNowVisitLiteral" runat="server" />
                    <div class="LeftBoxTime"></div>
                  </div>
                  <div class="scNscPopUpContactVisitBox01">
                    <div class="scNscPopUpContactVisitTextArea02">
                      <div class="scNscPopUpContactVisitScroll">
				                <%--依頼リスト--%>
				                <asp:Repeater ID="NoticeListRepeater" runat="server" enableViewState="false">
                          <HeaderTemplate>
                            <ul class="ListSet01">
                          </HeaderTemplate>
                          <ItemTemplate>
				                  <li id="NoticeName" runat="server">
					                  <asp:HiddenField id="NoticeReqctg" runat="server" Value='<%# Server.HTMLEncode(Eval("NOTICEREQCTG").ToString()) %>'></asp:HiddenField>
					                  <div id="NoticeNameDiv" class="ellipsis"><asp:Literal ID="NoticeNameLiteral" runat="server" /></div>
                            <div id="NoticeTimeDiv">
                              <p id="NoticeTime"></p>
                            </div>
                          </li>
				                  </ItemTemplate>
                          <FooterTemplate>
                            </ul>
                          </FooterTemplate>
				                </asp:Repeater>
                        <%--商談の詳細--%>
                        <ul class="ListSet02">
                          <%--顧客名--%>
                          <li class="list1">
                            <div id="StaffDetailCustomerNameDiv" class="ellipsis"><asp:Literal ID="StaffDetailCustomerName" runat="server" /></div>
													  <%--アイコンの表示--%>
													  <div class="listIcnSet">
														  <%--苦情アイコン--%>
														  <div id="ClaimIcon" runat="server" class="listIcn1"><asp:Literal ID="StaffDetailClaimIconLiteral" runat="server" /></div>
														  <%--Vアイコン:STEP2表示なし--%>
                              <%--<div class="listIcn2">V</div>--%>
													  </div>
                          </li>
												  <%--来店人数--%>
                          <li class="list2"><asp:Literal ID="VisitPersonNumberLiteral" runat="server" /><asp:Literal ID="StaffDetailVisitPersonLiteral" runat="server" visible="false" /></li>
												  <%--テーブルNo--%>
                          <li class="list3"><asp:Literal ID="DisplayTableNo" runat="server" visible="false" /><asp:Literal ID="StaffDetailDialogSalesTableNo" runat="server" /></li>
                          <%-- 希望(もしくは成約)車種の表示 --%>
                          <li class="list4 ellipsis"><asp:Literal ID="CarName" runat="server" /></li>
                          <li class="list5 ellipsis"><asp:Literal ID="GradeName" runat="server" /></li>
                          <%--見積もり金額:STEP2表示なし--%>
                          <%--<li class="list6">260,000</li>--%>
                        </ul>
											  <%--プロセス:ステータス--%>
                        <ul class="ListSet03">
                          <li id="StaffDetailProcess1" runat="server" ></li>
                          <li id="StaffDetailProcess2" runat="server" ></li>
                          <li id="StaffDetailProcess3" runat="server" ></li>
                          <li id="StaffDetailProcess4" runat="server" ></li>
												  <li ID="StaffDetailStatus" runat="server" ></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <!-- pop end-->
        <%--======================== 商談中詳細画面 End ========================--%>

        <%--======================== メインブロック Sta ========================--%>
        <div id="MainBlock" class="heightType01 baceBgType01">
          <%--処理中のローディング--%>
          <div class="registOverlay">
            <div class="registWrap">
              <div class="processingServer"></div>
            </div>
          </div>
        </div>
        <%--======================== メインブロック End ========================--%>

      </form>
    </div>
  </body>
</html>
