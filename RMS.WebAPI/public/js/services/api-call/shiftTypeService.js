"use strict";

var app = angular.module("services");
app.factory("shiftTypeService", [
    "$http",
    function ($http) {
        var shiftServiceFactory = {};

        var _generateNewShiftTypeModel = function () {
            var model = {
                IsActive: true,
                Name: null
            };

            return model;
        };

        var _getAll = function () {
            return $http.get("api/shifttype/getall").then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };


        shiftServiceFactory.generateNewShiftTypeModel = _generateNewShiftTypeModel;
        shiftServiceFactory.getAll = _getAll;

        return shiftServiceFactory;
    }
])