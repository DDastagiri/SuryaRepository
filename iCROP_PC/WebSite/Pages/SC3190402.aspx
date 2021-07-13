<%@ Import Namespace="Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic" %>
<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.master" AutoEventWireup="false" CodeFile="SC3190402.aspx.vb" Inherits="Pages_SC3190402" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" href="../Styles/SC3190402/SC3190402.css?20140922000000" type="text/css" media="screen,print" />
    <%-- $02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 START --%> 
    <script type="text/javascript" src="../Scripts/icrop.push.js?20170710000000"></script> 
    <script type="text/javascript" src="../Scripts/icrop.clientapplication.js?20170710000000"></script>
    <%-- $02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 END --%> 
    <script type="text/javascript" src="../Scripts/SC3190402/SC3190402.js?20170710000001"></script>
    <script type="text/javascript" src="../Scripts/icropBase.js?20140922000000"></script>
    <script type="text/javascript">
        //ユーザー名取得API実行
        icropBase.getUser = function () {
            return "<%=StaffContext.Current.Account%>";
        }
        //M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05

        // 遅れ更新日時
        var gDelayUpdateDateTime;

        // 遅れ更新間隔（ミリ秒）
        var gDelayUpdateInterval;

        //遅れの更新処理を設定する（初回）
        function SetUpdateIntervalTime(year, month, day, hour, minutes, seconds, interval) {
         // 更新日時の設定
		    gDelayUpdateDateTime = new Date(year, month - 1, day, hour, minutes, 0);
		    
		    // 遅れ更新間隔（ミリ秒）の設定
		    gDelayUpdateInterval = interval;
	
		    var intervalFirst;
	
	        //タイムアウトの計算
	        if (seconds == 0) {
	            // 0は60秒として計算する
	            intervalFirst = (interval + 1000) - 60 * 1000;
	        } else {
	            intervalFirst = (interval + 1000) - seconds * 1000;
	        }
	         
	        // 次回Timeoutを設定
	        setTimeout('SetUpdateIntervalTimeSecond()', intervalFirst);
        }
        // 遅れの更新処理を設定する（2回目）
	    function SetUpdateIntervalTimeSecond() {
		    // 遅れ更新処理の定期実行の設定を行う
		    setInterval('updateIntervalTime()', gDelayUpdateInterval);
		    
		    // 遅れの更新処理を行う
		    updateIntervalTime();
	    }
	    // 遅れの更新処理を行う
	    function updateIntervalTime() {

            //更新日時を更新する
            SetgDelayUpdateDateTime();

            //各エリアを更新する
            //見積待ち
            Area01UpDate();
            //作業計画待ち
            Area02UpDate();
            //出庫待ち
            Area03UpDate();
            //引き取り待ち
            Area04UpDate();
        }

        //更新日時の計算
        function SetgDelayUpdateDateTime() {
            //経過した(分)を加算する
            var addMinutes = gDelayUpdateInterval / 60000;
            gDelayUpdateDateTime.setMinutes(gDelayUpdateDateTime.getMinutes() + addMinutes);
            //日付フォーマットを取得する
            var dateFormat = document.getElementById('hidDateFormat').value;
            var timeFormat = document.getElementById('hidTimeFormat').value;
            //年の置き換え
            dateFormat = dateFormat.replace('yyyy', gDelayUpdateDateTime.getFullYear());
            //月の置き換え
            if ((gDelayUpdateDateTime.getMonth() + 1) < 10) {
                dateFormat = dateFormat.replace('MM', '0' + (gDelayUpdateDateTime.getMonth() + 1));
            } else {
                dateFormat = dateFormat.replace('MM', (gDelayUpdateDateTime.getMonth() + 1));
            }
            //日付の置き換え
            if (gDelayUpdateDateTime.getDate() < 10) {
                dateFormat = dateFormat.replace('dd', '0' + gDelayUpdateDateTime.getDate());
            } else {
                dateFormat = dateFormat.replace('dd', gDelayUpdateDateTime.getDate());
            }
            //時間の置き換え
            if (gDelayUpdateDateTime.getHours() < 10) {
                timeFormat = timeFormat.replace('HH', '0' + gDelayUpdateDateTime.getHours());
            } else {
                timeFormat = timeFormat.replace('HH', gDelayUpdateDateTime.getHours());
            }
            if (gDelayUpdateDateTime.getMinutes() < 10) {
                timeFormat = timeFormat.replace('mm', '0' + gDelayUpdateDateTime.getMinutes());
            } else {
                timeFormat = timeFormat.replace('mm', gDelayUpdateDateTime.getMinutes());
            }
            //時間をセットする
            document.getElementById('divUpdateDate').textContent = dateFormat;  //日付
            document.getElementById('lblUpdateTime').textContent = timeFormat;  //時間
        }

        function Area01UpDate() {
            //見積待ちエリア
            var detail01 = document.getElementById('ulSubAreaBox01').getElementsByClassName('divSubAreaDetail');
            var subArea01 = document.getElementById('ulSubAreaBox01').getElementsByClassName('divAreaBack');

            for (var i = 0; i < detail01.length; i++) {
                //件数回数ループ
                if (detail01[i].attributes[1] !== undefined) {
                    var targetDateArray = detail01[i].attributes[1].textContent.split(",");
                    var targetDate = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);
                    //遅れの場合、背景を赤くする
                    if (targetDate < gDelayUpdateDateTime && subArea01[i].className == "divAreaBack") {
                        subArea01[i].className = "divAreaBack BackColorRed";
                    }
                }
            }
        }

        function Area02UpDate() {
            //作業計画待ち
            var detail02 = document.getElementsByClassName('divSubAreaDetail02');
            var subArea02 = document.getElementById('ulSubAreaBox02').getElementsByClassName('divAreaBack');

            for (var i = 0; i < detail02.length; i++) {
                //件数回数ループ
                if (detail02[i].attributes[1] !== undefined) {
                    var targetDateArray = detail02[i].attributes[1].textContent.split(",");
                    var targetDate = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);
                    //遅れの場合、背景を赤くする
                    if (targetDate < gDelayUpdateDateTime && subArea02[i].className == "divAreaBack") {
                        subArea02[i].className = "divAreaBack BackColorRed";
                    }
                }
            }
        }

        function Area03UpDate() {
            //出庫待ち
            var areaLi = document.getElementsByClassName('ulSubAreaBoxIn');
            var detail03 = document.getElementById('ulSubAreaBox03').getElementsByClassName('divSubAreaDetail');
            var subArea03 = document.getElementById('ulSubAreaBox03').getElementsByClassName('divAreaBack');

            for (var i = 0; i < detail03.length; i++) {
                //件数回数ループ
                if (detail03[i].attributes[1] !== undefined) {
                    var targetDateArray = detail03[i].attributes[1].textContent.split(",");
                    var targetDate = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);
                    //遅れの場合、背景を赤くする
                    if (targetDate < gDelayUpdateDateTime && subArea03[i].className == "divAreaBack") {
                        subArea03[i].className = "divAreaBack BackColorRed";
                        //第1ソート:遅れ判定、 第2ソート:予定開始日時,　第3ソート:RO_NUM　,第4ソートRO_SEQ
                        for (var j = 0; j < i; j++) {
                            if (subArea03[j].className == "divAreaBack") {
                                $(areaLi[2].children[j]).before(areaLi[2].children[i]);
                                break;
                            }

                            targetDateArray = detail03[j].attributes[1].textContent.split(",");
                            var targetDateAfter = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);

                            if (subArea03[j].className == "divAreaBack BackColorRed"
                            && targetDate < targetDateAfter) {
                                    $(areaLi[2].children[j]).before(areaLi[2].children[i]);
                                    break;
                            }
                            if (subArea03[j].className == "divAreaBack BackColorRed"
                            && targetDate == targetDateAfter
                            && detail03[i].attributes[2].value < detail03[j].attributes[2].value) {
                                    $(areaLi[2].children[j]).before(areaLi[2].children[i]);
                                    break;
                            }
                            if (subArea03[j].className == "divAreaBack BackColorRed"
                            && targetDate == targetDateAfter
                            && detail03[i].attributes[2].value == detail03[j].attributes[2].value
                            && Number(detail03[i].attributes[3].value) < Number(detail03[j].attributes[3].value)) {
                                    $(areaLi[2].children[j]).before(areaLi[2].children[i]);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        function Area04UpDate() {
            //引取待ち
            var areaLi = document.getElementsByClassName('ulSubAreaBoxIn');
            var detail04 = document.getElementById('ulSubAreaBox04').getElementsByClassName('divSubAreaDetail');
            var subArea04 = document.getElementById('ulSubAreaBox04').getElementsByClassName('divAreaBack');

            for (var i = 0; i < detail04.length; i++) {
                //件数回数ループ
                if (detail04[i].attributes[1] !== undefined) {
                    var targetDateArray = detail04[i].attributes[1].textContent.split(",");
                    var targetDate = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);
                    //遅れの場合、背景を赤くする
                    if (targetDate < gDelayUpdateDateTime && subArea04[i].className == "divAreaBack") {
                        subArea04[i].className = "divAreaBack BackColorRed";
                        //第1ソート:遅れ判定、 第2ソート:予定納車日時,　第3ソート:RO_NUM　,第4ソートRO_SEQ
                        for (var j = 0; j < i; j++) {
                            if (subArea04[j].className == "divAreaBack") {
                                    $(areaLi[3].children[j]).before(areaLi[3].children[i]);
                                    break;
                            }

                            targetDateArray = detail04[i].attributes[2].textContent.split(",");
                            var targetDateBefore = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);

                            targetDateArray = detail04[j].attributes[2].textContent.split(",");
                            var targetDateAfter = new Date(targetDateArray[0], targetDateArray[1] - 1, targetDateArray[2], targetDateArray[3], targetDateArray[4], 0);


                            if (subArea04[j].className == "divAreaBack BackColorRed"
                            && targetDateBefore < targetDateAfter) {
                                    $(areaLi[3].children[j]).before(areaLi[3].children[i]);
                                    break;
                            }
                            if (subArea04[j].className == "divAreaBack BackColorRed"
                            && targetDateBefore == targetDateAfter
                            && detail04[i].attributes[3].value < detail04[j].attributes[3].value) {
                                    $(areaLi[3].children[j]).before(areaLi[3].children[i]);
                                    break;
                            }
                            if (subArea04[j].className == "divAreaBack BackColorRed"
                            && targetDateBefore == targetDateAfter
                            && detail04[i].attributes[3].value == detail04[j].attributes[3].value
                            && Number(detail04[i].attributes[4].value) < Number(detail04[j].attributes[4].value)) {
                                    $(areaLi[3].children[j]).before(areaLi[3].children[i]);
                                    break;
                            }
                        }
                    }
                }
            }
        }
        //M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

        //2017/07/15 改ページ機能追加
        //インジケーター設定関数
        function SetIndicator(area, indi, pos, page_cnt, disp_cnt, interval) {
            //alert(area + ',' + indi + ',' + pos + ',' + page_cnt + ',' + disp_cnt + ',' + interval);

            //1ページあたりの高さを算出
            var page_height = $('ul.ulSubAreaBoxIn li').height() * disp_cnt;

            //表示すべき開始位置を取得(pos:1 = margin-top:0)
            var margin_top = -((pos - 1) * page_height);

            //Top位置を変更
            document.getElementById(area).style.marginTop = margin_top + "px";

            //インジケーターの設定
            for (var i = 1, cnt = 10; i <= cnt; ++i) {
                //現在位置のインジケーターはONとする
                if (i <= page_cnt) {
                    //ページカウント以下のインジケーターは現在位置により分岐してセットする
                    if (i == pos) {
                        //現在位置のインジケーターはONとする
                        document.getElementById(indi + i).className = "divIndicator IndicatorOn";
                    } else {
                        //現在位置以外のインジケーターはOFFとする
                        document.getElementById(indi + i).className = "divIndicator IndicatorOff";
                    }
                } else {
                    //ページカウントより大きいインジケーターは非表示とする
                    document.getElementById(indi + i).className = "divIndicator IndicatorNone";
                }
            }
            //次ページの算出
            if (pos == page_cnt) {
                //現在位置が最終ページに到達したら1に戻す
                pos = 1;
            } else {
                //現在位置が最終ページでなかったら1加算する
                pos += 1;
            }
            //パラメータ設定
            var str = 'SetIndicator(\'' + area + '\',\'' + indi + '\',' + pos + ',' + page_cnt + ',' + disp_cnt + ',' + interval + ')';
            //次回Timeoutを設定
            setTimeout(str, interval);
            return false;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!-- ここからメインブロック -->
    <!-- M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05 -->
    <asp:HiddenField ID="hidDateFormat" runat="server"/>
    <asp:HiddenField ID="hidTimeFormat" runat="server"/>
    <!-- M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05 -->
    <div id="Main">
        <%--処理中のローディング--%>
        <div id="SC3190402_LoadingScreen" class="registOverlay">
            <div class="registWrap">
                <div class="processingServer"></div>
            </div>
        </div>

        <!-- ここからヘッダ -->
        <div class="divMainHeader">
            <icrop:CustomLabel class="divMainHeaderTitleLeft" TextWordNo="1" UseEllipsis="false" runat="server"></icrop:CustomLabel>
            <div class="divMainHeaderTitleRight">
                <asp:Label id="divUpdateDate" runat="server"></asp:Label>
                <asp:Label class="upDateTime" id="lblUpdateTime" runat="server"></asp:Label>
            </div>
        </div>
        <!-- ここまでヘッダ -->
        <!-- ここからコンテンツ -->
        <div id="contents">
        <!-- 隠しボタン -->
        <asp:Button ID="hdnBtnRefreshPage" runat="server" CssClass="HiddenBtn"/>
        <!-- 画面遷移用隠しボタン -->
        <asp:Button ID="hdnBtnMovePage" runat="server" CssClass="HiddenBtn" />
            <div class="divMainFrame">
                <div class="divMainArea">           
                    <!-- 見積り待ちエリア -->
                    <asp:Panel ID="pnlArea01" runat="server">
                        <div class="divArea01">
                            <div class="divAreaTitleHeader">
                                <icrop:CustomLabel class="divAreaTitle" TextWordNo="2" UseEllipsis="false" runat="server"></icrop:CustomLabel>
                                <icrop:CustomLabel class="divAreaCount" id="lblAreaCount01" UseEllipsis="false" runat="server"></icrop:CustomLabel>
                            </div>
                            <div class="divAreaSub01">
 		                        <div class="divMainAreaBoxIn">
                                    <ul class="ulSubAreaBoxIn" id="ulSubAreaBox01" runat="Server"></ul>
                                </div>
	                        </div>
                            <div class="divAreaSub02_Area01">
                            <!-- インジケーター -->
                                <table><tr>
                                    <td><div id="Indi01_1" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_2" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_3" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_4" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_5" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_6" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_7" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_8" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_9" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi01_10" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                </tr></table>
                            </div>
                        </div>
                    </asp:Panel>            
                    <!-- 作業計画待ちエリア -->
                    <asp:Panel ID="pnlArea02" runat="server">
                        <div class="divArea02">
                            <div class="divAreaTitleHeader TitlePatternArea02">
                                <asp:Label class="divAreaTitle" id="lblArea02Title" runat="server"></asp:Label>
                                <icrop:CustomLabel class="divAreaCount" id="lblAreaCount02" UseEllipsis="false" runat="server"></icrop:CustomLabel>
                            </div>
                            <div class="divAreaSub01">
                                <div class="divMainAreaBoxIn">
                                    <ul class="ulSubAreaBoxIn" id="ulSubAreaBox02" runat="Server"></ul>
                                </div>
	                        </div>
                            <div class="divAreaSub02_Area02">
                            <!-- インジケーター -->
                                <table><tr>
                                    <td><div id="Indi02_1" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_2" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_3" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_4" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_5" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_6" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_7" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_8" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_9" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi02_10" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                </tr></table>
                            </div>
                        </div>
                    </asp:Panel>            
                    <!-- 出庫待ちエリア -->
                    <asp:Panel ID="pnlArea03" runat="server">
                        <div class="divArea03">
	                        <div class="divAreaTitleHeader">
                                <icrop:CustomLabel class="divAreaTitle" TextWordNo="4" UseEllipsis="false" runat="server"></icrop:CustomLabel>
                                <icrop:CustomLabel class="divAreaCount" id="lblAreaCount03" UseEllipsis="false" runat="server"></icrop:CustomLabel>
	                        </div>
                            <div class="divAreaSub01">
	                            <div class="divMainAreaBoxIn">
                                     <ul class="ulSubAreaBoxIn" id="ulSubAreaBox03" runat="Server"></ul>
	                            </div>
	                        </div>
                            <div class="divAreaSub02_Area03">
                            <!-- インジケーター -->
                                <table><tr>
                                    <td><div id="Indi03_1" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_2" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_3" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_4" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_5" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_6" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_7" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_8" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_9" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi03_10" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                </tr></table>
                            </div>
                        </div>
                    </asp:Panel>            
                    <!-- 引き取り待ちエリア -->
                    <asp:Panel ID="pnlArea04" runat="server">
                        <div class="divArea04">
	                        <div class="divAreaTitleHeader">
                                <icrop:CustomLabel class="divAreaTitle" TextWordNo="5" UseEllipsis="false" runat="server"></icrop:CustomLabel>
                                <icrop:CustomLabel class="divAreaCount" id="lblAreaCount04" UseEllipsis="false" runat="server"></icrop:CustomLabel>
	                        </div>
                            <div class="divAreaSub01">
	                            <div class="divMainAreaBoxIn">
                                    <ul class="ulSubAreaBoxIn" id="ulSubAreaBox04" runat="Server"></ul>
	                            </div>
	                        </div>
                            <div class="divAreaSub02_Area04">
                            <!-- インジケーター -->
                                <table><tr>
                                    <td><div id="Indi04_1" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_2" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_3" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_4" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_5" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_6" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_7" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_8" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_9" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                    <td><div id="Indi04_10" class="divIndicator IndicatorNone" runat="Server"></div></td>
                                </tr></table>
                            </div>
                        </div>
                    </asp:Panel>            
                </div>
            </div>
        </div>
        <div id="hidden" style="display:none;">
<%--    各フレームのリフレッシュ用ボタンを用意する？        <asp:button --%>
        </div>
    </div>
</asp:Content>
