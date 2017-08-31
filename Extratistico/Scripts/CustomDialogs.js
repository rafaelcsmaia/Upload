$(document).ready(function () {
    $("#yearPickerDialog").dialog({
        autoOpen: false,
        width: 250,
        modal: true,
        resizable: false,
        show: {
            effect: 'slide'
        },
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
            },
            "OK": function () {
                $(this).dialog("close");
                callBackYearPicker($('#selectAno').val());
            } //end of Submit button function
        } //end of buttons:
    }); //end of dialog

    $("#yearMonthPickerDialog").dialog({
        autoOpen: false,
        width: 250,
        modal: true,
        resizable: false,
        show: {
            effect: 'slide'
        },
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
            },
            "OK": function () {
                $(this).dialog("close");
                callBackYearMonthPicker($('#selectAno').val(), $('#selectMes').val(), $('#selectMes option:selected').text());
            } //end of Submit button function
        } //end of buttons:
    }); //end of dialog

    $(".picker").datepicker();
    $('#datePickerDialog').dialog({
        autoOpen: false,
        width: 400,
        modal: true,
        resizable: false,
        show: {
            effect: 'slide'
        },
        close: function () {
            $('#message').html("");
            $('#dateRangeForm :input').each(function () {
                $(this).val('');
            });
        },
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
                $('#message').html("");
                $('#dateRangeForm :input').each(function () {
                    $(this).val('');
                });
            },
            "OK": function () {
                var errors = 0;

                $('#dateRangeForm :input').each(function () {
                    if ($(this).val() == '') {
                        $(this).prev().prev().css("color", "red");
                        $(this).prev().val('Click box and use calendar to enter date.');
                        errors++;
                    } else {
                        $(this).prev().css("color", "black");
                    }
                });

                if (errors == 0) {
                    var start_date = new Date($("#startdate").val());
                    var end_date = new Date($("#enddate").val());
                    if (end_date < start_date) {
                        $('#message').html("<div class='errorMessage'>Date range is invalid.</div>");
                    } else {
                        $(this).dialog("close");
                        $('#message').html("");
                        $('#dateRangeForm :input').each(function () {
                            $(this).val('');
                        });
                        callBackDatePicker(start_date, end_date);
                    }
                } //end of if(errors == 0)
            } //end of Submit button function
        } //end of buttons:
    }); //end of dialog
});