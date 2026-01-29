var table = $("#Grid2").DataTable({
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
        "url": "/SpecialDay/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "WorkProgramString", "orderable": false },
        { "data": "TypeString", "orderable": false },
        { "data": "Title", "orderable": false },
        { "data": "StartDate", "orderable": false },
        { "data": "DelayEnterance", "orderable": false },
        { "data": "Id", "orderable": false, "width": "100" },
    ],
    "columnDefs": [
        {
            targets: 6,
            render: function (data, type, row, meta) {
                return '<div class="dropdown mPointer"><button class="btn btn-dark btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true">عملیات<span class="caret"></span></button><ul class="dropdown-menu"><li onclick="Delete2(\'/SpecialDay/Delete?id=' + data + '\')"><span class="dropdown-item "> حذف </span></li></ul></div>';
            }
        }
    ],
    "order": [[0, "asc"]]
});


function ChangeHolidayType(data) {
    var $val = parseInt($(data).val());
    if ($val == 0) {
        $("#divWorkProgramId").hide();
        $("#divEndDate").hide();
        $(".duration").hide();
    } else if ($val == 1) {
        $("#divWorkProgramId").hide();
        $("#divEndDate").hide();
        $(".duration").show();
    }
    else if ($val == 2) {
        $("#divWorkProgramId").show();
        $("#divEndDate").show();
        $(".duration").hide();
    }
}



function Delete2(url) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: url,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف با موفقیت انجام شد");
                                GridReload("Grid2");
                            }
                            else {
                                Command: toastr["error"](data.Message)
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