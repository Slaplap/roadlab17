angular.module("umbraco.resources").factory("TransactionsResource",

    function ($q, $http, umbRequestHelper, notificationsService) {

        return {
            getAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/TransactionsBackendApi/TransactionsBackendApi/GetAllMembersOfTransactionsCompany"),
                    "Failed to retrieve data");
            },
            getOneById: function (memberId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/TransactionsBackendApi/TransactionsBackendApi/GetOneMemberById?memberId=" + memberId),
                    "Failed to retrieve data");
            },
            getPaged : function (itemsPerPage, pageNumber, sortColumn, sortOrder, status, searchTerm) {
                if (sortColumn === undefined)
                    sortColumn = "";
                if (sortOrder === undefined)
                    sortOrder = "";
                return $http.get("/Umbraco/Backoffice/TransactionsBackendApi/TransactionsBackendApi/GetPaged?itemsPerPage=" +
                    itemsPerPage +
                    "&pageNumber=" +
                    pageNumber +
                    "&sortColumn=" +
                    sortColumn +
                    "&sortOrder=" +
                    sortOrder +
                    "&status=" +
                    status +
                    "&searchTerm=" +
                    searchTerm);
            }
        };
    }
);
