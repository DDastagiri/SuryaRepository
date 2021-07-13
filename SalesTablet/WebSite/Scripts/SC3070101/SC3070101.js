/** 	
* @fileOverview 在庫状況の画面制御クラス.	
* 	
* @author KN Asano
* @version 1.0.0	
* 更新： 2013/07/19 TMEJ m.asano ポップアップの表示位置制御 $01
* 更新： 2013/09/27 TMEJ t.shimamura iOS7対応 $02
*/

// ==============================================================
// 定数
// ==============================================================
// タッチデバイスの判定用
var gIsTouch = false;
var gAgent = navigator.userAgent.toLowerCase();
if (0 <= gAgent.indexOf('iphone') || 0 <= gAgent.indexOf('ipad')) {
    gIsTouch = true;
}

// タッチ系イベント名
var C_TOUCH_START = gIsTouch ? 'touchstart' : 'mousedown';
var C_TOUCH_MOVE = gIsTouch ? 'touchmove' : 'mousemove';
var C_TOUCH_END = gIsTouch ? 'touchend' : 'mouseup';

// ==============================================================
// 変数
// ==============================================================
// ポップアップフラグ
var gPopupFlg = false;
// 画面検索時のロックフラグ
var gLockFlg = false;
// リストタッチフラグ
var gListTopFlg = false;
// 初期化フラグ
var gListOpenFlg = false;

$(window).load(function () {

    // -------------------------------
    // 初期表示
    // -------------------------------
    if ($('#DisplayClassValue').val() == '0') {
        // 初期値を保持
        $('#GradeSearchValue').attr('value', $('#Lable_GradeSearch').attr('textContent'));
        $('#SuffixSearchValue').attr('value', $('#Lable_SuffixSearch').attr('textContent'));

        //グレード件数が0の場合は文字色変更
        if ($("#GradeSerchNumber").val() == "0") {
            $("#Lable_GradeSearch").css("color", "#AAA");
            $("#Lable_SuffixSearch").css("color", "#AAA");
            $("#Lable_ColorSearch").css("color", "#AAA");
        }

    } else {
        // 初期値を保持
        $('#GradeSearchValue').attr('value', $('#Lable_GradeSearchGL2').attr('textContent'));
        $('#SuffixSearchValue').attr('value', $('#Lable_SuffixSearchGL2').attr('textContent'));

        //グレード件数が0の場合は文字色変更
        if ($("#GradeSerchNumber").val() == "0") {
            $("#Lable_GradeSearchGL2").css("color", "#AAA");
            $("#Lable_SuffixSearchGL2").css("color", "#AAA");
            $("#Lable_ColorSearchGL2").css("color", "#AAA");
        }
    }
});

//ロードスクリーン
function LoadingScreen() {
    $('#LoadButton').click();
    DispLoadingIcon(true);
};

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // -------------------------------
    // 非同期ポストバック用処理
    // -------------------------------
    var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();

    // 非同期ポストバックの完了後に呼び出される
    pageRequestManager.add_endRequest(
        function (aSender, aArgs) {

            if ($('#LoadingFlg').val() == '1') {
                return;
            }

            // スクロール設定
            $(".GradeListItemBox").fingerScroll({ popover: true });
            $(".SfxListItemBox").fingerScroll({ popover: true });
            $(".ExteriorListItemBox").fingerScroll({ popover: true });

            // ロック解除
            gLockFlg = false;

            // ロードアイコン非表示
            DispLoadingIcon(false);

            // 非同期通信に失敗した場合
            if (aArgs.get_error() != undefined) {
                onFailedClient(aArgs.get_error().message);
                aArgs.set_errorHandled(true);
            }

            // $01 START
            initPopOver();
            // $01 END

            if ($('#UpDateTypeValue').val() == '1') {
                if ($('#DisplayClassValue').val() == '0') {
                    $('#zaiko_GL1').css("display", "block");
                } else {
                    $('#zaiko_GL2').css("display", "block");
                }
            }
            else {
                var target = null;
                if ($('#DisplayClassValue').val() == '0') {
                    target = $('#zaiko_GL1');
                } else {
                    target = $('#zaiko_GL2');
                }
                if (gListOpenFlg || target.css("display") == 'block') {
                    target.css("display", "block");
                } else {
                    target.css("display", "none");
                }
            }
            if ($('#DisplayClassValue').val() == '0') {
                // 初期値を保持
                $('#GradeSearchValue').attr('value', $('#Lable_GradeSearch').attr('textContent'));
                $('#SuffixSearchValue').attr('value', $('#Lable_SuffixSearch').attr('textContent'));
            } else {
                // 初期値を保持
                $('#GradeSearchValue').attr('value', $('#Lable_GradeSearchGL2').attr('textContent'));
                $('#SuffixSearchValue').attr('value', $('#Lable_SuffixSearchGL2').attr('textContent'));
            }
        }
    );

    // スクロール設定
    $(".GradeListItemBox").fingerScroll({ popover: true });
    $(".SfxListItemBox").fingerScroll({ popover: true });
    $(".ExteriorListItemBox").fingerScroll({ popover: true });

    initPopOver();

    /**
    * エリア選択時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('div#bodyFrame').live(C_TOUCH_START, function (aEvent) {

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        if ($('#DisplayClassValue').val() == '0') {
            // グレード検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#GradeWindown').length > 0) {
                return;
            }
            // SFX検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#SfxWindown').length > 0) {
                return;
            }

            // 外装色検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#ExteriorWindown').length > 0) {
                return;
            }

            // 検索リストエリア外の場合は、表示中の検索リストを閉じる
            if ($('div#GradeWindown.active').length > 0) {
                $('#GradeSelectAria > p').trigger('hidePopover');
            }
            if ($('div#SfxWindown.active').length > 0) {
                $('#SuffixSelectAria > p').trigger('hidePopover');
            }
            if ($('div#ExteriorWindown.active').length > 0) {
                $('#ColorSelectAria > p').trigger('hidePopover');
            }

        } else {
            // グレード検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#GradeWindownGL2').length > 0) {
                return;
            }
            // SFX検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#SfxWindownGL2').length > 0) {
                return;
            }
            // 外装色検索リスト表示時、リスト範囲内でのタップの場合は処理を行わない。
            if ($(aEvent.target).parents('div#ExteriorWindownGL2').length > 0) {
                return;
            }

            // 検索リストエリア外の場合は、表示中の検索リストを閉じる
            if ($('div#GradeWindownGL2.active').length > 0) {
                $('#GradeSelectAriaGL2 > p').trigger('hidePopover');
            }
            if ($('div#SfxWindownGL2.active').length > 0) {
                $('#SuffixSelectAriaGL2 > p').trigger('hidePopover');
            }
            if ($('div#ExteriorWindownGL2.active').length > 0) {
                $('#ColorSelectAriaGL2 > p').trigger('hidePopover');
            }
        }

    });

    var selectValue = "";
    var selectObject = null;

    /**
    * グレード検索押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#GradeSelectAria > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }

        // 検索中は操作させない。
        if ($('#LoadingAria').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_GradeSearch').attr('textContent');
        ListDispInit(selectValue, '.GradeListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#GradeWindown').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#GradeWindown').css('top', calcTop + 'px');
        // $01 END
    });

    /**
    * グレード検索押下時の制御を行う。 GL2版
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#GradeSelectAriaGL2 > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }
        // 検索中は操作させない。
        if ($('#LoadingAriaGL2').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_GradeSearchGL2').attr('textContent');
        ListDispInit(selectValue, '.GradeListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#GradeWindownGL2').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#GradeWindownGL2').css('top', calcTop + 'px');
        // $01 END
    });

    /**
    * グレード検索リストのタッチ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.GradeListBoxSetIn1 li').live(C_TOUCH_START, function (aEvent) {

        gListTopFlg = true;
    });

    /**
    * グレード検索リストのタッチムーブ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.GradeListBoxSetIn1 li').live(C_TOUCH_MOVE, function (aEvent) {

        gListTopFlg = false;
    });

    /**
    * グレード検索リストのタッチエンド時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.GradeListBoxSetIn1 li').live(C_TOUCH_END, function (aEvent) {

        if (!gListTopFlg) {

            return;
        }

        // 選択された値を取得する。
        selectObject = aEvent.target;

        // タップされたリストを選択状態へ変更
        ListDispInit(selectObject.innerText, '.GradeListBoxSetIn1 li.Arrow');

        // リスト更新処理
        UpdateStockList(1);
    });

    /**
    * SFX検索押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#SuffixSelectAria > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }

        // 検索中は操作させない。
        if ($('#LoadingAria').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_SuffixSearch').attr('textContent');
        ListDispInit(selectValue, '.SfxListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#SfxWindown').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#SfxWindown').css('top', calcTop + 'px');
        // $01 END
    });

    /**
    * SFX検索押下時の制御を行う。 GL2版
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#SuffixSelectAriaGL2 > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }

        // 検索中は操作させない。
        if ($('#LoadingAriaGL2').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_SuffixSearchGL2').attr('textContent');
        ListDispInit(selectValue, '.SfxListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#SfxWindownGL2').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#SfxWindownGL2').css('top', calcTop + 'px');
        // $01 END
    });

    /**
    * SFX検索リストのタッチ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.SfxListBoxSetIn1 li').live(C_TOUCH_START, function (aEvent) {

        gListTopFlg = true;
    });

    /**
    * SFX検索リストのタッチムーブ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.SfxListBoxSetIn1 li').live(C_TOUCH_MOVE, function (aEvent) {

        gListTopFlg = false;
    });

    /**
    * SFX検索リストのタッチエンド時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.SfxListBoxSetIn1 li').live(C_TOUCH_END, function (aEvent) {

        if (!gListTopFlg) {

            return;
        }

        // 選択された値を取得する。
        selectObject = aEvent.target;

        // タップされたリストを選択状態へ変更
        ListDispInit(selectObject.innerText, '.SfxListBoxSetIn1 li.Arrow');

        // リスト更新処理
        UpdateStockList(2);
    });

    /**
    * 外装色検索押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#ColorSelectAria > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }

        // 検索中は操作させない。
        if ($('#LoadingAria').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_ColorSearch').attr('textContent');
        ListDispInit(selectValue, '.ExteriorListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#ExteriorWindown').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#ExteriorWindown').css('top', calcTop + 'px');
        // $01 END
    });

    /**
    * 外装色検索押下時の制御を行う。 GL2版
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#ColorSelectAriaGL2 > p').live(C_TOUCH_START, function (aEvent) {

        //イベントパブリングを抑制
        aEvent.stopPropagation();

        if (gPopupFlg) {
            gPopupFlg = false;
            return;
        }

        //グレード件数が0の場合は開かせない
        if ($("#GradeSerchNumber").val() == "0") {
            return;
        }

        // 検索中は操作させない。
        if ($('#LoadingAriaGL2').css('display') != 'none') {
            return;
        }

        selectValue = $('#Lable_ColorSearchGL2').attr('textContent');
        ListDispInit(selectValue, '.ExteriorListBoxSetIn1 li.Arrow');

        gPopupFlg = true;
        selectObject = null;

        // $01 START
        $(this).trigger('showPopover');
        var top = $('#ExteriorWindownGL2').attr('offsetTop');
        var calcTop = top - $('div.zaikoBoxSetWide', 'div#main').offset().top;
        $('#ExteriorWindownGL2').css('top', calcTop + 'px');
        // $01 END
    });


    /**
    * 外装色検索リストのタッチ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.ExteriorListBoxSetIn1 li').live(C_TOUCH_START, function (aEvent) {

        gListTopFlg = true;
    });

    /**
    * 外装色検索リストのタッチムーブ時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.ExteriorListBoxSetIn1 li').live(C_TOUCH_MOVE, function (aEvent) {

        gListTopFlg = false;
    });

    /**
    * 外装色検索リストのタッチエンド時の制御を行う。
    * 
    * @param {Object} aEvent イベントObject
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.ExteriorListBoxSetIn1 li').live(C_TOUCH_END, function (aEvent) {

        if (!gListTopFlg) {

            return;
        }

        // 選択された値を取得する。
        selectObject = aEvent.target;

        // タップされたリストを選択状態へ変更
        ListDispInit(selectObject.innerText, '.ExteriorListBoxSetIn1 li.Arrow');

        // リスト更新処理
        UpdateStockList(3);
    });

    /**
    * 検索リスト決定ボタン押下時の制御を行う。
    * 
    * @param {Integer} aType 検索タイプ
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function UpdateStockList(aType) {

        var setValue = "";

        // NULLチェック
        if (selectObject == null) {
            setValue = selectValue;
        }
        else {
            setValue = selectObject.innerText;
        }

        setValue = setValue.replace(/^\s+|\s+$/g, "");

        // 画面操作ロック
        gLockFlg = true;
        $('#LoadingFlg').attr('value', '1');

        if ($('#DisplayClassValue').val() == '0') {
            // グレード検索リスト
            if (aType == 1) {

                // 選択値を保持
                $('#Lable_GradeSearch').attr('textContent', setValue);
                $('#GradeSearchValue').attr('value', setValue);
                if ($(selectObject).parent(".Arrow").length > 0) {
                    // $02 start iOS7対応
                    $('#GradeCodeSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#GradeCode').attr("Value"));
                    $('#GradeSuffixSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#GradeSuffix').attr("Value"));
                    // $02 end iOS7対応
                }
                else {
                    // $02 start iOS7対応
                    $('#GradeCodeSearchValue').attr('value', $(selectObject).children('#GradeCode').attr("Value"));
                    $('#GradeSuffixSearchValue').attr('value', $(selectObject).children('#GradeSuffix').attr("Value"));
                    // $02 end iOS7対応
                }
                $('#GradeWindown').css('display', 'none');

                //リスト更新
                $('#ListUpdateGradeButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }

            // SFX検索リスト
            else if (aType == 2) {

                // 選択値を保持
                $('#Lable_SuffixSearch').attr('textContent', setValue);
                $('#SuffixSearchValue').attr('value', setValue);
                $('#SuffixSearchValueChange').attr('value', '1');
                $('#SfxWindown').css('display', 'none');

                //リスト更新
                $('#ListUpdateButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }

            // 外装色検索リスト
            else {

                // 選択値を保持
                $('#Lable_ColorSearch').attr('textContent', setValue);
                $('#ColorSearchValue').attr('value', setValue);

                if ($(selectObject).parent(".Arrow").length > 0) {
                    // $02 start iOS7対応
                    $('#ColorCodeSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#ColorCode').attr("Value"));
                    // $02 end iOS7対応
                }
                else {
                    // $02 start iOS7対応
                    $('#ColorCodeSearchValue').attr('value', $(selectObject).children('#ColorCode').attr("Value"));
                    // $02 end iOS7対応

                }

                $('#ExteriorWindown').css('display', 'none');

                //リスト更新
                $('#ListUpdateButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }
        } else {
            // グレード検索リスト
            if (aType == 1) {

                // 選択値を保持
                $('#Lable_GradeSearchGL2').attr('textContent', setValue);
                $('#GradeSearchValue').attr('value', setValue);
                if ($(selectObject).parent(".Arrow").length > 0) {
                    // $02 start iOS7対応
                    $('#GradeCodeSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#GradeCodeGL2').attr("Value"));
                    $('#GradeSuffixSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#GradeSuffixGL2').attr("Value"));
                    // $02 end iOS7対応
                }
                else {
                    // $02 start iOS7対応
                    $('#GradeCodeSearchValue').attr('value', $(selectObject).children('#GradeCodeGL2').attr("Value"));
                    $('#GradeSuffixSearchValue').attr('value', $(selectObject).children('#GradeSuffixGL2').attr("Value"));
                    // $02 end iOS7対応
                }
                $('#GradeWindownGL2').css('display', 'none');

                //リスト更新
                $('#ListUpdateGradeButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }

            // SFX検索リスト
            else if (aType == 2) {

                // 選択値を保持
                $('#Lable_SuffixSearchGL2').attr('textContent', setValue);
                $('#SuffixSearchValue').attr('value', setValue);
                $('#SuffixSearchValueChange').attr('value', '1');
                $('#SfxWindownGL2').css('display', 'none');

                //リスト更新
                $('#ListUpdateButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }

            // 外装色検索リスト
            else {

                // 選択値を保持
                $('#Lable_ColorSearchGL2').attr('textContent', setValue);
                $('#ColorSearchValue').attr('value', setValue);

                if ($(selectObject).parent(".Arrow").length > 0) {
                    // $02 start iOS7対応
                    $('#ColorCodeSearchValue').attr('value', $(selectObject).parent(".Arrow").children('#ColorCodeGL2').attr("Value"));
                    // $02 end iOS7対応
                }
                else {
                    // $02 start iOS7対応
                    $('#ColorCodeSearchValue').attr('value', $(selectObject).children('#ColorCodeGL2').attr("Value"));
                    // $02 end iOS7対応

                }

                $('#ExteriorWindownGL2').css('display', 'none');

                //リスト更新
                $('#ListUpdateButton').click();

                //ロードアイコン表示
                DispLoadingIcon(true);
            }
        }
    };

    /**
    * 検索リストの表示クリアを行う。
    * 
    * @param {Object} aCheckValue チェックをつけるリスト値
    * @param {String} aListName クリア対象となるリストのクラス名
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function ListDispInit(aCheckValue, aListName) {

        for (i = 0; i < $(aListName).size(); i++) {

            var target = $(aListName).eq(i)[0].innerText
            target = target.replace(/^\s+|\s+$/g, "");
            if (target == aCheckValue) {

                // チェックマークをつける
                $(aListName).eq(i).find('.Selection').css('display', 'block');
            }
            else {

                // チェックマークをはずす
                $(aListName).eq(i).find('.Selection').css('display', 'none');
            }
        }
    }

    /**
    * チップのタップ時の処理
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    var action = null;
    $('#TipBlock').live(C_TOUCH_START, function (aEvent) {

        if (action == null) {

            TipDispChange(true);

            /* 3秒後に色コードを表示し、日付を非表示にする */
            action = setTimeout(
                function () {
                    TipDispChange(false);
                    clearTimeout(action);
                    action = null;
                }, 3000);
        }
        else {

            TipDispChange(false);
            clearTimeout(action);
            action = null;
        }
    })

    /**
    * チップの表示変更を行う。
    * 
    * @param {bool} aChangeTypeDate 日付表示するかどうか
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function TipDispChange(aChangeTypeDate) {
        // $02 start iOS7対応
        for (i = 0; i < $('.SelectionList').find('.icn02, .icn03, .icn01, .icn01b, .icn02b, .icn03b').size(); i++) {
            var color = $('.SelectionList').find('.icn02, .icn03, .icn01, .icn01b, .icn02b, .icn03b').eq(i).children('#TipValueColor');
            var date = $('.SelectionList').find('.icn02, .icn03, .icn01, .icn01b, .icn02b, .icn03b').eq(i).children('#TipValueDate');
            // $02 end iOS7対応
            if (aChangeTypeDate) {

                // チェックマークをつける
                color.css('display', 'none');
                date.css('display', 'block');
            }
            else {
                // チェックマークをはずす
                color.css('display', 'block');
                date.css('display', 'none');
            }
        }
    }

});

// ==============================================================
// 関数
// ==============================================================
/**
* 非同期通信時のエラー処理を行う.
*/
function onFailedClient(errorMessage) {
    $('input#ErrorMessage').val(errorMessage);
    $('input#SendErrorMessageButton').click();
}

/**
* ロードアイコンの表示変更を行う。
* 
* @param {bool} aIsDisp 表示するかどうか True:表示/False:非表示
* @return {-} -
* 
* @example 
*  -
*/
function DispLoadingIcon(aIsDisp) {

    var target = null
    var loadAriaTarget = null;

    if ($('#DisplayClassValue').val() == '0') {
        target = $('.SelectionList').children('tbody').children('#DetailAria');
        loadAriaTarget = $('#LoadingAria');
    } else {
        target = $('.SelectionList').children('tbody').children('#DetailAriaGL2');
        loadAriaTarget = $('#LoadingAriaGL2');
    }        
    
    // ロードアイコン表示
    if (aIsDisp) {
        // $02 start iOS7対応
        for (i = 0; i < target.size(); i++) {
            target.eq(i).addClass('DisplayNone');
        }
        // $02 end iOS7対応

        loadAriaTarget.removeClass('DisplayNone');
    }
    // ロードアイコン非表示
    else {
        // $02 start iOS7対応
        for (i = 0; i < target.size(); i++) {
            target.eq(i).removeClass('DisplayNone');
        }
        // $02 end iOS7対応

        loadAriaTarget.addClass('DisplayNone');
    }
}

// $01 START
/**
* 検索リストの初期化処理
* 
* @param {-} -
* @return {-} -
* 
* @example 
*  -
*/
function initPopOver() {
    var leftPos = $('div.zaikoBoxSetWide', 'div#main').offset().left * -1;

    if ($('#DisplayClassValue').val() == '0') {
        // グレードリスト
        if ($('#GradeSelectAria > p').hasClass("popover-button")) {
            $('#GradeSelectAria > p.popover-button').unbind();
        }
        $('#GradeSelectAria > p').popoverEx({
            contentId: $('div#GradeWindown')
            , offsetX: leftPos
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });

        // SFXリスト
        if ($('#SuffixSelectAria > p').hasClass("popover-button")) {
            $('#SuffixSelectAria > p.popover-button').unbind();
        }
        $('#SuffixSelectAria > p').popoverEx({
            contentId: $('div#SfxWindown')
            , offsetX: leftPos
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });

        // 外装色リスト
        if ($('#ColorSelectAria > p').hasClass("popover-button")) {
            $('#ColorSelectAria > p.popover-button').unbind();
        }
        $('#ColorSelectAria > p').popoverEx({
            contentId: $('div#ExteriorWindown')
            , offsetX: leftPos - 16
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });

    } else {
        if ($('#GradeSelectAriaGL2 > p').hasClass("popover-button")) {
            $('#GradeSelectAriaGL2 > p.popover-button').unbind();
        }
        $('#GradeSelectAriaGL2 > p').popoverEx({
            contentId: $('div#GradeWindownGL2')
            , offsetX: leftPos
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });

        // SFXリスト
        if ($('#SuffixSelectAriaGL2 > p').hasClass("popover-button")) {
            $('#SuffixSelectAriaGL2 > p.popover-button').unbind();
        }
        $('#SuffixSelectAriaGL2 > p').popoverEx({
            contentId: $('div#SfxWindownGL2')
            , offsetX: leftPos
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });

        // 外装色リスト
        if ($('#ColorSelectAriaGL2 > p').hasClass("popover-button")) {
            $('#ColorSelectAriaGL2 > p.popover-button').unbind();
        }
        $('#ColorSelectAriaGL2 > p').popoverEx({
            contentId: $('div#ExteriorWindownGL2')
            , offsetX: leftPos - 16
            , preventLeft: true
            , preventRight: true
            , preventTop: false
            , preventBottom: false
        });
    }

    // タイトルタップ時にエリアの表示非表示切り替え
    $('#Lable_ZaikoJokyo').unbind();
    $('#Lable_ZaikoJokyo').bind(C_TOUCH_START, function (aEvent) {

        var target = null;

        if ($('#DisplayClassValue').val() == '0') {

            target = $('#zaiko_GL1');

        } else {

            target = $('#zaiko_GL2');

        }
        if (target.css("display") != 'none') {
            gListOpenFlg = false;
            target.css("display", "none");

        } else {
            gListOpenFlg = true;
            target.css("display", "block");

        }
    });
}

// $01 END