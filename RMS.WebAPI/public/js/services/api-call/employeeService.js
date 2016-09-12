"use strict";

var app = angular.module("services");
app.factory("employeeService", [
    "$http",
    function($http) {
        var employeeServiceFactory = {};

        var _create = function(employeeModel) {
            return $http.post("api/employee/create", employeeModel).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _getAll = function() {
            return $http.get("api/employee/getall").then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _getById = function(id) {
            return $http.get("api/employee/" + id).then(function(response) {
                return response;
            }, function(err) {
                return err.data.message;
            });
        };

        var _getByUserId = function(userId) {
            return $http.get("api/employee/getbyuserid/" + userId).then(function(response) {
                return response;
            }, function(err) {
                return err.data.message;
            });
        };

        var _getPersonalCalendarObjectsByDateRange = function(staffCalObjByDateRangeModel) {
            return $http.post("api/employee/getpersonalcalendarobjectsbydaterange", staffCalObjByDateRangeModel).then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _generateNewEmployeeModel = function() {
            var model = {
                IsActive: true,
                CompanyId: null,
                EmployeeId: null,
                Address: null,
                City: null,
                Postcode: null,
                Telephone: null,
                Mobile: null,
                GenderId: null,
                BaseSiteId: null,
                BaseSubSiteId: null,
                UserId: null,
                UserModel: {}, //needed?
                ContractModels: []
            };

            return model;
        };

        var _getAllByCompanyId = function(companyId) {
            return $http.get("api/employee/getallforcompany/" + companyId).then(function(response) {
                return response;
            }).catch(function(err) {
                return err;
            });
        };

        var _getByCompanyId = function (companyId) {
            return $http.get("api/employee/getforcompany/" + companyId).then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };

        var _getEmployeeNameAndId = function() {
            return $http.get("api/employee/getemployeenameandid").then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(employeeModel) {
            return $http.post("api/employee/update", employeeModel).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _checkIfExists = function(userId) {
            return $http.get("api/employee/checkifexists/" + userId).then(function(response) {
                return response;
            }, function (err) {
                return err.data.message;
            });
        };

        employeeServiceFactory.create = _create;
        employeeServiceFactory.getAll = _getAll;
        employeeServiceFactory.getById = _getById;
        employeeServiceFactory.getByUserId = _getByUserId;
        employeeServiceFactory.getAllByCompanyId = _getAllByCompanyId;
        employeeServiceFactory.getByCompanyId = _getByCompanyId;
        employeeServiceFactory.getPersonalCalendarObjectsByDateRange = _getPersonalCalendarObjectsByDateRange;
        employeeServiceFactory.generateNewEmployeeModel = _generateNewEmployeeModel;
        employeeServiceFactory.getEmployeeNameAndId = _getEmployeeNameAndId;
        employeeServiceFactory.update = _update;

        employeeServiceFactory.checkIfExists = _checkIfExists;


        return employeeServiceFactory;
    }
]);