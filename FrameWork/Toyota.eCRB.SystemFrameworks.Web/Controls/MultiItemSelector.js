$(function () {
    var theTargets = $(".icrop-MultiItemSelector");

    theTargets.find(".icrop-MultiItemSelector-item")
        .click(function (e) {
            if ($(this).hasClass("icrop-selected")) {
                $(this).removeClass("icrop-selected");
            } else {
                $(this).addClass("icrop-selected");
            }

            //選択値をカンマ区切りでpostBackDataに結合
            var values = "";
            var items = $(this).parent().children(".icrop-MultiItemSelector-item");
            items.each(function (index, elem) {
                if ($(elem).hasClass("icrop-selected")) {
                    values += $(elem).attr("data-value") + ",";
                }
            });

            var theTarget = $(this).parents(".icrop-MultiItemSelector");
            theTarget.children(".postBackData").val(values);
        });
});
