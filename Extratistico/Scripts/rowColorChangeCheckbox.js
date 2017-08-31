$(document).ready(function () {
    $(".rowColorChangeCheckbox").click(function () {
        if ($(this).is(":checked")) {
            $(this).parent("td").parent("tr").css('background-color', '#C6F9C1');
        } else {
            $(this).parent("td").parent("tr").css('background-color', '');
        }
    });
});