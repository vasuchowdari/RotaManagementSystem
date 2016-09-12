"use strict";

var app = angular.module("services");
app.factory("leaveRequestService", [
    "$http",
    function($http) {
        var leaveRequestServiceFactory = {};

        var _generateDeleteLeaveViewModel = function() {
            var vm = {
                startDateTime: null,
                endDateTime: null,
                leaveTypeStr: null
            };

            return vm;
        };

        var _generateNewLeaveRequestModel = function() {
            var leaveRequestModel = {
                Id: 0,
                StartDateTime: moment(),
                EndDateTime: moment(),
                AmountRequested: 0,
                EmployeeId: null,
                LeaveTypeId: null,
                Notes: null,
                IsApproved: true,
                IsTaken: false,
                IsActive: true
            };

            return leaveRequestModel;
        };

        var _getLeaveRequestsForEmployee = function(id) {
            return $http.get("api/leaverequest/getforemployee/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getLeaveRequestsForIds = function(staffIds) {
            return $http.post("api/leaverequest/getforids", staffIds).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        // Repo
        var _adminCreate = function(leaveRequestModel) {
            return $http.post("api/leaverequest/admin/create", leaveRequestModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _adminDelete = function(leaveRequestId) {
            return $http.get("api/leaverequest/delete/" + leaveRequestId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _adminUpdate = function(leaveRequestModel) {
            return $http.post("api/leaverequest/admin/update", leaveRequestModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        leaveRequestServiceFactory.generateDeleteLeaveViewModel = _generateDeleteLeaveViewModel;
        leaveRequestServiceFactory.generateNewLeaveRequestModel = _generateNewLeaveRequestModel;
        leaveRequestServiceFactory.getLeaveRequestsForEmployee = _getLeaveRequestsForEmployee;
        leaveRequestServiceFactory.getLeaveRequestsForIds = _getLeaveRequestsForIds;

        leaveRequestServiceFactory.adminCreate = _adminCreate;
        leaveRequestServiceFactory.adminDelete = _adminDelete;
        leaveRequestServiceFactory.adminUpdate = _adminUpdate;

        return leaveRequestServiceFactory;
    }
]);