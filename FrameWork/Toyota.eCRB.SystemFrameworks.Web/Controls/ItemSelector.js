$(function () {
    var theTargets = $(".icrop-ItemSelector");

    theTargets.find(".icrop-ItemSelector-item")
        .click(function (e) {
            var theTarget = $("#" + $(this).attr("data-selector"));
            theTarget.children(".postBackData").val($(this).attr("data-value"));
            theTarget.find(".icrop-CustomButton-label").text($(this).text());
            $("#bodyFrame").trigger("click.popover");
        });
});
