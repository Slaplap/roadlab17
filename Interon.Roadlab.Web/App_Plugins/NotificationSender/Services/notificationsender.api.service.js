
    var notificationApiService = function ($http) {

        var notificationApiFactory = {};

       
        notificationApiFactory.SendNotificationToMember = function (memberId, message) {
            debugger;
            var request = {
                memberId: memberId,
                message:message
            };
            return $http({
                method: 'POST',
                url: "/umbraco/backoffice/NotificationSender/NotificationHubBackendApi/SendNotificationToMember",
                data: request
            }).then(function (response) {
                if (response) {
                    var fileName = response.data;
                    return fileName;
                } else {
                    return false;
                }
            });
        };

        return notificationApiFactory;

    };

angular.module("umbraco").factory('notificationApiService', notificationApiService);

