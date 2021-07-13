
var startX = 0;
var endX = 0;
var startY = 0;
$("document").ready(function () {

    $('#main').bind('touchstart', function (e) {
        var touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
        startX = touch.pageX;
	startY = touch.pageY;
    });
    //返回圈
/*    $('#MstPG_BackLinkButton').bind("click", function () {

        dispLoading();
    });
*/
    $('#main').bind('touchmove', function (e) {
        e.preventDefault();
        var touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
        var elm = $(this).offset();
        var endX = touch.pageX;
  	var endY = touch.pageY;
        var x = touch.pageX - elm.left;
        var y = touch.pageY - elm.top;
        if (x < $(this).width() && x > 0) {
            //if (y < $(this).height() && y > 0) {
		if (Math.abs(endY - startY)<20) {
                if (Math.abs(endX - startX) > 200) {
                    if (endX - startX > 0) {
                        moveleft();
                    }
                    else {
                        moveright();
                    }
                }
            }
        }
    });
    
});

//オーバーレイ、ロード中表示
function dispLoading() {

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション(ロード中)
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);
    return true;
}

//一時的なNetwork障害などによって発生したクルクル現象の対応としての画面全体リロード処理のタイマー設定
function reloadPageIfNoResponse() {
    timerClearTime = (new Date().getTime()) - 1;
    commonRefreshTimer(reloadPage);
}

function executeCaleNew() {
            var ymd = { year: "", month: "", day: "" };
            ymd.year = $("#Yearhidden").val();
            ymd.month = ("0" + $("#Monthhidden").val()).slice(-2);
            ymd.day = ("0" + $("#Dayhidden").val()).slice(-2);
            window.location = "icrop:cale:::" + ymd.year + "-" + ymd.month + "-" + ymd.day;
            return false;
        }
function SetFutterApplication() {
    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });
    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {
        //アプリ起動クラス
        appExecute: {
            //カレンダーアプリ起動(単体)
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            //電話帳アプリ起動(単体)
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }
    });
}
/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {
    //顧客ボタン、TCメインボタンの場合は何もしない
    if (Id == 700 || Id == 200) {
        return false;
    }

    //全体クルクル表示
    ActiveDisplayOn();

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });
    //各イベント処理実行
    switch (Id) {
        case 100:
            //メインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 300:
            //FMメインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            //予約管理
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            //R/O
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 600:
            //連絡先
            __doPostBack('ctl00$MstPG_FootItem_Main_600', '');
            break;
        case 700:
            //顧客
            __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            //商品訴求コンテンツ
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            //キャンペーン
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1000:
            //全体管理
            __doPostBack('ctl00$MstPG_FootItem_Main_1000', '');
            break;
        case 1100:
            //SMB
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 1200:
            //追加作業
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
    }
}


function executeCont() {
	window.location = "icrop:cont:";
	return false;
}
//2012-6-12
//判断字符串长度
function showAltDiv() {
    if ($("#isshow").val() == "1") {

       // $(".header").css("display", "none");
       // $(".content").css("display", "none");
        //$(".triangleBorder").css("display", "none");
       // $(".popover").css("border", "0");
        $(".altdiv").html($("#textmsg").val());
        //$(".altdiv").css({ "width": 12 * textcount + "px" });
        //$(".TextArea").css("z-index","-1");
        $(".altdiv").css("display", "block");
        //$(".altdiv").show();
        setTimeout(hideAltDiv, 5000);
    }
    //else {

        //$(".header").css("display", "none");
        //$(".content").css("display", "none");
       // $(".triangleBorder").css("display", "none");
       // $(".popover").css("border", "0");
   // }
}


function hideAltDiv() {
    $(".altdiv").css("display", "none");
}
/*
$(function () {

    // var jBrokenItem = $("#showmsg")

    //点击弹出框
    //jBrokenItem.bind("click", showAltDiv);
    // jBrokenItem.popover({ "closeEvent": hideAltDiv });

    //超出div高度控制

    if ($("#lblMemo").get(0).offsetHeight > 176) {
        while ($("#lblMemo").get(0).offsetHeight > 176) {
            $("#lblMemo").text($("#lblMemo").text().substring(0, $("#lblMemo").text().length - 1));

        }
        $("#lblMemo").text($("#lblMemo").text().substring(0, $("#lblMemo").text().length - 2) + "...");
        $("#isshow").val("1");
    }
    else {
        $("#isshow").val("0");
    }

    // 2012/9/6 JIAN 显示▼ START 

    //显示客户的箭头
    setTimeout(function () {
        icropScript.ui.arrowImageOn("200");
        if ($("#resultAddStautsHiddenField").val() == "1" || $("#resultAddStautsHiddenField").val() == "2") {
            icropScript.ui.arrowImageOn("1100");
        }
    }, 500);

    // 2012/9/6 JIAN 显示▼ END 
});
*/