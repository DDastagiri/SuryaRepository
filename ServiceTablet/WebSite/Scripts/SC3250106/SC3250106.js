//------------------------------------------------------------------------------
//SC3250106.js
//------------------------------------------------------------------------------
//機能：部品説明_javascript
//
//作成：2014/08/XX NEC 上野	初版作成
//更新：
//------------------------------------------------------------------------------
$(function () {
    //横スクロール機能設定
    $('#ScrollArea1').SC3250106Flickable();
    $("#ScrollArea2").SC3250106Flickable();
    $("#ScrollArea3").SC3250106Flickable();
    $("#ScrollArea4").SC3250106Flickable();
    $("#LargeScrollArea").SC3250106Flickable();
    //スクロールエリアの上に配置するスクロールしない余白部分を作成する
    createCoverArea();
});

//各チャートをタップしたときの処理
function ClickChart(ChartNo) {

    //クルクル表示
    SetLoadingStart();

    //hiddenタグにタップしたチャートの番号を入れる
    document.getElementById("hdnSelectChart").value = ChartNo;

    //ポストバック
    this_form.submit();

}

//閉じるボタンをタップしたときの処理
function ClosePopUp() {
    //半透明ボードを閉じる
    document.getElementById("contentsMainonBoard").style.display = "none";
    //閉じるボタンを消す
    document.getElementById("closeBtn").style.display = "none";
    //拡大画面を消す
    document.getElementById("popUpWindow").style.display = "none";
}

//クルクル画面を表示する
function SetLoadingStart() {
    $("#ServerProcessOverlayBlack").css("display", "block");
    $("#ServerProcessIcon").css("display", "block");
}

//スクロールエリアの上に配置するスクロールしない余白部分の作成
function createCoverArea() {

    //コピー元のIMGタグを取得
    var element = document.getElementById("MyLargeChart");
    //スクロールエリア（下になるDIVタグ）の座標を取得
    var ele = document.getElementById("LargeScrollArea");
    var bounds = $("#LargeScrollArea").position(); //下になる要素の位置を相対位置に変更

    //下になる要素のx座標を取得
    var x = bounds.left;
    //下になる要素のy座標を取得
    var y = bounds.top;
    //下になる要素の幅を取得
    var x2 = ele.clientWidth;
    // 下になる要素の高さを取得
    var y2 = element.clientHeight; 	//画像の高さがスクロールエリアの高さより小さいため、画像の高さに変更

    //取得したDIVタグを複製
    var element2 = element.cloneNode(true);     //左側用（Y軸数値側）
    var element3 = element.cloneNode(true);     //右側用（X軸余白側）

    //左側（Y軸の数値）の配置
    var CoverLeftWidth = 51;                                                //隠したい幅（px）

    var objLeft = document.getElementById("CoverLeft");
    objLeft.appendChild(element2);                                          //コピーした要素を配置
    objLeft.style.width = CoverLeftWidth + "px";                            //隠したい幅
    objLeft.style.height = y2 + "px";                                       //隠したい高さ
    objLeft.style.top = y + "px";                                           //位置（Y）
    objLeft.style.left = x + "px";                                          //位置（X)


    //右側（余白）の配置
    var CoverRightWidth = 16;                                               //隠したい幅（px）

    var objRight = document.getElementById("CoverRight");
    var objRight2 = document.getElementById("CoverRight2");
    objRight2.appendChild(element3);                                        //コピーした要素を配置
    objRight.style.width = CoverRightWidth + "px";                          //隠したい幅
    objRight.style.height = y2 + "px";                                      //隠したい高さ
    objRight.style.top = y + "px";                                          //位置（Y）
    objRight.style.left = x + x2 - CoverRightWidth + "px";                  //位置（X)
    objRight2.style.left = -element.clientWidth + CoverRightWidth + "px";   //表示する画像の位置（右端を計算）

}
