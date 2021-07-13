var _ua = navigator.userAgent;
var isMobile = !!_ua.match(/AppleWebKit.*Mobile.*/) || !!_ua.match(/iPhone/) || !!_ua.match(/IPod/);
var clientHeight = $(window).height();
var iosKeyHeight = 44;
var keyboardAreaHeight = 344;

var CurrentKeyboardAreaHeight = 344;

var imeAttribute = "tdisIme";
var CancelAttribute = "targetCancelButton";
var vinTxtAttribute = "vinNoReInput"
var oldVinTxtAttribute = "usedVinNoReInput"
var skipFocusKey = "SkipFocusOn";
var isTouchSpeedDownAttribute = "isTouchSpeedDown";
var isTouchSpeedUpAttribute = "isTouchSpeedUp";
var isInputItemAttribute = "isInputItem";
var onKeyBoardCount = 0;

var clientYBefore = 0;
var useClientYBefore = false;

var isBeOpened = false;

// キーボード列挙
var OriginalKeyBoardEnum = {
    None: 0,
    NumberKey: 1,
    AlphabetKey: 2,
    InterpunctionKey: 4,
    ZoneKey : 256,
    AreaKey : 512
}
var defaultKeyBoardEnumValue = OriginalKeyBoardEnum.NumberKey + OriginalKeyBoardEnum.AlphabetKey + OriginalKeyBoardEnum.InterpunctionKey;
var tdisImeValue;

function ImeModel(_value, _name) {
    this.name = _name;
    this.value = _value;
}

// キーボード宣言
var imeList = new Array(
    new ImeModel(1, "NumberKey"),
    new ImeModel(2, "AlphabetKey"),
    new ImeModel(4, "InterpunctionKey"),
    new ImeModel(256, "ZoneKey"),
    new ImeModel(512, "AreaKey")
)

var targetTextBox;
var callBackOkMethod;
var callBackCancelMethod;
var targetCancelButton;
var targetDivId = "OriginalKeyBoard";
var _keyBoardOnClickValue;

function AssociateButton(txtBoxId, btnId) {
    if (document.getElementById(txtBoxId) && document.getElementById(btnId)) {
        $("#" + txtBoxId).attr(CancelAttribute, btnId);
    }
}

function switchIme(imeEnum) {
    var _divArray = new Array();
    $("td.auto-style2").hide();
    $("td.auto-style2").find("div").remove();
    $("td.auto-style2").append("<div class='divPageItem'></div>");

    $.each(imeList, function (n, ime) {
        if ((imeEnum & ime.value) == ime.value) {
            _divArray = _divArray.concat(switchImeByName(ime.name));
        }
    });

    $.each(_divArray, function (n, div) {
        $(div).html(resetPageItem(n,_divArray.length));
    });
}

function switchImeByName(name) {
    $('td[id*="' + name + '"]').show();
    return $('td[id*="' + name + '"]').find("div").toArray();
}

function resetPageItem(index, count) {
    var _html = "";
    for (var i = 0; i < count; i++) {
        _html += createPageItem(index == i);
    }
    return _html;
}

function createPageItem(isCurrent) {
    var _class = "txtPageItem";
    if (isCurrent) {
        _class = "txtCurrentPageItem";
    }
    return "<input name=\"ctl00$keyboardControl$OriginalKeyBoard$ctl84\" type=\"text\" readonly=\"readonly\" class=\"" + _class + "\" />";
}

function OnKeyBoardClick(value) {
    isBeOpened = true;

    if (value == 'BS') {
        if (targetTextBox.BsAttribute) {
            var _bsFunc = targetTextBox.BsAttribute;
            _bsFunc();
        } else {
            targetTextBox.value = targetTextBox.value.slice(0, -1);
        }
    } else if (value == 'AC') {
        if (targetTextBox.AcAttribute) {
            var _acFunc = targetTextBox.AcAttribute;
            _acFunc();
        } else {
            targetTextBox.value = targetTextBox.value = "";
        }
    } else {
        if ((targetTextBox.value + value).length <= targetTextBox.maxLength) {
            targetTextBox.value = targetTextBox.value + value;

//            if (targetTextBox.id.split("_")[1] == C_ID_ALP) {
//                ChangeFocus();
//            }
        }
    }
    if (targetTextBox.onchange) {
        var _changeIt = targetTextBox.onchange;
        _changeIt();
    }
}

function addHandler(id) {
    var keyBtn = $("#" + id)[0];
    $("#" + id).bind("touchstart", function (e) {
        _keyBoardOnClickValue = keyBtn.value;
        // 高速対応
        //if ($(targetTextBox).attr(isTouchSpeedUpAttribute)) {
        //    OnKeyBoardClick(keyBtn.value);
        //    e.preventDefault();
        //}
        // 低速対応
        if (!$(targetTextBox).attr(isTouchSpeedDownAttribute)) {
            OnKeyBoardClick(keyBtn.value);
            e.preventDefault();
        }
    });

    $("#" + id).bind("touchmove", function (e) {
        _keyBoardOnClickValue = "";
    });

    $("#" + id).bind("touchend", function (e) {
        // 高速対応
        //if (!$(targetTextBox).attr(isTouchSpeedUpAttribute)) {
        //    if (_keyBoardOnClickValue == keyBtn.value) {
        //        OnKeyBoardClick(keyBtn.value);
        //    } 
        //}
        // 低速対応
        if ($(targetTextBox).attr(isTouchSpeedDownAttribute)) {
            if (_keyBoardOnClickValue == keyBtn.value) {
                OnKeyBoardClick(keyBtn.value);
            }
        }

        _keyBoardOnClickValue = "";
        e.preventDefault();
    });

    if (!isMobile) {
        // ここからがマウス対応
        $("#" + id).bind("mousedown", function (e) {
            _keyBoardOnClickValue = keyBtn.value;
        });

        $("#" + id).bind("mouseup", function (e) {
            if (_keyBoardOnClickValue == keyBtn.value) {
                OnKeyBoardClick(keyBtn.value);
            }
            _keyBoardOnClickValue = "";
            //TMEJ TEI START
            //targetTextBox.focus();
            //TMEJ TEI END
        });

        // ここまで
    }
}

function SetImeByTextboxId(id, imeKey) {
    $("input#" + id).attr(imeAttribute, imeKey);
}

function SetImeForAllTextbox(imeKey) {
    $(":text").not(".txtPageItem").not(".txtCurrentPageItem").attr(imeAttribute, imeKey);
}

function resizeKeyBoard() {
    if ($("#keyboardDiv").css("visibility") == "visible") {
        $("#keyboardDiv").css("position", "fixed");
        $("#keyboardDiv").css("top", clientHeight - CurrentKeyboardAreaHeight);
    }
}

function ShowKeyBoard(e) {
        
        isBeOpened = true;
        setTimeout(function () {
                var _changedValue = 0;
                var _scrolledValue = 0;
                var workClientY = e.target.getBoundingClientRect().top + e.target.getBoundingClientRect().height + 1;
                var workClientYToBottom = clientHeight - workClientY - 25;
                if (useClientYBefore) {
                    $(window).scrollTop(clientYBefore);
                    useClientYBefore = false;
                } else if (workClientYToBottom <= CurrentKeyboardAreaHeight) {
                    _scrolledValue = $(window).scrollTop() + CurrentKeyboardAreaHeight - workClientYToBottom;
                    $(window).scrollTop(_scrolledValue);
                } 

                clientYBefore = $(window).scrollTop();
                $("#keyboardDiv").css("visibility", "visible");
                var topValue = clientHeight + $(window).scrollTop() - CurrentKeyboardAreaHeight;
                if (CurrentKeyboardAreaHeight == keyboardAreaHeight) {
                    $("#keyboardDiv").css("position", "absolute");
                } else {
                    topValue = clientHeight - CurrentKeyboardAreaHeight;
                }
                
                $("#keyboardDiv").css("top", topValue);
                $("#keyboardDiv").show();

        }, 100);
}

function HideKeyBoard() {

    var _result = null;
    isBeOpened = false;
    $(targetTextBox).trigger("blur");
    $("#keyboardDiv").css("top", clientHeight);
    $("#keyboardDiv").hide();
    $("#keyboardBgDiv").hide();
    if (targetTextBox) {
        _result = targetTextBox;
        targetTextBox.classList.remove('txtCommon');
    }
	targetTextBox.defaultValue = targetTextBox.value;
    targetTextBox = null;

    onKeyBoardCount = 0;
    
	$("#OriginalKeyBoard").css("visibility", "hidden");
	
	callBackOkMethod();
	callBackOkMethod = null;

    return _result;
}

function HideKeyBoard2()
{
	var element = window.document.getElementById("OriginalKeyBoard");
	element.innerHTML = "";

	$("#OriginalKeyBoard").css("visibility", "hidden");
	
	callBackCancelMethod();
	callBackCancelMethod = null;
}

function TextboxFocusOn(id) {
    if (!getUrlVars()[skipFocusKey]) {
        var _target = document.getElementById(id);
        if (_target) {
            targetTextBox = _target;
            setTimeout(function () {
                if (targetTextBox != null) {
                    //$("#keyboardBgDiv").show();
                    $(targetTextBox).trigger("focus");
                }
            },200);
        }
    }
}

function CancelClick() {
	targetTextBox.value = targetTextBox.defaultValue;
	targetTextBox.defaultValue = targetTextBox.value;

	var _result = null;
    isBeOpened = false;
    $(targetTextBox).trigger("blur");
	if (targetTextBox) {
        _result = targetTextBox;
        targetTextBox.classList.remove('txtCommon');
    }

    targetTextBox = null;
    HideKeyBoard2();
}

function enterCheck() {
    if (event.keyCode == 13) {
        CancelClick();
        return true;
    }
    return false;
}

function ClearAndBeginInput(e) {
    if (e.keyCode == 13) {
        if ($(e.target).attr(oldVinTxtAttribute)) {
            $(e.target).attr(vinTxtAttribute, "yes");
        }
    }
    // 英数字の場合、前のテキストボックスをクリアしてから入力する
    if ((e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)) {
        if ($(e.target).attr(vinTxtAttribute)) {
            $(e.target).removeAttr(vinTxtAttribute);
            $(e.target).val("");
        }
    }
}

function customizeBS(textboxID, func) {
    if (document.getElementById(textboxID)) {
        document.getElementById(textboxID).BsAttribute = func;
    }
}

function customizeAC(textboxID, func) {
    if (document.getElementById(textboxID)) {
        document.getElementById(textboxID).AcAttribute = func;
    }
}

function ShowKeyBoard2(targetTextBox, keyboardType)
{
	$("#OriginalKeyBoard").css("visibility", "visible");
	var element = window.document.getElementById(targetDivId);
	element.innerHTML = GetCalculator(keyboardType);
}

function GetCalculator(keyboardType)
{
	var innerHtml;
	
	if(keyboardType == "0")
	{
		innerHtml = "<div id='keyboardDiv' style='left:20px;height:527px;width:640px;position:fixed;border:none;background-color:#FFF'><table  id='table111' class='tableBody' style='border-collapse:collapse;'><tr><td  id='tdCancel' class='tdBackW' style='border:none; text-align: left;padding: 20px 0px 0px 6px;'><button id='CancelBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#FFFFFF), to(#F1F1F1));color:#507CD1;' onclick='CancelClick();return false;'>Cancel</button></td><td id='tdOK' class='tdBackW' style='border:none; text-align: right;padding: 20px 48px 0px 0px;vertical-align:middle' ><button id='OKBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#507CD1), to(#184499));color:#FFF;' onclick='HideKeyBoard();return false;'>OK</button></td></tr><tr><td class='tdBackW' colspan='2'><div id='keyAreaDiv' style='left:0px;position:absolute;z-index:90;height:450px;' class='tdBackW' ><div id='Div1' style='OVERFLOW: hidden; width:640px; height: 450px;'><table><tr id='keyboardCreativeArea'><td id='NumberKey0' class='auto-style2'><table style='width:580px;height:200px;'><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='bc8ed188-6c2c-4922-92f6-c62fa411dd6d' class='btnCalculator' type='button' tabindex='-1' value='1'>1</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='74356867-f82b-464a-9222-2b2c5d2e98f0' class='btnCalculator' type='button' tabindex='-1' value='2'>2</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='d4c48d91-1688-4591-a5eb-a49b5ecdfc0e' class='btnCalculator' type='button' tabindex='-1' value='3'>3</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='1d06ab31-f52e-4171-a3c2-158dc7d2c211' class='btnCalculator' type='button' tabindex='-1' value='4'>4</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='b57f43d9-0a76-4923-a6cd-1d9760ed5b32' class='btnCalculator' type='button' tabindex='-1' value='5'>5</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='157b8ebd-cbcf-409e-bb99-f2091df205b2' class='btnCalculator' type='button' tabindex='-1' value='6'>6</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='ecea0dca-68db-4125-99db-2a791d2b5d13' class='btnCalculator' type='button' tabindex='-1' value='7'>7</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='63971506-7efd-4ff9-be01-be966ce6fa7d' class='btnCalculator' type='button' tabindex='-1' value='8'>8</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='d91b1c2a-ddb1-46bb-ab3a-f8385c89324a' class='btnCalculator' type='button' tabindex='-1' value='9'>9</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='aabd7d89-be27-4169-9bed-ffc1a7f586e6' class='btnCalculatorBS' type='button' tabindex='-1' value='BS'>BS</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='3186bfdb-2dcc-439f-87bf-9b7af160d0bc' class='btnCalculator' type='button' tabindex='-1' value='0'>0</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='d2c0c7ed-8771-47dd-8082-4a7f6ee4524d' class='btnCalculatorAC' type='button' tabindex='-1' value='AC'>AC</button></td></tr></table></td></tr></table></div></div></td></tr></table></div><div id='keyboardBgDiv' style='left:0px;height:300px;width:580px;visibility:hidden;position:absolute;'><table  class='tableBody'><tr></tr></table></div>";
	}
	else
	{
		innerHtml = "<div id='keyboardDiv' style='left:20px;height:527px;width:640px;position:fixed;border:none;background-color:#FFF'><table  id='table111' class='tableBody' style='border-collapse:collapse;'><tr><td  id='tdCancel' class='tdBackW' style='border:none; text-align: left;padding: 20px 0px 0px 6px;'><button id='CancelBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#FFFFFF), to(#F1F1F1));color:#507CD1;' onclick='CancelClick();return false;'>Cancel</button></td><td id='tdOK' class='tdBackW' style='border:none; text-align: right;padding: 20px 48px 0px 0px;vertical-align:middle' ><button id='OKBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#507CD1), to(#184499));color:#FFF;' onclick='HideKeyBoard();return false;'>OK</button></td></tr><tr><td class='tdBackW' colspan='2'><div id='keyAreaDiv' style='left:0px;position:absolute;z-index:90;height:450px;' class='tdBackW' ><div id='Div1' style='OVERFLOW: hidden; width:640px; height: 450px;'><table><tr id='keyboardCreativeArea'><td id='AlphabetKey0' class='auto-style2'><table style='width:580px;height:200px;'><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='089f8073-2ad4-44eb-bbbb-fd8039096138' class='btnCalculator' type='button' tabindex='-1' value='D'>D</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='351de7be-a6ad-49e6-ab6b-6bfee2e90b49' class='btnCalculator' type='button' tabindex='-1' value='E'>E</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='9a166276-dc7b-4a33-ad53-fbaab4595c78' class='btnCalculator' type='button' tabindex='-1' value='Q'>Q</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='0227a724-c4ee-496f-b785-59949e5c061b' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='b1cf5593-a924-4647-93e1-255f6eb0990c' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='1f70282b-701e-4fdf-8934-82d51ffddc36' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='27321ba1-af05-46af-ab86-e0b9f2866bde' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='3be180d4-96ed-4813-ba81-85edd1d86e93' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='819daf3b-e4fe-4f68-9bc6-a202e635e637' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='3de2075b-cef5-452c-ae67-8874d72f3a7e' class='btnCalculatorBS' type='button' tabindex='-1' value='BS'>BS</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='23ef44a6-e1da-4cd6-a3ad-81e56adb4afa' class='btnCalculator' type='button' tabindex='-1' value=''>&nbsp;</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='e98c8864-93c8-4864-8413-4120736cf738' class='btnCalculatorAC' type='button' tabindex='-1' value='AC'>AC</button></td></tr></table></td></tr></table></div></div></td></tr></table></div><div id='keyboardBgDiv' style='left:0px;height:300px;width:580px;visibility:hidden;position:absolute;'><table  class='tableBody'><tr></tr></table></div>";
		//innerHtml = "<div id='keyboardDiv' style='left:20px;height:527px;width:640px;position:fixed;border:none;background-color:#FFF'><table  id='table111' class='tableBody' style='border-collapse:collapse;'><tr><td  id='tdCancel' class='tdBackW' style='border:none; text-align: left;padding: 20px 0px 0px 6px;'><button id='CancelBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#FFFFFF), to(#F1F1F1));color:#507CD1;' onclick='CancelClick();return false;'>Cancel</button></td><td id='tdOK' class='tdBackW' style='border:none; text-align: right;padding: 20px 48px 0px 0px;vertical-align:middle' ><button id='OKBtn' class='OkOrCancelButton' style='background: -webkit-gradient(linear, left top, left bottom, from(#507CD1), to(#184499));color:#FFF;' onclick='HideKeyBoard();return false;'>OK</button></td></tr><tr><td class='tdBackW' colspan='2'><div id='keyAreaDiv' style='left:0px;position:absolute;z-index:90;height:450px;' class='tdBackW' ><div id='Div1' style='OVERFLOW: hidden; width:640px; height: 450px;'><table><tr id='keyboardCreativeArea'><td id='AlphabetKey0' class='auto-style2'><table style='width:580px;height:200px;'><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='089f8073-2ad4-44eb-bbbb-fd8039096138' class='btnCalculator' type='button' tabindex='-1' value='A'>A</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='351de7be-a6ad-49e6-ab6b-6bfee2e90b49' class='btnCalculator' type='button' tabindex='-1' value='B'>B</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='9a166276-dc7b-4a33-ad53-fbaab4595c78' class='btnCalculator' type='button' tabindex='-1' value='C'>C</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='0227a724-c4ee-496f-b785-59949e5c061b' class='btnCalculator' type='button' tabindex='-1' value='D'>D</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='b1cf5593-a924-4647-93e1-255f6eb0990c' class='btnCalculator' type='button' tabindex='-1' value='E'>E</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='1f70282b-701e-4fdf-8934-82d51ffddc36' class='btnCalculator' type='button' tabindex='-1' value='F'>F</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='27321ba1-af05-46af-ab86-e0b9f2866bde' class='btnCalculator' type='button' tabindex='-1' value='G'>G</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='3be180d4-96ed-4813-ba81-85edd1d86e93' class='btnCalculator' type='button' tabindex='-1' value='H'>H</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='819daf3b-e4fe-4f68-9bc6-a202e635e637' class='btnCalculator' type='button' tabindex='-1' value='I'>I</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='3de2075b-cef5-452c-ae67-8874d72f3a7e' class='btnCalculatorBS' type='button' tabindex='-1' value='BS'>BS</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='23ef44a6-e1da-4cd6-a3ad-81e56adb4afa' class='btnCalculator' type='button' tabindex='-1' value='J'>J</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='e98c8864-93c8-4864-8413-4120736cf738' class='btnCalculatorAC' type='button' tabindex='-1' value='AC'>AC</button></td></tr></table></td><td id='AlphabetKey1' class='auto-style2'><table style='width:580px;height:300px;'><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='1d90924d-65a4-4876-9bf2-dfcef1c29c30' class='btnCalculator' type='button' tabindex='-1' value='K'>K</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='07fba498-1cc4-4ed1-b2fd-e208d456a0d4' class='btnCalculator' type='button' tabindex='-1' value='L'>L</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='a1ac4f02-c60a-436e-80c2-a996848a97d9' class='btnCalculator' type='button' tabindex='-1' value='M'>M</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='b7487dff-a54f-464f-a820-fcadaa6bd2ce' class='btnCalculator' type='button' tabindex='-1' value='N'>N</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='fa13981c-2266-43dc-afd5-43f20fac0b36' class='btnCalculator' type='button' tabindex='-1' value='O'>O</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='206329c2-174b-4bf0-bfeb-09a6659509c8' class='btnCalculator' type='button' tabindex='-1' value='P'>P</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='a3b30ac8-bbc4-4ac0-aade-531d56b781af' class='btnCalculator' type='button' tabindex='-1' value='Q'>Q</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='369b25af-6bcd-4e2a-8039-f1cfbf256ac2' class='btnCalculator' type='button' tabindex='-1' value='R'>R</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='99ff8e0b-c5b5-4824-872a-31ad6a62fc61' class='btnCalculator' type='button' tabindex='-1' value='S'>S</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='7d73bf9c-c641-426f-b7bc-fcad2ae7a672' class='btnCalculatorBS' type='button' tabindex='-1' value='BS'>BS</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='c1c031a4-da75-4b80-810a-b775f525a66f' class='btnCalculator' type='button' tabindex='-1' value='T'>T</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='4f413c0f-6743-4806-81bb-6003686001ae' class='btnCalculatorAC' type='button' tabindex='-1' value='AC'>AC</button></td></tr></table></td><td id='AlphabetKey2' class='auto-style2'><table style='width:580px;height:200px;'><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='e1380d8b-4856-4d30-840e-3cd290d89a5f' class='btnCalculator' type='button' tabindex='-1' value='U'>U</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='7a8484b1-7365-4254-9c73-2a1da5271b77' class='btnCalculator' type='button' tabindex='-1' value='V'>V</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='352ac6cf-4b40-4496-8e85-3685420c88ed' class='btnCalculator' type='button' tabindex='-1' value='W'>W</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='aa1f0547-0479-42b1-81a1-a0cfd8878cb2' class='btnCalculator' type='button' tabindex='-1' value='X'>X</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='319a5e7e-f886-4a13-a166-591cc94dbc8a' class='btnCalculator' type='button' tabindex='-1' value='Y'>Y</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='da8552b4-1b8e-46fd-9eac-2ba829b0dead' class='btnCalculator' type='button' tabindex='-1' value='Z'>Z</button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='261d2976-e56a-4da5-855f-269921cbc985' class='btnCalculator' type='button' tabindex='-1' value=&nbsp;></button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='4f2cf34b-cdb1-4158-b492-cd2c0a964396' class='btnCalculator' type='button' tabindex='-1' value=&nbsp;></button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='c32a5c9f-6cfb-4663-ae6a-6b8be4cf232f' class='btnCalculator' type='button' tabindex='-1' value=&nbsp;></button></td></tr><tr><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='aa94703f-5de2-4098-9c8e-589ec4782608' class='btnCalculatorBS' type='button' tabindex='-1' value='BS'>BS</button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='7686c767-0b2a-403c-8ddc-8085b46ed94f' class='btnCalculator' type='button' tabindex='-1' value=&nbsp;></button></td><td class='auto-style1' style='padding: 4px 3px 4px;'><button id='07c35231-8156-4cce-9fa1-c7f7c1b8e62a' class='btnCalculatorAC' type='button' tabindex='-1' value='AC'>AC</button></td></tr></table></td></tr></table></div></div></td></tr></table></div><div id='keyboardBgDiv' style='left:0px;height:300px;width:580px;visibility:hidden;position:absolute;'><table  class='tableBody'><tr></tr></table></div>";
	}
	return innerHtml;
}


var touched;
var keyboardstartX;
var keyboardstartY;

var keyboardType_NUM = "0";
var keyboardType_ALP = "1";

function ShowOriginalKeyBoard (targetControl, keyboardType, callBackOk, callBackCancel) {

	callBackOkMethod = callBackOk;
	callBackCancelMethod = callBackCancel;

    //TextBoxフォーカスイベント
    //iOSキーボードの抑制の為に実装
    $(targetControl).focus(function (e) {
        e.preventDefault();
        $(this).trigger("blur", ["self"]);
    });

    ShowKeyBoard2(targetControl, keyboardType);

    if (targetTextBox) {
        targetTextBox.classList.remove('txtCommon');
        var _nofocus = $.cookie('nofocus');
        if (_nofocus) {
            $(targetTextBox).trigger("blur");
        }
    }

    targetTextBox = targetControl;
    targetTextBox.classList.add('txtCommon');
    if ($(targetTextBox).attr("targetCancelButton")) {
        targetCancelButton = $("input[id*=" + $(targetTextBox).attr("targetCancelButton") + "]")[0];
    }

    if ($(targetTextBox).attr("tdisIme") && $(targetTextBox).attr("tdisIme") == OriginalKeyBoardEnum.None) {
        $("#keyboardDiv").css("top", clientHeight);
        $("#keyboardDiv").hide();
        $("#keyboardBgDiv").hide();
        return;
    }

//    $("#keyAreaDiv")[0].addEventListener("touchstart",
//	    function (e) {
//	        keyboardstartX = e.touches[0].clientX;
//	    }, false);
//
//    $("#keyAreaDiv")[0].addEventListener("touchmove",
//        function (e) {
//            preventScroll(e);
//        }, false);
//
//    $("#keyAreaDiv")[0].addEventListener("touchend",
//        function (e) {
//            var clientWidth = $("#keyAreaDiv")[0].offsetWidth;
//            var changedMoveX = e.changedTouches[0].clientX - keyboardstartX;
//            if (changedMoveX > 10) {
//                if ($("#keyAreaDiv")[0].offsetLeft + 318 <= 0) {
//                    $("#keyAreaDiv").css("left", $("#keyAreaDiv")[0].offsetLeft + 318);
//                }
//            } else if (changedMoveX < -10) {
//                if ($("#keyAreaDiv")[0].offsetLeft - 320 + clientWidth > 0) {
//                    $("#keyAreaDiv").css("left", $("#keyAreaDiv")[0].offsetLeft - 318);
//                }
//            }
//        }, false);
//
//    document.addEventListener("touchstart", onTouched, true);
//    document.addEventListener("touchmove", function () { touched = false; }, true);
//    document.addEventListener("touchend", checkIsCloseKeyBoard, true);

	AssociateKeyboard(targetTextBox);
	addHandlerInfo();
}

function AssociateKeyboard(targetControl) {

    $(targetControl).not(".txtPageItem").not(".txtCurrentPageItem").each(function () {
        if ($(this).attr(imeAttribute) == undefined) {
            $(this).attr(imeAttribute, defaultKeyBoardEnumValue);
        }
        if ($(this).attr(CancelAttribute) == undefined) {
            $(this).attr(CancelAttribute, "btnCompl");
        }
    });

    //TextBoxフォーカスイベント
    //iOSキーボードの抑制の為に実装
    $(targetControl).focus(function (e) {
        e.preventDefault();
        $(this).trigger("blur", ["self"]);
    });

	CurrentKeyboardAreaHeight = keyboardAreaHeight - iosKeyHeight;
	$("#keyboardDiv").css("height", CurrentKeyboardAreaHeight);

}

function onTouched(e) {
    touched = true;
    keyboardstartY = $(window).scrollTop();
}

function checkIsCloseKeyBoard(e) {
    if (touched == true) {
        if (e.target.type == "text") {
            if (!$(e.target).attr("tdisIme") || $(e.target).attr("tdisIme") != OriginalKeyBoardEnum.None) {
                $("#keyboardBgDiv").show();
                //preventScroll(e);
                //$(e.target).focus();
            }
        } else if (e.target && $(e.target).attr(isInputItemAttribute)) {
            $("#keyboardBgDiv").show();
        } else if (e.target == $("#CancelBtn")[0] ) {
            CancelClick();
            preventScroll(e);
        } else if (e.target == $("#OKBtn")[0] ) {
            HideKeyBoard2();
            preventScroll(e);
        } else {
            if (($(window).height() - e.changedTouches[0].clientY + 30) > CurrentKeyboardAreaHeight) {
                HideKeyBoard2();
            }
        }
    } else {
        if ($("#keyboardDiv").css("visibility") == "visible" && $("#keyboardDiv").css("display") != "none" && $("#keyboardDiv").css("position") != "fixed") {
            if (keyboardstartY != $(window).scrollTop()) {
                setTimeout(function () {
                    var _topValue = clientHeight + $(window).scrollTop() - keyboardAreaHeight;

                    $("#keyboardDiv").css("height", keyboardAreaHeight - iosKeyHeight);
                    $("#keyboardDiv").css("top", _topValue);
                }, 100);
            }
        }
    } 
    touched = false;
}

// スクロールを抑止する関数
function preventScroll(event) {
    // preventDefaultでブラウザ標準動作を抑止する。
    event.preventDefault();
}

function addHandlerInfo() {
addHandler('bc8ed188-6c2c-4922-92f6-c62fa411dd6d');
addHandler('74356867-f82b-464a-9222-2b2c5d2e98f0');
addHandler('d4c48d91-1688-4591-a5eb-a49b5ecdfc0e');
addHandler('1d06ab31-f52e-4171-a3c2-158dc7d2c211');
addHandler('b57f43d9-0a76-4923-a6cd-1d9760ed5b32');
addHandler('157b8ebd-cbcf-409e-bb99-f2091df205b2');
addHandler('ecea0dca-68db-4125-99db-2a791d2b5d13');
addHandler('63971506-7efd-4ff9-be01-be966ce6fa7d');
addHandler('d91b1c2a-ddb1-46bb-ab3a-f8385c89324a');
addHandler('aabd7d89-be27-4169-9bed-ffc1a7f586e6');
addHandler('3186bfdb-2dcc-439f-87bf-9b7af160d0bc');
addHandler('d2c0c7ed-8771-47dd-8082-4a7f6ee4524d');

addHandler('089f8073-2ad4-44eb-bbbb-fd8039096138');
addHandler('351de7be-a6ad-49e6-ab6b-6bfee2e90b49');
addHandler('9a166276-dc7b-4a33-ad53-fbaab4595c78');
addHandler('0227a724-c4ee-496f-b785-59949e5c061b');
addHandler('b1cf5593-a924-4647-93e1-255f6eb0990c');
addHandler('1f70282b-701e-4fdf-8934-82d51ffddc36');
addHandler('27321ba1-af05-46af-ab86-e0b9f2866bde');
addHandler('3be180d4-96ed-4813-ba81-85edd1d86e93');
addHandler('819daf3b-e4fe-4f68-9bc6-a202e635e637');
addHandler('3de2075b-cef5-452c-ae67-8874d72f3a7e');
addHandler('23ef44a6-e1da-4cd6-a3ad-81e56adb4afa');
addHandler('e98c8864-93c8-4864-8413-4120736cf738');
addHandler('1d90924d-65a4-4876-9bf2-dfcef1c29c30');
addHandler('07fba498-1cc4-4ed1-b2fd-e208d456a0d4');
addHandler('a1ac4f02-c60a-436e-80c2-a996848a97d9');
addHandler('b7487dff-a54f-464f-a820-fcadaa6bd2ce');
addHandler('fa13981c-2266-43dc-afd5-43f20fac0b36');
addHandler('206329c2-174b-4bf0-bfeb-09a6659509c8');
addHandler('a3b30ac8-bbc4-4ac0-aade-531d56b781af');
addHandler('369b25af-6bcd-4e2a-8039-f1cfbf256ac2');
addHandler('99ff8e0b-c5b5-4824-872a-31ad6a62fc61');
addHandler('7d73bf9c-c641-426f-b7bc-fcad2ae7a672');
addHandler('c1c031a4-da75-4b80-810a-b775f525a66f');
addHandler('4f413c0f-6743-4806-81bb-6003686001ae');
addHandler('e1380d8b-4856-4d30-840e-3cd290d89a5f');
addHandler('7a8484b1-7365-4254-9c73-2a1da5271b77');
addHandler('352ac6cf-4b40-4496-8e85-3685420c88ed');
addHandler('aa1f0547-0479-42b1-81a1-a0cfd8878cb2');
addHandler('319a5e7e-f886-4a13-a166-591cc94dbc8a');
addHandler('da8552b4-1b8e-46fd-9eac-2ba829b0dead');
addHandler('aa94703f-5de2-4098-9c8e-589ec4782608');
addHandler('07c35231-8156-4cce-9fa1-c7f7c1b8e62a');
}