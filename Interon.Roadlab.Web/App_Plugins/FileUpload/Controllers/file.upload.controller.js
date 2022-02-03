 
 

    var fileUploadController = function ($scope, fileApiService) {

        /*-------------------------------------------------------------------
         * Initialization Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name init
         * @function
         * 
         * @description - Called when the $scope is initalized.
         */
        $scope.init = function () {
            $scope.setVariables();
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial states of the $scope variables.
         */
        $scope.setVariables = function () {
			$scope.file = false;
			$scope.isUploading = false;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name fileSelected
         * @function
         * 
         * @param {array of file} files - One or more files selected by the HTML5 File Upload API.
         * @description - Get the file selected and store it in scope. This current example restricts the upload to a single file, so only take the first.
         */
        $scope.acceptSelectedFile = function (files) {
            debugger;
            if (files.length > 0) {
                $scope.file = files[0];
            }
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name uploadFile
         * @function
         * 
         * @description - Uploads a file to the backend.
         */
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

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module("umbraco").controller('FileUploadController', ['$scope', 'fileApiService', fileUploadController]);
 