var appSettings = {};
var addIndex = 0;
var idx = 0;
var testArray = [];
$(document).ready(function () {
    LoadDetailsModal();
    RejectReason();
    SelectCM();
   // AddTests();
   // LoadTests();
    window.setInterval(RejectReason, 1000);
    $("#contactform").bind("submit", disableButtons);
    $("#vacancyform").bind("submit", disableButtons);
});

function AddTests() {
    $('#addTestButton').on('click',
        function () {
            $('#testModal').modal('show');
        });
}
function LoadTests() {

    var url = "/umbraco/api/AppSettingsApi/GetAppSettings";
    $.get(url, function (data, status) {

        if (data === false) {

            $.toast({
                heading: 'Error',
                text: 'Could not load App Settings',
                icon: 'error',
                position: 'mid-center'
            });
        } else {
            appSettings = data;
            for (var i = 0; i < data.TestCategoriesDtos.length; i++) {
                var categoryId = data.TestCategoriesDtos[i].Id;
                var categoryText = data.TestCategoriesDtos[i].Name;
                $("#dropdownCategory").append("<option value='" + categoryId + "' >" + categoryText + "</option>");
            }
            for (var i = 0; i < data.TestDtos.length; i++) {
                if (data.TestDtos[i].TestCategoryId == data.TestCategoriesDtos[0].Id) {
                    var testId = data.TestDtos[i].Id;
                    var testText = data.TestDtos[i].Name;
                    $("#dropdownTest").append("<option value='" + testId + "' >" + testText + "</option>");
                }
            }


        }

    });
    $('#dropdownCategory').on('change', function (sender) {

        var categoryId = $('#dropdownCategory').val();
        $('#dropdownTest')
            .find('option')
            .remove()
            .end();

        for (var i = 0; i < appSettings.TestDtos.length; i++) {
            if (appSettings.TestDtos[i].TestCategoryId == categoryId) {
                var testId = appSettings.TestDtos[i].Id;
                var testText = appSettings.TestDtos[i].Name;
                $("#dropdownTest").append("<option value='" + testId + "' >" + testText + "</option>");
            }
        }
    });
    $("#buttonAdd").on("click",
        function () {
            var categoryId = $('#dropdownCategory').val();
            var testId = $('#dropdownTest').val();
            var qty = $('#qty').val();
            var category = $('#dropdownCategory option:selected').html();
            var test = $('#dropdownTest option:selected').html();

            idx = testArray.length;
            var testItem = {
                categoryId: categoryId,
                testId: testId,
                category: category,
                test: test,
                qty: qty,
                idx: idx

            }

            testArray.push(testItem);

            loadTestRows(testArray);
        });
}

function loadTestRows(_testArray) {
    $("#testList").empty();
    for (var i = 0; i < _testArray.length; i++) {


        $("#testList").append("<li class='row testrow testrow-" + i + "'>" +
            "<input name='TransactionLine[" + i + "].CategoryId' type='hidden' value='" + _testArray[i].categoryId + "'/>" +
            "<input name='TransactionLine[" + i + "].TestId' type='hidden' value='" + _testArray[i].testId + "'/>" +
            "<input name='TransactionLine[" + i + "].Qty' type='hidden' value='" + _testArray[i].qty + "'/>" +
            "<div class='col-md-3'>" + _testArray[i].category + "</div>" +
            "<div class='col-md-3'>" + _testArray[i].test + "</div>" +
            "<div class='col-md-3'>" + _testArray[i].qty + "</div>" +

            "<button  class='btn btn-black'  type='button' onclick='deleteLine(" + i + ")' >Delete</button>" +
            "</li>");
    }
}
function arrayRemove(arr, value) {
    return arr.filter(function (ele) { return ele != value; });

}
function deleteLine(i) {
    testArray = testArray.slice(0, i).concat(testArray.slice(i + 1, testArray.length));
    loadTestRows(testArray);
}
function RejectReason() {
    var messageBox = $('#rejectText');
    var rejectButton = $('#rejectButton');
    if (messageBox.val() === "") {
        rejectButton.prop('disabled', true);
    } else {
        rejectButton.prop('enabled', true);
    }
}
function SelectCM() {
    $('#selectCM').on('change', function () {
        window.open("/account/secure-area/List?type=" + this.value);
    });
}
var account = {
    getOtp: function () {
        var url = "/umbraco/api/MemberApi/RequestOTP?email=" + $('#Email').val();
        $.get(url, function (data, status) {
            if (data === false) {
                $.toast({
                    heading: 'Error',
                    text: 'Please enter an valid email',
                    icon: 'error',
                    position: 'mid-center'
                });
            }
            $(".otp").removeClass("fa-spin");

        });
        $(".otp").addClass("fa-spin");
    }
}

function LoadDetailsModal() {

    $('.openDetails').click(function () {
        debugger;
        var key = $(this).data('key');
        onGridClick(key);
        // AJAX request

    });
};

function onGridClick(obj) {


    var key = $(obj).data("key");
    $.ajax({
        url: '/account/secure-area/ajax?key=' + key + '&partial=_RenderTransactionDetails',
        type: 'get',
        success: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');

        },
        fail: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');
        }
    });
}
function onReadClick(obj) {


    var key = $(obj).data("key");
    $.ajax({
        url: '/account/secure-area/ajax?key=' + key + '&partial=_RenderTransactionDetails',
        type: 'get',
        success: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');

        },
        fail: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');
        }
    });
}
function onNotificationsViewClick(obj) {


    var key = $(obj).data("foreignkey");

    $.ajax({
        url: '/account/secure-area/ajax?key=' + key + '&partial=_RenderTransactionDetails',
        type: 'get',
        success: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');

        },
        fail: function (response) {

            $('.modal-body').html(response);

            // Display Modal
            $('#detailsModal').modal('show');
        }
    });
}
function onReadClick(obj) {

    var key = $(obj).data("key");
    var gridInstance = document.getElementById("LocalData").ej2_instances[0];
    var record = {};

    for (var i = 0; i < gridInstance.dataSource.length; i++) {
        if (gridInstance.dataSource[i].Key == key) {
            record = gridInstance.dataSource[i];
        }
    }
    gridInstance.deleteRecord("Key", record);
    var param = JSON.stringify({ notificationKey: key  });
    $.ajax({
        url: '/umbraco/api/NotificationsFrontendApi/SetNotificationToRead?notificationKey=' + key,
        type: "post"
        
        
    });
} 

function disableButtons() {

    var form = $(this);
    var btns = $("input:submit", form);

    if (!form.valid()) {
        // allow user to correct validation errors and re-submit
        btns.removeAttr("disabled");
    } else {
        btns.attr("disabled", "disabled");
    }
}



