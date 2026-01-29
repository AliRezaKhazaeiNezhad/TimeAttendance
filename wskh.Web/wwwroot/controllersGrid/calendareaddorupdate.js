/*  توابع مربوط به کارت اول  */
function isStepOneNull() {
    var $Title = $("#Title").val();
    var $PersianYearId = $("#PersianYearId").val();
    var $HolidayAndSpecialDayId = $("#HolidayAndSpecialDayId").val();
    return (parseInt($Title.length) > 0 && parseInt($PersianYearId) > 0 && parseInt($HolidayAndSpecialDayId) > 0) ? false : true;
}
function StepOneBTNState() {
    if (isStepOneNull()) {
        $("#StepOneBTN").prop("disabled", true);
        $("#Panel2").addClass("customeDisabled");
        $("#Panel3").addClass("customeDisabled");
        $("#StepThree").hide();
    }
    else {
        $("#StepOneBTN").removeAttr("disabled");
        $("#Panel2").removeClass("customeDisabled");
    }
    
}
$("#Title").on('keyup', function () {

    StepOneBTNState();
});
$("#PersianYearId").on('change', function () {
    StepOneBTNState();
    $("#StepThree").hide();
});
$("#HolidayAndSpecialDayId").on('change', function () {
    StepOneBTNState();
    $("#StepThree").hide();
});
function StepPneNextLevel() {
    $.ajax({
        url: "/TimeAttendance/Calendare/AddDataPartOne?title=" + $("#Title").val() + "&persianYearId=" + $("#PersianYearId").val() + "&holidayId=" + $("#HolidayAndSpecialDayId").val() + "&requestRuleId=" + $("#RequestRuleId").val(),
        type: 'get',
        dataType: "json",
        success: function (data) {
            if (data.State == "0") {
                $("#Panel3").removeClass("customeDisabled");
                $("#StepThree").show();
            } else {
                Message(1, "خطایی رخ داده است ، در فرصتی دیگر تلاش نمایید");
            }
        }
    });
}

/*   توابع مزبوط به کرت دوم   */
$("#WorkRuleModel_FlowTimeId").on('change', function () {

    var $val = $("#WorkRuleModel_FlowTimeId").val();
    if ($val == 1) {
        $(".FlowTimeMinPart").fadeIn();
        $(".NoneFlowTimeMinPart").fadeOut();
    }
    else {
        $(".FlowTimeMinPart").fadeOut();
        $(".NoneFlowTimeMinPart").fadeIn();
    }
});



function isTitleNull(data) {
    var $data = $(data).val();
    if ($data.length > 0) {
        $("#InsertionPart").fadeIn();
        $("#CalendarePart").fadeIn();
    }
    else {
        $("#InsertionPart").fadeOut();
        $("#CalendarePart").fadeOut();
    }
}

function GetImportantData() {
    var $title = $("#Title").val();

    $title = $.trim($title);
    $.ajax({
        url: "/TimeAttendance/Calendare/AddCalendare?title=" + $title,
        type: 'get',
        dataType: "json",
        success: function (data) {
        }
    });
}
function AllowDate() {
    var $startDate = "1390/" + $("#StartDateMonthId").val() + "/" + $("#StartDateDayId").val();
    var $endDate = "1390/" + $("#EndDateMonthId").val() + "/" + $("#EndDateDayId").val();
    $.ajax({
        url: "/TimeAttendance/Calendare/AllowDate?stDate=" + $startDate + "&edDate=" + $endDate,
        type: 'get',
        dataType: "json",
        success: function (data) {
            return data;
        }
    });
}
function AddRow() {
    var year = $("#PersianYearId").val();
    var $startDate = year + "/" + $("#CalendarFormatModel_StartDateMonthId").val() + "/" + $("#CalendarFormatModel_StartDateDayId").val();
    var $endDate = year + "/" + $("#CalendarFormatModel_EndDateMonthId").val() + "/" + $("#CalendarFormatModel_EndDateDayId").val();
    var $wpId = $("#CalendarFormatModel_WorkProgramId option:selected").val();

    $.ajax({
        url: "/TimeAttendance/Calendare/AllowDate?stDate=" + $startDate + "&edDate=" + $endDate,
        type: 'get',
        dataType: "json",
        success: function (data) {
            if (data == 1) {
                Message(1, "تاریخ پایان نمی تواند کوچکتراز تاریخ شروع باشد");
            }
            else if (data == 2) {
                Message(1, "تاریخ شروع و پایان نمی توانند برابر باشند");
            }
            else if ($startDate.length == 0 || $endDate.length == 0) {
                Message(1, "تاریخ شروع و پایان را وارد نمایید");
            }
            else {
                $.ajax({
                    url: "/TimeAttendance/Calendare/AddDay?startDate=" + $startDate + "&endDate=" + $endDate + "&wpId=" + $wpId,
                    type: 'get',
                    dataType: "json",
                    success: function (data) {
                        if (data.State == 0) {
                            Message(0, "اطلاعات با موفقیت افزوده شد");
                            GetCalendare();
                        } else if (data.State == 2) {
                            Message(1, data.Message);
                            GetCalendare();
                        }
                    }
                });
            }
        }
    });
}

function GetCalendare() {
    $.ajax({
        url: "/TimeAttendance/Calendare/GetCalendare",
        type: 'get',
        dataType: "html",
        success: function (data) {
            if (data) {
                $("#CalendarePart").html(data);
            }
        }
    });
}



function ChangeMonth(data, name) {
    var $val = parseInt($(data).val());
    if (name == "start") {
        $("#CalendarFormatModel_StartDateDayId option:gt(0)").show();

        if ($val >= 7 && $val <= 11) {
            $("#CalendarFormatModel_StartDateDayId option:gt(29)").hide();
        } else if ($val == 12) {
            $("#CalendarFormatModel_StartDateDayId option:gt(28)").hide();
        }
    }
    else {
        $("#CalendarFormatModel_EndDateDayId option:gt(0)").show();

        if ($val >= 7 && $val <= 11) {
            $("#CalendarFormatModel_EndDateDayId option:gt(29)").hide();
        } else if ($val == 12) {
            $("#CalendarFormatModel_EndDateDayId option:gt(28)").hide();
        }
    }
}

function RemoveDay(index) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف  روز مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: "/TimeAttendance/Calendare/RemoveDay?dayIndex=" + index,
                        type: 'get',
                        dataType: "json",
                        success: function (data) {
                            console.log(data)
                            if (data.State == "0") { 
                                Message(0, "اطلاعات با موفقیت حذف شد");
                            } else {
                                Message(1, "خطایی رخ داده است، در فرصتی دیگر تلاش نمایید");
                            }
                            GetCalendare();
                        }
                    });

                });
            }
        });
}

function AddOrUpdate(id) {
                Message(0, "اطلاعات با موفقیت ذخیره شد");

    $.ajax({
        url: "/TimeAttendance/Calendare/AddOrUpdate?id=" + id,
        type: 'post',
        dataType: "json",
        success: function (data) {
            $("#GetCalendare").hide();
            if (data.State == "0") { 
                Message(0, "اطلاعات با موفقیت ذخیره شد");
                $("#AlertPart").show();
                $("#DetailPart").hide();
            } else {
                Message(1, "خطایی رخ داده است، در فرصتی دیگر تلاش نمایید");
                $("#WarningPart").show();
            }
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    });
}
