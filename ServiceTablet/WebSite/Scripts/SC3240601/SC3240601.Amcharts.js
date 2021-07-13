/** 
* @fileOverview SC3240601.Amcharts.js
* 
* @author TMEJ 陳
* @version 1.0.0
* 作成： 2014/07/09 TMEJ 陳 タブレット版SMB（テレマ走行距離機能開発）
* 更新： 
*/

// Amchart Object
var chart;

// Amchart Cursor Object（）
var chartCursor;
// Amchart ValueAxis Object（X Axis）
var valueAxis;
// Amchart Legend Object(マーク標記）
var legend;

var C_COLOR_LINE1 = "#00d8ff"; // グラフ線色：青
var C_COLOR_LINE2 = "#fcff00"; // グラフ線色：黄
var C_COLOR_LINE3 = "#e2005a"; // グラフ線色：赤
var C_COLOR_LINE4 = "#00e200"; // グラフ線色：緑

var C_GRAPH_FIELD_REGDATE = "REGDATE";          // グラフフィールド：日付
var C_GRAPH_FIELD_REGMILE = "REGMILE";          // グラフフィールド：走行距離
var C_GRAPH_FIELD_DESCRIPTION = "DESCRIPTION";  // グラフフィールド：内容
var C_GRAPH_FIELD_LINECOLOR = "LINECOLOR";      // グラフフィールド：線色

/**
* グラフデータ更新
* @return {void}
*/
function UpdateChartData() {

    if (chart == undefined || $("#HiddenGraphDataField").val() == "")
    { return; }

	//グラフデータを更新
    chart.dataProvider.shift();
    chart.dataProvider = JSON.parse($("#HiddenGraphDataField").val());
    chart.validateData();

    //ZOOM最初化
    zoomChart();
}

/**
* グラフ初期化
* @return {void}
*/
function DrawCharts() {
    AmCharts.ready(function () {

        //グラフ全体設定
        // SERIAL CHART    
        chart = new AmCharts.AmSerialChart();
        chart.pathToImages = "../Scripts/SC3240601/amcharts/images/";
        chart.dataProvider = [];
        chart.categoryField = C_GRAPH_FIELD_REGDATE;
        chart.height = "100%";

        // ズームアウトボタンを消す
        chart.zoomOutButton = null;
        chart.zoomOutText = null;

        // グラフの背景色
        chart.fontFamily = "Helvetica";
        chart.fontSize = 14;

        var balloon = chart.balloon;
        balloon.bulletSize = 5;
        balloon.textAlign = "left";
        balloon.adjustBorderColor = false;
        balloon.cornerRadius = 5;
        balloon.color = "#FFFFFF";
        balloon.fillAlpha = 1;

        // 凡例
        legend = new AmCharts.AmLegend();
        legend.markerType = "circle";
        legend.markerSize = 12;
        legend.valueText = "";
        legend.fontSize = 14;
        legend.equalWidths = false;
        legend.valueWidth = 80;
        legend.position = "bottom";
        legend.switchable = false;
        legend.autoMargins = false;
        legend.marginLeft = 5;
        legend.marginTop = 0;
        legend.marginRight = 5;
        legend.marginBottom = 0;
        legend.verticalGap = 0;
        legend.color = "#FFFFFF";

        chart.addLegend(legend);

        // value 
        valueAxis = new AmCharts.ValueAxis();
        valueAxis.axisAlpha = 1;
        valueAxis.gridColor = "#000000";
        valueAxis.gridAlpha = 0.7;
        valueAxis.dashLength = 1.5;
        valueAxis.autoGridCount = false;
        valueAxis.unit = $("#HiddenKm").val().toString();
        valueAxis.fillAlpha = 0.5;
        valueAxis.fillColor = "#2b2b2b";
        valueAxis.color = "#FFFFFF";
        chart.addValueAxis(valueAxis);

        // AXES
        // category
        var categoryAxis = chart.categoryAxis;
        categoryAxis.parseDates = true;
        categoryAxis.minPeriod = "DD";
        categoryAxis.position = "top";
        categoryAxis.autoGridCount = false;
        categoryAxis.gridCount = 30;
        categoryAxis.axisColor = "#000000";
        categoryAxis.color = "#FFFFFF";

        // 軸の色変更
        categoryAxis.gridColor = "#000000";
        categoryAxis.gridAlpha = 0.7;
        categoryAxis.dashLength = 1.5;
        categoryAxis.dateFormats = [{
            period: 'DD',
            format: 'DD'
        }, {
            period: 'WW',
            format: 'MMM DD'
        }, {
            period: 'MM',
            format: 'MMM'
        }, {
            period: 'YYYY',
            format: 'YYYY'
        }];

        //  線 (1 本目)
        var graph = new AmCharts.AmGraph();
        graph.title = $("#HiddenGraphLegend1").val();
        graph.valueField = C_GRAPH_FIELD_REGMILE;
        graph.bullet = "round";
        graph.lineThickness = 1;
        graph.lineColor = C_COLOR_LINE1;
        graph.colorField = C_GRAPH_FIELD_LINECOLOR;
        graph.hideBulletsCount = 366;
        // マウスオーバー時に表示される吹き出しのテキスト
        graph.balloonText = "[[value]]" + $("#HiddenKm").val().toString() + "\r\n[[" + C_GRAPH_FIELD_DESCRIPTION + "]]";
        chart.addGraph(graph);

        //  線 (2 本目)
        var graph2 = new AmCharts.AmGraph();
        graph2.title = $("#HiddenGraphLegend2").val();
        graph2.lineColor = C_COLOR_LINE2;
        graph2.balloonColor = C_COLOR_LINE2;
        chart.addGraph(graph2);

        //  線 (3 本目)
        var graph3 = new AmCharts.AmGraph();
        graph3.title = $("#HiddenGraphLegend3").val();
        graph3.lineColor = C_COLOR_LINE3;
        graph3.balloonColor = C_COLOR_LINE3;
        chart.addGraph(graph3);

        //  線 (4 本目)
        var graph4 = new AmCharts.AmGraph();
        graph4.title = $("#HiddenGraphLegend4").val();
        graph4.lineColor = C_COLOR_LINE4;
        graph4.balloonColor = C_COLOR_LINE4;
        chart.addGraph(graph4);

        // CURSOR
        chartCursor = new AmCharts.ChartCursor();
        chartCursor.zoomable = false;
        chartCursor.categoryBalloonDateFormat = 'MM/DD/YYYY';
        chartCursor.categoryBalloonEnabled = false;
        chart.addChartCursor(chartCursor);

        // SCROLLBAR
        var chartScrollbar = new AmCharts.ChartScrollbar();
        chartScrollbar.resizeEnabled = false;
        chartScrollbar.autoGridCount = false;
        chartScrollbar.selectedBackgroundColor = "#272727"
        chart.addChartScrollbar(chartScrollbar);

        // GUIDES 
        var guide = new AmCharts.Guide();
        guide.fillColor = "#272727";
        guide.fillAlpha = 1;
        guide.lineAlpha = 0;
        valueAxis.addGuide(guide);

        // WHRITE
        chart.write("MileageGraph");

        //データ更新
        UpdateChartData();
    });
}

/**
* スケール初期化
* @return {void}
*/
function zoomChart() {
	//システム設定値より初期スケールモードでグラフスケールさせる
    changeZoomChart($("#HiddenMileScaleInit").val().toString());
}

/**
* Day,Month,Weekのスケール機能実現
* @return {void}
*/
function changeZoomChart(aChangeView) {

    var mileScaleDay = 0;
    var mileScaleWeek = 0;
    var mileScaleMonth = 0;
    var mileMaxValue = 0;

	//グラフスケール日数
    mileScaleDay = $("#HiddenMileScaleDayCount").val();
    //グラフスケール週数
    mileScaleWeek = $("#HiddenMileScaleWeeklyCount").val();
    //グラフスケール月数
    mileScaleMonth = $("#HiddenMileScaleMonthCountDays").val();

    if (aChangeView == "1") {
    	//日数よりスケール

		//グラフ表示区間を計算    	
        var date1 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date1.setDate(date1.getDate() - mileScaleDay);
        var date2 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date2.setDate(date2.getDate() + 1);

        var fromDate = new Date(date1);
        var toDate = new Date(date2);

		//グラフスケール実行
        chart.zoomToDates(fromDate, toDate);

		//スケールボタン制御
        $(".Btn01").attr("class", "Btn01 BtnOn");
        $(".Btn02").attr("class", "Btn02 BtnOFF");
        $(".Btn03").attr("class", "Btn03 BtnOFF");
    }
    else if (aChangeView == "2") {
    	//週数よりスケール

		//グラフ表示区間を計算  
        var date1 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date1.setDate(date1.getDate() - mileScaleWeek);
        var date2 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date2.setDate(date2.getDate() + 1);

        var fromDate = new Date(date1);
        var toDate = new Date(date2);

		//グラフスケール実行
        chart.zoomToDates(fromDate, toDate);

		//スケールボタン制御
        $(".Btn01").attr("class", "Btn01 BtnOFF");
        $(".Btn02").attr("class", "Btn02 BtnOn");
        $(".Btn03").attr("class", "Btn03 BtnOFF");
    }
    else if (aChangeView == "3") {
    	//月数よりスケール

		//グラフ表示区間を計算  
        var date1 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date1.setDate(date1.getDate() - mileScaleMonth);
        var date2 = new Date(Date.parse($("#HiddenGraphEndDate").val()));
        date2.setDate(date2.getDate() + 1);

        var fromDate = new Date(date1);
        var toDate = new Date(date2);

        //グラフスケール実行
        //chart.zoomToDates(fromDate, toDate);
        chart.zoomToDates(fromDate, toDate);

		//スケールボタン制御
        $(".Btn01").attr("class", "Btn01 BtnOFF");
        $(".Btn02").attr("class", "Btn02 BtnOFF");
        $(".Btn03").attr("class", "Btn03 BtnOn");
    }

}
