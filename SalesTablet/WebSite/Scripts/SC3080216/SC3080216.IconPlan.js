/**
* @fileOverview SC3080216 計画のアイコン処理
*
* @author TCS 安田
* @version 1.0.0
* 
* 更新： 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応
*/

//計画のアイコン操作用クラス
//iconProcess     工程コード文字列(000:受注 , 001:振当 , 002:入金 , 005:納車)
//iconDiv         DIVタグ
//iconDateObj     日付文字列(yyyyMMdd)Hiddenオブジェクト
//iconStTimeObj   開始日時Hiddenオブジェクト
//iconEdTimeObj   終了時間Hiddenオブジェクト
//jissekiDateObj  実績日オブジェクト
//mokuhyoDateObj  現在目標日オブジェクト
var IconPlanClass = function (iconProcess, iconDiv, iconDateObj, iconStTimeObj, iconEdTimeObj, jissekiDateObj, mokuhyoDateObj) {

    //工程コード文字列(000:受注 , 001:振当 , 002:入金 , 005:納車)
    this.iconProcess = iconProcess;

    //アイコン表示DIVタグ
    this.iconDiv = iconDiv;
    this.iconDiv.css("display", "none");

    //日付文字列(yyyyMMdd)Hiddenオブジェクト
    this.iconDateObj = iconDateObj;

    //開始日時Hiddenオブジェクト
    this.iconStTimeObj = iconStTimeObj;

    //終了時間Hiddenオブジェクト
    this.iconEdTimeObj = iconEdTimeObj;

    //実績日
    this.jissekiDate = jissekiDateObj.val();
    this.jissekiDateObj = jissekiDateObj;

    //現在目標日
    this.mokuhyoDate = mokuhyoDateObj.val();
    this.mokuhyoDateObj = mokuhyoDateObj;

    //表示左位置
    this.leftPos = 0;

    //前工程クラス
    this.prevIconDataClass = null;

    //後工程クラス
    this.nextIconDataClass = null;

    //最小スライド可能な左位置
    this.leftPosMin = 0;

    //最大スライド可能な左位置
    this.leftPosMax = maxLeftPos;

    //現在目標日の左位置
    this.mokuhyoLeft = dateCls.getDateToLeft(this.mokuhyoDate);

    //アイコンタップステータス
    //初期値・・・０、開始中・・・１、動作した・・・２
    this.tapStatus = 0;

    //前工程、次工程セット
    this.setNestData = function (icon_zorder, prev, next) {

        //z-order順
        this.icon_zorder = icon_zorder;
        this.iconDiv.css("z-index", this.icon_zorder);
        this.iconDiv.children("div:nth-child(1)").css("z-index", this.icon_zorder);

        this.prevIconDataClass = prev;
        this.nextIconDataClass = next;

    }

    //アイコンを種類を取得する
    //アイコン分類 (0:プロセス、1:通常、2:編集、3:遅れ、4:完了)
    this.getIconFlg = function () {

        var genzaiDate = $("#todayDate").val();     //現在日

        //2:編集アイコン
        var iconFlg = 2;

        if (this.jissekiDate != "") {
            //実績日が入っている場合は、4:完了アイコン
            iconFlg = 4;
        } else {
            if (this.iconDateObj.val() < genzaiDate) {
                //現在日を過ぎている場合は、3:遅れアイコン
                iconFlg = 3;
            }
            if ((this.mokuhyoDate != "") && (this.iconDateObj.val() > this.mokuhyoDate)) {
                //現在目標より遅れている場合は、3:遅れアイコン
                iconFlg = 3;
            }
        }

        return iconFlg;
    }

    //アイコンを画面に表示する。
    this.displayIcon = function () {

        //アイコンを種類を取得する
        this.iconFlg = this.getIconFlg();

        //重なりを調査 (工程コードの配列を取得する)
        var kouteiArray = new Array();
        kouteiArray = this.repeatPrpcessArray(kouteiArray, this.iconDateObj.val(), false);

        //アイコンのイメージを取得する
        var iconImage = getIconImagePath(this.iconFlg, kouteiArray[0], kouteiArray[kouteiArray.length - 1]);
        this.iconDiv.children("div:nth-child(1)").css("background-image", iconImage);

        //アイコン枠線の色を設定する
        if (this.iconFlg === 3) {        //3:遅れ
            this.iconDiv.removeClass("borderColor2 borderColor4").addClass("borderColor3");
        } else {
            if (this.iconFlg === 4) {    //4:完了
                this.iconDiv.removeClass("borderColor2 borderColor3").addClass("borderColor4");
            } else {                //2:編集
                this.iconDiv.removeClass("borderColor3 borderColor4").addClass("borderColor2");
            }
        }

        //アイコン位置を調整する
        this.SetIconPosition();

        //表示判定
        //if (this.leftPos >= 0 && (this.isDisplay() == true)) {
        if (this.leftPos >= 0) {

            this.iconDiv.css("display", "block");

        } else {
            this.iconDiv.css("display", "none");
        }

    }

    //アイコン位置を調整する
    this.SetIconPosition = function () {
        //左位置を設定
        this.leftPos = dateCls.getDateToLeft(this.iconDateObj.val());
        this.iconDiv.css("left", this.leftPos);
    }

    //アイコンを傾ける
    this.transformIcon = function () {

        //重なりを調査 (工程コードの配列を取得する)
        var kouteiArray = new Array();
        kouteiArray = this.repeatPrpcessArray(kouteiArray, this.iconDateObj.val(), false);

        //アイコン重複時にアイコンをずらす
        var transformValue = "rotate(0deg)";
        if (kouteiArray.length == 2) {
            transformValue = "rotate(-10deg)";
        }
        if (kouteiArray.length == 3) {
            transformValue = "rotate(10deg)";
        }
        this.iconDiv.css("transform", transformValue);
    }

    //表示判定、次工程が同一日ならば表示しない
    this.isDisplay = function () {
        if (this.nextIconDataClass == null) {
            return true;
        }
        if (this.nextIconDataClass.iconDateObj.val() == this.iconDateObj.val()) {
            return false;
        } else {
            return true;
        }
    }

    //同一日の工程リストを作成する
    this.repeatPrpcessArray = function (kouteiArray, nextDate, jissekiFlg) {

        //実績日入っている場合は対象に含めない
        if (jissekiFlg == true) {
            if (this.jissekiDate != "") {
                return kouteiArray;
            }
        }

        //前工程と同一日ならば、配列に日付をセットする。
        if (nextDate == this.iconDateObj.val()) {

            //工程コードを追加
            kouteiArray.unshift(this.iconProcess);

            //前工程があれば、前工程
            if (this.prevIconDataClass == null) {
                return kouteiArray;
            } else {
                return this.prevIconDataClass.repeatPrpcessArray(kouteiArray, nextDate, jissekiFlg);
            }
        } else {
            return kouteiArray;
        }
    }

    //自身のクラス (イベント処理で使用するため)
    var myClass = this;

    //アイコンタップ開始
    this.iconDiv.bind("mousedown touchstart", function (event) {

        //実績日が入力されている場合は、スライドできない。
        if (myClass.jissekiDate != "") {
            return true;
        }

        var leftPosTemp = 0;

        myClass.leftPosMin = 0; 			//最小左位置
        myClass.leftPosMax = maxLeftPos; 	//最大左位置

        //最小の左位置を取得する
        if (myClass.iconProcess == "001") {
            //001:振当ならば受注日で判定する。
            leftPosTemp = dateCls.getDateToLeft($("#D0Date").val());
            if (leftPosTemp >= 0) {
                myClass.leftPosMin = leftPosTemp;
            }
        } else {
            if (myClass.prevIconDataClass != null) {

                if (myClass.prevIconDataClass.jissekiDate != "") {
                    //前工程の実績日の左位置
                    leftPosTemp = dateCls.getDateToLeft(myClass.prevIconDataClass.jissekiDate);
                } else {
                    //前工程の計画日の左位置
                    leftPosTemp = dateCls.getDateToLeft(myClass.prevIconDataClass.iconDateObj.val());
                }
                if (leftPosTemp >= 0) {
                    myClass.leftPosMin = leftPosTemp;
                }
            }
        }

        //最大の左位置を取得する
        if (myClass.nextIconDataClass != null) {
            if (myClass.nextIconDataClass.jissekiDate != "") {
                //次工程の実績日の左位置
                leftPosTemp = dateCls.getDateToLeft(myClass.nextIconDataClass.jissekiDate);
            } else {
                //次工程の計画日の左位置
                leftPosTemp = dateCls.getDateToLeft(myClass.nextIconDataClass.iconDateObj.val());
            }
            if (leftPosTemp >= 0) {
                myClass.leftPosMax = leftPosTemp;
            }
        }

        //現在目標日の左位置
        myClass.mokuhyoLeft = dateCls.getDateToLeft(myClass.mokuhyoDate);

        if (myClass.mokuhyoLeft < 0) {
            if (myClass.mokuhyoDate < $("#currentDateYYYYMMDD").val()) {
                myClass.mokuhyoLeft = -500;
            } else {
                myClass.mokuhyoLeft = 500;
            }
        }

        //1:タップ開始
        myClass.tapStatus = 1;

        //最前面にする
        //        myClass.iconDiv.css("z-index", 100);
        //        myClass.iconDiv.children("div:nth-child(1)").css("z-index", 100);

        return true;
    });

    //アイコンタップ中
    this.iconDiv.bind("mousemove touchmove", function (event) {

        //1:タップ開始されていなければ処理しない
        if (myClass.tapStatus === 0) {
            event.preventDefault();
            return true;
        }

        //2:スライド開始
        myClass.tapStatus = 2;

        //画面スライド制御を無効にする
        icropScript.ui.bypassPreventDefault = true;

        //左スライド位置を求める
        var x = Number(event.pageX) - maginX;

        if (x < myClass.leftPosMin) {
            //最小左位置
            x = myClass.leftPosMin;
        } else {
            if (x > myClass.leftPosMax) {
                //最大左位置
                x = myClass.leftPosMax;
            }
        }

        //スライドした位置にスライドする
        myClass.iconDiv.css("left", x);

        //アイコン分類 (0:プロセス、1:通常、2:編集、3:遅れ、4:完了)        
        //現在目標より後、もしくは現在日より前
        if ((x > myClass.mokuhyoLeft + 20) || (x < currentDateLeftPos)) {
            myClass.iconFlg = 3; //3:遅れ
        } else {
            myClass.iconFlg = 2; //2:編集
        }

        //アイコンのイメージを取得する
        var iconImage = getIconImagePath(myClass.iconFlg, myClass.iconProcess, myClass.iconProcess);
        myClass.iconDiv.children("div:nth-child(1)").css("background-image", iconImage);
        //アイコン重複時にアイコンをずらす
        myClass.iconDiv.css("transform", "rotate(0deg)");

        if (myClass.iconFlg === 3) {        //3:遅れ
            myClass.iconDiv.removeClass("borderColor2 borderColor4").addClass("borderColor3");
        } else {
            if (myClass.iconFlg === 4) {    //4:完了
                myClass.iconDiv.removeClass("borderColor2 borderColor3").addClass("borderColor4");
            } else {                //2:編集
                myClass.iconDiv.removeClass("borderColor3 borderColor4").addClass("borderColor2");
            }
        }

        return false;
    });

    //アイコンタップ終了
    this.iconDiv.bind("mouseup touchend", function (event) {

        //2:スライド開始されていなければ処理しない
        if (myClass.tapStatus != 2) {
            myClass.tapStatus = 0;
            event.preventDefault();
            return true;
        }

        //0:タップ開始前
        myClass.tapStatus = 0;
        myClass.iconDiv.css("z-index", this.icon_zorder);
        myClass.iconDiv.children("div:nth-child(1)").css("z-index", this.icon_zorder);

        //現在アイコンのある日付を取得する
        var posDate = dateCls.positionToDate(myClass.iconDiv);

        //日付に変更がない場合は、アイコンの再表示のみする
        if (posDate === myClass.iconDateObj.val()) {
            myClass.displayIcon();

            setTimeout(function () {
                //アイコンを傾ける
                myClass.transformIcon();
            }, 300);

            event.preventDefault();
            return true;
        }

        //日付項目を設定する
        myClass.iconDateObj.val(posDate); 			//YYYYMMDD文字列
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        myClass.iconStTimeObj.val(geDateTimelocalDate(getParseDateValue(posDate)));
        //myClass.iconStTimeObj.get(0).valueAsDate = getParseDateValue(posDate);
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
        myClass.iconEdTimeObj.val(""); 				//時間はクリアする

        //計画アイコン際表示
        iconClsC1.displayIcon();
        iconClsC2.displayIcon();
        iconClsC3.displayIcon();
        
        setTimeout(function () {
            //アイコンを傾ける
            iconClsC1.transformIcon();
            iconClsC2.transformIcon();
            iconClsC3.transformIcon();
        }, 300);
        //重なっていることを考慮した最上位のDivを取得する
        var divTemp = myClass.iconDiv;
        var classTemp = myClass;
        while (classTemp != null) {
            if (myClass.iconDateObj.val() === classTemp.iconDateObj.val()) {
                divTemp = classTemp.iconDiv;
            }
            classTemp = classTemp.nextIconDataClass;
        }

        //ポップアップを閉じる
        $("#bodyFrame").trigger("click.popover");

        //日付指定ポップアップを表示する
        setTimeout(function () {
            divTemp.click();
        }, 200);
        

        //画面スライド制御を有効にする
        icropScript.ui.bypassPreventDefault = false;

        return true;
    });
}
