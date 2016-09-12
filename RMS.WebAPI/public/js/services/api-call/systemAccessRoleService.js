"use strict";

var app = angular.module("services");
app.factory("systemAccessRoleService", [
    "$http",
    function ($http) {
        var systemAccessRoleServiceFactory = {};

        var _generateNewSystemAccessRoleModel = function() {
            var systemAccessRoleModel = {
                Name: null,
                IsActive: true
            };

            return systemAccessRoleModel;
        };

        var _getAll = function() {
            return $http.get("api/systemaccessrole/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/systemaccessrole/" + id).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _create = function (systemAccessRoleModel) {
            return $http.post("api/systemaccessrole/create", systemAccessRoleModel).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        var _update = function(systemAccessRoleModel) {
            return $http.put("api/systemaccessrole/update", systemAccessRoleModel).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        systemAccessRoleServiceFactory.generateNewSystemAccessRoleModel = _generateNewSystemAccessRoleModel;
        systemAccessRoleServiceFactory.getAll = _getAll;
        systemAccessRoleServiceFactory.getById = _getById;
        systemAccessRoleServiceFactory.create = _create;
        systemAccessRoleServiceFactory.update = _update;

        return systemAccessRoleServiceFactory;
    }
]);