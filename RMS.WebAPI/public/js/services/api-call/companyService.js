"use strict";

var app = angular.module("services");
app.factory("companyService", [
    "$http",
    function ($http) {
        var companyServiceFactory = {};

        var _generateNewCompanyModel = function() {
            var companyModel = {
                Name: null,
                IsActive: true
            };

            return companyModel;
        };

        var _getAll = function () {
            return $http.get("api/company/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/company/" + id).then(function(response) {
                return response;
            }, function(err) {
                return err.data.message;
            });
        };

        var _getByIdWithEmployees = function (id) {
            return $http.get("api/company/withemployees/" + id).then(function (response) {
                return response;
            }, function (err) {
                return err.data.message;
            });
        };

        var _create = function (companyModel) {
            return $http.post("api/company/create", companyModel).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        var _update = function(companyModel) {
            return $http.put("api/company/update", companyModel).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        companyServiceFactory.generateNewCompanyModel = _generateNewCompanyModel;
        companyServiceFactory.getAll = _getAll;
        companyServiceFactory.getById = _getById;
        companyServiceFactory.getByIdWithEmployees = _getByIdWithEmployees;
        companyServiceFactory.create = _create;
        companyServiceFactory.update = _update;

        return companyServiceFactory;
    }
]);