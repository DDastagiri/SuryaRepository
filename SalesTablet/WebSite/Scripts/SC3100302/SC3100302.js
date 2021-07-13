

var SC3100302Script;

SC3100302Script = function () {
	/**
	* @class 定数
	*/
	var constants = {
		init: "CreatePriceConsultationWindow",
		request: "InsertPriceConsultationInfo",
		cancel: "CancelPriceConsultationInfo"
	}

	//来店実績更新中フラグ
	var isInUpdate = false;

	function update() {
		$("#SC3100302Trigger").click();
	}


	return {
		update: update,
		isInUpdate:isInUpdate
	}
} ();

$(function () {
    //チップをタップした時のイベント処理
        $("#VisitActualRow").live("click", function () {
        if (SC3100302Script.isInUpdate) {
            //更新中の場合は、処理終了
            return;
        }
        var visitActualRow;
        if (this.id == "VisitActualRow") {
            visitActualRow = $(this);
        } else {
            visitActualRow = $(this).parents("#VisitActualRow");
        }

        //顧客詳細（商談メモ）へ遷移
        //2013/10/02 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
        //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
        contactHistoryCustomerDetailTransfer(visitActualRow.find(".CustomerSegment").val(),
		visitActualRow.find(".CustomerClass").val(),
		visitActualRow.find(".CustomerId").val(),
		 visitActualRow.find(".Strcd").val(),
		 visitActualRow.find(".FllwupBoxSeqno").val(),
		 visitActualRow.find(".SalesStatus").val());
        //2013/10/02 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END
    });

})



