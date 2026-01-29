


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
    debugger
    $.ajax({
        url: "/TimeAttendance/DeviceCommand/IPDuplicate?ip=" + $IP,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            ToastOptions();
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
            ToastOptions();
            bootbox.dialog({
                message: data
            });
        }
    });
}


