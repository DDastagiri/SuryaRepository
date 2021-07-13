/** 
* @fileOverview SPMフレーム処理を記述するファイル.
* 
* @author TMEJ m.asano
* @version 1.0.0
*/

$(function () {

    initFrame();

    /**
    * フレームを初期化する.
    */
    function initFrame() {

        // iFrameの生成
        var frame = $('<iframe src="' + $('#SpmUrl').val() + '&uid=' + $('#UrlParam').val() + '&irregClassCd=' + $('#IrregClassCode').val() + '&irregItemCd=' + $('#IrregItemCode').val() + '"width="1024px" height="655px" scrolling="no" id="SpmFrame" seamless></iframe>');

        // iFrameの追加
        $('#Pages_SC3120201').append(frame);
    }
});

