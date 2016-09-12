"use strict";

var app = angular.module("services");
app.factory("userDefinedSystemConfigService", [
    "$http",
    function($http) {
        var udscServiceFactory = {};

        // Objects
        var _udscModel = {
            Id: null,
            NHoursAgo: null,
            ShiftPostStartStillValidThresholdValue: null,
            ShiftPreStartEarlyInThresholdValue: null,
            ShiftPostEndValidThresholdValue: null,
            PayrollStartDayOfMonth: null,
            PayrollEndDayOfMonth: null,
            NICFactor: null,
            AccruedHolidayFactor: null
        };


        // Data
        var _getAll = function() {
            return $http.get("api/systemconfig/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        // CRUD
        var _update = function(udscModel) {
            return $http.post("api/systemconfig/update", udscModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        udscServiceFactory.getAll = _getAll;
        udscServiceFactory.udscModel = _udscModel;
        udscServiceFactory.update = _update;

        return udscServiceFactory;
    }
]);