$(document).ready(function () {


    $(".checkimage").click(function () {
        if ($(this).siblings("input.checkFuncionalidade").is(":checked")) {
            $(this).siblings("input.checkFuncionalidade").attr("checked", false);
            $(this).attr("src", "/Content/Images/desabilitar-20.png");

            if ($(this).parent().siblings("li").children("input:checked").length == 0) {
                $(this).parent().parent().parent().children(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
                $(this).parent().parent().parent().children("input.checkFuncionalidade").attr("checked", false);
            }
            $(this).parent().find(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
            $(this).parent().find("input.checkFuncionalidade").attr("checked", false);
        }
        else {
            $(this).siblings("input.checkFuncionalidade").attr("checked", true);
            $(this).attr("src", "/Content/Images/habilitar-20.png");
            if ($(this).parent().children("li input:checked").length != 0) {
                $(this).parent().parent().parent().children(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
                $(this).parent().parent().parent().children("input.checkFuncionalidade").attr("checked", true);
            }
            $(this).parent().find(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
            $(this).parent().find("input.checkFuncionalidade").attr("checked", true);
        }
    });

    $(".checkFuncionalidade").click(function () {
        if ($(this).is(":checked")) {
            $(this).siblings(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
            if ($(this).parent().children("li input:checked").length != 0) {
                $(this).parent().parent().parent().children(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
                $(this).parent().parent().parent().children("input.checkFuncionalidade").attr("checked", true);
            }
            $(this).parent().find(".checkimage").attr("src", "/Content/Images/habilitar-20.png");
            $(this).parent().find("input.checkFuncionalidade").attr("checked", true);
        } else {
            $(this).siblings(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
            if ($(this).parent().siblings("li").children("input:checked").length == 0) {
                $(this).parent().parent().parent().children(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
                $(this).parent().parent().parent().children("input.checkFuncionalidade").attr("checked", false);
            }
            $(this).parent().find(".checkimage").attr("src", "/Content/Images/desabilitar-20.png");
            $(this).parent().find("input.checkFuncionalidade").attr("checked", false);
        }
    });

    $(".Area > label").click(function () {
        $(this).parent().children("ul").animate({
            opacity: 'toggle',
            height: 'toggle'
        }, 600);
    });


    //    $(".checked,.unchecked").click(function () {
    //        if ($(this).is(":checked")) {
    //            $(this).parent().css({ "color": "#66CC66" });
    //            if ($(this).parent().children("ul").children("li").length != 0) {
    //                $(this).parent().children("ul").children("li").each(function (index) {
    //                    $(this).css({ "color": "#66CC66" });
    //                    $(this).children("input").attr("checked", true);
    //                });
    //            } else {
    //                if ($(this).parent().parent().children("li").children(":checked").length == $(this).parent().parent().children("li").children("input").length) {
    //                    $(this).parent().parent().parent().children("input").attr("checked", true);
    //                    $(this).parent().parent().parent().css({ "color": "#66CC66" });
    //                } else if ($(this).parent().parent().children("li").children(":checked").length > 0) {
    //                    $(this).parent().parent().parent().children("input").attr("checked", true);
    //                    $(this).parent().parent().parent().css({ "color": "#FFF46C" });
    //                } else {
    //                    $(this).parent().parent().parent().children("input").attr("checked", false);
    //                    $(this).parent().parent().parent().css({ "color": "#FF4141" });
    //                }
    //            }
    //        } else {
    //            $(this).parent().css({ "color": "#FF4141" });
    //            if ($(this).parent().children("ul").children("li").length != 0) {
    //                $(this).parent().children("ul").children("li").each(function (index) {
    //                    $(this).css({ "color": "#FF4141" });
    //                    $(this).children("input").attr("checked", false);
    //                });
    //            } else {
    //                if ($(this).parent().parent().children("li").children(":checked").length == $(this).parent().parent().children("li").children("input").length) {
    //                    $(this).parent().parent().parent().children("input").attr("checked", true);
    //                    $(this).parent().parent().parent().css({ "color": "#66CC66" });
    //                } else if ($(this).parent().parent().children("li").children(":checked").length > 0) {
    //                    $(this).parent().parent().parent().children("input").attr("checked", true);
    //                    $(this).parent().parent().parent().css({ "color": "#FFF46C" });
    //                } else {
    //                    $(this).parent().parent().parent().children("input").attr("checked", false);
    //                    $(this).parent().parent().parent().css({ "color": "#FF4141" });
    //                }
    //            }
    //        }
    //    });
});