angular.module("umbraco.resources").factory("NotificationsResource",

    function ($q, $http, umbRequestHelper, notificationsService) {

        return {
            getAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/NotificationsBackendApi/NotificationsBackendApi/GetAllNotifications"),
                    "Failed to retrieve data");
            },
            getOneById: function (memberId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/NotificationsBackendApi/NotificationsBackendApi/GetOneMemberById?memberId=" + memberId),
                    "Failed to retrieve data");
            },
            getPaged : function (itemsPerPage, pageNumber, sortColumn, sortOrder, status, searchTerm) {
                if (sortColumn === undefined)
                    sortColumn = "";
                if (sortOrder === undefined)
                    sortOrder = "";
                return $http.get("/Umbraco/Backoffice/NotificationsBackendApi/NotificationsBackendApi/GetPaged?itemsPerPage=" +
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
