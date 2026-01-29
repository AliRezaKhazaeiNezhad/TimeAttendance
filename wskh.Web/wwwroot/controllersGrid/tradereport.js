var table = $("#Grid").DataTable({
    "language": {
        "sEmptyTable": "هیچ داده ای در جدول وجود ندارد",
        "sInfo": "نمایش _START_ تا _END_ از _TOTAL_ رکورد",
        "sInfoEmpty": "نمایش 0 تا 0 از 0 رکورد",
        "sInfoFiltered": "(فیلتر شده از _MAX_ رکورد)",
        "sInfoPostFix": "",
        "sInfoThousands": ",",
        "sLengthMenu": "نمایش _MENU_ رکورد",
        "sLoadingRecords": "در حال بارگزاری...",
        "sProcessing": "در حال پردازش...",
        "sSearch": "جستجو:",
        "sZeroRecords": "رکوردی با این مشخصات پیدا نشد",
        "oPaginate": {
            "sFirst": "ابتدا",
            "sLast": "انتها",
            "sNext": "بعدی",
            "sPrevious": "قبلی"
        },
        "oAria": {
            "sSortAscending": ": فعال سازی نمایش به صورت صعودی",
            "sSortDescending": ": فعال سازی نمایش به صورت نزولی"
        }
    },
    "processing": true,
    "serverSide": true,
    "searching": false,
    "info": true,
    "scrollX": false,
    "stateSave": true,
    "lenghtMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
    "ajax": {
        "url": "/TradeReport/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "GroupName", "orderable": true, "width": "200"  },
        { "data": "NameAndFamily", "orderable": true, "width": "150"  },
        { "data": "DayName", "orderable": false, "width": "60" },
        { "data": "PersianDaet", "orderable": true, "width": "100"  },
        { "data": "EnteranceTime", "orderable": false },
        { "data": "Button1", "orderable": false, "width": "10"  }
    ],
    "order": [[0, "asc"]]
});


function DeleteTrade(firstId, secondId) {
    ToastOptions();
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: '/TimeAttendance/TradeReport/DeleteTrade?firstId=' + firstId + '&secondId=' + secondId,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data == 1) {
                                Message(0, "حذف با موفقیت انجام شد");
                                GridReload("Grid");
                            }
                            else {
                                Message(1, "حذف انجام نشد! درفرصتی دیگر تلاش نمایید");
                            }

                        }
                    });

                    $('#Grid').DataTable().ajax.reload();
                });
            }
        });
}


function AddRowToUser(reportDayId, userId) {
    var $enteranceTime = $("#EnteranceTime").val();
    var $exitTime = $("#ExitTime").val();

    $.ajax({
        url: '/TimeAttendance/TradeReport/AddRowToUser',
        data: {
            reportDayId: reportDayId,
            userId: userId,
            enteranceTime: $enteranceTime,
            exitTime: $exitTime
        },
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data
            });
        }
    });


}

function AddTrade() {
    var $enteranceTime = $("#EnteranceTime").val();
    var $exitTime = $("#ExitTime").val();
    var $reportDayId = $("#ReportDayId").val();
    var $userId = $("#UserId").val();

    $.ajax({
        url: '/TimeAttendance/TradeReport/CheckTimes',
        data: {
            enteranceTime: $enteranceTime,
            exitTime: $exitTime
        },
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                $.ajax({
                    url: '/TimeAttendance/TradeReport/AddRowToUser',
                    data: {
                        reportDayId: $reportDayId,
                        userId: $userId,
                        enteranceTime: $enteranceTime,
                        exitTime: $exitTime
                    },
                    type: 'Post',
                    dataType: "html",
                    success: function (data2) {
                        if (data2) {
                            Message(0, "عملیات با موفقیت انجام شد");
                            HideBootbox();
                        }
                        else {
                            Message(1, "خطایی رخ داده است، در فرصتی دیگر تلاش نمایید");
                        }
                        $('#Grid').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Message(1, "زمان پایان باید بزرگتر از زمان شروع باشد");
            }
        }
    });
}