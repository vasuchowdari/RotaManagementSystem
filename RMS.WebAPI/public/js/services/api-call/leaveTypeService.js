"use strict";

var app = angular.module("services");
app.factory("leaveTypeService", [
    "$http",
    function($http) {
        var leaveTypeServiceFactory = {};

        var _getAll = function() {
            return $http.get("api/leavetype/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        leaveTypeServiceFactory.getAll = _getAll;

        return leaveTypeServiceFactory;
    }
]);