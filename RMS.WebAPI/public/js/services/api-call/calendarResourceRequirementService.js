"use strict";

var app = angular.module("services");
app.factory("calResRqService", [
    "$http",
    function($http) {
        var calPeriodResRqServiceFactory = {};

        var _checkResourceAllocationLimits = function(rotaModel) {
            return $http.post("api/resourcerequirement/checkresourceallocationlimits", rotaModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getAll = function() {
            return $http.get("api/resourcerequirement/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/resourcerequirement/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getForCalendar = function (calendarId, startDate, endDate) {
            return $http.get("api/resourcerequirement/calendar/" + calendarId + "/" + startDate + "/" + endDate).then(function (response) {
                return response;
            }).catch(function (error) {
                return error;
            });
        };

        var _create = function(rotaModel) {
            return $http.post("api/resourcerequirement/create", rotaModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        // THIS IS A POST RATHER THAN PUT AS NOUVITA WEB SERVER WAS
        // DENYING ACCESS WITH PUT COMMAND. POST WOORKED, SO POST IT IS
        var _update = function(rotaModel) {
            return $http.post("api/resourcerequirement/update", rotaModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _delete = function(id) {
            return $http.post("api/resourcerequirement/delete/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        calPeriodResRqServiceFactory.checkResourceAllocationLimits = _checkResourceAllocationLimits;
        calPeriodResRqServiceFactory.getAll = _getAll;
        calPeriodResRqServiceFactory.getById = _getById;
        calPeriodResRqServiceFactory.getForCalendar = _getForCalendar;
        calPeriodResRqServiceFactory.create = _create;
        calPeriodResRqServiceFactory.update = _update;
        calPeriodResRqServiceFactory.delete = _delete;

        return calPeriodResRqServiceFactory;
    }
]);