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
        "url": "/Ticket/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "Title", "orderable": false },
        { "data": "StateString", "orderable": false },
        { "data": "Buttom", "orderable": false, "width": "100" },
    ],
    "columnDefs": [
        {
            //targets: 2,
            //render: function (data, type, row, meta) {
            //    return '<div class="dropdown mPointer"><button class="btn btn-dark btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true">عملیات<span class="caret"></span></button><ul class="dropdown-menu"><li onclick="Delete(\'/Ticket/Delete?id=' + data + '\')"><span class="dropdown-item "> حذف </span></li><li onclick="Show(' + data + ')"><span class="dropdown-item "> نمایش </span></li><li onclick="Response(' + data + ')"><span class="dropdown-item "> پاسخ </span></li></ul></div>';
            //}
        }
    ],


    "order": [[0, "asc"]]
});



function Delete2(id) {
    ToastOptions();
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    $.ajax({
                        url: "/TimeAttendance/Ticket/Delete?id=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف با موفقیت انجام شد");
                                GridReload("Grid");
                            }
                            else {
                                Message(1, "حذف انجام نشد! درفرصتی دیگر تلاش نمایید");
                            }

                        }
                    });

                    $('#Grid').DataTable().ajax.reload();
                    $('#Grid2').DataTable().ajax.reload();
                });
            }
        });
}




function Show(id) {
    $.ajax({
        url: "/TimeAttendance/Ticket/Show?id=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data
            })
        }
    });
}

function Response(id) {
    $.ajax({
        url: "/TimeAttendance/Ticket/Response?id=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data
            })
        }
    });
}
function SubmitResponse(id) {
    $.ajax({
        url: "/TimeAttendance/Ticket/Response?id=" + id + "&response=" + $("#Response").val(),
        type: 'post',
        dataType: "json",
        success: function (data) {
            HideBootbox();
            Message(0, "پاسخ ارسال شد")
            GridReload("#Grid")
        }
    });
}