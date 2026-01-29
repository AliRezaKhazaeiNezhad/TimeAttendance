function AddLoading() {
    $("body").attr("overflow-x", "none");
    $("body").attr("overflow", "hidden");
    $("#loading").fadeIn();
}
function ReomveLoading() {
    $("#loading").fadeOut("slow");
    $("body").attr("overflow-x", "scroll");
}
function GridReload(id) {
    $('#' + id).DataTable().ajax.reload();
}

$(document).ajaxComplete(function () {
    ReomveLoading();
});

$(".link").click(function () {
    AddLoading();
});


function Message(state, message) {
    ToastOptions();
    if (state == "0") {
        toastr.success(message);
    }
    if (state == "1") {
        toastr.warning(message);
    }
    if (state == "2") {
        toastr.error(message);
    }
    if (state == "3") {
        toastr.info(message);
    }
}

function GetEmptyView(url, id) {
    $.ajax({
        url: url,
        type: 'get',
        dataType: "html",
        success: function (data) {
            $("#" + id).html(data);
            blink(id);
        }
    });
}

function blink(id) {
    $("#" + id).addClass("blink_me");
    $('.blink_me').fadeOut(500).fadeIn(500, blink);
    $("#" + id).removeClass("blink_me");
}

function ToastOptions() {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-center",
        //"positionClass": "toast-top-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "3000",
        "timeOut": "3000",
        "extendedTimeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function Delete(url) {
    ToastOptions();
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
function DeleteType3(url) {
    ToastOptions();
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
                            if (data == 1) {
                                Message(0, "حذف با موفقیت انجام شد");
                                GridReload("Grid");
                            }
                            else if (data == -2) {
                                Message(1, "حذف انجام نشد! درفرصتی دیگر تلاش نمایید");
                            }
                            else if (data == -1) {
                                Message(2, "این داده در جای دیگر استفاده شده و امکان حذف ندارد");
                            }
                            else if (data == -4) {
                                Message(2, "امکان حذف وجود ندارد");
                            }

                        }
                    });

                    $('#Grid').DataTable().ajax.reload();
                    $('#Grid2').DataTable().ajax.reload();
                });
            }
        });
}

function getEdit(_url) {
    $.ajax({
        url: _url,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            ToastOptions();
            $("#AddOrUpdate-Part").html(data);
        }
    });
}

function isNull(val) {
    if (val.length <= 0) {
        return false;
    }
    else {
        return true;
    }
}





(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));


$(".numeric").inputFilter(function (value) {
    return /^\d*$/.test(value);
});
function DataTableReload() {
    var table = $('#Grid').DataTable();
    table.ajax.reload();
}
function HideBootbox() {
    bootbox.hideAll();
}



function ActiveMenu(menu, data) {
    $(".page-header-inner a").removeClass("activepanel");
    $(data).addClass("activepanel");

    $(".customeLi").hide();
    $(".menu" + menu).fadeIn();
}
function getDescription(_url) {
    $.ajax({
        url: _url,
        type: 'Get',
        dataType: "html",
        success: function (data) {
            bootbox.dialog({
                message: data,
                closeButton: true
            })
        }
    });
}

function formatTime(timeInput) {

    intValidNum = timeInput.value;
    if (intValidNum < 24) {
        if (intValidNum.length == 2) {
            timeInput.value = timeInput.value + ":";
            return false;
        }
    }
    if (intValidNum == 24) {
        if (intValidNum.length == 2) {
            timeInput.value = timeInput.value.length - 2 + "0:";
            return false;
        }
    }
    if (intValidNum > 24) {
        if (intValidNum.length == 2) {
            timeInput.value = "";
            return false;
        }
    }


    if (intValidNum.length == 5 && intValidNum.slice(-2) < 60) {
        //timeInput.value = timeInput.value + ":";
        return false;
    }
    //if (intValidNum.length == 5 && intValidNum.slice(-2) > 60) {
    //    timeInput.value = timeInput.value.slice(0, 2) + ":";
    //    return false;
    //}
    if (intValidNum.length == 5 && intValidNum.slice(-2) == 60) {
        timeInput.value = timeInput.value.slice(0, 2) + "00";
        return false;
    }


    //if (intValidNum.length == 8 && intValidNum.slice(-2) > 60) {
    //    timeInput.value = timeInput.value.slice(0, 5) + ":";
    //    return false;
    //}
    //if (intValidNum.length == 8 && intValidNum.slice(-2) == 60) {
    //    timeInput.value = timeInput.value.slice(0, 5) + ":00";
    //    return false;
    //}
}


$(document).ready(function () {
    $(":input").attr("autocomplete", "off");

    $(document).keyup(function (e) {
        if (e.keyCode == 27) { bootbox.hideAll(); }
    });
    $(".bootbox-close-button").on("click", function () {
        HideBootbox();
    });
});

