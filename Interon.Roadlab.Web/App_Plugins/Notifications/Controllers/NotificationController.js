angular.module("umbraco")
    .controller("NotificationController", function($scope, $routeParams, NotificationResource, NotificationsResource, $location, notificationsService) {
         
        $scope.orderId = $location.search().orderId;
        $scope.statusus = [
            "Quote Open", "Quote Pending", "Quote Ready", "Quote Accepted", "Quote Rejected", "Quote Cancelled", "Quote Closed",
            "Order Open", "Order Pending", "Order Cancelled",
            "Booking Open","Booking Ready","Booking Pending","Booking Accepted","Booking Rejected","Booking Cancelled", "Booking Closed"];
        $scope.selectedItemvalue = "";
        var d = new Date();
        $scope.kkk = d.getMilliseconds();
        
        NotificationResource.getOneById($scope.orderId).then(function (response) {
            debugger;
            $scope.model = response;
            $scope.selectedItemvalue = response.status;


        });

        $scope.save = function(value) {
            
            
            NotificationResource.saveStatusById($scope.orderId, value).then(function(response) {
                $scope.model = response;
                $scope.selectedItemvalue = response.status;
                notificationsService.success("Saved", "Saved Status Update");
            });
        };
        $scope.Select = function(x) {
            debugger;
            $scope.selectedItemvalue = x;
        };
    });
