/// <reference path="jquery.js"/>
/// <reference path="eCRB.js"/>
/// <reference path="eCRB.ui.js"/>

/****************************************************************************

マスターページに関する処理のjQUERY拡張

****************************************************************************/
(function (window) {
    $.extend({ master: {

        blinkIcropLogoTimer: null,

        //i-CROPアイコン点滅開始
        blinkStartIcropLogo: function() {
            this.blinkIcropLogoTimer = setInterval(function() {
                 $("#mstpg_icropLogo").is(":hidden") ? $("#mstpg_icropLogo").fadeIn(200) : $("#mstpg_icropLogo").fadeOut(200);
            }, 200);
        },

        //i-CROPアイコン点滅終了
        blinkEndIcropLogo: function() {
            if (this.blinkIcropLogoTimer) clearInterval(this.blinkIcropLogoTimer);
            $("#mstpg_icropLogo").show(0);
        },

        OpenLoadingScreen: function () {
            $("#MstPG_LoadingScreen").css({ "width": $(window).width() + "px", "height": $(window).height() + "px" });
            setTimeout(function () {
                $("#MstPG_LoadingScreen").css({ "display": "table"});
            }, 0);
        },

        CloseLoadingScreen: function() {
            $("#MstPG_LoadingScreen").css({ "display": "none" });
        }
    }});
})(window);


/****************************************************************************

キーボード制御

****************************************************************************/
$(function () {

    $(document).keydown(function (e) {
        
        if (e.which != 13) return true; //13:Enterキー(Goボタン)
        var tclass = (e.target.className).toUpperCase();

        if (tclass == "VALIDKBPROTECT") {
            e.target.blur();
            return false;
        }

        var tagName = (e.target.tagName).toUpperCase();
        if (tagName != "INPUT" && tagName != "SELECT") return true;
        if (tagName == "INPUT") {
            var type = (e.target.type).toUpperCase();
            if (type == "SEARCH" || type == "PASSWORD") return true;
        }

        if (tclass == "UNVALIDKBPROTECT") return true;
        e.target.blur();
        return false;
    });

});
