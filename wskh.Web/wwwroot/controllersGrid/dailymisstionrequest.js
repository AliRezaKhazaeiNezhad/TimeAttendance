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
        "url": "/DailyMissionRequest/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "CreateDateStriing", "orderable": false },
        { "data": "StartDateString", "orderable": false },
        { "data": "StateString", "orderable": false },
        { "data": "UserRequester", "orderable": false },
        { "data": "UserRequesteManager", "orderable": false },
        { "data": "Buttom", "orderable": false, "width": "100" },
    ],
    "order": [[0, "asc"]]
});





function Show(id) {
    ToastOptions();
    $.ajax({
        url: '/TimeAttendance/DailyMissionRequest/Show?id=' + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data
            })
        }
    });
}


function Approve(id) {
    ToastOptions();
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از تایید درخواست مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: '/TimeAttendance/DailyMissionRequest/Approve?id=' + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "تایید با موفقیت انجام شد");
                                GridReload("Grid");
                            }
                            else {
                                Command: toastr["error"](data.Message)
                                Message(1, "تایید انجام نشد! درفرصتی دیگر تلاش نمایید");
                            }
                            $('#Grid').DataTable().ajax.reload();
                        }
                    });

                });
            }
        });
}



function DisApprove(id) {
    ToastOptions();
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از عدم تایید درخواست مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {

                    $.ajax({
                        url: '/TimeAttendance/DailyMissionRequest/DisApprove?id=' + id,
                        type: 'Get',
                        dataType: "html",
                        success: function (data) {
                            bootbox.dialog({
                                message: data
                            });
                        }
                    });
                });
            }
        });
}

function DisApprovePost(id) {
    $.ajax({
        url: '/TimeAttendance/DailyMissionRequest/DisApprove?id=' + id + '&desc=' + $("#RejectReason").val() ,
        type: 'post',
        dataType: "json",
        success: function (data) {
            if (data) {
                Message(0, "عملیات با موفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد! درفرصتی دیگر تلاش نمایید");
            }
            HideBootbox();
            $('#Grid').DataTable().ajax.reload();
        }
    });
}

function DisApproveReasn() {
    if ($("#RejectReason").val().length > 0) {
        $("#DisApprovePost").removeAttr('disabled');
    }
    else {
        $("#DisApprovePost").attr('disabled', 'disabled');
    }
}