// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//DOMロード時の処理
$(function () {

    $('#MstPG_FootItem_Main_200').bind("click", function (event) {
        // alert("click17");
        $('#MstPG_CustomerSearchTextBox').focus();
        //background: "url(../Styles/Images/FooterButtons/200_on.png)",
        $('#MstPG_FootItem_Main_200').css({ backgroundColor: "#0066FF" });
        setTimeout(function () {
            $('#MstPG_FootItem_Main_200').css({ backgroundColor: "" });
        }, 300);
        $.stopPropagation();
    });

    SetFutterApplication();
});

//フッターアプリの起動設定
function SetFutterApplication() {

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
