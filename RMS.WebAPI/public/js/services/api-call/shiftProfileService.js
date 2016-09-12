"use strict";

var app = angular.module("services");
app.factory("shiftProfileService", [
    "$http",
    function($http) {
        var shiftProfileServiceFactory = {};

        var _checkApproval = function(shiftIds) {
            return $http.post("api/shiftprofile/checkapproval", shiftIds).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _consecutiveDaysWorked = function(consDaysWorkedModel) {
            return $http.post("api/shiftprofile/consecutivedaysworked", consDaysWorkedModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _consecutiveDaysWorkedByRange = function(consDaysWorkedModel) {
            return $http.post("api/shiftprofile/consecutvedaysbyrange", consDaysWorkedModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getAllForShift = function(shiftId) {
            return $http.get("api/shiftprofile/getallforshift/" + shiftId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getForId = function(shiftProfileId) {
            return $http.get("api/shiftprofile/" + shiftProfileId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getInvalidShiftProfilesForEmployee = function(employeeId) {
            return $http.get("api/shiftprofile/invalidforemployee/" + employeeId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _returnOrderedShiftProfiles = function () {
            return $http.get("api/shiftprofile/returnorderedshiftprofiles").then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _returnInvalidShiftProfiles = function() {
            return $http.get("api/shiftprofile/returninvalidshiftprofiles").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _createForUnmantchedZkt = function(shiftProfileModel) {
            return $http.post("api/shiftprofile/create/", shiftProfileModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(shiftProfileModel) {
            return $http.post("api/shiftprofile/update/", shiftProfileModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        shiftProfileServiceFactory.checkApproval = _checkApproval;
        shiftProfileServiceFactory.consecutiveDaysWorked = _consecutiveDaysWorked;
        shiftProfileServiceFactory.consecutiveDaysWorkedByRange = _consecutiveDaysWorkedByRange;
        shiftProfileServiceFactory.getAllForShift = _getAllForShift;
        shiftProfileServiceFactory.getForId = _getForId;
        shiftProfileServiceFactory.getInvalidShiftProfilesForEmployee = _getInvalidShiftProfilesForEmployee;
        shiftProfileServiceFactory.returnOrderedShiftProfiles = _returnOrderedShiftProfiles;
        shiftProfileServiceFactory.returnInvalidShiftProfiles = _returnInvalidShiftProfiles;

        shiftProfileServiceFactory.createForUnmantchedZkt = _createForUnmantchedZkt;
        shiftProfileServiceFactory.update = _update;

        return shiftProfileServiceFactory;
    }
]);