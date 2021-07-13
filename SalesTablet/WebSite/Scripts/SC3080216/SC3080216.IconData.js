/**
* @fileOverview SC3080216 当初目標、現在目標、実績のアイコン処理
*
* @author TCS 安田
* @version 1.0.0
*/

//当初目標、現在目標、実績のアイコン操作用クラス
//iconProcess   工程コード文字列(000:受注 , 001:振当 , 002:入金 , 005:納車)
//iconDiv       アイコン表示DIVタグ
//iconDate      日付文字列(yyyyMMdd)
//iconFlg       アイコン分類（1:通常、4:完了）
var IconDataClass = function (iconProcess, iconDiv, iconDate, iconFlg) {

    //工程コード(000:受注 , 001:振当 , 002:入金 , 005:納車)
    this.iconProcess = iconProcess;

    //アイコン表示DIVタグ
    this.iconDiv = iconDiv;
    this.iconDiv.css("display", "none");

    //日付文字列(yyyyMMdd)
    this.iconDate = iconDate;

    //アイコン分類（1:通常、4:完了）
    this.iconFlg = iconFlg;

    //表示左位置
    this.leftPos = 0;

    //前工程クラス
    this.prevIconDataClass = null;

    //後工程クラス
    this.nextIconDataClass = null;

    //前工程、次工程セット
    this.setNestData = function (prev, next) {

        this.prevIconDataClass = prev;
        this.nextIconDataClass = next;
    }

    //アイコンを画面に表示する。
    this.displayIcon = function () {

        //重なりを調査 (工程コードの配列を取得する)
        var kouteiArray = new Array();
        kouteiArray = this.repeatPrpcessArray(kouteiArray, this.iconDate);

        //アイコンのイメージを取得する
        var iconImage = getIconImagePath(this.iconFlg, kouteiArray[0], kouteiArray[kouteiArray.length - 1]);
        this.iconDiv.children("div:nth-child(1)").css("background-image", iconImage);

        //左位置を設定
        this.leftPos = dateCls.getDateToLeft(this.iconDate);
        this.iconDiv.css("left", this.leftPos);

        //表示判定
        if (this.leftPos >= 0 && (this.isDisplay() == true)) {
            //if (this.leftPos >= 0) {
            this.iconDiv.css("display", "block");
        } else {
            this.iconDiv.css("display", "none");
        }
    }

    //表示判定、次工程が同一日ならば表示しない
    this.isDisplay = function () {
        if (this.nextIconDataClass == null) {
            return true;
        }
        if (this.nextIconDataClass.iconDate == this.iconDate) {
            return false;
        } else {
            return true;
        }
    }

    //同一日の工程リストを作成する
    this.repeatPrpcessArray = function (kouteiArray, nextDate) {

        //前工程と同一日ならば、配列に日付をセットする。
        if (nextDate == this.iconDate) {

            //工程コードを追加
            kouteiArray.unshift(this.iconProcess);

            //前工程があれば、前工程
            if (this.prevIconDataClass == null) {
                return kouteiArray;
            } else {
                return this.prevIconDataClass.repeatPrpcessArray(kouteiArray, nextDate);
            }
        } else {
            return kouteiArray;
        }
    }
}
