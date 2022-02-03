


var notificationSenderController = function ($scope, notificationsService, editorState, entityResource, notificationApiService ) {

    
    $scope.init = function () {
         
        $scope.setVariables();
    };


    $scope.setVariables = function () {
         
        $scope.message = "";
      
            


        };



    $scope.sendMessage = function () {

        debugger;
        var promise = notificationApiService.SendNotificationToMember(editorState.current.id, $scope.message);
        promise.then(function (response) {
            if (response) {
                notificationsService.success("Sent", "It was successful");
              
            }
            $scope.isUploading = false;
        }, function (reason) {
                notificationsService.warning("Not Sent", "It was not successful" + reason.message );
            console.info(reason.message);

        });


    };



    $scope.init();

};

angular.module("umbraco").controller('NotificationSenderController', ['$scope', 'notificationsService', 'editorState', 'entityResource','notificationApiService', notificationSenderController]);
