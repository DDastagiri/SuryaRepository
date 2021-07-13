//------------------------------------------------------------------------------
//SC3250103.js
//------------------------------------------------------------------------------
//機能：部品説明_javascript
//
//作成：2014/07/XX NEC 上野	初版作成
//更新：
//------------------------------------------------------------------------------

var CONTENT_CLASS_NAME = "pdcontentBox";    //コンテンツボックスのCSSクラス名
var CONTENT_MINIMIZE = "OnlyTitleBar";      //コンテンツボックス非表示　CSSクラス名
var CONTENT_SHOW = "ShowContents";          //コンテンツボックス表示　　CSSクラス名
var MINIMIZE_HEIGHT = 35;                   //コンテンツボックス非表示時の高さ
var IsTouch = false;

$(function () {
    //フッターアプリの起動設定
    SetFooterApplication();

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {
        //ヘッダーの顧客検索にフォーカスを当てる
        $('#MstPG_CustomerSearchTextBox').focus();
        event.stopPropagation();
    });

    //スクロール機能設定
    /*すべてのコンテンツを最小化（タイトルのみ表示）状態にしたとき、
    標準のFingerscrollではスクロールできなくなり、画面が途切れる症状が起きるため、
    内部コンテンツがスクロールエリアからはみ出ていない状態でもスクロール機能が有効になる
    mainMenuFingerScrollを使用*/
    //$("#mainScroll").fingerScroll();
    $("#mainScroll").mainMenuFingerScroll({ scrollMode: "all" });

    //ヘッダーの戻るボタンをタップしたときにクルクルを表示する
/*    $('#MstPG_BackLinkButton').bind("click", function () {
        ActiveDisplayOn();
    });
*/
    //iFrame読み込み完了後に呼び出すメソッドを追加する
    SetClearProcessIcon();

    //部品説明エリアのURLが空白の場合は、コンテンツ２～５（タイトル部分）を表示する
    if (document.getElementById('iFrame1').src == "") {
        setContentDisplay();
    }

});

// フッターボタンの2度押し制御
function FooterButtonControl() {

    $.master.OpenLoadingScreen();
    return true;
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

/**
* フッター部のアプリ
* @return {void}
*/
function SetFooterApplication() {

    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class アプリ起動クラス
        */
        appExecute: {

            /**
            * カレンダーアプリ起動(単体)
            */
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            /**
            * 電話帳アプリ起動(単体)
            */
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }

    });
}


// カートボタン押下時処理
function OnClickCart() {
    var CartImg = document.getElementById("imgCart");
    if (CartImg.className == "Cart_Enable") {
        //ボタン活性時はカート画面遷移実行
        //クルクル表示
        ActiveDisplayOn();

        //カートボタンをクリックして、サーバサイドにて画面遷移処理を実行
        document.getElementById('ButtonCart').click();

        return false;
    } else {
        //ボタン非活性時は何もしない
        return false;
    }
}


//各コンテンツのヘッダー部分をタップしたときの処理
function ClickContentHeader(ContentID) {

    //表示／最小化するコンテンツボックスを指定
    var ContentBox = document.getElementById("ContentBox" + ContentID);

    //コンテンツボックスを表示／最小化する
    if (ContentBox.clientHeight == MINIMIZE_HEIGHT) {
        //表示
        ContentBox.className = CONTENT_CLASS_NAME + " " + CONTENT_SHOW + ContentID;

        //表示時に表示するページが指定されていないときはURLをセットする
        var targetFrame = document.getElementById("iFrame" + ContentID);
        if (ContentID != "1" && targetFrame.src == "") {
            targetFrame.src = document.getElementById("hdnContent" + ContentID + "URL").value;
        }

    } else {
        //最小化（非表示）
        ContentBox.className = CONTENT_CLASS_NAME + " " + CONTENT_MINIMIZE;
    }

}

//各コンテンツのiFrame読み込み完了後にクルクルを消去する処理を追加
function SetClearProcessIcon() {

    document.getElementById('iFrame1').onload = function () {
        //クルクルを消す
        document.getElementById('ServerProcessIcon1').style.display = 'none';

        //ページID付きURLの場合は、再度URLをセットする（再度セットすることでスクロールが正常に動作できるようになるため）
        var PageId = document.getElementById('hdnContent1_PageId').value;
        if (PageId != "") {
            //URLをセットするfunctionを作成
            var setPageLinkURL = function () {
                var PageUrl = document.getElementById('iFrame1').src;
                //var PageId = document.getElementById('hdnContent1_PageId').value;
                //document.getElementById('iFrame1').src = PageUrl + "#" + PageId;
                document.getElementById('iFrame1').src = PageUrl;
            }

            //ページ内リンク付URLをセットするタイマーセット
            var settime = setTimeout(setPageLinkURL, 100);
        }

        //コンテンツ２～コンテンツ５を表示するfunctionを作成
        var displayContents = function () {
            setContentDisplay();
        }

        //コンテンツ２～５を表示するタイマーセット
        var settime = setTimeout(displayContents, 200);

    };
    document.getElementById('iFrame2').onload = function () {
        //クルクルを消す
        document.getElementById('ServerProcessIcon2').style.display = 'none';
    };
    document.getElementById('iFrame3').onload = function () {
        //クルクルを消す
        document.getElementById('ServerProcessIcon3').style.display = 'none';
    };
    document.getElementById('iFrame4').onload = function () {
        //クルクルを消す
        document.getElementById('ServerProcessIcon4').style.display = 'none';
    };
    document.getElementById('iFrame5').onload = function () {
        //クルクルを消す
        document.getElementById('ServerProcessIcon5').style.display = 'none';
    };

}

//各コンテンツの表示・非表示設定
function setContentDisplay() {

    //コンテンツ１の高さ調整
    document.getElementById("ContentBox1").className = CONTENT_CLASS_NAME + " " + CONTENT_SHOW + "1";

    //隠しフィールドから取得
    var Content = document.getElementById('hdnDisplayFlag').value.split(",");

    //各コンテンツ表示
    for (var i = 1; i < 5; i++) {
        if (Content[i] == '1') {
            var area = i + 1;
            document.getElementById('ContentBox' + area).style.removeProperty('display');
        }
    }
}

/*
function CoverTouchEnd(contentID) {

    //タッチイベントではなかった場合は終了する
    if (IsTouch == false) {
        return false;
    }

    //引数のIDコンテンツを非表示にする
    document.getElementById(contentID).style.display = 'none';

    //一定時間後、IDコンテンツを表示する
    var afterFunc = function () { document.getElementById(contentID).style.display = 'block'; };
    setTimeout(afterFunc, 400);
}
*/

