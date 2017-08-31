$(document).ready(function () {
    $(".checkimage").click(function () {
        if ($(this).siblings("input.imagecheckbox").is(":checked")) {
            $(this).siblings("input.imagecheckbox").attr("checked", false);
            $(this).attr("src", "/Content/Images/desabilitar-20.png");
        } else {
            $(this).siblings("input.imagecheckbox").attr("checked", true);
            $(this).attr("src", "/Content/Images/habilitar-20.png");
        }
    });

    $(".imagecheckbox").click(function () {
        if ($(this).is(":checked")) {
            $(this).siblings(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
        } else {
            $(this).siblings(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
        }
    });
});