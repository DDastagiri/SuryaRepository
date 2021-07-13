/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3050703.js
─────────────────────────────────────
機能： セールスポイント詳細設定
補足： 
作成： 2012/11/23 TMEJ 三和
更新： 
─────────────────────────────────────*/

/**
* 外装定数
* @return {String}
*/
var C_EXTERIOR = "exterior";

/**
* 内装定数
* @return {Integer}
*/
var C_INTERIOR = "interior";

/**
* 画面変更区分:変更なし
* @return {String}
*/
var MODIFY_OFF = "0";

/**
* 画面変更区分:変更あり
* @return {String}
*/
var MODIFY_ON = "1";

/**
* グレード適合:チェック
* @return {String}
*/
var GRADE_ON = "1";

/**
* 画像拡張子[png]
* @return {String}
*/
var IMAGE_PNG = "png";

/**
* 画像拡張子[PNG]
* @return {String}
*/
var IMAGE_PNG_BIG = "PNG";

/**
* 画像拡張子[jpg]
* @return {String}
*/
var IMAGE_JPG = "jpg";

/**
* 画像拡張子[JPG]
* @return {String}
*/
var IMAGE_JPG_BIG = "JPG";

/**
* 画像拡張子[jpeg]
* @return {String}
*/
var IMAGE_JPEG = "jpeg";

/**
* 画像拡張子[JPEG]
* @return {String}
*/
var IMAGE_JPEG_BIG = "JPEG";

/**
* 動画拡張子[mp4]
* @return {String}
*/
var VIDEO_MP4 = "mp4";

/**
* 動画拡張子[MP4]
* @return {String}
*/
var VIDEO_MP4_BIG = "MP4";

/**
* 動画拡張子[mov]
* @return {String}
*/
var VIDEO_MOV = "mov";

/**
* 動画拡張子[MOV]
* @return {String}
*/
var VIDEO_MOV_BIG = "MOV";

/**
* ファイル区分[image]
* @return {String}
*/
var FILE_DVS_IMAGE = "image";

/**
* ファイル区分[video]
* @return {String}
*/
var FILE_DVS_VIDEO = "video";

/**
* 座標単位
* @return {String}
*/
var POINT_PX = "px";

/**
* パネルグレーデフォルトスタイル
* @return {String}
*/
var PANEL_DFAULT_STYLE = "none";

/**
* エラー区分 ON
* @return {String}
*/
var ERROR_DVS_ON = "1";

/**
* エラー区分 OFF
* @return {String}
*/
var ERROR_DVS_OFF = "0";

/**
* 外装スクロール高さ
* @return {String}
*/
var EXTERIOR_HEIGHT = "901px";

/**
* 内装スクロール高さ
* @return {String}
*/
var INTERIOR_HEIGHT = "905px";

/**
* 内装画像縮尺率
* @return {String}
*/
var INTERIOR_REVISION = 0.91;

/**
* 外装リング補正値X
* @return {String}
*/
var EXTERIOR_REVISION_X = 10;

/**
* 外装リング補正値Y
* @return {String}
*/
var EXTERIOR_REVISION_Y = 10;

/**
* 非同期ポストバックコントロール格納
* @return {String}
*/
var gAjaxControlId = "";

/**
* セールスポイント情報格納
* @return {String}
*/
var gSalesPointInfo = null;

/**
* 初期表示設定
*/
$(function () {

    //リフレッシュ判定
    if ($("#refleshDvsField").val() == ERROR_DVS_ON) {
        $("#refleshDvsField").val("")
        $("#RefleshButton").click();

    }

    //ローディング開始
    showLoding();

    jQuery.event.add(window, "load", function () {
        //ローディング終了
        closeLoding();

        //スクロール設定
        setVScroll();

    });

    //ポップアップ画像読み込み終了時イベント設定
    $(tcvNcvPointBigWindowbBoxImage).bind('load', function () {
        var maxWidth = 520;
        var maxHeight = 403;
        var imgWidth = $("#tcvNcvPointBigWindowbBoxImage").width();
        var imgHeight = $("#tcvNcvPointBigWindowbBoxImage").height();

        //幅と高さの両方が基準値を超える場合
        if (maxWidth < imgWidth && maxHeight < imgHeight) {
            //基準値との比率を算出
            var widthRatio = imgWidth / maxWidth;
            var heightRatio = imgHeight / maxHeight;

            //高さの方が比率が大きい場合
            if (widthRatio < heightRatio) {
                //高さを基準にする
                $("#tcvNcvPointBigWindowbBoxImage").height(maxHeight);
            } else {
                //それ以外は幅を基準にする
                $("#tcvNcvPointBigWindowbBoxImage").width(maxWidth);
            }
        } else if (maxWidth < imgWidth) {
            //幅のみが基準値を超える場合は幅を基準にする
            $("#tcvNcvPointBigWindowbBoxImage").width(maxWidth);
        } else if (maxHeight < imgHeight) {
            //高さのみが基準値を超える場合は高さを基準にする
            $("#tcvNcvPointBigWindowbBoxImage").height(maxHeight);
        }

        //ローディング終了
        closeLoding();

        $("#tcvNcvPointBigWindowbBox").css("z-index", "60001");
    });

    //ポップアップ画像読み込み失敗時イベント設定
    $(tcvNcvPointBigWindowbBoxImage).bind('error', function () {
        //ローディング終了
        closeLoding();
    });


    //ポップアップ動画読み込み終了時イベント設定
    $(tcvNcvPointBigWindowbBoxVideo).bind('progress', function () {
        //ローディング終了
        closeLoding();

        $("#tcvNcvPointBigWindowbBox").css("z-index", "60001");
    });

    //ポップアップ動画読み込み失敗時イベント設定
    $(tcvNcvPointBigWindowbBoxVideo).bind('error', function () {
        //ローディング終了
        closeLoding();

        $("#tcvNcvPointBigWindowbBox").css("z-index", "60001");
    });

    //PageRequestManagerクラスをインスタンス化
    var mng = Sys.WebForms.PageRequestManager.getInstance();

    //initializeRequestイベント・ハンドラを定義
    mng.add_initializeRequest(
    //非同期ポストバックの開始前にイベント発生元の要素を格納
        function (sender, args) {
            gAjaxControlId = args.get_postBackElement().id;
        }
    );

    //非同期ポストバックの完了
    mng.add_endRequest(
        function (sender, args) {
            //非同期ポストバックの完了

            //チェック結果が正常なら保存処理を実行
            if ($("#ajaxErrorField").val() == ERROR_DVS_OFF) {
                //詳細(拡大画像)ファイル選択を活性化
                $("#detailPopupFile").attr("disabled", "");

                $("#SendButton").click();

            } else {
                //ローディング終了
                closeLoding();

            }
        }
    );

    //コントロール制御
    initControl(true);

    // セールスポイントポップアップ閉じるボタン
    $("#tcvNcvPointBigWindowBackFrameCloseBtn").click(function (e) {
        e.stopPropagation();
        popupClose();
    });

    // セールスポイントポップアップ背景
    $("#tcvNcvPointBigWindowbBox").click(function (e) {
        e.stopPropagation();

        var pos = {};
        if (e.originalEvent.x) pos = { x: e.originalEvent.pageX, y: e.originalEvent.pageY };
        else pos = { x: e.originalEvent.touches[0].pageX, y: e.originalEvent.touches[0].pageY };

        var offset = $("#tcvNcvPointBigWindowBackFrame").offset();
        var width = $("#tcvNcvPointBigWindowBackFrame").width();
        var height = $("#tcvNcvPointBigWindowBackFrame").height();

        if (pos.x < offset.left || offset.left + width < pos.x || pos.y < offset.top || offset.top + height < pos.y) {
            popupClose();
        }
    });

    //セールスポイント設定処理を定義
    $("#setPointArea").click(function (e) {
        var areaOffset = $('#setPointArea').offset();
        var offsetTop = ((e.pageY) - (areaOffset.top));
        var offsetLeft = ((e.pageX) - (areaOffset.left));

        var areaID = "";
        var layerX = offsetLeft - parseInt($("#imageFrame").css("padding-left"));
        var layerY = offsetTop - parseInt($("#imageFrame").css("padding-top"));
        //X、Y座標をテーブルエリアIDに変換

        areaID = convertLayerToArea(layerX, layerY);
        //指定エリアのスタイルを取得
        var background = $("#" + areaID).css("background-image");

        //パネルグレーの場合、キャンセル
        if (background != PANEL_DFAULT_STYLE) {
            return false;
        }
        //編集状態にする
        onChangeDisplay();

        //ポイント情報のクリア
        initPointControl();

        //ポイントのスタイルクラス設定
        var pointClass = null;
        if ($("#exInField").val() == C_EXTERIOR) {
            //外装
            pointClass = "pointEx";
        } else {
            //内装
            pointClass = "pointIn";
        }

        //ポイントの設定
        document.getElementById(areaID).innerHTML = "<div id = \"point\" class=\"" + pointClass + "\" style=\"display:block;\">" + $("#salesPointNoField").val() + "</div>";
        //吹き出し表示
        dispOverRay(layerX, layerY);

        //アングルを保持
        $("#angleField").val($("#angleBackUpField").val())
        //座標を保持
        $("#topPointField").val($("#topPointBkField").val())
        $("#leftPointField").val($("#leftPointBkField").val())

    });

    //グレードタップ開始時にメインスクロール停止
    $("#gradeWrapArea").bind("touchstart", function () {
        $("#settings").fingerScroll({ action: "stop" });
    });

    //グレードタップ終了時にメインスクロール開始
    $("#gradeWrapArea").bind("touchend", function () {
        $("#settings").fingerScroll({ action: "restart" });
    });

});

/**
* 読み込み中アイコン表示
*/
function showLoding() {
    //オーバーレイ表示
    $("#registOverlayBlack").css("display", "block");
    //アニメーション
    $("#processingServer").addClass("show");
    $("#registOverlayBlack").addClass("open");

}

/**
* 読み込み中アイコン非表示
*/
function closeLoding() {
    $("#processingServer").removeClass("show");
    $("#registOverlayBlack").removeClass("open").one("webkitTransitionEnd", function (e) {
        $("#registOverlayBlack").css("display", "none");
    });

    //オーバーレイ非表示
    $("#serverProcessOverlayBlack").css("display", "none");

}

/**
* スクロール設定
*/
function setVScroll() {

    //サムネイル画像スクロール設定
    $("#angleList").HScroll().css("overflow:scroll", "touch");
    $("#angleList").HScroll().css("overflow-x", "scroll");
    $("#angleList").HScroll().css("overflow-y", "hidden");
    $("#angleList").children(".HScroll-inner").width($("#angleListDetails").width() + POINT_PX)

    //メインスクロール設定
    $("#settings").fingerScroll();

    //メインスクロール高さ設定
    if ($("#exInField").val() == C_EXTERIOR) {
        //外装
        $("#settingInner").height(EXTERIOR_HEIGHT);
    } else {
        //内装
        $("#settingInner").height(INTERIOR_HEIGHT);
    }

    //グレード適合スクロール設定
    $("#gradeWrapArea").VScroll().css("overflow:scroll", "touch");
    $("#gradeWrapArea").VScroll().css("overflow-x", "hidden");
    $("#gradeWrapArea").VScroll().css("overflow-y", "scroll");

}
/**
* 初期コントロール制御
*/
function initControl(initDvs) {

    //Hiddenの値をjavaScriptの変数にセット
    gSalesPointInfo = eval('(' + $("#salesPointJsonField").val() + ')');

    //初期アングルを設定
    selectAngle($("#angleField").val(), $("#defaultGridPathField").val(), true);

    //初回のみ設定
    if (initDvs) {
        //グレード情報を設定
        setGrade();
    }

    //詳細(拡大画像)のスタイルを判定
    setDetailPopupDisabled();

}

/**
* ポイント情報のクリア
*/
function initPointControl() {

    //ポイントの初期化
    if (document.getElementById('point')) {
        var pointObj = document.getElementById('point');
        var pointObjParent = pointObj.parentNode;
        pointObjParent.removeChild(pointObj);
    }
    //吹き出しの初期化
    document.getElementById('SalesPointOverView').style.display = 'none';

}

/**
* 吹き出し表示
*/
function dispOverRay(layerX, layerY) {

    var baseWidth = $('#setPointArea').width();
    var baseHeight = $('#setPointArea').height();

    var rows = $("#frame tr");
    var separateCol = Math.floor(rows[0].cells.length / 2);
    var separateRow = Math.floor(rows.length / 2);
    var border = 2;
    var cellWidth = rows[0].cells[0].offsetWidth;
    var cellHeight = rows[0].cells[0].offsetHeight;
    var centerWidth = (cellWidth * separateCol) + border;
    var centerHeight = (cellHeight * separateRow) + border;

    var layerStyle = document.getElementById('SalesPointOverView').style;
    var layerWidth = $('#SalesPointOverView').width();
    var layerHeight = $('#SalesPointOverView').height();
    var layerMargin = 5;

    //吹き出し位置を設定
    var tcvLeft = 0;
    var tcvTop = 0;
    var left = 0;
    var top = 0;
    if (layerX < centerWidth && layerY < centerHeight) {
        //ポイント位置:左上┏
        //レイヤー位置:右下┛
        tcvLeft = 550;
        tcvTop = 230;
        left = (baseWidth - layerWidth - (layerMargin * 2))
        top = (baseHeight - layerHeight - (layerMargin * 2))
        layerStyle.display = 'block';
    } else if (layerX >= centerWidth && layerY < centerHeight) {
        //ポイント位置:右上┓
        //レイヤー位置:左下┗
        if ($("#exInField").val() == C_EXTERIOR) {
            tcvLeft = 290;
        } else {
            tcvLeft = 30;
        }
        tcvTop = 230;
        left = layerMargin
        top = (baseHeight - layerHeight - (layerMargin * 2))
        layerStyle.display = 'block';
    } else if (layerX < centerWidth && layerY >= centerHeight) {
        //ポイント位置:左下┗
        //レイヤー位置:右上┓
        tcvLeft = 550;
        tcvTop = 30;
        left = (baseWidth - layerWidth - (layerMargin * 2))
        top = layerMargin
        layerStyle.display = 'block';
    } else if (layerX >= centerWidth && layerY >= centerHeight) {
        //ポイント位置:右下┛
        //レイヤー位置:左上┏
        if ($("#exInField").val() == C_EXTERIOR) {
            tcvLeft = 290;
        } else {
            tcvLeft = 30;
        }
        tcvTop = 30;
        left = layerMargin
        top = layerMargin
        layerStyle.display = 'block';
    } else {
        layerStyle.display = 'none';
    }
    layerStyle.left = left + "px"
    layerStyle.top = top + "px"
    if (left != 0) {
        left = Math.ceil(left / INTERIOR_REVISION);
    }
    if (top != 0) {
        top = Math.ceil(top / INTERIOR_REVISION);
    }
    $("#leftOverPointField").val(tcvLeft + "px");
    $("#topOverPointField").val(tcvTop + "px");
}

/**
* アングル設定
*/
function selectAngle(angle, gridImagePath, initDvs) {
    //選択されたアングルを保持
    $("#angleBackUpField").val(angle)

    //ポイント情報のクリア
    initPointControl();

    //パネルグレー初期化
    initPanelGray();

    //グリッド画像の変更
    var url = 'url(' + gridImagePath + ')';
    var frameObj = document.getElementById("imageFrame");
    frameObj.border = 1;
    frameObj.style.backgroundImage = url;
    frameObj.style.backgroundRepeat = 'no-repeat';
    //車両画像を表示
    document.getElementById('settings').style.display = 'block';

    var i = 0;
    var areaID = null;
    var topPoint = 0;
    var leftPoint = 0;
    var topCurrentPoint = 0;
    var leftCurrentPoint = 0;

    //セールスポイント情報からポイント情報を設定
    for (i = 0; i < gSalesPointInfo.sales_point.length; i++) {
        //外装/内装判定
        if ($("#exInField").val() == C_EXTERIOR) {
            //外装の場合
            //現在選択されているアングルのみ対象とする
            if (angle == gSalesPointInfo.sales_point[i].angle[0]) {

                //セールスポイントIDの判定
                if ($("#targetID").val() != gSalesPointInfo.sales_point[i].id) {
                    //その他のID
                    leftPoint = gSalesPointInfo.sales_point[i].left[0].replace("px", "");
                    topPoint = gSalesPointInfo.sales_point[i].top[0].replace("px", "");
                    //X、Y座標をテーブルエリアIDに変換
                    areaID = convertLayerToArea(Number(leftPoint) + Number(EXTERIOR_REVISION_X), Number(topPoint) + Number(EXTERIOR_REVISION_Y));
                    //グレーパネル表示
                    $("#" + areaID).css({ "background": "-webkit-gradient(linear, left top, left bottom, from(#aeb2b8), to(#6d7580))" });
                }

            }

        } else {
            //内装の場合
            //現在選択されている内装IDのみ対象とする
            if (angle == gSalesPointInfo.sales_point[i].interiorid[0]) {

                //セールスポイントIDの判定
                if ($("#targetID").val() != gSalesPointInfo.sales_point[i].id) {
                    //その他のID
                    leftPoint = gSalesPointInfo.sales_point[i].left[0].replace("px", "");
                    topPoint = gSalesPointInfo.sales_point[i].top[0].replace("px", "");
                    //X、Y座標をテーブルエリアIDに変換
                    areaID = convertLayerToArea(Math.ceil(leftPoint * INTERIOR_REVISION), Math.ceil(topPoint * INTERIOR_REVISION));
                    //グレーパネル表示
                    $("#" + areaID).css({ "background": "-webkit-gradient(linear, left top, left bottom, from(#aeb2b8), to(#6d7580))" });
                }
            }
        }
    }

    //セールスポイントIDの判定
    if (angle == $("#angleField").val()) {
        //ポイント設定判定
        if ($("#leftPointField").val() != '' && $("#topPointField").val() != '') {
            //ポイントのスタイルクラス設定
            var pointClass = null;

            //X、Y座標をテーブルエリアIDに変換
            //外装/内装判定
            if ($("#exInField").val() == C_EXTERIOR) {
                leftCurrentPoint = $("#leftPointField").val().replace("px", "");
                topCurrentPoint = $("#topPointField").val().replace("px", "");

                leftCurrentPoint = Number(leftCurrentPoint) + Number(EXTERIOR_REVISION_X);
                topCurrentPoint = Number(topCurrentPoint) + Number(EXTERIOR_REVISION_Y); 

                pointClass = "pointEx";
            } else {
                leftCurrentPoint = $("#leftPointField").val().replace("px", "");
                topCurrentPoint = $("#topPointField").val().replace("px", "");
                leftCurrentPoint = Math.ceil(leftCurrentPoint * INTERIOR_REVISION);
                topCurrentPoint = Math.ceil(topCurrentPoint * INTERIOR_REVISION);
                pointClass = "pointIn";
            }
            areaID = convertLayerToArea(leftCurrentPoint, topCurrentPoint);

            //ポイントの設定
            document.getElementById(areaID).innerHTML = "<div id = \"point\" class=\"" + pointClass + "\" style=\"display:block;\">" + $("#salesPointNoField").val() + "</div>";
            
            //吹き出し表示
            dispOverRay(leftCurrentPoint, topCurrentPoint);
        }
    }

}

/**
* 座標からテーブル要素に変換
*/
function convertLayerToArea(layerX, layerY) {

    if (layerX < 0) {
        layerX = 0;
    }
    if (layerY < 0) {
        layerY = 0;
    }


    var isExterior = false;
    if ($("#exInField").val() == C_EXTERIOR) {
        isExterior = true;
    }

    var colID = null;
    var rows = $("#frame tr");
    var colCount = rows[0].cells.length;
    var border = 2;
    var cellWidth = rows[0].cells[0].offsetWidth + border;

    //X座標から列IDを検索
    for (var i = 0; i < colCount; i++) {
        //列の左端と右端の座標を設定
        var left = 0;
        if (0 < i) {
            left = (cellWidth * i) - (border * (i - 1));
        }
        var right = cellWidth * (i + 1) - (border * i);
        //特定の列範囲内または見つからなかった場合
        if (left <= layerX && right > layerX || (i == colCount - 1 && colID == null)) {
            //列IDを設定
            colID = ("0" + (i + 1)).slice(-2);

            //内装の場合は画像縮小分の座標を補正
            if (!isExterior && left != 0) {
                left = Math.ceil(left / INTERIOR_REVISION);
            }
            $("#leftPointBkField").val(left + POINT_PX);
            break;
        }
    }

    var rowID = null;
    var rowCount = rows.length;
    var cellHeight = rows[0].cells[0].offsetHeight + border;

    //Y座標から行IDを検索
    for (var i = 0; i < rowCount; i++) {
        //行の上端と下端の座標を設定
        var top = 0;
        if (0 < i) {
            top = (cellHeight * i) - (border * (i - 1));
        }
        var bottom = cellHeight * (i + 1) - (border * i);
        //特定の行範囲内または見つからなかった場合
        if (top <= layerY && bottom > layerY || (i == rowCount - 1 && rowID == null)) {
            //行IDを設定
            rowID = ("0" + (i + 1)).slice(-2);

            //内装の場合は画像縮小分の座標を補正
            if (!isExterior && top != 0) {
                top = Math.ceil(top / INTERIOR_REVISION);
            }
            $("#topPointBkField").val(top + POINT_PX);
            break;
        }
    }
    var areaID = "td" + colID + rowID;


    return areaID;

}

/**
* パネルグレー初期化
*/
function initPanelGray() {

    var i = 0;
    var j = 0;
    var pointX = null;
    var pointY = null;

    var rows = $("#frame tr");
    var rowCount = rows.length;
    var colCount = rows[0].cells.length;

    //全ての枠情報をクリア
    for (i = 0; i < rowCount; i++) {
        pointX = ("0" + (i + 1)).slice(-2)
        for (j = 0; j < colCount; j++) {
            pointY = ("0" + (j + 1)).slice(-2)
            $("#td" + pointY + pointX).css({ "background": "" });
        }
    }
}

/**
* グレード設定
*/
function setGrade() {
    var i = 0;
    var j = 0;

    //セールスポイント情報からポイント情報を設定
    for (i = 0; i < gSalesPointInfo.sales_point.length; i++) {

        //セールスポイントIDの判定
        if ($("#targetID").val() == gSalesPointInfo.sales_point[i].id) {

            if (gSalesPointInfo.sales_point[i].grd.length == 1) {
                //グレード要素を反映
                if (gSalesPointInfo.sales_point[i].grd[j] == GRADE_ON) {
                    grade.checked = true;
                } else {
                    grade.checked = false;
                }
            } else {
                //グレード要素を反映
                for (j = 0; j < gSalesPointInfo.sales_point[i].grd.length; j++) {
                    if (gSalesPointInfo.sales_point[i].grd[j] == GRADE_ON) {
                        grade[j].checked = true;
                    } else {
                        grade[j].checked = false;
                    }
                }
            }
        }
    }
}

/**
* 画像ポップアップ表示
*/
function onClickImage(areaDvs) {

    var popWindow = this;

    if (areaDvs == 0) {
        if ($("#overViewFile").text() == "") {
            return false;
        }
    } else if (areaDvs == 1) {
        if ($("#popUpFile").text() == "") {
            return false;
        }
    } else if (areaDvs == 2) {
        if ($("#fullPopUpFile").text() == "") {
            return false;
        }
    }

    //ローディング開始
    showLoding();

    // タイトル
    $("#tcvNcvPointBigWindowbBoxTitle").text($("#salesPointTxt").val());

    // コンテンツ
    $("#tcvNcvPointBigWindowbBoxImage").css({"width": "", "height": ""});
    
    $("#tcvNcvPointBigWindowbBoxImage").parent().css({ "-webkit-box-align": "center", "-webkit-box-pack": "center" });
    $("#tcvNcvPointBigWindowbBoxVideo").parent().css({ "-webkit-box-align": "center", "-webkit-box-pack": "center" });
    $("#tcvNcvPointBigWindowbBoxImage").parent().css("display", "none");
    $("#tcvNcvPointBigWindowbBoxVideo").parent().css("display", "none");
    $("#tcvNcvPointBigWindowbBoxImage").css("display", "none");
    $("#tcvNcvPointBigWindowbBoxVideo").css("display", "none");

    var fileUrl = null;
    if (areaDvs == 0) {
        fileUrl = $("#overViewFilePathField").val();
    } else if (areaDvs == 1) {
        fileUrl = $("#popUpFilePathField").val();
    } else if (areaDvs == 2) {
        fileUrl = $("#fullPopUpFilePathField").val();
    }

    var fileDvs = null;
    var extension = getExtention(fileUrl);
    if (extension == IMAGE_PNG || extension == IMAGE_JPG || extension == IMAGE_JPEG
    || extension == IMAGE_PNG_BIG || extension == IMAGE_JPG_BIG || extension == IMAGE_JPEG_BIG) {
        fileDvs = FILE_DVS_IMAGE;
    } else if (extension == VIDEO_MP4 || extension == VIDEO_MOV
    || extension == VIDEO_MP4_BIG || extension == VIDEO_MOV_BIG) {
        fileDvs = FILE_DVS_VIDEO;
    }

    // 画像のみ
    $("#tcvNcvPointBigWindowBackFrame").css({ "width": "574px", "left": "225px" });

    if (fileDvs == FILE_DVS_IMAGE) {
        $("#tcvNcvPointBigWindowbBoxImage").attr("src", fileUrl);
        $("#tcvNcvPointBigWindowbBoxImage").parent().css("display", "block");
        $("#tcvNcvPointBigWindowbBoxImage").css("display", "block");
        $("#tcvNcvPointBigWindowbBoxImage").parent().css({ "display": "-webkit-box", "width": "534px" });
    }

    if (fileDvs == FILE_DVS_VIDEO) {
        $("#tcvNcvPointBigWindowbBoxVideo").attr("src", fileUrl);
        $("#tcvNcvPointBigWindowbBoxVideo").parent().css("display", "block");
        $("#tcvNcvPointBigWindowbBoxVideo").css({ "display": "-webkit-box", "width": "534px" });
    }
    $("#tcvNcvPointBigWindowbBox").css("display", "block");
    $("#tcvNcvPointBigWindowbBox").css("z-index", "-100");
}

function popupClose() {
    $("#tcvNcvPointBigWindowbBox").css("display", "none");
    $("#tcvNcvPointBigWindowbBoxImage").attr("src", "");
    $("#tcvNcvPointBigWindowbBoxVideo").attr("src", "");

};

/**
* ファイル名から拡張子を取得
*/
function getExtention(fileName) {
    var ret;
    if (!fileName) {
        return ret;
    }
    var fileTypes = fileName.split(".");
    var len = fileTypes.length;
    if (len === 0) {
        return ret;
    }
    ret = fileTypes[len - 1];
    return ret;
}

/**
* 画面変更フラグON
*/
function onChangeDisplay() {
    //画面変更フラグON
    $("#modifyDvsField").val(MODIFY_ON);

}

/**
* 画面変更チェック
*/
function onChangeDisplayCheck() {
    if ($("#modifyDvsField").val() == MODIFY_ON) {
        //画面変更確認
        if (!confirm($("#modifyMessageField").val())) {
            //キャンセル
            return false;
        }
    }

    //ファイル削除
    delFiles();

    //ローディング開始
    showLoding();
    
    return true;

}

/**
* 保存ボタン押下イベント
*/
function sendSalesPointInfo() {
    var overViewImageMaxFileSize = parseFloat($("#overViewImageMaxFileSizeField").val());
    var popUpImageMaxFileSize = parseFloat($("#popUpImageMaxFileSizeField").val());
    var fullPopUpImageMaxFileSize = parseFloat($("#fullPopUpImageMaxFileSizeField").val());
    var movieMaxSize = parseFloat($("#movieMaxFileSizeField").val());

    //セールスポイント表示位置必須チェック
    if ($("#topPointField").val() == '' && $("#leftPointField").val() == '') {
        //メッセージ表示
        alert($("#pointMessageField").val());

        return false;
    }

    //セールスポイント名必須チェック
    if ($.trim($("#salesPointTxt").val()) == '') {
        //メッセージ表示
        alert($("#salesPointMessageField").val());

        return false;
    }

    var summaryFileList = document.getElementById("summaryFile").files;
    for (var i = 0; i < summaryFileList.length; i++) {
        //概要アップロード拡張子チェック
        if (getExtention(summaryFileList[i].name) != IMAGE_PNG &&
            getExtention(summaryFileList[i].name) != IMAGE_JPG &&
            getExtention(summaryFileList[i].name) != IMAGE_JPEG &&
            getExtention(summaryFileList[i].name) != IMAGE_PNG_BIG &&
            getExtention(summaryFileList[i].name) != IMAGE_JPG_BIG &&
            getExtention(summaryFileList[i].name) != IMAGE_JPEG_BIG) {
            //メッセージ表示
            alert($("#summaryMessageField").val());

            return false;
        }

        //概要アップロードサイズチェック(実サイズをKBに変換)
        if (overViewImageMaxFileSize < (summaryFileList[i].size / 1024)) {
            alert($("#summaryFileSizeMessageField").val());
            return false;
        }

    }

    var detailFileList = document.getElementById("detailFile").files;
    for (var i = 0; i < detailFileList.length; i++) {
        //詳細アップロード拡張子チェック
        if (getExtention(detailFileList[i].name) != IMAGE_PNG &&
            getExtention(detailFileList[i].name) != IMAGE_JPG &&
            getExtention(detailFileList[i].name) != IMAGE_JPEG &&
            getExtention(detailFileList[i].name) != IMAGE_PNG_BIG &&
            getExtention(detailFileList[i].name) != IMAGE_JPG_BIG &&
            getExtention(detailFileList[i].name) != IMAGE_JPEG_BIG &&
            getExtention(detailFileList[i].name) != VIDEO_MP4 &&
            getExtention(detailFileList[i].name) != VIDEO_MP4_BIG &&
            getExtention(detailFileList[i].name) != VIDEO_MOV &&
            getExtention(detailFileList[i].name) != VIDEO_MOV_BIG) {

            //メッセージ表示
            alert($("#detailMessageField").val());

            return false;
        }

        if (getExtention(detailFileList[i].name) == IMAGE_PNG ||
            getExtention(detailFileList[i].name) == IMAGE_JPG ||
            getExtention(detailFileList[i].name) == IMAGE_JPEG ||
            getExtention(detailFileList[i].name) == IMAGE_PNG_BIG ||
            getExtention(detailFileList[i].name) == IMAGE_JPG_BIG ||
            getExtention(detailFileList[i].name) == IMAGE_JPEG_BIG) {

            //詳細アップロードサイズチェック(実サイズをKBに変換)(画像)
            if (popUpImageMaxFileSize < (detailFileList[i].size / 1024)) {
                alert($("#detailFileSizeImageMessageField").val());
                return false;
            }

        } else {
            //詳細アップロードサイズチェック(実サイズをKBに変換)(動画)
            if (movieMaxSize < (detailFileList[i].size / 1024)) {
                alert($("#detailFileSizeMovieMessageField").val());
                return false;
            }

        }

    }

    //詳細(拡大画像)アップロード拡張子チェック
    var detailPopupFileList = document.getElementById("detailPopupFile").files;
    for (var i = 0; i < detailPopupFileList.length; i++) {
        if (getExtention(detailPopupFileList[i].name) != IMAGE_PNG &&
            getExtention(detailPopupFileList[i].name) != IMAGE_JPG &&
            getExtention(detailPopupFileList[i].name) != IMAGE_JPEG &&
            getExtention(detailPopupFileList[i].name) != IMAGE_PNG_BIG &&
            getExtention(detailPopupFileList[i].name) != IMAGE_JPG_BIG &&
            getExtention(detailPopupFileList[i].name) != IMAGE_JPEG_BIG) {
            //メッセージ表示
            alert($("#detailPopupMessageField").val());

            return false;
        }

        //詳細(拡大画像)アップロードサイズチェック(実サイズをKBに変換)
        if (fullPopUpImageMaxFileSize < (detailPopupFileList[i].size / 1024)) {
            alert($("#detailPopupFileSizeMessageField").val());
            return false;
        }

    }

    //グレード必須チェック
    var objControl = document.all;
    var flg = 0;

    for (var i = 0; i < gSalesPointInfo.sales_point[0].grd.length; i++) {
        if (gSalesPointInfo.sales_point[0].grd.length == 1) {
            if (objControl['grade'].checked == true) {
                flg = 1;
            }
        } else {
            if (objControl['grade'][i].checked == true) {
                flg = 1;
            }
        }
    }

    if (flg == 0) {
        //メッセージ表示
        alert($("#greadMessageField").val());
        return false;

    }

    //ローディング開始
    showLoding();

    //ダミーボタンをクリック
    $("#CheckButton").click();

    return false;

}

/**
* 削除ボタン押下イベント
*/
function deleteSalesPointInfo() {
    //削除確認
    if (!confirm($("#deleteAlertField").val())) {
        return false;
    }

    //ファイル削除
    delFiles();

    //ローディング開始
    showLoding();

    //ダミーボタンをクリック
    $("#DeleteButton").click();

    return false;

}


/**
* ファイル選択変更イベント
*/
function onChangeUploadFile(mode) {

    //編集状態にする
    onChangeDisplay();

    if (mode == 1) {
        //概要ファイル名をクリア
        $("#overViewFile").text("");
    } else if (mode == 2) {
        //詳細ファイル名をクリア
        $("#popUpFile").text("");
    } else if (mode == 3) {
        //詳細(拡大画像)ファイル名をクリア
        $("#fullPopUpFile").text("");
    }
}

/**
* 概要ファイル削除ボタン押下イベント
*/
function deleteSummaryFile() {
    //削除確認
    if (!confirm($("#summaryAlertField").val())) {
        return false;
    }
    //編集状態にする
    onChangeDisplay();

    //概要ファイル名をクリア
    $("#overViewFile").text("");
    $("#overViewFileNameField").val("");
    $("#summaryFile").val("");

}

/**
* 詳細ファイル削除ボタン押下イベント
*/
function deleteDetailFile() {
    //削除確認
    if (!confirm($("#detailAlertField").val())) {
        return false;
    }
    //編集状態にする
    onChangeDisplay();

    //詳細ファイル名をクリア
    $("#popUpFile").text("");
    $("#popUpFileNameField").val("");
    $("#detailFile").val("");

    //詳細(拡大画像)のスタイルを判定
    setDetailPopupDisabled();

}

/**
* 詳細(拡大画像)ファイル削除ボタン押下イベント
*/
function deleteDetailPopupFile() {
    //削除確認
    if (!confirm($("#detailPopupAlertField").val())) {
        return false;
    }
    //編集状態にする
    onChangeDisplay();

    //詳細(拡大画像)ファイル名をクリア
    $("#fullPopUpFile").text("");
    $("#fullPopUpFileNameField").val("");
    $("#detailPopupFile").val("");

}

/**
* 詳細(拡大画像)スタイル判定
*/
function setDetailPopupDisabled() {

    //詳細ファイルの拡張子を取得
    var detailFileName = "";
    var detailFileList = document.getElementById("detailFile").files;
    for (var i = 0; i < detailFileList.length; i++) {
        detailFileName = detailFileList[i].name
    }

    if (detailFileName != "") {
        if (getExtention(detailFileName) != VIDEO_MP4 &&
            getExtention(detailFileName) != VIDEO_MP4_BIG &&
            getExtention(detailFileName) != VIDEO_MOV &&
            getExtention(detailFileName) != VIDEO_MOV_BIG) {
            $("#detailPopupFile").attr("disabled", "");
        } else {
            $("#detailPopupFile").attr("disabled", "disabled");
        }
    } else if ($("#popUpFile").text() != "") {
        if (getExtention($("#popUpFile").text()) != VIDEO_MP4 &&
            getExtention($("#popUpFile").text()) != VIDEO_MP4_BIG &&
            getExtention($("#popUpFile").text()) != VIDEO_MOV &&
            getExtention($("#popUpFile").text()) != VIDEO_MOV_BIG) {
            $("#detailPopupFile").attr("disabled", "");
        } else {
            $("#detailPopupFile").attr("disabled", "disabled");
        }
    } else {
        $("#detailPopupFile").attr("disabled", "");
    }
   
}

/**
* ファイル参照を削除します。
*/
function delFiles() {
    // ファイル削除
    $("#summaryFile").val("");
    $("#detailFile").val("");
    $("#detailPopupFile").val("");
}