var programType = null;
var _state = false;

/*  انتخاب نوع برنامه کاری  */
function SelectWP(data) {
    var $val = $(data).val();

    if ($val == 1 || $val == 3) {
        $("#flowprogram").fadeOut();
        $("#otherprogram").show();
        _state = false;
    }
    if ($val == 2) {
        $("#otherprogram").fadeOut();
        $("#flowprogram").show();
        _state = true;
    }
    if ($val == 0) {
        $("#otherprogram").fadeOut();
        $("#flowprogram").fadeOut();
        _state = false;
    }

    ShowHideSubmit(_state);
}

/*  درصورت خالی نبودن فیلد عنوان، مقدار دراپ نوع برنامه کاری را نمایش میدهد */
function isTitleNull(data) {
    var $val = $(data).val();
    if ($val.length > 0) {
        $("#submitPart").removeClass("hide");
    }
    else {
        $("#submitPart").addClass("hide");
    }

    if (programType) {

    }
}



