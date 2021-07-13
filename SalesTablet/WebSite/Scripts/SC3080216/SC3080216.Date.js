/**
* @fileOverview SC3080216 日付リスト処理
*
* @author TCS 安田
* @version 1.0.0
*/

//日付リスト操作クラス
var DateDataClass = function () {

    //日付リスト
    var $datelist = $('#confirmContents60 .DatesBox .InnerSet .DateList').children();
    this.dateList = new Array($datelist.length);

    //左位置リスト
    this.leftList = new Array($datelist.length);

    //内容セット
    for (i = 0; i < $datelist.length; i++) {
        var dateTarget = "" + $("#" + $datelist[i].id + "").attr("value");    //日付
        var leftPos = $("#litypeA_" + (i + 1)).get(0).offsetLeft + 1;
        this.dateList[i] = dateTarget;
        this.leftList[i] = leftPos;
    }

    //日付から、左位置を求める
    this.getDateToLeft = function (selectDate) {
        for (i = 0; i < this.dateList.length; i++) {
            if (this.dateList[i] == selectDate) {
                return this.leftList[i];
            }
        }
        return -1;
    }

    //現在アイコンDIV位置から日付を算出
    this.positionToDate = function (divItem) {

        var marginPt = 20;
        var xpos = divItem.get(0).offsetLeft + 1;
        for (i = 0; i < this.dateList.length; i++) {

            var leftPos = this.leftList[i] - marginPt;
            var leftPosNext = 0;
            if (i >= (this.leftList.length - 1)) {
                leftPosNext = leftPos + 100;
            } else {
                leftPosNext = this.leftList[i + 1] - marginPt + 5;
            }

            if ((xpos > leftPos) && (xpos < leftPosNext)) {
                return this.dateList[i];
            }
        }
    }

    //最大の左位置を求める
    this.getMaxLeftPos = function () {
        return this.leftList[this.leftList.length - 1];
    }
}