angular.module("umbraco.resources").factory("NotificationResource",

    function ($q, $http, umbRequestHelper, notificationsService) {

        return {
            getOneById: function (NotificationId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/NotificationsBackendApi/NotificationsBackendApi/GetNotificationById?NotificationId=" + NotificationId),
                    "Failed to retrieve data");
            },
            saveStatusById: function(orderId, status) {
                 
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/NotificationsBackendApi/NotificationsBackendApi/SaveStatusById?orderId=" + NotificationId + "&status=" + status),
                    "Failed to retrieve data");
            }
        };
    }
);