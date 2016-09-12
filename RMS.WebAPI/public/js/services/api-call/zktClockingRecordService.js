"use strict";

var app = angular.module("services");
app.factory("zktService", [
    "$http",
    function($http) {
        var zktServiceFactory = {};

        var _getUnmatchedZkTimeDataForEmployee = function(zktUnmatchedModel) {
            return $http.post("api/zktrecords/getunmatchedbyname/", zktUnmatchedModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        zktServiceFactory.getUnmatchedZkTimeDataForEmployee = _getUnmatchedZkTimeDataForEmployee;

        return zktServiceFactory;
    }
]);