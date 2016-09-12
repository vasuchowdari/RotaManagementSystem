"use strict";

var app = angular.module("services");
app.factory("siteService", [
    "$http",
    function ($http) {
        var siteFactory = {};

        var _generateNewSiteModel = function () {
            var siteModel = {
                Name: null,
                IsActive: true
            };

            return siteModel;
        };

        var _getAll = function () {
            return $http.get("api/site/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function (id) {
            return $http.get("api/site/" + id).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        var _getSitesForCompany = function(companyId) {
            return $http.get("api/site/company/" + companyId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _create = function (siteModel) {
            return $http.post("api/site/create", siteModel).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        var _update = function (siteModel) {
            return $http.put("api/site/update", siteModel).then(function (response) {
                return response.data;
            }, function (err) {
                return err.data.message;
            });
        };

        siteFactory.generateNewSiteModel = _generateNewSiteModel;
        siteFactory.getAll = _getAll;
        siteFactory.getById = _getById;
        siteFactory.getSitesForCompany = _getSitesForCompany;
        siteFactory.create = _create;
        siteFactory.update = _update;

        return siteFactory;
    }
]);