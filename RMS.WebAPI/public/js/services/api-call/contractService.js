"use strict";

var app = angular.module("services");
app.factory("contractService", [
    "$http",
    function ($http) {
        var contractServiceFactory = {};

        var _getContractForEmployee = function(employeeId) {
            return $http.get("api/contract/getcontractforemployee/" + employeeId).then(function(response) {
                return response;
            }, function(err) {
                return err.data.message;
            });
        };

        var _update = function(contractModel) {
            return $http.post("api/contract/update", contractModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        contractServiceFactory.getContractForEmployee = _getContractForEmployee;
        contractServiceFactory.update = _update;

        return contractServiceFactory;
    }
]);