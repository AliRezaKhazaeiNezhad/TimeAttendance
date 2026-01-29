function isTitleNull(data) {
    var $data = $(data).val();
    if ($data.length > 0) {
        $("#DropPart").fadeIn();
    }
    else {
        $("#DropPart").fadeOut();
    }
}


function AddingDay() {
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/AddDay",
        type: 'get',
        dataType: "json",
        success: function (data) {
            if (data) {
                GetTable();
            }
        }
    });
    var $val = parseInt($("#ProgramTypeDrop").val());
    var tableRowCount = (parseInt($(".custable tbody tr").length) + 1) / 7;

    if ($val == 1) {
        if (tableRowCount == 1 || tableRowCount == 2 || tableRowCount == 3 || tableRowCount == 4 || tableRowCount == 5) {
            $("#Submit").removeAttr("disabled");
            $("#WeeklyAlert").fadeOut();
        } else {
            $("#Submit").attr("disabled", "disabled");
            $("#WeeklyAlert").fadeIn();
        }
    }
    else{
        $("#Submit").removeAttr("disabled");
        $("#WeeklyAlert").fadeOut();
    }
   
}



function SelectDayIndex(data) {
    var $val = parseInt($(data).val());

    if ($val == 1) {
        $("#WeeklyAlert").fadeIn();
    }
    else {
        $("#WeeklyAlert").fadeOut();
    }

    if ($val == 2 || $val == -1) {
        $("#CalculateAbsence").fadeOut();
    }
    else {
        $("#CalculateAbsence").fadeIn();
    }

    if ($val == -1) {
        $("#ProgramGrid").fadeOut();
        $("#Submit").attr("disabled", "disabled");
    }
    else {

        $.ajax({
            url: "/TimeAttendance/OrdinaryWorkProgram/ChangeDayIndex?index=" + $val,
            type: 'get',
            dataType: "json",
            success: function (data) {
                GetTable();
            }
        });
        $("#ProgramGrid").fadeIn();
        var tableRowCount = (parseInt($(".custable tbody tr").length) + 1) / 7;

        if ($val == 1) {
            if (tableRowCount == 1 || tableRowCount == 2 || tableRowCount == 3 || tableRowCount == 4 || tableRowCount == 5) {
                $("#Submit").removeAttr("disabled");
                $("#WeeklyAlert").fadeOut();
            } else {
                $("#Submit").attr("disabled", "disabled");
                $("#WeeklyAlert").fadeIn();
            }
        }
        else {
            $("#Submit").removeAttr("disabled");
        }
    }
}

function GetTable() {
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/GetTable",
        type: 'get',
        dataType: "html",
        success: function (data) {
            $("#ProgramGrid").html(data);
            $("#ProgramGrid").fadeIn();
        }
    });
}

function DeleteCol(dayIndex) {

    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف  روز مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: "/TimeAttendance/OrdinaryWorkProgram/DeleteDay?dayIndex=" + dayIndex,
                        type: 'get',
                        dataType: "json",
                        success: function (data) {
                            GetTable();
                        }
                    });

                });
            }
        });

}

function ChangeWorkTypeCol(dayIndex, data) {
    var $val = $(data).val();
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/ChangeWorkType?dayIndex=" + dayIndex + "&workType=" + parseInt($val),
        type: 'get',
        dataType: "json",
        success: function (data) {
            if (data) {
                switch (parseInt($val)) {
                    case 0:
                        $(data).closest("tr").fadeOut();
                        $(data).closest("tr").fadeIn();
                        $(data).closest("tr").removeClass("bgOrange");
                        $(data).closest("tr").removeClass("bgRed");
                        $(data).closest("tr").addClass("bgDefault");
                        break;
                    case 1:
                        $(data).closest("tr").fadeOut();
                        $(data).closest("tr").fadeIn();
                        $(data).closest("tr").removeClass("bgDefault");
                        $(data).closest("tr").removeClass("bgOrange");
                        $(data).closest("tr").addClass("bgRed");
                        break;
                    case 2:
                        $(data).closest("tr").fadeOut();
                        $(data).closest("tr").fadeIn();
                        $(data).closest("tr").removeClass("bgDefault");
                        $(data).closest("tr").removeClass("bgRed");
                        $(data).closest("tr").addClass("bgOrange");
                        break;
                    default:
                        $(data).closest("tr").fadeOut();
                        $(data).closest("tr").fadeIn();
                        $(data).closest("tr").removeClass("bgDefault");
                        $(data).closest("tr").removeClass("bgOrange");
                        $(data).closest("tr").removeClass("bgRed");
                        break;
                }
            }
            GetTable();
        }
    });

}



function RefreshCol(dayIndex) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از بروزرسانی روز مطمئنید ؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: "/TimeAttendance/OrdinaryWorkProgram/ResetDay?index=" + dayIndex,
                        type: 'get',
                        dataType: "json",
                        success: function (data) {
                            if (data) {
                                Message(0, "بروزرسانی انجام شد");
                                GetTable();
                            }
                            else {
                                Message(2, "بروزرسانی انجام شد");
                            }
                        }
                    });

                });
            }
        });
}

function CoppyCol(dayIndex) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از کپی این روز به رز بعدی مطمئنید ؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: "/TimeAttendance/OrdinaryWorkProgram/CoppyCol?index=" + dayIndex,
                        type: 'get',
                        dataType: "json",
                        success: function (data) {
                            if (data) {
                                Message(0, "کپی به روز بعد انجام شد");
                                GetTable();
                            }
                            else {
                                Message(2, "کپی به روز بعد انجام شد");
                            }
                        }
                    });

                });
            }
        });
}


function EditCol(dayIndex, data) {
    var $data = data;
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/GetDay?dayIndex=" + dayIndex,
        type: 'get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({ message: data })
        }
    });

}

function CloseDay(dayIndex) {
    bootbox.hideAll();
    $(".CustomeCheck").remove();
    $(".IndexNumber").each(function (e) {
        var $val = parseInt($(this).text());
        if (parseInt(dayIndex) == $val) {
            $(this).closest("tr").children("td:nth-child(1)").prepend("<i class='fa fa-check-circle CustomeCheck' style='color:blue' aria-hidden='true'></i>");
        }
    })
    $(".CrudFamily").show();
    $("button[type='submit']").removeAttr("disabled");
    $("#AddDay").removeAttr("disabled");
}



function AddTime(programType, dayIndex, data) {
    var $programType = parseInt(programType);
    var $dayIndex = parseInt(dayIndex);
    var $data = $(data);

    var $WorkTimeInDay = $("#WorkTimeInDay").val();
    var $MoreThan24Houres = $("#MoreThan24Houres").is(":checked");

    var $WorkType = $("#WorkType").val();
    var $StartTime = $("#StartTime").val();
    var $EndTime = $("#EndTime").val();




    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/AddDayTime",
        data: {
            programType: $programType,
            dayIndex: $dayIndex,
            workTimeInDay: $WorkTimeInDay,
            moreThan24Houres: $MoreThan24Houres,
            workType: $WorkType,
            startTime: $StartTime,
            endTime: $EndTime
        },
        type: 'get',
        dataType: "json",
        success: function (data) {
            if (data == 0) {
                Message(1, "خطایی رخ داده است! در فرصتی دیگر تلاش نمایید");
            } else if (data == 1) {
                Message(0, "عملیات با موفقیت انجام شد");
                GetDay(dayIndex);
            }
            else if (data == 2) {
                Message(2, "زمان وارد شده، تداخل دارد!");
            }
            if (programType == 2) {
                HideBootbox();
            }
        }
    });
}


function GetDuration() {
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/GetDuration",
        data: {
            startTime: $("#StartTime").val(),
            endTime: $("#EndTime").val(),
        },
        type: 'get',
        dataType: "json",
        success: function (data) {
            $("#Duration").val(data);
        }
    });
}
function GetDay(dayIndex) {
    $.ajax({
        url: "/TimeAttendance/OrdinaryWorkProgram/GetDay?dayIndex=" + dayIndex,
        type: 'get',
        dataType: "html",
        success: function (data) {
            $("#ModelDays").html(data);
            GetTable();
        }
    });
}



function DeleteTime(index, dayIndex) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید ؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: "/TimeAttendance/OrdinaryWorkProgram/DeleteTime?index=" + index + "&dayIndex=" + dayIndex,
                        type: 'get',
                        dataType: "json",
                        success: function (data) {
                            if (data) {
                                Message(0, "عملیات با موفقیت انجام شد");
                                GetDay(parseInt(dayIndex) + 1)
                                GetTable();
                            }
                            else {
                                Message(2, "خطایی رخ داده است! در فرصتی دیگر تلاش نمایید");
                            }
                        }
                    });

                });
            }
        });
}