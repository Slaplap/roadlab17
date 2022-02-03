angular.module("umbraco")
    .controller("TransactionController", function ($scope, $routeParams, TransactionResource, TransactionsResource, $location, notificationsService, fileApiService) {
        $scope.file = false;
        $scope.isUploading = false;
        $scope.transactionId = $location.search().transactionId;
        $scope.statusus = [
            "Quote Local", "Quote Open", "Quote Ready", "Quote Accepted", "Quote Rejected", "Quote Cancelled", "Quote Closed",
            "Order Closed", "Order Open", "Order Cancelled",
            "Booking Local", "Booking Ready", "Booking Open", "Booking Accepted", "Booking Rejected", "Booking Cancelled", "Booking Closed",
            "Test Open","Test Ready","Test Cancelled","Test Pending","Test Closed"];
        $scope.selectedItemvalue = "";
        var d = new Date();
        $scope.kkk = d.getMilliseconds();
        
        TransactionResource.getOneById($scope.transactionId).then(function (response) {
          
            $scope.model = response;
            $scope.model.commentsObject = JSON.parse($scope.model.comments);
            $scope.selectedItemvalue = response.status;


        });

        $scope.save = function(value) {
            
            
            TransactionResource.saveStatusById($scope.transactionId, value).then(function(response) {
                $scope.model = response;
                $scope.selectedItemvalue = response.status;
                notificationsService.success("Saved", "Saved Status Update");
            });
        };
        $scope.Select = function(x) {
            
            $scope.selectedItemvalue = x;
        };

        $scope.acceptSelectedFile = function (files) {
            debugger;
            if (files.length > 0) {
                $scope.file = files[0];
            }
        };

        $scope.uploadFile = function () {
            debugger;
            if (!$scope.isUploading) {
                if ($scope.file) {
                    $scope.isUploading = true;
                    var promise = fileApiService.uploadFileToServer($scope.file);
                    promise.then(function (response) {
                        if (response) {
                            console.info('Saved to server with the filename ' + response);
                        }
                        $scope.isUploading = false;
                    }, function (reason) {
                        console.info("File import failed.");
                        console.info(reason.message);
                        $scope.isUploading = false;
                    });
                } else {
                    console.info("Must select a file to import.");
                    $scope.isUploading = false;
                }
            }
        };
    });
