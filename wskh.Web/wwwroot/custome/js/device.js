var _url = "/TimeAttendance/DeviceCommand/";




function DeactiveBTNs() {
    $(".actionBTN").attr("disabled", "disabled");
}

function ActiveBTNs() {
    $(".actionBTN").removeAttr("disabled");
}




function ChangeFunctionKey($id, $keyName, data) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "ChangeFunctionKey?deviceId=" + $id + "&keyName=" + $keyName + "&dropId=" + $(data).val(),
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                Message(0, "عملیت با موفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد");
            }
            $("#GetLogSpiner").fadeOut();
            StateButtons(false);
            ActiveBTNs();
        }
    });
}


function StateButtons(state) {
    if (state == true) {
        //$("button").attr("disabled", "disabled");
        $(".fontBTN").fadeOut();
    }
    else {
        //$("button").removeAttr("disabled");
        $(".fontBTN").fadeIn();
    }
}

function UpdateDateTime(id) {
    StateButtons(true);
    $("#SetDateTimeSpiner").fadeIn();
    Message(3, "لطفا چندلحظه صبر نماید");
    DeactiveBTNs();
    $.ajax({
        url: _url + "SetDateTime?deviceId=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            ToastOptions();
            if (data) {
                Message(0, "بروزرسانی زمان و تاریخ باموفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد");
            }
            $("#SetDateTimeSpiner").fadeOut();
            StateButtons(false);
            ActiveBTNs();
        }
    });
}
function ClearAdministrator(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف تمامی مدیرهای دستگاه مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    DeactiveBTNs();
                    StateButtons(true);
                    Message(3, "لطفا چندلحظه صبر نماید");
                    $("#ClearAdministratorSpiner").fadeIn();
                    $.ajax({
                        url: _url + "ClearAdministrator?deviceId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف سطح دسترسی مدیران دستگاه باموفقیت انجام شد");
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            $("#ClearAdministratorSpiner").fadeOut();
                            StateButtons(false);
                            ActiveBTNs();
                        }
                    });
                });
            }
        });
}
function DeleteGeneralLogs(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف تمامی ورود و خروج های دستگاه مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    Message(3, "لطفا چندلحظه صبر نماید");
                    StateButtons(true);
                    $("#DeleteGeneralLogSpiner").fadeIn();
                    DeactiveBTNs();
                    $.ajax({
                        url: _url + "DeleteGeneralLog?deviceId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف کلیه ورود و خروج ها باموفقیت انجام شد");
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            $("#DeleteGeneralLogSpiner").fadeOut();
                            StateButtons(false);
                            ActiveBTNs();
                        }
                    });


                });
            }
        });



}
function IsConnectedByBTN(id) {
    $("#connectionBTN").attr("disabled", "disabled");
    $("#connectionBTNSpiner").fadeIn();

    $("#ConnectionState").html('بررسی اتصال<i class= "fa fa-spinner fa-spin"></i>');
    IsConnected(id);

    $("#connectionBTN").removeAttr("disabled");
    $("#connectionBTNSpiner").fadeOut();
}

function IsConnected(id) {
    $(".actionBTN").attr("disabled", "disabled");
    StateButtons(true);
    $.ajax({
        url: _url + "IsConnected?deviceId=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            ToastOptions();
            if (data == true) {
                $("#ConnectionState").html('<span style="color:green">اتصال برقرار است</span>');
                $(".actionBTN").removeAttr("disabled");
            }
            else {
                $("#ConnectionState").html('<span style="color: red">اتصال برقرار نیست</span>');
            }
            $("#connectionBTNSpiner").fadeOut();
            StateButtons(false);
        }
    });
}
function ClearAllData(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف تمامی اطلاعات دستگاه مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    Message(3, "لطفا چندلحظه صبر نماید");
                    $("#ClearAllDataSpiner").fadeIn();
                    StateButtons(true);
                    DeactiveBTNs();
                    $.ajax({
                        url: _url + "ClearAllData?deviceId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف همه اطلاعات دستگاه و پیغام هشدار به با اعمال این گزینه کلیه ورود و خروج های دستگاه و کاربران دستگاه حذف می گردند! ایا مطمئن هستید ؟");
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            $("#ClearAllDataSpiner").fadeOut();
                            StateButtons(false);
                            ActiveBTNs();
                        }
                    });


                });
            }
        });
}
function RestartDevice(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از شروع مجدد دستگاه مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    Message(3, "لطفا چندلحظه صبر نماید");
                    StateButtons(true);
                    $("#RestartDeviceSpiner").fadeIn();
                    DeactiveBTNs();
                    $.ajax({
                        url: _url + "RestartDevice?deviceId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "عملیات باموفقیت انجام شد");
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            $("#RestartDeviceSpiner").fadeOut();
                            StateButtons(false);
                            ActiveBTNs();
                        }
                    });


                });
            }
        });
}

function Update(id) {
    StateButtons(true);
    $("#UpdateBTN").attr("disabled", "disabled");
    $.ajax({
        url: _url + "Update?deviceId=" + id,
        type: 'POST',
        dataType: "json",
        success: function (data) {
            $.ajax({
                url: _url + "Update?deviceId=" + id,
                type: 'get',
                dataType: "HTML",
                success: function (data1) {
                    $("#FingerDetail").html(data1);
                }
            });
            ToastOptions();
            if (data) {
                Message(0, "بروزرسانی با موفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد");
            }
            $("#RestartDeviceSpiner").fadeOut();
            $("#UpdateBTN").removeAttr("disabled");
            StateButtons(false);
        }
    });
}



function GetLogs(id) {
    ToastOptions();
    Message(3, "لطفا چندلحظه صبر نماید");
    StateButtons(true);
    $("#GetLogSpiner").fadeIn();
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetLog?deviceId=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                GetLogsGrid(id);
                Message(0, "دریافت ورود و خروج ها باموفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد");
            }
            $("#GetLogSpiner").fadeOut();
            StateButtons(false);
            ActiveBTNs();
        }
    });
}


function AddCard(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "AddCard?deviceId=" + id + "&dropId=" + $("#AddCard").val() + "&enrollNo=" + $("#AddCardEnroll").val(),
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data == 1) {
                Message(0, "عملیت با موفقیت انجام شد");
                GetCardList(id);
            }
            else if (data == 0) {
                Message(1, "عملیات انجام نشد");
            }
            else if (data == 2) {
                Message(1, "شماره کارت موجود میباشد");
            }
            $("#GetLogSpiner").fadeOut();
            ActiveBTNs();
        }
    });
}


function AddWorkCode(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "AddWorkCode?deviceId=" + id + "&dropId=" + $("#AddWorkCode").val() + "&codeNo=" + $("#AddCardEnroll").val(),
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data == 1) {
                Message(0, "عملیت با موفقیت انجام شد");
                GetWorkCodeList(id);
            }
            else if (data == 0) {
                Message(1, "عملیات انجام نشد");
            }
            else if (data == 2) {
                Message(1, "شماره کارت موجود میباشد");
            }
            $("#GetLogSpiner").fadeOut();
            ActiveBTNs();
        }
    });
}


function GetWorkCodeList(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetWorkCodeList?deviceId=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            $("#WorkCodeList").html(data);
            ActiveBTNs();
        }
    });
}

function GetCardList(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetCardList?deviceId=" + id ,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            $("#CardsList").html(data);
            ActiveBTNs();
        }
    });
}

function DeleteCard(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    Message(3, "لطفا چندلحظه صبر نماید");
                    StateButtons(true);
                    $("#RestartDeviceSpiner").fadeIn();
                    DeactiveBTNs();
                    $.ajax({
                        url: _url + "DeleteCard?cardId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف باموفقیت انجام شد");
                                GetCardList($("#DEVICEID").val());
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            ActiveBTNs();
                        }
                    });


                });
            }
        });
}

function DeleteWorkCode(id) {
    toastr.warning("<br /><br /><button type='button' id='confirmationRevertYes' class='btn btn-default clear  btn-sm'>بله</button>&nbsp;&nbsp;&nbsp;<button type='button'  class='btn clear btn-dark btn-sm'>انصراف</button>", 'آیا از حذف مطمئنید؟',
        {
            closeButton: false,
            allowHtml: true,
            progressBar: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    Message(3, "لطفا چندلحظه صبر نماید");
                    StateButtons(true);
                    $("#RestartDeviceSpiner").fadeIn();
                    DeactiveBTNs();
                    $.ajax({
                        url: _url + "DeleteWorkCode?workCodeId=" + id,
                        type: 'Get',
                        dataType: "json",
                        success: function (data) {
                            ToastOptions();
                            if (data) {
                                Message(0, "حذف باموفقیت انجام شد");
                                GetWorkCodeList($("#DEVICEID").val());
                            }
                            else {
                                Message(1, "عملیات انجام نشد");
                            }
                            ActiveBTNs();
                        }
                    });


                });
            }
        });
}

function GetLogsGrid(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetLogs?deviceId=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            $("#logdetail").html(data);
            ActiveBTNs();
        }
    });
}
function GetEnrollsGrid(id) {
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetEnrolls?deviceId=" + id,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            $("#enrolldetail").html(data);
            ActiveBTNs();
        }
    });
}
function GetEnroll(id) {
    ToastOptions();
    Message(3, "لطفا چندلحظه صبر نماید");
    StateButtons(true);
    $("#GetEnrollSpiner").fadeIn();
    DeactiveBTNs();
    $.ajax({
        url: _url + "GetEnroll?deviceId=" + id,
        type: 'Get',
        dataType: "json",
        success: function (data) {
            if (data) {
                GetEnrollsGrid(id);
                Message(0, "ارسال کاربران باموفقیت انجام شد");
            }
            else {
                Message(1, "عملیات انجام نشد");
            }
            $("#GetEnrollSpiner").fadeOut();
            StateButtons(false);
            ActiveBTNs();
        }
    });
}