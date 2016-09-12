"use strict";

var app = angular.module("services");
app.factory("resourceService", [
    "$http",
    function ($http) {
        var resourceServiceFactory = {};

        var _generateNewResourceModel = function() {
            var resourceModel = {
                Name: null,
                BaseRate: null,
                IsActive: true
            };

            return resourceModel;
        };

        var _getAll = function() {
            return $http.get("api/resource/getall").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(id) {
            return $http.get("api/resource/" + id).then(function(response) {
                return response.data;
            }, function(err) {
                return err.data.message;
            });
        };

        var _create = function(resourceModel) {
            return $http.post("api/resource/create", resourceModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(resourceModel) {
            return $http.put("api/resource/update", resourceModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        resourceServiceFactory.generateNewResourceModel = _generateNewResourceModel;
        resourceServiceFactory.getAll = _getAll;
        resourceServiceFactory.getById = _getById;
        resourceServiceFactory.create = _create;
        resourceServiceFactory.update = _update;

        return resourceServiceFactory;
    }
]);