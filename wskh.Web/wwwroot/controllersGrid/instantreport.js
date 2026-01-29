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
        "url": "/TimeAttendance/InstantReport/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "GroupName", "orderable": false },
        { "data": "NameAndFamily", "orderable": false },
        { "data": "EnteranceTime", "orderable": false },
        { "data": "StateString", "orderable": false }
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
                        url: '/TimeAttendance/InstantReport/DeleteTrade?firstId=' + firstId + '&secondId=' + secondId,
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
        url: '/TimeAttendance/InstantReport/AddRowToUser',
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
