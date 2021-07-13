/* 
* ToDo�ꗗ
* �쐬�F 2012/02/01 TCS �|��
* �X�V�F 2012/03/13 TCS �n� $01 SalesStep2���[�U�[�e�X�g�ۑ�No.15�A18�A36
* �X�V�F 2012/04/23 TCS �_�{ $02 �����ۑ�No.123
* �X�V�F 2012/05/29 TCS �_�{ �N���N���Ή� 
* �X�V�F 2013/01/11 TCS ���{ �yA.STEP2�z������e-CRB �V�ԃ^�u���b�g�V���[���[���Ǘ��@�\�J��
* �X�V�F 2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J��
* �X�V�F 2014/11/04 TCS ���� iOS8 �Ή�(i-CROP_V4_sales���}�[�W) 
* �X�V�F 2020/02/17 TS ����(��) TR-SLT-TMT-20200218-001
*/
var sc3010401Script = function () {
    var constants = {
        open: 1,
        close: 0,
        sortType: { carName: 1, status: 2, cract: 3 },
        sortOrder: { asc: 1, desc: 2 }
    }

    var beforeChangeCriteria = {};
    var status = constants.close;

    //���������G���A���J��
    function openCriteria() {
        $(".WindDown").css("display", "none");
        //�ύX�O�̒��o������ۑ�
        saveCriteria();
        status = constants.open;
        //2020/02/17 TS ����(��) TR-SLT-TMT-20200218-001�Ή� START
        $("#CheckBoxArea").css("display", "block");
        //2020/02/17 TS ����(��) TR-SLT-TMT-20200218-001�Ή� END
        $("#SetIcons").addClass("SetIconsHeightL");
        //2013/01/11 TCS ���{ �yA.STEP2�zAdd Start
        $(".WindUp").css("display", "block");
        //2013/01/11 TCS ���{ �yA.STEP2�zAdd End
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
        //���������G���A�̍s�����ςɂ���
        $("#SetIcons").css("height", 120 + (50 * (Math.ceil(3 + $(".AfterCriteria").length) / 8)) + "px");
        $(".WindUp").css("top", 100 + (50 * (Math.ceil(3 + $(".AfterCriteria").length) / 8)) + "px");
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END
        return false;
    }

    //���������G���A�����
    function closeCriteria(isOutOfArea) {

        //2013/01/11 TCS ���{ �yA.STEP2�zAdd Start
        $(".WindUp").css("display", "none");
        //2020/02/17 TS ����(��) TR-SLT-TMT-20200218-001�Ή� START
        $("#CheckBoxArea").css("display", "none");
        //2020/02/17 TS ����(��) TR-SLT-TMT-20200218-001�Ή� END
        //2013/01/11 TCS ���{ �yA.STEP2�zAdd End
        $("#SetIcons").removeClass("SetIconsHeightL");
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
        //���������G���A�̍s�������ɖ߂�
        $("#SetIcons").css("height", "51px");
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END
        if (isOutOfArea && status == constants.open) {
            resetCriteria();
        }
        status = constants.close;
        $(".WindDown").css("display", "block");
        return false;
    }

    //���������ۑ�
    function saveCriteria() {
        //����������ۑ�
        $.extend(beforeChangeCriteria, getCriteria());
    }

    //�����������擾����
    function getCriteria() {
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
        var AfterOdrProc = [];
        $(".icrop-CustomCheckBox>:checkbox.AfterCriteria").each(function () {
            AfterOdrProc.push($(this).CustomCheckBox("value"));
        });
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END
        return {
            //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
            toDoSearchType: $("#ToDoSegmentedButton input:checked").val(),
            toDoSearchText: $("#ToDoSearchTextBox").val(),
            //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END
            isCheckDelay: $("#checkDelay").CustomCheckBox("value"),
            isCheckDue: $("#checkDue").CustomCheckBox("value"),
            isCheckFuture: $("#checkFuture").CustomCheckBox("value"),
            isCheckCold: $("#checkCold").CustomCheckBox("value"),
            isCheckWarm: $("#checkWarm").CustomCheckBox("value"),
            isCheckHot: $("#checkHot").CustomCheckBox("value"),
            //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
            isCheckAfter: AfterOdrProc,
            //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END
            sortType: $("#sortTypeHidden").val(),
            sortOrder: $("#sortOrderHidden").val()

        }
    }

    //�������������ɖ߂�
    function resetCriteria() {
        //�������������ׂ�ON�ɂ���

        $(".icrop-CustomCheckBox>:checkbox").each(function () {
            $(this).CustomCheckBox("value", true);
        });
        //�O�񌟍������𔽉f����
        $("#checkDelay").CustomCheckBox("value", beforeChangeCriteria.isCheckDelay);
        $("#checkDue").CustomCheckBox("value", beforeChangeCriteria.isCheckDue);
        $("#checkFuture").CustomCheckBox("value", beforeChangeCriteria.isCheckFuture);
        $("#checkCold").CustomCheckBox("value", beforeChangeCriteria.isCheckCold);
        $("#checkWarm").CustomCheckBox("value", beforeChangeCriteria.isCheckWarm);
        $("#checkHot").CustomCheckBox("value", beforeChangeCriteria.isCheckHot);
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
        for (i = 0; i < $(".AfterCriteria").length; i++) {
            var j = 0;
            $(".icrop-CustomCheckBox>:checkbox.AfterCriteria").each(function () {
                if (i == j) {
                    $(this).CustomCheckBox("value", beforeChangeCriteria.isCheckAfter[i]);
                }
                j++;
            });
        }
        //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END

    }

    //TODO�ꗗ�Ŗ��בI��
    function selectCustomer(cstkindHidden, customerclassHidden, crcustidHidden, fllwupboxseqHidden, strcdHidden, columeId) {
        //2012/05/29 TCS �_�{ �N���N���Ή� START
        //        displayOverlay();
        //2012/05/29 TCS �_�{ �N���N���Ή� End
        var $divlist = $("#" + columeId + "").children("td");
        for (i = 0; i < $divlist.length; i++) {
            $divlist[i].className = $divlist[i].className + " ColorBlue";
        }

        $("#cstkindHidden").val(cstkindHidden);
        $("#customerclassHidden").val(customerclassHidden);
        $("#crcustidHidden").val(crcustidHidden);
        $("#fllwupboxseqHidden").val(fllwupboxseqHidden);
        $("#strcdHidden").val(strcdHidden);



        //2012/05/29 TCS �_�{ �N���N���Ή� START
        commonRefreshTimer(function () {
            $("#refreshButton").click();
            //return true;
        });
        displayOverlay();
        $("#nextButton").click();
        //2012/05/29 TCS �_�{ �N���N���Ή� End
    }

    //�I�[�o�[���C�\��
    function displayOverlay() {

        //�I�[�o�[���C�\��
        $("#serverProcessOverlayBlack").css("display", "block");
        //�A�j���[�V����(���[�h��)
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);


    }

    //�I�[�o�[���C�I��
    function closeOverlay() {
        $("#serverProcessIcon").removeClass("show");
        //2014/11/04 TCS ���� iOS8 �Ή�(i-CROP_V4_sales���}�[�W)  Start
        $("#serverProcessOverlayBlack").removeClass("open");
        setTimeout(function () {
            $("#serverProcessOverlayBlack").css("display", "none");
        }, 300);
        //2014/11/04 TCS ���� iOS8 �Ή�(i-CROP_V4_sales���}�[�W)  End
    }

    function escapeHTML(text) {
        return $("<span>").text(text).html();
    }

    return {
        constants: constants,
        openCriteria: openCriteria,
        closeCriteria: closeCriteria,
        getCriteria: getCriteria,
        displayOverlay: displayOverlay,
        closeOverlay: closeOverlay,
        saveCriteria: saveCriteria,
        selectCustomer: selectCustomer,
        escapeHTML: escapeHTML
    }
} ();

    //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
    //�ڋq�����p���W�I�{�^���ύX��
    var g_IniLoad = false;
    function ToDoSearchTypeSegmenteButton_select(value) {
        if (value == '001') {
    		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordNameHidden").val();
      	}
      	else if (value == '002') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordTelHidden").val();
      	}
      	else if (value == '003') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordSocialIDHidden").val();
      	}
      	else if (value == '004') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordBookingNoHidden").val();
      	}
      	else if (value == '005') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordVinHidden").val();
      	}

      	if (g_IniLoad) {
      		$("#FocusinDummyButton").click();
      	} else {
      		g_IniLoad = true;
      	}
    }

    //�����������̓e�L�X�g�{�b�N�X�I����
    function FocusInToDoSearchTextBox(text) {
        if (text == "dummy") {
            $("#ToDoSearchTextBox").focus();
        }
      	custSearchfouusFlg = true;
   		$("#ToDoSearchTextBox").CustomTextBox("showClearButton");
    }

    //�����������̓e�L�X�g�{�b�N�X���͎�
    function InputInToDoSearchTextBox() {
        //Enter�������̂݁A�����������s
        if (event.keyCode == 13) {
            $("#ToDoSearchTextBox").blur();
            $("#AddIconRight").click();
        }
    }
    //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END

$(function () {
    //��i�`�F�b�N�{�b�N�X�쐬
    $(".icrop-CustomCheckBox>:checkbox" + ".dateCriteria").CustomCheckBox(
	{ "check": function (value) {
	    if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".dateCriteria").length == 0 && value == false) {
	        $(this).CustomCheckBox("value", true);
	    }
	}
	});

    //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START DEL
   	//2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END

    //$02 Add Start
    $(".SetIconsList>li").click(function (e) {
        //�C�x���g���������`�F�b�N�{�b�N�X�̏ꍇ�́A�������Ȃ�
        if ($(e.target).parents(".icrop-CustomCheckBox").length == 1) {
            return;
        }
        var checkbox = $(this).find(".icrop-CustomCheckBox>:checkbox");
        checkbox.CustomCheckBox("value", !checkbox.CustomCheckBox("value"));
    });
    //$02 Add End

    //�����G���A�̃A�j���[�V��������
    $("#SetIcons").swipe({
        swipeUp: sc3010401Script.closeCriteria,
        swipeDown: sc3010401Script.openCriteria,
        threshold: 20,
        triggerOnTouchEnd: false
    });

    //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� START
    var CheckAllFlg = 0;
    var CheckEachFlg = 0;
    //�󒍑O�ꊇ�`�F�b�N�{�b�N�X�̃`�F�b�N��
    $("#CheckAllBefore").CustomCheckBox({
    	check: function (value) {
    		if (value === true) {
    			CheckAllFlg = 1;
    			$(".BeforeCriteria").CustomCheckBox("value", true);
    			CheckAllFlg = 0;
    		} else {
    			if (CheckEachFlg == 0) {
    				CheckAllFlg = 1;
    				$(".BeforeCriteria").CustomCheckBox("value", false);
    				CheckAllFlg = 0;
    			}
    		}
    	}
    });

    //�󒍌�ꊇ�`�F�b�N�{�b�N�X�̃`�F�b�N��
    $("#CheckAllAfter").CustomCheckBox({
    	check: function (value) {
            if (value === true) {
    			CheckAllFlg = 1;
    			$(".AfterCriteria").CustomCheckBox("value", true);
    			CheckAllFlg = 0;
    		} else {
    			if (CheckEachFlg == 0) {
    				CheckAllFlg = 1;
    				$(".AfterCriteria").CustomCheckBox("value", false);
    				CheckAllFlg = 0;
    			}
    		}
    	}
    });

    //�e�󒍑O�`�F�b�N�{�b�N�X�̃`�F�b�N��
    $(".icrop-CustomCheckBox>:checkbox" + ".BeforeCriteria").CustomCheckBox(
        { "check": function (value) {
        	$(this)[0].checked = value;
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".BeforeCriteria").length == $(".BeforeCriteria").length
                 && $("#CheckAllBefore").CustomCheckBox("value") == false) {
        		$("#CheckAllBefore").CustomCheckBox("value", true);
        	}
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".BeforeCriteria").length < $(".BeforeCriteria").length
                 && $("#CheckAllBefore").CustomCheckBox("value") == true && CheckAllFlg == 0) {
        		CheckEachFlg = 1;
        		$("#CheckAllBefore").CustomCheckBox("value", false);
        		CheckEachFlg = 0;
        	}
        }
    });

    //�e�󒍌�`�F�b�N�{�b�N�X�̃`�F�b�N��
    $(".icrop-CustomCheckBox>:checkbox" + ".AfterCriteria").CustomCheckBox(
        { "check": function (value) {
        	$(this)[0].checked = value;
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".AfterCriteria").length == $(".AfterCriteria").length
                 && $("#CheckAllAfter").CustomCheckBox("value") == false) {
        		$("#CheckAllAfter").CustomCheckBox("value", true);
        	}
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".AfterCriteria").length < $(".AfterCriteria").length
                 && $("#CheckAllAfter").CustomCheckBox("value") == true && CheckAllFlg == 0) {
        		CheckEachFlg = 1;
        		$("#CheckAllAfter").CustomCheckBox("value", false);
        		CheckEachFlg = 0;
        	}
        }
    });
    //2014/02/17 TCS �R�c �󒍌�t�H���[�@�\�J�� END

    /* �_�{�C��Start */
    //���O�p�{�^��(���������J������)
    $(".WindDown").click(function () {
        sc3010401Script.openCriteria();
    });

    //��O�p�{�^��(�����������鏈��)
    $(".WindUp").click(function () {
        sc3010401Script.closeCriteria();
    });
    /* �_�{�C��End */

    //�����G���A�O�̃^�b�`����(�����G���A���J���Ă��������)
    $("#bodyFrame").click(function () {
        try {
            if ($(event.target).parents("#SetIcons").length == 0) {
                sc3010401Script.closeCriteria(true);
            }
        } catch (e) {

   	    }

   	});

    /* �_�{�C��Start */
    //2013/01/11 TCS ���{ �yA.STEP2�zDel Start
    /*
    //���O�p�{�^��(���������J������)
    $(".WindDown").click(function () {
        sc3010401Script.openCriteria();
    });
    */
    //2013/01/11 TCS ���{ �yA.STEP2�zDel End

    //�����{�^����������
    $("#AddIconRight").click(function () {
        //$01 Add Start
        //�\�[�g����������
        $("#sortTypeHidden").val(sc3010401Script.constants.sortType.cract);
        $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        //$01 Add End
        //���������ۑ�
        sc3010401Script.saveCriteria();
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    ///�ԗ������N����
    $("#CustomLabelCar").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.carName) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.carName);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }
        //�\�[�g�����i�Č����j
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    //�X�e�C�^�X�����N����
    $("#CustomLabelStatus").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.status) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.status);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }

        //�\�[�g�����i�Č����j
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    //���񊈓��������N����
    $("#CustomLabelCRACT").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.cract) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.cract);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }

        //�\�[�g�����i�Č����j
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });



});
