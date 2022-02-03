
    var fileApiService = function ($http) {

        var fileApiFactory = {};

        /**
         * @ngdoc method
         * @name importFile
         * @function
         * 
         * @param {file} file - File object acquired via File Upload API.
         * @description - Upload a file to the server.
         */
        fileApiFactory.uploadFileToServer = function (file) {
            debugger;
            var request = {
                file: file,
                memberId: "hhhhhh"
            };
            return $http({
                method: 'POST',
                url: "/umbraco/api/FileUploadApi/UploadFileToServer",
                // If using Angular version <1.3, use Content-Type: false.
                // Otherwise, use Content-Type: undefined
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("file", data.file);
                    formData.append("memberId", data.memberId);
                    return formData;
                },
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

        return fileApiFactory;

    };

    angular.module("umbraco").factory('fileApiService', fileApiService);

