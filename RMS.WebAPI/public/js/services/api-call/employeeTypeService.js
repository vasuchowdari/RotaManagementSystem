"use strict";

var app = angular.module("services");
app.factory("employeeTypeService", [
    "$http",
    function ($http) {
        var employeeTypeServiceFactory = {};

        var _generateNewEmployeeTypeModel = function() {
            var employeeTypeModel = {
                Name: null,
                IsActive: true
            };

            return employeeTypeModel;
        };

        var _getAll = function () {
            return $http.get("api/employeetype/getall").then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/employeetype/" + id).then(function (response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _create = function (employeeTypeModel) {
            return $http.post("api/employeetype/create", employeeTypeModel).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        var _update = function(employeeTypeModel) {
            return $http.put("api/employeetype/update", employeeTypeModel).then(function (response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        employeeTypeServiceFactory.generateNewSystemAccessRoleModel = _generateNewEmployeeTypeModel;
        employeeTypeServiceFactory.getAll = _getAll;
        employeeTypeServiceFactory.getById = _getById;
        employeeTypeServiceFactory.create = _create;
        employeeTypeServiceFactory.update = _update;

        return employeeTypeServiceFactory;
    }
]);