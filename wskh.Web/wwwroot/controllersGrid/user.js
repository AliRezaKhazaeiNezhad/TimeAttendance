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
    "searching": true,
    "info": true,
    "scrollX": false,
    "stateSave": true,
    "lenghtMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
    "ajax": {
        "url": "/User/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "GroupName", "orderable": false },
        { "data": "FirstName", "orderable": false },
        { "data": "UserName", "orderable": false },
        { "data": "NationalCode", "orderable": false },
        { "data": "Active", "orderable": false },
        { "data": "Id", "orderable": false, "width": "100" },
    ],
    "columnDefs": [
        {
            targets: 6,
            render: function (data, type, row, meta) {
                return '<div class="dropdown mPointer"><button class="btn btn-dark btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true">عملیات<span class="caret"></span></button><ul class="dropdown-menu"><li onclick="Delete(\'/User/Delete?id=\'' + data + '\')"><span class="dropdown-item "> حذف </span></li><li onclick="GetEmptyView(\'/TimeAttendance/User/AddOrUpdate?id=' + data + '\', \'AddOrUpdate-Part\')"><span class="dropdown-item "> ویرایش </span></li><li onclick="ResetPassword(\'' + data + '\')"><span class="dropdown-item "> ریست گذرواژه </span></li><li onclick="Deactive(\'' + data + '\')"><span class="dropdown-item "> فعال/غیرفعال </span></li><li onclick="Detail(\'' + data + '\')"><span class="dropdown-item "> نمایش </span></li><li onclick="AssignEnroll(\'' + data + '\')"><span class="dropdown-item "> انتساب کاربر سخت افزار </span></li></li></ul></div>';
            }
        }
    ],



    "order": [[0, "asc"]]
});


function ResetPassword(id) {
    $.ajax({
        url: "/TimeAttendance/User/ResetPassword?id=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                Message(0, "گذرواژه به کدپرسنلی تغییر یافت");
            }
            else {
                Message(1, "خطایی رخ داده است! درفرصتی دیگر تلاش نمایید");
            }
            DataTableReload();
        }
    })
}

function AssignEnroll(id) {
    $.ajax({
        url: "/TimeAttendance/User/AssignEnroll?id=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: "large",
            })
        }
    })
}


function Deactive(id) {
    $.ajax({
        url: "/TimeAttendance/User/ActiveDeactive?id=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                Message(0, "تغییر وضعیت کاربر با موفقیت انجام شد");
            }
            else {
                Message(1, "خطایی رخ داده است! درفرصتی دیگر تلاش نمایید");
            }
            DataTableReload();
        }
    })
}



function Detail(id) {
    $.ajax({
        url: "/TimeAttendance/User/Deatil?id=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: "large",
            })
        }
    })
}

function AddRelation(id) {
    $.ajax({
        url: "/TimeAttendance/Contract/Index?userId=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: "large",
            })
        }
    })
}


function AssignEnrollToUser(id) {
    $.ajax({
        url: "/TimeAttendance/User/AssignEnrollToUser?userId=" + $("#CurrentUserId").val() + "&enroll=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                Message(0, "انتساب/عدم انتساب با موفقیت انجام شد");
            }
            else {
                Message(1, "خطایی رخ داده است! درفرصتی دیگر تلاش نمایید");
            }
            LogToReport(id);
            //HideBootbox();
        }
    })
}

function LogToReport(id) {
    $.ajax({
        url: "/TimeAttendance/User/LogToReport?enroll=" + id ,
        type: 'Get',
        dataType: "json",
        success: function (data) {
          
        }
    })
}
