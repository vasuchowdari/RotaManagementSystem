"use strict";

var app = angular.module("services");
app.factory("shiftTemplateService", [
    "$http",
    function($http) {
        var shiftTemplateServiceFactory = {};

        var _returnNewShiftTemplateModel = function() {
            var model = {
                Name: null,
                ShiftRate: null,
                ResourceId: null,
                SiteId: null,
                SubSiteId: null,
                StartTime: moment(),
                EndTime: moment(),
                Duration: null,
                UnpaidBreakDuration: 30,
                Mon: false,
                Tue: false,
                Wed: false,
                Thu: false,
                Fri: false,
                Sat: false,
                Sun: false,
                IsActive: true,
                ShiftTypeId: 1 // <-- this will now split training and regular shifts
            };

            return model;
        };

        var _getAll = function() {
            return $http.get("api/shifttemplate/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/shifttemplate/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getBySearchCriteria = function(searchCriteriaModel) {
            return $http.post("api/shifttemplate/getbysearchcriteria", searchCriteriaModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getForSite = function(siteId) {
            return $http.get("api/shifttemplate/getforsite/" + siteId).then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        }

        var _create = function(shiftTemplateModel) {
            return $http.post("api/shifttemplate/create", shiftTemplateModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _createBatch = function(shiftTemplateModelCollection) {
            return $http.post("api/shifttemplate/createbatch", shiftTemplateModelCollection).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(shiftTemplateModel) {
            return $http.post("api/shifttemplate/update", shiftTemplateModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _delete = function(id) {
            return $http.put("api/shifttemplate/delete/" + id).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };


        shiftTemplateServiceFactory.returnNewShiftTemplateModel = _returnNewShiftTemplateModel;

        shiftTemplateServiceFactory.getAll = _getAll;
        shiftTemplateServiceFactory.getById = _getById;
        shiftTemplateServiceFactory.getBySearchCriteria = _getBySearchCriteria;
        shiftTemplateServiceFactory.getForSite = _getForSite;
        shiftTemplateServiceFactory.create = _create;
        shiftTemplateServiceFactory.createBatch = _createBatch;
        shiftTemplateServiceFactory.update = _update;
        shiftTemplateServiceFactory.delete = _delete;

        return shiftTemplateServiceFactory;
    }
]);