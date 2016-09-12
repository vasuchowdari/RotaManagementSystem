"use strict";

var app = angular.module("services");
app.factory("sitePersonnelLookupService", [
    "$http",
    function($http) {
        var sitePersonnelLookupServiceFactory = {};

        var _getAll = function() {
            return $http.get("api/sitepersonnellookup/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/sitepersonnellookup/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getForEmployee = function(employeeId) {
            return $http.get("api/sitepersonnellookup/getforemployee/" + employeeId).then(function (response) {
                return response;
            }).catch(function (error) {
                return error;
            });
        };

        var _getAllPlusInactiveForEmployee = function (employeeId) {
            return $http.get("api/sitepersonnellookup/getallplusinactiveforemployee/" + employeeId).then(function (response) {
                return response;
            }).catch(function (error) {
                return error;
            });
        };

        var _getForSite = function(siteId) {
            return $http.get("api/sitepersonnellookup/getforsite/" + siteId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _create = function(sitePersonnelLookupModel) {
            return $http.post("api/sitepersonnellookup/create", sitePersonnelLookupModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _createBatch = function (sitePersonnelLookupModels) {
            return $http.post("api/sitepersonnellookup/createbatch", sitePersonnelLookupModels).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(sitePersonnelLookupModel) {
            return $http.post("api/sitepersonnellookup/update", sitePersonnelLookupModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateBatch = function (sitePersonnelLookupModels) {
            return $http.post("api/sitepersonnellookup/updatebatch", sitePersonnelLookupModels).then(function (response) {
                return response;
            }).catch(function (error) {
                return error;
            });
        };

        sitePersonnelLookupServiceFactory.getAll = _getAll;
        sitePersonnelLookupServiceFactory.getById = _getById;
        sitePersonnelLookupServiceFactory.getForEmployee = _getForEmployee;
        sitePersonnelLookupServiceFactory.getAllPlusInactiveForEmployee = _getAllPlusInactiveForEmployee;
        sitePersonnelLookupServiceFactory.getForSite = _getForSite;

        sitePersonnelLookupServiceFactory.create = _create;
        sitePersonnelLookupServiceFactory.createBatch = _createBatch;
        sitePersonnelLookupServiceFactory.update = _update;
        sitePersonnelLookupServiceFactory.updateBatch = _updateBatch;

        return sitePersonnelLookupServiceFactory;
    }
]);