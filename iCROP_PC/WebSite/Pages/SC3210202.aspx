<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3210202.aspx.vb" Inherits="PagesSC3210202" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" /> 
    <title></title>
    
    <%' $01 start 新車受付機能改善 %>
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

    <script type="text/javascript" src="../Scripts/SC3210201/jquery.popoverEx.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/SC3210201/Common.js?20121003000000"></script>
    <script type="text/javascript" src="../Scripts/SC3210201/SC3210202.js?20121003000000"></script>
    <%' $01 start 新車受付機能改善 %>

  </head>
  <body style="height: 100%;">
  <div id="bodyFrame" style="height: 100%;">
    <%--======================== インナーメインブロック Sta ========================--%>
    <div id="InnerMainBlock">
      <form id="Form1" runat="server">
        
        <%--======================== 画面共通エリア Sta ========================--%>
        <%--商談開始時間リスト--%>
        <asp:HiddenField id="SalesStartTimeList" runat="server"></asp:HiddenField>
        <%--来店時間リスト(来店状況)--%>
        <asp:HiddenField id="VisitVisitTimeList" runat="server"></asp:HiddenField>
        <%--来店時間リスト(待ち状況)--%>
        <asp:HiddenField id="WaitVisitTimeDateList" runat="server"></asp:HiddenField>
        <%--依頼通知時間リスト(査定)--%>
        <asp:HiddenField id="RequestAssessmentTimeDateList" runat="server"></asp:HiddenField>
        <%--依頼通知時間リスト(価格相談)--%>
        <asp:HiddenField id="RequestPriceConsultationTimeDateList" runat="server"></asp:HiddenField>
        <%--依頼通知時間リスト(ヘルプ)--%>
        <asp:HiddenField id="RequestHelpTimeDateList" runat="server"></asp:HiddenField>
        <%--アンドンハイライトチップ画像のプリロード--%>
        <img src="<%=ResolveClientUrl("~/Styles/Images/SC3210201/nssv00UpBg01cOn.png")%>" style="display:none" />
        <%--======================== 画面共通エリア End ========================--%>

        <%--======================== アンドンチップエリア Sta ========================--%>
        <div id="nssv00HeadLeftBox" class="HeadLeftBoxShadow">
    	    <ul>
            <%--来店--%>
      	    <li id="ReceptionistMainComingAria" class="type1">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox colorYellow"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="VisitLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainComing" class="DataBox colorNone"><span class="LightBox">
            	    <span class="textData"><asp:Literal id="BoardVisitNumber" runat="server" /></span>
                </span></div>
              </div>
            </li>
            <%--待ち--%>
      	    <li id="ReceptionistMainWaitAria" class="type3">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox colorYellowGreen"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="WaitLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainWait" class="DataBox colorNone"><span class="LightBox">
            	    <span class="textData"><asp:Literal id="BoardWaitNumber" runat="server" /></span>
                </span></div>
              </div>
            </li>
            <%--査定--%>
      	    <li id="ReceptionistMainAssessmentAria" class="type3">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox colorGreen"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="AssessmentLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainAssessment" class="DataBox colorNone"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="BoardAssessmentNumber" runat="server" /></span>
                  </span></div>
              </div>
            </li>
            <%--試乗車--%>
      	    <li id="ReceptionistMainRideVehicleAria" class="type2">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox Noactive"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="TestCarLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainRideVehicle" class="DataBox colorNone"><span class="LightBox">
            	      <span class="textData">&nbsp;</span>
                  </span></div>
              </div>
            </li>
            <%--価格相談--%>
      	    <li id="ReceptionistMainPriceConsultationAria" class="type2">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox colorAqua"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="PriceConsultationLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainPriceConsultation" class="DataBox colorNone"><span class="LightBox">
            	      <span class="textData"><asp:Literal id="BoardPriceConsultationNumber" runat="server" /></span>
                  </span></div>
              </div>
            </li>
            <%--ヘルプ--%>
      	    <li id="ReceptionistMainHelpAria" class="type3">
        	    <div class="InnerDatas">
          	    <h3 class="HeadBox colorPurple"><span class="LightBox">
            	      <span class="textData"><asp:Literal ID="HelpLiteral" runat="server" /></span>
                  </span></h3>
                <div class="HeaderBG">&nbsp;</div>
                <div id="ReceptionistMainHelp" class="DataBox colorNone"><span class="LightBox">
            	      <span class="textData"><asp:Literal id="BoardHelpNumber" runat="server" /></span>
                  </span></div>
              </div>
            </li>
          </ul>
        </div>
        <%--======================== アンドンチップエリア End ========================--%>

        <%--======================== 当日商談&制約件数 Sta ========================--%>
        <div id="nssv00HeadRightBox">
    	    <div class="InnerBox">
            <%--当日商談--%>
    	      <div class="LeftBox"><asp:Literal id="BoardResultNumber" runat="server" /><span><asp:Literal ID="PersonUnitLiteral" runat="server" /></span></div>
            <%--制約件数--%>
    	      <div class="RightBox"><asp:Literal id="BoardAgreeNumber" runat="server" /><span><asp:Literal ID="CarUnitLiteral" runat="server" /></span></div>
          </div>
        </div>
        <%--======================== 当日商談&制約件数 End ========================--%>

        <%--======================== スタッフエリア Sta ========================--%>
        <div class="SituationFlame">
          <%--スタッフエリアタイトル--%>
          <div class="SituationTitle">
            <asp:Literal ID="StaffTitleLiteral" runat="server" />
          </div>
          <ul class="SituationListSet">
            <%--スタッフ情報の繰り返し--%>
            <asp:Repeater ID="StaffRepeater" runat="server" enableViewState="false">
              <ItemTemplate>
              <%--お客様情報ポップオーバー表示用--%>
                <li id="StuffChip" runat="server">
                  <%--グレーアウト用--%>
                  <div class="StuffScreenBlack" style="display:none"></div>
                  <%--選択エリア用（絶対位置指定となる）--%>
                  <div class="staffChip">
                    <%--スタッフ状況--%>
                    <asp:Literal ID="MainNegoLiteral"    runat="server" text="<div id='MainDiv' class='ListBox'>" visible="false"></asp:Literal>
                    <asp:Literal ID="MainStandbyLiteral" runat="server" text="<div id='MainDiv' class='ListBoxStandby'>" visible="false"></asp:Literal>
                    <asp:Literal ID="MainLeavingLiteral" runat="server" text="<div id='MainDiv' class='ListBoxAway'>" visible="false"></asp:Literal>
                    <asp:Literal ID="MainOfflineLiteral" runat="server" text="<div id='MainDiv' class='ListBoxStandby'>" visible="false"></asp:Literal>
                      <asp:HiddenField id="VisitSeq" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSEQ").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField id="StaffStatus" runat="server" Value='<%# Server.HTMLEncode(Eval("STAFFSTATUS").ToString()) %>'></asp:HiddenField>
                      <asp:HiddenField id="VisitorLinkingCount" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITORLINKINGCOUNT").ToString()) %>'></asp:HiddenField>
      	              <%--オフライン用のカバーエリア--%>
                      <div ID="OfflineCoverDiv" runat="server" class="BlackScreens" visible="false">
                        &nbsp;
                      </div>
                      <%--上段--%>
                      <%--アイコン表示エリア--%>
          	          <div id="ClaimIcnDiv" class="IcnSet" runat="server" visible="false">
            	          <div id="ClaimIcn" class="Icn1" runat="server">
                          <asp:Literal ID="ClaimChar" runat="server"></asp:Literal>
                        </div>
                      </div>
                      <%--テーブルNo.表示エリア--%>
      	              <div id="SalesTableNoDiv" class="TMark" runat="server" visible="false">
                        <asp:Literal ID="SalesTableNoLiteral" runat="server"></asp:Literal>
                      </div>
                      <%--当日実績表示エリア--%>
      	              <div id="ResultDiv" class="IcnBox" runat="server" visible="false">
      	                <p class="TextNumber"><asp:Literal ID="ResultNumLiteral" runat="server"></asp:Literal></p>
      	                <p id="Chip1" class="Chip" runat="server" visible="false"><asp:Image runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00lefticn2.png" width="27" height="27" /></p>
      	                <p id="Chip2" class="Chip" runat="server" visible="false"><asp:Image runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00lefticn2.png" width="27" height="27" /></p>
      	                <p id="Chip3" class="Chip" runat="server" visible="false"><asp:Image runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00lefticn2.png" width="27" height="27" />
                          <span><asp:Literal ID="AgreeNumLiteral" runat="server"></asp:Literal></span>
                        </p>
                      </div>
                      <%--顧客情報エリア--%>
          	          <div id="CustmerDiv" class="PhotoBox1 ellipsis" runat="server" visible="false">
                        <asp:Image ID="CustImageFileImage" runat="server" width="84" height="84" /><br />
                        <asp:Literal ID="CustNameLiteral" runat="server"></asp:Literal>
                      </div>
                      <%--空エリア--%>
      	              <div id="EmptyDiv" class="PhotoBox1" runat="server" visible="false">
                      </div>
                      <%--スタッフ情報エリア--%>
          	          <div class="PhotoBox2 ellipsis">
                        <div id="LinkingCountDiv" class="RedIcn" runat="server" visible="false">
                          <asp:Literal ID="VisitorLinkingCountLiteral" runat="server"></asp:Literal>
                        </div>
                        <asp:Image ID="OrgImgFileImage" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00photo2.png" width="84" height="84" /><br />
                        <asp:Literal ID="UserNameLiteral" runat="server"></asp:Literal>
                      </div>
                      <%--下段(スタンバイ以外)--%>
      	              <div id="UnderNegoDiv" class="TimeBox" runat="server" visible="false">
      	                <div class="IcnSet">
      	                  <div id="AssessmentIconDiv" class="Icn">
                            <asp:Image ID="AssessmentIconOff" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn1off.png" width="33" height="34" />
                            <asp:Image ID="AssessmentIconOn" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn1on.png" width="33" height="34" Visible="false" />
                          </div>
      	                  <div id="TrialVehicleIconDiv" class="Icn">
                            <asp:Image ID="TrialVehicleIconOff" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn2off.png" width="33" height="34" />
                            <asp:Image ID="TrialVehicleIconOn" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn2on.png" width="33" height="34" Visible="false" />
                          </div>
      	                  <div id="PriceIconDiv" class="Icn">
                            <asp:Image ID="PriceIconOff" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn3off.png" width="33" height="34" />
                            <asp:Image ID="PriceIconOn" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn3on.png" width="33" height="34" Visible="false" />
                          </div>
      	                  <div id="HelpIconDiv" class="Icn">
                            <asp:Image ID="HelpIconOff" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn4off.png" width="34" height="34" />
                            <asp:Image ID="HelpIconOn" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00timeicn4on.png" width="34" height="34" Visible="false" />
                          </div>
      	                </div>
      	                <div id="BusinessLapsedTime" class="Time">
                          <p id="SalesStartTime"></p>
                        </div>
                      </div>  
                      <%--下段(スタンバイ時)--%>
      	              <div id="UnderOherDiv" class="TimeBox" runat="server" visible="false">
      	                <div class="CenterText">
                          <asp:Literal ID="StaffStatusLiteral" runat="server"></asp:Literal>
                        </div>
      	              </div>
                    <asp:Literal ID="MainEndLiteral" runat="server" text="</div>"></asp:Literal>
      	          </div>
      	        </li>
              </ItemTemplate>
            </asp:Repeater>
          </ul>
        </div>
        <%--======================== スタッフエリア End ========================--%>


        <%--======================== 右フレーム Sta ========================--%>
        <div class="RightFlame">

          <%--======================== 来店状況エリア Sta ========================--%>
          <div id="ComingStore" class="VisitFlame">
            <%--タイトル--%>
      	    <div class="VisitTitle">
              <asp:Literal ID="VisitTitleLiteral" runat="server" />
            </div>
      	    <div class="Cassette">
              <asp:Repeater ID="VisitRepeater" runat="server" enableViewState="false">
                <ItemTemplate>
                  <div id="CustomerChip" class="CassetteLeft" runat="server">
                    <%--グレーアウト用
                    <div class="CustomerScreenBlack" style="display:none"></div>--%>
                    <%--アイコン表示エリア--%>
          	        <div id="VisClaimIcnDiv" class="IcnSet" runat="server" visible="false">
            	        <div id="VisClaimIcn" class="Icn1" runat="server">
                        <asp:Literal ID="VisClaimChar" runat="server"></asp:Literal>
                      </div>
                    </div>
                    <%--テーブルNo.表示エリア--%>
      	            <div id="VisSalesTableNoCustomer" class="TMark" runat="server" visible="false">
                      <asp:Literal ID="VisSalesTableNoLiteral" runat="server"></asp:Literal>
                    </div>
                    <%--左部表示エリア--%>
                    <div class="CassetteName">
                      <div class="VisitStartTime"><asp:Literal ID="VisVisitStartLiteral" runat="server"></asp:Literal></div>
                      <div class="CassetteCustName ellipsis"><asp:Label ID="VisCustNameLabel" runat="server"></asp:Label></div>
                    </div>
                    <%--右部表示エリア--%>
                    <div class="CassettePhoto">
                      <p id="VisLapsedTime"></p>
                      <br />
                      <div id="VisAccountImageAreaNormal" class="Photo" runat="server" visible="true">
                        <asp:Image ID="VisOrgImgfileImage" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00rightphoto0.png" width="63" height="64" />
                      </div>
                      <div id="VisAccountImageAreaBroadcast" class="Photo" runat="server" visible="false">
                        <asp:Image runat="server" ImageUrl="../Styles/Images/VisitCommon/icon_broadcast02.png" width="63" height="64" />
                      </div>
                    </div>
                  </div>
                </ItemTemplate>
              </asp:Repeater>
            </div>
          </div>
          <%--======================== 来店状況エリア End ========================--%>

          <%--======================== 待ち状況エリア Sta ========================--%>
          <div id="WaitStore" class="WaitFlame">
            <%--タイトルエリア--%>
      	    <div class="WaitTitle">
              <asp:Literal ID="WaitTitleLiteral" runat="server" />
            </div>
            <div class="Cassette">
              <asp:Repeater ID="WaitRepeater" runat="server" enableViewState="false">
                <ItemTemplate>
                  <div id="CustomerChip" class="CassetteLeft" runat="server">
                    <%--グレーアウト用
                    <div class="CustomerScreenBlack" style="display:none"></div>--%>
                    <div class="StopBorder"></div>
                    <%--アイコン表示エリア--%>
                    <div id="WaitClaimIcnDiv" class="IcnSet" runat="server" visible="false">
            	        <div id="WaitClaimIcn" class="Icn1" runat="server">
                        <asp:Literal ID="WaitClaimChar" runat="server"></asp:Literal>
                      </div>
                    </div>
                    <%--テーブルNo.表示エリア--%>
                    <div id="WaitSalesTableNoCustomer" class="TMark" runat="server" visible="false">
                      <asp:Literal ID="WaitSalesTableNoLiteral" runat="server"></asp:Literal>
                    </div>
                    <%--左部表示エリア--%>
                    <div class="CassetteName">
                      <div class="VisitStartTime"><asp:Literal ID="WaitVisitStartLiteral" runat="server"></asp:Literal></div>
                      <div class="CassetteCustName ellipsis"><asp:Label ID="WaitCustNameLabel" runat="server"></asp:Label></div>
                    </div>
                    <%--右部表示エリア--%>
                    <div class="CassettePhoto">
                      <p id="WaitLapsedTime"></p>
                      <br />
                      <div id="WaitAccountImageAreaNormal" class="Photo" runat="server" visible="true">
                        <asp:Image ID="WaitOrgImgfileImage" runat="server" ImageUrl="../Styles/Images/SC3210201/nssv00rightphoto0.png" width="63" height="64" />
                      </div>
                      <div id="WaitAccountImageAreaBroadcast" class="Photo" runat="server"  visible="false">
                        <asp:Image runat="server" ImageUrl="../Styles/Images/VisitCommon/icon_broadcast02.png" width="63" height="64" />
                      </div>
                    </div>
                  </div>
                </ItemTemplate>
              </asp:Repeater>
            </div>
          </div>
          <%--======================== 待ち状況エリア End ========================--%>

        </div>
        <%--======================== 右フレーム End ========================--%>

      </form>
    </div>
    <%--======================== インナーメインブロック End ========================--%>
    
  </div>
  </body>
</html>
