angular.module("umbraco")
    .controller("NotificationsController", function ($scope,   NotificationsResource, notificationsService ) {
       
        $scope.test = "test";
        $scope.selectedIds = [];
        $scope.statusus = ["Quote Open", "Quote Pending", "Quote Ready","Quote Accepted","Quote Rejected", "Order Open","Order Pending","Booking Open","Booking Closed"];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 50;
        $scope.totalPages = 1;
     
        $scope.reverse = true;

        $scope.statusFilter = "";
        $scope.searchTerm = "";
        $scope.predicate = 'UpdateDate';
        $scope.loading = true;
        
    


        function fetchData() {
            $scope.loading = true;
            NotificationsResource.getPaged($scope.itemsPerPage, $scope.currentPage, $scope.predicate, $scope.reverse ? "desc" : "asc", $scope.statusFilter, $scope.searchTerm).then(function (response) {
                 
                $scope.data = response.data.data;
                console.log($scope);
                $scope.totalPages = response.data.totalPages;
                $scope.loading = false;
            }, function (response) {
                notificationsService.error("Error", "Could not load Notifications list");
            });
        };

        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
            $scope.currentPage = 1;
            fetchData();
        };

        $scope.toggleSelection = function (val) {
            var idx = $scope.selectedIds.indexOf(val);
            if (idx > -1) {
                $scope.selectedIds.splice(idx, 1);
            } else {
                $scope.selectedIds.push(val);
            }
        };

        $scope.isRowSelected = function (id) {
            return $scope.selectedIds.indexOf(id) > -1;
        };

        $scope.isAnythingSelected = function () {
            return $scope.selectedIds.length > 0;
        };

        $scope.prevPage = function () {
            if ($scope.currentPage > 1) {
                $scope.currentPage--;
                fetchData();
            }
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.currentPage++;
                fetchData();
            }
        };

        $scope.setPage = function (pageNumber) {
            $scope.currentPage = pageNumber;
            fetchData();
        };

        $scope.search = function (searchFilter) {
            $scope.searchTerm = searchFilter;
            $scope.currentPage = 1;
            fetchData();
        };

        $scope.filter = function (statusFilter) {
            $scope.statusFilter = statusFilter;
            $scope.currentPage = 1;
            fetchData();
        };
        fetchData();
        console.log($scope);
    });
