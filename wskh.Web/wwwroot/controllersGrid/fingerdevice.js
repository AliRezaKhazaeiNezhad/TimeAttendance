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
        "url": "/FingerDevice/List",
        "type": "get"
    },
    "columns": [
        { "data": "Index", "orderable": false, "width": "50" },
        { "data": "Title", "orderable": false },
        { "data": "IP", "orderable": false },
        { "data": "PortNo", "orderable": false },
        { "data": "MacAddress", "orderable": false },
        { "data": "ModelName", "orderable": false },
        { "data": "Id", "orderable": false, "width": "100" },
    ],
    "columnDefs": [
        {
            targets: 6,
            render: function (data, type, row, meta) {
                return '<div class="dropdown mPointer"><button class="btn btn-dark btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true">عملیات<span class="caret"></span></button><ul class="dropdown-menu"><li onclick="Delete(\'/FingerDevice/Delete?id=' + data + '\')"><span class="dropdown-item "> حذف </span></li><li onclick="GetEmptyView(\'/TimeAttendance/FingerDevice/AddOrUpdate?id=' + data + '\', \'AddOrUpdate-Part\')"><span class="dropdown-item "> ویرایش </span></li><li onclick="Actions(' + data + ')"><span class="dropdown-item "> دستورات دستگاه</li></ul></div>';
            }
        }
    ],
    "order": [[0, "asc"]]
});


var $Title = $("#Title").val();
var $PortNo = $("#PortNo").val();
var $CommKey = $("#CommKey").val();
var $IP = $("#IP").val();
var $DeviceInnerId = $("#DeviceInnerId").val();


function isNullInput() {
    $("button[type='submit']").attr("disabled", "disabled");
    $Title = $("#Title").val();
    $PortNo = $("#PortNo").val();
    $CommKey = $("#CommKey").val();
    $IP = $("#IP").val();
    $DeviceInnerId = $("#DeviceInnerId").val();
    if ($Title.length > 0 && $CommKey.length > 0 && $IP.length > 0 && $DeviceInnerId.length > 0) {
        $("#Connection").removeClass("btn-default");
        $("#Connection").addClass("btn-warning");
        $("#Connection").removeAttr("disabled");
    }
    else {
        $("#Connection").attr("disabled", "disabled");
        $("#Connection").addClass("btn-default");
        $("#Connection").removeClass("btn-warning");
    }
}


function ConnectionSearch() {
    $("#Spiner").show();
    $.ajax({
        url: "/TimeAttendance/DeviceCommand/IPDuplicate?ip=" + $IP,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            ToastOptions();
            console.log(data.Success)
            if (data.Success) {
                $.ajax({
                    url: "/TimeAttendance/DeviceCommand/CheckConnection?portNo=" + $PortNo + "&commKey=" + $CommKey + "&iP=" + $IP + "&deviceInnerId=" + $DeviceInnerId,
                    type: 'Get',
                    dataType: "json",
                    success: function (data) {
                        ToastOptions();
                        if (data.Success) {
                            Message(0, "اتصال با موفقیت انجام شد");
                            $("#Manufacturer").val(data.Manufacturer);
                            $("#SDKVersion").val(data.SDKVersion);
                            $("#FirmwareVersion").val(data.FirmwareVersion);
                            $("#SerialNo").val(data.SerialNo);
                            $("#MacAddress").val(data.MacAddress);
                            $("#ModelName").val(data.ModelName);
                            $("#FTPDescription").val(data.FTPDescription);
                            $("#IsColorScreen").prop('checked', true);
                            $("button[type='submit']").removeAttr("disabled");
                            $("button[type='submit']").removeClass("btn-default");
                            $("button[type='submit']").addClass("btn-success");
                        }
                        else {
                            Message(1, "اتصال انجام نشد");
                            $("#Manufacturer").val("-");
                            $("#SDKVersion").val("-");
                            $("#FirmwareVersion").val("-");
                            $("#SerialNo").val("-");
                            $("#MacAddress").val("-");
                            $("#ModelName").val("-");
                            $("#FTPDescription").val("-");
                            $("#IsColorScreen").prop('checked', false);
                            $("button[type='submit']").attr("disabled", "disabled");
                            $("button[type='submit']").addClass("btn-default");
                            $("button[type='submit']").removeClass("btn-success");
                        }
                        $("#Spiner").hide();
                    }
                });
            }
            else {
                Message(1, " ای پی تکراری میباشد");
                $("#IsColorScreen").prop('checked', false);
                $("button[type='submit']").attr("disabled", "disabled");
                $("button[type='submit']").addClass("btn-default");
                $("button[type='submit']").removeClass("btn-success");
                $("#Spiner").hide();
            }
        }
    });

}

function Actions(id) {
    $.ajax({
        url: "/TimeAttendance/DeviceCommand/Actions?deviceId=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            Message(1, "درحال لود دستگاه میباشد، لطفا منتظر بمانید.");
            ToastOptions();
            bootbox.dialog({
                message: data
            });
        }
    });
}

