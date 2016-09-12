"use strict";

var app = angular.module("services");
app.factory("shiftService", [
    "$http",
    function($http) {
        var shiftServiceFactory = {};

        var _generateNewShiftModel = function() {
            var model = {
                IsActive: true,
                IsAssigned: false,
                StartTime: moment(),
                EndTime: moment(),
                Duration: 0,
                UnpaidBreakDuration: null,
                ShiftTypeId: 1,
                CalendarPeriodId: null,
                ResourceId: 1
            };

            return model;
        };

        var _getForCalendarPeriod = function(calendarPeriodId) {
            return $http.get("api/shift/getforperiod/" + calendarPeriodId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getForEmployee = function(employeeId) {
            return $http.get("api/shift/getforemployee/" + employeeId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _unassignEmployeeFromFutureShifts = function(employeeId) {
            return $http.get("api/shift/unassignemployee/" + employeeId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _checkIfStaffAssignedToShiftByDateRange = function(dataModel) {
            return $http.post("api/shift/checkStaffByDate", dataModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getShiftLocationName = function(shiftId) {
            return $http.get("api/shift/getshiftlocationname/" + shiftId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(shiftId) {
            return $http.get("api/shift/" + shiftId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        // CRUD
        var _create = function(shiftModel) {
            return $http.post("api/shift/create", shiftModel).then(function(response) {
                return response;
            }, function(err) {
                return err;
            });
        };

        var _delete = function(shiftId) {
            return $http.post("api/shift/delete/" + shiftId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        // THIS IS A POST RATHER THAN PUT AS NOUVITA WEB SERVER WAS
        // DENYING ACCESS WITH PUT COMMAND. POST WORKED, SO POST IT IS
        var _update = function(shiftModel) {
            return $http.post("api/shift/update", shiftModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        shiftServiceFactory.create = _create;
        shiftServiceFactory.delete = _delete;
        shiftServiceFactory.update = _update;

        shiftServiceFactory.checkIfStaffAssignedToShiftByDateRange = _checkIfStaffAssignedToShiftByDateRange;
        shiftServiceFactory.generateNewShiftModel = _generateNewShiftModel;
        shiftServiceFactory.getById = _getById;
        shiftServiceFactory.getForCalendarPeriod = _getForCalendarPeriod;
        shiftServiceFactory.getForEmployee = _getForEmployee;
        shiftServiceFactory.getShiftLocationName = _getShiftLocationName;
        shiftServiceFactory.unassignEmployeeFromFutureShifts = _unassignEmployeeFromFutureShifts;

        return shiftServiceFactory;
    }
]);