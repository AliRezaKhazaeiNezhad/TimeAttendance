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
        "url": "/OrganizationLevel/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "Title", "orderable": false },
        { "data": "Id", "orderable": false, "width": "100" },
    ],
    "columnDefs": [
        {
            targets: 2,
            render: function (data, type, row, meta) {
                return '<div class="dropdown mPointer"><button class="btn btn-dark btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true">عملیات<span class="caret"></span></button><ul class="dropdown-menu"><li onclick="DeleteType3(\'/OrganizationLevel/Delete?id=' + data + '\')"><span class="dropdown-item "> حذف </span></li><li onclick="GetEmptyView(\'/TimeAttendance/OrganizationLevel/AddOrUpdate?id=' + data + '\', \'AddOrUpdate-Part\')"><span class="dropdown-item "> ویرایش </span></li></ul></div>';
            }
        }
    ],



    "order": [[0, "asc"]]
});
