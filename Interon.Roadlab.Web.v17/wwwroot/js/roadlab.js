$(document).ready(function () {
    $("#contactform").bind("submit", disableButtons);
    $("#vacancyform").bind("submit", disableButtons);
});

function disableButtons() {
    var form = $(this);
    if (form.find("[type='submit']").attr("disabled")) {
        return false;
    }
    form.find("[type='submit']").attr("disabled", "disabled");
    return true;
}
