//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3060101.js
//─────────────────────────────────────
//機能： 査定チェックシート
//補足：
//作成： 2011/11/26 KN 清水
//更新： 2012/03/09 KN 清水 【SALES_1B】UT課題No36対応・ピンチ箇所を中心に車両画像を拡大するように修正。
//更新： 2012/03/13 KN 清水 【SALES_1B】UT課題No35対応・車両画像が原寸より縮小するように修正。
//                                      合わせて、指が離れた瞬間に画像を閉じるように修正。
//─────────────────────────────────────



var slideFlg;

//var bigImagesize = 334;
//var bigImageTop = "200px";
//var bigImageLeft = "350px";

var bigImagesize = 660;
//2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 START
var bigImagesizeH = 495;
//2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 END
var bigImageTop = "80px";
var bigImageLeft = "182px";


// 拡大表示されている画像Noを保持
var bigImageNo = -1;

//サムネイル画像選択時のサムネイル画像拡大率
var sumLarge = 0.5;

//サムネイル画像選択時のサムネイル画像Div左寄せピクセル
var sumMargin = 95;

/**
* 新規活動中かどうかを示すフラグ(aspx.vb側からセットされる)
*/
var newActivityFlg = false;
/**
* 新規活動中に他画面に行こうとした場合に表示するメッセージ(aspx.vb側からセットされる)
*/
var redirectMessage = "";

//2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 START

//マルチタップによる、タップ座標格納用（車両画像拡大縮小用）・指一本目
var firstTouchX;
var firstTouchY;

//マルチタップによる、タップ座標格納用（車両画像拡大縮小用）・指二本目
var secoundTouchX;
var secoundTouchY;

//シングルタップによる、タップ座標格納用（車両画像移動用）
var firstTouchXMove;
var firstTouchYMove;
//2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 END


$(function ($) {

    // 画面読み込み中アイコンを表示
    showLoadingIcon();

    //拡大画像読み込み終了時イベント設定
    $(popImg).bind('load', function () {
        hideLoadingIcon();
    });


    //画像読み込み失敗時イベント設定
    $(popImg).bind('error', function () {
        hideLoadingIcon();
    });

    //装備閉じるアイコンが画面に表示されていれば、押下イベントを追加
    if (document.getElementById("downImg") != null) {
        document.getElementById("downImg").addEventListener("touchend", divSlideDown, true);
    }
    // ゼスチャーイベント開始時の画像サイズ
    var orgSize;

    //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 START
    document.getElementById("popImg").addEventListener("touchstart", touchEventHandler, false);
    document.getElementById("popImg").addEventListener("touchmove", touchEventHandlerMove, false);
    document.getElementById("popImg").addEventListener("touchend", touchEventHandlerEnd, false);
    //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 END


    document.getElementById("popImg").addEventListener("gesturestart", pinchInEventHandler, false);
    document.getElementById("popImg").addEventListener("gesturechange", pinchOutEventHandler, false);




    //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 START
    function touchEventHandler(e) {

        event.preventDefault();

        // 初回タップ座標を取得（移動処理用）
        firstTouchXMove = e.touches[0].pageX;
        firstTouchYMove = e.touches[0].pageY;


    }

    /**
    * タッチ移動イベント
    * 
    * @param {e} イベントオブジェクト
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function touchEventHandlerMove(e) {

        event.preventDefault();

        // 一本目の現在座標を取得
        firstTouchX = e.touches[0].pageX;
        firstTouchY = e.touches[0].pageY;

        // タップ時の移動処理
        if (e.touches.length == 1) {

            // 初期タップ座標から、今回のタップ座標を減算し、移動距離を算出
            var moveX = firstTouchXMove - firstTouchX;
            var moveY = firstTouchYMove - firstTouchY;

            //1ピクセル毎に移動させてしまうとパフォーマンスが劣化する(移動処理が重くなる)ので5ピクセル毎の移動とする
            if (Math.abs(moveX) > 5 || Math.abs(moveY) > 5) {

                var objDataWind1;
                objDataWind1 = document.getElementById("dataWind1");

                var nowLeft = objDataWind1.style.left;
                nowLeft = nowLeft.replace("px", "");
                objDataWind1.style.left = new Number(nowLeft) - new Number(moveX) + 'px';

                var nowTop = objDataWind1.style.top;
                nowTop = nowTop.replace("px", "");
                objDataWind1.style.top = new Number(nowTop) - new Number(moveY) + 'px';

                // 初回タップ座標に、現在座標を指定
                firstTouchXMove = firstTouchX;
                firstTouchYMove = firstTouchY;
            }
        }


        // ピンチ時には二点間の距離を算出（拡大処理の中心座標に使用）
        if (e.touches.length > 1) {
            secoundTouchX = e.touches[1].pageX;
            secoundTouchY = e.touches[1].pageY;

            if (firstTouchX > secoundTouchX) {
                firstTouchX = (firstTouchX + secoundTouchX) / 2;
            } else {
                firstTouchX = (secoundTouchX + firstTouchX) / 2;
            }

            if (firstTouchY > secoundTouchY) {
                firstTouchY = (firstTouchY + secoundTouchY) / 2;
            } else {
                firstTouchY = (secoundTouchY + firstTouchY) / 2;
            }

        }

    }


    /**
    * タッチ終了イベント
    * 
    * @param {e} イベントオブジェクト
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function touchEventHandlerEnd(e) {

        event.preventDefault();

        // タッチ本数が一本になった場合には、移動用の現在座標に、タッチされている座標をセット
        if (e.touches.length == 1) {
            // 初回タップ座標に、現在座標を指定
            firstTouchXMove = e.touches[0].pageX;
            firstTouchYMove = e.touches[0].pageY;
        }

        //SALES_1B】UT課題No35対応・車両画像が原寸より縮小するように修正 START
        //拡大画像閉じる処理
        var objPopImg = document.getElementById("popImg");
        var nowWidth = objPopImg.width;
        // 初期表示の大きさを下回った場合は、閉じる
        if (nowWidth < bigImagesize) {
            closeBigImage();
        }
        //SALES_1B】UT課題No35対応・車両画像が原寸より縮小するように修正 END

    }

    /**
    * 車両画像拡大処理
    * 
    * @param {scaleSize} ピンチスケール
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function zoomImg(scaleSize) {

        var objPopImg = document.getElementById("popImg");

        // 変更前の画像サイズを保持
        var nowWidth = objPopImg.width;
        var nowHeight = objPopImg.height;

        // ピンチ前の画像サイズにピンチスケールを乗算
        var newWidth = nowWidth * scaleSize;
        var newHeight = nowHeight * scaleSize;


        //拡大される高さ、幅を取得
        var subHeight = newHeight - nowHeight;
        var subWidth = newWidth - nowWidth;

        //そのままの拡大幅ではタッチポイント以上に拡大してしまうため、拡大幅を調整する。
        newHeight = nowHeight + (subHeight / 5);
        newWidth = nowWidth + (subWidth / 5);

        //拡大の上限は初期表示サイズの３倍とする
        if (new Number(newWidth) > new Number(bigImagesize) * 3) {
            return;
        }

        //SALES_1B】UT課題No35対応・車両画像が原寸より縮小するように修正 START
        //縮小の下限は初期表示サイズの１／３倍とする
        if (new Number(newWidth) < new Number(bigImagesize) / 3) {
            return;
        }
        //SALES_1B】UT課題No35対応・車両画像が原寸より縮小するように修正 END


        //拡大される高さ、幅を取得
        subHeight = newHeight - nowHeight;
        subWidth = newWidth - nowWidth;

        // タップ座標を中心とした拡大後、座標を算出
        var y = (firstTouchY - $(dataWind1).offset().top) / nowHeight * subHeight;
        var x = (firstTouchX - $(dataWind1).offset().left) / nowWidth * subWidth;

        // 拡大縮小実施
        objPopImg.width = newWidth;

        var objDataWind1 = document.getElementById("dataWind1");

        objDataWind1.style.top = new Number(objDataWind1.style.top.replace("px", "")) - new Number(y) + 'px';
        objDataWind1.style.left = new Number(objDataWind1.style.left.replace("px", "")) - new Number(x) + 'px';
    }
    //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 END

    function pinchInEventHandler(e) {

        event.preventDefault();
        //ゼスチャー開始時の画像サイズを保存
        orgSize = document.getElementById("popImg").width;

    }

    function pinchOutEventHandler(e) {

        event.preventDefault();

        //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 START
        zoomImg(e.scale);
        //2012/03/09 KN 清水 【SALES_1B】ピンチ箇所を中心に車両画像を拡大 END

    }

});


//メイン画面読み込み完了時、読み込みアイコン消去
$(window).load(function () {
        hideLoadingIcon();
});


    /**
    * 新規活動破棄チェック
    */
    function cancellationCheck() {

        if (newActivityFlg === true) {
            //新規活動中
            return confirm(redirectMessage);
        }
        return true;
    };


function showLoadingIcon() {
    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション
    $("#serverProcessOverlayBlack").addClass("open");
    $("#serverProcessIcon").addClass("show");

}

function hideLoadingIcon() {
    //アニメーション
    $("#serverProcessIcon").removeClass("show");
    $("#serverProcessOverlayBlack").removeClass("open");
    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "none");

}


/**
* 装備展開表示処理
* 
* @param {-} -
* @return {-} -
* 
* @example 
*  -
*/
function divSlideDown() {

    if (document.getElementById("DivSlideImage1") != null) {
        document.getElementById("DivSlideImage1").style.display = 'none';
    }

    $("#newsBoxInfo").slideDown();


    if (document.getElementById("divSlideIMG2") != null) {
        document.getElementById("divSlideIMG2").style.display = 'block';
    }

    // 下矢印ボタンが画面に表示されていれば押下イベントを削除
    if(document.getElementById("downImg") != null) {

        document.getElementById("downImg").removeEventListener("touchend", divSlideDown, true);
    }

    // 画面全体に装備展開領域を閉じるイベントを追加
    document.getElementById("SC3060101Display").addEventListener("touchend", divSlideUp, true);


    slideFlg = "1";
    return false;

}

/**
* 装備格納表示処理
* 
* @param {-} -
* @return {-} -
* 
* @example 
*  -
*/
function divSlideUp() {

    // if (document.getElementById("DivSlideImage1").style.display != "block") {

    if (document.getElementById("DivSlideImage1") != null) {
        document.getElementById("DivSlideImage1").style.display = "block";
    }

    if (document.getElementById("divSlideIMG2") != null) {
        document.getElementById("divSlideIMG2").style.display = 'none';
    } 


        $("#newsBoxInfo").slideUp();
    //}

        slideFlg = "0"
    // 下矢印ボタンが画面に表示されていれば押下イベントを追加
        if (document.getElementById("downImg") != null) {

            document.getElementById("downImg").addEventListener("touchend", divSlideDown, true);
        }
        // 画面全体から装備展開領域を閉じるイベントを削除
    document.getElementById("SC3060101Display").removeEventListener("touchend", divSlideUp, true);




    return false;

}

/**
* 車両拡大画像表示処理
* 
* @param {obj} 選択されたサムネイル画像オブジェクト
* @param {fileUrl} 拡大画像Url
* @return {-} -
* 
* @example 
*  -
*/
function selectBigImage(obj, fileUrl, divName, imageNo) {


    //既に拡大表示されているサムネイル画像を再度押下された場合は何もしない。
    if (bigImageNo == imageNo) {
        closeBigImage();
        return false;
    }

    //既に拡大表示されているサムネイル画像を閉じる。
    closeBigImage();

    //サムネイル画像は、グレー領域の前面に表示する。
    document.getElementById("SumDiv").style.zIndex = 110;

    //拡大画像読み込みアイコンを表示
    showLoadingIcon();

    //押下されたサムネイル画像を拡大画像に設定
    bigImageNo = imageNo;

    document.getElementById("tcvNsc31Main").style.display = "block";
    document.getElementById("tcvNsc31Black").style.display = "block";

    // ここから、拡大画像に対しての処理
    $("#popWind").animate({ top: '130px' }, "slow");
    document.getElementById("popImg").src = fileUrl;

    document.getElementById("popImg").width = bigImagesize;

    // divの初期表示位置を設定
    dataWind1.style.top = bigImageTop;
    dataWind1.style.left = bigImageLeft;

    // サムネイル画像に対しての拡大処理
    var oheight = $(obj).height();
    var owidth = $(obj).width();
    var nheight = (oheight + (oheight * sumLarge));
    var nwidth = (owidth + (owidth * sumLarge));
    var top = ((oheight - nheight) / 2);
    var left = ((owidth - nwidth) / 2);

/*
    $(obj).stop().animate({
            'height': nheight + 'px',
            'width': nwidth + 'px',
            'left': left + 'px',
            'top': top + 'px'
        }, "slow");
        */


    obj.style.width = nwidth + 'px';
    obj.style.height = nheight + 'px';

    // サムネイル画像のDivに対しての処理(サムネイル拡大に伴い位置を調整する。)
    oheight = $("#" + divName).height();
    owidth = $("#" + divName).width();
    nheight = (oheight + (oheight * sumLarge));
    nwidth = (owidth + (owidth * sumLarge));

    //拡大後の左端補正
    nowLeft = imageNo * 73 + 2;

    top = ((oheight - nheight) / 2);
    left = nowLeft + ((owidth - nwidth) / 2);
/*
    $("#" + divName).stop().animate({
            'height': nheight + 'px',
            'width': nwidth + 'px',
            'left': left + 'px',
            'top': top + 'px'
        }, "slow");
        */
        document.getElementById(divName).style.width = nwidth + 'px';
        document.getElementById(divName).style.height = nheight + 'px';
        document.getElementById(divName).style.left = left + 'px';
        document.getElementById(divName).style.top = top + 'px';


    // 選択サムネイル画像の浮き上がり表示
    document.getElementById(divName).style.zIndex = document.getElementById(divName).style.zIndex + 10;

    //サムネイル画像全体を画面中央に配置されるように調整
    var nowSumLeft = document.getElementById("SumDiv").style.left;
    nowSumLeft = nowSumLeft.replace("px", "");

    // TODO document.getElementById("SumDiv").style.left = new Number(nowSumLeft) - sumMargin + 'px';

    return false;

}
/**
* 車両サムネイル画像初期化処理
* * 
* @example 
*  -
*/

function resetSumDiv() {

    var divName;
    var imgName;

    //拡大画像が選択されていなければ何もしない。
    if (bigImageNo == -1) {
        return false;
    }

    //拡大画像のimgタグ名を取得
    imgName = "sumImg" + bigImageNo;

    // サムネイル画像に対しての縮小処理
    var top = 0;
    var left = 0;
    var nheight = 53;
    var nwidth = 71;
    /*
    $("#" + imgName).stop().animate({
        'height': nheight + 'px',
        'width': nwidth + 'px',
        'left': left + 'px',
        'top': top + 'px'
    }, "slow");
    */
    document.getElementById(imgName).style.width = nwidth + 'px';
    document.getElementById(imgName).style.height = nheight + 'px';

    //拡大画像のdivタグ名を取得
    divName = "sumDiv" + bigImageNo;

    // サムネイル画像のDivに対しての処理
    left = bigImageNo * 73 + 2;

    /*
    $("#" + divName).stop().animate({
        'height': nheight + 'px',
        'width': nwidth + 'px',
        'left': left + 'px',
        'top': top + 'px'
    }, "slow");
    */

    //縮小後の座標を計算
    document.getElementById(divName).style.width = nwidth + 'px';
    document.getElementById(divName).style.height = nheight + 'px';

    document.getElementById(divName).style.left = left + 'px';
    document.getElementById(divName).style.top = top + 'px';

    // 選択サムネイル画像の浮き上がり表示解除
    document.getElementById(divName).style.zIndex = "";
    //拡大画像の記憶値を初期化
    bigImageNo = -1;

    //画面中央に配置されていたサムネイル画像全体を右に調整
    var nowSumLeft = document.getElementById("SumDiv").style.left;
    nowSumLeft = nowSumLeft.replace("px", "");

    //TODO document.getElementById("SumDiv").style.left = new Number(nowSumLeft) + sumMargin + 'px';

}

/**
* 車両拡大画像閉じる処理
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function closeBigImage() {

        //画像を閉じた場合は、ピンチによるスケールを初期化する。
        document.getElementById("popImg").width = bigImagesize;
        document.getElementById("popImg").src = "";

        //既に拡大表示されているサムネイル画像を閉じる。
        resetSumDiv();

        //
        document.getElementById("tcvNsc31Main").style.display = "none";
        document.getElementById("tcvNsc31Black").style.display = "none";
        //サムネイル画像は、背面にセット。（査定ポップアップの前面に来ないようにするため。）
        document.getElementById("SumDiv").style.zIndex = 0;


        return false;
 
}

/**
* 車両拡大画像div移動
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function moveDiv(addTop, addLeft) {

    var nowTop;

    var objDataWind1;
    objDataWind1 = document.getElementById("dataWind1");

    if (objDataWind1.style.top != "") {
        nowTop = objDataWind1.style.top;
    } else {
        nowTop = "0";
    }

    nowTop = nowTop.replace("px", "");

    objDataWind1.style.top = new Number(nowTop) + new Number(addTop) + 'px';

    var nowLeft;

    if (objDataWind1.style.left != "") {
        nowLeft = objDataWind1.style.left;
    } else {
        nowLeft = "0";
    }

    nowLeft = nowLeft.replace("px", "");

    objDataWind1.style.left = new Number(nowLeft) + new Number(addLeft) + 'px';


}
