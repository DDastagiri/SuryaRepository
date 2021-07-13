
// ダメージデータ
var gDamageData = {};

// 起動パラメータ
var gUrlParamList = [];

// 部位管理テーブル
var tblPartsCtrlTable = [
            { "id": "00", "name": "FSkirt" },
            { "id": "01", "name": "FrontBumper" },
            { "id": "02", "name": "Grill" },
            { "id": "03", "name": "Hood" },
            { "id": "04", "name": "FrontWindow" },
            { "id": "05", "name": "RightFender" },
            { "id": "06", "name": "RightFrontDoor" },
            { "id": "07", "name": "RightRearDoor" },
            { "id": "08", "name": "RightQuarter" },
            { "id": "09", "name": "RightLocker" },
            { "id": "10", "name": "RightFrontPillar" },
            { "id": "11", "name": "RightFrontSideWindow" },
            { "id": "12", "name": "RightCenterPillar" },
            { "id": "13", "name": "RightRearSideWindow" },
            { "id": "14", "name": "RightRearPillar" },
            { "id": "15", "name": "Roof" },
            { "id": "16", "name": "LeftFender" },
            { "id": "17", "name": "LeftFrontDoor" },
            { "id": "18", "name": "LeftRearDoor" },
            { "id": "19", "name": "LeftQuarter" },
            { "id": "20", "name": "LeftLocker" },
            { "id": "21", "name": "LeftFrontPillar" },
            { "id": "22", "name": "LeftFrontSideWindow" },
            { "id": "23", "name": "LeftCenterPillar" },
            { "id": "24", "name": "LeftRearSideWindow" },
            { "id": "25", "name": "LeftRearPillar" },
            { "id": "26", "name": "RearWindow" },
            { "id": "27", "name": "Trunk" },
            { "id": "28", "name": "BackPanel" },
            { "id": "29", "name": "RearBumper" },
            { "id": "30", "name": "RearSkirt" },
            { "id": "31", "name": "RightFrontWheel" },
            { "id": "32", "name": "RightRearWheel" },
            { "id": "33", "name": "LeftFrontWheel" },
            { "id": "34", "name": "LeftRearWheel" },
            { "id": "35", "name": "SpareWheel" },

        ];

// 損傷種別マスタテーブル
var tblDamageType = new Array;

// 凡例表示
var displayExplanation = function () {
    var MAX_COLUMN = 4;
    var LINE_HEIGHT = 27;
    var COLUMN_WIDTH = 95;

    var target = $("<ul />");

    // 凡例生成
    for (var i in tblDamageType) {
        var outer = $("<li />").addClass("LegendText");
        // 文字とグラデーション表示
        var typeText = $("<div />").addClass("InRound")
                                            .css({ background: makeGradient(tblDamageType[i].type) })
                                            .append(tblDamageType[i].type);
        $("<div />").addClass("LegendPoint")
                            .append(typeText)
                            .appendTo(outer);
        // タイトル文字表示と位置調整
        outer.append($("<span />").text(tblDamageType[i].title)
                                          .attr('style', "width:" + COLUMN_WIDTH + "px;white-space:nowrap; overflow:hidden; -webkit-text-overflow: ellipsis; -o-text-overflow: ellipsis;text-overflow: ellipsis;"))
            .css({
                top: Math.floor(i / MAX_COLUMN) * LINE_HEIGHT,
                left: Math.floor(i % MAX_COLUMN) * COLUMN_WIDTH
            })
            .appendTo(target);
    };

    // 凡例数により高さを調整
    $("div > .KSMainblockCheckIconContents").css({ height: LINE_HEIGHT * Math.ceil(tblDamageType.length / MAX_COLUMN) })
                                                    .append(target);
}

// 部位表示
var displayParts = function () {
    // 部位初期化
    $(".S-SA-06Right1-2 > li").each(function () {
        initParts(this);
    });
}

// 部位の初期化
var initParts = function (target) {
    if ($(target).hasClass("Half")) {
        $(target).removeClass("Half");
    }
    $(target).children().remove();
    $(target).css("background", "");
}

// グラデーション設定
var makeGradient = function (damageType) {
    var bgStyle = "";
    $(tblDamageType).each(function () {
        if (this.type == damageType) {
            bgStyle = "-webkit-gradient(linear, left top, left bottom, from(" + this.from + "), to(" + this.to + "))";
        }
    })

    return bgStyle;
}

// ダメージセット
var setPartsDamage = function (target, options) {
    // デフォルト値を構築
    var defaults = {
        damage: "",
        class: ""
    };
    // 引数とデフォルト値を比較し、変数[setting]へ格納する
    var setting = $.extend(defaults, options);

    // アンカータグ生成
    var tmpContens = $('<a>' + setting.damage + '</a>');

    // グラデーション設定
    var bgStyle = makeGradient(setting.damage);
    if ("" != setting.class) {
        tmpContens.addClass(setting.class);
        tmpContens.css("background", bgStyle);
    } else {
        $(target).css("background", bgStyle);
    }

    tmpContens.appendTo($(target));
};

// カメラアイコン追加
var setPartsCamera = function (target) {
    $(target).append($("<div class='CameraIcn'></div>"));
}

// チェックボックスON/OFF表示
var setCheckBox = function (target, checked) {
    if (checked) {
        $(target).addClass("KSCheckInjuryBoxChecked");
    } else {
        $(target).removeClass("KSCheckInjuryBoxChecked");
    }
};

// 全データ更新
var requestDataAll = function () {
    // データ更新
    $.ajax({
        type: "POST",
        dataType: "json",
        url: 'SC3160218.aspx/GetDamageInfo',
        data: "{id: '" + C_ExteriorId + "' }",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                gDamageData = $.parseJSON(result.d);
                updateDataAll(gDamageData);
            }
        },
        error: function () {
            $("#debug").text("[error]requestDataAll");
        },
        async: false
    });
};

// 部位表示更新処理
var updateParts = function (damageData) {
    // 部位初期化
    $(".S-SA-06Right1-2 > li").each(function () {
        initParts(this);
    });
    // 部位更新
    for (var i in damageData.data) {
        var parts = damageData.data[i];
        var name = getPartsNameById(parts.PARTS_TYPE);
        if (name) {
            name = "#" + name;
            var damage1Enable = (parts.DAMAGE_TYPE_1.length > 0) ? true : false;
            var damage2Enable = (parts.DAMAGE_TYPE_2.length > 0) ? true : false;
            // 損傷表示
            if (damage1Enable && !damage2Enable) {
                // 損傷1個
                setPartsDamage(name, { damage: parts.DAMAGE_TYPE_1 });
            } else if (damage1Enable && damage2Enable) {
                // 損傷2個
                $(name).addClass("Half");
                setPartsDamage(name, { damage: parts.DAMAGE_TYPE_1, class: "UpA" });
                setPartsDamage(name, { damage: parts.DAMAGE_TYPE_2, class: "DwA" });
            }
            // カメラアイコン追加
            if (0 <= parts.RO_THUMBNAIL_ID) {
                setPartsCamera(name);
            }
        }
    }
}

// 全画面表示更新処理
var updateDataAll = function (damageData) {
    // NoDamageチェックボックス
    if (damageData.NO_DAMAGE_FLG != null) {
        setCheckBox("#chkNoDamage", (damageData.NO_DAMAGE_FLG == '1') ? true : false);
    }
    // NoDamageチェックボックス
    if (damageData.CANNOT_CHECK_FLG != null) {
        setCheckBox("#chkCanNotCheck", (damageData.CANNOT_CHECK_FLG == '1') ? true : false);
    }
    // 部位更新
    updateParts(damageData);
};

// 部位番号から部位名を取得する
var getPartsNameById = function (id) {
    for (var i in tblPartsCtrlTable) {
        if (tblPartsCtrlTable[i].id == id) {
            return tblPartsCtrlTable[i].name;
        }
    }
    return null;
};

// 傷種別マスタテーブルからJSONデータに変換
var createExplanation = function () {
    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: 'SC3160218.aspx/getExplanation',
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                tblDamageType = [];
                // json配列生成
                var explanationList = $.parseJSON(result.d);
                for (var i in explanationList) {
                    var dat = {};
                    dat['type'] = explanationList[i].type;
                    dat['from'] = explanationList[i].from;
                    dat['to'] = explanationList[i].to;
                    dat['title'] = explanationList[i].title;
                    tblDamageType.push(dat);
                }
                displayExplanation();
            }
        },
        error: function () {
        },
        async: false
    });
};

// Getパラメータリスト作成
var getUrlVars = function () {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
};

// 部位ボタン押下イベント処理
var clickPartsButton = function () {
    // Editモード且つNoDamageチェックボックスONの場合イベントを無視する
    if (("0" == gUrlParamList["ViewMode"]) && ($("#chkNoDamage").hasClass("KSCheckInjuryBoxChecked"))) {
        return;
    }
    for (var e in tblPartsCtrlTable) {
        if (tblPartsCtrlTable[e]["name"] == this.id) {
            // 損傷登録画面表示用URLスキーム生成
            var strUrl = window.location.href;
            var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3160219.aspx";
            baseUrl = baseUrl + "?ViewMode=" + gUrlParamList["ViewMode"];
            baseUrl = baseUrl + "&ExteriorId=" + C_ExteriorId;
            baseUrl = baseUrl + "&PartsNo=" + tblPartsCtrlTable[e]["id"];
            baseUrl = baseUrl + "&LoginUserID=" + gUrlParamList["LoginUserID"];
            var callbackStr = "::callback=var tmp=function(msg){$('iframe').each(function(){$(this).contents().find('iframe').each(arguments.callee);if($(this).contents().find('body#SC3160218').length>0){this.contentWindow.cbExteriorCheck(msg);}});};tmp";

            // 損傷登録画面表示位置取得
            var frame1;
            var frame2;
            $('iframe', parent.parent.document).each(function (e) {
                var tempFrame1 = $(this);
                $(this).contents().find('iframe').each(function (e2) {
                    var tempFrame2 = $(this);
                    if ($(this).contents().find('body#SC3160218').length > 0) {
                        frame1 = tempFrame1;
                        frame2 = tempFrame2;
                    }
                });
            });

            var frameLeft;
            if (frame1) {
                frameLeft = frame1.position().left;
            } else {
                frameLeft = 0;
            }
            var frameLeft2
            if (frame2) {
                frameLeft2 = frame2.position().left;
            } else {
                frameLeft2 = 0;
            }
            var buttonPosition = $(this).position();
            var zoomrate = $("#SC3160218").css("zoom");

            var buttonPositionLeft = 0;
            var positionLeft = 0;
            buttonPositionLeft = buttonPosition.left * zoomrate;
            positionLeft = frameLeft + frameLeft2 + buttonPositionLeft;
            if (zoomrate == 1) {
                positionLeft = positionLeft - 300 - (50 * zoomrate);
            } else {
                positionLeft = positionLeft - 300 - (60 * zoomrate);
            }
            if (positionLeft <= 0) {
                if (zoomrate == 1) {
                    positionLeft = frameLeft + frameLeft2 + buttonPositionLeft + (50 * zoomrate);
                } else {
                    positionLeft = frameLeft + frameLeft2 + buttonPositionLeft + (40 * zoomrate);
                }
            }
            var urlScheme = "icrop:titlebarPopup?url=" + baseUrl + "::x=" + Math.floor(positionLeft) + "::y=100::w=300::h=400" + callbackStr;
            // URLスキーム発行
            window.location.href = urlScheme;
            $("#debug").text(urlScheme);
            return;
        }
    }
};

// MoDamageチェックボックス押下イベント
var clickNoDamageCheckBox = function () {
    // Readonly指定時は何もしない
    if ("1" == gUrlParamList["ViewMode"]) {
        return;
    }

    var check = !$("#chkNoDamage").hasClass("KSCheckInjuryBoxChecked");

    // NoDamageフラグをONにする場合は損傷データ削除確認メッセージを表示する
    if (check) {
        // 損傷データが１つでもある場合
        if (0 < gDamageData.data.length) {
            if (!window.confirm(C_ConfirmMsg)) {
                // 損傷データ削除の同意が取れなかった場合
                return;
            }
        }
    }

    // データ更新
    $.ajax({
        type: "POST",
        url: 'SC3160218.aspx/SetNoDamage',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{id: '" + C_ExteriorId + "', value: '" + check + "', userId: '" + gUrlParamList["LoginUserID"] + "' } ",
        dataType: "json",
        success: function () {
            $("#debug").text("SetNoDamage complate");
            // 表示更新
            if (check && (0 < gDamageData.data.length)) {
                requestDataAll();
            } else {
                setCheckBox("#chkNoDamage", check);
            }
        },
        error: function () {
            $("#debug").text("SetNoDamage error");
        }
    });
};

// Can't check チェックボックス押下イベント
var clickCanNotCheckBox = function () {
    // Readonly指定時は何もしない
    if ("1" == gUrlParamList["ViewMode"]) {
        return;
    }

    var check = !$("#chkCanNotCheck").hasClass("KSCheckInjuryBoxChecked");
    // データ更新
    $.ajax({
        type: "POST",
        url: 'SC3160218.aspx/SetCanNotCheck',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{id: '" + C_ExteriorId + "', value: '" + check + "', userId: '" + gUrlParamList["LoginUserID"] + "' } ",
        dataType: "json",
        success: function () {
            $("#debug").text("SetCanNotCheck complate");
            // 表示更新
            setCheckBox("#chkCanNotCheck", check);
        },
        error: function () {
            $("#debug").text("SetCanNotCheck error");
        }
    });
};

// DamageInputからのコールバック呼び出し
var cbExteriorCheck = function (msg) {
    // 全更新
    requestDataAll();
    $("#debug").text("cbExteriorCheck callback");
};

// READYイベント
var readySC3160218 = function () {
    // Getパラメータリスト作成
    gUrlParamList = getUrlVars();

    // 凡例テーブル生成
    createExplanation();

    // 部位タップイベント
    $(".S-SA-06Right1-2 > li").click(clickPartsButton);

    // [NoDamageチェックボックス]クリックイベント
    $("p.KSCheckInjuryBox").click(clickNoDamageCheckBox);

    // [Can'tCheckチェックボックス]クリックイベント
    $("p.KSCheckInjuryBox2").click(clickCanNotCheckBox);

    // 部位初期表示
    displayParts();

    // 凡例表示フラグ
    if ("1" == gUrlParamList["LegendDisp"]) {
        $("div > .KSMainblockCheckIconContents").css({ display: "none" });
    }
    // チェックボックス表示フラグ
    if ("1" == gUrlParamList["CheckboxDisp"]) {
        $("div > .KSMainblockCheckExplodedViewBox").css({ display: "none" });
    }
    // スケールモード
    if ("1" == gUrlParamList["ScaleMode"]) {
        window.document.body.style.zoom = C_ZOOM_RATE;
    }

    // 全更新
    requestDataAll();
};
