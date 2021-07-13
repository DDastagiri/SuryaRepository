<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC3100102.aspx.vb" Inherits="PagesSC3100102" %>
<!DOCTYPE html>
<html lang="ja">
<head id="Head1" runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="format-detection" content="telephone=no" />

    <%'タイトル %>
    <title></title>
    
    <%' $01 start 新車受付機能改善 %>
    <%'スタイルシート %>
    <link rel="Stylesheet" href="../Styles/Style.css?20130128000000" />
    <link rel="Stylesheet" href="../Styles/jquery.popover.css?20130128000000" />
   	<link rel="stylesheet" href="../Styles/Controls.css?20130128000000" />

    <%'スクリプト %>
    <script type="text/javascript" src="../Scripts/jquery-1.5.2.min.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.16.custom.min.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.ipad.altfix.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.doubletap.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.flickable.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.json-2.3.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.popover.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.fingerscroll.js?20130128000000"></script>

    <script type="text/javascript" src="../Scripts/icropScript.js?20130128000000"></script>
    
    <script type="text/javascript" src="../Scripts/jquery.CheckButton.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CheckMark.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomButton.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomLabel.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomTextBox.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.DateTimeSelector.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.PopOverForm.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SegmentedButton.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.SwitchButton.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.CustomRepeater.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/jquery.NumericKeypad.js?20130128000000"></script>

    <link rel="Stylesheet" href="../Styles/SC3100101/SC3100101.css?20200220000000" type="text/css" media="screen" />
    <script type="text/javascript" src="../Scripts/SC3100101/jquery.popoverEx.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100101/Common.js?20130128000000"></script>
    <script type="text/javascript" src="../Scripts/SC3100101/SC3100102.js?20180712000000"></script>
    <%' $01 end   新車受付機能改善 %>
</head>
<body>
<div id="bodyFrame">
<form id="ThisForm" runat="server">

<div id="Main">
    <%--======================== 画面共通エリア ========================--%>
    <%--商談開始時間リスト--%>
    <asp:HiddenField id="SalesStartTimeList" runat="server"></asp:HiddenField>
    <%--来店時間リスト（来店状況）--%>
    <asp:HiddenField id="VisitVisitTimeList" runat="server"></asp:HiddenField>
    <%--来店時間リスト（待ち状況）--%>
    <asp:HiddenField id="WaitVisitTimeDateList" runat="server"></asp:HiddenField>
    
    <%--振当て待ち時間リスト--%>
    <asp:HiddenField id="WaitAssginedTimeList" runat="server"></asp:HiddenField>
    <%--接客待ち時間リスト--%>
    <asp:HiddenField id="WaitServiceTimeList" runat="server"></asp:HiddenField>
    <%--接客中時間リスト--%>
    <asp:HiddenField id="NegotiationTimeList" runat="server"></asp:HiddenField>

    <%--依頼通知時間リスト（査定）--%>
    <asp:HiddenField id="RequestAssessmentTimeDateList" runat="server"></asp:HiddenField>
    <%--依頼通知時間リスト（価格相談）--%>
    <asp:HiddenField id="RequestPriceConsultationTimeDateList" runat="server"></asp:HiddenField>
    <%--依頼通知時間リスト（ヘルプ）--%>
    <asp:HiddenField id="RequestHelpTimeDateList" runat="server"></asp:HiddenField>
    
    <%--非同期読み込みのためのScriptManagerタグ--%>
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <div id="Inner">
        <div class="SC_MainOutFrame">
            <%-- アンドンエリア --%>
            <ul class="DigitalDisplay">
                <li>
                    <div class="TitleText"><asp:Literal ID="BoardVisitLiteral" runat="server" /></div>
                    <div class="Number"><asp:Literal id="BoardVisitNumber" runat="server"></asp:Literal></div>
                </li>
                <li>
                    <div class="TitleText"><asp:Literal ID="BoardSalesLiteral" runat="server" /></div>
                    <div class="Number"><asp:Literal id="BoardSalesNumber" runat="server"></asp:Literal></div>
                </li>
                <li>
                    <div class="TitleText"><asp:Literal ID="BoardEstimateLiteral" runat="server" /></div>
                    <div class="Number"><asp:Literal id="BoardEstimateNumber" runat="server"></asp:Literal></div>
                </li>
                <li>
                    <div class="TitleText"><asp:Literal ID="BoardConclusionLiteral" runat="server" /></div>
                    <div class="Number"><asp:Literal id="BoardConclusionNumber" runat="server"></asp:Literal></div>
                </li>
                <li>
                    <div class="TitleText"><asp:Literal ID="BoardDeliveryLiteral" runat="server" /></div>
                    <div class="Number"><asp:Literal id="BoardDeliveryNumber" runat="server"></asp:Literal></div>
                </li>
            </ul>

            <%-- 接客状況エリア --%>
            <div class="SC_Cassette">
                <%-- 振当て待ちエリア --%>
                <div class="SC_CassetteBox01">
                    <div class="SC_CassetteTitle">
                        <div class="Title">
                            <asp:Literal ID="WaitAssginedTitleLiteral" runat="server"></asp:Literal>
                        </div>
                        <div class="Number">
                            <asp:Literal ID="WaitAssginedNumberLiteral" runat="server"></asp:Literal>
                            <asp:Literal ID="WaitAssginedUnitLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="SC_CassetteBoxIn">
                        <div id="CassetteBox01FlickArea" >
                            <div id="CassetteBox01FlickAreaInner">
                                <ul class="CassetteSet">
                                    <asp:Repeater ID="WaitAssginedRepeater" runat="server" enableViewState="false">
                                        <ItemTemplate>
                                            <li>
                                                <div class="CassetteBack">
                                                    <%-- 各種イベント補足用エリア --%>
                                                    <div class="ReceptionChip" >
                                                        <%-- 顧客情報 --%>
                                                        <div class="CassetteTextSet">
                                                            <%--隠し項目--%>
                                                            <asp:HiddenField id="WaitAssginedVisitSeq" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSEQ").ToString()) %>'></asp:HiddenField>
                                                            <asp:HiddenField id="WaitAssginedVisitStatus" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSTATUS").ToString()) %>'></asp:HiddenField>
                                                        
                                                            <%--来店時間--%>
                                                            <div class="Clock">
                                                                <asp:Literal ID="WaitAssginedVisitStartLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--経過時間--%>
                                                            <div class="Time">8’12”</div>
                                                            <%--顧客名称--%>
                                                            <div class="Name ellipsis">
                                                                <asp:Literal ID="WaitAssginedCustNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START--%>
                                                            <%--Lマーク--%>
                                                            <div id="WaitAssginedLIcon" class="LIcon" runat="server" visible="false"></div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END--%>
                                                            <%--来店人数--%>
                                                            <div id="WaitAssginedVisitPersonNum" class="Number" runat="server" Visible="false">
                                                                <asp:Literal ID="WaitAssginedVisitPersonNumLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--車両登録No.--%>
                                                            <div id="WaitAssginedVclregNoLine" class="String ellipsis" runat="server" visible="false">
                                                                <asp:Literal ID="WaitAssginedVclregNoLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--来店手段（歩き）--%>
                                                            <div id="WaitAssginedMeansWalkLine" class="Person" runat="server" visible="false"></div>
                                                            <%--来店手段（車）--%>
                                                            <div id="WaitAssginedMeansCarLine" class="Car" runat="server" visible="false"></div>

                                                            <%--顧客担当SC名--%>
                                                            <div id="WaitAssginedUserName" class="Staff ellipsis" runat="server" Visible="false">
                                                                <asp:Literal ID="WaitAssginedUserNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
                                                    
                                                        <%-- 接客不要(4未満) --%>
                                                        <div id="UnnecessarySet" class="CassettePinSet" runat="server" Visible="false">
                                                            <ul>
                                                                <asp:Literal ID="PinIconLiteral" runat="server"></asp:Literal>
                                                            </ul>
                                                        </div>
                                                    
                                                        <%-- 接客不要(5以上) --%>
                                                        <div id="UnnecessarySetMore" class="CassettePinSetMore" runat="server" Visible="false">
                                                            <div class="Pin"></div>
                                                            <div class="PinNumber">
                                                                <asp:Literal ID="PinNumberLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
                                                    
                                                        <%--苦情アイコン--%>
                                                        <div id="WaitAssginedClaimIcnDiv" class="exmark" runat="server" Visible="false">
                                                            <asp:Literal ID="WaitAssginedClaimIconLiteral" runat="server"></asp:Literal>
                                                        </div>

                                                        <%-- 商談テーブルNO. --%>
                                                        <div id="WaitAssginedSalesTableNoCustomer" class="Tmark" runat="server" Visible="false">
                                                            <p>
                                                                <asp:Literal ID="WaitAssginedSalesTableNoLiteral" runat="server"></asp:Literal>
                                                            </p>
                                                        </div>
                                                    
                                                        <%--削除ボタン--%>
                                                        <div class="ComingStoreOff1-1-5 ComingStoreOffDeletionButton" style="display:none;">
                                                            <button class="ComingStoreOff1-1-5-1"><asp:Literal id="WaitAssginedVisitorDeleteLiteral" runat="server" /></button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                
                <%-- 接客待ちエリア --%>
                <div class="SC_CassetteBox02">
                    <div class="SC_CassetteTitle">
                        <div class="Title">
                            <asp:Literal ID="WaitServiceTitleLiteral" runat="server"></asp:Literal>
                        </div>
                        <div class="Number">
                            <asp:Literal ID="WaitServiceNumberLiteral" runat="server"></asp:Literal>
                            <asp:Literal ID="WaitServiceUnitLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="SC_CassetteBoxIn">
                        <div id="CassetteBox02FlickArea" >
                            <div id="CassetteBox02FlickAreaInner">
                                <ul class="CassetteSet">
                                    <asp:Repeater ID="WaitServiceRepeater" runat="server" enableViewState="false">
                                        <ItemTemplate>
                                            <li>
                                                <div class="CassetteBack">
                                                    <%-- 各種イベント補足用エリア --%>
                                                    <div class="ReceptionChip" >
                                                        <%-- 顧客情報 --%>
                                                        <div class="CassetteTextSet">
                                                            <%--隠し項目--%>
                                                            <asp:HiddenField id="WaitServiceVisitSeq" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSEQ").ToString()) %>'></asp:HiddenField>
                                                            <asp:HiddenField id="WaitServiceVisitStatus" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSTATUS").ToString()) %>'></asp:HiddenField>
                                                        
                                                            <%--来店時間--%>
                                                            <div class="Clock">
                                                                <asp:Literal ID="WaitServiceVisitStartLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--経過時間--%>
                                                            <div class="Time">8’12”</div>
                                                            <%--顧客名称--%>
                                                            <div class="Name ellipsis">
                                                                <asp:Literal ID="WaitServiceCustNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START--%>
                                                            <%--Lマーク--%>
                                                            <div id="WaitServiceLIcon" class="LIcon" runat="server" visible="false"></div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END--%>
                                                            <%--来店人数--%>
                                                            <div id="WaitServiceVisitPersonNum" class="Number" runat="server" Visible="false">
                                                                <asp:Literal ID="WaitServiceVisitPersonNumLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--車両登録No.--%>
                                                            <div id="WaitServiceVclregNoLine" class="String ellipsis" runat="server" visible="false">
                                                                <asp:Literal ID="WaitServiceVclregNoLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--来店手段（歩き）--%>
                                                            <div id="WaitServiceMeansWalkLine" class="Person" runat="server" visible="false"></div>
                                                            <%--来店手段（車）--%>
                                                            <div id="WaitServiceMeansCarLine" class="Car" runat="server" visible="false"></div>

                                                            <%--顧客担当SC名--%>
                                                            <div id="WaitServiceUserName" class="Staff ellipsis" runat="server" Visible="false">
                                                                <asp:Literal ID="WaitServiceUserNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
                                                    
                                                        <%--- 左右区切り線 --%>
                                                        <div class="VerticalLine"></div>
        
                                                        <%-- 対応担当SC写真 --%>
                                                        <div id="WaitServiceAccountImageAreaNormal" class="Photo" runat="server" Visible="true">
                                                            <asp:Image ID="WaitServiceOrgImgfileImage" runat="server" width="42" height="40"></asp:Image>
                                                        </div>
                                                        <div id="WaitServiceAccountImageAreaBroadcast" class="ComingStoreOff1-1-4-3" runat="server" Visible="false">
                                                        
                                                        </div>

                                                        <%--苦情アイコン--%>
                                                        <div id="WaitServiceClaimIcnDiv" class="exmark" runat="server" Visible="false">
                                                            <asp:Literal ID="WaitServiceClaimIconLiteral" runat="server"></asp:Literal>
                                                        </div>

                                                        <%-- 商談テーブルNO. --%>
                                                        <div id="WaitServiceSalesTableNoCustomer" class="Tmark" runat="server" Visible="false">
                                                            <p>
                                                                <asp:Literal ID="WaitServiceSalesTableNoLiteral" runat="server"></asp:Literal>
                                                            </p>
                                                        </div>

                                                        <%-- グレーアウト --%>
                                                        <div id="WaitServiceInactive" class="Inactive" runat="server" Visible="false"></div>
                                                    
                                                        <%--削除ボタン--%>
                                                        <div class="ComingStoreOff1-1-5 ComingStoreOffDeletionButton" style="display:none;">
                                                            <button class="ComingStoreOff1-1-5-1"><asp:Literal id="WaitServiceVisitorDeleteLiteral" runat="server" /></button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                
                <%-- 接客中エリア --%>
                <div class="SC_CassetteBox03">
                    <div class="SC_CassetteTitle">
                        <div class="Title">
                            <asp:Literal ID="NegotiationTitleLiteral" runat="server"></asp:Literal>
                        </div>
                        <div class="Number">
                            <asp:Literal ID="NegotiationNumberLiteral" runat="server"></asp:Literal>
                            <asp:Literal ID="NegotiationUnitLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="SC_CassetteBoxIn">
                        <div id="CassetteBox03FlickArea" >
                            <div id="CassetteBox03FlickAreaInner">
                                <ul class="CassetteSet">
                                    <asp:Repeater ID="NegotiationRepeater" runat="server" enableViewState="false">
                                        <ItemTemplate>                                            
                                            <li>
                                                <div class="CassetteBack">
                                                    <%-- 各種イベント補足用エリア --%>
                                                    <div id="EventDiv"  runat="server">
                                                        <%-- 顧客情報 --%>
                                                        <div class="CassetteTextSet">
                                                            <%--隠し項目--%>
                                                            <asp:HiddenField id="NegotiationVisitSeq" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSEQ").ToString()) %>'></asp:HiddenField>
                                                            <asp:HiddenField id="NegotiationVisitStatus" runat="server" Value='<%# Server.HTMLEncode(Eval("VISITSTATUS").ToString()) %>'></asp:HiddenField>
                                                        
                                                            <%--来店時間--%>
                                                            <div class="Clock">
                                                                <asp:Literal ID="NegotiationVisitStartLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--経過時間--%>
                                                            <div class="Time">8’12”</div>
                                                            <%--顧客名称--%>
                                                            <div class="Name ellipsis">
                                                                <asp:Literal ID="NegotiationCustNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START--%>
                                                            <%--Lマーク--%>
                                                            <div id="NegotiationLIcon" class="LIcon" runat="server" visible="false"></div>
                                                            <%--2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END--%>
                                                            <%--来店人数--%>
                                                            <div id="NegotiationVisitPersonNum" class="Number" runat="server" Visible="false">
                                                                <asp:Literal ID="NegotiationVisitPersonNumLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--車両登録No.--%>
                                                            <div id="NegotiationVclregNoLine" class="String ellipsis" runat="server" visible="false">
                                                                <asp:Literal ID="NegotiationVclregNoLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                            <%--来店手段（歩き）--%>
                                                            <div id="NegotiationMeansWalkLine" class="Person" runat="server" visible="false"></div>
                                                            <%--来店手段（車）--%>
                                                            <div id="NegotiationMeansCarLine" class="Car" runat="server" visible="false"></div>

                                                            <%--顧客担当SC名--%>
                                                            <div id="NegotiationUserName" class="Staff ellipsis" runat="server" Visible="false">
                                                                <asp:Literal ID="NegotiationUserNameLiteral" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
                                                    
                                                        <%--- 左右区切り線 --%>
                                                        <div class="VerticalLine"></div>
        
                                                        <%-- 対応担当SC写真 --%>
                                                        <div id="NegotiationAccountImageAreaNormal" class="Photo" runat="server" Visible="true">
                                                            <asp:Image ID="NegotiationOrgImgfileImage" runat="server" width="42" height="40"></asp:Image>
                                                        </div>
                                                        <div id="NegotiationAccountImageAreaBroadcast" class="ComingStoreOff1-1-4-3" runat="server" Visible="false">
                                                        
                                                        </div>
                                                    
                                                        <%-- 依頼情報 --%>
                                                        <ul id="RequestInfoDiv" class="consultation">
                                                            <asp:Literal ID="RequestInfoLiteral" runat="server"></asp:Literal>
                                                        </ul>

                                                        <%--苦情アイコン--%>
                                                        <div id="NegotiationClaimIcnDiv" class="exmark" runat="server" Visible="false">
                                                            <asp:Literal ID="NegotiationClaimIconLiteral" runat="server"></asp:Literal>
                                                        </div>

                                                        <%-- 商談テーブルNO. --%>
                                                        <div id="NegotiationSalesTableNoCustomer" class="Tmark" runat="server" Visible="false">
                                                            <p>
                                                                <asp:Literal ID="NegotiationSalesTableNoLiteral" runat="server"></asp:Literal>
                                                            </p>
                                                        </div>
                                                    
                                                        <%-- グレーアウト --%>
                                                        <div id="NegotiationInactive" class="Inactive" runat="server" Visible="false"></div>

                                                        <%-- 納車作業枠線 --%>
                                                        <div id="NegotiationDelivery" class="Border_Pink" runat="server" Visible="false"></div>
                                                    
                                                        <%--削除ボタン--%>
                                                        <div class="ComingStoreOff1-1-5 ComingStoreOffDeletionButton" style="display:none;">
                                                            <button class="ComingStoreOff1-1-5-1"><asp:Literal id="NegotiationVisitorDeleteLiteral" runat="server" /></button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <%-- スタッフ状況エリア --%>
            <div class="SC_CassetteBox04">
                <div id="staffFlickArea" style="width: 985px; height:67px; overflow:hidden;">
                    <div id="staffFlickAreaInner">
                        <ul>
                            <asp:Repeater ID="StaffRepeater" runat="server" enableViewState="false">
                                <ItemTemplate>
                                    <li class="StaffChip">
                                        <div id='StuffChipDiv' class="CassetteBack" runat="server">
                                            <%--隠し項目--%>
                                            <asp:HiddenField id="Account" runat="server" Value='<%# Server.HTMLEncode(Eval("ACCOUNT").ToString()) %>'></asp:HiddenField>
                                            <asp:HiddenField id="Status" runat="server" Value='<%# Server.HTMLEncode(Eval("PRESENCECATEGORY").ToString()) %>'></asp:HiddenField>
                                        
                                            <div class="Photo" style="position:relative;">
                                                <div class="ImgFilter" style="position:absolute; top:0; z-index:2; width:38px; height:32px;"></div>
                                                <asp:Image ID="OrgImgFileImage" runat="server" style="z-index:1;" width="38" height="32"></asp:Image>
                                                
                                                <%--紐付き人数--%>
                                                <div id="LinkingCountDiv" class="Icon_Red" runat="server" visible="false" style="z-index:3;">
                                                    <asp:Literal ID="VisitorLinkingCountLiteral" runat="server"></asp:Literal>
                                                </div>
                                            </div>

                                            <%--SC名称--%>
                                            <div class="Name ellipsis" style="position:absolute; top:0;">
                                                <asp:Literal ID="UserNameLiteral" runat="server"></asp:Literal>
                                            </div>

                                            <%-- グレーアウト --%>
                                            <div id='InactiveDiv' class="Inactive" runat="server" visible="false"></div>
                                        </div>
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

<%--オーバーレイが2回目以降出現しなくなってしまうため設定--%>
<div style="clear:both;"></div>

</form>
</div>
</body>
</html>
