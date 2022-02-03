angular.module("umbraco.resources").factory("TransactionResource",

    function ($q, $http, umbRequestHelper, notificationsService) {

        return {
            getOneById: function (transactionKey) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/TransactionsBackendApi/TransactionsBackendApi/GetTransactionById?transactionKey=" + transactionKey),
                    "Failed to retrieve data");
            },
            saveStatusById: function(transactionKey, status) {
                 
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/TransactionsBackendApi/TransactionsBackendApi/SaveStatusById?transactionKey=" + transactionKey + "&status=" + status),
                    "Failed to retrieve data");
            }
        };
    }
);