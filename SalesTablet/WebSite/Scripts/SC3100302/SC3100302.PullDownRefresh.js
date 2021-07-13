$(function () {

	//スクロールイベントを監視
	$("#SC3100302InnerBox").live("move.fingerscroll", function (e, position) {


		$(".pullDownToRefresh", this).removeClass("step0 step1 step2");

		if (position.top >= 37) {
			$(".pullDownToRefresh", this).addClass("step1");
		} else {
			$(".pullDownToRefresh", this).addClass("step0");
		}
	});

	//スクロール終了イベントを監視
	$("#SC3100302InnerBox").live("end.fingerscroll", function (e, position) {

		$(".pullDownToRefresh", this).removeClass("step0 step1 step2");

		if (position.top >= 37) {
			//内部状態を更新中にする
			SC3100302Script.isInUpdate = true;
			//更新中にする
			$(".pullDownToRefresh", this).addClass("step2");

			//スクロール停止
			$("#SC3100302InnerBox").mainMenuFingerScroll({ action: "stop", scrollMode: "all" });
			//更新処理
			SC3100302Script.update();

		}

	});
});

//
function endRefreshVisitActual() {
	$("#SC3100302InnerBox .pullDownToRefresh").removeClass("step0 step1 step2").addClass("step0");
	$("#SC3100302InnerBox").mainMenuFingerScroll({ action: "restart", scrollMode: "all" });
	//内部状態を初期化する
	SC3100302Script.isInUpdate = false;
}
